---
updated: 2026-09-10
cycle: 20260909-preproduction-c7
status: current
owner: game-vfx-artist
supersedes: null
---

# T0 VFX contract validation receipt

`[OBSERVED, 2026-09-10]` Commands ran from `/Users/jangyoung/orca/unknown`. Initial checks below are static design-contract checks. The later native PlayMode receipt review is recorded separately. No performance scene capture, frame-time result, target-hardware measurement or human readability result was produced by this VFX task.

## Validator

```sh
python3 /Users/jangyoung/.agents/skills/game-vfx/scripts/validate_vfx_spec.py _workspace/current/vfx/vfx-spec.json
```

Exit **0**; stdout: `valid: particles=0 draw_calls=1`. Log: `/tmp/unknown-t0-vfx-validation.log`.

The requested `--self-test` attempt exited **2** with `error: required: spec`; this installed validator exposes only a positional `spec` and optional `--json`, and ships no separate self-test. This is not reported as a passing built-in self-test. Log: `/tmp/unknown-t0-vfx-selftest.log`.

An isolated Python import of the installed validator's `validate()` API then accepted the packet and rejected six copied mutations: duplicate layer ID; overlapping phases; 501 particles against a 0 particle budget; one layer draw call against a 0 draw-call budget; nonfinite overdraw; disabled reduced-motion fallback. **7 checks passed, exit 0**. No skill files or game code changed. Log: `/tmp/unknown-t0-vfx-mutation-check.log`.

The generic validator checks phases, layers, budgets and fallback presence. It does **not** execute the extended trigger guards, verify source citations, resolve an audio asset, or prove cleanup/deduplication behavior.

## Source reconciliation

`[OBSERVED]` `animation/anim-list.md:44` fixes `stamp_down` at **300 ms**; `:68` places both hit and `seal_confirm` FX at **+150 ms**; `:95-97` authorize entry only after `tCommitReceipt` and forbid failure/stale-callback playback. `vfx-budget.md:36` is interpreted as shared envelope start, with visible ink at the authored keyframe. Director ACK of this interpretation was received through team message on 2026-09-10. The 900–1400 ms/default 1200 ms water animation is excluded. `concept/style-guide.md:30` supplies confirmed ink `#173238`.

## Native T0 receipt and clock verification

`[OBSERVED, 2026-09-10]` Reviewed the durable `systems/tech-verification/t0-m2/results/playmode-5.xml`: test-run **12 total, 12 passed, 0 failed**, `result="Passed"`, UTC start `2026-09-10 08:48:58Z`, end `08:49:06Z`. The companion `systems/tech-verification/t0-m2/logs/playmode-5.log` records `Test run completed. Exiting with code 0 (Ok). Run completed.` No Unity run or code edit was performed by the VFX reviewer.

Reviewed exact source `unity/Unknown/Assets/_Project/Tests/PlayMode/T0PlayModeTests.cs:59-74` and the passing XML test `ConfirmationInkUsesPresentationClockAndClearsOnContextExit`. It sets and restores `Time.captureDeltaTime=.025f`, yields real Unity frames, and checks actual component state against its elapsed presentation clock. It observes both the pre-impact interval and ink interval; ink is absent below 150 ms, present from 150 ms until before 300 ms, and cleared at envelope completion. Frame-stepped completion is allowed from 300 ms through 325.1 ms by the test. That bound and the 25 ms step are deterministic test tolerances/input, **not an observed frame-time or performance claim**. Failed/duplicate/reduced-motion presentations emit no new ink; UI `Click("start")` triggers real `ScreenChanged` cleanup.

Reviewed `Presentation/T0CommitFeedback.cs:17-23`: success/id/deduplication guards precede `Present`, reduced motion skips playback, `Update` advances `Time.deltaTime`, ink visibility uses the supplied impact/duration bounds, and `Clear` disables playback and the mark. `App/T0GameSession.cs:49-51` initializes timings from `T0Vfx` JSON, binds successful receipts and screen changes; `:88-97` checks current save attempt, cancellation and context before emitting the success event. The same 12-test receipt passes `PendingAllowsOverlayUndoAndSuppressesStaleReceipt` and `SaveFailureNeverEmitsSuccessAndRetryEmitsOnce`. Test coverage plus this binding inspection establishes the scoped T0 receipt/clock use; it does not measure appearance or GPU behavior.

The actual T0 mark is procedural HUD geometry at anchor `(0.955, 0.15)`, size `48 × 42` UI units, overlay sort order `30` (`Presentation/T0CommitFeedback.cs:14-15,25-27`). Director approval received in the review assignment authorizes **this exact visual only** after receipt confirmation. `vfx-spec.json` therefore records `runtimeEligible:true` for that scoped visual. The separately generated Higgsfield audio candidate stays **disabled / `runtimeEligible:false`**; no audio audition or playback test is implied.

## Remaining checks

- `[TARGET]` Dedicated pause/unload/load-replay effect lifecycle tests, low-quality behavior, and muted/missing audio behavior. Successful receipt, pending/stale/failure, duplicate, reduced-motion and context-exit visual coverage is now recorded above.
- `[TARGET]` Separate audio provenance, audition and asset-specific audit before any audio activation. Other effects and generated visual candidates receive no eligibility from this T0 procedural mark review.
- `[TARGET]` QA capture of frame/GPU time, particle/draw-call/overdraw peaks and per-frame allocations. Current runtime measurement count **0**; G4/G5 unclaimed.

`[OBSERVED]` `git diff --check -- _workspace/current/vfx` exited **0**. Only new VFX packet/receipt files were authored. Repository-wide graph and memory synchronization remain the director's integration responsibility; no code was changed here.
