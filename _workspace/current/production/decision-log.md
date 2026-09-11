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
