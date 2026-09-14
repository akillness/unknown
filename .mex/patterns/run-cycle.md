---
name: run-cycle
description: Run one game-ops cycle end to end.
last_updated: 2026-09-14
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
- Native Unity work targets `unity/Unknown`, never the repository root. Record the actual build fingerprint, isolated checkpoint and diagnostic flags; an import that closes a resource gate requires native review before reapproval.
- Gameplay capture must include the actual input inside the recording interval. A dispatched input or captured window is not proof that the UI accepted an action: inspect the resulting surface/state. Keep failed attempts and Blender renders distinct from accepted native media; preserve existing user scenes and source outputs before regeneration.
