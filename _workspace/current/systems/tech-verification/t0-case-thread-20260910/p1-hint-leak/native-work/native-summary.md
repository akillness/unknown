---
updated: 2026-09-10
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
---
# P1 generic case guidance — native re-verification

[OBSERVED] Unchanged `Tide.EditorTools.T0ProjectBuilder.BuildMac` exited 0 and logged `T0_MAC_BUILD Succeeded bytes=314141603` at the end of the 2026-09-10 13:46:30–13:48:14 UTC invocation. `build-mac-command.json` and `build-mac.log` retain the exact command, input hashes, timestamps and output. The build ran in task-owned `/tmp/t0-case-thread-p1-native-20260910/build-project`, created through four recorded `rtk proxy cp -cR` commands. All 251 production Assets/Packages/ProjectSettings inputs matched before the diagnostic suffix was appended. The only clone input difference after build was `Assets/_Project/App/T0GameSession.cs`: the byte-exact production source prefix plus `native-driver.cs.txt`. The complete diagnostic file is preserved in `diagnostic-T0GameSession.cs.txt`.

[OBSERVED] Retained native bundle: `unity/Unknown/Builds/T0-case-thread-p1-hint-leak-diagnostic/Unknown.app`. `player-manifest.json` records every retained file and hash; copying the bundle from the clone preserved all bytes. No diagnostic class was added to production runtime, and the existing BuildMac method, scenes, resource approval state, input implementation, renderer-only occlusion implementation and their tests were not edited by this subtask.

## Preserved failed receipts and recovery

1. [OBSERVED] The initial build wrapper returned 1 after **Unity BuildMac itself passed with exit 0**. Its strict post-build enumeration saw the concurrently running original-project PlayMode runner's temporary `Assets/InitTestSceneb0ff8234-7e50-45b7-ae4f-b5d242a383c6.unity` and `.meta`. No one of the 251 original input hashes changed. The original `build-mac-command.json` remains unchanged with `productionInputsUnchanged: false`; `post-build-check-failure.json` records the immediate diagnostic filenames/hashes and later empty delta. After the parent's PlayMode process finished and Unity cleaned its files, `resume-retention.py.txt` rechecked all inputs, the exact diagnostic prefix and the sole clone source delta. `retention-after-test-cleanup.json` records successful equality and bundle retention. No source intervention or rebuild was needed.
2. [OBSERVED] `native-1` reported false at `first actual citation committed once`. This was a real failed save: `native-1-player.log:5310` says `T0 commit: Disk full. Path /tmp/t0-case-thread-p1-native-20260910/native-1-save/save.json.tmp`. The retained save remains at sequence 25 before citation; the focused generic-text checks through confirmation passed. `native-1-launch.json`, `native-1/native-result.json`, screenshots and `native-1-save/` preserve this failure. The assertion and production behavior were not weakened.
3. [OBSERVED] After confirming the retained bundle hashes and saving the exact diagnostic source, this subtask removed only its own temporary clone. `clone-cleanup.json` records the exact resolved directory and operation. Measured free space rose from 148,725,760 to 519,213,056 bytes. Existing bundles and unrelated files were untouched. The unchanged bundle and unchanged driver then ran with a fresh `native-2-save` directory.

## Passing native execution

[OBSERVED] `native-2` passed **482 driver assertions across 11 state snapshots**, with **13 screenshots**, on Metal / Apple M2 Pro. Its foreground LaunchServices command exited 0; PID 11138 was observed and was absent after the result. The monitor measured 11.03 seconds, within the external 300-second bound; the driver also has a 240-second deadline. LaunchServices does not return the player's process exit code, so none is claimed. Exact argv and timestamps are in `native-2-launch.json`; the raw checks and action trace are in `native-2/native-result.json`. `native-2-player.log` has no matched `Exception`, `T0 commit:`, `T0 autosave:`, `NullReference` or `MissingComponent` entry.

```sh
rtk proxy python3 _workspace/current/systems/tech-verification/t0-case-thread-20260910/p1-hint-leak/native-work/build-native.py.txt
rtk proxy python3 _workspace/current/systems/tech-verification/t0-case-thread-20260910/p1-hint-leak/native-work/resume-retention.py.txt
rtk proxy python3 _workspace/current/systems/tech-verification/t0-case-thread-20260910/p1-hint-leak/native-work/run-native.py.txt native-1
rtk proxy python3 _workspace/current/systems/tech-verification/t0-case-thread-20260910/p1-hint-leak/native-work/run-native.py.txt native-2
```

| Observed state | Progress | Exact next-action text |
|---|---|---|
| Start, initial hub, open intake document | 0/2 | 기록을 읽고 근거를 살펴보세요. |
| After actual `t0-b1` completion | 0/2 | 근거를 대조하고 정렬해 보세요. |
| After actual `t0-b2`, reader entry, first prepared citation, confirmation | 0/2 | 근거를 살펴보고 인용을 고정하세요. |
| First committed citation, second prepared citation | 1/2 | 근거를 살펴보고 인용을 고정하세요. |
| Full live `Simulation.IsComplete(state, "t0-b3")` completion | 2/2 | 고정한 근거를 검토하세요. |

[OBSERVED] The diagnostic oracle is a fixed allowlist of the five generic localized values (`expected-guidance.json`, including the wait value). Every measured card snapshot rejects all current authored record IDs and Korean display names, concrete source-medium names, authored H-offset/time-window forms, selected hidden narrative phrases and the solution offset action IDs. This snapshot run exercises the four settled-state guidance values; it does not claim a separately captured transient saving-state screenshot. The parent PlayMode checks cover the production projection separately.

[OBSERVED] The driver preserves the prior native mixed input flow: virtual InputSystem keyboard Start, Tab navigation, occluded drawer navigation and second citation confirmation; virtual mouse visible drawer selection, blocked drawer selection, disabled-renderer recovery, document open and first record-line action. Other intake/circuit/reader steps use explicitly labeled existing `Interface.Activate` adapters. No direct simulation commit or fabricated puzzle state is injected. The renderer-only fixture reuses the existing mesh/material, has zero Colliders, is active/in-frustum, and lies closer on the actual ray (0.21967639 vs drawer 2.76798081). Mouse selection is blocked while the renderer is present, restored when disabled, and keyboard access remains.

[OBSERVED] At each of the 11 snapshots, repeated Render preserves journal identity, state hash, head, branch, entries, snapshots, loaded record, successful receipt count, pending flag, surface and every save file's byte count, SHA256 and modification timestamp. It preserves action IDs and keyboard focus; the card is not a Selectable or raycast target. Final live completion agrees with a validated decode of the actual persisted journal in `native-2-save/`.

[OBSERVED] The initial, post-`t0-b2`, first-citation, completed and both drawer screenshots were visually inspected at 1280×800. The four card lines fit inside their panel; its next-action line is generic at 0/2, 1/2 and 2/2. Existing navigation and content controls remain outside the card panel. Each snapshot also checks preferred text height against the allocated card height.

[INFERENCE] These are automated diagnostic native checks with virtual devices and labeled UI adapters. They do not establish human play, physical device/gamepad coverage, accessibility certification, performance, puzzle comprehension, fun, or any final production gate. No commit or push was made. The prior independent no-ship finding and earlier receipts remain the parent's evidence history; this subtask reports only fresh build/native verification.
