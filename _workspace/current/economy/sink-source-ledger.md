---
updated: 2026-09-10
cycle: 20260909-preproduction-c3
status: current
supersedes: null
owner: game-economy-designer
---

# 소스·싱크 원장 · 고갈 무진행불가 보장

## 0. 지위

`economy/currency-map.md`의 자원 6종(R1~R6)에 대한 **흐름 원장**이다. 통화가 없으므로 이 원장의 목적은 인플레이션 관리가 아니라 **"자원 때문에 못 나아가는 경로가 0건임을 증명 가능한 형태로 고정하는 것"**이다. 실제 플레이 측정 n=0 [OBSERVED].

**정본 전제 (RFC-P3-009, 2026-09-10)**: 부식 예산 = 전역 상한 9 · `routing` 확정만 소모 · 누적 고갈 없음. **"장 경계 리셋"이라는 표현은 폐기됐고 이 문서에서 전부 제거했다** — 소모 사건이 캠페인 전체에 1회뿐이라 되돌릴 잔량이 없다.

**2026-09-10 재측정 영수증 [OBSERVED]** — 이 원장의 인용문·집계는 아래 명령으로 다시 확인했다.

| 명령 | 결과 |
|---|---|
| `shasum -a 256 _workspace/current/planning/campaign.json` | `fdabf1d421932d679429870a06eeb31e9527562370c6a107aec77c2c884fb2b7` (120479 B) |
| `node _workspace/current/planning/validate-campaign.mjs` | 44/44 PASS · 비트 33 · `routing` 3비트 · `corrosion` 3비트 |
| `grep -cF "<인용문>" planning/campaign.json` | §3.2·§3.3의 모든 [OBSERVED] 문구 live 1건 확인 (§3.3 주 참조) |
| `ls _workspace/current/systems/ops _workspace/current/systems/data-schemas` | `telemetry-contract.md` 1건 · 스키마 6건 **존재** (§6) |

**개정 (R4, 2026-09-10 · 같은 사이클 제자리 갱신 = RFC-Q2, `supersedes: null` 유지) [OBSERVED]**: §2에 **저장 축 동기화 표**를 신설했다 — R1 부식은 `save.md:94`의 `operationalCorrosion` 제거로 **저장 없음(파생)**, R2 원본 상태의 정본 필드명은 `readCounts`+`readBudget`(C3-F27(c)·C3-F35). §6.3의 요청 키 `econ.corrosion.spent_at_commit`은 "소모"를 전제한 오칭이라 **`econ.corrosion.config_cost_at_commit`으로 교체**했다(미등재 키이므로 계약 버전 영향 0). 흐름 수치·소모 지점·보장 규칙은 바뀌지 않았고 실측은 여전히 n=0이다.

## 1. 표준 원장 규약의 대체

| 표준 규약 | 판정 | 대체 지표 |
|---|---|---|
| `source_per_day` / `sink_per_day` | **N/A** — 일 단위 재생 자원 없음 | `source_per_chapter` / `sink_per_chapter` (§3) |
| `ratio_band [0.9, 1.1]` | **N/A** — 균형 잡을 순환이 없음 | `blocked_by_resource == 0` (§4 INV-P0) |
| 인플레이션 월간 상한 | **N/A** — 통화 없음 | 상한 유한 + 단조 통제(등급 M/F/P) |
| "새 소스에는 같은 사이클 내 싱크" | **조건부 적용** | 소모성 C 등급에만 적용. 현재 C 등급은 R1 1종이고, 소스(캠페인 시작 시 상한 9 부여 · 막다른 구성에 대한 무료 우회관)와 싱크(`routing` 확정 1회)가 짝을 이룬다. **재부여·리셋 소스는 없다** |

## 2. 자원별 소스·싱크

| 자원 | 소스 (증가) | 싱크 (감소) | 순환 여부 | 캠페인 총 소모 |
|---|---|---|---|---|
| R1 부식 예산 | 캠페인 시작 시 전역 상한 9 부여(재부여 없음) · 막다른 구성마다 무료 우회관 1회 | `routing` 확정 1회 | **순환 없음 — 1회성 차감** | **7 또는 8, 1회** |
| R2 원본 상태 | 없음 | 원본 직접 파괴적 절차 확정 | 단조 감소, 비차단 | 최대 2회 (`c1-b2`, `c4-b2`) |
| R3 청문 신뢰도 | 확정 카드 | 없음 | 파생·비저장 | — |
| R4 증인 상태 | 대화 분기 확정 | 없음 | 이산 플래그 | — |
| R5 열람 권한 | 확정·대화 | **없음(회수 금지)** | 단조 증가, 유한 집합 | — |
| R6 D-21 | 미채택 | 미채택 | — | — |

**저장 축 동기화 (C3-F27(c) · C3-F35, 2026-09-10 재측정) [OBSERVED]** — 이 원장의 자원 중 저장 필드에 걸리는 것과 걸리지 않는 것을 분리해 적는다.

| 자원 | 저장 필드 | 근거 |
|---|---|---|
| R1 부식 예산 | **저장 없음(파생)** | `systems/data-schemas/save.md:94`가 `operationalCorrosion`을 **제거**했다(RFC-P3-009: 소모가 없으니 누계도 없다). 상태가 필요하면 `save.md:57` `committedRouting`에서 재계산(`save.md:102`). 계통별 분해도 파생 표시값이다(`currency-map.md` §4.1) |
| R1 우회관 이력 | `bypassUsed` string[] (`save.md:55`) | 소모가 아니라 **사용 이력**이므로 유지(`save.md:103`) |
| R2 원본 상태 | `readCounts` map<recordId,int> (`save.md:54`) + 상한 `readBudget`=3 (`plates.md:39`, `tunable: economy`) | 철회된 이름 `plateOriginalWear`는 쓰지 않는다(C3-F35) |
| R3 청문 신뢰도 | **저장 없음(파생)** | 재확정 시 재계산(§2) |
| R4 증인 상태 · R5 열람 권한 | v2 요청(`witnessState`·`accessGranted`) | `currency-map.md` §6 |

## 3. 장별 흐름 [TARGET] (실측 아님 · campaign.json 비트 근거)

### 3.1 R1 부식 예산

상한 9는 **캠페인 전체에 하나**다. 아래 "잔여 상한" 열은 장마다 다시 채워지는 값이 아니라 **그 시점까지의 누적 소모를 뺀 값**이다.

| 장 | 진입 시 잔여 | 소모 지점 | 소모량 | 종료 잔여 | 근거 |
|---|---:|---|---:|---:|---|
| T0 | 9 | 없음 | 0 | 9 | `routing` 미사용 [OBSERVED] |
| C1 | 9 | 없음 | 0 | 9 | 〃 |
| C2 | 9 | 없음 (`corrosion`은 시험 도구) | 0 | 9 | `c2-b2`,`c2-b3` 판독/시험 [OBSERVED] |
| C3 | 9 | 없음 | 0 | 9 | 〃 |
| C4 | 9 | 없음 (`c4-b2`는 그늘 역산 판독) | 0 | 9 | 〃 |
| **C5** | 9 | `c5-b2` 가상 운전 **0** / `c5-b4` 확정 | **7 또는 8** | **2 또는 1** | `c5-b2` "가상 운전은 실제 계통을 바꾸지 않는다" [OBSERVED, live 1건] |
| C6 | **2 또는 1** | 없음 | 0 | 2 또는 1 | 〃 |
| C7 | **2 또는 1** | `c7-b2` 시연 — **무소모로 확정 요청** | 0 [INFERENCE] | 2 또는 1 | 열린 질문 Q2 (currency-map §8) |
| E0 | **2 또는 1** | 없음 | 0 | 2 또는 1 | 〃 |

- **캠페인 전체 소모 합 = 7 또는 8, 발생 1회.** 이 자원은 "경제"가 아니라 **단 한 번의 선택 제약**이다.
- **위 표의 "잔여/소모" 어휘는 미해소 문언 차이다 [OBSERVED, R4 2026-09-10]**: systems `corrosion-budget.md` L94 C-R9는 "부식은 소모되지 않는다 — 확정도 잔량을 깎지 않는다"고 적고, 이 표는 잔량이 줄어드는 것처럼 적는다. **판정 결과는 두 읽기에서 동일**하다(확정 게이트는 `configCost ≤ 9` 비교 1줄, C5 이후 `routing` 확정 비트 0개 → 막히는 확정 0건). 그래서 **이번 개정에서 수치를 바꾸지 않았다.** 문언 통일은 `negotiation-record.md` N-18 / RFC-E8(디렉터 판정 대상)이며, C-R9 쪽으로 정리되면 "진입 시 잔여/종료 잔여" 두 열은 "그 시점 구성안 비용" 한 열로 축약된다.
- C6 이후 잔여가 9로 돌아가지 않는 것이 이 표의 유일한 실질 변경이다(RFC-P3-009로 "장 경계 리셋"이 폐기됐다). 소모 지점이 C5 하나뿐이므로 **잔여가 줄어든 상태로 남아도 이후 어떤 확정도 막지 않는다** — `routing` 확정 비트가 그 뒤에 없다(`c7-b2`는 시연, Q2).
- 여유 = 9 − 8 = 1 (최악의 경우). 두 보호 선택이 모두 열린다 → 법4 제로섬이 예산이 아니라 **가치 판단**으로만 갈린다.
- `dock-express`(12)는 항상 초과다(12 > 9). 이것은 **한도를 가르치기 위한 오답 선택지**이며, "부두 진영이 불가능하다"는 오해를 만들지 않도록 `dock`(8, 가능)과 나란히 보이고 초과 사유를 문장으로 표시해야 한다 → 연출/시스템 요청 (§5 RFC-E2).

### 3.2 R5 열람 권한 — 개방 원장 (단조, 회수 0)

| 대상 | 개방 비트 | 근거 문구 [OBSERVED] | 회수 |
|---|---|---|---|
| `hub` | 초기 부여 | T0 zone | 없음 |
| 도구 `circuit` | `t0-b2` | 벽 지도 음영 3구획 | 없음 |
| 도구 `reader` | `t0-b3` | 첫 판독·자동 사본 | 없음 |
| `gate` 제3수문 | `c1-b1` | "제3수문 구역이 열리고" | 없음 |
| 도구 `seal` | `c1-b3` | 필적 대조 확정 | 없음 |
| 도구 `corrosion` | `c1-b4` | "부식 시험대 사용 허가가 열린다" | 없음 |
| `pump` 지하수로 | `c2-b1` | "지하수로 구역이 열리고" | 없음 |
| `dock` 부두 | `c2-b4` | "조건부로 부두사무소 대장 열람을 주선" | 없음 |
| 도구 `alignment` | `c3-b2` | 공통 시간축 생성 | 없음 |
| `lowland` 저지대 | `c3-b4` | "저지대 주민회 접촉 경로가 열린다" | 없음 |
| 양수장 판독 목록 확장 | `c4-b4` | "양수장 판독 목록이 확장되고" | 없음 |
| 도구 `routing` | `c5-b2` | 두 경로 가상 완주 | 없음 |
| 제출 초안 3종 | `c6-b4` | "세 관점의 제출 초안이 열리고" | 없음 |

구역 5종·도구 6종이 **C5 이전에 전부 개방**되고 이후 축소되지 않는다. 05:00 계통 이관은 엔딩 사건이며 권한 만료 UI를 만들지 않는다.

### 3.3 R4 증인 상태 — 확보 원장

| 인물 | 접촉 비트 | 확보 실패 시 열리는 대체 경로 [OBSERVED] |
|---|---|---|
| 문재화 | `c1-b1`, `c1-b3`, `c2-b4` | `c2-b4`에서 조건부 대장 열람 주선 |
| 표성찬 | `c3-b4` | "수락·거절 어느 쪽이든 경로는 열리며 청문 대조표의 각주 한 줄만 달라진다" — 저지대 주민회 경로는 선택과 무관하게 개방 |
| 오은정 | `c5-b3` | 소각 확정과 동시에 증거가 복구된다 |
| 한도연 | `c4-b4` | 진술은 "단서"로만 분류돼 확정 칸에 들어가지 않는다 = 없어도 확정 가능 |

**증인 0인으로도 33비트 전부 통과 가능해야 한다**(INV-W0). 현재 비트 텍스트상 모든 인물 경로가 "실패해도 대체가 열린다"로 기술돼 있어 조건은 충족되는 것으로 읽힌다 [INFERENCE] — 코드 불변식으로는 아직 강제되지 않는다.

> **C3-F16 수정 (2026-09-10)** — 이전 판은 `c3-b4`를 "거래를 수락해도 같은 경로가 열린다"로 인용했다. 재측정 결과 그 문장은 **live에 0건, 아카이브 c3에만 1건**이다: `grep -cF "거래를 수락해도 같은 경로가 열린다" _workspace/current/planning/campaign.json` → `0`, 같은 명령을 `_workspace/archive/20260909-preproduction-c3/planning/campaign.json`에 → `1`. live 원문은 `c3-b4.consequence`의 **"수락·거절 어느 쪽이든 경로는 열리며 청문 대조표의 각주 한 줄만 달라진다"**(`grep -cF` → `1`)이며 위 표와 `reward-bands.md` §1 채널 D 예시를 그 문구로 교체했다. **의미는 동일**(선택이 경로 접근을 바꾸지 않는다)하므로 §4 INV-W0 판정과 보상 채널 분류에는 영향이 없다.

## 4. "고갈로 진행 불가 0건" 보장 규칙

| ID | 규칙 | 현재 상태 | 강제 수단 |
|---|---|---|---|
| **INV-P0** | 어떤 자원의 **잔량**도 필수 확정의 활성 조건에 들어가지 않는다. 유일한 예외는 R1→`routing` 확정이며 안전 하한(§4.1)으로 상쇄한다 | 충족 [INFERENCE] | **미강제** → RFC-E4 (신규 INV10) |
| **INV-P1** | 연습·프리뷰·가상 시험·되돌림은 어떤 자원도 소모하지 않는다 | 충족 (`c5-b2` 문구) | **미강제** → RFC-E4 (신규 INV11) |
| **INV-P2** | 확정 직전 체크포인트로 잔량이 확정 이전 값으로 복원된다 | 충족 (`interaction-rules` §5.2) | INV4(프리뷰 선행)로 간접 강제 |
| **INV-P3** | 모든 필수 확정에 **`sourceType` 상이 AND 루트 `originId` 상이**를 만족하는 자료쌍이 1쌍 이상 있고, 그 쌍의 한쪽 경로는 어떤 플레이어 행동으로도 파괴 불가 | 충족 | `unity-implementation` §5 임포트 검증 (fail-closed) [OBSERVED] |
| **INV-P4** | 막다른 구성은 **무료 우회관으로 항상 복구된다 — 구성마다 1회**(캠페인 1회가 아니다) | 충족 (법5) | **미강제** |
| **INV-P5** | 열람 권한은 회수되지 않는다 (`accessGranted` 제거 연산 부재) | 충족 (§3.2) | **미강제** → 제거 API를 만들지 않는 것으로 |
| **INV-W0** | 증인 0인 확보로 캠페인 완주 가능 | 충족 [INFERENCE] | **미강제** |
| **INV-H0** | 힌트 3단계는 무료·무제한이며 어떤 자원도 소모하지 않고 결말·평가에 영향 0 | 충족 (`interaction-rules` §4) | 세이브 `hintLevelUsed`는 기록만 [OBSERVED] |
| **INV-S0** | 세이브 로드 시 자원 잔량은 해당 체크포인트 값으로 복원되며, 세이브를 거듭할수록 줄어드는 전역 자원은 없다 | 충족 (단조 자원 없음, R2는 판당 상한) | 저장 스키마 v2 설계 시 |

**우회관 단위 정정 [OBSERVED, 2026-09-10 재측정]**: 이전 판은 우회관을 "캠페인 1회"로 읽히게 적었다. 정본 두 곳은 **구성 단위**를 말한다 — `systems/system-specs/corrosion-budget.md` C-F2 "`UseBypass` 제안(무료, … **구성마다 1회**)", `systems/data-schemas/save.md:56` `bypassUsed: string[]` "무료 우회관 사용 **구성 id**". `systems/interaction-rules.md` §2.5의 "무료 우회관 1회로 언제나 복구"는 요약 문구이며 단위를 말하지 않는다 [OBSERVED]. 셋을 한 문장으로 통일해 달라는 요청은 §5 RFC-E8. 복구 보장 자체는 어느 해석에서도 성립한다.

프로토타입에 이미 존재하는 불변식은 INV1~INV9 [OBSERVED] `systems/prototype/model.mjs:670-746`이며, 그중 자원 관련은 **INV3(부식 초과 구성은 확정되지 않는다, `model.mjs:687`)** 하나뿐이다. INV3는 "초과를 막는" 방향만 검사하고 **"부족해서 못 나아가는"** 방향을 검사하지 않는다 → 아래 두 불변식 추가를 요청한다.

```
INV10  모든 필수 확정에 대해, 도달 가능한 임의 상태에서 자원 잔량만을 이유로
       확정이 영구히 불가능해지는 경로가 존재하지 않는다.  (탐색: 상태 그래프 전수)
INV11  preview / practice / undo 경로는 자원 벡터를 변경하지 않는다.
       (같은 입력 2회 호출 시 자원 해시 불변 — 기존 Preview 부작용 테스트와 동일 형식)
```

### 4.1 안전 하한 공식

```
최소 조건 :  corrosionLimit  >=  max(정상 선택지 비용)          = 8   ->  9 >= 8  충족(여유 1)
권장 조건 :  corrosionLimit  >=  max(정상 선택지 비용) + 1      = 9   ->  9 >= 9  경계 충족(여유 0)
붕괴 조건 :  어떤 보호 선택지의 비용 > corrosionLimit           ->  법4 제로섬이 강제 선택으로 변질
```

`dock` 비용을 9로 올리면 권장 조건이, 10으로 올리면 최소 조건이 깨진다. **balance/systems는 이 세 값(7·8·9) 중 하나라도 바꿀 때 RFC를 연다.**

### 4.2 대체 경로 보장 (요약)

| 막힐 수 있는 지점 | 1차 경로 | 대체 경로 | 근거 |
|---|---|---|---|
| `routing` 확정 초과 | 한도 내 재구성 | 무료 우회관(구성마다 1회) | 법5 · `corrosion-budget.md` C-F2 |
| 원본 훼손 우려 | 원본 절차 | 자동 보존 사본(불파괴) | 법2 / INV1 |
| 증인 거절 | 증언 확보 | 매체 2종 대조 | 법6 / INV-P3 |
| 시간 순서 판정 불가 | 정합 후 확정 | `unknown` 상태로 진행 지속 | 법3 / INV7 |
| 배선 밖 근거 | 다른 매체 수집 | 판정 보류(진행은 지속) | 법1 |

**어느 행에도 "재화를 더 모아야 한다"가 없다.** 이 표에 그런 행이 생기는 순간 G3 대체 검사 ①이 깨진다.

## 5. RFC (열림)

| ID | 대상 레인 | 질문 | 제안 |
|---|---|---|---|
| **RFC-E1** | systems | `LIMITS.maxUndo: 32` [OBSERVED]와 `interaction-rules` §0.2 "되돌림 무제한"이 불일치한다 | 문서를 코드에 맞춰 **"되돌림 이력 32단"**으로 정정하거나, 이력 상한을 자원처럼 읽히지 않게 UI에서 감춘다. 32단을 넘겨 되돌릴 수 없어 진행이 막히는 경로가 있는지 INV10 탐색으로 확인 |
| **RFC-E2** | systems, presentation | `dock-express`(12)가 항상 초과라 "부두 선택 불가"로 오해될 수 있다 | `dock`(8, 가능)과 항상 나란히 표시하고 초과 사유를 문장으로 병기 |
| **RFC-E3** | worldview, systems | "재생 예산"이라는 명칭이 법2(사본 무제한)와 충돌해 읽힌다 | UI 표기를 **"원본 상태"**로, "예산"은 부식에만 사용. 용어집 후보표에 반영 요청 |
| **RFC-E4** | systems | 자원 고갈 방향 불변식이 없다 | INV10·INV11 추가, 임포트 검증기와 배치 테스트 양쪽에 연결 |
| **RFC-E5**(개정) | systems | ~~텔레메트리 계약 파일이 없다~~ → **계약은 존재한다**(`systems/ops/telemetry-contract.md`, current, 7.6K). 그러나 §1~§8 어디에도 **자원·부식·권한·증인 키가 0건**이다: `grep -c "econ\." telemetry-contract.md` → **0**, `grep -n "부식\|corrosion\|자원\|권한\|증인"` → **0행** [OBSERVED 2026-09-10] | 계약 §2·§3 사이에 "자원 키" 절을 신설하고 §6 표의 8키를 등재한다. `system-specs/corrosion-budget.md` §6이 이미 정의한 `corrosion_trial_count` 등 5키도 계약에 올라와 있지 않다 — **스펙에만 있고 계약에 없는 키는 수집되지 않는다** |
| **RFC-E8** | systems | 무료 우회관의 단위가 문서마다 다르게 읽힌다(`interaction-rules` §2.5 "1회" vs `corrosion-budget` C-F2 "구성마다 1회" vs `save.md` `bypassUsed: string[]`) | `corrosion-budget` C-F2 + `save.md` 배열형이 정본이므로 `interaction-rules` §2.5를 "막다른 구성마다 무료 우회관 1회"로 정정 |

## 6. 텔레메트리 — 계약은 존재하나 자원 키가 0건 (C3-F15 재작성)

### 6.1 이전 판의 무엇이 틀렸나 [OBSERVED]

이전 판은 "`_workspace/current/systems/ops/`는 **빈 디렉터리**이며 `telemetry-contract.md`가 없다"고 적고, 그 부재 위에 "**G3는 PASS할 수 없다**"를 세웠다. **부재 주장이 사실이 아니다.**

| 명령 (2026-09-10 실행) | 결과 |
|---|---|
| `ls _workspace/current/systems/ops/` | `telemetry-contract.md` (7.6K, cycle c3, status current, owner game-systems-designer) — **존재** |
| `ls _workspace/current/systems/data-schemas/` | `beats.md hints.md plates.md save.md tools.md zones.md` — **6종 존재** |
| `grep -c "econ\." systems/ops/telemetry-contract.md` | **0** |
| `grep -n "부식\|corrosion\|자원\|권한\|증인" systems/ops/telemetry-contract.md` | **0행** |

### 6.2 다시 세운 근거 — 결론은 유지, 이유가 바뀐다

계약이 없어서가 아니라 **계약에 이 레인의 키가 하나도 없고, 있어도 값이 없기 때문에** G3는 아직 PASS할 수 없다. 세 이유는 독립이며 하나만 해소돼도 통과하지 않는다.

| # | 차단 이유 | 증거 | 해소 주체 |
|---|---|---|---|
| B1 | **자원 키 미등재.** 계약은 시간·진행·힌트·연습/되돌림·AFK·세이브·성능 7군을 정의하지만 자원군이 없다. 빌드가 생겨도 §6.3의 값은 수집되지 않는다 | `grep -c "econ\."` → 0 [OBSERVED] | systems (RFC-E5 개정) |
| B2 | **표본 n=0.** 계약 §1이 스스로 "현재 수집된 표본은 n = 0", "`observed_*` 키는 전부 값이 비어 있다"고 적는다. 대체 검사 5종 중 측정으로 답할 수 있는 항목이 0개다 | `telemetry-contract.md` §0·§1·§9 [OBSERVED] | 플레이테스트(계약 `## Time acceptance` 표본 12명/5유형) |
| B3 | **강제 수단 부재.** `reachability.blocked_by_resource_max: 0`은 INV10·INV11이 구현되기 전까지 설계 주장이다. 프로토타입 불변식은 INV1~INV9뿐이고 자원 관련은 INV3(초과 차단) 하나 | `model.mjs:670-746`, `:687` [OBSERVED] | systems (RFC-E4) |

**따라서 G3 판정 = `DESIGN_ONLY_NOT_MEASURED`**이며 PASS가 아니다. CLAUDE.md §6("숫자 자리의 `[TARGET]`/`[INFERENCE]`는 PASS를 막는다")에 따라, 위 세 항목이 모두 닫히기 전에는 `qa/gate-measurements.md#g3`의 `substitute_checks`가 `measured: null`을 벗어날 수 없다.

**결론이 바뀌지 않는 것**과 **근거가 바뀐 것**을 구분해 적는다: 이전 판의 결론(PASS 불가)은 유지되지만, 이전 판이 든 이유(파일 부재)는 **거짓이었고 삭제**했다. 부재를 근거로 삼는 문장은 이 레인에서 다시 쓰지 않는다 — 존재 여부는 `ls`로 확인한다.

### 6.3 요청 필드 (systems 계약에 등재 요청)

계약 §8이 "키 추가는 자유, 개명·의미 변경은 버전 증가 필요"라 했으므로 **추가만으로 가능**하다 [OBSERVED].

| 요청 키 | 형 | 무엇을 검사하는가 | 계약 등재 | 값 |
|---|---|---|---|---|
| `econ.corrosion.limit` | int | 한도가 데이터에서 왔는가 (기대 9) | **미등재** | 없음 (n=0) |
| `econ.corrosion.config_cost_at_commit` | int | 확정 시점 구성안 비용 (기대 7\|8). **잔량 차감이 아니다** — `corrosion-budget.md` C-R9 "부식은 소모되지 않는다" · `save.md:94` 저장 없음. 이전 판의 키 이름 `…spent_at_commit`은 폐기된 소모 모델을 되살리는 오칭이라 이번 개정에서 교체했다(미등재 키이므로 계약 버전 영향 0) | 미등재 | 없음 |
| `econ.corrosion.blocked_attempts` | int | 초과 구성 시도 = 학습 신호 | 미등재 | 없음 |
| `econ.corrosion.bypass_used_count` | int | 우회관 사용 (구성 단위, §4 정정) | 미등재 | 없음 |
| `econ.blocked_progress_events` | int | **0이어야 한다** — 자원으로 막힌 사건 | 미등재 | 없음 |
| `econ.plate.destructive_ops` | map | 판당 파괴적 절차 (기대 ≤2) = 세이브 `readCounts`(`save.md:54`)의 판별 값, 상한은 `readBudget`(`plates.md:39`) | 미등재 | 없음 |
| `econ.access.granted_at` | map | 권한 개방 시각·비트 | 미등재 | 없음 |
| `econ.access.revoked_events` | int | **0이어야 한다** | 미등재 | 없음 |
| `econ.witness.state_at_c7` | map | 증인 0인 완주 표본 확보 | 미등재 | 없음 |
| `econ.hint.level_used` | map | 세이브 `hintLevelUsed`(`save.md:64`)는 **존재**하나 텔레메트리 키로는 미정의 | 미등재 | 없음 |
| `econ.undo.depth_max` | int | `maxUndo: 32`(`model.mjs:82`) 상한 접촉 여부 (RFC-E1) | 미등재 | 없음 |

`system-specs/corrosion-budget.md` §6의 `corrosion_trial_count` · `corrosion_over_budget_count` · `corrosion_over_by_max` · `corrosion_bypass_used` · `corrosion_final_ratio` 5키도 **스펙에만 있고 계약에 없다** [OBSERVED]. 이름 중복을 피하려면 계약 등재 시 두 목록을 systems가 한 번에 정리해야 한다 → RFC-E5.

**열린 작업**: 계약에 자원군이 등재되기 전까지 G3 자기점검은 "설계 충족 / 측정 없음"으로만 보고한다. 이 문장은 이전 판과 같지만, 이제 근거가 실재하는 문서를 가리킨다.

## 7. 미측정 [OBSERVED]

- 부식 소모 실측 0건, 초과 시도 횟수 0건, 자원으로 막힌 사건 0건 — **모두 "측정하지 않았다"이지 "0이었다"가 아니다.**
- 증인 0인 완주를 시도한 사람 0명. INV-W0은 텍스트 근거의 [INFERENCE]이며 코드로 강제되지 않았다.
- 무료 우회관이 실제로 모든 막다른 구성을 푸는지 전수 탐색한 적 없다(단위 정정과 무관하게 미측정).
- **텔레메트리 계약이 존재한다는 사실은 아무것도 측정하지 않았다는 뜻과 양립한다.** 계약 §9가 스스로 "이 계약이 존재한다는 사실은 G6를 PASS시키지 않는다"고 적는다 [OBSERVED] — 같은 규율이 G3에도 적용된다.
