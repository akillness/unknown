---
updated: 2026-09-18
cycle: 20260918-content-update-m26
status: current
supersedes: null
owner: game-qa
---

# QA loop ledger (3 h cadence · `scripts/qa-loop.sh`)

One row per cycle, appended by `scripts/qa-loop-report.py`. Verdict GREEN = all suites present, 0 failures, build OK. SKIPPED rows carry the reason. Digest is the QA build (`Builds/qa-loop/Unknown.app`), not the release artifact.

| run (UTC) | HEAD | dirty | editmode | playmode | boot | build | digest | Δ vs prev | verdict |
|---|---|---|---|---|---|---|---|---|---|
| 20260919T133921Z | `19db780bba14` | 86 | - | - | - | - | - | - | **SKIPPED (dry-run)** |
| 20260919T133959Z | `19db780bba14` | 88 | 65/65 | 139/141 (1F) (1S) | 0/1 (1F) | OK 444,954,067B | `3a7caea3` | first | **RED** |
| 20260919T134955Z | `19db780bba14` | 91 | - | - | - | - | - | - | **SKIPPED (dry-run)** |
| 20260919T135101Z | `19db780bba14` | 91 | 65/65 | 141/142 (1S) | 0/1 (1F) | OK 444,954,067B | `a784cf04` | first | **RED·MIXED** |
| 20260919T135355Z | `19db780bba14` | 93 | 65/65 | 141/142 (1S) | 1/1 | OK 444,954,067B | `82cebecb` | +0F/−1F · +0B | **GREEN** |
