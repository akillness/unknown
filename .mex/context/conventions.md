---
name: conventions
description: Patterns, naming, and the verify checklist for this project. Load when writing or reviewing artifacts or code.
triggers:
  - "conventions"
  - "naming"
  - "frontmatter"
  - "verify"
  - "checklist"
edges:
  - target: context/architecture.md
    condition: when a convention depends on a structural boundary
last_updated: 2026-09-09
---

# Conventions

## Artifacts
- Every `_workspace/**/*.md` starts with frontmatter `updated, cycle, status (current|superseded|draft), supersedes, owner`.
- Kebab-case filenames; one H1; YAML blocks for every gate-checkable number; prose explains, never replaces numbers.
- Claims tagged `[OBSERVED] [INFERENCE] [TARGET] [CARRIED]`; every measurement cites command/session + timestamp.
- Names (units, items, assets, features) originate in `worldview/glossary.md`; asset ids derive from glossary entries.
- Decision log is append-only with unique `RFC-{n}`; reread the tail before appending.

## Code (systems lane)
- Search-first: `mex graph scope` → `zg query` → `graphify query` → read cited lines only → edit.
- Every code change ends with `graphify update .`, `mex graph`, `mex check`, `mex log`.
- Tuning numbers in data tables; persisted-field renames require migrations.

## Verify Checklist
1. Touched artifacts have fresh frontmatter and `status: current`; predecessors archived via `archive-cycle.sh`.
2. `freshness-check.sh` exits 0.
3. Every number cites a measurement or is tagged `[TARGET]`.
4. RFC acks present for every ● in the dependency matrix.
5. Graph/memory receipts present if code changed.
6. `mex check` reports no drift.
