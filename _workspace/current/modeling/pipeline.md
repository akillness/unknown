---
updated: 2026-09-10
cycle: 20260909-preproduction-c4
status: current
supersedes: null
owner: game-modeler
---

# Modeling Pipeline — Blender → GLB(정본) + 허브 셸 FBX 1 → Unity

> 2026-09-10 T0 추가 작업: §16의 `hub-view-drawer`는 기존 허브 작업대의 별도 후보 부품이다. 이전 파일·47종 기준선·GLB 정본 판정은 유지한다. 실제 실행·측정이 기록되기 전 §16 값은 `[TARGET]`이다.

[OBSERVED] 이 문서의 "실행됨" 표기는 2026-09-10 세션에서 실제로 돌린 것만이다. Unity 임포트 실측 0건, tri/drawcall 런타임 실측 0건.
[INVARIANT] 여기서 나오는 모든 산출물은 **컨셉/프리비즈**다. 게임플레이 자산이 아니며 `runtimeEligible:false`로 시작한다.

## 1. 도구 사슬과 실행 상태

| 단계 | 도구 | 상태 | 증거 |
|---|---|---|---|
| 그레이박스·블록아웃·렌더 | Blender 5.1.2 (MCP `execute_blender_code`) | **실행됨** | `assets/generated/3d/scripts/build_hub_greybox.py`, `assets/generated/3d/provenance.json` |
| 내보내기 — **GLB 7(도구 6 + 허브 1) · FBX 1(허브 셸 참고)** | `bpy.ops.export_scene.gltf` / `bpy.ops.export_scene.fbx` | **실행됨** | provenance `export` 블록 · §6.1 |
| Unity 임포트 | Unity 6000.5.6f1 | **미실행** | `unity/Unknown/Assets/` 비어 있음 [OBSERVED] |
| 히어로 프롭 이미지→3D | Higgsfield (`multi_image_to_3d`) | **미실행(승인 전)** | 계약 `Asset pipeline` 행: CLI 설치됨·이 프로젝트 MCP 미등록·인증/크레딧 미확인 |
| 휴머노이드 리깅·모션 | Mixamo(웹) | **미실행(조건부 불필요)** | §8 |
| 2D(컨셉·UI·텍스처·키아트) | GTI `scripts/gen-2d.sh` | 이 레인 소관 아님 | `assets/README.md` |

[OBSERVED] Blender 애드온의 `get_objects_summary`는 이 연결에서 타임아웃한다. 씬 확인은 `get_scene_info` + `execute_blender_code` 안의 `bpy.data` 조회로 한다. 측정치는 이 경로로 얻은 계산값이며 추정이 아니다.

## 2. 단위·좌표·변환

- Blender: METRIC, `scale_length = 1.0`, 길이 단위 m, **1 blender unit = 1 m** [OBSERVED].
- Blender는 Z-up / -Y forward, Unity는 Y-up / +Z forward. 변환은 **내보내기에서 한 번만** 한다. 씬 안에서 오브젝트를 미리 눕히지 않는다.
- glTF: `export_yup=True` (glTF 규격 자체가 Y-up).
- FBX: `axis_up='Y'`, `axis_forward='-Z'`, `global_scale=1.0`, `apply_unit_scale=True`, `apply_scale_options='FBX_SCALE_NONE'`, `bake_space_transform=False`.
- 내보내기 전 **회전·스케일은 메시에 굽는다**(`transform_apply(location=False, rotation=True, scale=True)`). 위치는 굽지 않는다 — 위치를 굽으면 피벗이 월드 원점으로 끌려가 §3을 깬다. [OBSERVED] 현재 12개 메시 전부 rot=(0,0,0), scale=(1,1,1).

## 3. 피벗 규칙

| 대상 | 피벗 | 이유 |
|---|---|---|
| 도구 프롭 `SM_Tool_*` | **바닥 중심(도구 원점)** | `animation/animation-contract.md`의 `valve_turn / lever_throw / plate_insert / stamp_down`이 프롭 원점을 회전·삽입 기준으로 쓴다 |
| 벽·바닥·고정물 `SM_Hub_*` | 바닥 중심 | 층 높이 변경 시 z만 만지면 된다 |
| 개별 프롭 파일(GLB) | 월드 원점 | 프롭을 원점으로 옮겨 내보낸 뒤 씬 위치를 복원한다. 씬 배치는 Unity 프리팹이 갖고 메시는 로컬 좌표만 갖는다 |

[OBSERVED] 도구 6종의 씬 위치는 내보내기 후 원위치로 복원됐다(provenance `tool_locations_restored` 확인 경로 = `stage_70_export`).

## 4. 명명 규칙

```
SM_<zone>_<name>      정적 메시 (Static Mesh)
SM_Tool_<toolId>      도구 프롭 — toolId 는 캐논 6종만
MSH_<objectName>      메시 데이터블록
MAT_<Group>_<name>    재질
CAM_<zone>_<role>     카메라
LGT_<zone>_<role>     라이트
```

**toolId 6종은 캐논이며 새로 만들지 않는다** [OBSERVED `planning/gdd.md` L87 — `campaign.json` 도구 id와 1:1]:
`circuit` · `reader` · `alignment` · `routing` · `corrosion` · `seal`.

**zone 토큰** — 제작 라벨이며 in-fiction 고유명사가 아니다. 고유명사는 **용어집에 등록된 것만** 파일명·오브젝트명에 쓴다(`worldview/glossary.md`: "여기에 없는 고유명사는 … 에셋 파일명에 쓸 수 없다"). 등록 여부와 무관하게 **상표 미확인 가제 문자열(기관 가제명·영문 코드네임)** 은 파일명·오브젝트명·이미지 내 텍스트에 넣지 않는다(계약 `Unity / 저장소 배치` 조항).

| zone 토큰 | 용어집 대응 (KO / EN) | 비고 |
|---|---|---|
| `Hub` | 당직실 / Watch Room | 허브. `zoneId` `hub` 의 표기 변형 — glossary §7 파생 규칙으로 **유효 확정** |
| `Gate3` | 제3수문 / Gate Three | |
| `Lowland` | 구염전 저지대 / Old Saltern Lowland | |
| `Quay` | 냉동창고 부두 / Cold Quay | |
| `Pump1` | 제1양수장 / Pump House One | |
| `Kit` | (공용 모듈, in-fiction 아님) | 전역 공유 소품 |

- 로케일 문자열·간판 텍스트를 메시나 텍스처에 굽지 않는다(`concept/art-direction.md`: "영어 간판을 텍스처에 굽지 않는다").
- **OPEN-M1 — closed (2026-09-10).** [OBSERVED] `worldview/glossary.md` §7 파생 규칙(L152~154)이 판정했다: "에셋 오브젝트명·파일명의 zone 토큰은 `zoneId` 영문 토큰을 그대로 쓰는 것을 허용" · "`modeling/pipeline.md`의 `Hub`는 **그대로 유효** … **OPEN-M1 닫힘**". `Watch`로의 일괄 치환은 요구되지 않으며 파일명 `hub-greybox.*`도 그대로 둔다.
- 읽는 법(glossary L154): `Hub`↔`hub` · `Gate3`↔`gate` · `Pump1`↔`pump` · **`Quay`↔`dock`** · `Lowland`↔`lowland`. 위 표의 5개 토큰은 계속 쓰되, **새로 만드는 zone 토큰은 `zoneId` 값을 그대로 쓴다**(계보를 더 늘리지 않기 위해). 이 규칙을 §4 명명 규칙의 일부로 취급한다.

## 5. 폴더 배치와 승격

```
assets/generated/3d/
  hub-greybox.blend            씬 소스(새 파일. 사용자 기본 씬을 덮어쓰지 않았다)
  hub-greybox.glb              허브 전체 — **런타임 플레이스홀더 정본**
  hub-greybox.fbx              허브 셸 **참고 1개**(교환·대조용. 도구 프롭에는 FBX 를 만들지 않는다 — §6.1)
  SM_Tool_<toolId>.glb         도구 프롭 개별 6종(원점 정렬) — **런타임 플레이스홀더 정본**
  renders/hub-cam.png          고정 카메라 프리비즈
  renders/turntable/frame_NN.png
  scripts/build_hub_greybox.py 실제 실행한 소스
  scripts/write_provenance.py  provenance 생성기
  provenance.json              손으로 쓰지 않는다(assets/README.md)
assets/generated/archive/<YYYY-MM-DD>/...   재생성으로 대체된 이전 산출물 (§14). 읽기 전용 역사
unity/Unknown/Assets/          승격 대상. 지금은 비어 있다
```

승격(= `runtimeEligible:false → true` + `unity/Unknown/Assets/` 복사)은 `production/decision-log.md` 감사로만 한다. 모델러가 임의로 올리지 않는다(CLAUDE.md §9).

승격의 **실행 순서·감사 항목·Unity 임포트 설정·재현 명령**은 `handoff/asset-runbook.md`(C7, `status: draft`) §3·§7 이 잇는다. 규격 정본은 계속 이 문서이고, 런북은 절차만 소유한다 — 충돌하면 이 문서가 이긴다.

**대체(재생성)는 삭제가 아니다.** 기존 산출물을 다시 뽑아 같은 경로에 쓰는 모든 경로는 §14 의 비파괴 재생성 계약을 **선행**해야 한다. 보존 영수증 없이 덮어쓴 산출물은 `assets/` 가 git 미추적이므로 복구 수단이 남지 않는다.

## 6. 내보내기 설정 (실제 인자 [OBSERVED])

**glTF/GLB** — `bpy.ops.export_scene.gltf(export_format='GLB', use_selection=True, export_apply=True, export_yup=True)`
**FBX** — `bpy.ops.export_scene.fbx(use_selection=True, global_scale=1.0, apply_unit_scale=True, apply_scale_options='FBX_SCALE_NONE', axis_forward='-Z', axis_up='Y', bake_space_transform=False, object_types={'MESH'}, use_mesh_modifiers=True, mesh_smooth_type='FACE', path_mode='COPY')`

Blender 5.1에는 새 익스포터 `bpy.ops.wm.fbx_export`도 있다 [OBSERVED]. 이번에는 인자 의미가 검증된 `export_scene.fbx`를 썼다. 익스포터를 바꾸면 provenance의 `export.operator`가 바뀌고 Unity 재검증이 필요하다.

### 6.1 포맷 역할 분담 — 정본 1개 (C7-F3 해소 · 디렉터 판정 ⑦)

[OBSERVED 2026-09-10 · `production/decision-log.md` 「C6-F11 / PRE-1 / C7-F14 / C7-F12 / C7-F3」 ⑦] 디렉터 판정:
**GLB 가 런타임 플레이스홀더 정본이다(도구 6종 · 허브 1). FBX 는 허브 셸 참고용 1개만 두며 `SM_Tool_*.fbx` 는 만들지 않는다.**

| 포맷 | 파일 | 역할 | 만드는가 |
|---|---|---|---|
| GLB | `SM_Tool_<toolId>.glb` ×6 · `hub-greybox.glb` | **Unity 임포트 대상 정본**(T0 플레이스홀더) | 예 — `stage_70_export()` 가 이미 낸다 |
| FBX | `hub-greybox.fbx` ×1 | 허브 셸 **참고**(축·스케일 대조, 외부 DCC 교환) | 유지만. 재생성 시에도 1개 |
| FBX | `SM_Tool_<toolId>.fbx` | — | **아니오. 만들지 않는다** |

[OBSERVED 재현 · 2026-09-10] `ls -1 assets/generated/3d/ | grep -E '\.(glb|fbx|blend)$'` →
`SM_Tool_{alignment,circuit,corrosion,reader,routing,seal}.glb` · `hub-greybox.{blend,fbx,glb}` = **GLB 7 · FBX 1 · blend 1**. 판정과 현재 산출물이 일치하므로 **추가 내보내기 작업은 0건**이다.

이전 판의 「승격 포맷 FBX 단일화」[INFERENCE] 제안은 **폐기**된다(런북 §6-5 도 함께 닫혔다). GLB 를 Unity 로 읽으려면 임포터 패키지(glTFast 계열)가 필요할 수 있고, **패키지 추가는 실행자가 임의로 하지 않는다** — `handoff/README.md` §2 「반드시 되물어야 하는 것: 패키지 추가」에 따라 RFC 로 되묻는다(§6.2 미결).

### 6.2 미결 — GLB 임포트 경로 [OPEN · systems·director]

Unity 6000.5.6f1 이 `.glb` 를 **내장 임포터로** 읽는지 이 세션에서 확인하지 못했다(`unity/Unknown/Assets/` 파일 0 [OBSERVED]). 첫 임포트에서 실행자가 다음 둘 중 하나를 실측하고 결과를 `systems/tech-verification/` 영수증으로 남긴다:
(a) 내장 임포트 성공 → 패키지 추가 없음. (b) 실패 → `com.unity.cloud.gltfast` 등 패키지 추가가 필요 → **RFC 제출 후 디렉터 판정**(§6.1 마지막 문단). 어느 쪽이든 `SM_Tool_*.fbx` 를 새로 만드는 것으로 우회하지 않는다 — 그것은 판정 ⑦ 위반이다.

## 7. Unity 임포트 규격 [TARGET — 임포트 실측 0건]

Unity **6000.5.6f1** [OBSERVED `unity/Unknown/ProjectSettings/ProjectVersion.txt`].

| 항목 | 값 | 이유 |
|---|---|---|
| Scale Factor | 1 | Blender 1 unit = 1 m로 내보냈다 |
| Convert Units | on | **FBX 에만 해당**(내부 cm 단위를 m로 되돌린다). GLB 는 glTF 규격상 m 단위라 이 항목이 없다 |
| Bake Axis Conversion | off | 이미 `axis_up='Y'`로 내보냈다. 이중 변환 금지 |
| Import Cameras / Lights | off | 조명·카메라는 Unity 씬이 소유한다. 카메라 값은 §9 표로 옮긴다 |
| Read/Write Enabled | off | 메모리 2배. 켜야 할 근거가 생기면 그때 |
| Mesh Compression | Off | 그레이박스는 12 tris/오브젝트다. 압축 이득 없음 |
| Generate Colliders | off | 콜라이더는 §10대로 수동 박스 |
| Material Creation Mode | None | 프로젝트 재질을 쓴다. 임포트 재질이 아트디렉션 색을 덮지 않게 한다 |
| Normals / Tangents | Import / Calculate Mikk | 그레이박스는 face smooth |
| Animation | off | 이 자산군에 클립 없음 |

위 표는 FBX(`hub-greybox.fbx`) 기준이다. **정본인 GLB 7개**는 §6.1 대로 임포트하며, 임포터가 노출하는 항목만 같은 값으로 맞춘다(Scale 1 · Import Cameras/Lights off · Read/Write off · Material Creation None 상당). GLB 임포트 경로 자체가 §6.2 미결이다.

첫 임포트 시 반드시 측정할 것: 실제 tri/vert, drawcall(SRP Batcher 포함), 텍스처 상주. 그 전까지 G5는 `[TARGET]`이며 통과할 수 없다.

## 8. Mixamo 리깅 규격 — 조건부이며 현재 미실행

[OBSERVED] **현재 설계에 3D 휴머노이드는 0체다.** `concept/art-direction.md`는 "인물은 2D 초상", `animation/animation-contract.md`는 "캐릭터 locomotion 0 / 아바타·보행·NavMesh 없음", `modeling/asset-budget.md`는 초상 5×3을 "2D, 별도 리깅 없음"으로 못박는다. 따라서 Mixamo는 **지금 필요 없다**. 사용자 지시(리소스는 Mixamo로)를 이 설계와 충돌 없이 지키는 방법은 "휴머노이드가 생기면 Mixamo가 유일 경로"로 규격만 고정해 두는 것이다. 3D 인물을 만들지 않기로 한 결정을 모델러가 뒤집지 않는다 → **OPEN-M2**로 디렉터에 올린다.

휴머노이드 5인이 승인될 경우의 납품 규격 [INFERENCE — 이번 세션에 Mixamo 접속·검증 0회]:

| 항목 | 규격 |
|---|---|
| 업로드 포맷 | FBX (binary), 단일 메시 권장. 기존 스켈레톤·모디파이어 없이 |
| 포즈 | **T-pose**, 팔 수평, 손바닥 아래, 다리 어깨너비 |
| 스케일 | 1 blender unit = 1 m, 캐릭터 신장 1.6~1.9 m. 오브젝트 스케일 apply 필수 |
| 좌표 | Y-up, +Z front (§2 FBX 설정 그대로) |
| 정점 웨이트 | 자동 리깅 산출. Unity Humanoid 상한에 맞춰 정점당 ≤4 본 |
| 손가락 | 확대뷰에 손이 나오면 finger joints on, 아니면 off |
| 다운로드 | 애니 팩은 **사용자가 수동으로** 받는다. 공개 API 없음 → 에이전트 자동화 불가 |
| Unity | Rig → Humanoid, Avatar Definition → Create From This Model |
| provenance | Mixamo 산출물도 `runtimeEligible:false` + 라이선스 표기(Adobe 약관 확인은 사용자 몫) |

[OBSERVED · 2026-09-10 R4 재측정] `animation/rig-requirements.md`는 **존재한다**(21.6 KB · `status: draft` · `cycle: 20260909-preproduction-c5` · owner `game-animator`). 앞선 판의 "아직 없다 [OBSERVED]"는 스테일 주장이었다(C4-F10 과 같은 유형 — QA 미지목분, 이 레인이 자율 정정). 그 문서 §5 가 Mixamo 조건부 규격을, §7 이 모델링 인계 체크리스트(ack 요청)를 담고 있다. 다만 `status: draft`이므로 **이 표를 아직 그 파일 기준으로 재정렬하지 않았다** — 대조·ack·편차 기록은 R5(C5) 작업이며 현재 편차 검사 실적은 **0건**이다.

## 9. 2.5D 고정 카메라 값 (Unity 재현용) [OBSERVED]

| 항목 | 값 |
|---|---|
| 카메라 | `CAM_Hub_Fixed`, perspective |
| 위치 (Blender Z-up, m) | `(0.0, -5.30, 5.501)` |
| 조준점 | 작업대 상면 중심 `(0.0, 1.20, 0.95)` |
| 부감각 | 35.0° |
| 초점거리 | 34 mm (36 mm 센서 기준) |
| Unity 변환 | Y-up 좌표로 `(x, z, y)` = `(0.0, 5.501, -5.30)`, 조준점 `(0.0, 0.95, 1.20)` |

렌즈를 40 mm에서 34 mm로 내린 이유: 40 mm에서는 부식 시험대와 염판 선반이 프레임 밖으로 잘려 "당직실은 6개 도구를 같은 프레임에서 가르친다"(`concept/art-direction.md`)를 만족하지 못했다. 렌더로 확인 후 변경 [OBSERVED].

## 10. 콜리전 · LOD · 텍스처 예산

- **콜리전**: 단순 박스/캡슐만. 메시 콜라이더 금지. 그레이박스는 형상 자체가 박스라 1:1.
- **LOD**: 카메라가 고정 2.5D이고 거리 변화가 없다 → **LOD0 단일**을 기본으로 한다. LOD는 거리 변화가 있는 자산(구역 전환 시 원경으로 남는 셸)에만 만들고 감축 목표는 LOD1 50% / LOD2 25% [TARGET]. 실측 전에는 LOD를 미리 만들지 않는다(만들면 예산만 늘고 이득 0).
- **텍스처** (`modeling/asset-budget.md` 인용): 원거리 1K / hero 2K 후보 / **4K 기본값 금지**. 뷰당 가시 삼각형 ≤300k · 드로콜 ≤150 · 텍스처 상주 ≤512 MiB — 전부 [TARGET], 측정 0건.

## 11. Higgsfield 이미지→3D (선택 경로, 승인 전 미실행)

- 용도: **히어로 프롭 후보** 1종을 컨셉 이미지 1~4장으로 뽑아 형태 탐색에 쓴다. 공간 셸·구조물에는 쓰지 않는다(스케일·피벗·토폴로지 통제 불가).
- 크레딧을 소모하므로 실행 전 계정/크레딧 확인 → 실패 시 `skipped` 영수증. 디렉터 승인 없이 실행하지 않는다.
- 산출물은 반드시: 리토폴로지 여부, 원본 이미지 경로+해시, 모델명, 라이선스 `UNVERIFIED`, `runtimeEligible:false`.
- Blender MCP에는 Hyper3D/Rodin·Hunyuan3D 생성 도구도 노출돼 있으나 이 프로젝트에서 승인된 경로가 아니다. 상태 조회조차 하지 않았다 [미측정].

## 12. Provenance 규칙

- `assets/generated/3d/provenance.json`은 **생성기로만** 쓴다: `scripts/write_provenance.py`. 손으로 편집 금지(`assets/README.md`).
- 필수 필드: `id / file / asset_id / tool / method / source_script / sha256 / bytes / generated_at / license / runtimeEligible(false) / promoted_by(null) / claim`.
- `claim`은 `[OBSERVED] greybox; not final art; not gameplay`처럼 관측 등급을 앞에 붙인다.
- 파일이 바뀌면 생성기를 다시 돌린다. 해시가 파일과 다른 provenance는 없는 것과 같다.
- **provenance 항목도 산출물이다.** 재생성이 같은 `id` 의 이전 항목을 지우면 옛 파일의 모델·프롬프트 해시·생성 시각·출력 해시가 함께 사라진다. 항목 수가 그대로여도 손실이다 → §14.
- 아카이브에 남기는 provenance 는 **파일 통째 복사**(`provenance.before-r<NN>.json`)다. 복사는 손편집이 아니므로 §12 첫 줄의 "생성기로만 쓴다"를 어기지 않는다. 아카이브 안의 JSON 도 이후 편집 금지.

## 13. 검증 체크리스트 (내보내기 전 매번)

0. **이번 실행이 기존 산출물을 덮어쓰는가**(`FORCE=1`, 같은 경로 재-export, `save_as_mainfile`) → **먼저 §14**. 보존 영수증(해시 2줄) 없이 덮어쓰기를 시작하지 않는다.
1. 씬 조사 먼저 — `get_scene_info`. 사용자 오브젝트는 **숨기고 삭제하지 않는다**.
2. 이름이 §4 규칙과 캐논 toolId를 지키는가. `.001` 접미사가 붙은 중복이 없는가.
3. 모든 메시의 rotation/scale이 identity인가. 위치(피벗)가 §3인가.
4. 삼각형 수를 **세었는가**(폴리곤 팬 합). 추정치를 예산 표에 쓰지 않는다.
5. **GLB 7개**(도구 6 원점 정렬본 + 허브 1)를 냈는가. 허브 셸 FBX 는 **1개뿐**인가. `SM_Tool_*.fbx` 를 만들지 않았는가(§6.1 · 디렉터 판정 ⑦).
6. 렌더를 **눈으로 봤는가**. 프레이밍이 요구(6도구 동시 가시)를 만족하는가.
7. `write_provenance.py` 재실행 → 해시 갱신.
8. 새 `.blend`로 저장했는가(사용자의 미저장 기본 씬을 덮지 않았는가).

## 14. 재생성 시 원본 보존 — 비파괴 재생성 계약 (C7-F13 해소)

[INVARIANT] **어떤 재생성 경로도 이전 산출물과 그 provenance 항목을 소멸시키지 않는다.** CLAUDE.md §2("삭제는 없다 · 이전 작업은 사라지지 않고 항상 참조 가능해야 한다")는 `_workspace/` 문서 전용 규칙이 아니라 이 레인이 만든 파일에도 그대로 적용된다. 이 절이 그 적용 방법을 소유한다.

### 14.1 무엇이 깨져 있었는가 [OBSERVED · 2026-09-10 재현]

| 경로 | 관측한 코드/명령 | 결과 |
|---|---|---|
| `scripts/gen-2d.sh:19` | `if [ -f "$OUT" ] && [ "${FORCE:-0}" != "1" ]; then echo "skip (exists)"; exit 0; fi` | `FORCE=1` 이면 존재 검사를 건너뛰고 L20 `gti … --output "$OUT"` 이 같은 경로를 **덮어쓴다** |
| `scripts/gen-2d.sh:29` | `data["assets"] = [a for a in data["assets"] if a["id"] != aid]` | 같은 `id` 의 **이전 provenance 항목이 삭제**된다 → 옛 이미지의 모델·프롬프트 해시·요청/실제 크기·생성 시각·`output_sha256` 이 함께 사라진다 |
| `assets/generated/3d/scripts/build_hub_greybox.py:347` | `stage_90_save()` → `bpy.ops.wm.save_as_mainfile(filepath=…/hub-greybox.blend)` | 씬 소스 `.blend` 를 **덮어쓴다**. 같은 실행의 export(L280·L285·L301)·render 단계가 `.glb/.fbx/renders/*` 도 덮어쓴다 |
| `git ls-files assets \| wc -l` | **0**. 그리고 `.gitignore` 에 `assets/` 항목 **없음** — 무시된 것이 아니라 한 번도 add 된 적이 없다 | 덮어쓴 파일은 **git 이력으로도 복구되지 않는다** |

즉 `handoff/asset-runbook.md` §4.4 의 `FORCE=1 …` 세 줄을 그대로 실행하면 2D 45장 중 3장의 원본과 그 provenance 3항목이 **동시에** 사라진다. 재생성 후에도 "PNG 45 ↔ provenance 45항목"의 1:1 은 유지되므로([OBSERVED] 오늘 값 = concept 21 · previz 9 · readme 7 · ui 4 · capsule 2 · keyart 2 = **45**, PNG `find` 결과와 일치) 수량 검사는 통과한다. **손실이 없다는 뜻이 아니라 손실이 보이지 않는다는 뜻이다.**

### 14.2 아카이브 레이아웃

```
assets/generated/archive/<YYYY-MM-DD>/<generated 하위 상대경로>/<asset-id>.r<NN><확장자>
assets/generated/archive/<YYYY-MM-DD>/<generated 하위 상대경로>/provenance.before-r<NN>.json
assets/generated/archive/<YYYY-MM-DD>/3d/r<NN>/                (3D 는 폴더 통째 · §14.4)
```

- `<generated 하위 상대경로>` = 원래 위치에서 `assets/generated/` 를 뗀 값(`2d/concept` · `2d/ui` · `3d` · `video` · `previz`). 폴더 구조를 보존해야 파일만 보고도 **어느 카테고리의 어느 id 였는지** 복원된다.
- `<NN>` = 그 날짜·그 asset-id 기준 2자리 순번, `01` 부터. 같은 날 두 번 재생성해도 충돌하지 않는다.
- **이동이 아니라 복사**다(§5 승격 규칙과 같은 원칙). 원본 경로의 파일은 재생성이 덮어쓸 때까지 그대로 둔다 — 복사 직후 재생성이 실패해도 원본이 자리에 있다.
- 아카이브는 `_workspace/archive/` 와 같은 취급이다: **읽기 전용 역사**. 편집·삭제·재사용 금지, 인용만 한다.
- 아카이브 파일은 **승격 대상이 아니다**. `runtimeEligible` 은 영원히 `false` 이며 `unity/Unknown/Assets/` 로 복사되지 않는다.
- [OBSERVED] 오늘 `find assets -type d -name 'archive*'` = **0건**. 아직 어떤 재생성도 실행되지 않았으므로 **현재까지 실제 손실은 없다** — 이 절은 손실 발생 전에 잠근 것이다.

### 14.3 절차 A — 2D 재생성 (오늘 실행 가능 · 스크립트 수정 불필요)

`scripts/gen-2d.sh` 를 고치지 않고도 보존이 성립한다. 1~3단계는 **복사와 해시뿐**이며 provenance 를 손으로 편집하지 않는다.

```bash
# 0) 사유가 프롬프트에 있으면 프롬프트를 먼저 고친다(concept 레인 소유). 그 전에는 같은 그림이 다시 나온다.
ID=space-gate-three-mood; CAT=concept; DATE=$(date +%F)
SRC="assets/generated/2d/$CAT/$ID.png"
DST="assets/generated/archive/$DATE/2d/$CAT"

# 1) 순번 계산 + 아카이브 폴더 생성
mkdir -p "$DST"
COUNT=$(ls "$DST" 2>/dev/null | grep -c "^$ID\.r" || true)
N=$(printf "%02d" $((COUNT + 1)))

# 2) 원본 이미지와 provenance 를 통째로 복사(-p 로 mtime 보존)
cp -p "$SRC" "$DST/$ID.r$N.png"
cp -p "assets/generated/2d/$CAT/provenance.json" "$DST/provenance.before-r$N.json"

# 3) 보존 영수증 — 이 두 줄이 없으면 4)를 실행하지 않는다
shasum -a 256 "$DST/$ID.r$N.png" "$DST/provenance.before-r$N.json"

# 4) 이제서야 재생성
FORCE=1 scripts/gen-2d.sh "$ID" "$CAT" 1536x1024 "_workspace/current/concept/prompts/$ID.txt"

# 5) 검증 (§14.7)
```

[OBSERVED · 2026-09-10] 위 1~3단계를 **스크래치패드의 모의 트리**에서 그대로 실행해 확인했다(저장소 파일 생성 0건 — 아직 실제 재생성 승인이 없다): 같은 날 두 번 돌렸을 때 순번이 `r01` → `r02` 로 증가하고, 스냅샷 재판독 `python3 … provenance.before-r01.json` 이 옛 항목의 `output_sha256` 을 그대로 돌려줬다. 명령의 문법·순번 로직은 검증됐고, **GTI 재생성 자체(4단계)는 실행 0회**다.

### 14.4 절차 B — 3D 씬 재빌드 (`build_hub_greybox.py` 재실행 전)

3D 는 한 번의 실행이 `.blend`·`.glb`·`.fbx`·`renders/*`·`provenance.json` 을 함께 덮어쓰므로 **파일 단위가 아니라 폴더 단위**로 보존한다.

```bash
DATE=$(date +%F); DST="assets/generated/archive/$DATE/3d"
COUNT=$(ls -d "$DST"/r* 2>/dev/null | wc -l | tr -d ' ' || true)
N=$(printf "%02d" $((COUNT + 1)))
mkdir -p "$DST/r$N" && cp -Rp assets/generated/3d/. "$DST/r$N/"
find "$DST/r$N" -type f | wc -l                      # 영수증 1 — 파일 수
shasum -a 256 "$DST/r$N/hub-greybox.blend" "$DST/r$N/provenance.json"   # 영수증 2
```

- 스냅샷에 `scripts/` 도 포함한다. 재빌드의 원인이 스크립트 변경일 때 **옛 스크립트가 없으면 옛 결과를 재현할 수 없다**.
- Blender 쪽 규칙은 §13-8 과 함께 읽는다: 사용자 씬을 덮지 않는 것과 **우리 씬의 이전 판을 덮지 않는 것**은 다른 문제이며 둘 다 지킨다.
- 렌더만 다시 뽑는 경우(`renders/` 만 변경)에도 같은 절차를 쓴다. 부분 보존은 하지 않는다 — 어느 파일이 바뀔지 실행 전에 확정할 수 없다.

### 14.5 provenance 보존 스키마 [TARGET — 생성기 패치 후]

절차 A/B 는 옛 항목을 **폴더 밖 스냅샷**으로만 보존한다. live `provenance.json` 안에서 계보가 읽히게 하려면 생성기 패치가 필요하고, 그 파일들(`scripts/gen-2d.sh` · `assets/generated/3d/scripts/write_provenance.py`)은 이 레인 소유가 아니다 → **RFC-M6**. 요청 스키마는 다음으로 고정한다.

| 대상 | 필드 | 값 |
|---|---|---|
| 대체된 옛 항목 (삭제하지 않는다) | `status` | `"superseded"` |
| | `archivedTo` | `"archive/<date>/2d/<cat>/<id>.r<NN>.png"` (repo 상대) |
| | `supersededBy` | 새 항목의 `output_sha256` |
| 새 항목 | `status` | `"current"` |
| | `supersedes` | 옛 항목의 `output_sha256` + `archivedTo` 경로 |
| | `regeneration` | `{ "n": <NN>, "reason": "<한 줄>", "decidedBy": "<RFC id 또는 결함 id>" }` |

- **중복 검사 규칙**: 같은 `id` 는 `status != "superseded"` 인 항목이 **정확히 1건**이어야 한다. `gen-2d.sh:29` 의 무조건 제거는 이 조건부 검사로 바뀐다. 이렇게 하면 기존 소비자(항목 수·해시 대조)가 깨지지 않는다 — 다만 "PNG 수 = 항목 수" 검사는 "PNG 수 = `status != superseded` 항목 수"로 함께 바뀌어야 한다.
- 패치 전까지 스냅샷이 **유일한** 옛 항목 보관소다. 그래서 절차 A 의 2단계 두 번째 `cp` 는 선택이 아니다.

### 14.6 잠금 상태 — 런북 §4.4 재생성 권고 3장

| 대상 | 현재 상태 | 해제 조건 |
|---|---|---|
| `space-gate-three-mood` (1순위) | **이중 잠금** | §14.3 절차 A 선행 **+** 프롬프트 NEGATIVE 절 선수정(런북 §6-3 · concept 레인 소유). 프롬프트를 고치지 않은 `FORCE=1` 은 같은 성곽 모티프를 다시 뽑을 수 있다 [INFERENCE] |
| `tool-alignment-hero` (2순위) | **잠금** | §14.3 절차 A 선행 |
| `tool-corrosion-hero` (3순위) | **잠금** | §14.3 절차 A 선행(+ 육각 결정은 SUBJECT 절 수정이 근본 해법 — concept 레인) |
| 그 밖의 모든 `FORCE=1` 2D 재생성 | **잠금** | §14.3 절차 A 선행 |
| `build_hub_greybox.py` 재실행 | **잠금** | §14.4 절차 B 선행 |

**읽는 법**: 런북 §4.4 표의 `FORCE=1 …` 명령은 **보존 선행 없이는 실행 불가**다. 런북 머리말이 "이 문서가 소유하지 않는 것 … 저작 규격(`modeling/pipeline.md`) … 충돌하면 그 문서가 이긴다"를 명시하므로, 런북 문장이 이 조건을 담을 때까지 §4.4 는 이 절의 조건이 붙은 것으로 읽는다. 런북 §4.4·§1.1 문장 자체의 정정(§1.1 "FORCE=1 이면 기존 파일을 덮어쓰고 provenance 를 같은 id 로 교체한다(항목 수 유지)" 는 사실 서술이지만 **경고가 없다**)은 `handoff/` 쓰기 권한을 가진 쪽이 수행한다 → **RFC-M6**.

절차 A/B 를 실행하면 그 asset-id(또는 3D 씬)에 한해 잠금이 풀린다. **전면 동결이 아니다** — 보존이 재생성의 선행 단계일 뿐이다.

### 14.7 재생성 후 검증 (매번, 실행자가 그대로 붙여 넣는다)

```bash
# V1) 새 파일과 아카이브 파일의 해시가 달라야 한다. 같으면 재생성이 일어나지 않은 것이다(백엔드 실패/캐시).
shasum -a 256 "assets/generated/2d/$CAT/$ID.png" "$DST/$ID.r$N.png"

# V2) 스냅샷이 옛 항목을 실제로 담고 있는가 (출력은 옛 PNG 의 output_sha256 1건 — 비어 있으면 실패)
python3 -c "import json,sys;d=json.load(open(sys.argv[1]));print([a['output_sha256'] for a in d['assets'] if a['id']==sys.argv[2]])" "$DST/provenance.before-r$N.json" "$ID"

# V3) live provenance 항목 수 불변 (2D 6폴더 합 = PNG 수)
find assets/generated/2d -name '*.png' | wc -l
for f in $(find assets/generated/2d -name provenance.json); do python3 -c "import json,sys;print(len(json.load(open(sys.argv[1]))['assets']))" "$f"; done

# V4) 아카이브가 손상되지 않았는가 — §14.3 3단계 영수증과 두 줄 모두 문자 일치해야 한다
shasum -a 256 "$DST/$ID.r$N.png" "$DST/provenance.before-r$N.json"
```

- V1 두 해시가 같으면 **재생성 실패로 기록**하고 provenance 를 손대지 않는다(`skipped` 영수증).
- V2 가 빈 리스트를 내면 스냅샷을 잘못된 시점에 떴다는 뜻이다. V2 의 해시는 §14.3 3단계 영수증의 **아카이브 이미지 해시와는 다른 값**이다(전자는 옛 PNG 의 `output_sha256`, 후자는 아카이브 사본 파일의 해시) — 두 값을 같다고 기대하지 않는다. 재생성은 되돌릴 수 없으므로 V2 가 비면 **그 자리에서 결함으로 올린다**.
- V3 은 재생성 전후로 값이 같아야 한다(§14.5 패치 전 기준: PNG 수 = 항목 수 합).
- [OBSERVED · 2026-09-10] V4 를 처음에 `find assets/generated/archive -newer <원본> -type f` 로 적었으나 **오탐 1건**이 났다. `cp -p` 가 mtime 을 보존하므로 아카이브 사본의 mtime 이 원본 png 보다 나중일 수 있다(같은 폴더 provenance.json 의 mtime 을 물려받는 경우). mtime 기반 불변 검사는 이 절차에서 성립하지 않는다 → 해시 비교로 교체했다.

### 14.8 이 절이 해결하지 않는 것

- **git 이력 보존이 아니다.** 아카이브는 파일시스템 보존이며, `assets/` 는 여전히 git 미추적(0 파일)이다. 저장소 clone 에는 아카이브도 원본도 없다. 추적 여부(용량·LFS 포함)는 사용자·디렉터 판정 사항이고 commit/push 는 사용자만 수행한다(CLAUDE.md §8, 계약 Release safety) → **OPEN-M7 / RFC-M5**.
- **자동화가 아니다.** 절차 A/B 는 사람이 순서를 지켜야 성립한다. 생성기 자체가 보존을 수행하게 만드는 것이 근본 해법이다 → RFC-M6.
- **`docs/media/` 파생본**은 이 절의 대상이지만 스키마 결손이 별도로 열려 있다(런북 §6-4, 소유자 미정). 파생본 재생성 시에도 §14.2 레이아웃을 그대로 쓴다.

## 15. 변경 로그 (RFC-Q2 — 같은 `cycle` 안의 제자리 갱신 = 개정)

| 일자 | 개정 | 근거 |
|---|---|---|
| 2026-09-10 (C6/C7 수정 루프 1) | §14 비파괴 재생성 계약 신설(아카이브 레이아웃·절차 A/B·provenance 보존 스키마·잠금 표·검증 4종), §5 폴더 배치에 `archive/` 추가 + 대체 선행 조건 1문단, §12 provenance 항목도 산출물임을 명시, §13 체크리스트 0번 신설 | `qa/c6-review.md` §3 C7-F13(S2) · `qa/defect-register.md` L341 · CLAUDE.md §2 |

`cycle` 값(`20260909-preproduction-c4`)·`status`(`current`)·`supersedes`(`null`)는 바꾸지 않았다 — 같은 사이클 안의 제자리 개정이므로 아카이브 의무가 발생하지 않는다(RFC-Q2). §1~§13 의 규격 수치(단위·피벗·명명·내보내기 인자·카메라 값·예산)는 이번 개정에서 **한 건도 바뀌지 않았다**.


## 16. T0 hub-view-drawer candidate — original Blender attachment

### 16.1 Identity, concept and scope

Director authority: `production/decision-log.md` RFC-CX-003 records the workbench attachment and FBX import-candidate exception; GLB remains canonical and runtime promotion remains a separate audit.

[OBSERVED 2026-09-10] `handoff/README.md` OPEN-S11 identifies `hub-view-drawer` without a mesh. `animation/anim-list.md` still reports `drawer_open` without a target. The baseline has 47 production identities and no drawer row. The candidate is a **subassembly of the existing `SM_Hub_Shell` / `SM_Hub_Workbench`**, named `SM_Hub_Workbench_Drawer`; it does not assert a 48th completed production asset. `Hub` is a glossary-derived technical zone token (pipeline §4), and `Workbench_Drawer` is a descriptive component name, not a new in-fiction proper name.

- `concept_ref`: `concept/sheets/README.md` §2 `space-hub-watchroom-mood` and §4 `ui-workbench-frame`; `concept/prompts/ui-workbench-frame.txt` explicitly describes shallow evidence drawers with pull handles. These are contextual references, **not a dedicated approved prop sheet**. Final-art approval remains open.
- Existing parent geometry: `SM_Hub_Workbench`, base pivot `(0, 1.20, 0)` m in Blender, dimensions `2.40 × 0.90 × 0.90` m, top `z=0.90` (`modeling/specs/hub-watchroom.md` §2). These are historical measured anchors, not this candidate's measurements.
- Target envelope: `1.10 × 0.70 × 0.26` m; attach at Blender `(0, 1.16, 0.54)` m. This places the closed attachment inside the workbench's existing bounding volume. A final integration must replace/cut the solid workbench front or display the attachment in its own inspection view; simply overlapping the original solid box hides it.
- Root pivot: bottom centre. Local Blender `-Y` faces the player; a separate rigid tray can translate along `-Y`. **No open travel or clip timing is chosen here**: the authored 180 ms record-drawer value may be UI-only (RFC-A4 remains relevant), and animation owns physical travel/timing.
- Material slots: casing `#36565C`, inset/tray `#173238`, pull handle `#4F7A6B` from `concept/style-guide.md` §2. No baked text, emissive glow, skeleton, animation, hidden puzzle coordinates, or additional narrative contents.
- Collision: target one box for the casing and an optional child box for a moving tray; engine integration owns collider fitting. LOD0 only; fixed observation camera.
- Budget: inherit **1,500 triangles [TARGET]** as the existing small-prop ceiling (§4 of `asset-manifest.md`), **0 texture files** for this material-only greybox. Scene target remains 300k triangles / 150 draw calls / 512 MiB; Blender object counts are never treated as draw-call measurements.

```yaml
asset_id: SM_Hub_Workbench_Drawer
parent_asset_id: SM_Hub_Shell
interaction_node: hub-view-drawer
status: spec-ready
runtimeEligible: false
concept_approval: contextual-greybox-only
tri_budget_target: 1500
tri_measured: null
material_slots_target: 3
texture_files_target: 0
lod_levels: [LOD0]
rig_ref: animation/rig-requirements.md section 2.1
rig_deviation: no skeleton; rigid root and one tray pivot only; clip authoring pending
candidate_dimensions_m: [1.10, 0.70, 0.26]
blender_attachment_position_m: [0, 1.16, 0.54]
source_mutation: none
```

### 16.2 Bounded CLI construction recipe — executor handoff, not an execution receipt

The modeler role has no Blender MCP connection in this session, so it supplies specs only. The director can run the user's requested Blender work through the installed CLI in a fresh factory scene. The following recipe reads no existing `.blend`, refuses existing candidate output names, and exports only the new assembly. It must be saved as a new source script under `assets/generated/3d/scripts/` by the executing lane. Existing `build_hub_greybox.py`, historical outputs, and their provenance must not be overwritten.

FBX below is an **import-audit candidate**, not automatic revision of the GLB-canonical decision (§6.1). Any runtime-format choice and `runtimeEligible:true` require the director's recorded audit.

```python
# Execute with Blender --background --factory-startup --python <new-script.py>.
from pathlib import Path
import bpy, json
from mathutils import Vector

out = Path('/Users/jangyoung/orca/unknown/assets/generated/3d/hub-view-drawer-r01')
asset = 'SM_Hub_Workbench_Drawer'
assert not bpy.data.filepath, 'Use a fresh factory scene, not an existing project'
assert not any((out / (asset + ext)).exists() for ext in ('.blend', '.glb', '.fbx'))
out.mkdir(parents=True, exist_ok=True)
initial = [{'name': o.name, 'type': o.type} for o in bpy.data.objects]
for o in bpy.data.objects:
    o.hide_render = True  # Preserve the factory objects, do not delete them.
scene = bpy.context.scene
scene.unit_settings.system = 'METRIC'
scene.unit_settings.scale_length = 1.0
collection = bpy.data.collections.new('HUB_DRAWER_CANDIDATE')
scene.collection.children.link(collection)

def empty(name, parent=None):
    o = bpy.data.objects.new(name, None)
    collection.objects.link(o)
    o.parent = parent
    return o

def material(name, hexcode):
    def linear(v):
        c = int(v, 16) / 255
        return c / 12.92 if c <= .04045 else ((c + .055) / 1.055) ** 2.4
    m = bpy.data.materials.new(name)
    m.use_nodes = True
    color = tuple(linear(hexcode[i:i+2]) for i in (0, 2, 4)) + (1,)
    m.diffuse_color = color
    bsdf = m.node_tree.nodes.get('Principled BSDF')
    bsdf.inputs['Base Color'].default_value = color
    bsdf.inputs['Roughness'].default_value = .8
    return m

mats = [material('MAT_Hub_Drawer_Casing', '36565C'),
        material('MAT_Hub_Drawer_Inset', '173238'),
        material('MAT_Hub_Drawer_Handle', '4F7A6B')]
root = empty('ROOT_Hub_Workbench_Drawer')
tray = empty('JNT_Hub_Workbench_Drawer_tray', root)
tray.lock_location = (True, False, True)
tray.lock_rotation = (True, True, True)
tray.lock_scale = (True, True, True)

# Each component tuple = (centre XYZ metres, dimensions XYZ metres, material index).
# Disconnected boxes are joined into two mesh objects, keeping one moving rigid body.
def mesh_boxes(name, components, parent):
    verts, faces, indices = [], [], []
    quad = [(0,3,2,1),(4,5,6,7),(0,1,5,4),(1,2,6,5),(2,3,7,6),(3,0,4,7)]
    signs = [(-1,-1,-1),(1,-1,-1),(1,1,-1),(-1,1,-1),
             (-1,-1,1),(1,-1,1),(1,1,1),(-1,1,1)]
    for centre, dimensions, mi in components:
        offset = len(verts)
        verts.extend(tuple(centre[j] + s[j]*dimensions[j]/2 for j in range(3)) for s in signs)
        faces.extend(tuple(offset + i for i in face) for face in quad)
        indices.extend([mi]*6)
    data = bpy.data.meshes.new('MSH_' + name)
    data.from_pydata(verts, [], faces)
    data.update()
    obj = bpy.data.objects.new(name, data)
    collection.objects.link(obj)
    obj.parent = parent
    for mat in mats:
        data.materials.append(mat)
    for poly, index in zip(data.polygons, indices):
        poly.material_index = index
    return obj

casing = mesh_boxes(asset + '_Casing', [
    ((0,0,.015),(1.10,.70,.03),0), ((0,0,.245),(1.10,.70,.03),0),
    ((-.535,0,.13),(.03,.70,.20),0), ((.535,0,.13),(.03,.70,.20),0),
    ((0,.335,.13),(1.04,.03,.20),0)], root)
moving = mesh_boxes(asset + '_Tray', [
    ((0,-.322,.13),(1.02,.036,.18),0), ((0,-.010,.055),(.98,.59,.02),1),
    ((-.48,-.010,.115),(.02,.59,.10),1), ((.48,-.010,.115),(.02,.59,.10),1),
    ((0,.275,.115),(.98,.02,.10),1),
    ((-.15,-.355,.13),(.024,.05,.024),2), ((.15,-.355,.13),(.024,.05,.024),2),
    ((0,-.383,.13),(.324,.024,.024),2)], tray)
mesh_objects = [casing, moving]
bpy.context.view_layer.update()
measurements = {'asset_id': asset, 'status': 'blender-measured', 'runtimeEligible': False,
                'blender_version': bpy.app.version_string, 'initial_scene': initial,
                'tri_budget_target': 1500, 'texture_files': 0, 'objects': []}
for obj in mesh_objects:
    obj.data.calc_loop_triangles()
    measurements['objects'].append({'name': obj.name, 'vertices': len(obj.data.vertices),
        'triangles': len(obj.data.loop_triangles), 'polygons': len(obj.data.polygons),
        'material_slots': len(obj.material_slots), 'location': list(obj.location),
        'rotation': list(obj.rotation_euler), 'scale': list(obj.scale),
        'bounds_local': [list(Vector(c)) for c in obj.bound_box]})
measurements['triangles'] = sum(o['triangles'] for o in measurements['objects'])
assert measurements['triangles'] <= measurements['tri_budget_target']
(out / 'measurements.json').write_text(json.dumps(measurements, indent=2) + '\n')
bpy.ops.object.select_all(action='DESELECT')
for obj in (root, tray, casing, moving):
    obj.select_set(True)
bpy.context.view_layer.objects.active = casing
bpy.ops.export_scene.gltf(filepath=str(out / (asset + '.glb')), export_format='GLB',
    use_selection=True, export_apply=True, export_yup=True, export_animations=False)
bpy.ops.export_scene.fbx(filepath=str(out / (asset + '.fbx')), use_selection=True,
    object_types={'MESH', 'EMPTY'}, axis_up='Y', axis_forward='-Z', global_scale=1.0,
    apply_unit_scale=True, apply_scale_options='FBX_SCALE_NONE', bake_space_transform=False,
    bake_anim=False, add_leaf_bones=False, mesh_smooth_type='FACE', path_mode='AUTO')
bpy.ops.wm.save_as_mainfile(filepath=str(out / (asset + '.blend')))
print(json.dumps(measurements))
```

Execution owner adds a small preview in the **new** candidate directory, records actual bounds (the handle extends beyond the casing envelope), hashes every file and source recipe, and writes `provenance.json` with `asset_id`, `concept_ref`, generator/version, author, original-greybox license, source/export files, `runtimeEligible:false`, and `promoted_by:null`. No measurement or successful render is claimed merely because this recipe is present.

### 16.3 Physical station-panel evidence for systems

The existing `SM_Tool_circuit` box is `2.00 × 0.09 × 1.20` m, base pivot `(0,3.79,1.05)` in Blender and identity rotation/scale (`asset-manifest.md` §2). Thus its physical panel extent is X `[-1,1]`, Y `[3.745,3.835]`, Z `[1.05,2.25]` m **derived from those recorded dimensions**. `animation/rig-requirements.md` §2.2 gives the circuit no moving parts. Those bounds can frame a future inspection overlay; they establish no anchor grid, snapped coordinate, alignment offset, or correct answer. Planning/systems must author missing puzzle coordinates from their own data contract.

### 16.4 r02 material correction — executor-ready recipe

[OBSERVED 2026-09-10] The director executed r01 in Blender 5.1.2: `assets/generated/3d/hub-view-drawer-r01/measurements.json` records 2 meshes, 156 triangles, no texture files, identity local transforms and the preserved casing/tray bounds. `concept/t0-source-drawer-review.md` ACKs the existing-identity silhouette/palette and leaves final material FIX open for broad pristine surfaces and absent downward corrosion / edge salt. r01 and its receipts remain unchanged.

[TARGET / authored correction] The director authorizes four small baked maps for r02: a 1024 × 1024 BaseColor and Roughness PNG for each mesh, with one material slot per mesh. This is a bounded departure from r01’s zero-texture greybox target, within the existing 512 MiB scene texture ceiling; actual runtime GPU memory and scene totals remain unmeasured. The conservative RGBA8 base-level allocation bound is 16 MiB (about 21.34 MiB with full mip chains), not a measured importer allocation. Geometry, dimensions, parent/tray pivots, LOD0 and attachment occlusion warning remain unchanged. No physical animation is introduced.

The palette stays within frame `#36565C`, dark support `#173238`, verdigris `#4F7A6B`, salt `#C8D6D3`; generic rust `#8C4A3A` is absent. Widespread low-gloss worn coating, broken downward vertical streaks, and small flat hexagonal edge grains are authored into UV atlases. The renderer must establish whether they are visible and consistent with style-guide intent. Targets remain corrosion ≤15% frame, salt ≤12% frame, unworn metal ≤5% frame. Recipe UV-mask counts are explicitly **not frame coverage**, and cannot close the concept FIX or pass G5.

Execution owner: save this exact fenced recipe as new `assets/generated/3d/scripts/build_hub_drawer_r02.py`, then run `rtk run '/Applications/Blender.app/Contents/MacOS/Blender --background --factory-startup --python assets/generated/3d/scripts/build_hub_drawer_r02.py'`. It refuses an existing r02 output directory. A failed attempt must be preserved and moved by the execution owner before a new revision attempt; no output is silently overwritten. The script emits GLB canonical export, FBX derived Unity-import candidate, sidecar textures, `.blend`, `preview.png`, geometry/texture receipts and provenance. Runtime eligibility starts false. FBX texture bindings and material appearance require a real Unity import check. The modeler supplies this recipe because Blender MCP remains unavailable; execution and final visual inspection belong to the director and concept/QA owners.

```python
# Execute with Blender --background --factory-startup --python <new-script.py>.
from pathlib import Path
import bpy, json
from mathutils import Vector

out = Path('/Users/jangyoung/orca/unknown/assets/generated/3d/hub-view-drawer-r02')
asset = 'SM_Hub_Workbench_Drawer'
assert not bpy.data.filepath, 'Use a fresh factory scene, not an existing project'
assert not out.exists(), 'Preserve previous output; choose a new revision directory'
out.mkdir(parents=True, exist_ok=True)
initial = [{'name': o.name, 'type': o.type} for o in bpy.data.objects]
for o in bpy.data.objects:
    o.hide_render = True  # Preserve the factory objects, do not delete them.
scene = bpy.context.scene
scene.unit_settings.system = 'METRIC'
scene.unit_settings.scale_length = 1.0
collection = bpy.data.collections.new('HUB_DRAWER_CANDIDATE')
scene.collection.children.link(collection)

def empty(name, parent=None):
    o = bpy.data.objects.new(name, None)
    collection.objects.link(o)
    o.parent = parent
    return o

def material(name, hexcode):
    def linear(v):
        c = int(v, 16) / 255
        return c / 12.92 if c <= .04045 else ((c + .055) / 1.055) ** 2.4
    m = bpy.data.materials.new(name)
    m.use_nodes = True
    color = tuple(linear(hexcode[i:i+2]) for i in (0, 2, 4)) + (1,)
    m.diffuse_color = color
    bsdf = m.node_tree.nodes.get('Principled BSDF')
    bsdf.inputs['Base Color'].default_value = color
    bsdf.inputs['Roughness'].default_value = .8
    return m

mats = [material('MAT_Hub_Drawer_Casing', '36565C'),
        material('MAT_Hub_Drawer_Inset', '173238'),
        material('MAT_Hub_Drawer_Handle', '4F7A6B')]
root = empty('ROOT_Hub_Workbench_Drawer')
tray = empty('JNT_Hub_Workbench_Drawer_tray', root)
tray.lock_location = (True, False, True)
tray.lock_rotation = (True, True, True)
tray.lock_scale = (True, True, True)

# Each component tuple = (centre XYZ metres, dimensions XYZ metres, material index).
# Disconnected boxes are joined into two mesh objects, keeping one moving rigid body.
def mesh_boxes(name, components, parent):
    verts, faces, indices = [], [], []
    quad = [(0,3,2,1),(4,5,6,7),(0,1,5,4),(1,2,6,5),(2,3,7,6),(3,0,4,7)]
    signs = [(-1,-1,-1),(1,-1,-1),(1,1,-1),(-1,1,-1),
             (-1,-1,1),(1,-1,1),(1,1,1),(-1,1,1)]
    for centre, dimensions, mi in components:
        offset = len(verts)
        verts.extend(tuple(centre[j] + s[j]*dimensions[j]/2 for j in range(3)) for s in signs)
        faces.extend(tuple(offset + i for i in face) for face in quad)
        indices.extend([mi]*6)
    data = bpy.data.meshes.new('MSH_' + name)
    data.from_pydata(verts, [], faces)
    data.update()
    obj = bpy.data.objects.new(name, data)
    collection.objects.link(obj)
    obj.parent = parent
    for mat in mats:
        data.materials.append(mat)
    for poly, index in zip(data.polygons, indices):
        poly.material_index = index
    return obj

casing = mesh_boxes(asset + '_Casing', [
    ((0,0,.015),(1.10,.70,.03),0), ((0,0,.245),(1.10,.70,.03),0),
    ((-.535,0,.13),(.03,.70,.20),0), ((.535,0,.13),(.03,.70,.20),0),
    ((0,.335,.13),(1.04,.03,.20),0)], root)
moving = mesh_boxes(asset + '_Tray', [
    ((0,-.322,.13),(1.02,.036,.18),0), ((0,-.010,.055),(.98,.59,.02),1),
    ((-.48,-.010,.115),(.02,.59,.10),1), ((.48,-.010,.115),(.02,.59,.10),1),
    ((0,.275,.115),(.98,.02,.10),1),
    ((-.15,-.355,.13),(.024,.05,.024),2), ((.15,-.355,.13),(.024,.05,.024),2),
    ((0,-.383,.13),(.324,.024,.024),2)], tray)
mesh_objects = [casing, moving]
bpy.context.view_layer.update()

# r02 authored surface correction. No new geometry; diffuse/roughness atlases survive GLB export.
import math, hashlib
from array import array
SIZE = 1024
PAD = 3
texture_dir = out / 'textures'
texture_dir.mkdir()

def linear_hex(code):
    def linear(c):
        v = int(c, 16) / 255.0
        return v / 12.92 if v <= .04045 else ((v + .055) / 1.055) ** 2.4
    return tuple(linear(code[i:i+2]) for i in (0, 2, 4))

FRAME = linear_hex('36565C')
DARK = linear_hex('173238')
CORROSION = linear_hex('4F7A6B')
SALT = linear_hex('C8D6D3')

def mix(a, b, t):
    return tuple(a[i] * (1-t) + b[i] * t for i in range(3))

def noise(x, y, seed):
    v = math.sin(x * 127.1 + y * 311.7 + seed * 19.19) * 43758.5453
    return v - math.floor(v)

def face_layout(obj):
    faces = []
    for poly in obj.data.polygons:
        # UV V is world/local +Z on vertical faces, so the authored streaks flow down.
        axis = max(range(3), key=lambda i: abs(poly.normal[i]))
        axes = (0, 1) if axis == 2 else ((1, 2) if axis == 0 else (0, 2))
        points = [obj.data.vertices[i].co for i in poly.vertices]
        lo = [min(p[a] for p in points) for a in axes]
        hi = [max(p[a] for p in points) for a in axes]
        faces.append({'poly': poly.index, 'axes': axes, 'lo': lo,
                      'span': [hi[j]-lo[j] for j in range(2)],
                      'vertical': axis != 2, 'material': poly.material_index})
    # Shelf pack the physical face extents; large visible faces get proportionate texel area.
    density = 700.0
    for attempt in range(30):
        for f in faces:
            f['w'] = max(4, math.ceil(f['span'][0] * density))
            f['h'] = max(4, math.ceil(f['span'][1] * density))
        x = y = row_height = 0
        fits = True
        for f in sorted(faces, key=lambda f: (-f['h'], -f['w'], f['poly'])):
            w, h = f['w'] + 2*PAD, f['h'] + 2*PAD
            if x + w > SIZE:
                y += row_height
                x, row_height = 0, 0
            if w > SIZE or y + h > SIZE:
                fits = False
                break
            f['x'], f['y'] = x+PAD, y+PAD
            x += w
            row_height = max(row_height, h)
        if fits:
            return faces, density
        density *= .92
    raise RuntimeError('Atlas packing failed')

def paint_face(u, v, f, seed):
    # All surfaces receive low-gloss mottled wear, with no untouched uniform paint field.
    grain = noise(math.floor(u*f['w']), math.floor(v*f['h']), seed)
    cloud = .5 + .5 * math.sin(u*18 + seed) * math.sin(v*13 + .7*seed)
    edge_px = min(u*f['w'], (1-u)*f['w'], v*f['h'], (1-v)*f['h'])
    wear = .18 + .23*cloud + .12*grain
    base = DARK if f['material'] == 1 else FRAME
    color = mix(base, DARK, wear if f['material'] != 1 else .05)
    color = tuple(c*(.88 + .12*grain) for c in color)
    if edge_px < 1.7 and grain > .34:
        color = mix(color, DARK, .45)  # chipped dark supporting layer, not bright bare metal
    corrosion = False
    if f['vertical']:
        # Local V decreases from each deposit head; tails narrow as they descend.
        for j in range(4):
            centre = .10 + .8*noise(j, 1, seed)
            head = .68 + .27*noise(j, 2, seed)
            length = .22 + .43*noise(j, 3, seed)
            t = (head-v)/length
            if 0 <= t <= 1:
                width = (.017 + .008*noise(j, 4, seed))*(1-.8*t)
                wandering = .003*math.sin(v*22 + j + seed)
                if abs(u-centre-wandering) < width and grain > .14:
                    corrosion = True
    else:
        # Broken verdigris deposits remain small; horizontal surfaces have no fake flow direction.
        patch = math.sin(u*28+seed) * math.sin(v*23+.3*seed)
        corrosion = patch > .87 and grain > .2
    if corrosion:
        color = mix(DARK, CORROSION, .62 + .30*grain)
    # Fine matte hexagonal grains clustered at physical component edges; no raised white blobs.
    cell = 5.0
    px, py = u*f['w'], v*f['h']
    cx, cy = math.floor(px/cell), math.floor(py/cell)
    dx, dy = abs(px-(cx+.5)*cell), abs(py-(cy+.5)*cell)
    radius = .65 + .35*noise(cx, cy, seed+21)
    hexagon = dx <= radius and .5*dx + .8660254*dy <= radius
    cluster = noise(math.floor(cx/3), math.floor(cy/3), seed+7) > .48
    salt = edge_px < 4.8 and hexagon and cluster and noise(cx, cy, seed+8) > .28
    if salt:
        color = mix(color, SALT, .62 + .22*grain)
    roughness = .86 + .10*grain if salt else (.76 + .15*grain if corrosion else .74 + .15*cloud)
    return color, roughness, corrosion and not salt, salt

texture_receipts = []
for object_index, obj in enumerate(mesh_objects):
    faces, density = face_layout(obj)
    rgba = array('f', [0.0]) * (SIZE*SIZE*4)
    rough = array('f', [0.82]) * (SIZE*SIZE*4)
    count = {'painted_texels': 0, 'corrosion_texels': 0, 'salt_texels': 0}
    uv = obj.data.uv_layers.new(name='UV_Drawer_Surface')
    for f in faces:
        for loop_id in obj.data.polygons[f['poly']].loop_indices:
            p = obj.data.vertices[obj.data.loops[loop_id].vertex_index].co
            local_uv = [(p[a]-f['lo'][j])/f['span'][j] for j, a in enumerate(f['axes'])]
            uv.data[loop_id].uv = ((f['x']+.5 + local_uv[0]*(f['w']-1))/SIZE,
                                  (f['y']+.5 + local_uv[1]*(f['h']-1))/SIZE)
        for yy in range(-PAD, f['h']+PAD):
            for xx in range(-PAD, f['w']+PAD):
                # Clamp padding to edge pixels to prevent atlas seams under filtering/mips.
                u = max(0, min(f['w']-1, xx))/max(1, f['w']-1)
                v = max(0, min(f['h']-1, yy))/max(1, f['h']-1)
                color, r, corrosion, salt = paint_face(u, v, f, 101*object_index+f['poly'])
                offset = ((f['y']+yy)*SIZE + f['x']+xx)*4
                rgba[offset:offset+4] = array('f', (*color, 1.0))
                rough[offset:offset+4] = array('f', (r, r, r, 1.0))
                if 0 <= xx < f['w'] and 0 <= yy < f['h']:
                    count['painted_texels'] += 1
                    count['corrosion_texels'] += int(corrosion)
                    count['salt_texels'] += int(salt)
    images = []
    for suffix, pixels, space in [('BaseColor', rgba, 'sRGB'), ('Roughness', rough, 'Non-Color')]:
        name = obj.name + '_' + suffix
        img = bpy.data.images.new(name, width=SIZE, height=SIZE, alpha=True)
        img.colorspace_settings.name = space
        img.pixels.foreach_set(pixels)
        img.filepath_raw = str(texture_dir / (name + '.png'))
        img.file_format = 'PNG'
        img.save()
        images.append(img)
    mat = bpy.data.materials.new('MAT_' + obj.name + '_Worn_r02')
    mat.use_nodes = True
    mat.diffuse_color = (*FRAME, 1)
    bsdf = mat.node_tree.nodes.get('Principled BSDF')
    bsdf.inputs['Metallic'].default_value = .08
    for img, socket in zip(images, ['Base Color', 'Roughness']):
        node = mat.node_tree.nodes.new('ShaderNodeTexImage')
        node.image = img
        node.interpolation = 'Linear'
        mat.node_tree.links.new(node.outputs['Color'], bsdf.inputs[socket])
    obj.data.materials.clear()
    obj.data.materials.append(mat)
    for poly in obj.data.polygons:
        poly.material_index = 0
    count.update({'object': obj.name, 'atlas_size': [SIZE, SIZE], 'pixels_per_metre': density,
                  'corrosion_mask_fraction': count['corrosion_texels']/count['painted_texels'],
                  'salt_mask_fraction': count['salt_texels']/count['painted_texels'],
                  'scope': 'Authored UV mask counts; NOT frame coverage or a visual pass'})
    texture_receipts.append(count)

bpy.context.view_layer.update()
measurements = {'asset_id': asset, 'revision': 'r02', 'status': 'blender-measured',
    'runtimeEligible': False, 'blender_version': bpy.app.version_string, 'initial_scene': initial,
    'tri_budget_target': 1500, 'texture_files': 4, 'objects': [],
    'material_target': {'frame_corrosion_max': .15, 'frame_salt_max': .12,
                        'frame_unworn_metal_max': .05, 'measured_frame_coverage': None},
    'texture_mask_receipts': texture_receipts,
    'runtime_texture_memory_bytes': None,
    'texture_memory_rgba8_no_mips_upper_bound_bytes': 4*SIZE*SIZE*4}
for obj in mesh_objects:
    obj.data.calc_loop_triangles()
    measurements['objects'].append({'name': obj.name, 'vertices': len(obj.data.vertices),
        'triangles': len(obj.data.loop_triangles), 'polygons': len(obj.data.polygons),
        'material_slots': len(obj.material_slots), 'location': list(obj.location),
        'rotation': list(obj.rotation_euler), 'scale': list(obj.scale),
        'bounds_local': [list(Vector(c)) for c in obj.bound_box],
        'uv_layers': [layer.name for layer in obj.data.uv_layers]})
measurements['triangles'] = sum(o['triangles'] for o in measurements['objects'])
assert len(mesh_objects) == 2 and measurements['triangles'] <= 1500
measurements['texture_disk_bytes'] = sum(p.stat().st_size for p in texture_dir.glob('*.png'))
(out / 'measurements.json').write_text(json.dumps(measurements, indent=2) + '\n')

# Export only the unchanged drawer hierarchy. GLB embeds textures; FBX points to the sidecar folder.
bpy.ops.object.select_all(action='DESELECT')
for obj in (root, tray, casing, moving):
    obj.select_set(True)
bpy.context.view_layer.objects.active = casing
bpy.ops.export_scene.gltf(filepath=str(out / (asset + '.glb')), export_format='GLB',
    use_selection=True, export_apply=True, export_yup=True, export_animations=False)
bpy.ops.export_scene.fbx(filepath=str(out / (asset + '.fbx')), use_selection=True,
    object_types={'MESH', 'EMPTY'}, axis_up='Y', axis_forward='-Z', global_scale=1.0,
    apply_unit_scale=True, apply_scale_options='FBX_SCALE_NONE', mesh_smooth_type='FACE',
    path_mode='RELATIVE', embed_textures=False)

# Isolated inspectable preview: no unrelated mesh, emission, bloom, screen flash or compositor FX.
scene.render.engine = 'CYCLES'
scene.cycles.samples = 32
scene.cycles.use_denoising = True
scene.render.resolution_x = 960
scene.render.resolution_y = 640
scene.render.resolution_percentage = 100
scene.render.image_settings.file_format = 'PNG'
scene.render.film_transparent = False
scene.world.use_nodes = True
scene.world.node_tree.nodes['Background'].inputs[0].default_value = (.028,.04,.042,1)
scene.world.node_tree.nodes['Background'].inputs[1].default_value = .7
scene.view_settings.view_transform = 'Standard'
scene.view_settings.exposure = 0
scene.view_settings.gamma = 1
camera_data = bpy.data.cameras.new('CAM_Drawer_r02_Preview')
camera = bpy.data.objects.new(camera_data.name, camera_data)
collection.objects.link(camera)
camera.location = (1.3,-1.8,1.13)
camera.rotation_euler = (Vector((0,0,.12))-camera.location).to_track_quat('-Z','Y').to_euler()
camera_data.type = 'ORTHO'
camera_data.ortho_scale = 1.58
scene.camera = camera
for name, pos, power, size in [('Key',(-1.7,-2.5,3),180,3), ('Fill',(2,1,2),90,2.5)]:
    data = bpy.data.lights.new('LIGHT_Drawer_r02_'+name, 'AREA')
    data.energy, data.shape, data.size = power, 'DISK', size
    obj = bpy.data.objects.new(data.name, data)
    collection.objects.link(obj)
    obj.location = pos
    obj.rotation_euler = (Vector((0,0,.12))-obj.location).to_track_quat('-Z','Y').to_euler()
scene.render.filepath = str(out / 'preview.png')
# Store relative image paths in the blend after exporting to make the review bundle portable.
for image in bpy.data.images:
    if image.filepath_raw and Path(image.filepath_raw).parent == texture_dir:
        image.filepath = '//textures/' + Path(image.filepath_raw).name
bpy.ops.wm.save_as_mainfile(filepath=str(out / (asset + '.blend')))
bpy.ops.render.render(write_still=True)
provenance = {'assetId': asset, 'revision': 'r02', 'provider': 'Blender CLI',
    'version': bpy.app.version_string, 'recipe': str(Path(__file__).resolve()),
    'recipe_sha256': hashlib.sha256(Path(__file__).read_bytes()).hexdigest(),
    'recipeSource': '_workspace/current/modeling/pipeline.md section 16.4',
    'decision': 'RFC-CX-003; director r02 texture/material correction instruction',
    'concept_ref': ['_workspace/current/concept/t0-source-drawer-review.md',
                    '_workspace/current/concept/style-guide.md'],
    'author': 'game-modeler recipe; director executes and audits',
    'license': 'Original procedural geometry and textures; no third-party source media',
    'runtimeEligible': False, 'promoted_by': None,
    'classification': 'Generated material-correction candidate; visual/import audit pending',
    'canonical_export': asset + '.glb', 'derived_import_candidate': asset + '.fbx',
    'hashes': {str(p.relative_to(out)): hashlib.sha256(p.read_bytes()).hexdigest()
               for p in sorted(out.rglob('*')) if p.is_file() and p.name != 'provenance.json'}}
(out / 'provenance.json').write_text(json.dumps(provenance, indent=2) + '\n')
print(json.dumps(measurements))
```

Review checklist after execution: compare r01/r02 geometry bounds and pivots; inspect preview plus GLB material images; check downward flow and fine edge grains without snow-like lumps; measure actual frame coverage before any quantitative style claim; compare Unity imported FBX geometry/material bindings and use the GLB as the canonical reference. Keep OPEN-S11 and material FIX scoped to their respective owners until evidence resolves them.

[OBSERVED 2026-09-10] The §16.4 fenced Python recipe passed `ast.parse` through `rtk run` with exit 0; its `mesh_boxes` constructor and component definitions compare byte-for-byte equal to the preserved r01 recipe. These are authoring checks only. No r02 Blender execution, render or geometry/texture runtime measurement was performed by the modeler.

### 16.5 r03 material correction — irregular deposits and explicit color encoding

[OBSERVED 2026-09-10] The r02 Blender candidate exists; `concept/t0-source-drawer-review.md` r02 follow-up keeps final material FIX open for top-face diagonal spot repetition, similarly spaced/shaped drips, perimeter salt dots, and weak structural/recess separation. The r02 preview was visually inspected. Actual BaseColor PNG inspection with Pillow found the casing’s most-common RGB value `(6,17,20)` and the tray’s `(2,8,10)`. The recipe computed linear palette values and assigned them to a generated byte image tagged sRGB; the stored values closely match those linear numbers quantized directly to 8-bit, explaining substantial darkening when read as sRGB. This establishes an encoding defect; it does not independently prove the lighting is ideal.

[TARGET / authored correction] r03 writes BaseColor PNG pixels through an explicit linear-to-sRGB conversion using Python standard-library PNG chunks, then loads those files as sRGB. Roughness remains linear grayscale/Non-Color. The original camera, lights, exposure and world remain unchanged so the encoding correction can be compared independently. The structural palette stays `#36565C`, recesses `#173238`; existing verdigris and salt colors are retained. Worn-coating attenuation is reduced while maintaining spatial variation. No emissive material, new palette color or geometry is added.

Corrosion uses seeded clusters of unequal overlapping lobes, varying omitted regions and ragged boundaries. Vertical runs have independently seeded origins, widths, lengths and connected secondary runs; all flow down. Salt uses unequal, sparse clusters of jittered fine hexagonal grains on selected edge regions, with explicit gaps. Spatial buckets only accelerate lookup; they do not place the grains. Quantitative frame coverage and final visual compliance remain unmeasured.

Execution: extract the first Python fence after this §16.5 heading as new `assets/generated/3d/scripts/build_hub_drawer_r03.py`; run with the same fresh-factory Blender CLI command as r02, changing only the script filename. The output directory is new `assets/generated/3d/hub-view-drawer-r03/`; existing output names are refused. Preserve both earlier revisions. Geometry source remains byte-for-byte the r01/r02 constructor and component table: 2 meshes and 156 triangles are the earlier measured baseline, and the r03 recipe must still emit its own measured receipt. Budget remains 4 maps at 1024², two material slots total, existing triangle/scene budgets, GLB canonical and FBX derived import candidate. Final material, Unity texture binding and runtime eligibility need their own evidence.

```python
# Execute with Blender --background --factory-startup --python <new-script.py>.
from pathlib import Path
import bpy, json
from mathutils import Vector

out = Path('/Users/jangyoung/orca/unknown/assets/generated/3d/hub-view-drawer-r03')
asset = 'SM_Hub_Workbench_Drawer'
assert not bpy.data.filepath, 'Use a fresh factory scene, not an existing project'
assert not out.exists(), 'Preserve previous output; choose a new revision directory'
out.mkdir(parents=True, exist_ok=True)
initial = [{'name': o.name, 'type': o.type} for o in bpy.data.objects]
for o in bpy.data.objects:
    o.hide_render = True  # Preserve the factory objects, do not delete them.
scene = bpy.context.scene
scene.unit_settings.system = 'METRIC'
scene.unit_settings.scale_length = 1.0
collection = bpy.data.collections.new('HUB_DRAWER_CANDIDATE')
scene.collection.children.link(collection)

def empty(name, parent=None):
    o = bpy.data.objects.new(name, None)
    collection.objects.link(o)
    o.parent = parent
    return o

def material(name, hexcode):
    def linear(v):
        c = int(v, 16) / 255
        return c / 12.92 if c <= .04045 else ((c + .055) / 1.055) ** 2.4
    m = bpy.data.materials.new(name)
    m.use_nodes = True
    color = tuple(linear(hexcode[i:i+2]) for i in (0, 2, 4)) + (1,)
    m.diffuse_color = color
    bsdf = m.node_tree.nodes.get('Principled BSDF')
    bsdf.inputs['Base Color'].default_value = color
    bsdf.inputs['Roughness'].default_value = .8
    return m

mats = [material('MAT_Hub_Drawer_Casing', '36565C'),
        material('MAT_Hub_Drawer_Inset', '173238'),
        material('MAT_Hub_Drawer_Handle', '4F7A6B')]
root = empty('ROOT_Hub_Workbench_Drawer')
tray = empty('JNT_Hub_Workbench_Drawer_tray', root)
tray.lock_location = (True, False, True)
tray.lock_rotation = (True, True, True)
tray.lock_scale = (True, True, True)

# Each component tuple = (centre XYZ metres, dimensions XYZ metres, material index).
# Disconnected boxes are joined into two mesh objects, keeping one moving rigid body.
def mesh_boxes(name, components, parent):
    verts, faces, indices = [], [], []
    quad = [(0,3,2,1),(4,5,6,7),(0,1,5,4),(1,2,6,5),(2,3,7,6),(3,0,4,7)]
    signs = [(-1,-1,-1),(1,-1,-1),(1,1,-1),(-1,1,-1),
             (-1,-1,1),(1,-1,1),(1,1,1),(-1,1,1)]
    for centre, dimensions, mi in components:
        offset = len(verts)
        verts.extend(tuple(centre[j] + s[j]*dimensions[j]/2 for j in range(3)) for s in signs)
        faces.extend(tuple(offset + i for i in face) for face in quad)
        indices.extend([mi]*6)
    data = bpy.data.meshes.new('MSH_' + name)
    data.from_pydata(verts, [], faces)
    data.update()
    obj = bpy.data.objects.new(name, data)
    collection.objects.link(obj)
    obj.parent = parent
    for mat in mats:
        data.materials.append(mat)
    for poly, index in zip(data.polygons, indices):
        poly.material_index = index
    return obj

casing = mesh_boxes(asset + '_Casing', [
    ((0,0,.015),(1.10,.70,.03),0), ((0,0,.245),(1.10,.70,.03),0),
    ((-.535,0,.13),(.03,.70,.20),0), ((.535,0,.13),(.03,.70,.20),0),
    ((0,.335,.13),(1.04,.03,.20),0)], root)
moving = mesh_boxes(asset + '_Tray', [
    ((0,-.322,.13),(1.02,.036,.18),0), ((0,-.010,.055),(.98,.59,.02),1),
    ((-.48,-.010,.115),(.02,.59,.10),1), ((.48,-.010,.115),(.02,.59,.10),1),
    ((0,.275,.115),(.98,.02,.10),1),
    ((-.15,-.355,.13),(.024,.05,.024),2), ((.15,-.355,.13),(.024,.05,.024),2),
    ((0,-.383,.13),(.324,.024,.024),2)], tray)
mesh_objects = [casing, moving]
bpy.context.view_layer.update()

# r03 authored surface correction. No new geometry; diffuse/roughness atlases survive GLB export.
import math, hashlib, random
from array import array
SIZE = 1024
PAD = 3
texture_dir = out / 'textures'
texture_dir.mkdir()

def linear_hex(code):
    def linear(c):
        v = int(c, 16) / 255.0
        return v / 12.92 if v <= .04045 else ((v + .055) / 1.055) ** 2.4
    return tuple(linear(code[i:i+2]) for i in (0, 2, 4))

FRAME = linear_hex('36565C')
DARK = linear_hex('173238')
CORROSION = linear_hex('4F7A6B')
SALT = linear_hex('C8D6D3')

def mix(a, b, t):
    return tuple(a[i] * (1-t) + b[i] * t for i in range(3))

def noise(x, y, seed):
    v = math.sin(x * 127.1 + y * 311.7 + seed * 19.19) * 43758.5453
    return v - math.floor(v)

def face_layout(obj):
    faces = []
    for poly in obj.data.polygons:
        # UV V is world/local +Z on vertical faces, so the authored streaks flow down.
        axis = max(range(3), key=lambda i: abs(poly.normal[i]))
        axes = (0, 1) if axis == 2 else ((1, 2) if axis == 0 else (0, 2))
        points = [obj.data.vertices[i].co for i in poly.vertices]
        lo = [min(p[a] for p in points) for a in axes]
        hi = [max(p[a] for p in points) for a in axes]
        faces.append({'poly': poly.index, 'axes': axes, 'lo': lo,
                      'span': [hi[j]-lo[j] for j in range(2)],
                      'vertical': axis != 2, 'material': poly.material_index})
    # Shelf pack the physical face extents; large visible faces get proportionate texel area.
    density = 700.0
    for attempt in range(30):
        for f in faces:
            f['w'] = max(4, math.ceil(f['span'][0] * density))
            f['h'] = max(4, math.ceil(f['span'][1] * density))
        x = y = row_height = 0
        fits = True
        for f in sorted(faces, key=lambda f: (-f['h'], -f['w'], f['poly'])):
            w, h = f['w'] + 2*PAD, f['h'] + 2*PAD
            if x + w > SIZE:
                y += row_height
                x, row_height = 0, 0
            if w > SIZE or y + h > SIZE:
                fits = False
                break
            f['x'], f['y'] = x+PAD, y+PAD
            x += w
            row_height = max(row_height, h)
        if fits:
            return faces, density
        density *= .92
    raise RuntimeError('Atlas packing failed')

def smooth_noise(x, y, seed):
    ix, iy = math.floor(x), math.floor(y)
    fx, fy = x-ix, y-iy
    fx, fy = fx*fx*(3-2*fx), fy*fy*(3-2*fy)
    a = noise(ix, iy, seed)*(1-fx) + noise(ix+1, iy, seed)*fx
    b = noise(ix, iy+1, seed)*(1-fx) + noise(ix+1, iy+1, seed)*fx
    return a*(1-fy) + b*fy

def prepare_patterns(f, seed):
    # Seeded positions/sizes, with overlapping lobes, absent regions and broken subclusters.
    rng = random.Random(seed+4001)
    f['patches'], f['drips'], f['salt_bins'] = [], [], {}
    if not f['vertical']:
        for _ in range(rng.randint(2, 4)):
            centre_u, centre_v = rng.uniform(.05,.95), rng.uniform(.06,.94)
            for j in range(rng.randint(2,4)):
                f['patches'].append((centre_u+rng.gauss(0,.035), centre_v+rng.gauss(0,.034),
                                     rng.uniform(.025,.085), rng.uniform(.018,.066)))
    else:
        for _ in range(rng.randint(2,5)):
            u, head = rng.uniform(.04,.96), rng.uniform(.50,.99)
            length, width = rng.uniform(.13,.83), rng.uniform(.006,.024)
            offsets = [rng.uniform(-.010,.010) for _ in range(5)]
            f['drips'].append((u,head,length,width,offsets))
            if rng.random() < .45:
                # Occasional joined shorter run from the same deposit, not another stamped mark.
                f['drips'].append((u+rng.uniform(-.017,.017), head-rng.uniform(.02,.17),
                                   length*rng.uniform(.28,.61), width*rng.uniform(.35,.72), offsets))
    # Salt grains are seeded points, not a cell lattice. Only selected discontinuous edge clusters.
    selected_edges = rng.sample(range(4), rng.randint(1,3))
    for edge in selected_edges:
        along_extent = f['w'] if edge < 2 else f['h']
        for _ in range(rng.randint(1,2)):
            centre = rng.uniform(.06,.94)*along_extent
            spread = rng.uniform(3, max(4,min(22,along_extent*.065)))
            for _ in range(rng.randint(8,29)):
                if rng.random() < .22:
                    continue
                along = centre + rng.gauss(0,spread)
                inset = abs(rng.gauss(1.5,1.15))
                if edge == 0: px,py = along,inset
                elif edge == 1: px,py = along,f['h']-1-inset
                elif edge == 2: px,py = inset,along
                else: px,py = f['w']-1-inset,along
                if 0 <= px < f['w'] and 0 <= py < f['h']:
                    point=(px,py,rng.uniform(.48,1.12),rng.uniform(.40,.73))
                    key=(math.floor(px/4),math.floor(py/4))
                    f['salt_bins'].setdefault(key,[]).append(point)

def paint_face(u, v, f, seed):
    px, py = u*(f['w']-1), v*(f['h']-1)
    grain = noise(math.floor(px), math.floor(py), seed)
    cloud = smooth_noise(u*5.3, v*4.7, seed+31)
    fine = smooth_noise(u*21.9, v*19.1, seed+61)
    edge_px = min(px, f['w']-1-px, py, f['h']-1-py)
    # Recover structural #36565C versus recessed #173238 through correct encoding and restrained wear.
    # No clean untouched field: even the lightest metal has an irregular worn-coating treatment.
    base = DARK if f['material'] == 1 else FRAME
    wear = .05 + .21*cloud + .035*grain
    color = mix(base, DARK, wear if f['material'] != 1 else .035)
    color = tuple(c*(.95+.05*fine) for c in color)
    if edge_px < 1.65 and grain > .43 and fine > .40:
        color = mix(color, DARK, .42)
    corrosion = False
    if f['vertical']:
        for centre,head,length,width,offsets in f['drips']:
            t=(head-v)/length
            if 0 <= t <= 1:
                k=min(3,math.floor(t*4))
                q=t*4-k
                wander=offsets[k]*(1-q)+offsets[k+1]*q
                broken_width=width*(1-.86*t)*(.48+.75*fine)
                if abs(u-centre-wander) < broken_width and grain > .10:
                    corrosion = True
    else:
        for centre_u,centre_v,rx,ry in f['patches']:
            distance=((u-centre_u)/rx)**2 + ((v-centre_v)/ry)**2
            if distance < .70+.62*fine and grain > .07:
                corrosion = True
                break
    if corrosion:
        color = mix(DARK, CORROSION, .67+.26*fine)
    salt = False
    if edge_px < 7:
        bucket_x,bucket_y=math.floor(px/4),math.floor(py/4)
        for bx in range(bucket_x-1,bucket_x+2):
            for by in range(bucket_y-1,bucket_y+2):
                for sx,sy,radius,strength in f['salt_bins'].get((bx,by),[]):
                    dx,dy=abs(px-sx),abs(py-sy)
                    if dx <= radius and .5*dx+.8660254*dy <= radius:
                        color=mix(color,SALT,strength)
                        salt=True
                        break
    roughness = .89+.07*grain if salt else (.78+.13*grain if corrosion else .76+.12*cloud)
    return color, roughness, corrosion and not salt, salt

def srgb_byte(value):
    value=max(0.0,min(1.0,value))
    encoded=12.92*value if value <= .0031308 else 1.055*(value**(1/2.4))-.055
    return round(encoded*255)

# Explicit PNG encoding avoids r02's generated byte-buffer quantization of linear values as sRGB.
assert tuple(srgb_byte(c) for c in FRAME) == (54,86,92)
assert tuple(srgb_byte(c) for c in DARK) == (23,50,56)

def write_texture_png(path, pixels, color_map):
    import struct, zlib
    def chunk(tag, data):
        return struct.pack('>I',len(data))+tag+data+struct.pack('>I',zlib.crc32(tag+data)&0xffffffff)
    channels=4 if color_map else 1
    scanlines=bytearray()
    # Blender UV/image buffers start at the bottom; PNG scanlines start at the top.
    for y in reversed(range(SIZE)):
        scanlines.append(0)
        for x in range(SIZE):
            i=(y*SIZE+x)*4
            if color_map:
                scanlines.extend(srgb_byte(pixels[i+j]) for j in range(3))
                scanlines.append(round(max(0,min(1,pixels[i+3]))*255))
            else:
                scanlines.append(round(max(0,min(1,pixels[i]))*255))
    payload=b'\x89PNG\r\n\x1a\n'
    payload+=chunk(b'IHDR',struct.pack('>IIBBBBB',SIZE,SIZE,8,6 if color_map else 0,0,0,0))
    if color_map:
        payload+=chunk(b'sRGB',b'\x00')
    payload+=chunk(b'IDAT',zlib.compress(bytes(scanlines),6))+chunk(b'IEND',b'')
    path.write_bytes(payload)

texture_receipts = []
for object_index, obj in enumerate(mesh_objects):
    faces, density = face_layout(obj)
    rgba = array('f', [0.0]) * (SIZE*SIZE*4)
    rough = array('f', [0.82]) * (SIZE*SIZE*4)
    count = {'painted_texels': 0, 'corrosion_texels': 0, 'salt_texels': 0}
    uv = obj.data.uv_layers.new(name='UV_Drawer_Surface')
    for f in faces:
        prepare_patterns(f, 101*object_index+f['poly'])
        for loop_id in obj.data.polygons[f['poly']].loop_indices:
            p = obj.data.vertices[obj.data.loops[loop_id].vertex_index].co
            local_uv = [(p[a]-f['lo'][j])/f['span'][j] for j, a in enumerate(f['axes'])]
            uv.data[loop_id].uv = ((f['x']+.5 + local_uv[0]*(f['w']-1))/SIZE,
                                  (f['y']+.5 + local_uv[1]*(f['h']-1))/SIZE)
        for yy in range(-PAD, f['h']+PAD):
            for xx in range(-PAD, f['w']+PAD):
                # Clamp padding to edge pixels to prevent atlas seams under filtering/mips.
                u = max(0, min(f['w']-1, xx))/max(1, f['w']-1)
                v = max(0, min(f['h']-1, yy))/max(1, f['h']-1)
                color, r, corrosion, salt = paint_face(u, v, f, 101*object_index+f['poly'])
                offset = ((f['y']+yy)*SIZE + f['x']+xx)*4
                rgba[offset:offset+4] = array('f', (*color, 1.0))
                rough[offset:offset+4] = array('f', (r, r, r, 1.0))
                if 0 <= xx < f['w'] and 0 <= yy < f['h']:
                    count['painted_texels'] += 1
                    count['corrosion_texels'] += int(corrosion)
                    count['salt_texels'] += int(salt)
    images = []
    for suffix, pixels, space in [('BaseColor', rgba, 'sRGB'), ('Roughness', rough, 'Non-Color')]:
        name = obj.name + '_' + suffix
        texture_path = texture_dir / (name + '.png')
        write_texture_png(texture_path, pixels, suffix == 'BaseColor')
        img = bpy.data.images.load(str(texture_path), check_existing=False)
        img.colorspace_settings.name = space
        images.append(img)
    mat = bpy.data.materials.new('MAT_' + obj.name + '_Worn_r03')
    mat.use_nodes = True
    mat.diffuse_color = (*FRAME, 1)
    bsdf = mat.node_tree.nodes.get('Principled BSDF')
    bsdf.inputs['Metallic'].default_value = .08
    for img, socket in zip(images, ['Base Color', 'Roughness']):
        node = mat.node_tree.nodes.new('ShaderNodeTexImage')
        node.image = img
        node.interpolation = 'Linear'
        mat.node_tree.links.new(node.outputs['Color'], bsdf.inputs[socket])
    obj.data.materials.clear()
    obj.data.materials.append(mat)
    for poly in obj.data.polygons:
        poly.material_index = 0
    count.update({'object': obj.name, 'atlas_size': [SIZE, SIZE], 'pixels_per_metre': density,
                  'corrosion_mask_fraction': count['corrosion_texels']/count['painted_texels'],
                  'salt_mask_fraction': count['salt_texels']/count['painted_texels'],
                  'scope': 'Authored UV mask counts; NOT frame coverage or a visual pass'})
    texture_receipts.append(count)

bpy.context.view_layer.update()
measurements = {'asset_id': asset, 'revision': 'r03', 'status': 'blender-measured',
    'runtimeEligible': False, 'blender_version': bpy.app.version_string, 'initial_scene': initial,
    'tri_budget_target': 1500, 'texture_files': 4, 'objects': [],
    'material_target': {'frame_corrosion_max': .15, 'frame_salt_max': .12,
                        'frame_unworn_metal_max': .05, 'measured_frame_coverage': None},
    'texture_mask_receipts': texture_receipts,
    'texture_encoding': {'baseColor': 'explicit linear-to-sRGB PNG; sRGB import',
                         'roughness': 'linear grayscale PNG; Non-Color import'},
    'preview_lighting': 'unchanged from r02; texture encoding correction isolated',
    'runtime_texture_memory_bytes': None,
    'texture_memory_rgba8_no_mips_upper_bound_bytes': 4*SIZE*SIZE*4}
for obj in mesh_objects:
    obj.data.calc_loop_triangles()
    measurements['objects'].append({'name': obj.name, 'vertices': len(obj.data.vertices),
        'triangles': len(obj.data.loop_triangles), 'polygons': len(obj.data.polygons),
        'material_slots': len(obj.material_slots), 'location': list(obj.location),
        'rotation': list(obj.rotation_euler), 'scale': list(obj.scale),
        'bounds_local': [list(Vector(c)) for c in obj.bound_box],
        'uv_layers': [layer.name for layer in obj.data.uv_layers]})
measurements['triangles'] = sum(o['triangles'] for o in measurements['objects'])
assert len(mesh_objects) == 2 and measurements['triangles'] <= 1500
measurements['texture_disk_bytes'] = sum(p.stat().st_size for p in texture_dir.glob('*.png'))
(out / 'measurements.json').write_text(json.dumps(measurements, indent=2) + '\n')

# Export only the unchanged drawer hierarchy. GLB embeds textures; FBX points to the sidecar folder.
bpy.ops.object.select_all(action='DESELECT')
for obj in (root, tray, casing, moving):
    obj.select_set(True)
bpy.context.view_layer.objects.active = casing
bpy.ops.export_scene.gltf(filepath=str(out / (asset + '.glb')), export_format='GLB',
    use_selection=True, export_apply=True, export_yup=True, export_animations=False)
bpy.ops.export_scene.fbx(filepath=str(out / (asset + '.fbx')), use_selection=True,
    object_types={'MESH', 'EMPTY'}, axis_up='Y', axis_forward='-Z', global_scale=1.0,
    apply_unit_scale=True, apply_scale_options='FBX_SCALE_NONE', mesh_smooth_type='FACE',
    path_mode='RELATIVE', embed_textures=False)

# Isolated inspectable preview: no unrelated mesh, emission, bloom, screen flash or compositor FX.
scene.render.engine = 'CYCLES'
scene.cycles.samples = 32
scene.cycles.use_denoising = True
scene.render.resolution_x = 960
scene.render.resolution_y = 640
scene.render.resolution_percentage = 100
scene.render.image_settings.file_format = 'PNG'
scene.render.film_transparent = False
scene.world.use_nodes = True
scene.world.node_tree.nodes['Background'].inputs[0].default_value = (.028,.04,.042,1)
scene.world.node_tree.nodes['Background'].inputs[1].default_value = .7
scene.view_settings.view_transform = 'Standard'
scene.view_settings.exposure = 0
scene.view_settings.gamma = 1
camera_data = bpy.data.cameras.new('CAM_Drawer_r03_Preview')
camera = bpy.data.objects.new(camera_data.name, camera_data)
collection.objects.link(camera)
camera.location = (1.3,-1.8,1.13)
camera.rotation_euler = (Vector((0,0,.12))-camera.location).to_track_quat('-Z','Y').to_euler()
camera_data.type = 'ORTHO'
camera_data.ortho_scale = 1.58
scene.camera = camera
for name, pos, power, size in [('Key',(-1.7,-2.5,3),180,3), ('Fill',(2,1,2),90,2.5)]:
    data = bpy.data.lights.new('LIGHT_Drawer_r03_'+name, 'AREA')
    data.energy, data.shape, data.size = power, 'DISK', size
    obj = bpy.data.objects.new(data.name, data)
    collection.objects.link(obj)
    obj.location = pos
    obj.rotation_euler = (Vector((0,0,.12))-obj.location).to_track_quat('-Z','Y').to_euler()
scene.render.filepath = str(out / 'preview.png')
# Store relative image paths in the blend after exporting to make the review bundle portable.
for image in bpy.data.images:
    if image.filepath_raw and Path(image.filepath_raw).parent == texture_dir:
        image.filepath = '//textures/' + Path(image.filepath_raw).name
bpy.ops.wm.save_as_mainfile(filepath=str(out / (asset + '.blend')))
bpy.ops.render.render(write_still=True)
provenance = {'assetId': asset, 'revision': 'r03', 'provider': 'Blender CLI',
    'version': bpy.app.version_string, 'recipe': str(Path(__file__).resolve()),
    'recipe_sha256': hashlib.sha256(Path(__file__).read_bytes()).hexdigest(),
    'recipeSource': '_workspace/current/modeling/pipeline.md section 16.5',
    'decision': 'RFC-CX-003; director r03 texture/material correction instruction',
    'concept_ref': ['_workspace/current/concept/t0-source-drawer-review.md',
                    '_workspace/current/concept/style-guide.md'],
    'author': 'game-modeler recipe; director executes and audits',
    'license': 'Original procedural geometry and textures; no third-party source media',
    'runtimeEligible': False, 'promoted_by': None,
    'classification': 'Generated material-correction candidate; visual/import audit pending',
    'canonical_export': asset + '.glb', 'derived_import_candidate': asset + '.fbx',
    'hashes': {str(p.relative_to(out)): hashlib.sha256(p.read_bytes()).hexdigest()
               for p in sorted(out.rglob('*')) if p.is_file() and p.name != 'provenance.json'}}
(out / 'provenance.json').write_text(json.dumps(provenance, indent=2) + '\n')
print(json.dumps(measurements))
```

[OBSERVED 2026-09-10] Pure-Python authoring checks passed: AST syntax, exact sRGB roundtrip of frame `(54,86,92)` and recess `(23,50,56)`, PNG top/bottom row orientation, linear roughness byte `204` for `0.8`, repeatable seeded pattern preparation and nonidentical primary/secondary stroke tuples. The helpers were tested without Blender execution; these checks do not establish r03 export, preview quality, triangle totals or runtime appearance.

### 16.6 r03 executed candidate receipt — T0 visual ACK; runtime approval in §16.7

[OBSERVED 2026-09-10] The director executed the §16.5 recipe in Blender **5.1.2** and produced `assets/generated/3d/hub-view-drawer-r03/`. Status advances from recipe-ready to **generated T0 visual candidate**. The modeler independently read the actual measurement/provenance files, hashed every listed output and the executed recipe, parsed the actual GLB header/JSON index accessors, and read PNG dimensions on disk. No Blender process was launched for this receipt check.

| Verified item | Actual result | Evidence / boundary |
|---|---|---|
| Geometry | 2 meshes, 156 triangles: casing 60 + tray 96 | `measurements.json`; independently corroborated by GLB index accessor counts |
| Material / images | 2 GLB materials, 4 embedded images; 1 material slot per measured mesh | Actual GLB JSON and Blender receipt; not Unity draw calls |
| Texture files | 4 PNGs, each 1024 × 1024; total 787,100 bytes on disk | PNG headers and actual file lengths; runtime GPU memory remains unmeasured |
| Geometry preservation | Vertex/triangle/polygon counts, local bounds, position/rotation/scale match r01 exactly | Compared actual r01 and r03 measurement object fields; both revisions retained |
| Artifact integrity | All 9 listed output SHA-256 values match `provenance.json`; executed recipe SHA-256 also matches | `node:fs` bytes + `node:crypto` SHA-256, 9/9 outputs + 1/1 recipe |
| Canonical export | GLB, 1,017,988 bytes; header length matches actual file length | `SM_Hub_Workbench_Drawer.glb`; FBX remains the derived Unity-import candidate |
| Concept verdict | **T0 visual candidate ACK**; all four r02 actionable visual FIX items cleared | `concept/t0-source-drawer-review.md` section “r03 scoped re-review”; bounded actual-preview review |
| Runtime status | **runtimeEligible:true for T0 scene only** | Current source provenance and director RFC-CX-003 addendum; exact adapter and audit scope in §16.7. Generation-time receipt remains an immutable pre-approval snapshot. |

The actual preview SHA-256 is `3ad6437aab240fdb3d28d4c06bacfef42b44bf291f7a3235e3df81c8b1f81b90`, matching the concept reviewer’s cited image. The scoped ACK confirms irregular grouped corrosion, varied downward runs, interrupted salt clusters and readable structural/handle separation in that preview. It does not supply frame-level coverage percentages, runtime performance, animation approval or a production gate pass. UV-mask statistics remain authoring-space measurements; the 16 MiB RGBA8 base-level bound remains a calculated bound, not actual Unity memory. Earlier r01/r02 files and review dispositions are preserved.

### 16.7 r03 T0 runtime scene approval — exact candidate and placement

[OBSERVED 2026-09-10] `production/decision-log.md` addendum **“RFC-CX-003 addendum — r03 T0 runtime scene approval”** authorizes this exact r03 drawer for the **T0 scene only**. The director read the native Unity Metal `unity/Unknown/Builds/t0-diagnostics/import-material-fit-audit.json` and personally reviewed the isolated drawer, original solid-workbench interference, and workbench front-adaptation captures. The modeler read that audit, the director decision and current source provenance before updating this receipt. Source `.blend`/GLB/FBX/textures and r01/r02 remain preserved.

| Approved / observed contract | Exact result and scope |
|---|---|
| Imported drawer | **2 meshes, 156 triangles**, corroborated by the native Unity audit |
| Texture import | **4 × 1024²** maps; both BaseColor maps have `sRGB:true`, both Roughness maps `sRGB:false` |
| Renderer material binding | Both casing/tray renderers report BaseColor and Roughness maps bound and `shaderSupported:true`; imported metallic is `0.08` |
| Observed rendering context | **Metal**, project color space **Gamma**; this evidence does not establish another backend/color space |
| Unity adapter | **yaw 180°**, drawer position **(0, 0.32, 1.1)** |
| Workbench fit | Director-approved subtraction of the drawer AABB from the existing solid workbench front; comparison captures restore the visible recess without solid interference |
| Current runtime permission | Source `provenance.json`: **`runtimeEligible:true`**, `promoted_by:"game-production-director; RFC-CX-003 r03 T0 runtime scene approval"` |

This is a scoped asset/placement approval, not a new production identity, recovered world fact or authored animation. The diagnostic audit itself still says `runtimeEligible:false` and “no runtime promotion”: those fields are the preserved **pre-director-decision snapshot**, not the current permission state. The later director decision and source provenance govern current T0 runtime eligibility. The original Blender measurements likewise retain their generation-time eligibility field; no historical/source receipt was rewritten by the modeler.

Standalone **player** appearance still awaits direct inspection; the audit’s isolated-drawer capture is not a player validation claim. Final G4/G5, frame-level palette/wear budgets, full-scene geometry/draw calls, runtime GPU memory and performance remain unmeasured by this approval. The historical 144-triangle hub receipt and the 156-triangle drawer count are not a post-adaptation whole-scene measurement. No new asset revision or visual-polish scope is introduced.
