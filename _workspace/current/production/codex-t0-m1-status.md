---
updated: 2026-09-10
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-production-director
---

# Codex T0 M1 implementation status

## Scope and authority

[OBSERVED 2026-09-10] The user requested a README update and Git push, followed by game implementation under repository rules. This continues the existing T0 handoff within preproduction; it does not start full campaign production.

[OBSERVED] README/media publication completed at `13a41337e654332949b49ab16eca1945a7c1ec53`; a subsequent README-only update `75102fd` published the verified local M1 outcome and blockers to `origin/main`. The initial 18 published files are exclusively `README.md` and `docs/media/`. Independent review confirmed the initial remote HEAD and relative links; the final push also succeeded. Implementation and other pre-existing changes remain local.

[TARGET] M1 delivers the approved package/assembly boundaries, generated-table receipt verification, immutable simulation state, deterministic command/event reduction, circuit and reader rules, and EditMode acceptance tests. Player UI, atomic save/recovery, keyboard/controller completion, performance capture, and human playtests remain later work. M1 must not be described as a playable or completed T0 slice.

## Ownership

| Work | Owner | State |
|---|---|---|
| README/media publication | director + independent publication review | done |
| Unity code, settings, tests and technical receipts | game-systems-designer | bounded Sim/Data implementation verified; packages and full T0 incomplete |
| Independent contract and implementation review | game-qa | accepted within M1 scope; provenance defect open |
| Integration, graph refresh and final verification | director | source graph updated; cache exclusion refresh blocked by disk space |

The systems lane is the sole writer of Unity source. Existing simultaneous-session changes are preserved. No generated media are promoted to runtime assets.

## Confirmed data blocker

[OBSERVED] Systems and independent QA found that all five generated `systems/data/t0/records.json` entries omit `systemId` and `stationId`. The T0 campaign clues do not supply a stable station mapping. `systems/data-schemas/plates.md` requires those source fields and `systems/system-specs/plate-readout.md` requires them on a citation card.

M1 must reject missing source provenance, preserve `t0-b3` as incomplete, and document the required authoring correction in the handoff RFC inbox. A synthetic test fixture is not evidence that the actual T0 data completes. No gameplay `ReasonCode` may be invented outside the current enum contract.

## Baseline verification

[OBSERVED] `node _workspace/current/planning/validate-campaign.mjs`: 49 checks passed, 0 failed. `--t0 _workspace/current/systems/data/t0`: 5 checks passed, 0 failed. Session-start freshness: 0 findings across 121 Markdown artifacts. These results do not validate runtime source completeness.

[OBSERVED] `mex-agent` is unavailable; session-start correctly skipped it rather than invoking the unrelated TeX `mex`. Existing graphify and zg indexes are available. No zg index creation or rebuild is authorized by this task.

## Completion evidence

[OBSERVED] Unity 6000.5.6f1 native Editor contract runner: 21 checks, 0 failures, process exit 0. The negative compiler fixture using Unity's generated `Tide.Sim.rsp` rejected `UnityEngine` with expected exit 1 / CS0246. These are not NUnit, PlayMode, Player build or human playtest receipts. Original commands, XML and failed/successful logs are in `systems/tech-verification/t0-m1-native.md` and `systems/tech-verification/t0-m1/`; independent review is `qa/t0-m1-review.md`.

[OBSERVED] QA found and verified fixes for citation-window mutation (`C7-F48`) and inadequate random stress coverage (`C7-F49`). Source provenance remains open as `C7-F50` / `handoff/rfc-inbox/RFC-CX-001.md`. `scripts/regen-cycle-ledger.py` parsed 160 canonical rows with 0 dropped; C7 has 1 open S2 and remains `fix-in-progress`. New status cells contain only the state token, so `fail-closed` explanatory text cannot be misparsed as a closed defect.

[OBSERVED] `graphify update .` succeeded and includes 41 Sim nodes, but also indexed Unity Library cache nodes. The tested `.graphifyignore` excludes generated Unity paths and retains authored source. Its follow-up refresh failed with ENOSPC; the prior successful graph is retained. Only graph extraction caches created during this task were removed to recover space. `mex-agent` and a zg index rebuild were not run.

Runtime gates are not promoted by this status document; `qa/gate-measurements.md` remains the gate source of truth. Next implementation work requires approved citation provenance and enough disk space to resolve the five approved Unity packages, followed by the remaining circuit FSM, UI/input and save/recovery work.

## Final integration checks

[OBSERVED 2026-09-10] `freshness-check.sh --root /Users/jangyoung/orca/unknown` returned exit 0 and `0 finding(s) across 127 markdown artifact(s)`. This checks frontmatter and supersedes topology, not runtime gates or complete memory synchronization. `git diff --check` passed for touched tracked integration files. `git ls-remote --heads origin main` and local `HEAD` both returned `75102fd609b638344a7a637adaf11c90b6b53a1d`; the staging area is empty.

[OBSERVED] `zg status` returned exit 0 but reported that the existing index needs update (765/771 files, 4 added / 2 modified / 2 deleted at this snapshot). No manual index creation, rebuild or drop was performed. G8 remains partial.
