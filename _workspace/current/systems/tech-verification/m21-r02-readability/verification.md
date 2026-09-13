---
updated: 2026-09-13
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
---

# M21 — r02 게이트 ON 본문 가독성 정정: 엄격 TDD 영수증

> **이 문서가 무엇인가**: M20 네이티브 캡처가 드러낸 **결함 하나**(게이트 ON에서 작업면 본문
> 글리프:지면 1.11:1)를 가장 작은 content-safe 정정으로 고치고, 실제 네이티브 플레이어에서
> 촬영·측정한 영수증이다. 승격이 **아니고**, 텍스트 재색칠이 **아니며**, 접근성 인증·성능·사람
> 플레이테스트 주장도 **아니다**. 어떤 게이트도 올리지 않는다.

연출 계약(소유 산출물): `_workspace/current/presentation/t0-reading-band-m21.md` (`status: draft`)

## 0. 결론 — 실측

[OBSERVED] **띠가 본문 가독성을 실제로 되살렸고, r02는 띠 바깥에서 픽셀 단위로 보존되었다.**

같은 창 기하(640×432 pt, 1088×734 px, scale 1.7)의 캡처 3장을 **같은 픽셀 사각형**에서 측정.
측정 스크립트: `tools/measure-glyph-ground.py`, 원시 결과: `shots/glyph-ground-measurement.json`.
사각형은 추정이 아니라 **유도**했다 — 작업면 패널 = M19와 M20이 열의 과반에서 불일치하는 행
구간(287–639, 즉 r02가 대체한 지면), 본문 띠 = 그 첫 구간의 글리프 보유(고분산) 행(302–314).

### 본문 띠 (x 522–1044, y 302–315)

| 측정 | M19 베이스라인 | M20 게이트 ON | **M21 게이트 ON** |
|---|---|---|---|
| 평균 상대휘도 | 0.4665 | 0.0254 | **0.4314** |
| 표준편차 (글리프 존재 신호) | 0.1610 | **0.0030** | **0.1432** |
| 글리프 코어(하위 2%) | 0.0923 | 0.0188 | 0.0893 |
| 지면(상위 20%) | 0.6191 | 0.0294 | 0.5394 |
| **글리프 : 지면** | **4.70 : 1** | **1.15 : 1** | **4.23 : 1** |

- **글리프 신호가 돌아왔다**: 표준편차 0.0030 → **0.1432** (베이스라인 0.1610의 89%).
- **대비가 돌아왔다**: 1.15:1 → **4.23:1** = 같은 사각형에서 잰 M19 베이스라인의 **90.0%**.
- M20 영수증이 공표한 1.11:1 / 3.53:1과 이 표의 1.15:1 / 4.70:1은 **사각형이 달라서** 다르다.
  M20의 절대값을 재현한 것이 아니라, **세 캡처를 같은 사각형에서 다시 재어** 비교 가능하게 만든 값이다.
  붕괴(약 1.1:1)와 회복(약 4.2:1)의 방향·규모는 두 사각형에서 동일하게 관측된다.

### 빈 작업면 대조군 (x 522–1044, y 456–594, 어느 캡처에도 글자 없음)

| 측정 | M19 베이스라인 | M20 게이트 ON | M21 게이트 ON |
|---|---|---|---|
| 평균 상대휘도 | 0.5842 | 0.0254 | **0.0254** |
| 표준편차 | 0.0364 | 0.0023 | **0.0023** |
| 글리프 코어 | 0.4793 | 0.0204 | **0.0204** |
| 지면 | 0.6316 | 0.0287 | **0.0287** |

[OBSERVED] **M21의 대조군 통계가 M20과 완전히 동일하다** (`r02_preserved_outside_the_band: true`).
즉 띠는 문단 밑에만 있고 **r02 표면은 띠 바깥에서 건드려지지 않았다**. 띠가 덮은 행은 패널 353행 중
**13행 = 패널 높이의 3.7%** 다. 전면 판이 아니다.

| 단계 | 결과 | 영수증 |
|---|---|---|
| 집중 RED (1차) | 1 case, 0 passed, **1 failed** — 읽기 처리 **부재** 단언에서 실패 | `red.xml`, `red.log` |
| 집중 RED (2차) | 1 case, 0 passed, **1 failed** — 내 기대값의 **좌표계 오류** (§3.3) | `red2.xml`, `red2.log` |
| 집중 GREEN | 1 case, **1 passed**, 0 failed, 1.79s | `green.xml`, `green.log` |
| EditMode 전체 | 54 total, **54 passed**, 0 failed — case 집합 M20과 **동일** | `editmode.xml` |
| PlayMode 전체 | 89 total, **88 passed**, 0 failed, 1 skipped(기존) — **+1 = 이번 테스트뿐** | `playmode.xml` |
| macOS 플레이어 빌드 | `T0_MAC_BUILD Succeeded bytes=408564125`, 디스크 실측 일치, `error CS` 0건 | `build-mac.log:6097` |
| 네이티브 캡처 (게이트 ON) | **pid 81512 / window 5976 명시 지정** 단일 창 | `shots/00-m21-gate-on-pid81512-window5976.png` |
| 네이티브 캡처 (같은 빌드, 게이트 OFF) | **pid 95244 / window 5988**, M19·M20-게이트OFF 영수증과 **sha256 동일** | `shots/01-m21-gate-off-same-build-pid95244-window5988.png` |
| Player.log ×2 (전용 `-logFile`) | 예외·에러 **0건**, 부팅 4단계 완주 | `logs/player-m21-gate-{on,off}.log` |
| 보존 | 베이스라인 154건 중 관련 그룹 **전부 바이트 동일**, HEAD 불변 | §7 |
| **동시 세션 충돌** | 캡처 이후 다른 세션이 같은 트리를 씀 → 현재 트리에서 스위트 재현 **불가** | §8 — **반드시 읽을 것** |

## 1. 고친 결함과 고친 방법

[OBSERVED · M20 인용] `m20-ui-surface/verification.md` §0·§9.2: 게이트 ON에서 커밋 본문색
(어두운 `ink`)이 어두운 r02 지면에 얹혀 본문 띠 평균(0.0255)과 빈 배경 평균(0.0253)이 사실상
같아졌다. M20은 텍스트를 바꿀 권한이 없어 결함으로만 기록했다.

M20 §9.2의 세 선택지 중 **(b) 텍스트 뒤 content-safe 판**을 택했다. (a) 본문 색조 반전은 M19 3단
위계와 게이트 OFF 룩을 함께 건드리고, (c) r02 밝은 변형 재생성은 **신규 에셋 생성**이라 금지 범위다.

### 1.1 프로덕션 변경 (최소)

| 파일 | 변경 | sha256 |
|---|---|---|
| `UI/M21ReadingBackingInterface.cs` | **신규** — `T0Interface` partial 1개 + `ReadingBandFollow` 1개 | `3efac7422de95999bd2017c024b44c4b8b39e54544ea6cbe98dbea3b840ea547` |
| `UI/T0Interface.cs` | **추가 3행뿐** (주석 2 + `ApplyM21ReadingBacking(content);` 1) | M14~M21 누적 `+73/−8` vs HEAD |

[OBSERVED] M14~M20이 쓴 행은 **하나도 수정하지 않았다**. M20 §7이 기록한 M20 편집 2건
(`T0Interface.cs`, `T0GameSession.cs`)의 내용은 그대로 존재한다(`FullBleedBacking("M20 work surface"…)`
포함). `App/T0GameSession.cs`는 이 세션이 **편집하지 않았다**.

동작:

```
게이트 OFF  → surface == null → 띠 0개, 커밋 M7 타일 경로 그대로 (픽셀 불변, §6.4)
게이트 ON   → 작업면 Content의 직접 자식 문단("Text")마다
              [i]   M21 reading : Image(paper.rgb, α .82), raycastTarget=false,
                                  LayoutElement.ignoreLayout=true, sprite=null
              [i+1] Text        : 문구·색·크기·굵기 전부 불변
```

- 띠는 **문단의 직전 형제**이므로 Unity UI 깊이우선 순서에서 **글리프 뒤**에 그려진다.
- `ignoreLayout=true`이므로 `VerticalLayoutGroup`이 띠를 세지 않는다 → **커밋 문단·버튼 0px 이동**.
- 버튼은 자기 불투명 판을 가지므로 **띠를 받지 않는다**(계약 R3). 방향 섹션·사건 카드도 제외.
- `ReadingBandFollow.LateUpdate`가 기하를 미러한다. 1회 복사는 어긋난다 — `FlowText`는 `minHeight`만
  고정하고 `WrappedButtonHeight`가 **이후 프레임에** 열을 재유동시키기 때문이다. 진단 게이트 전용이며
  게이트 OFF에서는 이 컴포넌트가 **존재하지 않는다**.

## 2. 승인 상태 — 이번 세션이 건드리지 않은 것

| 항목 | 값 | 판정 |
|---|---|---|
| `M20WorkSurface.asset` | `runtimeApproved: 0` | **불변** |
| `provenance.json` r02 `runtimeEligible` | `false` | **불변** |
| `provenance.json` r02 `promoted_by` | `null` | **불변** |
| r02 PNG sha256 | `cc52db8a…3d75a7` = provenance `output_sha256` | **바이트 불변** |
| `M20WorkSurfaceProfile.cs` / `M20WorkSurfaceProjectBuilder.cs` / 후보 텍스처 | 이 세션 편집 **0건** | §7 |
| `M7UiSkin.asset` | 이 세션 편집 **0건** (§8의 타 세션 변경은 별개) | — |
| r01 | 임포트·참조·연결 **0건** | 미승인·미통합 유지 |

**M20 r02는 이 정정 이후에도 미승격이다.** 이 영수증은 승격 근거가 아니다. 게이트 ON 표면은
여전히 `--m20-ui-surface-diagnostic` / `diagnosticOverride` 뒤에만 존재하고, 승격 경로는 디렉터 전용
decision-log 감사로만 열린다. 이번 세션은 승인 플래그를 **메모리에서도** 뒤집지 않았다(§3.2).

## 3. TDD 증거

러너: `Unity 6000.5.6f1 -batchmode -nographics -runTests -testPlatform PlayMode`,
필터 `Tide.Tests.T0ReadingBackingPlayModeTests`. 신규 테스트
`Tests/PlayMode/T0ReadingBackingPlayModeTests.cs`
(sha256 `c95c2b68ddbddb32269be9a9ba8028dd7ef8189040f03f183eb1718319a1c283`).

### 3.1 픽스처와 단언 방식

M20 스위트와 같은 커밋 픽스처 `_Project/Tests/Fixtures/C1PatrolCompletedV2.json`을 **양쪽 절반에
각각 새 임시 저장 루트로 복사**해 부팅했다 — 실제 도달 가능하고, 라이브 버튼과 M19 텍스트 3역할이
한 번에 렌더되는 화면이다. **목(mock) 0건 · 수기 `GameScreen` 0건 · 소스텍스트 단언 0건.**
모든 단언은 실제 Unity 컴포넌트 상태를 읽는다: `Image.color`/`sprite`/`raycastTarget`,
`LayoutElement.ignoreLayout`, `RectTransform` 앵커·`rect`·`GetWorldCorners`·형제 index,
`Text.color`/`fontSize`/`fontStyle`/`preferredHeight`, `Selectable.colors`, `RawImage.texture`/`uvRect`.

대비 수치는 **라이브 컴포넌트 색**에서 계산한다. 반투명 지면은 투과된 것만큼만 밝으므로
**검정 위 합성**(`OverBlack`)을 써서 **어떤 지면에서도 성립하는 하한**을 단언한다 — 압축된 후보
텍스처를 GPU에서 되읽지 않고도 계산 가능한 보수적 경계다.

### 3.2 게이트를 `runtimeApproved`로 흔들지 않았다

M20 선례를 따라 `[NonSerialized] diagnosticOverride`만 조작했다. `[UnityTearDown]`이 값을 복원한 뒤
**`runtimeApproved`가 변하지 않았음을 단언**하고, 테스트 본문은 시작 시 `runtimeApproved == false`를
단언한다. 승인 플래그는 디스크에도 메모리에도 기록되지 않았다.

### 3.3 RED은 부재 단언에서 실패했다

**1차 RED** (`red.xml`, 프로덕션 코드 작성 **전**):

```
gate on: every work-surface paragraph must sit on a content-safe reading band, because the
committed ink text is unreadable directly on the dark r02 ground (measured glyph:ground 1.11:1)
  Expected: 3
  But was:  0
  at T0ReadingBackingPlayModeTests.cs:247
```

[OBSERVED] 이 단언 **앞의 모든 단언이 통과**했다 — 프로필·텍스처 로드, `runtimeApproved==false`,
게이트 OFF에서 완료 C1 화면 도달(`PatrolActive`/`PatrolComplete`), 커밋 액션 순서, **띠 0개**,
타일 `M7 paper` 1개, M19 판 계약 전부(모든 버튼 `Bevel`/`Rule`·`Rule.activeSelf==interactable`·
`selectedColor==white`·`fadeDuration==0`·포커스 반전·버튼 내 `RawImage` 0개·**버튼 내 띠 0개**·3단
위계), 문단 2개 초과 존재, 그리고 게이트 ON에서 r02 배킹 1개·비타일 `uvRect`·형제 index 0·비raycast·
`M7 paper` 0개·문단 수 동일까지. 즉 **도달하지 못해서가 아니라 읽기 처리가 없어서** 실패했다.

**2차 RED** (`red2.xml`, 프로덕션 코드 작성 **후**): 구현이 아니라 **내 기대값이 틀렸다**.

```
gate on: the band must cover the whole flowed paragraph, including lines that overflow its layout rect
  Expected: less than or equal to 169.547897f
  But was:  241.551254f
```

[OBSERVED] 원인: 내가 **월드 좌표**(`GetWorldCorners`)와 **캔버스 로컬 단위**(`Text.preferredHeight`)를
한 식에서 비교했다. 캔버스는 `ScaleWithScreenSize` 1600×900이므로 두 단위는 같지 않다.

- 정정 방향은 **완화가 아니라 강화**다: 월드 좌표 포함 검사를 **4면 전부**로 늘리고(아래쪽 변 추가),
  흐른 높이 검사는 `content` 자식들이 공유하는 **로컬 단위**로 분리해 단위 일관성을 맞췄다.
- **GREEN 이후에 테스트를 고쳐 실패를 숨긴 것이 아니다** — 이 시점에 테스트는 한 번도 통과하지 않았다.

**GREEN** (`green.xml`): 1 case, **1 passed**, 0 failed, 1.79s.

### 3.4 테스트가 단언하는 플레이어 관측 사실

| 항목 | 단언 |
|---|---|
| 게이트 OFF 보존 | `Content`에 띠 **0개**, 타일 `M7 paper` 1개, M19 판·반전·3단 위계 전부 성립 |
| 1문단 1띠 | 작업면 문단 수 == 띠 수, 각 띠가 해당 문단의 **직전 형제**(= 글리프 뒤) |
| 입력 밖 | 각 띠 `raycastTarget == false`, `Focus`/`Activate("c1-review")`가 실제로 화면을 바꿈 |
| 레이아웃 밖 | 각 띠 `LayoutElement.ignoreLayout == true` |
| 신규 에셋 0 | 각 띠 `sprite == null` (단색 `Image`) |
| 기하 | 띠 월드 사각형이 문단 월드 사각형을 **4면 포함**, 로컬 높이 ≥ `max(rect 높이, preferredHeight)` |
| 저노이즈 | `0 < α < 1` **및** `α ≤ 0.85`, 띠 너비 < 패널 너비, 띠 높이 < 패널 높이×0.6 |
| 대비 회복 | 본문: 최악 지면 기준 **> 4.0:1**; 본문·보조 둘 다 자기 게이트 OFF 지면 대비의 **> 55%** 회복 |
| 재색칠 0 | 문단 `(name, color, fontSize, fontStyle, text)` 스냅샷이 게이트 ON/OFF **완전 동일** |
| 무변이 | 액션 ID 순서·라벨 딕셔너리·`interactable`·포커스 링 크기·작업면 자식 수·`Viewport` index/rect·패널 색 동일, `SavePending` 거짓 |
| M19 불가침 | **모든** 버튼: 루트 raycastable, `Bevel`/`Rule` 존재, `Rule.activeSelf==interactable`, Label 1개, `RawImage` 0개, **띠 0개** |
| M20 불가침 | r02 배킹 정확히 1개·`profile.workSurface`와 참조 동일·`uvRect==(0,0,1,1)`·형제 index 0·비raycast·`M7 paper` 0개 |
| 재렌더 내구성 | 클릭 후 재렌더에서도 띠 수 == 문단 수, 전부 비raycast |
| 승인 불변 | 본문 `runtimeApproved==false` 단언 + `[UnityTearDown]` 불변 단언 |

## 4. 전체 회귀

| 스위트 | 이번 실행 | M20 베이스라인 | 델타 |
|---|---|---|---|
| EditMode | 54 total, 54 passed, 0 failed, 0 skipped | 54 / 54 | **0** — case 집합 동일 |
| PlayMode | 89 total, **88 passed**, 0 failed, 1 skipped | 88 total, 87 passed, 0 failed, 1 skipped | **+1** = 이번 테스트 |

[OBSERVED] M20의 `editmode.xml`·`playmode.xml`과 leaf case 집합을 기계 대조:

- 사라진 case **0건**, 상태가 바뀐 case **0건** (양 스위트)
- 추가된 case: `T0ReadingBackingPlayModeTests.M21GateOnGivesWorkSurfaceBodyTextAContentSafeReadingBandWithoutTouchingCopyColoursOrPlates` **1건뿐**
- 유일한 skip은 `T0BootSceneTests.SerializedBootLoadsHubAndInitializesVisibleStartScreen` — M14~M20
  베이스라인에서도 skip인 **기존 항목**이며 이번에 생긴 것이 아니다.
- 인접 표면 통과: `T0WorkSurfacePlayModeTests`(M20 계약), `M7UiSkinPlayModeTests`,
  `T0ActionPlatePlayModeTests`(M19 판), `T0PlayModeTests`, `ScrollReachabilityTests`,
  `T0CaseThreadTests`, `C1SignaturePlayModeTests`, `C1InterviewPrepPlayModeTests`,
  `ReviewNotesPlayModeTests`, `M5DirectionPlayModeTests`, `M9CoreTests`.

## 5. 빌드 영수증 — 게이트를 빌드 직전에 실측했다

| 항목 | 값 | 판정 |
|---|---|---|
| 빌드 전 여유 디스크 | **10.9190 GiB** (11,449,440 KiB, `/System/Volumes/Data`) | ≥ 10 GiB → **빌드 허용** |
| Unity Editor 프로세스 | **없음** (`Unity.app/Contents/MacOS/Unity` / `Unity Hub` / `UnityShaderCompiler`) | 허용 |
| `unity/Unknown/Temp/UnityLockfile` | **부재** | 허용 |
| 빌드 전 기존 메인 바이너리 sha256 | `e28544f6…a88561` = **M20 영수증 값과 일치** (연속성 확인 후 덮어씀) | — |
| 빌드 후 여유 디스크 | 10.9141 GiB | — |

| 항목 | 값 |
|---|---|
| 메서드 | `Tide.EditorTools.T0ProjectBuilder.BuildMac` |
| 결과 | `T0_MAC_BUILD Succeeded bytes=408564125` (`build-mac.log:6097`), exit 0, `error CS` **0건** |
| 디스크 실측 | **408,564,125 바이트 / 315 파일 — 보고값과 정확히 일치** |
| 메인 바이너리 | `Unknown.app/Contents/MacOS/Unknown T0`, 67,916 B, sha256 `defe2da7c591d145e13d33797059106490602e2296ccaa6e1f948b0ef562cc02` |
| `globalgamemanagers` sha256 | `df63fe5eb5cd9732edb747697c377cb07efca111458f895ee11e370599f7513d` |
| M20 빌드와 비교 | 408,563,585 → 408,564,125 = **+540 바이트**, 메인 sha `e28544f6…` → `defe2da7…` |

[OBSERVED] **+540 바이트뿐**이라는 것이 "신규 에셋 0건"의 빌드 측 증거다 — M20은 r02 텍스처
때문에 +4.7 MB였다. M21은 **코드만** 늘렸다. `BuildMac`은 `Prepare()`를 호출하지 않아 씬·에셋을
재생성하지 않았고 `Builds/`·`Library/`·`Temp/`는 `.gitignore` 대상이라 작업 트리를 오염시키지 않는다.
모든 빌드·테스트 런에 `caffeinate -dimsu`를 부착했다.

## 6. 네이티브 실행과 캡처 (명시적 출처)

### 6.1 PID 모호성을 실제로 해소했다

[OBSERVED] 실행 시점에 `Unknown T0` 창이 **3개** 존재했고 전부 같은 소유자명·**같은 기하**
(957,50 / 640×432)였다. 번들 ID 지정 캡처는 틀린 프로세스를 잡는다:

| PID | 시작 | 창 id | 실행 경로 / 저장 루트 | 소유 |
|---|---|---|---|---|
| 60510 | 15:56:58 | 5712 | `Builds/T0-mac/…` `--t0-save-dir …/unknown-m22-…/baseline-save` | **다른 세션 (M22)** |
| 65733 | 15:58:11 | 5727 | `Builds/C1-M4-mac/…` | **다른 세션** |
| 67330 | 15:58:35 | 5731 | `Builds/T0-mac/…` `--m20-ui-surface-diagnostic` | 이번 M21 · **폐기**(§6.2) |
| **81512** | **15:59:5x** | **5976** | `Builds/T0-mac/…` **`--m20-ui-surface-diagnostic`** `--t0-save-dir /tmp/unknown-m21-gateon-…` | **이번 M21 · 게이트 ON** |
| **95244** | **16:04:5x** | **5988** | `Builds/T0-mac/…` (**플래그 없음**) `--t0-save-dir /tmp/unknown-m21-gateoff-…` | **이번 M21 · 게이트 OFF** |

[OBSERVED] 내 프로세스는 `subprocess` **자식 pid로 직접** 식별했고(문자열 추측 아님), 창은
`m16-runtime-capture/tools/listwindows`(CGWindowList, 읽기 전용 재사용)로 열거해 **pid별 창 id**를
얻었다. 번들 ID 지정 캡처는 **사용하지 않았다**. 데스크톱 전체 캡처 **미사용**.

### 6.2 첫 시도는 검은 프레임이었다 — 원인을 규명하고 재실행했다

[OBSERVED] pid 67330의 캡처는 창 테두리와 `Development Build` 워터마크만 있는 **검은 프레임**
(32,583 B)이었다. 폐기하지 않고 증거로 보존한다:
`shots/02-m21-unfocused-black-frame-pid67330-window5731.png`.

원인: `ProjectSettings.asset`에 **`runInBackground`가 없다** → 기본값 false → 플레이어가 포커스를
잃으면 플레이어 루프가 멈춘다. 그 런의 `Player.log`는 `T0_BOOT ui-root-loaded`에서 **정지**했다
(`hub` 씬 비동기 로드 코루틴이 틱하지 않음). **게임 코드 결함이 아니라 포커스 조건**이다.

정정: 내 pid만 `osascript`로 **활성화**(창 관리 동작이며 게임에 합성 입력을 넣는 것이 아니다)한 뒤
`T0_BOOT session-initialized`를 **로그에서 폴링**해 부팅 완주를 확인하고 캡처했다. TCC 우회 **0회**.

### 6.3 캡처 (게이트 ON = 진단 후보 증거, promotion 아님)

| 항목 | 값 |
|---|---|
| 실행 인자 | `--m20-ui-surface-diagnostic --t0-save-dir /tmp/unknown-m21-gateon-… -logFile <전용> -screen-width 1280 -screen-height 800 -screen-fullscreen 0` |
| 캡처 명령 | `orca computer get-app-state --app pid:81512 --window-id 5976 --restore-window --json` |
| 캡처 엔진 | `screenCaptureKit`, `screenshotStatus.state = captured`, `metadata.windowId = 5976` |
| 스냅샷이 보고한 앱 | `pid 81512` — **요청한 PID와 일치** |
| 창 | id 5976 / (957,50) / 640×432 pt, `isOffscreen:false`, `isMinimized:false` |
| 해상도 | **1088×734 px (scale 1.7) — M19·M20 영수증과 동일**하므로 픽셀 사각형 비교가 유효하다 |
| 파일 | `shots/00-m21-gate-on-pid81512-window5976.png`, 838,425 B |
| sha256 | `05b1852d24800f096b2037fcb2db263fc983f83858f4c4119b5a955c05ce835f` |
| 원시 스냅샷 | `shots/appstate-pid81512-window5976.json` |

### 6.4 캡처에서 실제로 보이는 것

관측된 시각 상태는 **1종뿐이다** — 진입 전 타이틀 화면(`started == false`), M20과 동일 조건.

- **작업면(우측 하단 패널)**: 여전히 **r02의 어두운 청록/미드나이트 에나멜 단일 전면 배경**.
- **본문 한 줄**(`21:00. 인수 각서와 이관 목록을 살핀 뒤, 당직실의 기록을 대조하세요.`)이
  **종이색 띠 위에 어두운 잉크로 다시 읽힌다** — M20에서 사라졌던 그 줄이다.
- **띠는 그 문단 크기뿐**이고 아래·주변은 r02 원본 그대로 (§0 대조군이 픽셀로 확인).
- **`당직 시작 / 계속`**(포커스): 판 전체 **황동/오커** + **어두운 잉크 Bold 라벨** + 잉크 눈금 —
  **M19 계약 그대로**. **`접근성 · 설정`**(비포커스): **어두운 잉크 판** + **밝은 종이색 라벨** +
  **황동 눈금**. **두 버튼 어디에도 띠가 없다**(계약 R3 육안 확인).
- 헤더/푸터 청동 띠, 우상단 사건 카드(잉크 판 + 종이색 본문, **띠 없음**), 좌측 3D 허브 뷰포트,
  `Development Build` 워터마크는 **모두 M19·M20과 동일**.

### 6.5 게이트 OFF A/B — 기본 커밋 런타임 무변경을 픽셀로 증명

[OBSERVED] **같은 M21 빌드**를 플래그 **없이** 다시 실행해 같은 방식으로 캡처했다.

| 항목 | 값 |
|---|---|
| 파일 | `shots/01-m21-gate-off-same-build-pid95244-window5988.png`, 897,798 B |
| sha256 | **`1514dd0c1a1158a843e21f57af18e0958a2af9e887023cc315540d3ef219f7d0`** |
| M20 게이트 OFF 영수증 sha256 | **동일** |
| M19 영수증 캡처 sha256 | **동일** |
| 판정 | **바이트 동일 — M21 빌드의 게이트 OFF 화면은 M19/M20과 픽셀 단위로 같다** |

즉 M21 코드가 들어간 빌드에서도 **플래그가 없으면 화면이 한 픽셀도 달라지지 않는다.**

### 6.6 격리와 무변이

| 항목 | 결과 |
|---|---|
| 격리 저장 루트 (게이트 ON) | 실행 전 **0** 파일 → 종료 후 **0** 파일 (쓰기 0건). 디렉터리 제거 완료 |
| 격리 저장 루트 (게이트 OFF) | 동일 — **0 → 0**. 제거 완료 |
| 종료 대상 | **내가 만든 pid 67330 · 81512 · 95244만** `kill`. 종료 후 **65733(다른 세션)은 여전히 실행 중** |
| Player.log | **전용 `-logFile` 2개**를 썼다 — 플레이어 3개가 공유 `Player.log`를 돌려쓰는 상황에서 로테이션 추론 없이 소유가 확정된다 (M20 대비 개선) |
| Player.log 스캔 ×2 | `exception\|nullreference\|error\|assertion\|stack trace` 대소문자 무시 매치 **0건** (양쪽) |
| 부팅 시퀀스 | `entry-start → ui-root-loaded → hub-loaded → session-initialized` **4단계 완주** (양쪽) |
| 사본 | `logs/player-m21-gate-on.log` (6,757 B) · `logs/player-m21-gate-off.log` (6,757 B) |

### 6.7 입력 한계 (기존 조건 유지)

[OBSERVED] M16이 기록한 **macOS TCC 거부**로 합성 입력이 전달되지 않는다. 이번에도 **우회를 시도하지
않았고** 승인 상태를 변경하지 않았다. 따라서 **타이틀 화면만 캡처했고 진입 후 화면은 캡처하지
못했다** — OS 권한 문제이며 게임 코드 결함이 아니다.

**결과적으로 네이티브로 직접 확인된 M21 표면은 타이틀의 작업면 본문 띠 1종**이다. 나머지 화면
(허브·판독기·회로·서명지·오버레이)의 작업면은 **PlayMode 렌더 계층 단언**으로 검증했고, 같은
`Render()` 경로를 공유한다는 점은 **구조에 의한 [INFERENCE]** 다. 네이티브로 주장하지 않는다.

## 7. 범위 준수 — M14~M20 보존

[OBSERVED] 첫 쓰기 **전에** `git status --porcelain` 74행을 기록하고, 그 경로들을 펼친
**파일 154건**을 sha256으로 베이스라인화했다(`/tmp/m21-preserve/manifest-before.sha256`).
모든 작업 후 재검증:

| 검사 | 결과 |
|---|---|
| HEAD | `52ad5510522eca6d5cfd8d743d85f28cb254635f` **불변** |
| 삭제/사라진 베이스라인 파일 | **0건** |
| `assets/generated/**` (r01 PNG·프롬프트, r02 PNG·프롬프트, `provenance.json`) | **5/5 바이트 불변** |
| `graphify-out/*` (4건) | **4/4 바이트 불변** — 재생성·수기 편집 모두 없음 |
| `.mex/*` | **불변** — `mex` 계열 미실행, 최근 1시간 내 변경 0건 |
| M20 후보 텍스처 `Art/Candidates/m20-ui-r02/*` (3건) | **3/3 바이트 불변** |
| M20 영수증 디렉터리 (21건) | **21/21 바이트 불변** |
| M19 영수증 디렉터리 (12건) | **12/12 바이트 불변** |
| `M7UiSkinProfile.cs` | 이 세션 편집 **0건** |
| M20이 넣은 행 | `FullBleedBacking("M20 work surface"…)` 등 **전부 존재** |

[OBSERVED] `reset` · `stash` · `revert` · `clean` · `checkout` · `commit` · `push` · `add`
**미실행**. 외부 공개 0건 · 에셋 구매/생성 0건 · 2D/3D/이미지/영상 생성 0건 ·
`runtimeEligible`/`runtimeApproved` 승격 0건 · 공유 진실 산출물(`decision-log.md` 등) 편집 0건 ·
`graphify`/`.mex` 실행 0건.

이 세션이 쓴 것의 **전체 목록**:

```
 M unity/Unknown/Assets/_Project/UI/T0Interface.cs                            (+3행, 추가만)
?? unity/Unknown/Assets/_Project/UI/M21ReadingBackingInterface.cs             (+ .meta)
?? unity/Unknown/Assets/_Project/Tests/PlayMode/T0ReadingBackingPlayModeTests.cs (+ .meta)
?? _workspace/current/presentation/t0-reading-band-m21.md
?? _workspace/current/systems/tech-verification/m21-r02-readability/
```

## 8. 동시 세션 충돌 — 이 영수증의 시간 경계 [중요]

[OBSERVED] **캡처를 마친 뒤, 다른 세션이 같은 작업 트리를 쓰고 있음을 확인했다.** 이 세션은
그 변경을 **되돌리지 않았고 편집하지도 않았다** (CLAUDE.md §8 · §10.1).

세션 시작 시 `git status` 74행 → 현재 **92행**. 이 세션 소유가 아닌 신규/변경 항목:

```
 M _workspace/current/intake/production-brief.md
 M _workspace/current/production/decision-log.md          ← 디렉터 소유 공유 진실
 M _workspace/current/production/task-manifest.md
 M _workspace/current/systems/data-schemas/tools.md
 M _workspace/current/systems/data/t0/tools.json
 M _workspace/current/systems/system-specs/hint-system.md
 M unity/Unknown/Assets/_Project/App/M7ReaderSession.cs
 M unity/Unknown/Assets/_Project/App/T0GameSession.cs      ← 힌트 오퍼 기능 +49행
 M unity/Unknown/Assets/_Project/Data/Tables/tools.json
 M unity/Unknown/Assets/_Project/Input/WatchInput.cs
 M unity/Unknown/Assets/_Project/Resources/M7UiSkin.asset  ← workSurfaceTint α 0.97 → 0.3
 M unity/Unknown/Assets/_Project/Tests/PlayMode/{M7ReaderPlayModeTests,T0PlayModeTests}.cs
?? _workspace/current/messages/20260913-aside-research-coordination.md
```

추가로, 이 세션이 편집하지 않은 **미추적** 파일 3건의 내용이 16:00:52~16:01:11에 바뀌었다:
`M20WorkSurfaceProfile.cs`, `T0WorkSurfacePlayModeTests.cs`, `presentation/t0-work-surface-m20.md`.
세 파일 모두 M20 게이트 표면(`runtimeApproved`·`diagnosticOverride`·`workSurface`·`NonSerialized`,
게이트 식, `--m20-ui-surface-diagnostic`)과 M20 테스트의 핵심 단언을 **여전히 보유**하며,
`M20WorkSurface.asset`의 `runtimeApproved`는 **여전히 0**이다.

### 8.1 시간 경계 — 무엇이 언제 측정되었나

| 시각 | 사건 |
|---|---|
| 15:45 | 보존 베이스라인 154건 해시 |
| 15:51 – 15:55 | RED · RED2 · GREEN |
| 15:56 – 15:57 | EditMode 54/54 · PlayMode 89(88 passed) |
| 15:57:24 | **빌드 완료** (`defe2da7…`) |
| 15:59 · 16:04 | **게이트 ON/OFF 네이티브 캡처 + 측정** |
| **15:58 – 16:01** | **다른 세션의 쓰기 (tools.json · tables-receipt.json · T0GameSession.cs · M7UiSkin.asset …)** |

**따라서 §0·§3·§4·§5·§6의 모든 수치는 15:45~15:59 시점의 트리에서 생산되었고, 현재 디스크 트리와
동일하지 않다.** 이것을 숨기지 않는다.

### 8.2 현재 트리에서는 스위트가 재현되지 않는다 — 원인은 M21이 아니다

[OBSERVED] 현재 트리에서 집중 테스트를 재실행하면 실패한다
(`green-reverify-concurrent-tree.xml`, 1 total / 0 passed / **1 failed**):

```
InvalidOperationException: V-4: receipt contract differs from producer receipt
  at Tide.Data.ReceiptVerifier.Verify (ReceiptVerifier.cs:22)
  at Tide.Data.T0CatalogAsset.Load (T0CatalogAsset.cs:26)
```

즉 **데이터 테이블 영수증 불일치**로 카탈로그 로드가 실패한다 — 다른 세션이 15:58에 편집 중인
`tools.json` / `tables-receipt.json` 때문이다. 세션을 부팅하는 모든 테스트가 여기서 죽는다.

**이것이 M21 때문이 아님을 대조군으로 증명했다**: **M21 코드가 전혀 관여하지 않는** M20 집중 테스트를
같은 트리에서 실행하면 **동일한 `V-4` 예외로 실패한다** (`m20-control-on-concurrent-tree.xml`,
1 total / 0 passed / 1 failed). 결함은 **트리 전역이며 외부 기원**이다.

[OBSERVED] 이 세션은 그들의 파일을 **고치지 않았다**. 내 테스트를 통과시키려고 다른 세션의 미커밋
작업을 수정하는 것은 보존 요구 위반이다. 디스크의 M21 코드는 **온전하다**
(`ApplyM21ReadingBacking(content);` 존재, `M21ReadingBackingInterface.cs` sha256 `3efac742…` 불변).

### 8.3 후속 필요 작업 (이 세션이 결정하지 않는다)

1. **디렉터/사용자 판정 필요**: 어느 세션이 마감을 맡는지. CLAUDE.md §10.1은 세션 시작 이후 예상 밖
   레인 파일이 생기면 **멈추고 사용자에게 묻도록** 규정한다 — 이 영수증이 그 보고다.
2. 데이터 테이블 영수증(`tables-receipt.json`)을 **소유 세션이** 재생성한 뒤
   (`systems/pipeline/emit-tables.mjs` → `validate-campaign.mjs --t0`) EditMode·PlayMode 전체를
   **한 번 더** 돌려야 한다. M21의 15:56 수치는 그 재실행으로 갱신되어야 한다.
3. `M7UiSkin.asset`의 `workSurfaceTint` α 0.97 → 0.3 변경은 **게이트 OFF 작업면 룩**을 바꾼다.
   §6.5의 바이트 동일 A/B는 그 변경 **이전** 빌드에서 얻은 것이다. 소유 세션의 변경이 확정되면
   게이트 OFF 베이스라인 캡처를 다시 떠야 한다. **M21의 띠 색은 `paper` RGB만 읽으므로 영향받지
   않는다** (`paper`는 불변).
4. 선행 미커밋 산출물의 소유가 정리된 후 `graphify update .` · mex 동기화 · `zg index --rebuild`.

## 9. 그래프/메모리 신선도 한계 (G8 주장 없음) — [UNGRAPHED]

[OBSERVED] `graphify update .` 와 `mex` 계열(`mex graph`, `mex check`, `mex sync`, `mex log`)을
**의도적으로 실행하지 않았다.** 이유: `graphify-out/`의 4개 파일과 `.mex/` 산출물이 이미 커밋되지
않은 선행 작업 상태이며, 이번 지시가 그 보존을 명시적으로 요구했다. 재생성은 그 미커밋 산출물을
덮어쓴다.

**정확한 결과**: 이번 변경(신규 소스 2 + 편집 1)은 코드 그래프와 프로젝트 메모리에 **반영되지
않았다**. `graphify-out/graph.json`·`GRAPH_REPORT.md`와 `.mex/`는 M21 이전 상태를 가리킨다.
CLAUDE.md §5 시스템 레인 순서 중 편집 이후 단계는 **미이행**이며 **`[UNGRAPHED]`** 로 표시한다.

- **G8을 PASS로 주장하지 않는다.**
- 이 머신의 `mex`는 TeX Live이고 `scripts/mex-agent-bin.sh`가 **부재**하여 신원 확인이 불가능하다
  (session-start: `mex: skipped`). memory_sync 영수증은 **`skipped`** — 도구 부재 + 보존 요구의 중첩.
- `zg` 인덱스 재생성도 하지 않았다. 이번 탐색은 `grep`/`read`/`git` **읽기 전용**으로 수행했다.

## 10. 한계 — 주장하지 않는 것

1. **승격이 아니다.** `provenance.json` `runtimeEligible: false` · `promoted_by: null`,
   `M20WorkSurface.asset` `runtimeApproved: 0` **모두 불변**. 임포터에 승인 메뉴가 없다.
   **M20 r02는 이 정정에도 불구하고 미승격이다.** 가독성 결함 하나가 닫혔다는 것이 후보 승격 근거는
   아니다 — 승격은 §8의 동시 세션 판정, 미해결 ● ack 6건, 그리고 아래 2~6의 미측정 항목이 모두
   해소된 뒤 디렉터 감사로만 열린다.
2. **접근성 인증이 아니다.** §0의 픽셀 통계와 테스트의 색 산술은 **평면 sRGB 값**이며 글리프
   안티에일리어싱·한국어 획 두께·디스플레이 감마·관측 거리·시력을 반영하지 않는다.
   **WCAG 등 어떤 표준의 적합·부적합도 주장하지 않는다.** 실측 대비 검증은 NOT-MEASURED다.
3. **성능 주장 0건.** 진단 게이트 ON에서 문단당 `Image` 1개와 `LateUpdate` 미러 1개가 추가되지만
   프레임 시간·드로콜·VRAM·발열을 **측정하지 않았다**. 빌드는 +540 B뿐이다.
4. **사람 플레이테스트 n=0.** "더 읽기 좋다"는 **사람 판정이 없다**. 측정된 것은 픽셀 통계와
   컴포넌트 상태뿐이며, 자동 테스트 + 빌드 + 창 캡처 2장이 증거 전부다.
5. **진입 후 네이티브 캡처 0장.** §6.7 참조. 합성 입력 0회, TCC 우회 0회. 타이틀 1종만 관측.
6. **최종/출시 아트가 아니다.** 진단 게이트 뒤의 정정이며 아트 디렉션 확정도, 비주얼 레인 산출물의
   대체도 아니다. 9-슬라이스·셰이더·프레임 텍스처는 범위 밖이다.
7. **r02의 문자 부재는 여전히 육안 검수다.** 자동 OCR·주기성 분석 미실시(M20 한계 상속).
8. **r01 판정 0건.** 미승인·미통합 유지.
9. **G1–G8 이동 0건.** G4·G5 `NOT-MEASURED` 유지, G8 주장 없음(§9). 연출 계약은 `status: draft`이며
   미해결 ● ack(synopsis, vfx, animation, motion, modeling, qa)가 남아 게이트 투입 불가.
10. **RFC 블록 미작성.** RFC는 디렉터 소유 공유 진실 파일 `production/decision-log.md`에 기록되며
    이번 지시가 그 편집을 금지했다. CLAUDE.md §11에 따라 가상의 ack·회의를 만들지 않았다.
    **M21 마일스톤 라벨 소유권과 §8 동시 세션 마감 배정은 디렉터/사용자 판정 사항**이다.
11. **현재 트리 재현 불가** — §8.2. 이 영수증의 수치는 15:45~15:59 트리 상태에 대한 것이다.
12. 커밋/푸시 **미실행**. 사용자가 수행한다.
