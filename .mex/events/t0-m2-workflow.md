---
name: t0-m2-workflow
description: Verified T0 M2 implementation, publication dependencies and partial memory synchronization receipt.
last_updated: 2026-09-10
---

# T0 M2 workflow receipt

[OBSERVED] This is a manually written workflow receipt. It is not a successful `mex log` event: the repository resolver found no verified mex-agent executable. No TeX `mex`, installation or Codex memory-folder update was performed.

## Implementation and verification

- Unity 6000.5.6f1 implements the T0 hub/circuit/reader route, input/UI, atomic save/recovery and undo/redo. The scope remains one vertical slice; 25 minutes is a design target.
- `_workspace/current/systems/tech-verification/t0-m2-native.md` is the native verification report. Its final `results/editmode-7.xml` has 18 passed testcases; `results/playmode-11.xml` has 15 passed testcases. Both have zero failed, skipped or inconclusive cases. M1's 21 internal checks are included in one EditMode testcase, not added to this total.
- `t0-m2/logs/build-framing.log` records `T0_MAC_BUILD Succeeded bytes=314105987` and a successful batchmode exit for `BuildMacFramingFix`.
- `t0-m2/source-manifest.json`, observed at 2026-09-10T09:23:05.846Z, contains 92 files. All recorded SHA-256 values matched current bytes during this integration. Its baseline receipt remains separate.
- `_workspace/current/systems/tech-verification/t0-m2-player-smoke.md` records the director's actual macOS player interaction: complete route, process exit/relaunch, restored completion/settings and Continue. Corrected-player observations cover the visible drawer front/handle, ledger scrolling without header overlap, restored completion/settings and Continue. Retained logs and isolated save snapshots are under `t0-m2/player-smoke/`.
- These receipts do not establish representative human usability, physical gamepad operation, baseline-hardware performance, actual 25-minute playtime or G4/G5/G6 completion.

## Final workflow checks

Commands below ran from `/Users/jangyoung/orca/unknown` through `rtk`.

| Check | Command / evidence | Outcome |
|---|---|---|
| Session contract | `bash .claude/skills/game-ops-harness/scripts/session-start.sh "T0 M2 final verified source and native smoke workflow receipt" /Users/jangyoung/orca/unknown` | exit 0; `/tmp/unknown-t0-m2-session-verified.log` |
| Cycle freshness | `bash .claude/skills/game-ops-harness/scripts/freshness-check.sh --root /Users/jangyoung/orca/unknown --since 2026-09-09` | exit 0, zero findings across 206 Markdown artifacts; `/tmp/unknown-t0-m2-freshness-verified.log` |
| mex graph/check/log | identity-gated resolver in `scripts/mex-agent-bin.sh`; `/tmp/unknown-t0-m2-final-mex.log` | all skipped; wrapper exit 3 because mex-agent is unavailable |
| Final code graph | `graphify update . --force` | exit 0; `/tmp/unknown-t0-m2-graph-final.log` |
| zg status | session-start's existing read-only status step | `ZVEC_GREP.ENGINE.LOCK.BUSY`, owner operation `index`; no index rebuild/drop or lock removal attempted |

The active artifact cycle is `20260909-preproduction-c7`; September 9 is the cycle-start freshness boundary. A stricter September 10 check retained one finding for `intake/production-brief.md`, updated September 9. That document was not restamped. An earlier ENOSPC run is invalid as freshness evidence; its log was preserved separately.

[OBSERVED] Final graph: 7,270 nodes, 7,573 edges, 979 communities. Unity cache nodes and Unity cache manifest entries are both zero. `Assets/_Project` contains 1,169 nodes from 49 source files; all 32 C# files in the final source receipt have matching graph AST hashes. This is code extraction evidence; semantic extraction of every changed document was not performed.

The earlier cache repair preserved all 7,117 non-cache predecessor nodes. Graphify 0.8.14's ordinary update preserves excluded old nodes, so one repair used the installed rebuild function's documented `changed_paths` eviction behavior, followed by its `save_manifest` helper. Final ordinary update retained the clean exclusions. Rollback: `/tmp/unknown-t0-m2-graph-before-cache-eviction.tar.gz`; repair evidence: `/tmp/unknown-t0-m2-graph-cache-fix.log`. No authoritative wiki graph or zg index was changed by this integration.

[OBSERVED] G8 remains partial: Markdown freshness passed, but mex drift verification is unavailable and zg status is busy. A successful shell exit from session-start does not override those per-tool outcomes.

## Publication boundaries

The packaged project can open, run and build using `unity/Unknown/Assets/` including every `.meta`, both `Packages` JSON files, complete `ProjectSettings/` and the project `.gitignore`. Do not use the source-hash receipt as a staging manifest: it excludes `.meta` and most settings. Do not stage the whole repository or generated player/cache folders.

`T0ProjectBuilder.Prepare`, `T0AssetImporter.Import` and the legacy M1 `RunBatch` additionally require SHA-matching `_workspace/current/planning/campaign.json`. `BuildMac` uses existing scenes; `BuildMacFramingFix` applies its packaged scene adjustment and builds. Full asset diagnostics require `assets/generated/3d/hub-view-drawer-r03/` GLB, FBX and textures; preserve its provenance and `assets/generated/3d/scripts/build_hub_drawer_r03.py`. Preserve `assets/generated/audio/higgsfield-stamp-r01/` as candidate provenance, not proof of audible runtime integration.

Full table regeneration has this explicit dependency closure under `_workspace/current/`; several files predate the implementation and require deliberate whole-file inclusion rather than broad lane staging:

- `systems/pipeline/emit-tables.mjs`
- `planning/campaign.json`, `planning/validate-campaign.mjs`, `planning/t0-circuit-overlay.json`, `planning/gdd.md`
- `synopsis/t0-records.md`
- `systems/data-schemas/tools.md`, `systems/data-schemas/zones.md`
- `concept/style-guide.md`
- `modeling/specs/hub-watchroom.md`, `modeling/asset-manifest.md`
- `worldview/worldview-bible.md`
- `systems/system-specs/wiring-trace.md`, `systems/system-specs/plate-readout.md`, `systems/interaction-rules.md`

Include existing metadata sidecars when publishing generated JSON/evidence. Resource approval and design decisions remain in their canonical production/modeling/concept documents; this receipt does not promote candidate assets or replace lane ownership.
