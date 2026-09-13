---
updated: 2026-09-13
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
---

# M19 — T0 액션 판(action plate) · 텍스트 위계: 전역 표현 슬라이스

> **이 문서가 무엇인가**: T0Interface의 **버튼 한 종류와 텍스트 역할 3종**의 표현을 TDD로 바꾸고,
> Unity 자동 테스트 · 플레이어 빌드 · 실제 네이티브 창 캡처로 검증한 영수증이다.
> 최종/출시 아트가 **아니고**, 접근성 인증이 **아니며**, 사람 플레이테스트 · 성능 측정 주장도
> **아니다**. 어떤 게이트도 올리지 않는다.

같은 사이클 안의 제자리 갱신 (RFC-Q2: `cycle` 불변 → `supersedes: null`, 아카이브 의무 없음).

연출 계약(소유 산출물): `_workspace/current/presentation/t0-action-plate-m19.md` (`status: draft`)

사용자 요청 원문: "버튼유아이랑 텍스트 유아이 게임스럽게 개선" (2026-09-13).

## 0. 결론

[OBSERVED] 모든 T0 액션 버튼이 **평평한 단색 사각형**에서 **황동 눈금과 모서리 선을 가진 계기 판**이
되었고, **포커스된 판은 판 자체가 황동으로 반전**되어(라벨·눈금은 잉크색으로 뒤집힘) 어두운 배경에
묻히지 않는다. 텍스트는 **본문 20 / 액션 라벨 18 Bold / 보조 설명 16 약화색** 3단으로 갈렸다.
플레이어 문구 · 액션 ID · 순서 · 활성 동작 · 입력 경로 · 시뮬레이션 상태 · 저장 필드 ·
공개 표면 · 생성 테이블은 **모두 불변**이다.

| 단계 | 결과 | 영수증 |
|---|---|---|
| 집중 RED | 1 case, 0 passed, **1 failed** (장식 부재 단언) | `red.xml`, `red.log` |
| 집중 GREEN | 1 case, **1 passed**, 0 failed | `green.xml`, `green.log` |
| EditMode 전체 | 54 total, **54 passed**, 0 failed — case 집합 M18과 **동일** | `editmode.xml`, `editmode.log` |
| PlayMode 전체 | 87 total, **86 passed**, 0 failed, 1 skipped(기존) — **+1 = 이번 테스트뿐** | `playmode.xml`, `playmode.log` |
| macOS 플레이어 빌드 | `T0_MAC_BUILD Succeeded bytes=403840809`, 디스크 실측 일치 | `build-mac.log:6036` |
| 네이티브 캡처 | **pid 92370 / window 5296 명시 지정** 단일 창 캡처 1장 | `shots/00-m19-title-pid92370-window5296.png` |
| Player.log | 예외 · 에러 **0건** | `logs/player-m19-pid92370.log` |
| M14~M18 보존 | 98/98 베이스라인 파일 **바이트 동일**, HEAD 불변 | §7 |

## 1. 변경 (정확한 경로)

| 경로 | 변경 | sha256 |
|---|---|---|
| `unity/Unknown/Assets/_Project/UI/T0Interface.cs` | **+31 / −7** — `Button()` 판 장식 · 포커스 반전 · 라벨 18 Bold, 보조 설명 16 약화색, `Ornament()`/`Muted()` 헬퍼, `FocusSelection`에 `IDeselectHandler` | `f4468449c6f76f0196c1de7a6e3cecf285621a11df32f07ae4f32751ba3c414b` |
| `unity/Unknown/Assets/_Project/Tests/PlayMode/T0ActionPlatePlayModeTests.cs` | **신규** — 집중 PlayMode 테스트 1 case (11,372 bytes) | `42f6e211539874f227489536b48e1d8c1432a8ea20bb5d19ddc1dc5ea7f121d0` |
| `…/T0ActionPlatePlayModeTests.cs.meta` | 신규, Unity가 임포트 시 생성 | — |
| `_workspace/current/presentation/t0-action-plate-m19.md` | 신규 소유 연출 계약 (`status: draft`) | `a1ab801e851e070702a94fbb5c9e698602d2902a18398a75aad6ee23365377a1` |

`T0Interface.cs` HEAD 원본 sha256 = `8f453fdfd68975084149fa32415b7134560fa2bbf717d4758ae6c0e4be03363d`
(이 파일은 M14~M18이 건드리지 않아 HEAD와 동일한 상태였다 — 즉 M19의 유일한 코드 수정 지점이다).

**수정 지점은 4곳뿐이다.** 화면별 분기·예외를 만들지 않았으므로 타이틀 · 허브 · 판독기 · 회로 ·
서명지 · 오버레이 · 툴바 · 내비게이션의 **모든 버튼이 같은 규칙**을 받는다 (한 개의 전역 슬라이스).

### 1.1 정확히 무엇이 달라졌는가

| 요소 | 이전 [OBSERVED] | 이후 [OBSERVED] |
|---|---|---|
| 판 배경 | `Panel(id,…,ink)` — 테두리·눈금 **0개**의 단색 사각형 | 동일 `ink` 판 + 비-raycast `Bevel`(상단 황동 하이라이트 α.32) + 비-raycast `Rule`(좌측 황동 눈금) |
| 비활성 표식 | 없음 (판만 흐려짐) | `Rule.SetActive(false)` — "누를 수 있음" 표식이 **물리적으로 사라진다** |
| 선택 색 | `selectedColor = brass` → `ink` 판에 **곱셈** → `(.073,.101,.055)` = **더 어두워짐** | `selectedColor = Color.white` + `FocusSelection`이 판 색을 **반전**(`plate=brass`, `label=ink`, `rule=ink`) |
| 선택 전환 | `fadeDuration` 기본 0.1 | `fadeDuration = 0` (상태 즉시 판독) |
| 액션 라벨 | `17`, Normal, anchor x .04 | `18`, **Bold**, anchor x .075 (눈금 자리 확보) |
| 보조 설명(`Detail`) | `17`, 본문과 **같은 색·같은 크기** | `16`, `Muted(fore,back)` = 배경 쪽 0.32 보간 |
| 본문 / 제목 / 부제 / 상태 / 푸터 / 사건카드 / 차트라벨 | 20 / 28 / 15 / 16 / 13 / 20 / 15 | **전부 불변** |

**핵심 근거 [OBSERVED]**: Unity `Selectable`의 색 전환은 대상 그래픽 색에 **곱셈**으로 적용되고
정점 색은 `Color32`로 클램프된다. 따라서 어두운 `ink` 판에서는 어떤 `selectedColor`도 판을
**밝게 만들 수 없다**. 기존 `brass` 선택색은 실제로 판을 더 어둡게 했다. 포커스 가독성은
곱셈 틴트로 해결이 **불가능**하므로 판 색 자체를 반전시켰다.

### 1.2 불변식 유지 [OBSERVED]

플레이어 문구 0건 변경(`T0Strings.json` 무변경) · 액션 ID/순서 0건 · `button.interactable=action.Enabled`
그대로 · 입력 경로(`Navigate`/`Focus`/`Activate`/`BeginActivation`/`EndActivation`/`HoldButton`) 0행 변경 ·
`PuzzleCommand` 제출 0건 · 저널 쓰기 0건 · 저장 필드 0건(§9 마이그레이션 불변식 무관) ·
공개 표면(사건카드 가드·힌트 단계·기록 표시명·증거 규칙) 0건 · 데이터 테이블(`Data/Tables/*`,
`systems/data/t0/*`) 0건 · `M7UiSkinProfile` 필드/기본값/`runtimeApproved` 0건 ·
`runtimeEligible`/`runtimeApproved` 승격 0건 · 2D/3D/이미지/영상 생성·구매 0건 ·
공유 진실 산출물 편집 0건 · 신규 위젯/프리팹/스프라이트/셰이더/머티리얼 0건.

### 1.3 RFC 판정

RFC 블록을 쓰지 **않았다**. `references/dependency-matrix.md`의 presentation 행은
`syn ● / vfx ● / anim ● / mot ● / mod ● / qa ●`를 요구하지만, RFC는 디렉터 소유 공유 진실 파일
`production/decision-log.md`에 기록되며 이번 작업 지시는 공유 진실 문서 편집을 금지했다.
CLAUDE.md §11은 가상의 ack 생성을 금지한다. 따라서 matrix가 스스로 정한 처리("required acks가
없으면 `status: draft`, 게이트 투입 불가")를 적용해 연출 계약을 **`status: draft`** 로 두고
미해결 ● ack를 그 문서 §0에 명시했다. 이 영수증은 관측 사실 기록이므로 M17/M18 선례대로
`status: current`이며, 게이트를 올리지 않는다.

## 2. TDD 증거

러너: `Unity 6000.5.6f1 -batchmode -nographics -runTests`,
필터 `Tide.Tests.T0ActionPlatePlayModeTests`.

### 2.1 왜 완료된 C1 순찰 화면인가

**실제 도달 가능한 화면 중 유일하게 텍스트 3역할이 한 번에 렌더된다** — 본문(`GameScreen.Body`),
액션 라벨(`ViewAction.Label`), 보조 설명(`ViewAction.Detail`). `Detail`을 쓰는 화면은 저장소 전체에서
`c1-interview-prep`(완료된 C1)과 순찰 진행 중 `c1-condition`/`c1-confirm`뿐이다.
상태는 조작하지 않았다: 기존 C1/M5/M8/M18 스위트가 쓰는 실제 완료 저장 픽스처
`_Project/Tests/Fixtures/C1PatrolCompletedV2.json`을 복사해 부팅했다.
**목(mock) 0건 · 수기 `GameScreen` 0건 · 소스텍스트 단언 0건** — 모든 단언은 실제 Unity 컴포넌트
상태(`Image.color`, `Graphic.raycastTarget`, `Text.fontSize`/`fontStyle`, `Selectable.colors`,
`GameObject.activeSelf`, `RectTransform.rect`)를 읽는다.

### 2.2 RED은 설정 오류가 아니라 부재 단언에서 실패했다

```
continue-c1-signature must carry a 'Bevel' plate ornament so the action reads as an
instrument plate, not a flat dark rectangle
  Expected: not null
  But was:  null
  at T0ActionPlatePlayModeTests.Ornament (…:85)
  at T0ActionPlatePlayModeTests.AssertPlateAffordance (…:100)
```

[OBSERVED] 이 단언 **앞의 모든 단언이 통과**했다는 점이 harness가 실제 완료된 C1 화면에 도달하고
기존 계약이 이미 성립함을 증명한다 — `PatrolActive` 참, `PatrolComplete` 참, `SignatureActive` 거짓,
`Surface=="shell"`, 액션 ID 순서 `["continue-c1-signature","c1-review","c1-interview-prep"]` 일치,
3개 전부 `interactable`, 라벨 3개 문자열 일치(1개는 런타임 strings 테이블에서 파생), 3개 전부
포커스 가능 + `CurrentFocusId` 일치, 루트 `Image.raycastTarget` 참, 루트에 `LayoutGroup` 부재.
즉 "도달하지 못해서"가 아니라 **판 장식이 없어서** 실패했다.

### 2.3 테스트가 단언하는 플레이어 관측 사실

| 항목 | 단언 |
|---|---|
| 행동 가능성 (P1) | **모든** 라이브 버튼에 `Bevel`·`Rule` 자식 존재, 둘 다 `raycastTarget==false`, 루트 `Image.raycastTarget==true`, 루트 `LayoutGroup` 부재, `Label` 정확히 1개 |
| 비활성 구분 (P3) | `Rule.activeSelf == button.interactable` — 전 버튼에 대한 **보편 법칙**으로 단언 |
| 포커스 반전 (P2) | 포커스 판: `Lum(label) < Lum(plate)`(밝은 판·어두운 글자). 비포커스 판: `Lum(label) > Lum(plate)`. 포커스 판과 비포커스 판의 배경 휘도 차 **> 0.12**. `selectedColor==Color.white`, `fadeDuration==0`. 포커스를 옮기면 반전도 따라 이동(2개 버튼으로 교차 검증) |
| 텍스트 위계 (P4) | `본문.fontSize > 라벨.fontSize > 보조.fontSize`, 라벨만 `FontStyle.Bold`, 보조·본문은 `Normal`, `보조.color != 본문.color`, `Lum(보조) > Lum(본문)` |
| 무변이 | `SavePending` 거짓, 상태 해시 · 저널 head · 성공 영수증 수 불변 |
| 배율 1.5 리플로 (N11) | 5단계 후 `TextScale==1.5`, `Surface=="shell"` 복귀, 액션 ID 순서 동일, **라벨 문자열 딕셔너리 동일**, P1·P2 재성립, 위계 순서 유지, 라벨이 실제로 커짐, 모든 `WrappedButtonHeight`가 wrapped 라벨을 담음(잘림 0) |

### 2.4 실측 수치 (산술)

| 항목 | 커밋 리터럴 | M7 스킨 |
|---|---|---|
| 판 휘도 차 (포커스 − 비포커스) | **0.2810** | **0.4496** |
| 테스트 임계값 | 0.12 | 0.12 |
| 폰트 크기 @배율 1.0 | 본문 20 / 라벨 18 Bold / 보조 16 | 동일 |
| 폰트 크기 @배율 1.5 | 본문 30 / 라벨 27 Bold / 보조 24 | 동일 |
| (이전) 라벨 / 보조 | **17 / 17 — 구분 없음** | 동일 |

## 3. 전체 회귀

| 스위트 | 이번 실행 | M18 베이스라인 | 델타 |
|---|---|---|---|
| EditMode | 54 total, 54 passed, 0 failed, 0 skipped | 54 / 54 | **0** — case 집합 동일 |
| PlayMode | 87 total, 86 passed, 0 failed, 1 skipped | 86 total, 85 passed, 0 failed, 1 skipped | **+1** = 이번 테스트 |

[OBSERVED] M18의 `editmode.xml`·`playmode.xml`과 leaf case 집합을 기계 대조:

- 사라진 case: **0건** (양 스위트)
- 추가된 case: `Tide.Tests.T0ActionPlatePlayModeTests.CompletedC1ActionPlatesCarryAffordanceFocusInversionAndThreeTextTiersWithoutChangingActionsLabelsOrReflow` **1건뿐**
- 상태가 바뀐 case: **0건**
- 유일한 skip은 `T0BootSceneTests.SerializedBootLoadsHubAndInitializesVisibleStartScreen`으로
  M14/M15/M17/M18 베이스라인에서도 skip이다. 기존 항목이며 이번에 생긴 것이 아니다.

[OBSERVED] 이 전역 변경이 깨뜨릴 수 있었던 인접 표면이 모두 통과했다:
`M7UiSkinPlayModeTests`(스킨 게이트 OFF 리터럴 계약 · ON 배킹 계약 · **배율 1.5에서 커밋 사각형
무이동** · 전 버튼 raycast 유지), `T0PlayModeTests`(배율 1.5 `WrappedButtonHeight` 라벨 담김),
`ScrollReachabilityTests`(도달성), `T0CaseThreadTests`(사건카드 가드 · 라벨 복사 금지 · 카드
비-raycast), `C1SignaturePlayModeTests`(150% 카드 맞춤), `ReviewNotesPlayModeTests`,
`C1InterviewPrepPlayModeTests`(M18 표면), `M5DirectionPlayModeTests`, `M9CoreTests`.

## 4. 빌드 영수증

[OBSERVED] 표준 규칙대로 빌드 **직전에** 게이트를 측정했다:

| 항목 | 값 | 판정 |
|---|---|---|
| 빌드 전 여유 디스크 | **10.24 GiB** (10,736,908 KiB, `/System/Volumes/Data`) | ≥ 10 GiB → 빌드 허용 |
| Unity Editor 프로세스 | **없음** (`Unity.app` / `Unity Hub` / `UnityShaderCompiler`) | 허용 |
| `unity/Unknown/Temp/UnityLockfile` | **부재** | 허용 |
| 빌드 후 여유 디스크 | 10.14 GiB | — |

| 항목 | 값 |
|---|---|
| 메서드 | `Tide.EditorTools.T0ProjectBuilder.BuildMac` |
| 결과 | `T0_MAC_BUILD Succeeded bytes=403840809` (`build-mac.log:6036`), exit 0, `error CS` 0건 |
| 디스크 실측 | **403,840,809 바이트 / 315 파일 — 보고값과 정확히 일치** |
| 메인 바이너리 | `Unknown.app/Contents/MacOS/Unknown T0`, 67,916 바이트, mtime `Sep 13 15:04:12` |
| 메인 바이너리 sha256 | `aa0ea126f97c13fdff514cc1d67424a850362eccb80650697dd0f197e7e02134` |
| `globalgamemanagers` sha256 | `766105021fffc3a7f72b296b8d5558004c3d017d212c02ea66806d610f74b30e` |
| M18 빌드와 비교 | 바이트 403,839,508 → **403,840,809**, sha256 `aeb397f5…` → `aa0ea126…` — **M19 소스가 실제로 반영된 새 빌드** |

[OBSERVED] `BuildMac`은 `Prepare()`를 호출하지 않으므로 씬·에셋을 재생성하지 않았고
`Builds/`·`Library/`·`Temp/`는 `unity/Unknown/.gitignore` 대상이라 작업 트리를 오염시키지 않았다.
`caffeinate -dimsu`를 빌드에 부착했다.

## 5. 네이티브 실행과 캡처 (명시적 출처)

[OBSERVED] 실행 인자와 격리 저장 루트:

```
open -n unity/Unknown/Builds/T0-mac/Unknown.app --args \
  --t0-save-dir /tmp/unknown-m19-capture-1789279488 \
  -screen-width 1280 -screen-height 800 -screen-fullscreen 0
```

### 5.1 PID 모호성을 실제로 해소했다 (중요)

[OBSERVED] 실행 직후 `Unknown T0` 프로세스가 **3개** 존재했고 **모두 같은 번들 ID**
`com.TideRegistry.Unknown-T0`를 갖는다. 따라서 번들 ID 지정 캡처는 **틀린 프로세스를 잡는다**:

| PID | 시작 | 실행 경로 / 저장 루트 | 소유 |
|---|---|---|---|
| 57356 | 09-13 10:50:46 | `Builds/C1-M4-mac/…` | **다른 세션** (선행) |
| 85786 | 09-13 14:40:18 | `Builds/T0-mac/…` `--t0-save-dir /tmp/unknown-m18-screen` | **다른 세션** (M18, 선행) |
| **92370** | **09-13 15:04:48** | `Builds/T0-mac/…` `--t0-save-dir /tmp/unknown-m19-capture-1789279488` | **이번 M19** |

[OBSERVED] `orca computer get-app-state --app com.TideRegistry.Unknown-T0 --window-index 0`은
실제로 **pid 57356**(다른 세션의 C1-M4 빌드)을 반환했다. 그 결과는 **폐기**했고 영수증에 포함하지
않았다. 내 프로세스는 `ps -o command=`에서 **내가 만든 저장 루트 문자열**로 식별했고, 창 목록은
`m16-runtime-capture/tools/listwindows`(CGWindowList, 읽기 전용 재사용)로 열거해 pid 92370 소유
창 id **5296**을 얻었다.

| 항목 | 값 |
|---|---|
| 캡처 명령 | `orca computer get-app-state --app pid:92370 --window-id 5296 --restore-window --json` |
| 캡처 엔진 | `screenCaptureKit`, `screenshotStatus.state = captured`, `metadata.windowId = 5296` |
| 스냅샷이 보고한 앱 | `pid 92370`, `com.TideRegistry.Unknown-T0` — **요청한 PID와 일치** |
| 창 | id 5296 / 좌표 (957,50) / 640×432 pt, `isOffscreen:false`, `isMinimized:false` |
| 캡처 방식 | **명시적 단일 창 캡처**. 데스크톱 전체 캡처 **미사용** |
| 캡처 해상도 | 1088×734 px (scale 1.7) |
| 파일 | `shots/00-m19-title-pid92370-window5296.png`, 897,798 bytes |
| sha256 | `1514dd0c1a1158a843e21f57af18e0958a2af9e887023cc315540d3ef219f7d0` |
| `screencapture -l 5296` | **실패** — `could not create image from window` (M16이 기록한 TCC 화면기록 거부와 동일). 우회 시도 0건 |

### 5.2 캡처에서 실제로 보이는 것 [OBSERVED]

관측된 시각 상태는 **1종뿐이다** — 진입 전 타이틀 화면(`started == false`).

- **`당직 시작 / 계속`** (포커스됨): 판 전체가 **황동/오커**, 라벨은 **어두운 잉크색 Bold**,
  좌측 눈금도 잉크색으로 **반전**. 어두운 배경과 섞이지 않는다.
- **`접근성 · 설정`** (비포커스): 판은 **어두운 잉크색**, 라벨은 **밝은 종이색 Bold**,
  좌측에 **황동 눈금** 가시.
- 두 판 상단에 황동 모서리 선(얇음), 우측 작업면의 M7 넝마지 질감, 헤더/푸터 청동 띠,
  우하단 `Development Build` 워터마크.

즉 §1.1의 P1(눈금·모서리) · P2(포커스 반전) · P4(라벨 Bold와 본문 구분)가 **실제 네이티브
플레이어에서 렌더된다**. 이 화면은 목·프리비즈·에디터 프리뷰가 아니라 표준 macOS 플레이어다.

### 5.3 격리와 무변이

| 항목 | 결과 |
|---|---|
| 격리 저장 루트 파일 수 | 실행 전 **0** → 종료 후 **0** (쓰기 0건). 디렉터리 제거 완료 |
| 종료 대상 | **pid 92370만** `kill`. 종료 후 57356 · 85786은 **여전히 실행 중**(다른 세션 보존) |
| Player.log | `~/Library/Logs/TideRegistry/Unknown T0/Player.log`, mtime `15:04:55`(= 내 실행), 직전 로그는 `Player-prev.log` mtime `14:40:24`(M18 세션) — 로테이션으로 소유 확인 |
| Player.log 스캔 | `exception|NullReference|Error|Assertion|stack trace` 대소문자 무시 매치 **0건**. 마지막 줄은 정상 `T0_BOOT session-initialized` |
| 사본 | `logs/player-m19-pid92370.log` sha256 `f48d4820db871a4a62ba7af9a213c3089876138080fcc175940dea64b447b9b5` |

## 6. 입력 한계 (기존 조건 유지)

[OBSERVED] M16이 기록한 **macOS TCC 거부**로 합성 입력이 전달되지 않는다
(`osascript` `-25211`, `orca computer click` `window_not_focused`). 이번에도 **우회를 시도하지
않았고** 승인 상태를 변경하지 않았다. 따라서 **타이틀 화면만 캡처했고 진입 후 화면은 캡처하지
못했다** — 이는 OS 권한 문제이며 게임 코드 결함이 아니고, 게임플레이 결함으로 재분류하지 않았다.

**결과적으로 네이티브 캡처로 직접 확인된 M19 표면은 타이틀의 액션 판 2개**다. 나머지 화면
(허브 · 판독기 · 회로 · 서명지 · 오버레이 · 툴바 · 내비게이션 · 비활성 판 · 보조 설명 3단 위계)은
**PlayMode 렌더 계층 단언**으로 검증했고, 같은 `Button()` 코드 경로를 공유한다는 점은
**구조에 의한 [INFERENCE]** 이다. 네이티브 캡처로 주장하지 않는다.

## 7. 범위 준수 — M14/M15/M16/M17/M18 보존

[OBSERVED] 첫 쓰기 전에 `git status --porcelain` 50행을 기록하고, 그 경로들을 펼친
**파일 98건**을 sha256으로 베이스라인화했다. 테스트 4회 + 빌드 1회 + 네이티브 실행 1회 이후 재검증:

| 검사 | 결과 |
|---|---|
| HEAD | `52ad5510522eca6d5cfd8d743d85f28cb254635f` **불변** |
| 베이스라인 98건 중 내용이 바뀐 파일 | **0건** |
| 삭제/사라진 베이스라인 파일 | **0건** |
| 바이트 동일 | **98 / 98** |
| `graphify-out/*` (4건) | **바이트 불변** — 재생성·수기 편집 모두 없음 |
| `.mex/*` | **불변** — `mex` 계열 미실행 |
| M14~M18 영수증 디렉터리 | 전부 존재 (11 / 9 / 11 / 10 / 12 파일), 편집 0건 |
| `M7UiSkinProfile.cs`, `Resources/M7UiSkin.asset` | **clean** — 승인 플래그·프로필 값 0건 변경 |

[OBSERVED] `reset` · `stash` · `revert` · `clean` · `checkout` · `commit` · `push` **미실행**.
외부 공개 0건 · 에셋 구매/생성 0건 · 2D/3D/이미지/영상 생성 0건 · 런타임 승인 변경 0건.

M19가 추가한 작업 트리 항목은 **4개**이며 전부 M19 소유다:

```
 M unity/Unknown/Assets/_Project/UI/T0Interface.cs
?? unity/Unknown/Assets/_Project/Tests/PlayMode/T0ActionPlatePlayModeTests.cs (+ .meta)
?? _workspace/current/presentation/t0-action-plate-m19.md
?? _workspace/current/systems/tech-verification/m19-ui-polish/
```

### 7.1 다른 세션의 동시 쓰기 — 관측과 처리 (CLAUDE.md §10.1)

[OBSERVED] 이번 세션이 **만들지 않은** 작업 트리 항목이 세션 도중 나타났다:

```
 M assets/generated/2d/ui/provenance.json                          (mtime 14:54:00)
?? assets/generated/2d/ui/m19-interview-control-surface-r01.png    (mtime 14:48:53) + .prompt.txt
?? assets/generated/2d/ui/m20-archival-work-surface-r02.png        (mtime 14:53:10) + .prompt.txt
```

- 이 파일들은 **내 베이스라인 스냅샷(≈14:57)에 없었고**, mtime은 그보다 이른 14:48~14:54다.
  이번 세션은 **이미지 생성 명령(`gti`/`gen-2d.sh`/Higgsfield/Blender)을 단 한 번도 실행하지 않았다.**
- 동시 실행 중인 `claude --dangerously-skip-permissions` 프로세스가 **3개** 확인된다
  (pid 94988 · 47629 · 47902). 그 중 한 세션의 산출물로 판단한다 [INFERENCE].
- **처리**: 손대지 않았다 — 되돌리지 않고, 스테이징하지 않고, 편집하지 않았다.
- **내 검증에 영향 없음 [OBSERVED]**: 해당 파일들은 저장소 루트 `assets/generated/2d/ui/`에 있고
  **`unity/Unknown/Assets/` 안이 아니므로 Unity가 임포트하지 않는다**. 새 PNG에 `.meta`가 없고,
  Unity 소스에서 이 이름을 참조하는 곳이 **0건**이며, provenance의 6개 항목은 전부
  `runtimeEligible: false`다. 또한 mtime이 내 테스트(15:00~15:03)·빌드(15:04)보다 **앞선다**.
- **경고**: 그 세션이 `m19-…` · `m20-…` 라벨을 쓰고 있어 **마일스톤 라벨이 충돌**한다.
  이 영수증의 "M19"는 **`T0Interface` 표현 슬라이스**만 가리킨다. 누가 M19 라벨을 최종적으로
  소유하고 그 에셋 작업을 마무리할지는 **디렉터/사용자 판정이 필요하다** — 이번 세션이 대신
  결정하지 않았다.

## 8. 그래프/메모리 신선도 한계 (G8 주장 없음) — [UNGRAPHED]

[OBSERVED] `graphify update .` 와 `mex` 계열(`mex graph`, `mex check`, `mex sync`, `mex log`)을
**의도적으로 실행하지 않았다.** 이유: `graphify-out/`의 4개 파일과 `.mex/` 산출물이 이미 커밋되지
않은 선행 작업 상태이며, 이번 지시가 그 보존을 요구했다. 재생성은 그 미커밋 산출물을 덮어쓴다.

**정확한 결과**: 이번 코드 변경(수정 1건, 신규 1건)은 코드 그래프와 프로젝트 메모리에
**반영되지 않았다**. `graphify-out/graph.json`·`GRAPH_REPORT.md`와 `.mex/`는 M19 이전 상태를
가리키며 **그래프/메모리 신선도는 갱신되지 않았다**. CLAUDE.md §5 시스템 레인 순서 중 편집 이후
단계(`graphify update .` → `mex graph` → `mex check` → `mex log`)는 **미이행**이다.
`game-systems-designer` 정의에 따라 **`[UNGRAPHED]`** 로 표시한다.

- **G8을 PASS로 주장하지 않는다.** 신선도·메모리 게이트는 이번 증분으로 움직이지 않았다.
- 이 머신의 `mex`는 TeX Live이고 `scripts/mex-agent-bin.sh`가 **존재하지 않아** 신원 확인이
  불가능하다 (session-start 출력: `mex: skipped — mex-agent not found or failed identity probe`).
  따라서 memory_sync 영수증은 **`skipped`** — 도구 부재 + 보존 요구의 **중첩** 결과다.
- 검색 우선 순서 중 `zg query`/`graphify query`는 읽기 전용으로 사용 가능했으나, 이번 탐색은
  `grep`/`read`로 수행했다. 후속 작업: 선행 미커밋 산출물의 소유 세션이 정리된 후
  `graphify update .` 와 mex 동기화를 한 번에 수행해야 한다.

## 9. 한계 — 주장하지 않는 것

1. **표현 기초 한 조각이다.** 프로그래밍 도형 2개(`Bevel`, `Rule`)와 색 반전으로 만든 표현이며,
   **최종/출시 아트가 아니다.** 아트 디렉션 확정도, 비주얼 레인 산출물의 대체도 아니다.
   스프라이트·9-슬라이스·셰이더·프레임 텍스처 작업은 이번 범위 밖이다.
2. **접근성 인증이 아니다.** 연출 계약 §6의 대비 수치는 **평면 색 sRGB 산술값**이며
   (커밋 리터럴: 비선택 라벨 11.23:1 · 선택 라벨 4.80:1 · 본문 11.23:1 · 보조 4.56:1 /
   M7 스킨: 10.57 · 6.80 · 10.57 · 4.39), 아래 깔린 넝마지·청동 텍스처, 글리프 안티에일리어싱과
   한국어 획 두께, 실제 디스플레이 감마를 반영하지 않는다. **WCAG 등 어떤 표준의 적합·인증도
   주장하지 않는다.** 실측 대비·가독성 검증은 NOT-MEASURED다.
3. **비활성 판은 이 픽스처에서 관측되지 않았다.** 완료된 C1 화면의 액션 3개는 모두 활성이므로
   P3는 `Rule.activeSelf == button.interactable` 이라는 **보편 법칙 형태**로 단언했다.
   실제 비활성 인스턴스에서의 시각 확인은 이번에 하지 않았다 [한계].
4. **사람 플레이테스트 n=0.** 자동 테스트 + 빌드 + 타이틀 1장 캡처뿐이다. "게임스럽다"는
   사용자 요청의 충족 여부는 **주관적 판단이며 측정하지 않았다**.
5. **성능 주장 0건.** 버튼당 비-raycast `Image` 2개가 추가되므로 드로 콜/배칭 영향이 있을 수
   있으나 프레임 시간 · GPU · 메모리를 **측정하지 않았다**. 최대 노출 화면은 `phasePicker`
   (액션 30개 → 추가 Image 60개)로 **추정**될 뿐이다 [INFERENCE].
6. **진입 후 네이티브 캡처 0장.** §6 참조. 합성 입력 0회.
7. **G1–G8 이동 0건.** G4 `NOT-MEASURED` 유지, G8 주장 없음(§8). 연출 계약은 `status: draft`이며
   미해결 ● ack(synopsis, vfx, animation, motion, modeling, qa)가 남아 있어 게이트 투입 불가.
8. **호버(highlighted) 색은 의도적으로 그대로 두었다.** 기존 `buttonHighlight` 곱셈이 유지되므로
   비포커스 판에 마우스를 올리면 여전히 약간 어두워진다. 포커스 상태가 Unity `Selectable`에서
   호버보다 **우선**하므로 포커스된 판은 황동을 유지한다 — 이는 코드 구조에 의한 [INFERENCE]이며
   포인터 호버는 단언하지 않았다.
9. 텍스트는 한국어 저작본만 검증했다. 영문(`language: en`) 경로의 리플로는 측정하지 않았다.
10. 커밋/푸시 **미실행**. 사용자가 수행한다.
