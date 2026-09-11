# Cycle Types

First distinguish preproduction from a released live game. The type fixes entry phase,
required lanes, gates, and how much of the workspace is archived at close.

| Type | Trigger | Entry | Required lanes | Gates | Typical beat |
|---|---|---|---|---|---|
| **preproduction** | new premium game | P0→P1→P2→model→QA | all specialist lanes + PM | D1–D5 documents; G runtime separate | five real cycles when requested |
| **hotfix** | S1/S2 defect, live exploit, crash | P4 (QA pre-seeded) → P3 fix → P4 | qa, systems, + owning lane (balance/motion/economy) | G6, G8 (+G2/G3 if numbers moved) | patch vX.Y.Z within days |
| **balance-patch** | telemetry drift, exploit register, meta stagnation | P2 (balance ↔ economy) | balance, economy, systems (data-only), qa, planner (notes) | G2, G3, G7, G8 | vX.Y within 1–2 weeks |
| **content-update** | roadmap item, event, new feature | P1 | planner, systems, balance, economy, presentation, vfx, animation, motion, modeling, qa (+synopsis/concept if narrative) | G1, G2, G3, G4, G5, G7, G8 | vX.0 monthly |
| **season / expansion** | new region, faction, major system | P2 worldview first | all specialist lanes + PM if pricing changes | G1–G8 | quarterly |

## Phase map

```
P0 sync ─ P1 scope ─ P2 foundation ─ P3 production ─ P4 verification ─ P5 close
            planner   [worldview→synopsis→concept]   [presentation→(modeling∥anim∥motion∥vfx)]   QA loop ≤2   director
                      ∥ [systems↔balance↔economy]    ∥ systems code + receipts
```

## Per-type notes
- **hotfix**: no new content; archive only the replaced artifact(s); retrospective may be a
  single section appended to the current cycle's retrospective.
- **balance-patch**: `patch-deltas.md` + `sink-source-ledger.md` are mandatory outputs;
  `changelog.md` is generated from them.
- **content-update**: every new noun passes through the glossary; presentation spec exists
  before any visual lane starts.
- **season**: retcons allowed only here, with continuity note; full archive of superseded
  lanes; llm-wiki report is mandatory (`wiki/reports/{date}-{game}-season-{n}.md`).

## Re-entry
- Resume: newest `production/task-manifest.md` → first phase with open tasks.
- Emergency: enter at P4 with registers pre-seeded; untouched gates carry forward as `[CARRIED]`.
- Scope growth mid-cycle: director re-intakes as the larger type; the smaller cycle closes
  with what it has (archived), never stretched.

## Premium new-game override
Read `_workspace/current/production/premium-preproduction-contract.md`. Full duration stays NOT-MEASURED until human play. Noncombat winrate/TTK is justified N/A, not PASS; verify evidence reachability/free recovery/base ending completeness instead. Draft canon can change with RFC+archive. Only released canon needs season retcon. Five-cycle requests are not capped by two QA loops.
