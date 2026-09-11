---
updated: 2026-09-10
cycle: 20260909-preproduction-c4
status: current
supersedes: null
owner: game-modeler
---

# Asset Manifest — 47종

`modeling/asset-budget.md`(세션 P)의 수량 계약 **셸5 / 도구6 / 초상5 / 공용30 / UI1 = 47**을 행 단위로 편 정식 매니페스트다. 예산 총량은 `asset-budget.md`가 계속 소유하고 이 문서는 개별 자산의 신원·상태·경로를 소유한다. 파이프라인 규격은 `modeling/pipeline.md`.

## 0. 상태 표기와 게이트

| 상태 | 뜻 |
|---|---|
| `greybox` | 실제 파일이 있고 눈으로 확인했다. 형상은 부피 대용물이며 최종 아트가 아니다 |
| `pending` | 파일 0개. 이름·예산·경로만 정해져 있다 |

- **모든 행의 `runtimeEligible`은 `false`다.** 승격은 `production/decision-log.md` 감사로만(CLAUDE.md §9).
- [OBSERVED · 2026-09-10 R4 재측정] **컨셉 입력은 더 이상 0이 아니다.** 명령과 값:
  `find _workspace/current/concept/sheets -type f | wc -l` → **1** (`concept/sheets/README.md` 14.8 KB · `status: current` · 45장 시트 색인) ·
  `find assets/generated/2d -name '*.png' | wc -l` → **45** (카테고리별 concept 21 / previz 9 / readme 7 / ui 4 / capsule 2 / keyart 2) ·
  `find assets/generated/2d -name provenance.json | wc -l` → **6** · `concept/generation-manifest.md` L114 "45 / 45 생성 성공, skipped 0".
  규범 스펙 `concept/style-guide.md`(`status: current` · `updated: 2026-09-10`)가 §2 팔레트 8색 · §2.1 색약 대체 세트 · §3 실루엣 최소 3 px · §9 도구 6종 형상·글리프를 준다.
  **이 문서의 앞선 판은 "`concept/sheets/` 파일 수 = 0 … 컨셉 시트가 있는 자산은 하나도 없다"고 적었다 — 스테일 `[OBSERVED]`였고 위 실측으로 교체한다**(C4-F10 · C3-F28·C3-F34 와 같은 유형).
- [OBSERVED] `find _workspace/current/presentation/scene-boards -type f | wc -l` → **0**. 씬 보드는 아직 없다 — 앞선 판의 이 값만은 그대로 유효하다.
- 레인 원칙 "concept_ref 없는 자산은 만들지 않는다"는 **유지**된다. 그레이박스를 컨셉보다 먼저 만든 것은 여전히 **의도된 예외**이며(근거 `concept/art-direction.md` 당직실 행) 그 예외의 유효 범위도 **부피·동선·카메라 가설 검증까지**로 변함이 없다.
- **차단선 재판정 (C4-F10 → OPEN-M4)**: 앞선 판의 자체 차단선("실루엣·재질·디테일 모델링은 `concept/sheets/*.md`가 생기기 전에는 시작하지 않는다")은 전제가 해소되어 **부분 해제**한다. 이제 행별 판정은 아래 §0.1 대응표가 소유한다. 남은 보류는 "시트가 없어서"가 아니라 **시트가 스스로 재생성을 요구해서**(§9) 또는 **전용 시트가 없어서**(공용 소품 30)다.
- [미측정] Unity 임포트 0건, 런타임 tri/drawcall 0건. 아래 `tri 예산`은 전부 `[TARGET]`이고 `측정 tri`만 `[OBSERVED]`다.

### 0.1 컨셉 입력 ↔ 매니페스트 대응과 행별 착수 판정 (C4-F10 재판정) [OBSERVED]

출처는 `concept/sheets/README.md`(§1~§7 검수표, §9 재생성 우선순위)와 `concept/style-guide.md`(§2·§3·§9)다.

| 분류 | 대응하는 컨셉 입력 | §9 재생성 표시 | 착수 판정 |
|---|---|---|---|
| 공간 셸 5 | §2 공간 무드 **5장**이 zone 5개와 1:1 | `space-gate-three-mood` = §9 **3순위 권장**(배경 성곽/아치교가 캐논 밖) | Hub·Lowland·Quay·Pump1 **해제** / `SM_Gate3_Shell` **보류** |
| 영웅 도구 6 | §3 히어로 프롭 **6장** + `style-guide.md` §9 형상·글리프 | `tool-alignment-hero`(실루엣 분리 약함)·`tool-corrosion-hero`(결정이 육각 아님) = §9 **5순위 조건부** | circuit·reader·routing·seal **해제** / alignment·corrosion 은 그레이박스 유지, **최종 형상 확정 보류** |
| 인물 초상 5 | §1 인물 **10장**(전신 5 + 대화 초상 5) | §9 6순위(정사각 슬롯 확정 후 1:1 재생성 또는 크롭) | 이 레인 소유 아님 — 3D 작업 0 |
| 공용 소품 30 | **전용 시트 0.** §2 공간 무드·§6 readme 7장의 배경 등장이 전부 | — | **보류** — 사유가 "시트 0"에서 "**전용** 시트 0"으로 좁혀졌다 (OPEN-M5) |
| UI 프레임 1 | §4 `ui-workbench-frame`(조건부·요청 규격 미달) + `ui-tool-icon-sheet` | §9 4순위(규격 크롭·리사이즈 공정) | 이 레인 소유 아님 — 3D 작업 0 |

`concept/sheets/README.md` §8 `constraints:` 블록이 이 레인의 **측정 가능한 규범 입력**이다(팔레트 8색 + 예비 1 · 실루엣 ≥3 px @1080p · `hero_tools: 6` 피벗=도구 원점·단위 1 m · `color_only_encoding: forbidden`). 대조 결과 **불일치 2건**을 발견했다 → OPEN-M6, `production/decision-log.md` RFC 대상.

| 항목 | `sheets/README.md` §8 [TARGET] | `modeling/pipeline.md` §9 [OBSERVED] | 차이 |
|---|---|---|---|
| 카메라 부감각 | `pitch_deg: -18` | 34.998° (`atan((5.501−0.95)/(1.20−(−5.30)))` 재계산) | **17.0°** |
| 카메라 높이 | `eye_height_m: 1.55` | z = 5.501 m | **3.95 m** |
| 렌즈 | `lens_mm_equiv: 35` | 34 mm (36 mm 센서) | 1 mm — 무시 가능 |

모델링 값은 "당직실은 6개 도구를 같은 프레임에서 가르친다"를 만족시키기 위해 렌더로 확정한 실측이고(§2 말미), 컨셉 값은 생성 이전에 정한 `[TARGET]`이다. **어느 쪽이 정본인지는 모델러가 정하지 않는다.**

## 1. 공간 셸 5

| id | zone | concept_ref | 상태 | 측정 tri | tri 예산 | 텍스처 예산 | 생성 경로 | runtimeEligible |
|---|---|---|---|---:|---:|---|---|---|
| `SM_Hub_Shell` | Hub (당직실) | `sheets/README.md` §2 `space-hub-watchroom-mood` (+§5 keyart 2장) | **greybox** | **72** | 60k | 2K albedo + 2K ORM (hero) | Blender (`build_hub_greybox.py`) | false |
| `SM_Gate3_Shell` | Gate3 (제3수문) | §2 `space-gate-three-mood` — **§9-3 재생성 선행** | pending | 0 | 60k | 1K~2K | Blender | false |
| `SM_Lowland_Shell` | Lowland (구염전 저지대) | §2 `space-lowland-mood` | pending | 0 | 60k | 1K~2K | Blender | false |
| `SM_Quay_Shell` | Quay (냉동창고 부두) | §2 `space-wharf-mood` | pending | 0 | 60k | 1K~2K | Blender | false |
| `SM_Pump1_Shell` | Pump1 (제1양수장) | §2 `space-pumphouse-one-mood` | pending | 0 | 60k | 1K~2K | Blender | false |

`SM_Hub_Shell`은 6개 오브젝트로 구성된다 [OBSERVED, 각 12 tris]: `SM_Hub_Floor`(6.00×8.00×0.10 m) · `SM_Hub_Wall_N`(6.00×0.12×2.60) · `SM_Hub_Wall_W`(0.12×8.00×2.60) · `SM_Hub_Wall_E`(0.12×8.00×2.60) · `SM_Hub_Workbench`(2.40×0.90×0.90) · `SM_Hub_PlateShelf`(0.36×1.80×1.15). 남쪽 벽과 천장은 2.5D 고정 시점을 위해 없다.

## 2. 영웅 도구 6 — toolId는 캐논, 신규 생성 금지

| id | toolId | concept_ref | spec_ref | 상태 | 측정 tri | tri 예산 | 텍스처 예산 | 개별 파일 | runtimeEligible |
|---|---|---|---|---|---:|---:|---|---|---|
| `SM_Tool_circuit` | `circuit` | §3 `tool-circuit-hero` | `planning/feature-specs/verb-01-circuit-trace.md` | **greybox** | **12** | 12k | 1K(기본)/2K(확대뷰 후보) | `SM_Tool_circuit.glb` | false |
| `SM_Tool_reader` | `reader` | §3 `tool-reader-hero` | `verb-02-plate-read.md` | **greybox** | **12** | 12k | 〃 | `SM_Tool_reader.glb` | false |
| `SM_Tool_alignment` | `alignment` | §3 `tool-alignment-hero` | `verb-03-tide-alignment.md` | **greybox** | **12** | 12k | 〃 | `SM_Tool_alignment.glb` | false |
| `SM_Tool_routing` | `routing` | §3 `tool-routing-hero` | `verb-04-drain-routing.md` | **greybox** | **12** | 12k | 〃 | `SM_Tool_routing.glb` | false |
| `SM_Tool_corrosion` | `corrosion` | §3 `tool-corrosion-hero` | `verb-05-corrosion-assay.md` | **greybox** | **12** | 12k | 〃 | `SM_Tool_corrosion.glb` | false |
| `SM_Tool_seal` | `seal` | §3 `tool-seal-hero` | `verb-06-dual-seal.md` | **greybox** | **12** | 12k | 〃 | `SM_Tool_seal.glb` | false |

치수·배치 (m, 피벗 = 바닥 중심, 씬 좌표는 Blender Z-up) [OBSERVED]:

| id | 치수 (X×Y×Z) | 씬 위치 | 회전 Z | 설치 |
|---|---|---|---:|---|
| `SM_Tool_reader` | 0.80×0.50×0.34 | (-0.62, 1.22, 0.90) | 0° | 작업대 위 좌 |
| `SM_Tool_seal` | 0.70×0.44×0.22 | (0.68, 1.18, 0.90) | 0° | 작업대 위 우 |
| `SM_Tool_circuit` | 2.00×0.09×1.20 | (0.00, 3.79, 1.05) | 0° | 북쪽 벽 패널 |
| `SM_Tool_routing` | 0.09×1.60×1.00 | (-2.83, 0.60, 1.10) | 0° | 서쪽 벽 패널 |
| `SM_Tool_alignment` | 1.20×0.60×1.00 | (1.85, 2.55, 0.00) | -15° | 바닥 콘솔(동북) |
| `SM_Tool_corrosion` | 1.00×0.70×0.85 | (-2.00, 0.45, 0.00) | +12° | 바닥 시험대(서) |

여섯 도구는 고정 카메라 한 프레임에 **전부** 들어온다 — `renders/hub-cam.png`에서 확인했다 [OBSERVED]. 이는 "당직실은 모든 도구를 같은 프레임에서 가르친다"(`concept/art-direction.md`)의 형상 쪽 요구다.

## 3. 인물 초상 5 (2D — 이 레인이 만들지 않는다)

각 인물 3표정(평상/방어/결심), 시트 1장. 리깅 없음(`asset-budget.md`: "2D, 별도 리깅 없음"). 생성 경로는 GTI(`scripts/gen-2d.sh`)이며 소유는 concept 레인이다. 3D 메시 예산은 스프라이트 쿼드 2 tris 외 0.

| id | 인물 (용어집 등록) | concept_ref | 상태 | 텍스처 예산 | 생성 경로 | runtimeEligible |
|---|---|---|---|---|---|---|
| `PT_HanSeorin` | 한서린 / Han Seorin | §1 `char-seorin-sheet` + `char-seorin-portrait` | pending | 2K 시트(3표정 1장) | GTI 2D | false |
| `PT_MunJaehwa` | 문재화 / Mun Jaehwa | §1 `char-jaehwa-sheet` + `char-jaehwa-portrait` | pending | 〃 | GTI 2D | false |
| `PT_OEunjeong` | 오은정 / O Eunjeong | §1 `char-eunjeong-sheet` + `char-eunjeong-portrait` | pending | 〃 | GTI 2D | false |
| `PT_PyoSeongchan` | 표성찬 / Pyo Seongchan | §1 `char-seongchan-sheet` + `char-seongchan-portrait` | pending | 〃 | GTI 2D | false |
| `PT_HanDoyeon` | 한도연 / Han Doyeon | §1 `char-doyeon-sheet` + `char-doyeon-portrait` | pending | 〃 | GTI 2D | false |

## 4. 공용 소품 30 — 전역 공유 모듈 키트

`asset-budget.md`의 "상자·파이프·판·레버 모듈, 전역 공유"를 30행으로 편다. zone 토큰은 `Kit`(in-fiction 아님). 전부 `pending`, 개당 tri 예산 **1.5k [TARGET]**, 텍스처는 **30종 공유 1K 아틀라스 1장**, 생성 경로 Blender, `runtimeEligible:false`.

[OBSERVED] concept_ref는 **30행 전부 전용 시트 없음**이다 — `concept/sheets/README.md` §1~§7 어디에도 소품 단위 항목이 없다. 다만 §2 공간 무드 5장·§6 readme 7장의 **배경에 상자·파이프·판·레버·격자가 실제로 등장**하므로 재질·비례의 간접 참조는 존재한다. 따라서 이 30행의 보류 사유는 "컨셉이 없다"가 아니라 "**소품 단위 정본이 없다**"이며, 해소 경로는 OPEN-M5(§7)다.

| # | id | 군 | 용도 메모 |
|---:|---|---|---|
| 1 | `SM_Kit_Crate_S` | 상자 | 저지대 재산 선택 연출의 최소 단위 |
| 2 | `SM_Kit_Crate_M` | 상자 | |
| 3 | `SM_Kit_Crate_L` | 상자 | |
| 4 | `SM_Kit_CrateStack` | 상자 | 3~4단 적재 프리셋 |
| 5 | `SM_Kit_Drum` | 상자 | 원통 용기 |
| 6 | `SM_Kit_Pipe_Straight_1m` | 파이프 | 염선 도관 기본 |
| 7 | `SM_Kit_Pipe_Straight_2m` | 파이프 | |
| 8 | `SM_Kit_Pipe_Elbow` | 파이프 | 90° |
| 9 | `SM_Kit_Pipe_Tee` | 파이프 | 분기 |
| 10 | `SM_Kit_Pipe_Flange` | 파이프 | 접합부 |
| 11 | `SM_Kit_Pipe_Bracket` | 파이프 | 벽 고정 |
| 12 | `SM_Kit_Pipe_Valve` | 파이프 | 밸브 몸통(핸들은 21번) |
| 13 | `SM_Kit_Panel_Wall` | 판 | 벽면 모듈 |
| 14 | `SM_Kit_Panel_Gauge` | 판 | 계기판(문자 없음) |
| 15 | `SM_Kit_Panel_Junction` | 판 | 결선함 |
| 16 | `SM_Kit_Panel_Plate` | 판 | 무지 표지판 — 텍스트를 굽지 않는다 |
| 17 | `SM_Kit_Panel_Grate` | 판 | 배수 격자 |
| 18 | `SM_Kit_Panel_Hatch` | 판 | 점검구 |
| 19 | `SM_Kit_Lever_Long` | 레버 | `lever_throw` 클립 대상 |
| 20 | `SM_Kit_Lever_Short` | 레버 | |
| 21 | `SM_Kit_Wheel_Valve` | 레버 | `valve_turn` 클립 대상 |
| 22 | `SM_Kit_Switch_Toggle` | 레버 | |
| 23 | `SM_Kit_Crank` | 레버 | `pump_spin` 연계 |
| 24 | `SM_Kit_Beam_H` | 구조 | 수평 보 |
| 25 | `SM_Kit_Beam_V` | 구조 | 수직 기둥 |
| 26 | `SM_Kit_Stair_Step` | 구조 | 계단 1단 |
| 27 | `SM_Kit_Rail` | 구조 | 난간 |
| 28 | `SM_Kit_Ladder` | 구조 | 사다리 |
| 29 | `SM_Kit_Duct` | 구조 | 냉장 덕트(부두) |
| 30 | `SM_Kit_Bollard` | 구조 | 계선주 |

## 5. UI 아트 프레임 1 (2D)

| id | concept_ref | 상태 | 텍스처 예산 | 생성 경로 | runtimeEligible |
|---|---|---|---|---|---|
| `UI_ToolFrame` | §4 `ui-workbench-frame` + `ui-tool-icon-sheet` | pending | 2K 1장 | GTI 2D | false |

도구 6종과 상태·언어가 **같은 프레임**을 공유한다(`asset-budget.md`). 프레임이 갈라지면 재사용 계약이 깨지므로 도구별 프레임을 만들지 않는다.

## 6. 합계와 대조

| 분류 | 계약 수량 | 매니페스트 행 | greybox | pending |
|---|---:|---:|---:|---:|
| 공간 셸 | 5 | 5 | 1 | 4 |
| 영웅 도구 | 6 | 6 | 6 | 0 |
| 인물 초상 | 5 | 5 | 0 | 5 |
| 공용 소품 | 30 | 30 | 0 | 30 |
| UI 프레임 | 1 | 1 | 0 | 1 |
| **합계** | **47** | **47** | **7** | **40** |

[OBSERVED] 오늘 실제로 존재하는 3D 메시는 12개(허브 셸 6 + 도구 6), 합계 **144 tris**. 파일 목록과 해시는 `assets/generated/3d/provenance.json`.

## 7. 열린 항목

| id | 상태 | 판정 |
|---|---|---|
| OPEN-M1 | **closed** | worldview 결정(2026-09-10) |
| OPEN-M2 | **closed** | RFC-P4-001 디렉터 판정(2026-09-10) |
| OPEN-M3 | **closed** | `style-guide.md` 도착 + 아래 판정 |
| OPEN-M4 | **부분 해제(re-scoped)** | §0.1 대응표가 행별로 소유 |
| OPEN-M5 | open | 공용 소품 30 — 전용 시트 부재 |
| OPEN-M6 | open | 카메라 규범 값 불일치 2건 |
| OPEN-M7 | open | 재생성 원본의 git 이력화 여부 (`assets/` 미추적) |

- **OPEN-M1 — closed.** [OBSERVED] `worldview/glossary.md` §7 파생 규칙(2026-09-10, `grep -n '파생 규칙 — 모델러 OPEN-M1' _workspace/current/worldview/glossary.md` → L152)이 "에셋 오브젝트명·파일명의 zone 토큰은 `zoneId` 영문 토큰을 그대로 쓰는 것을 허용"으로 판정했고, L153 이 "`modeling/pipeline.md`의 `Hub`는 **그대로 유효** … **OPEN-M1 닫힘**"을 명시했다. 읽는 법(L154): `Hub`↔`hub` · `Gate3`↔`gate` · `Pump1`↔`pump` · **`Quay`↔`dock`** · `Lowland`↔`lowland`. 일괄 치환은 **요구되지 않는다.** 다만 **새로 만드는 토큰은 `zoneId` 값을 그대로 쓴다** — 이 매니페스트의 기존 5개 zone 토큰은 이 규칙의 대상이 아니고, 앞으로 추가되는 zone 은 대상이다.
- **OPEN-M2 — closed.** [OBSERVED] `production/decision-log.md` RFC-P4-001(2026-09-10, 디렉터): "현 사전제작 범위에서는 Mixamo 를 **조건부 미사용**으로 둔다. … 3D 인물 도입은 C4 이후 별도 RFC(인물 셸 5체 추가 = 아트 예산 +25인일 이상 [INFERENCE])로만 열린다. `modeling/pipeline.md` 의 Mixamo 규격은 그 RFC 를 위해 유지한다." 이 매니페스트의 3D 휴머노이드 행 수는 **0으로 확정**이며 §6 합계 47은 영향받지 않는다.
- **OPEN-M3 — closed.** [OBSERVED] `concept/style-guide.md`는 존재한다(`status: current` · `updated: 2026-09-10`). 앞선 판의 "`concept/style-guide.md`가 없어"는 스테일 `[OBSERVED]`였다(C4-F10). 정본을 읽은 결과 **도구별 강조색이라는 개념 자체가 기각된다**:
  - §2 팔레트는 8색이고 난색 악센트는 `#E2AF62`(≤8%) **하나뿐**, 예비 `#8C4A3A`(≤2%)는 "9번째 색은 팔레트에 들지 않으며 무단 확장 금지".
  - §2.1 은 "모든 상태·매체·도구는 색 + 형태(윤곽 실루엣) + 위치의 **3중 부호**"를, §8 `constraints` 는 `color_only_encoding: forbidden` 을 요구한다. 도구 구분의 정본 부호는 **§9 형상·글리프 6종**(격자 / 팔 달린 원 / 평행선+눈금 / 분기 Y / 육각 결정 / 겹친 사각 2)이며 §9 말미가 "색을 지운 1비트 실루엣에서도 상호 구분"을 요구한다.
  - [OBSERVED] 그레이박스 재질 9종의 hex 중 §2 팔레트 8색(+예비 1)과 일치하는 값은 **0/9**다. 재측정 명령: `python3` 로 `assets/generated/3d/scripts/build_hub_greybox.py` 의 `ACCENT`·`GREY_*` 를 파싱해 팔레트 9값과 집합 비교 → `palette matches: 0`.
  - **판정**: 임시 6색은 최종 재질의 후보가 아니라 **그레이박스 뷰포트 식별 전용 디버그 값**으로 격하한다. 최종 재질은 §2 팔레트 안에서만 만들고 도구 구분은 §9 형상이 맡는다. 이 매니페스트의 텍스처 예산 열은 변경 없음.
  - [남은 작업 · 이 레인 밖] 생성 스크립트 `assets/generated/3d/scripts/build_hub_greybox.py` L18~20 주석이 아직 "`concept/style-guide.md` 가 아직 없어"라고 적고 있다. 스크립트는 `assets/` 소유라 이 레인이 고쳐 쓰지 않는다 — RFC-M3.
- **OPEN-M4 — 부분 해제(re-scoped).** §0.1 대응표가 정본이다. 해제: 셸 4종(Hub·Lowland·Quay·Pump1) + 도구 4종(circuit·reader·routing·seal). 보류: `SM_Gate3_Shell`(§9-3) · 도구 2종 최종 형상(§9-5) · 공용 소품 30(OPEN-M5) · 초상 5·UI 1(이 레인 소유 아님).
  QA 요구대로 **`sheets/README.md` §9 재생성 우선순위 1~3 선행**을 조건으로 기록한다. [INFERENCE] 다만 1위 `readme-verb-seal`(저장소 README 삽화)과 2위 `capsule-library-candidate`(상점 캡슐 후보)는 **모델링 입력이 아니다** — 실제로 이 레인의 착수를 막는 것은 3위 `space-gate-three-mood` 하나다. 이 차이는 RFC-M1 로 올린다.
- **OPEN-M5 — open.** 공용 소품 30종의 **전용 컨셉 시트가 0**이다(§4). 두 경로 중 하나가 필요하다: (a) concept 레인이 소품 키트 시트를 추가 생성, (b) `sheets/README.md` §8 `constraints` + §2 공간 무드의 배경 등장만으로 착수해도 좋다는 concept 레인의 명시적 회신. 모델러는 (b)를 스스로 선언하지 않는다 → RFC-M2.
- **OPEN-M6 — open.** §0.1 표의 카메라 값 불일치 2건(부감각 17.0° · 높이 3.95 m). 컨셉 `[TARGET]`과 모델링 `[OBSERVED]` 중 무엇을 정본으로 삼을지 presentation/concept/director 판정 필요 → RFC-M4. 판정 전까지 이 매니페스트의 배치 좌표(§2 표)는 **모델링 실측값 기준**을 유지한다.
- **OPEN-M7 — open (신설 2026-09-10).** [OBSERVED] `git ls-files assets | wc -l` = **0**이고 `.gitignore` 에 `assets/` 항목이 **없다** — 생성 산출물은 무시된 것이 아니라 한 번도 add 된 적이 없다. `modeling/pipeline.md` §14 가 재생성 시 원본을 `assets/generated/archive/<날짜>/` 로 **파일시스템 보존**하도록 잠갔으나, 그것은 git 이력 보존이 아니며 새 clone 에는 원본도 아카이브도 없다. 추적 여부(총 용량·LFS·Steam 자산 라이선스 `UNVERIFIED` 상태에서의 공개 저장소 노출)는 모델링 레인이 스스로 정하지 않는다. commit/push 는 사용자만 수행한다(CLAUDE.md §8 · 계약 Release safety) → RFC-M5. 판정 전까지 이 매니페스트의 47행 상태·경로 열은 영향받지 않는다(생성물 45장·12메시는 47종의 진척이 아니다 — C5-F10).


## 8. 변경 로그 (RFC-Q2 — 같은 `cycle` 안의 제자리 갱신 = 개정)

| 일자 | 개정 | 근거 |
|---|---|---|
| 2026-09-10 (C6/C7 수정 루프 1) | §7 에 OPEN-M7 신설(재생성 원본의 git 이력화 여부) — `pipeline.md` §14 비파괴 재생성 계약의 잔여 위험. 47행·수량 계약·상태 열 변경 없음 | `qa/c6-review.md` §3 C7-F13 · `modeling/pipeline.md` §14.8 |
| 2026-09-10 (R4 수정 루프 1) | §0 스테일 `[OBSERVED]` 3건 교체(시트 0 → 실측 · style-guide 부재 → 존재 · OPEN-M1 미판정 → 판정됨), §0.1 신설, 47행 전체 `concept_ref` 채움, OPEN-M1/M2/M3 closed, OPEN-M4 부분 해제, OPEN-M5/M6 신설 | `qa/c4-review.md` C4-F10 · `qa/defect-register.md` L194 |

수량 계약은 이번 개정에서 **바뀌지 않았다** — `5 + 6 + 5 + 30 + 1 = 47`이 §6 표·`asset-budget.md` L13~19 와 그대로 일치한다(QA 부기와 동일 결론). `status`·`cycle` 값도 바꾸지 않았다(디렉터 지시 대기).


## 9. T0 workbench attachment candidate (RFC-CX-003)

[OBSERVED 2026-09-10] OPEN-S11 names `hub-view-drawer` but the historical 47-row production manifest has no drawer model. RFC-CX-003 authorizes a separate original Blender candidate under the **existing `SM_Hub_Workbench` / `SM_Hub_Shell` identity**, preserving the baseline 47-item count and original greybox files. Its contextual concept references describe the hub workspace and shallow evidence drawers; a dedicated final-art prop sheet remains unapproved.

| id | type / parent | concept_ref | triangle budget | measured triangles | texture budget | LOD | rig_ref | status | candidate path | runtimeEligible |
|---|---|---|---:|---|---|---|---|---|---|---|
| `SM_Hub_Workbench_Drawer` (r01) | rigid attachment / `SM_Hub_Shell` | `concept/sheets/README.md` §2 `space-hub-watchroom-mood`, §4 `ui-workbench-frame`; `concept/t0-source-drawer-review.md` | 1,500 [TARGET] | 156 triangles / 2 meshes [OBSERVED Blender 5.1.2 receipt] | 0 texture files [OBSERVED] | LOD0 | `animation/rig-requirements.md` §2.1; separate root and tray pivot, no skeleton | generated blockout; shape/palette ACK; final material FIX; runtime audit pending | `assets/generated/3d/hub-view-drawer-r01/` | false |
| `SM_Hub_Workbench_Drawer` (r02) | same attachment; no new production identity | r01 refs + `concept/t0-source-drawer-review.md` r02 material FIX; `concept/style-guide.md` §1/§4 | 1,500 [TARGET] | 156 triangles / 2 meshes [OBSERVED actual receipt] | 4 × 1024² PNG [OBSERVED files]; runtime allocation unmeasured | LOD0 | same root/tray pivots; no new motion | generated; final material FIX for pattern repetition and dark encoding; runtime audit pending | `assets/generated/3d/hub-view-drawer-r02/` | false |
| `SM_Hub_Workbench_Drawer` (r03) | same attachment; no new production identity | existing refs + `concept/t0-source-drawer-review.md` r03 T0 visual ACK; `modeling/pipeline.md` §16.5–16.7 | 1,500 [TARGET] | 156 triangles / 2 meshes [OBSERVED Blender + GLB + native Unity] | 4 × 1024² PNG [OBSERVED], 787,100 bytes on disk; native sRGB BaseColor / linear Roughness bindings verified; runtime GPU allocation unmeasured | LOD0 | unchanged source pivots; Unity yaw 180°, position (0, 0.32, 1.1), reviewed workbench-front AABB adaptation; no new motion | generated; T0 visual ACK and exact T0 runtime scene approval by director RFC-CX-003; player appearance / final production gates unmeasured | `assets/generated/3d/hub-view-drawer-r03/` | true — T0 scene only |

The preserved r01 construction recipe and target dimensions are `modeling/pipeline.md` §16.1–16.2; its actual generation receipt is now recorded above. The separate r02 material-correction recipe is §16.4. Both recipes pass Python syntax parsing; this alone proves no Blender execution or runtime outcome for r02. Modeler MCP access is unavailable, so the director executes the CLI candidate and returns receipts. OPEN-S11 still requires its runtime attachment/inspection check. The historical 144-triangle hub figure excludes the 156-triangle r01 attachment.

[OBSERVED 2026-09-10] r01 geometry counts come from its actual `measurements.json`; the 156-triangle attachment is separate from the historical 144-triangle hub receipt. r02 authoring and executable recipe are `modeling/pipeline.md` §16.4. The director authorized baked textures to address the concept material FIX; no new production identity, budget measurement, final style pass, Unity material pass or runtime promotion is asserted for an unexecuted recipe.

[OBSERVED 2026-09-10 — r03 receipt] Actual `hub-view-drawer-r03/` generation is verified: 2 meshes / 156 triangles, 2 GLB materials / 4 images, four 1024² sidecar PNGs totaling 787,100 bytes. All 9 output hashes and the executed recipe hash match provenance. `concept/t0-source-drawer-review.md` clears the four prior visual FIX items as a bounded **T0 visual candidate ACK**. Details are `modeling/pipeline.md` §16.6. The measured geometry matches r01; r01/r02 remain preserved. At this initial receipt, runtime eligibility was false pending Unity material/fit audit. The later exact T0 approval below supersedes that permission state; the original generation receipt and output hashes remain preserved.

[OBSERVED 2026-09-10 — r03 T0 runtime approval] Director RFC-CX-003 addendum approves the exact r03 candidate for **T0 scene only**, after native Metal/Gamma material and fit audit plus three diagnostic captures. The audit verifies 2 meshes / 156 triangles; four 1024² maps with BaseColor sRGB and Roughness linear; both renderer map bindings and supported shaders. Approval requires the measured Unity yaw **180°**, position **(0, 0.32, 1.1)** and reviewed workbench-front AABB adaptation. Current source provenance is **runtimeEligible:true**, promoted by `game-production-director; RFC-CX-003 r03 T0 runtime scene approval`. The audit’s false flag is the preserved pre-approval snapshot; current permission comes from the later director decision/provenance. See `modeling/pipeline.md` §16.7. Standalone-player appearance, final G4/G5, frame budgets and performance remain unmeasured. No generated source files, earlier revision or historical receipt were changed in this owner update.
