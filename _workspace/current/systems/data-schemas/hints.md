---
updated: 2026-09-10
cycle: 20260909-preproduction-c3
status: current
supersedes: null
owner: game-systems-designer
---

# 데이터 스키마 — hints (3단계 무료 힌트)

런타임 데이터 파일: `unity/Unknown/Assets/_Project/Data/Tables/hints.json` [TARGET, 미생성] — **생성기 출력**이며 손으로 만들지 않는다(`beats.md` §1-2 · `systems/pipeline/emit-tables.mjs`, 2026-09-10 드라이런에서 **99행** 생성 확인 [OBSERVED]).
저작 원본은 `planning/campaign.json`의 `beats[].hints[3]` (33비트 × 3 = **99개**) [OBSERVED].

**T0 인스턴스 값**: `_workspace/current/systems/data/t0/hints.json` (9행 = T0 3비트 × 3단) — 같은 생성기의 `--scope t0` 출력이다.

**로컬라이즈 키 규칙**: 생성기가 `textKey = hint.{beatId}.l{level}` 을 결정론적으로 만들고 저작 문자열을 `sourceTextKo` 로 함께 낸다.
**`T-12` 완화 `[RFC-S6 · C7-F5 · 2026-09-10 R7 종료]`**: `T-12`(미해결 키 0건)는 **KO 필수 · EN 선택**으로 판정됐다 — EN 필드가 없으면 **KO 로 폴백**하며 그것만으로 임포트를 실패시키지 않는다(**T0 는 KO 전용**이고 EN 은 로컬라이제이션 회차 산출물이다). fail-closed 로 남는 조건은 ① KO 키 부재 ② 그 KO 명사의 `worldview/glossary.md` **미등재** 둘뿐이다. 따라서 [OPEN-S7]은 "지금 통과 불가"에서 **"EN 회차 과제"**로 내려간다 — 상태를 감추는 것이 아니라 판정에 맞춰 등급을 낮춘 것이다.

## 0. 표기 규약 (6개 스키마 공통)

| 대상 | 규약 | 근거 |
|---|---|---|
| 직렬화 필드(JSON / ScriptableObject) | **camelCase 단일 채택** | `planning/campaign.json`(해시·크기는 `planning/validate-campaign.mjs` 출력을 읽는다 — 고정값 재기재 금지, RFC-Q1)과 `systems/unity-implementation.md` save v1이 이미 camelCase [OBSERVED] |
| C# 공개 멤버 | PascalCase | 변환은 **첫 글자만 소문자화**하는 결정적 1:1 규칙. 약어 대문자 유지 등 그 외 변형 금지 |
| id 값 | 소문자 kebab 또는 소문자 단어 (`t0-b2`, `hub`, `circuit`) | campaign.json 실제 값 [OBSERVED] |
| 텔레메트리 키 | snake_case (**예외**) | 로그 파이프라인 관례. `ops/telemetry-contract.md`가 소유 |

디렉터 지시는 `snake_case | PascalCase` 두 선택지를 제시했다. 위 선택은 그 밖이므로 **RFC-S1**로 제기한다(각 문서 말미).

`tunable` 열: `no` = 코드/스키마 상수, `balance` = `game-balance-designer` 소유, `economy` = `game-economy-designer` 소유, `narrative` = `worldview`/`synopsis` 소유. **tunable ≠ no 인 값을 코드에 하드코딩하면 결함이다**(CLAUDE.md §9).


## 1. Hint

| 필드 | 타입 | tunable | 설명 |
|---|---|---|---|
| `hintId` | string (id) | no | `t0-b2-h1` 형식. 99개 유일 |
| `beatId` | string | no | `beats.json` 참조 |
| `level` | int | no | 1 \| 2 \| 3. 비트당 각 1개 |
| `textKey` | string | narrative | 로컬라이제이션 키. **문자열 직접 저장 금지** |
| `revealScope` | enum | no | `direction`(1) \| `procedure`(2) \| `solution`(3) |
| `mentionsTools` | string[] | narrative | 2단에서 허용되는 도구 순서 |
| `mentionsMediaTypes` | enum[] | narrative | 2단에서 허용되는 **매체 종류**(값이 아님) |
| `revealsValues` | bool | no | `level == 3` 일 때만 `true` 허용 |
| `warnsBeforeReveal` | bool | no | `level == 3` 이면 **항상 `true`** |
| `cost` | int | **economy** | **0 고정**. 0이 아니면 임포트 실패 |
| `achievementPenalty` | int | **economy** | **0 고정** |
| `affectsEnding` | bool | no | **항상 `false`** |
| `forwardLeakBeatIds` | string[] | narrative | 이 힌트가 언급해도 되는 비트(자기 이하만). 이후 장 비트가 들어가면 임포트 실패 |

## 2. 단계 경계 (hint-system.md H-R1/H-R2 데이터화)

| level | 주는 것 | 주지 않는 것 | 스키마 강제 |
|---|---|---|---|
| 1 | 지금 무엇을 결정해야 하는가, 어느 구역/도구 | 어떤 자료인지 | `mentionsMediaTypes` 비어 있어야 함 |
| 2 | 필요한 매체 2종의 **종류**, 도구 순서 | 정답 값·정답 대상 | `revealsValues == false` |
| 3 | 정확한 자료·조작 값·성립 이유 | 이후 장의 진실 | `forwardLeakBeatIds ⊆ 자기 이하` |

`forwardLeakBeatIds`는 자동 검사가 가능한 부분(비트 id 참조)만 강제한다. 힌트 **문장**이 다음 장 진실을 흘리는지는 자동 검사 불가이며 문서 리뷰 항목으로 남는다 `[INFERENCE]`.

## 3. 무진전 제안 설정

| 필드 | 타입 | tunable | 기본 |
|---|---|---|---|
| `idleHintOfferSeconds` | int | **balance** | 180 |
| `offerCooldownSeconds` | int | **balance** | 180 |
| `offerMaxPerSession` | int | **balance** | 미정 [TARGET] |
| `alwaysShowLevel1` | bool | no | 접근성 설정. 기본 `false`, 켜도 페널티 0 |

무입력 60초 이상 구간은 `afk_gap`으로 별도 기록되며 **자동 제외하지 않는다**(`ops/telemetry-contract.md` §5). 생각 시간은 실패가 아니다.

## 4. 불변식 (임포트 검증, fail-closed)

| id | 검사 |
|---|---|
| H-I1 | 모든 비트가 정확히 3개 힌트를 갖고 `level`이 1·2·3 각 1개 |
| H-I2 | 모든 `cost == 0`, `achievementPenalty == 0`, `affectsEnding == false` |
| H-I3 | `level == 3` → `warnsBeforeReveal == true` |
| H-I4 | `level < 3` → `revealsValues == false` |
| H-I5 | `level == 1` → `mentionsMediaTypes`가 비어 있음 |
| H-I6 | `forwardLeakBeatIds`의 모든 항목이 자기 비트 이하의 순서 |
| H-I7 | 모든 `textKey`가 KO/EN 테이블에 존재 |
| H-I8 | `hintId` 99개 유일 |

## 5. 미측정

힌트 사용률, 단계별 도달률, `stuck_after_l3` 발생 여부 전부 **n = 0**. `stuck_after_l3 > 0`은 G7 대체 검증 FAIL 조건이지만 아직 측정 자체가 없다.

## RFC-S1 (표기 규약)

| 항목 | 내용 |
|---|---|
| 대상 레인 | game-production-director, game-balance-designer, game-economy-designer, game-planner |
| 질문 | 직렬화 필드 표기를 camelCase로 통일해도 되는가 |
| 제안 | camelCase 채택. snake_case로 가면 `planning/campaign.json` 전체 키를 변환해야 하고 변환 계층이 하나 더 생긴다 |
| 증거 | `planning/campaign.json`의 `schemaVersion / designMinutes / fastMinutes / zoneIds / sourceType` (2026-09-10 재확인) [OBSERVED] |
| 예외 | `systems/game-ui-contract.json`은 외부 스킬 스키마(snake_case)를 따르는 별개 계약이며 런타임 데이터가 아니다 |
