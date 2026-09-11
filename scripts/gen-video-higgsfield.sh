#!/usr/bin/env bash
# gen-video-higgsfield.sh — one image-to-video clip via Higgsfield CLI (credits!) + provenance.
# Usage: scripts/gen-video-higgsfield.sh <clip-id> <start.png> <end.png|-> "<prompt>" [duration=5] [model=seedance_2_0_mini]
set -euo pipefail
ID="$1"; START="$2"; END="$3"; PROMPT="$4"; DUR="${5:-5}"; MODEL="${6:-seedance_2_0_mini}"
ROOT="$(cd "$(dirname "$0")/.." && pwd)"; OUT_DIR="$ROOT/assets/generated/video"; mkdir -p "$OUT_DIR"
LOG="$OUT_DIR/$ID.create.json"
EST="$(higgsfield generate cost "$MODEL" --prompt "$PROMPT" --duration "$DUR" --resolution 720p --aspect_ratio 16:9 --generate_audio false --start-image "$START" $( [ "$END" != "-" ] && echo --end-image "$END" ) </dev/null 2>/dev/null | head -1 || echo unknown)"
echo "estimate: $EST"
higgsfield generate create "$MODEL" --prompt "$PROMPT" --duration "$DUR" --resolution 720p --aspect_ratio 16:9 --generate_audio false \
  --start-image "$START" $( [ "$END" != "-" ] && echo --end-image "$END" ) --wait --wait-timeout 20m --wait-interval 10s --json </dev/null > "$LOG" 2>"$OUT_DIR/$ID.stderr.log" || { echo "create failed, see $OUT_DIR/$ID.stderr.log"; exit 3; }
URL="$(grep -oE 'https://[^" ]+\.mp4[^" ]*' "$LOG" | head -1)"
[ -n "$URL" ] || { echo "no mp4 url in $LOG"; exit 4; }
curl -sSL "$URL" -o "$OUT_DIR/$ID.mp4"
SHA="$(shasum -a 256 "$OUT_DIR/$ID.mp4" | cut -d' ' -f1)"
python3 - "$OUT_DIR/provenance.json" "$ID" "$MODEL" "$START" "$END" "$PROMPT" "$DUR" "$EST" "$SHA" "$URL" <<'PY'
import json,sys,os,datetime
prov,aid,model,start,end,prompt,dur,est,sha,url=sys.argv[1:11]
d={"schema":"provenance/v1","assets":[]}
if os.path.exists(prov): d=json.load(open(prov))
d["assets"]=[a for a in d["assets"] if a["id"]!=aid]
d["assets"].append({"id":aid,"file":f"{aid}.mp4","category":"previz-video","tool":"higgsfield CLI","model":model,
 "start_image":os.path.relpath(start,os.path.dirname(prov)),"end_image":None if end=="-" else os.path.relpath(end,os.path.dirname(prov)),
 "prompt":prompt,"duration_s":int(dur),"credits_estimate":est,"source_url":url,"output_sha256":sha,
 "generated_at":datetime.datetime.now(datetime.timezone.utc).isoformat(),"license":"UNVERIFIED (Higgsfield ToS)","runtimeEligible":False,
 "claim":"[OBSERVED] image-to-video previz clip; NOT gameplay"})
json.dump(d,open(prov,"w"),indent=2,ensure_ascii=False)
PY
echo "$OUT_DIR/$ID.mp4"
