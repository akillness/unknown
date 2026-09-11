---
updated: 2026-09-10
cycle: 20260909-preproduction-c3
status: current
supersedes: null
owner: game-systems-designer
---

# 시스템 스펙 — tide-alignment (도구 `alignment`, 법3 "정합 전 시계는 믿지 않는다")

> **법 호명 정본** [OBSERVED · RFC-P3-014 · RFC-W3]: 6법의 호명 문구는 `worldview/worldview-bible.md` §3 표(= 아카이브 c3 세션 P 원문) 하나뿐이다. 이 스펙은 인용만 하고 다시 쓰지 않는다. 폐기된 C2 호명 문구는 `worldview/consistency-audit.md` 의 **「사용 금지 문구」 절**(그 안의 표 **「폐기된 6법 호명 문구」** — RFC-W3 이 "사용 금지 문구 원장"이라 부르는 절)에 보존돼 있다. **절 번호로 인용하지 않는다** — 감사 문서의 절 번호가 바뀌어도 이 인용은 살아 있어야 한다(RFC-W3, `qa/c3-review.md` §9.3 q-4 의 QA 대안).

전부 `[TARGET]`. 이 스펙은 **C2 QA F3(20분 인과가 ±40분 오차보다 작다)** 의 시스템 해답이다.

| 항목 | 값 |
|---|---|
| 도구 id | `alignment` |
| 도입 비트 / 미안내 재문제 | `c3-b2` / `c6-b3` [OBSERVED] |
| 등장 비트 수 | 8 / 33 [OBSERVED] |
| 세계관 상수 | 원시 관측소 간 오차 최대 ±40분, 분해능 4분, 정합 후 허용 잔차 ≤ 4분 [OBSERVED: worldview/timeline.md 정합 오차 규칙] |

## 1. 입력

| 입력 | KB/마우스 | 패드 | 결과 |
|---|---|---|---|
| 자료 2종 적재 | 증거함 → 좌·우 트랙 | `A` | `trackA`, `trackB` |
| 공통 피크 지정 | 곡선 위 클릭 ×3 | `A` ×3 | `anchors[0..2]` |
| 오프셋 조절 | 드래그 / `Shift` 정밀 | 스틱 / `LT` 정밀 / D-Pad 1분 | `offsetMinutes` |
| 자동 최소자승 제안 | `Space` | `X` | 제안 오프셋 표시(**적용은 수동**) |
| 기준선 확정 | 확정 버튼 초점 후 `Enter` (기본 `two-step`) | 초점 후 `A` | `AlignBaseline` 명령. **길게 누름은 `hold` opt-in 에서만**이며 유지 시간은 데이터 노브 `commitHoldSeconds`(기본 0.4 s · 범위 0.2~1.5 s [tunable: balance])다 `[C4-F20 정정 2026-09-10 R7 종료]` |
| 선후 판정 조회 | **`Q`** | `RS` | 두 사건 간격 vs 오차폭 비교 패널. **`H`가 아니다** — `H`는 표면과 무관하게 가설판이다(`interaction-rules.md` §1-3.1 예외 · C7-F10 정정 2026-09-10 R7) |

## 1-A. 키보드 파생 `[C4-F21]`

**신설 2026-09-10 R6 (C6/C7).** §1 표에서 KB/마우스 열이 포인터 조작만 적은 행의 키보드 단독 경로다. 규칙 id 는 `systems/interaction-rules.md` §1-3.4(D-1~D-7)를 인용한다. §0-8(키보드 단독 완결)이 전역 보장이고, 이 표는 그 보장이 **행 단위로** 어떻게 성립하는지를 적는다.

| 위 표의 행 | 파생 규칙 | 키보드 단독 경로 |
|---|---|---|
| 자료 2종 적재 (증거함 → 좌·우 트랙) | **D-2** + **D-7** | `I`로 증거함을 열고 자료 초점 `Enter`(집기) → 좌/우 트랙 초점 `Enter`(적재). 두 트랙을 순서대로 반복 |
| 공통 피크 지정 (곡선 위 클릭 ×3) | **D-1** + **D-3** · **스텝 = 1분** | 곡선 커서를 `←`/`→` 1분 스텝으로 옮겨 `Enter`, 또는 `Tab` 으로 **극값 후보 사이를 점프**한 뒤 `Enter`. 후보 목록은 데이터의 조위 극값이며 임의 좌표가 아니다 |
| 오프셋 조절 | — | 이미 키보드 경로다(`Shift` 정밀 + §1 방향키 1스텝 · **1분**) |
| 자동 최소자승 제안 · 기준선 확정 · 선후 판정 조회 | — | 이미 키보드 경로다(`Space` · `Enter` · **`Q`**). `Q`는 도구 패널 표면 전용 조회 키이며 `H`(가설판)와 겸용하지 않는다(C7-F10) |

- 「기준선 확정」 행의 지속시간 표기는 **C4-F20 으로 해소됐다**(2026-09-10 R7 종료): 확정 기본값은 `two-step`, 0.4 s 는 홀드 opt-in 전용 데이터 노브. `interaction-rules.md` §0-9·§0-11·§1-1 이 정본이다.
- 이 도구의 연속값 이산 대안(1분 스텝 · 극값 점프)은 `planning/gdd.md` §8 계약의 이행분이다.

## 2. 상태기계

상태 변수: `trackA`, `trackB`, `anchors`, `offsetMinutes`, `residualMinutes`, `baselineLocked`.

| 현재 | 이벤트 | 다음 | 부작용 |
|---|---|---|---|
| `Empty` | `LoadTracks(a,b)` | `Loading` | 두 곡선 표시, 원시 오차 표시(±40분) |
| `Loading` | 두 트랙 준비 완료 | `Anchoring` | 피크 후보 하이라이트 |
| `Anchoring` | `SetAnchor(i)` (i<3) | `Anchoring` | 앵커 표시 |
| `Anchoring` | 3번째 앵커 지정 | `Adjusting` | 잔차 실시간 계산 시작 |
| `Adjusting` | `SetOffset` | `Adjusting` | `residualMinutes` 갱신, 라벨 갱신 |
| `Adjusting` | `residual ≤ 4` | `Alignable` | 확정 버튼 활성 |
| `Alignable` | `residual > 4` (재조작) | `Adjusting` | 확정 버튼 비활성 + 사유 |
| `Alignable` | `AlignBaseline` | `Locked` | 기준선 확정, 체크포인트, 이후 이 자료쌍의 시간축 통일 |
| `Locked` | `Unlock` | `Adjusting` | **무제한 재정합**, 비용 0 |
| any | `ClearAnchors` | `Anchoring` | 손실 0 |

## 3. 규칙

| id | 규칙 |
|---|---|
| A-R1 | 확정 조건 = 앵커 3개 지정 **그리고** `abs(residualMinutes) ≤ residualLimit(=4)` [tunable: balance] |
| A-R2 | **선후 판정 규칙**: 두 사건의 간격 `gap`이 두 근거 각각의 오차폭 합 `errA + errB` 보다 **클 때만** 순서를 확정한다. 아니면 `indeterminate`. 같은 계통 로그의 두 각인이어도 각 근거에 정합 후 잔차를 보수적으로 따로 적용한다(캐논이 총 오차폭 8분을 쓰는 이유) |
| A-R3 | 캐논 사례(RFC-P3-013 확정 시각만 사용): **밸브 개폐 각인(H-1:24) → 봉인 완료 접점 각인(H-1:04)**, 간격 20분. 정합 후 잔차 ±4분 × 2 = 총 오차폭 8분 → **20 > 8 이므로 확정 가능** [OBSERVED: `worldview/timeline.md` §2 L35 · §8 "순서 앵커 — 왜 20분을 말할 수 있는가"] |
| A-R4 | `indeterminate`는 실패가 아니다. 진행은 계속되고 그 상태로 다른 근거를 찾을 수 있다 |
| A-R5 | 시각 표기는 절대시가 아니라 조위 위상(H±). 절대시로 적힌 자료는 그 자체가 의심 신호로 표시된다 |
| A-R6 | 자동 제안은 **적용되지 않는다**. 플레이어가 확정한다(퍼즐을 대신 풀지 않는다) |
| A-R7 | 재정합 횟수·시간에 페널티 없음. 부식·예산과 무관 |
| A-R8 | 성찬 대장은 기준 관측소가 다르다. 데이터에서 `stationId` 로 표현하고 코드에 인물 이름을 넣지 않는다 |

**A-R3 이력** [CARRIED · C3-F26]: 이전 판의 A-R3은 RFC-P3-013으로 폐기된 시각쌍(서명 → 밸브)을 캐논 사례로 인용했다. 폐기 시각 목록의 정본 기록은 `worldview/timeline.md` L50이며 이 스펙은 그 문자열을 다시 쓰지 않는다. 현행 A-R3은 RFC-P3-013이 **확정**한 두 시각만 사용하므로 C3-F25(H-1:40의 지위)가 (a)·(b) 어느 쪽으로 판정되어도 재작성 대상이 아니다 [INFERENCE].

## 4. 실패 모드

| id | 상황 | 시스템 반응 | 플레이어 손실 |
|---|---|---|---|
| A-F1 | 잔차 > 4분에서 확정 시도 | 버튼 비활성 + "잔차 초과 (현재 N분 / 한도 4분)" | 없음 |
| A-F2 | 공통 조위 사건이 없는 두 자료 | `Anchoring`에서 후보 0개 + "같은 조위 사건을 공유하지 않음" | 없음, 다른 조합 유도 |
| A-F3 | 미정합 상태로 순서 주장 | `seal` 슬롯이 시간 근거를 거부, 사유 표시 | 없음(제출이 막힘) |
| A-F4 | 앵커 3개가 같은 피크에 몰림 | 잔차가 수렴하지 않고 "앵커 분산 부족" 표시 | 없음 |
| A-F5 | 정합 확정 후 원본 자료가 붕괴(법2) | 확정된 기준선은 사본에 귀속되어 **유지된다** | 없음 |

## 5. 데이터 스키마 참조

- `systems/data-schemas/plates.md` — `stationId`, `stationErrorMinutes`, `tidePeaks[]`
- `systems/data-schemas/tools.md` — `alignment` 행, `residualLimitMinutes`(tunable)
- `systems/data-schemas/beats.md` — `alignment` 사용 8개 비트
- `systems/data-schemas/save.md` — `alignedPairs[]`(자료쌍 → 확정 오프셋)

## 6. 텔레메트리 필드

| 키 | 타입 | 의미 |
|---|---|---|
| `align_attempts` | int | 정합 시도 수 |
| `align_residual_final` | float | 확정 시 잔차(분) |
| `align_residual_history` | float[] | 조작 중 잔차 추이(표본 상한 200) |
| `align_indeterminate_count` | int | 선후 판정 불가 표시 횟수 |
| `align_auto_suggest_used` | int | 자동 제안 조회 수 |
| `align_time_min` | float | 도구 체류 시간 |
| `align_unlock_count` | int | 확정 후 재정합 횟수 |

## 7. 성능 예산 [TARGET] — NOT-MEASURED

| 항목 | 목표 |
|---|---|
| 잔차 재계산(앵커 3, 샘플 2×180) | ≤ 2 ms (드래그 중 매 프레임) |
| 곡선 2종 렌더 | ≤ 4 ms |
| 최소자승 제안 계산 | ≤ 20 ms (1회성, 비동기 아님) |
| 확정 + 체크포인트 | ≤ 200 ms |

## 8. 인수 기준

### 문서 단계 (D)
| id | 기준 |
|---|---|
| D-A1 | 잔차 한도 4분이 데이터 상수이고 코드 리터럴이 아닌가 |
| D-A2 | A-R2의 부등식이 캐논 사례(밸브 개폐 각인 H-1:24 → 봉인 완료 접점 각인 H-1:04, 간격 20분)에서 실제로 성립하는가 (20 > 8 ✓) |
| D-A3 | `indeterminate`가 진행 차단이 아님을 상태기계가 보이는가 |
| D-A4 | 인물 이름이 규칙에 등장하지 않고 `stationId`로만 표현되는가 |

### 빌드 후 (B)
| id | 기준 | 방법 |
|---|---|---|
| B-A1 | 잔차 4.0분 경계에서 확정 활성/비활성이 뒤집힘 | 경계값 테스트 |
| B-A2 | 간격 8분 이하 사건쌍은 어떤 조작으로도 `ordered`가 되지 않음 | 속성 기반 테스트 |
| B-A3 | 드래그 중 잔차 계산이 프레임 예산 2 ms 이내 | 프레임타임 캡처 |
| B-A4 | `residualLimitMinutes`를 4→3으로 바꾸면 코드 수정 없이 반영 | 데이터 스왑 |
