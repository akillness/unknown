---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-planner
---

# validate-evidence-graph.mjs 메타데이터

[TARGET] TRACE-RPG 번안 2/2 — **검증기**. `evidence-graph.json`이 (1) 지금 디스크의 입력에서 파생된 것이 맞고 (2) TRACE-RPG 6검사 가족을 이 게임 데이터로 번안한 구조 불변식을 지키는지 판정한다. `validate-campaign.mjs`(유일 캠페인 측정 명령, 49검사)를 **대체하지 않는** 별도 파일이며, **어떤 경우에도 파일을 쓰지 않는다** — 실패해도 상태 불변(TRACE-RPG: 거부된 후보는 변경 없는 이전 상태로 폴백).

- 실행: `node _workspace/current/planning/validate-evidence-graph.mjs` (Node ≥18, 의존성 0)
- 출력: 검사마다 `<id> <이름> <PASS|FAIL> count=<위반수> [위반 id…]` 1줄 + `EG-SUMMARY checks N pass N fail N <PASS|FAIL>` 1줄. 종료 코드 0/1/2.

## 검사 18건 — TRACE-RPG 가족 대응

| 가족 (TRACE-RPG) | 번안 id | 내용 |
|---|---|---|
| C3 audit-linked | EG-PROV-01 | provenance 입력 sha256·bytes를 디스크에서 재계산해 대조 |
| precondition | EG-PRE-01·02 | requires 양끝 실존 beat · requires DAG(사이클 0, DFS 착색) |
| quest stage | EG-STAGE-01·02 | 스테이지 순서(T0<C1<…<C7<E0) 역행 0 · stage 귀속 = contains 일치 |
| reachability | EG-REACH-01·02·03 | docIndex가 requires DAG의 위상 순서임을 **먼저 검증** · t0-b1 기점 전 beat 도달(고아 0) · clue별 yields 정확히 1 |
| NPC knowledge | EG-KNOW-01~05 | originId→source 실존 · source 집합 == campaign 재파생 카탈로그(31종, 그래프 자기참조 공허 통과 방지) · copied_from 대상 실존 · 계보 지도 모순 0 · 계보 사이클 0(루트 도달) |
| action policy | EG-TOOL-01·02 | 도구 canon 6종 안 · 각 도구 첫 교습 ≤ 첫 사용 (검증된 docIndex 기준) |
| disclosure | EG-DISC-01·02·03 | 오버레이 fact의 seed·reveal 실존 · 모든 seed가 reveal보다 위상 순서상 앞 · reveal의 requires 조상 폐포에 seed 전부 포함. **구조 검사만** — 텍스트 의미 판정(어느 문장이 반전을 미리 말하는지)은 하지 않으며, 텍스트 상한은 timeline §7 상한표·validate-campaign 텍스트 검사 소유 |

"위상 순서" 비교 기준: EG-REACH-01이 campaign 문서 순서(docIndex)가 requires DAG의 유효한 위상 정렬임을 먼저 기계로 확인한 뒤, EG-TOOL-02·EG-DISC-02가 그 검증된 docIndex를 쓴다. 위상 정렬이 유일하지 않은 DAG에서 자의적 순서를 고르지 않기 위한 설계다.

## 실행 영수증 [OBSERVED 2026-09-11]

- live 데이터: `EG-SUMMARY checks 18 pass 18 fail 0 PASS`, exit 0. 실제 데이터 결함 0건 — FAIL로 기록할 항목 없음.
- 결정론: 생성기 연속 2회 실행 → `shasum -a 256` byte-동일.
- 음성 시험(반증): 리포 밖 임시 사본에 결함 4종 주입(prerequisites 사이클, 유령 originId, canon 밖 도구, reveal 뒤 seed) → 7개 검사 FAIL·exit 1·리포 파일 무변경. 검사들이 공허하게 통과하지 않음을 확인.
- 비간섭: 동일 시점 `validate-campaign.mjs` 49/49 PASS, exit 0 — 캠페인 계약 무변경.

sha 숫자는 여기 재기재하지 않는다 — EG-PROV-01 출력을 인용한다.

[TARGET] draft 사유: QA 레인 독립 재실행 전. 이 검증기는 문서·데이터 정합만 검사하며 플레이 표본 n=0, 어떤 G 게이트도 올리지 않는다.
