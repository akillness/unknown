---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
decision: RFC-CX-004
---

# C1 first-beat runtime

The shipped slice ends at `c1-b1`. `planning/c1-patrol-contract.json` is copied unchanged to `unity/Unknown/Assets/_Project/Resources/C1PatrolContract.json`; the generated T0 tables and their producer receipt remain unchanged. `C1PatrolData.Attach` adds one beat with a nonempty `patrolAccessGranted` requirement. It does not import later campaign beats without executable completion rules.

## Inputs and state

`EnterPatrol` requires completed `t0-b3` and exposes the C1 stage. The initial lighting/reader preview is read from the approved packet and applied only on that command, never over a restored preview. `ObservePatrol` accepts exactly the two authored clue IDs. `SetPatrolBranch` changes only `c1:preview:lighting` or `c1:preview:reader`. `AcknowledgePatrol` changes the candidate source condition. `RecoverPatrol` restores the authored safe preview and records bypass usage without granting observations, acknowledgement, journal entry, access, or checkpoint.

`ConfirmPatrol` requires both observations, the authored valid branch combination, the exact current preview values, and the source condition candidate. All other combinations return the packet's cause. One command applies both committed branch values, the source-condition journal fact, gate access, and `cp-c1-b1`. Completion derives from this transaction. No T0 state keys or command meanings are repurposed.

The app's immediate command API rejects `ConfirmPatrol`. `RequestConfirm`/`CommitAsync` is the app persistence boundary: preview, required confirmation gesture, precommit checkpoint, candidate atomic write, publish candidate, accepted success receipt. Failure and cancelled/stale attempts do not publish chapter effects. Retrying commits once. Undo/redo and replay operate on the whole command. C1 defaults to two-step confirmation; its selectable hold/dialog setting is saved independently from the existing T0 preference.

## Presentation and boundaries

The existing single camera and reserved scene viewport render the approved static C1 panel. Hub render roots are hidden while C1 is active, then restored on return/session disposal; legacy drawer picking is disabled in C1. The panel's lens, blank plaques, and readout are static surfaces; the UI is the authority for branch states and observations. No pressure threshold or sample is invented. C1 success uses the saved UI state; the T0-scoped stamp feedback is not replayed for C1 commands.

The case thread, clues, hints, evidence, and terminal message follow the active stage. The terminal states that the implemented slice is complete and later story is unavailable. Performance remains unmeasured; the panel import audit is geometry/material evidence, not a frame-time result.

## Save compatibility

New saves use schema 2. `AtomicSaveStore.Load` accepts schemas 0–2 and refuses malformed or future versions before backup fallback. A successful supported migration preserves the original file as `.v0.bak` or `.v1.bak`, returns a schema-2 document, and leaves original command logs, snapshots, save identity, and v1 story clock intact. The original file is not rewritten merely by loading. The first normal save writes schema 2. An old T0 reader refuses schema 2, preventing accidental fallback to a stale v1 backup after C1 adoption.

The command payload and snapshot shapes are unchanged. App stage/beat metadata is derived from the candidate journal being saved. C1 bypass/checkpoint progress mirrors the journal state. Save size/entry caps remain the existing data-driven T0 values.

## Verification and telemetry

See `systems/tech-verification/c1-m3-native.md` for commands and observed outcomes. Existing command-log fields and `tCommitReceipt` expose command ID, attempt ID, context generation, and success; the simulation stores the authored branch/clue/condition IDs. No new live telemetry service, gameplay-duration measurement, or performance gate is claimed.
