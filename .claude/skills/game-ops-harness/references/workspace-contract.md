# Workspace Contract

One live folder, one archive. Cycle *n+1* updates `current/` in place; superseded
material moves to `archive/{run-id}/` with `git mv`. Nothing is ever deleted.
`run-id` = `{YYYYMMDD}-{cycle-type}-{version}` and is carried in frontmatter; it becomes
a directory name only at archive time.

## Layout

```
_workspace/
├── current/
│   ├── intake/production-brief.md          director
│   ├── planning/                            game-planner (기획)
│   │   gdd.md  update-scope.md  priority-board.md  feature-specs/
│   ├── balance/                             game-balance-designer (밸런스)
│   │   balance-sheet.md  patch-deltas.md  sim-results/
│   ├── systems/                             game-systems-designer (시스템)
│   │   architecture-contract.md  system-specs/  data-schemas/  tech-verification/  ops/
│   ├── economy/                             game-economy-designer (재화)
│   │   currency-map.md  sink-source-ledger.md  reward-bands.md  offers.md  negotiation-record.md
│   ├── presentation/                        game-presentation-director (연출)
│   │   presentation-spec.md  camera-timing.md  ui-flow.md  scene-boards/
│   ├── synopsis/                            game-synopsis-writer (시놉시스)
│   │   synopsis.md  chapter-beats.md  continuity.md  quests/  dialogue/
│   ├── worldview/                           game-worldview-architect (세계관)
│   │   worldview-bible.md  glossary.md  timeline.md  consistency-audit.md  lore/
│   ├── concept/                             game-concept-artist (컨셉)
│   │   style-guide.md  references.md  sheets/
│   ├── vfx/                                 game-vfx-artist (이팩트)
│   │   vfx-spec.md  effect-library.md  perf-budget.md  readability.md
│   ├── animation/                           game-animator (에니메이션)
│   │   anim-list.md  rig-requirements.md  state-machines/  clip-specs/
│   ├── motion/                              game-motion-designer (모션)
│   │   locomotion-spec.md  feel-tuning.md  camera-motion.md  mocap-plan.md  feel-verification.md
│   ├── modeling/                            game-modeler (모델링)
│   │   asset-manifest.md  poly-budget.md  pipeline.md  specs/
│   ├── qa/                                  game-qa
│   │   test-plan.md  defect-register.md  exploit-register.md  gate-measurements.md
│   │   regression-matrix.md  telemetry-summary.md  immersion-scores.md  playtest-report.md
│   ├── production/                          director
│   │   task-manifest.md  decision-log.md  changelog.md  gate-reviews/
│   ├── messages/{seq}-{from}.md             fallback + audit trail
│   ├── conflicts.md
│   └── retrospectives/cycle-{n}-retrospective.md
└── archive/{run-id}/{lane}/...              READ-ONLY
```

## Frontmatter (every artifact under current/ and archive/)

```yaml
---
updated: YYYY-MM-DD
cycle: {run-id}
status: current | superseded | draft
supersedes: {archive path} | null
owner: {agent-name}
---
```

Rules:
- `status: current` is unique per logical artifact; two `current` versions of the same
  artifact is a G8 failure.
- `supersedes:` must resolve to a file under `archive/` whose `status` is `superseded`.
- `updated` must be ≥ the cycle start date for any artifact the cycle touched.
- Mark claims `[OBSERVED]`, `[INFERENCE]`, `[TARGET]`, `[CARRIED]` whenever ambiguous.

## Archiving procedure (director, at close or at replacement)
```bash
bash .claude/skills/game-ops-harness/scripts/archive-cycle.sh <run-id> <path-under-current>...
```
The script `git mv`s each path into `archive/<run-id>/<same relative path>`, rewrites its
`status` to `superseded`, and prints the archive path to paste into the successor's
`supersedes:`.

## Key schemas
- `task-manifest.md` row: `| task | owner | phase | artifact | gate | status | beat |`
- `decision-log.md` entry: `RFC-{n}` block with `lanes, question, proposal, evidence, replies[], decision, decided_by, date`
- `gate-measurements.md`: `#g{n}` sections with `measured:` YAML (value, method, command, timestamp)
- `changelog.md`: per version, grouped by lane, each line citing the artifact + RFC
