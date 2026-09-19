#!/usr/bin/env bash
# qa-loop.sh - one deterministic QA cycle for the Unity slice (RFC-CX-M26-20260918 §QA loop).
#
#   EditMode -> PlayMode -> isolated boot -> macOS QA build -> inventory digest -> summary/triage/ledger.
#
# Scheduled every 3 h by launchd (scripts/launchd/io.github.akillness.unknown.qa-loop.plist, StartInterval 10800);
# also runnable by hand:  scripts/qa-loop.sh [--dry-run]
#
# Boundaries (deliberate): never git add/commit/push, never edits Assets/, never promotes a gate, never calls a
# model. It measures and records. Fixing is a session's job: read _workspace/current/qa/loop/latest-triage.md.
# Skips (recorded, exit 0) when another Unity process has the project open or a previous cycle is still running.
#
# Optional extension point: QA_LOOP_POST_CMD="<command>" runs "<command> <run-dir>" after the ledger row is
# written (e.g. a notifier). Unset by default; the launchd plist does not set it.
set -uo pipefail
# Run from a private copy so editing this file while a cycle is in flight cannot change the running cycle (bash reads
# scripts incrementally; the 2026-09-19T13:39Z row was contaminated exactly that way).
if [ -z "${QA_LOOP_EXEC:-}" ]; then t="$(mktemp /tmp/qa-loop.XXXXXX)"; cp "$0" "$t"; QA_LOOP_EXEC=1 QA_LOOP_SRC="$(cd "$(dirname "$0")" && pwd)/$(basename "$0")" exec /bin/bash "$t" "$@"; fi
UNITY="${UNITY:-/Applications/Unity/Hub/Editor/6000.5.6f1/Unity.app/Contents/MacOS/Unity}"
PY=/opt/homebrew/bin/python3; GIT=/opt/homebrew/bin/git
ROOT="$(cd "$(dirname "$QA_LOOP_SRC")/.." && pwd)"; P="$ROOT/unity/Unknown"
LOOP="$ROOT/_workspace/current/qa/loop"; RUNS="$P/Builds/qa-loop"; mkdir -p "$LOOP" "$RUNS"
RUN_ID="$(date -u +%Y%m%dT%H%M%SZ)"; RUN="$RUNS/$RUN_ID"; mkdir -p "$RUN"
DRY=0; [ "${1:-}" = "--dry-run" ] && DRY=1
log(){ printf '%s %s\n' "$(date -u +%H:%M:%S)" "$*" | tee -a "$RUN/loop.log"; }
report(){ "$PY" "$ROOT/scripts/qa-loop-report.py" "$RUN" "$LOOP" "$@"; }
finish(){ # $1 = skip reason or empty
  report ${1:+--skip "$1"} || log "report failed"
  if [ -n "${QA_LOOP_POST_CMD:-}" ]; then log "post: $QA_LOOP_POST_CMD"; bash -c "$QA_LOOP_POST_CMD \"$RUN\"" >>"$RUN/loop.log" 2>&1 || log "post-cmd failed"; fi
  ls -1dt "$RUNS"/*/ 2>/dev/null | tail -n +9 | xargs -I{} rm -rf {}   # keep the last 8 run folders
  rm -f "$0"; exit 0; }

# --- 0. guards ---------------------------------------------------------------------------------------------
LOCK="$LOOP/.lock"
if mkdir "$LOCK" 2>/dev/null; then echo $$ >"$LOCK/pid"; trap 'rm -rf "$LOCK"' EXIT
else
  if kill -0 "$(cat "$LOCK/pid" 2>/dev/null)" 2>/dev/null; then log "skip: previous cycle still running (pid $(cat "$LOCK/pid"))"; finish "locked"
  else rm -rf "$LOCK"; mkdir "$LOCK"; echo $$ >"$LOCK/pid"; trap 'rm -rf "$LOCK"' EXIT; fi
fi
if pgrep -f "Unity.app/Contents/MacOS/Unity.*-projectPath.*unity/Unknown" >/dev/null || { [ -f "$P/Temp/UnityLockfile" ] && pgrep -f "Unity.app/Contents/MacOS/Unity" >/dev/null; }; then
  log "skip: another Unity process has unity/Unknown open"; finish "editor-open"; fi
[ -x "$UNITY" ] || { log "skip: Unity not found at $UNITY"; finish "no-unity"; }

# --- 1. tree identity --------------------------------------------------------------------------------------
tree(){ ( cd "$ROOT" && "$GIT" rev-parse --short=12 HEAD; "$GIT" status --porcelain | wc -l | tr -d ' '; "$GIT" status --porcelain | shasum -a 256 | cut -c1-16 ) 2>/dev/null; }
tree >"$RUN/git.txt"
log "run $RUN_ID @ $(head -1 "$RUN/git.txt") dirty=$(sed -n 2p "$RUN/git.txt")"
[ $DRY = 1 ] && { log "dry-run: no Unity steps"; finish "dry-run"; }

# --- 2. tests ----------------------------------------------------------------------------------------------
t0=$(date +%s)
"$UNITY" -batchmode -nographics -projectPath "$P" -runTests -testPlatform EditMode -testResults "$RUN/editmode.xml" -logFile "$RUN/editmode.log" >/dev/null 2>&1; echo "editmode exit=$? s=$(( $(date +%s)-t0 ))" >>"$RUN/steps.txt"
t0=$(date +%s)
"$UNITY" -batchmode -projectPath "$P" -runTests -testPlatform PlayMode -assemblyNames Tide.Tests.Play -testResults "$RUN/playmode.xml" -logFile "$RUN/playmode.log" --burst-disable-compilation >/dev/null 2>&1; echo "playmode exit=$? s=$(( $(date +%s)-t0 ))" >>"$RUN/steps.txt"
SAVE="/tmp/unknown-c1-m4-boot-qa-$RUN_ID-$$"; t0=$(date +%s)   # T0BootSceneTests requires the /tmp/unknown-c1-m4-boot prefix AND a not-yet-existing directory
"$UNITY" -batchmode -projectPath "$P" -runTests -testPlatform PlayMode -testFilter Tide.Tests.T0BootSceneTests -testResults "$RUN/boot.xml" -logFile "$RUN/boot.log" --t0-save-dir "$SAVE" >/dev/null 2>&1; echo "boot exit=$? s=$(( $(date +%s)-t0 ))" >>"$RUN/steps.txt"; rm -rf "$SAVE"

# --- 3. build + digest --------------------------------------------------------------------------------------
t0=$(date +%s)
"$UNITY" -batchmode -quit -projectPath "$P" -executeMethod Tide.EditorTools.T0ProjectBuilder.BuildMacQa -logFile "$RUN/build-mac.log" --burst-disable-compilation >/dev/null 2>&1; echo "build exit=$? s=$(( $(date +%s)-t0 ))" >>"$RUN/steps.txt"
grep -E "^T0_MAC_BUILD" "$RUN/build-mac.log" >"$RUN/build.txt" 2>/dev/null || true
tree >"$RUN/git-end.txt"   # a session editing the tree mid-cycle makes this row MIXED, not a clean measurement
log "$(tr '\n' ' ' <"$RUN/steps.txt")"
finish ""
