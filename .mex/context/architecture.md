---
name: architecture
description: How the major pieces of this project connect and flow. Load when working on system design, integrations, or understanding how components interact.
triggers:
  - "architecture"
  - "system design"
  - "how does X connect to Y"
  - "integration"
  - "flow"
  - "harness"
  - "workspace"
edges:
  - target: context/decisions.md
    condition: when a structural choice needs its rationale
  - target: context/conventions.md
    condition: when writing artifacts or code
last_updated: 2026-09-09
---

# Architecture

## Harness topology
- Orchestrator `game-production-director` → 13 file-based specialists (flat team, 2 levels max). Lane leads: planner (design), worldview-architect (narrative), presentation-director (visual). QA broadcasts to all.
- Discussion = RFC blocks in `_workspace/current/production/decision-log.md` + SendMessage (fallback `messages/{seq}-{from}.md`); acks per `.claude/skills/game-ops-harness/references/dependency-matrix.md`.

## Data flow per cycle
`intake/production-brief.md` (cycle_type) → `planning/update-scope.md` + feature specs → [worldview → synopsis → concept] ∥ [systems ↔ balance ↔ economy] → `presentation/presentation-spec.md` → modeling ∥ animation ∥ motion ∥ vfx → `qa/gate-measurements.md` → `production/gate-reviews/` → `retrospectives/` → archive.

## Shared-truth files (owner writes, others cite)
`animation/anim-list.md` (key-event ms) · `motion/feel-tuning.md` (hit-stop, reaction band) · `worldview/glossary.md` (all names) · `presentation/presentation-spec.md` (intent + timeline) · `balance/balance-sheet.md` + `economy/reward-bands.md` (mirrored to data by systems) · `qa/gate-measurements.md`.

## Game architecture (to be filled by the first content-update/season cycle)
- Engine: [TARGET] not chosen
- Sim/render split: render reads snapshots, never writes sim state (invariant)
- Data tables: tuning numbers only in data; code exposes knobs
