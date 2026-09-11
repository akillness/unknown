---
updated: 2026-09-10
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-economy-designer
---

# T0 회로 좌표 패킷 경제 검토

**[OBSERVED · 검토 결정] ACK — RFC-CX-003의 회로 좌표 데이터에 한정한다.** `planning/t0-circuit-overlay.json`과 같은 basename의 메타데이터를 현재 경제 정본과 대조했다. 패킷은 도식 대응점과 평행 이동 규칙을 저작하며, 새 보상·자원 소스·싱크·가격·시간 제한을 도입하지 않는다. 이 ACK는 해당 메타데이터의 다른 레인 승인이나 런타임 게이트 승격을 대신하지 않는다.

## 검토 입력과 경계

- [TARGET · 기획 저작값] 대응점 3개, `gridStep: 1`, `fineGridStep: null`, `initialOffset: {x: 0, y: 0}`. 정답 오프셋 `(-1, +1)`은 각 `target - overlay`의 공통 결과다. 좌표는 기획 패킷 §1–2의 저작값이며 플레이 시간·난이도 실측이 아니다.
- [OBSERVED · 2026-09-10] 읽기 전용 Node 검사로 JSON 파싱, 대응점별 차이 계산, 전체 속성명 순회를 수행했다. 모든 대응점의 차이는 `(-1, +1)`이고 `reward/source/sink/price/cost/currency/budget/time/duration/minute/hint`에 해당하는 속성은 없다. 메타데이터 §1도 기존 캠페인 비트·분 배분·보상을 바꾸지 않는 범위를 명시한다.
- [CARRIED] `campaign.json`의 기존 `t0-b2` 결과와 보상은 그대로 참조한다. 이 패킷은 기존 완료 결과를 추가 지급하거나, 좌표 이동·정렬 시도·초기 위치 복원에 비용을 붙이는 권한을 주지 않는다.

## 유지하는 정본 계약

| 경계 | 유지할 계약 | 근거 |
|---|---|---|
| 소스·싱크 | T0에서 `routing`을 사용하지 않으며 부식 예산 소모 없음. 회로 정렬에 새 부식 소모·충전·재부여를 추가하지 않는다. | `economy/sink-source-ledger.md` §2, §3.1 T0 행; `economy/currency-map.md` §3–4.1 |
| 연습·실패·복구 | 연습·프리뷰·되돌림은 자원을 소모하지 않는다. 좌표 재시도와 초기 위치 복원에 비용이나 횟수 소모를 붙이지 않는다. | `economy/currency-map.md` §3 불변식; `economy/reward-bands.md` §3 `practice.resource_cost` |
| 힌트·접근 | 힌트는 무료·무제한이며 결말·평가에 영향이 없다. 열람 권한은 회수하지 않는다. | `economy/sink-source-ledger.md` §4 INV-H0, §2 R5; `economy/currency-map.md` §5 |
| 보상 | 기존 접근 권한·열람 경로·대화 분기·공간 상태 채널을 유지한다. 좌표를 맞추는 과정에 통화·스탯·무작위 보상을 추가하지 않는다. | `economy/reward-bands.md` §1–2, 특히 기존 `t0-b2` 공간 상태 예시 |
| 가격·시간 | 통화·상점·거래, 시간 잠금·대기 보상은 도입하지 않는다. 판매 가격은 제품 PM 소관이며 좌표 저작으로 변경하지 않는다. | `economy/reward-bands.md` §2; `economy/negotiation-record.md` §1 및 N-02 |

## 판정과 미측정 항목

```yaml
review:
  rfc: RFC-CX-003
  scope: t0-circuit-overlay-economic-contract
  reviewer: game-economy-designer
  agent: t0_economy_ack
  decision: ack
  evidence_type: document-and-json-inspection
  new_reward: false
  new_source: false
  new_sink: false
  pricing_change: false
  authored_time_change: false
  economy_defect_found: false
  runtime_cost_behavior: NOT_MEASURED
  player_completion_time: NOT_MEASURED
  G3: NOT_MEASURED
```

[OBSERVED] 이 파일만 신규 작성하며 기존 경제 정본·캠페인·생성 테이블·Unity 코드는 수정하지 않는다. 플레이 소요 시간이 달라지지 않는다는 실측 주장은 하지 않는다. 실제 입력·취소·재개·저장·완료에서 무소모 계약을 지키는지는 시스템 구현과 QA가 확인해야 한다. G3의 미측정 항목과 기존 열린 협상은 이 ACK로 닫지 않는다.

[OBSERVED · 2026-09-10] `rtk run 'bash .claude/skills/game-ops-harness/scripts/freshness-check.sh'` → exit 0, `0 finding(s) across 135 markdown artifact(s)`. 이는 frontmatter·supersedes 구조 검사이며 시점 신선도·memory_sync·G8 전체 검증은 아니다. 새 검토 기록에 fresh frontmatter를 확인했고, 기존 파일 대체·코드 변경은 없어 아카이빙·코드 그래프 갱신 대상은 없다. 다른 레인의 ACK 수집과 전체 게이트 판정은 디렉터 소관이다.
