---
updated: 2026-09-14
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-qa
---

# M23 · 독립 목표 검토와 합의

[OBSERVED] `GoalReview` reviewer의 읽기 전용 코드/기준 검토와 실제 IRC 응답을 Main이 기록했다. ReaderCompare·SignatureSources·AlignmentPractice·CanonConsistency 네 구현 레인이 기준을 ACK하고 아래 반례를 논의했다. 이 문서는 테스트 실행·사람 측정 영수증이 아니다. P1은 통합 전 확인할 우선순위이며 별도 Base 생산 게이트와 같지 않다.

| 권고 | 실제 코드/명세 근거 | 반례와 수용한 결정 | 필요한 증거 |
|---|---|---|---|
| R1 | T0GameSession.ReaderScreen/readerEvidence, ReaderComparisonSession | 원래 LoadRecord는 저널 명령이다. R1의 비변경 약속은 새 비교 전용 선택·구간·고정/해제에 한정하고 실제 적재/읽기/인용과 명시적으로 구별한다. 수치축·조위 시각·단위·NaN 결손을 보존한다 | 두 카드 150% 동시 표시, 키보드, 고정 후 선택/Undo, 초기화·저장 슬롯 선택·복구 재시도에서 reset; 저널/읽기/저장 bytes 불변 |
| R2 | C1SignatureGameSession, C1SignatureDefinition, C1SignatureData | 답을 후보에 미리 넣지 않는다. 현재 선택안과 과거 기록 쌍을 분리하고 dirty 후보는 UI 확정을 막되 이전 proof를 지우지 않는다. 새 payload 버전은 구 reducer/hash를 바꾸지 않는다 | 관찰 전 후보 부재, 원본/사본 동일 루트 거부, 역순 valid 쌍, Undo/Redo·replay·folded save·구 무표식 save |
| R3 | ReviewNotesSession.ObservedReviewSources/ReviewQuestionFor/ReviewSourceOriginalAvailable | 실제 관찰/생성된 사본만 노출. 질문은 명시 요청 시만 갱신. C1/도구 상태의 원문 복귀 거부는 의도적으로 유지하며 새 편의 기능으로 주장하지 않는다 | 사본 계보·매체·원본 수 분리, hidden ID/이름 미노출, 노트 저장/재진입, 질문 자동 생성 없음 |
| R4 | bible P1/P2, continuity §5.2, audit-campaign-preservation.mjs | 같은 root-pair 반복을 대안 경로로 세지 않는다. 첫 읽기 사본은 읽기 전 접근·양쪽 매체 불파괴의 증명이 아니다. 기존 9/10·10/10 문구의 강한 PASS 주장을 철회한다 | 최소 쌍 검사와 강한 보존 감사 별도 실행. authored mapping 부족의 blocked/exit3은 정상 미충족 판정이지 PASS나 억제할 오류가 아님 |
| R5 | tide-alignment A-R1/A-R2, AlignmentPractice, WatchInput | fitted residual이 작아도 각 사건의 보수적 오차 하한은 데이터 residualLimit. 겹침·접촉은 indeterminate. 별도 overlay에서 본편 툴을 유지하므로 전역 Undo/Redo/overlay 및 직접 API를 격리한다 | 실키 down/up·포인터·150%·잠금/해제·전이/초기화; Journal/hash/읽기/save 불변, CaseThread/완료 subtitle/연출/캠페인 탐색 미노출 |
| R6 | verification-plan H1–H3, App에 이벤트 producer 없음 | 문서 템플릿을 계측기로 세지 않는다. 명시적 대안은 타임스탬프를 보존한 네이티브 영상 주석. 첫 상호작용 프레임과 첫 유효 행동 정의, 2명 독립 H2 채점, 파일럿 비혼합 | 한 frozen build, 초회 12명/5유형, 중도 이탈 포함; 미측정 null, thinking/unknown AFK 차감 금지. 실제 참가자가 없으면 blocked |
| R7 | SubmitImmediate→QueueSave, CommitAsync, Simulation.Preview | TrialSignature는 저장되는 작업 상태다. 비적용·비영속은 preview clone/별도 연습장이다. 수락·작업저장·증거확정·실패를 구분하며 완료 전 저장 성공을 보여주지 않는다 | QueueSave/CommitAsync 각각 지연·실패·취소·상황전환에서 실제 완료와 표시가 일치, 늦은 비동기 완료가 새 상태를 덮지 않음 |
| R8 | 현행 종결은 C1-b2, JournalSave 후반 상태 배열 비어 있음 | 실제 재방문 생산 없음. Base 이후 첫 질문/변경조건/새 비교 구현 대상 | Base 네 조건 충족 전 blocked |
| R9 | C1InterviewPrepSession은 display-only, c1-b4 아님 | 면접 준비 패널을 NPC 증언 대안·3제출 관점으로 세지 않는다 | Base 네 조건 충족 전 blocked |
| R10 | T0→c1-b1→c1-b2와 격리 연습만 실행 가능 | 현재 슬라이스 회복/저장/네이티브 증거와 전체 캠페인·Windows 장치·사람 시간 증거를 분리한다 | 현행 범위 회귀/빌드 우선. 전체 450–540분과 목표 장치 성능은 NOT-MEASURED |

## 검토 정정과 범위

- [OBSERVED] WatchInput는 native Query/Tool/Disconnect/Adjust에 이미 OverlayActive guard가 있다. 이 경로를 기존 네이티브 유출 결함으로 기록하지 않는다. 직접 메서드 방어와 전역 Undo/Redo/Overlay는 통합 증거 대상으로 유지한다.
- [OBSERVED] SignatureScreen 실제 위치는 App/C1SignatureGameSession.cs다. 해당 writer가 본문과 데이터 생성자 callsite를 직접 통합하도록 소유권을 정정했다.
- [OBSERVED] GoalReview가 작성된 R1/R2/R3/R4/R5 코드·회귀를 읽고 추가 정적 결함을 찾지 못했다고 보고했다. 실행/네이티브 통과를 뜻하지 않는다.
- [TARGET] Main은 테스트와 실제 native 입력·동시 카드·save 실패를 실행하고, 결과를 독립 재검토하여 수정한다. 이 문서 작성 시점에 신규 실행값을 만들지 않았다.

## Base 판정

`production/premium-preproduction-contract.md` 네 조건을 그대로 적용한다. 사람12/5 H1–H3, 별도 조위정합 이해 결과, 최초 추정 대비 실제 T0 제작 노력과150% STOP 적용, 현행 open-S1=0·freshness 원문이 모두 필요하다. 기존 pass 표는 carried evidence다. 실제 노력 기록의 부재는0이 아니며 사람 n=0도 PASS가 아니다. 개발 빌드/일반 Git push 승인은 생산/상점 출시 승인이 아니다.

## 우선순위·소스 색인과 노출 경계

- R1–R7: **P1 인수/통합 증거**, R8/R9: **Base gate-blocked**, R10: **P2 범위/증거**. 이 우선순위를 미실행 S1 결함으로 재분류하지 않는다.
- 코드 정본: `unity/Unknown/Assets/_Project/App/{T0GameSession,ReaderComparisonSession,C1SignatureGameSession,C1SignatureData,ReviewNotesSession,AlignmentPracticeSession,T0ActionFeedbackSession}.cs`, `unity/Unknown/Assets/_Project/Sim/{C1SignatureDefinition,AlignmentPractice}.cs`, `unity/Unknown/Assets/_Project/UI/{T0Interface,ReaderComparisonView,AlignmentPracticeView}.cs`, `unity/Unknown/Assets/_Project/Input/WatchInput.cs`.
- 문서/감사 정본: `_workspace/current/worldview/worldview-bible.md` §3-bis.3, `_workspace/current/synopsis/continuity.md` §5.2, `_workspace/current/systems/system-specs/tide-alignment.md` A-R1/A-R2, `_workspace/current/planning/audit-campaign-preservation.mjs`, `_workspace/current/handoff/verification-plan.md` §1, `_workspace/current/production/premium-preproduction-contract.md`.
- **AFK 회고 누락은 afk_total_min=null·total_minus_afk_min=null이며 유효 판정 표본에서 제외하고 미회고로 별도 보고한다.** 0이나 이탈로 바꾸지 않는다.
- R5는 Editor 또는 명시적 네이티브 `--m23-alignment-practice` 실행에서만 진입한다. 정식 T0 초회 빌드는 그 인자를 사용하지 않으며 연습장 진입 UI가 없다. 같은 빌드 지문과 별도로 인자를 기록한다. 연습장 노출 참가자는 파일럿/전이 표본이며 정식12명으로 섞지 않는다.

## 구현 후 종결 증거

[OBSERVED 2026-09-14] 이후 Main 실행과 RuntimeAcceptance/EvidenceAcceptance의 독립 정적 리뷰에서 Q1–Q5를 발견·수정했다. 원재현 RED와 중간 실패, 최종177개 고유 Unity 통과 및 실제150% 네이티브 증거는 `qa/m23-integration-review.md`와 `systems/tech-verification/m23/verification.json`을 따른다. 위 초기 TARGET/정적 검토를 실행 증거로 소급하지 않는다. R6/R8/R9/전체R10 및 강한 P1/P2 보존 입증은 계속 차단이다.

[POST-PUBLICATION] 초기35ebdc5 뒤 후속 Q6–Q11의 가시성·직접 API·진단 대비·끝점 경계를 교정했다. 추가9개 고유 회귀를 포함한 최종 프로젝트 Unity186통과와 빌드별 native 증거는 위 영수증의 postPublicationCorrection을 따른다. PracticeIsolationFix/PracticeContrastFix는 분리 테스트/패치 제안을 했고 실제 검증은 Main 소유다. 초기 목표 합의·미측정 gate는 소급 변경하지 않는다.

[FINAL FOLLOW-UP]5d7b18c 이후 같은 Q6의 off-reader Undo/Redo 재발을 실제 RED로 확인하고 공통 Render에서 해제하도록 보강했다. 최신187개 고유 프로젝트 통과 및 native112–117은 영수증의 offReaderLifetimeFollowUp에 있다. 위177/186개 및 이전 네이티브는 각 빌드의 역사다.
