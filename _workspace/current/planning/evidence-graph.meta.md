---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-planner
---

# evidence-graph.json 메타데이터

[OBSERVED] **파생물이다 — 손으로 편집하지 않는다.** `emit-evidence-graph.mjs`가 정본 `planning/campaign.json`(+`evidence-graph-overlay.json`)에서 생성한다. 편집이 필요하면 정본이나 오버레이를 고치고 재생성한다. 정본 데이터·유일 캠페인 측정 명령은 그대로 `campaign.json` / `validate-campaign.mjs`다.

- 재생성: `node _workspace/current/planning/emit-evidence-graph.mjs`
- 검증: `node _workspace/current/planning/validate-evidence-graph.mjs` → 요약 줄 `EG-SUMMARY checks 18 pass 18 fail 0 PASS` [OBSERVED 2026-09-11]
- 입력 무결성: 파일 내 `provenance.inputs`에 입력별 sha256·bytes가 기계 기록되며(TRACE-RPG C3 audit-linked), 여기 재기재하지 않는다 — 대조는 EG-PROV-01 출력 인용.

## 노드/엣지 카운트 [OBSERVED — 생성기 stdout 및 파일 내 `counts`, 2026-09-11]

| 노드 type | 수 | | 엣지 type | 수 |
|---|---|---|---|---|
| stage | 9 | | contains | 33 |
| beat | 33 | | yields | 73 |
| clue | 73 | | from_source | 73 |
| source | 31 | | copied_from | 2 |
| tool | 6 | | requires | 42 |
| **합계** | **152** | | uses_tool | 42 |
| | | | teaches_tool | 12 |
| | | | **합계** | **277** |

검산: 9+33+73+31+6 = 152. 33+73+73+2+42+42+12 = 277. source 31 = glossary §7 자료 카탈로그 31종과 일치(EG-KNOW-02가 기계 검사).

[TARGET] draft 사유: QA 독립 재실행 전. `overlay` 블록(반전 씨앗/회수)은 저작물인 `evidence-graph-overlay.json`에서 그대로 옮겨진다 — 캐논 정본은 `worldview/timeline.md` §4·§7·§8.
