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
last_updated: 2026-09-11
---

# Session Bootstrap

If you haven't already read `AGENTS.md`, read it now — it contains the project identity, non-negotiables, and commands.

Then read this file fully before doing anything else in this session.

## Current Project State

**Resource provider override (2026-09-11):** New image generation uses god-tibo-imagen (GTI) per the latest user instruction, the image-provider directive in `CLAUDE.md` and RFC-CX-006. Blender 3D and Higgsfield video workflows continue. Preserve historical provider provenance and approved runtime assets. GTI 0.3.0 and valid local Codex authentication verified without exposing credentials.
**Working:**
- M8 optional offline review notes are implemented in App/ReviewNotesSession.cs, Save/ReviewNotesStore.cs and UI/T0ReviewNotesInterface.cs. Source analysis, exact-source native receipts and baseline M5 regression isolation: `_workspace/current/systems/tech-verification/ai-native-m8/verification.md`. Notes cannot confirm evidence or unlock content; no runtime AI or new art resources.
- Harness scaffold: 14 agent definitions (`.claude/agents/game-*.md`), skill `game-ops-harness` with references + scripts, root `CLAUDE.md` rule file (§10.1 re-derived 2026-09-10)
- Preproduction documents C1–C5 in `_workspace/current/` (canon: worldview/*, planning/campaign.json + validate-campaign.mjs, systems/{architecture-contract,system-specs,data-schemas}, balance, economy, product, presentation deck); C1–C2 and the session-P C3 versions archived under `_workspace/archive/20260909-preproduction-c{1,2,3,4,5-prep}/`
- Unity 6000.5.6f1 project at `unity/Unknown/` has the T0 M2 hub/circuit/reader runtime, input/UI, atomic save/recovery and undo/redo. Start with `_workspace/current/production/codex-t0-m2-status.md` and `_workspace/current/systems/tech-verification/t0-m2-native.md`; final test/build/player receipts are separate evidence. Historical M1's 21 internal checks are contained in one EditMode testcase and must not be added to the NUnit count.
- Asset pipeline: `scripts/gen-2d.sh` (GTI), `scripts/gen-video-higgsfield.sh`, `scripts/make-previz-gif.sh`, `scripts/refresh-2d-provenance.py`; outputs under `assets/generated/{2d,3d,video,previz}/` with provenance.json; README media in `docs/media/`
- Memory stack: `.mex/` scaffold, `graphify-out/` code graph, `.zvec-grep/` index, vault `~/vaults/llm-wiki/wiki/projects/unknown/`

**Not yet verified / pending:**
- Full-campaign production, representative human playtests and baseline-hardware performance remain gated. The 25-minute T0 length is still a design target; native test and director smoke receipts do not promote G4/G5/G6.
- Required package resolution is blocked by disk ENOSPC; native Editor contract checks are not NUnit/EditMode or PlayMode test-runner receipts. Actual source data still lacks citation provenance, so t0-b3 remains incomplete. Gates G2/G4/G5/G6/G7 stay NOT-MEASURED; playtests n=0.
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
