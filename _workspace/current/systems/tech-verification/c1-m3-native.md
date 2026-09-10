---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
decision: RFC-CX-004
---

# C1 M3 native verification

[OBSERVED] Unity 6000.5.6f1 built the T0→C1 first-beat slice. Final EditMode: 27/27; PlayMode: 28/28. macOS build succeeded, reported 331,066,757 bytes. Existing renderer-occlusion/case-thread work and its tests were preserved. The 28 PlayMode testcases include those existing regressions; they are not 28 new C1 cases.

Scope: `c1-b1` only. No full-C1, campaign playtime, performance, G4/G5/G6, or human-playtest claim follows from these automated tests. Actual standalone pointer/visual/restart evidence belongs to the director's separate `c1-m3/player-smoke/` receipts.

## Commands and observed results

Common executable: `/Applications/Unity/Hub/Editor/6000.5.6f1/Unity.app/Contents/MacOS/Unity`. Common project: `/Users/jangyoung/orca/unknown/unity/Unknown`. All calls were routed through `rtk proxy`.

| Command arguments after executable | Receipt | Observed |
| --- | --- | --- |
| `-batchmode -nographics -projectPath <project> -runTests -testPlatform EditMode -assemblyNames Tide.Tests.Sim -testResults <c1-m3>/results/editmode-3.xml -logFile <c1-m3>/logs/editmode-3.log` | `c1-m3/results/editmode-3.xml` | 27/27 PASS, exit 0 |
| `-batchmode -nographics -projectPath <project> -runTests -testPlatform PlayMode -assemblyNames Tide.Tests.Play -testResults <c1-m3>/results/playmode-1.xml -logFile <c1-m3>/logs/playmode-1.log` | `c1-m3/results/playmode-1.xml` | 28/28 PASS, exit 0 |
| `-batchmode -quit -projectPath <project> -executeMethod Tide.EditorTools.C1ProjectBuilder.ImportAndCapture -logFile <c1-m3>/logs/panel-diagnostic-3.log` | `c1-m3/panel/import-audit.json`, front/rear PNG | Native Metal render; 6 meshes, 2,092 triangles; preserved source texture inputs plus exact constant-factor adapters |
| `-batchmode -quit -projectPath <project> -executeMethod Tide.EditorTools.C1ProjectBuilder.ApprovePanelAndBuildMac -logFile <c1-m3>/logs/build-mac-1.log` | `c1-m3/logs/build-mac-1.log` | Succeeded, 331,066,757 bytes, exit 0 |

`<c1-m3>` expands to `/Users/jangyoung/orca/unknown/_workspace/current/systems/tech-verification/c1-m3`. Build: `unity/Unknown/Builds/C1-M3-mac/Unknown.app`; actual executable basename is `Unknown T0`, retained from existing PlayerSettings. Launch with `--t0-save-dir <isolated-save-directory> -logFile <player-log>`.

## Failure/recovery evidence

EditMode run 1 found C1 reducer fallthrough into the old unknown-event rejection (21 pass / 4 fail). Dispatch was repaired; run 2 passed 25/25. Run 3 added individual missing-clue, all-invalid-preview recovery, and double-fold checks and passed 27/27. The copied physical v1 fixture preserves source SHA-256 `6625ef4e331d893ae142fbd1eabc63e683095194f19e00efe5aa7ff53e394a57`.

PlayMode proves the public immediate API cannot apply `ConfirmPatrol`; failure at the second rename boundary (after successful precommit checkpoint) leaves live and persisted state unchanged. Retry publishes one chapter result. Cancellation during the candidate write suppresses all chapter effects and stale receipt. Keyboard/pad two-step, hold gesture, terminal-stage restart, and original T0 tests passed.

Native diagnostic 1 exposed incorrect replacement of the imported FBX axis rotation. Diagnostic 2 preserved the imported rotation with a 180-degree yaw adapter but exposed white solid-factor materials. Diagnostic 3 converted glTF factors into 1×1 base/roughness inputs for the existing verified URP shader; source files remain unchanged. Director approved this exact static adapter under RFC-CX-004. Blank readout/plaques and warm lens are decorative, with no live measurement claim.

## Knowledge receipts

Ran session-start, `zg query 'T0 data loader JournalSave AtomicSaveStore screen render beat completion commands C1 patrol'`, and `graphify query 'How are T0 journal commands persisted and rendered to UI from snapshots?'` before code edits. The mex-agent identity probe failed; PATH `mex` is TeX and was not invoked. `[UNGRAPHED]` applies until the director's final graph update receipt is filed. The director owns the final `graphify update .`, `mex graph/check/log` skipped receipt, and durable wiki integration for the combined task.
