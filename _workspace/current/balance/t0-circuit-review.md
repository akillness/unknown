---
updated: 2026-09-10
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-balance-designer
---

# T0 circuit authored-coordinate balance review

**Verdict: scoped ACK for development integration and the balance dependency of current promotion under RFC-CX-003.** This review does not promote the planning packet on behalf of the director or other lanes. The authoring packet remains subject to its outstanding dependency ACKs.

## Reviewed inputs and authority

- `planning/t0-circuit-overlay.json` and `planning/t0-circuit-overlay.meta.md` are newly authored diagram data [TARGET], not recovered geography or observed player behavior.
- `production/decision-log.md` RFC-CX-003 authorizes explicit planner-authored coordinates.
- `systems/system-specs/wiring-trace.md:41` carries D-3 **one grid cell per directional step**; `systems/interaction-rules.md` §1-3.4 requires each spec to define its step. Neither evidence supplies a numeric fine step.
- `worldview/glossary.md:25`, `:26`, `:30`, and `:31` establish the existing labels 당직실, 제3수문, 부두사무소 and the three wired stations.

## Independent deterministic verification

[OBSERVED · 2026-09-10] Ran a read-only Python standard-library check via `rtk proxy python3` from the repository root. **16 assertions passed; 0 failed; exit 0.** These checks were run independently of the planner/QA receipt. No random sampling; seed: N/A. No files or runtime tables were changed by the check.

| Property | Required / measured result |
|---|---|
| Schema surface | Only circuitOverlay; only gridStep, fineGridStep, initialOffset, anchors; anchor fields only anchorId, displayNameKo, target, overlay |
| Anchor identity | Exactly 3 anchors; 3 unique IDs; exactly the 3 canonical labels |
| Point validity | Finite integer coordinates; 3 distinct target points; 3 distinct overlay points |
| Noncollinearity | Target and overlay signed determinants both **6**, therefore nonzero |
| Existing coarse step | gridStep = **1** |
| Precision | fineGridStep = **null**; no numeric precision invented |
| Initial state | Offset **(0,0)**; **0** corresponding points aligned |
| Unique common translation | Each target − overlay = **(−1,+1)**; all **3** align at that offset |
| Derived shortest path | **2** cardinal grid moves from the initial offset |

The 16 assertions cover root shape, overlay shape, anchor shape, exact count, unique IDs, canonical labels, distinct targets, distinct overlays, target noncollinearity, overlay noncollinearity, finite integers, coarse step, null fine step, initial state, shared solution, and minimum cardinal moves.

Reviewed JSON SHA-256: `2e3ea0a1aeadb913c46c794d27f38ae49a45849d56fa4ecb895043cf457be9df`. A source change requires rechecking this ACK.

Reproduction of the geometric result:

```bash
rtk proxy python3 - <<'PY'
import json
from pathlib import Path
d = json.loads(Path('_workspace/current/planning/t0-circuit-overlay.json').read_text())['circuitOverlay']
a = d['anchors']
points = lambda key: [(v[key]['x'], v[key]['y']) for v in a]
t, o = points('target'), points('overlay')
det = lambda p: (p[1][0]-p[0][0])*(p[2][1]-p[0][1])-(p[1][1]-p[0][1])*(p[2][0]-p[0][0])
offsets = [(x[0]-y[0], x[1]-y[1]) for x, y in zip(t, o)]
assert len(a) == 3 and len({v['anchorId'] for v in a}) == 3
assert {v['displayNameKo'] for v in a} == {'당직실', '제3수문', '부두사무소'}
assert len(set(t)) == len(set(o)) == 3 and det(t) == det(o) == 6
assert offsets == [(-1, 1)] * 3
assert d['gridStep'] == 1 and d['fineGridStep'] is None
assert d['initialOffset'] == {'x': 0, 'y': 0}
assert all(x != y for x, y in zip(t, o))
print({'solution': offsets[0], 'determinants': [det(t), det(o)], 'minimumCardinalMoves': 2})
PY
```

## Balance boundary

- **No balance defect found in the authored coordinates.** This is first authoring of missing diagram geometry, not an evidence-triggered retune of established play numbers.
- No new reward, cost, timing, precision, attempt limit, or movement-bound tuning is present. Existing campaign minutes, hint criteria, and first-interaction targets are unchanged by this packet. Metadata retains cost-free cancellation/retry and introduces no movement upper bound.
- The two-move path is a mathematical property, not a claim about completion time, learning, puzzle difficulty, fun, accessibility, or human success rate. No new balance band is inferred from it.
- No reward-flow change is proposed; economy's separate dependency ACK is not supplied by this review.
- Runtime import/mirror correctness, absolute-offset behavior, keyboard handling of null fineGridStep, marking completion, saves/recovery, and rendered usability require systems/QA evidence. They are not established by these authoring checks.
- **G2 human/puzzle difficulty validation remains NOT-MEASURED.** This ACK supplies only the authored-coordinate balance dependency; it does not grant a gameplay gate PASS.
