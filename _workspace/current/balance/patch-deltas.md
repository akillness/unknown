---
updated: 2026-09-10
cycle: 20260909-preproduction-c3
status: current
supersedes: null
owner: game-balance-designer
---

# 패치 델타 (밸런스 수치 변경 대장)

## 0. 이번 회차 판정: **델타 33건** (전부 RFC 강제 모델 정정) [OBSERVED]

| 항목 | 값 |
|---|---|
| 이번 사이클 변경(old → new) 건수 | **41** — 신설 8 · 값 변경 2 · 키 개명 5 · 폐기 18 (§2.1~2.4, 33행) **+ 2026-09-10 개정 8행(§2.5, C3-F31)** (§2 표 행 수와 일치) |
| 성격 | **밴드 재튜닝이 아니다.** 전부 디렉터 판정(RFC-P3-009 · RFC-P3-015)과 QA 결함(C3-F2 · F4 · F9 · F15 · F19 · F20)이 강제한 **모델 정정·입력 재집계**다 |
| 개정 이력 (RFC-Q2) | **2026-09-10** — 같은 `cycle` 값 안의 제자리 개정. 디렉터 판정 **C3-F31**로 난이도 지수 정의가 바뀌어 §2.5 8행을 추가했다. 아카이브·`supersedes` 대상 아님(RFC-Q2) |
| 이전 판정 철회 | 2026-09-09 판은 "델타 0건 · C3는 초기값 설정 회차"라고 적었다. 그 v0 표는 저장소에 존재하지 않는 해시(`2bfe4d52…`)로 재확인했다고 주장했으므로 **v0 자체가 무효**였다(§1) |
| 델타로 세지 않는 것 | 문서 서술 정리, 절 재배치, 근거 경로 갱신 |
| 유효한 델타 트리거 | (a) `qa/*-review.md`·`qa/defect-register.md` 결함 행, (b) 디렉터 RFC 판정, (c) 시뮬 결과가 밴드를 벗어남, (d) 라이브 텔레메트리. **"느낌상 강하다/약하다"는 트리거가 아니다** |
| 전/후 시뮬 | **0쌍** [OBSERVED]. 시뮬 하네스가 존재하지 않는다(`sim-results/README.md` §0). §3의 "재튜닝마다 전/후 시뮬 1쌍" 규칙은 **텔레메트리·시뮬 근거 재튜닝**에 걸리며, 이번 회차처럼 RFC가 모델 자체를 무효화한 경우에는 비교할 old 밴드가 성립하지 않는다. 그 대신 **이번 델타로 검증 상태가 오르지 않는다**는 것을 명시한다 — 신설된 밴드도 전부 미검증이다 |
| 재화 결합 | 부식 관련 8행은 `game-economy-designer` 소유 모델을 채택한 것이며, ack 대신 **디렉터 판정 RFC-P3-009**가 근거다(economy가 제안 → 디렉터가 정본 확정). 나머지는 보상 흐름 무관 `n/a` |

## 1. v0 베이스라인 재도출 (이전 v0는 무효) [OBSERVED]

[OBSERVED] 재측정 명령·결과는 `balance/balance-sheet.md` §0.1에 그대로 있다 — live `planning/campaign.json` 의 sha256·바이트·집계는 **`node _workspace/current/planning/validate-campaign.mjs` 출력만 인용한다**(RFC-Q1 · 디렉터 판정 RFC-B6, 고정 문자열 재기재 금지). 2026-09-10 재실행: exit 0 · `summary.verdict PASS` · 47검사 47 PASS.

**이전 v0가 무효인 이유**: 2026-09-09 판은 "C4 개정판(sha256 `2bfe4d52…`)으로 재집계해 확인했다 … 집계치(염판 22·정합비트 7·circuit 10·reader 8·seal 6·routing 3)가 C3판과 동일"이라고 적었다. 그 해시는 저장소 어디에도 없고(C3-F2), 실측 집계는 **염판 24 · reader 11 · seal 7 · 단서 73**이다(`balance-sheet.md` §0.1, 두 독립 명령 일치). 따라서 그 표의 유도값(29 / 15 / 45 / 계통별 한도)은 근거가 붕괴했다. 염판 수치는 디렉터 판정 RFC-B6 으로 **실측 24 채택**이 확정됐다(등록부 C3-F2 행의 21 은 QA 가 정정) → `balance-sheet.md` §10.2 Q7 은 닫혔다. §1의 어떤 v0 값도 염판 수를 입력으로 쓰지 않으므로 아래 표는 그대로다.

**새 v0 (C3 종료 시점, 당시 검증기 출력 기준 — 해시 문자열은 RFC-B6 에 따라 옮겨 적지 않는다)** — 이후 회차의 델타는 이 표의 값을 old로 삼는다.

| 키 | 네임스페이스 | v0 값 | 산출 근거 |
|---|---|---|---|
| `readBudget` | plates.json (schema) | 3 | 원본 직접 절차 실측 1(`c1-b2`) + 여유 2 · `data-schemas/plates.md` 기본값 |
| `autoCopyOnFirstRead` | plates.json | true | 법2 [CARRIED] |
| `indestructible` | plates.json | true | 매체 2경로 불변식 |
| `corrosionLimit` | routes.json (미생성) | 9 | `model.mjs:79` [OBSERVED] |
| `routeCosts.lowland` | routes.json | 7 | `model.mjs:63` [OBSERVED] |
| `routeCosts.dock` | routes.json | 8 | `model.mjs:64` [OBSERVED] |
| `routeCosts.dock-express` | routes.json | 12 | `model.mjs:65` [OBSERVED] |
| `safety_margin` | 문서 밴드 | 0 | 9 − (8+1) = 0, 경계 충족 |
| `systemLimits` | zones.json | null | 표시 전용 (economy §4.1 조건부 [TARGET]) |
| `stationErrorMinutes` | plates.json | 4.0 | 세계관 §2 [CARRIED] |
| `alignment_drift_range_min` | 문서 밴드 | 40 | 세계관 §2 [CARRIED] · `model.mjs` ±45와 불일치(미해소) |
| `alignment_candidate_slots` | 문서 밴드 | 21 | (40/4)×2+1 |
| `alignment_pass_residual_min` | 문서 밴드 | 4 | QA F3 수정 요구 |
| `alignment_nearmiss_residual_max_min` | 문서 밴드 | 8 | 총 오차폭 ±4 |
| `alignment_required_sources` | 문서 밴드 | 2 | timeline §3 |
| `hint_offer_idle_s` | hints.json (헤더 미정) | 180 | `hint-system.md` §2 상태기계 [OBSERVED] |
| `hint_offer_cooldown_s` | 동상 | 180 | `hint-system.md` §1 "재제안은 다음 3분 후" [OBSERVED] |
| `hint_offer_per_session_max` | 동상 | null | 근거 표본 n=0 → 값을 지어내지 않음 |
| `cost` / `achievementPenalty` | hints.json | 0 / 0 | 계약 Premium overrides · `data-schemas/hints.md` 고정값 |
| `difficulty_index_by_stage` | 문서 밴드 | 5,7,7,7,7,8,11,8,4 | 스테이지별 최대 단서+최대 도구+가설 수 |
| `difficulty_max_rise_per_stage` | 문서 밴드 | 2 | 급경사 금지 규칙(현재 **FIX 1건** — C5→C6 +3) |
| `safety_checkpoint_count` | 문서 밴드 | 33 | `campaign.json` 실측 [OBSERVED] |
| `safety_open_protected_routes_min` | 문서 밴드 | 2 | `lowland` 7 · `dock` 8 둘 다 ≤ 9 |
| `safety_progress_block_count_target` | 문서 밴드 | 0 | G7 대체 검사 |

**[2026-09-10 개정 주]** 위 v0 표의 `difficulty_index_by_stage`·`difficulty_max_rise_per_stage` 두 행에 붙은 괄호 상태 표기("현재 **FIX 1건** — C5→C6 +3")는 **§2.5로 대체됐다**. v0 값 자체는 다음 회차 델타의 `old`로 쓰이므로 지우지 않는다.

## 2. 이번 회차 델타 (old → new) [OBSERVED 변경 사실 · TARGET 값]

`evidence` 열의 경로는 전부 저장소 상대 경로다. `sim` 열이 `없음`인 행은 **밴드가 검증됐다는 뜻이 아니라 검증이 존재하지 않는다**는 뜻이다.

### 2.1 부식 예산 — RFC-P3-009 (C3-F4 · C3-F9)

| key | old | new | reason | trigger | evidence | sim | economy_ack |
|---|---|---|---|---|---|---|---|
| `corrosion_limit_brine_line` | 14 | **폐기** | 계통별 차단 한도가 데이터에 존재하지 않음 | C3-F4 / RFC-P3-009 | `production/decision-log.md#rfc-p3-009` | 없음 | RFC-P3-009 |
| `corrosion_limit_power_bus` | 14 | **폐기** | 〃 | 〃 | 〃 | 없음 | RFC-P3-009 |
| `corrosion_limit_reader_head` | 12 | **폐기** | 〃 | 〃 | 〃 | 없음 | RFC-P3-009 |
| `corrosion_limit_seal_press` | 9 | **폐기** | 〃 | 〃 | 〃 | 없음 | RFC-P3-009 |
| `corrosion_limit_gate_valve` | 9 | **폐기** | 〃 | 〃 | 〃 | 없음 | RFC-P3-009 |
| `corrosion_limit_pump_motor` | 9 | **폐기** | 〃 | 〃 | 〃 | 없음 | RFC-P3-009 |
| `corrosionLimit` | (없음) | **9** | 전역 단일 상한이 정본 | 〃 | `systems/prototype/model.mjs:79` | 없음 | RFC-P3-009 |
| `routeCosts` | (없음) | **{lowland 7, dock 8, dock-express 12}** | 부식이 걸리는 대상은 `routing` 구성안 | 〃 | `systems/prototype/model.mjs:63-65` | 없음 | RFC-P3-009 |
| `corrosion_safety_floor_ratio` | 0.20 | **폐기** | 소모가 없으므로 잔여율이 정의되지 않음 | 〃 | `balance/balance-sheet.md#11` | 없음 | RFC-P3-009 |
| `corrosion_rework_allowance_ratio` | 0.40 | **폐기** | 재작업 여유의 전제(확정별 소모)가 사라짐 | 〃 | 〃 | 없음 | RFC-P3-009 |
| `corrosion_floor_breach_mode` | `practice_confirm_no_cost` | **폐기** | 돌파할 하한 자체가 없음 | 〃 | 〃 | 없음 | RFC-P3-009 |
| `safety_margin` | (없음) | **0** | `9 ≥ max(7,8)+1` 경계 충족 — 여유 0을 숨기지 않음 | 〃 | `balance/balance-sheet.md#42` | 없음 | RFC-P3-009 |
| `budget_reset_on_stage` | false | **`resets_on_stage: false` (개념 미성립 주석 추가)** | 소모가 없어 리셋 개념이 성립하지 않음 | 〃 | 〃 | 없음 | RFC-P3-009 |
| `corrosion_narrative_pattern_writable` | false | **`narrative_pattern_writable: false`** (개명) | 키 네임스페이스 정리 | C3-F15 | `balance/balance-sheet.md#4` | 없음 | n/a |

### 2.2 원본 상태(법2) — RFC-P3-009 · 법2 정본

| key | old | new | reason | trigger | evidence | sim | economy_ack |
|---|---|---|---|---|---|---|---|
| `plate_replay_limit_per_plate` | 3 | **`readBudget: 3`** (개명, 값 유지) | 스키마 필드명과 일치 | C3-F15 | `systems/data-schemas/plates.md` | 없음 | n/a |
| `plate_replay_required_total` | 29 | **폐기** | 사본 판독 무제한이면 "필요 재생"이 예산이 아님 | RFC-P3-009 / 법2 | `worldview/worldview-bible.md` §3 법2 | 없음 | ack 필요 없음(비차단) |
| `plate_pool_total` | 15 | **폐기** | 위와 동일. 판 지급량은 자산 생산 문제이지 밴드가 아님 | 〃 | 〃 | 없음 | n/a |
| `plate_replay_capacity_total` | 45 | **폐기** | 〃 | 〃 | 〃 | 없음 | n/a |
| `plate_replay_headroom_min` | 1.5 | **폐기** | 비차단 카운터에는 여유율이 정의되지 않음 | 〃 | `economy/currency-map.md` §4.2 | 없음 | n/a |
| `plate_replay_copy_resolution_min` | 8 | **폐기** | "4회째 원판 붕괴 → 8분 강등" 모델 폐기 | 〃 | `balance/balance-sheet.md#11` | 없음 | n/a |
| `blocking` | (없음) | **false** | 옵션 C(차단형)는 법2 위반으로 거부됨 | 〃 | `economy/currency-map.md` §4.2 | 없음 | economy 채택안 그대로 |

### 2.3 힌트 — RFC-P3-015 (C3-F20)

| key | old | new | reason | trigger | evidence | sim | economy_ack |
|---|---|---|---|---|---|---|---|
| `hint_offer_t1_idle_s` | 180 | **`hint_offer_idle_s: 180`** (개명, 값 유지) | 자동 제안이 1종뿐이라 단계 접미사가 무의미 | C3-F20 / RFC-P3-015 | `systems/system-specs/hint-system.md` §2 | 없음 | n/a |
| `hint_offer_t2_idle_s` | 420 | **폐기** | 자동 승격 모델 폐기(단계는 플레이어가 연다) | 〃 | 〃 | 없음 | n/a |
| `hint_offer_t3_idle_s` | 900 | **폐기** | 〃 | 〃 | 〃 | 없음 | n/a |
| `hint_offer_t2_failed_confirms` | 2 | **폐기** | 〃 | 〃 | 〃 | 없음 | n/a |
| `hint_offer_t3_failed_confirms` | 4 | **폐기** | 〃 | 〃 | 〃 | 없음 | n/a |
| `hint_t3_requires_confirmation_click` | true | **`warnsBeforeReveal: true`** (수동 3단 열람 경고로 의미 이동) | 자동 승격이 없으므로 "확인 클릭"의 위치가 바뀜 | 〃 | `systems/data-schemas/hints.md` | 없음 | n/a |
| `hint_offer_cooldown_s` | (없음) | **180** | 무시 시 재제안 간격 명시 | 〃 | `hint-system.md` §1·H-F5 | 없음 | n/a |
| `hint_offer_per_session_max` | (없음) | **null** | H-F5가 요구하나 근거 n=0 | 〃 | `hint-system.md` H-F5 | 없음 | n/a |

### 2.4 집계 재도출 — RFC-P3-008 (C3-F2 · C3-F19)

| key | old | new | reason | trigger | evidence | sim | economy_ack |
|---|---|---|---|---|---|---|---|
| `difficulty_index_by_stage` | [5,5,7,7,7,8,10,9,4] | **[5,7,7,7,7,8,11,8,4]** | 폐기 해시 집계 → live `fdabf1d4…` 재집계 | C3-F2 / RFC-P3-008 | `balance/balance-sheet.md#7` | 없음 | n/a |
| `difficulty_rise_rule_status` | (없음) | **FIX** | 재도출 결과 C5→C6 = +3으로 규칙(≤2) 위반 | 〃 | `balance/balance-sheet.md#71` | 없음 | n/a |
| `safety_open_protected_routes_min` | (없음) | **2** | 법4 두 보호 선택이 모두 예산 안에 있어야 함 | RFC-P3-009 | `systems/prototype/model.mjs:63-65` | 없음 | RFC-P3-009 |
| `source_json_sha256` | `2bfe4d52…`(부재) | **`fdabf1d4…`(120479 B)** | 실측 재기재, 축약 대신 전체 해시 | C3-F2 | `planning/campaign.meta.md` §1 | 없음 | n/a |

### 2.5 난이도 지수 재정의 — C3-F31 디렉터 판정 [2026-09-10 개정]

트리거는 QA 결함 `qa/defect-register.md` C3-F31(S2, open-rfc)과 그에 대한 디렉터 판정 `production/decision-log.md` "C3 종료 판정 묶음 · C3-F31"이다. **밴드 완화가 아니라 지수 정의에서 미측정 열을 제거한 것**이며, 그 결과로 상승 폭 판정의 부호가 바뀌었다.

| key | old | new | reason | trigger | evidence | sim | economy_ack |
|---|---|---|---|---|---|---|---|
| `difficulty_index_formula` | `max_clues + max_tools + concurrent_hypotheses` | **`max_clues + max_tools`** | 세 번째 열이 [INFERENCE]이고 재현 명령이 없어 게이트 판정에 쓸 수 없다 | C3-F31 / decision-log | `balance/balance-sheet.md#7` | 없음 | n/a |
| `difficulty_index_by_stage` | [5,7,7,7,7,8,11,8,4] | **[4,5,5,4,4,5,7,4,3]** | 위 정의 변경 후 live 재집계 | 〃 | `balance-sheet.md#7` · `planning/validate-campaign.mjs` | 없음 | n/a |
| `difficulty_observed_max_rise` | 3 | **2** | C5→C6 재계산(경계값, 여유 0) | 〃 | `balance-sheet.md#71` | 없음 | n/a |
| `difficulty_rise_rule_status` | FIX | **PASS** | 규칙(≤2)을 넓히지 않고 지수 입력에서 미측정 열을 뺀 결과 [OBSERVED] | 〃 | `balance-sheet.md#71` | 없음 | n/a |
| `difficulty_observed_drop_segments` | 2 | **3** | 2열 지수에서 C2→C3이 평탄(0)에서 −1로 바뀜 | 〃 | `balance-sheet.md#71` | 없음 | n/a |
| `difficulty_drop_rule_status` | (없음) | **RFC** | 임계 "≤2개"가 폐기된 3열 지수 기준이라 재보정 안 됨 → RFC-B5 디렉터 판정 대기. **임계 완화 미적용** | 〃 | `balance-sheet.md#71` | 없음 | n/a |
| `difficulty_hypotheses_in_index` | (암묵 true) | **false** | `동시 가설 수`는 §7.2에 [TARGET]으로 분리 보관, 게이트 판정 미사용 | 〃 | `balance-sheet.md#72` | 없음 | n/a |
| `source_json_sha256` | `fdabf1d4…`(120479 B) | **`92301c0a…`(121457 B)** | RFC-Q1대로 검증기 출력 sha 재인용. 밸런스 입력 집계는 전건 불변 [OBSERVED] | C3-F31 / RFC-Q1 | `validate-campaign.mjs` 출력 47/47 PASS | 없음 | n/a |

**이 8행이 올리지 않는 것** [OBSERVED]: 시뮬 0회·플레이 n=0은 그대로다. 상승 폭 규칙 통과는 **문서 게이트 안의 산술**이며 체감 난도와 무관하다. 하락 축은 아직 판정되지 않았으므로 §7은 전건 PASS가 아니다.

**밴드 검증 상태**: 위 33행 중 시뮬·플레이로 검증된 것은 **0건**이다. 산술로 확인한 것은 §2.1의 `safety_margin`(9 − 9 = 0)과 §2.4의 `difficulty_index_by_stage` 재집계뿐이며, 둘 다 문서 게이트 수준이다.

## 3. 델타 기록 포맷 (다음 회차부터 강제)

한 행 = 한 수치. 근거 경로가 비면 그 행은 무효다.

| 필드 | 규칙 |
|---|---|
| `key` | 키 이름. **네임스페이스를 밝힌다** — 스키마 직렬화 필드는 camelCase(`readBudget`), 문서 밴드·텔레메트리는 snake_case(`hint_offer_idle_s`). `balance-sheet.md` §2 규약 |
| `old` → `new` | 값만. 범위 변경은 `[a,b]` → `[c,d]` |
| `reason` | 한 문장. 형용사 금지, 관측 문장만 |
| `trigger` | QA 결함 id / 시뮬 파일 경로 / 텔레메트리 지표명 중 하나 |
| `evidence` | 저장소 상대 경로 (`balance/sim-results/…` 또는 `qa/…`) |
| `before_after_sim` | **시뮬·텔레메트리 근거 재튜닝**마다 전/후 시뮬 1쌍 필수. 없으면 변경 보류. RFC가 모델 자체를 무효화한 정정(§2)은 비교할 old 밴드가 없으므로 예외이며, 그 경우 `sim: 없음`을 적고 **검증 상태가 오르지 않았음**을 함께 적는다 |
| `economy_ack` | 보상 흐름 관련이면 economy 레인 ack 링크, 아니면 `n/a` |
| `cycle` | 변경이 실린 사이클 id |

예시(가상, 실제 변경 아님):

| key | old | new | reason | trigger | evidence | before_after_sim | economy_ack | cycle |
|---|---|---|---|---|---|---|---|---|
| `hint_offer_idle_s` | 180 | 240 | 자동 제안 수락률이 4%로 밴드(≥5%) 미만 | telemetry:`hint_offer_accept_rate` | `balance/sim-results/…` | 필요 | n/a | (미래) |

## 4. 이번 회차에 하지 않은 것 [OBSERVED]

- 다른 레인 파일(`planning/`, `systems/`, `economy/`, `qa/`, `production/`) 편집 0건.
- 아카이브 파일 수정·이동·삭제 0건.
- 시뮬레이션 실행 0건 → §1 v0와 §2 델타의 어떤 값도 시뮬로 검증되지 않았다.
- 밴드 완화 0건 — §2.4의 난이도 상승 규칙 위반(C5→C6 +3)은 규칙을 넓히지 않고 FIX로 열어 두었고, **[2026-09-10 개정] 그 FIX는 규칙 완화가 아니라 지수에서 미측정 열을 뺀 디렉터 판정(C3-F31)으로 닫혔다**(§2.5). 하락 구간 임계도 넓히지 않고 RFC-B5로 올렸다 — 임계 변경 0건.
- git commit / push 0건.

---

## 5. C6 회차 변경 (2026-09-10) — 용어 정정 1건 [OBSERVED]

수치 변경 0건. 아래는 **표현 정정**이며 어떤 밴드·임계·데이터 값도 바뀌지 않았다.

| key | old | new | reason | trigger | evidence | before_after_sim | economy_ack | cycle |
|---|---|---|---|---|---|---|---|---|
| `safety_two_disjoint_media_paths`(설명 문구) | "루트 출처가 겹치지 않는 **매체**+**경로**(폐기 용어, 자기 오탐을 피해 조각으로 적음) ≥ 2, 그중 1개는 파괴 불가" | "**`sourceType` 상이 AND `originId` 상이**를 만족하는 자료쌍이 최소 1개 존재하고, 그 쌍의 두 경로 중 1개는 어떤 플레이어 행동으로도 파괴되지 않는다" | systems 가 폐기 선언한 용어를 정본 표현으로 교체 (`unity-implementation.md` L69 불변식 1) | qa:C5-F5 (balance 배정 1곳) | `_workspace/current/balance/balance-sheet.md` §8 · `_workspace/current/qa/c5-review.md` L418 | 없음 — 문구 정정이므로 검증 상태는 오르지 않았다 | n/a | 20260909-preproduction-c6 |

- 영수증: 폐기 용어(「매체」+공백+「경로」)를 `grep -rn` 으로 `_workspace/current/balance` 전체에서 찾으면 **0건** [OBSERVED 2026-09-10]. 이 문서도 자기 오탐을 피해 그 문자열을 통째로 적지 않는다.
- **규칙 키 이름은 바꾸지 않았다.** `safety_two_disjoint_media_paths` 는 게이트 키(데이터 식별자)이고, 이름 변경은 이 키를 인용하는 문서·게이트 스크립트를 동시에 여는 변경이라 **RFC-B7** 로 올린다(아래 §6, id 정정 사유 포함). 문구는 이미 정본이므로 키 이름은 스테일이 아니라 **미개명**이다.
- `puzzle-balance.md` 는 이번 회차에 **승격하지 않았다** — 근거는 §6.

## 6. C6 미해결 · 판정 요청 [OBSERVED]

- **RFC-B7 (balance → systems/qa/director)** — *id 정정 [2026-09-10]: 원래 `RFC-B6` 으로 올렸으나 디렉터가 같은 번호를 **밸런스 문서의 sha 문자열** 판정에 썼다(`production/decision-log.md` "RFC-B6 · 밸런스 문서의 sha 문자열"). 그 판정은 아래 키 개명 질문에 답하지 않는다 → 충돌 회피를 위해 **RFC-B7** 로 재번호한다. 원문 보존.*: 규칙 키 `safety_two_disjoint_media_paths` 의 `media_paths` 부분이 폐기 용어(「매체」+「경로」)의 잔재다. 제안: `safety_two_disjoint_proof_sources` 로 개명. 질문 — 개명할 것인가, 아니면 키는 식별자로 동결하고 설명만 정본으로 유지할 것인가. 증거: `qa/c5-review.md` L418, `systems/unity-implementation.md` L69.
- **`puzzle-balance.md` status 유지(draft)**: `qa/c4-review.md` L326·L348 및 L513 이 이 파일을 **"차단(의도)"** 로 판정했다 — "문서 스스로 '정본이 아니다'를 선언했다. 승격하면 `balance-sheet.md` 의 소유와 충돌한다 — draft 유지가 맞다", R5 에서도 "차단 유지 · 사유 불변". `defect-register.md` L238·L247 도 같다. 즉 R4 QA 결과는 **지적 0건이지만 승격 차단이 명시적 의도**이므로, 세션 지시의 "지적이 없으면 승격" 조건이 성립하지 않는다고 판단해 `status: draft` 를 유지했다. 승격을 원하면 디렉터가 QA 의 차단 판정을 뒤집는 RFC 가 필요하다.

## 7. R7 회차 변경 (2026-09-10) — 수치 델타 **0건** [OBSERVED]

이번 회차(R7 종료 수정)에 밸런스 레인이 한 일은 **인용 형식 교체와 위험 등록**뿐이다. 밴드·임계·상한·지수 중 **어떤 값도 바뀌지 않았다**.

| key | old | new | reason | trigger | evidence | before_after_sim | economy_ack | cycle |
|---|---|---|---|---|---|---|---|---|
| *(없음 — 수치 델타 0건)* | — | — | — | — | — | — | — | 20260909-preproduction-c6 |

델타로 세지 않은 변경(§0 "델타로 세지 않는 것" 규칙 적용):

| 변경 | 내용 | 근거 |
|---|---|---|
| 인용 형식 | `balance-sheet.md` §0 정본 행 · §0.1 · §3.2 · §4.3 의 고정 sha/바이트 문자열 → **검증기 출력 인용**. 본 문서 §1 도 동일 처리 | 디렉터 판정 RFC-B6 |
| 집계 재확인 | 검증기 재실행으로 단서 73 · 염판 24 · 도구 10·11·8·3·3·7 · 비트 33 **전건 재현**(47검사 47 PASS). 값 변화 0 | `balance-sheet.md` §0.1 재실행 고지 |
| 위험 등록 | `balance-sheet.md` §10.4 **R-T0-1** 신설(표·칸 21/33 = 63.6%, `manipulation_share` 관측 지표 정의). **밴드 미정의(null)** — 값을 지어내지 않았다 | 디렉터 판정 C6-F5("문서로 닫지 않는다") |
| RFC id 정정 | 본 문서 §6 의 키 개명 요청 `RFC-B6` → **`RFC-B7`** (디렉터가 같은 번호를 다른 판정에 사용) | `production/decision-log.md` RFC-B6 |

- **전/후 시뮬**: 0쌍 [OBSERVED]. 바뀐 수치가 없으므로 비교 대상이 없다. 시뮬 하네스는 여전히 부재(`sim-results/README.md` §0).
- **재화 결합**: `n/a` — 보상 흐름에 닿는 값이 없다.
