---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
---
# C1 signature architecture · RFC-CX-005

[OBSERVED] This is the M4 implementation contract for `c1-b2` only. The existing broader `architecture-contract.md` is preserved as a workspace draft and is not part of this publication.

| Owner | Interface and boundary |
|---|---|
| `Tide.Sim / C1SignatureDefinition` | Receives a validated definition and `PuzzleCommand`; validates identities/readiness, materializes trial outcome, reduces deterministic facts/values. No Unity/UI/file access. |
| `Tide.App / C1SignatureData` | Reads `Resources/C1SignatureContract.json`; validates approved scope, source roots, copy lineage, mask and confirmation invariants before attaching the definition. |
| `Tide.App / C1SignatureGameSession` | Converts explicit player actions into commands and builds the presentation model; confirmation uses the existing checkpoint/candidate/accepted-save boundary. |
| `Tide.Save / JournalSave, AtomicSaveStore` | Serializes v3 journal/snapshot state; preserves older source bytes before migration; validates before fallback; reports accepted save without deciding gameplay readiness. |
| `Tide.UI / SignaturePaperView` | Reads paper texture, the authored opaque rectangle and state flags. It cannot mutate simulation state or reconstruct concealed meaning. |
| `Tide.UI / T0Interface` | Renders text, controls, scroll and pointer/keyboard focus; selection changes no journal state. The App invokes gameplay commands. |

[OBSERVED] The existing seven production assembly boundaries remain unchanged. App coordinates Sim, Save and UI. Renderers consume state; they never write it. The static reader and blank paper are presentation dependencies referenced by `C1SignatureView.asset`. Its runtime approval flag is enabled only after native resource review.

[OBSERVED] Save schema v3 adds signature command identities and `c1:signature:` facts/values without renaming T0/patrol fields or commands. Versioned backups preserve exact prior v1/v2 bytes; old command entries and stable save identity remain intact. Future versions refuse before stale fallback. One accepted confirmation publishes all signature filing, completion and checkpoint effects; failed/pending/cancelled saves publish none.

Contracts: `systems/data-schemas/c1-signature-runtime.md` and `systems/system-specs/c1-signature-runtime.md`. Exact source projection, test/build evidence and limitations: `systems/tech-verification/c1-m4-native.md` and `systems/tech-verification/c1-m4/delivery-source-manifest.json`.
