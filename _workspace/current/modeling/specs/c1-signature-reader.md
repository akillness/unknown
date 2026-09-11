---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-modeler
---

# C1 M4 판독기·습도판 받침 — r02

- Glossary device name: 판독기 / Plate Reader. The related action id reader means 판독; it is not the device name. The shallow humidity tray is an accessory working surface of the existing reader, not a new canonical tool.
- concept_ref: ../../concept/c1-signature-art-brief.md; existing ../../concept/prompts/tool-reader-hero.txt and ../../../../assets/generated/2d/concept/tool-reader-hero.png.
- presentation_ref: ../../presentation/c1-signature-presentation.md.
- decision: RFC-CX-005. Scope is c1-b2 only.

## Authoring and execution boundary

[OBSERVED] The modeler inspected the existing reader concept image. Its low circular cradle, rear magnifier arm and worn maritime metal inform this original geometry. The authored model omits the reference image's numeric gauge, writing, paper evidence and animated mechanisms.

[OBSERVED] Blender MCP is not callable in this modeling lane. This lane supplies the recipe and spec only. Root executes the isolated Blender CLI, checks output receipts and performs product-render review. No geometry, output byte count or native result is claimed before that execution.

Recipe: assets/generated/3d/scripts/build_c1_signature_reader_r02.py.

Operator invocation from repository root:

~~~sh
rtk run '/Applications/Blender.app/Contents/MacOS/Blender --background --factory-startup --python-exit-code 1 --python assets/generated/3d/scripts/build_c1_signature_reader_r02.py > /tmp/unknown-c1-signature-reader-r02.log 2>&1'
~~~

A fresh unsaved background process is mandatory. The recipe inspects the factory objects before any additions, hides them without deleting them, and refuses to overwrite an existing output directory. A failed output directory must be preserved under a separate attempt directory before a new attempt. Exit 0 alone is insufficient: require SUCCESS.json and the terminal C1_SIGNATURE_READER_GENERATION_COMPLETE marker.

## Delivery targets

| Property | [TARGET] | Execution evidence |
|---|---|---|
| Bounds | at most 0.72 m X × 0.42 m Y × 0.28 m Z | measurements.json bounds_m |
| Pivot | base bottom centre; front Blender -Y, up +Z; metres | bounds min Z and exported transforms |
| Geometry | at most 6,000 triangles, 8 static meshes | source and export measurements |
| Material groups | at most 4 | Casting / Patina / Recess / Salt |
| Textures | four shared 1024² maps at most | two base-color/roughness pairs; measured bytes |
| Rig | none; static short crank, arm and lens | no bones/animation exported |
| LOD | LOD0 only at this bounded prototype scope | no unmeasured scene-performance claim |
| Collision | none authored | visual-only import, no collider logic |
| Preview | 1024² PNG product render | not native gameplay evidence |

Materials keep blue-grey #36565C, dark #0E1F26/#173238, patina #4F7A6B and restrained matte salt #C8D6D3. The opaque matte lens avoids transparent-sort concerns. Accent, text, paper pages, source relationships, humidity, separation and occlusion state belong to UI; no visual mesh state claims puzzle truth.

## Files and exact import names

Current output directory: assets/generated/3d/c1-signature-reader-r02/. Original r01 directory remains immutable.

- SM_C1_Signature_Reader.blend — canonical authored scene, with separate preview and FBX verification scene clearly named.
- SM_C1_Signature_Reader.glb — canonical mesh/material delivery, index-accessor triangle count measured.
- SM_C1_Signature_Reader.fbx — Unity adapter using M3 axis/unit/export convention. Blender fresh-scene reimport measures triangle count and metre bounds; Unity remains a separate validation step.
- ROOT_C1_Signature_Reader — root at bottom centre.
- SM_C1_Signature_Reader_Casting / _Patina / _Recess / _Salt — material-group meshes, resolved before joining to avoid invalid StructRNA access.
- MAT_C1_Reader_Casting / _Patina / _Recess / _Salt — original material names.
- textures/MAT_C1_Reader_Casting_BaseColor.png, _Roughness.png; textures/MAT_C1_Reader_Patina_BaseColor.png, _Roughness.png.
- preview.png, prompt.txt, measurements.json, provenance.json, SUCCESS.json.

The source blend retains a PREVIEW_ONLY stage and VERIFY_ONLY_FBX_ADAPTER scene as authoring/evidence helpers. They are excluded from GLB/FBX exports. Import the explicit GLB or FBX, not the source blend's whole scene.

## Acceptance and provenance

- [x] Brief and existing concept reviewed; original code authored without outside models.
- [x] Recipe refuses an existing output directory and a non-background/loaded scene.
- [x] Static assertions enforce dimension, triangle, mesh, material and texture targets when run.
- [x] Generated receipts record exact recipe/reference/output hashes, source/import measurements and bytes.
- [ ] Root operator generation and hash verification.
- [ ] Root visual inspection of product render.
- [ ] Systems native material, scale, framing and visibility review.
- [ ] Director decision-log promotion after evidence.

All generated outputs start runtimeEligible:false. Blender-only budget compliance does not pass native performance G5, and a product render does not pass G4. Preserve generation receipts before any eligibility promotion. This modeler recipe/spec does not modify the shared manifest, budgets, Unity files or production decision log; root integrates measured data after execution.

## Preserved r01 root operator receipt and modeler read-only review

[OBSERVED] Root operator executed Blender CLI 5.1.2 in a separate background process. Log /tmp/unknown-c1-signature-reader-r01.log ended with C1_SIGNATURE_READER_GENERATION_COMPLETE and Blender quit. The modeler subsequently read the generated receipts and inspected preview.png; the modeler did not operate Blender.

| Receipt | Observed value |
|---|---|
| Source geometry | 3,920 triangles / 4 meshes / 4 materials |
| Bounds | 0.700 × 0.390 × 0.240 m |
| Shared maps | 4 × 1024² / 1,732,640 bytes |
| Canonical GLB | 3,920 triangles / 2,242,516 bytes |
| FBX adapter reimport | 3,920 triangles / 126,396 bytes |

The circular cradle, magnifier and shallow rectangular tray are visually distinct. Text, numerals, signatures and puzzle-state indicators are absent. However, the product-preview illumination washes the surfaces toward pale cyan. This preview is not approval for native material/lighting appearance. Systems/root must retain the approved dark blue-grey rough appearance in native review; G4/G5 and runtimeEligible remain pending.

Metadata clarification: original measurements/provenance use glossary_id=reader. The glossary explicitly separates the device 판독기 from the action reader / 판독. Interpret that original field only as a related action reference. Preserve the original hash-bound generation receipt. Any reviewed metadata can use glossary_name=판독기, tool_action_id=reader, and internal asset_id=c1-signature-reader, with a recorded audit rather than silent rewriting.

## r02 — one rotation-sign correction

[OBSERVED] Diagnostic-3 reader/reader-front.png revealed that r01's swing arm visibly missed both the rear post and lens neck. This is authored source geometry, not a Unity axis or camera error. Root required a correction before runtime promotion.

The only geometry difference is Swing_Arm rotation about Blender Z: -0.47 radians becomes +0.47 radians. The new recipe also updates revision and output directory to r02. All r01 recipe/output/receipt bytes are preserved. Material, palette, dimensions, mesh budgets and every other part remain identical in the recipe.

Calculated centreline endpoints from the authored arm centre (-0.210, 0.038, 0.226), length 0.164 and rotation +0.47:

- Rear end: (-0.247137, 0.111109, 0.226), within the rear hinge/post around (-0.250, 0.116, 0.229).
- Lens end: (-0.172863, -0.035109, 0.226), within the lens neck around (-0.164, -0.043, 0.225).

These are numerical checks of authored coordinates, not a new Blender measurement or native pass. Root must execute r02 and inspect both physical connections in the native elevated source-front capture before promotion. r02 runtimeEligible starts false.

Recipe SHA256: 1dbcae85a73d4c88853efe516e6a3ff5afb6d60068fa265da10927c92acf34f5.

- [x] r02 Python syntax validated.
- [x] Diff limited to revision, output path and one angle sign.
- [ ] Root Blender execution and r02 receipt verification.
- [ ] Native reimport confirms arm joins the post and lens neck.
