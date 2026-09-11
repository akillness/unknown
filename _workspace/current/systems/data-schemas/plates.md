---
updated: 2026-09-10
cycle: 20260909-preproduction-c3
status: current
supersedes: null
owner: game-systems-designer
---

# 데이터 스키마 — plates (기록 매체: 염판 · 당직일지 · 조위대장)

런타임 저작본: `unity/Unknown/Assets/_Project/Data/Authoring/Records/*.asset` (ScriptableObject) [TARGET, 미생성] — 근거는 §0-1.
**T0 인스턴스 값**: `_workspace/current/systems/data/t0/records.json` (5행) — `systems/pipeline/emit-tables.mjs --scope t0` 가 `synopsis/t0-records.md` 의 **형식 고정 표**를 파싱해 낸다. 염판 180샘플·조위대장 136칸은 저장된 표가 아니라 **규칙에서 생성**되며, 문서 앵커와 **33/33 · 28/28** 대조에 실패하면 생성기가 exit 2 한다(C7-F1 · RFC-C7-001).
매체 3종은 `planning/campaign.json`의 `clues[].sourceType` 3값 `plate` / `log` / `ledger`와 1:1 대응한다 [OBSERVED]. **건수는 여기에 옮겨 적지 않는다** — `node planning/validate-campaign.mjs`의 `aggregates.sourceTypeDist`를 읽는다(RFC-Q1). 이전 판의 `plate`(22)/`log`(25)/`ledger`(23)는 저작 원본이 바뀌기 전 값이었고 live 값과 어긋나 있었다 [OBSERVED 2026-09-10: 검증기 출력과 불일치].

## 0-1. 저작 형태 — ScriptableObject인 이유 `[C7-F6]`

이 스키마의 런타임 표현은 **JSON 테이블이 아니라 ScriptableObject**다. 이유는 필드가 Unity 오브젝트를 가리키기 때문이다(매체 텍스처·판독 프리팹·씬 배치). `zones.md`(`viewNodes[].cameraPose`)·`tools.md`(`panelPrefab`)도 같은 이유로 ScriptableObject다.

| 스키마 | 런타임 형태 | 위치 | 왜 |
|---|---|---|---|
| `beats` · `hints` | **JSON** (생성기 출력) | `Data/Tables/` | 저작 원본이 `planning/campaign.json` 하나이고 엔진 참조가 없다 |
| `zones` · `plates` · `tools` | **ScriptableObject** | `Data/Authoring/` | 필드가 Unity 에셋·프리팹·트랜스폼을 가리킨다 |
| `save` | 런타임 직렬화 대상 | 세이브 파일 | 저작본이 아니다 |

이전 판은 이 세 문서의 머리줄이 `Data/Tables/*.json`이라 적고 핸드오프 브리프 §④-1은 ScriptableObject라 적어 **한 회차 안에서 두 말**을 했다(C7-F6). 위 표가 정본이며 브리프 §③ 트리·§④-1이 이것을 인용한다.

## 0. 표기 규약 (6개 스키마 공통)

| 대상 | 규약 | 근거 |
|---|---|---|
| 직렬화 필드(JSON / ScriptableObject) | **camelCase 단일 채택** | `planning/campaign.json`(해시·크기는 `planning/validate-campaign.mjs` 출력을 읽는다 — 고정값 재기재 금지, RFC-Q1)과 `systems/unity-implementation.md` save v1이 이미 camelCase [OBSERVED] |
| C# 공개 멤버 | PascalCase | 변환은 **첫 글자만 소문자화**하는 결정적 1:1 규칙. 약어 대문자 유지 등 그 외 변형 금지 |
| id 값 | 소문자 kebab 또는 소문자 단어 (`t0-b2`, `hub`, `circuit`) | campaign.json 실제 값 [OBSERVED] |
| 텔레메트리 키 | snake_case (**예외**) | 로그 파이프라인 관례. `ops/telemetry-contract.md`가 소유 |

디렉터 지시는 `snake_case | PascalCase` 두 선택지를 제시했다. 위 선택은 그 밖이므로 **RFC-S1**로 제기한다(각 문서 말미).

`tunable` 열: `no` = 코드/스키마 상수, `balance` = `game-balance-designer` 소유, `economy` = `game-economy-designer` 소유, `narrative` = `worldview`/`synopsis` 소유. **tunable ≠ no 인 값을 코드에 하드코딩하면 결함이다**(CLAUDE.md §9).


## 1. Record (매체 공통)

| 필드 | 타입 | tunable | 설명 |
|---|---|---|---|
| `recordId` | string (id) | no | 유일 |
| `sourceType` | enum | no | `plate` \| `log` \| `ledger`. **독립성 판정의 단위**. `beats.md` §5 `Clue.sourceType`과 **같은 이름·같은 도메인**이다 — 이전 판의 `mediaType`에서 개명했다(`beats.md` §1-4 · C6-F13) |
| `displayNameKey` | string | narrative | 로컬라이제이션 키 |
| `systemId` | string? | no | 계통. `sourceType == plate` 일 때 필수 |
| `stationId` | string | no | 기준 관측소. 조위정합의 입력 |
| `stationErrorMinutes` | float | **balance** | 이 관측소의 오차폭 (기본 4.0) |
| `sensorCoverage` | bool | no | 법1. `false`면 확정 슬롯에 못 들어감 |
| `readBudget` | int | **economy** | **원본 상태 카운터의 상한**. 기본 3 (법2). 원본에 직접 가하는 파괴적 절차만 +1 하며 사본 판독은 무제한 — **비차단**(`economy/currency-map.md` §4.2 옵션 B = `plateOriginalWear`와 같은 노브). UI 표기는 "원본 상태", "예산"은 부식 전용 |
| `autoCopyOnFirstRead` | bool | no | **항상 `true`**. `false` 값은 임포트 실패 |
| `indestructible` | bool | no | 매체 2경로 불변식의 "파괴 불가" 경로 표시 |
| `ringHours` | int | no | 염판 12. 다른 매체는 `null` |
| `resolutionMinutes` | int | no | 4. 전 매체 공통 |
| `segments` | Segment[] | no | 시간 구간별 내용 |
| `tidePeaks` | float[] | no | 조위정합 앵커 후보(위상 값) |
| `historicalCorrosion` | CorrosionPattern? | narrative | **읽기 전용 증거**. 운영 부식과 분리(법5 자기오염 방지) |
| `revealBeatId` | string | narrative | 이 매체가 처음 등장하는 비트 |

## 2. Segment

| 필드 | 타입 | tunable | 설명 |
|---|---|---|---|
| `segmentId` | string | no | |
| `phaseStart` / `phaseEnd` | float | narrative | 조위 위상 H± (분). 절대시 금지 |
| `contentKind` | enum | no | `valve` \| `pressure` \| `salinity` \| `waterLevel` \| `door` \| `callStart` \| `handwriting` \| `numeric` |
| `valueKey` | string | narrative | 표시 문자열 키 |
| `isGap` | bool | no | 결손 구간 |
| `gapReason` | enum? | narrative | `crystal_collapse` \| `four_minute_repeat` \| `unrecorded` |
| `clueId` | string? | no | `beats.json`의 단서와 연결 |

**금지 필드**: 얼굴·음성·대화 내용·의도·사람 위치를 담는 필드는 스키마에 **존재하지 않는다**(세계관 §2 한계). 추가 시도는 임포트 실패로 막는다.

## 3. CorrosionPattern (역사 증거 전용)

| 필드 | 타입 | tunable | 설명 |
|---|---|---|---|
| `patternId` | string | no | |
| `phaseStart` / `phaseEnd` | float | narrative | |
| `inconsistentWith` | enum[] | narrative | 예: `power_outage` — R1 반전의 근거 |
| `readOnly` | bool | no | **항상 `true`**. 플레이어 조작으로 변하지 않는다 |

## 4. 캐논 고정값 [OBSERVED: worldview/timeline.md]

| 항목 | 값 |
|---|---|
| 결손 구간 | H-1:00 ~ H+3:00, 정확히 4시간 [OBSERVED: `timeline.md` L46·L119] |
| **순서 앵커 간격** | **20분 — 밸브 개폐 각인 → 봉인 완료 접점 각인** [OBSERVED: `timeline.md` L35·L142(B26)·§8, RFC-P3-013] |
| 정합 후 오차폭 합 | 4 + 4 = 8분 → 20 > 8 이므로 선후 확정 가능 |
| 판 #0 | 원본에 **이름 없음**. 서명지의 가려진 두 번째 이름은 `c4`에서 복원 |

## 5. 불변식 (임포트 검증, fail-closed)

| id | 검사 |
|---|---|
| P-I1 | `recordId` 중복 0, `sourceType` enum 준수 |
| P-I2 | 모든 `autoCopyOnFirstRead == true` |
| P-I3 | 모든 필수 확정에 대해 겹치지 않는 `sourceType` 경로 ≥ 2, 그중 `indestructible == true`가 ≥ 1. **런타임 전용 검사**(`beats.md` §1-3 `R-3`) — 저작 검증기는 앞 절반(독립쌍 존재, `C-07`)까지만 본다 |
| P-I4 | `historicalCorrosion.readOnly == true`, 운영 부식 필드와 교차 참조 0 |
| P-I5 | 금지 필드(얼굴/음성/의도/위치) 0건 |
| P-I6 | `segments`의 위상 구간이 겹치지 않고 `ringHours` 범위 안 |
| P-I7 | `stationErrorMinutes > 0`, 정합 한도(4분)와 모순 없음 |
| P-I8 | 모든 `displayNameKey` / `valueKey`가 KO/EN에 존재 |

## 6. 미측정

매체 텍스처 용량, 판독 렌더 비용, 실제 단서(건수는 검증기 `aggregates.clues`를 읽는다 — 2026-09-10 관측 **73**)의 파일 크기는 **n = 0**.

## RFC-S1 (표기 규약)

| 항목 | 내용 |
|---|---|
| 대상 레인 | game-production-director, game-balance-designer, game-economy-designer, game-planner |
| 질문 | 직렬화 필드 표기를 camelCase로 통일해도 되는가 |
| 제안 | camelCase 채택. snake_case로 가면 `planning/campaign.json` 전체 키를 변환해야 하고 변환 계층이 하나 더 생긴다 |
| 증거 | `planning/campaign.json`의 `schemaVersion / designMinutes / fastMinutes / zoneIds / sourceType` (2026-09-10 재확인) [OBSERVED] |
| 예외 | `systems/game-ui-contract.json`은 외부 스킬 스키마(snake_case)를 따르는 별개 계약이며 런타임 데이터가 아니다 |
