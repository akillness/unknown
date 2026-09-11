---
updated: 2026-09-10
cycle: 20260909-preproduction-c3
status: current
supersedes: null
owner: game-economy-designer
---

# 보상 밴드 (G3 소스)

## 0. 원칙

이 게임의 보상은 **숫자가 오르는 것이 아니라 할 수 있는 일이 늘어나는 것**이다. 지급 수단은 4채널뿐이고, 무작위·반복 채집·통화·스탯 상승은 0종이다. 아래 분류는 `planning/campaign.json`의 33개 비트 `consequence` 원문을 읽어 붙인 [INFERENCE]이며, JSON에는 아직 보상 필드가 없다(§5 RFC-E6).

**재도출 영수증 (2026-09-10) [OBSERVED]** — RFC-P3-008(계보 B 정본)에 따라 §1 예시 문구와 §4 집계를 **live 파일에서 전부 다시 계산**했다. 이전 판은 아카이브 c3 계보 문구를 일부 인용하고 있었다(C3-F16).

| 명령 | 결과 |
|---|---|
| `shasum -a 256 _workspace/current/planning/campaign.json` | `fdabf1d421932d679429870a06eeb31e9527562370c6a107aec77c2c884fb2b7` (120479 B) |
| `node _workspace/current/planning/validate-campaign.mjs` | 44/44 PASS · 스테이지 분 **25·50·55·65·65·70·75·65·10** · 비트 33 |
| 33비트 `consequence` 전문 재판독 | §4.1 채널 라벨 **재도출** — A 10 · L **28** · D 6 · Z **11** (이전 판 L 27 · Z 12) |

재도출로 바뀐 판정은 §4.2에 **여유 감소 2건**으로 표시했다. 통과 여부 자체는 9행 모두 유지된다.

**개정 (R4, 2026-09-10 · 같은 사이클 제자리 갱신 = RFC-Q2, `supersedes: null` 유지) [OBSERVED]**: §3 게이트 블록의 `corrosion:`에 `persisted: false`·`system_split: derived_display_only`를 추가하고, R2 필드명을 고정한 `plate_original_state:` 블록을 신설했다(C3-F27(c)·C3-F35). `bypassUsed` 인용을 `save.md:56` → **`:55`**로 정정했다. **밴드 값·판정은 한 줄도 바뀌지 않았다** — 추가된 것은 필드명·저장 여부 사실뿐이며 `measurement_status`는 `DESIGN_ONLY_NOT_MEASURED` 그대로다.

## 1. 허용 채널 4종 (allowlist)

| 코드 | 채널 | 무엇이 지급되는가 | 예 [OBSERVED 문구] |
|---|---|---|---|
| **A** | 접근 권한 | 구역·도구·절차 단계의 개방 | `c1-b1` "제3수문 구역이 열리고" [OBSERVED, live 1건] |
| **L** | 열람 경로 | 자료 사본·판독 지점·사건판 항목·상시 표시 | `c3-b2` "이후 모든 판독 화면에 오차띠가 함께 표시된다" [OBSERVED, live 1건] |
| **D** | 대화 분기 | NPC 조건·거래·경로의 변화 | `c3-b4` "수락·거절 어느 쪽이든 경로는 열리며 청문 대조표의 각주 한 줄만 달라진다" [OBSERVED, live `grep -cF` = 1] |
| **Z** | 공간 상태 변화 | 구역·작업대·표시등·씬 상태의 지속 변경 | `t0-b2` "벽 지도에 “기록 밖” 음영 3구획이 생기고" [OBSERVED, live 원문은 곧은따옴표가 아니라 “ ”] |

**체감 종류(4종)와의 대응**: 공간 = Z, 정보 = L, 관계 = D, 표현 = 플레이어 선택이 기록으로 남는 비트(A·Z 혼합). 채널은 지급 수단, 종류는 체감이다. 둘을 이중 계산하지 않는다.

## 2. 금지 목록 (0건이어야 하며 예외 없음)

| 금지 | 현재 | 검사 방법 |
|---|---|---|
| 무작위 드롭 | 0 | 퍼즐 그래프에 난수 없음 [OBSERVED] `unity-implementation` §5 "난수 없음" |
| 반복 채집·파밍 | 0 | 33비트 단일 임계경로, 반복 수집 비트 0 [OBSERVED] `campaign.meta` §5.7 |
| 통화·상점·거래 | 0 | `gdd.md` 제외 목록 |
| 수치 스탯 상승 | 0 | 캐릭터 능력치 개념 부재 |
| 확률 보상·상자 | 0 | 〃 |
| 시간 잠금·대기 보상 | 0 | 전역 실시간 타이머 없음 [OBSERVED] `interaction-rules` §0.1 |
| 힌트 사용에 대한 보상 차감 | 0 | "엔딩·보상·평가에 어떤 영향도 주지 않는다" [OBSERVED] `interaction-rules` §4 |
| 업적·평가 등급으로 인한 콘텐츠 잠금 | 0 | 세 결말 항상 접근 (INV2) |

## 3. G3 게이트 블록

```yaml
gate: G3
cycle: 20260909-preproduction-c3
owner: game-economy-designer
measurement_status: DESIGN_ONLY_NOT_MEASURED
# 왜 NOT_MEASURED 인가 (sink-source-ledger.md §6.2, C3-F15 재작성 결론):
#   B1 telemetry-contract.md 는 존재하지만 자원 키 0건 (grep -c "econ." -> 0)
#   B2 플레이 표본 n=0
#   B3 INV10/INV11 미구현
source_data:
  file: _workspace/current/planning/campaign.json
  sha256: fdabf1d421932d679429870a06eeb31e9527562370c6a107aec77c2c884fb2b7
  bytes: 120479
  validator: "node planning/validate-campaign.mjs -> 44/44 PASS"
  lineage: "RFC-P3-008 계보 B (live)"
  remeasured_utc_date: 2026-09-10

# ---- 표준 항목: 대상 부재로 N/A (누락이 아니라 비존재) ----
inflation:
  monthly_max_pct: null
  na_reason: "게임 내 통화·상점·거래 0종. 팽창할 값이 없다."
fairness:
  paid_free_winrate_delta_max_pp: null
  na_reason: "승패 없음(전투·PvP·랭킹 부재). 승률을 정의할 대상이 없다."
comeback:
  reversal_probability_max: null
  na_reason: "승패 역전 개념 부재."
steady:
  parity_sessions_band: null
  na_reason: "반복 세션·일일 보상·시즌 진행도 부재. 1회 완결 캠페인."

# ---- 대체 항목: 이 게임의 실질 G3 ----
reachability:
  blocked_by_resource_max: 0          # 자원 잔량 때문에 막힌 사건
  observed: null                      # 미측정
  enforced_by: "INV10 (요청 중, 미구현)"
practice:
  resource_cost: 0                    # 연습·프리뷰·되돌림 소모
  enforced_by: "INV11 (요청 중, 미구현)"
plate_original_state:                 # R2 — C3-F35 필드명 정본
  count_field: readCounts             # save.md:54  map<recordId,int> (누계, 개명 금지 목록 save.md:92)
  limit_field: readBudget             # plates.md:39 int, tunable: economy, 기본 3
  retired_name: plateOriginalWear     # 철회 — 지시어로 쓰지 않는다 (currency-map.md 4.2)
  blocking: false                     # plate-readout.md P-R9 · P-F1 (사본 경로 유지)
  effect_on_ending_or_score: none     # 에필로그 보존 등급 문장 1줄만
hint:
  price: 0
  uses_max: unlimited
  effect_on_ending_or_score: none
entitlement:
  dlc_free_delta_features: 0          # 도구6/결말3/힌트/접근성/저장/수정
  save_requires_dlc_flag: false
corrosion:
  model: "RFC-P3-009 정본 — 전역 상한 1개, 계통별 차단 한도 없음, 리셋 없음"
  limit: 9                            # [OBSERVED] systems/prototype/model.mjs:79
  normal_option_costs: [7, 8]         # lowland, dock  [OBSERVED] model.mjs:63-64
  blocked_option_costs: [12]          # dock-express (학습용 오답)
  safety_floor_rule: "limit >= max(normal_option_costs)"      # 9 >= 8  PASS (여유 1)
  recommended_rule: "limit >= max(normal_option_costs) + 1"   # 9 >= 9  PASS (여유 0)
  sink_events_per_campaign: 1         # c5-b4 만 (routing 3비트 중 c5-b2 가상운전·c7-b2 시연은 무소모)
  deduction_semantics: UNRESOLVED     # RFC-E8 (N-18): economy 문서는 "7|8 차감", systems C-R9 는 "잔량을 깎지 않는다".
                                      #   관측 결과는 동일 (게이트 = configCost <= 9 비교, C5 이후 routing 확정 0건).
                                      #   문언 통일은 디렉터 판정 대상 — 이 블록의 수치는 그때까지 불변
  budget_reset_events: 0              # RFC-P3-009: "장 경계 리셋" 표현 폐기 — 되돌릴 잔량이 없다
  bypass_scope: per_configuration     # corrosion-budget.md C-F2 · save.md:55 bypassUsed string[]
  persisted: false                    # C3-F27(c): 저장 필드 없음. save.md:94 operationalCorrosion 제거,
                                      #   상태는 save.md:57 committedRouting 에서 재계산 (save.md:102)
  system_split: derived_display_only  # 계통별 분해 = 저장 없는 파생 표시값 (currency-map.md 4.1),
                                      #   d_s = 계통 s 부품 비용 합, Sum(d_s) = configCost, 차단 비교에 미참여
reward_channels:
  allowlist: [access, lookup, dialogue, space]
  random_drop_count_max: 0
  repeat_gather_count_max: 0
  beats_without_reward_max: 0
  channels_per_chapter_min: 2
  consecutive_lookup_only_beats_max: 3      # 재도출 관측 = 3 (c3-b1..b3), 여유 0
  dialogue_channel_min_total: 5
  space_channel_min_total: 8
  access_channel_min_per_chapter: 1   # exempt: [T0, E0]  (사유 §4.2)
expression_rewards:
  first_allowed_stage: C5             # 선택이 기록으로 남는 보상은 후반부터
  count: 6
```

## 4. 자기점검 (33비트 전수, [INFERENCE] 분류)

### 4.1 채널 분포

**live 33비트 `consequence` 전문 재판독 결과 (2026-09-10 재도출).**

| 채널 | 비트 수 | 비율 | 비트 id |
|---|---:|---:|---|
| A 접근 권한 | 10 | 30.3% | c1-b1, c1-b4, c2-b1, c2-b4, c3-b4, c4-b4, c5-b2, c6-b4, c7-b1, c7-b3 |
| L 열람 경로 | **28** | 84.8% | t0-b1~b3, c1-b2~b4, c2-b1~b4, c3-b1~b4, c4-b1~b4, c5-b1~b3, c6-b1~b4, c7-b2, c7-b4, e0-b2 |
| D 대화 분기 | 6 | 18.2% | c1-b1, c1-b3, c2-b4, c3-b4, c4-b4, c7-b1 |
| Z 공간 상태 | **11** | 33.3% | t0-b1~b3, c2-b3, c4-b2, c5-b1, c5-b4, c6-b1, c7-b4, e0-b1, e0-b2 |

라벨 합 55 / 비트 33 = **비트당 평균 1.67채널**. 다중 라벨이므로 비율 합은 100%를 넘는다.

**이전 판과 달라진 라벨 2건 (근거 명시)**

| 비트 | 이전 | 재도출 | 이유 |
|---|---|---|---|
| `c6-b4` | A | **A + L** | "세 관점의 **제출 초안이 열리고** 청문 검증 절차가 예고된다" — 초안은 열람 가능한 자료다. 절차 개방(A)과 자료 개방(L)이 동시에 있다 |
| `c3-b2` | L + Z | **L만** | "이후 모든 판독 **화면**에 오차띠가 함께 표시된다" — 화면 상시 표시는 §1 정의상 L이다. 공간·작업대·표시등이 바뀌지 않으므로 Z 라벨은 근거가 없었다(`t0-b2`의 "벽 지도"와 다르다) |

라벨은 여전히 사람이 문장을 읽어 붙인 [INFERENCE]다. 두 건이 바뀌었다는 사실 자체가 §6 한계와 RFC-E6(데이터 필드화)의 근거다.

### 4.2 밴드 판정

| 밴드 | 기준 | 실제 (2026-09-10 재도출) | 판정 |
|---|---|---|---|
| 보상 없는 비트 | 0 | **0 / 33** | PASS |
| 장별 채널 종류 수 | ≥ 2 | T0 2 · C1 3 · C2 4 · **C3 3** · C4 4 · C5 3 · C6 3 · C7 4 · E0 2 | PASS (9/9) — C3는 4 → 3 |
| L 단독 연속 | ≤ 3 | 최대 **3** (`c3-b1`·`c3-b2`·`c3-b3`) | PASS **(여유 0)** — 이전 판 표기 2는 `c3-b2`의 Z 오라벨 때문 |
| D 총량 | ≥ 5 | **6** | PASS (여유 1) |
| Z 총량 | ≥ 8 | **11** | PASS (여유 3) |
| 장별 A ≥ 1 | C1~C7 | 7/7 장 충족 | PASS |
| A 면제 장 | T0, E0 | T0=허브 단독 튜토리얼, E0=권한이 아니라 결과를 다루는 장 | 사유 있는 면제 |
| 무작위·채집 | 0 | 0 | PASS |
| 표현 보상 첫 등장 | C5 이후 | c5-b4 · c6-b4 · c7-b3 · c7-b4 · e0-b1 · e0-b2 (6건) | PASS |

**여유 0이 된 행을 통과로 넘기지 않는다**: `c3-b1`→`c3-b2`→`c3-b3`는 65분 동안 열람 보상만 이어진다. 밴드 상한과 정확히 같으므로, `c3-b*` 중 한 비트에 Z나 D가 하나라도 빠지면 다음 회차에 FAIL이 된다. 판정은 PASS지만 **감시 항목**으로 §4.3에 올린다.

### 4.3 밴드는 통과했지만 남는 위험 [INFERENCE]

| 위험 | 근거 | 제안 |
|---|---|---|
| **C5·C6 연속 대화 보상 0** | D 채널이 C5·C6 두 장 연속으로 없다. 두 장의 설계 분량은 **70 + 75 = 145분** [OBSERVED, live 스테이지 분]. 이전 판의 "135분"은 아카이브 계보 값이었다 | 후반 인물 존재감 저하 위험. 최소 1비트에 D를 얹거나, 이것이 의도(고립된 밤의 심화)임을 synopsis가 명시 → RFC-E7 |
| **C3 L 단독 3연속 = 밴드 경계** | `c3-b1`·`c3-b2`·`c3-b3` 세 비트(합 **50분** [OBSERVED: 16+17+17], 65분 장의 대부분)가 전부 열람 전용 [재도출] | 여유 0. C3의 세 비트 중 하나에 공간 변화나 대화 분기를 붙이면 여유가 생긴다. 붙이지 않기로 한다면 그 선택을 문서에 남긴다 → RFC-E9 |
| **L 편중 84.8%** | 28/33이 열람 경로 보상 (이전 판 27/33) | 추리 게임의 성격상 자연스러우나, "읽을 것만 늘어난다"는 체감이 될 수 있다. Z(공간 변화)가 붙는 11비트가 리듬을 만드는지 슬라이스에서 확인 |
| **E0 채널 2종** | 마지막 **10분** [OBSERVED, live E0 = 10분]이 Z·L만. 이전 판의 "25분"은 아카이브 계보 값이었다 | 에필로그는 보상 지급보다 정산 구간이므로 의도로 본다. 밴드 최소 2종은 충족 |

## 5. 검증 자동화 요청

| ID | 대상 | 내용 |
|---|---|---|
| **RFC-E6** | planner (campaign.json 소유) | 비트마다 `rewardChannels: [A\|L\|D\|Z]` 필드를 추가해 §4 분류를 손 읽기가 아닌 데이터로 만든다. 추가되면 밴드 검사는 임포트 검증기에서 자동화된다 |
| **RFC-E7** | synopsis, planner | C5·C6(합 145분)의 D 채널 공백이 의도인지 확인 |
| **RFC-E9** | planner, presentation | `c3-b1`·`c3-b2`·`c3-b3`(합 50분: 16+17+17)가 L 단독 3연속으로 밴드 상한과 같다. 한 비트에 Z 또는 D를 붙이거나, 붙이지 않는 이유를 문서에 남긴다 |
| RFC-E4 재확인 | systems | INV10·INV11 없이는 `reachability.blocked_by_resource_max: 0`이 설계 주장에 머문다 |

## 6. 한계

- §4의 분류는 **문장을 읽고 붙인 [INFERENCE]**다. 다른 사람이 읽으면 라벨이 몇 개 달라질 수 있고, 그래서 RFC-E6이 필요하다. **이번 재도출에서 같은 사람이 같은 규칙으로 두 건(`c6-b4`·`c3-b2`)을 다르게 붙였다** — 이것이 가설이 아니라 관측이다.
- 밴드 표의 "여유"는 **문서 상수끼리의 거리**이지 플레이 체감의 여유가 아니다.
- 밴드 PASS는 **문서 게이트(D)**이며 플레이 게이트(G)가 아니다. 보상이 "충분히 자주 온다"는 것과 "충분히 기쁘다"는 다르며 후자는 n=0이다.
- 힌트 무료·되돌림 무료가 실제로 성취감을 해치지 않는지도 미측정이다.
