---
updated: 2026-09-10
cycle: 20260909-preproduction-c3
status: current
supersedes: null
owner: game-systems-designer
---

# 시스템 스펙 — dual-seal (도구 `seal`, 법6 "원본 책임과 제출을 나눈다")

> **법 호명 정본** [OBSERVED · RFC-P3-014 · RFC-W3]: 6법의 호명 문구는 `worldview/worldview-bible.md` §3 표(= 아카이브 c3 세션 P 원문) 하나뿐이다. 이 스펙은 인용만 하고 다시 쓰지 않는다. 폐기된 C2 호명 문구는 `worldview/consistency-audit.md` 의 **「사용 금지 문구」 절**(그 안의 표 **「폐기된 6법 호명 문구」** — RFC-W3 이 "사용 금지 문구 원장"이라 부르는 절)에 보존돼 있다. **절 번호로 인용하지 않는다** — 감사 문서의 절 번호가 바뀌어도 이 인용은 살아 있어야 한다(RFC-W3, `qa/c3-review.md` §9.3 q-4 의 QA 대안).

전부 `[TARGET]`. 이 도구가 게임의 **유일한 결론 확정 지점**이다. 다른 다섯 도구는 여기에 근거를 공급한다.

| 항목 | 값 |
|---|---|
| 도구 id | `seal` |
| 도입 비트 / 미안내 재문제 | `c1-b3` / `c4-b3` [OBSERVED] |
| 등장 비트 수 | **7 / 33** [OBSERVED 2026-09-10 재측정] — `c1-b3` `c4-b3` `c6-b3` `c6-b4` `c7-b3` `c7-b4` `e0-b2`. 이전 판의 6은 폐기(C3-F2·C3-F19) |
| 최종 사용 | `c7-b4` 제출(관점 3종을 **한 번만** 센다) [OBSERVED: campaign.meta.md §5] |

## 1. 입력

| 입력 | KB/마우스 | 패드 | 결과 |
|---|---|---|---|
| 결론 카드 선택 | 클릭 | `A` | `conclusionId` |
| 근거 슬롯 채우기 | 증거함 → 슬롯 ×2 | `A` | `SetEvidence(slot, clueId)` |
| 증인 선택 | 목록 선택 | `A` | `SetWitness(personId)` |
| 프리뷰 | `Space` | `X` | 성립 여부 + 사유 |
| 서명(확정) | 확정 버튼 초점 후 `Enter` (기본 `two-step`) | 초점 후 `A` | `CommitSeal`. **길게 누름은 `hold` opt-in 에서만**이며 그때 유지 시간은 데이터 노브 `commitHoldSeconds`(기본 0.4 s · 범위 0.2~1.5 s [tunable: balance])다 `[C4-F20 정정 2026-09-10 R7 종료]` |
| 해제 | `Ctrl+Z` | `LB+X` | 확정 취소, 무제한 |

## 1-A. 키보드 파생 `[C4-F21]`

**신설 2026-09-10 R6 (C6/C7).** §1 표에서 KB/마우스 열이 포인터 조작만 적은 행의 키보드 단독 경로다. 규칙 id 는 `systems/interaction-rules.md` §1-3.4(D-1~D-7)를 인용한다. §0-8(키보드 단독 완결)이 전역 보장이고, 이 표는 그 보장이 **행 단위로** 어떻게 성립하는지를 적는다.

| 위 표의 행 | 파생 규칙 | 키보드 단독 경로 |
|---|---|---|
| 결론 카드 선택 (클릭) | **D-1** | 카드 목록에서 방향키 초점 후 `Enter` |
| 근거 슬롯 채우기 (증거함 → 슬롯 ×2) | **D-1** + **D-7** | `I`로 증거함을 열고 항목 초점 후 `Enter`로 집기 → 슬롯에 초점 후 `Enter`로 놓기. 슬롯 2개를 순서대로 반복 |
| 증인 선택 (목록 선택) | **D-1** | 목록 초점 후 `Enter` |
| 프리뷰 · 서명(확정) · 해제 | — | 이미 키보드 경로다(`Space` · `Enter` · `Ctrl+Z`) |

- 「서명(확정)」 행의 지속시간 표기는 **C4-F20 으로 해소됐다**(2026-09-10 R7 종료): 확정 기본값은 `two-step` 이고 0.4 s 는 홀드를 켠 플레이어에게만 적용되는 **데이터 노브**다. 이 정정으로 §5 「길게 누름은 필수가 아니다」와의 **같은 파일 자기모순**이 닫힌다. `interaction-rules.md` §0-9·§0-11·§1-1 이 정본이다.

## 2. 상태기계

상태 변수: `conclusionId`, `evidence[2]`, `witnessId`, `validity`, `sealed`.

| 현재 | 이벤트 | 다음 | 부작용 |
|---|---|---|---|
| `Idle` | `OpenConclusion(id)` | `Composing` | 빈 슬롯 2개, 매체 뱃지 표시 |
| `Composing` | `SetEvidence(slot, clue)` | `Composing` | 슬롯 채움, 즉시 검증 |
| `Composing` | 같은 `sourceType` 두 번째 | `Composing` | 두 번째 슬롯 회색 + "독립 매체 2종 필요" |
| `Composing` | 매체 2종 + 배선 안 + 잔차 ≤4분 | `Sealable` | 증인 선택 활성 |
| `Sealable` | `SetWitness` | `Sealable` | 증인의 조건부 승낙 규칙 평가 |
| `Sealable` | `CommitSeal` | `Sealed` | **체크포인트 먼저**, 결론 확정, 대화·공간 변화 |
| `Sealed` | `Unseal` | `Composing` | **무제한 취소**, 손실 0 |
| any | `Close` | `Idle` | 상태 보존 |
| `Sealable` | 근거가 무효화됨(상위 상태 변화) | `Composing` | 사유 표시, 자동 해제 |

## 3. 규칙

| id | 규칙 |
|---|---|
| S-R1 | 확정 조건 3개 동시 충족: (a) **서로 다른 `sourceType` 2종**, (b) 두 근거 모두 `sensorCoverage == true`(법1), (c) 시간 근거는 정합 후 잔차 ≤ 4분(법3) |
| S-R2 | 독립성은 **매체 종류**로 센다. 염판 원본·그 표면 부식·복제 스캔은 모두 같은 출처 1개다 |
| S-R3 | 단독 증언·단일 염판은 단서로 표시되지만 슬롯을 채우지 못한다. 사유가 항상 문장으로 붙는다 |
| S-R4 | 증인은 조건부 승낙을 할 수 있다(예: 결론이 자신에게 유리할 때만). 승낙 실패는 **다른 증인 경로**로 항상 우회 가능하다 |
| S-R5 | 단독·위조 서명은 성립하지 않는다. 시도하면 `hearing_rejected` 상태가 되고 되돌릴 수 있다 |
| S-R6 | 판 #0 원본에는 **이름이 없다**. 서명지의 가려진 두 번째 이름은 `c4`에서 복원되어 처음 확정된다. Sim은 그 이전에 `plateZeroNameKnown = false`를 유지한다 [OBSERVED: timeline.md 정합 규칙] |
| S-R7 | 최종 제출(`c7-b4`)의 관점 3종은 **같은 회차에서 재시작 없이 전환·재확정 가능**하다 |
| S-R8 | 관점 3종은 **공통 종결 씬 1개 + 기록 패널 3종**을 공유하고 새 씬을 만들지 않는다 |

## 4. 실패 모드

| id | 상황 | 시스템 반응 | 플레이어 손실 |
|---|---|---|---|
| S-F1 | 같은 매체 2개 제출 | 슬롯 거부 + 사유 | 없음 |
| S-F2 | 미배선 근거 제출 | 슬롯 거부 + `out_of_coverage` | 없음 |
| S-F3 | 미정합 시간 근거 | 슬롯 거부 + 잔차 값 표시 | 없음 |
| S-F4 | 증인 승낙 거부 | 대체 증인 목록 표시(존재 보증) | 시간만 |
| S-F5 | 확정 후 후회 | `Unseal` 또는 체크포인트 로드 | 없음 |
| S-F6 | 어떤 결론도 근거 2종을 못 채우는 데이터 | **임포트 실패**(fail-closed) | 없음 |
| S-F7 | 오확정(확정 입력 오작동 — `two-step` 오조작 또는 홀드 미끄러짐) | 확정 직전 체크포인트로 복귀 | 없음 |

## 5. 데이터 스키마 참조

- `systems/data-schemas/plates.md` — `sourceType`(2026-09-10 R7 개명 · 이전 `mediaType` · `beats.md` §1-4), `sensorCoverage`, `stationId`
- `systems/data-schemas/tools.md` — `seal` 행(확정 있음, 프리뷰 필수). **길게 누름은 필수가 아니다** — 확정 기본값은 `two-step`, 홀드는 opt-in(`interaction-rules.md` §1-1, RFC-P3-015 F10)
- `systems/data-schemas/beats.md` — `seal` 사용 6개 비트, `c7-b4` 최종 제출
- `systems/data-schemas/save.md` — `sealedConclusions[]`, `witnessChoices[]`, `submissionPerspective`

## 6. 텔레메트리 필드

| 키 | 타입 | 의미 |
|---|---|---|
| `seal_attempt_count` | int | 확정 시도 |
| `seal_reject_reason` | enum[] | `same_media` \| `out_of_coverage` \| `residual_exceeded` \| `witness_declined` |
| `seal_commit_count` | int | 확정 수 |
| `seal_unseal_count` | int | 확정 취소 수 |
| `submission_perspective` | enum | `full_restoration` \| `system_defect` \| `incomplete_acknowledged` |
| `perspective_switch_count` | int | 최종 화면에서 관점 전환 횟수 |
| `seal_time_min` | float | 도구 체류 시간 |

## 7. 성능 예산 [TARGET] — NOT-MEASURED

| 항목 | 목표 |
|---|---|
| 슬롯 검증(단서 ≤ 70) | ≤ 4 ms |
| 프리뷰 생성 | ≤ 16 ms |
| 확정 + 체크포인트 | ≤ 200 ms |
| 최종 제출 화면 전환 | ≤ 1000 ms |

## 8. 인수 기준

### 문서 단계 (D)
| id | 기준 |
|---|---|
| D-S1 | 확정 3조건이 6개 `seal` 비트 전부에 적용 가능한가 |
| D-S2 | 관점 3종이 새 씬을 만들지 않고 패널 3종으로 표현되는가(에셋 예산 G5) |
| D-S3 | `plateZeroNameKnown` 게이팅이 `c4` 이전 노출을 막는가 |
| D-S4 | 증인 거부의 대체 경로 존재가 데이터 불변식으로 강제되는가 |

### 빌드 후 (B)
| id | 기준 | 방법 |
|---|---|---|
| B-S1 | 세 확정 조건 중 하나라도 미충족이면 `Commit`이 이벤트 0개 반환 | EditMode 테스트 |
| B-S2 | 임의 도달 상태에서 관점 3종 전부 도달 가능 | 도달성 탐색 |
| B-S3 | 확정 → 취소 → 재확정 후 상태 해시가 최초 확정과 동일 | 결정론 테스트 |
| B-S4 | `c4` 이전 어떤 UI에도 판 #0의 이름 문자열이 바인딩되지 않음 | 바인딩 스냅샷 테스트 |
