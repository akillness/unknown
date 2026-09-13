---
updated: 2026-09-13
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-qa
---

# M16 런타임 리소스 반영·캡처·모니터링 — 검증

> **이 문서의 성격**: **자동화 에이전트가 조작한 네이티브 캡처**다.
> 사람 플레이테스트가 **아니다**. 성능 증명이 **아니다**. 물리 입력 증명이 **아니다**.
> G4–G7 인증이 **아니다**. "모든 리소스가 시각적으로 완벽하다"는 주장을 **하지 않는다**.

## 0. 결론 요약

[OBSERVED] 빌드는 성공했고 네이티브 플레이어는 격리 저장 루트로 정상 부팅했다(예외 0건).
**그러나 게임 내부로 진입하지 못했다.** macOS TCC 보조 접근(Accessibility)이 이 자동화
컨텍스트에 부여되지 않아 **합성 포인터·키 입력이 단 1건도 전달되지 않았다**.
타이틀 화면의 `당직 시작 / 계속`을 누를 수 없으므로 요구된 (1) 진입 후 M7 허브,
(2) M7 판독기 스테이지, (3) M8 검토 노트 **모두 캡처하지 못했다**.

| 요구 | 결과 | 비고 |
|---|---|---|
| 디스크·Unity 게이트 | **통과** | 11.95–12.0 GiB 여유(>10), Unity 에디터 비실행·락파일 없음 |
| 현재 소스 빌드 | **성공** | `T0_MAC_BUILD Succeeded bytes=403838113` |
| 격리 저장 루트 실행 | **성공** | `/tmp/unknown-m16-capture-1789264187`, 종료 후 **빈 디렉터리** |
| 명시적 게임 창 캡처 | **성공** | 데스크톱 전체 캡처 미사용 |
| (1) 진입 후 M7 허브+UI 스킨 | **미달성** | 진입 전 타이틀 상태에서만 관측(§3) |
| (2) M7 판독기 스테이지 | **미달성** | 입력 차단(§4) |
| (3) M8 검토 노트 | **미달성** | 상태 변경 없이 도달 *가능*했으나 입력 차단(§4) |
| Player.log 예외 | **0건** | 부팅 마커 4개 정상 |
| M14/M15 보존 | **완전 보존** | 56개 항목 바이트 동일(§6) |

## 1. 사전 측정 (빌드 게이트)

[OBSERVED] 빌드 전 필수 측정을 먼저 수행했다.

| 항목 | 측정값 | 판정 |
|---|---|---|
| 여유 디스크 | **12.0 GiB** (`/System/Volumes/Data`, 12578824 KiB) | 10 GiB 초과 → 빌드 허용 |
| Unity 에디터 프로세스 | **없음** (`Unity.app`/`Unity Hub`/`UnityShaderCompiler` 0건) | 빌드 허용 |
| `Temp/UnityLockfile` | **없음** | 빌드 허용 |

[OBSERVED] 최초 `ps` 검색에 걸린 항목은 VS Code의 Roslyn 언어 서버와 `UnityCodeModel.dll`로,
Unity 에디터가 아니다. 빌드 후 여유 디스크는 **11.95 GiB**다.

## 2. 빌드

[OBSERVED] 승인된 런타임 프로필 4종은 실행 시점에 모두 `runtimeApproved: 1`이었다 —
`M7Hub` · `M7UiSkin` · `M7ReaderStage` · `M8ReviewNotes`.
따라서 **진단 플래그를 하나도 넘기지 않았다**. 모든 게이트는 승인 값만으로 통과했다.
이는 M9 네이티브 플레이테스트가 `--m8-review-notes-diagnostic`으로 강제 점등해야 했던 것과
다른 조건이다(당시 `M8ReviewNotes.runtimeApproved: 0`).

| 항목 | 값 |
|---|---|
| Unity | `6000.5.6f1` |
| 메서드 | `Tide.EditorTools.T0ProjectBuilder.BuildMac` |
| 옵션 | `BuildOptions.Development` — **개발 빌드**(릴리스 아님, 우하단 `Development Build` 워터마크) |
| 결과 | `T0_MAC_BUILD Succeeded bytes=403838113` (`build-mac.log:6088`) |
| 디스크 실측 총량 | `403838113` bytes (315 파일) — 보고값과 **정확히 일치** |
| 메인 바이너리 | `Contents/MacOS/Unknown T0`, 67916 bytes |
| 바이너리 sha256 | `047771b8d1c579ef4e1a5a690b3a0240c641613392a8192527127513faa41562` |
| `globalgamemanagers` sha256 | `327c2129f21440587b37cfe569937faeb180296377e62ebd30d6b8c70f97a60f` |
| 번들 매니페스트 sha256 | `fdd59572f324d1e1dba33fee2f50a597cfedde3317e760d6ffcd89da513d97f1` (경로+크기 정렬 해시) |
| 번들 ID | `com.TideRegistry.Unknown-T0` |

[OBSERVED] `BuildMac`은 `Prepare()`를 호출하지 않는다(`T0ProjectBuilder.cs:71`). 씬·에셋
재생성 경로를 타지 않으므로 커밋된 자산을 건드리지 않는다. `caffeinate -dimsu`를 빌드에 부착했다.
`Builds/`·`Library/`는 `unity/Unknown/.gitignore`로 무시되어 작업 트리를 오염시키지 않는다.

## 3. 실행과 실제 관측된 화면

[OBSERVED] 실행 인자와 창:

```
open -n Unknown.app --args --t0-save-dir /tmp/unknown-m16-capture-1789264187 \
     -screen-width 1280 -screen-height 800 -screen-fullscreen 0
```

| 항목 | 값 |
|---|---|
| 저장 루트 | `/tmp/unknown-m16-capture-1789264187` — 종료 후 **파일 0개** |
| 창 | pid 54206 / window 5203 / 좌표 (957,50) / **640×432 pt = 1280×864 px @2×** |
| 캡처 경로 | `orca computer get-app-state --app com.TideRegistry.Unknown-T0 --window-index 0` |
| 캡처 방식 | **명시적 단일 창 캡처**. 데스크톱 전체 캡처 **미사용** |
| 캡처 해상도 | 1088×734 px (scale 1.7) |

[OBSERVED] **관측된 시각 상태는 1종뿐이다** — 진입 전 타이틀 화면(`started == false`).

| 샷 | sha256 | 관측 내용 |
|---|---|---|
| `shots/00-title-focused.png` | `6c43b62d…9004a` | 헤더 `조수기록국 · 당직 인수`, 부제 `21:00 · 시작 전 설정`, 액션 `당직 시작 / 계속`·`접근성 · 설정`, 하단 조작 범례, 우하단 `Development Build` |
| `shots/01-title-stability.png` | `6c43b62d…9004a` | **동일 해시**. 모든 입력 시도 이후 재캡처 — 입력이 전달되지 않았고 상태가 진행되지 않았음을 증명 |

[OBSERVED] 두 샷의 sha256이 **바이트 단위로 동일**하다는 사실 자체가 "합성 입력 0건"의 직접 증거다.

### 3.1 진입 전 화면에서 실제로 보이는 M7 요소

[OBSERVED] 코드 경로상 두 M7 레인은 **`started` 이전에 이미 적용**된다:

- `BindM7Hub()`는 `Initialize()` 안에서 호출된다(`T0GameSession.cs:61`). 즉 부팅 직후
  허브 씬 루트에 `floorAndWall`·`workbench`·`plateShelf` 머티리얼이 적용된다
  (`M7HubSession.cs:18-23`).
- `ApplyM7UiSkin(s)`는 `Render()` 마지막에서 **무조건** 호출된다(`T0GameSession.cs:184`).
  `UiSkinEnabled`가 참이므로 `s.Skin`이 채워진다(`M7UiSession.cs:23-28`).

[OBSERVED] 따라서 샷에서 3D 뷰포트의 석재/콘크리트 질감 바닥·벽과 작업대, 그리고 우측 콘텐츠
패널의 넝마지(rag-paper) 질감 표면과 질감이 들어간 헤더/푸터 띠는 **M7 허브 셸 머티리얼과
M7 UI 스킨이 네이티브 플레이어에서 실제로 렌더된 결과**다.

[OBSERVED] **다만 이것은 요구 (1)을 충족하지 않는다.** 요구는 "게임 진입 후 M7 허브"이고,
진입 후에만 나타나는 시점 노드 목록(`작업대 전경`·`사물 서랍`·`판독기` …)과 하단 툴바
(`되돌림`·`다시`·`증거함`·`힌트`·`접근성·설정`·`닫기/뒤로`)는 **관측되지 않았다**
(`T0GameSession.cs:175-176`은 `started` 분기 안에 있다).

[INFERENCE] 이 샷만으로 M7 리소스 전체가 의도대로 보인다고 말할 수 없다. 판독기 스테이지·
검토 노트 카드 표면은 **한 번도 렌더되지 않았다**.

## 4. 차단 사유 — macOS TCC 보조 접근 거부

[OBSERVED] 타이틀에서 벗어나려면 `당직 시작 / 계속` 액션 활성화가 **반드시** 필요하다.
`started`는 `StartGame()`만 참으로 만들고, `StartGame()`은 `BeginOpeningOrStart`(시작 버튼)
또는 `FinishOpening()`에서만 호출된다(`T0GameSession.cs:80,173` · `T0OpeningSession.cs:23-37`).
플레이어가 소비하는 커맨드라인 인자 전체(`--t0-save-dir`, `--m5-direction-diagnostic`,
`--m7-hub/-ui/-reader-diagnostic`, `--m8-review-notes-diagnostic`, `--c1-*-diagnostic`)에
**자동 시작 경로는 없다**. 저장 파일을 미리 넣어도 `started`는 거짓으로 남는다.

[OBSERVED] 입력 경로 3종을 모두 시도했고 모두 거부됐다:

| 경로 | 결과 |
|---|---|
| `osascript` System Events `click at {1418,257}` | **execution error -25211** — `osascript에 보조 접근이 허용되지 않습니다` |
| `cliclick` | `Accessibility privileges not enabled` 경고 + `m:400,400` 후 커서 좌표 **1418,332 → 1418,332 불변** |
| `orca computer click` / `press-key` | `window_not_focused` — 단 같은 시점 `osascript`는 frontmost를 **`Unknown T0`**로 보고 |

[OBSERVED] `orca computer permissions --json`은 **`accessibility: not-granted`,
`screenshots: granted`**를 보고한다. Orca의 `window_not_focused`는 포커스 경쟁이 아니라
**헬퍼가 AX 없이는 포커스 상태를 읽지 못해 발생하는 권한 실패**다 — 앱을 `open -a`로
전면화하고 `osascript`로 frontmost를 확인한 직후 `--restore-window`와 함께 재시도해도 동일했다.

[OBSERVED] 다음은 **원인이 아니다**: 화면 잠금(잠금 키 없음), 창 가림, 앱 비전면화, 창 ID 해석 실패.
`screencapture -l`/`-x`도 동일 컨텍스트에서 `could not create image from window/display`로
실패하며, 이 때문에 캡처는 화면 기록 권한이 있는 **Orca 경로**로 수행했다.

[OBSERVED] **해제 방법**: 시스템 설정 → 개인정보 보호 및 보안 → 손쉬운 사용에서 제어 프로세스
(`Orca Computer Use` 헬퍼 및/또는 터미널 호스트)에 권한을 부여한 뒤 재실행하면 된다.
캡처 경로 자체는 이미 동작하므로 **입력 권한만 해제되면 남은 3개 상태를 그대로 이어서 촬영할 수 있다**.

### 4.1 M8 검토 노트 — 도달 가능성은 확인했으나 미촬영

[OBSERVED] 요구사항이 물은 "퍼즐 상태를 바꾸지 않고 도달 가능한가"에 대한 답은 **가능하다**이다.
`OpenReviewNotes()`는 `overlay="reviewNotes"` 설정과 `Render()`만 수행하며 `PuzzleCommand`를
전혀 제출하지 않는다(`ReviewNotesSession.cs:86-95`). 진입점인 `검토 노트 · 대조 메모` 액션은
`overlay`가 `evidence`/`hypothesis`일 때 추가된다(`T0GameSession.cs:180`).
**그럼에도 촬영하지 못한 이유는 오직 §4의 입력 차단** — 게임 진입 1회 + 활성화 2회가 선행돼야 한다.

## 5. 모니터링 — Player.log

[OBSERVED] `logs/Player.log` (6820 bytes, 종료 후 재수집) · `logs/Player-prev.log` 보존.

- 부팅 마커 4개 모두 정상: `T0_BOOT entry-start` → `ui-root-loaded` → `hub-loaded` → `session-initialized`.
- `session-initialized` 도달은 `Initialize()`의 `catch`가 발동하지 않았다는 뜻 —
  즉 카탈로그·표·인용 메타데이터 검증과 `BindM7Hub()`가 예외 없이 끝났다.
- **예외 0건**. 스캔 패턴 `Exception|NullReference|Error:|error CS|Unhandled|Assertion failed|StackOverflow`
  (정상 `Debug.Log` 스택 프레임 제외) 일치 **0줄**.
- `SIGTERM` 종료 후에도 로그 크기 불변(6820 bytes) — 종료 경로 예외 없음.

[OBSERVED] 로그의 스택 프레임에 보이는 `/private/tmp/unknown-c1-m4-publish-20260911/...` 경로는
**빌드 시점 디버그 심볼에 박힌 경로 문자열**이며 행 번호는 현재 소스와 일치한다. 실행 소스가
다르다는 뜻이 아니다.

## 6. 범위 준수 — M14/M15 보존

[OBSERVED] 실행 전 `git status --porcelain -uall` 56개 항목 전부의 sha256을
`baseline-worktree.json`에 기록하고, 종료 후 재검증했다(`worktree-verification.json`).

| 검사 | 결과 |
|---|---|
| HEAD | `52ad5510522eca6d5cfd8d743d85f28cb254635f` **불변** |
| 삭제된 항목 | **0** |
| 내용이 바뀐 항목 | **0** (56/56 바이트 동일) |
| 증거 디렉터리 **밖**에 추가된 항목 | **0** |
| M14/M15 관련 항목 | 22개 추적, **위반 0** |

[OBSERVED] 새로 생긴 8개 경로는 전부
`_workspace/current/systems/tech-verification/m16-runtime-capture/` 내부다.
`reset`·`stash`·`revert`·`clean`·`commit`·`push`를 실행하지 않았고, 승인/자산 생성/구매/
브라우저 퍼블리싱 변경도 없다. 빌드 산출물(`Builds/`)과 저장 루트(`/tmp/...`)는 추적 대상이 아니다.

## 7. 한계 (명시)

1. **자동화 에이전트 캡처**다. 사람 플레이테스트가 아니다.
2. **성능 증명이 아니다** — 프레임타임·메모리·발열을 측정하지 않았다.
3. **물리 입력 증명이 아니다** — 실제 키보드/마우스/컨트롤러 입력은 0건이며,
   합성 입력조차 전달되지 않았다. 입력 동등성은 여전히 PlayMode 테스트 몫이다.
4. **G4–G7 인증이 아니다.**
5. **모든 리소스가 시각적으로 완벽하다고 주장하지 않는다.** 실제로 판독기 스테이지와
   검토 노트 표면은 이번 실행에서 **한 번도 렌더되지 않았다**.
6. 관측된 시각 상태는 **1종(진입 전 타이틀)** 뿐이며, 샷 파일 2개는 동일 상태의 중복이다.
7. 개발 빌드(`BuildOptions.Development`) 기준 관측이다.

## 8. 산출물

```
m16-runtime-capture/
├── verification.md              # 이 문서
├── summary.json                 # 기계 판독 요약
├── build-mac.log                # Unity 배치 빌드 로그 (T0_MAC_BUILD Succeeded)
├── baseline-worktree.json       # 실행 전 56개 항목 sha256
├── worktree-verification.json   # 실행 후 보존 검증 결과
├── shots/
│   ├── 00-title-focused.png     # 진입 전 타이틀 (M7 허브 셸 + UI 스킨 렌더)
│   └── 01-title-stability.png   # 동일 해시 재캡처 (입력 미전달 증명)
├── logs/
│   ├── Player.log               # 예외 0건
│   └── Player-prev.log
└── tools/
    ├── listwindows.swift        # CGWindowList 기반 창 열거 (명시적 창 캡처용)
    └── listwindows              # 컴파일 산출물
```
