---
updated: 2026-09-14
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-production-director
---

# Changelog

## v0.1.0-pre (사전제작 C1~C7, 2026-09-09 → 2026-09-10) — 코드 0줄, 실측 n=0

### production / director
- 세션 병합 판정 RFC-P3-008~015, C3 종료 판정(RFC-Q1/Q2, F25/F30/F31/F33/F22, RFC-W4, A37), RFC-P4-001(Mixamo), C6/C7 판정 묶음(RFC-C7-001, RFC-S2~S6, RFC-N6, RFC-B6, RFC-M1/M3, RFC-C6-001/002) — `production/decision-log.md`.
- 계약 C3 개정: Time acceptance(RFC-P3-011), Evidence storage 제자리 갱신 규칙(RFC-Q2), Base production gate(C6-F9), Asset pipeline, Unity/저장소 배치 — `production/premium-preproduction-contract.md`.
- 회차 기록 `production/cycles/c{3..7}-development.md`, 영수증 `production/receipts/unity-batchmode/`, 대장 `production/cycle-ledger.json`(`scripts/regen-cycle-ledger.py` 파생), 충돌 기록 `conflicts.md`.

### planning
- `campaign.json` 계보 B 확정 + `validate-campaign.mjs`(49검사, `--pairs`, `--t0`) 신설; zoneId 33비트, RFC-W4 문장 이동, `c1-b4` 매체 2종, hints[0] 1단 재작성(H-04), `c1-b2`·`c3-b1` proofRequired, t0-b1 목표 문장, completion 술어 — RFC-P3-008/012/013, C3-F11/F22/F29/F35, RFC-N6, C6-F1/F3/F17, RFC-C7-001.
- `gdd.md` C3 후속본(동사 6·2층·힌트·저장·접근성·범위), `content-matrix.md`·`campaign-time-budget.md` live 재도출, `feature-specs/verb-01~06`·`recap-panel.md`, `game-draft-v1.md`(C6).

### worldview
- 세션 P c3 본문으로 재기반(RFC-P3-010), 캐논 시각 H-1:24/H-1:04/H+0:12(RFC-P3-013), T0 공개 상한(RFC-P3-012), B# 단일 정의(C3-F30), 용어집 125행+도구 표시명 4건(C3-F13, RFC-S6), 일관성 감사 pass 37/violation 0.

### synopsis
- `chapter-beats.md`·`continuity.md`·`synopsis.md` live 재도출, K 표 검증기 파생(A37), R2 의도 4장/6장 분리(RFC-W4), S6 대사·S1 판 #0 정의(C3-F7/F21), `t0-records.md`(RFC-C7-001).

### systems
- `architecture-contract.md`(7분할 asmdef, sim/render, 세이브 불변식), `system-specs/` 8종, `data-schemas/` 6종(+zoneId, 부식 저장 없음, dayIndex 제거), `ops/telemetry-contract.md`(`total_minus_afk_min`), `interaction-rules.md`·`unity-implementation.md`·`game-ui-contract.*` R1~R4 반영·정본 표시명·접근성 15키, `pipeline/emit-tables.mjs` + `data/t0/*`(C7-F1), `handoff/` 4문서 + `rfc-inbox/`.

### balance / economy
- 부식 = 전역 상한 9(RFC-P3-009), 원본 상태 카운터 `readCounts`/`readBudget` 3(C3-F35), 힌트 180초 단일 제안(RFC-P3-015), 난이도 지수 관측 2열(C3-F31), 위험 R-T0-1(C6-F5); `currency-map`·`sink-source-ledger`·`reward-bands`·`negotiation-record` 정본 인용 교체(C3-F15/F16/F27).

### presentation / product
- 덱 36장 법 문구·폐기 용어 정정 재생성(C3-F24, C5-F5), `media-direction.md`; `business-model.md` 포지셔닝·가격 후보·생성형 AI 공개 위험(C6-F2/F6/F8), `economics.meta.md`·`assumption-tests`·`skill-application`·`steam-registration-guide` current.

### concept / modeling / animation / motion / vfx
- `style-guide.md`·`generation-manifest.md`·프롬프트 45 + 재생성 2(C4-F11, RFC-M1); `asset-manifest.md` 47종·`pipeline.md`·`specs/hub-watchroom.md`, 그레이박스 GLB 7/FBX 1/렌더 13; `rig-requirements.md`·`anim-list.md`; `motion-contract.md`·`vfx-budget.md` 영수증 게이팅 정합(C4-F7).

### assets / docs / unity / scripts
- `assets/generated/2d`(45, GTI gpt-6-astra) · `3d`(Blender 5.1.2) · `video`(Higgsfield 2클립, 25 credits) · `previz` GIF, 전건 provenance·`runtimeEligible:false`. `docs/media/` 15 파생본 + README. `unity/Unknown/` 6000.5.6f1 스켈레톤(배치 생성·헤드리스 열기 영수증). `scripts/{gen-2d.sh,gen-video-higgsfield.sh,make-previz-gif.sh,refresh-2d-provenance.py,regen-cycle-ledger.py}`.

### 미해결 (등록부 기준, R7 종료)
- S1 0 · S2: C4 1 · C5 1(C5-F2 대장 — 재생성 완료, QA 확인 대기) · C6 2 · C7 1(R7b 처리 중) · S3/S4 다수(표기·영수증 재현성). 런타임 게이트 전부 NOT-MEASURED.


## 2026-09-11 · M6 cinematic video delivery

- Four real Higgsfield MCP clips plus two GTI keyframes; original requests, hashes and60credit observed delta preserved (RFC-CX-008).
- 36second intro/progression film and54second gameplay-method film, Korean captions, stage-rule cards, spoiler masks, authored quiet ambience and separately labeled M5 native inserts.
- Exactframe boundary correction and readable primary captions; full media decode and gallery link checks pass. No M6 Unity changes or new playable chapter claims.


## 2026-09-11 · M7 original-concept art target

- Latest user correction now governs CLAUDE.md/README: originalworldview and earlyconcepts drive video; currentgameplay/prefabs/runtime/M5M6visuals excluded (RFC-CX-009).
- One newGTI opticalworkbench,5realMCPtakes/4selected,24s/32s concept-only films and gallery. R01handstyle rejected and replacedbyreviewedr02.
- Strictsourcehash checks, framecounts and full decode passed. GTItexture/rebuild handoff records subsequent resource/prefab work separately.

## 2026-09-11 · M8 source-informed review notes

- Optional offline hypothesis notes link only observed sources and persist separately from gameplay; authored next-check questions retain canonical evidence/unlock authority (RFC-CX-010).
- Protect drafts, unknown/future saves, source visibility and editing shortcuts; native clipboard/Tab regression fixed without transforming stored notes.
- EditMode46/46, M8 PlayMode10/10, broad64/65, boot1/1, macOS build and actual note-save smoke. The one broad failure is baseline M5 reduced-motion behavior, independently reproduced without M8. Exact source/attempt history: systems/tech-verification/ai-native-m8/verification.md.
- No new AI service or assets. Original-concept/M7/GTI resource direction remains in force; physical IME/controller and human outcome checks remain separate.

## 2026-09-11 · M9 completeness hop (RFC-CX-011)

- Three parallel READ-ONLY lane audits (core loop / tutorial+balance / resources+presentation) drove a bounded scope: every slice implements an existing canonical contract, zero new balance numbers.
- Core loop + balance: per-beat hint levels persist under the unchanged `hintLevelUsed` field and survive reopen/reload; reveal warning is `warnsBeforeReveal`-driven; default confirmation is `two-step` (interaction-rules §1-1); T0 preview overlay renders `Simulation.Preview` change sentences + undo notice; `snapshotInterval` knob is live; idle hint offer pauses while a document is open.
- Tutorial: case-thread objective follows `beats.json` per beat behind a data-driven disclosure guard (director ruling B keeps AssertNoDisclosure); circuit/reader panels show `toolTeaching`-driven level-1 hint + remaining-predicate count; opening motto/buttons moved to `T0Strings` (en added).
- Review notes (M8 targets closed): explicit `검토 질문 보기` (recomputed only on press; stale label on structure change), record-source `원문 열기` with return + focus restore, independent-media question branch. Presentation layer: `M8ReviewVfx.json`-authored 180/140/120 ms CanvasGroup transitions, reduced-motion 0 ms, previous-overlay restore, `M8ReviewNotesProfile` runtimeApproved gate with safe null fallback.
- Resources: GTI rag-paper review-card `m8-review-card-r01` (1536x1024, allowlist reference only, no baked text) imported via hash-checked `Tools/M8/Import review card` as `runtimeApproved:false`; `T0ProjectBuilder.WireBeats` wires beats without a scene-rebuilding Prepare.
- Native: EditMode 53/53, PlayMode 68/68, serialized boot 1/1, macOS build Succeeded 354233020 B, player launch alive; prior M5 reduced-motion failure now passes on the preserved fix. Window-level readability capture failed (desktop-only capture discarded) — card promotion, physical IME/controller and human playtests remain open. Independent QA: `qa/completeness-m9-review.md`.
- QA FIX cycles: R1 found S2 D-M9-01 (C1 `원문 열기` left `document` set; Esc reopened notes) → stage gate + regression; R2 closed all six fixes and found adjacent S2 D-M9-13 (same path with a tool panel open leaks input to the hidden tool) → `tool==null` gate + regression. Rulings: `select_ms` dropped (180/140/120 ms remain), teaching header ≠ hint use, zone/evidence-pair preview sentences deferred. Post-fix EditMode 53/53, PlayMode 69/69→70/70, boot 1/1, build Succeeded; transcript-backed launch receipt replaced a stale-pid smoke.

## 2026-09-11 · M12 native playtest, monitoring and gameplay video (RFC-CX-015)

- Not a batch run: the shipped macOS player was driven in an actual window (1280x800, `--m8-review-notes-diagnostic`, isolated save dir) through M5 opening → handover brief → t0-b1 → circuit map (remaining predicates 2→1→0 live) → alignment/anchor/zones/grounds → t0-b3 → reader standard-plate read → **preview (diff sentences + undo notice) → confirm** → tide-ledger citation → `T0 완료` → undo/redo → **OS restart resume** → C1 patrol (two-step confirm, sluice authority) → C1 review notes. Monitoring: Player.log 0 exceptions, live `save.json` diffing (`hintLevelUsed={"t0-b1":2}` preserved, headSeq 31→30→31, save sha unchanged across OS restart). Screenshots: shots 184 + rec-shots 63+ under `systems/tech-verification/native-playtest-m9/`.
- Three defects that only real operation could surface, fixed in place (RFC-CX-011 file set, no separate FIX cycle): **D-M9-15** (S2) `검토 질문 보기` rendered the answer panel *above* its button and the 40% focus scroll pushed it off-screen — answer present but invisible, unrecoverable because pointer wheel never reached the list → panel now renders *below* the anchor action (`ReviewNotesView.QuestionAnchorId`); **D-M9-16** (S3) resume showed the t0-b1 welcome line even at T0-complete → `ResumeStatus()` three-branch (b3 done → `caseReview`, b1 done → new `resumeInProgress`, else welcome); **D-M9-17** (S3) internal ids leaked into four player-facing strings (`station-bureau-standard`, `출처: watchlog-bureau`, `· watchlog-bureau`, `· log/ledger`) → `ReviewMediaName(sourceType)`. D-M9-11 was demonstrated live (t0-b2 objective still exposes its authoring directive; planner carry retained).
- Post-fix rebuild re-verification: EditMode 53/53, PlayMode 73/73 passed (6 skipped of 79 total), serialized boot 1/1, `T0_MAC_BUILD Succeeded bytes=354247240`.
- Core-loop and gameplay films (`core-loop`: observe→operate→confirm→undo plus preview→confirm; `gameplay`: full T0 → C1 entry) target `docs/media/gameplay-m9/` using the M5 precedent (`screencapture -v -l <windowID>`, 64px titlebar crop to 1280x720, 30fps H.264). These are **native window recordings, not previz**, and will say so in filename, caption and provenance. Status: in progress — output directory not yet created.
- Input boundary: this pass used cliclick pointer input only; synthetic key events never reached the Unity Input System, so keyboard and controller equivalence stays delegated to PlayMode tests and physical IME/controller review remains open. Carried non-blocking: preserved-clue id display names (no name data), t0-b2 authoring directive, tick-selector wheel scroll (drag-reachable; inertia causes automation-only misclicks), M7 prefab reconstruction (circuit-map and reader nodes still greybox).
- Not measured: human playtests, immersion, fun and 8-hour completion remain n=0. Nothing here claims a G4/G7 runtime PASS.

## M13 — RFC-CX-016 (2026-09-11)

- Promoted four gated visual profiles to runtime on the root director's explicit instruction: `M7Hub` (watchroom shell), `M7UiSkin` (ragged-paper and bronze UI), `M7ReaderStage` (optical reader), `M8ReviewNotes` (GTI review card). Each was wired behind an approval gate by an earlier session and shipped `runtimeApproved:false`; the gate-off fallbacks remain intact.
- The shipped macOS player now renders the concept lineage in a real window: M5 opening (sea horizon, moonlit watchroom, desk), M7 hub shell (wet concrete, desk, lit drawer), paper-textured document panels, and the verdigris optical reader stage. Evidence: 14 screenshots in `systems/tech-verification/ui-promotion-m13/smoke/`, Player.log exceptions 0.
- Closed a standing measurement gap: five M7 PlayMode tests that had been `Assert.Ignore`-skipped for missing asset imports now execute and pass. PlayMode moved from 79 total (73 passed / 6 skipped) to 83 total (82 passed / 1 skipped). The one remaining skip is the serialized boot test, which is designed to run only under an isolated `--t0-save-dir`; run separately it passes 1/1.
- Three carried items resolved: automatic-preservation clue ids no longer leak into the evidence panel (derived record labels, ownership uniqueness verified 6 clues / 5 records); D-M9-11 was closed as an **erroneous carry** — the authoring directive had already been removed by RFC-CX-012 B-14 and re-synced in `6dccbc6`, and the prior session re-listed it without re-checking; the "wheel scroll does not reach the list" claim from M12 is downgraded from a product defect to an instrumentation limit, since no blocking point exists in code and three new regression tests now cover the wheel path through the input module.
- Removed four superseded legacy assets (34 files, 20,845,750 B): `c1-signature-reader-r01`, `drawer-r01.fbx`, `stamp-confirm.wav`, and `hub-greybox.fbx`. The last required scene surgery — it was a live PrefabInstance in `hub.unity` under an inactive review node — so 95 YAML lines were removed and anchor integrity was re-parsed (57 objects/12 roots → 53/11, dangling 0 both sides). Source lineage under `assets/generated/` was not touched (558 → 558 files).
- Resolved a three-way RFC id collision left by parallel sessions: the M7-runtime block was renumbered `RFC-CX-013` → `RFC-CX-014` (it had no manifest citation, unlike the carry-over block). No real git merge conflict existed — single worktree, zero conflict markers.
- Build grew to 403,838,357 B (+49,591,117 B). The purge lane had predicted a shrink; that prediction is **confounded, not refuted** — promotion pulled roughly 73 MB of source art into the build path while the removed greybox was 50 KB. Isolating the purge's own contribution needs a control build with the purge alone reverted, which was not run.
- Not measured: human playtests remain n=0. Fun, immersion, performance, the 25-minute budget, 8-hour completion, and real-device mouse wheel / IME / controller are unverified. Pointer input this pass was `cliclick` only; synthetic key events still never reach the Unity Input System. No G4/G7 runtime PASS is claimed.

## 2026-09-13 Aside immersion source update (uncommitted)

- 서사: 일반 조작 안내였던 M5 오프닝을 마지막 당직과 목록/서랍 대조로 변경, serialized asset/C# 기본값 동기화. 초반 목표 fallback은 일반 문구로 방어 강화(현재 정상 objective 경로는 변화 없음).
- 리서치/밸런스: 개발자 원문을 비교하고 2매체 검증·발견 보상은 유지. 근거 없는 난이도/시간 변경과 소비 코드 없는 near-miss 노브는 보류.
- 리소스: GTI 두 후보를 원본·실제 revisedPrompt·해시와 함께 추가. 오프닝 후보는 수정 필요, 정적 패널은 종이색 대비만 원본 픽셀 기준 통과. 모두 런타임/상업 승격 없음.
- 검증: 정적17/17·campaign50/50·독립 source QA. Unity는 license exit198로 테스트 전 차단. 신규4케이스/기존M5 native 실행 증거 없음. G4/G5/G6/G7 승격하지 않음; graphify/zg 동기 상태도 별도 부분 실패로 기록.
- 근거: RFC-CX-ASIDE-20260913 및 handoff/aside-immersion-20260913.md. OMP 소유 힌트·크랭크·Blender 변경은 이 항목의 성과로 집계하지 않는다.

## 2026-09-13 · M22 로컬 개발 빌드 검증 (uncommitted, RFC-CX-017/018)

- Blender MCP로 한서린 전신 및 좌/우 손을 직접 저작하고 Generic 6종 의미 동작/11개 FBX take를 임포트했다. 최종 실측은 전신 15,124 tri/50본, 양손 합 5,544 tri/각 18본이다. 시작 화면 구도, 엄지 방향, 소매 끝 노출, 실제 손잡이 접촉을 교정하고 기본 개발 빌드에 적용했다.
- 힌트의 idle 180초와 재제안 cooldown 180초를 독립 노브로 두고 입력 활동·문서/메뉴/저장 대기 문맥을 반영한다. 무료·선택적 힌트, 원본 읽기 예산, 증거·저장·해금 규칙은 유지했다. M20 r02 기본 적용은 계속 보류한다.
- 크랭크는 성공한 사용자 판독에서만 재생하며 내구 저장 실패는 성공 동작을 내지 않는다. 새 동작이 이전 프레임 시간 때문에 접촉 전에 끝나던 문제를 수락 시점 기준 unscaled 시계로 수정했다. 모션 축소/취소/화면 이탈은 정지·복귀하고 자동 재생하지 않는다.
- EditMode 56/56, PlayMode 97 통과/실패 0/격리 부팅 1 skip, 별도 부팅 1/1 통과. 문구·기본값 고정 테스트 1개와 fallback 문구 고정값을 제거한 뒤 EditMode를 다시 실행했으며 초반 스포일러 차단·목표 선택 3건은 통과했다. 이전 57/57 영수증과 수정 전 PlayMode 4실패 XML도 보존했다. 기본 macOS 빌드와 실제 마우스 판독·삽입·복귀·모션 축소, content focus 후 Tab 이동을 확인했다. 키보드 주입의 초기 무반응 시도와 실제 성공 매크로는 영수증에서 구분하며 물리 패드/IME 동등성을 주장하지 않는다.
- 증거: `systems/tech-verification/m22/verification.json`, `docs/media/gameplay-m22/`. 영상은 실제 개발 빌드의 창 장식만 제거한 30fps 인코딩이며 성능 수치가 아니다. main 작업트리 하나에 통합했으며 삭제할 완료 worktree는 없었다. commit/push, 상업 라이선스, 사람 플레이 n=0, 480분·G4/G7 미측정 경계 유지.

## 2026-09-13 · M22 입력·중단·저작 계약 보완 (uncommitted)

- 힌트 제안을 실제 보기·닫기 버튼으로 바꾸고 키보드 초점 목록에 연결했다. Tab·포인터 활동은 idle을 갱신하되 이미 뜬 제안을 숨기지 않으며, 유효 게임 조작은 제안을 소비한다. 닫기는 세션 내 계수만 올리고 작업 중인 도구를 보존한다. 도움 열기는 힌트 수준을 자동 공개하지 않는다.
- 제목·부제목과 겹치지 않는 헤더 영역을 확보하고 150% 글자를 검증했다. 원래 180/180초를 유지한 기본 macOS 빌드에서 문서 열람 중 억제, 판독기 제안, Tab/Shift+Tab 접근, Enter 닫기, 재제안의 포인터 도움 열기, Esc로 판독기 복귀를 실제 확인했다.
- 손/크랭크의 수락 시점 `realtimeSinceStartupAsDouble` 시계를 유지하면서 앱 pause/포커스 상실에 취소하고 중단 중 새 요청을 차단했다. 복귀 후 밀린 동작은 재생하지 않는다. Unity 앱 콜백 회귀이며 Editor pause API 동등성을 주장하지 않는다.
- 작업면 C# 기본 알파를 승인 asset과 같은 0.3으로 맞췄다. 임시 Editor 재생성 스모크에서 오프닝 문구·샷 시간·리소스와 표면 기본값 일치를 확인한 뒤 임시 소스/.meta를 제거했다. 문구 고정 테스트는 복원하지 않고 보호 기록명 fallback 차단과 저작 목표 우선순위 3건을 유지·강화했다.
- 손 미적용으로 남아 있던 리그 문서의 과거 상태를 명시적으로 구분하고 manifest §10에 실제 전신·좌손·우손 파일명을 정리했다. 새 Blender 제작이나 재임포트로 로컬 승인 상태를 덮어쓰지 않았다.
- 최종 EditMode 56/56, PlayMode 100 pass/0 fail/격리 부팅 1 skip, 별도 boot 1/1: 고유 157건. 기본 macOS 빌드 410,816,644B 생성과 네이티브 로그 오류 검색 0건. 새 영수증은 `*-contract-corrections.xml`, `profile-recreation.json`, `native-contract-actions.json`; 이전 154건 영수증과 모션 캡처의 이전 assembly 지문은 별도 보존했다.
- 입력 이벤트 순서 추가 확인: `Watch.Activity`는 이미 idle만 갱신하는 `NoteInputActivity`에 연결돼 있으므로 선행 버튼 이벤트가 제안을 숨기지 않는다. 기존 InputSystem Enter/Esc/패드 B 회귀에 ScreenChanged 0·surface 보존·Journal.HeadSeq 불변 단정을 추가하고 해당 3건을 다시 통과했다. 런타임/네이티브 빌드 변경 없음, 고유 157건에 중복 가산 없음. 증거: `hint-input-ordering.xml`.
- 초점 복귀 추가 확인: 이전 일반 초점을 기억한 뒤 닫기 버튼에 초점을 두고 실제 InputSystem Enter로 닫는 기존 사례 1/1을 통과했다. CurrentFocusId 복귀·무재구축·저널 불변을 함께 확인했으며, 네이티브 시각적 복귀 증거로 확대하지 않는다. 증거: `hint-focus-restoration.xml`; 고유 합계 157 및 런타임/빌드 변경 없음.

## 2026-09-13 · Aside 규칙·코어 루프 조사 (연구만, uncommitted)

- 실제 Aside ultrabrowse 실행으로 정본·현재 코드와 공개 원문 12건을 조사했다. 전체 보고서의 10개 권고·3개 실험과 Main 교차 확인은 `handoff/aside-core-loop-results-20260913.md`에서 시작한다.
- 다음 후보는 T0 두 자료/시간창의 동시 비교, 기존 M8를 재사용하는 출처 계보, 조위정합 회색상자다. 현재 C1 고정 근거 ID와 정본/검증 범위 차이를 구별하고 새 보드·전체 캠페인 확대부터 하지 않는다.
- 단일 변수 실험 JSON 및 검증기 self-test 통과. 파일럿의 사전 노출을 정식 한 빌드의 12명/5유형 T0 평가에 섞지 않는다. 실제 사람 n=0이며 구조 PASS는 재미/학습 증명이 아니다.
- 명시적 deep 모델은 크레딧 부족으로 실패 후 기본 프로필로 조사를 마쳤다. 독립 Deep Research SaaS/전용 모델 성공을 주장하지 않으며 이번 권고의 런타임 구현·정본 수정·유료 생성·생산/출시 승격은 없다.

## 2026-09-14 · M23 코어 루프 구현·독립 리뷰·실행 검증

- R1 두 자료/시간창 동시 비교, R2 명시적 출처 선택·미기록 후보 확정 차단, R3 기존 M8 사본 계보와 요청형 질문, R5 격리 정합 연습장, R7 수락/작업저장/미리보기/내구 확정 피드백을 구현했다. 이전 M22/Aside 기반과 미승인 아트 경계를 보존한다.
- 서브에이전트 목표 합의와 독립 구현 리뷰 후 Q1–Q5를 수정했다. 늦은 자동저장 실패·화면 Back·확정 취소 뒤 no-op 저장·키보드 연습 설명·깊은 스크롤의 저장 알림을 검증한다. RED와 첫 Navigation 축소안의 실패도 보존했다.
- Unity64 EditMode +112 PlayMode +1격리 boot=고유177통과. Node7, campaign50, 파생 그래프18통과. macOS315파일/410875105B 빌드와150% 실제 비교·명시 인용/확정·프로세스 재시작 복구를 확인했다. OS 재부팅이나 Windows/사람 증거가 아니다.
- R4 문구/감사 강도 교정, 생성 테이블과 authoring receipt/증거 그래프 동기. 강한 P1/P2 감사는 exit3/blocked이며 약속을 완화하지 않았다.
- 임시 Editor 시편 생성기/.meta 제거. 정확한 지문·선별 스틸·원재현 XML은 `systems/tech-verification/m23/`; 결과/개선 계획은 `handoff/m23-results-and-improvement-plan.md`.
- 사용자 최신 지시는 이번 범위의 commit/일반 push를 승인한다. 위 uncommitted/사용자 전용 표기는 각 과거 기록의 상태다. 사람0명·Base·실제 노력150% STOP·후반R8/R9·전체450–540분·Windows 목표 성능·상업/출시는 여전히 미충족이다.

## 2026-09-14 · M23 최초 전달 후 경계 교정

- 초기35ebdc5 이후 Q6–Q11(S2 다섯·S3 하나)을 교정했다. Undo로 접근이 사라진 고정 자료는 해제하고 Redo에 자동 복원하지 않는다. 연습장 직접 API6개는 취소/전이/설정·노트 쓰기 전에 차단한다.
- 진단 배경에서 비교 카드·CaseThread·연습 대응 문단·트랙/A-B 라벨이 묻히지 않도록 비입력 읽기 배경을 적용했다. 기본 스킨의 색/식별자·미승인 상태는 보존한다. 양 끝 피크 여백은 기본/진단 양쪽에서 수정했다.
- 후속9개 고유 사례의 실제 RED→GREEN, 최종 PlayMode121 +프로젝트 EditMode64 +격리 boot1=186통과. 외부 예제 stub1·반복 실행은 제외한다. 최종 Mac315파일/410875885B, 지문6c88870f…와 실제150% mesh/설명/복귀·저장/백업/설정 바이트 불변을 확인했다.
- 원래177개와17스틸을 덮어쓰지 않고 후속11 XML/11스틸/192이벤트와 중간·최종 전체 빌드 목록을 별도로 보존한다. 잘못 추정한 초점/미이동 합성 입력은 제외한다. 전체 사람/노력/Base/Windows/강한 보존 gate는 변하지 않는다.

## 2026-09-14 · M23 판독기 밖 pin 수명 보강

- 5d7b18c 이후 추가 리뷰에서 Q6의 off-reader 반례를 실제 재현했다. 가시성 검사를 reader 화면에서 공통 Render 진입점으로 옮겨 회로 Undo→Redo→reader 재진입에도 pin이 되살아나지 않는다. 같은 결함의 경계 확장이다.
- 신규 회귀 RED→GREEN, 전체122 PlayMode +64 프로젝트 EditMode +별도 boot1=187통과. 첫 boot의 격리 인자 누락 skip은 실패/통과로 왜곡하지 않고 보존한다.
- 최신 Mac315파일/410875921B·ba14f79e…와 native112–117/54이벤트로 실제 경로를 확인했다. 기존 빌드 목록/스틸/원재현을 보존하고 네이티브 종료·메모리 동기 후 후속 일반 push로 전달한다. 생산 gate는 바뀌지 않는다.

## 2026-09-14 · M23 검증 증거 정합

- 최종 소스7e2c383에서 EditMode65/65를 재실행했다. 외부 예제1개를 제외한 현행 프로젝트 합계187은 더 이상 이전 EditMode 이월에 의존하지 않는다.
- 6c88870f epoch에 정확한 최종 소스20개를 연결하고 Git blob의 바이트/SHA로 확인했다. 완료 시각도 최신 검증 이후로 갱신했다. 런타임·빌드·생산 gate는 바꾸지 않는다.
