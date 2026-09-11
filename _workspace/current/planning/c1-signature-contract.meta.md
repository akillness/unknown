---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-planner
---

# C1 서명지 데이터 계약 메타데이터

[OBSERVED] 새 `c1-b2` 구현 패킷이다. 기존 `c1-patrol-contract` 또는 campaign을 대체하지 않는다.

- JSON: [c1-signature-contract.json](c1-signature-contract.json)
- 해설: [c1-signature-contract.md](c1-signature-contract.md)
- SHA-256: `9bff17b3118e8c51388fca96f4ec94e6451b388d816ec504cd1737166d651515`
- 승인: `production/decision-log.md` RFC-CX-005, director ACK 2026-09-11.
- 정본: `planning/campaign.json` → C1/c1-b2. 캐논 원문, 관찰 sourceType/originId/copiedFrom은 일치 검사를 통과했다.
- 저작 검증: 44/44 정적 assertion PASS; 17개 런타임 수용 기준은 선언이며 실행 통과를 뜻하지 않는다.

[TARGET] current는 최신 저작 계약 상태다. Unity 회귀, 실제 저장 이전, 입력별 조작, 아트/연출 판독성, 사람 재미/플레이시간은 systems 및 QA의 별도 실행 영수증으로 판정한다.
