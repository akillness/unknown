---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-qa
---

# C1 M4 independent review — c1-b2 only

[OBSERVED] Delivery QA is complete for the approved c1-b2 slice. Independent source and staged-index checks bind the tested isolated project to baseline `ae098e0636b0075d1abb0ebf408bd66f7f459811` plus the approved 65-file overlay. Isolated native results are **EditMode 36/36, PlayMode 39/39, and separate serialized boot 1/1 PASS**, with no failures or skips. The director’s final2 ordinary-runtime receipt confirms completed-save restoration, resources, both copies, the lower mask and corrected resumed-status copy. QA independently verified the restored save is byte-identical to the confirmed save and contains exactly one `ConfirmSignature`.

These are bounded implementation and agent-operated native observations. They do not promote G4/G5/G6, performance, fun, physical gamepad, audio, human playtest or full-C1 completion claims. QA did not start a competing Unity instance or repeat the director’s CUA route.

## Delivery source and test closure

The isolated project is `/tmp/unknown-c1-m4-publish-20260911/unity/Unknown`. `c1-m4-isolation-review.json` records the independently repeated final post-build audit at 2026-09-11T04:37:05Z:

| Check | Result |
|---|---|
| Approved overlay hashes | 65/65 match tested isolated files |
| Baseline tracked Unity files | 315 accounted for |
| Non-overlay baseline Git-blob mismatches | 0 |
| Extra Assets/Packages/ProjectSettings files | 0 |
| Workspace and isolated manifest copies | Byte-identical |
| Staged approved files versus tested isolated sources | 0 mismatches |
| Other staged Unity paths versus baseline | 0 mismatches; 0 extra paths |

Receipt: `systems/tech-verification/c1-m4/delivery-source-manifest.json`. Sixty-three manifest staging sources are `workspace`; two substitutions use isolated paths. Delivery restores the exact HEAD `PatrolText` localization fallback, excludes the external `ObservedInvalidRouting...` test, and retains HEAD `T0Strings.json`. The authorized `DefaultPatrolGuidanceDoesNotRevealSolutionOrMutateState` regression remains present and passed. Shared-checkout external changes and the mixed-source 40-test run are not isolated-delivery evidence.

During import, Unity cleared only the URP global-settings runtime RID list. Independent installed-package source review confirmed Editor serialization behavior; the authoring settings list was unchanged. Systems restored HEAD, and the final post-build asset matches HEAD exactly. Historical evidence remains in `systems/tech-verification/c1-m4/isolated/urp-runtime-settings-observation.json` and `urp-after-import.diff`; no additional overlay was required.

| Fresh receipt under systems/tech-verification/c1-m4/isolated/ | Result |
|---|---|
| results/editmode.xml | 36/36 PASS |
| results/playmode.xml | 39/39 PASS |
| results/boot-playmode.xml | 1/1 PASS |

QA independently read XML totals, checked their SHA-256 values and confirmed matching isolated-project paths in the corresponding logs. Normal PlayMode has no global save override. Boot is an exact-filtered separate run. Nine EditMode and ten normal PlayMode tests are signature-specific; totals include regressions. The completed-signature resume test asserts the saved contrast-record message and rejects the stale T0 welcome line. The planner’s 17 declared acceptance cases remain a checklist, not a measured 17/17 suite.

## Native route, interaction and restart

[OBSERVED director; receipts reviewed by QA] `systems/tech-verification/c1-m4/player-final-smoke/native-observation.json` records the diagnostic-3 full c1-b2 route at 150% text with reduced motion: introduction and source open at the top; joined, separated, copied, marked, compared and completed masks and paper labels fit the viewport; wrong humidity trial and free reset preserve observations; two explicit copies, marking, comparison and confirmation succeed. Screenshots were inspected by the director in conversation; this review does not invent standalone screenshot files.

`systems/tech-verification/c1-m4/feedback-smoke/native-observation.json` records the ordinary final1 input/trial-card spot-check: one nonfocused click opens plate #0; Return records the selected plate and observations advance 1→2; humidity 2 / trial-not-run and humidity 2 / ink-risk remain visible in the persistent case card while lower controls are focused. Earlier intermittent CUA clicking remains recorded. One bounded successful route does not establish universal input reliability or a causal explanation for the earlier symptom.

`player-final-smoke/restored-save.json` records the director’s final1 OS-process restart. QA verified it equals the confirmed save byte-for-byte. Independent diagnostic-3 save comparison also established that the original 42 command entries remain an exact prefix of the 57 final entries, saveId and createdUtc are unchanged, the v2 backup equals its input bytes, ConfirmSignature changes from 0 to 1, and cp-c1-b2 is added only after confirmation.

`player-final-smoke/final-runtime-verification.json` records final2 ordinary build (348,163,868 bytes; diagnostic flag false), bundle fingerprint `af69e25e5c1f677c9203cee0600ca68998ee25c4d2bbaf2430c1b7b38af51738`. At 150% text with reduced motion, completed state, resources, two copies and the lower mask are retained. The director observed `저장된 대조 기록을 복원했습니다.` and no stale T0 welcome text. An initially inactive black window displayed UI after titlebar activation in the same process/build; no broader startup reliability claim follows. `final2-restored-save.json` is independently byte-identical to `confirmed-save.json` (SHA-256 `214d770689fdbdbef495c59a5587f258b0e039f4d7dfb6190a2fdb5a6c7e6d6b`), with exactly one ConfirmSignature. All delivery-specific native follow-ups are evidenced.

## Acceptance-critical findings

| ID | Severity | Lane | Reproduction / source | Closure evidence | Status | Owner |
|---|---|---|---|---|---|---|
| C1-M4-QA-01 | S2 | UI/narrative | Paper source invented page numeral 1 before observation. | Gated neutral 번호 표기; default-guidance regression | CLOSED | systems |
| C1-M4-QA-02 | S2 | save | Checkpoint summary omitted cp-c1-b2. | Failed-save/retry regression asserts saved c1-b2 and cp-c1-b2 | CLOSED | systems |
| C1-M4-QA-03 | S3 | presentation | Completed patrol copy said next story was closed beside enabled continuation. | Obsolete sentence removed; signature continuation retained | CLOSED (source) | systems |
| C1-M4-QA-04 | S2 | UI/narrative | Same-source footer appeared before explicit copies. | Default-guidance rejection plus both-copy gate | CLOSED | systems |
| C1-M4-QA-05 | S2 | verification | Initial fault test failed checkpoint, not candidate save. | Second BeforeRename failure; original bytes/replay unchanged; retry commits once | CLOSED | systems |
| C1-M4-QA-06 | S2 | UI/presentation | Lower mask and labels clipped at 150% text. | Geometry regression plus diagnostic-3 native source states | CLOSED | systems |
| C1-M4-QA-07 | S2 | UI/presentation | Source introduction auto-scrolled away from top. | ResetScroll after SelectFocus; 150% next-frame regression plus native top position | CLOSED | systems |

Findings were broadcast to director and systems; feedback-requested-by: 2026-09-11. Final source/native closure is reported to both lanes. No S1 findings were opened by this bounded review.

## Static acceptance and evidence history

`c1-m4-static-review.json` preserves the initial **32/32 independent static assertions**, timestamp and input hashes. This is separate from the planner’s 44 static assertions; it is not relabeled as a fresh final-source run. Final source/test hashes are in `c1-m4-native-review.json` and `c1-m4-isolation-review.json`.

- Campaign observations and narrative fields are preserved. No c1-b3/C4 identity or later-beat mechanism is introduced; forbidden-reveal IDs remain guardrails, not player copy.
- Both copies inherit signature-annex. Comparison uses distinct plate-zero; a second copy is not an independent proof source.
- Humidity begins unset. Trial choices have no percentage, cost, time advance, damage or trial limit. Invalid IDs are rejected; reset preserves copies.
- Separation clears adhesion without clearing the unresolved lower region. The normalized mask remains (0.12, 0.70, 0.76, 0.22), converted to Unity anchors (0.12, 0.08)–(0.88, 0.30). No concealed signer text is rendered under it.
- Blender has four static reader material-group meshes. Imported assets do not create optional STATE_SaltAdhesion / STATE_LowerObscuration hooks; functional paper separation, adhesion and obscuration are independent UI states. This review does not claim dynamic 3D paper behavior.
- Confirmation requires observations, two copies, separation, region marking, comparison and explicit proof. Native tests cover immediate rejection, candidate-save failure/retry, second-rename cancellation, duplicate refusal, undo/redo/replay and save-version regressions.

Earlier receipts remain preserved. Whole-suite `playmode-4.xml` used an invalid global save-directory override (6 pass/30 fail), so it is not gameplay regression evidence. `boot-playmode-1.xml` reached startup but its generic Canvas selector matched multiple canvases; the selector was corrected to Interface Canvas. Subsequent startup/layout/interaction runs, including playmode-9, are historical steps; isolated 36/39/1 receipts above are delivery evidence. The separate boot regression checks the actual serialized boot scene, active/enabled Interface Canvas, and visible enabled Start UI, with exclusive ownership and cleanup of its temporary save directory.

Physical gamepad, auditory review, five human playtest archetypes, immersion, performance and full-chapter duration remain NOT-MEASURED. Final visual/resource promotion is the director’s decision based on native observations and provenance.
