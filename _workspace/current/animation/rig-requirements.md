---
updated: 2026-09-10
cycle: 20260909-preproduction-c5
status: draft
supersedes: null
owner: game-animator
---

# Rig Requirements — 리그 납품 규격 (애니메이션 → 모델링 인계)

## 0. 이 문서의 지위 · 실측 상태

**왜 생겼는가 (모델러 OPEN-M5)** [OBSERVED 2026-09-10]
- `modeling/pipeline.md` §8 마지막 줄: "`animation/rig-requirements.md`는 아직 없다 [OBSERVED]. 생기면 이 표를 그 파일 기준으로 다시 맞추고 편차를 여기 기록한다."
- `modeling/specs/hub-watchroom.md` L82: "`animation/rig-requirements.md` 대조 — **파일 없음**" (미충족 체크박스).
- `production/premium-preproduction-contract.md` Asset pipeline 표(휴머노이드 리깅·모션 행): "`animation/rig-requirements.md`가 Mixamo 호환 규격을 명시".
- 재측정: `grep -rn "OPEN-M5" _workspace/current/` → **0 hit**. 즉 OPEN-M5 라는 **id 자체는 아직 어느 파일에도 등재되어 있지 않다**(등재는 modeling 레인 소유). 이 문서는 위 세 인용이 가리키는 **부재**를 해소한다.

**실측 [OBSERVED]**: 리깅 0건 · Unity Animator Controller 0개 · `.anim` 클립 파일 0개 · Mixamo 접속 0회 · 재생 프레임 캡처 0장 · 손 자산 0개. 이 문서의 모든 시간·본 수·각도는 **[TARGET]** 이며, 자산의 존재/치수/피벗만 타 레인 문서 인용의 [OBSERVED] 다.

**frontmatter 근거**: 신설 파일이므로 대체가 아니다(`supersedes: null`). `cycle` 은 이 레인의 live 문서 `animation/animation-contract.md` 와 같은 `20260909-preproduction-c5` 를 쓴다. `status: draft` 는 C3-F33("R4/R5 에서 QA 가 검증한 문서를 소유 레인이 같은 cycle 값 그대로 current 로 올린다")에 따라 **검증 전 자기 승격을 하지 않기** 위함이다.

**인용 정책(C3-F33)**: draft 문서를 인용할 때 **(C4/C5 검증 대기)** 를 붙인다. 이 문서가 인용하는 draft = `animation/animation-contract.md` · `motion/motion-contract.md` · `vfx/vfx-budget.md` · `systems/interaction-rules.md` · `systems/unity-implementation.md`. current 인용 = `modeling/pipeline.md` · `modeling/asset-manifest.md` · `modeling/asset-budget.md` · `concept/art-direction.md` · `concept/style-guide.md`.

## 1. 설계 전제 — 리그가 **요구하지 않는** 것

| 항목 | 수량 | 근거 [OBSERVED 인용] |
|---|---:|---|
| 3D 휴머노이드 | **0** | `modeling/pipeline.md` §8 · RFC-P4-001 |
| locomotion / 보행 / NavMesh | **0** | `animation/animation-contract.md` (C4/C5 검증 대기) · `systems/unity-implementation.md` §8 (C4/C5 검증 대기) |
| 립싱크 · 눈 깜빡임 루프 | **0** | `concept/art-direction.md` "걷는 주인공/전투/립싱크를 만들지 않는다" |
| 블렌드셰이프 · 2D 스켈레톤(PSD Importer/Anima2D) | **0** | `modeling/asset-budget.md` "초상 5×3 — 2D, 별도 리깅 없음" |
| 스킨 메시 | **0** (프롭은 부모-자식 계층) | 본 문서 §2 |
| 루트 모션 | **0** | 고정 2.5D 노드 카메라, 프롭은 제자리 |

→ 이 프로젝트가 실제로 요구하는 리그는 **세 종류뿐**이다: **(A) 도구·소품 프롭 리그**, **(B) 조건부 손 표현**, **(C) 2D 초상 시트(리그 아님)**. 그 밖에 (D) 휴머노이드 규격은 **RFC 가 열릴 때만** 발동하는 잠금 규격이다.

## 2. (A) 도구 6종 프롭 리그 규격

### 2.1 공통 규격 (6종 전부)

| 항목 | 규격 | 근거 |
|---|---|---|
| 피벗 | **바닥 중심 = 도구 원점** | `modeling/pipeline.md` §3 표 1행 |
| 단위 | 1 blender unit = 1 m, 오브젝트 스케일 apply | pipeline §2 |
| 회전/스케일 | 내보내기 전 identity (rot=(0,0,0), scale=(1,1,1)) | pipeline §2 [OBSERVED 12메시 확인됨] |
| 축 | Blender Z-up 저작 → FBX `axis_up='Y'`, `axis_forward='-Z'`. **씬에서 미리 눕히지 않는다** | pipeline §2 |
| 개별 파일 | `SM_Tool_<toolId>.glb` 는 월드 원점 정렬본, **리그 계층 포함** | pipeline §3 3행 |
| 가동부 표현 | **오브젝트 부모-자식 계층**(empty/축 오브젝트). 스킨·웨이트·IK·컨스트레인트 **금지** | 가동부가 전부 강체 회전/직선이동이라 스킨이 이득 0 |
| 가동부 상한 | 프롭당 **≤ 8 노드** [TARGET] | 예산 방어선. 초과 시 애니메이터에 RFC |
| 명명 | 루트 `ROOT_<propId>` · 가동부 `JNT_<propId>_<part>` (`part` 는 소문자 snake) | Unity 경로 문자열이 곧 애니메이션 바인딩 경로다 |
| 계층 깊이 | 1단 권장 · 최대 2단 | 깊은 계층은 바인딩 경로가 길어져 리네임 사고를 만든다 |
| 스케일 키 | **금지** (비균등 스케일은 노멀·배칭을 깬다) | pipeline §10 |
| 잠금 | keyframe 슬롯 **이외의 모든 채널은 잠근다** — 애니메이터는 잠기지 않은 채널에만 키를 놓는다 | 클립 회귀 방지 |

`toolId` 6종은 캐논이며 신설하지 않는다: `circuit` · `reader` · `alignment` · `routing` · `corrosion` · `seal` [OBSERVED `modeling/pipeline.md` §4].

### 2.2 프롭별 가동부 · 회전축 · keyframe 슬롯

치수·씬 위치·회전은 `modeling/asset-manifest.md` §2 표 [OBSERVED]. 아래 **가동부·축·범위·클립 결합은 애니메이션 레인의 [TARGET] 제안**이며 모델러 ack 대상이다(§7).

| 자산 id | 가동부 노드 | 축 · 범위 [TARGET] | keyframe 슬롯 | 결합 클립 |
|---|---|---|---|---|
| `SM_Tool_circuit` | **없음** (판독 전용) | — | 없음 (`ROOT_` 만 납품) | **0** — 상태 표현은 재질·UI 오버레이가 담당 |
| `SM_Tool_reader` | `JNT_reader_tray` · `JNT_reader_dial` | tray: 로컬 +Y 직선 0 → 0.12 m / dial: 로컬 Z 회전 ±150° | tray.position.y · dial.rotation.z | `plate_insert` |
| `SM_Tool_alignment` | `JNT_align_needle` · `JNT_align_dial` | needle: 로컬 Z 회전 −60° → +60° / dial: 로컬 Z 회전 ±180° | needle.rotation.z · dial.rotation.z | `gauge_settle` |
| `SM_Tool_routing` | `JNT_routing_conn_01`~`_08` · `JNT_routing_bypass` | conn: 로컬 Z 직선 0 → 0.008 m (스냅 삽입) / bypass: 로컬 X 회전 0 → −35° | conn_NN.position.z · bypass.rotation.x | `connector_snap` |
| `SM_Tool_corrosion` | `JNT_corr_needle` · `JNT_corr_lid` | needle: 로컬 Z 회전 0 → +210° (게이지 0 → 상한 9) / lid: 로컬 X 힌지 0 → +72° | needle.rotation.z · lid.rotation.x | `gauge_settle`(needle 인스턴스). lid 는 **클립 미배정 — 정적** |
| `SM_Tool_seal` | `JNT_seal_head` · `JNT_seal_arm` | head: 로컬 Z 직선 0 → −0.06 m / arm: 로컬 X 회전 0 → −12° | head.position.z · arm.rotation.x | `stamp_down` |

- 도구 6종 중 클립 대상은 **4종**(`reader`·`alignment`·`routing`·`seal`) + `corrosion` 의 게이지 바늘 1슬롯. `circuit` 은 가동부 0 이다 — 없는 움직임을 만들지 않는다.
- `gauge_settle` 은 **한 클립을 두 프롭(alignment · corrosion)에 인스턴스화**한다(계약의 재사용 원칙).
- 각도 부호는 **로컬축 오른손 규칙** 기준이며, 모델러가 축 방향을 반대로 굽으면 클립이 거울로 재생된다 → §7 ack 항목.

### 2.3 zone 프롭(공용 키트) 리그

`modeling/asset-manifest.md` §4 가 이미 클립을 지목한 행 [OBSERVED]: `SM_Kit_Lever_Long` → `lever_throw` · `SM_Kit_Wheel_Valve` → `valve_turn` · `SM_Kit_Crank` → `pump_spin`(연계). 애니메이션 레인 추가 제안 [TARGET]: `SM_Kit_Panel_Hatch` → `shutter_raise`.

| 자산 id | 가동부 | 축 · 범위 [TARGET] | keyframe 슬롯 |
|---|---|---|---|
| `SM_Kit_Wheel_Valve` | `JNT_valve_wheel` | 로컬 Z 회전 0 → +270° | rotation.z |
| `SM_Kit_Lever_Long` / `_Short` | `JNT_lever_arm` | 로컬 X 회전 +25° → −25° | rotation.x |
| `SM_Kit_Crank` | `JNT_crank_axle` | 로컬 Z 연속 회전(loop 1회전 = 360°) | rotation.z |
| `SM_Kit_Panel_Hatch` | `JNT_hatch_leaf` | 로컬 X 힌지 0 → +85° | rotation.x |

## 3. (B) 손 표현 — **결정: 기본값 "손 없음", 필요 시 2D 오버레이. 3D 손은 별도 RFC**

**결정 [TARGET · 애니메이션 레인 소유, 디렉터 확인 요청 = RFC-A1]**

| 안 | 내용 | 판정 |
|---|---|---|
| **기본(채택)** | 확대뷰에 손을 넣지 않는다. 도구 가동부가 **자율 구동**하고 조작 주체는 UI·사운드·1인칭 문장으로 읽힌다 | 자산 0 추가, RFC-P4-001 과 무모순 |
| 옵션 A | **2D 손 오버레이** — 도구별 2컷(접근/접촉) 스프라이트, concept 레인 GTI 생성 | 자산 **+12** → `asset-budget.md` 47종 계약 개정 필요(모델러·디렉터) |
| 옵션 B | **3D 손 셸** — Generic 리그(Humanoid 아님), 손목1 + 손가락 5×4 + 보조1 = **≤ 22 본** [TARGET] | **RFC-P4-001 을 뒤집는 새 RFC 없이는 착수 금지** |

근거 [OBSERVED]: `modeling/asset-manifest.md` 47행 어디에도 손 자산 행이 없고, `modeling/asset-budget.md` 수량 계약(셸5/도구6/초상5/공용30/UI1)에도 손 항목이 없다. RFC-P4-001 은 "3D 인물 도입은 C4 이후 별도 RFC" 로 못박았다. 손은 인물의 일부이므로 **예산과 판정을 애니메이터가 혼자 열 수 없다.**

**따라서 `modeling/pipeline.md` §8 의 조건부 항목이 닫힌다**: "손가락 — 확대뷰에 손이 나오면 finger joints on, 아니면 off" → **off 확정**(옵션 B가 승인되기 전까지). 이 값은 Mixamo 규격 §5 에도 그대로 반영한다.

옵션 A 가 승인될 경우의 납품 규격 [TARGET, 미착수]: PNG 알파, 도구당 2컷, 시트 폭 2048, 앵커 = 도구 원점의 화면 투영점, 색은 `concept/style-guide.md` 팔레트 안, 저감모션에서는 **접촉 정지컷 1장만** 표시, 클립 아님(UI 이미지 스왑).

## 4. (C) 인물 초상 15컷 (5인 × 3표정) 납품 규격 — **리그 없음**

**리깅·본·블렌드셰이프 0.** 표정은 **컷 교체**다 [OBSERVED `modeling/asset-budget.md` "2D, 별도 리깅 없음" · `concept/style-guide.md` §7 "표정 3종은 같은 두상 위 변형이며 별도 실루엣을 만들지 않는다"].

| 자산 id | 인물 | 납품 파일 3종 (`PT_<Name>_<expr>`) |
|---|---|---|
| `PT_HanSeorin` | 한서린 | `PT_HanSeorin_calm` · `_guard` · `_resolve` |
| `PT_MunJaehwa` | 문재화 | `PT_MunJaehwa_calm` · `_guard` · `_resolve` |
| `PT_OEunjeong` | 오은정 | `PT_OEunjeong_calm` · `_guard` · `_resolve` |
| `PT_PyoSeongchan` | 표성찬 | `PT_PyoSeongchan_calm` · `_guard` · `_resolve` |
| `PT_HanDoyeon` | 한도연 | `PT_HanDoyeon_calm` · `_guard` · `_resolve` |

`expr` 토큰 `calm|guard|resolve` = 평상|방어|결심(`concept/style-guide.md` §7 이 KO 정본). 토큰은 in-fiction 고유명사가 아니므로 용어집 등록 대상이 아니다(`worldview/glossary.md` §6-1 파생 규칙과 무충돌).

**시트·슬라이싱 규격 [TARGET]**
- 1인 1시트, 3컷 가로 3분할, 셀 폭 **균등**(비균등 분할 금지 — 교체 시 위치가 튄다).
- **눈높이 라인과 어깨선이 3컷에서 같은 y** 여야 한다. 이것이 "리그 없이 표정만 바뀐다"를 성립시키는 유일한 조건이다.
- 스프라이트 pivot = **(0.5, 0.0)** 하단 중앙, 배경 알파 투명, 안전여백 4%.
- 시점은 `concept/style-guide.md` §5 예외 규칙(눈높이·3/4각·상반신·중립배경)을 따른다 — 2.5D 노드 카메라 규칙(1.55 m/−18°)을 초상에 적용하지 않는다.
- 텍스처 예산 2K 시트 1장/인 [OBSERVED `modeling/asset-manifest.md` §3].

**현재 생성 상태 [OBSERVED]** `concept/generation-manifest.md`: `char-{seorin,jaehwa,eunjeong,seongchan,doyeon}-portrait` **5컷(평상만) 생성 = OK**, 실측 해상도 1145×1374 또는 1024×1536. 즉 **15컷 중 5컷**만 존재하고 `guard`·`resolve` **10컷은 미생성**이다. 5컷 전부 `runtimeEligible:false` 프리비즈이며 승격 0건. 실측 해상도가 시트 규격(3분할 2K)과 다르므로, 시트화는 **재생성 또는 합성 단계가 따로 필요**하다(concept 소유).

**전환 시간**: 표정 교체는 애니메이션 클립이 아니라 UI 이미지 스왑이다. 제안 [TARGET] **크로스페이드 120 ms · 저감모션에서 0 ms 즉시 교체**. 다만 UI 전환 시간의 정본 소유자는 motion 레인(`motion/motion-contract.md` hint 펼침 140 ms / 확정 패널 180 ms, C4/C5 검증 대기)이므로 **RFC-A2** 로 올리고, motion 이 값을 주면 그 값으로 교체한다. 애니메이션 레인이 UI 시간 숫자를 소유하지 않는다.

## 5. (D) Mixamo 조건부 규격 — 발동 전, 이번 세션 실행 0회

**발동 조건**: RFC-P4-001("현 사전제작 범위에서 Mixamo 조건부 미사용")을 뒤집는 **새 디렉터 RFC**. 그 전까지 업로드·다운로드·리깅 **0건**이며 이 절은 잠금 규격이다.

| 항목 | 규격 | 출처 |
|---|---|---|
| 업로드 포맷 | FBX(binary), 단일 메시, 기존 스켈레톤·모디파이어 없음 | pipeline §8 |
| 포즈 | **T-pose** (팔 수평, 손바닥 아래, 다리 어깨너비) | pipeline §8 |
| 스케일 | **1 unit = 1 m**, 신장 1.6~1.9 m, 스케일 apply 필수 | pipeline §8 |
| 좌표 | Y-up, `-Z` forward (FBX 설정 그대로) | pipeline §2·§8 |
| 정점 웨이트 | 정점당 **≤ 4 본** | pipeline §8 |
| **본 총수** | **≤ 65 본** (Mixamo 표준 스켈레톤 상한). 초과 스켈레톤은 반려 | **본 문서 신설 [TARGET]** |
| 손가락 조인트 | **off** (§3 결정). 옵션 B 승인 시에만 on | 본 문서 §3 이 pipeline §8 조건부를 닫음 |
| 루트 모션 | **off** — in-place 클립만 받는다 | **본 문서 신설**. 노드 카메라 설계에 루트 이동이 들어오면 씬 배치가 깨진다 |
| Unity | Rig → **Humanoid**, Avatar Definition → Create From This Model | pipeline §8 |
| 클립 명명 | 다운로드 원본 그대로 두지 않고 `AN_<charId>_<clipId>.anim` 로 재명명 후 `anim-list.md` 에 등재 | **본 문서 신설** |
| 다운로드·라이선스 | **사용자 수동**. 공개 API 없음 → 에이전트 자동화 불가. Adobe 약관 확인은 사용자 몫, 산출물도 `runtimeEligible:false` | pipeline §8 · 계약 Asset pipeline |

**pipeline §8 대비 델타** (pipeline §8 마지막 문장의 "편차를 여기 기록한다" 요구에 대한 애니메이션 레인 측 회신):

| # | 델타 | 성격 |
|---|---|---|
| D1 | 본 총수 상한 **65** 신설 | 추가 (§8에는 정점당 본 수만 있고 총수 없음) |
| D2 | 손가락 조인트 **off 확정** | §8의 조건부를 §3 결정으로 닫음 |
| D3 | 루트 모션 off · in-place 전용 신설 | 추가 |
| D4 | 클립 명명 규칙 신설 | 추가 |
| D5 | 그 외 6항(포맷·포즈·스케일·좌표·웨이트·Unity) | **변경 없음 — §8 과 동일** |

## 6. Unity Animator 상태기계 명명

**어디에 붙이는가**: **3D 프롭에만**. 2D 초상·UI·수면 표현에는 Animator 를 붙이지 않는다(초상 = 이미지 스왑, UI = motion 레인 표면, 수면 = vfx/셰이더).

### 6.1 명명 규칙 [TARGET]

```
AC_<propId>                 Animator Controller       예) AC_Tool_reader, AC_Kit_Wheel_Valve
AN_<propId>_<clipId>.anim   클립 파일                 clipId 는 anim-list.md 의 10종 정본만
ROOT_<propId> / JNT_<propId>_<part>   리그 노드(§2)
Layer: Base                 단일 레이어 기본. 가중치 층이 필요하면 Additive_<name>
State: PascalCase 동사       Idle · Insert · Held · Retract · Snap · SnapBack · Settle
                             StampDown · StampUp · Spin · Raise · Lower · Open · Close · Rise
```

- `StateMachineBehaviour` 로 시뮬레이션 상태를 쓰지 않는다 — 렌더/연출은 스냅샷을 **읽기만** 한다(CLAUDE.md §9 불변식 · `systems/unity-implementation.md` §2 `Tide.Presentation`, C4/C5 검증 대기).
- Animator Culling Mode = `Cull Update Transforms` [TARGET], Apply Root Motion = **off** (전 컨트롤러).
- 클립 길이·키 이벤트 ms 는 **`animation/anim-list.md` 가 유일 정본**이다. 컨트롤러 전이 시간(Transition Duration)으로 클립 길이를 재정의하지 않는다.

### 6.2 파라미터 — 출처가 없는 파라미터는 만들지 않는다

| 파라미터 | 타입 | 출처(요구) | 상태 |
|---|---|---|---|
| `bSavePending` | Bool | `systems/interaction-rules.md` §5-3 `SavePending` (C4/C5 검증 대기) | 문서 근거 있음 · **코드 필드명 미정 → RFC-A3** |
| `tCommitReceipt` | Trigger | §5-4 "저장 영수증 도착 후에만 결과 패널·도장 애니·확정 사운드" | 문서 근거 있음 · **코드 필드명 미정 → RFC-A3** |
| `tCommitFailed` | Trigger | §5-5 실패 경로(성공 연출 금지, 재시도/뒤로) | 〃 |
| `tPreview` | Trigger | UI 계약 `confirm-preview` 진입 | 〃 |
| `tCancel` | Trigger | UI 계약 `back_behavior`(취소는 상태를 바꾸지 않는다) | 〃 |
| `fValue01` | Float | 도구별 정규화 표시값(alignment 잔차, corrosion 총비용/상한 9) | 〃 |
| `bReduceMotion` | Bool | 접근성 설정(저감모션) | 〃 |
| `bOffscreen` | Bool | `motion/motion-contract.md` "offscreen/일시정지 애니는 중지"(C4/C5 검증 대기) | 〃 |

**규칙**: 스냅샷에 대응 필드가 없는 파라미터로 전이를 만들지 않는다. 필드가 없으면 systems 레인 과제로 올리고 클립에 `[BLOCKED:param]` 을 표시한다 — 가짜 전이를 만들지 않는다.

### 6.3 인터럽트 우선순위 (위가 이긴다)

| # | 조건 | 동작 |
|---:|---|---|
| 1 | `bReduceMotion == true` | Any State → 해당 클립의 **최종 포즈**로 전이 시간 0. 보간·감속 없음 |
| 2 | `bOffscreen == true` / 일시정지 | `speed = 0` 후 최종 포즈 스냅. 타이머·이벤트 발행 중지 |
| 3 | `tCommitFailed` | 성공 연출 상태로 **진입 금지**, 확정 이전 포즈 유지, 사운드 0회 |
| 4 | `tCancel` (취소 창 안) | 케이블/판 = `SnapBack` 경유 원위치 · 밸브/레버 = 상태 snap(즉시 최종) |
| 5 | 그 외 | 정상 전이 |

### 6.4 금지 전이 (안전 규칙)

- `bSavePending` 을 조건으로 **도장·봉인·확정 연출 상태에 진입하는 전이를 만들지 않는다.** `stamp_down` 진입 조건은 오직 `tCommitReceipt` 다. 근거: `animation/animation-contract.md` "권위 있는 상태 변화로 읽히는 연출은 영수증 이후에만"(C4/C5 검증 대기) · `systems/interaction-rules.md` §5-4.
- 확정 성공 연출을 먼저 재생하고 실패 시 되돌리는 전이를 만들지 않는다.
- 클립이 실행되지 않아도 게임 진행은 계속되어야 한다 — Animator 를 진행 게이트로 쓰는 전이 금지(계약 테스트 절).

## 7. 모델링 인계 체크리스트 (ack 요청)

`modeling/specs/hub-watchroom.md` L82 체크박스("`animation/rig-requirements.md` 대조 — 파일 없음")의 대조 대상은 이제 이 문서다. 모델러 ack 요청 항목:

1. §2.1 공통 규격 6항(피벗·단위·축·개별파일 리그 포함·가동부 상한 8·명명) — ack / counter
2. §2.2 프롭 6종의 **가동부 이름·축·범위** — 특히 로컬축 방향(부호 반전 시 클립이 거울로 재생)
3. §2.3 zone 프롭 4종(`Wheel_Valve`·`Lever_*`·`Crank`·`Panel_Hatch`)의 가동부
4. §3 손 = "없음" 기본값 채택 → **pipeline §8 finger joints = off** 반영
5. §5 델타 D1~D4 를 pipeline §8 에 편차로 기록
6. §4 초상 3컷의 **눈높이·어깨선 공통 y** 요구를 concept 레인에 전달(모델러는 2D 초상을 만들지 않으므로 경유만)

**rig-first 원칙**: 위 1~3 이 ack 될 때까지 `animation/clip-specs/*.md` 는 **0건**을 유지한다(현재 `clip-specs/` 파일 수 = 0 [OBSERVED]). 리그가 확정되지 않은 클립 스펙은 재작업을 예약하는 문서다.

## 8. 미해결 · RFC

| id | 대상 레인 | 질문 | 제안 |
|---|---|---|---|
| **RFC-A1** | director, modeling, concept | 확대뷰에 손을 넣는가 | **넣지 않는다(기본)**. 옵션 A(2D 12컷)는 `asset-budget.md` 47종 계약 개정을 동반, 옵션 B(3D 손)는 RFC-P4-001 뒤집기 |
| **RFC-A2** | motion | 초상 표정 교체 시간의 소유자와 값 | 애니메이션 제안 120 ms / 저감 0 ms. motion 이 정본을 주면 교체 |
| **RFC-A3** | systems | §6.2 파라미터 8종을 렌더 스냅샷에 어떤 필드명으로 노출하는가 | `bSavePending`·`tCommitReceipt` 두 개만이라도 이름을 확정해 주면 Animator 골격이 선다 |
| **RFC-A4** | modeling, vfx | `drawer_open`·`water_level` 의 **대상 자산이 매니페스트 47종에 없다** (재측정: `grep -in "drawer\|서랍\|수면\|water" asset-manifest.md` → **0 hit**) | `drawer_open` = UI 기록 서랍(motion 180 ms)인지 3D 서랍 프롭인지 판정 · `water_level` 은 vfx `water_rise`(수면 1장) 소유로 이관 후보 |
| **OPEN-A5** | (애니메이션 자기 과제) | `shutter_raise`·`pump_spin` 의 시간 대역이 `animation-contract.md` 5행 표에 없다 | 다음 계약 개정에서 "대형 가동부" 대역 신설 제안. 그 전까지 `anim-list.md` 값은 대역 밖 [TARGET] 로 표시 |

## 9. 이 문서가 증명하지 않는 것 [OBSERVED]

리깅 0건 · Unity 프로젝트 임포트 0건 · Animator Controller 0개 · `.anim` 0개 · Mixamo 접속 0회 · 손 자산 0개 · 초상 15컷 중 10컷 미생성 · 재생 프레임 캡처 0장 · 사람 플레이 n=0. 이 문서는 **납품 규격**이며 리그가 존재한다는 주장이 아니다. G4(연출/몰입)·G5(에셋 예산) 어느 것도 이 문서로 PASS 하지 않는다.

## 10. 변경 로그 (RFC-Q2 — 같은 사이클 제자리 갱신 = 개정)

| 날짜 | 회차 | 내용 |
|---|---|---|
| 2026-09-10 | R4 | **신설.** 모델러 부재 지적(pipeline §8 · specs/hub-watchroom.md L82 · 계약 Asset pipeline 행) 해소. §3 손 결정으로 pipeline §8 finger joints 조건부를 닫고, §5 에 델타 D1~D4 를 기록. `supersedes: null` 유지 |
