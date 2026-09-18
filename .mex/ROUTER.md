---
name: router
description: Session bootstrap and navigation hub. Read at the start of every session before any task. Contains project state, routing table, and behavioural contract.
edges:
  - target: context/architecture.md
    condition: when working on system design, integrations, or understanding how components connect
  - target: context/stack.md
    condition: when working with specific technologies, libraries, or making tech decisions
  - target: context/conventions.md
    condition: when writing new code, reviewing code, or unsure about project patterns
  - target: context/decisions.md
    condition: when making architectural choices or understanding why something is built a certain way
  - target: context/setup.md
    condition: when setting up the dev environment or running the project for the first time
  - target: patterns/INDEX.md
    condition: when starting a task — check the pattern index for a matching pattern file
last_updated: 2026-09-18
---

# Session Bootstrap

If you haven't already read `AGENTS.md`, read it now — it contains the project identity, non-negotiables, and commands.

Then read this file fully before doing anything else in this session.

## Current Project State

**Resource provider (2026-09-18, RFC-CX-M25-20260918):** the user's latest instruction makes **Higgsfield CLI** the provider for M25 images/video (`gpt_image_2`, `nano_banana_flash`, `seedance_2_0`), superseding the 2026-09-11 GTI override for this cycle. Generator: `scripts/gen-higgsfield.py` + `_workspace/current/concept/m25-higgsfield-jobs.json`; outputs `assets/generated/{2d,video}/m25/` with provenance (`runtimeEligible:false`). Blender 3D continues. Historical provenance preserved.
**Working:**
- M25 Higgsfield resources · guide overlay · readability · build/release (2026-09-18): start with `_workspace/current/systems/tech-verification/m25/verification.json` and `docs/media/m25/provenance.meta.md`. 18 images + 3 clips generated (2 quay rejections kept), imported by `Editor/M25ResourceProjectBuilder.Import` (SHA-checked, 18 items) and approved as the **local development profile only** (`Resources/M25Resources.asset`; commercial licence UNVERIFIED). Runtime: opening still + `VideoPlayer` clip (still under reduced motion/batchmode/error), start-screen watch-room backdrop, tool-wheel icons, `guide` overlay (F2 · toolbar · start screen) with objective/steps/tools/controls/rules/cast(Seorin, Doyeon)/zones, structured teaching header, `Tide.UI.TypeScale` (30/24/21/18/18/17/15/14, line 1.15). Receipts: EditMode 65/65, PlayMode 134 (133/0/1 conditional skip), isolated boot 1/1, build 316 files/438,910,587 B digest `aaa5361d…`, 5 native captures. Delivery: `main` push + GitHub Release `v0.25.0-dev` (unsigned dev zip). Human playtest n=0; performance/Windows unmeasured.
- M24 character detail / gameplay refresh (2026-09-14): start with `_workspace/current/systems/tech-verification/m24/verification.json` and `docs/media/gameplay-m24/provenance.json`. Actual Blender MCP revised face/hair/garment geometry to19,618 triangles/50 bones under the20k cap; original scene/structure passed19 recorded checks, three skeletons/11 takes and hand geometry stayed unchanged. Reimport closed the profile gate; Main reviewed native appearance/contact/return and reapproved only the default local development profile. Final Mac315 files/411,041,089B, digest115fc67e…, no diagnostic flags, isolated T0-b3. New18-second native title/read/comparison edit and separate Blender comparison; save unchanged by comparison. M23's187 tests are historical, not rerun in M24. Human/performance/Windows/Base/full-campaign/commercial gates remain unchanged; source runtimeEligible:false. User explicitly authorized relevant commit/normal push.
- M23 integration and final pin-lifetime correction (2026-09-14): start with `_workspace/current/handoff/m23-results-and-improvement-plan.md` and `systems/tech-verification/m23/verification.json`. Publications35ebdc5 and5d7b18c are historical. Q6–Q11 corrected pin visibility, six direct practice API boundaries, diagnostic text/card/track contrast and peak clipping. A further Q6 regression exposed off-reader resurrection: availability reconciliation now runs at every session Render before selecting a surface. Current187 unique project Unity passes =64 EditMode +122 PlayMode +1 isolated boot;10 distinct correction regressions passed, vendor stub excluded. Final Mac315 files/410875921B, fingerprintba14f79e34fb87473660a4d6009c04796024d71825e1ea378f8f4c634e716161. Native112–117 proves off-reader Undo/Redo and one-chart reentry; earlier practice/native evidence retains its own build epoch. Human n=0, strong preservation blocked, actual effort/Windows/full campaign unmeasured, R8/R9 Base-gated. Scoped commit/normal push is authorized, not production approval.
- Aside core-loop research (2026-09-13): start with `_workspace/current/handoff/aside-core-loop-results-20260913.md`, then `planning/aside-core-loop-research-20260913.md` (12 public sources, 10 recommendations, 3 experiments). Actual CLI `--effort ultrabrowse`; Deep Research skill/source/browser workflow completed with default-profile fallback after deep-model credit failures. Priority: T0 player-selected two-record comparison, source lineage, and alignment greybox before expansion. The experiment JSON is structurally valid but unexecuted; human n=0, canon/runtime/production approval unchanged.
- Concurrent Git state at research closure (2026-09-13): the single `main` worktree is now at `b2ae1fb8119be1b797f874c38441cbd433aa72d4` (external `docs(m13)` cleanup-receipt commit, parent `52ad551`). M22/runtime and research changes remain uncommitted; the observation reported staged 0/unstaged 66/untracked 73. This assistant did not stage, commit, push or fetch. Earlier 52ad551 receipts are historical, not current HEAD. See `planning/core-loop-research-20260913/aside-run.json`.
- M22 default local native integration (RFC-CX-017/018, 2026-09-13): Blender-MCP Han Seorin/hands, reader contact/return/reduced-motion, accessible hint-offer controls and app-pause/focus cancellation are integrated. Corrected proof: EditMode 56/56, PlayMode 100 pass/0 fail/1 isolated-boot skip, separate boot 1/1 (157 unique passes), default macOS build and actual 150%-text/180-second native hint interaction captures. Start with `_workspace/current/systems/tech-verification/m22/verification.json` and `docs/media/gameplay-m22/native-contract-provenance.json`; older body/motion media retain their previous assembly fingerprint. Only `main` worktree exists; no completed worktree to remove, no commit/push. M20 r02 remains unapproved; no human/performance/commercial gate promotion.
- Aside scoped immersion update (2026-09-13, parallel with OMP M22/Blender): M5 opening copy/defaults introduce the last watch and list/drawer comparison; T0 objective fallback is spoiler-safe. Original source hashes, static 17/17, campaign 50/50 and license-blocked test attempt: `_workspace/current/systems/tech-verification/aside-immersion-20260913/`. M22 later removed the copy/default snapshot test, strengthened protected-record fallback invariants and passed the remaining 3 behavioral cases in its final EditMode run; disposable `m22/profile-recreation.json` separately proves opening/default/resource parity without changing persistent assets. The two GTI candidates remain runtimeEligible:false; opening needs revision, navigation source contrast is paper7.76:1/muted4.20:1 only. Original handoff: `_workspace/current/handoff/aside-immersion-20260913.md`; no art promotion, commit or push.
- M8 optional offline review notes are implemented in App/ReviewNotesSession.cs, Save/ReviewNotesStore.cs and UI/T0ReviewNotesInterface.cs. Source analysis, exact-source native receipts and baseline M5 regression isolation: `_workspace/current/systems/tech-verification/ai-native-m8/verification.md`. Notes cannot confirm evidence or unlock content; no runtime AI or new art resources.
- Harness scaffold: 14 agent definitions (`.claude/agents/game-*.md`), skill `game-ops-harness` with references + scripts, root `CLAUDE.md` rule file (§10.1 re-derived 2026-09-10)
- Preproduction documents C1–C5 in `_workspace/current/` (canon: worldview/*, planning/campaign.json + validate-campaign.mjs, systems/{architecture-contract,system-specs,data-schemas}, balance, economy, product, presentation deck); C1–C2 and the session-P C3 versions archived under `_workspace/archive/20260909-preproduction-c{1,2,3,4,5-prep}/`
- Unity 6000.5.6f1 project at `unity/Unknown/` has the T0 M2 hub/circuit/reader runtime, input/UI, atomic save/recovery and undo/redo. Start with `_workspace/current/production/codex-t0-m2-status.md` and `_workspace/current/systems/tech-verification/t0-m2-native.md`; final test/build/player receipts are separate evidence. Historical M1's 21 internal checks are contained in one EditMode testcase and must not be added to the NUnit count.
- Asset pipeline: `scripts/gen-2d.sh` (GTI), `scripts/gen-video-higgsfield.sh`, `scripts/make-previz-gif.sh`, `scripts/refresh-2d-provenance.py`; outputs under `assets/generated/{2d,3d,video,previz}/` with provenance.json; README media in `docs/media/`
- M7 concept-first resources (RFC-CX-009 delivery, 2026-09-11): 5 GTI material texture candidates at `assets/generated/2d/texture/m7-*-r01/` (basecolor GTI + derived-luminance channel candidates), Blender blockout `assets/generated/3d/concept-first-m7/` (4 targets, build script `scripts/blender/build_m7_assets.py`, GLB export candidates, articulation proof). Pre-runtime QA done: no material tileable unblended (salt-concrete horizontal-only by design); 180deg crank stroke clips bench 0.025m (limit ~150deg before rigging); GLB uses Smart-UV, not box projection. All `runtimeEligible:false`; in-engine seam check and native verification open. mex-agent unavailable → memory_sync receipt `skipped`. **Runtime wiring (RFC-CX-013 delivery, 2026-09-11):** those candidates are now imported into `unity/Unknown` (Unity 6000.5.6f1, URP 17.5.0) and bound to the T0 stage-1 runtime behind per-lane diagnostic gates — HubShell (`Resources/M7Hub.asset`, `Editor/M7HubProjectBuilder.cs`, `App/M7HubSession.cs`, textures=6 materials=3), ReaderStage (`Resources/M7ReaderStage.asset`, `Editor/M7ReaderProjectBuilder.cs`, `App/M7ReaderSession.cs`, fbx=2 textures=8 materials=7, camera solved from imported bounds), UiSkin (`Resources/M7UiSkin.asset`, `Editor/M7UiSkinProjectBuilder.cs`, `App/M7UiSession.cs`, textures=2); FBX candidates via `scripts/blender/export_m7_fbx.py`. Gate = `runtimeApproved || --m7-{hub,ui,reader}-diagnostic`; with the gate off the committed hub scene, UI literals and stage flow are untouched. Verified: EditMode 53/53, PlayMode 78/78 (8 new M7 tests, skipped 0 → no `Assert.Ignore` fallback), native capture 7 shots at 1280x800 in `unity/Unknown/Builds/m7-diagnostics/` (`crankStrokeDeg 150.0`, `readerTriangles 8028`). All three profiles stay `runtimeApproved:false`; batch runs need `--burst-disable-compilation` (graphics batchmode segv in the Burst compiler). Receipts: `_workspace/current/systems/tech-verification/concept-first-m7/runtime/`. Promotion needs `Tools/M7/Approve …(director only)` + a decision-log audit (asset-runbook §3.1, 8 items); human playtest n=0 and G4/G5/G6/G7 stay unmeasured.
- M9 completeness hop (RFC-CX-011, 2026-09-11): per-beat hint persistence (`hintLevelUsed`), `two-step` default confirmation, `Simulation.Preview` diff sentences on the T0 confirm overlay, `beats.json` per-beat objective behind a record-name disclosure guard, `toolTeaching`-driven panel guidance, M8 targets closed (`검토 질문 보기` on press, source original open/return, independent-media branch), `Resources/M8ReviewVfx.json` transitions + `M8ReviewNotesProfile` gate, `T0ProjectBuilder.WireBeats`, GTI card candidate `assets/generated/2d/texture/m8-review-card-r01/` (`runtimeApproved:false`). Native: EditMode 53/53, PlayMode 68/68, boot 1/1, mac build OK. Receipts: `_workspace/current/systems/tech-verification/completeness-m9/`. `mex` on PATH resolved to mex-agent 0.7.1 in this session (graph/check/log ran; check drift is pre-existing path globs).
- Memory stack: `.mex/` scaffold, `graphify-out/` code graph, `.zvec-grep/` index, vault `~/vaults/llm-wiki/wiki/projects/unknown/`

**Not yet verified / pending:**
- Full-campaign production, representative human playtests and baseline-hardware performance remain gated. The 25-minute T0 length is still a design target; native test and director smoke receipts do not promote G4/G5/G6.
- Earlier M2 ENOSPC/package and missing citation-provenance blockers are historical, not the current executable state. M22 has real NUnit receipts, serialized boot, successful native build and actual original-reading/copy interaction evidence. These do not establish full-campaign completion or promote human/performance gates; playtests remain n=0.
- Production telemetry and unattended capture automation remain outside the verified T0 slice. See `events/t0-m2-workflow.md` for current freshness/memory-sync outcomes and publication boundaries.

**Known issues:**
- `mex` on PATH is TeX Live; only an identity-probed `mex-agent` may run (`scripts/mex-agent-bin.sh`); otherwise memory_sync receipts are `skipped`
- `LLM_WIKI_VAULT` env points at another project; the harness reads `GAME_OPS_VAULT`
- GTI backend accepts only `--model gpt-6-astra` for this Codex login and ignores `--size` on landscape/square requests (actual dims recorded in provenance)
- Two sessions wrote the workspace concurrently on 2026-09-09; merged via RFC-P3-008..015 (see `_workspace/current/conflicts.md`)
## Routing Table

Load the relevant file based on the current task. Always load `context/architecture.md` first if not already in context this session.

| Task type | Load |
|-----------|------|
| Understanding how the system works | `context/architecture.md` |
| Working with a specific technology | `context/stack.md` |
| Writing or reviewing code | `context/conventions.md` |
| Making a design decision | `context/decisions.md` |
| Setting up or running the project | `context/setup.md` |
| Any specific task | Check `patterns/INDEX.md` for a matching pattern |

## Behavioural Contract

For every task, follow this loop:

1. **CONTEXT** — Load the relevant context file(s) from the routing table above. Check `patterns/INDEX.md` for a matching pattern. If one exists, follow it. Narrate what you load: "Loading architecture context..."
2. **BUILD** — Do the work. If a pattern exists, follow its Steps. If you are about to deviate from an established pattern, say so before writing any code — state the deviation and why.
3. **VERIFY** — Load `context/conventions.md` and run the Verify Checklist item by item. State each item and whether the output passes. Do not summarise — enumerate explicitly.
4. **DEBUG** — If verification fails or something breaks, check `patterns/INDEX.md` for a debug pattern. Follow it. Fix the issue and re-run VERIFY.
5. **GROW** — After meaningful work, run this binary checklist:
   - **Ground:** What changed in reality? Name the changed behavior, system, command, dependency, or workflow.
   - **Record:** If project state changed, update the "Current Project State" section above. If documented facts changed, update the relevant `context/` file surgically.
   - **Orient:** If this task can recur and no pattern exists, create one in `patterns/` using `patterns/README.md`, then add it to `patterns/INDEX.md`. If a pattern exists but you learned a gotcha, update it.
   - **Write:** Bump `last_updated` in every scaffold file you changed. If the why matters, run `mex log --type decision "<what changed and why>"` or `mex log "<note>"`.
