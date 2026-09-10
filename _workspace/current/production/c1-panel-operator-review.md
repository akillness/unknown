---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-production-director
---

# C1 panel — Blender operator inspection

[OBSERVED] Root executed the modeler recipe with Blender 5.1.2 in a factory-startup scene. The failed first material-join attempt is preserved under assets/generated/3d/c1-patrol-panel-r01-attempt1. The corrected invocation used --python-exit-code 1; SUCCESS.json and the terminal success marker were present.

[OBSERVED] Successful output: 6 static material-group meshes, 2,092 triangles, four 1024 by 1024 maps totaling 1,615,402 bytes. All 10 output hashes, recipe hash and SUCCESS provenance hash match. Files: assets/generated/3d/c1-patrol-panel-r01/.

[OBSERVED] Preview inspection shows distinguishable round lighting housing and rectangular readout, connected to a shared central supply, with weathered maritime blue-grey surfaces and no baked lettering or numeric pressure values. The static prop does not animate live meter/light state.

Decision: approved for native diagnostic import under RFC-CX-004; runtime eligibility remains false pending native Unity material/framing inspection. Full campaign G4/G5 and GPU performance remain unmeasured.

### Native panel promotion — 2026-09-11

[OBSERVED] Director inspected diagnostic #3 front and alternate PNGs in systems/tech-verification/c1-m3/panel. The adapter is upright, retains the six meshes and texture maps, shows the shared connection clearly, and separates the warm lamp lens from the readout recess. Approve this exact static c1-b1 panel/adapter for runtime use. Blank plaques and readout are decorative; the UI remains authoritative for switch/readout state. Full G4/G5 and performance remain unmeasured. Original generation provenance and SUCCESS pair are preserved under assets/generated/3d/c1-patrol-panel-r01/generation-receipt/. The live provenance records capture hashes and this scoped promotion.
