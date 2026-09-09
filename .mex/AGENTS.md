---
name: agents
description: Always-loaded project anchor. Read this first. Contains project identity, non-negotiables, commands, and pointer to ROUTER.md for full context.
last_updated: 2026-09-09
---

# unknown — game ops harness

## What This Is
A game project operated by a 13-role agent studio (기획·밸런스·시스템·재화·연출·시놉시스·세계관·컨셉·이팩트·에니메이션·모션·모델링·QA + director) whose cycles are updates (hotfix / balance-patch / content-update / season) over one live `_workspace/current/` and a read-only archive.

## Non-Negotiables
- Write artifacts only under `_workspace/current/`; archive via `archive-cycle.sh`, never delete.
- Every artifact carries `updated/cycle/status/supersedes/owner` frontmatter; one `status: current` per logical artifact (G8).
- Code work is search-first: `mex graph scope` → `zg query` → `graphify query` → edit → `graphify update .` → `mex graph && mex check` → `mex log`.
- Tuning numbers live in data tables, never in code; persisted field renames need a migration.
- Rationale that code can't verify goes to the llm-wiki vault (`/Users/jangyoung/vaults/llm-wiki`), facts go here.

## Commands
- Session start: `bash .claude/skills/game-ops-harness/scripts/session-start.sh "<task>"`
- Freshness (G8): `bash .claude/skills/game-ops-harness/scripts/freshness-check.sh . <cycle-start-date>`
- Archive: `bash .claude/skills/game-ops-harness/scripts/archive-cycle.sh <run-id> <path>...`
- Validate harness: `bash ~/.claude/skills/harness/scripts/validate-harness.sh .claude/agents .claude/skills`
- Graphs: `graphify update .` · `mex graph` · `zg index --rebuild`

## Code Graph
The repo is indexed into `.mex/graph.db`. Use `mex graph scope "<task>"` for a compact manifest, `mex graph query <who-calls|what-calls|where-defined> <symbol>` for exact lookups, `mex impact <symbol|file>` before editing a symbol. Treat sources the graph returns as already read. Pair with `zg query` (semantic/exact search) and `graphify query` (affected flows).

## Scaffold Growth
After meaningful work: Ground (what changed) → Record (`ROUTER.md` state + `context/`) → Orient (`patterns/` runbook if recurring) → Write (bump `last_updated`, `mex log` when rationale matters).

## Navigation
Read `ROUTER.md` at the start of every session, then run `session-start.sh`.
