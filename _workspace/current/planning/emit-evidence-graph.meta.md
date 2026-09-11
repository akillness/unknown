---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-planner
---

# emit-evidence-graph.mjs 메타데이터

[TARGET] TRACE-RPG(`~/orca/neural_symbolic_in_game`) 방법론 번안 1/2 — **생성기**. 정본 `planning/campaign.json`(+선택 오버레이 `planning/evidence-graph-overlay.json`)에서 typed 증거 그래프 `planning/evidence-graph.json`을 파생한다. kg-ontology의 typed 노드/엣지·closed-world 방식과 C3 audit-linked(입력 sha256·bytes를 출력 provenance에 기록) 방식을 따른다.

- 실행: `node _workspace/current/planning/emit-evidence-graph.mjs` (Node ≥18, 의존성 0, ESM)
- 캠페인 정본과 유일 캠페인 측정 명령(`validate-campaign.mjs`, 49검사)을 **대체하지 않는다**. 어떤 정본 파일도 쓰지 않고 `evidence-graph.json`만 쓴다.
- 결정론: 노드 (type,id)·엣지 (type,from,to) 정렬 + 재귀 키 정렬. 동일 입력 → byte-동일 출력 [OBSERVED — 연속 2회 실행 후 `shasum -a 256 evidence-graph.json` 동일, 2026-09-11].
- 노드 5종: `stage`(9) · `beat`(33, `docIndex` = campaign 전역 등장 순서로 timeline.md §7-0 파생과 동일 — 인용 키는 언제나 campaign id) · `clue`(73) · `source`(campaign에 실제 등장하는 originId 유니크 31, plate-zero 포함) · `tool`(등장 도구 6종).
- 엣지 7종: `contains`(stage→beat) · `yields`(beat→clue) · `from_source`(clue→source) · `copied_from`(clue→source) · `requires`(beat→선행 beat) · `uses_tool`(beat→tool) · `teaches_tool`(beat→tool, mode 부착).
- [OBSERVED] 과제 문안은 `copied_from`을 clue→clue로 적었으나, live 데이터의 `clue.copiedFrom` 값은 clue id가 아니라 자료 카탈로그의 source id다(`c4-b1-c1 → tide-ledger-bureau`, `c5-b3-c3 → council-copybook`). 그래서 엣지를 **clue→source**로 발행하고, source 간 계보는 검증기가 `validate-campaign.mjs` C-04/C-05와 같은 방식(originId→copiedFrom 지도)으로 파생한다.
- sha 숫자는 여기 재기재하지 않는다(RFC-Q1 방식) — provenance는 `evidence-graph.json` 안의 기계 기록이고, 대조는 `validate-evidence-graph.mjs` EG-PROV-01 출력을 인용한다.

[TARGET] draft 사유: QA 레인의 독립 재실행 검증 전. 승격은 QA 판정 후 별도 결정.
