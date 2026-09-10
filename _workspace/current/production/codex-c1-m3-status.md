---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-production-director
---

# C1 M3 — first patrol beat delivered

[OBSERVED] Implemented only c1-b1, 순찰로의 두 분기, from C1 두 개의 필적. Intake HEAD was 687477d; prior T0 implementation was 68967c3. Preserved and exercised the existing renderer-only occlusion and case-thread work. The remaining three C1 beats and chapter-wide design minutes are outside this increment.

## Implemented behavior

A completed T0 save can explicitly continue into C1. The player observes the duty-log and gate-pressure clues, previews the shared lighting/readout supply, and folds lighting while retaining readout. Both enabled is the authored initial preview. Free bypass repairs preview only. Source-attribution acknowledgement and explicit two-step confirmation precede one accepted save that applies branch state, gate access, journal condition, completed beat and cp-c1-b1 together. The endpoint clearly says the next story is not yet open.

Save schema v2 preserves old T0 command identities and bytes, copies the actual prior v1 save before migration, and rejects future schema versions. A guard prevents the public immediate submission path from bypassing the accepted-save boundary. Candidate-write failure after the precommit checkpoint leaves all chapter effects unpublished; retry commits once.

## Verification

| Evidence | Observed result |
|---|---|
| Unity EditMode | 27/27 PASS |
| Unity PlayMode | 28/28 PASS |
| Independent current static QA | 17/17 PASS |
| macOS standalone | Successful build; 331,066,757 bytes |
| Root CUA native smoke | Real prior v1 save, C1 entry, both observations, invalid routing block, free bypass, acknowledgement, two-step confirmation and endpoint |
| Actual process restart | Exit 0; relaunch/Continue restored C1 endpoint at 150% text and reduced motion |
| Save integrity | v1 backup byte-identical; all 36 prior command entries preserved; 42 final entries with exactly one ConfirmPatrol |
| Frozen source and resource checks | 309 Unity source hashes, 10 Blender output hashes and 17 listed README media hashes verified |
| Preproduction document validator | 514 checks passed; runtime status NOT-MEASURED |

Native test execution belongs to the systems runner; QA and root independently inspected its XML. Root operated the standalone through CUA and examined the actual saves/logs. This is a bounded automated smoke, not a human playtest. Full evidence: [root receipt](../systems/tech-verification/c1-m3/root-verification.json), [systems](../systems/tech-verification/c1-m3-native.md), [independent QA](../qa/c1-m3-review.md).

## Resources and remaining scope

Blender 5.1.2 produced an original static 수문 계통판: 6 meshes, 2,092 triangles, four 1024² maps. Director inspected native diagnostic #3, then approved the exact c1-b1 static adapter under RFC-CX-004. Generation provenance/SUCCESS snapshots remain immutable under generation-receipt/. The UI is authoritative for current switch/readout state; the static prop does not animate a meter. See [operator review](c1-panel-operator-review.md).

Higgsfield stamp audio remains disabled pending auditory review. MuAPI authentication/generation is not established in this increment, so no MuAPI output is claimed. Physical gamepad use, audio quality, GPU performance, fun and full G4/G5 remain unmeasured. Minor future UI polish: the case-thread next-action label can invite confirmation after source acknowledgement while invalid routing is still correctly explained and blocked below.

## Integration and publication

Code graph update completed; receipt is in c1-m3/logs/root-graph-update.log. Document semantic extraction was not rerun. mex-agent is unavailable, so mex graph/check/log were skipped without invoking TeX mex. No zg index was created or rebuilt. A final frontmatter/topology check is recorded separately; it does not prove runtime or knowledge freshness. Publication includes the explicit tested Unity dependency closure, bounded C1 resources/evidence and new production sections. Unrelated existing workspace changes stay outside the commit.

Authored staged source/document whitespace checks passed. Full diff whitespace findings are confined to raw Unity logs/XML and Unity-generated meta/asset/material/prefab YAML; those bytes are preserved for receipt/source integrity.
