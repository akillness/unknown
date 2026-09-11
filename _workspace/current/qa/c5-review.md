---
updated: 2026-09-10
cycle: 20260909-preproduction-c5
status: current
supersedes: null
owner: game-qa
---

# C5 독립 검토 (R5) — 상품 · 생산 · 회귀

## 0. 이 검토의 지위와 방법

- 회차: **R5 = C5 독립 검토**. 계약 `## Cycle plan` 표의 C5 행("상품·생산·회귀 / QA 전문서 회귀 + 반박(가격/생산량/일관성)"), 닫는 질문 = **"문서 간 숫자·이름 모순이 0인가"**.
- 방법: 레인 보고를 옮기지 않고 **파일을 열고 명령으로 재측정**했다. §1의 명령 12종은 QA가 이 세션에서 직접 실행한 것이며 결과를 그대로 인용한다.
- **쓰기는 `_workspace/current/qa/` 안에서만 했다**(본 파일 · `defect-register.md` · `gate-measurements.md`). product · production · presentation · systems · planning · modeling · concept · 루트 README · `assets/` 는 **읽기만** 했다. 덱은 원본을 건드리지 않고 **스크래치패드로만 재빌드**해 대조했다.
- **이 검토는 어떤 게이트도 PASS로 올리지 않는다.** 빌드 0줄 · 사람 플레이 n=0 · 시뮬 0회 · 성능 캡처 0건. **이번 회차에 새로 측정된 런타임 값은 0건**이다.
- QA는 **구매 의사를 만들어내지 않는다.** 가격·수익·판매량은 전부 PM 소유의 목표·가정이며, 본 검토는 그 숫자가 *서로 일치하는가*와 *관측치로 위장되지 않았는가*만 본다. 어떤 가격 후보도 본 검토로 승격되지 않는다.
- 정본 우선순위(지시문): 디렉터 RFC > 세션 P 명시 결정 > live `planning/campaign.json`(검증기 출력의 sha) > `current/worldview/*` > 기타 current 문서.

---

## 1. 재측정 명령 원문 [OBSERVED 2026-09-10]

| # | 명령 | 결과 |
|---|---|---|
| M1 | `node _workspace/current/planning/validate-campaign.mjs` | `checks 47 · pass 47 · fail 0 · verdict PASS`, exit 0 |
| M2 | `shasum -a 256 _workspace/current/planning/campaign.json` · `wc -c` | `92301c0a5ecfc7e142646f5f17c29c21609e2b2de7c90408de7bffc9d815ae23` · **121457 B** — RFC-Q1이 지정한 "검증기 출력의 sha" 규칙대로 재측정. C3 종료 시점 값(`fdabf1d4…` · 120479 · 44/44)은 **더 이상 live 가 아니다** |
| M3 | 검증기 aggregates | stages 9 · stageMinutes `25/50/55/65/65/70/75/65/10` · total **480** · beats **33** · clues **73** · fast **322** · deliberate **673** · toolBeats `circuit 10 · reader 11 · alignment 8 · routing 3 · corrosion 3 · seal 7` |
| M4 | `node -e` 로 `stages[0]` 직접 판독 | `T0.id=T0 · minutes=25 · zoneIds=["hub"]` · 전 구역 집합 = `hub, gate, pump, dock, lowland` (5) |
| M5 | `node _workspace/current/presentation/generate-deck.mjs --out <scratchpad>/deck-r5.html` | exit 0 · `errors: []` · 경고 1건(외부 링크 16장) · 리포트 `beats 33 / clues 73 / campaignMinutes 480 / baseDays 287 / totalDaysWithContingency 359 / laborCostKRW 107,700,000 / defaultNetPerUnit 7350.95 / defaultBreakevenCash 1361 / defaultBreakevenLabor 14652 / uiContract{screens 19, player_decisions 7, data_bindings 10, verificationMatrixRows 17}` |
| M6 | `shasum -a 256` · `wc -c` on live HTML ↔ 재빌드본, 그리고 태그 단위 `diff` | **바이트 동일** — `67c6592abe2cf3ed569b8897ca83601771cfd14ac18fdd37a5972232e5d6c7cf` · 100,060 B, diff **0행**. 덱 HTML은 live 원본으로부터 재현 가능하다 |
| M7 | 덱 HTML 전문 텍스트 추출 후 금칙어 계수 | `gameplay` **0** · `수상\|어워드\|award\|GOTY\|베스트셀러` **0** · `게임플레이` 3건(전부 31번 "실제 Unity 캡처 전에는 어떤 영상도 게임플레이라고 부르지 않는다" 계열 금지 문장) · Unity 버전 문자열 `6000.5.6f1` 1건, `2022.3*` **0건** |
| M8 | 덱 슬라이드별 외부 URL 추출 | 링크 보유 16장 = `3,4,17,20,21,22,23,24,27~34` — **가격·정책·금액을 다루는 슬라이드는 전건 보유**. 도메인 = `partner.steamgames.com, store.steampowered.com, unity.com, gcrb.or.kr, grac.or.kr, law.go.kr, nts.go.kr` |
| M9 | `node .claude/skills/game-ops-harness/scripts/validate-preproduction.mjs` | `status FIX` · `checks 512 / passed 512` · **errors 2건** = `missing cycle artifact qa/c5-review.md`, `missing cycle artifact production/cycles/c5-development.md`. `files[]` 해시가 M2와 일치 |
| M10 | `python3` 로 `docs/media/provenance.json` 전수 대조(파일 존재 · `sha256` 재계산 · `claim` 문자열) | 항목 **15 / 디스크 15**, 누락 0 · 여분 0 · **sha 불일치 0** · `claim` 에 `NOT gameplay` 없는 항목 **0** |
| M11 | `assets/generated/2d/*/provenance.json` 6개 집계 + 파일 수 | 항목 **45**(concept 21 · previz 9 · readme 7 · ui 4 · keyart 2 · capsule 2) / 파일 **45** / `runtimeEligible:false` **45** |
| M12 | `bash .claude/skills/game-ops-harness/scripts/freshness-check.sh` | 1회차 `0 finding(s) across **92** markdown artifact(s)` exit 0 (본 파일 작성 전) → **재실행 `0 finding(s) across 95` exit 0** (본 파일 1건 + **병행 R4 회차가 쓴 2건** 포함). §1-a 참조 |

### 1-a. 측정 중 워크스페이스가 움직였다 [OBSERVED — 정직성 기록]

본 검토를 수행하는 동안 **같은 세션의 R4(C4 재검증) 회차가 다른 파일을 쓰고 있었다.** `find -newermt` 로 확인한 사실:

| 파일 | 시각 | 쓴 주체 |
|---|---|---|
| `qa/c4-review.md` (13.7K → **60.9K**) | 05:11 | **R4** — 재검증 절(R4.0~R4.6), 신규 결함 C4-F5~C4-F18, 승격 판정표 추가 |
| `animation/{anim-list,rig-requirements}.md` | 05:03~05:05 | animation 레인 |
| `qa/{c5-review,defect-register,gate-measurements}.md` | 05:09~05:12 | **본 회차(R5)** |

그래서 freshness 산출물 수가 **92 → 95** 로 늘었다. **이것은 결함이 아니라 병행 작업의 정상 결과**이며, 여기 적는 이유는 세 가지다. (1) `#g8` 의 `artifacts_scanned` 는 **측정 시각에 종속**되며 회귀 신호로 쓸 수 없다. (2) 본 검토의 **M1~M11 은 05:09 이전 값**이다 — 그 뒤 R4 가 바꾼 것은 `qa/` 와 `animation/` 뿐이고 **본 검토 대상(product · production · presentation · systems §10 · planning · README · assets)은 하나도 포함되지 않으므로 판정은 유효**하다. (3) C5-F2 의 근거인 `qa/c4-review.md` 는 **R4 확장 후 상태로 다시 읽어 인용을 갱신했다**(아래 C5-F2).

부수 명령: `git status --short`(본 세션 외 변경 없음) · `git log --oneline -3`(HEAD = `b79577b`, **신규 커밋 0건**) · `find unity/Unknown -name '*.cs'`(**0**) · `cat unity/Unknown/ProjectSettings/ProjectVersion.txt`(`6000.5.6f1`) · `ls _workspace/current/handoff/`(**파일 0개**) · `ls _workspace/current/production/cycles/`(c1~c4만).

---

## 2. 판정: **SPEC-FIX**

| 축 | 결과 |
|---|---|
| C5 닫는 질문 "문서 간 숫자·이름 모순 0인가" | **아니오.** 확인된 모순 **6건**(C5-F1 · F2 · F4 · F5 · F8 · F9) |
| 산술 검산 (가격·손익·견적) | **전건 일치.** 아래 §3에 재계산 원문 |
| 정직성 표기 (n=0 · NOT-MEASURED · 가정 라벨 · "권장" 승격 0) | **유지됨.** 위반 0건 |
| 계약 Release safety | **위반 0건.** §8 |
| 생성 리소스 provenance | **위반 0건.** M10 · M11 |
| 런타임 증거 | **0건.** 어떤 게이트도 움직이지 않는다 |

**SPEC-PASS가 아닌 이유**: open S2 4건(C5-F1·F2·F3·F6)이 전부 *대외로 나가는 문서*(브리핑 덱 · 사이클 대장 · 루트 README)에 있고, 그중 둘은 **없는 것을 있다고 적거나 폐기된 범위로 착수 승인을 요청**한다. **SPEC-REDO가 아닌 이유**: 산술·데이터 파생 축은 전건 일치했고, 문제는 전부 **손으로 굳힌 문자열 5~6곳**이다. 재작성이 아니라 지목 수정으로 닫힌다.

---

## 3. 가격 · 손익 산술 재검산 [OBSERVED — QA 독립 재계산]

`product/economics.json` 의 `unitFormula` = `price*(1-discount)/(1+vatKR)*regionalFactor*(1-refundRate-chargebackRate)*developerShareAssumption-perUnitReserve`.

### 3.1 9,000원 실결제 하한과 40% 상한 (business-model.md §3)

| 후보 | `floor((1-9000/P)*100)` | 그 할인의 결제가 | 40% 적용 시 | 문서 값 | 판정 |
|---|---:|---:|---:|---|---|
| B1 14,900 | **39%** | 9,089 | 8,940 → 하한 위반 | 39 / 9,089 / 위반 | **일치** |
| B2 17,500 | **48%** | 9,100 | 10,500 | 48 / 9,100 / 가능 | **일치** |
| B3 19,900 | **54%** | 9,154 | 11,940 | 54 / 9,154 / 가능 | **일치** |

"B1은 정책 상한(40%)보다 내부 상한(39%)이 먼저 걸린다"는 서술은 **산술적으로 옳다**. 덱 21번·22번이 같은 계산을 빌드 시점에 수행한다(`maxDiscountPct`, 시뮬레이터 옵션 `0 10 20 30 39 40`).

### 3.2 본당 수취 · 손익분기

정가 기준(할인 0%, r=0.08, c=0, s=0.70, v=500, k∈{1.00, 0.85}) 재계산 → `8,223 / 6,915`(B1) · `9,745 / 8,209`(B2) · `11,151 / 9,403`(B3) — **business-model.md §5 표와 전건 일치**. 손익분기 12칸(`ceil(현금/Net)`)도 12/12 일치(예: 1,000만÷8,223.27 = 1,216.0 → **1,217**).

할인 10% 기준(덱·검증기) 재계산 → B1 `13,410 → 12,190.909 → 11,215.636 → 7,850.945 → **7,350.945**`, 현금 3,000만 회수 `ceil(30,000,000/7,350.945)` = **4,082**, 인건비 1억770만 회수 = **14,652** — 덱 23번·`steam-game-plan.meta.md` 계산 검산 절·M9 검증기와 **3중 일치**.

> **그래서 발견된 것이 C5-F8이다.** 두 표는 각각 옳지만 **기준 할인율이 다르고**, `business-model.md` §5 표 머리에 "할인 0%(정가 기준)"이 없다. 같은 이름(`B2 본당 수취`)이 문서에 따라 **9,745원**과 **8,721원**으로 존재한다.

### 3.3 8시간 → 가격 논리의 부재 [OBSERVED 전수]

| 위치 | 문장 | 판정 |
|---|---|---|
| `business-model.md` §1 | "8시간이라는 분량은 가치 서술이지 **가격 근거가 아니다**. 공식 문서 어디에도 플레이타임-가격 기준이 없다" | 명시적 차단 |
| `business-model.md` §7 | DLC 가격을 "본편 시급 환산으로 정당화하지 않는다" | 차단 |
| `business-model.md` §10 | **단정 금지** 목록에 "8시간에서 적정가 도출" | 차단 |
| `planning/gdd.md` L265·L266 | 가격 숫자 삭제 + "8시간 분량을 가격 근거로 쓰지 않는다" | 차단 |
| `planning/market-decision.md` Duck Detective 행 | 복제 금지 = "8시간이면 무조건 더 비싸다는 추론" | 차단 |
| `product/skill-application.md` | "8시간당 가격/TAM 스케일/경쟁작 리뷰→판매량으로 강행하지 않는다" | 차단 |
| 덱 21·22·23번 | 분량을 가격 근거로 연결하는 문장 **0건**(M7 텍스트 전수) | 차단 |

**8시간→가격 논리는 어느 문서에도 없다 — 부재가 의도적으로 명문화돼 있다.** 이것은 C1-F2의 정확한 반대편이며, 본 검토가 확인한 가장 견고한 축이다.

### 3.4 "권장" 승격 0건 [OBSERVED]

`grep -rn "권장" product/ deck-outline.md` + 덱 HTML 전수 = 가격 후보를 권장으로 올린 문장 **0건**. 등장하는 "권장"은 전부 (a) Steam 공식 권장 할인폭 10~15%, (b) 출시일 DLC 비권장, (c) 트레일러 HUD 권장, (d) **"지불 의사 응답이 모여야 가격 후보 중 하나를 권장으로 승격할 수 있다"**(덱 35번, 승격 조건 서술)뿐이다. **가격 3안은 여전히 3안이다.**

### 3.5 정가 / 현재가 구분 [OBSERVED]

`market-decision.md` 비교작 8종은 전부 **"현재가"** 로 라벨돼 있고, 유일하게 정가·할인율·종료일이 모두 관측된 건(`Desktop Explorer` 정가 19,300 / 20% / 15,440 / 9월12일 종료)만 따로 적혀 있다. 덱 4번도 같은 문장("현재가이고 정가표가 아니다")과 같은 단서(노트에 Desktop Explorer 사례)를 싣는다. `business-model.md` §2가 "정가/상시 할인폭 표는 여전히 미측정"이라고 적어 **미측정이 미측정으로 남아 있다**. 위반 없음.

---

## 4. 신규 결함 — C5-F1 ~ C5-F11

### C5-F1 [S2 · presentation] 덱이 폐기된 T0 범위로 착수 승인을 요청한다 (같은 덱 안에서 자기모순)

- **관측**: 덱 **11번** = "T0 … 25분, **허브에서만** 진행한다. 도구는 **circuit, reader** 2종만 열어…"(생성기 `firstStage.tools` 파생, 데이터에서 계산). 덱 **26번** = "포함은 **허브와 제3수문**, 도구 **reader 와 alignment 와 seal**, 결론 1건, 힌트 3단계. **제외는 나머지 3구역**…"(`generate-deck.mjs:1033` **하드코딩 문자열**, 구역 수는 `SLICE_ZONE_COUNT = 2`(L173)에서 파생).
- **정본**: `systems/unity-implementation.md` §10 = "**T0 범위(확정)**: 공간 **`hub` 1개**, 도구 **`circuit` + `reader` 2개**, 목표 길이 **25분**. alignment·routing·corrosion·seal은 **T0에 없다**." live 데이터도 같다 — M4: `T0.zoneIds = ["hub"]`. 전 구역 5개이므로 제외는 **4구역**이지 3구역이 아니다.
- **더 나쁜 점 둘**. (1) `generate-deck.mjs:172` 주석이 `SLICE_ZONE_COUNT = 2` 의 근거로 **`systems/unity-implementation.md 10절`을 인용**한다 — 그 절이 반대를 적는다. (2) 26번은 *"슬라이스 우선"* 슬라이드, 즉 **T0 착수 승인을 요청하는 화면**이다. 이 상태로 승인되면 승인된 범위와 계약된 범위가 다르다.
- **부가 위반**: `deck-outline.md` L13 "덱의 모든 수치는 `generate-deck.mjs`가 빌드 시점에 아래 원본에서 다시 읽어 계산한다. **슬라이드에 숫자를 손으로 적어 넣지 않는다**" — 이 주장이 이 두 곳(그리고 C5-F5)에서 거짓이다.
- **왜 QA가 이것을 최우선으로 두는가**: C4-review F4가 지적한 T0 3자 불일치는 **systems가 T0를 축소하는 방향으로 해소**했고(`tech-verification/c4-self-check.md` F4 표: "불필요해짐 — 3자 일치"), `modeling/asset-budget.md:24`(`T0:셸1·도구2·초상1`)와 `production-estimate.json` T0(`newAssets 4 · artDays 12.6`)도 **축소된 범위와 이미 일치**한다. 즉 **덱만 옛 범위에 남아 있다.**
- **재현**:
  ```
  node _workspace/current/presentation/generate-deck.mjs --out <scratchpad>/deck.html
  grep -n "제3수문\|SLICE_ZONE_COUNT" _workspace/current/presentation/generate-deck.mjs   # 172, 173, 1033
  sed -n '114,116p' _workspace/current/systems/unity-implementation.md
  node -e 'const c=require("fs").readFileSync("_workspace/current/planning/campaign.json","utf8");const s=JSON.parse(c).stages[0];console.log(s.id,s.zoneIds)'
  ```
- **요구 수정(presentation 소유)**: 26번 본문의 포함 범위를 **`firstStage.zoneIds` · T0 도구 목록에서 파생**시키고, `SLICE_ZONE_COUNT` 상수를 삭제해 `allZones.length - firstStage.zoneIds.length` 로 계산한다. 그 뒤 재빌드하고 `steam-game-plan.meta.md` 산출물 표의 sha·바이트를 갱신한다. 하드코딩 재유입은 빌드 자체 점검에 문자열 게이트로 막는다(생성기 소스 검사 방식은 이미 r2에 있다).

### C5-F2 [S2 · director / production] `cycle-ledger.json` 이 실제 회차 상태보다 뒤처져 있고, 그 값이 덱 19번으로 전파된다

- **관측 (파일)**: C3 `independent-review-running` · `findings: null` · `fixed: null` / C4 `draft-preparation` · null · null / C5 `draft-preparation` · null · null.
- **실제 [OBSERVED 2026-09-10 05:12, R4 확장 반영 후 재확인]**: C3는 **QA 재검증 3회를 마치고 종료 판정 묶음까지 나온 상태**다 — `qa/c3-review.md` §9.6 = 총 36건 / closed **27** / open **4** / open-rfc **5** / 열린 S1 **0**, 판정 **SPEC-FIX**. C4는 **독립 검토 + 재검증까지 끝났다** — `qa/c4-review.md` 1차 판정 **FIX**(material 4 · blocker 2)이고, **R4 재검증에서 F1~F4 가 전건 `closed`**, 신규 **C4-F5~C4-F18 14건**(S2 8 · S3 6)과 **승격 판정표**(승격 가능 4 · 차단 8)까지 나왔다. systems 는 `tech-verification/c4-self-check.md` 로 대응을 적었다. C5는 **본 문서로 검토가 진행됐다**.
- **즉 대장은 한 회차가 아니라 두 회차만큼 뒤처져 있다.** "C4 = `draft-preparation` / 발견 미기록 / 수정 미기록"이라는 표기는 **실제로 18건이 판정된 회차**를 가리킨다.
- **전파**: 덱 19번은 이 표를 그대로 렌더한다(M5 리포트 `cycleLedger: "present"`). 현재 덱 화면 문자열 = "C4 상호작용·Unity **draft-preparation 미기록 미기록**". 브리핑을 보는 사람은 C4가 아직 검토되지 않았다고 읽는다 — **사실보다 나쁘게 틀렸다**.
- **`cycle-ledger.meta.md` 도 같이 틀린다**: "실제연속검토와수정상태" 라고 선언하지만 실제와 다르다.
- **재현**:
  ```
  python3 -c "import json;print(json.load(open('_workspace/current/production/cycle-ledger.json'))['cycles'])"
  grep -n "판정" _workspace/current/qa/c4-review.md | head -3
  sed -n '/## 9.6/,/최종 판정/p' _workspace/current/qa/c3-review.md
  ```
- **요구 수정(director 소유)**: C3 → `reviewed-and-revised`(또는 `spec-fix-open`), `findings 36 / fixed 27 / remaining "open S2 3 · S3 1 · open-rfc 5 · 런타임 n=0"`. C4 → `independently-reviewed`, `findings 4 / fixed <실측>`. C5 → `independent-review-running`, `findings 11`(본 문서). 값을 채운 뒤 덱을 **재빌드**해야 19번이 따라간다. QA는 대장을 쓰지 않는다(디렉터 소유).

### C5-F3 [S2 · director / production] `production/cycles/c5-development.md` 부재 — 회차 기록 의무 미충족

- **관측**: `production/cycles/` = `c1,c2,c3,c4-development.md` 4건. c5 없음. 하네스 검증기가 이를 error로 잡는다(M9: `missing cycle artifact production/cycles/c5-development.md`).
- **계약 근거**: CLAUDE.md §10 "`preproduction` 사이클: 최소 5회의 조사→개발→독립 검토→플레이타임 논의. **회차마다 이전 문서·변경·근거·미측정 항목을 남긴다**."
- **왜 S2인가**: C5 산출물(product 4종 + economics.json + 견적 + 대장 + 덱)은 이미 존재하는데 **그것들을 만든 회차의 개발 기록만 없다**. 다음 세션은 "이 숫자를 왜 골랐는가"를 되짚을 문서가 없다. 본 검토(`qa/c5-review.md`)는 그 짝의 한쪽일 뿐이다.
- **재현**: `node .claude/skills/game-ops-harness/scripts/validate-preproduction.mjs | head -12` · `ls _workspace/current/production/cycles/`
- **본 파일 작성 후 재실행 [OBSERVED 2026-09-10]**: `errors` 가 **2건 → 1건**으로 줄었다(`missing cycle artifact production/cycles/c5-development.md` 만 남음). `checks 512 / passed 512` 는 불변, `status` 는 여전히 **FIX**. **QA 산출물로 닫을 수 있는 절반은 닫혔고, 남은 절반은 디렉터 소유다.**
- **요구 수정(director 소유)**: `c5-development.md` 작성(입력 문서 · 이번 회차의 변경 · 근거 · 미측정 항목 · 플레이타임 논의). 작성 후 M9 errors 는 0이 된다(다른 한 건은 본 파일로 해소됨).

### C5-F4 [S3 · presentation] `steam-game-plan.meta.md` r3 측정표가 죽은 정본 입력을 고정 기재

- **관측**: 메타 L54~L55 = `campaign.json sha256 fdabf1d4… · 120,479 bytes` / `checks 44 / pass 44`. **live는 `92301c0a…` · 121,457 B · 47/47**(M1·M2). L62 의 "계보 주의" 절도 같은 죽은 값을 현재값으로 서술한다.
- **RFC-Q1 위반**: "이후 인용은 `planning/validate-campaign.mjs` 출력의 sha 를 쓴다(**고정 숫자 재기재 금지**)."
- **완화 사실 [OBSERVED]**: HTML 자체는 스테일하지 **않다** — M6에서 live 원본으로 재빌드한 결과가 **바이트 동일**했다(sha `67c6592a…`, diff 0행). 즉 campaign.json 이 개정됐지만 덱이 읽는 집계값(33 · 73 · 480 · 322 · 673 · 도구별 비트)이 바뀌지 않아 출력이 같았다. **스테일한 것은 산출물이 아니라 영수증 절이다.**
- **재현**:
  ```
  node _workspace/current/planning/validate-campaign.mjs | head -12
  sed -n '52,62p' _workspace/current/presentation/steam-game-plan.meta.md
  shasum -a 256 _workspace/current/presentation/steam-game-plan.html    # 67c6592a… (메타와 일치)
  ```
- **요구 수정**: r3 표의 campaign 행을 **재측정값으로 교체**하거나, 값을 적는 대신 "재측정 명령과 그 출력"만 인용하는 형태로 바꾼다(RFC-Q1 취지). 산출물 행(100,060 B · `67c6592a…`)은 **현재도 정확하므로 그대로 둔다**.

### C5-F5 [S3 · presentation] 덱 17번의 인수 테스트 수와 폐기 용어

- **관측 A**: `generate-deck.mjs:824` = "기술 인수 테스트 **14개**를 상태 술어로 정의했고 실행은 0건이다." 실측: `systems/unity-implementation.md` §11 의 고유 `T-nn` = **27개**(T-01~T-27). 14는 R2·R3·R1 확장 이전 값이다.
- **관측 B**: `generate-deck.mjs:829` = "겹치지 않는 **매체 경로** 2개…". systems가 이 용어를 **명시적으로 폐기**했다 — `unity-implementation.md:69` "(C4의 모호한 '매체 경로'라는 세 번째 용어는 폐기하고 이 두 필드로만 말한다.)", `tech-verification/c4-self-check.md` F3 표 2행 = "**있음(명시적 폐기)**". 정본 표현은 `sourceType 상이 AND originId 상이`.
- **재현**:
  ```
  grep -oE "T-[0-9]{2}" _workspace/current/systems/unity-implementation.md | sort -u | wc -l   # 27
  grep -n "인수 테스트 14\|매체 경로" _workspace/current/presentation/generate-deck.mjs        # 824, 829
  sed -n '69p' _workspace/current/systems/unity-implementation.md
  ```
- **요구 수정**: 테스트 수를 `unity-implementation.md` §11 표에서 **빌드 시점에 세어** 렌더하고(손으로 적지 않는다 — `deck-outline.md` L13 자기 규약), "매체 경로"를 정본 표현으로 교체한 뒤 폐기 문구 게이트에 `매체 경로` 를 추가한다. `systems/system-specs/{plate-readout,drainage-routing}.md` · `data-schemas/beats.md` 의 잔존은 **systems 레인 소유**이므로 브로드캐스트만 한다(§6).

### C5-F6 [S2 · director / systems] 루트 `README.md` 가 존재하지 않는 핸드오프 산출물을 있는 것처럼 서술

- **관측**: README L70 = "`_workspace/current/handoff/`  Codex(GPT-6 Astra) Unity 구현 핸드오프 브리프 · 검증 계획 · 리소스 런북", L80 = "구현은 [`_workspace/current/handoff/`] 의 **브리프 순서를 따릅니다**: 아키텍처 계약 → 수직 슬라이스 T0(허브 + `circuit`/`reader`, 25분) → 인수 테스트."
- **실측**: `ls -la _workspace/current/handoff/` → **파일 0개**(빈 디렉터리). 계약상 핸드오프는 **C7 회차** 산출물이며 아직 시작되지 않았다.
- **왜 S2인가**: README는 이 저장소의 **유일한 대외 문서**이고, 계약 Honesty gates 는 "미제작을 미제작으로 적는다"를 요구한다. 없는 폴더 내용을 목차에 적는 것은 §2 위반은 아니지만 **정직성 축의 위반**이며, 실행자(Codex)가 README를 먼저 읽으면 존재하지 않는 브리프를 찾게 된다.
- **정확성 인정**: README L80의 **T0 서술 자체는 정본과 일치한다**("허브 + circuit/reader, 25분") — 덱 26번(C5-F1)보다 README가 더 정확하다.
- **재현**: `ls -la _workspace/current/handoff/` · `sed -n '64,81p' README.md`
- **요구 수정**: `handoff/` 행과 L80을 **"C7 예정 · 현재 비어 있음"** 으로 표기하거나, C7에서 브리프를 만든 뒤 문장을 유지한다. 둘 중 하나이며 지금 상태는 어느 쪽도 아니다.

### C5-F7 [S3 · product] `economics.meta.md` 의 재생성 경로가 존재하지 않는다

- **관측**: "계산은 **`scripts/validate-preproduction.mjs`** 에서 재생성한다." → `ls scripts/validate-preproduction.mjs` = **No such file**. `scripts/` 에는 `gen-2d.sh · gen-video-higgsfield.sh · make-previz-gif.sh · refresh-2d-provenance.py` 만 있다. 실제 검증기는 **`.claude/skills/game-ops-harness/scripts/validate-preproduction.mjs`**(M9에서 실행, exit 0).
- **왜 결함인가**: "계산 원본"을 자처하는 문서의 **재현 명령이 그대로 실행되지 않는다**. CLAUDE.md §6·본 레인 원칙 "명령 없는 수치는 측정이 아니다"의 문서판이다.
- **재현**: `ls scripts/` · `node .claude/skills/game-ops-harness/scripts/validate-preproduction.mjs | head -8`
- **요구 수정**: 경로를 실제 경로로 정정한다(한 줄).

### C5-F8 [S3 · product] `business-model.md` §5 표에 할인 조건이 없어 덱과 같은 이름의 다른 값이 병존

- **관측**: §5 "본당 수취액과 손익분기" 가정 줄 = `r=0.08, c=0, s=0.70, v=500, w x u_US=0` — **할인율이 없다**. 실제 표는 할인 0%(정가) 기준이다(§3.2 재계산으로 확인). 덱 23번·시뮬레이터·M9 검증기는 **할인 10% 기준**이다.
- **충돌하는 값**: `B2 본당 수취(k=1)` = **9,745원**(문서) ↔ **8,721원**(덱). `B1 현금 3,000만 회수` = **3,649본**(문서) ↔ **4,082본**(검증기/할인 기준). 어느 쪽도 계산 오류가 아니다 — **라벨이 없을 뿐이다.**
- 문서 말미 "C5 계산 검토" 절이 "정가 기준 표와 할인 기준을 섞지 않는다"고 적지만, **정작 §5 표 자신이 어느 쪽인지 말하지 않는다.**
- **재현**: §3.2의 산술 두 줄 · `node .claude/skills/game-ops-harness/scripts/validate-preproduction.mjs | sed -n '/economy/,/production/p'`
- **요구 수정**: §5 표 머리에 **"할인 0% · 정가 기준"** 을 명시하고, 덱 23번이 쓰는 할인 10% 표를 같은 절에 병기하거나 `qa`/덱 경로를 한 줄로 상호 참조한다.

### C5-F9 [S3 · product] B1의 근거 "GDD 상품가설 승계"가 live GDD에 없다

- **관측**: `business-model.md` §2 B1 성격 = "**GDD 상품가설 승계**, 비교작 중하단". live `planning/gdd.md` 에 `14,900`/`14900` **0건** — 같은 문서 L24·L265가 "**가격 숫자 삭제, PM 문서 인용 한 줄로 대체**", "GDD는 가격 숫자를 재기재하지 않는다"고 적는다.
- **숫자의 실제 출처**: `_workspace/archive/20260909-preproduction-c{2,3}/planning/gdd.md` **L29** = "출시 기준가 후보 14,900원 … 기본 출시 할인 후보 10%면 13,410원. DLC는 … 2~3시간/5,900~7,900원 가설". 즉 **계보는 실재하나 인용이 끊겨 있다**.
- **재현**: `grep -rn "14,900\|14900" _workspace/current/planning/gdd.md`(0건) · `grep -rn "14,900" _workspace/archive/*/planning/gdd.md`
- **요구 수정**: §2 B1 행의 근거를 `_workspace/archive/20260909-preproduction-c3/planning/gdd.md` L29 로 명시한다(CLAUDE.md §2 "아카이브는 인용하되 편집하지 않는다"의 정상 사용).

### C5-F10 [S3 · production / modeling 인용] "신작 자산 47종"의 진척이 견적 문서에서 0으로 읽히지 않는다

- **관측 (수량은 정확하다)**: `production-estimate.json` `newAssets` 합 = **47**(SHARED 31 + T0 4 + C1~C3 각 3 + C4·C5·C6 각 1), `production-estimate.meta.md` "신작자산47종은 **셸5/도구6/초상5/공용30/UI1**" — `modeling/asset-budget.md` L15~L19·L24 와 **전건 일치**. 아트 인일도 일치: 셸 25/5=5 · 도구 18/6=3 · 초상 8/5=1.6 로 환산하면 T0 `5+6+1.6=12.6` · C1~C3 각 `9.6` · C4 `1.6` · C5 `3` · C6 `5` = 51, SHARED 12 를 더해 **63** = `asset-budget` 의 `25+18+9+8+3`. 총 인일도 재계산 일치: 단계 합 **267** + 조정 12 + 출시행정 8 = **287**, ×1.25 = **359**(358.75 올림), ×300,000 = **107,700,000**, `359/20 = 17.95 → 17.9개월`, `17.95/(2×0.7) = 12.82 → 12.8개월`.
- **문제**: 이 문서 어디에도 **47종 중 생산 완료가 0종**이라는 사실이 없다. 실측은 `modeling/asset-manifest.md` §6에 있다 — `greybox 7 / pending 40`, **47행 전부 `runtimeEligible:false`**, 실재 3D 메시 12개 · 합 **144 tris**. 견적 메타는 이 매니페스트를 인용하지 않는다.
- **오독 경로 [INFERENCE]**: README 현재 상태 표는 "2D 컨셉 리소스 **45장**", "3D 허브 그레이박스 + 도구 6종 블록아웃"을 나란히 적는다. 두 문서를 이어 읽으면 "47종 중 45종 완료"로 읽힐 여지가 있다. **실제 관계는 그렇지 않다** — 생성된 45장은 전부 **컨셉/프리비즈**이며(`concept/generation-manifest.md`: "전량 컨셉/프리비즈다. 게임플레이가 아니다"), 47종 계약의 초상 5·UI 1은 **아직 `pending`** 이다(`asset-manifest` §3·§5).
- **정직성 축은 이미 지켜지고 있다 [OBSERVED]**: M10·M11 — provenance 15/15·45/45 전건 sha 일치, `NOT gameplay` 누락 0, `runtimeEligible:false` 100%. README 본문도 "전부 컨셉·프리비주얼라이제이션이며 실제 게임플레이 캡처가 아닙니다"를 최상단에 둔다. **부족한 것은 진척률을 잇는 한 문장뿐이다.**
- **재현**:
  ```
  python3 -c "import json;d=json.load(open('_workspace/current/production/production-estimate.json'));print(sum(r['newAssets'] for r in d['rows']), sum(r['artDays'] for r in d['rows']))"   # 47 63.0
  sed -n '118,129p' _workspace/current/modeling/asset-manifest.md
  find assets/generated/2d -type f ! -name '*.json' | wc -l    # 45
  ```
- **요구 수정**: `production-estimate.meta.md` 에 한 줄 — "47종의 현재 상태는 `modeling/asset-manifest.md` §6 이 소유한다: **greybox 7 / pending 40 / 생산 완료 0**. 생성된 2D 45장·3D 그레이박스·영상 2클립은 **컨셉·프리비즈이며 이 47종의 진척이 아니다**."

### C5-F11 [S4 · product] `skill-application.md` 의 "포커스 검수" 주장에 측정 기록이 없다

- **관측**: "measured-ui-callouts: 실제 최종화면 픽셀/DOM경계로 **넘침과포커스** 검수". `steam-game-plan.meta.md` 「기하 측정」 절은 **넘침만** 측정한다(4개 뷰포트 × 36장 → 넘침 0장). 포커스 순서·초점 트랩 측정 기록은 어느 산출물에도 없다. 같은 메타의 「조작 계약」 절은 `inert` + `aria-hidden` 을 **선언**하지만 측정하지 않았다고 적지도 않았다.
- **판정**: 관찰(S4). 넘침 축은 실제로 측정됐고, 포커스는 *하지 않은 것을 했다고 적은* 쪽에 가깝지만 문장이 짧아 단정으로 읽히지 않는다. **다음 회차에 포커스 순회를 실제로 측정하거나 문구에서 빼면 닫힌다.**
- **재현**: `grep -n "포커스" _workspace/current/presentation/steam-game-plan.meta.md`(선언 1건, 측정 0건) · `sed -n '/## 기하 측정/,/조작 계약/p' _workspace/current/presentation/steam-game-plan.meta.md`

---

## 5. 결함으로 올리지 않은 것 (판단 근거 기록)

| 항목 | 왜 결함이 아닌가 |
|---|---|
| **덱 HTML 이 `status: draft` 문서를 인용** | C3-F33(open-rfc)이 이미 다루는 축이며 디렉터가 R5 검증 후 승격을 명시했다. 중복 계수하지 않는다 |
| **가격·수익 수치 전량** | `[TARGET]`·`[가정]`·`n=0` 라벨이 §0·§4·§5·§9와 `economics.json` `status: "scenario-not-forecast"`·`humanPriceStudyN: 0`·`shareVerified: false` 로 **기계 판독 가능하게** 붙어 있다. 라벨된 가정은 결함이 아니다 |
| **`s = 0.70`** | "가정이며 Steam 공개 사실이 아니다"가 문서·덱·시뮬레이터 3곳에 있고, 배분율을 라벨 없이 적으면 **빌드가 실패한다**(메타 자체 점검 절). 모범 사례로 기록 |
| **`market-decision.md` 가 `cycle: c2 · status: draft`** | C5 문서들이 이를 **관측 시점 스냅샷**으로 인용하고 날짜를 함께 적는다. 시장 관측은 재측정 대상이지 개정 대상이 아니다 |
| **Steam 정책 인용 전건** | 날짜(2026-09-09 접속) · URL · "미확인" 표기가 §8·§9·§10에 일관. **U1~U6 6건이 모두 "확인 창구"까지 적혀 있다.** 자체등급분류·2FA·조세조약·최저기준가·배분율을 어느 쪽으로도 단정하지 않는다 — 계약 "단정 금지"의 정확한 이행 |
| **Higgsfield 25 크레딧 소모** | 계약 `## Asset pipeline` 이 사전 `account status` 확인 후 사용을 허용하고, `decision-log.md` 에 **영수증(153.88 → 128.88)** 이 남아 있다. Release safety 위반이 아니다 |
| **`pm-skills` · `solo-skills` 커밋 핀** | 외부 저장소 해시라 오프라인에서 검증 불가. `[OBSERVED]` 라벨은 "그 시점 조회"라는 뜻으로 읽는다. **본 검토는 이 핀을 검증하지 않았다**(§7에 기록) |
| **README L54 "`qa/c{1..5}-review.md`"** | 측정 시점에는 `c5-review.md` 가 없었으나 **본 파일 작성으로 참이 된다.** 결함 대신 관찰로 기록한다 |
| **덱 DLC 슬라이드의 인질 금지 목록이 5항목** | `business-model.md` §7은 9항목. 덱은 본문 6줄 제약이 있고 **모순이 아니라 축약**이며, 노트가 원문 경로를 가리킨다 |

---

## 6. 브로드캐스트 (dependency-matrix ● 항목)

`feedback-requested-by: 2026-09-11`

| 받는 레인 | 결함 | 요청 |
|---|---|---|
| **game-presentation-director** | **C5-F1**, C5-F4, C5-F5 | ① 26번 T0 범위를 데이터 파생으로 바꾸고 `SLICE_ZONE_COUNT` 삭제 ② 17번 테스트 수를 §11에서 세어 렌더 + "매체 경로" 정본 표현 교체 ③ 재빌드 후 메타 r3 표의 campaign 행을 재측정값으로 교체(산출물 행은 유지) ④ 폐기 문구 게이트에 `매체 경로` 추가 |
| **game-production-director** | **C5-F2**, **C5-F3**, C5-F6 | ① `cycle-ledger.json` 을 실제 상태로 갱신 후 덱 재빌드 ② `production/cycles/c5-development.md` 작성(M9 error 해소) ③ 루트 README `handoff/` 서술 정정 — README 소유 레인이 계약에 없다. **소유자 지정도 판정 요청 대상**(RFC-Q3) |
| **game-product-manager** | C5-F7, C5-F8, C5-F9, C5-F11 | ① `economics.meta.md` 재생성 경로 정정 ② §5 표에 "할인 0% · 정가 기준" 명시 ③ §2 B1 근거를 아카이브 경로로 ④ "포커스 검수" 문구를 측정하거나 뺀다 |
| **game-systems-designer** | C5-F5(잔존), C5-F6(handoff 소유) | `system-specs/{plate-readout,drainage-routing}.md` · `data-schemas/beats.md` 의 "매체 경로" 3곳은 **systems 소유**다. 폐기 선언(§5 L69)과 실제 사용이 어긋난다 — 교체 여부를 결정하고 `c4-self-check.md` F3 2행에 반영할 것 |
| **game-modeler** | (참조) | C4-review **F4는 T0 축소로 해소**됐다 — `asset-budget.md:24` 수정 **불필요**. 다만 C5-F10 이 요구하는 진척 문장은 `asset-manifest.md` §6을 **인용**하는 형태이므로 modeling 문서 수정은 없다 |
| **game-planner** | (참조) | live `campaign.json` 이 `92301c0a…` · 121,457 B · **47검사 47통과**로 갱신됐다. 이 값을 인용하는 문서는 고정 숫자를 다시 적지 말고 검증기 출력을 인용할 것(RFC-Q1) |

**RFC-Q3 (qa → director) [신규]**: **루트 `README.md` 의 소유 레인이 계약에 정의돼 있지 않다.** CLAUDE.md §1 표는 `_workspace/current/` 폴더만 배정하고, 루트 README는 2차 요청("루트 README에 게임 소개와 컷씬 GIF·이미지를 등록")으로 생겼다. 지금 README는 **13개 레인의 수치를 한곳에 모으는 문서**이면서 소유자가 없어, C5-F6 같은 스테일이 어느 레인의 회귀 대상도 아니다. 판정 요청: README를 **디렉터 소유**로 두고 사이클 종료 체크리스트(§7)에 "README 주장 재측정" 항목을 넣을 것인가. QA는 README를 쓰지 않으므로 스스로 정할 수 없다.

---

## 7. 이 검토가 증명하지 않는 것 [OBSERVED]

- **어떤 사람도 이 게임을 플레이하지 않았다.** 가격 수용·구매 의사·완주 시간·DLC 전환은 전부 **n = 0**이다. §3의 산술 일치는 **계산이 서로 맞는다**는 뜻이지 그 가격에 팔린다는 뜻이 **아니다**. QA는 지불 의사를 만들어내지 않는다.
- **생산 견적 359인일은 검증되지 않았다.** 단계별 인일은 저작 입력이고, 본 검토가 확인한 것은 **합계 산술과 자산 수량이 `asset-budget`·`asset-manifest` 와 일치하는가**뿐이다. 1인일 6시간·1인일 30만원·병렬효율 70%는 전부 가정이며 실측 근거가 없다.
- **덱의 시각 품질은 판정하지 않았다.** M5~M8은 텍스트·수치·링크·바이트 축이다. 가독성·대비·인쇄·발표 흐름은 사람이 봐야 한다.
- **외부 사실을 재확인하지 않았다.** Steam 정책 문서 원문, 비교작 현재가, `pm-skills`/`solo-skills` 커밋 핀, Higgsfield 크레딧 잔액은 **본 회차에서 네트워크로 재조회하지 않았다** — 문서가 적은 날짜·URL·미확인 표기의 **내부 일관성만** 검사했다.
- **`unity/Unknown/` 은 스켈레톤이다.** `Assets/` 파일 0개, `.cs` **0개**, 에디터 `6000.5.6f1` 확인. 프로젝트가 존재한다는 것과 게임이 존재한다는 것은 다르다.
- **C4 레인 문서의 판정은 본 회차 소유가 아니다.** C4-F1~F4의 해소 여부는 **R4(C4 재검증)** 소유이며, 본 검토는 `unity-implementation.md` **§10(T0)** 만 C5 대상으로 열었다. 그 밖의 절은 `[CARRIED]`.
  **R4 판정과의 관계 [OBSERVED]**: R4 는 `systems/unity-implementation.md` 를 **승격 차단**으로 판정했다(C4-F6 — 세이브 v1 필드가 `data-schemas/save.md`(current)와 다르다, CLAUDE.md §9 불변식 구역, RFC-S3 선행 · C4-F14 SC-3). 본 검토의 §10 검증은 **그 차단을 뒤집지 않는다** — QA 두 회차가 같은 파일의 서로 다른 절을 봤을 뿐이며, **더 보수적인 판정이 이긴다**. 본 회차의 승격 반환값도 이에 맞춘다.
  R4 의 **C4-F8**("`tech-verification/README.md`(current)가 T0 정본과 어긋난다")은 본 회차 **C5-F1**(덱이 폐기된 T0 범위를 렌더)과 **같은 뿌리**다 — §10 이 T0 를 축소 확정한 뒤 그 사실이 **아직 전파되지 않은 문서가 최소 2곳**이라는 뜻이다. 두 결함을 함께 닫는 편이 싸다.
- **`qa/exploit-register.md` · `regression-matrix.md` · `playtest-report.md` · `immersion-scores.md` 는 여전히 미작성이다.** 빌드와 표본이 없어 작성하면 측정 위장이 된다(C3 1차 §4 판정 유지).

---

## 8. 계약 Release safety 점검 [OBSERVED]

| 금지 행위 | 실측 | 판정 |
|---|---|---|
| Steam 계정 생성·연동, NDA/배포계약 서명 | 문서 전건이 "등록 행위 0건"으로 선언, 실행 흔적 0 | **위반 0** |
| 신원·은행·세무 제출 / $100 결제 | 동일. `steam-registration-guide.md` §0이 체크박스를 **전부 미체크**로 유지 | **위반 0** |
| 스토어 공개·빌드 업로드·리뷰 제출 | 0건. 덱 27~34번은 **순서 서술**이며 실행 기록이 아니라고 각 슬라이드가 명시 | **위반 0** |
| git commit / push | `git log --oneline -3` → HEAD `b79577b`(하네스 커밋), **신규 커밋 0건**. 작업 트리는 전부 미커밋 | **위반 0** |
| 유료 도구·외주 | Higgsfield 25 크레딧 소모 1건 — 계약 `## Asset pipeline` 이 허용하고 `decision-log.md` 에 잔액 영수증(153.88 → 128.88) 존재. 미실행 항목(`multi_image_to_3d`)은 승인 대기로 기록 | **위반 0 (영수증 확인)** |
| 가격 확정 | 3안 전부 미승인, "권장" 승격 **0건**(§3.4) | **위반 0** |

---

## 9. 집계

| severity | 신규 | open | 비고 |
|---|---:|---:|---|
| S1 | **0** | 0 | 이번 회차에서 발견된 차단 결함 없음 |
| S2 | 4 | 4 | C5-F1(덱 T0) · C5-F2(대장) · C5-F3(c5-development 부재) · C5-F6(README handoff) |
| S3 | 6 | 6 | C5-F4 · F5 · F7 · F8 · F9 · F10 |
| S4 | 1 | 1 | C5-F11 |
| **합계** | **11** | **11** | closed 0 (전건 신규) |

C3 이월분은 본 회차에서 **판정을 옮기지 않았다** — `qa/c3-review.md` §9.6 값(총 36 / closed 27 / open 4 / open-rfc 5)을 `[CARRIED]` 로 유지한다.

**게이트 영향**: G1~G8 **0 / 8 PASS 유지**. 본 회차가 바꾼 것은 `#g3`(상품 경제와 게임 내 경제의 경계 명문화) · `#g7`(정본 입력 재측정 · C5 문서 축 추가) · `#g8`(신선도 92 artifacts · 회차 산출물 부재 1건)의 **근거뿐**이며 어떤 값도 PASS 방향으로 올리지 않았다.

## 10. 다음 소유자

`game-presentation-director`(C5-F1 최우선 — 잘못된 범위로 승인 요청이 나가는 유일한 슬라이드) → `game-production-director`(C5-F2 · F3 · F6 · RFC-Q3) → `game-product-manager`(C5-F7~F9 · F11) → 정정 후 `game-qa` 재검증. **C5-F1과 C5-F2는 덱 재빌드가 필요하므로 같은 회차에 묶어 처리하는 편이 싸다.**


---

# 재검증 1 (2026-09-10, R5 수정 루프 1 이후) — game-qa

> 같은 사이클 안의 **제자리 개정**이다(RFC-Q2). `cycle`·`status`·`supersedes` 는 그대로 두고 이 절만 append 했다. 위 §0~§10 본문은 **당시 기록으로 보존**한다.
> 대상은 디렉터 배정 **C5-F1** 1건이다. 레인 보고를 옮기지 않고 **덱을 다시 빌드하고 게이트를 직접 흔들어 봤다** — 아래 `D#` 가 QA 측 원문이다. 원본 파일은 하나도 수정하지 않았고, 음성 시험은 **스크래치패드 미러**(live 디렉터리로의 심볼릭 링크 + 생성기 사본)에서만 했다.
> **런타임 증거는 이번에도 0건이다.** 빌드 0줄 · 플레이 표본 n=0. 어떤 게이트도 PASS 로 올리지 않는다.

## D0. 재측정 명령 원문 [OBSERVED 2026-09-10, QA 직접 실행 · Node 로컬]

| # | 명령 | 결과 |
|---|---|---|
| D1 | `node _workspace/current/planning/validate-campaign.mjs` | `{checks 47, pass 47, fail 0, verdict PASS}` · sha `92301c0a5ecfc7e142646f5f17c29c21609e2b2de7c90408de7bffc9d815ae23` · **121,457 B**(RFC-Q1 대로 검증기 출력에서 읽음) |
| D2 | `node -e` 로 `stages[0]` 판독 | `T0.minutes 25` · `zoneIds ["hub"]` · 비트 도구 합집합 `["circuit","reader"]` · `proofRequired` 비트 **1** · 전 구역 **5** → 제외는 **4구역** |
| D3 | `grep -n "SLICE_ZONE_COUNT\|제3수문" presentation/generate-deck.mjs` | `SLICE_ZONE_COUNT` **0행**. `제3수문` 2행 = L281(세계 지도 SVG 의 구역 라벨 — T0 범위 주장 아님) · L1807(**금지 문자열 게이트가 조각 조립으로 만든 검사 문자열**) |
| D4 | `node presentation/generate-deck.mjs --out <scratchpad>/deck-qa-r4.html` | exit 0 · `errors: []` · 경고 1(외부 링크 16장) · 리포트 `sliceT0 = {id T0, minutes 25, zoneIds ["hub"], tools ["circuit","reader"], proofBeats 1, hintTiers 3, excludedZones 4}` — **D2 와 문자 일치** |
| D5 | `shasum -a 256` · `wc -c` (스크래치패드 산출물 ↔ live `steam-game-plan.html`) | 양쪽 **`0468eab2bb1f5e1b9fe6c970b8cf39ceaa5128a874ad3c3e4e0c5c1347783e8c` · 100,041 B — 바이트 동일**. 즉 live HTML 은 현재 생성기의 산출물이며 스테일이 아니다 |
| D6 | `shasum -a 256 presentation/generate-deck.mjs` · `wc -c` | `cc88f17f1af536e67225f65626603d7309926fb789142aa187398b2ce8a1713f` · **107,237 B** — `steam-game-plan.meta.md` r4 산출물 표와 **문자 일치** |
| D7 | `grep -o` 로 11·26번 렌더 문장 | 11번 "T0 마지막 당직 인수, **25분, 당직실에서만** 진행한다." · 26번 "포함은 **당직실**, 도구 **circuit 와 reader**, 결론 1건, 힌트 3단계." · "제외는 나머지 **4구역**과 …" |
| D8 | 음성 시험 **N1**(미러 사본에 옛 하드코딩 복귀: 26번 문장에 `허브와 제3수문` · `reader 와 alignment 와 seal`, `SLICE_ZONE_COUNT = 2` 재도입) | **exit 1** · `errors` 4건 — 하드코딩 3건 + "26번 T0 포함 범위 문장이 파생값으로 렌더되지 않았다" |
| D9 | 음성 시험 **N2**(미러의 `unity-implementation.md` §10 을 `hub`+`gate` · `reader`+`seal` · **30분** 으로 흔듦) | **exit 1** · `errors` 3건 — "T0 구역이 정본 10절과 다르다: 데이터 hub vs 정본 gate/hub" · "T0 도구가 … circuit/reader vs reader/seal" · "T0 분이 … 25 vs 30" |
| D10 | 양성 대조(미러, 무수정) | exit 0 · sha `0468eab2…` — D5 와 동일 |
| D11 | `bash .claude/skills/game-ops-harness/scripts/freshness-check.sh` | `0 finding(s) across 95 markdown artifact(s)` · exit 0 |
| D12 | C5-F5 존속 확인: `grep -o "기술 인수 테스트 [0-9]*개" steam-game-plan.html` · `grep -c "매체 경로"` · `grep -oE "T-[0-9]{2}" unity-implementation.md \| sort -u \| wc -l` | **14개** · **1** · **27** — C5-F5 는 그대로 open |

## D1. 판정 — **C5-F1 closed**

- **자기모순이 소멸했다 [OBSERVED, D4·D7]**: 11번과 26번이 이제 **같은 원본(`firstStage`)에서 파생된 같은 문자열**(`sliceZoneText`)을 쓴다. 두 슬라이드가 다시 갈라지려면 데이터가 갈라져야 하는데, 데이터는 하나다. QA 가 요구한 것은 26번의 파생화였고 레인은 **11번의 손으로 적은 구역명까지 같은 변수로 묶었다** — 재발 경로를 하나 더 없앤 것이다.
- **`SLICE_ZONE_COUNT` 는 없다 [OBSERVED, D3]**: 제외 구역 수가 `allZones.length - sliceZoneIds.length` = **4** 로 계산돼 D2 와 일치한다. 남은 `제3수문` 2행은 지도 라벨과 **게이트 자신의 검사 문자열**이며, 후자는 자기 오탐을 피하려 조각으로 조립돼 있다 — 정확한 처리다.
- **게이트가 실제로 작동한다 [OBSERVED, D8·D9·D10]**: QA 가 레인 보고를 믿지 않고 **직접 흔들었다**. N1 은 옛 하드코딩을, N2 는 정본 §10 드리프트를 각각 exit 1 로 잡았고, 양성 대조는 exit 0 이다. 특히 **N2 는 C4-F8(정본 T0 가 30분으로 어긋난 사건)과 같은 드리프트를 덱 빌드가 스스로 잡는다**는 뜻이다 — 이 게이트는 presentation 레인의 결함 재발만 막는 것이 아니라 **systems 레인의 정본 드리프트를 덱 쪽에서 한 번 더 잡는 교차 검사**다. QA 는 이것을 요구하지 않았다.
- **산출물이 스테일이 아니다 [OBSERVED, D5·D6]**: live HTML 이 현재 생성기의 산출과 **바이트 동일**하고, 메타 r4 표의 sha·바이트가 실측과 문자 일치한다. "고쳤다고 적었으나 산출물은 옛 것"이라는 실패 양식이 아니다.
- **자기 규약이 회복됐다**: `deck-outline.md` L13("슬라이드에 숫자를 손으로 적어 넣지 않는다")이 26번에서 다시 참이 됐다. 다만 **C5-F5 에서는 아직 거짓**이다(D12) — 17번의 "기술 인수 테스트 **14개**"(정본 27)와 폐기 용어 "매체 경로" 1회가 그대로다.

**측정 방법에 대한 QA 자기 기록 [OBSERVED]**: 첫 N2 시도는 미러의 생성기를 **심볼릭 링크**로 두어 실패했다 — Node 가 `import.meta.url` 을 실경로로 해석해 미러가 아니라 live 문서를 읽었고 exit 0 이 나왔다. 생성기를 **실제 사본**으로 바꾼 뒤 D9 의 3건이 나왔다. 이 함정을 남기는 이유는, 같은 방식으로 게이트를 시험하는 다음 사람이 **"게이트가 안 잡는다"는 거짓 음성**을 얻지 않게 하기 위해서다.

## D2. 이번 루프에서 닫히지 않은 것 (배정 밖 · 그대로 open)

| id | severity | 상태 | 확인 |
|---|---|---|---|
| C5-F2 대장 뒤처짐 | S2 | open | director 미착수 |
| C5-F3 `cycles/c5-development.md` 부재 | S2 | open | director 미착수 |
| C5-F4 메타 r3 표 고정 숫자 | S3 | open | 레인이 r4 절에서 스스로 "이번 배정 밖이라 손대지 않았다"고 고지 — 정직한 처리 |
| C5-F5 17번 테스트 수 14 vs 27 · 폐기 용어 | S3 | open | D12 로 존속 재확인 |
| C5-F6~F11 | S2·S3·S4 | open | 해당 레인 미착수 |

**C5 집계 갱신**: 신규 11 · closed **1**(C5-F1) · open **10**. 열린 S1 **0** 유지, open S2 **4 → 3**(F2·F3·F6).

## D3. 승격 판정 (presentation 레인)

| 파일 | 판정 | 사유 |
|---|---|---|
| `presentation/generate-deck.mjs` · `steam-game-plan.html` | **차단 유지** | C5-F5 가 **렌더된 화면의 사실 오류**(테스트 수 14 vs 27)와 **systems 가 명시적으로 폐기한 용어**를 그대로 담고 있다(D12). C5-F1 하나로는 덱이 승격되지 않는다 |
| `presentation/deck-outline.md` | **차단 유지** | 자기 규약 L13 이 C5-F5 에서 아직 거짓 |
| `presentation/steam-game-plan.meta.md` | **차단 유지** | C5-F4(r3 표 고정 숫자, RFC-Q1 위반)가 이 파일 소유 |
| `presentation/video-study.md` | **차단 유지** | C4-F18 · `cycle: …-c2` |

**C5-F5 와 C5-F4 는 다음 루프에 묶어 배정할 것을 권한다** — 둘 다 덱 재빌드가 필요하고, 지금 묶으면 HTML sha 갱신이 **한 번**으로 끝난다(C5-F1 에서 이미 한 번 갱신됐다).

## D4. 이 재검증이 증명하지 않는 것 [OBSERVED]

- **덱이 옳은 범위를 말한다는 것이지, 그 범위가 만들어졌다는 것이 아니다.** Unity `Assets` 0파일 · T0 빌드 0줄 · `observedMedianMinutes: null` · `humanPlaytests: []`.
- **게이트 3종(N1·N2 로 확인)은 문자열·데이터 대조 게이트다.** 슬라이드가 *읽는 사람에게 설득력 있는가*는 측정하지 않았다.
- **가격·수익·판매량은 여전히 PM 소유의 목표·가정**이며 이번 재검증으로 승격된 값은 0건이다.
- **G1~G8 은 0/8 PASS 유지.** 이 절은 `qa/gate-measurements.md` 의 어떤 값도 PASS 방향으로 올리지 않는다.

---

# 재검증 2 (2026-09-10, R4/R5 수정 루프 2 이후) — game-qa

> 같은 사이클 안의 **제자리 개정**이다(RFC-Q2). `cycle`·`status`·`supersedes` 는 그대로 두고 이 절만 append 했다. 위 1차 본문과 「재검증 1」 절은 **당시 기록으로 보존**한다(CLAUDE.md §2).
> **이번 루프의 R5 축 배정은 0건이다.** 배정된 결함은 `C4-F19`(S2 · systems) 하나이며 판정 전문은 `qa/c4-review.md` **「재검증 2」** 절(V2.0~V2.7)에 있다. 이 절은 **(a) R4 편집이 R5 산출물을 움직였는지**와 **(b) 그 과정에서 R5 결함 하나의 범위가 커졌다는 사실** 두 가지만 기록한다.

## D2-0. 재측정 명령 원문 [OBSERVED 2026-09-10, QA 직접 실행]

| id | 명령 | 결과 |
|---|---|---|
| Y1 | `shasum -a 256 presentation/steam-game-plan.html` + `wc -c` | **`0468eab2bb1f5e1b…3e8c` · 100,041 B** — 재검증 1 값과 **바이트 동일**. 이번 루프에서 덱은 **재빌드되지 않았고 변경되지도 않았다** |
| Y2 | `node planning/validate-campaign.mjs` | `checks 47 · pass 47 · fail 0 · PASS`, sha `92301c0a5ecfc7e1…ae23` · **121,457 B**, exit 0 — 정본 입력 불변 |
| Y3 | `bash .claude/skills/game-ops-harness/scripts/freshness-check.sh` | `0 finding(s) across 96 markdown artifact(s)` · exit 0. 재검증 1 = 95 → **+1 = `systems/tech-verification/c4-fixloop2-input-binding.md` 1건뿐** |
| Y4 | `git status --short` | 변경 범위가 `_workspace/current/systems/`(디렉터리 `??` 한 줄)와 `qa/` 에 한정. `product/`·`production/`·`presentation/` 신규 쓰기 **0건** |
| Y5 | `grep -rn "매체 경로" _workspace/current \| grep -v qa/ \| grep -vE "폐기\|잔여\|배정 밖\|세 번째 용어"` | **12곳 / 9파일** — 아래 D2-2 |
| Y6 | `node systems/prototype/test-model.mjs` | **37 통과 / 0 실패** · exit 0 |

## D2-1. R4 루프 2 편집이 R5 축에 미친 영향 — **0건** [OBSERVED]

- [Y1] 덱 HTML 이 **바이트 동일**하므로 `C5-F2`(원장 스테일) · `C5-F4`(r3 측정표) · `C5-F5`(17번 슬라이드) 의 관측 대상은 **문자 하나도 움직이지 않았다**. 재검증 1 의 판정을 그대로 `[CARRIED]` 한다.
- [Y4] `product/`·`production/` 쓰기 0건 → `C5-F3`(`production/cycles/c5-development.md` 부재) · `C5-F7`~`C5-F11` 전부 **open 유지**. 이번 루프는 이 결함들을 **건드리지도, 악화시키지도 않았다**.
- [Y3] 산출물 수 +1 은 systems 레인의 검증 영수증 1건이다. `qa/c5-review.md` §1-a 가 적은 대로 **산출물 수는 회귀 신호가 아니다** — 여기서는 증가분의 정체가 명확하다는 사실만 기록한다.

## D2-2. C5-F5 **범위 확대** (S3 유지 · presentation → + systems · economy · balance)

1차 검토는 폐기 용어 「매체 경로」의 잔존을 **`{plate-readout, drainage-routing}.md` · `beats.md`** 로 적고 "systems 레인 소유이므로 브로드캐스트만 한다"고 했다. **이번 재측정은 그 목록이 부분집합임을 보인다.**

[OBSERVED 2026-09-10 · Y5] 폐기 선언·잔여 보고 문장(`unity-implementation.md` L69 · `c4-self-check.md` L62 · `c4-fixloop2-input-binding.md` L71 · `drainage-routing.md` L139 · `steam-game-plan.meta.md` L256)과 `qa/` 를 제외한 **본문 사용 = 12곳 / 9파일**:

| 파일 | 행 | 1차 C5-F5 가 지목했는가 |
|---|---|---|
| `systems/system-specs/plate-readout.md` | L60 · L70 · L118 | 지목 |
| `systems/system-specs/drainage-routing.md` | L57 · L74 | 지목 |
| `systems/data-schemas/beats.md` | L161 | 지목 |
| `systems/system-specs/wiring-trace.md` | L72 | **미지목** |
| `economy/currency-map.md` | L131 | **미지목** |
| `economy/sink-source-ledger.md` | L122 (`INV-P3` 불변식 문장) | **미지목** |
| `balance/balance-sheet.md` | L494 (`safety_two_disjoint_media_paths` 설명) | **미지목** |
| `presentation/generate-deck.mjs` | L866 | 지목(관측 B) |
| `presentation/steam-game-plan.html` | L167 (L866 의 렌더 산출물) | 파생 |

- **왜 중요한가**: C5-F5 의 요구 수정에는 "**폐기 문구 게이트에 `매체 경로` 를 추가한다**"가 들어 있다. 지금 그 게이트를 켜면 presentation 2곳이 아니라 **12곳이 걸리고 4개 레인이 동시에 열린다**. 게이트를 먼저 켜고 나서 놀라는 대신, **켜기 전에 12곳을 아는 편**이 낫다.
- **경중 구분** [INFERENCE]: `sink-source-ledger.md` L122 와 `balance/balance-sheet.md` L494 는 **불변식의 이름과 설명**이라 용어 교체가 곧 불변식 문장 재작성이다. 나머지 8곳은 산문 교체다. 이 구분은 측정이 아니라 QA 판단이며, 실제 난이도는 소유 레인이 판정한다.
- **severity 는 S3 유지**: 정본 표현(`sourceType` 상이 AND `originId` 상이)이 `unity-implementation.md` L69 에 한 번 서 있고 데이터·게이트는 그 필드로 동작한다 — 틀린 것은 **문서의 용어**이지 규칙이 아니다.
- owner 갱신: `game-presentation-director`(덱 2곳) · `game-systems-designer`(6곳) · `game-economy-designer`(2곳) · `game-balance-designer`(1곳). **한 레인이 남의 파일을 고치지 않는다.**

## D2-3. 승격 판정 (presentation·product·production 레인) — **전건 [CARRIED]**

| 파일 | 재검증 1 판정 | **재검증 2 판정** | 사유 |
|---|---|---|---|
| `presentation/steam-game-plan.html` · `generate-deck.mjs` | 차단 유지 | **[CARRIED] 차단 유지** | Y1 바이트 동일. C5-F4·C5-F5(확대) 그대로 |
| `presentation/deck-outline.md` | 차단 유지 | **[CARRIED]** | 자기 규약 L13 이 C5-F5 에서 아직 거짓 |
| `presentation/steam-game-plan.meta.md` | 차단 유지 | **[CARRIED]** | C5-F4(r3 표 고정 숫자, RFC-Q1) |
| `presentation/video-study.md` | 차단 유지 | **[CARRIED]** | C4-F18 · `cycle: …-c2` |
| `product/*` · `production/cycle-ledger.json` | 차단 유지 | **[CARRIED]** | C5-F2·F3·F7~F11 미착수, Y4 로 쓰기 0건 확인 |

**여전히 권한다**: `C5-F4` 와 `C5-F5` 를 **한 편집으로 묶을 것**. 둘 다 덱 재빌드가 필요하고, 지금 묶으면 HTML sha 갱신이 한 번으로 끝난다. **D2-2 의 systems·economy·balance 8곳은 그 묶음과 별개**이며 각 소유 레인이 자기 파일에서 처리한다.

## D2-4. 이 재검증이 증명하지 않는 것 [OBSERVED]

- **R5 축에서 이번에 닫힌 결함은 0건이다.** C5-F1 만 재검증 1 에서 closed 이고 나머지 **10건은 그대로 open** 이다.
- **덱이 안 바뀌었다는 것은 덱이 옳다는 뜻이 아니다.** Y1 은 "움직이지 않았다"만 말한다.
- **가격·수익·판매량·8시간 플레이타임은 여전히 목표·가정**이며 이번 루프로 승격된 값은 0건이다. Unity `Assets` 0파일 · `observedMedianMinutes: null` · `humanPlaytests: []` 불변.
- **G1~G8 은 0/8 PASS 유지.** 이 절은 `qa/gate-measurements.md` 의 어떤 값도 PASS 방향으로 올리지 않는다.
