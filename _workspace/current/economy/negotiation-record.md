---
updated: 2026-09-10
cycle: 20260909-preproduction-c3
status: current
supersedes: null
owner: game-economy-designer
---

# 협상 기록 — 경계 확인과 미해결 항목

## 0. 서명 규약

- 한 항목은 **economy 서명 + 상대 레인 서명**이 모두 있을 때 "합의"다. 이번 회차에서 economy가 먼저 기록했고 상대 서명은 **전부 미수령**이다. 미수령을 합의로 적지 않는다.
- 상태값: `제안(economy 단독)` / `합의(양측)` / `이견` / `보류` / **`디렉터 판정`**(RFC로 확정 — 상대 레인 서명 없이도 구속력이 있으며, 서명 대신 판정 id를 적는다).
- 플레이 수치를 건드리는 항목은 balance·planner 서명 없이 데이터에 반영하지 않는다.

## 1. PM ↔ economy 경계 (핵심)

| 구분 | PM (`game-product-manager`) 소유 | economy 소유 |
|---|---|---|
| 값의 성격 | 실제 돈 | 게임 내 상태 |
| 예 | 표시가·할인율·환불·배분율·DLC 가격·손익분기 | 부식 한도·권한 개방·보상 채널·힌트 비용·세이브 자원 필드 |
| 문서 | `product/business-model.md` | `economy/*.md` |
| 게이트 | 상품 판단 (D 계열) | G3 대체 검사 5종 |
| 금지 | 게임 내 공정성 규칙을 가격 논리로 바꾸지 않는다 | 가격·수익·판매량을 이 레인에서 정하지 않는다 |

**공통 경계면 3곳** — 두 레인이 같은 약속을 서로 다른 수단으로 지킨다.

| 경계 | PM 수단 | economy 수단 | 상태 |
|---|---|---|---|
| 기능 인질 금지 | 판매 정책상 DLC 뒤로 옮기지 않겠다는 선언 (`business-model.md` §7) | `baseEntitlement` 매트릭스로 차이 0을 데이터·세이브에서 강제 (`currency-map.md` §5) | **제안** |
| 힌트 무료 | 과금 지표에서 힌트 제외 (§1 "시간·회차·힌트·저장은 과금 대상이 아니다") | `hint.price: 0`, 무제한, 결말 영향 0 (`reward-bands.md` §3) | **제안** |
| DLC 독립성 | 본편 결말을 DLC로 미루지 않음 (§7) | 세이브에 DLC 플래그 없어도 본편 로드·결말 도달 (`currency-map.md` §5) | **제안** |

## 2. 기록 항목

### N-01 · PM · 유료 재화 부재 확인

- **사안**: G3 표준 항목(유료/무료 격차, 인플레이션)을 N/A로 처리해도 되는가.
- **economy 입장**: 된다. 단 "대상 부재"라는 사유를 명시하고, 대체 검사 5종을 면제하지 않는다.
- **근거**: `gdd.md` 범위 "제외: 전투, 멀티플레이, 유료 통화, 가챠, 시즌패스" [OBSERVED] · `business-model.md` §8 "생산 실체 전 금지" [OBSERVED] · `premium-preproduction-contract.md` "Premium overrides" [OBSERVED].
- **상태**: `제안(economy 단독)`. PM 서명 미수령.

### N-02 · PM · 9,000원 하한은 economy 사안이 아니다

- **사안**: 실결제 하한 9,000원을 게임 내 자원 설계가 참조해야 하는가.
- **economy 입장**: **참조하지 않는다.** 가격 하한은 판매 정책이며, 게임 내 자원·보상은 가격과 독립적으로 정해진다. 가격이 바뀌어도 부식 한도나 힌트 정책은 바뀌지 않는다.
- **역방향 금지**: "가격이 높으니 콘텐츠 자원을 조여 체감 분량을 늘린다"는 논리는 이 레인에서 **거부**한다. 시간 인위 연장 금지는 `campaign.meta.md` §5.7과 같은 규율이다.
- **상태**: `제안(economy 단독)`.

### N-03 · PM · DLC 가격과 게임 내 권한의 분리

- **사안**: DLC 5,900~7,900원 후보(`business-model.md` §7)가 본편 자원·권한에 영향을 주는가.
- **economy 입장**: 영향 0이어야 한다. `baseEntitlement` 매트릭스 8행 전부 차이 0(`currency-map.md` §5).
- **검증 요청**: DLC 미소유 세이브로 본편 3결말 도달 테스트를 QA 항목으로 등록. 현재 테스트 0건 [OBSERVED].
- **상태**: `제안(economy 단독)`. QA 등록 미완.

### N-04 · balance · 부식 한도 9의 소유권과 변경 규칙

- **사안**: `LIMITS.corrosionLimit: 9`와 선택지 비용 7·8·12는 누가 바꿀 수 있는가.
- **economy 입장**: 값 자체는 balance/systems가 조정하되, **안전 하한 공식**(`limit ≥ max(정상 선택지 비용)`)은 economy가 소유하는 공정성 규칙이다. 세 값 중 하나라도 바꿀 때 RFC를 연다.
- **현재 계산** [OBSERVED, 2026-09-10 재측정 `model.mjs:63-65`·`:79`]: 9 ≥ 8 충족(여유 1). 권장형 9 ≥ 9 경계 충족(여유 0). `dock`이 9가 되면 권장형이, 10이 되면 최소 조건이 깨진다.
- **왜 중요한가**: 이 한 줄이 깨지면 법4의 제로섬 선택이 "가치 판단"에서 "예산이 강제하는 단일 해답"으로 변질된다.
- **상태**: **`디렉터 판정`(RFC-P3-009)**. 판정이 전역 상한 9 모델을 정본으로 확정하면서 이 항목의 전제(단일 `limit` 값이 존재한다)가 함께 확정됐다. 안전 하한 공식의 소유권은 economy로 유지되며, 값 조정 시 RFC 조건도 유지된다. balance의 개별 서명은 여전히 미수령이지만 **판정이 서명을 대체한다**.

### N-05 · balance · 난도와 자원의 분리

- **사안**: 퍼즐 난도를 자원 압박으로 만들 것인가.
- **economy 입장**: 만들지 않는다. `puzzle-balance.md`는 난도를 전제 수·자료 수·인과 단계·도구 조합으로 정의했고 [OBSERVED], 자원 잔량은 그 축에 없다. 자원으로 난도를 만들면 힌트 무료 원칙과 충돌한다(힌트로는 자원을 되돌릴 수 없으므로).
- **상태**: `제안(economy 단독)`. balance 문서와 **모순 없음**으로 읽힌다.

### N-06 · planner · 보상 채널 필드 추가 요청 (RFC-E6)

- **사안**: `campaign.json` 33비트에 `rewardChannels` 필드를 추가할 것인가.
- **economy 입장**: 추가를 요청한다. 현재 보상 분류는 `consequence` 원문을 사람이 읽어 붙인 [INFERENCE]이며, 필드가 생기면 밴드 검사가 임포트 검증기에서 자동화된다.
- **주의**: JSON은 planner 소유다. economy는 값 제안만 하고 직접 쓰지 않았다.
- **상태**: `제안(economy 단독)`.

### N-07 · synopsis/planner · C5·C6 대화 보상 공백 (RFC-E7)

- **사안**: D(대화 분기) 채널이 C5·C6 두 장(135분) 연속으로 0건이다 [INFERENCE, `reward-bands.md` §4.3].
- **economy 입장**: 의도라면 문서에 그렇게 적고, 의도가 아니라면 최소 1비트를 보강한다. 후반 인물 존재감이 사라지는 것은 자원 문제가 아니라 보상 리듬 문제이며 economy가 먼저 발견했을 뿐 판단은 서사 레인의 것이다.
- **상태**: `제안(economy 단독)`.

### N-08 · systems · 자원 고갈 방향 불변식 부재 (RFC-E4)

- **사안**: 프로토타입 INV1~INV9 중 자원 관련은 INV3(초과 차단) 하나뿐이고, **부족해서 막히는 방향**을 검사하는 불변식이 없다 [OBSERVED] `systems/prototype/model.mjs:670-746`.
- **economy 입장**: INV10(자원 잔량만을 이유로 확정이 영구 불가능해지는 경로 없음)과 INV11(연습·프리뷰·되돌림은 자원 벡터 불변)을 추가한다. 이것이 없으면 G3 대체 검사 ①은 설계 주장에 머문다.
- **상태**: `제안(economy 단독)`.

### N-09 · systems · 되돌림 상한 32 vs "무제한" (RFC-E1)

- **사안**: `LIMITS.maxUndo: 32` [OBSERVED] 와 `interaction-rules.md` §0.2 "되돌림 무제한"이 다르다.
- **economy 입장**: 되돌림은 economy가 의존하는 **무료 복구 소스**다. 32단이 실제 상한이라면 문서를 "되돌림 이력 32단"으로 정정하고, 32단을 넘겨야만 풀리는 막다른 상태가 있는지 INV10 탐색으로 확인한다.
- **상태**: `제안(economy 단독)`.

### N-10 · systems · 저장 스키마 v2와 자원 필드 (data_mirror)

- **사안**: `corrosionSpent` `bypassUsed` `plateOriginalWear` `witnessState` `accessGranted` 5개 필드 추가.
- **2026-09-10 재측정으로 요청이 줄었다** [OBSERVED] — `systems/data-schemas/save.md`(current)를 실제로 열어보니 셋이 이미 v1에 있었다. 따라서
  - `corrosionSpent` 신설 요청 **철회**.
  - `plateOriginalWear` 신설 요청 **철회** → `readCounts` 사용(같은 축의 필드를 둘 만들지 않는다).
  - 남는 v2 요청은 **`accessGranted`** 와 **`witnessState`** 2종뿐이다.
- **재측정 2 (R4, 2026-09-10) — 위 항목 중 하나가 다시 바뀌었다 [OBSERVED]**: `operationalCorrosion`은 **v1에서 제거**됐다(`save.md:94` "### `operationalCorrosion` 제거 [RFC-P3-009 · 2026-09-10]"). 그러므로
  - `corrosionSpent` 철회의 대체는 "`operationalCorrosion` 사용"이 아니라 **"저장하지 않는다 — `committedRouting`(`save.md:57`)에서 재계산"**(`save.md:102`)이다. economy는 이 처리에 **동의**하며 부식 저장 필드를 **요청하지 않는다**.
  - v1에 실제로 있는 자원 관련 필드는 **셋**(`bypassUsed` `save.md:55` · `readCounts` `save.md:54` · `witnessChoices` `save.md:60`)이고, 그중 economy 자원 축에 **그대로 쓰는 것은 둘**이다(`witnessChoices`는 축이 달라 §4.4 상태 열거용 `witnessState`를 v2로 계속 요청한다). `currency-map.md` §6 표와 같은 값이다.
  - 계통별 표시는 저장 없는 **파생값**으로 재정의했다(`currency-map.md` §4.1). 표시 기능을 켜기 위한 저장 필드도 요구하지 않는다.
- **economy 입장**: 추가는 v2 + 마이그레이터로만. **기존 필드 개명 금지** — `save.md:92` 개명 금지 목록 15종에 `readCounts`가 포함돼 있다 [OBSERVED]. (`operationalCorrosion`은 제거된 필드라 그 목록에 **없다** — 이전 판이 구성원으로 적은 것은 스테일이었고 정정한다.) 청문 신뢰도는 파생값이므로 저장하지 않는다.
- **부가 확인 (이전 판 정정)**: 이전 판은 "v1에 `"dayIndex": 2`가 있다"며 의미 고정을 권고했다. **정본 스키마에는 그 필드가 없다** — `save.md:38`이 "**`dayIndex` 없음**(단일 야간)"을 명시한다 [OBSERVED]. `dayIndex`가 남은 곳은 `systems/unity-implementation.md:84`의 구 예시뿐이고 `architecture-contract.md` RFC-S3가 제거를 제안 중이다. 따라서 **"의미 고정" 권고를 철회**한다. economy는 D-21을 저장하지 않는다.
- **상태**: `제안(economy 단독)`, 요청 범위 축소.

### N-11 · worldview · "재생 예산" 명칭 (RFC-E3)

- **사안**: "재생 예산"은 법2(사본 무제한 재생)와 충돌하게 읽힌다.
- **economy 입장**: 기계는 유지하되(파괴적 절차 카운터, 비차단) UI 표기는 **"원본 상태"**로 한다. "예산"이라는 단어는 부식에만 쓴다. 용어집 후보표 반영은 worldview 소유.
- **상태**: `제안(economy 단독)`.

### N-12 · worldview/planner · 당직 잔여일 D-21 미채택

- **사안**: D-21을 게임 내 자원으로 만들 것인가.
- **economy 입장**: **미채택 권고.** 본편은 하룻밤(21:00~05:00)이고 D-21은 폐국 고시 시점이다. 자원화하면 QA가 C2에서 지적한 시간축 모순(`qa/c2-review.md:49`)이 되살아나고, "전역 실시간 타이머 없음"(`interaction-rules` §0.1)과 충돌한다.
- **대안**: 비소모 서사 표지(날짜 스탬프·고시문·인수 목록 머리글)로만 사용.
- **채택하려면**: 연표 개정 + 비트 재배치 + 저장 스키마 변경이 동시에 필요 → 이번 회차 범위 밖.
- **상태**: `제안(economy 단독)`. worldview·planner 판단 필요.

### N-13 · balance · 부식 예산 정본 판정 결과 기록 (C3-F4 / RFC-P3-009)

- **사안**: 이 게임 유일한 소모 자원의 정본이 둘이었다. balance = 6계통 한도(`brine_line 14 · power_bus 14 · reader_head 12 · seal_press 9 · gate_valve 9 · pump_motor 9`) · 도구 확정마다 누적 소모 · 리셋 없음 / economy = 전역 단일 9 · `routing` 확정 1회만 소모 · 장 경계 리셋.
- **판정 [OBSERVED]**: `production/decision-log.md` **RFC-P3-009**, decided_by `game-production-director`, date 2026-09-10. **economy/세션 P 모델이 정본.** 근거로 인용된 문서: `systems/interaction-rules.md` §2.5, `economy/resources-and-fairness.md`, `systems/prototype/model.mjs:79`, `systems/game-ui-contract.json`, `messages/003` C2, `planning/campaign.json` corrosion 비트.
- **판정이 economy에게 지운 의무 2가지** — 이번 회차에 둘 다 이행했다.
  1. "장 경계 리셋" 표현 폐기 → `currency-map.md` §3·§4.1, `sink-source-ledger.md` §1·§2·§3.1에서 제거. 확인: `grep -rn "장 경계" _workspace/current/economy/` → **7행이 남지만 전부 "폐기됐다"는 메타 서술**(RFC 인용 3 · 이행 기록 2 · 검증 명령 1 · 판정 사안 서술 1)이고, **리셋을 자원 규칙으로 주장하는 문장은 0행**이다. 폐기 사실 자체는 기록으로 남겨야 하므로 단어를 지우지 않았다.
  2. 계통별 분해를 **조건부 [TARGET] 표시 전용**으로만 유지 → `currency-map.md` §4.1 표를 차단 한도 없음 형태로 재작성. **(R4 재개정, 2026-09-10)** 그 재작성은 표시 누계를 `operationalCorrosion` 저장 필드에 걸었는데, 그 필드가 스키마에서 제거되면서 **저장 없는 파생 표시값**으로 다시 정의했다(C3-F27(c), `currency-map.md` §4.1 R4-1). 판정 자체(전역 9·표시 전용)는 그대로다.
- **경계 (중요)**: `balance/balance-sheet.md` §4 전면 재작성은 **balance 레인의 작업**이다. economy는 그 파일을 **읽기만 했고 편집하지 않았다**. 판정으로 내 모델이 이겼다는 사실이 남의 문서를 고칠 권한을 주지 않는다. 이번 회차 종료 시점 `balance-sheet.md` §4는 아직 6계통 모델을 담고 있다 [OBSERVED 2026-09-10] — 이는 economy의 미이행이 아니라 **balance의 열린 작업**이며 QA가 F4 종결을 판단할 때 두 문서를 함께 봐야 한다.
- **역방향 금지**: 판정이 나왔다고 해서 balance의 20% 안전 잔여율·40% 재작업 여유 같은 **난도 규율까지 폐기되는 것은 아니다.** 폐기된 것은 "부식이 계통별로 누적 고갈된다"는 자원 모델이지, 오확정에 여유를 두라는 설계 원칙이 아니다. 그 원칙을 전역 9 모델에 어떻게 옮길지는 balance가 결정한다.
- **상태**: **`디렉터 판정`(RFC-P3-009)**. economy 측 이행 완료, balance 측 이행 대기.

### N-14 · systems · `corrosion-budget.md` C-R2가 판정과 어긋난다

- **사안** [OBSERVED, 2026-09-10 재측정]: `systems/system-specs/corrosion-budget.md`(status: current, owner systems) §2 상태 변수 `systemLimits[systemId]`, §3 **C-R2 "한도는 계통별(`systemLimits`)이며 [tunable: balance]"**, §4 C-F3 "데이터의 한도가 어떤 유효 구성으로도 만족 불가 → 임포트 실패". 이 스펙은 **계통별 한도를 차단 규칙으로** 기술한다.
- **왜 문제인가**: RFC-P3-009는 차단 한도를 **전역 9 하나**로 확정하고 계통별은 표시 전용으로 내렸다. 이 스펙대로 구현되면 C3-F4가 데이터가 아니라 **코드에서 재발**한다. 결함 등록부의 F4 항목은 `balance-sheet.md`와 `currency-map.md`만 지목했고 이 파일은 목록에 없었다.
- **economy 제안**: C-R2를 "한도는 전역 단일(`corrosionLimit`)이며 [tunable: balance]. `systemLimits`는 표시 분해용이며 확정 게이트를 좌우하지 않는다"로 정정. `operationalCorrosion: map<systemId,float>`(`save.md:55`)는 **그대로 유지** — 누계 표시는 판정과 모순되지 않는다.
- **주의**: `systems/` 파일은 systems 소유다. economy는 편집하지 않았고 제안만 남긴다.
- **종결 (R4, 2026-09-10 재측정) [OBSERVED]**: systems가 이미 이행했다 — `corrosion-budget.md` **L91 C-R2 = "한도는 전역 단일 `corrosionLimit = 9` 이며 [tunable: economy]. 계통별 한도는 존재하지 않는다"**, **L94 C-R9 = "부식은 소모되지 않는다"**, §2 "`systemLimits[systemId]`는 상태 변수가 아니다". 다만 economy가 그때 함께 제안한 "`operationalCorrosion`(`save.md:55`)은 그대로 유지" 문장은 **철회한다** — systems는 그 필드를 제거하는 더 강한 처리를 택했고(`save.md:94`), 그 편이 폐기된 누적 모델의 재발 경로를 아예 없애므로 economy도 동의한다.
- **상태**: **`이행 확인(종결)`** — economy 제안 → systems 이행 완료. 후속은 N-16.

### N-15 · systems · 텔레메트리 계약에 자원 키가 없다 (C3-F15 / RFC-E5 개정)

- **이전 판의 오류 [OBSERVED]**: `sink-source-ledger.md` §6은 "`systems/ops/`는 빈 디렉터리이며 `telemetry-contract.md`가 없다 → G3는 PASS할 수 없다"고 적었다. `ls` 결과 **파일은 존재한다**(7.6K, cycle c3, current, owner systems). 부재를 근거로 삼은 문장을 삭제하고 §6 전체를 다시 썼다.
- **다시 세운 사실**: 계약은 시간·진행·힌트·연습/되돌림·AFK·세이브·성능 7군을 정의하지만 **자원군이 0건**이다. `grep -c "econ\." telemetry-contract.md` → **0**, `grep -n "부식\|corrosion\|자원\|권한\|증인"` → **0행**. `system-specs/corrosion-budget.md` §6이 정의한 `corrosion_*` 5키도 계약에 등재돼 있지 않다 — **스펙에만 있고 계약에 없는 키는 수집되지 않는다.**
- **결론은 유지된다**: G3는 여전히 PASS할 수 없다. 이유가 (파일 부재) → (자원 키 미등재 B1 + 표본 n=0 B2 + INV10/INV11 미구현 B3)로 **바뀌었을 뿐**이다. 세 이유는 독립이며 하나만 닫혀도 통과하지 않는다.
- **요청**: 계약 §8이 "키 추가는 자유"라 했으므로 **버전 증가 없이 추가 가능**하다. `sink-source-ledger.md` §6.3의 11키 + `corrosion-budget.md` §6의 5키를 systems가 한 번에 정리해 등재한다.
- **상태**: `제안(economy 단독)`.

### N-16 · systems · `readBudget`을 폐기된 이름 `plateOriginalWear`의 별칭으로 설명하는 두 줄 (C3-F35 후속)

- **사안** [OBSERVED, 2026-09-10 재측정]: economy가 §4.2에서 `plateOriginalWear`를 지시어로 폐기했으므로, 그 이름을 **현행 별칭**으로 설명하는 systems 두 줄이 스테일이 된다.
  - `systems/data-schemas/plates.md:39` — "…**비차단**(`economy/currency-map.md` §4.2 옵션 B = `plateOriginalWear`와 같은 노브)".
  - `systems/system-specs/plate-readout.md:78` — "`readBudget`(= economy가 `plateOriginalWear` 상한으로 부르는 **같은 노브**)".
- **왜 지금 올리는가**: C3-F35는 "기획서가 스키마에 없는 필드명을 지시한다"는 결함이었다. economy 문서에서 이름을 지워도 systems 문서가 그 이름을 **정당한 동의어로 소개**하면 핸드오프에서 Codex가 다시 그 이름을 쓸 근거가 남는다. 결함의 뿌리(economy §4.2)는 닫혔지만 재유입 경로가 하나 남는다.
- **economy 제안**: 두 줄의 괄호를 `(economy `currency-map.md` §4.2 옵션 B의 상한 — economy 측 별칭 없음)` 류로 바꿔 **동의어를 제거**한다. 값·규칙·`tunable: economy` 표기는 그대로. 2줄, 값 변경 0.
- **주의**: `systems/` 파일은 systems 소유다. economy는 **읽기만 했고 편집하지 않았다**.
- **상태**: `제안(economy 단독)`. systems 확인 필요 (`currency-map.md` §8 Q7).

### N-17 · planner · `plateOriginalWear` 인용 교체 요구 (C3-F35 economy 측 이행 완료)

- **사안**: `planning/gdd.md` ③(L112 부근)과 `planning/feature-specs/verb-02-plate-read.md` R3a(L39 부근)가 카운터 이름을 `plateOriginalWear`로 확정 인용한다. 근거로 삼은 것은 economy `currency-map.md` **개정 전 L101** "채택안 (옵션 B) `plateOriginalWear`"였다(현재 위치 §4.2) [OBSERVED, `qa/c3-review.md` C3-F35].
- **economy 이행 (R4, 2026-09-10)**: 그 근거 줄을 **교체 완료**했다 — `currency-map.md` §4.2 채택안 = **`readCounts`(누계, `save.md:54`) + `readBudget`(상한 3, `plates.md:39`)**, 근거는 systems 실제 스키마 인용(`plate-readout.md` P-R2·P-R9·P-R10). 문서 내부 모순(L101 vs L179)은 사라졌다.
- **planner에게 남는 작업**: 위 두 곳의 이름을 `readCounts`(누계)/`readBudget`(상한)로 교체. `gdd.md` L118·L280의 `[counter → 디렉터]`는 QA 실측대로 **"모델은 일치하고 이름만 달랐다"** 로 축소하면 된다(`plate-readout.md` P-R9·P-R11).
- **economy 경계**: `planning/` 파일은 planner 소유다. economy는 **편집하지 않았다**. 순서는 QA 요구대로 economy 먼저 → planner.
- **상태**: `제안(economy 단독, 선행 조건 이행 완료)`. planner 서명 대기.

### N-18 · systems / balance / director · "차감"인가 "비교"인가 (RFC-E8, R4 신규)

- **사안** [OBSERVED, 2026-09-10 재측정]: 같은 자원을 두 문언이 다르게 기술한다.
  - economy: `currency-map.md` §4.1 "소모 지점 … `c5-b4` 확정", `sink-source-ledger.md` §3.1 표 "진입 시 잔여 9 → 소모량 7\|8 → 종료 잔여 2\|1".
  - systems: `corrosion-budget.md` **L94 C-R9** "**부식은 소모되지 않는다.** 확정(`routing` 커밋 포함)도 부식 잔량을 깎지 않는다. 확정이 하는 일은 '이 구성안이 상한 안인가'를 통과시키는 것뿐이다" · §2 "**상태기계에 없는 것**: 잔량 변수, 차감 전이".
  - 디렉터 판정 RFC-P3-009 본문("`routing` 구성안의 총 부식 비용에 대한 전역 상한 9")은 **두 읽기를 모두 허용**한다.
- **관측 결과는 같다 [OBSERVED]**: 확정 게이트는 `configCost ≤ 9` 비교 1줄(`INV3`, `model.mjs:687`)이고 C5 이후 `routing` 확정 비트가 없으므로(ledger §3.1) **어느 읽기로도 막히는 확정은 0건**이다. 그래서 economy는 이번 회차에 **수치를 한 줄도 바꾸지 않았다.**
- **그래도 올리는 이유**: "잔량이 존재한다"는 문언은 폐기된 누적 모델을 UI 게이지·저장 필드·텔레메트리 키에서 되살릴 유인을 만든다. 그것이 systems가 `operationalCorrosion`을 제거한 사유였고(`save.md:94`), C3-F27(c)·C3-F4가 같은 뿌리에서 나왔다. 문언을 하나로 만들지 않으면 **세 번째 회차에 같은 유형이 다시 나온다** [INFERENCE].
- **economy 선호**: **C-R9 쪽(비교 게이트·잔량 없음)**. 채택되면 economy가 이행할 것 — `currency-map.md` §3 요약표 "확정 소모" 열 → "확정 시 비교되는 비용", §4.1 "소모 지점" → "비교 지점", `sink-source-ledger.md` §3.1의 잔여 2열 → "그 시점 구성안 비용" 1열, `reward-bands.md` `sink_events_per_campaign` → `gate_checks_per_campaign`. **판정 전에는 바꾸지 않는다** — RFC-P3-009 문언 해석은 디렉터 소유다.
- **상태**: `제안(economy 단독)` · 디렉터 판정 대상. 관련 결함 없음(QA 미등록, R4에서 economy가 자기 문서 대조로 발견).

## 3. 열린 질문 (디렉터 판정 대상)

| # | 질문 | 막히면 생기는 일 |
|---|---|---|
| Q1 | `economy/resources-and-fairness.md`(c4 draft)를 아카이빙하고 `currency-map.md`가 잇게 할지 | 같은 주제의 문서 2개가 남아 어느 쪽이 정본인지 모호해진다 |
| Q2 | `c7-b2`의 `routing` 시연이 부식을 실제 차감하는가 | 캠페인 총 소모가 1회인지 2회인지 확정되지 않는다 |
| ~~Q3~~ | ~~계통별 부식 분해를 도입할지, 전역 9를 유지할지~~ | **판정 완료 — RFC-P3-009(2026-09-10): 전역 9 유지, 계통별은 표시 전용.** 후속은 Q6 |
| ~~Q6~~ | ~~`systems/system-specs/corrosion-budget.md` C-R2를 언제 정본에 맞추는가~~ | **닫힘 (R4, 2026-09-10 재측정)** — C-R2(L91)·C-R9(L94)가 이미 정본이다. N-14 종결 |
| Q7 | `plates.md:39`·`plate-readout.md:78`의 `plateOriginalWear` 별칭 2줄을 언제 지우는가 | C3-F35가 systems 문서 경로로 재유입된다 (N-16) |
| Q8 | 부식 확정이 **차감**인가 **비교**인가 (RFC-P3-009 문언 해석) | 문언이 둘로 남아 폐기된 누적 모델이 UI·저장·텔레메트리에서 재발할 유인이 남는다 (N-18) |
| Q4 | C5·C6 대화 보상 공백이 의도인가 | 후반 인물 존재감에 대한 판단이 유보된다 |
| Q5(개정) | 텔레메트리 계약에 **자원 키군을 언제 등재하는가** — 계약 파일 자체는 이미 존재한다 [OBSERVED] | 자원 키가 없으면 빌드가 생겨도 G3 대체 검사 5종이 수집되지 않는다 (N-15) |

## 4. 이번 회차 미실행 [OBSERVED]

- 다른 레인 문서를 **편집하지 않았다**. `systems/`, `planning/`(`campaign.json`·`gdd.md`·`feature-specs/` 포함), `worldview/`, `balance/balance-sheet.md`, `qa/`, `production/decision-log.md`는 읽기만 했다. RFC-P3-009로 economy 모델이 정본이 됐지만 그것이 balance·systems·planner 파일 편집 권한을 주지 않는다(N-13·N-16·N-17).

### 4.0 R4 회차 처리 기록 — C3-F27(c) · C3-F35 [OBSERVED 2026-09-10]

| 결함 | economy 측 요구 | 이행 위치 | 남는 것 |
|---|---|---|---|
| **C3-F27(c)** (S2, 스키마 충돌) | 계통별 표시 모델을 삭제된 저장 필드 `operationalCorrosion` 위에 세우지 말 것 | `currency-map.md` §4.1(재정의 + 산출식 `d_s`·재계산 시점·세이브 무영향) · §5 안내 · §6 표 R1 행 "저장 없음(파생)" · §6 개명 금지 목록 오기 정정 · `sink-source-ledger.md` §2 저장 축 표 · `reward-bands.md` §3 `corrosion.persisted:false`·`system_split` | **systems 측 요구는 이미 이행됨** — `save.md:94`가 필드 제거와 사유를, `:102`가 대체 규칙을 명시한다. economy가 저장 자리를 요구하지 않으므로 **두 레인 모델이 일치**한다 |
| **C3-F35** (S2, 필드명) | `currency-map.md` L101 "채택안 `plateOriginalWear`"를 L179(철회)와 일치시킬 것 (행 번호는 **개정 전** 기준 — 현재 위치는 §4.2·§6) | `currency-map.md` §4.2(채택안 = `readCounts`+`readBudget`, 근거는 `plate-readout.md` P-R2/P-R9/P-R10 인용) · 옵션표 B행 · 상한 3 행 · `reward-bands.md` `plate_original_state:` 블록 · `sink-source-ledger.md` §2·§6.3 | planner 2곳 교체(N-17) · systems 별칭 2줄(N-16). economy 문서 내부 모순은 **0** |

- **모델을 바꾸지 않았다**: 부식 상한 9·선택지 비용 7/8/12·안전 하한 공식·R2 상한 3·비차단 성질·G3 N/A 5종·밴드 값은 그대로다. 바뀐 것은 **어디에 저장되는가(→ 저장하지 않는다)** 와 **무엇이라 부르는가**뿐이다.
- **저장 필드 삭제가 불변식(CLAUDE.md §9)에 걸리지 않는 이유** [OBSERVED]: `save.md`가 "출시 0회·빌드 0회·임포터 0줄·표본 n=0 — 이 스키마를 읽은 세이브 파일이 세상에 존재하지 않는다"로 호환 위험을 없음으로 판정했다. economy는 이 판정을 **인용**할 뿐 스스로 만들지 않았다. 출시 후 같은 삭제는 마이그레이터 1개를 요구한다.
- 위 RFC는 **본 문서에 기록된 제안**이며 `decision-log.md`에는 아직 append되지 않았다(그 파일은 디렉터 소유). RFC-P3-008~015는 반대로 **디렉터가 이미 기록한 판정**이며 이 문서는 그것을 인용할 뿐이다.
- 실제 사람 플레이·지불 의사·성능 측정 **n=0**. 이 문서의 어떤 항목도 게임 게이트를 올리지 못한다.

### 4.1 이번 회차 재측정 명령 (재현 가능)

```
shasum -a 256 _workspace/current/planning/campaign.json
node _workspace/current/planning/validate-campaign.mjs
grep -cF "거래를 수락해도 같은 경로가 열린다" _workspace/current/planning/campaign.json                       # 0
grep -cF "거래를 수락해도 같은 경로가 열린다" _workspace/archive/20260909-preproduction-c3/planning/campaign.json  # 1
grep -cF "수락·거절 어느 쪽이든 경로는 열리며 청문 대조표의 각주 한 줄만 달라진다" _workspace/current/planning/campaign.json  # 1
ls _workspace/current/systems/ops _workspace/current/systems/data-schemas
grep -c "econ\." _workspace/current/systems/ops/telemetry-contract.md                                        # 0
grep -n "corrosionLimit\|corrosion:" _workspace/current/systems/prototype/model.mjs                          # :63-65, :79
grep -rn "장 경계" _workspace/current/economy/                                # 7행 — 전부 "폐기" 메타 서술, 규칙 주장 0행
```

**R4 회차 추가 명령 (C3-F27(c) · C3-F35 재측정, 2026-09-10)**

```
grep -n "operationalCorrosion" _workspace/current/systems/data-schemas/save.md      # :94 (제거 선언) 1행뿐 — 필드 정의 0행
sed -n '54p;55p;92p;102p' _workspace/current/systems/data-schemas/save.md           # readCounts / bypassUsed / 개명 금지 15종 / 대체 규칙
sed -n '39p' _workspace/current/systems/data-schemas/plates.md                      # readBudget int, tunable: economy, 기본 3
sed -n '55p;56p;57p' _workspace/current/systems/system-specs/plate-readout.md       # P-R2 / P-R9 / P-R10
sed -n '90p;91p;93p;94p' _workspace/current/systems/system-specs/corrosion-budget.md # C-R1 / C-R2(전역 9) / C-R4 / C-R9(무소모)
grep -rn "plateOriginalWear" _workspace/current/economy/                            # 지시어(채택안·필드 지정) 사용 0행. 남은 히트는 전부 "철회/폐기" 메타 서술이며,
                                                                                   #   총행 수는 기록이 늘수록 증가하므로 회귀 신호로 쓰지 않는다 (qa/c3-review.md §9.4 자기 계수 경고)
```

측정 결과: `campaign.json` sha256 `fdabf1d421932d679429870a06eeb31e9527562370c6a107aec77c2c884fb2b7`, 120479 B, 검증 44/44 PASS.
