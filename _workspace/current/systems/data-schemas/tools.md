---
updated: 2026-09-10
cycle: 20260909-preproduction-c3
status: current
supersedes: null
owner: game-systems-designer
---

# 데이터 스키마 — tools (도구 6종)

런타임 저작본: `unity/Unknown/Assets/_Project/Data/Authoring/Tools/*.asset` (ScriptableObject) [TARGET, 미생성] — 형태 근거는 `plates.md` §0-1 표(`panelPrefab`이 프리팹을 가리키므로 JSON 테이블이 아니다 · C7-F6)
**T0 인스턴스 값**: `_workspace/current/systems/data/t0/tools.json` (`circuit`·`reader` 2행 + 스텁 4) — `systems/pipeline/emit-tables.mjs` 출력이며 **손으로 만들지 않는다**. 이 문서 §1 표·§4 노브를 파싱해 파생하므로 표를 고치면 다음 생성에서 값이 따라 바뀐다(C7-F1 · RFC-C7-001).
도구 6종·도입 비트·재문제 비트는 `planning/campaign.json` 실제 값 [OBSERVED].

## 0. 표기 규약 (6개 스키마 공통)

| 대상 | 규약 | 근거 |
|---|---|---|
| 직렬화 필드(JSON / ScriptableObject) | **camelCase 단일 채택** | `planning/campaign.json`(해시·크기는 `planning/validate-campaign.mjs` 출력을 읽는다 — 고정값 재기재 금지, RFC-Q1)과 `systems/unity-implementation.md` save v1이 이미 camelCase [OBSERVED] |
| C# 공개 멤버 | PascalCase | 변환은 **첫 글자만 소문자화**하는 결정적 1:1 규칙. 약어 대문자 유지 등 그 외 변형 금지 |
| id 값 | 소문자 kebab 또는 소문자 단어 (`t0-b2`, `hub`, `circuit`) | campaign.json 실제 값 [OBSERVED] |
| 텔레메트리 키 | snake_case (**예외**) | 로그 파이프라인 관례. `ops/telemetry-contract.md`가 소유 |

디렉터 지시는 `snake_case | PascalCase` 두 선택지를 제시했다. 위 선택은 그 밖이므로 **RFC-S1**로 제기한다(각 문서 말미).

`tunable` 열: `no` = 코드/스키마 상수, `balance` = `game-balance-designer` 소유, `economy` = `game-economy-designer` 소유, `narrative` = `worldview`/`synopsis` 소유. **tunable ≠ no 인 값을 코드에 하드코딩하면 결함이다**(CLAUDE.md §9).


## 1. 도구 표 [OBSERVED]

| toolId | 법 | 도입 비트 | 미안내 재문제 | 등장 비트 수 | 확정 있음 | 스펙 |
|---|---|---|---|---|---|---|
| `circuit` | 법1 | `t0-b2` | `c6-b2` | 10 | 아니오 | `system-specs/wiring-trace.md` |
| `reader` | 법2 | `t0-b3` | `c1-b2` | **11** | **예(인용 고정)** `[RFC-C7-001]` | `system-specs/plate-readout.md` |
| `alignment` | 법3 | `c3-b2` | `c6-b3` | 8 | 예 | `system-specs/tide-alignment.md` |
| `routing` | 법4 | `c5-b2` | `c7-b2` | 3 | 예 | `system-specs/drainage-routing.md` |
| `corrosion` | 법5 | `c2-b2` | `c4-b2` | 3 | 게이트만 | `system-specs/corrosion-budget.md` |
| `seal` | 법6 | `c1-b3` | `c4-b3` | **7** | 예 | `system-specs/dual-seal.md` |

합계 **42** (한 비트가 여러 도구를 씀). 도구 6종은 **기능 수**이며 UI 프레임 6개가 아니다 — 공용 작업대 셸 1개 + 도구 패널 6종.

**`reader` 의 「확정 있음」 정정 `[RFC-C7-001 (1) · C7-F8 · 2026-09-10 R7 종료]`**: 이전 판은 `reader` 를 「확정 없음」으로 적었다. 디렉터 판정은 **「T0 의 확정(commit) 명령은 `reader` 의 인용 고정(pin citation)이다」**이며 정본은 `planning/gdd.md` §4 표의 `reader` 확정 층(「판독 결과를 가설판에 인용으로 고정, 확정 조건: 매체·계통·관측소 출처가 채워졌을 때」)이다. `interaction-rules.md` §2.2 「판독 자체는 확정이 아니다」와 모순되지 않는다 — **판독(`Read`) ≠ 인용 고정(`CiteToBoard`)** 이다.
결과로 `reader` 는 `hasCommit: true` → **`requiresPreview: true`** 이고 `CiteToBoard`·`ReadOriginal` 두 `commit` 명령이 **`checkpointBefore: true`** 를 갖는다(§5 `T-I3`). `circuit` 은 여전히 확정 없음이다. **T0 에 확정이 0개라는 서술은 폐기한다** — 그 서술 때문에 DoD 6·8 이 검사 대상을 잃었다(C7-F8).

**C3-F2 재측정 [OBSERVED 2026-09-10]**: 이전 판의 `reader` 8 · `seal` 6 · 합계 38은 존재하지 않는 해시(`2bfe4d52…`) 위에 세운 값이었다. live 파일 재집계로 대체한다 — 명령 `node _workspace/current/planning/validate-campaign.mjs`(`aggregates.toolBeatCounts`). 도구 미사용 비트는 **5건**(`t0-b1` `c1-b4` `c3-b4` `c7-b1` `e0-b1`)이며, 이 5건은 도구 확정이 없는 비트다(문서상 "연습 배정"은 데이터가 아니다 — `planning/content-matrix.md` 소유).

| toolId | 등장 비트 id [OBSERVED] |
|---|---|
| `circuit` (10) | `t0-b2` `t0-b3` `c1-b1` `c2-b1` `c2-b3` `c3-b1` `c5-b1` `c6-b1` `c6-b2` `c6-b3` |
| `reader` (11) | `t0-b3` `c1-b2` `c1-b3` `c2-b1` `c2-b4` `c3-b1` `c3-b3` `c4-b1` `c4-b2` `c4-b4` `c5-b3` |
| `alignment` (8) | `c3-b2` `c3-b3` `c4-b1` `c5-b1` `c5-b3` `c6-b3` `c7-b2` `c7-b3` |
| `routing` (3) | `c5-b2` `c5-b4` `c7-b2` |
| `corrosion` (3) | `c2-b2` `c2-b3` `c4-b2` |
| `seal` (7) | `c1-b3` `c4-b3` `c6-b3` `c6-b4` `c7-b3` `c7-b4` `e0-b2` |

등장 횟수는 `campaign.json` 집계, 도입/재문제 비트는 `campaign.meta.md` §5의 "도구 학습 규칙"이 출처다. **12개 (비트, 도구) 쌍이 실제로 그 비트의 `tools` 배열에 존재하는지 2026-09-10에 재확인했다 — 12/12 일치** [OBSERVED].

## 2. Tool

| 필드 | 타입 | tunable | 설명 |
|---|---|---|---|
| `toolId` | string (id) | no | 위 6종 |
| `lawId` | int | no | 1~6 |
| `displayNameKey` | string | narrative | |
| `panelPrefab` | string | no | 공용 셸 안의 패널 |
| `commands` | CommandDef[] | no | 이 도구가 제출할 수 있는 명령 |
| `hasCommit` | bool | no | 확정 명령 보유 여부 |
| `requiresPreview` | bool | no | `hasCommit == true` 면 반드시 `true` |
| `commitHoldSeconds` | float | **balance** | **`hold` opt-in에서만** 쓰이는 유지 시간. 기본 0.4, 범위 0.2~1.5. 확정 기본값은 `two-step`이며 이 값은 홀드를 켠 플레이어에게만 적용된다(RFC-P3-015 F10) |
| `introBeatId` | string | narrative | 안내 도입 1회 |
| `unguidedBeatId` | string | narrative | 미안내 새 문제 1회 |
| `undoable` | bool | no | **항상 `true`** |
| `consumesBudget` | enum | no | `none` \| `readBudget` \| `corrosionBudget` |
| `sandboxDisplaysOnly` | bool | no | 연습에서 자원 소모 여부. `consumesBudget != none` 이면 `true` |

## 3. CommandDef

| 필드 | 타입 | tunable | 설명 |
|---|---|---|---|
| `commandId` | string | no | 예: `AlignBaseline`, `CommitRouting`, `CommitSeal` |
| `kind` | enum | no | `inspect` \| `edit` \| `preview` \| `commit` |
| `validationRules` | string[] | no | 규칙 id (예: `S-R1a`, `A-R1`) |
| `reasonCodes` | enum[] | no | 실패 사유. **UI 문자열이 아니라 코드** |
| `checkpointBefore` | bool | no | `kind == commit` 이면 반드시 `true` |
| `telemetryKeys` | string[] | no | `ops/telemetry-contract.md`의 키만 허용 — 도구 키는 §4.1, 공통 키는 §2~§4·§6 |

## 4. 튜닝 노브 (코드 하드코딩 금지)

| 노브 | 기본값 | 소유 | 사용처 |
|---|---|---|---|
| `commitHoldSeconds` | 0.4 | balance | 전 확정 도구 |
| `readBudget` | 3 | balance | `plates` 저작본 per record |
| `residualLimitMinutes` | 4 | balance | `alignment` 확정 조건 |
| `corrosionLimit` | 9 | economy | `corrosion` 게이트 — **전역 단일 상한** |
| `routeCorrosionCost[*]` | lowland 7 · dock 8 · dock-express 12 | economy | `routing` 선택지 비용 |
| `systemLimits[*]` | `null` | economy | **표시 라벨 전용**, 차단하지 않음 (RFC-P3-009) |
| `idleHintOfferSeconds` | 180 | balance | `hint-system` |
| `hintCost` | 0 | economy | **0 고정**. 0이 아닌 값은 임포트 실패 |

## 5. 불변식 (임포트 검증, fail-closed)

| id | 검사 |
|---|---|
| T-I1 | `toolId` 6종만 존재, 중복 0 |
| T-I2 | 모든 `introBeatId` / `unguidedBeatId`가 `beats.json`에 존재하고 `introBeatId`가 시간상 먼저 |
| T-I3 | `hasCommit == true` → `requiresPreview == true` 이고 모든 `commit` 명령이 `checkpointBefore == true` |
| T-I4 | 모든 `undoable == true` |
| T-I5 | `hintCost == 0` |
| T-I6 | `telemetryKeys`의 모든 키가 텔레메트리 계약에 정의됨. **등재부는 `ops/telemetry-contract.md` §4.1(도구 키)**이며 2026-09-10 R7에 신설됐다 — 그 전에는 도구 키가 계약에 **0건**이라 T0 도구 2종 임포트가 이 검사에서 실패했을 것이다(C7-F11). 검사는 **완화하지 않았다**; 계약 쪽을 채웠다 |
| T-I7 | `consumesBudget != none` → `sandboxDisplaysOnly == true` |
| **T-I8** | `hasCommit == true` 인 도구는 **`commitCondition` 을 갖는다**(무엇이 채워져야 확정되는가). `reader` = 「인용에 `sourceType`·계통·관측소 출처가 모두 채워졌을 때」 `[RFC-C7-001]`. 조건 없는 확정은 임포트 실패 |
| **T-I9** | 코드의 `ReasonCode` enum 집합이 **`handoff/codex-unity-brief.md` §⑤-7 표의 집합과 일치**하고 `Localization/strings/ko.json` 의 키 집합이 같다 `[C7-F14]`. **KO 문안 일치는 검사하지 않는다** — 문안은 `[TARGET] 자리표시자`다 |

`T-I1`·`T-I3`·`T-I4`·`T-I7` 은 `emit-tables.mjs --scope t0` 가 생성 시점에 이미 검사하며 결과가 `tables-receipt.json` 의 `toolInvariants` 에 찍힌다 [OBSERVED 2026-09-10 R7 종료: 전건 PASS]. 임포터는 그 영수증을 대조한다.

## 6. 미측정

도구 전환 시간, 패널 열기 지연, 실제 조작감은 **n = 0**. 패드 실기 테스트 0회.

## RFC-S1 (표기 규약)

| 항목 | 내용 |
|---|---|
| 대상 레인 | game-production-director, game-balance-designer, game-economy-designer, game-planner |
| 질문 | 직렬화 필드 표기를 camelCase로 통일해도 되는가 |
| 제안 | camelCase 채택. snake_case로 가면 `planning/campaign.json` 전체 키를 변환해야 하고 변환 계층이 하나 더 생긴다 |
| 증거 | `planning/campaign.json`의 `schemaVersion / designMinutes / fastMinutes / zoneIds / sourceType` (2026-09-10 재확인) [OBSERVED] |
| 예외 | `systems/game-ui-contract.json`은 외부 스킬 스키마(snake_case)를 따르는 별개 계약이며 런타임 데이터가 아니다 |

## 7. 변경 로그 (같은 사이클 제자리 개정 · RFC-Q2)

| 날짜 | 회차 | 결함 | 바뀐 절 | 내용 |
|---|---|---|---|---|
| 2026-09-10 | **R7 종료 수정** | **C7-F1**(S1) · **C7-F8** · C7-F14 | 머리글 · §1 `reader` 행 + 정정 문단 · §5 `T-I8`·`T-I9` 신설 · 이 절 신설 | `reader` 의 「확정 있음」을 **예(인용 고정)** 로 정정(RFC-C7-001 (1)). T0 인스턴스 값 경로(`systems/data/t0/tools.json`)를 머리글에 등재. 확정 조건 보유(`T-I8`)와 `ReasonCode` 집합 일치(`T-I9`)를 불변식으로 세웠다 |

- `cycle` 값 불변(RFC-Q2). **새로 측정된 런타임 값 0건** — 도구 전환 시간·조작감은 여전히 n=0(§6).
