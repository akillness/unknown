---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-planner
---

# C1 첫 비트 — 순찰로의 두 분기

[CARRIED] T0 인수를 마친 플레이어가 당직일지와 계통판을 읽고, 두 분기가 같은 배전을 쓴다는 사실로 열람 경로를 정한다. 수문 열람과 문재화의 출처 표기 조건을 함께 책임지는 것이 이 비트의 플레이어 판타지다. 범위는 C1 「두 개의 필적」의 `c1-b1` 하나다.

[TARGET] 다음 수용 수치는 구현이 충족해야 할 조건이다.

```yaml
feature_id: c1-patrol-contract
beat_id: c1-b1
prerequisite: t0-b3 / T0 complete
checkpoint: cp-c1-b1
goal: 두 단서를 읽고 분기와 출처 표기 조건을 확정해 제3수문 열람을 얻는다.
player_fantasy: 기록과 계통을 직접 대조한 뒤 접근 권한의 조건을 자신의 일지에 남긴다.
proof_required: false
touched_lanes: [systems, synopsis, worldview, balance, economy, presentation, modeling, qa]
acceptance_criteria:
  observed_clues: 2
  branch_combinations: 4
  valid_combinations: 1
  invalid_combinations_with_reason: 3
  atomic_chapter_confirmations: 1
  accepted_atomic_effects: 5
  partial_applications: 0
  bypass_progress_grants: 0
  new_rewards: 0
  numeric_pressure_thresholds: 0
  new_story_reveals: 0
```

[JSON 계약](c1-patrol-contract.json)이 구현 필드와 표시 문자열의 단일 패킷이다. `sourceType + rootOriginId`는 출처의 동일성 보존에 쓰며, `proofRequired:false`인 이 비트에 새 증명 퍼즐이나 관측소 배정을 추가하지 않는다.

| 단서 | 원문 출처 | 기록 조건 |
|---|---|---|
| `c1-b1-c1` | `log / watchlog-bureau / copiedFrom:null` | 당직일지의 두 필적 관찰. 두 번째 필적의 작성자는 추론하지 않는다 |
| `c1-b1-c2` | `plate / brine-log-gate3 / copiedFrom:null` | 규정 하한 바로 위에서 진동하는 야간 압력 관찰. 하한 숫자는 만들지 않는다 |

## 분기와 확정

[INFERENCE · RFC-CX-004 director ACK · 2026-09-11] 최초 미리보기는 `lightingEnabled:true, readerEnabled:true`다. 같은 배전의 충돌을 보여 주고 조명 분기를 접도록 하는 구현 초기화 결정이다. 캐논의 「조명 분기를 접고 판독 분기를 살린다」에서 도출했으며, 새로 관측한 세계 상태라는 주장은 아니다. 기존 저장·후보 상태를 불러올 때 이 최초 값으로 덮어쓰지 않는다. 무료 우회관은 `false/true`의 안전한 미리보기로 복구한다.

[TARGET] 두 단서 관찰은 각각 한 번 이상 필요하다. 조명 OFF·판독 ON 한 조합만 유효하다. 판독 OFF인 두 조합은 판독 경로 부재를, 둘 다 ON인 조합은 같은 배전에서 조명 분기를 접어야 함을 문자로 표시한다. 색만으로 상태를 구별하지 않는다.

[RFC-CX-004 · director ACK] **명시적 장 진행 확정 한 건**이 저장 성공 후 아래 다섯 효과를 함께 적용한다.

1. 수락한 조명/판독 분기 상태.
2. 제3수문 열람 권한.
3. 계장 문재화의 「그 자료의 출처를 공식 기록에 남긴다」 조건과 두 단서의 정확한 출처 참조를 오늘 자 당직일지에 기록.
4. `c1-b1` 완료.
5. `cp-c1-b1` 체크포인트.

회로의 접기·펴기는 확정 없는 미리보기로 유지한다. 조건 확인도 후보 상태이며 일지를 쓰지 않는다. 두 단서 관찰 + 유효 조합 + 조건 확인 이후에만 프리뷰→기본 two-step(또는 기존 hold/confirm-dialog 설정)→확정 전 체크포인트→저장→적용 경로가 열린다. 이것은 `circuit`의 새 commit 명령이 아니라 비트의 장 진행 트랜잭션이다.

한 번으로 묶는 이유는 캐논 완료 술어가 열람 권한과 일지 조건을 함께 요구하기 때문이다. 둘을 별도 확정하면 권한만 열리거나 조건만 기록되는 미저작 중간 세계 상태가 생긴다. 저장 실패·취소·지연/중복 receipt에는 부분 적용 0건이어야 한다. Undo/Redo는 다섯 효과 전체를 같은 트랜잭션으로 다룬다.

## 실패와 복구

[CARRIED/TARGET] 접기·펴기 횟수 제한은 없고 비용·부식·시뮬레이션 시간 증가가 없다. 무료 우회관은 조명 OFF·판독 ON의 안전한 **미리보기**로 즉시 복구한다. 기존 관찰과 확정 상태를 보존하고, 누락 단서·조건 확인·일지 기록·열람 권한·완료를 대신 부여하지 않는다. 복구 뒤에도 동일한 명시적 저장 확정이 필요하다. 확정 뒤 다시 접어도 현재 세계는 새 저장 확정 또는 전체 Undo 전까지 바뀌지 않는다.

## 인수와 근거

JSON `acceptanceCriteria`의 C1-P01~P12가 최소 의미 있는 검사 집합이다. T0 진입 가드, 2개 출처, 4개 조합, 누락 준비 조건, 미리보기 무부작용, 5효과 원자 적용, 저장 실패/취소/지연/중복, 접기 왕복, 3개 잘못된 조합의 무료 복구, Undo/Redo, 저장 재개, localization/입력 동등성을 검사한다. 시간·재미·대표 하드웨어 성능은 이 계약의 관측치가 아니다.

텔레메트리는 JSON `telemetryFields`에 선언한 beat/clue/source, 분기 후보, 거부 사유, 조건 확인, commit 키/저장 결과, 접근/체크포인트/복구 횟수다. 신규 인물 대사·압력 수치·재화 보상 필드는 없다.

| 근거 | 가져온 규칙 |
|---|---|
| [campaign.json](campaign.json) C1/c1-b1 | 제목·진입·목표·단서 출처·행동·완료·회복·힌트·체크포인트 정본 |
| [chapter-beats.md](../synopsis/chapter-beats.md) 표 B c1-b1/B04 | 두 필적/압력 단서와 문재화의 출처 조건 |
| [wiring-trace.md](../systems/system-specs/wiring-trace.md) §2, W-R5/W-R6 | 회로 Resolved는 commit이 아님, 자유 토글, 도달 불가 상태 금지 |
| [interaction-rules.md](../systems/interaction-rules.md) §0-4/§0-10, §1-1 | 미리보기 선행·저장 성공 후 확정·기존 세 확정 모드 |
| [save-undo.md](../systems/system-specs/save-undo.md) SV-R8/SV-R12/SV-F8 | 확정 전 저장 경계, idempotency, 늦거나 중복된 결과 비적용 |
| [decision-log.md](../production/decision-log.md) RFC-CX-004 addendum | 원자 확정 및 진행을 부여하지 않는 무료 복구 최종 판정 |

## 정적 검증 영수증

[OBSERVED · 2026-09-11] Node.js 읽기 전용 인라인 검사 **27/27 PASS**. 원본 campaign 필드 일치, 단서 2개 출처, 조합 4개/유효 1개, 거부 사유, 저장/원자성 선언, 무료 복구 비부여, Undo 단위, localization 참조, 고유 수용 사례 12개, 신규 수치 미발명, 디렉터 ACK, sidecar SHA-256을 검사했다. 이 결과는 데이터 계약 검사이며 런타임의 12개 수용 사례가 실행됐다는 뜻은 아니다.

```yaml
static_verification:
  date: 2026-09-11
  executor: Codex Node.js read-only inline assertions
  checks: 27
  passed: 27
  failed: 0
  runtime_acceptance_cases_executed: 0
  trailing_whitespace_findings: 0
  conflict_marker_findings: 0
```
