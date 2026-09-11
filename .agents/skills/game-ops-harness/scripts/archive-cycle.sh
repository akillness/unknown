#!/usr/bin/env bash
# archive-cycle.sh — move superseded artifacts from _workspace/current into
# _workspace/archive/<run-id>. The archive is immutable history: this script
# never overwrites an existing archive entry and never edits a file after it has
# landed there.
#
# Usage:
#   archive-cycle.sh [--root <repo-root>] [--no-stage] <run-id> <path>...
#     <path> is relative to _workspace/current; absolute paths are accepted but
#     must still resolve inside _workspace/current.
#
# Guarantees:
#   - run-id must match {YYYYMMDD}-{cycle-type}-{version}; no path separators.
#   - every source must resolve inside _workspace/current: no '..', no escape,
#     no symlinked file and no symlinked path component.
#   - a missing source is a hard error (exit 2), never a silent skip.
#   - an existing archive destination is a hard error (exit 2); history is never
#     overwritten.
#   - all sources are validated before any move: one bad path moves nothing.
#   - 'status: superseded' is written into a temporary copy, atomically renamed
#     over the source, and only then moved into the archive. Nothing is edited
#     after it reaches the archive.
#   - index (default): tracked sources use 'git mv' (two explicit pathspecs) and
#     the destination is re-added by explicit pathspec. Untracked sources use
#     plain 'mv' and never touch the index.
#   - index (--no-stage): NOTHING touches the index, tracked or not. The move is
#     always a plain 'mv', so git reports an unstaged deletion plus an untracked
#     archive file for the operator to stage by hand.
#   - never 'git add -A/.', never commit, never push.
#
# Exit: 0 ok - 1 usage/environment error - 2 validation rejection
set -euo pipefail

SCRIPT_DIR="$(cd -P -- "$(dirname -- "${BASH_SOURCE[0]}")" && pwd -P)"
GUARD="$SCRIPT_DIR/pathguard.mjs"

usage() { sed -n '2,31p' "${BASH_SOURCE[0]}" | sed 's/^#\{1,\} \{0,1\}//' >&2; }
die()    { echo "archive-cycle: error: $*" >&2; exit 1; }
reject() { echo "archive-cycle: reject: $*" >&2; exit 2; }

ROOT=""
STAGE=1
while [ "$#" -gt 0 ]; do
  case "$1" in
    --root)     ROOT="${2:-}"; [ -n "$ROOT" ] || die "--root needs a value"; shift 2 ;;
    --no-stage) STAGE=0; shift ;;
    -h|--help)  usage; exit 0 ;;
    --)         shift; break ;;
    -*)         die "unknown option: $1" ;;
    *)          break ;;
  esac
done

if [ "$#" -lt 2 ]; then usage; exit 1; fi
RUN_ID="$1"; shift

# --- run-id validation (it becomes a directory name, so it must be inert) -----
case "$RUN_ID" in
  */*|*\\*|.*|*..*) reject "run-id must not contain path separators or '..': '$RUN_ID'" ;;
esac
if ! printf '%s' "$RUN_ID" | grep -qE '^[0-9]{8}-[a-z][a-z0-9]*(-[a-z0-9]+)*-[A-Za-z0-9][A-Za-z0-9._-]*$'; then
  reject "run-id must be {YYYYMMDD}-{cycle-type}-{version}, got '$RUN_ID'"
fi

# --- repo root (script is runnable from any cwd) ------------------------------
if [ -z "$ROOT" ]; then
  ROOT="$(git -C "$SCRIPT_DIR" rev-parse --show-toplevel 2>/dev/null || true)"
  if [ -z "$ROOT" ]; then ROOT="$(cd -P -- "$SCRIPT_DIR/../../../.." && pwd -P)"; fi
fi
if [ ! -d "$ROOT" ]; then die "repo root not found: $ROOT"; fi
ROOT="$(cd -P -- "$ROOT" && pwd -P)"
CUR="$ROOT/_workspace/current"
ARC="$ROOT/_workspace/archive/$RUN_ID"
if [ ! -d "$CUR" ]; then die "$CUR not found (run bootstrap-workspace.sh first)"; fi
command -v node >/dev/null 2>&1 || die "node is required for path canonicalization"
if [ ! -f "$GUARD" ]; then die "missing helper: $GUARD"; fi

IS_GIT=0
if git -C "$ROOT" rev-parse --is-inside-work-tree >/dev/null 2>&1; then IS_GIT=1; fi

# --- phase 1: validate every path before touching anything --------------------
SRCS=(); RELS=(); DSTS=()
for p in "$@"; do
  errf="$(mktemp)"
  if guard_ok="$(node "$GUARD" --root "$ROOT" --base "_workspace/current" --path "$p" --require file 2>"$errf")"; then
    rm -f "$errf"
  else
    detail="$(head -1 "$errf" | tr '\t' ' ')"; rm -f "$errf"
    reject "$p: ${detail:-invalid path}"
  fi
  abs="$(printf '%s\n' "$guard_ok" | sed -n '1p')"
  rel="$(printf '%s\n' "$guard_ok" | sed -n '2p')"
  if [ -z "$abs" ] || [ -z "$rel" ]; then reject "$p: path guard returned no result"; fi

  dst="$ARC/$rel"
  if [ -e "$dst" ] || [ -L "$dst" ]; then
    reject "$rel: archive destination already exists (archive is immutable): ${dst#$ROOT/}"
  fi

  i=0
  while [ "$i" -lt "${#DSTS[@]}" ]; do
    if [ "${DSTS[$i]}" = "$dst" ]; then reject "$rel: listed twice in one invocation"; fi
    i=$((i + 1))
  done

  SRCS[${#SRCS[@]}]="$abs"
  RELS[${#RELS[@]}]="$rel"
  DSTS[${#DSTS[@]}]="$dst"
done

# --- phase 2: rewrite into a temp copy, rename over source, then archive ------
rewrite_superseded() { # $1 = source file; rewritten content on stdout
  awk '
    NR == 1 { print; if ($0 !~ /^---[ \t\r]*$/) plain = 1; next }
    plain { print; next }
    !fmdone && $0 ~ /^---[ \t\r]*$/ { if (!seen) print "status: superseded"; fmdone = 1; print; next }
    !fmdone && /^status:[ \t]/ { print "status: superseded"; seen = 1; next }
    { print }
  ' "$1"
}

idx=0
while [ "$idx" -lt "${#SRCS[@]}" ]; do
  src="${SRCS[$idx]}"; rel="${RELS[$idx]}"; dst="${DSTS[$idx]}"
  idx=$((idx + 1))

  mkdir -p "$(dirname "$dst")"

  backup=""
  case "$rel" in
    *.md)
      if head -1 "$src" | tr -d '\r' | grep -q '^---[[:space:]]*$'; then
        backup="$(mktemp)"
        cp -p "$src" "$backup"
        tmp="$(dirname "$src")/.archive-cycle.$$.$(basename "$src").tmp"
        rewrite_superseded "$src" > "$tmp"
        if ! grep -q '^status: superseded' "$tmp"; then
          rm -f "$tmp" "$backup"
          reject "$rel: could not set 'status: superseded' in frontmatter"
        fi
        mv -f "$tmp" "$src"
      fi
      ;;
  esac

  tracked=0
  if [ "$IS_GIT" -eq 1 ] && git -C "$ROOT" ls-files --error-unmatch -- "$src" >/dev/null 2>&1; then
    tracked=1
  fi

  # `git mv` itself stages the rename, so it is only used when staging is wanted.
  # Under --no-stage the move must be a plain `mv` or the index would still be
  # mutated for tracked sources.
  moved=0
  if [ "$tracked" -eq 1 ] && [ "$STAGE" -eq 1 ]; then
    if git -C "$ROOT" mv -- "$src" "$dst" >/dev/null 2>&1; then moved=1; fi
  else
    if mv -- "$src" "$dst" >/dev/null 2>&1; then moved=1; fi
  fi

  if [ "$moved" -ne 1 ]; then
    if [ -n "$backup" ] && [ -f "$backup" ]; then mv -f "$backup" "$src"; fi
    reject "$rel: move into archive failed; source restored"
  fi
  if [ -n "$backup" ]; then rm -f "$backup"; fi

  # Index: only the destination path, only when the source was tracked and
  # staging is enabled. 'git mv' already staged the rename; this records the
  # frontmatter rewrite on the very same pathspec. Untracked sources and every
  # --no-stage run never reach the index.
  if [ "$tracked" -eq 1 ] && [ "$STAGE" -eq 1 ]; then
    git -C "$ROOT" add -- "$dst" >/dev/null 2>&1 || \
      echo "archive-cycle: warn: could not stage ${dst#$ROOT/} (file is archived; stage it manually)" >&2
  fi

  echo "${dst#$ROOT/}"
done
