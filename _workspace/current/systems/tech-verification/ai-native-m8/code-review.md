---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-qa
---

# M8 가설노트 코드 경계 독립 리뷰

[OBSERVED] **STATIC-REVIEW-PASS — 아래 해시에 고정한 좁은 범위.** 저장·출처·텍스트 렌더링·입력·비동기 상태 경계에서 현재 미해결인 구체적 결함을 발견하지 않았다. 이 판정은 컴파일·Unity 실행·사람 IME 입력·게임 전체 QA 통과를 뜻하지 않는다. 네이티브 실행은 root가 담당하며 결과의 정본은 같은 폴더 verification.json이다.

- 검토 시각: 2026-09-11T09:30:11.784Z
- 기준 커밋: 01bec3cb7a7431c4069633f0bf321a02f14ccd79
- delivery-source-manifest.json **attempt 3** SHA256: 6f1da837584b42284137386427a4705b2cb9956d42d61a6b52f9739ab78b2c97
- manifest overlay 13개 파일과 작업공간 바이트를 각각 SHA256으로 대조: **13/13 일치**.
- 기존 세 파일(T0GameSession.cs, WatchInput.cs, T0Interface.cs)은 기준 커밋 대비 diff를 읽었다. 새 저장·App·UI 코드와 두 테스트 파일을 읽었다. 새 .meta는 manifest 바이트 일치 범위다.
- 검증 프로젝트 경로: /tmp/unknown-ai-native-m8-20260911/unity/Unknown. 이 리뷰는 해당 프로젝트를 실행하지 않았다.

## 정적으로 확인한 계약

| 경계 | 코드 근거 | 검토 결과 |
|---|---|---|
| 저장 분리 | Save/ReviewNotesStore.cs 29·32·62행; App/ReviewNotesSession.cs 28행 | saveId 해시가 붙은 review-notes 파일을 슬롯 디렉터리별로 사용한다. 별도 AtomicSaveStore에 명시적인 파일명을 전달하므로 노트 .tmp/.bak도 노트 이름에 붙는다. 노트 경로에서 게임 save.json, save.bak, 체크포인트를 쓰거나 복구하는 호출은 없다. |
| 신선 게임 초안 | App/ReviewNotesSession.cs 44·58·156행; App/T0GameSession.cs 121행 | 정상 게임 save.json에 같은 saveId가 있어야 노트 보관을 허용한다. 없으면 메모는 실행 중 초안이며 보관 불가 설명을 표시한다. 노트는 게임 강제 저장을 호출하지 않는다. 정상 autosave 완료의 dirty flag로 보관 가능 여부를 갱신한다. |
| 문서 검증·실패 | Save/ReviewNotesStore.cs 32·42·50·62·77행 | 체크섬, 버전, kind, saveId, 텍스트형/길이, 출처형/길이/개수/중복을 검사한다. 거절한 기존 파일은 ReadOnly로 보존한다. 검증에서 직접 던지는 InvalidDataException을 Load와 SaveAsync 모두 명시적으로 처리한다. 과대 정수 버전은 정수 cast 대신 문자열 동등성으로 거절한다. |
| 원문은 수동적 데이터 | UI/T0ReviewNotesInterface.cs 33·44행; App/ReviewNotesSession.cs 101행 | 자유 메모는 rich text 해석을 끄고 4,096 UTF-16 code unit 한도로 편집한다. 질문 함수는 메모 본문을 받지 않는다. 자유 문장을 명령·정답·증거·진행 판정으로 실행하는 경로가 없다. |
| 공개된 출처만 | App/ReviewNotesSession.cs 67행 | T0 기록은 실제 읽은 line/row 또는 고정 인용 상태, C1은 관찰 상태에 있는 항목만 투영한다. 저장할 출처도 현재 관찰 출처와의 교집합으로 제한한다. 원본 루트는 별도 구조로 유지하고 같은 루트의 두 자료를 독립 원본 두 개로 표시하지 않는다. |
| 노트 저장과 게임 상태 | App/ReviewNotesSession.cs 156행 | 메모·출처 저장 경로에 Journal.Submit, CommitAsync, 진행 해금, 단서 생성, 판정 변경 호출이 없다. 다른 saveId/디렉터리로 바인딩하면 초안과 링크를 그 저장 영역에 맞춰 다시 읽는다. |
| 비동기 표시 | App/ReviewNotesSession.cs 156행 | 저장 당시 텍스트뿐 아니라 선택 출처 집합까지 같아야 현재 상태를 보관 완료로 표시한다. 다른 store로 전환된 이전 저장 완료는 새 store의 상태를 덮어쓰지 않는다. 편집 중에는 저장 완료나 autosave 가능 여부 갱신 때문에 화면을 재생성하지 않는다. |
| 키보드 입력 분리 | Input/WatchInput.cs 20·24·55행 | 편집 중 Watch action map을 비활성화하고 context를 갱신한다. 종료 후 눌린 Watch 제어가 모두 해제될 때까지 재활성화하지 않는다. IME 조합 중 및 종료 직후 한 프레임의 경계를 노출한다. |
| Esc·Tab·IME | UI/T0ReviewNotesInterface.cs 69·109행 | Esc는 UGUI의 취소/원문 되돌림 경로 대신 초안을 유지하며 편집을 끝낸다. 조합 중 및 grace 구간의 Tab/Esc와 zero-character/Return 후속 이벤트를 게임·노트 조작으로 처리하지 않도록 막는다. |
| 외부 의존성 | 새 App/Save/UI 코드 및 기준 대비 세 파일 diff | 새 AI provider, HTTP 클라이언트, 네트워크 호출, 인증·계정·에너지·과금 의존성이나 새 패키지 변경은 검토된 overlay에 없다. 결과는 결정론적 로컬 노트이며 AI GM 구현으로 해석하지 않는다. |

## 검토 중 수정된 사항의 최종 확인

[OBSERVED] parent가 먼저 보고한 네 항목은 최종 코드에서 다음과 같이 수정된 것을 확인했다.

1. 저장 도중 출처 선택 변경 → 저장 스냅샷의 출처 집합과 현재 집합까지 비교한다.
2. 로드된 비공개 링크가 출처 선택 한도를 점유 → 현재 보이는 선택 출처 개수로 UI 한도를 계산하며 저장 시 관찰 출처로 필터링한다.
3. sourceType/originId 내부 식별자가 사용자 설명에 노출 → 매체명을 한국어로 표시하고 공통 원본 여부를 설명한다.
4. ReviewNotesScreen의 CaseThread 제거가 상위 Render에서 덮임 → 상위 Render 자체에서 reviewNotes일 때 CaseThread를 비운다.

[OBSERVED] 독립 입력 리뷰에서는 설치된 UGUI InputField.cs 2040–2047행의 macOS 조합 종료 후속 이벤트 억제와 새 override의 차이를 보고했다. 최종 ProcessReviewKey는 조합/grace 구간의 zero-character 이벤트와 Return을 막는 경로를 갖는다. 해당 PlayMode 테스트 코드가 Backspace/LeftArrow 후속 이벤트와 Esc/Tab/Return의 경계를 직접 호출해 확인하도록 작성된 점도 읽었다. **이 문서는 그 테스트를 실행했다는 증거가 아니다.**

[OBSERVED] root 실행에서 드러난 InvalidDataException 필터 누락은 최종 Store 42·77행에 명시적으로 추가되었다. source schema 검증 실패가 예외로 빠져나오던 초기 후보는 이 리뷰의 PASS 대상이 아니다.

[OBSERVED] autosave 완료 후 노트 보관 버튼이 갱신되지 않는 경계는 완료 dirty flag와 UpdateReviewNotesAvailability로 보완되었다. 편집/노트 저장 중에는 갱신을 보류하고 이후 처리한다.

## 테스트 코드 범위와 남은 검증

[TARGET] 새 EditMode 코드는 슬롯/정체성별 분리, 손상·미지원·과대버전·중복 출처 거절, rename 전 실패 보존/재시도, 길이 제한, 질문의 명시적 출처 의존성을 검사한다. 새 PlayMode 코드는 신선 게임 초안, 공개 출처 필터, 본문 수동성, 저장/실패/진행 불변성, 문자 렌더링, 키보드 단축키와 held-release, IME grace, 저장 중 출처 변경 등을 다룬다. 이 목록은 **코드에 작성된 검사 범위**이며 성공 횟수는 적지 않는다.

- 실제 macOS 한국어 IME 후보 선택/조합 완료, 포커스 전환, 복사·붙여넣기, 150% 텍스트 크기의 도달성과 가독성은 별도 실행 검증이 필요하다.
- 이 slice는 선택적 메모 도구다. 읽기·작성·저장 성공은 새 증거 또는 정식 인용 고정과 동의어가 아니다.
- M7 이미지/영상/컨셉 출처를 이 QA에 열거나 새 시각 기준으로 사용하지 않았다. 현재 UI 코드 검토는 동작 경계 검토이며 이후 영화 아트의 참조 허용이 아니다.
- 전체 게임 결말·장기 잔존율·재미·성능·상업적 효능·모델 능력은 검증하지 않았다.
- 아래 파일 또는 기준 커밋이 바뀌면 이 PASS는 자동 확장되지 않으며 바뀐 부분을 다시 검토해야 한다.

## 검토 대상 SHA256

| 파일 | SHA256 |
|---|---|
| unity/Unknown/Assets/_Project/App/T0GameSession.cs | 496679b594dc0a656a8e4e3d4127f8bdcf80ff68ef0fded61c0ecf8cbb31da3e |
| unity/Unknown/Assets/_Project/Input/WatchInput.cs | 42c01140ac2e1a260f076b532f47f543d1c28863e2930639d8f62dcc55e5f1c6 |
| unity/Unknown/Assets/_Project/UI/T0Interface.cs | 5aab1fd7466b9fb8ad21164775d41b1c3f97ba36d1d29648c23392bc6ed193c9 |
| unity/Unknown/Assets/_Project/App/ReviewNotesSession.cs | 28214fd29fda75b80155e1c7c44bc499c8d5010e9ee495ac059b51ea7cf1d9e2 |
| unity/Unknown/Assets/_Project/App/ReviewNotesSession.cs.meta | f7408c34fdbdc6ffc37ddd0fbb173fc4815b5c57ec2a7b8e60df1411490a13a8 |
| unity/Unknown/Assets/_Project/Save/ReviewNotesStore.cs | f3efe0ab3165ff88f94a0039ba2aee21dd3520b03c64b96acba2c89c1d7fbdb5 |
| unity/Unknown/Assets/_Project/Save/ReviewNotesStore.cs.meta | f95c81becf3c5dc786278be68b508ba9f769cb18f7562fe609924b56ca2bfb8d |
| unity/Unknown/Assets/_Project/UI/T0ReviewNotesInterface.cs | e7466fb5ed77a3a75ac2ab79296edd1a1694d8c7c7aba23d930c6e20fed72656 |
| unity/Unknown/Assets/_Project/UI/T0ReviewNotesInterface.cs.meta | 8a437ef0769219e3bd4f45a03a3b4dbdcf34e7d88d36b70fa4bb26a027758704 |
| unity/Unknown/Assets/_Project/Tests/EditMode/ReviewNotesTests.cs | 285ef7882f8ea587cd4bb1b93725066959f78fbe2f7298a6b26b5a4345431546 |
| unity/Unknown/Assets/_Project/Tests/EditMode/ReviewNotesTests.cs.meta | 3b5f6e20630f60eea31016ff3fc459d99ea0c7602951cb8e79ca9cfb80d76f80 |
| unity/Unknown/Assets/_Project/Tests/PlayMode/ReviewNotesPlayModeTests.cs | 203e2552e7e5010e94406a9fae9e5b7dfd2c61b30fc45aea447b2a8a0a960f44 |
| unity/Unknown/Assets/_Project/Tests/PlayMode/ReviewNotesPlayModeTests.cs.meta | f653a5f79662cbd5704eb7fe0d72254999374642bd95fbcfc50f532e84f9d327 |

## Attempt 4 정적 리뷰 보완 — 2026-09-11T09:38:50.295Z

[OBSERVED] 앞의 attempt 3 검토와 해시표는 역사 근거로 보존한다. 최종 정적 검토 대상은 delivery-source-manifest.json **attempt 4**이며 SHA256은 **f3f432b3dbb1b4faf0b64c637c895b3e82a4f3c9b0e04f2c45ecd939e1484159**다. overlay **13/13** 작업공간 해시가 일치한다. 기준 커밋은 01bec3cb7a7431c4069633f0bf321a02f14ccd79로 동일하다.

[OBSERVED] attempt 3 대비 변경은 **Tests/PlayMode/ReviewNotesPlayModeTests.cs 한 파일**뿐이다. 새 SHA256: **2641805e4e82a8d3033a2a1c93129586caa429b71aa92890cc13fb9408d24d06**. 나머지 12개 파일 및 production App/Save/UI/Input 코드는 앞의 리뷰 대상과 바이트가 같다. 따라서 production 정적 경계 판정은 유지하고, 아래 테스트 하니스 변경을 추가 검토했다.

- **fixture 디렉터리 검증**(Create 50–57행): 전달한 고유 디렉터리와 실제 game.Store.DirectoryPath의 정규화 경로를 비교하고 불일치하면 즉시 실패한다. parent가 발견한 실행 옵션 --t0-save-dir의 우선 적용으로 여러 테스트가 같은 저장 폴더를 공유하던 오염을 감추지 않는다. parent는 후속 run에서 그 옵션을 제거한다고 보고했다. 이 문서는 그 실행 명령이나 실행 성공을 독립 증명하지 않는다.
- **유한 대기와 예외 가시성**(88–105행): 일반 Wait는 Stopwatch 기준 15초 후 종료 여부를 단언하고 fault/cancel을 전파한다. WaitForGate는 주입 gate와 실제 연산 중 먼저 끝난 대상을 기다리고 연산 fault 또는 gate 도달 전 종료를 구분한다. gate가 열리지 않는 실패를 무기한 대기로 바꾸지 않는다.
- **주입 해제**(185–207·226–245행): fault 주입을 사용하는 테스트는 finally에서 release gate를 열고 injection을 해제한다. 중간 단언 실패가 다음 테스트에 막힌 writer를 남기는 경계를 보완한다.
- **정리 경계**(61–86행): teardown이 모든 추적 gate를 열고 추적 task와 게임 flush를 제한 시간 안에 기다린다. finally에서 host·입력 장치를 정리하고 전역 InputSystem 설정을 복원한다. task가 완료되지 않은 경우에는 디렉터리를 지우지 않고 경로를 기록한다.
- **진단성**: 테스트 이름, 단계, task 상태, fixture 경로를 로그에 남겨 단언 실패와 실행 환경 오염을 구분할 수 있다. 실제 결과는 root의 verification.json 및 테스트 원본 출력으로 판단한다.

[OBSERVED] 동일 production 코드에 대해 사용자 노트 보존·숨은 source 출력·IME 경로를 한 번 더 읽었다. 초안은 onValueChanged로 별도 메모 필드에만 남고, Esc 경로는 UGUI 취소 복원을 호출하지 않는다. 저장 실패 또는 저장 도중 텍스트/출처 변경은 초안을 보관 완료로 오표시하지 않으며, 공개 전 source ID를 복구한 경우에도 화면·저장 투영은 관찰 출처 교집합으로 제한된다. 편집 시 조합/grace 상태와 Watch action map 중단, 종료 후 키 해제 대기 구조도 동일하다. 이 재검토에서 새로 확정한 production 결함은 없다.

[INFERENCE] **Attempt 4 STATIC-REVIEW-PASS 유지 — 정적 검토 범위만.** 하니스 오염 상태의 이전 실행은 이 판정으로 유효화되지 않는다. 새 fixture 경로에서 다시 실행한 EditMode/PlayMode·네이티브 결과가 별도로 필요하다. 합성 IME 이벤트를 검사하는 테스트 코드는 실제 macOS 한국어 후보 창·조합 완료·붙여넣기·포커스 이동의 사람 검증을 대신하지 않는다.

## Attempt 5 정적 수정 리뷰 — macOS Tab 후속 문자 — 2026-09-11T09:55:19.355Z

[OBSERVED] **최신 검토 바인딩은 attempt 5**다. 이전 attempt 3·4 검토와 해시표는 당시 정적 검토의 역사 자료로 보존한다. delivery-source-manifest.json SHA256: **bf726e0efef74ba8d3c25e3a4e8103145a8bba610743ac8d5353bfe21eed7151**, 기준 커밋: 01bec3cb7a7431c4069633f0bf321a02f14ccd79. overlay **13/13** 작업공간 해시를 다시 대조했고 모두 일치했다.

| attempt 4 이후 변경 파일 | 최종 SHA256 |
|---|---|
| unity/Unknown/Assets/_Project/UI/T0ReviewNotesInterface.cs | b4c9e8830b6a5955f4a7aafff2fa2f0cf881e5ef1b07783230ac57b0538f73e0 |
| unity/Unknown/Assets/_Project/Tests/PlayMode/ReviewNotesPlayModeTests.cs | a4ef2268a4abb84dd9e9c0b33b84dc608a258a9199918936b48987a7b4872ccc |

[OBSERVED] 나머지 11개 manifest 파일은 attempt 4와 동일하다. production 수정은 노트 입력 필드의 경계 처리뿐이며, 저장소·App·WatchInput은 바뀌지 않았다.

### 실제 반례와 이전 판정의 한계

[OBSERVED] 검토자는 native-tab-before-note.json에서 저장된 text의 첫 code point가 **9(U+0009)**인 것을 직접 확인했다. 파일 SHA256은 **afb199e8cfb1d8e0ff22772904db49eabb8e82a1a47cecd7cff22c3610bec726**이다. 한국어 메모 붙여넣기 후 Tab 종료·보관에서 선행 Tab이 생겼다는 parent의 macOS 관측을 뒷받침한다. 게임 save bytes 불변은 parent의 실행 검증 범위이며 이 파일 하나만으로 다시 증명하지 않는다.

[OBSERVED] 검토자는 root가 실행한 native-tab-red.xml도 읽었다. 기존 UI와 신규 테스트 조합은 **1개 중 1개 실패**, 해당 NativeTabCharacterAndPostExitEventsCannotModifyPastedNoteOrSavedText 실패 메시지는 Expected: False / But was: True였다. 이는 XML 아티팩트 열람이며 검토자가 Unity를 실행했다는 뜻이 아니다. 이전 정적 PASS가 native Tab 이벤트 배치의 안전을 입증하지 못했음을 명시한다. 이 반례는 기존 판정으로 무효화하지 않는다.

### 최소 수정과 새 테스트 검토

[OBSERVED] ProcessReviewKey는 이제 **각 이벤트마다 isFocused를 확인**한다(UI 109–112행). 첫 KeyCode.Tab 이벤트가 편집을 끝내고 caret을 초기화한 뒤, 같은 GUI 이벤트 루프에서 도착하는 후속 문자 이벤트는 KeyPressed에 전달되지 않는다. 따라서 이전 OnUpdateSelected 진입 시점의 isFocused 검사 하나에 의존하던 간격을 닫는다.

[OBSERVED] 종료 키 분기는 KeyCode.Tab뿐 아니라 **character가 Tab인 char-only 이벤트**도 소비한다(UI 114–117행). 조합/grace 중에는 종료하지 않고 소비하며, 정상 편집 상태에서는 초안을 유지한 채 종료한다. 그 뒤의 Tab 문자나 일반 문자 이벤트도 초점을 잃은 필드에 추가되지 않는다. 이는 키 이벤트 처리 수정이며, 붙여넣기 본문 전체를 재작성하거나 사용자 문자열에서 문자를 일괄 삭제하는 처리는 추가하지 않았다.

[OBSERVED] 신규 테스트(PlayMode 226–253행)는 문자열 직접 대입만으로 대체하지 않고 기존 공개 API와 GUIUtility.systemCopyBuffer, 플랫폼별 Cmd+V/Ctrl+V 키 경로로 한국어와 문자 그대로의 markup을 붙여넣는다. 이어서 **char-only Tab**과 **KeyCode.Tab + 후속 char-only Tab/일반 문자** 두 경로를 각각 검사하고, 편집 상태 종료·필드 본문·App 초안·실제 보관 문서 text의 동등성을 단언한다. clipboard는 finally에서 원래 값으로 복원한다. 실제 운영체제 이벤트를 자동화한 것이 아니라 native 반례의 이벤트 형태를 입력 계층에 재현한 회귀 코드라는 한계는 유지한다.

[INFERENCE] **Attempt 5 STATIC-FIX-REVIEW-PASS.** 보고된 Tab 후속 문자 결함의 원인에 대응하는 최소 수정이며 검토된 경로에서 추가 구체적 결함은 발견하지 않았다. 이 판정은 수정 후보의 정적 수용이다. 수정 후 GREEN 및 전체 회귀 실행은 당시 root가 진행 중이며, 완료 여부·원본 XML·재현 저장 파일·실제 macOS 재확인 결과는 verification.json과 연결 증거에서 별도로 확인해야 한다. 현재 파일은 그 결과를 선제적으로 PASS 처리하지 않는다.
