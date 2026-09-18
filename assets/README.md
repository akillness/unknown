# assets/ — generated resources (all `runtimeEligible:false` until audited)

| folder | tool | provenance |
|---|---|---|
| `generated/2d/<category>/` | GTI (`scripts/gen-2d.sh`) — Codex backend, model `gpt-6-astra` (2026-09-11 rule) | `provenance.json` per folder |
| `generated/2d/m25/{backgrounds,portraits,tools}/` | **Higgsfield CLI** (`scripts/gen-higgsfield.py` + `_workspace/current/concept/m25-higgsfield-jobs.json`) — `gpt_image_2`, `nano_banana_flash` (2026-09-18 rule, RFC-CX-M25-20260918). `rejected/` keeps the two §10-4 rejections. | `provenance.json` per set (job id, refs + hashes, credits before/after) |
| `generated/video/m25/` | Higgsfield `seedance_2_0` image-to-video, 5 s 720p silent. **Pre-visualization, not gameplay.** | `provenance.json` |
| `generated/3d/` | Blender MCP blockouts / rigs | `provenance.json` |
| `generated/previz/` | GTI frames → ffmpeg GIF (README cutscene). **Pre-visualization, not gameplay.** | `provenance.json` |
| `generated/audio/` | Higgsfield seed_audio (optional) | `provenance.json` |

Promotion to `unity/Unknown/Assets/` requires a decision-log entry (CLAUDE.md §9). M25 assets are copied by `Tide.EditorTools.M25ResourceProjectBuilder.Import` (SHA-checked) into `Art/Candidates/m25/` and gated by `Resources/M25Resources.asset` (`Approve()` = local development profile only; commercial licence UNVERIFIED). Never edit `provenance.json` by hand.
