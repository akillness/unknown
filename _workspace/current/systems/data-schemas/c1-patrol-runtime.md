---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
decision: RFC-CX-004
---

# C1 packet and save mapping

Runtime file: `unity/Unknown/Assets/_Project/Resources/C1PatrolContract.json`. Its canonical authoring file is `planning/c1-patrol-contract.json`. Both use packet schema 1; this is distinct from player save schema 2. Runtime code validates scope, prerequisite, clue provenance, initialization, four configurations, valid recovery configuration, and forbidden bypass grants before constructing an immutable simulation definition.

| Packet | Runtime mapping |
| --- | --- |
| `entry.requiredCompletedBeatIds` | `t0-b3` entry guard |
| `observations[].id` | `c1:observed:<id>` fact |
| `preview.initialConfiguration` | `EnterPatrol` preview initialization only |
| `preview.combinations` | Complete four-state truth table, reason keys |
| `journalCondition.id` | `c1:conditionCandidate`, later `c1:journal:<id>` |
| `recovery.freeBypass.targetPreview` | `RecoverPatrol`, with no completion grants |
| `scope.checkpointId` | `checkpoint:cp-c1-b1` on chapter confirmation |

Preview values are lower-case `true`/`false` strings in the existing snapshot values map. Confirmed values use separate `c1:committed:lighting` / `c1:committed:reader` keys. `c1:gateAccess` plus the condition/checkpoint facts is the nonempty completion predicate. The generic journal payload remains `{subjectId,value,otherValue}`; no persisted field is renamed.

The original T0 player fixture in `Tests/Fixtures/T0CompletedV1.json` is copied byte-for-byte from the earlier physical player smoke. Tests migrate an isolated copy and verify identity, original log/progress, replay hash, future-version refusal, and preservation of the actual legacy backup. Gameplay values belong to the canonical packet; the native view asset contains presentation framing only.
