---
updated: 2026-09-10
cycle: 20260909-preproduction-c7
status: draft
supersedes: null
owner: game-modeler
---

## 2026-09-11 이미지 제공자 갱신 — RFC-CX-006

[OBSERVED] 사용자 최신 지시에 따라 신규 이미지 생성은 `god-tibo-imagen` 스킬/GTI를 사용한다. 아래 2026-09-10 MuAPI/Higgsfield 지정 및 GTI 대체 금지는 이미지에 한해 과거 기록이다. Blender 3D·Higgsfield 영상 경로와 기존 자산 출처는 유지한다. 인증 내용을 출력하지 않고 유효성만 확인한 뒤 `--dry-run`을 실행한다. 설치된 GTI 0.3.0은 참조 이미지에 `--image` 플래그를 사용하므로 실제 `--help`를 따른다. 요청 크기와 실제 출력 크기를 각각 기록하고, 결과는 provenance와 `runtimeEligible:false`로 시작한다.

# Asset Runbook — 리소스 재생성·확장·승격 절차 (C7 핸드오프)

외부 실행자(Codex GPT-6 Astra)와 아티스트가 **추가 질문 없이** 리소스를 다시 만들고, 늘리고, Unity 로 올릴 수 있게 하는 절차서다.
이 문서는 `handoff/` 소유권(systems·디렉터 승인) 아래에서 **모델러가 리소스 파이프라인 절에 한해** 작성 권한을 받아 쓴 것이다.

- 이 문서가 **소유하지 않는** 것: 예산 수치(`modeling/asset-budget.md`) · 자산 신원/상태/경로(`modeling/asset-manifest.md`) · 저작 규격(`modeling/pipeline.md`) · 스타일 규범(`concept/style-guide.md`) · 리그 규격(`animation/rig-requirements.md`) · 캠페인 정본(`planning/campaign.json`). 충돌하면 **그 문서가 이긴다.**
- 이 문서가 소유하는 것: 위 문서들을 **실행 순서로 잇는 절차**, 도구의 실제 동작·함정, provenance 스키마 대조표, 승격 감사 항목, 남은 리소스 목록, 금지, 미결.
- **모든 생성물은 컨셉/프리비즈다. 게임플레이가 아니다.** 47종 중 생산 완료 0종, 전건 `runtimeEligible:false`.

## 공급자 변경 — 2026-09-10 사용자 지시 (RFC-CX-002)

- [OBSERVED · 2026-09-10] 사용자의 최신 지시 `you use muapi and heiggsfield for resources`에 따라 앞으로 생성하는 리소스의 공급자는 **MuAPI와 Higgsfield**다. 이 지시는 이전 GTI 전용 지시보다 우선한다. 정본 규칙은 저장소 `CLAUDE.md` §10.2, 결정 기록은 `production/decision-log.md`의 RFC-CX-002를 따른다.
- **GTI로 자동 폴백하지 않는다.** 아래 §1.1과 GTI 재현 명령은 기존 산출물의 제작 이력·재현 참고로 보존하며, 신규 생성의 기본 경로로 실행하지 않는다. 기존 GTI/Blender/Higgsfield 자산의 공급자, ID, 해시와 provenance는 변경하거나 새 공급자로 재표기하지 않는다.
- [OBSERVED · 2026-09-10] Higgsfield CLI는 `/Users/jangyoung/.nvm/versions/node/v22.19.0/bin/higgsfield`에 설치되어 있고 `account status`가 종료 코드 0으로 완료됐다. 이 확인은 새 리소스 생성 완료를 의미하지 않는다. 사용 시 §1.3의 작업별 실제 지원 여부·비용·영수증 절차를 따른다.
- [OBSERVED · 2026-09-10] MuAPI 공식 [인증 문서](https://muapi.ai/docs/authentication), [CLI 문서](https://muapi.ai/docs/cli), [MCP 문서](https://muapi.ai/docs/mcp)는 HTTP 200으로 확인했다. 공식 CLI 패키지는 `muapi-cli`, 호스팅 MCP는 `https://api.muapi.ai/mcp`(Streamable HTTP, Bearer API key)다. 현재 세션에는 MuAPI CLI/MCP가 노출되지 않았고 셸의 `MUAPI_API_KEY`·`MUAPI_KEY`도 설정되지 않아 실제 인증 연결은 미완이다. 로컬 연결과 안전한 키 설정이 필요하며, 다른 저장소에 키가 없다는 뜻은 아니다. 인증 문서의 sandbox 키는 mock 응답을 내므로 실제 리소스 생성 증거로 쓰지 않는다.
- 두 공급자의 새 생성물도 §2의 provenance 기록과 §3의 승격 감사를 거친다. 최초 상태는 `runtimeEligible:false`이며, 공급자 지정이나 생성 성공만으로 런타임 사용을 승인하지 않는다. 이 변경은 공급자 선택을 기록하며 새 자산 생성이나 승격을 주장하지 않는다.

## 0. 착수 전 3분 — 이 세션에서 실제로 측정한 것 [OBSERVED · 2026-09-10]

| 확인 | 명령 | 결과 |
|---|---|---|
| provenance ↔ 파일 해시 | §7-1 스크립트 | **86/86 일치**, mismatch 0, 파일 없음 0 |
| 2D 생성물 | `find assets/generated/2d -name '*.png' \| wc -l` | **45** (프롬프트 `concept/prompts/` 도 45) |
| 3D 산출 | `ls assets/generated/3d/*.glb *.fbx *.blend \| wc -l` · `find assets/generated/3d/renders -type f \| wc -l` | **9 파일** (GLB 7 · FBX 1 · blend 1) · 렌더 **13** · provenance **23항목** |
| 영상 | `ls assets/generated/video/*.mp4` | **2 클립** |
| README 미디어 | `ls docs/media` | 미디어 **15** + `provenance.json` |
| Unity 임포트 | `find unity/Unknown/Assets -type f \| wc -l` | **0** — 임포트 실적 0건, 런타임 tri/drawcall/텍스처 상주 **미측정** |
| Unity 버전 | `cat unity/Unknown/ProjectSettings/ProjectVersion.txt` | **6000.5.6f1** (revision 0e0577a1a2ac) |
| Higgsfield 잔액 | `higgsfield account status` | `ultra plan, **128.88 credits**` — `decision-log.md` 영수증(생성 후 128.88)과 **일치**, 그 뒤 추가 소모 0 |
| Blender MCP | `get_addon_status` · `get_scene_info` | **연결됨**(폴백 모드: `up_to_date:false`, `source:"missing"`, 기대 프로토콜 5). 씬 `Scene` **오브젝트 20 · 재질 11**, 사용자 `Cube/Light/Camera` **존속**(삭제 0건) |
| Blender CLI | `which blender` | **없음** — 이 머신에서 Blender 는 **MCP 경유로만** 접근한다. 헤드리스 `blender --python` 경로는 미검증 |
| 보조 도구 | `which ffmpeg` · `python3 -c "import PIL"` | ffmpeg `/opt/homebrew/bin/ffmpeg` · Pillow **12.2.0** (둘 다 스크립트 전제조건) |

---

## 1. 도구 사실 — 문서가 아니라 실제 동작

### 1.1 GTI (2D 전량) — `scripts/gen-2d.sh`

```
scripts/gen-2d.sh <asset-id> <category> <size> <prompt-file>
#   category  = concept|ui|texture|keyart|capsule|readme|previz
#   → assets/generated/2d/<category>/<asset-id>.png + 같은 폴더 provenance.json
#   FORCE=1 이면 기존 파일을 덮어쓰고 provenance 를 같은 id 로 교체한다(항목 수 유지)
```

| 사실 | 값 | 함정 |
|---|---|---|
| 모델 | 이 계정 허용 = **`gpt-6-astra` 하나** (다른 모델 HTTP 400) [OBSERVED `concept/generation-manifest.md` L11] | `gti` 자체 기본값은 **`gpt-5.4`** [OBSERVED `gti --help`]. **`gti` 를 직접 호출하면 400 이 난다** — 반드시 `gen-2d.sh`(기본 `GTI_MODEL=gpt-6-astra`)를 쓰거나 `--model gpt-6-astra` 를 명시한다 |
| 백엔드 | Codex 비공식 경로 (`gti --help` 첫 줄이 스스로 경고) | 예고 없이 끊길 수 있다. 끊기면 `concept/generation-manifest.md` 가 재생성 계약서다 |
| `--size` | **강제되지 않는다** | 45장 중 **29장이 요청과 다른 해상도**로 반환됐다(내 측정: `size_note` 집계 = `backend ignored --size` **29** / `matches request` **16**). 예: `ui-status-badge-sheet` 1024×1024 요청 → **2056×765** |
| 소요 | 1장 **90.9초** 실측(이전 세션 스모크) | 45장 일괄 재생성 ≈ 70분. 병렬 2 · 호출 간 `sleep 2` · 실패 1회 재시도 후 `skipped` |
| 프롬프트 | `concept/prompts/<asset-id>.txt` (45개) | `style-guide.md` §2 팔레트 · §4 재질 · §5 카메라 문단을 **문자 그대로** 반복하고 §10 금지를 `NEGATIVE` 절로 싣는다. 새 프롬프트도 같은 5절 구조(SUBJECT/STYLE/CAMERA/SEQUENCE CONSISTENCY/NEGATIVE) |
| previz 체인 | `f01` 단독 생성 후 `f02~f09` 는 `gti --image <f01.png> --prompt … --model gpt-6-astra` 직접 호출 | provenance 는 `gen-2d.sh` 와 **동일 스키마**로 수동 append 하고 `claim` 끝에 `previz chain ref=…` 를 붙인다 |

**크기 후처리**: `scripts/refresh-2d-provenance.py` 는 픽셀에서 실제 크기를 읽어 `requested_size`/`actual_size`/`size_note`/`bytes` 를 채운다. 멱등이며 프롬프트·해시는 건드리지 않는다. **크기를 바꾸는 도구가 아니다** — Steam 규격(header 920×430 / library 600×900 등) 크롭·리사이즈 공정은 **아직 0건**이며 `concept/sheets/README.md` §9-4 가 그 신설을 요구한다.

### 1.2 Blender MCP (3D 그레이박스) — `assets/generated/3d/scripts/build_hub_greybox.py`

재실행 절차 (순서를 지킨다):

1. **씬을 먼저 조사한다** — `get_scene_info`. `get_objects_summary` 는 이 연결에서 타임아웃한다 [OBSERVED]. 사용자 오브젝트(`Cube`/`Light`/`Camera`)는 **숨기되 삭제하지 않는다**.
2. 스크립트를 `execute_blender_code` 로 **단계별** 실행한다. 파일 전체를 한 번에 보내지 않는다(폴백 애드온에서 페이로드가 잘린다).
   `build_all()` = `purge_greybox()` → `stage_00_setup` → `10_materials` → `20_shell` → `30_fixtures` → `40_tools` → `50_camera_lights` → `60_measure`.
   `purge_greybox()` 는 **`HUB_GREYBOX` 컬렉션 안만** 비운다 — 사용자 오브젝트는 그 컬렉션 밖이라 대상이 아니다. 즉 **재실행은 클린 리빌드이며 1회 실행 == 저장되는 `.blend` 상태**다.
3. `stage_70_export()` → GLB/FBX + 도구 6종 개별 GLB(원점 정렬 후 **씬 위치 복원**).
4. `stage_80_render()` → `renders/hub-cam.png`(1920×1080) + `renders/turntable/frame_00~11.png`(960×540, 30° 간격).
5. `stage_90_save()` → **새 `.blend`** (`assets/generated/3d/hub-greybox.blend`). 사용자의 미저장 기본 씬을 덮지 않는다.
6. `python3 assets/generated/3d/scripts/write_provenance.py` 재실행 → 해시 갱신. **손으로 편집 금지.**

내보내기 규격(실제 인자, `pipeline.md` §6 이 정본):

```
GLB : bpy.ops.export_scene.gltf(export_format='GLB', use_selection=True, export_apply=True, export_yup=True)
FBX : bpy.ops.export_scene.fbx(use_selection=True, global_scale=1.0, apply_unit_scale=True,
        apply_scale_options='FBX_SCALE_NONE', axis_forward='-Z', axis_up='Y',
        bake_space_transform=False, object_types={'MESH'}, use_mesh_modifiers=True,
        mesh_smooth_type='FACE', path_mode='COPY')
```

- 1 blender unit = 1 m. **회전·스케일은 굽고 위치는 굽지 않는다**(위치를 구우면 피벗이 월드 원점으로 끌려가 리그 계약이 깨진다).
- Blender 5.1 의 새 익스포터 `bpy.ops.wm.fbx_export` 로 바꾸면 provenance `export.operator` 가 바뀌고 **Unity 재검증이 필요**하다.
- 애드온이 구버전이다(`uvx blender-mcp install-addon` 으로 갱신 가능). **갱신은 사용자 결정** — 폴백으로 위 절차가 전부 동작함을 이번 세션에 확인했다.

### 1.3 Higgsfield CLI (영상 · 이미지→3D) — `scripts/gen-video-higgsfield.sh`

```
scripts/gen-video-higgsfield.sh <clip-id> <start.png> <end.png|-> "<prompt>" [duration=5] [model=seedance_2_0_mini]
```

- **크레딧을 소모한다.** 실행 전 `higgsfield account status` 로 잔액을 읽고, 실행 후 잔액을 다시 읽어 **차액을 영수증으로 제출한다.**
  `[C7-F12]` **실행자는 `production/decision-log.md` 를 쓰지 않는다**(디렉터 소유). 영수증은 `handoff/rfc-inbox/RFC-CX-{n}.md` 로 **제출**하고 **디렉터가 decision-log 에 append** 한다 — `decision-log.md` 「C6-F11 / PRE-1 / C7-F14 / C7-F12 / C7-F3」 판정 ⑥. 양식은 `handoff/rfc-inbox/README.md`.
  기존 영수증(디렉터가 이미 append 함): 2건 생성에 **25.0 credits**(153.88 → 128.88), 추정 12.5/건.
- 스크립트는 `generate cost` 로 견적을 먼저 찍고, `--wait --wait-timeout 20m --json` 결과를 `<clip-id>.create.json` 에, 표준에러를 `<clip-id>.stderr.log` 에 남긴다. mp4 URL 이 없으면 exit 4.
- **디렉터 승인 없이 실행하지 않는다.** 미실행 승인 대기 경로: `multi_image_to_3d`(히어로 프롭 3D 후보 1종), 추가 클립.
- `multi_image_to_3d` 는 **히어로 프롭에만** 쓴다. 공간 셸·구조물에는 쓰지 않는다(스케일·피벗·토폴로지 통제 불가). 산출물은 리토폴로지 여부·원본 이미지 경로+해시·모델명·`license: UNVERIFIED`·`runtimeEligible:false` 를 반드시 기록한다.
- Blender MCP 에 노출된 Hyper3D/Rodin·Hunyuan3D 생성 도구는 **이 프로젝트에서 승인된 경로가 아니다**(상태 조회조차 하지 않았다).

### 1.4 Mixamo — 조건부 미사용 (RFC-P4-001)

현 설계에 3D 휴머노이드 **0체**(인물은 2D 초상, locomotion 0). 따라서 업로드·다운로드·리깅 **0건**이며, 규격은 **잠금 상태로만** 유지한다: `modeling/pipeline.md` §8 + `animation/rig-requirements.md` §5(델타 D1 본 총수 ≤65 · D2 손가락 off · D3 루트모션 off/in-place · D4 클립 명명 `AN_<charId>_<clipId>.anim`).
**발동 조건은 RFC-P4-001 을 뒤집는 새 디렉터 RFC 하나뿐이다.** Mixamo 는 공개 API 가 없으므로 다운로드는 **사용자 수동**이고 에이전트가 자동화할 수 없다.

### 1.5 GIF 조립 — `scripts/make-previz-gif.sh`

```
scripts/make-previz-gif.sh <frames-glob> <out.gif> [fps=1.5] [width=960]
```
2-pass 팔레트(palettegen/paletteuse), 프레임은 **파일명 정렬 순서**(제로패딩 권장), 결과는 `docs/media/provenance.json` 에 `frames_glob`·`frame_count`·`fps`·`width`·`bytes`·`output_sha256`·`claim(NOT gameplay capture)` 로 자동 등재된다. 기존 산출: 턴테이블 GIF(12프레임) · 컨셉 컷씬 GIF(9프레임) · 영상 컷씬 GIF.
**주의**: `docs/media/*.jpg` 파생본 12장은 이 스크립트가 아니라 **손으로 append** 됐고 필드가 결손이다(§2.5).

---

## 2. 폴더 · 명명 · provenance 스키마

### 2.1 폴더

```
assets/generated/2d/<category>/   concept|ui|keyart|capsule|readme|previz   (+ provenance.json)
assets/generated/3d/              hub-greybox.{blend,glb,fbx} · SM_Tool_<toolId>.glb   (GLB 7 · FBX 1 — §3.3-0)
                                  renders/ · scripts/ · provenance.json
assets/generated/video/           <clip-id>.mp4 · .create.json · .stderr.log · provenance.json
assets/generated/previz/          조립 GIF(컨셉 체인) + provenance.json
assets/generated/audio/           (현재 0 파일)
docs/media/                       README 파생본(jpg/gif) + provenance.json
unity/Unknown/Assets/             승격 대상. 현재 0 파일
```

### 2.2 명명 (정본 `modeling/pipeline.md` §4)

```
SM_<zone>_<name>   정적 메시      MSH_<objectName>  메시 데이터블록
SM_Tool_<toolId>   도구 프롭      MAT_<Group>_<name> 재질
CAM_<zone>_<role>  카메라         LGT_<zone>_<role>  라이트
PT_<PersonName>    2D 초상        UI_<name>          UI 아트
ROOT_<propId> / JNT_<propId>_<part>   리그 노드 (animation/rig-requirements.md §2.1)
```

- `toolId` **6종은 캐논이며 신설 금지**: `circuit` `reader` `alignment` `routing` `corrosion` `seal`.
- zone 토큰 = 제작 라벨. 기존 5개(`Hub` `Gate3` `Lowland` `Quay` `Pump1`)는 glossary §7 파생 규칙으로 **유효 확정**(OPEN-M1 closed). 읽는 법: `Hub↔hub` · `Gate3↔gate` · `Pump1↔pump` · **`Quay↔dock`** · `Lowland↔lowland`. **새로 만드는 zone 토큰은 `zoneId` 값을 그대로 쓴다.**
- 용어집에 없는 고유명사는 파일명·오브젝트명에 쓸 수 없다. 상표 미확인 가제 문자열은 등록 여부와 무관하게 금지(§5).

### 2.3 provenance 4종 필드 표 — 내 측정 기준 결손 현황 [OBSERVED]

| 필드 | 2D (`2d/*/`) | 3D (`3d/`) | video | docs-media |
|---|---|---|---|---|
| `id` / `file` | ● | ● | ● | ● |
| 분류 | `category` | `asset_id` + `method` | `category` | (없음) |
| 도구 | `tool`(gti) + `backend` + `model` | `tool`(Blender 5.1.2 via MCP) + `source_script` | `tool`(higgsfield CLI) + `model` | `tool` — **12/15 결손** |
| 입력 | `prompt_file` + `prompt_sha256` | `source_script`(+ `export`·`objects`·`dims_m`) | `prompt` + `start_image` + `end_image` + `duration_s` | `derived_from` |
| 크기 | `requested_size` / `actual_size` / `size_note` / `bytes` | `bytes`(+ `tris_total`·`units`) | (mp4) | `width`/`height` 또는 `bytes` — **bytes 12/15 결손** |
| 해시 | `output_sha256` | `sha256` | `output_sha256` | `output_sha256` |
| 시각 | `generated_at` | `generated_at` | `generated_at` | **14/15 결손** |
| 비용 | — | — | `credits_estimate` + `source_url` | — |
| 권리 | `license`(UNVERIFIED) | `license`(original greybox) | `license`(UNVERIFIED) | (없음) |
| 승격 | `runtimeEligible:false` + `promoted_by:null` | 〃 (+ 폴더 헤더 `promotion_rule`) | `runtimeEligible:false` | `runtimeEligible:false` |
| 주장 | `claim` `[OBSERVED] … not gameplay` | 〃 | 〃 `NOT gameplay` | 〃 `NOT gameplay` |

- **2D 45 · 3D 23 · video 2 항목은 위 필수 필드 결손 0.** 결손은 `docs/media` 파생본과 `previz/` GIF 1건(`bytes` 등 4필드)에만 있다.
- 새 카테고리를 만들면 **위 4종 중 가장 가까운 스키마를 그대로 복제**한다. `schema: "provenance/v1"` 문자열을 바꾸지 않는다.
- **provenance 는 생성기로만 쓴다.** 3D 는 `write_provenance.py`, 2D 는 `gen-2d.sh` 내장 블록, video 는 스크립트 내장 블록. 파일이 바뀌면 생성기를 다시 돌린다 — **해시가 파일과 다른 provenance 는 없는 것과 같다.**

### 2.4 비-Markdown 산출물

JSON/CSV/HTML/PNG/GIF 에 Markdown frontmatter 를 넣지 않는다. 메타데이터는 같은 basename 의 `.meta.md` 또는 **폴더 단위 `provenance.json`** 이 갖는다(CLAUDE.md §10).

### 2.5 알려진 스키마 결손 (수정 소유자 미정 → §6-4)

`docs/media/provenance.json` 15항목 중 `tool` **12**, `bytes` **12**, `generated_at` **14** 누락. 해시는 15/15 정상이라 무결성은 유지된다. 이 폴더는 `assets/` 와 마찬가지로 모델링 레인 소유가 아니므로 **이 문서는 고쳐 쓰지 않고 보고만 한다.**

---

## 3. 승격 절차 — `runtimeEligible:false → true`

승격 = **(a) 디렉터 승격 감사 판정 + (b) `unity/Unknown/Assets/` 로 복사 + (c) provenance 플립**. 셋 다 있어야 승격이다. 에이전트가 임의로 올리지 않는다(CLAUDE.md §9).

### 3.1 감사 항목 (`handoff/rfc-inbox/RFC-CX-{n}.md` 로 **제출**한다 — 디렉터가 판정해 `decision-log.md` 에 append)

`[C7-F12]` 아래 표를 채운 RFC 를 **rfc-inbox 에 제출**한다. 실행자·모델러 어느 쪽도 `production/decision-log.md` 를 직접 편집하지 않는다(판정 ⑥ · `handoff/README.md` §3).

| # | 항목 | 통과 기준 | 현재 |
|---:|---|---|---|
| 1 | **라이선스** | 생성 백엔드 ToS 확인 결과가 기록됐는가. `license: UNVERIFIED` 인 채로는 승격 불가 | **미확인** — GTI(Codex 비공식)·Higgsfield 둘 다 `UNVERIFIED`. 상업적 사용 가부 판단은 **사용자 몫** |
| 2 | **해시 무결성** | §7-1 재검증이 mismatch 0 | **86/86 PASS** |
| 3 | **텍스트 부재** | 이미지 내 텍스트·숫자·로고 **100% 확대 검수**(썸네일 육안 아님). 대상 최소 3장: `char-seongchan-portrait`(온도계 눈금) · `readme-verb-seal`(책자) · `previz-f03`(일지) | **0건** — 육안 검수만 |
| 4 | **명명·용어집** | §2.2 규칙 + 용어집 등록 고유명사 + 가제 문자열 0 | 프롬프트 45개 전수 grep 0건(이전 세션) |
| 5 | **예산 실측** | Unity 런타임에서 뷰당 tri ≤300k · drawcall ≤150 · 텍스처 상주 ≤512 MiB 를 **측정**했는가 | **0건.** Blender 계산값 144 tris 는 런타임 관측값이 아니다 |
| 6 | **리그 대조** | `animation/rig-requirements.md` §7 1~3 항 ack 완료, 편차는 pipeline §8 에 기록 | **미완** — 그 문서 `status: draft`, 편차 검사 실적 0 |
| 7 | **세이브 참조** | 이름·재질·텍스처 로케일·세이브 참조 GUID 변경 검사(`asset-budget.md` 수입 게이트) | 해당 자산 0 |
| 8 | **플립 기록** | provenance `runtimeEligible:true` + `promoted_by:"RFC-…"` 를 **생성기 재실행으로** 기록 | 0건 |

**한 항목이라도 미충족이면 승격하지 않는다.** 특히 1·3·5 는 오늘 전부 미충족이므로 **현 시점 승격 가능 자산은 0종**이다.

### 3.2 경로 규칙 — 복사이지 이동이 아니다

```
assets/generated/3d/hub-greybox.glb            → unity/Unknown/Assets/_Project/Art/Meshes/Hub/SM_Hub_Shell.glb
assets/generated/3d/SM_Tool_<toolId>.glb       → unity/Unknown/Assets/_Project/Art/Meshes/Tools/SM_Tool_<toolId>.glb
(hub-greybox.fbx 는 참고본 — 승격 대상이 아니다. §3.3-0)
assets/generated/2d/concept/<id>.png           → unity/Unknown/Assets/_Project/Art/Textures/<Zone>/T_<name>.png
assets/generated/2d/ui/<id>.png                → unity/Unknown/Assets/_Project/Art/UI/UI_<name>.png
(초상)                                          → unity/Unknown/Assets/_Project/Art/Portraits/PT_<PersonName>.png
```

> **`[C7-F6]` 경로 정정 2026-09-10 R7**: 이전 판은 승격 대상을 `Assets/Art/**` 로 적었는데, 이는 `architecture-contract.md` §3 · 브리프 §③ 의 「`Assets/_Project/` 밖에는 서드파티·패키지 샘플만」과 정면 충돌이었다. 세 문서를 **`Assets/_Project/Art/**` 하나로** 맞췄다. 지금 바꾸는 비용은 **0** — `unity/Unknown/Assets/` 에 파일이 0개이고 승격된 자산도 **0종**이라 옮길 것도 끊길 `.meta` GUID 도 없다 [OBSERVED].

- **원본은 `assets/generated/` 에 남긴다.** 이동·삭제 금지(CLAUDE.md §2 "삭제는 없다"). provenance 해시는 원본을 가리킨다.
- Unity 가 만든 `.meta`(GUID)는 삭제하지 않는다. 같은 자산을 다시 승격할 때 `.meta` 를 지우면 GUID 가 새로 발급돼 **씬·프리팹·세이브 참조가 끊긴다**.
- `Library/ Temp/ Logs/ obj/ UserSettings/` 는 `.gitignore` 대상이다(`unity/Unknown/.gitignore` 확인됨). git commit/push 는 **사용자가 수행**한다.

### 3.3 Unity 임포트 설정

#### 3.3-0 포맷 — 정본은 GLB 하나 `[C7-F3]`

[OBSERVED 2026-09-10 · `production/decision-log.md` 「C6-F11 / PRE-1 / C7-F14 / C7-F12 / C7-F3」 판정 ⑦]
**GLB 가 런타임 플레이스홀더 정본이다 — 도구 6종 + 허브 1. FBX 는 허브 셸 참고용 1개뿐이며 `SM_Tool_*.fbx` 는 만들지 않는다.**
이전 판의 「승격 포맷 FBX 단일화」[INFERENCE] 는 **폐기**됐고 §6-5 도 닫혔다. 규격 정본은 `modeling/pipeline.md` §6.1.

| 포맷 | 파일 | 역할 |
|---|---|---|
| GLB | `SM_Tool_<toolId>.glb` ×6 · `hub-greybox.glb` | Unity 임포트·승격 대상 **정본** |
| FBX | `hub-greybox.fbx` ×1 | 허브 셸 참고(축·스케일 대조, 외부 DCC 교환). 승격 대상 아님 |
| FBX | `SM_Tool_*.fbx` | **만들지 않는다** |

재현: `ls -1 assets/generated/3d/ | grep -E '\.(glb|fbx|blend)$'` → GLB **7** · FBX **1** · blend **1** [OBSERVED 2026-09-10]. 판정과 현재 산출물이 일치하므로 **추가 내보내기 0건**이다.
**미결(막힘 아님, `pipeline.md` §6.2)**: Unity 6000.5.6f1 이 `.glb` 를 내장 임포터로 읽는지 미측정(임포트 실적 0). 첫 임포트에서 (a) 성공이면 패키지 추가 없음, (b) 실패면 glTFast 계열 패키지 추가가 필요하며 **패키지 추가는 RFC 사항**이다(`handoff/README.md` §2). 어느 쪽이든 `SM_Tool_*.fbx` 를 새로 만들어 우회하지 않는다 — 판정 ⑦ 위반이다.

**메시(GLB 정본 / FBX 참고)** — 정본은 `modeling/pipeline.md` §7 · §6.1. 요약: Scale Factor 1 · Convert Units **on** · Bake Axis Conversion **off**(이미 `axis_up='Y'` 로 내보냈다. 이중 변환 금지) · Import Cameras/Lights **off** · Read/Write **off** · Mesh Compression **Off** · Generate Colliders **off**(콜라이더는 단순 박스/캡슐 수동) · Material Creation Mode **None** · Normals Import / Tangents Calculate Mikk · Animation **off**.

- 위 임포트 값은 FBX 항목명 기준이다. **정본인 GLB** 는 임포터가 노출하는 항목만 같은 값으로 맞춘다(Scale 1 · Cameras/Lights off · Read/Write off · Material Creation None 상당). `Convert Units` 는 FBX 전용 항목이다(glTF 는 규격상 m).
- `[C7-F3 · closed]` 이전 판의 "[INFERENCE] Unity 는 `.glb` 를 기본 임포트하지 않으니 **FBX 단일화**" 제안은 디렉터 판정 ⑦ 로 **폐기**됐다(§3.3-0). GLB 임포트 경로 실측은 §3.3-0 미결로 남는다.
- 첫 임포트 시 **반드시 측정**: 실제 tri/vert, drawcall(SRP Batcher 포함), 텍스처 상주. 그 전까지 G5 는 `[TARGET]` 이며 통과할 수 없다.

**텍스처(PNG)**

| 용도 | Texture Type | sRGB | Mip | 압축 | Max Size |
|---|---|---|---|---|---|
| 알베도/컬러 | Default | **on** | on | BC7(데스크톱) | hero 2048 · 원거리 1024. **4K 기본값 금지** |
| ORM/마스크/러프니스 | Default | **off** (선형 데이터) | on | BC7 또는 BC4(단일 채널) | 알베도와 동급 이하 |
| 노멀 | **Normal map** | off(타입이 강제) | on | BC5 | 〃 |
| UI 프레임·아이콘 시트 | **Sprite (2D and UI)** | on | **off**(화면 공간에서 밉맵은 흐림만 만든다) | BC7 고품질 또는 무압축 | 실제 픽셀 크기 이상으로 올리지 않는다 |
| 인물 초상 `PT_*` | Sprite | on | off | BC7 | 슬롯 종횡비 확정 후(§6-1) |

- **NPOT 경고**: 생성 이미지 **29/45 가 요청과 다른 비정형 해상도**다. `Non-Power of 2 → ToNearest` 자동 스케일은 **구도를 왜곡**하므로 쓰지 않는다. 메시 텍스처는 **크롭·리사이즈 공정을 먼저 거쳐** 2의 거듭제곱으로 정렬한 뒤 임포트한다(그 공정은 아직 0건 — `concept/sheets/README.md` §9-4).
- 텍스처 없는 그레이박스는 재질 9종이 전부 단색이며 그 hex 는 **팔레트와 0/9 일치**한다 — 뷰포트 디버그 값이지 최종 재질 후보가 아니다. 최종 재질은 `style-guide.md` §2 팔레트 8색 + 예비 1 안에서만 만들고, 도구 구분은 **§9 형상·글리프**가 맡는다(색 단독 부호화 금지).

---

## 4. 남은 리소스 — 47종 중 pending 40

수량 계약(정본 `modeling/asset-budget.md`, 행 단위 `modeling/asset-manifest.md`): 셸 5 / 도구 6 / 초상 5 / 공용 소품 30 / UI 1 = **47**. 상태 **greybox 7 · pending 40 · 생산 완료 0**, 전건 `runtimeEligible:false`.
**생성된 2D 45장 · 3D 12메시(144 tris) · 영상 2클립은 이 47종의 진척이 아니다** — 컨셉·프리비즈다(C5-F10).

### 4.1 공간 셸 5 ↔ 컨셉 시트 대응

| id | zone | concept_ref (`concept/sheets/README.md` §2) | 상태 | 착수 |
|---|---|---|---|---|
| `SM_Hub_Shell` | Hub 당직실 | `space-hub-watchroom-mood` (+키아트 2) | **greybox** (6오브젝트 · 72 tris) | 실루엣·재질 재작업 가능 |
| `SM_Lowland_Shell` | Lowland 구염전 저지대 | `space-lowland-mood` | pending | **해제** |
| `SM_Quay_Shell` | Quay 냉동창고 부두 | `space-wharf-mood` | pending | **해제** |
| `SM_Pump1_Shell` | Pump1 제1양수장 | `space-pumphouse-one-mood` | pending | **해제** |
| `SM_Gate3_Shell` | Gate3 제3수문 | `space-gate-three-mood` | pending | **보류** — 시트 재생성 선행(§4.4-1) |

### 4.2 영웅 도구 6 (전부 greybox · 각 12 tris)

`circuit` `reader` `routing` `seal` = 최종 형상 **해제**. `alignment` `corrosion` = 시트가 스스로 재생성을 요구하므로 **최종 형상 확정 보류**(§4.4-2·3), 그레이박스는 유지.
치수·씬 위치·회전은 `asset-manifest.md` §2 표가 소유한다. 가동부·회전축·keyframe 슬롯은 `animation/rig-requirements.md` §2.2 [TARGET] 이며 **모델러 ack 전이다** — 축 부호를 반대로 구우면 클립이 거울로 재생된다.

### 4.3 나머지 pending

| 분류 | 수량 | 상태 | 차단 사유 |
|---|---:|---|---|
| 인물 초상 `PT_*` | 5 | pending | 2D·concept 레인 소유. 3D 작업 0(스프라이트 쿼드 2 tris 외). 슬롯 종횡비 미확정(§6-1) |
| 공용 소품 `SM_Kit_*` | 30 | pending | **전용 컨셉 시트 0.** 간접 참조(공간 무드 5 · readme 7 배경)만 존재 → OPEN-M5 |
| UI 프레임 `UI_ToolFrame` | 1 | pending | 2D·concept 레인 소유. `ui-workbench-frame` 이 요청 규격 미달(2048×1152 요청 → 1536×1024) |

소품 30 중 리그·클립이 이미 지목된 4행은 우선 착수 후보다: `SM_Kit_Wheel_Valve`(`valve_turn`) · `SM_Kit_Lever_Long`(`lever_throw`) · `SM_Kit_Crank`(`pump_spin`) · `SM_Kit_Panel_Hatch`(`shutter_raise` [TARGET]).
**클립은 있는데 프롭이 없는 3종**이 남아 있다(C4-F17, open): `drawer_open`(서랍) · `shutter_raise`(셔터) · `water_level`(수면 — vfx `water_rise` 와 소유 중복). 예산표에 **VFX 카테고리·견적 인일이 0**이고, `vfx/vfx-budget.md` 6효과에 **프롭 부착점 정의가 0건**이다(내 grep 확인).

### 4.4 재생성 권고 3장 (모델링 입력에 한정)

| 순위 | asset-id | 사유 | 재생성 명령 |
|---:|---|---|---|
| 1 | `space-gate-three-mood` | 배경 좌상단이 **성곽/아치교**로 읽혀 유럽 판타지 인상 — 캐논 밖. `SM_Gate3_Shell` 의 **유일한** concept_ref | `FORCE=1 scripts/gen-2d.sh space-gate-three-mood concept 1536x1024 _workspace/current/concept/prompts/space-gate-three-mood.txt` |
| 2 | `tool-alignment-hero` | 실루엣 분리가 약해 §3 "1비트 실루엣 구분" 미달 | `FORCE=1 scripts/gen-2d.sh tool-alignment-hero concept 1024x1024 …/tool-alignment-hero.txt` |
| 3 | `tool-corrosion-hero` | 결정이 **육각이 아님** — `style-guide.md` §9 형상 부호 위반 | `FORCE=1 scripts/gen-2d.sh tool-corrosion-hero concept 1024x1024 …/tool-corrosion-hero.txt` |

`concept/sheets/README.md` §9 의 1·2순위(`readme-verb-seal` README 삽화 · `capsule-library-candidate` Steam 캡슐)는 **모델링 입력이 아니다** — 대응 매니페스트 행 0개, 소유는 concept/presentation 레인(QA 자기정정으로 확정). 프롬프트 수정 없이 `FORCE=1` 만 주면 같은 프롬프트로 다시 뽑히므로, **사유가 프롬프트에 있으면 프롬프트를 먼저 고친다**(성곽 모티프 → NEGATIVE 절에 추가; 육각 결정 → SUBJECT 절 명시). 프롬프트 편집 소유는 concept 레인이다.

---

## 5. 금지 (위반 시 REDO, 예외 없음)

1. **상표 미확인 가제 문자열**(국문 기관 가제명·영문 코드네임)을 이미지·파일명·오브젝트명·번들명·상점명에 넣기. 폴더는 `unity/Unknown/`(코드네임=저장소명)이다.
2. **이미지 내 텍스트·숫자·로고·간판.** 모든 프롬프트에 `no text, no letters, no numbers, no logos, no signage` 계열 NEGATIVE 필수. **영어 간판을 텍스처에 굽지 않는다.** 로케일 문자열도 굽지 않는다.
3. **타 게임 HUD·선택 카드·폰트·레이아웃 복제.** 참고는 관찰이며 복제가 아니다.
4. **실존 재난 사진·보도 이미지·피해자 서사의 시각적 차용.**
5. 수상 월계수·평점·페스티벌 표기.
6. 전투·무기·유혈·유령·환영·예지(세계관에 없다) · 붉은 경고등으로 위험 표시(위험은 **수위선**으로 읽힌다) · 네온/글로시 CGI/렌즈 플레어/블룸/애니풍.
7. **생성물을 "게임플레이"라 부르기.** 파일명·캡션·provenance `claim` 에 `previz`/`NOT gameplay` 를 명시한다.
8. 색 **단독** 부호화. 모든 상태·매체·도구는 색 + 형태 + 위치의 3중 부호.
9. 사용자 Blender 오브젝트 삭제 · `provenance.json` 손편집 · `_workspace/archive/` 편집 · 승격 없는 `runtimeEligible:true`.

---

## 6. 미결 — 이 문서가 스스로 닫지 않는 것 (판정은 디렉터)

### 6-1. 초상 슬롯 종횡비 [concept 레인 open question]
초상 5장은 1024×1024 를 요청했으나 실제는 **1145×1374 또는 1024×1536** 이다. `sheets/README.md` §9-6 은 "정사각 슬롯이 확정되면 1:1 재생성 또는 크롭"이라 적었지만 **슬롯 규격 자체가 아직 없다**. 대화 UI 초상 슬롯의 종횡비를 `systems/game-ui-contract.json` 이 확정해야 재생성/크롭 중 무엇을 할지 정해진다. 영향: `PT_*` 5행 · UI 임포트 설정 · Steam 캡슐 공정.

### 6-2. previz ↔ concept/keyart 화법 분리 [concept 레인 open question]
`sheets/README.md` L120 [OBSERVED]: previz 9장은 윤곽선이 강한 일러스트 화법, concept·keyart 21+2장은 회화적 화법 — **나란히 두면 다른 게임처럼 보인다**. 같은 README·같은 상점 페이지에 섞지 말라는 관측만 있고 통일 방향은 미정(`presentation-director` 판정 사항, §9-7 "최대 9장 또는 21장 재생성"). 영향: README 현행 배치(이미 두 화법이 같은 페이지에 있다) · 캡슐 후보 · 재생성 비용 규모.

### 6-3. `space-gate-three-mood` 성곽 모티프 [concept 레인 open question]
배경 성곽/아치교가 캐논 밖. 재생성만 하면 되는지, **프롬프트의 NEGATIVE 절을 먼저 고쳐야 하는지**가 미정이다(같은 프롬프트로 `FORCE=1` 하면 같은 모티프가 재발할 수 있다). 이것 하나가 `SM_Gate3_Shell` 착수를 막는 **유일한** 차단이다.

### 6-4. `docs/media` provenance 스키마 결손 (§2.5) — 수정 소유자 미정
15항목 중 `tool` 12 · `bytes` 12 · `generated_at` 14 누락. 모델링 레인 소유가 아니다.

### 6-5. 승격 포맷 — **닫힘 (2026-09-10 R7 · 디렉터 판정 ⑦)** `[C7-F3]`
「FBX 단일화」[INFERENCE] 는 폐기. **GLB 가 정본**(도구 6 + 허브 1), FBX 는 허브 셸 참고 1개, `SM_Tool_*.fbx` 는 만들지 않는다 → §3.3-0 · `pipeline.md` §6.1.
남는 것은 **미결이 아니라 측정 과제**다: Unity 내장 임포터의 `.glb` 처리 여부(§3.3-0 (a)/(b)). 패키지가 필요하면 그때 RFC 를 낸다.

### 6-6. 이월된 레인 열린 항목
| id | 내용 | 소유 |
|---|---|---|
| OPEN-M5 | 공용 소품 30 전용 시트 0 — (a) 시트 생성 또는 (b) "§8 constraints + 공간 무드로 충분" 명시 회신. 모델러가 (b)를 스스로 선언하지 않는다 | concept |
| OPEN-M6 | 카메라 규범 불일치 **부감각 17.0°**(컨셉 [TARGET] −18° ↔ 모델링 [OBSERVED] 34.998°) · **높이 3.95 m**(1.55 ↔ 5.501). 렌즈는 1 mm 차로 무시 가능 | concept · presentation · director |
| C4-F17 | VFX 카테고리·견적 인일 0 · 드로콜↔emitter 배분 규칙 0 · 부착점 0 · 클립 대상 프롭 3종 부재 | modeling · vfx · animation |
| RFC-M3 | **closed 2026-09-10 R7** — `build_hub_greybox.py` L18~22 주석을 현재 사실로 정정(style-guide 존재 · 9색은 디버그 전용 · 팔레트 교집합 0). 색 값·로직 불변이라 Blender 재실행 불요, `write_provenance.py` 재실행으로 `source_script` sha 갱신(23항목 · 전수 86/86 일치) | modeling (완료) |
| rig ack | `rig-requirements.md` §7 1~6 항 ack — 그 문서가 `status: draft` 라 대조·편차 기록 실적 **0건** | modeling ↔ animation |

---

## 7. 재현 명령 모음 (Codex 가 그대로 실행한다)

**7-1. provenance ↔ 파일 해시 전수 검증** — 승격 감사 2번 항목:
```bash
python3 - <<'PY'
import json,glob,os,hashlib
def sha(p):
    h=hashlib.sha256()
    with open(p,'rb') as f:
        for c in iter(lambda:f.read(1<<20),b''): h.update(c)
    return h.hexdigest()
tot=ok=bad=miss=0
targets=[("assets/generated/3d/provenance.json","sha256")]+[(p,"output_sha256") for p in
  glob.glob("assets/generated/2d/*/provenance.json")+
  ["assets/generated/previz/provenance.json","assets/generated/video/provenance.json","docs/media/provenance.json"]]
for prov,key in targets:
    base=os.path.dirname(prov)
    for a in json.load(open(prov))["assets"]:
        f=os.path.join(base,a["file"]); tot+=1
        if not os.path.exists(f): miss+=1; print("MISSING",f); continue
        if sha(f)==a[key]: ok+=1
        else: bad+=1; print("MISMATCH",f)
print(f"entries={tot} hash_ok={ok} mismatch={bad} missing={miss}")
PY
```
기대값(2026-09-10): `entries=86 hash_ok=86 mismatch=0 missing=0`.

**7-2. 수량·상태 재측정**
```bash
find assets/generated/2d -name '*.png' | wc -l          # 45
ls _workspace/current/concept/prompts | wc -l           # 45
find assets/generated/3d/renders -type f | wc -l        # 13
find unity/Unknown/Assets -type f | wc -l               # 0 (임포트 실적)
cat unity/Unknown/ProjectSettings/ProjectVersion.txt    # 6000.5.6f1
```

**7-3. 크기 미준수 집계**
```bash
python3 -c "import json,glob,collections;c=collections.Counter();[c.update([a.get('size_note')]) for p in glob.glob('assets/generated/2d/*/provenance.json') for a in json.load(open(p))['assets']];print(dict(c))"
# {'backend ignored --size': 29, 'matches request': 16}
```

**7-4. 그레이박스 재질 ↔ 팔레트 대조**(임시 6색이 최종 후보가 아님을 재확인): `build_hub_greybox.py` 의 `ACCENT` 6 + `GREY_*` 3 을 파싱해 `style-guide.md` §2 팔레트 9값과 집합 비교 → 교집합 **0/9**.

**7-5. 캠페인 정본**: 집계·sha 는 `node _workspace/current/planning/validate-campaign.mjs` **출력에서만** 읽는다. 고정 sha 숫자를 문서에 재기재하지 않는다(RFC-Q1).

**7-6. Higgsfield 실행 전후**: `higgsfield account status` → 생성 → 다시 `higgsfield account status` → **차액을 `handoff/rfc-inbox/RFC-CX-{n}.md` 에 영수증으로 제출**(디렉터가 `decision-log.md` 에 append). `[C7-F12]` 실행자는 decision-log 를 쓰지 않는다.

---

## 8. English summary (one section, for the external executor)

**Scope.** Procedure for regenerating, extending and promoting art assets. Ownership of numbers stays with `modeling/asset-budget.md` (budget), `modeling/asset-manifest.md` (identity/status/path), `modeling/pipeline.md` (authoring spec), `concept/style-guide.md` (visual norm), `animation/rig-requirements.md` (rig). This runbook only sequences them.

**Everything generated so far is concept / pre-visualization, not gameplay.** 47 contracted assets: 7 greybox, 40 pending, 0 production-complete, all `runtimeEligible:false`. The 45 generated 2D images, 12 greybox meshes (144 tris, Blender-computed) and 2 video clips are **not** progress against those 47.

**Tools.** 2D: `scripts/gen-2d.sh` wrapping GTI — this account accepts **only `--model gpt-6-astra`** while `gti`'s own default is `gpt-5.4`, so a direct `gti` call returns HTTP 400; `--size` is advisory and **29 of 45** images came back at a different resolution. 3D: Blender via MCP only (`which blender` finds no CLI); re-run `assets/generated/3d/scripts/build_hub_greybox.py` stage by stage — `purge_greybox()` clears only the `HUB_GREYBOX` collection, never the user's `Cube/Light/Camera`; export args are fixed (GLB `export_yup=True`; FBX `axis_up='Y'`, `axis_forward='-Z'`, no bake). Video: `scripts/gen-video-higgsfield.sh` **spends credits** — read `higgsfield account status` before and after and file the delta as a receipt in `handoff/rfc-inbox/RFC-CX-{n}.md` — the executor never writes `production/decision-log.md`; the director appends the ruling there (balance today: 128.88). Mixamo stays **conditionally unused** (RFC-P4-001) because the design has zero 3D humanoids. GIFs: `scripts/make-previz-gif.sh`.

**Provenance.** Four schemas (2D / 3D / video / docs-media), written by generators only, never by hand. Measured today: 2D 45, 3D 23, video 2 entries have **no missing required fields**; `docs/media` is the loose one (12/15 lack `tool` and `bytes`, 14/15 lack `generated_at`). All 86 file hashes verify.

**Promotion** = director audit ruling (submitted via `handoff/rfc-inbox/`, appended by the director to `production/decision-log.md`) + copy into `unity/Unknown/Assets/` + provenance flip, all three. Today **zero assets qualify**: licences are `UNVERIFIED`, no 100 % zoom text inspection has been done, and there is no runtime tri/draw-call/texture measurement (Unity import count is 0). Copy, never move; never delete `.meta` files (GUIDs break scene and save references). Import settings: meshes per `pipeline.md` §7 and §6.1 (Scale 1, Bake Axis Conversion off, Material Creation None; **Convert Units applies to the FBX reference only** — glTF is metres by specification); textures sRGB on for colour, off for data, mips off for UI sprites, and **crop/resize before import** because most generated images are non-power-of-two.

**Format (settled 2026-09-10, ruling ⑦):** **GLB is the canonical runtime placeholder** — 6 tools + the hub. The single `hub-greybox.fbx` is a reference copy only, and **`SM_Tool_*.fbx` files are not produced**. What remains is a measurement, not a decision: whether Unity 6000.5.6f1 imports `.glb` natively; if it does not, adding a glTFast-class package is an RFC, never a silent switch back to FBX.

**Open, not decided here** (director rules): portrait slot aspect ratio; previz-vs-concept rendering-style split; the castle/arch motif in `space-gate-three-mood` that blocks `SM_Gate3_Shell`; `docs/media` provenance gaps; plus carried items OPEN-M5, OPEN-M6, C4-F17 and the pending rig ack. **RFC-M3 is closed** — the greybox script's colour comment now states the measured fact (style guide exists; the 9 debug colours share 0 values with its palette).

---

## 9. 이 문서가 주장하지 않는 것

- **Unity 안에서 무엇이 어떻게 보이는지 모른다.** 임포트 0건, 런타임 지표 0건. §3.3 의 임포트 설정은 근거 있는 `[TARGET]` 이며 첫 임포트에서 실측으로 교체해야 한다.
- **라이선스를 확인하지 않았다.** 두 생성 백엔드 모두 `UNVERIFIED`.
- **플레이·재미·8시간을 검증하지 않았다.** 이 문서는 리소스 절차서다.
- **Blender 헤드리스 실행을 검증하지 않았다.** 이 머신에서 Blender 는 MCP 경유로만 접근된다.
- **소품 30·초상 5·UI 1 의 착수 판정을 내리지 않았다.** 소유 레인의 회신·판정이 선행한다.
