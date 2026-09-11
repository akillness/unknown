---
updated: 2026-09-10
cycle: 20260909-preproduction-c3
status: current
supersedes: null
owner: game-planner
describes: _workspace/current/planning/validate-campaign.mjs
---

# validate-campaign.mjs 메타

`.mjs`는 비-Markdown 산출물이라 frontmatter를 넣지 않는다(CLAUDE.md §10). 소유자·목적·한계는 이 파일이 보유한다.

## 1. 파일 사실 [OBSERVED]

| 항목 | 값 | 측정 명령 |
|---|---|---|
| 경로 | `_workspace/current/planning/validate-campaign.mjs` | — |
| 소유 레인 | `game-planner` | — |
| 런타임 | Node **v26.8.1** (내장 모듈 `node:fs` `node:crypto` `node:path` `node:url`만 사용, 의존성 설치 0건) | `node --version` |
| 검사 대상 | `_workspace/current/planning/campaign.json` (인자로 다른 경로 지정 가능) | — |
| 종료 코드 | 0 = 전건 PASS · 1 = FAIL 존재 · 2 = 실행 오류 | `node …/validate-campaign.mjs ; echo "exit=$?"` |
| 실행 모드 | 기본 = 검사 **49건**(R7에서 `Z-03`·`H-04` 추가) · `--pairs` = `proofRequired` 비트별 독립쌍 출력(A37 / C3-F12 파생용, 종료 코드 0 = **17/17** 쌍 존재 — RFC-N6으로 15 → 17) | `node …/validate-campaign.mjs --pairs` |
| 실행 모드 (신설) | `--t0 <dir>` = T0 인스턴스 데이터 검사 **5건**(`T0-01`~`T0-05`). `<dir>` 생략 시 기본값 `../systems/data/t0`, `--t0=<dir>` 형태도 받는다. 이 모드는 캠페인 49건 검사를 **대신**한다(따로 돌린다) | `node …/validate-campaign.mjs --t0 _workspace/current/systems/data/t0` |

## 2. 왜 만들었는가 [OBSERVED]

`qa/c3-review.md` C3-F11이 `campaign.meta.md` §4의 "검증 영수증"을 반증했다 — 그 표는 생성기 결과를 **손으로 옮겨 적은 것**이라 live 파일과 어긋났고, 실제로는 매체 2종 미달 1건·`tools: []` 5건이 "PASS"로 적혀 있었다.
같은 실수가 반복되지 않도록 **주장을 실행 가능한 검사로 바꿨다.** 이제 §4는 사람이 쓴 표가 아니라 이 스크립트의 출력이다.

## 3. 무엇을 검사하는가 (49건) [OBSERVED]

| 군 | 검사 | 대표 id |
|---|---|---|
| 파일 사실 | `schemaVersion` · `designMinutes` 480 · `observedMedianMinutes` null · `humanPlaytests` 빈 배열 | `F-01`~`F-04` |
| 스테이지·분 | 스테이지 9 · 분 배분 `[25,50,55,65,65,70,75,65,10]` · 총합 480 · 스테이지별 분 = 소속 비트 분 합 · zoneIds 5구역 | `S-01`~`S-06` |
| **비트 구역 (R4 신설 · R7 확장)** | `beat.zoneId ∈ 소속 stage.zoneIds` · 33/33 전건 존재(C3-F22) · **본문 `subtasks[0]`이 말하는 구역 ∋ `beat.zoneId`**(구역 무언급 비트는 대상 외 — C6-F17) | `Z-01`·`Z-02`·**`Z-03`** |
| 비트 | 33개 · 스테이지별 3/4·7/2 · beat·clue·checkpoint id 유일 · kind 분포(퍼즐 21 · 탐색 2 · 대화 5 · 정산 5) | `B-01`~`B-06` |
| 시간 구조 | `activityBudget` 5키 음수 없음·합 = `minutes` · 범주 합 480 · `fast < min < deliberate` · `timeConfidence ∈ {low,medium}` · `authorEstimateBasis` · `subtasks` 3~5 | `T-01`~`T-06` |
| 힌트·복구·저장 **(R7 확장)** | 힌트 3단(빈 문자열 없음) · `recovery` 비어있지 않음 · `checkpoint` 존재 · **1단 힌트에 자료명·정답값·정답 단정·조작 지시 없음**(gdd §6 1단 정의 — C6-F3) | `H-01`~`H-03`·**`H-04`** |
| 단서·독립성 | `sourceType` enum · `originId` 전건 · `copiedFrom` 카탈로그 내·모순 없음·순환 없음 · **모든 비트 매체 종류 ≥ 2** · **`proofRequired` 비트의 루트 `originId` 상이 AND `sourceType` 상이 쌍** | `C-01`~`C-07` |
| 도구 | 도구 id 6종 고정 · `toolTeaching` 12건 · 6종 각각 guided 1 + unguided 1 · teaching 도구가 그 비트 `tools`에 포함 | `V-01`~`V-04` |
| 선행 조건 | `prerequisites` 참조 존재 · 전건이 앞선 비트 · **무순환**(DFS) | `P-01`~`P-03` |
| 캐논 회귀 | 기록 불가 명제 문자열 부재 · `봉인 완료 접점` 존재 · **RFC-P3-013 시각**(H-1:24·H-1:04·H+0:12 존재 / H-1:20·H+0:10 부재) · **RFC-P3-012 공개 순서**(도연 = `t0-b1`, 한서린 = `c4-b2`) | `K-01`~`K-05` |
| **의도 공개 시점 (R4 신설)** | **RFC-W4**: '방패가 아니라 잠금장치'가 `c4-b3`에 **부재**하고 `c6-b4`에 **존재** | `K-06` |

**독립성 판정 방식** [OBSERVED]: `copiedFrom`을 재귀적으로 해석해 **루트 `originId`**를 구한다(순환 방지). 사본은 루트를 물려받으므로 "원본 × 그 사본"은 독립 쌍이 되지 못한다 — `worldview/worldview-bible.md` §3-bis.3 P3, `systems/interaction-rules.md` §3.

## 4. 무엇을 검사하지 **않는가** [OBSERVED]

- **재미·난이도·도달 가능성**. 49/49 PASS는 JSON이 문서 계약과 자기 자신에 대해 무모순이라는 뜻뿐이다.
- **구역이 그 장면의 옳은 무대인지**. `Z-01`은 `zoneId`가 스테이지 구역 목록 **안에** 있는지만 본다. R7의 `Z-03`이 그 구멍을 **첫 하위과제 한 줄**까지 좁혔지만(C6-F17로 4비트 적발), 뒤 하위과제·`consequence`의 구역 이동과 **구역 명사를 아예 쓰지 않는 16비트**는 여전히 사람이 읽어야 한다.
- **1단 힌트가 정말 방향만 주는지**. `H-04`는 **어휘 바닥**이다 — 자료명·정답값·정답 단정·조작 지시 네 부류의 어휘만 막는다. 금지 어휘를 피하면서 문장으로 정답 통찰을 흘리는 1단(`c3-b4`·`c4-b3`·`c4-b4`가 그랬다)은 잡지 못하며 그 판정은 QA 렌즈가 한다. **힌트의 실효성은 표본 n=0으로 미측정**이다.
- **`--pairs`의 쌍이 플레이어에게 발견 가능한지**. 쌍의 존재는 문서 정합이며 실제 도달성은 표본 n=0.
- **다른 문서와의 일치**. `chapter-beats.md` 표 A·`balance-sheet.md` 등의 인용값은 이 스크립트 밖에 있다.
- **분 추정의 타당성**. `timeConfidence`가 low 12건임을 읽을 뿐 그 추정이 옳은지는 모른다.
- **사람 플레이**. 표본 **n = 0**. 이 스크립트는 G2/G4/G5/G6/G7 중 어느 것도 올리지 않는다.

## 5. 상수를 바꿔야 할 때 [OBSERVED]

기대값은 파일 상단 `EXPECT` 객체 한 곳에 모여 있다. 스테이지 분 배분·비트 수·kind 분포·캐논 시각·공개 순서가 바뀌면 **그 변경의 RFC 번호를 주석으로 남기고** `EXPECT`를 고친다.
`EXPECT`를 결과에 맞춰 조용히 고치는 것은 검사를 무력화하는 행위다 — 데이터가 바뀌었으면 `campaign.meta.md`에 사유를 적는다.

## 5.1 R4 개정 이력 (2026-09-10) [OBSERVED]

| 무엇 | 왜 | 근거 |
|---|---|---|
| `Z-01`·`Z-02` 신설 | 비트 단위 `zoneId` 도입을 기계가 강제 | 디렉터 C3 종료 판정 **C3-F22** |
| `K-06` 신설 | R2 의도 문장이 4장으로 되돌아오는 회귀를 막는다 | **RFC-W4** |
| `--pairs` 모드 신설 | `synopsis/continuity.md` §5 K 표를 손으로 고르지 않고 `C-07`과 **같은 규칙**의 출력에서 파생 | **A37 / C3-F12** |
| `aggregates.zone*` 6키 신설 | `content-matrix.md` §1·§3·§4.3의 구역 집계(비트·분·D1·D2)를 명령 하나로 재도출 | C3-F22 파생 |

신설 3검사는 **고치기 전에 실제로 FAIL을 냈다** — `Z-02` 33건 · `K-06` `{c4-b3: true, c6-b4: false}`(`campaign.meta.md` §4.1). 검사를 데이터에 맞춰 사후 조정하지 않았다는 뜻이다.

## 5.2 R7 개정 이력 (2026-09-10) [OBSERVED]

| 무엇 | 왜 | 근거 |
|---|---|---|
| **`Z-03` 신설** | `Z-01`이 `zoneId` 필드끼리만 대조해 **본문이 다른 구역을 말해도 PASS**였다. 첫 하위과제가 말하는 구역 중 하나는 `beat.zoneId`여야 한다 | 디렉터 R7 배정 **C6-F17** |
| **`H-04` 신설** | `H-01`이 **빈 문자열만** 검사해서 1단 힌트가 자료·정답값·인과를 지목해도 통과했다 | 디렉터 R7 배정 **C6-F3** |
| `H-04`의 구역 고유명 예외 | 구역 이름 자체에 숫자가 있다(`제3수문`·`제1양수장`). 1단은 구역을 말해도 되므로 값·수량 검사 전에 이 두 고유명만 지운다 | 본 회차 자체 발견 [OBSERVED] |

신설 2검사도 **고치기 전에 실제로 FAIL을 냈다** [OBSERVED · 재현]: R7 편집 전 값만 되돌린 이미지를 만들어 같은 검증기를 돌린 결과 `exit=1` · **`Z-03` 4비트**(`c1-b2`·`c2-b4`·`c5-b2`·`c6-b4`) · **`H-04` 24비트** · 나머지 47건 PASS였다(`campaign.meta.md` §4.1 재현 블록). 재작성한 1단은 27건으로 24건보다 3건 많다 — `H-04`가 증명하지 못하는 부류를 사람 검토로 더 고쳤기 때문이다.

## 5.3 R7 종료 수정 회차 — `--t0 <dir>` 모드 신설 (2026-09-10) [OBSERVED]

근거: 디렉터 판정 **RFC-C7-001 (2)** — "planner 가 `validate-campaign.mjs` 에 `T0-01~` 검사(인스턴스 ↔ 캠페인 단서 id·originId·매체 일치)를 추가한다".
소유 경계: 검사기는 planner 소유, **피검사 파일 `systems/data/t0/*.json` 은 systems 소유**다. 이 모드는 읽기만 하며 planner 는 FAIL 을 고치지 않고 systems 로 돌려보낸다.

| 검사 | 무엇을 대조하는가 | 정본 |
|---|---|---|
| `T0-01` | `beats.json.rows` ⊂ campaign T0 비트(id·kind·zoneId·minutes·proofRequired) · `completion` 존재 및 campaign 원문 일치 · `completionPredicate._srcCompletion` 동일 · `emitsCommitCommand` 집합 = campaign `proofRequired` 집합 = `commitCommandBeats` | live `campaign.json` · RFC-C7-001 (1)·(3) |
| `T0-02` | campaign 전 비트가 3단 힌트 33/33 · t0 3비트 × 3단 9행이 `hintId = {beatId}-h{level}` · `sourceTextKo` 가 campaign `hints[level-1]` 과 **문자열 일치** · `revealScope` = direction/procedure/solution · 1단 `revealsValues:false` | live `campaign.json` · `gdd.md` §6 1단 정의 (H-04 와 같은 계약) |
| `T0-03` | `records.rows[].clueIds` 합집합 = campaign T0 단서 6건과 **1:1**(중복·미참조·외부참조 0) · 행별 `sourceType`·`originId`·`medium` 이 해당 단서와 동일 · `rootOriginId` = `copiedFrom` 체인 루트(RFC-S5 예외 없음) · **매체 2종** (T0 전체 및 비트별 서로 다른 `sourceType` ≥ 2) | live `campaign.json` · `interaction-rules.md` §3 |
| `T0-04` | `zones.hub.systemIds` ⊂ 계통 목록 · 전 `viewNodes[].cameraPose` 가 style-guide §5 수치(수평 화각 54° · 시선 높이 1.55 m · 피치 −18° · 롤 0° · 요 ∈ {−30,0,30}) · `cameraConstants` 동일 | `concept/style-guide.md` §5 |
| `T0-05` | `tools` circuit = `hasCommit:false` + `commitCondition:null`(gdd 셀 "— (해당 없음)") · reader = `hasCommit:true` + `commandId:CiteToBoard` + 규칙 문자열이 gdd 셀의 근거 슬롯(매체·계통·관측소 + "출처")을 전건 포함 + `goesThrough` 에 체크포인트·저장·되돌림 | `planning/gdd.md` §4 표(파일에서 직접 파싱) · RFC-C7-001 (1) |

`T0-04` 의 "계통 목록" [INFERENCE]: `systems/data-schemas/zones.md` 는 계통을 **열거하지 않고** 불변식 `Z-I2`("모든 `systemIds` 가 plates 저작본의 `systemId` 에 존재")만 둔다. `zones.json` 의 `_src.systemIds` 가 "계통 id 는 구역 토큰을 그대로 쓴다 — 캐논은 계통에 고유명을 주지 않는다"라고 파생 규칙을 밝혔으므로 이 검사는 **계통 목록 = 캠페인 고정 5구역 토큰**으로 본다. 계통에 고유명이 생기면 정본은 worldview 이며 이 상수를 함께 고쳐야 한다.

`T0-05` 의 일치 판정 범위 [OBSERVED]: 슬롯 단위다. `tools.json` 은 gdd 의 "**매체**"를 스키마 필드명 "`sourceType`"으로 적으므로 **문자 단위 동일은 아니다** — 출력의 `literalMatch: false` 가 그 사실이며 검사는 슬롯(매체≈sourceType · 계통 · 관측소 + "출처")으로 통과시킨다. 문자 단위 통일을 원하면 gdd(planner) 또는 tools.json(systems) 중 하나를 고쳐야 하는 **표기 결정**이며 이 회차에서는 고치지 않고 열어 둔다.

### 5.3.1 실행 결과 원문 [OBSERVED · 재현 가능]

명령 (저장소 루트에서):

```
node _workspace/current/planning/validate-campaign.mjs --t0 _workspace/current/systems/data/t0 ; echo "exit=$?"
```

결과: `exit=0` · `summary.verdict = PASS` · 5/5. stdout 원문 전체:

```json
{
  "validator": "planning/validate-campaign.mjs --t0",
  "contract": "RFC-C7-001 (2) — T0 인스턴스 데이터 ↔ campaign.json / gdd §4 / style-guide §5 정합",
  "owner": "planner (검사) · systems (피검사 파일)",
  "campaignFile": "/Users/jangyoung/orca/unknown/_workspace/current/planning/campaign.json",
  "campaignBytes": 124007,
  "campaignSha256": "8a43d334f8f69d6642a74708e7b3d68353c7982fb5f506dbba291021a538a93a",
  "t0Dir": "/Users/jangyoung/orca/unknown/_workspace/current/systems/data/t0",
  "t0Files": {
    "beats": "/Users/jangyoung/orca/unknown/_workspace/current/systems/data/t0/beats.json",
    "hints": "/Users/jangyoung/orca/unknown/_workspace/current/systems/data/t0/hints.json",
    "records": "/Users/jangyoung/orca/unknown/_workspace/current/systems/data/t0/records.json",
    "zones": "/Users/jangyoung/orca/unknown/_workspace/current/systems/data/t0/zones.json",
    "tools": "/Users/jangyoung/orca/unknown/_workspace/current/systems/data/t0/tools.json"
  },
  "sourceShaDeclared": {
    "beats": "8a43d334f8f69d6642a74708e7b3d68353c7982fb5f506dbba291021a538a93a",
    "hints": "8a43d334f8f69d6642a74708e7b3d68353c7982fb5f506dbba291021a538a93a",
    "records": "8a43d334f8f69d6642a74708e7b3d68353c7982fb5f506dbba291021a538a93a"
  },
  "sourceShaMatchesLiveCampaign": true,
  "summary": {
    "checks": 5,
    "pass": 5,
    "fail": 0,
    "verdict": "PASS"
  },
  "checks": [
    {
      "id": "T0-01",
      "name": "beats.json ⊂ campaign T0 비트 · completion(및 술어 원문) 존재·일치",
      "status": "PASS",
      "expected": {
        "beats": [
          "t0-b1",
          "t0-b2",
          "t0-b3"
        ],
        "completionNonEmpty": true
      },
      "actual": {
        "beats": [
          "t0-b1",
          "t0-b2",
          "t0-b3"
        ],
        "commitCommandBeats": [
          "t0-b3"
        ]
      }
    },
    {
      "id": "T0-02",
      "name": "campaign 힌트 33/33 3단 · t0 3비트 × 3단이 hints.json 과 문자열 일치",
      "status": "PASS",
      "expected": {
        "campaign": "33/33",
        "t0Rows": 9
      },
      "actual": {
        "campaign": "33/33",
        "t0Rows": 9
      }
    },
    {
      "id": "T0-03",
      "name": "records.clueIds·originId·sourceType ↔ campaign 단서 1:1 · 매체 2종",
      "status": "PASS",
      "expected": {
        "clues": [
          "t0-b1-c1",
          "t0-b1-c2",
          "t0-b2-c1",
          "t0-b2-c2",
          "t0-b3-c1",
          "t0-b3-c2"
        ],
        "minMediaKinds": 2
      },
      "actual": {
        "records": [
          "rec-handover-brief",
          "rec-transfer-list",
          "rec-plate-standard-hub",
          "rec-watchlog-bureau",
          "rec-tide-ledger-bureau"
        ],
        "mediaKinds": [
          "log",
          "ledger",
          "plate"
        ],
        "mediaPerBeat": [
          {
            "beat": "t0-b1",
            "kinds": [
              "log",
              "ledger"
            ]
          },
          {
            "beat": "t0-b2",
            "kinds": [
              "plate",
              "log"
            ]
          },
          {
            "beat": "t0-b3",
            "kinds": [
              "plate",
              "ledger"
            ]
          }
        ]
      }
    },
    {
      "id": "T0-04",
      "name": "zones.hub.systemIds ⊂ 계통 목록 · 전 viewNode cameraPose = style-guide §5 수치",
      "status": "PASS",
      "expected": {
        "fovDeg": 54,
        "fovAxis": "horizontal",
        "eyeHeightM": 1.55,
        "pitchDeg": -18,
        "rollDeg": 0,
        "yawOptionsDeg": [
          -30,
          0,
          30
        ],
        "fillRatio": 0.7
      },
      "actual": {
        "systemIds": [
          "hub"
        ],
        "viewNodes": 6,
        "yawUsed": [
          0,
          30
        ]
      },
      "note": "계통 목록 = 캠페인 고정 5구역 토큰 [INFERENCE] — zones.md 는 계통을 열거하지 않는다(Z-I2 만 있다)"
    },
    {
      "id": "T0-05",
      "name": "tools circuit/reader 확정 조건이 gdd §4 표와 일치",
      "status": "PASS",
      "expected": {
        "circuit": "— (해당 없음)",
        "reader": "인용에 매체·계통·관측소 출처가 채워졌을 때"
      },
      "actual": {
        "circuit": {
          "hasCommit": false,
          "commitCondition": null
        },
        "reader": {
          "hasCommit": true,
          "commandId": "CiteToBoard",
          "rule": "인용에 sourceType · 계통 · 관측소 출처가 모두 채워졌을 때만 고정된다"
        },
        "literalMatch": false
      },
      "note": "일치 판정은 슬롯 단위다(매체·계통·관측소 + '출처'). tools.json 은 '매체'를 스키마 필드명 'sourceType' 으로 적으므로 문자 단위 동일은 아니다 — literalMatch 필드로 보고한다 [OBSERVED]"
    }
  ],
  "notMeasured": [
    "Unity 실행 0회 — 이 데이터가 런타임에 로드되는지·술어가 실제로 판정되는지 미측정",
    "사람 플레이 표본 n=0 — 힌트 실효성·카메라 프레이밍 가독성·조작감 전부 미측정",
    "loadCostMb · 프레임 시간 등 성능 값은 이 검사 대상이 아니다(hw_profile_id 미확정 · PRE-1)"
  ]
}
```

회귀 [OBSERVED]: 같은 편집 뒤 기본 모드를 다시 돌린 결과 `node _workspace/current/planning/validate-campaign.mjs ; echo "exit=$?"` → `exit=0`, `summary` = `{"checks":49,"pass":49,"fail":0,"verdict":"PASS"}`, `sha256` = 검증기 출력값(=`--t0` 출력의 `campaignSha256`), `bytes` = 124007. `--pairs` 도 `exit=0`. 인자 형태 3종(`--t0` 생략형 · `--t0=<dir>` · `<campaign.json> --t0 <dir>`) 모두 `exit=0` 이며 위치 인자 해석이 깨지지 않았다.

### 5.3.2 검사가 실제로 검사한다는 증거 (음성 시험) [OBSERVED · 재현 가능]

주장만으로는 PASS 5/5 가 "검사가 아무것도 보지 않는다"와 구별되지 않는다(C7-F38 유형). 그래서 **스크래치패드 사본**(저장소 파일 0건 수정)에 결함 5개를 심고 같은 명령을 돌렸다.

심은 결함: `beats.rows[0].completion` 을 빈 문자열로 · `hints.rows[0].sourceTextKo` 를 다른 문자열로 · `records.rows[1].sourceType/medium` 을 `ledger`→`log` 로 · `zones` 의 `hub.systemIds` 를 `["sewer"]` 로 + `hub-view-desk` 피치를 −20° 로 · `tools` reader 규칙을 "아무 때나 고정된다"로.

결과: `exit=1` · `summary` = `{"checks": 5, "pass": 0, "fail": 5, "verdict": "FAIL"}`.

| 검사 | 상태 | 적발 내용 |
|---|---|---|
| `T0-01` | FAIL | ["t0-b1: completion 없음/빈 문자열"] |
| `T0-02` | FAIL | ["t0-b1-h1: sourceTextKo ≠ campaign hints[0]"] |
| `T0-03` | FAIL | ["rec-transfer-list/t0-b1-c2: sourceType log ≠ ledger", "rec-transfer-list/t0-b1-c2: medium log ≠ sourceType ledger", "t0-b1: records 매체 1종 < 2"] |
| `T0-04` | FAIL | ["hub.systemIds ∉ 계통 목록(구역 토큰): sewer", "hub-view-desk.pitchDeg -20 ≠ -18"] |
| `T0-05` | FAIL | ["reader 확정 조건 슬롯 '매체' 이 tools.json 규칙에 없다", "reader 확정 조건 슬롯 '계통' 이 tools.json 규칙에 없다", "reader 확정 조건 슬롯 '관측소' 이 tools.json 규칙에 없다", "reader 규칙에 '출처' 없음 (gdd §4 「… 출처가 채워졌을 때」)"] |

즉 5개 검사 모두 **고장 난 입력에서 FAIL 을 낸다.** 이 시험은 스크래치패드에서만 했고 `systems/data/t0/` 의 실제 파일은 이 회차에서 planner 가 **읽기만** 했다.

### 5.3.3 이 모드가 올리지 않는 것 [OBSERVED]

Unity 실행 0회이므로 이 데이터가 런타임에 로드되는지, 완료 술어가 실제로 판정되는지는 **미측정**이다. 사람 표본 n=0 이므로 힌트 실효성·카메라 프레이밍 가독성·조작감도 미측정이다. `--t0` PASS 는 **문서·데이터 정합**일 뿐 어떤 G 게이트도 올리지 않는다.

## 6. 미실행 [OBSERVED]

이 스크립트는 파일을 **읽기만** 한다. `campaign.json`을 수정하지 않고, 다른 레인 폴더를 건드리지 않으며, 네트워크·git·설치를 하지 않는다. `--t0` 모드도 마찬가지다 — systems 소유의 `systems/data/t0/*.json` 과 `concept/style-guide.md` 파생 상수·`planning/gdd.md` §4 표를 **읽어서 대조만** 하고, FAIL 이면 종료 코드 1 로 알릴 뿐 고치지 않는다.
