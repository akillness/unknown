---
updated: 2026-09-10
cycle: 20260909-preproduction-c3
status: current
supersedes: null
owner: game-economy-designer
---

# 자원 지도 — 조수기록국: 마지막 당직 (가제)

## 0. 지위 · 입력 · 선행 문서

- 이 문서는 **게임 내 비화폐 자원**의 정의·상한·회복·소스·싱크를 고정한다. 가격·수익·할인·DLC 가격은 이 레인이 아니다(`product/business-model.md`, PM 소유, 인용만).
- `supersedes: null` — 아카이브(`_workspace/archive/20260909-preproduction-c{1,2,3}/`)에 economy 산출물이 **한 건도 없음을 확인**했다 [OBSERVED].
- `economy/resources-and-fairness.md`(cycle c4, `status: draft`, 같은 소유자)는 본 문서의 **압축 선행 초안**이다. 내용은 여기서 확장·수치화되며, 중복 해소를 위한 아카이빙은 디렉터 권한이다(본 문서 §8 열린 질문 Q1).
- 입력: `worldview/worldview-bible.md` §3 6법 · `planning/gdd.md` · `planning/campaign.json` · `systems/interaction-rules.md` · `systems/prototype/model.mjs` · `product/business-model.md`.

### 0.1 C3 종료 수정 루프 재측정 영수증 (2026-09-10) [OBSERVED]

| 대상 | 명령 | 결과 |
|---|---|---|
| live 캠페인 데이터 | `shasum -a 256 _workspace/current/planning/campaign.json` | `fdabf1d421932d679429870a06eeb31e9527562370c6a107aec77c2c884fb2b7`, **120479 B** |
| 집계 | `node _workspace/current/planning/validate-campaign.mjs` | 44/44 PASS · 스테이지 분 25·50·55·65·65·70·75·65·10 = 480 · 비트 33 · clue **73** · `routing` 3비트 · `corrosion` 3비트 · `tools: []` 5비트 · fast 322 · deliberate 673 |
| 상수 | `grep -n "corrosionLimit\|corrosion:" systems/prototype/model.mjs` | `model.mjs:63-65` lowland 7 · dock 8 · dock-express 12 / `model.mjs:79` `corrosionLimit: 9` |

- RFC-P3-008이 정본으로 지정한 live sha는 판정 시점 값 `775a984c…`였으나, **본 세션에서 planner가 F5·F6·F11을 수리하면서 파일이 다시 바뀌어 현재 실물은 `fdabf1d4…`(120479 B)**다 [OBSERVED]. 계보는 동일(계보 B, 스테이지 분 배분 불변)하고 집계 중 `clue`만 72 → **73**으로 변했다. 이 문서의 모든 집계는 `fdabf1d4…` 기준으로 재계산했다.
- 이 절의 숫자는 전부 **문서·코드 상수**다. 플레이 실측은 여전히 n=0이다(§7).

### 0.2 개정 이력 (같은 사이클 제자리 갱신 — RFC-Q2) [OBSERVED]

`cycle` 값은 `20260909-preproduction-c3` 그대로이고 `supersedes: null`을 유지한다. RFC-Q2에 따라 같은 사이클 안의 제자리 갱신은 "대체"가 아니므로 아카이빙 대상이 아니며, 이력은 이 절이 담당한다.

| 개정 | 대상 절 | 무엇을 바꿨나 | 근거 |
|---|---|---|---|
| R4-1 (2026-09-10) | §4.1 | 계통별 표시 분해를 **저장 필드 기반 → 저장 없는 파생 표시값**으로 재정의. 산출식 `d_s`·재계산 시점·세이브 무영향 명시. 이전 판의 "`save.md:55`에 저장 자리가 있다"는 `[OBSERVED]` 주장 철회 | C3-F27(c) · RFC-P3-009 · `systems/data-schemas/save.md:94`·`:102` |
| R4-2 (2026-09-10) | §4.1 끝 | `corrosion-budget.md` C-R2 불일치 주장 **철회**(실물은 이미 전역 9로 정정됨). 대신 `plateOriginalWear` 별칭 잔존을 새 후속으로 등록 | C3-F27(a) economy 측 · `corrosion-budget.md:91`·`:94` |
| R4-3 (2026-09-10) | §4.2 | "채택안 `plateOriginalWear`"를 **`readCounts`(누계) + `readBudget`(상한 3)** 으로 교체하고 근거를 systems 실제 스키마 인용으로 바꿈 | C3-F35 · `save.md:54` · `plates.md:39` · `plate-readout.md:55~57` |
| R4-4 (2026-09-10) | §5 · §6 | 저장 필드 표(§6)의 R1 행을 **"저장 없음(파생)"** 으로 정정, v1 존재 필드 넷 → **셋**, 개명 금지 목록 구성원 오기 정정. §5에 저장 필드 표의 소재 안내 추가 | C3-F27(c) · `save.md:92`·:94 |
| R4-5 (2026-09-10) | §8 | Q6 닫음(대상 해소), Q7 신설(`plateOriginalWear` 별칭 2행) | 위 R4-2 |
| R4-6 (2026-09-10) | §4.1 결론 | "차감/소모" 문언이 systems C-R9("부식은 소모되지 않는다")와 어긋난다는 사실을 **수치 변경 없이** 명시하고 판정 요청으로 올림 | `corrosion-budget.md:94` · `negotiation-record.md` N-18 / RFC-E8 |

**바꾸지 않은 것**: §0.1 영수증 3행(재측정 결과 동일 — sha `fdabf1d4…`·120479 B·44/44 PASS·clue 73), 부식 상한 9와 선택지 비용 7/8/12, 안전 하한 공식, R2 상한 3, G3 N/A 판정 5종. **플레이 실측은 여전히 n=0이며 이 개정으로 올라간 게이트는 없다.**

## 1. G3(경제 건전성) 표준 항목의 N/A 판정

| 표준 항목 | 판정 | 근거 |
|---|---|---|
| `fairness.paid_free_winrate_delta_max_pp` | **N/A** | 승패가 없다. 전투·PvP·경쟁 순위 없음(`gdd.md` 범위 "제외: 전투, 멀티플레이"). 승률을 정의할 대상 자체가 없다 [OBSERVED] |
| `inflation.monthly_max_pct` | **N/A** | 통화가 없다. 유료 재화·게임 내 상점·거래·시세 0종(`gdd.md` 제외 목록, `business-model.md` §8) [OBSERVED] |
| `comeback.reversal_probability_max` | **N/A** | 승패 역전 개념이 없다. 실패는 상태를 되돌릴 뿐 순위를 만들지 않는다 |
| `steady.parity_sessions_band` | **N/A** | 반복 세션·일일 접속 보상·시즌 진행도가 없다. 1회 완결 캠페인 |
| `source_per_day / sink_per_day / ratio_band` | **N/A** | 일 단위 재생성 자원이 없다. 소모 발생 지점이 캠페인 전체에서 1곳이다(§4.1) |

**대체 검사(이 게임의 G3 실질)**: ① 고갈로 인한 진행 불가 0건 ② 연습·되돌림 무소모 ③ 보상 채널 4종 외 지급 0건 ④ DLC 미소유 시 본편 기능 차이 0 ⑤ 안전 하한 공식 충족. 상세는 `economy/sink-source-ledger.md`, 밴드는 `economy/reward-bands.md`.

> N/A는 "검사하지 않았다"가 아니라 "대상이 존재하지 않는다"이며, 대체 검사 5종은 면제 대상이 아니다.

## 2. 자원 분류 체계

| 등급 | 뜻 | 소모 | 이 게임의 예 |
|---|---|---|---|
| **C 소모성** | 확정 시 잔량이 줄고 한도가 있다 | 있음 | 부식 예산 |
| **W 마모 상태** | 되돌릴 수 없이 누적되지만 진행을 막지 않는다 | 단조 증가 | 재생(원본 취급) 상태 |
| **M 단조 권한** | 열리기만 하고 회수되지 않는다 | 없음 | 열람 권한 |
| **F 플래그 집합** | 이산 상태값, 소모 아님 | 없음 | 증인 확보 상태, propertyProtection |
| **P 파생 지표** | 다른 값에서 계산되며 저장하지 않는다 | 없음 | 청문 신뢰도 |
| **X 미채택** | 제안됐으나 캐논·설계 위험으로 자원화하지 않음 | — | 당직 잔여일 D-21 |

C 등급만 "재화"에 가깝고, 이 게임에는 C가 **1종뿐이며 소모 지점도 1곳**이다.

## 3. 자원 요약표

| # | 자원 | 등급 | 상한 | 회복 | 소스 | 싱크 | 연습 소모 | 확정 소모 |
|---|---|---|---|---|---|---|---|---|
| R1 | 부식 예산 | C | 9 [OBSERVED] | **리셋 없음** · 막다른 구성은 무료 우회관(구성마다 1회)으로 복구 | 캠페인 시작 시 전역 상한 9 부여, 이후 재부여 없음 | `routing` 확정 1회 | **0** | 7 또는 8 |
| R2 | 재생(원본 취급) 상태 | W | 판당 파괴적 절차 3회 [TARGET] | 없음(단조) | — | 원본 직접 절차 확정 | **0** | 1회/절차 |
| R3 | 청문 신뢰도 | P | 필수 100% + 보강 0..N | 재확정 시 즉시 재계산 | 확정 카드 | 없음 | **0** | 0 |
| R4 | 증인 확보 상태 | F | 4인 × 4상태 | 거절도 대체 절차로 회복 | 대화 분기 확정 | 없음 | **0** | 0 |
| R5 | 열람 권한 | M | 구역 5 + 자료 목록 N | 회수 없음 | 확정·대화 | **없음(설계상)** | **0** | 0 |
| R6 | 당직 잔여일(D-21) | X | — | — | — | — | — | **미채택** |

불변식: **연습·프리뷰·되돌림은 어떤 자원도 소모하지 않는다**(`systems/interaction-rules.md` §0.2). 소모는 길게 누름 확정 순간에만 일어나며, 확정 직전 체크포인트가 항상 선행한다(같은 §5).

## 4. 자원별 상세

### 4.1 R1 부식 예산 (C, 유일한 소모성 자원)

> **정본 채택 [OBSERVED] — RFC-P3-009 (`production/decision-log.md`, decided_by game-production-director, 2026-09-10).**
> C3-F4(정본 2개)는 디렉터 판정으로 닫혔고, **이 절의 모델이 부식 예산의 정본**이다: 전역 상한 9 · `routing` 구성안의 총 부식 비용만 소모 · `circuit`/`reader`/`seal` 확정은 무소모 · 누적 고갈·계통 영구 고장·연습 소모 없음.
> 같은 판정이 **"장 경계 리셋"이라는 표현을 폐기**했다 — 장마다 소모가 발생하지 않으므로 되돌릴 잔량이 없고, "리셋"은 존재하지 않는 순환을 있는 것처럼 읽히게 한다. 본 개정에서 이 문서와 `sink-source-ledger.md`의 **자원 규칙으로서의** 해당 표현을 전부 제거했다 — `grep -rn "장 경계" _workspace/current/economy/`는 7행을 돌려주지만 전부 "폐기됐다"는 메타 서술이고 리셋을 규칙으로 주장하는 문장은 0행이다. 폐기 사실은 기록으로 남긴다.
> 진 모델(`balance/balance-sheet.md` §4 6계통 한도 14/14/12/9/9/9 · 도구 확정마다 누적 소모)은 폐기이며, 그 문서의 §4 전면 재작성은 **balance 레인의 작업**이다. economy는 이 절을 인용 대상으로 제공할 뿐 남의 레인 파일을 편집하지 않았다(§8 · `negotiation-record.md` N-13).

- **정의**: `routing` 구성안의 총 부식 비용. 법5 "소금은 비용으로 보인다"의 기계 표현이며 **구매·채집·시간 회복 재화가 아니다**(worldview §3 법5).
- **상한 = 9** [OBSERVED] `systems/prototype/model.mjs:79` `LIMITS.corrosionLimit: 9`.
- **선택지 비용** [OBSERVED] `model.mjs:63-65`: `lowland` 7 · `dock` 8 · `dock-express` 12.
- **가용성 계산**: 9 − 7 = 2, 9 − 8 = 1, 12 − 9 = 3 초과. 즉 **법4의 두 보호 선택(lowland/dock)이 모두 예산 안에서 열린다**. 예산이 한쪽을 강제하지 않는다 — 이것이 이 자원의 유일한 공정성 요건이다.
- **안전 하한 공식**: `corrosionLimit ≥ max(정상 선택지 비용) = 8` → 9 ≥ 8 **충족(여유 1)**. 권장형 `≥ max + 1 = 9` → 9 ≥ 9 **경계 충족(여유 0)**. dock 비용이 9로 오르면 권장형이 깨지고, 10으로 오르면 최소 조건도 깨져 법4가 붕괴한다 → 변경 시 balance/systems RFC 필수.
- **소모 지점**: 캠페인 33비트 중 `routing`을 쓰는 비트는 `c5-b2`·`c5-b4`·`c7-b2` 3개 [OBSERVED] `planning/campaign.json`. 이 중 실제 계통을 바꾸는 것은 **`c5-b4` 1곳뿐**이다 — `c5-b2`는 "가상 운전은 실제 계통을 바꾸지 않는다"[OBSERVED], `c7-b2`는 12년 전 조건의 **시연**이다 [INFERENCE]. `corrosion` 도구 비트(`c2-b2`·`c2-b3`·`c4-b2`)는 전부 판독·시험이라 무소모다.
- **결론**: 부식 예산은 캠페인 전체에서 **한 번만 걸리는 선택 제약**이지 경제가 아니다. 사건이 1회뿐이므로 **리셋·일일 재생·누적 인플레이션 개념이 성립하지 않는다**. 순환이 없는 자원에 "리셋"을 쓰지 않는다(RFC-P3-009).
- **"차감/소모"라는 표현의 지위 — 미해소 문언 차이 [OBSERVED, R4 재측정]**: 이 문서와 `sink-source-ledger.md` §3.1은 `c5-b4` 확정을 **"7 또는 8 차감"**으로 적고, systems `corrosion-budget.md` **L94 C-R9**는 "**부식은 소모되지 않는다.** 확정(`routing` 커밋 포함)도 부식 잔량을 깎지 않는다"고 적는다. RFC-P3-009 본문("`routing` 구성안의 총 부식 비용에 대한 전역 상한 9")은 두 읽기를 모두 허용한다.
  - **관측 가능한 결과는 두 읽기에서 같다**: 확정 게이트는 `configCost ≤ 9` 비교 한 줄이고(`INV3`, `model.mjs:687`), 이후 `routing` 확정 비트가 없으므로(§ ledger §3.1) **어느 읽기로도 막히는 확정은 0건**이다. 그래서 이번 개정은 수치를 바꾸지 않았다.
  - **그러나 문언은 하나여야 한다** — "잔량"이 존재한다는 서술은 폐기된 누적 모델을 UI·저장·텔레메트리에서 되살릴 유인을 만든다(그것이 `operationalCorrosion` 제거의 사유였다). economy는 **C-R9 쪽(비교 게이트, 잔량 없음)** 을 선호하며, 그 경우 ledger §3.1의 "잔여" 열은 "이 시점 구성안 비용" 열로 다시 써야 한다.
  - 판정 요청 → `negotiation-record.md` **N-18 / RFC-E8**(systems·balance·director). **economy가 단독으로 바꾸지 않는다** — RFC-P3-009 문언 해석이므로 디렉터 소유다.
- **계통별 분해 (RFC-P3-009: 조건부 [TARGET] 표시 전용)**: 차단 규칙으로 작동하는 한도는 **전역 단일 9 하나뿐**이다. 계통별 한도 값은 어떤 데이터 파일에도 존재하지 않는다 [OBSERVED] — `model.mjs`의 `LIMITS`에 `corrosionLimit` 단일 키만 있고 계통별 키가 없다.
- **계통별 표시는 저장 필드에 걸지 않는다 — 재정의 (C3-F27(c), 2026-09-10 재측정) [OBSERVED]**: 이전 판은 `save.md:55` `operationalCorrosion: map<systemId,float>`를 "계통별 표시 누계를 담을 v1 저장 자리"로 인용했다. **그 필드는 스키마에서 제거됐다** — `systems/data-schemas/save.md:94` "### `operationalCorrosion` 제거 [RFC-P3-009 · 2026-09-10]", 사유 "부식은 **소모되지 않으므로 누계가 존재하지 않는다**", 대체 규칙 `save.md:102` "부식 상태를 보고 싶으면 `committedRouting`에서 비용을 재계산한다. **저장하지 않는다**". economy는 이 판단에 **동의한다** — 누계 저장 자리를 남겨 두면 RFC-P3-009가 폐기한 누적 모델이 데이터 층에서 되살아난다. 이전 판의 "저장 자리는 있다"는 `[OBSERVED]` 주장은 스테일이었고 **철회한다**.
- **재정의 [OBSERVED 산출식 · TARGET 도입 여부]**: 계통별 분해는 **저장 없는 파생 표시값**이다. 매 `routing` 구성안의 **시험 결과**(`systems/system-specs/corrosion-budget.md` §2 `RunTrial` → `configCost`)를 계통 축으로 쪼갠 값이며, 세이브에는 어떤 형태로도 기록되지 않는다.
  - 산출식: `d_s = Σ { partCost(p) × pathMultiplier(path) | p ∈ 현재 구성안, system(p) = s }`. `corrosion-budget.md` **L90 C-R1**(`cost = Σ partCost(part) × pathMultiplier(path)`, 결정론·난수 없음)의 계통별 부분합이므로 정의상 **`Σ_s d_s = configCost`**다. 차단 비교는 `configCost ≤ 9` 한 줄뿐이고 `d_s`는 비교에 참여하지 않는다.
  - 계통 축의 출처는 저장이 아니라 **정적 데이터**다 [OBSERVED]: `systems/data-schemas/zones.md:36` `systemIds`("이 구역을 지나는 계통"), `plates.md:35` `systemId`. economy는 이 목록을 소유하지 않는다.
  - 재계산 시점: 구성안이 바뀔 때마다(가상 시험은 무제한·무료 — `corrosion-budget.md` L93 C-R4), 슬롯 로드 시에는 `save.md:57` `committedRouting`에서 다시 계산한다(`save.md:102` 대체 규칙과 동일 경로).
  - 세이브 호환: 파생값이므로 **마이그레이션 대상이 아니다**. 표시를 껐다 켜도 세이브는 그대로 열린다 — CLAUDE.md §9의 저장 필드 개명·삭제 축에 걸리지 않는다.
  - 도입 여부 자체는 여전히 **[TARGET]**이다. UI에 계통 분해를 띄울지는 연출·UX 판단이며 이 절은 "띄운다면 이렇게 계산한다"만 고정한다.
- 계통 id 도메인은 `zones.json`의 `systemIds`가 정의하며 economy는 그 목록을 소유하지 않는다. 아래 표의 행 이름은 자리표시자이고, 실제 id 목록·계통별 비용 `c_s`는 systems가 산출한다.

| 계통(자리표시자) | 기호 | 차단 한도 | 표시값 산출 | 저장 | 승격 조건 |
|---|---|---|---|---|---|
| `zones.json.systemIds[*]` | `d_s` (표시값) | **없음 — 전역 9만 차단** | `d_s = Σ(계통 s 부품의 부식 비용)`, 현재 구성안 시험 결과에서 계산 | **저장 없음(파생)** [OBSERVED `save.md:94` 필드 제거] | 차단 한도 `L_s`를 도입하려면 `L_s ≥ c_s(정상 선택지 최대) + 1` |
| 합 | `Σ_s d_s` | 전역 9와만 비교 | `= configCost` (항등 — 별도 집계가 아니다) | **저장 없음(파생)** | 계통 한도를 도입한다면 `Σ_s L_s ≥ 9` (전역보다 조이면 기존 두 선택이 닫힐 수 있다) |

계통별 표시는 "어느 계통이 예산을 먹는지"를 보여주는 UI 라벨이며 **확정 가능성을 바꾸지 않는다**. 차단 규칙으로 승격하려면 (1) 프로토타입 불변식 `INV3`(`model.mjs:687`, 전역 비교 1줄)을 계통별로 일반화하고 (2) 계통별 한도 값을 데이터 테이블에 신설해야 하며, 둘 다 RFC 사안이다. **저장 필드 신설은 그때도 필요하지 않다** — 한도는 정적 데이터, 현재값은 구성안의 함수다.

- **이전 판이 적은 레인 간 불일치는 해소됐다 [OBSERVED, 2026-09-10 재측정]**: `systems/system-specs/corrosion-budget.md`(status: current, owner systems)는 이제 **L91 C-R2 = "한도는 전역 단일 `corrosionLimit = 9` 이며 [tunable: economy]. 계통별 한도는 존재하지 않는다"**, **L94 C-R9 = "부식은 소모되지 않는다"**, §2 "**`systemLimits[systemId]`는 상태 변수가 아니다**"로 정정돼 있다. 이전 판의 "C-R2가 아직 어긋난다"는 `[OBSERVED]` 주장은 **스테일이었고 철회한다**(C3-F27(a)의 economy 측 위치). 후속 질문 Q6도 함께 닫는다(§8).
- **새로 열리는 후속 [OBSERVED]**: 대신 `plates.md:39`와 `plate-readout.md:78`이 아직 `readBudget`을 "economy가 `plateOriginalWear`로 부르는 같은 노브"라는 **별칭**으로 설명한다. §4.2에서 그 이름을 폐기했으므로 이 두 줄의 별칭은 스테일이 된다 → `negotiation-record.md` **N-16**으로 systems에 정정 요청한다(economy는 systems 파일을 편집하지 않았다).

### 4.2 R2 재생(원본 취급) 상태 (W, 진행 비차단)

- **캐논 제약**: 법2 "원본은 닳지만 사본은 남는다". 첫 판독 시 검증 사본이 자동 보존되고 **이후 재생은 사본이며 판독 횟수 제한이 없다**(worldview §3 법2, systems §2.2). 따라서 "재생 횟수를 소모하는 예산"은 **법2 위반이며 채택할 수 없다**.
- **채택안 (옵션 B) — 정본 필드명은 systems 스키마의 2종이다 [OBSERVED, 2026-09-10 재측정]**: 원본에 직접 가하는 **파괴적 절차 횟수** 카운터이며, economy 전용 새 이름을 만들지 않는다. 사본 판독·재생·되돌림·미리보기는 이 값을 건드리지 않는다.
  - **`readCounts`** (누계) — `systems/data-schemas/save.md:54` `map<recordId,int>`, 설명 "법2 **원본에 가한 파괴적 절차 횟수**(상한 3, 비차단). `plates.md` `readBudget`과 짝". `save.md:92` **개명 금지 목록에 포함**돼 있다.
  - **`readBudget`** (상한, 기본 3) — `systems/data-schemas/plates.md:39` `int`, `tunable: **economy**`, 설명 "**원본 상태 카운터의 상한**. 기본 3 (법2). 원본에 직접 가하는 파괴적 절차만 +1 하며 사본 판독은 무제한 — **비차단**".
  - 규칙 본문은 `systems/system-specs/plate-readout.md`가 소유한다: **L55 P-R2**("원본 상태 카운터 `readBudget = 3` [tunable: economy] … 사본 판독·재생·되돌림·미리보기는 이 값을 건드리지 않는다"), **L56 P-R9**("**비차단이다** … 상한 도달 후에도 사본 경로로 모든 필수 확정이 가능"), **L57 P-R10**(UI 표기는 "원본 상태", `readBudget`은 데이터 키로만 남고 화면에 노출되지 않는다).
  - 상한 도달 후의 처리는 `plate-readout.md` **P-F1**(L84, "사유 표시 + **대체 자료 안내**(다른 `sourceType`·다른 루트 `originId` · 힌트 1단계와 동일 정보). 사본 판독은 계속 열려 있다")이며 확정 경로는 남는다 — 이것이 옵션 B가 "비차단"인 이유다.
- **폐기된 이름 (C3-F35 뿌리) [OBSERVED]**: economy가 한때 제안한 `plateOriginalWear`는 **철회됐다**(§6 표 · `negotiation-record.md` N-10, 2026-09-10). 같은 축의 필드가 이미 v1에 있고 개명은 금지이기 때문이다(`save.md:92`). 이전 판이 이 자리에 "채택안 = `plateOriginalWear`"를 남겨 둔 탓에 `planning/gdd.md` ③·`planning/feature-specs/verb-02-plate-read.md` R3a가 **스키마에 없는 필드명**을 확정 인용하게 됐다. 본 개정으로 economy 문서 내부 모순을 없앤다 — 이 이름은 이 레인의 어떤 문서에서도 **지시어로 쓰지 않는다**(폐기 기록으로만 남긴다).

| 옵션 | 내용 | 판정 |
|---|---|---|
| A 표시 전용 | 원본 상태를 게이지로만 표시, 카운터 없음 | 가능하나 c4-b2 "그늘 역산" 같은 절차의 무게가 사라짐 |
| **B 비차단 카운터 (채택)** | 파괴적 절차만 +1, 상한 3, 상한 도달 후에도 **사본 경로로 모든 필수 확정 가능**. 필드 = `readCounts`(누계, `save.md:54`) + `readBudget`(상한, `plates.md:39`) | **채택** — 모델 [TARGET], 필드명 [OBSERVED] |
| C 차단형 예산 | 잔량 0이면 해당 판독 불가 | **거부** — 법2 위반, 진행 불가 경로 생성 |

- **상한 3/판** [TARGET] = `readBudget` 기본값 3(`plates.md:39`, `tunable: economy`) — 근거: 원본 직접 절차가 등장하는 비트는 `c1-b2`(습도 분리), `c4-b2`(그늘 역산) 2회이며 [OBSERVED], 상한은 등장 횟수 + 여유 1 = 3으로 잡는다. 실측 없음. 값 변경은 데이터 테이블 교체만으로 반영돼야 한다(`plate-readout.md` B-P3), 0으로 낮춰도 세 결말 도달이 유지된다는 비차단 증명은 B-P5가 담당한다.
- **효과 범위**: 에필로그 기록 패널의 **보존 등급 문장 1줄**만 바뀐다. 필수 단서·세 결말 접근·힌트·저장에는 영향 0.
- **명칭 위험**: "재생 예산"이라는 이름은 플레이어에게 "아껴 써라"로 읽혀 탐색을 억제한다 [INFERENCE]. UI 표기는 **"원본 상태"**를 권고하고 "예산"이라는 단어는 부식에만 쓴다 → RFC-E3.

### 4.3 R3 청문 신뢰도 (P, 파생·비저장)

- **정의**: 저장하지 않고 계산한다. 두 부분으로 나눈다.
  - `requiredCoverage` = 필수 결론 중 **독립 매체 2종**으로 확정된 비율. 필수 결론 수 = 8 [INFERENCE] (`worldview/timeline.md` §3의 장별 "확정되는 것" 행 8개: T0~C7).
  - `corroborationCount` = 선택적 보강 근거 수 (0..N).
- **왜 게이트가 아닌가**: 33비트는 단일 임계경로이고(`planning/campaign.meta.md` §5.1 [OBSERVED]) 모든 필수 확정에 매체 2종 경로가 임포트 검증으로 강제된다(`systems/unity-implementation.md` §5 불변식 1 [OBSERVED]). 따라서 C7 시점 `requiredCoverage`는 **항상 8/8 = 100%**다. 변동하는 것은 보강분뿐이다.
- **상한**: `requiredCoverage` 100%(구조적 보장), `corroborationCount` 상한 없음.
- **소스**: 확정 카드. **싱크**: 없음(감소하지 않는다). **회복**: 재확정 시 즉시 재계산.
- **영향 범위**: 에필로그 기록 패널 문장 세부 + C7 요약 화면 표기. **세 결말 접근·제출 가능 여부에 영향 0**(법6 "어떤 NPC도 제출을 봉쇄하지 못한다").
- 점수·등급·랭크로 표시하지 않는다. 백분율 배지는 "몇 %를 놓쳤다"는 압박을 만든다 [INFERENCE] → 표기는 "보강 근거 n건".

### 4.4 R4 증인 확보 상태 (F, 플래그)

| 인물 | 상태 집합 | 확보 조건 근거 | 거절 시 대체 |
|---|---|---|---|
| 문재화 | `unmet / conditional / secured` | 출처를 공식 기록에 남기는 조건 (`c1-b1`, `c1-b3`) | 조건 거부해도 `c2-b4`에서 대장 열람 경로가 열린다 [OBSERVED] |
| 표성찬 | `unmet / refused / deal / silent` | 청문 항목 거래 3안(수락/거절/보류) | 세 경우 모두 주민회 경로가 열리고 그의 진술은 **어느 경우에도 단독 확정 근거가 아니다** (`synopsis/scenes-and-dialogue.md:77`) |
| 오은정 | `unmet / conditional / secured` | 조건부 증인 서명 | 서명 없이도 매체 2종 경로로 확정 가능 |
| 한도연 | `unmet / contacted` | 전화 | 진술은 항상 "단서"로만 분류되고 확정 칸에 들어가지 않는다 (`c4-b4` [OBSERVED]) |

- **상한**: 4인 × 상태 1개. **소스**: 대화 분기 확정. **싱크**: 없음. **회복**: 거절 상태에서도 법6의 대체 검증 절차가 열린다.
- **진행 보장**: 증인 **0인 확보**로도 33비트 전부 통과 가능해야 한다 → 검증 규칙 `INV-W0`(ledger §4).

### 4.5 R5 열람 권한 (M, 단조 증가)

- **정의**: 구역 접근(`hub`/`gate`/`lowland`/`dock`/`pump`)과 자료 목록·도구 사용 허가.
- **상한**: 구역 5 + 도구 6 + 자료 목록 항목 N. **소스**: 확정·대화(§ reward-bands 채널 A). **싱크**: **없음 — 권한은 회수되지 않는다**(법1 "필수 권한 박탈 없음").
- **05:00 이관에 대한 명시**: 재화의 대사 "새벽 다섯 시에 계통 권한 넘깁니다"(`scenes-and-dialogue.md:20`)는 **엔딩 사건**이며 플레이 중 권한을 회수하는 타이머가 아니다. 권한 만료 UI·카운트다운을 만들지 않는다.
- **"모든 소스에는 싱크" 원칙 처리**: 이 원칙은 소모성 통화의 인플레이션 통제 규칙이다. M 등급은 **상한 유한 + 단조**로 통제되므로 싱크가 없어도 팽창하지 않는다(권한 총량 = 유한 집합). 이 예외는 등급 M·F·P에만 적용된다.

### 4.6 R6 당직 잔여일 D-21 (X, 미채택 권고)

| 항목 | 내용 |
|---|---|
| 제안 | 폐국까지 남은 일수 21을 자원화해 장마다 차감, 압박 연출 |
| **판정** | **본편 자원으로 채택하지 않는다 [TARGET] 권고** |
| 위험 1 (캐논) | 본편은 **하룻밤 21:00~05:00**이다(`worldview §1`, `campaign.md §1`). D-21은 폐국 **고시** 시점(`timeline.md` §1 `T-0(D-21)`)이며 플레이 기간이 아니다. 일수를 차감하면 QA가 C2에서 이미 지적한 "마지막 당직 vs 매 장 반복" 모순이 되살아난다(`qa/c2-review.md:49`) |
| 위험 2 (설계) | 카운트다운은 읽는 속도에 벌점을 만든다. `systems/interaction-rules.md` §0.1 "전역 실시간 타이머 없음"과 정면 충돌 |
| 위험 3 (저장) | **2026-09-10 재측정으로 상황이 바뀌었다** [OBSERVED]. 정본 세이브 스키마 `systems/data-schemas/save.md:38`은 `stageId`만 두고 **"`dayIndex` 없음(단일 야간)"** 을 명시한다. `dayIndex: 2`가 남은 곳은 `systems/unity-implementation.md:84`(구 예시)뿐이고 `architecture-contract.md` RFC-S3·`save-undo.md` §0가 이미 제거를 제안했다. 따라서 이전 판의 "의미 고정" 권고는 **철회**한다 — 고정할 필드가 정본에 없다. D-21은 저장되지 않으며, 저장이 필요해지는 순간 그것은 스키마 v2 사안이다 |
| 대안 (권고) | D-21을 **비소모 서사 표지**로만 쓴다 — 서류 상단 날짜 스탬프, 폐국 고시문, 인수 목록 머리글. 자원 잔량·차감·경고 UI 없음 |
| 채택하려면 | worldview 연표 개정 + planner 비트 재배치 + systems 저장 스키마 v2가 동시에 필요하다 → 회차 내 처리 불가, season급 결정 |

## 5. baseEntitlement 매트릭스 (DLC 미소유 = 차이 0)

| 항목 | 본편만 소유 | 본편+DLC | 차이 |
|---|---|---|---|
| 도구 6종 | 전부 | 전부 | **0** |
| 결말 3종 + 에필로그 | 전부 | 전부 | **0** |
| 힌트 3단계 | 무료·무제한 | 동일 | **0** |
| 접근성(키 재매핑·자막·색·속도) | 전부 | 동일 | **0** |
| 저장·복구·되돌림 | 전부 | 동일 | **0** |
| 버그 수정·성능 개선·현지화 수정 | 전부 | 동일 | **0** |
| 본편 자원 상한(부식 9 등 — **저장 없음(파생)**, §4.1) | 동일 | 동일 | **0** |
| DLC 독립 사건(감사관·새 구역) | 없음 | 있음 | 증분 콘텐츠 |

- 세이브에 DLC 플래그가 없어도 본편 로드·결말 도달이 가능하고, DLC 삭제 시 본편 진행이 보존된다.
- 이 표는 PM `business-model.md` §7 "기능 인질 금지 목록"과 **동일 내용의 게임 내 검증판**이다. PM은 판매 정책으로, economy는 데이터·세이브로 같은 약속을 지킨다.
- **저장 필드 열의 소재 (C3-F27(c) 안내)**: 자원별 **저장 필드**를 담은 표는 이 절이 아니라 **§6 data_mirror**다. 부식 관련 저장 필드는 §6에서 **"저장 없음(파생)"** 으로 정정됐다 — DLC 소유 여부와 무관하게 부식 상한 9는 **저장되지 않는 비교 상수**이므로 위 표의 차이 0은 세이브 필드가 아니라 데이터 테이블로 보장된다.

## 6. data_mirror 제안 (systems 조율 필요, 본 레인은 값만 소유)

**2026-09-10 재측정 [OBSERVED]**: `systems/data-schemas/save.md`(current, systems 소유)가 이미 v1으로 존재하며, 이전 판이 "제안"이라 적은 필드 중 **셋은 이미 v1에 있다.** 없는 것만 v2 요청으로 남긴다.

**이번 개정에서 바뀐 것 (C3-F27(c))**: 이전 판은 R1(부식)에 저장 필드 `operationalCorrosion`이 v1에 **존재한다**고 적었다. 그 필드는 `save.md:94`에서 **제거**됐으므로 해당 행을 **"저장 없음(파생)"** 으로 정정한다. 따라서 v1 존재 필드는 넷이 아니라 **셋**(`bypassUsed`·`readCounts`·`witnessChoices`)이다.

| 자원 | 정본 v1 필드 (`save.md`) | 형 | 상태 | 비고 |
|---|---|---|---|---|
| R1 (부식 잔량·계통 누계) | ~~`operationalCorrosion`~~ | — | **저장 없음(파생)** — v1에서 **제거** (`save.md:94`, RFC-P3-009) | 부식은 소모되지 않아 누계가 없다. 상태가 필요하면 `committedRouting`(`save.md:57`)에서 매번 재계산한다(`save.md:102`). 계통별 분해도 같은 이유로 파생 표시값(§4.1) |
| R1 (우회관 이력) | `bypassUsed` | string[] | **v1 존재** (`save.md:55`) | **구성 id 배열** — 우회관이 구성마다 1회임을 데이터가 이미 말한다. `save.md:103`이 "소모가 아니라 사용 이력"으로 **유지**를 명시 |
| R2 | `readCounts` | map<recordId,int> | **v1 존재** (`save.md:54`) | 법2 **원본에 가한 파괴적 절차 누계**. 상한은 저장이 아니라 `plates.md:39` `readBudget`(기본 3, `tunable: economy`)이다. 철회된 이름 `plateOriginalWear`는 쓰지 않는다(§4.2, C3-F35) |
| R4 | `witnessChoices` | map<conclusionId,personId> | **v1 존재** | §4.4 상태 집합(`unmet/…`)을 담기에는 축이 다르다 → 상태 열거는 v2 요청(아래) |
| R5 | `accessGranted` | string[] | **미존재** | 구역·도구 개방 이력. v2 요청 |
| R4 | `witnessState` | map<npcId,enum> | **미존재** | §4.4 4상태. v2 요청 |
| R3 | — | — | — | **저장하지 않는다**(파생) |

- v1에 이미 있는 필드는 **개명·의미 변경을 요청하지 않는다**. `save.md:92` 개명 금지 목록의 현재 실물은 `schemaVersion`·`saveId`·`stageId`·`beatId`·`autoKeptClueIds`·**`readCounts`**·`propertyProtection`·`sealedConclusions`·`submissionPerspective`·`hintLevelUsed`·`checkpoints`·`dlcFlags`·`commandLog.headSeq`·`commandLog.entries`·`checksum` **15종**이며 **`operationalCorrosion`은 (제거됐으므로) 목록에 없다** [OBSERVED, 2026-09-10 재측정] — 이전 판이 그 이름을 금지 목록 구성원으로 적은 것은 스테일이었고 정정한다. 새 필드 2종(`accessGranted`·`witnessState`)만 **v2 + 마이그레이터**로 요청한다(저장 호환 불변식, CLAUDE.md §9).
- **제거된 필드에는 마이그레이션 부채가 없다** [OBSERVED]: `save.md`가 "출시 0회·빌드 0회·임포터 0줄·표본 n=0 — 이 스키마를 읽은 세이브 파일이 세상에 존재하지 않는다"로 호환 위험을 **없음**으로 판정했다. economy는 이 판단을 그대로 인용하며, 출시 이후에는 같은 삭제가 마이그레이터 1개를 요구한다는 점도 함께 기록한다.
- 수치는 코드가 아니라 데이터 테이블에 산다. 현재 한도 9는 `model.mjs`의 `LIMITS` 상수이며, Unity 이관 시 ScriptableObject/JSON으로 옮기는 것이 systems의 과제다.

## 7. 미측정 [OBSERVED]

플레이 표본 n=0. 부식 한도 9가 실제로 "선택이 무겁게 느껴지는" 값인지, 원본 마모 카운터가 탐색을 억제하는지, 증인 0인 완주가 답답한지 — **전부 측정되지 않았다**. 위 숫자 중 [OBSERVED]는 프로토타입 코드 상수뿐이고, 그 상수가 재미의 증거는 아니다.

## 8. 열린 질문

- **Q1** `economy/resources-and-fairness.md`(c4 draft)를 archive-cycle로 아카이빙하고 본 문서가 `supersedes`로 잇게 할지 — 디렉터 판단.
- **Q2** `c7-b2`의 `routing` 시연이 부식을 실제 차감하는가, 무소모 시연인가 — planner/systems 확인 필요.
- ~~**Q3** 계통별 부식 분해를 C4에서 도입할지, 전역 9를 유지할지~~ → **판정 완료: RFC-P3-009 (2026-09-10)**. 전역 9가 유일한 차단 한도, 계통별은 조건부 [TARGET] 표시 전용. 후속 항목은 Q6.
- ~~**Q6** `systems/system-specs/corrosion-budget.md` C-R2(계통별 한도 차단)를 RFC-P3-009 정본에 맞춰 정정하는 시점~~ → **닫힘 (2026-09-10 재측정, 대상 해소)**: 실물 C-R2(L91)는 이미 "전역 단일 `corrosionLimit = 9` … 계통별 한도는 존재하지 않는다"이고 C-R9(L94)가 "부식은 소모되지 않는다"를 명시한다. N-14는 이행 확인으로 종결.
- **Q7 (신규)** `plates.md:39`·`plate-readout.md:78`이 `readBudget`을 **폐기된 이름 `plateOriginalWear`의 별칭**으로 설명하는 두 줄을 언제 정정하는가 — systems (N-16). 방치하면 C3-F35가 systems 문서 경로로 되살아난다.
- **Q4** `LIMITS.maxUndo: 32` [OBSERVED]와 "되돌림 무제한" 서술의 불일치 — ledger §5 RFC-E1.
