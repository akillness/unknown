---
updated: 2026-09-14
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-production-director
---

# M23 · Aside 권고 구현·검증 계약

## 사용자 지시와 실행 범위

[OBSERVED] 2026-09-13 사용자가 계획 구현, 서브에이전트와 목표/기준 논의, 디버깅·개선, 모든 단계 진행, Git push와 빌드, 마지막 리뷰·개선 계획을 명시 요청했다. 이번 요청은 이 작업의 명시적 pathspec stage/commit/일반 push를 승인한다. 예전 ‘사용자 전용 commit/push’는 이전 요청의 경계이며 이번 승인에는 적용하지 않는다. force push·타 세션 변경 되돌림·Steam 공개·결제 승인은 아니다.

시작점: main b2ae1fb8, staged 0/unstaged 66/untracked 73의 기존 M22·리서치 작업을 보존한다. 현재 OMP 활성 peer는 없다. 같은 C7 사전제작의 M23 실행이며 역사적 출력은 덮어쓰지 않는다.

전체 권고 R1–R10을 추적한다. **Base production gate 네 조건은 유지한다.** R6의 실제 12명/5유형·사람 이해 및 R10의 실제 목표 Windows 하드웨어/전체 완주 시간은 에이전트나 자동 테스트로 만들지 않는다. R8/R9의 본 생산은 그 게이트에 종속한다. 먼저 모든 실행 가능한 시편/구현/빌드/리뷰를 끝내고 실제 외부 조건이 없으면 해당 항목만 명시적으로 block한다. 자료·법칙·숨은 서사를 날조하여 범위를 채우지 않는다.

## 공통 불변식

- sim이 진실을 소유한다. UI/손/차트는 읽기 전용 표현이다.
- 무료 연습/Undo/힌트, 첫 읽기 보존, 독립 근거의 sourceType AND 루트 originId, v1/v2/v3 세이브 호환, 180/180초 제안 계약을 보존한다.
- 입력 하나당 명령 하나, 키보드 순차 경로, 150% 글자, 모션 축소와 중단 취소를 지킨다.
- 내부 ID·미열람 이름·정답 자동 선택/인용·자동 후보 제거를 UI에 추가하지 않는다.
- 튜닝 수치는 저작 데이터에 둔다. T0 생성 테이블은 emitter만 쓴다.
- 현재 M8 노트·경고·sourceType/루트 분석을 재사용한다. AI 서비스/새 증거 DB를 만들지 않는다.
- 테스트는 실제 소비자 행동/경계/복구를 보호하는 것만. 문구·배선·기본값 snapshot 검사는 추가하지 않는다.

## 구현/통과 기준

| id | 구현 목표 | 기계·네이티브 인수 기준 | 사람/생산 증거 경계 |
|---|---|---|---|
| R1 | T0 두 자료와 선택 시간창 동시 비교 | 현재 자료와 사용자가 고정한 다른 자료의 제목/구간/단위/결손을 동시에 보존. 고정/해제/자료 전환은 Journal·읽기 횟수·인용을 바꾸지 않음. 매 렌더에서 고정 출처의 현재 가시성을 검사하며 Undo로 접근이 사라지면 해제, Redo로 자동 복원하지 않음. 키보드/150%와 M20 진단 배경에서도 차트·설명을 읽을 수 있음 | 탐색 편의가 추론 향상이라는 주장은 금지 |
| R2 | C1-b2에서 사용자가 근거를 고른다 | 공개된 원문/사본 중 좌·우를 선택. 같은 루트/같은 매체를 형식 사유로 거부. valid selection만 기존 SelectSignatureProof로 기록. 다른 선택안과 이미 기록한 근거 구분. 세이브 replay/Undo/Redo 보존 | 완료율만으로 자력 이해 PASS 금지 |
| R3 | 원본·사본·인용·독립성 표시 및 기존 M8 연결 | 관찰한 출처만 표시. M8 질문은 요청 시만 생성. 같은 루트·같은 매체를 별도로 설명, 숨은 이름/ID 비공개. 원문 복귀는 UI 도구 입력/진행을 침범하지 않음 | 연결선 없는 전이 실험은 별도 파일럿 |
| R4 | 문구/검사 강도 정합 | README 폐국 시점·확정 기본 라벨·현재 save 버전·t0-b3 끝점 힌트 정정. P1 2쌍/P2 불파괴 약속은 약화하지 않음. C-07의 최소1쌍 검사와 강한 보존 감사를 분리하며, 근거 없는 항목은 NOT-MEASURED/blocked | 새 출처/근거를 날조해 강한 감사 PASS 금지 |
| R5 | 실제 조작 가능한 조위정합 회색상자 | 일반 시편 데이터의 세 피크를 수동 대응/조절, 제안은 수동 적용, residualLimit 데이터, lock/unlock/reset, gap <= errA+errB는 indeterminate. 직접 세션 API까지 포함해 진행·설정·노트·저장 바이트 불변. Back만 연습을 종료하고 원래 작업면으로 돌아감. 설명은 Text/M21 읽기 배경, 트랙은 별도 부모 배경으로 150% 진단 화면의 A/B 라벨·실제 불투명 mesh 획 대비를 보존 | C3 콘텐츠 구현/사람 개념 이해 PASS 아님 |
| R6 | 한 T0 빌드의 사람 관찰 실행 | 기존 프로토콜에 맞는 캡처/기록/채점 패키지, 첫 행동 시점·AFK와 미측정 분리. 파일럿과 초회 표본 분리 | H1 median<=60s, H2>=10/12, H3=0/12, 12명/5유형은 실제 참가자가 없으면 block |
| R7 | 조작 수락·시험 결과·내구 저장 피드백 구분 | 기존 손/크랭크 수락·취소를 보존. 사용자는 수락/시험/기록완료·실패를 문자와 시각으로 구별. 실패에 성공 연출 없음. 새 유료/후보 오디오 승격 없음 | 손맛/피로 개선은 사람 관찰과 별개 |
| R8 | 재방문 질문·현재 상태 | Base gate 후 구역별 첫 질문/변경조건/새 비교축을 실제 적용 | gate 전 본 생산 시작 금지 |
| R9 | NPC 증언·제출 관점 | Base gate 후 사실/증언/의도와 대체검증, 공통 사실의 3제출 관점 | 새 과거 사실·별도 3캠페인·결말 자격 손실 금지 |
| R10 | 전체 도달/재진입/복구/시간/성능 | 실행 가능한 현재 슬라이스 회귀와 빌드 우선. 전체 캠페인 및 Windows 목표 장치·인간 시간은 별도 실제 증거 | 480분 표/에이전트 주행으로 8시간 PASS 금지 |

## 공유 코드 계약과 소유권

모든 writer는 `.claude/agents/game-systems-designer.md` 및 필요한 파일 기반 레인 정의를 읽는다. 처음에 기준에 대해 Main에 ack/counter를 보낸다. 반례가 있으면 구체적으로 제안하고 기준을 조용히 바꾸지 않는다.

**Main 통합 소유:** `App/T0GameSession.cs`, `UI/T0Interface.cs`, `Tests/PlayMode/T0PlayModeTests.cs`, 공통 Editor/build, production/decision-log·manifest·changelog, CLAUDE/ROUTER, 빌드/런타임 검증/스테이징/푸시. 다른 writer는 이 파일을 직접 편집하지 않는다.

### ReaderCompare 소유 (R1)
- 새 `App/ReaderComparisonSession.cs`, `UI/ReaderComparisonView.cs`(T0Interface partial 포함), 관련 전용 테스트.
- `void ReaderComparisonScreen(GameScreen screen)`를 구현. Main이 ReaderScreen 마지막에 호출한다. 비교 상태/버튼을 채우고 필요한 경우 기존 `screen.Chart`를 null로 해 단일 차트를 대체한다.
- `Tide.UI.ReaderComparisonView`를 정의. Main이 `GameScreen.ReaderComparison` 필드와 `RenderReaderComparison(RectTransform parent, ReaderComparisonView view)` 호출을 추가한다.
- UI helper `FlowText`, `Rect`, `Panel`, 색/scale은 기존 T0Interface partial에서 재사용한다. 대상 범위의 결손/평탄값과 서로 다른 측정 단위를 보존한다.
- 세션별 보기 상태만 고정하며 새 게임/저장 슬롯 전환에는 해제한다. `void ResetReaderComparison()` 훅은 Main이 실제 컨텍스트 교체 지점에 호출한다.
- 출처 표시는 다른 writer의 새 helper에 의존하지 말고 현행 `Definition.Records`, `Definition.ResolveRoot`, `ReviewMediaName`를 사용한다. UI에 originId 원문을 노출하지 않는다.

### SignatureSources 소유 (R2/R3)
- `App/C1SignatureGameSession.cs`, `App/ReviewNotesSession.cs`, `UI/T0ReviewNotesInterface.cs`, 필요시 `Sim/C1SignatureDefinition.cs`, 해당 C1Signature/ReviewNotes 테스트. T0Simulation 공유 수정 필요 시 먼저 Main과 조정한다.
- 새 소스/사본은 기존 signaturePacket/Definition만 사용. 공개된 사본 계보를 M8에도 연결하되 읽지 않은 기록은 보이지 않게 한다.
- 기본 C1 선택을 답으로 미리 채우지 않는다. 저장된 기존 proof는 보존한다. 후보를 바꿨다고 이전 유효 기록을 몰래 삭제하지 않는다.
- Main 공유 파일을 바꿔야 하면 메서드/호출 위치만 요청한다.

### AlignmentPractice 소유 (R5)
- 새 `Sim/AlignmentPractice.cs`, `App/AlignmentPracticeSession.cs`, `UI/AlignmentPracticeView.cs`, `Resources/M23AlignmentPractice.json`, 관련 전용 테스트.
- `void AlignmentPracticeScreen(GameScreen screen)`와 `public void OpenAlignmentPractice()` 구현. 전용 overlay 이름 `alignmentPractice`. Main이 OverlayScreen dispatch와 명시적인 별도 연습장 진입 버튼을 통합한다.
- `Tide.UI.AlignmentPracticeView`와 `RenderAlignmentPractice(RectTransform parent, AlignmentPracticeView view)` 구현. Main이 GameScreen 필드/UI 호출 추가.
- 일반 시편만 쓴다. 후반 이야기 데이터/이름을 가져오지 않는다. 시편 설정은 JSON에 두고 새로운 캠페인 세이브/진행을 만들지 않는다.
- `void ResetAlignmentPractice()` 훅 제공. 별도 연습 상태와 게임 상태 혼합 금지. 기존 기준선 잔차4/정밀1분/원시오차40의 근거는 tide-alignment.md이고 시편 데이터에 둔다.

### CanonConsistency 소유 (R4)
- README의 폐국 시점 한정, `Resources/T0Strings.json` 기본값 라벨 한정, 필요한 `synopsis/t0-records.md`, `systems/interaction-rules.md`, save 관련 현재 스펙 문구, `planning/validate-campaign.mjs` 또는 별도 보존 감사(기존 패턴 우선), 그 감사 테스트/산출물.
- `campaign.json`·바이블/연표의 의미 변경이 필요하면 먼저 Main에 counter; 역사·출처 창작 또는 약속 약화 금지. 새 전수감사를 위해 authored mapping이 부족하면 사실대로 미충족 출력과 소유자를 명시한다.
- emitter와 Unity 임포트/테스트는 실행하지 않고 변경된 원본과 Main 실행 명령을 인계한다.

### GoalReview 소유 (QA 독립 검토)
- `.claude/agents/game-qa.md`를 읽고 이 기준/현재 코드의 실패 가능성을 검토한다. `qa/m23-goal-review.md`만 작성 가능. 심각도와 근거, ack/counter, 사람/기계 경계를 기록한다.
- 작성 레인의 역할별 기준에 반례가 있으면 Main과 해당 peer에게 알려 실제 논의를 남긴다. 코드 구현/테스트 실행/전체 감사 확장은 하지 않는다.

## 검증·리뷰 루프

동시 writer는 formatter/linter/빌드/테스트/Unity/graphify 갱신을 모두 건너뛴다. Main이 통합 후 한 번 실행하고 실패한 경로만 재현→수정→재검증한다. writer는 근거 있는 회귀를 작성할 수 있으나 테스트 실행은 하지 않는다. Unity/Blender/빌드 프로세스는 Main 단독 소유다.

기계 검증 → 실제 macOS 네이티브 입력/150%/저장 복구 → 독립 reviewer가 결과/코드/기준 대조 → 결함 수정 → 필요한 경로 재검증 → 실제 빌드 지문/명시적 Git pathspec/push → 최종 리뷰 및 개선 계획. 새 아트 없이 기존 승인 리소스를 사용한다. Windows 빌드/실행은 설치 모듈·실행 장치 확인 후 가능한 범위를 구분한다.

## 최종 통합 경계

[OBSERVED 2026-09-14] R5는 Editor 또는 `--m23-alignment-practice`에서만 접근한다. R7은 큐 sequence/표시 revision 소유권과 취소 후 작업저장을 보완했고, 실제 깊은 스크롤에서 발견한 Q5를 오른쪽 고정 피드백 영역으로 수정했다. Navigation 높이는 유지해 기존150% 비교를 보존한다. 최종 실행·RED·네이티브 증거·외부 차단·개선 순서는 `handoff/m23-results-and-improvement-plan.md` 및 `systems/tech-verification/m23/verification.json`이 소유한다. 기존 계약의 Base 조건을 낮추지 않았다.

후속 경계 교정에서는 Main이 `PracticeIsolationFix`에 T0PlayModeTests의 회귀 작성, `PracticeContrastFix`에 T0ReadingBackingPlayModeTests의 회귀 작성을 명시 위임했다. 두 레인은 프로덕션 패치를 제안만 했고 Main이 RED 실행 뒤 적용·통합했다. ReaderComparison/CaseThread 대비의 실제 네이티브 발견과 교정도 Main 소유다. 화면의 pending 닫기는 쓰기를 계속하고 Esc/Undo는 취소하는 구분을 유지한다.

## 최초35ebdc5 이후 교정

- Q6–Q11/C7-F61..F66을 추가로 닫았다. Undo로 비가시화된 pin을 해제하고 연습장 직접 세션 API6개는 mutation 전 반환한다. pending 화면 Back과 Esc/Undo 취소의 기존 구분은 바꾸지 않는다.
- M21 Text 규칙과 이름 있는 CaseThread를 유지하면서 진단 카드/트랙의 부모 읽기 배경을 분리했다. 기본 색/상업 승인 상태는 그대로다. 실제 mesh 끝점 반례로 가로 피크 여백은 양 모드4%로 수정했다.
- 분리 테스트 파일에 한해 PracticeIsolationFix/PracticeContrastFix가 회귀/패치 제안을 맡았고, validation과 프로덕션 통합은 Main이 전담했다. 최종 고유186(프로젝트 EditMode64 + PlayMode121 + boot1), 신규9사례 RED→GREEN이다. 외부 stub1/RED 반복12회를 중복 계상하지 않는다.
- 최종 Mac315파일/410875885B·6c88870f…와 native104/106/107/108의 모든 피크/라벨/설명/복귀·3파일 바이트 불변을 확인했다. Undo/Redo native86/89/90/92는 중간11a42388…에서 얻은 별도 증거다. 전체 지문/소스/부정 증거는 `systems/tech-verification/m23/verification.json`을 따른다.
