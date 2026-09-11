---
name: decisions
description: Architectural and process decisions with rationale. Load when making design choices or asking why something is built a certain way.
triggers:
  - "why"
  - "decision"
  - "rationale"
  - "trade-off"
edges:
  - target: context/architecture.md
    condition: when the decision shapes structure
last_updated: 2026-09-10
---

# Decisions

| id | date | decision | why | alternatives |
|---|---|---|---|---|
| D-001 | 2026-09-09 | Cycle model is update-centric (hotfix/balance-patch/content-update/season) | Ops, expansion, and updates dominate the project's life; a build-once pipeline would leave live work unstructured | Stage-1/2/3 build cycle (game-studio-harness) — kept as ancestry, not used as-is |
| D-002 | 2026-09-09 | Single live `_workspace/current/` + read-only `archive/{run-id}/`, freshness frontmatter, G8 gate | Latest work must be unambiguous and prior work must stay citable; dated sibling folders make "current" ambiguous | Per-run folders |
| D-003 | 2026-09-09 | Memory routing: mex = code-verifiable facts; llm-wiki = rationale/synthesis; graphify = code decomposition; zg = search surface | Each store is trusted only for what its tool can verify | mex-only (insufficient for rationale), wiki-only (drifts from code) |
| D-004 | 2026-09-09 | Flat 14-agent team; lane leads are first responders, not sub-orchestrators | Team members cannot create teams; >2 levels adds coordination cost without quality | Nested guild teams |
| D-005 | 2026-09-09 | Harness reads `GAME_OPS_VAULT` (default `/Users/jangyoung/vaults/llm-wiki`), ignores `LLM_WIKI_VAULT` | The shell's `LLM_WIKI_VAULT` points at another project; the user's global rule names `/Users/jangyoung/vaults/llm-wiki` | Honor `LLM_WIKI_VAULT` |

Rationale narratives live in the vault: `wiki/projects/unknown/decisions.md`.
| D-006 | 2026-09-10 | Session P draft is canonical; session Q re-derives on top; one writer afterwards | Two sessions wrote `_workspace/current/` concurrently; user ruled on the merge | Independent second draft (rejected: duplicate current artifacts) |
| D-007 | 2026-09-10 | Live `planning/campaign.json` + `validate-campaign.mjs` are the single source for campaign aggregates; campaign ids are the cross-doc key | Two lineages of the campaign file existed; hand-copied numbers drifted | Archived lineage A (30·50·…·25) |
| D-008 | 2026-09-10 | Corrosion budget = global routing-cost cap 9, no consumption/depletion | Parent design + prototype + UI contract agree; per-system depletion model contradicted them | Per-system limits 14/14/12/9/9/9 (rejected) |
| D-009 | 2026-09-10 | Time acceptance key `total_minus_afk_min`; target median 450–540; <420 or p25<360 triggers withdrawal of the 8h claim | Design total ≠ observed median; ±6% band unfalsifiable at n=12 | Band-only acceptance |
| D-010 | 2026-09-10 | Unity project lives in-repo at `unity/Unknown/` on 6000.5.6f1; Codex reads only `handoff/` | User asked for in-repo project and Codex implementation | Separate repo |
| D-011 | 2026-09-10 | Generated assets start `runtimeEligible:false` with provenance; GTI 2D, Blender greybox, Higgsfield video; Mixamo conditional (no 3D humanoids) | User named the tools; design has 2D portraits only | Add 3D characters now (deferred to RFC) |
