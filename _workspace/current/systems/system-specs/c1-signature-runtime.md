---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
---
# C1 second-beat runtime · RFC-CX-005

[OBSERVED] Runtime input is `unity/Unknown/Assets/_Project/Resources/C1SignatureContract.json`, copied byte-for-byte from `planning/c1-signature-contract.json`. `C1SignatureData` validates beat authority, source/root identities, copy lineage, the unresolved rectangle and atomic-save requirements before attachment. Existing T0 and patrol tables and command identities are retained.

## State and rules

Entry requires completed `c1-b1` and an explicit Continue action. Signature facts and values use the separate `c1:signature:` namespace. Selecting humidity clears only the trial result. A trial requires the first observation and an explicit ordinal choice; the safe configured result permits separation. A risk trial preserves the source. Separation also requires the automatic original backup and is idempotent after success.

Each sheet copy requires its own explicit action after separation. Repeating a copy action cannot increase the count. Automatic first-read backups never count as sheet copies. Both copies retain the `signature-annex` root and cannot be selected as independent proof. Reset removes humidity and trial values while preserving observations, backups, separation, copies and annotations.

Confirmation requires both observations, separation, both explicit copies, the second-sheet lower-region mark, an explicit plate-zero comparison and the selected original log/plate proof pair from different roots. `ConfirmSignature` publishes filed copies, unresolved region, band link, independent citation, completion and checkpoint as one accepted-save event. Immediate submission rejects. Pre-commit checkpoint, candidate journal, async accepted save, cancellation generation and receipt context use the existing App transaction. Undo/redo covers the entire confirmation event.

## Persistence and failure behavior

Save schema is v3. Supported older versions migrate in memory after byte-preserving versioned backup. Missing, malformed, negative or future versions refuse before backup fallback. Tests use an isolated copy of the actual completed v2 native-player save and verify exact original/backup bytes, the old command log, progress, replay hash and stable save identity. Migration does not rewrite prior command hashes or progress. Accepted new exports report the c1-b2 endpoint and both C1 checkpoints.

Unsupported identities reject with unchanged hash/head. Risk trials preserve originals. Reset grants no completion. Failed, pending or cancelled saves expose no committed effects or success receipt. Duplicate confirmation refuses. The data and journal contract is `systems/data-schemas/c1-signature-runtime.md`.

## Presentation and interaction

`SignaturePaperView` receives state-derived flags, the blank texture and authored normalized rectangle; it never writes simulation state. The salt edge disappears on separation. The lower mask stays opaque on originals, copies, comparison and completion. The page-number relationship appears only after observation, using neutral `번호 표기` labels without inventing a numeral. Generated paper contains no clue text, signature identity or hidden reconstruction.

Paper stage height is independent of text scale. At 150%, original/copy labels and the lower mask fit inside the left clipping viewport. Signature surface transitions restore both scroll surfaces to the top. Pointer selection synchronizes keyboard focus without a new render or scroll. Selected humidity and the executed trial result persist in the upper case card while lower controls are focused; no safe-answer guidance appears before trial.

[OBSERVED] Native reader r02 and Higgsfield paper received director review. `C1SignatureView.runtimeApproved` is enabled in the final ordinary-runtime build. All cuts are static, 0 ms. Reader housing is static. No humidity camera animation, authored handwriting, salt dissolve, audible sound or C1-b3 content is included.

## Telemetry, budgets and evidence

Telemetry uses existing command ID, sequence, state hash, idempotency key and successful-receipt count; no remote analytics or personal data are added. The reader import has 4 meshes and 3,920 triangles with four 1K maps. The paper is 1024². These asset counts do not establish GPU/frame-time compliance. Frame rate, GPU budget, human comprehension and fun remain unmeasured.

Native/test/build evidence is `systems/tech-verification/c1-m4-native.md` and `systems/tech-verification/c1-m4/`. [UNGRAPHED] mex-agent is unavailable; the host's TeX mex is deliberately not invoked. zg and graphify retrieval were used without persistent zg-index mutation; graph update closure is recorded in the verification report.
