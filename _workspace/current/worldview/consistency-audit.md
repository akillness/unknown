---
updated: 2026-09-11
cycle: 20260909-preproduction-c3
status: current
supersedes: null
owner: game-worldview-architect
---

# G1 세계관 일관성 감사 (C3 종료 수정 루프 재실행)

용도: G1의 **증거 소스 문서**다. 판정은 `qa/gate-measurements.md#g1`이 인용하고 디렉터가 확정한다. 이 문서 자체는 게이트를 PASS로 올리지 않는다.
재실행 사유 [OBSERVED]: RFC-P3-010이 세계관 본문을 세션 P의 아카이브 c3 판으로 재기반했고, RFC-P3-009·012·013·014가 부식예산·T0 상한·사건 시각·법 문구를 확정했다. 앞선 판(C3 1차)의 점검은 폐기된 본문 위에서 수행됐으므로 §2·§3·§4를 전부 다시 실행했다. 앞선 판의 항목 id(A01~A32)는 추적을 위해 그대로 두고 A33~A40을 덧붙였다. **2차(C3-F28)**: 본문 판정은 유지하고 §3 기계 검사 영수증 전체를 수리된 live 파일(`fdabf1d4…`)로 재측정해 교체했으며 그 재측정을 A41로 기록했다.
**3차(C3-F34, 2026-09-10 수정 루프 3)**: 유일하게 남아 있던 violation **A33**이 systems 레인의 정본 교체로 실체를 잃었음을 직접 재측정으로 확인하고, §1 집계 · §2 A33 · §3 「법 문구 유통」행 · §4-1 · §5 · §6을 그 실측 위에서 다시 썼다. 항목 수는 **41로 불변**이다 — 새 항목을 만들지 않고 A33의 verdict만 뒤집었다. 앞선 판이 A33을 현행 `[OBSERVED]` 위반으로 유지한 것은 감사 문서 자신의 스테일이었고, 그 스테일이 §3의 "2차 재측정" 표제 아래 있었다는 점이 결함의 핵심이다.
**4차(R4, 2026-09-10 · C3 종료 판정 묶음 반영)**: 디렉터 판정 **RFC-W4**(4장 효력 / 6장 의도) · **C3-F25**(H-1:40 캐논 유지) · **C3-F30**(B# 색인 정의 단일화) · **C3-F22**(비트 `zoneId`) · **RFC-W3 소멸** · **RFC-Q1/Q2**를 반영해 §1·§2(A22·A37 종결, **A42·A43 신설**)·§3(입력 교체 + 검사 4행 추가)·§4-1(인용 방식 규칙)·§5·§6을 다시 실행했다. 항목 수는 41 → **43**으로 늘었고 늘어난 2건 중 1건은 **open**이다 — 집계를 좋게 만드는 방향의 항목 추가가 아니다. 이 판도 **대체가 아니라 개정**이며 `cycle`·`supersedes:`는 그대로다(RFC-Q2).
**5차(R7, 2026-09-10 · C6/C7 디렉터 판정 묶음 반영)**: **RFC-S6 / RFC-C6-001 / C4-F9 / C7-F5** 판정에 따라 도구 표시명 4건(배선 추적·판독·배수 편성·부식 시험)을 `glossary.md` §3에 등재하고 §3-1(도구 id 6종 ↔ 표시명)을 신설했다. §3 기계 검사에 **등재 검사 4행**(도구 표시명 6종 · id 6종 · zone 토큰 5종 · 총 행 수 재도출)을 추가했다. **캐논 변경·항목 신설 없음** — §2 점검표는 43항목 그대로이며 verdict 변경도 없다. 이 판도 개정이므로 `cycle`·`supersedes:`·`status:`는 그대로다(RFC-Q2). 이 회차에 `campaign.json`은 입력으로 다시 돌리지 않았다(§3 4차 행의 해시·집계는 그 회차 값으로 유지).
**6차(RFC-CX-013 ACK-b, 2026-09-11 · RFC-CX-012 [CARRIED] 이월 마감)**: RFC-CX-012 ACK-b(`term-audit-20260911.md` §ACK 2)가 후속 안건으로 남긴 `c6-b3.consequence` R3 문장 vs §7 B26 상한 긴장을 **A44 신설**로 등재하고, planner ACK-a의 이관(`c6-b4.consequence`로) + 신설 검증기 **K-07** PASS를 세계관 레인이 직접 재측정해 **pass**로 닫았다. 같은 회차에 timeline §7 B13(`c3-b2`) "공통 피크"→"공통 조위 피크" 이형 1셀 정정(timeline 말미 ACK 절). 항목 수 43 → **44**, verdict 변경 0(신설 1건 = pass). **캐논 변경 없음** — glossary 무변경, 유지 13행 개명·재정의 0. 이 판도 개정이며 `cycle`·`supersedes:`·`status:`는 그대로다(RFC-Q2). §3 기계 검사 표는 이번 회차에 다시 돌리지 않았다 — 입력 sha가 바뀌었으므로(ACK-a 교체) §3 표의 4차 값은 그 회차 값이며, 현행 검증기 출력은 A44 행이 인용한다(RFC-Q1: 고정 숫자 재기재 없음). §3 전 행 재실행은 §6 자기 인계 「집계 영수증 재실행」에 따라 다음 세계관 편집 회차 몫으로 남긴다(아래 §1 집계 주석).
범위: 6법 ↔ 인물 5인 ↔ 저자 진실 연표 ↔ 플레이어 인지 연표 ↔ 33비트 상한 ↔ 결말 3갈래 ↔ DLC 독립성 ↔ 용어집 ↔ live `planning/campaign.json`.
방법 [OBSERVED]: 문서·데이터 대조만 수행했다. 빌드·플레이 표본 0건이므로 도달 가능성·진행 막힘은 **문서상 불변식의 존재 여부**만 검사했고 실제 도달률은 측정하지 않았다.
대상: `_workspace/current/worldview/{worldview-bible,timeline,glossary}.md`(2026-09-10 R4판), 대조 입력 `_workspace/archive/20260909-preproduction-c3/worldview/*`, live `planning/campaign.json`(sha `92301c0a…` · 121457 B, 2026-09-10 **R4 재측정**, 검증기 47/47 PASS), `synopsis/{chapter-beats,continuity}.md`, `planning/content-matrix.md`, `modeling/{pipeline,asset-manifest}.md`, `planning/campaign.meta.md`, `systems/{interaction-rules.md,game-ui-contract.json,system-specs/*}`, `production/decision-log.md` RFC-P3-008~015, `qa/c3-review.md`, `qa/defect-register.md`.

## 1. 집계

집계 회차 [OBSERVED]: 아래 표는 **2026-09-11 6차 재도출(RFC-CX-013 ACK-b)** 기준이며, 손으로 세지 않고 §2 표에서 재도출했다. 실행한 명령(행 번호가 아니라 A01·A44 앵커를 쓰므로 편집 후에도 재실행 가능):

```sh
F=_workspace/current/worldview/consistency-audit.md
S=$(grep -n "^| A01 " $F | cut -d: -f1); E=$(grep -n "^| A44 " $F | cut -d: -f1)
awk -v s=$S -v e=$E 'NR>=s && NR<=e' $F | awk -F'|' '{print $5}' | sed 's/ //g' | sort | uniq -c
# → 41 pass · 3 **open** · violation 0 · 행 수 44  [OBSERVED 2026-09-11 6차]
# (4차: 40 pass · 3 open · 0 violation · 43행 [OBSERVED 2026-09-10 R4])
```

| verdict | 건수 | 의미 | 6차 전(4차·5차 값) |
|---|---|---|---|
| pass | **41** | 문서 간 모순 없음 | 40 |
| violation | **0** | 현재 문서가 서로 모순. 소유 레인 수정 필요 | 0 |
| open | **3** | 모순은 아니나 미정의·타 레인 소유로 남음 | 3 |
| **합계** | **44** | | 43 |

증감 내역 [OBSERVED]: **6차** — **A44 신설 = pass**(`c6-b3` R3 문장 §7 상한 정렬, RFC-CX-013 이관 + K-07). 4차 — A22 open→pass(RFC-W4) · A37 open→pass(synopsis 파생 완료) · **A42 신설 = open**(타 레인 표의 B# 파생 불일치) · **A43 신설 = pass**(zoneId 5값 용어집 등재). 4차에 늘어난 항목이 집계를 개선하지 않았다는 점(신설 2건 중 1건 open)은 그대로 남긴다. 6차 A44는 관찰 시점(RFC-CX-012 ACK-b)에 이미 planner 이월 안건으로 열려 있던 것을 이관 완료 뒤 등재한 것이라 open 이력 없이 pass로 들어온다 — 열린 상태의 기록은 `term-audit-20260911.md` §ACK 2에 있다.
**§3 기계 검사 표 회차 주의 [OBSERVED, 6차]**: §3 표의 값은 **4차·R7 회차 값**이며 현행 live 입력(ACK-a 교체 후, 검증기 50검사)으로는 다시 돌리지 않았다. 현행 출력은 A44 행(K-06·K-07·summary·`--pairs`·`--t0`·EG)이 인용한다. 표 전 행 재실행은 §6 「집계 영수증 재실행」 인계대로 다음 세계관 편집 회차에 한다 — 이 문서가 4차 값을 현행으로 오독하지 않도록 여기 적어 둔다.

차단 판단 [OBSERVED, 2026-09-10 4차 재측정]: **violation 0건**(3차에 이어 유지). 앞선 판이 유일한 violation으로 적었던 A33(= 폐기 6법 호명 문구가 `systems/system-specs/*` 5행에 살아 있다)은 systems 레인이 6곳(스펙 5행 + `interaction-rules.md` L101)을 정본으로 교체해 **실체가 사라졌다** — 재측정 `grep -rlE "<법 호명 문구 5종>" _workspace/current/systems` = **0파일 / 0행**(§3 「법 문구 유통 — 폐기(3차)」). 앞선 판의 violation 3건(A10·A11·A31)은 세션 P 본문 재기반으로 이미 해소됐다.
남은 open **3건**은 **A25**(에필로그 조합 축약 · planner/synopsis) · **A29**(EN 표기 상표 조사 n=0 · product-manager) · **A42**(synopsis 표의 B# 파생 불일치 17행 · synopsis)이며 전부 **타 레인 소유 또는 미조사**로, 세계관 본문 내부의 모순이 아니다. 3차의 open 2건은 이번 회차에 닫혔다 — **A22**는 디렉터 판정 RFC-W4 + 검증기 K-06 PASS로, **A37**은 `synopsis/continuity.md` §5.1이 루트·종류가 모두 다른 쌍으로 재작성되고 검증기 `C-07`·`--pairs`(`beatsWithoutPair []`)가 그것을 뒷받침하면서 닫혔다. **A42는 세계관이 정의를 소유하고 타 레인이 파생을 아직 안 맞춘 경우**이므로 세계관 본문 수정으로는 닫히지 않는다.
[INFERENCE] 세계관 레인 자체 감사 기준으로 G1을 막는 항목은 이제 없다. **다만 이 문서는 G1을 PASS로 올리지 않는다** — 판정은 `qa/gate-measurements.md#g1`이 하고, 빌드 0줄·플레이 표본 n=0이라는 조건은 이번 회차에도 그대로다.

## 2. 점검표 (41항목)

| # | artifact | claim (검사한 주장) | verdict | fix / 해소 경로 |
|---|---|---|---|---|
| A01 | bible §3 법2 ↔ 계약 E1 | 재생 소진으로 증거가 영구 소실돼 필수 단서 보존과 충돌한다 | pass | 정본 법2가 "필수 단서 삭제/재생 횟수 제한 없음"을 이미 명시. 소진 전제 자체가 폐기됨 |
| A02 | bible §3 법4 ↔ E1·E3 | 선택하지 않은 구역의 증거·인물 접근이 상실된다 | pass | 정본 법4 "사건 증거는 이미 복제. 물자·후일담만 달라지며 진행·결말 선택은 보존" |
| A03 | bible §3 법5 ↔ E2 | 한도 초과가 계통 영구 고장을 부른다 | pass | 정본 법5 "무료 우회관으로 언제나 복구, 영구 도구 상실 없음" + RFC-P3-009(소모 없음) |
| A04 | bible §2 ±40분 ↔ timeline §2 20분 간격 | 오차보다 작은 간격의 선후를 확정할 수 없다 | pass | 정합 후 잔차 ±4분 → 총 오차폭 8분 < 간격 20분. 앵커 근거는 timeline §8(같은 계통 로그의 두 각인) |
| A05 | bible §3 법1 ↔ 진행 보장 | 미배선 구간 추정 확정이 권한을 박탈한다 | pass | 정본 법1 "판정 보류 후 다른 매체 수집. 필수 권한 박탈 없음" |
| A06 | bible §3 법6 ↔ §6 결말 3갈래 | 증인 거부가 제출을 막는다 | pass | 정본 법6 "NPC가 거부해도 대체 검증 절차가 열림". live `c7-b1` [OBSERVED] |
| A07 | bible §3 법2 ↔ timeline §7 B03 | 튜토리얼 판독이 한정 자원을 초반에 태운다 | pass | 첫 판독은 자동 사본을 남긴다. 태울 자원이 없다 |
| A08 | bible 3-bis.3 P4 ↔ timeline §4 R1 | 플레이어 부식과 12년 전 부식 무늬가 같은 계통이면 증거가 자기오염된다 | pass | P4가 계통 분리를 요구. 부식 비용은 `routing` 구성안에만 걸린다(RFC-P3-009). 계통 지정 확인은 systems |
| A09 | bible 3-bis.4 ↔ `qa/c2-review.md` F4 | 1야간에 예산이 회복되지 않아 후반이 고갈된다 | pass | 소모 자원이 없으므로 고갈·회복 개념이 성립하지 않는다 |
| A10 | bible §4 서린 ↔ timeline §5 | 주인공 인지 "0"인데 판과 대조표를 갖고 있다 | pass | **해소.** 재기반된 §7·§5가 "가족에 대한 가설과 판 #0은 알되 대필의 이름은 모름"으로 명시 |
| A11 | bible §4·§8 판 #0 ↔ bible §2 매체 한계 | 염판에 이름이 각인될 수 없다 | pass | **해소.** 정본은 "**번호만** 적힌 미봉인 염판". 이름은 서명지(live `e0-b2` [OBSERVED]) |
| A12 | bible §4 도연 ↔ 1야간 | 반복 면회가 여러 날을 전제한다 | pass | 정본 증거 열이 "이전 면회 일지"다. live `c4-b4`가 전화+일지 3회분으로 실현 |
| A13 | bible §4 재화 ↔ 불변식 P1 | NPC가 예산을 회수하면 필수 단서가 잠긴다 | pass | 정본 행동은 "출처를 공식 기록에 남기길 요구". 예산 회수 문구 자체가 폐기됨(live `c1-b1`) |
| A14 | bible §4 은정 ↔ §6 3갈래 | 조건부 증인이 특정 갈래를 봉쇄한다 | pass | A06과 동일 근거. 조건부성은 비용·신뢰 축으로만 작동 |
| A15 | bible §4 성찬 ↔ E1 | 거래로 필수 단서가 사라질 수 있다 | pass | P5: 거래가 바꾸는 것은 편의·후일담·신뢰 |
| A16 | bible §4 도연 ↔ 법6 | 어머니의 기억이 단독 증거로 쓰이면 법6이 무너진다 | pass | 바이블 명문화 + timeline §7 B27 금지 열 |
| A17 | timeline §4 R3 ↔ §1 | 폐쇄일이 사건 이전인데 그날 1호기가 돌았다 | pass | **해소.** §1이 T-13(성능 저하·정상 보고)과 T-12 이후(물리적 폐쇄 등재)로 분리. live `c6-b1` [OBSERVED] |
| A18 | bible §2 분해능 4분 ↔ "결손 4분" | 분해능 1칸이라 단일 누락과 구별되지 않는다 | pass | **해소.** live `c2-b4`가 근무표 교차 **반복률**로 재정의하고 단일 표본 지목 불가를 명시. §7 B11 상한에 반영 |
| A19 | timeline §1 T-0(D-21) ↔ 제목 "마지막 당직" | 게임이 3주인지 하룻밤인지 정해지지 않았다 | pass | 정본 §1이 "3주 전 폐국 고지 … 이관 전 마지막 밤 21:00~05:00"로 직접 규정. §1에 D-1 행 추가 |
| A20 | timeline §8 앵커 ↔ §2 | 앵커 행이 진실표와 모순되는가 | pass | 앵커를 "같은 염판 1매"에서 **같은 계통 로그의 두 각인**으로 교체해 §2 H-1:24·H-1:04 행과 동일 근거가 됨 |
| A21 | timeline §7 검산 ↔ 계약 Time acceptance | 33비트·480분이 맞는가, 480이 관측치로 오인되는가 | pass | 재측정: 3+4×7+2=33, 25+50+55+65+65+70+75+65+10=480. `design_budget_min` 명시, `observed_median_min` n=0 별도 키 |
| A22 | timeline §3 장별 오해 열 ↔ §7 B18 ↔ live `c4-b3`·`c6-b4` | live `c4-b3`가 4장에서 "방패가 아니라 잠금장치"라고 말해 §3의 6장 재해석보다 앞선다 | pass | **해소 · closed (2026-09-10 R4, RFC-W4).** 디렉터 판정 = 연표 §3이 이긴다(4장 효력 / 6장 의도). planner가 의도 문장을 `c6-b4`로 이동했고 세계관 레인이 직접 재측정: `node planning/validate-campaign.mjs` **K-06** = `PASS`, `actual {"c4-b3": false, "c6-b4": true}` [OBSERVED 2026-09-10 R4]. timeline §9 OPEN-4 **해소**, §3에 「RFC-W4 일치 확인」 절 추가, §7 B18 행의 금지 열을 "의도는 B27=`c6-b4`로 이월"로 명시 |
| A23 | timeline §4 ↔ §7 | 씨앗과 회수가 33슬롯 안에 모두 존재하는가 | pass | R1 B02→B10, R2 B05·B06·B07→B17·B18→B27, R3 B08·B11·B12→B24·B25→B27 |
| A24 | timeline §7 B23 ↔ bible §6 주석 | 5장 확정이 결말 자격을 잠그는가 | pass | P5 + §6 주석 + B23 금지 열. live `c5-b4` "제출 자격이 아니다" [OBSERVED] |
| A25 | bible §6 에필로그 4항목 ↔ §7 에필 2비트 10분 | 잃은 구역 × 결말 3갈래 조합이 저작량을 폭발시킨다 | **open** | E0가 10분·2비트로 더 좁아졌다 [OBSERVED]. 템플릿 고정·축약은 planner·synopsis 소유. 세계관은 상한만 제시 |
| A26 | bible §6 DLC ↔ 결말 3갈래 | DLC가 본편 결말에 의존하는가 | pass | "어떤 갈래에서 시작해도 성립". B33은 배경 언급만 |
| A27 | bible §6 "떡밥 금지" ↔ §7 B32·B33 | 에필로그가 DLC 구매를 요구하는 미완인가 | pass | B32가 4항목을 닫고 B33 금지 열이 미완 인상을 막는다 |
| A28 | glossary ↔ bible·timeline 신규 명사 | C3가 도입한 새 명사가 100% 용어집에 있는가 | pass | §3 기계 검사 `missing_count=0` (34종) |
| A29 | glossary EN 열 ↔ 계약 Unity/저장소 배치 | 영어 표기가 상표·동명 확인을 거쳤는가 | **open** | 조사 0건. 용어집 머리에 "내부 설계 라벨, 공개물 사용 금지" 유지. 상표 조사는 product-manager 소유 |
| A30 | glossary ↔ bible §8 후보표 15항목 | C2 후보표가 전부 승격됐는가 | pass | 15/15 승격. §8은 출처 기록으로만 잔존 |
| A31 | synopsis 캠페인 T0 ↔ bible §4 판 #0 소유 | 캠페인이 판 #0을 "남이 두고 간" 물건으로 쓴다 | pass | **해소.** live `t0-b1` "내가 구 서고에서 빼내 숨긴 것" [OBSERVED], `campaign.meta.md` §5 F5 |
| A32 | timeline §7 ↔ 판 #0 기능 | 판 #0이 본편 중반 내내 기능이 없다 | pass | live 6비트 실사용(`t0-b1 c1-b2 c1-b3 c4-b2 c6-b3 e0-b2`) [OBSERVED] |
| A33 | bible §3 ↔ `systems/system-specs/*` 헤더 | 같은 법을 두 레인이 다른 이름으로 부른다 | pass | **해소(C3-F3 closed → 본 문서 3차 재측정, C3-F34).** 세계관 레인이 직접 재측정: 6개 스펙 9행의 법 호명이 bible §3 L44~L49와 **6/6 문자 일치**(법1 `wiring-trace` … 법6 `dual-seal`), `_workspace/current/systems`의 폐기 문구 **0파일 / 0행** [OBSERVED 2026-09-10]. 근거 `qa/c3-review.md` §8.1 S5·§8.2 · `systems/tech-verification/c3-fixloop2-canon-alignment.md`. 재발 방지: 각 스펙 L11이 "정본 = bible §3 / 폐기 보존 = 본 문서 §4-1"을 인용주로 고정 |
| A34 | bible 3-bis.2·glossary §4 ↔ RFC-P3-009 | 부식예산 정의가 하나인가 | pass | 용어집 "전역 상한 9·무소모"로 교체, bible 차감 문구 삭제. `balance/balance-sheet.md` §4 재작성은 balance 소유 |
| A35 | timeline §2 ↔ live `campaign.json` 시각 | 캐논과 데이터가 선후를 반대로 말한다 | pass | 재측정: live `H-1:24` 2건·`H-1:04` 2건·`H+0:12` 1건, `H-1:20`·`H+0:10` 0건. timeline §2가 RFC-P3-013으로 동일해짐. `chapter-beats.md` B17 문구는 synopsis 소유 |
| A36 | worldview frontmatter ↔ 아카이브 c3 | 두 계보가 c2를 함께 supersedes 하는 포크·고아 | pass | bible·timeline의 `supersedes`를 c3 아카이브로 재연결 [OBSERVED, 재측정: current에서 c3 아카이브 참조 7파일] |
| A37 | bible 3-bis.3 P2·P3 ↔ `interaction-rules.md` §3 ↔ `synopsis/continuity.md` §5.1 | 확정 사본·주민회 사본이 원본의 루트를 물려받아 쌍이 무효화된다 | pass | **해소 · closed (2026-09-10 R4, 디렉터 「A37 / C3-F12」 판정).** bible P2·P3은 앞선 회차에 "서로의 사본이 아닐 것"으로 정정됐고, 이번 회차에 **synopsis 파일을 다시 열어 확인**했다 — `continuity.md` §5.1 K1~K10이 사본 조합을 전부 버리고 `originId`(sourceType) 쌍으로 재작성됐으며(예: K6 `brine-log-gate3`(plate) × `pump-maintenance-ledger`(ledger)), "거부되는 조합"을 별도로 기록한다. 데이터 뒷받침 [OBSERVED R4]: 검증기 `C-07`(루트 상이 AND 종류 상이) `PASS`, `--pairs` 출력 `proofRequiredBeats 15` · `beatsWithoutPair []`. **잔여(비차단)**: §5.1이 파생 출처인 `--pairs`를 명시 인용하지 않고 B# 3행이 정의와 어긋난다 → **A42**로 분리 |
| A38 | glossary ↔ live 데이터·UI 계약 명사 | 용어집 미수록 명사가 이미 쓰이고 있다 | pass | §3 기계 검사. F13 지목 14건 + 추가 20건 승격, `originId` 31/31 수록 |
| A39 | timeline §7 B01 ↔ RFC-P3-012 | T0가 '한도연'·'판 #0'을 노출하는 것이 상한 위반인가 | pass | B01 허용 열에 인수 각서 기재와 판 #0 소유를 명시하고, 금지 열을 "서명란의 이름 '서린'(B17까지)"로 한정 |
| A40 | timeline §7 분량표 ↔ live 계보 | 33비트 상한표가 폐기된 30·50·…·25 계보 위에 서 있다 | pass | **R4 재실행 [OBSERVED 2026-09-10]**: 현행 입력 sha `92301c0a…`·121457 B로 다시 돌려도 값이 같다(3차 입력 `fdabf1d4…`·120479 B): 9 스테이지 / 33 비트 / 25·50·55·65·65·70·75·65·10 = 480으로 재도출(RFC-P3-008). 수리 전 판(`775a984c…`) 대비 스테이지 분·비트 수 변동 없음 [CARRIED · `campaign.meta.md` §11: 차분은 단서 1건] |
| A41 | glossary §7 머리글 · 본 문서 §3 ↔ live `campaign.json` | 집계 영수증이 planner 수리 **전** 판(sha `775a984c…` / 단서 72 / `plate` 23)을 [OBSERVED]로 적는다 | pass | **해소(C3-F28) · R4에서 재이행**(입력이 다시 바뀌어 `92301c0a…`·121457 B로 교체, §3·§5).** 두 문서의 해시·크기·단서 수·`sourceType` 분포를 `fdabf1d4…`·120479 B / 73 / `log 27 · ledger 22 · plate 24`로 교체. `originCatalogSize`는 31로 불변이고 추가 단서의 `originId`(`brine-log-gate3`)가 이미 수록돼 있어 **C3-F13 수록 판정과 §7 카탈로그 본문은 영향받지 않는다** [OBSERVED, `missing=[]` 재실행] |
| A42 | timeline §7-0 B# 정의 ↔ `synopsis/chapter-beats.md` 표 A · `synopsis/continuity.md` §5.1 | 같은 B#가 문서에 따라 다른 비트를 가리킨다(C3-F30) | **open** | **정의는 이번 회차에 고정됐다** — timeline §7-0이 한 줄 정의를 소유하고 표 33행이 그 정의와 **전건 일치**(재도출 명령 §3). planner `content-matrix.md` §3도 33행 **불일치 0**. 남은 것은 synopsis 두 표다 [OBSERVED 2026-09-10 R4 기계 대조]: `chapter-beats.md` 표 A **14행**(`c1-b1 c1-b2 c1-b3 c2-b1 c2-b2 c3-b3 c3-b4 c4-b1 c4-b2 c4-b3 c5-b3 c5-b4 c7-b2 c7-b3`), `continuity.md` §5.1 **3행**(K2 `c1-b2` · K3 `c1-b3` · K7 `c4-b2`, 추가로 K5 본문의 `c3-b3` 참조 1건). `chapter-beats.md` §0의 "B#는 synopsis가 유지하는 고정 라벨"·"두 체계" 주석은 C3-F30 판정으로 **폐기 대상**이다. 소유: **synopsis**(세계관 본문 수정으로 닫히지 않음) |
| A43 | glossary §6-1 ↔ live `campaign.json` `zoneId` (C3-F22) | 데이터가 쓰는 구역 토큰 5종이 용어집에 없어 에셋·UI 이름의 출처가 불명확하다 | pass | **해소(2026-09-10 R4).** glossary **§6-1 신설** — `hub·gate·pump·dock·lowland` 5값에 KO/EN 정본 행을 연결하고, 값의 소유자가 `campaign.json`임을 명시(비트→구역 배정은 복제하지 않음). 재측정 [OBSERVED R4] `--pairs.zoneBeatCounts` = `{hub 15, gate 3, pump 6, dock 5, lowland 4}`(합 33), 검증기 `Z-01`·`Z-02` PASS. 모델러 **OPEN-M1 닫힘**: 오브젝트명 토큰은 `zoneId` 영문 토큰을 그대로 써도 되며 `Hub`는 유효, `Quay`↔`dock` 대응을 기록 |
| A44 | live `c6-b3.consequence`·`inference` ↔ timeline §7 B26(`c6-b3`)/B27(`c6-b4`) 상한 | `c6-b3.consequence`의 R3 동기("1호기 고장 은폐"·"창고 보호=사후 설명")·R2 의도 재해석("거부 수단") 문장이 §7 B26 상한(순서 확정까지)을 한 비트 앞선다 — RFC-CX-012 ACK-b(`term-audit-20260911.md` §ACK 2) 관찰, §7 우선(RFC-W4 선례) 판정 후 planner 편집 회차로 이월 | pass | **해소 · closed (2026-09-11, RFC-CX-013 ACK-a 이관 + ACK-b 확인).** planner가 R3 동기·R2 재해석 문장을 `c6-b3.consequence`/`inference`에서 빼고 `c6-b4.consequence` 말미로 **이관**했다(전후 원문 `planning/field-classification-20260911.md` §RFC-CX-013 ACK §3). 세계관 레인이 직접 재측정 [OBSERVED 2026-09-11]: live `c6-b3.consequence` = "순서 앵커가 확정 항목으로 오른다 — 밸브가 봉인 완료보다 20분 앞서 돌았고 … 그 이유의 확정은 다음 비트로 넘어간다" (B26 문면 일치) · `c6-b4.consequence` 말미 "R3 확정. 실제 동기는 1호기 고장 은폐이고 \"창고 보호\"는 사후 설명이다. 동시에 R2가 재해석돼 무효 서명이 도연의 거부 수단이었음이…" (B27 문면 일치). 기계 검사: `node planning/validate-campaign.mjs` **K-07**(RFC-CX-013 R3 회수 위치 = c6-b4, 동기 문구 3종 `c6-b3` consequence·inference·objective 부재 · `c6-b4` 존재) = `PASS`, `actual {"c6-b3": false, "c6-b4": true}` · **K-06** 유지 `{"c4-b3": false, "c6-b4": true}` · summary `{checks 50, pass 50, fail 0, PASS}` · `--pairs` `beatsWithoutPair []`(독립쌍 17) · `--t0` 5/5 · `emit-evidence-graph` → `validate-evidence-graph` **18/18 PASS**. 완료 조건(ACK-b가 건 49→50 + EG 18 동시 PASS) 충족. 잔여(비차단, 세계관 소유 아님): `c6-b3.objective`/`completion`의 "이유·동기" 문장은 K-07 문구 검사 밖의 문장 긴장으로 planner가 디렉터 종합에 올렸다(ACK-a §6) — 본 항목 verdict에 영향 없음 |

## 3. 기계 검사 [OBSERVED · 입력 = live `campaign.json` sha `92301c0a…` · 121457 B · 검증기 47/47 PASS]

**회차 표기 규칙 [OBSERVED, C3-F34 대응]**: 한 표에 회차가 다른 값을 섞지 않기 위해 행마다 재측정 회차를 적는다. **R4에서 입력 파일이 바뀌었으므로(`fdabf1d4…` → `92301c0a…`, RFC-W4 문장 이동 + `zoneId` 추가) 이 표의 모든 행을 4차로 다시 돌렸다** — §6 자기 인계 「집계 영수증 재실행」의 이행이다. 아래는 전부 **4차(2026-09-10 R4)** 값이며, 3차와 값이 같은 행에는 `(3차와 동일)`을 적는다. 앞선 판은 표제만 "2차 재측정"이라 적고 「법 문구 유통」행을 1차 값 그대로 두어 스테일 `[OBSERVED]`를 만들었다 — 그 재발을 막는 것이 이 규칙이다.

| 검사 | 명령 | 결과 |
|---|---|---|
| live 캠페인 해시·크기 (**4차**) | `node planning/validate-campaign.mjs` 출력의 `sha256`·`bytes`·`summary` (RFC-Q1) | `92301c0a5ecfc7e142646f5f17c29c21609e2b2de7c90408de7bffc9d815ae23` / `121457` / `{checks 47, pass 47, fail 0, PASS}`. 3차 입력 `fdabf1d4…`/`120479`(검사 44)는 RFC-W4 이동·`zoneId` 추가 전 값 — 폐기 |
| 스테이지·비트·분 (**4차**) | `python3`/`node` 로 `stages[].{id,minutes,len(beats)}` | 9 스테이지 / 33 비트 / 25·50·55·65·65·70·75·65·10 = 480 **(3차와 동일)** |
| 단서·출처 (**4차**) | `node` 로 `clues[].{originId,sourceType}` · 대조 검증기 `aggregates` | 단서 **73** / 출처 **31종** / `log 27 · ledger 22 · plate **24**` (합 73) **(3차와 동일 — 입력이 바뀌었어도 단서 축은 불변)** |
| 용어집 수록 (F13 목록 + 추가) (**4차**) | `grep -c "^\| <term> \|" glossary.md` 34종 | `missing_count=0` **(3차와 동일)** |
| `originId` 수록 (**4차**) | `node` 로 31종을 glossary 본문과 대조 | 미수록 `[]` (30종은 §7, `plate-zero`는 §2) **(3차와 동일)** |
| 용어집 총 행 수 (**4차**) | `awk -F'|' '/^## /{sec=$0} /^\|/{if(NF>3 && $2 !~ /^ *-+ *$/ && $2 !~ /term \(KO\)/ && $2 !~ /`zoneId`/)c[sec]++} END{...}' glossary.md` | **130행** (§1 10 · §2 30 · §3 17 · §4 15 · §5 8 · §6 10 · **§6-1 5** · §7 32 · §8 3). 3차 125행 대비 **+5 = §6-1 zoneId 5행**뿐이며 기존 행 삭제·개명 0 |
| 캐논 시각 (**4차**, C3-F25 반영) | `grep -o "H-1:24\|H-1:04\|H+0:12\|H-1:20\|H+0:10\|H-1:40" campaign.json \| sort \| uniq -c` | `H-1:24` 2 · `H-1:04` 2 · `H+0:12` 1 · **`H-1:40` 3** · `H-1:20` **0** · `H+0:10` **0**. C3-F25 판정(폐기는 H-1:20·H+0:10 두 시각뿐, H-1:40은 캐논)과 데이터가 일치 |
| 회귀 금지 문구 (timeline 본문) (**4차**) | `grep -c "서명 확인\|봉인 호출\|밸브 명령 대기 흔적" timeline.md` | **0건** — R4 개정 후 재실행 **(3차와 동일)** |
| 법 문구 유통 — 폐기 (**4차**) | `grep -rlE "<법 호명 문구 5종>" _workspace/current/systems \| wc -l` · `grep -rnE … \| wc -l` | **0파일 / 0행** [OBSERVED 2026-09-10 **4차 재실행**]. 패턴은 손으로 다시 적지 않고 아래 §4-1 표에서 추출했다(자기 계수 회피): `RE=$(awk '/^### 4-1/,/^법1 /' $F | grep '^| [2-6] |' | sed 's/^| [0-9] | "//' | sed 's/".*//' | sed 's/ — .*//' | paste -sd'|' -)`. → A33 pass **(3차와 동일)** |
| 법 문구 유통 — 전수 (**4차**) | `grep -rlE "<법 호명 문구 5종>" _workspace/current` 후 파일별 `grep -c` | **4파일 / 14행이며 전부 기록** [OBSERVED 4차]: `planning/gdd.md` 1(출처주) · `qa/c3-review.md` 7 · `qa/gate-measurements.md` 1(검증 명령) · 본 문서 §4-1 5(폐기 원장). **본문 사용 0** **(3차와 동일)**. 주의 [OBSERVED]: 같은 줄에 문구가 2종 이상 있으면 *행* 수와 *출현* 수가 다르다 — 4차에 패턴별 합계도 재봤고 `gdd 1 · c3-review 15 · gate-measurements 5 · 본 문서 5`였다. 회귀 신호로 쓰는 축은 여전히 **systems 0 + 본문 사용 0** 두 개다 |
| 법 문구 유통 — 정본 (**4차**) | `sed -n '9p' systems/system-specs/{6종}.md` ↔ `worldview-bible.md` L44~L49 | 법1~법6 호명 **6/6 문자 일치** [OBSERVED 4차 재실행] **(3차와 동일)** |
| supersedes 재연결 (**4차**) | `grep -rl "archive/20260909-preproduction-c3/worldview" _workspace/current` | **8파일** [OBSERVED 2026-09-10 3차 재실행]: `worldview/{bible,timeline,consistency-audit}` · `concept/style-guide.md` · `production/decision-log.md` · `qa/{c3-review,defect-register,gate-measurements}`. 앞선 판 0건 → 고아 해소. 7파일은 1차 측정 시점 값이며 2·3·**4차** 모두 8로 일치 |
| **B# 색인 파생** (**4차 신설**, C3-F30) | `node -e '…for(const s of c.stages)for(const b of s.beats){n++;…}'` 로 정의를 코드화한 뒤 `timeline.md` §7 · `planning/content-matrix.md` · `synopsis/chapter-beats.md` 표 A · `synopsis/continuity.md` §5.1 의 (B#, id) 쌍과 대조 | timeline §7 **33행 / 불일치 0** · content-matrix **33행 / 불일치 0** · chapter-beats 표 A **33행 / 불일치 14** · continuity §5.1 **10행 / 불일치 3** [OBSERVED 2026-09-10 R4] → **A42 open**(소유 synopsis) |
| **비트 `zoneId`** (**4차 신설**, C3-F22) | `node planning/validate-campaign.mjs --pairs` 의 `zoneBeatCounts` · 검증기 `Z-01`·`Z-02` | `{hub 15, gate 3, pump 6, dock 5, lowland 4}` 합 33 · `Z-01`·`Z-02` **PASS**. glossary §6-1이 5값 전부를 KO/EN 정본 행에 연결 → **A43 pass** |
| **RFC-W4 의도 문장 위치** (**4차 신설**) | 검증기 `K-06` | `PASS` · `actual {"c4-b3": false, "c6-b4": true}` → **A22 pass**, timeline §9 OPEN-4 해소 |
| **불파괴 자료쌍 파생** (**4차 신설**, A37/C3-F12) | 검증기 `C-07` · `--pairs` | `C-07 PASS` · `proofRequiredBeats 15` · `beatsWithoutPair []`. `continuity.md` §5.1이 사본 조합을 버리고 루트·종류 상이 쌍으로 재작성됨을 파일 재독으로 확인 → **A37 pass** |

| **도구 표시명 등재** (**R7 신설**, RFC-S6 / RFC-C6-001 / C4-F9 / C7-F5) | `for t in "배선 추적" "판독" "배수 편성" "부식 시험" "조위정합" "이중서명"; do grep -c "^| $t |" glossary.md; done` | **6/6 = 1** [OBSERVED 2026-09-10 R7]. R7 이전에는 앞 4종이 `0`이었다(QA X-16). 등재로 「미등재 명사는 UI 문자열에 쓸 수 없다」 제약이 도구 표시명에 대해 해소 → C4-F9 잔여·C7-F5 **용어집 몫** 충족. EN 표시 문자열 확정은 로컬라이제이션 회차 몫이므로 **C7-F5 전체가 닫히지는 않는다** |
| **도구 id ↔ 표시명 대응** (**R7 신설**) | `for i in circuit reader alignment routing corrosion seal; do grep -c "^| \`$i\` |" glossary.md; done` | **6/6 = 1** [OBSERVED 2026-09-10 R7] — glossary §3-1. id 값의 소유자는 live `campaign.json`이며 이 표는 연결만 한다(값 재기재 없음) |
| **zone 토큰 KO 대응 재확인** (**R7 재실행**, C3-F22) | `for z in hub gate pump dock lowland; do grep -c "^| \`$z\` |" glossary.md; done` | **5/5 = 1** [OBSERVED 2026-09-10 R7] — §6-1 불변. 4차 `zoneBeatCounts` 값은 이 회차에 재측정하지 않았다(캠페인 데이터 미변경 · 아래 한계 참조) |
| **용어집 총 행 수** (**R7 재도출**) | 4차와 같은 awk에 `$2 !~ /id \(/` 배제를 추가(§3-1 헤더 제외) | **140행** (§1 10 · §2 30 · §3 **21** · §3-1 **6** · §4 15 · §5 8 · §6 10 · §6-1 5 · §7 32 · §8 3) [OBSERVED 2026-09-10 R7]. 4차 130행 대비 **+10 = §3 도구 4행 + §3-1 대응 6행**뿐이며 기존 행 삭제·개명·정의 변경 0 → **캐논 변경 없음** |

한계 [OBSERVED]: 용어 검사는 **내가 열거한 명사 목록과 `originId` 카탈로그**에 대한 것이다. 문서 전체에서 고유명사를 자동 추출한 전수 검사는 아니며, 시놉시스 초고 확정 후 추출 스크립트로 다시 돌려야 한다.

QA S5와의 차이 [OBSERVED · 판정에 영향 없음]: `qa/c3-review.md` §8.1 S5는 전수를 **4파일 / 13행**(`qa/c3-review.md` 6)으로 적고, 본 문서 3차 재측정은 **4파일 / 14행**(`qa/c3-review.md` 7)이다. [INFERENCE] 차이 1행은 `qa/c3-review.md` 안에 **S5 명령 행 자체가 기록되면서** 늘어난 것이다(자기 계수). 두 측정은 **`systems` 레인 0**과 **본문 사용 0**에서 완전히 일치하므로 A33 판정은 어느 값으로도 동일하다.

## 4. 사용 금지 문구 목록 (RFC-P3-014, 기록 보존)

세계관 문구의 정본은 `worldview-bible.md` §3(6법)·§4(인물)이며 아카이브 c3(세션 P) 본문이다. 아래 문구는 **어느 레인의 어느 문서에도 쓰지 않는다.** 삭제하지 않고 여기 보존해 다음 사이클이 같은 논의를 반복하지 않게 한다.

### 4-1 폐기된 6법 호명 문구 (C2 계보)

| 법 | 사용 금지 문구 | 정본 문구 | 폐기 사유 |
|---|---|---|---|
| 2 | "판은 재생할수록 닳는다 — 염판 1매 최대 3회, 4회째 결정 붕괴" | **원본은 닳지만 사본은 남는다** | 재생 소진이 필수 단서 보존(E1)과 충돌. 자동 사본이 정본 |
| 3 | "시계는 조수에 매인다" | **정합 전 시계는 믿지 않는다** | ±40분이 정합 **전** 값임을 문구가 감춘다 |
| 4 | "물은 한 번에 한 곳으로만 간다 — 선택하지 않은 구역의 증거·인물 접근 상실" | **이번 조수에는 보호 용량이 부족하다** | 결말 비의존(E3) 위반. 주민 선(先)대피가 정본 |
| 5 | "소금은 모든 것을 먹는다 — 한도 초과 시 계통 영구 고장" | **소금은 비용으로 보인다** | 영구 상실이 무제한 되돌림(E2)과 충돌. RFC-P3-009로 소모 자체가 없음 |
| 6 | "당직은 하나, 서명은 둘" | **원본 책임과 제출을 나눈다** | 과거 명령의 책임 요건과 현재 제출 절차를 구분하지 못한다 |

법1 "배선된 것만 남는다"는 두 계보가 동일하므로 금지 대상이 없다.
**해소 이력 [OBSERVED, 2026-09-10 · 3차 재측정으로 확인]**: 위 문구들은 2026-09-10 systems 수정 루프 2 **이전까지** `systems/system-specs/{plate-readout,drainage-routing,corrosion-budget,dual-seal,tide-alignment}.md` 각 9행과 `systems/interaction-rules.md` L101에서 쓰였다(A33 · C3-F3). systems 레인이 그 6곳을 bible §3 정본으로 교체했고, **현재 `_workspace/current/systems` 잔존은 0파일 / 0행**이다(§3 「법 문구 유통 — 폐기(3차)」). 따라서 이 절은 **위반 목록이 아니라 재유입 방지용 원장**이다 — 위 스펙 6종의 L11이 이 절을 폐기 문구의 유일한 보존 위치로 인용하므로, 문구 자체는 삭제하지 않고 그대로 둔다.
**인용 방식 규칙 [OBSERVED, 2026-09-10 R4 · RFC-W3 판정에서 채택된 QA 대안]**: 이 절을 가리키는 타 레인의 인용주는 **절 번호(§4-1)가 아니라 제목 문자열 「폐기된 6법 호명 문구」로 쓴다.** 이유 — `systems/system-specs/*` 6종의 L11이 이 절을 폐기 문구의 **유일한 보존 위치**로 인용하는데, 절 번호는 문서 편집으로 움직이고 번호가 움직이면 6개 인용주가 동시에 고아가 된다(그 위험이 q-4가 "절 번호 고정을 불변식으로 승격할까"를 물은 이유다). 제목 문자열을 앵커로 쓰면 불변식 승격(=CLAUDE.md 편집) 없이 같은 보호를 얻는다. 따라서 **이 절의 제목 문자열은 캐논 앵커**이며 바꾸려면 RFC가 필요하다. 절 번호는 편의 표기일 뿐이다. 같은 규칙을 §4-2(「폐기된 장치·전제」)에도 적용한다.
**오해 주의**: 앞선 판의 §4는 위 "정본 문구" 열을 *기각된 재작성문*으로 기록했다. RFC-P3-010·014로 그 기각 판정은 무효가 됐다. 기각된 것은 **C2 문구** 쪽이다.

### 4-2 폐기된 장치·전제 (세션 Q의 C3 추가분 중 P 본문과 충돌한 것)

| 폐기 문구(원문 보존) | 폐기 사유 |
|---|---|
| "**모의 재생** 회로 — 판독침을 각인층에 대지 않고 염선의 잔류 반향만 읽어 곡선의 띠를 그린다. 결정은 닳지 않는다. 대신 4분 눈금이 서지 않아 값의 범위만 보이고 확정 눈금은 없다" | 정본 법2가 재생 횟수를 제한하지 않으므로 무비용 예비 모드가 불필요하며, 그 존재가 일반 판독에 비용이 있다는 오해를 만든다. live 사용 0건 |
| "**대기 명령**(pending) — 밸브·수문 명령은 거는 순간 집행되지 않는다 … 지우고 다시 거는 횟수 제한 없음" | 법4의 연습은 편성기 가상 완주로 실현된다. live 사용 0건이며 "밸브 명령 대기 흔적" 회귀 문구를 부른다 |
| "봉인이 붙는 그 순간에만 각인층이 열리고(법2 재생 1회 차감) … 부식이 본관에 기록된다(법5 한도 차감)" | RFC-P3-009: 확정은 부식을 차감하지 않는다. 차감 개념 자체가 없다 |
| "법2·법5 예산의 **일차 회복은 없다**" | 소모가 없으므로 회복·리셋 개념이 성립하지 않는다 |
| "P2 불파괴 매체 예시 — (a) 청문 접수부의 확정 사본, (b) 주민회 사본, (c) 염판 표면 부식" | 사본은 루트 출처를 물려받아 원본과 쌍을 이루지 못한다(C3-F12). 정정된 예시는 bible 3-bis.3 |
| "§4 인물표: 서린 '자기 이름이 적힌 염판 #0' / 재화 '특정 계통의 재생 예산을 회수' / 도연 '특정 판을 지목해 재생을 유도'" | 인물 캐논은 세션 P 본문. 판 #0은 "번호만 적힌" 판이며 재화의 거래는 출처 표기 요구다 |
| 연표 §2 "H-1:20 현장이 서명 확인 전에 밸브를 돌림" / "H+0:10 침수" / §8 "봉인 호출과 밸브 집행이 같은 염판 1매에 연속 각인" | RFC-P3-013. 기계가 기록할 수 없는 '확인'에 결론을 걸었고, 시각이 4분 격자에 얹히지 않으며, 순서 앵커의 물리 근거가 틀렸다 |

## 5. 결함·RFC 처리 결과

| 결함 | 소유 | 세계관 레인 처리 | 잔여 |
|---|---|---|---|
| **C3-F3** (기각된 법 문구 유통) | planner → **systems** | §4-1을 "사용 금지 문구 목록"으로 재작성하고 정본을 bible §3 하나로 고정. 3차에서 §4-1을 **해소 이력**으로 문맥 전환(문구는 원장으로 보존) | **해소(2026-09-10, closed).** systems가 6곳(스펙 5행 + `interaction-rules.md` L101)을 정본으로 교체 → **A33 pass**. 세계관 재측정 `systems` 0파일/0행 · 호명 6/6 문자 일치. **RFC-W3 소멸**(§6) |
| **C3-F34** (감사가 이미 닫힌 위반을 현행 `[OBSERVED]`로 유지) | worldview | **해소.** §1 집계를 `pass 37 / violation 0 / open 4`(합 41)로 재계산, §2 A33 → pass, §3에 「법 문구 유통」 3행을 3차 실측으로 교체하고 회차 표기 규칙 신설, §4-1 표제를 "현재 위반 위치" → "해소 이력"으로 전환, 본 표에 닫힘 기록 | 없음. **재발 방지**: §3 회차 표기 규칙 + §6 자기 인계에 "타 레인이 닫은 결함의 감사 행 재측정" 추가 |
| **C3-F5** (live 시각이 캐논을 뒤집음) | planner/synopsis | timeline §2를 RFC-P3-013으로 재작성(H-1:24 → H-1:04, H+0:12). §4·§5·§8의 파생 문구 정정, 회귀 금지 문구 0건 | `synopsis/chapter-beats.md` B17 문구 교체(synopsis) |
| **C3-F8** (포크·고아) | worldview/director | bible·timeline을 아카이브 c3 본문으로 재기반하고 `supersedes`를 c3로 재연결 [OBSERVED 확인] | `freshness-check.sh`의 고아 검사 추가(director 요청 유지) |
| **C3-F9** (차감 시점 2규정) | worldview | **해소.** RFC-P3-009로 도구 확정은 부식을 소모하지 않는다 → T0에 `seal`이 없는 것과 비용 발생은 무관해진다. bible 3-bis.2의 차감 문구 삭제 | `balance/balance-sheet.md` §4·§4.3, `chapter-beats.md` 표 A의 소모 열(balance·synopsis) |
| **C3-F12** (불파괴 쌍 독립성) | synopsis/systems | bible 3-bis.3 P2·P3을 "루트가 서로 다를 것 / 사본은 원본과 짝이 되지 못함"으로 정정하고 예시를 다른 기관 서류철로 교체 | `continuity.md` §5 K1~K10 재작성(synopsis), §3 예외 명문화 여부(systems) |
| **C3-F13** (용어집 미수록) | worldview | **해소.** F13 지목 14건 + grep으로 찾은 20건 승격, `originId` 31/31 수록, 폐기 용어 §8 신설. 기계 검사 `missing=0` | 시놉시스 초고 확정 후 전수 추출 재검사 |
| **C3-F24** (덱 슬라이드 8) | presentation | 재측정 결과 `generate-deck.mjs:549` "이번 조수에 보호 용량이 부족하다"는 **정본 문구**다 [OBSERVED] | 조사 결과 수정 불필요. RFC-W3이 소멸했으므로 동봉 대상 없음 — 이 행이 "덱은 정본을 쓰고 있다"의 기록으로 남는다 |
| **C3-F28** (집계 영수증이 폐기된 판을 가리킴) | worldview | **해소.** `glossary.md` 머리글·§7 머리글과 본 문서 §3·A40의 해시·크기·단서 수·`plate` 분포를 `fdabf1d4…`·120479 B / 73 / 24로 재측정 교체하고 A41로 기록 | **R4에서 실제로 다시 돌렸다** — 입력이 `92301c0a…`·121457 B로 바뀌어 §3 전 행·`glossary.md` §7 머리글·`timeline.md` §7·`worldview-bible.md` §7의 해시를 4차 값으로 교체(§6 자기 인계 이행) |
| **RFC-W4 / A22** (B18 의도 vs 효력) | director → worldview·synopsis·planner | **해소.** timeline §9 OPEN-4를 판정문으로 닫고 §3에 「RFC-W4 일치 확인」을 추가, §7 B18 금지 열을 "의도는 B27로 이월"로 명시. A22 → pass | 없음(세계관). synopsis는 B18/B27 대사 상한을 이 판정으로 저작한다 |
| **C3-F25** (RFC-P3-013 문구 오기) | director → worldview | **반영.** timeline §2 H-1:40 행에 「캐논 유지 · C3-F25 정정」 주석, 폐기 목록을 "H-1:20·H+0:10 두 시각뿐"으로 재작성하고 데이터 재측정(H-1:40 3건)을 첨부 | 없음. 검증기 `K-04`가 두 시각만 검사하는 현행 구현이 옳다 |
| **C3-F30 / A42** (B# 체계 두 벌) | director → worldview(정의 소유) · synopsis(파생) | **정의 고정.** timeline **§7-0**에 한 줄 정의를 신설하고 표 33행을 명령으로 재도출(불일치 0). 「두 번째 부여 체계는 없다」를 명시 | **open** — `chapter-beats.md` 표 A 14행 · `continuity.md` §5.1 3행(+K5 본문 1건)과 `chapter-beats.md` §0의 "고정 라벨"·"두 체계" 주석이 남았다(synopsis) |
| **C3-F22 / A43 · 모델러 OPEN-M1** (비트 `zoneId` · zone 토큰) | planner(데이터) → worldview(명사) → modeling | **해소.** glossary **§6-1** 신설로 `hub·gate·pump·dock·lowland` 5값에 KO/EN 정본을 연결하고, 오브젝트명 토큰은 zoneId 영문을 그대로 써도 된다고 판정 → `Hub` 유효, **OPEN-M1 닫힘** | 모델러가 `pipeline.md` §4·`asset-manifest.md`의 OPEN-M1 문구를 "닫힘(worldview 판정, glossary §6-1)"으로 갱신하고 `Quay`↔`dock` 대응을 표에 적는다 |
| ~~**RFC-W3**~~ (법 문구 적용 방향) | — (**소멸 확정**) | **디렉터가 소멸을 확정했다** — `production/decision-log.md` 「RFC-W3 · 소멸 확인」(2026-09-10): 교체 대상이 0건이 되어 판정할 것이 남지 않았고, §4-1은 「사용 금지 문구 원장」으로 유지하며 systems 스펙은 **제목 문자열로 인용**한다(위 §4-1 인용 방식 규칙) | 없음. 제기 내용은 §6에 취소선으로 보존한다 |

## 6. 다음 사이클 인계

| 항목 | 받는 레인 | 내용 |
|---|---|---|
| ~~RFC-W3 (법 문구 적용 방향)~~ | — (**소멸, 2026-09-10**) | 제기 내용: "RFC-P3-014의 교체 대상은 planner·덱이 아니라 `systems/system-specs/*`다". **systems가 이미 6곳을 교체해 판정 요청 대상이 남지 않았다** [OBSERVED, §3 3차]. 삭제하지 않고 기록으로 보존 — 다음 사이클이 같은 판정을 다시 요청하지 않게 하기 위함 |
| ~~RFC-W4 (B18 의도 vs 효력)~~ | — (**판정 완료, 2026-09-10**) | 디렉터 판정: 4장 = 효력, 6장(B27=`c6-b4`) = 의도. timeline §9 OPEN-4 해소 · A22 pass · 검증기 K-06 PASS. synopsis는 판정된 상한 안에서 B18/B27 대사를 저작한다 |
| ~~K1~K10 재작성~~ | — (**완료, 2026-09-10**) | A37 / C3-F12 해소. `continuity.md` §5.1이 루트·종류 상이 쌍으로 재작성됨(파일 재독 + `C-07`·`--pairs` 확인) |
| **B# 파생 재도출 (A42)** | **synopsis** | `chapter-beats.md` 표 A **14행** · `continuity.md` §5.1 **3행**(+K5 본문 1건)의 B#를 `timeline.md` **§7-0 정의**로 재도출하고, `chapter-beats.md` §0의 "고정 라벨"·"두 B# 체계" 주석을 폐기 문구로 교체한다. 인용 키는 campaign id. 재실행 명령은 본 문서 §3 「B# 색인 파생」 행 |
| **OPEN-M1 닫힘 반영** | **modeling** | glossary §6-1 판정(오브젝트명 토큰 = zoneId 영문 허용, `Hub` 유효, `Quay`↔`dock`)을 `pipeline.md` §4·`asset-manifest.md`에 반영 |
| **`--pairs` 인용 명시** | synopsis | `continuity.md` §5.1에 파생 출처(`validate-campaign.mjs --pairs`, C-07)를 한 줄로 인용하면 다음 데이터 변경 때 표가 스테일인지 명령 하나로 판별된다(비차단 권고) |
| 부식 모델 반영 | balance, economy, systems | A34 / RFC-P3-009. `balance-sheet.md` §4 전면 재작성 |
| 에필로그 상태 조합 축약 | planner, synopsis | A25. E0 10분·2비트 상한 안에서 |
| EN 표기 상표 조사 | product-manager | A29 |
| 고아 아카이브 검사 | director | `freshness-check.sh` 범위 확장 |
| B01~B33 저작 | synopsis | timeline §7을 **상한**으로 사용. 상한 초과 대사는 G1 violation |
| 타 레인이 닫은 결함의 감사 행 재측정 | worldview(자기 인계, **C3-F34 재발 방지**) | 다른 레인이 결함을 `closed`로 보고하면 그 결함이 근거였던 **§1 집계 · §2 verdict · §3 기계 검사 행 · §4 문맥**을 같은 회차에 전부 다시 돌린다. 감사 문서가 자기 레인에 **유리한 방향으로 스테일한 경우도** 결함이다(C3-F34는 불리한 방향이었다). §3의 행별 회차 표기를 유지한다 |
| 집계 영수증 재실행 | worldview(자기 인계) | `campaign.json` 이 바뀌면 `shasum -a 256` + `node planning/validate-campaign.mjs` 를 다시 돌려 `glossary.md` §7 머리글 · 본 문서 §3 · A40 · `timeline.md` §7 · `worldview-bible.md` §7 의 해시·집계를 같은 회차에 교체한다(C3-F28 재발 방지) |

## 7. T0 인용 출처 저작 보충 감사 (RFC-CX-001)

**[TARGET · 저작 결정 · draft / 교차 레인 ACK 대기]** 디렉터가 RFC-CX-001 선택 A를 채택한 범위의 감사 입력이다(`handoff/rfc-inbox/RFC-CX-001.md`, `production/decision-log.md`). 기존 자연어 정본과 새 기술 ID·관계 저작을 구분한다. 이 절은 §2의 기존 41항목 재검증 집계에 합산하지 않으며 런타임 G1 PASS를 주장하지 않는다. 최종 승격은 필수 레인 ACK·QA와 디렉터의 판정 후에만 가능하다.

| artifact | claim | verdict | fix |
|---|---|---|---|
| `worldview/glossary.md` §6-2 · `synopsis/t0-records.md` §5.1 | 기존 “허브 계통”에 기술 ID `system-hub`를 지정한다. 기존 표현·계통 상태·사건·수치는 유지한다 | design-supported / ACK 대기 | 기술 ID 문자열은 기존 관측값이 아닌 RFC-CX-001 저작 결정으로 표시 |
| `worldview/glossary.md` §6-2 · `synopsis/t0-records.md` §7 | 기존 “기록국 표준 관측소”에 `station-bureau-standard`를 지정한다. 새 장소를 만들지 않는다 | design-supported / ACK 대기 | 기존 개념의 근거는 §7 머리말·`tideHeight` 행과 용어집 §7; ID 지정의 근거는 RFC-CX-001로 분리 |
| `worldview/glossary.md` §6-2 · `rec-plate-standard-hub` | `systemId=system-hub`, `stationId=station-bureau-standard`. 관측소 연결은 표준 조위를 비교 기준으로 채택한 새 저작 결정이다 | authored-reference / ACK 대기 | 관측 생산지·새 센서로 해석 금지. 압력·염도·내측 수위와 조위의 다른 축 유지. synopsis의 명시 귀속표와 대조 후 generator 반영 |
| `worldview/glossary.md` §6-2 · `rec-tide-ledger-bureau` · `systems/data-schemas/plates.md` §1 | 기록국 표준 관측소 기준이며 `systemId=null`은 염판 계통 비해당이다. schema는 `sourceType==plate`에만 계통을 필수로 한다 | design-supported / ACK 대기 | 레저까지 계통 nonnull을 요구하거나 `system-hub`를 채우지 않음. 인용의 계통 비해당과 기준 관측소를 보존 |
| `rec-handover-brief` · `rec-transfer-list` · `rec-watchlog-bureau` | 나머지 3개 레코드의 관측소 귀속은 확정하지 않는다. 목록 속 조위대장·회선 3개소의 설명은 해당 레코드의 관측소를 증명하지 않는다 | unresolved / 범위 밖 유지 | `zoneId`·파일명·동일 수치에서 station을 추정하지 않음. 해당 자료에 귀속이 필요한 기능은 별도 저작 근거 필요 |
| `handoff/codex-unity-brief.md` §⑤-3 · `worldview-bible.md` §2 | 실제 T0 인용 2건은 기존 plate/ledger 및 각 `rootOriginId`를 유지한다. 공유 기준 관측소가 독립 매체 판정을 대체하지 않는다 | design-supported / ACK 대기 | 독립성·관측값·오차폭·사건시각·T0 공개 상한을 변경하지 않고 기존 규칙으로 검증 |

[OBSERVED · 문서 확인] 근거 파일에 허브 계통과 기록국 표준 관측소의 자연어 표현이 존재한다. [TARGET · 이번 저작] 기술 ID 2개와 표준판의 비교 기준 연결을 명시한다. 게임 화면의 인용 동작·실측·최종 G1은 이 감사에서 검증하지 않았다.
