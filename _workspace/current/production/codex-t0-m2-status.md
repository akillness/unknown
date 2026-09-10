---
updated: 2026-09-10
cycle: 20260909-preproduction-c7
status: draft
supersedes: null
owner: game-production-director
---

# T0 M2 playable slice — implementation and validation

[OBSERVED · 2026-09-10] Unity 6000.5.6f1 now runs the T0 hub/circuit/reader flow with input, settings, canonical save and recovery. Director-operated native macOS smoke completed T0 through real pointer interaction and verified completion/settings after the app process exited and relaunched with the same isolated save directory. The initial build supplied the full end-to-end path. After drawer/UI occlusion and reader-chart scroll overflow were corrected, a targeted smoke of the final player passed: restored T0 completion at 150%, continued hub, visible drawer front/handle and contained reader waveform at the scroll bottom.

Scope remains `t0-b1` / `t0-b2` / `t0-b3`. The 25-minute duration is a design target; no complete campaign, human playtest, baseline performance or production gate is promoted. Systems froze the implementation source manifest at 92 hashes; publication is pending director closeout and commit.

## Runnable implementation

- The packaged `unity/Unknown/` project includes boot/ui-root/hub scenes, tables, input assets and T0 resources. Opening `Assets/_Project/Scenes/boot.unity` or running `Tide.EditorTools.T0ProjectBuilder.BuildMac` uses the included project data.
- `T0ProjectBuilder.Prepare` reads the packaged tables and validates the parent workspace `planning/campaign.json` SHA before preparing scenes; it does not regenerate tables from author documents. Full regeneration uses `systems/pipeline/emit-tables.mjs` and its complete authoring inputs. Neither authoring path is a prerequisite for direct project opening or BuildMac.
- Implemented flows include circuit trace/overlay/anchor/mark/derived resolution, explicit reader windows and independent citations, keyboard/gamepad navigation, settings, preview/confirm/SavePending, receipt-gated publication, undo/redo, atomic saves and read-only recovery. Packages resolved successfully; the earlier package/disk blocker is historical.

## Fresh verification evidence

| Evidence | Result and scope |
|---|---|
| EditMode `systems/tech-verification/t0-m2/results/editmode-7.xml` | 18/18 passed, 0 failed/skipped. The M1 adapter internally runs 21 contract checks; these are not additional NUnit cases. |
| PlayMode `systems/tech-verification/t0-m2/results/playmode-11.xml` | 15/15 passed, 0 failed/skipped. Final systems-confirmed PlayMode receipt. |
| macOS corrected build | Director verified `BuildMacFramingFix`: 314,105,987 bytes, `unity/Unknown/Builds/T0-mac-framing/Unknown.app`. Corrected-build targeted visual smoke passed. |
| Native macOS CUA smoke | Initial player: pointer-driven T0 completion and actual process exit/relaunch. Final player: persisted completion/settings, 150% text, drawer visibility and reader scroll regression passed. |

Raw native test/build procedures and limits are maintained in `systems/tech-verification/t0-m2-native.md`. Earlier failure receipts remain preserved, including automatic ISO timestamp parsing/checksum reload failures and PlayMode UI/layout failures. They are regression history, not the current pass count.

## Observed native path and corrected-build regression

The director used mouse input through documents → plate 0 → circuit alignment `(-1,+1)` → three marks with rules copies → plate and ledger Read/Cite/Confirm with `H−1:H+3` windows. The header reached `T0 완료`. Settings at 150% remained readable; Tab/Return toggled reduced motion. Closing the window removed the actual app process (`pgrep` exit 1). Relaunching with the same isolated save directory restored T0 completion and settings; Continue restored `t0-b3`.

This real process-restart observation is distinct from NUnit GameSession recreation and Input System state-event injection. It proves the full observed path on the initial native build. On the corrected `T0-mac-framing/Unknown.app`, the director reopened the same save directory, observed `T0 완료` at 150%, continued to the hub, saw the drawer front/handle at upper left and scrolled the reader waveform to the bottom without header leakage. This targeted regression passed; it is not a second full replay or physical gamepad/human-usability coverage. Scope and receipt: [native player smoke](../systems/tech-verification/t0-m2-player-smoke.md).

## Source and resource decisions

RFC-CX-003 records the authored source assignments from `synopsis/t0-records.md` §12: standard plate `system-hub` / `station-bureau-standard`, bureau ledger null `systemId` / the same station. The plate association defines a comparison reference frame, not an observed origin. The canonical circuit preserves three existing labels, grid step 1, no fine step and shared solution `(-1,+1)`. Runtime-generated data and source hashes were integrated; source-lane ACKs and defect status remain in their owning records.

- Blender r03 drawer: 2 meshes, 156 triangles, four 1024px maps; actual Unity import/material/fit review and T0-only runtime approval are recorded in `assets/generated/3d/hub-view-drawer-r03/provenance.json`. Original source audit and earlier revisions remain intact. Corrected-player smoke confirmed the drawer front/handle remains visible above the UI.
- Higgsfield `higgsfield-stamp-r01`: generated 3.5-second PCM16 stereo 24kHz WAV, zero clipped samples; exact prompt, job receipt and SHA-256 preserved. The quoted 2.2 credits is not a verified charge. Auditory review remains unperformed; runtime playback is disabled and eligibility is false.
- MuAPI: official interface verified, but no callable authenticated local integration; no mock result or MuAPI resource is counted as generated.

## Completion boundaries

The corrected player, final 18/18 EditMode and 15/15 PlayMode receipts, and scoped native smoke are complete. The final source manifest contains 92 hashes. Director workflow freshness is 0 stale out of 206 documents. Final graph update completed with 7,270 nodes, 7,573 edges and 979 communities, excluding generated caches. mex was skipped because its resolver is unavailable; zg remained lock-busy. G8 remains PARTIAL. Publication is separate director closeout work. Physical gamepad hardware, auditory approval, human playtests, measured 25-minute duration, baseline-machine performance and G4/G5/G6 remain separate evidence requirements. This status does not turn automated tests, T0 completion or an asset approval into those measurements.

## Publication verification

- Index snapshot: all 92 final source hashes match; 220 Unity Asset files include 122 `.meta` files, with no missing file metadata; all 25 ProjectSettings files are included.
- No Unity caches or builds are staged. M2 decision/task sections are staged separately from pre-existing working-tree history.
- The broad staged whitespace check reports native log/XML and Unity serialization whitespace, plus extra final blank lines in nine C# files. These source/evidence bytes are retained to match the verified receipts; this cosmetic check is not claimed as a global PASS. Authored Markdown and script whitespace checks are separate.
