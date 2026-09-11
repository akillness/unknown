---
updated: 2026-09-10
cycle: 20260909-preproduction-c4
status: current
supersedes: null
owner: game-modeler
---

# Model Spec — `SM_Hub_Shell` + 도구 6종 (당직실 그레이박스)

[OBSERVED] 이 스펙의 치수·좌표는 실제로 만들어진 `assets/generated/3d/hub-greybox.blend`에서 읽은 값이다. **그레이박스이며 최종 아트가 아니다. 게임플레이 자산이 아니다.**

## 1. 신원

| 항목 | 값 |
|---|---|
| asset_id | `SM_Hub_Shell` (셸) + `SM_Tool_<toolId>` × 6 |
| zone | `Hub` (용어집: 당직실 / Watch Room) |
| concept_ref | `concept/art-direction.md` §공간별 감각과 기능 — 당직실 행. **전용 컨셉 시트 없음** |
| spec_ref | `planning/feature-specs/verb-01..06`, `planning/gdd.md` §도구 6종 |
| 소스 | `assets/generated/3d/scripts/build_hub_greybox.py` (실행됨) |
| 산출 | `hub-greybox.blend` / `.glb` / `.fbx` / `SM_Tool_*.glb` ×6 / 렌더 13장 |
| runtimeEligible | **false** (전 파일) |

## 2. 치수와 좌표계

- 단위 1 m. Blender Z-up. 원점 = 방 중앙 바닥면(바닥 상면 z=0).
- 방 6.00 m (X) × 8.00 m (Y), 벽 높이 2.60 m, 벽 두께 0.12 m.
- 벽 3면: 북(+Y) · 서(-X) · 동(+X). **남(-Y)과 천장은 없다** — 2.5D 고정 시점의 개방면.
- 작업대 2.40 × 0.90 × 0.90, 중심 (0.00, 1.20), 상면 z = 0.90.
- 염판 선반 0.36 × 1.80 × 1.15, 중심 (2.55, 0.75) — 동쪽 벽 앞.
- 도구 6종 치수·좌표·회전은 `modeling/asset-manifest.md` §2 표.

## 3. 피벗

전 오브젝트 **바닥 중심**. 회전·스케일은 메시에 구워 오브젝트 트랜스폼이 identity다 [OBSERVED: rot (0,0,0), scale (1,1,1) × 12개]. 도구 프롭 개별 GLB는 월드 원점 정렬본이다.

## 4. 재질 슬롯

오브젝트당 슬롯 1개. 텍스처 0장(단색).

| 재질 | 대상 | 색 (sRGB) | 비고 |
|---|---|---|---|
| `MAT_Grey_Floor` | 바닥 | `#7E7E7A` | |
| `MAT_Grey_Shell` | 벽 3면 | `#9A9A96` | |
| `MAT_Grey_Fixture` | 작업대·선반 | `#8C8C88` | |
| `MAT_Tool_circuit` | 회로 지도 패널 | `#3F8EA8` | **임시** |
| `MAT_Tool_reader` | 판독기 | `#7E6FB0` | **임시** |
| `MAT_Tool_alignment` | 조위정합 콘솔 | `#4FA07A` | **임시** |
| `MAT_Tool_routing` | 배수 편성 패널 | `#C4703F` | **임시** |
| `MAT_Tool_corrosion` | 부식 시험대 | `#B0566B` | **임시** |
| `MAT_Tool_seal` | 서명대 | `#8A8F5C` | **임시** |

임시 6색의 근거: `concept/style-guide.md`가 없다 [OBSERVED]. `art-direction.md`의 의미색(변경예정 `#E2AF62`, 확정 `#173238`)을 도구 식별에 재사용하면 의미가 충돌하므로 그 두 값을 피해 색상만 6방향으로 벌렸다. **색은 유일한 식별 채널이 아니다** — art-direction의 "색과 함께 아이콘·문자·선형" 규칙은 최종 아트에서 지켜야 하며 그레이박스는 그 규칙을 아직 만족하지 못한다.

## 5. 콜리전

박스 6개(바닥·벽3·작업대·선반) + 도구 6개 박스. 메시 콜라이더 금지. 형상이 박스라 1:1 대응.

## 6. LOD

**LOD0 단일.** 카메라가 고정이라 거리 변화가 없다. LOD1/LOD2는 Unity 실측에서 필요가 증명되기 전에는 만들지 않는다.

## 7. 카메라·조명 (씬에 포함, GLB/FBX에는 미포함)

- `CAM_Hub_Fixed` — 위치 (0.0, -5.30, 5.501), 조준 (0.0, 1.20, 0.95), 부감 35°, 34 mm.
- `CAM_Hub_Turntable` — 같은 반경 6.5 m·높이, 30° 간격 12위치.
- `LGT_Hub_Key` / `LGT_Hub_Fill` / `LGT_Hub_Rim` — 에어리어 3점, 190 / 80 / 130 W.
- 조명·카메라는 Unity 임포트에서 **제외**하고 Unity 씬이 다시 만든다(`pipeline.md` §7).

## 8. 납품 체크리스트

- [x] 이름이 `SM_<zone>_<name>` / `SM_Tool_<toolId>` 규칙과 캐논 toolId 6종을 지킨다
- [x] 트랜스폼 identity, 피벗 바닥 중심
- [x] 삼각형 수를 셈: 오브젝트당 12, 합계 144
- [x] GLB + FBX 내보냄, 도구 6종 개별 GLB 내보냄
- [x] 렌더를 눈으로 확인(6도구 동시 가시)
- [x] `provenance.json` 생성기로 기록, 전 항목 `runtimeEligible:false`
- [x] 새 `.blend`로 저장, 사용자 기본 씬 오브젝트 삭제 0건(숨김 처리)
- [ ] 컨셉 시트 도착 후 실루엣·재질 재작업 — **미착수(차단)**
- [ ] Unity 임포트 및 tri/drawcall 실측 — **미착수**
- [ ] `animation/rig-requirements.md` 대조 — **파일 없음**
