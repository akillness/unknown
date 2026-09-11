---
updated: 2026-09-10
cycle: 20260909-preproduction-c3
status: current
supersedes: null
owner: game-planner
---

# verb-04 · 배수 편성 (Drain Routing)

```yaml
verb_id: routing
name_ko: 배수 편성
name_en: Drain Routing
law: "법4 이번 조수에는 보호 용량이 부족하다"
layer: sandbox(대기) -> commit(확정)   # 이 게임에서 세계 상태를 실제로 바꾸는 유일한 동사
introduced_at: c5-b2
unguided_reprise: c7-b2
required_beats: 3          # live campaign.json 재측정 (RFC-P3-008)
design_minutes_touched: 52  # 그 비트들의 분 합(중복 계상). 480의 분해가 아니다
goal: >
  선택의 무게를 문장이 아니라 지도 위에서 보게 만든다. 두 경로를 먼저 끝까지
  연습해 보고, 그 다음에 무엇을 잃을지 알고 확정한다.
player_fantasy: >
  물이 한 번에 한 곳으로만 간다는 사실을 알면서, 두 경로를 모두 돌려보고
  그래도 하나를 골라 손잡이를 길게 눌러야 하는 당직자.
touched_lanes: [systems, economy, balance, presentation, synopsis, qa]
```

> **`law:` 필드의 출처** [OBSERVED, RFC-P3-014]: 6법 호명 문구의 정본은 `worldview/worldview-bible.md` §3 표 하나뿐이며 위 값은 그 문구를 **문자 그대로 인용**한 것이다(2026-09-10 전건 대조, 6/6 일치). `worldview/consistency-audit.md` §4에 보존된 폐기 문구는 쓰지 않는다.
> **집계의 출처** [OBSERVED]: `required_beats`·`design_minutes_touched`는 live `planning/campaign.json`을 `node planning/validate-campaign.mjs`로 집계한 값이다(2026-09-10 R4 재실행 = sha256 `92301c0a…` · 121457 B · `zoneId` 추가분, 집계는 **불변**). 이후 인용은 고정 sha를 다시 적지 말고 **검증기 출력의 `sha256`을 쓴다**(RFC-Q1). 이전 판의 값은 아카이브 c3 계보였다(C3-F1 / RFC-P3-008).

## rules

| # | 규칙 | 근거 |
|---|---|---|
| R1 | 밸브·수문·양수 경로를 노드에 연결한다. 구성 변경 시 통수 화살표와 예상 수위 막대가 즉시 갱신된다 | `systems/interaction-rules.md` §2.4 |
| R2 | **대기(연습) 층**: 두 경로를 각각 끝까지 편성하고 완주 결과를 미리 본다. 비용 0, 횟수 제한 0 | GDD §3.3, `c5-b2` "두 경로 연습" |
| R3 | **확정 층**: 규칙 위반 0 + **구성안 총 부식 비용 ≤ 전역 상한 9** + 프리뷰 확인이 모두 참일 때만 활성. 확정 입력은 **두 단계 확인(`two-step`)이 기본** — 정본 문구 그대로 「프리뷰 → 초점을 확정 버튼으로 옮겨 한 번 누름. 홀드 없음」이며, `hold`(0.4초)는 opt-in, `confirm-dialog`(확정 후 확인 대화 1회)는 순서가 반대인 별도 옵션이다 | systems §2.4·§1-1, RFC-P3-009·RFC-P3-015 [C6-F14] |
| R4 | 확정 순간 구역 상태가 실제로 바뀌고 체크포인트가 생성된다 | GDD §3.3 원칙 2 |
| R5 | 제로섬의 결과는 **이진 플래그 `propertyProtection`(lowland\|dock) 1개**뿐이다. 후일담 문단 2쌍만 바뀐다 | systems §0.5, `campaign.meta.md` §5.2 |
| R6 | **주민은 항상 먼저 대피한다.** 선택 대상은 재산이며 인명이 아니다 | 세계관 §3 법4 |
| R7 | 필수 단서는 이미 복제되어 있다. 침수된 구역에서도 증거 접근과 세 결말 도달은 불변 | 법2, systems §0.5 |
| R8 | 확정 직전 자동 저장으로 선택을 재시도할 수 있다 | systems §5 |

## edge_cases

| # | 상황 | 요구 동작 |
|---|---|---|
| E1 | 한쪽 경로만 연습하고 확정 흐름에 들어간다(`two-step` 1단계 = 프리뷰) | 확정 허용. 단 **프리뷰 단계에서** 패널이 "미확인 경로 1개"를 명시하고 그쪽 결과 요약을 함께 보여준 뒤, 2단계(확정 버튼 누름 1회)로 발행된다 [C6-F14 순서 명시] |
| E2 | 규칙 위반 구간이 있는 채 확정 | 확정 비활성 + 위반 이름(붉은 점선 **및** 문자 라벨). 구성은 지우지 않는다 |
| E3 | 구성안 부식 비용이 **9**를 넘는다 | 확정 비활성 + 초과분·원인 부품 목록. `verb-05`가 해소 경로를 제공. 계통별 한도·누적 고갈은 없다(RFC-P3-009) |
| E4 | 막다른 구성으로 어떤 경로도 성립하지 않는다 | **무료 우회관 1회**가 언제나 복구한다. 영구 도구 상실 없음 |
| E5 | 확정 직후 되돌리고 싶다 | 확정 직전 체크포인트에서 재시도. 크레딧 이후에도 복귀 가능 |
| E6 | 플레이어가 두 구역 모두 보호를 시도 | 불가하며 사유가 문장으로 표시된다("이번 조수의 보호 용량 부족"). 게임이 침묵하지 않는다 |
| E7 | `c7-b2`(그날 밤 재현)에서 현재 상태와 12년 전 조건이 충돌 | 재현은 **연습 층 전용**이며 현재 구역 상태를 바꾸지 않는다 |

## acceptance_criteria

### D — 문서 단계에서 지금 측정 가능

| id | 기준 | 현재 값 | 판정 |
|---|---|---|---|
| D1 | 필수 비트 ≥ 3건 | **3건 / 52분** [OBSERVED live 재측정] — 하한 겨우 충족 | WARN (GDD §4.2 RFC-P3-002) |
| D2 | 도입/미안내 재문제 | `c5-b2` / `c7-b2` [OBSERVED] | PASS |
| D3 | 세계 상태를 바꾸는 확정이 이 동사에만 있다 | live `tools`에 `routing`이 붙은 비트 **3건**(`c5-b2` `c5-b4` `c7-b2`) [OBSERVED]. 그중 상태 전이는 `c5-b4` 확정 1건이고 `c5-b2`는 연습, `c7-b2`는 연습 층 재현이다. 다른 동사에 의한 상태 전이 0건 [INFERENCE 대조] | PASS |
| D4 | 선택 결과가 이진 플래그 1개를 넘지 않는다 | 1개 [OBSERVED campaign.meta.md §5.2] | PASS |
| D5 | 두 플래그 값 모두에서 필수 단서 집합·엔딩 3종 동일 | `systems/unity-implementation.md` T-14에 검증 항목으로 존재, **실행 0회** | NOT-MEASURED |
| D6 | 후일담 문단 2쌍이 실제로 집필되어 있다 | 구조만 존재 — `synopsis/scenes-and-dialogue.md`(e0 지문)와 `synopsis/campaign.md`가 "두 쌍 중 한 쌍"을 지시하나 **본문 2쌍은 미집필** [OBSERVED] | FIX → C4 synopsis |

### B — 빌드 후에만 측정 가능 (n=0)

| id | 기준 | 표본 |
|---|---|---|
| B1 | 확정 전 두 경로를 모두 연습한 비율 ≥ 70% [TARGET] | 12명/5유형 |
| B2 | "무엇을 잃었는지" 정확히 진술 ≥ 10/12명 | 동일 |
| B3 | 확정 후 되돌리기 시도 성공률 100% | 동일 |
| B4 | 이 동사로 인한 진행 불가 0/12 | 동일 |
| B5 | 확정을 실수로 눌렀다는 보고 0건 (`two-step` 확인 단계의 효능) | 동일 |

## telemetry_fields

| 필드 | 타입 | 출처 | 이 스펙에서의 용도 |
|---|---|---|---|
| `routing_edit_count` | int | systems drainage-routing | 연결·밸브 조작 수 |
| `routing_preview_count` | int | systems drainage-routing | 가상 시험 횟수 |
| `routing_violation_count` | int | systems drainage-routing | 위반 발생(유형별 분해) |
| `routing_commit_count` | int | systems drainage-routing | 확정 수 |
| `routing_checkpoint_reload` | int | systems drainage-routing | B3 확정 후 재선택 |
| `property_protection_choice` | enum(lowland, dock) | systems drainage-routing | 확정된 플래그 값 |
| `sandbox_time_min` / `commit_time_min` | float | 공통 키 | 연습/확정 시간 분리 |
| `corrosion_bypass_used` | int | systems corrosion-budget | E4 우회관 |
| `hint_used` | (level, beat_id, t) | systems hint-system | B4 |
| `routes_previewed` | int(0-2) | **[신규 제안]** | B1 두 경로 연습 비율 — `routing_preview_count`로는 "서로 다른 두 경로"를 셀 수 없다 |

> **명명 규칙.** 아래 키는 `systems/system-specs/*.md` §6이 이미 정의한 이름을 그대로 쓴다. 기획이 새로 요구하는 키만 **[신규 제안]**으로 표시하며, 채택 여부는 systems가 판정한다. 공통 키(`beat_reached`, `hint_used`, `sandbox_time_min`, `commit_time_min`, `afk_gap_sec`)의 정의는 `systems/ops/telemetry-contract.md`가 소유한다 — [OBSERVED 2026-09-10 재측정] **이 파일은 존재한다**(`ls systems/ops/` → `telemetry-contract.md`, cycle c3 · status current). 이전 판의 "아직 없다"는 스테일 주장이었다(C3-F15).
수집 원칙: 연습(sandbox) 층 로그는 **분석 전용**이며 게임 내 판정·엔딩·업적에 쓰지 않는다(GDD §3.3 원칙 4).
