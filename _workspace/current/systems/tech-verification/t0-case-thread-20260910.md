---
updated: 2026-09-10
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
---

# T0 read-only case thread — P1 disclosure correction

[OBSERVED] The source-specific next-action P1 is corrected in the uncommitted T0 increment. Focused RED/GREEN, final full EditMode/PlayMode, BuildMac and diagnostic native verification are complete; all changes remain uncommitted. This is a correction within the existing increment, with no new assets, rules, approvals, runtimeEligible changes, commits or pushes.

## Independent-review correction

[OBSERVED] The current user-supplied independent verdict was **no_ship**: the persistent card selected the authored gap record or remaining required citation, then returned its live action label. After t0-b2 it named **당직실 표준판**, identifying the solution source. A passing earlier test that expected a live label did not establish non-disclosure. The former no-P1/P2 disposition in `t0-case-thread-20260910/independent-review.json` is superseded for this issue. That raw review and every prior passing/failed receipt remain unchanged.

[OBSERVED] The exact previous report is retained as `t0-case-thread-20260910/p1-hint-leak/prior-increment-report.md.txt`. It is historical evidence, not the current behavioral specification. `p1-hint-leak/review-intake.json` records the supplied P1/no_ship finding and its provenance; it does not invent an unavailable original review transcript.

## Smallest runtime correction

- `App/T0GameSession.cs`: `CaseThreadNext()` now selects only localization keys using SavePending and the t0-b1/b2/b3 completion predicates. It has no screen, action, requirement, record, name, medium, stored-window, target-selection or validation-command dependency. Its five outputs are generic reading, alignment, citation pinning, review and save-wait guidance.
- `Resources/T0Strings.json`: add `caseRead` and `casePin`; make `caseAlign`, `caseReview` and `caseCheckPins` generic. All non-case strings are unchanged. The approved four-hour objective remains; next guidance contains no time window, named source pair, input sequence or hidden fact.
- `Tests/PlayMode/T0CaseThreadTests.cs`: add the post-t0-b2 disclosure regression and extend the existing behavior test with negative disclosure assertions through every card phase. Replace obsolete action-label expectations with objective-level expectations. No layout/render/input production code changed.

[OBSERVED] Required-citation progress remains the existing state-derived 0/2, 1/2 or 2/2. Completion remains solely `Simulation.IsComplete(state, "t0-b3")`. Two required pins with incorrect stored endpoints still show 2/2 without completion; preview-window edits do not re-pin. The incomplete 2/2 detail now asks for a generic evidence review. No simulation or save semantics changed.

## Strict TDD and verification receipts

All new paths below are relative to `t0-case-thread-20260910/p1-hint-leak/`. The retained `run-unity.py.txt` records exact argv, UTC timestamps, process exit and source hashes in each `*-command.json`. Full suites have no test/assembly filter.

| Check | Observed result | Receipt |
| --- | --- | --- |
| Focused RED before runtime/string changes | Exit 2, **0/1**. Actual next text **당직실 표준판**, expected **근거를 살펴보고 인용을 고정하세요.** after live t0-b2. Assertion failure, not compilation/setup failure. | `focused-red.xml`, `focused-red.log`, `red-source-proof.json`, `red-T0CaseThreadTests.cs.txt` |
| Focused GREEN | Exit 0, **2/2**, no skips. New regression plus extended existing behavior test. | `focused-green.xml`, `focused-green.log` |
| Full EditMode | Exit 0, **19/19**, no skips. | `editmode-final.xml`, `editmode-final.log` |
| Full PlayMode | Exit 0, **23/23**, no skips; includes all six unchanged resource/occlusion tests. | `playmode-final.xml`, `playmode-final.log` |
| BuildMac | Unity exit 0, **Succeeded, 314141603 bytes**. | `native-work/build-mac-command.json`, `native-work/build-mac.log` |
| Diagnostic native smoke | Final **PASS, 482 assertions, 11 state snapshots, 13 screenshots**. | `native-work/native-2/native-result.json`, `native-work/native-2-launch.json`, `native-work/native-2-player.log` |

[OBSERVED] RED production and string bytes match the P1 intake baseline. The focused regression explicitly completes t0-b2 through existing UI actions and opens the reader before its failing assertion. The extended negative check rejects all authored record IDs/display names, exact live action-label reuse, phase/time-window tokens and prohibited narrative facts. It permits only five literal generic next lines at start, intake, alignment, post-b2 before selecting a source, reader/confirmation, deterministic pending save, one citation, incorrect 2/2, corrected completion, undo/redo and existing overlays. The save-wait phase is held with the existing save-injection seam, released in finally; no runtime test hook was added.

[OBSERVED] The existing long test retains pointer and keyboard paths, default/maximum text-scale containment, persistent 0/2→1/2→2/2 semantics, invalid-endpoint noncompletion, undo/redo, and repeated-render checks of state hash, journal entries/head, loaded record, focus, action IDs, receipt count, disk hashes and zero saves. The focused cases are a subset of the full PlayMode total, not additional suite counts.

## Native and preservation verification

[OBSERVED] The unchanged BuildMac method ran in an isolated APFS clone with the exact final production T0GameSession.cs bytes plus the retained command-gated diagnostic suffix. All **251 production input hashes** match the final sources. The retained bundle `unity/Unknown/Builds/T0-case-thread-p1-hint-leak-diagnostic/Unknown.app` has **315 files** matching its manifest. Production contains no diagnostic class.

[OBSERVED] The first build wrapper exited 1 after Unity BuildMac itself succeeded: the concurrent original-project PlayMode runner briefly added two InitTestScene files to the inventory. No existing input hash changed. After normal test cleanup, the strict 251-file comparison passed without source intervention or rebuild (`native-work/post-build-check-failure.json`, `native-work/retention-after-test-cleanup.json`). The original false strict-check result is retained, not rewritten as a first-pass success.

[OBSERVED] Native attempt 1 failed at the first citation receipt assertion. `native-work/native-1-player.log:5310` records **T0 commit: Disk full** for the task-owned save.json.tmp; the persisted journal stayed at sequence 25 with no citation. This was an actual ENOSPC save failure, not an async timing diagnosis. The result, player log, screenshots and failed save are retained. Only the task-owned build clone was removed after preserving the diagnostic source and proving retained-bundle equality. Attempt 2 used the **same bundle and driver**, with a fresh isolated save directory; no production edit or assertion weakening was made.

[OBSERVED] Final foreground Metal smoke on Apple M2 Pro passed. LaunchServices returned 0; player PID **11138** was observed and then absent. The 11.03-second run stayed within the 300-second external bound. LaunchServices does not expose the direct player exit code, so none is claimed. Exact command and timing are in `native-work/native-2-launch.json`; the driver reported PASS and requested exit. Its final log has zero matches for the recorded exception/commit/autosave/NullReference/MissingComponent scan.

[OBSERVED] The 11 snapshots include start, initial room, intake document, post-b1, post-b2, post-b2 reader before source selection, preparation, confirmation, one durable citation, second preparation and completion. Actual next text at post-b2 and 1/2 is **근거를 살펴보고 인용을 고정하세요.**; at completed 2/2 it is **고정한 근거를 검토하세요.** Parent visually inspected the post-b2 reader, 1/2 and completed 2/2 screenshots: the four card lines are visible and contained. Existing virtual InputSystem keyboard/mouse events and explicitly labeled UI adapters drive the smoke. The visible drawer click, nearer renderer-only blocking, restoration after disabling that renderer, keyboard availability while blocked and repeated-render/save-footprint checks passed. Full physical-device/gamepad-only native coverage is not claimed.

[OBSERVED] Final preservation compares **2297 baseline files**: only the three scoped runtime/string/test files and this report changed; **2293 files are byte-identical**, none missing, no unrelated drift. Every prior case-thread and occlusion receipt is unchanged. UI, renderer-only guard, existing occlusion tests, assets, data, approvals and runtimeEligible inputs are preserved. `source-boundary-proof.json` and `preservation.json` record the comparisons. HEAD and index are unchanged. `fix.diff` is the exact P1 delta against the already-dirty intake baseline, not against HEAD.

[OBSERVED] Independent reviewer `/root/independent_review` found no remaining P1/P2 within this correction and separately verified RED-before-edit hashes, all 55 source hashes in each passing Unity command receipt, 251 build input hashes, 315 retained player hashes, final native card text and baseline preservation. This is a scoped technical review, not a global ship decision or new approval. See `p1-hint-leak/independent-review.json`.

## Memory and claim boundaries

[OBSERVED] Session-start completed. mex scope/check/log are skipped because no identity-verified mex-agent is installed; TeX mex was not used. Existing graph lookup found no CaseThreadNext node, so a scoped graph refresh covers the changed files and directly related simulation/UI/input/occlusion sources. `graph-command.json` retains the exact inputs, command and exit 0; canonical repository/vault graphs and persistent zvec indexes were not rebuilt or overwritten.

[INFERENCE] Automated PlayMode and diagnostic native evidence do not establish human play, physical-device gamepad use, accessibility certification, comprehension, performance or final game gates. No G4/G5/G6/G8 promotion or shipping approval follows from this fix.


## Conventions checklist

1. [OBSERVED] This same-cycle correction and its evidence sidecars carry current frontmatter. The exact prior report and all earlier receipts are retained.
2. [OBSERVED] Final freshness exited **0**, with **0 findings across 462 Markdown artifacts** (`p1-hint-leak/freshness.log`); its scope is metadata/supersedes structure, not final gates or memory freshness.
3. [OBSERVED] Every test/build/native result above has an exact command or recorded driver invocation, timestamp and source/artifact hashes. Failed RED, wrapper inventory and ENOSPC attempts remain distinguishable from passing reruns.
4. [OBSERVED] No new rule, dependency, asset, approval, RFC decision or runtimeEligible change was required. This applies the explicit P1 correction request and preserves the approved brief.
5. [OBSERVED] Scoped graph inputs/output and rationale are retained. The task finding is filed in the vault report; canonical graphs and persistent zvec indexes were not overwritten.
6. [OBSERVED] mex-agent is unavailable, so scope/check/log remain explicitly skipped. No no-drift or full G8 claim is made. No staging, commit or push occurred.
