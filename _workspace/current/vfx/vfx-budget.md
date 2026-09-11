---
updated: 2026-09-10
cycle: 20260909-preproduction-c4
status: draft
supersedes: null
owner: game-vfx-artist
sync-repair-by: game-vfx-artist (C4-F7 대응 R2 · interaction-rules §0-10/§5 대조, 제자리 개정 RFC-Q2)
---

# VFX Budget and Readability

전부 `[TARGET]` 설계다. 실효과·실측 **n=0**. 이 문서는 이펙트가 **언제 발행되는가**(발행 게이트)와 **얼마를 쓰는가**(예산)의 계약이며, 연출 품질의 증거가 아니다.

## 0. 정본 참조 `R2`

이 문서는 다음을 인용만 하고 재정의하지 않는다.

| 사실 | 정본 |
|---|---|
| 확정 = 저장 성공 이후 (후보/권위 구분) | `systems/interaction-rules.md` §0-10 |
| 확정 → `SavePending` → 성공(영수증) / 실패 절차 | `systems/interaction-rules.md` §5-1~5-6 |
| 「권위 있는 연출은 영수증 이후」·저장 실패 시 미재생 | `animation/animation-contract.md` §확정 트랜잭션 |
| 클립 시간·저감모션 대체 | `animation/anim-list.md` |
| 결과 패널 상태명 | `systems/game-ui-contract.json` `screens[14]` (`result-checkpoint`) |

[OBSERVED, 2026-09-10] `node -e "const c=require('./_workspace/current/systems/game-ui-contract.json');console.log(c.screens[14].states.join(' | '))"`
→ `저장 진행 중 영수증 대기 | 저장 실패 재시도 또는 뒤로 | 구역 상태 변경 성공 | 보호 선택 반영 | 필수 단서 보존 표시`

**상태명은 이 다섯 줄과 `SavePending` 만 쓴다.** 「확정됨」·「완료」 같은 자체 상태명을 이 레인에서 새로 만들지 않는다. `[C4-F7]`

## 1. 효과 6종 · 발행 게이트 `R2` `[C4-F7]`

`권위`= 그 이펙트가 재생되면 플레이어가 "상태가 바뀌었다"고 읽는가. 권위 있는 이펙트는 **저장 영수증 도착 이후에만** 발행하고, `SavePending` 중과 **저장 실패 시에는 발행하지 않는다**(§5-4·§5-5).

| id | 표현 | 트리거(시스템 훅) | 권위 | 발행 시점 | 저장 실패 시 |
|---|---|---|---|---|---|
| `seal_confirm` | 짧은 잉크(봉인·도장) | 확정 명령 저장 성공 | **예** | **영수증 도착 후**, `stamp_down`(300ms)과 같은 프레임에서 시작 | **미발행**. 확정 사운드도 없음 |
| `water_rise` | 수면 1장(셰이더 파라미터) | 구역 수위 확정 | **예** | **영수증 도착 후**. 지속 900~1400ms(기본 1200) — `anim-list.md` `water_level` 행과 동일 값 | **미발행**. 수면은 확정 이전 값 유지 |
| `brine_flow` | 관 내부 방향 | 배관 경로 상태 | 조건부 | **후보 경로는 점선·저채도로 발행 가능**(프리뷰). **확정 경로 표현(실선·정채도 전환)은 영수증 이후** | 후보 표현으로 되돌리고 확정 표현 미발행 |
| `pressure_pulse` | 점선 + 화살표 | 압력 시뮬 스냅샷 읽기 | 아니오 | 상시(읽기 전용) | 영향 없음 |
| `salt_reveal` | 사본 윤곽 | 단서 판독 결과 표시 | 아니오 | 판독 즉시 (§2.2 「판독은 확정이 아니다」) | 영향 없음 |
| `rain_window` | 장식 | 장 진입 | 아니오 | 상시, 설정에서 off | 영향 없음 |

- **되돌리는 연출을 만들지 않는다.** 성공 연출을 먼저 재생하고 실패 시 역재생하는 경로는 이 문서에서 금지한다(`animation-contract.md` 동일 문구).
- **지연 콜백 방어**: 영수증이 늦게 도착했는데 그 사이 플레이어가 뒤로/재시도로 떠났다면 `seal_confirm`·`water_rise` 재생 횟수는 **0**이다.
- `water_rise` 의 소유(vfx 셰이더 vs `water_level` 클립)는 **RFC-A4 미해결** — `anim-list.md` L87. 이 문서는 시간 값만 맞추고 소유를 확정하지 않는다 `[CARRIED: C4-F17]`.

## 2. `SavePending` 구간의 이펙트 규격 `R2` `[C4-F7]`

1. `SavePending` 은 **중복 확정만 막고** 나머지 UI 는 응답한다(§5-3). 따라서 이 구간에서 **전역 화면 잠금·암전·풀스크린 이펙트를 넣지 않는다**.
2. 이 구간에 허용되는 vfx 는 **없다**. 진행 표시는 모션/UI 레인 소유(`motion-contract.md`)이며 vfx 예산 0, emitter 0, 드로콜 0 이다.
3. 저감(줄어든 시각자극) 모드에서 `water_rise` 는 즉시 최종 수면으로 스냅한다(`anim-list.md` "reduce motion 즉시최종"). 게이팅은 그대로 — **즉시여도 영수증 이후**다.
4. 확정과 미확정은 색·움직임 단독이 아니라 **실선/점선·체크/시계 + 문자 라벨**로 구분한다(§0-7).

## 3. 예산 `[TARGET]`

| 항목 | 값 | 비고 |
|---|---|---|
| 동시 emitter | ≤ 8 | 장을 나갈 때 owner scope 로 전량 반환(풀링) |
| transparent 입자 | ≤ 500 | |
| `SavePending` 구간 추가 emitter | **0** | §2-2 |
| bloom / 화면 플래시 / 기본 카메라 흔들림 | 0 / 0 / 0 | |
| 도구 중요선 위 효과 | 0 | 가독성 |
| 물 반사 | 실시간 RT 금지, 베이크·단색 반사 | |
| 측정 GPU 시간 | **null** — 실효과 생성 전 | n=0 |

## 4. 검증

- 전후 스크린샷 + 동일 입력 녹화 + Profiler/GPU 2초 구간으로 가독성·시간 측정.
- 이펙트를 전부 꺼도 핵심 단서·정답·수위·압력이 모두 이해돼야 한다. 저감 모드에서도 같은 결과.
- `R2` 추가 케이스: **저장 지연 주입 중 `seal_confirm`·`water_rise` 재생 0회** · **저장 실패 주입 후 `seal_confirm` 0회, `brine_flow` 확정 표현 0회** · **지연 완료 콜백 도착 시(패널 이탈 후) 재생 0회**.

## 5. 재현 명령 (C4-F7 판정용)

```
grep -c "영수증\|SavePending\|저장 실패" _workspace/current/vfx/vfx-budget.md
```
[OBSERVED, 2026-09-10] 개정 전 결과 `0` → 개정 후 재실행 값이 이 문서의 근거다.

```
grep -n "권위 있는\|영수증 이후" _workspace/current/animation/animation-contract.md _workspace/current/vfx/vfx-budget.md
sed -n '301,308p' _workspace/current/systems/interaction-rules.md
```
