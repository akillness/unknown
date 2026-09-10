---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-planner
---

# C1 순찰로 데이터 계약 메타데이터

[OBSERVED] 신규 구현 패킷은 `c1-b1` 한 비트에 한정한다. 대체할 이전 동일 산출물이 없어 `supersedes: null`이다. 캐논은 [campaign.json](campaign.json)의 C1/c1-b1이며, 이 패킷은 서사·시간표·보상을 바꾸지 않는다.

- JSON: [c1-patrol-contract.json](c1-patrol-contract.json)
- 해설과 수용 기준: [c1-patrol-contract.md](c1-patrol-contract.md)
- SHA-256: `a06bcba4209c0caa044d8a37a9cc1b1d69b7dbd36d5ac32a8c7046705445731d`
- 승인: [decision-log.md RFC-CX-004 addendum](../production/decision-log.md#rfc-cx-004-addendum--canonical-transaction-and-prop-target), director ACK 2026-09-11.
- 데이터 필드 `authority.references`는 문서 경로이며 세부 규칙은 해설의 근거 표를 따른다.

[TARGET] `current`는 소유 기획 계약의 최신 상태다. Unity 구현·회귀·실제 입력 경로·사람 플레이·게이트 통과를 뜻하지 않는다. 단서·목표·결과·복구·힌트 문자열은 canonical beat에서 복사했다. UI 조작명과 상태/거부 사유는 이 패킷의 표시문구이며 인물 대사나 새 설정이 아니다. `systemId`·`stationId`·압력 하한 수치는 추론하지 않는다.

## 검증 경계

정적 계약 검사는 캐논 일치, 네 조합 중 유효 하나, 원자 확정 효과, 무료 복구의 진행 비부여, localization 키 참조를 확인한다. 실제 저장·입력·UI·Undo/Redo는 시스템 레인과 QA의 런타임 검사 대상이다. 자세한 실행 영수증은 해설 하단에 기록한다.

[OBSERVED · 2026-09-11] `rtk run 'bash .claude/skills/game-ops-harness/scripts/freshness-check.sh --root /Users/jangyoung/orca/unknown'` exit 0: 466 Markdown artifacts, findings 0. 검사는 frontmatter·supersedes 구조에 한정하며 시점 신선도와 memory_sync는 측정하지 않았다.

[INFERENCE · RFC-CX-004 director ACK · 2026-09-11] `preview.initialConfiguration`은 조명 ON·판독 ON이다. 공유 배전 충돌을 처음부터 표시하고 조명 분기를 접게 하는 구현 초기화 결정이며, 새로 관측한 캐논 사실이 아니다. 무료 우회관은 기존 조명 OFF·판독 ON 미리보기 복구를 유지한다.

[OBSERVED · 2026-09-11] 초기화 변경 집중 검사 5/5 PASS: ON/ON 최초 값, 공유 배전 충돌·표시문구, 무료 복구의 안전값과 진행 비부여, 나머지 기존 JSON 필드 무변경, 현재 SHA sidecar 일치. 기존 27개 검사는 앞선 패킷 검증 영수증이며 이번 변경을 런타임 실행으로 주장하지 않는다.
