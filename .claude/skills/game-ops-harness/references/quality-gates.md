# Quality Gates G1–G8

Verdicts: PASS / FIX (≤2 loops) / REDO (previous phase). An open S1 defect, a missing
measurement, or a `[TARGET]`/`[INFERENCE]` where a number is required blocks PASS.
QA measures; director rules; every verdict links `qa/gate-measurements.md#g{n}`.

| Gate | Name | Source lanes | PASS threshold |
|---|---|---|---|
| **G1** | Worldview consistency | worldview, synopsis, concept | `consistency-audit.md`: 0 unresolved violations; 100% of new nouns in glossary; continuity ledger has no open contradictions |
| **G2** | Balance bands | balance, motion | every touched system inside `win_rate_band` / TTK ±tolerance / combo EV ≤ cap in ≥1 sim with seed+command; 0 open exploits ≥S2 |
| **G3** | Economy health | economy, balance | per-currency `sink/source ratio` inside band; `paid_free_winrate_delta ≤ cap`; `inflation ≤ monthly_max`; every new source has a sink |
| **G4** | Presentation & immersion | presentation, vfx, animation, motion | per-scene immersion score ≥ `immersion_target` (QA, ≥3 archetypes); timing numbers verified from capture within ±1 frame at target fps |
| **G5** | Asset & effect budget | modeling, vfx | measured tris/textures/particles ≤ budgets per scene; 0 assets without concept_ref; 0 runtime assets with `runtimeEligible:false` |
| **G6** | Ops stability | systems, motion, qa | p95 frame time ≤ budget; memory soak ≤ budget; input latency ≤ `input_latency_budget_ms`; rollback runbook executed once this cycle; crash-free sessions ≥ 99.5% (live) or 0 S1 (pre-live) |
| **G7** | Feature acceptance & core loop | planner, systems, qa | 100% of `acceptance_criteria` in touched feature specs measured PASS; core loop repeat-rate ≥ target in playtest |
| **G8** | Freshness & memory | director, all | `freshness-check.sh` exit 0 (no duplicate `current`, all `supersedes` resolve to `archive/`, all touched `updated` ≥ cycle start); `memory_sync` receipts present (mex log/check, vault report, graphify if code changed) |

Cycle-type minimums are in `cycle-types.md`. Gates not required by the type may be
`[CARRIED]` from the last cycle that measured them, with the path cited.
