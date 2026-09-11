---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-production-director
---

# Image provider override: GTI

[OBSERVED] The user explicitly selected the `god-tibo-imagen` skill for image generation on 2026-09-11. This replaces the earlier MuAPI/Higgsfield image-provider instruction. Blender remains the 3D authoring tool and Higgsfield remains available for video. Existing generated assets retain their original provenance and runtime approvals.

[OBSERVED] Installed GTI version: 0.3.0. Local Codex login validity was checked without exposing credentials. The installed CLI accepts `--image` for references; use its live help rather than assuming the skill example flag. Dry-run passed for an original blank-paper texture with `--model gpt-6-astra --size 1024x1024`. Backend quota/credit cost is not exposed by this receipt and must not be called zero.

[OBSERVED] Candidate: `assets/generated/2d/texture/c1-signature-paper-gti-r01/`. Scope is blank paper substrate only: no text, signatures, source identities, clues, salt or lower mask. The existing Higgsfield paper in the game is preserved. New generation starts `runtimeEligible:false`; native readability and rights review are required for a later runtime promotion.

Generation outcome and actual dimensions are recorded in the candidate provenance. Decision: RFC-CX-006 in production/decision-log.md.
