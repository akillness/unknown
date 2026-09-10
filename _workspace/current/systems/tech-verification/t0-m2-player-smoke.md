---
owner: production-director
updated: 2026-09-10
cycle: 20260909-preproduction-c7
status: current
supersedes: null
---

# T0 native player smoke — 2026-09-10

The director operated the standalone macOS player through CUA mouse and keyboard input. The T0 hub → circuit → reader path completed, then survived an actual OS process exit and relaunch. Two visual defects found during this run were fixed and checked in the corrected standalone build.

## Environment and scope

- Unity 6000.5.6f1, macOS native Metal player, windowed 1280 × 800 launch arguments. CUA returned 640 × 432 window captures including title bar.
- Baseline application: `unity/Unknown/Builds/T0-mac/Unknown.app`, successful build receipt 314,104,679 bytes.
- Corrected application: `unity/Unknown/Builds/T0-mac-framing/Unknown.app`, successful build receipt 314,105,987 bytes.
- Isolated save directory: `/tmp/unknown-t0-player-smoke-20260910-root`. No normal player save was modified.
- This is OS-level UI automation of the actual player, not a human usability study or physical gamepad test. Screenshots were observed inline in CUA; the separately retained Unity diagnostic renders are not labeled as player screenshots.

## Executed baseline route

1. Start duty; read the three required handover lines and transport-list lines, including the written fourth-row observation; place plate #0 in the temporary workbench slot. Header advanced to `t0-b2`.
2. Open wall circuit map, trace duty-room system, begin overlay, align to `(-1,+1)`, and fix three anchor points. The reset control was also exercised and alignment repeated.
3. Mark all three outdoor areas and attach the duty-rules copy to each. Header advanced to `t0-b3`.
4. Open reader, select standard plate, create verification copy through the pointer Read action, select `H−1:00` to `H+3:00`, cite and confirm/save.
5. Replace with tide ledger, create verification copy, use the same `H−1:00` to `H+3:00` window, cite and confirm/save. The player header displayed `T0 완료`.
6. Increase text through settings to 150%. Visible controls remained readable and scrollable. Tab navigation followed by Return toggled reduced motion from false to true.
7. Close the native window. A process lookup returned exit 1 with no matching player process. Relaunch using the same isolated save directory, observe `T0 완료` and enlarged text, then choose Continue. Hub returned with `t0-b3 · T0 완료`.

Keyboard `x` was attempted while reading, but this observation did not establish a keyboard Read binding. The positive keyboard claim is limited to Tab/Return settings operation.

## Defects and corrected-build checks

- Drawer camera originally centered its subject behind the work panel/navigation. The corrected camera uses the unobstructed upper-left scene viewport. In the corrected standalone player at 150% text, the drawer front and handle were visible within this viewport.
- The reader waveform originally drew through the header after scrolling. SignalChart and AnchorDiagram now participate in UI masking. In the corrected standalone player, the restored ledger waveform was visible in the work panel; after scrolling to the citation/replacement controls, no waveform strokes leaked into the header.
- The corrected build restored the same completed save and settings, continued into the hub, opened the drawer node, and opened the ledger with `H−1:00 → H+3:00` preserved.

The complete gameplay route was run in the baseline build; the corrected build received the targeted visibility, clipping, and save-compatibility checks above. Automated receipts are in [t0-m2-native.md](t0-m2-native.md), including the two added regressions.

## Evidence and limits

Native player logs and isolated final save snapshots are retained under `t0-m2/player-smoke/` with hashes and a metadata sidecar. They support process/runtime/save inspection; the detailed UI observations above are the director's CUA session record.

No measured 25-minute play duration, frame-time/GPU budget, human UX result, physical gamepad validation, or campaign G4/G5 approval is claimed. The short confirmation VFX was not independently timing-measured in this UI smoke. Higgsfield stamp audio remains generated but disabled pending auditory review; MuAPI generation remains unavailable without authenticated integration.
