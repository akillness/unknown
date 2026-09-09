---
name: stack
description: Technologies and tools in use. Load when working with a specific tool or making tech decisions.
triggers:
  - "stack"
  - "tool"
  - "mex"
  - "graphify"
  - "zg"
  - "obsidian"
edges:
  - target: context/setup.md
    condition: when installing or verifying a tool
last_updated: 2026-09-09
---

# Stack

| Tool | Version (verified 2026-09-09) | Role |
|---|---|---|
| mex-agent | 0.7.1 (`mex`) | long-term project memory, code graph (`.mex/graph.db`), drift check |
| graphify (graphifyy) | 0.8.14 | code/corpus graph → `graphify-out/` |
| zvec-grep | 0.2.2 (`zg`) | hybrid FTS+vector search, index `.zvec-grep/` (local/potion-code-16m-v2) |
| obsidian-cli | Obsidian.app bundled | vault `llm-wiki` at `/Users/jangyoung/vaults/llm-wiki` |
| rtk | on PATH | token-compact shell output (hook-wrapped) |
| semble | on PATH | code discovery first pass |
| Blender MCP | available when Blender runs | modeling lane measurements |
| Game engine | [TARGET] undecided | — |
