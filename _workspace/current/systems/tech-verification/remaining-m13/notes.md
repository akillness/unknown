---
updated: 2026-09-12
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
---

# RFC-CX-016 남은 작업 마감 (M13) — 이월 3건 판정

[OBSERVED] RFC-CX-016 `[DECISION·미측정 마감]`이 지목한 **비차단 이월 3건**을 닫았다. 범위는 소형 수리 + 판정이며, 이 문서는 **정적 근거와 데이터 대조에 대한 기술 영수증**이다. 전수 테스트·빌드는 디렉터의 최종 실행 소관이고, 아래 신규 PlayMode 3건은 **작성 시점에 미실행**이다(Unity 프로젝트 락 — 병행 세션 점유). 사람 플레이테스트·재미·몰입·25분 예산은 이 회차에서도 **미측정**이다.

이월 목록의 출처: `production/task-manifest.md:137`, `production/decision-log.md:513-517`, `systems/tech-verification/native-playtest-m9/verification.md` §3–§4.

## 1. 자동 보존 단서 id 노출 — **수리** (D-M9-17 잔여)

- 증상 [OBSERVED]: 증거함(`overlay=="evidence"|"hypothesis"`)이 내부 clue id를 그대로 냈다. `T0GameSession.cs` 구 `:311` = `foreach(var id in Journal.State.AutoKeptClues)s.Body+="\n✓ "+id;` → 화면에 `✓ t0-b2-c1`. 스크린샷 증거: `native-playtest-m9/shots/92-notes-in-c1.png` 계열.
- 이월 사유였던 "clue 표시명 데이터 부재"는 **성립하지 않았다** [OBSERVED]. `Data/Tables/records.json`의 `clueIds`를 역인덱스로 만들어 대조한 결과 **단서 6건 / 소유 레코드 5건, 중복 소유 0건**이다:

  | clue | 소유 레코드 | `displayNameKo` |
  |---|---|---|
  | `t0-b1-c1` | `rec-handover-brief` | 인수 각서 |
  | `t0-b1-c2` | `rec-transfer-list` | 이관 목록 |
  | `t0-b2-c1` · `t0-b3-c1` | `rec-plate-standard-hub` | 당직실 표준판 |
  | `t0-b2-c2` | `rec-watchlog-bureau` | 근무 규정 필사본 |
  | `t0-b3-c2` | `rec-tide-ledger-bureau` | 기록국 조위대장 |

- 소유가 유일하다는 것의 근거는 데이터 우연이 아니라 **시뮬레이션 계약**이다 [OBSERVED]: `kept:` 사실은 `T0Simulation.Commit :92`에서 `command.CommandId=="Read"` 일 때 `data.Records[state.LoadedRecordId].ClueIds`로만 생성되고, `Reduce :135`가 그 `ProtectedClues`만 `kept:`로 적는다. 즉 모든 보존 단서는 **자기를 등재한 레코드의 판독**으로 생긴다 → 소유 레코드는 항상 파생 가능하다.
- 수리: **개수 집계가 아니라 파생 라벨**을 택했다(과제가 제시한 두 선택지 중 전자). 이월 사유가 무효화됐으므로 정보량을 줄일 이유가 없다 — 플레이어는 "무엇이 보존됐는지"를 매체명으로 읽어야 하고, 이는 D-M9-17 본 수리(`ReviewMediaName(sourceType)`)가 이미 세운 관례와 같은 방향이다.
  - `App/T0GameSession.cs` 신규 `KeptClueLines()` (`:208-224`): `Definition.Records.Values.FirstOrDefault(r=>r.ClueIds.Contains(clue))`로 소유 레코드를 찾고 기존 `Name(id)`(= `records.json` `displayNameKo`, `:74`)로 라벨화한다. **그룹 키는 레코드 id**이며 렌더 문자열이 아니다 — 소유 없는 단서가 이름 있는 그룹과 섞이지 않는다.
  - `App/T0GameSession.cs:311`: `foreach(var line in KeptClueLines())s.Body+="\n✓ "+line;`
  - `Resources/T0Strings.json` +4키(ko·en 모두): `keptClue` `{0}의 보존 단서` · `keptClueCount` `{0}의 보존 단서 {1}건` · `keptClueUnnamed` `보존 단서` · `keptClueUnnamedCount` `보존 단서 {0}건`. 기존 162키는 **바이트 불변**(json 재파싱 대조: 기존 키 전부 동일, 키 수 162→166).
- 렌더 결과 [OBSERVED — 실제 테이블 데이터로 대입 시뮬레이션]: T0 완주 상태에서
  ```
  ✓ 인수 각서의 보존 단서
  ✓ 이관 목록의 보존 단서
  ✓ 당직실 표준판의 보존 단서 2건
  ✓ 근무 규정 필사본의 보존 단서
  ✓ 기록국 조위대장의 보존 단서
  ```
  `t0-b\d-c\d` 정규식 검색 **0건**. `rec-plate-standard-hub`만 단서 2건이라 집계 분기를 실제로 타는 유일한 레코드다.
- 폴백 [OBSERVED]: 소유 레코드가 없는 단서는 `보존 단서` / `보존 단서 N건`으로 낸다. 현재 데이터에는 해당 사례가 없고(C1 서명·순찰은 `observed:`/`copy:`/`separated:` 네임스페이스를 쓴다) **방어용**이다. 1차 구현은 폴백 라벨을 그룹 이름으로 재사용해 `보존 단서의 보존 단서`를 만들었고, 위 대입 시뮬레이션에서 잡아 그룹 키를 레코드 id로 바꿨다.
- 용어 [DECISION]: 증거함 서두(`evidenceIntro`)는 같은 대상을 "자동 사본"으로 부른다. 새 줄은 이월 항목·decision-log·본 과제가 일관되게 쓰는 **"보존 단서"**를 따랐다. 사본(Read가 만드는 매체)과 단서(그 판독으로 보존되는 항목)는 층위가 다르므로 두 용어를 섞지 않고 병존시킨다. 통일이 필요하면 planner 어휘 회차 소관이다.
- 회귀 테스트: **추가하지 않았다**. 기존 테스트 중 `✓ `+clue id 형식을 단정한 것은 **0건**(`Tests/**` grep `AutoKeptClues|✓ |keptClue`)이며, 이 수리는 표시 문자열 변경이라 `T0CaseThreadTests.AssertNoDisclosure()`(사건 흐름 카드 한정)와 범위가 겹치지 않는다. 증거함 문자열 전용 단정은 다음 회차 후속으로 남긴다 [CARRIED].

## 2. t0-b2 objective 저작 지시문 노출 — **오류 이월 · 종결** (D-M9-11)

- [OBSERVED] **결함은 이미 닫혀 있었다.** 현재 `Data/Tables/beats.json`의 `t0-b2.objective` =
  `회로 지도에 오늘 밤 판독 가능한 범위와 "기록 밖" 구획을 직접 접어 표시하고 법1을 손으로 익힌다.`
  — 문장 1개. 문제의 둘째 문장 `안내 표시가 각 단계에 붙는다.` 는 **없다**(json 파싱 후 문자열 대조, `안내 표시가` 검색 0건).
- 해소 경로 [OBSERVED]: `worldview/term-audit-20260911.md:207-210`이 objective 산문에 병기된 튜토리얼 스캐폴딩 지시문 9곳(`t0-b2` 포함)을 지적 → **RFC-CX-012 B-14**가 제거 → **RFC-CX-013**이 `beats.json`에 재동기했다(`systems/rfc-cx-012-ack.md:48`, `systems/tech-verification/rfc-cx-013-tables-resync.md:66`). 커밋 `6dccbc6`이 `campaign.json`·`beats.json`을 함께 옮긴 마지막 변경이고, 두 파일 모두 워킹 트리에 미변경 상태다(`git status --porcelain` 0행).
- 판정 [DECISION, 디렉터 확인 2026-09-12]: M9 QA R2/R3의 `STILL-OPEN`과 직전 회차 이월 기록은 **재확인 없이 이어 적힌 오류 이월**이다. QA가 읽은 바이트는 그 시점 그대로 옳았고(`completeness-m9-review.md:180`이 "campaign.json 재-emit 후에는 결과가 달라질 수 있다"고 스스로 경계를 적었다), 그 뒤 재동기가 먼저 해결했다. **결함으로 닫는다.**
- 따라서 **`campaign.json` 수정 불필요** — 적용 지시를 남길 대상이 없다. **런타임 가드도 넣지 않았다**: 데이터가 이미 정본이고, 문장 단위 휴리스틱 필터는 `M9CoreTests.CaseObjectiveFollowsTheBeatsTableThroughTheDisclosureGuard`가 지키는 "저작 문면 그대로 표시" 계약을 약화시키는 임시방편이다. 기존 disclosure 가드(`CaseObjective`, `T0GameSession.cs:203`)는 **레코드 표시명 노출**만 막는 것이며 저작 지시문 검출기가 아니다 — 그 책임 경계를 넓히지 않는다.
- 같은 레인의 **잔존 항목**: `t0-b1.objective`는 레코드 표시명 2개(`인수 각서`·`이관 목록`)를 담고 있어 disclosure 가드가 고정 문구(`caseObjective`)로 폴백시킨다 → **저작한 t0-b1 목표는 화면에 한 번도 나오지 않는다**. 이것이 원래 한 묶음이던 "t0-b1 무스포일러 재작성"(planner)이며 여전히 열려 있다. 교정안·적용 절차는 [`planning/objective-copy-m13.md`](../../../planning/objective-copy-m13.md)에 남겼다(`campaign.json`은 병행 편집 중이라 직접 수정하지 않았다).
- 부수 관측(결함 아님) [OBSERVED]: `Resources/C1SignatureContract.json`의 `narrative.action`에 `이번에는 안내 표시가 붙지 않는다.`가 남아 있다. 렌더 경로를 전수 확인한 결과 패킷에서 화면으로 나가는 필드는 `narrative.objective`(순찰만, `C1GameSession.cs:68`) · `narrative.hints`(`T0GameSession.cs:314`) · `observations[].description` · `localization.*`뿐이고 **`narrative.action`을 읽는 코드는 0건**이다(`Assets/_Project` grep). 서명 비트의 사건 흐름은 `SignatureCaseThread()`가 `localization` 키로만 조립한다. 노출 없음 → 설계 필드로 둔다.

## 3. 눈금 선택기 휠 스크롤 미도달 — **차단 지점 없음 · 계측 한계로 판정** (+ 회귀 3건 추가)

- 원 관측 [OBSERVED]: `decision-log.md:517`, `native-playtest-m9/verification.md:55,117` — "페이지당 30행 중 ~8행 가시, 휠 무반응, 드래그로 도달"(`shots/49-drag-scroll`). D-M9-15 서술(`verification.md:90`)은 "포인터 휠이 검토 노트 리스트에 닿지 않아 되돌릴 방법이 없다"까지 적었다.
- 정적 조사 결과 [OBSERVED — 소스 열람] 휠 경로에 **차단 지점이 없다**:
  - `GraphicRaycaster`는 프로젝트 전체에 **1개**(`UI/T0Interface.cs:63`, Interface Canvas)다. `Presentation/T0CommitFeedback.cs:14`가 만드는 `sortingOrder=30` 캔버스에는 레이캐스터가 없고 잉크 마크도 `raycastTarget=false`이므로 상위에서 히트를 가로채지 않는다.
  - 콘텐츠 `ScrollRect`는 `Work Surface` 패널 **자신**에 붙는다(`T0Interface.cs:111-112`). `Panel()`(`:203-204`)이 만드는 `Image`는 `raycastTarget` 기본 `true`이고 이 패널만은 끄지 않는다(끈 것은 `Case thread` 카드 `:95,:97`와 M7 스킨 배킹 `:211`). `Viewport`/`Content`는 그래픽이 없지만 버튼 사이 빈 틈의 레이캐스트는 부모 `Work Surface` 이미지에 떨어지고, `ExecuteEvents.GetEventHandler<IScrollHandler>`가 그 위에서 `ScrollRect`를 찾는다.
  - `scrollSensitivity=30`(`:112`, 내비게이션은 `:88`)이며 `vertical=true`, `horizontal=false`다. `RectMask2D`는 `Viewport`에 있어 **자식** 그래픽만 클립하므로 `Work Surface` 히트에 영향이 없다.
  - `T0GameSession.cs:96-100`의 `RaycastAll` 가드는 **3D 서랍 포인터 전용**(UI 히트가 있으면 3D 픽을 포기)이고 스크롤 디스패치와 무관하다.
  - 입력 모듈은 `T0GameSession.cs:38`에서 `InputSystemUIInputModule` + `AssignDefaultActions()`로 배선되고, 명시적으로 null로 끊는 것은 `move`·`submit`·`cancel` **3개뿐** — `scrollWheel`은 남는다.
- 키보드 도달성 [OBSERVED — 소스 열람]: `T0Interface.Navigate()`(`:157-161`)가 활성 버튼 전체를 순환하고 `SelectFocus()`(`:167-179`)가 소유 `ScrollRect`를 초점 항목이 뷰포트 상단 40% 지점에 오도록 이동시킨다(`extent>0`일 때). **초점 이동이 스크롤을 끌고 오므로 휠 없이도 모든 항목에 도달한다** — interaction-rules §0-8 요건에 대해 휠은 실제 차단이 아니다.
- 판정 [DECISION, 디렉터 확인 2026-09-12]: M9의 "휠 무반응"은 **계측 도구의 한계**로 판정한다. 그 회차 입력은 cliclick 포인터 전용이었고 합성 키 이벤트가 Unity Input System에 닿지 않았음이 이미 기록돼 있다(`native-playtest-m9/verification.md:24`, `decision-log.md:508`) — 합성 휠이 닿지 않는 것은 **같은 경계의 같은 원인**이다. 직전 회차가 이를 "실제 UX 결함"으로 단정한 것은 근거를 넘은 서술이었다.
- **경계 — 과대 주장 금지** [OBSERVED]: 코드에 차단 지점이 없다는 것은 "차단이 없다"의 증거이며 **"실기기 휠이 동작한다"의 증거가 아니다**. 아래 3건 중 `RealMouseWheelReachesTheListThroughTheInputModule`이 통과하면 **입력 모듈 경로까지 검증됨**이라고 쓸 수 있고, 그래도 **사람이 실제 마우스 휠을 돌린 결과는 n=0으로 미측정**이다. 합성 입력으로 관측하지 못한 것을 "정상 동작"으로 승격하지 않는다.
- 추가한 회귀 — `Tests/PlayMode/ScrollReachabilityTests.cs` (신규 파일, `Tide.Tests.Play`). 대상은 T0 최장 리스트인 눈금 선택기다(`rec-plate-standard-hub` 위상 **180개**를 30개씩 6페이지 → 페이지 1의 32개 액션 + `overlay-back`; 버튼 최소 높이 48로도 한 뷰포트를 훨씬 넘는다):

  | 테스트 | 단정 |
  |---|---|
  | `PointerWheelOverTheWorkSurfaceScrollsTheLongestList` | 콘텐츠가 뷰포트를 넘김 · `scrollSensitivity>0` · 작업면 중앙 `RaycastAll` 히트 존재 · `GetEventHandler<IScrollHandler>`가 **그 `ScrollRect` 자신**으로 해석 · 휠 다운이 `verticalNormalizedPosition`을 top에서 내리고 휠 업이 되돌림 |
  | `RealMouseWheelReachesTheListThroughTheInputModule` | `InputSystemUIInputModule`이 존재하고 `scrollWheel`·`scrollWheel.action`이 바인딩돼 있음 · **실제 `Mouse` 디바이스 상태 이벤트**(position → position+scroll)가 리스트를 움직임 |
  | `KeyboardAloneReachesEveryTickRowWithTheScrollFollowing` | Tab만으로 페이지 1의 **모든 액션 id**에 초점이 닿고, 닿을 때마다 그 버튼 중심이 뷰포트 밴드 안에 들어옴(초점 스크롤 추종) |

- [OBSERVED] 위 3건은 **작성 시점 미실행**이다. Unity 배치는 프로젝트 락으로 동시 1개만 가능하고 병행 세션(M7 승급 → 전수 → 빌드)이 점유 중이라 디렉터의 **최종 전수 실행에 포함**하기로 조정했다(irc 합의 2026-09-12). 실패 시 스택을 받아 이 회차 소유 파일 안에서 수리한다.

## 경계

- [OBSERVED] 변경 파일: `unity/Unknown/Assets/_Project/App/T0GameSession.cs`, `unity/Unknown/Assets/_Project/Resources/T0Strings.json`, 신규 `unity/Unknown/Assets/_Project/Tests/PlayMode/ScrollReachabilityTests.cs`(+`.meta`), 신규 `_workspace/current/planning/objective-copy-m13.md`, 본 문서. 그 외 **무변경** — `production/*`·`qa/*`·`planning/campaign.json`·`Data/Tables/*`·`systems/data/t0/*`·`UI/T0Interface.cs`·M7 레인 파일 전부 손대지 않았다. 포매터·린터 0회. git 커밋·푸시 0회.
- [OBSERVED] `T0Simulation.cs`의 clue 표시명 경로는 **수정하지 않았다**. 소유 레코드 역인덱스가 표시 계층(`T0GameSession`)에서 완결되므로 시뮬레이션에 표시 관심사를 넣을 이유가 없었다(CLAUDE.md §9 — 연출은 시뮬 스냅샷을 읽되 쓰지 않는다).
- [CARRIED] 다음 인테이크: t0-b1 objective 무스포일러 재작성(planner, `objective-copy-m13.md`) · 증거함 보존 단서 문자열 전용 단정 · "자동 사본"/"보존 단서" 용어 통일 여부(planner) · 실기기 마우스 휠·IME·컨트롤러 검수 · 사람 플레이테스트(n=0).
