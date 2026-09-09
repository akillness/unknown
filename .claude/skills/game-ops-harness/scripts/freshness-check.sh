#!/usr/bin/env bash
# freshness-check.sh — G8 measurement for the game-ops harness.
# Usage: freshness-check.sh [repo-root] [cycle-start YYYY-MM-DD]
# Exit 0 = fresh, 2 = findings, 1 = error. Prints findings to stdout as `kind<TAB>path<TAB>detail`.
set -uo pipefail
ROOT="${1:-$(pwd)}"
SINCE="${2:-}"
CUR="$ROOT/_workspace/current"
ARC="$ROOT/_workspace/archive"
[ -d "$CUR" ] || { echo "error: $CUR not found" >&2; exit 1; }
FINDINGS=0
finding() { printf '%s\t%s\t%s\n' "$1" "$2" "$3"; FINDINGS=$((FINDINGS+1)); }
fm() { awk 'NR==1&&$0!="---"{exit} NR>1&&$0=="---"{exit} NR>1' "$1"; }
fmval() { fm "$1" | awk -v k="$2" -F': *' '$1==k{sub(/^[^:]*: */,""); print; exit}'; }

SEENLIST="$(mktemp)"; trap 'rm -f "$SEENLIST"' EXIT
while IFS= read -r -d '' f; do
  rel="${f#$ROOT/}"
  if ! head -1 "$f" | grep -q '^---$'; then finding "no-frontmatter" "$rel" "missing YAML frontmatter"; continue; fi
  st="$(fmval "$f" status)"; up="$(fmval "$f" updated)"; sup="$(fmval "$f" supersedes)"
  [ -z "$st" ] && finding "no-status" "$rel" "status missing"
  [ -z "$up" ] && finding "no-updated" "$rel" "updated missing"
  if [ "$st" = "superseded" ]; then finding "superseded-in-current" "$rel" "superseded file still under current/"; fi
  if [ -n "$sup" ] && [ "$sup" != "null" ]; then
    tgt="$ROOT/${sup#/}"; [ -f "$sup" ] && tgt="$sup"
    if [ ! -f "$tgt" ]; then finding "dangling-supersedes" "$rel" "$sup not found"
    else case "$tgt" in "$ARC"/*) [ "$(fmval "$tgt" status)" = "superseded" ] || finding "archive-not-superseded" "$rel" "$sup status != superseded";;
                          *) finding "supersedes-outside-archive" "$rel" "$sup";; esac; fi
  fi
  if [ -n "$SINCE" ] && [ -n "$up" ] && [[ "$up" < "$SINCE" ]] && [ "$st" = "current" ]; then finding "stale-updated" "$rel" "updated $up < cycle start $SINCE"; fi
  key="${rel#_workspace/current/}"; key="${key%.md}"
  if [ "$st" = "current" ]; then
    prev="$(awk -F'\t' -v k="$key" '$1==k{print $2; exit}' "$SEENLIST")"
    if [ -n "$prev" ]; then finding "duplicate-current" "$rel" "also $prev"; else printf '%s\t%s\n' "$key" "$rel" >> "$SEENLIST"; fi
  fi
done < <(find "$CUR" -type f -name '*.md' -not -name 'README.md' -print0)

echo "freshness: $FINDINGS finding(s) across $(find "$CUR" -type f -name '*.md' | wc -l | tr -d ' ') files" >&2
[ "$FINDINGS" -eq 0 ] && exit 0 || exit 2
