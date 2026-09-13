#!/usr/bin/env python3
"""M21 — glyph:ground measurement over the T0 work-surface body band.

Reads three native window captures of the SAME window geometry (640x432 pt, captured at
1088x734 px, scale 1.7) and reports the same statistics over the SAME pixel rectangles:

  M19 baseline   committed rag-paper work surface
  M20 gate on    r02 archival surface, committed ink body text, no reading ground
  M21 gate on    r02 archival surface + the M21 per-paragraph reading band

Region location is derived, not guessed:
  1. The work-surface panel is the vertical span of the right-hand column where M19 and M20
     disagree across most of the column — i.e. exactly the ground r02 replaced. The gaps inside
     that span are the M19 instrument plates, which are identical in both captures by contract.
  2. The BODY BAND is the high-variance (glyph-bearing) rows of the first changed run, taken from
     the M19 baseline.
  3. The CONTROL region is empty work surface below the plates, carrying no text in any capture.
     It is the "r02 preserved around the band" check: M21 must match M20 there.

These are FLAT sRGB pixel-arithmetic values. They are blind to glyph anti-aliasing, Korean stroke
weight, display gamma and viewing distance. They are NOT an accessibility measurement and this
script claims conformance with no standard.

Usage: measure-glyph-ground.py <m19.png> <m20.png> <m21.png>
"""
import hashlib
import json
import sys

from PIL import Image

LUT = []
for _i in range(256):
    _v = _i / 255.0
    LUT.append(_v / 12.92 if _v <= 0.04045 else ((_v + 0.055) / 1.055) ** 2.4)


def lum(px):
    return 0.2126 * LUT[px[0]] + 0.7152 * LUT[px[1]] + 0.0722 * LUT[px[2]]


def stats(pix, rect):
    xa, ya, xb, yb = rect
    vals = sorted(lum(pix[x, y]) for y in range(ya, yb) for x in range(xa, xb))
    n = len(vals)
    mean = sum(vals) / n
    sd = (sum((v - mean) ** 2 for v in vals) / n) ** 0.5
    k = max(1, int(n * 0.02))
    glyph = sum(vals[:k]) / k                      # darkest 2% = glyph cores
    j = int(n * 0.80)
    ground = sum(vals[j:]) / (n - j)               # brightest 20% = the reading ground
    return {
        "pixels": n,
        "mean_luminance": round(mean, 4),
        "stdev_glyph_signal": round(sd, 4),
        "glyph_core_luminance": round(glyph, 4),
        "ground_luminance": round(ground, 4),
        "glyph_ground_ratio": round((ground + 0.05) / (glyph + 0.05), 2),
    }


def main():
    paths = {"M19_baseline": sys.argv[1], "M20_gate_on": sys.argv[2], "M21_gate_on": sys.argv[3]}
    ims = {k: Image.open(v).convert("RGB") for k, v in paths.items()}
    size = ims["M19_baseline"].size
    for k, im in ims.items():
        if im.size != size:
            raise SystemExit("capture geometry mismatch for %s: %s != %s" % (k, im.size, size))
    w, h = size
    pix = {k: im.load() for k, im in ims.items()}

    # 1. the work-surface panel = where the committed ground was replaced by r02
    x0, x1 = int(w * 0.48), int(w * 0.96)
    span = x1 - x0
    changed = []
    for y in range(h):
        d = sum(1 for x in range(x0, x1) if pix["M19_baseline"][x, y] != pix["M20_gate_on"][x, y])
        changed.append(d > span * 0.5)
    runs, cur = [], None
    for y, c in enumerate(changed):
        if c:
            cur = (y, y + 1) if cur is None else (cur[0], y + 1)
        elif cur:
            runs.append(cur)
            cur = None
    if cur:
        runs.append(cur)
    if not runs:
        raise SystemExit("no work-surface difference found between the M19 and M20 captures")
    panel = (runs[0][0], runs[-1][1])

    # 2. the body band = glyph-bearing rows of the first changed run, per the M19 baseline
    first = runs[0]
    hot = [y for y in range(first[0], first[1])
           if stats(pix["M19_baseline"], (x0, y, x1, y + 1))["stdev_glyph_signal"] > 0.05]
    if not hot:
        raise SystemExit("no glyph rows found in the first changed run")
    body = (x0, min(hot), x1, max(hot) + 1)

    # 3. the control region = empty surface below the plates (no text in any capture)
    last = runs[-1]
    cy0 = last[0] + int((last[1] - last[0]) * 0.20)
    cy1 = last[0] + int((last[1] - last[0]) * 0.80)
    control = (x0, cy0, x1, cy1)

    report = {
        "note": "flat sRGB pixel arithmetic over one native window capture each; NOT an accessibility measurement, no standard claimed",
        "capture_size": [w, h],
        "work_surface_rows": list(panel),
        "m19_m20_changed_runs": [list(r) for r in runs],
        "plate_rows_identical_in_m19_and_m20": [[runs[i][1], runs[i + 1][0]] for i in range(len(runs) - 1)],
        "body_band_rect_xyxy": list(body),
        "control_rect_xyxy": list(control),
        "sources": {k: v.split("/")[-1] for k, v in paths.items()},
        "sha256": {k: hashlib.sha256(open(v, "rb").read()).hexdigest() for k, v in paths.items()},
        "body_band": {k: stats(pix[k], body) for k in paths},
        "empty_surface_control": {k: stats(pix[k], control) for k in paths},
    }
    ctl = report["empty_surface_control"]
    report["r02_preserved_outside_the_band"] = (
        ctl["M21_gate_on"] == ctl["M20_gate_on"]
    )
    print(json.dumps(report, indent=2, ensure_ascii=False))


if __name__ == "__main__":
    main()
