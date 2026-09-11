---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
---

# M5Direction profile schema

Runtime file: unity/Unknown/Assets/_Project/Resources/M5Direction.asset.
Type: Tide.Presentation.M5DirectionProfile, ScriptableObject.
Owner: game-systems-designer. This is presentation configuration; it is not player-save data or gameplay tuning.

| Field | Type | Current contract |
|---|---|---|
| runtimeApproved | bool | true under RFC-CX-007 for internal prototype use; ordinary launch needs no diagnostic override |
| openingImage | Texture2D nullable | imported original intro-m5-r03 PNG; null falls back to ink and same controls |
| sectionSurface | Texture2D nullable | imported original m5-direction-surface-r01 PNG; null uses flat ink |
| firstShotSeconds | float |3.125, the measured intro hard-cut time |
| secondShotSeconds | float |2.875, native total6.0 seconds |
| firstTitle / firstCaption | string | public t0-b1 context only; no inferred clue/result |
| secondTitle / secondCaption | string | process guidance only |
| observeTitle / trialTitle / recordTitle | string | spatial group descriptions, no correctness recommendation |
| recordPreparation | string | candidate preparation wording; acceptance always comes from journal/save state |

The profile is loaded through Resources/M5Direction. Optional fields do not mutate saves or publish evidence on failure. No field is serialized into save.json, and save schema remainsv3. Imported PNG originals and .meta identities are recorded in the owned-source manifest and provider provenance. Approval changes must cite the director's native review; resource import deliberately resets approvalfalse.
