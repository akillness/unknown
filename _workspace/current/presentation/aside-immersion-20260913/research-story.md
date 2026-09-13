---
updated: 2026-09-13
cycle: 20260909-preproduction-c7
status: draft
supersedes: null
owner: game-synopsis-writer
---

> [OBSERVED] 독립 조사 시점의 보고서. 제안은 승인·구현·현재 런타임 검증을 뜻하지 않는다. 최종 채택/기각은 이 폴더 decision-and-implementation.md를 우선한다.

# T0 초반 몰입·스토리 전달 개선 — 개발자 1차 출처 연구 (읽기 전용, 저장소 미편집)

작성: 2026-09-13 / 역할 적용: `game-worldview-architect`(캐논 = 게이트) + `game-synopsis-writer`(비트는 반드시 gameplay hook + reward beat 보유, 로어를 대사로 신설 금지) / 방법: `deep-research` web-search 파이프라인(질의 변형 → 1차 출처 직접 fetch → 인용 보존)

## 0. 읽은 캐논 (좌표)

| 파일 | 참조 위치 |
|---|---|
| `_workspace/current/worldview/worldview-bible.md` | §2(매체 3종·남는 것/남지 않는 것 L.19-24), §3 6법 표(L.30-39), §4 인물표 L.106-114 |
| `_workspace/current/synopsis/t0-records.md` | §8.1 T0 공개 상한 B01·B02·B03(L.315-319), §8.2 상한 점검(L.323-332) |
| `_workspace/current/planning/campaign.json` | stage `T0`(minutes 25, zone `hub`) beats `t0-b1`/`t0-b2`/`t0-b3`, stage `C1` beats `c1-b1`~ |
| `_workspace/current/synopsis/narrative-flow-bridges.md` | §2 L.62-65 (`t0-b1`~`c1-b1` bridge-in / 완료 / 보류 3열) |
| `unity/.../App/T0OpeningSession.cs` | L.42-51 `BeginOpening`/`FinishOpening`, L.52-61 `OpeningScreen` |
| `unity/.../Resources/M5Direction.asset` | `firstShotSeconds 3.125`, `secondShotSeconds 2.875`, `firstTitle/firstCaption/secondTitle/secondCaption` |
| `unity/.../Resources/T0Strings.json` | `intro`, `openingMotto`, `openingMottoDetail`, `caseObjective` |
| `unity/.../App/T0GameSession.cs` | L.206 (opening 중 `CaseThread=null`), L.219 `CaseObjective(..., L("caseObjective"))` fallback |

## 1. 1차 출처 5건

### S1. Thomas Grip (Frictional Games) — "4-Layers, A Narrative Design Approach" (2014-04-29)
https://frictionalgames.com/2014-04-4-layers-a-narrative-design-approach/

- Layer 2 Narrative Goal: "The way to fix this is to give the player some sort of short-term narrative goal, one that is directly connected to the current gameplay. … It is no longer about 'doing stuff to get the story going', instead it is about 'doing stuff because of the story'."
- 그중 첫 유형이 Mystery: "The most obvious and simple is mystery; that there is something unknown you want find out about. It's pretty easy to have environmental assets that constantly reminds the player of this."
- Layer 3 Narrative Background — Emotionally Significant Assets: "There is a huge difference in finding 'standard knife A' and 'the murder weapon from a hideous crime'."
- Layer 4 Mental Modeling — Goal-focused Mystery: "the player should see the game world as a place where important clues are to be discovered. So whenever the player finds a new location they should instantly start thinking about what new things it can teach them about the mystery."
- 경고: "Many narrative games already have some degree of mental modeling, but in the worst way possible: collectables."

### S2. Thomas Grip (Frictional Games) — "Storytelling through fragments and situations" (2010-03-15)
https://frictionalgames.com/2010-03-storytelling-through-fragments-and-situations/

- "instead of forcing the player to take part of certain story elements (fragments) we have made sure to make the most important things are really obvious (and hard to miss) and the less important more hidden."
- 환경 파편의 범위 확장: "by interacting with the world the player can find out things, not just about the environment, but about the character too."

### S3. Kelsey Beachum (Mobius Digital → Obsidian) — GDC 2021 "Sparking Curiosity-Driven Exploration Through Narrative in *Outer Wilds*" (공식 슬라이드 PDF)
https://media.gdcvault.com/GDC+2021/beachum_gdc_2021(1).pdf

- 정의: "Starts with a question… that drives the player toward discovering an answer" / "The player taking action to try to answer questions they have about the world around them"
- 방법: "What specific questions do we want the player to ask? • These are what guides the player's journey" — 그리고 각 초기 질문에 **물리적 앵커 1개**를 붙인 표(예: "Why are we in a time loop? → Nomai statue in museum activating and syncing with the player's memory").
- "Big, exciting, interesting events that we SHOW rather than TELL" / "text generally doesn't deliver our biggest initial hooks … and when it DOES deliver a hook, it can only do so when the player chooses to interact with it."
- Push vs pull: "we minimized the frequency and quantity of information the player was forced to receive through text — Limited to very start and very end of game. … Instead, we make that info available, signal its availability, and let the player decide when to access it."

### S4. Lucas Pope — *Return of the Obra Dinn* 개발로그 #734 (2017-08-11)
https://dukope.com/devlogs/obra-dinn/tig-30/

- 알파 실패 진단: "The main issue is that the game not only doesn't hold your hand, it breaks both arms at the wrist and zip-ties your pinkies together. The story itself is relayed in such a limited way, and the player is given so little guidance, that it's natural to A) get totally lost and B) not care."
- 해결 ① The Book: "The Book takes over all the duties of the manifest in addition to explicitly listing chapters and every death in the entire game, **initially as blank pages**. … The linear layout also makes explicit the flow of time".
- 해결 ② 상호작용 대상 축소: "there's no need to pick up any items from the ship. That solves a major 'item-importance' design problem".
- 해결 ③ 검증 마찰: 3건 단위 확정 — "The game now tells you when any set of 3 fates are correct. … Cheating one fate is easy but cheating three fates requires just enough work".

### S5. inkle (Jon Ingold) — "The Pillars of the Game" (2015-12-16)
https://www.inklestudios.com/2015/12/16/pillars.html

- 확정 축이 아주 적다: "It's about a partnership. Two characters are better than one, *especially* if your game is about dialogue!" — 초반 기둥을 메커니즘이 아니라 관계로 잡는다는 선언. (본 건에서는 보조 근거로만 사용; T0는 무대화 NPC 0명이라 직접 적용 불가.)

## 2. 현 오프닝의 실측 상태 (판정 근거)

- 오프닝 총 길이 6.0초(`firstShotSeconds 3.125` + `secondShotSeconds 2.875`), 2컷.
- 카피 전문(디코딩): 1컷 "기록 앞에서 / 원본을 펼쳐 내용을 살펴봅니다." · 2컷 "직접 확인하고, 기록하기 / 도구로 확인한 결과를 근거와 함께 기록합니다." + 항상 붙는 "관찰 · 시험 · 기록 / 자료를 살피고, 조건을 시험하고, 근거를 기록하세요."(`openingMotto`·`openingMottoDetail`)
- 즉 **오프닝 4문장이 전부 동사 설명**이고, 세계·질문·이해관계가 0건이다. 시작 화면 `intro`도 "21:00. 인수 각서와 이관 목록을 살핀 뒤, 당직실의 기록을 대조하세요."로 절차문이다.
- 반면 `narrative-flow-bridges.md` §2의 비트 이음 문장(L.62-65)은 이미 질문 구동형으로 잘 써 있다 → **결함은 비트 전환이 아니라 비트 이전(오프닝·시작 화면) 구간에 국한**된다.
- `T0GameSession.cs:206`은 오프닝 중 `CaseThread=null`이라 "오늘 밤 무엇이 미해결인가"를 보여줄 유일한 표면이 오프닝에서 꺼져 있다.

## 3. 적용 원칙 (출처 → 현 프로젝트 번역)

| # | 원칙 | 출처 | T0 번역 |
|---|---|---|---|
| P1 | 짧은 **서사 목표**를 현재 플레이 구간에 직접 묶는다 — "doing stuff because of the story" | S1 Layer 2 | 오프닝 2컷을 "동사 설명"에서 "오늘 밤 남길 것 / 목록 밖에 있는 한 점"으로 재프레이밍. 동사 설명은 `caseThread`·`hints`가 이미 소유한다 |
| P2 | 질문을 먼저 정하고 **질문마다 물리 앵커 1개**를 붙인다 | S3 | T0 초기 질문 2개 = ①"이 판은 왜 목록에 없나" → 앵커: 인쇄 3줄 vs 실물 6점 / ②"기록이 비었다면 아무 일도 없던 것인가" → 앵커: 압력 곡선의 평평 3구간 |
| P3 | 훅은 **텍스트로 밀지 말고 보여준다**; 정보는 열어두고 당기게 한다 | S3 | "미봉인 1점" 표시등·공란 기입란·평평 구간을 화면에 먼저 두고, 설명 문장은 줄인다 |
| P4 | 미해결의 **형태**를 초반에 명시한다(빈 페이지 목차) | S4 The Book | `사건판`/`caseThread`를 오프닝 직후 즉시(빈 항목 포함) 노출 — 단 항목명은 상한 준수 |
| P5 | **중요한 것은 놓칠 수 없게, 덜 중요한 것은 숨긴다** | S2 | `tl-r4` 공란 기입란·미봉인 표시등 = 놓칠 수 없게 / 부식 무늬·회선 3개소 = 숨김 유지 |
| P6 | 소품에 **정서적 무게**를 준다(도구가 아니라 사물) | S1 Layer 3 | 판 #0을 "슬롯에 올릴 대상"이 아니라 "내가 여기 둔 것"으로 호명 |
| P7 | 초반 상호작용 대상 수를 **줄인다** | S4 | T0 오프닝 직후 클릭 가능 대상을 작업대·서랍·목록으로 제한(현 설계와 이미 부합, 회귀 방지용 기준으로 둘 것) |
| P8 | 수집물형 mental model은 **역효과** | S1 Layer 4 경고 | "N/6 열람 완료" 식 진척 카운터를 주 문구로 쓰지 않는다(현 `intakeProgress`가 이 위험에 가깝다) |

## 4. 현 자료의 gap (사용자/소유 레인 판단 필요)

| G | 내용 | 소유 |
|---|---|---|
| G1 | **T0 초기 질문 목록이 캐논 어디에도 없다.** S3가 요구하는 "플레이어가 묻기를 바라는 구체적 질문 + 앵커" 표는 `timeline.md` §7 상한표(=금지 목록)와 `campaign.json`의 `inference`(=비트 종료 후 추론)만 있고, **비트 시작 시점의 질문**은 `t0-b3` completion의 "정전이라면 왜 대장 수치는 살아 있는가" 1건뿐이다. T0 진입 시점(0~5분)에 쓸 수 있는 질문은 0건 | worldview + synopsis |
| G2 | 오프닝 카피의 캐논 소유자가 불명확하다. `M5Direction.asset`의 4개 문자열은 `presentation` 산출물처럼 보이는데 `t0-records.md`·`narrative-flow-bridges.md` 어느 곳도 이 문자열을 인용·상한 점검하지 않는다 → **상한 자기검사 사각지대** | presentation ↔ synopsis 경계 |
| G3 | `T0Strings.json`의 `caseObjective` = "결손 4시간의 양 끝을 두 기록으로 고정"이 **fallback 문자열**이다(`T0GameSession.cs:219`). 현재는 `t0-b1`·`t0-b2` objective가 레코드명을 포함하지 않아 fallback이 발동하지 않지만, **어떤 pre-`t0-b3` 비트 objective가 레코드명을 담는 순간 "결손 4시간"이 B03 이전에 노출**된다. 상한 위반 경로가 코드에 열려 있다 | systems + worldview |
| G4 | 몰입도·이해도 실측 n=0. `t0-records.md` §11이 스스로 "T0 사람 검증(H-2 목표 설명 10/12)에서만 확인된다"고 적는다. 본 연구는 **수치 개선을 예측하지 않는다** | QA |
| G5 | S4의 "빈 페이지 목차" 패턴을 적용하려면 사건판이 T0 시작 시점에 무엇을 보여줄지 정해야 하는데, 공개 상한 B01은 "결손"을 `t0-b3`까지 금지한다 → **빈 항목의 라벨을 무엇으로 쓸지**가 미결 | synopsis + worldview |

## 5. 바로 쓸 수 있는 문구 (캐논 무훼손, 기존 작품 고유 장치 미복제)

기준: B01 상한 안에서만 — 판 #0은 "번호만 적힌 미봉인 염판이며 서린 자신이 숨긴 것"까지 허용, **서명란의 이름 "서린"·12년 전 인과·결손의 존재는 전부 금지**. 아래 전부 `t0-b1` 진입 이전/직후 구간용.

### A안 — 오프닝 2컷 대체 (권장, P1·P2·P6)
> **1컷 / 제목: 마지막 당직**
> 오늘 밤이 지나면 이 방의 기록은 전부 남의 손으로 넘어간다.
>
> **2컷 / 제목: 목록과 서랍**
> 인쇄된 목록은 세 줄이고, 서랍에는 그보다 한 점이 더 있다.

(동사 설명 `openingMotto`는 삭제하지 말고 `hints`/`caseThread` 쪽으로 옮기는 것을 제안 — S3의 push→pull 전환.)

### B안 — 시작 화면 `intro` 대체 (절차문 → 상황문, P3)
> 21:00. 인수 각서와 이관 목록이 작업대에 놓여 있고, 사물 서랍은 아직 열지 않았다.

### C안 — `t0-b1` → `t0-b2` 비트 전환 보강 1문장 (기존 bridge-in L.63 앞에 덧대는 형태, P2 질문 앵커)
> 목록 밖의 한 점을 손에 쥔 채, 이 방이 오늘 밤 무엇까지 읽을 수 있는지부터 알아야 한다.

### D안 — `t0-b2` → `t0-b3` 전환 보강 1문장 (P5: 중요한 것을 놓칠 수 없게)
> 접어 둔 세 구획은 읽지 못하는 자리다. 읽을 수 있는 자리에서 무엇이 어디까지 이어지는지는 아직 아무도 보지 않았다.

**금지 확인**: 위 5개 문장에 "결손"·"4시간"·"정전"·"한도연"·"대조의 밤"·서명란 이름·인과 서술 0건. "한 점 더 있다"는 B01이 명시 허용한 `tl-r4` 차이(인쇄 3줄=실물 5점 vs 서랍 6점)의 관측 서술이며 판 #0의 각인 내용·번호대 일치(`c1-b2` 소유)를 말하지 않는다.

## 6. 채택 / 보류 / 기각

| 항목 | 판정 | 근거 |
|---|---|---|
| P1 서사 목표로 오프닝 재프레이밍 (A·B안) | **채택 제안** | S1 Layer 2. 현 카피 4문장이 전부 동사라 캐논 대비 전달 손실이 명백하고, 상한 위반 없이 교체 가능 |
| P2 T0 초기 질문 2개 + 앵커 표 신설 | **채택 제안(단, worldview RFC 필요)** | S3. 단 G1대로 캐논에 없는 신규 산출물 → `game-synopsis-writer` 운영원칙 1("로어를 대사로 몰래 만들지 않는다")에 따라 RFC로 올려야 한다 |
| P5·P6·P7 (놓칠 수 없게 / 정서적 소품 / 대상 축소) | **채택 제안** | S2·S1·S4. 현 `t0-b1` 설계와 이미 정합하므로 회귀 방지 기준으로 명문화만 하면 된다 |
| P4 빈 목차(사건판 조기 노출) | **보류** | G5 미결 — 빈 항목 라벨이 B01·B03 중 어느 쪽을 건드리는지 확정 전에는 진행 불가 |
| G3 `caseObjective` fallback 경로 | **보류(결함 후보로 보고)** | 현재 실제 노출 0건이므로 결함 단정 불가. systems 소유이며 현재 OMP가 UI resource 수정 중이라 접촉 금지 |
| 오프닝 길이 6.0초 조정 | **기각** | 근거 없음. S3·S4 어디에도 길이 수치 권고가 없고 실측 n=0이다. 수치 변경은 발명이 된다 |
| "이해도 N% 개선" 류 예측 | **기각** | G4. 본 연구는 몰입도 수치를 산출하지 않는다 |
| 기존 게임 장치 이식(시계·회중시계·챕터 책·시간 루프 등) | **기각** | 요청 제약. S4의 Book은 *패턴*(미해결의 형태 명시)만 취하고 장치·문구는 취하지 않는다 |

## 7. 테스트 (스포일러 누출 포함)

| T | 대상 | 방법 | 통과 조건 |
|---|---|---|---|
| T-1 | 오프닝·`intro` 신규 문구 | 금지 문자열 grep: `결손|4시간|정전|한도연|대조의 밤|서린` | 오프닝·`intro` 범위에서 0건 (`t0-records.md` §8.2 금지 문자열 검사와 동일 방식) |
| T-2 | 상한 귀속 | 신규 문장마다 B01/B02/B03 중 어느 행이 허용하는지 1:1 표기 | 미귀속 문장 0건 |
| T-3 | **G3 회귀** | `T0GameSession.CaseObjective` 에 pre-`t0-b3` 비트 objective + 레코드명 조합을 넣은 EditMode 케이스 추가(기존 `M9CoreTests.cs:60` 패턴 확장) | fallback이 발동해도 `caseObjective`가 `t0-b3` 이전 화면에 나타나지 않음 |
| T-4 | 용어 신설 0건 | 신규 문구의 모든 고유명을 `worldview/glossary.md`와 대조 | 미등재 고유명 0건 (A~D안은 고유명 0개 사용) |
| T-5 | 사람 검증 | `t0-records.md` §11이 지목한 "H-2 목표 설명 10/12" 계약 그대로 | 교체 전/후 **비교값을 만들지 않고**, 교체본 단독 통과 여부만 기록 |
| T-6 | 이음 문장 무결 | `narrative-flow-bridges.md` §2 재대조 절차(L.145) 재실행 + `validate-campaign.mjs` | `checks/pass` 일치, `fail 0` |

## 8. 이 문서가 증명하지 않는 것

- 저장소를 편집하지 않았다. 위 문구는 **제안**이며 어느 파일에도 반영되지 않았다.
- 실행·빌드·플레이 검증 0건. `validate-campaign.mjs`도 이 세션에서 돌리지 않았다(공유 워크트리 + OMP 동시 작업 회피).
- S1~S5는 전부 개발자 1차 출처지만, 그들의 결론이 이 게임(비전투·문서 추리·1야간)에 전이된다는 것은 **검증되지 않은 유추**다.
