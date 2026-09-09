---
name: setup
description: How to set up and run this project. Load when bootstrapping a machine or verifying the environment.
triggers:
  - "setup"
  - "install"
  - "bootstrap"
  - "environment"
edges:
  - target: context/stack.md
    condition: when a tool is missing
last_updated: 2026-09-09
---

# Setup

```bash
npm i -g mex-agent && pip install graphifyy          # mex, graphify
# zg: see zvec-grep install docs; obsidian-cli ships with Obsidian.app
bash .claude/skills/game-ops-harness/scripts/bootstrap-workspace.sh "$(pwd)"   # idempotent
bash .claude/skills/game-ops-harness/scripts/session-start.sh "smoke"
bash ~/.claude/skills/harness/scripts/validate-harness.sh .claude/agents .claude/skills
```
Set `GAME_OPS_VAULT` only if the vault is not `/Users/jangyoung/vaults/llm-wiki`.
