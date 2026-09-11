---
updated: 2026-09-10
cycle: 20260909-preproduction-c3
status: current
supersedes: null
owner: game-planner
---

# C3 업데이트 범위 — 기획 레인

사이클 타입: `preproduction` / 회차 **C3 (캠페인·시간·서사 비트)** / 닫는 질문: "33비트가 480분 설계 예산과 행동 예산으로 역산되는가"
이 문서는 **계약**이다. 다른 레인은 이 in/out 표를 인용해 자기 범위를 정한다. 범위가 커지면 늘리지 않고 **재-인테이크**한다.

## 1. C3 in — 기획 레인이 이번 회차에 실제로 한 것

| # | 산출물 | 상태 | 내용 |
|---|---|---|---|
| 1 | `planning/gdd.md` | 신규(current) | C2 GDD의 후속본. 연습/확정 분리 원칙, 동사 6개 확정표, 입력·힌트·저장·접근성 목록, 범위 in/out 표 추가. 가격 숫자 삭제 |
| 2 | `planning/feature-specs/verb-01`~`verb-06` | 신규(current) | 동사 6개 각각 goal / player_fantasy / rules / edge_cases / acceptance_criteria(D·B 분리) / telemetry_fields / touched_lanes |
| 3 | `planning/content-matrix.md` | 신규(current) | 5구역 × 22상태 × 33사건 × 동사 × 자료 2종 매트릭스, 재방문 횟수 포함. C1 F1 해소의 입력 문서 |
| 4 | `planning/update-scope.md` | 신규(current) | 본 문서 |
| 5 | `planning/campaign-time-budget.md` | 신규(current) | 480분 행동 예산 역산. C3 종료 수정 루프에서 live 계보로 **전면 재도출** |
| 6 | `planning/campaign.json` + `campaign.meta.md` | 갱신(current) | C3-F11 단서 1건 추가, 영수증 실측 재작성 |
| 7 | **`planning/validate-campaign.mjs`** | **신규(2026-09-10)** | 44개 검사 재실행 가능 검증기. Node 내장만 사용. `campaign.meta.md` §4의 "손으로 옮겨 적은 표"를 대체한다 |

## 2. C3 out — 이번 회차에서 **하지 않은** 것

| 항목 | 이유 | 언제 |
|---|---|---|
| 480분 행동 예산 역산 | 비트별 구역 배정이 아직 [INFERENCE]이고 행동 단위 원가가 없다 | `planning/campaign-time-budget.md` (C4) |
| 가격·할인·DLC 가격 결정 | PM 레인 소유. GDD는 인용만 한다 | `product/business-model.md` (C5) |
| 다른 레인 문서 편집 | 쓰기는 `planning/`에만. worldview·systems·synopsis·qa·production은 읽기만 했다 | — |
| `campaign.json` **스키마 확장**(비트 단위 `zoneId` 등) | systems·synopsis 승인이 필요하다. 이번 회차의 데이터 편집은 C3-F11 단서 1건으로 제한했다 | C4 이후 RFC (RFC-P3-004) |
| 사람 플레이·성능·판매 측정 | 빌드 0줄, 모집 0회 | 검증 슬라이스 이후 |
| 게이트 PASS 판정 | QA가 재고 디렉터가 판정한다 | C5 |
| git commit / push | 사용자가 직접 수행 | — |

## 3. 이번 회차에 발견한 것 (근거 있는 것만)

| # | 발견 | 근거 | 성격 |
|---|---|---|---|
| P1 | `corrosion`·`routing`이 각각 필수 비트 **3건**(3/33 = 9.1%)에만 등장. 학습 후 재등장 공백이 길다 | `campaign.json` 도구별 집계 | [OBSERVED] 편중 / [INFERENCE] 지루함 위험 |
| P2 | `campaign.json`은 `zoneIds`를 **스테이지 단위로만** 갖는다. 비트별 구역은 전부 추론이다 | JSON 스키마 | [OBSERVED] |
| P3 | ~~systems 스펙 6종이 참조하는 스키마 5파일과 `ops/telemetry-contract.md`가 존재하지 않는다~~ → **해소.** `systems/data-schemas/`에 **6파일**(`beats.md` `hints.md` `plates.md` `save.md` `tools.md` `zones.md`), `systems/ops/`에 `telemetry-contract.md`(cycle c3 · status current) | **2026-09-10 재측정 [OBSERVED]**: `ls _workspace/current/systems/data-schemas/` → 6건 · `ls _workspace/current/systems/ops/` → `telemetry-contract.md` 1건 · `sed -n '1,7p'`로 frontmatter 확인 | [OBSERVED] 이전 주장은 **스테일**이었다(C3-F15). 참조 대상 부재를 이유로 한 FIX는 철회한다 — 다만 **내용 검증은 하지 않았다**(파일 존재 ≠ 필요한 필드 수록) |
| P4 | ~~systems `plate-readout.md` P-R2의 `readBudget = 3`(4회째 원본 붕괴)을 GDD가 따른다~~ → **재정정.** 그 차단형 모델은 `economy/currency-map.md` §4.2가 **옵션 C로 거부**한 것이다(법2 위반·진행 불가 경로 생성). 채택된 것은 **옵션 B — 비차단 카운터 `plateOriginalWear`, 상한 3, 사본 무제한**이다. GDD §4.1과 `verb-02` R3a~R3d를 그쪽으로 다시 맞췄다 | 세 문서 대조(worldview 법2 · economy §4.2 · systems P-R2) | [OBSERVED] 기획 문서는 해소. **systems `plate-readout.md` P-R2 문구는 아직 옛 모델** → systems 인계 |
| P5 | 저지대 전용 비트가 **2건**뿐이며 제로섬 상대인 부두(5건)와 비대칭 | content-matrix §4.3 | [OBSERVED] |
| P7 | ~~`campaign.meta.md` 영수증이 실물과 어긋난다 (주장 74342 / 5029d44a…, 실물 74341 / 2bfe4d52…)~~ → **해소.** 그 주장 자체가 스테일이었다. `2bfe4d52…`로 시작하는 파일은 저장소 어디에도 **없다**(C3-F2). 현재 실물과 영수증이 **일치**한다 | **2026-09-10 재측정 [OBSERVED]**: `shasum -a 256 _workspace/current/planning/campaign.json` → `fdabf1d421932d679429870a06eeb31e9527562370c6a107aec77c2c884fb2b7` · `wc -c` → **120479** · `campaign.meta.md` §1이 같은 값을 적는다 | [OBSERVED] 계보 이력: `5029d44a…`/74342 = 아카이브 c3 판 · `775a984c…`/120087 = C3-F11 수정 **전** live 판 · `fdabf1d4…`/120479 = `zoneId` 추가 **전** · **`92301c0a…`/121457 = 현재(2026-09-10 R4, C3-F22)**. 이후 인용은 고정 숫자 대신 검증기 출력의 `sha256`을 쓴다(RFC-Q1) |
| P8 | live `campaign.json` 33비트 중 **1건(`c1-b4`)이 매체 2종 규칙을 어겼고**, 도구 없는 비트가 **5건**이며, 그 사실이 3개 문서에 "0건 / 33-of-33 PASS"로 적혀 있었다 | `node _workspace/current/planning/validate-campaign.mjs` (신설, 44개 검사) | [OBSERVED] **해소** — 단서 1건 추가 후 44/44 PASS(`campaign.meta.md` §4·§10) |
| P9 | `tools` 열의 의미가 live JSON과 `synopsis/chapter-beats.md` 표 A에서 **7건 어긋난다**. 비트 **재생 순서**도 두 문서에서 다르다(C1·C3·C4·C5·C7) | JSON 배열 vs 표 A B번호 전건 대조 | [OBSERVED] 미해소 → RFC-P3-016 · RFC-P3-019 |
| P6 | 12년 전 이중서명(책임 요건)과 현재 제출 서명(검증 절차)의 **UI·표기 구분이 어느 문서에도 없다** | systems `dual-seal.md` 대조 | [OBSERVED] |

> [OBSERVED] 본 회차 작업 중 다른 세션이 `_workspace/archive/20260909-preproduction-c3/`(planning·synopsis·worldview)와 `_workspace/archive/20260909-preproduction-c4/`(systems·animation)를 생성했고 `planning/campaign.json`·`campaign.meta.md`가 c4 판으로 교체됐다. 기획은 다른 세션의 변경을 되돌리지 않았고, 소유 파일이라도 **동시 편집 중인 `campaign.meta.md`는 수정하지 않았다**(P7은 보고만 한다). 즉 아래 C4 인계 항목 중 일부는 이미 다른 레인에서 진행 중일 수 있다.

## 4. RFC 현황 (등재는 director — 기획은 `production/decision-log.md`를 쓰지 않는다)

### 4.1 C3에서 기획이 연 RFC와 그 결말 [OBSERVED]

| id | 질문 | 기획 제안 | 판정 |
|---|---|---|---|
| RFC-P3-001 | 동사 이름 2건을 "부식 시험"·"이중서명"으로 확정하는가 | 확정 | 유지 — `worldview/glossary.md` 정의와 정합 |
| RFC-P3-002 | `corrosion`·`routing` 3건 편중(P1) | (c) 현행 유지 + 재도입 리마인더 | **열림** |
| RFC-P3-003 | 부재 스키마 5파일(P3)을 누가 언제 만드는가 | systems가 C4에 작성 | **해소** — 6파일 존재 확인(§3 P3) |
| RFC-P3-004 | `campaign.json` 비트에 `zoneId` 추가 | 추가 | **C4 이후로 이월.** 그때까지 `content-matrix.md` §3이 단일 출처(C3-F22 처리) |
| RFC-P3-005 | 과거 서명 / 현재 서명의 표기 구분(P6) | 시각·문구 모두 구분 | **열림** — `verb-06` D7 |
| RFC-P3-006 | `campaign-time-budget.md`의 소유와 시점 | planner 소유, C4 착수 | **완료** |
| RFC-P3-007 | `campaign.meta.md` 영수증 불일치(P7) | c4 판을 쓴 세션이 재계산 | **해소** — §3 P7 재측정 |

### 4.2 디렉터 판정으로 닫힌 것 (기획 레인이 이번 회차에 반영) [OBSERVED]

| id | 판정 | 기획이 고친 곳 |
|---|---|---|
| RFC-P3-008 | 계보 B(live `campaign.json`)가 유일 정본 | `gdd.md` §3.2·§4.2·§10 · `content-matrix.md` 전면 · `campaign-time-budget.md` 전면 · `feature-specs` 6종 yaml |
| RFC-P3-009 | 부식예산 = `routing` 구성안의 **전역 상한 9**. 확정 소모·누적 고갈·리셋 없음 | `gdd.md` §4 표·§4.1 · `verb-04` R3·E3 · `verb-05` R2·R3a·R3b·R4·E2·E4·D5·D7 |
| RFC-P3-011 | 판정 키 `total_minus_afk_min` · 목표 450~540 · 420/360은 철회 트리거 · p75 삭제 | `gdd.md` §10.1 · `campaign-time-budget.md` §8·§9.2·§9.3 |
| RFC-P3-012 | T0의 한도연·판 #0 노출은 세션 P 결정 유지 | 데이터 원복 **하지 않음**. 검증기 `K-05`가 공개 순서를 검사한다 |
| RFC-P3-013 | 밸브 H-1:24 → 봉인 완료 접점 H-1:04, 침수 H+0:12 | 데이터 원복 **하지 않음**. 검증기 `K-03`·`K-04`가 캐논 시각을 검사한다 |
| RFC-P3-014 | 6법 호명 문구 정본 = 아카이브 c3 bible §3 | `gdd.md` §4 머리 · `feature-specs` 6종 `law:` **전건 대조 결과 이미 일치** — 문구 변경 0건, 출처 표기만 추가 |
| RFC-P3-015 | 확정 기본값 `two-step`, `hold` opt-in · 힌트 180초 단일 제안 | `gdd.md` §3.3 층B·§5·§6 · `verb-04` R3·B5 |

### 4.3 기획이 새로 여는 RFC [OBSERVED]

| id | 질문 | 기획 제안 | 영향 레인 |
|---|---|---|---|
| RFC-P3-016 | `tools` 열의 정의 — 확정 / 확정+연습 / 실사용 중 무엇인가 (live와 표 A가 7건 불일치) | 데이터를 "그 비트에서 쓰는 도구"로 정의하고 표 A는 확정/연습을 별도 열로 분리 | synopsis, planner, systems |
| RFC-P3-017 | 5범주 텔레메트리 키 `design_activity_*_min`(53/190/168/28/41) 채택 | 채택. 스키마 버전 유지 | systems, qa |
| RFC-P3-018 | 신규 자산 30.0%의 소유 | 기획 추정치를 concept·modeling·presentation 산정으로 교체 | concept, modeling, presentation |
| RFC-P3-019 | 비트 **재생 순서**의 정본 (JSON 배열 vs 표 A B번호) | JSON 순서를 정본으로 하고 표 A의 B 번호를 재부여 | synopsis, planner, director |
| RFC-P3-020 | `c4-b1` 매체(데이터 대장+일지 vs 표 A 일지+염판) · `c1-b4` 구역 텍스트(데이터 "제3수문" vs 표 A `hub`) | 데이터를 정본으로 두고 표 A·씬 지문을 맞춘다. 0분 수정 | synopsis, planner |

[OBSERVED] `campaign-time-budget.md` 이전 판이 기획 RFC로 쓴 번호 `RFC-P3-009`·`RFC-P3-010`은 **디렉터가 다른 판정에 이미 사용**했다. 위 표의 `RFC-P3-017`·`RFC-P3-018`이 그 재번호다.

## 5. 위험 (이번 회차 종료 시점)

| # | 위험 | 영향 | 현재 완화 | 잔여 |
|---|---|---|---|---|
| R1 | **480분이 여전히 가설이다.** 합계 검산·행동 예산 역산은 끝났으나 사람 플레이가 **n=0**이다 | 8시간 주장·가격 논리·상점 문구가 전부 이 위에 서 있다 | 키 분리(RFC-P3-011 판정 키 `total_minus_afk_min`), 문서 4곳에서 "설계 예산 ≠ 관측" 명시 | 높음 — **검증 슬라이스 전까지 해소 불가** |
| R2 | 문서가 레인별로 빠르게 늘어 **서로 다른 이름·수치가 생길 수 있다** | C5 회귀에서 모순 다발 — C3 QA가 실제로 24건을 냈다 | **기계 검증기 신설**(`validate-campaign.mjs`, 44검사)로 데이터-문서 대조를 재실행 가능하게 만들었다. 손으로 옮겨 적은 영수증은 폐기 | 중간 → **낮음(데이터 측면) / 중간(문서 간 인용)** |
| R3 | 접근성 요구(GDD §8, 13항목)가 systems 계약보다 **넓다** | 검증 부담이 C4·C7에 몰린다 | 항목마다 빌드 후 판정 기준을 숫자로 명시 | 중간 |
| R4 | 비트별 구역이 **데이터에 없다**(P2). `content-matrix.md` §3 표에만 존재한다 | 코드·에셋이 그 표를 읽지 않으면 런타임 검증 불가 | §3 표를 단일 출처로 **선언**하고 JSON 확장은 C4 이후 RFC로 이월 | 중간 — 구조적 |
| R7 | `tools` 열과 **비트 재생 순서**의 정의가 두 문서에서 다르다(P9) | 재방문 런·신규 자산 비율·"확정 N회" 주장이 전부 흔들린다 | 두 값을 나란히 적고 데이터 기준을 명시. RFC-P3-016·019 | 중간 |
| R5 | 동사 2개의 사용 빈도 편중(P1) | 후반에 도구를 잊는다 | 리마인더 제안 | 낮음~중간 |
| R6 | 실측이 전부 n=0이라 어떤 D 게이트 PASS도 **G 게이트로 승격되지 않는다** | 성공 착시 | 모든 문서에 NOT-MEASURED 표기 | 구조적 — 빌드 전까지 상수 |

## 6. C4 인계 (기획이 다음 회차에 받는 것 / 주는 것)

**받는 것**
| 출처 레인 | 필요한 것 | 없으면 생기는 일 |
|---|---|---|
| systems | (스키마 6파일은 **존재 확인됨**) `plate-readout.md` P-R2를 economy §4.2 옵션 B로 재작성 · `corrosion-budget.md` `systemLimits` 폐기(RFC-P3-009) · 과거/현재 서명 구분 · `interaction-rules.md` current 승격 | `verb-02` R3a·`verb-05` D5·`verb-06` D7·`gdd.md` §6 각주가 FIX로 남는다 |
| synopsis | 비트별 대체 증인 목록, 후일담 문단 2쌍 본문, 비트별 시각 간격, **표 A 분 열의 live 재도출**(RFC-P3-008), `tools` 열·재생 순서 정의(RFC-P3-016·019), `c4-b1` 매체·`c1-b4` 구역(RFC-P3-020) | `verb-04` D6 · `verb-06` D6 · `verb-02` D6이 FIX로 남고 `content-matrix.md` §3.1이 미해소로 남는다 |
| balance | B 기준치 승인(첫 조작 60초, 힌트 3단 도달률, 자력 해소율 등) | 기획이 제안한 [TARGET] 숫자가 근거 없는 값으로 남는다 |
| presentation / concept / modeling | 상태 22종의 연출 재사용 비율 | 에셋 물량 견적 불가 |

**주는 것**
| 산출물 | 받는 레인 | 용도 |
|---|---|---|
| `content-matrix.md` §1~§4 | modeling, concept, presentation | 구역·상태 단위 물량 견적 |
| `feature-specs/verb-0*.md` acceptance_criteria B | qa, balance | 검증 슬라이스의 측정 항목 |
| `feature-specs/*` telemetry_fields | systems | `ops/telemetry-contract.md`의 입력 |
| `gdd.md` §9 범위 in/out | 전 레인 | 범위 동결 기준 |

**C4에서 기획이 직접 쓸 것**
1. ~~`planning/campaign-time-budget.md`~~ — **완료** (RFC-P3-006, C3 종료 수정 루프에서 live 계보로 재도출)
2. `planning/priority-board.md` — P1~P9과 C4 발견을 (플레이어 영향 × 운영 위험 × 노력)으로 순위화 — **미착수** [OBSERVED]. 기획 레인 필수 산출물이며 이번 회차에 만들지 못했다
3. `content-matrix.md` 갱신 — RFC-P3-004 승인 시 `zoneId` 관측값으로 재생성
4. `validate-campaign.mjs` 확장 — 비트-구역 매핑이 데이터에 들어오면 `content-matrix.md` §3과의 대조 검사를 추가

## 7. 정지선 재확인

이 회차의 어떤 산출물도 다음을 승인하지 않는다: Steam 계약·신원/은행/세금 제출·등록비 결제·유료 도구/외주·상점 공개·git commit/push·Unity 전체 콘텐츠 생산.
승인된 것은 로컬 문서 작성뿐이며, 다음 실행 단위는 **20~30분 검증 슬라이스**다.
