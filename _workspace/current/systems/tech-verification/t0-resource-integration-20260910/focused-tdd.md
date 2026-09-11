---
updated: 2026-09-10
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: systems
---

# Focused resource interaction TDD

Selected increment: left-click the visible, already-approved T0-only r03 drawer in the committed hub scene to select the existing `hub-view-drawer` investigation. No physical drawer movement or new puzzle command.

Test authored first: `T0ResourceInteractionTests.cs` loads the committed hub scene and injects real Input System mouse/keyboard device events. The expected new assertion is the appearance of `inspect-plate-zero` after a left-click on the tray front surface. It also covers start screen, other mouse buttons, overlay/tool/document/UI/outside-screen suppression, scene occlusion, unchanged resource transforms/meshes/materials, and keyboard navigation.

## RED command (recorded before execution)

```sh
rtk proxy /Applications/Unity/Hub/Editor/6000.5.6f1/Unity.app/Contents/MacOS/Unity -batchmode -projectPath /Users/jangyoung/orca/unknown/unity/Unknown -runTests -testPlatform PlayMode -testFilter Tide.Tests.T0ResourceInteractionTests -testResults /Users/jangyoung/orca/unknown/_workspace/current/systems/tech-verification/t0-resource-integration-20260910/focused-red.xml -logFile /Users/jangyoung/orca/unknown/_workspace/current/systems/tech-verification/t0-resource-integration-20260910/focused-red.log
```

Runtime is unchanged at initial RED execution. Existing Unity process for repository root `/Users/jangyoung/orca/unknown` was left untouched; the target Unity project is the distinct `/Users/jangyoung/orca/unknown/unity/Unknown`.

RED result: Unity exited **2**; XML reports **3 total, 0 passed, 3 failed**. All three tests reached the intended positive navigation assertion and found the original desk actions (`handover`, `transfer`, `load-plate-zero`) instead of `inspect-plate-zero`. The initial negative input-boundary assertions passed. No compile or fixture failure. Receipts: `focused-red.xml`, `focused-red.log`.

Minimal implementation after RED: the session binds default MeshColliders to the two committed mesh filters under the exact `T0 approved r03 drawer` root in the loaded `hub` scene. A left-button press must be in the camera viewport, outside blocking UI, on the nearest scene collider belonging to that drawer, with no overlay/tool/document/start screen active. It invokes existing `GoNode("hub-view-drawer")`. No asset data, mesh, material, transform, approval, or puzzle command changes.

## GREEN command (recorded before execution)

```sh
rtk proxy /Applications/Unity/Hub/Editor/6000.5.6f1/Unity.app/Contents/MacOS/Unity -batchmode -projectPath /Users/jangyoung/orca/unknown/unity/Unknown -runTests -testPlatform PlayMode -testFilter Tide.Tests.T0ResourceInteractionTests -testResults /Users/jangyoung/orca/unknown/_workspace/current/systems/tech-verification/t0-resource-integration-20260910/focused-green.xml -logFile /Users/jangyoung/orca/unknown/_workspace/current/systems/tech-verification/t0-resource-integration-20260910/focused-green.log
```

First GREEN attempt exited **2**, with all three fixtures failing on `MissingComponentException`: Unity's destroyed/missing-object null semantics are incompatible with null-coalescing on `GetComponent<MeshCollider>()`. Replaced the null-coalescing check with Unity's `== null` check. Original failed attempt receipts are preserved as `focused-green.xml` and `focused-green.log`.

Retry command (recorded before execution):

```sh
rtk proxy /Applications/Unity/Hub/Editor/6000.5.6f1/Unity.app/Contents/MacOS/Unity -batchmode -projectPath /Users/jangyoung/orca/unknown/unity/Unknown -runTests -testPlatform PlayMode -testFilter Tide.Tests.T0ResourceInteractionTests -testResults /Users/jangyoung/orca/unknown/_workspace/current/systems/tech-verification/t0-resource-integration-20260910/focused-green-retry.xml -logFile /Users/jangyoung/orca/unknown/_workspace/current/systems/tech-verification/t0-resource-integration-20260910/focused-green-retry.log
```

Final focused GREEN: Unity exited **0**; `focused-green-retry.xml` reports **3 total, 3 passed, 0 failed, 0 skipped**. All tests use the committed scene and real Input System device events. This proves automated PlayMode resource interaction and the specified guards, not human testing, native-player behavior, or performance gates. Full suites, BuildMac, native interaction smoke, graph/memory receipts, and final baseline verification are parent-owned follow-up evidence.
