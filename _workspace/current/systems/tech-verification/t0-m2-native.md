---
updated: 2026-09-10
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
---

# T0 M2 — Unity native 구현·저장·입력 검증

[OBSERVED] Unity 6000.5.6f1에서 T0의 실제 UI와 저장 흐름을 구현했다. EditMode 18/18, 실제 플레이어 UI 결함 수정 후 PlayMode 15/15 NUnit testcases가 PASS했다. M1 내부 규칙 검사 21개는 이 중 EditMode wrapper 1건에 포함된다. 이를 39건으로 더하지 않는다. 실제 macOS player의 한글/포인터/앱 재실행 관측은 director의 별도 smoke receipt와 함께 읽는다. G4/G5/G6, 플레이시간, 성능 측정은 이 보고서로 PASS하지 않는다.

## 구현 계약

- boot / ui-root / hub 3개 build scene, Both input, URP. Input System 1.20.0, Localization 1.5.13, URP 17.5.0, Test Framework 1.7.0, uGUI 2.5.0. 기존 전이 Newtonsoft 3.2.2를 사용했다.
- Sim은 불변 PuzzleState를 받고 snapshot을 출력한다. UI/연출은 이를 읽는다. UI에서 제안 → preview → 확정 → SavePending → checkpoint/temp/fsync/atomic replace → 현재 성공 receipt → publish 순서를 유지한다.
- 회로는 OpenTool/Tracing/BeginOverlay/Overlaying/Anchor/Marking/derived Resolved/Cancel/Close를 구현했다. 정본 overlay 해답 (-1,+1), gridStep 1, fineGridStep null을 보존한다. b2는 야외 구역당 매체 1개, b3는 독립 인용 매체 2개다.
- Reader는 원본/보존사본, NaN gap, normal/flat/missing, 실제 sample/phase picker, 명시적인 SetWindow, pinned window, independent citation을 연결한다. 단순 LoadRecord는 인용 창을 자동 확정하지 않는다.
- 생성기는 synopsis/t0-records.md §12의 승인된 출처 배정과 planning/t0-circuit-overlay.json을 읽어 systems/data/t0 및 Unity Data/Tables를 함께 생성한다. plate=system-hub/station-bureau-standard, ledger=null/station-bureau-standard. 다른 세 record 배정은 바꾸지 않는다.
- 9 Watch 액션의 keyboard/gamepad routing, release latch, LB press-time chord, rebinding binding-name identity를 구현했다. Reader X/Space는 사본 Read, Y/Delete는 인용 요청, Q/RS는 없음. Circuit X는 현재 구역 표시, Y 없음, Q/RS는 coverage 조회. shell Y는 tool wheel이다.
- 키보드 Tab/Enter 또는 gamepad left-stick/A만으로 T0 3 beat를 완료하는 독립 PlayMode 경로가 있다. InputSystemUIInputModule의 UI keyboard/gamepad submit을 꺼 Watch와 이중 실행되지 않게 한다. 모디파이어 Ctrl/LB 자체의 임의 재바인딩은 제공하지 않는다.
- 언어/100–150% 글자크기/reduced motion/확정방식/rebind/hold 시간 설정을 제공한다. 긴 문서 라벨은 높이에 맞춰 늘어나며 nav/content 모두 스크롤된다. 마지막 keyboard/mouse/gamepad 입력에 따라 footer 안내를 갱신한다. opt-in hold는 .2–1.5초, 기본 .4초와 진행 링을 사용한다.
- 영어 UI에서도 정본 한국어 내러티브를 fallback으로 표시한다. OS 동적 한국어 font를 사용하므로 현재 지원 증거 범위는 macOS이다.

## 저장과 복구

- v1 root 필드를 변경하지 않았다. 승인 snapshot은 {seq,stateHash,stateBlob:{facts,values,readCounts}}이며 데이터 schema save.md §3.3에 기록했다.
- checkpoint/snapshot 간격 200, 최대 log 20,000 entries, byte cap 6,291,456을 SavePolicy.json에서 읽는다. ancestor snapshot, branch-aware undo/redo, folded history와 coarse checkpoint undo를 처리한다.
- recursively canonical sorted JSON/SHA-256 checksum, payload hash와 ordered log chain, backup/checkpoint fallback, cancellation before rename, before-write/during-write/before-rename/after-rename failure injection과 idempotent retry를 구현했다.
- ISO date의 자동 DateTime 변환을 금지해 roundtrip checksum을 안정화했다. schemaVersion이 누락/null/negative/fractional/string/future면 유효 backup이 있어도 primary를 덮거나 자동 downgrade하지 않는다. v0 backup migration은 별도 검증했다.
- semantic replay validator가 checksum-valid invalid state도 거부한다. read-only recovery의 validated retry는 원 saveId/createdUtc를 보존하고 refusal을 해제한다. 새 recovery child slot은 다음 GameSession recreation에서도 목록에 나타나며 원 primary bytes를 보존한다.
- settings.json은 진행 save와 별도다. 선택 holdSeconds 누락 시 기존 .4초 값으로 호환된다. 범위는 외부 HoldOptions.json에 있고 옵션 저장으로 진행 상태를 재계산하지 않는다.

## 리소스와 연출

- r03의 FBX importer axis rotation에 yaw180을 곱해 보존한다. 승인된 position (0,.32,1.1)과 diagnostic AABB subtraction을 builder와 실제 hub.scene에 반영했다. 원본 mesh, physics, 애니메이션은 수정하지 않았다.
- native Metal diagnostic: 2 meshes/156 triangles, 4×1024 maps, base color sRGB / roughness linear, 두 material map binding 및 shaderSupported 확인. shader는 smoothness=1-roughness, authored metallic .08을 사용한다. 당시 원본 audit의 runtimeEligible:false는 보존한다.
- T0 한정 승인: production/decision-log.md 'RFC-CX-003 addendum — r03 T0 runtime scene approval (2026-09-10)' 및 source provenance runtimeApproval. 증거는 t0-m2/diagnostics-r03-approved/의 standalone/solid-interference/front-adaptation PNG와 import-material-fit-audit.json이다.
- 승인 VFX canonical packet을 Resources/T0Vfx.json으로 복사했다. 현재 성공 receipt만 300ms envelope / +150ms ink marker의 절차적 두 획 HUD 표시를 낸다. failure/stale/duplicate/recovery/replay는 표시하지 않으며 context exit에서 해제한다. reduced motion은 transient 0이고 일반 UI 성공 상태를 유지한다.
- PlayMode timing 검사는 실제 MonoBehaviour.Update/Time.deltaTime에 25ms capture step을 적용해 marker와 종료를 단언했다. 이는 프레임 비용/실시간 기기 성능 측정이 아니다. audio는 승인되지 않아 재생하지 않는다.

## 재현 명령과 결과

Unity 명령은 저장소 root에서 rtk proxy를 통해 실행했다. 아래 경로는 모두 절대 projectPath를 사용한다.

| 검사 | 명령 핵심 | 최신 receipt | 관측 |
|---|---|---|---|
| Scene/settings | -batchmode -nographics -quit -executeMethod Tide.EditorTools.T0ProjectBuilder.Prepare | t0-m2/logs/prepare-3.log | exit 0, 승인 r03 hub 저장 |
| PlayMode | -batchmode -nographics -runTests -testPlatform PlayMode -assemblyNames Tide.Tests.Play | t0-m2/results/playmode-11.xml, logs/playmode-11.log | 15/15 PASS, exit 0 |
| EditMode | -batchmode -nographics -runTests -testPlatform EditMode -assemblyNames Tide.Tests.Sim | t0-m2/results/editmode-7.xml, logs/editmode-7.log | 18/18 PASS, exit 0 |
| Native diagnostic | -batchmode -quit -executeMethod Tide.EditorTools.T0ResourceDiagnostics.ImportAndCapture | t0-m2/logs/r03-diagnostics-3.log | Metal capture, exit 0 |
| macOS Development build (UI 수정) | -batchmode -nographics -quit -executeMethod Tide.EditorTools.T0ProjectBuilder.BuildMacFramingFix | t0-m2/logs/build-framing.log | Succeeded, 314105987 bytes, exit 0 |

공통 executable: /Applications/Unity/Hub/Editor/6000.5.6f1/Unity.app/Contents/MacOS/Unity

공통 argument: -projectPath /Users/jangyoung/orca/unknown/unity/Unknown

각 실행의 -testResults/-logFile은 위 영구 보존본으로 복사하기 전 /tmp/unknown-t0-m2-<suite>-<run>.<xml|log>였다. 원문 command line은 각 log의 COMMAND LINE ARGUMENTS에 포함된다.

실패 기록도 삭제하지 않았다. PlayMode4의 keyboard/manual InputSystem.Update 및 binding identity 오류는 5에서 해결했다. PlayMode6은 새 hold ring의 CanvasRenderer 누락 1건(12/13)이었고 7에서 해결했다. EditMode4의 ISO date checksum과 EditMode5의 no-window stale fixture 실패는 6/7에서 해결했다. 최초 async test main-thread 대기 실패/샘플도 logs에 보존했다.

## 플레이어 검증 경로와 한계

- app: unity/Unknown/Builds/T0-mac-framing/Unknown.app. 새 슬롯 격리 인자: --t0-save-dir /tmp/unknown-t0-player-smoke-<unique>.
- Start → handover 3 clauses → transfer first3 rows + written/blank decision → plate#0. Circuit node → open/trace → begin overlay → left1/up1 → anchor → outdoor3 각각 규정 필사본 1개 부착. Reader node → standard plate → Read → start H-1:00/end H+3:00 → Cite/Confirm → ledger 같은 창 → Cite/Confirm → T0 완료.
- full keyboard/gamepad tests는 실제 InputSystem state-event injection과 frame delivery이다. 실제 OS 키보드/패드 장치, 포인터 hit testing, 한글 렌더, quit→relaunch는 자동 테스트가 증명하지 않는다. 일부 UI adapter 회귀는 Interface.Activate와 직접 SetWindow를 함께 사용하며 pointer injection으로 부르지 않는다.
- recovery restart 회귀는 GameObject/GameSession recreation이며 OS process restart가 아니다.
- camera는 authored position/target를 사용해 LookAt한다. 별도 pitch field의 직접 적용이나 최종 구성의 미학적 수용은 주장하지 않는다. Gamma/native 캡처 관측이며 색 공간/가독성/성능 최종 G4/G5는 미측정이다.

## 그래프와 메모리 연계

[OBSERVED] 이 lane은 director의 단독 통합 소유에 따라 graphify/mex를 병렬 실행하지 않는다. 최종 source freeze 후 director가 graphify update, mex graph/check/log, wiki와 freshness receipt를 통합한다. 해당 receipt 전 이 문서만으로 G8 PASS를 주장하지 않는다. 패키지/Unity cache로 인한 과거 graph ENOSPC는 기존 그래프를 보존한 상태에서 director가 처리 중이다.

## 실제 플레이어 UI 결함 수정 (2026-09-10 후속)

- [OBSERVED:director] 초기 플레이어의 CUA 마우스 경로로 docs/plate0/circuit3표시+근거/plate+ledger 독립 인용을 완료하고 T0완료를 확인했다. 150% 설정과 reduced-motion 설정, OS 앱 종료 후 동일 슬롯 재시작에서 완료/설정 복원이 관측됐다. 정확한 증거 범위와 최종 수정 앱 smoke는 `systems/tech-verification/t0-m2-player-smoke.md`에 director가 기록한다.
- [OBSERVED] 그 과정에서 서랍이 오른쪽 UI 뒤에 가려지고, 스크롤 파형이 상단 header 위에 그려지는 두 시각 결함을 받았다. 원인은 full-screen scene camera와 UI 비가림 영역 불일치, 그리고 커스텀 SignalChart/AnchorDiagram이 MaskableGraphic이 아닌 Graphic을 상속한 점이다.
- [OBSERVED] UI의 실제 RectTransform에서 SceneViewport를 계산해 camera.rect에 연결했다. generated camera position/target 및 horizontal FOV를 유지하며 viewport aspect만 반영한다. 파형/앵커는 RectMask2D가 clip/cull할 수 있도록 MaskableGraphic과 CanvasRenderer를 사용한다. Sim/save/data 변경은 없다.
- [OBSERVED] PlayMode11 15/15 PASS: 기존13 + 서랍 center의 viewport 내부 투영/UI 비중첩/정본 화각 + 파형 실제 renderer clip 활성 및 완전 이탈 시 cull. 마지막 이탈 검사는 unrestricted fixture scroll을 사용하며 CanvasScaler 좌표 변환을 적용한다. PlayMode9/10은 이 fixture 가정/좌표 보정 전 실패로 보존했고 production 회귀로 주장하지 않는다.
- [OBSERVED] native Metal 1280×800 전후 캡처 `t0-m2/viewport-diagnostics/`: before에는 UI 뒤에 서랍이 가려지고 after에는 왼쪽 위 영역에서 서랍 전면과 손잡이가 보인다. 같은 T0Interface 레이아웃과 diagnostic labels를 사용한 두 RT native composite이며 실제 standalone 화면 검증은 별도다. 최초 단일 RT 시도는 UI camera가 scene을 clear하여 비교로 무효였고 `viewport-diagnostic-attempt1/`에 남겼다.
- [OBSERVED] 수정 빌드 `Builds/T0-mac-framing/Unknown.app` 성공(exit0, 314105987 bytes). 초기 앱(314104679 bytes)/baseline source manifest를 보존해 smoke 수행 중 실행물을 덮지 않았다. 최종 source manifest는 baseline 복사본을 별도로 보존한 뒤 갱신했다.
- [OBSERVED] 빌드 성공 뒤 영구 로그 복사 중 ENOSPC가 발생했다. 이 lane이 만든 재생성 가능한 `unity/Unknown/Library/Bee` 캐시만 제거했다(사전249960KiB, rm exit0). source/app/save/raw evidence를 보존하고 로그 복사를 재시도했다. 이 조치는 게임 산출물을 삭제하지 않는다.

[OBSERVED:director 최종 수정 앱] 같은 smoke save-dir로 수정 앱을 실행해 T0완료/150% 설정을 복원했다. 서랍 node의 왼쪽 위 영역에서 전면·손잡이가 보였고, ledger의 H-1:00→H+3:00 창이 복원됐다. Reader를 끝까지 스크롤해도 파형이 work panel/header 밖에 그려지지 않음을 CUA로 확인했다. 영구 근거: `systems/tech-verification/t0-m2-player-smoke.md`.
