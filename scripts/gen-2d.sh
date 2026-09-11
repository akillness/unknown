#!/usr/bin/env bash
# gen-2d.sh — generate one 2D asset with GTI (god-tibo-imagen, Codex backend) and
# record provenance. Never promotes an asset: runtimeEligible stays false until a
# decision-log audit flips it.
#
# Usage: scripts/gen-2d.sh <asset-id> <category> <size> <prompt-file>
#   asset-id   e.g. char-seorin-sheet   (becomes assets/generated/2d/<category>/<asset-id>.png)
#   category   concept|ui|texture|keyart|capsule|readme|previz
#   size       1024x1024 | 1536x1024 | 1024x1536 | 2048x2048 | 2048x1152
#   prompt-file  text file with the full prompt (style-guide compliant)
# Env: GTI_MODEL (default gpt-6-astra — the only model accepted by this Codex login on 2026-09-10)
set -euo pipefail
ID="$1"; CAT="$2"; SIZE="$3"; PF="$4"
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
OUT_DIR="$ROOT/assets/generated/2d/$CAT"; mkdir -p "$OUT_DIR"
OUT="$OUT_DIR/$ID.png"
MODEL="${GTI_MODEL:-gpt-6-astra}"
PROMPT="$(cat "$PF")"
if [ -f "$OUT" ] && [ "${FORCE:-0}" != "1" ]; then echo "skip (exists): $OUT"; exit 0; fi
gti --prompt "$PROMPT" --output "$OUT" --size "$SIZE" --model "$MODEL" 2> >(grep -v WARNING >&2)
SHA="$(shasum -a 256 "$OUT" | cut -d' ' -f1)"
PROV="$OUT_DIR/provenance.json"
python3 - "$PROV" "$ID" "$CAT" "$SIZE" "$MODEL" "$PF" "$SHA" <<'PY'
import json, sys, os, datetime
prov, aid, cat, size, model, pf, sha = sys.argv[1:8]
data = {"schema": "provenance/v1", "assets": []}
if os.path.exists(prov):
    data = json.load(open(prov))
data["assets"] = [a for a in data["assets"] if a["id"] != aid]
data["assets"].append({
    "id": aid, "file": f"{aid}.png", "category": cat, "tool": "god-tibo-imagen(gti)", "requested_size": size, "actual_size": __import__("PIL.Image",fromlist=["Image"]).open(os.path.join(os.path.dirname(prov), f"{aid}.png")).size.__str__().replace("(","").replace(")","").replace(", ","x"),
    "backend": "codex private backend (chatgpt.com)", "model": model, "size": size,
    "prompt_file": os.path.relpath(pf, os.path.dirname(prov)) if os.path.isabs(pf) else pf,
    "prompt_sha256": __import__("hashlib").sha256(open(pf,'rb').read()).hexdigest(),
    "output_sha256": sha, "generated_at": datetime.datetime.now(datetime.timezone.utc).isoformat(),
    "license": "UNVERIFIED (generated; check backend ToS before commercial use)",
    "runtimeEligible": False, "promoted_by": None, "claim": "[OBSERVED] generated image; not gameplay"
})
json.dump(data, open(prov, "w"), indent=2, ensure_ascii=False)
PY
echo "$OUT"
