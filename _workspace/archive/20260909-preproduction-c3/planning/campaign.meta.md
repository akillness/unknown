---
updated: 2026-09-09
cycle: 20260909-preproduction-c3
status: superseded
supersedes: null
owner: game-planner
describes: _workspace/current/planning/campaign.json
authored_by: game-synopsis-writer (위임 배정 / delegated assignment)
---

# campaign.json 메타

`campaign.json`은 구조화 산출물이므로 frontmatter를 넣지 않는다(CLAUDE.md §10). 소유자·해시·출처·관측/목표는 이 파일이 보유한다.

## 1. 파일 사실 [OBSERVED]

| 항목 | 값 |
|---|---|
| 경로 | `_workspace/current/planning/campaign.json` |
| bytes | 74342 |
| sha256 | `5029d44a8042323d438d5975604c12b17f2717fc41b93db82f1cab06996f7e16` |
| schemaVersion | 1 |
| 소유 레인 | `game-planner` (JSON) |
| 저작 | `game-synopsis-writer`가 위임 배정으로 작성, 계획 레인이 소유·유지 |
| 짝 문서 | `synopsis/campaign.md`, `synopsis/scenes-and-dialogue.md` |

## 2. 출처 (전부 C2 수정 반영 후 판본)

- `worldview/worldview-bible.md` — 불가침 6법, 기록 매체 3종, 인물 인지 범위, 결말 3종·공통 종결 씬 1개
- `worldview/timeline.md` — 저자 진실 연표 / 플레이어 인지 연표, R1·R2·R3 씨앗과 회수, 장별 밤 매핑(21:00~05:00), 정합 오차 규칙
- `worldview/glossary.md` — 고유명 표기
- `planning/gdd.md` — 9장 480분 예산, 6도구·5구역 범위, 코어 루프
- `production/premium-preproduction-contract.md` — `designMinutes` / `observedMedianMinutes` 분리, NOT-MEASURED 규율
- `qa/c2-review.md` — F1~F6. **C3 진입 전 6건 모두 원본에서 해소된 것을 확인**했다: F1 폐쇄일(사건 이후 등재), F2 판 #0(이름 없음·번호만), F3 잔차 ±4분·총 오차폭 8분, F4 단일 야간 장 매핑, F5 영구 손실 제거·자동 사본 불파괴, F6 이진 플래그 1개 + 종결 씬 1개.

## 3. 값의 성격 [TARGET] / [OBSERVED]

| 필드 | 성격 | 근거 |
|---|---|---|
| `designMinutes: 480` | [TARGET] | GDD 예산의 비트 단위 역산 |
| `observedMedianMinutes: null` | [OBSERVED] 미측정 | 사람 플레이 표본 없음 |
| `humanPlaytests: []` | [OBSERVED] 미측정 | 모집·실행 0회 |
| `minutes` (비트) | [TARGET] | 설계 중심값 |
| `fastMinutes` / `deliberateMinutes` | [TARGET] **시나리오 폭** | 빠른 진행/신중한 진행의 설계 경계이며 **측정 신뢰구간이 아니다**. 부트스트랩·IQR·표본 없음 |

숫자 합계가 맞는 것은 문서 게이트(D)이지 플레이 게이트(G7)가 아니다. 480분 합계는 8시간 플레이의 증거가 아니다.

## 4. 검증 영수증 [OBSERVED] (2026-09-09, 생성 스크립트 자체 검사)

| 검사 | 결과 |
|---|---|
| 스테이지 9개 / 분 배분 30·50·55·60·65·65·70·60·25 | PASS |
| 총합 = 480 | PASS |
| 비트 수 = 33 (T0 3, C1~C7 각 4, E0 2) | PASS |
| 스테이지별 비트 분 합 = 스테이지 분 | PASS (9/9) |
| beat id / clue id / checkpoint id 유일 | PASS (33 / 70 / 33) |
| `kind` enum 준수 | PASS — puzzle 21, exploration 2, dialogue 5, payoff 5 |
| puzzle ≥ 20 | PASS (21) — 예외 보고 불필요 |
| `sourceType` enum 준수 + 비트마다 서로 다른 매체 2종 이상 | PASS (33/33) |
| `hints` 정확히 3단 | PASS (33/33) |
| `objective/inference/action/consequence/completion/recovery` 공란 없음 | PASS |
| `prerequisites`가 앞선 비트만 참조 (순환 없음, DAG) | PASS |
| 도구 ID = circuit·reader·alignment·routing·corrosion·seal 6종 | PASS |
| 구역 ID = hub·gate·lowland·dock·pump 5종 | PASS |
| `fastMinutes < minutes < deliberateMinutes` | PASS (33/33) |

## 5. 설계 불변식 (JSON이 지키는 계약)

1. **비분기 본선.** 33비트는 단일 임계경로다. `prerequisites`는 지식 의존을 나타내는 DAG이지 선택 분기가 아니다.
2. **선택 축 2개뿐.** `propertyProtection`(lowland|dock) 이진 플래그 1개는 `e0-b1`의 후일담 문단 두 쌍만 바꾼다. 제출 관점 3종은 `c7-b4`에서 **한 번만** 세며 결말을 가산하지 않는다. 세 관점은 **공통 종결 씬 1개 + 기록 패널 3종**을 공유하고 새 씬을 만들지 않는다.
3. **영구 손실 없음.** 모든 비트의 `recovery`가 무료 복구를 명시한다. 첫 판독 자동 사본은 어떤 플레이어 행동으로도 파괴되지 않으며(`c5-b3`), 확정 직전 자동 백업으로 재시도한다(`c5-b4`, `c7-b4`, `e0-b2`).
4. **가상 시험 후 확정.** `c5-b2`(두 경로 완주)와 `c7-b2`(그날 조건 시연)는 확정 전에 결과를 먼저 보여 준다.
5. **전지 금지.** 어떤 `clues`도 염판에서 얼굴·음성·의도·사람 위치를 복원하지 않는다. 확정은 매체 2종 대조로만 이뤄진다.
6. **도구 학습 규칙.** 6도구 각각 안내 도입 1회 + 이후 미안내 새 문제 1회. 도입 비트: circuit `t0-b2` / reader `t0-b3` / seal `c1-b3` / corrosion `c2-b2` / alignment `c3-b2` / routing `c5-b2`. 미안내 재문제: `c6-b2` / `c1-b2` / `c4-b3` / `c4-b2` / `c6-b3` / `c7-b2`.
7. **가짜 시간 없음.** 강제 대기·읽기 패딩·수집 반복·실시간 침수 압박이 들어간 비트는 없다. 튜토리얼 0~30분은 설명벽 없이 단일 사건(결손 4시간)에 도구 2종과 법 2개를 붙여 가르친다.

## 6. 잔여 [OBSERVED]

실제 사람 플레이타임 표본 **n = 0**. 힌트 사용률·이탈률·읽기 시간·완주율·IQR 모두 미측정이며 `humanPlaytests`는 빈 배열로 정직하게 남긴다. 이 파일의 어떤 수치도 G2/G4/G5/G6/G7을 올리지 못하고 D 게이트 입력일 뿐이다. 다음 측정 계획은 T0 30분 + `c3-b2`~`c3-b3` 정합 구간만 담은 검증 슬라이스로 `observedMedianMinutes`의 첫 값과 정합 퍼즐 통과율을 재는 것이다.

## 7. 미실행

이 사이클에서 공유 산출물(`worldview/`, `gdd.md`, `qa/`, `decision-log.md`, `task-manifest.md`)은 **편집하지 않았다**. git commit / push도 실행하지 않았다.
