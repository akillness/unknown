---
updated: 2026-09-10
cycle: 20260909-preproduction-c3
status: current
supersedes: null
owner: game-systems-designer
---

# 데이터 스키마 — beats (33비트 캠페인 테이블)

저작 원본: `_workspace/current/planning/campaign.json` — **소유 레인 `game-planner`**, 이 문서는 그것을 수정하지 않는다.
런타임 데이터 파일: `unity/Unknown/Assets/_Project/Data/Tables/beats.json` [TARGET, 미생성] — 저작 원본의 **1:1 미러**.
**T0 인스턴스 값**: `_workspace/current/systems/data/t0/beats.json` (3행 · 서브셋 + **`completionPredicate`**) — `systems/pipeline/emit-tables.mjs --scope t0` 출력이며 손으로 만들지 않는다. 완료 술어는 각 비트의 `completion` 원문(`_srcCompletion`)이 지목하는 대상을 기계 형태로 옮긴 것이고, `proofRequired` 비트에 확정 명령이 없으면 생성기가 **exit 2** 한다(C7-F1 · C7-F8 · RFC-C7-001 (3)).

## 0. 표기 규약 (6개 스키마 공통)

| 대상 | 규약 | 근거 |
|---|---|---|
| 직렬화 필드(JSON / ScriptableObject) | **camelCase 단일 채택** | `planning/campaign.json`(해시·크기는 §1.1 = `validate-campaign.mjs` 출력, 고정값 재기재 금지 — RFC-Q1)과 `systems/unity-implementation.md` save v1이 이미 camelCase [OBSERVED] |
| C# 공개 멤버 | PascalCase | 변환은 **첫 글자만 소문자화**하는 결정적 1:1 규칙. 약어 대문자 유지 등 그 외 변형 금지 |
| id 값 | 소문자 kebab 또는 소문자 단어 (`t0-b2`, `hub`, `circuit`) | campaign.json 실제 값 [OBSERVED] |
| 텔레메트리 키 | snake_case (**예외**) | 로그 파이프라인 관례. `ops/telemetry-contract.md`가 소유 |

디렉터 지시는 `snake_case | PascalCase` 두 선택지를 제시했다. 위 선택은 그 밖이므로 **RFC-S1**로 제기한다(각 문서 말미).

`tunable` 열: `no` = 코드/스키마 상수, `balance` = `game-balance-designer` 소유, `economy` = `game-economy-designer` 소유, `narrative` = `worldview`/`synopsis` 소유. **tunable ≠ no 인 값을 코드에 하드코딩하면 결함이다**(CLAUDE.md §9).


## 1. 미러 규칙 (동기화 계약)

| 규칙 | 내용 |
|---|---|
| M1 | `beats.json`은 `campaign.json`에서 **생성기 하나로만** 생성된다 — `systems/pipeline/emit-tables.mjs`(§1-2). 손으로 편집하지 않고, 다른 경로로 만들지 않는다 |
| M2 | 필드명은 변환하지 않는다(양쪽 camelCase). **변환 계층 0** — 생성기는 `beats.json`을 저작 원본의 **바이트 동일 사본**으로 낸다. 즉 `sha256(beats.json) == sha256(campaign.json)`이며, 이것이 M2를 문서 주장이 아니라 기계 성질로 만든다 [OBSERVED 2026-09-10, §1-2 영수증] |
| M3 | 런타임 전용 필드(`checkpointId` 정규화, `localizationKey`)는 **임포터가 메모리에서** 붙인다. 테이블 파일에 굽지 않는다(구우면 M2가 깨진다) |
| M4 | 저작 원본이 바뀌면 생성기를 다시 돌린다. 생성기는 검증기를 먼저 호출하고 `verdict != PASS`이면 **테이블을 만들지 않는다**(fail-closed) |
| M5 | 필드 개명이 필요하면 **먼저 영향 범위를 조회**한 뒤 마이그레이션과 함께 진행한다. 이번 회차는 `mex` 실행 금지이므로 조회 영수증 = `[SKIPPED: mex 금지]` |
| **M6** | **검증 규칙은 한 곳에만 산다** — `planning/validate-campaign.mjs`(planner 소유). 생성기도 임포터도 그 규칙을 **복제하지 않는다**. 임포터는 영수증 해시를 대조하고 **런타임 전용 불변식만** 검사한다(§1-3) |

### 1-2 파이프라인 — 생성기·시점·영수증 `[C6-F13]`

**신설 2026-09-10 R7 (C6/C7 수정 루프 1).** 이전 판은 M1에 "생성된다"고만 적고 *무엇이* 생성하는지·*언제* 도는지·*무엇으로 대조하는지*를 정의하지 않았다. 그 공백 때문에 핸드오프 브리프 §④-2는 Unity 임포터에게 **47검사를 C#으로 재구현**하라고 지시하고 있었다. 두 구현이 갈라지면 어느 쪽이 정본인지 말할 수 없다.

```
planning/campaign.json  (저작 원본 · planner 소유 · 읽기 전용)
        │
        ├─▶ planning/validate-campaign.mjs   ← 규칙의 유일한 출처 (planner 소유)
        │        verdict != PASS 면 여기서 멈춘다 (fail-closed, exit 1, 출력 0건)
        ▼
systems/pipeline/emit-tables.mjs  (생성기 · systems 소유 · 검증기를 호출만 한다)
        │
        ├─▶ Data/Tables/beats.json          derivation: copy       (바이트 동일)
        ├─▶ Data/Tables/hints.json          derivation: projection (99행)
        └─▶ Data/Tables/tables-receipt.json 영수증(sha256 · 검증기 판정 · 임포터 계약)
                 │
                 ▼
        Unity 임포터: 영수증 해시 대조 + 런타임 전용 불변식만 (§1-3)
```

| 시점 | 무엇이 돈다 |
|---|---|
| 저작 원본 편집 직후(planner) | `node planning/validate-campaign.mjs` — planner 몫 |
| Unity 에디터 빌드 스텝 / 임포트 전(실행자) | `node systems/pipeline/emit-tables.mjs --out <프로젝트>/Assets/_Project/Data/Tables` |
| 임포트 시(런타임 코드) | 영수증 대조 + §1-3 런타임 검사 |

**실행 영수증 [OBSERVED 2026-09-10 · 본 레인이 실제로 실행]**

```
$ node _workspace/current/systems/pipeline/emit-tables.mjs                    # 드라이런(쓰기 0건)
$ node _workspace/current/systems/pipeline/emit-tables.mjs --out <scratch>
$ shasum -a 256 _workspace/current/planning/campaign.json <scratch>/beats.json
```

| 관측 | 값 |
|---|---|
| 검증기 판정 | `checks 47 / pass 47 / fail 0 / verdict PASS`, exit 0 |
| `beats.json` | `copy` · 121,457 B · 33행 · **저작 원본과 sha256 동일**(두 줄 같은 해시로 출력됨) |
| `hints.json` | `projection` · 40,724 B · **99행**(33비트 × 3단) |
| fail-closed 픽스처 | `t0-b1.zoneId`를 스테이지 밖 값으로 바꾼 사본 → `Z-01` FAIL → 생성기 **exit 1, 출력 디렉터리 미생성** |

숫자는 저작 원본이 바뀌면 달라진다. 인용할 때는 옮겨 적지 말고 생성기를 다시 돌린다(RFC-Q1). 상세는 `systems/pipeline/emit-tables.meta.md`.

> **R7 종료 고지 [OBSERVED 2026-09-10]**: 위 표는 **R7 실행 시각의 관측**이며 그 뒤 planner 가 `campaign.json` 을 갱신해 **바이트·sha·검사 수가 모두 달라졌다**. 현행 값은 `node _workspace/current/planning/validate-campaign.mjs` 와 `node _workspace/current/systems/pipeline/emit-tables.mjs` 를 다시 돌려 읽는다. 이 표는 **역사로만 읽고 인용하지 않는다** — 「47검사」 같은 수를 다음 문서로 옮기는 것이 C7-F25 가 지적한 형태다.

**이번 회차에 Unity 프로젝트로 쓴 파일은 0건이다** — 출력은 스크래치 디렉터리뿐이며 `unity/Unknown/Assets/`는 비어 있는 상태 그대로다 [OBSERVED].

### 1-3 임포터가 재구현하는 것 / 하지 않는 것 `[C6-F13]`

| 임포터 몫(런타임 전용) | 왜 저작 시점에 못 하는가 |
|---|---|
| `R-1` 로컬라이즈 미해결 키 0건 (`T-12`) | KO/EN 테이블이 저작 JSON 밖에 있다 |
| `R-2` 고아 0건 | Unity 에셋 참조 그래프가 있어야 판정된다 |
| `R-3` 독립쌍 중 **파괴 불가 경로 ≥ 1** | 런타임 규칙. §6 `B-I11`의 미검증 절반이 정확히 이것이다 |
| `R-4` 임의 도달 상태에서 엔딩 3종 도달 | 런타임 상태공간 탐색. §6 `B-I12` |

**임포터가 하지 않는 것**: §6의 저작 시점 검사 전건(현재 47). 영수증 해시 대조가 그 자리를 대신한다. 재구현하면 규칙이 두 벌이 되고, 두 벌은 반드시 갈라진다.

### 1-4 `sourceType` 단일 명칭 `[C6-F13]`

**같은 개념에 이름이 두 벌이었다**: `beats.md` §5 `Clue.sourceType`(enum `plate` \| `log` \| `ledger`) ↔ `plates.md` §1 `Record.mediaType`(같은 3값). 저작 원본이 쓰는 이름은 `sourceType`이고 [OBSERVED: `campaign.json` `clues[].sourceType`], 임포터가 한쪽을 다른 쪽으로 옮기면 그것이 곧 "변환 계층 1"이다(M2 위반).

**결정**: **`sourceType` 하나로 통일**한다. `plates.md` §1의 `mediaType`을 `sourceType`으로 개명했다. 개명 비용은 **0** — 코드 0줄·런타임 테이블 0개·세이브 파일 0개이며 `save.md` §4 개명 금지 목록에도 없는 필드다 [OBSERVED]. **v1 확정 이후에는 이 개명이 불가능**해지므로 지금 한다.

- 용어("매체 종류")는 남는다. 바뀐 것은 **직렬화 필드 이름 하나**다.
- `hints.md` §1 `mentionsMediaTypes`는 **다른 필드**이며(힌트 2단이 언급해도 되는 *종류의 목록*) 이번 개명 대상이 아니다. 값 도메인만 `sourceType` enum과 같다.

### 1.1 저작 원본 실측 영수증 [OBSERVED 2026-09-10, C3-F2 대응]

정본은 **RFC-P3-008 계보 B** = live `_workspace/current/planning/campaign.json` 하나뿐이다. 아카이브 c3 판본은 역사로만 인용한다.

재측정 명령 (본 레인이 이번 회차에 실제로 실행):

```
shasum -a 256 _workspace/current/planning/campaign.json
wc -c        _workspace/current/planning/campaign.json
node         _workspace/current/planning/validate-campaign.mjs
```

**RFC-Q1 규칙**: sha·크기·검사 수는 **검증기 출력에서 읽는다**. 아래 값은 *이 회차의 실행 시각(2026-09-10 R4)에 관측된 값*이며, 저작 원본(planner 소유)이 바뀌면 달라진다 — 인용할 때는 숫자를 옮기지 말고 검증기를 다시 돌린다.

| 항목 | 실측값 [OBSERVED 2026-09-10 R4] |
|---|---|
| sha256 | `92301c0a5ecfc7e142646f5f17c29c21609e2b2de7c90408de7bffc9d815ae23` (직전 R3 관측 `fdabf1d4…`) |
| 크기 | 121,457 bytes (직전 120,479) |
| 검증기 판정 | **47 검사 / 47 PASS / 0 FAIL** (직전 44 / 44 / 0 — `Z-01`·`Z-02`·`K-06` 추가) |
| 스테이지 · 비트 | 9 · 33 |
| 스테이지 분 | 25 · 50 · 55 · 65 · 65 · 70 · 75 · 65 · 10 = 480 |
| `fastMinutes` 합 / `deliberateMinutes` 합 | **322** / **673** |
| 단서 수 | **73** (id 유일 73/73) |
| 비트 키 수 (합집합) | **24키**(직전 23 + `zoneId`), 24키 전부 33/33 비트에 존재 |
| 힌트 | 3단 × 33비트, 빈 문자열 0 |
| 도구 등장 비트 수 | `circuit` 10 · `reader` 11 · `alignment` 8 · `routing` 3 · `corrosion` 3 · `seal` 7 (합 42) |
| 도구 미사용 비트 | 5건 — `t0-b1` `c1-b4` `c3-b4` `c7-b1` `e0-b1` |
| `toolTeaching` | 12건 (도구 6종 × guided 1 + unguided 1) |
| `proofRequired` | 15 / 33 |
| 구역 | 5종 (`hub` `gate` `pump` `dock` `lowland`) |
| **비트별 `zoneId` 분포** | `hub` 15 · `pump` 6 · `dock` 5 · `lowland` 4 · `gate` 3 = 33 [검증기 `aggregates.zoneBeatCounts`] |
| 구역별 설계 분 | `hub` 188 · `pump` 102 · `dock` 84 · `lowland` 67 · `gate` 39 = 480 [`aggregates.zoneBeatMinutes`] |
| 구역 연속 구간 | 13구간 [`aggregates.zoneRunCount` · `zoneRunSequence`] |

**폐기된 인용**: 이전 판의 "실측 sha256 `2bfe4d52…`"는 저장소 어디에도 존재하지 않는 값이었고(C3-F2), 그 위에 세운 "fast 321 / deliberate 672 / 단서 70 / 도구 10·8·8·6·3·3"도 함께 폐기한다. 위 표가 그 자리를 대체한다.

**RFC-S4 종결**: 해시 드리프트 RFC는 RFC-P3-008(계보 B 확정)과 `planning/validate-campaign.mjs`(해시를 매 실행 재계산해 출력)로 해소됐다. 이후 저작 원본이 바뀌면 검증기 출력의 `sha256` 필드가 그대로 영수증이 되므로 본 문서는 그 값을 인용한다.

## 2. Campaign (루트) [OBSERVED: campaign.json 실제 키]

| 필드 | 타입 | tunable | 설명 |
|---|---|---|---|
| `schemaVersion` | int | no | 현재 1 |
| `cycle` | string | no | `20260909-preproduction-c3` |
| `designMinutes` | int | **balance** | 480. **설계 분량 합계**이며 관측치가 아니다 |
| `observedMedianMinutes` | float? | no | 실측 전용. 현재 `null` [OBSERVED] |
| `humanPlaytests` | object[] | no | 현재 `[]` — 표본 **n = 0** [OBSERVED] |
| `stages` | Stage[] | no | 9개 |

## 3. Stage [OBSERVED]

| 필드 | 타입 | tunable | 설명 |
|---|---|---|---|
| `id` | string | no | `T0` `C1`…`C7` `E0` |
| `title` | string | narrative | |
| `zoneIds` | string[] | no | `zones` 저작본(ScriptableObject · `plates.md` §0-1) 참조. **스테이지가 허용하는 구역 집합**이며, 비트 단위 배정은 `Beat.zoneId`(§4)가 담당한다 — 두 값의 포함관계를 `Z-01`이 검사한다 |
| `storyPhase` | enum | narrative | `tutorial` `setup` `rising` `complication` `midpoint` `turn` `revelation` `resolution` `epilogue` |
| `minutes` | int | **balance** | 25·50·55·65·65·70·75·65·10 = 480 [OBSERVED, 검증기 S-02] |
| `beats` | Beat[] | no | T0=3, C1~C7=4, E0=2 |

**런타임 추가 필드**: `storyClock` (string, narrative) — 21:00 / 21:30 / 22:10 / 23:00 / 00:00 / 01:00 / 02:00 / 03:30 / 05:00 [OBSERVED: worldview/timeline.md]. 단일 야간이므로 `dayIndex`는 **정의하지 않는다**(`system-specs/save-undo.md` §0).

## 4. Beat [OBSERVED 2026-09-10 R4: 실제 **24개 키**, 24키 전부 33/33 비트에 존재]

| 필드 | 타입 | tunable | 설명 |
|---|---|---|---|
| `id` | string | no | `t0-b2` 형식. 33개 유일 |
| `title` | string | narrative | |
| `kind` | enum | no | `puzzle`(21) \| `exploration`(2) \| `dialogue`(5) \| `payoff`(5) |
| `minutes` | int | **balance** | 설계 중심값 |
| `fastMinutes` | int | **balance** | 빠른 진행 **설계 경계**. 합 **322** |
| `deliberateMinutes` | int | **balance** | 신중한 진행 **설계 경계**. 합 **673** |
| `prerequisites` | string[] | no | 앞선 비트만. DAG, 순환 0 |
| `tools` | string[] | no | `tools` 저작본(ScriptableObject) 참조 |
| `zoneId` | string | no | **비트가 벌어지는 구역 1개**. `zones` 저작본의 5구역 중 하나이며 **소속 스테이지의 `zoneIds`에 포함**돼야 한다. 값 분포 `hub` 15 · `pump` 6 · `dock` 5 · `lowland` 4 · `gate` 3 [OBSERVED 2026-09-10 R4]. 검증 `Z-01`(∈ stage.zoneIds) · `Z-02`(33/33 존재) — 둘 다 PASS. **비트-구역의 단일 출처는 이 필드**이며 `planning/content-matrix.md` §3 표는 여기서 파생한다(C3-F22 · C3-F29) |
| `objective` / `inference` / `action` / `consequence` / `completion` / `recovery` | string | narrative | 공란 금지 |
| `activityBudget` | object | **balance** | 5키 고정 (`exploration` `reasoning` `manipulation` `dialogue` `payoff`), 음수 없음, 합 = `minutes`. 전체 합 53·190·168·28·41 = 480 |
| `timeConfidence` | enum | no | `low`(12) \| `medium`(21). **`high` 금지** — 표본 n=0에서 높은 신뢰를 주장하지 않는다 |
| `authorEstimateBasis` | string | narrative | 그 비트의 분 추정 근거 문장. 33/33 존재, 공란 금지 |
| `subtasks` | string[] | narrative | 4~5개 (4개 16비트 · 5개 17비트). 비트 분이 무엇으로 채워지는지의 분해 |
| `toolTeaching` | object[] | no | `{tool, mode}` — `mode ∈ {guided, unguided}`. 총 12건, 도구 6종 각각 guided 1 + unguided 1 |
| `proofRequired` | bool | no | 15건 `true`. `true`면 독립 쌍(루트 `originId` 상이 AND `sourceType` 상이)이 데이터에 존재해야 한다 — 불변식 **B-I18**(§6), 규칙 원문은 `systems/interaction-rules.md` §3 |
| `clues` | Clue[] | no | 총 **73개**, id 유일 |
| `hints` | string[3] | narrative | 정확히 3개 |
| `checkpoint` | string | no | 총 33개, id 유일 |

`fastMinutes` / `deliberateMinutes`는 **시나리오 폭이며 측정 신뢰구간이 아니다**(부트스트랩·IQR·표본 없음). 텔레메트리에서 `observed_p25_min` / `observed_p75_min` 과 **같은 축에 그리지 않는다**.

## 5. Clue

| 필드 | 타입 | tunable | 설명 |
|---|---|---|---|
| `id` | string | no | `t0-b2-c1` 형식 |
| `sourceType` | enum | no | `plate`(24) \| `log`(27) \| `ledger`(22) [OBSERVED] |
| `originId` | string | no | **물리 출처 식별자**. 카탈로그 31종 [OBSERVED]. 표시 라벨이 아니라 독립성 판정의 근거 |
| `copiedFrom` | string? | no | 사본이면 원본 `originId`, 아니면 `null`. **재귀 해석해 루트까지 간다** — 사본은 루트를 물려받는다(`systems/interaction-rules.md` §3). 현재 사본 간선 2개: `council-copybook → tide-ledger-bureau`, `bureau-copy-index → council-copybook`(둘 다 루트 `tide-ledger-bureau`) |
| `description` | string | narrative | |
| `recordId` | string | no | **런타임 추가**. `plates` 저작본 연결 |
| `autoKept` | bool | no | **런타임 추가**. 첫 판독 자동 사본 대상 |

## 6. 불변식 (임포트 검증, fail-closed)

**출처 갱신**: 이전 판은 `campaign.meta.md` §4의 자체 검사 결과를 인용했다. 이번 회차부터는 **본 레인이 직접 실행한** `node _workspace/current/planning/validate-campaign.mjs` 출력(2026-09-10 R4 실행 = **47/47 PASS**)을 인용한다. 검사 id 대응을 함께 적어 재현 가능하게 한다.

| id | 검사 | 검증기 대응 | 결과 [OBSERVED 2026-09-10] |
|---|---|---|---|
| B-I1 | 스테이지 9개, 분 배분 일치, 총합 480 | S-01·S-02·S-03·S-05 | PASS (25·50·55·65·65·70·75·65·10) |
| B-I2 | 비트 33개, 스테이지별 분 합 = 스테이지 분 | B-01·B-02·S-04·S-05 | PASS (9/9) |
| B-I3 | beat / clue / checkpoint id 유일 | B-03·B-04·B-05 | PASS (**33 / 73 / 33**) |
| B-I4 | `kind` enum 준수, puzzle ≥ 20 | B-06 | PASS (puzzle 21 · exploration 2 · dialogue 5 · payoff 5) |
| B-I5 | 비트마다 서로 다른 매체 2종 이상 | C-06 | PASS (33/33) |
| B-I6 | `hints` 정확히 3단, 빈 문자열 없음 | H-01 | PASS (33/33) |
| B-I7 | `prerequisites` DAG, 순환 0, 앞선 비트만 참조 | P-01·P-02·P-03 | PASS |
| B-I8 | 도구 6종 / 구역 5종 id 일치 | V-01·S-06 | PASS |
| B-I9 | `fastMinutes < minutes < deliberateMinutes` | T-03 | PASS (33/33) |
| B-I13 | `activityBudget` 5키·비음수·합 = `minutes` | T-01·T-02 | PASS (범주 합 480) |
| B-I14 | `timeConfidence`에 `high` 없음 | T-04 | PASS (low 12 · medium 21) |
| B-I15 | `subtasks` 3~5개, `authorEstimateBasis` 전건 존재 | T-05·T-06 | PASS |
| B-I16 | `toolTeaching` 12건, 도구별 guided 1 + unguided 1, 해당 비트 `tools`에 포함 | V-02·V-03·V-04 | PASS |
| B-I17 | `clue.originId` 전건 부착 · `copiedFrom` 참조 유효 · 지도 모순 0 · 순환 0 | C-01~C-05 | PASS (카탈로그 31종, 사본 간선 2개) |
| B-I18 | **`proofRequired` 비트마다 독립 쌍 존재** (루트 `originId` 상이 AND `sourceType` 상이) | C-07 | PASS (15/15) — C3-F12의 유일한 기계 증거 |
| B-I19 | **비트 `zoneId` 전건 존재 + 소속 스테이지 `zoneIds` 안** | **Z-02 · Z-01** | PASS (33/33 · 위반 0) [OBSERVED 2026-09-10 R4]. C3-F22 신설. 비트-구역 단일 출처가 문서 표에서 **데이터로** 이동했다 |
| B-I10 | 모든 서술 필드의 로컬라이제이션 키가 KO/EN에 존재 | — | **미검증** (런타임 테이블 미생성) |
| B-I11 | 모든 필수 확정에 `sourceType` 상이 AND 루트 `originId` 상이인 자료쌍 ≥ 1, 그 쌍의 파괴 불가 경로 ≥ 1 | C-06·C-07이 **부분** 검사 | **부분 검증** — "겹치지 않는 2경로"는 C-07이 데이터에서 확인했으나 "파괴 불가 1개"는 런타임 규칙이라 미검증 |
| B-I12 | 임의 도달 상태에서 엔딩 3종 도달 가능 | — | **미검증** (Unity 도달성 탐색 미실행. 프로토타입 `model.mjs`는 T0·C3 부분 모형에서만 INV2로 확인) |

B-I1~B-I9·B-I13~B-I18은 **저작 원본(JSON)에 대한 검사**이며 **런타임 임포트로는 아직 확인하지 않았다**. 임포터가 생기면 같은 표를 임포트 경로에서 다시 돌린다.

## 7. 미측정

`observedMedianMinutes`(현재 `null`), `humanPlaytests`(현재 `[]`), 완주율, 힌트 사용률, 이탈 시점 전부 **n = 0** [OBSERVED, 검증기 F-03·F-04]. 480 합계는 D 게이트 입력이지 G7 플레이 검증이 아니다.

`fastMinutes` 합 322 · `deliberateMinutes` 합 673은 **시나리오 경계**이며 표본 통계(`observed_p25_min` / `observed_p75_min`)와 같은 축에 놓지 않는다(RFC-P3-011). 수용 판정 키는 `total_minus_afk_min` 하나이며 정의는 `ops/telemetry-contract.md` §5에 있다.

## RFC-S1 (표기 규약)

| 항목 | 내용 |
|---|---|
| 대상 레인 | game-production-director, game-balance-designer, game-economy-designer, game-planner |
| 질문 | 직렬화 필드 표기를 camelCase로 통일해도 되는가 |
| 제안 | camelCase 채택. snake_case로 가면 `planning/campaign.json` 전체 키를 변환해야 하고 변환 계층이 하나 더 생긴다 |
| 증거 | `planning/campaign.json`의 `schemaVersion / designMinutes / fastMinutes / zoneIds / sourceType` (2026-09-10 재확인) [OBSERVED] |
| 예외 | `systems/game-ui-contract.json`은 외부 스킬 스키마(snake_case)를 따르는 별개 계약이며 런타임 데이터가 아니다 |
