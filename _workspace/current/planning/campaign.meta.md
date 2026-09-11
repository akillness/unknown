---
updated: 2026-09-10
cycle: 20260909-preproduction-c4
status: current
supersedes: _workspace/archive/20260909-preproduction-c3/planning/campaign.meta.md
owner: game-planner
describes: _workspace/current/planning/campaign.json
authored_by: game-synopsis-writer (위임 배정 / delegated assignment)
---

# campaign.json 메타 (C4 수리본)

`campaign.json`은 구조화 산출물이라 frontmatter를 넣지 않는다(CLAUDE.md §10). 소유자·해시·출처·관측/목표는 이 파일이 보유한다. **런타임 주장 0건** — 빌드도 플레이도 없다.

## 1. 파일 사실 [OBSERVED]

| 항목 | 값 | 측정 명령 |
|---|---|---|
| 경로 | `_workspace/current/planning/campaign.json` | — |
| bytes | **124007** | `wc -c _workspace/current/planning/campaign.json` |
| sha256 | `8a43d334f8f69d6642a74708e7b3d68353c7982fb5f506dbba291021a538a93a` | `shasum -a 256 _workspace/current/planning/campaign.json` |
| 직전 live 판 sha256 | `92301c0a5ecfc7e142646f5f17c29c21609e2b2de7c90408de7bffc9d815ae23` (121457 B) — **R7 편집 전**(§5.2) | 동상 |
| 그 이전 live 판 sha256 | `fdabf1d421932d679429870a06eeb31e9527562370c6a107aec77c2c884fb2b7` (120479 B) — **zoneId 추가 전** | 동상 |
| 그 이전 live 판 sha256 | `775a984cdc1a4d76a55bfed5f3e9dc6dc497ca0c93b8bcb3b7afce56fe6a9a52` (120087 B) — C3-F11 수정 **전** | 동상 |
| 아카이브 c3 판 sha256 | `5029d44a8042323d438d5975604c12b17f2717fc41b93db82f1cab06996f7e16` (74342 B) — **역사 인용 전용**(RFC-P3-008) | 동상 |
| schemaVersion | 1 (필드 추가만, 제거 없음) | — |
| 소유 레인 | `game-planner` | — |

**2026-09-10 갱신 사유 (1차)** [OBSERVED]: `qa/c3-review.md` C3-F11(`c1-b4` 매체 1종)을 닫기 위해 단서 1건을 더했다(§10). 그 편집으로 bytes·sha256·단서 수가 바뀌었으므로 위 표를 실측으로 다시 적었다.

**2026-09-10 갱신 사유 (2차 · R4)** [OBSERVED]: 디렉터 C3 종료 판정(**C3-F22** 비트 단위 `zoneId`, **RFC-W4** R2 의도 공개 시점, **C3-F29** `c1-b4` 구역 오기)에 따라 세 종류의 편집을 했다 — ① 33/33 비트에 `zoneId` 필드 추가(§2·§5.1), ② `c4-b3.inference`의 의도 문장을 `c6-b4.inference`로 이동, ③ `c1-b4.subtasks[0]`의 "제3수문에서" → "당직실에서". **분·단서·도구·선행조건 값은 하나도 바꾸지 않았다**(§5.1 회귀표). 그 결과 sha256이 `fdabf1d4…` → **`92301c0a…`**, bytes 120479 → **121457**로 바뀌었다. `fdabf1d4…`를 인용하는 문서는 전부 스테일이다 — 이후 인용은 고정 숫자를 재기재하지 말고 **검증기 출력의 `sha256`을 쓴다**(RFC-Q1).

**2026-09-10 갱신 사유 (3차 · R7)** [OBSERVED]: 디렉터 R7 레인 배정(**C6-F1 · C6-F3 · C6-F17 · RFC-N6 · RFC-C7-001(3)**)을 데이터에 반영했다 — ① `t0-b1.objective`에 오늘 밤의 산출물(청문 제출 문서 1건) 문장 추가와 `t0-b1-c1` 단서 서술 1문장 보강(C6-F1), ② `hints[0]` **27비트 재작성**(C6-F3), ③ `c2-b4`·`c6-b4`의 `zoneId`를 본문에 맞추고 스테이지 `C2`·`C6`의 `zoneIds` 확장 + `c1-b2`·`c5-b2` 본문 구역 정정(C6-F17), ④ `c1-b2`·`c3-b1`의 `proofRequired` → `true`(RFC-N6), ⑤ `t0-b1`~`t0-b3`의 `completion`을 열람·표시·인용 고정 술어로 정밀화(RFC-C7-001 (3)). **분·도구·선행조건·단서 수는 하나도 바꾸지 않았다**(§5.2 회귀표). sha256 `92301c0a…` → **`8a43d334…`**, bytes 121457 → **124007**. 고정 sha를 인용하지 말고 검증기 출력의 `sha256`을 쓴다(RFC-Q1).

## 2. 스키마 추가분 (C3 검토 대응)

기존 스키마를 깨지 않고 비트마다 6개 필드를 더했다.

| 필드 | 형 | 목적 |
|---|---|---|
| `activityBudget` | `{exploration,reasoning,manipulation,dialogue,payoff}` | 분이 **무엇에 쓰이는지** 분해. 합 = `minutes` (검증기 강제) |
| `authorEstimateBasis` | string | 그 분을 어떻게 잡았는지 한 문장 |
| `subtasks` | string[3~5] | 기존 도구·장소에 묶인 실제 하위 작업 |
| `timeConfidence` | `'low'｜'medium'` | 근거가 약하면 낮게 적는다. **`high`는 쓰지 않았다 — 표본이 0이라 쓸 수 없다** |
| `proofRequired` | bool | 확정을 생산하는 비트만 true |
| `toolTeaching` | `{tool,mode:'guided'｜'unguided'}[]` | 12개 훈련 이벤트를 검증기가 셀 수 있게 명시 |
| `zoneId` (**2026-09-10 · R4 추가**) | `'hub'｜'gate'｜'pump'｜'dock'｜'lowland'` | **비트 단위 구역**. 값은 소속 스테이지 `zoneIds`의 부분집합(검증기 `Z-01`), 33/33 전건 존재(`Z-02`). 이 필드가 비트-구역 매핑의 **단일 출처**이며 `planning/content-matrix.md` §3은 여기서 파생한다(C3-F22) |

단서에는 `originId`와 `copiedFrom`이 추가됐다. **매체 종류만으로는 독립이 아니다** — `council-copybook`은 `tide-ledger-bureau`의 사본이고 `bureau-copy-index`는 그 사본철의 파생이라 같은 비트에서 서로를 뒷받침하지 못하며 검증기가 그 그림자 관계를 배제한다. 독립 2출처 + 2매체는 `proofRequired: true`인 15개 비트에만 요구한다(C3 비차단 관찰 2 대응).

## 3. 시간 정의 (7개 문서 공통, 구 분배 폐기)

`[25, 50, 55, 65, 65, 70, 75, 65, 10] = 480`. T0 −5, E0 −15로 회수한 20분을 C3·C5·C6·C7에 **각 +5분씩 실제 검증 작업**으로 배정했다: C3 반증 정합 시험(`c3-b3`), C5 사본 무결성 대조(`c5-b3`), C6 접점 대체가설 배제(`c6-b3`), C7 반대안 반증 운전(`c7-b2`). 비퍼즐 162 → **149분**(exploration 22 / dialogue 69 / payoff 58)이며 전사(轉寫)만 하는 액션은 남지 않았다.

`designMinutes=480`은 [TARGET], `observedMedianMinutes=null`·`humanPlaytests=[]`는 [OBSERVED] 미측정. `fastMinutes` 합 **322**, `deliberateMinutes` 합 **673**은 **시나리오 경계**이지 표본 중앙값이 아니다.

## 4. 검증 영수증 — **기계 검사**로 재도출 [OBSERVED] (2026-09-10)

이전 판의 §4는 생성기 자체 검사 결과를 **손으로 옮겨 적은 표**였고, 그래서 live 파일과 어긋났다
(`qa/c3-review.md` C3-F11: 매체 2종 미달 1건·`tools: []` 5건이 "PASS"로 적혀 있었다).
본 판은 그 표를 없애고 **재실행 가능한 검증기**로 교체한다. 소유·경로: `_workspace/current/planning/validate-campaign.mjs` (game-planner, Node 내장 모듈만 사용).

### 4.1 실행 명령과 환경 [OBSERVED]

```
$ node --version
v26.8.1
$ node _workspace/current/planning/validate-campaign.mjs ; echo "exit=$?"
exit=0
$ shasum -a 256 _workspace/current/planning/campaign.json
8a43d334f8f69d6642a74708e7b3d68353c7982fb5f506dbba291021a538a93a  _workspace/current/planning/campaign.json
$ wc -c _workspace/current/planning/campaign.json
  124007 _workspace/current/planning/campaign.json
$ node _workspace/current/planning/validate-campaign.mjs --pairs ; echo "exit=$?"
exit=0
```

실행 시각 2026-09-10 (R7 회차). 종료 코드 **0** = **49개** 검사 전건 PASS.
R7에서 검사 2건이 늘었다 — **`Z-03`**(본문 `subtasks[0]`이 말하는 구역 ∋ `beat.zoneId`, C6-F17)와 **`H-04`**(1단 힌트 어휘 제약, C6-F3). `--pairs`는 A37 파생용 출력이며 `proofRequired` **17비트**(RFC-N6로 15 → 17) 전건이 독립쌍을 갖는다(`beatsWithoutPair: []`).

**새 검사 2종이 고치기 전에 실제로 빨간 상태를 만들었는가** [OBSERVED · 재현 명령 포함]: 새 검사가 이미 통과하는 데이터 위에 얹힌 장식이 아님을 보이기 위해, R7 편집 **전** 값(힌트 27건·`zoneId` 2건·본문 2건)만 되돌린 이미지를 스크래치에 만들어 같은 검증기를 돌렸다.

```
$ node _workspace/current/planning/validate-campaign.mjs <R7 편집 전 이미지> ; echo "exit=$?"
exit=1
  Z-03 FAIL  actual = ["c1-b2:hub≠gate","c2-b4:gate≠hub","c5-b2:dock≠lowland","c6-b4:pump≠hub"]
  H-04 FAIL  actual = 24비트 (t0-b1 t0-b2 t0-b3 c1-b2 c1-b3 c1-b4 c2-b1 c2-b2 c2-b3 c2-b4
             c3-b1 c3-b2 c3-b3 c4-b1 c5-b1 c5-b2 c5-b4 c6-b1 c6-b2 c6-b3 c6-b4 c7-b1 c7-b2 c7-b3)
  나머지 47건 PASS
```

즉 **Z-03은 4비트, H-04는 24비트를 편집 전에 잡아냈다**. 재작성한 `hints[0]`은 27비트로 24비트보다 3건 많다 — `c3-b4`·`c4-b3`·`c4-b4`는 어휘로는 걸리지 않지만 판정단·QA가 **정답 통찰 유출**로 지목한 비트라 함께 고쳤다(그 3건은 H-04가 증명하지 못하는 부류다 — §4.4).

### 4.2 검사 49건 (출력의 `checks` 배열을 표로 옮긴 것)

| id | 검사 | expected | actual | 판정 |
|---|---|---|---|---|
| `F-01` | schemaVersion = 1 | `1` | `1` | **PASS** |
| `F-02` | designMinutes = 480 (문서 상수 [TARGET]) | `480` | `480` | **PASS** |
| `F-03` | observedMedianMinutes = null (실측 n=0) | `null` | `null` | **PASS** |
| `F-04` | humanPlaytests = [] (모집·실행 0회) | `0` | `0` | **PASS** |
| `S-01` | 스테이지 9개 | `9` | `9` | **PASS** |
| `S-02` | 스테이지 분 배분 | `[25, 50, 55, 65, 65, 70, 75, 65, 10]` | `[25, 50, 55, 65, 65, 70, 75, 65, 10]` | **PASS** |
| `S-03` | 스테이지 분 총합 480 | `480` | `480` | **PASS** |
| `S-04` | 비트 분 총합 480 | `480` | `480` | **PASS** |
| `S-05` | 스테이지별 (분 = 소속 비트 분 합) | `[25, 50, 55, 65, 65, 70, 75, 65, 10]` | `[25, 50, 55, 65, 65, 70, 75, 65, 10]` | **PASS** |
| `S-06` | 스테이지 zoneIds ⊂ 고정 5구역 | `["hub", "gate", "pump", "dock", "lowland"]` | `["hub", "gate", "pump", "dock", "lowland"]` | **PASS** |
| `Z-01` | beat.zoneId ∈ 소속 stage.zoneIds | `0` | `[]` | **PASS** |
| `Z-02` | 전 비트 zoneId 존재 | `0` | `[]` | **PASS** |
| `Z-03` | 본문(subtasks[0])이 말하는 구역 ∋ beat.zoneId (구역 무언급은 대상 외) | `0` | `[]` | **PASS** |
| `B-01` | 비트 33개 | `33` | `33` | **PASS** |
| `B-02` | 스테이지별 비트 수 (T0 3 · C1~C7 4 · E0 2) | `{"t0":3, "c1":4, "c2":4, "c3":4, "c4":4, "c5":4, "c6":4, "c7":4, "e0":2}` | `{"t0":3, "c1":4, "c2":4, "c3":4, "c4":4, "c5":4, "c6":4, "c7":4, "e0":2}` | **PASS** |
| `B-03` | beat id 유일 | `33` | `33` | **PASS** |
| `B-04` | clue id 유일 | `73` | `73` | **PASS** |
| `B-05` | checkpoint id 유일 | `33` | `33` | **PASS** |
| `B-06` | kind 분포 | `{"puzzle":21, "exploration":2, "dialogue":5, "payoff":5}` | `{"exploration":2, "puzzle":21, "dialogue":5, "payoff":5}` | **PASS** |
| `T-01` | activityBudget 5키 · 음수 없음 · 합 = minutes | `0` | `[]` | **PASS** |
| `T-02` | activityBudget 범주 합 = 480 | `480` | `480` | **PASS** |
| `T-03` | fastMinutes < minutes < deliberateMinutes | `0` | `[]` | **PASS** |
| `T-04` | timeConfidence ∈ {low, medium} ('high' 금지 — 표본 0) | `0` | `[]` | **PASS** |
| `T-05` | authorEstimateBasis 전건 존재 | `0` | `[]` | **PASS** |
| `T-06` | subtasks 3~5개 | `0` | `[]` | **PASS** |
| `H-01` | 힌트 3단 (빈 문자열 없음) | `0` | `[]` | **PASS** |
| `H-02` | recovery 비어있지 않음 | `0` | `[]` | **PASS** |
| `H-03` | checkpoint 존재 | `0` | `[]` | **PASS** |
| `H-04` | 1단 힌트에 자료명·정답값·정답 단정·조작 지시 없음 (gdd §6 1단 정의) | `0` | `[]` | **PASS** |
| `C-01` | clue sourceType ∈ {log, ledger, plate} | `0` | `[]` | **PASS** |
| `C-02` | clue originId 전건 부착 | `0` | `[]` | **PASS** |
| `C-03` | copiedFrom 참조가 카탈로그 안에 있음 | `0` | `[]` | **PASS** |
| `C-04` | originId → copiedFrom 지도 모순 없음 | `0` | `[]` | **PASS** |
| `C-05` | copiedFrom 계보에 순환 없음 | `0` | `[]` | **PASS** |
| `C-06` | 모든 비트 매체 종류 ≥ 2 | `0` | `[]` | **PASS** |
| `C-07` | proofRequired 비트의 독립 쌍 (루트 originId 상이 AND sourceType 상이) | `0` | `[]` | **PASS** |
| `V-01` | 도구 id 고정 6종 | `0` | `[]` | **PASS** |
| `V-02` | toolTeaching 총 12건 | `12` | `12` | **PASS** |
| `V-03` | 도구 6종 각각 guided 1 + unguided 1 | `0` | `[]` | **PASS** |
| `V-04` | toolTeaching의 도구가 그 비트 tools에 포함 | `true` | `[]` | **PASS** |
| `P-01` | prerequisites 참조 비트 존재 | `0` | `[]` | **PASS** |
| `P-02` | prerequisites 전건이 앞선 비트를 가리킴 | `0` | `[]` | **PASS** |
| `P-03` | prerequisites 무순환 | `false` | `false` | **PASS** |
| `K-01` | 기록 불가 명제 문자열 부재 (이전 C3 F1 회귀) | `0` | `[]` | **PASS** |
| `K-02` | '봉인 완료 접점' 존재 | `0` | `[]` | **PASS** |
| `K-03` | RFC-P3-013 캐논 시각 존재 (H-1:24 · H-1:04 · H+0:12) | `0` | `[]` | **PASS** |
| `K-04` | RFC-P3-013 폐기 시각 부재 (H-1:20 · H+0:10) | `0` | `[]` | **PASS** |
| `K-05` | RFC-P3-012 공개 순서 (도연 = t0-b1 · 한서린 = c4-b2) | `{"도연":"t0-b1", "한서린":"c4-b2"}` | `{"도연":"t0-b1", "한서린":"c4-b2"}` | **PASS** |
| `K-06` | RFC-W4 의도 문장 위치 ('방패가 아니라 잠금장치' — c4-b3 부재 · c6-b4 존재) | `{"c4-b3":false, "c6-b4":true}` | `{"c4-b3":false, "c6-b4":true}` | **PASS** |

### 4.3 출력 원문 [OBSERVED]

아래는 위 명령의 stdout **전문**이다. 표 4.2는 이 JSON의 `checks`를 옮긴 것이며 값을 가공하지 않았다.

```json
{
  "validator": "planning/validate-campaign.mjs",
  "contract": "RFC-P3-008 계보 B (live planning/campaign.json)",
  "file": "/Users/jangyoung/orca/unknown/_workspace/current/planning/campaign.json",
  "bytes": 124007,
  "sha256": "8a43d334f8f69d6642a74708e7b3d68353c7982fb5f506dbba291021a538a93a",
  "summary": {
    "checks": 49,
    "pass": 49,
    "fail": 0,
    "verdict": "PASS"
  },
  "aggregates": {
    "stages": 9,
    "stageMinutes": [
      25,
      50,
      55,
      65,
      65,
      70,
      75,
      65,
      10
    ],
    "totalMinutes": 480,
    "beats": 33,
    "kindCounts": {
      "exploration": 2,
      "puzzle": 21,
      "dialogue": 5,
      "payoff": 5
    },
    "clues": 73,
    "sourceTypeDist": {
      "log": 27,
      "ledger": 22,
      "plate": 24
    },
    "originCatalogSize": 31,
    "activityBudgetSums": {
      "exploration": 53,
      "reasoning": 190,
      "manipulation": 168,
      "dialogue": 28,
      "payoff": 41
    },
    "fastMinutesSum": 322,
    "deliberateMinutesSum": 673,
    "toolBeatCounts": {
      "circuit": 10,
      "reader": 11,
      "alignment": 8,
      "routing": 3,
      "corrosion": 3,
      "seal": 7
    },
    "toolBeatMinutes": {
      "circuit": 149,
      "reader": 165,
      "alignment": 141,
      "routing": 52,
      "corrosion": 46,
      "seal": 108
    },
    "toollessBeats": [
      "t0-b1",
      "c1-b4",
      "c3-b4",
      "c7-b1",
      "e0-b1"
    ],
    "proofRequiredBeats": 17,
    "zoneBeatCounts": {
      "hub": 17,
      "gate": 2,
      "pump": 5,
      "dock": 5,
      "lowland": 4
    },
    "zoneBeatMinutes": {
      "hub": 218,
      "gate": 26,
      "pump": 85,
      "dock": 84,
      "lowland": 67
    },
    "zoneStageSpan": {
      "hub": 7,
      "gate": 2,
      "pump": 2,
      "dock": 2,
      "lowland": 2
    },
    "zoneRuns": {
      "hub": 5,
      "gate": 2,
      "pump": 2,
      "dock": 2,
      "lowland": 3
    },
    "zoneRunCount": 14,
    "zoneRunSequence": [
      "hub:t0-b1→t0-b3(3)",
      "gate:c1-b1→c1-b1(1)",
      "hub:c1-b2→c1-b4(3)",
      "pump:c2-b1→c2-b2(2)",
      "gate:c2-b3→c2-b3(1)",
      "hub:c2-b4→c2-b4(1)",
      "dock:c3-b1→c3-b4(4)",
      "lowland:c4-b1→c4-b1(1)",
      "hub:c4-b2→c4-b4(3)",
      "lowland:c5-b1→c5-b1(1)",
      "dock:c5-b2→c5-b2(1)",
      "lowland:c5-b3→c5-b4(2)",
      "pump:c6-b1→c6-b3(3)",
      "hub:c6-b4→e0-b2(7)"
    ],
    "mediaKindsPerBeatMin": 2,
    "timeConfidenceCounts": {
      "low": 12,
      "medium": 21,
      "high": 0
    }
  },
  "checks": [
    {
      "id": "F-01",
      "name": "schemaVersion = 1",
      "status": "PASS",
      "expected": 1,
      "actual": 1
    },
    {
      "id": "F-02",
      "name": "designMinutes = 480 (문서 상수 [TARGET])",
      "status": "PASS",
      "expected": 480,
      "actual": 480
    },
    {
      "id": "F-03",
      "name": "observedMedianMinutes = null (실측 n=0)",
      "status": "PASS",
      "expected": null,
      "actual": null
    },
    {
      "id": "F-04",
      "name": "humanPlaytests = [] (모집·실행 0회)",
      "status": "PASS",
      "expected": 0,
      "actual": 0
    },
    {
      "id": "S-01",
      "name": "스테이지 9개",
      "status": "PASS",
      "expected": 9,
      "actual": 9
    },
    {
      "id": "S-02",
      "name": "스테이지 분 배분",
      "status": "PASS",
      "expected": [
        25,
        50,
        55,
        65,
        65,
        70,
        75,
        65,
        10
      ],
      "actual": [
        25,
        50,
        55,
        65,
        65,
        70,
        75,
        65,
        10
      ]
    },
    {
      "id": "S-03",
      "name": "스테이지 분 총합 480",
      "status": "PASS",
      "expected": 480,
      "actual": 480
    },
    {
      "id": "S-04",
      "name": "비트 분 총합 480",
      "status": "PASS",
      "expected": 480,
      "actual": 480
    },
    {
      "id": "S-05",
      "name": "스테이지별 (분 = 소속 비트 분 합)",
      "status": "PASS",
      "expected": [
        25,
        50,
        55,
        65,
        65,
        70,
        75,
        65,
        10
      ],
      "actual": [
        25,
        50,
        55,
        65,
        65,
        70,
        75,
        65,
        10
      ]
    },
    {
      "id": "S-06",
      "name": "스테이지 zoneIds ⊂ 고정 5구역",
      "status": "PASS",
      "expected": [
        "hub",
        "gate",
        "pump",
        "dock",
        "lowland"
      ],
      "actual": [
        "hub",
        "gate",
        "pump",
        "dock",
        "lowland"
      ]
    },
    {
      "id": "Z-01",
      "name": "beat.zoneId ∈ 소속 stage.zoneIds",
      "status": "PASS",
      "expected": 0,
      "actual": []
    },
    {
      "id": "Z-02",
      "name": "전 비트 zoneId 존재",
      "status": "PASS",
      "expected": 0,
      "actual": []
    },
    {
      "id": "Z-03",
      "name": "본문(subtasks[0])이 말하는 구역 ∋ beat.zoneId (구역 무언급은 대상 외)",
      "status": "PASS",
      "expected": 0,
      "actual": []
    },
    {
      "id": "B-01",
      "name": "비트 33개",
      "status": "PASS",
      "expected": 33,
      "actual": 33
    },
    {
      "id": "B-02",
      "name": "스테이지별 비트 수 (T0 3 · C1~C7 4 · E0 2)",
      "status": "PASS",
      "expected": {
        "t0": 3,
        "c1": 4,
        "c2": 4,
        "c3": 4,
        "c4": 4,
        "c5": 4,
        "c6": 4,
        "c7": 4,
        "e0": 2
      },
      "actual": {
        "t0": 3,
        "c1": 4,
        "c2": 4,
        "c3": 4,
        "c4": 4,
        "c5": 4,
        "c6": 4,
        "c7": 4,
        "e0": 2
      }
    },
    {
      "id": "B-03",
      "name": "beat id 유일",
      "status": "PASS",
      "expected": 33,
      "actual": 33
    },
    {
      "id": "B-04",
      "name": "clue id 유일",
      "status": "PASS",
      "expected": 73,
      "actual": 73
    },
    {
      "id": "B-05",
      "name": "checkpoint id 유일",
      "status": "PASS",
      "expected": 33,
      "actual": 33
    },
    {
      "id": "B-06",
      "name": "kind 분포",
      "status": "PASS",
      "expected": {
        "puzzle": 21,
        "exploration": 2,
        "dialogue": 5,
        "payoff": 5
      },
      "actual": {
        "exploration": 2,
        "puzzle": 21,
        "dialogue": 5,
        "payoff": 5
      }
    },
    {
      "id": "T-01",
      "name": "activityBudget 5키 · 음수 없음 · 합 = minutes",
      "status": "PASS",
      "expected": 0,
      "actual": []
    },
    {
      "id": "T-02",
      "name": "activityBudget 범주 합 = 480",
      "status": "PASS",
      "expected": 480,
      "actual": 480,
      "note": "{\"exploration\":53,\"reasoning\":190,\"manipulation\":168,\"dialogue\":28,\"payoff\":41}"
    },
    {
      "id": "T-03",
      "name": "fastMinutes < minutes < deliberateMinutes",
      "status": "PASS",
      "expected": 0,
      "actual": []
    },
    {
      "id": "T-04",
      "name": "timeConfidence ∈ {low, medium} ('high' 금지 — 표본 0)",
      "status": "PASS",
      "expected": 0,
      "actual": []
    },
    {
      "id": "T-05",
      "name": "authorEstimateBasis 전건 존재",
      "status": "PASS",
      "expected": 0,
      "actual": []
    },
    {
      "id": "T-06",
      "name": "subtasks 3~5개",
      "status": "PASS",
      "expected": 0,
      "actual": []
    },
    {
      "id": "H-01",
      "name": "힌트 3단 (빈 문자열 없음)",
      "status": "PASS",
      "expected": 0,
      "actual": []
    },
    {
      "id": "H-02",
      "name": "recovery 비어있지 않음",
      "status": "PASS",
      "expected": 0,
      "actual": []
    },
    {
      "id": "H-03",
      "name": "checkpoint 존재",
      "status": "PASS",
      "expected": 0,
      "actual": []
    },
    {
      "id": "H-04",
      "name": "1단 힌트에 자료명·정답값·정답 단정·조작 지시 없음 (gdd §6 1단 정의)",
      "status": "PASS",
      "expected": 0,
      "actual": [],
      "note": "어휘 바닥 검사 — 의미 유출 일반은 판정하지 않는다"
    },
    {
      "id": "C-01",
      "name": "clue sourceType ∈ {log, ledger, plate}",
      "status": "PASS",
      "expected": 0,
      "actual": []
    },
    {
      "id": "C-02",
      "name": "clue originId 전건 부착",
      "status": "PASS",
      "expected": 0,
      "actual": []
    },
    {
      "id": "C-03",
      "name": "copiedFrom 참조가 카탈로그 안에 있음",
      "status": "PASS",
      "expected": 0,
      "actual": []
    },
    {
      "id": "C-04",
      "name": "originId → copiedFrom 지도 모순 없음",
      "status": "PASS",
      "expected": 0,
      "actual": []
    },
    {
      "id": "C-05",
      "name": "copiedFrom 계보에 순환 없음",
      "status": "PASS",
      "expected": 0,
      "actual": []
    },
    {
      "id": "C-06",
      "name": "모든 비트 매체 종류 ≥ 2",
      "status": "PASS",
      "expected": 0,
      "actual": []
    },
    {
      "id": "C-07",
      "name": "proofRequired 비트의 독립 쌍 (루트 originId 상이 AND sourceType 상이)",
      "status": "PASS",
      "expected": 0,
      "actual": [],
      "note": "proofRequired 비트 17건"
    },
    {
      "id": "V-01",
      "name": "도구 id 고정 6종",
      "status": "PASS",
      "expected": 0,
      "actual": []
    },
    {
      "id": "V-02",
      "name": "toolTeaching 총 12건",
      "status": "PASS",
      "expected": 12,
      "actual": 12
    },
    {
      "id": "V-03",
      "name": "도구 6종 각각 guided 1 + unguided 1",
      "status": "PASS",
      "expected": 0,
      "actual": [],
      "note": "{\"circuit\":{\"guided\":[\"t0-b2\"],\"unguided\":[\"c6-b2\"]},\"reader\":{\"guided\":[\"t0-b3\"],\"unguided\":[\"c1-b2\"]},\"alignment\":{\"guided\":[\"c3-b2\"],\"unguided\":[\"c6-b3\"]},\"routing\":{\"guided\":[\"c5-b2\"],\"unguided\":[\"c7-b2\"]},\"corrosion\":{\"guided\":[\"c2-b2\"],\"unguided\":[\"c4-b2\"]},\"seal\":{\"guided\":[\"c1-b3\"],\"unguided\":[\"c4-b3\"]}}"
    },
    {
      "id": "V-04",
      "name": "toolTeaching의 도구가 그 비트 tools에 포함",
      "status": "PASS",
      "expected": true,
      "actual": []
    },
    {
      "id": "P-01",
      "name": "prerequisites 참조 비트 존재",
      "status": "PASS",
      "expected": 0,
      "actual": []
    },
    {
      "id": "P-02",
      "name": "prerequisites 전건이 앞선 비트를 가리킴",
      "status": "PASS",
      "expected": 0,
      "actual": []
    },
    {
      "id": "P-03",
      "name": "prerequisites 무순환",
      "status": "PASS",
      "expected": false,
      "actual": false
    },
    {
      "id": "K-01",
      "name": "기록 불가 명제 문자열 부재 (이전 C3 F1 회귀)",
      "status": "PASS",
      "expected": 0,
      "actual": []
    },
    {
      "id": "K-02",
      "name": "'봉인 완료 접점' 존재",
      "status": "PASS",
      "expected": 0,
      "actual": []
    },
    {
      "id": "K-03",
      "name": "RFC-P3-013 캐논 시각 존재 (H-1:24 · H-1:04 · H+0:12)",
      "status": "PASS",
      "expected": 0,
      "actual": []
    },
    {
      "id": "K-04",
      "name": "RFC-P3-013 폐기 시각 부재 (H-1:20 · H+0:10)",
      "status": "PASS",
      "expected": 0,
      "actual": []
    },
    {
      "id": "K-05",
      "name": "RFC-P3-012 공개 순서 (도연 = t0-b1 · 한서린 = c4-b2)",
      "status": "PASS",
      "expected": {
        "도연": "t0-b1",
        "한서린": "c4-b2"
      },
      "actual": {
        "도연": "t0-b1",
        "한서린": "c4-b2"
      }
    },
    {
      "id": "K-06",
      "name": "RFC-W4 의도 문장 위치 ('방패가 아니라 잠금장치' — c4-b3 부재 · c6-b4 존재)",
      "status": "PASS",
      "expected": {
        "c4-b3": false,
        "c6-b4": true
      },
      "actual": {
        "c4-b3": false,
        "c6-b4": true
      }
    }
  ],
  "notMeasured": [
    "사람 플레이 표본 n=0 — 완주 분·힌트 실효성·도달성·이탈률 전부 미측정",
    "이 검증기는 문서 정합만 본다. G2/G4/G5/G6/G7을 올리지 않는다"
  ]
}
```

### 4.4 이 영수증이 증명하지 않는 것 [OBSERVED]

49/49 PASS는 **JSON이 문서 계약과 자기 자신에 대해 무모순**이라는 뜻뿐이다.
`Z-01`·`Z-02`는 "`zoneId`가 스테이지 구역 목록 안에 있다"만 증명한다 — **그 구역이 그 장면의 옳은 무대인지는 검사하지 않는다**(도출 근거와 그 한계는 §5.1). R7이 더한 `Z-03`은 그 구멍을 **첫 하위과제 한 줄**까지만 좁힌다 — 뒤 하위과제·`consequence`의 구역 이동, 구역 명사를 아예 쓰지 않는 비트(현재 **16**비트 — `node`로 `subtasks[0]` 전수 스캔한 실측)는 여전히 사람이 읽어야 한다.
`H-04`는 **어휘 바닥**이다. 자료명·정답값·정답 단정·조작 지시라는 네 부류의 어휘만 막으며, 금지 어휘를 피하면서 문장으로 정답 통찰을 흘리는 1단은 잡지 못한다(`c3-b4`·`c4-b3`·`c4-b4`가 그 예였고 사람 검토로 고쳤다). **힌트가 실제로 도움이 되는지·1단만으로 풀리는지는 표본 n=0이라 미측정이다.**
비트가 재미있는지·도달 가능한지·8~19분이 맞는지는 검사하지 않으며 사람 플레이 표본은 **n = 0**이다.
검증기는 G2/G4/G5/G6/G7 중 어느 것도 올리지 않는다.

## 5. C3 검토 5건 처리

- **F1** 확정 대상을 사람의 '서명 확인'에서 **기계 각인 두 개의 선후**(밸브 개폐 H-1:24 → 봉인 완료 접점 H-1:04, 간격 20분 > 총 오차폭 8분)로 교체. 씨앗은 `c1-b3` 봉인대 훈련. 동기는 **양수장 정비 대장**과 **항만공사 증설 심사철** 두 기관의 서류로 뒷받침한다(같은 사건의 다른 파일 형식이 아님). 세 결말의 의미 불변.
- **F2** §3. 모든 dialogue/payoff 비트가 대조표·교차 검증·결과 예고 열람 중 하나를 산출물로 낸다. 강제 대기·읽기 패딩·시간 끌기용 되돌림 0건.
- **F3** `t0-b1`의 인수 각서가 '한도연'을 당직 주임으로 기재 → `c1-b4` 가설이 문서 근거를 얻는다. 서명란의 글자는 `c4-b2`까지 미공개.
- **F4** `c3-b1`이 **NPC 대화 전에** 대장을 판독해 만조 피크 3개를 자동 사본으로 증거함에 넣고, 대장 우회로를 삭제했다. `c3-b2`의 331분 잠김 경로가 닫혔다.
- **F5** 판 #0의 소유를 서린으로 되돌리고 6개 비트에서 실제 입력으로 쓴다.

**자체 발견**: 침수 시각 H+0:10은 4분 격자에 얹히지 않아 조위대장 기입일 수 없다 → **H+0:12**.

### 5.1 C3 종료 판정 묶음 처리 (2026-09-10 · R4) [OBSERVED]

디렉터 판정 **C3-F22(비트 단위 `zoneId`) · RFC-W4(R2 의도 공개 시점) · C3-F29(`c1-b4` 구역 오기)** 세 건을 데이터에 반영했다. 편집은 이 셋뿐이다.

**(1) `zoneId` 도출 규칙** — 값은 손으로 고르지 않고 아래 사다리를 위에서부터 적용했다. 도출 입력은 **비트 본문(`objective` · `action` · `subtasks` · `completion`)의 장소 서술**이며, 문서 표가 아니다(C3-F22 결정: 단일 출처 = JSON).

| 규칙 | 정의 | 적용 비트 | 수 |
|---|---|---|---:|
| **R0** | 디렉터 판정이 지정한 구역 | `c1-b4` → `hub` (C3-F29) | 1 |
| **R1** | 스테이지 `zoneIds`가 1개면 그 구역 | T0 3 · C3 4 · C6 4 · C7 4 · E0 2 | 17 |
| **R2** | 본문이 스테이지 구역 중 **정확히 하나**를 장소로 지시 | `c1-b1` gate(제3수문 순찰로) · `c1-b3` hub(봉인대) · `c2-b1` pump(제1양수장 수로 입구) · `c2-b2` pump(지하수로 시험대) · `c4-b1` lowland(저지대 주민회 사무실) · `c4-b2` hub(작업대) · `c4-b3` hub(당직실 봉인대) · `c4-b4` hub(당직실 수화기) · `c5-b3` lowland(주민회 보관 일지) | 9 |
| **R3** | 본문이 스테이지 구역 **둘을 함께** 지시하거나 장소 서술이 **없음** → 스테이지 `zoneIds[0]` | `c1-b2` hub(제3수문 판독대 + 작업대) · `c2-b3` gate(장소 서술 없음) · `c2-b4` gate(본문은 스테이지 밖 구역을 지시 — 아래 (4)) · `c5-b1` lowland(저지대 분기 + 부두 분기) · `c5-b4` lowland(장소 서술 없음) | 5 |
| **R4 (예외 1건)** | 본문에 장소 서술이 없고 **기존 문서 전건이 단일 구역으로 일치**하면 그 값을 쓴다 | `c5-b2` → `dock` — `synopsis/chapter-beats.md` 표 A(B21 `dock` D1 연습) · `content-matrix.md` §3 · `campaign-time-budget.md` 표 1·표 2 **4문서 일치**. R3를 적용하면 `lowland`가 되어 C5에서 `dock` 배정이 0이 되고 구역 상태 D1(대기)이 어느 비트에도 걸리지 않는다 | 1 |

합 33 [OBSERVED: 1+17+9+5+1]. **R4는 디렉터 확인 대상**이다 — 지시된 fallback("애매하면 스테이지 첫 구역")을 그대로 쓰면 `c5-b2`는 `lowland`가 되며, 그 경우 §4.3·`content-matrix` §1의 dock 분이 84 → 65, lowland 67 → 86으로 바뀐다. 두 값 모두 총합 480을 유지한다.

**(2) RFC-W4** — `c4-b3.inference`에서 의도 문장을 제거하고 효력 판정만 남겼다.
`- 그 이름은 규정상 서명 자격이 없다. … 집행 요건을 갖추지 못한다. **그것은 방패가 아니라 잠금장치다.**`
`+ 그 이름은 규정상 서명 자격이 없다. … 집행 요건을 갖추지 못한다.`
같은 문장을 `c6-b4.inference` 끝에 붙였다 — 주어를 잇기 위해 연결절 한 개를 더했다(원문 표현은 그대로): `… 필요해서 쓰인 것이다. **도연이 고른 무효 서명도 같은 성격이다 — 그것은 방패가 아니라 잠금장치다.**` 검증기 `K-06`이 이 위치를 잠근다.

**(3) C3-F29** — `c1-b4.subtasks[0]` "**제3수문에서** 재화의 진술 3항을 받아…" → "**당직실에서** 재화의 진술 3항을 받아…". 비트 `zoneId`(`hub`)와 본문이 이제 같은 곳을 가리킨다.

**(4) 남은 텍스트-구역 모순 5건 [OBSERVED · 이번 회차에서 고치지 않음]** — 스테이지 `zoneIds`나 다른 레인 문서를 건드려야 하므로 데이터만으로 닫히지 않는다.

| # | 비트 | 관측 | 왜 남겼나 |
|---|---|---|---|
| 1 | `c2-b4` | 본문 "**당직실**에서 12년치 근무표를…" = `hub`인데 `C2.zoneIds = [gate, pump]`에 `hub`가 없다 | `zoneId`는 R3로 `gate`. 스테이지 `zoneIds` 확장은 이번 회차 편집 범위 밖(디렉터 지시: 다른 값 변경 금지). synopsis 표 A도 `gate` G0으로 적는다 → 본문 문구 정정이 최소 수정 |
| 2 | `c6-b4` | 본문 "**당직실**에서 조사 종결 보고서를…" = `hub`인데 `C6.zoneIds = [pump]` | 위와 같은 유형. `zoneId`는 R1로 `pump` |
| 3 | `c4-b2` | 본문이 "작업대"(hub)와 "**부식 시험대**"를 함께 쓴다. 부식 시험대는 `c2-b2`에서 **지하수로(pump)**에 있다 | 시험대가 두 곳에 있는지·이동식인지 미확정. systems·worldview 확인 필요 |
| 4 | `c7-b2` | `hub`에서 시작해 `gate` G3로 **이동해 시연**한다 | 단일 `zoneId`로는 이동이 표현되지 않는다. `zoneId = hub`(스테이지 유일 구역)이며 gate 등장은 문서 주석으로만 남는다 |
| 5 | `c5-b1` · `c5-b4` | 두 구역을 **동시에** 다루는 제로섬 비트 | 단일 `zoneId`는 **주 무대**만 표시한다. 교차 구역은 `content-matrix.md` §3의 별도 열이 담는다 |

**(5) 이 편집이 바꾸지 않은 것 — 회귀 영수증 [OBSERVED]**

```
# 편집 전 파일을 역산으로 복원해 바이트 동일성을 확인했다
$ node -e '... zoneId 제거 + 세 문장 원복 ...' > campaign.prev.json
$ shasum -a 256 campaign.prev.json
fdabf1d421932d679429870a06eeb31e9527562370c6a107aec77c2c884fb2b7   # 편집 전 sha와 일치
$ wc -c campaign.prev.json
  120479                                                           # 편집 전 bytes와 일치
$ node validate-campaign.mjs campaign.prev.json   # aggregates 비교
차이가 난 키: zoneBeatCounts · zoneBeatMinutes · zoneStageSpan · zoneRuns · zoneRunCount · zoneRunSequence (6개, 전부 신규)
```

즉 `stageMinutes` · `totalMinutes 480` · `kindCounts` · `clues 73` · `sourceTypeDist` · `originCatalogSize 31` · `activityBudgetSums` · `fastMinutesSum 322` · `deliberateMinutesSum 673` · `toolBeatCounts` · `toolBeatMinutes` · `toollessBeats` · `proofRequiredBeats 15`는 **전건 불변**이다. 편집이 가역적이라는 사실(복원본 sha 일치)이 "다른 값을 건드리지 않았다"의 증거다.

**(6) 파생 집계 (검증기 `aggregates`, 이 값이 `content-matrix.md` §1·§3·§4.3의 출처)** [OBSERVED]

| 구역 | 비트 | 분 | D1 = 등장 스테이지 | D2 = 연속 런 |
|---|---:|---:|---:|---:|
| `hub` | 15 | 188 | 5 | 4 |
| `gate` | 3 | 39 | 2 | 2 |
| `pump` | 6 | 102 | 2 | 2 |
| `dock` | 5 | 84 | 2 | 2 |
| `lowland` | 4 | 67 | 2 | 3 |
| **계** | **33** | **480** | **13** | **13** |

D2 런 순서(`zoneRunSequence`)도 출력에 있다. 이전 판 §1이 적던 D2 **16**은 비트-구역 배정이 문서 표에 있던 때의 값이며, 이제 D2는 **데이터에서 기계로 세는 값**이다.

**[R7 갱신 고지]** 위 표는 **R4 시점** 값이다(역사 기록이므로 고치지 않는다). C6-F17 구역 편집 이후의 값은 **§5.2 (5)** 표가 정본이다.

### 5.2 C6/C7 디렉터 판정 묶음 처리 (2026-09-10 · R7) [OBSERVED]

배정 근거: `production/decision-log.md` **「C6/C7 디렉터 판정 묶음」** — `RFC-C7-001`(3) · `RFC-N6`(구 RFC-S4 재번호) · 레인 배정 표(**C6-F1 · C6-F3 · C6-F17**). 이 회차의 데이터 편집은 아래 다섯 종류뿐이다.

**(1) 무엇을 왜 바꿨는가**

| # | 결함 / RFC | 편집 | 대상 |
|---|---|---|---|
| ① | **C6-F1** | `t0-b1.objective`에 오늘 밤의 산출물 문장 추가 + `t0-b1-c1.description`에 그 근거 1문장 보강 | `t0-b1` |
| ② | **C6-F3** | `hints[0]` 재작성 — 1단(방향) 정의 위반 전수 | 27비트 |
| ③ | **C6-F17** | `c2-b4`·`c6-b4`의 `zoneId`를 본문에 맞추고 스테이지 `zoneIds` 확장, `c1-b2`·`c5-b2` 본문 구역 정정 | 4비트 + 스테이지 2 |
| ④ | **RFC-N6** | `proofRequired` → `true` (결말 A·B의 근거 비트) | `c1-b2` · `c3-b1` |
| ⑤ | **RFC-C7-001 (3)** | `t0-b1`~`t0-b3`의 `completion`을 **열람 / 표시 / 인용 고정** 술어로 정밀화 | `t0-b1` `t0-b2` `t0-b3` |

**(2) ① C6-F1 — 오늘 밤의 목표가 첫 비트에 없었다**

[OBSERVED] 33비트 전문에서 「청문」 최초 등장은 `c3-b4`, 「제출」은 `c5-b4`였다(`qa/c6-review.md` C6-F1). 첫 25분이 끝날 때까지 "오늘 밤 무엇을 끝내야 하는가"가 데이터에 없었다. 추가한 문장은 `c7-b4.objective`의 캐논 산출물(**청문 제출 문서 1건**)을 T0로 앞당겨 **말하기만** 한다 — 사건의 인과·이름·시각은 그대로 `RFC-P3-012` 상한 아래에 있다(`K-05` PASS 유지: 도연 = `t0-b1` · 한서린 = `c4-b2`).
[INFERENCE] 이 편집은 `worldview/timeline.md` §7 **B01 행의 허용 열**과 `synopsis/continuity.md` 「청문 소집」 행에 인용 갱신을 요구한다 — §5.2 (7) 브로드캐스트.

**(3) ② C6-F3 — 1단 힌트가 2·3단의 것을 주고 있었다**

정본 정의는 `planning/gdd.md` §6 표다: 1단(방향)이 주는 것 = "지금 무엇을 결정해야 하는지, 어느 **구역·도구**가 관련 있는지" / 주지 않는 것 = "**어떤 자료인지**". 2단이 매체 종류와 도구 사용 순서를, 3단이 정확한 자료와 조작 값을 준다.
재작성 원칙: 1단은 **결정 대상 + 구역/도구**만 말한다. 매체·자료명(`염판` `대장` `일지` `각서` `시편` `요약` `보고서` …), 수량·정답값, "X가 아니라 Y"·"…때문이다"·"…만 보라" 형태의 정답 단정, 조작 지시 동사(겹쳐·물려·계산하라·대조하라·세어라 …)를 1단에서 뺐다.
기계화: 검증기에 **`H-04`**를 추가했다(§4.2). 기존 `H-01`은 **빈 문자열만** 검사했으므로 위반이 33/33 PASS 아래에 숨어 있었다.

**(4) ③④ 구역·확정 편집의 before → after**

| 대상 | 필드 | before | after | 근거 |
|---|---|---|---|---|
| `c2-b4` | `zoneId` | `gate` | **`hub`** | 본문 "당직실에서 12년치 근무표…" |
| 스테이지 `C2` | `zoneIds` | `["gate","pump"]` | **`["gate","pump","hub"]`** | 〃 (Z-01 유지 조건) |
| `c6-b4` | `zoneId` | `pump` | **`hub`** | 본문 "당직실에서 조사 종결 보고서…" |
| 스테이지 `C6` | `zoneIds` | `["pump"]` | **`["pump","hub"]`** | 〃 |
| `c1-b2` | `subtasks[0]` | "**제3수문** 판독대에서…" | "**당직실** 판독대에서…" | `zoneId = hub` |
| `c5-b2` | `subtasks[0]` | "편성기에 저지대 우선안…" | "**부두** 편성기에 저지대 우선안…" | `zoneId = dock`(「저지대 우선안」은 안 이름이지 무대가 아니다) |
| `c1-b2` | `proofRequired` | `false` | **`true`** | RFC-N6 (결말 A 근거) |
| `c3-b1` | `proofRequired` | `false` | **`true`** | RFC-N6 (결말 B 근거) |

RFC-N6이 예고한 **C-07 FAIL 시 두 번째 매체 단서 추가**는 **필요 없었다** [OBSERVED] — `--pairs` 실측:

```
$ node _workspace/current/planning/validate-campaign.mjs --pairs
proofRequiredBeats: 17 · beatsWithoutPair: []
c1-b2 → c1-b2-c1 (log · signature-annex) × c1-b2-c2 (plate · plate-zero)
c3-b1 → c3-b1-c1 (ledger · tide-ledger-seongchan) × c3-b1-c2 (log · brine-log-dock)
```

즉 두 비트는 이미 **루트 originId 상이 AND sourceType 상이**인 쌍을 갖고 있었고, `proofRequired`가 `false`였던 것이 데이터가 아니라 **표기의 누락**이었다. 단서는 하나도 만들지 않았다(`clues 73` 불변).

**(5) 회귀 — 무엇이 바뀌지 않았는가** [OBSERVED · `aggregates` 전건 대조]

편집 전 실행(sha `92301c0a…`)과 편집 후 실행(sha `8a43d334…`)의 `aggregates`를 키 단위로 비교했다.

| 상태 | 키 |
|---|---|
| **불변 (16키)** | `stages` `stageMinutes` `totalMinutes 480` `beats 33` `kindCounts` `clues 73` `sourceTypeDist` `originCatalogSize` `activityBudgetSums` `fastMinutesSum 322` `deliberateMinutesSum 673` `toolBeatCounts` `toolBeatMinutes` `toollessBeats` `mediaKindsPerBeatMin` `timeConfidenceCounts` |
| **변화 (7키)** | `proofRequiredBeats` 15 → **17** · `zoneBeatCounts` · `zoneBeatMinutes` · `zoneStageSpan` · `zoneRuns` · `zoneRunCount` 13 → **14** · `zoneRunSequence` |

**분·도구·단서·선행조건·시간 신뢰도는 한 개도 건드리지 않았다.** 변화한 7키는 전부 ③④의 직접 파생이다.

**(6) R7 이후 파생 집계 (검증기 `aggregates` — `content-matrix.md` §1·§3·§4.3의 출처)** [OBSERVED]

| 구역 | 비트 | 분 | D1 = 등장 스테이지 | D2 = 연속 런 |
|---|---:|---:|---:|---:|
| `hub` | 17 | 218 | 7 | 5 |
| `gate` | 2 | 26 | 2 | 2 |
| `pump` | 5 | 85 | 2 | 2 |
| `dock` | 5 | 84 | 2 | 2 |
| `lowland` | 4 | 67 | 2 | 3 |
| **계** | **33** | **480** | **15** | **14** |

[INFERENCE] 허브 비중이 **188분 → 218분(45.4%)**으로 올랐다. 이것은 새 콘텐츠가 아니라 **원래 당직실에서 벌어지던 두 비트가 데이터에서 다른 구역으로 적혀 있던 것을 바로잡은 결과**다 — 「같은 장소가 다르게 읽힌다」 기둥의 부담이 그만큼 커졌다는 뜻이므로 연출·모델링 레인이 허브 상태 변화를 몇 단계로 보여 줄지는 열린 질문으로 남긴다.

**(7) 이 편집이 깨뜨리는 인용 — 브로드캐스트** [OBSERVED]

| 문서 | 무엇이 스테일해졌나 | 소유 |
|---|---|---|
| `planning/content-matrix.md` §1·§3·§4.3 | 구역 파생 집계 6키 전부 | **planner (본 회차 갱신)** |
| `synopsis/chapter-beats.md` `c2-b4`·`c6-b4` 행의 구역 주석 | `gate`/`pump` → `hub` | synopsis |
| `worldview/timeline.md` §7 B01 허용 열 · `synopsis/continuity.md` 「청문 소집」 행 | T0가 청문 제출 문서를 언급하게 됐다 | worldview · synopsis |
| `balance/puzzle-balance.md` 힌트 인용 | `hints[0]` 27건 문구 | balance |
| 고정 sha/바이트를 적은 모든 문서 | `92301c0a…` / 121457 | 각 소유 레인 (RFC-Q1: 검증기 출력을 인용한다) |

## 6. 저자 확인 — 수용하지 않은 검토 주장

> **2026-09-10 처리** [OBSERVED]: 아래 1번은 **RFC-P3-011로 채택**됐다 — 판정 키 `total_minus_afk_min`, 목표 밴드 450~540분, 420/360은 통과선이 아니라 **철회 트리거**, p75 조건 삭제, 322/673은 시나리오 경계. 2·3·4번은 그대로 유효하다.

1. **시간 봉투 420/360/600 기각.** 계약이 정한 것은 관측 중앙값 450~540분이며 이는 표본 확보 후의 값이다. 검토자가 쓴 세 임계는 승인 이력이 없다. 따라서 `fastMinutes` 합을 하한에 맞추려 올리지 않았고 재산정값(322/673)을 그대로 보고한다. 시나리오 경계를 표본 봉투와 비교하는 것 자체가 범주 오류다.
2. **"합계 405~415분" 기각.** 비퍼즐 분을 검토자가 임의 할인한 파생 가정이므로 관측으로 인정하지 않는다. 그 밑의 결함(전사 액션)은 전부 수용해 고쳤다.
3. **F5의 RFC 경로 기각.** 두 갈래 중 바이블 준수 쪽을 택했으므로 캐논 변경 RFC는 불필요하다.
4. `humanPlaytests: []`가 결함이 아니라는 검토자 판단은 **수용**한다.

## 7. 범위 밖으로 남은 상위 캐논 변경 (**2026-09-10 전건 적용 완료**)

> **처리 결과** [OBSERVED]: 아래 7-1·7-2·7-3은 더 이상 "적용 대기"가 아니다.
> - 7-1·7-2 → **RFC-P3-010 + RFC-P3-013**으로 세계관 레인이 아카이브 c3 본문 위에 재기반하며 적용했다. `current/worldview/worldview-bible.md` §2·§3, `worldview/timeline.md` §2가 정본이며 본 절은 **적용 이력**으로만 남는다.
> - 7-3 → **RFC-P3-008**로 계보 B가 유일 정본이 됐고 `planning/gdd.md` §10을 본 절의 문구로 교체했다.
> - 마지막 문단이 디렉터 판정 사항으로 올린 "두 계보가 동시에 current" 상태는 RFC-P3-008이 닫았다. `synopsis/campaign.md`·`scenes-and-dialogue.md`의 F1·F3 서술은 이제 **캠페인 측 제안이 아니라 캐논 정합 서술**이다.
> 아래 원문은 삭제하지 않는다 — 무엇을 어떤 문구로 적용했는지의 증거다.

작업 중 `worldview/worldview-bible.md`(00:10:49, 20668 B) · `worldview/timeline.md`(00:12:37, 15300 B) · `planning/gdd.md`(00:21:42, 18585 B)가 **다른 세션에 의해 다른 계보(`cycle: c3` / `status: current` / supersedes C2)로 덮여** 있었다. 부모가 만든 C4 후속본은 그 덮어쓰기로 사라졌다. 그 세션은 작업 중 계속 쓰고 있었으므로(4분 내 20개 이상 파일 변경) **되돌리지 않았다** — CLAUDE.md §8 "다른 세션의 변경을 되돌리지 않는다". 대신 적용할 정확한 문구를 아래에 남긴다. 소유 레인이 그대로 붙여 넣을 수 있다.

**7-1 `worldview-bible.md` §2 "남는 것" 줄에 추가**
> `밸브 개폐, 관 압력, 염도, 수위, 문 개폐, 당직 호출` 뒤에 **`, 봉인 완료 접점`**을 더하고 다음 두 줄을 잇는다.
> - **봉인 완료 접점**은 설립 때부터 봉인대에 달린 기계 접점이다. 이중서명 서식의 압착이 끝나면 접점이 닫히고 그 시각 한 줄이 염선 계통 로그에 각인된다. 접점은 **압착이 끝났다는 사실만** 남기고 누가 서명했는지·사람이 무엇을 확인했는지·서명이 유효한지는 남기지 않는다. 신설 장치가 아니라 밸브 개폐 각인과 같은 계통·같은 형식의 기존 배선이다.
> - "남지 않는 것" 줄 끝에 **`, 사람이 무엇을 확인했는지`**를 더한다. 확인·판단·기억은 시각을 갖지 않으므로 어떤 결론도 "누가 언제 확인했다"에 걸 수 없다.

**7-2 `timeline.md` §2 대조의 밤 표 — 세 행 교체**
> `H-1:20 현장이 서명 확인 전에 밸브를 돌림` → **`H-1:24 현장이 밸브를 돌림 / 기록: 밸브 개폐 각인`**
> 그 아래 신설: **`H-1:04 봉인대 압착 완료 / 기록: 봉인 완료 접점 각인. 집행이 이중서명 완성보다 20분 앞섰다`**
> `H+0:10 저지대 침수 시작` → **`H+0:12`** (4분 분해능 격자 정합)
> §4 R2·R3 회수 문구의 "서명 확인 전"을 **"봉인 완료 전"**으로 바꾼다.

**7-3 `gdd.md` 시간 예산 문장 교체**
> `튜토리얼 30 + 1장 50 + 2장 55 + 3장 60 + 4장 65 + 5장 65 + 6장 70 + 7장 60 + 에필로그 25 = 480분`
> → **`튜토리얼 25 + 1장 50 + 2장 55 + 3장 65 + 4장 65 + 5장 70 + 6장 75 + 7장 65 + 에필로그 10 = 480분`** (근거: `planning/campaign.json` 33비트 역산, C3-F2 대응)

반영 전까지 `synopsis/campaign.md`와 `scenes-and-dialogue.md`의 F1·F3 서술은 **캠페인 측 제안**이며 캐논 확정이 아니다. 두 계보가 동시에 `status: current`를 주장하는 상태는 워크스페이스 계약 위반이므로 **디렉터 판정 사항**이다.

## 8. 잔여 [OBSERVED]

사람 플레이타임 표본 **n = 0**. 힌트 사용률·이탈률·읽기 시간·완주율·IQR 전부 미측정. 이 문서의 어떤 수치도 G2/G4/G5/G6/G7을 올리지 못하며 D 게이트 입력일 뿐이다. 다음 측정은 T0 25분 + `c3-b2`~`c3-b3` 정합 구간 슬라이스로 `observedMedianMinutes` 첫 값과 정합 퍼즐 통과율을 재는 것이다.

## 9. 미실행

이번 작업에서 아카이브 수정, git commit/push, 범위 밖 파일 편집은 **하지 않았다**. 실제로 쓴 파일은 `planning/campaign.json`, `planning/campaign.meta.md`, `synopsis/campaign.md`, `synopsis/scenes-and-dialogue.md` 네 개뿐이다.

## 10. C3 종료 수정 루프 — 데이터 편집 1건 (2026-09-10) [OBSERVED]

### 10.1 무엇을 왜 바꿨는가 — C3-F11 (매체 2종 미달)

| 항목 | 내용 |
|---|---|
| 결함 | `qa/c3-review.md` C3-F11 · `qa/defect-register.md` C3-F11 (S2) — `c1-b4`의 단서 2건이 모두 `sourceType: log`라 "서로 다른 매체 2종" 규칙을 유일하게 어긴다 |
| 근거 | 검증기 `C-06` 검사, 수정 전 실행에서 `FAIL ["c1-b4(log)"]` |
| 편집 | `c1-b4.clues`에 **1건 추가**. 그 외 `c1-b4`의 목표·추론·행동·결과·힌트·복구·분·도구는 **한 글자도 바꾸지 않았다** |
| 추가한 단서 | `id: c1-b4-c3` · `sourceType: plate` · `originId: brine-log-gate3` · `copiedFrom: null` |
| 본문 | "제3수문 계통판의 야간 각인은 당직 호출이 걸린 시각만 남긴다. 호출을 받은 사람의 이름도, 그가 무엇을 확인했는지도 새겨지지 않는다." |
| 캐논 근거 | `worldview/glossary.md` §계통판·`brine-log-gate3`(제3수문 계통 로그, 기존 카탈로그 31종 내) · `worldview-bible.md` §2 "남지 않는 것: … 사람이 무엇을 확인했는지" · 법1 |
| 독립성 | `brine-log-gate3`는 `copiedFrom: null`이라 루트 출처가 `watchlog-bureau`·`handover-brief`와 다르고 매체도 다르다 → `c1-b4` 매체 3종(log·log·plate) |
| 문서 정합 | `content-matrix.md` §3이 이미 이 비트의 자료를 "일지 + 염판"으로 적고 있었다. **문서가 옳고 데이터가 비어 있던 경우**다 |
| 시각 문자열 | 추가하지 **않았다**. RFC-P3-013 캐논 시각(H-1:24 / H-1:04 / H+0:12)에 손대지 않기 위해 각인의 **형식**만 진술한다 |
| 검증 | 재실행 `exit=0`, 44/44 PASS (**그 시점의 검사 수**. R4에서 `Z-01`·`Z-02`·`K-06`이 늘어 현재는 47건 — §4.2). 단서 72 → **73**, `plate` 23 → **24** |

새 캐논·새 고유명사·새 사건은 **0건**이다. 카탈로그(31종)·도구(6)·구역(5)·비트(33)·분(480)·스테이지 분 배분은 편집 전후 동일하다.

### 10.2 `tools: []` 5비트는 결함이 아니다 — 도구 없는 비트 허용 규칙 [OBSERVED]

C3-F11은 `tools`가 빈 비트 5건도 함께 보고했다. 그 5건은 **전부 비퍼즐 비트**이며 설계상 정당하다.

| 비트 | `kind` | 분 | 왜 확정 도구가 없는가 |
|---|---|---:|---|
| `t0-b1` | 탐색 | 5 | 인수 각서·이관 목록을 읽고 서랍의 판을 슬롯에 올린다. 도구가 아직 열리지 않은 첫 화면(구역 상태 H0) |
| `c1-b4` | 대화 | 10 | 재화의 진술을 각서와 3분류한다. 판정은 분류이지 도구 확정이 아니다 |
| `c3-b4` | 대화 | 15 | 거래 제안의 수락·거절. 어느 쪽이든 경로가 열리므로 확정 눈금이 없다 |
| `c7-b1` | 대화 | 16 | 청문 검증 개시 — 절차 안내와 3×3표 열람 |
| `e0-b1` | 정산 | 6 | 청문 결과와 네 구역 상태 열람 |

**규칙(본 문서가 정한다)**: `kind ∈ {exploration, dialogue, payoff}` 비트는 `tools: []`를 가질 수 있다.
`kind: puzzle` 비트는 `tools`가 비어 있으면 결함이다 — 실측 [OBSERVED] **퍼즐 21/21 전건이 도구 ≥1개**를 가진다(검증기 집계 `toollessBeats`에 퍼즐 0건).
따라서 "동사 33/33" 주장은 **성립하지 않으며 앞으로도 목표가 아니다.** 올바른 주장은 "**퍼즐 21/21이 도구를 쓰고, 비퍼즐 12건 중 7건이 부수 도구를 쓴다**"이다.

### 10.3 `tools` 배열의 의미 — chapter-beats 표 A와 7건 어긋난다 [OBSERVED]

live `tools`가 `synopsis/chapter-beats.md` 표 A의 "확정"과도, "확정+연습"과도 일치하지 않는다. 전수 대조:

| 동사 | JSON `tools` 비트 수 | 표 A 확정 | 표 A 연습 | JSON에만 있음 | 표 A에만 있음 |
|---|---:|---:|---:|---|---|
| `circuit` | 10 | 10 | 2 | — | `c1-b4` `c6-b4` |
| `reader` | 11 | 8 | 6 | `c1-b3` | `t0-b1` `t0-b2` `c1-b4` `e0-b1` |
| `alignment` | 8 | 3 | 6 | — | `c3-b4` |
| `routing` | 3 | 2 | 1 | — | — |
| `corrosion` | 3 | 0 | 3 | — | — |
| `seal` | 7 | 5 | 2 | `c6-b4` | `c7-b1` |

[INFERENCE] `tools`는 "확정 도구"도 "확정+연습"도 아니고 **그 비트에서 실제로 손에 쥐는 도구**에 가깝다. 두 문서 중 하나가 틀린 것이 아니라 **같은 열 이름이 두 정의를 갖는다.** 데이터를 임의로 맞추지 않고 정의 확정을 RFC로 올린다(§11 RFC-P3-016). 그때까지 어떤 문서도 "확정 N회"를 `tools` 집계로 주장하지 않는다.

## 11. 이 편집이 깨뜨리는 인용 — 브로드캐스트 [OBSERVED]

| 갱신 대상 | 무엇이 바뀌었나 | 소유 레인 |
|---|---|---|
| **sha256 `fdabf1d4…` → `92301c0a…`, bytes 120479 → 121457 (R4, zoneId 추가)** | `fdabf1d4…`를 인용하는 모든 문서 — `planning/{gdd,content-matrix,campaign-time-budget}.md` · `planning/feature-specs/*` · `qa/{c3-review,defect-register}.md` · `production/decision-log.md`(RFC-Q1) · systems·balance·economy의 §0 인용줄. **고정 숫자를 다시 적지 말고 검증기 출력의 `sha256`을 인용한다**(RFC-Q1) | 전 레인 |
| **비트 `zoneId` 신설** | `systems/data-schemas/beats.md`(스키마 표) · `synopsis/chapter-beats.md` 표 A(구역 열은 이제 파생값) · `content-matrix.md` §3(본 회차에서 planner가 재작성) | systems, synopsis, planner |
| **`c4-b3` → `c6-b4` 의도 문장 이동 (RFC-W4)** | `synopsis/chapter-beats.md` B18·B27 문구 · `worldview/consistency-audit.md` A22 · `worldview/worldview-bible.md` §9 OPEN-4 | synopsis, worldview |
| sha256 `775a984c…` → `fdabf1d4…`, bytes 120087 → 120479 | `systems/data-schemas/*.md` §0 · `systems/architecture-contract.md` · `balance/balance-sheet.md` · `balance/patch-deltas.md` · `balance/sim-results/README.md` (C3-F2가 이미 재측정을 지시한 문서들) | systems, balance |
| 단서 72 → **73** · `plate` 23 → **24** | `worldview/glossary.md` §서류 카탈로그 머리글(현재 "단서 72건 / log 27 · ledger 22 · plate 23") · `systems/data-schemas/beats.md` | worldview, systems |
| `c1-b4` 매체 log+log → log+plate | `synopsis/chapter-beats.md` B07(이미 "일지+염판"으로 적혀 있어 **수정 불필요**) | synopsis |
| RFC-P3-016 (`tools` 열 정의) | `synopsis/chapter-beats.md` 표 A · `planning/content-matrix.md` §4.1 | synopsis, planner, director |

기획 레인이 직접 고친 문서는 `planning/` 안뿐이다. 위 표의 다른 레인 파일은 **읽기만 했고 수정하지 않았다**(CLAUDE.md §4 공유 진실 파일 규칙).
