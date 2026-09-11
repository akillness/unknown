---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
---
# C1 signature runtime schema

[OBSERVED] RFC-CX-005 runtime input: `unity/Unknown/Assets/_Project/Resources/C1SignatureContract.json`, a byte-identical mirror of `planning/c1-signature-contract.json`. Packet validation lives in `C1SignatureData`; simulation rules live in `C1SignatureDefinition`. All gameplay choices and identities come from the packet.

| Contract | Runtime rule |
|---|---|
| `scope.beatId`, `scope.checkpointId` | `c1-b2`, `cp-c1-b2`; entered only after completed `c1-b1` |
| Observation IDs | `c1-b2-c1` original log from `signature-annex`; `c1-b2-c2` plate from `plate-zero` |
| Humidity | Three ordinal choices, initially unset; explicit trial materializes `safe` or `risk` from the configured choice |
| Copies | `c1-b2-copy-1` and `c1-b2-copy-2` have unique origin IDs and common root `signature-annex`; neither is independent proof |
| Lower unresolved region | `c1-b2-region-lower`, second sheet; authored top-left normalized rectangle `(0.12, 0.70, 0.76, 0.22)` remains opaque |
| Comparison | Explicit `c1-b2-plate-zero-band`; proof uses the original log and plate identities |

[OBSERVED] Persisted signature facts and values use the `c1:signature:` namespace inside the existing snapshot maps. Facts include entry, observed/backup identities, separation, explicit copy identities, region mark, comparison and selected proof. Values store selected humidity and materialized trial result. Selection or reset removes the trial result; reset also removes humidity and preserves observations, backups, separation, copies and annotations.

[OBSERVED] Journal command IDs are `EnterSignature`, `ObserveSignature`, `SetSignatureHumidity`, `TrialSignature`, `SeparateSignature`, `CopySignature`, `MarkSignature`, `CompareSignature`, `SelectSignatureProof`, `ResetSignatureTrial` and `ConfirmSignature`. Existing payload fields remain `{subjectId,value,otherValue}`. `ConfirmSignature` records all filing/completion/checkpoint effects as one accepted-save event. Old T0/patrol IDs and persisted field names are unchanged.

[OBSERVED] Runtime save schema is v3. `AtomicSaveStore` preserves exact legacy bytes in versioned backups before migration; replay does not rewrite prior command entries, progress or stable save identity. A missing, malformed, negative or future schema refuses before stale-backup fallback. v4 is the current future-version test case. The actual prior v2 fixture is `unity/Unknown/Assets/_Project/Tests/Fixtures/C1PatrolCompletedV2.json`; both migration tests and native route receipts verify the backup and old command prefix.

[OBSERVED] Presentation-only `Resources/C1SignatureView.asset` points to the approved static reader and blank paper. It does not own gameplay thresholds, source identities, unresolved meaning or save fields. The generated substrate supplies texture only; semantic labels, salt-edge visibility and the permanent lower mask are state-driven UI.

Evidence: `systems/tech-verification/c1-m4-native.md`. This runtime addendum coexists with the broader planned save contract in `data-schemas/save.md`; it does not assert that the entire planned schema is implemented.
