---
updated: 2026-09-11
cycle: 20260909-preproduction-c6
status: current
supersedes: null
owner: game-planner
---

# 게임 초안 v1 — 조수기록국: 마지막 당직 (가제 · 상표 미확인)

## 0. 한 줄 피치와 정직성 선언

**폐국을 하루 앞둔 조수기록국의 마지막 야간 당직 8시간 동안, 기록복원사 한서린은 12년 전 대조의 밤에서 사라진 결손 4시간을 서로 다른 매체 두 종의 대조만으로 복원한다 — 복원이 끝나는 자리에 어머니의 서명이 있다는 것을 알면서.** (출처: `_workspace/current/synopsis/synopsis.md` §1)

**상품 약속 — 정본 문구** [디렉터 판정 **C6-F2**, 2026-09-10]:

> **무엇을 근거로 결론에 이르는지가 손으로 바뀌고, 마지막에 세 관점 중 하나를 제출한다.**

이전 문구 "내가 직접 손을 대서 **결론이 바뀌는** 추리"는 **철회**한다. 캠페인은 **단일 임계경로 + 제출 관점 3갈래**이며 씬은 갈라지지 않는다 — 이것은 결함이 아니라 설계 의도다(`c7-b4.consequence` "세 선택은 같은 종결 씬으로 들어가고 관점별 기록 패널만 달라진다"). 따라서 결말 3갈래는 "선택"이 아니라 **제출 관점**으로 부르고, `product/business-model.md` §1 · 루트 README · 본 문서는 위 한 문장으로 통일한다. **플레이어의 손이 바꾸는 것은 결론이 아니라 근거와 기록**이다.

이 문서가 무엇인가 / 무엇이 아닌가:

| 항목 | 선언 |
|---|---|
| 지위 | C6 회차의 **통합 서술본**. 한 문서로 게임을 설명·판단하기 위한 편집물이다 |
| `gdd.md`와의 관계 | **대체하지 않는다.** `planning/gdd.md`는 "지금 무엇인가"의 **색인**이고 본 문서는 그 색인이 가리키는 것들의 **통합 서술**이다. 두 문서가 어긋나면 소유 레인 문서 → `gdd.md` → 본 문서 순으로 고친다 |
| 숫자 | 본 문서는 숫자를 **재기재하지 않는다**. 아래 모든 표에 **출처 열**이 있고, 값이 바뀌면 출처가 먼저 바뀐다 |
| 측정 | **실제 사람 플레이·성능·판매·지불의사 표본은 전부 n = 0이다.** 이 문서의 어떤 표도 G2/G4/G5/G6/G7을 올리지 못한다 |
| 프리비즈 | 생성된 2D·3D·영상은 전부 **컨셉/프리비즈이며 게임플레이가 아니다**(`production/premium-preproduction-contract.md` Honesty gates) |
| 가제 | "조수기록국: 마지막 당직" / "TIDE ARCHIVE"는 **상표·동명 확인 전이며 미승인**이다. 폴더명·번들명·상점명에 쓰지 않는다(계약 Unity/저장소 배치) |
| 빌드 | Unity 코드 0줄. 본 문서는 **문서 게이트(D)** 축에서만 판단 대상이다 |

표기: `[OBSERVED]` 파일·명령 출력에서 직접 확인 · `[INFERENCE]` 문서 대조로 도출 · `[TARGET]` 설계 목표 · `[CARRIED]` 이전 회차 승계.

### 0.1 C6/C7 수정 루프 1에서 바뀐 것 (2026-09-10 · 같은 사이클 제자리 개정, RFC-Q2 → `supersedes: null` 유지)

| 절 | 무엇을 | 근거 |
|---|---|---|
| §4.1 층 B · §4.1 「순서 주의」 · §4.3 | `two-step` 순서 서술을 정본 문구(「프리뷰 → 초점을 확정 버튼으로 옮겨 한 번 누름. 홀드 없음」)로 **문자 일치**시키고, 옛 서술이 실제로 가리키던 `confirm-dialog`와 한 행으로 분리 | **C6-F14** · `systems/interaction-rules.md` §1-1 |
| §2.5 신설 · §3.1 C3·C6 행 · 표 읽는 법 | 대조의 밤 **캐논 시각·순서 앵커** 절을 신설(시각 값은 재기재하지 않고 `worldview/timeline.md` §2·§8을 출처 열로 인용)하고, 스테이지 표 C3·C6 행에 **B14 기준 / B26 회수** 앵커를 병기 | **C6-F15** · `worldview/timeline.md` §8 |
| §2.3 표 머리 | 결말 3갈래 표의 **머리행이 4열, 본문행이 5열**이라 렌더가 어긋나 있었다 — 머리에 「무엇이 기록에 남는가」 열을 채워 5열로 맞춤. 값·주장 변경 없음. `[INFERENCE]` 표기와 출처 명시(C6-F22)는 **아직 열려 있다** | 본 루프 중 발견 [OBSERVED] · C6-F22 미해소 |

### 0.2 C6/C7 수정 루프 2 (R7)에서 바뀐 것 (2026-09-10 · 같은 사이클 제자리 개정, RFC-Q2 → `supersedes: null` 유지)

| 절 | 무엇을 | 근거 |
|---|---|---|
| §0 신설 문단 · §2.3 제목·표 머리·주석 | **상품 약속 정본 문구** 등재("무엇을 근거로 결론에 이르는지가 손으로 바뀌고, 마지막에 세 관점 중 하나를 제출한다")와 "결말 3갈래" → **"제출 관점 3갈래"** 개명. "분기·멀티 엔딩" 표기 금지 | **C6-F2** 디렉터 판정 |
| §2.6 신설 | **T0 공개 상한 · 캐논 시각 · 순서 앵커** 요약 표(허용/금지 2열) | **C6-F16** · RFC-P3-012 |
| §3.1 C2·C6 행 · §3.2 구역 집계 · §3 검증기 실행 문단 | `zoneIds` 확장과 구역 파생 집계 재도출(허브 15/188 → **17/218**, 런 13 → **14**), 검사 47 → **49** | **C6-F17** · C6-F3 |
| §4.2 | **도구 표시명 정본 한 벌** 확정 문장(옛 OPEN 블록 대체)과 **T0 KO 전용**(EN은 선택, 용어집 등재만 fail-closed) 주석 | **RFC-S6 / RFC-C6-001 / C4-F9** · **C7-F5** |
| §6 머리 · §6.1 신설 · §6.2 표 2행 | 본 생산 승인 조건을 계약 **「Base production gate」 절 인용만**으로 바꾸고, **T0 확정 = `reader` 인용 고정** + T0 세 비트 완료 술어 표 추가 | **C6-F9** · **C6-F10** · RFC-C7-001 |
| §11.1 G1 행 · §11.2 1·2행 · §11.3 신설 | 도구 표시명·비트 구역 행을 판정 결과로 갱신하고 **위험 R-T0-1(조작 밀도)** 행 신설 | **C6-F5** · C6-F17 · RFC-C6-001 |
| Appendix A | 검증기 결과 47/47 → **49/49** | 재실행 [OBSERVED] |

**이 개정이 하지 않은 것**: 분·비트 수·도구 배분·가격·견적 숫자는 하나도 바꾸지 않았다. 사람 플레이 표본은 여전히 **n = 0**이다.

---

## 1. 세 기둥 [CARRIED]

| # | 기둥 | 이 기둥이 금지하는 것 | 출처 |
|---|---|---|---|
| 1 | **행동으로 추리한다** — 단서 수집으로 끝내지 않고 배선·경로·기록의 가설을 손으로 시험한다 | 읽기만 하면 진행되는 문서 열람 게임 | `planning/gdd.md` §2 |
| 2 | **같은 장소가 다르게 읽힌다** — 새 지역 대신 허브 1 + 구역 4를 상태 변화로 재사용한다 | 맵 증설로 분량을 만드는 것 | `planning/gdd.md` §2, `planning/content-matrix.md` §1 |
| 3 | **본편만으로 닫힌다** — 도시의 위기·사건의 책임·주인공의 선택이 본편에서 끝난다 | 결말을 DLC로 파는 것 | `planning/gdd.md` §2, `worldview/worldview-bible.md` §6 |

장르 경계 [OBSERVED]: **전투 없음 · 유료 재화 없음 · 멀티플레이 없음.** 따라서 승률·TTK·일간 인플레이션 게이트는 이유 있는 N/A이며 대체 검사는 **퍼즐 도달성 · 진행 막힘 · 본편 완결성 · DLC 비의존성**이다(`planning/gdd.md` §1, `economy/currency-map.md` §1).

---

## 2. 세계

### 2.1 불가침 6법 — 정본 문구 (인용, 재작성 금지)

**정본은 `worldview/worldview-bible.md` §3 표 하나뿐이다**(RFC-P3-014). 아래는 그 표의 호명 문구를 그대로 옮긴 것이며 폐기 문구(`worldview/consistency-audit.md` §4)는 어디에도 쓰지 않는다.

| 법 | 호명 문구 (정본) | 플레이어 행동 | 실패와 회복 | 출처 |
|---|---|---|---|---|
| 1 | **배선된 것만 남는다** | 회로 지도에서 센서 범위 대조 | 근거 부족 사유 표시, 판정 보류. 권한 박탈 없음 | `worldview/worldview-bible.md` §3 |
| 2 | **원본은 닳지만 사본은 남는다** | 원본 상태 확인 → 사본 판독 무제한 | 오조작은 무료 초기화. 필수 단서 삭제·재생 횟수 제한 없음 | 〃 |
| 3 | **정합 전 시계는 믿지 않는다** | 공통 피크 3개로 기준선 맞추기 | 오차띠가 겹치면 확정 비활성 + 이유, 재정합 가능 | 〃 |
| 4 | **이번 조수에는 보호 용량이 부족하다** | 두 경로를 연습한 뒤 결과 요약과 함께 확정 | 증거는 이미 복제. 물자·후일담만 달라진다 | 〃 |
| 5 | **소금은 비용으로 보인다** | 가상 시험 → 한도 내 구성 → 확정 | 초과 구성은 확정 전에 차단. 무료 우회관으로 언제나 복구 | 〃 |
| 6 | **원본 책임과 제출을 나눈다** | 매체 대조 검증 → 제출할 관점 선택 | NPC가 거부해도 대체 검증 절차가 열린다 | 〃 |

무대 [CARRIED]: 가상의 대조차 항구 **은포항**, 폐국 전날 밤 **21:00 ~ 05:00 단일 야간**. 기록 매체는 염판(소금 각인) · 당직일지(손글씨) · 조위대장(수치) 3종. **목소리·영상·사람 위치·의도는 복원되지 않는다** — 이 제한은 캐논이며 레트콘 금지다(`worldview/worldview-bible.md` §2, `synopsis/synopsis.md` §2).

### 2.2 인물 5인 (요약 — 정의는 바이블 §4)

| 인물 | 역할 | 원하는 것 ↔ 두려운 것 | 모순 | 출처 |
|---|---|---|---|---|
| **한서린** | 주인공, 기록복원사 | 결손 4시간 복원 ↔ 어머니의 과실 확정 | 무결성을 신봉하면서 미봉인 판 #0을 숨긴다 | `worldview/worldview-bible.md` §4 |
| **문재화** | 항만공사 수문 계장 | 무사고 자동화 이관 ↔ 자기 지시가 문서로 남는 것 | 안전을 위해 기록을 "정리"한다 | 〃 |
| **오은정** | 저지대 주민회 총무 | 공식 인정·보상 ↔ 천재(天災) 결론 | 진실을 요구하며 유리한 사본만 준다 | 〃 |
| **표성찬** | 냉동창고 운영주 | 하역권·지분 유지 ↔ 그날의 통화 기록 | 가장 먼저 협조하지만 그 자료가 시간축을 방해한다 | 〃 |
| **한도연** | 서린의 어머니, 당시 당직 주임 | 딸이 그만두는 것 ↔ 기억과 기록의 불일치 | 그만두라면서 열쇠와 좌표를 흘린다 | 〃 |

**아무도 전체를 모른다.** 전지적 화자·해설 NPC는 금지다. 도연은 편리한 증인이 아니며 혼자서 사실을 확정하지 못한다(`synopsis/synopsis.md` §7, `worldview/worldview-bible.md` §4).

### 2.3 제출 관점 3갈래 — 본편 안에서 닫힌다 [C6-F2 판정 반영]

| 관점 | 제출 내용 | 무엇이 기록에 남는가 | 무엇을 잃는가 | 출처 |
|---|---|---|---|---|
| A | **완전 복원 제출** | 도연의 대필과 기관 책임이 함께 기록된다 | 개인의 이름이 공식 기록에 남는다 | `worldview/worldview-bible.md` §6 |
| B | **계통 결함 중심 제출** | 보상 성립, 개인 책임 희석 | 책임 소재가 흐려져 같은 구조가 남는다 | 〃 |
| C | **불완전 인정 제출** | 공백을 공백으로 남기고 기록국 존치 근거를 얻는다 | 이번 세대의 보상이 지연된다 | 〃 |

**용어 규칙(C6-F2)**: 위 A·B·C는 서로 다른 **제출 관점**이지 서로 다른 결말 씬이 아니다 — 종결 씬은 하나이고 관점별 기록 패널과 후일담 문단만 달라진다. 문서·상점 문구에서 "분기"·"멀티 엔딩"이라는 말을 쓰지 않는다.

세 관점 모두 에필로그에서 **청문 결과 · 4구역 최종 상태 · 서린의 다음 근무 · 판 #0의 처리**를 명시한다. 5장의 법4 선택이 무엇을 잃든 **세 갈래 전부 7장에서 선택 가능**하다(바이블 §6 주석 · 3-bis.3 P5). 미해결 떡밥을 DLC 판매 근거로 남기지 않는다.

### 2.4 DLC는 독립 사건이다

이관 2년 후, 자동 기록되는 항구에서 주민들이 스스로 **기록 정지**를 청원한다. **새 주인공(파견 감사관) · 새 구역 · 새 딜레마**이며 본편의 어느 결말에서 시작해도 성립한다. 본편 결말을 바꾸거나 완성하지 않는다(`worldview/worldview-bible.md` §6, `product/business-model.md` §7).

---

### 2.5 대조의 밤 — 캐논 시각과 순서 앵커 [CARRIED · 정본은 worldview]

12년 전 **대조의 밤**의 저자 진실 연표가 이 게임의 **정답 판정 기준**이다. 본 문서는 §0 「숫자」 선언에 따라 **시각 값을 재기재하지 않는다** — 캐논 시각과 간격·오차폭의 값은 `worldview/timeline.md` §2 표가, 순서 앵커의 논리는 같은 문서 §8이 소유한다. 아래는 그 두 절이 무엇을 확정하는지에 대한 서술이다.

| 축 | 이 초안이 전달하는 것 | 값의 소유자 (출처) |
|---|---|---|
| 캐논 시각 4개 (만조 H 기준) | ① 도연이 두 번째 서명란에 **서린의 이름**을 적음 → ② 현장이 **밸브를 돌림**(밸브 개폐 각인) → ③ **봉인대 압착 완료**(봉인 완료 접점 각인) → ④ **저지대 침수 시작**. 네 시각 전부 4분 격자 위에 있다 | `worldview/timeline.md` §2 표 (RFC-P3-013 · C3-F25 적용) |
| 폐기된 시각 | 폐기는 **밸브·침수 두 시각의 옛 값 2개뿐**이며 ①의 서명 기입 시각은 **캐논 유지**다(RFC-P3-013 본문의 반대 서술은 C3-F25로 정정됨) | 〃 · 검증기 `K-03`(캐논 시각 존재) · `K-04`(폐기 시각 부재) 2026-09-10 실행 **PASS** [OBSERVED] |
| 순서 앵커 | ②·③ 두 각인은 **같은 염선 계통 로그 · 같은 형식 · 같은 기록계**에 남는다. 정합 후 관측소별 잔차가 남아도 **두 각인의 간격 > 총 오차폭**이므로 ②→③의 선후가 확정된다 — 즉 **집행이 이중서명 완성보다 앞섰다**가 증명 가능한 명제가 된다 | `worldview/timeline.md` §8 (간격·오차폭 두 값 모두 그 표가 소유) |
| 왜 관측소 드리프트가 반론이 못 되는가 | 큰 드리프트는 **관측소 사이**의 문제이고, 이 명제는 같은 계통의 두 각인만 쓴다 | 〃 |
| 게임 안에서의 위치 | 씨앗 **B06**(`c1-b3`, 봉인대 연습) → 기준 **B14**(`c3-b3`, 반증 시험) → 회수 **B26**(`c6-b3`) | 〃 §8 「어디서 회수되나」 행 · 본 문서 §3.1·§3.2 |

**이 절이 없으면 무엇이 무너지는가** [OBSERVED, `worldview/timeline.md` §8 마지막 줄 그대로]: 이 앵커가 없으면 **3장(C3)과 6장(C6)의 정답 판정 기준이 정의되지 않는다.** 두 장의 분·비트·구역은 §3.1 표가 소유하며, 그 안의 퍼즐이 "무엇을 확정했는가"를 전부 이 앵커에 걸고 있다. 즉 §2.5는 배경 설정이 아니라 **2막 두 스테이지의 정답 판정 기준 그 자체**다.

**검증기가 증명하는 것과 증명하지 않는 것** [OBSERVED]: `K-03`·`K-04`는 **live `campaign.json`이 캐논 시각을 갖고 폐기 시각을 갖지 않는다**는 데이터 사실만 증명한다. 그 사실이 **읽는 사람에게 전달됐는지**는 검증기가 보지 않으며, 그 전달이 본 절의 일이다(C6-F15의 지적이 정확히 이것이었다).

**서명대 4조건과의 관계**: §4.2의 G-b(잔차 및 사건 간격 조건)는 이 앵커를 **플레이어의 제출 검사로 옮긴 것**이다. 앵커는 세계의 사실이고 G-b는 그 사실을 근거로 제출할 때 통과해야 하는 검사다 — 둘은 같은 규칙의 저자 측·플레이어 측 표현이다.

**복원되지 않는 것** [CARRIED, §2.1 무대 문단]: 이 앵커는 **각인의 선후**만 세운다. 밸브를 **누가** 돌렸는지, 그 사람의 **의도**가 무엇이었는지는 각인에 남지 않는다 — 그 의도의 확정은 6장(`c6-b4`)의 서사 판정이며 시간 앵커가 대신하지 않는다(RFC-W4).

### 2.6 T0 공개 상한 · 캐논 시각 · 순서 앵커 — 실행자용 요약 표 [C6-F16]

T0를 먼저 만드는 실행자(Codex)가 **허용과 금지를 이 초안 안에서** 알 수 있어야 한다. 값의 소유자는 여전히 worldview이며 본 표는 규칙만 옮긴다.

| 축 | T0(`t0-b1`~`t0-b3`)에서 **해도 되는 것** | T0에서 **하면 안 되는 것** | 정본 |
|---|---|---|---|
| 인물 이름 | 인수 각서가 12년 전 대조의 밤 당직 주임을 **한도연**으로 기재(공식 문서). 그가 주인공의 어머니임을 밝힌다 | **서명란의 이름 「서린」 노출** — `c4-b2`(B17)까지 미공개. 검증기 `K-05`가 기계로 막는다 | RFC-P3-012 · `worldview/timeline.md` §7 B01 |
| 판 #0 | 주인공이 구 서고에서 빼내 숨긴 **자기 소유물**로 등장. 이관 목록 등재 여부를 오늘 밤 결정한다 | 판 #0이 사건의 무엇을 증명하는지 말하는 것 | 〃 |
| 오늘 밤의 목표 | "이 당직이 끝나면 남는 것은 **청문에 낼 제출 문서 1건**"까지 (`t0-b1.objective`, C6-F1) | 그 문서가 어떤 결론을 담게 되는지 예고하는 것 | `campaign.json` `t0-b1` · `c7-b4` |
| 사건 인과 | **결손 4시간(H-1:00~H+3:00)이 존재한다**는 사실과 그 두 끝을 두 매체로 확정하는 것 | 12년 전 사건의 인과 일체. 오해 **"정전 때문이다"는 T0에서 유지**된다 | `worldview/timeline.md` §7 B01~B03 |
| 캐논 시각 | 시각 **값**은 T0 데이터가 이미 가진 것만 쓴다(`t0-b3` 결손 구간) | ①서명 기입 ②밸브 개폐 ③봉인 완료 접점 ④침수 시작 네 시각을 T0 화면에 노출하는 것. **네 시각의 값과 회수 지점의 소유자는 `worldview/timeline.md` §2·§7이며 본 초안은 값을 재기재하지 않는다**(§2.5) | §2.5 · `worldview/timeline.md` §2 |
| 순서 앵커 | 없음 — T0에는 `alignment`가 없다 | 두 각인의 선후(집행이 이중서명보다 앞섰다)를 T0에서 암시하는 것 | §2.5 · 같은 문서 §8 |

**표 읽는 법**: 왼쪽 열이 상한이고 오른쪽 열이 금지다. 금지 열의 위반은 G1 violation이며, 이름 노출 1건만 기계 검사(`K-05`)가 잡는다 — 나머지는 사람 검토(QA 서사 렌즈)가 판정한다 [OBSERVED].

---

## 3. 캠페인 — 9스테이지 33비트

집계·해시의 **유일한 재현 명령**은 다음 하나다(RFC-Q1: 고정 sha 숫자를 손으로 다시 적지 않는다):

```
node _workspace/current/planning/validate-campaign.mjs
```

[OBSERVED] 2026-09-10 R7 실행: `summary` = **checks 49 / pass 49 / fail 0 / verdict PASS**, 입력 = live `_workspace/current/planning/campaign.json`, `sha256`·`bytes`는 그 출력이 소유한다. R7에서 검사 2건이 늘었다 — `Z-03`(본문 ↔ `zoneId`, C6-F17) · `H-04`(1단 힌트 어휘, C6-F3). 두 검사는 **고치기 전 데이터에서 각각 4비트·24비트를 FAIL시켰다**(`planning/campaign.meta.md` §4.1 재현 블록).

### 3.1 9스테이지 표

| 스테이지 | 제목 | 분 | 비트 | 구역(`zoneIds`) | 이 장에서 학습·심화하는 법 | 이 장의 전환(반전) | 출처 |
|---|---|---:|---:|---|---|---|---|
| **T0** | 마지막 당직 인수 | 25 | 3 | `hub` | 법1 도입 · 법2 도입 | R1 씨앗: 부식 무늬가 눈에 들어온다 | 검증기 `aggregates` · `synopsis/chapter-beats.md` 표 A · `worldview/timeline.md` §7 |
| **C1** | 두 개의 필적 | 50 | 4 | `hub` `gate` | 법6 도입 · 법2 미안내 재문제 | R2 씨앗: 두 서명의 필압·잉크가 다르다 | 〃 |
| **C2** | 소금이 남긴 반증 | 55 | 4 | `gate` `pump` `hub` | 법5 도입 | **[R1 회수]** 부식 무늬 ≠ 정전 → 밸브는 사람 손으로 돌았다 | 〃 |
| **C3** | 다른 관측소 | 65 | 4 | `dock` | 법3 도입 | 기준 관측소가 다르다 → 오차띠가 겹치면 `unknown` · **순서 앵커 기준 = B14(`c3-b3`) 반증 시험**(§2.5) | 〃 |
| **C4** | 가려진 두 번째 이름 | 65 | 4 | `lowland` `hub` | 법5·법6 미안내 재문제 | **[R2 사실·효력 회수]** 두 번째 서명은 당시 미성년 서린의 이름이며 명령은 집행 요건을 갖추지 못한다 | 〃 · RFC-W4 |
| **C5** | 이번 조수의 용량 | 70 | 4 | `lowland` `dock` | 법4 도입(`routing`) | 한 구역을 잃는 확정 — 물자·후일담만 달라진다 | 〃 |
| **C6** | 1호기의 은폐 | 75 | 4 | `pump` `hub` | 법1·법3 미안내 재문제 | **[R3 회수]** 1호기 고장 은폐가 실제 동기, 창고 보호는 사후 설명. R2 의도 재해석 = 무효 서명은 거부 수단 · **순서 앵커 회수 = B26(`c6-b3`)**(§2.5) | 〃 · RFC-W4 |
| **C7** | 제출 | 65 | 4 | `hub` | 법4·법3·법6 종합 | 새 반전 없음 — 남은 문제는 해석과 손실 | 〃 · `synopsis/synopsis.md` §3 |
| **E0** | 에필로그: 인계 | 10 | 2 | `hub` | — | 청문 결과 · 4구역 상태 · 판 #0의 처리 | 〃 |
| **합계** | | **480** | **33** | 5구역 | 6법 × (도입 1 + 미안내 재문제 1) = 12건 | 3막: 130 / 275 / 75 | 검증기 `stageMinutes` `V-02` `V-03` |

**표 읽는 법** — 「분」·「비트」·「구역」은 검증기 출력 [OBSERVED]이고, 「학습·심화하는 법」은 live `toolTeaching`(도구 6종 × guided 1 + unguided 1 = 12건, `V-02`·`V-03` PASS)과 `synopsis/chapter-beats.md` 표 A 「학습·사용 법」 열에서, 「전환(반전)」은 `worldview/timeline.md` §7 R1·R2·R3 회수 행에서 **파생한 요약** [INFERENCE]이다. 두 열의 정의를 본 문서가 새로 만들지 않는다 — 어긋나면 소유 문서가 이긴다. C3·C6 행에 병기한 **순서 앵커**(B14 기준 / B26 회수)는 `worldview/timeline.md` §8 「어디서 회수되나」 행에서 파생한 것이며 근거는 §2.5다 [OBSERVED · C6-F15].

3막 묶음 [OBSERVED]: 1막 공백의 확인(T0·C1·C2, 130분·11비트) / 2막 시간과 이름(C3~C6, 275분·16비트) / 3막 제출(C7·E0, 75분·6비트) — `synopsis/synopsis.md` §3.

### 3.2 33비트 — 인용 키는 campaign id 다

문서 간 인용 키는 **campaign id 뿐**이다(C3-F30). B01~B33은 `worldview/timeline.md` §7이 소유하는 표시 색인이며 정의는 하나(스테이지 순서대로 `t0-b1`=B01 … `e0-b2`=B33).

| 스테이지 | 비트 id (순서대로) |
|---|---|
| T0 | `t0-b1` `t0-b2` `t0-b3` |
| C1 | `c1-b1` `c1-b2` `c1-b3` `c1-b4` |
| C2 | `c2-b1` `c2-b2` `c2-b3` `c2-b4` |
| C3 | `c3-b1` `c3-b2` `c3-b3` `c3-b4` |
| C4 | `c4-b1` `c4-b2` `c4-b3` `c4-b4` |
| C5 | `c5-b1` `c5-b2` `c5-b3` `c5-b4` |
| C6 | `c6-b1` `c6-b2` `c6-b3` `c6-b4` |
| C7 | `c7-b1` `c7-b2` `c7-b3` `c7-b4` |
| E0 | `e0-b1` `e0-b2` |

비트 구조 [OBSERVED, 검증기 `aggregates`]: 종류 = 탐색 `exploration` 2 · 퍼즐 `puzzle` 21 · 대화 `dialogue` 5 · 결과 `payoff` 5. 비트 분 범위 4~22분(중앙값 16분), 본편 퍼즐 비트는 10~22분에 모인다. **매체 2종 이상 요구가 33/33에서 성립**(`C-06` PASS). 힌트는 33비트 × 3단 = **99단**, 빈 문자열 0건(`H-01` PASS). 체크포인트 33/33. 도구 없는 5비트(`t0-b1` `c1-b4` `c3-b4` `c7-b1` `e0-b1`)는 전부 탐색·대화·정산이며 **퍼즐 21/21은 전건이 도구 ≥ 1개**를 가진다.

구역 배정의 단일 출처는 비트별 `zoneId` 필드다(C3-F22 해소, `Z-01`·`Z-02`·`Z-03` PASS). 배정 [OBSERVED, R7]: `hub` **17비트/218분(45.4%)** · `pump` 5/85 · `dock` 5/84 · `lowland` 4/67 · `gate` 2/26. 연속 체류 런 **14회**. 출처: 검증기 `zoneBeatCounts`·`zoneBeatMinutes`·`zoneRunSequence`, 파생 표는 `planning/content-matrix.md` §1·§3.

[INFERENCE] R7의 구역 정정(C6-F17)으로 허브 비중이 188 → 218분으로 올랐다. 새 콘텐츠가 아니라 **원래 당직실에서 벌어지던 두 비트(`c2-b4`·`c6-b4`)의 좌표를 데이터에서 바로잡은 것**이며, 그 결과 기둥 2(같은 장소가 다르게 읽힌다)의 부담이 커졌다 — 허브 상태를 몇 단계로 보여 줄지는 연출·모델링 레인의 열린 질문이다(`campaign.meta.md` §5.2 (6)).

---

## 4. 도구 — 여섯 동사

### 4.1 연습 / 확정 두 층 (세 번째 층은 만들지 않는다)

| 층 | 이름 | 무엇이 일어나는가 | 비용 | 세계 상태 | 출처 |
|---|---|---|---|---|---|
| A | **연습(sandbox)** | 구성·정합·판독·경로를 무제한 시도하고 결과를 미리 본다 | 0 (횟수·자원·시간 제한 없음) | 변하지 않는다 | `planning/gdd.md` §3.3 |
| B | **확정(commit)** | **두 단계 확인(`two-step`)** — 정본 문구 그대로 「프리뷰 → 초점을 확정 버튼으로 옮겨 한 번 누름. 홀드 없음」 | 체크포인트 1개 생성 | 변한다 | `systems/interaction-rules.md` §1-1(정본) · `planning/gdd.md` §3.3 |

원칙 4개 [TARGET]: ① 확정 전 프리뷰 100%(바뀌는 것·되돌릴 수 있는지·영향 구역·근거 2종을 문장으로) ② 확정 직전 자동 저장 100% ③ **필수 단서는 확정의 대상이 아니다**(첫 판독 시 자동 사본, 이후 전부 사본, 횟수 제한 없음) ④ 연습은 엔딩·평가·업적에 어떤 영향도 주지 않는다.

**순서 주의** [OBSERVED, `systems/interaction-rules.md` §1-1 · C6-F14]: `two-step`은 **프리뷰가 먼저**이고 확정 버튼 누름이 **마지막 1회**다. 반대 순서(확정한 뒤 확인을 한 번 더 받는 것)는 `two-step`이 아니라 별도 옵션 `confirm-dialog`이며 기본값이 아니다. 키 배정과 세 옵션의 구분은 §4.3.

### 4.2 여섯 동사와 확정 조건

| # | 동사 (기획 표시명) | 도구 id | 대응 법 | 확정 조건 (1문장) | 출처 |
|---|---|---|---|---|---|
| 1 | 배선 추적 | `circuit` | 법1 | **확정 없음(판독 전용)** — 다른 동사의 확정에서 배선 밖 근거를 무효로 만든다 | `planning/gdd.md` §4 |
| 2 | 판독 | `reader` | 법2 | 인용에 매체·계통·관측소 **출처가 채워졌을 때** | 〃 |
| 3 | 조위정합 | `alignment` | 법3 | **공통 피크 3개 + 잔차 절댓값 ≤ 4분** | 〃 |
| 4 | 배수 편성 | `routing` | 법4 | 규칙 위반 0 + **구성안 부식 비용 ≤ 9** + 프리뷰 확인 | 〃 |
| 5 | 부식 시험 | `corrosion` | 법5 | 구성안 총 부식 비용 **≤ 9**(초과는 확정 전 차단). 계통별 한도·누적 고갈·리셋 없음 | 〃, RFC-P3-009 |
| 6 | 이중서명 | `seal` | 법6 | **서로 다른 매체 2종 + 배선 범위 안 + 시간 근거 잔차 ≤ 4분** | 〃 |

서명대 4조건(불충족은 실패가 아니라 **표시된 보류**): G-a 독립 매체 2종 · G-b 잔차 ≤ 4분 및 사건 간격 > 총 오차폭 8분 · G-c 구성안 부식 ≤ 9 · G-d 증인 또는 대체 검증 절차 확보(`worldview/worldview-bible.md` §3-bis.2).

동사별 비트 분포 [OBSERVED, 검증기 `toolBeatCounts`]: `reader` 11 · `circuit` 10 · `alignment` 8 · `seal` 7 · `routing` 3 · `corrosion` 3. **`corrosion`·`routing`의 재등장 공백이 길다**는 불균형 1건은 열려 있고 기획 선호는 "현행 유지 + 재도입 리마인더"다(`planning/gdd.md` §4.2, RFC-P3-002).

**도구 표시명 정본 — 한 벌만 쓴다** [디렉터 판정 **RFC-S6 / RFC-C6-001 / C4-F9**, 2026-09-10]:

> 여섯 도구의 표시명 정본은 **`planning/gdd.md` §4 = `concept/style-guide.md` §9**이며 그 한 벌은 **배선 추적(Circuit Trace) / 판독(Plate Read) / 조위정합(Tide Alignment) / 배수 편성(Drain Routing) / 부식 시험(Corrosion Assay) / 이중서명(Dual Seal)** 이다. `systems/interaction-rules.md` §2가 쓰던 「회로 지도 / 판독기 / 경로 구성 / 부식예산」은 **장치·화면 이름**이지 도구 표시명이 아니다 — UI 문자열에는 위 여섯만 쓴다. worldview는 미등재 4건을 용어집에 등재한다(그 등재 완료가 C4-F9의 closed 조건이다).

**T0는 KO 전용이다** [디렉터 판정 **RFC-S6 / C7-F5**]: live `campaign.json`에는 EN 문자열이 **0건**이고, 그것은 결함이 아니라 범위 결정이다 — **EN 문자열은 로컬라이제이션 회차의 산출물**이며 T0 빌드는 한국어만 낸다. 따라서 브리프의 임포터 규칙 `I-9`·`T-12`는 "EN 필드는 **선택**(없으면 KO 폴백), **용어집 등재 여부만 fail-closed**"로 읽는다(systems 소유). 위 표의 영문 병기는 문서용 대조이지 런타임 문자열 계약이 아니다.

### 4.3 입력 3종

| 규칙 | 내용 | 출처 |
|---|---|---|
| 확정 기본값 | **두 단계 확인(`two-step`)** — 정본 문구 그대로 「프리뷰 → 초점을 확정 버튼으로 옮겨 한 번 누름. 홀드 없음」. 키 배정은 키보드 = 프리뷰 `Space` → 확정 버튼으로 초점 이동 후 `Enter`, 패드 = `X` → 초점 이동 후 `A`, 마우스 = 프리뷰 버튼 → 확정 버튼 클릭. 되돌릴 수 있는 조작은 전부 짧게 누름 한 번 | `systems/interaction-rules.md` §1-1 · §1 「프리뷰」·「확정」 행, RFC-P3-015 |
| `confirm-dialog`와의 차이 | `two-step`은 **프리뷰 → 확정** 순서이고 마지막 입력이 확정 버튼 1회다. `confirm-dialog`는 **확정 → 확인 대화 1회**로 순서가 반대인 **별도 옵션**(기본값 아님)이다. 따라서 「확정 버튼 → 프리뷰 패널의 확인」이라는 서술은 `confirm-dialog` 쪽을 가리키므로 `two-step`에 쓰지 않는다 | 〃 [C6-F14] |
| 길게 누름 | `hold`(0.4초)는 **opt-in 대체 방식**이며 기본값이 아니다. 켜도 `two-step` 경로는 남는다. 세 값(`two-step`·`hold`·`confirm-dialog`) 어디서도 진행 가능한 경로가 사라지지 않는다 | 〃 |
| 3종 1급 지원 | 마우스 · 키보드 · 컨트롤러. 전 항목 재매핑, 글리프 자동 전환 | 〃, `planning/gdd.md` §5 |
| 이산 대안 | 드래그(마우스) = 스틱+정밀 트리거(패드) = D-Pad 1스텝. **정합·편성·부식 3동사 전부에 이산 입력 대안이 있어야 한다** | `planning/gdd.md` §5 [TARGET] |

### 4.4 힌트 3단 — 무료·무제한·무페널티

| 단계 | 주는 것 | 주지 않는 것 | 출처 |
|---|---|---|---|
| 1 방향 | 지금 무엇을 결정해야 하는지, 어느 구역·도구가 관련 있는지 | 어떤 자료인지 | `planning/gdd.md` §6, `systems/interaction-rules.md` §4 |
| 2 절차 | 필요한 매체 2종의 **종류**와 도구 사용 순서 | 정답 값·정답 대상 | 〃 |
| 3 해답 | 정확한 자료와 조작 값, 그 판단이 성립하는 이유 | 이후 장의 진실 | 〃 |

무료 · 횟수 제한 0 · 업적/엔딩/평가 영향 0. **순서대로만** 열리고 3단은 사전 경고 후 표시한다. 무진전 자동 제안은 **180초 1회 + 180초 쿨다운**이며 단계 자동 승격은 없다(RFC-P3-015). 생각하는 시간을 실패로 세지 않는다.

### 4.5 저장 / 복구

자동 저장 = 장 경계 · 구역 이동 · **모든 확정 직전**. 수동 3슬롯 + 자동 1슬롯. 손상 세이브는 복구 패널을 열고 **손상 파일을 덮어쓰지 않는다**. 미등록 상위 스키마 세이브는 거부하고 파일을 그대로 둔다. **되돌림은 스펙상 상한 없음**(`model.mjs maxUndo: 32`는 프로토타입 한정 — RFC-P3-015 F23). 다만 **되돌림 무제한과 로그 크기 상한은 다른 축이다** — 명령 로그는 `entryCap` **20,000 엔트리** / `byteCap` **6 MiB**(≤ 8 MB 세이브 예산의 몫)를 넘으면 **체크포인트 입도로 접힌다(SV-F6)**; 접혀도 플레이어가 되돌릴 수 있는 지점은 사라지지 않고 입도만 굵어진다. 「50,000 엔트리 / 8 MB」는 `payload` 도입 전 값이라 폐기됐다. 출처: `systems/system-specs/save-undo.md` §6(SV-F6)·§9, `systems/data-schemas/save.md` §3.2 [C6-F12]. 저장 데이터 필드 개명은 마이그레이션 없이 금지(CLAUDE.md §9). 출처: `planning/gdd.md` §7, `systems/data-schemas/save.md`, `systems/system-specs/save-undo.md`.

### 4.6 접근성 — 본편 무료 계약 (요약 목록)

입력: 전 항목 재매핑 · 길게 누름 → 토글 · 연속값의 이산 대안. 표시: 텍스트 3단 이상·UI 배율 · **색 단독 금지(문자 라벨 병기)** · 색약 대체 팔레트(적록·청황) · 모션 축소. 소리: 전체 자막·화자 이름·효과음 문자 알림 · 3채널 볼륨. 진행: 무료 힌트 3단 · 무제한 되돌림 · **전역 타이머 0개**. 경로: 접근성 설정을 **플레이 시작 전** 타이틀/부팅 화면에서 도달. 언어: 한국어/영어.
출처: `planning/gdd.md` §8(판정 기준 열 포함), `systems/game-ui-contract.json` `accessibility`. **[OBSERVED] 실사용 검증 0건.** 이 목록은 DLC 뒤로 옮기지 않는다(`product/business-model.md` 기능 인질 금지 목록).

---

## 5. 자원 — 유일한 소모성은 하나뿐

| # | 자원 | 등급 | 상한 | 연습 소모 | 확정 소모 | 출처 |
|---|---|---|---:|---:|---|---|
| R1 | **부식 예산** | 소모성 C | **9**(전역, `routing` 구성안 총비용) | **0** | 7 또는 8 (구성 선택에 따라) | `economy/currency-map.md` §3·§4.1, RFC-P3-009 |
| R2 | **원본 상태** (재생 취급) | 마모 W | 판당 파괴적 절차 **3** [TARGET] | **0** | 1회/절차 | 〃 §4.2, `systems/data-schemas/{plates,save}.md` |
| R3 | 청문 신뢰도 | 파생 P (저장 안 함) | 필수 100% + 보강 0..N | 0 | 0 | 〃 §4.3 |
| R4 | 증인 확보 상태 | 플래그 F | 4인 × 4상태 | 0 | 0 | 〃 §4.4 |
| R5 | 열람 권한 | 단조 M | 구역 5 + 자료 목록 N | 0 | 0 | 〃 §4.5 |

세 가지 계약이 이 표를 지배한다.

1. **부식 상한 9는 구성안 단위의 전역 상한이다.** `circuit`/`reader`/`seal` 확정은 부식을 차감하지 않는다. 누적 고갈·계통 영구 고장·연습 소모·장 경계 리셋은 **이 세계에 없다.** 한도 초과 구성은 확정 전에 막히고 원인이 표시되며, 막다른 구성은 **무료 우회관(구성마다 1회)** 으로 복구된다. (RFC-P3-009, `worldview/worldview-bible.md` §3-bis.2)
2. **원본 상태 카운터는 3개 개념으로만 존재한다** — 누계 `readCounts`(`save.md`) · 상한 `readBudget`(`plates.md`, 기본 3) · UI 표기 **"원본 상태"**("예산"이라는 단어는 부식에만 쓴다). 사본 판독·재생·되돌림은 이 값을 건드리지 않으며 **상한 3에 닿아도 진행은 막히지 않는다** — 바뀌는 것은 에필로그 보존 등급 문장 1줄뿐이다. (`economy/currency-map.md` §4.2 옵션 B, `planning/gdd.md` §4.1)
3. **청문 신뢰도는 게이트가 아니라 보강이다.** 필수 결론 커버리지는 구조적으로 항상 100%이고 변동하는 것은 보강 근거 수뿐이다. 점수·등급·랭크·백분율 배지로 표시하지 않고 "보강 근거 n건"으로만 적는다. (`economy/currency-map.md` §4.3)

불변식 [OBSERVED]: **연습·프리뷰·되돌림은 어떤 자원도 소모하지 않는다.** 유료 통화 0 · 게임 내 상점 0 · 가챠 0 · 행동 에너지 0 · 수집품은 선택 메모이며 480분 필수 예산에 넣지 않는다(`economy/resources-and-fairness.md`).

---

## 6. 수직 슬라이스 T0 — 먼저 만드는 것은 이것 하나

| 항목 | 값 | 출처 |
|---|---|---|
| 공간 | **`hub` 1개** | `systems/unity-implementation.md` §10 |
| 도구 | **`circuit` + `reader` 2개** (배선 범위 술어는 스텁이 아니라 `circuit`이 실제 산출) | 〃 |
| 목표 길이 | **25분** | 〃, 검증기 `stageMinutes[0]` |
| 포함 비트 | `t0-b1` `t0-b2` `t0-b3` | live `campaign.json` |
| T0에 **없는 것** | `alignment` `routing` `corrosion` `seal` — 이 넷의 조작감은 T0로 검증되지 않는다 | `systems/unity-implementation.md` §10 |
| 별도 스파이크 | `alignment` 그레이박스: `c3-b2`·`c3-b3`만 떼어 독립 측정(추가 완성 에셋 만들지 않음) | 〃 |

**본 생산 승인 조건은 이 문서가 정의하지 않는다** [디렉터 판정 **C6-F9**, 2026-09-10]: 정본은 `production/premium-preproduction-contract.md`의 **「## Base production gate」 한 절뿐**이며 초안·브리프·`handoff/verification-plan.md`는 그 절을 **인용만** 한다. 그 절이 요구하는 것은 네 가지이고 **어느 하나라도 없으면 본 생산을 시작하지 않는다** — ① T0 사람 검증 H-1~H-3 통과(12명/5유형, 탈락 포함 보고), ② 조위정합 스파이크 개념 검증, ③ T0 실제 소요가 슬라이스 견적의 **150%를 넘으면 STOP**(범위 재설계), ④ 열린 S1 **0** · `freshness-check.sh` **exit 0**. 조건의 문면·수치가 바뀌면 계약 절이 바뀌는 것이고 본 절은 따라간다(반대가 아니다).

### 6.1 T0의 확정 명령은 무엇인가 — `reader`의 인용 고정 [C6-F10 · RFC-C7-001]

C6 판정단이 연 결함은 이것이었다 [OBSERVED, `qa/c6-review.md` C6-F10]: T0 도구 합집합은 `circuit`·`reader` 둘뿐이고 `systems/data-schemas/tools.md` §1은 두 도구 모두 `hasCommit = 아니오`인데, 이전 판 §6은 **확정·저장 롤백 테스트(`T-15`~`T-20`·`T-25`·`T-27`)를 T0 인수 기준으로 올려** 두고 정작 **T0에서 발행되는 확정 명령의 id를 어느 문서에도 적지 않았다.**

디렉터 판정(**RFC-C7-001** (1)):

> **T0의 확정(commit) 명령은 `reader`의 「인용 고정(pin citation)」이다.** `planning/gdd.md` §4 표의 `reader` 확정 층("판독 결과를 가설판에 **인용으로 고정**한다. 확정 조건: 인용에 매체·계통·관측소 출처가 채워졌을 때")이 정본이며, `systems/interaction-rules.md` §2.2의 "**판독 자체는 확정이 아니다**"와 모순되지 않는다 — **판독 ≠ 인용 고정**이다. 인용 고정은 체크포인트·저장·롤백(`T-15`)·되돌림을 **모두 거친다**.

따라서 T0 세 비트의 **완료 술어**는 다음과 같고, 이 문장들은 live `campaign.json`의 `completion` 필드에 그대로 들어 있다(문서가 데이터를 인용하는 것이지 그 반대가 아니다) [OBSERVED, 2026-09-10 R7]:

| 비트 | 도구 | 완료 술어 (데이터 원문 요지) | 확정 명령 |
|---|---|---|---|
| `t0-b1` | 없음 | **열람 술어** — 인수 각서 3항과 이관 목록 3줄을 모두 열람하고, 판 #0이 상시 슬롯에 적재되며, 목록 처리 여부가 한 번 기록된다 | **없음** |
| `t0-b2` | `circuit` | **표시 술어** — 음영 3구획이 모두 지정되고 각 구획에 근거 매체가 하나씩 붙는다 | **없음**(`circuit`은 판독 전용) |
| `t0-b3` | `reader`·`circuit` | **인용 고정 술어** — 표준 염판(plate)과 기록국 조위대장(ledger)을 각각 가설판에 인용으로 고정해 **인용 2건**이 남고, 그 둘이 결손 구간의 두 끝을 서로 다른 매체로 확정하며 검증 사본 1점이 생성된다 | **`reader` 인용 고정 × 2** |

`t0-b3.proofRequired = true`는 이 **인용 고정 2건**으로 충족된다 — 규칙은 "독립 쌍(루트 `originId` 상이 AND `sourceType` 상이)이 데이터에 존재한다"이며 `seal` 확정을 요구하지 않는다(판정단 J-engineer-1 전제 기각). 검증기 `C-07` 실측 **17/17 PASS**, `beatsWithoutPair: []` [OBSERVED].

**인수 테스트의 T0 적용 범위** [C6-F10 해소]: 확정 흐름 묶음 `T-15`~`T-20`·`T-25`·`T-27`은 **버려지지 않고 이 명령 하나에 적용된다** — T0에서 검증되는 확정 방식은 `two-step` 한 가지이며(§4.1), 확정 3방식 완주(`T-25`)와 릴리스 래치(`T-27`)의 나머지 두 방식은 **T0 범위 밖**이다. 이 구분을 브리프 DoD 6·8과 같은 문면으로 유지한다(systems 소유).

### 6.2 인수 테스트 id (공개 상태 단언 · 문자열 검색 금지) — 정본 표는 `systems/unity-implementation.md` §11

| 묶음 | id | 무엇을 막는가 |
|---|---|---|
| 결정론·무부작용 | `T-01` `T-02` `T-03` `T-04` | 프리뷰가 상태를 바꾸는 것, 같은 입력이 다른 결과를 내는 것 |
| 진행 보장 | `T-05` `T-06` `T-07` `T-14` | 필수 단서 축소, 엔딩 도달 불가, 소프트락 |
| 저장 내구 | `T-08` `T-09` `T-10` `T-13` `T-15`~`T-20` | 손상 파일 덮어쓰기, 상위 스키마 수용, 중복 커밋, 지연 완료 — **T0에서는 `reader` 인용 고정 명령에 적용한다**(§6.1) |
| 근거 독립성 | `T-11` `T-21` `T-22` `T-23` | 사본을 독립 근거로 세는 것 |
| 입력·접근성 | **`T-24`** `T-25` `T-26` `T-27` | **키보드 단독 전 퍼즐 완주 실패**, 확정 방식별 완주 불가, 키 겸용, 릴리스 래치 누락. T0 범위는 `two-step` 한 방식뿐이다(§6.1) |

경계 [OBSERVED]: `systems/prototype/`의 브라우저 참조 모형(순수 Node 리듀서 37테스트)은 **T0 게임이 아니다.** 규칙 무모순성 확인용이며 조작감·재미·플레이 시간을 주장하지 않는다. **T0 25분은 8시간을 증명하지 않는다** — 슬라이스 측정치로 480분 완주를 주장하지 않으며 `observedMedianMinutes`는 여전히 `null`이다.

---

## 7. 아트 · 리소스 현재 상태

### 7.1 스타일 가이드 요지 (정본은 `concept/style-guide.md`)

| 축 | 규칙 | 출처 |
|---|---|---|
| 팔레트 | 기본 8색 + 예비 악센트 1. **유일한 난색 악센트 `#E2AF62` ≤ 8%**, 예비 `#8C4A3A` ≤ 2%. 한 프레임에 최암부·중간·최명부 각 10% 이상 | `concept/style-guide.md` §2 |
| 색약 | 세트 A(적록)·B(청황) + **빗금 형태 부호 병행**. 색 단독 부호화 금지, 모든 상태는 색+형태+위치 3중 부호 | 〃 §2.1 [TARGET·미검증] |
| 카메라 | **2.5D 고정 관찰점(노드)만.** 이동·팬·줌 없음, 35mm 상당, 시선 1.55 m, 피치 −18°, 롤 0°, 더치 앵글 금지. 수평 수위선은 프레임 상단 38~44% | 〃 §5 |
| 매체 부호 | 염판·일지·대장 3종을 실루엣으로 구분(법2·법6 대응) | 〃 §6 |
| 금지 | §10 금지 목록 위반은 REDO(예외 없음). 이미지 내 텍스트 전면 금지 | 〃 §10, `concept/generation-manifest.md` |

### 7.2 실제로 만들어진 것 (전부 컨셉/프리비즈)

| 종류 | 수량 | 도구 | 상태 | 출처 |
|---|---:|---|---|---|
| 2D 생성 이미지 | **45장** (컨셉 21 · UI 4 · 키아트 2 · 캡슐 2 · README 7 · 프리비즈 프레임 9) | GTI `gti` CLI (`gpt-6-astra`) | `runtimeEligible:false` 전건 | `concept/generation-manifest.md`, `assets/generated/2d/*/provenance.json` |
| 3D 그레이박스 | 스크립트·메시 등 **23 엔트리** | Blender MCP (authoring script) | 〃, 실재 12메시 144 tris | `assets/generated/3d/provenance.json`, `qa/gate-measurements.md` #g5 |
| 프리비즈 영상 | **2클립** (각 5초·720p·무음) + README GIF | Higgsfield CLI `seedance_2_0_mini` | 〃 | `assets/generated/video/provenance.json`, `production/decision-log.md` 유료 도구 영수증 |

**모든 생성 에셋은 `runtimeEligible:false`로 시작한다.** 승격은 `production/decision-log.md` 감사로만 이뤄지며 `provenance.json`(도구·모델·프롬프트·해시·날짜·라이선스)이 항상 동반한다(CLAUDE.md §9, 계약 Asset pipeline).

**[OBSERVED] 생산 완료 자산은 0종이다.** 게이트 관측: `greybox 7 / pending 40 / 생산 완료 0`(`qa/gate-measurements.md` #g5, C5-F10). 45장이 존재한다는 사실은 아트 생산이 끝났다는 뜻이 **아니다**.

3D 휴머노이드는 현재 설계에 **0체**이므로 Mixamo는 조건부 미사용이다. 인물은 2D 초상과 작업대 확대뷰의 손으로만 등장한다(RFC-P4-001).

---

## 8. 상품 가설 — 전부 미승인

| 항목 | 값 | 성격 | 출처 |
|---|---|---|---|
| 가격 후보 3안 | B1 / B2 / B3 (표시가 KRW, VAT 포함) | **셋 다 미승인.** 선택 근거는 아직 없다 | `product/business-model.md` §2 |
| 실결제 하한 | **9,000원** (사용자 요구 = 출시가 최소 9,000원, 내부 제안 = 할인 후에도 유지) | 정책 제안 | 〃 §3 |
| 하한이 만드는 제약 | B1을 고르면 정책 런치 할인 상한(40%)보다 **내부 상한이 먼저 걸린다** — 세일 캘린더에 명시하지 않으면 자동 설정에서 깨진다 | [OBSERVED 산식] | 〃 §3 |
| DLC | 2~3시간 / 5,900~7,900원 후보, **출시일 동시 판매 금지** | 미승인 | 〃 §7 |
| 기능 인질 금지 | 힌트·접근성·저장/복구·되돌림·3결말·버그 수정·진행 막힘 해소·현지화·성능은 **영원히 무료** | 계약 | 〃 §7 |

**기획이 유지하는 제약은 하나뿐이다 — 8시간 분량을 가격 근거로 쓰지 않는다**(`planning/gdd.md` §11). 손익분기 표는 예측이 아니라 역산이며 판매·위시리스트·전환율 **실측 n = 0**이다(`production/cycles/c5-development.md`).

---

## 9. 생산 견적 — 견적은 약속이 아니다

| 항목 | 값 | 출처 |
|---|---|---|
| 기준 | **287인일** (1인일 = 6시간) | `production/production-estimate.json`, `production/production-estimate.meta.md` |
| 위험 여유 | **+25%** → **359인일** | 〃 (`contingencyRate: 0.25`) |
| 아트 | 63인일 (`modeling/asset-budget.md` 25+18+9+8+3과 일치) | 〃 |
| 신작 자산 | 47종 = 셸 5 / 도구 6 / 초상 5 / 공용 30 / UI 1 (표정 15은 초상 안 변형이며 독립 셸 아님) | 〃 |
| 인건비 가정 | 1인일 30만원 기회비용 가정 시 인건비만 1억 7백만원대. 외주·현지화·마케팅·세금·Direct 비용 별도 | 〃 |

**이것은 작업 분해 견적이지 납기·외주 견적이 아니다**(`kind: task-breakdown-estimate-not-quote`). 제작자 인건비를 "무료"로 놓아 수익성을 부풀리지 않는다. **생성된 45장은 이 견적의 인일에 반영되지 않는다** — 생성물은 컨셉/프리비즈이고 견적의 자산 47종은 생산 자산이다(`production/cycles/c5-development.md`).

---

## 10. 시간 수용 — 설계 480 vs 관측 null

| 키 | 값 | 성격 | 출처 |
|---|---|---|---|
| `designMinutes` | **480** | [TARGET] 설계 분량 합계(문서 상수) | live `campaign.json`, 검증기 `totalMinutes` |
| `design_fast_min` / `design_deliberate_min` | 322 / 673 | [TARGET] **시나리오 경계**. 표본 통계가 아니며 수용 밴드와 비교하지 않는다 | 검증기 `fastMinutesSum`·`deliberateMinutesSum` |
| `observedMedianMinutes` | **`null`** | [OBSERVED] 미측정 | live `campaign.json` |
| `humanPlaytests` | **`[]`** | [OBSERVED] 모집·실행 0회 | 〃 |

판정 규칙 — **RFC-P3-011**(`production/decision-log.md`):

| 항목 | 값 |
|---|---|
| **판정 키** | **`total_minus_afk_min`** (AFK 구간은 회고로 판별. 60초 무입력을 자동 제외하지 **않는다**) |
| 병기 필수 | `total_min` · `afk_total_min` — 제외 여부를 밝히지 않은 시간 수치는 무효 |
| **목표 밴드** | 처음 플레이하는 일반 이용자(공략 없음) 완주 **중앙값 450~540분** |
| **철회 트리거** (통과선이 아니다) | 중앙값 < 420 **또는** 하위 25% < 360 → "8시간" 주장을 철회하거나 콘텐츠를 증설한다. **목표를 낮춰 자동 통과시키지 않는다** |
| p75 조건 | **없음**(삭제 — 322/673은 시나리오 경계라 표본 봉투와 비교하지 않는다) |
| 표본 | 최소 12명 / 5유형, 탈락 포함 보고 |
| 합산 제외 | 선택 콘텐츠 · NG+ · 수집 100% · 로딩 · 일시정지 · 자리비움 · 재시작 대기 |

> **480분은 설계 예산이며 관측이 아니다. 표를 더한 것은 8시간을 플레이한 증거가 아니다.** 게임 내 8시간(21:00~05:00)과 설계 480분이 같은 숫자인 것은 연출 의도이며 서로를 검증하지 않는다.

---

## 11. 위험과 미측정

### 11.1 게이트 현황 — **0 / 8 PASS** [OBSERVED, `qa/gate-measurements.md`]

| 게이트 | 판정 | 막고 있는 것 |
|---|---|---|
| G1 세계관 일관성 | FAIL(문서) | violation 1건 — **C3-F29 → C6-F17로 승계됐고 R7에서 데이터 수정 완료**(§11.2-2). 재판정은 QA 몫이며 이 표는 QA가 다시 재지 않는 한 FAIL로 읽는다 |
| G2 밸런스 밴드 | N/A(전투) + NOT-MEASURED(대체) | 시뮬 0회 |
| G3 경제 건전성 | N/A(통화) + NOT-MEASURED(대체) | 지불의사 n=0, C3-F27(c)·C3-F35 |
| G4 연출·몰입 | NOT-MEASURED | 표본 0, 소스 문서 draft |
| G5 에셋·이펙트 예산 | NOT-MEASURED | 런타임 실측 0건, 생산 완료 0종 |
| G6 운영 안정성 | NOT-MEASURED | 빌드 0줄 |
| G7 피처 수용·코어루프 | NOT-MEASURED | n=0 (수용 조건 정의는 해소됨) |
| G8 신선도·메모리 | NOT-MEASURED | `memory_sync` 미검증 |

**열린 S1은 0이다.** 지금 PASS를 막는 것은 ① 남은 open S2/S3 ② 빌드 0줄·표본 n=0·시뮬 0회로 인한 NOT-MEASURED ③ G8의 memory_sync다. **②는 문서 작업으로 해소되지 않는다.**

### 11.2 이 초안이 닫지 못한 것

| # | 항목 | 소유 | 상태 |
|---|---|---|---|
| 1 | **도구 표시명 불일치** | systems + planner + concept + worldview | **판정 완료 (RFC-S6 / RFC-C6-001, 2026-09-10)** — 정본 한 벌은 `gdd.md` §4 = `style-guide.md` §9(§4.2에 전문). 잔여 작업은 **worldview 용어집 미등재 4건 등재**뿐이고 그것이 C4-F9의 closed 조건이다 |
| 2 | 비트-구역 본문 모순 | planner + synopsis | **데이터 수정 완료 (2026-09-10 · R7 · C6-F17)** — `c2-b4`·`c6-b4`의 `zoneId`를 본문(당직실)에 맞추고 스테이지 `C2`·`C6`의 `zoneIds` 확장, `c1-b2`·`c5-b2` 본문 구역 정정. 검증기 `Z-03` 신설로 회귀 차단. `c4-b1`/`c4-b3` 반전은 live 데이터에서 이미 해소. **남은 것은 synopsis 씬 지문·`chapter-beats.md` 구역 주석 동기화와 QA 재판정** |
| 3 | `plateOriginalWear` 폐기 별칭이 economy 본문에 잔존 | economy + planner | C3-F35 open |
| 4 | `system-specs/plate-readout.md` 표 안의 차단형 서술(P-R2)과 P-R9/P-R11의 문구 모순 | systems | `planning/gdd.md` §14-1 |
| 5 | `tools` 열 정의가 `chapter-beats.md` 표 A와 7건 불일치 | synopsis + planner | RFC-P3-016 |
| 6 | 정본 후보 문서가 아직 `status: draft` (`interaction-rules` · `unity-implementation` · `game-ui-contract.*` · `puzzle-balance` · `business-model` · 덱) | 소유 레인 + director | C3-F33. **본 문서가 이들을 인용한 문장은 "(C4/C5 검증 대기)"로 읽는다** |
| 7 | **결함 등록부와 회차 문서의 집계 불일치** — `cycles/c{4,5}-development.md`는 "S1/S2 open 0"이라 적으나 `qa/defect-register.md` 표는 C4-F7·F9·F11·F12·C5-F2·F3·F6를 S2 `open`으로 유지 | qa + director | 판정 필요 → RFC-C6-002 |

### 11.3 위험 등록 (R-*) — 문서로 닫지 않기로 판정된 것

| id | 위험 | 관측 근거 | 왜 문서로 닫지 않는가 | 무엇으로 닫히는가 |
|---|---|---|---|---|
| **R-T0-1** | **조작 밀도 — 손으로 하는 조작이 대부분 「표·칸 채우기」다** | [OBSERVED, `qa/c6-review.md` C6-F5 · 디렉터 판정] 정본 수치는 **표·칸 21/33 = 63.6%(비트 기준)**이다(`balance/balance-sheet.md` §10.4 재계산 명령·출력). 디렉터 판정문의 **「61%」는 어느 분모로도 재현되지 않으며**(하위과제 분모는 35/149 = 23.5%, `campaign.json`에 하위과제별 활동 분류 필드는 없다) 이전 판이 이를 「분모가 다르다」로 설명한 것은 오설명이었다 — 정정 대상은 판정문이다 [C7-F41]. 조작 유형 열의 재도출 소유는 balance | 「행동으로 추리한다」(기둥 1)의 손맛은 **문서로 증명되지 않는다.** 여기서 표를 고쳐도 손 조작 감은 여전히 미측정이다 | T0 사람 검증에서 관측한다 — `manipulation_share`(조작 분 / 총 분)와 **"손 조작을 재미로 꼽은 응답 수"**를 `handoff/verification-plan.md`에 관측 지표로 추가(systems 소유). **재설계 여부는 T0 결과 이후에 판정한다** |

[OBSERVED] 이 행은 디렉터 판정 C6-F5의 결론 그대로다 — "문서로 닫지 않는다. 위험으로 등록한다." 같은 행이 `balance/puzzle-balance.md`(또는 `balance-sheet.md`) §10에도 등록된다(balance 소유).

### 11.4 미측정 (n = 0) — 문서로는 절대 닫히지 않는 것

사람 완주 시간 · 힌트 실효성 · 퍼즐 도달성 · 이탈률 · 프레임/저장 실측 · 드로콜·텍스처 상주 · 패드 실측 · 접근성 실사용 · 색약 팔레트 검증 · 지불 의사 · 위시리스트 · 전환율 · Unity 빌드 성능. **이 목록의 어느 항목도 이 문서의 표로 대체되지 않는다.**

---

## 12. 문서 색인 — 레인별 정본 경로

경로는 전부 `_workspace/current/` 기준이다(에셋 행만 저장소 루트). **한 행 안에서 첫 항목이 폴더를 밝히면 이후 항목은 같은 폴더**다.
[OBSERVED] 본 문서가 인용한 경로는 이 회차에서 전건 존재 확인을 마쳤다 — 확인 명령은 `_workspace/current/` 상대 경로 목록에 대한 `[ -e … ]` 루프와 `ls _workspace/current/production/cycles/c[1-5]-development.md _workspace/current/qa/c[1-5]-review.md _workspace/current/planning/feature-specs/verb-0[1-6]-*.md assets/generated/2d/*/provenance.json assets/generated/{3d,video,previz}/provenance.json _workspace/current/systems/data-schemas/*.md _workspace/current/systems/system-specs/*.md` 이며 결과는 **누락 0건**이다.

| 레인 | 정본 경로 | cycle | status |
|---|---|---|---|
| 기획 | `planning/gdd.md` (색인) · **`planning/game-draft-v1.md`**(본 문서) | c3 / **c6** | current / **current** |
| 기획 | `planning/campaign.json` + `campaign.meta.md` + `validate-campaign.mjs` | c4 | current |
| 기획 | `planning/campaign-time-budget.md` · `content-matrix.md` · `update-scope.md` · `feature-specs/verb-0{1..6}-*.md` · **`feature-specs/recap-panel.md`**(신설, C6-F4) | c3 / **c6** | current |
| 기획 | `planning/market-decision.md` | c2 | **draft** |
| 세계관 | `worldview/worldview-bible.md` · `timeline.md` · `glossary.md` · `consistency-audit.md` | c3 | current |
| 시놉시스 | `synopsis/synopsis.md` · `chapter-beats.md` · `continuity.md` · `scenes-and-dialogue.md` · `campaign.md` | c3 | current |
| 시스템 | `systems/architecture-contract.md` · `data-schemas/*.md` · `system-specs/*.md` · `ops/telemetry-contract.md` · `prototype/` · `tech-verification/` | c3~c5 | current |
| 시스템 | `systems/interaction-rules.md` · `unity-implementation.md` · `game-ui-contract.json`·`.meta.md` | c5 | **draft (C4/C5 검증 대기)** |
| 밸런스 | `balance/balance-sheet.md` · `patch-deltas.md` | c3 | current |
| 밸런스 | `balance/puzzle-balance.md` | c4 | **draft** |
| 재화 | `economy/currency-map.md` · `sink-source-ledger.md` · `reward-bands.md` · `negotiation-record.md` | c3 | current |
| 재화 | `economy/resources-and-fairness.md` | c4 | current |
| 컨셉 | `concept/style-guide.md` · `art-direction.md` · `generation-manifest.md` · `sheets/README.md` · `references.md` | c4 | current |
| 모델링 | `modeling/asset-budget.md` · `asset-manifest.md` · `pipeline.md` · `specs/hub-watchroom.md` | c4 | current |
| 애니메이션 | `animation/animation-contract.md` · `anim-list.md` · `rig-requirements.md` | c5 | current |
| 모션 / VFX | `motion/motion-contract.md` · `vfx/vfx-budget.md` | c4 | **draft** |
| 연출 | `presentation/deck-outline.md` · `steam-game-plan.html`·`.meta.md` · `video-study.md` · `generate-deck.mjs` | c5 | **draft** |
| 제품(PM) | `product/assumption-tests.md` | c5 | current |
| 제품(PM) | `product/business-model.md` · `economics.json`·`.meta.md` · `steam-registration-guide.md` · `skill-application.md` | c5 | **draft** |
| 제작 | `production/premium-preproduction-contract.md` · `cycles/c{1..5}-development.md` · `production-estimate.*` · `cycle-ledger.*` · `changelog.md` · `task-manifest.md` | c3~c5 | current / draft 혼재 |
| 제작 | `production/decision-log.md` (RFC 정본 · append-only) | bootstrap | draft |
| 핸드오프 | `handoff/README.md`(독서 순서) · `codex-unity-brief.md` · `verification-plan.md` | c7 | current |
| 핸드오프 | `handoff/asset-runbook.md` | c7 | **draft** |
| 핸드오프 | `handoff/rfc-inbox/README.md` (실행자 되묻기 창구) | c7 | current |
| QA | `qa/gate-measurements.md` · `defect-register.md` · `c{1..5}-review.md` | c3~c5 | current |
| 에셋 | `assets/generated/{2d,3d,video,previz}/provenance.json` (저장소 루트) | c4 | `runtimeEligible:false` |

---

## Appendix A — English summary (for the Unity implementer)

**Pitch.** On the final night shift before the Tide Records Bureau closes, restorer **Han Seorin** reconstructs a missing four-hour gap from a flood twelve years ago, using only cross-checks between two independent record media — knowing her mother's signature sits at the end of the reconstruction.

**Shape.** Single-night, non-combat, no-currency, single-player PC investigation game. One hub plus four zones, 2.5D fixed camera nodes, no walking avatar. **9 stages / 33 beats / 480 design-minutes**, verified by `node _workspace/current/planning/validate-campaign.mjs` (**49/49 PASS** on 2026-09-10). Every beat requires at least two independent media; all 33 beats carry a 3-tier free hint set and a checkpoint.

**Six verbs, one law each.** `circuit` (only what is wired remains) · `reader` (originals wear, copies remain) · `alignment` (never trust an unaligned clock — 3 shared peaks, residual ≤ 4 min) · `routing` (this tide lacks protection capacity) · `corrosion` (salt shows up as cost — global cap 9 per routing plan) · `seal` (separate original responsibility from submission — two independent media + inside wiring range + residual ≤ 4 min).

**Two layers only.** Sandbox practice is unlimited and free and changes nothing; commit always writes a checkpoint first. The default commit mode is `two-step`, and its canonical wording is **preview first, then move focus to the confirm button and press it once — no hold** (keyboard `Space` -> `Enter`; pad `X` -> `A`; mouse: preview button -> confirm button). The reverse order — confirm, then one more confirmation dialog — is the separate, non-default `confirm-dialog` option, not `two-step`. `systems/interaction-rules.md` §1-1 is canon for all three values [C6-F14]. There is no third, immediately irreversible layer. Hold-to-confirm is opt-in, never the default. Undo has no spec-level ceiling. Essential clues can never be destroyed by any player action.

**Ground truth timeline (why C3 and C6 have a correct answer).** The Contrast Night has four canon clock marks on a 4-minute grid — the second signature being written in Seorin's name, the valve being turned (valve-open engraving), the sealing press completing (seal-complete contact engraving), and the lowland flooding. **The clock values themselves are owned by `worldview/timeline.md` §2 and are deliberately not restated here or in the Korean body.** The ordering anchor (§8 of the same file) is what makes the case provable: the two engravings sit in the same brine-line system log, in the same format, from the same recorder, so the interval between them exceeds the total post-alignment error width and their order is fixed — execution preceded the completed dual signature. Seeded at B06 (`c1-b3`), established at B14 (`c3-b3`), paid off at B26 (`c6-b3`). Without this anchor stages C3 and C6 have no correctness criterion. The anchor fixes order only — who turned the valve and with what intent is never recoverable from engravings [C6-F15].

**First build target.** Vertical slice **T0**: `hub` only, `circuit` + `reader` only, 25 minutes, beats `t0-b1`..`t0-b3`. Acceptance tests are id-addressed in `systems/unity-implementation.md` §11 — `T-24` (complete every T0 puzzle with keyboard alone) is the accessibility blocker. Full production requires **two** human proofs: the T0 slice and a separate `alignment` greybox spike.

**Honesty.** Zero Unity code exists. Zero human playtests (`observedMedianMinutes: null`, `humanPlaytests: []`). All generated 2D/3D/video assets are concept or previz, `runtimeEligible:false`, never gameplay. Quality gates stand at **0 / 8 PASS**. 480 minutes is a design budget, not a measurement; adding the table up is not evidence that anyone played for eight hours. Price candidates, the DLC, and the working title are all unapproved.
