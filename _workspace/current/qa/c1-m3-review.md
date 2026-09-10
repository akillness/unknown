---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-qa
---

# C1-M3 acceptance and regression review

[OBSERVED] The scoped automated regression review for canonical **c1-b1** is accepted. QA independently executed **17/17** static contract assertions and inspected systems' Unity receipts: **EditMode 27/27 PASS** (9 C1 tests) and **PlayMode 28/28 PASS** (5 C1 tests). QA found one public-command transaction-boundary defect; systems fixed it and its executed regression passed. No finding remains open in this scoped review. Root owns the separate native player smoke and Blender scene review.

Evidence:

- [Current static QA receipt](c1-m3-contract-review-r02.json): input hashes, all 17 assertions and execution time. The initial [15-check receipt](c1-m3-contract-review.json) is retained for the packet before explicit initial branch state was added; it is historical evidence.
- [Independent source/XML review receipt](c1-m3-runtime-review.json): inspected code and XML hashes, timestamps, C1 test names and results. QA parsed and reviewed the results; **systems launched the Unity runs**.
- [EditMode rerun](../systems/tech-verification/c1-m3/results/editmode-3.xml) and [PlayMode run](../systems/tech-verification/c1-m3/results/playmode-1.xml). Earlier failing EditMode evidence remains preserved; the mutually exclusive C1/legacy reducer dispatch was corrected before these passes.

Authority: [campaign C1/c1-b1](../planning/campaign.json), [current C1 contract](../planning/c1-patrol-contract.json), [current metadata](../planning/c1-patrol-contract.meta.md), and [RFC-CX-004](../production/decision-log.md). Planning metadata has status:current and supersedes:null. The packet exactly preserves the canonical objective, action, consequence, completion, recovery, hints, prerequisite, checkpoint and two source observations. Eleven minutes is an author estimate, not measured play time.

## Acceptance matrix

| Case | Required behavior | Observed evidence and limits |
| --- | --- | --- |
| C1-P01 entry | Incomplete T0 cannot enter or confirm C1. Completed T0 exposes Continue; the demo stops at c1-b1. | EditMode guards entry/noncompletion. PlayMode starts from the real prior completed save, takes Continue and reaches the current endpoint. Physical interaction is root's separate smoke. |
| C1-P02 sources | Two exact clue IDs, source types, origins, copiedFrom:null and root origins; no new pressure threshold or author identity. | QA static canonical comparison passes; C1PatrolData guards the two source triples. Runtime observation commands preserve the IDs in journal replay. |
| C1-P03 combinations | Four states, only lighting off / reader on valid; invalid states explain why. | EditMode enumerates all three invalid combinations; successful transaction covers the valid one. Source renders reason keys and text state labels. Visual readability remains root's player check. |
| C1-P04 requirements | Either missing clue or missing acknowledgement blocks the valid branch combination. | Both individual missing-clue cases and missing acknowledgement now pass EditMode checks. |
| C1-P05 preview | Folding and candidate acknowledgement cannot grant access, record the condition, complete the beat or alter applied branches. | EditMode preserves committed-state invariants across fold/recovery; confirmation creates all applied effects together. Autosaving the candidate is allowed and does not itself grant world progress. |
| C1-P06 atomic effects | Current successful save applies branches, access, journal condition, c1-b1 completion and cp-c1-b1 together. | EditMode checks all five effects and exact undo/redo hashes. PlayMode exercises accepted save, pending cancellation and candidate-save failure. Public immediate confirmation is rejected before any mutation. |
| C1-P07 failure / repeat | Cancel, failed save, stale work and repeats cannot partly apply or duplicate progress; retry succeeds once. | PlayMode cancellation suppresses effects/receipt. Failure occurs on the **second BeforeRename**, after the precommit checkpoint, and preserves both in-memory and loaded prior state; retry produces one successful receipt. EditMode rejects repeated confirmation and verifies replay. |
| C1-P08 fold | Each branch folded twice restores preview without cost, time advance or committed-world mutation. | EditMode double-fold roundtrips and state checks pass. No economy fields or clocks are mutated by C1 preview commands. |
| C1-P09 free bypass | Each invalid configuration recovers valid preview without new observations, acknowledgement, access, condition or completion. | EditMode now covers all three invalid starting states and confirms previously observed clue retention plus no-grant facts. |
| C1-P10 undo / redo | All five effects are one history unit; no partial world state. | EditMode exact state-hash equality before/after one undo/redo passes; PlayMode pending undo cancels the unaccepted transaction. |
| C1-P11 reload | Persist accepted branches, access, condition association and cp-c1-b1; restore consistent state. | PlayMode loads the disk save, checks C1 metadata and reconstructs GameSession/GameObject with the same state hash and endpoint. This is **not an OS process restart**. |
| C1-P12 UI / inputs | Keys resolve; state is not color-only; equivalent confirmation actions are reachable. | QA checked all 15 referenced keys; source shows text states. PlayMode injects keyboard and pad for two-step stages and exercises hold confirmation. Setup/action helpers use the interface adapter; this does not prove physical mouse or hardware pad input, full C1 pad traversal, or 150% text visibility. |
| C1-S01 v1 migration | Real completed prior v1 save migrates to v2 with prior bytes/identity/history intact; C1 persists afterward. | EditMode verifies primary and .v1.bak bytes, command log, progress, identity, timestamps and state hash. QA independently verifies fixture SHA **6625ef4e331d893ae142fbd1eabc63e683095194f19e00efe5aa7ff53e394a57** equals the prior physical-player save. PlayMode completes C1 from that fixture and reconstructs from its new save. |
| C1-S02 future refusal | Refuse unsupported version before stale-v1 backup fallback or writes. | EditMode uses a schema-3 primary plus the actual v1 backup and asserts refusal and unchanged bytes for both. Source rejects versions greater than 2 before fallback. |
| C1-R01 prior regression | Preserve T0 command/hash semantics and existing renderer/case-thread work. | The complete discovered EditMode and PlayMode suites pass, including prior T0 tests. QA made no code edits and did not revert or stage renderer/case-thread changes. Final build and physical regression remain root/system evidence. |

## Independent finding closure

| Review ID | Severity | Defect and resolution | Closure evidence |
| --- | --- | --- | --- |
| C1-QA-01 | S3 | Public SubmitImmediate(ConfirmPatrol) applied the live journal before autosave, bypassing accepted-save gating. Systems now rejects it with C1_REQUIRES_ATOMIC_COMMIT before cancellation/mutation. Normal C1 UI already used RequestConfirm. | **closed**: PlayMode PublicImmediateSubmissionCannotBypassAcceptedSaveBoundary passed. It asserts rejected command, unchanged head/state, zero success receipts and no completion under a failed-save injection. Source/XML hashes are in the QA runtime review receipt. |

The finding and acceptance-critical coverage requests were broadcast to director and systems with feedback-requested-by: 2026-09-11. Candidate-save failure coverage was inspected separately from checkpoint failure. No existing defect register or gate measurement was edited by this scoped QA task.

## Evidence boundary

This review does not promote G4, G5 or G6. Human immersion, baseline hardware performance, 11-minute duration, auditory quality and physical gamepad use remain [NOT-MEASURED]. The 17 static assertions are not player tests; injected Input System events and adapter calls are not physical UI evidence. Root's native player smoke, visual approval, final build and freshness/graph/memory integration are separate evidence lanes.
