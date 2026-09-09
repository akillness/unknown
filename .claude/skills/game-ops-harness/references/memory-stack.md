# Memory Stack — mex / llm-wiki / graphify / zg

Four layers, each with one job. Put knowledge where the tool that verifies it lives.

| Layer | Path | Holds | Verified by | Write when |
|---|---|---|---|---|
| **workspace** | `_workspace/current/` | the live artifacts (source of truth for the cycle) | freshness-check (G8) | every task |
| **archive** | `_workspace/archive/{run-id}/` | frozen predecessors | git history | cycle close / replacement |
| **mex** | `.mex/` (`context/`, `patterns/`, `graph.db`, events) | code-grounded facts: architecture, stack, conventions, decisions timeline, symbol-anchored notes | `mex check` (drift) | any decision; any code change; cycle close |
| **llm-wiki** | `~/vaults/llm-wiki` (obsidian-cli vault `llm-wiki`) | rationale, surveys, playtest synthesis, retrospectives, cross-project knowledge — anything `mex check` cannot verify against code | lint + human read | RFC resolution with lasting rationale; cycle close; research |
| **graphify** | project `graphify-out/` (`graph.json`, `GRAPH_REPORT.md`) | code decomposition: modules, hubs, communities, affected flows | `graphify update` | after any code change |
| **zg** | `.zvec-grep/` index | search surface over code + workspace docs | `zg status` | bootstrap; after layout change (`zg index --rebuild`) |

Do not mix the vault's `graphify-out/` (knowledge graph of prompts/sources) with the
project's `graphify-out/` (code graph, at the repo root). Never overwrite one with the other.

## Routing rule (mex vs llm-wiki)
- Fact about **how the code/data works** → `.mex/context/*.md` + `mex log`.
- **Why** a decision was made, what alternatives lost, what the playtest taught, what a
  survey found → llm-wiki (`wiki/projects/{game}/…`, `wiki/reports/…`, `wiki/queries/…`).
- If unsure: write the fact to mex, and the reasoning to the vault, and link both from the
  `decision-log.md` entry.

## Commands by moment

Session start (`scripts/session-start.sh` runs these):
```bash
mex graph scope "<task>"                     # compact, task-relevant code context
mex timeline | tail -20                      # recent decisions
sed -n "1,/^## Graph Freshness/p" graphify-out/GRAPH_REPORT.md   # hubs/communities (graphify 0.8.x has no `summary`)
zg status                                    # index health
obsidian-cli vault=llm-wiki read path=index.md | grep -i "<game>"
```

Code work (systems lane; mandatory order):
```bash
mex graph scope "<task>"
zg query "<semantic intent>"                 # unknown wording/location
zg query --rg -F "<exact symbol>" <path>     # exact anchor
graphify query "<which flows does X touch?>" --budget 1500
# … edit …
graphify update .
mex graph && mex check
mex log "systems: <what changed and why> (RFC-n)"
```

Decision (any lane, via director):
```bash
mex log "RFC-{n} decided: <one line>"
obsidian-cli vault=llm-wiki append path=wiki/projects/{game}/decisions.md content="- {date} RFC-{n}: <why> ([[wiki/reports/…]])"
```

Cycle close (director):
```bash
mex check                                    # drift report
mex sync                                     # only if check reports drift
obsidian-cli vault=llm-wiki create path=wiki/reports/{date}-{game}-{cycle-type}-{version}.md content="<retrospective synthesis>"
obsidian-cli vault=llm-wiki append path=index.md content="- [[wiki/reports/{date}-…]] — {one line}"
graphify update .                            # if code changed this cycle
zg index --rebuild                           # if workspace layout changed
```
Fallback when Obsidian is not running: write the files directly under `~/vaults/llm-wiki/`
and update `index.md` + `log.md` by hand.

## Receipts
The retrospective's `memory_sync:` block lists each command run and its exit status. A
missing tool is recorded under `skipped:` with the reason — never a fabricated receipt.
