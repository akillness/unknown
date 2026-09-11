---
updated: 2026-09-10
cycle: 20260909-preproduction-c3
status: current
supersedes: null
owner: game-planner
---

# verb-03 · 조위정합 (Tide Alignment)

```yaml
verb_id: alignment
name_ko: 조위정합
name_en: Tide Alignment
law: "법3 정합 전 시계는 믿지 않는다"
layer: sandbox + commit      # 연습=피크 지정과 잔차 확인 / 확정=기준선 확정
introduced_at: c3-b2
unguided_reprise: c6-b3
required_beats: 8          # live campaign.json 재측정 (RFC-P3-008)
design_minutes_touched: 141  # 그 비트들의 분 합(중복 계상). 480의 분해가 아니다
goal: >
  "언제"가 공짜로 주어지지 않는 게임을 만든다. 시간은 읽는 것이 아니라 두 자료를
  맞춰서 만드는 것이며, 못 맞추면 순서를 말하지 않는 것이 정답이다.
player_fantasy: >
  서로 다른 관측소의 곡선 두 개를 겹쳐 공통 피크 세 개를 짚고, ±40분의 안개가
  ±4분으로 좁아지는 순간을 손으로 만들어내는 사람.
touched_lanes: [systems, balance, presentation, worldview, qa]
```

> **`law:` 필드의 출처** [OBSERVED, RFC-P3-014]: 6법 호명 문구의 정본은 `worldview/worldview-bible.md` §3 표 하나뿐이며 위 값은 그 문구를 **문자 그대로 인용**한 것이다(2026-09-10 전건 대조, 6/6 일치). `worldview/consistency-audit.md` §4에 보존된 폐기 문구는 쓰지 않는다.
> **집계의 출처** [OBSERVED]: `required_beats`·`design_minutes_touched`는 live `planning/campaign.json`을 `node planning/validate-campaign.mjs`로 집계한 값이다(2026-09-10 R4 재실행 = sha256 `92301c0a…` · 121457 B · `zoneId` 추가분, 집계는 **불변**). 이후 인용은 고정 sha를 다시 적지 말고 **검증기 출력의 `sha256`을 쓴다**(RFC-Q1). 이전 판의 값은 아카이브 c3 계보였다(C3-F1 / RFC-P3-008).

## rules

| # | 규칙 | 근거 |
|---|---|---|
| R1 | 두 자료의 조위 곡선을 겹치고 **공통 피크 3개**를 각각 지정한다 | `systems/interaction-rules.md` §2.3 |
| R2 | 원시 오차 **±40분** → 정합 후 잔차 **±N분**이 상단에 실시간 갱신된다 | 세계관 §2 |
| R3 | 확정 활성 조건: 피크 3개 지정 **그리고** 잔차 절댓값 ≤ **4분** | 세계관 §2, systems §2.3 |
| R4 | 사건 선후 확정 조건: 두 사건 간격 > **두 관측소 오차폭 합 8분**. 20분 간격은 확정 가능(20 > 8) | 세계관 §2 |
| R5 | 오차띠가 겹치면 판정은 `indeterminate`로 남고 **진행은 막히지 않는다** | 법3 실패와 회복 |
| R6 | 재정합 횟수 제한 0 · 비용 0. 정합 실패는 자원·권한을 빼앗지 않는다 | GDD §3.3 |
| R7 | 오프셋 조작에 **이산 입력 대안**(D-Pad 1분 스텝)이 반드시 있다 | GDD §5, §8 |

## edge_cases

| # | 상황 | 요구 동작 |
|---|---|---|
| E1 | 피크를 2개만 지정하고 확정 시도 | 확정 비활성 + "공통 피크 3개 필요" 문장. 지정한 2개는 유지 |
| E2 | 잔차가 4.0분 경계값 | ≤ 4.0 은 통과. 부동소수 비교는 정수 분 단위로 반올림 후 비교(경계 흔들림 금지) |
| E3 | 두 자료의 관측소가 같다 | 정합 불필요 안내 + 도구를 열되 확정 슬롯을 비활성 |
| E4 | 잘못된 피크 쌍으로 잔차가 우연히 4분 이하 | 허용한다. 대신 그 기준선으로 도출한 순서가 다른 매체와 충돌하면 이중서명 단계에서 사유가 뜬다 |
| E5 | 사건 간격이 정확히 8분 | `indeterminate` (초과 조건은 **엄격 부등호**) |
| E6 | 색약 사용자가 두 곡선을 구분 못 함 | 선 종류(실선/파선) + 곡선 라벨 병기, 색 단독 금지 |
| E7 | 정합 확정 후 원본 자료가 추가로 발견됨 | 기존 기준선을 유지한 채 재정합 가능. 이전 결론은 자동 무효화하지 않고 "재검토" 표시 |

## acceptance_criteria

### D — 문서 단계에서 지금 측정 가능

| id | 기준 | 현재 값 | 판정 |
|---|---|---|---|
| D1 | 필수 비트 ≥ 6건 | **8건 / 141분** [OBSERVED live 재측정] | PASS |
| D2 | 도입/미안내 재문제 | `c3-b2` / `c6-b3` [OBSERVED] | PASS |
| D3 | 세계관 수치와 문서 간 불일치 0 (±40 / ±4 / 4분 / 8분) | systems·worldview·planning 3문서 일치 [OBSERVED 대조] | PASS |
| D4 | `indeterminate` 상태로도 진행 가능한 경로가 비트마다 존재 | 비트별 대체 경로 **미열거** | FIX → C4 synopsis |
| D5 | 20분 인과(C2 QA F3)가 8분 오차폭보다 크다는 계산이 문서에 있다 | 있음 [OBSERVED 세계관 §2] | PASS |

### B — 빌드 후에만 측정 가능 (n=0)

| id | 기준 | 표본 |
|---|---|---|
| B1 | `c3-b2` 최초 정합 성공까지 시도 횟수 중앙값 ≤ 4회 [TARGET, balance 승인 필요] | 12명/5유형 |
| B2 | 정합 개념을 자기 말로 설명 ≥ 9/12명 | 동일 |
| B3 | 이 동사로 인한 진행 불가 0/12 | 동일 |
| B4 | 패드 D-Pad 단독으로 잔차 ≤ 4분 도달 성공 100% | 패드 전용 ≥ 3명 |
| B5 | `c6-b3`(3도구 종합) 이탈률 ≤ 10% [TARGET] | 12명 |

## telemetry_fields

| 필드 | 타입 | 출처 | 이 스펙에서의 용도 |
|---|---|---|---|
| `align_attempts` | int | systems tide-alignment | B1 시도 횟수 중앙값 |
| `align_residual_final` | float | systems tide-alignment | 확정 잔차(분) |
| `align_residual_history` | float[] | systems tide-alignment | 조작 추이(상한 200) |
| `align_indeterminate_count` | int | systems tide-alignment | 판정 불가 표시 |
| `align_auto_suggest_used` | int | systems tide-alignment | 자동 제안 조회 |
| `align_unlock_count` | int | systems tide-alignment | 확정 후 재정합 |
| `align_time_min` | float | systems tide-alignment | 체류 시간 |
| `hint_used` | (level, beat_id, t) | systems hint-system | B2 |
| `input_precision_mode` | enum(drag, stick, dpad) | **[신규 제안]** | B4 이산 입력 대안 검증 |

> **명명 규칙.** 아래 키는 `systems/system-specs/*.md` §6이 이미 정의한 이름을 그대로 쓴다. 기획이 새로 요구하는 키만 **[신규 제안]**으로 표시하며, 채택 여부는 systems가 판정한다. 공통 키(`beat_reached`, `hint_used`, `sandbox_time_min`, `commit_time_min`, `afk_gap_sec`)의 정의는 `systems/ops/telemetry-contract.md`가 소유한다 — [OBSERVED 2026-09-10 재측정] **이 파일은 존재한다**(`ls systems/ops/` → `telemetry-contract.md`, cycle c3 · status current). 이전 판의 "아직 없다"는 스테일 주장이었다(C3-F15).
수집 원칙: 연습(sandbox) 층 로그는 **분석 전용**이며 게임 내 판정·엔딩·업적에 쓰지 않는다(GDD §3.3 원칙 4).
