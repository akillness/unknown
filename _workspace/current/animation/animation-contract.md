---
updated: 2026-09-10
cycle: 20260909-preproduction-c5
status: current
supersedes: _workspace/archive/20260909-preproduction-c4/animation/animation-contract.md
owner: game-animator
sync-repair-by: game-systems-designer (C4 F2 대응 R2, 저장 트랜잭션 문구만 정정)
---

# Object Animation Contract

[TARGET] 캐릭터 locomotion0/전투0/립싱크0. 아바타·보행·NavMesh 없음. 10개 재사용 클립: valve_turn, lever_throw, gauge_settle, connector_snap, drawer_open, plate_insert, stamp_down, pump_spin, shutter_raise, water_level. 시간/각도는 데이터. 시작·완료·취소 이벤트만 시뮬레이션에 요청하고 애니메이션이 정답을 쓰지 않는다.

| 레이어 | 시간 목표 | 진실 소유자 | 취소 |
|---|---|---|---|
| 케이블/판 끼우기 |180~280ms|command preview(후보)|입력취소면 원위치|
| 밸브 회전 |350~650ms|committed state|완료 전에 back이면 상태snap|
| 계기 정착 |300~500ms|state snapshot|감속 없이 최종값 표시 가능|
| 수면 변화 |900~1400ms|routing solver|reduce motion 즉시최종|
| 도장 확인 |300ms|**저장 성공 영수증 이후 UI**|실패 시 도장·사운드 모두 미재생|

## 확정 트랜잭션과 입력 (R2 정정)

- **전역 입력 잠금 없음.** 이전 판의 "확정 트랜잭션 최대 1프레임 입력 잠금"은 폐기한다 — 저장 완료 목표(200ms)와 프레임 예산(16.7ms)이 서로 다른 축이라 1프레임 잠금으로는 저장을 덮을 수 없었다.
- `SavePending` 동안 **중복 확정만 비활성**이고 되돌림·설정·힌트·열람 입력은 계속 처리된다. 애니메이션이 입력을 기다리게 하지 않는다.
- **두 축을 분리해 읽는다**: 확정 눌림의 **렌더 응답(ack)은 1프레임 이내 목표**, **디스크 완료는 200ms 목표**. 도장 애니는 렌더 ack가 아니라 **저장 영수증**에 걸린다.
- 저장 실패 시 `stamp_down`은 재생하지 않고 확정 사운드도 내지 않으며, 화면은 확정 이전 상태를 유지한다. 성공 연출을 먼저 보여주고 나중에 되돌리지 않는다.
- 후보(preview) 애니는 재생해도 좋으나 **권위 있는 상태 변화로 읽히는 연출**(도장·봉인·구역 수면 확정)은 영수증 이후에만 재생한다.

## 테스트

애니 도중 저장/일시정지/도구변경/언어변경/빠른연속10회. 추가: **저장 지연 주입 중 도장 미재생**, **저장 실패 주입 후 확정 사운드 0회**, **지연 완료 콜백 도착 시 도장 재생 0회**. 클립 미실행이 게임 진행을 막으면 FIX.
