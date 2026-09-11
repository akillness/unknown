---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
---

# M5 systems verification

Status: systems implementation and final ordinary build PASS. RFC-CX-007 approved prototype resources; upstream2ece517 and M5 passed36 EditMode /54 PlayMode /1 serialized boot without diagnostic override. Director owns final native recording and publication.

## Scope and provenance

- Isolated worktree: /tmp/unknown-m5-video-runtime-20260911, baseline6514f54.
- Unity project: unity/Unknown; editor6000.5.6f1.
- Exact25 owned files and SHA-256: m5-direction/ownership-source-manifest.json.
- Shared-workspace Unity was not edited. Exact committed C1GameSession.cs, C1PlayModeTests.cs and T0Strings.json blobs from upstream2ece517 were copied only into the isolated project for combined verification; they are excluded from the25-file owned overlay and must not overwrite main. git diff --exit-code2ece517 for those3 paths returned0. No staging, commit or push was performed by systems.
- System contract: ../system-specs/intro-gameplay-m5.md.
- Resource approval is director-owned. RFC-CX-007 approved internal-prototype use, commercial eligibilityfalse; serialized M5Direction.runtimeApproved=true. Receipt: m5-direction/runtime-profile-approval.json. Earlier diagnostic runs used --m5-direction-diagnostic with false profile.

## Final integrated verification

Exact commands, XML hashes and observed counts: m5-direction/final-validation.json. Final source closure: m5-direction/source-closure.json; exactly8 modified +17 new owned files against upstream2ece517. The3 committed upstream files match2ece517 byte-for-byte and are not in the owned overlay.

| Current evidence | Observed result |
|---|---|
| editmode-integrated-final.xml |36/36 passed,0 failed/skipped |
| playmode-integrated-final.xml |54/54 passed,0 failed/skipped;40 baseline/upstream +14 M5 |
| boot-integrated-final.xml |1/1 passed; serialized boot with restored v2 fixture |
| logs/build-integrated-final.log |M5_MAC_BUILD Succeeded bytes=353421261 |
| final-app-inventory.json |316 files;353421261 bytes; fingerprint484ab029f0bf29b4af6ffdd2bb9587ca6e08a432e09dde979d3635517b84d736 |

Ordinary app path remains /tmp/unknown-m5-video-runtime-20260911/unity/Unknown/Builds/M5-mac/Unknown.app. No diagnostic flag is needed or used in final tests. Approved profile SHA8e99a764023d958b3844a846832bbde556c5ad94618893039fe23b6c61a0c97b. Shared HEAD was rechecked as2ece517 immediately before the final build. Earlier editmode-final.xml / playmode-final.xml / boot-final.xml passed on the pre-integration baseline and are explicitly superseded by the integrated results above.

## Diagnostic commands and results

Commands ran against the isolated project. Logs and XML are retained under m5-direction/.

| Command suffix after Unity -batchmode -projectPath <isolated-project> | Observed result | Evidence |
|---|---|---|
| -quit -executeMethod Tide.EditorTools.M5DirectionProjectBuilder.ImportResources --m5-source-root /Users/jangyoung/orca/unknown --m5-evidence-dir <m5-direction> | PASS; intro1672x941, surface1024x1024; original PNG bytes retained; approvalfalse | logs/import-2.log; import-audit.json |
| -runTests -testPlatform PlayMode -testFilter Tide.Tests.M5DirectionPlayModeTests --m5-direction-diagnostic | PASS13/13 before additional mouse test | playmode-m5-diagnostic-3.xml |
| -runTests -testPlatform PlayMode -testFilter Tide.Tests.M5DirectionPlayModeTests.PointerSettingsFromReduced150IntroOpensAndReturns --m5-direction-diagnostic | PASS1/1 actual mouse-state press/release, no pre-focus | pointer-settings-diagnostic.xml |
| -nographics -runTests -testPlatform PlayMode -testFilter "Tide.Tests.C1PlayModeTests;Tide.Tests.C1SignaturePlayModeTests;Tide.Tests.T0CaseThreadTests;Tide.Tests.T0PlayModeTests;Tide.Tests.T0ResourceInteractionTests;Tide.Tests.M5DirectionPlayModeTests" --m5-direction-diagnostic | PASS53/53,0 failed/skipped,33.151222s | playmode-full-diagnostic-2.xml |
| -nographics -runTests -testPlatform EditMode -assemblyNames Tide.Tests.Sim --m5-direction-diagnostic | PASS36/36,0 failed/skipped,2.2637054s | editmode-diagnostic.xml |
| -nographics -runTests -testPlatform PlayMode -testFilter Tide.Tests.T0BootSceneTests.SerializedBootLoadsHubAndInitializesVisibleStartScreen --m5-direction-diagnostic --t0-save-dir /tmp/unknown-c1-m4-boot-m5-diagnostic-20260911 | PASS1/1, serialized boot/ui-root/hub and restored v2 fixture | boot-diagnostic-2.xml |
| -quit -executeMethod Tide.EditorTools.M5DirectionProjectBuilder.BuildMac | PASS; diagnostic app with false profile | logs/build-diagnostic-1.log; diagnostic-app-inventory.json |

Full diagnostic PlayMode also received -screen-width1280 -screen-height720; actual Editor test surface remained640x480, so those flags do not establish native target resolution. Logged150% reduced-motion caption height410.8944 <= viewport523.2525, and skip-button bounds passed at that actual geometry. Director native1280x720 review is separate under intro-gameplay-m5/native-fresh/.

## Behavioral coverage

Fresh-only opening, measured3.125s caption cut /6s total, first-frame skip, Escape, settings clock pause, focus loss/application pause, reduced motion immediate continue, all guide labels and150% fit, missing-image ink fallback, actual mouse settings/back/skip, loaded-save bypass, explicit replay byte/snapshot/context preservation, old/new recovery-slot eligibility, reader replay keyboard navigation, held Enter across both caption cut and timeout (release0 renders; next fresh press works), actual selected category without rerender, risk trial after first observation, opaque lower mask, pending/cancelled/failed/accepted record status.

Earlier failed runs are retained honestly: initial compile needed the existing Tide.Presentation asmdef reference; targeted tests exposed missing title/intro-settings Back (fixed), plus a wrong test mask-object name (corrected). First full diagnostic run found three old immediate-start test assumptions and one exact normalized-float equality. Helpers now send a separate legitimate Skip input, and viewport-top equality uses1e-5 tolerance. Final diagnostic baseline passes all53 tests. The first boot command used a new temporary prefix rejected by its existing safety assertion; rerunning with the required /tmp/unknown-c1-m4-boot- prefix passed without source change. git diff --check returned0.

## Diagnostic artifact identity

App: /tmp/unknown-m5-video-runtime-20260911/unity/Unknown/Builds/M5-mac/Unknown.app.

316 files,353421021 bytes. Fingerprint18066f11cd0a556758916d9c654e7b293d44cfc7b26cb0e683783090ca92e629. This is an inventory-hash identity, not a gameplay/performance claim. False-profile SHA5d138fdeaf39380ac5061c61234239e22886653541957fd1d2fa7fc8ec51c6ac. No app copy was made for the receipt.

## Graph and memory

[OBSERVED] graphify update . ran at the isolated worktree and returned0:10397 nodes,11096 edges,1261 communities; graph.json and GRAPH_REPORT.md updated. Log: m5-direction/logs/graphify-update-isolated.log. Final graphify update . also completed0 after source freeze/integration:10403 nodes,11110 edges,1260 communities. Final log: m5-direction/logs/graphify-update-final.log.

[UNGRAPHED] mex-agent is unavailable; the host mex resolves to TeX, not the required memory tool. mex graph/mex log/mex check cannot be claimed. Director notified. A read-only inspection of the existing mex-agent-bin.sh resolver found all5 known executable paths absent and MEX_AGENT_BIN unset, so the fallback remains [UNGRAPHED]. No installs, unrelated-file edits or Codex memory edits were performed.

## Evidence limits

Generated clips are measured previz and are not native gameplay. Import/static/runtime tests do not establish human G4 or performance G6. Native diagnostic promotion is director-approved, final default-profile suites and ordinary app identity are verified above. Final ordinary-app native recording and release publication are director-owned. Human G4 and measured performance remain unclaimed.
