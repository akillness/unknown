#!/usr/bin/env python3
"""Tileability QA for concept-first-m7 texture sets.

For each material folder's basecolor.png:
  1. Seam check: roll 50% in X/Y, save ~200px strips centred on the wrap seam
     (qa/seam-x.png, qa/seam-y.png) and compute mean absolute pixel delta
     across the wrap edge (seamDeltaX / seamDeltaY, 0-255 scale).
  2. Tiling preview: qa/tiled-2x2.png (2x2 tile, downscaled to ~1024px).
  3. Repeat frequency: FFT autocorrelation on grayscale; strongest off-centre
     peak spacing in px (repeatPeakPx), null if no peak > 0.35 outside a 32px
     centre exclusion.
  4. Derived channel sanity: every non-basecolor png must match basecolor dims.

Outputs (candidates only, runtimeEligible:false semantics):
  assets/generated/2d/texture/m7-tiling-qa-report.json
  assets/generated/2d/texture/m7-tiling-qa-report.md

Never touches basecolor/derived textures or provenance.json.
"""

import json
import sys
from pathlib import Path

import numpy as np
from PIL import Image

ROOT = Path(__file__).resolve().parent.parent
TEXTURE_ROOT = ROOT / "assets/generated/2d/texture"
MATERIALS = ["bronze", "perforated-steel", "salt-concrete", "salt-crystal", "rag-paper"]

STRIP_WIDTH = 200          # px width of seam strip crops
CENTRE_EXCLUSION = 32      # px radius excluded around zero-lag in autocorrelation
PEAK_THRESHOLD = 0.35      # min normalized correlation for a repeat peak
PREVIEW_MAX = 1024         # max edge of tiled-2x2 preview


def seam_delta_x(arr: np.ndarray) -> float:
    """Mean abs delta across the horizontal wrap edge (last col vs first col)."""
    return float(np.mean(np.abs(arr[:, -1, ...].astype(np.int16) - arr[:, 0, ...].astype(np.int16))))


def seam_delta_y(arr: np.ndarray) -> float:
    """Mean abs delta across the vertical wrap edge (last row vs first row)."""
    return float(np.mean(np.abs(arr[-1, :, ...].astype(np.int16) - arr[0, :, ...].astype(np.int16))))


def save_seam_strips(arr: np.ndarray, qa_dir: Path) -> None:
    h, w = arr.shape[:2]
    rolled_x = np.roll(arr, w // 2, axis=1)   # seam is a vertical line at x = w//2
    rolled_y = np.roll(arr, h // 2, axis=0)   # seam is a horizontal line at y = h//2
    half = STRIP_WIDTH // 2
    cx, cy = w // 2, h // 2
    strip_x = rolled_x[:, max(0, cx - half):min(w, cx + half), ...]
    strip_y = rolled_y[max(0, cy - half):min(h, cy + half), :, ...]
    Image.fromarray(strip_x).save(qa_dir / "seam-x.png")
    Image.fromarray(strip_y).save(qa_dir / "seam-y.png")


def save_tiled_preview(img: Image.Image, qa_dir: Path) -> None:
    w, h = img.size
    tiled = Image.new(img.mode, (w * 2, h * 2))
    for ox in (0, w):
        for oy in (0, h):
            tiled.paste(img, (ox, oy))
    scale = PREVIEW_MAX / max(tiled.size)
    if scale < 1.0:
        tiled = tiled.resize(
            (max(1, round(tiled.size[0] * scale)), max(1, round(tiled.size[1] * scale))),
            Image.LANCZOS,
        )
    tiled.save(qa_dir / "tiled-2x2.png")


def repeat_peak_px(arr: np.ndarray):
    """Strongest off-centre autocorrelation peak spacing (px), or None."""
    gray = np.asarray(Image.fromarray(arr).convert("L"), dtype=np.float64)
    gray -= gray.mean()
    spectrum = np.fft.fft2(gray)
    ac = np.fft.ifft2(np.abs(spectrum) ** 2).real
    zero_lag = ac[0, 0]
    if zero_lag <= 0:
        return None, None
    ac = np.fft.fftshift(ac / zero_lag)
    h, w = ac.shape
    cy, cx = h // 2, w // 2
    yy, xx = np.mgrid[0:h, 0:w]
    dist = np.hypot(yy - cy, xx - cx)
    masked = np.where(dist > CENTRE_EXCLUSION, ac, -np.inf)
    peak_idx = np.unravel_index(np.argmax(masked), masked.shape)
    peak_val = float(masked[peak_idx])
    if peak_val <= PEAK_THRESHOLD:
        return None, round(peak_val, 4)
    spacing = float(np.hypot(peak_idx[0] - cy, peak_idx[1] - cx))
    return round(spacing, 1), round(peak_val, 4)


def verdict_for(dx: float, dy: float) -> str:
    if dx <= 8 and dy <= 8:
        return "tileable-candidate"
    if dx <= 20 and dy <= 20:
        return "needs-edge-blend"
    return "not-tileable"


def main() -> int:
    materials = {}
    observations = []
    artifacts = []
    failures = []

    for mat in MATERIALS:
        folder = TEXTURE_ROOT / f"m7-{mat}-r01"
        base_path = folder / "basecolor.png"
        if not base_path.is_file():
            failures.append(f"{mat}: basecolor.png missing")
            materials[mat] = {"error": "basecolor.png missing"}
            continue

        img = Image.open(base_path)
        if img.mode not in ("RGB", "L"):
            img = img.convert("RGB")
        arr = np.asarray(img)
        w, h = img.size

        qa_dir = folder / "qa"
        qa_dir.mkdir(exist_ok=True)

        dx = round(seam_delta_x(arr), 2)
        dy = round(seam_delta_y(arr), 2)
        save_seam_strips(arr, qa_dir)
        save_tiled_preview(img, qa_dir)
        peak_px, peak_val = repeat_peak_px(arr)

        mismatches = []
        for png in sorted(folder.glob("*.png")):
            if png.name == "basecolor.png" or png.parent != folder:
                continue
            size = Image.open(png).size
            if size != (w, h):
                mismatches.append(f"{png.name}: {size[0]}x{size[1]} != {w}x{h}")

        verdict = verdict_for(dx, dy)
        rel = folder.relative_to(ROOT)
        materials[mat] = {
            "folder": str(rel),
            "basecolorSize": [w, h],
            "seamDeltaX": dx,
            "seamDeltaY": dy,
            "repeatPeakPx": peak_px,
            "repeatPeakCorrelation": peak_val,
            "derivedChannelMismatches": mismatches,
            "verdict": verdict,
            "runtimeEligible": False,
        }
        artifacts.extend(str(rel / "qa" / n) for n in ("seam-x.png", "seam-y.png", "tiled-2x2.png"))
        observations.append(
            f"[OBSERVED] {mat}: seamDeltaX={dx} seamDeltaY={dy} "
            f"repeatPeakPx={peak_px} (corr={peak_val}) verdict={verdict}"
        )
        if mismatches:
            failures.append(f"{mat}: dimension mismatch {mismatches}")
            observations.append(f"[OBSERVED] {mat}: derived channel mismatches: {mismatches}")
        else:
            observations.append(f"[OBSERVED] {mat}: all derived channels match {w}x{h}")

    status = "fail" if any("missing" in f for f in failures) else ("partial" if failures else "pass")

    report_json = TEXTURE_ROOT / "m7-tiling-qa-report.json"
    report_md = TEXTURE_ROOT / "m7-tiling-qa-report.md"
    artifacts.extend(str(p.relative_to(ROOT)) for p in (report_json, report_md))

    report = {
        "lane": "texture-tiling-qa",
        "status": status,
        "observations": observations,
        "artifacts": artifacts,
        "untested": [
            "in-engine seam behaviour (Unity/runtime sampling)",
            "mip/anisotropy behaviour under minification",
            "normal-map generation (no normal maps exist or were derived)",
            "roughness/height/microdetail visual correctness (dimensions only were checked)",
            "physical tiling scale vs. real-world material dimensions",
        ],
        "materials": materials,
        "verdictRubric": {
            "tileable-candidate": "seamDelta <= 8 on both axes",
            "needs-edge-blend": "seamDelta <= 20 on both axes",
            "not-tileable": "seamDelta > 20 on either axis",
        },
    }
    report_json.write_text(json.dumps(report, indent=2) + "\n")

    lines = [
        "# M7 Texture Tiling QA Report",
        "",
        "Candidates only (`runtimeEligible:false`). Values are [OBSERVED] mean absolute",
        "pixel deltas across the wrap edge (0-255 scale, lower = more tileable).",
        "",
        "| material | seamDeltaX | seamDeltaY | repeatPeakPx | verdict |",
        "| --- | --- | --- | --- | --- |",
    ]
    for mat in MATERIALS:
        m = materials.get(mat, {})
        if "error" in m:
            lines.append(f"| {mat} | - | - | - | ERROR: {m['error']} |")
        else:
            peak = m["repeatPeakPx"] if m["repeatPeakPx"] is not None else "null"
            lines.append(
                f"| {mat} | {m['seamDeltaX']} | {m['seamDeltaY']} | {peak} | {m['verdict']} |"
            )
    lines += [
        "",
        "Verdict rubric: seamDelta <= 8 both axes = tileable-candidate; <= 20 = needs-edge-blend; else not-tileable.",
        "",
        "Untested: in-engine seam behaviour, mip/anisotropy behaviour, normal-map generation,",
        "derived-channel visual correctness (dimensions only), physical tiling scale.",
        "",
    ]
    report_md.write_text("\n".join(lines))

    for obs in observations:
        print(obs)
    print(f"status={status} report={report_json.relative_to(ROOT)}")
    return 0


if __name__ == "__main__":
    sys.exit(main())
