---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
---

# M9 완성도 hop — SYS-CORE 구현 보고 (RFC-CX-011 S-A·S-B·S-C·S-D·S-G + open-review-notes 계약)

기준 커밋 6a1a761 위. 커밋/푸시 없음(사용자 수행). Unity 배치 테스트 미실행(QA 단계 일괄 — 지시 준수). 기존 미커밋 reduced-motion fix(T0OpeningSession.cs·M5DirectionPlayModeTests.cs) 보존 — M5DirectionPlayModeTests.cs는 미수정, T0OpeningSession.cs는 그 위에 문자열 이관만 얹음.

## 소유 경계

RFC-CX-011 기본 경계 + 디렉터 승인 확장 2건(세션 중 irc 승인):
1. **T0CaseThreadTests.cs 편입** — two-step preview 단계 경유 반영. 추가로 S-D 판정 B의 귀결로 AssertCard 목표줄을 런타임과 같은 가드 경유 데이터 구동 기대값으로 교체(아래 §S-D). 약화 없음 — beats 미배선/가드 폴백 시 기대값이 기존 고정 문자열과 문자 동일.
2. **S-D 판정 B(디렉터)** — beats objective 표시는 disclosure-clean 비트로 한정. 구현은 하드코딩 비트 목록이 아니라 **objective에 record displayNameKo가 포함되면 폴백하는 데이터 구동 가드**. 현재 데이터에서 t0-b1만 가드에 걸림(objective가 "인수 각서"·"이관 목록" 포함).

## 변경 파일 · 구현 지점

| 파일 | 슬라이스 | 지점 |
|---|---|---|
| `App/T0GameSession.cs` | S-A | `int hintLevel` → `Dictionary<string,int> hintLevels` + `HintLevel` 프로퍼티(CurrentBeat 키); `OpenOverlay`의 `hintLevel=0` 리셋 제거(비트별 단계 재열람 보존); hints 오버레이 경고 게이트를 다음 행 `warnsBeforeReveal` 구동으로(C1 합성 행은 필드 부재 → level 3 진입 전 경고 폴백); `RestoreHintLevels(doc)`로 Initialize Decode 성공 분기·recovery retry·slot select에서 `progress.hintLevelUsed` 복원(부재 시 빈 사전); 힌트 단계 상승 시 `QueueSave()` |
| | S-B | `DefaultSettings()` 신설(static) — `confirmMode:"two-step"`; Initialize가 이를 사용(기존 settings.json은 그대로 우선) |
| | S-C | `PreviewDifferenceSentences(current,candidate,text,name)` static 순수 메서드 — 팩트(citation:/copy:/일반)·readCounts 차이를 템플릿 문장화; confirm/preview 오버레이의 T0 경로에서 `Simulation.Preview` 결과로 바뀌는 항목+되돌림 고지를 confirmExplanation 앞에 병기(C1 확정문 불변) |
| | S-D | `CaseObjective(beatsTable,beatId,recordNames,fallback)` static — disclosure 가드 포함; CaseThreadText가 사용(beats 미배선 시 기존 caseObjective 폴백); `GuidedTeachingText(toolId)` — tools.json `introBeatId`/`toolTeaching(guided)` 소비, 현재 비트 일치 시 도구 패널 본문 상단에 level1 힌트+잔여 술어 집계(`RequirementMet` 읽기 전용 투영) 병기 — CircuitScreen·ReaderScreen |
| | S-G | `SnapshotInterval`(policy `snapshotInterval`) → `new CommandJournal(...)` 2곳·`JournalSave.Decode` 5곳 전달(값 200 동일 → 동작 불변); Update() 유휴 힌트 게이트에 `document==null` 추가 |
| | 계약 | `open-review-notes` 액션 `()=>OpenOverlay("reviewNotes")` → `OpenReviewNotes`(SYS-M8 정의) 호출 |
| `App/T0OpeningSession.cs` | S-D(4) | L53-54 하드코딩 한국어 → `L("openingMotto")`·`L("openingMottoDetail")`·`L("openingReturn")`·`L("openingBeginWork")`·`L("openingSkip")` (ko 값 불변, en 추가). reduced-motion fix 코드 보존 |
| `App/T0RuntimeConfig.cs` | S-D(1) | `beats` TextAsset 필드 추가 |
| `Editor/T0ProjectBuilder.cs` | S-D(1) | Prepare에 `config.beats=Text("Data/Tables/beats.json")` 1줄(기존 hints 패턴) |
| `Save/JournalSave.cs` | S-A(4) | `Encode(...,IReadOnlyDictionary<string,int> hintLevels=null)` 선택 인자 — null=기존 빈 객체, 값>0만 기록. **저장 필드명 `hintLevelUsed` 불변** |
| `Resources/T0Strings.json` | S-C·S-D | 신규 키 13(ko+en): preview* 6, teaching* 2, opening* 5. 기존 항목 바이트 불변(접두 검증) |
| `Tests/EditMode/M9CoreTests.cs` (신규, +.meta) | 테스트 | 아래 목록 |
| `Tests/PlayMode/T0PlayModeTests.cs` | S-B | two-step 기본 전제 어서션 3곳 preview 경유 갱신 |
| `Tests/PlayMode/T0CaseThreadTests.cs` | S-B·S-D | two-step 경유 3곳 + AssertCard 목표줄 데이터 구동화(`ExpectedObjective()`) |

미수정: Sim/CommandJournal.cs·JournalSave.Decode 시그니처(이미 `snapshotInterval=200` 보유 — 신규 시그니처 불필요), T0ContractTests.cs(수정 불요 — T0Verification 위임 그대로 성립), SYS-M8 소유 파일 전부, 데이터 테이블 전부.

## 테스트

신규 `Tests/EditMode/M9CoreTests.cs` (NUnit, Tide.Tests 네임스페이스, TIDE_TEST_FRAMEWORK 가드):
- (a) `EncodePersistsHintLevelsUnderTheExistingFieldNameAndStaysDecodable` / `EncodeWithoutHintLevelsKeepsTheEmptyObjectCompatibility`
- (b) `DefaultConfirmationModeIsTwoStep`
- (c) `PreviewDifferenceSentencesDescribeCitationAndOriginalReadChanges`
- (d) `CaseObjectiveFollowsTheBeatsTableThroughTheDisclosureGuard`

갱신(약화 없음, 단계 추가만): T0PlayModeTests — `UiActionAdapterCompletesCanonicalT0AndReopensSavedState`, `CanonicalToolRoutingAndRebindingRetainIntent`, `FullInputRun`(Keyboard/GamepadOnlyCompletesAllThreeBeats 공용); T0CaseThreadTests — `CaseThreadFollowsLiveCitationsWithoutMutatingPuzzleOrNavigation` 3개 확정 지점 + `ExpectedObjective()` 도입. PlayMode 신규 테스트 0건(지시 준수).

## 검증 (명령 + 관측 결과)

- 소스 전체 어셈블리 체인 컴파일 [OBSERVED 2026-09-11]: mono csc + Unity 6000.5.6f1 엔진 모듈(`Unity.app/Contents/Resources/Scripting/Managed/UnityEngine/*`) + PackageCache Newtonsoft로 Tide.Sim→Save→Data→Input→Presentation→UI→**App**→EditorTools→Tests.Sim→Tests.Play **전부 exit 0**. App은 SYS-M8 계약 스텁 1줄(`public void OpenReviewNotes(){}` — 컴파일 게이트 전용, 저장소 미포함)과 함께 clean — SYS-M8이 partial에 실제 정의를 넣으면 그대로 성립.
- 스모크 실행 [OBSERVED 2026-09-11]: 실제 컴파일 산출물 + 실제 `Data/Tables/*.json`으로 M9Smoke 하니스(/tmp, 저장소 밖) 실행 — **11/11 PASS** (`M9SMOKE ALL PASS`): 힌트 사전 왕복·빈 사전 호환·two-step 기본·CiteToBoard/ReadOriginal diff 문장·항등 diff 0건·CaseObjective 4분기(clean 표시/가드 폴백/미배선 폴백/미지 비트 폴백). 데이터 로딩만 Newtonsoft로 대체(JsonUtility 네이티브 icall이 mono에서 불가) — Sim·Save·App static은 전부 실코드.
- Unity EditMode/PlayMode 배치 실행 0회(지시) — 정본 판정은 QA 일괄에서.
- graphify update . [OBSERVED]: 25740 nodes rebuilt. mex graph [OBSERVED]: 1195 nodes / 27 files.

## Open items (M9 범위 밖, 기록만)

1. **t0-b1 objective 무스포일러 재작성** — campaign.json 소유(planner). 현재는 disclosure 가드가 t0-b1을 폴백시킴. 재작성되어 record 명칭이 빠지면 자동으로 표시 경로에 들어온다(코드 변경 불요).
2. ~~`T0Runtime.asset`의 `beats` 배선은 Prepare 재실행 시점에 생김~~ → 디렉터가 `T0ProjectBuilder.WireBeats`(beats만 배선, Prepare 미실행)로 연결 완료(`completeness-m9/wire-beats.log`). 이후 EditMode/PlayMode 전 회차는 배선 경로에서 실행됐다.
3. ~~힌트 단계 상승 중 SavePending이면 QueueSave를 미루고 다음 확정 저장에 편승~~ → **FIX 1(D-M9-10)에서 변경**: `SaveHintLevels`가 pending 중 `hintLevelsDirty`를 세우고, `FlushHintLevelsIfDirty`가 commit 성공·실패·취소 시 즉시 `QueueSave` 1회 — 다음 저장 시점을 기다리지 않는다. (D-M9-14 문구 정정)
