---
updated: 2026-09-10
cycle: 20260909-preproduction-c3
status: current
supersedes: null
owner: game-planner
---

# verb-05 · 부식 시험 (Corrosion Assay)

```yaml
verb_id: corrosion
name_ko: 부식 시험
name_en: Corrosion Assay
law: "법5 소금은 비용으로 보인다"
layer: sandbox-only          # 확정은 verb-04(routing)가 대신한다 — 부식 자체를 확정하지 않는다
introduced_at: c2-b2
unguided_reprise: c4-b2
required_beats: 3          # live campaign.json 재측정 (RFC-P3-008)
design_minutes_touched: 46  # 그 비트들의 분 합(중복 계상). 480의 분해가 아니다
goal: >
  비용을 자원 관리가 아니라 정보로 만든다. 부식은 모아 쓰는 재화도 소모되는 잔량도 아니라
  "이 구성안이 계통을 얼마나 갉는가"를 읽는 계기판이며, 동시에 한 구성안의 상한선이다.
player_fantasy: >
  구성안을 시험대에 올려놓고 어느 부품이 계통을 갉아먹는지 짚어낸 뒤,
  한도 안에서 성립하는 조합을 스스로 찾아내는 정비공.
touched_lanes: [systems, economy, balance, worldview, qa]
```

> **`law:` 필드의 출처** [OBSERVED, RFC-P3-014]: 6법 호명 문구의 정본은 `worldview/worldview-bible.md` §3 표 하나뿐이며 위 값은 그 문구를 **문자 그대로 인용**한 것이다(2026-09-10 전건 대조, 6/6 일치). `worldview/consistency-audit.md` §4에 보존된 폐기 문구는 쓰지 않는다.
> **집계의 출처** [OBSERVED]: `required_beats`·`design_minutes_touched`는 live `planning/campaign.json`을 `node planning/validate-campaign.mjs`로 집계한 값이다(2026-09-10 R4 재실행 = sha256 `92301c0a…` · 121457 B · `zoneId` 추가분, 집계는 **불변**). 이후 인용은 고정 sha를 다시 적지 말고 **검증기 출력의 `sha256`을 쓴다**(RFC-Q1). 이전 판의 값은 아카이브 c3 계보였다(C3-F1 / RFC-P3-008).

## rules

| # | 규칙 | 근거 |
|---|---|---|
| R1 | 구성안을 **가상 시험대**에 올려 실행한다. 실행은 무제한·무료이며 세계 상태를 바꾸지 않는다 | `systems/interaction-rules.md` §2.5 |
| R2 | **구성안의 총 부식 비용 / 전역 상한 9** 게이지를 보여주고, 초과 시 **초과분과 원인 부품 목록**을 문장으로 낸다. 선택지 비용 예: lowland 7 · dock 8 · dock-express 12 | systems §2.5, `economy/currency-map.md` §4.1, RFC-P3-009 |
| R3 | 부식은 **구매·채집·시간 회복 자원이 아니다.** 게임 내 상점·통화·에너지와 연결 금지 | `economy/resources-and-fairness.md`, `worldview/glossary.md` |
| R3a | **부식은 확정으로 소모되지 않는다.** `circuit`·`reader`·`seal` 확정은 부식을 차감하지 않으며 **누적 고갈·계통 영구 고장·연습 소모·장 경계 리셋은 존재하지 않는다.** 매 구성안이 상한 9와 독립적으로 비교된다 | **RFC-P3-009** (디렉터 판정) |
| R3b | 계통별 한도(brine_line 14 · power_bus 14 …)는 **정본이 아니다.** `economy/currency-map.md` §4.1 표의 조건부 [TARGET] 표기로만 남는다 | RFC-P3-009 |
| R4 | **비용 ≤ 9**일 때만 `routing` 확정이 열린다. 초과는 **확정 전에** 막히고 사유가 보인다 | 법5 |
| R5 | 막다른 구성은 **무료 우회관 1회**로 언제나 복구된다. **영구 도구 상실 없음** | 법5 개정(C3) |
| R6 | 부식 수치는 데이터 테이블에만 산다. 코드는 노브만 노출하고 튜닝을 하드코딩하지 않는다 | CLAUDE.md §9 |
| R7 | 부식 무늬는 동시에 **증거**다(`c2-b3` 정전 반증). 비용 판정과 판독 판정을 같은 화면에서 섞지 않는다 | `campaign.json` `c2-b3` |

## edge_cases

| # | 상황 | 요구 동작 |
|---|---|---|
| E1 | 플레이어가 부식을 아끼려고 시험 실행을 피한다 | 시험이 무료임을 도구 첫 진입 시 1회 명시. 이후 반복 안내 없음 |
| E2 | 비용이 정확히 9인 구성 | 통과(≤ 9). 경계는 정수 단위 비교 |
| E3 | 원인 부품이 5개 이상 | 상위 3개 + "외 N개" 접기. 전체 목록은 펼치기로 접근 가능 |
| E4 | 부식 게이지를 색으로만 읽는 사용자 | 수치·문자 라벨 병기(예: "비용 12 / 상한 9 · 초과 3") |
| E5 | 우회관을 이미 쓴 상태에서 또 막다른 구성 | 우회관은 **막힘 상황마다 1회** 제공된다(총 1회 소진형 아님). 진행 불가 상태를 만들지 않는다 |
| E6 | `c4-b2`(미안내 재문제)에서 부식 무늬를 판독 근거로 쓰려 한다 | 허용. 단 `verb-02` R6에 따라 염판 원본과 **같은 출처 1개**로 센다 |
| E7 | 부식 수치가 밸런스 조정으로 바뀐다 | 데이터 테이블 변경만으로 반영. 세이브 필드명 변경 금지 |

## acceptance_criteria

### D — 문서 단계에서 지금 측정 가능

| id | 기준 | 현재 값 | 판정 |
|---|---|---|---|
| D1 | 필수 비트 ≥ 3건 | **3건 / 46분** [OBSERVED live 재측정] — 하한 겨우 충족 | WARN (GDD §4.2 RFC-P3-002) |
| D2 | 도입/미안내 재문제 | `c2-b2` / `c4-b2` [OBSERVED] | PASS |
| D3 | 유료·채집·시간회복 자원 연결 0건 | 0건 [OBSERVED economy 문서 대조] | PASS |
| D4 | 영구 손실 문구 0건 | 0건 [OBSERVED — 법5 정본 + RFC-P3-009] | PASS |
| D7 | 누적 소모 모델의 잔재 0건 | 본 스펙 기준 0건 [OBSERVED]. `balance/balance-sheet.md` §4는 아직 6계통 누적 모델 → **balance 레인 재작성 대상**(RFC-P3-009) | FIX → balance |
| D5 | 부식 수치가 데이터 테이블로 분리 명시 | 명시됨 — 코드 리터럴 금지 [OBSERVED]. 참조 대상 `data-schemas/tools.md`는 [OBSERVED 2026-09-10 재측정] **존재한다**(6파일). 단 systems `corrosion-budget.md` C-R2의 `systemLimits`(계통별 한도)는 RFC-P3-009로 정본이 아니게 됐다 → systems 재작성 대상 | PASS(규칙·파일) / FIX(모델 문구) |
| D6 | 학습 후 공백 구간 | `c4-b2` 이후 재등장 0건 [OBSERVED] | WARN — 재도입 리마인더 필요 |

### B — 빌드 후에만 측정 가능 (n=0)

| id | 기준 | 표본 |
|---|---|---|
| B1 | 시험 실행 횟수 중앙값 ≥ 3회 (아끼지 않는가) [TARGET] | 12명/5유형 |
| B2 | "부식은 소모 자원이 아니다(모아 쓰지 않는다)"를 정확히 진술 ≥ 9/12명 | 동일 |
| B3 | 한도 초과 후 자력 해소율 ≥ 80% (힌트 3단계 없이) [TARGET, balance 승인 필요] | 동일 |
| B4 | 우회관 사용 후 진행 불가 0/12 | 동일 |

## telemetry_fields

| 필드 | 타입 | 출처 | 이 스펙에서의 용도 |
|---|---|---|---|
| `corrosion_trial_count` | int | systems corrosion-budget | B1 시험 실행 횟수 |
| `corrosion_over_budget_count` | int | systems corrosion-budget | 한도 초과 횟수 |
| `corrosion_over_by_max` | float | systems corrosion-budget | 최대 초과분 |
| `corrosion_final_ratio` | float | systems corrosion-budget | 확정 시 cost / 9 |
| `corrosion_bypass_used` | int | systems corrosion-budget | B4 우회관 |
| `corrosion_time_min` | float | systems corrosion-budget | 체류 시간 |
| `hint_used` | (level, beat_id, t) | systems hint-system | B3 자력 해소율(3단계 미사용 판정) |
| `corrosion_over_top_part` | string | **[신규 제안]** | 최다 원인 부품 id — 데이터 튜닝 입력 |

> **명명 규칙.** 아래 키는 `systems/system-specs/*.md` §6이 이미 정의한 이름을 그대로 쓴다. 기획이 새로 요구하는 키만 **[신규 제안]**으로 표시하며, 채택 여부는 systems가 판정한다. 공통 키(`beat_reached`, `hint_used`, `sandbox_time_min`, `commit_time_min`, `afk_gap_sec`)의 정의는 `systems/ops/telemetry-contract.md`가 소유한다 — [OBSERVED 2026-09-10 재측정] **이 파일은 존재한다**(`ls systems/ops/` → `telemetry-contract.md`, cycle c3 · status current). 이전 판의 "아직 없다"는 스테일 주장이었다(C3-F15).
수집 원칙: 연습(sandbox) 층 로그는 **분석 전용**이며 게임 내 판정·엔딩·업적에 쓰지 않는다(GDD §3.3 원칙 4).
