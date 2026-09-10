---
updated: 2026-09-11
cycle: 20260909-preproduction-c4
status: current
supersedes: null
owner: game-modeler
---

# C1 수문 계통판 — r01 제작 스펙

`c1-patrol-panel`은 제작용 asset ID다. 플레이어에게 새 고유명사를 도입하지 않는다. 캐논 명칭은 `planning/campaign.json`의 C1 / `c1-b1` / 단서 `c1-b1-c2`에 있는 **수문 계통판**이다.

## 입력과 적용 범위

- `concept_ref`: `concept/style-guide.md`, `planning/campaign.json#c1-b1-c2`, `production/decision-log.md`의 **RFC-CX-004**, 이 문서의 원본 시각 브리프.
- 전용 컨셉 시트가 아직 없으므로 디렉터가 스타일 가이드·캐논 단서·현재 브리프를 이 첫 비트 프로토타입의 시각 목표로 지정했다. 완성 아트·G4/G5 승인으로 확장하지 않는다.
- 원본 브리프: 해안 순찰용 작은 수문 계통판. 중앙 공통 공급부에서 조명과 판독이라는 서로 다른 형태의 두 분기가 이어진다. 청회 주물 외함, 아래로 흐른 녹청, 이음쇠 소금 고리. 조명은 둥근 보호망 렌즈, 판독은 직사각형 종이 삽입부. 글자·숫자·임계값 눈금·상표·외부 참고 이미지 없음.
- 작업자는 Blender MCP 연결을 찾지 못했으므로 레시피와 사양만 작성한다. 루트 실행자가 새 Blender CLI factory scene에서 생성하고 실측한다. 기존 사용자 씬을 열거나 삭제하지 않는다.

## 치수·피벗·기능 [TARGET]

| 항목 | 납품 규칙 |
|---|---|
| 단위 / 외함 | 1 Blender unit = 1m / 폭 1.10m × 높이 0.88m × 깊이 0.18m |
| 전체 치수 | 발·보호망을 포함하는 정확한 크기는 `measurements.json.bounds_m`에서 실측 |
| 피벗 | 바닥 중앙. Blender 정면 -Y, 위 +Z. FBX/GLB Y-up export |
| 분기 형태 | 왼쪽 원형 보호망 렌즈 / 오른쪽 직사각형 종이 삽입부. 색만으로 구분하지 않음 |
| 공급 연결 | 중앙 원형 공급부 + 좌우에 이어지는 관. 숫자 계기·압력 문턱값·추가 게임 규칙 없음 |
| 애니메이션 / 리그 | 정적 프롭, 없음. 손잡이는 움직이지 않으며 기능 판정은 시스템 레인 소유 |
| 충돌 | 파일에 미포함. Unity import adapter에서 정적 상호작용 영역을 지정·검증 |
| LOD | LOD0만. 노드 고정 시점 후보이며 LOD 감소 효과 미측정 |

## 표면·예산

| 항목 | 납품 규칙 |
|---|---|
| 기본 색 | `#36565C` 주물, `#173238` 홈, `#4F7A6B` 녹청, `#C8D6D3` 소금/자기, `#E7E3D8` 빈 종이, `#E2AF62` 렌즈 |
| 텍스처 | 주물/녹청 2개 표면 × BaseColor/Roughness = 4 × 1024² PNG |
| 색 공간 | BaseColor는 선형 연산 후 명시적 linear→sRGB PNG 부호화. Roughness는 선형 Non-Color |
| 마모 | 불규칙 다중 크기 노이즈·아래로 흐른 스트릭·부분 소금 군집. 규칙적인 점무늬를 사용하지 않음 |
| 머티리얼 | 6개 정적 메쉬 그룹, 렌즈는 비발광. 무채색/색약 형태 구분을 유지 |
| 삼각형 | 제작 상한 5,000 [TARGET]. 실제 `calc_loop_triangles` 합계는 실행 후 `measurements.json`에 기록 |
| 측정 경계 | 삼각형·파일 바이트는 Blender/파일 실측. 프레임 팔레트 점유율·드로콜·GPU·메모리·가독성·G4/G5는 미측정 |

## 생성·검증

레시피: `assets/generated/3d/scripts/build_c1_patrol_panel_r01.py`.

```sh
"/Applications/Blender.app/Contents/MacOS/Blender" --background --factory-startup --python-exit-code 1 \
  --python assets/generated/3d/scripts/build_c1_patrol_panel_r01.py
```

출력 폴더 `assets/generated/3d/c1-patrol-panel-r01/`가 이미 존재하면 레시피는 종료한다. 재시도 시 기존 결과를 삭제하거나 덮어쓰지 말고 새 리비전을 선택한다.

실행 전 상태는 **spec/recipe-ready**다. 생성 완료 여부와 실측 숫자는 출력 영수증으로만 판단한다. Blender는 기본 설정에서 Python 예외에도 프로세스 종료 코드 0을 반환할 수 있다. 따라서 `--python-exit-code 1`을 반드시 사용하고, 로그의 `C1_PANEL_GENERATION_COMPLETE`와 마지막에 쓰인 `SUCCESS.json`, provenance의 모든 출력 해시를 함께 확인한다. 텍스처나 종료 코드 0만으로 성공 처리하지 않는다.

2026-09-11 첫 루트 실행은 메쉬 join 이후 삭제된 Blender 객체 참조를 다음 머티리얼 그룹에서 다시 읽어 중단되었다. 해당 부분을 모든 그룹의 구성원을 join 전에 확정하는 방식으로 수정했다. 첫 시도의 텍스처는 루트 운영자가 `c1-patrol-panel-r01-attempt1/`로 보존하며 이 문서는 그 시도를 완료 납품으로 세지 않는다.

- [ ] 루트 운영자가 Blender CLI 실행·종료 코드를 보존
- [ ] `.blend`, `.glb`, `.fbx`, PNG 4장, `preview.png`, `prompt.txt`, `measurements.json`, `provenance.json`, `SUCCESS.json` 생성
- [ ] 삼각형·AABB·메쉬/머티리얼·UV·텍스처 바이트 실측
- [ ] 출력 및 레시피 SHA-256 검증
- [ ] Blender preview 원본 시각 검토 (Unity 캡처라고 표시하지 않음)
- [ ] Unity import·머티리얼 색 공간·정면·가림·형태 구분 검증
- [ ] 디렉터가 필요한 경우 해당 장면에 한정해 runtime 승격 기록

모든 생성 영수증은 `runtimeEligible:false`로 시작한다. 프로토타입 소스 납품은 런타임 통합이나 게임 완성 승인이 아니다.
