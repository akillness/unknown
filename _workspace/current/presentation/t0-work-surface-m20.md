---
updated: 2026-09-13
cycle: 20260909-preproduction-c7
status: draft
supersedes: null
owner: game-presentation-director
---

# M20 — T0 작업면(Work Surface) 배경 후보 r02: 연출 + 통합 계약

> **이 문서가 무엇인가**: 부모 세션이 생성한 GTI 아카이브 작업면 후보
> `m20-archival-work-surface-r02`를 T0Interface의 **작업면 패널 배경 한 겹**으로만,
> **전용 M20 진단 게이트 뒤에서만** 적용하는 표현·통합 계약이다.
> 승격이 **아니고**, 타일 텍스처가 **아니며**, 버튼 텍스처가 **아니고**,
> 성능·접근성 인증·사람 플레이테스트·최종 아트 확정도 **아니다**.
> 어떤 게이트도 올리지 않는다.

같은 사이클 안의 제자리 추가다 (RFC-Q2: `cycle` 값 불변 → `supersedes: null`, 아카이브 의무 없음).

선행 연출 계약: `_workspace/current/presentation/t0-action-plate-m19.md` (`status: draft`) — **유지·불변**.

## 0. status: draft 인 이유 (게이트 규칙 준수)

`references/dependency-matrix.md` **presentation 행**은 `syn ● / vfx ● / anim ● / mot ● / mod ● / qa ●`
를 요구하고, 같은 문서가 "required acks가 없는 변경은 `status: draft`로 남고 게이트에 투입될 수
없다"고 규정한다. 원래 M20 작업에서는 해당 레인의 **독립 응답이 없었으므로** draft로 남았다.
RFC-CX-017의 실제 응답과 승인 판정은 디렉터 소유 `production/decision-log.md`가 정본이다.
정의 파일을 읽었다는 사실을 다른 레인의 독립 동의로 바꾸지 않는다.

- [OBSERVED · RFC-CX-017 사용자 최종 선택] **"기존 승인 리소스 개선"**: 일반 플레이는 기존
  승인 M7 리소스를 개선한다. **r02는 진단 전용**이며 runtime/provenance 승격을 하지 않는다.
  이 레인은 새로 추가했던 승인 메뉴·감사/라이선스 예외 프레임워크를 제거했다.
- [OBSERVED · 역할 로드] `.claude/agents/game-concept-artist.md`,
  `.claude/agents/game-presentation-director.md`, `.claude/agents/game-modeler.md`를 읽었다.
- **RFC-CX-017 ack**: 사용자 선택에 따라 기존 승인 리소스 개선과 r02 진단 전용 유지에 동의한다.
  **counter**: 네이티브 가독성이나 과거 내부 프로토타입 예외를 새 라이선스 승인으로 해석하지 않는다.
  r02의 license `UNVERIFIED`와 기존 provenance를 보존하며, 상업 사용 자격을 새로 부여하지 않는다.
- 이 레인은 M7 설정을 편집하지 않는다. M7 `workSurfaceTint` 알파의 네이티브 대비 비교와
  실제 조정은 Main 소유다. 다른 레인의 ack·사람 플레이·네이티브 검수 결과를 대신 기록하지 않는다.
- 이 문서는 **어떤 게이트도 올리지 않는다**. G4·G5는 `NOT-MEASURED`를 유지한다.

## 1. 에셋 출처와 실측 (provenance)

| 항목 | 값 [OBSERVED] |
|---|---|
| 저장소 경로 | `assets/generated/2d/ui/m20-archival-work-surface-r02.png` |
| 프롬프트 | `assets/generated/2d/ui/m20-archival-work-surface-r02.prompt.txt` |
| provenance 항목 id | `m20-archival-work-surface-r02` (`assets/generated/2d/ui/provenance.json`) |
| 도구 / 백엔드 / 모델 | `god-tibo-imagen(gti)` / codex private backend / `gpt-6-astra` |
| 요청 크기 → 실제 크기 | `2048x1152` → **`1672x941`** (백엔드가 요청 크기를 무시) |
| 바이트 | 2,220,940 |
| `output_sha256` | `cc52db8a8950aa7fd0915e4cae92b04a5892f9d9130863d89b2b52cbf83d75a7` |
| 생성 시각 | `2026-09-13T05:52:00Z` (이번 세션이 **생성하지 않았다** — 부모 세션 산출물) |
| 라이선스 | `UNVERIFIED (generated; check backend ToS before commercial use)` |
| `runtimeEligible` | **false — 이 계약은 이 값을 바꾸지 않는다** |
| `promoted_by` | `null` — 유지 |

**소유권 판정**: 이 파일들은 부모 세션이 만든 **알려진 상위 소유 추가물**이다. 외부 충돌이나
낯선 침입으로 취급하지 않는다. 이번 세션은 `gti` / `gen-2d.sh` / Higgsfield / Blender를
**한 번도 실행하지 않으며**, `assets/generated/**` 의 내용과 `provenance.json`을 **편집하지 않는다**.
r02는 **읽기 전용 입력**이고, Unity 임포트는 별도 후보 경로로 **복사**한다.

**r01 취급**: `m19-interview-control-surface-r01`은 **미승인·미통합 상태로 남는다**.
M20은 r01을 임포트하지 않고, 참조하지 않고, 프로필에 연결하지 않는다.

### 1.1 시각 검수 결과 (육안, 이번 세션 판정 전제)

[OBSERVED · 부모 세션 측정 인용] 산출물은 **단일 비-타일 작업면 후보**이며, 육안 검수에서
**문자·숫자·로고·아이콘이 없다**. 프롬프트의 NEGATIVE 절이 `text, letters, numbers, logos, icons,
UI labels, watermark, obvious repeating pattern, border frame` 을 명시적으로 배제한다.

이 판정은 **육안 검수**이며 자동 OCR·주기성 스펙트럼 분석이 **아니다**. 문자 부재를
측정값으로 승격하지 않는다 [한계].

## 2. 비-타일 규칙 (the non-tile rule) — 이 계약의 핵심 제약

[OBSERVED] 기존 M7 스킨 배킹은 **타일**이다: `UI/T0Interface.cs`의 `SkinBacking()`이 `RawImage`를
만들고 `TiledBacking.Apply()`가 `uvRect = (0,0,TilesAcross, TilesAcross*h/w)`를 써서
`paperTilesAcross = 2.5` 만큼 **반복**시킨다. 임포터도 그래서 `wrapMode = Repeat`를 쓴다.

r02는 **이 경로를 쓸 수 없다.** 독립 연출+기술 검토 판정:

| # | 규칙 | 이유 |
|---|---|---|
| T1 | r02는 **작업면 패널의 전면(full-bleed) 배경 한 겹**이다 | 16:9 단일 구도이며 중앙이 텍스트용으로 비워져 있다. 반복하면 그 중앙 여백과 외곽 마모가 격자로 드러난다 |
| T2 | r02는 **버튼 텍스처가 아니다** | 버튼은 M19의 **평면 계기 판**(단색 판 + `Bevel` + `Rule`)을 그대로 유지한다. 작은 사각형마다 1672×941 구도를 축소 반복하면 버튼마다 다른 얼룩이 생긴다 |
| T3 | `uvRect`는 **(0,0,1,1)** 이어야 한다 | 타일 UV 금지. M20 배킹은 `TiledBacking` 목록에 **등록되지 않는다** |
| T4 | 임포트 `wrapMode = Clamp` | 반복을 **엔진 수준에서** 불가능하게 만든다. `Repeat`는 M7 전용 |
| T5 | 정확히 **1개** | 작업면 패널에 M20 배킹은 한 개뿐이고, 게이트 ON일 때 같은 패널의 M7 타일 배킹을 **대체**한다 (겹쳐 쌓지 않는다) |

## 3. UI 위계 호환성 (텍스트는 전경에 남는다)

[OBSERVED] `Render()`의 작업면 하위 구조는 다음 순서다:

```
"Work Surface" (Panel, Image = paper·workSurfaceTint)
 ├─ [index 0] 배킹      ← SkinBacking / M20 둘 다 SetAsFirstSibling()
 └─ [index 1] "Viewport" (RectMask2D)
      └─ "Content" (VerticalLayoutGroup + ContentSizeFitter, ScrollRect content)
           ├─ "Text"   본문 / 상태 / 보조 설명   ← 전경
           ├─ "Signal" / "Overlay" 그래픽
           └─ 액션 버튼 (M19 판)                ← 전경, raycast 대상
```

| # | 호환성 요구 | 판정 방식 |
|---|---|---|
| H1 | 배킹은 **index 0**(첫 자식)이므로 `Viewport`/`Content`/텍스트/버튼 **전부 그 위에** 그려진다 | 자동 검사 |
| H2 | 배킹은 `raycastTarget = false` → 클릭·포커스가 **버튼에 도달한다** | 자동 검사 |
| H3 | **커밋된 자식 수와 `Viewport`의 형제 index가 게이트 OFF와 동일하다** — M20은 배킹을 **추가**하지 않고 **대체**하기 때문이다. `Viewport` 사각형도 불변 | 자동 검사 |
| H4 | `Work Surface` 루트 `Image`의 색·알파는 **불변** (배경 판은 그대로, 배킹 한 겹만 교체) | 자동 검사 |
| H5 | 헤더·툴바의 M7 `M7 frame` 배킹 2개는 **무변경** | 자동 검사 |
| H6 | 본문 20 / 라벨 18 Bold / 보조 16 약화색(M19 3단 위계)이 **그대로** | 자동 검사 |

**텍스트 문구는 한 글자도 바뀌지 않고, 새 공개(disclosure)도 없다.** r02는 이미 보이는 표면의
**배경 재질**만 바꾼다.

## 4. 진단 게이트 (정확한 사양)

| 항목 | 값 |
|---|---|
| 전용 프로필 에셋 | `Assets/_Project/Resources/M20WorkSurface.asset` (`Tide.Presentation.M20WorkSurfaceProfile`) |
| 후보 텍스처 경로 | `Assets/_Project/Art/Candidates/m20-ui-r02/UI_M20_ArchivalWorkSurface.png` |
| **명령행 게이트** | **`--m20-ui-surface-diagnostic`** |
| 승인 플래그 | `runtimeApproved` — **false 유지**. M20 임포터는 승인하지 않는다 |
| 메모리 전용 진단 스위치 | `diagnosticOverride` (`[NonSerialized]`, 디스크에 저장되지 않음) — 테스트/에디터 진단 전용 |
| 게이트 식 | `profile != null && profile.workSurface != null && (runtimeApproved \|\| diagnosticOverride \|\| args.Contains("--m20-ui-surface-diagnostic"))` |
| 게이트 OFF 동작 | **M19 작업면이 바이트 동일하게 유지된다** — 기존 M7 타일 경로 그대로, M20 오브젝트 0개 |

- **기본 커밋 런타임은 변하지 않는다.** `runtimeApproved:false` + 플래그 없음 → 게이트 OFF.
- `diagnosticOverride`는 **승격이 아니다**. 직렬화되지 않으므로 에셋 파일에 승인 상태를 남길 수 없다.
- 소비자 테스트는 메모리에서 승인을 끈 뒤 `diagnosticOverride` OFF/ON을 각각 검증하고
  원래 profile 값을 복구한다. 현재 커밋의 승인 기본값을 고정하는 단언이나 승인 저장은 없다.
- M20에는 승인 메뉴/API가 없다. RFC-CX-017 사용자 선택에 따라 r02는 진단 전용으로 유지한다.


## 5. 불변 목록 (no-copy-change / no-disclosure / no-promotion)

| # | 불변 | 근거 |
|---|---|---|
| N1 | **플레이어 문구** 0건 변경 (`T0Strings.json` 무변경) | |
| N2 | **액션 순서 · ID · 개수** 불변 | `ActionIds` 단언 |
| N3 | **라벨 문자열** 불변 | 딕셔너리 대조 |
| N4 | **활성 판정** `button.interactable = action.Enabled` 불변 | |
| N5 | **입력 경로** `Navigate/Focus/Activate/BeginActivation/EndActivation/HoldButton` 0행 변경 | |
| N6 | **클릭·포커스 보존** — 배킹은 비-raycast, 포커스 반전(M19 P2) 그대로 | |
| N7 | **시뮬레이션 상태** `PuzzleCommand` 제출 0건, 저널 쓰기 0건 | CLAUDE.md §9 |
| N8 | **저장 필드** 0건 변경 | 마이그레이션 불변식 무관 |
| N9 | **공개 표면** 새 정보 0건 | |
| N10 | **생성 데이터 테이블** `Data/Tables/*`, `systems/data/t0/*` 무변경 | |
| N11 | **`M7UiSkinProfile` 필드 · 기본값 · `runtimeApproved`** 0건 변경 | M7 승인 게이트 그대로 |
| N12 | **M19 버튼 표현** (`Bevel`/`Rule`/반전/18 Bold/16 약화색) 0건 변경 | |
| N13 | **r01** 임포트·참조·연결 0건 | 미승인·미통합 유지 |
| N14 | **`runtimeEligible` / `runtimeApproved` 승격** 0건 | 사용자 선택: r02 진단 전용 |
| N15 | **2D/3D/이미지/영상 생성·구매** 0건. `assets/generated/**` 편집 0건 | |
| N16 | **공유 진실 산출물** 편집 0건 | |
| N17 | **신규 위젯 · 프리팹 · 셰이더 · 머티리얼 · 스프라이트 아틀라스** 0건 | 배킹은 기존 `RawImage` 프리미티브 재사용 |

## 6. 구현 경계

- `Presentation/M20WorkSurfaceProfile.cs` — 기존 순수 데이터 ScriptableObject. 승인 계보 필드 추가 없음.
- `Editor/M20WorkSurfaceProjectBuilder.cs` — 기존 후보 임포터를 유지한다. `provenance.json`의
  `output_sha256`을 **대조한 뒤에만** 복사하고, `runtimeApproved=false`로 저장하며 감사 JSON을 남긴다.
- `App/M20WorkSurfaceSession.cs` — **신규 partial**, 게이트 1개 + `s.WorkSurface` 대입 1행.
  부수효과는 그 대입뿐이다.
- `UI/T0Interface.cs` — **추가만**: `GameScreen.WorkSurface` 필드 1개, `FullBleedBacking()` 헬퍼 1개,
  작업면 배킹 호출을 게이트 분기로 감싸는 것. **M19가 수정한 행은 하나도 건드리지 않는다.**
- `App/T0GameSession.cs` — `ApplyM7UiSkin(s);` 옆에 `ApplyM20WorkSurface(s);` **호출 1개 추가**.
  이 행은 HEAD 원본이며 M14~M19가 수정한 행이 **아니다**.
- 화면별 분기 0개. 작업면 패널 **한 곳**만 영향을 받는다.

## 7. 수용 기준 (자동 검사 가능한 형태)

1. **게이트 OFF**: 작업면 패널에 `M20` 이름의 자식이 **0개**이고, 기존 M7 타일 배킹
   (`M7 paper`, `uvRect.width == paperTilesAcross`)이 **그대로 1개**다. 루트 `Image.color` 불변.
2. **게이트 ON**: 작업면 패널에 M20 배킹이 **정확히 1개**, `RawImage.texture`가 r02 후보 텍스처,
   `raycastTarget == false`, 형제 index **0**, 앵커 `(0,0)–(1,1)` **전면**,
   `uvRect == (0,0,1,1)` — **타일 UV 아님**, `texture.wrapMode == Clamp`.
3. **게이트 ON**: 같은 패널에 `M7 paper` 타일 배킹이 **0개** (겹쳐 쌓지 않는다 = T5).
4. **게이트 ON**: 작업면 패널의 **자식 수**와 `Viewport`의 형제 index가 OFF와 **동일**하고
   사각형도 **동일**하다. (index가 하나 밀렸다면 r02가 M7 배킹 **위에 쌓였다**는 뜻이므로 T5 위반이다.)
5. **게이트 ON/OFF 공통**: 액션 ID 순서 · 라벨 문자열 · `interactable` · 포커스 가능 수 ·
   M19 판 장식(`Bevel`/`Rule`) · 포커스 반전 · 텍스트 3단 위계가 **모두 동일**하다.
6. **게이트 ON**: 버튼 하위에 `RawImage`가 **0개** (T2 — r02가 버튼에 붙지 않는다).
7. **게이트 ON**: 클릭이 실제로 동작하고(오버레이 전환) 포커스가 유지된다.
8. 저장된 현재 승인 기본값과 무관하게 OFF/ON을 각각 검증하고 테스트 후 원래 값으로 복구한다.
   진단 ON에서도 profile/provenance 승인 파일을 저장하지 않는다.

## 8. 비목표 (explicit non-goals)

1. **승격이 아니다.** `runtimeEligible:false` · `runtimeApproved:false` 유지. 기본 커밋 런타임 무변경.
2. **최종/출시 아트가 아니다.** 진단 게이트 뒤의 **후보 1장**이며 아트 디렉션 확정이 아니다.
3. **접근성 인증이 아니다.** r02 위의 본문·보조 텍스트 실측 대비를 **측정하지 않는다**.
   M19 §6의 평면 색 산술값은 넝마지·청동 텍스처 기준이며 r02에는 적용되지 않는다 → NOT-MEASURED.
4. **성능 주장 0건.** 1672×941 텍스처 1장의 VRAM·드로콜·프레임 영향을 **측정하지 않는다**.
5. **사람 플레이테스트 n=0.** "더 좋아 보인다"는 주장을 하지 않는다.
6. **타일 가능성(seamlessness) 판정이 아니다.** r02는 애초에 비-타일 용도로만 쓰인다 (§2).
7. **r01 판정이 아니다.** r01은 미승인·미통합으로 남는다.
8. **연출 타임라인 추가가 아니다.** 새 전환·컷·카메라·사운드 큐 0건.
9. 게이트 이동 0건. G4·G5 `NOT-MEASURED` 유지, `status: draft` 유지(§0).
