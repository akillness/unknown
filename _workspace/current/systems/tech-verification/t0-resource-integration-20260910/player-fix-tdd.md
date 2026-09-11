---
updated: 2026-09-10
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: systems
---

# Native-player resource picking correction

Parent native smoke observed `CollisionMeshData couldn't be created because the mesh has been marked as non-accessible` for the approved tray and casing. The original passing Editor PlayMode tests did not prove player support for runtime collider cooking. Parent retains original native-failure receipts.

Behavioral regression authored before the correction: assert the fixture's committed r03 meshes remain non-readable; then exercise disabled/inactive/camera-excluded renderers, a scene-window ray missing both bounds, and positive selection after restoration. Existing three committed-scene mouse tests remain unchanged. The native cooking failure is the actual RED evidence for this correction; Editor PlayMode cannot reproduce its CPU-mesh access boundary.

An initial test revision also asserted absence of MeshColliders. Parent review rejected that as an implementation-mirroring assertion; it was removed before the correction. That already-running revision exited 2 (3 passed, 1 failed); its `focused-red-player-fix.xml/log` receipts are retained as superseded evidence only, not claimed as behavioral RED.

Superseded test-revision command, recorded before execution:

```sh
rtk proxy /Applications/Unity/Hub/Editor/6000.5.6f1/Unity.app/Contents/MacOS/Unity -batchmode -projectPath /Users/jangyoung/orca/unknown/unity/Unknown -runTests -testPlatform PlayMode -testFilter Tide.Tests.T0ResourceInteractionTests -testResults /Users/jangyoung/orca/unknown/_workspace/current/systems/tech-verification/t0-resource-integration-20260910/focused-red-player-fix.xml -logFile /Users/jangyoung/orca/unknown/_workspace/current/systems/tech-verification/t0-resource-integration-20260910/focused-red-player-fix.log
```

Correction: retain only enabled, active, camera-visible renderers beneath the exact approved hub root; intersect each renderer's bounds and reject any closer scene Physics collider. No runtime collider creation or mesh-data access. Bounds are an approximate target volume, not triangle-exact picking; parent native smoke must validate the visible target.

GREEN command recorded before execution:

```sh
rtk proxy /Applications/Unity/Hub/Editor/6000.5.6f1/Unity.app/Contents/MacOS/Unity -batchmode -projectPath /Users/jangyoung/orca/unknown/unity/Unknown -runTests -testPlatform PlayMode -testFilter Tide.Tests.T0ResourceInteractionTests -testResults /Users/jangyoung/orca/unknown/_workspace/current/systems/tech-verification/t0-resource-integration-20260910/focused-green-player-fix.xml -logFile /Users/jangyoung/orca/unknown/_workspace/current/systems/tech-verification/t0-resource-integration-20260910/focused-green-player-fix.log
```

GREEN result: Unity exited **0**; XML records **4 total, 4 passed, 0 failed, 0 skipped**, 2026-09-10 11:03:54Z–11:03:56Z. Focused runtime/test `git diff --check` exited 0. Target Unity process has exited. Full suites, rebuilt native player, and repeated native interaction smoke remain parent-owned required evidence; passing Editor tests do not resolve the player-only failure by themselves.
