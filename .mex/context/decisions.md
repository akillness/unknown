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
last_updated: 2026-09-09
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
