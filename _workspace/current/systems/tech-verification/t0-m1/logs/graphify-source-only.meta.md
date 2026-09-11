---
updated: 2026-09-10
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
---

# graphify-source-only.log 영수증

- command: `graphify update . --force`
- cwd: `/Users/jangyoung/orca/unknown`
- executor: root director
- prerequisite: root가 `.graphifyignore`를 생성하고 `_is_ignored`에서 Unity 캐시=True / Sim=False를 확인했다.
- result: [OBSERVED] root 보고 exit 1. 원문은 `[graphify watch] Rebuild failed: [Errno 28] No space left on device`다.
- source: `/tmp/unknown-t0-m1-graphify-source-only.log`
- durable: `graphify-source-only.log`
- bytes: 237
- sha256: `312e5b2db7666518538aea0615f4c9ba3e0647d2d823013fbf521bc500b2f007`
- copy_verification: [OBSERVED] source와 durable 로그의 바이트가 동일하다.
- retained_graph: [OBSERVED] 현재 `graphify-out/graph.json`과 `/tmp/unknown-t0-m1-graph-before-ignore.json.gz` 압축 해제 바이트 동일. SHA-256 `70b6c08f0680cd8aee538dab0925736e399fa67081d5d3cf9c67a1a45a4e965a`.
- verdict: **PARTIAL** — 먼저 성공한 소스 포함 그래프가 유지되며 캐시 제외 그래프는 미완료다. 재시도나 추가 디스크 정리는 실행하지 않았다.

통합 근거는 `../../t0-m1-native.md`를 참조한다. 기존 그래프를 없는 것으로 취급하거나 캐시 제외 성공으로 표기하지 않는다.
