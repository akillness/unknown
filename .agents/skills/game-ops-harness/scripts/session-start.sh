#!/usr/bin/env bash
# session-start.sh — Phase 0 sync for the game-ops harness. Prints the freshest state from every memory layer.
# Usage: session-start.sh "<one-line task>" [repo-root]
set -uo pipefail
SCRIPT_DIR="$(cd -P -- "$(dirname -- "${BASH_SOURCE[0]}")" && pwd -P)"
. "$SCRIPT_DIR/mex-agent-bin.sh"
TASK="${1:-general session}"
ROOT="${2:-}"
if [ -z "$ROOT" ]; then
  ROOT="$(git -C "$SCRIPT_DIR" rev-parse --show-toplevel 2>/dev/null || true)"
  if [ -z "$ROOT" ]; then ROOT="$(cd -P -- "$SCRIPT_DIR/../../../.." && pwd -P)"; fi
fi
if [ ! -d "$ROOT" ]; then echo "session-start: repo root not found: $ROOT" >&2; exit 1; fi
ROOT="$(cd -P -- "$ROOT" && pwd -P)"
CUR="$ROOT/_workspace/current"; VAULT="${GAME_OPS_VAULT:-$HOME/vaults/llm-wiki}"
GAME="$(basename "$ROOT")"
hr(){ printf '\n== %s ==\n' "$1"; }
hr "task"; echo "$TASK"
hr "manifest (open rows)"; [ -f "$CUR/production/task-manifest.md" ] && grep -E '^\|' "$CUR/production/task-manifest.md" | grep -viE '\| *(done|completed) *\|' | head -30 || echo "(no manifest — run bootstrap)"
hr "latest retrospective"; ls -t "$CUR"/retrospectives/*.md 2>/dev/null | head -1 | xargs -I{} sh -c 'echo {}; head -40 "{}"' || echo "(none)"
hr "latest brief"; [ -f "$CUR/intake/production-brief.md" ] && head -25 "$CUR/intake/production-brief.md" || echo "(none)"
hr "open RFCs"; [ -f "$CUR/production/decision-log.md" ] && grep -E '^## RFC-' "$CUR/production/decision-log.md" | tail -10 || echo "(none)"
hr "freshness (G8 file contract only)"; bash "$SCRIPT_DIR/freshness-check.sh" --root "$ROOT" 2>&1 | tail -18
hr "mex"
if ! resolve_mex_agent; then
  echo "mex: skipped — $MEX_SKIP_REASON"
elif [ ! -d "$ROOT/.mex" ]; then
  echo "mex: skipped — .mex/ missing (run bootstrap-workspace.sh)"
else
  echo "mex: using $MEX_BIN"
  (cd "$ROOT" && "$MEX_BIN" graph scope "$TASK" </dev/null 2>/dev/null | head -40; echo "-- timeline --"; "$MEX_BIN" timeline </dev/null 2>/dev/null | tail -10)
fi
hr "graphify"; if command -v graphify >/dev/null && [ -d "$ROOT/graphify-out" ]; then (sed -n "1,/^## Graph Freshness/p" "$ROOT/graphify-out/GRAPH_REPORT.md" | head -25); else echo "(no graphify-out — run graphify update . after first code lands)"; fi
hr "zg"; command -v zg >/dev/null && (cd "$ROOT" && zg status 2>&1 | head -10) || echo "(zg status failed — index busy or missing; see above)"
hr "llm-wiki index entries for $GAME"
if [ -f "$VAULT/index.md" ]; then grep -i -- "$GAME" "$VAULT/index.md" | head -15 || echo "(no entries yet)"; else echo "(vault not found at $VAULT)"; fi
hr "archive"; ls "$ROOT/_workspace/archive" 2>/dev/null | tail -10 || echo "(empty)"
