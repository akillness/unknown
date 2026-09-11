---
updated: 2026-09-09
cycle: 20260909-preproduction-c4
status: superseded
supersedes: null
owner: game-animator
---

# Object Animation Contract

[TARGET] 캐릭터 locomotion0/전투0/립싱크0. 10개 재사용 클립: valve_turn, lever_throw, gauge_settle, connector_snap, drawer_open, plate_insert, stamp_down, pump_spin, shutter_raise, water_level. 시간/각도는 데이터. 시작·완료·취소 이벤트만 시뮬레이션에 요청하고 애니메이션이 정답을 쓰지 않는다.

| 레이어 | 시간 목표 | 진실 소유자 | 취소 |
|---|---|---|---|
| 케이블/판 끼우기 |180~280ms|command preview|입력취소면 원위치|
| 밸브 회전 |350~650ms|committed state|완료 전에 back이면 상태snap|
| 계기 정착 |300~500ms|state snapshot|감속 없이 최종값 표시 가능|
| 수면 변화 |900~1400ms|routing solver|reduce motion 즉시최종|
| 도장 확인 |300ms|save receipt 이후 UI|실패 시확정소리 없음|

입력 잠금은 확정 트랜잭션 최대1프레임, 모션을 기다리게 하지 않는다. 애니 도중 저장/일시정지/도구변경/언어변경/빠른연속10회 테스트. 클립 미실행이 게임 진행을 막으면 FIX.
