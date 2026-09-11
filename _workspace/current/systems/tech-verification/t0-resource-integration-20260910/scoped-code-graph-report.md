---
updated: 2026-09-10
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: systems
---

# Graph Report - graph-scope  (2026-09-10)

## Corpus Check
- 7 files · ~2,556 words
- Verdict: corpus is large enough that graph structure adds value.

## Summary
- 170 nodes · 356 edges · 12 communities (11 shown, 1 thin omitted)
- Extraction: 100% EXTRACTED · 0% INFERRED · 0% AMBIGUOUS · INFERRED: 1 edges (avg confidence: 0.8)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `687477d6`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- [[_COMMUNITY_Community 0|Community 0]]
- [[_COMMUNITY_Community 1|Community 1]]
- [[_COMMUNITY_Community 2|Community 2]]
- [[_COMMUNITY_Community 3|Community 3]]
- [[_COMMUNITY_Community 4|Community 4]]
- [[_COMMUNITY_Community 5|Community 5]]
- [[_COMMUNITY_Community 7|Community 7]]
- [[_COMMUNITY_Community 11|Community 11]]

## God Nodes (most connected - your core abstractions)
1. `T0GameSession` - 60 edges
2. `T0Interface` - 28 edges
3. `T0ResourceInteractionTests` - 18 edges
4. `HoldButton` - 16 edges
5. `WatchInput` - 16 edges
6. `T0ProjectBuilder` - 10 edges
7. `float` - 9 edges
8. `string` - 7 edges
9. `WrappedButtonHeight` - 6 edges
10. `ViewAction` - 5 edges

## Surprising Connections (you probably didn't know these)
- `ViewAction` --references--> `bool`  [EXTRACTED]
  UI/T0Interface.cs → Input/WatchInput.cs
- `T0Interface` --references--> `string`  [EXTRACTED]
  UI/T0Interface.cs → Editor/T0ProjectBuilder.cs
- `CommitReceipt` --references--> `string`  [EXTRACTED]
  App/T0GameSession.cs → Editor/T0ProjectBuilder.cs
- `T0GameSession` --references--> `string`  [EXTRACTED]
  App/T0GameSession.cs → Editor/T0ProjectBuilder.cs
- `T0ResourceInteractionTests` --references--> `string`  [EXTRACTED]
  Tests/PlayMode/T0ResourceInteractionTests.cs → Editor/T0ProjectBuilder.cs

## Communities (12 total, 1 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.14
Nodes (15): Action, float, LayoutElement, List, MaskableGraphic, string, Text, AnchorDiagram (+7 more)

### Community 1 - "Community 1"
Cohesion: 0.19
Nodes (7): Canvas, Color, Dictionary, Font, RectTransform, ScrollRect, T0Interface

### Community 2 - "Community 2"
Cohesion: 0.12
Nodes (8): T0Entry, Tide.App, HoldProgressRing, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler, MonoBehaviour, HoldButton

### Community 3 - "Community 3"
Cohesion: 0.15
Nodes (8): CommitReceipt, Tide.App, bool, IDisposable, Tide.Input, WatchInput, int, RebindingOperation

### Community 4 - "Community 4"
Cohesion: 0.18
Nodes (8): BackgroundBehavior, GameObject, Keyboard, Mouse, T0ResourceInteractionTests, Tide.Tests, Scene, T0GameSession

### Community 5 - "Community 5"
Cohesion: 0.13
Nodes (9): T0GameSession, CancellationTokenSource, JObject, PuzzleCommand, Renderer, T0CommitFeedback, T0RuntimeConfig, Task (+1 more)

### Community 11 - "Community 11"
Cohesion: 0.33
Nodes (5): T0RuntimeConfig, Tide.App, ScriptableObject, T0CatalogAsset, TextAsset

## Knowledge Gaps
- **33 isolated node(s):** `Tide.UI`, `Canvas`, `Font`, `RectTransform`, `Dictionary` (+28 more)
  These have ≤1 connection - possible missing edges or undocumented components.
- **1 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `T0GameSession` connect `Community 5` to `Community 0`, `Community 2`, `Community 3`, `Community 6`, `Community 8`, `Community 10`, `Community 12`?**
  _High betweenness centrality (0.524) - this node is a cross-community bridge._
- **Why does `string` connect `Community 0` to `Community 1`, `Community 3`, `Community 4`, `Community 5`, `Community 7`?**
  _High betweenness centrality (0.332) - this node is a cross-community bridge._
- **Why does `T0Interface` connect `Community 1` to `Community 0`, `Community 2`, `Community 3`?**
  _High betweenness centrality (0.273) - this node is a cross-community bridge._
- **What connects `Tide.UI`, `Canvas`, `Font` to the rest of the system?**
  _33 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.14285714285714285 - nodes in this community are weakly interconnected._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.11695906432748537 - nodes in this community are weakly interconnected._
- **Should `Community 3` be split into smaller, more focused modules?**
  _Cohesion score 0.14619883040935672 - nodes in this community are weakly interconnected._