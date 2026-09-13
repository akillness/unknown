---
updated: 2026-09-13
cycle: 20260909-preproduction-c7
status: draft
supersedes: null
owner: game-presentation-director
---

# M21 — T0 작업면 본문 읽기 띠(reading band): r02 게이트 ON 가독성 정정 계약

> **이 문서가 무엇인가**: M20 네이티브 캡처가 드러낸 **단일 결함 하나**를 고치는 표현 계약이다.
> M20 게이트 ON에서 작업면 지면이 어두운 r02가 되었는데 커밋 본문색은 어두운 `ink`여서
> 글리프:지면이 **1.11:1** 로 붕괴했다(M19 베이스라인 3.53:1, `m20-ui-surface/verification.md` §0).
> M21은 **지면(ground)만** 바꾼다. 승격이 **아니고**, 텍스트 색·문구 변경이 **아니며**,
> 버튼 변경이 **아니고**, 새 에셋이 **아니고**, 접근성 인증·성능·사람 플레이테스트 주장도 **아니다**.
> 어떤 게이트도 올리지 않는다.

같은 사이클 안의 제자리 추가다 (RFC-Q2: `cycle` 값 불변 → `supersedes: null`, 아카이브 의무 없음).

선행 계약(둘 다 **유지·불변**):
- `_workspace/current/presentation/t0-action-plate-m19.md` (`status: draft`)
- `_workspace/current/presentation/t0-work-surface-m20.md` (`status: draft`)

## 0. status: draft 인 이유 (게이트 규칙 준수)

`references/dependency-matrix.md` **presentation 행**은 `syn ● / vfx ● / anim ● / mot ● / mod ● / qa ●`
를 요구하고, required ack 없는 변경은 `status: draft`로 남아 게이트에 투입될 수 없다. 이번 세션에
해당 레인의 **독립 응답은 없다**. CLAUDE.md §11에 따라 ack를 자작하지 않는다 (M18·M19·M20 선례 동일).

- **미해결 ● ack**: synopsis, vfx, animation, motion, modeling, qa
- **RFC 블록 미작성**: RFC는 디렉터 소유 공유 진실 파일 `production/decision-log.md`에 기록되며
  이번 지시가 그 편집을 금지했다. 이 변경은 캐논·수치·용어를 수립하지 않고
  `presentation-spec.md`·`glossary.md`·`campaign.json`·`balance-sheet.md`·`gate-measurements.md`를
  건드리지 않는다. 스스로 `runtimeApproved:false`를 고정하므로 승격 요청이 아니다.
- 이 문서는 **어떤 게이트도 올리지 않는다**. G4·G5는 `NOT-MEASURED`를 유지한다.

## 1. 고치는 결함 (정확히 하나)

[OBSERVED · M20 영수증 인용] 게이트 ON 캡처(pid 87336 / window 5311)에서 작업면 상단 본문 한 줄
(`21:00. 인수 각서와 이관 목록을 살핀 뒤, 당직실의 기록을 대조하세요.`)이 **시각적으로 사라졌다**.
본문 띠 표준편차 0.1360 → **0.0036** (글리프 신호 소멸), 글리프:지면 3.53:1 → **1.11:1**.

원인은 **색 충돌 하나**다: 커밋 본문색 = 어두운 `ink`, r02 지면 = 어두운 청록 에나멜.
M20은 텍스트를 바꿀 권한이 없어(M20 계약 N1·N12) 고치지 않고 결함으로 기록했다.

M20 §9.2가 남긴 세 선택지 중 이 계약은 **(b) 텍스트 뒤에 content-safe 판을 깐다** 를 택한다.
(a) 본문 색조 반전은 M19 3단 위계와 게이트 OFF 룩을 동시에 건드리고,
(c) r02 밝은 변형 재생성은 신규 에셋 생성이므로 이번 범위 밖이다.

## 2. 시각 약속 (the exact visual promise)

[TARGET] 플레이어가 얻는 것은 한 문장이다:
"작업면은 여전히 **어두운 아카이브 작업면**이지만, **읽어야 하는 문단 밑에만 얇은 종이 띠**가 깔려
글자가 다시 읽힌다."

| # | 약속 | 어떻게 보이는가 |
|---|---|---|
| R1 | **문단 단위 띠** | 작업면에 직접 얹히는 문단마다 그 문단 크기의 종이색 띠 한 겹. 패널 전체를 덮는 카드가 아니다 |
| R2 | **r02 보존** | 띠는 반투명(α 0.82)이므로 r02가 **띠를 통해 18% 투과**하고, 띠 **바깥(문단 사이·버튼 열·여백)은 r02 원본 그대로** |
| R3 | **버튼 불가침** | M19 계기 판은 자기 판(불투명 `ink`/`brass`)을 이미 가지므로 띠를 **받지 않는다** |
| R4 | **텍스트 불변** | 문구·색·크기·굵기 0건 변경. 지면만 바뀐다 |

- 감정 목표: 화려함이 아니라 **기록물 위에 놓인 작업 용지**. 재질은 여전히 종이와 금속 둘뿐이다.
- 애니메이션·전환 0건 추가. 띠는 상태를 갖지 않는다.

## 3. 정확한 사양

| 항목 | 값 |
|---|---|
| 오브젝트 이름 | `M21 reading` |
| 컴포넌트 | 기존 `Image` 프리미티브 (스프라이트 **없음** = 단색 쿼드). 신규 위젯·프리팹·셰이더·아틀라스 0건 |
| 색 | `Render()`가 이미 해석한 `paper` RGB + **α 0.82**. 신규 팔레트·신규 스킨 필드 0건 |
| raycast | `raycastTarget = false` — 입력 경로 밖 |
| 배치 | 문단의 **직전 형제**(sibling index = 문단 index) → 글리프 **뒤에** 그려진다 |
| 레이아웃 | `LayoutElement.ignoreLayout = true` — `VerticalLayoutGroup`이 띠를 **보지 않는다** → 커밋 문단·버튼 위치 0px 이동 |
| 기하 | 문단의 `anchorMin/anchorMax/pivot/offsetMin/offsetMax` 복사 + 패딩 (8, 3)px, 흐른 줄 수가 레이아웃 rect를 넘으면 아래로 확장 |
| 적용 대상 | `FlowText`가 작업면 `Content`에 **직접** 만든 문단(`"Text"`)뿐 |
| 제외 대상 | 버튼(자기 판 보유) · 방향 섹션(자기 패널 보유) · 사건 카드(자기 `ink` 판 보유) · 오프닝 화면 |
| 게이트 | **M20과 완전히 동일**. `surface != null` (= `runtimeApproved \|\| diagnosticOverride \|\| --m20-ui-surface-diagnostic`) |
| 게이트 OFF | 띠 오브젝트 **0개**. 기본 커밋 런타임 **픽셀 불변** (§영수증 A/B sha256 동일) |

### 3.1 α = 0.82 의 근거

| 조건 | 값 |
|---|---|
| 최악 지면(완전 검정) 위 본문 `ink` 대비 | **약 7.0:1** (평면 sRGB 산술) |
| r02 투과율 | **18%** — 아카이브 표면이 띠를 계속 변조한다 |
| α ≥ 0.85 | 띠가 불투명 카드에 가까워져 R2를 깬다 → 계약이 금지 |
| α = 1.0 | 아카이브 작업면 구성을 파괴 → 금지 |

## 4. 불변 목록 (no-copy-change / no-recolor / no-promotion)

| # | 불변 | 검사 |
|---|---|---|
| N1 | **플레이어 문구** 0건 변경 (`T0Strings.json` 무변경) | 문단 `text` 스냅샷 대조 |
| N2 | **텍스트 색·크기·굵기** 0건 변경 — 전역 재색칠 **금지** | 게이트 ON/OFF 역할 스냅샷 동일 |
| N3 | **액션 ID · 순서 · 개수 · 라벨 · `interactable`** 불변 | 자동 검사 |
| N4 | **입력 경로** `Navigate/Focus/Activate/BeginActivation/EndActivation/HoldButton` 0행 변경 | 자동 검사 |
| N5 | **포커스 링 크기·M19 반전** 불변 | 자동 검사 |
| N6 | **M19 판 표현** (`Bevel`/`Rule`/반전/18 Bold/16 약화색) 0건 변경, 버튼 안에 띠 **0개** | 자동 검사 |
| N7 | **M19 3단 위계** 본문 > 라벨 > 보조 유지 | 자동 검사 |
| N8 | **시뮬레이션·저장** `PuzzleCommand` 0건, `SavePending` false, 저장 필드 0건 변경 | 자동 검사 |
| N9 | **M20 계약 전부** — r02 배킹 1개·비타일 `uvRect(0,0,1,1)`·형제 index 0·비raycast·패널 자식 수·`Viewport` index/rect 불변 | 자동 검사 |
| N10 | **`M20WorkSurfaceProfile` 필드·에셋·임포터** 0건 변경 | 바이트 대조 |
| N11 | **`M7UiSkinProfile`** 필드·기본값·`runtimeApproved` 0건 변경 | 이 세션 편집 0건 |
| N12 | **`runtimeEligible` / `runtimeApproved` 승격 0건** | `provenance.json` · `.asset` 바이트 대조 |
| N13 | **에셋 생성·구매 0건**, `assets/generated/**` 편집 0건 | 바이트 대조 |
| N14 | **공유 진실 산출물** (`decision-log.md` 등) 편집 0건 | 이 세션 편집 0건 |
| N15 | **r01** 임포트·참조·연결 0건 | 미승인·미통합 유지 |
| N16 | **신규 이미지·텍스처·머티리얼 0건** — 띠는 단색 `Image` | 자동 검사 (`sprite == null`) |

## 5. 수용 기준 (자동 검사 가능한 형태)

1. **게이트 OFF**: `Content`에 `M21 reading` **0개**, 커밋 타일 `M7 paper` 배킹 1개 유지.
2. **게이트 ON**: 작업면 문단 수 == 띠 수, 각 띠는 해당 문단의 **직전 형제**.
3. **게이트 ON**: 각 띠 `raycastTarget == false`, `sprite == null`, `LayoutElement.ignoreLayout == true`.
4. **게이트 ON**: 띠 월드 사각형이 문단 월드 사각형을 **4면 모두 포함**, 띠 로컬 높이가
   `max(문단 rect 높이, preferredHeight)` 이상 (흐른 줄 전부 덮음).
5. **게이트 ON**: `0 < α < 1` 이고 `α ≤ 0.85` (불투명 카드 금지), 띠 너비 < 패널 너비,
   띠 높이 < 패널 높이 × 0.6 (전면 판 금지).
6. **게이트 ON**: 띠 위 본문 대비가 최악 지면 기준 **4.0:1 초과**이고, 본문·보조 둘 다
   자기 게이트 OFF 지면 대비의 **55% 초과** 회복.
7. **게이트 ON/OFF 공통**: §4 N1–N9의 자동 검사 항목 전부 동일.
8. **게이트 ON**: 클릭이 띠를 통과해 실제로 화면을 바꾸고, 재렌더 후에도 띠 수가 유지된다.
9. `M20WorkSurface.asset`의 `runtimeApproved`가 **false**, provenance `runtimeEligible`이 **false**.

## 6. 구현 경계

- `UI/M21ReadingBackingInterface.cs` — **신규**. `T0Interface` partial 1개 + 기하 미러 컴포넌트
  `ReadingBandFollow` 1개. 띠 생성·색·배치가 전부 이 파일 안에 있다.
- `UI/T0Interface.cs` — **추가 3행뿐** (주석 2 + 호출 1). M14~M20이 쓴 행은 **하나도 건드리지 않는다**.
- 화면별 분기 0개. 신규 상태 기계 0개. 신규 에셋 0개.
- `ReadingBandFollow`가 `LateUpdate`에서 미러하는 이유: `FlowText`는 `minHeight`만 고정하고
  `WrappedButtonHeight`가 이후 프레임에 열을 재유동시키므로, 1회 복사는 어긋난다.
  **진단 게이트 전용**이므로 게이트 OFF에서는 이 컴포넌트가 **존재하지 않는다**.

## 7. 비목표 (explicit non-goals)

1. **승격이 아니다.** r02는 `runtimeEligible:false` · `runtimeApproved:false` 유지.
   **M20 r02는 이 정정 이후에도 미승격이다** — 이 계약은 승격 근거가 아니라 진단 게이트 뒤의 정정이다.
2. **접근성 인증이 아니다.** §3.1과 영수증의 모든 대비 수치는 **평면 sRGB 픽셀/색 산술**이며
   글리프 안티에일리어싱·한국어 획 두께·디스플레이 감마·관측 거리를 반영하지 않는다.
   **WCAG 등 어떤 표준의 적합·부적합도 주장하지 않는다.**
3. **성능 주장 0건.** 진단 게이트 ON에서 문단당 `Image` 1개 + `LateUpdate` 1개가 추가되지만
   프레임 시간·드로콜·VRAM·발열을 **측정하지 않았다**.
4. **사람 플레이테스트 n=0.** "더 읽기 좋다"는 사람 판정은 **없다**. 측정된 것은 픽셀 통계뿐이다.
5. **최종/출시 아트가 아니다.** 아트 디렉션 확정도, 비주얼 레인 산출물의 대체도 아니다.
6. **M19·M20 계약 변경이 아니다.** 두 문서 모두 `status: draft` 그대로, 내용 편집 0건.
7. **연출 타임라인 추가가 아니다.** 새 전환·컷·카메라·사운드 큐 0건.
8. 게이트 이동 0건. G4·G5 `NOT-MEASURED` 유지, `status: draft` 유지(§0).
