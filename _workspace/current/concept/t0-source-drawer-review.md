---
updated: 2026-09-10
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-concept-artist
---

# T0 source / drawer scoped concept review

## Verdict and scope

[OBSERVED] Concept ACK for the two authored source assignments and the r01 drawer's existing-identity blockout silhouette/palette. **Final material FIX** remains open. This review covers the isolated static preview and actual GLB material data only; it is not a Unity import, scene-fit, animation, readability, runtime, or gameplay pass. It does not promote runtime eligibility or any production gate.

Scope: RFC-CX-001 / RFC-CX-003, T0 hub attachment and two citation records. New sheets, names, symbols, production identities, gameplay parameters, and timings were not authored here.

## Source assignments: ACK

Sources: [worldview glossary §6-2](../worldview/glossary.md#6-2-t0-인용-출처-식별자-rfc-cx-001) and [synopsis T0 records §12](../synopsis/t0-records.md).

| Record | systemId | stationId | Concept disposition |
|---|---|---|---|
| rec-plate-standard-hub | system-hub | station-bureau-standard | ACK: existing hub-system name; station assignment is a newly adopted comparison reference frame. It does not assert historical observation production or sensor ownership. |
| rec-tide-ledger-bureau | null | station-bureau-standard | ACK: existing bureau-standard reference; systemId is explicitly not applicable to this ledger. Do not fabricate a system identity. |

[OBSERVED] The machine-readable §12 packet contains exactly these two assignments. The three non-citation records are excluded. Existing Korean names and plate/ledger identities remain unchanged. These technical IDs do not introduce a location, sensor, icon, color, or separate visual identity. Evidence independence remains sourceType + rootOriginId; the new associations do not add an independent source.

[TARGET] The plate's station association is an authoring decision, not a recovered measurement. This concept ACK covers that semantic and visual boundary only. Glossary §6-2 remains draft pending the director's cross-lane collection; this report supplies only the concept lane ACK.

## r01 drawer inspection

Inputs: assets/generated/3d/hub-view-drawer-r01/{preview.png,SM_Hub_Workbench_Drawer.glb,measurements.json,provenance.json}; recipe assets/generated/3d/scripts/build_hub_drawer.py.

[OBSERVED] Preview SHA-256: f4993f7f4df677a61940859fbf57b3e0591459e0b9437630091ed5fa97db6308.

[OBSERVED] The isolated view shows a low rectangular casing, recessed single front tray, centered horizontal handle, and dark separating seam. It is consistent with the existing hub workbench/drawer structure and the **절차의 무게** pillar; it adds no in-fiction emblem or identity. Context targets already exist in [concept sheets](sheets/README.md): space-hub-watchroom-mood (hub drawers) and ui-workbench-frame (evidence compartments). The modeling manifest explicitly leaves the dedicated final-art prop sheet unapproved.

[OBSERVED] GLB JSON material inspection, converting linear baseColorFactor back to sRGB, gives:

| Actual material | sRGB | Roughness | Emission |
|---|---|---|---|
| MAT_Hub_Drawer_Casing | #36565C | 0.8 | none |
| MAT_Hub_Drawer_Inset | #173238 | 0.8 | none |
| MAT_Hub_Drawer_Handle | #4F7A6B | 0.8 | none |

[OBSERVED] These three colors exist in style-guide §2. GLB has no texture references. The generated measurements receipt reports two meshes, 156 triangles total; this is not a Unity-import triangle measurement. Provenance remains runtimeEligible:false. Original GLB stays canonical and FBX is a derived candidate under RFC-CX-003.

**Concept defect — final material FIX:** the static preview has broad uniform surfaces and clean edges, with no visible salt clusters or downward corrosion streaks. This fails the final material intent in [style-guide §1 and §4](style-guide.md). The r01 remains useful as a blockout. A small patch count alone cannot establish the §1 frame-level target of at most 5% unworn metal surface. No quantitative worn-area measurement was performed.

## Existing wear rules for r02; no new palette

- style-guide §2: #36565C structural base (frame 25–45%), #173238 dark outlines; #4F7A6B corrosion/pipe joints (frame ≤15%), #C8D6D3 salt crystals (frame ≤12%). These are frame limits, not per-drawer allocation quotas.
- §4 corrosion: verdigris spots on a dark supporting layer, streaks always downward; broad low-gloss highlights. Uniform mint paint is a failure signal.
- §4 salt: fine hexagonal clusters growing at edges, matte scattered points without specular highlights. Sugar-like white lumps and snow are failure signals.
- §1 metal: visible wear, salt rings and corrosion traces; prohibit chrome gloss, freshly painted surfaces, and pristine edges.
- §2 reserve #8C4A3A is limited to oxidation on **pipe joints**, frame ≤2%. It is not an authorized general-purpose drawer rust color. Do not add a ninth palette color.
- §10: no baked text/numbers/logos, neon, glossy CGI, lens flare, bloom, or red warning lamp. A drawer label or source icon is not introduced by this review.

[TARGET] Root will request an r02 modeling candidate. Geometry plus existing-palette material patches can be inspected as a candidate; this report does not prescribe new geometry counts, motion, or authoring technique. Existing modeling ceiling ≤1500 triangles and the final-art sheet/import/scene reviews still apply.

## Existing T0 UI / scene colors sent to systems

Exact existing roles from style-guide §2; this is not a new UI token map:

| Color | Existing role |
|---|---|
| #0E1F26 | Deepest dark, underwater, shadow core |
| #173238 | Confirmed records, signatures, outlines |
| #36565C | Structural base, frame 25–45% |
| #C8D6D3 | Salt crystals, frost, spray, frame ≤12% |
| #E7E3D8 | Paper and brightest range, frame ≤20% |
| #E2AF62 | Sole warm accent: lamp / proposed-change indicator, frame ≤8% |

[OBSERVED] The style guide does **not** specify dedicated universal background, body-text, interaction, or error UI tokens. Confirmed-record color has direct support; shadow/structure values support those existing material roles without defining a universal UI background. Proposed-change accent is not a generic error or success token. Do not infer an error red.

Existing hierarchy: §3 requires a dark separation line at least 3 px at 1080p where needed, and concentrates tool/hand detail in the central 40–65% height band. §2.1 requires state/media/tool identification through color, silhouette, and position together; color alone is insufficient. §6 preserves hexagonal plate, rounded book, and tall ledger-spine silhouettes. §10 says danger is read from waterlines rather than red warning lamps. These are targets; actual Canvas contrast/focus/keyboard/controller behavior remains unmeasured by this review.

## Completion boundaries

[OBSERVED] Concept ACK and material FIX were communicated to the director; the existing six-color excerpt and absent UI tokens were sent to systems. Only this new concept report was authored. Canonical defect registration, r02 authoring, runtime import review, and gate decisions remain with their owners. Director owns final project graph/wiki integration while parallel implementation continues.

## r02 actual preview follow-up — FIX

[OBSERVED, 2026-09-10] Inspected assets/generated/3d/hub-view-drawer-r02/preview.png, actual GLB JSON, measurements.json and provenance.json. Preview SHA-256: 3fefeec7b0881b296a577c66a6600df774ec69c25609fd43c9250adefba80a4a. GLB contains two worn-material definitions with four embedded BaseColor/Roughness images; the measurement receipt reports 156 triangles. Provenance remains runtimeEligible:false. This is a static preview/material receipt check, not an import or runtime pass.

[OBSERVED] r02 adds downward corrosion on the front/side and pale deposits along edges. Those placements move toward style-guide §4. **Candidate palette/identity direction remains acceptable; final material remains FIX.**

| Visible issue | Bounded correction using existing rules |
|---|---|
| Top-face verdigris spots repeat in a conspicuous diagonal grid, with similar size and spacing; they read like a printed polka-dot pattern. | Break the repeated spacing and size. Group, merge, vary and omit patches so corrosion reads as uneven deposits on the supporting dark metal. Preserve existing palette; do not solve this by adding a color. |
| Front/side drips repeat similar tapered shapes, lengths and gaps. | Keep the existing downward direction, but vary the streak origins, lengths, widths and connectivity. Avoid a repeated stamp pattern. |
| Pale edge deposits form regular dotted lines around nearly the whole perimeter. | Use irregular small clusters and gaps at selected edge areas, preserving the fine hexagonal/matte salt language. Avoid bead-like continuous trim or luminous outlining. |
| Structural face, handle, recess and background are all very dark; their visual separation is weak. | Restore visible distinction between existing #36565C structural planes and #173238 recess/outline roles in the next actual preview. Check texture interpretation and lighting as separate possible causes; this image does not establish which caused the darkness. Do not brighten everything into one value or introduce emissive material. |

[TARGET] Re-review the next rendered candidate for irregular corrosion, readable handle/front separation and restrained edge salt. No new color, wear-coverage percentage, geometry budget, lighting parameter or runtime claim is assigned here. The guide's frame-level limits are unchanged and unmeasured; local patches do not prove final material compliance. Root received the focused FIX feedback. Generated/modeling files were not edited by this review.

## r03 scoped re-review — T0 visual candidate ACK

[OBSERVED, 2026-09-10] Inspected the actual assets/generated/3d/hub-view-drawer-r03/preview.png. SHA-256: 3ad6437aab240fdb3d28d4c06bacfef42b44bf291f7a3235e3df81c8b1f81b90. This follow-up rechecks only the four r02 visual FIX items. It does not expand the acceptance criteria to campaign or frame-budget measurements.

| Earlier actionable FIX | Observed r03 result | Scoped verdict |
|---|---|---|
| Regular top-face polka-dot grid | The evenly spaced grid is absent. Verdigris forms uneven grouped patches with varied sizes, connected areas, and substantial gaps. | ACK |
| Repeated equal drips | Visible front/side streaks vary in placement, length and outline while retaining a downward flow. | ACK |
| Continuous dotted edge trim | Edge deposits break into localized irregular clusters and gaps; they no longer read as one regular beaded perimeter. | ACK |
| Dark structure/handle/recess/background merging | The blue-grey supporting planes are visibly separated from the darker front recess, handle edges and background. The drawer silhouette and handle/front relationship are readable in this preview. | ACK |

**[OBSERVED] No remaining concept blocker among these four items.** r03 is accepted as the corrected T0 visual asset candidate. This replaces the r02 FIX disposition for those four visual issues only; the earlier revision and review remain preserved.

[OBSERVED] The preview establishes the visual result, not the technical cause of its changed brightness. Root reports corrected texture gamma, but this review does not independently certify color-space settings or Unity shader interpretation. Provenance read at review time has runtimeEligible:false.

[TARGET] Next owner check is Unity material/texture interpretation and workbench/inspection-view fit. Runtime scene eligibility requires that separate asset audit and director decision. Final production numerical gates remain unmeasured by this concept review; they are not invoked as a new blocker for this bounded T0 candidate ACK. Only this report was appended, with generated/modeling files unchanged by the reviewer.
