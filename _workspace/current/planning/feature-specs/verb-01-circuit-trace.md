---
updated: 2026-09-10
cycle: 20260909-preproduction-c3
status: current
supersedes: null
owner: game-planner
---

# verb-01 · 배선 추적 (Circuit Trace)

```yaml
verb_id: circuit
name_ko: 배선 추적
name_en: Circuit Trace
law: "법1 배선된 것만 남는다"
layer: sandbox-only          # 확정 층 없음 — 다른 동사의 확정을 검증하는 판독 도구
introduced_at: t0-b2
unguided_reprise: c6-b2
required_beats: 10          # live campaign.json 재측정 (RFC-P3-008)
design_minutes_touched: 149  # 그 비트들의 분 합(중복 계상). 480의 분해가 아니다
goal: >
  플레이어가 "이 사건은 애초에 기록될 수 없었다"를 스스로 판정하게 만든다.
  증거의 부재와 사건의 부재를 구분하는 능력이 이 게임 추리의 1층이다.
player_fantasy: >
  도면을 손으로 짚어가며 "여기엔 센서가 없었네"를 먼저 알아채는 사람.
  남이 준 결론이 아니라 배관을 따라간 내 손가락이 그 결론을 만든다.
touched_lanes: [systems, presentation, concept, qa, worldview]
```

> **`law:` 필드의 출처** [OBSERVED, RFC-P3-014]: 6법 호명 문구의 정본은 `worldview/worldview-bible.md` §3 표 하나뿐이며 위 값은 그 문구를 **문자 그대로 인용**한 것이다(2026-09-10 전건 대조, 6/6 일치). `worldview/consistency-audit.md` §4에 보존된 폐기 문구는 쓰지 않는다.
> **집계의 출처** [OBSERVED]: `required_beats`·`design_minutes_touched`는 live `planning/campaign.json`을 `node planning/validate-campaign.mjs`로 집계한 값이다(2026-09-10 R4 재실행 = sha256 `92301c0a…` · 121457 B · `zoneId` 추가분, 집계는 **불변**). 이후 인용은 고정 sha를 다시 적지 말고 **검증기 출력의 `sha256`을 쓴다**(RFC-Q1). 이전 판의 값은 아카이브 c3 계보였다(C3-F1 / RFC-P3-008).

## rules

| # | 규칙 | 근거 |
|---|---|---|
| R1 | 구역 평면에서 계통선을 따라 센서 노드를 선택한다. `Shift`/다중 선택으로 범위를 비교한다 | `systems/interaction-rules.md` §2.1 |
| R2 | 센서 범위 **밖** 영역은 사선 해칭 + "미배선" 문자 라벨로 표시한다. 색 단독 금지 | GDD §8, `systems/game-ui-contract.json` accessibility.signals |
| R3 | 이 동사는 **확정하지 않는다.** 대신 다른 동사의 확정에서 배선 밖 근거를 자동 무효로 만들고 사유를 문장으로 표시한다 | `systems/interaction-rules.md` §2.1 |
| R4 | 미배선 판정은 진행을 막지 않는다. "근거 부족"으로 표시되고 다른 매체 수집 경로가 열린다 | `worldview/worldview-bible.md` §3 법1 실패와 회복 |
| R5 | 어떤 조작도 세계 상태를 바꾸지 않는다. 되돌림 개념 자체가 없다(연습 층 전용) | GDD §3.3 |

## edge_cases

| # | 상황 | 요구 동작 |
|---|---|---|
| E1 | 플레이어가 미배선 구간의 사건을 결론 슬롯에 넣는다 | 슬롯이 즉시 비활성 + "배선 범위 밖" 배지 + 사유 문장. 넣은 입력은 지우지 않는다 |
| E2 | 계통이 중간에 끊긴 구간(폐쇄된 `pump`) | "폐쇄 계통" 별도 라벨. 미배선과 다른 표시 — 폐쇄는 과거에 배선이 있었음을 뜻한다 |
| E3 | 같은 사건이 두 계통의 경계에 걸친다 | 두 계통 모두 강조하고, 확정에는 두 계통 각각의 근거를 요구한다 |
| E4 | 색약 팔레트에서 강조선과 해칭이 겹친다 | 해칭 패턴 각도를 계통별로 다르게(색 외 2차 채널) |
| E5 | 플레이어가 도면을 한 번도 열지 않고 `c6-b2`(미안내 재문제)에 도달 | 진입은 허용. 힌트 1단계가 도구 존재를 알리되 정답 계통은 말하지 않는다 |
| E6 | 대형 구역에서 노드가 화면 밖에 있다 | 패드 스틱/D-Pad로 노드 간 이산 순회 가능. 드래그 전용 경로 금지 |

## acceptance_criteria

### D — 문서 단계에서 지금 측정 가능

| id | 기준 | 현재 값 | 판정 |
|---|---|---|---|
| D1 | 필수 비트 ≥ 8건 | **10건 / 149분** [OBSERVED live 재측정, `validate-campaign.mjs` `toolBeatCounts.circuit`] | PASS |
| D2 | 도입 1회 + 미안내 재문제 ≥ 1회 | `t0-b2` / `c6-b2` [OBSERVED campaign.meta.md §5.6] | PASS |
| D3 | 이 동사를 쓰는 모든 비트에 매체 2종 이상 | 10/10 [OBSERVED] | PASS |
| D4 | 이 동사만으로 확정되는 비트 = 0 (판독 전용 원칙) | 0건 [OBSERVED — 확정 조건 필드 없음] | PASS |
| D5 | 비활성 사유 문자열이 규칙마다 1:1 | 미배선은 정의됨(`out_of_coverage`, systems `wiring-trace.md` W-R2/W-F1). **폐쇄 계통·계통 경계 2종은 미정의** [OBSERVED] | FIX → C4 systems |

### B — 빌드 후에만 측정 가능 (현재 전부 n=0, NOT-MEASURED)

| id | 기준 | 표본 |
|---|---|---|
| B1 | `t0-b2` 첫 유효 조작까지 중앙값 ≤ 60초 | 12명/5유형 (`balance/puzzle-balance.md` 목표 인용) |
| B2 | `c6-b2`(미안내) 힌트 3단계 도달률 ≤ 35% [TARGET, 기획 제안 — balance 승인 필요] | 동일 |
| B3 | "미배선"의 뜻을 자기 말로 설명 ≥ 10/12명 | 동일 |
| B4 | 이 동사로 인한 진행 불가 0/12 | 동일 |
| B5 | 패드 단독 플레이에서 노드 순회 실패 0건 | 패드 전용 세션 ≥ 3명 |

## telemetry_fields

| 필드 | 타입 | 출처 | 이 스펙에서의 용도 |
|---|---|---|---|
| `circuit_open_count` | int | systems wiring-trace | 도구 접근 빈도 |
| `circuit_trace_count` | int | systems wiring-trace | 계통 추적 시도 |
| `circuit_uncovered_marked` | int | systems wiring-trace | 미배선 구획 표시 수 |
| `circuit_out_of_coverage_blocked` | int | systems wiring-trace | B4 진행 불가 판정 |
| `circuit_time_min` | float | systems wiring-trace | 체류 시간(afk 제외 규칙 적용) |
| `beat_reached` | (beat_id, t) | 공통 키 | 사건 식별 |
| `hint_used` | (level, beat_id, t) | systems hint-system | B2 힌트 3단 도달률 |
| `time_to_first_valid_action_ms` | int | **[신규 제안]** | B1 첫 조작 60초 기준 |
| `input_device_last` | enum(kbm, pad) | **[신규 제안]** | B5 패드 단독 검증 |

> **명명 규칙.** 아래 키는 `systems/system-specs/*.md` §6이 이미 정의한 이름을 그대로 쓴다. 기획이 새로 요구하는 키만 **[신규 제안]**으로 표시하며, 채택 여부는 systems가 판정한다. 공통 키(`beat_reached`, `hint_used`, `sandbox_time_min`, `commit_time_min`, `afk_gap_sec`)의 정의는 `systems/ops/telemetry-contract.md`가 소유한다 — [OBSERVED 2026-09-10 재측정] **이 파일은 존재한다**(`ls systems/ops/` → `telemetry-contract.md`, cycle c3 · status current). 이전 판의 "아직 없다"는 스테일 주장이었다(C3-F15).
수집 원칙: 연습(sandbox) 층 로그는 **분석 전용**이며 게임 내 판정·엔딩·업적에 쓰지 않는다(GDD §3.3 원칙 4).
