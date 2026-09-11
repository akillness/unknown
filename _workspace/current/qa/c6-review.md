---
updated: 2026-09-10
cycle: 20260909-preproduction-c6
status: current
supersedes: null
owner: game-qa
---

# C6/C7 통합 검토 — 초안 5렌즈 판정단 + 핸드오프 3렌즈 반박

## 0. 이 문서가 무엇이고 무엇이 아닌가

- **대상**: (a) `planning/game-draft-v1.md`(C6 통합 초안, 405행) + 그것이 인용하는 정본, (b) `handoff/{README,codex-unity-brief,verification-plan,asset-runbook}.md`(C7 핸드오프 4종).
- **입력**: 5렌즈 판정단(플레이어·퍼블리셔·엔지니어·서사·프로듀서, 발견 51건)과 3렌즈 반박(착수 가능성·모순·안전, 발견 45건). QA는 두 묶음을 **병합·중복 제거**하고 **근거를 직접 재측정**한 뒤 결함 id를 부여했다. **재측정으로 근거가 서지 않은 발견은 §4에서 기각한다** — 렌즈가 말했다는 사실만으로 결함이 되지 않는다.
- **아닌 것**: Unity 실행 0회 · 빌드 0줄 · 사람 플레이 표본 **n = 0** · 프레임/저장 캡처 0건 · 패드 실측 0건. 이 문서의 어떤 값도 **게임 측정치가 아니며 어떤 게이트도 PASS로 올리지 않는다.** repro는 전부 문서·데이터 대조다.
- 심각도 규약은 `qa/defect-register.md` 머리와 같다(S1 차단 · S2 같은 회차 수정 · S3 다음 회차 · S4 관찰).

### 0.1 이번 회차 재측정 명령 [OBSERVED 2026-09-10]

| # | 명령 | 결과 |
|---|---|---|
| X-1 | `git status --short` | 다른 세션 변경 다수(레인 폴더 대부분 미추적). QA는 `qa/` 밖을 쓰지 않았다 |
| X-2 | `node _workspace/current/planning/validate-campaign.mjs` | `{checks 47, pass 47, fail 0, verdict PASS}` · exit 0. 집계 `beats 33 · clues 73 · designMinutes 480 · observedMedianMinutes null · humanPlaytests [] · toolBeatCounts {circuit 10, reader 11, alignment 8, routing 3, corrosion 3, seal 7} · zoneBeatCounts {hub 15, gate 3, pump 6, dock 5, lowland 4} · proofRequiredBeats 15`. **sha·바이트는 검증기 출력이 소유한다 — 이 문서에 재기재하지 않는다(RFC-Q1)** |
| X-3 | `bash .claude/skills/game-ops-harness/scripts/freshness-check.sh` | `0 finding(s) across **103** markdown artifact(s)` · exit 0 (본 문서 생성 후 재실행 = **104**). 도구 스스로 "since 미지정 = 시점 신선도 미측정 · memory_sync 미검증"을 출력 |
| X-4 | `node .claude/skills/game-ops-harness/scripts/validate-preproduction.mjs` | `status FIX` · `checks 512 / passed 512` · errors **4건** = `qa/c6-review.md` · `production/cycles/c6-development.md` · `qa/c7-review.md` · `production/cycles/c7-development.md` 부재. **C5-F3이 지목한 c5 아티팩트 2건은 해소됐고 오류가 c6/c7로 옮겨갔다.** 본 문서 생성 후 재실행 = errors **3건**(`c6-review.md` 해소) |
| X-5 | `grep -rn "매체 경로" _workspace/current \| grep -v qa/` | 규칙 본문 사용 **0건**(잔존 5행은 전부 "폐기했다"는 선언·기록 문장). 렌더 HTML `grep -c` = **0** |
| X-6 | `grep -oE "기술 인수 테스트 [0-9]+개" steam-game-plan.html` | `기술 인수 테스트 27개` — 정본 §11 전수와 일치 |
| X-7 | `shasum -a 256` × 5 (systems 3 + presentation 2) | `game-ui-contract.meta.md` 표 3행, `steam-game-plan.meta.md` r5 2행 **전건 문자 일치** |
| X-8 | python: `beats[].subtasks[0]`의 구역 명사 ↔ `zoneId` 전수 대조 | **4비트 불일치** (§2 C6-F17) |
| X-9 | python: `proofRequired` 15건 중 `tools ∋ seal` | **3건**(`c1-b3`·`c4-b3`·`c6-b3`). T0 도구 합집합 = `circuit`·`reader` |
| X-10 | python: economics.json 입력으로 §5·§5-1 12개 값 독립 재계산 | **12/12 일치**(§5 Net 6값·손익분기 18값, §5-1 Net 3값·손익분기 9값 표본 전건) |
| X-11 | `grep -c '"en"' planning/campaign.json` | **0** |
| X-12 | `find _workspace/current -name "zones.json" -o -name "plates.json" -o -name "tools.json" -o -name "hints.json" -o -name "beats.json"` | **0건**. `campaign.json` 최상위 키 = `schemaVersion cycle designMinutes observedMedianMinutes humanPlaytests stages` |
| X-13 | `ls assets/generated/3d/` · `find unity/Unknown/Assets -type f \| wc -l` | `SM_Tool_*.glb` 6 + `hub-greybox.{blend,fbx,glb}`. **`SM_Tool_*.fbx` 0건**. Unity `Assets/` 파일 **0** |
| X-14 | `python3 -c` manifest.json 비-모듈 의존 | `["com.unity.multiplayer.center"]` — 브리프 필수 4종 **0/4 설치** |
| X-15 | `grep -c` decision-log의 RFC id | `RFC-C6-001 0 · RFC-C6-002 0 · RFC-Q3 0 · RFC-S2 0 · RFC-S3 0 · RFC-S5 0 · RFC-S6 0 · RFC-B6 0 · RFC-M3 0 · RFC-CX 0` |
| X-16 | 용어집 행 머리 대조 | 등재: `조위정합` `이중서명`. 미등재: `배선 추적` `판독` `배수 편성` `부식 시험` `판독대` `조위관측판` `성에선` |

---

## 1. 판정 요약

| 축 | 판정 | 근거 |
|---|---|---|
| C6 닫는 질문 — "한 문서로 게임을 설명·판단할 수 있는가" | **부분** | 서술·인용 규율은 높다(§6 강점). 그러나 연표·공개 상한·T0 착수 결정·가격·환불·법무가 빠져 있어 **판단**에는 초안 밖 문서가 계속 필요하다. 신규 S2 17건 |
| C7 닫는 질문 — "외부 실행자가 추가 질문 없이 착수할 수 있는가" | **아니오** | T0 퍼즐의 **인스턴스 데이터가 저장소에 없다**(X-12). DoD 4는 실행자가 캐논·수치를 발명해야 달성되고 `handoff/README.md` §2가 그것을 금지한다 → **C7-F1 (S1)** |
| 전체 | **SPEC-FIX** | 재작성이 아니다. C7-F1은 디렉터 판정 1건 + 데이터 저작 1건으로 닫히고, 나머지 S2는 전부 문장·표·필드 수준이다 |

**열린 S1 = 1건 (C7-F1).** C3·C4에서 유지되던 "열린 S1 0"이 이번 회차에 깨졌다. 사유는 회귀가 아니라 **새 산출물(핸드오프)이 처음으로 실행 가능성 축에서 측정됐기 때문**이다.

---

## 2. C6 결함 — 통합 초안 (`planning/game-draft-v1.md` 및 그것이 인용하는 정본)

| id | severity | lane | repro (파일·절) | evidence [OBSERVED 2026-09-10] | status |
|---|---|---|---|---|---|
| C6-F1 | S2 | planner + synopsis | `campaign.json` `t0-b1`~`t0-b3` 전문 검색 ↔ `game-draft-v1.md` §0·§3.1 ↔ `balance/puzzle-balance.md` T0 목표 | 33비트 전문에서 「청문」 최초 등장 = `c3-b4`, 「제출」 최초 등장 = `c5-b4`. `t0-b1.objective` = "판 #0을 이관 목록에 올릴지 결정", `t0-b3.completion` = 결손 구간 확정 + 남는 질문. **첫 30분에 "오늘 밤 무엇을 끝내야 하는가"(제출 문서 1건)를 말하는 문장이 없다.** 초안 §0·README는 그것을 핵심으로 내세운다 | open |
| C6-F2 | S2 | planner + product-manager + synopsis | `product/business-model.md` §1 ↔ `synopsis/scenes-and-dialogue.md` 6씬 ↔ `campaign.json` `c3-b4`·`c5-b4`·`c7-b4`·`e0-b1` | 상품 약속 = "내가 직접 손을 대서 **결론이 바뀌는** 추리". 데이터: `c7-b4.consequence` "세 선택은 같은 종결 씬 … 새 씬은 갈라지지 않는다", `c5-b4.consequence` "후일담 텍스트 두 쌍만 … 세 결말 접근, 이후 사건은 동일", `scenes-and-dialogue.md` 「분기(비확장)」 **6/6 씬**. 초안은 두 문서를 함께 인용하면서 이 모순을 조정하지 않는다. `c7-b4.recovery`의 "확정 직전 자동 백업으로 다른 선택을 즉시 다시 볼 수 있다"까지 더하면 결말은 **선택이 아니라 3장짜리 텍스트 메뉴**로 읽힌다 | open |
| C6-F3 | S2 | planner + balance | `campaign.json` `hints[0]` 33건 전수 ↔ `gdd.md` §6 · `interaction-rules.md` §4 1단 정의 ↔ 검증기 H-01 | 1단 정의 = "주는 것: 결정 대상·구역·도구 / **주지 않는 것: 어떤 자료인지**". 실제 1단이 인과·자료를 지정: `c2-b1` "해치가 물리는 것은 잠금이 아니라 **압력 차 때문이다**", `c1-b2` "소금은 열이 아니라 **습기로 푼다**", `c6-b2` "지금 지도는 오늘의 지도다. 그날의 지도가 아니다", `c6-b3` "**기계가 찍은 두 각인만** 보라", `c4-b3` "그 이름이 **서명할 수 있는 사람인지** 물어라". 검증기 H-01은 **빈 문자열만** 검사한다(`hints.length !== 3 \|\| !h.trim()`). 무진전 180초 자동 제안과 결합하면 막힌 플레이어의 첫 클릭이 곧 정답 통찰이다 | open |
| C6-F4 | S2 | planner + systems | `grep -rn "재개\|이어하기\|리캡\|recap\|resume"` on `gdd.md` · `interaction-rules.md` · `save-undo.md` | **0건.** 타깃 이용자는 480분을 주말 2~4회로 나눠 플레이하는데(`business-model.md` §1), 세션 복귀 시 확정 항목·열린 질문·되돌릴 수 있는 것을 보여주는 **재개 요약이 어디에도 스펙되지 않았다**. `gdd.md` §7 슬롯 카드는 장·조위 위상·플레이 시간만 표시. 단서 73 · 33비트 사건판을 하루 뒤 맨손으로 다시 읽어야 한다 | open |
| C6-F5 | S2 | balance + planner | python: `subtasks`에 「표」 또는 「칸」 포함 비트 계수 ↔ 검증기 `toolBeatCounts` | **21 / 33 비트**가 표·칸 채우기를 포함한다. 클라이맥스 C7(65분)은 `c7-b1` 3×3표 → `c7-b3` 아홉 칸 → `c7-b4` 예고 교차검증으로 이어져 손 조작은 `c7-b2` 하나. `routing`·`corrosion`은 각 **3비트**뿐. "도구 6개가 서로 다른 재미를 준다"를 뒷받침하는 조작 유형 분류가 밸런스 시트에 없다 | open |
| C6-F6 | S2 | product-manager + planner | `game-draft-v1.md` §8 ↔ `planning/market-decision.md` ↔ `product/business-model.md` §2 | §8 가격 행 = "B1 / B2 / B3" — **숫자 없음**. 초안 본문에 `market-decision.md` 인용 **0건**. 퍼블리셔가 요구하는 "가격·분량·비교작 대비 차별점 한 문장"이 초안 어디에도 없다. `market-decision.md`는 `status: draft`(c2)이며 그 사실도 표기되지 않았다 | open |
| C6-F7 | S2 | production-director | `grep -c` on `production/decision-log.md` (X-15) | 초안 §4.2·§11.2가 **RFC-C6-001·RFC-C6-002**를 개설된 것처럼 인용하나 decision-log에 **0건**. 같은 명령으로 **RFC-Q3·RFC-S2·RFC-S3·RFC-S5·RFC-S6·RFC-B6·RFC-M3·RFC-CX도 전부 0건** — 이 8건은 systems·balance·modeling·QA 레인 문서와 `handoff/README.md` §4가 "디렉터 판정 대기"로 인용하는 것들이다. CLAUDE.md §4 "파일이 바뀌지 않은 RFC는 일어나지 않은 것이다" 정면 위배. **핸드오프의 RFC 되묻기 절차(README §2 3단계 "decision-log 검색")가 구조적으로 빈 결과를 낸다** | open |
| C6-F8 | S2 | product-manager + concept + planner | `game-draft-v1.md` §7.2·§11.2·§11.3 ↔ 계약 Asset pipeline ↔ `product/steam-registration-guide.md` §5 ↔ `assets/generated/*/provenance.json` | 초안 §11 위험 표에 **생성형 AI 공개(Steam 콘텐츠 설문)와 라이선스 UNVERIFIED가 0행**이다. 계약은 2D를 "전부 GTI"로 정하고, provenance 전건이 `"license": "UNVERIFIED (…check backend ToS before commercial use)"`이며 영상 2건도 같다. 상점 캡슐·키아트 후보 자체가 상업 이용 미확인 생성물이다 | open |
| C6-F9 | S2 | production-director + systems + planner | `game-draft-v1.md` §6 ↔ `handoff/verification-plan.md` §1.3·§4 ↔ `production/production-estimate.meta.md` 범위 조정 규칙 | "본 생산 승인"의 조건이 **세 문서에 흩어져 있고 초안은 그중 하나(두 증거 요구)만 담는다**. 초안 본문에 `handoff` **0회**, "50% 초과 → 전체 생산 STOP · 도구 6→4" **0회**. 두 번째 증거인 **alignment 스파이크는 측정 프로토콜·판정선이 어느 파일에도 없다**(`verification-plan.md` §0이 alignment를 명시적 제외) | open |
| C6-F10 | S2 | planner + systems | `campaign.json` T0 3비트 `tools` ↔ `data-schemas/tools.md` §1 `hasCommit` ↔ `game-draft-v1.md` §6 인수 테스트 묶음 | T0 도구 합집합 = `circuit`·`reader`이고 두 도구 모두 **확정 없음**(tools.md §1). 그런데 초안 §6은 `T-15`~`T-20`(commit→SavePending→롤백)·`T-25`(확정 3방식 완주)·`T-27`(확정 발행 래치)을 **T0 인수 기준**으로 올렸다. **T0에서 발행되는 commit 명령 id가 어느 문서에도 없다.** 비트 완료 술어(`t0-b1`~`b3`를 무엇으로 닫는가)도 미정의 | open |
| C6-F11 | S2 | systems + production-director | `grep` on `game-draft-v1.md` ↔ `systems/architecture-contract.md` §1·§10·§12 | 초안 본문 문자열 계수: 「기준 하드웨어」 **0** · 「Input System」 **0** · 「URP」 **0** · 「asmdef」 **0**. 정본이 "T0 착수 첫 작업"으로 못박은 결정 4건(기준 PC 1대 고정 · `activeInputHandler: 0` → Input System 전환 · 렌더 파이프라인 · 어셈블리 분할 RFC-S2)이 초안 §6·§11.2 어디에도 없고, **성능 예산 절 자체가 없어** T0 사람 검증에 프레임·저장 인수 기준이 붙지 않는다 | open |
| C6-F12 | S2 | systems + planner | `game-draft-v1.md` §4.5·Appendix A ↔ `architecture-contract.md` §6 ↔ `system-specs/save-undo.md` SV-F6 | 초안은 "되돌림은 스펙상 상한 없음"만 적고 정본의 **로그 상한(50,000 엔트리 / 8 MB)과 초과 시 체크포인트 입도로 접힘(SV-F6)**을 빠뜨렸다. 플레이어에게 고지되는 계약("무제한")과 구현 계약("접힘")이 어긋난다. RAM 상주 정책(스냅샷 간격·브랜치 캐시 상한·되돌림 1스텝 재생 명령 수)은 어느 문서에도 없다 | open |
| C6-F13 | S2 | systems | `data-schemas/beats.md` §1 M1·M4 ↔ `architecture-contract.md` §3 ↔ `data-schemas/plates.md` §1 ↔ `game-draft-v1.md` §3 | 저작 원본 → 런타임 테이블 파이프라인이 정의되지 않았다: `beats.json`이 `campaign.json`에서 "생성된다"고만 적히고 **생성기·실행 시점·해시 대조 방식이 없다**(X-12: 그 파일들이 실재하지 않는다). 같은 개념에 이름 두 벌 — `plates.md` `mediaType` ↔ `beats.md` `sourceType` — 이라 임포터에 변환 계층이 생기고 이는 "변환 계층 0"(M2)과 모순 | open |
| C6-F14 | S2 | planner | `game-draft-v1.md` §4.1 층 B·§4.3 ↔ `interaction-rules.md` §1-1 | 초안·`gdd.md`: "`two-step` = **확정 버튼 → 프리뷰 패널의 확인**". 정본: "**프리뷰 `Space` → 초점을 확정 버튼으로 옮겨 한 번 누름**, 홀드 없음". 순서가 반대이며 초안 서술은 오히려 `confirm-dialog`와 같아진다. `gdd.md`는 스스로 "systems 쪽이 정본"이라 적고도 본문 2곳이 옛 순서 | open |
| C6-F15 | S2 | planner (정본: worldview) | `grep -n "H-1\|연표" game-draft-v1.md` = **0** ↔ `worldview/timeline.md` §2·§8 | 초안에 **저자 진실 연표·대조의 밤 캐논 시각(H-1:40 / H-1:24 / H-1:04 / H+0:12)·순서 앵커(간격 20분 > 오차폭 8분)가 한 줄도 없다.** 3장(65분)·6장(75분)의 정답 판정 기준이 이 앵커에 걸려 있으므로(timeline §8), 초안만으로는 2막 핵심 퍼즐이 무엇을 확정하는지 판단할 수 없다. 검증기 K-03·K-04는 데이터가 그 시각을 갖고 있음을 증명하나 **초안이 그것을 전달하지 않는다** | open |
| C6-F16 | S2 | planner + synopsis | `grep -n "RFC-P3-012\|공개 상한" game-draft-v1.md` = **0** ↔ `decision-log.md` RFC-P3-012 ↔ `timeline.md` §7 B01~B03 | 수직 슬라이스 §6에 **서사 공개 상한이 없다.** T0를 먼저 만드는 실행자가 허용(한도연=당직 주임·판 #0·결손 4시간 존재)과 금지(서명란 이름 '서린'은 `c4-b2`까지, 오해 "정전 때문" 유지)를 초안에서 알 수 없다. 검증기 K-05는 데이터를 지키고 있으나(`도연: t0-b1 / 한서린: c4-b2`) 규칙이 실행자에게 전달되지 않는다 | open |
| C6-F17 | S2 | planner + synopsis | X-8 전수 스캔: `subtasks[0]`의 구역 명사 ↔ `zoneId` ↔ 스테이지 `zoneIds` | **4비트 불일치** — (a) **스테이지 `zoneIds` 밖**: `c2-b4` `zoneId=gate`(스테이지 `[gate,pump]`)인데 본문 "당직실에서 … 부두사무소 대장 열람"(hub·dock), `c6-b4` `zoneId=pump`(스테이지 `[pump]`)인데 본문 "당직실에서 조사 종결 보고서". (b) **스테이지 안이지만 비트 `zoneId`와 다름**: `c1-b2` `hub` ↔ 본문 "제3수문 판독대", `c5-b2` `dock` ↔ 본문 "저지대 우선안". 검증기 Z-01/Z-02는 `zoneId ∈ stage.zoneIds`만 보므로 **본문은 검사되지 않는다**. 판정단이 지목한 2건에서 **4건으로 넓어졌고**, 판정단이 함께 지목한 `c4-b1`/`c4-b3` 서술 반전은 **live 데이터에서 해소돼 있다**(§5 C3-F29 재판정) | open |
| C6-F18 | S3 | production-director (README 소유 미정) | `README.md` L11 ↔ `worldview-bible.md` L26·L99 ↔ `synopsis/synopsis.md` §1 ↔ 초안 §0 | README L11 = "**폐국을 3주 앞둔** 조수기록국의 마지막 야간 당직". 캐논: 폐국 고지가 연표 T-0(**D-21**)이고 이 밤은 **D-1**. 초안·시놉시스는 "하루 앞둔"으로 맞다. **플레이어가 처음 읽는 문장이 시간 설정을 틀리게 전한다.** 같은 README L38이 `docs/media/verb-seal.jpg`(C4-F11 open S2, `style-guide.md` §10 위반 후보·재생성 1순위)를 게시 중 | open |
| C6-F19 | S3 | planner | 초안 §4.2 OPEN 블록·§11.2 #1 ↔ `interaction-rules.md` §2 제목 6개 ↔ `gdd.md` §4 ↔ `style-guide.md` §9 | 초안은 "`interaction-rules.md` §2는 회로 지도 / 판독기 / 조위정합 / **경로 구성** / **부식예산** / 이중서명"이라 적으나 실제 §2 제목은 `배선 추적 · 판독 · 조위정합 · 배수 편성 · 부식 시험 · 이중서명`으로 **gdd·style-guide와 6/6 일치**한다(2026-09-10 R6). 초안이 정본보다 뒤처져 UI 문자열 상태를 잘못 전한다. 잔여는 용어집 미등재 4건(§5 C4-F9) | open |
| C6-F20 | S3 | planner | 초안 §2.1 표 ↔ `worldview-bible.md` §3 표 | 표제가 "정본 문구(인용, **재작성 금지**)"인데 「플레이어 행동」·「실패와 회복」 두 열이 축약 재작성됐다: 법4에서 "**진행·결말 선택은 보존. 확정 직전 저장으로 선택 재시도 가능**" 탈락, 법6에서 "**세 결말 모두 본편에서 접근 가능**" 탈락, 법1에서 "**판정 보류 후 다른 매체 수집**" 탈락. 호명 문구 6/6은 문자 일치(정확) | open |
| C6-F21 | S3 | planner | 초안 §2.1 무대 문장 ↔ `worldview-bible.md` §2 "남지 않는 것" ↔ 검증기 K-01 | 초안은 "목소리·영상·사람 위치·의도"만 적고 **"사람이 무엇을 확인했는지"를 빠뜨렸다** — 이 항목이 RFC-P3-013의 전제이자 회귀 금지 문구(K-01)의 대상이다. 없으면 "누가 언제 확인했다"를 근거로 쓰는 회귀 서술을 초안이 막지 못한다 | open |
| C6-F22 | S3 | planner | 초안 §2.3 결말 표 ↔ `worldview-bible.md` §6 | 「무엇을 잃는가」 열 셀 문구가 §6에 없는 파생문인데 출처를 §6으로 달고 `[INFERENCE]` 표기가 없다. 동시에 §6이 명시한 갈래별 결과(A "주민 보상 근거는 가장 강해진다"·"구체 처분은 가상 기관 절차로만 표현", B "재화의 자동화 이관이 무사히 통과한다")를 떨어뜨렸다 | open |
| C6-F23 | S3 | production-director + planner | 초안 §9 ↔ `production/production-estimate.meta.md`(`status: draft`) · `production-estimate.json` | 초안 §9에 「개월·일정·납기·인력」 **0회**. 원본 meta의 "1명×월20일 ≈ 17.9개월 / 2명 병렬 ≈ 12.8개월, 인력 미확정"과 `measuredAssetReuseRate: null` · `externalCosts: null` · 근거 없는 `contingencyRate 0.25`가 초안에 없고, 원본이 `status: draft`라는 표기도 없다. **첫 사람 증거(T0)까지의 비용(SHARED + T0 인일)**도 없다 | open |
| C6-F24 | S3 | planner + product-manager | 초안 §11.3 13항목 | 미측정 목록이 플레이·성능·시장에 한정돼 **법무·라이선스·파이프라인·플랫폼**이 빠졌다: 생성물 상업 이용 UNVERIFIED · GTI "비공식 백엔드, 중단 가능" · Higgsfield 인증·크레딧 · Steam 배분율 `shareVerified:false` · KRW 최저 기준가 · Unity 6000.5.6f1 LTS 지위 · Steam Deck · 기준 하드웨어 미정 · 상표/동명 확인 | open |
| C6-F25 | S3 | planner + production-director | 초안 §6·§12 ↔ `systems/unity-implementation.md` frontmatter ↔ `ls handoff/` ↔ `ls production/cycles/` | (a) T0 정의의 정본이 `status: draft`이고 C4-F6(RFC-S3) 미결인데 §6 표에 "(C4/C5 검증 대기)" 표기가 없다 — 초안 §11.2 #6이 스스로 약속한 규칙 위반. (b) §12 경로 색인에 **`handoff/` 4파일이 없다**(초안 본문 `handoff` 0회). (c) `production/cycles/c6-development.md` **부재**(X-4가 오류로 출력) | open |
| C6-F26 | S3 | product-manager + planner + balance | 초안 전문 `grep -c 환불` = **0** ↔ `campaign.json` 누적 분 ↔ `business-model.md` §4 | 환불 2시간 룰과 도입부 길이의 관계가 초안에 없다. 검증기 값으로 역산하면 120분 지점 도달 스테이지가 페이스별로 다르다(설계 페이스 누적: T0 25 · C1 75 · C2 130) — **천천히 플레이하는 사람은 첫 반전 R1 회수(C2 종료) 이전에 환불 창이 닫힌다.** business-model §4는 "도입부 늘리기 금지"만 있고 반대 리스크는 어느 문서도 다루지 않는다 | open |
| C6-F27 | S3 | concept + presentation + product-manager | 초안 §7.2 ↔ `concept/generation-manifest.md` ↔ `product/steam-registration-guide.md` §7 ↔ `assets/generated/video/provenance.json` | 초안 §7.2가 "키아트 2 · 캡슐 2"를 보유 수량처럼 세지만 **상점 자산 준비도는 사실상 0**이다: 캡슐 2장 로고 미포함(가제 미승인) · 45장 중 29장이 요청 해상도와 다르고 크롭·리사이즈 공정 0건 · 트레일러 0(5초 프리비즈 2클립뿐) · 스크린샷 0(P2 규칙상 게임플레이만 허용, 빌드 0줄) | open |
| C6-F28 | S3 | product-manager | `grep -rn "상표" product/ production/ gdd.md deck-outline.md` ↔ `steam-registration-guide.md` §4·§6 | 상표·동명 검색 **영수증 0건**(규칙 문장 2건뿐), GRAC 자체등급분류 경로 K1~K5 **미수행**. 둘 다 Coming Soon(≥2주) + Direct 30일 대기의 크리티컬 패스인데 초안 §11.2 미해결 표에 소유자·기한 없이 빠져 있다. `consistency-audit.md` A29(open)와 같은 뿌리 | open |
| C6-F29 | S3 | planner | 초안 L21 선언 ↔ §8 표 | §0은 "본 문서는 숫자를 재기재하지 않는다"고 선언하면서 §8이 **DLC 2~3시간 / 5,900~7,900원 / 9,000원 / 40%는 재기재하고 본편 가격 3안 숫자만 뺐다.** 초안만 읽으면 DLC 가격은 알고 본편 가격은 모르는 비대칭 | open |
| C6-F30 | S3 | planner | 초안 §6 묶음 표 계수 ↔ `unity-implementation.md` §11 `grep -oE "T-[0-9]{2}" \| sort -u \| wc -l` | 정본 **27**, 초안 묶음 합계 **26** — **`T-12`(로컬라이제이션 미해결 키 0건) 누락**. 추가로 `T-11`(임포트 검증기 픽스처 5종)이 「근거 독립성」에, `T-13`(되돌림 포인터 재적용 → 원래 해시)이 「저장 내구」에 잘못 묶였다 | open |
| C6-F31 | S3 | systems | `data-schemas/save.md` §0·§6 ↔ `unity-implementation.md` §7 ↔ `interaction-rules.md` §6 ↔ 초안 §4.5 ↔ `handoff/codex-unity-brief.md` §⑥ | 저장 파일 명명이 **세 벌**: `slot{n}.json`/`slot{n}.bak`/`slot{n}.v{old}.bak` ↔ `save.json`/`save.bak`/`checkpoint.pre-commit.json` ↔ 초안 "수동 3슬롯 + 자동 1슬롯". 복구 3단계가 슬롯별인지 전역인지, `checkpoint.pre-commit.json`이 자동 슬롯인지 별도 파일인지 구현자가 정할 수 없다. `save.md` 경로가 Windows 전용(`%USERPROFILE%/AppData/LocalLow/…`)이라 macOS 개발 환경의 T-08/T-15/T-16 픽스처 경로가 미정의 | open |
| C6-F32 | S3 | systems | `interaction-rules.md` §5 ↔ `save-undo.md` SV-R8·§9 ↔ `architecture-contract.md` §10 ↔ 초안 §4.1 ②·§4.5 | **확정 1회에 디스크 쓰기가 몇 번인지** 어느 문서도 명시하지 않는다. 성능 표는 "`Commit` + 체크포인트 ≤200 ms"와 "세이브 쓰기(fsync 포함) ≤200 ms"를 따로 두고, 자동 저장 프레임 스파이크 ≤4 ms(워커)는 `save-undo.md` §9에만 있어 T0 측정 항목이 갈린다 | open |
| C6-F33 | S3 | economy → worldview / synopsis | 초안 §5 계약 2 ↔ `economy/currency-map.md` L141 ↔ `grep -rn "보존 등급" worldview/ synopsis/` = **0** ↔ `consistency-audit.md` A25(open) | 초안 §5가 에필로그에 "**보존 등급 문장 1줄**"이라는 변수를 도입하지만 세계관·시놉시스 어디에도 그 문장이 없다. 바이블 §6 에필로그는 4항목(청문 결과·4구역 최종 상태·다음 근무·판 #0)뿐이고, A25(에필로그 조합 폭발)가 이 축을 세지 않는다 | open-rfc |
| C6-F34 | S3 | qa | `qa/gate-measurements.md` L32 ↔ `decision-log.md` C3-F25 판정 ↔ `worldview/timeline.md` §7-0·§9 ↔ `consistency-audit.md` §1 | `#g1` measured 값이 스테일이었다 — "open-rfc 2건 (F25 · F30)"인데 **C3-F25는 디렉터가 closed**(H-1:40 캐논 유지)했고 **C3-F30은 판정 완료**(campaign id 단일 인용 키)다. 감사(증거 소스)는 `violation 0 · open 3(A25·A29·A42)`이며 **C3-F29가 감사에 등재돼 있지 않아** 게이트의 유일 violation을 증거 문서가 싣지 않는다. **QA 문서가 스테일 측인 세 번째 회차** — 원인은 같다(값을 고정 문자열로 적었다) | **closed** (본 문서 §7에서 재측정 교체) |
| C6-F35 | S4 | balance + systems | `balance/balance-sheet.md` §6.2 ↔ `gdd.md` §8 접근성 목록 | 무진전 제안 임계 180초 · 쿨다운 180초 · **세션당 상한 `null`**(미정)인데 **제안 표시를 끄는 옵션이 스펙에 없다.** 타이머 정지 조건에 "사건판을 보며 생각하는 상태"가 없어 중앙값 16분 비트에서 3분마다 개입한다. "생각하는 시간을 실패로 세지 않는다"는 선언과 어긋난다 | open |
| C6-F36 | S4 | production-director + systems | 초안 §6 ↔ `interaction-rules.md` §2.4 ↔ 검증기 `toolBeatCounts` | 본 생산 게이트는 T0(`circuit`+`reader`)와 alignment 스파이크만 사람 검증을 요구한다. 그러나 기둥 1을 체감시키는 **유일한 세계 상태 변경 동사 `routing`**(§2.4 "성공 시 구역 상태가 바뀌고 체크포인트가 생긴다")과 그 짝 `corrosion`은 **어떤 사람 검증도 없이** 본 생산에 들어간다. 초안 스스로 "이 넷의 조작감은 T0로 검증되지 않는다"고 적는다 | open-rfc |
| C6-F37 | S4 | planner | 초안 §2.2 ↔ `timeline.md` §5 ↔ `worldview-bible.md` §5 | "아무도 전체를 모른다" 한 문장만 있고 **인물별 인지 범위 5행 표와 세 세력 표가 인용되지 않아** 전지적 화자 금지를 이 문서로는 검사할 수 없다(누가 무엇을 모르는지가 없으면 대사 검토 기준이 없다) | open |
| C6-F38 | S4 | product-manager + production-director | 초안 §6·§8 ↔ `product/assumption-tests.md` L26 ↔ `steam-registration-guide.md` §7 P5 | **데모 정책 미정**이 초안에 언급조차 없다. `assumption-tests.md`의 퍼널은 "위시리스트→**데모 시작**→기준 비트 완료→구매"로 데모를 전제하고, 데모는 별도 App ID + 전용 트레일러 + 최소 5스크린샷 비용을 부른다. 위시리스트 목표는 `null` | open |
| C6-F39 | S4 | production-director + synopsis | `production-estimate.json` `externalCosts: null` ↔ `planning/content-matrix.md` 단어 집계 0건 ↔ 초안 §4.6 | 한국어/영어 동시 출시를 약속하면서 **텍스트 분량(대사·힌트 99단·UI 문자열)의 자수 산정이 0건**이고 견적 `externalCosts: null`이다. Coming Soon 페이지부터 영문 카피가 필요하므로 현지화는 상점 공개 **선행** 비용이다. C7-F5(EN 원천 0건)와 같은 뿌리 | open |
| C6-F40 | S4 | planner | 초안 Appendix A 전문 | 영문 요약이 Unity 구현자 대상인데 **미결 RFC·리스크 언급 0건**이고 "Undo has no spec-level ceiling"이 C6-F12와 같은 누락을 반복한다. 구현자가 문서만 읽고 착수 가능하다고 오독할 수 있다 | open |
| C6-F41 | S4 | product-manager + planner | 초안 §9 ↔ `product/economics.json` ↔ `business-model.md` §5-1 | (제안) 초안이 §9 인건비 1억 770만원과 회수 가능성을 한 문서에서 대조할 수 없다 — 가격 후보 값과 손익분기 역산이 §8·§9에 없다. 인건비 회수 판매량 역산 한 줄이 있으면 판단이 닫힌다. **선택 사항** | open |

### 2.1 C6 집계

| severity | 신규 | closed | open | open-rfc |
|---|---:|---:|---:|---:|
| S1 | 0 | 0 | **0** | 0 |
| S2 | 17 | 0 | 17 | 0 |
| S3 | 17 | 1 (F34) | 15 | 1 (F33) |
| S4 | 7 | 0 | 6 | 1 (F36) |
| **합계** | **41** | **1** | **38** | **2** |

---

## 3. C7 결함 — Unity 핸드오프 (`handoff/` 4종)

| id | severity | lane | repro (파일·절) | evidence [OBSERVED 2026-09-10] | status |
|---|---|---|---|---|---|
| C7-F1 | **S1** | systems + planner + production-director | X-12 · `codex-unity-brief.md` §④-1·§⑤-3·DoD 4 ↔ `handoff/README.md` §2 | **T0 퍼즐을 성립시키는 인스턴스 데이터가 저장소에 없다.** `find`로 `zones.json`·`plates.json`·`tools.json`·`hints.json`·`beats.json` **0건**, `campaign.json` 최상위 키에 그 어느 것도 없다. `plate-standard-hub`는 단서 `originId` 문자열로만 존재하고 4분 `segments`가 없으며, `hub`의 `viewNodes`/`cameraPose`/`sensorCoverage`/`systemIds`, `circuit` 3점 앵커, `ToolAsset` 6종 필드값도 없다. 브리프 §④-1은 저작 주체만 적고 **누가 언제 만드는지·실행자가 만들어도 되는지 말하지 않는다.** DoD 4("`t0-b1`→`t0-b3` 처음부터 끝까지 플레이 가능")는 실행자가 구역 내용·캐논·수치를 발명해야 달성되고 **README §2가 정확히 그것을 금지한다** → 착수 즉시 blocking RFC | open |
| C7-F2 | S2 | systems | X-14 ↔ `codex-unity-brief.md` §③-1·§⑩-2·DoD #2·N-10 | 필수 패키지 4종(inputsystem·ugui·localization·test-framework)이 manifest에 **0/4**인데, §③-1은 "버전을 발명하지 않는다"면서 **추가 절차를 정의하지 않는다**(manifest.json은 버전 문자열 필수). §⑩-2 명령 #1은 프로젝트를 열 뿐이고 #2/#3의 `-runTests`는 test-framework 없이 실행되지 않는다. 부가: `com.unity.localization`이 끌어오는 `com.unity.addressables`를 N-10이 "추가 전 RFC 필요"로 두어 **필수 패키지 설치 자체가 금지 위반**이 된다 | open |
| C7-F3 | S2 | modeling + systems | X-13 ↔ `codex-unity-brief.md` §⑧-3 ↔ `asset-runbook.md` §3.3·§6-5 | 브리프는 `hub-greybox.glb`·`SM_Tool_*.glb` **GLB 임포트**를 지시하지만 런북 §3.3은 "[INFERENCE] Unity는 `.glb`를 기본 임포트하지 않는다 → 승격 포맷 **FBX 단일**"이라 적고 §6-5를 열린 RFC로 남긴다. 실제로 **`SM_Tool_*.fbx`는 0건**이고 FBX는 `hub-greybox` 하나뿐. GLB 임포트에는 추가 패키지가 필요해 N-10과 충돌 → **두 핸드오프 문서 어느 경로로도 T0 도구 프롭을 RFC 없이 임포트할 수 없다** | open |
| C7-F4 | S2 | production-director + systems | `handoff/README.md` §1 우선순위 ↔ §4 ↔ `codex-unity-brief.md` §③ ↔ `unity-implementation.md` §2 ↔ `architecture-contract.md` §2·§3 | README §1은 "브리프와 어긋나면 `unity-implementation.md`·`interaction-rules.md`가 이긴다"고 하고 §4는 "브리프 §③ **7분할**로 시작"을 지시한다. `unity-implementation.md` §2는 **5분할**이며 모듈명도 다르다(`Tide.Sim` ↔ `Tide.Domain`). 규칙을 문자대로 따르면 **첫 asmdef부터 되물어야 한다.** 부가 (a) 우선순위 규칙이 `status: draft`이자 QA 승격차단 상태인 `unity-implementation.md`를 `status: current`인 `save.md`·`architecture-contract.md` **위에** 세워 죽은 필드(`dayIndex`)가 세이브에 되살아날 경로를 만든다. (b) `architecture-contract.md` §2·§3의 "Tests는 Sim만 참조"는 EditMode 테스트가 `Tide.Data`를 필요로 하므로 성립 불가. RFC-S2는 decision-log **0건**(X-15) | open |
| C7-F5 | S2 | planner + systems + worldview | X-11 ↔ `codex-unity-brief.md` I-9·DoD #7·T-12 ↔ `handoff/README.md` §2·§4 | `campaign.json`에 EN 문자열 **0건**이고 로컬라이즈 테이블도 없다. I-9("모든 표시 문자열이 KO/EN 양쪽 키를 가진다")와 T-12("미해결 키 0건")가 **fail-closed**이므로 임포트는 필연적으로 실패하고 N-9(경고 통과 금지)로 빌드가 멈춘다. 게다가 T0 두 도구의 **KO 표시명조차 용어집 미등재**(X-16, RFC-S6 미판정)이고 README §2는 UI 문자열을 실행자가 정하지 못하게 한다 → **DoD 4·7 도달 불가**. 부가: `campaign.json` `hints`는 KO 리터럴 `string[3]`이라 `hints.md` 스키마 필드(`textKey`·`mentionsMediaTypes`·`forwardLeakBeatIds`·`revealsValues`)를 파생할 수 없어 H-I5/H-I6 검사가 성립하지 않는다 | open |
| C7-F6 | S2 | systems | `codex-unity-brief.md` §③ 트리 ↔ §④-1 매핑표 ↔ §③ L103 ↔ `asset-runbook.md` §3.2 | 폴더 레이아웃이 브리프 **안에서** 자기모순: §③ 트리는 `Data/Tables/`에 `zones.json`·`plates.json`·`tools.json`을 두는데 §④-1은 그 셋을 ScriptableObject(`Data/Authoring/*.asset`)로 정하고 JSON은 `beats`/`hints`만이다. 별도로 §③ L103 "`Assets/_Project/` 밖에는 서드파티·패키지 샘플만"이 런북 §3.2의 승격 경로 `unity/Unknown/Assets/Art/**`와 충돌 → 그레이박스 임포트 위치가 두 갈래 | open |
| C7-F7 | S2 | systems | `grep -n "commitIdempotencyKey\|payload" data-schemas/save.md` ↔ `architecture-contract.md` §6 ↔ `unity-implementation.md` §7·T-17·T-19 ↔ `codex-unity-brief.md` SP-4·§⑥-5 | `save.md`의 `CommandEntry`는 `seq · branchId · parentSeq · commandId · **payloadHash** · committed`뿐 — **페이로드 자체를 저장하지 않는다.** 그러면 "되돌림 = 명령 로그 재생"·"로드 = 최신 스냅샷 이후 재생"·T-13(포인터 0까지 내린 뒤 재적용 → 원래 해시)·B-A2(같은 명령열 재생 시 `stateHash` 동일)를 구현할 수 없다. 동시에 T-17/T-19가 요구하는 **`commitIdempotencyKey`가 `save.md`·`save-undo.md`·`architecture-contract.md`·브리프 §⑥-5 어디에도 없다**(`unity-implementation.md` §7 draft JSON에만 존재). 부가: 같은 레인의 `unity-implementation.md` §3은 이벤트 소싱(`EventLog(seq, evtId, payload, causeCommandId)`)을 적어 **명령 소싱/이벤트 소싱이 갈린다**. README §2가 저장 필드 추가를 "반드시 되묻는 것"으로 두므로 **DoD 6이 RFC 없이 충족 불가** | open |
| C7-F8 | S2 | systems + planner | `codex-unity-brief.md` §⑤-3·DoD 6·8 ↔ `data-schemas/tools.md` §1 ↔ `campaign.json` T0 (X-9) | T0 두 도구 모두 `hasCommit = 아니오`이고 브리프도 "T0에서 확정되는 것은 없다"고 적으면서, 같은 문서가 **`ReadOriginal`을 "확정 흐름(two-step)"으로 발행**하고 DoD 6(T-15~T-20)·DoD 8(T-25·T-27)이 확정 테스트를 필수로 요구한다. 또 브리프에 `t0-b1`~`b3`의 **완료 술어가 0행**이라 DoD 4("끝까지 플레이 가능")와 T-24("퍼즐 종료로 판정")의 종료 상태가 정의되지 않는다. C6-F10과 짝 | open |
| C7-F9 | S2 | systems | `codex-unity-brief.md` T-26 ↔ `unity-implementation.md` §11 ↔ `interaction-rules.md` §1-3.2 ↔ `game-ui-contract.json` `matrix[12]` | T-26 문안("도구 패널이 열린 동안 `X`는 프리뷰만, `Y`는 해제만")이 **T0 두 도구에서 성립하지 않는다**: §1-3.2 전수표에서 `circuit`의 `X` = 구획 접기, `reader`의 `X` = 재생, `Y`는 `reader` = 인용 고정 / `circuit` = **없음**. 정본 세 곳(브리프·§11·`matrix[12]`)이 같은 틀린 문안을 공유한다 | open |
| C7-F10 | S2 | systems | `interaction-rules.md` §1-3.1 마지막 문단 ↔ 같은 파일 §1-3.2 전수표 | **한 파일 안 자기모순**: §1-3.1이 "오버레이 3진입점(`I`·`H`·`F1`)은 표면과 무관하게 **같은 일**을 한다 — 이것이 우선순위의 유일한 예외"라 하고, 두 문단 아래 §1-3.2 전수표는 `I` = `ToolPanel(circuit)`에서 **근거 유효성 조회**, `H` = `ToolPanel(alignment)`에서 **선후 판정 조회**라 적는다. 반박 렌즈는 `I` 1키를 지목했으나 **재측정 결과 `I`·`H` 2키**다. §0-11(명시되지 않은 겸용은 결함)이 정의한 상태이며 T-24 키보드 단독 경로가 어느 의미를 타는지 불명 | open |
| C7-F11 | S2 | systems | `codex-unity-brief.md` §⑨-2 ↔ `ops/telemetry-contract.md` ↔ `data-schemas/tools.md` T-I6 | 브리프가 텔레메트리 계약 키로 제시한 **7종(`circuit_open_count`·`read_count_total`·`auto_copy_created`·`citation_count`·`indeterminate_shown`·`reader_time_min`·`alt_path_offered`) 전건이 계약 파일에 0건**이다(정의는 `wiring-trace.md`·`plate-readout.md` §6에만). `command_count`(브리프 U-8 · `verification-plan.md` §2.2)는 **어디에도 정의가 없다**. `tools.md` T-I6("`telemetryKeys`의 모든 키가 텔레메트리 계약에 정의됨")이 fail-closed이므로 **T0 도구 2종 임포트가 실패한다** — DoD 9와 I-계열 fail-closed가 서로를 막는다. 반대로 `verification-plan.md` §1.3 H-1 판정의 핵심 키 `first_valid_action`은 계약에 있으나 **브리프에 0건** | open |
| C7-F12 | S2 | production-director + modeling | `asset-runbook.md` L91·L175·L357 ↔ `handoff/README.md` §3 표 | 런북이 실행자(Codex)에게 **`production/decision-log.md`에 크레딧 영수증·승격 감사 RFC 블록을 직접 쓰라**고 3곳에서 지시한다. 같은 폴더의 README §3은 그 파일을 "디렉터 소유 — 실행자 쓰지 않는다 · RFC는 `messages/`로"라 명시한다. **한 폴더 안에서 쓰기 권한이 정반대** | open |
| C7-F13 | S2 | modeling | `asset-runbook.md` §1.1·§4.4·§1.2 ↔ `scripts/gen-2d.sh` ↔ `git ls-files assets/generated \| wc -l` | `FORCE=1` 경로가 기존 PNG를 덮어쓰고 provenance 항목을 **같은 id로 교체(이전 항목 삭제)**한다. `stage_90_save()`도 `hub-greybox.blend`를 덮어쓴다. `assets/generated/`는 git 미추적(0파일)이라 **이전 산출물은 복구 불가** → CLAUDE.md §2 "삭제는 없다 · 이전 작업은 항상 참조 가능" 위반을 실행자에게 지시한다 | open |
| C7-F14 | S2 | systems + worldview | `grep -rl "센서가 닿지 않는 구획" .` · `grep -rn OUT_OF_COVERAGE .` ↔ `handoff/README.md` §2·§4 ↔ DoD #5 | 브리프 §⑤-3의 「플레이어가 보는 문장(KO 초안)」 7건과 `ReasonCode` 문자열이 **저장소 어디에도 없는 발명 문자열**이다(소유 spec의 W-F/P-F 표는 동작만 정의). README §2는 UI 표시 문자열을 "반드시 되물어야 하는 것"으로, RFC-S6은 "KO 값 확정은 용어집 등재 후"로 못박는데 **DoD #5가 "실패 문장을 그대로 구현"을 요구**한다 → 실행자가 미승인 문자열을 로컬라이즈 값으로 굽게 유도한다 | open |
| C7-F15 | S3 | systems | `handoff/README.md` §2 ↔ `codex-unity-brief.md` §③-1·DoD #2 | README §2는 「패키지 추가」를 반드시 되물어야 할 항목으로 적었는데 브리프 DoD #2는 필수 4종 추가 + `multiplayer.center` 제거를 RFC 없이 수행하라고 한다. **두 문서 모두 systems 소유**이며 실행자는 어느 쪽을 따를지 알 수 없다(`architecture-contract.md` §4·`unity-implementation.md` §1이 이미 같은 4종을 필수로 적으므로 사전 승인으로 인용 가능) | open |
| C7-F16 | S3 | production-director + modeling | `asset-runbook.md` L93 ↔ CLAUDE.md §10 ↔ 계약 Release safety | 런북이 Higgsfield 크레딧 소모(유료 플랜)를 **"디렉터 승인"**으로 게이트한다. 디렉터는 에이전트이고, 계약·CLAUDE.md §10은 유료 도구·비용 결제를 "이번 기획 요청만으로 실행하지 않는다 / 별도 승인"으로 둔다 → **에이전트 간 승인으로 사용자 비용이 발생하는 경로**. 기존 25.0 credits 소모는 사용자 2차 요청을 근거로 기록돼 있어 선례와도 어긋난다 | open |
| C7-F17 | S3 | systems | `grep "buildTarget\|BuildPipeline\|-build"` on `codex-unity-brief.md` = **0** ↔ DoD 1 · T-B1 | DoD 1이 T-B1(`Tide.Sim`이 UnityEngine 참조 시 컴파일 실패)을 "배치 빌드"로 검증하라 하지만 §⑩-2에 **빌드 명령·빌드 타깃·빌드 메서드가 없다.** 대상 플랫폼은 Windows 우선인데 호스트는 macOS이며 Windows Build Support 모듈 설치 여부 미확인 | open |
| C7-F18 | S3 | systems | `grep "actionMap\|control scheme\|액션 맵"` on brief·`interaction-rules.md` = **0** ↔ DoD 3 | §⑦-1 바인딩표가 키만 나열하고 `Actions.inputactions`의 **액션 맵 이름·액션 이름·컨트롤 스킴 이름을 정하지 않아** T-24~T-27이 참조할 식별자가 없다. DoD 3("재매핑이 실제로 동작한다")은 테스트 id·측정 기준이 없어 판정 불가 | open |
| C7-F19 | S3 | systems | `verification-plan.md` §2.2 ↔ `codex-unity-brief.md` §③ Tests 트리 | 성능 캡처 명령의 `Tide.Tests.Perf.CaptureCli`가 §③의 어느 asmdef(`Tide.Tests.Sim`/`Tide.Tests.Play`)에도 속하지 않고, `-batchmode`(비-`nographics`) 에디터 실행은 **플레이어 빌드 프레임타임이 아니며**, `-out` 경로의 `{hw_profile_id}`는 리터럴 플레이스홀더다. 기준기 미정(PRE-1)이라 판정에 쓰이지 않는다고 명시돼 있어 **착수는 막지 않는다** | open |
| C7-F20 | S3 | systems | `codex-unity-brief.md` L222 U-3 ↔ `ls` | 인용 경로 `prototype/prototype.meta.md` 는 존재하지 않는다(실제 `systems/prototype/prototype.meta.md`). **인용 25건 중 유일한 경로 오류** | open |
| C7-F21 | S3 | systems + economy + balance | `data-schemas/tools.md` §4 · `plate-readout.md` L92 ↔ `data-schemas/plates.md` §1 · `plate-readout.md` P-R2 · `economy/reward-bands.md` ↔ 브리프 §④-1 | `readBudget`의 소유 레인이 **둘**(balance / economy)로 갈리고 브리프는 `[balance]`로 인용한다. CLAUDE.md §4 "공유 진실 파일은 소유자만 편집" 규칙상 **실행자가 RFC를 보낼 레인이 결정되지 않는다** | open |
| C7-F22 | S3 | production-director | `handoff/README.md` §1 3.5행 ↔ `asset-runbook.md` frontmatter | README §1이 `asset-runbook.md`를 "에셋 작업 전 **필독**"으로 지정하지만 그 파일은 `status: draft`이며 스스로 §6-5(FBX 단일화)·초상 슬롯 비율 등 미결을 연다. **draft를 착수 전제로 두면 실행자가 판정 전 문서를 따르게 된다** | open |
| C7-F23 | S3 | systems + production-director | `handoff/README.md` §3 쓰기 표 ↔ `asset-runbook.md` §1.1·§1.5·§4.4·§7 | 런북은 실행자에게 `assets/generated/**`·`docs/media/` 쓰기(재생성·GIF 조립·provenance 갱신)를 지시하지만 README §3 쓰기 표에는 `unity/Unknown/**`·`systems/tech-verification/`·`messages/`만 있다. `assets/` 소유자는 RFC-M3로 미정(X-15: decision-log 0건) | open |
| C7-F24 | S3 | systems | `codex-unity-brief.md` S-2 ↔ §⑤-5 ↔ §⑨ ↔ `system-specs/hint-system.md` §2 | `idleSeconds ≥ 180`과 `afk_gap` 60초는 **실시간 카운터**인데 브리프가 계층을 지정하지 않는다. `hint-system.md`가 `idleSeconds`를 힌트 상태기계의 상태 변수로 두므로 실행자가 이를 `Tide.Sim`에 넣으면 S-2/R-3("Sim에 시간이 들어가지 않는다")·`noEngineReferences`와 즉시 충돌한다 | open |
| C7-F25 | S3 | systems | `codex-unity-brief.md` T-IMP-1 ↔ 같은 문서 L146(RFC-Q1 준수 선언) | T-IMP-1이 기대 집계를 **고정 숫자**(비트 33 · 단서 73 · `proofRequired` 15 · 독립쌍 15/15)로 테스트에 박도록 요구한다. 같은 문서가 "해시·바이트 수를 옮겨 적지 않는다(RFC-Q1)"고 선언해 자기모순이며, planner가 `campaign.json`을 고치면 테스트가 깨져 **N-9(경고 통과 금지) 우회 유혹**을 만든다. 동치 비교(`임포터 집계 == 검증기 출력 집계`)로 정의하면 닫힌다 | open |
| C7-F26 | S3 | systems + planner | `codex-unity-brief.md` §④-1 L133 ↔ `handoff/README.md` §3 ↔ `architecture-contract.md` RFC-S4 | `Data/Tables/beats.json`을 `planning/campaign.json`의 **복사본**으로 두라고 한다 → 진실이 둘이 되며, README §3의 "임포터는 읽기만"과 RFC-S4(해시 드리프트 이력)에 비추어 **드리프트가 재발한다**. C6-F13과 같은 뿌리 | open |
| C7-F27 | S4 | systems | `data-schemas/hints.md` §3 ↔ `system-specs/hint-system.md` H-F5 ↔ 브리프 §⑤-5 ↔ `interaction-rules.md` §5-4 ↔ `save.md` | `offerMaxPerSession`이 `[TARGET 미정]`이고 H-F5가 "세션당 상한"을 요구하는데 브리프 §⑤-5는 언급이 없다 → 실행자가 값을 고르면 N-3 위반. 부가: 체크포인트 라벨이 `interaction-rules.md` §5-4 "장·**일차**·조위 위상"(제거된 `dayIndex` 전제) ↔ `save.md` "장·조위 위상·플레이 시간"으로 갈린다 | open |
| C7-F28 | S4 | systems | `codex-unity-brief.md` L498·L511 ↔ `architecture-contract.md` §9 ↔ `telemetry-contract.md` §1 | §⑨-2가 `design_budget_min = 480 · design_fast_min = 322 · design_deliberate_min = 673`을 "T0에서 수집할 키"로 열거해 실행자가 세 숫자를 코드 리터럴로 박게 유도한다. **완화 사유**: 같은 표가 이를 "문서 상수"로 라벨하고 L511이 "예상 플레이 시간·분포·신뢰구간으로 부르지 않는다"를 명시한다 → S4 | open |
| C7-F29 | S4 | production-director | `codex-unity-brief.md` §③ `Tide.*` ↔ `worldview/glossary.md` L22 ↔ 계약 G8 | asmdef·네임스페이스 접두어 `Tide.*`가 가제("Tide Records Bureau")에서 파생됐고 어셈블리 DLL 이름은 빌드에 실린다. 원문 "TIDE ARCHIVE"가 아니므로 **위반은 아니지만 판정 기록이 없어** 후일 대량 개명 위험이 남는다. `productName = Unknown` 확인 | open-rfc |
| C7-F30 | S4 | systems | `codex-unity-brief.md` L672~L678 ↔ CLAUDE.md §10 ↔ `scripts/mex-agent-bin.sh` | §⑪-3이 실행자에게 맨 `mex graph scope` · `mex check` · `mex log`를 실행하게 한다. CLAUDE.md §10은 "정체를 검증한 mex-agent만 실행"을 요구하고 저장소에 래퍼가 있다. **완화**: 같은 블록이 "도구가 없거나 정체 불명이면 `[SKIPPED: 사유]`"를 이미 적었다 → S4이나 래퍼 경로 명시가 맞다 | open |
| C7-F31 | S4 | planner | `verification-plan.md` §1.1 유형 ⑤·§1.4 ↔ `telemetry-contract.md` §0 | 참가자 「접근성 요구(키보드 전용·홀드 곤란·색각 이상·자막 필요)」 유형을 기록하게 하는데 이는 **민감 속성**이다. 계약 §0은 식별자 저장만 금지하고 민감 범주 취급을 정하지 않는다. 요구: 유형은 QA 보고서에 익명 ID로만, `telemetry/*.jsonl`에는 기록 금지 + 동의문에 명시 | open |
| C7-F32 | S4 | systems | `codex-unity-brief.md` §⑩-2 ↔ `ls -R systems/tech-verification` | 실행 명령이 `$OUT/logs`·`$OUT/results` 디렉터리 존재를 전제하나 그 디렉터리가 없고 `mkdir` **0건**. `<repo>` 치환도 미설명. **그 외 Unity 플래그와 바이너리 경로는 실재 확인** | open |

### 3.1 C7 집계

| severity | 신규 | closed | open | open-rfc |
|---|---:|---:|---:|---:|
| S1 | 1 | 0 | **1** (F1) | 0 |
| S2 | 13 | 0 | 13 | 0 |
| S3 | 12 | 0 | 12 | 0 |
| S4 | 6 | 0 | 5 | 1 (F29) |
| **합계** | **32** | **0** | **31** | **1** |

---

## 4. 기각 — 재측정으로 근거가 서지 않은 발견

| 원 발견 | 기각 사유 [OBSERVED 2026-09-10] |
|---|---|
| **J-engineer-1 (S1)** "`t0-b3`가 `proofRequired`인데 T0에 `seal`이 없다 → 규칙상 필수 확정은 `seal`에서만" | **전제 기각.** `data-schemas/beats.md` §5·§6이 `proofRequired`를 "**독립 쌍(루트 `originId` 상이 AND `sourceType` 상이)이 데이터에 존재해야 한다**"(불변식 B-I18 · 검증기 C-07)로 정의한다. 확정 명령 요구가 아니다. 전수 재측정(X-9): `proofRequired` 15건 중 `tools ∋ seal`은 **3건**(`c1-b3`·`c4-b3`·`c6-b3`)뿐 — 12건이 `seal` 없이 `proofRequired`이므로 "`proofRequired` ⇒ `seal`"은 데이터·스키마 어느 쪽에서도 규칙이 아니다. C-07은 15/15 PASS. **잔여 결함만 승계**: T0에 확정 명령이 없는데 초안 §6·브리프 DoD가 확정 테스트를 T0 인수로 올린 것 → C6-F10 · C7-F8 (S2). 등급도 S1 → S2 |
| **J-engineer-2 (S1)** "명령 로그에 페이로드가 없어 되돌림 재생 불가" | **사실 확인, 등급만 하향.** `save.md` §3 `CommandEntry`에 `payload` 없음은 재현됐다. 그러나 이는 **미출시 스키마의 미완**이지 동작 중인 계약의 파손이 아니고, 개명 금지 목록 진입 전이라 비용 0으로 닫힌다 → **S2**로 C7-F7에 병합(`commitIdempotencyKey` 부재와 같은 편집) |
| **J-narrative-3** "`c4-b1`/`c4-b3` 서술 반전이 G1 유일 violation의 실체" | **부분 기각.** live 데이터 재측정: `c4-b1` `zoneId=lowland` ↔ 본문 "저지대 주민회 사무실", `c4-b3` `zoneId=hub` ↔ 본문 "당직실 봉인대" — **일치한다.** C3-F29의 이 범위는 해소됐다(§5). 남은 실체는 **본문 장소 ≠ `zoneId`이며, 재측정으로 지목 2건이 4건으로 넓어졌다** → C6-F17 |
| **J-player-2 / J-player-3** (별도 2건) | **병합.** 두 발견의 요구 수정이 같은 디렉터 판정 하나("사실은 고정되고 남길 기록만 고른다"로 약속을 낮추거나, 최소 1개 선택이 실제 경로를 바꾸게 하거나")로 수렴한다 → C6-F2 |
| **J-player-8 / J-publisher-4 / J-narrative-7 / J-producer-5** (README 4중 발견) | **병합** → C6-F18. `verb-seal.jpg` 게시는 **기존 C4-F11**이며 새 id를 열지 않는다 |
| **J-publisher-2 / J-producer-1** (RFC 부재 2건) | **병합 + 확장** → C6-F7. 재측정으로 대상이 RFC-C6-001/002 2건에서 **8건**(+ RFC-Q3·S2·S3·S5·S6·B6·M3·CX)으로 넓어졌다 |
| **H-startability-4 / H-contradiction-1 / H-contradiction-11** (asmdef·우선순위 3건) | **병합** → C7-F4 |
| **H-startability-5 / H-contradiction-7** (EN 로컬라이즈 2건) | **병합** → C7-F5 |
| **H-startability-7 / H-contradiction-2 / J-engineer-2** (세이브 필드 3건) | **병합** → C7-F7 |
| **H-startability-3 / H-contradiction-8 / H-safety-2** (GLB·경로 3건) | **병합** → C7-F3 · C7-F6. `H-safety-2`의 "그레이박스 임포트 = 승격인가" 질문은 **런북 §3.1 항목 1이 이미 차단**하고 3D provenance는 `license: original greybox`(자체 저작)라 라이선스 위험이 없다 → 등급 하향, C7-F3에 흡수 |
| **H-startability-6 / H-contradiction-8 후반** (폴더 레이아웃) | **병합** → C7-F6 |
| **H-contradiction-10 / J-engineer-9** (세이브 파일명) | **병합** → C6-F31(소유가 `save.md`이므로 초안 축에 둔다) |
| **H-startability-11 / H-contradiction-6** (텔레메트리 키) | **병합** → C7-F11 |
| **H-safety-16 · H-safety-17** ("검증 완료") | **결함 아님.** QA 독립 재확인: `grep -rn "git commit\|git push\|git add" handoff/*.md` → 금지 문맥만, `save.md` 개명 금지 목록 15개 ↔ 브리프 문자 일치, 3D provenance 23항목 전건 `license: original greybox` · `runtimeEligible:false`. **기록만 남기고 id를 부여하지 않는다** |
| **H-safety-7 · H-safety-13(mex) · H-safety-15(hintCost)** | **등급 하향.** 셋 다 브리프가 이미 완화 문장을 갖고 있다(`design_*`는 "문서 상수"로 라벨 + L511 금지문 / `mex`는 `[SKIPPED: 사유]` 탈출구 / `hintCost == 0`은 T-I5가 소유한 스키마 불변식의 검증 비교). → C7-F28 · C7-F30 (S4), hintCost는 **결함 아님**(G5 예외 문구 추가는 권고로만 §8에 둔다) |
| **H-contradiction-5** "`I` 키가 두 명령" | **확장 후 채택.** 재측정 결과 같은 자기모순이 **`I`와 `H` 2키**에 걸린다 → C7-F10 |

---

## 5. 잔여 S3/S2 재측정 (C3 · C4 · C5)

`c4-review.md` · `c5-review.md`가 남긴 open 항목을 이번 회차에 전건 재측정했다.

| id | 이전 | 재측정 결과 [OBSERVED 2026-09-10] | 새 status |
|---|---|---|---|
| C3-F27(c) | open S2 | `save.md` **§2.1 「부식 저장 필드 없음 — 부식은 구성안 시험의 파생값」** 신설 확인, §4에 `operationalCorrosion` 제거 절. economy 3문서(`currency-map` §4.1·R1 표 · `sink-source-ledger` §2 · `reward-bands` `persisted:false`)가 "저장 없음(파생)"으로 일치 | **closed** |
| C3-F29 | open S2 | `c4-b1`·`c4-b3`·`c1-b4` 범위는 **해소**(§4 기각 참조). 잔여 실체는 본문↔`zoneId` 4비트 → **C6-F17로 승계**하고 원 id는 닫는다 | **closed** (승계 C6-F17) |
| C3-F35 | open S2 | economy 이행 완료(`currency-map` §4.2 R4-3 · `negotiation-record` N-17). 잔여는 **`data-schemas/plates.md` L39 한 줄**("옵션 B = `plateOriginalWear`와 같은 노브")뿐 — `plate-readout.md` L78 별칭은 **소멸 확인**. 등급 S2 → **S3**(1행) | open (S3으로 하향) |
| C3-F36 | open S3 | `prototype/model.mjs` L58 = `label: '밸브 개폐 각인 ↔ 봉인 완료 접점 각인'` — 캐논 쌍으로 교체 확인 | **closed** |
| C3-F25 | open-rfc | 디렉터가 C3 종료 판정 묶음에서 **closed**(H-1:40 캐논 유지, 폐기는 H-1:20·H+0:10 둘뿐) | **closed** |
| C3-F30 | open-rfc | 디렉터 판정 완료(인용 키 = campaign id, B#은 `timeline.md` §7 파생). 잔여 파생 불일치는 `consistency-audit.md` **A42**(synopsis 소유, open)로 이관 | **closed** (잔여 → A42) |
| C3-F31 · C3-F33 · C3-F22 | open-rfc | 디렉터 판정문이 C3 종료 묶음에 있으나 **decision-log RFC 블록 형식이 아니다** → C6-F7에 흡수, 상태 유지 | open-rfc `[CARRIED]` |
| C4-F6 | open-rfc (RFC-S3) | `unity-implementation.md` §7이 여전히 `chapter`/`dayIndex`/`eventSeq`/`eventLogHash`/`commitIdempotencyKey`, `save.md`는 `stageId`/"`dayIndex` 없음"/`propertyProtection` enum. **RFC-S3는 decision-log에 0건**(X-15) | open-rfc `[CARRIED]` |
| C4-F7 | open S2 | `vfx/vfx-budget.md`·`motion/motion-contract.md`에서 「영수증」 **0회** · `SavePending` **0회** · 「저장 실패」 **0회**. 두 파일 모두 `status: draft` 유지 | open `[CARRIED]` |
| C4-F9 | open S2 | **표시명은 통일됐다**: `gdd.md` §4 = `style-guide.md` §9 = `interaction-rules.md` §2 제목 **6/6**(`배선 추적 · 판독 · 조위정합 · 배수 편성 · 부식 시험 · 이중서명`), `game-ui-contract.json` 3곳 교체 확인. **잔여는 용어집 등재 4건**(X-16: `배선 추적`·`판독`·`배수 편성`·`부식 시험` 미등재) — 용어집 규칙이 "미등재 명사는 UI 문자열에 쓸 수 없다"이므로 **RFC-S6 판정 전까지 이 4개는 `[INFERENCE]`이며 최종 UI 문자열이 아니다**. 등급 S2 → **S3**(소유 이관: systems → worldview) | open (S3, worldview) |
| C4-F11 | open S2 | `docs/media/verb-seal.jpg` 존재 · README L38 게시 유지. `style-guide.md` §10 = "위반 시 REDO, 예외 없음" | open `[CARRIED]` |
| C4-F12 | open S2 | `game-ui-contract.json` 재파싱: `색약` **0** · `볼륨` **0** · `이산` **0**(`색각`은 `matrix[3].setting` 1회). `accessibility{}` 7키에 3팔레트 순회·채널 볼륨·연속값 이산 대안 **없음** | open `[CARRIED]` |
| C4-F13 | closed | `game-ui-contract.meta.md` 해시 표 3행 ↔ `shasum -a 256` 실측 **3/3 문자 일치**(X-7). 재확인 | closed `[CARRIED]` |
| C4-F14 | open S3 | `grep -n "휴은\|중복로\|덴리트\|옥긴다" game-ui-contract.json` = **0행**, `grep -n "C3-b2\|C3-b3" unity-implementation.md` = **0행**. SC-2·SC-3 이행 확인. **SC-4(RFC-P3-015가 180초 정본을 `interaction-rules §4`로 인용하나 그 파일에 `180` 0행)는 디렉터 몫으로 미이행** | open (SC-4만) |
| C4-F16 | open S3 | `interaction-rules.md` **§1-3 신설 확인**(표면 3종 · 배달 우선순위 · 표면별 키 전수 · 도구별 `Space`/`X`·`Y` 배정 · `reader` 예외). 요구 이행. **다만 §1-3.1 예외 문단이 §1-3.2 전수표와 자기모순을 만들었다 → C7-F10 신설.** `verification.matrix`는 여전히 미실행 | **closed** (신규 C7-F10) |
| C4-F17 | open S3 | `modeling/asset-budget.md`에 VFX 카테고리·견적 인일 여전히 부재. 드로콜 실측 0건 | open `[CARRIED]` |
| C4-F18 | open S3 | `presentation/video-study.md` frontmatter = `cycle: …-c2` · `status: draft` 유지, §채택 "55~60초 제목/플랫폼" 유지 | open `[CARRIED]` |
| C4-F20 | open S3 | `dual-seal.md` L30 · `tide-alignment.md` L30 = 「확정 \| `Enter` **길게 0.4 s**」 유지. 두 파일 모두 L44에 "**C4-F20으로 열려 있다**"를 자기 표기 | open `[CARRIED]` (자기 고지 가산) |
| C4-F21 | open S3 | `interaction-rules.md` §1-3.4 D-1~D-7 + 8개 스펙 **§1-A 신설** 확인. systems 파서 재현: KB 토큰 없는 **20행 / 20행 대응 · 누락 0**. 측정 도구(41행 awk)가 깨지지 않도록 별도 절로 올린 판단은 타당 | **closed** |
| C4-F22 | open S3 | 브리프·`c4-fixloop2-input-binding.md`에 조건 `&& $3 !~ /LB\+X/` 명시 + 8행/6행 사실 기재, §5 근거를 `git status` → 해시 전후 비교로 교체 확인 | **closed** |
| C4-F15 | open S3 | 용어집 재측정: `판독대` **0** · `조위관측판` **0** · `성에선` **0** — 미등재 유지. C4-F9 잔여와 **같은 편집**(RFC-S6) | open `[CARRIED]` |
| C5-F2 | open S2 | `cycle-ledger.json`이 C1~C7 7행으로 갱신되고 C6·C7 = `in-progress`. **그러나 집계가 다시 스테일**: C4 `fixed 9 / remaining "S3잔여4"`인데 등록부 실측은 closed 11 · open S2 3 · open S3 5, C5 `fixed 6 / "S3잔여1"`인데 실측 closed 5 · open 6. note "C1~C5 … S1/S2 open 0"도 등록부와 어긋난다 | open (근거 갱신) |
| C5-F3 | open S2 | `production/cycles/c5-development.md` 존재 확인, 검증기(X-4) 오류가 **c5 → c6/c7로 이동**. 원 지적 해소 | **closed** (후속 = C6-F25) |
| C5-F4 | open S3 | `steam-game-plan.meta.md` r5 절이 현행 실측(`92301c0a…` · 121,457 B · 47/47)을 기재하고 r3 값은 `[CARRIED]` 역사로 분리. 산출물 해시 2행 ↔ `shasum` 실측 **2/2 일치**(X-7) | **closed** |
| C5-F5 | open S3 | 규칙 본문 사용 **0건**(X-5, 잔존 5행은 폐기 선언·기록 문장). 덱 렌더 HTML `grep -c` = **0**, 17번 = **"기술 인수 테스트 27개"**(X-6, `unity-implementation.md` §11에서 빌드 시점 파생). economy 2곳·balance 1곳도 정본 표현으로 교체 확인 | **closed** |
| C5-F6 | open S2 | `handoff/` 4파일 존재, README L64~L84가 실제 파일·순서를 서술 | **closed** |
| C5-F7 | open S3 | `economics.meta.md`가 `.claude/skills/game-ops-harness/scripts/validate-preproduction.mjs`(실재, 7.2K)를 인용하고 그 스크립트가 `economics.json` 입력에서 net·손익분기를 재계산함을 확인(X-4 실행) | **closed** |
| C5-F8 | open S3 | `business-model.md` §5 머리에 "**할인 0% · 정가 기준**" 라벨, §5-1(할인 10%) 병기, 인용 시 절 번호 명시 규칙 추가. **QA 독립 재계산(X-10): §5 Net 6값 · 손익분기 18값 · §5-1 Net 3값 · 손익분기 9값 전건 일치**(B1 7,350.95 · B2 8,720.91 · B3 9,985.49 · 3,000만원 B1 4,082본) — PM이 요청한 독립 재현 완료 | **closed** |
| C5-F9 | open S3 | §2 B1 근거가 live `gdd.md` §11 인용(가격 위임 문구) + 아카이브 c3 `gdd.md` L29 계보 병기로 교체 확인 | **closed** |
| C5-F10 | open S3 | (이번 회차 재측정 대상 밖 — modeling 인용 형태) | open `[CARRIED]` |
| C5-F11 | open S4 | `skill-application.md`가 "**포커스 순서·초점 트랩은 측정하지 않았다** … `inert`/`aria-hidden` 적용은 선언일 뿐 실측이 아니다"로 정정 확인 | **closed** |

---

## 6. 승격 판정 (C3-F33 · RFC-Q2)

| 파일 | 판정 | 사유 [OBSERVED 2026-09-10] |
|---|---|---|
| `product/economics.meta.md` | **승격 가능** | 유일 결함 C5-F7 closed(경로 실재 + 스크립트 실행 확인). 이 파일에 열린 결함 0건 |
| `presentation/deck-outline.md` | **승격 가능** | C5-F5 presentation 몫 closed(렌더 0건 · 27개 파생), C5-F4 closed. 원본 표 행(`unity-implementation.md` §11)과 표기 규칙 2줄 추가 확인. 이 파일에 열린 결함 0건 |
| `presentation/steam-game-plan.meta.md` | **승격 가능** | C5-F4·C5-F5 closed. r5 산출물 해시 2행 ↔ `shasum` 실측 2/2 일치, r3·r4는 역사로 보존(RFC-Q2 준수) |
| `product/business-model.md` | **차단** | C5-F8·C5-F9는 closed이고 산식은 QA 독립 재계산과 12/12 일치. **그러나 §1 「가치 약속」에 신규 open S2 C6-F2**(약속 "결론이 바뀐다" ↔ 캠페인 전 분기 비확장)가 열렸다. 이 절의 문장이 디렉터 RFC 판정에 따라 바뀌므로 승격은 판정 후 |
| `systems/interaction-rules.md` | **차단** | C4-F16·C4-F21·C4-F22 closed, C4-F9 표시명 통일 확인. **그러나 (a) 신규 open S2 C7-F10**(§1-3.1 예외 ↔ §1-3.2 전수표 자기모순, `I`·`H`), **(b) RFC-S6 미판정**으로 §2 표시명 4건이 아직 `[INFERENCE]`, (c) §5-4 「일차」 잔존(C7-F27) |
| `systems/unity-implementation.md` | **차단** | **C4-F6(RFC-S3) open-rfc 유지** — §7 저장 스키마가 `data-schemas/save.md`(current)와 충돌하며 CLAUDE.md §9 세이브 필드 불변식 구역. 추가 open S2 3건(C7-F4 asmdef 5 vs 7 · C7-F7 명령 로그/`commitIdempotencyKey` · C7-F9 T-26 문안) |
| `systems/game-ui-contract.meta.md` | **차단** | meta **자체 결함 0**(C4-F13 closed, 해시 3/3 일치). 그러나 이 문서가 기술하는 `game-ui-contract.json`에 **C4-F12(open S2)**가 남아 있다 — C4 판정 규칙("기술 대상이 차단") 유지 |
| `balance/puzzle-balance.md` | **차단 (의도)** | 문서 L11이 스스로 "`status: draft`이며 **정본이 아니다** — 수치는 `balance/balance-sheet.md` §6이 소유"라고 고지한다. 승격하면 정본이 둘이 된다. **사유 불변** — 승격 지시와 QA 판정이 충돌하면 디렉터가 차단을 해제하는 RFC를 발행해야 한다(balance 레인 counter 수용) |

---

## 7. 게이트 측정 — 최종 문서 수준 값 (런타임은 전부 NOT-MEASURED)

전문·YAML `measured:` 블록은 `qa/gate-measurements.md` `#g1`~`#g8`에 동기화했다. 아래는 요약이다.

| 게이트 | 문서 수준 값 | 런타임 | 막고 있는 것 |
|---|---|---|---|
| **#g1** 세계관 일관성 | **FAIL** — violation **1건**(C6-F17, 본문↔`zoneId` 4비트) · `consistency-audit.md` open **3건**(A25·A29·A42) · 서사 인용 결함 4건(C6-F15·F20·F21·F22) | NOT-MEASURED | C3-F25·C3-F30 스테일 인용은 **교체 완료**(C6-F34 closed). C6-F17을 감사 §2에 등재해야 게이트와 증거 소스가 같은 것을 가리킨다 |
| **#g2** 밸런스 밴드 | **N/A(전투) + NOT-MEASURED(대체)** · 문서 신규 S2 2건(C6-F3 힌트 1단 유출 · C6-F5 조작 유형 편중 21/33) | NOT-MEASURED | 시뮬 0회 · `puzzle-balance.md` 차단 유지 · `offerMaxPerSession` 미정 |
| **#g3** 경제 건전성 | **N/A(통화) + NOT-MEASURED(대체)** · **상품 산식은 QA 독립 재계산 12/12 일치**(X-10) · C3-F27(c) closed · C3-F35 잔여 1행 | NOT-MEASURED | 지불의사 n=0 · `shareVerified:false` · C6-F2(약속↔설계) |
| **#g4** 연출 / 몰입 | **문서 FAIL** — open S2 2건(C4-F7 · C4-F11) + C4-F9 잔여(RFC-S6) + C6-F27(상점 자산 준비도 0) + C4-F18 | NOT-MEASURED | 표본 0 · `vfx-budget`·`motion-contract` 둘 다 draft |
| **#g5** 에셋 / 이펙트 예산 | **문서 부분** — provenance ↔ 파일 해시 86/86(modeling 재현), 수량 계약 47종 불변. **승격 가능 자산 0종**(런북 §3.1: 라이선스 UNVERIFIED · 텍스트 확대검수 0 · 런타임 예산 실측 0) | NOT-MEASURED | C4-F17(VFX 예산 항목 부재) · C7-F3(FBX 0건) · C7-F13(FORCE 덮어쓰기) |
| **#g6** 운영 안정성 | **문서 FAIL** — 세이브 v1이 두 문서에서 갈리고(C4-F6 / RFC-S3 미판정), 명령 로그에 페이로드·`commitIdempotencyKey` 부재(C7-F7), 파일명 세 벌(C6-F31), 확정당 쓰기 횟수 미정의(C6-F32) | NOT-MEASURED | Unity `Assets/` **0파일** · 배치모드 실행 **0회** · 기준 하드웨어 미정(PRE-1) · **C7-F1(S1)** |
| **#g7** 피처 수용 / 코어루프 | **NOT-MEASURED** — 수용 조건 정의는 완료(RFC-P3-011: `total_minus_afk_min` · 목표 450~540 · 철회 트리거 <420/<360). `verification-plan.md` 판정선 3개(≤60초 · 10/12 · 0/12)는 **표본 0 위의 제안값** | NOT-MEASURED | n=0. 문서 결함 C6-F1(첫 30분 목표) · C6-F2(선택 실체) · C6-F4(재개 요약) |
| **#g8** 신선도 · 메모리 | **PARTIAL** — `freshness-check.sh` **exit 0 · 0 finding / 104 artifacts**(본 문서 생성 후 재실행). 도구 출력 그대로: "scope = frontmatter contract + supersedes topology only" · "**memory_sync receipts (mex/llm-wiki/graphify/zg) are NOT verified here**" · "no cycle start supplied; **staleness not measured**" | NOT-MEASURED | `mex` 이번 회차 **미실행**(CLAUDE.md §10 · `[SKIPPED]`) · `graphify` **[UNGRAPHED]**(코드 0줄) · `zg index --rebuild` 미실행(`handoff/` 신설로 레이아웃 변경) · vault 리포트 미작성 → **G8 전체 PASS로 읽지 않는다** |

**0 / 8 PASS.** 막는 것: ① 열린 S1 1건(C7-F1) ② open S2 32건 ③ 빌드 0줄 · 표본 n=0 · 시뮬 0회 · 캡처 0건 ④ `memory_sync` 미검증. **③은 문서 작업으로 해소되지 않는다.**

---

## 8. 브로드캐스트 (dependency-matrix ● 항목) · `feedback-requested-by: 2026-09-11`

| 받는 레인 | 결함 | 요청 |
|---|---|---|
| **game-production-director** | **C6-F7**(최우선) · C6-F9 · C6-F18 · C6-F23 · C6-F25 · C7-F1 · C7-F4 · C7-F12 · C7-F16 · C7-F22 · C7-F23 · C7-F29 · C4-F6 · C4-F14 SC-4 · C5-F2 | **decision-log에 RFC 블록 8건을 실제로 append**(RFC-C6-001·002 · Q3 · S2 · S3 · S5 · S6 · M3)하거나 초안·핸드오프의 인용을 "개설 예정"으로 낮춘다 · **C7-F1 판정**(T0 인스턴스 데이터 저작 주체·경로·납기, 또는 실행자에게 T0 한정 저작 권한) · README 소유 레인 확정 + L11 캐논 정정 · `production/cycles/c6-development.md` 작성 · `cycle-ledger` 집계를 등록부와 일치 |
| **game-planner** | C6-F1 · C6-F2 · C6-F3 · C6-F4 · C6-F5 · C6-F10 · C6-F14 · C6-F15 · C6-F16 · C6-F17 · C6-F19~F30 · C6-F37 · C6-F40 · C7-F5 · C7-F26 | 초안에 **연표·공개 상한·T0 착수 결정·환불 창·인수 테스트 27전수**를 넣고, `hints[0]` 33건을 1단 정의대로 재작성, `c2-b4`·`c6-b4`·`c1-b2`·`c5-b2` 본문 구역을 `zoneId`와 맞추거나 "허브 회귀 교차 구역"을 RFC로 캐논화, 검증기에 **H-02(1단 인과·자료 금지)·Z-03(본문 구역 ↔ `zoneId`)** 저자 체크 추가 |
| **game-systems-designer** | C6-F11 · C6-F12 · C6-F13 · C6-F31 · C6-F32 · C7-F2 · C7-F6 · C7-F7 · C7-F8 · C7-F9 · C7-F10 · C7-F11 · C7-F15 · C7-F17~F21 · C7-F24~F28 · C7-F30 · C7-F32 · C4-F12 · C3-F35(잔여 1행) | **§1-3.1 예외 문장에서 `I`·`H` 제외**(또는 두 조회를 다른 키로) · `save.md`에 `payload`·`commitIdempotencyKey` 등재 · 텔레메트리 계약에 도구 키 절 + `command_count` 신설(또는 T-I6 완화) · 브리프 §③/§④-1 레이아웃 통일 · 패키지 추가 절차·빌드 명령·액션 맵 이름 명시 · T-IMP-1을 동치 비교로 · `plates.md` L39 별칭 삭제 |
| **game-worldview-architect** | C4-F9(잔여) · C4-F15 · C6-F33 · C7-F5 · C7-F14 | **RFC-S6 판정**: `배선 추적`·`판독`·`배수 편성`·`부식 시험`(+`판독대`·`조위관측판`·`성에선`) 용어집 등재. 등재 전까지 이 명사들은 UI 문자열이 아니다. `보존 등급 문장 1줄`을 바이블 §6 에필로그에 추가할지 ack/거부 |
| **game-product-manager** | C6-F6 · C6-F8 · C6-F26 · C6-F28 · C6-F38 · C6-F39 · C6-F41 · C6-F2(공동) | 포지셔닝 한 문장 · **Steam 생성형 AI 공개(Pre-Generated) 신고 + 라이선스 UNVERIFIED를 위험 표에 등재** · 상표/GRAC 크리티컬 패스 · 데모 결정 시점 · 현지화 자수·`externalCosts` |
| **game-modeler** | C7-F3 · C7-F13 · C7-F12(공동) · C7-F23 | FBX 단일화 판정 후 `SM_Tool_*.fbx` 6종 내보내기 · `FORCE=1` 경로에 원본 아카이빙 절차 신설(또는 재생성 잠금) · 런북의 decision-log 쓰기 지시를 `messages/`로 |
| **game-balance-designer** | C6-F3(공동) · C6-F5 · C6-F35 · C7-F21 | 밸런스 시트에 **조작 유형 열**(물리 조작 / 표·분류 / 읽기) 추가 후 스테이지별 비율 재도출 · 무진전 제안 on/off·간격 설정 · 같은 비트 1회 무시 시 재제안 금지 · `readBudget` 소유 확정 |
| **game-concept-artist / game-presentation-director** | C4-F11 · C4-F18 · C6-F27 · C6-F8(공동) | push 전 `verb-seal` 재생성 또는 README에서 제거 · `video-study.md` 승격 또는 인용 강등 · 상점 자산 준비도 표(캡슐 0/N · 스크린샷 0/5 · 트레일러 0 · 라이선스 UNVERIFIED) |
| **game-vfx-artist / game-motion-designer** | C4-F7 · C4-F17 | 「권위 있는 연출은 저장 영수증 이후」를 두 계약에 명문화 + `SavePending` 진행 표시 규격 |
| **game-synopsis-writer** | C6-F1 · C6-F2 · C6-F16 · C6-F33 · A42 | T0 텍스트의 밤 목표 문장 · 분기 실체 판정 반영 · A42(B# 파생) 닫기 |
| **game-economy-designer** | C6-F33 · C7-F21 · C3-F35(완료 확인) | 에필로그 보존 등급 축 RFC 개설 · `readBudget` 소유 협상 |

---

## 9. 이 문서가 주장하지 않는 것

- **게임이 존재한다고 주장하지 않는다.** Unity `Assets/` 0파일 · 배치모드 실행 0회 · 코드 0줄.
- `47/47 PASS`는 **저작 JSON의 구조 검사**다. Unity 임포트·런타임 불변식은 여전히 0건 검증이다.
- `512/512 passed`(preproduction 검증기)는 **문서·스키마·산술**만 본다 — 도달 가능성·시장 적합성·플레이 시간이 아니다.
- 핸드오프 4종이 존재한다는 사실은 **G6·G7을 올리지 않는다.** 계획은 측정이 아니다.
- **판정단 5렌즈·반박 3렌즈는 사람 플레이어가 아니다.** 그들의 "score"·"ship" 값을 이 문서는 인용하지 않으며 게이트 입력으로 쓰지 않는다. 채택한 것은 **파일에서 재현되는 근거뿐**이고, 재현되지 않은 것은 §4에서 기각했다.
- **480분은 설계 예산이다.** `observedMedianMinutes: null` · `humanPlaytests: []`.
- 생성 리소스 45장·영상 2클립은 **프리비즈이며 게임플레이가 아니다.**

---

## 10. 재검증 1 — C6/C7 수정 루프 1 (2026-09-10)

**대상**: 디렉터가 3개 레인(systems 7 · planner 2 · modeling 1)에 배정한 **S2 10건**.
**방법**: 레인 보고는 상태가 아니다. 각 결함의 `required_fix`를 기준으로 **파일을 다시 읽고 명령을 다시 돌려** 판정했다. 레인이 [OBSERVED]로 적은 값은 **독립 재현되는 것만** 채택한다.
**이 절이 올리는 게이트**: **없다.** Unity 실행 0회 · 빌드 0줄 · 사람 플레이 표본 n = 0 · 패드 실측 0건 · 프레임/저장 캡처 0건은 이 루프에서 전혀 바뀌지 않았다.

### 10.1 재검증 명령 [OBSERVED 2026-09-10]

| # | 명령 | 결과 |
|---|---|---|
| Y-1 | `node planning/validate-campaign.mjs` | `{checks 47, pass 47, fail 0, verdict PASS}` · exit 0. sha·바이트는 검증기 출력이 소유한다(RFC-Q1, 재기재하지 않는다) |
| Y-2 | `node systems/pipeline/emit-tables.mjs --out <scratch>` | exit 0 · `beats.json` **121,457 B** · `hints.json` `rows 99` · `tables-receipt.json` **2,491 B** 생성 |
| Y-3 | `shasum -a 256` × 2 (저작 원본 ↔ 생성된 `beats.json`) | **문자 일치**. 「변환 계층 0」(beats.md M2)이 문서 주장이 아니라 **기계 성질**임을 QA가 독립 확인 |
| Y-4 | fail-closed 픽스처: `t0-b1.zoneId`를 스테이지 밖으로 바꾼 사본으로 `emit-tables.mjs --out <새 경로>` | 검증기 `Z-01` **FAIL**(46/47) → 생성기 **exit 1**, stderr `검증기 verdict=FAIL fail=1 …(fail-closed)`, **출력 디렉터리 미생성** |
| Y-5 | `node planning/validate-campaign.mjs <픽스처>` 의 `file` 필드 확인 | 픽스처 **절대경로**를 반환 — 검증기가 argv 경로를 실제로 읽는다. 즉 Y-4는 live 파일을 검사한 위장 시험이 **아니다** |
| Y-6 | `python3` manifest 비-모듈 의존 | `['com.unity.multiplayer.center']` — 필수 4종 여전히 **0/4**(수정 루프는 문서만 고쳤다. 맞다) |
| Y-7 | `python3` `packages-lock.json` id 중복 계수(최상위 + 중첩 `dependencies`) | **17개 id가 2회 이상** 등장(`unitywebrequest` 7 · `jsonserialize` 6 · `physics` 6 …) → §10.3 C7-F33 |
| Y-8 | `grep -rn "mediaType" _workspace/current` | **8행 / 8파일**. 전건 개명 이력 서술이며 **스키마 정의는 0건** → 개념 통일은 성립. 다만 systems 영수증의 "2곳뿐" 주장과 불일치 → §10.3 C7-F36 |
| Y-9 | `grep -rn "확정 버튼 → 프리뷰" _workspace/current` | 잔존 4행 전부 **부정문·변경로그·QA 원장**. 주장으로 선 곳 **0** |
| Y-10 | B# 색인 독립 재계산(C3-F30 정의 = 스테이지 순서 계수) | `c1-b3`=**B06** · `c3-b3`=**B14** · `c6-b3`=**B26** — 초안 §2.5·§3.1 병기와 일치 |
| Y-11 | 6개 도구 스펙 §6 키 ↔ `telemetry-contract.md` 전문 대조(python) | 스펙 §6 키 **전건이 계약에 정의됨**. §4.1 밖 3건(`beat_reached`·`commit_time_min`·`sandbox_time_min`)은 공통 키 절에 정의 → `T-I6` 충족 가능. **미등재 0** |
| Y-12 | `grep -rn '`Q`' systems/ handoff/` + `zones.md` L53 | `Q`가 **미사용 토큰이 아니다** — `zones.md` `neighbors` 열이 `Q`/`E` 순회를 배정 → §10.3 **C7-F35** |
| Y-13 | `shasum -a 256` × 4 (`game-ui-contract.json`·`interaction-rules.md`·`unity-implementation.md`·`animation-contract.md`) ↔ `game-ui-contract.meta.md` 표 | **4/4 해시·바이트 문자 일치**. 이 루프의 편집 후에도 meta 표가 스테일하지 않다 |
| Y-14 | `python3` `game-ui-contract.json` 재파싱 | 최상위 13키 · `verification.matrix` **20행** · `decisions` 13. `matrix[18]`(T-26a/b) · `matrix[19]`(`Q` 표면 스코프) 신설 확인 |
| Y-15 | frontmatter 전수(15파일) | `updated: 2026-09-10` 전건 · `cycle` 값 **불변** · `supersedes` 변경 0 → RFC-Q2 준수 |
| Y-16 | `freshness-check.sh` | `0 finding(s) across **106** markdown artifact(s)` · exit 0. 도구 자기 고백 그대로: "memory_sync receipts … are **NOT** verified here" · "no cycle start supplied; **staleness not measured**" |
| Y-17 | `git status --short` | **62행 · 삭제(`D`) 0건 · 추적 수정 17건**. 세션 시작과 동일하며 타 세션 작업 무손상. QA는 `qa/` 밖을 쓰지 않았다 |

### 10.2 배정 10건 판정

| id | sev | 레인 | `required_fix` 이행 여부 [OBSERVED 2026-09-10] | 새 status |
|---|---|---|---|---|
| **C6-F13** | S2 | systems | **전건 이행.** ① 검증 규칙 단일 출처 — `emit-tables.mjs`가 `validate-campaign.mjs`를 **호출**하고 규칙을 복제하지 않는다(Y-2·Y-5). ② 영수증 = `tables-receipt.json`(sha256·verdict·집계·임포터 계약). ③ `beats.json`이 저작 원본과 **바이트 동일**(Y-3) — 「변환 계층 0」이 기계로 증명된다. ④ fail-closed 실증(Y-4). ⑤ 브리프 §④-2가 **재작성**되어 임포터 몫은 영수증 대조 `V-1~V-4` + 런타임 전용 `R-1~R-4` 넷뿐이고 "47검사를 C#으로 재구현"은 철회됐다(L727·L812 영문 요약까지 일치). ⑥ `mediaType`→`sourceType` 통일, 스키마 정의 잔존 0(Y-8) | **closed** |
| **C6-F14** | S2 | planner | **전건 이행.** 정본 문구 「프리뷰 → 초점을 확정 버튼으로 옮겨 한 번 누름. 홀드 없음」이 초안 §4.1 층 B·§4.1 순서 주의·§4.3 확정 기본값 / gdd §3.3 층 B·§5 / Appendix A에 **문자 일치**로 들어갔고, 키 배정(`Space`→`Enter` / `X`→`A`)이 병기됐다. 요구된 "`confirm-dialog`와의 차이 한 줄"이 초안 §4.3·gdd §5에 **별도 행**으로 신설됐고 그 서술이 `interaction-rules.md` §1-1 L54(「확정 시 확인 대화 1회」)와 일치한다. 옛 순서는 주장으로 **0곳**(Y-9) | **closed** |
| **C6-F15** | S2 | planner | **전건 이행.** §2.5 「대조의 밤 — 캐논 시각과 순서 앵커」 신설. 요구대로 **시각 값을 재기재하지 않고** `worldview/timeline.md` §2·§8을 전 행 출처 열로 인용했다(§2 L34·§8 L172·L181·L183 실재 확인). 순서 앵커 논리(같은 계통·같은 형식·같은 기록계 → 간격 > 총 오차폭)와 폐기 범위(밸브·침수 2개, 서명 기입은 캐논 유지 = C3-F25)가 들어갔고, §3.1 **C3 행 = B14(`c3-b3`) · C6 행 = B26(`c6-b3`)** 병기 확인. B# 독립 재계산 일치(Y-10). 검증기 경계(`K-03`·`K-04`는 데이터 사실만 증명)를 초안이 스스로 적은 것은 가산 | **closed** |
| **C7-F2** | S2 | systems | **전건 이행 + 신규 1건.** §③-1a 신설(버전 미지정 `Client.AddAndRemove` Editor 부트스트랩) · §⑩-2 **#0 단계** 신설 · N-10 예외 3곳 등재(L124·L170·L747, 전이 의존 허용 / API 직접 사용은 RFC 유지). **가산**: "이 절은 실행되지 않았다 [INFERENCE]"와 GUI 폴백을 스스로 적어 측정 위장을 피했다. **다만 하드 게이트 명령이 과다 계수한다** → C7-F33 | **closed** (신규 C7-F33) |
| **C7-F6** | S2 | systems | **전건 이행 + 신규 1건.** §③ 트리에서 `zones/plates/tools.json` 제거, `Authoring/` ScriptableObject로 통일(형태 근거표 `plates.md` §0-1 신설 확인). 씬·아트·에디터를 `_Project/` 안으로 들여 "밖에는 서드파티만"을 **예외 0건**으로 만들고, 런북 §3.2 승격 경로 5행을 `Assets/_Project/Art/**`로 정정(L193~L197·L200). 비용 0 근거 재측정: `find unity/Unknown/Assets -type f` = **0**. **다만 새로 등장한 `Tide.EditorTools`가 모듈 경계표에 없다** → C7-F34 | **closed** (신규 C7-F34) |
| **C7-F7** | S2 | systems | **전건 이행.** 재생 단위 = **명령 소싱** 확정. `CommandEntry`에 정규화 `payload` 신설(L127), `payloadHash`는 **무결성 전용**으로 격하(L128), 이벤트는 `Commit` 파생물이며 세이브에 0건(L95·L106). `commitIdempotencyKey`가 §1 L45에 등재되고 **개명 금지 목록**에 `payload`·`commandId`와 함께 편입(L162·L164). 상한 재산정: 「50,000/8 MB」 폐기 → `byteCap` 6 MiB · `entryCap` 20,000(먼저 닿는 쪽), `save-undo.md` L155 동기화. 불변식 `S-I10`(Commit 순수성)·`S-I11`·`S-I12` 신설. **가산**: 엔트리 평균 280 B를 `[INFERENCE]`로 명시하고 T0 재측정 과제를 남겼다 — 추정을 관측으로 쓰지 않았다 | **closed** |
| **C7-F9** | S2 | systems | **전건 이행.** T-26을 "패널 안 `X` = 등재한 부작용 없는 실행 1개, `Y` = 등재 의미 1개, 등재 없으면 명령 0개"로 재기술하고 T0 실체화 `T-26a`(circuit `X`=구획 접기·`Y`=없음) · `T-26b`(reader `X`=사본 재생·`Y`=인용 고정) · `T-26c`를 신설. 정본 4곳 동기화 확인 — 브리프 L644~L647·K-3 L512 · `unity-implementation.md` §11 L161~L163 · `matrix[12]` · `matrix[18]`. **QA 대조**: 넷 모두 `interaction-rules.md` §1-3.2 전수표(circuit/reader/routing 배정)와 일치 | **closed** |
| **C7-F10** | S2 | systems | **지정된 해소는 이행, 그러나 선택한 키가 자유롭지 않다.** §1-3.1 예외가 `I`·`H`·`F1` 오버레이 진입으로 복원되고 도구 패널 조회는 **`Q`**로 분리됐다(§1-3.2 키 표 + 조회 배정표 전수). 파생 동기화 확인: `wiring-trace.md` §1·§1-A · `tide-alignment.md` §1·§1-A · 브리프 L333·L503·L517·T-26c · `matrix[19]`. 기각안 A의 근거(§0-8 비대칭)도 문서에 남겼다 — **원 결함이 지목한 §1-3.1 ↔ §1-3.2 자기모순은 소멸**했다. 그러나 `Q`가 미사용이라는 [OBSERVED] 근거가 재현되지 않는다(Y-12) → **C7-F35 신설** | **closed** (신규 **C7-F35 S2**) |
| **C7-F11** | S2 | systems | **전건 이행 · 완화 없이 계약 쪽을 채웠다.** `telemetry-contract.md` **§4.1 도구 키 절** 신설(6종 도구 전건 등재) · `command_count`·`entries_count`를 §4에 정의 · `tools.md` T-I6은 **완화되지 않았고** 등재부 경로만 명시. 브리프 §⑨-2에 퍼즐 6구간(`first_valid_action` 포함)·`ending_reached` 행 추가, DoD 9에 T-I6 통과 조건 명시. **QA 독립 대조(Y-11): 6개 스펙 §6 키 전건이 계약에 정의됨 · 미등재 0** | **closed** |
| **C7-F13** | S2 | modeling | **레인 몫 해소 · 결함이 지목한 파일은 미변경.** `modeling/pipeline.md` §14 신설(14.1 근거 재현 · 14.2 아카이브 레이아웃 · 14.3/14.4 절차 A·B · 14.5 계보 스키마 · **14.6 잠금표** · 14.7 검증 · 14.8 한계)과 `asset-manifest.md` OPEN-M7 확인. 잠금의 구속력 전제를 QA가 **원문 대조**했다 — `asset-runbook.md` L14에 「충돌하면 **그 문서가 이긴다.**」가 **실재**하므로 §14.6 인용은 정확하고 잠금은 성립한다. **그러나 결함 파일(`handoff/asset-runbook.md`)은 이 루프에서 바뀌지 않았다**: §1.1 L44가 파괴적 동작을 경고 없이 서술하고, §4.4 L263~L265가 복사·실행 가능한 `FORCE=1` 3줄을 **§14 교차 참조 없이** 제시하며, `scripts/gen-2d.sh`는 미패치(L19 덮어쓰기 · L29 옛 provenance 항목 삭제 · `git ls-files assets/generated` = **0** 재확인) | **open** (S2 → **S3** 하향) |

**배정 10건 = closed 9 · open 1(하향).** 열린 S1은 이 루프에서 **다뤄지지 않았다** — C7-F1은 디렉터 배정이며 **여전히 open**이다.

### 10.3 이 루프에서 새로 생긴 결함 (재측정으로만 발견)

| id | sev | 레인 | repro | evidence [OBSERVED 2026-09-10] | status |
|---|---|---|---|---|---|
| **C7-F35** | **S2** | systems | `grep -rn '`Q`'` ↔ `data-schemas/zones.md` L53 ↔ `interaction-rules.md` §1 L29 · §1-3.2 L139 ↔ `matrix[19]` | **`Q`는 미사용 토큰이 아니었다.** C7-F10 해소가 `Q`를 도구 패널 조회로 배정하며 근거로 "저장소 전체 미사용 [OBSERVED grep 0건]"을 들었으나, `zones.md` L53이 `neighbors` 열을 **「`Q`/`E` 순회 순서를 결정」**으로 정의하고 있다(Y-12). 그 결과 같은 `status: current` systems 문서 **셋이 서로 다른 말**을 한다 — (a) `zones.md` L53 `Q`/`E` = 셸 표면 시점 노드 순회, (b) `interaction-rules.md` §1-3.2 L139 `Q`@`Shell` = **발행할 명령 0개**, (c) 같은 파일 §1 L29 시점 노드 이동 = `Tab`/`Shift+Tab`+`Enter`. `matrix[19]`의 `expected`가 "셸에서 Q는 아무 명령도 발행하지 않는다"를 **인수 조건으로 고정**해 모순이 테스트에 박혔다. 브리프 §0-11("명시되지 않은 겸용은 결함")이 금지하는 상태이며 **C7-F10이 없애려던 결함 종류가 다른 키에서 재발**했다 | open |
| **C7-F33** | S3 | systems | `codex-unity-brief.md` §⑩-2 #0 하드 게이트 ↔ `packages-lock.json` 구조(Y-7) | 게이트가 `grep -c '"com.unity.inputsystem"\|"com.unity.ugui"\|"com.unity.localization"\|"com.unity.test-framework"' packages-lock.json` 출력이 **정확히 `4`**여야 진행하라고 한다. 그런데 `packages-lock.json`은 각 패키지의 중첩 `dependencies` 블록에 **같은 id를 반복 기재**한다 — 설치 전인 현재 파일에서도 **17개 id가 2회 이상** 등장한다 [OBSERVED]. `com.unity.localization`·`com.unity.inputsystem`이 `ugui`·`test-framework`를 의존으로 적으면 계수는 4를 넘고 게이트는 **성공을 실패로 읽는다** [INFERENCE — 설치 후 실측 0회]. 방향은 안전(멈춤)이지만, 브리프 자신이 N-9에서 세운 "게이트를 무시하게 만들지 않는다" 원칙을 깬다. **요구**: 계수를 최상위 `dependencies` 키에 한정한다(예: `python3 -c "import json;d=json.load(open(...));print(sum(k in d['dependencies'] for k in [...]))"`) | open |
| **C7-F34** | S3 | systems | `architecture-contract.md` §2 표(L41~L48) ↔ 같은 파일 §3 트리(L93) ↔ `codex-unity-brief.md` §③ 트리(L83) | C7-F2/C7-F6 해소가 `Editor/ (asmdef **`Tide.EditorTools`**)`를 두 트리에 추가했는데, **모듈 경계표 §2에는 그 행이 없다**. §2는 모듈별 `UnityEngine 참조`·`책임`·`금지`를 정하고 §2.1 허용 의존 방향 그래프의 노드 집합을 이루는 절이다. 따라서 `Tide.EditorTools`는 **참조 허용 범위가 정의되지 않은 어셈블리**이며, 실행자가 `PackageBootstrap`·`EmitTablesMenu`를 놓을 때 `Tide.Data` 참조 가부를 알 수 없다. `grep -rn "Tide.EditorTools" _workspace/current` = **2행 / 2파일**(둘 다 트리 그림) | open |
| **C7-F36** | S4 | systems | `systems/tech-verification/c6-c7-fixloop1-systems.md` L100 ↔ Y-8 | 영수증이 "저장소에 남은 `mediaType`은 **QA 문서의 결함 서술 2곳뿐**이다 [OBSERVED `grep`]"이라 적었으나 실측은 **8행 / 8파일**이다(QA 2 + `beats.md`·`plates.md`·`dual-seal.md`·`plate-readout.md`·브리프 L181 개명 이력 5 + 영수증 자신 1). **개명 자체는 성립**하고(스키마 정의 0건) 잔존은 전부 이력 서술이라 실질 영향은 없으나, `[OBSERVED]` 표기가 붙은 문장이 재현되지 않는다. **요구**: "스키마 정의 0건, 잔존 8행은 개명 이력 서술"로 정정 | open |
| **C7-F37** | S4 | systems | `data-schemas/save.md` §3.2 L150~L151 ↔ `save-undo.md` L155 | 단위가 섞였다 — 「세이브 파일 전체 ≤ **8 MB**」 + 「`commandLog` ≤ **6 MiB**」 + 「나머지 **2 MB**는 `progress`·`snapshots`·루트 몫」. 6 MiB = 6.29 MB이므로 총량이 8 MB(10진)면 나머지는 1.71 MB이고, 2 MB가 맞으려면 총량이 **8 MiB**여야 한다. 상한 자체가 `[TARGET]`이라 판정에 쓰이지 않지만 임포터·직렬화 가드가 이 숫자를 상수로 받는다 | open |

### 10.4 레인 counter 처리

| counter | 판정 | 근거 |
|---|---|---|
| systems: "C6-F13 `required_fix`의 `--emit tables/`는 planner 소유 파일이라 편집 불가 → 대신 `emit-tables.mjs`가 검증기를 **호출**" | **수용** | `validate-campaign.mjs`는 `planning/` 소유가 맞다. 요구의 목적(규칙 단일 출처 · 영수증 해시 · fail-closed)은 Y-2~Y-5로 **전건 실증**됐다. 구조 선택은 목적을 바꾸지 않는다. planner가 `--emit`을 원하면 RFC로 뒤집을 수 있고 그때 생성기는 얇은 래퍼로 남는다 |
| systems: "C7-F10 (A)안 기각 — 패드는 패널 안에서 증거함·가설판에 가는데 키보드만 못 가면 §0-8 비대칭" | **수용(판단 자체)** | 비대칭 손실 지적은 타당하고 기각 근거를 문서에 남긴 것도 옳다. **다만 채택안 (B)의 전제(`Q` 미사용)가 재현되지 않으므로** 결론은 C7-F35로 다시 열린다 — 기각이 잘못이었다는 뜻이 **아니다**. 세 번째 키를 고르거나 `zones.md`의 `Q`/`E`를 정리하면 (B)안이 그대로 산다 |
| systems: "`Q`가 재매핑 UI·글리프·비-QWERTY 배열에서 문제 없는지 UI/UX 확인 필요" | **유효 · 미해소** | `matrix[19]` 미실행. 이 루프는 실측을 만들지 않았다 — `interaction-rules.md` L359가 "「`Q`` 바인딩은 한 번도 눌린 적이 없다"를 스스로 적은 것은 가산 |
| systems: "`game-ui-contract.json` 편집이 공백 서식 정규화를 함께 일으켰다(+약 0.8 KB)" | **수용** | QA 재파싱 결과 최상위 13키 · `matrix` 20행 · `decisions` 13으로 **구조 손실 0**, meta 해시 표는 실측과 4/4 일치(Y-13·Y-14). 고지도 됐다 |
| systems: "C4-F12를 이번 배정에 넣으면 `game-ui-contract.json` 해시가 한 번 더 갱신된다 — 함께 배정할지 판단 필요" | **QA 답변** | 배정하라. C4-F12는 재측정에서 여전히 `색약 0 · 볼륨 0 · 이산 0`이며(§5 `[CARRIED]`) `game-ui-contract.meta.md` 승격을 혼자 막고 있다. 해시 갱신은 meta 표를 같은 편집에서 고치면 비용 0이다 |
| planner: "§2.5에 `H-` 시각 값이 없으므로 다음 QA가 C6-F15의 claim 문장만 보고 재개설할 소지" | **수용 · 이 절이 선례** | 판정 기준은 **`required_fix`**이며 그 요구가 "숫자를 재기재하지 말라"였다. 본 재검증은 값 없음을 **이행으로** 판정했다. 이후 회차도 같게 읽는다 |
| planner: "verb-04 R3/E1 수정은 배정 밖 — 엄격히 지켜야 하면 되돌린다" | **되돌리지 않는다** | R3 L39·E1 L50을 읽은 결과 **같은 오해의 파생**이며 정본 문구와 일치한다. 같은 편집에서 고친 것이 옳다 |
| modeling: "지시문은 `handoff/`를 systems 소유라 했으나 `asset-runbook.md` frontmatter는 `owner: game-modeler`" | **사실은 인정, 결론은 기각** | 런북 **L12 원문**: "이 문서는 `handoff/` 소유권(systems·디렉터 승인) 아래에서 **모델러가 리소스 파이프라인 절에 한해** 작성 권한을 받아 쓴 것이다." 즉 §1.1·§4.4는 **모델러가 쓸 수 있는 절**이며 권한 부재가 아니다 |
| modeling: "그럼에도 쓰지 않은 이유는 동시 편집 위험(C7-F12·F16이 같은 파일에 디렉터 배정, git 미추적이라 충돌 복구 불가)" | **수용 · 무과실** | CLAUDE.md §8 근거로 타당하다. `git status --short` 재측정에서 삭제 0·타 세션 손상 0을 확인했다(Y-17). C7-F13이 열려 있는 것은 **모델러 과실이 아니라 편집 조정 문제**이며 디렉터 판정 사항이다 |
| modeling: "§14.5 `status` 필드 도입이 `assets[].id` 유일성 의존 소비자에 영향" | **QA 답변** | QA가 아는 소비자는 provenance ↔ 파일 해시 86/86 대조와 「PNG 수 = 항목 수」 둘뿐이며, §14.5가 `status != "superseded"` 한정으로 설계돼 둘 다 깨지지 않는다. 다른 의존이 있으면 해당 레인이 회신해야 한다(브로드캐스트) |

### 10.5 승격 판정 갱신 (§6 대체)

| 파일 | 이전 | 이번 | 사유 [OBSERVED 2026-09-10] |
|---|---|---|---|
| `product/economics.meta.md` | 승격 가능 | **승격 가능(유지)** | 이 루프에서 신규 결함 0건 |
| `presentation/deck-outline.md` | 승격 가능 | **승격 가능(유지)** | 〃 |
| `presentation/steam-game-plan.meta.md` | 승격 가능 | **승격 가능(유지)** | 〃 |
| `systems/interaction-rules.md` | 차단 | **차단(유지)** | C7-F10은 closed됐으나 **(a) 신규 C7-F35(S2)**, (b) RFC-S6 미판정으로 §2 표시명 4건 `[INFERENCE]`, (c) §5-4 L299 「장·**일차**·조위 위상」 잔존(C7-F27) — 재측정 확인 |
| `systems/unity-implementation.md` | 차단 | **차단(유지)** | C7-F7·C7-F9 closed로 2건 해소. 그러나 **C4-F6(RFC-S3) open-rfc** + **C7-F4**(asmdef 5분할 ↔ 7분할, 이제 §③ 트리는 Editor 포함 8) 유지 |
| `systems/game-ui-contract.meta.md` | 차단 | **차단(유지)** | meta 자체 결함 0 · 해시 4/4 일치(Y-13). 기술 대상 `game-ui-contract.json`의 **C4-F12 open**(재측정 `색약 0·볼륨 0·이산 0`) |
| `balance/puzzle-balance.md` | 차단(의도) | **차단(유지)** | 사유 불변 — 문서가 스스로 "정본이 아니다"를 고지 |
| `handoff/asset-runbook.md` | — | **차단(신규 판정)** | `status: draft`(C7-F22)이고 **C7-F13 open**. 착수 전 필독으로 지정된 문서가 파괴적 명령을 경고 없이 싣고 있다 |
| `systems/pipeline/emit-tables.mjs` · `.meta.md` | — | **판정 불요** | 신규 c7 산출물이며 이미 `status: current`. 결함 0건, 실행 검증 완료(Y-2~Y-5) |

### 10.6 게이트 영향 — **0 / 8 PASS 불변**

이 루프는 **어떤 게이트도 올리지 않는다.** 문서 수준 개선만 있었고 §7의 막는 요인 ①~④ 중 ③(빌드 0줄·표본 n=0·시뮬 0회·캡처 0건)은 그대로다.

| 게이트 | 변화 | 남은 차단 |
|---|---|---|
| #g1 | 없음 | C6-F17 · 감사 A25·A29·A42 |
| #g2 | 없음 | 시뮬 0회 · `offerMaxPerSession` 미정 |
| #g3 | 없음 | 지불의사 n=0 · C6-F2 |
| #g4 | 없음 | C4-F7 · C4-F11 · RFC-S6 |
| #g5 | **악화 없음 · 해소 없음** | C7-F13 **open 유지**(S3) · C7-F3 · C4-F17 |
| #g6 | **문서 모순 3건 감소**(C7-F7 세이브 재생 단위 단일화 · C7-F6 레이아웃 · C6-F13 파이프라인) — **게이트는 여전히 FAIL** | C4-F6(RFC-S3 미판정) · **C7-F1(S1)** · Unity `Assets/` 0파일 · 배치모드 0회 |
| #g7 | 없음 | n = 0 |
| #g8 | `freshness-check.sh` 0 finding / **106** artifacts · exit 0 (**PARTIAL 유지**) | `mex` 미실행 `[SKIPPED]` · `graphify` `[UNGRAPHED]`(코드 0줄) · memory_sync 미검증 |

**열린 S1 = 1건(C7-F1) 유지 → 어떤 게이트도 PASS 불가.**

### 10.7 브로드캐스트 · `feedback-requested-by: 2026-09-11`

| 받는 레인 | 항목 | 요청 |
|---|---|---|
| **game-systems-designer** | **C7-F35(최우선 S2)** · C7-F33 · C7-F34 · C7-F36 · C7-F37 | `Q` 충돌을 먼저 닫는다 — (a) `zones.md` L53의 `Q`/`E`를 `interaction-rules.md` §1 L29 정본(`Tab`/`Shift+Tab`+`Enter`)으로 정정하고 `Q`를 조회 전용으로 확정하거나, (b) 조회에 제3의 키를 주고 `matrix[19]`·브리프 3곳을 다시 맞춘다. 어느 쪽이든 **세 문서와 `matrix[19]`가 한 문장을 말해야** 한다 · §⑩-2 #0 게이트를 최상위 키 계수로 교체 · `architecture-contract.md` §2에 `Tide.EditorTools` 행 추가 |
| **game-production-director** | **C7-F13(잔여)** · C7-F12 · C7-F16 · C7-F1(유지) · C4-F12 배정 | `handoff/asset-runbook.md` 편집 조정을 판정한다 — 런북 L12가 모델러에게 리소스 파이프라인 절 작성 권한을 이미 주므로 **누가 언제 §1.1·§4.4를 고치는지**만 정하면 닫힌다(요구: §1.1 L44에 경고 1줄 + §4.4 표 머리에 「`modeling/pipeline.md` §14.3 선행」 1줄). C4-F12를 systems에 배정할지 결정 |
| **game-modeler** | C7-F13(잔여) · C7-F3 | 편집권이 확정되면 런북 2줄 + (선택) `scripts/gen-2d.sh` FORCE 경로 패치. §14는 이미 서 있으므로 추가 설계 불요 |
| **game-planner** | C6-F19(우선) · C6-F22 · C6-F20 · C6-F21 · C6-F24 · C6-F29 · C6-F30 | 본 재검증은 `required_fix` 기준 판정을 선례로 세웠다(§10.4). C6-F19는 실측이 이미 나와 있어 한 줄로 닫힌다 |
| **game-worldview-architect** | RFC-S6(유지) | 용어집 등재가 `interaction-rules.md` 승격을 계속 막고 있다 |

### 10.8 이 절이 주장하지 않는 것

- **게임이 존재한다고 주장하지 않는다.** Unity 실행 0회 · 빌드 0줄 · `Assets/` 파일 0개.
- `47/47 PASS`와 `emit-tables.mjs` exit 0은 **저작 JSON의 구조 검사와 그 파생 생성**이다. Unity 임포트·런타임 불변식은 여전히 **0건 검증**이다.
- `beats.json`의 sha 일치는 **파이프라인이 정의됐다**는 증거이지 **데이터가 옳다**는 증거가 아니다.
- 브리프 §③-1a 패키지 절차는 **한 번도 실행되지 않았다**(브리프 자신이 `[INFERENCE]`로 표기). 4/4 설치는 여전히 **미측정**.
- C7-F33은 설치 후 실측이 아니라 **락 파일 구조에서 파생한 [INFERENCE]**다. 설치 1회로 확정된다.
- **`freshness-check.sh` exit 0은 시점 신선도·memory_sync를 검증하지 않는다**(도구 자기 고백 그대로).

---

## 11. 재검증 2 — C6/C7 수정 루프 2 (2026-09-10)

**대상**: 재검증 1이 새로 연 **C7-F35 (S2 · systems)** 1건.
**방법**: 레인 보고는 상태가 아니다. `required_fix`가 요구한 두 조건 — ① **세 문서와 `matrix[19]`가 한 문장을 말하는가** ② **채택 근거의 `[OBSERVED]` 문장이 실측으로 정정됐는가** — 를 파일 재독과 명령 재실행으로 각각 판정했다. 레인이 `[OBSERVED]`로 적은 값은 **QA가 같은 명령을 다시 돌려 같은 출력이 나오는 것만** 인정했다.
**이 절이 올리는 게이트**: **없다.** Unity 실행 0회 · 빌드 0줄 · **키 입력 실측 0건** · 패드 실측 0건 · 사람 플레이 표본 n = 0. `matrix[19]`는 이번 루프에서도 **미실행**이다 — 문안이 넓어진 것이지 돌아간 것이 아니다.

### 11.1 재검증 명령 [OBSERVED 2026-09-10]

| # | 명령 / 대상 | 결과 |
|---|---|---|
| Z-1 | `git status --short \| wc -l` · 삭제 계수 | **62행 · 삭제(`D`) 0건**. 재검증 1의 Y-17과 동일 — 타 세션 작업 무손상 |
| Z-2 | `grep -rn '`Q`' _workspace/current/ \| wc -l` | **63행** (systems 49 · qa 8 · handoff 6). systems 영수증 §1.1의 **편집 전 30행**은 이 파일들이 **git 미추적**이라 되돌려 재현할 수 없다 → `[CARRIED]`, QA는 인정도 반박도 하지 않는다 |
| Z-3 | `grep -rn '`Q`' _workspace/current/systems/data-schemas/` | **3행** — `zones.md` L53(정본 **인용**) · L59(**부정** 문장) · L116(§7 변경로그 **서술**). 셋 다 키를 **배정하지 않는다** → 「스키마 배정 0건」 주장은 **독립 재현 성립** |
| Z-4 | `grep -rnE '`[A-Z]`\|`(Tab\|Enter\|Space\|Esc\|Delete\|F1\|RS\|LB\|RB\|LT\|D-Pad\|back)`' _workspace/current/systems/data-schemas/` | **3행**. `zones.md` L58 괄호가 `[OBSERVED]`로 적은 「**2행**」과 **불일치**(§7 변경로그 L116 누락) → §11.3 **C7-F38** |
| Z-5 | `grep -rn '`E`' _workspace/current/systems/ _workspace/current/handoff/ \| wc -l` | **28행 / 7파일**. `zones.md` L59 괄호가 `[OBSERVED]`로 적은 「이 행의 부정 문장 **1건 외 0건**」과 **불일치** → §11.3 **C7-F38** |
| Z-6 | `interaction-rules.md` §1 **L29** · §1-3.2 **L142**(`Q` 행) · **L145**(불릿) 재독 | 「노드 이동 = `Tab`/`Shift+Tab` 초점 + `Enter`(패드 좌스틱 + `A`)」 · 「`Q` @ `Shell` = 발행할 명령 0개 — **시점 노드 이동이 아니다**」 · 「`E`는 어떤 표면에도 배정이 없다」. **세 자리 상호 모순 0** |
| Z-7 | `data-schemas/zones.md` **L53** `neighbors` 재독 | 「**순서만** 결정 · 입력 정본은 `interaction-rules.md` §1 · **`Q`/`E`가 아니다**」. 스키마가 키를 **배정하지 않는 형태**로 바뀐 것을 확인 |
| Z-8 | `python3 json.load game-ui-contract.json` | 최상위 **13키** · `verification.matrix` **20행** · `decisions` **13**. 재검증 1의 Y-14와 **동일** — 행 추가·삭제 0건 |
| Z-9 | `matrix[19].expected` 재독 | 「… 셸에서 Q는 아무 명령도 발행하지 않고 **셸의 시점 노드 이동은 탭과 시프트 탭 초점 이동 후 엔터로만 이뤄지며 Q나 E로는 이뤄지지 않는다**」 — 인수 조건이 **어느 쪽을 검사하는지 판정 가능**해졌다 |
| Z-10 | `python3 ~/.claude/skills/game-ui-ux/scripts/validate-game-ui.py game-ui-contract.json` | `PASS: valid game UI contract` · **exit 0** |
| Z-11 | `shasum -a 256` × 5 ↔ `game-ui-contract.meta.md` 해시 표 ↔ `c6-c7-fixloop2-systems.md` §1.4 | **5/5 해시·바이트·문자 수 문자 일치**. `unity-implementation.md`(`7d0a9afa…` · 15,421 B)는 **불변** — 「건드리지 않았다」는 주장 성립 |
| Z-12 | `handoff/codex-unity-brief.md` **L493**(⑦-1) · **L517**(`K-9`) · **L647**(`T-26c`) · **L811**(⑫ English 6) 재독 | 네 곳이 같은 문장. **영문 요약까지 정합**된 것을 확인 |
| Z-13 | frontmatter 7파일 전수 | `updated: 2026-09-10` 전건 · `cycle` **불변**(`zones.md` c3 · `interaction-rules.md` c5 · `game-ui-contract.meta.md` c5 · `tech-verification/README.md` c3) · 신규 `c6-c7-fixloop2-systems.md`만 c7 · `status` 변경 0 → **RFC-Q2 준수** |
| Z-14 | `interaction-rules.md` 잔여 차단 요인 재측정 | RFC-S6 **미등재 4건** 잔존(L210~L214 표 · L349) · C7-F27 「장·**일차**·조위 위상」 **L305 잔존**(R8 편집으로 L299 → L305 이동) · `status: draft` |
| Z-15 | `c6-c7-fixloop1-systems.md` L141~L142 | 거짓 `[OBSERVED]`가 **취소선 + 정정 주석**으로 남아 있고 **삭제되지 않았다**. 기록 보존 규칙 준수 |

### 11.2 C7-F35 판정 — **closed**

| `required_fix` 조건 | 판정 | 근거 |
|---|---|---|
| (a)안 채택 시 `zones.md` L53의 `Q`/`E`를 §1 정본으로 정정 | **이행** | Z-7 |
| `Q`를 도구 패널 조회 전용으로 확정 | **이행** | Z-6(`Q` 행) · Z-3 |
| **세 문서 + `matrix[19]`가 한 문장** | **이행 — 실제로는 5곳** | Z-6(§1 L29 · §1-3.2 L142/L145) · Z-7(`zones.md`) · Z-9(`matrix[19]`) · Z-12(브리프 4곳, 영문 요약 포함) |
| 채택 근거의 `[OBSERVED]` 문장을 실측으로 정정 | **이행(취소선 + 정정)** | Z-15 · `interaction-rules.md` L128~L130 |

**추가로 QA가 인정하는 것**: 레인이 (b)안(제3의 키 신설)을 기각한 이유 — 「**미사용 키를 고른다는 절차 자체가 이번 결함의 원인**」 — 은 **수용한다**. 키를 바꾸는 대신 「**데이터 스키마는 순회 *순서*만 소유하고 *키 배정*은 소유하지 않는다**」는 경계를 `zones.md` §2에 명문화한 것이 이 루프의 실질적 재발 방지책이며, QA는 Z-3으로 그 경계가 실제로 지켜지는지(배정 0건)를 독립 확인했다. **counter 없음.**

**C7-F35 → `closed`.** 단, 아래 §11.3이 같은 편집에서 새 결함을 연다.

### 11.3 신규 결함 — **C7-F38 (S3 · systems)**

| id | sev | 레인 | 증거 | 요지 |
|---|---|---|---|---|
| **C7-F38** | **S3** | systems | Z-4 · Z-5 ↔ `data-schemas/zones.md` L58·L59 괄호 ↔ `tech-verification/c6-c7-fixloop2-systems.md` §1.2 | **C7-F35를 닫은 편집이 자기 검증 영수증에 재현되지 않는 `[OBSERVED]`를 새로 넣었다.** ① `zones.md` L58: 「`data-schemas/` 키 토큰 = **2행**」 → 실측 **3행**(§7 변경로그 L116 누락). ② `zones.md` L59: 「`grep -rn '`E`' systems/ handoff/` = 이 행의 부정 문장 **1건 외 0건**」 → 실측 **28행 / 7파일**. ③ 같은 값이 `c6-c7-fixloop2-systems.md` §1.2 표에도 「2」로 박혀 있다 |

- **두 문장의 *결론*은 참이다.** 「스키마의 키 **배정** 0건」(Z-3) 과 「`E`는 어떤 표면에도 **배정**이 없다」(Z-6)는 QA가 독립 재현했다. 거짓인 것은 **인용된 명령의 출력값**이다 — 즉 결론이 아니라 **영수증**이 틀렸다.
- **왜 결함인가**: C7-F35가 지적한 결함 종류는 「재현되지 않는 `[OBSERVED]`를 채택 근거로 세우는 것」이었다. 그 해소 편집이 **같은 종류를 두 줄 더 만들었다.** 게다가 `zones.md`는 `status: current`이며 브리프가 Codex에게 **착수 전 필독**으로 지정한 스키마 문서다 — 실행자가 그 괄호를 그대로 믿고 명령을 돌리면 즉시 불일치를 본다.
- **심각도 근거**: 선례 C7-F36(같은 종류, S4)보다 한 단계 올린다. 이유는 (a) 위치가 검증 로그가 아니라 **실행자가 읽는 `status: current` 스키마 본문**이고, (b) 이 문장이 **S2 해소의 검증 근거 자체**여서 closure의 신뢰도를 직접 깎기 때문이다. 디렉터가 C7-F36과의 형평을 우선한다면 S4 하향은 합리적이며 QA는 반대하지 않는다.
- **`required_fix`**: 두 괄호를 실측값(**3행** · **28행 / 7파일**)으로 교체하거나, **행 수 인용을 빼고 「배정 0건」이라는 판정만 남기고** 그 판정의 재현 절차(행 수가 아니라 **문장 성격** — 인용/부정/서술 vs 배정 — 으로 판정한다)를 적는다. `c6-c7-fixloop2-systems.md` §1.2의 「2」도 같이 고친다. 되돌릴 수 없는 **편집 전 수치(30 / 22 / 1 / 6 / 1)** 는 파일이 **git 미추적**이라 재검증이 불가능하므로 `[CARRIED]`로 표기한다(Z-2).

### 11.4 레인 `open_questions` 처리

| 레인 질문 | QA 판정 |
|---|---|
| `Q`의 물리적 타당성 — 비-QWERTY 배열(AZERTY `A` 자리 · Dvorak `'`)에서 「왼손 조회」 배치 의도가 성립하는가, 재매핑 UI가 `Q`를 다루는가 | **유효 · 미해소 유지.** C6 검토에서 이미 「유효 · 미해소」로 남긴 항목이며 이번 루프도 닫지 못했다. systems 판단 축이 아니라는 레인 의견에 **동의**한다 → **presentation(UI/UX) + planner(현지화)** 로 브로드캐스트. 다만 `interaction-rules.md` L131이 이미 「재매핑 대상이다」를 명시하므로 **새 결함으로 열지 않는다** |
| `E`를 비워 둘 것인가, 조회 역방향(패널 닫기)에 예약할 것인가 | **지금 결정하지 않는 것이 옳다 — 단 절차를 바꾼다.** 「미사용이라 안전」이라는 근거로 키를 고르는 절차가 이번 결함의 원인이므로, `E`를 **예약**하는 것도 같은 절차의 반복이다. QA 권고: 키가 필요해지는 **기능이 먼저 정해진 뒤에만** 배정하고, 배정 시 §11.5의 상설 대조를 통과시킨다. 결함 아님 |
| R7의 grep이 0을 낸 원인 미조사 — 같은 종류 재발을 QA 정기 대조로만 잡을 수 있다. C7 종료 체크리스트 상설 항목으로 넣을지 | **동의 · 디렉터 판정 요청.** 원인 미조사는 **정직한 처리**로 인정한다(추정을 적지 않았다). 상설 항목화는 QA가 단독으로 결정할 사안이 아니므로 §11.7로 디렉터에 올린다. QA 제안 문안: **「`status: current` systems 문서 셋에서 한 입력 토큰이 두 곳 이상에 나타나면, 그중 *배정*이 2건 이상인지 문장 성격으로 판정한다」** — 행 수 계수가 아니라 성격 판정이어야 한다(C7-F38이 보여준 대로 행 수는 서술문에 오염된다) |

### 11.5 승격 판정 갱신

| 대상 | 이전 | 이번 | 사유 |
|---|---|---|---|
| `systems/interaction-rules.md` | 차단 | **차단(유지)** | C7-F35 해소로 사유 (a) 제거. 그러나 **RFC-S6 미판정**(표시명 4건 미등재 · Z-14) · **C7-F27 잔존**(L305 「장·일차·조위 위상」) · `status: draft` 가 남는다 |
| `systems/data-schemas/zones.md` | — | **차단(신규 판정)** | **C7-F38 open.** `status: current`이면서 본문 괄호 2개가 거짓 `[OBSERVED]`다 |
| `systems/tech-verification/c6-c7-fixloop2-systems.md` | — | **차단(신규 판정)** | §1.2 표의 「2」가 C7-F38에 걸린다. §1.1 편집 전 수치는 재검증 불가 `[CARRIED]` |
| `systems/game-ui-contract.meta.md` | 차단 | **차단(유지)** | meta 자체 결함 0 · **해시 5/5 문자 일치**(Z-11). 기술 대상 `game-ui-contract.json`의 **C4-F12 open** |
| `handoff/codex-unity-brief.md` | 차단 | **차단(유지)** | **C7-F1(S1) open**. 이번 편집(⑦-1·K-9·T-26c·EN 6)은 결함 0건이나 착수 차단 요인이 그대로다 |
| `systems/tech-verification/c6-c7-fixloop1-systems.md` | — | **차단(유지)** | **C7-F36 open**(L100 거짓 `[OBSERVED]`). L141 취소선 처리는 올바르나 별건 |

**이번 루프의 신규 승격 = 0건.**

### 11.6 게이트 영향 — **0 / 8 PASS 불변**

| 게이트 | 변화 | 남은 차단 |
|---|---|---|
| #g4 | **없음** — 입력 계약 문서 모순 1건 감소는 몰입 측정이 아니다 | C4-F7 · C4-F11 · RFC-S6 |
| #g6 | **문서 모순 1건 감소**(C7-F35). **게이트는 여전히 FAIL** | C4-F6 · **C7-F1(S1)** · Unity `Assets/` 0파일 · 배치모드 0회 |
| #g1·#g2·#g3·#g5·#g7 | 없음 | 불변 |
| #g8 | 이번 루프에서 재측정하지 않음 → 재검증 1의 Y-16 값 `[CARRIED]` | `mex` `[SKIPPED]` · `graphify` `[UNGRAPHED]` · memory_sync 미검증 |

**열린 S1 = 1건(C7-F1) 유지 → 어떤 게이트도 PASS 불가.**

### 11.7 브로드캐스트 · `feedback-requested-by: 2026-09-11`

| 받는 레인 | 항목 | 요청 |
|---|---|---|
| **game-systems-designer** | **C7-F38(신규 S3)** | `zones.md` L58·L59 괄호 2개와 `c6-c7-fixloop2-systems.md` §1.2의 「2」를 실측값으로 교체하거나 행 수 인용을 판정 문장으로 바꾼다. 편집 전 수치는 `[CARRIED]` 표기 |
| **game-production-director** | ① C7-F38 심각도(S3 ↔ C7-F36 형평 S4) ② **종료 체크리스트 상설 항목화** (§11.4 3번) ③ C7-F35 closed 반영 | ②의 QA 제안 문안은 §11.4에 있다. **행 수 계수가 아니라 문장 성격 판정**이어야 한다 |
| **game-presentation-director** | `Q`의 물리 배열 타당성(§11.4 1번) | 비-QWERTY에서 「도구 패널 왼손 조회」 배치 의도가 성립하는지, 재매핑 UI가 `Q`를 다루는지 |
| **game-planner** | `Q`의 현지화·배열 영향(§11.4 1번) | AZERTY/Dvorak 사용자 기본 배정 정책 |
| **game-worldview-architect** | RFC-S6(유지) | 표시명 4건 미등재가 `interaction-rules.md` 승격을 계속 막는다(Z-14) |

### 11.8 이 절이 주장하지 않는 것

- **`matrix[19]`는 미실행이다.** 「`Shell`에서 `Q`·`E`가 노드를 이동시키지 않는다」는 **설계 주장**이며 관측이 아니다. 이번 루프에서 눌린 키는 **0개**이고 Unity 실행은 **0회**다.
- **문서 5곳이 한 문장을 말한다 ≠ 빌드가 그렇게 동작한다.** Z-6·Z-7·Z-9·Z-12는 전부 **문서 대조**다.
- **`validate-game-ui.py` exit 0은 계약 JSON의 구조 검사**이며 UI가 존재한다는 증거가 아니다.
- **편집 전 수치(30 / 22 / 1 / 6 / 1)를 QA가 확인했다고 주장하지 않는다.** 대상 파일이 git 미추적이라 되돌릴 수 없다 → `[CARRIED]`.
- **R7의 grep이 0을 낸 원인을 QA도 조사하지 않았다.** 레인과 동일하게 **재현 실패**만 기록한다.

---

## 12. 재검증 3 (2026-09-10, R7) — 종료 수정 회차 최종 재검증

> **대상**: 디렉터 R7 배정 전건(열린 S1/S2 23건 + 디렉터 처리분 3건 + C3-F31 + RFC-S6 등재 + C7-F38).
> **방법**: 파일 재열람 + 명령 재측정. **재현되지 않는 `[OBSERVED]` 는 결함으로 연다**(C7-F38 유형) — 이 절의 모든 값은 아래 R7-1~R7-23 명령을 그대로 실행해 얻었다.
> **이 회차가 올린 게이트는 0개다.** Unity 실행 **0회** · 빌드 **0줄** · 키 입력 **0건** · 사람 플레이 표본 **n = 0** · 성능 캡처 **0건**.
> **QA 는 이 회차에 `_workspace/current/qa/` 밖의 파일을 쓰지 않았다.**

### 12.1 재측정 명령과 값 [OBSERVED 2026-09-10 R7]

| id | 명령 | 값 |
|---|---|---|
| **R7-1** | `git status --short` | 미커밋 변경 목록 확인. QA 산출물 3종(`c6-review.md`·`defect-register.md`·`gate-measurements.md`) 외 이 세션의 QA 쓰기 **0건** |
| **R7-2** | `node _workspace/current/planning/validate-campaign.mjs` | `{checks 49, pass 49, fail 0, verdict PASS}` · `sha256 8a43d334…` · **124,007 B**. 검사 47 → **49**(신설 `H-04`·`Z-03`) |
| **R7-3** | `node _workspace/current/planning/validate-campaign.mjs --t0` | `{checks 5, pass 5, fail 0, verdict PASS}` · `T0-01`~`T0-05` · `sourceShaMatchesLiveCampaign: true` |
| **R7-4** | `bash .claude/skills/game-ops-harness/scripts/freshness-check.sh` | `0 finding(s) across 118 markdown artifact(s)` · **exit 0**. 원문 3줄은 `qa/gate-measurements.md#g8` |
| **R7-5** | `node .claude/skills/game-ops-harness/scripts/validate-preproduction.mjs` | `status: FIX` · `runtimeStatus: NOT-MEASURED` · `passed 514` · **errors 3**(`production/cycles/c6-development.md` · `qa/c7-review.md` · `production/cycles/c7-development.md`) → §12.3 **C7-F43** |
| **R7-6** | `node _workspace/current/systems/prototype/test-model.mjs` | **37 통과 / 0 실패** · exit 0. **참조 모형이지 Unity 빌드가 아니다** |
| **R7-7** | `python3 ~/.claude/skills/game-ui-ux/scripts/validate-game-ui.py _workspace/current/systems/game-ui-contract.json` | `PASS: valid game UI contract` · exit 0. **구조 검사이며 UI 존재 증거가 아니다** |
| **R7-8** | `node _workspace/current/systems/pipeline/emit-tables.mjs --scope t0 --out <scratch>` → `shasum -a 256` 5쌍 대조 | **5/5 MATCH**(`beats`·`hints`·`records`·`tools`·`zones`). QA 가 **독립 실행**해 디스크 사본과 바이트 일치를 확인했다 → **C7-F1 결정론 주장 재현 성공** |
| **R7-9** | `awk` gdd §8 표 행 수 ↔ `python3` 계약 `accessibility` 키·`gdd_coverage` | gdd **13행** ↔ `gdd_coverage` **13행** · `accessibility` 키 **15** · 미대응 **0** |
| **R7-10** | `python3` — `hints[0]` 33건에 대한 **QA 독립 금지어휘 스캔**(자료명·수치·정답 단정) | **0건**. 검증기 `H-04` 구현을 열어 어휘 목록·한계 주석(`어휘 바닥`)까지 확인 |
| **R7-11** | 검증기 `Z-01`·`Z-02`·`Z-03` + `python3` 4비트 직접 조회 | `c2-b4`·`c6-b4` = `hub` ∈ 스테이지 `zoneIds`(C2 `[gate,pump,hub]` · C6 `[pump,hub]`) · `c1-b2` `hub` · `c5-b2` `dock` — **전건 포함** |
| **R7-12** | `zones.md` §2.1 의 `awk` 배정 행 명령 그대로 (`E` 판 / `Q` 판) | **`E` 0행**(출력 없음) · **`Q` 2행**(`system-specs/tide-alignment.md:31` · `handoff/codex-unity-brief.md:350`) — 문서가 적은 값과 **문자 일치** |
| **R7-13** | `grep -rn "일차" systems/ handoff/ planning/` | **4행 / 4파일**. 그중 **3행이 살아 있는 UI 라벨 규정**(`interaction-rules.md:315` · `game-ui-contract.json:360·396`)이고 1행은 그것을 **금지**하는 브리프(`codex-unity-brief.md:557`) → §12.3 **C7-F40** |
| **R7-14** | `grep -rn "RFC-P4-001" _workspace/current` | **14행**. 12행 = Mixamo 적용 범위(decision-log 정본), **2행 = `planning/feature-specs/recap-panel.md` 가 「인용 고정 저장 필드」라는 다른 주제로 같은 id 를 연다** → §12.3 **C7-F39** |
| **R7-15** | `python3` — 조작 밀도 두 분모 재계산 | 비트 **21 / 33 = 63.6 %** · 하위과제 **35 / 149 = 23.5 %**. `campaign.json` 에 하위과제별 활동 분류 필드는 **없다** → 「**61 %**」는 **어느 분모로도 재현되지 않는다** → §12.3 **C7-F41** |
| **R7-16** | `balance/balance-sheet.md` §7 의 `node -e` 3종을 그대로 재실행 | `[4,5,5,4,4,5,7,4,3]` · 스테이지별 `2/2 3/2 3/2 2/2 2/2 3/2 4/3 2/2 2/1` · 단일 비트 최대 부하 `4 5 5 4 4 5 7 4 3` — **포락 = 실부하 재현 성공**(C3-F31 해소 근거 성립). 같은 절의 sha·바이트·검사 수는 **재현 실패** → §12.3 **C7-F42** |
| **R7-17** | `python3` — 등록부 전 행 엄격 파싱(id·severity·status) | 행 **148** · 중복 id **0** · C3 36/closed 32 · C4 **22**/closed **12** · C5 11/closed 9 · C6 41/closed 4 · C7 **38**/closed **7** |
| **R7-18** | `scripts/regen-cycle-ledger.py` 의 정규식·파서를 **읽기 전용으로 복제 실행** | 생성기가 보는 행 **144**(등록부 148 중 **4행 조용히 탈락**: `| **C7-F35** |`·`| **C7-F38** |` = 굵은 id, `C4-F1`·`C4-F2` = severity 셀이 `S1(1차 blocker)`). 그 결과가 현 `cycle-ledger.json` 과 **완전 일치** → 대장은 충실히 재생성됐고 **틀린 것은 생성기다** → §12.3 **C7-F44** |
| **R7-19** | `grep -o` on `presentation/steam-game-plan.html` | `reviewed-and-revised` **5건** · `fix-in-progress` **0건**. 19번 슬라이드 표 = C3 closed **27** · C4 **22/9** · C5 **11/6** ↔ R7-17 실측 **32 / 22·12 / 11·9** → **C5-F2 open 유지** |
| **R7-20** | `shasum -a 256 docs/media/verb-seal.jpg` + `stat` mtime 대조 | 파생본 `c71796…` (provenance 기재값과 동일) · mtime **02:56:08** ↔ 재생성 원본 `readme-verb-seal.png` mtime **08:57:23** — **파생본이 원본보다 6시간 이르다** → **C4-F11 open 유지** |
| **R7-21** | `grep -c "RFC-{id}" production/decision-log.md` 10종 | C6-001 **1** · C6-002 **1** · S2 **2** · S3 **1** · S5 **1** · S6 **1** · B6 **1** · M3 **1** · CX **1** · **Q3 0** → **C6-F7 축소 open**(1/10 미개설) |
| **R7-22** | `wc -c` · `shasum -a 256` on `presentation/steam-game-plan.html` · `generate-deck.mjs` | **100,806 B** / `585f4a04…` · **109,579 B** / `3b2ef9e2…` — `steam-game-plan.meta.md` §산출물 해시 표와 **2/2 문자 일치** |
| **R7-23** | `grep -c "^| {표시명} " worldview/glossary.md` 6종 + §3-1 표 | 배선 추적 1 · 판독 1 · 조위정합 1 · 배수 편성 1 · 부식 시험 1 · 이중서명 2 → **6/6 등재** · §3-1 이 id ↔ KO ↔ EN 을 1:1로 잇는다. `screens[9].states` = 정본 6종과 문자 일치 |

### 12.2 배정 전건 판정

**closed 26건.**

| 결함 | 등급 | 판정 근거 (재측정 id) |
|---|---|---|
| **C7-F1** | **S1** | **closed.** `systems/data/t0/{beats,hints,records,tools,zones}.json` + `.meta.md` 존재, `--t0` 검사 **5/5 PASS**(R7-3), QA 독립 재생성 **5/5 바이트 일치**(R7-8). 값은 발명이 아니라 `campaign.json` t0 서브셋·`style-guide.md` §5 카메라·`worldview-bible.md` 파생이며 `sourceShaMatchesLiveCampaign: true` 가 계보를 건다 |
| **C7-F8** | S2 | **closed.** `T0-01` 이 `commitCommandBeats = ["t0-b3"]` 과 `completion` 비어 있지 않음을 기계 검사(R7-3). 초안 §6.1 이 세 비트 완료 술어를 표로 세우고 DoD 6·8 적용 범위를 명시 |
| **C6-F10** | S2 | **closed.** `tools.json` `reader.hasCommit = true` · `commandId: CiteToBoard`(`T0-05`). 초안 §6.1 이 확정 명령을 `reader` 인용 고정으로 확정하고 `T-25`·`T-27` 의 T0 범위 밖 판정을 함께 적었다 |
| **C7-F5** | S2 | **closed.** 용어집 §3 에 4건 신설 + §3-1 id↔KO↔EN 표(R7-23). 브리프 `I-9`→`R-1` 완화(KO 필수·EN 선택, fail-closed 는 **용어집 등재 하나뿐**), `T-12` 동반 완화 |
| **C4-F9** | S3 | **closed.** 표시명 6/6 한 벌 + 용어집 등재 완료(R7-23). `부식예산` 은 자원 이름 자리에만 남고(`game-ui-contract.json` L486 「부식예산 게이지」) 도구 이름 자리에서 사라졌다 |
| **C7-F4** | S2 | **closed.** `unity-implementation.md` §2 를 7분할로 재작성(생산 7 + 테스트 2 + EditorTools = asmdef 10). `handoff/README.md` §1 이 `status` 열을 명시하고 current 13행을 draft 2행 위에 세운다 |
| **C7-F14** | S2 | **closed.** 브리프 §⑤-7 이 `ReasonCode` 를 **실패 모드 행 → 코드** 1:1 표로 도출하고 KO 문장을 `strings/ko.json` 한 곳으로 몰았다. DoD #5 = 「집합 일치 + 문자열 파일 존재」 |
| **C7-F12** | S2 | **closed.** 런북 3곳(L92·L179·L381) + 영문 요약 L391 이 「실행자는 decision-log 를 쓰지 않는다」로 통일. `handoff/rfc-inbox/README.md` 신설 |
| **C7-F3** | S2 | **closed.** 런북 §3.3-0 = 「GLB 정본 · FBX 는 허브 셸 1개 · `SM_Tool_*.fbx` 만들지 않는다」. 실측 GLB **7** · FBX **1** · blend **1** 로 판정과 산출물이 일치(추가 내보내기 0건) |
| **C7-F38** | S3 | **closed.** `zones.md` §2.1 이 「등장 횟수」를 **「배정 행 수」로 교체**하고 재현 명령을 본문에 넣었다. QA 재실행 결과 `E` **0행** · `Q` **2행** 으로 문서와 문자 일치(R7-12). `c6-c7-fixloop2-systems.md` §1.2 도 정정 |
| **C4-F12** | S2 | **closed.** `accessibility{}` 가 gdd §8 **13행 전건**을 덮고 `gdd_coverage` 가 행↔키를 1:1로 잇는다(R7-9). 미커버 3행(색약 3팔레트 · 3채널 볼륨 · 이산 대안)이 각각 `color_vision_palettes`·`audio_channel_volumes`·`discrete_alternatives` 로 신설 |
| **C4-F20** | S3 | **closed.** 지속시간 분기 3건이 모드 분리/모디파이어로 이동(`RB` 다음 · `LB`+`RB` 이전 / 확정은 `two-step` 기본이고 0.4 s 는 `hold` opt-in 데이터 노브). 「누름 시간은 해소 방식이 아니다」를 §0-11 에 명문화 |
| **C4-F6** | S2(open-rfc) | **closed.** RFC-S3 판정(제거 승인·마이그레이션 대상 아님) 적용. `unity-implementation.md` §7 조각이 `save.md` §1·§3 과 맞춰졌고 `chapter`/`dayIndex`/`eventSeq` 가 사라졌다. **잔여 UI 라벨 「일차」는 C7-F40 으로 분리** |
| **C4-F7** | S2 | **closed.** `vfx/vfx-budget.md` §1~§2 가 `seal_confirm`·`water_rise`·`brine_flow` 를 **영수증 이후 발행 / 저장 실패 시 미발행**으로 규정하고 상태명을 §5 다섯 줄 + `SavePending` 으로 고정. `motion/motion-contract.md` §2 가 옛 「상태가 먼저 확정」을 폐기하고 §3 에 `SavePending` 진행 표시 규격(120 ms 지연·400 ms 최소·1,500 ms 문장)을 세웠다 |
| **C6-F1** | S2 | **closed.** `t0-b1.objective` 에 「이 당직이 끝나면 남는 것은 **청문에 낼 제출 문서 1건**」이 데이터로 들어갔다(R7-11 인접 조회). 초안 §2.6 이 그것을 공개 상한 표의 「해도 되는 것」 열에 올린다 |
| **C6-F3** | S2 | **closed.** 검증기 **`H-04` 신설**(자료·값·단정·조작 지시 4부류 금지, 어휘 바닥 한계를 주석으로 고지) + `hints[0]` 전수 재작성. QA 독립 스캔 **0건**(R7-10) |
| **C6-F4** | S2 | **closed.** `planning/feature-specs/recap-panel.md` 신설 — 4블록(마지막 체크포인트·현재 목표·열린 가설·최근 인용 고정) · R1~R11 · E1~E8 · A/B 인수 분리 · `deps` 4건 공개. `gdd.md` §7 이 슬롯 카드와 재개 요약을 구분 |
| **C6-F16** | S2 | **closed.** 초안 §2.6 이 T0 공개 상한·캐논 시각·순서 앵커를 **허용/금지 2열**로 세우고 값 소유를 worldview 로 남겨 재기재를 피했다 |
| **C6-F17** | S2 | **closed.** 스테이지 `zoneIds` 확장 + 검증기 `Z-03`(본문 구역 ∋ `zoneId`) 신설, `Z-01`·`Z-02`·`Z-03` 전건 PASS(R7-11). G1 유일 violation 소멸 |
| **C6-F2** | S2 | **closed.** 정본 문구가 초안 §0·§2.3 · `business-model.md` §1 · `assumption-tests.md` L13 에 같은 문장으로 서 있다. 「분기·멀티 엔딩」 금지와 「제출 관점」 개명이 §2.3 주석에 명문화. 루트 README 는 옛 문구를 쓴 적이 없다(`grep` 0건) |
| **C6-F5** | S2 | **closed(위험 등록으로).** 디렉터 판정대로 문서로 닫지 않고 **R-T0-1** 로 등록(초안 §11.3 · `balance-sheet.md` §10.4) + 관측 지표 `H-10 manipulation_share`·`H-11` 신설 + 텔레메트리 `tool_panel_active_min` 신설(판정선 없음 명시). **다만 판정문 「61 %」는 재현 불가 → C7-F41** |
| **C6-F6** | S2 | **closed.** `business-model.md` §1-1 포지셔닝 한 문장 + §2 가격 3숫자(14,900 / 17,500 / 19,900, 「미승인」) + `market-decision.md` 인용. 초안 §8 이 숫자를 재기재하지 않는 것은 단일 출처 규율이며 결손이 아니다 |
| **C6-F8** | S2 | **closed.** `business-model.md` §9-1 에 위험 2행 신설. 「Steam 생성형 AI 공개」는 접속 미확인이라 `[TARGET] 확인 과제`로, 「라이선스 UNVERIFIED」는 `assets/generated/{previz,video}/provenance.json` 실측 인용으로 `[OBSERVED]` — **QA 가 그 3행을 열어 문자 일치를 확인했다** |
| **C6-F9** | S2 | **closed.** 계약에 「## Base production gate」 절이 신설되고 4조건이 한 곳에 있다. 초안 §6 머리 · 브리프 §① · `verification-plan.md` 머리글·§4-4 가 **인용만** 한다(재서술 0곳) |
| **C6-F11** | S2 | **closed.** 착수 전 결정 4건이 브리프 **§⑦-0** 에 「되묻지 않는다」 표로 확정(기준 HW 미정·PRE-1 / Input System new + `activeInputHandler=2` + 액션 맵 `Watch` / URP / asmdef 7). **초안 문자열은 여전히 0회이나** 판정 ⑥·`handoff/README.md` §1 이 실행자의 독서 대상을 `handoff/` 로 정했으므로 harm 이 소멸했다 — 초안 §12 색인에 `handoff/` 행이 없는 잔여는 **C6-F25**(디렉터, 별건)로 남는다 |
| **C3-F31** | S2(open-rfc) | **closed.** §7 이 미측정 열을 빼고 관측 2열로 재도출, 세 번째 열은 §7.2 `[TARGET]` 으로 분리. QA 가 §7 의 `node -e` 3종을 그대로 재실행해 `[4,5,5,4,4,5,7,4,3]` 과 **포락 = 단일 비트 실부하** 동일성까지 재현(R7-16). C5→C6 = **+2** 로 규칙 통과 |

**open 유지 4건 (범위 축소).**

| 결함 | 등급 | 남은 것 | 요구 |
|---|---|---|---|
| **C4-F11** | S2 | **파생본이 갱신되지 않았다.** concept 는 `readme-verb-seal` 재생성을 마쳤고(08:57, 100 % 확대 검수 통과) 컨셉 시트 §6·§9 도 「완료」로 갱신됐다. 그러나 **push-ready README 가 게시하는 `docs/media/verb-seal.jpg` 는 02:56 파생본 그대로**이며 sha 가 `docs/media/provenance.json` 기재값과 동일하다(R7-20) → **README 는 아직 §10-4 위반 이미지를 싣는다** | 디렉터: 파생본 재생성 + `docs/media/provenance.json` `output_sha256` 갱신 |
| **C6-F12** | S2 | **브리프는 이행, 초안은 미이행.** 브리프 §⑤-2·U-3·U-8·N-12 가 로그 상한과 `SV-F6` 접힘을 병기했다. `planning/game-draft-v1.md` 는 `되돌림 상한`·`로그 상한`·`SV-F6`·`byteCap` **전건 0회**(R7 grep) | 배정 레인 재지정 필요 — 초안은 planner 소유다. 초안 §4.5 또는 §11 에 「되돌림 무제한 ↔ 로그 크기 상한은 다른 축(`SV-F6` 접힘)」 한 줄 |
| **C6-F7** | S2 → **S3 하향 제안** | **10건 중 9건 개설, `RFC-Q3` 만 0건**(R7-21). Q3 = 「루트 `README.md` 의 소유 레인이 계약에 없다」 — 소유자가 없으면 README 스테일(C4-F11 이 지금 그 사례다)이 어느 레인의 회귀 대상도 아니다 | 디렉터: RFC-Q3 판정(README 소유 레인 지정 + 사이클 종료 체크리스트 항목화) |
| **C5-F2** | S2 | **대장은 충실히 재생성됐으나 생성기가 4행을 조용히 버린다**(→ C7-F44). 그리고 **덱 19번 슬라이드가 손으로 적은 옛 숫자를 렌더**한다 — C3 closed 27(실측 32) · C4 22/9(실측 22/12) · C5 11/6(실측 11/9) · C4·C5 상태 `reviewed-and-revised`(실측은 열린 S2 보유)(R7-19) | 디렉터: 생성기 수정(C7-F44) → 대장 재생성 → **덱 재빌드**. 세 단계가 한 묶음이 아니면 다시 갈라진다 |

### 12.3 신규 결함 — 6건 (전부 재측정에서만 나왔다)

| id | severity | lane | repro | evidence | status | owner |
|---|---|---|---|---|---|---|
| **C7-F39** | **S3** | planner | R7-14: `grep -rn "RFC-P4-001" _workspace/current` | **RFC id 충돌.** `production/decision-log.md` L73 의 `RFC-P4-001` 은 **Mixamo 적용 범위**(모델러 OPEN-M2)이고 12행이 그 뜻으로 쓴다. `planning/feature-specs/recap-panel.md` L27 `blocked_on:` 과 L103 `deps` D1 은 같은 id 를 **「인용 고정의 저장 필드」**라는 다른 주제로 연다. 같은 id 두 주제는 RFC 추적을 깬다 | open | game-planner |
| **C7-F40** | **S2** | systems | R7-13: `grep -rn "일차" systems/ handoff/ planning/` | **RFC-S3 반영 잔여 3행.** 브리프 L557 은 「UI 라벨에도 「일차」·「N일째」를 쓰지 않는다」로 못박는데, `interaction-rules.md` **L315**(「체크포인트 라벨(장·**일차**·조위 위상)」)과 `game-ui-contract.json` **L360**(「체크포인트 목록과 각 지점의 장 일차 조위 위상」)·**L396**(「상단 좌측에 장과 일차와 조위 위상 표시」)이 그대로 남았다. 두 파일 모두 **승격 후보**이며 `gdd.md` §7 슬롯 카드·`recap-panel.md` R2 는 장·조위 위상·플레이 시간 3항이다 | open | game-systems-designer |
| **C7-F41** | **S3** | production-director | R7-15: `python3` 두 분모 재계산 | **「61 %」가 어느 분모로도 재현되지 않는다.** 비트 분모 **21/33 = 63.6 %** · 하위과제 분모 **35/149 = 23.5 %** 이고 `campaign.json` 에는 하위과제별 활동 분류 필드가 없다. 그런데 초안 §11.3 은 두 값을 「분모가 다르다 — 비트 vs 하위과제」로 **설명해 오류를 확대**했고, 같은 문자열이 `decision-log.md` L162 · `verification-plan.md` L74 · `telemetry-contract.md` L120(「61 %(21/33 비트)」 — 자기모순)까지 **4문서**에 퍼졌다. balance 가 §10.2 Q8 로 이미 정정 요청했고 시트는 63.6 % 를 쓴다 | open | game-production-director |
| **C7-F42** | **S3** | balance | R7-16 · R7-2 | **「이 문서에는 고정 해시가 남지 않는다」(L58)가 거짓이다.** `balance-sheet.md` L386·L396·L463 에 `92301c0a…` · `121457` 이 남아 있고 그 값은 live(`8a43d334…` · **124,007 B** · **49검사**)와 다르다. §7 L400 의 「R7 에 같은 명령으로 재실행해 **동일 출력**을 확인했다(47/47 PASS · sha 불변)」 `[OBSERVED]` 는 **재현되지 않는다**. §7 대조 행의 `proofRequired 15` 도 live **17**(RFC-S4 로 `c1-b2`·`c3-b1` 상향)과 어긋난다. **난이도 지수 자체는 R7-16 으로 재현되므로 C3-F31 결론은 유효하다** — 틀린 것은 영수증 값이다 | open | game-balance-designer |
| **C7-F43** | **S3** | production-director | R7-5 | **검증기 필수 아티팩트 목록 ↔ QA 문서 병합 정책 충돌.** `validate-preproduction.mjs` 가 `status: FIX` 를 내는 유일한 이유가 errors 3(`production/cycles/c6-development.md` · **`qa/c7-review.md`** · `production/cycles/c7-development.md`)이다. QA 는 C6/C7 을 **`qa/c6-review.md` 한 문서로 병합**했다고 §0 에 고지했으므로 `qa/c7-review.md` 는 만들지 않았다. 둘 중 하나를 판정해야 한다 — (a) QA 가 분리 문서를 만든다 (b) 검증기 목록이 병합을 허용한다. `c{6,7}-development.md` 는 디렉터 몫(C6-F25 승계) | open-rfc | game-production-director |
| **C7-F44** | **S3** | production-director | R7-18: 생성기 정규식·파서 읽기 전용 복제 실행 | **`scripts/regen-cycle-ledger.py` 가 등록부 4행을 조용히 버린다.** ① `^\| (C[1-7])-F(\d+)[^\|]*\|` 는 굵은 id 행(`\| **C7-F35** \|` · `\| **C7-F38** \|`)에 매칭되지 않는다. ② severity 셀이 `S1(1차 blocker)` 인 `C4-F1`·`C4-F2` 는 `in ('S1','S2','S3','S4')` 동치 비교에서 탈락한다. 결과: 등록부 **148행 → 생성기 144행**, 대장 C4 총 20(실측 22)·closed 10(12) · C7 총 36(38)·closed 6(7). **누락은 경고 없이 일어난다** — 실패가 조용하면 다음 회차에도 같은 일이 난다 | open | game-production-director |

### 12.4 레인 counter 처리

| 레인 | counter | QA 판정 |
|---|---|---|
| **systems** (`r7-systems-receipt.md` §4) | C6-F12 배정문의 로그 상한 「50k / 8MB」는 `payload` 도입 전 값이며 C7-F7 해소에서 `byteCap` 6 MiB / `entryCap` 20,000 으로 재산정·폐기됐다. 50,000 × 280 B ≈ 14 MB 로 8 MB 예산과 모순 | **counter 채택.** 근거가 파일(`save.md` §3.2 · `save-undo.md` §9)과 산식 둘 다에 있고, 배정문 숫자를 그대로 적었으면 같은 저장소에 상한이 둘 생겼다. **배정문 수치 정정을 디렉터에 올린다**(§12.6). 다만 **「초안 병기」 요구는 미이행**이므로 C6-F12 는 열려 있다 |
| **balance** (`balance-sheet.md` §10.2 Q8) | 판정문 「61 %」 ↔ 재현 명령 「63.6 %(21/33)」 | **counter 채택 + 확대.** QA 독립 재계산으로 하위과제 분모까지 재어 **61 % 는 어느 분모로도 안 나온다**를 확인 → **C7-F41** 로 등록. 시트가 63.6 % 를 쓴 것은 옳다 |
| **planner** (`recap-panel.md` D1) | `save.md` §2 `Progress` 에 인용 고정 필드가 없다 `[OBSERVED]` | **재현 성공.** `grep -n "citation\|인용" save.md` = **1행(L158)**, 그 1행은 무관 문장 — 문서가 적은 그대로다. 다만 그 요청이 쓴 **RFC id 가 충돌**한다 → **C7-F39** |
| **systems** (OPEN-S8~S12) | 좌표 변환식 `[INFERENCE]` · `systemIds` 토큰 재사용 · `hub-uncovered-*` 표시명 대기 · 서랍 프롭 미모델링 · 신설 텔레메트리 실측 n=0 | **전건 정직한 미결 처리로 인정.** 어느 것도 `[OBSERVED]` 로 위장하지 않았다. `T0-04` 의 `note`("계통 목록 = 캠페인 고정 5구역 토큰 `[INFERENCE]`")가 검증기 출력 안에까지 한계를 실어 놓은 것은 **모범 사례**로 기록한다 |

### 12.5 승격 판정 (C3-F33 · RFC-Q2) — §11.5 대체

**승격 가능 9** / **차단 5**.

| 판정 | 파일 | 사유 [OBSERVED R7] |
|---|---|---|
| **승격 가능** | `handoff/README.md` · `handoff/codex-unity-brief.md` · `handoff/verification-plan.md` | 이미 `status: current`. R7 배정분(C7-F4·F8·F12·F14 · C6-F9·F11·F12(브리프 몫)·F5) 전건 이행 확인. **`status` 변경 없음 — 유지 판정** |
| **승격 가능** | `systems/tech-verification/r7-systems-receipt.md` · `r7-t0-data.md` | 이미 `current`. 자기 검사표 S-1~S-17 중 QA 가 독립 재현한 것 4건(S-2 → R7-8, S-6 → R7-9, S-10·S-11 → R7-12) **전건 일치**. 재현 불가 `[OBSERVED]` **0건** |
| **승격 가능** | `systems/unity-implementation.md` (**draft → current**) | C7-F4·C7-F7·C7-F9·C4-F6 전건 closed. §2 7분할·§7 저장 스키마가 `architecture-contract.md`·`save.md` 정본과 일치. **잔여 결함 0건** |
| **승격 가능** | `product/economics.meta.md` · `presentation/deck-outline.md` · `presentation/steam-game-plan.meta.md` | §11.5 판정 유지. 해시 표 2/2 문자 일치 재확인(R7-22). **단 `steam-game-plan.meta.md` L59 의 `92301c0a…` 는 r5 빌드 시점 영수증이므로 `[CARRIED]` 표기 권고**(현재 live 는 `8a43d334…` · 49검사) |
| **차단** | `systems/interaction-rules.md` | **C7-F40 신규 open S2** — L315 「일차」가 브리프 금지 지시와 충돌. C4-F7·F9·F16·F20·F21·F22·C7-F10·F11 은 전건 closed 이므로 **이 한 줄이 유일한 차단 사유**다 |
| **차단** | `systems/game-ui-contract.meta.md` | C4-F12 closed 이나 기술 대상 `game-ui-contract.json` L360·L396 에 **C7-F40 open S2** |
| **차단** | `product/business-model.md` | C6-F2·F6·F8 전건 closed. **차단 사유는 문서 자신의 §0 「C5 초안 준비물이며 완료된 사이클이 아니다」** — 승격하면 그 고지와 모순된다. PM 이 §0 을 고쳐 함께 올리거나 draft 유지 |
| **차단 (의도)** | `balance/puzzle-balance.md` | 문서 L11 자기 고지 「정본이 아니다」. 승격하면 정본이 둘이 된다. **사유 불변** |
| **차단** | `planning/game-draft-v1.md` | 이미 `current` 이나 **C6-F12 미반영 · C7-F41 오설명 · C6-F25(§12 색인에 `handoff/` 부재)** 3건이 열려 있다. `status` 는 그대로 두되 **다음 편집에서 세 곳을 함께 고친다** |
| **차단** | `handoff/asset-runbook.md` | `owner: game-modeler` · `status: draft`. C7-F3·C7-F12 는 closed 이나 §3.3-0 미결(Unity `.glb` 임포터 실측 0)이 남아 modeling 이 스스로 올릴 시점이 아니다 |

### 12.6 브로드캐스트 · `feedback-requested-by: 2026-09-11`

| 받는 레인 | 항목 | 요청 |
|---|---|---|
| **game-production-director** | **C7-F41**(「61 %」 4문서 전파) · **C7-F43**(검증기 목록 ↔ 문서 병합) · **C7-F44**(대장 생성기 4행 탈락) · **C4-F11**(파생 이미지) · **RFC-Q3**(C6-F7 잔여) · C6-F12 배정문 수치 정정(systems counter 채택) | C7-F44 → 대장 재생성 → **덱 재빌드**는 한 묶음이다. 따로 하면 C5-F2 가 세 번째로 갈라진다 |
| **game-systems-designer** | **C7-F40**(신규 S2 — 「일차」 3행) | `interaction-rules.md` L315 · `game-ui-contract.json` L360·L396 을 「장 · 조위 위상 · 플레이 시간」으로 맞추면 **`interaction-rules.md`·`game-ui-contract.meta.md` 승격 차단이 동시에 풀린다** |
| **game-planner** | **C7-F39**(RFC id 충돌) · **C6-F12**(초안 병기) · **C7-F41**(초안 §11.3 문장) | RFC id 는 디렉터가 부여하지 않은 번호를 쓰지 않는다. 셋 다 한 편집 |
| **game-balance-designer** | **C7-F42**(고정 sha 3곳 · 「동일 출력」 주장 · `proofRequired 15`) | 난이도 지수 결론은 유효하다 — **영수증만** 검증기 출력 인용으로 바꾸면 된다 |
| **game-concept-artist** | C4-F11 잔여 없음(레인 몫 완료) | 파생본 갱신은 디렉터 몫임을 확인만 |
| **game-presentation-director** | `steam-game-plan.meta.md` L59 `[CARRIED]` 표기 · 덱 19번 재빌드 대기 | 덱 재빌드는 C7-F44 해소 이후에 한 번만 |
| **game-modeler** | 런북 §3.3-0 미결(`.glb` 임포트 실측 0) | T0 첫 임포트 결과를 `rfc-inbox` 로 |
| **game-product-manager** | `business-model.md` §0 지위 문장 ↔ 승격 여부 | 승격을 원하면 §0 을 함께 고친다 |

### 12.7 이 절이 주장하지 않는 것

- **열린 S1 이 0 이 되었다고 게이트가 올라가지 않는다.** 런타임 값은 여전히 **전부 NOT-MEASURED** 이고 `0 / 8 PASS` 는 불변이다(`qa/gate-measurements.md` 최종 표).
- **`--t0` 5/5 PASS 는 데이터 정합이지 T0 가 플레이된다는 증거가 아니다.** Unity 임포트 0회 · 술어 판정 0회.
- **`emit-tables.mjs` 결정론 재현(R7-8)은 값이 옳다는 증명이 아니다** — 같은 입력에서 같은 바이트가 나온다는 것뿐이다. 값의 캐논 정합은 생성기 자신의 대조에 의존하며 QA 는 그 대조 코드를 읽었을 뿐 별도 캐논 감사를 하지 않았다.
- **37/0 참조 모형 · UI 계약 exit 0 · 문서 5곳 일치는 전부 문서·순수 Node 검사다.** 재미·조작감·완주 시간·접근성 실사용은 **한 건도 측정되지 않았다**.
- **「61 %」의 원 출처를 QA 도 찾지 못했다.** 두 분모를 재어 둘 다 아니라는 것까지가 관측이고, 그 수가 어디서 왔는지는 **추정하지 않는다**.
- **덱 19번 슬라이드의 값이 언제 옳았는지 QA 는 확인하지 않았다.** 현재 값이 현재 등록부와 다르다는 것만 관측했다.

---

## 재검증 4 (R7b) — 2026-09-10 · 승격 판정 회차

> **대상 5건**: C7-F40 · C7-F41 · C6-F12 · C6-F25 · `product/business-model.md` §0.
> **방법**: 파일 재열람 + `grep` / `shasum` 재현. **이 회차도 올린 게이트는 0개다** — Unity 실행 0회 · 빌드 0줄 · 사람 표본 n = 0. QA 는 `qa/` 밖을 쓰지 않았다.

### 13.1 재측정 명령과 값 [OBSERVED 2026-09-10 R7b]

| id | 명령 | 값 |
|---|---|---|
| **R7b-1** | `cd _workspace/current/systems && grep -rn "일차" .` | **6행** — `game-ui-contract.meta.md` **L50·L54·L55·L57·L61**(개정 7 자기기술) + `tech-verification/r7-systems-receipt.md` L96(금지 규칙 인용). **UI 라벨 행 0** |
| **R7b-2** | `grep -rn "일차" systems/ handoff/ planning/` (R7-13 재실행) | 위 6행 + `handoff/codex-unity-brief.md` L557(금지 규칙 원문). `interaction-rules.md` · `game-ui-contract.json` **0행** |
| **R7b-3** | `sed -n '315p' systems/interaction-rules.md` | 「체크포인트 라벨(**스테이지**·조위 위상)」 |
| **R7b-4** | `sed -n '360p;396p' systems/game-ui-contract.json` | 「… 각 지점의 **스테이지** 조위 위상」 · 「상단 좌측에 **스테이지**와 조위 위상 표시」 |
| **R7b-5** | `shasum -a 256` + `wc -c` (json · interaction-rules) | `5b3ffdb1…` · **43,643 B** / `75bc0fd2…` · **55,479 B** — `game-ui-contract.meta.md` 해시 표 2행과 **일치** |
| **R7b-6** | `sed -n '264p' planning/game-draft-v1.md` | 「되돌림 무제한과 로그 크기 상한은 다른 축」 + `entryCap` **20,000** / `byteCap` **6 MiB** + **SV-F6** 접힘 + 「50,000/8MB 폐기」 + 출처 2건 **전건 존재** |
| **R7b-7** | `grep -n "entryCap\|byteCap\|SV-F6" systems/system-specs/save-undo.md systems/data-schemas/save.md` | 정본 = `save-undo.md` L155 · `save.md` L116·L117·L153·L154. 초안 수치가 정본과 **일치** |
| **R7b-8** | `grep -rn "61 %\|61%" .` | 초안 §11.3(L454)은 **63.6 %(21/33) 정본 + 「61 %」 재현 불가 명시**로 정정됨. 「61 %」 **잔존 4곳**: `production/decision-log.md` L162 · `handoff/verification-plan.md` L74 · `systems/ops/telemetry-contract.md` L120 · `balance/balance-sheet.md` L557·L627(정정 **요청** 문맥이므로 오류 아님) |
| **R7b-9** | `ls production/cycles/` · `grep -n "handoff/" planning/game-draft-v1.md` | `c1`~**`c7`-development.md 전건 존재** · 초안 §12 에 `handoff/` **3행**(README·brief·verification-plan / asset-runbook / rfc-inbox) |
| **R7b-10** | `sed -n '9,13p' product/business-model.md` | §0 = 「[TARGET] **C5 독립 검토(qa/c5-review.md)·R7 재검증을 거친 현행판이다.**」 + 「가격·수익·판매량 전부 승인 전 후보 · 실측 **n=0**」 |
| **R7b-11** | `bash .claude/skills/game-ops-harness/scripts/freshness-check.sh` | **exit 0** — 원문 3줄은 §13.4 |

### 13.2 판정

| 결함 | 판정 | 근거 |
|---|---|---|
| **C7-F40** (S2) | **closed** | R7b-1~R7b-5. 지목 3행 전건 정정, 다른 키·행·순서 변경 0건(해시 표 2행이 실측과 일치). 남은 「일차」는 **금지 규칙을 인용한 문장**뿐이며 UI 라벨이 아니다 |
| **C6-F12** (S2) | **closed** | R7b-6·R7b-7. 초안 §4.5 L264 병기 완료. 상한 수치는 systems counter(6 MiB / 20,000)를 채택했고 디렉터 배정문의 「50k/8MB」는 초안에 **재기재되지 않았다** |
| **C7-F41** (S3) | **open 유지 · 범위 축소** | R7b-8. 초안의 「분모가 다르다」 **오설명은 제거**됐다(planner 몫 완료). 잔여는 소유 레인(**game-production-director**)의 3문서 문자열 — `decision-log.md` L162 · `verification-plan.md` L74 · `telemetry-contract.md` L120 |
| **C6-F25** (S3) | **open 유지 · 범위 축소** | R7b-9. 3항 중 2항 해소(`c6-development.md` 존재 · §12 `handoff/` 등재). 잔여 1항 = **초안 §6 이 `systems/unity-implementation.md`(status `draft`)를 「(C4/C5 검증 대기)」 표기 없이 출처로 인용**(§12 표는 표기함) |
| **business-model §0** | **문장 정정 확인 · 승격은 1행 조건부** | R7b-10. §0 문장은 정정됐다. 다만 **H1 이 「# 상품 · 수익모델 (C5 초안 준비물)」 그대로**여서 `status: current` 로 올리면 제목이 본문 지위와 모순된다 → §13.3 |

### 13.3 승격 판정 (C3-F33 절차)

| 파일 | 판정 | 사유 |
|---|---|---|
| `systems/interaction-rules.md` | **승격 가능** | C7-F40 closed. 이 파일을 겨눈 열린 결함 없음 — C4-F14 잔여(SC-4)는 **RFC 본문의 인용 오류**이고 C3-F33 은 승격으로 해소되는 쪽이다 |
| `systems/game-ui-contract.json` | **승격 가능** | 지목 2행 정정, 구조 검사 PASS, 오타 4건(C4-F14 SC-2)은 이미 0행 |
| `systems/game-ui-contract.meta.md` | **1행 조건부 차단** | 아래 **C7-F45**. 내용 판정은 옳고 **재현 출력 한 줄만** 사실과 다르다 — 그 줄을 고치면 즉시 승격 가능 |
| `product/business-model.md` | **1행 조건부 차단** | H1 「(C5 초안 준비물)」 ↔ `status: current` 모순. 제목 한 줄 정정 후 즉시 승격 가능(§0 본문은 이미 정정됨) |
| `planning/game-draft-v1.md` | **판정 대상 아님** | 이미 `status: current` (cycle c6) |

**신규 결함 1건**

| id | severity | lane | repro | evidence | status | owner |
|---|---|---|---|---|---|---|
| **C7-F45** | **S4** | systems | `cd _workspace/current/systems && grep -rn "일차" .` (R7b-1) | `game-ui-contract.meta.md` **L61** 의 `[OBSERVED]` 재현 주석이 「→ `tech-verification/r7-systems-receipt.md:96` **한 행만 남음**」이라고 적었으나 실제 출력은 **6행**이다(자기 자신 L50·L54·L55·L57·L61 포함). **실질 판정(UI 라벨 0행)은 옳다 — 틀린 것은 영수증 문장**이며 C7-F42 와 같은 유형이다. 같은 블록의 마지막 `shasum`/`wc` 두 줄은 **출력이 붙어 있지 않다**(값은 위 해시 표에 있으므로 재현은 가능) | open | game-systems-designer |

**권고(신규 결함 열지 않음)**: 초안 §4.5 의 `entryCap`/`byteCap` 두 수치는 정본(`save.md` §3.2 · `save-undo.md` §9)에서 `[TARGET]`·`[INFERENCE] 파생`으로 표기돼 있으나 초안에는 태그 없이 실린다. 사양 서술이라 결함으로 열지 않되, T0 계측 뒤 재파생 대상임을 초안이 함께 적으면 태그 손실이 사라진다.

### 13.4 `freshness-check.sh` 원문 [OBSERVED 2026-09-10 R7b · exit 0]

```
$ bash .claude/skills/game-ops-harness/scripts/freshness-check.sh
freshness: 0 finding(s) across 120 markdown artifact(s) under _workspace/current
freshness: scope = frontmatter contract + supersedes topology only; memory_sync receipts (mex/llm-wiki/graphify/zg) are NOT verified here
freshness: no cycle start supplied; staleness not measured
exit=0
```

**이 exit 0 이 뜻하지 않는 것**: 프론트매터 계약과 `supersedes` 위상만 본 값이다. `--since` 를 주지 않았으므로 **시점 신선도는 미측정**이고 `memory_sync` 영수증(mex/llm-wiki/graphify/zg)은 **검사되지 않았다** → G8 전체 PASS 로 읽지 않는다(CLAUDE.md §11).

### 13.5 이 회차가 바꾸지 않은 것

- **게이트 0 / 8 불변.** 승격 판정은 문서 지위의 문제이며 어떤 런타임 값도 재지 않았다.
- 열린 S1 **0** 유지 · 열린 S2 **5 → 3**(C7-F40 · C6-F12 closed).
- **Codex Unity 핸드오프의 착수 가부는 QA 판정 사항이 아니다** — 브리프 반박 3렌즈(startability/contradiction/safety)는 §12 값 그대로이며 R7b 는 그것을 재측정하지 않았다.

---

## 재검증 5 (R7c, 디렉터 처리분) — 2026-09-10

디렉터가 처리했다고 통보한 4건(C4-F11 · C5-F2 · C7-F43 · C7-F44)을 **파일 재열람 + 명령 재실행**으로만 판정했다. 통보 문장은 근거로 쓰지 않았다. **QA 는 이 회차에도 `qa/` 밖의 파일을 쓰지 않았다** — 단 하나의 예외는 디렉터가 실행을 지시한 `scripts/regen-cycle-ledger.py` 이며, 이 스크립트가 `production/cycle-ledger.json` 을 재생성한다(§아래 R7c-6 · 부작용 명시).

### 재검증 5.1 명령과 출력 [OBSERVED]

| id | 명령 | 출력(발췌) |
|---|---|---|
| **R7c-1** | `shasum -a 256 assets/generated/2d/readme/readme-verb-seal.png docs/media/verb-seal.jpg` | `289689814355a5d4…` / `c685b2c0d954ea93…` |
| **R7c-2** | `stat -f '%N %z %Sm'` 위 2파일 | png **3,080,168 B · 08:57:23** · jpg **259,457 B · 10:47:20** → **파생본이 원본보다 나중** |
| **R7c-3** | `docs/media/provenance.json` · `assets/…/readme/provenance.json` 의 `verb-seal` 항목 파싱 | jpg `output_sha256` = `c685b2c0…` **= 실측 일치** · `derived_from` = `assets/generated/2d/readme/readme-verb-seal.png` · `regenerated: "2026-09-10 C4-F11 (no handwriting)"` · png `output_sha256` = `289689…` **= 실측 일치**, `bytes 3080168` = 실측 |
| **R7c-4** | PIL 로 png→600×400 ↔ jpg→600×400 평균 채널 절대차 | **1.266 / 255** (JPEG 양자화 수준) → **파생본은 재생성본에서 나왔다** |
| **R7c-5** | 원본 png 책자 영역 crop `(0.58–0.99 w, 0.50–0.76 h)` → 1400 px 업스케일 육안 판정 | **괘선 + 짧은 파선뿐. 자소·글자꼴·필기체 0.** 압착판 crop `(0.30–0.60 w, 0.42–0.78 h)` 은 삼각 구조 리브 — 문자 아님 |
| **R7c-6** | `python3 scripts/regen-cycle-ledger.py` | stderr `parsed 155 rows, dropped 0: []` · exit **0** · **부작용**: `production/cycle-ledger.json` 재작성 |
| **R7c-7** | `grep -cE '^\| \**C[1-7]-F[0-9]+' qa/defect-register.md` + 회차별 분해 | **155** 행 · C3 **36** / C4 **22** / C5 **11** / C6 **41** / C7 **45** → 대장 `total` 과 **전건 일치** |
| **R7c-8** | 생성기 정규식 **읽기 전용 복제**로 이전 탈락 4행 재파싱 | `C7-F35` `**S2**` · `C7-F38` `**S3**` · `C4-F1`·`C4-F2` `S1(1차 blocker)` → **4행 전부 파싱 성공** |
| **R7c-9** | `node .claude/skills/game-ops-harness/scripts/validate-preproduction.mjs` | `"status": "SPEC-PASS"` · `"checks": 514` · `"passed": 514` · **`"errors": []`** · `runtimeStatus: NOT-MEASURED` · exit **0** |
| **R7c-10** | 검증기 L11–15 `mergedReview()` 원문 + `ls production/cycles/` | `rel === 'qa/c7-review.md'` 일 때만 `qa/c6-review.md` 존재 ∧ 앞 4000자에 `C7` 이면 허용. `c1`~`c7-development.md` **7종 전부 존재** |
| **R7c-11** | 덱 s19 표 ↔ 재생성된 대장 대조 · mtime 비교 | 덱 = C3 36/**27** · C4 22/**9** · C5 11/**6** · C6·C7 **미기록** · 상태 `reviewed-and-revised` **5건**. 대장 = C3 36/**33** · C4 22/**17** · C5 11/**9** · C6 **41/16** · C7 **45/16** · `fix-in-progress` **3건**. html **06:28:51** < ledger **10:56:40** → **미재빌드** |
| **R7c-12** | `presentation/generate-deck.mjs` L426–431 ↔ 대장 `cycles[]` 키 | 생성기는 `c.findings`·`c.fixed`·`c.remaining` 을 읽는데 **`schemaVersion 2`** 의 키는 `id·focus·review·status·total·closed·open_S1·open_S2·open_S3plus·open_rfc` → **지금 재빌드하면 발견·수정·잔여 21칸이 전부 `미기록`** 으로 렌더된다 |
| **R7c-13** | 등록부 status 셀의 **판정 토큰 다중 출현** 스캔(읽기 전용) | **1행 = `C6-F12`.** 셀이 `open (재검증 3 …) → **closed** (재검증 4 …)` 라 생성기가 **첫 토큰 `open`** 으로 집계 → 대장 C6 `closed 16 / open_S2 2`, 등록부 §11.3 `closed 17 / open_S2 1` |
| **R7c-14** | `bash .claude/skills/game-ops-harness/scripts/freshness-check.sh` | `0 finding(s) across 120 markdown artifact(s)` · exit **0** |
| **R7c-15** | 등록부 동기화 **후** 생성기 로직 읽기 전용 복제(파일 미기록) | `parsed 157 / dropped 0` · C3·C4·C5·C7 **등록부 §12.3 과 전건 일치** · C6 만 `closed 16 / open_S2 2`(등록부 17 / 1) · 합계 closed **94 ↔ 95** — 차이는 `C6-F12` 1행뿐(C7-F46) |

### 재검증 5.2 판정

| id | 판정 | 근거 한 줄 |
|---|---|---|
| **C4-F11** | **closed** | 파생본 sha `c685b2c0…` 가 provenance 기재값과 일치하고 mtime 이 원본보다 **나중**이며 원본과의 픽셀 차 **1.266/255** — 옛 이미지가 아니다. 책자 확대 판정 결과 **글자꼴 0** 이라 `style-guide.md` §10-4 위반 사유가 남지 않는다(R7c-1~R7c-5) |
| **C5-F2** | **open (범위 축소)** | 등록 범위 두 축 중 **대장↔등록부 모순은 해소**(155 = 155 · 회차별 전건 일치). 잔여는 **덱 재빌드 1건** — s19 가 아직 손으로 적은 옛 값을 싣고 C4·C5 를 `reviewed-and-revised` 로 표시한다(R7c-11). 그리고 지금 재빌드하면 카운트가 전부 `미기록` 이 된다(→ **C7-F47**) |
| **C7-F43** | **closed** | RFC 선택지 (b) 「검증기 목록이 병합 허용」이 `mergedReview()` 로 구현되고 `c6·c7-development.md` 가 신설돼 `errors: []` · `SPEC-PASS` · 514/514(R7c-9·R7c-10) |
| **C7-F44** | **closed** | 등록 범위인 **4행 조용한 탈락**이 두 사유 모두 해소 — 굵은 id `**S2**`/`**S3**` 와 `S1(1차 blocker)` 가 전부 파싱되고 `dropped 0`, 155 = 155(R7c-6~R7c-8). **집계 오분류는 등록 범위 밖**이라 신규 **C7-F46** 으로 분리한다 |

### 재검증 5.3 신규 결함 2건

- **C7-F46 (S2, director/production)** — 생성기가 status 셀의 **첫 판정 토큰**을 취해 `open (…) → closed (…)` 서술형 셀을 **open 으로 오분류**한다. 현재 1행(`C6-F12`) 이 걸려 대장이 등록부 §11.3 과 **closed 1건·open S2 1건** 어긋난다. 행이 버려지지 않으므로 `dropped` 경고도 뜨지 않는다 — **C7-F44 와 같은 「조용한 불일치」의 다른 얼굴**이다. 반대 방향(`closed → open` 서술)이 생기면 **닫힌 것으로 잘못 집계**되므로 방향 위험은 대칭이 아니다.
- **C7-F47 (S2, presentation/director)** — 대장 스키마가 **v1 → v2** 로 바뀌었는데 덱 생성기는 v1 필드(`findings`/`fixed`/`remaining`)를 읽는다. 두 생성기가 같은 파일을 두고 **다른 계약**을 쓰며, 어느 쪽도 `schemaVersion` 을 검사하지 않는다. C5-F2 의 잔여(덱 재빌드)는 **이것을 고치기 전에는 해소될 수 없다** — 재빌드가 곧 회귀다.

### 재검증 5.4 결함으로 올리지 않은 관측

- `docs/media/provenance.json` 과 `assets/generated/2d/readme/provenance.json` 에 **동명 `.meta.md` 가 없다**(CLAUDE.md §10 비-Markdown 산출물 규칙). 두 파일 모두 `_workspace/` 밖이라 `freshness-check.sh` 의 검사면 밖이고, 이번 회차의 배정 범위도 아니다 — **관측만 남기고 결함을 부여하지 않는다**.
- `mergedReview()` 의 허용 조건은 `qa/c6-review.md` **앞 4000자에 `C7` 문자열**이면 참이다. 지금은 표제(L9 `# C6/C7 통합 검토`)가 그 근거지만, 조건 자체는 `C7-F1` 같은 단순 언급으로도 성립한다 — **느슨하나 오작동은 관측되지 않았다**.

### 재검증 5.5 이 회차가 바꾸지 않은 것

- **게이트 0 / 8 불변.** 이번에 잰 값은 전부 문서·해시·픽셀이며 **런타임 측정은 0건**이다. `validate-preproduction.mjs` 스스로 `runtimeStatus: NOT-MEASURED` 를 반환한다.
- `freshness-check.sh` **exit 0** 은 프론트매터 계약과 `supersedes` 위상만 본 값이다. `--since` 미지정이라 **시점 신선도 미측정**, `memory_sync` 영수증 **미검사** → G8 전체 PASS 로 읽지 않는다.
- **열린 S1 = 0 유지.** 열린 S2 는 **3 → 4**(C4-F11 해소로 −1, 신규 2건으로 +2).

---

## 재검증 6 (R7d, 최종 동기화) — 2026-09-10

재판정 **3건**(C7-F46 · C7-F47 · C5-F2)을 **명령 재실행으로만** 판정했다. 통보 문장은 근거로 쓰지 않았다. 이 회차는 **동기화 전용**이라 **신규 결함을 열지 않는다** — 관측은 §재검증 6.4 에만 적는다. **QA 는 `qa/` 밖의 파일을 쓰지 않았다.** 유일한 부작용은 지시된 `scripts/regen-cycle-ledger.py` 의 대장 재생성이며, 재생성 결과는 **실행 전후 바이트 동일**이었다(R7d-1). 덱 재빌드는 `presentation/` 을 쓰지 않도록 **scratchpad `--out` 으로만** 돌렸다(R7d-9).

### 재검증 6.1 명령과 출력 [OBSERVED]

| id | 명령 | 출력(발췌) |
|---|---|---|
| **R7d-1** | `python3 scripts/regen-cycle-ledger.py` | stderr **`parsed 157 rows, dropped 0: []`** · exit **0** · stdout C6 = `closed 17 / open_S2 1`. **부작용**: `production/cycle-ledger.json` 재작성 — 단 실행 전 사본과 `diff` **동일**, sha `f0c901e6…` 불변 |
| **R7d-2** | `grep -cE '^\| \*{0,2}C[1-7]-F[0-9]+' qa/defect-register.md` + 고유 id 수 + 회차 분해 | **157** 행 · 고유 id **157** · C3 **36** / C4 **22** / C5 **11** / C6 **41** / C7 **47** → 대장 `total` 과 **전건 일치**, 등록부 행 수 = parsed 수 |
| **R7d-3** | 새 규칙(첫 상태-토큰 시작 cell + 그 cell 의 마지막 토큰) 읽기 전용 복제를 `C6-F12` 한 행에 적용 | status cell **index 4** · 토큰 `['open','closed']` → **마지막값 `closed` 채택**. 대장 C6 `closed` **16 → 17** · `open_S2` **2 → 1** |
| **R7d-4** | `grep -nE 'open_S1\|open_S2\|open_S3plus\|open_rfc\|schemaVersion\|c\.status\|c\.review' presentation/generate-deck.mjs` | **L468–475** 가 `c.total`·`c.closed`·`c.open_S1`·`c.open_S2`·`c.open_S3plus`·`c.open_rfc`·`c.status`·`c.review` **9칸 전부**를 읽는다. **L448–459** 에 `sv !== 2` 게이트: 표를 비우고 값 렌더를 중단하며 재생성 명령을 본문에 적는다 |
| **R7d-5** | `grep -c 'c\.findings\|c\.fixed\|c\.remaining' generate-deck.mjs` | **0** (v1 필드 잔존 없음) |
| **R7d-6** | **음성 시험** — `_workspace/current` 를 scratchpad 로 복사해 사본 대장만 `schemaVersion: 3` 으로 바꾸고 사본 생성기 실행 | 렌더된 회차 행 **0** · 「`schemaVersion` 이 3 이라 이 생성기의 기대(2)와 계약이 다르다」 · 각주 「`schemaVersion 불일치(기대 2, 실제 3)`」 → **게이트가 실제로 발동한다**. 저장소 파일 무변경 |
| **R7d-7** | `wc -c` + `shasum -a 256` 로 메타 r6 기재값 대조 | html **101,252 B** / `f2028d3b87d887a0…` · 생성기 **110,835 B** / `3bf990376e70dc33…` → 메타 r6 「산출물」 표와 **4개 값 전건 일치** |
| **R7d-8** | 메타 r6 재현 명령의 **대조 단계**를 기존 html 그대로에 적용(`diff <(대장 9키)` ↔ `<(s19 렌더 행)`) | **차이 0줄** · 렌더 회차 행 **7** × 9칸 = **63칸 일치** |
| **R7d-9** | `node presentation/generate-deck.mjs --out <scratchpad>/rebuild-check.html` 후 sha 비교 | 체크인 html 과 재빌드본이 **바이트 동일** `f2028d3b…` · 빌드 `errors: []` · warnings 1(의도된 상태) → **덱은 현재 대장으로부터 재생성된 그것과 같다** |
| **R7d-10** | `grep -o 'production/cycle-ledger.json · generated_at [^<]*' steam-game-plan.html` | `generated_at 2026-09-10 · source qa/defect-register.md (single source, RFC-C6-002) · generator scripts/regen-cycle-ledger.py` → 각주 스탬프가 대장 필드와 일치 |
| **R7d-11** | status 동기화 **후** 생성기 로직 읽기 전용 복제(파일 미기록) | `parsed 157 / dropped 0` · C3 36/33 · C4 22/18 · C5 **11/10** · C6 41/17 · C7 **47/20** · 합계 closed **98** · 열린 S2 **1** · 열린 S1 **0** |

### 재검증 6.2 판정

| id | 판정 | 근거 한 줄 |
|---|---|---|
| **C7-F46** | **closed** | 새 규칙이 서술형 셀의 **마지막 토큰**을 취해 `C6-F12` 를 `closed` 로 집계한다 — 대장 C6 `closed 17 / open_S2 1` 이 등록부 §11.3 과 일치하고, `dropped 0` 에 등록부 행 **157 = parsed 157** 이라 「조용한 불일치」의 두 얼굴(탈락·오분류)이 함께 닫혔다(R7d-1~R7d-3) |
| **C7-F47** | **closed** | 소비부가 v2 9키로 교체됐고 v1 필드 **0건**이며, `schemaVersion !== 2` 게이트가 **음성 시험에서 실제로 발동**해 값 렌더를 멈춘다(회차 행 0). 메타 r6 의 sha·bytes 4개 값도 실측과 일치한다(R7d-4~R7d-7) |
| **C5-F2** | **closed** | **내용 일치 기준을 수용한다.** mtime 은 재현 명령이 대장과 html 을 함께 다시 만들 때마다 갱신되므로 신선도의 증거가 되지 못한다 — 대신 대장 **63칸 ↔ 렌더 63칸 `diff` 0줄**, 각주 스탬프 일치, 그리고 체크인 html 과 재빌드본의 **바이트 동일**이 「덱이 옛 손기입 값을 싣고 있다」는 등록 사유를 값으로 소거한다(R7d-8~R7d-10) |

**C5-F2 의 기준 변경을 수용한 이유 한 줄**: mtime 비교는 *언제 만들었나* 를 묻고 내용 대조는 *무엇을 싣고 있나* 를 묻는다. 이 결함의 등록 사유는 후자였다(옛 손기입 값 전파). 바이트 동일까지 확인되면 mtime 이 어떻든 실릴 수 있는 옛 값이 없다.

### 재검증 6.3 동기화 결과 — 전 회차 최종 집계 [OBSERVED 2026-09-10 · 재검증 6 기준]

| 회차 | 총 | closed | 열린 S1 | 열린 S2 | 열린 S3 | 열린 S4 | open-rfc |
|---|---:|---:|---:|---:|---:|---:|---:|
| C3 | 36 | 33 | 0 | 0 | 1 (F35) | 0 | 2 (F22·F33) |
| C4 | 22 | 18 | 0 | 0 | 4 | 0 | 0 |
| C5 | 11 | **10** | 0 | **0** | 1 (F10) | 0 | 0 |
| C6 | 41 | 17 | 0 | **1 (F7)** | 15 | 6 | 2 (F33·F36) |
| C7 | 47 | **20** | 0 | **0** | 18 | 8 | 1 (F29) |
| **합계** | **157** | **98** | **0** | **1** | **39** | **14** | **5** |

검산: 98 + (0+1+39+14) + 5 = **157** ✓ · 직전(재검증 5) 대비 closed **95 → 98**(+3) · 열린 S2 **4 → 1**(−3) · 총계·열린 S1·S3·S4·open-rfc **불변**.

### 재검증 6.4 결함으로 올리지 않은 관측 (동기화 전용 회차)

- **대장 파일이 이 동기화보다 한 걸음 뒤에 있다.** 디스크의 `cycle-ledger.json` 은 status 편집 **이전** 값(C5 `closed 9`, C7 `closed 18 / open_S2 2`)이다. R7d-11 의 읽기 전용 복제는 재생성 시 **C5 11/10 · C7 47/20 · 합계 98** 이 나옴을 이미 보였다. QA 는 대장을 다시 쓰지 않았다 — 재생성 후 **덱도 함께 다시 빌드해야** 63칸이 갈라지지 않으므로, 두 단계는 소유 레인이 **한 묶음**으로 실행할 인계 항목이다(§12.4 의 조치 순서와 같은 이유).
- **`schemaVersion` 게이트는 빌드를 실패시키지 않는다** — 불일치 시 exit **0** 으로 표만 비운다. 조용한 오값보다는 낫고 현재 계약(2)에서는 발동하지 않으므로 결함으로 열지 않는다. 다만 CI 가 이 덱을 자동 빌드하게 되면 「표가 비었다」를 사람이 봐야만 알아챈다.
- **새 status 규칙의 남은 가정**: 「첫 상태-토큰 시작 cell」이 status 열이라는 가정은 앞선 어떤 셀도 상태 토큰으로 시작하지 않을 때만 참이다. 157행에서는 성립했고(`dropped 0`, 회차별 전건 일치) 위반 사례는 관측되지 않았다.
- **이 회차의 런타임 측정은 0건**이다. 잰 값은 전부 해시·바이트·문자열 대조다.

### 재검증 6.5 이 회차가 바꾸지 않은 것

- **게이트 0 / 8 불변.** 결함 3건이 닫힌 것은 **문서·생성기 계약의 문제**이며 어떤 플레이 값도 재지 않았다. 플레이 표본 **n=0** 유지.
- **열린 S1 = 0 유지.** 열린 S2 는 **4 → 1**(C6-F7 하나만 남는다).
- 회차가 문서로 남았다는 사실만 늘었을 뿐 **그 회차가 게임을 개선했는지는 여전히 미측정**이다.

### 재검증 6.6 `freshness-check.sh` 원문 [OBSERVED 2026-09-10 R7d · exit 0]

```
$ bash .claude/skills/game-ops-harness/scripts/freshness-check.sh
freshness: 0 finding(s) across 121 markdown artifact(s) under _workspace/current
freshness: scope = frontmatter contract + supersedes topology only; memory_sync receipts (mex/llm-wiki/graphify/zg) are NOT verified here
freshness: no cycle start supplied; staleness not measured
exit=0
```

아티팩트 수는 직전 회차(R7c·R7b)의 **120 → 121**. 늘어난 1건은 이번 동기화가 만든 파일이 아니라 다른 레인이 추가한 것이다 — QA 는 기존 `qa/` 파일 **2개(`c6-review.md`·`defect-register.md`)를 append 했을 뿐 새 파일을 만들지 않았다**.

**이 exit 0 이 뜻하지 않는 것**: 프론트매터 계약과 `supersedes` 위상만 본 값이다. `--since` 를 주지 않았으므로 **시점 신선도는 미측정**이고 `memory_sync` 영수증(mex/llm-wiki/graphify/zg)은 **검사되지 않았다** → **G8 전체 PASS 로 읽지 않는다**(CLAUDE.md §6). 또한 이 스크립트는 `_workspace/current` **밖**을 보지 않으므로 `production/cycle-ledger.json` 이 §13.3 대로 한 걸음 뒤에 있다는 사실은 **이 exit 0 이 잡아 주지 않는다**.
