---
updated: 2026-09-10
cycle: 20260909-preproduction-c3
status: current
supersedes: null
owner: game-planner
---

# verb-06 · 이중서명 (Dual Seal)

```yaml
verb_id: seal
name_ko: 이중서명
name_en: Dual Seal
law: "법6 원본 책임과 제출을 나눈다"
layer: sandbox + commit      # 연습=슬롯 시험과 사유 읽기 / 확정=검증자 확인 + 주인공 서명
introduced_at: c1-b3
unguided_reprise: c4-b3
required_beats: 7          # live campaign.json 재측정 (RFC-P3-008)
design_minutes_touched: 108  # 그 비트들의 분 합(중복 계상). 480의 분해가 아니다
goal: >
  "안다"와 "제출한다"를 분리한다. 플레이어가 진실을 알아도 매체 2종으로
  세우지 못하면 제출되지 않으며, 그 제한이 이 게임의 결말을 만든다.
player_fantasy: >
  결론 카드에 서로 다른 두 매체를 나란히 올리고, 슬롯이 초록으로 바뀌는 순간
  이 문장이 청문에서 살아남는다는 것을 아는 기록복원사.
touched_lanes: [systems, worldview, synopsis, presentation, qa]
```

> **`law:` 필드의 출처** [OBSERVED, RFC-P3-014]: 6법 호명 문구의 정본은 `worldview/worldview-bible.md` §3 표 하나뿐이며 위 값은 그 문구를 **문자 그대로 인용**한 것이다(2026-09-10 전건 대조, 6/6 일치). `worldview/consistency-audit.md` §4에 보존된 폐기 문구는 쓰지 않는다.
> **집계의 출처** [OBSERVED]: `required_beats`·`design_minutes_touched`는 live `planning/campaign.json`을 `node planning/validate-campaign.mjs`로 집계한 값이다(2026-09-10 R4 재실행 = sha256 `92301c0a…` · 121457 B · `zoneId` 추가분, 집계는 **불변**). 이후 인용은 고정 sha를 다시 적지 말고 **검증기 출력의 `sha256`을 쓴다**(RFC-Q1). 이전 판의 값은 아카이브 c3 계보였다(C3-F1 / RFC-P3-008).

## rules

| # | 규칙 | 근거 |
|---|---|---|
| R1 | 결론 카드의 근거 슬롯 2개를 채우고 검증자 확인 → 주인공 서명 순으로 확정한다 | `systems/interaction-rules.md` §2.6 |
| R2 | 슬롯마다 **매체 종류 뱃지**(염판/일지/대장)와 출처가 보인다 | systems §2.6 |
| R3 | 확정 조건 3중: **서로 다른 매체 2종** + 각 근거가 **배선 범위 안**(verb-01) + 시간 근거는 **잔차 ≤ 4분**(verb-03) | systems §2.6 |
| R4 | 같은 출처를 두 번 넣으면 두 번째 슬롯이 즉시 회색 + "독립 매체 2종 필요" | systems §3 |
| R5 | **과거의 이중서명**(12년 전 명령의 책임 요건)과 **현재의 제출 서명**(검증 절차)은 다른 것이다. 표기와 UI를 구분한다 | 법6 C3 개정, `worldview/glossary.md` |
| R6 | NPC가 증인을 거부해도 **대체 검증 절차가 열린다.** 진행을 막는 열쇠 NPC는 없다 | 법6 실패와 회복 |
| R7 | 세 제출 관점은 `c7-b4`에서 **한 번만** 세며 새 씬을 만들지 않는다. 공통 종결 씬 1개 + 기록 패널 3종 | `campaign.meta.md` §5.2 |
| R8 | 최종 제출 화면에서 관점을 **같은 회차 안에서** 바꿔 재확정할 수 있다. 재시작 요구 없음 | systems §0.6, §7 |

## edge_cases

| # | 상황 | 요구 동작 |
|---|---|---|
| E1 | 근거 1개만으로 서명 시도 | 서명 비활성 + 사유 문장. 넣은 근거는 유지 |
| E2 | 배선 밖 근거를 슬롯에 넣음 | 슬롯 비활성 + "배선 범위 밖" 배지(verb-01 R3) |
| E3 | 시간 근거의 잔차가 4분 초과 | 슬롯 비활성 + "정합 필요" + `verb-03`으로 가는 바로가기 |
| E4 | 증인 NPC(은정)가 조건부 거부 | 대체 검증 절차 표시. 결말 3종 접근성 불변 |
| E5 | 플레이어가 위조/단독 서명을 시도 | 서사적으로는 시도 가능하되 제출은 성립하지 않고 사유가 문장으로 남는다. **진행 차단·권한 박탈은 없다** |
| E6 | `e0-b2`(판 #0 처리)에서 서명 확정 | 봉인·제출·파기 3선택 모두 에필로그에서 명시적으로 서술된다 |
| E7 | 크레딧을 본 뒤 다른 관점을 확정하고 싶다 | 마지막 체크포인트로 복귀 가능. 세이브 새로 만들 필요 없음 |
| E8 | 저시력 사용자가 슬롯 상태(회색/활성)를 구분 못 함 | 상태를 문자 라벨로 병기하고 비활성 사유를 항상 문장으로 표시 |

## acceptance_criteria

### D — 문서 단계에서 지금 측정 가능

| id | 기준 | 현재 값 | 판정 |
|---|---|---|---|
| D1 | 필수 비트 ≥ 5건 | **7건 / 108분** [OBSERVED live 재측정] — 이전 판의 "6건"은 아카이브 c3 계보였다(C3-F19와 같은 뿌리) | PASS |
| D2 | 도입/미안내 재문제 | `c1-b3` / `c4-b3` [OBSERVED] | PASS |
| D3 | 확정 조건 3중이 세 문서에서 동일 서술 | planning·systems·worldview 일치 [OBSERVED 대조] | PASS |
| D4 | 결말 3종이 한 회차에서 도달 가능하다고 명시 | 명시됨 [OBSERVED systems §0.6] | PASS |
| D5 | 관점 3종이 새 씬을 만들지 않는다(저작량 폭발 방지) | 씬 1 + 패널 3 [OBSERVED synopsis/campaign.md] | PASS |
| D6 | 대체 검증 절차가 비트별로 열거되어 있다 | 시스템 규칙은 존재(systems `dual-seal.md` S-R4 "다른 증인 경로로 항상 우회 가능", S-F4 "대체 증인 목록 표시(존재 보증)") [OBSERVED]. **비트별 실제 증인 목록은 미열거** | FIX → C4 synopsis |
| D7 | 과거 서명 / 현재 서명의 UI 구분이 문서에 있다 | **미기술** — systems `dual-seal.md`는 현재 제출 절차만 상태기계로 다루고 12년 전 이중서명(책임 요건)의 표기 구분이 없다 [OBSERVED] | FIX → C4 systems |

### B — 빌드 후에만 측정 가능 (n=0)

| id | 기준 | 표본 |
|---|---|---|
| B1 | "왜 서명이 안 되는지"를 사유 문장만 보고 해소 ≥ 10/12명 | 12명/5유형 |
| B2 | 세 관점 중 2개 이상을 실제로 확인한 비율 ≥ 50% [TARGET] | 동일 |
| B3 | 관점 전환에 재시작이 필요하다고 오해한 비율 ≤ 10% [TARGET] | 동일 |
| B4 | 이 동사로 인한 진행 불가 0/12 | 동일 |
| B5 | `e0-b2` 판 #0 처리를 자기 선택으로 인식 ≥ 10/12명 | 동일 |

## telemetry_fields

| 필드 | 타입 | 출처 | 이 스펙에서의 용도 |
|---|---|---|---|
| `seal_attempt_count` | int | systems dual-seal | 확정 시도 |
| `seal_reject_reason` | enum[] (`same_media`, `out_of_coverage`, `residual_exceeded`, `witness_declined`) | systems dual-seal | B1 사유만으로 해소되는가 |
| `seal_commit_count` | int | systems dual-seal | 확정 수 |
| `seal_unseal_count` | int | systems dual-seal | 확정 취소 수 |
| `submission_perspective` | enum(`full_restoration`, `system_defect`, `incomplete_acknowledged`) | systems dual-seal | 확정한 제출 관점 |
| `perspective_switch_count` | int | systems dual-seal | B2·B3 관점 전환 |
| `seal_time_min` | float | systems dual-seal | 체류 시간 |
| `hint_used` | (level, beat_id, t) | systems hint-system | B4 |
| `alternate_procedure_used` | bool | **[신규 제안]** | E4 대체 검증 절차 실사용 |
| `plate_zero_disposition` | enum(seal, submit, destroy) | **[신규 제안]** | B5 판 #0 처리 인지 |

> **명명 규칙.** 아래 키는 `systems/system-specs/*.md` §6이 이미 정의한 이름을 그대로 쓴다. 기획이 새로 요구하는 키만 **[신규 제안]**으로 표시하며, 채택 여부는 systems가 판정한다. 공통 키(`beat_reached`, `hint_used`, `sandbox_time_min`, `commit_time_min`, `afk_gap_sec`)의 정의는 `systems/ops/telemetry-contract.md`가 소유한다 — [OBSERVED 2026-09-10 재측정] **이 파일은 존재한다**(`ls systems/ops/` → `telemetry-contract.md`, cycle c3 · status current). 이전 판의 "아직 없다"는 스테일 주장이었다(C3-F15).
수집 원칙: 연습(sandbox) 층 로그는 **분석 전용**이며 게임 내 판정·엔딩·업적에 쓰지 않는다(GDD §3.3 원칙 4).
