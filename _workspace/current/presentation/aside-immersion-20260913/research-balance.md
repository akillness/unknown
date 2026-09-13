---
updated: 2026-09-13
cycle: 20260909-preproduction-c7
status: draft
supersedes: null
owner: game-balance-designer
---

> [OBSERVED] 독립 조사 시점의 보고서. 제안은 승인·구현·현재 런타임 검증을 뜻하지 않는다. 최종 채택/기각은 이 폴더 decision-and-implementation.md를 우선한다.

# 딥리서치 — 비전투 추리 게임의 추론 확인·힌트·자율 탐색 (밸런스 레인)

- 작성: game-balance-designer 역할 (`.claude/agents/game-balance-designer.md`)
- 파이프라인: deep-research SKILL.md → `references/web-search-pipeline.md` **general-web** 모듈 (Best Practices / Comparative Research)
- 범위 한정: 조사 4개 출처 직접 정독 → 본 프로젝트 대조 → 데이터-only 개선 **1건** 제안
- **저장소 편집 0건.** 원작 디자인·이미지·텍스트 복제 0건 (규칙 수준의 설계 원리만 인용).
- 측정 상태: 본 프로젝트 사람 플레이 n=0, 빌드 n=0, 시뮬 0건 → 아래 제안은 **수치 발명 없이 기존 선언값의 미러 완성**에 한정한다.

---

## 1) 원문 출처 URL · 짧은 인용

### S1. Lucas Pope 개발 로그 — Return of the Obra Dinn, "Fate Validation"
`https://dukope.com/devlogs/obra-dinn/tig-30/` (#734, 2017-08-11, 저자 = Lucas Pope 본인)

> "I prefer something more immediate just for player satisfaction, but there's a problem that immediate feedback can be cheated. If I tell them their guess was correct right away, it's trivial for them to just try all the combinations and boom, game skipped. The solution I ended up with is to basically make it really hard to try all the combinations. The game now tells you when any set of 3 fates are correct."

> "Cheating one fate is easy but cheating three fates requires just enough work, intent, or both that you'd be better off solving it the right way."

같은 글, 힌트 전달에 대한 자기 비판 (이전 PAX 빌드):

> "you would've been treated to numerous popup messages with graduated hints and clues about how you're supposed to advance ... **Nobody read the hints and so most people got stuck.**"

그리고 그 해법이 힌트 강화가 아니라 **구조 변경**이었다는 진술:

> "Now it's possible to **hintlessly** progress through the game while piecing together its internal logic and story flow at a much more natural pace."

### S2. 같은 개발 로그의 플레이어 보고 — 확정 입도가 만든 무차별 대입 메타
`https://dukope.com/devlogs/obra-dinn/tig-37` (#854, 2019-08-01)

⚠️ **귀속 주의**: 아래는 **Lucas Pope가 아니라 포럼 사용자 `sherjilozair`의 글**이다(같은 페이지의 Replies 블록). 개발자 진술로 인용하면 안 된다.

> "it was too easy to brute force many of the fates of the topmen and seamen ... Since the game explicitly tells me that I get to know correct fates in pairs of three, it felt that **this brute forcing is the intended method**. Due to this, I completely missed out on how to use the hammocks to identify topmen and seamen."

→ S1의 방어책(3개 묶음)도 **탐색 공간이 작은 구역(이름 후보가 몇 개뿐인 선원군)에서는 뚫린다**는 사후 관측. 개발자 응답은 그 페이지에 없다 → `[OBSERVED: 플레이어 1인 보고]`, 규칙으로 승격 금지.

### S3. Color Gray Games (Andrejs Klavins) 인터뷰 — The Case of the Golden Idol
`https://www.gamedeveloper.com/design/case-of-the-golden-idol` (2022-11-01, Game Developer)

> "When building an early demo, players had to fill in the scroll puzzle for a scenario and it would tell them if it was correct or incorrect. This worked fine for a smaller, tutorial-like scenario, but with more complex scenarios, **players would get very frustrated. This was because there were so many places they might be wrong and they had no idea which of their deductions were correct.**"

> "Therefore, we introduced the extra puzzles, such as identifying people in the scene and solving an additional custom puzzle for each scenario, which greatly improved the satisfaction ... Now, they had a feeling that they are getting somewhere and were rewarded for figuring things out."

> "With a limited input system, an option to brute force solutions appears. This meant **we could not validate each phrase slot**, but evaluating the whole puzzle as wrong or right frustrated the players. Thus, we settled on adding the indicator **'two or fewer slots are incorrect'**."

> "However, this **reduced the difficulty and length of the game**."

→ 두 축이 정면으로 대립한다는 **개발자 본인의 실측 증언**: 슬롯 단위 검증 = 무차별 대입 / 전체 단위 검증 = 좌절 / 해법 = **근접도만 알려주는 중간 입도**, 대가는 난이도·길이 감소.

### S4. Mobius Digital 공식 개발 블로그 — Outer Wilds, 자율 탐색의 "의도성"
`https://www.mobiusdigitalgames.com/news/the-intentionality-of-wandering` (2016-08-18, 크리에이티브 디렉터 Alex Beachum 인터뷰)

> "the whole point of the game is having your choices motivated by your curiosity. **So we decided to telegraph where paths lead.**"

> "it's not that I'm opposed to wandering, but **I want players to know whether or not they're actively wandering. We want to remove aimless wandering from the design**, because the game is entirely freeform, we don't tell you what to do, but **we give you enough information that you make conscious choices.**"

> "We're trying to avoid explicit signs as much as possible ... I can tell you 'This path goes to the Southern Observatory,' and that doesn't really prepare you for what it looks like or what it is."

(참고 · 보조) GDC 2020 세션 공식 안내 `https://gdconf.com/article/attend-gdc-and-learn-how-outer-wilds-nailed-curiosity-driven-game-design/` — Beachum·Verneau의 "Curiosity-Driven Exploration: The Design of 'Outer Wilds'" 세션이 존재한다는 사실 확인용. **내용 인용 없음**(세션 본문 미정독).

---

## 2) 본 프로젝트 관찰 (파일·라인 증거)

| # | 관찰 | 증거 |
|---|---|---|
| O1 | 힌트는 **수동 3단 + 180초 비강제 제안 1종**, 자동 승격 전이 0개(H-R9). 비용·업적·엔딩 영향 전부 0 | `_workspace/current/systems/system-specs/hint-system.md` §2 상태기계 표 · §3 H-R9 · `balance/balance-sheet.md` L317-318, L360-368 |
| O2 | 힌트 3단은 **경고 후 해답 노출**이 데이터로 선언돼 있고 런타임이 그 플래그를 읽는다 | `systems/data/t0/hints.json` `"warnsBeforeReveal": true` (t0-b1-h3/b2-h3/b3-h3) · `unity/Unknown/Assets/_Project/App/T0GameSession.cs:341-342` |
| O3 | **정합(법3)에만 근접 실패대가 있다**: 합격 \|r\|≤4 / 근접 4<\|r\|≤8("한 칸 차이" 피드백만, 정답 미노출) / 명백 \|r\|>8(방향만). S3의 "two or fewer slots are incorrect"와 같은 계열의 설계 | `balance/balance-sheet.md` §5 표 (L263~278) |
| O4 | 그런데 **근접 실패대 상한 8은 데이터 테이블에 미러가 없다.** knobs에는 `residualLimitMinutes: 4`만 있고 near-miss 키가 없다 | `_workspace/current/systems/data/t0/tools.json` `knobs` (balance 소유 5키: `commitHoldSeconds 0.4`, `readBudget 3`, `residualLimitMinutes 4`, `idleHintOfferSeconds 180`, `hintOfferCooldownSeconds 180`) · `unity/.../Data/Tables/tools.json:19` |
| O5 | 무차별 대입 표면이 **실제로 존재한다**: 정합 후보 21슬롯, 무작위 정답 1/21 = 4.76%. 2자료 동시는 1/441 = 0.23% — 즉 **"매체 2종 대조" 불변식이 곧 S1의 3개 묶음 방어와 같은 역할**을 한다 | `balance/balance-sheet.md` §5 "무작위 정답 확률" 행 · §8 `safety_two_disjoint_media_paths` |
| O6 | 확정은 **미리보기 2단계가 기본**(`confirmMode: "two-step"`), `preview → confirm` 오버레이. 즉 확정 전 되돌림 지점이 코드로 보장된다 | `T0GameSession.cs:64-65` (`DefaultSettings`), `:136-138` `RequestConfirm`, `:345-357` preview/confirm 렌더 |
| O7 | T0 3비트는 **자율 탐색 + 매체 2종 대조**를 그대로 구현한다. `t0-b3.completion`이 "인용 2건이 서로 다른 매체로 결손 구간의 시작과 끝을 확정"으로 명시, `proofRequired: true` | `planning/campaign.json` beat `t0-b3` (`clues`: `plate/plate-standard-hub` + `ledger/tide-ledger-bureau`, `completion` 문자열) |
| O8 | T0 힌트 L1은 **구역·도구를 알려주되 자료·값은 말하지 않는다** — S4의 "telegraph where paths lead, 단 그 끝이 무엇인지는 말하지 않는다"와 같은 규약 | `hints.json` `t0-b1-h1` "작업대와 서랍이 그 결정의 자리다" · `t0-b3-h1` "판독기 앞에서, 하나의 자료만으로 닫으려 하지 말라" (`revealScope: "direction"`, `revealsValues: false`) |
| O9 | **실패에 자원 비용이 없다**: 정합 실패 부식 0·원본 마모 0·되돌림 무료, 확정도 무소모(RFC-P3-009). 소모성 자원 자체가 없어 소진 막힘이 구조적으로 불가 | `balance/balance-sheet.md` §5 "실패 비용" 행 · §8 `safety_critical_path_resource_dependency: none` |
| O10 | `hint_offer_per_session_max`가 `null`로 **의도적 공란**(근거 n=0이라 값 발명 금지, Q4) | `balance/balance-sheet.md` L367, §10.2 Q4 |
| O11 | knobs 중 **런타임이 실제로 읽는 것은 `idleHintOfferSeconds` 하나뿐**이다. `residualLimitMinutes`를 읽는 `.cs`는 `_Project` 전체에 0건 | `T0GameSession.cs:77` (`tools["knobs"]["idleHintOfferSeconds"]["value"]`) · grep `residualLimit` in `unity/Unknown/Assets/_Project/**/*.cs` → 0 hits |
| O12 | **동시 편집 경계**: `hints.json` / `hints.meta.md` / `T0GameSession.cs` / `beats.json` 등이 현재 `M`(수정됨) 상태다. OMP가 힌트 idle 타이머·키보드 입력 통일을 작업 중 → 이 파일들은 본 레인이 건드리지 않는다 | `git status --porcelain` (2026-09-13 관측) |

---

## 3) 채택 / 보류 / 기각

| id | 원문 설계 원리 | 판정 | 사유 |
|---|---|---|---|
| A1 | **S3** 전체 정오답 이분 판정은 좌절을 만든다 → **근접도만 주는 중간 입도** | **채택(이미 설계에 있음, 미러만 미완)** | O3가 §5에 같은 모델을 이미 선언. 다만 O4로 **데이터 미러가 없어 튜닝·검증 불가** → §4 개선안의 근거 |
| A2 | **S1** 즉시 피드백은 무차별 대입을 부른다 → 정답 확정을 **묶음 단위로 어렵게** | **채택(형태를 바꿔서)** | 본 게임의 대응물은 "3개 묶음"이 아니라 **매체 2종 대조**(O5·O7). 1/441 = 0.23%로 추측 풀이가 이미 차단돼 있다. Obra Dinn식 묶음 카운터를 **추가 도입하지 않는다** |
| A3 | **S1** "그래픽 팝업 힌트를 아무도 읽지 않아 다 막혔다" → 힌트 강화가 아니라 **구조를 고쳐 힌트 없이도 진행되게** | **채택(원칙으로)** | O1·O9와 정렬. 힌트 임계(180/180) 조정으로 막힘을 해결하려는 시도는 **이 증거에 반한다** → 힌트 튜닝을 1순위 레버로 쓰지 말 것 |
| A4 | **S4** "정처 없는 배회를 제거하되 목적지의 정체는 말하지 않는다" | **채택(이미 규약화됨)** | O8의 `revealScope: direction` + H-R1/H-R2가 같은 규약. **추가 변경 불필요** |
| A5 | **S2** 탐색 공간이 작은 구역에서 확정 피드백이 대입 메타를 만든다 | **보류** | 개발자 진술이 아니라 **플레이어 1인 보고**(귀속 주의). 본 게임에 적용하려면 "후보 수가 적은 확정"의 실측이 필요 → **QA 관측 항목으로만 등록**, 수치 변경 근거로 쓰지 않는다 |
| A6 | **S3** "'two or fewer' 지표는 난이도와 길이를 줄였다" | **보류** | 대가가 개발자 본인 증언으로 확인됨. 본 게임의 근접대(4<\|r\|≤8)도 같은 대가를 가질 수 있으나 **n=0이라 판정 불가**. 밴드를 넓히는 방향의 변경 금지 |
| A7 | **S1** 확정 후 페이지 **잠금**(수정 불가) | **기각** | 본 게임은 §8 `safety_rollback_depth_beats ≥ 1` + 무제한 되돌림(O9)이 불변식이다. 잠금은 정면 충돌 |
| A8 | **S3** 보조 퍼즐을 추가해 중간 보상을 만든다 | **기각(이번 사이클)** | 콘텐츠 신설 = planner/systems 레인. 데이터-only 아님 |
| A9 | 힌트 세션 상한 `hint_offer_per_session_max`에 값 지정 | **기각** | ① 근거 n=0(O10, Q4) — 발명 금지 ② **OMP가 힌트 idle/입력 레인을 작업 중**이라 파일 충돌 |

---

## 4) 현재 구현 가능한 데이터-only 개선 **1개**

### CHG-B-01 · 근접 실패대 상한을 knobs 테이블에 미러한다

**무엇**: `tools.json`의 `knobs`에 **`nearMissLimitMinutes`** 키를 1개 추가한다.

- 값 = **8** — **새로 만든 수치가 아니다.** `balance/balance-sheet.md` §5가 이미 `alignment_nearmiss_residual_max_min: 8` / "근접 실패대 4 < \|r\| ≤ 8" / "총 오차폭 8분"으로 선언한 값을 **그대로 옮겨 적는 것**이다.
- `owner: "balance"` — 기존 `residualLimitMinutes`(4)와 같은 소유.
- 대상 파일 2곳(미러 계약상 쌍으로 움직인다): `_workspace/current/systems/data/t0/tools.json`, `unity/Unknown/Assets/_Project/Data/Tables/tools.json`.
- **힌트 레인 파일(`hints.json`·`hints.meta.md`·`hint-system.md`·`T0GameSession.cs`)은 건드리지 않는다.**

**왜 이것인가**
1. S3이 실측으로 증명한 유일한 탈출구(전부-아니면-전무 판정 대신 **근접도 신호**)를 본 프로젝트는 이미 §5에 설계해 두었는데, **데이터 미러가 없어 존재하지 않는 것과 같다**(O3 vs O4).
2. 밸런스 designer 계약: *"Every number has a band, a measurement method, and a **data-mirror path**"* / *"numbers change in data tables, never in code"*. 지금은 밴드는 있고 미러가 없다 — 계약 위반 상태의 해소다.
3. 수치 효과를 발명하지 않는다. 값·의미·경계는 전부 기존 [TARGET] 선언을 옮긴 것이고, **밴드 자체를 넓히거나 좁히지 않는다**(A6 보류 판정 준수).

**정직한 한계 [OBSERVED]**: O11 — 현재 `_Project`의 어떤 `.cs`도 `residualLimitMinutes`조차 읽지 않는다. 따라서 이 변경만으로 **런타임 동작은 바뀌지 않는다.** 이것은 "근접 피드백을 켜는 변경"이 아니라 **튜닝 표면과 검증 대상을 만드는 변경**이며, 실제 소비는 §5 의존성(systems 레인)이다. 완료 보고에 이 문장을 반드시 포함할 것.

### Acceptance tests

문서/데이터 단계에서 **지금 실행 가능**한 것만 적는다.

| id | 기준 | 방법 (실행 가능) | 통과 조건 |
|---|---|---|---|
| **AT-B01-1** | 키가 두 미러에 **동일하게** 존재한다 | `node -e` 로 두 `tools.json`의 `knobs.nearMissLimitMinutes` 를 읽어 deep-equal 비교 | 두 파일 모두 `{value:"8", owner:"balance"}`, 불일치 0건 |
| **AT-B01-2** | 값이 **밸런스 시트 선언값과 일치**한다(발명 아님 증명) | `grep -n "alignment_nearmiss_residual_max_min" _workspace/current/balance/balance-sheet.md` 출력의 정수와 knobs 값 문자열 비교 | 둘 다 `8` |
| **AT-B01-3** | 밴드 **순서 불변식**이 깨지지 않는다 | `residualLimitMinutes(4) < nearMissLimitMinutes(8)` 산술 검사 | true. 같거나 역전이면 FAIL |
| **AT-B01-4** | 기존 knobs가 **변하지 않았다**(스코프 증명) | 변경 전/후 `knobs` 객체에서 새 키 1개를 뺀 나머지를 JSON 정규화 후 sha256 비교 | 해시 동일. 특히 `idleHintOfferSeconds`·`hintOfferCooldownSeconds` 바이트 불변(= OMP 레인 무간섭 증명) |
| **AT-B01-5** | 테이블 영수증이 재생성·일치한다 | `systems/data/t0/tables-receipt.json` 의 tools 항목 재계산 | receipt 와 실제 파일 sha 일치. 불일치 시 FAIL(fail-closed) |
| **AT-B01-6** | 캠페인 게이트가 여전히 초록 | `node _workspace/current/planning/validate-campaign.mjs` | `summary.verdict = PASS`, 검사 수·PASS 수 변화 0 (이 변경은 campaign.json 을 건드리지 않으므로 **집계가 바뀌면 그 자체가 FAIL**) |
| **AT-B01-7** | 런타임 회귀 0 | `T0PlayModeTests` / `T0CaseThreadTests` 기존 스위트 | 변경 전과 동일한 결과. 새 키를 읽는 코드가 없으므로 **행동 변화 0이 기대값**이다 |

> 측정 밴드(첫 시도 통과율, 근접대 진입률 등)는 **여전히 `null`이다.** 이 변경은 그 값을 채우지 않으며, 채운 척해서도 안 된다(`alignment_first_try_pass_rate_target: null` 유지).

---

## 5) 다른 레인 의존성

| 대상 레인 | 요청 | 차단 여부 |
|---|---|---|
| **game-systems-designer** | (a) `nearMissLimitMinutes` 를 실제로 소비하는 정합 판정 경로 구현 RFC — 근접대에서는 **방향/근접 신호만**, 값·정답 대상 노출 금지(§5 + S1 무차별 대입 방어). (b) O11: `residualLimitMinutes` 도 현재 미소비 — 두 키를 함께 배선해야 한다. (c) 스키마 문서 `systems/data-schemas/plates.md` 또는 tools 스키마에 새 키 등재 | **CHG-B-01 자체는 비차단.** 런타임 효과는 차단됨 |
| **OMP (힌트 idle 타이머 · 키보드 입력 통일)** | 무간섭. `hints.json` · `hints.meta.md` · `hint-system.md` · `T0GameSession.cs` **미편집**. AT-B01-4가 `idleHintOfferSeconds`/`hintOfferCooldownSeconds` 바이트 불변을 기계적으로 증명한다 | 충돌 없음 |
| **game-qa** | (a) A5(S2 유래) 를 exploit-register 관측 항목으로 등록: *"후보 수가 적은 확정에서 대입 시도 패턴"* — 수치 변경 트리거가 아니라 **관측 항목**이다. (b) 근접대 도달 시도 분포를 플레이테스트 로그에 남길 것 | 비차단 (n=0이라 어차피 미측정) |
| **game-economy-designer** | 없음. `cost`·`achievementPenalty`·부식 흐름 전부 불변(O9) → 보상 흐름 영향 0이므로 ack 불필요 | 해당 없음 |
| **director** | S3의 대가("근접 지표가 난이도와 길이를 줄였다")를 **알고 있는 상태로** 근접대 모델을 유지하는지 확인. 밴드 폭 변경 판단은 T0 사람 검증 이후(§10.4 R-T0-1 와 같은 원칙) | 비차단 |
| **planner** | 없음. `campaign.json` 미편집 (AT-B01-6 이 기계적으로 증명) | 해당 없음 |

---

## 부록 · 조사 자기점검 (web-search-pipeline Quality Assurance)

- 모듈 로딩: general-web 로드 후 검색 실행 (파이프라인 MANDATORY 조건 충족).
- 출처 4곳 **전문 직접 정독**(검색 스니펫 인용 아님): S1·S2 = dukope.com 원문 페이지, S3 = gamedeveloper.com 원문, S4 = mobiusdigitalgames.com 원문.
- **귀속 교정 1건**: 검색 결과에서 Pope의 글처럼 보이던 무차별 대입 비판이 실제로는 포럼 사용자 `sherjilozair`의 댓글이었다 → S2 에 명시하고 **규칙 승격에서 배제**(A5 보류).
- **미확인으로 남긴 것**: Outer Wilds Ship Log / Rumor Mode 의 설계 의도를 진술한 **개발자 1차 출처를 찾지 못했다.** 검색 상위는 전부 팬 커뮤니티·모드 문서·학술 에세이였다. GDC 2020 세션은 존재만 확인했고 **본문 미정독** → Ship Log 관련 주장은 본 문서에 **쓰지 않았다**.
- 수치 발명 0건. 본 문서가 제안하는 유일한 값(8)은 기존 `balance-sheet.md` §5 선언값의 복사이며 AT-B01-2 가 이를 기계적으로 검증한다.
