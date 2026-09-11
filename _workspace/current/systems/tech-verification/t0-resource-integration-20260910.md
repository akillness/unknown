---
updated: 2026-09-10
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: systems
---

# T0 approved resource interaction — bounded cycle

## Scope and selection

[OBSERVED] Complete and uncommitted. Baseline and final HEAD are `687477d6428c7fc94e681cc6d11045acadc91aa1`. This cycle selects exactly one interaction: left-click the visible, approved r03 drawer in the T0 hub to enter the existing `hub-view-drawer` investigation. It reuses `GoNode` and `inspect-plate-zero`; it adds no puzzle command, animation, physical tray travel, resource generation, approval change, or production-gate claim.

The entry audit read `CLAUDE.md`, `.mex/ROUTER.md`, session-start output, `t0-m2-native.md`, `t0-m2-player-smoke.md`, and the committed RFC-CX-003 r03 approval. Session-start completed with exit 0 and 0 structural freshness findings across 207 Markdown artifacts; mex-agent was unavailable and the workspace index reported that an update was needed. These do not establish G8 completion.

## P1 renderer-only occlusion follow-up — current final evidence

[OBSERVED 2026-09-10] Fixed and left uncommitted. The reviewed defect allowed a nearer visible renderer without a Collider to leave the drawer selectable. The production delta is limited to `T0GameSession.cs` and `T0ResourceInteractionTests.cs`; `occlusion-fix.diff` compares against the pre-task uncommitted integration, not HEAD. The original report is retained as `occlusion-pre-fix-report.md.txt`; the original receipts and inventories below remain historical.

The guard reuses world-space Renderer bounds: after the existing qualified left-click/UI and Physics checks, it checks other scene renderers for a strictly nearer ray hit. Both drawer and occluder eligibility use the interaction camera's frustum and culling mask plus active/enabled/forceRenderingOff/shadow-only state. Drawer descendants are excluded from blockers. The keyboard navigation path is unchanged. No collider cooking, new imported/generated asset, resource approval, material, mesh, transform or scene change is required.

| Verification | Current observed result | Receipt under `t0-resource-integration-20260910/` |
| --- | --- | --- |
| Focused meaningful RED | Exit 2; 1 test failed at `A nearer visible renderer without a Collider must block drawer selection`: expected false, actual true. Collider absence, nearer ray/frustum hit and Physics miss passed first. Runtime was byte-identical to the pre-fix source. | `occlusion-focused-red.xml/log`, `occlusion-red-source-hashes.json`, `occlusion-red-T0GameSession.cs.txt` |
| Final focused GREEN | Exit 0; 6/6 tests, 0 failed/skipped. | `occlusion-focused-green.xml/log`, `occlusion-focused-receipts.json` |
| Full EditMode | Exit 0; 19/19, 0 failed/skipped; no assembly/test filter. | `occlusion-editmode-final.xml/log` |
| Full PlayMode | Exit 0; 21/21, 0 failed/skipped; no assembly/test filter. | `occlusion-playmode-final.xml/log` |
| BuildMac | Exit 0; Succeeded, 314,122,250 bytes; unchanged BuildMac method in isolated APFS clone. | `occlusion-build-mac.log`, `occlusion-build-inputs.json`, `occlusion-player-manifest.json` |
| Automated native Metal smoke | PASS: visible click selects; renderer-only blocker rejects; disabling it restores click; keyboard selection works while occluded; puzzle state unchanged. | `occlusion-native-foreground/native-result.json`, `occlusion-native-player-foreground.log`, five PNG captures |
| Independent source review | No P1/P2 findings in the new runtime delta or temporary diagnostic driver. | `occlusion-verification.json` (`/root/qa_review`) |

The new focused regression injects MouseState/KeyboardState through the committed hub fixture. The second new test exercises seven nonblocking conditions: disabled, inactive, forceRenderingOff, camera-excluded layer, shadows-only, behind drawer, and behind camera. The existing four tests retain valid selection, UI/context guards, non-readable resource identity and collidable occlusion coverage. Focused counts are subsets of PlayMode, not additional suite totals.

Strict TDD order and exact focused argv are in `occlusion-focused-receipts.json`. The initial misspelled namespace run selected zero tests and is explicitly **not** RED or GREEN evidence. The first six-test GREEN is retained separately; the final rerun follows an added fixture precondition confirming that the occluder layer initially belongs to the camera mask.

Final full commands (Unity 6000.5.6f1):

```sh
rtk proxy /Applications/Unity/Hub/Editor/6000.5.6f1/Unity.app/Contents/MacOS/Unity -batchmode -nographics -projectPath /Users/jangyoung/orca/unknown/unity/Unknown -runTests -testPlatform EditMode -testResults /Users/jangyoung/orca/unknown/_workspace/current/systems/tech-verification/t0-resource-integration-20260910/occlusion-editmode-final.xml -logFile /Users/jangyoung/orca/unknown/_workspace/current/systems/tech-verification/t0-resource-integration-20260910/occlusion-editmode-final.log
rtk proxy /Applications/Unity/Hub/Editor/6000.5.6f1/Unity.app/Contents/MacOS/Unity -batchmode -nographics -projectPath /Users/jangyoung/orca/unknown/unity/Unknown -runTests -testPlatform PlayMode -testResults /Users/jangyoung/orca/unknown/_workspace/current/systems/tech-verification/t0-resource-integration-20260910/occlusion-playmode-final.xml -logFile /Users/jangyoung/orca/unknown/_workspace/current/systems/tech-verification/t0-resource-integration-20260910/occlusion-playmode-final.log
rtk proxy /Applications/Unity/Hub/Editor/6000.5.6f1/Unity.app/Contents/MacOS/Unity -batchmode -nographics -quit -projectPath /tmp/t0-occlusion-20260910/build-project -executeMethod Tide.EditorTools.T0ProjectBuilder.BuildMac -logFile /Users/jangyoung/orca/unknown/_workspace/current/systems/tech-verification/t0-resource-integration-20260910/occlusion-build-mac.log
```

### Native artifact and scope

[OBSERVED] `unity/Unknown/Builds/T0-renderer-occlusion-20260910-diagnostic/Unknown.app` is a **diagnostic BuildMac artifact**. Its production runtime source is an exact byte prefix followed by the retained `occlusion-native-driver.cs.txt`; the driver exists only in the temporary clone and activates on `--t0-occlusion-smoke`. The clone's 249 production input hashes match the final workspace. Two captured `InitTestScene*` files belonged to the concurrently finishing Unity test runner; they were not production inputs or enabled build scenes and are explicitly identified in the manifest. The build log records boot/ui-root/hub as the built scenes. The bundle manifest retains 315 file hashes. The clone was removed after identity verification to recover disk space; pre-existing player bundles were preserved.

The native fixture creates only a transient GameObject with MeshFilter/MeshRenderer, reusing the existing `Workbench above drawer` shared Cube mesh and material. It confirms reference identity, zero Collider components, camera visibility, and no Physics hit before the drawer. On Metal/Apple M2 Pro, the blocker ray entry was 0.21967639 and drawer entry was 2.76798081. Real Input System mouse/keyboard events traverse the production interaction path. The saved screenshots show the drawer investigation after a visible click, the desk actions remaining after an occluded click, and selection restored after disabling the blocker. This is automated native evidence, not human testing.

The successful foreground launch command is retained exactly in `occlusion-verification.json`. It uses `open -n` on the retained app with an isolated save directory. The driver reports PASS and requests `Application.Quit(0)`; process absence was observed, but LaunchServices does not expose a directly measured player exit code. Earlier direct launches are retained: the first timed out before boot/interaction checks, and a background retry was terminated without a result. They are not behavioral test failures or successful smoke evidence.

[OBSERVED] Source/build/native hashes, current file inventory and preservation findings are in `occlusion-receipt-manifest.json`, `occlusion-changed-files.json`, and `occlusion-preservation.json`. r03's approval, source/import resource files and hub scene remain byte-identical to the task baseline. HEAD and index remain unchanged. The only unrelated baseline mismatch was the already-running root Editor's `Logs/Editor.log`, whose licensing log continued updating; it was not edited or restored by this task.

[OBSERVED] A five-file scoped graph refresh exited 0 (156 nodes, 344 edges, 10 communities); exact input hashes and output are retained as `occlusion-graph-receipt.json` and `occlusion-scoped-code-graph*`. Canonical repository/vault graphs were preserved. mex scope/check/log remain skipped because the identity probe found no mex-agent; no zvec index was created or rebuilt. Freshness checks cover metadata/supersedes structure only and do not establish G8.

Conventions checklist for this follow-up:

1. Touched artifacts have current frontmatter; this is a same-cycle in-place update, so `supersedes: null` remains appropriate.
2. Final `freshness-check.sh` exited 0: 0 findings across 290 Markdown artifacts (`occlusion-freshness.txt`).
3. Test/build/smoke measurements cite raw receipts and command timestamps; approximation limits are labeled separately.
4. Existing r03 approval is preserved; independent scoped QA found no P1/P2 issues. No new asset/design approval is asserted.
5. Scoped graph receipt and unified-vault rationale report are present; canonical graphs remain unchanged.
6. `mex check` was unavailable (identity probe failed); no no-drift or full G8 PASS is asserted.

[INFERENCE] World-space bounds are a conservative approximation and can overblock transparent, hollow, back-facing or empty parts of combined bounds. This change does not claim triangle/fragment/alpha-perfect picking, human playtests, performance/frame budgets, or production gate completion. No commit or push was performed.

## Committed resource audit

| Resource | Approval and committed integration | This cycle |
| --- | --- | --- |
| r03 drawer | `assets/generated/3d/hub-view-drawer-r03/provenance.json:16–17,37–50` and `production/decision-log.md:241–245` approve this exact T0 placement/material/mesh candidate. The committed `hub.unity` references `DrawerDiagnostic.prefab` at `(0,0.32,1.1)`. The source FBX and all four texture files are byte-identical to their Unity imports. | Static placement is already integrated. Direct pointer selection was absent; this is the selected increment. |
| Existing drawer investigation | `systems/interaction-rules.md:31–32` defines node clicks and target investigation. Committed `Data/Tables/zones.json:97–128` names `hub-view-drawer`, `drawer`, and `plate-zero`. `T0GameSession` already exposes the node through UI navigation and the plate inspection action. | Route the resource click to that existing interaction; preserve keyboard navigation. |
| r01 drawer and hub greybox | Imported candidates remain pending; `T0ProjectBuilder.Prepare` keeps the candidate root inactive. r01 provenance remains ineligible. | Not activated or changed. |
| Stamp audio | `assets/generated/audio/higgsfield-stamp-r01/provenance.json` remains `runtimeEligible:false`; the M2 report records pending listening approval and disabled playback. | Not activated or changed. |
| Approved commit feedback | `Resources/T0Vfx.json` is already bound to successful current commit receipts in `T0GameSession`. | No additional increment selected. |

[OBSERVED] Independent QA and code review supported nonanimated pointer selection. The mesh approval does not authorize physical drawer-opening motion: `modeling/pipeline.md:370,1332` leaves travel/timing unchosen, and the RFC explicitly excludes animation timing. Pre-existing untracked authored specification files were read without edits.

## Original integration validation (before the P1 review)

All receipt paths in the table are under `t0-resource-integration-20260910/` alongside this report.

| Check | Fresh result | Receipt |
| --- | --- | --- |
| Focused RED, tests authored before runtime changes | Exit 2; 0/3 passed, 3 failed at the missing drawer-investigation assertion. No compile/setup failure. | `focused-red.xml`, `focused-red.log` |
| First implementation attempt | Exit 2; 0/3 passed. Unity's missing-component wrapper was not handled by C# `??`; corrected to Unity `== null` semantics. | `focused-green.xml`, `focused-green.log` |
| Focused GREEN after correction | Exit 0; 3/3 passed, 0 skipped. | `focused-green-retry.xml`, `focused-green-retry.log` |
| Initial full suites before native correction | Exit 0; EditMode 19/19 and PlayMode 18/18 passed. These are superseded by the final runs below. | `editmode-full.xml/log`, `playmode-full.xml/log` |
| Initial BuildMac | Exit 0; Succeeded, 314,108,583 bytes. | `build-mac.log`, `build-inputs.json`, `player-build-manifest.json` |
| Initial native smoke / corrective RED | FAILED: both approved non-readable meshes emitted `CollisionMeshData couldn't be created because the mesh has been marked as non-accessible`. Editor passes did not establish player behavior. | `player-smoke.log`, `native-first-result.json`, `native-failed-runtime.cs.txt` |
| Focused GREEN after player correction | Exit 0; 4/4 behavioral tests passed, 0 skipped. Renderer bounds picking replaces runtime collider cooking. | `focused-green-player-fix.xml/log`, `player-fix-tdd.md` |
| Final full EditMode, no assembly/filter restriction | Exit 0; 19/19 passed, 0 failed/skipped. | `editmode-final.xml`, `editmode-final.log` |
| Final full PlayMode, no assembly/filter restriction | Exit 0; 19/19 passed, 0 failed/skipped. | `playmode-final.xml`, `playmode-final.log` |
| Final build input identity | APFS-cloned final Assets, Packages and ProjectSettings: 249 SHA-256 entries, 0 mismatches against the current project. | `build-inputs-final.json` |
| Final BuildMac | Exit 0; Succeeded, 314,109,143 bytes. 249 source hashes still match both clone and original after build. | `build-mac-final.log`, `player-build-manifest-final.json` |
| Final native interaction smoke | PASS; actual drawer click enters investigation, inspection text appears, start/hints guards hold, repeat click works after closing hints. Quit exit 0; no mesh-cooking or exception matches. | `player-smoke-final.log`, `native-final-result.json` |

The focused tests load the actual committed hub scene and inject MouseState/KeyboardState events. They check visible drawer selection, existing keyboard navigation, unchanged resource transforms/meshes/materials and puzzle state, and rejection on start screen, right-click, overlay/tool/document/UI, outside the viewport, behind nearer scene geometry, and for hidden/inactive/camera-excluded renderers or missed bounds. The raw failed attempts remain preserved. `focused-red-player-fix.xml/log` belongs to a superseded component-absence assertion; that assertion was removed, and its run is explicitly not counted as behavioral RED. The actual native failure above is the corrective RED.

Exact focused commands and execution order are in `focused-tdd.md` and `player-fix-tdd.md`. The final full-suite and build commands were:

```sh
rtk proxy /Applications/Unity/Hub/Editor/6000.5.6f1/Unity.app/Contents/MacOS/Unity -batchmode -nographics -projectPath /Users/jangyoung/orca/unknown/unity/Unknown -runTests -testPlatform EditMode -testResults /Users/jangyoung/orca/unknown/_workspace/current/systems/tech-verification/t0-resource-integration-20260910/editmode-final.xml -logFile /Users/jangyoung/orca/unknown/_workspace/current/systems/tech-verification/t0-resource-integration-20260910/editmode-final.log
rtk proxy /Applications/Unity/Hub/Editor/6000.5.6f1/Unity.app/Contents/MacOS/Unity -batchmode -nographics -projectPath /Users/jangyoung/orca/unknown/unity/Unknown -runTests -testPlatform PlayMode -testResults /Users/jangyoung/orca/unknown/_workspace/current/systems/tech-verification/t0-resource-integration-20260910/playmode-final.xml -logFile /Users/jangyoung/orca/unknown/_workspace/current/systems/tech-verification/t0-resource-integration-20260910/playmode-final.log
rtk proxy /Applications/Unity/Hub/Editor/6000.5.6f1/Unity.app/Contents/MacOS/Unity -batchmode -nographics -quit -projectPath /tmp/t0-resource-cycle-20260910/build-project-final -executeMethod Tide.EditorTools.T0ProjectBuilder.BuildMac -logFile /Users/jangyoung/orca/unknown/_workspace/current/systems/tech-verification/t0-resource-integration-20260910/build-mac-final.log
```

## Original integration native player smoke (before the P1 review)

[OBSERVED] Final artifact: `unity/Unknown/Builds/T0-resource-integration-20260910-final/Unknown.app`. The existing `BuildMac` implementation was invoked unchanged inside an APFS clone; its produced bundle was moved to this distinct output path. Original user preview and framing-smoke bundles were not overwritten. The final bundle has 315 file hashes in `player-build-manifest-final.json`.

```sh
rtk proxy '/Users/jangyoung/orca/unknown/unity/Unknown/Builds/T0-resource-integration-20260910-final/Unknown.app/Contents/MacOS/Unknown T0' --t0-save-dir /tmp/t0-resource-cycle-20260910/native-save-final -screen-fullscreen 0 -screen-width 1280 -screen-height 800 -logFile /Users/jangyoung/orca/unknown/_workspace/current/systems/tech-verification/t0-resource-integration-20260910/player-smoke-final.log
```

CUA operated this actual Metal player on Apple M2 Pro. The app window was 1280×800; CUA returned 640×432 captures including the title bar. No screen coordinate was inferred from a test injection: actions followed visible screenshots.

1. Clicking the visible drawer before Start left the start screen intact.
2. Start opened the existing workbench actions. Clicking the rendered drawer at CUA `(145,204)` moved the camera to the authored close view and exposed `서랍의 판 #0 조사`.
3. Clicking that existing action displayed the canonical plate-zero slot explanation.
4. F1 opened Hints. Clicking the visible drawer left Hints open.
5. Escape closed Hints. Existing workbench navigation returned to the desk, and another scene drawer click restored the investigation action.
6. Cmd+Q closed this test player with process exit 0. Final log scan found zero mesh-cooking/non-accessible/NullReference/MissingComponent/exception error matches.

Screenshots were observed inline in CUA; separate screenshot files were not retained. This is automated native interaction evidence, not human testing. The failed initial app was removed only after its hashes, exact source snapshot and native errors were retained, to free space for the corrected build. Only this cycle's temporary build clones were removed.

## Original integration file inventory (before the P1 review)

Runtime/test files:

- `unity/Unknown/Assets/_Project/App/T0GameSession.cs`
- `unity/Unknown/Assets/_Project/Tests/PlayMode/T0ResourceInteractionTests.cs`
- `unity/Unknown/Assets/_Project/Tests/PlayMode/T0ResourceInteractionTests.cs.meta`

Systems evidence consists of this report and the exact paths in `t0-resource-integration-20260910/changed-files.json`; the inventory includes every raw receipt, sidecar and manifest, and excludes pre-existing changes and ignored build products. The original integration inventory has 75 files: 3 runtime/test files, this report, and 71 evidence/sidecar/manifest files. `receipt-manifest.json` binds the evidence to hashes.

## Scoped graph and memory receipt

[OBSERVED] Final `graphify update /tmp/t0-resource-cycle-20260910/graph-scope` exited 0 and rebuilt 170 nodes, 356 edges and 12 communities from seven explicitly copied T0 source/test files. The corresponding `scoped-code-graph.json` and report are retained here. This refreshes the code evidence for this bounded change without modifying the canonical repository or vault graphs. Canonical graph synchronization remains outside the requested file scope. mex scope/check/log are skipped because the session-start identity probe found no mex-agent; TeX's `mex` was not used. No persistent zvec index was created or rebuilt, and no assistant-memory files were updated.

The graphify CLI also appended seven temporary-input cache entries to the repository's `graphify-out/manifest.json` despite the scoped target. The closing scope audit identified these exact entries, verified every other entry matched the originally clean HEAD version, and restored only this attributable side effect. The canonical graph files and manifest have no new changes from this cycle.

Final `bash .claude/skills/game-ops-harness/scripts/freshness-check.sh` exited 0: 0 findings across 245 Markdown artifacts. Its captured output is `freshness.txt`. This checks metadata/supersedes structure only; no cycle start was supplied and it does not establish G8. Scoped `git diff --check` also exited 0.

## Preservation and evidence limits

The baseline dirty/untracked file inventory and SHA-256 snapshot are at `/tmp/t0-resource-cycle-20260910/preexisting.json`; the index snapshot is `index-before.txt` alongside it. Final comparison verified all 1,465 pre-existing files byte-for-byte unchanged, unchanged index and HEAD, no newly changed resource/scene/approval paths, and no final build-input drift. The pre-existing decision-log diff is preserved, not attributed to this cycle. See `preservation.json`. No commit or push was performed.

The hit region is the visible renderer's world-space bounds, not triangle-exact picking; unusual silhouette-edge precision remains unmeasured. The original tests covered collidable occlusion only; independent review subsequently exposed the renderer-only P1 corrected in the follow-up below. Human playtests, physical gamepad hardware, performance/frame budgets, physical drawer-opening approval and G4/G5/G6/G8 completion remain unverified. Historical M2 totals and earlier runs above are kept separate from the final 19/19 EditMode and 19/19 PlayMode results; focused tests are subsets, not additional suite totals.
