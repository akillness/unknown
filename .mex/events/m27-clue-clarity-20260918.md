---
name: m27-clue-clarity-20260918
description: RFC-CX-M27-20260918 — requirement checklist as a data contract (labels/captions/steps + validator T0-06), stage marker, data-driven next/guide/C1, hint-offer setting, three Higgsfield checklist glyphs; QA loop caught 3 regressions mid-cycle.
last_updated: 2026-09-18
---
- Data: emit-tables predicates carry label/caption/step; validator --t0 T0-06 (6/6); t0-b1/t0-b2 objectives rewritten; t0-b3 L2 hint states the half-open gap rule.
- Runtime: App/M27ChecklistSession.cs + UI/M27ChecklistInterface.cs; card .185→.15 + band .045; guide task section; C1 from packets; teaching header/subtitle = stage; hint-offer setting; no new beat literals.
- Loop: 20260919T163626Z RED (3) → fixes → 164126Z/164740Z GREEN. Release 445,068,423 B, v0.27.0-dev.
- Lesson: python literal replace turned a test helper into self-recursion → SIGSEGV at editor start; stale Temp/UnityLockfile after a crash kills the next run.
