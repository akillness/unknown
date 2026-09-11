---
updated: 2026-09-10
cycle: 20260909-preproduction-c4
status: current
supersedes: null
owner: game-systems-designer
---

# 자체 확인표 — C4 검토 F1~F4 대응이 문서에 실제로 있는가 (R4 QA 재검증 대비)

## 0. 이 표의 지위와 방법

- **목적**: `qa/c4-review.md`(FIX · material 4 / blocker 2)가 요구한 수정이 systems 문서에 **실물로 존재하는지**를, 레인 스스로 명령으로 확인한 기록. QA 재검증을 대신하지 않는다.
- **하지 않은 것**: Unity 실행 0회 · 빌드 0회 · 플레이 n=0 · 프레임타임 캡처 0건. 따라서 **이 표는 어떤 게이트도 올리지 않는다.** 확인한 것은 "계약 문장이 있다"이지 "동작한다"가 아니다.
- **status 는 바꾸지 않았다.** `interaction-rules.md` · `unity-implementation.md` · `game-ui-contract.meta.md` 는 전부 `cycle: 20260909-preproduction-c5` · `status: draft` 그대로다. C3-F33 / RFC-P3-015 에 따라 **`current` 승격은 R4 QA 검증 후 소유 레인이 수행**한다.
- 방법: 문장을 기억으로 옮기지 않고 `grep` · `python3 -c "json.load(...)"` · `node` 로 다시 읽었다. 아래 §1 이 명령 원문이다.

## 1. 재측정 명령 원문 [OBSERVED 2026-09-10 R4]

| # | 명령 | 결과 |
|---|---|---|
| V1 | `grep -c "R1\|R2\|R3\|R4" interaction-rules.md unity-implementation.md game-ui-contract.meta.md` | **11 / 21 / 5** 행 — 세 문서 모두 대응 표기가 실재 |
| V2 | `python3 -c "import json; json.load(open('game-ui-contract.json'))"` | `json parse OK` |
| V3 | `shasum -a 256 game-ui-contract.json` · `wc -c` | `83b92c822383ca2dc464a03664b8177b11e54b46f63b81869d74aaf598417730` / **37,990 B** — `game-ui-contract.meta.md` L21 기록과 **문자 일치** |
| V4 | `python3` 로 계약 구조 집계 | `screens` **19** · `verification.matrix` **17행** · `verification.acceptance` **8항** · `decisions` **13건** · `accessibility` 키 6종(`hold_alternative` · `keyboard_only` 포함) |
| V5 | `node _workspace/current/planning/validate-campaign.mjs` | `{checks 47, pass 47, fail 0, verdict PASS}` · sha `92301c0a…` · 121,457 B |
| V6 | `node -e` 로 live `campaign.json` 의 T0 스테이지 판독 | `T0.minutes 25` · `zoneIds ["hub"]` · `t0-b1 tools []` · `t0-b2 ["circuit"]` · `t0-b3 ["reader","circuit"]` |
| V7 | `node -e` 로 C3 스테이지 도구 판독 | `c3-b2 ["alignment"]` · `c3-b3 ["alignment","reader"]` — alignment 스파이크 대상 비트가 데이터에 실재 |
| V8 | `cd systems/prototype && node test-model.mjs` | **37 통과 / 0 실패 · exit 0** (C3-F36 라벨 교체 후 회귀 확인) |

## 2. F1~F4 × 요구 항목 대응표

`qa/c4-review.md` 의 「수정」 항목을 **한 줄도 합치지 않고** 그대로 세었다. 판정은 `있음`(문장·필드가 실재) / `부분` / `없음` 세 값뿐이며, `있음`은 **문서에 있다**는 뜻이지 **구현·검증됐다**는 뜻이 아니다.

### F1 — 입력맵 충돌 · 홀드 대안 · 키보드 단독 (blocker)

| 요구 | 판정 | 위치 [OBSERVED] |
|---|---|---|
| 1. routing 해제를 `X` 밖으로, KB 해제 키 신설 | **있음** | `interaction-rules.md` §1 표 「연결 해제」 행 = 우클릭 / **`Delete`** / **패널 안에서만 `Y`**, 그 아래 "`X` 충돌 해소 — `X`는 모든 화면에서 프리뷰 전용" · "`Y` 모드 분리" 두 항 |
| 2. `hold_alternative` 토글 + 0.2~1.5s 슬라이더 + 재매핑 대상 포함 | **있음** | `game-ui-contract.json` `accessibility.hold_alternative`("기본값은 두 단계 … 영점이 초에서 일점오 초까지 조절 … 확인 대화 방식도 고를 수 있다") · `screens[3] input-remap` 상태에 **"확정 방식 세 가지 중 선택"** · `interaction-rules.md` §1-1 `two-step`(기본) / `hold`(opt-in) / `confirm-dialog` |
| 3. 키보드 바인딩 신설(조사=`Enter`, 미세조절=방향키·`Shift`) | **있음** | `interaction-rules.md` §1 표에 **「키보드 단독(항상 성립)」 열이 전 행에 존재** — 대상 조사 `Enter`, 값 미세 조절 `방향키 1스텝 / Shift+방향키 정밀`, 커넥터 연결 2단계 `Enter` |
| 4. `verification.matrix` 2행 추가 | **있음(요구 이상)** | matrix **4행** 추가 확인: `[10]` T0 퍼즐 키보드 단독 완주(**"화면 도달이 아니라 퍼즐 종료"** 명시) · `[11]` 확정 방식 3종 각각 완주 · `[12]` 도구 패널 열린 상태의 `X`/`Y` 분리 · `acceptance[5]` "퍼즐 단위 완주가 키보드 단독으로 가능하고 길게 누름은 선택 기능이다" |
| (검토가 지적한 인수 기준의 구멍) | **닫힘** | 구 `acceptance[0]`("화면 도달만 본다")은 **그대로 두고**, 퍼즐 완주를 보는 `acceptance[5]`를 **추가**했다. 지표를 바꿔치기하지 않고 늘렸다 |

**전역 불변식 승격 확인**: `interaction-rules.md` §0 8·9번이 `R1` 로 "모든 도구 조작은 키보드 단독으로 완결", "길게 누름은 선택 기능" 을 **불변식 자리**에 올려두었다 — 절 하나의 문장이 아니라 문서 전체의 전제다.

### F2 — 확정 트랜잭션 저장 실패 경로 · 체크포인트 (blocker)

| 요구 | 판정 | 위치 [OBSERVED] |
|---|---|---|
| 1. 체크포인트를 실제 파일로 + `checkpointRefs[]` (또는 "자동 1슬롯" 정정) | **있음** | `unity-implementation.md` §7 저장 스키마에 `"checkpointRefs": ["checkpoint.pre-commit.json"]` · 파일 4갈래 분리(`save.json` / `checkpoint.pre-commit.json` / `save.bak` / 수동 3슬롯). `interaction-rules.md` §6 이 같은 4갈래를 반복. **검토가 지적한 "수동 3슬롯 + 자동 1슬롯" 문장은 현재 파일에 없다**(`grep -n "슬롯" interaction-rules.md` → 3행, 전부 4갈래 서술·seal 근거 슬롯) |
| 2. `screens[14]` 저장 중·저장 실패 상태 | **있음** | `result-checkpoint` 상태 5종 중 **「저장 진행 중 영수증 대기」 · 「저장 실패 재시도 또는 뒤로」** |
| 3. 인수 테스트 T-15 신설 | **있음(요구 이상)** | `unity-implementation.md` §11 `T-15`(rename 직전 쓰기 실패) 외에 `T-16`(중간 절단) · `T-17`(멱등) · `T-18`(지연 완료) · `T-19`(rename 후 크래시) · `T-20`(`SavePending` 중 부분 입력) **6건** |
| 4. "1프레임 잠금 vs 200 ms 저장" 정정 | **있음** | §9 표가 두 항목을 **다른 축**으로 분리(`확정 입력 렌더 응답(ack) 1프레임 · 렌더` / `저장 디스크 완료 200 ms · I/O`)하고 "저장이 프레임 예산 안에 끝나야 한다는 요구는 이 계약에 없다" 를 명시. `interaction-rules.md` §5 3항이 **전역 입력 잠금 없음 · 중복 확정만 차단**으로 같은 결론 |
| (핵심 명제) | **있음** | §0 불변식 10 `R2` **"확정은 저장이 성공해야 확정이다. 저장 전 화면은 후보(candidate)"** · `acceptance[6]` "저장이 성공하기 전에는 어느 화면도 확정된 결과로 보이지 않는다" |

### F3 — 근거 독립성(매체 종류 ↔ 출처)

| 요구 | 판정 | 위치 [OBSERVED] |
|---|---|---|
| 1. `mediumType`/`originId` 분리 + 판정 한 문장 확정, 모순 문장 삭제 | **있음** | `unity-implementation.md` §5 "`sourceType`(염판/일지/대장)과 `originId`(물리 출처)를 분리하고 파생본은 `copiedFrom` 으로 루트를 가리킨다" · 임포트 불변식 1 = **`sourceType` 상이 AND `originId` 상이**. `interaction-rules.md` §3 `R3` 가 같은 문장 |
| 2. "매체 경로" 세 번째 용어 폐기 | **있음(명시적 폐기)** · **2026-09-10 R6: 선언과 실제 사용의 어긋남 해소** | 선언은 `unity-implementation.md` §5 1항 괄호 — "C4의 모호한 '매체 경로'라는 세 번째 용어는 폐기하고 이 두 필드로만 말한다". **C5-F5 재검증 2 가 지적한 systems 소유 6곳**(`plate-readout` 3 · `drainage-routing` 2 · `beats` 1)과 미지목 1곳(`wiring-trace` W-F4)을 정본 표현으로 다시 썼다. 재측정 `grep -rn "매체 경로" _workspace/current/systems` → **남은 4행은 전부 「폐기했다」는 선언·기록 문장**이고 규칙 본문 사용은 **0건** [OBSERVED 2026-09-10] |
| 3. T-11 픽스처에 원본+파생 쌍, matrix 행 추가 | **있음** | `T-21`(원본 + 파생 스캔 → `copiedFrom` 루트 해석으로 거부) · `T-22`(다른 `originId` 염판 2점 → **종류 중복으로 의도적 거부**, 사유 구분) · `T-23`(`proofRequired` 아닌 경로는 서명 불요). matrix `[15]` · `[16]` 이 같은 두 경우, `acceptance[7]` 이 "종류와 루트 출처를 **모두** 검사하고 거부 사유를 구분" |
| 4. seal 슬롯 뱃지에 종류·출처 둘 다 | **있음** | `interaction-rules.md` §2.6 "슬롯마다 **종류 배지와 출처 배지를 둘 다** 표시. 판정 실패 시 어느 조건이 어긋났는지 문장으로 표기" |
| (데이터 측 증거) | **있음** | 검증기 `C-07` = `proofRequired` 15비트 전건 독립쌍 존재 **PASS 15/15** (V5). 규칙이 문서에만 있는 것이 아니라 **저작 데이터가 이미 만족**한다 |

**남은 판정 요청 1건**: `interaction-rules.md` §7-1 **RFC-S5**(확정 사본의 루트 승계 — 예외를 둘 것인가)는 여전히 **open**이며 디렉터 소유다. 레인 제안은 "예외 없음", 근거는 C-07 이 예외 없이도 15/15 PASS 라 **예외의 실익이 없다**는 것.

### F4 — T0 범위 · 에셋 수량 · 공간별 기능

| 요구 | 판정 | 위치 [OBSERVED] |
|---|---|---|
| 1. `asset-budget.md` T0 행 정정(셸·도구 수) | **불필요해짐 — 3자 일치** | `unity-implementation.md` §10 이 T0 를 **`hub` 1공간 + `circuit`·`reader` 2도구 + 25분**으로 축소 확정 → `modeling/asset-budget.md:24` 의 `T0: 셸1·도구2·초상1` 과 **수량이 맞는다**(셸 1 = `hub`, 도구 2 = circuit·reader). 검토가 계산한 "+63% 과소 산정"은 T0 범위를 줄이는 방향으로 해소됐다. **modeling 문서는 읽기만 했고 수정 0건** |
| 2. T0 공간과 T0 도구의 홈 공간 일치 | **있음** | live `campaign.json` T0(V6): `zoneIds ["hub"]` · 비트 도구 `[] / ["circuit"] / ["reader","circuit"]` — **문서·데이터가 같은 조합**. `gate`(routing 홈)도 `dock`(alignment 홈)도 T0 에 없고, T0 가 쓰지 않는 공간도 만들지 않는다 |
| 3. seal 의 배선 술어가 T0 에서 스텁이 되는 문제 | **있음(구조적 해소)** | §10 "alignment·routing·corrosion·seal 은 **T0 에 없다**. T0 의 결론 판정은 이 두 도구로 닫히는 것만 쓰며, 배선 범위 술어는 `circuit` 이 실제로 산출한다(스텁 아님)" — 스텁을 두는 대신 **스텁이 필요한 도구를 T0 에서 뺐다** |
| 4. alignment 검증 경로 | **있음** | §10 "alignment 는 별도 그레이박스 스파이크 … 비트 `C3-b2`/`C3-b3` 만 떼어 독립 측정 … 추가 완성 에셋 0" + **본 생산 게이트는 (a) T0 사람 검증과 (b) alignment 스파이크 두 증거를 모두 요구**. V7 로 해당 비트의 도구가 데이터에 실재함을 확인 |
| (정직성) | **있음** | §10 "T0 25분은 8시간을 증명하지 않는다 … `observedMedianMinutes` 는 여전히 `null`" |

## 3. 교차 확인 — 문서 · 데이터 · 타 레인이 같은 값을 말하는가 [OBSERVED]

| 축 | systems 문서 | live `campaign.json` | 타 레인 | 일치 |
|---|---|---|---|---|
| T0 길이 | 25분 (`unity-implementation.md` §10) | `T0.minutes = 25` | `balance` T0 프로토콜 | **일치** |
| T0 공간 | `hub` 1개 | `T0.zoneIds = ["hub"]`, 3비트 전부 `zoneId: hub` | `modeling/asset-budget.md:24` 셸1 | **일치** |
| T0 도구 | `circuit` + `reader` | `t0-b2 ["circuit"]` · `t0-b3 ["reader","circuit"]` | `asset-budget.md:24` 도구2 | **일치** |
| 독립성 판정 | `sourceType` AND 루트 `originId` | 검증기 `C-07` PASS 15/15 | `synopsis/continuity.md` §5 K표(C-07 파생, A37) | **일치** |
| 확정 기본값 | `two-step` 기본 · `hold` opt-in | — | `planning/gdd.md` §3.3(RFC-P3-015 로 정정 지시됨) | systems 측 **일치**, planner 측은 R4 QA 확인 대상 |

## 4. 자체 발견 — **미수정**, R4 QA·디렉터 판정 대기

레인이 스스로 찾았고 **이번 회차에 고치지 않은 것**을 숨기지 않고 적는다. 넷 다 지시받은 절(a~d) 밖이라 손대지 않았다.

| # | 위치 | 관측 | 왜 지금 고치지 않았나 | 제안 |
|---|---|---|---|---|
| SC-1 | `unity-implementation.md` §7 저장 스키마 `"chapter": 3, "dayIndex": 2, "propertyProtection": false` ↔ `data-schemas/save.md` §1·§2 (`stageId`/`beatId`, `dayIndex` 없음, `propertyProtection` = `lowland\|dock\|null`) | 같은 레인의 두 문서가 **세이브 v1 필드를 다르게 적는다.** 캐논은 단일 야간이므로 `dayIndex` 는 항상 0인 죽은 필드 | 이미 **RFC-S3**(`architecture-contract.md`)로 열려 있는 디렉터 판정 대상이다. 레인이 임의로 한쪽을 지우면 판정을 앞질러 결정하는 것이 된다. 세이브 필드명은 CLAUDE.md §9 불변식 대상이라 더 조심한다 | RFC-S3 채택 시 `unity-implementation.md` §7 을 `save.md` 스키마로 교체(코드 0줄이므로 마이그레이션 비용 0). 반대 판정이면 `save.md` 를 맞춘다 |
| SC-2 | `game-ui-contract.json` `verification.matrix[12].expected` "도구 **휴**은 패널 밖에서만 열린다" | 오탈자(`휠` → `휴`). 의미는 `interaction-rules.md` §1 「`Y` 모드 분리」와 같다 | 지시된 편집 대상은 `game-ui-contract.**meta.md**` 이고 JSON 본문이 아니다. JSON 을 고치면 해시가 바뀌어 meta L21 기록(V3 에서 문자 일치 확인)을 같은 회차에 다시 갱신해야 한다 | R4 QA 가 승인하면 1글자 수정 + meta 해시 갱신을 한 번에. `matrix[15].expected` "출처 중복**로**"(→ `중복으로`) 도 같은 처리 |
| SC-3 | `unity-implementation.md` §10 이 alignment 스파이크 대상을 **`C3-b2`/`C3-b3`** (대문자) 로 적음 | live 비트 id 는 소문자 `c3-b2`·`c3-b3`. C3-F30 이 "문서 간 인용 키는 campaign id 뿐"으로 정했고 `beats.md` §0 은 id 를 소문자 kebab 으로 규정 | 표기 정정이지만 지시 절 밖 | `c3-b2`/`c3-b3` 로 교체 |
| SC-4 | RFC-P3-015 (F20) 이 180초 자동 제안의 정본 위치를 "`interaction-rules` §4·puzzle-balance" 로 적음 ↔ 실제 `interaction-rules.md` §4 에는 180초 문장이 **없다**(`grep -n "180" interaction-rules.md` → 0행) | 규칙 자체는 **존재한다** — `system-specs/hint-system.md` L17·L18·L28·L41·L43·L48·L62·L63·L73 이 "180초 단일 제안 + 180초 쿨다운 · 자동 승격 없음"을 상태기계까지 정의 | 내용 결손이 아니라 **RFC 의 위치 인용이 어긋난 것**이라 디렉터 판정이 싸다 | `interaction-rules.md` §4 에 한 줄 추가: "무진전 자동 제안(180초 제안 + 180초 쿨다운, 단계 자동 승격 없음)의 정본은 `system-specs/hint-system.md` 다." 또는 RFC 본문의 인용 위치 정정 |

## 4-1. 이번 회차(R4) systems 레인이 실제로 편집한 파일 — QA diff 대비

지시받은 절만 고쳤고, 그 밖의 편집은 **스테일 `[OBSERVED]` 제거**(C3-F34 와 같은 결함 유형의 자기 예방)뿐이다. 전부 systems 레인 소유 파일이며 타 레인 파일 수정·이동·삭제 **0건**이다.

| 파일 | 무엇을 | 근거 |
|---|---|---|
| `prototype/model.mjs` | L58 `EVENT_PAIRS` 라벨 1줄 (`id`·`gapMin`·테스트 불변) | **C3-F36** |
| `prototype/prototype.meta.md` | `model.mjs` 해시 행 갱신 + C3-F36 영수증 절 신설 + 폐기 라벨 보존 주의 | 위 편집의 영수증 |
| `data-schemas/save.md` | **§2.1 「부식 저장 필드 없음 — 부식은 구성안 시험의 파생값」 신설**, §4 상호참조, 불변식 `S-I9` | **C3-F27(c)** · RFC-P3-009 |
| `data-schemas/beats.md` | `Beat.zoneId` 필드 행 신설(§4), 키 수 23→24, §1.1 실측표 R4 갱신, 불변식 `B-I19`(Z-01·Z-02) | **C3-F22** · RFC-Q1 |
| `data-schemas/zones.md` | 불변식 `Z-I9`(Beat.zoneId 교차 검사) | C3-F22 |
| `system-specs/{plate-readout,tide-alignment,drainage-routing,corrosion-budget,dual-seal}.md` | 인용주의 `§4-1` → 제목 문자열 인용 | **RFC-W3** |
| `system-specs/wiring-trace.md` | 없던 인용주 **신설**(법1은 폐기 문구 없음) | RFC-W3 · A33 의 "6종" 주장과 실측 5종의 차이 해소 |
| `tech-verification/README.md` | §0-1 에 회차 표기 + **§0-2 R4 재측정 영수증** 신설 | RFC-Q1 |
| `tech-verification/c3-fixloop2-canon-alignment.md` | 말미에 「후속(R4)」 절 추가 (본문 3곳의 `§4-1` 표기는 **당시 기록으로 보존**) | RFC-W3 · CLAUDE.md §2 삭제 금지 |
| `architecture-contract.md` | RFC-S4 종결 영수증에 R4 재확인 행, RFC-S1 증거 행 갱신 | RFC-Q1 |
| `ops/telemetry-contract.md` | 설계 상수 재실행 확인(480/322/673 불변) + 대상 파일 해시 갱신 | RFC-Q1 |
| `data-schemas/{plates,tools,hints,zones,save}.md` | §0 표기 규약 행의 **고정 sha 인용 제거** → 검증기 출력 참조 | RFC-Q1(고정 숫자 재기재 금지) |
| `interaction-rules.md` | **1행만** — §3 C-07 영수증의 「검사 대상 파일 sha256」에 R4 재실행 결과 추가. **규칙 문장·구조·`status` 는 건드리지 않았다** | RFC-Q1. 이 파일은 R4 QA 검증 대상이므로 최소 편집 |

`unity-implementation.md` · `game-ui-contract.meta.md` · `game-ui-contract.json` 은 **읽기만 했다**(편집 0건). 세 파일의 `status: draft` 도 그대로다.

## 5. 이 표가 증명하지 않는 것 [OBSERVED]

- **F1~F4 가 해소됐다고 선언하지 않는다.** 확인한 것은 요구된 문장·필드·테스트 id 의 **존재**다. 판정은 `qa/c4-review.md` 를 쓴 QA 의 것이다.
- `T-15`~`T-27`, matrix 17행, acceptance 8항은 **전부 미실행**이다. 인수 테스트가 문서에 있다는 사실은 통과를 뜻하지 않는다 — Unity 프로젝트의 `Assets` 파일 수는 여전히 0이다.
- `hold_alternative` · `keyboard_only` 는 계약 문장이며 **접근성 실사용 검증 0건**이다. 홀드를 유지할 수 없는 실제 플레이어가 완주했다는 증거는 없다.
- T0 3자 일치(§3)는 **수량과 id 의 일치**이지 T0 가 재미있거나 25분이라는 증거가 아니다. `observedMedianMinutes = null`, 표본 n=0.
- §4 의 4건은 **미수정 상태 그대로**다. 이 표에 적었다는 사실이 해소가 아니다.

## 6. R4 QA 에 넘기는 것

1. **status 승격 판단**: `interaction-rules.md` · `unity-implementation.md` · `game-ui-contract.meta.md`(+ `game-ui-contract.json`) 를 `cycle: 20260909-preproduction-c5` 유지 · `status: draft → current` 로 올릴지 (C3-F33 · RFC-P3-015 · RFC-Q2 — 같은 cycle 제자리 갱신이므로 아카이브 불필요). **승격은 QA 검증 후 systems 가 수행**하며 이번 회차에는 하지 않았다.
2. **§4 SC-1 ~ SC-4** 처리 방향. SC-1 은 디렉터(RFC-S3), SC-2·SC-3 은 QA 승인 후 systems 1회 편집, SC-4 는 디렉터의 RFC 인용 정정.
3. **RFC-S5**(확정 사본의 루트 승계 예외 여부) 판정 — `interaction-rules.md` §7-1.
