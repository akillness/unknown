---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-production-director
---

# Task Manifest

## T0 M2 continuation (2026-09-10)

See `production/codex-t0-m2-status.md` for current scope and evidence.

| task | owner | phase | artifact | gate | status | beat |
|---|---|---|---|---|---|---|
| T0 M2 save, undo, input, UI and playable scene | systems, QA | P3/P4 | unity/Unknown/, systems/tech-verification/t0-m2-native.md, systems/tech-verification/t0-m2-player-smoke.md | T0 technical checks | verified: EditMode 18/18, PlayMode 15/15, macOS build success; pointer T0 completion and OS restart; corrected-player visual smoke PASS | t0-b1/b2/b3 |
| Citation and circuit authored source packets | worldview, synopsis, planner, QA | P2/P4 | synopsis/t0-records.md, planning/t0-circuit-overlay.json | scoped source QA | scoped source ACK approved and runtime integration verified; campaign gates remain separate | t0-b2/b3 |
| Blender r03 drawer and Higgsfield stamp candidate | modeling, director, QA | P3 | assets/generated/3d/hub-view-drawer-r03/, assets/generated/audio/higgsfield-stamp-r01/ | asset-specific import/inspection | r03 approved for T0 and visible in native player; audio listening review pending, runtime playback disabled | t0-b1 |

## C1 M3 continuation (2026-09-11)

See `production/codex-c1-m3-status.md` and RFC-CX-004 for bounded c1-b1 implementation.

| task | owner | phase | artifact | status |
|---|---|---|---|---|
| C1 first-beat authored contract | planner | P2 | planning/c1-patrol-contract.json | current; static review passed |
| C1 entry, circuit preview, atomic chapter save and v1 migration | systems, QA | P3/P4 | unity/Unknown/ | complete: EditMode 27/27, PlayMode 28/28, native restart verified |
| Original patrol panel | modeler, director | P3 | assets/generated/3d/c1-patrol-panel-r01 | complete: generated, native-inspected and scoped runtime-approved (RFC-CX-004) |

## C1 M4 continuation (2026-09-11)

See RFC-CX-005 and production/codex-c1-m4-status.md.

| task | owner | phase | artifact | status |
|---|---|---|---|---|
| Signature reader authored packet | planner | P2 | planning/c1-signature-contract.json | complete — scoped evidence recorded |
| State-driven paper/reader art and reveal | presentation, modeling, director | P2/P3 | presentation/c1-signature-presentation.md | complete — scoped evidence recorded |
| c1-b2 runtime, safe saves and native verification | systems, QA | P3/P4 | unity/Unknown/ | complete — scoped evidence recorded |

## M5 intro and gameplay direction continuation — 2026-09-11

| Slice | Owner | Status | Acceptance |
|---|---|---|---|
| GTI stills + Higgsfield intro/C1 previz | production + presentation | complete | source/clip provenance, no premature disclosures, reviewed timestamps |
| Video-derived native intro and C1 orientation | systems | complete | skip/settings/reduced-motion, state-only direction, save/input invariance |
| Isolated build, native capture and QA | QA + production | complete | new source tests, previous-save restart, actual window-only recording |

Baseline `6514f549ac7b2e71846005c75a4ff9227882c47c`; scope and chronology in `production/intro-gameplay-m5-status.md`. Existing source/frame material remains preserved.

[OBSERVED] Final ordinary build integrates upstream2ece517 with runtimeApproved:true. EditMode36/36, PlayMode54/54, serialized boot1/1 passed with no diagnostic override. App fingerprint484ab029f0bf29b4af6ffdd2bb9587ca6e08a432e09dde979d3635517b84d736 (316files;353421261bytes). Fresh intro naturally completed without creating gameplay save; C1 recording reached one accepted confirmation (54commands); completed v3 restart preserved57commands and save bytes. Native films are window-only recordings, separately labelled from generated previz.
