#!/usr/bin/env python3
"""Add actual_size (WxH from pixels) next to requested `size` in every 2D provenance.json.
Never touches prompts/hashes; idempotent."""
import json, glob, os, sys
from PIL import Image
root = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
n = fixed = 0
for prov in glob.glob(os.path.join(root, "assets/generated/2d/*/provenance.json")):
    d = json.load(open(prov)); base = os.path.dirname(prov)
    for a in d["assets"]:
        p = os.path.join(base, a["file"])
        if not os.path.exists(p): continue
        w, h = Image.open(p).size; n += 1
        actual = f"{w}x{h}"
        if a.get("requested_size") is None: a["requested_size"] = a.get("size")
        a["size"] = actual; a["actual_size"] = actual
        a["size_note"] = "backend ignored --size" if a["requested_size"] != actual else "matches request"
        a["bytes"] = os.path.getsize(p); fixed += 1
    json.dump(d, open(prov, "w"), indent=2, ensure_ascii=False)
print(f"refreshed {fixed}/{n} entries")
