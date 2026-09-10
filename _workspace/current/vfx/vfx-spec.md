---
updated: 2026-09-10
cycle: 20260909-preproduction-c7
status: current
owner: game-vfx-artist
supersedes: null
---

# T0 save confirmation effect packet

`vfx-spec.json` is the machine-readable integration contract for one reusable effect, `seal_confirm`. It derives from the existing six-effect library in `vfx-budget.md` §1; no additional world or combat effect is introduced. `[OBSERVED, 2026-09-10]` The director approved the existing T0 procedural ink/checkmark after native PlayMode receipt and source review; `runtimeEligible:true` applies only to that visual and its verified receipt/clock path. Audio remains disabled and ineligible. All performance budgets remain `[TARGET]`, performance/readability capture count is **0**, and G4/G5 are unclaimed.

## Authored timing and sources

| Meaning | Value | Authoritative reference |
|---|---|---|
| `stamp_down` presentation envelope | 300 ms, fixed | `animation/anim-list.md:44` |
| Ink FX and hit marker within envelope | +150 ms | `animation/anim-list.md:68` |
| Only permitted entry | Current successful `tCommitReceipt` | `animation/anim-list.md:95`; `systems/interaction-rules.md:315` |
| Failed or stale callback | 0 stamp / 0 confirmation sound | `animation/anim-list.md:96-97`; `vfx-budget.md` §1 |
| Confirmed ink | `#173238` | `concept/style-guide.md:30` |

The 900–1400 ms/default 1200 ms value belongs to **`water_level` / `water_rise`**, not `seal_confirm`. Water FX are excluded from this packet; ownership RFC-A4 remains unresolved. The same-frame start language in `vfx-budget.md:36` refers here to starting the receipt-authorized stamp envelope; the visible ink marker follows the exact +150 ms animation keyframe. No gameplay timing was added.

```yaml
id: seal_confirm
trigger: tCommitReceipt accepted by the current App save adapter
presentation_intent: communicate a durably saved confirmation
anticipation_ms: 150 # authored stamp envelope before its impact marker
impact_ms: 150 # offset from receipt-started envelope, not a new duration
dissipate_ms: 150 # remaining envelope; static ink, no animated decay
layers: [confirmed_ink]
palette_ref: concept/style-guide.md:30
particle_budget: 0
overdraw_budget: 1 # proposed local UI layer ceiling, not measured
sound_cue: SFX_stamp_down # optional; separate asset audit required
```

## App integration boundary

`[OBSERVED, 2026-09-10]` Systems explicitly ACKed the adapter contract `tCommitReceipt(attemptId, commandId, contextGeneration, success)`: only the current successful save attempt emits once. The actual `CommitReceipt` fields are `AttemptId`, `CommandId`, `ContextGeneration` and `Success` (`App/T0GameSession.cs:20-21`); `:49-51` binds the receipt to `T0CommitFeedback.Present` and binds UI `ScreenChanged` to `Clear`. The save flow checks current attempt/cancellation and context before emitting success (`:88-97`). Sim remains outside this presentation binding.

`[OBSERVED, 2026-09-10]` `presentation/t0-circuit-vfx-review.md` ACKs the 300 ms envelope and +150 ms impact marker. Native `playmode-5.xml` now reports **12/12 PASS**, and its log records runner exit **0**. `ConfirmationInkUsesPresentationClockAndClearsOnContextExit` exercises the real Unity `Update` / `Time.deltaTime` presentation clock: no ink before +150 ms, visible ink from +150 ms until before 300 ms, then clear; failed/duplicate/reduced-motion presentations produce no new ink, and a real UI screen change clears active ink. Existing save-success/stale/failure tests also pass. The fixed **25 ms** `Time.captureDeltaTime` is test input, **not measured frame performance**. Exact receipts and coverage limits are in `vfx-validation.md`.

`[OBSERVED]` The approved T0 implementation is a procedural two-stroke HUD checkmark (`Presentation/T0CommitFeedback.cs:14-15,25-27`) at normalized anchor `(0.955, 0.15)`, size `48 × 42` UI units, screen-overlay canvas sorting order `30`. It is not an imported/generated stamp image or a physical stamp animation; the authored stamp timings provide this confirmation variant's clock only. This exact variant is recorded in the JSON `runtime_implementation`; the result-panel placement and clipping below remain design targets, not a claim of measured runtime readability. The visual approval does not promote other effects, audio, campaign release or performance gates.

Start the 300 ms shared presentation clock only after that acceptance. At +150 ms show the static ink mark. `SFX_stamp_down` remains a future, separately audited cue; the current T0 candidate is disabled and no audio playback is approved here. Clear the transient layer at envelope end; UI retains the accessible result state. Leaving the result context invalidates pending presentation tokens and clears transient playback, without reversing committed state. Pause freezing, loading/recovery and historical replay suppression remain lifecycle requirements; this receipt does not establish dedicated pause/unload/audio test coverage.

`SavePending`, save failure, duplicate receipts and stale callbacks produce **0 new emitters, 0 particles, 0 VFX draw calls and 0 confirmation sounds**. They do not globally lock input. Reduced motion and low quality skip the transient stamp and use the existing static success label/checkmark at receipt acceptance; the same gating remains. Audio may use that static confirmation event once if enabled and eligible.

## Readability and budgets

Only the result panel may host the ink layer; clip it behind its text and away from tool lines, focus indicators, citation content and controls. The existing UI owns the localized state labels and non-color checkmark/line distinction; the packet creates no new UI state strings. No enemy/player or danger telegraph is in T0 scope.

Scene caps remain **8 simultaneous emitters / 500 transparent particles**, while this effect uses **0 emitters / 0 particles**, at most one instance and one local UI draw call. Max local ink overdraw 1 is a conservative authored ceiling for the new single-layer representation, not a measured result. Bloom, screen flash, blur and default camera shake are 0. Preallocate the single layer; suppress repeat emission instead of growing a pool. Any measured budget violation requires redesign.

## Verification handoff

Run the skill validator using the exact command in `vfx-validation.md`. The installed validator has no built-in self-test; the receipt records a separate mutation check. Native PlayMode coverage now establishes the visual receipt/clock behavior listed above. Runtime QA still needs dedicated scene unload, pause/resume, load/replay effect suppression, low-quality and audio behavior checks, plus target-hardware frame/GPU time, particles, draw calls, overdraw and allocations. Performance/readability measurement fields remain `null`; G4/G5 are unclaimed.
