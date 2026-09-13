---
updated: 2026-09-14
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-qa
---

# M23 · 구현 후 독립 리뷰와 수정 기록

## 역할과 판정 경계

`GoalReview`는 최초 R1–R10 목표/반례를 검토했다. 구현 후 `RuntimeAcceptance`는 R1/R7, `EvidenceAcceptance`는 R2/R3/R5를 독립으로 읽었다. 모두 **읽기 전용**이며 빌드·테스트·Git·네이티브 입력을 실행하지 않았다. 아래 실행 결과는 Main의 증거다. 초기 범위 검토의 no-findings는 후속 구현이 무결하다는 뜻이 아니다.

결함 상태 정본은 `qa/defect-register.md` §16의 C7-F56..F66이다. M23-Q1..Q11은 같은 순서의 진단 참조이며 중복 결함으로 계상하지 않는다. Q1–Q5는 초기35ebdc5, Q6–Q11은 후속 교정이다.

## 실제 발견과 수정

| ID | 심각도 | 반례 | 수정/경계 |
|---|---|---|---|
| M23-Q1 | S2 | working 자동저장 A와 hint 자동저장 B가 같은 표시 revision을 공유. A 성공이 revision을 바꾸면 최신 B 실패가 숨겨짐 | 저장 큐마다 독립 sequence. 최신 같은 작업공간의 실패는 화면 revision으로 숨기지 않음. 성공만 새 미리보기를 덮지 않도록 표시 revision도 검사 |
| M23-Q2 | S2 | `read-original` 미리보기의 화면상 `overlay-back`으로 닫으면 Preview 문구 잔류 | 공통 `DismissOverlay`로 제안/Preview를 해제. 대기 화면의 뒤로는 쓰기를 취소하지 않던 계약 유지. 미리보기 중 더 늦게 도착한 SaveFailed는 닫기로 지우지 않음 |
| M23-Q3 | S2 |150%에서 연습장 하단 설명이 넘치지만 오른쪽에 키보드 접근 경로 없음 | Navigation의 실제 페이지 위/아래 조작으로 기존 작업 ScrollRect를 이동. Render를 호출하지 않아 초점/스크롤을 초기화하지 않음. 하단 대응 요약과 상단 차트가 모두 viewport 안에 들어오는 실제 Enter 입력 회귀 |
| M23-Q4 | S2 · Q1 후속 반례 | commit이 기존 autosave를 무효화한 뒤 tip에서 Redo로 취소하면 Redo는 no-op이고 대체 저장이 없어 실패가 모두 사라질 수 있음 | `CancelPending` 결과를 모든 호출자가 소비. 변경 명령은 변경본을, no-op Undo/Redo와 거부된 일반 명령은 현재 작업본을 한 번 재저장. 작업공간 reset/새 commit의 소유권도 별도로 무효화 |
| M23-Q5 | S2 · 실제 네이티브 발견 | 작업저장 문구를 스크롤 내용의 첫 줄에 두어 깊은 C1 근거 선택/확정 초점에서 보이지 않음 | 오른쪽 고정 영역으로 이동하고 SetActionFeedback은 기존 Text만 갱신. Navigation 높이는 유지. 실제 깊은 초점과 쓰기 완료 전후 가시성·초점 불변 회귀 |

후속 검토에서 RuntimeAcceptance는 현재 소스가 원래 두 결함과 취소/경고 보존 반례를 처리하며 추가로 입증된 결함은 없다고 답했다. EvidenceAcceptance는 실제 페이지 동작·비재렌더·두 viewport 경계 검증이 제시한 결함을 해결하는 방향이라고 ACK했다. 이것은 정적 확인이며 사람 개념 이해나 Windows 성능 승인이 아니다.

## 실행으로 드러난 통합 문제

- 최초 컴파일: Unity6000.5 `CanvasRenderer.GetMesh()`를 인자가 필요한 API로 잘못 쓴 비교 테스트4곳을 수정했다. 반환된 renderer 소유 mesh를 임의 파괴하지 않는다. 새 UI의 비직렬화 모델 필드는 자동 property로 두었다.
- 잘못된 EditMode 어셈블리 필터의0건 결과를 통과로 인정하지 않았다. 실제 이름은 `Tide.Tests.Sim`이다.
- 실제 EditMode64건 중42건은 갱신 테이블과 기존 authoring catalog의 producer receipt 차이(V-4)로 setup에서 차단됐다. 정본 검증/emitter 이후 **기존 `T0AssetImporter.Import`만** 실행해 source SHA와 receipt를 검증/임포트했다. verifier를 완화하거나 `T0ProjectBuilder.Prepare`로 승인 씬을 재생성하지 않았다.
- 이 교정 뒤 EditMode64/64, 최초 통합 PlayMode109 pass/0 fail/격리 boot1 skip을 확인했다. 이후 리뷰 수정의 최종 결과는 M23 실행 영수증과 XML을 사용한다.
- `review-feedback-red.xml`: Q1·Q2 회귀2건 모두 실제 실패(최신 실패 대신 WorkSaved, 닫은 후 None 대신 Preview).
- `review-boundary.xml`: 수정한 화면 Back과150% 키보드 viewport는 통과, Q1을 화면 변경까지 확장한 경계는 실제 실패(SaveFailed 대신 Preview). 그 결과를 받아 실패 판단을 표시 revision에서 분리하고 Q4를 함께 수정했다.
- 최종 PlayMode/격리 boot/macOS 빌드와 실제 네이티브 시나리오는 `systems/tech-verification/m23/verification.json`을 정본으로 한다. 이전 XML의 통과 수를 반복 실행 횟수만큼 더하지 않는다.

## 미충족을 결함 통과로 바꾸지 않은 항목

R4의 강한 P1/P2 보존 감사는 증거 부족 **exit3/blocked**다. 최소 독립쌍 검사와 Node 회귀7건 통과는 명제별 대체 경로/양쪽 불파괴 전수 증명이 아니다. R3 C1 원본 복귀 제한은 기존 경계를 유지했으며 새 경로 구현으로 포장하지 않는다. R6 실제 사람0명, R8/R9 Base gate, R10 본편 시간/Windows 기준기와 실제 노력150% STOP 판단은 별도 미충족 상태다.

## 초기35ebdc5 수정안의 실행과 독립 재검토

- `feedback-visibility-red.xml`은 원래 배치에서 실제로 실패했다. 첫 고정안이 Navigation까지 줄여 `playmode-release.xml`의 기존 Band comparison150% 회귀가 실패했다. 오른쪽 Work Surface만 분리한 최종안은 `playmode-release-final.xml`112 pass/0 fail/격리 boot1 skip, 별도 `boot-release.xml`1/1이다. EditMode64를 합친 고유 수는177이다.
- Main이 최종 앱에서 미기록 후보 차단(48), 깊은 목록의 WorkSaved(49/50), 비적용 Preview(51), 실제2단계 확정의 CommitSaved(54), 프로세스 재시작 후 같은 저장/노트 바이트(55),150% 두 차트(59), 키보드 연습 readout/return/exit(61–63)를 관찰했다.55는 OS 재부팅이 아니다.
- RuntimeAcceptance는 Q1/Q2/Q4와 고정 피드백 배치를 정적으로 재검토하고 담당 범위의 추가 입증 결함 없음으로 종료했다. 네이티브 실행/테스트는 Main의 증거라고 명확히 분리했다.
- 전이 후 도구막대 초점을 놓친 매크로19는 연습장을 나간 뒤 일반 게임을 조작했으므로 격리 증거에서 제외한다.32–34의 합성 포인터 스크롤/드래그도 실제 이동이 관찰되지 않았다. 도구의 `ok`/`synthetic_input:unverified`를 게임 행동 성공으로 세지 않는다.

## 최초 전달 후 경계·진단 화면 교정

Main이 Q6를 추적한 뒤 PracticeIsolationFix와 PracticeContrastFix에 서로 다른 테스트 파일을 위임했다. 두 레인은 validation/프로덕션 편집 없이 회귀와 패치를 제안했고 Main이 실행·적용·빌드했다. Q9–Q11은 Main의 실제 창 관찰에서 발견됐다.

| ID / 정본 | 심각도 | 실제 반례와 교정 |
|---|---|---|
| Q6 / C7-F61 | S2 | loaded Plate가 유지되어도 Undo로 Ledger가 비가시화되면 pinned graph를 해제. Redo는 접근만 복구하고 pin은 복구하지 않음 |
| Q7 / C7-F62 | S2 | StartGame/OpenReviewNotes/SetConfirmMode/SaveReviewNoteAsync/ContinueToPatrol/ContinueToSignature를 mutation 전 차단. 합법 진행 fixture와 파일 목록/바이트 불변의 실제 RED6건 → GREEN |
| Q8 / C7-F63 | S2 | 대응 설명의 사용자 정의 이름이 M21 Text 스캔을 빠져나감. 기본 식별자 복원으로150% 흐름/배경/대비 회복 |
| Q9 / C7-F64 | S2 | native71의 비교 카드·CaseThread 대비 누락. 진단 전용 부모 배경으로 원래 색/이름/기본 스킨 보존. 카드와 CaseThread 각각 실제 RED |
| Q10 / C7-F65 | S2 | native95의 트랙과 A/B 라벨이 어두운 표면에 묻힘. 별도 부모 배경, 입력 비간섭, 실제 canvas mesh 불투명 획·라벨 대비 검증 |
| Q11 / C7-F66 | S3 | native99에서 양 끝 피크가 잘림. 기본 모드 mesh도 경계4.48단위 초과 RED.4% 가로 여백으로 양 모드 정점 경계 통과 |

- 최종 `correction-playmode-final.xml`:121 pass/0 fail/boot1 skip. 별도 `correction-boot-final.xml`:1/1. EditMode는 프로젝트64 + 외부 예제 stub1이므로 고유 프로젝트 수는186이다. 신규9사례를 확장·재실행한 RED12회를12개 신규 테스트로 더하지 않는다.
- 모든 후속 XML/11개 스틸/192개 입력·캡처 이벤트는 M23 영수증에 연결했다.86/89/90/92는 지문11a42388…의 reader 검증,99는35755c5c…의 대비 수정/피크 잘림,104/106/107/108은6c88870f…의 최종 검증이다.95와71은 부정 증거다.
- 최종 native에서 모든 피크·A/B 라벨·오차대·하단 대응 설명을 직접 보았고1/I/H 후 연습장이 유지됐다. Escape는 원래 reader로 돌아갔으며 save/save.bak/settings의 목록·바이트·해시가 전후 동일했다. 직접 API6개는 PlayMode 증거이지 native에서 외부 API를 호출한 증거가 아니다.
- frame87의 예상 Undo 파일명과 달리 실제 초점은 Cite였다. Enter를 보내지 않고 두 Tab 뒤 frame88의 Undo 초점을 확인했다.91은 t0-b2의 Redo 초점,92는 실제 Redo 후 t0-b3/고정 없음이다. 실패한 focus 복원/미이동 hotkey·scroll은 성공으로 세지 않았다.
- 최초 회귀 작성의 누락된 Tide.UI import로 생긴 컴파일 오류는 테스트 RED로 세지 않았다. 실제 실패 XML 이후에만 원인 교정을 적용했다. renderer 소유 GetMesh() 반환값은 파괴하지 않는다.
