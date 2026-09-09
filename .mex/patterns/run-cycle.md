---
name: run-cycle
description: Run one game-ops cycle end to end.
last_updated: 2026-09-09
---

# Task: run a cycle

## Steps
1. `session-start.sh "<task>"`; read output.
2. Director writes `intake/production-brief.md` with ONE `cycle_type`; announces next beat.
3. TeamCreate with required lanes (`references/cycle-types.md`); manifest rows via TaskCreate.
4. Phases P1–P4 per SKILL.md Step 4; RFCs for every multi-lane change.
5. QA fills `qa/gate-measurements.md`; director issues verdicts in `production/gate-reviews/`.
6. Close: retrospective → `archive-cycle.sh` → `freshness-check.sh` exit 0 → memory sync (`mex log/check/sync`, vault report, `graphify update .`) → re-derive `CLAUDE.md` if the harness changed.

## Gotchas
- Scope growth → re-intake as larger type; never stretch.
- A gate number without command/session is not a measurement.
