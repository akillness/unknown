---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
---

# M9 완성도 hop — SYS-M8 구현 보고 (RFC-CX-011 S-E·S-F)

[OBSERVED] SYS-M8 서브에이전트가 구현을 마친 뒤 보고 단계에서 세션 경계로 종료되어, 이 보고는 디렉터가 실제 diff·파일을 검수해 재구성했다. 코드가 정본이고 이 문서는 색인이다. 기준 커밋 6a1a761 위. Unity 배치 테스트는 QA 단계에서 일괄 실행(영수증: 같은 폴더 editmode/playmode).

## 변경·생성 파일 · 구현 지점

| 파일 | 슬라이스 | 내용 |
|---|---|---|
| `App/ReviewNotesSession.cs` | S-E1 | `ShowReviewQuestion()` — `검토 질문 보기` enabled 액션으로 전환. 누를 때만 질문·구조 요약 계산 후 세션 필드에 고정(`reviewShownQuestion/Structure/Summary`). 화면 재구성·출처 토글은 재계산하지 않고, 연결 구조가 바뀌면 `이전 질문 · … 다시 누르면 갱신합니다` 라벨만 표시(`ReviewStructureKey` 비교) |
| | S-E2 | `OpenReviewSourceOriginal(recordId,sourceId)` — record: 출처의 `원문 열기` 보조 액션이 overlay=null·document=recordId로 이동, 복귀 플래그·노드·초점 id 기억. `ResumeAfterReviewSourceOriginal()`이 기존 매 프레임 가용성 경로(`UpdateReviewNotesAvailability`)에서 document가 닫힌 것을 관찰해 reviewNotes 오버레이 재개 + `Interface.Focus("review-source-"+id)` 복원. overlay/tool/node 이탈 시 복귀 플래그 해제(오탈출 방지). patrol:/signature: 출처는 제외(원문 화면 없음) |
| | S-E3 | `ReviewQuestionFor`에 독립 매체 분기 추가: 연결 2개↑·원본 중복 없음·`SourceType` 1종뿐이면 `서로 다른 매체에서도 이 해석을 확인할 수 있나요?`, 매체 2종↑이면 기존 대조 질문. 순서 0개→1개→동일원본→매체부족→대조 (planning/ai-native-m8-reference-application.md 표와 일치). static 순수 함수 유지 |
| | S-F | `OpenReviewNotes()`(직전 overlay 기억→열림 전환)·`CloseReviewNotes()`(닫힘 전환 완료 후 직전 overlay 복원, 기본 evidence)·`StartReviewPanelTransition`/`AdvanceReviewTransitions`(CanvasGroup alpha, `Time.unscaledDeltaTime` 구동)·`ReviewVfxMs(key)`(Resources/M8ReviewVfx.json 저작 수치, `ReducedMotion`이면 `reduced_motion_ms`=0 즉시)·`ReviewProfile`/`ReviewDirectionEnabled`(`runtimeApproved \|\| --m8-review-notes-diagnostic` 게이트, 에셋 부재 시 null 폴백)·관측 프로퍼티 `ReviewShownQuestion`/`ReviewPanelTransitionActive`. 닫는 동안 `ReviewPanelGroup.interactable=false` |
| `UI/T0ReviewNotesInterface.cs` | S-F | `ReviewNotesView`에 `Question`/`QuestionLabel`/`CardTexture` 추가. `RenderReviewNotes`: CardTexture 존재 시 InputField 뒤 RawImage 백드롭(raycastTarget=false — 클릭 보전), 부재 시 현행 단색. 질문 패널(라벨+본문) + `ReviewPanelGroup`/`ReviewQuestionGroup` CanvasGroup 노출, Clear 시 null 복원 |
| `Presentation/M8ReviewNotesProfile.cs` (신규) | S-F | ScriptableObject{cardPaper, runtimeApproved} — M5DirectionProfile 선례 |
| `Resources/M8ReviewVfx.json` (신규) | S-F | open 180 / close 140 / question 120 / reduced_motion 0 (ms), `timing_source: presentation/ai-native-m8-direction.md`. 코드 하드코딩 0 |
| `Editor/M8ReviewNotesProjectBuilder.cs` (신규) | S-H 연결 | 메뉴 `Tools/M8/Import review card`: `assets/generated/2d/texture/m8-review-card-r01/image.png` sha256 대조(65fc439b…) 후 `Art/Candidates/m8-review-card/CardPaper.png` 복사·임포트, `Resources/M8ReviewNotes.asset` 생성(runtimeApproved=false), import-audit.json 기록. 소스 부재 시 경고 후 안전 스킵(무텍스처 폴백 유지). `Tools/M8/Approve review card`: 검수 후 승격 전용 |
| `Tests/EditMode/ReviewNotesTests.cs` | S-E3 | 매체 분기 케이스 추가(단일 매체 vs 복수 매체 vs 동일 원본 우선순위) |
| `Tests/PlayMode/ReviewNotesPlayModeTests.cs` | S-E1·E2·F | 질문 버튼 누르기 전 미표시/누르면 표시·고정, 원문 열기→닫기→복귀+초점, reducedMotion 전환 0ms 즉시 완료 — 기존 fixture 패턴 준수(전역 저장 인자 없음) |

## 계약 확인

- `OpenReviewNotes()` 시그니처: `public void OpenReviewNotes()` — SYS-CORE의 open-review-notes 액션이 호출(T0GameSession.cs 확인).
- 저장·확정·해금 권한 불변: 노트 저장은 기존 `ReviewNotesStore` 경로 그대로, 시뮬 상태 역기록 없음(연출은 스냅샷 읽기 전용 — CLAUDE.md §9).
- 질문 규칙: 누를 때만 갱신·타이핑마다 재계산 없음·정답/확신도 배지 없음 (ai-native-m8-direction.md §화면 구조).
