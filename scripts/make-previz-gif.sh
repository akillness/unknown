#!/usr/bin/env bash
# make-previz-gif.sh — assemble pre-visualization frames into README GIFs.
# This is PREVIZ (concept frames / greybox renders), NOT gameplay capture.
#
# Usage: scripts/make-previz-gif.sh <frames-glob> <out.gif> [fps] [width]
#   e.g. scripts/make-previz-gif.sh 'assets/generated/2d/previz/*.png' docs/media/previz-cutscene.gif 1.5 960
#   e.g. scripts/make-previz-gif.sh 'assets/generated/3d/renders/turntable/frame_*.png' docs/media/previz-hub-turntable.gif 8 640
set -euo pipefail
GLOB="$1"; OUT="$2"; FPS="${3:-1.5}"; W="${4:-960}"
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
TMP="$(mktemp -d)"; trap 'rm -rf "$TMP"' EXIT
i=0
# sort frames by name so sequence order = manifest order (zero-padded names recommended)
for f in $(ls $GLOB 2>/dev/null | sort); do
  cp "$f" "$TMP/$(printf 'f%04d.png' $i)"; i=$((i+1))
done
[ "$i" -gt 0 ] || { echo "no frames matched: $GLOB" >&2; exit 2; }
mkdir -p "$(dirname "$OUT")"
# two-pass palette for quality; scale keeps aspect; each frame holds 1/FPS seconds
ffmpeg -y -loglevel error -framerate "$FPS" -i "$TMP/f%04d.png" \
  -vf "scale=${W}:-1:flags=lanczos,palettegen=stats_mode=diff" "$TMP/pal.png"
ffmpeg -y -loglevel error -framerate "$FPS" -i "$TMP/f%04d.png" -i "$TMP/pal.png" \
  -lavfi "scale=${W}:-1:flags=lanczos[x];[x][1:v]paletteuse=dither=sierra2_4a" -loop 0 "$OUT"
SZ=$(wc -c <"$OUT"); SHA=$(shasum -a 256 "$OUT" | cut -d' ' -f1)
python3 - "$ROOT/docs/media/provenance.json" "$OUT" "$GLOB" "$i" "$FPS" "$W" "$SZ" "$SHA" <<'PY'
import json, sys, os, datetime
prov, out, glob_, n, fps, w, sz, sha = sys.argv[1:9]
data = {"schema": "provenance/v1", "assets": []}
if os.path.exists(prov): data = json.load(open(prov))
rel = os.path.relpath(out, os.path.dirname(prov))
data["assets"] = [a for a in data["assets"] if a["file"] != rel]
data["assets"].append({"id": os.path.splitext(os.path.basename(out))[0], "file": rel, "category": "previz-gif",
  "tool": "ffmpeg (palettegen/paletteuse)", "frames_glob": glob_, "frame_count": int(n), "fps": float(fps), "width": int(w),
  "bytes": int(sz), "output_sha256": sha, "generated_at": datetime.datetime.now(datetime.timezone.utc).isoformat(),
  "runtimeEligible": False, "claim": "[OBSERVED] pre-visualization assembled from generated frames; NOT gameplay capture"})
json.dump(data, open(prov, "w"), indent=2, ensure_ascii=False)
PY
echo "$OUT ($i frames, ${SZ} bytes)"
