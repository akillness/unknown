---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-qa
---

# M9 완성도 hop (RFC-CX-011) — 독립 QA 검토

**판정 요약**: S1 0건 → PASS 차단 없음. **S2 1건**(C1 단계에서 `원문 열기`가 원문을 열지 않고 내비게이션 상태를 오염 — D-M9-01) → 계약 위반이므로 hop 완료 판정은 디렉터가 S2 수리 여부를 정한 뒤 내리기를 권고. S3 10건은 마감 품질·보고 정합·테스트 강도 항목이다.

검토 조건: 기준 HEAD `6a1a761`, 미커밋 working tree. READ-ONLY(소스·에셋 수정 0건, Unity 배치 실행 0회, 포매터/린터 0회). 영수증은 XML/로그 재해시·재파싱으로 대조했다(python hashlib/re). 코드 판단은 정적 열람이며 실행 관측이 아닌 것은 `[INFERENCE]`로 표기한다.

## 0. 검토 범위 확정

- [OBSERVED] `git status --porcelain --untracked-files=all -- unity/`: 수정 14 · 신규 6(+`.meta` 7). 신규 = `Art/Candidates/m8-review-card/CardPaper.png`, `Editor/M8ReviewNotesProjectBuilder.cs`, `Presentation/M8ReviewNotesProfile.cs`, `Resources/M8ReviewNotes.asset`, `Resources/M8ReviewVfx.json`, `Tests/EditMode/M9CoreTests.cs`.
- [OBSERVED] `git diff HEAD --stat -- unity/`: 14 files, +416/−46.
- [OBSERVED] `git diff HEAD --stat -- unity/Unknown/Assets/_Project/Data/Tables/` → 출력 0줄. **Tables/* 바이트 불변.**
- 범위 밖(병행 세션 편집 중): planning/·synopsis/·worldview/·campaign.json. 이 문서는 그 파일들을 읽기만 했다.
- [CARRIED] `Tests/PlayMode/M5DirectionPlayModeTests.cs` 변경(테스트 개명 + `reduced-motion` 클릭 제거)은 RFC-CX-011 파일 소유 목록 밖의 선행 세션 reduced-motion fix다(decision-log.md:408). 아래 §3 O-3에 관측만 남긴다.

## 1. 불변식 (CLAUDE.md §9)

| 항목 | 판정 | 근거 |
|---|---|---|
| 1-a 저장 필드명 변경 0건 | **PASS** | [OBSERVED] `git show HEAD:…/Save/JournalSave.cs`의 `["hintLevelUsed"]=new JObject()` → 현재 `JournalSave.cs:16` `["hintLevelUsed"]=hintLevels==null?new JObject():JObject.FromObject(…Where(p=>p.Value>0)…)`. 키 이름 동일, 신규 키 0, 제거 키 0. `Encode` 시그니처는 선택 인자 `IReadOnlyDictionary<string,int> hintLevels=null` 추가만(`JournalSave.cs:12`). `Decode` 무변경. 스키마 `systems/data-schemas/save.md:64` `hintLevelUsed: map<beatId,int>`와 형식 일치 |
| 1-b 연출 코드의 시뮬 상태 쓰기 0건 | **PASS** | [OBSERVED] `ReviewNotesSession.cs:86-222` 신규 코드는 `overlay`/`document`/CanvasGroup alpha/세션 필드만 쓴다. `Journal.Submit`·`Simulation.Commit` 호출 0건(grep). `T0ReviewNotesInterface.cs` 변경분은 UI 트리만. `T0GameSession.cs:215-228 RequirementMet`·`:231-247 PreviewDifferenceSentences`는 `PuzzleState` 읽기 전용 순수 함수. `Simulation.Preview`(`T0Simulation.cs:101-105`)는 원본 상태를 변경하지 않는다 |
| 1-c 밸런스/연출 수치 하드코딩 신규 0건 | **PASS**(관측 1) | [OBSERVED] 전환 타이밍은 `Resources/M8ReviewVfx.json`(open 180/close 140/select 100/question 120/reduced 0)에서 `ReviewVfxMs`(`ReviewNotesSession.cs:201-211`)가 읽는다. 유휴 힌트 임계는 `tools.json knobs.idleHintOfferSeconds`(`T0GameSession.cs:74`), `SnapshotInterval`은 `Resources/SavePolicy.json:2`(`T0GameSession.cs:66`, 부재 시 기존 기본 200). 관측: `T0ReviewNotesInterface.cs:65-69`의 패널 색 2종·`120*scale` 높이는 기존 T0Interface 관례(`Color.white`·`190*scale`)와 같은 UI 레이아웃 상수이며 밸런스/타이밍 값이 아니다(O-4) |
| 1-d Data/Tables/* 바이트 불변 | **PASS** | [OBSERVED] §0 diff stat 0줄 |
| 1-e 생성 에셋 runtimeEligible/runtimeApproved false 시작 | **PASS** | §5 참조 |

## 2. 계약 이행

| 슬라이스 | 판정 | 근거 |
|---|---|---|
| S-A 힌트 재열람 단계 보존 | PASS | [OBSERVED] `OpenOverlay`(`T0GameSession.cs:116`)에서 `hintLevel=0` 리셋 제거. `HintLevel`은 `hintLevels[CurrentBeat]`(`:68`) — 비트별 독립(hint-system.md §2 "단계는 비트별로 독립 유지") |
| S-A 저장 왕복 | PASS / S3 D-M9-10 | [OBSERVED] `SaveDocument`(`:126`)가 `hintLevels` 전달 → `Encode`. 복원은 `RestoreHintLevels`(`:67`)를 Initialize Decode(`:59`)·recovery slot(`:298`)·recovery-retry(`:345`)에서 호출. 새 슬롯은 `hintLevels.Clear()`(`:345`). 단계 상승 시 `QueueSave`(`:328-329`)는 `!SavePending` 조건 — pending 중 상승분은 다음 저장까지 미기록(D-M9-10) |
| S-A `warnsBeforeReveal` 데이터 구동 | PASS | [OBSERVED] `:328` `next["warnsBeforeReveal"]!=null?(bool)…:(int)next["level"]==3`. hints.json 9행 중 level 3만 `true`(t0-b1/b2/b3 h3). C1 합성 행(`signaturePacket`/`patrolPacket narrative.hints`)은 필드 부재 → level 3 진입 전 경고 폴백(H-R3 준수) |
| S-B two-step 기본 | PASS | [OBSERVED] `DefaultSettings()`(`:65`) `confirmMode:"two-step"`; Initialize(`:47`)가 사용. interaction-rules.md:54 정본 일치 |
| S-B 기존 settings.json 우선 | PASS | [OBSERVED] `:48` `if(File.Exists(settingsPath))…settings=SaveCodec.Decode(…)` 무변경 — 기존 파일이 기본값을 덮어쓴다 |
| S-C 프리뷰 diff | PASS(부분) / S3 D-M9-07 | [OBSERVED] `:332-340` confirm/preview 오버레이 T0 경로에서 `Simulation.Preview` 후보와의 차이를 `previewFactCitation/Copy/Generic/ReadCount` 템플릿으로 문장화 + `previewUndoNote`. C1 확정문(`SignatureConfirmationText`/`PatrolConfirmationText`)은 무변경. GDD §3.3 원칙1의 4요소(바뀌는 것·되돌림·영향 구역·근거 2종) 중 앞 2요소만 구현 |
| S-D 비공개 가드 데이터 구동 | PASS | [OBSERVED] `CaseObjective`(`:198-202`)는 `recordNames.Any(objective.Contains)` — 하드코딩 비트 목록 없음. 실데이터 대입(python): t0-b1 가드 적중 `['인수 각서','이관 목록']` → 폴백, t0-b2/b3 표시. `Name()`(`:71`)은 항상 `displayNameKo`이므로 EN 설정에서도 가드 대상 동일 |
| S-D guided 티칭 | PASS(계약 긴장 1) / S3 D-M9-06·D-M9-05 | [OBSERVED] `GuidedTeachingText`(`:203-213`) = tools.json `introBeatId` == CurrentBeat && beats.json `toolTeaching{tool,mode:"guided"}` → level 1 `sourceTextKo` + 잔여 술어 수. CircuitScreen(`:268`)·ReaderScreen(`:281`) 본문 상단 병기 |
| S-E1 질문은 누를 때만 갱신 | PASS | [OBSERVED] `ShowReviewQuestion`(`ReviewNotesSession.cs:130-144`)만 `reviewShownQuestion/Structure/Summary`를 쓴다. `ReviewNotesScreen`(`:272-315`)은 읽기만 하고 `staleQuestion`(`:281`) 시 라벨만 교체(`:286`). 타이핑 콜백 `Changed`(`:284`)는 draft/status만 변경. PlayMode `QuestionOnlyUpdatesWhenExplicitlyRequestedAndStaysPinnedAcrossLinkChanges` Passed(playmode-final.xml) |
| S-E2 원문 열기→닫기 복귀 · 이탈 시 해제 | **S2 D-M9-01**(C1) / T0 PASS / S3 D-M9-02 | [OBSERVED] `OpenReviewSourceOriginal`(`:146-156`) overlay=null·document=recordId·복귀 플래그·노드·초점 id 기억. `ResumeAfterReviewSourceOriginal`(`:159-171`) `overlay!=null||tool!=null||node!=reviewReturnNode` → 플래그 해제, `document==null` 관측 시 reviewNotes 재개 + `Interface.Focus`. T0에서는 PlayMode `OpeningASourceOriginalReturnsToNotesWithFocusOnThatSource` Passed. **C1 단계는 §6 D-M9-01** |
| S-E3 질문 순서 | PASS | [OBSERVED] `ReviewQuestionFor`(`:261-270`) 0개→1개→동일 원본→매체 1종(`:267-268` "서로 다른 매체에서도 이 해석을 확인할 수 있나요?" 정본 문구 일치)→대조. `planning/ai-native-m8-reference-application.md:51-56` 표 순서(출처 없음→같은 원본→독립 매체 부족→독립 매체 연결)와 일치. EditMode 2건 Passed |
| S-F reducedMotion 0ms 즉시 | PASS | [OBSERVED] `ReviewVfxMs`(`:201-211`) `ReducedMotion ? "reduced_motion_ms" : key` → 0 → `StartReviewPanelTransition` 즉시 alpha=1(`:113-120`), `CloseReviewNotes` `<=0`이면 `OpenOverlay(target)` 동기(`:104`). PlayMode `ReducedMotionCompletesNoteTransitionsImmediately` Passed |
| S-F 에셋/텍스처 부재 null 안전 | PASS | [OBSERVED] `Resources.Load` 실패 시 `reviewVfx=null`→0ms, JSON 파싱 실패 catch(`:206-210`). `ReviewProfile` null → `ReviewDirectionEnabled=false` → `CardTexture=null`(`:285`) → 인터페이스가 RawImage 생략(`T0ReviewNotesInterface.cs:32-38`) |
| S-F RawImage raycastTarget=false | PASS | [OBSERVED] `T0ReviewNotesInterface.cs:37` `image.raycastTarget = false`. 질문 패널 Text 2종도 `raycastTarget=false`(`:70-71`) |
| S-F 직전 오버레이 복원 | PASS(버튼) / S3 D-M9-02(Esc) | [OBSERVED] `OpenReviewNotes`(`:86-95`) `reviewPreviousOverlay=overlay`; `CloseReviewNotes`(`:97-109`) 닫힘 완료 후 `OpenOverlay(target)`(`:187`). Esc(`Cancel`→`Back()`)는 `T0GameSession.cs:119`에서 `overlay=null` 직행 |
| S-F 출처 선택 100ms | **S3 D-M9-03** | `select_ms` 소비처 0건 |
| S-G snapshotInterval 전달 | PASS | [OBSERVED] `new CommandJournal(Simulation,SnapshotInterval)` 2곳(`:42`,`:345`), `JournalSave.Decode(…,SnapshotInterval)` 6곳(`:58,:59,:296,:298×2,:345`). `CommandJournal.cs:24` 기본 200과 SavePolicy 200 동일 → 동작 불변 |
| S-G 문서 열람 중 유휴 제안 정지 | PASS | [OBSERVED] `:74` 게이트에 `document==null` 추가 |
| S-H 카드 임포트 게이트 | PASS | §5 |
| 공유 계약 `OpenReviewNotes()` | PASS | [OBSERVED] `T0GameSession.cs:171` 액션이 `OpenReviewNotes`(`ReviewNotesSession.cs:86` public) 호출 |

## 3. 테스트 적정성

| 항목 | 판정 | 근거 |
|---|---|---|
| 기존 어서션 약화 0건 (T0PlayModeTests) | PASS | [OBSERVED] diff는 3개 테스트에 `Assert.AreEqual("preview",…)`+`preview-next` 단계 **추가**만. `ThreeConfirmationModesRequireTheirOwnGesture`(모드 명시 설정) 무변경 |
| 기존 어서션 약화 0건 (T0CaseThreadTests) | PASS(1건 S3) | [OBSERVED] 확정 3지점에 preview 단계 추가; `AssertRenderOnly(0,false)` preview 단계에도 추가(강화). `AssertCard` 목표줄이 리터럴 → `ExpectedObjective()`(프로덕션 `CaseObjective` 호출)로 교체 → **D-M9-08** |
| AssertNoDisclosure 유지 | PASS | [OBSERVED] `T0CaseThreadTests.cs:179-197` 무변경, `AssertCard`에서 `if(checkDisclosure)AssertNoDisclosure()` 유지, 금지어 10종 그대로 |
| 신규 EditMode 테스트가 행동을 단정 | PASS | [OBSERVED] `M9CoreTests.cs`: (a) 필드명·미공개 비트 제외·`Decode` 재생 무결성, (b) 기본값, (c) CiteToBoard/ReadOriginal 문장 + 항등 diff 0건, (d) 실데이터로 가드 적중/청정/미배선/미지 비트 4분기. `ReviewNotesTests` 2건: 우선순위·null/빈 매체 처리 |
| 신규 PlayMode 테스트가 행동을 단정 | PASS | [OBSERVED] 질문 미표시→표시→고정→'이전 질문'→재갱신, 원문 열기→닫기→복귀+초점 id, reduced-motion 즉시 완료 + 직전 오버레이 복원 |
| 커버리지 공백 | 기록 | (1) C1 단계에서의 `원문 열기` 미검사(D-M9-01 발견 경로). (2) `RestoreHintLevels` 런타임 왕복(세션 재시작 후 HintLevel 복원) PlayMode 미검사 — EditMode Encode/Decode만. (3) Esc 경로 닫힘 미검사(D-M9-02). (4) guided 티칭 헤더 표시 조건(introBeat 일치/불일치) 미검사 |
| 영수증의 테스트 존재 | PASS | [OBSERVED] editmode-final.xml에 M9CoreTests 5건·ReviewNotesTests 신규 2건 Passed; playmode-final.xml에 ReviewNotes 신규 3건·T0CaseThread·FullInputRun 2건·M5 개명 테스트 Passed. 소스 mtime(10:53–11:16Z) < 테스트 start-time(13:48Z~), `T0Runtime.asset` mtime 13:53:47Z < editmode-final 13:55:16Z → **-final 실행은 beats 배선 상태를 검사했다** |

## 4. 영수증 정합

방법: `python hashlib.sha256(file)` vs `verification.json.nativeRuns[].sha256`; `<test-run>` 속성 re 파싱.

| 파일 | sha256 일치 | XML testcasecount/passed/failed/skipped | result | start-time |
|---|---|---|---|---|
| editmode.xml | ✓ `bc7221c2…` | 53/53/0/0 | Passed | 13:48:27Z |
| playmode.xml | ✓ `bedc69d1…` | 69/68/0/1 | Skipped:Ignored | 13:48:50Z |
| boot.xml | ✓ `3532843b…` | 1/1/0/0 | Passed | 13:52:03Z |
| editmode-final.xml | ✓ `57c41e0a…` | **53/53**/0/0 | Passed | 13:55:16Z |
| playmode-final.xml | ✓ `b9a1ac31…` | 69/**68**/0/1 | Skipped:Ignored | 13:55:36Z |
| boot-final.xml | ✓ `21563265…` | **1/1**/0/0 | Passed | 13:56:29Z |

- [OBSERVED] playmode skipped 1 = `T0BootSceneTests.SerializedBootLoadsHubAndInitializesVisibleStartScreen`(전용 인자 필요) — boot-final.xml 1/1이 이를 보완. verification.md 표와 일치.
- [OBSERVED] build-mac.log: `T0_MAC_BUILD Succeeded bytes=354233020` 1회, `error CS` 0건. `unity/Unknown/Builds/T0-mac/Unknown.app` mtime 2026-09-11 22:58:18 KST(=13:58Z, 테스트 이후).
- [OBSERVED] wire-beats.log `T0_M9_BEATS_WIRED beats`; import-card.log `M8_REVIEW_CARD_IMPORTED 1536x1024 runtimeApproved=false`.
- [OBSERVED] import-audit.json `card.sha256`=`65fc439b…` == provenance.json `output_sha256` == 실제 `assets/generated/2d/texture/m8-review-card-r01/image.png` sha == `unity/…/Art/Candidates/m8-review-card/CardPaper.png` sha (4자 일치). `M8ReviewNotesProjectBuilder.cs:14,23` 상수 해시 대조 게이트 동일 값.
- [OBSERVED] **불일치 1건(S3 D-M9-09)**: verification.md:37 「네이티브 실행 … PLAYER_ALIVE→TERMINATED」가 인용한 `player-smoke.log`는 31줄 전부 `[UnityMemory] Configuration Parameters` 블록이며 `ALIVE`/`TERMINATED`/씬 로그가 없다(`grep -il` exit 1). 12초 생존 주장을 이 영수증으로는 입증할 수 없다.

## 5. 리소스 경계

| 항목 | 판정 | 근거 |
|---|---|---|
| `M8ReviewNotes.asset runtimeApproved=false` | PASS | [OBSERVED] 파일 `runtimeApproved: 0`; `cardPaper` guid `69bf0559…` == `CardPaper.png.meta`; `m_Script` guid `0c5a7ae3…` == `M8ReviewNotesProfile.cs.meta` |
| provenance `runtimeEligible=false` | PASS | [OBSERVED] `runtimeEligible:false`, `commercialReleaseEligible:false`, `credits_actual:null`(0 기재 안 함), `postProcessing:none`, 요청 1024² / 실제 1536×1024 분리 기록 |
| 참조 입력이 allowlist 내 | PASS | [OBSERVED] 참조 1건 `assets/generated/2d/concept/space-hub-watchroom-mood.png` sha `a90bc9ca…` == `_workspace/current/concept/concept-first-m7-sources.json allowlist[0].sha256` == 실제 파일 sha. denylist(`unity/`, `docs/media/`, `assets/generated/3d/`, 비-allowlist 2d) 접촉 0. `generationAuthorizations[]`에 `m8-review-card-r01` 승인 항목 존재(authorizedBy: director under RFC-CX-011 S-H) |
| 프롬프트 해시 | PASS | [OBSERVED] `prompt.txt` sha == provenance `prompt_sha256`; 네거티브에 글자·표식·숫자 금지 명시(direction.md:90 준수) |
| 승격 경로 | PASS | [OBSERVED] `Tools/M8/Approve review card`(`M8ReviewNotesProjectBuilder.cs:45-51`)만 `runtimeApproved=true`. 런타임 `--m8-review-notes-diagnostic` 플래그는 M5(`--m5-direction-diagnostic`)·C1(`--c1-signature-diagnostic`) 선례와 동일 관례(O-5) |

## 6. 결함 목록

형식: `id · 등급 · 소유 레인` → 증상 / 재현 / 근거 / 권고.

### D-M9-01 · **S2** · SYS-M8 (ReviewNotesSession) — C1 단계에서 `원문 열기`가 원문을 열지 않고 `document`를 잔류시켜 다음 Esc가 검토 노트를 재개한다 `[INFERENCE — 코드 열람, 미실행]`

- 증상: T0에서 관찰한 record 출처는 C1(순찰/서명지) 단계에서도 `ObservedReviewSources()`에 남아(`ReviewNotesSession.cs:230-243`, 단계 필터 없음) `원문 열기 · <레코드>` 액션이 생성된다(`:306-310`). 누르면 `overlay=null; document=recordId`(`:153-154`). 그러나 `Render()`는 `overlay==null`일 때 `SignatureActive→SignatureScreen`, `PatrolActive→PatrolScreen`을 `document!=null→DocumentScreen`보다 **먼저** 분기한다(`T0GameSession.cs:169`). PatrolScreen은 `patrolPacket["observations"]`에서 `document`를 찾지 못하면 일반 순찰 화면을 그리고(`C1GameSession.cs:57-67`), SignatureScreen도 같다(`C1SignatureGameSession.cs:48-57`). 결과: 화면은 노트가 닫힌 C1 화면으로 보이고 원문은 없으며 `document="rec-…"`가 남는다. 이후 Esc → `Back()`(`T0GameSession.cs:119`) `document=null` → 다음 프레임 `ResumeAfterReviewSourceOriginal`(`:159-171`: overlay/tool/node 조건 충족)이 **검토 노트를 재개**한다 — 플레이어 의도(C1 화면에서 뒤로)와 다른 화면 전이.
- 재현: T0 3비트 완료 → `continue-c1`(또는 `continue-c1-signature`) → 도구바 `evidence` → `open-review-notes` → `review-open-record:rec-handover-brief` → (원문 미표시 확인) → Esc.
- 계약: RFC-CX-011 S-E "출처 원문 열기+검토 노트 복귀"; direction.md:34 "원문 보기를 닫으면 선택했던 출처로". C1에서는 열리지도 닫히지도 않는다. 기존 PlayMode `…continue-c1-signature…` 테스트는 C1 노트 열기와 signature 출처 집계만 확인하고 `원문 열기`는 누르지 않는다.
- 권고: `PatrolActive||SignatureActive`면 record 출처의 `원문 열기` 액션을 생성하지 않거나(`:306` 조건), `OpenReviewSourceOriginal`에서 조기 반환. C1 회귀 테스트 1건 추가.

### D-M9-02 · S3 · SYS-M8 — Esc 닫기 경로가 S-F 닫힘 전환·직전 오버레이 복원을 우회한다

- [OBSERVED] `WatchInput.cs:46` `Cancel`→`Back()`; `T0GameSession.cs:119` `if(overlay!=null){overlay=null;…}` — reviewNotes도 동일 경로. 버튼(`review-note-back`/`overlay-back` 액션, `ReviewNotesSession.cs:313-314`)은 `CloseReviewNotes`로 140ms 페이드 + `evidence`/`hypothesis` 복원, Esc는 0ms로 도구/셸 직행. 같은 "닫기"가 입력 장치에 따라 다른 목적지로 간다.
- 계약: direction.md:55-57 "닫기 140ms … 직전 도구에 초점", interaction-rules §0-8 키보드 단독 동등성 취지.
- 권고: `Back()`에서 `overlay=="reviewNotes"`면 `CloseReviewNotes()`로 위임하거나, Esc=도구 복귀·버튼=오버레이 복귀를 direction.md에 명문화.

### D-M9-03 · S3 · SYS-M8 + director(보고 정정) — `select_ms` 저작만 있고 소비처 0건; 보고가 4종 타이밍 구현으로 과대 서술

- [OBSERVED] `M8ReviewVfx.json:4 "select_ms":100`; `ReviewVfxMs(` 호출은 `close_ms`(`:104,:113`)·`open_ms`(`:113`)·`question_ms`(`:137`)뿐. 출처 연결 토글에 전환 없음. impl-sysm8.md:23·verification.md:20 "180/140/100/120ms" 문구는 100ms 구현을 함의한다.
- 계약: direction.md:49-51 "출처 선택 100ms 선택 윤곽·매체 표기 교체". RFC-CX-011 S-F 결정문은 열림/닫힘만 명시하므로 범위 결정은 디렉터.
- 권고: 구현하거나 JSON 키를 제거하고 보고 문구를 "180/140/120"으로 정정.

### D-M9-04 · S3 · systems(Sim 소유) — 완료 술어 10종이 `T0Simulation.Satisfied`(private)와 `T0GameSession.RequirementMet`에 이중 정의

- [OBSERVED] `T0Simulation.cs:177-200` vs `T0GameSession.cs:215-228`. 후자는 `default:return false`(전자는 throw). 술어 추가·수정 시 두 곳 동기화가 필요하고 어긋나면 티칭 헤더의 "남은 조건 N개"가 실제 완료 판정과 달라진다.
- 권고: Sim에 읽기 전용 `IsSatisfied(PuzzleState,CompletionRequirement)`를 공개하고 App 복제본 삭제(단일 출처).

### D-M9-05 · S3 · SYS-CORE(strings) + worldview(용어) — 플레이어 문자열에 개발 용어 "비트" 신규 노출

- [OBSERVED] `T0Strings.json teachingRemaining.ko` "이 비트의 남은 조건 {0}개". HEAD의 148 키 중 "비트" 포함 0건 → 신규 1건. `worldview/glossary.md:162` "비트 B01~B33 — 33개 서사 슬롯"은 synopsis 소유 내부 라벨. EN "conditions remain in this beat"도 동일.
- 권고: "이 단계의 남은 조건 {0}개" 등 §3-1 표시명 체계 안의 낱말로 교체(용어 판정은 worldview).

### D-M9-06 · S3 · director 판정 + systems — guided 티칭 헤더가 1단 힌트 원문을 무조건 표시하며 `hintLevelUsed`에 기록하지 않는다 (hint-system H-R8 긴장)

- [OBSERVED] `GuidedTeachingText`(`T0GameSession.cs:203-213`)는 `HintLevel`과 무관하게 `hints.rows[beat,level1].sourceTextKo`를 도구 패널 상단에 병기. `hint-system.md:73` H-R8 "항상 1단 표시"는 **플레이어가 켠 설정**이지 시스템 자동이 아님; `:72` H-R7 `hintLevelUsed`는 텔레메트리 — 1단 문장이 노출됐는데 기록은 0으로 남는다(텔레메트리 과소 집계). RFC-CX-011 S-D가 "1단 힌트+잔여 술어 집계 병기"를 결정했으므로 구현은 결정문대로다 — 결함이 아니라 **계약 정합 판정 대상**.
- 권고: (a) 티칭 헤더는 힌트 사다리와 별개 기능임을 hint-system.md에 등재하고 표시 문장을 힌트 행이 아닌 별도 `teaching` 문자열로 분리하거나, (b) 표시 시 해당 비트 `hintLevels`를 1로 기록. 둘 중 하나를 decision-log에 남긴다.

### D-M9-07 · S3 · SYS-CORE + director(이월 기록) — 프리뷰 문장이 GDD §3.3 원칙1의 4요소 중 2요소만 다룬다

- [OBSERVED] `T0GameSession.cs:339` = `previewChanged` + 차이 문장(citation/copy/readCount) + `previewUndoNote`. GDD §3.3 원칙1(`planning/gdd.md:101`) "바뀌는 것·되돌릴 수 있는지·**영향 구역**·**근거 2종**"; interaction-rules §5 R2-1(`:312`) 동일. 영향 구역·근거 2종 요약 없음. RFC S-C 결정문은 "차이를 문장화"이므로 나머지 2요소는 명시 이월이 필요하다.
- 권고: decision-log RFC-CX-011 이월 항목에 추가하거나 T0(구역=hub 고정, 근거=독립 쌍 충족 여부)로 문장 2개 보강.

### D-M9-08 · S3 · SYS-CORE(tests)/QA — `T0CaseThreadTests.ExpectedObjective()`가 프로덕션 `CaseObjective`를 그대로 호출해 목표줄 어서션이 동어반복이 됐다

- [OBSERVED] `T0CaseThreadTests.cs:199-207`: 기대값을 런타임과 같은 함수·같은 데이터로 계산. beats 배선 상태(현재)에서는 가드가 잘못 통과시켜도 테스트가 같은 값을 기대한다. 독립 검사는 `AssertNoDisclosure` 금지어 10종뿐이며, t0-b3 목표문 "대조의 밤 **표준판**을 처음 판독해 **검증 사본**을…"의 부분 명사는 금지어 목록(`당직실 표준판` 전체 문자열)에 걸리지 않는다.
- 권고: 비트별 리터럴 기대 문자열 3건(또는 t0-b1 폴백 리터럴 + t0-b2/b3 저작문 리터럴)로 교체. 가드 단위 검증은 이미 `M9CoreTests (d)`가 맡는다.

### D-M9-09 · S3 · director(영수증) — verification.md 네이티브 실행 행의 인용 로그가 주장을 담고 있지 않다

- [OBSERVED] §4 마지막 항목. `player-smoke.log`(1.8KB, 31줄) = UnityMemory 설정만. `PLAYER_ALIVE`/`TERMINATED`/12초 마커 없음.
- 권고: 셸 트랜스크립트(실행 명령·pid·sleep·kill·exit) 또는 Player.log 사본을 영수증으로 교체하고, 없으면 해당 행을 `[NOT-MEASURED]`로.

### D-M9-10 · S3 · SYS-CORE — 확정 저장 pending 중 힌트 단계 상승분이 다음 저장까지 미기록 (S-A 왕복의 작은 구멍)

- [OBSERVED] `T0GameSession.cs:328-329` `if(!SavePending)QueueSave()`. `Persist`(`:128`)는 호출 시점에 `SaveDocument`를 동기 평가하므로 이미 진행 중인 pending 저장은 상승 전 사전을 담는다. F1→hints는 pending 중에도 열린다(`PendingAllowsOverlayUndoAndSuppressesStaleReceipt`가 그 경로를 사용). pending 완료 후 종료하면 단계 기록 유실. impl-syscore.md:61 open item 3에 자인.
- 영향: `hintLevelUsed`는 텔레메트리·복구용(H-R7)이라 진행 손상 없음 → S3.
- 권고: pending 완료 콜백에서 `hintLevelsDirty`면 `QueueSave()` 1회.

### D-M9-11 · S3 · planner(콘텐츠, M9 코드 범위 밖) — t0-b2 objective의 설계 메모 문장이 플레이어 화면에 노출된다

- [OBSERVED] `beats.json t0-b2.objective` = "회로 지도에 … 법1을 손으로 익힌다. **안내 표시가 각 단계에 붙는다.**" 두 번째 문장은 저작 지시문이다. S-D 판정 B로 t0-b2는 가드를 통과해 CaseThread에 그대로 표시된다(python 대입 확인). campaign.json 소유는 planner이며 병행 편집 중이므로 이 검토는 표시 사실만 기록한다.
- 권고: planner 재-인테이크(impl-syscore.md open item 1의 t0-b1 무스포일러 재작성과 함께).

## 7. 관측(결함 아님)

- O-1 [OBSERVED] `PreviewDifferenceSentences`의 `previewFactGeneric`은 원시 팩트 문자열(`"line:rec-…"` 등)을 그대로 넣지만, T0에서 `RequestConfirm`을 거치는 명령은 `CiteToBoard`·`ReadOriginal` 2종뿐(`T0GameSession.cs:118,:285,:289`)이라 현재 데이터로는 도달하지 않는다. 확정 명령이 늘면 표시명 매핑이 필요하다.
- O-2 [INFERENCE] 열림 전환(180ms) 도중 `Render()`가 일어나면(`UpdateReviewNotesAvailability`의 `reviewCanSaveAtRender` 변화 등) 새 CanvasGroup(alpha 기본 1)이 생성돼 1프레임 밝기 튐이 가능하다. `AdvanceReviewTransitions`가 매 프레임 `Interface.ReviewPanelGroup`을 다시 잡아 다음 프레임에 복구된다. 실측 없음.
- O-3 [CARRIED] `M5DirectionPlayModeTests.cs`: `PointerSettingsFromReduced150IntroOpensAndReturns` → `PointerSettingsAt150IntroOpensAndReturns`, `Click("reduced-motion")` 제거. `T0OpeningSession.cs:30` `if(ReducedMotion){FinishOpening();return;}`로 reduced-motion에서는 오프닝이 존재하지 않게 됐으므로 옛 전제가 성립하지 않는다. reduced-motion 경로는 `ReducedMotionStartsFreshGameImmediatelyWithoutWrites`(Passed)가 맡는다. RFC-CX-011 소유 목록 밖이며 decision-log.md:408이 보존을 명시.
- O-4 [OBSERVED] `T0ReviewNotesInterface.cs:65-69` 질문 패널 색 `(.96,.95,.89)`·`(.38,.18,.07)`·`120*scale`은 UI 레이아웃 상수(기존 관례). 타이밍·밸런스 아님. 팔레트 정합은 네이티브 검수 시 concept/style-guide.md 대조 대상.
- O-5 [OBSERVED] `--m8-review-notes-diagnostic`(`ReviewNotesSession.cs:222`)은 `runtimeApproved=false` 자산을 진단 목적으로 표시하는 CLI 스위치. M5/C1 선례와 동일 패턴이며 승격(asset 값 변경)이 아니다. 출시 빌드에서 이 계열 플래그를 일괄 무력화할지는 별도 판정(전 마일스톤 공통).
- O-6 [OBSERVED] `ReviewNotesSession.cs` 신규 한국어 문장(`reviewShownSummary`, 라벨, 버튼 설명)은 `L()`을 거치지 않는다 — M8 기존 코드와 동일 관례(EN 미지원, G10 이월 계열). 신규 위반으로 세지 않는다.
- O-7 [OBSERVED] `CloseReviewNotes`가 열림 전환 중(alpha<1)에 호출되면 `StartReviewPanelTransition(closing:true)`가 alpha를 1로 올린 뒤 페이드한다(`:118`). 순간 점프 가능. 마감 폴리시 항목.

## 8. 이 검토가 말하지 않는 것

- 네이티브 창 가독성·카드 텍스처의 실제 대비/타일링·팔레트 정합 — 미검수(verification.md:43 자인과 동일). `runtimeApproved=false` 유지가 맞다.
- 사람 플레이(재미·몰입·25분 예산·힌트 3단 후 `stuck_after_l3`)·실기기 한국어 IME·물리 컨트롤러 — n=0, [CARRIED].
- 180/140/120ms 전환의 **실측 프레임 타이밍** — PlayMode 테스트는 완료 여부와 reduced-motion 즉시성만 단정하고 지속시간을 측정하지 않는다.
- D-M9-01/02/10의 실행 재현 — 본 검토는 배치 실행 금지 조건이라 정적 추론(`[INFERENCE]`)이며, 수리 후 회귀 테스트로 확정해야 한다.
- 빌드 산출물의 실제 실행(12초 생존) — D-M9-09대로 영수증 부재.
- planning/synopsis/worldview/campaign.json의 동시 편집 결과 — 범위 밖. 이 검토가 읽은 beats.json/tools.json은 `Data/Tables/*` 현재 바이트이며 campaign.json 재-emit 후에는 D-M9-11·S-D 가드 결과가 달라질 수 있다.

열린 S1: 0 · S2: 1 · S3: 10

---

## 디렉터 FIX cycle 1 부기 (game-production-director · 2026-09-11)

[OBSERVED] 위 판정(S1 0 · S2 1 · S3 10)에 대한 1회차 수리·판정 기록. 근거: `production/decision-log.md` "RFC-CX-011 QA FIX cycle 1", 영수증 `systems/tech-verification/completeness-m9/verification.md` §QA FIX cycle 1.

| id | 처리 | 검증 |
|---|---|---|
| D-M9-01 (S2) | 수리 — `ReviewSourceOriginalAvailable` 게이트(C1에서 액션 미생성·메서드 조기 반환) | 신규 PlayMode `C1StagesNeverOfferSourceOriginalOpen` PASS (playmode-fix1 69/69) |
| D-M9-02 | 수리 — `Back()`이 reviewNotes에서 `CloseReviewNotes()` 위임 | 같은 회귀 테스트가 Esc 경유 닫힘·재개 없음 단정 |
| D-M9-03 | 판정 — `select_ms` 제거, 영수증 문구 180/140/120 정정, 100ms 윤곽 TARGET 이월 | M8ReviewVfx.json·impl/verification/changelog/manifest 정정 |
| D-M9-04 | 수리 — `T0Simulation.IsSatisfied` 공개, App 복제 삭제 | EditMode 53/53 · PlayMode 69/69 |
| D-M9-05 | 수리 — `teachingRemaining` "비트"→"단계" / beat→step | 문자열 단일 치환 확인 |
| D-M9-06 | 판정 — 티칭 헤더는 힌트 사다리와 별개 안내(선택 a); hint-system.md 등재는 systems 후속 | decision-log |
| D-M9-07 | 판정 — 영향 구역·근거 2종 문장 이월 | decision-log 이월 |
| D-M9-08 | 수리 — `ExpectedObjective` 비트별 리터럴 | T0CaseThreadTests PASS |
| D-M9-09 | 수리 — 트랜스크립트+Player.log 영수증으로 교체(사전/사후 프로세스 0, 새 pid, T0_BOOT 4마커) | player-smoke-transcript.log |
| D-M9-10 | 수리 — `SaveHintLevels`/`FlushHintLevelsIfDirty` | 기존 pending 테스트 PASS(회귀 없음); 전용 단정은 후속 |
| D-M9-11 | 이월 — planner 콘텐츠(campaign.json 병행 편집 중) | manifest 이월 항목 |

[TARGET] QA 재검토(R2)로 S2 폐쇄를 확정한다. 그 전까지 위 표는 디렉터 자기 보고이며 독립 판정이 아니다.

열린 S1: 0 · S2: 1(수리됨, QA 재확인 대기) · S3: 6 이월/판정 · 수리 4

---

## QA R2 (game-qa)

**판정 요약**: FIX cycle 1의 수리 6건(D-M9-01/02/04/05/08/10) **전부 CLOSED**, 판정 3건(03/06/07)은 decision-log 기록과 코드가 정합해 CLOSED(판정 수용), 이월 2건(09/11) 중 09 CLOSED·11 STILL-OPEN. 영수증 6종은 재해시·재파싱·디스크 대조로 문서 주장과 일치. **신규 2건**: D-M9-12(S3, 보고 정합 — 이월·후속 항목이 decision-log에만 있고 manifest·changelog·impl 보고에 미반영), **D-M9-13(S2, R1 누락 — 도구 패널이 열린 채 `원문 열기`를 누르면 복귀가 즉시 취소되고, 문서가 표시된 동안 숨은 도구로 입력이 전달돼 회로 Overlaying 상태에서 화살표가 `SetOverlayOffset` 확정 쓰기를 낸다)**. D-M9-13은 수리로 생긴 것이 아니라 R1이 `tool!=null` 게이트를 "이탈 조건"으로만 읽고 지나친 것이다.

검토 조건: R1과 동일(READ-ONLY, Unity 배치 실행 0회, 포매터/린터 0회). 코드 판단은 정적 열람. 검토 시각 기준 `git status -- unity/`는 R1 §0과 동일 집합(수정 15 · 신규 6 +meta). `Data/Tables/*` diff stat 0줄 유지.

### R2-1. 수리 6건

| id | R2 판정 | 근거 |
|---|---|---|
| D-M9-01 (S2) | **CLOSED** | [OBSERVED] `ReviewNotesSession.cs:147` `ReviewSourceOriginalAvailable => !PatrolActive && !SignatureActive`(`C1GameSession.cs:21` `Journal.State.Has("c1:entered")`, `C1SignatureGameSession.cs:14` `Has(...,"entered")`). 액션 생성은 `:309` `if (ReviewSourceOriginalAvailable && item.Id.StartsWith("record:"))`로 게이트 — C1에서 `review-open-*`는 리스트에 오르지 않는다. 메서드도 `:150` `if (!ReviewSourceOriginalAvailable) return;`로 `document` 쓰기(`:157`) 전에 조기 반환. 게이트 조건은 `T0GameSession.cs:172` Render 우선순위(`SignatureActive→PatrolActive→document`)와 정확히 같은 술어라 "C1 화면이 DocumentScreen을 가리는 상태"와 "액션 미제공 상태"가 항상 일치한다. 회귀 테스트 `ReviewNotesPlayModeTests.cs:317-332`: C1PatrolCompletedV2 픽스처로 `PatrolActive||SignatureActive` 단정(`:322`) → 노트 열기 → record 출처가 여전히 집계됨을 단정(`:324`, 게이트가 출처 필터가 아님을 확인) → `ActionIds`에 `review-open-` 접두 0건 단정(`:325`) → `Back()` 후 Surface≠reviewNotes(`:329`) → 두 번째 `Back()` 후에도 ≠reviewNotes(`:331`). 행동 단정이며 private 게이트를 직접 호출하지 않는다. playmode-fix1.xml에서 `C1StagesNeverOfferSourceOriginalOpen` `result="Passed"` duration 0.258s. 부수 관측: `c1:entered`는 영구 팩트라 C1 진입 후에는 T0 record 원문 열기가 영구 비활성 — 결정문(C1에서 액션 미생성)대로이며 M8 이전 계약에는 없던 기능이므로 축소로 세지 않는다 |
| D-M9-02 | **CLOSED** | [OBSERVED] `T0GameSession.cs:122` `Back()` 분기 순서 = OpeningActive → `if(SavePending){CancelPending();QueueSave();}` → **`if(overlay=="reviewNotes"){CloseReviewNotes();return;}`** → `overlay!=null` → document → circuit Cancel → CloseTool. HEAD(`git show HEAD:…:112`)와 비교해 SavePending 분기 위치는 불변이고 reviewNotes 분기가 그 뒤에 삽입됐다 → pending 중 Esc가 확정을 취소하는 기존 전제(모든 오버레이 공통)는 유지. 기존 테스트 중 `overlay=="reviewNotes"` 상태에서 `Back()`을 호출하는 것은 없고(`T0PlayModeTests.cs:79` settings, `ReviewNotesPlayModeTests.cs:301,:308` settings/evidence), `PendingAllowsOverlayUndoAndSuppressesStaleReceipt`(`:42`)는 hints 오버레이+Undo 경로라 무관 — playmode-fix1 Passed. `CloseReviewNotes`(`ReviewNotesSession.cs:97-109`)는 `:99` 닫힘 중 재호출 가드, `:104` reduced-motion 즉시 복원, `:190` 페이드 완료 후 `overlay=="reviewNotes"`일 때만 직전 오버레이 복원(중간에 pending 완료로 `overlay=null`이 되면 부활 없음). 테스트 `:326-329`는 Esc가 닫힘을 **완료**함을 단정하지만 페이드 경유 여부(직행 0ms vs 140ms)를 판별하지는 못한다(대기 루프가 둘 다 통과) — 페이드 위임은 `:122` 코드 열람으로 확인. 기록 |
| D-M9-04 | **CLOSED** | [OBSERVED] `grep RequirementMet unity/Unknown/Assets/` **0건**. `T0Simulation.cs:177` `public bool IsSatisfied(PuzzleState,CompletionRequirement) => Satisfied(state,requirement);` — private `Satisfied`(`:179-202`)를 그대로 감싼다(추가 분기 없음). `T0GameSession.cs:214` `Requirements.Count(r=>!Simulation.IsSatisfied(Journal.State,r))`. 행동 차이 1건: 구 복제본은 `default:return false`, Sim은 `:200` `throw` — 그러나 같은 beat의 같은 `Requirements`를 `IsComplete`(`:174`)가 이미 `Satisfied`로 평가하므로(`CaseThreadNext :195-196`, Render `:175`) 미지 술어 타입은 어차피 던진다. 신규 노출면 0. EditMode 53/53·PlayMode 69/69(fix1) |
| D-M9-05 | **CLOSED** | [OBSERVED] `T0Strings.json` 161키 전체를 json 파싱해 모든 문자열 값에서 "비트" 검색 → **0건**; `\bbeat\b`(EN) 0건. `teachingRemaining` = ko "이 단계의 남은 조건 {0}개" / en "{0} conditions remain in this step". 관측(범위 밖, 신규 아님): 부제 `"21:00 · "+CurrentBeat`(`T0GameSession.cs:164`)는 raw 비트 id(`t0-b2`)를 플레이어에게 표시하나 HEAD `:154`에 동일 — M9 신규 노출이 아니므로 D-M9-05 범위 밖. 별도 항목으로 올릴지는 worldview 판정 |
| D-M9-08 | **CLOSED** | [OBSERVED] `T0CaseThreadTests.cs:202-209` `ExpectedObjective()`: 비트 선택은 `Simulation.IsAvailable/IsComplete`(Sim 술어, 프로덕션 `CaseObjective`·`BeatFor` 호출 없음) → `switch` 리터럴 3건. `CaseObjective`(`T0GameSession.cs:201`) 참조 0건(grep). 리터럴 대조: t0-b2 리터럴 = `beats.json:163` objective의 첫 문장까지 정확 일치(접두; 둘째 문장 "안내 표시가 각 단계에 붙는다"는 의도적으로 제외 — D-M9-11 수정 후에도 테스트가 살아남도록). t0-b3 리터럴 = `beats.json:252` 전문 일치. 기본값 "결손 4시간의 양 끝을 두 기록으로 고정" = `T0Strings.json caseObjective.ko` 일치 → t0-b1에서 가드 폴백이 **실제로** 일어남을 독립 단정(가드가 오통과하면 objective 원문이 들어가 이 어서션이 실패). `StringAssert.Contains(ExpectedObjective(),card.text)`(`:217`) + `AssertNoDisclosure`(`:215`) 유지. 비트 선택 순서 `t0-b1→b2→b3`는 `BeatFor`(`C1GameSession.cs:24-25`)의 `Definition.Beats.FirstOrDefault(available&&!complete)`와 선형 전제조건 사슬에서 동치. playmode-fix1 `T0CaseThreadTests` 2건 Passed |
| D-M9-10 | **CLOSED** `[INFERENCE — 코드 열람, 전용 테스트 부재]` | [OBSERVED] `T0GameSession.cs:70` `SaveHintLevels(){if(SavePending)hintLevelsDirty=true;else QueueSave();}` — 호출처 `:315`(hint-next)·`:316`(hint-reveal) 2곳. `:71` `FlushHintLevelsIfDirty(){if(!hintLevelsDirty)return;hintLevelsDirty=false;QueueSave();}` — 호출처 3곳 = **성공** `:145`(`SavePending=false` 직후, `Render()` 전), **실패** `:147`(catch, `attempt==generation`일 때), **취소** `:128`(`CancelPending`, `SavePending=false` 후). 세 경로 모두 `SavePending=false`가 선행하므로 `QueueSave`가 `Persist`(`:131`)로 이어지고 `SaveDocument`(`:129`)가 호출 시점의 `hintLevels`를 동기 캡처한다. 누락 경로 점검: (a) `:144` 조기 반환(`attempt!=generation||IsCancellationRequested`)은 `generation++`/`Cancel()`이 `CancelPending`(`:128`)에서만 발생하므로(`OnDestroy :336` 제외) 이미 flush됨; (b) catch의 `attempt!=generation` 조기 반환도 동일; (c) `OnDestroy`(`:336`)는 `pendingCancel.Cancel()`만 호출 — 종료 시점 pending 중 상승분은 미기록이나 그 확정 자체도 유실되는 종료 경계라 R1 권고 범위 밖; (d) `saveReadOnly`면 `QueueSave` 자체가 no-op이고 dirty도 남지 않음(`SavePending`이 true가 될 수 없음). 경쟁 점검: 취소된 커밋의 `save.json` 쓰기는 `AtomicSaveStore.cs:53` rename 직전 `ThrowIfCancellationRequested`로 막히고, flush의 `QueueSave`와 호출자 자신의 `QueueSave`(Back `:122`/Undo `:132`/Redo `:133`/SubmitImmediate `:125-126`)는 `SemaphoreSlim gate`(`AtomicSaveStore.cs:21,:26`)로 직렬화 — 같은 내용(Back) 또는 선후 순서(Undo: 사전→사후)로 기록되므로 "1회 보장"은 성립하되 취소 경로에서는 **2회**(중복, 무해). 재진입: `Persist`는 첫 `await` 전까지 동기이며 세션 메서드를 다시 호출하지 않는다 → `CancelPending` 내 `QueueSave` 재진입 없음. **테스트 부재**: 어느 테스트도 pending 중 `hint-next`를 누르지 않는다(`PendingAllowsOverlayUndo…`는 F1로 hints를 열 뿐). 결함 서술(상승분 미기록)은 코드로 닫혔으므로 CLOSED, 회귀 보호 공백은 아래 G-1로 기록 — 디렉터 부기 "전용 단정은 후속"과 일치 |

### R2-2. 판정 3건 · 이월 2건의 기록 정합

| id | R2 판정 | 근거 |
|---|---|---|
| D-M9-03 | **CLOSED(판정 수용)** | [OBSERVED] `M8ReviewVfx.json` 7줄: `open_ms/close_ms/question_ms/reduced_motion_ms/timing_source/deferred` — `select_ms` 부재, `deferred` 값에 "select outline 100ms … not implemented in M9; no consumer key kept" 명시. `ReviewVfxMs`는 4키만 읽는다(`:104,:113,:137,:211`). 문구 정정: impl-sysm8.md:23 "open 180 / close 140 / question 120", verification.md:20 "180/140/120ms", changelog.md:72 "180/140/120 ms", task-manifest.md:102 "(180/140/120ms)" — 4곳 일치. **100ms 윤곽 TARGET 이월은 decision-log.md:447에만** 있고 manifest 이월 목록(`:106`)에는 없다 → D-M9-12 |
| D-M9-06 | **CLOSED(판정 수용)** | [OBSERVED] decision-log.md:448 선택 (a) 기록, `GuidedTeachingText`(`T0GameSession.cs:206-216`)는 `hintLevels`를 쓰지 않는다(R1 관측 유지). `systems/system-specs/hint-system.md`에 `teaching|티칭|guided` 0건 — "등재는 systems 후속"이라는 decision-log 문구와 정합하나 그 후속이 manifest에 없다 → D-M9-12 |
| D-M9-07 | **CLOSED(판정 수용)** | [OBSERVED] decision-log.md:449 이월 기록. manifest `:106` 이월 목록에 "영향 구역·근거 2종" 0건 → D-M9-12 |
| D-M9-09 | **CLOSED** | §R2-3 영수증 |
| D-M9-11 | **STILL-OPEN(S3, planner)** | [OBSERVED] `beats.json:163` t0-b2 objective 둘째 문장 "안내 표시가 각 단계에 붙는다." 잔존(Tables 바이트 불변이므로 당연). decision-log.md:450 이월 기록은 있으나 디렉터 부기가 근거로 든 "manifest 이월 항목"은 **없다**(task-manifest.md:106의 이월 목록은 RFC 명시 5건뿐; `t0-b2|안내 표시` grep 0건). 결함 자체는 열려 있고 기록 불일치는 D-M9-12 |

### R2-3. 영수증 대조 (FIX cycle 1)

방법: R1과 동일 — `python hashlib.sha256` vs `verification.json.nativeRuns[phase=qa-fix-cycle-1].sha256`, `<test-run>`/`<test-case>` 속성 re 파싱, 빌드 산출물 디스크 바이트 합산, 소스 mtime vs 실행 start-time.

| 항목 | 문서 주장 | 실제 | 일치 |
|---|---|---|---|
| editmode-fix1.xml | 53/53 | sha `daf8b52a…` 일치 · testcasecount 53 passed 53 failed 0 skipped 0 · Passed · 14:27:38Z | ✓ |
| playmode-fix1.xml | 69/69 (+1 C1 회귀) + boot ignored | sha `33717609…` 일치 · testcasecount **70** passed **69** failed 0 skipped 1 · Skipped:Ignored · 14:27:58Z · skipped = `SerializedBootLoadsHubAndInitializesVisibleStartScreen` · `C1StagesNeverOfferSourceOriginalOpen` Passed · `OpeningASourceOriginalReturnsToNotesWithFocusOnThatSource`·`ReducedMotionCompletesNoteTransitionsImmediately`·`PendingAllowsOverlayUndoAndSuppressesStaleReceipt` Passed | ✓ |
| boot-fix1.xml | 1/1 | sha `dd76edf1…` 일치 · 1/1/0/0 · Passed · 14:29:55Z | ✓ |
| build-mac-fix1.log | `T0_MAC_BUILD Succeeded bytes=354230869` | 해당 줄 1회, `error CS` 0건 · `Builds/T0-mac/Unknown.app` 재귀 바이트 합 **354230869** 정확 일치 · `Contents/Resources/Data/globalgamemanagers` mtime 14:30:17Z(빌드 로그 14:30:25Z 직전; `.app` 디렉터리 mtime 13:58Z는 항목 추가/삭제 없는 덮어쓰기라 불변 — 정상) | ✓ |
| player-smoke-transcript.log | 사전/사후 프로세스 0 · 새 pid 12초 · T0_BOOT 4마커 · kill 확인 | "precondition: pgrep 'Unknown T0' -> 0 processes" · `open exit=0` · `PLAYER_ALIVE pid=31749 after 12s` · `ps` 행 `31749 00:12` · 마커 4줄 · `TERMINATED pid=31749 exit-confirmed` · "postcondition: … 0 processes". `player.log`(6.7KB, mtime 14:33:22Z = 트랜스크립트와 동시각)에 `T0_BOOT entry-start/ui-root-loaded/hub-loaded/session-initialized` 4줄 실재 | ✓ (주의 1) |
| 소스 ↔ 실행 순서 | fix1 실행이 현재 코드를 검사 | 수정 파일 mtime 최댓값 `ReviewNotesPlayModeTests.cs` 14:26:40Z < editmode-fix1 start 14:27:38Z. 이후 `_Project` 하위에서 바뀐 파일은 `Rendering/T0URP.asset` 14:30:07Z 1건뿐(빌드 중 touch; `git status`에 미등재 = 내용 불변) | ✓ |

- 주의 1 [OBSERVED]: 트랜스크립트의 사전/사후 "0 processes"는 `pgrep` 원출력이 아니라 스크립트가 쓴 요약 문장이다(원출력이 있는 행은 `ps` 1줄과 `open exit=0`뿐). `player.log`에는 `--t0-save-dir`/pid 에코가 없어 pid 31749와의 결합은 mtime 동시각과 서술에 의존한다. R1 D-M9-09의 요구(생존·종료·마커가 영수증에 **담길 것**)는 충족하므로 CLOSED. 다음 사이클부터 `pgrep` 원출력·`date -u` 타임스탬프를 행마다 남기면 이 주의는 사라진다.
- [OBSERVED] verification.json `qaFixCycle1.fixed/ruled/carried` 11건 id 집합 = 디렉터 부기 표와 일치. `nativeLaunch` 문자열 = 트랜스크립트 내용과 일치.

### R2-4. 신규 결함

### D-M9-12 · S3 · director(보고 정합) — FIX 1의 이월·후속 결정이 decision-log에만 있고 추적 문서(manifest·changelog·impl 보고)에 미반영

- [OBSERVED] `task-manifest.md:106` 이월 목록 = RFC 명시 5건(hintOfferCooldownSeconds·EN textKey·alignment 동사·ReadOriginal undo·t0-b1 무스포일러)뿐. decision-log.md:447-450이 새로 만든 이월/후속 4건 — D-M9-03 100ms 선택 윤곽 TARGET, D-M9-06 hint-system.md 등재(systems), D-M9-07 영향 구역·근거 2종 문장, D-M9-11 t0-b2 둘째 문장(planner) — 이 manifest에 **0건**(`D-M9|t0-b2|안내 표시|영향 구역|근거 2종|윤곽|100ms` grep). 디렉터 부기 표의 "manifest 이월 항목"(D-M9-11 행) 근거는 존재하지 않는다.
- [OBSERVED] `changelog.md:74` 네이티브 행은 "PlayMode 68/68 … 354233020 B"(FIX 전 수치) 그대로이고 FIX cycle 1 항목이 없다(`FIX|QA R|69/69|354230869` grep 0건). `impl-syscore.md:61` open item 3은 "단계 기록이 저장되는 것은 다음 저장 시점"으로 FIX 전 동작을 서술한다(현재는 settle 시 flush).
- 영향: 이월 항목이 매니페스트 우선순위 보드에 오르지 않으면 다음 사이클 인테이크에서 누락된다(G8 freshness 취지). 코드·플레이 영향 없음 → S3.
- 권고: manifest `:106`에 4건 추가(소유 레인·기한), changelog에 "M9 QA FIX 1" 소항목(69/69·354230869·수리 6·판정 3), impl-syscore.md:61 문구를 flush 동작으로 갱신.

### D-M9-13 · **S2** · SYS-M8 (ReviewNotesSession) + SYS-CORE(input) — 도구 패널이 열린 상태에서 `원문 열기`를 누르면 (a) 복귀 플래그가 다음 프레임에 즉시 해제돼 닫아도 노트로 돌아오지 않고, (b) 문서가 표시되는 동안 숨은 도구로 입력이 전달된다 `[INFERENCE — 코드 열람, 미실행 · R1 누락, 수리로 생긴 것 아님]`

- 재현 경로 [OBSERVED — 도달 가능성만]: T0에서 record 원문(예 `rec-handover-brief` 줄) 관찰 → `hub-view-circuitmap`→`open-circuit`(또는 `hub-view-reader`→`open-reader`) → 도구 화면에서 툴바 `evidence`(`T0GameSession.cs:170`, 도구 여부와 무관하게 항상 추가) 또는 Overlay 키(`WatchInput.cs:42`→`OpenOverlay :119`, `tool` 불변) → `open-review-notes`(`:174`) → `review-open-record:rec-handover-brief`. `ReviewSourceOriginalAvailable`(`ReviewNotesSession.cs:147`)는 C1만 막고 `tool`은 보지 않는다.
- (a) 복귀 실패: `OpenReviewSourceOriginal`(`:148-158`)은 `overlay=null; document=recordId`만 쓰고 **`tool`을 지우거나 기억하지 않는다**. 다음 `Update`→`UpdateReviewNotesAvailability`(`:75-78`)→`ResumeAfterReviewSourceOriginal`(`:162-172`)의 `:165` `if (overlay != null || tool != null || node != reviewReturnNode) { reviewReturnPending = false; …; return; }` — `tool!=null`이 **문서를 닫기 전에** 참이므로 플래그가 첫 프레임에 해제된다. `close-document`→`document=null`→Render(`:172`)는 `tool=="circuit"→CircuitScreen`/`"reader"→ReaderScreen`으로 떨어지고 노트·초점 복귀는 일어나지 않는다. 계약 위반: RFC-CX-011 S-E "닫으면 검토 노트 복귀+초점 복원", direction.md:34. 기존 테스트 `OpeningASourceOriginalReturnsToNotesWithFocusOnThatSource`(`:353-364`)는 `tool==null`(shell)에서만 검사한다.
- (b) 숨은 도구 입력: 문서가 표시되는 동안 `Watch.ToolPanel=tool!=null&&!OpeningActive`(`:165`)가 true, `OverlayActive=false`. `WatchInput.cs:64` `if(ToolPanel&&!OverlayActive&&!navigationStick)Adjust?.Invoke(...)` → `Adjust`(`T0GameSession.cs:150-155`)는 `tool=="circuit"&&CircuitMode=="Overlaying"`이면 **`SubmitImmediate(SetOverlayOffset)`** — 문서를 스크롤/탐색하려는 화살표가 회로 오버레이를 옮기고 저널·`save.json`에 기록된다(되돌리기 가능하나 플레이어가 보지 못한 쓰기). `tool=="reader"`면 `Disconnect`(`WatchInput.cs:45`→`T0GameSession.cs:121`)가 `RequestConfirm(CiteToBoard)`를 열어 문서 위에 preview/confirm 오버레이가 뜬다. `Query`(`:120`)도 같은 게이트. 이 `document!=null && tool!=null` 동시 상태는 M9 이전에는 만들어지지 않았다(`document`는 ShellScreen 액션 `:237-238,:243`에서만, `OpenTool :116`은 `document=null`) — `OpenReviewSourceOriginal`이 처음 만든 상태이며 `Render`·입력 라우팅 어느 쪽도 이를 가정하지 않는다.
- 등급: (a)만이면 S3(상태 오염 없음, 자연 복구)이나 (b)의 숨은 확정 쓰기(회로 Overlaying 한정)와 키보드 단독 문서 탐색 불능(interaction-rules §0-8)이 겹쳐 **S2** — R1 D-M9-01과 같은 기준(내비게이션 상태가 플레이어 의도와 어긋남). 회로 Overlaying 중 노트 진입이 드문 경로라면 디렉터가 S3로 내릴 수 있다. 미실행 `[INFERENCE]`.
- 권고: `OpenReviewSourceOriginal`에서 `tool`을 `reviewReturnTool`로 기억하고 `CloseTool()`(circuit이면 `CloseTool` 명령 확정 — 마모 없음 확인 필요) 후 `document`를 열거나, 최소한 `ReviewSourceOriginalAvailable`에 `tool==null`을 추가해 도구 중에는 액션을 내지 않는다(D-M9-01과 같은 방식, 후자가 가장 작다). 회귀 테스트: reader/circuit 도구 → evidence → 노트 → `원문 열기` → `close-document` → Surface=="reviewNotes" 또는(축소안) `review-open-*` 0건 단정 + Overlaying 중 화살표가 `overlayX/Y`를 바꾸지 않음 단정.

### R2-5. 관측(결함 아님) · 커버리지 공백

- O-8 [OBSERVED] `Back()`의 reviewNotes 분기(`:122`)는 `proposed=null`을 하지 않는다(일반 오버레이 분기는 한다). `proposed`는 confirm/preview/saveFailure 오버레이 안에서만 소비되고 `RequestConfirm`이 매번 덮어쓰므로 무해.
- O-9 [OBSERVED] Undo/Redo(`:132-133` `overlay=null`)·`GoNode`(`:79`)·툴바 `evidence/hints`(`OpenOverlay` 직행)는 reviewNotes를 페이드·직전 오버레이 복원 없이 닫는다. 모든 오버레이 공통의 기존 관례(HEAD 동일)이며 D-M9-02는 Esc 한정이었으므로 결함으로 세지 않는다. 키보드/버튼 동등성은 유지(같은 `Undo()`).
- O-10 [INFERENCE] `SemaphoreSlim`은 대기자 FIFO를 계약으로 보장하지 않는다. Undo 경로에서 flush(사전 저널)와 호출자 `QueueSave`(사후 저널)가 연달아 큐잉되므로 순서 역전 시 `save.json`이 한 저장 주기만큼 되돌아갈 수 있다 — 그러나 `SubmitImmediate` 연타도 같은 패턴(HEAD 동일)이라 M9 신규 위험이 아니다. 실측 없음.
- O-11 [OBSERVED] `OnDestroy`(`:336`) 취소는 `CommitAsync` catch(`:147`)에서 `Render()`를 파괴된 컴포넌트 위에 호출한다. HEAD `:112` 계열과 동일 경로(FIX 1 무관). 테스트 teardown에서만 도달.
- G-1 커버리지 공백(D-M9-10): pending 중 `hint-next`→commit 성공/실패/취소 후 `save.json.progress.hintLevelUsed` 반영 단정 없음. `Store.Injection`(BeforeRename 지연) + F1 + `hint-next` + 해제/실패 주입 3분기 1테스트로 닫을 수 있다.
- G-2 커버리지 공백(D-M9-02): Esc 닫힘이 페이드를 **경유**했는지(`ReviewPanelTransitionActive` true 구간 관측 또는 `reviewPreviousOverlay` 복원 목적지 단정)를 테스트가 판별하지 않는다. `C1StagesNeverOfferSourceOriginalOpen :329` 뒤에 `Assert.AreEqual("evidence", game.Surface)` 1줄이면 직전 오버레이 복원까지 단정된다.
- G-3 커버리지 공백(D-M9-13): 도구 열린 상태의 `원문 열기` 경로 0건.

### R2-6. 집계

수리 6건 CLOSED(01·02·04·05·08·10) · 판정 3건 CLOSED-판정 수용(03·06·07) · 이월 2건 = 09 CLOSED / 11 STILL-OPEN · 신규 12(S3)·13(S2). S2 D-M9-01은 **독립 확인으로 폐쇄**되나, 같은 메서드의 인접 경로에서 S2 1건이 새로 열려 hop 완료 판정은 다시 디렉터 판정(D-M9-13 수리 또는 S3 강등 근거 기록)이 필요하다.

R2 열린 S1: 0 · S2: 1 · S3: 2

## QA R3 (game-qa)

**판정 요약**: FIX cycle 2의 수리 대상 D-M9-13(S2) **CLOSED**(독립 확인 — 코드 게이트·회귀 테스트·영수증 3중 일치), D-M9-12(S3) **CLOSED(S3 근거 해소)** — 단 R2 권고 3항 중 `impl-syscore.md:61` 문구 갱신 1항이 미이행이라 **D-M9-14(S4)**로 분리 등록. G-2 CLOSED. D-M9-11은 범위 밖(planner·campaign.json)으로 STILL-OPEN 유지. FIX 2 영수증 4종은 재해시·재파싱·디스크 대조로 문서 주장과 일치. **S2 재개방 없음** → decision-log FIX 2 "R3에서 S2가 다시 열리면" 조건 미발동.

검토 조건: R1/R2와 동일(READ-ONLY, Unity 배치 실행 0회, 포매터/린터 0회, planning/synopsis/worldview/campaign.json 미열람). 코드 판단은 정적 열람 `[INFERENCE]`, 파일·해시·XML 속성은 `[OBSERVED]`.

### R3-1. D-M9-13 — CLOSED

| 검토 질문 | 판정 | 근거 |
|---|---|---|
| 도구 열림 중 `review-open-*` 액션 미생성 | **예** | [OBSERVED] `ReviewNotesSession.cs:148` `bool ReviewSourceOriginalAvailable => !PatrolActive && !SignatureActive && tool == null;` — `tool==null` 추가됨. 액션 생성 `:310` `if (ReviewSourceOriginalAvailable && item.Id.StartsWith("record:"))` → `:313` `screen.Actions.Add(A("review-open-"+…))`. |
| 메서드 조기 반환이 **같은 게이트** | **예** | [OBSERVED] `:151` `if (!ReviewSourceOriginalAvailable) return;` — 생성 조건과 조기 반환이 단일 프로퍼티 `:148`을 공유하므로 둘이 어긋날 수 없다. 오래된 액션이 남더라도 호출 시점 재검사로 차단. |
| 셸에서 유지 | **예** | [OBSERVED] `T0GameSession.cs:29` `Surface=>overlay??tool??"shell"` — 셸은 정의상 `tool==null`이므로 게이트 참. 기존 `OpeningASourceOriginalReturnsToNotesWithFocusOnThatSource`(`:369-380`)가 이 경로를 그대로 단정(§R3-4). |
| 노트 열린 동안 `tool`이 바뀔 수 없는가(게이트 우회) | **예** | [OBSERVED] `WatchInput.cs:44` `Tool` 액션 `if(OverlayActive)return;`, `:42` `Overlay`·`:43` `Query`(`ToolPanel&&!OverlayActive`)·`:45` `Disconnect` 동일 게이트. `tool`에 non-null을 쓰는 곳은 `OpenTool :116` 하나뿐(`App/` grep). `Undo/Redo :132-133`는 `overlay=null`만 쓰고 `tool` 불변. |
| (b) 숨은 도구 입력 — 이 게이트로 도달 불가해지는가 | **예 · `document!=null && tool!=null` 동시 상태는 더 이상 생성되지 않는다** | [OBSERVED] R2가 지목한 유일한 생성처 `OpenReviewSourceOriginal :158`은 이제 `tool==null`에서만 실행. 나머지 `document=` 기록처 전수: `T0GameSession.cs:237,:238,:243`(ShellScreen — `:172` 분기 순서상 `tool==null`일 때만 렌더), `C1GameSession.cs:74`, `C1SignatureGameSession.cs:64`(C1 — `OpenTool :116`이 `PatrolActive`면 `tool=null` 강제, `continue-c1 :234`는 ShellScreen 안이므로 진입 시 `tool==null`). **셸 노트에서 원문 열기 후 도구 단건키** 경로: `document` 표시 중 `OverlayActive=false`이므로 `Tool` 액션 통과 → `SelectTool :117`→`OpenTool :116`이 `document=null`을 **먼저** 쓰고 `tool=id` — 동시 상태 없음. 휠키(`:44` `key=="wheel"`, `ToolPanel=false`→`ToolWheel`)는 `toolWheel` 오버레이(`:293`)를 거쳐 같은 `OpenTool`. `load-document :251`도 `OpenTool("reader")` 경유. 셸-문서 상태에서 화살표는 `WatchInput.cs:64` `ToolPanel`(`:165` `tool!=null&&…`)=false → `Navigate`로 간다(Adjust 아님). |
| 회귀 테스트가 (a)(b)(c)를 **실제로** 단정하는가 | **예** | [OBSERVED] `ReviewNotesPlayModeTests.cs:336-348`. (a) `:339` `OpenTool("reader")`·`Surface=="reader"` → `:340` OpenNotes → `:341` `ObservedReviewSources().Any(record:)`(비공허 보장) → `:342` `IsFalse(ActionIds.Any(StartsWith("review-open-")))`. (b) `:343` `CloseNotes()`(`review-note-back` 페이드 완료 대기 `:110-116`)→`AreEqual("evidence")`→`Back()`→`:344` `AreEqual("reader")`. (c) `:345` `Back()`→`AreEqual("shell")`→`:346` OpenNotes→`:347` `Contains("review-open-record:rec-handover-brief")`. `ActionIds`는 `model.Actions` 사영(`T0Interface.cs:70`)이고 `review-open-*`는 `screen.Actions`(`:313`)에 들어가므로 같은 목록을 본다. |
| playmode-fix2.xml Passed | **예** | [OBSERVED] `OpenToolPanelsNeverOfferSourceOriginalOpenWhileShellStillDoes` result=Passed start=`2026-09-11 14:53:18Z` duration=0.206s. `playmode-fix2 − playmode-fix1` test-case 집합 차 = **정확히 이 1건**(역차 0건) → "+1 도구 열림 회귀" 주장과 일치. |

등급 판단: R2가 S2로 올린 근거 두 축 — (b) 숨은 확정 쓰기(회로 Overlaying 중 Adjust→`SetOverlayOffset`)와 키보드 단독 문서 탐색 불능 — 모두 `document∧tool` 동시 상태를 전제했고, 그 상태의 생성처가 게이트로 막혔다. (a) 복귀 실패는 도구 중 액션 자체가 없으므로 성립 불가. 확장안(도구를 기억·닫고 문서를 여는 방식)은 manifest `:106` 이월에 등재됨 [OBSERVED].

### R3-2. G-2 — CLOSED

[OBSERVED] `C1StagesNeverOfferSourceOriginalOpen :326-329`: `game.Back()` → `while(Surface=="reviewNotes")` 대기 → `:329` `Assert.AreEqual("evidence", game.Surface, "Esc must restore the overlay the notes were opened from (D-M9-02 / QA G-2).")` — R2 권고 문장 그대로. playmode-fix2.xml Passed(14:53:16Z, 0.231s). Esc가 `CloseReviewNotes`(`ReviewNotesSession.cs:97-108`)를 경유해 `reviewPreviousOverlay`(`:91`에서 "evidence" 기억)로 복원됐음을 목적지로 단정한다.

### R3-3. D-M9-12 — CLOSED(S3 근거 해소) · 잔여 1항 → D-M9-14

| R2 권고 | 이행 | 근거 |
|---|---|---|
| manifest `:106`에 이월 4건 추가 | **이행** | [OBSERVED] `task-manifest.md:106` "이월(RFC-CX-011 명시 + QA FIX 판정)": … `t0-b2 objective 저작 지시문 제거(planner·campaign.json, D-M9-11)`, `프리뷰 문장 영향 구역·근거 2종(D-M9-07)`, `검토 노트 출처 선택 100ms 윤곽(D-M9-03)`, `티칭 헤더 hint-system.md 등재(D-M9-06)`, `도구 열림 중 원문 열기 허용(D-M9-13 축소 수리의 확장안)` — R2가 0건이라 한 `D-M9|t0-b2|안내 표시|영향 구역|근거 2종|윤곽|100ms` 패턴이 이제 전부 매치. 5건(D-M9-13 확장안 포함). |
| changelog "M9 QA FIX" 소항목 | **이행** | [OBSERVED] `changelog.md:75` "QA FIX cycles: R1 found S2 D-M9-01 … → stage gate + regression; R2 closed all six fixes and found adjacent S2 D-M9-13 … → `tool==null` gate + regression. Rulings: `select_ms` dropped …, teaching header ≠ hint use, zone/evidence-pair preview sentences deferred. Post-fix EditMode 53/53, PlayMode 69/69→70/70, boot 1/1, build Succeeded; transcript-backed launch receipt …". FIX 1·2 판정과 수치 반영. `:74`의 FIX 전 수치(68/68·354233020)는 "M9 초기 검증" 행으로 남아 있으나 `:75`가 후속 행으로 명시되어 오독 여지 없음. |
| `impl-syscore.md:61` flush 동작으로 갱신 | **미이행** | [OBSERVED] `impl-syscore.md:61` "… 단계 기록이 저장되는 것은 다음 저장 시점." 그대로. 코드는 `T0GameSession.cs:70` `SaveHintLevels(){if(SavePending)hintLevelsDirty=true;else QueueSave();}`, `:71` `FlushHintLevelsIfDirty`, 호출처 `:128`(CancelPending)·`:145`(commit 성공)·`:147`(commit 실패) — settle 시 flush. decision-log FIX 2 항목은 "manifest 이월 목록·changelog"만 언급하고 impl 보고는 빠져 있다. |

판정: D-M9-12의 S3 등급 근거는 "이월 항목이 매니페스트 우선순위 보드에 오르지 않으면 다음 사이클 인테이크에서 누락"이었고, 그 근거는 manifest `:106`·changelog `:75`로 해소 → **CLOSED**. 남은 impl 보고 문구 불일치는 인테이크 영향이 없는 보고 정확성 문제라 별건 S4로 분리(아래).

### R3-4. 기존 동작 보존 (FIX 2 회귀 없음)

- [OBSERVED] `OpeningASourceOriginalReturnsToNotesWithFocusOnThatSource`(셸 경로, `:369-380`) playmode-fix2.xml **Passed**(14:53:18Z, 0.076s). 소스 불변(테스트 본문 R2 열람과 동일 단정: `close-document` 존재 → 닫기 → `Surface=="reviewNotes"` → `CurrentFocusId=="review-source-record:rec-handover-brief"`).
- [OBSERVED] ReviewNotes 픽스처 15 케이스 전부 Passed. PlayMode 71 케이스 중 non-Passed는 `T0BootSceneTests.SerializedBootLoadsHubAndInitializesVisibleStartScreen`(Skipped:Ignored) 1건뿐이며 boot-fix2.xml에서 같은 케이스가 Passed — FIX 1과 동일 구성.
- [OBSERVED] `T0GameSession.cs` mtime `14:23:57Z`(FIX 1 시점) — FIX 2는 `ReviewNotesSession.cs`(`14:51:08Z`)·`ReviewNotesPlayModeTests.cs`(`14:51:26Z`) 2파일만 건드렸다.

### R3-5. 영수증 대조 (FIX cycle 2)

방법: R1/R2와 동일 — `python hashlib.sha256` vs `verification.json.nativeRuns[phase=qa-fix-cycle-2].sha256`, `<test-run>`/`<test-case>` 속성 re 파싱, `Builds/T0-mac/Unknown.app` 재귀 바이트 합산, 소스 mtime vs 실행 start-time.

| 영수증 | 문서 주장 | 대조 | 판정 |
|---|---|---|---|
| `editmode-fix2.xml` | 53/53 · sha `a3ddd6f2…` | [OBSERVED] sha256 **일치**; `testcasecount=53 passed=53 failed=0 skipped=0 result=Passed start=2026-09-11 14:52:31Z` — json 7속성 전부 일치 | 일치 |
| `playmode-fix2.xml` | 70/70 + boot ignored · sha `f21b781b…` | [OBSERVED] sha256 **일치**; `testcasecount=71 passed=70 failed=0 skipped=1 result=Skipped:Ignored start=14:52:54Z` — 일치. skipped 1 = boot 케이스(위) | 일치 |
| `boot-fix2.xml` | 1/1 · sha `6190792e…` | [OBSERVED] sha256 **일치**; `1/1 Passed start=14:53:49Z`; 케이스 = `SerializedBootLoadsHubAndInitializesVisibleStartScreen` Passed | 일치 |
| `build-mac-fix2.log` | `T0_MAC_BUILD Succeeded bytes=354230868` | [OBSERVED] 로그 말미 "Build Finished, Result: Success." + `T0_MAC_BUILD Succeeded bytes=354230868`; 디스크 `Unknown.app` 합산 **354230868 B**(정확히 일치); `Tide.App.dll` mtime `14:54:16Z`, 로그 mtime `14:54:27Z` | 일치 |
| 소스↔실행 순서 | — | [OBSERVED] 수정 소스 2파일 mtime(14:51:08Z/14:51:26Z) < editmode 시작 14:52:31Z < playmode 14:52:54Z < boot 14:53:49Z < 빌드 14:54. `Assets/**/*.cs` 중 14:52:31Z 이후 수정 **0건**. `UniversalRenderPipelineGlobalSettings.asset`·`Rendering/T0URP.asset`이 14:54:06Z에 touch되었으나 `git diff HEAD --stat` 0줄(빌드 파이프라인의 재직렬화, 내용 변화 없음) | 정합 |

verification.md §QA FIX cycle 2 표 4행·verification.json `qaFixCycle2.fixed` 3항(D-M9-13·D-M9-12·G-2)·`fixCyclesUsed=2/2` — 본 검토 결과와 모순 없음.

### R3-6. 신규 결함

### D-M9-14 · S4 · systems(보고) — `impl-syscore.md:61` open item 3이 FIX 1 이전 동작("다음 저장 시점")을 서술한다 (D-M9-12 권고 3항 중 미이행분 분리)

- [OBSERVED] `impl-syscore.md:61` "3. 힌트 단계 상승 중 SavePending이면 QueueSave를 미루고 다음 확정 저장에 편승 … 단계 기록이 저장되는 것은 다음 저장 시점." vs 코드 `T0GameSession.cs:69-71`(주석 "flushed once that commit settles (QA D-M9-10)") + `:128,:145,:147` 호출.
- 영향: 코드·플레이·인테이크 영향 없음. impl 보고를 근거로 다음 systems 회차가 "다음 저장 시점" 가정으로 설계하면 D-M9-10 재발 위험만 남는다 → S4.
- 권고: 문장을 "settle(성공·실패·취소) 시 `FlushHintLevelsIfDirty`로 즉시 flush(D-M9-10)"로 교체. 1줄.

### R3-7. 관측(결함 아님)

- O-12 [OBSERVED] `OpenToolPanelsNeverOffer…`는 `reader`만 연다. 게이트 `:148`은 도구 id를 보지 않으므로(`tool==null`) circuit도 같은 술어로 막히며, R2가 S2 근거로 든 circuit Overlaying 중 Adjust 쓰기는 정적으로 도달 불가. 별도 circuit 케이스는 있으면 좋으나 필수 아님.
- O-13 [INFERENCE] 셸에서 원문을 연 뒤 `close-document` 전에 오버레이 키(F1 hints/evidence)나 도구키를 누르면 `ResumeAfterReviewSourceOriginal :166`이 `overlay!=null||tool!=null`로 `reviewReturnPending`을 해제해 노트 자동 복귀만 포기한다(오버레이 닫으면 문서→셸, 재개 없음, 잔류 상태 없음). O-9와 같은 "다른 표면으로 이동하면 노트 맥락을 버린다" 관례이며 D-M9-13(b)의 숨은 입력과는 다르다. 실측 없음.
- O-14 [OBSERVED] manifest `:106` 이월 5건 중 소유 레인이 명시된 것은 D-M9-11(planner)뿐이고 기한은 전부 없다(R2 권고 "소유 레인·기한"). 인테이크 가시성(S3 근거)은 충족되므로 등급을 매기지 않는다 — 다음 회차 planner 보드 인테이크 시 레인 배정.

### R3-8. 집계

FIX 2 수리 1건 CLOSED(D-M9-13 S2) · 보고 정합 1건 CLOSED(D-M9-12 S3, 잔여 1항 → D-M9-14 S4 신규) · G-2 CLOSED · 이월 1건 STILL-OPEN(D-M9-11 S3, planner·범위 밖) · 영수증 4종 일치. **S2 재개방 없음** — hop 완료 판정을 보류할 QA 측 사유 없음. 열린 S4: 1(D-M9-14).

R3 열린 S1: 0 · S2: 0 · S3: 1
