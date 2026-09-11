---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-planner
---

# evidence-graph-overlay.json 메타데이터

[TARGET] **저작 오버레이**다 — `worldview/timeline.md` §4(세 반전의 씨앗과 회수)·§7(33비트 공개 상한표)·§8(순서 앵커)의 씨앗/회수 지점을 기계가독 `{factId, seedBeats[], revealBeat}` 형식으로 옮긴 것. 캐논 정본은 timeline.md이며 timeline이 바뀌면 이 파일은 재도출 대상이다. glossary·timeline 등 worldview 정본은 편집하지 않았다.

fact 6건과 근거 (각 항의 `basis` 필드에 원문 행 인용):

| factId | seedBeats | revealBeat | 근거 |
|---|---|---|---|
| R1 | t0-b2 | c2-b3 | §4 R1 행 · §7 B02 'R1 씨앗' → B10 '[R1 회수]' |
| R2-fact | c1-b2, c1-b3, c1-b4 | c4-b2 | §4 R2 행 · §7 B05/B06 'R2 씨앗', B07 '보강' → B17 '[R2 사실 회수]' |
| R2-effect | c1-b2, c1-b3, c1-b4 | c4-b3 | §7 B18 'R2 회수(효력)' — RFC-W4: 효력은 c4-b3까지 |
| R2-intent | c1-b2, c1-b3, c1-b4, c4-b4 | c6-b4 | §7 B19 'R2 보강' → B27 'R2 회수(의도)' — RFC-W4(§9 OPEN-4 해소): 의도는 c6-b4 |
| R3 | c2-b1, c2-b4, c3-b1 | c6-b4 | §4 R3 행 · §7 B08 'R3 원거리 씨앗', B11/B12 'R3 씨앗' → B27 '[R3 회수]' |
| order-anchor | c1-b3, c3-b3 | c6-b3 | §8 표(씨앗 B06 봉인대 훈련·기준 B14 반증 시험·회수 B26) · §7 B06/B14/B26 행 |

저작 판단 2건:
- [TARGET] R3의 `c6-b1`·`c6-b2`는 §7에 'R3 회수 준비'로 적혀 있어 씨앗이 아니라 준비 단계로 보고 seedBeats에서 제외했다.
- [TARGET] R2 계열 3건(fact/effect/intent)은 §4의 "4장(사실·효력) / 6장(의도)" 분리를 RFC-W4 판정 그대로 3개 fact로 나눴다 — 효력 c4-b3, 의도 c6-b4.

B# 표기는 위 표에서 timeline §7-0 정의의 파생 색인으로만 썼고, JSON 본문의 키는 전부 campaign id다.

검증: `node _workspace/current/planning/validate-evidence-graph.mjs` EG-DISC-01~03 (존재·위상 순서·requires 조상 폐포 — 구조 검사만, 텍스트 의미 판정 없음) [OBSERVED 2026-09-11 PASS].

[TARGET] draft 사유: worldview 레인의 캐논 대조 ACK 전. 씨앗/회수 캐논이 이 표와 어긋나면 timeline.md가 이긴다.
