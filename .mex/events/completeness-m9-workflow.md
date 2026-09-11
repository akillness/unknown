---
name: completeness-m9-workflow
description: M9 completeness hop — audit-driven contract fulfilment across core loop, tutorial, presentation and balance
last_updated: 2026-09-11
---

# M9 completeness hop (RFC-CX-011)

[OBSERVED] Three READ-ONLY lane audits ran in parallel before any edit; every implemented slice maps to an existing canonical contract (interaction-rules §1-1/§4, GDD §3.3, hint-system.md, ai-native-m8 planning/direction). Zero new balance numbers; M8 presentation timings authored in `Resources/M8ReviewVfx.json`.

[OBSERVED] Two implementers ran with a strict file-ownership split (SYS-CORE: T0GameSession/T0OpeningSession/JournalSave/T0Strings/T0RuntimeConfig/T0ProjectBuilder + core tests; SYS-M8: ReviewNotesSession/T0ReviewNotesInterface + new Profile/Vfx/Builder + review tests) joined by one method contract `OpenReviewNotes()`. Director owned the GTI lane. Mid-flight rulings: test-boundary extensions for the two-step default; disclosure conflict → option B data-driven guard (record display name in objective → fallback), `AssertNoDisclosure` untouched.

[OBSERVED] Wiring used `T0ProjectBuilder.WireBeats` (beats only) and `M8ReviewNotesProjectBuilder.ImportReviewCard` (sha-checked, `runtimeApproved:false`). Full `Prepare` was avoided: it rebuilds scenes that later milestone builders own.

[OBSERVED] Final native: EditMode 53/53, PlayMode 68/68, serialized boot 1/1 (requires `/tmp/unknown-c1-m4-boot-*` prefix), macOS build Succeeded 354233020 B, player alive under `--m8-review-notes-diagnostic`. Prior M5 reduced-motion failure passes on the preserved uncommitted fix. Receipts: `_workspace/current/systems/tech-verification/completeness-m9/`.

[OBSERVED] `mex` on PATH resolved to mex-agent 0.7.1 (not TeX) in this session; `mex graph`/`check`/`log` ran. `mex check` drift (4 errors) is pre-existing ROUTER path globs, not M9. `graphify update .` exit 0. Freshness check exit 0 / 0 findings / 598 artifacts. Obsidian not running → vault written directly (`wiki/reports/2026-09-11-unknown-m9-completeness-hop.md` + index/log lines).

[OBSERVED] Subagent results were lost at turn boundaries three times (audit batch, first implementer pair, SYS-M8's report step). Recovery came from file-persisted `impl-*.md` reports and director-side diff review. Pattern to keep: any lane whose result matters beyond the turn writes its own receipt file before yielding.

[CARRIED] GTI card `m8-review-card-r01` stays a candidate until window-level readability review (desktop-only capture discarded). Deferred: `hintOfferCooldownSeconds` split, EN hint textKey, `alignment` verb re-intake, ReadOriginal wear-undo ruling, t0-b1 spoiler-free objective (planner). Parallel sessions (M7 texture QA, M10 TRACE-RPG) edit the same tree; M9 wrote none of their lane files. Commit/push by user.

[OBSERVED] QA FIX cycle 1: S2 D-M9-01 (C1 `원문 열기` left `document` set → Esc reopened notes) fixed via `ReviewSourceOriginalAvailable` gate + `C1StagesNeverOfferSourceOriginalOpen`; Esc→`CloseReviewNotes`; `T0Simulation.IsSatisfied` single source; `select_ms` dropped (no consumer; per-toggle fade ≠ outline change). Post-fix EditMode 53/53, PlayMode 69/69, boot 1/1, build 354230869 B. Launch receipt replaced by transcript + Player.log after discovering the first smoke's stale-pid kill (player alive 31 min). QA R2 confirms closure separately.

[OBSERVED] QA FIX cycle 2 (last allowed): R2 closed all six FIX-1 repairs independently and opened adjacent S2 D-M9-13 (tool panel open → `원문 열기` return flag cleared next frame + hidden-tool input under the document). Minimal repair `tool==null` in `ReviewSourceOriginalAvailable`; regression `OpenToolPanelsNeverOfferSourceOriginalOpenWhileShellStillDoes`; G-2 `evidence` restoration assert. EditMode 53/53, PlayMode 70/70, boot 1/1, build 354230868 B. FIX budget exhausted (2/2); a re-opened S2 at R3 becomes an open defect with hop completion held, not a REDO.

[OBSERVED] Closure: QA R3 S1 0 · S2 0 · S3 1 (planner-owned, out of M9 code scope) · S4 1 (impl wording, corrected). Director ruling: in-scope PASS at automation/document level; G4/G7 runtime PASS not claimed. Final EditMode 53/53, PlayMode 70/70, boot 1/1, build 354230868 B. Freshness exit 0. Commit/push by user.
