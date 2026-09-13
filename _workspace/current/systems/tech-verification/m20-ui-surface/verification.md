---
updated: 2026-09-13
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
---

# M20 — GTI r02 아카이브 작업면 후보의 게이트된 진단 런타임 통합

> **이 문서가 무엇인가**: 부모 세션이 생성한 GTI 후보
> `m20-archival-work-surface-r02`를 T0 작업면 패널의 **단일 비-타일 전면 배경**으로,
> **전용 진단 게이트 뒤에서만** 적용하고 실제 네이티브 플레이어에서 촬영한 영수증이다.
> 승격이 **아니고**, 최종 아트가 **아니며**, 성능·접근성 인증·사람 플레이테스트 주장도 **아니다**.
> 어떤 게이트도 올리지 않는다.

같은 사이클 안의 제자리 갱신 (RFC-Q2: `cycle` 불변 → `supersedes: null`, 아카이브 의무 없음).

연출 계약(소유 산출물): `_workspace/current/presentation/t0-work-surface-m20.md` (`status: draft`)

## 0. 결론 — 실제 캡처 결과

[OBSERVED] **게이트 ON 네이티브 캡처는 성공했고, r02는 의도한 구조 그대로 렌더되었다.**
작업면 패널이 M7 넝마지(밝은 종이) 대신 **r02의 어두운 청록 에나멜 단일 전면 배경**이 되었고,
버튼은 **M19 평면 계기 판을 그대로** 유지했다(포커스 판 = 황동/오커 + 잉크 Bold 라벨, 비포커스 판 =
잉크 판 + 종이색 라벨 + 황동 눈금). r02는 **어떤 버튼에도 붙지 않았다**.

**그리고 그 캡처가 후보의 결함을 하나 드러냈다.** 커밋된 본문 텍스트는 어두운 `ink` 색인데
r02 표면도 어둡다. M19 캡처에서 작업면 상단에 읽히던 본문 한 줄
(`21:00. 인수 각서와 이관 목록을 살핀 뒤, 당직실의 기록을 대조하세요.`)이 게이트 ON 캡처에서는
**시각적으로 사라졌다**. 두 캡처의 **같은 픽셀 영역**을 측정한 값:

| 측정(같은 창 기하, 1088×734px) | M19 베이스라인 | M20 게이트 ON |
|---|---|---|
| 본문 띠 상대휘도 평균 | 0.5161 | **0.0255** |
| 본문 띠 표준편차 (글리프 존재 신호) | 0.1360 | **0.0036** |
| 글리프 코어(하위 2%) 휘도 | 0.1354 | 0.0197 |
| 지면(상위 80%) 휘도 | 0.6047 | 0.0273 |
| 글리프 : 지면 비 | **3.53 : 1** | **1.11 : 1** |
| 버튼 아래 빈 작업면 평균 휘도 | 0.5833 | 0.0253 |

[OBSERVED] 게이트 ON에서 **본문 띠 평균(0.0255)과 빈 배경 평균(0.0253)이 사실상 같다** — 즉 그
영역은 배경과 구별되지 않는다. **이것이 r02가 `runtimeApproved:false` · `runtimeEligible:false`로
남아야 하는 이유이며, 진단 게이트가 존재하는 목적 그 자체다.** M20은 텍스트 색을 바꿀 권한이 없어
(계약 N1·N12) **고치지 않았다**. 이 수치는 **평면 픽셀 산술값이며 접근성 인증이 아니다**(§8.2).

| 단계 | 결과 | 영수증 |
|---|---|---|
| 집중 RED (1차) | 1 case, 0 passed, **1 failed** — M20 배킹 부재 단언 | `red.xml`, `red.log` |
| 집중 RED (2차) | 1 case, 0 passed, **1 failed** — 내 기대값 오류 (§3.3) | `red2.xml`, `red2.log` |
| 집중 GREEN | 1 case, **1 passed**, 0 failed | `green.xml`, `green.log` |
| EditMode 전체 | 54 total, **54 passed**, 0 failed — case 집합 M19와 **동일** | `editmode.xml` |
| PlayMode 전체 | 88 total, **87 passed**, 0 failed, 1 skipped(기존) — **+1 = 이번 테스트뿐** | `playmode.xml` |
| macOS 플레이어 빌드 | `T0_MAC_BUILD Succeeded bytes=408563585`, 디스크 실측 일치 | `build-mac.log:6128` |
| 네이티브 캡처 (게이트 ON) | **pid 87336 / window 5311 명시 지정** 단일 창 1장 | `shots/00-m20-gate-on-pid87336-window5311.png` |
| 네이티브 캡처 (같은 빌드, 게이트 OFF) | **pid 93504 / window 5324**, M19 영수증 이미지와 **sha256 동일** | `shots/01-m20-gate-off-same-build-pid93504-window5324.png` |
| Player.log ×2 | 예외 · 에러 **0건** | `logs/player-m20-gate-{on,off}-*.log` |
| 베이스라인 보존 | 119건 중 **117 바이트 동일**, 변경 2건은 M20 의도 편집, HEAD 불변 | §7 |

## 1. 에셋: r02 입력 실측과 임포트

### 1.1 입력 (부모 소유 · 읽기 전용)

[OBSERVED] 이번 세션은 `gti` / `gen-2d.sh` / Higgsfield / Blender를 **한 번도 실행하지 않았다**.
r02는 부모 세션 산출물이며 **알려진 상위 소유 추가물**로 취급했다(외부 충돌 아님).

| 항목 | 값 |
|---|---|
| 경로 | `assets/generated/2d/ui/m20-archival-work-surface-r02.png` |
| 바이트 | 2,220,940 |
| **차원 (PNG IHDR 직접 파싱)** | **1672 × 941** |
| **sha256** | **`cc52db8a8950aa7fd0915e4cae92b04a5892f9d9130863d89b2b52cbf83d75a7`** |
| provenance `output_sha256` | 동일 — **대조 통과** |
| provenance `runtimeEligible` | **false** (임포터가 이 값을 강제 검사한다) |
| provenance `promoted_by` | `null` |
| 요청 → 실제 크기 | `2048x1152` → `1672x941` (백엔드가 요청 무시) |
| 프롬프트 sha256 | `5cacd03e3ca71483fbcb4a8242ca4c22951518d6b175cf9c95968535955cf356` |
| 산출물 성격 | **단일 비-타일 작업면 후보 1장** |
| 문자·숫자·로고·아이콘 | **육안 검수에서 없음** (프롬프트 NEGATIVE가 명시 배제) — 자동 OCR·주기성 분석은 **미실시** [한계] |
| `assets/generated/**` 편집 | **0건** (§7에서 바이트 동일 확인) |
| r01 | **임포트·참조·연결 0건** — 미승인·미통합 유지 |

### 1.2 임포트 (가장 작은 프로젝트 관례 경로)

[OBSERVED] `Editor/M7UiSkinProjectBuilder.cs`의 기존 관례를 그대로 따르되 **두 값만** 의도적으로 다르다.

| 항목 | 값 | M7과 비교 |
|---|---|---|
| 명령 | `-executeMethod Tide.EditorTools.M20WorkSurfaceProjectBuilder.ImportWorkSurface` | 동형 |
| 로그 | `M20_WORK_SURFACE_IMPORTED 1672x941 wrap=Clamp runtimeApproved=false` (`import.log:548`) | 동형 |
| Unity 경로 | `Assets/_Project/Art/Candidates/m20-ui-r02/UI_M20_ArchivalWorkSurface.png` | 동형(후보 폴더) |
| 복사본 sha256 | `cc52db8a…` — **원본과 바이트 동일** | 동형 |
| `textureType` | `Default` | M7은 `Sprite` (M20은 `RawImage` 전용이므로 스프라이트 메타 불필요) |
| **`wrapMode`** | **`Clamp`** | **M7은 `Repeat`** — 반복을 엔진 수준에서 불가능하게 만든다 (계약 T4) |
| **`maxTextureSize`** | **2048** | M7은 1024 — 1672폭을 단일 구도로 보존 (계약 T1) |
| `mipmapEnabled` / `npotScale` / `sRGB` | false / None / true | 동형 |
| 프로필 에셋 | `Assets/_Project/Resources/M20WorkSurface.asset` · `runtimeApproved: 0` | 동형(별도 프로필) |
| 승인 메뉴 | **builder에 존재하지 않는다** | M7은 `Tools/M7/Approve UI skin` 보유 |
| 감사 JSON | `unity/Unknown/Builds/m20-work-surface-import-audit.json` (`Builds/`는 .gitignore) | 동형 |

[OBSERVED] 임포터는 복사 **전에** ① provenance 항목 존재 ② `output_sha256` 일치
③ `runtimeEligible == false` 를 검사하고 하나라도 어긋나면 **예외로 중단**한다.
승격된 소스를 임포트하는 경로가 코드에 **없다**.

## 2. 변경 (정확한 경로와 해시)

### 2.1 신규 파일 (전부 M20 소유)

| 경로 | 역할 | bytes | sha256 |
|---|---|---|---|
| `Presentation/M20WorkSurfaceProfile.cs` | 순수 데이터 프로필 | 1,737 | `68730969873092e5c9bbdd8a7ddc084c28ef5f3b697d0decca76b4a240cfb599` |
| `Editor/M20WorkSurfaceProjectBuilder.cs` | 임포터 (승인 메뉴 없음) | 5,995 | `9c7b6bee086a5b669e9ad538fc3ff6245532c1eccf7662ea4181fd392676f22d` |
| `App/M20WorkSurfaceSession.cs` | 게이트 + `s.WorkSurface` 대입 1행 | 1,783 | `69eadd9ab31ae2dffec3266dc5dee0d5dc301d5951802539326422ad3fa7455e` |
| `Tests/PlayMode/T0WorkSurfacePlayModeTests.cs` | 집중 테스트 1 case | 16,254 | `12d9f6addca3ef84f9a574954c0084450bbe4c09ed2ff9c8883f63f5a4a8b27d` |
| `Resources/M20WorkSurface.asset` | 후보 프로필 (`runtimeApproved: 0`) | 597 | `746303366e6b4f592fbeb68e8c2430266de9e907abaec5155d787b3627c3a264` |
| `Art/Candidates/m20-ui-r02/UI_M20_ArchivalWorkSurface.png` | r02 후보 복사본 | 2,220,940 | `cc52db8a…` (원본과 동일) |
| `presentation/t0-work-surface-m20.md` | 연출+통합 계약 (`status: draft`) | 14,325 | `1253edb00f3db6619b256134fbe1f172404928f0f81a5d05d2824d7fc2e5cd40` |

`.meta` 4건은 Unity가 임포트 시 생성했다.

### 2.2 기존 파일 편집 — 정확히 2곳, 전부 가산적

| 경로 | M20 델타 | M20 이전 sha256 | M20 이후 sha256 |
|---|---|---|---|
| `UI/T0Interface.cs` | **+20 / −1** | `f4468449c6f76f0196c1de7a6e3cecf285621a11df32f07ae4f32751ba3c414b` | `d9a1120a1d9e07d595975f851699e3cc4536601d4d24f1a7d8d34ae9656af596` |
| `App/T0GameSession.cs` | **+1 / −1** | `a61290acc193df9bc79c6861b1dc9dded95b45c8cb4a9b2c06d68c1ce3da0c92` | `f3e26e9377c85af3c32042747902c37c668d80b4dea8e709fd525b1cd7cc8079` |

[OBSERVED] **삭제된 2행은 둘 다 HEAD 원본이며 M14~M19가 수정한 행이 아니다**, 그리고 그 내용은
보존되었다:

```
HEAD:103   SkinBacking("M7 paper",contentPanel,skin?.paperPanel,…,…);
M20 :113   else SkinBacking("M7 paper",contentPanel,skin?.paperPanel,…,…);   ← 호출 인자 완전 동일

HEAD:180   ApplyM7UiSkin(s);Interface.Render(s);ApplyStagePresentation();ApplySceneViewport();
M20 :186   ApplyM7UiSkin(s);ApplyM20WorkSurface(s);Interface.Render(s);ApplyStagePresentation();ApplySceneViewport();
```

[OBSERVED] M19가 `T0Interface.cs`에 넣은 **12개 행 전부**와 M14~M18이 `T0GameSession.cs`에 넣은
**4개 행 전부**가 문자열 단위로 **여전히 존재한다**(§7 기계 대조). M20 이전 `T0Interface.cs` 해시
`f4468449…`는 M19 영수증이 기록한 값과 **정확히 일치**했다 — 즉 M19 작업은 손상 없이 인수되었다.

### 2.3 정확히 무엇이 달라졌는가

| 요소 | 게이트 OFF (= 커밋 기본값) | 게이트 ON |
|---|---|---|
| 작업면 배킹 | `M7 paper` **타일** `RawImage` 1개, `uvRect=(0,0,2.5,tilesDown)` | `M20 work surface` **비-타일** `RawImage` 1개, `uvRect=(0,0,1,1)` |
| 배킹 개수 | 1 | **1** (겹쳐 쌓지 않고 **대체**) |
| 작업면 자식 수 · `Viewport` index | n · 1 | **동일 n · 동일 1** |
| 배킹 raycast | false | **false** |
| 배킹 형제 index | 0 | **0** (텍스트·버튼이 전부 그 위) |
| 버튼 | M19 판 (`Bevel`+`Rule`+반전) | **동일. 버튼 하위 `RawImage` 0개** |
| 액션 순서 · 라벨 · 활성 · 포커스 수 | 커밋값 | **전부 동일** |
| 헤더·툴바 `M7 frame` | 각 1개 (타일) | **각 1개, 무변경** |
| 본문/라벨/보조 크기·굵기 | 20 / 18 Bold / 16 | **전부 동일** |

`skin==null`(스킨 게이트 OFF) 경로도 그대로다: M20 게이트가 닫히면 그 라인은 **HEAD 원본 호출**이다.

## 3. TDD 증거

러너: `Unity 6000.5.6f1 -batchmode -nographics -runTests -testPlatform PlayMode`,
필터 `Tide.Tests.T0WorkSurfacePlayModeTests`.

### 3.1 왜 완료된 C1 순찰 픽스처인가

**실제 도달 가능하고, 라이브 버튼과 M19 텍스트 3역할이 한 번에 렌더되는 유일한 화면**이다.
기존 C1/M5/M8/M18/M19 스위트가 쓰는 커밋 픽스처
`_Project/Tests/Fixtures/C1PatrolCompletedV2.json`을 **양쪽 절반에 각각 새 임시 저장 루트로 복사**해
부팅했다 — 두 세션이 서로의 쓰기를 볼 수 없다.
**목(mock) 0건 · 수기 `GameScreen` 0건 · 소스텍스트 단언 0건.** 모든 단언은 실제 Unity 컴포넌트
상태(`RawImage.texture`/`uvRect`, `Graphic.raycastTarget`, `RectTransform` 앵커·`rect`·형제 index,
`Image.color`, `Text.fontSize`/`fontStyle`, `Selectable.colors`, `GameObject.activeSelf`)를 읽는다.

### 3.2 게이트를 `runtimeApproved`로 흔들지 않았다 (중요)

[OBSERVED] 기존 `M7UiSkinPlayModeTests`는 `profile.runtimeApproved`를 임시로 **반전**시켜
게이트 ON을 시험한다. M20은 그 방식을 **쓰지 않았다** — 승인 플래그를 메모리에서라도 뒤집는 것은
승격 형태의 조작이기 때문이다. 대신 프로필에 `[NonSerialized] diagnosticOverride` 를 두어
**디스크에 승인 상태가 남을 수 없게** 하고 테스트는 그것만 조작한다.
`[UnityTearDown]`은 값을 복원한 뒤 **`runtimeApproved`가 변하지 않았음을 단언**한다.
에셋 파일의 `runtimeApproved: 0`은 임포트 시점 이후 한 번도 바뀌지 않았다(§7).

### 3.3 RED은 설정 오류가 아니라 부재 단언에서 실패했다

**1차 RED** (`red.xml`, 프로덕션 코드 작성 **전**):

```
gate on: the Work Surface must carry exactly one 'M20 work surface' backing so the
r02 candidate is applied once
  Expected: 1
  But was:  0
  at T0WorkSurfacePlayModeTests.cs:207
```

[OBSERVED] 이 단언 **앞의 모든 단언이 통과**했다 — 프로필·텍스처 로드, `runtimeApproved==false`,
게이트 OFF에서 실제 완료 C1 화면 도달(`PatrolActive`/`PatrolComplete`), 커밋 액션 순서 일치,
M20 배킹 0개, **타일 M7 배킹 정확히 1개(`uvRect.width > 1`)**, 헤더·툴바 프레임 각 1개,
M19 판 계약 전부(모든 버튼의 `Bevel`/`Rule`·`Rule.activeSelf==interactable`·
`selectedColor==white`·`fadeDuration==0`·포커스 반전·**버튼 내 `RawImage` 0개**·3단 위계),
그리고 게이트 ON 부팅 성공까지. 즉 **"도달하지 못해서"가 아니라 배킹이 없어서** 실패했다.

**2차 RED** (`red2.xml`, 프로덕션 코드 작성 **후**): 구현이 아니라 **내 기대값이 틀렸다**.

```
gate on: the committed Viewport is pushed one index, not reordered
  Expected: 2   But was: 1
```

[OBSERVED] 이때 M20 배킹 단언 **전부가 통과**했다(정확히 1개 · 텍스처 동일성 · 1672×941 ·
`wrapMode==Clamp` · `uvRect==(0,0,1,1)` · 전면 앵커 · 형제 index 0 · 비-raycast ·
`M7 paper` 0개 · 판 색 불변). 실패한 것은 "Viewport가 한 칸 밀린다"는 내 가정이다.
**이 저장소의 `M7UiSkin.asset`은 `runtimeApproved: 1`이므로 게이트 OFF 베이스라인에 이미 배킹이
하나 있다.** M20은 배킹을 **추가**하지 않고 **대체**하므로 자식 수와 index는 **변하지 않는다** —
index가 밀렸다면 오히려 r02가 M7 위에 **쌓였다**는 뜻이고 그것은 계약 T5 위반이다.

- 따라서 단언을 **더 엄격하게** 고쳤다: `자식 수 동일` **+** `Viewport index 동일` **+** `rect 동일`.
  행동 검사를 **제거하거나 완화한 것이 아니다.**
- **GREEN 이후에 테스트를 고쳐 실패를 숨긴 것이 아니다** — 이 시점에 테스트는 한 번도 통과하지
  않았다. 연출 계약 §3 H3 · §7.4도 관측된 대체 의미론으로 **정정**했다.

**GREEN** (`green.xml`): 1 case, **1 passed**, 0 failed, 0.518s.

### 3.4 테스트가 단언하는 플레이어 관측 사실

| 항목 | 단언 |
|---|---|
| 게이트 OFF 보존 | 작업면에 M20 자식 **0개**, 타일 `M7 paper` **1개**(`uvRect.width>1`), M19 판·반전·3단 위계 전부 성립 |
| 정확히 한 번 (T5) | 게이트 ON에서 `M20 work surface` **정확히 1개**, 같은 패널의 `M7 paper` **0개**, 자식 수 OFF와 동일 |
| 비-타일 (T3/T4) | `uvRect == (0,0,1,1)`, `texture.wrapMode == Clamp`, 텍스처 1672×941, `profile.workSurface`와 **참조 동일** |
| 전면 (T1) | `anchorMin=(0,0)` · `anchorMax=(1,1)` · `offsetMin=offsetMax=(0,0)` |
| 전경 보존 (H1) | 배킹 형제 index **0**, `Viewport` index·`rect` OFF와 동일, 작업면 루트 `Image.color` 불변 |
| 입력 보존 (H2/N6) | 배킹 `raycastTarget==false`, 버튼 루트 `raycastTarget==true`, `Focus("c1-review")` 성공 + `CurrentFocusId` 일치, `Activate("c1-review")` **실제로 화면을 바꿈**, 재렌더 후에도 배킹 **여전히 1개 · 비-raycast** |
| 버튼 불가침 (T2) | **모든** 버튼 하위 `RawImage` 개수 **0** |
| 무변이 (N2/N3/N4/N7) | 액션 ID 순서 · 라벨 문자열 딕셔너리 · `interactable` · 포커스 가능 수 OFF와 동일, `SavePending` 거짓 |
| 헤더·툴바 (H5) | 양쪽 절반 모두 `M7 frame` 각 1개, 비-raycast |
| 승인 불변 | `[UnityTearDown]`이 `runtimeApproved` 불변을 단언 |

## 4. 전체 회귀

| 스위트 | 이번 실행 | M19 베이스라인 | 델타 |
|---|---|---|---|
| EditMode | 54 total, 54 passed, 0 failed, 0 skipped | 54 / 54 | **0** — case 집합 동일 |
| PlayMode | 88 total, 87 passed, 0 failed, 1 skipped | 87 total, 86 passed, 0 failed, 1 skipped | **+1** = 이번 테스트 |

[OBSERVED] M19의 `editmode.xml`·`playmode.xml`과 leaf case 집합을 기계 대조:

- 사라진 case: **0건** (양 스위트)
- 추가된 case: `T0WorkSurfacePlayModeTests.M20GateOffKeepsTheM19WorkSurfaceAndGateOnAppliesR02OnceAsANonTiledFullBleedBacking` **1건뿐**
- 상태가 바뀐 case: **0건**
- 유일한 skip은 `T0BootSceneTests.SerializedBootLoadsHubAndInitializesVisibleStartScreen`으로
  M14~M19 베이스라인에서도 skip이다. 기존 항목이며 이번에 생긴 것이 아니다.
- 인접 표면 통과: `M7UiSkinPlayModeTests`(게이트 OFF 리터럴 · ON 배킹 · 배율 1.5 무이동 · 전 버튼
  raycast), `T0ActionPlatePlayModeTests`(M19 판), `T0PlayModeTests`, `ScrollReachabilityTests`,
  `T0CaseThreadTests`, `C1SignaturePlayModeTests`, `C1InterviewPrepPlayModeTests`,
  `ReviewNotesPlayModeTests`, `M5DirectionPlayModeTests`, `M9CoreTests`.

## 5. 빌드 영수증

[OBSERVED] 표준 규칙대로 빌드 **직전에** 게이트를 측정했다:

| 항목 | 값 | 판정 |
|---|---|---|
| 빌드 전 여유 디스크 | **10.2186 GiB** (10,715,004 KiB, `/System/Volumes/Data`) | ≥ 10 GiB → **빌드 허용** |
| Unity Editor 프로세스 | **없음** (`Unity.app` / `Unity Hub` / `UnityShaderCompiler`) | 허용 |
| `unity/Unknown/Temp/UnityLockfile` | **부재** | 허용 |
| 빌드 후 여유 디스크 | 10.2261 GiB | — |

| 항목 | 값 |
|---|---|
| 메서드 | `Tide.EditorTools.T0ProjectBuilder.BuildMac` |
| 결과 | `T0_MAC_BUILD Succeeded bytes=408563585` (`build-mac.log:6128`), exit 0, `error CS` **0건** |
| 디스크 실측 | **408,563,585 바이트 / 315 파일 — 보고값과 정확히 일치** |
| 메인 바이너리 | `Unknown.app/Contents/MacOS/Unknown T0`, 67,916 바이트, mtime `Sep 13 15:28:05` |
| 메인 바이너리 sha256 | `e28544f6b63928c14f449994769fd18d97c0f604c1b568539e633c43d7a88561` |
| `globalgamemanagers` sha256 | `4539d206203e1f12a18921b620b994176cc811378665bab2d9f5f893607a2cbb` |
| M19 빌드와 비교 | 403,840,809 → **408,563,585** (+4,722,776 ≈ r02 텍스처), 메인 sha `aa0ea126…` → `e28544f6…` — **M20 소스가 실제로 반영된 새 빌드** |

[OBSERVED] `BuildMac`은 `Prepare()`를 호출하지 않으므로 씬·에셋을 재생성하지 않았고
`Builds/`·`Library/`·`Temp/`는 `unity/Unknown/.gitignore` 대상이라 작업 트리를 오염시키지 않았다.
`caffeinate -dimsu`를 빌드와 모든 테스트 런에 부착했다.
빌드 전 M19 빌드의 메인 바이너리 sha가 `aa0ea126…`(M19 영수증 값과 일치)임을 확인한 뒤 덮어썼다 —
`Builds/`는 무시 대상이므로 커밋 대상 증거가 아니다.

## 6. 네이티브 실행과 캡처 (명시적 출처)

### 6.1 PID 모호성을 실제로 해소했다

[OBSERVED] 실행 시점에 `Unknown T0` 창이 **4개** 존재했고 모두 같은 번들 ID
`com.TideRegistry.Unknown-T0`, **같은 기하(957,50 / 640×432)** 를 갖는다. 번들 ID 지정 캡처는
**틀린 프로세스를 잡는다**:

| PID | 시작 | 창 id | 실행 경로 / 저장 루트 | 소유 |
|---|---|---|---|---|
| 57356 | 09-13 10:50:46 | 5214 | `Builds/C1-M4-mac/…` | **다른 세션** |
| 85786 | 09-13 14:40:18 | 5263 | `Builds/T0-mac/…` `--t0-save-dir /tmp/unknown-m18-screen` | **다른 세션** (M18) |
| **87336** | **09-13 15:28:30** | **5311** | `Builds/T0-mac/…` **`--m20-ui-surface-diagnostic`** `--t0-save-dir /tmp/unknown-m20-capture-17484-1789280909` | **이번 M20 · 게이트 ON** |
| **93504** | **09-13 15:30:25** | **5324** | `Builds/T0-mac/…` (**플래그 없음**) `--t0-save-dir /tmp/unknown-m20-gateoff-17484-1789281025` | **이번 M20 · 게이트 OFF** |

[OBSERVED] 내 프로세스는 `ps -o command=`에서 **내가 만든 저장 루트 문자열**로 식별했고, 창은
`m16-runtime-capture/tools/listwindows`(CGWindowList, 읽기 전용 재사용)로 열거해 pid별 창 id를 얻었다.
번들 ID 지정 캡처는 **사용하지 않았다**.

### 6.2 캡처 (게이트 ON = 진단 후보 증거)

| 항목 | 값 |
|---|---|
| 실행 인자 | `--m20-ui-surface-diagnostic --t0-save-dir /tmp/unknown-m20-capture-17484-1789280909 -screen-width 1280 -screen-height 800 -screen-fullscreen 0` |
| 캡처 명령 | `orca computer get-app-state --app pid:87336 --window-id 5311 --restore-window --json` |
| 캡처 엔진 | `screenCaptureKit`, `screenshotStatus.state = captured`, `metadata.windowId = 5311` |
| 스냅샷이 보고한 앱 | `pid 87336`, `com.TideRegistry.Unknown-T0` — **요청한 PID와 일치** |
| 창 | id 5311 / (957,50) / 640×432 pt, `isOffscreen:false`, `isMinimized:false` |
| 캡처 방식 | **명시적 단일 창 캡처**. 데스크톱 전체 캡처 **미사용** |
| 해상도 | 1088×734 px (scale 1.7) |
| 파일 | `shots/00-m20-gate-on-pid87336-window5311.png`, 841,222 bytes |
| sha256 | `9cbe76c2829a7165b2adcda0384558f8b43d5d208db0872100dd4072724e5ffc` |
| 원시 스냅샷 | `shots/appstate-pid87336-window5311.json` |

### 6.3 캡처에서 실제로 보이는 것 — **진단 후보 증거 (promotion 아님)**

관측된 시각 상태는 **1종뿐이다** — 진입 전 타이틀 화면(`started == false`).

- **작업면(우측 패널)**: M19의 밝은 넝마지 대신 **어두운 청록/미드나이트 에나멜 단일 면**.
  외곽에 희미한 청동빛 마모 반점이 드문드문. **반복 격자·이음선·중앙 장식·테두리 프레임 없음** —
  1672×941 구도 한 장이 패널 전체를 덮는다.
- **`당직 시작 / 계속`** (포커스됨): 판 전체가 **황동/오커**, 라벨은 **어두운 잉크색 Bold**,
  좌측 눈금도 잉크색으로 반전 — **M19 계약 그대로**.
- **`접근성 · 설정`** (비포커스): 판은 **어두운 잉크**, 라벨은 **밝은 종이색 Bold**,
  좌측에 **황동 눈금** 가시.
- **두 버튼 어디에도 r02 텍스처가 없다** — 버튼은 평면 계기 판을 유지한다(T2 육안 확인).
- 헤더/푸터 청동 띠, 우측 사건 카드(잉크 판 + 종이색 본문), 좌측 3D 허브 뷰포트,
  우하단 `Development Build` 워터마크는 **모두 M19와 동일**.
- **[결함] 작업면 상단의 커밋 본문 한 줄이 시각적으로 사라졌다.** §0 표의 측정값 참조.

### 6.4 게이트 OFF A/B — 기본 커밋 런타임 무변경을 픽셀로 증명

[OBSERVED] **같은 M20 빌드**를 플래그 **없이** 다시 실행해 같은 방식으로 캡처했다.

| 항목 | 값 |
|---|---|
| 파일 | `shots/01-m20-gate-off-same-build-pid93504-window5324.png`, 897,798 bytes |
| sha256 | **`1514dd0c1a1158a843e21f57af18e0958a2af9e887023cc315540d3ef219f7d0`** |
| M19 영수증 캡처 sha256 | **`1514dd0c1a1158a843e21f57af18e0958a2af9e887023cc315540d3ef219f7d0`** |
| 판정 | **바이트 동일 — 게이트 OFF 화면은 M19와 픽셀 단위로 같다** |

즉 M20 코드가 들어간 빌드에서도 **플래그가 없으면 화면이 한 픽셀도 달라지지 않는다**.
이것이 "기본 커밋 런타임 무변경"의 가장 강한 형태의 증거다.
(같은 창 기하·같은 스케일·같은 캡처 엔진이므로 해시 비교가 유효하다.)

### 6.5 격리와 무변이

| 항목 | 결과 |
|---|---|
| 격리 저장 루트 (게이트 ON) | 실행 전 **0** 파일 → 종료 후 **0** 파일 (쓰기 0건). 디렉터리 제거 완료 |
| 격리 저장 루트 (게이트 OFF) | 동일 — **0 → 0**. 제거 완료 |
| 종료 대상 | **pid 87336 · 93504만** `kill`. 종료 후 **57356 · 85786은 여전히 실행 중**(다른 세션 보존 확인) |
| Player.log | `~/Library/Logs/TideRegistry/Unknown T0/Player.log`, mtime `15:28`(게이트 ON 실행) → `15:30`(게이트 OFF 실행). 직전 로그 `Player-prev.log` mtime `15:04` = M19 세션 — 로테이션으로 소유 확인 |
| Player.log 스캔 ×2 | `exception\|nullreference\|error\|assertion\|stack trace` 대소문자 무시 매치 **0건** (양쪽). 부팅 시퀀스 `T0_BOOT entry-start → ui-root-loaded → hub-loaded → session-initialized` 완주 |
| 사본 | `logs/player-m20-gate-on-pid87336.log` (6,757 B) · `logs/player-m20-gate-off-pid93504.log` (6,755 B) |

### 6.6 입력 한계 (기존 조건 유지)

[OBSERVED] M16이 기록한 **macOS TCC 거부**로 합성 입력이 전달되지 않는다. 이번에도 **우회를 시도하지
않았고** 승인 상태를 변경하지 않았다. 따라서 **타이틀 화면만 캡처했고 진입 후 화면은 캡처하지
못했다** — OS 권한 문제이며 게임 코드 결함이 아니고, 게임플레이 결함으로 재분류하지 않았다.
지시대로 **title-only 캡처로 충분**하게 처리했다.

**결과적으로 네이티브 캡처로 직접 확인된 M20 표면은 타이틀의 작업면 배경 1종**이다. 나머지 화면
(허브 · 판독기 · 회로 · 서명지 · 오버레이)의 작업면은 **PlayMode 렌더 계층 단언**으로 검증했고,
같은 `Render()` 경로를 공유한다는 점은 **구조에 의한 [INFERENCE]** 다. 네이티브로 주장하지 않는다.

## 7. 범위 준수 — M14~M19 보존

[OBSERVED] 첫 쓰기 전에 `git status --porcelain` **60행**을 기록하고, 그 경로들을 펼친
**파일 119건**을 sha256으로 베이스라인화했다(`baseline-before.json`).
테스트 4회 + 임포트 1회 + 빌드 1회 + 네이티브 실행 2회 이후 재검증(`baseline-after.json`):

| 검사 | 결과 |
|---|---|
| HEAD | `52ad5510522eca6d5cfd8d743d85f28cb254635f` **불변** |
| 바이트 동일 | **117 / 119** |
| 삭제/사라진 베이스라인 파일 | **0건** |
| 내용이 바뀐 파일 | **2건 — 전부 M20 의도 편집** (`T0Interface.cs`, `T0GameSession.cs`, §2.2) |
| `graphify-out/*` (4건) | **바이트 불변** — 재생성·수기 편집 모두 없음 |
| `.mex/*` (21항목) | **불변** — `mex` 계열 미실행 |
| `assets/generated/**` (5건: r01 PNG·프롬프트, r02 PNG·프롬프트, `provenance.json`) | **바이트 불변** |
| M14~M19 영수증 디렉터리 | 전부 존재, 편집 0건 |
| `M7UiSkinProfile.cs`, `Resources/M7UiSkin.asset` | **clean** — 승인 플래그·프로필 값 0건 변경 |
| M19가 넣은 12행 / M14~M18이 넣은 4행 | **전부 문자열 단위로 존재** (기계 대조) |
| 부모 세션 r01 | 임포트·참조 0건, 파일 불변 |

[OBSERVED] `reset` · `stash` · `revert` · `clean` · `checkout` · `commit` · `push` · `add`
**미실행**. 외부 공개 0건 · 에셋 구매/생성 0건 · 2D/3D/이미지/영상 생성 0건 ·
`runtimeEligible`/`runtimeApproved` 승격 0건 · 공유 진실 산출물(`decision-log.md` 등) 편집 0건.

M20이 추가한 작업 트리 항목(전부 M20 소유):

```
 M unity/Unknown/Assets/_Project/UI/T0Interface.cs                          (+20/−1)
 M unity/Unknown/Assets/_Project/App/T0GameSession.cs                       (+1/−1)
?? unity/Unknown/Assets/_Project/Presentation/M20WorkSurfaceProfile.cs      (+ .meta)
?? unity/Unknown/Assets/_Project/Editor/M20WorkSurfaceProjectBuilder.cs     (+ .meta)
?? unity/Unknown/Assets/_Project/App/M20WorkSurfaceSession.cs               (+ .meta)
?? unity/Unknown/Assets/_Project/Tests/PlayMode/T0WorkSurfacePlayModeTests.cs (+ .meta)
?? unity/Unknown/Assets/_Project/Resources/M20WorkSurface.asset             (+ .meta)
?? unity/Unknown/Assets/_Project/Art/Candidates/m20-ui-r02/                 (+ .meta)
?? _workspace/current/presentation/t0-work-surface-m20.md
?? _workspace/current/systems/tech-verification/m20-ui-surface/
```

### 7.1 임시 산출물 정리

[OBSERVED] `-testResults`에 상대 경로를 주면 Unity가 **projectPath 기준**으로 해석해
`unity/Unknown/_workspace/…/red.xml` 트리를 만들었다. 결과 파일을 올바른 증거 경로로 **이동**하고
그 빈 트리를 제거했으며, 이후 실행은 **절대 경로**를 사용했다. 작업 트리에 잔여물 **0건**
(§7의 `git status` 목록이 이를 확인한다).

## 8. 그래프/메모리 신선도 한계 (G8 주장 없음) — [UNGRAPHED]

[OBSERVED] `graphify update .` 와 `mex` 계열(`mex graph`, `mex check`, `mex sync`, `mex log`)을
**의도적으로 실행하지 않았다.** 이유: `graphify-out/`의 4개 파일과 `.mex/` 산출물이 이미 커밋되지
않은 선행 작업 상태이며, 이번 지시가 그 보존을 명시적으로 요구했다. 재생성은 그 미커밋 산출물을
덮어쓴다.

**정확한 결과**: 이번 변경(신규 4 소스 + 에셋 2 + 편집 2)은 코드 그래프와 프로젝트 메모리에
**반영되지 않았다**. `graphify-out/graph.json`·`GRAPH_REPORT.md`와 `.mex/`는 M20 이전 상태를
가리키며 **그래프/메모리 신선도는 갱신되지 않았다**. CLAUDE.md §5 시스템 레인 순서 중 편집 이후
단계(`graphify update .` → `mex graph` → `mex check` → `mex log`)는 **미이행**이다.
`game-systems-designer` 정의에 따라 **`[UNGRAPHED]`** 로 표시한다.

- **G8을 PASS로 주장하지 않는다.** 신선도·메모리 게이트는 이번 증분으로 움직이지 않았다.
- 이 머신의 `mex`는 TeX Live이고 `scripts/mex-agent-bin.sh`가 **존재하지 않아** 신원 확인이
  불가능하다 (session-start 출력: `mex: skipped — mex-agent not found or failed identity probe`).
  따라서 memory_sync 영수증은 **`skipped`** — 도구 부재 + 보존 요구의 **중첩** 결과다.
- `zg`는 세션 시작 시 `97% · 57 added · 6 modified` 로 **이미 갱신 대기** 상태였고, 인덱스 재생성은
  하지 않았다. 이번 탐색은 `grep`/`read`/`git` 읽기 전용으로 수행했다.
- 후속 작업: 선행 미커밋 산출물의 소유 세션이 정리된 후 `graphify update .` · mex 동기화 ·
  `zg index --rebuild` 를 한 번에 수행해야 한다.

## 9. 한계 — 주장하지 않는 것

1. **승격이 아니다.** `provenance.json`의 `runtimeEligible: false` · `promoted_by: null`,
   `M20WorkSurface.asset`의 `runtimeApproved: 0` **모두 불변**. 임포터에 승인 메뉴가 **없다**.
   기본 커밋 런타임은 §6.4에서 픽셀 동일로 확인했다.
2. **[후보 결함 · 가장 중요] 본문 가독성이 깨진다.** §0 측정: 게이트 ON에서 작업면 본문 띠의
   글리프:지면 비가 **3.53:1 → 1.11:1**, 표준편차 0.1360 → 0.0036. 커밋 본문색은 어두운 `ink`이고
   r02 표면도 어두우므로 **작업면에 직접 얹히는 본문·상태·보조 텍스트는 읽을 수 없게 된다**.
   M20은 텍스트 색·문구를 바꿀 권한이 없어(계약 N1·N12) **고치지 않았다**. r02를 실제로 쓰려면
   (a) 본문 계열 색조를 밝은 쪽으로 뒤집거나 (b) 텍스트 뒤에 별도 content-safe 판을 깔거나
   (c) r02를 밝은 변형으로 재생성해야 한다 — **모두 이번 범위 밖이며 별도 연출 판정이 필요하다.**
3. **이 픽셀 수치는 접근성 인증이 아니다.** 캡처 PNG의 평면 휘도 산술값이며 글리프
   안티에일리어싱·한국어 획 두께·실제 디스플레이 감마·관측 거리를 반영하지 않는다.
   **WCAG 등 어떤 표준의 적합·부적합도 주장하지 않는다.** 실측 대비 검증은 NOT-MEASURED다.
4. **최종/출시 아트가 아니다.** 진단 게이트 뒤의 후보 1장이며 아트 디렉션 확정도, 비주얼 레인
   산출물의 대체도 아니다. 9-슬라이스·셰이더·프레임 텍스처 작업은 범위 밖이다.
5. **성능 주장 0건.** 1672×941 텍스처 1장이 추가되고 빌드가 +4.7 MB 커졌으나 VRAM · 드로콜 ·
   프레임 시간 · 발열을 **측정하지 않았다**. 타일 배킹 1개를 비-타일 배킹 1개로 **교체**하므로
   드로콜 수는 동일할 것으로 **추정**된다 [INFERENCE].
6. **타일 가능성(seamlessness) 판정이 아니다.** r02는 비-타일 용도로만 쓰이며 이음선 검사를
   하지 않았다. 반복은 `wrapMode=Clamp` + `uvRect=(0,0,1,1)`로 **차단**되어 있다.
7. **r02의 문자 부재는 육안 검수다.** 자동 OCR·주기성 스펙트럼 분석 **미실시**.
8. **사람 플레이테스트 n=0.** 자동 테스트 + 빌드 + 창 캡처 2장뿐이다. "더 좋아 보인다"는 판단은
   **주관적이며 측정하지 않았다**.
9. **진입 후 네이티브 캡처 0장.** §6.6 참조. 합성 입력 0회, TCC 우회 0회.
10. **r01 판정 0건.** `m19-interview-control-surface-r01`은 미승인·미통합으로 남는다.
11. **G1–G8 이동 0건.** G4·G5 `NOT-MEASURED` 유지, G8 주장 없음(§8). 연출 계약은 `status: draft`이며
    미해결 ● ack(synopsis, vfx, animation, motion, modeling, qa)가 남아 게이트 투입 불가.
12. **RFC 블록 미작성.** RFC는 디렉터 소유 공유 진실 파일 `production/decision-log.md`에 기록되며
    이번 지시가 그 편집을 금지했다. CLAUDE.md §11에 따라 가상의 ack·회의를 만들지 않았다.
    **M19/M20 마일스톤 라벨 소유권과 r01·r02의 최종 처분은 디렉터/사용자 판정 사항**이다 —
    이번 세션이 대신 결정하지 않았다.
13. 커밋/푸시 **미실행**. 사용자가 수행한다.
