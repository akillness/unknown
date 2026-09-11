---
updated: 2026-09-10
cycle: 20260909-preproduction-c3
status: current
supersedes: null
owner: game-balance-designer
---

# 밸런스 시트 — 조수기록국: 마지막 당직 (가제)

## 0. 이 문서의 지위와 표기

**C3 종료 수정 루프 재기반 고지 (2026-09-10)** [OBSERVED]: 본 판은 디렉터 판정 **RFC-P3-008**(계보 B = live `planning/campaign.json`이 유일 정본) · **RFC-P3-009**(부식예산 = `routing` 구성안에 걸리는 전역 상한 9, 확정 무소모) · **RFC-P3-015**(무진전 자동 제안 = 180초 단일 제안 모델)을 적용하고, `qa/c3-review.md`의 **C3-F2 · F4 · F9 · F15 · F19 · F20**을 닫기 위해 §0 영수증 · §2 미러 · §3 · §4 · §6 · §7 · §9 · §10을 **전량 재도출**한 것이다. 폐기한 모델(6계통 한도표 · 하한 돌파 모드 · 판 15매 지급표 · 힌트 3단 자동 승격 임계)은 삭제하지 않고 §11 폐기 원장에 원문 값으로 보존한다(CLAUDE.md §2 "삭제는 없다").

**R7 종료 수정 회차 제자리 개정 고지 (2026-09-10)** [OBSERVED]: 같은 `cycle` 값 안의 제자리 개정이다(RFC-Q2 — 아카이브·`supersedes` 대상 아님). 이번 개정이 손댄 곳은 **① 디렉터 판정 RFC-B6** — §0 정본 행 · §0.1 · §3.2 · §4.3 의 고정 sha/바이트 문자열을 검증기 출력 인용으로 교체하고 염판 **24** 실측을 확정(Q7 닫힘), **② 디렉터 판정 C6-F5** — 문서로 닫지 않고 **§10.4 위험 R-T0-1** 로 등록. **수치 델타 0건**(`patch-deltas.md` §7).

| 항목 | 값 |
|---|---|
| 성격 | 비전투 추리 어드벤처의 **자원·판정·난이도 밴드** 정의서 (G2 판정 소스) |
| 측정 상태 | [OBSERVED] 사람 플레이 n=0, 빌드 n=0, 밸런스 시뮬 실행 0건. 아래 모든 밴드값은 [TARGET]이며 집계값만 [OBSERVED] |
| 역산 입력 (정본) | `_workspace/current/planning/campaign.json` — **집계·sha256·바이트는 `node _workspace/current/planning/validate-campaign.mjs` 출력만 인용한다**(RFC-Q1 · RFC-B6, 고정 문자열 재기재 금지). 2026-09-10 재실행: `summary.verdict = PASS` · 47검사 47 PASS · `aggregates.beats 33` · `stages 9` · `totalMinutes 480` [OBSERVED, 명령 §0.1] |
| 역산 입력 (규칙) | `_workspace/current/worldview/worldview-bible.md` §2·§3(6법)·§5, `_workspace/current/systems/interaction-rules.md` §2.4·§2.5·§3·§4, `_workspace/current/systems/prototype/model.mjs` L63-65·L79, `_workspace/current/economy/currency-map.md` §4.1·§4.2 |
| 역사 인용 전용 | 아카이브 c3 `campaign.json` sha256 `5029d44a8042323d438d5975604c12b17f2717fc41b93db82f1cab06996f7e16`(74342 B) — RFC-P3-008에 따라 역산 대상 아님 |
| 직전 live 판 | sha256 `775a984cdc1a4d76a55bfed5f3e9dc6dc497ca0c93b8bcb3b7afce56fe6a9a52`(120087 B) — QA C3-F2가 집계한 시점. planner의 C3-F11 수정(단서 1건 추가)으로 대체됨 [OBSERVED `planning/campaign.meta.md` §1] |
| **폐기된 인용 (C3-F2)** | 이전 판 §0·§3.1이 "재집계 시점 sha256 `2bfe4d52…`"를 [OBSERVED]로 적었으나 **저장소 전수 검색에서 0건**이다. 그 해시로 유도한 집계(염판 22·reader 8·seal 6)는 전부 무효이며 아래 영수증이 대체한다 |
| 선행 초안 | `_workspace/current/balance/puzzle-balance.md` (`status: draft`, c4). 본 시트가 그 내용을 수치화·확장한다. 초안 파일은 삭제·이동하지 않는다 |
| 금지 | "느낌상 강하다"류 사유의 수치 변경. 변경 트리거는 QA 결함 등록·시뮬 결과·텔레메트리뿐 |

### 0.1 재집계 영수증 [OBSERVED · 2026-09-10]

측정 명령(그대로 재현 가능):

```
# RFC-B6 적용: sha·바이트를 이 문서에 옮겨 적지 않는다. 검증기 출력 필드를 그 자리에서 읽는다.
$ node _workspace/current/planning/validate-campaign.mjs            # exit 0
#   출력 JSON 의 file / bytes / sha256 / summary / aggregates 가 본 시트의 유일한 입력 인용원이다.
$ node _workspace/current/planning/validate-campaign.mjs | node -e 'let s="";process.stdin.on("data",d=>s+=d).on("end",()=>{const j=JSON.parse(s);const a=j.aggregates;console.log(j.summary.verdict,j.summary.checks,j.summary.fail,"| beats",a.beats,"| clues",a.clues,JSON.stringify(a.sourceTypeDist),JSON.stringify(a.toolBeatCounts),JSON.stringify(a.kindCounts),"| proofRequired",a.proofRequiredBeats,"| origins",a.originCatalogSize,"| fast/delib",a.fastMinutesSum,a.deliberateMinutesSum,"| activity",JSON.stringify(a.activityBudgetSums));});'
PASS 47 0 | beats 33 | clues 73 {"log":27,"ledger":22,"plate":24} {"circuit":10,"reader":11,"alignment":8,"routing":3,"corrosion":3,"seal":7} {"exploration":2,"puzzle":21,"dialogue":5,"payoff":5} | proofRequired 15 | origins 31 | fast/delib 322 673 | activity {"exploration":53,"reasoning":190,"manipulation":168,"dialogue":28,"payoff":41}
```

| 집계 | 이전 표기 (출처) | **재측정값** | 비고 |
|---|---|---|---|
| 단서 총수 | 70 (`systems/data-schemas/beats.md` L79) | **73** | 두 명령 일치 |
| `sourceType` 분포 (염판/일지/대장) | 22 / 25 / 23 (`data-schemas/plates.md` 머리줄) | **24 / 27 / 22** | 밸런스 §0 이전 판은 염판만 22로 적었다 |
| `circuit` 비트 | 10 (밸런스 §0) | **10** | 변화 없음 |
| `reader` 비트 | 8 (밸런스 §0 · `data-schemas/tools.md`) | **11** | C3-F2 파생 오류 정정 |
| `alignment` 비트 | 8 (동상) | **8** | 변화 없음 |
| `routing` 비트 | 3 (동상) | **3** | 변화 없음 |
| `corrosion` 비트 | 3 (동상) | **3** | 변화 없음 |
| `seal` 비트 | 6 (동상) | **7** | C3-F19와 동일한 정정(확정 5 + 연습 2) |
| 비트 / 체크포인트 / 힌트 | 33 / 33 / 99 | **33 / 33 / 99** | 변화 없음 |
| `kind` 분포 | puzzle 21·dialogue 5·payoff 5·exploration 2 | **동일** | G2 N/A 판정 근거(§1) 유지 |
| `fastMinutes` / `deliberateMinutes` 합 | 321 / 672 (`beats.md` L74-75) | **322 / 673** | 시나리오 경계, 표본 통계와 비교 금지(RFC-P3-011) |
| `proofRequired` 비트 | — | **15** | 신규 집계 |
| `originId` 카탈로그 | — | **31**(사본 관계 `copiedFrom` 2건) | 신규 집계 |
| `activityBudget` 합 (탐색/추론/조작/대화/보상) | — | **53 / 190 / 168 / 28 / 41 분** | 신규 집계 · 합 480분. §10.4 R-T0-1 의 `manipulation_share` 분모·분자 |

**재실행 고지 [OBSERVED · 2026-09-10]**: 위 표의 모든 값은 이번 회차에 **위 두 명령을 그대로 실행해 재현**했다(47검사 47 PASS, `fail 0`). 이전 개정이 표 머리에 적어 두었던 sha·바이트 문자열은 RFC-B6 판정에 따라 삭제하고 검증기 출력 인용으로 대체했다 — 이 문서에는 고정 해시가 남지 않는다. 집계값은 이전 개정 대비 **전건 불변**이다(단서 73 · 염판 24 · 도구 10·11·8·3·3·7 · 비트 33).

**QA 인용값과의 차이 해명**: `qa/c3-review.md` C3-F2는 `775a984c…` 시점에 **염판 21 · 단서 72 · 도구 10·11·8·7·3·3**을 실측으로 적었다. 그 뒤 planner가 C3-F11을 닫으려 `c1-b4`에 단서 **1건**(`c1-b4-c3` · `sourceType: plate`)만 더했다 [OBSERVED `campaign.meta.md` §10 "그 외 한 글자도 바꾸지 않았다"].

| 항목 | QA(`775a984c…`) | 편집 | 기대값 | 본 시트 실측(`fdabf1d4…`) | 판정 |
|---|---|---|---|---|---|
| 단서 총수 | 72 | +1 | 73 | **73** | **일치** |
| 염판 단서 | 21 | +1 | 22 | **24** | **2 불일치** |
| 도구 6종 | 10·11·8·7·3·3 | 0 | 동일 | **10·11·8·7·3·3** | **일치** |

`775a984c…` 판은 untracked 상태로 덮어써져 working tree에 남아 있지 않아 재계산이 불가능하다 [OBSERVED `git status --short`: `planning/`은 `??`]. 도구 집계가 전건 일치하므로 **본 시트는 현재 파일 실측 24를 쓴다**.

**RFC-B6 판정 반영 [2026-09-10]**: 디렉터가 "**염판 수 24 실측 채택, `defect-register.md` C3-F2 행의 21 은 QA 가 정정**"으로 판정했다 [OBSERVED `production/decision-log.md` RFC-B6]. 따라서 §10.2 Q7 은 **닫힌다** — 본 시트는 검증기 `aggregates.sourceTypeDist.plate = 24` 를 단일 값으로 쓰고, QA 인용 21 은 [CARRIED] 역사 표기로만 남는다(본 시트 어떤 유도값의 입력도 아니다). 등록부 행 정정은 QA 레인 작업이며 밸런스는 대기하지 않는다.

**표기**: [OBSERVED] 실제 관측 · [TARGET] 설계 목표 · [INFERENCE] 문서 기반 추론 · [CARRIED] 상위 문서에서 그대로 이어받음.

## 1. G2 승률/TTK — N/A 판정 근거 [OBSERVED]

- `planning/gdd.md` §범위: "제외: **전투**, 멀티플레이, … 유료 통화, 가챠, 시즌패스". 전투 주체·피해 수치·대전 상대가 설계에 존재하지 않는다.
- `campaign.json` `kind` enum 분포 [OBSERVED §0.1 검증기 출력 `aggregates.kindCounts`]: puzzle 21 / dialogue 5 / payoff 5 / exploration 2 — **combat 0**. 33비트 어디에도 체력·피해·적 개체가 없다.
- 따라서 승률 밴드·TTK 목표·TTK 허용오차·조합 EV 상한은 **모집단 자체가 없다**. 값을 0이나 100%로 채우는 것은 측정 위장이므로 `null` + `na_reason`으로 남긴다.
- 대체 검사(계약 `## Premium overrides`): 원본 상태 비차단성(§3) · 부식 상한 안전 조건 `9 ≥ max(7,8)+1`(§4.2) · 두 보호 선택 동시 개방(§4.2) · 조위정합 판정 창(§5) · 힌트 도달성(§6) · 난이도 단조성(§7) · 진행 막힘 0(§8).

## 2. 데이터 미러 계약 [OBSERVED 경로 · TARGET 값]

숫자는 코드가 아니라 데이터 테이블에만 산다(CLAUDE.md §9).

**C3-F15 정정** [OBSERVED, `ls _workspace/current/systems/data-schemas/`]: 이전 판의 "`systems/data-schemas/`는 **현재 비어 있다**"는 사실과 반대다. `beats.md · hints.md · plates.md · save.md · tools.md · zones.md` **6종이 존재**하며 전부 `status: current`다. `systems/ops/telemetry-contract.md`도 존재한다. 따라서 아래 표는 "제안"이 아니라 **systems 스키마의 `tunable: balance` 필드와 1:1로 맞춘 미러 계약**이다.

**경로·키 규약**: 런타임 접두는 systems 소유값 `unity/Unknown/Assets/_Project/Data/Tables/`를 따른다(`data-schemas/*.md` 머리줄 [OBSERVED]). 이전 판의 `unity/Unknown/Assets/Data/Balance/*.csv`는 폐기한다(§11). 직렬화 필드는 `data-schemas/*.md` §0 규약대로 **camelCase**, 텔레메트리·문서 밴드 키는 **snake_case**다. 아래 YAML 블록도 이 두 네임스페이스를 섞지 않는다.

| 밸런스 값 | 스키마 문서 | 런타임 파일 | 필드 (`tunable: balance`) | 본 시트 |
|---|---|---|---|---|
| 원본 상태 상한 | `plates.md` | `plates.json` | `readBudget` (기본 3) | §3 |
| 관측소 오차폭 | `plates.md` | `plates.json` | `stationErrorMinutes` (기본 4.0) | §5 |
| 확정 홀드 시간 | `tools.md` | `tools.json` | `commitHoldSeconds` (기본 0.4, **opt-in**) | §8 |
| 계통별 부식 한도 | `zones.md` L42 · `tools.md` L79 | `zones.json` | `systemLimits` — **현재 `null`(표시 전용)** | §4.4 |
| 힌트 비용·페널티 | `hints.md` | `hints.json` | `cost` 0 / `achievementPenalty` 0 | §6 |
| 설계 분 | `beats.md` | `beats.json` | `designMinutes` · `minutes` · `fastMinutes` · `deliberateMinutes` | §7 |

**스키마가 아직 자리를 주지 않은 밸런스 값** — systems 조율 필요(§10 Q1, RFC-B4):

| 값 | 지금 사는 곳 [OBSERVED] | 필요한 것 |
|---|---|---|
| 전역 부식 상한 `9` | `systems/prototype/model.mjs:79` `LIMITS.corrosionLimit` | 런타임 테이블 키(`routes.json` 헤더 또는 `zones.json` 전역 행). 프로토타입 상수는 미러가 아니다 |
| 경로 선택지 비용 `7 / 8 / 12` | `model.mjs:63-65` `ROUTES[].corrosion` | `routes.json` — `routeId` · `corrosion` · `protects`. 스키마 문서 없음 |
| 자동 제안 임계 `180` · 쿨다운 `180` | `systems/system-specs/hint-system.md` §2 상태기계 | `hints.json` 헤더 또는 설정 테이블. `[tunable: balance]`는 이미 `hint-system.md` H-F5가 인정 |
| 난이도 지수·상승 규칙 | 본 시트 §7 | 런타임 미러 **없음(의도)** — 문서 게이트 전용 대리 지표 |
| 막힘 방지 불변식 | 본 시트 §8 | 임포트 검증 규칙으로 구현(`zones.md` Z-I6 · `hints.md` H-F2 형태). 테이블 값 아님 |

## 3. 원본 상태 — 법2 (`readBudget`) [TARGET]

**RFC-P3-009·법2 정본 적용**: 이전 판의 "판독 재생 예산(필요 재생 29 · 판 15매 · 여유율 ≥1.50)"은 **폐기**한다(§11). 근거는 캐논이 바뀐 것이 아니라 이전 판이 캐논을 잘못 읽은 것이다 — `worldview/worldview-bible.md` §3 법2 [OBSERVED 원문]: "원본은 닳지만 사본은 남는다 / 첫 판독 때 자동으로 검증 사본을 보존; 이후 재생은 사본 / **원본 상태 확인→사본 판독 무제한** / 필수 단서 삭제·**재생 횟수 제한 없음**". 사본 판독이 무제한이면 "재생 횟수 예산"과 그 여유율은 성립하지 않는다.

### 3.1 채택 모델 — 비차단 카운터 (economy §4.2 옵션 B)

| 축 | 값 | 근거 |
|---|---|---|
| 세는 대상 | **원본에 직접 가하는 절차 횟수**만 | `economy/currency-map.md` §4.2 옵션 B(채택), 법2 |
| 세지 않는 것 | 사본 판독·재판독·되돌림·연습·확대 관찰 | 동상. `interaction-rules.md` §2.2·§5 |
| 상한 | **3 / 판** (`readBudget` 기본값) | `systems/data-schemas/plates.md` `readBudget` 기본 3 |
| 상한 도달 시 | **진행 영향 0.** 에필로그 기록 패널의 보존 등급 문장 1줄만 바뀐다 | economy §4.2 "효과 범위" |
| 차단 여부 | **비차단**(옵션 C 차단형은 법2 위반으로 거부됨) | economy §4.2 표 |
| 자동 사본 | 첫 판독에서 항상 생성, 파괴 불가 | `plates.md` `autoCopyOnFirstRead` 항상 `true`, `indestructible` |

### 3.2 상한 3의 역산 [OBSERVED 집계 + INFERENCE 여유]

**입력 출처 (RFC-B6)**: 이 절의 비트 수·비트 id 는 §0.1 검증기 출력(`node _workspace/current/planning/validate-campaign.mjs` → `aggregates.beats = 33`)에서 읽는다. 고정 sha·바이트 문자열은 적지 않으며, 검증기가 `fail > 0` 이거나 `aggregates.beats` 가 33이 아니면 아래 표는 무효다.

live 33비트에서 **원본에 물리적으로 손을 대는 절차**를 `action`·`recovery` 전문 대조로 셌다.

| 비트 | 절차 | 원본 상태 변화 | 판정 |
|---|---|---|---|
| `t0-b3` | "원본을 판독기에 걸어 첫 판독으로 검증 사본을 자동 보존" | 접촉 1회, 마모 서술 없음 | 카운트 0 (첫 판독 = 사본 생성 절차) |
| `c1-b2` | "판독기 습도판의 단수를 스스로 골라 소금을 풀고" | **표면 처리 = 원본 직접 절차** | **카운트 1** |
| `c3-b1` | "대장을 판독대에 걸어 첫 판독으로 전 페이지를 자동 사본화" | 접촉 1회 | 카운트 0 |
| `c4-b2` | "염도와 시간을 낮은 값부터 올리며 그늘만 걷어내고" | `recovery` [OBSERVED]: "**원본은 잠금 보관 중이라 어떤 시험도 원본을 소모하지 않는다**" | 카운트 0 |
| `c5-b3` | "이웃 페이지 원본과 자동 사본을 겹쳐 일치를 확인" | 비파괴 대조 | 카운트 0 |

- 실측 원본 직접 절차 = **1회**(`c1-b2`) [OBSERVED]. economy §4.2가 근거로 든 2회 중 `c4-b2`는 live 데이터의 `recovery` 문장과 어긋난다 → economy 레인에 RFC(§10 Q3). 결론값 3은 바뀌지 않는다.
- 상한 = 관측 1 + 여유 2 = **3**. 여유 2는 (a) 미저작 DLC·재플레이 절차, (b) `plates.md` `readBudget` 기본값 3과의 일치를 위해 둔다 [INFERENCE].
- **여유율 밴드는 두지 않는다.** 비차단 카운터에는 "부족해서 막힘"이 없으므로 잰다면 잴 대상은 *도달률*이지 여유율이 아니다.

### 3.3 상한 도달 시 처리

| 상태 | 잃는 것 | 남는 것 | 진행 |
|---|---|---|---|
| 원본 절차 0~2회 | — | 원본 A등급 표기 | 정상 |
| 원본 절차 3회(상한) | 에필로그 보존 등급 문장 A → B | 자동 사본 전량, 필수 단서 전량, 세 결말 전부 | **막히지 않음** |
| 원본 소실·소각(서사 사건) | 원본 그 자체 | 자동 사본(파괴 불가)으로 확정 경로 유지 | 막히지 않음 — `c5-b3` `recovery` [OBSERVED] |

이전 판이 §3.3에서 제안했던 "4회째 원판 결정 붕괴 → 8분 분해능 강등 사본"은 **캐논 개정 제안**이었고 RFC-P3-009·법2 정본에서 불필요해졌다. 원문은 §11에 보존한다.

```yaml
system: plate_original_wear
win_rate_band: null
ttk_target_s: null
ttk_tolerance: null
combo_ev_cap_vs_median: null
na_reason: "전투·대전·DPS 없음(gdd.md 범위, campaign.json kind enum에 combat 0). 모집단 부재"
data_mirror: unity/Unknown/Assets/_Project/Data/Tables/plates.json
schema_doc: systems/data-schemas/plates.md
schema_fields:
  readBudget: 3
  autoCopyOnFirstRead: true
  indestructible: true
  resolutionMinutes: 4
blocking: false
copy_reads_unlimited: true
observed_original_procedures_in_campaign: 1
observed_original_procedure_beats: [c1-b2]
wear_cap_reached_effect: epilogue_preservation_grade_line_only
wear_cap_reached_rate_measured: null
measurement_method: "sim S1(33비트 결정론 소비) + 플레이테스트 원본 절차 로그"
```

## 4. 부식 예산 — 법5 (전역 상한 9) [RFC-P3-009 정본]

**전면 재작성 고지**: 디렉터 판정 RFC-P3-009는 economy/세션 P 모델을 정본으로 확정했다. 이전 판의 6계통 한도표(14/14/12/9/9/9) · 도구 확정별 소모표 · 장별 누적표 · 하한 돌파 모드는 **폐기**하고 §11에 보존한다. 이 재작성으로 **C3-F4가 닫히고 C3-F9(차감 시점 모순)는 자동 해소된다** — 도구 확정이 부식을 소모하지 않으므로 "T0에 봉인이 없는데 소모가 발생한다"는 모순 자체가 사라진다.

### 4.1 정의

| 축 | 값 | 근거 [OBSERVED] |
|---|---|---|
| 무엇에 걸리는가 | `routing` **구성안 1건의 총 부식 비용** | `economy/currency-map.md` §4.1, `interaction-rules.md` §2.4·§2.5 |
| 한도 구조 | **전역 단일 상한**. 계통별 차단 한도는 데이터에 존재하지 않는다 | `model.mjs:79` `LIMITS.corrosionLimit: 9` |
| 소모·누적 | **없음.** 확정해도 차감되지 않고 누적되지 않는다 | RFC-P3-009, `worldview-bible.md` §3-bis [OBSERVED]: "부식은 확정으로 소모되지 않는다 … 누적 고갈·계통 영구 고장·연습 소모·장 경계 리셋은 이 세계에 없다" |
| 리셋 | 개념 자체가 성립하지 않는다(소모가 없으므로) | RFC-P3-009 "장 경계 리셋 표현 폐기" |
| 시험 | 가상 시험대에서 **무제한·무료**, 시간·자원 비용 0 | `interaction-rules.md` §2.5, `system-specs/corrosion-budget.md` C-R4 |
| 초과 처리 | 확정 **전에** 게이트가 닫히고 초과분·원인 부품이 표시된다. 사후 페널티·영구 고장 없음 | `corrosion-budget.md` C-R3·C-R5, `model.mjs:248` `CORROSION_OVER` |
| 복구 | 무료 우회관 1회로 언제나 재구성 가능 | `interaction-rules.md` §2.5 |
| 서사 부식 | `historicalCorrosion`은 읽기 전용 증거. 운영 부식과 같은 집계에 들어가지 않는다 | `corrosion-budget.md` C-R6, `plates.md` |

### 4.2 선택지 비용과 안전 조건 [OBSERVED · model.mjs:63-65]

| `routeId` | 라벨 | `corrosion` | 보호 대상 | 상한 9 대비 여유(9 − 비용) |
|---|---|---|---|---|
| `lowland` | 저지대 주거지 우선 | **7** | `lowland` | 여유 2 · **열림** |
| `dock` | 부두 냉동창고 우선 | **8** | `dock` | 여유 1 · **열림** |
| `dock-express` | 부두 급속 증설안 | **12** | `dock` | 3 초과 · **닫힘(의도된 유혹 선택지)** |

**안전 조건 (G2 대체 검사 1)**: `corrosion_limit ≥ max(정상 선택지 비용) + 1`
→ `9 ≥ 8 + 1 = 9` **충족(경계, 여유 0)** [OBSERVED 계산].

- 정상 선택지 = 법4의 두 보호 선택(`lowland` `dock`). 둘 다 열려야 "예산이 결말을 강제하지 않는다"가 성립한다.
- `dock-express`는 예산을 넘도록 **의도된** 선택지다. 상한을 이 값에 맞춰 올리면 법4의 제로섬 선택이 무의미해지므로 `max(정상)`에서 제외한다.
- **경계 경보**: 여유가 0이므로 `dock`가 9로 오르면 권장형이 깨지고, 10이면 최소 조건(`≥ max`)까지 깨져 법4가 붕괴한다. 이 세 숫자(7·8·9) 중 하나라도 바꾸려면 balance·economy·systems 3자 RFC가 필수다(`economy/currency-map.md` §4.1 동일 결론).

### 4.3 캠페인 전체 노출

**입력 출처 (RFC-B6)**: `routing` 3 · `corrosion` 3 은 §0.1 검증기 출력 `aggregates.toolBeatCounts` 에서 읽는다(고정 sha 인용 없음). 같은 출력의 `aggregates.toolBeatMinutes` 는 `routing 52분` · `corrosion 46분` 이다 [OBSERVED 2026-09-10 재실행].

| 항목 | 값 [OBSERVED] | 비트 |
|---|---|---|
| `routing` 도구 비트 | **3** | `c5-b2` · `c5-b4` · `c7-b2` |
| 실제 계통을 바꾸는 확정 | **1** | `c5-b4` (`c5-b2`는 가상 운전, `c7-b2`는 12년 전 조건 시연) |
| `corrosion` 도구 비트 | **3** | `c2-b2` · `c2-b3` · `c4-b2` — 전부 판독·시험, 소모 0 |
| 캠페인 총 차감 | **0** (상한은 구성안마다 독립 평가) | — |

부식예산은 **경제가 아니라 한 번의 선택 제약**이다. 인플레이션·수급·회복 곡선을 잴 대상이 없다(G3 표준 항목 N/A 근거와 동일 — `economy/currency-map.md` §1).

### 4.4 계통별 분해 — 표시 전용 [TARGET · 조건부]

RFC-P3-009: 계통별 분해는 **`economy/currency-map.md` §4.1 표의 조건부 [TARGET] 표시 전용**으로만 남는다. 본 시트는 값을 정의하지 않고 그 표를 인용한다.

| 계통 | 기호 | 값 | 도입 조건 (economy §4.1) |
|---|---|---|---|
| 수문 `gate` | `L_gate` | `null` [TARGET] | `L_s ≥ c_s(정상 선택지 최대) + 1` |
| 양수 `pump` | `L_pump` | `null` [TARGET] | 〃 |
| 배수 `drain` | `L_drain` | `null` [TARGET] | 〃 |
| 전력·회선 `power` | `L_power` | `null` [TARGET] | 〃 |
| 합 | `ΣL_s` | `null` | `ΣL_s ≥ 9` (전역보다 조이면 두 보호 선택이 닫힐 수 있다) |

- 승격 절차: **표시 분해 → (프로토타입 불변식 `INV3` 일반화 후) 차단 규칙**. 그 전까지 계통 게이지는 UI 라벨이며 확정 가능성을 바꾸지 않는다.
- `systems/data-schemas/zones.md` L42 `systemLimits`(map, `tunable: balance`)와 `system-specs/corrosion-budget.md` C-R2·`save.md` `operationalCorrosion`(계통별 누계)는 **차단형 계통 한도를 전제**한다 → RFC-P3-009와 어긋나므로 systems 레인에 정정 요청(§10 Q2).

```yaml
system: corrosion_budget
win_rate_band: null
ttk_target_s: null
ttk_tolerance: null
combo_ev_cap_vs_median: null
na_reason: "전투 없음. 부식은 구성안 제약이지 피해 모델도 재화도 아니다"
data_mirror: unity/Unknown/Assets/_Project/Data/Tables/routes.json
data_mirror_status: schema_missing   # systems RFC-B4 (§10 Q1)
prototype_source: systems/prototype/model.mjs#L63-L65,L79
corrosionLimit: 9
routeCosts:
  lowland: 7
  dock: 8
  dock-express: 12
normal_route_ids: [lowland, dock]
safety_rule: "corrosionLimit >= max(normal_route_costs) + 1"
safety_margin: 0
consumes_on_confirm: false
accumulates_across_beats: false
resets_on_stage: false            # 소모가 없어 리셋 개념 미성립
practice_trials_free_unlimited: true
bypass_uses_per_config: 1
systemLimits: null                # zones.json, 표시 전용 (economy §4.1 조건부)
narrative_pattern_writable: false
open_protected_options_measured: null
measurement_method: "sim S1(3개 routing 비트 전수 + 구성 공간 완전탐색), model.mjs commitStatus 유닛테스트"
```

## 5. 조위정합 허용 오차 (법3) [TARGET]

| 키 | 값 | 근거·계산 |
|---|---|---|
| 분해능 | 4분 | worldview-bible §2 [CARRIED] |
| 관측소 간 드리프트 | ±40분 | worldview-bible §2 [CARRIED] |
| 후보 슬롯 수 | **21** | (40÷4)×2+1 = 21 |
| 합격 잔차 | \|r\| ≤ 4분 | QA F3 수정 요구 "정합 후 허용 잔차 상한 ≤4분" |
| 총 오차폭 | 8분 | ±4분 |
| 근접 실패대 | 4 < \|r\| ≤ 8분 | "한 칸 차이" 피드백만, 정답 미노출 |
| 명백 실패 | \|r\| > 8분 | 방향(빠름/느림) 1가지만 알려줌 |
| 필수 자료 수 | 2종 이상, **모두** 잔차 ≤ 4분 | timeline §3 "매체 2종 대조" |
| 무작위 정답 확률 | 1/21 = 4.76% (2자료 동시 1/441 = 0.23%) | 추측 풀이 차단 근거 |
| 확정 전 미리보기 | 필수 | campaign.meta 불변식 4 |
| 실패 비용 | 부식 0 · 원본 상태 0 · 되돌림 무료 | 법3 실패조건은 "인과 역전"이지 자원 소실이 아니다 |

관측소별 고정 오프셋(예: 표성찬 대장의 기준 관측소)은 `plates.json` `stationId`에 딸린 값으로 두되 **값 자체는 세계관·시놉시스 레인 소유**다. 밸런스는 허용 범위(|offset| ≤ 40, 4의 배수)와 오차폭(`stationErrorMinutes` 기본 4.0)만 규정한다.

**프로토타입 상수 불일치 [OBSERVED · 미해소]**: `systems/prototype/model.mjs:79`는 `offsetMin: -45 / offsetMax: 45`다. 캐논은 ±40분(`worldview-bible.md` §2·법3 [OBSERVED])이며 ±45는 4분 격자로 나눠떨어지지도 않는다(45/4 = 11.25). 본 시트는 **캐논 ±40을 유지**하고 후보 슬롯 21을 그대로 쓴다. 프로토타입 상수 정정은 systems 레인 RFC(§10 Q6)이며, 해소 전까지 S2 시뮬의 입력 범위는 확정된 것이 아니다.

```yaml
system: tide_alignment
win_rate_band: null
ttk_target_s: null
ttk_tolerance: null
combo_ev_cap_vs_median: null
na_reason: "전투 없음. 정합은 판정 창이지 명중 판정이 아니다"
data_mirror: unity/Unknown/Assets/_Project/Data/Tables/plates.json
schema_doc: systems/data-schemas/plates.md
schema_fields:
  stationErrorMinutes: 4.0
  resolutionMinutes: 4
alignment_resolution_min: 4
alignment_drift_range_min: 40
alignment_candidate_slots: 21
alignment_pass_residual_min: 4
alignment_nearmiss_residual_max_min: 8
alignment_required_sources: 2
alignment_station_offset_min_range: [-40, 40]   # 캐논. model.mjs 는 ±45 (§10 Q6 미해소)
alignment_preview_before_confirm: true
alignment_fail_cost_corrosion: 0
alignment_fail_cost_original_wear: 0
alignment_first_try_pass_rate_target: null
measurement_method: "sim S2(21슬롯 완전탐색 + 무작위 대조군) + 플레이테스트 시도 횟수 분포"
```

## 6. 힌트 — 수동 3단계 + 자동 제안 1종 [RFC-P3-015 정본]

**C3-F20 해소**: 이전 판의 3단 **자동 승격** 임계표(T1 180초 / T2 누적 420초 + 오확정 2회 / T3 누적 900초 + 오확정 4회 + 확인 클릭)를 **폐기**한다(§11). 자동 제안 모델은 하나뿐이다.

### 6.1 두 축을 분리한다

| 축 | 규칙 | 소유 |
|---|---|---|
| **수동 열람 3단계** | 0초부터 항상 가능. 무료·무제한·업적 페널티 0. 1 → 2 → 3 **순서대로만** 열리며 3단은 열기 전 명확한 경고. **플레이어가 연다** | 내용은 systems(`hint-system.md` H-R1~R3), 비용은 economy(`hints.md` `cost`·`achievementPenalty`) |
| **자동 제안 1종** | 무진전 **180초** → 비강제 토스트 1줄("도움 보기"). 무시하면 **180초 쿨다운** 후 재제안. 자동으로 단계를 올리지 않는다 | 임계값은 balance (`hint-system.md` H-F5 `[tunable: balance]`) |

`systems/system-specs/hint-system.md` §2 상태기계와 1:1 대응 [OBSERVED]: `Closed --(idleSeconds ≥ 180)--> Offered`, `Offered --(Dismiss)--> Closed` "다음 제안까지 180초", `any --(BeatChanged)--> Closed`. 단계 승격은 `RevealHint(level+1)` **플레이어 입력**으로만 일어난다.

### 6.2 임계 정의

| 키 | 값 | 근거 |
|---|---|---|
| 무진전 제안 임계 | **180초** | `puzzle-balance.md` 초안 "3분 무진전 감지" 계승 · `hint-system.md` §2 |
| 재제안 쿨다운 | **180초** | `hint-system.md` §1 "재제안은 다음 3분 후" · H-F5 |
| 세션당 제안 상한 | `null` (미정) | H-F5가 "세션당 상한 `[tunable: balance]`"을 요구하나 근거 표본 n=0 → 값을 지어내지 않는다(§10 Q4) |
| 타이머 범위 | 비트 경계에서 리셋 | `hint-system.md` §2 `BeatChanged` |

**무진전(`no_progress`) 정의**: 새 단서 열람 0 ∧ 유효 조작 0 ∧ 도구 창 전환 ≤ 1.
**타이머 정지 조건**: 단서·대사 텍스트 표시 중, 확대 뷰 관찰 중, 일시정지 중. **생각하는 시간을 실패로 세지 않는다.**
60초 이상 무입력 구간은 `afk_gap`으로 별도 기록되며 **자동 제외하지 않는다**(`ops/telemetry-contract.md` §5, RFC-P3-011과 같은 원칙).
힌트 사용은 완주 시간 목표(계약 §Time acceptance)에 벌점으로 들어가지 않는다.

### 6.3 향후 텔레메트리 밴드 [TARGET, 측정 전이므로 판정 불가]

| 지표 | 밴드 | 의미 |
|---|---|---|
| 비트당 3단 열람률 | ≤ 25% | 초과 시 그 비트의 정보 설계를 재검토 |
| 단일 비트 3단 열람률 | < 50% | 이상이면 그 비트 재설계 |
| 자동 제안 수락률 | ≥ 5% | 미만이면 180초가 너무 이르다는 신호 |
| `stuck_after_l3` 발생 | **0건** | 1건이라도 나오면 G7 대체 검증 FAIL (`hint-system.md` H-F1) |

```yaml
system: hint_exposure
win_rate_band: null
ttk_target_s: null
ttk_tolerance: null
combo_ev_cap_vs_median: null
na_reason: "전투 없음. 힌트는 접근성 장치이며 승패 확률에 관여하지 않는다"
data_mirror: unity/Unknown/Assets/_Project/Data/Tables/hints.json
schema_doc: systems/data-schemas/hints.md
schema_fields:
  cost: 0
  achievementPenalty: 0
  affectsEnding: false
  warnsBeforeReveal: true          # level 3
  revealsValues: true              # level 3 에서만 허용
hint_tiers: 3
hint_reveal_order_enforced: true
hint_manual_available_from_s: 0
hint_use_limit: null
hint_auto_offer_model: single_offer_with_cooldown
hint_offer_idle_s: 180
hint_offer_cooldown_s: 180
hint_offer_per_session_max: null   # 근거 없음, 지어내지 않음 (§10 Q4)
hint_auto_reveal: false
hint_idle_pause_on_read: true
hint_timer_reset_scope: beat
hint_t3_open_rate_target_max: 0.25
hint_t3_open_rate_measured: null
hint_offer_accept_rate_target_min: 0.05
stuck_after_l3_target: 0
measurement_method: "sim S3(임계 민감도 스윕) + 플레이테스트 힌트 로그·이탈 시점"
```

## 7. 난이도 곡선 [재도출 · C3-F31 판정 적용 · 2026-09-10]

**이번 개정의 근거** [OBSERVED]: 디렉터 판정 **C3-F31**(`production/decision-log.md` "C3 종료 판정 묶음") — *미측정 열(`동시 가설 수` [INFERENCE])은 지수에서 **제외**한다. 지수 = 관측 가능한 2열(최대 단서 수·최대 도구 수)의 합, live 재계산 `[4,5,5,4,4,5,7,4,3]`, C5→C6 +2 는 규칙 통과. 세 번째 열은 [TARGET] 주석으로만 남긴다.* 이전 판의 3열 지수 `[5,7,7,7,7,8,11,8,4]`와 그로부터 나온 "C5→C6 = +3 위반(FIX)" 판정은 값을 지우지 않고 §11 폐기 원장으로 내린다. RFC-Q2에 따라 같은 `cycle` 값 안의 제자리 개정이므로 `supersedes`는 `null` 유지다.

**입력 재측정 영수증** [OBSERVED · 2026-09-10]. RFC-Q1("이후 인용은 `validate-campaign.mjs` 출력의 sha를 쓴다 — 고정 숫자 재기재 금지")에 따라 해시를 옮겨 적지 않고 검증기를 재실행했다.

```
$ node _workspace/current/planning/validate-campaign.mjs        # exit 0
  "sha256": "92301c0a5ecfc7e142646f5f17c29c21609e2b2de7c90408de7bffc9d815ae23", "bytes": 121457,
  "summary": { "checks": 47, "pass": 47, "fail": 0, "verdict": "PASS" }
$ node -e 'const j=require("./_workspace/current/planning/campaign.json");const r=j.stages.map(s=>Math.max(...s.beats.map(b=>b.clues.length))+Math.max(...s.beats.map(b=>b.tools.length)));console.log(JSON.stringify(r));'
[4,5,5,4,4,5,7,4,3]
$ node -e 'const j=require("./_workspace/current/planning/campaign.json");console.log(j.stages.map(s=>`${s.id}:${Math.max(...s.beats.map(b=>b.clues.length))}/${Math.max(...s.beats.map(b=>b.tools.length))}`).join(" "));'
T0:2/2 C1:3/2 C2:3/2 C3:2/2 C4:2/2 C5:3/2 C6:4/3 C7:2/2 E0:2/1     # 스테이지 id 는 대문자, 비트 id 는 소문자(t0-b1…)
```

| 확인 | 값 |
|---|---|
| live 정본 (검증기 출력) | sha256 `92301c0a…` · **121457 B** · 47검사 47 PASS [OBSERVED] |
| §0.1 영수증이 적은 sha | ~~`fdabf1d4…` · 120479 B · 44검사~~ — **RFC-B6 적용 후 §0.1 에는 고정 해시가 없다**(검증기 출력 인용으로 교체). 이 행은 교체 이전 상태의 역사 기록이다 [CARRIED] |
| 해시가 움직인 사유 | 타 레인 편집(C3-F22 비트 `zoneId` 필드 추가 · RFC-W4 의도 문장 `c4-b3`→`c6-b4` 이동). 검증기 신설 검사 `Z-01`·`Z-02`·`K-06`이 그 편집을 확인한다 [OBSERVED 검증기 출력] |
| §0.1 집계와의 대조 | 단서 **73** · `sourceType` **log 27 / ledger 22 / plate 24** · 도구 `circuit 10 · reader 11 · alignment 8 · routing 3 · corrosion 3 · seal 7` · 비트 33 · 체크포인트 33 · `proofRequired` 15 · `originId` 31 · fast/deliberate 322/673 — **전건 불변** [OBSERVED, 위 검증기 `aggregates`] |
| 따라서 | 밸런스가 쓰는 입력 열(`clues` 수·`tools` 수)은 바뀌지 않았고 §7 재계산 입력은 유효하다. **[2026-09-10 R7] RFC-B6 승인 적용 완료** — §0.1·§3.2·§4.3 의 sha·바이트 문자열은 검증기 출력 인용으로 교체됐다(§10.1). 본 절의 위 영수증 블록은 R7 에 **같은 명령으로 재실행해 동일 출력**을 확인했다(`summary` 47/47 PASS, `sha256` 불변) [OBSERVED] |

### 난이도 지수 표 (관측 2열의 합)

| 스테이지 | 최대 단서 수 [OBSERVED] | 최대 도구 수 [OBSERVED] | **난이도 지수** | 증감 | 최대 부하 비트 [OBSERVED] | 사유 |
|---|---|---|---|---|---|---|
| T0 | 2 | 2 | **4** | — | `t0-b3` (2 + reader·circuit) | 법1·법2, 도구 2종 도입 |
| C1 | 3 | 2 | **5** | +1 | `c1-b3` (3 + seal·reader) | 법6 도입 + 단서 3건 비트 등장(`c1-b3`·`c1-b4`) |
| C2 | 3 | 2 | **5** | 0 | `c2-b3` (3 + corrosion·circuit) | 같은 부하 유지, 부식 도구로 조합만 교체 |
| C3 | 2 | 2 | **4** | −1 | `c3-b1` (2 + reader·circuit) | 정합 도구 단독 학습 구간(`c3-b2` guided) — 단서 폭을 줄여 새 도구에 집중 |
| C4 | 2 | 2 | **4** | 0 | `c4-b1` (2 + reader·alignment) | R2 사실 회수. 부하 평탄, 조합만 교체 |
| C5 | 3 | 2 | **5** | +1 | `c5-b1` (3 + circuit·alignment) | 법4 제로섬, 경로 도구 도입(`c5-b2` guided) |
| C6 | 4 | 3 | **7** | **+2** | `c6-b3` (4 + alignment·seal·circuit) | 33비트 중 **유일한 단서 4 + 도구 3** 비트. 상승 폭 규칙의 **경계값** |
| C7 | 2 | 2 | **4** | −3 | `c7-b2` (2 + routing·alignment) | 종합 → 제출로 부하 전환(비트당 2+2 상한) |
| E0 | 2 | 1 | **3** | −1 | `e0-b2` (2 + seal) | 완결·정리(2비트 10분) |

- 지수 = `max(스테이지 내 비트 단서 수) + max(스테이지 내 비트 도구 수)`. 두 최댓값이 서로 다른 비트에서 나올 수 있으므로 **포락(envelope)** 값이다.
- 포락값이 과대평가인지 확인했다 [OBSERVED]: 스테이지별 `max(단서+도구)`(단일 비트 최대 부하)는 `4 5 5 4 4 5 7 4 3`으로 **9스테이지 전건 포락값과 동일**하다 — 즉 이 표의 지수는 실제로 한 비트가 동시에 지는 부하다. 명령(그대로 재현 가능): `node -e 'const j=require("./_workspace/current/planning/campaign.json");console.log(j.stages.map(s=>Math.max(...s.beats.map(b=>b.clues.length+b.tools.length))).join(" "));'`
- 지수는 난이도의 **대리 지표**이며 체감 난도의 측정치가 아니다. 사람 표본 **n=0**, 빌드 0, 시뮬 0회.

### 7.1 상승·하락 규칙 검사 [OBSERVED 유도]

**규칙**: 상승 폭 ≤ 2/스테이지(급경사 금지) · 하락 구간 ≤ 2개(각 하락은 사유 기재).

증감 수열 [OBSERVED]: `[—, +1, 0, −1, 0, +1, +2, −3, −1]`

| 검사 | 측정 | 판정 |
|---|---|---|
| 최대 상승 폭 ≤ 2 | **C5 → C6 = +2** (그 외 최대 +1) | **통과** — 단, 여유 0인 **경계값** |
| 단조성(역전 없음) | 상승 3구간·평탄 2구간·하락 3구간, 스테이지 순서 역전 없음 | 통과 |
| 하락 구간 ≤ 2개 | **3개** — C2→C3 `−1` · C6→C7 `−3` · C7→E0 `−1` | **판정 보류(RFC-B5)** — 아래 |
| 각 하락에 사유 기재 | 3건 전건 기재(위 표 사유 열) | 통과 |

**C5→C6 = +2 통과의 의미와 한계** [OBSERVED]: 이전 판의 위반(+3)은 미측정 열이 만든 것이었고, 관측 2열만으로는 규칙을 통과한다. 따라서 §10.2 Q5가 planner에게 요구하던 **A안(`c6-b3`의 `seal`을 `c6-b4`로 분리)은 근거가 사라졌다 — 요구를 철회한다.** 다만 여유가 0이므로 다음 두 편집 중 하나만 일어나도 다시 위반이 된다: (a) `c6-b3`에 단서·도구가 1개라도 추가, (b) C5의 최대 단서 3 또는 최대 도구 2가 감소. planner가 C6·C5를 손대면 이 표를 재집계해야 한다(`sim-results/README.md` §1 만료 규칙).

**하락 구간 3개를 위반으로 적지 않고 보류하는 이유** [INFERENCE·명시]: 임계 "≤ 2개"는 **폐기된 3열 지수** 위에서 정한 값이다. 그 지수에서 C2→C3은 `7→7`(평탄)이었고 하락은 2구간이었다. 열을 하나 빼면 같은 곡선의 하락 구간 수가 3으로 늘어난다 — 즉 이 임계는 2열 지수에 대해 **한 번도 재보정된 적이 없다**. 그렇다고 결과에 맞춰 임계를 넓히는 것은 계약 `## Honesty gates` 위반이므로 밸런스 단독으로 채택하지 않는다. **RFC-B5(디렉터)** 로 올린다:

| 안 | 내용 | 판정 시 결과 |
|---|---|---|
| B5-a | 임계 "≤ 2개"를 문자 그대로 유지 | 하락 축 **위반 1건** → 해소는 콘텐츠 편집(−1 딥 제거)뿐이며, 문서 지표를 위해 학습 구간(C3)을 바꾸는 셈이 된다 |
| B5-b (밸런스 권고) | 임계를 **"유의 하락(\|Δ\| ≥ 2) 구간 ≤ 2개 + 모든 하락에 사유 기재"** 로 재정의 | 측정값 = 유의 하락 **1개**(C6→C7 −3) → 통과. 재정의 근거는 "±1은 도구 학습 구간의 조합 교체에서 나오는 잔물결"이라는 것이며, **결과가 통과하도록 넓히는 것으로 읽힐 수 있음을 여기 명시한다** |
| B5-c | 하락 축 임계를 **삭제**하고 "모든 하락에 사유 기재"만 남긴다 | 개수 제약이 사라짐. 급락(C6→C7 −3)을 잡을 축도 함께 사라진다 |

**본 개정의 상태**: 상승 축 **통과**[OBSERVED], 하락 축 **미판정**. 그러므로 §9의 `난이도 단조성`은 여전히 **전건 PASS가 아니다**. 다만 이전 판과 달리 남은 것은 콘텐츠 결함이 아니라 **규칙 문구의 보정 여부**다.

### 7.2 지수에서 분리한 열 — `동시 가설 수` [TARGET · 지수 미포함]

| 스테이지 | T0 | C1 | C2 | C3 | C4 | C5 | C6 | C7 | E0 |
|---|---|---|---|---|---|---|---|---|---|
| 동시 가설 수 [TARGET] | 1 | 2 | 2 | 3 | 3 | 3 | 4 | 4 | 1 |

- 출처: `worldview/timeline.md` §3의 "의도적으로 남는 오해" + 미회수 반전 수를 손으로 센 값이다. **재현 가능한 명령이 없다** — QA C3-F31 요구 (1)이 지적한 그대로다.
- 지위: C3-F31 판정에 따라 **지수에 더하지 않는다.** 위 값은 설계 의도의 기록([TARGET])이며 게이트 판정에 쓰지 않는다.
- 재도입 조건: (a) 산출 규칙을 `campaign.json` 필드에서 유도하는 명령으로 정의하고, (b) T0 실측(플레이테스트) 후 체감과의 상관을 확인한다. 둘 다 충족되기 전에는 재도입 금지.

```yaml
system: difficulty_curve
win_rate_band: null
ttk_target_s: null
ttk_tolerance: null
combo_ev_cap_vs_median: null
na_reason: "전투 없음. 난도는 정보 처리 부하로 정의한다"
data_mirror: null                     # 문서 게이트 전용 대리 지표 (§2)
source_json_sha256: 92301c0a5ecfc7e142646f5f17c29c21609e2b2de7c90408de7bffc9d815ae23   # validate-campaign.mjs 출력 (RFC-Q1)
source_json_bytes: 121457
difficulty_index_formula: "max_clues_in_stage + max_tools_in_stage"   # C3-F31: 미측정 열 제외
difficulty_index_by_stage: [4, 5, 5, 4, 4, 5, 7, 4, 3]
difficulty_index_equals_single_beat_peak: true       # 9/9 스테이지 일치 [OBSERVED]
difficulty_max_rise_per_stage: 2
difficulty_observed_max_rise: 2                      # C5->C6 (경계값)
difficulty_rise_headroom: 0
difficulty_rise_rule_status: PASS                    # [OBSERVED 집계] C3-F31 판정 적용 후
difficulty_max_drop_segments: 2                      # 3열 지수 기준 임계. 2열 지수용 재보정 없음
difficulty_observed_drop_segments: 3                 # C2->C3 -1, C6->C7 -3, C7->E0 -1
difficulty_observed_significant_drop_segments: 1     # |delta| >= 2 기준 (RFC-B5-b 가정 시)
difficulty_drop_rule_status: RFC                     # RFC-B5 디렉터 판정 대기. 임계 완화 미적용
difficulty_hypotheses_column: [1, 2, 2, 3, 3, 3, 4, 4, 1]   # [TARGET] §7.2, 지수 미포함
difficulty_hypotheses_in_index: false
difficulty_perceived_measured: null
measurement_method: "validate-campaign.mjs aggregates + 스테이지별 max(clues)/max(tools) 재집계(문서 게이트). 플레이테스트 비트별 소요·재시도 분포는 미실행"
```

### 7.3 `puzzle-balance.md` 대조 [OBSERVED · R4 QA 대비]

선행 초안 `balance/puzzle-balance.md`(`status: draft`, C4 — 본 개정에서 상태·본문 모두 건드리지 않음)의 복잡도 대역과 위 실측을 맞춰봤다.

| 초안 대역 (자료 수) | 대응 스테이지 [INFERENCE] | live 최대 단서 수 [OBSERVED] | 일치 |
|---|---|---|---|
| T0 `2자료` | T0 | 2 | ○ |
| 초반 `2~3자료` | C1 · C2 | 3 · 3 | ○ |
| 중반 `3~4자료` | C3 · C4 · C5 | **2 · 2** · 3 | **C3·C4가 하한 미달(1 부족)** |
| 종합 `4~6자료` | C6 (제출 전 최대 부하) | 4 | ○ (하한 일치, 상한 미도달) |

- 모순 1건 [OBSERVED]: 초안의 중반 대역 하한 3에 대해 C3·C4의 실측 최대 단서 수는 2다. 우연이 아니라 설계 의도로 보인다 — C3는 정합 도구, C4는 부식/봉인 조합을 **각각 단독 학습**시키는 구간이라 단서 폭을 좁혔다(§7.1 사유 열).
- 처리: 초안은 `status: draft`이고 그 값은 [TARGET]이므로 **정본은 본 시트 §7**이다(초안 머리 고지와 동일). 초안 본문 정정은 C3-F33(초안의 `status: current` 승격) 절차에서 R5에 수행하며, 본 개정은 지시대로 초안의 `status`를 바꾸지 않는다. 그때까지 다른 레인은 초안이 아니라 §7을 인용한다.
- 그 밖의 축(전제 수·인과 단계 수)은 `campaign.json`에 대응 필드가 없어 대조 불가다 — 지어내지 않는다.

## 8. 막힘 방지 불변식 [TARGET]

| 규칙 키 | 값 | 근거 |
|---|---|---|
| `safety_practice_mode_unlimited` | true (연습 조작 무제한, 소모 0) | gdd C2 시스템 쟁점, economy 문서 |
| `safety_essential_clue_indestructible` | true (첫 판독 자동 사본은 어떤 행동으로도 파괴 불가) | QA F5, campaign.meta 불변식 3 |
| `safety_two_disjoint_media_paths` | 모든 필수 확정(`proofRequired` **15**개 [OBSERVED §0.1])에 **`sourceType` 상이 AND `originId` 상이**를 만족하는 자료쌍이 최소 1개 존재하고, 그 쌍의 두 경로 중 1개는 어떤 플레이어 행동으로도 파괴되지 않는다 | `unity-implementation.md` L69 불변식 1(정본 표현), `interaction-rules.md` §3, `worldview-bible.md` P2 |
| `safety_critical_path_resource_dependency` | none (소모성 자원이 존재하지 않으므로 소진으로 막힐 수 없다) | RFC-P3-009 §4: 확정 무소모 · §3: 원본 카운터 비차단 |
| `safety_open_protected_routes_min` | 2 (`lowland` 7 · `dock` 8 모두 상한 9 이내) | §4.2 [OBSERVED `model.mjs:63-65`] |
| `safety_checkpoint_count` | 33 (비트마다 1개) | `campaign.json` checkpoint 33 [OBSERVED §0.1 검증기 출력] |
| `safety_rollback_depth_beats` | ≥ 1 | 확정 직전 자동 백업(c5-b4/c7-b4/e0-b2) |
| `safety_endings_reachable_min` | 3/3 (어떤 자원 상태에서도) | worldview §6 · `model.mjs` `ENDINGS` 3종 [OBSERVED] |
| `safety_progress_block_count_target` | 0 | G7 대체 검사 |
| `safety_progress_block_count_measured` | null | 플레이 n=0 |

## 9. 측정 방법 (밴드별)

| 밴드 | 방법 | 현재 상태 |
|---|---|---|
| 원본 상태 상한 3 도달률 | S1 결정론 소비 + 원본 절차 로그 | 미실행 (비차단이므로 게이트 차단 요인 아님) |
| 부식 안전 조건 `9 ≥ max(7,8)+1` | 산술 검증 + `model.mjs commitStatus` 유닛테스트 | **본 문서에서 1회 수행** [OBSERVED 계산] · 유닛테스트 미실행 |
| 두 보호 선택 동시 개방 ≥ 2 | 구성 공간 완전탐색(S1) | 미실행 (문서 계산으로는 충족) |
| 정합 판정 창 (21슬롯·잔차 ≤4) | S2 완전탐색 + 무작위 대조군 | 미실행 · 입력 상수 불일치 미해소(§5 주) |
| 힌트 임계 180/180 | S3 민감도 + 실제 텔레메트리 | 미실행 |
| 난이도 단조성 | `campaign.json` 재집계(문서 게이트) | **본 문서에서 2회 수행**(2026-09-10 재집계 포함) [OBSERVED 집계] → **상승 축 통과**(C5→C6 = +2, 여유 0) · **하락 축 미판정**(RFC-B5) — §7.1 |
| 진행 막힘 0 | 플레이테스트 12명/5유형 | n=0 |
| `stuck_after_l3` 0건 | 텔레메트리 (`hint-system.md` H-F1) | n=0 |

**게이트 정직성**: 위 표에서 "본 문서에서 수행"이라고 적힌 두 줄만 [OBSERVED]다. 나머지는 실행 0건이며, 시뮬 하네스 자체가 존재하지 않는다(`balance/sim-results/README.md` §0). G2는 "밴드가 내부적으로 성립한다"까지만 말할 수 있고, 그 조차 §7.1의 **하락 축 미판정(RFC-B5)** 때문에 현재 **전건 PASS가 아니다**. C3-F31이 지목한 상승 축 위반은 닫혔지만(미측정 열 제외 후 +2), 그것으로 G2가 오르지는 않는다 — 런타임 축은 여전히 전건 n=0이다.

## 10. 미측정 · 열린 질문

### 10.1 이번 회차에 닫힌 것

| 결함 | 처리 |
|---|---|
| C3-F2 | §0.1 재집계 영수증으로 `2bfe4d52…` 폐기, `fdabf1d4…`(120479 B)로 전 집계 재측정. 전체 해시 기재. 잔여 1건: 염판 수치가 QA 인용과 2 어긋남 → Q7 |
| C3-F4 | RFC-P3-009 적용 — §4 전면 재작성(전역 상한 9). 6계통 한도표 폐기 |
| C3-F9 | §4 재작성으로 **자동 해소** — 도구 확정이 부식을 소모하지 않으므로 차감 시점 충돌 자체가 소멸 |
| C3-F15 | §2에서 "`data-schemas/` 비어 있다" 스테일 [OBSERVED] 정정 (6종 존재) |
| C3-F19 | §0.1이 `seal` **7**(확정 5 + 연습 2)을 실측으로 확정. 본 시트는 `seal` 횟수로 자원을 계산하지 않으므로 파생 영향 0 |
| C3-F20 | RFC-P3-015 적용 — §6을 180초 단일 제안 + 180초 쿨다운으로 통일, 3단 자동 승격 임계 폐기 |
| **C6-F5** [2026-09-10] | **문서로 닫지 않는다** — 디렉터 판정에 따라 **위험 R-T0-1 로 등록**(§10.4). 재계산 명령·값·T0 관측 지표 `manipulation_share` 정의를 §10.4 에 적었다. 재설계 여부는 T0 사람 검증 결과 후 [OBSERVED `production/decision-log.md` "C6-F5 · 조작 밀도(표·칸 21/33)"] |
| **RFC-B6** [2026-09-10] | 디렉터 승인 적용 — §0 정본 행 · §0.1 영수증 · §3.2 · §4.3 의 고정 sha/바이트 문자열을 **검증기 출력 인용**으로 교체. 본 시트에 고정 해시 0건. 집계는 재실행으로 전건 재현(47검사 47 PASS) |
| **Q7** [2026-09-10] | RFC-B6 판정("염판 24 실측 채택, 등록부 C3-F2 행의 21 은 QA 가 정정")으로 **닫힘**. 본 시트는 `aggregates.sourceTypeDist.plate = 24` 단일 값 사용 |
| **C3-F31** [2026-09-10 개정] | 디렉터 판정 적용 — §7 지수에서 미측정 열(`동시 가설 수`) 제외, live 재집계 `[4,5,5,4,4,5,7,4,3]`(sha `92301c0a…` 47/47 PASS), **C5→C6 = +2 규칙 통과**. 세 번째 열은 §7.2에 [TARGET]으로 분리 보관. QA 요구 (1)(산출 규칙 정의 또는 분리) = **분리**로 충족. 잔여: 하락 구간 임계 재보정(RFC-B5) |

### 10.2 열린 질문 (RFC 대상)

| id | 대상 레인 | 질문 | 본 시트의 잠정 처리 |
|---|---|---|---|
| Q1 | systems | 전역 부식 상한 9와 경로 비용 7/8/12가 프로토타입 상수(`model.mjs`)에만 있다. 런타임 데이터 테이블(`routes.json`)과 스키마 문서가 필요하다 — CLAUDE.md §9("튜닝을 코드에 하드코딩하지 않는다") 위반 소지 | §4 YAML `data_mirror_status: schema_missing` |
| Q2 | systems | `zones.md` L42 `systemLimits`·`corrosion-budget.md` C-R2·`save.md` `operationalCorrosion`(계통별 누계)이 **차단형 계통 한도와 누적 소모**를 전제한다. RFC-P3-009는 둘 다 없다고 판정했다 | §4.4에서 `systemLimits: null`(표시 전용)로 고정하고 정정 요청 |
| Q3 | economy | §4.2 옵션 B의 상한 3 근거 "원본 직접 절차 2회(`c1-b2`·`c4-b2`)" 중 `c4-b2`는 live `recovery`가 "어떤 시험도 원본을 소모하지 않는다"로 명시한다. 실측은 1회 | 상한 3은 유지(관측 1 + 여유 2), 근거 문장만 정정 요청 |
| Q4 | systems | `hint-system.md` H-F5의 "세션당 제안 상한 `[tunable: balance]`"에 줄 근거가 없다(표본 n=0) | `hint_offer_per_session_max: null`. 값을 지어내지 않는다 |
| Q5 | planner | ~~§7.1 난이도 상승 폭 위반(C5→C6 = +3)~~ → **철회 [2026-09-10]**. C3-F31 판정으로 지수에서 미측정 열을 빼면 +2로 통과하므로 A안(`seal`을 `c6-b4`로 분리)의 근거가 사라졌다 | **요구 철회.** planner는 `campaign.json`을 이 사유로 재저작하지 않는다. 단 C5·C6의 단서/도구 수를 바꾸면 §7을 재집계해야 한다(여유 0) |
| Q6 | systems / worldview | §5 정합 상수 불일치: 캐논 드리프트 ±40분(`worldview-bible.md` §2·법3)과 `model.mjs:79` `offsetMin/Max: ±45`가 다르다. ±45는 4분 격자로 나눠떨어지지도 않는다 | 캐논 ±40 유지, 후보 슬롯 21 유지. 프로토타입 상수 정정 요청 |
| **RFC-B5** | director (cc: qa, planner) | §7.1 하락 구간 임계 "≤ 2개"는 **폐기된 3열 지수** 위에서 정해졌고 2열 지수용으로 재보정된 적이 없다. 2열 지수의 실측 하락 구간은 **3개**(−1 / −3 / −1) | 세 안(B5-a 문자 유지 · **B5-b 유의 하락 \|Δ\| ≥ 2 기준 재정의(권고)** · B5-c 개수 제약 삭제)을 §7.1에 적었다. 밸런스 단독으로 임계를 넓히지 않는다 |
| ~~**RFC-B6**~~ **[closed 2026-09-10]** | director (cc: qa) | *(승인·적용 완료 — §10.1 참조. 원문 보존)* live `campaign.json` sha가 `fdabf1d4…`(120479 B, 44검사) → **`92301c0a…`(121457 B, 47검사)** 로 움직였다(C3-F22 `zoneId` · RFC-W4 문장 이동). 밸런스 입력 집계는 **전건 불변**이나 §0.1·§3.2·§4.3의 sha 문자열은 이전 개정 값이다 | 본 개정은 C3-F31이 지목한 §7만 손댔다. §0.1·§3.2·§4.3의 sha 갱신(집계 재측정 불필요, 문자열 교체) 승인 요청 |
| **Q9** | director (cc: qa) | RFC-B6 은 §0.1·§3.2·§4.3 만 지목했다. §7 의 재측정 영수증 블록과 §7 YAML `source_json_sha256`·`source_json_bytes` 에는 **여전히 고정 해시·바이트가 적혀 있다**(그 값은 R7 재실행에서 불변으로 확인됨). 영수증은 출력을 그대로 보이는 것이 목적이라 그대로 두는 것이 맞는지, YAML 필드까지 "검증기 출력 인용"으로 바꿔야 하는지 | **현행 유지**(값이 재현되므로 스테일 아님). 확장 판정이 오면 그때 교체한다. 밸런스 단독으로 QA 검증된 영수증을 고쳐 쓰지 않는다 |
| **Q8** | director (cc: qa) | `decision-log.md` C6-F5 판정문의 "61%"가 재현 명령 결과 **63.6%(21/33)** 와 어긋난다. QA `c6-review.md` 원문도 21/33 | 본 시트는 63.6% 사용(§10.4 R-T0-1 에 명령·출력 첨부). 판정문 문자열 정정 요청 |
| ~~Q7~~ **[closed 2026-09-10]** | qa | *(RFC-B6 판정으로 닫힘 — 염판 24 채택. 원문 보존)* C3-F2의 염판 21이 `fdabf1d4…` 실측 24와 **2** 어긋난다(단서 1건 추가로는 1만 설명된다). 도구 6종 집계는 전건 일치 | 실측 24 채택. `defect-register.md` C3-F2 행의 염판 수치 재확인 요청 |

### 10.3 미측정 [OBSERVED]

1. 사람 플레이 n=0, 빌드 n=0, 밸런스 시뮬 실행 0건. §3·§4·§5·§6의 모든 밴드는 계산으로 유도한 [TARGET]이다.
2. 오확정률·재작업률은 **가정조차 폐기**됐다 — 부식이 확정으로 소모되지 않으므로 40% 재작업 여유라는 개념이 없어졌다(§11).
3. 관측소 오프셋 실제 값이 정해지면 §5의 후보 슬롯 21이 스테이지별로 달라질 수 있다.
4. 원본 상태 상한 3의 여유 2는 미저작 콘텐츠를 위한 [INFERENCE]다. DLC 절차가 저작되면 재산정한다.
5. 본 시트의 유도값은 `campaign.json`의 `tools` / `clues[].sourceType` 구조에 묶여 있다. **검증기 출력의 `sha256` 이 바뀌면 §0.1·§3.2·§4.3·§7을 재집계해야 하며, 재집계 없는 인용은 무효다**(RFC-B6 이후에도 이 규칙은 그대로 — 다만 해시 문자열을 문서에 적어 고정하지는 않는다)(`sim-results/README.md` §1 만료 규칙).
6. **조작 유형별 손 조작의 재미는 측정된 적이 없다** — 표·칸 21/33(63.6%)은 `subtasks` **문자열** 집계이며 손동작의 동일성 증명이 아니다. 조작 유형 분류 열은 T0 관측 전에는 만들지 않는다(§10.4 R-T0-1).
7. `manipulation_share` 의 설계 예산값(캠페인 35.0% · T0 36.0%)은 **저작된 예산**이지 플레이 실측이 아니다. 밴드는 `null` 이며 T0 12명 관측 후 RFC 로 정의한다(§10.4).
8. 이전 판 §10-6이 "QA C3 검토 F4(`c3-b1.recovery` 우회 약속 모순)로 §8이 무효"라고 적었으나, live `c3-b1.recovery`는 [OBSERVED] "첫 판독의 검증 사본은 자동으로 보존되므로 성찬이 원본을 회수해도 만조 피크 3개가 증거함에 남아 `c3-b2` 정합이 성립한다"로 모순이 해소돼 있다. 해당 무효 선언은 철회한다. (현행 C3-F4는 부식 모델 결함이며 §4에서 닫혔다.)

### 10.4 위험 등록부

디렉터 판정(C6-F5)은 "문서로 닫지 않는다"이다. 아래는 **닫힌 결함이 아니라 열린 위험**이며, T0 사람 검증 결과가 나오기 전에는 어떤 수치도 이 위험을 근거로 바꾸지 않는다.

#### R-T0-1 · 조작 하위과제가 표·칸 채우기에 몰려 있다 (열림 · 출처 C6-F5)

**주장 [OBSERVED · 2026-09-10 재계산]**: 33비트 중 **21비트(63.6%)** 의 `subtasks` 에 「표」 또는 「칸」이 들어 있다.

재현 명령과 그 출력(그대로 실행 가능):

```
$ node -e 'const j=require("./_workspace/current/planning/campaign.json");const b=j.stages.flatMap(s=>s.beats);const hit=b.filter(x=>(x.subtasks||[]).some(t=>/표|칸/.test(t)));console.log("total",b.length,"hit",hit.length,(hit.length/b.length*100).toFixed(1)+"%");console.log(hit.map(x=>x.id).join(" "));'
total 33 hit 21 63.6%
t0-b2 t0-b3 c1-b1 c1-b3 c1-b4 c2-b2 c2-b3 c2-b4 c3-b1 c3-b2 c3-b3 c3-b4 c4-b1 c4-b2 c4-b3 c5-b1 c6-b3 c6-b4 c7-b1 c7-b3 c7-b4
```

비해당 12비트: `t0-b1 c1-b2 c2-b1 c4-b4 c5-b2 c5-b3 c5-b4 c6-b1 c6-b2 c7-b2 e0-b1 e0-b2` [OBSERVED, 같은 명령의 여집합].

| 관측 | 값 | 출처 |
|---|---|---|
| 표·칸 포함 비트 | **21 / 33 = 63.6%** | 위 명령 [OBSERVED] |
| 클라이맥스 C7(65분) 중 표·칸 아닌 비트 | **`c7-b2` 1건** (`c7-b1`·`c7-b3`·`c7-b4` 는 전부 표·칸) | 위 명령의 hit/miss 목록 [OBSERVED] · 스테이지 분: `node -e 'const j=require("./_workspace/current/planning/campaign.json");console.log(j.stages.map(s=>s.id+":"+s.minutes).join(" "));'` → `T0:25 C1:50 C2:55 C3:65 C4:65 C5:70 C6:75 C7:65 E0:10` |
| `routing` 비트 / 분 | **3비트 · 52분** | §0.1 검증기 출력 `aggregates.toolBeatCounts` · `toolBeatMinutes` [OBSERVED] |
| `corrosion` 비트 / 분 | **3비트 · 46분** | 동상 [OBSERVED] |
| 설계 예산상 조작 분 비중 | **168 / 480 = 35.0%** | §0.1 `aggregates.activityBudgetSums` [OBSERVED — 설계 예산값이지 플레이 실측이 아니다] |
| T0 스테이지 조작 분 비중 | **9 / 25 = 36.0%** (T0 3비트 중 표·칸 2건: `t0-b2`·`t0-b3`) | `stages[T0].beats[].activityBudget` 합산 [OBSERVED] |
| 손 조작의 재미(감) | **미측정** — 사람 플레이 n=0, 빌드 n=0 | §10.3-1 |

**왜 위험인가 [INFERENCE]**: 상품 약속(도구 6종이 서로 다른 재미를 준다)은 조작 유형이 갈린다는 전제 위에 있는데, 데이터가 보여 주는 것은 대부분의 비트가 "표/칸을 채운다"는 같은 손동작으로 수렴한다는 것이다. `routing`·`corrosion` 은 각 3비트로 표본이 얇아 대비축 역할을 못 할 수 있다. 다만 「표」·「칸」은 **문자열 일치**이므로 서술 어휘가 같다는 뜻이지 손 조작이 실제로 동일하다는 증명은 아니다 — 그래서 결함이 아니라 위험이고, 판정 입력은 T0 사람 검증이다.

**T0 관측 지표** (디렉터 판정: verification-plan 에 추가 — 소유 레인 systems, 밴드 정의는 밸런스):

```yaml
risk_id: R-T0-1
status: open
trigger_defect: C6-F5
metric_manipulation_share:
  definition: 조작에 쓴 분 / 총 플레이 분   # 참가자별, T0 세션 기준
  design_reference: 0.360                  # T0 설계 예산 9/25 [OBSERVED · 플레이 실측 아님]
  campaign_reference: 0.350                # 전 캠페인 설계 예산 168/480 [OBSERVED]
  observed: null                           # 사람 플레이 n=0
  band: null                               # 밴드를 지어내지 않는다. T0 12명 관측 후 정의(RFC 대상)
  method: T0 세션 화면 기록 구간 코딩(조작 / 추론 / 탐색 / 대화), 코더 2인 교차
metric_hand_manipulation_named_fun:
  definition: "가장 재미있었던 조작 1개"에 손 조작(표·칸 채우기가 아닌 것)을 꼽은 응답 수 / 유효 응답 수
  observed: null
  band: null
  method: T0 종료 설문 자유응답 + 코딩
retune_policy: >
  이 위험만으로는 어떤 수치도 바꾸지 않는다. 재설계(비트 유형 재배분) 여부는
  T0 사람 검증 결과와 디렉터 판정 이후에만 연다.
data_mirror: null   # 관측 지표이며 런타임 데이터 테이블이 아니다
```

**밸런스 시트의 기존 조작 유형 열**: 없다 [OBSERVED — §7 지수는 단서 수·도구 수만 쓴다]. QA 가 요구한 "조작 유형 분류 열(물리 조작 / 표·분류 / 읽기)"은 **T0 관측 후에 만든다** — 지금 만들면 33비트를 문자열 어휘로 분류한 [INFERENCE] 표가 지수의 입력이 되어, 미측정 값이 난이도 곡선에 들어간다(C3-F31 에서 이미 한 번 뺀 실수의 재발). 근거: §7.2 분리 원칙.

**디렉터 판정문과의 수치 차이 [OBSERVED]**: `decision-log.md` C6-F5 판정문은 "조작 하위과제의 **61%**"로 적혀 있으나, 위 명령의 재계산은 **63.6%(21/33)** 다. QA `c6-review.md` 원문도 21/33 이다. 본 시트는 재현 명령이 있는 **63.6%** 를 쓰고, 판정문 문자열 정정을 디렉터에게 요청한다(§10.2 Q8).

## 11. 폐기 원장 (삭제하지 않고 보존) [CARRIED]

CLAUDE.md §2 "삭제는 없다". 아래는 이전 판(2026-09-09)의 정규 절에 있었으나 RFC 판정으로 규범적 지위를 잃은 값이다. 인용·재도입 금지, 역사 참조 전용.

| 폐기 항목 | 이전 판 값 | 폐기 사유 |
|---|---|---|
| 판독 재생 예산 §3.1~§3.2 | 필요 재생 29(염판 22 + 정합 재판독 7) · 판 15매 · 용량 45 · 여유율 ≥1.50 | 법2 정본이 "사본 판독 무제한"이므로 재생 횟수 예산이 성립하지 않음 (RFC-P3-009) |
| 소진 처리 §3.3 | 4회째 원판 결정 붕괴 → 8분 분해능 강등 사본 | 위와 동일. 캐논 개정 제안이었고 불필요해짐 |
| 확정 1회당 소모표 §4.1 | `circuit`→`brine_line`1+`power_bus`1 / `reader`→`reader_head`1 / `routing`→`gate_valve`2+`pump_motor`2 / `seal`→`seal_press`1 | 부식은 확정으로 소모되지 않음 (RFC-P3-009) |
| 6계통 한도 §4.2 | `brine_line`14 · `power_bus`14 · `reader_head`12 · `seal_press`9 · `gate_valve`9 · `pump_motor`9 | 계통별 차단 한도는 데이터에 없음. 표시 분해는 economy §4.1 조건부 [TARGET] |
| 장별 누적표 §4.3 | T0 2/2/1 … E0 10/10/8/6/6/6 | 누적 소모 개념 폐기 |
| 하한 돌파 모드 | `corrosion_floor_breach_mode: practice_confirm_no_cost` · `corrosion_safety_floor_ratio: 0.20` · `corrosion_rework_allowance_ratio: 0.40` | 소모가 없으므로 잔여율·하한 돌파가 정의되지 않음 |
| 힌트 자동 승격 임계 §6 | T1 180초 / T2 누적 420초 + 오확정 2 / T3 누적 900초 + 오확정 4 + 확인 클릭 | RFC-P3-015: 자동 제안은 180초 단일 모델. 단계는 플레이어가 연다 |
| 데이터 미러 경로 §2 | `unity/Unknown/Assets/Data/Balance/*.csv` 7종 | systems 소유 경로 `Assets/_Project/Data/Tables/*.json`로 통일 |
| 집계 근거 해시 | `2bfe4d52…` (염판 22 · reader 8 · seal 6) | 저장소에 존재하지 않는 해시 (C3-F2) |
| 3열 난이도 지수 §7 | `difficulty_index_formula: max_clues + max_tools + concurrent_hypotheses` · `[5,7,7,7,7,8,11,8,4]` | C3-F31: 세 번째 열이 [INFERENCE]·재현 명령 없음. 판정의 부호가 미측정 열 하나로 뒤집혔다 → 지수에서 제외(값은 §7.2에 [TARGET]으로 보존) |
| 상승 폭 위반 판정 §7.1 | `difficulty_observed_max_rise: 3` · `difficulty_rise_rule_status: FIX` · 해소안 A(planner `campaign.json` 재저작)/B(규칙 ≤3 완화) | 위와 같은 사유. 2열 지수 실측은 +2로 규칙 통과이며 A안·B안 모두 불필요해졌다 [OBSERVED 2026-09-10] |

