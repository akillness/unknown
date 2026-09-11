---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
---

# M5 introductory guidance and C1 operation orientation

- Owner: game-systems-designer
- Baseline: isolated worktree6514f54 plus committed upstream2ece517, Unity6000.5.6f1.
- Resource gate: RFC-CX-007 approved internal-prototype use; runtimeApproved=true, commercial eligibilityfalse.
- Authority: presentation/intro-gameplay-m5.json and the director's measured video review.
- Runtime source: unity/Unknown/Assets/_Project/{App,UI,Presentation,Editor}.

## Inputs and projection boundary

M5DirectionProfile binds the two reviewed still images, authored caption strings, and first/second shot durations 3.125/2.875 seconds. Runtime enablement reads runtimeApproved; --m5-direction-diagnostic permits the unapproved profile only for native diagnostic review. No runtime movie or new package is used. Source PNGs remain byte-identical to provenance. Import caps are 2048 for opening and 1024 for the decorative surface.

T0GameSession reads the existing journal, restored-save status, settings, selected UI action, and accepted-save state. M5 does not add commands, predicates, persisted fields, a migration, or a reward. C1SignatureGameSession preserves the existing trial predicate: the first observation is sufficient to attempt a trial. No unobserved correct humidity is inferred.

## Opening state machine

Fresh title Start enters the opening only before t0-b1 with no restored save. Existing-save starts bypass it. Selecting/retrying an existing recovery slot resets eligibility to false; creating a new recovery slot resets fresh eligibility. Opening cuts caption context at the observed 3.125-second cut and starts normal play at 6 seconds. First-frame Skip and Escape consume their input through the existing Watch.NewContext mechanism. The clock pauses in settings, on application focus loss, and on application pause.

Reduced motion uses one static image, all three operation labels and context copy, and an immediate continue button without advancing a timer. Missing image uses an ink background and the same guide/controls. Settings remains reachable, with an explicit Back route. Saved-slot switching is hidden while opening is active.

Settings offers explicit replay while no save is pending. Replay preserves underlying document/tool and returns to the prior settings overlay. During replay the input context is UI navigation. Undo, redo, tool changes, adjust, preview, query and disconnect cannot write gameplay. Opening/replay/timeout/skip/load failure do not serialize saves or publish evidence. Settings changes retain their existing settings.json behavior.

## C1 operation orientation

The persistent labels 관찰 / 시험 / 기록 follow the actual selected action category. Unmapped controls leave all labels neutral. Highlight changes only graphic colors/font style; they do not rebuild the view or write state. Existing observation, trial, and record controls are grouped without adding a phase gate. Current humidity and last actual trial use existing visible status. Record status distinguishes preparation, candidate ready, save pending, save failure, and accepted completion. Completion reads the accepted journal state, never a clock/video frame.

Nested action groups use content-space coordinates for focused-button scrolling. Camera transforms, lower obscuration, and puzzle state remain under existing owners. Render consumes state snapshots; presentation never mutates simulation state.

## Failure and compatibility rules

- Unapproved profile: M5 disabled unless diagnostic launch flag is explicit.
- Missing decorative texture: flat ink group/strip surface.
- Missing opening texture: readable ink opening with Skip/Settings.
- Save pending/failure/cancel: no accepted wording or effect before existing accepted-save boundary.
- Save v3 remains unchanged; legacy v2 migration remains the M4 path.
- Timing/resource values are profile data, not new gameplay tuning.

## Telemetry and performance

No new telemetry, external network call, video player, or frame-by-frame rebuild is introduced. Intro rerenders on open/cut/end or explicit settings changes. Selection updates only the three existing indicators. Human playtest/G4 and measured performance remain NOT-MEASURED; no performance gate is inferred from PlayMode or native screenshots.

Verification and exact command receipts: ../tech-verification/intro-gameplay-m5.md.
