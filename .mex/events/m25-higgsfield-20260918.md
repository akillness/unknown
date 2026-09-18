---
name: m25-higgsfield-20260918
description: M25 content-update — Higgsfield resources applied behind M25Resources gate, guide overlay, TypeScale, macOS dev build, GitHub Release v0.25.0-dev.
last_updated: 2026-09-18
---

# M25 · 2026-09-18

- Provider decision: Higgsfield CLI for M25 (RFC-CX-M25-20260918) supersedes the GTI image rule for this cycle; generator `scripts/gen-higgsfield.py`, manifest `_workspace/current/concept/m25-higgsfield-jobs.json`.
- Generated 18 images + 3 clips (2 quay rejections kept); imported 18 items via `Editor/M25ResourceProjectBuilder.Import` (SHA-checked); approved local dev profile only.
- Runtime: `App/M25ResourceSession.cs` (guide overlay, figures, teaching header), `UI/TypeScale.cs`, `UI/T0Interface.cs` (sections, figure rows, navigation backdrop, opening VideoPlayer with still fallback), `Input/WatchInput.cs` + `WatchBindings.json` (F2 guide), `Resources/T0Strings.json` (+26 keys, teachingHeader), `M22SeorinSession.cs` (title stage stays under guide).
- Receipts: EditMode 65/65 · PlayMode 134 (133/0/1 conditional skip) · boot 1/1 · build 316 files/438,910,587 B digest aaa5361d… · 5 native captures in `docs/media/m25/`.
- Memory sync: mex-agent skipped (PATH mex = TeX); graphify/zg/vault results recorded in `_workspace/current/retrospectives/m25-higgsfield-resources-20260918.md`.
