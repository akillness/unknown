---
updated: 2026-09-10
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
---

# graphify-update.log 영수증

- command: `graphify update .`
- cwd: `/Users/jangyoung/orca/unknown`
- executor: root director
- result: [OBSERVED] root 보고 exit 0. 로그 및 현재 graph.json에서 13,592 nodes / 17,350 edges를 확인했다.
- source: `/tmp/unknown-t0-m1-graphify-update.log`
- durable: `graphify-update.log`
- bytes: 1124
- sha256: `76772467159996da5c8b5e6319b14c9617bbbc9892f24802dc46c35a31daa545`
- copy_verification: [OBSERVED] source와 durable 로그의 바이트가 동일하다.
- scope: [OBSERVED] Sim 소스 41 nodes가 포함됐다. 커밋된 HEAD graph 기준 Sim 0 nodes에서 실제 구현 소스가 추가됐다. 동시에 Unity Library 캐시 7,578 nodes가 포함된 상태다.
- verdict: **PARTIAL** — 소스 반영 확인, 캐시 제외 재갱신은 후속 ENOSPC로 미완료. mex-agent 없음도 별도 미완료다.

통합 근거는 `../../t0-m1-native.md`를 참조한다. 이 로그는 플레이/성능 게이트 영수증이 아니다.
