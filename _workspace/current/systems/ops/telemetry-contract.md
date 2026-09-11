---
updated: 2026-09-10
cycle: 20260909-preproduction-c3
status: current
supersedes: null
owner: game-systems-designer
---

# 텔레메트리 계약 — C3 (키 정의 · 정직성 규율 · 로컬 전용)

이 계약의 목적은 **문서 상수와 실측치를 같은 이름으로 부르지 못하게 막는 것**이다.
현재 수집된 표본은 **n = 0**이다. 아래 `observed_*` 키는 전부 값이 비어 있다.

## 0. 전송·저장 정책

| 항목 | 값 |
|---|---|
| 원격 전송 | **없음**. 서버·클라우드·분석 SDK 0건 |
| 저장 위치 | 로컬 파일 `telemetry/session-{id}.jsonl` [TARGET] |
| 수집 동의 | 내부 플레이테스트에서만, 참가자에게 사전 고지 후 수동 수집 |
| 개인정보 | 이름·계정·IP·하드웨어 식별자 저장 금지. 세션 id는 임의 UUID |
| 정본 게임 빌드 | 텔레메트리 기본 **꺼짐**. 플레이테스트 빌드에서만 켠다 |

## 1. 시간 키 — 설계 상수 vs 실측 (C1 F4 대응)

**같은 축에 그리지 않는다. 같은 표에 넣을 때는 열 이름에 출처를 붙인다.**

| 키 | 성격 | 정의 | 현재 값 |
|---|---|---|---|
| `design_budget_min` | **문서 상수** | 기획 문서의 설계 분량 합계. `planning/campaign.json` `designMinutes` | 480 [TARGET] |
| `design_fast_min` | 문서 상수 | `fastMinutes` 합. **시나리오 경계이며 신뢰구간 아님** | **322** [TARGET] |
| `design_deliberate_min` | 문서 상수 | `deliberateMinutes` 합. 동일 | **673** [TARGET] |
| `observed_completion_min` | **실측** | 한 참가자의 본편 완주 분 | 없음 (n=0) |
| `observed_median_min` | 실측 | 참가자 완주 분의 중앙값 | 없음 (n=0) |
| `observed_p25_min` | 실측 | 하위 25% 분위 | 없음 (n=0) |
| `observed_p75_min` | 실측 | 상위 25% 분위. **보고는 하되 수용 판정 조건이 아니다**(RFC-P3-011로 p75 조건 삭제) | 없음 (n=0) |
| `observed_n` | 실측 | 완주 표본 수 (탈락 포함 보고) | 0 |
| `total_min` | 실측 | 세션 총 경과 분 (제외 없음) | 없음 (n=0) |
| `afk_total_min` | 실측 | `afk_gap` 구간 합 (§5) | 없음 (n=0) |
| `total_minus_afk_min` | **실측 · 판정 키** | `total_min − afk_total_min`. **수용 판정은 이 키 하나로만 한다**(RFC-P3-011) | 없음 (n=0) |
| `observed_dropout_n` | 실측 | 미완주 표본 수 | 0 |

설계 상수 재측정 [OBSERVED 2026-09-10] — 명령 `node _workspace/current/planning/validate-campaign.mjs`:
`designMinutes` 480 · `fastMinutesSum` **322** · `deliberateMinutesSum` **673** · 스테이지 분 25·50·55·65·65·70·75·65·10.
**R4 재실행 [OBSERVED 2026-09-10 R4]**: 같은 명령을 다시 돌려 위 다섯 상수가 **전부 불변**임을 확인했다(`totalMinutes` 480 · 322 · 673 · 같은 스테이지 분 · 단서 73). 대상 파일은 R3 의 `fdabf1d4…`(120,479 B, 44/44)에서 **`92301c0a…`(121,457 B, 47/47 PASS)** 로 바뀌었다 — planner 의 비트 `zoneId` 추가(C3-F22) 때문이며 시간 상수와는 무관하다. 해시는 고정 기재하지 않고 검증기 출력을 읽는다(RFC-Q1).
이전 판이 적었던 321 / 672는 존재하지 않는 해시(`2bfe4d52…`) 위의 값이었고 폐기한다(C3-F2).

### 1.1 금지된 표현

- `design_budget_min`을 "예상 플레이 시간"으로 부르지 않는다.
- `design_fast_min` ~ `design_deliberate_min` 을 "분포"·"IQR"·"신뢰구간"으로 부르지 않는다.
- 실측 표가 비어 있는 상태에서 "8시간 게임"을 관측 문장으로 쓰지 않는다.

### 1.2 수용 조건 — RFC-P3-011 정본

**판정 키는 `total_minus_afk_min` 하나다.** `total_min`과 `afk_total_min`을 병기하지 않은 보고는 무효다(§5 AF3·AF4).

| 항목 | 값 | 성격 |
|---|---|---|
| 판정 키 | `total_minus_afk_min` | 유일 |
| **목표 밴드** | 완주 **중앙값 450 ~ 540분** | [TARGET] |
| 대상 | 공략을 보지 않고 처음 플레이하는 일반 이용자의 본편 완주 | — |
| 표본 | 최소 12명 / 5유형, **탈락 포함 보고** | 미충족 시 판정 불가 |
| `p75` 조건 | **없음** | RFC-P3-011로 삭제 |

**철회 트리거(통과선이 아니다)**

| 조건 | 조치 |
|---|---|
| 중앙값 `total_minus_afk_min` < **420** | "8시간" 주장을 철회하거나 콘텐츠를 증설한다 |
| 하위 25%(`observed_p25_min`, 같은 키 기준) < **360** | 동일 |

420 / 360은 **통과선이 아니라 경보선**이다. 관측이 목표 밴드에 못 미친다고 해서 목표를 420으로 낮춰 자동 통과시키는 처리를 금지한다.

**`design_fast_min` 322 · `design_deliberate_min` 673은 표본 통계와 비교하지 않는다.** 시나리오 경계를 표본 봉투(하한 360 / 상한 600)와 대조하는 것은 범주 오류이며, 이 계약은 그 대조를 수행하지 않는다(RFC-P3-011 (4)). 따라서 "322 < 360이므로 하한 이탈"이라는 문장은 이 문서에서 성립하지 않는다.

본편 목표 시간에 **합산하지 않는 것**: 선택 콘텐츠, NG+, 수집 100%, 로딩, 일시정지, 자리비움, 재시작 대기.

**현재 상태**: `observed_n = 0`. 위 표의 어떤 칸도 관측으로 채워지지 않았다. 이 절이 존재한다는 사실이 G7을 PASS시키지 않는다.

## 2. 진행 키

| 키 | 타입 | 정의 |
|---|---|---|
| `beat_reached` | `{beat_id, t}` | 비트 최초 진입. `t`는 세션 시작 이후 초 |
| `beat_completed` | `{beat_id, t, duration_sec}` | `completion` 조건 충족 |
| `stage_completed` | `{stage_id, t, duration_sec}` | |
| `beat_revisit_count` | int | 같은 비트 재진입 |
| `puzzle_enter` / `first_valid_action` / `clue_seen` / `hypothesis_preview` / `confirm` / `exit` | timestamp | `balance/puzzle-balance.md`가 정의한 6구간 [OBSERVED] |
| `ending_reached` | enum | `full_restoration` \| `system_defect` \| `incomplete_acknowledged` |

## 3. 힌트 키

| 키 | 타입 | 정의 |
|---|---|---|
| `hint_used` | `{level, beat_id, t}` | `level ∈ {1,2,3}` |
| `hint_offer_shown` / `hint_offer_dismissed` | int | 무진전 제안 |
| `time_to_first_hint_sec` | float | 비트 진입 → 첫 힌트 |
| `stuck_after_l3` | int | **0이어야 하는 값.** 1건이라도 발생하면 G7 대체 검증(퍼즐 도달성) FAIL |

힌트 사용은 **점수·평가·엔딩과 연결되지 않는다**. 이 키는 설계 개선용이며 플레이어 등급이 아니다.

## 4. 연습 / 확정 / 되돌림 키

| 키 | 타입 | 정의 |
|---|---|---|
| `sandbox_time_min` | float | 연습 브랜치에서 보낸 시간 합 |
| `commit_time_min` | float | 확정 경로(프리뷰 열기 → 확정)에서 보낸 시간 합 |
| `sandbox_discarded` | int | 폐기된 연습 브랜치 수 |
| `undo_count` | int | `headSeq` 하향 횟수 |
| `redo_count` | int | |
| `branch_created` | int | 되돌림 후 재조작으로 생긴 분기 |
| `checkpoint_created` / `checkpoint_loaded` | int | |
| **`command_count`** | int | **확정된(`committed: true`) 명령 수** [C7-F11 신설 2026-09-10 R7]. `data-schemas/save.md` §3 `commandLog.entries` 에 실제로 append 된 항목 수와 같다. sandbox 명령은 **세지 않는다**(저장되지 않으므로). `undo_count` 로 되돌려도 항목은 지워지지 않으므로 이 값은 **감소하지 않는다** |
| **`entries_count`** | int | 세이브 파일 안 `commandLog.entries` 길이. `command_count` 와 같은 값이어야 하며 다르면 직렬화 결함이다. §6.1 `save_corruption_report` 가 같은 이름으로 이미 쓰고 있다 |
| **`tool_panel_active_min`** | float | **도구 패널이 열려 있고 그 세션에서 입력이 발생한 구간의 분 합** [C6-F5 신설 2026-09-10 R7 종료]. `handoff/verification-plan.md` **H-10 `manipulation_share`** 의 **분자**이며, 분모는 `total_min` 이다. **비율은 코드가 계산하지 않는다** — 두 원값만 기록하고 보고 시점에 나눈다(비율만 남기면 분모가 사라진다). 패널이 열려 있어도 60초 이상 무입력이면 그 구간은 `afk_gap` 후보이며 §5 규칙을 따른다(자동 제외 없음) |

`sandbox_time_min`과 `commit_time_min`은 **분리 보고한다**. 합쳐서 "퍼즐 시간"으로 보고하면 "연습이 길어서 8시간"인지 "확정이 어려워서 8시간"인지 구분이 사라진다.

**`tool_panel_active_min` 이 재려는 것** [C6-F5]: 위험 **R-T0-1** — 「조작 하위과제의 61%(21/33 비트)가 표·칸 조작이며 **손 조작 감은 미측정**」. 이 키는 그 위험을 **문서로 닫지 않고 재기 위한** 것이며 **판정선이 없다**. 어떤 값이 나와도 그 자체로 PASS/FAIL 이 아니고, 재설계 여부는 T0 결과를 본 뒤 디렉터가 판정한다. 짝이 되는 사람 지표(H-11 「손 조작을 재미로 꼽은 응답 수」)는 **텔레메트리가 아니라 자유 답변**이다 — 코드는 "재미"를 기록하지 않는다.

**`command_count` 의 첫 용도** [C7-F7]: `save_file_bytes / entries_count` 와 함께 **명령 엔트리 평균 바이트**를 재고, 그 값으로 `save.md` §3.2 의 `entryCap`(현재 20,000 [TARGET·INFERENCE])을 재파생한다. 현재 값 **없음(n=0)**.

## 4.1 도구 키 — 6종 도구가 발행하는 키 `[C7-F11 신설 2026-09-10 R7]`

**왜 이 절이 생겼는가**: `data-schemas/tools.md` `T-I6` 은 "`telemetryKeys` 의 모든 키가 텔레메트리 계약에 정의됨"을 **fail-closed 임포트 검사**로 세운다. 그런데 도구 키는 각 `system-specs/*.md` §6 에만 있었고 이 계약에는 **한 건도 정의돼 있지 않았다** [OBSERVED 2026-09-10: `grep -c` 로 7종 전건 0]. 그 상태로 임포트하면 **T0 도구 2종이 즉시 실패**한다. 아래 표가 그 공백을 메운다.

**소유 규칙**: 각 키의 *의미*는 해당 스펙 §6 이 소유하고, 이 절은 **계약 등재부**다. 스펙이 키를 추가하면 이 표에도 같은 편집에서 등재한다 — 등재되지 않은 키는 `T-I6` 이 막는다(그것이 이 검사의 목적이다).

| 도구 | 키 | 타입 | 소유 스펙 §6 |
|---|---|---|---|
| `circuit` | `circuit_open_count` · `circuit_trace_count` · `circuit_overlay_attempts` · `circuit_uncovered_marked` · `circuit_out_of_coverage_blocked` | int | `wiring-trace.md` |
| `circuit` | `circuit_time_min` | float | `wiring-trace.md` |
| `reader` | `read_count_total` · `read_budget_exhausted` · `auto_copy_created` · `citation_count` · `indeterminate_shown` · `alt_path_offered` | int | `plate-readout.md` |
| `reader` | `reader_time_min` | float | `plate-readout.md` |
| `alignment` | `align_attempts` · `align_indeterminate_count` · `align_auto_suggest_used` · `align_unlock_count` | int | `tide-alignment.md` |
| `alignment` | `align_residual_final` · `align_time_min` | float | `tide-alignment.md` |
| `alignment` | `align_residual_history` | float[] (표본 상한 200) | `tide-alignment.md` |
| `routing` | `routing_edit_count` · `routing_preview_count` · `routing_violation_count` · `routing_commit_count` · `routing_config_corrosion` · `routing_checkpoint_reload` | int | `drainage-routing.md` |
| `routing` | `property_protection_choice` | enum `lowland` \| `dock` | `drainage-routing.md` |
| `corrosion` | `corrosion_trial_count` · `corrosion_over_budget_count` · `corrosion_bypass_used` | int | `corrosion-budget.md` |
| `corrosion` | `corrosion_over_by_max` · `corrosion_final_ratio` · `corrosion_time_min` | float | `corrosion-budget.md` |
| `seal` | `seal_attempt_count` · `seal_commit_count` · `seal_unseal_count` · `perspective_switch_count` | int | `dual-seal.md` |
| `seal` | `seal_time_min` | float | `dual-seal.md` |
| `seal` | `seal_reject_reason` | enum[] `same_media` \| `out_of_coverage` \| `residual_exceeded` \| `witness_declined` | `dual-seal.md` |
| `seal` | `submission_perspective` | enum `full_restoration` \| `system_defect` \| `incomplete_acknowledged` | `dual-seal.md` (§2 `ending_reached` 와 **다른 키**) |
| `hint` | `hint_l1_rate` / `hint_l2_rate` / `hint_l3_rate` | float | `hint-system.md` (나머지 힌트 키는 §3) |
| `hint` | `afk_gap_sec` | float[] | `hint-system.md` — §5 `afk_gap` 이벤트의 **구간 길이 목록 집계본**이며 판정에 단독 사용 금지(AF4) |

**T0 에서 실제로 발행되는 것은 `circuit`·`reader` 두 벌뿐이다.** 나머지 4종은 스텁이므로 키가 **정의는 되어 있고 값은 발행되지 않는다** — 정의를 지우면 `T-I6` 이 6종 임포트를 막는다.

`*_time_min` 키는 전부 **`afk_gap` 규칙(§5)의 적용 대상**이며 제외 여부를 명시하지 않은 채 인용하면 계약 위반이다(AF4).

## 5. `afk_gap` 표기 규칙 (핵심)

| 규칙 | 내용 |
|---|---|
| AF1 | **60초 이상 유효 입력이 없는 구간**을 하나의 `afk_gap`으로 **기록(marking)**한다. 기록은 표시일 뿐 판정이 아니다 |
| AF2 | `afk_gap`은 **자동으로 제외하지 않는다**. 60초 무입력이라는 사실만으로 그 구간을 자리비움으로 확정하지 않는다 |
| AF3 | 보고는 항상 세 값을 함께 낸다: `total_min`, `afk_total_min`, `total_minus_afk_min` |
| **AF7** | **`total_minus_afk_min` 정의 (RFC-P3-011 판정 키)**: `total_min` − `afk_total_min`. 여기서 `afk_total_min`은 §5의 `afk_gap` 후보 구간 중 **회고(retrospective)로 자리비움이 확인된 구간만** 합산한다. 회고 = 세션 종료 후 참가자에게 각 후보 구간을 제시하고 "자리를 비웠는가 / 생각 중이었는가"를 확인하는 절차. **자동 제외는 없다** |
| **AF8** | 회고를 수행하지 않은 세션은 `afk_total_min`이 `null`이며, 그 세션의 `total_minus_afk_min`도 `null`이다. `null`인 세션은 수용 판정 표본에 넣지 않는다(탈락으로도 세지 않고 "미회고"로 별도 보고) |
| **AF9** | 회고 결과는 세션당 `afk_review: [{gap_index, verdict: away\|thinking\|unknown}]`으로 원본과 함께 보존한다. `unknown`은 **자리비움으로 치지 않는다**(보수적으로 시간에 남긴다) |
| AF4 | 제외한 수치를 단독으로 인용하는 것은 계약 위반이다. 제외 여부를 명시하지 않은 시간 수치는 무효 |
| AF5 | 생각 시간은 자리비움이 아니다. 마우스 이동·스크롤·패널 열기도 유효 입력으로 센다 |
| AF6 | 60초 임계는 [TARGET] 이며 실측 후 조정 대상. 조정하면 과거 데이터도 같은 임계로 재계산해 병기한다. 임계 조정은 **후보 구간의 수만 바꾸며 회고 판별을 대체하지 않는다** |

기록 형태:
```json
{"key":"afk_gap","start_t":1820.4,"duration_sec":312.0,"beat_id":"c3-b2","last_input":"mouse_move","verdict":null}
```
`verdict`는 수집 시점에 항상 `null`이며 회고(AF7) 후에만 `away` / `thinking` / `unknown` 중 하나로 채워진다. 코드가 이 값을 스스로 채우는 경로는 없다.

## 6. 세이브 손상·복구 로그

| 키 | 타입 | 정의 |
|---|---|---|
| `save_write_ms` / `save_load_ms` | float | |
| `save_recovery_path` | enum | `primary` \| `backup` \| `checkpoint` \| `panel` |
| `save_hash_mismatch` | int | **0이어야 하는 값** |
| `save_refused_version` | int | 상위 스키마 거부 |
| `save_write_failed` | `{reason, free_bytes}` | `disk_full` \| `permission` \| `io` |
| `save_file_bytes` | int | |
| `save_corruption_report` | object | §6.1 |

### 6.1 `save_corruption_report` 필수 필드

`{ slot, schema_version, file_bytes, checksum_expected, checksum_actual, recovery_path, last_command_id, last_beat_id, entries_count, app_version, os }`

- 손상 파일은 **삭제하지 않고** `slot{n}.corrupt-{timestamp}.json`으로 보존한다. 원인 분석 없이 지우면 같은 결함이 재발한다.
- `save_recovery_path != primary` 인 모든 건은 G6(운영 안정성) 입력이며 릴리스 준비도 문서에 건수를 그대로 적는다.

## 7. 성능 키

| 키 | 타입 | 정의 |
|---|---|---|
| `frame_time_ms_p50` / `p95` / `p99` | float | 구간별 |
| `frame_time_capture_scene` | string | 어느 씬·어느 도구 |
| `zone_load_ms` | float | |
| `view_node_transition_ms` | float | |
| `peak_memory_mb` | float | |
| `hw_profile_id` | string | 기준 PC 식별자. **미정이면 수치 무효** |

`hw_profile_id`가 비어 있는 성능 수치는 판정에 쓰지 않는다. 지금은 전부 비어 있다.

## 8. 스키마·버전

| 항목 | 값 |
|---|---|
| 라인 포맷 | JSONL. 1줄 = 1 이벤트 |
| 공통 필드 | `{key, t, session_id, build_id, schema_version}` |
| `schema_version` | 1 |
| 키 추가 | 추가는 자유, **개명·의미 변경은 버전 증가 필요** |
| 키 명명 | snake_case (직렬화 데이터 camelCase와 의도적으로 다름 — 로그 파이프라인 관례) |

## 9. 이 문서가 주장하지 않는 것

- 어떤 키도 값을 갖고 있지 않다. 표본 **n = 0**.
- 성능 목표는 `[TARGET]`이며 기준 하드웨어가 미정이라 통과/실패를 말할 수 없다.
- 이 계약이 존재한다는 사실은 G6를 PASS시키지 않는다. G6는 실측과 릴리스 준비도 문서를 함께 요구한다.
- §4.1 은 **키 이름을 등재했을 뿐 값을 만들지 않았다.** 도구 키 전건이 `n = 0` 이며, 어떤 도구도 아직 한 번도 실행되지 않았다.
