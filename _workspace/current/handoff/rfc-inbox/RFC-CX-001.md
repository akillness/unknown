---
updated: 2026-09-10
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
---

### RFC-CX-001 · T0 인용 출처의 안정 ID 저작 계약

- lanes: worldview / synopsis / systems / director
- question: 용어집의 승인된 관측소·계통 ID와 레코드 귀속표를 소유자 ACK 후 생성기로 파생하도록 확정할 것인가?
- proposal: worldview가 기존 세계관 안의 안정 ID를 등록하고, synopsis가 레코드별 귀속을 명시한 뒤 systems 생성기가 `systemId`·`stationId`를 파생한다. 새 관측소·사건은 만들지 않는다. 현재 Unity는 누락 인용을 fail-closed로 거부하고 T0 전체 시작을 차단한다. 문자열이나 `zoneId`로 ID를 추정하지 않는다.
- evidence: `systems/data-schemas/plates.md` §1은 plate의 `systemId` 및 `stationId`를 요구한다. `handoff/codex-unity-brief.md` §⑤-3(B)는 인용에 매체·계통·관측소 출처가 모두 있어야 한다고 정한다. 생성 `systems/data/t0/records.json` 전 행에는 두 필드가 없다. `synopsis/t0-records.md:137`의 허브 계통, `:249`/`:256`과 `worldview/glossary.md:194`의 기록국 표준 관측소는 자연어 근거다. `t0-records.md:105`는 목록에 든 조위대장 설명이므로 이관 목록 자체의 station으로 전파할 수 없다. `:234`/`:238`~`:240`의 회선 범위도 기준 관측소 지정이 아니다. worldview 독립 검토가 동일한 경계를 확인했다.
- blocking: yes — 출처가 완비된 `CiteToBoard` 및 실제 T0 완주만 차단한다. M1의 탐색·표시 완료 술어, reader 사본 연습, 데이터 검증은 계속한다.
- measured: [OBSERVED] 실제 생성 데이터의 5개 레코드 모두 `systemId`/`stationId` 없음. 실제 완료 술어가 요구하는 인용 레코드 2개가 차단됨. Unity native 계약검사에서 누락 인용의 이벤트 0개 및 `t0-b3` 미완료를 검증했다. 양성 fixture의 `synthetic-system`/`synthetic-station`은 테스트 코드 안에만 있으며 생성 데이터에 기록하지 않았다. 영수증: `systems/tech-verification/t0-m1-native.md`.
- required_acks: worldview의 안정 ID 등록 / synopsis의 레코드 귀속 / director의 채택 및 감사 기록. 아직 저작 변경이나 승인된 ID는 없다.
- decided_by:
