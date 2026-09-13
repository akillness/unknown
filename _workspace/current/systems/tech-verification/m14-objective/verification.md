---
updated: 2026-09-12
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
---

# M14 T0 b1 spoiler-free objective

## Scope

[OBSERVED] The fresh T0 b1 case-thread card previously fell back to the later t0-b3 objective because the authored b1 objective contained record display names. This increment replaces only the canonical b1 objective with a disclosure-clean line and refreshes its generated runtime tables.

## Change

- Canonical field: `planning/campaign.json` → `stages[0].beats[id=t0-b1].objective`
- Rendered copy: `오늘 밤, 무엇이 남을지는 아직 정해지지 않았다.`
- Generated outputs refreshed through `emit-tables.mjs` for both systems and Unity tables, followed by `T0AssetImporter.Import`.
- `CaseObjective` is unchanged. Its negative disclosure guard remains covered with a synthetic record-name objective.
- The fresh t0-b1 PlayMode path now asserts the literal copy and excludes old source/object/location/action tokens.

## Evidence

| Check | Result | Receipt |
|---|---|---|
| Focused RED | 0/1 expected failure: former fallback was shown | `red.xml` |
| Campaign validation | 50/50 | emission stdout and generated receipts |
| Focused GREEN | 1/1 | `green.xml` |
| EditMode | 53/53 | `editmode.xml` |
| PlayMode | 82 passed, 0 failed, 1 explicitly ignored | `playmode.xml` |
| macOS build | `T0_MAC_BUILD Succeeded bytes=403838052` | `build-mac.log` |
| Independent review | PASS; campaign/generated/Unity hashes, importer receipt, spoiler boundary, and guard coverage verified | `deleg_c902ffc7` |

## Limits

- [OBSERVED] This is automated verification, not a human playtest, physical controller/IME test, performance measurement, or G4–G7 promotion.
- [OBSERVED] Unity YAML serializes the producer receipt string with physical wrap whitespace. `git diff --check` flags those serialized lines, but independent review verified this does not alter the embedded JSON receipt and the importer/full suites accept it.
- [OBSERVED] No commit or push was performed.
