---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-qa
---

# M5 intro and gameplay direction — independent QA

**PASS — bounded M5 internal-prototype verification.** QA independently verified integrated **36/36 EditMode, 54/54 PlayMode (14 M5 + 40 baseline), 1/1 boot**, the ordinary build, all 316 app files, 28 canonical source hashes and three final native screenshots/saves. Native inputs/captures were performed by the director; QA inspected the resulting evidence. Diagnostic and approved-baseline 36/53/1 receipts remain historical. The 14 broad criteria below retain their exact coverage limits; the earlier 18 static checks and 13 authored declarations are separate counts.

Scope: read-only skippable fresh-game intro and persistent C1 operation orientation, derived from separately labelled generated previz. Baseline is `6514f549ac7b2e71846005c75a4ff9227882c47c`; root/systems use `/tmp/unknown-m5-video-runtime-20260911`. QA has not launched Unity, run native input, edited code or written the index.

## Input authority and static snapshot

- Reviewed `presentation/intro-gameplay-m5.json`, its Markdown and metadata, source-still provenance, r03 repair prompt and actual gameplay-video request prompt. At the06:39 static snapshot the packet was draft. At07:00 its Markdown/metadata are current with director-approved3125/2875ms native phases and exact asset bindings; this approves design, not native acceptance. The earlier18 checks remain bound to their recorded input hashes.
- Current canon/input basis remains `systems/interaction-rules.md`, `handoff/codex-unity-brief.md`, `planning/campaign.json`, `worldview/timeline.md`, `worldview/worldview-bible.md` and `concept/style-guide.md`. Earlier read-only preparation confirmed current canon metadata and archived supersession targets. `presentation/video-study.md` and `systems/game-ui-contract.meta.md` are draft references, not independent acceptance authority.
- Static receipt: [intro-gameplay-m5-static-review.json](intro-gameplay-m5-static-review.json), including command/session,18 per-check outcomes, exact input hashes and PNG dimensions. No generated pixels or video frames were independently visually inspected in this static pass.

| Static review group | Observed result |
|---|---|
| Scene chronology and timeline | Two distinct scenes; authored intervals contiguous; fixed node-cut/no-motion declarations consistent |
| JSON/Markdown/provider prompt | Both JSON scene prompts appear exactly in Markdown; actual gameplay request prompt matches JSON |
| Reference associations | Every still ID resolves; intro references only hub; C1 still explicitly forbids fresh intro/end-frame use |
| Read-only/accessibility declarations | Zero skip/hold/transition delay, resume bypass, explicit replay, settings, reduced-motion fallback, no simulation/save/evidence writes are declared |
| Source receipts | Both completed still prompt/output hashes, byte sizes and actual1672×941 dimensions match provenance; requested2048×1152 is recorded separately |
| Approved reference | C1 source reference hash matches `docs/media/c1-signature-reader-native-r02.png` |
| Runtime eligibility | At the06:39 static snapshot, both completed source stills and packet defaults were false; later prototype promotion is documented separately |

These outcomes validate the recorded declarations and associations. They do not prove that the produced media obeys its prompt, that controls work, or that the game is usable.

## Regression matrix

| ID | Required outcome | Author mapping | Required evidence | Status |
|---|---|---|---|---|
| M5-R01 | Fresh intro: natural timeout, immediate skip, late skip and explicit replay return to the same authorized gameplay state; one transition only. | P05/P08 | PlayMode + native | BOUNDED · timer/skip/replay tests + F01; late skip not separately timed |
| M5-R02 | Consume pointer/keyboard/back skip input through release; no click-through, duplicate command or held-key activation on the next screen. | P05/P13 | Input regression + native | BOUNDED · held-cut/timeout and pointer tests |
| M5-R03 | Keyboard-only skip, settings and play entry work from visible focus; no required hold or pointer-only route. | P05/P07 | Input regression + native | BOUNDED · keyboard tests and director native sequence |
| M5-R04 | Persisted reduced motion applies from first frame: one static frame, all labels, immediate explicit continue. Toggling it cannot leave a pending timed transition. | P06/P13 | Settings/PlayMode + native | BOUNDED · reduced-motion timer/settings tests |
| M5-R05 | Settings remain reachable before play and during intro; at150% caption, labels, skip/continue and focus are visible or scrollable without occlusion. | P07/P13 | Geometry + native | BOUNDED · 150% geometry and diagnostic screenshot |
| M5-R06 | Fresh start preserves initial canon state; presentation does not auto-observe documents, create clues, grant evidence or publish checkpoints. | P02/P08/P10 | State assertions | VERIFIED · initial-state assertions and F01 no-save |
| M5-R07 | Existing v1/v2/v3, active T0/C1 and completed c1-b2 saves resume without forced intro; retain command identity, backups, checkpoint, two copies and lower mask. | P08/P09 | Save fixtures + OS restart | PARTIAL · native v2/v3; not every schema/state |
| M5-R08 | Open/elapsed/skip/replay/asset failure preserve simulation, command log, story time and persisted save bytes; presentation never executes gameplay commands. | P08/P10 | State and byte comparison | BOUNDED · state/replay-byte/fallback tests |
| M5-R09 | Missing/corrupt still or load failure shows the ink-background fallback with the same controls; playback/network/video completion is not required. | P08 | Fault injection + native | PARTIAL · null-image fault injection |
| M5-R10 | Focus loss/regain, repeated open/close and OS restart do not leave a black/input-blocking overlay, duplicate callbacks or hidden playback. | P05/P08/P09 | Lifecycle + native | PARTIAL · callbacks and completed native restart |
| M5-R11 | Persistent orientation follows selected action and already-visible authoritative status; no focus/scroll reset, new predicate, answer recommendation or animation/timer-driven success. | P10/P12/P13 | Source + interaction/save regression + native | BOUNDED · selection/save tests and F02/F03 |
| M5-R12 | Inspect actual stills and clip frames for pre-t0-b1 intro disclosure, separate c1-b2 chronology, fixed camera, no text/clues/identity/solution; generated bowls/trays/masks never become new game rules. | P02/P03/P04 | Prompt/frame/canon audit | PARTIAL · final intro frame; full movie audit not by QA |
| M5-R13 | Record actual durations, dimensions, rates, models, prompts, reference/output hashes and measured costs; distinguish generated previz, in-game presentation and actual native input proof. | P01/P11/P12 | Provenance + frame/adoption/build receipts | PARTIAL · file/import/native receipts; provider limits retained |
| M5-R14 | Re-run affected T0→C1 progression, save/cancel/retry and150% signature regressions on the isolated M5 source; bind XML/native receipts to the final source manifest. | P05–P13 | Systems XML + source manifest + native | VERIFIED · 36/54/1, canonical hashes and final native evidence |

Save comparisons start after the authorized load/migration baseline is established. Expected v1/v2 migration is not an intro defect; intro/open/skip/replay must not add further gameplay writes or alter preserved source/backup data. Settings preference changes are evaluated separately from simulation and progress saves. Synthetic controller events do not certify physical gamepad operation.

## Open delivery evidence

- Hub r01 provenance records generated cover lettering and horizon mismatch; keep it rejected. Root reports r02 did not fully repair the text and has selected clean r03. The current presentation packet binds r03 explicitly. QA verified the imported resource hash against the source manifest and observed its final native intro application; the generated movie frame review remains director-owned. This does not approve r01/r02 for use.
- C1 source is accepted **by root for previz reference only**. Its extra bowls/tools, hexagonal salt tray and literal generated mask are not runtime resources or mechanics. The game keeps its existing authoritative paper-mask/source rules and approved reader; source association is not a geometry/puzzle approval.
- Root now reports both generated movies complete. Actual durations/cuts/frame review and final provider costs have not been independently inspected by this QA review; a quote is not a final charge. Movies are not imported into runtime.
- Root/director supplied a video-to-runtime binding table with 3125/2875 ms native caption phases and a 1254×1254 dark metal surface derived after inspection of the actual C1 clip at 2.5 seconds. QA verified the imported image/source hashes and final native visual application. The movie's2.5-second source frame itself was inspected by the director, not independently remeasured by QA.
- Integrated approved-profile tests, ordinary build and final normal-launch screenshots/saves are independently inspected: 36/54/1 pass; all316 canonical app files and28 source files match manifests. Diagnostic screenshots and earlier36/53/1 receipts retain their original scope.

## Implementation and targeted-test review

At `2026-09-11T07:04:05Z`, QA independently traced the final source corrections for five findings: Escape/back handling, null-image fallback, replay keyboard context, recovery-slot eligibility and the no-op saved-slots action. All five are **fixed in source**. The 11 targeted test methods directly cover these paths except recovery-retry, which has source review only. Exact source/test/profile hashes and finding-level coverage are recorded in [the code review receipt](intro-gameplay-m5-code-review.json).

Both requested diagnostic gaps now have evidence. The extended 14-case suite includes Enter held across the 3.125-second caption cut and six-second timeout, followed by release without next-screen activation and a successful fresh press. Synthetic focus/pause callbacks stop and resume the clock. The updated 150% test measures caption content `410.8944 <= 523.2525` viewport units at Editor `640×480`; the independently viewed native screenshot shows the full reduced-motion text and both buttons. Other aspect ratios, physical focus-held-input behavior and physical gamepads are not certified.

## Diagnostic evidence inspected at 07:16 UTC

See [diagnostic review receipt](intro-gameplay-m5-diagnostic-review.json) for exact XML/source/screenshot hashes and methods.

| Evidence | Independently verified result | Limit |
|---|---|---|
| Diagnostic XML | EditMode 36/36; PlayMode 53/53 including 14 M5 cases; boot 1/1; no failures or skips | Tests ran under the diagnostic profile |
| Source binding | 24/25 manifest files still match; the sole change is approved profile promotion from false to true | New profile needs fresh suites/build |
| Native intro screenshot | Full 150% reduced-motion copy and both buttons visible; matching settings file has textScale 1.5/reducedMotion true; no progress save exists | Still image does not prove button presses |
| Native v2 screenshot/save | Observation 1/2, humidity 2 risk, Trial highlight, lower mask retained; commands 43–47 are EnterSignature, one observation, two humidity choices and trial | Migration plus deliberate gameplay changed the save; not a byte-preservation test |
| Native v3 screenshot/save | Record saved, 2/2 copies, lower region unresolved; all 57 commands and input/output bytes retained, SHA-256 `214d770689fdbdbef495c59a5587f258b0e039f4d7dfb6190a2fdb5a6c7e6d6b` | Restart/intro-bypass sequence performed by director |
| Native pointer/focus report | Director reports active-app settings clicks pass at 100% and 150%; synthetic pointer test passes | First click after shell screenshot was swallowed once; unreproduced on active-app retry, tracked as M5-O01 observation |

Director approved internal prototype promotion in RFC-CX-007. Current profile hash is `8e99a764023d958b3844a846832bbde556c5ad94618893039fe23b6c61a0c97b`. QA subsequently parsed `editmode-final.xml`, `playmode-final.xml` and `boot-final.xml`: 36/53/1 pass without a diagnostic override in their logs. These are **superseded implementation-baseline evidence**: actual delivery combines upstream `2ece517` plus M5 and requires 36/54/1 integrated-final XML and ordinary build. Diagnostic screenshots and earlier baseline receipts retain their original scope.

## Integrated publication evidence

At `2026-09-11T07:19:32Z`, QA independently parsed `editmode-integrated-final.xml` (36/36), `playmode-integrated-final.xml` (54/54) and `boot-integrated-final.xml` (1/1). All have zero failures/skips; all three logs point to the isolated integrated project without `--m5-direction-diagnostic`. The additional upstream test `ObservedInvalidRoutingExplainsContradictionWithoutPrescribingAction` passes. Exact hashes and outputs are in [the integrated review receipt](intro-gameplay-m5-integrated-final-review.json).

All five reported code findings have corrected source and integrated regression evidence. At `2026-09-11T07:23:29Z`, QA independently verified the final BuildMac success log, all 316 app files (353,421,261 bytes), all 25 owned plus three upstream source hashes, and the exact 8 tracked/17 new source closure. The approved profile is true and app fingerprint is `484ab029f0bf29b4af6ffdd2bb9587ca6e08a432e09dde979d3635517b84d736`. Final normal-launch native evidence was independently inspected at `2026-09-11T07:34:02Z`: first intro frame/controls visible and no progress save; actual low-humidity C1 completion contains54 commands and exactly one ConfirmSignature; completed57-command restart save is byte-identical to its fixture. This evidence does not pass human playtest, immersion, performance, audio or physical controller gates.

Final native evidence is in [native-acceptance.json](../systems/tech-verification/intro-gameplay-m5/native-acceptance.json), specifically its `finalOrdinaryBuild` section; top-level fields retain the earlier diagnostic build. QA verified all three final screenshot hashes and independently viewed them. The native v2 result hash is `c1ca60aca948d8e2008165796f25b7c1978135a6de860b4eb12f18d292162433`; the native v3 restart remains `214d770689fdbdbef495c59a5587f258b0e039f4d7dfb6190a2fdb5a6c7e6d6b`. Canonical workspace app files and source files match the tested manifests. The native movies are root-owned gallery/encoding artifacts; this QA pass does not claim to have reviewed the completed movie edit.

## Rights and claim boundaries

See [M5 rights review](intro-gameplay-m5-rights-review.md) and the [official OpenAI Terms of Use](https://openai.com/policies/terms-of-use/) (effective2026-01-01; reviewed2026-09-11). The OpenAI output-ownership clause is verified; actual GTI image-model identity, backend support and contract applicability remain unverified. Record `gpt-6-astra` as the requested orchestration model, not an observed image-generator identity. Unknown cost remains unknown.

Generated Higgsfield clips remain **previz, not runtime or engine gameplay**. Any native capture must identify actual app/build/source and observed input. No final resource promotion is made by this QA preparation. Human archetype playtests, comprehension, immersion/G4, performance/G5, audio and physical controller certification remain NOT-MEASURED. The six-second native clock has integrated PlayMode and director native-smoke evidence. Generated clip timing is not independently remeasured here; immersion targets3/4 remain targets.
