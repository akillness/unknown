---
updated: 2026-09-13
cycle: 20260909-preproduction-c5
status: draft
supersedes: null
owner: game-animator
---

# Anim List — 기존 프롭 10종 · M22 인물/손 6동작

## 0. 지위 · 정본 관계 · 실측 상태

- 기존 프롭 10종은 `animation/animation-contract.md`(C4/C5 검증 대기)의 목록을 행 단위로 펴고 키 이벤트를 붙인 것이다. **M22 인물/손 증분은 최신 사용자 지시와 RFC-CX-018에 의해 별도로 개방**되며 §11이 실제 Blender 저작·내보내기 목록을 기록한다. 기존 프롭의 시간·취소 규칙은 바꾸지 않는다.
- 리그 규격은 `animation/rig-requirements.md`. **rig-first**: §7 모델러 ack 전에는 `clip-specs/` 를 열지 않는다(현재 파일 수 0 [OBSERVED]).
- **M22 이전 기준선 [OBSERVED 2026-09-10]**: `.anim` 파일 0개 · Animator Controller 0개 · 재생 캡처 0장 · 사람 플레이 n=0 · 프레임 타임 측정 0건. 이를 M22 이후 실제 FBX 내장 클립의 부재로 인용하지 않는다. 아래 기존 프롭의 length·키 이벤트·프레임은 [TARGET]이고, M22 저작 결과는 §11 및 생성 manifest에서 구분한다.
- frontmatter: 신설이므로 `supersedes: null`, `cycle` 은 레인 live 문서와 동일한 `20260909-preproduction-c5`, `status: draft`(C3-F33 — QA 검증 후 소유 레인이 current 로 올린다).

## 1. 시간 기준 (timebase) — ms 가 정본, 프레임은 파생

1. **키 이벤트의 정본 단위는 ms** 다. vfx·motion·QA 는 이 표의 ms 를 읽고, 프레임을 **재도출하지 않는다**(CLAUDE.md §4 공유 진실 파일 규칙 · 본 레인 운영 원칙 1).
2. 저작 프레임레이트 = **60 fps**. 파생 규칙 `frame = ms × 0.06`.
3. **모든 키 이벤트 ms 는 50 ms 격자에 놓는다.** 50 ms = 정확히 3프레임이므로 반올림 오차가 **0** 이다. 격자 밖 값은 AnimationEvent 를 걸지 않고 보간 전용으로만 쓴다(현재 예외 1건: `drawer_open` 180 ms — motion 레인 소유 값).
4. 재도출 명령 [OBSERVED 2026-09-10, 아래 §2·§3 값 입력]:
   ```
   node -e 'const v=[200,250,550,400,1000,400,300,1200,180,700,150,350];
     for(const m of v){const f=m*0.06;console.log(m+"ms -> "+f+"f "+(Number.isInteger(f)?"grid":"OFF-GRID"))}'
   ```
   결과 [OBSERVED 2026-09-10 실행]: `180ms -> 10.799999999999999f OFF-GRID` **1건**(부동소수 표기, 실값 10.8f) 외 **전부 정수 프레임**.
5. 클립 길이·이벤트 위치를 Animator 의 Transition Duration 이나 `speed` 로 재정의하지 않는다.
6. 확대뷰 진입 모션(작업대 확대 **220 ms**, `motion/motion-contract.md`, C4/C5 검증 대기)은 **클립 길이에 포함되지 않는다**. 체감 지연 = 220 ms + 클립 length 이며, 이 합계는 motion 레인이 소유한다.

## 2. 클립 목록 10종

`type`: `one` = 원샷 · `loop` = 루프. `root` = root motion(전부 false — `rig-requirements.md` §6.1).

| # | clip id | 대상 프롭 (자산 상태) | 계약 대역 | length_ms [TARGET] | 프레임(60fps) | type | loop | root | Animator state | status |
|---:|---|---|---|---:|---:|---|---|---|---|---|
| 1 | `connector_snap` | `SM_Tool_routing` (greybox) | 케이블/판 끼우기 180~280 | **200** | 12 | one | false | false | `Snap` / `SnapBack` | spec-pending-ack |
| 2 | `plate_insert` | `SM_Tool_reader` (greybox) | 케이블/판 끼우기 180~280 | **250** | 15 | one | false | false | `Insert` / `Retract` | spec-pending-ack |
| 3 | `valve_turn` | `SM_Kit_Wheel_Valve` (pending, 클립 결합 **매니페스트 기재됨**) | 밸브 회전 350~650 | **550** | 33 | one | false | false | `Turn` | spec-ready |
| 4 | `lever_throw` | `SM_Kit_Lever_Long` (pending, 클립 결합 **매니페스트 기재됨**) | 밸브 회전 350~650 | **400** | 24 | one | false | false | `Throw` | spec-ready |
| 5 | `pump_spin` | `SM_Kit_Crank` (pending, "연계" 기재됨) | **대역 없음**(OPEN-A5) | **1000**/회전 | 60 | loop | **true** | false | `Spin` | spec-ready |
| 6 | `gauge_settle` | `SM_Tool_alignment` + `SM_Tool_corrosion` (둘 다 greybox, **1클립 2인스턴스**) | 계기 정착 300~500 | **400** | 24 | one | false | false | `Settle` | spec-pending-ack |
| 7 | `stamp_down` | `SM_Tool_seal` (greybox) | 도장 확인 **300 고정** | **300** | 18 | one | false | false | `StampDown` / `StampUp` | spec-pending-ack |
| 8 | `shutter_raise` | `SM_Kit_Panel_Hatch` (pending, 클립 결합 **미기재 — 본 문서 제안**) | **대역 없음**(OPEN-A5) | **700** | 42 | one | false | false | `Raise` / `Lower` | spec-pending-ack |
| 9 | `drawer_open` | **대상 자산 없음** (§4) | 대역 없음 · motion "기록 서랍 180 ms" 인용 | **180** `[CARRIED]` | 10.8 (격자 밖) | one | false | false | `Open` / `Close` | **[BLOCKED:rig]** |
| 10 | `water_level` | **대상 자산 없음** (§4) — vfx `water_rise`(수면 1장) 후보 | 수면 변화 900~1400 | **1200** | 72 | one | false | false | `Rise` | **[BLOCKED:rig]** |

집계: `spec-ready` 3 · `spec-pending-ack` 5 · `[BLOCKED:rig]` 2 = **10**. **저작된 클립 0개.**

- 도구 6종 중 클립 대상은 4종(`routing`·`reader`·`alignment`·`seal`) + `corrosion` 게이지 바늘. **`SM_Tool_circuit` 은 가동부 0 · 클립 0** 이다(`rig-requirements.md` §2.2).
- `spec-pending-ack` = 리그 규격(`rig-requirements.md` §7 항목 1~3)에 대한 모델러 ack 대기. ack 전 클립 저작 착수 금지.

## 3. 키 이벤트 표 — vfx·motion·QA 가 읽는 공유 진실

- **hit** = 물리 접촉·맞물림 순간(사운드 트리거 기준점). **fx** = vfx 생성 시각. **cancel window** = 취소해도 비용 0인 구간(계약 4열 파생).
- fx 효과 id 는 `vfx/vfx-budget.md` 6종(C4/C5 검증 대기): `brine_flow` · `pressure_pulse` · `salt_reveal` · `water_rise` · `seal_confirm` · `rain_window`.
- 사운드는 **소유 레인 미배정**이다. 이벤트 이름 `SFX_<clip>` 만 예약하고 재생 규격은 정하지 않는다(없는 소유자를 대신하지 않는다).

| clip id | hit (ms / f) | fx (ms / f) | fx 효과 id | cancel window (ms) | 취소 후 처리 |
|---|---|---|---|---|---|
| `connector_snap` | **150** / 9 | **150** / 9 | `pressure_pulse` | **0–150** | 150 이후는 `SnapBack` 경유 원위치(≤150 ms) |
| `plate_insert` | **200** / 12 | **200** / 12 | `salt_reveal` | **0–200** | 200 이후는 `Retract` 경유 원위치(≤150 ms) |
| `valve_turn` | **300** / 18 | **350** / 21 | `brine_flow` 시작 | **0–300** | 300 이후 back = **상태 snap**(즉시 최종, 감속 없음) |
| `lever_throw` | **250** / 15 | **250** / 15 | `pressure_pulse` | **0–250** | 250 이후 back = 상태 snap |
| `pump_spin` | — (연속) | **0** / 0 | `brine_flow` 지속 | **전 구간** | 정지 요청 시 감속 200 ms 후 정지 |
| `gauge_settle` | — | **없음** | — (효과 0 — vfx 예산 절약) | — | 계약: "감속 없이 최종값 표시 가능" |
| `stamp_down` | **150** / 9 | **150** / 9 | `seal_confirm` | **없음** | **취소 불가** — 저장 영수증 이후에만 재생되므로 이미 확정 상태 |
| `shutter_raise` | **550** / 33 | **550** / 33 | `pressure_pulse` | **0–550** | 550 이후 back = 상태 snap |
| `drawer_open` | **150** / 9 | 없음 | — | **0–150** | 원위치. 격자 밖 length(180)로 인해 **AnimationEvent 금지 · 보간 전용** |
| `water_level` | — | **0** / 0 | `water_rise` (동기 시작) | **없음** | 결과 표시 전용(routing solver 소유). 저감모션 시 즉시 최종 |

**읽는 쪽 규칙**: vfx 는 `fx` 열을, motion 은 `cancel window` 와 length 를, QA 는 세 열 전부를 인용한다. **어느 레인도 이 값을 자기 문서에서 다시 계산하지 않는다.** 값이 틀렸다고 판단하면 수정이 아니라 RFC 로 온다.

## 4. 대상 자산이 없는 클립 2건 — `[BLOCKED:rig]`

재측정 [OBSERVED 2026-09-10]:
```
grep -in "drawer\|서랍" _workspace/current/modeling/asset-manifest.md   # → 0 hit
grep -in "수면\|water"   _workspace/current/modeling/asset-manifest.md   # → 0 hit
```
`asset-manifest.md` 47행(셸5·도구6·초상5·공용30·UI1) 어디에도 **서랍 프롭과 수면 판이 없다.**

| clip | 해석 후보 | 필요한 판정 |
|---|---|---|
| `drawer_open` | (a) **UI 기록 서랍** — `motion/motion-contract.md` "기록 서랍 180 ms" 가 이미 소유 → 3D 클립이 아니라 UI 모션. (b) 3D 서랍 프롭 신설 → 공용 소품 31번째 = 예산 계약 개정 | **RFC-A4** — (a)면 이 행은 motion 으로 이관하고 anim-list 는 9종이 된다 |
| `water_level` | `vfx/vfx-budget.md` 의 `water_rise`("수면 1장")가 실질 소유. 메시 애니가 아니라 셰이더/머티리얼 파라미터 | **RFC-A4** — vfx 소유로 확정되면 이 행은 참조만 남긴다 |

판정 전까지 두 행을 **지우지 않는다**(계약의 10종 목록과 대조가 끊긴다). 클립 저작은 착수하지 않는다.

## 5. 저장 영수증 게이트 — `stamp_down` 전용 안전 규칙

`animation/animation-contract.md`(C4/C5 검증 대기) · `systems/interaction-rules.md` §5 를 클립 수준으로 옮긴 것:

1. `stamp_down` 의 유일한 진입 조건은 **`tCommitReceipt`** 다. `bSavePending` 로는 진입할 수 없다(`rig-requirements.md` §6.4 금지 전이).
2. 저장 실패(`tCommitFailed`) 시 `stamp_down` 재생 **0회**, `SFX_stamp_down` **0회**, 화면은 확정 이전 포즈 유지.
3. 지연 완료 콜백이 뒤늦게 도착해도 재생 **0회**(계약 테스트 절: "지연 완료 콜백 도착 시 도장 재생 0회").
4. 후보(preview) 단계에서 재생 가능한 클립: `connector_snap` · `plate_insert` · `gauge_settle` · `drawer_open`. **권위 있는 상태 변화로 읽히는 클립**(`stamp_down` · `water_level` · 봉인 연출)은 영수증 이후에만.

## 6. 저감모션(reduce motion) 대체 표

| clip | 저감모션 동작 |
|---|---|
| `connector_snap` · `plate_insert` · `drawer_open` | 최종 포즈 즉시(전이 0 ms), fx 는 1프레임 표시 후 정적 |
| `valve_turn` · `lever_throw` · `shutter_raise` | 최종 각도 즉시 스냅 |
| `pump_spin` | 회전 정지, 정지 포즈 + "가동 중" 문자 라벨 |
| `gauge_settle` | 최종값 즉시 표시(계약이 이미 허용) |
| `stamp_down` | 영수증 후 최종 도장 이미지 즉시 표시(하강 보간 생략), 결과 문장은 동일 |
| `water_level` | 최종 수위 즉시(계약: "reduce motion 즉시최종") |

모든 경우에 **문자 라벨이 함께 있어야 한다** — 색·동작 단독으로 결정적 상태를 전달하지 않는다(`systems/interaction-rules.md` §0-7).

## 7. QA 인수 항목 (타이밍 검증용)

계약 테스트 절 + 본 표에서 파생. 전부 **미실행** [OBSERVED].

| # | 단언 | 근거 열 |
|---:|---|---|
| A1 | 각 클립의 fx 이벤트 시각이 §3 ms 값과 ±1프레임 이내 | fx |
| A2 | 저장 지연 주입 중 `stamp_down` 재생 0회 | §5-1 |
| A3 | 저장 실패 주입 후 확정 사운드 0회 | §5-2 |
| A4 | 지연 완료 콜백 도착 시 도장 재생 0회 | §5-3 |
| A5 | cancel window 안에서 취소 시 상태 변화 0건 | cancel |
| A6 | 저감모션에서 모든 클립이 최종 포즈에 도달하고 결과 문장이 동일 | §6 |
| A7 | 클립 미실행(에셋 누락 주입) 상태에서도 진행이 막히지 않음 | 계약 |

## 8. 이 문서가 증명하지 않는 것 [OBSERVED]

2026-09-10의 `.anim`/Animator/Unity 임포트/캡처 0건은 과거 기준선이다. M22는 FBX 내장 클립과 수동 Generic Playables를 사용하므로 독립 `.anim` 파일이나 Animator Controller의 개수로 모션 유무를 판단하지 않는다. 사람 플레이 n=0이며 **설계 ms와 저작 키프레임은 실사용 타이밍/조작감 관측값이 아니다**. 이 목록만으로 G4·G5를 PASS하지 않는다.

## 9. 미해결

- **OPEN-A5**(본 레인): `pump_spin` · `shutter_raise` 는 계약 5행 대역표에 대응 행이 없다. 다음 계약 개정에서 "대형 가동부" 대역 신설을 제안한다. 그 전까지 두 값은 **대역 밖 [TARGET]**.
- **RFC-A4**(modeling · vfx): §4 두 클립의 대상 자산 부재.
- **RFC-A3**(systems): Animator 파라미터의 스냅샷 필드명(`rig-requirements.md` §6.2).
- **RFC-A2**(motion): `drawer_open` 180 ms 와 표정 전환 시간의 소유권.

## 10. 변경 로그 (RFC-Q2 — 같은 사이클 제자리 갱신 = 개정)

| 날짜 | 회차 | 내용 |
|---|---|---|
| 2026-09-10 | R4 | **신설.** 계약 10클립을 행으로 펴고 60 fps·50 ms 격자 timebase 와 키 이벤트(hit/fx/cancel) 표를 붙였다. 대상 자산 부재 2건을 `[BLOCKED:rig]` 로 표시. `supersedes: null` 유지 |

## 11. M22 — Blender MCP 직접 저작 클립 (RFC-CX-018)

[OBSERVED 2026-09-13] 실제 Blender MCP에서 모델·리그·키프레임·FBX/GLB 및 프리뷰를 생성했다. `assets/generated/3d/seorin-m22/r01/manifest.json`과 생성기 `scripts/blender/build-seorin-m22.py`가 정확한 소스/해시/본/프레임을 소유한다. 프리뷰와 Unity 네이티브 비교로 목 길이·봉제선·전신 중립 손 자세, 화면 안에 끝나던 소매, 좌/우 엄지 방향을 원본에서 교정했다. 이전 후보는 history에 보존했다.

| 동작 / 정확한 takeName | 적용 파일 | 내보낸 구간 @60fps | 접촉/재생 규칙 |
|---|---|---|---|
| `Seorin_Idle` | Character | 0–120 (2.0s) | 시작 화면의 절제된 호흡/자세. 루트 이동 없음 |
| `Hands_Rest` | LeftHand / RightHand | 0–60 (1.0s) | 런타임은 정지 포즈를 샘플. 대기 자체가 성공 조작은 아님 |
| `Hands_Insert` | LeftHand / RightHand | 0–15 (250ms) | 기존 plate_insert 접촉 200ms; 성공 장전에서만 |
| `Hands_Align` | LeftHand / RightHand | 0–12 (200ms) | 짧은 정렬/복귀. 퍼즐 이동·입력을 지연하는 시간이 아님 |
| `Hands_Grip` | LeftHand / RightHand | 0–24 (400ms) | 닫힌 grip 포즈는 0.4s. 실제 크랭크의 420/160/520ms·150도는 기존 reader driver가 소유 |
| `Hands_Seal` | LeftHand / RightHand | 0–18 (300ms) | 접촉 150ms. 현재 context의 내구 저장 성공만 확정 반응 |

- 6개 의미 동작, 실제 FBX take는 전신 1 + 좌/우 각각 5 = **11개**다. 양손의 코드 경로가 비슷하다고 리그나 파일을 한 손으로 대체하지 않는다.
- [OBSERVED · Blender/Unity 임포트] 전신 15,124 tri/50본, 양손 각각 2,772 tri/18본(합 5,544 tri); 정점당 최대 영향 본 2개. `Root`/`Forearm` 이동은 0이며 손 클립의 손목·15개 손가락 회전 채널을 내보냈다. 각 손에 Forearm 100% 웨이트의 소매를 연장해 팔 끝이 네이티브 화면 안에서 잘리지 않게 했다.
- [OBSERVED · 저작] 각 손의 `Contact` 본 원점이 grip 앵커다. Blender 세계좌표를 Unity 본 로컬좌표로 오인하지 않도록 런타임 gripPoint는 `(0,0,0)`이다. 실제 접촉 여부는 Unity 변환·포즈를 포함해 별도 검증한다.
- [DECISION · 런타임] 판독기 왼손은 카메라 기준 대기 위치에서 접촉점으로 접근해 Insert/Seal의 접촉 구간을 유지한 뒤 150ms에 복귀한다. 오른손은 조작 중 닫힌 grip을 유지하며 실제 손잡이 위치를 따른다. `rd-hex-plate`는 불러온 기록물이 아니라 고정 판독기 하드웨어이므로 손과 함께 이동시키지 않는다. 모션 축소/이탈/실패에서는 즉시 취소하며 150ms 복귀가 입력·저장의 지연은 아니다.
- [OBSERVED / BOUNDARY] 진단 플래그 없는 Unity 개발 빌드에서 삽입·복귀, 손잡이 추적과 모션 축소를 확인했다. 프레임 시작 이전의 작업 시간이 새 동작에 섞이지 않도록 판독/손 동작은 수락 시점의 unscaled monotonic clock을 사용한다. 접촉·취소·저장 실패 회귀와 실제 화면은 `systems/tech-verification/m22/verification.json` 및 `docs/media/gameplay-m22/`에 기록했다. Blender 프리뷰를 게임 화면으로 부르거나 사람 플레이 검증을 주장하지 않는다.
- [DECISION · 중단] 수락 시점 이전 작업을 제외하는 `realtimeSinceStartupAsDouble`은 유지한다. 앱 pause/포커스 상실 시 손·크랭크를 즉시 대기 포즈로 되돌리고 중단 중 새 요청을 거절하므로, 복귀 프레임이 중단 시간을 소비하거나 이전 명령을 재생하지 않는다. Editor의 프로그램식 일시정지 API 동등성은 별도 주장하지 않는다.
