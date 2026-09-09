---
name: game-ops-harness
description: >
  13-role game production & live-ops harness (기획·밸런스·시스템·재화·연출·시놉시스·
  세계관·컨셉·이팩트·에니메이션·모션·모델링·QA) under one production director.
  Runs update-centric cycles (hotfix | balance-patch | content-update | season) over a
  single live `_workspace/current/` with read-only `_workspace/archive/`, freshness
  frontmatter on every artifact, cross-role RFC discussion, gates G1–G8, and a layered
  memory stack: mex (.mex/ long-term project memory) → llm-wiki vault (rationale &
  synthesis) → graphify (code decomposition) → zg (search-first code work).
  Use when starting or resuming a game cycle, preparing a patch/season/expansion,
  running a gate review, archiving a cycle, or when any role lane needs to discuss a
  change with another. Triggers on: 게임 제작, 게임 하네스, 업데이트 사이클, 핫픽스,
  밸런스 패치, 시즌 준비, 컨텐츠 업데이트, 게이트 리뷰, 아카이빙, 회고, 라이브 운영,
  game ops, game production cycle, patch cycle, live-ops.
allowed-tools: Bash Read Write Edit Glob Grep Agent TeamCreate TaskCreate TaskUpdate SendMessage
metadata:
  version: "1.0.0"
  tags: game-production, live-ops, multi-agent, mex, llm-wiki, graphify, zvec-grep, workspace-archive, freshness
  keyword: game-ops-harness
---

# Game Ops Harness

A standing 13-role studio for a game that will be **operated, expanded, and updated**
far longer than it is initially built. Every cycle is an update. The workspace has one
live folder and one archive; every artifact says when it was updated and what it
replaced; every role can open a discussion with any other; and knowledge that must
outlive the session goes into the memory stack, not chat.

Read `references/workspace-contract.md`, `references/cycle-types.md`,
`references/memory-stack.md`, `references/dependency-matrix.md`, and
`references/quality-gates.md` before creating or resuming a cycle. Gate thresholds in
`references/quality-gates.md` override any paraphrase here.

## When to use this skill
- Start a cycle from an idea, GDD, live signal (telemetry/defect/exploit), or roadmap item
- Resume an in-flight cycle (read `_workspace/current/production/task-manifest.md` first)
- Prepare a hotfix, balance patch, content update, season, or expansion
- Run a gate review (G1–G8) or close/archive a cycle with a retrospective
- Facilitate a cross-role discussion (e.g., 연출 needs a synopsis beat changed; 밸런스 vs 재화)
- 게임 제작·운영·확장 사이클을 역할별 에이전트 팀으로 굴릴 때

Route narrower packets elsewhere: build/log failures → `game-build-log-triage`;
profiler captures → `game-performance-profiler`; feedback-only triage →
`game-demo-feedback-triage`; store/launch ops → `steam-store-launch-ops`; pure
code-graph questions → `graphify`; pure memory setup → `mex`; pure wiki filing → `llm-wiki`.

## Instructions

### Step 0: Bootstrap once per repository
Why: agents must be file-based, the workspace must exist, and the memory stack must be
initialized before any role writes.
```bash
bash .claude/skills/game-ops-harness/scripts/bootstrap-workspace.sh "$(pwd)"
```
This creates `_workspace/current/{lanes}`, seeds `production/task-manifest.md`, installs
`.mex/` (`mex setup`), builds `graphify-out/` (`graphify update .`), indexes the workspace
with `zg index`, and checks the llm-wiki vault. It is idempotent. Re-run after adding a
lane or tool.

### Step 1: Session start (every session, before any assignment)
Why: the freshest state lives in files and memory, never in chat history.
```bash
bash .claude/skills/game-ops-harness/scripts/session-start.sh "<one-line task>"
```
Prints: `mex graph scope`, latest task-manifest + retrospective, freshness report, graph
summary, and the vault index entries for this project. Read it; then act.

### Step 2: Intake → cycle type (director)
Normalize the request into `intake/production-brief.md` and pick ONE cycle type from
`references/cycle-types.md`. The type fixes entry phase, required lanes, and gates.
Announce it to the team with the next public beat (version/date).

### Step 3: Materialize the team
- Claude Code with `CLAUDE_CODE_EXPERIMENTAL_AGENT_TEAMS=1`: `TeamCreate` with the
  required lanes from `.claude/agents/game-*.md`; use `TaskCreate`/`TaskUpdate` for the
  manifest rows and `SendMessage` for discussion.
- Teams unavailable: run the same roles as sequential sub-agents in phase order; peer
  messages become `_workspace/current/messages/{seq}-{from}.md`.
Team members never create teams. Lane leads (planner / worldview-architect /
presentation-director) are first responders for their lane, not sub-orchestrators.

### Step 4: Run the phases (see `references/cycle-types.md` for per-type entry)
- **P1 Scope** — planner: `planning/update-scope.md`, feature specs; RFCs opened for
  multi-lane features.
- **P2 Foundation** (parallel groups): [worldview → synopsis → concept] ∥ [systems ↔
  balance ↔ economy]. Each handoff is a file with fresh frontmatter and an ack in the RFC.
- **P3 Production** (parallel): presentation spec first, then modeling ∥ animation ∥
  motion ∥ vfx implement toward it; systems lands code with graph/memory receipts.
- **P4 Verification** — QA producer-reviewer loop (≤2 FIX loops per gate); broadcasts to
  affected lanes per dependency-matrix.
- **P5 Close** — director: gate table, retrospective, archive, memory sync, rule-file
  re-derivation.

### Step 5: Discussion protocol (any role ↔ any role)
Why: 13 roles produce contradictions; unlogged resolutions are lost by the next session.
1. Opener writes an RFC entry in `production/decision-log.md` (`RFC-{n}`: lanes, question,
   proposal, evidence paths) and SendMessages the affected lanes.
2. Each affected lane replies within the cycle with `ack | counter (with evidence) | block`.
3. One exchange unresolved → director arbitrates on evidence and logs the decision.
4. Resolution updates the owning artifacts (fresh frontmatter) and `mex log "RFC-{n}: …"`.

### Step 6: Freshness & archive rule (the 최신화 contract)
Every artifact under `_workspace/current/` carries:
```yaml
---
updated: 2026-09-09
cycle: 20260909-content-update-v1.3
status: current            # current | superseded | draft
supersedes: _workspace/archive/20260801-season-1/planning/gdd.md   # or null
---
```
When a file is replaced, `git mv` the old one into `_workspace/archive/{run-id}/{lane}/`,
set its `status: superseded`, and point the new file's `supersedes:` at it. Old work is
therefore always reachable and citable, never edited. `scripts/freshness-check.sh` is the
G8 measurement.

### Step 7: Memory sync at close (director)
Follow `references/memory-stack.md`:
`mex log` (decisions) → `mex check` (drift) → `mex sync` if drift → llm-wiki report via
`obsidian-cli` (rationale, retrospective synthesis) → `graphify update .` if code changed
→ `zg index --rebuild` if the workspace layout changed → re-derive root `CLAUDE.md` if a
lane/tool/invariant changed.

### Step 8: Error handling
| Scenario | Response |
|---|---|
| Agent timeout | Retry once → `failed` in manifest, continue partial, flag at gate |
| Lane conflict | `conflicts.md` + RFC; prefer newer measurement; director arbitrates |
| Missing output | Gate cannot PASS; task stays open |
| Messaging failure | `messages/{seq}-{from}.md` fallback |
| Memory tool missing | Record under `memory_sync.skipped` in retrospective; never fake receipts |

## Examples

### Example 1: New season
Input: "시즌 2 준비 시작. 새 지역 + 신규 재화 + 보스 연출"
Expected: brief with `cycle_type: season`, worldview expansion → synopsis beats → concept
sheets; economy adds currency with sink; presentation spec for boss; all G1–G8 measured;
archive of season-1 material; llm-wiki report `wiki/reports/{date}-season-2-cycle.md`.

### Example 2: Balance hotfix from live telemetry
Input: "라이브에서 대시 i-frame 익스플로잇 발견"
Expected: `cycle_type: hotfix`, entry at P4 with exploit register pre-seeded; balance +
motion RFC on i-frame ms; systems applies data-only change; QA re-measures G2; G8 confirms
the superseded feel-tuning is archived; `mex log` receipt.

### Example 3: Cross-role discussion
Input: "연출팀이 3챕터 보스 등장 씬을 바꾸고 싶어함"
Expected: `RFC-n` by presentation-director → synopsis-writer + worldview-architect +
vfx + animator acks; resolution updates `chapter-beats.md` and `presentation-spec.md`
with fresh frontmatter and archived predecessors.

## Best practices
1. One cycle type per cycle; scope growth is re-intaken.
2. Numbers gate everything; adjectives never pass a gate.
3. Search before read (`zg`), structure before edit (`graphify`), memory before invention (`mex`, vault).
4. Archive, never delete. `_workspace/` is the studio's memory; the vault is its reasoning; `.mex/` is its code-grounded truth.
5. Every RFC ends in a file change with fresh frontmatter, or it didn't happen.
6. Re-derive `CLAUDE.md` when the harness itself changes — the rule file is a cycle artifact.

## References
- [Workspace Contract](references/workspace-contract.md)
- [Cycle Types](references/cycle-types.md)
- [Memory Stack (mex / llm-wiki / graphify / zg)](references/memory-stack.md)
- [Dependency Matrix](references/dependency-matrix.md)
- [Quality Gates G1–G8](references/quality-gates.md)
