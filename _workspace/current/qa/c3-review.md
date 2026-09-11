---
updated: 2026-09-10
cycle: 20260909-preproduction-c3
status: current
supersedes: null
owner: game-qa
---

# C3 독립 사양 검토 (2차) — 세계관·시놉시스·기획·시스템·밸런스·재화

## 0. 이 판본의 지위

- [OBSERVED] 같은 경로에 **이전 C3 검토본**(mtime `09-10 00:08:53`, `status: draft`)이 있었고 본 판본이 그것을 같은 경로에서 대체한다. 그 검토는 `_workspace/archive/20260909-preproduction-c3/planning/campaign.json`(당시 live)만을 대상으로 했고, 그 뒤 **레인 문서 40여 건이 00:10~01:04에 다시 쓰였다.** 이전 5건의 주장·처리 상태는 삭제하지 않고 §3에 전문 취지로 이월했다.
- 판정은 **문서 정합성**이다. 빌드 0줄·사람 플레이 n=0이므로 이 검토는 **어떤 게이트도 PASS로 올리지 않는다.** 플레이테스트를 만들어내지 않았고, 다른 레인 파일을 수정하지 않았다.
- 계약(`production/premium-preproduction-contract.md`, C3 개정)은 acknowledge한다. Honesty gates · Premium overrides · Evidence storage · Release safety 조항이 아래 판정을 그대로 지지한다.
- 표기: `[OBSERVED]` 명령·파일로 직접 확인 · `[INFERENCE]` 문서 대조 도출 · `[TARGET]` 설계 목표 · `[CARRIED]` 이전 회차 승계.

### 0.1 측정 방법 (재현 가능)

| # | 검사 | 명령/방법 | 결과 |
|---|---|---|---|
| M1 | live/archive `campaign.json` 구조 대조 | `node` 집계 (스테이지 분·비트 분·`tools`·`clues[].sourceType`·`hints`·`recovery`·`checkpoint`) | §1 표 |
| M2 | sha256·bytes 실측 | `shasum -a 256` · `wc -c` · `find _workspace -name campaign.json` | C3-F2 |
| M3 | 인용 문자열 전수 대조 | `grep -cF` (live vs archive) | C3-F16 |
| M4 | 용어집 수록 대조 | 명사 22종 × `grep -c "^| <term> "` on `worldview/glossary.md` | C3-F13 |
| M5 | frontmatter 인구조사 | 82개 `.md`의 `cycle/status/supersedes/owner` 추출 | C3-F8 |
| M6 | 신선도 | `bash .claude/skills/game-ops-harness/scripts/freshness-check.sh` | `#g8` |

## 1. 판정

> **최종 판정은 §7 「재검증 1 (2026-09-10)」이다.** 아래 §1~§6은 2026-09-09 1차 판정의 원문이며 삭제하지 않고 역사로 보존한다. 그 시점 이후 12개 레인이 수정했고, §7이 결함별로 다시 판정한다. §1.1의 `sourceType` 분포 한 줄은 §7.6에서 **QA 자체 오기**로 정정됐다(C3-F32).

**SPEC-FIX** — 차단(S1) **7건**, 수정(S2) **8건**, 경미(S3) **6건**, 관찰(S4) **3건**.

C3의 닫는 질문 "33비트가 480분 설계 예산과 행동 예산으로 역산되는가"에 대한 답: **아니오.** 산수는 두 계보 모두 재현된다(§1.1). 그러나 **역산 대상 파일이 두 벌**이고 `status: current` 문서 4종이 인용하는 계보는 **아카이브 안에만** 있다. 역산이 틀린 것이 아니라 **무엇을 역산했는지가 확정되지 않았다.**

> 디렉터 주의: C3-F1(계보)을 계보 B(현행 파일)로 닫으면 `gdd.md`·`content-matrix.md`·`campaign-time-budget.md`·`chapter-beats.md`·`telemetry-contract.md`의 분·집계를 같은 회차에 재도출해야 한다. 그 경우 planner·synopsis 두 레인에 한해 판정은 **SPEC-REDO**로 승격된다.

### 1.1 live `campaign.json` 실측 (2026-09-09T16:1xZ) [OBSERVED]

| 항목 | live `_workspace/current/planning/campaign.json` | archive `…/preproduction-c3/planning/campaign.json` |
|---|---|---|
| `cycle` | `20260909-preproduction-c4` | `20260909-preproduction-c3` |
| sha256 / bytes | `775a984c…` / 120087 | `5029d44a…` / 74342 |
| 스테이지 분 | 25·50·55·65·65·70·75·65·10 | 30·50·55·60·65·65·70·60·25 |
| 합 / 비트 / 스테이지별 합 | 480 / 33 / 9-of-9 일치 | 480 / 33 / 9-of-9 일치 |
| `fastMinutes` 합 / `deliberateMinutes` 합 | **322 / 673** | 321 / 672 |
| clue 수 | **72** | 70 |
| `sourceType` 분포 | log 26 · ledger 22 · plate 21 | log 25 · ledger 23 · plate 22 |
| 도구 등장 (circuit/reader/alignment/seal/corrosion/routing) | 10 · **11** · 8 · **7** · 3 · 3 | 10 · 8 · 8 · 6 · 3 · 3 |
| `tools: []` 비트 | **5** (`t0-b1 c1-b4 c3-b4 c7-b1 e0-b1`) | 8 |
| 매체 2종 미달 비트 | **1 (`c1-b4`: log/log)** | 0 |
| `hints` 3단 / `recovery` 비어있지 않음 / `checkpoint` | 33/33 · 33/33 · 33/33 | 33/33 · 33/33 · 33/33 |
| `zoneId`(비트 단위) | **0/33 부재** | 0/33 부재 |

## 2. 결함 목록

### C3-F1 · S1 · planner/synopsis/systems/balance/economy — `campaign.json`이 두 벌이고 `current` 4종이 아카이브를 인용한다

- 근거: 위 §1.1. `planning/gdd.md` §10 "출처는 `planning/campaign.json` `designMinutes`이며 …(30·50·55·60·65·65·70·60·25)" — **인용한 파일의 실제 값은 25·50·55·65·65·70·75·65·10**이다. 같은 불일치가 `content-matrix.md` §3(33행 분)·`campaign-time-budget.md` §1(스스로 "계보 A로 계산한다"고 선언)·`chapter-beats.md` 표 A·`telemetry-contract.md` §1(fast 321/deliberate 672)에 있다.
- 왜 차단인가: CLAUDE.md §2는 `_workspace/archive/`를 **읽기 전용 역사**로 규정한다. 현재 설계의 단일 진실이 역사 폴더 안에 있고, live 파일은 `campaign.meta.md`(draft, c4)만 설명한다. 어느 쪽을 고쳐도 최소 5개 문서가 동시에 움직인다.
- 요구 수정: 디렉터가 RFC-P3-008을 A/B 중 하나로 닫는다. ① B 채택 시 planner가 5개 문서의 분·집계를 같은 회차에 재도출하고 `campaign.meta.md`를 `status: current`로 올린다. ② A 채택 시 live 파일을 아카이브 계보 값으로 되돌리고 c4 개정분(`activityBudget`·`subtasks`·`proofRequired`·`toolTeaching`·`originId`)만 이식한다. **어느 쪽이든 "합계가 480이므로 문제없다"는 처리를 금지한다** — 두 계보 모두 480이므로 총합은 판별력이 없다.

### C3-F2 · S1 · systems/balance — 저장소에 존재하지 않는 sha256을 9개 문서가 [OBSERVED] 측정으로 인용한다

- 근거: `systems/data-schemas/{beats,hints,plates,save,tools,zones}.md` §0, `systems/architecture-contract.md` L263, `balance/balance-sheet.md` L17·L20, `balance/patch-deltas.md` L25, `balance/sim-results/README.md` L88이 모두 `planning/campaign.json`의 "2026-09-10 실측 sha256 `2bfe4d52…`"를 든다. 실측 [OBSERVED]: live = `775a984c…`, archive c3 = `5029d44a…`, 개행 제거본 = `1b69b5da…`. `find _workspace -name '*.json' | shasum` 전수에서 `2bfe4d52`로 시작하는 파일 **0건**.
- 파생 오류: `beats.md` §1.1은 "구조 사실(… fast 321 / deliberate 672 / 단서 70 / 도구 10·8·8·6·3·3)은 두 판본에서 동일했다"고 적었다. live 실측은 **322 / 673 / 72 / 10·11·8·7·3·3**이다. `balance-sheet.md` §0 재집계 영수증(염판 22·reader 8·seal 6)도 live에서 **21·11·7**이다.
- 요구 수정: 두 레인이 live 파일로 재측정하고 해시·집계를 갱신한다. 재측정 전까지 `beats.md` B-I1~B-I9, `balance-sheet.md` §3·§4·§7의 모든 유도값은 `[CARRIED]`로 강등한다(QA 운영 원칙 4). 8자리 축약 해시 대신 전체 해시를 적는다.

### C3-F3 · S1 · planner — 기각된 6법 재작성문이 `status: current` 문서에서 유통된다

- 근거: `worldview/consistency-audit.md` §4가 **기각**으로 보존한 문구가 `planning/gdd.md` §4 "대응 법" 열과 `planning/feature-specs/verb-0{2,3,4,5,6}.md`의 `law:` 필드에 그대로 있다.

| 법 | 캐논 (bible §3) | gdd §4 · feature-specs `law:` | audit §4 판정 |
|---|---|---|---|
| 2 | 판은 재생할수록 닳는다 | "원본은 닳지만 사본은 남는다" | 기각 |
| 3 | 시계는 조수에 매인다 | "정합 전 시계는 믿지 않는다" | (부연, 경계) |
| 4 | 물은 한 번에 한 곳으로만 간다 | "이번 조수에는 보호 용량이 부족하다" | 기각 |
| 5 | 소금은 모든 것을 먹는다 | "소금은 비용으로 보인다" | 기각 |
| 6 | 당직은 하나, 서명은 둘 | "원본 책임과 제출을 나눈다" | 기각 |

- 대조: `systems/system-specs/{plate-readout,drainage-routing,corrosion-budget,dual-seal,tide-alignment}.md`는 **캐논 원문**을 쓴다. 즉 같은 법을 두 레인이 다른 이름으로 부른다. `synopsis/continuity.md` §7 마지막 행이 이미 같은 사실을 director 판정 요청으로 올렸다.
- 요구 수정: planner가 gdd §4와 feature-specs 6종의 법 문구를 bible §3 원문으로 되돌린다. 기각된 문장은 UX 설명으로 별도 열에 둘 수는 있으나 "대응 법"으로 표기하지 않는다. 레트콘 금지(CLAUDE.md §9).

### C3-F4 · S1 · balance/economy — 이 게임 유일 소모 자원의 정본이 둘이다

| 축 | `balance/balance-sheet.md` §4 (c3, current, G2 소스) | `economy/currency-map.md` §4.1 (c3, current, G3 소스) |
|---|---|---|
| 한도 구조 | **6계통** `brine_line 14 · power_bus 14 · reader_head 12 · seal_press 9 · gate_valve 9 · pump_motor 9` | **전역 단일 9** (`model.mjs:79`) |
| 계통별 한도 존재 | 정의함 (`corrosion_limit_*` 6키) | "계통별 한도는 **데이터에 존재하지 않는다** [OBSERVED]" |
| 리셋 | `budget_reset_on_stage: false` | "**장 경계 전액 리셋**" |
| 소모 도구 | circuit·reader·seal·routing | `routing` **1종** |
| 캠페인 총 소모 | 계통별 10/10/8/6/6/6 | **7 또는 8, 1회** |

- 대조: `worldview/glossary.md` §4 "부식예산 = **계통별** 변경 허용 한도(법5)" — 캐논은 계통별이다. `synopsis/chapter-beats.md` §3은 balance 모델(brine_line 10/14 …)을 인용하고, `economy/sink-source-ledger.md` §3.1·§4.1 안전 하한 공식(9 ≥ 8)은 전역 모델 위에 선다.
- 왜 차단인가: G2 대체 검사(부식 잔여율 ≥20%)와 G3 대체 검사(고갈 진행 불가 0)가 **서로 다른 자원을 재고 있다.** 어느 쪽으로 측정해도 상대 게이트를 지지하지 못한다.
- 요구 수정: RFC로 한 모델을 정본으로 고르고, 진 쪽 문서의 표를 인용으로 교체한다. 전역 9를 택하면 `chapter-beats.md` §3과 balance §4.2·§4.3이 무효가 되고, 계통별을 택하면 economy §4.1 안전 하한 공식·`model.mjs` 상수·`sink-source-ledger` §3.1이 재작성 대상이다.

### C3-F5 · S1 · planner/synopsis — live `campaign.json`이 연표 캐논을 뒤집는다

- 근거 [OBSERVED, 전수 검색]: live JSON에 `H-1:24`·`H-1:04`가 `c6-b3`에, `H+0:12`가 `c4-b1`에 있다. `H-1:20`은 **0건**. `planning/campaign.meta.md` §5·§7-2와 `synopsis/campaign.md` §2가 "밸브 개폐 H-1:24가 봉인 완료 접점 H-1:04보다 20분 앞선다"로 명시한다.
- 대조: `worldview/timeline.md` §2 `H-1:40` 도연 서명 → `H-1:20` "현장이 서명 확인 전에 밸브를 돌림", §8 앵커 "봉인 호출과 밸브 집행이 같은 염판 1매에 연속 각인", `H+0:10` 침수. `synopsis/chapter-beats.md` B17(current) 힌트3 "**호출이 먼저, 밸브가 20분 뒤**". 즉 live 데이터와 current 서사가 **선후를 반대로** 말한다.
- 부가: `H+0:10 → H+0:12` 변경은 T-12 진실표 수정이며 `timeline.md` §6이 "T-12 이전 사실은 season 사이클 RFC로만"으로 잠근 구간이다. `campaign.meta.md` §7이 "적용 대기"라고 적었지만 **데이터에는 이미 적용돼 있다**.
- 요구 수정: 데이터를 캐논에 맞추거나(권장, 0분) 캐논 개정 RFC를 열고 승인 전까지 live JSON의 세 시각을 되돌린다. "문서에 반영되면 성립한다"는 상태로 데이터를 먼저 바꾸는 것은 계약 Evidence storage 위반이다.

### C3-F6 · S1 · planner/synopsis — 공개 상한 초과: 튜토리얼이 '한도연'과 '판 #0'을 노출한다

- 근거 [OBSERVED, JSON 전수]: `t0-b1.subtasks[0]` "…당직 주임 이름(**한도연**)을 확인한다", `t0-b1.clues[0]` "당직 주임이 \"**한도연**\"으로 기재돼 있다", `t0-b1.objective` "…숨겨둔 **판 #0**을 오늘 밤 이관 목록에 올릴지 결정한다". `synopsis/scenes-and-dialogue.md` S1 지문도 동일("당직 주임 한도연. 인쇄된 활자다").
- 대조: `worldview/timeline.md` §7 **B01 금지 열** "12년 전 사건의 인과 일체", 판 #0은 **B07(1장)** 슬롯. `synopsis/chapter-beats.md` §4-6 "`t0-b1` 판 #0을 튜토리얼에서 공개하지 않는다 — **튜토리얼 공개는 상한 초과다**". `synopsis/continuity.md` §4 "한도연 게임 내 최초 등장 = **B18**", `synopsis/synopsis.md` §7 금지 5.
- 또한 live `c1-b4.consequence`는 "…**누군가 도연을 도왔다**"가 사건판에 오른다고 적는다. `continuity.md` §4는 그 문장을 **저자 표기이며 게임 내 문자열이 아니다**라고 못 박고 "두 번째 서명자는 이름을 남기지 않았다"를 실현 문구로 지정했다.
- 요구 수정: `t0-b1`에서 이름·판 #0 노출을 제거하거나, worldview가 §7 상한표 B01·B07을 개정한다. 데이터와 씬이 상한을 먼저 넘은 상태를 유지하지 않는다. `c1-b4.consequence`는 continuity §4 문구로 교체한다.

### C3-F7 · S1 · synopsis — 본편 마지막 대사가 법1을 위반한다

- 근거: `synopsis/scenes-and-dialogue.md` S6 마지막 줄 — 서린 "…밸브가 봉인보다 이십 분 빨랐다는 것. 그리고 **그걸 누가 언제 확인했는지**."
- 대조: `worldview/worldview-bible.md` §2 "남지 않는 것: 얼굴, **의도**, 대화 내용…". `planning/campaign.meta.md` §7-1이 스스로 "남지 않는 것" 줄에 "**사람이 무엇을 확인했는지**"를 더하라고 적었다. `synopsis/synopsis.md` §7 금지 7 "기록되지 않은 인간 행위(예: \"서명을 확인했다\")를 확정 근거로 쓰기". 즉 **이전 C3 검토 F1이 지적한 바로 그 명제가 게임의 마지막 문장으로 남아 있다.**
- 요구 수정: 해당 절을 "그 순서가 기계 각인 두 줄로 남았다는 것" 류의 기록 가능 명제로 교체. 분 배분 영향 0.

### C3-F8 · S2 · worldview — 아카이브 c3 세계관 2건이 고아이며 supersedes 체인이 포크했다

- 근거 [OBSERVED, frontmatter 인구조사]: `_workspace/archive/20260909-preproduction-c3/worldview/{worldview-bible,timeline}.md` = `status: superseded`, `supersedes: …/c2/…`. 현재 `_workspace/current/worldview/{worldview-bible,timeline}.md` = `status: current`, `supersedes: …/**c2**/…`. **두 문서가 같은 c2 조상을 가리키는 포크**이며, c3 아카이브본을 잇는 current 문서는 없다(`grep "archive/20260909-preproduction-c3/worldview" _workspace/current` → 0건).
- 배경: `planning/campaign.meta.md` §7이 "부모가 만든 C4 후속본이 다른 세션의 c3 계보 덮어쓰기로 사라졌다"고 기록한다. 즉 소실이 이미 관측됐다.
- 신선도 검사의 한계 [OBSERVED]: `freshness-check.sh`는 exit 0 / 0 finding을 반환한다. 스크립트 범위가 "frontmatter contract + supersedes topology"인데 **고아 아카이브(어떤 current도 가리키지 않는 superseded 문서)를 검출하지 않는다.**
- 요구 수정: director가 두 계보를 병합해 current bible/timeline의 `supersedes`를 c3 아카이브본으로 다시 걸고, 소실된 C4 후속본의 내용 차분을 `consistency-audit.md` §4 방식으로 보존한다. 겸하여 freshness 스크립트에 고아 검사 추가를 요청한다.

### C3-F9 · S2 · worldview/balance/systems — 자원 차감 시점이 두 규정으로 갈린다

- 근거: `worldview-bible.md` §3-bis.2 "본인 서명 + 증인 봉인이 붙는 **그 순간에만** … 법2 재생 1회 차감 … 법5 한도 차감". `balance/balance-sheet.md` §4.1은 **도구 확정 1회당** 차감(circuit→brine_line 1+power_bus 1 등)으로 규정한다.
- 충돌 지점: `synopsis/chapter-beats.md` 표 A의 T0에는 `seal`이 없는데 B02가 `brine_line 1 · power_bus 1`, B03이 `reader_head 1 · 원판 재생 1`을 소모한다. 봉인이 없으므로 bible 규정상 **비용이 발생할 수 없다.** `continuity.md` §7 RFC-N5가 같은 문제를 이미 등록했다.
- 요구 수정: worldview 주관으로 하나를 고른다. 봉인 시점을 유지하면 balance §4.3 장별 누적표(T0 2/2/1)와 chapter-beats 표 A의 T0·C2·C3 소모 행이 0으로 바뀐다.

### C3-F10 · S2 · planner/systems — 확정 입력 방식이 두 문서에서 반대다

- 근거: `planning/gdd.md` §3.3 층 B "**길게 누름으로** 결과를 세계에 적용한다", §5 "**0.4초 길게 누름은 확정 전용**". `systems/interaction-rules.md` §1-1 "`two-step` (**기본**) … 홀드 없음 / `hold` (**opt-in**)", §0 규칙 9 "길게 누름은 선택 기능이다".
- 부가: `interaction-rules.md`는 `cycle: c5`, `status: draft`다. GDD(current)가 draft 문서를 접근성·입력의 정본으로 인용하는 구조 자체가 계약 위반 소지다(같은 문제: `balance/puzzle-balance.md` c4 draft를 gdd §6이 인용).
- 요구 수정: systems 쪽 최신 규칙으로 gdd §3.3·§5를 정정하고, GDD가 인용하는 systems·balance 문서의 `status`를 current로 승격하거나 인용을 잠정 표기로 바꾼다.

### C3-F11 · S2 · planner — live 데이터의 비트별 필수 요소 누락 (요청된 전수 검사)

33비트 각각에 **자료 2종 · 동사 · 힌트 3단 · 복구 · 저장**이 있는지 live `campaign.json` 기준 [OBSERVED]:

| 요소 | 충족 | 누락 비트 id |
|---|---|---|
| 서로 다른 매체 2종 | 32/33 | **`c1-b4`** (clue 2개 모두 `sourceType: log`) |
| 동사(`tools` 비어있지 않음) | 28/33 | **`t0-b1` `c1-b4` `c3-b4` `c7-b1` `e0-b1`** |
| 힌트 3단 | 33/33 | — |
| 복구(`recovery` 비어있지 않음) | 33/33 | — |
| 저장(`checkpoint`) | 33/33 | — |

- 거짓이 된 주장: `planning/content-matrix.md` §4.2 "**단일 매체 0건** [OBSERVED]", `planning/feature-specs/verb-02-plate-read.md` D3 "33/33 [OBSERVED] PASS", `systems/data-schemas/beats.md` B-I5 "PASS (33/33)", §4 "실제 17개 키"(live는 23키), B-I3 "clue 70"(live 72). `content-matrix.md` §4.1 "도구를 쓰지 않는 사건 **0건**"은 `chapter-beats.md` 표 A의 연습 배정을 센 값이며 **데이터에는 반영돼 있지 않다**(5건).
- 요구 수정: (a) `c1-b4`에 염판 또는 대장 단서 1건 추가 — `content-matrix.md` §3은 이미 이 비트를 "일지 + 염판"으로 적고 있다. (b) `chapter-beats.md`가 33/33에 부여한 연습 동사를 데이터의 `tools`/`toolTeaching`에 반영하거나, 문서 쪽 "33/33 동사" 주장을 "확정 28 + 문서상 연습 배정"으로 정정. (c) 위 4개 문서의 [OBSERVED] 수치를 재측정값으로 교체.

### C3-F12 · S2 · synopsis/systems — 불파괴 쌍(P2)이 독립성 규칙과 충돌한다

- 근거: `systems/interaction-rules.md` §3 "`copiedFrom`은 재귀적으로 **루트 출처까지 해석**한다. 복제 스캔·표면 부식·재촬영본은 루트의 `originId`를 물려받는다. **표시 라벨은 증명이 아니다**". `planning/campaign.meta.md` §2도 "`council-copybook`은 `tide-ledger-bureau`의 사본 … 서로를 뒷받침하지 못한다".
- 충돌: `synopsis/continuity.md` §5는 K1~K10의 **경로 2를 대부분 "접수부 확정 사본" 또는 "주민회 사본"과 원본 매체의 조합**으로 세운다 — K1 "조위대장 × 접수부 확정 사본(B04)", K3 "서명지 × 접수부 사본", K6 "접수부 확정 사본 × 근무 기입", K9 "승인 서류 × 주민회 보관 사본". 사본이 그 원본의 루트를 물려받는다면 K1·K3은 **같은 루트끼리의 쌍**이라 독립 판정에서 거부된다. 그런데 §5는 "P2 10/10 충족 — 전부 접수부 확정 사본 또는 관외 주민회 사본을 포함"이라고 결론짓는다.
- 왜 중요한가: P2는 계약 Premium overrides의 "진행 막힘 0 / 결말 3갈래 도달성"을 지탱하는 단 하나의 문서상 근거다. 이 쌍이 무효면 G7 대체 검증의 유일한 논거가 사라진다.
- 요구 수정: 각 필수 단서마다 **루트 `originId`가 서로 다른** 불파괴 쌍을 명시한다(예: 접수부 사본 ↔ 주민회 사본은 둘 다 조위대장 파생이므로 쌍이 되지 못한다). 또는 systems가 "확정 사본은 새 루트를 갖는다"를 §3에 예외로 명문화한다. 어느 쪽이든 `worldview-bible.md` §3-bis.3 P1·P2 문구와 대조 승인이 필요하다.

### C3-F13 · S2 · worldview — 용어집 미수록 명사가 UI·대사·데이터에 이미 쓰인다

`worldview/glossary.md` 머리 규칙: "여기에 없는 고유명사는 시놉시스·대사·UI 문자열·에셋 파일명에 쓸 수 없다." 실측 [OBSERVED, M4]:

| 명사 | 용어집 | 사용처 (발췌) |
|---|---|---|
| 가설판 | **없음** | `systems/game-ui-contract.json`, `interaction-rules` §2.2, `gdd` §4, `plate-readout.md` |
| 증거함 | **없음** | `game-ui-contract.json`, `interaction-rules` §1, `campaign.json`, `scenes-and-dialogue` |
| 사건판 | **없음** | `campaign.json` consequence, `reward-bands.md` |
| 자동 사본 | **없음** | `gdd` §3.3 원칙3, `balance-sheet` §3.3, `verb-02` R2, `sink-source-ledger` |
| 8분 분해능 | **없음** | `balance-sheet` §3.3, `chapter-beats` B03 |
| 봉인 완료 접점 | **없음** | `campaign.json`, `campaign.meta` §7-1, `scenes-and-dialogue` S2 |
| 인수 각서 / 이관 목록 / 근무표 / 당직 자격 명부 | **없음** | `campaign.json`, `chapter-beats` 표 B, `continuity` |
| 소금 그늘 / 계통판 / 결번 / 상시 슬롯 | **없음** | `campaign.json`, `content-matrix`, `chapter-beats` |

- `continuity.md` §6.1은 미수록을 **3건**("인수 목록"·"당직 자격 명부"·"근무표")으로 보고했다. 실제 범위는 그보다 넓고, 그중 `가설판`·`증거함`은 **UI 계약 파일의 패널 이름**, `봉인 완료 접점`·`자동 사본`·`8분 분해능`은 **세계 물리 규칙**이라 worldview 소유다.
- 요구 수정: worldview가 위 명사를 용어집으로 승격하거나 사용 금지 판정을 내린다. 특히 `봉인 완료 접점`은 bible §2 "남는 것" 목록 개정(= 캐논 변경)이 선행돼야 하며, 그 전까지 `campaign.json`·씬 대사에서 쓰일 수 없다.

### C3-F14 · S2 · qa/director — 수용 조건이 `afk` 처리 미정의라 측정해도 판정 불가

- 근거: 계약 `## Time acceptance` "중앙값 ≥ 420 · p25 ≥ 360 · p75 ≤ 600". `systems/ops/telemetry-contract.md` §5 AF3은 보고를 `total_min` / `afk_total_min` / `total_minus_afk_min` **세 값 병기**로 규정하고 AF4는 "제외 여부를 명시하지 않은 시간 수치는 무효"라고 한다. 그런데 **수용 조건이 세 값 중 무엇을 쓰는지 어디에도 없다.**
- 부가 분쟁 미해소: `design_fast_min` 321/322 < 하한 360, `design_deliberate_min` 672/673 > 상한 600. `campaign.meta.md` §6-1은 "시나리오 경계를 표본 봉투와 비교하는 것은 범주 오류"로 기각하고, `campaign-time-budget.md` §9.3은 "하한 이탈은 별도 위험으로 유지"한다(RFC-P3-011). 두 문장이 모두 `_workspace/current`에 살아 있다.
- 요구 수정: 계약에 판정 키를 하나 못 박는다(권장: `total_minus_afk_min`, 단 `total_min` 병기 필수). RFC-P3-011은 director가 닫는다.

### C3-F15 · S2 · systems/balance/planner/economy — 스테일 [OBSERVED]가 현재 파일 상태와 반대다

| 주장 | 위치 | 현재 사실 [OBSERVED] |
|---|---|---|
| "`systems/data-schemas/`는 현재 **비어 있다**" | `balance/balance-sheet.md` §2 | `beats/hints/plates/save/tools/zones.md` 6종 존재 |
| "`systems/data-schemas/`에 `zones.md`만 존재" | `planning/feature-specs/verb-02-plate-read.md` D5 | 동상 |
| "`systems/ops/`는 **빈 디렉터리**이며 `telemetry-contract.md`가 없다 → **G3는 PASS할 수 없다**" | `economy/sink-source-ledger.md` §6 | `systems/ops/telemetry-contract.md`(c3, current) 존재 |
| "스키마 5파일과 telemetry-contract가 **존재하지 않는다**" | `planning/update-scope.md` P3 | 6+1 존재 |
| "`campaign.meta.md` 실물 `74341 / 2bfe4d52…`" | `planning/update-scope.md` P7 | 실물 `120087 / 775a984c…` |

- 요구 수정: 각 소유 레인이 재측정 후 문장을 갱신한다. 특히 economy §6의 "G3는 PASS할 수 없다"는 **결론 자체가 부재 근거 위에 서 있으므로** 다시 판단해야 한다(결론은 유지될 수 있으나 근거는 바뀐다).

### C3-F16 · S3 · economy — [OBSERVED 문구] 인용이 live 데이터에 없다

- 근거 [OBSERVED, M3]: `economy/reward-bands.md` §1 채널 D 예시 `c3-b4` "**거래를 수락해도 같은 경로가 열린다**"와 `economy/sink-source-ledger.md` §3.3 동일 인용 — live JSON에 **0건**, archive c3에만 1건. live `c3-b4.consequence`는 "수락·거절 어느 쪽이든 경로는 열리며 청문 대조표의 각주 한 줄만 달라진다"다.
- 요구 수정: live 문구로 교체. 의미는 보존되므로 §1 채널 분류·§2 금지 목록 판정에는 영향이 없다.

### C3-F17 · S3 · synopsis — 필수 단서 K2가 어느 결말 요건에도 없다

- 근거: `synopsis/continuity.md` §5 K2(밸브 명령 대기 흔적) "필요한 결말: **A·B**". `synopsis/synopsis.md` §5는 A = K1·K3·K6·K7(+K4), B = K1·K4·K8·K9·K10, C = K1·K5 — **K2가 세 목록 어디에도 없다.**
- 요구 수정: 두 표 중 하나를 정정. 같은 레인·같은 회차의 두 `current` 문서 간 불일치이므로 RFC 불필요.

### C3-F18 · S3 · synopsis — 저장 표기가 표 A와 표 B에서 반대다

- 근거: `chapter-beats.md` 표 A는 33/33 "저장 = 예". 표 B는 B08 "`pump` P1(연습 시각화, **저장 안 됨**)", B21 "`dock` D1(연습, **저장 안 됨**)".
- [INFERENCE] 표 A는 체크포인트 유무, 표 B는 **연습 상태의 세이브 잔존 여부**를 말하는 것으로 읽힌다(`continuity.md` §1 "`*1`(대기) 상태는 세이브에 남지 않는다"와 정합). 두 열의 이름이 같아서 생기는 오독이다.
- 요구 수정: 표 A 열 이름을 "체크포인트"로, 표 B 문구를 "연습 상태는 세이브에 남지 않음"으로 명시.

### C3-F19 · S3 · synopsis/balance — `seal` 사용 횟수 표기 3종

- 근거: `chapter-beats.md` §3 차이 1 "seal **6회** 중 B16을 연습으로" — 표 A 실제 배정은 **7회**(확정 5: B04·B26·B29·B31·B33 / 연습 2: B16·B28). `content-matrix.md` §8-M6이 이미 "6 → 7 표기 정정 요청". live JSON `tools` 집계도 **7**, archive는 6.
- 요구 수정: `chapter-beats.md` §3 문장 정정. 자원 소모(확정 5)에는 영향 없다.

### C3-F20 · S3 · systems/balance — 힌트 자동 제안 모델이 두 개다

- 근거: `systems/system-specs/hint-system.md` §2 — 180초 무진전 → 비강제 토스트 1종, 무시 시 180초 쿨다운. `balance/balance-sheet.md` §6 — T1 180초 / T2 누적 420초 + 오확정 2회 / T3 누적 900초 + 오확정 4회 + 확인 클릭.
- 요구 수정: 임계는 balance 소유(`tunable: balance`)이므로 systems 상태기계를 3단 제안으로 확장하거나, balance 표를 "수동 열람 단계"와 "자동 제안 임계"로 분리 표기한다.

### C3-F21 · S3 · synopsis — 씬 문서가 기각 문구·보류 항목·이동된 구역을 그대로 쓴다

- 근거: `scenes-and-dialogue.md` S1 오브젝트 "**번호만 찍힌 염판 #0**" — `consistency-audit.md` §4가 서린 항목 재작성으로 **기각**한 표현이며, `continuity.md` §7은 OPEN-2 해소 전까지 "판 #0의 **호칭·대사 저작 보류**"를 지시한다. S2 헤더는 `c1-b4 · **gate**` — `chapter-beats.md` §4-1과 `content-matrix.md` §8-M1이 `hub`로 옮긴 비트다.
- 요구 수정: OPEN-2 판정 후 S1 재작성, S2 구역 표기를 `hub`로 정정.

### C3-F22 · S4 · planner — 비트 단위 `zoneId`가 데이터에 없다

- 근거 [OBSERVED]: live JSON 33/33 비트에 `zoneId` 부재, `zoneIds`는 스테이지 단위. 비트-구역의 유일 출처가 `content-matrix.md` §3 표다(같은 문서 §1이 자인). RFC-P3-004 / RFC-N1로 이미 열려 있다.

### C3-F23 · S4 · systems/economy — `maxUndo: 32` vs "되돌림 무제한"

- 근거: `systems/prototype/model.mjs:82` `maxUndo: 32`, `interaction-rules.md` §0.2·`gdd.md` §8·`business-model.md`는 "무제한 되돌림". RFC-E1로 열려 있으며 32단 초과가 진행을 막는 경로가 있는지 미탐색.

### C3-F24 · S4 · presentation (검토 범위 밖 관찰) — 덱이 기각 문구를 bible 출처로 표시한다

- 근거: `presentation/steam-game-plan.html` 슬라이드 8 "법 4 용량 — 이번 조수에 보호 용량이 부족하다", 출처 표기 `worldview/worldview-bible.md`. `generate-deck.mjs:549` 동일. C3-F3과 같은 뿌리이며 대외 슬라이드라 노출 위험이 더 크다.

## 3. 이전 C3 검토 5건의 이월 판정 [CARRIED]

| 이전 id | 원 주장 (요지) | 현재 판정 | 근거 |
|---|---|---|---|
| F1 | `c6-b3` 결론이 기록 불가능한 '서명 확인'에 걸림 | **부분 해소 / 재발** | 데이터는 '봉인 완료 접점'으로 옮겼으나 (a) 캐논 미반영 상태로 선반영(C3-F5) (b) 순서를 반대로 뒤집음 (c) `scenes-and-dialogue` S6 마지막 대사가 '누가 언제 확인했는지'로 원 결함을 복원(C3-F7) |
| F2 | 비퍼즐 12비트 162분의 액션이 전사(轉寫)뿐 | **형식 해소 / 수치 미해소** | `activityBudget`·`subtasks`가 33/33에 붙었고 비퍼즐 162 → 149분. 그러나 fast/deliberate 봉투 이탈은 그대로이고(322/673) 범주 오류 반론과 위험 유지 주장이 둘 다 살아 있다(C3-F14) |
| F3 | `c1-b4`가 단서 없이 '도연'을 호명 | **미해소 (방식 변경)** | 각서 근거를 붙여 정당화했으나 최초 등장이 `t0-b1`로 **더 앞당겨졌다**. current `continuity.md` §4는 B18을 강제한다(C3-F6) |
| F4 | `c3-b1` 회복 경로가 `c3-b2` 요건과 모순 | **해소** | live `c3-b1`이 대화 전 대장 판독으로 만조 피크 3개를 자동 사본에 넣고 우회로를 삭제. `recovery` 33/33 비어있지 않음 확인 |
| F5 | 판 #0이 461분 무기능 · 소유 서사가 바이블과 반대 | **해소 (부작용 발생)** | 소유를 서린으로 되돌리고 `plate-zero`가 6비트에서 실사용(`c1-b2 c1-b3 c4-b2 c4-b4 c6-b3 e0-b2`). 다만 `t0-b1` 노출로 상한을 넘었고(C3-F6) 호칭 보류 규칙을 어겼다(C3-F21) |

## 4. 요청된 항목별 결론 (요약)

| 요청 항목 | 결론 |
|---|---|
| C1 F1 해소 여부 | **문서 수준 해소.** `content-matrix.md`(5구역·22상태·33사건) + `campaign-time-budget.md` §3·§7(사건당 동사 실행 1.76회, 58 ≪ 160). 단 역산 입력이 아카이브 계보라 C3-F1에 종속 |
| C1 F2 (정가 표본 0) | **미해소·이관.** `planning/market-decision.md`는 `cycle: c2 / status: draft`로 멈춰 있고, 가격은 `product/business-model.md`(c5 draft)로 이동했다. C5 회귀 대상 |
| C1 F3 (가중치·총점) | **미해소.** `market-decision.md`(c2 draft)에 총점 열·민감도 표가 추가되지 않았다 |
| C1 F4 (설계/관측 키 분리) | **정의 수준 해소, 판정 수준 미해소.** `telemetry-contract.md` §1이 `design_*`/`observed_*`를 분리. 판정 키 미정(C3-F14), n=0 |
| 문서 간 숫자·용어 불일치 | C3-F1·F2·F4·F11·F13·F16·F19 |
| 비트 분량 합계 vs 480분 | 두 계보 모두 합 480, 스테이지별 합 9-of-9 일치 [OBSERVED]. **합계는 판별력이 없다**(C3-F1) |
| 33비트 자료 2종·동사·힌트·복구·저장 | C3-F11 표. 누락 = 매체 `c1-b4`, 동사 `t0-b1 c1-b4 c3-b4 c7-b1 e0-b1` |
| 연습/확정 분리 vs 6법 | 구조는 정합(bible §3-bis, audit A01~A03·A09). **차감 시점**에서 모순(C3-F9), 법 문구 유통에서 모순(C3-F3) |
| 결말 3갈래 필수 단서 보존 경로 | 표는 존재(continuity §5 K1~K10, 10/10 주장). **독립성 규칙 적용 시 불파괴 쌍이 무효화될 수 있다**(C3-F12). K2 귀속 불일치(C3-F17) |
| 전지적 화자 위반 | NPC 해설 비트 0건은 재확인. 위반은 **화자가 아니라 매체**에서 발생 — S6 대사(C3-F7), `t0-b1`·`c1-b4` 상한 초과(C3-F6) |
| 플레이타임 착시 | 480 = **행동 분**이며 벽시계 시간이 아니다(`campaign-time-budget.md` §0.1이 이동·대기·로딩을 0분 처리). 게임 내 8시간과 설계 480분이 같은 수인 것은 연출 의도(bible §3-bis.4 [INFERENCE]). **수용 조건은 벽시계 기반이라 두 수는 직접 비교되지 않는다**(C3-F14) |

## 5. C4 인계 우선순위

**C3-F1 → F5 → F6 → F3 → F4 → F2 → F7 → F9 → F12 → F13 → 나머지.**
F1이 닫히기 전에는 F2·F11·F19의 재집계가 두 번 일어난다. F5·F6·F7은 0분 수정이며 씬·UI 저작 전에 닫아야 한다. F3은 planner 문서 편집만으로 닫힌다. F4는 G2·G3 판정의 선행 조건이다.

## 6. 이 검토가 증명하지 않는 것 [OBSERVED]

문서 간 모순의 **부재**를 주장하지 않는다(대조는 전수가 아니다). 어떤 비트도 플레이되지 않았고, 도달 가능성·진행 막힘·힌트 실효성·완주 분포·결말 도달률·성능은 전부 **n = 0**이다. 게이트 값은 `qa/gate-measurements.md`에 있으며 그중 런타임 항목은 모두 `NOT-MEASURED`다.

---

# 7. 재검증 1 (2026-09-10) — C3 종료 수정 루프

## 7.0 이 절의 지위와 방법

- 입력: 1차 검토 결함 **24건** + `production/decision-log.md` **RFC-P3-008 ~ RFC-P3-015**(디렉터 판정, 구속력) + 7개 레인(worldview·planner·synopsis·systems·balance·economy·presentation)의 수정 보고.
- 방법: **레인 보고를 신뢰하지 않고 파일을 다시 열고 명령으로 재측정**했다. 아래 §7.1의 명령 8종이 이 절의 모든 수치를 만든다. 레인 보고와 실측이 어긋난 곳은 실측을 채택하고 결함으로 등록했다.
- 본 절도 **어떤 게이트도 PASS로 올리지 않는다.** 빌드 0줄 · 사람 플레이 n=0 · 시뮬 실행 0회는 이번 회차에도 바뀌지 않았다. 문서 정합만 판정한다.
- 쓰기는 `_workspace/current/qa/` 안에서만 했다. 다른 레인 파일은 **읽기만** 했다.

## 7.1 재측정 명령 원문 [OBSERVED 2026-09-10]

| # | 명령 | 결과 |
|---|---|---|
| R1 | `shasum -a 256 _workspace/current/planning/campaign.json` | `fdabf1d421932d679429870a06eeb31e9527562370c6a107aec77c2c884fb2b7` |
| R2 | `wc -c < _workspace/current/planning/campaign.json` | `120479` |
| R3 | `node _workspace/current/planning/validate-campaign.mjs` | `checks 44 / pass 44 / fail 0 / verdict PASS`, exit `0` |
| R4 | `bash .claude/skills/game-ops-harness/scripts/freshness-check.sh` | `0 finding(s) across 90 markdown artifact(s)`, exit `0` (원문 §7.5) |
| R5 | `grep -rnE "30[ ,/·]+50[ ,/·]+55[ ,/·]+60" _workspace/current` | 8행 — **전부 폐기 기록·역사 인용**, 집계 입력 0건 |
| R6 | `grep -rn "<기각 문구>" _workspace/current` (13종) | §7.4 표 |
| R7 | `grep -rln "archive/20260909-preproduction-c3/worldview" _workspace/current` | **8파일**(1차 검토 시점 0파일) |
| R8 | `python3` 로 `stages[].beats[].{clues,tools}` 최대치 집계 | `T0:2/2 C1:3/2 C2:3/2 C3:2/2 C4:2/2 C5:3/2 C6:4/3 C7:2/2 E0:2/1` |

### R3 집계 (검증기 `aggregates` 원문)

```
stageMinutes  25·50·55·65·65·70·75·65·10 = 480 · beats 33
kindCounts    puzzle 21 · dialogue 5 · payoff 5 · exploration 2
clues 73      sourceTypeDist  log 27 · ledger 22 · plate 24
originCatalogSize 31 · proofRequiredBeats 15 · mediaKindsPerBeatMin 2
activityBudget 탐색 53 · 추론 190 · 조작 168 · 대화 28 · 결과 41 = 480
fastMinutesSum 322 · deliberateMinutesSum 673
toolBeatCounts circuit 10 · reader 11 · alignment 8 · routing 3 · corrosion 3 · seal 7
toollessBeats  t0-b1 · c1-b4 · c3-b4 · c7-b1 · e0-b1  (5건)
timeConfidence low 12 · medium 21 · high 0
```

> **정본 해시 갱신 [OBSERVED]**: 지시서와 RFC-P3-008 본문이 인용한 `775a984c…`(120087 B)는 planner가 C3-F11을 수리하기 **전** 값이다. 현재 실물은 `fdabf1d4…`(120479 B)이고 계보는 동일(스테이지 분 9값 불변), `clue`만 72 → **73**(`plate` 23 → **24**)으로 변했다. 6개 레인이 독립적으로 같은 값을 보고했고 R1·R3 두 명령이 일치한다. **본 절은 실측값을 채택한다.** RFC 본문의 해시·단서 수 갱신은 디렉터 소관이다(§7.7 RFC-Q1).

## 7.2 결함별 재판정 — 1차 24건

| id | S | 레인 | 재측정 결과 [OBSERVED] | 판정 |
|---|---|---|---|---|
| C3-F1 | S1 | planner/synopsis/systems/balance/economy | R3 `S-02` PASS. R5 = 폐기 계보 문자열 8행 전부 "폐기됨/역사 인용"으로 명시된 기록이며 집계 입력 0건. `gdd.md` §10·`content-matrix.md` §3·`campaign-time-budget.md` 표1·`chapter-beats.md` 표 A·`telemetry-contract.md` §1 전부 live 값 | **closed** (RFC-P3-008) |
| C3-F2 | S1 | systems/balance | `grep -rn "2bfe4d52"` = 17행이나 **전부 "저장소에 없는 해시" 폐기 기록**이며 [OBSERVED] 측정 주장 0건. 전체 64자리 `fdabf1d4…`를 인용하는 문서 **31파일** 확인 | **closed** |
| C3-F3 | S1 | planner→**systems** | planner: gdd §4·feature-specs 6종 = 정본 6/6 문자 일치 [OBSERVED]. presentation: 덱이 bible §3을 빌드 시점 파싱. **그러나** `systems/system-specs/{plate-readout,tide-alignment,drainage-routing,corrosion-budget,dual-seal}.md` **각 9행 제목**이 폐기된 C2 문구를 그대로 쓴다 — "판은 재생할수록 닳는다"·"시계는 조수에 매인다"·"물은 한 번에 한 곳으로만 간다"·"소금은 모든 것을 먹는다"·"당직은 하나, 서명은 둘" | **open** (소유 이동: planner → systems) |
| C3-F4 | S1 | balance/economy | 부식 정본 **6곳 전건 일치** = 전역 상한 9: `balance-sheet.md` L167·L176 / `currency-map.md` L74·L79 / `corrosion-budget.md` L20·L54·**L89 C-R2** / `worldview-bible.md` L69 / `glossary.md` L93 / `gdd.md` L92 | **closed** (RFC-P3-009) |
| C3-F5 | S1 | planner/synopsis | R3 `K-03` PASS(H-1:24·H-1:04·H+0:12 존재) · `K-04` PASS(H-1:20·H+0:10 부재). `timeline.md` 본문 금지 문구 0건, L50에 폐기 기록만. `chapter-beats.md` B26에 순서 기술 | **closed** (RFC-P3-013). 잔여 = 신규 C3-F25·F26 |
| C3-F6 | S1 | planner/synopsis | R3 `K-05` PASS(도연 = `t0-b1`, 한서린 = `c4-b2`). `continuity.md` L114 "최초 등장 B18 규칙 폐기" · L118·L119 재작성. `timeline.md` §7 B01 허용 열에 인수 각서·판 #0 명시 | **closed** (RFC-P3-012) |
| C3-F7 | S1 | synopsis | `scenes-and-dialogue.md` L137 = "기계가 찍은 두 각인 중 밸브 쪽이 봉인 완료 접점보다 이십 분 먼저 찍혔다는 것. 그 두 줄만 남았습니다." `grep "누가 언제 확인"` → 본문 0건. R3 `K-01` PASS | **closed** |
| C3-F8 | S2 | worldview/director | frontmatter 실측: `worldview-bible.md`·`timeline.md` `supersedes: _workspace/archive/20260909-preproduction-c3/worldview/<file>.md`. R7 = 8파일. 아카이브 c3 worldview 실체는 `timeline.md`·`worldview-bible.md` 2종뿐이므로 `glossary.md`가 c2를 잇는 것은 **정상**(c3 대응본 없음) | **closed** (RFC-P3-010) |
| C3-F9 | S2 | worldview/balance/systems | 소모 개념 자체가 폐기돼 "차감 시점" 충돌이 성립하지 않는다. `corrosion-budget.md` L82 "상태기계에 없는 것: 잔량 변수, 차감 전이, 리셋 전이" · `chapter-beats.md` L36에서 6계통 소모 열 삭제 확인 | **closed** (RFC-P3-009) |
| C3-F10 | S2 | planner/systems | `gdd.md` L72·L148·L149 = `two-step` 기본 / `hold` opt-in. `grep "길게 누름을 요구\|길게 누름 필수"` 0건 | **closed** (RFC-P3-015). 부가 지적(current→draft 인용)은 신규 C3-F33 |
| C3-F11 | S2 | planner | R3 `C-06` PASS(`mediaKindsPerBeatMin` = 2). `content-matrix.md` L152·L163·L165가 "단일 매체 0건"·"도구 미사용 0건"이 거짓이었음을 명시하고 실측 5건으로 정정 | **closed** |
| C3-F12 | S2 | synopsis/systems | R3 `C-07` PASS(proofRequired 15건 전건 독립쌍). `continuity.md` §5 K1~K10 재작성 확인 — K행 10개 전부 루트 originId·sourceType이 다른 쌍이며 "접수부 확정 사본" 조합은 L153 오류 기록에만 남음. `interaction-rules.md` §3에 "확정 사본은 새 루트를 갖지 않는다" 명문화 | **closed** |
| C3-F13 | S2 | worldview | 지목 명사 14종 전건 `grep -c "^| <term> "` = **1** (가설판·증거함·사건판·자동 사본·8분 오차폭·봉인 완료 접점·인수 각서·이관 목록·근무표·당직 자격 명부·소금 그늘·계통판·결번·상시 슬롯) | **closed** |
| C3-F14 | S2 | director/qa | 계약 `## Time acceptance` 개정 확인. `telemetry-contract.md` L40·L55·L59·L63·L69·L70·L74 = 판정 키 `total_minus_afk_min` 단일, 목표 450~540, 420/360 철회 트리거, p75 조건 삭제, AF7·AF8 신설 | **closed** (RFC-P3-011) |
| C3-F15 | S2 | systems/balance/planner/economy | `update-scope.md` P3·P7 재측정 반영. `balance-sheet.md` §2 미러 재작성. `sink-source-ledger.md` §6이 부재 근거를 B1(자원 키 0건)·B2(n=0)·B3(INV10/11 미구현)로 교체 | **closed** |
| C3-F16 | S3 | economy | `reward-bands.md` L31 · `sink-source-ledger.md` L94 = live 원문 "수락·거절 어느 쪽이든 경로는 열리며 청문 대조표의 각주 한 줄만 달라진다". 폐기 문구는 L100·L171 검증 기록에만 | **closed** |
| C3-F17 | S3 | synopsis | `synopsis.md` L74·L75에 K2가 A·B 인용 목록에 등재. L78에서 도달 요건(선형 사슬 공통)과 인용 요건을 분리 | **closed** |
| C3-F18 | S3 | synopsis | `chapter-beats.md` L37·L40 = 표 A 열 이름 **"체크포인트"**로 변경 + 정의 기재 | **closed** |
| C3-F19 | S3 | synopsis/balance | `chapter-beats.md` L27·L151 = seal **7**. R3 `toolBeatCounts.seal` = 7 일치 | **closed** |
| C3-F20 | S3 | systems/balance | `hint-system.md`·`balance-sheet.md` §6 모두 180초 단일 제안 + 180초 쿨다운. 3단 자동 승격 모델은 balance §11 폐기 원장으로 이동 | **closed** (RFC-P3-015) |
| C3-F21 | S3 | synopsis | S1 오브젝트 = "번호만 적힌 미봉인 염판 #0"(glossary §2 정본). S2 헤더 = `c1-b4 · hub` | **closed**. 잔여 구역 텍스트 불일치는 신규 C3-F29 |
| C3-F22 | S4 | planner | `grep -c '"zoneId"' campaign.json` = **0**. `content-matrix.md` L81·L84가 "비트-구역 매핑의 단일 출처는 본 절의 표"로 선언하고 스키마 확장을 C4 RFC로 이월 | **open-rfc** (RFC-P3-004 / RFC-N1) |
| C3-F23 | S4 | systems/economy | `prototype.meta.md` L120·L127·L131·L133 = `maxUndo 32`는 전수 탐색기 이력 링 길이이며 `pushPast()`가 되돌림을 거부하지 않음을 코드 인용으로 규정. 정본 = 무제한 | **closed** (RFC-P3-015) |
| C3-F24 | S4 | presentation | `steam-game-plan.html` 정본 문구 "이번 조수에는 보호 용량이 부족하다" **1건**, 기각 문구 "이번 조수에 보호 용량이 부족하다" **0건**. `generate-deck.mjs` L1788에 재발 방지 게이트 | **closed** (RFC-P3-014) |

## 7.3 신규 결함 — C3-F25 ~ C3-F33

### C3-F25 · S2 · worldview/planner/director — `H-1:40`의 지위가 RFC 본문과 캐논에서 반대다 · open-rfc

- 근거 [OBSERVED]: `production/decision-log.md` L55 RFC-P3-013 decision — "**H-1:40**/H-1:20/H+0:10은 폐기." 그러나 `worldview/timeline.md` L43이 `H-1:40`을 **현행 캐논**(도연이 두 번째 서명란에 서린의 이름을 적음)으로 유지하고, live `campaign.json`에 `H-1:40` **3건**, `synopsis/continuity.md` L219가 B17을 "H-1:40 부근 좌표화"로 쓴다.
- 검증기 `K-04`는 `H-1:20`·`H+0:10`만 검사하므로 이 모순을 잡지 못한다 [OBSERVED `validate-campaign.mjs` L42 `canonTimesAbsent`].
- [INFERENCE] RFC 본문이 세 시각을 한 묶음으로 적었으나 실제 판정 대상은 **밸브·침수 두 시각**이고 서명 시각 H-1:40은 유지가 의도였을 가능성이 높다. 그러나 구속력 있는 판정문이 폐기라고 적은 이상 **문서와 데이터가 판정문을 위반한 상태**다.
- 요구: 디렉터가 RFC-P3-013 본문을 (a) "H-1:20·H+0:10 폐기, H-1:40 유지"로 정정하거나 (b) H-1:40 폐기를 관철한다. (b)면 timeline §2·live 3건·continuity B17이 동시에 움직인다.

### C3-F26 · S2 · systems — 폐기된 시각 `H-1:20`을 규칙표가 캐논 사례로 [OBSERVED] 인용한다 · open

- 근거 [OBSERVED]: `systems/system-specs/tide-alignment.md` L54 — `| A-R3 | 캐논 사례: 서명(H-1:40) → 밸브(H-1:20) 간격 20분, 정합 후 오차폭 합 4+4=8분 → 20 > 8 이므로 확정 가능 [OBSERVED: timeline.md 정합 오차 규칙] |`
- 이것은 폐기 기록이 아니라 **현행 규칙표 §3의 행**이며 `[OBSERVED]` 표기까지 붙어 있다. 캐논은 밸브 = `H-1:24`(RFC-P3-013)이고 `timeline.md` L50이 `H-1:20`을 폐기로 적는다. 같은 회차의 두 `status: current` 문서가 반대를 말한다.
- 요구: A-R3을 `서명(H-1:40) → 밸브(H-1:24) 간격 16분` 또는 캐논이 확정한 `밸브(H-1:24) → 봉인 완료 접점(H-1:04) 간격 20분`으로 교체. 간격이 20 → 16으로 바뀌면 `20 > 8` 판정 예시의 숫자도 재작성 대상이다. C3-F25 (a)/(b) 판정에 종속된다.

### C3-F27 · S2 · balance/economy — systems 재작성본을 반영하지 않은 교차 레인 스테일 `[OBSERVED]` 3건 · open

같은 세션에서 systems가 먼저 고친 내용을 balance·economy가 옛 상태로 인용한다. 세 건 모두 `status: current` 문서의 `[OBSERVED]` 주장이다.

| # | 주장 위치 | 주장 내용 | 실측 [OBSERVED 2026-09-10] |
|---|---|---|---|
| a | `balance/balance-sheet.md` L223 · L480 Q2 · `economy/currency-map.md` L96 · Q6 · `negotiation-record.md` N-14 | `systems/system-specs/corrosion-budget.md` C-R2가 "한도는 계통별(`systemLimits`)"이라 RFC-P3-009와 어긋난다 | `corrosion-budget.md` **L89 C-R2 = "한도는 전역 단일 `corrosionLimit = 9`이며 [tunable: economy]. 계통별 한도는 존재하지 않는다"** · L67 "`systemLimits[systemId]`는 상태 변수가 아니다" → **이미 정정됨** |
| b | `balance/balance-sheet.md` L223 · L480 Q2 | `zones.md` L42 `systemLimits`가 차단형 계통 한도를 전제한다 | `zones.md` **L42 = "표시 분해 전용(법5) … 확정 가능성을 바꾸지 않는다"** · L90 Z-I6 재정의 → **이미 정정됨** |
| c | `economy/currency-map.md` L86 · L91 · L177 · L185 | `save.md:55` `operationalCorrosion: map<systemId,float>`가 **v1에 존재**하며 계통별 표시 누계를 담는다 (`[OBSERVED, 2026-09-10 재측정]` 표기) | `save.md` **L94 "### `operationalCorrosion` 제거 [RFC-P3-009 · 2026-09-10]"** — 필드가 스키마에서 삭제됐다. economy §4.1 계통별 표시 모델이 **없는 필드 위에 서 있다** |
- (a)(b)는 문장만 정정하면 닫힌다. **(c)는 모델 충돌**이다 — economy가 계통별 표시 누계를 유지하려면 저장 자리가 필요하고, systems는 "저장하면 폐기된 누적 모델이 데이터 층에서 되살아난다"는 이유로 지웠다. 둘 중 하나가 움직여야 한다.
- 부가 [OBSERVED]: economy가 미이행으로 보고한 `telemetry-contract.md` §1.2도 실측은 **이미 RFC-P3-011로 재작성돼 있다**(L55·L63·L69). 같은 유형의 스테일이다.

### C3-F28 · S2 · worldview — 용어집·감사의 집계 영수증이 폐기된 판을 가리킨다 · open

- 근거 [OBSERVED]: `worldview/glossary.md` L138 "31종 / 단서 **72**건 / `log 27 · ledger 22 · plate **23**`", L12가 sha `775a984c…`를 입력으로 표기. `worldview/consistency-audit.md` L79 "단서 **72** / 출처 31종 / `log 27 · ledger 22 · plate **23**`".
- 실측(R1·R3): 단서 **73** / `plate` **24** / sha `fdabf1d4…`. 출처 31종은 불변이므로 **수록 명사에는 영향이 없다**(C3-F13 판정은 유지된다). 틀린 것은 영수증 숫자와 입력 해시다.
- `planning/campaign.meta.md` L729가 이미 이 갱신을 worldview에 브로드캐스트했다.
- 요구: 두 곳의 집계 3값과 해시를 R1·R3 값으로 교체.

### C3-F29 · S2 · planner/synopsis — 비트-구역 배정이 세 곳에서 갈린다 · open

`content-matrix.md` §3은 스스로 "비트-구역 매핑의 **단일 출처**"라고 선언한다(L81·L84). 그 단일 출처가 데이터 본문·시놉시스와 반대다.

| 비트 | live `subtasks[0]` 본문 [OBSERVED] | `content-matrix.md` §3 | `synopsis` (chapter-beats·continuity) |
|---|---|---|---|
| `c4-b1` | "**저지대 주민회 사무실**에서…" → `lowland` | **`hub` H1** (L106) | `lowland` (live 채택) |
| `c4-b3` | "**당직실 봉인대**에서…" → `hub` | **`lowland` L0** (L108) | `hub` (live 채택) |
| `c1-b4` | "**제3수문**에서…" → `gate` (L426) | `hub` | `hub` (S2 헤더도 `hub`) |
- 즉 `c4-b1`↔`c4-b3`은 단일 출처 표와 데이터가 **서로 뒤바뀐** 상태이고, `c1-b4`는 데이터 본문만 `gate`를 말한다. 스테이지 `zoneIds`는 두 구역을 모두 포함하므로(`C4 = [lowland, hub]`, `C1 = [hub, gate]`) 검증기가 잡지 못한다.
- 세 건 모두 0분 수정이며 소유가 planner(데이터·표)와 synopsis(씬)에 걸쳐 있다. C3-F22(비트 단위 `zoneId` 부재)가 열려 있는 한 이런 불일치를 기계로 막을 수단이 없다 — 두 결함은 같은 뿌리다.

### C3-F30 · S3 · planner/synopsis/worldview — 비트 번호(B#) 부여 체계가 두 벌이다 · open-rfc

- 근거 [OBSERVED]: `planning/content-matrix.md` §3·`synopsis/chapter-beats.md` 표 A = `c1-b3`→B04 · `c4-b1`→B17 · `c4-b3`→B16. `worldview/timeline.md` §7(live 재도출본) = `c1-b3`→B06 · `c4-b1`→B16 · `c4-b2`→B17 · `c4-b3`→B18(live 순차 부여). T0·C6·C7·E0에서만 두 체계가 일치한다.
- 결과: "B17"이 문서에 따라 다른 비트를 가리킨다. 본 검토 §7.2의 C3-F5 행이 `chapter-beats` 체계를, C3-F25가 `continuity` 체계를 인용하는 것도 같은 이유로 위험하다.
- 완화책은 이미 있다 — `chapter-beats.md` §0이 "상호 참조는 campaign id로만"을 명시한다. 그러나 세 문서가 여전히 B#을 본문에 쓴다.
- 요구: 디렉터가 한 체계를 캐논으로 지정하거나, B# 사용을 각 문서 내부 참조로 한정하고 문서 간 인용은 `campaign id`로 강제한다.

### C3-F31 · S2 · balance/planner — 난이도 상승 폭 규칙 위반이 **[INFERENCE] 열 하나에 전적으로 걸려 있다** · open-rfc

- balance 자체 보고: 난이도 지수 `[5,7,7,7,7,8,11,8,4]`, C5→C6 = **+3** > 규칙 ≤2 → G2 FIX 미해소.
- QA 재측정 [OBSERVED, R8]: 지수 3열 중 **2열은 재현된다** — `대조 수`(스테이지 내 최대 단서)·`도구 조합 수`(최대 도구)가 `T0 2/2 · C1 3/2 · C2 3/2 · C3 2/2 · C4 2/2 · C5 3/2 · C6 4/3 · C7 2/2 · E0 2/1`로 balance 표와 전건 일치한다.
- **그러나 세 번째 열 `동시 가설 수`는 `[INFERENCE]`이며 balance 스스로 "이번 회차에 재측정하지 않았다"고 적는다(`balance-sheet.md` §7 각주).** 그 열을 빼고 관측 2열만 더하면 지수는 `[4,5,5,4,4,5,7,4,3]`이고 **C5→C6 = +2로 규칙을 통과한다** [OBSERVED, 본 절 산출].
- 즉 위반 판정의 부호가 **측정되지 않은 열 하나**로 뒤집힌다. 그 열을 근거로 planner에게 `campaign.json` 재저작(A안)을 요구하는 것은 `[INFERENCE]`를 `[OBSERVED]`처럼 쓰는 것이다. 반대로 규칙을 ≤3으로 완화하는 것(B안)은 결과에 맞춰 밴드를 넓히는 처리라 금지된다(CLAUDE.md §6).
- 요구: (1) balance가 `동시 가설 수`의 산출 규칙을 재현 가능한 명령으로 정의하거나 그 열을 지수에서 분리한다. (2) 그 전까지 G2의 `difficulty_monotonicity`는 `DOC_ONLY · DISPUTED`이며 A안·B안 어느 쪽도 실행하지 않는다.

### C3-F32 · S3 · qa — 1차 검토 §1.1의 `sourceType` 분포가 산술로 성립하지 않았다 · closed (본 절 자기정정)

- 근거 [OBSERVED]: 1차 §1.1 live 열이 `log 26 · ledger 22 · plate 21`을 적었다. **합이 69**인데 같은 열의 `clue 수`는 **72**다. 내부 모순이다.
- 정정: 수정 전(`775a984c…`) 실제 분포는 `log 27 · ledger 22 · plate 23 = 72`다. 근거 셋이 일치한다 — (1) 현재 실측 `log 27 · ledger 22 · plate 24 = 73`(R3), (2) planner가 추가한 단서는 `c1-b4-c3` **`plate` 1건뿐**(`campaign.meta.md` §10), (3) `worldview/glossary.md` L138·`consistency-audit.md` L79의 독립 집계가 `27 / 22 / 23`.
- 파생: `defect-register.md` C3-F2 행의 "염판 21"과 §3 브로드캐스트의 "재집계(염판 21…)"를 **염판 23(수정 전) → 24(현재)**로 정정했다. `balance/balance-sheet.md` §10.2 **Q7**과 planner의 counter는 이 정정으로 **해소**된다 — balance가 채택한 24가 옳다.
- 이 오기는 1차 검토의 `[OBSERVED]` 라벨이 붙은 표에서 발생했다. 재발 방지: 분포 집계는 합계와 대조하고 검증기 `aggregates` 출력을 그대로 붙인다(본 절 §7.1 R3).

### C3-F33 · S3 · systems/planner/director — `status: current` 문서가 `status: draft` 문서를 정본으로 인용한다 · open-rfc

- 근거 [OBSERVED]: `systems/interaction-rules.md` = `cycle: c5 / status: draft`인데 `planning/gdd.md`(current)·`systems/data-schemas/*.md`(current)·`synopsis/continuity.md`(current)가 입력 방식·자료 독립성의 **정본**으로 인용한다. `balance/puzzle-balance.md`(c4 draft)를 gdd §6이 인용하는 것도 동일.
- 1차 C3-F10의 "부가" 지적이었고 RFC-P3-015는 `interaction-rules.md` §1-1을 **정본으로 지정**했으나 문서의 `status`는 draft 그대로다. 즉 판정문이 draft 문서를 정본으로 삼은 상태가 명시적으로 만들어졌다.
- `freshness-check.sh`는 인용 방향을 검사하지 않는다 [OBSERVED §7.5].
- 요구: 디렉터가 (a) C5에서 승격, (b) 인용 측에 "잠정 인용" 표기 의무화 중 하나를 지정한다. systems 레인이 임의 승격하지 않은 판단은 계약에 맞다.

## 7.4 기각·폐기 문구 잔존 검사 [OBSERVED, R6]

`grep -rn` 전수. **"본문 사용"** = 현행 규칙·정의·대사로 쓰인 것, **"폐기 기록"** = "폐기됐다/기각됐다/쓰지 않는다"로 감싼 역사 보존.

| 문구 | 총 히트 | 본문 사용 | 판정 |
|---|---|---|---|
| `서명 확인 전` | 7 | **0** | 검증기 금지 문자열·campaign.meta 변경 기록·1·2차 검토·audit §4 | PASS |
| `봉인 호출` | 6 | **0** | 위와 동일 + `model.mjs:58` 퍼즐 쌍 라벨 `봉인 호출 ↔ 밸브 대기`(도메인 명사, 캐논 명제 아님) | PASS |
| `밸브 명령 대기 흔적` | 6 | **0** | 검증기 금지 문자열·검토·audit | PASS |
| `계통 영구 고장` | 13 | **0** | 전부 "없다/폐기"의 목적어로만 등장 | PASS |
| `4회째 결정 붕괴` | 1 | **0** | audit §4 폐기 원장 1행 | PASS |
| `H-1:20` | 17 | **1** | **`systems/system-specs/tide-alignment.md` L54 A-R3** → C3-F26 | **FAIL** |
| `H+0:10` | 19 | **0** | 폐기 기록·검증기 | PASS |
| C2 법 문구 5종 | 17 | **5** | `system-specs` 5파일 각 9행 제목 → C3-F3 | **FAIL** |
| `이번 조수에 보호 용량`(기각형) | 7 | **0** | 덱 재발 방지 게이트·검토·meta | PASS |
| `2bfe4d52…` | 17 | **0** | 전부 "저장소에 없는 해시" 폐기 기록 | PASS |
| `거래를 수락해도 같은 경로가 열린다` | 5 | **0** | 검증 명령·수정 기록 | PASS |
| `번호만 찍힌 염판` | 3 | **0** | 검토·수정 기록 | PASS |
| 아카이브 계보 `30·50·55·60…` | 8 | **0** | 전부 "폐기·역사 인용 전용" 표기 | PASS |

## 7.5 신선도 · supersedes 토폴로지 [OBSERVED, R4·R7]

```
freshness: 0 finding(s) across 90 markdown artifact(s) under _workspace/current
freshness: scope = frontmatter contract + supersedes topology only; memory_sync receipts (mex/llm-wiki/graphify/zg) are NOT verified here
freshness: no cycle start supplied; staleness not measured
EXIT_CODE=0
```

| 항목 | 1차(2026-09-09) | 재검증 1(2026-09-10) |
|---|---|---|
| 스캔 산출물 | 82 | **90** |
| finding / exit | 0 / 0 | 0 / 0 |
| 아카이브 c3 worldview 고아 | **2건** | **0건** — `worldview-bible.md`·`timeline.md`가 아카이브 c3를 `supersedes` |
| 아카이브 c3 worldview 인용 파일 | 0 | **8** |

**스크립트가 여전히 잡지 못하는 것** (범위 밖, 본 절이 손으로 잰다): ① 인용 방향(current → draft, C3-F33) ② 문서 간 집계 영수증 대조(C3-F27·F28) ③ JSON은 frontmatter가 없어 90건에 포함되지 않음 ④ 같은 개념의 서로 다른 번호 체계(C3-F30) ⑤ memory_sync 영수증(스크립트 자체 선언, 본 회차는 `[SKIPPED: mex 금지]`).

## 7.6 집계와 최종 판정

| severity | 1차 | 신규 | 합 | closed | open | open-rfc |
|---|---|---|---|---|---|---|
| S1 | 7 | 0 | 7 | 6 | **1** (F3) | 0 |
| S2 | 8 | 6 | 14 | 8 | 4 (F26·F27·F28·F29) | 2 (F25·F31) |
| S3 | 6 | 3 | 9 | 7 | 0 | 2 (F30·F33) |
| S4 | 3 | 0 | 3 | 2 | 0 | 1 (F22) |
| **합계** | **24** | **9** | **33** | **23** | **5** | **5** |

### 최종 판정: **SPEC-FIX**

- 1차 24건 중 **22건이 실측으로 닫혔다**(F3·F22 제외). 디렉터 RFC 8건이 정본을 확정했고 레인들이 그 위에서 재도출한 것이 실제로 파일에 반영됐음을 명령으로 확인했다. **1차 검토의 핵심 차단 사유(계보 2벌·부재 해시·정본 2개·캐논 역전·공개 상한·법1 위반)는 전부 해소됐다.**
- **그러나 S1 C3-F3이 열려 있다.** RFC-P3-014가 정본을 뒤집은 뒤 `systems/system-specs` 5파일이 반대 방향으로 남았다 — 결함의 소유가 planner에서 systems로 이동했을 뿐 "같은 법을 두 레인이 다르게 호명한다"는 사실은 그대로다. **열린 S1 1건이 있는 한 어떤 게이트도 PASS 불가**(CLAUDE.md §6).
- 신규 9건은 **전부 이번 회차의 대규모 동시 수정이 만든 2차 효과**다. 유형이 셋뿐이다 — (i) 판정문과 캐논의 문언 불일치(F25·F26), (ii) 같은 세션 병렬 편집이 만든 교차 레인 스테일(F27·F28), (iii) 기계가 검사하지 않는 축의 표류(F29 구역·F30 번호·F33 인용 방향). 셋 다 **검사기를 늘리면 재발을 막을 수 있다**.
- C3 닫는 질문 "33비트가 480분 설계 예산과 행동 예산으로 역산되는가"에 대한 재답: **예, 문서 수준에서.** 역산 대상이 하나로 확정됐고(`fdabf1d4…`), 44검사가 재실행 가능하며, `activityBudget` 5범주 합이 비트별·전체 모두 일치한다(`T-01`·`T-02` PASS). **다만 480은 설계 분이지 플레이 시간이 아니며 사람 표본은 여전히 n=0이다.** 표를 더한 것은 8시간을 플레이한 증거가 아니다.

### C4 인계 우선순위

**C3-F3 → F27(c) → F25 → F26 → F29 → F28 → F31 → F33 → F30 → F22.**
F3는 systems 문서 5줄 수정으로 닫히고 그것이 유일한 S1이다. F27(c)는 스키마 충돌이라 문장 수정으로 닫히지 않는다. F25는 디렉터 한 줄 판정이며 F26이 거기 종속된다.

## 7.7 브로드캐스트 (dependency-matrix ● 항목)

`feedback-requested-by: 2026-09-11`

| 받는 레인 | 결함 | 요청 |
|---|---|---|
| game-production-director | F25, F30, F31, F33, RFC-Q1 | H-1:40 지위 판정 · B# 체계 지정 · 난이도 지수 처리 · draft 인용 정책 · RFC-P3-008 본문의 해시(`fdabf1d4…`)·단서 수(73) 갱신 |
| game-systems-designer | **F3**, F26, F27(c) | 스펙 5파일 제목의 법 문구를 bible §3 정본으로 교체 · A-R3의 `H-1:20` 제거 · `operationalCorrosion` 제거를 economy와 합의 |
| game-balance-designer | F27(a)(b), F31 | `corrosion-budget.md` C-R2·`zones.md` L42에 대한 스테일 주장 철회 · `동시 가설 수` 산출 규칙 정의 또는 지수 분리 |
| game-economy-designer | F27(c) | `currency-map.md` §4.1 계통별 표시 모델의 저장 근거 재작성(필드가 삭제됐다) · telemetry §1.2 미이행 주장 철회 |
| game-worldview-architect | F25, F28 | 용어집·감사 집계를 `73 / plate 24 / fdabf1d4…`로 갱신 · H-1:40 판정 대기 |
| game-planner | F29, F22, F30 | `content-matrix.md` §3의 `c4-b1`↔`c4-b3` 구역 정정 · `c1-b4.subtasks[0]` "제3수문" 정정 · 비트 `zoneId` RFC |
| game-synopsis-writer | F29, F30 | 씬·표의 구역 표기를 데이터와 맞춤 · B# 사용 범위 축소 |

**RFC-Q1 (qa → director)**: RFC-P3-008 본문이 정본으로 지정한 sha `775a984c…`·`clue 72`는 판정 시점 값이며 현재 실물은 `fdabf1d4…`·`clue 73`이다(R1·R3, 6개 레인 독립 확인). 계보 판정 자체는 유효하나(스테이지 분 9값 불변), 본문 숫자를 갱신하지 않으면 다음 세션이 같은 계보 검증에서 다시 막힌다.

## 7.8 이 재검증이 증명하지 않는 것 [OBSERVED]

- 문서 간 모순의 **부재**를 주장하지 않는다. §7.4의 grep은 13개 문자열의 전수이지 의미의 전수가 아니다.
- **어떤 비트도 플레이되지 않았다.** 도달 가능성·진행 막힘·힌트 실효성·완주 분포·결말 도달률·성능은 전부 n = 0이며, 이번 회차에 새로 측정된 런타임 값은 **0건**이다.
- 레인들이 "재도출했다"고 보고한 표 중 본 절이 원소 단위로 재계산한 것은 §7.1 R3·R8의 집계와 §7.4의 문자열뿐이다. 33행 표 A·표 B의 서사 내용, K1~K10 쌍의 서사적 타당성, 33슬롯 공개 상한의 서사 정합은 **구조만 보고 내용은 표본 검사했다**.
- `qa/exploit-register.md`·`regression-matrix.md`·`playtest-report.md`·`immersion-scores.md`는 여전히 미작성이다. 빌드와 표본이 없어 작성하면 측정 위장이 된다(1차 §4 판정 유지).

---

# 8. 재검증 2 (2026-09-10) — C3 종료 수정 루프 2

## 8.0 이 절의 지위와 방법

- 입력: 재검증 1이 남긴 **열린 S1 1건(C3-F3)** · **open S2 4건 중 2건(C3-F26 · C3-F28)** + systems·worldview 두 레인의 수정 보고 + 디렉터 판정 **RFC-P3-008 ~ RFC-P3-015**(구속력).
- 방법: **레인 보고를 신뢰하지 않고 파일을 다시 열고 명령으로 재측정**했다. §8.1의 명령 8종이 이 절의 모든 수치를 만든다. 레인이 "닫았다"고 보고한 것도 grep·해시·집계로 직접 확인한 뒤에만 `closed`로 적었다.
- 레인 보고가 인용한 근거 파일(`worldview-bible.md` §3 표, `timeline.md` §2·§8, `campaign.json`)을 **원문까지 따라가** 대조했다. 레인이 인용한 줄이 실제로 그 주장을 지지하는지까지 확인한 것은 이번 회차가 처음이다.
- 본 절도 **어떤 게이트도 PASS로 올리지 않는다.** 빌드 0줄 · 사람 플레이 n=0 · 시뮬 실행 0회 · 성능 캡처 0건은 이번 회차에도 그대로다. 새로 측정된 런타임 값은 **0건**이다.
- 쓰기는 `_workspace/current/qa/` 안에서만 했다. 다른 레인 파일은 **읽기만** 했다.

## 8.1 재측정 명령 원문 [OBSERVED 2026-09-10]

| # | 명령 | 결과 |
|---|---|---|
| S1 | `shasum -a 256 _workspace/current/planning/campaign.json` | `fdabf1d421932d679429870a06eeb31e9527562370c6a107aec77c2c884fb2b7` |
| S2 | `wc -c < _workspace/current/planning/campaign.json` | `120479` |
| S3 | `node _workspace/current/planning/validate-campaign.mjs` | `summary {checks 44, pass 44, fail 0, verdict PASS}`, exit `0` |
| S4 | `bash .claude/skills/game-ops-harness/scripts/freshness-check.sh` | `0 finding(s) across 91 markdown artifact(s)`, exit `0` (재검증 1 = 90) |
| S5 | `grep -rlE "판은 재생할수록 닳는다\|시계는 조수에 매인다\|물은 한 번에 한 곳으로만 간다\|소금은 모든 것을 먹는다\|당직은 하나, 서명은 둘" _workspace/current` 후 파일별 `grep -c` | **4파일 / 13행** — `planning/gdd.md` 1 · `qa/c3-review.md` 6 · `qa/gate-measurements.md` 1 · `worldview/consistency-audit.md` 5. **`_workspace/current/systems` = 0파일** |
| S6 | `grep -rc "H-1:20" _workspace/current` | **11파일 / 31행** — 전부 검증기 문자열·폐기 기록·검토 인용. `systems` 레인 **0**, `tide-alignment.md` **0** |
| S7 | `python3` 로 `stages[].beats[].clues[].{originId,sourceType}` 독립 집계 | 단서 **73** · `log 27 · ledger 22 · plate 24`(합 73) · `originId` 고유 **31** · 스테이지 분 `25·50·55·65·65·70·75·65·10 = 480` · 비트 33 |
| S8 | `grep -rn "775a984c" _workspace/current` | **33행** — `worldview` 레인의 입력 주장 **0건**(전부 "수리 전 값·폐기" 문맥). 남은 현행 주장은 `production/decision-log.md` L23 RFC-P3-008 본문 1건뿐 → RFC-Q1 유지 |

> S7은 레인이 제출한 숫자를 옮긴 것이 아니라 **QA가 직접 실행한 집계**이며, S3의 검증기 `aggregates`(`clues 73` · `sourceTypeDist {log:27, ledger:22, plate:24}` · `originCatalogSize 31`)와 **두 경로가 독립적으로 일치**한다.

## 8.2 대상 3건 재판정

| id | S | 레인 | 재측정 결과 [OBSERVED] | 판정 |
|---|---|---|---|---|
| C3-F3 | **S1** | systems | S5 = systems 레인 **0파일**. 6개 스펙 9행 제목을 정본과 문자 대조: 법1 `wiring-trace` "배선된 것만 남는다" · 법2 `plate-readout` "원본은 닳지만 사본은 남는다" · 법3 `tide-alignment` "정합 전 시계는 믿지 않는다" · 법4 `drainage-routing` "이번 조수에는 보호 용량이 부족하다" · 법5 `corrosion-budget` "소금은 비용으로 보인다" · 법6 `dual-seal` "원본 책임과 제출을 나눈다" → `worldview-bible.md` L44~L49와 **6/6 문자 일치**. 추가로 `interaction-rules.md` L101 법6 괄호 호명도 정본 | **closed** |
| C3-F26 | S2 | systems | S6 = `tide-alignment.md` 내 `H-1:20` **0건**. A-R3(현 L56)이 "밸브 개폐 각인(H-1:24) → 봉인 완료 접점 각인(H-1:04), 간격 20분, 잔차 ±4분 × 2 = 8분 → 20 > 8"으로 교체됨. 인용 근거를 원문 대조: `timeline.md` L35(§2 머리 "간격 20분 > 총 오차폭 8분") · L44·L45(H-1:24 밸브 개폐 각인 / H-1:04 봉인 완료 접점) · L142 B26 · §8 표 4행이 **전건 지지**. D-A2 산술 `20 > 8`도 성립 | **closed** |
| C3-F28 | S2 | worldview | S1·S2·S7이 `glossary.md` L139·L140의 영수증과 **전건 일치**(sha `fdabf1d4…` · 120479 B · 단서 73 · `log 27 · ledger 22 · plate 24` · 31종). `consistency-audit.md` §3 6행·A40·A41도 같은 값. §7 카탈로그 125행·`missing=[]`은 불변이므로 **C3-F13 수록 판정에 영향 없음**을 재확인. S8 = worldview 레인의 스테일 입력 주장 0건 | **closed** |

### C3-F3 상세 — 마지막 S1이 닫혔다

- 정본 대조를 **아카이브까지** 확인했다: RFC-P3-014가 정본으로 지목한 것은 아카이브 c3 `worldview-bible.md` §3이고, `current/worldview/worldview-bible.md` L13이 그 재기반 사실을 `[OBSERVED]`로 고지한다. 즉 systems가 인용한 §3 표는 세션 P 원문 계보다.
- systems가 5개 스펙 제목 아래에 넣은 **"법 호명 정본" 인용주**(정본 = bible §3, 폐기 문구 보존 = `consistency-audit.md` §4-1)를 확인했다. 이 주석은 다음 세션이 문구를 다시 저작하지 않고 인용하게 만드는 장치이며, 결함을 닫는 것 이상의 재발 방지다 — **수용**.
- 남은 13행(S5)은 전부 기록이다: `gdd.md` L84 출처주(폐기 문구를 "쓰지 않는다"의 목적어로 인용) · 본 검토 6행 · `gate-measurements.md` L60 검증 명령 · `consistency-audit.md` §4-1 폐기 원장 5행. **본문 사용 0**.

### C3-F26 상세 — 대안 (b) 채택을 검증했다

- QA가 제시한 두 대안 중 systems는 (b) "밸브 개폐 각인 → 봉인 완료 접점 각인"을 택했다. **이 선택이 (a)보다 낫다는 systems의 근거를 QA가 검증하고 수용한다**: (b)는 RFC-P3-013이 *확정*한 두 시각만 사용하므로 **C3-F25(H-1:40의 지위)의 (a)/(b) 어느 판정에도 종속되지 않는다.** (a)를 택했다면 간격이 20 → 16으로 바뀌어 D-A2의 `20 > 8` 예시와 `plates.md` §4 고정값까지 연쇄로 움직였을 것이다.
- 부수 수정 A-R2("두 관측소 오차폭의 합" → "두 근거 각각의 오차폭 합")도 확인했다. 새 사례가 **같은 계통 로그의 두 각인**이므로 "관측소"라는 낱말이 규칙과 예시를 어긋나게 하던 것이 맞다. `timeline.md` §8 "왜 드리프트가 없나 / 왜 확정되나" 두 행이 이 정정을 지지한다 — **수용**.

## 8.3 레인 counter 처리 [OBSERVED]

| # | 레인 | counter 내용 | QA 판정 |
|---|---|---|---|
| c-1 | systems | C3-F3 `required_fix`가 폐기 문구 보존 위치를 **§4-2**로 적었으나 실제는 **§4-1**이다 | **인정.** `consistency-audit.md` L95 = "### 4-1 폐기된 6법 호명 문구 (C2 계보)", L109 = "### 4-2 폐기된 장치·전제". QA 오기이며 본 절에서 §4-1로 정정한다 |
| c-2 | systems | C3-F3의 "5줄 수정으로 닫힌다"가 과소 계산이었다 — 레인 전체 grep은 **6곳**(+ `interaction-rules.md` L101) | **인정.** 1차·재검증 1의 grep 범위가 결함이 인용한 5파일에 묶여 있었다. S5는 이번에 `_workspace/current` 전수로 돌렸고 systems 레인 0을 확인했다. **재검증 규칙 개정**: 문자열 결함의 재측정 범위는 결함이 인용한 파일이 아니라 **소유 레인 폴더 전체**로 한다 |
| c-3 | systems | C3-F26의 뿌리가 한 줄이 아니었다 — `data-schemas/plates.md` L77이 "서명 → 밸브 간격 20분"을 유지했다 | **인정.** 캐논상 서명(H-1:40)→밸브(H-1:24)는 16분이고 20분은 밸브→봉인 완료 접점이다(`timeline.md` L43~L45). 이 행은 **폐기 문자열을 쓰지 않으면서 폐기 산술만 남긴** 형태여서 `H-1:20` grep을 통과했다. 현재 L77은 "순서 앵커 간격 = 20분 — 밸브 개폐 각인 → 봉인 완료 접점 각인"으로 교체됨을 확인 → **함께 closed**. **검사 규칙 추가**: 시각 결함은 문자열뿐 아니라 **간격 산술(쌍 + 분)**로도 grep 한다 |
| c-4 | systems | `interaction-rules.md`는 RFC-S5 미해소라 `status: draft` 유지 | **수용(판정 아님).** 열린 판정 요청이 있는 문서에 `current`를 붙이지 않은 것은 정직한 처리다. 다만 이 상태가 **C3-F33**(current 문서가 draft 문서를 정본 인용)을 유지시키므로 F33은 계속 열어 둔다. 디렉터 판정 대상 |
| c-5 | worldview | C3-F25는 RFC-P3-013 본문의 오기이며 (a) "H-1:20·H+0:10 폐기, H-1:40 유지"로 정정하면 timeline은 현행 유지로 닫힌다 | **근거 보강 확인, 판정은 디렉터.** QA 독립 측정: live `campaign.json`에 `H-1:40` **3건** · `H-1:20` **0건** · `H+0:10` **0건**(S3 K-03·K-04 PASS), 검증기 L42 `canonTimesAbsent`도 두 값만 검사. 데이터·검증기·`timeline.md` L50 폐기 목록 셋이 모두 (a) 해석 위에 서 있다는 worldview 진술은 **실측과 일치**한다. C3-F25는 `open-rfc` 유지 |
| c-6 | worldview | C3-F30(B# 두 벌)은 상한표 소유자로서 임의 변경 불가, 디렉터 지정 필요 | **수용.** C3-F30 `open-rfc` 유지 |
| c-7 | worldview | `consistency-audit.md` A33은 세계관 본문 수정으로 닫히지 않는다 | **사실이었으나 이번 회차에 무효화됐다.** systems가 6곳을 고쳤으므로 A33이 주장하는 위반은 **더 이상 존재하지 않는다**(S5). 감사 문서가 남긴 스테일 `[OBSERVED]`를 신규 **C3-F34**로 등록한다 |

## 8.4 신규 결함 — C3-F34 ~ C3-F36

### C3-F34 · S2 · worldview — 정합 감사가 이미 닫힌 위반을 "2차 재측정" 표제 아래 현행 `[OBSERVED]`로 유지한다 · open

- 근거 [OBSERVED, S5]: `worldview/consistency-audit.md` 다섯 위치가 A33(= 폐기 6법 문구가 `systems/system-specs/*` 5행에 살아 있다)을 현재 사실로 적는다 — L26 §1 차단 판단("유일한 violation A33은 systems 레인의 헤더 5줄") · L64 §2 A33 행 `verdict: **violation**` · **L86 §3 기계 검사 행**("폐기 문구 = `systems/system-specs/*` 5건") · L106 §4-1("현재 위반 위치 [OBSERVED] … 각 9행") · L125 §5.
- 실측은 `systems` 레인 **0파일 / 0행**이다. 특히 L86은 §3 표제가 **"[OBSERVED, 2026-09-10 2차 재측정]"**인 기계 검사 표의 행이므로, 재측정을 선언한 자리에 재측정하지 않은 값이 들어가 있다 — 재검증 1이 C3-F15·C3-F27로 잡은 것과 **동일 유형**(스테일 `[OBSERVED]`)이다.
- 파급: §1 집계가 `violation 1`을 유지하므로 **G1의 세계관 자체 감사 수치가 실제보다 나쁘게** 나온다. 게이트 근거 문서가 자기 레인에 불리한 방향으로 틀린 경우라 판정을 막지는 않지만, 디렉터가 G1을 볼 때 닫힌 결함을 열린 것으로 읽게 된다.
- 요구 수정: A33 → `pass`(해소, 근거 = 본 절 S5 + `systems/tech-verification/c3-fixloop2-canon-alignment.md`), §1 집계 `pass 37 / violation 0 / open 4`로 재계산, §3 "법 문구 유통" 행을 재측정값(`systems` 0)으로 교체, §4-1의 "현재 위반 위치"를 **"해소 이력(2026-09-10 수정 루프 2)"**으로 문맥 변경(문구 자체는 보존), §5 C3-F3 행에 닫힘 기록. RFC-W3(법 문구 적용 방향)은 이 수정으로 소멸한다.

### C3-F35 · S2 · planner / economy — 원본 상태 카운터의 이름이 세 벌이고, 기획서가 **철회된 필드명**을 "채택안"으로 인용한다 · open

- 근거 [OBSERVED, `grep -rn "plateOriginalWear\|readBudget" _workspace/current`]:
  - `economy/negotiation-record.md` L102 = "`plateOriginalWear` 신설 요청 **철회** → `readCounts` 사용(같은 축의 필드를 둘 만들지 않는다)", `economy/currency-map.md` L179 = "economy가 제안했던 `plateOriginalWear`는 **철회** — 같은 축의 필드가 이미 있고 개명은 금지다".
  - 그런데 같은 economy 문서 `currency-map.md` **L101**은 여전히 "**채택안 (옵션 B)**: `plateOriginalWear`"라 적는다 → **economy 문서 내부 모순**.
  - 그 L101을 근거로 `planning/gdd.md` **L112 ③**과 `planning/feature-specs/verb-02-plate-read.md` **L39 R3a**가 카운터 이름을 `plateOriginalWear`로 확정 인용한다.
  - 실제 스키마는 `systems/data-schemas/plates.md` L39 `readBudget`(상한) + `systems/data-schemas/save.md` L54 `readCounts`(누계, "`plates.md` `readBudget`과 짝") 두 필드이며, `save.md` L92 v1 필드 목록에 `plateOriginalWear`는 **없다**.
- 왜 S2인가: CLAUDE.md §9 불변식이 저장 필드 이름 변경을 세이브 고아화 사유로 규정한다. C4 핸드오프에서 Codex가 읽는 문서(`gdd.md` · `verb-02`)가 **스키마에 없는 필드명**을 지시하면, 구현자는 새 필드를 만들거나 되물어야 한다. 문서 대조로 지금 닫히는 결함이지만 방치하면 코드에서 재발한다(C3-F4·C3-F27과 같은 경로).
- 부수 [OBSERVED]: `gdd.md` L118·L280의 `[counter → 디렉터]`("systems P-R2는 아직 '4회째 원본 붕괴'로 적혀 있다 … 차단형 예산 문구가 남아 있음")는 **절반만 사실**이다. 문구는 `plate-readout.md` L55에 남아 있으나 같은 표 **P-R9**가 "비차단이다 … 차단형 예산(옵션 C)은 거부됐다", **P-R11**이 "에필로그 문장 1줄만 바꾼다"로 규정한다. 즉 모델은 이미 일치하고 남은 차이는 **이름뿐**이다.
- 요구 수정: economy가 `currency-map.md` L101의 "채택안" 이름을 `readCounts`(누계) + `readBudget`(상한)로 정정하고 L179와 일치시킨다 → planner가 `gdd.md` ③·`verb-02` R3a의 `plateOriginalWear`를 그 두 이름으로 교체하고 L118·L280 counter를 "이름 불일치"로 축소한다. 소유가 두 레인이므로 economy 먼저.

### C3-F36 · S3 · systems — 프로토타입이 순서 앵커 쌍을 **폐기된 두 이름**으로 라벨링한다 · open

- 근거 [OBSERVED]: `systems/prototype/model.mjs` L58 `{ id: 'pair-20', label: '봉인 호출 ↔ 밸브 대기', gapMin: 20 }`. 캐논 쌍은 **"밸브 개폐 각인 ↔ 봉인 완료 접점 각인"**(`timeline.md` §8 · RFC-P3-013)이고, "봉인 호출"은 폐기된 §8 앵커 문구, "밸브 대기"는 금지 문자열 "밸브 명령 대기 흔적"의 축약형이다. 라벨 순서(호출 먼저 → 밸브 나중)는 캐논의 선후(밸브 H-1:24 → 봉인 완료 H-1:04)와 **반대**로 읽힌다.
- 재검증 1은 이 줄을 "도메인 명사, 캐논 명제 아님"으로 PASS 처리했다(§7.4). **그 판정을 좁힌다**: `gapMin: 20`은 캐논과 일치하므로 수치는 문제가 아니지만, C3-F26·자체발견 S-2가 닫힌 지금 이 줄은 **워크스페이스에서 앵커 쌍을 폐기 용어로 부르는 마지막 위치**다. 같은 법을 두 이름으로 부르던 C3-F3과 동형이다.
- 완화 요인(그래서 S3): `prototype.meta.md`가 `status: draft`이며 프로토타입 한정임을 선언한다. 값·테스트(`test-model.mjs` L226 `pair-20=ordered`)는 캐논과 모순되지 않는다.
- 요구 수정: `label`을 "밸브 개폐 각인 ↔ 봉인 완료 접점 각인"으로 교체(id·값·테스트 불변). 1줄.

## 8.5 폐기·기각 문구 잔존 재검사 [OBSERVED, S5·S6]

`grep -rn` 전수. **"본문 사용"** = 현행 규칙·정의·대사로 쓰인 것.

| 문구 | 총 히트 | 본문 사용 | 재검증 1 대비 | 판정 |
|---|---|---|---|---|
| C2 법 문구 5종 | 21행(유니크 13행) | **0** | **5 → 0** | **PASS** (C3-F3 closed) |
| `H-1:20` | 31 | **0** | **1 → 0** | **PASS** (C3-F26 closed) |
| `H+0:10` | 26 | 0 | 동일 | PASS |
| `서명 확인 전` | 9 | 0 | 동일 | PASS |
| `봉인 호출` | 8 | **1** | 동일(판정만 변경) | **관찰** → C3-F36 (`model.mjs:58`, draft 프로토타입) |
| `밸브 명령 대기 흔적` | 8 | 0 | 동일 | PASS |
| `계통 영구 고장` | 15 | 0 | 동일 | PASS |
| `4회째 결정 붕괴` | 3 | 0 | 동일 | PASS |
| `이번 조수에 보호 용량`(기각형) | 9 | 0 | 동일 | PASS |
| `2bfe4d52…` | 19 | 0 | 동일 | PASS |
| `거래를 수락해도 같은 경로가 열린다` | 6 | 0 | 동일 | PASS |
| `번호만 찍힌 염판` | 4 | 0 | 동일 | PASS |
| 아카이브 계보 `30·50·55·60…` | 11 | 0 | 동일 | PASS |
| `775a984c…`(수리 전 sha) | 33 | **1** | — | **관찰** — `decision-log.md` L23 RFC-P3-008 본문(디렉터 소유) → RFC-Q1 |

총 히트가 늘어난 것은 **검토·기록 문서가 늘었기 때문**이며(본 절 포함), 본문 사용은 두 축 모두 0이 됐다.

## 8.6 신선도 [OBSERVED, S4]

```
freshness: 0 finding(s) across 91 markdown artifact(s) under _workspace/current
freshness: scope = frontmatter contract + supersedes topology only; memory_sync receipts (mex/llm-wiki/graphify/zg) are NOT verified here
freshness: no cycle start supplied; staleness not measured
EXIT_CODE=0
```

| 항목 | 재검증 1 | 재검증 2 |
|---|---|---|
| 스캔 산출물 | 90 | **91** (+1 = `systems/tech-verification/c3-fixloop2-canon-alignment.md`) |
| finding / exit | 0 / 0 | 0 / 0 |
| `status: current` 중복 | 0 | 0 |
| memory_sync 영수증 | `[SKIPPED: mex 금지]` | **동일** — `mex`는 CLAUDE.md §10에 따라 실행 금지(TeX 동명 바이너리), graphify는 소스코드 변경 0줄이라 `[UNGRAPHED]` |

## 8.7 집계와 최종 판정

| severity | 총 | closed | open | open-rfc |
|---|---|---|---|---|
| S1 | 7 | **7** | **0** | 0 |
| S2 | 16 (신규 F34·F35) | 10 | 4 (F27 · F29 · **F34** · **F35**) | 2 (F25 · F31) |
| S3 | 10 (신규 F36) | 7 | 1 (**F36**) | 2 (F30 · F33) |
| S4 | 3 | 2 | 0 | 1 (F22) |
| **합계** | **36** | **26** | **5** | **5** |

### 최종 판정: **SPEC-FIX**

- **열린 S1이 0이 됐다.** C3 전 회차를 통틀어 처음이다(1차 7건 → 재검증 1 1건 → 재검증 2 **0건**). CLAUDE.md §6의 "열린 S1은 어떤 게이트도 PASS를 막는다"는 **차단 사유가 소멸**했다. 이것이 이번 회차의 유일한 구조적 전진이다.
- **그러나 SPEC-PASS는 아니다.** ① open S2 4건이 남아 있고 그중 F27(c)는 문장이 아니라 **스키마 충돌**이다. ② 새로 연 F34·F35는 이번 수정 루프가 만든 2차 효과이며, 재검증 1이 진단한 세 유형(판정문↔캐논 불일치 / 교차 레인 스테일 / 기계 미검사 축) 중 **두 번째 유형이 다시 나왔다** — 대규모 동시 수정이 있는 한 이 유형은 회차마다 재생산된다. ③ **런타임 축은 전부 0이다.** 빌드 0줄·표본 n=0·시뮬 0회는 세 회차 내내 바뀌지 않았다.
- 게이트에 미치는 실제 효과: G1의 문서 위반이 **2 → 0**(F3·F26 닫힘)이 됐으나, F29(비트-구역 반전)와 F34(감사 스테일)가 남아 G1은 여전히 문서 수준 FAIL이다. G2~G8은 이번 회차에 **변동 없음**(전부 NOT-MEASURED). 수치는 `qa/gate-measurements.md#g1`~`#g8`.
- C3 닫는 질문 "33비트가 480분 설계 예산과 행동 예산으로 역산되는가"에 대한 재답은 재검증 1과 같다: **예, 문서 수준에서.** 44/44 PASS·집계 2경로 일치가 그것을 지지한다. **480은 설계 분이지 플레이 시간이 아니다.**

### C4 인계 우선순위

**F27(c) → F35 → F34 → F25 → F29 → F31 → F33 → F30 → F36 → F22.**
F27(c)는 스키마 충돌이라 문장 수정으로 닫히지 않는다. F35는 핸드오프 직전에 반드시 닫혀야 하는 필드명 결함이다. F34는 감사 문서 5곳 갱신으로 닫힌다. F25·F30·F31·F33·F22는 디렉터 판정 대기.

## 8.8 브로드캐스트 (dependency-matrix ● 항목)

`feedback-requested-by: 2026-09-11`

| 받는 레인 | 결함 | 요청 |
|---|---|---|
| game-production-director | F25, F30, F31, F33, F22, RFC-Q1 | H-1:40 지위 판정(worldview 실측이 (a)를 지지) · B# 체계 지정 · 난이도 지수 처리 · draft 인용 정책(`interaction-rules.md` RFC-S5 포함) · 비트 `zoneId` · RFC-P3-008 본문 sha·단서 수 갱신 |
| game-worldview-architect | **F34** | `consistency-audit.md` A33 → pass, §1 집계 재계산(`pass 37 / violation 0 / open 4`), §3 "법 문구 유통" 행 재측정, §4-1 "현재 위반 위치"를 해소 이력으로 문맥 변경. RFC-W3 소멸 |
| game-economy-designer | **F35**, F27(c) | `currency-map.md` L101 "채택안 `plateOriginalWear`"를 L179(철회)와 일치시킴 · `operationalCorrosion` 제거(`save.md` L94)를 §5 자원표에 반영 |
| game-planner | **F35**, F29, F22, F30 | `gdd.md` ③·`verb-02` R3a의 `plateOriginalWear` → `readCounts`/`readBudget` · L118·L280 counter를 "이름 불일치"로 축소 · `content-matrix.md` §3 구역 정정 |
| game-systems-designer | **F36**, F27(c) | `model.mjs:58` 라벨을 캐논 쌍 이름으로 교체(1줄) · `operationalCorrosion` 제거를 economy와 합의 |
| game-balance-designer | F27(a)(b), F31 | 스테일 주장 철회 · `동시 가설 수` 산출 규칙 정의 또는 지수 분리 |
| game-synopsis-writer | F29, F30 | 씬·표의 구역 표기를 데이터와 맞춤 |

**QA 자기정정 2건 [OBSERVED]**: (1) C3-F3 `required_fix`의 "§4-2" 표기는 **§4-1**의 오기다(레인 counter c-1 인정). (2) C3-F3 `required_fix`의 "5줄로 닫힌다"는 **6곳**이 맞았다(c-2 인정) — 문자열 결함의 재측정 범위를 소유 레인 폴더 전체로 넓히는 것으로 규칙을 고쳤다.

## 8.9 이 재검증이 증명하지 않는 것 [OBSERVED]

- **어떤 비트도 플레이되지 않았다.** 도달 가능성·진행 막힘·힌트 실효성·완주 분포·결말 도달률·성능은 전부 n = 0이다. 열린 S1이 0이 된 것은 **문서가 서로 모순되지 않게 됐다**는 뜻이지 게임이 동작한다는 뜻이 아니다.
- §8.5의 grep은 **14개 문자열의 전수**이지 의미의 전수가 아니다. c-3이 보여줬듯 **문자열을 쓰지 않으면서 폐기된 산술만 남기는** 형태는 문자열 검사를 통과한다. 이번에 간격 산술 검사를 더했으나 그것도 전수는 아니다.
- 레인들이 "재도출했다"고 보고한 표 중 본 절이 원소 단위로 재계산한 것은 §8.1 S5·S6·S7의 집계와 문자열뿐이다. 33행 표 A·표 B의 서사 내용, K1~K10의 서사적 타당성은 **구조만 보고 내용은 표본 검사했다**.
- `qa/exploit-register.md`·`regression-matrix.md`·`playtest-report.md`·`immersion-scores.md`는 여전히 **미작성**이다. 빌드와 표본이 없어 작성하면 측정 위장이 된다(1차 §4 · 재검증 1 판정 유지).

---

# 9. 재검증 3 (2026-09-10) — C3 종료 수정 루프 3

## 9.0 이 절의 지위와 방법

- 입력: 재검증 2가 남긴 **open S2 4건 중 worldview 소유 1건 = C3-F34** + worldview 레인의 수정 보고 + 디렉터 판정 **RFC-P3-008 ~ RFC-P3-015**(구속력). 나머지 open(F27·F29·F35·F36)은 이번 회차의 대상이 아니며 **판정을 옮기지 않는다**(재검증 2 값 그대로 이월).
- 방법: **레인 보고를 신뢰하지 않고 파일을 다시 열고 명령으로 재측정**했다. 이번 결함은 성격이 특수하다 — 대상이 *감사 문서 자신의 스테일*이므로, "고쳤다"는 보고를 그대로 옮겨 적으면 QA가 같은 유형(스테일 `[OBSERVED]`)을 한 단계 위에서 재생산한다. 그래서 §9.1의 명령 7종을 QA가 직접 실행했다.
- 이번 회차에 처음 한 것: 레인이 §1에 **집계 재도출 명령을 문서 안에 적어 넣었기에, 그 명령을 그대로 복사해 실행**했다. 문서가 주장하는 재현 가능성을 QA가 실제로 재현한 것은 C3 전 회차 중 처음이다(T3).
- **자기 계수 회피 [OBSERVED]**: 본 절은 폐기 6법 문구 5종을 **문자 그대로 적지 않고** 플레이스홀더 `<법 호명 문구 5종>`로만 쓴다. 재검증 2의 S5가 자기 파일에 문구를 적어 13 → 14로 값을 스스로 움직인 것(§9.3 (3))을 되풀이하지 않기 위함이며, 그 결과 T2의 `qa/c3-review.md` 7행은 **본 절 append 후에도 불변**이다.
- 본 절도 **어떤 게이트도 PASS로 올리지 않는다.** 빌드 0줄 · 사람 플레이 n=0 · 시뮬 실행 0회 · 성능 캡처 0건. 새로 측정된 런타임 값은 **0건**이다.
- 쓰기는 `_workspace/current/qa/` 안에서만 했다(본 파일 · `defect-register.md` · `gate-measurements.md`). 다른 레인 파일은 **읽기만** 했다.

## 9.1 재측정 명령 원문 [OBSERVED 2026-09-10]

| # | 명령 | 결과 |
|---|---|---|
| T1 | `grep -rlE "<법 호명 문구 5종>" _workspace/current/systems \| wc -l` · `grep -rnE … \| wc -l` | **0파일 / 0행** — A33이 주장하던 위반의 실체가 없다 |
| T2 | `grep -rlE "<법 호명 문구 5종>" _workspace/current` 후 파일별 `grep -c` | **4파일 / 14행** — `planning/gdd.md` 1 · `qa/c3-review.md` **7** · `qa/gate-measurements.md` 1 · `worldview/consistency-audit.md` 5. **본문 사용 0** |
| T3 | 감사 §1이 문서 안에 적어 둔 재도출 명령을 그대로 실행: `S=$(grep -n "^\| A01 " $F …); E=$(grep -n "^\| A41 " $F …); awk … \| awk -F'\|' '{print $5}' \| sort \| uniq -c` | `37 pass` · `4 **open**` · violation 항목 **0** · 행 수 **41** — 문서 §1 표(37/0/4, 합 41)와 **전건 일치** |
| T4 | `sed -n '9p' _workspace/current/systems/system-specs/{wiring-trace,plate-readout,tide-alignment,drainage-routing,corrosion-budget,dual-seal}.md` ↔ `worldview-bible.md` L44~L49 | 법1~법6 호명 **6/6 문자 일치** [OBSERVED, 원문 대조] |
| T5 | `grep -rl "archive/20260909-preproduction-c3/worldview" _workspace/current` | **8파일** — `worldview/{worldview-bible,timeline,consistency-audit}` · `concept/style-guide.md` · `production/decision-log.md` · `qa/{c3-review,defect-register,gate-measurements}` |
| T6 | `shasum -a 256` · `wc -c` on `planning/campaign.json` · `node planning/validate-campaign.mjs` | `fdabf1d4…` / `120479` / `checks 44 · pass 44 · fail 0 · verdict PASS`, exit 0 — 감사 §3의 **2차 행들이 여전히 유효**함을 확인(입력이 안 바뀌었다) |
| T7 | `bash .claude/skills/game-ops-harness/scripts/freshness-check.sh` | `0 finding(s) across 91 markdown artifact(s)`, exit `0` (재검증 2와 동일) |

> T1·T2·T4는 QA가 실행한 값이며 worldview 보고서의 숫자를 옮긴 것이 아니다. T3은 **문서가 제시한 명령의 재현 검사**이므로 "값이 맞다"와 "값을 다시 만들 수 있다"를 함께 확인한다.

## 9.2 대상 1건 재판정 — 결함이 지목한 다섯 위치 전부

| 위치 | 결함이 지목한 상태(재검증 2) | 현재 상태 [OBSERVED 2026-09-10] | 판정 |
|---|---|---|---|
| §1 차단 판단 (구 L26) | "유일한 violation A33은 systems 레인의 헤더 5줄" — `[INFERENCE]` | L36 = "**violation 0건**" `[OBSERVED, 3차 재측정]`, 남은 open 4건을 A22·A25·A29·A37로 열거하고 전부 타 레인 소유임을 명시 | **수정 확인** |
| §1 집계 (표) | `pass 36 / violation 1 / open 4` | `pass 37 / violation 0 / open 4 / 합 41`, 옛 수치는 "3차 전(스테일)" 열로 **보존**. T3이 재도출 명령까지 재현 | **수정 확인** |
| §2 A33 (구 L64) | `verdict: **violation**` | `pass` + 해소 근거(6/6 문자 일치 · systems 0파일/0행 · `qa/c3-review.md` §8.1 S5·§8.2 · `systems/tech-verification/c3-fixloop2-canon-alignment.md`) + 재발 방지(각 스펙 L11 인용주) | **수정 확인** |
| §3 기계 검사 (구 L86) | 표제가 "2차 재측정"인데 행은 1차 값("`systems/system-specs/*` 5건") | 「법 문구 유통」이 **3행으로 분해**(폐기 0파일/0행 · 전수 4파일/14행 · 정본 6/6) + **행별 회차 표기 규칙** 신설. 「supersedes 재연결」도 3차 재실행(8파일, 파일명 열거) | **수정 확인 + 요구 이상** |
| §4-1 (구 L106) | "**현재 위반 위치** `[OBSERVED]` … 각 9행" | "**해소 이력** `[OBSERVED, 2026-09-10 · 3차 재측정으로 확인]`"으로 문맥 전환, 과거형 서술. 폐기 문구 5행은 **원문 그대로 보존**(T2에서 5행 확인) | **수정 확인** |
| §5 (구 L125) | C3-F3 잔여가 열려 있음 · 소유 표기 planner | C3-F3 소유를 `planner → **systems**`로 정정하고 **해소(closed)** 기록, **C3-F34 행 신설**, C3-F24 행의 "RFC-W3 동봉"을 소멸 반영으로 수정 | **수정 확인** |
| (요구 밖) §6 인계 | — | RFC-W3을 **취소선 + 소멸(2026-09-10)** 로 전환하되 제기 내용 보존. "타 레인이 닫은 결함의 감사 행 재측정"을 worldview 자기 인계로 신설 | **수용** |

**C3-F34 → `closed`.** 다섯 위치 전부가 실측 위에서 다시 쓰였고, QA 독립 재측정(T1~T5)이 문서의 모든 새 수치와 일치한다.

### C3-F34 상세 — 왜 "고쳤다"를 넘어 수용하는가

1. **삭제가 아니라 문맥 전환으로 닫았다.** 폐기 문구 5행을 지우면 T2가 깨끗해지고 감사도 통과하지만, `systems/system-specs/*` 6종의 L11이 §4-1을 폐기 문구의 **유일한 보존 위치**로 인용하고 있어 6개 스펙의 인용주가 동시에 고아가 된다. 레인은 문구를 보존하고 절의 성격만 "위반 목록 → 재유입 방지 원장"으로 바꿨다 — CLAUDE.md §2 "삭제는 없다"와 재발 방지가 동시에 성립하는 유일한 처리다. **수용.**
2. **집계를 손으로 세지 않았다.** §1이 행 번호가 아니라 `A01`·`A41` **앵커**를 쓰는 명령을 문서에 남겨, 표가 편집돼도 같은 명령이 다시 돈다. T3에서 QA가 그대로 실행해 37/0/4·41행을 재현했다. 재검증 1·2가 반복 지적한 "재도출했다는 주장에 명령이 없다"의 첫 구조적 해소다.
3. **QA 자기정정 [OBSERVED]**: 재검증 2 S5는 전수를 `4파일 / 13행`(`qa/c3-review.md` 6)으로 적었고, 감사 3차와 본 회차 T2는 `4파일 / 14행`(동 7)이다. 차이 1행의 정체를 확인했다 — `qa/c3-review.md` **L521**, 즉 **S5 명령 자체를 기록한 줄**이다(측정 후 기록되어 다음 측정에 잡힘). 감사가 `[INFERENCE]`로 적은 자기 계수 설명은 **[OBSERVED]로 승격**한다. 두 값 모두 `systems = 0`·본문 사용 0에서 같으므로 A33 판정에는 어느 쪽으로도 영향이 없다.
4. **판정 방향의 정직성.** 이 결함은 감사가 자기 레인에 **불리한 방향으로** 틀린 경우였다(violation을 실제보다 1건 많게 유지). 그래서 수정은 G1 수치를 **좋게** 만든다 — QA가 레인 보고만 보고 옮겼다면 "게이트 수치가 좋아지는 방향의 자기 보고"를 검증 없이 통과시킨 셈이 된다. T1·T3·T4를 QA가 직접 돌린 이유가 이것이다.

## 9.3 레인 open question 처리 [OBSERVED]

| # | 레인이 올린 것 | QA 처리 |
|---|---|---|
| q-1 | `gate-measurements.md` L47 `worldview_self_audit_claim`이 아직 `pass 36 / violation 1 / open 4`를 인용한다(QA 소유) | **인정, 본 회차에서 갱신.** `#g1`의 해당 키를 `pass 37 / violation 0 / open 4 (41항목, 3차 재측정, QA 재현 T3)`로 교체했다. 지적이 맞다 — 감사가 고쳐진 순간 QA 인용이 스테일이 됐고, **이번엔 QA가 스테일 측**이었다 |
| q-2 | `#g1` `violation_open` 목록에 C3-F34가 남아 있다 | **인정, 본 회차에서 제거.** `#g1` violation 은 2건 → **1건(C3-F29)** 으로 줄었다 |
| q-3 | RFC-W3 소멸을 `production/decision-log.md`에도 적을지 | **디렉터 판정 요청(아래 브로드캐스트).** QA도 decision-log를 쓰지 않는다(디렉터 소유). 다만 소멸 근거는 QA 실측으로 뒷받침된다 — T1 = 0파일/0행이므로 RFC-W3이 묻던 "교체 대상이 planner·덱인가 systems인가"는 **대상 자체가 남아 있지 않다**. 레인이 문서에서 삭제하지 않고 취소선+기록으로 보존한 처리는 CLAUDE.md §2에 부합하므로, decision-log 기재 없이도 추적성은 유지된다 |
| q-4 | §4-1의 절 번호가 systems 스펙 6종 L11의 인용 대상이므로 절 번호 고정을 불변식으로 승격할지 | **QA 판단: 근거 있는 요청. 디렉터 판정 대상.** 실측으로 뒷받침한다 — 6종 스펙이 §4-1을 인용하고(T4 대조 시 확인), §4-1이 사라지면 T2의 "본문 사용 0"을 지탱하는 보존처가 없어져 폐기 문구가 다시 각 스펙으로 흩어질 유인이 생긴다. 다만 **불변식 승격은 CLAUDE.md §9 편집**이므로 디렉터·사용자 승인 사항이며 QA가 정할 수 없다. 대안(더 싼 것): 인용을 절 번호가 아니라 **제목 문자열**(“폐기된 6법 호명 문구”)로 바꾸면 번호 변경에 견딘다 |

## 9.4 이번 수정이 만든 2차 효과 검사 [OBSERVED]

재검증 1·2가 "대규모 동시 수정은 회차마다 2차 효과를 낳는다"고 판정했으므로, 이번 1파일 수정에 대해서도 같은 검사를 돌렸다.

| 축 | 검사 | 결과 |
|---|---|---|
| 스테일 인용(교차 레인) | 감사 §1의 옛 집계를 인용하는 `status: current` 문서 전수 `grep -rn "pass 36"` | **1건 — `qa/gate-measurements.md` L47(QA 소유).** 본 회차에서 갱신 → 잔존 0 |
| 스테일 인용(RFC) | `grep -rn "RFC-W3" _workspace/current` | 4행 — 감사 §5·§6(소멸 기록) + QA 문서 2행(요구 수정 인용). **RFC-W3을 현행 판정 대기로 적는 문서 0건** |
| 소유 표기 | 감사 §5가 C3-F3 소유를 `planner → systems`로 정정 vs `defect-register.md` L25 lane 열 = `planner` | **QA 측 스테일.** 본 회차에서 register lane 열을 `planner → systems`로 정정(자기정정) |
| 문구 재유입 | T1 · T2 | systems 0파일/0행, 본문 사용 0 — 재유입 없음 |
| 항목 수 조작 | T3 행 수 | **41행 불변.** verdict만 뒤집혔고 항목을 지우거나 더해 집계를 개선한 흔적 없음 |
| 신선도 | T7 | 0 finding / 91 artifacts / exit 0 — 산출물 수 불변(신규 파일 0) |

**측정 지표의 안정성에 대한 경고 [INFERENCE]**: T2의 "전수 14행"은 기록 문서가 늘면 증가한다(본 절이 플레이스홀더를 쓴 이유). 회귀 신호로 쓸 축은 **`systems` 레인 0 + 본문 사용 0** 두 개이며, 총행 수는 참고값이다. 다음 회차가 총행 증가를 위반으로 오독하지 않도록 여기 남긴다.

## 9.5 신규 관찰 — 결함으로 등록하지 않은 것 [OBSERVED]

**`supersedes: null`인데 같은 경로의 앞선 판을 대체했다고 자기 고백하는 문서** — `worldview/consistency-audit.md` §0("앞선 판(C3 1차)의 점검은 폐기된 본문 위에서 수행됐으므로 …")과 **`qa/c3-review.md` §0**("같은 경로에 이전 C3 검토본이 있었고 본 판본이 그것을 같은 경로에서 대체한다")이 둘 다 `supersedes: null`이다. CLAUDE.md §2는 대체 시 `archive-cycle.sh` 경유와 `supersedes:` 연결을 요구한다. `freshness-check.sh`는 **이 축을 검사하지 않는다**(T7이 0 finding인 이유). 모집단: `status: current` + `supersedes: null` = **53개 문서**(전체 current `.md` 95, 아카이브 경로를 가리키는 것 15) [OBSERVED].

결함 id를 열지 않은 이유: (1) **QA 자신이 위반 당사자**이므로 타 레인 결함으로 등록하면 부정직하다. (2) "같은 세션 안에서 초안을 제자리 갱신한 것"이 §2의 대체인지 여부는 **계약 해석**이며 디렉터 소유다. → **RFC-Q2**로 올린다(아래).

## 9.6 집계와 최종 판정

| severity | 총 | closed | open | open-rfc | 재검증 2 대비 |
|---|---|---|---|---|---|
| S1 | 7 | **7** | **0** | 0 | 변동 없음 |
| S2 | 16 | **11** | **3** (F27 · F29 · F35) | 2 (F25 · F31) | closed +1 (**F34**) |
| S3 | 10 | 7 | 1 (F36) | 2 (F30 · F33) | 변동 없음 |
| S4 | 3 | 2 | 0 | 1 (F22) | 변동 없음 |
| **합계** | **36** | **27** | **4** | **5** | closed 26 → **27** |

### 최종 판정: **SPEC-FIX**

- **닫힌 것**: C3-F34(S2) 1건. 열린 S1은 재검증 2에 이어 **0건 유지**. G1의 문서 violation 은 **2건 → 1건**(C3-F29만 남음)이 됐다.
- **SPEC-PASS가 아닌 이유** — 세 가지가 그대로다.
  1. **open S2 3건.** F27(c)는 문장이 아니라 **스키마 충돌**(`operationalCorrosion` 삭제 vs economy 계통별 표시 모델), F35는 **저장 필드명**이 철회된 이름으로 핸드오프 문서에 박혀 있는 상태(CLAUDE.md §9 세이브 고아화 축), F29는 데이터↔단일출처표의 실제 반전이다. 어느 것도 이번 회차에 손대지 않았다.
  2. **open-rfc 5건**(F25 · F30 · F31 · F33 · F22)은 전부 디렉터 판정 대기이며, QA가 닫을 수 없다.
  3. **런타임 축 전부 0.** 빌드 0줄 · 사람 표본 n=0 · 시뮬 0회 · 성능 캡처 0건. 네 회차 내내 바뀌지 않았다. G2~G8은 전건 `NOT-MEASURED`이며 **이번 회차에 새로 측정된 런타임 값은 0건**이다.
- **SPEC-REDO도 아닌 이유**: 이번 수정은 1파일 · 지목된 절만 정밀 수정이고, 재도출을 명한 집계(§1)는 명령으로 재계산돼 QA 재현과 일치했다. 재작성을 요구할 근거가 없다.
- C3 닫는 질문에 대한 답은 재검증 1·2와 동일하다: **"예, 문서 수준에서."** 44/44 PASS·집계 2경로 일치가 그것을 지지한다. **480은 설계 분이지 플레이 시간이 아니다.**

### C4 인계 우선순위 (F34 제거 반영)

**F27(c) → F35 → F25 → F29 → F31 → F33 → F30 → F36 → F22.**

## 9.7 브로드캐스트 (dependency-matrix ● 항목)

`feedback-requested-by: 2026-09-11`

| 받는 레인 | 결함 | 요청 |
|---|---|---|
| game-worldview-architect | — | **C3-F34 closed.** 추가 요청 없음. §6 자기 인계("타 레인이 닫은 결함의 감사 행 재측정")와 §3 행별 회차 표기 규칙을 다음 사이클에도 유지할 것 |
| game-production-director | RFC-Q2, q-3, q-4, F25 · F30 · F31 · F33 · F22, RFC-Q1 | ① **RFC-Q2**(같은 경로 제자리 대체 문서 53건의 `supersedes` 처리 — QA 자신 포함) ② RFC-W3 소멸의 decision-log 기재 여부 ③ §4-1 절 번호 고정의 불변식 승격 여부(QA 대안: 인용을 제목 문자열로) ④ 기존 판정 대기 5건 + RFC-Q1(RFC-P3-008 본문 sha·단서 수 갱신) |
| game-economy-designer | F35, F27(c) | 재검증 2 요청 유지 — `currency-map.md` L101을 L179(철회)와 일치시켜 `readCounts`/`readBudget`으로 정정 |
| game-planner | F35, F29, F22, F30 | 재검증 2 요청 유지 |
| game-systems-designer | F36, F27(c) | 재검증 2 요청 유지 — `model.mjs` L58 라벨 1줄 |
| game-balance-designer | F27(a)(b), F31 | 재검증 2 요청 유지 |
| game-synopsis-writer | F29, F30 | 재검증 2 요청 유지 |

**RFC-Q2 (qa → director)**: `status: current` + `supersedes: null` 문서 **53건** 중 최소 2건(`worldview/consistency-audit.md`, **`qa/c3-review.md`**)이 본문에서 "같은 경로의 앞선 판을 대체했다"고 적는다. CLAUDE.md §2는 대체에 `archive-cycle.sh` + `supersedes:` 연결을 요구하지만 `freshness-check.sh`는 이 축을 검사하지 않는다. 판정 요청: **같은 사이클 안의 초안 제자리 갱신을 §2의 "대체"로 볼 것인가.** 아니라면 계약에 예외를 명문화하고, 맞다면 `freshness-check.sh`에 검사를 추가해야 한다(현재는 어느 쪽도 아니어서 규칙이 조용히 지켜지지 않는다). QA는 당사자이므로 스스로 닫지 않는다.

**QA 자기정정 3건 [OBSERVED]**: (1) `gate-measurements.md` L47의 `worldview_self_audit_claim`이 스테일이었다 → 37/0/4로 교체(q-1). (2) 같은 파일 `violation_open`에 닫힌 F34가 남아 있었다 → 제거(q-2). (3) `defect-register.md` C3-F3 행의 lane 열이 `planner` 단독이었다 → `planner → systems`로 정정. 세 건 모두 **감사 문서가 아니라 QA 문서가 스테일 측**이었던 경우다.

## 9.8 이 재검증이 증명하지 않는 것 [OBSERVED]

- **어떤 비트도 플레이되지 않았다.** 도달 가능성 · 진행 막힘 · 힌트 실효성 · 완주 분포 · 결말 도달률 · 성능은 전부 n = 0이다. C3-F34가 닫힌 것은 **G1 증거 문서의 자기 수치가 실물과 일치하게 됐다**는 뜻이지, 세계관이 플레이어에게 일관되게 전달된다는 뜻이 아니다(G1의 몰입·설득력 축은 측정되지 않았다).
- **감사 문서의 41항목이 세계관 정합의 전수라는 주장을 지지하지 않는다.** 항목은 세계관 레인이 스스로 열거한 것이고, 본 회차가 검사한 것은 그 항목들의 **verdict가 실물과 맞는가**이지 항목 목록의 완전성이 아니다.
- T1·T2는 **문자열 5종의 전수**이지 의미의 전수가 아니다. 재검증 2 c-3이 보여준 "문자열 없이 폐기 산술만 남기는" 형태는 이 검사를 통과한다.
- **이번 회차는 대상 1건만 판정했다.** F27 · F29 · F35 · F36의 status 는 파일을 다시 열지 않고 재검증 2 값을 이월했다 — 그 사이 해당 레인이 수정했다면 본 절의 집계는 실제보다 나쁠 수 있다. 이월값임을 `[CARRIED]`로 명시한다.
- `qa/exploit-register.md` · `regression-matrix.md` · `playtest-report.md` · `immersion-scores.md` 는 여전히 **미작성**이다. 빌드와 표본이 없어 작성하면 측정 위장이 된다(1차 §4 판정 유지).
