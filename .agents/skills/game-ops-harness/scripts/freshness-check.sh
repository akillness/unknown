#!/usr/bin/env bash
# freshness-check.sh — G8 (freshness) *file-contract* measurement for the harness.
#
# SCOPE (read this before quoting it as a gate result): this script checks the
# frontmatter contract and the current/archive supersedes topology under
# _workspace/current. It does NOT verify mex/llm-wiki/graphify/zg memory_sync
# receipts, so exit 0 is a necessary — not sufficient — condition for G8.
#
# Usage:
#   freshness-check.sh [repo-root] [cycle-start YYYY-MM-DD]
#   freshness-check.sh [--root <repo-root>] [--since YYYY-MM-DD]
#   env: FRESHNESS_SINCE=YYYY-MM-DD (used only when no explicit argument given)
#
# Checks: mandatory frontmatter (updated, cycle, status, owner), status enum,
# updated date format, superseded-in-current, supersedes resolution (absolute,
# file-relative, root-relative) against the canonical archive path, duplicate
# `status: current` per logical artifact, duplicate `artifact_id` (optional
# field, checked only when present), and staleness against an explicit SINCE.
#
# Output: findings on stdout as `kind<TAB>path<TAB>detail`; summary on stderr.
# Exit: 0 = no findings, 2 = findings, 1 = error.
set -uo pipefail

ROOT=""
SINCE=""
SINCE_SET=0
while [ "$#" -gt 0 ]; do
  case "$1" in
    --root)  ROOT="${2:-}"; shift 2 ;;
    --since) SINCE="${2:-}"; SINCE_SET=1; shift 2 ;;
    -h|--help) sed -n '2,24p' "${BASH_SOURCE[0]}" | sed 's/^#\{1,\} \{0,1\}//' >&2; exit 0 ;;
    --) shift; break ;;
    -*) echo "freshness-check: unknown option: $1" >&2; exit 1 ;;
    *)
      if [ -z "$ROOT" ]; then ROOT="$1"
      elif [ "$SINCE_SET" -eq 0 ]; then SINCE="$1"; SINCE_SET=1
      else echo "freshness-check: unexpected argument: $1" >&2; exit 1
      fi
      shift ;;
  esac
done

SCRIPT_DIR="$(cd -P -- "$(dirname -- "${BASH_SOURCE[0]}")" && pwd -P)"
if [ -z "$ROOT" ]; then
  ROOT="$(git -C "$SCRIPT_DIR" rev-parse --show-toplevel 2>/dev/null || true)"
  if [ -z "$ROOT" ]; then ROOT="$(cd -P -- "$SCRIPT_DIR/../../../.." && pwd -P)"; fi
fi
if [ ! -d "$ROOT" ]; then echo "freshness-check: error: repo root not found: $ROOT" >&2; exit 1; fi
ROOT="$(cd -P -- "$ROOT" && pwd -P)"

# Explicit SINCE wins; env fallback only when no argument was supplied.
if [ "$SINCE_SET" -eq 0 ] && [ -n "${FRESHNESS_SINCE:-}" ]; then SINCE="$FRESHNESS_SINCE"; fi
if [ -n "$SINCE" ]; then
  if ! printf '%s' "$SINCE" | grep -qE '^[0-9]{4}-[0-9]{2}-[0-9]{2}$'; then
    echo "freshness-check: error: --since must be YYYY-MM-DD, got '$SINCE'" >&2; exit 1
  fi
fi

CUR="$ROOT/_workspace/current"
ARC="$ROOT/_workspace/archive"
if [ ! -d "$CUR" ]; then echo "freshness-check: error: $CUR not found" >&2; exit 1; fi
ARC_CANON="$ARC"
if [ -d "$ARC" ]; then ARC_CANON="$(cd -P -- "$ARC" && pwd -P)"; fi

FINDINGS=0
finding() { printf '%s\t%s\t%s\n' "$1" "$2" "$3"; FINDINGS=$((FINDINGS + 1)); }

# Canonical absolute path: realpath of the parent + literal basename, so a file
# that exists is normalized without following the final entry.
canon() {
  _c_p="$1"; _c_d="$(dirname "$_c_p")"; _c_b="$(basename "$_c_p")"
  _c_dr="$(cd -P -- "$_c_d" 2>/dev/null && pwd -P)" || return 1
  [ -n "$_c_dr" ] || return 1
  printf '%s/%s\n' "$_c_dr" "$_c_b"
}

fm() { awk 'NR==1{sub(/\r$/,""); if ($0!="---") exit; next} {sub(/\r$/,""); if ($0=="---") exit; print}' "$1"; }
fmval() {
  fm "$1" | awk -v k="$2" '
    index($0, k ":") == 1 {
      v = substr($0, length(k) + 2)
      sub(/^[ \t]+/, "", v); sub(/[ \t]+$/, "", v)
      gsub(/^["\x27]|["\x27]$/, "", v)
      print v; exit
    }'
}

SEENLIST="$(mktemp)"; IDLIST="$(mktemp)"
trap 'rm -f "$SEENLIST" "$IDLIST"' EXIT

TOTAL=0
while IFS= read -r f; do
  [ -n "$f" ] || continue
  TOTAL=$((TOTAL + 1))
  rel="${f#$ROOT/}"

  if ! head -1 "$f" | tr -d '\r' | grep -q '^---[[:space:]]*$'; then
    finding "no-frontmatter" "$rel" "missing YAML frontmatter"
    continue
  fi

  st="$(fmval "$f" status)"
  up="$(fmval "$f" updated)"
  cy="$(fmval "$f" cycle)"
  ow="$(fmval "$f" owner)"
  sup="$(fmval "$f" supersedes)"
  aid="$(fmval "$f" artifact_id)"

  # --- mandatory fields ------------------------------------------------------
  [ -z "$st" ] && finding "no-status"  "$rel" "mandatory field 'status' missing"
  [ -z "$up" ] && finding "no-updated" "$rel" "mandatory field 'updated' missing"
  [ -z "$cy" ] && finding "no-cycle"   "$rel" "mandatory field 'cycle' missing"
  [ -z "$ow" ] && finding "no-owner"   "$rel" "mandatory field 'owner' missing"

  if [ -n "$st" ]; then
    case "$st" in
      current|superseded|draft) : ;;
      *) finding "bad-status" "$rel" "status must be current|superseded|draft, got '$st'" ;;
    esac
  fi
  if [ -n "$up" ] && ! printf '%s' "$up" | grep -qE '^[0-9]{4}-[0-9]{2}-[0-9]{2}$'; then
    finding "bad-updated" "$rel" "updated must be YYYY-MM-DD, got '$up'"
  fi

  if [ "$st" = "superseded" ]; then
    finding "superseded-in-current" "$rel" "superseded artifact still under _workspace/current/"
  fi

  # --- supersedes: normalize to a canonical absolute path before checking ----
  case "$sup" in ""|null|Null|NULL|"~"|none) sup="" ;; esac
  if [ -n "$sup" ]; then
    fdir="$(dirname "$f")"
    tgt=""
    case "$sup" in
      /*)
        if [ -f "$sup" ]; then tgt="$sup"; fi
        ;;
      *)
        # relative values may be file-relative, root-relative, _workspace-relative
        # or current-relative; resolving all four keeps a mis-targeted value a
        # precise `supersedes-outside-archive` finding instead of a vague dangle.
        for cand in "$fdir/$sup" "$ROOT/$sup" "$ROOT/_workspace/$sup" "$CUR/$sup"; do
          if [ -f "$cand" ]; then tgt="$cand"; break; fi
        done
        ;;
    esac
    if [ -z "$tgt" ]; then
      finding "dangling-supersedes" "$rel" "supersedes target not found: $sup"
    else
      tgt_canon="$(canon "$tgt")" || tgt_canon="$tgt"
      case "$tgt_canon" in
        "$ARC_CANON"/*)
          tst="$(fmval "$tgt_canon" status)"
          if [ "$tst" != "superseded" ]; then
            finding "archive-not-superseded" "$rel" "$sup has status '${tst:-<none>}' (expected superseded)"
          fi
          ;;
        *)
          finding "supersedes-outside-archive" "$rel" "$sup resolves to $tgt_canon, outside $ARC_CANON"
          ;;
      esac
    fi
  fi

  # --- explicit cycle-start staleness ---------------------------------------
  if [ -n "$SINCE" ] && [ -n "$up" ] && [ "$st" = "current" ] && [[ "$up" < "$SINCE" ]]; then
    finding "stale-updated" "$rel" "updated $up < cycle start $SINCE"
  fi

  # --- one `status: current` per logical artifact ----------------------------
  key="${rel#_workspace/current/}"; key="${key%.md}"
  if [ "$st" = "current" ]; then
    prev="$(awk -F'\t' -v k="$key" '$1==k{print $2; exit}' "$SEENLIST")"
    if [ -n "$prev" ]; then
      finding "duplicate-current" "$rel" "also $prev"
    else
      printf '%s\t%s\n' "$key" "$rel" >> "$SEENLIST"
    fi
  fi

  # --- optional artifact_id uniqueness (checked only when the field exists) --
  if [ -n "$aid" ]; then
    prevs="$(awk -F'\t' -v k="$aid" '$1==k{print $2; exit}' "$IDLIST")"
    if [ -n "$prevs" ]; then
      finding "duplicate-artifact-id" "$rel" "artifact_id '$aid' also in $prevs"
    else
      printf '%s\t%s\n' "$aid" "$rel" >> "$IDLIST"
    fi
  fi
done <<EOF
$(find "$CUR" -type f -name '*.md' -not -name 'README.md' | LC_ALL=C sort)
EOF

{
  echo "freshness: $FINDINGS finding(s) across $TOTAL markdown artifact(s) under _workspace/current"
  echo "freshness: scope = frontmatter contract + supersedes topology only; memory_sync receipts (mex/llm-wiki/graphify/zg) are NOT verified here"
  if [ -n "$SINCE" ]; then echo "freshness: staleness measured against cycle start $SINCE"; else echo "freshness: no cycle start supplied; staleness not measured"; fi
} >&2

if [ "$FINDINGS" -eq 0 ]; then exit 0; fi
exit 2
