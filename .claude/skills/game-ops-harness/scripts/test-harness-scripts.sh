#!/usr/bin/env bash
# test-harness-scripts.sh — bounded regression tests for archive-cycle.sh,
# freshness-check.sh and the mex-agent identity guard.
#
# Every fixture is built in an isolated temp tree; the live repository is never
# read from or written to by these tests. Nothing is committed or pushed.
#
# Usage: bash test-harness-scripts.sh
#        GAME_OPS_TEST_TMP=/path bash test-harness-scripts.sh
# Callers choose GAME_OPS_TEST_TMP; default is the platform temporary directory.
set -uo pipefail

SCRIPT_DIR="$(cd -P -- "$(dirname -- "${BASH_SOURCE[0]}")" && pwd -P)"
ARCHIVE="$SCRIPT_DIR/archive-cycle.sh"
FRESH="$SCRIPT_DIR/freshness-check.sh"
BOOTSTRAP="$SCRIPT_DIR/bootstrap-workspace.sh"

TMPBASE="${GAME_OPS_TEST_TMP:-${TMPDIR:-/tmp}}"
if [ ! -d "$TMPBASE" ]; then
  echo "test tmp base not found: $TMPBASE (set GAME_OPS_TEST_TMP)" >&2; exit 1
fi
WORK="$(mktemp -d "${TMPBASE%/}/game-ops-tests.XXXXXX")" || { echo "cannot create temp dir"; exit 1; }
trap 'rm -rf "$WORK"' EXIT

PASS=0; FAIL=0; N=0
pass() { N=$((N+1)); PASS=$((PASS+1)); printf 'ok   %-2s %s\n' "$N" "$1"; }
fail() { N=$((N+1)); FAIL=$((FAIL+1)); printf 'FAIL %-2s %s\n     %s\n' "$N" "$1" "${2:-}"; }
check() { if [ "$2" = "$3" ]; then pass "$1"; else fail "$1" "expected [$2] got [$3]"; fi; }

# ---------------------------------------------------------------- fixtures ---
newfix() { # $1 = "git" | "nogit"; echoes a fresh isolated fixture root
  # NOTE: called inside $( ), so a shared counter would not persist - use mktemp.
  local d; d="$(mktemp -d "$WORK/fixXXXXXX")"
  mkdir -p "$d/_workspace/current/production" "$d/_workspace/current/systems" "$d/_workspace/archive"
  if [ "${1:-nogit}" = "git" ]; then
    git -C "$d" init -q
    git -C "$d" config user.email t@example.com
    git -C "$d" config user.name test
  fi
  printf '%s' "$d"
}

art() { # art <file> <status> [supersedes] [updated] [cycle] [owner] [artifact_id]
  local f="$1" st="$2" sup="${3:-null}" up="${4:-2026-09-09}" cy="${5:-20260909-preproduction-c1}" ow="${6:-game-production-director}" aid="${7:-}"
  mkdir -p "$(dirname "$f")"
  {
    echo "---"
    echo "updated: $up"
    echo "cycle: $cy"
    echo "status: $st"
    echo "supersedes: $sup"
    echo "owner: $ow"
    [ -n "$aid" ] && echo "artifact_id: $aid"
    echo "---"
    echo
    echo "# body $(basename "$f")"
  } > "$f"
}

run() { # run <cmd...> ; sets RC and OUT
  OUT="$("$@" 2>&1)"; RC=$?
}

echo "== archive-cycle.sh =="

# 1 positive: tracked markdown is archived, superseded, rename staged, no commit
F="$(newfix git)"
art "$F/_workspace/current/systems/spec.md" current
git -C "$F" add -A >/dev/null 2>&1; git -C "$F" commit -qm base >/dev/null 2>&1
BEFORE_COMMITS="$(git -C "$F" rev-list --count HEAD)"
run bash "$ARCHIVE" --root "$F" 20260909-preproduction-v1 systems/spec.md
DST="$F/_workspace/archive/20260909-preproduction-v1/systems/spec.md"
if [ "$RC" -eq 0 ] && [ -f "$DST" ] && [ ! -e "$F/_workspace/current/systems/spec.md" ] \
   && grep -q '^status: superseded' "$DST"; then
  pass "tracked .md archived + status rewritten + source gone"
else
  fail "tracked .md archived" "rc=$RC out=$OUT"
fi
check "no commit created" "$BEFORE_COMMITS" "$(git -C "$F" rev-list --count HEAD)"
STAGED="$(git -C "$F" diff --cached --name-only | tr '\n' ' ')"
case "$STAGED" in
  *_workspace/archive/20260909-preproduction-v1/systems/spec.md*) pass "destination staged by explicit pathspec" ;;
  *) fail "destination staged" "staged=[$STAGED]" ;;
esac
check "archived file has no unstaged drift" "" "$(git -C "$F" diff --name-only | tr -d '\n')"

# 2 positive: untracked markdown never touches the index
F="$(newfix git)"
art "$F/_workspace/current/production/keep.md" current
git -C "$F" add -A >/dev/null 2>&1; git -C "$F" commit -qm base >/dev/null 2>&1
art "$F/_workspace/current/production/loose.md" current
run bash "$ARCHIVE" --root "$F" 20260909-preproduction-v1 production/loose.md
if [ "$RC" -eq 0 ] && [ -f "$F/_workspace/archive/20260909-preproduction-v1/production/loose.md" ]; then
  pass "untracked .md archived"
else
  fail "untracked .md archived" "rc=$RC out=$OUT"
fi
check "untracked archive leaves index untouched" "" "$(git -C "$F" diff --cached --name-only | tr -d '\n')"

# 3 negative: escape out of current/
F="$(newfix nogit)"
mkdir -p "$F/outside"; art "$F/outside/secret.md" current
run bash "$ARCHIVE" --root "$F" 20260909-preproduction-v1 "$F/outside/secret.md"
if [ "$RC" -eq 2 ] && [ -f "$F/outside/secret.md" ]; then pass "rejects absolute path outside current/"
else fail "rejects outside current/" "rc=$RC out=$OUT"; fi

# 4 negative: traversal
run bash "$ARCHIVE" --root "$F" 20260909-preproduction-v1 "../../outside/secret.md"
if [ "$RC" -eq 2 ] && [ -f "$F/outside/secret.md" ]; then pass "rejects '..' traversal"
else fail "rejects traversal" "rc=$RC out=$OUT"; fi

# 5 negative: symlinked source file
F="$(newfix nogit)"
mkdir -p "$F/outside"; art "$F/outside/secret.md" current
ln -s "$F/outside/secret.md" "$F/_workspace/current/production/link.md"
run bash "$ARCHIVE" --root "$F" 20260909-preproduction-v1 production/link.md
if [ "$RC" -eq 2 ] && [ -L "$F/_workspace/current/production/link.md" ] && [ -f "$F/outside/secret.md" ]; then
  pass "rejects symlinked source file"
else fail "rejects symlinked source" "rc=$RC out=$OUT"; fi

# 6 negative: symlinked directory component
F="$(newfix nogit)"
mkdir -p "$F/outside/lane"; art "$F/outside/lane/doc.md" current
ln -s "$F/outside/lane" "$F/_workspace/current/lane"
run bash "$ARCHIVE" --root "$F" 20260909-preproduction-v1 lane/doc.md
if [ "$RC" -eq 2 ] && [ -f "$F/outside/lane/doc.md" ]; then pass "rejects symlinked path component"
else fail "rejects symlinked component" "rc=$RC out=$OUT"; fi

# 7 negative: missing source is a hard error, not a silent skip
F="$(newfix nogit)"
run bash "$ARCHIVE" --root "$F" 20260909-preproduction-v1 production/nope.md
if [ "$RC" -ne 0 ]; then pass "missing source fails (no silent skip)"
else fail "missing source fails" "rc=$RC out=$OUT"; fi

# 8 negative: never overwrite an existing archive entry
F="$(newfix nogit)"
art "$F/_workspace/current/production/dup.md" current
mkdir -p "$F/_workspace/archive/20260909-preproduction-v1/production"
printf 'ORIGINAL-HISTORY\n' > "$F/_workspace/archive/20260909-preproduction-v1/production/dup.md"
run bash "$ARCHIVE" --root "$F" 20260909-preproduction-v1 production/dup.md
if [ "$RC" -eq 2 ] \
   && [ "$(cat "$F/_workspace/archive/20260909-preproduction-v1/production/dup.md")" = "ORIGINAL-HISTORY" ] \
   && [ -f "$F/_workspace/current/production/dup.md" ]; then
  pass "refuses to overwrite existing archive entry (predecessor immutable)"
else fail "archive immutability" "rc=$RC out=$OUT"; fi
check "source untouched after rejected overwrite" "current" "$(awk -F': ' '/^status:/{print $2; exit}' "$F/_workspace/current/production/dup.md")"

# 9 negative: invalid run-ids
F="$(newfix nogit)"
art "$F/_workspace/current/production/a.md" current
BAD=0
for rid in "../evil" "bad" "2026-09-09-x-v1" "20260909-preproduction" "/abs" "20260909-Preproduction-v1"; do
  run bash "$ARCHIVE" --root "$F" "$rid" production/a.md
  [ "$RC" -eq 2 ] || BAD=$((BAD+1))
done
check "rejects 6 invalid run-ids" "0" "$BAD"
run bash "$ARCHIVE" --root "$F" 20260909-balance-patch-v0.1.0 production/a.md
if [ "$RC" -eq 0 ]; then pass "accepts {YYYYMMDD}-{cycle-type}-{version}"
else fail "accepts valid run-id" "rc=$RC out=$OUT"; fi

# 10 negative: all-or-nothing across multiple paths
F="$(newfix nogit)"
art "$F/_workspace/current/production/good.md" current
run bash "$ARCHIVE" --root "$F" 20260909-preproduction-v1 production/good.md production/missing.md
if [ "$RC" -ne 0 ] && [ -f "$F/_workspace/current/production/good.md" ] \
   && [ ! -d "$F/_workspace/archive/20260909-preproduction-v1" ]; then
  pass "one bad path moves nothing (all-or-nothing)"
else fail "all-or-nothing" "rc=$RC out=$OUT"; fi

# 11 --no-stage must leave the index byte-identical for a TRACKED source.
# Regression: `git mv` stages the rename by itself, so the tracked branch has to
# fall back to a plain `mv` when staging is disabled.
F="$(newfix git)"
art "$F/_workspace/current/systems/s.md" current
art "$F/_workspace/current/production/unrelated.md" current
git -C "$F" add -A >/dev/null 2>&1; git -C "$F" commit -qm base >/dev/null 2>&1
# pre-existing staged work by another session, which must survive untouched
printf 'edited by another session\n' >> "$F/_workspace/current/production/unrelated.md"
git -C "$F" add -- "_workspace/current/production/unrelated.md" >/dev/null 2>&1
CACHED_BEFORE="$(git -C "$F" diff --cached)"
CACHED_NAMES_BEFORE="$(git -C "$F" diff --cached --name-status | tr '\n' ';')"
run bash "$ARCHIVE" --root "$F" --no-stage 20260909-preproduction-v1 systems/s.md
CACHED_AFTER="$(git -C "$F" diff --cached)"
CACHED_NAMES_AFTER="$(git -C "$F" diff --cached --name-status | tr '\n' ';')"
if [ "$RC" -eq 0 ] && [ -f "$F/_workspace/archive/20260909-preproduction-v1/systems/s.md" ] \
   && [ ! -e "$F/_workspace/current/systems/s.md" ]; then
  pass "--no-stage archives a tracked file"
else fail "--no-stage archives tracked file" "rc=$RC out=$OUT"; fi
check "--no-stage leaves full 'git diff --cached' byte-identical" "$CACHED_BEFORE" "$CACHED_AFTER"
check "--no-stage leaves staged name-status identical" "$CACHED_NAMES_BEFORE" "$CACHED_NAMES_AFTER"
case "$CACHED_NAMES_AFTER" in
  *systems/s.md*|*archive/20260909-preproduction-v1*) fail "--no-stage kept the rename out of the index" "staged=[$CACHED_NAMES_AFTER]" ;;
  *) pass "--no-stage kept the rename out of the index" ;;
esac
case "$(git -C "$F" status --porcelain)" in
  *" D _workspace/current/systems/s.md"*) pass "--no-stage reports an UNSTAGED deletion for the operator" ;;
  *) fail "--no-stage unstaged deletion" "$(git -C "$F" status --porcelain | tr '\n' ';')" ;;
esac

# 11b default (staging enabled) still records the rename in the index
F="$(newfix git)"
art "$F/_workspace/current/systems/s.md" current
git -C "$F" add -A >/dev/null 2>&1; git -C "$F" commit -qm base >/dev/null 2>&1
run bash "$ARCHIVE" --root "$F" 20260909-preproduction-v1 systems/s.md
case "$(git -C "$F" diff --cached --name-only | tr '\n' ' ')" in
  *archive/20260909-preproduction-v1/systems/s.md*) pass "default mode still stages the archived path" ;;
  *) fail "default staging" "rc=$RC staged=[$(git -C "$F" diff --cached --name-only | tr '\n' ' ')]" ;;
esac

# 12 no temp files left behind in current/
F="$(newfix nogit)"
art "$F/_workspace/current/production/t.md" current
run bash "$ARCHIVE" --root "$F" 20260909-preproduction-v1 production/t.md
check "no .archive-cycle tmp residue" "0" "$(find "$F/_workspace" -name '.archive-cycle.*' | wc -l | tr -d ' ')"

echo
echo "== freshness-check.sh =="

# 13 positive: clean workspace
F="$(newfix nogit)"
art "$F/_workspace/current/production/a.md" current
art "$F/_workspace/current/systems/b.md" draft
run bash "$FRESH" --root "$F"
check "clean workspace exits 0" "0" "$RC"

# 14 positive: relative supersedes values resolve (the old false positive)
F="$(newfix nogit)"
mkdir -p "$F/_workspace/archive/20260909-preproduction-v1/production"
art "$F/_workspace/archive/20260909-preproduction-v1/production/old.md" superseded
art "$F/_workspace/current/production/new.md" current "_workspace/archive/20260909-preproduction-v1/production/old.md"
run bash "$FRESH" --root "$F"
check "root-relative supersedes accepted" "0" "$RC"
art "$F/_workspace/current/production/new.md" current "../../archive/20260909-preproduction-v1/production/old.md"
run bash "$FRESH" --root "$F"
check "file-relative supersedes accepted" "0" "$RC"

# 15 negative: dangling and non-archive supersedes
art "$F/_workspace/current/production/new.md" current "../../archive/nope/x.md"
run bash "$FRESH" --root "$F"
case "$OUT" in *dangling-supersedes*) pass "dangling supersedes detected" ;; *) fail "dangling supersedes" "$OUT" ;; esac
art "$F/_workspace/current/production/other.md" current
art "$F/_workspace/current/production/new.md" current "_workspace/current/production/other.md"
run bash "$FRESH" --root "$F"
case "$OUT" in *supersedes-outside-archive*) pass "supersedes outside archive detected" ;; *) fail "supersedes outside archive" "$OUT" ;; esac

# 16 negative: mandatory frontmatter fields
F="$(newfix nogit)"
printf -- '---\nupdated: 2026-09-09\nstatus: current\nsupersedes: null\n---\n\nx\n' > "$F/_workspace/current/production/nocycle.md"
run bash "$FRESH" --root "$F"
MISS=0
case "$OUT" in *no-cycle*) : ;; *) MISS=$((MISS+1)) ;; esac
case "$OUT" in *no-owner*) : ;; *) MISS=$((MISS+1)) ;; esac
check "missing cycle+owner both reported" "0" "$MISS"
check "findings exit code" "2" "$RC"

# 17 negative: bad status enum and bad date
F="$(newfix nogit)"
art "$F/_workspace/current/production/s.md" "final"
art "$F/_workspace/current/production/d.md" current null "09/09/2026"
run bash "$FRESH" --root "$F"
case "$OUT" in *bad-status*) pass "invalid status value detected" ;; *) fail "bad status" "$OUT" ;; esac
case "$OUT" in *bad-updated*) pass "invalid updated date detected" ;; *) fail "bad updated" "$OUT" ;; esac

# 18 optional artifact_id duplicate check
F="$(newfix nogit)"
art "$F/_workspace/current/production/x.md" current null 2026-09-09 20260909-preproduction-c1 game-production-director "ART-1"
art "$F/_workspace/current/systems/y.md"    current null 2026-09-09 20260909-preproduction-c1 game-systems-designer   "ART-1"
run bash "$FRESH" --root "$F"
case "$OUT" in *duplicate-artifact-id*) pass "duplicate artifact_id detected" ;; *) fail "duplicate artifact_id" "$OUT" ;; esac
F="$(newfix nogit)"
art "$F/_workspace/current/production/x.md" current
art "$F/_workspace/current/systems/y.md" current
run bash "$FRESH" --root "$F"
check "absent artifact_id is not a finding" "0" "$RC"

# 19 explicit SINCE is honored, and omitted SINCE measures nothing
F="$(newfix nogit)"
art "$F/_workspace/current/production/old.md" current null "2026-01-01"
run bash "$FRESH" --root "$F"
check "no SINCE -> no staleness finding" "0" "$RC"
run bash "$FRESH" --root "$F" --since 2026-09-01
case "$OUT" in *stale-updated*) pass "explicit --since flags stale artifact" ;; *) fail "--since staleness" "$OUT" ;; esac
run bash "$FRESH" "$F" 2026-09-01
case "$OUT" in *stale-updated*) pass "positional since still supported" ;; *) fail "positional since" "$OUT" ;; esac
run bash "$FRESH" --root "$F" --since "not-a-date"
check "malformed since is an error" "1" "$RC"

# 20 superseded artifact left under current/ is a finding
F="$(newfix nogit)"
art "$F/_workspace/current/production/z.md" superseded
run bash "$FRESH" --root "$F"
case "$OUT" in *superseded-in-current*) pass "superseded-in-current detected" ;; *) fail "superseded-in-current" "$OUT" ;; esac

# 21 scope disclaimer is printed (G8 is not fully enforced here)
F="$(newfix nogit)"
art "$F/_workspace/current/production/a.md" current
run bash "$FRESH" --root "$F"
case "$OUT" in *"memory_sync receipts"*) pass "prints G8 scope disclaimer" ;; *) fail "scope disclaimer" "$OUT" ;; esac

echo
echo "== mex-agent identity guard =="

BIN="$WORK/bin"; mkdir -p "$BIN"
cat > "$BIN/mex" <<'TEX'
#!/bin/sh
case "$1" in
  --version) echo "pdfTeX 3.141592653-2.6-1.40.27 (TeX Live 2025/Homebrew)"; echo "kpathsea version 6.4.1"; exit 0 ;;
  --help) echo "Usage: pdftex [OPTION]... [TEXNAME[.tex]] [COMMANDS]"; exit 0 ;;
esac
echo "This is pdfTeX" > texput.log
exit 1
TEX
cat > "$BIN/mex-agent-real" <<'AGENT'
#!/bin/sh
case "$1" in
  --version) echo "mex-agent 0.9.0"; exit 0 ;;
  --help) echo "mex <command>"; echo "  setup   scaffold project memory"; echo "  graph   build code graphs"; exit 0 ;;
esac
exit 0
AGENT
chmod +x "$BIN/mex" "$BIN/mex-agent-real"

. "$SCRIPT_DIR/mex-agent-bin.sh"

( export MEX_AGENT_BIN="$BIN/mex"; resolve_mex_agent ) >/dev/null 2>&1
check "TeX binary rejected via MEX_AGENT_BIN" "1" "$?"
( export MEX_AGENT_BIN="$BIN/mex-agent-real"; resolve_mex_agent ) >/dev/null 2>&1
check "real mex-agent accepted via MEX_AGENT_BIN" "0" "$?"
( export MEX_AGENT_BIN="$WORK/does-not-exist"; resolve_mex_agent ) >/dev/null 2>&1
check "non-executable MEX_AGENT_BIN rejected" "1" "$?"
if [ -x /opt/homebrew/bin/mex ]; then
  ( export MEX_AGENT_BIN=/opt/homebrew/bin/mex; resolve_mex_agent ) >/dev/null 2>&1
  check "installed TeX /opt/homebrew/bin/mex rejected" "1" "$?"
fi

# 22 bootstrap must not execute a TeX `mex` found on PATH
F="$(newfix nogit)"
OUT="$(cd "$F" && env PATH="$BIN:/usr/bin:/bin:/usr/sbin:/sbin" MEX_AGENT_BIN= bash "$BOOTSTRAP" --root "$F" --no-index 2>&1)"; RC=$?
case "$OUT" in *"mex: skipped"*) pass "bootstrap records mex skipped instead of running TeX" ;; *) fail "bootstrap mex skip" "$OUT" ;; esac
check "no texput.log written into fixture" "0" "$(find "$F" -name 'texput.log' | wc -l | tr -d ' ')"
check "bootstrap seeded the workspace" "1" "$([ -f "$F/_workspace/current/production/task-manifest.md" ] && echo 1 || echo 0)"

echo
echo "-------------------------------------------"
echo "tests: $N  passed: $PASS  failed: $FAIL"
echo "fixtures: $WORK (removed on exit)"
[ "$FAIL" -eq 0 ] || exit 1
exit 0
