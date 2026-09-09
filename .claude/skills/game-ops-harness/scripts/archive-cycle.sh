#!/usr/bin/env bash
# archive-cycle.sh — move superseded artifacts from _workspace/current into _workspace/archive/<run-id>.
# Usage: archive-cycle.sh <run-id> <path-under-current>...   (paths relative to _workspace/current or absolute)
# Uses `git mv` when tracked, plain mv otherwise; rewrites status: superseded; prints archive paths.
set -euo pipefail
RUN_ID="${1:?run-id required}"; shift
[ "$#" -ge 1 ] || { echo "usage: archive-cycle.sh <run-id> <path>..." >&2; exit 1; }
ROOT="$(git rev-parse --show-toplevel 2>/dev/null || pwd)"
CUR="$ROOT/_workspace/current"; ARC="$ROOT/_workspace/archive/$RUN_ID"
for p in "$@"; do
  src="$p"; [ -e "$src" ] || src="$CUR/$p"
  [ -e "$src" ] || { echo "skip: $p not found" >&2; continue; }
  rel="${src#$CUR/}"; dst="$ARC/$rel"; mkdir -p "$(dirname "$dst")"
  if git -C "$ROOT" ls-files --error-unmatch "$src" >/dev/null 2>&1; then git -C "$ROOT" mv "$src" "$dst"; else mv "$src" "$dst"; fi
  if [ -f "$dst" ] && head -1 "$dst" | grep -q '^---$'; then
    # rewrite status inside the frontmatter block only
    awk 'NR==1{print;next} !done&&/^---$/{done=1;print;next} !done&&/^status:/{print "status: superseded";next} {print}' "$dst" > "$dst.tmp" && mv "$dst.tmp" "$dst"
    if ! grep -q '^status: superseded' "$dst"; then awk 'NR==1{print;print "status: superseded";next}{print}' "$dst" > "$dst.tmp" && mv "$dst.tmp" "$dst"; fi
    git -C "$ROOT" add "$dst" 2>/dev/null || true
  fi
  echo "${dst#$ROOT/}"
done
