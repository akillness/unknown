#!/usr/bin/env bash
# bootstrap-workspace.sh — create the 13-lane workspace and initialize the memory stack. Idempotent.
# Usage: bootstrap-workspace.sh [repo-root] [--no-index]
set -uo pipefail
SCRIPT_DIR="$(cd -P -- "$(dirname -- "${BASH_SOURCE[0]}")" && pwd -P)"
. "$SCRIPT_DIR/mex-agent-bin.sh"
ROOT=""; NOINDEX=""
while [ "$#" -gt 0 ]; do
  case "$1" in
    --no-index) NOINDEX="--no-index"; shift ;;
    --root)     ROOT="${2:-}"; shift 2 ;;
    -*)         echo "bootstrap-workspace: unknown option: $1" >&2; exit 1 ;;
    *)          if [ -z "$ROOT" ]; then ROOT="$1"; fi; shift ;;
  esac
done
if [ -z "$ROOT" ]; then
  ROOT="$(git -C "$SCRIPT_DIR" rev-parse --show-toplevel 2>/dev/null || true)"
  if [ -z "$ROOT" ]; then ROOT="$(cd -P -- "$SCRIPT_DIR/../../../.." && pwd -P)"; fi
fi
mkdir -p "$ROOT" || exit 1
ROOT="$(cd -P -- "$ROOT" && pwd -P)"
CUR="$ROOT/_workspace/current"; TODAY="$(date +%F)"
LANES="intake planning planning/feature-specs balance balance/sim-results systems systems/system-specs systems/data-schemas systems/tech-verification systems/ops economy presentation presentation/scene-boards synopsis synopsis/quests synopsis/dialogue worldview worldview/lore concept concept/sheets vfx animation animation/state-machines animation/clip-specs motion modeling modeling/specs qa production production/gate-reviews messages retrospectives"
for l in $LANES; do mkdir -p "$CUR/$l"; done
mkdir -p "$ROOT/_workspace/archive"
touch "$ROOT/_workspace/archive/.gitkeep"
seed() { local f="$1"; shift; [ -f "$f" ] && return; printf -- '---\nupdated: %s\ncycle: bootstrap\nstatus: draft\nsupersedes: null\nowner: %s\n---\n\n%s\n' "$TODAY" "$1" "$2" > "$f"; }
seed "$CUR/production/task-manifest.md" game-production-director "# Task Manifest

| task | owner | phase | artifact | gate | status | beat |
|---|---|---|---|---|---|---|
| intake first cycle | game-production-director | P1 | intake/production-brief.md | – | open | – |"
seed "$CUR/production/decision-log.md" game-production-director "# Decision Log (append-only; RFC-n ids unique)"
seed "$CUR/production/changelog.md" game-production-director "# Changelog"
seed "$CUR/conflicts.md" game-production-director "# Conflicts"
seed "$CUR/intake/production-brief.md" game-production-director "# Production Brief

\`\`\`yaml
cycle_type: content-update   # hotfix | balance-patch | content-update | season
version: v0.1.0
entry_phase: P1
required_lanes: []
main_question: ''
next_beat: ''
source_signal: ''
\`\`\`"
seed "$CUR/qa/gate-measurements.md" game-qa "# Gate Measurements

#g1
#g2
#g3
#g4
#g5
#g6
#g7
#g8"
seed "$CUR/worldview/glossary.md" game-worldview-architect "# Glossary

| term (KO) | term (EN) | definition | first cycle |
|---|---|---|---|"
echo "workspace: $CUR ready"
# ---- memory stack ----
cd "$ROOT" || exit 1
# `mex` on PATH may be TeX Live's pdfTeX driver. Only a probe-validated mex-agent
# binary is ever executed here; nothing is installed and no network is touched.
if resolve_mex_agent; then
  echo "mex: using $MEX_BIN"
  if [ ! -d .mex ]; then
    printf 'claude\nn\n' | "$MEX_BIN" setup >/dev/null 2>&1 && echo "mex: .mex/ scaffolded" || echo "mex: setup failed (run '$MEX_BIN setup' manually)"
  else
    echo "mex: .mex/ exists"
  fi
  "$MEX_BIN" graph </dev/null >/dev/null 2>&1 && echo "mex: graph built" || echo "mex: graph skipped (no supported code yet)"
else
  echo "mex: skipped — $MEX_SKIP_REASON"
fi
if command -v graphify >/dev/null; then graphify update . >/dev/null 2>&1 && echo "graphify: graphify-out/ updated" || echo "graphify: update skipped/failed (empty corpus is fine)"; else echo "graphify: not available (pip install graphifyy)"; fi
if command -v zg >/dev/null && [ "$NOINDEX" != "--no-index" ]; then zg index >/dev/null 2>&1 && echo "zg: index built" || echo "zg: index failed (check 'zg status')"; fi
VAULT="${GAME_OPS_VAULT:-$HOME/vaults/llm-wiki}"; [ -d "$VAULT/wiki" ] && echo "llm-wiki: vault at $VAULT" || echo "llm-wiki: vault missing at $VAULT (bootstrap via llm-wiki skill)"
mkdir -p "$VAULT/wiki/projects/$(basename "$ROOT")" 2>/dev/null && echo "llm-wiki: wiki/projects/$(basename "$ROOT")/ ready"
