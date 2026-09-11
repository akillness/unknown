---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
---
# C1 M4 delivery verification

[OBSERVED] RFC-CX-005 implements only `c1-b2` after shipped `c1-b1`. Save schema v3 preserves original v1/v2 bytes and old replay identities. The runtime/planner signature packets are byte-identical: SHA256 `9bff17b3118e8c51388fca96f4ec94e6451b388d816ec504cd1737166d651515`.

## Exact delivery source

[OBSERVED] Delivery is an isolated projection at `/tmp/unknown-c1-m4-publish-20260911/unity/Unknown`: exported HEAD `ae098e0636b0075d1abb0ebf408bd66f7f459811` plus the 65 approved Unity paths in `c1-m4/delivery-source-manifest.json`. Each path records SHA256, bytes and staging source. Two blobs are substituted only in isolation: `C1GameSession.cs` retains the original packet-first `PatrolText` fallback; `C1PlayModeTests.cs` removes the concurrent `ObservedInvalidRoutingExplainsContradictionWithoutPrescribingAction` test while retaining the director-approved pre-existing default-guidance regression. `T0Strings.json` stays at HEAD. Shared worktree edits are preserved.

[OBSERVED] The earlier shared-worktree `playmode-11.xml` passed 40 cases with concurrent external guidance code/data/test; it is mixed-source evidence, not delivery validation. `source-freeze-start.json` is likewise a mixed-worktree drift snapshot, not a publish manifest. The isolated 36/39/1 runs below validate the delivery projection.

## Isolated commands and results

Executable: `/Applications/Unity/Hub/Editor/6000.5.6f1/Unity.app/Contents/MacOS/Unity`. `<project>` is the isolated project above. `<receipts>` is `/Users/jangyoung/orca/unknown/_workspace/current/systems/tech-verification/c1-m4/isolated`. Every command routes through `rtk run`.

| Command arguments | Receipt under `c1-m4/isolated/` | Observed result |
|---|---|---|
| `-batchmode -nographics -projectPath <project> -runTests -testPlatform EditMode -assemblyNames Tide.Tests.Sim -testResults <receipts>/results/editmode.xml -logFile <receipts>/logs/editmode.log` | `results/editmode.xml` | 36/36 pass, 0 failed/skipped |
| `-batchmode -nographics -projectPath <project> -runTests -testPlatform PlayMode -testFilter "Tide.Tests.C1PlayModeTests;Tide.Tests.C1SignaturePlayModeTests;Tide.Tests.T0CaseThreadTests;Tide.Tests.T0PlayModeTests;Tide.Tests.T0ResourceInteractionTests" -testResults <receipts>/results/playmode.xml -logFile <receipts>/logs/playmode.log` | `results/playmode.xml` | 39/39 pass, 0 failed/skipped |
| `-batchmode -nographics -projectPath <project> -runTests -testPlatform PlayMode -testFilter Tide.Tests.T0BootSceneTests.SerializedBootLoadsHubAndInitializesVisibleStartScreen --t0-save-dir /tmp/unknown-c1-m4-boot-isolated-delivery-20260911 -testResults <receipts>/results/boot-playmode.xml -logFile <receipts>/logs/boot-playmode.log` | `results/boot-playmode.xml` | 1/1 pass; actual serialized boot → ui-root → hub and visible Interface Canvas start control |
| `-batchmode -quit -projectPath <project> -executeMethod Tide.EditorTools.C1SignatureProjectBuilder.BuildMac -logFile <receipts>/logs/build-final.log` | `logs/build-final.log` | Succeeded; 348,163,868 bytes; canonical copy verified across all 316 bundle files |

[OBSERVED] Regular PlayMode classes run without the global `--t0-save-dir`, which overrides explicit per-test paths. The serialized boot test runs separately with an exact filter and a fresh owned isolation directory.

## Established behavior

[OBSERVED] Tests cover prior v2 migration, exact backup bytes, unsupported identities, copied-source roots, reset and atomic undo/redo/replay. Public session/UI tests establish explicit observation/selection/trial, reversible risk, safe separation, separate per-sheet copy actions, permanent lower-region opacity, explicit plate-zero comparison and independent original log/plate proof. Immediate confirmation rejects. Candidate-save failure after a successful pre-commit checkpoint preserves live hash and exact previous disk bytes; retry publishes one receipt. Pending Undo cancels all commit effects. Duplicate confirmation rejects. Reload restores the accepted c1-b2 endpoint and its contextual status rather than the old T0 welcome.

[OBSERVED] UI regressions exercise 150% scale, header/footer/mask clipping bounds, per-sheet labels, scroll reset on signature surface transitions, ordinary and same-frame InputSystem clicks, pointer-selected keyboard focus without rerender/scroll, and persistent selected humidity/actual trial outcome in the upper case card while lower controls are focused. No safe-answer guidance appears before an executed trial.

## Native art and route evidence

[OBSERVED] Reader r02 corrects the r01 swing-arm rotation and has 4 meshes / 3,920 triangles. Final Unity native capture `c1-m4/reader/reader-front.png` SHA256 is `e22f401da9e2a3751448056d23ad66eb31ec83959dc520be16435634da769c45`. The blank 1024² Higgsfield substrate has no generated clue text. Both resources received director native review; the final view flag enables normal runtime use. First-frame texture warm-up, framing iterations and the rejected r01 capture remain preserved in the earlier reader receipt directories; unused Unity r01 candidates are excluded.

[OBSERVED] `c1-m4/player-final-smoke/native-observation.json` records the full diagnostic-3 route at 150% text/reduced motion from the actual prior v2 save: risky trial, reset, safe trial, separation, both explicit copies, unresolved region, comparison, evidence choice and accepted confirmation. All 42 old command entries remain the exact prefix of 57 entries; `.v2.bak` is byte-identical, exactly one `ConfirmSignature` is stored, and `cp-c1-b2` appears only after confirmation. Mask and copy presentation persist through completion.

[OBSERVED] `c1-m4/feedback-smoke/native-observation.json` records an ordinary-runtime spot check: one nonfocused click opened plate-zero, Return recorded the selected document, and selected humidity plus executed risk result remained visible in the upper case card at 150% while lower controls were focused. Earlier intermittent CUA click observations remain in the diagnostic record; this is one successful native input route, not a universal hardware-reliability claim. The first ordinary-runtime completed-save restart restored approved resources/copies/mask/completion; the final status-only fix replaces a stale T0 welcome with the restored C1 record message.

[OBSERVED] `c1-m4/player-final-smoke/final-runtime-verification.json` closes the final isolated-build restart. The exact copied ordinary-runtime bundle (no diagnostic flag), at 150% text/reduced motion, restored completed c1-b2, both copied papers, approved resources and the retained lower mask. The status reads `저장된 대조 기록을 복원했습니다.` with no stale T0 line. Confirmed save bytes are unchanged and exactly one `ConfirmSignature` remains. This is agent-operated native smoke, not human-playtest or device/performance certification.

## Import and failed-run accounting

[OBSERVED] Fresh Unity import cleared only `UniversalRenderPipelineGlobalSettings.asset`'s generated `m_RuntimeSettings.m_List`. Package `RenderPipelineGraphicsSettingsContainer` uses the unchanged `m_SettingsList` in Editor, clears runtime settings after deserialization, and repopulates them through stripping when building a player. HEAD source was restored after first import and remained byte-identical through the isolated PlayMode run. Full diff and package source hash: `c1-m4/isolated/urp-after-import.diff`, `urp-runtime-settings-observation.json`. The asset also remained byte-identical to HEAD after boot and the final build. All 65 delivery source hashes were unchanged; no additional renderer-settings overlay was required. `c1-m4/isolated/build-copy-receipt.json` records the verified canonical app copy and bundle fingerprint.

[OBSERVED] Earlier failed receipts remain preserved: `playmode-1` test compile error from a private-property reference; `playmode-4` invalid whole-suite invocation with a global save override; `boot-playmode-1` an overly broad single-Canvas assertion; `playmode-7` the missing ResetScroll consumer later fixed. `playmode-10.log` and `build-final-2.log` are shared-project lock failures; that build attempt produced no app. The first direct-binary diagnostic launch stayed black without app logs, while LaunchServices subsequently booted all stages; no proven cause is assigned to that first launch.

## Graph and limits

[OBSERVED] Retrieval used `zg query "C1 patrol simulation save continuation" --limit 3 --preview short --refresh off` and graphify flow queries without rebuilding the zg index. `graphify update .` in the isolated snapshot completed: 68,880 nodes / 98,518 edges / 5,467 communities, including imported PackageCache code. Receipt: `c1-m4/isolated/logs/graphify-update.log`. Its code graph/GRAPH_REPORT were generated; HTML was skipped at the configured 5,000-node cap. Earlier shared-worktree graph receipts remain separate.

[UNGRAPHED] mex-agent is unavailable; the host's TeX mex is deliberately not invoked. No mex graph/check/log success is claimed.

[INFERENCE] No GPU/CPU frame-time, fun, comprehension, physical input-device validation, chapter duration or G4/G5/G6 passage follows from these tests. Audio remains disabled; MuAPI generation is not claimed. Reader housing is static; salt-edge state and the permanent lower mask are UI layers without physical paper, humidity or camera animation.
