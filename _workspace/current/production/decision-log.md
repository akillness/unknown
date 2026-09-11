---
updated: 2026-09-11
cycle: bootstrap
status: draft
supersedes: null
owner: game-production-director
---

# Decision Log (append-only; RFC-n ids unique)

---
<!-- 2026-09-10 세션 재개 · 디렉터 판정 블록. frontmatter는 파일 머리 유지, 본 절은 append-only. -->

## 세션 병합 기록 (2026-09-10 00:30 KST) [OBSERVED]
- 2026-09-09 밤 두 세션이 같은 `_workspace/current/`에 병행 기록했다. 세션 P(부모)는 C1~C5 초안·`campaign.json` C4 수리본·UI 계약·덱·견적·C1/C2/C4 검토를, 세션 Q(본 세션)는 C3 레인 후속본 40여 건과 C3 2차 QA 검토(`qa/c3-review.md`, 결함 24건)를 남겼다.
- 사용자 판정(2026-09-10): **세션 P의 초안을 정본으로 삼아 그 위에서 마무리**하고, 이후 쓰기는 **본 세션만** 한다. 따라서 아래 RFC는 "P의 명시적 결정(`messages/003`, `planning/campaign.meta.md` §5·§6, `production/cycles/c3,c4-development.md`)이 Q의 후속본과 충돌하면 P가 이긴다"를 원칙으로 판정한다. 삭제는 없다. Q의 문서는 P 위로 재도출한다.

## RFC-P3-008 · C3-F1 계보 판정
- lanes: planner, synopsis, systems, balance, economy, qa
- question: `campaign.json`이 두 벌(아카이브 c3 = 30·50·55·60·65·65·70·60·25 / live c4 수리본 = 25·50·55·65·65·70·75·65·10)인데 `status: current` 문서가 아카이브 계보를 인용한다. 무엇을 역산 대상으로 삼는가.
- proposal(QA): 계보 B(live)로 닫고 planner·synopsis 재도출(SPEC-REDO).
- evidence: `qa/c3-review.md` §1.1, `planning/campaign.meta.md` §1·§3(sha `775a984c…`, 120087B, 시간 패딩 기각 결정), `production/cycles/c3-development.md` Decision.
- decision: **계보 B(live `planning/campaign.json`, sha `775a984c…`)가 유일한 정본.** `gdd.md` §4.2·§10, `content-matrix.md`, `campaign-time-budget.md`, `synopsis/chapter-beats.md`, `systems/ops/telemetry-contract.md` §1, `systems/data-schemas/beats.md`, `balance/balance-sheet.md` 집계(clue 72 · reader 11 · seal 7 · fast 322 · deliberate 673 · `tools: []` 5비트 · `c1-b4` log/log)를 live 값으로 재도출한다. 아카이브 c3 `campaign.json`은 역사로만 인용한다. planner·synopsis 두 레인은 SPEC-REDO.
- decided_by: game-production-director · date: 2026-09-10

## RFC-P3-009 · C3-F4 부식 예산 정본
- lanes: balance, economy, systems, worldview, planner
- question: balance = 6계통 한도(14/14/12/9/9/9)·도구 확정마다 누적 소모·리셋 없음 / economy = 전역 단일 9·`routing` 구성 비용·장 경계 리셋. 어느 모델이 정본인가.
- evidence: `systems/interaction-rules.md` §2.5("무제한·무료 시험 … 무료 우회관 1회로 언제나 복구, 영구 도구 상실 없음"), `economy/resources-and-fairness.md`("부식예산은 … 현재 선택의 제약을 보여주는 UI"), `systems/prototype/model.mjs:79 corrosionLimit: 9`, `systems/game-ui-contract.json` data_bindings "부식예산 게이지 … 구성안 시험 결과", `messages/003` C2("부식은 가상 시험 제약이며 무료 우회관으로 언제나 복구"), `planning/campaign.json` corrosion 비트(c2-b2·c2-b3·c4-b2 = 시험·판독, 소모 없음).
- decision: **economy/세션 P 모델이 정본.** 부식예산 = `routing` 구성안의 총 부식 비용에 대한 **전역 상한 9**(선택지 비용 lowland 7 · dock 8 · dock-express 12). circuit/reader/seal 확정은 부식을 소모하지 않는다. 누적 고갈·계통 영구 고장·연습 소모는 없다. "장 경계 리셋"이라는 표현은 폐기 — 소모 자체가 없으므로 리셋 개념이 성립하지 않는다(economy §4.1 결론 채택). 계통별 분해는 economy §4.1 표의 조건부 [TARGET] 표시 전용으로만 남긴다. `balance/balance-sheet.md` §4 전면 재작성, `worldview/glossary.md` "부식예산" 정의를 "구성안 단위 상한"으로, `worldview/worldview-bible.md` 법5 문구는 아카이브 c3(세션 P) 원문을 따른다.
- decided_by: game-production-director · date: 2026-09-10

## RFC-P3-010 · C3-F8 세계관 포크 병합
- lanes: worldview, synopsis, planner
- question: `_workspace/archive/20260909-preproduction-c3/worldview/*`(세션 P의 C3판, C2-QA 6건 반영: 하룻밤 21:00~05:00 프레임, 정합 잔차 ±4, 자동 사본, 판 #0 재정의, 시각 H-1:24/H-1:04/H+0:12)와 `current/worldview/*`(세션 Q, "C2 문구 그대로 보존" 원칙으로 P의 수정을 되돌림)가 둘 다 c2를 supersedes 한다(포크·고아).
- decision: **아카이브 c3 세계관(세션 P)이 캐논 본문.** `current/worldview/worldview-bible.md`·`timeline.md`는 아카이브 c3 본문 위에 재작성하고 `supersedes:`를 `_workspace/archive/20260909-preproduction-c3/worldview/<file>.md`로 바꾼다. Q가 더한 §3-bis(연습/확정 분리)·§9 확장표·용어 승격은 P 본문과 모순되지 않는 범위에서 유지한다. C2 캐논 규칙(T-12 이전 사실은 season RFC)은 **미출시 사전제작 단계에서는 본 RFC로 대체**한다 — 출시 후에만 season 규칙이 산다.
- decided_by: game-production-director · date: 2026-09-10

## RFC-P3-011 · C3-F14 시간 수용 판정 키
- lanes: director, qa, systems, planner
- question: 계약 `## Time acceptance`의 조건(중앙값≥420·p25≥360·p75≤600)이 세션 P가 기각한 값이고, 판정에 쓸 텔레메트리 키(`total_min`/`afk_total_min`/`total_minus_afk_min`)가 미정의다.
- evidence: `planning/campaign.meta.md` §6-1(420/360/600 기각, 계약 450~540 인용), `messages/002` F4(420 완화안 기각·설계/관측 키 분리), `qa/c1-review.md` F4(±6% 밴드는 n=12로 판정 불가), `systems/ops/telemetry-contract.md` §5 AF3·AF4.
- decision: (1) 판정 키 = **`total_minus_afk_min`**(AFK 구간은 회고로 판별, 60초 무입력 자동 제외 없음). (2) **목표 밴드는 세션 P의 관측 중앙값 450~540분**을 유지한다. (3) 420/360은 *통과선*이 아니라 **철회 트리거**다: 중앙값 < 420 또는 p25 < 360이면 "8시간" 주장을 철회하거나 콘텐츠를 증설한다. 목표를 낮춰 자동 통과시키지 않는다. (4) `fastMinutes` 322 / `deliberateMinutes` 673은 시나리오 경계이며 표본 통계와 비교하지 않는다(P의 범주 오류 지적 채택) → p75 조건은 삭제. 계약·telemetry-contract §5·campaign-time-budget §9.3을 이 문구로 통일.
- decided_by: game-production-director · date: 2026-09-10

## RFC-P3-012 · C3-F6 T0 공개 상한
- lanes: synopsis, worldview, planner
- question: `t0-b1`이 '한도연'·'판 #0'을 노출하는 것이 timeline §7·continuity §4의 "최초 등장 B18" 규칙 위반인가.
- evidence: `messages/003` C3("도연 이름 자체를 서린이 모른다는 해석은 기각하고 T0에서 당시 당직자 관계를 명시한다. 두 번째 대필 이름 '서린'은 4장까지 미공개"), `planning/campaign.meta.md` §5 F3·F5, `synopsis/campaign.md` §1.
- decision: **세션 P 결정 유지.** T0 인수 각서가 한도연을 당직 주임으로 기재하고, 판 #0은 T0에서 서린의 소유로 등장한다. 숨기는 것은 서명란의 이름 '서린'뿐이며 `c4-b2`까지 미공개. `timeline.md` §7 B01 금지 열과 `continuity.md` §4를 이 규칙으로 재작성한다. `c1-b4.consequence` "누군가 도연을 도왔다"는 플레이어 오해 문구로 허용(저자 서술이 아님) — continuity §4의 금지 대상은 *저자 확정 서술*로 한정한다.
- decided_by: game-production-director · date: 2026-09-10

## RFC-P3-013 · C3-F5 사건 시각 캐논
- lanes: worldview, synopsis, planner
- decision: 세션 P의 C3 결정(`campaign.meta.md` §5 F1·자체 발견)을 캐논으로 확정: **밸브 개폐 H-1:24 → 봉인 완료 접점 H-1:04(간격 20분 > 총 오차폭 8분), 저지대 침수 H+0:12**(4분 격자). H-1:40/H-1:20/H+0:10은 폐기. `timeline.md` §2는 아카이브 c3 본문(RFC-P3-010)으로 해결되며 `chapter-beats.md` B17 문구를 맞춘다.
- decided_by: game-production-director · date: 2026-09-10

## RFC-P3-014 · C3-F3 / F24 법 문구 정본
- lanes: worldview, planner, presentation, systems
- decision: 6법의 호명 문구는 **아카이브 c3 `worldview-bible.md` §3 표(세션 P)** 하나뿐이다. `consistency-audit.md` §4의 "기각된 재작성문"은 어디에도 쓰지 않는다. `planning/gdd.md` §4·`feature-specs/verb-0{2,3,4,5,6}.md` `law:`·`presentation/generate-deck.mjs`(슬라이드 8) → 정본 문구로 교체 후 덱 재생성.
- decided_by: game-production-director · date: 2026-09-10

## RFC-P3-015 · C3-F10 / F20 / F23 입력·힌트·되돌림 정본
- lanes: systems, planner, balance
- decision: (F10) 확정 기본값 = `two-step`, `hold`는 opt-in — `systems/interaction-rules.md` §1-1이 정본, `gdd.md` §3.3·§5 정정. (F20) 무진전 자동 제안 = **180초 단일 제안 + 180초 쿨다운**(interaction-rules §4·puzzle-balance) — `balance/balance-sheet.md` §6의 3단 자동 승격 모델 폐기(힌트 단계는 플레이어가 순서대로 연다). (F23) 스펙상 되돌림은 무제한이며 `model.mjs maxUndo: 32`는 프로토타입 한정 — `systems/prototype/prototype.meta.md`에 명시, 핸드오프 브리프는 "되돌림 상한 없음(체크포인트 재로드 항상 가능)"으로 적는다.
- decided_by: game-production-director · date: 2026-09-10

## 유료 도구 사용 영수증 · Higgsfield (2026-09-10) [OBSERVED]
- 사용자 2차 요청("리소스는 higgsfield mcp … 이용해서")에 근거해 이 저장소에서는 MCP 대신 `higgsfield` CLI(v1.1.23)를 사용했다. 실행 전 `higgsfield account status` = 153.88 credits.
- 생성: `seedance_2_0_mini` image-to-video 2건(각 5초·720p·16:9·오디오 없음, 시작/끝 프레임 = GTI 프리비즈 f01→f02, f04→f05). 추정 12.5 credits/건, 실행 후 잔액 **128.88** (소모 25.0). 산출 `assets/generated/video/*.mp4` + `provenance.json`, README 용 GIF `docs/media/previz-cutscene-video.gif`.
- 미실행(승인 대기): `multi_image_to_3d`(히어로 프롭 3D 후보), 추가 클립. Mixamo 는 현재 설계에 3D 휴머노이드가 0체라 적용 대상이 없다(RFC-P4-001 참조, 아래).

## RFC-P4-001 · Mixamo 적용 범위 (모델러 OPEN-M2)
- lanes: modeling, animation, concept, director
- question: 사용자 지시 "maxiam(=Mixamo 해석)"과 현재 설계(인물 5인은 2D 초상, locomotion 0, 3D 아바타 없음)가 충돌한다.
- decision: **현 사전제작 범위에서는 Mixamo 를 조건부 미사용으로 둔다.** 근거: art-direction·animation-contract·asset-budget 세 문서가 일치하여 3D 인물이 없고, 2.5D 고정 시점 작업대 게임에서 인물은 초상·손(작업대 확대뷰)으로만 등장한다. 3D 인물 도입은 C4 이후 별도 RFC(인물 셸 5체 추가 = 아트 예산 +25인일 이상 [INFERENCE])로만 열린다. `modeling/pipeline.md` 의 Mixamo 규격(T-pose FBX·1m·휴머노이드 리그)은 그 RFC 를 위해 유지한다. 사용자가 3D 인물을 원하면 이 판정을 뒤집는다.
- decided_by: game-production-director · date: 2026-09-10

## C3 종료 판정 묶음 (2026-09-10 04:10 KST) — QA 재검증 3회 후 잔여 RFC
[OBSERVED] `qa/c3-review.md` §7~§9: 결함 36건 중 closed 27 · open S2 3(F27c·F35·F29) · open S3 1(F36) · open-rfc 5. 열린 S1 0건. `node planning/validate-campaign.mjs` 44/44 PASS, live `campaign.json` sha `fdabf1d421932d679429870a06eeb31e9527562370c6a107aec77c2c884fb2b7` · 120479 B · 단서 73. `freshness-check.sh` 0 finding / 91 artifacts.

### RFC-Q1 · RFC-P3-008 수치 갱신
- decision: RFC-P3-008 본문의 sha `775a984c…`·120087B·단서 72는 planner 가 `c1-b4`에 두 번째 매체를 추가하기 전 값이다. 현행 정본 = **sha `fdabf1d4…` · 120479 B · 단서 73**. 이후 인용은 `planning/validate-campaign.mjs` 출력의 sha 를 쓴다(고정 숫자 재기재 금지).

### RFC-Q2 · 같은 사이클 안의 제자리 갱신은 "대체"가 아니다
- lanes: director, qa, all
- decision: CLAUDE.md §2의 아카이브 의무는 **frontmatter `cycle` 값이 바뀌는 대체**(다음 회차가 이전 회차 산출물을 잇는 경우) 또는 **사이클 종료 시점**에만 발생한다. 같은 `cycle` 값 안에서 QA 루프로 문서를 제자리 갱신하는 것은 *개정(revision)*이며 `supersedes: null` 유지가 맞다. 개정 이력은 문서 §0/변경 로그 절과 `qa/*-review.md` 재검증 절이 담당한다. `freshness-check.sh` 확장(자기 고백 문자열 검사)은 도구 후속 과제로 남긴다(측정 위장 아님). 계약 Evidence storage 에 같은 문장을 추가한다.

### C3-F25 · RFC-P3-013 문구 정정
- decision: RFC-P3-013의 "H-1:40/H-1:20/H+0:10은 폐기"는 오기다. **H-1:40(도연이 서명란에 서린의 이름을 적음)은 캐논 유지**, 폐기는 **H-1:20·H+0:10 두 시각뿐**. 검증기 K-04 가 옳다. timeline L43·continuity B17 의 현행 사용은 정당하다. closed.

### C3-F30 · 비트 번호 체계 단일화
- lanes: worldview, planner, synopsis
- decision: 문서 간 인용 키는 **campaign id(`t0-b1` … `e0-b2`)뿐**이다. B01~B33 은 `worldview/timeline.md` §7 이 소유하는 **표시 색인**이며 정의는 하나 — 스테이지 순서대로 비트를 세어 `t0-b1=B01 … t0-b3=B03, c1-b1=B04 … c7-b4=B31, e0-b1=B32, e0-b2=B33`. planner·synopsis 표는 campaign id 를 1열로 두고 B# 은 이 정의에서 파생한 값만 표시한다. 어긋난 B# 은 재도출.

### C3-F31 · 난이도 지수
- lanes: balance
- decision: 미측정 열(`동시 가설 수` [INFERENCE])은 지수에서 **제외**한다. 지수 = 관측 가능한 2열(최대 단서 수·최대 도구 수)의 합, live 재계산 `[4,5,5,4,4,5,7,4,3]`, C5→C6 +2 는 규칙 통과. 세 번째 열은 [TARGET] 주석으로만 남기고 T0 실측 후 재도입 여부를 정한다. closed(재도출 후).

### C3-F33 · draft 정본 인용 정책
- lanes: systems, balance, economy, planner, presentation, animation, motion, vfx, director
- decision: 세션 P 의 C4/C5 레인 문서(`systems/interaction-rules.md`·`unity-implementation.md`·`game-ui-contract.json(.meta)`, `balance/puzzle-balance.md`, `economy/resources-and-fairness.md`, `animation/animation-contract.md`, `motion/motion-contract.md`, `vfx/vfx-budget.md`, `presentation/deck*`, `product/*`, `production/production-estimate.*`·`cycle-ledger.*`)는 사용자 판정으로 **정본 후보**다. R4(C4 재검증)·R5(C5 검토)에서 QA 가 검증한 문서는 소유 레인이 같은 `cycle` 값 그대로 **`status: current` 로 올린다**(RFC-Q2 에 따라 아카이브 불필요). 그 전까지 current 문서의 인용에는 "(C4/C5 검증 대기)" 주석을 붙인다.

### C3-F22 · 비트 단위 zoneId
- lanes: planner, systems, synopsis
- decision: `campaign.json` 각 비트에 **`zoneId` 필드를 추가**한다(스키마 추가, 기존 필드 제거 없음, 값 ∈ 소속 스테이지 `zoneIds`). 검증기에 `Z-01 beat.zoneId ∈ stage.zoneIds`·`Z-02 전 비트 존재` 검사 추가. `content-matrix.md` §3 은 이 필드에서 파생한다(단일 출처 = JSON). `systems/data-schemas/beats.md` 갱신.

### RFC-W4 · R2 의도 공개 시점
- lanes: worldview, synopsis, planner
- decision: **세션 P 연표 §3 이 이긴다** — 4장(B18=`c4-b3`)은 *효력 판정*(자격 없는 이름 → 집행 요건 미비)까지, 도연의 *의도*(방패가 아니라 잠금장치)는 6장(`c6-b4`)에서 확정. planner 가 `c4-b3.inference` 의 의도 문장("그것은 방패가 아니라 잠금장치다")을 `c6-b4` 로 옮기고 검증기 재실행·meta sha 갱신. synopsis 는 B18/B27 문구 동기화, worldview 는 §9 OPEN-4·audit A22 를 닫는다. 근거: live JSON 이 P 의 자기 연표와 모순되므로 캐논(연표) 우선.

### A37 / C3-F12 · 불파괴 자료쌍
- decision: `synopsis/continuity.md` §5 K 표는 손으로 짝을 고르지 않고 **`validate-campaign.mjs` C-07 출력(proofRequired 15비트의 독립쌍)** 에서 파생한다. 검증기에 `--pairs` 출력을 추가해 비트별 독립쌍(루트 originId·sourceType)을 찍고 그 표를 인용한다. 파생 후 A37 closed.

### RFC-W3 · 소멸 확인
- decision: systems 스펙의 폐기 문구는 수정 루프 2에서 0건이 됐다. RFC-W3 은 대상 소멸로 닫는다. `consistency-audit.md` §4-1 은 "사용 금지 문구 원장"으로 유지하며 systems 스펙은 절 번호가 아니라 **제목 문자열**로 인용한다(QA 대안 채택).

### 잔여 S2/S3 배정 (R4 워크플로 Phase A)
| 결함 | 레인 | 요구 |
|---|---|---|
| C3-F27(c) | economy → systems | `save.md` 에서 `operationalCorrosion` 이 제거됐으므로 economy §4.1 계통별 표시 모델은 **저장 필드에 걸지 않는다**(표시 전용, 파생값). economy §5 자원표 정정, systems `save.md` 에 "부식 저장 필드 없음" 명시 |
| C3-F35 | economy → planner | economy L101 `plateOriginalWear` → `readCounts`(누계)/`readBudget`(상한, 3). 이후 `gdd.md` ③·`verb-02` R3a 교체 |
| C3-F29 | planner → synopsis | 비트-구역: **JSON 이 정본**. `content-matrix.md` §3 을 zoneId 파생으로 재작성, `c1-b4.subtasks[0]` '제3수문' 오기 정정(hub), `chapter-beats.md` 동기화 |
| C3-F36 | systems | `prototype/model.mjs:58` label → '밸브 개폐 각인 ↔ 봉인 완료 접점 각인' |

## C6/C7 디렉터 판정 묶음 (2026-09-10 09:10 KST) — 판정단 5렌즈 · 반박 3렌즈 · QA c6-review 이후
[OBSERVED] `qa/c6-review.md`: 판정단 점수 플레이어 4 · 퍼블리셔 4.5 · 엔지니어 4 · 서사 5.5 · 프로듀서 5.5 (다음 단계 진행 가부 1/5), 핸드오프 반박 startability=false · contradiction=false · safety=true. 열린 S1 1건(C7-F1), S2 약 20건 — 대부분 복합 레인("planner / synopsis" 등)으로 표기돼 R6 수정 루프가 배정하지 못했다(디렉터 워크플로 결함, 아래 R7에서 **첫 레인 = 소유자** 규칙으로 배정).

### RFC-C7-001 · C7-F1 (S1) T0 인스턴스 데이터 + T0 확정 경로 (C6-F10 · C7-F8 포함)
- lanes: systems, planner, synopsis, qa
- decision: (1) **T0 의 확정(commit) 명령은 `reader` 의 "인용 고정(pin citation)"이다** — `planning/gdd.md` §4 표의 reader 확정 층("판독 결과를 가설판에 인용으로 고정, 확정 조건: 매체·계통·관측소 출처가 채워졌을 때")이 정본이며 `interaction-rules.md` §2.2 "판독 자체는 확정이 아니다"와 모순되지 않는다(판독 ≠ 인용 고정). 인용 고정은 체크포인트·저장·롤백(T-15)·되돌림을 모두 거친다 → DoD 6·8 은 이 명령에 적용한다. `t0-b3` 의 `proofRequired` 는 인용 고정 2건(plate/plate-standard-hub × ledger/tide-ledger-bureau, 검증기 C-07 실측)으로 충족한다. (2) **T0 인스턴스 데이터를 저장소에 만든다**: `systems/data/t0/{zones,records,tools,hints,beats}.json` + `.meta.md`. 값의 출처는 발명이 아니라 파생 — 비트·단서·힌트·prerequisites 는 `campaign.json` t0 서브셋에서 `systems/pipeline/emit-tables.mjs` 로 추출, 허브 ZoneAsset(viewNodes·cameraPose·sensorCoverage·systemIds)은 `concept/style-guide.md` §5 카메라·`modeling/specs/hub-watchroom.md` 배치·`worldview-bible.md` §1·§2(3개소 회선, 12시간 링, 4분 분해능)에서 파생, RecordAsset(염판 곡선 샘플·대장 수치·일지 문장)은 **synopsis 가 t0-b1~b3 단서 서술 범위 안에서 저작**하고 planner 가 `validate-campaign.mjs` 에 `T0-01~` 검사(인스턴스 ↔ 캠페인 단서 id·originId·매체 일치)를 추가한다. 캐논 밖 사실(새 인물·새 사건·시각)은 만들지 않는다. (3) `t0-b1`~`b3` 완료 술어를 `beats.json` 에 명시(어떤 인용 고정/열람이 완료 조건인지).
- decided_by: game-production-director · date: 2026-09-10

### RFC-S2 / C7-F4 · asmdef 분할
- decision: **7분할(`architecture-contract.md` §2, status current)이 정본**. `unity-implementation.md` §2 의 5분할 서술을 7분할로 정정(systems). handoff/README 의 읽기 순서에서 current 문서를 draft 문서보다 앞에 둔다.

### RFC-S3 · 세이브 `dayIndex`
- decision: **제거 승인.** 캐논은 단일 야간(21:00→05:00)이므로 `dayIndex`·`chapter` 는 저장하지 않는다. 스테이지 진행은 `storyPhase`(스테이지 id)로 충분. 마이그레이션 대상 아님(v1 이전에 제거).

### RFC-S4 (id 충돌 정리)
- decision: systems 의 RFC-S4(해시 드리프트)는 RFC-P3-008·RFC-Q1 로 **종결**. synopsis 가 같은 id 로 연 건(K2·K8 확정 비트 `c1-b2`·`c3-b1` 이 `proofRequired:false` 라 독립쌍 미출력)은 **RFC-N6** 으로 재번호. 판정: planner 가 `c1-b2`·`c3-b1` 의 `proofRequired` 를 **true** 로 올린다(결말 A·B 근거이므로). 검증기 C-07 이 FAIL 하면 synopsis/planner 가 두 번째 매체 단서를 카탈로그 안에서 추가한다. P1 불변식은 그 뒤 "10/10 기계 검증"으로 복귀.

### RFC-S5 · 확정 사본의 루트 승계 예외
- decision: **예외 없음 유지.** 사본은 원본의 루트 originId 를 물려받는다(interaction-rules §3). 검증기 C-07 15/15 PASS 가 예외 없이도 성립함을 증명한다.

### RFC-S6 / RFC-C6-001 / C4-F9 / C7-F5 · 도구 표시명 정본
- decision: 도구 6종 표시명 정본 = **`planning/gdd.md` §4 = `concept/style-guide.md` §9**: 배선 추적(Circuit Trace) / 판독(Plate Read) / 조위정합(Tide Alignment) / 배수 편성(Drain Routing) / 부식 시험(Corrosion Assay) / 이중서명(Dual Seal). worldview 가 미등재 4건을 용어집에 등재(id·KO·EN·정의). systems 는 이미 이 한 벌로 통일했다(C4-F9 closed 조건 = 등재 완료). **T0 는 KO 전용**이며 EN 문자열은 로컬라이제이션 회차 산출물이다 → 브리프 임포터 규칙 I-9·T-12 를 "EN 필드는 선택(없으면 KO 폴백), 용어집 등재 여부만 fail-closed"로 완화(systems).

### RFC-B6 · 밸런스 문서의 sha 문자열
- decision: RFC-Q1 적용 — `balance-sheet.md` §0.1·§3.2·§4.3 의 고정 sha/바이트 문자열을 "검증기 출력 인용"으로 교체(balance). 염판 수 24 실측 채택, `defect-register.md` C3-F2 행의 21 은 QA 가 정정.

### RFC-M1 / RFC-M3 · 모델링 착수 조건 · 그레이박스 색
- decision: 모델링 셸 착수를 막는 컨셉 재생성은 **`space-gate-three-mood` 1장뿐**(README 삽화·캡슐 후보는 모델링 입력이 아님). 그레이박스 6색은 디버그 전용으로 격하(승인). `assets/generated/3d/scripts/build_hub_greybox.py` 주석 정정은 **모델러가 직접** 한다(스크립트 저작자 = 모델러, assets/ 는 공유 폴더이되 저작 레인이 편집).

### RFC-C6-002 / C5-F2 · 집계 정본
- decision: **`qa/defect-register.md` 가 결함 상태의 유일한 정본**이다. `production/cycles/c{4,5}-development.md` 의 "S1/S2 open 0" 문장은 스테일 → 디렉터가 등록부 집계로 정정한다. `production/cycle-ledger.json` 은 회차 종료 시 등록부에서 스크립트로 재생성한다(`scripts/regen-cycle-ledger.py`, 디렉터 소유).

### C6-F2 · 상품 약속 문구
- decision: "결론이 바뀌는 추리"는 **철회**. 캠페인은 단일 임계경로 + 제출 관점 3갈래이며 씬은 갈라지지 않는다(설계 의도). 정본 문구: **"무엇을 근거로 결론에 이르는지가 손으로 바뀌고, 마지막에 세 관점 중 하나를 제출한다."** product(business-model·assumption-tests)·README·draft 가 이 문구로 통일. 결말 3갈래는 "선택"이 아니라 "제출 관점"으로 부른다.

### C6-F5 · 조작 밀도(표·칸 21/33)
- decision: 문서로 닫지 않는다. **위험 R-T0-1** 로 등록(초안 §11·balance-sheet §10): "조작 하위과제의 61% 가 표·칸 조작이며 손 조작 감은 미측정". T0 사람 검증 프로토콜(verification-plan)에 `manipulation_share`(조작 분 / 총 분)와 "손 조작을 재미로 꼽은 응답 수" 관측 지표를 추가(systems). 재설계 여부는 T0 결과 후.

### C6-F9 · 본 생산 승인 조건 통합
- decision: 계약에 "## Base production gate" 절을 신설(디렉터, 아래 실행)하고 초안 §6·브리프 ①·verification-plan 은 그 절만 인용한다. 조건: (a) T0 사람 검증 H-1~H-3 통과(12명/5유형, 탈락 포함 보고) **그리고** (b) 조위정합 스파이크 개념 검증, **그리고** (c) T0 실제 소요가 슬라이스 예상치의 150% 를 넘으면 STOP(범위 재설계), **그리고** (d) 열린 S1 0·G8 exit 0. 어느 하나 없이 본 생산을 시작하지 않는다.

### C6-F11 / PRE-1 / C7-F14 / C7-F12 / C7-F3 · T0 착수 전 결정 4건 + 실행자 규칙
- decision: ① 기준 HW: **미정 유지** — 개발 참조 기기는 이 세션의 Apple Silicon Mac(16 GB)이며 성능 수치는 캡처만 하고 PASS/FAIL 판정은 `hw_profile_id` 확정 후(PRE-1). ② Input: **Unity Input System 패키지(new)** 사용, `ProjectSettings` `activeInputHandler = 2(Both)` 로 T0 시작(레거시 UI 호환), 액션 맵 이름 `Watch`(노드 이동/조사/도구/프리뷰/확정/되돌림/증거함/힌트/뒤로). ③ 렌더: **URP** (2.5D 고정 시점·라이트 프로브 불필요, 포스트프로세싱 최소) — 버전은 [PIN-AFTER-RESOLVE]. ④ asmdef: 7분할(RFC-S2). ⑤ 실패 문장·`ReasonCode`: enum 은 systems 소유 정본(system-specs 실패 모드에서 도출), KO 문자열은 [TARGET] 자리표시자로 허용하되 `strings/ko.json` 한 곳에만 두고 DoD #5 는 "ReasonCode 집합 일치 + 문자열 파일 존재"로 완화. ⑥ 실행자(Codex)는 decision-log 를 쓰지 않는다 — RFC 는 `handoff/rfc-inbox/RFC-CX-{n}.md` 로 제출하고 디렉터가 append 한다(런북 3곳 정정, modeling). ⑦ 임포트 포맷: **GLB 가 런타임 플레이스홀더 정본**(도구 6종·허브), FBX 는 허브 셸 참고용 1개만 — 런북·브리프 통일, `SM_Tool_*.fbx` 는 만들지 않는다.

### C4-F11 · README 삽화 손글씨
- decision: `readme-verb-seal` 1장 재생성(concept, 네거티브 강화 "no handwriting, no glyphs"), `docs/media/verb-seal.jpg` 파생 갱신(디렉터). 나머지 README 미디어 유지.

### C4-F7 / C4-F12 / C4-F20 / C6-F12 / C6-F1 / C6-F3 / C6-F4 / C6-F16 / C6-F17 / C6-F6 / C6-F8 · 레인 배정 (R7)
| 결함 | 소유 레인(첫 레인) | 요구 |
|---|---|---|
| C4-F7 | vfx → motion | 두 계약을 interaction-rules §0-10·§5 정본과 대조해 수치·상태명 정정 |
| C4-F12 | systems | game-ui-contract `accessibility{}` 가 gdd §8 13행을 전부 커버하도록 키 추가 |
| C4-F20 | systems | 지속시간 분기 바인딩 3건 → 모드 분리/모디파이어로 교체(§0-11) |
| C6-F12 | systems | "되돌림 상한 없음"에 로그 상한(50k/8MB)·SV-F6 접힘 규칙을 브리프·초안에 병기 |
| C6-F1 | planner → synopsis | T0+`c1-b1` 안에 "오늘 밤의 목표 = 청문 제출 문서 1건" 을 명시하는 비트 문장(t0-b1 objective) 추가 |
| C6-F3 | planner | `hints[0]` 이 1단 정의(방향만)를 어기는 비트 전수 재작성 + 검증기 H-04(1단에 자료명·정답값 금지 어휘) 추가 |
| C6-F4 | planner → systems | 세션 재개 요약(리캡 패널) 스펙 1절: 마지막 체크포인트·현재 스테이지 목표·열린 가설 |
| C6-F16 | planner | 초안 §2 에 T0 공개 상한(RFC-P3-012)·캐논 시각·순서 앵커 요약 표 추가 |
| C6-F17 | planner → synopsis | `c2-b4`·`c6-b4` zoneId 를 본문과 맞추고 스테이지 `zoneIds` 에 추가(검증기 Z-01 재실행), `c1-b2`·`c5-b2` 본문 정정 |
| C6-F6 · C6-F8 | product | 포지셔닝 한 문장 + 가격 후보 3 숫자(미승인 표기) + 생성형 AI 공개·라이선스 UNVERIFIED 위험 행 |
| C3-F31 | qa | balance §7 재도출 확인 후 등록부 closed 동기화 |
| C7-F38 | systems | zones.md L58·59 괄호 [OBSERVED] 를 재현 가능한 명령·값으로 교체 |

### Unity 헤드리스 검증 정정 [OBSERVED]
- `production/receipts/unity-batchmode/`: 프로젝트 **생성** 배치 실행은 성공("Exiting batchmode successfully", create.full.log). **열기** 검증(open-validate)은 로그 34행에서 끝나 성공 문구가 없다 → "헤드리스 열림 확인"은 **미확인**으로 정정하고 재실행 결과를 같은 폴더에 기록한다. 브리프 ①-1 문장 정정(systems).
- decided_by: game-production-director · date: 2026-09-10


## RFC-CX-002 · 신규 리소스 제공자 MuAPI / Higgsfield

- date: 2026-09-10
- lanes: director, modeling, systems, concept
- source: 사용자 후속 지시 "you use muapi and heiggsfield for resources"
- decision: 신규 리소스 생성은 MuAPI와 Higgsfield를 사용한다. 사용자 직접 지정은 이전 GTI 전량 생성 절차보다 우선한다. 제공자별 모델과 입력은 실제 지원 스키마 및 자산 명세에 맞춰 선택한다.
- evidence: Higgsfield CLI가 설치되어 있고 `higgsfield account status`가 exit 0으로 완료됐다. 현재 MuAPI CLI/MCP 연결은 확인되지 않아 공식 인터페이스와 인증 준비 상태를 별도로 확인한다. 제공자 선택을 연결·생성 완료로 기록하지 않는다.
- preservation: 기존 GTI/Blender/Higgsfield 결과의 실제 provenance와 재현 레시피는 유지한다. 신규 호출에서 GTI로 자동 대체하지 않는다. 출처·모델·프롬프트·참조 입력·작업 ID·해시·확인된 비용을 기록하고 기존 runtimeEligible 감사 규칙을 따른다.
- changed_artifacts: CLAUDE.md §10.2; handoff/asset-runbook.md provider override; README.md 리소스 출처와 라이선스.
- execution_scope: 이번 후속 지시는 제공자 선택을 갱신한다. 구체적 신규 자산 배치를 임의로 생성하거나, 생성하지 않은 결과를 주장하지 않는다.
- decided_by: 사용자 직접 지정; game-production-director가 파일에 반영

### RFC-CX-002 연결 확인 후속 · 2026-09-10

[OBSERVED] 공식 MuAPI [authentication](https://muapi.ai/docs/authentication), [CLI](https://muapi.ai/docs/cli), [MCP](https://muapi.ai/docs/mcp) 문서가 HTTP 200으로 확인됐다. 공식 CLI 패키지는 `muapi-cli`, hosted Streamable HTTP MCP는 `https://api.muapi.ai/mcp`이며 Bearer API key를 사용한다. 현재 세션에는 MuAPI CLI/MCP가 연결되어 있지 않고 shell의 `MUAPI_API_KEY`/`MUAPI_KEY`는 미설정이다. 다른 보관 위치에 키가 없다고 단정하지 않는다. 안전하게 보관된 키로 통합을 연결해야 실제 생성할 수 있다. Sandbox mock 응답은 실리소스 생성 증거가 아니다. 이번 확인에서 패키지 설치·비밀값 출력·생성 호출은 없었다.


## RFC-CX-003 · Playable T0 구현 및 원본 리소스 제작

- date: 2026-09-10
- lanes: director, systems, worldview, synopsis, planner, modeling, vfx, QA
- source: 사용자 후속 요청 "make the game based on workspace and develop resource with blender, heiggsfield" 및 ecc/game-vfx/game-sounds/game-ui-ux 지정.
- scope: 기존 preproduction T0(hub, circuit, reader)를 실제 조작 가능한 Unity 슬라이스로 이어간다. 본편 production gate와 사람 검증 경계는 유지한다.
- RFC-CX-001 director ruling: worldview 제안 A 채택. 기존 허브 계통을 `system-hub`, 기록국 표준 관측소를 `station-bureau-standard`로 명시 저작한다. 실제 인용판 `rec-plate-standard-hub`는 두 ID를, `rec-tide-ledger-bureau`는 `systemId:null`과 표준 관측소 ID를 사용한다. plate.stationId 연결은 이번 저작에서 채택한 비교 기준 프레임이며 과거 관측 생산지·새 센서·사건의 발견이 아니다. 비인용 3레코드로 자동 전파하지 않는다. `plates.md`의 systemId는 plate일 때만 필수이므로 ledger까지 비-null을 강제하지 않는다. Worldview 등록→synopsis 귀속표→systems 생성기 파생 및 독립 QA를 거쳐 확정한다.
- circuit authoring: 기존 명세의 3점 AnchorOverlay와 단위 격자에 필요한 좌표는 planner가 명시 저작한다. 코드가 답 좌표를 발명하지 않는다. 전 점이 같은 offset으로 정렬 가능해야 하고 값·출처는 데이터에 둔다. 새 미정 precision step은 발명하지 않는다.
- resources: Blender로 누락 서랍을 기존 SM_Hub_Workbench의 부속 그레이박스로 제작한다. 기존 47-item 자산 신원을 임의로 48개로 변경하지 않는다. GLB 정본과 함께 동일 기하의 FBX를 Unity 임포트용 파생 후보로 허용한다. 기존 소스·provenance를 덮어쓰지 않는다. 자산별 실측/임포트 감사 전 runtimeEligible:false를 유지한다. Higgsfield는 현재 계정/모델/비용을 확인해 원본 게임 리소스 제작에 사용한다.
- skill routing: ECC 2.2.1 installed/enabled는 provider inventory로 확인; 설치/후크 변경 없음. game-sounds는 개발 에이전트 알림 도구로 판정, 런타임 음원 재사용 금지. 게임 오디오는 원본 제작/라이선스 검토 경로를 따른다. UI/VFX skill validator 통과는 디자인 계약이며 프레임 성능 또는 조작 검증의 대체가 아니다.
- decided_by: game-production-director, 사용자 게임 제작 요청 범위 내

### RFC-CX-003 addendum — M2 persisted Snapshot shape (2026-09-10)

Director approves systems proposal for commandLog.snapshots entries: `{seq:long,stateHash:string,stateBlob:{facts:string[],values:map<string,string>,readCounts:map<string,int>}}`. This fills the existing Snapshot[] serialization gap without a new save-root field or gameplay decision. Systems must document canonical ordering/hash, immutable reconstruction and replay-base validation and verify 200-commit/limit-fold behavior using the same representation. Authored citation source decision references remain RFC-CX-001 and RFC-CX-003; no separate RFC-CX-001A is created.

### RFC-CX-003 addendum — drawer r02 material correction (2026-09-10)

Concept ACKs the r01 shape and canonical palette but requests FIX for pristine surfaces, missing downward corrosion and edge salt; see concept/t0-source-drawer-review.md. Director authorizes a new r02 Blender candidate, preserving r01, and permits small baked texture assets to replace the r01 zero-texture blockout target within existing overall resource budgets. Keep canonical dimensions, 2-mesh separation where feasible and the 1500-triangle target; GLB remains canonical and FBX a derived Unity import candidate. Use the existing palette and material rules; no new rust token or gameplay/animation timing. Final style and runtime eligibility still require actual preview/import evidence, not recipe claims.

### RFC-CX-003 addendum — scoped source ACK evidence (2026-09-10)

Two-record provenance: worldview proposal, synopsis §12 and planning ACK are joined by QA in `qa/t0-m2-source-resource-review.md` and concept in `concept/t0-source-drawer-review.md`. The plate association remains a new comparison reference frame; ledger.systemId remains null. Root inspected the regenerated canonical records.json after the QA snapshot: both approved assignments are present and recordsDocSha256 equals the latest synopsis document SHA-256. C7-F50 runtime closure still awaits systems/QA execution.

Circuit packet dependency ACKs received: systems (authoring metadata), QA (`qa/t0-m2-source-resource-review.md`), balance (`balance/t0-circuit-review.md`) and economy (`economy/t0-circuit-review.md`). Presentation review remains pending. No ACK here is a human playtest, runtime asset promotion or campaign gate PASS.

### RFC-CX-003 addendum — circuit authored packet promotion (2026-09-10)

Presentation scoped ACK is recorded in `presentation/t0-circuit-vfx-review.md`. With systems, QA, balance and economy ACKs cited above, director accepts current promotion of the authored coordinate packet. Planner is recording the ACK links/status; JSON coordinates and hash remain unchanged. This is authored-data readiness only, not runtime readability, measured difficulty or G2/G4/G5 PASS.

### RFC-CX-003 addendum — r03 T0 runtime scene approval (2026-09-10)

Director inspected the native Unity Metal audit and all three captures in unity/Unknown/Builds/t0-diagnostics: standalone, original solid interference and front adaptation. The supported materials bind both base-color and roughness maps; the four1024px textures use sRGB for base and linear sampling for roughness. The imported asset remains2meshes/156triangles. Y180 adaptation makes the authored front face the scene camera. Subtracting the drawer AABB from the existing solid workbench restores the front recess distinction, as shown by the comparison captures.

Approve r03 for the T0 scene with the measured adapter yaw180 and drawer position(0,0.32,1.1) plus the reviewed workbench front adaptation. This is a placement/mesh-integration decision, not recovered world evidence, a new production identity or animation timing. Preserve source revisions. The current observed rendering project is Gamma on Metal; this approval does not establish final frame budgets, production G4/G5 or standalone-player appearance. Systems may now activate this exact candidate in the T0 player; root will inspect the resulting player screen.

## RFC-CX-004 · C1 opening patrol circuit (2026-09-11)

- Trigger: user requested continued game development after T0 M2.
- Scope decision: implement canonical c1-b1 only, preserving the existing T0 path and saves. The remaining C1 beats retain their authored status; no full-chapter or measured-duration claim is made.
- Evidence: planning/campaign.json C1/c1-b1; synopsis/chapter-beats.md c1-b1; production/codex-c1-m3-status.md.
- Proposed transaction: preview branch changes without granting access; accepted confirmation durably records gate access, the existing source-attribution condition, and checkpoint cp-c1-b1. Invalid configurations remain explainable and reversible. Planner contract and systems compatibility review are pending.
- Resource: original Blender patrol distribution panel; runtimeEligible:false until visual review. Existing uncommitted renderer/occlusion work is preserved and identified separately.

### RFC-CX-004 addendum — canonical transaction and prop target

Director ACK after planner review: one chapter confirmation/save applies branch state, gate access, source-attribution journal condition and cp-c1-b1 together. Circuit folding remains reversible preview; free bypass recovers a safe preview and never grants progress or replaces observations. This follows the existing wiring/interaction save doctrine and avoids unauthored partial completion states.

The Blender asset `c1-patrol-panel` depicts the existing 수문 계통판 in canonical clue c1-b1-c2; its ID is an internal production name. There is no new lore term. A dedicated concept sheet is absent: for this scoped blockout, the canonical clue, current concept/style-guide.md and director brief define the visual target. This deviation does not approve final concept art or campaign G4/G5.

### RFC-CX-004 addendum — save compatibility and implementation ACK

Director ACKs systems architecture: import a separate validated c1-b1 packet with explicit nonempty requirements; do not append raw campaign beats that could auto-complete. Keep existing T0 command identities and snapshot hash semantics, namespace C1 preview, and derive stage/beat/save metadata from the journal. Add explicit Continue to C1, stage-aware evidence presentation and a clear current-demo endpoint; hub-only drawer interactions remain gated to hub.

Save schema v2 is approved with explicit backup-preserving v1→v2 migration and future-version refusal. Rationale: an older T0 binary must not treat a C1 command as replay corruption and silently fall back to a stale v1 backup. Tests must cover an actual completed prior v1 save copied into an isolated fixture, invalid branch/observation guards, atomic commit/undo/replay/retry and bypass noncompletion.

### RFC-CX-004 addendum — initial branch preview

Director ACKs initial lighting=true and reader=true as a prototype initialization choice: it exposes the authored shared-supply conflict and makes folding the lighting branch meaningful. Planner records preview.initialConfiguration in the packet; systems reads it from data. This is an implementation choice consistent with the authored inference, not a measured pressure value or new narrative fact. Free bypass remains lighting=false, reader=true and grants no completion effects.

### RFC-CX-004 addendum — Blender generation inspection

Root operator executed Blender 5.1.2 with a fresh factory scene. First attempt failed during material-group joining (StructRNA reference removal); failed textures/log/source snapshot are preserved in assets/generated/3d/c1-patrol-panel-r01-attempt1. The corrected recipe completed with --python-exit-code 1, SUCCESS.json and a terminal success marker.

Successful resource:6 meshes, 2,092 triangles, four 1024-square maps totaling 1,615,402 bytes. Root inspected preview.png: round lamp housing and rectangular readout are distinguishable, both connect to the central supply, material wear follows the maritime bluegrey target, and no baked labels/numeric pressure values are present. All output/recipe/provenance hashes verified. This approves native diagnostic import only; runtime promotion awaits the native Unity capture.

### Native panel promotion — 2026-09-11

[OBSERVED] Director inspected diagnostic #3 front and alternate PNGs in systems/tech-verification/c1-m3/panel. The adapter is upright, retains the six meshes and texture maps, shows the shared connection clearly, and separates the warm lamp lens from the readout recess. Approve this exact static c1-b1 panel/adapter for runtime use. Blank plaques and readout are decorative; the UI remains authoritative for switch/readout state. Full G4/G5 and performance remain unmeasured. Original generation provenance and SUCCESS pair are preserved under assets/generated/3d/c1-patrol-panel-r01/generation-receipt/. The live provenance records capture hashes and this scoped promotion.

## RFC-CX-005 · C1 signature reader and art continuity (2026-09-11)

- lanes: planner, systems, presentation, modeling, QA; balance/economy carried because no currencies or authored numeric balance changes.
- request: Continue c1-b2 with resources/art, consistency, presentation and logic as explicit priorities.
- scope: c1-b2 only, continuing from shipped c1-b1 (ae098e0); no c1-b3/C4 identity reveal and no chapter-duration or full-game gate claim.
- canonical evidence: planning/campaign.json c1-b2; current reader/save specs; concept/style-guide.md.
- decision: ACK planner's three ordinal humidity choices (low/medium/high), initially unset, with low enabling separation and other levels producing reversible ink-risk preview. This is a labeled implementation inference, not a humidity percentage or additional lore. Canonical lowest-step hint remains opt-in; default guidance must not reveal the correct step or branch.
- first observation preserves the joined source; both post-separation verified copies must be made explicitly, inherit signature-annex, and never count as independent proof. Plate-zero comparison requires the distinct plate observation and explicit player comparison; no inferred signature identity is revealed.
- hidden region: approve a document-local normalized lower-sheet rectangle x=0.12, y=0.70, width=0.76, height=0.22, origin top-left. These are authored presentation coordinates for the visible paper surface, not a measurement of fictional historical evidence. Mark the same region on original/copy with a hatch and plain unresolved label; preserve obscuration after completion.
- completion: accepted-save receipt atomically publishes both copy receipts, unresolved region annotation, distinct-source plate-band link, completed c1-b2 and cp-c1-b2. All preview/recovery is free; no premature completion on failed/cancelled save or direct immediate submission.
- save architecture: ACK systems proposal for schema v3, backup-preserving v2 and v1 migration, byte/identity preservation of existing commands, and future-schema refusal. A shipped v2 binary must not silently replay new c1-b2 commands and fall back to a stale save.
- art intent: approve presentation lead's blank damp paper substrate via Higgsfield and static Blender reader/humidity tray assembly in existing palette/material language. Exact state reveal remains in Unity renderer driven only by authoritative snapshot; no generated lettering/numerals/signatures, no static prop presented as live simulation. Assets start runtimeEligible:false; native visual inspection precedes scoped promotion.
- evidence boundaries: include native edit/play tests, actual prior-v2 standalone route/restart, 150% text and reduced-motion capture; production G4/G5/performance/fun remain unmeasured.
- continuity: preserve the pre-existing unstaged C1PlayModeTests.cs non-spoiler regression (observed mtime 2026-09-11 02:08) and complete its intended fix; no concurrent coding process found at intake, Blender UI left untouched.
- status: implemented and independently verified within c1-b2; final isolated source and native restart approved.
- memory: mex-agent unavailable pending identity check; no zg index mutation authorized.


### RFC-CX-005 resource receipt — Higgsfield paper

[OBSERVED] Higgsfield 1.1.23, GPT Image 2.5 high/1k, job 54b52722-337a-42fc-baa4-e289c64db09e completed. Requested and actual 1024×1024; cost quote 3.5 credits and account delta 120.68 → 117.18 = 3.5. Exact prompt, schema and response are preserved in assets/generated/2d/texture/c1-signature-paper-r01/. Full-resolution visual source inspection found no generated text, numerals, signatures or clue marks. Approved for diagnostic import only; runtime eligibility remains false pending native inspection and provider terms review.

### RFC-CX-005 r02 geometry correction

[OBSERVED] Independent native review found a disconnected swing arm in r01 (rotation sign). ACK the minimal r02 derivative: same source recipe except revision/output path and arm Z rotation −0.47 → +0.47. Preserve r01 as rejected diagnostic evidence. Root re-executed and hash-verified r02; native promotion must reference r02 and final Unity adapter. No new lore, motion or gameplay state is introduced.

### RFC-CX-005 native reader r02 approval

[OBSERVED] Director inspected the r02 native Metal capture in systems/tech-verification/c1-m4/reader/reader-front.png (SHA-256 e22f401da9e2a3751448056d23ad66eb31ec83959dc520be16435634da769c45). The elevated fixed front view exposes the circular cradle, lens opening and right treatment tray; corrected arm connects the rear post to the lens neck. Muted weathered materials load correctly on a neutral dark background. Approve this exact static r02 housing/Unity adapter for c1-b2. Paper and its state layers require separate native inspection. This is scoped asset approval, not G4/G5/performance certification.

### RFC-CX-005 native paper approval

[OBSERVED] Director exercised diagnostic-3 standalone at 1280×800 requested viewport, 150% text and reduced motion using actual previous v2 save. Original, separated, individually copied, marked, compared and completed paper states kept the lower obscuration without exposing a name. Labels and mask remained inside the viewport after layout repair; original/individual copies were distinguishable. Approve the exact Higgsfield blank paper (SHA-256 0b35b75810e2bb052b580b4671541e06a92e68169e2c70ee3af3e66f02e66efe) and this UI state adapter for c1-b2. Evidence: systems/tech-verification/c1-m4/player-final-smoke/native-observation.json. The static reader r02 approval remains in force. Normal resource view may now be enabled and final app built; later pointer/fixed-summary corrections require native spot checks. This is not human accessibility/fun or performance proof.

### RFC-CX-005 final delivery verification

[OBSERVED] Isolated HEAD+65 approved Unity source passed EditMode36/36, PlayMode39/39 and serialized boot1/1. Concurrent external patrol fallback/string/test edits remain in the workspace and are excluded through two isolated index blobs and HEAD T0Strings. Final copied app is348,163,868 bytes (316 files; bundle fingerprint af69e25e5c1f677c9203cee0600ca68998ee25c4d2bbaf2430c1b7b38af51738). Ordinary-runtime restart restored c1-b2 completion, two copied papers, unresolved lower mask and correct resumed status with unchanged save bytes. Scope-approved implementation/resources may be committed and pushed per the user’s standing request. Human playtime/fun/accessibility, physical pad, auditory approval, performance and full G4/G5 remain unmeasured.

## RFC-CX-006 — Image generation uses god-tibo-imagen (2026-09-11)

- Authority: explicit user instruction to generate images with the god-tibo-imagen skill; this supersedes the earlier MuAPI/Higgsfield selection for new images only.
- Blender 3D and Higgsfield video workflows continue. Preserve every existing asset's actual provenance and runtime approval. No automatic replacement of approved art.
- [OBSERVED] GTI 0.3.0 and valid local Codex login verified without exposing credentials; dry-run and generation succeeded using requested model `gpt-6-astra`. No image-model identity or quota charge was returned.
- [OBSERVED] Original blank-paper candidate: `assets/generated/2d/texture/c1-signature-paper-gti-r01/`. Requested 1024×1024; actual 1254×1254, 2,017,998 bytes. SHA256 `5dd394fe188e287f1a81f73e8d5761d1807c6b9d5f711f15d93c0ef8b1c80cae`. Source and revised prompts, sanitized dry-run, execution and output receipts retained.
- [OBSERVED] Independent visual review passed the blank-substrate criteria. Size differs from request. Keep `runtimeEligible:false`; native readability/import and rights review remain unmeasured. This RFC authorizes the provider and candidate generation, not runtime promotion.
- Handoff: `production/image-provider-gti-20260911.md`.

## RFC-CX-007 — Video-led intro and gameplay direction (2026-09-11)

- Authority: user requested intro/gameplay videos, then direction/resource derivation and gameplay application. Images use GTI; video uses Higgsfield. This is a bounded M5 continuation, not full campaign completion.
- [OBSERVED] Generated two Seedance2mini clips; each actual6.041667s/1280×720/24fps/noaudio. Intro job`5c8b8815-fece-4459-807d-c9cef3ba2959`; C1job`8254dc3e-b7cc-405c-9019-ee33f4205f4d`. Quotes15+15credits; account balance117.18→87.18(delta30), not an itemized transaction audit.
- [OBSERVED] Intro measuredcut3.125s adopted as native3125+2875=6000ms two-context presentation. Use clean GTI hubr03 only. Reject generated postcut linedtraypaper/dialmarkings, and log horizonapprox25% versus requested40%.
- [OBSERVED] C1 cuts2.083333/4.041667 informed observation/trial/record spatial orientation. Reject automaticpaper-clearing, inventedinventory, knobaction andcameradrift. Actual lower mask, evidence and receipt state stay authoritative.
- [OBSERVED] New GTI UI surface was generated after the completed C1video, from its2.5s frame. Source video/frame/output hashes and execution timestamps are in `systems/tech-verification/intro-gameplay-m5/art-and-video-receipt.json`.
- [APPROVED DESIGN] Fresh-onlyskippable intro with settings access; existing-save bypass; release latch; reduced-motion-static information;150%text; staticC1orientation reads actualselection/trial/acceptedrecord. No runtime movie dependency, animation-gatedprogress, camera travel or new solverorder.
- [INTERNAL DIAGNOSTIC ONLY] Clean hubr03 and darkmetal surface may be imported into isolatedM5 for native inspection. `runtimeEligible:false` and profile runtimeApproved=false until native acceptance is recorded below. Rights review separates output ownership from GTI support/route and commercial clearance; no Steam-release clearance claimed.
- Sources: `presentation/intro-gameplay-m5.json`, `presentation/intro-gameplay-m5-video-review.md`, `qa/intro-gameplay-m5-review.md`, `qa/intro-gameplay-m5-rights-review.md`.

### RFC-CX-007 native prototype promotion — 2026-09-11T07:13:55.671Z

[OBSERVED] Diagnostic native build fingerprint 18066f11cd0a556758916d9c654e7b293d44cfc7b26cb0e683783090ca92e629 passed fresh intro150%/reduced-motion readability and settings retry, v2 direct continuation/one-observation trial/mask retention, and v3 completed-save restart (57 commands; save hash unchanged). Automated diagnostic suites: EditMode36/36, PlayMode53/53, separate boot1/1.

[DECISION] Promote only GTI hub source r03 (24a2e45c733fba2a43bffd602906358191479f47e183355001de36c691d7a5e2) and video-derived UI surface (52c4b3cc65cc1432bd94c5e7207ea35a50cbd5d8507f98e9c27f760db590b414) for bounded internal prototype runtime. Preserve pre-runtime provenance; commercialReleaseEligible remains false. Generated films and other stills remain previz-only. The UI source1254² is imported with max1024; intro1672×941 retains source dimensions.

[PENDING] Final runtimeApproved:true suites and ordinary native build/capture must pass before publication. No human-playtest or performance claim. Native evidence: systems/tech-verification/intro-gameplay-m5/native-acceptance.json; rights boundary: qa/intro-gameplay-m5-rights-review.md.

### RFC-CX-007 final ordinary delivery

[OBSERVED] Final ordinary build integrates upstream2ece517 with runtimeApproved:true. EditMode36/36, PlayMode54/54, serialized boot1/1 passed with no diagnostic override. App fingerprint484ab029f0bf29b4af6ffdd2bb9587ca6e08a432e09dde979d3635517b84d736 (316files;353421261bytes). Fresh intro naturally completed without creating gameplay save; C1 recording reached one accepted confirmation (54commands); completed v3 restart preserved57commands and save bytes. Native films are window-only recordings, separately labelled from generated previz.

The approved-profile requirement is fulfilled. Final native/video receipts retain diagnostic and ordinary build identities separately. Prototype-only source promotion remains bounded; generated footage is not gameplay evidence. Graphify updated; mex-agent unavailable remains [UNGRAPHED].


## RFC-CX-008 — Higgsfield MCP cinematic and play-method films (2026-09-11)

- [OBSERVED] Latest user request explicitly selects Higgsfield MCP and asks for intro, stage-unlock and immersive gameplay-method videos. M6 stays within the current preproduction scope and does not modify Unity.
- [OBSERVED] Official https://mcp.higgsfield.ai/mcp used for model/cost/upload/confirm/generation/status/balance. Existing authenticated CLI was used only to obtain the private bearer credential in memory; no credential was printed or committed. Seedance 2.0 Mini generated four reference-led clips, all completed. Quotes total60credits; observed account balance81.18→21.18, delta60.00. This is an account observation, not an itemized invoice.
- [OBSERVED] GTI produced two new reference frames; existing GTI duty-room art and Blender reader preview complete the four-shot reference set. Each raw clip is1280x720,24fps,145frames,6.041667s video/6.08s container with AAC32kHz audio. Provenance binds actual dimensions, hashes and request/job IDs.
- [OBSERVED] Delivery: docs/media/cinematic-gameplay-m6/cinematic.mp4 (36s) and gameplay-method.mp4 (54s), both1280x720,30fps. Manual Korean captions, progression rules, spoiler-value overlays and labeled M5 native inserts explain observation, trial, copy preservation and save results. Primary authored body text is32px or larger; small source labels are auxiliary.
- [DECISION] T0-b1→b2→b3→C1-b1→b2 is the only current implemented chain. T0-b1/b2 have no invented commit animation. T0/C1 earlier live-save events were absent from source capture, so the edit presents manual rule cards. Native patrol footage shows restored completed state and explicit entry, not fresh unlock evidence. Ready and saved results remain separate excerpts; final saved result does not unlock C1-b3.
- [OBSERVED] Producer/reviewer fixes enlarged captions and removed two-frame cumulative cut drift. Final frames1080/1620; all19segment boundaries match captions; full video/audio decode passed;10gallery asset links resolve. Native ready ends frame1409, saved-result segment begins1410(47s). Final independent editorial review records scoped passes and unmeasured targets separately.
- [DECISION] Raw provider camera travel exceeded small-motion targets; clips remain cinematic previz, not runtime motion proof. Generated AAC is preserved but excluded pending listening approval. Final films use authored filtered pink-noise ambience with no speech, melody or save-success stingers; measured-27.19/-27.13LUFS, peaks-15.73/-14.87dBTP.
- [BOUNDARY] runtimeEligible:false and commercialReleaseEligible:false throughout M6. No Unity implementation, human immersion/playtest, human listening approval, full campaign unlock evidence, numerical caption contrast certification or new market claim. This is a video/resource delivery, not G4/G8 release approval.
- Sources: assets/generated/previz/cinematic-gameplay-m6/provenance.json; presentation/cinematic-gameplay-m6.json; systems/tech-verification/cinematic-gameplay-m6/{mcp-production-receipt.json,validation.json,editorial-review.md}; docs/media/cinematic-gameplay-m6/edit-timeline.json.


## RFC-CX-009 — Original-concept-first films and GTI resource rebuild direction (2026-09-11)

- [OBSERVED] User corrected source authority: keep consistency with existing worldview and original concept images; do not reference current gameplay, prefabs or resources, because the project resources/prefabs are to be rebuilt to match the video. Textures/images will use GTI.
- [DECISION] The authoritative flow is lore/originalconcept → cinematic target → GTI textures/resources → prefab reconstruction. The4original image SHA allowlist is concept/concept-first-m7-sources.json; currently implemented Unity/native/3D and M5/M6-derived visuals are forbidden future visual parents. Older evidence remains preserved. CLAUDE.md and README now state this direction.
- [OBSERVED] GTI generated m7-optical-workbench-r01 from exactly2originals (readerhero +watchroommood), requested2048x1152/actual1672x941. Root separately approved its lineage for I2V only. It preserves the circular saltplate reader, articulated optics and ochre crank in the original expansive wet-metal watchroom.
- [OBSERVED] Official Higgsfield MCP produced5 Hailuo2.3Fast takes:4initial +1hand-material revision.4selected, flat/plastic-hand r01 excluded as final art reference. Quote16+4credits; account21.18→1.18, observed20. Allrawtakes24fps141frames5.875s,silent. S1/S2=1364x768;S3/S4=1152x768 despite16:9request, recorded individually.
- [OBSERVED] Final cinematic24s576frames and method32s768frames at720p24fps use only selectedM7clips. Eachclip gains3frozen lastframes(.125s). Uniformscale/top-aligned16:9crop retains gatewinch/sea horizon and watchroom context; no stretched geometry or native footage. ManualKorean captions and new temporary filtered-noise ambience were authored locally. Full decode/frame checks and strict5input ancestry checks pass.
- [OBSERVED] Independent art sampling accepted material/mood continuity and readable primary text. S2r02 skin is improved; exacthandrig/crankreturn and saltplate geometry remain unverified. S1partialprofile is a staging deviation, not portraitapproval. S3is recordcontext only; S4remainsclosed and is destination context, not observed savedrecord or unlockedstage.
- [DECISION] Production texturemaps, meshes, handrig and Unity prefab reconstruction remain subsequent work under handoff/concept-first-m7-resources.json. Video staging informs that work; sourceconcept/GTIstill controls rigidgeometry and surface marks, so generated motion distortions are not baked into assets. Runtime/commercial eligibility remainsfalse; no new Unity or humanimmersion claims.
- Art review: systems/tech-verification/concept-first-m7/art-review.md. Provider receipt and final validation are adjacent. Film gallery: docs/media/concept-first-m7/index.html.

## RFC-CX-010 — Source-informed offline review notes (2026-09-11)

- [OBSERVED] User asks to apply the WeChat article https://mp.weixin.qq.com/s/YmqCm2Hh8l6WPTIAIEMFhQ to game development. Public full-body retrieval and a second independent source read support the adaptation; market figures remain source-reported, not independent measurements. Source receipt: systems/tech-verification/ai-native-m8/source-review.json.
- [DECISION] Add optional player-authored review notes, available evidence links and authored next-check questions. This is deterministic offline gameplay support; no semantic grading, AI verdict, hidden evidence disclosure, model calls, inference credits or monetization changes. Note use is never required for progression.
- [DECISION] Preserve simulation, journal/save schema and accepted-save unlock authority. Store notes separately with explicit draft/error states; text entry must suppress gameplay shortcuts. Source identities and media relationships must come from canonical data, never inferred from naming or the player's prose.
- [CARRIED] RFC-CX-009 original-concept/M7 video authority remains in force. M8 adds a note/card staging brief and GTI future resource requirements, without adopting current prefab/runtime art or third-party article images.
- [OBSERVED] Actual role exchange: messages/006-director-m8-reference-decision.md. Design: planning/ai-native-m8-reference-application.md; presentation/ai-native-m8-direction.md. Implementation/native validation receipt follows separately; this decision is not proof of completion.

### RFC-CX-010 delivery evidence — 2026-09-11

- [OBSERVED] Optional review notes implemented with observed-source links, authored next-check text and separate per-save atomic storage. No model calls, semantic verdict, hidden evidence or progression authority. Existing unsaved drafts are labeled session-only.
- [OBSERVED] Final exact-source tests: EditMode46/46, M8 subset10/10 within broad PlayMode64/65, boot1/1 and macOS development build success. The one M5 reduced-motion failure reproduces on base01bec3c with M8 removed; existing dirty M5 fixes remain excluded.
- [OBSERVED] Native clipboard→Tab→save smoke uncovered a text-event bug missed by initial synthetic tests. New test failed against the old UI; the corrected UI passes and final native persisted text is exact, with gameplay save bytes unchanged. Receipts: systems/tech-verification/ai-native-m8/verification.md and verification.json.
- [CARRIED] Full physical IME/controller QA, human engagement and complete-campaign gates remain open. M7 original-concept→film→GTI→prefab resource direction is unchanged, with no new art/provider spending in M8.

## RFC-CX-011 — M9 완성도 hop: 코어루프·튜토리얼·리소스 반영 연출·밸런스 (2026-09-11)

- [OBSERVED] 사용자 지시(2026-09-11, 동일 문안 반복): 서브에이전트로 지금까지의 작업을 확인하고, 게임 코어루프·튜토리얼·리소스 반영 연출·밸런스를 고려해 완성도를 높이며, 워크스페이스 구현 단계의 hop을 한 단계 깊게 강화한다. hop 해석: T0(M1·M2)→C1(M3·M4)→M5→M6→M7→M8로 이어진 구현 마일스톤 사다리의 다음 단(M9). M9는 문서·previz가 아니라 **기존 계약(TARGET)의 런타임 구현**으로 한 단계 내려간다.
- [OBSERVED] 3레인 병렬 감사(시스템/밸런스/연출, 각 READ-ONLY) 완료. 근거: 본 세션 감사 보고 3건 — 코어루프(미세루프 4단계·2층 분리 성립, 힌트 기록 미저장 G1, 프리뷰 정적문 G3, M8 TARGET 3건 미구현 G5), 튜토리얼·밸런스(guided 티칭 데이터 `toolTeaching`/`introBeatId` 소비처 0건 G6, 기본 확정 방식 `confirm-dialog` ≠ 정본 `two-step` G5, 비트 목표 미노출 G7, snapshotInterval 노브 사장 G9), 리소스·연출(M7 materials 미생성 G1, 검토 노트 텍스처 슬롯·전환 시스템 부재 G2/G3, 초점 복귀 불일치 G4).
- [DECISION] M9 범위 (전부 기존 정본 계약의 이행 — 새 밸런스 수치 발명 0건):
  - S-A 힌트 영속화: 비트별 hintLevel 사전, 저장 `progress.hintLevelUsed` 왕복, OpenOverlay 리셋 제거, 경고 게이트를 `warnsBeforeReveal` 데이터 구동으로. (interaction-rules §4, balance-sheet §6, hint-system.md)
  - S-B 기본 확정 방식 `two-step` 정합 (interaction-rules §1-1). 기존 세이브 settings.json 불변.
  - S-C 런타임 프리뷰 diff: preview 오버레이에서 `T0Simulation.Preview` 차이를 문장화 (GDD §3.3 원칙1).
  - S-D guided 티칭 최소판: `toolTeaching` 비트의 도구 패널에 1단 힌트+잔여 술어 집계 병기, CaseThread 목표를 비트별 objective로 (campaign.json t0-b2/b3 안내 약속, beats.json 기존 데이터).
  - S-E M8 TARGET 3건: 명시적 `검토 질문 보기` 버튼(누를 때 갱신), 출처 원문 열기+검토 노트 복귀, 독립 매체 부족/충족 질문 분기 (ai-native-m8-reference-application.md 표, ai-native-m8-direction.md).
  - S-F 검토 노트 연출: M8ReviewNotesProfile SO(runtimeApproved 게이트)+카드 배경 텍스처 슬롯+열림/닫힘 180/140ms CanvasGroup 페이드+reduced-motion 0ms+직전 오버레이 복귀. 수치는 `Resources/M8ReviewVfx.json` 저작.
  - S-G 소형 계약 마감: snapshotInterval policy 전달(G9), 유휴 힌트 제안 타이머 document 열람 중 정지(G4a).
  - S-H GTI 리소스 1건: M7-MAT-rag-paper 방향의 검토 카드 바탕(글자·증거 표식 없음) 생성 → `assets/generated/2d/texture/m8-review-card-r01/` provenance(runtimeEligible:false) → 빌더로 Candidates 임포트(runtimeApproved=false) → 네이티브 검수 후 별도 승격 판정. 참조는 concept-first-m7-sources.json allowlist 원본만.
- [DECISION] 제외: emit-tables 재생성이 필요한 `hintOfferCooldownSeconds` 노브 분리(G4d)와 EN 힌트 textKey 연결(G10)은 M9 범위 밖 개방 항목으로 이월. alignment 신규 도구 동사(G6 전략)는 콘텐츠 저작이 따라와야 하므로 기획 재-인테이크 대상. ReadOriginal 마모 undo 복원(G7 판정 필요)은 본 RFC에서 판정하지 않고 개방 RFC로 남긴다.
- [DECISION] 파일 소유 경계(병렬 세션 안전): SYS-CORE = T0GameSession.cs·T0OpeningSession.cs(문자열 L() 이관 시)·JournalSave.cs·T0Strings.json·T0ContractTests/T0M2Tests·M9 신규 EditMode 테스트. SYS-M8 = ReviewNotesSession.cs·T0ReviewNotesInterface.cs·신규 M8ReviewNotesProfile.cs·M8ReviewNotesProjectBuilder.cs·M8ReviewVfx.json·ReviewNotesTests/ReviewNotesPlayModeTests. 공유 지점은 `OpenReviewNotes()` 진입 메서드 하나로 계약: SYS-M8이 ReviewNotesSession partial에 정의하고 SYS-CORE의 T0GameSession.cs:164 액션이 그것을 호출한다.
- [CARRIED] 기존 미커밋 변경(M5 reduced-motion fix + M8 소스)은 선행 세션의 완료 작업으로 보존하고 그 위에 얹는다. 커밋/푸시는 사용자가 수행. mex-agent identity probe 실패 시 degraded G8 PARTIAL 유지.

## RFC-CX-012 — TRACE-RPG 방법론 이식: 증거 그래프 계약 · aside 용어 판정 · 내러티브 브리지 (2026-09-11)

- [OBSERVED] 사용자 지시(2026-09-11): subagent로 `../neural_symbolic_in_game/`(TRACE-RPG) 방법론을 이용해 게임의 일관성·증거 로직이 깨지지 않도록 그래프 형식으로 코어 증거해금·흐름을 유지하고, 용어는 세계관에 맞게 aside에 요청해 바꾸되 주요 용어는 유지, 게임플레이 흐름을 잇는 내러티브 구성을 개선한다.
- [OBSERVED] 방법론 원전: TRACE-RPG README(생성 이벤트=미신뢰 트랜잭션 제안, 결정론적 커밋 게이트 7검사/6가족, 실패 시 상태 불변, 해시 연동 영수증, 진단적 ledger 문법 [P]/[C]/[H]/[N])와 KG 시뮬레이션 계약(typed 노드/엣지, closed-world, 온톨로지 위반 0 검사).
- [DECISION] 산출물 3계열 (전부 status: draft — QA 독립 재검증 후 승격 판정 별도):
  - **G-A 증거 그래프 계약** (planner): `planning/emit-evidence-graph.mjs`(campaign.json→typed 그래프 파생, provenance 수록) · `planning/evidence-graph.json`(노드 152 = stage 9·beat 33·clue 73·source 31·tool 6, 엣지 277) · `planning/evidence-graph-overlay.json`(반전 seed/reveal 6팩트: R1→c2-b3, R2-fact→c4-b2, R2-effect→c4-b3, R2-intent→c6-b4[RFC-W4 준수], R3→c6-b4, order-anchor→c6-b3 [TARGET 저작 — worldview 캐논 대조 ACK 대기]) · `planning/validate-evidence-graph.mjs`(TRACE-RPG 6가족 번안 EG-PROV/PRE/STAGE/REACH/KNOW/TOOL/DISC 18검사). 실측 [OBSERVED]: EG-SUMMARY 18/18 PASS · 2회 실행 byte-동일 · /tmp 사본 결함 4종 주입 시 7검사 FAIL·저장소 무변경(공허 통과 아님 확인) · validate-campaign.mjs 49/49 PASS 병존. 이 검증기는 캠페인 검증기를 대체하지 않는 **추가 게이트**다.
  - **G-B 용어 판정** (worldview 감사 + aside 협의 + 디렉터 판정): `worldview/term-audit-20260911.md`(A 미수록 16·B 톤 미정합 15·C 유지 확인 13, campaign 노출 문자열 453건 스캔) → aside exec 자문 회신 → `production/term-decision-aside-20260911.md`(최종 선정 표). 디렉터 가감 3건: ① 훈련→연습 흡수(「연습 서식」·「연습 압착」 — 용어집 §3 「연습」 정의가 aside 확인 질문에 답함), ② B-05 aside의 "보호 지정 5장" 기각(원문 "5장"은 章 — "5장의 보호 지정"으로 정정), ③ aside 미회부 기계적 통일 12건은 감사 후보 ① 채택. 유지 13행 침해 0 [OBSERVED].
  - **G-C 내러티브 브리지** (synopsis): `synopsis/narrative-flow-bridges.md` — TRACE-RPG ledger 문법을 세계관 언어로 번안한 33비트 × 3줄(이음/해금 예고/보류 문구) 저작. timeline §7 공개 상한·오해 유지·B23/B28 금지열 준수 자기 검사 포함.
- [DECISION] 적용 경계: 본 RFC 시점에 **정본 무변경** — glossary.md·campaign.json·timeline.md·validate-campaign.mjs 전부 그대로 [OBSERVED, git status]. 문자열 교체·용어 등재는 아래 ACK 후 별도 편집 회차: (a) planner — completion/objective 필드가 표시 문자열인지 검증 술어인지 재분류(B-11·B-12·B-14 종속, **선결**), (b) worldview — 오버레이 seed/reveal 캐논 대조 + §4 신설 등재 대기 목록(term-decision §4), (c) systems — UI 계약 `information_hierarchy` 「체크포인트」→「복귀 지점」 반영 여부.
- [DECISION] 게이트 연결: G-A 검증기는 G1(세계관 일관성)·G7(코어루프)의 **문서 수준 기계 검사**를 보강한다. 런타임 게이트 아님 — 회고 "빌드 0줄" 한계 그대로. campaign.json이 바뀌는 미래 회차는 validate-campaign 49검사 + validate-evidence-graph 18검사를 함께 통과해야 한다.
- decided_by: game-production-director · 영향 레인: planner(ACK-a), worldview(ACK-b), systems(ACK-c), synopsis(브리지 저작 완료), qa(승격 검증 대기)

### RFC-CX-009 delivery evidence — GTI 재질·Blender 블록아웃·pre-runtime QA (2026-09-11)

- [OBSERVED] 사용자 지시(2026-09-11): M7 시네마틱을 바탕으로 GTI 텍스처와 Blender 3D 리소스를 생성·적용. 후속 지시로 서브에이전트 병렬 진행과 마무리를 요청.
- [OBSERVED] GTI 재질 5종 생성 완료 — `assets/generated/2d/texture/m7-{bronze,perforated-steel,salt-concrete,salt-crystal,rag-paper}-r01/`. dry-run 선행, `--model gpt-6-astra` 요청 1024x1024/실제 1254x1254(요청·실제 분리 기록), 참조는 concept-first-m7-sources allowlist 원본 4장 + 승인 키프레임(`8bdf991d…`)만. basecolor만 GTI 산출이고 roughness/height/microdetail은 휘도 파생 후보로 provenance에 구분 기록. 백엔드 크레딧 소모량은 이 영수증에 노출되지 않으며 0으로 기재하지 않는다.
- [OBSERVED] Blender 5.1.2 headless 블록아웃 — `assets/generated/3d/concept-first-m7/concept-first-m7.blend` (재실행 스크립트 `scripts/blender/build_m7_assets.py`, 4 컬렉션 M7-ENV-watchroom/M7-PROP-optical-reader/M7-PROP-record-set/M7-ENV-gate-three, 236 오브젝트, box-projection으로 5재질 적용, 텍스처 pack). Cycles 검수 렌더 3장 `renders/`. M7-PROP-player-hand는 핸드오프대로 후속 리깅 단계로 제외.
- [OBSERVED] 서브에이전트 3레인 pre-runtime QA:
  - 타일 이음새(`m7-tiling-qa-report.{json,md}`): rag-paper 8.53/10.57 최우수, bronze·perforated-steel·salt-crystal needs-edge-blend, salt-concrete Y축 Δ67.76 not-tileable(조수선 수직 그라디언트 — 좌우 타일 전용 설계, X=20.03 경계). 어느 재질도 무보정 tileable-candidate 아님 — 인엔진 검사 전 엣지 블렌드 필요.
  - 가동 테스트(`articulation/`): S02 전진→멈춤→원위치를 48프레임 키로 재현, 복원 편차 0.0(허용 1e-3). 발견: 크랭크 180° 풀스트로크가 작업대 상판을 0.025m 관통(AABB) — 런타임 리깅 전 ~150° 제한 또는 판독기 높이 상향 필요. 파생 씬 `concept-first-m7-articulation.blend` 별도 저장, 원본 blend sha 전후 동일(`8c76350a…`).
  - GLB 후보 4종(`SM_Env_Watchroom/SM_Prop_OpticalReader/SM_Prop_RecordSet/SM_Env_GateThree.glb`): 재임포트 라운드트립 224/224 일치. glTF가 box projection을 표현하지 못해 export 시 Smart-UV로 대체 — 시각 동등성 미보장, Bump 노드 미이관.
- [BOUNDARY] 전 산출물 `runtimeEligible:false` 유지. 미검증: 인엔진 이음새/반복·밉·텍셀 밀도, 무조명 base colour 리뷰 승인, Unity 임포트, 메시 수준 충돌, 크랭크-판 기어 연동, 라이트맵 UV/LOD. 승격은 decision-log 감사로만.
- Sources: `assets/generated/2d/texture/m7-*-r01/provenance.json`(5) · `assets/generated/3d/concept-first-m7/provenance.json`(16 자산 + acceptanceProgress) · `glb-export-report.json` · `articulation/articulation-report.json` · `m7-tiling-qa-report.json`. 소유: 재질/연출 방향 game-presentation-director, 블록아웃 game-modeler 후속 인수 대상.

### RFC-CX-011 delivery evidence — 2026-09-11

- [OBSERVED] 구현 2레인 병렬(SYS-CORE/SYS-M8) + 디렉터 리소스 레인 완료. SYS-M8 서브에이전트는 보고 단계에서 세션 경계로 종료됐고 코드는 온전했다 — 디렉터가 diff 검수 후 `completeness-m9/impl-sysm8.md`로 보고를 재구성했다. 세션 중 irc 판정 3건: T0PlayModeTests/T0CaseThreadTests·T0RuntimeConfig/T0ProjectBuilder 경계 확장(승인), S-D 비공개 충돌 → 판정 B(objective에 record 표시명 포함 시 폴백하는 데이터 구동 가드; AssertNoDisclosure 유지).
- [OBSERVED] 배선은 `T0ProjectBuilder.WireBeats`(신규, beats 테이블만)와 `M8ReviewNotesProjectBuilder.ImportReviewCard`(sha 65fc439b… 대조)로 수행 — 전체 Prepare는 후속 마일스톤 빌더 소유 씬을 재생성하므로 실행하지 않았다.
- [OBSERVED] 최종 네이티브: EditMode 53/53(+7: M9CoreTests 5·ReviewNotesTests 매체 분기), PlayMode 68/68 + 직렬화 부트 1/1(격리 인자), macOS 개발 빌드 Succeeded 354233020B, `open` 실행 생존 12초. 이전 M5 reduced-motion 실패는 선행 수정 위에서 통과. 영수증: `systems/tech-verification/completeness-m9/verification.{md,json}`.
- [OBSERVED] 창 캡처는 데스크톱 전체만 얻어 판독 증거로 무효(병행 세션 창 포함) → 즉시 폐기. 따라서 GTI 카드 텍스처는 `runtimeApproved:false`·`runtimeEligible:false` 후보로 유지하며, 승격은 네이티브 창 가독성 검수 후 별도 감사 블록으로만 기록한다. 크레딧 사용량은 백엔드가 노출하지 않아 null(0으로 적지 않음).
- [DECISION] 이월 확정: hintOfferCooldownSeconds 분리(G4d, tools.md §4 + emit-tables 재생성), EN 힌트 textKey(G10), alignment 신규 동사(기획 재-인테이크), ReadOriginal 마모 undo 복원 여부(개방 RFC — 법2 압력 판정 필요), t0-b1 objective 무스포일러 재작성(planner). 독립 QA: `qa/completeness-m9-review.md`.
- [CARRIED] 사람 플레이·IME/컨트롤러 실기·25분 예산 실측·G4 몰입은 미측정. 커밋/푸시는 사용자 수행. 병행 세션(M7 텍스처 QA·M10 TRACE-RPG)이 같은 트리를 편집 중 — 본 RFC는 그 레인 파일을 쓰지 않았다.

### RFC-CX-011 QA FIX cycle 1 — 2026-09-11

- [OBSERVED] 독립 QA(`qa/completeness-m9-review.md`): S1 0 · **S2 1** · S3 10. S2 = D-M9-01 (C1 단계에서 `원문 열기`가 원문을 열지 못하고 `document`를 잔류시켜 다음 Esc가 검토 노트를 재개). FIX 1회차로 디렉터가 직접 수리(서브에이전트 세션 경계 유실 3회 후 판단).
- [DECISION·수리] D-M9-01: `ReviewSourceOriginalAvailable`(=!PatrolActive&&!SignatureActive) 게이트로 C1에서 액션 미생성 + 메서드 조기 반환; 회귀 테스트 `C1StagesNeverOfferSourceOriginalOpen` 추가. D-M9-02: `Back()`이 reviewNotes에서 `CloseReviewNotes()`로 위임 — Esc와 버튼이 같은 140ms 닫힘·직전 오버레이 복원. D-M9-04: `T0Simulation.IsSatisfied` 공개, App 복제 술어 삭제(단일 출처). D-M9-05: `teachingRemaining` "비트"→"단계"(en beat→step). D-M9-08: `T0CaseThreadTests.ExpectedObjective`를 비트별 리터럴로(동어반복 제거). D-M9-10: `SaveHintLevels`/`FlushHintLevelsIfDirty` — pending 중 힌트 단계 상승은 commit 성공/실패/취소 시 1회 QueueSave.
- [DECISION] D-M9-03: `select_ms`(100ms 선택 윤곽)는 소비처가 없고 전체 재구성 UI에서 토글마다 리스트 페이드는 지시된 "윤곽 변화"가 아니라 깜빡임이 된다 → 키 제거·영수증 문구 180/140/120으로 정정, 100ms 윤곽은 TARGET 이월.
- [DECISION] D-M9-06: guided 티칭 헤더는 **힌트 사다리와 별개의 안내 기능**으로 판정한다(선택 a). 1단 문장을 재사용하되 `hintLevelUsed`에 기록하지 않는다 — 플레이어가 요청한 힌트가 아니므로 텔레메트리 의미(H-R7 "플레이어가 연 단계")를 보존한다. hint-system.md 등재는 systems 후속.
- [DECISION] D-M9-07: 프리뷰 문장은 GDD §3.3 4요소 중 "바뀌는 것·되돌림" 2요소를 구현했고 "영향 구역·근거 2종"은 이월 항목에 추가한다(T0는 구역=hub 고정, 근거=independentPair 충족 여부로 보강 가능).
- [CARRIED] D-M9-09: 실행 영수증은 재실행 트랜스크립트로 교체(아래 FIX 검증). D-M9-11: t0-b2 objective 두 번째 문장("안내 표시가 각 단계에 붙는다")은 저작 지시문이 화면에 노출되는 콘텐츠 결함 — planner 이월(t0-b1 무스포일러 재작성과 함께).

### RFC-CX-011 QA FIX cycle 2 (마지막 허용 회차) — 2026-09-11

- [OBSERVED] QA R2: 수리 6건 전부 CLOSED(독립 확인), 판정 3건 수용, D-M9-09 CLOSED, D-M9-11 STILL-OPEN(planner). 신규: D-M9-12(S3, 이월·판정이 decision-log에만 있고 manifest/changelog 미반영), **D-M9-13(S2, 도구 패널 열린 상태에서 `원문 열기` → 복귀 플래그 즉시 해제 + 문서 아래 숨은 도구로 Adjust/Query/Disconnect 입력 전달 — R1 누락 경로, 수리로 생긴 것 아님)**.
- [DECISION·수리] D-M9-13: 최소 수리(QA 권고 후자) — `ReviewSourceOriginalAvailable`에 `tool==null` 추가. 도구 중 노트에서는 `원문 열기`를 내지 않고, 셸에서는 유지. 회귀 `OpenToolPanelsNeverOfferSourceOriginalOpenWhileShellStillDoes`(reader 열림→노트→액션 0건→닫기→reader 복귀→셸→액션 존재). 도구를 기억·닫고 문서를 여는 확장안은 이월(manifest). D-M9-12: manifest 이월 목록·changelog에 FIX 판정 반영. G-2: C1 회귀에 `evidence` 복원 단정 추가.
- [DECISION] FIX 회차는 2회로 종료(quality-gates FIX≤2). R3에서 S2가 다시 열리면 REDO가 아니라 해당 항목을 개방 결함으로 등록하고 hop 완료 판정을 보류한다.

## RFC-CX-012 종합 판정 — ACK 3건 해소·적용 완료·승격 (2026-09-11, decided_by: game-production-director)

- [OBSERVED] 사용자 지시(2026-09-11): "subagent 이용해서 완료시키고 메인에 머지". 적용 단계 3 wave + QA 2회로 이행. 세션 소멸로 2회 재-스폰(PlannerApply→PlannerFix, SynopsisApply→BridgeAlign); 산출물 무손실.
- [DECISION] **ACK-a(planner, `planning/field-classification-20260911.md`) 수용**: `completion` = 검증 술어 — 런타임 소비처 0건(`T0Json.BeatJson`에 필드 부재, 로더는 `completionPredicate.requires`만 역직렬화, `unity/**/*.cs` grep 0), `objective`·`subtasks`·`hints`·`recovery`·`title`·`clues.description` = 표시(CaseObjective verbatim 표기 + `M9CoreTests` 단언 + emit-tables 투영). 파급: B-11 위반 아님(토큰 보존) · B-12 표시 필드 4건만 교체 · B-14 objective 안내문 9곳 제거(`toolTeaching` 1:1 보유 확인).
- [DECISION] **ACK-b(worldview) 수용**: glossary R8(신설 9행 + 회로 지도 2상태 + §7 「-철」 규칙) + R9(신설 3행 보호 지정·공통 종결부·종결부 후일담 + §7 어간 파생 면제 규칙[역대조·확대 판독] + §4 공통 조위 피크 축약 허용). HEAD↔live: 추가 12·삭제 0·개명 0 [OBSERVED]. 오버레이 6팩트 seed/reveal 캐논 대조 counter 0. `c6-b3.consequence` R3 문장 vs §7 상한 긴장은 §7 우선(RFC-W4 선례)으로 판정, 수정 형태(이관 vs 한정)는 차기 consistency-audit 안건 — planner 편집 회차 판정.
- [DECISION] **ACK-c(systems, `systems/rfc-cx-012-ack.md`) 수용**: `game-ui-contract.json` information_hierarchy 「복귀 지점 목록과 각 지점의 장(章) 조위 위상」 1곳. `data/t0/*` emit-tables 재생성 2회(2차는 QA 조사 정정 반영 + emitter L896 `_src` 「증거함」 1어 정정 승인). diff는 문자열·sha·날짜·`_src` 주석만, 수치·id 변화 0. **Unity `Assets/_Project/Data/Tables/` 사본은 교체 전 campaign sha를 물고 있어 다음 `Tools/T0/Import`가 fail-closed** — 이 RFC는 unity/ 무접촉이며 재복사·재임포트는 M9 세션/다음 회차 systems 몫 [CARRIED].
- [DECISION] **디렉터 가감(aside 자문 대비)**: ① 훈련→연습 흡수(glossary §3 「연습」 정의가 aside 확인 질문에 답함 → 「연습 서식」·「연습 압착」), ② aside "보호 지정 5장" 기각(5장=章 → "5장의 보호 지정"), ③ aside 미회부 기계적 통일 12건은 감사 후보 ① 채택. `production/term-decision-aside-20260911.md` §1·§2가 교체어 유일 정본.
- [OBSERVED] **적용 실측**: campaign.json 교체 65연산/62필드 + QA 조사 정정 2건(D-CX012-01/02). t0-records.md 2행(hb-l4 청문 접수부, hb-l1 봉인대→서명대 — §2 자리/기구 구분 근거). narrative-flow-bridges.md 8행/10셀 정합. 유지 13행(27명사) 개명·재정의 0.
- [OBSERVED] **QA 2차 (`qa/rfc-cx-012-review.md`)**: validate-campaign 49/49 · `--pairs` 17쌍 · `--t0` 5/5(sourceSha 일치) · evidence-graph 2회 emit sha 동일 · EG 18/18(음성 시험 비공허 확인) · K-06 {c4-b3:false, c6-b4:true} · validate-game-ui PASS. 결함: S1 0 · S2 0(3건 closed) · S3 closed 5 / open 0(D-07 term-decision §4 R9 반영·D-08 bridges §3.3 R9 정합 — 디렉터 직접 정정). 승격 14/14 PASS.
- [DECISION] **승격**: QA PASS 근거로 draft 10건 → `status: current` (planning 4 meta + field-classification, worldview term-audit, production term-decision, synopsis bridges, systems ack, qa review). glossary·t0-records·timeline·ui-contract meta·data/t0 meta는 기존 current의 RFC-Q2 제자리 갱신. 디렉터 재검증 [OBSERVED]: 49/49 · `--t0` 5/5 · EG 18/18 · `freshness-check.sh` 0 finding / 600 artifacts.
- [DECISION] **게이트 영향**: G1(세계관 일관성) 문서 수준 — 용어집 미수록 노출 고유명사 16→0, 톤 미정합 기술어 15→0(표시 필드 기준), 캐논 변경 0. G7 문서 수준 — 증거 해금 위상·도달성·공개 순서가 기계 검사(EG 18)로 고정. **런타임 게이트는 여전히 NOT-MEASURED**(빌드 0·플레이 n=0). campaign.json이 바뀌는 모든 미래 회차의 완료 조건 = 49 + `--t0` 5 + EG 18 동시 PASS.
- [CARRIED] 이월(비차단): 저작 주석 필드(action/consequence/inference) 교체 전 어휘 22건 잔존(감사 범위 밖, 화면 비노출) · `c1-b3` title/completion/clue의 「훈련」 3건(제목은 타 레인 인용 위험으로 단독 개명 보류) · timeline §7 B13 "공통 피크" 이형 · `c6-b3.consequence` R3 정렬 · Unity Data/Tables 재복사. 전부 다음 worldview/planner/systems 편집 회차 안건.
- 머지: main 직접 커밋(사용자 지시). 스테이징은 RFC-CX-012 범위 파일만 명시 pathspec — M5/M9 병행 세션 산출물(unity/·assets/·m5-direction/·concept/·handoff/·changelog·.mex/·qa/completeness-m9-review.md·tech-verification/completeness-m9/·graphify-out/)은 스테이징하지 않는다. decision-log.md·task-manifest.md는 공유 append-only 파일이라 병행 세션의 M9 append가 함께 실린다(되돌리지 않음, §8).

### RFC-CX-011 종료 판정 — 2026-09-11

- [OBSERVED] QA R3: D-M9-13(S2) CLOSED(코드 게이트·회귀 테스트·영수증 3중 일치), D-M9-12 CLOSED, G-2 CLOSED. **S2 재개방 없음.** 잔여 D-M9-11(S3, planner·campaign.json 병행 편집 중 — M9 코드 범위 밖), D-M9-14(S4, impl 보고 문구 → 즉시 정정). R3 집계 `S1 0 · S2 0 · S3 1`.
- [DECISION] **M9 hop 완료 판정: 범위 내 PASS(문서·자동화 검증 수준).** 근거: RFC-CX-011 S-A~S-H 전부 구현·배선, 독립 QA 3회(R1→FIX1→R2→FIX2→R3), 최종 EditMode 53/53 · PlayMode 70/70 · 직렬화 부트 1/1 · macOS 빌드 Succeeded 354230868B · 트랜스크립트 실행 영수증. FIX 회차 2/2 사용.
- [BOUNDARY] 이 판정은 G4(연출/몰입)·G7(코어루프 수용)의 **런타임 PASS가 아니다** — 사람 플레이 n=0, 창 수준 가독성 캡처 미완, 실기 IME/컨트롤러 미검. GTI 카드 `m8-review-card-r01`은 `runtimeApproved:false` 후보 유지(승격은 네이티브 창 검수 후 별도 감사). 커밋/푸시는 사용자 수행.
- [CARRIED → 다음 인테이크] manifest M9 절 이월 목록(hintOfferCooldownSeconds 분리 · EN textKey · alignment 동사 · ReadOriginal 마모 undo 판정 · t0-b1/t0-b2 objective 재작성(planner) · 프리뷰 영향구역/근거2종 · 100ms 선택 윤곽 · 티칭 헤더 hint-system 등재 · 도구 열림 중 원문 열기 확장안 · D-M9-10 전용 테스트(QA G-1)).

## RFC-CX-013 — M7 컨셉 리소스의 T0 스테이지1 런타임 반영: UI/UX·연출·GUI (2026-09-11)

- [OBSERVED] 사용자 지시(2026-09-11): "이어서 디벨롭까지 진행해보자. 코어루프 스테이지1, 리소스가 전혀 업데이트되지 않았네 ui/ux, 연출, gui, 모두야." RFC-CX-009 delivery(GTI 재질 5·블록아웃 4·FBX/GLB 후보)가 `assets/generated/`에만 있고 런타임에는 0건 반영된 상태를 지적.
- [OBSERVED] 읽기 전용 정찰 2레인(씬/아트·UI/연출) 결과: 허브 = URP-Lit 단색 큐브 3(`Authored-*`)+`Workbench-*` 5+승인 r03 서랍, 창·판독기 3D·소금판·크랭크 **없음**(`Editor/T0ProjectBuilder.cs:43-54`); 판독기는 uGUI 파형 화면(`App/T0GameSession.cs:267-278`); UI는 매 Render마다 코드 생성, 배경 전부 `Image.color` 리터럴, 스킨/테마 객체 **없음**(`UI/T0Interface.cs:52,72,76,93,128,212-218`); 판독기 초점/확대경/조명 연출 **없음**; 애니메이션 **없음**. 게이트 정본 패턴 = `profile.runtimeApproved || args.Contains("--<x>-diagnostic")`(`App/T0OpeningSession.cs:16`, `App/ReviewNotesSession.cs:226`, `App/C1GameSession.cs:114`), 빌더는 `Resources/*.asset` 생성 시 `runtimeApproved=false` 강제(`Editor/M8ReviewNotesProjectBuilder.cs:31-34`). 감축 모션 계약 = `settings["reducedMotion"]` + `*Vfx.json` `reduced_motion_ms:0` + `Time.unscaledDeltaTime`(`App/ReviewNotesSession.cs:205-215`).
- [DECISION] 범위 = **기존 게이트 패턴을 그대로 확장**하는 3레인 병렬, 전부 `runtimeApproved:false` 후보 + 진단 플래그. 새 밸런스·세이브 스키마·시뮬 변경 0건.
  - **HubShell**(modeling↔systems): `Editor/M7HubProjectBuilder.cs`(`T0ResourceDiagnostics.ImportAndCapture` 규칙 재사용: BaseColor sRGB on/Roughness off, maxSize 1024, `Tide/Candidate Roughness`) → `Art/Candidates/m7-hub-r01/`(perforated-steel→Workbench 5·서랍 케이싱 제외, salt-concrete→Authored floor/back wall, bronze→plate shelf), `Presentation/M7HubProfile.cs`(`runtimeApproved`, `Material[]` 매핑) → `Resources/M7Hub.asset`. 런타임 `App/M7HubSession.cs:BindM7Hub()`가 게이트 통과 시에만 허브 루트 renderer의 sharedMaterial 교체; false면 커밋된 씬 무변경. 조명: builder에서 `RenderSettings.ambientMode=Flat` 명시 + 황토 작업등 point light 1 추가는 **게이트 안**에서만.
  - **ReaderStage**(presentation↔systems): `SM_Prop_OpticalReader.fbx`·`SM_Prop_RecordSet.fbx`(`assets/generated/3d/concept-first-m7/fbx/`) → `Art/Candidates/m7-reader-r01/` (`C1SignatureProjectBuilder.ImportAndCapture` 패턴, URP Lit + MetallicSmoothness), `Presentation/M7ReaderStageProfile.cs`(`runtimeApproved`, reader/recordSet prefab, cameraPosition/lookAt/horizontalFov, background, 램프 색·강도, 크랭크 stroke deg ≤150·pivot 이름) → `Resources/M7ReaderStage.asset`. 런타임 `App/M7ReaderSession.cs`: `M7ReaderStageEnabled`(tool=="reader" && !PatrolActive && 게이트) → `ApplyStagePresentation` "reader" 스테이지(디렉터가 `App/C1GameSession.cs:95,110` 배선 완료)에서 카메라 고정 포즈·SolidColor·황토 point + 한랭 fill 2등·프리팹 인스턴스. 크랭크: `Resources/T0ReaderVfx.json`(forward/hold/return ms, `reduced_motion_ms:0`) 수치로 stroke ≤150°(RFC-CX-009 evidence: 180°는 상판 관통), `Time.unscaledDeltaTime`, reducedMotion이면 정지 포즈. 카메라는 고정(팬/줌/셰이크 0 — `presentation/cinematic-gameplay-m6.md:11`).
  - **UiSkin**(presentation↔systems): `Editor/M7UiSkinProjectBuilder.cs` → `Art/Candidates/m7-ui-r01/`(rag-paper basecolor→Sprite(2D and UI) sRGB mip off, bronze basecolor 동일), `Presentation/M7UiSkinProfile.cs`(`runtimeApproved`, paperPanel/frameTexture, ink/paper/brass/header/nav/toolbar 색 = `concept/style-guide.md:29-36` hex) → `Resources/M7UiSkin.asset`. 런타임 `App/M7UiSession.cs:ApplyM7UiSkin(GameScreen)`이 게이트 통과 시 `GameScreen`에 skin 필드를 채우고 `UI/T0Interface.cs`는 skin이 null이면 **기존 리터럴 그대로**, 있으면 Work Surface/헤더/툴바 뒤에 `RawImage` 1장(기존 `T0OpeningInterface.cs:10` 패턴) + 색 치환. 텍스트 대비·150% textScale 클리핑 0 유지.
- [DECISION] 파일 소유(병렬 세션 안전, RFC-CX-011 §소유 경계 준용): 디렉터가 스텁 3개(`App/M7HubSession.cs`·`App/M7UiSession.cs`·`App/M7ReaderSession.cs`)와 훅 3줄(`T0GameSession.cs:61,178`, `C1GameSession.cs:95,110`)을 선배선. 각 레인은 **자기 스텁·자기 Profile·자기 Builder·자기 Candidates 폴더·자기 테스트 파일**만 쓴다. UiSkin만 `UI/T0Interface.cs`를 추가로 소유. `T0GameSession.cs`·`C1GameSession.cs`·`Data/Tables/`·타 레인 파일은 어느 레인도 편집하지 않는다. Unity 실행은 디스크 2.6G 여유(Library 2.2G) 때문에 **레인은 실행하지 않고** 디렉터가 종료 시 1회 batchmode(빌더 3종 executeMethod → EditMode/PlayMode)로 검증한다.
- [DECISION] 테스트: 레인별 PlayMode 1파일(`Tests/PlayMode/M7{Hub,Reader,UiSkin}PlayModeTests.cs`, `Create()`+fixture 하네스 `C1SignaturePlayModeTests.cs:22-28`) — (a) `runtimeApproved=false`면 커밋 상태 불변(허브 renderer sharedMaterial 동일 / reader 스테이지 미진입·hub 루트 활성 / skin RawImage 0개·리터럴 색 유지), (b) 진단 플래그 경로에서 적용됨, (c) reducedMotion 시 크랭크 즉시 정지 포즈. 기존 `T0ResourceInteractionTests`(r03 서랍 2메시·sharedMaterials 불변)와 `T0PlayModeTests` 카메라 hFOV 검사는 **깨지면 안 된다**.
- [BOUNDARY] 전 후보 `runtimeEligible:false`·`runtimeApproved:false`. 승격은 네이티브 검수 + 라이선스(UNVERIFIED) + G5 예산 실측 후 별도 decision-log 감사(asset-runbook §3.1 8항). 제외: 손 리그, 제3수문 런타임, 확대경 DOF/볼륨(URP Volume 0건 유지), 사람 플레이. FBX는 임포트 시점 후보 포맷(기존 `Art/Candidates/*.fbx` 관행)이며 GLB 핸드오프 정본은 유지(`fbx/fbx-export-report.json`).
- decided_by: game-production-director(사용자 대행) · 영향 레인: modeling, presentation, systems, qa(레인 테스트 → 디렉터 1회 실행 영수증)

## RFC-CX-013 — RFC-CX-012 이월 해소: 저작 주석 정합·훈련→연습·R3 회수 위치 K-07·Unity 테이블 재동기 (2026-09-11, decided_by: game-production-director)

- [OBSERVED] 사용자 지시(2026-09-11): "subagent 이용해서 다음 작업 이어서" → "작업완료하고 main 브랜치에 머지해 최신화하고 워크트리정리해". 대상 = RFC-CX-012 [CARRIED] 5건. 3 wave + QA 1회 + 결함 정정 1회. main은 `1bd0097`(M9 세션, push 완료) 위; 워크트리는 main 1개뿐(부속 브랜치·worktree 0 — 정리 대상 없음 [OBSERVED `git worktree list`]).
- [DECISION] **ACK-a(planner, `planning/field-classification-20260911.md` §RFC-CX-013)** 수용: campaign.json 변경 28리프(추가/삭제/id 0) — 저작 주석 4필드(action·consequence·inference·authorEstimateBasis) 잔존 21필드/23어휘 교체(전문 잔존 0) · `c1-b3` title/completion/clue 「훈련」→「연습」 3건 · **R3 이관**: `c6-b3` consequence/inference를 순서 앵커(밸브 20분 선행·집행이 이중서명 완성 전)까지로 한정하고 R3 동기(1호기 고장 은폐·창고 보호=사후 설명)+R2 의도 재해석(거부 수단)을 `c6-b4` consequence/inference로 이관 — timeline §7 B26/B27 문면 일치. **K-07 신설**(validate-campaign 49→50): R3 3구가 `c6-b3`(consequence·inference·objective) 부재·`c6-b4` 존재. 음성 시험: `fc737c1` 사본에서 K-07만 FAIL [OBSERVED]. overlay §8 인용 재파생 후 EG 18/18.
- [DECISION] **ACK-b(worldview)** 수용: timeline §7 B13(`c3-b2`) 「공통 피크」→「공통 조위 피크」, §8 「봉인대 훈련」→「봉인대 연습」; consistency-audit **A44 = pass**(c6-b3 R3 긴장, 이관+K-07로 닫힘; 집계 44 = 41 pass·3 open·0 violation); term-audit 이월 4건 closed. glossary 이번 회차 무변경(바이트 동일).
- [DECISION] **ACK-c(systems, `systems/rfc-cx-012-ack.md` §RFC-CX-013 + `tech-verification/rfc-cx-013-tables-resync.md`)** 수용: `data/t0` 3차 재생성(`--t0` 5/5, sourceShaMatchesLiveCampaign **true**) · **Unity `Assets/_Project/Data/Tables/` 재발행(`--scope t0`, 1차 회차 추정 `all`은 오류 — receipt scope 필드로 확정)** → receipt.source.sha256 == live campaign(python 재계산) · 헤드리스(Unity Editor 프로세스 0건 가드) `Tide.EditorTools.T0AssetImporter.Import` exit 0("Producer source differs" 부재, Authoring 6 asset 갱신) · `T0Verification.RunBatch` **T0_M1_CHECKS tests=21 failures=0**, 컴파일 에러 0. RunBatch가 재작성한 `unity/Unknown/results/t0-m1-contract-checks.xml`(testcase 개명 1건 — HEAD 소스 대비 원래 stale)은 **재커밋**(디렉터 판정). fail-closed 임포트 이월 해소.
- [DECISION] **synopsis 핸드오프** 수용: campaign.md·chapter-beats.md 「봉인대 연습」 인용 갱신; QA D-CX013-02(S2) — chapter-beats 표 B `c6-b3` 행이 삭제된 inference 문장을 인용 → live+B26으로 재작성, R3/R2 `c6-b4` 행(B27)으로, `c1-b3` 「당직자」; D-03 표 B 14행+표 A 1행+§3 1행 CX-012 교체어 정합(잔존 2건은 열 이름/제작 산문으로 명시 보존); D-04 bridges L18/L139 스테일 정정.
- [OBSERVED] **QA(`qa/rfc-cx-013-review.md`)**: 30검사 PASS 28·FAIL 1(D-01 frontmatter, 정정)·재-인테이크 이관 1. S1 0 · S2 1→0 · S3 3→0. 승격: tables-resync PASS→current, 갱신 current 델타 PASS 11, chapter-beats HOLD→(D-02 정정 후) 해소. **디렉터 재검증** [OBSERVED]: validate-campaign 50/50 · `--t0` 5/5 + sha match · EG 18/18 · Unity receipt == live · results XML 21/0 · `freshness-check.sh` 0 finding / 602 artifacts.
- [DECISION] **c6-b3 설계 재-인테이크 — open (S 등급 없음)**: QA R3-4가 지목한 `c6-b3.objective`("…이유를 서로 다른 출처로 밝힌다")·`completion`("동기는 …두 기관의 서류로 각각 확정된다")·subtasks[2]/[4]·clues c3/c4는 HEAD와 동일한 설계 원형이며, 부분 이관으로 consequence("확정은 다음 비트로") vs completion 비트 내부 긴장이 생겼다. K-07(구 단위)은 통과하나 문장 층위는 잡지 못한다 [INFERENCE]. 해소는 (A) timeline §7 B26/B27 상한 재정의(worldview) 또는 (B) c6-b3의 동기 관련 subtasks/clues/hints/completion을 c6-b4로 이동(planner; clues 73·독립쌍·EG 재도출 연쇄). **비트 설계 변경이므로 carry 회차에서 결정하지 않는다** — `content-update` 재-인테이크 안건(RFC-W4 선례: 상한 우선이면 (B)가 정합). QA·planner·worldview 3레인 관찰 일치.
- [CARRIED] 비차단 이월: continuity.md §5 K10 행 위치 `c6-b3`(B26)→B27 정정(synopsis 관찰) · glossary §2 「정합기」 정의문 "공통 피크" 축약형(worldview 관찰, 다음 glossary 개정) · consistency-audit §3 기계 검사표 4차/R7 값 미재실행(§1에 명시) · Unity `Data/Authoring/` 6 asset은 헤드리스 임포트 산출물(runtimeEligible 규칙 대상 아님 — 데이터 테이블) · 런타임 게이트 NOT-MEASURED 지속(빌드 0·플레이 n=0; T0_M1_CHECKS 21/0은 에디터 계약 검사).
- 머지: main 직접 커밋 + `origin/main` push(사용자 지시 "최신화"). 스테이징은 RFC-CX-013 범위 파일 명시 pathspec(unity/는 `Data/Tables`·`Data/Authoring`·`results/t0-m1-contract-checks.xml`만). M9 잔여(`Art/Candidates/m8-review-card.meta` 등 untracked)·graphify-out·.mex·assets/ 무스테이징.

## RFC-CX-013 — 네이티브 실동작 플레이테스트·모니터링과 게임플레이 영상 (2026-09-11)

- [OBSERVED] 사용자 지시: 최신화된 내용을 전부 실동작 테스트하고 모니터링해 완성도를 높이며, 코어루프·게임플레이 영상을 만든다. 배치 테스트가 아니라 **빌드된 플레이어를 실제 창에서 조작**했다(`open` 인자 실행, 1280x800, `--m8-review-notes-diagnostic`, 격리 저장 디렉터리). 조작은 cliclick 포인터(합성 키 이벤트는 Unity Input System에 닿지 않아 키보드 경로는 PlayMode 검증에 위임). Player.log 모니터링(예외 0), save.json 실시간 대조.
- [OBSERVED] 실동작 통과: M5 오프닝(2샷+S-D 모토 L() 이관) → 셸 → 인수 각서/이관 목록(✓ 6/6·결정) → 판 #0 → t0-b1 완료·비트 목표 전환 → 회로 지도(안내 헤더·잔여 조건 2→1→0 실시간) → 정렬(−1,+1)·앵커·구획 3·근거 3 → t0-b3 → 판독기(안내 헤더) → 표준판 판독·H−1:00→H+3:00·**프리뷰(diff 문장+되돌림 고지)→확정** → 조위대장 판독·인용 → `T0 완료` → 되돌림/다시(headSeq 31→30→31, 자동 사본 보존) → **OS 재시작** 후 `t0-b1:2` 힌트 유지·t0-b3 힌트 0단 → C1 순찰(관찰 2/2·분기·조건·two-step 확정·수문 권한) → C1 검토 노트(`원문 열기` 없음 = D-M9-01 게이트 실증). 검토 노트: 카드 텍스처 배경 렌더·플레이스홀더 가독, 질문 버튼, 출처 연결, 원문 열기→닫기→초점 복귀 실증. 힌트: 1→2단·경고 게이트·재열람 보존. 스크린샷 94장: `systems/tech-verification/native-playtest-m9/shots/`.
- [OBSERVED] 실동작에서만 드러난 결함 3건 + 기존 1건 실증:
  - **D-M9-15 (S2·UX)**: `검토 질문 보기`를 누르면 질문 패널이 버튼 *위*에 렌더되고 초점 스크롤(40%)이 버튼을 내려 패널이 화면 밖으로 밀림 — 답이 존재하나 보이지 않음. 포인터 휠 스크롤이 리스트에 닿지 않아 회복 불가. 수리: 패널을 앵커 액션(`review-note-question`) *아래*에 렌더(`ReviewNotesView.QuestionAnchorId`, `T0Interface` 액션 루프).
  - **D-M9-16 (S3)**: 저장 재개 시 T0 완료 상태에서도 t0-b1 환영문("각서와 목록을 열람하고…") 표시. 수리: `ResumeStatus()` — b3 완료→`caseReview`, b1 완료→신규 `resumeInProgress`, 그 외 welcome.
  - **D-M9-17 (S3)**: 내부 id가 플레이어 문자열로 노출 — 판독기 자료 선택 `station-bureau-standard`, 순찰 관찰 상세 `출처: watchlog-bureau`, 증거함 `· watchlog-bureau`, 구획 근거 `· log/ledger`. 수리: 4개소를 `ReviewMediaName(sourceType)` 매체명으로. 자동 보존 단서 id(`t0-b2-c1` 등)는 이름 데이터가 없어 이월(planner: clue 표시명).
  - D-M9-11 실증: t0-b2 목표의 저작 지시문 "안내 표시가 각 단계에 붙는다."가 사건 흐름에 그대로 노출됨(planner 이월 유지).
- [OBSERVED] 관찰(결함 아님): 눈금 선택기는 페이지당 30행 중 ~8행만 보이고 휠 스크롤이 닿지 않으나 **드래그 스크롤**로 도달 가능(UGUI ScrollRect 드래그). 관성으로 클릭 직전 레이아웃이 움직여 오클릭이 생김 — 사람 플레이에선 손이 기다리므로 자동화 특유의 현상. 3D 뷰포트는 승인 서랍(r03)·순찰 패널(r01)만 있고 회로 지도/판독기 노드는 그레이박스(M7 프리팹 재구성 이월).
- [DECISION] 위 3건은 RFC-CX-011 파일 집합 안의 소형 수리이므로 별도 FIX 회차가 아니라 본 RFC의 실동작 수리로 기록한다. 재빌드 후 EditMode/PlayMode/부트/빌드 재실행 + 카드 승격 판정은 아래 delivery evidence.
- [DECISION] 영상: 승인 방식은 M5 선례(`screencapture -v -l <windowID>` 창 한정, 64px 타이틀바 크롭 1280x720, 30fps H.264). `docs/media/gameplay-m9/`에 `core-loop`(관측→조작→확인→되돌림 + 프리뷰→확정)과 `gameplay`(T0 전체→C1 진입) 두 편. **네이티브 창 녹화이며 previz가 아님**을 파일명·캡션·provenance에 명시.
