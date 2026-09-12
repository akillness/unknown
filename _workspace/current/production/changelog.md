---
updated: 2026-09-11
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
