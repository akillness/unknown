---
updated: 2026-09-10
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-qa
---

# T0 M1 independent implementation review

[OBSERVED] The reviewed M1 simulation changes and native editor contract checks pass within the scope below. Both implementation/test findings raised during review are closed. The production-data provenance blocker remains open, and this review does not certify the T0 vertical-slice DoD or any game gate.

## Scope and evidence

| Area | Review method | Result |
|---|---|---|
| `Sim/PuzzleState.cs` | Read collection ownership, transition copies, state-hash ordering and invariant-culture serialization | [OBSERVED] Public collections are copied/read-only; the reducer constructs new state collections; hash inputs are tagged and sorted with ordinal comparison. Native reducer and culture checks pass. |
| `Sim/T0Simulation.cs` | Read Validate, Commit, Preview, Reduce and command replay | [OBSERVED] Rejected validation returns no events; Preview reduces into a separate state; replay reruns commands through validation. Native purity/replay checks pass; citation windows are now captured at commit. |
| `Sim/T0Definition.cs` | Read immutable DTO construction and copy-root resolution | [OBSERVED] Collection inputs are copied; independent pairs require different media and resolved root origins. Native lineage/cycle/orphan checks pass. |
| `Editor/T0Verification.cs`, `Tests/EditMode/T0ContractTests.cs` | Read available assertions and fixture provenance; inspect actual native log and XML | [OBSERVED] 21 native editor contract checks / 0 failures. NUnit execution remains unavailable. QA inspected systems' evidence and did not run Unity concurrently. |

All code paths above are under `unity/Unknown/Assets/_Project/`. The authoritative architecture is `systems/architecture-contract.md`; acceptance requirements are `handoff/codex-unity-brief.md` sections ⑤ and ⑩.

## Findings and disposition

The canonical defect statuses are owned by [qa/defect-register.md §14](defect-register.md#14-m1-코드-검토native-editor-재검증-2026-09-10-c7-착수). Review aliases map to canonical IDs as follows: **QA-M1-01 → C7-F48 (closed)**; **QA-M1-02 → C7-F49 (closed)**; **QA-M1-DATA-01 → C7-F50 (open)**. The table below preserves the review detail and is not a separate source of defect status. C7-F50's owner decision is tracked in [RFC-CX-001](../handoff/rfc-inbox/RFC-CX-001.md).

| ID | Severity | Finding and reproduction | Evidence | Status / owner |
|---|---|---|---|---|
| QA-M1-01 | S2 | With a synthetic record carrying valid provenance, load/read it and cite without selecting a window. The initial implementation accepted the citation. A later SetWindow could satisfy gapEndpointsFixed because it read the mutable current window instead of the citation snapshot. Equal endpoints also lacked the required indeterminate rejection. | Initial source: Validate CiteToBoard lines 48–60, Reduce line 112, gapEndpointsFixed line 158. Fixed `T0Simulation.cs` lines 54–60, 80–82, 121–124, 171–172. Native case `Synthetic_provenance_citation_captures_window_at_commit` passes. | CLOSED: systems fixed and QA rereviewed. Missing/zero-width windows are rejected, editing the viewing window does not rewrite the citation, and only explicit re-citation can change the fixed interval. |
| QA-M1-02 | S3 | The initial T05 loop alternated only Read and ToggleUncovered. It was a 10,000-step stress check, not the random-step test required by T-05. | Fixed `Editor/T0Verification.cs` lines 87–103. Native case `T05_Seeded_10000_steps_preserve_auto_kept_clues` passes. | CLOSED: systems added a test-only deterministic random seed, five valid/invalid command candidates, per-step kept-clue monotonicity and rejected-command hash checks. No RNG was added to production Sim. |
| QA-M1-DATA-01 | S2 | Generated records omit systemId and stationId. Campaign clue origin/media fields do not supply a station identity. Valid source identities require an owner-authored decision; a synthetic fixture is not production provenance. | `systems/data-schemas/plates.md` fields systemId/stationId; `system-specs/plate-readout.md` CiteToBoard; `systems/data/t0/records.json` five rows. Native case `Actual_missing_provenance_blocks_citation_and_b3` passes. | OPEN: owner resolution / regenerated receipt required. Actual data fail closed; t0-b3 remains incomplete. The director confirmed no production JSON changes in this milestone. |

Findings were broadcast to the director and systems owner through the collaboration channel on 2026-09-10; feedback-requested-by: 2026-09-10. Systems retains exclusive implementation and Unity/package execution ownership. At the director's request, QA appended C7-F48–F50 and the M1 revalidation section to the canonical defect register without changing prior defect rows. Gate registers were not changed.

## Explicit M1 exclusions

- The circuit overlay, three-point AnchorOverlay and complete tool state machines are not implemented. M1 executes generated exploration/marking completion predicates and the available circuit marking/evidence commands. A passing t0-b2 predicate is not evidence that the full circuit interaction has shipped.
- UI, keyboard/controller completion, atomic saves, SavePending, persistent undo, T-07 softlock exploration, full runtime asset/localization checks and T0 end-to-end player completion remain outside this milestone.
- Package resolution failed with ENOSPC according to the systems owner. A standalone compiler or native contract-check result must be named accurately and cannot stand in for a successful resolved-package Unity player build or NUnit Test Runner result.
- Human playtesting, performance target verdicts and G4–G7 are not measured by this review.

## Final verification receipt

```yaml
measured:
  observed_utc: 2026-09-10T05:34:19.214Z
  method: independent static rereview plus inspection of systems-owned native Unity editor log and XML
  engine: Unity 6000.5.6f1
  command: /Applications/Unity/Hub/Editor/6000.5.6f1/Unity.app/Contents/MacOS/Unity -batchmode -nographics -projectPath /Users/jangyoung/orca/unknown/unity/Unknown -executeMethod Tide.EditorTools.T0Verification.RunBatch -logFile /tmp/unknown-t0-m1-native-checks-3.log
  log: _workspace/current/systems/tech-verification/t0-m1/logs/native-checks-3.log
  xml: _workspace/current/systems/tech-verification/t0-m1/results/t0-m1-contract-checks.xml
  execution_receipt: _workspace/current/systems/tech-verification/t0-m1-native.md
  suite_name: T0 M1 native editor contract checks
  tests: 21
  failures: 0
  exit_code: 0
  exit_code_source: systems execution receipt reported to director and QA; QA independently read the success marker and XML
  runner_kind: custom native editor contract checks; JUnit-style XML; not NUnit Test Runner
  gameplay_build: NOT-MEASURED
  full_t0_completion: blocked by source provenance and unfinished UI/save/input/tool FSM
```

Observed log marker: `T0_M1_CHECKS tests=21 failures=0; full T0 remains blocked by provenance and unfinished UI/save.`

| Reviewed artifact | SHA-256 |
|---|---|
| `unity/Unknown/Assets/_Project/Sim/PuzzleState.cs` | `96911aeafeaf99488a6a19b0122b852a5abb8484771c4dcb98d08bd46bbfcc83` |
| `unity/Unknown/Assets/_Project/Sim/T0Definition.cs` | `862d17e13da4e8ca71d24ed0e0a70cc30ffa6c020e3c8b231633c4fc46327753` |
| `unity/Unknown/Assets/_Project/Sim/T0Simulation.cs` | `076d1ba55316f520da3054c6976e353a19c3a8d06e5d25e48f8ea9a0b2cabc90` |
| `unity/Unknown/Assets/_Project/Editor/T0Verification.cs` | `a1deb34b4c87a6708f07a6049dc7003f616af72660cba460cd277afaf93b4dbc` |
| `unity/Unknown/Assets/_Project/Tests/EditMode/T0ContractTests.cs` | `18402f6bb379c64ced5605520ce9c50833551e8fcb868a0318553bbea757ffa5` |
| `_workspace/current/systems/data/t0/tables-receipt.json` | `5c586e0e32b1fb7e08d59724dcc697c8edafd73cb3108cfc1a818fba065e6d0c` |
| `unity/Unknown/results/t0-m1-contract-checks.xml` | `87d154b809ad27caab592f0afae152520305a21cd71c4b3051e6700b2ed92b37` |

QA computed these hashes directly from the reviewed files. Reviewed code modification times precede the XML timestamp. The durable log and XML copies above were compared byte-for-byte against the original `/tmp` log and `unity/Unknown/results` XML and match. The systems execution receipt also preserves package failures and the engine-boundary negative compile probe.

[OBSERVED] QA read `systems/tech-verification/t0-m1/logs/engine-boundary-negative.log`: `exit_code=1` and `CS0246` for a fixture referencing `UnityEngine`. The systems receipt identifies this as a compiler invocation using Unity's generated `Tide.Sim.rsp`. It supports the engine-free Sim assembly boundary and is not an additional gameplay test.

This review accepts the bounded M1 simulation foundation; the remaining data blocker prevents acceptance of actual t0-b3 completion. Existing gate measurements were not changed.
