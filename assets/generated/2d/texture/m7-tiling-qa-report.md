# M7 Texture Tiling QA Report

Candidates only (`runtimeEligible:false`). Values are [OBSERVED] mean absolute
pixel deltas across the wrap edge (0-255 scale, lower = more tileable).

| material | seamDeltaX | seamDeltaY | repeatPeakPx | verdict |
| --- | --- | --- | --- | --- |
| bronze | 16.49 | 18.93 | null | needs-edge-blend |
| perforated-steel | 10.63 | 12.71 | null | needs-edge-blend |
| salt-concrete | 20.03 | 67.76 | 418.0 | not-tileable |
| salt-crystal | 10.51 | 10.1 | null | needs-edge-blend |
| rag-paper | 8.53 | 10.57 | null | needs-edge-blend |

Verdict rubric: seamDelta <= 8 both axes = tileable-candidate; <= 20 = needs-edge-blend; else not-tileable.

Untested: in-engine seam behaviour, mip/anisotropy behaviour, normal-map generation,
derived-channel visual correctness (dimensions only), physical tiling scale.
