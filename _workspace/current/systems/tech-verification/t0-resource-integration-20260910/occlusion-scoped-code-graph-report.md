---
updated: 2026-09-10
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: systems
---

# Graph Report - graph-scope  (2026-09-10)

## Corpus Check
- 5 files · ~2,466 words
- Verdict: corpus is large enough that graph structure adds value.

## Summary
- 156 nodes · 344 edges · 10 communities
- Extraction: 100% EXTRACTED · 0% INFERRED · 0% AMBIGUOUS · INFERRED: 1 edges (avg confidence: 0.8)
- Token cost: 0 input · 0 output

## Community Hubs (Navigation)
- [[_COMMUNITY_Community 0|Community 0]]
- [[_COMMUNITY_Community 1|Community 1]]
- [[_COMMUNITY_Community 2|Community 2]]
- [[_COMMUNITY_Community 3|Community 3]]
- [[_COMMUNITY_Community 4|Community 4]]
- [[_COMMUNITY_Community 5|Community 5]]

## God Nodes (most connected - your core abstractions)
1. `T0GameSession` - 61 edges
2. `T0Interface` - 28 edges
3. `T0ResourceInteractionTests` - 20 edges
4. `HoldButton` - 16 edges
5. `WatchInput` - 16 edges
6. `float` - 9 edges
7. `string` - 6 edges
8. `WrappedButtonHeight` - 6 edges
9. `ViewAction` - 5 edges
10. `bool` - 5 edges

## Surprising Connections (you probably didn't know these)
- `T0Interface` --references--> `string`  [EXTRACTED]
  UI/T0Interface.cs → Tests/PlayMode/T0ResourceInteractionTests.cs
- `T0GameSession` --references--> `string`  [EXTRACTED]
  App/T0GameSession.cs → Tests/PlayMode/T0ResourceInteractionTests.cs
- `HoldButton` --references--> `bool`  [EXTRACTED]
  UI/T0Interface.cs → Input/WatchInput.cs
- `T0GameSession` --references--> `bool`  [EXTRACTED]
  App/T0GameSession.cs → Input/WatchInput.cs
- `T0Interface` --references--> `float`  [EXTRACTED]
  UI/T0Interface.cs → Input/WatchInput.cs

## Communities (10 total, 0 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.11
Nodes (19): Action, CommitReceipt, Tide.App, bool, float, int, LayoutElement, List (+11 more)

### Community 1 - "Community 1"
Cohesion: 0.19
Nodes (8): BackgroundBehavior, GameObject, Keyboard, Mouse, T0ResourceInteractionTests, Tide.Tests, Scene, T0GameSession

### Community 2 - "Community 2"
Cohesion: 0.19
Nodes (7): Canvas, Color, Dictionary, Font, RectTransform, ScrollRect, T0Interface

### Community 3 - "Community 3"
Cohesion: 0.13
Nodes (9): T0GameSession, CancellationTokenSource, JObject, PuzzleCommand, Renderer, T0CommitFeedback, T0RuntimeConfig, Task (+1 more)

### Community 4 - "Community 4"
Cohesion: 0.12
Nodes (8): T0Entry, Tide.App, HoldProgressRing, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler, MonoBehaviour, HoldButton

### Community 5 - "Community 5"
Cohesion: 0.21
Nodes (4): IDisposable, Tide.Input, WatchInput, RebindingOperation

## Knowledge Gaps
- **29 isolated node(s):** `Tide.UI`, `Canvas`, `Font`, `RectTransform`, `Dictionary` (+24 more)
  These have ≤1 connection - possible missing edges or undocumented components.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `T0GameSession` connect `Community 3` to `Community 0`, `Community 4`, `Community 6`, `Community 7`, `Community 8`, `Community 9`?**
  _High betweenness centrality (0.586) - this node is a cross-community bridge._
- **Why does `T0Interface` connect `Community 2` to `Community 0`, `Community 4`?**
  _High betweenness centrality (0.299) - this node is a cross-community bridge._
- **Why does `string` connect `Community 0` to `Community 1`, `Community 2`, `Community 3`?**
  _High betweenness centrality (0.280) - this node is a cross-community bridge._
- **What connects `Tide.UI`, `Canvas`, `Font` to the rest of the system?**
  _29 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.11076923076923077 - nodes in this community are weakly interconnected._
- **Should `Community 3` be split into smaller, more focused modules?**
  _Cohesion score 0.13450292397660818 - nodes in this community are weakly interconnected._
- **Should `Community 4` be split into smaller, more focused modules?**
  _Cohesion score 0.11695906432748537 - nodes in this community are weakly interconnected._