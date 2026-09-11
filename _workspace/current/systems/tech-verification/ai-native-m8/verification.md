---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-production-director
---

# M8 검토 노트 구현·검증

[OBSERVED] 사용자 지정 [AI 게임 사례 글](https://mp.weixin.qq.com/s/YmqCm2Hh8l6WPTIAIEMFhQ)에서 상황 대조, 자유로운 가설 표현, 문맥 보존을 가져와 **선택형 오프라인 검토 노트**를 구현했다. 원문은 2차 자료이며 시장·비용 수치를 독립 검증하거나 이 게임의 성과 근거로 사용하지 않았다. [출처 영수증](source-review.json), [독립 원문 검토](source-independent-review.md).

## 실제 변경

- 자료 목록에서 노트를 열어 자기 가설을 작성하고 직접 관찰한 T0/C1 출처를 연결한다. 질문은 출처 수·원본 관계에 따른 저작된 문장이다. 자유문장 의미 평가, 새 사실 생성, 모델 호출은 없다.
- 노트는 저장 식별자별 별도 파일에 보관한다. 진행 저장과 식별자가 일치해야 보관할 수 있으며, 첫 저장 이전에는 실행 중 초안이라는 상태를 표시한다. 게임 journal/schema와 증거 확정·해금 권한을 변경하지 않는다.
- 손상·미래 버전 파일을 덮어쓰지 않고 저장 실패 때 초안을 유지한다. 저장 중 텍스트나 연결 출처를 바꾸면 미보관 상태를 유지한다.
- 관찰하지 않은 출처와 내부 ID를 표시하지 않는다. 편집 중 게임 단축키와 IME 조합 종료 직후 키 간섭을 방어한다. 게임 저장 완료 후 노트 가용성은 편집 종료까지 갱신을 미루며 매 프레임 디스크를 읽지 않는다.

## 정확한 검증 범위

[OBSERVED] Unity6000.5.6f1, base `01bec3cb7a7431c4069633f0bf321a02f14ccd79` + [13개 명시 소스](delivery-source-manifest.json)만 `/tmp/unknown-ai-native-m8-20260911/unity/Unknown`에 적용했다. 기존 미커밋 T0OpeningSession.cs, M5DirectionPlayModeTests.cs와 M5로그를 포함하지 않았다. [독립 코드 검토](code-review.md)는 최종 attempt5 해시와 대조했다.

| 검사 | 실제 결과 | 영수증 |
|---|---|---|
| EditMode | 46/46 통과 | [XML](editmode.xml) · [log](editmode.log) |
| M8 PlayMode 부분집합 | 10/10 통과 | [통합 XML](playmode.xml) · [log](playmode.log) |
| 기존 T0/C1/M5 + M8 PlayMode | 64/65 통과, 기존 M5 실패1 | [XML](playmode.xml) · [log](playmode.log) |
| 변경 전 M5 재현 | 같은 검사·같은 assertion 실패1/1 | [XML](baseline-reduced-motion.xml) · [소스](baseline-source-manifest.json) |
| 직렬화 부트 | 1/1 통과 | [XML](boot.xml) · [log](boot.log) |
| macOS 개발 빌드 | 성공(exit0) | [log](build-mac.log) |

[OBSERVED] M8 10개는 통합65개에 포함되어 중복 합산하지 않는다. 최종 UI 수정 뒤 전체 EditMode와 PlayMode를 재실행했다. 검증 명령·시간·해시는 [verification.json](verification.json)에 기록한다.

[OBSERVED] 통합 실패는 `ReducedMotionStartsFreshGameImmediatelyWithoutWrites`의 M5 테스트50행, Expected False / But True다. 전체 M8 오버레이를 제거하고 git archive 기준 커밋을 복원해 같은 실패를 재현했다. HEAD의 reduced-motion 첫 시작은 OpeningActive를 켜고 타이머를 정지시키며, 같은 M5 파일의 다른 검사는 그 활성 상태를 요구한다. 이 작업은 기존 미커밋 인트로 수정의 대체 검증이나 승격이 아니다. 따라서 전체 회귀를 녹색으로 보고하지 않는다.

[OBSERVED] 최종 macOS 창에서 명시적 편집 → 한국어 붙여넣기 → Tab 종료 → 포인터 보관을 수행했다. 저장된 텍스트는 입력 문장과 정확히 같고 선행 Tab이 없었다. `<b>...</b>`도 서식 명령이 아닌 원문으로 표시됐으며 게임 save.json 바이트는 그대로였다. [실제 창 검사 영수증](player-smoke.json), [보관 노트](native-tab-final-note.json). CUA 붙여넣기 도구는 clipboard-read timeout을 반환했지만 화면과 보관 파일로 실제 성공을 확인했다. 이는 물리 IME 조합 검수가 아니다.

## 실패·재실행 이력

- attempt1: 테스트 Key.Alpha1 컴파일 오류 → 실제 InputSystem Key.Digit1로 수정.
- attempt2: EditMode40/46. InvalidDataException 처리 누락6건 → Load/SaveAsync 예외 필터 보완 후46/46.
- attempt3: 전역 --t0-save-dir가 개별 fixture를 덮어써 저장이 공유됨. fault gate 이전 assertion 실패 후 정리 대기가 끝나지 않아 실행 중단; 완결 XML 없음. 이 실행은 테스트 통과 근거가 아니다.
- attempt4: 전역 저장 인자 제거, 실제 fixture 경로 확인, 15초 제한, 단계 로그와 finally gate 해제 후 M8 및 회귀 재실행.
- 부트 최초 실행은 필수 저장 경로 인자 없어1개 ignored. 전용 필터만 별도 고유 경로 인자를 전달해1/1 통과.

- attempt5: 실제 macOS 창에서 붙여넣기→Tab 종료 후 선행 탭 문자가 저장되는 결함을 확인. 문자형 Tab과 편집 종료 뒤 후속 이벤트를 차단했다. [수정 전 새 테스트](native-tab-red.xml)는 실패하고 최종 통합의 같은 테스트는 통과했다. 기존 보관 메모를 자동 정리하거나 변환하지 않는다.

## 경계와 후속 작업

[CARRIED] 실제 한국어 IME/컨트롤러 기기 검수, 사람 플레이테스트, 몰입·재방문 향상, 성능 및 전체 캠페인 완성은 이 자동화 검사로 입증하지 않는다. 명시적 질문 요청 버튼, 특정 출처로 초점 복귀와 전환시간은 [연출 문서](../../../presentation/ai-native-m8-direction.md)의 TARGET이다.

[CARRIED] 아트는 원본 세계관·초기 컨셉 → M7 영상 목표 → GTI 텍스처·리소스 → 새 프리팹 재구성 순서를 유지한다. 이번 작업에는 리소스 생성·외부 추론·의존성 추가가 없다. 기존 런타임/M5/M6 아트를 새 시각 원전으로 사용하지 않는다.

[OBSERVED] 최종 코드 변경 뒤 graphify update . 완료(exit0). 공유 graph 산출물은 기존 미커밋 변경을 포함하므로 이번 커밋에서 제외한다. mex-agent는 identity probe 실패(PATH mex는 TeX)로 graph/check/log를 수행하지 못했다. 프로젝트 .mex 수동 기록과 wiki 보고서는 성공한 mex log를 뜻하지 않으며 G8 전체PASS도 주장하지 않는다.
