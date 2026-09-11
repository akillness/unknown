---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
---

# M5 reduced-motion immediate-start correction

## Scope

[OBSERVED] Independent review `deleg_0a7dfdd1` found that reduced motion froze the opening and required a manual `작업 시작` activation. This contradicted the M5 acceptance requirement for immediate start. This report records only the bounded correction; it does not supersede prior M5 implementation evidence.

## Change

- `T0OpeningSession.BeginOpening()` now finishes the opening immediately when `ReducedMotion` is enabled.
- `ReducedMotionStartsFreshGameImmediatelyWithoutWrites` asserts fresh-game immediate entry, handover availability, and unchanged journal/save state.
- The existing pointer test now covers a 150% standard-motion opening, rather than retaining the invalid reduced-motion/manual-dismissal contract.

## Evidence

| Check | Result | Receipt |
|---|---|---|
| Focused RED | 0/1 failed for expected `OpeningActive == True` behavior | `reduced-motion-fix/red.xml` |
| Focused GREEN | 1/1 | `reduced-motion-fix/green.xml` |
| M5 PlayMode family | 15/15 | `reduced-motion-fix/playmode-m5-green.xml` |
| Integrated PlayMode selection | 55/55 | `reduced-motion-fix/playmode-final.xml` |
| EditMode | 36/36 | `reduced-motion-fix/editmode-final.xml` |
| macOS build | `M5_MAC_BUILD Succeeded bytes=353402333` | `reduced-motion-fix/build-mac.log` |

## Limits

- [OBSERVED] The M5 implementation as a whole is not strict TDD: this correction has a focused RED→GREEN record, while earlier M5 behaviors retain the previously documented test-after limitation.
- [OBSERVED] The built native player was launched and remained alive, but desktop capture was unavailable from this session (`computer_use list_apps` returned no interactive app). This is not a new native interaction or visual acceptance result.
- [TARGET] A fresh independent code/evidence review remains required before any commit or push.
- [OBSERVED] Existing generated clips remain PREVIZ; this correction does not embed or relabel them as gameplay.
