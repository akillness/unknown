---
updated: 2026-09-10
cycle: 20260909-preproduction-c3
status: current
supersedes: null
owner: game-systems-designer
---

# 시스템 스펙 — plate-readout (도구 `reader`, 법2 "원본은 닳지만 사본은 남는다")

> **법 호명 정본** [OBSERVED · RFC-P3-014 · RFC-W3]: 6법의 호명 문구는 `worldview/worldview-bible.md` §3 표(= 아카이브 c3 세션 P 원문) 하나뿐이다. 이 스펙은 인용만 하고 다시 쓰지 않는다. 폐기된 C2 호명 문구는 `worldview/consistency-audit.md` 의 **「사용 금지 문구」 절**(그 안의 표 **「폐기된 6법 호명 문구」** — RFC-W3 이 "사용 금지 문구 원장"이라 부르는 절)에 보존돼 있다. **절 번호로 인용하지 않는다** — 감사 문서의 절 번호가 바뀌어도 이 인용은 살아 있어야 한다(RFC-W3, `qa/c3-review.md` §9.3 q-4 의 QA 대안).

전부 `[TARGET]`. 구현 0줄. 이 스펙은 **C2 QA F5(영구 손실)** 대응을 시스템 수준에서 못 박는다.

| 항목 | 값 |
|---|---|
| 도구 id | `reader` |
| 세계관 근거 | 법2 (`worldview/worldview-bible.md` §3), 기록 물리 §2 |
| 도입 비트 / 미안내 재문제 | `t0-b3` / `c1-b2` [OBSERVED] |
| 등장 비트 수 | **11 / 33** [OBSERVED 2026-09-10 재측정] — `t0-b3` `c1-b2` `c1-b3` `c2-b1` `c2-b4` `c3-b1` `c3-b3` `c4-b1` `c4-b2` `c4-b4` `c5-b3`. 이전 판의 8은 폐기(C3-F2) |
| 매체 3종 | `plate`(염판, 24) / `log`(당직일지, 27) / `ledger`(조위대장, 22) — 단서 73건 [OBSERVED: 검증기 `sourceTypeDist`] |
| 원본 상태 모델 | **비차단 카운터**(economy §4.2 옵션 B). 상한 3, **사본 판독 무제한** |

## 1. 입력

| 입력 | KB/마우스 | 패드 | 결과 |
|---|---|---|---|
| 매체 적재 | 증거함에서 판독대로 드래그 | `A` 선택 → `A` 적재 | `loadedRecordId` 설정 |
| 시간 범위 지정 | 타임라인 드래그 | 좌스틱 + `D-Pad ←/→` 4분 스텝 | `windowStart`, `windowEnd` |
| 배율 | 휠 | `LT/RT` | `zoom` |
| 재생(판독) | `Space` | `X` | `Read` 명령 제출 |
| 인용 고정 **(확정)** | 결과 위 `Ctrl+클릭`, 또는 결과 항목 초점 후 `Enter`(기본 `two-step`) | `Y` | `CiteToBoard` — **이 도구의 확정(commit) 명령**. 가설판에 출처(`sourceType`·계통·관측소)와 함께 고정한다. 확정이므로 **사전 체크포인트**를 남기고 저장 트랜잭션을 거친다 `[RFC-C7-001 (1) · 2026-09-10 R7 종료]` |

## 1-A. 키보드 파생 `[C4-F21]`

**신설 2026-09-10 R6 (C6/C7).** §1 표에서 KB/마우스 열이 포인터 조작만 적은 행의 키보드 단독 경로다. 규칙 id 는 `systems/interaction-rules.md` §1-3.4(D-1~D-7)를 인용한다. §0-8(키보드 단독 완결)이 전역 보장이고, 이 표는 그 보장이 **행 단위로** 어떻게 성립하는지를 적는다.

| 위 표의 행 | 파생 규칙 | 키보드 단독 경로 |
|---|---|---|
| 매체 적재 (증거함에서 판독대로 드래그) | **D-2** + **D-7** | `I`로 증거함을 열고 자료에 초점 후 `Enter`(집기) → 판독대에 초점 후 `Enter`(적재) |
| 시간 범위 지정 (타임라인 드래그) | **D-3** · **스텝 = 4분** | `←`/`→` 로 경계를 4분 스텝 이동, `Shift`+방향키는 정밀 이동. 4분은 `plates.md` `resolutionMinutes` 상수이며 코드 하드코딩 대상이 아니다(P-R5) |
| 배율 (휠) | **D-3** · **스텝 = 배율 단계 1칸** | `↑`/`↓` 로 배율 단계를 한 칸씩 이동. **단계 목록의 값은 아직 정해지지 않았다 `[TARGET]`** — 데이터 노브로 두며, 값이 정해지기 전까지 D-3 의 「스텝 크기 명시」 요건은 "1칸 = 목록의 인접 항목"으로 충족한다 |
| 인용 고정 (결과 위 `Ctrl+클릭`) | **D-1** | 결과 항목에 초점 후 `Enter`. `Ctrl+클릭` 은 포인터 편의 경로다 |
| 재생(판독) | — | 이미 키보드 경로다(`Space`). **사본 재생에만 배정**되며 원본 직접 절차는 확정 흐름을 거친다 — `interaction-rules.md` §1-3.3 |

- 연속값 2행(시간 범위 · 배율)의 이산 대안은 `planning/gdd.md` §8 「연속값 조작의 이산 대안」 계약을 이 도구에서 이행한 것이다.

## 2. 상태기계

상태 변수: `loadedRecordId`, `readCount[recordId]`, `autoCopyTaken[recordId]`, `window`, `citations`.

| 현재 | 이벤트 | 다음 | 부작용 |
|---|---|---|---|
| `Empty` | `LoadRecord(id)` | `Loaded` | 4분 분해능 눈금 표시 |
| `Loaded` | `SetWindow` | `Loaded` | 미리보기 갱신(재생 아님, 예산 소모 0) |
| `Loaded` | `Read` (최초, `autoCopyTaken == false`) | `Read` | **검증 사본 생성** + `autoCopyTaken = true` + 토스트 |
| `Loaded` | `Read` (재판독, 사본 존재) | `Read` | **사본에서 읽음. `readCount` 증가 없음** |
| `Read` | `CiteToBoard` | `Read` | 인용 카드 생성(매체·계통·관측소 기록) |
| `Read` | `ReadOriginal` (사본에 없는 구간) | `Read` | `readCount[id] += 1` |
| `Read` | `readCount == 3` 인 상태에서 `ReadOriginal` | `Degraded` | 원본 결정 붕괴, **사본은 그대로 유지**. 진행 차단 아님 |
| `Degraded` | `Read` | `Read` | 사본 기반 판독은 **무제한** 계속 가능 |
| any | `Unload` | `Empty` | 상태 보존 |

## 3. 규칙

| id | 규칙 |
|---|---|
| P-R1 | **첫 판독은 항상 자동 사본을 만든다.** 사본은 어떤 플레이어 행동으로도 파괴되지 않는다(하드 불변식) |
| P-R2 | **원본 상태 카운터** `readBudget = 3` [tunable: economy]. **원본에 직접 가하는 파괴적 절차만** +1 한다. 상한(3)에 닿은 뒤의 원본 직접 절차는 **원본의 결정 구조를 붕괴시키지만 사본은 그대로 유지되며 진행을 막지 않는다**(P-R9 · P-R3 · §2 `Degraded` 전이). 사본 판독·재생·되돌림·미리보기는 이 값을 건드리지 않는다. **[2026-09-10 R6 정정]** 이전 판의 「4회째에 원본이 붕괴한다」는 같은 표의 P-R9(비차단)와 차단형으로 읽히는 모순을 만들었다(`planning/gdd.md` §4.1 counter (a)) |
| P-R9 | **비차단이다** (economy §4.2 옵션 B 채택안). 상한 도달 후에도 **사본 경로로 모든 필수 확정이 가능**하다. 잔량 0이 판독 자체를 막는 "차단형 예산"(옵션 C)은 법2 위반이자 진행 불가 경로 생성이므로 **거부**됐다 |
| P-R10 | **명칭 규칙**: UI 표기는 **"원본 상태"**이며 "예산"이라는 단어는 부식에만 쓴다(economy RFC-E3). 잔량 막대·소진 경고·아껴 쓰라는 문구를 쓰지 않는다. `readBudget`은 데이터 키 이름으로만 남고 화면에 노출되지 않는다 |
| P-R11 | 이 카운터는 **에필로그 기록 패널의 보존 등급 문장 1줄**만 바꾼다. 필수 단서·세 결말 접근·힌트·저장에는 영향 0 |
| P-R3 | 원본 붕괴는 **사본에 이미 담긴 정보를 잃게 하지 않는다**. 잃는 것은 사본에 없는 구간의 신규 판독 능력뿐이다 |
| P-R4 | 모든 필수 확정에는 **`sourceType` 상이 AND 루트 `originId` 상이**를 만족하는 자료쌍이 1쌍 이상 있고, 그 쌍의 한쪽 경로는 어떤 플레이어 행동으로도 파괴되지 않는다(임포트 검증으로 강제 · 검증기 `C-07`). 「매체 경로」라는 세 번째 용어는 쓰지 않는다 [C5-F5 · `unity-implementation.md` §5 1항] |
| P-R5 | 분해능은 4분. Sim은 4분 미만 간격을 **`indeterminate`** 로 유지하고 반올림하지 않는다 |
| P-R6 | 염판은 밸브·압력·염도·수위·문 개폐·당직 호출과, 회선 3개소의 **통화 개시 시각·계통**만 담는다. 얼굴·음성·의도·사람 위치는 어떤 조작으로도 나오지 않는다 |
| P-R7 | 카운터 증가는 **연습(sandbox)에서 일어나지 않는다**. sandbox는 표시만 한다 |
| P-R8 | 판독은 확정이 아니다. 확정은 `dual-seal`에서만 일어난다 |

## 4. 실패 모드

| id | 상황 | 시스템 반응 | 플레이어 손실 |
|---|---|---|---|
| P-F1 | 상한 도달 후 원본 신규 구간 판독 시도 | 사유 표시 + **대체 자료 안내**(다른 `sourceType`·다른 루트 `originId` · 힌트 1단계와 동일 정보). 사본 판독은 계속 열려 있다 | 그 구간의 **원본** 판독만. 확정 경로는 남는다 |
| P-F2 | 같은 매체를 근거 2슬롯에 넣음 | 두 번째 슬롯 회색 + "독립 매체 2종 필요" | 없음 |
| P-F3 | 사본 파괴를 시도하는 명령이 데이터에 존재 | **임포트 실패**(fail-closed) | 없음 |
| P-F4 | 4분 미만 간격으로 선후 확정 시도 | `indeterminate` 유지, 확정 비활성 + 사유 | 없음, 진행 계속 |
| P-F5 | 판독 중 세이브·종료 | `readCount`는 이미 커밋된 값만 저장. 진행 중 판독은 증가 아님 | 없음 |

## 5. 데이터 스키마 참조

- `systems/data-schemas/plates.md` — `plateId`, `sourceType`(2026-09-10 R7 개명 · 이전 `mediaType` · `beats.md` §1-4), `systemId`, `stationId`, `ringHours: 12`, `resolutionMinutes: 4`, `readBudget`(상한 3 · `[tunable: balance]`) **[2026-09-10 R6 정정]** — 이전 판은 이 노브를 economy 의 `plateOriginalWear` 별칭으로 설명했으나 그 이름은 economy 가 §4.2 에서 **폐기**했고 스키마에 존재하지 않는다(`economy/negotiation-record.md` N-16 · `gdd.md` §4.1 counter (b)). 정본 이름은 상한 `readBudget` 과 누계 `save.md` `readCounts` 둘뿐이다, `autoCopyOnFirstRead`, `segments[]`, `indestructible`
- `systems/data-schemas/tools.md` — `reader` 행
- `systems/data-schemas/beats.md` — `clues[].sourceType` ∈ {plate, log, ledger}
- `systems/data-schemas/save.md` — `readCounts`, `autoKeptClueIds`

## 6. 텔레메트리 필드

| 키 | 타입 | 의미 |
|---|---|---|
| `read_count_total` | int | 원본 판독 총 횟수 |
| `read_budget_exhausted` | int | 예산 소진 발생 건수(레코드 단위) |
| `auto_copy_created` | int | 자동 사본 생성 수(= 최초 판독 레코드 수) |
| `citation_count` | int | 가설판 인용 고정 수 |
| `indeterminate_shown` | int | 4분 미만 판정 회피 표시 횟수 |
| `reader_time_min` | float | 도구 체류 시간 |
| `alt_path_offered` | int | P-F1에서 대체 경로 안내 횟수 |

## 7. 성능 예산 [TARGET] — NOT-MEASURED

| 항목 | 목표 |
|---|---|
| 곡선 렌더(12시간 링, 4분 눈금 = 180 샘플) | ≤ 4 ms |
| 판독 명령 `Validate` | ≤ 8 ms |
| 사본 생성(메모리 복사) | ≤ 16 ms |
| 매체 텍스처 스트리밍 | ≤ 250 ms, 그 전까지 저해상 프록시 |
| 판독대 메모리 | ≤ 48 MB |

## 8. 인수 기준

### 문서 단계 (D)
| id | 기준 |
|---|---|
| D-P1 | 8개 `reader` 비트 전부가 "사본 보존 후 재판독" 경로를 갖는가 |
| D-P2 | 예산 소진이 어떤 비트에서도 필수 확정을 불가능하게 만들지 않는가(매체 2경로 표로 확인) |
| D-P3 | 4분 분해능이 스키마 상수로 존재하고 코드 하드코딩 대상이 아닌가 |

### 빌드 후 (B)
| id | 기준 | 방법 |
|---|---|---|
| B-P1 | 임의 시드 10k 스텝 후 `autoKeptClues ⊆ currentClues` | 퍼즈 테스트 |
| B-P2 | 모든 필수 확정에 `sourceType` 상이 AND 루트 `originId` 상이인 자료쌍 ≥ 1, 그 쌍의 파괴 불가 경로 ≥ 1 | 그래프 검증 |
| B-P3 | `readBudget`을 테이블에서 3→2로 바꾸면 코드 수정 없이 반영 | 데이터 스왑 테스트 |
| B-P5 | `readBudget`을 **0으로 낮춰도** 사본 경로로 세 결말 전부 도달 가능(비차단 증명) | 도달성 탐색 |
| B-P4 | 예산 소진 상태에서 세 엔딩 모두 도달 가능 | 도달성 탐색 |

## 변경 로그 (같은 사이클 제자리 개정 · RFC-Q2)

| 날짜 | 회차 | 결함 | 절 | 정정 |
|---|---|---|---|---|
| 2026-09-10 | R4 수정 루프 1 | **C4-F5 파생** | §1 「시간 범위 지정」 행 | 패드 `LB/RB` 4분 스텝 → **`D-Pad ←/→`**. `interaction-rules.md` §1-2가 `LB`를 **모디파이어 전용**으로 내렸으므로(되돌림 `LB`+`X` 가 패널 안에서도 살아야 한다) `LB` 단독 스텝은 성립하지 않는다. `D-Pad 1스텝`은 `interaction-rules.md` §1 「값 미세 조절」의 정본 행이며 `LT` 정밀도 그대로다 |

- 키보드 경로(타임라인 드래그 / 방향키 1스텝)는 **바뀌지 않았다**.
- `cycle` 값 불변 — 제자리 개정, `supersedes: null` 유지(RFC-Q2). 새로 측정된 값 0건.
- **잔여 [OPEN, 이번 배정 밖]**: 같은 표의 「재생(판독) | `Space` | `X`」 는 `interaction-rules.md` §1의 `Space`/`X`=프리뷰와 표면 우선순위가 정해지지 않은 상태다 — **C4-F16**(`Space` 우선순위)의 대상이며 이번 루프에서 손대지 않았다.
