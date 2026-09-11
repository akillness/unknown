---
updated: 2026-09-11
cycle: 20260909-preproduction-c3
status: current
supersedes: _workspace/archive/20260909-preproduction-c2/worldview/glossary.md
owner: game-worldview-architect
---

# 용어집 (정본)

**규칙**: 여기에 없는 고유명사는 시놉시스·대사·UI 문자열·에셋 파일명에 쓸 수 없다. 명사는 여기서 태어나고, 다른 레인은 인용만 한다. 추가·수정은 worldview 레인만 한다.
**C3 종료 수정 루프(2026-09-10, R4 이전 회차 기록 [CARRIED])**: `qa/c3-review.md` C3-F13이 지목한 미수록 명사 14건과, 그 회차의 live `planning/campaign.json`(**당시 입력** sha `fdabf1d4…` · 120479 B — 현행 입력은 위 R4 고지와 §7 머리글 참조) · `systems/game-ui-contract.json` · `systems/interaction-rules.md` 를 grep으로 재검색해 찾은 나머지를 승격했다(§2·§3·§4·§7). "부식예산" 정의는 RFC-P3-009로 교체했고, 세계관 본문 재기반(RFC-P3-010)에 따라 판 #0·결손 4시간·서린 항을 정정했다. 폐기된 명사는 삭제하지 않고 §8로 옮겼다.
**입력 판 정정 (C3-F28 · 2026-09-10 2차 수정 루프)**: 이 문서의 앞선 판은 planner 수리 **전** 파일(sha `775a984c…` · 120087 B)을 입력으로 적었다. 그 판은 디스크에 없어 재검증할 수 없으므로 `775a984c…`는 [CARRIED · `planning/campaign.meta.md` §11 브로드캐스트]로만 인용한다. 두 판의 차분은 단서 `c1-b4-c3`(`sourceType: plate` · `originId: brine-log-gate3`) **1건**이고 그 `originId`는 이미 §7 카탈로그에 있다 [OBSERVED] → **수록 명사 판정(C3-F13)과 §7 카탈로그 본문·행 수는 바뀌지 않는다.** 바뀐 것은 §7 머리글의 집계 영수증 숫자와 입력 해시뿐이며 아래 §7에서 재측정값으로 교체했다.
**R4 개정 (2026-09-10, 같은 사이클 제자리 갱신 · RFC-Q2)** [OBSERVED]: 대체가 아니라 개정이므로 `cycle`·`supersedes:`는 그대로다. 바뀐 곳 — **§6-1 신설**(비트 `zoneId` 5종의 KO/EN 대응과 에셋 토큰 파생 규칙, C3-F22 · 모델러 OPEN-M1 판정) · §7 머리글의 입력 해시·집계 영수증을 R4 재측정값으로 교체(RFC-Q1). 기존 명사 행은 추가만 있고 삭제·개명은 없다.
**R7 개정 (2026-09-10, 같은 사이클 제자리 갱신 · RFC-Q2)** [OBSERVED]: **§3 에 도구 표시명 4건 등재**(배선 추적·판독·배수 편성·부식 시험) + **§3-1 신설**(도구 id 6종 ↔ 표시명 대응, RFC-S6 판정 반영). 기존 행은 추가만 있고 삭제·개명·정의 변경은 없다. `cycle`·`supersedes:`·`status:` 그대로.
**R8 개정 (2026-09-11, 같은 사이클 제자리 갱신 · RFC-Q2 · RFC-CX-012 ACK-b)** [OBSERVED]: `production/term-decision-aside-20260911.md` §4 등재 대기 목록 적용 — **§2 신설 5행**(판독대·조습기·구역 지도·서명 요건표·1호기) · **§2 「회로 지도」 정의 확장**("기록 밖"·"기록 억제" 2상태 명시, A-07 — 이 1행만 정의문 확장이며 개명 아님) · **§3 신설 4행**(확정 전 보존·복귀 지점·연습 압착·연습 서식) · **§7 머리글 파생 규칙 1줄**(등재명사+「철」 허용, 낱장 수량 「N점」, A-15). 신설 행의 first cycle `c7` = 이번 사이클 판정(RFC-CX-012)으로 태어난 명사. 기존 행은 삭제·개명 없음. campaign·UI 계약 문자열 교체는 planner(ACK-a 선결)·systems 레인 소관이라 이 개정에 포함하지 않는다. `cycle`·`supersedes:`·`status:` 그대로.
**R9 개정 (2026-09-11, 같은 사이클 제자리 갱신 · RFC-Q2 · QA `qa/rfc-cx-012-review.md` D-CX012-03/06 디렉터 판정 반영)** [OBSERVED]: RFC-CX-012 교체로 표시 문자열에 들어간 신규 어휘 5종의 등재 완전성 결함(D-CX012-03) 해소 — **§3 신설 3행**(보호 지정·공통 종결부·종결부 후일담, term-decision §2 B-05·B-06·B-07) · **§7 머리글 어간 파생 면제 규칙 1줄**(「역대조」=잠금 용어 「대조표」의 어간 파생 · 「확대 판독」=「판독」의 어간 파생, B-08·B-09 — 정본 행이 있는 어간을 따르므로 별도 등재하지 않음) · **§4 「공통 조위 피크」 정의문에 축약 허용 1절**('공통'이 문맥에서 확정된 뒤 「조위 피크」 축약 가능, D-CX012-06 — 정의·명칭 변경 아님). 신설 행의 first cycle `c7`. 기존 행은 삭제·개명 없음. `cycle`·`supersedes:`·`status:` 그대로.
**영어명 주의** [OBSERVED]: 아래 EN 표기는 내부 설계 라벨이며 상표·동명 조사를 하지 않았다. 상점명·번들명·공개물에 그대로 쓰지 않는다.
first cycle 표기: `c1`~`c3` = 그 사이클에서 태어남.

## 1. 기관 · 장소

| term (KO) | term (EN) | definition | first cycle | owner-lane |
|---|---|---|---|---|
| 조수기록국 | Tide Records Bureau | 방어계통 상태를 기록·보존하던 기관. 3주 전 폐국을 고지했고 게임은 이관 전 마지막 밤을 다룬다 | c2 | worldview |
| 은포항 | Eunpo Port | 가상의 대조차 항구 도시, 대조차 9.2 m | c2 | worldview |
| 당직실 | Watch Room | 본관 허브. 작업대·판독기·서명대가 있는 방 | c3 | worldview |
| 제3수문 | Gate Three | 외항 구역 | c2 | worldview |
| 구염전 저지대 | Old Saltern Lowland | 염전 폐업 후 침하한 주거 구역 | c3 | worldview |
| 냉동창고 부두 | Cold Quay | 어시장·냉동물류 구역 | c3 | worldview |
| 제1양수장 | Pump House One | 폐쇄된 지하수로 구역, 1호기가 있던 곳 | c2 | worldview |
| 부두사무소 | Quay Office | 회선이 깔린 3개소 중 하나 | c3 | worldview |
| 회선 3개소 | Three Wired Stations | 통화 개시 시각·계통이 남는 유일한 지점(당직실·제3수문·부두사무소) | c3 | worldview |
| 구 서고 | Old Stack Room | 폐국 정리 중인 기록국 서고. 서린이 판 #0을 빼낸 곳 | c3 | worldview |

## 2. 매체 · 장치

| term (KO) | term (EN) | definition | first cycle | owner-lane |
|---|---|---|---|---|
| 염판 | Salt Plate | 12시간 신호를 소금 결정에 각인한 기록 매체 | c2 | worldview |
| 각인층 | Etch Layer | 염판에서 실제 신호가 새겨진 층 | c3 | worldview |
| 염선 | Brine Line | 압력·염도로 신호를 전달하는 도관망. 배관 자체가 회로다 | c2 | worldview |
| 당직일지 | Watch Log | 손글씨 기록, 누락·과장·거짓 가능 | c2 | worldview |
| 조위대장 | Tide Ledger | 수치만 기록된 대장 | c2 | worldview |
| 판독기 | Plate Reader | 염판을 읽는 장치. 첫 판독에서 검증 사본을 자동 보존한다 | c3 | worldview |
| 서명대 | Seal Desk | 이중서명 봉인을 받는 자리. 확정 게이트가 여기서 판정된다 | c3 | worldview |
| 봉인대 | Seal Press | 서명대의 압착 기구. 압착이 끝나면 봉인 완료 접점이 닫힌다 | c3 | worldview |
| 작업대 | Workbench | 당직실 허브의 조사·대조 면. 상시 슬롯과 자료 탭이 붙는다 | c3 | worldview |
| 부식 시험대 | Corrosion Test Bench | 폐관 한 토막·기준 시편으로 부식률만 계산하는 대. 본관에 소금이 닿지 않는다 | c3 | worldview |
| 정합기 | Aligner | 두 자료의 조위 기준선을 공통 피크로 맞추는 장치 | c3 | worldview |
| 편성기 | Router Bench | 배수 경로 편성안을 짜고 가상 완주시키는 장치 | c3 | worldview |
| 계통판 | Line Board | 구역마다 붙은 염선 계통의 압력·염도 표시판. 현장 판독 지점 | c3 | worldview |
| 표준판 | Standard Plate | 기록국 표준 규격의 염판. 대조의 밤 표준판이 첫 판독 대상이다 | c3 | worldview |
| 시편 | Coupon | 부식 시험대에서 조건별 무늬를 만들어 비교하는 시험 조각 | c3 | worldview |
| 상시 슬롯 | Standing Slot | 작업대에 자료 한 점을 밤 내내 고정해 두는 자리. 판 #0이 여기 놓인다 | c3 | worldview |
| 가설판 | Hypothesis Board | 근거 슬롯·검증·서명 순서로 결론을 세우는 UI 패널 | c3 | worldview |
| 증거함 | Evidence Box | 확보한 자료가 자동으로 들어가는 보관·열람 UI 패널 | c3 | worldview |
| 사건판 | Case Board | 확정된 사실과 미해결 영역이 고정되는 사건 전개 판 | c3 | worldview |
| 결론 카드 | Conclusion Card | 가설판에서 확정 직전에 여는 결론 요약 카드 | c3 | worldview |
| 회로 지도 | Circuit Map | 판독 가능 범위와 "기록 밖"·"기록 억제" 구획을 구분해 접어 표시하는 지도(법1의 도구). 음영 상태 어휘는 "기록 밖"(판독 범위 밖)과 "기록 억제"(범위 안이나 기록이 억제된 구획) 2종이다 | c3 | worldview |
| 판 #0 | Plate Zero | **번호만 적힌 미봉인 염판** 1매. 서린이 구 서고에서 빼내 서랍에 숨겼다. 이름은 각인되지 않으며 이 판이 담은 것은 명령이 대기한 시간뿐이다 | c2 | worldview |
| 자동 사본 | Auto Copy | 원본을 처음 판독할 때 계통이 자동으로 남기는 검증 사본(법2). 원본의 루트 출처를 물려받는다 | c3 | worldview |
| 사본 | Certified Copy | 확정된 사실이 청문 접수부에 등재된 복본. 원본 소실 후에도 남는다 | c3 | worldview |
| 청문 접수부 | Hearing Registry | 확정 사본이 등재되는 장부 | c3 | worldview |
| 주민회 사본 | Council Copy | 주민회가 관외에 보관한 사본철. 페이지 번호가 불연속이다 | c3 | worldview |
| 사적 대조표 | Private Cross-Table | 서린이 12년간 혼자 만든 대조표. 단독으로는 확정 근거가 아니다 | c3 | worldview |
| 소금 그늘 | Salt Shade | 잉크가 마르기 전에 소금이 덮인 자리에 생기는 가림. 염도·시간을 역산하면 아래 획을 되살릴 수 있다 | c3 | worldview |
| 결번 | Missing Folio | 사본철에서 페이지 번호가 건너뛴 자리. 원래 무언가 있던 자리다 | c3 | worldview |
| 잠금 보관 | Sealed Storage | 첫 판독을 마친 원본을 옮겨 두는 보관 상태. 이후 판독은 사본으로 한다 | c3 | worldview |
| 판독대 | Reader Stand | 구역 현장에서 판독기를 거치하는 대. 서명대·봉인대와 같은 대(臺) 계열이며, 장치 「판독기」와는 별개 항이다 | c7 | worldview |
| 조습기 | Humidity Regulator | 판독기에 물리는 가습 기구. 단수로 소금 용해 강도를 조절하며 표기는 「조습기 N단」. 판(板)은 기록 매체 전용 계열이므로 기구에는 기(器)를 쓴다 | c7 | worldview |
| 구역 지도 | Zone Map | 네 구역 이동용 지도. 신호 계통을 다루는 「회로 지도」와 별개의 물건이다 | c7 | worldview |
| 서명 요건표 | Signature Requirement Table | 봉인대에 붙은, 서명 자격 요건을 대조하는 표. 규정 필사본의 3항을 칸으로 갖는다. 약칭 「요건표」 | c7 | worldview |
| 1호기 | Pump Unit One | 제1양수장의 주 양수기 실물. 정식 표기는 「제1양수장 1호기」(문서 최초 1회), 이후 약칭 「1호기」로 운용한다 | c7 | worldview |

## 3. 절차 · 행동

| term (KO) | term (EN) | definition | first cycle | owner-lane |
|---|---|---|---|---|
| 연습 | Rehearsal | 확정 전 단계의 모든 조작. 비용 0, 횟수 무제한, 사실을 확정하지 못한다 | c3 | worldview |
| 확정 | Commit | 이중서명 봉인으로 변경을 성립시키는 행위 | c3 | worldview |
| 가상 시험 | Dry Test | 부식 시험대에서 구성안의 부식 비용만 계산하는 절차(법5) | c2 | worldview |
| 가상 완주 | Dry Run | 편성기에서 한 편성안을 끝까지 돌려 결과를 미리 보는 절차(법4) | c3 | worldview |
| 반증 시험 | Falsification Test | 일부러 정합을 무너뜨려 자기 판정 기준이 진짜인지 확인하는 절차 | c3 | worldview |
| 조위정합 | Tide Alignment | 공통 조위 피크 3개로 두 자료의 기준선을 맞추는 행동 | c2 | worldview |
| 이중서명 | Dual Seal | 과거 명령의 책임 요건이자 현재 제출의 검증 절차(법6) | c2 | worldview |
| 배선 추적 | Circuit Trace | 계통선을 따라가며 센서 범위 안과 밖을 비교하는 판독 전용 행동(법1). 자체 확정 명령이 없고, 다른 도구의 확정에서 배선 밖 근거를 무효로 만든다 | c3 | worldview |
| 판독 | Plate Read | 염판 사본을 재생·확대·시간범위 지정으로 읽는 행동(법2). 확정 명령은 그 결과를 가설판에 인용으로 고정하는 것이며, 사본 재생 자체는 비용·횟수 제한이 없다 | c3 | worldview |
| 배수 편성 | Drain Routing | 두 배수 경로를 대기 상태로 편성해 각각 가상 완주 결과를 보고, 우선순위를 확정해 구역 상태를 실제로 바꾸는 행동(법4) | c3 | worldview |
| 부식 시험 | Corrosion Assay | 구성안을 가상 시험대에 올려 총 부식 비용과 전역 상한 9를 확인하는 행동(법5). 자체 확정은 없고 `routing` 확정의 게이트로만 쓰인다 | c3 | worldview |
| 봉인 | Seal | 서명에 부여되는 물리적 효력 표식. 압착으로 완성된다 | c3 | worldview |
| 봉인 완료 접점 | Seal-Complete Contact | 봉인대의 압착이 끝나면 닫히는 기계 접점. 그 시각 한 줄이 염선 계통 로그에 각인된다. **압착이 끝났다는 사실만** 남기고 서명자·확인 여부·유효성은 남기지 않는다 | c3 | worldview |
| 증인 | Witness | 두 번째 봉인을 놓는 사람 | c3 | worldview |
| 대체 검증 절차 | Alternate Verification | 증인이 거부해도 확정을 가능하게 하는 우회 절차(법6 유지, 진행 보장) | c3 | worldview |
| 무료 우회관 | Free Bypass Line | 부식 한도를 넘긴 구성에서 언제나 한 번 쓸 수 있는 복구 경로. 영구 도구 상실을 막는다(법5) | c3 | worldview |
| 청문 | Hearing | 제출 문서를 심리하는 절차. 본편의 종착점 | c3 | worldview |
| 제출 문서 | Submission Document | 7장에서 확정하는 1건. 결말 3갈래는 이 문서의 관점 차이다 | c3 | worldview |
| 확정 게이트 | Commit Gate | 서명대가 봉인을 받기 위한 4조건(G-a~G-d) | c3 | worldview |
| 결과 예고 | Outcome Preview | 확정 직전에 제시되는 결과 요약. 보지 않고 고르지 않는 것이 이 작업대의 방식이다 | c3 | worldview |
| 대조표 | Cross-Table | 두 자료의 항을 나란히 놓고 일치·불일치를 채우는 표 | c3 | worldview |
| 확정 전 보존 | Pre-Commit Preservation | 확정 직전 상태가 당직자의 어떤 조작도 없이 보존되는 절차. 법2의 「자동 사본」(원본 첫 판독의 검증 사본)과는 별개다 | c7 | worldview |
| 복귀 지점 | Return Point | 당직 기록상 되돌아갈 수 있는 지점 | c7 | worldview |
| 연습 압착 | Rehearsal Press | 빈 서식에 하는 연습 단계 압착. §3 「연습」의 봉인대 적용형이며, 확정을 만들지 못한다 | c7 | worldview |
| 연습 서식 | Rehearsal Form | 연습 압착용 빈 서식 | c7 | worldview |
| 보호 지정 | Protection Designation | 배수 편성 확정으로 보호 구역 하나를 지정하는 행위 명사(법4). 5장에서 1건이 확정되며, 종결부 후일담의 저지대·부두 문단 쌍만 바꾼다 | c7 | worldview |
| 공통 종결부 | Common Ending Section | 결말 3갈래가 공유하는 종결 문단 묶음(부(部)=문서 구조 단위). 청문 결과·4구역 최종 상태·서린의 다음 근무 여부가 여기서 확인된다 | c7 | worldview |
| 종결부 후일담 | Ending Section Aftermath | 공통 종결부 안의 후일담 문단. 결과 예고 2층이 가리키는 대상이며, 보호 지정에 따라 어느 쌍으로 바뀌는지가 정해진다 | c7 | worldview |

## 3-1. 도구 표시명 정본 ↔ 데이터 id (RFC-S6 / RFC-C6-001 / C4-F9 / C7-F5 · 2026-09-10 R7 신설)

정본 근거: `production/decision-log.md` 「RFC-S6 / RFC-C6-001 / C4-F9 / C7-F5 · 도구 표시명 정본」(decided_by: game-production-director, 2026-09-10) — 표시명 한 벌은 `planning/gdd.md` §4 = `concept/style-guide.md` §9. id 값의 소유자는 live `planning/campaign.json`(도구 id 6종)이며 이 표는 **연결만** 한다.

이 표의 등재로 6종 전부가 용어집 명사가 된다 → 「미등재 명사는 UI 문자열에 쓸 수 없다」 제약이 도구 표시명에 대해 해소된다(C4-F9 잔여·C7-F5 용어집 몫). **EN 열은 내부 설계 라벨**이며 T0 는 KO 전용이다 — EN 표시 문자열 확정은 로컬라이제이션 회차 산출물이다(RFC-S6) [CARRIED].

| id (데이터 토큰) | term (KO) | term (EN) | 이 용어집의 정본 행 | first cycle | owner-lane |
|---|---|---|---|---|---|
| `circuit` | 배선 추적 | Circuit Trace | §3 「배선 추적」 (R7 신설) | c3 | worldview |
| `reader` | 판독 | Plate Read | §3 「판독」 (R7 신설) | c3 | worldview |
| `alignment` | 조위정합 | Tide Alignment | §3 「조위정합」 (기존) | c2 | worldview |
| `routing` | 배수 편성 | Drain Routing | §3 「배수 편성」 (R7 신설) | c3 | worldview |
| `corrosion` | 부식 시험 | Corrosion Assay | §3 「부식 시험」 (R7 신설) | c3 | worldview |
| `seal` | 이중서명 | Dual Seal | §3 「이중서명」 (기존) | c2 | worldview |

주의(혼동 방지): `corrosion`(행동 「부식 시험」)은 §2 장치 「부식 시험대」·§4 규칙 「부식예산」과 다른 항이다. `reader`(행동 「판독」)는 §2 장치 「판독기」와 다른 항이다. 캐논 변경 없음 — 새 사실·인물·시각을 만들지 않고 기존 6법 서술을 명사로 등재만 했다.

## 4. 규칙 · 상태

| term (KO) | term (EN) | definition | first cycle | owner-lane |
|---|---|---|---|---|
| 불가침 6법 | Six Laws | 세계의 물리·절차 규칙 6개. 호명 문구의 정본은 `worldview-bible.md` §3 표 하나뿐이다(RFC-P3-014) | c3 | worldview |
| 부식예산 | Corrosion Budget | **`routing` 구성안의 총 부식 비용에 걸리는 전역 상한(9)**. 소모·채집·회복 재화가 아니며 판독·회로·봉인 확정으로 차감되지 않는다. 누적 고갈·계통 영구 고장·장 경계 리셋은 없다 (RFC-P3-009) | c2 | worldview |
| 분해능 4분 | Four-Minute Resolution | 기록이 구별할 수 있는 최소 시간 칸 | c3 | worldview |
| 정합 잔차 | Alignment Residual | 정합 후 남는 관측소별 오차. 확정 요건은 ≤4분 | c3 | worldview |
| 8분 오차폭 | Eight-Minute Error Band | 관측소별 ±4분이 두 관측소에 걸칠 때의 총 오차폭. 이보다 큰 간격만 선후를 말할 수 있다 | c3 | worldview |
| 공통 조위 피크 | Common Tide Peak | 두 계통이 함께 기록한 조위 극값. 정합의 기준점(3개 필요). (문맥에서 '공통'이 확정된 뒤는 「조위 피크」로 축약 가능 — R9 · D-CX012-06) | c3 | worldview |
| 오차띠 | Error Band | 한 사건 시각의 허용 범위. 두 띠가 겹치면 순서를 확정하지 못한다 | c3 | worldview |
| unknown 판정 | Unknown Verdict | 확정도 부정도 아닌 상태. 실패가 아니며 제출 문서에 그대로 적힐 수 있다 | c3 | worldview |
| 필수 단서 | Critical Clue | 결말 3갈래 중 최소 한 갈래 도달에 반드시 확정돼야 하는 증거. 목록 소유자는 timeline §7 | c3 | worldview |
| 확정 경로 | Confirmation Pair | **루트 출처가 서로 다른** 매체 2종의 한 쌍 | c3 | worldview |
| 루트 출처 | Root Origin | 사본·재촬영본·표면 부식이 재귀적으로 물려받는 원출처. 표시 라벨은 증명이 아니다 | c3 | systems |
| 불파괴 쌍 | Indestructible Pair | 어떤 플레이어 행동으로도 파괴되지 않고 서로의 사본이 아닌 확정 경로(불변식 P2) | c3 | worldview |
| 결손 4시간 | Four-Hour Gap | 대조의 밤 **H-1:00~H+3:00**의 각인 공백. 본편의 중심 대상 | c3 | worldview |
| 결손 4분 | Four-Minute Gap | 재화 근무일에 되풀이되는 로그 공백. 지목 근거는 단일 표본이 아니라 근무표와 교차한 **반복률**이다 | c2 | worldview |
| 대조의 밤 | Spring-Tide Night | 12년 전(T-12) 사건 당일 밤 | c2 | worldview |

## 5. 인물 · 조직

| term (KO) | term (EN) | definition | first cycle | owner-lane |
|---|---|---|---|---|
| 한서린 | Han Seorin | 주인공, 기록복원사. 가족에 대한 가설과 판 #0은 알지만 대필의 이름과 기관의 동기는 모른다 | c2 | worldview |
| 문재화 | Mun Jaehwa | 항만공사 수문 계장. 12년 전 우선순위 변경 지시자 | c2 | worldview |
| 오은정 | O Eunjeong | 저지대 주민회 총무. 유리한 사본만 제공한다 | c2 | worldview |
| 표성찬 | Pyo Seongchan | 냉동창고 운영주. 그의 대장만 조위 기준 관측소가 다르다 | c2 | worldview |
| 한도연 | Han Doyeon | 서린의 어머니, 당시 당직 주임. 전지적 증인이 아니다 | c2 | worldview |
| 저지대 주민회 | Lowland Residents' Council | 침수 피해 주민 조직 | c2 | worldview |
| 항만공사 운영과 | Port Authority Operations | 사람 우선·기록 차순의 세력 | c3 | worldview |
| 기록국 잔류·감사 계열 | Bureau Audit Remnant | 서면 기준선을 보존하는 세력 | c3 | worldview |

## 6. 제작 용어 (in-fiction 아님, 문서에서만 사용)

| term (KO) | term (EN) | definition | first cycle | owner-lane |
|---|---|---|---|---|
| 저자 진실 연표 | Author Truth Timeline | 작가만 전부 아는 완전 연표(timeline §1·§2) | c2 | worldview |
| 플레이어 인지 연표 | Player Knowledge Timeline | 장별 공개·확정 상한표(timeline §3·§7) | c2 | worldview |
| 반전 R1·R2·R3 | Reversal R1–R3 | 의도적 변경 / 대필 서명 / 고장 은폐 | c2 | worldview |
| 비트 B01~B33 | Beat B01–B33 | 33개 서사 슬롯. 상한은 timeline §7, 내용 저작은 synopsis | c3 | synopsis |
| 톤 기둥 | Tone Pillars | 모든 비주얼·연출이 매핑돼야 하는 3개 축 | c2 | worldview |
| 설계 분량 | design_budget_min | 설계상 배분한 분(합 480). 관측치가 아니다 | c1 | planner |
| 관측 완주 | observed_median_min | 실제 플레이 완주 중앙값. 현재 n=0, 값 없음 | c1 | qa |
| 캐논 | Canon | 6법·인물 5인·결말 3갈래·DLC 독립성 등 수정 금지 사실 | c3 | worldview |
| 레트콘 | Retcon | 캐논 문구를 사후에 다시 쓰는 것. 미출시 단계는 RFC, 출시 후는 season RFC + 연속성 노트 | c3 | worldview |
| expansion_hooks | expansion_hooks | 구역·세력 엔트리 말미의 확장 여지 목록 | c2 | worldview |

## 6-1. 구역 식별자 `zoneId` — 데이터 토큰 ↔ 용어집 대응 (C3-F22 · 2026-09-10 R4 신설)

값의 소유자는 **`planning/campaign.json`**(비트별 `zoneId`, C3-F22로 추가)이며 이 표는 그 5개 값에 KO/EN 정본 명사를 **연결만** 한다. 비트→구역 배정 자체는 여기서 정하지 않는다(파생표는 `planning/content-matrix.md` §3).

재측정 [OBSERVED, 2026-09-10 R4] — `node _workspace/current/planning/validate-campaign.mjs --pairs` 의 `zoneBeatCounts` → `{hub 15, gate 3, pump 6, dock 5, lowland 4}`(합 33), 검증기 `Z-01`(`beat.zoneId ∈ stage.zoneIds`)·`Z-02`(전 비트 존재) **PASS**.

| `zoneId` (데이터 토큰) | term (KO) | term (EN) | 이 용어집의 정본 행 | 비트 수 [OBSERVED R4] | owner-lane |
|---|---|---|---|---|---|
| `hub` | 당직실 (본관 허브) | Watch Room | §1 「당직실」 | 15 | worldview |
| `gate` | 제3수문 | Gate Three | §1 「제3수문」 | 3 | worldview |
| `pump` | 제1양수장 | Pump House One | §1 「제1양수장」 | 6 | worldview |
| `dock` | 냉동창고 부두 | Cold Quay | §1 「냉동창고 부두」 | 5 | worldview |
| `lowland` | 구염전 저지대 | Old Saltern Lowland | §1 「구염전 저지대」 | 4 | worldview |

**파생 규칙 — 모델러 OPEN-M1 판정 (worldview 결정, 2026-09-10)**: 에셋 **오브젝트명·파일명의 zone 토큰은 `zoneId` 영문 토큰을 그대로 쓰는 것을 허용**한다(대소문자·PascalCase 표기는 자유). 용어집 EN(`Watch Room` 등)에서 파생할 의무는 없다 — `zoneId`는 in-fiction 고유명사가 아니라 데이터 식별자이고, 에셋 이름이 데이터 정본과 같은 문자열이면 대조 비용이 0이 되기 때문이다.
- 따라서 `modeling/pipeline.md`의 `Hub`는 **그대로 유효**하다(`hub`의 표기 변형). `Watch`로의 일괄 치환은 **요구하지 않는다** → **OPEN-M1 닫힘**.
- 기존 토큰 읽는 법: `Hub`↔`hub` · `Gate3`↔`gate` · `Pump1`↔`pump` · **`Quay`↔`dock`** · `Lowland`↔`lowland`. `Quay`/`Gate3`/`Pump1`은 용어집 EN에서 파생한 표기라 계속 쓸 수 있으나, **새로 만드는 토큰은 `zoneId` 값을 그대로 쓴다**(두 계보가 더 늘어나지 않게).
- 변하지 않는 금지: 이 용어집에 없는 KO/EN 고유명사를 파일명·UI 문자열·대사에 쓰는 것, 그리고 상표 미확인 가제(기관 가제명·영문 코드네임)를 파일명·오브젝트명·이미지 내 텍스트에 넣는 것(계약 `Unity / 저장소 배치`).
- `Kit`(공용 모듈)는 구역이 아니므로 이 표의 대상이 아니다.

## 6-2. T0 인용 출처 식별자 (RFC-CX-001)

**[TARGET · 저작 결정]** 디렉터가 `handoff/rfc-inbox/RFC-CX-001.md` 및 `production/decision-log.md`의 RFC-CX-001 결정으로 채택한 기술 식별자다. 아래 문자열과 표준판의 기준 관측소 연결은 이번에 명시한 저작 결정이며, 기존 파일에서 추출된 관측 사실이 아니다. **이 절의 상태는 draft(교차 레인 ACK 대기)**이며 기존 용어집의 current 항목과 구분한다. generator 반영·게이트 입력 승격은 디렉터가 필수 ACK를 수집한 뒤 판정한다.

| 기존 한국어 표현 | 영문 대응 | 기술 ID | 필드 | 기존 개념의 근거 |
|---|---|---|---|---|
| 허브 계통 | Hub System | `system-hub` | `systemId` | `synopsis/t0-records.md` §5.1 `valve` 행의 “이 판(허브 계통)”; `worldview-bible.md` §2의 본관 당직실 허브 |
| 기록국 표준 관측소 | Bureau Standard Tide Station | `station-bureau-standard` | `stationId` | `synopsis/t0-records.md` §7 머리말·§7.1 `tideHeight` 행 및 이 문서 §7의 기록국 조위대장 |

**필드 의미.** `systems/data-schemas/plates.md` §1에서 `stationId`는 기준 관측소이자 조위정합 입력이다. 표준판에 이 ID를 부여하는 것은 기록국 표준 조위를 **비교 기준으로 채택**한다는 뜻이다. 표준판의 압력·염도·갑문 내측 수위를 해당 관측소가 생산했다는 뜻이 아니며, 새 관측소·센서·사건·측정값·오차폭을 추가하지 않는다. 회선 3개소의 통화 기록 범위와 조위 기준 관측소를 서로 대체하지 않는다.

### 적용 범위 — 실제 T0 인용 2건만

| `recordId` | `systemId` | `stationId` | 관계의 근거와 저작 범위 |
|---|---|---|---|
| `rec-plate-standard-hub` | `system-hub` | `station-bureau-standard` | 계통 관계는 `synopsis/t0-records.md` §5.1의 기존 명시. 관측소 관계는 RFC-CX-001에서 새로 채택한 **비교 기준 연결 [TARGET]**이며 관측 생산지의 추출이 아님 |
| `rec-tide-ledger-bureau` | `null` | `station-bureau-standard` | 관측소 관계는 `synopsis/t0-records.md` §7·이 문서 §7의 기존 명시. ID 문자열은 이번 기술 저작 결정. 조위 수치대장은 염판 계통의 자료가 아니므로 `systemId`는 JSON `null` |

`systemId`의 형식은 `string?`이며 `sourceType == plate`일 때만 필수다(`systems/data-schemas/plates.md` §1). 조위대장의 `null`은 **계통 비해당**이며 누락된 계통을 임의로 보충하지 않는다. 인용에는 해당 필드를 비해당으로 표현하고 유효한 `stationId`를 보존한다. 두 자료의 독립성은 기존 `sourceType`·`rootOriginId` 규칙으로 판정하며 기준 관측소 공유만으로 동일 출처로 합치거나 독립성을 새로 부여하지 않는다.

`rec-handover-brief`, `rec-transfer-list`, `rec-watchlog-bureau`의 관측소 귀속은 **미정**으로 남긴다. 이 절의 ID를 전파하지 않는다. 특히 `synopsis/t0-records.md` §4.1 `tl-r3`의 관측소는 목록에 든 조위대장 설명이며 이관 목록 자체의 출처가 아니다. synopsis는 이 결정의 두 연결만 형식 고정 표에 저작하고 systems는 승인된 표에서 파생한다. `zoneId`·파일명·동일 수치 곡선으로 출처를 추정하지 않는다.

## 7. 자료 카탈로그 — live `campaign.json` 31 출처 [OBSERVED 재측정]

입력 [OBSERVED, 2026-09-10 R4 재측정 · RFC-Q1(고정 숫자를 손으로 다시 적지 않고 검증기 출력을 인용)]: `node _workspace/current/planning/validate-campaign.mjs` → `sha256 92301c0a5ecfc7e142646f5f17c29c21609e2b2de7c90408de7bffc9d815ae23` · `bytes 121457` · `summary {checks 47, pass 47, fail 0, PASS}`. 앞선 판이 적은 `fdabf1d4…`/`120479`(검사 44종)는 RFC-W4 의도 문장 이동과 `zoneId` 추가 **전** 값이며 폐기한다.
집계 [OBSERVED, 같은 입력]: `node` 로 `stages[].beats[].clues[].{originId,sourceType}` 를 집계 → **31종 / 단서 73건 / sourceType `log 27 · ledger 22 · plate 24`**(합 73), 검증기 `aggregates` 와 전건 일치. **입력이 바뀌었어도 이 세 값은 불변**이다 [OBSERVED, R4 재계산] — 이번 차분은 `zoneId` 필드 추가와 `c4-b3`→`c6-b4` 문장 이동이라 단서·출처 카탈로그를 건드리지 않았다. 미수록 검사 `originId 미수록: []` 재실행 [OBSERVED R4]. 더 앞선 판의 `단서 72 / plate 23`은 수리 전 파일(`775a984c…`)의 값이며 폐기한다(C3-F28). 이 표의 KO 명칭이 대사·UI에 쓸 수 있는 유일한 서류 이름이다. 표는 30종을 싣고 `plate-zero`는 §2 "판 #0"이 보유하므로 **31/31 수록**이다(기계 검사 `missing=[]`). `(규정 부속)`·`(정비 대장 부속)`으로 표기한 2행은 originId가 없는 파생 서류이며 `synopsis/chapter-beats.md` 표 B에서 이름으로만 쓰인다.

**파생 규칙 (R8 · A-15)**: 등재 명사 + 「철」(합철 묶음)은 허용 파생 서류명이다(예: 「계통 표준판철」·「당직일지철」 — 기등재 「주민회 사본철」·「증설 심사철」·「제출 초안철」과 같은 계열). 낱장 수량은 「N점」으로 적는다 — 철(綴)은 묶음이지 수량 단위가 아니다.

**파생 면제 규칙 (R9 · D-CX012-03)**: 등재 명사의 어간 파생 합성어(예: 역대조=대조 어간, 확대 판독=판독 어간)는 정본 행이 있는 어간을 따르며 별도 등재하지 않는다.

| term (KO) | originId | definition | owner-lane |
|---|---|---|---|
| 인수 각서 | `handover-brief` | 당직 인수 문서. 12년 전 대조의 밤 당직 주임이 한도연으로 기재된 공식 문서 | worldview |
| 이관 목록 | `transfer-list` | 오늘 밤 넘길 자료의 인쇄 목록 3줄. 판 #0은 어느 줄에도 없다 | worldview |
| 근무 규정 필사본 | `watchlog-bureau` | 회선 3개소의 통화 기록 범위를 정한 규정 사본 | worldview |
| 이중서명 규정 | `regulation-dual-seal` | 서명자의 자격 요건 3항이 적힌 당시 규정 필사본 | worldview |
| 검증 절차 규정 | `regulation-verification` | 검증자 부재 시 대체 검증 경로와 요건을 명시한 규정 | worldview |
| 당직 자격 명부 | (규정 부속) | 그날 서명 자격을 가진 사람의 명부. 요건 판정의 대조 대상 | worldview |
| 근무표 | `duty-roster` | 12년치 야간 근무자·날짜 기록. 결손 반복률의 대조군 | worldview |
| 기록국 조위대장 | `tide-ledger-bureau` | 기록국 표준 관측소 기준의 조위 수치 대장 | worldview |
| 성찬 대장 | `tide-ledger-seongchan` | 기준 관측소가 기록국 표준과 다른 냉동창고 측 대장 | worldview |
| 조위 예보표 | `forecast-table` | 오늘 만조고 예보. 두 분기 합계 용량과 비교된다 | worldview |
| 당직일지(12년 전) | `watchlog-archive-t12` | 그 시간대를 "전원 계통 불안"으로만 적은 일지 | worldview |
| 서명지 | `signature-annex` | 당직일지 부속 서명지 2장. 두 번째 장 아래가 소금 그늘에 덮였다 | worldview |
| 당직실 표준판 | `plate-standard-hub` | 대조의 밤 허브 계통 표준판 | worldview |
| 제3수문 계통 로그 | `brine-log-gate3` | 외항 수문 계통판의 압력·각인 기록 | worldview |
| 제1양수장 계통 로그 | `brine-log-pump1` | 지하수로 입구 계통판의 잔압 기록 | worldview |
| 부두사무소 회선 기록 | `brine-log-dock` | 통화 개시 시각·계통만 남은 회선 기록 | worldview |
| 당직실 계통 로그 | `brine-log-hub` | 보관실 문 개폐 등 허브 계통 기록 | worldview |
| 시험 시편 | `plate-test-coupon` | 통전·정전 조건의 무늬를 나란히 남긴 기준 시편 | worldview |
| 양수장 정비 대장 | `pump-maintenance-ledger` | 폐쇄 이후 점검 기입 간격과 공백이 남은 대장 | worldview |
| 센서 설치 대장 | `sensor-install-ledger` | 지점별 설치일. 사건 이후 설치된 자리를 가른다 | worldview |
| 증설 심사철 | `expansion-approval-file` | 냉동창고 증설 승인 조건과 사유란이 있는 항만공사 서류철 | worldview |
| 조사 종결 보고서 | `closure-report-t12` | "기록적 호우에 의한 천재" 결론 문장 3개 | worldview |
| 폐쇄 등재 기록 | (정비 대장 부속) | 제1양수장 물리적 폐쇄가 사건 뒤에 등재된 기록 | worldview |
| 주민회 사본철 | `council-copybook` | 기록국 대장을 옮겨 적은 주민회 보관 사본 | worldview |
| 주민회 보관 일지 | `council-custody-log` | 사본철의 반출·보관 이력. 결번 위치의 대조 축 | worldview |
| 기록국 자동 사본 목록 | `bureau-copy-index` | 자동 사본의 등재 대장. 파생 관계가 여기서 드러난다 | worldview |
| 청문 접수 대장 | `hearing-intake-ledger` | 확정 사본 등재와 청구권 소멸 통지일이 남는 장부 | worldview |
| 가족수당 대장 | `family-allowance-ledger` | 서린의 생년이 남아 사건 당시 미성년임이 계산된다 | worldview |
| 이전 면회 일지 | `visit-log` | 도연 면회 3회분. 같은 다섯 문장이 매번 다른 순서로 적혔다 | worldview |
| 대피 완료 보고 | `evac-report` | 두 구역 주민 대피 완료 접수 보고 | worldview |
| 제출 초안철 | `draft-set` | 세 관점의 제출 초안과 각 초안이 요구하는 근거 | worldview |
| 이관 완료 일지 | `transfer-completion-log` | 4구역 최종 상태와 당직 배치 계획이 적히는 마지막 일지 | worldview |

## 8. 폐기 용어 (기록 보존, 사용 금지)

| term (KO) | 폐기 사유 | 근거 |
|---|---|---|
| 모의 재생 (Simulated Playback) | 법2가 이미 "재생 횟수 제한 없음"을 보장하므로 별도 무비용 판독 모드는 불필요하며, 그 존재가 일반 판독에 비용이 있다는 오해를 만든다. live 데이터 사용 0건 [OBSERVED] | RFC-P3-010, `worldview-bible.md` §10 |
| 대기 명령 (Pending Order) | 법4의 연습은 편성기 가상 완주로 실현된다. live 데이터 사용 0건 [OBSERVED]이며 "밸브 명령 대기 흔적" 계열 회귀 문구를 유발한다 | RFC-P3-010, `campaign.meta.md` §4 |
| 계통별 부식 한도 (6계통 14/14/12/9/9/9) | 부식은 소모 자원이 아니다. 계통별 분해는 `economy` 조건부 [TARGET] 표시 전용 | RFC-P3-009 |
