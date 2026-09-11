---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-production-director
---

# C1 M4 — reader and paper operator review

[OBSERVED] Root executed the modeler-authored recipe through Blender CLI 5.1.2 in a separate factory-startup background process with --python-exit-code 1. The user’s open Blender GUI scene was not touched. Exit 0, SUCCESS.json and C1_SIGNATURE_READER_GENERATION_COMPLETE were present. Operator log: assets/generated/3d/c1-signature-reader-r01/operator.log.

[OBSERVED] Source measurements: 3,920 triangles, 4 static meshes, 4 materials, 0.700 × 0.390 × 0.240 m, four shared 1024² maps totaling 1,732,640 bytes. GLB accessor counts and FBX fresh-scene round trip agree on triangles; metre bounds agree within the recipe tolerance. Root verified all ten declared output hashes/bytes, recipe hash and SUCCESS-to-provenance hash. These are asset measurements, not native performance results.

[OBSERVED] Product-render inspection shows the round plate cradle, short magnifier arm/crank and separate rectangular treatment tray with blank recesses and no clue lettering. Preview lighting washes the surface toward pale cyan. Source base-color maps remain muted/dark; native material and lighting must preserve the approved maritime palette. Diagnostic import is approved under RFC-CX-005; normal runtime promotion requires native captures.

The original generation metadata uses glossary_id=reader. Canonical device name is 판독기; reader is the related action 판독. Reviewed metadata will clarify glossary_name=판독기 and tool_action_id=reader while preserving the original generation receipt.

[OBSERVED] Higgsfield generated the blank paper substrate with gpt_image_2_5, job 54b52722-337a-42fc-baa4-e289c64db09e. Requested and delivered 1024², 1,752,951 bytes; quoted and observed charge 3.5 credits. Full-resolution inspection found quiet fibres/damp mottling, no text, signature, clue, adhesion salt or lower obstruction. The UI must author adhesion and the permanent lower mask as separate state-driven layers. Official output-use terms inspection is filed in assets/generated/2d/texture/c1-signature-paper-r01/rights-review.md. No MuAPI output is claimed.

[OBSERVED] Native r02 material/framing inspection, standalone continuation, and final isolated ordinary-runtime restart passed. See `../systems/tech-verification/c1-m4/player-final-smoke/final-runtime-verification.json` for the final restart receipt. [NOT-MEASURED] Human playtesting, physical controller use, sound listening and performance remain outside these receipts.

## r02 structural correction

[OBSERVED] Independent native diagnostic-3 review found the swing arm disconnected from the rear post/lens neck. Its authored Z rotation used the wrong sign. The modeler produced a separate r02 recipe changing only the output revision/path and rotation −0.47 to +0.47; r01 source/receipts remain unchanged. Root executed r02 in a fresh background Blender process (exit 0 plus SUCCESS marker), verified all ten outputs and receipt/recipe hashes, and inspected the continuous connection in the resulting preview. Counts and bounds remain within the same budgets. r01 is retained as rejected diagnostic evidence; only r02 is a candidate for native promotion.

[OBSERVED] r02 native capture inspected and approved by director under RFC-CX-005: continuous arm/post/lens neck connection, dark weathered materials, clear elevated source-front framing. Original generation provenance/SUCCESS are frozen under generation-receipt/. Live r02 provenance records static housing approval; paper UI approval remains separate.

## Native paper and scene inspection

[OBSERVED] Director completed the diagnostic-3 c1-b2 route through native CUA at requested 1280×800, Korean text150% and reduced motion. Both source descriptions begin at the top after entry; fixed document geometry keeps the lower mask and all copy labels within the viewport. Separation removes edge adhesion while the second-sheet mask persists through copies, mark, comparison and completion. Higgsfield fibres stay quiet and carry no baked evidence. Paper promotion is recorded under RFC-CX-005 and its frozen generation receipt is preserved. Detailed observations and limits: systems/tech-verification/c1-m4/player-final-smoke/native-observation.json.

The first direct-executable diagnostic launch was blank and left its v2 save untouched. The later bundle-launched instrumented build ran successfully. These observations do not establish a general direct-launch failure or a scene-loading defect. Early150% clipping and introductory scroll defects were reproduced, fixed and rechecked natively. Intermittent CUA first clicks and the later pointer/keyboard handoff fix are recorded separately from physical input testing.
