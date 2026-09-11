---
updated: 2026-09-11
cycle: bootstrap
status: draft
supersedes: null
owner: game-production-director
---

# Decision Log (append-only; RFC-n ids unique)

## RFC-CX-003 · Playable T0 구현 및 원본 리소스 제작

- date: 2026-09-10
- lanes: director, systems, worldview, synopsis, planner, modeling, vfx, QA
- source: 사용자 후속 요청 "make the game based on workspace and develop resource with blender, heiggsfield" 및 ecc/game-vfx/game-sounds/game-ui-ux 지정.
- scope: 기존 preproduction T0(hub, circuit, reader)를 실제 조작 가능한 Unity 슬라이스로 이어간다. 본편 production gate와 사람 검증 경계는 유지한다.
- RFC-CX-001 director ruling: worldview 제안 A 채택. 기존 허브 계통을 `system-hub`, 기록국 표준 관측소를 `station-bureau-standard`로 명시 저작한다. 실제 인용판 `rec-plate-standard-hub`는 두 ID를, `rec-tide-ledger-bureau`는 `systemId:null`과 표준 관측소 ID를 사용한다. plate.stationId 연결은 이번 저작에서 채택한 비교 기준 프레임이며 과거 관측 생산지·새 센서·사건의 발견이 아니다. 비인용 3레코드로 자동 전파하지 않는다. `plates.md`의 systemId는 plate일 때만 필수이므로 ledger까지 비-null을 강제하지 않는다. Worldview 등록→synopsis 귀속표→systems 생성기 파생 및 독립 QA를 거쳐 확정한다.
- circuit authoring: 기존 명세의 3점 AnchorOverlay와 단위 격자에 필요한 좌표는 planner가 명시 저작한다. 코드가 답 좌표를 발명하지 않는다. 전 점이 같은 offset으로 정렬 가능해야 하고 값·출처는 데이터에 둔다. 새 미정 precision step은 발명하지 않는다.
- resources: Blender로 누락 서랍을 기존 SM_Hub_Workbench의 부속 그레이박스로 제작한다. 기존 47-item 자산 신원을 임의로 48개로 변경하지 않는다. GLB 정본과 함께 동일 기하의 FBX를 Unity 임포트용 파생 후보로 허용한다. 기존 소스·provenance를 덮어쓰지 않는다. 자산별 실측/임포트 감사 전 runtimeEligible:false를 유지한다. Higgsfield는 현재 계정/모델/비용을 확인해 원본 게임 리소스 제작에 사용한다.
- skill routing: ECC 2.2.1 installed/enabled는 provider inventory로 확인; 설치/후크 변경 없음. game-sounds는 개발 에이전트 알림 도구로 판정, 런타임 음원 재사용 금지. 게임 오디오는 원본 제작/라이선스 검토 경로를 따른다. UI/VFX skill validator 통과는 디자인 계약이며 프레임 성능 또는 조작 검증의 대체가 아니다.
- decided_by: game-production-director, 사용자 게임 제작 요청 범위 내

### RFC-CX-003 addendum — M2 persisted Snapshot shape (2026-09-10)

Director approves systems proposal for commandLog.snapshots entries: `{seq:long,stateHash:string,stateBlob:{facts:string[],values:map<string,string>,readCounts:map<string,int>}}`. This fills the existing Snapshot[] serialization gap without a new save-root field or gameplay decision. Systems must document canonical ordering/hash, immutable reconstruction and replay-base validation and verify 200-commit/limit-fold behavior using the same representation. Authored citation source decision references remain RFC-CX-001 and RFC-CX-003; no separate RFC-CX-001A is created.

### RFC-CX-003 addendum — drawer r02 material correction (2026-09-10)

Concept ACKs the r01 shape and canonical palette but requests FIX for pristine surfaces, missing downward corrosion and edge salt; see concept/t0-source-drawer-review.md. Director authorizes a new r02 Blender candidate, preserving r01, and permits small baked texture assets to replace the r01 zero-texture blockout target within existing overall resource budgets. Keep canonical dimensions, 2-mesh separation where feasible and the 1500-triangle target; GLB remains canonical and FBX a derived Unity import candidate. Use the existing palette and material rules; no new rust token or gameplay/animation timing. Final style and runtime eligibility still require actual preview/import evidence, not recipe claims.

### RFC-CX-003 addendum — scoped source ACK evidence (2026-09-10)

Two-record provenance: worldview proposal, synopsis §12 and planning ACK are joined by QA in `qa/t0-m2-source-resource-review.md` and concept in `concept/t0-source-drawer-review.md`. The plate association remains a new comparison reference frame; ledger.systemId remains null. Root inspected the regenerated canonical records.json after the QA snapshot: both approved assignments are present and recordsDocSha256 equals the latest synopsis document SHA-256. C7-F50 runtime closure still awaits systems/QA execution.

Circuit packet dependency ACKs received: systems (authoring metadata), QA (`qa/t0-m2-source-resource-review.md`), balance (`balance/t0-circuit-review.md`) and economy (`economy/t0-circuit-review.md`). Presentation review remains pending. No ACK here is a human playtest, runtime asset promotion or campaign gate PASS.

### RFC-CX-003 addendum — circuit authored packet promotion (2026-09-10)

Presentation scoped ACK is recorded in `presentation/t0-circuit-vfx-review.md`. With systems, QA, balance and economy ACKs cited above, director accepts current promotion of the authored coordinate packet. Planner is recording the ACK links/status; JSON coordinates and hash remain unchanged. This is authored-data readiness only, not runtime readability, measured difficulty or G2/G4/G5 PASS.

### RFC-CX-003 addendum — r03 T0 runtime scene approval (2026-09-10)

Director inspected the native Unity Metal audit and all three captures in unity/Unknown/Builds/t0-diagnostics: standalone, original solid interference and front adaptation. The supported materials bind both base-color and roughness maps; the four1024px textures use sRGB for base and linear sampling for roughness. The imported asset remains2meshes/156triangles. Y180 adaptation makes the authored front face the scene camera. Subtracting the drawer AABB from the existing solid workbench restores the front recess distinction, as shown by the comparison captures.

Approve r03 for the T0 scene with the measured adapter yaw180 and drawer position(0,0.32,1.1) plus the reviewed workbench front adaptation. This is a placement/mesh-integration decision, not recovered world evidence, a new production identity or animation timing. Preserve source revisions. The current observed rendering project is Gamma on Metal; this approval does not establish final frame budgets, production G4/G5 or standalone-player appearance. Systems may now activate this exact candidate in the T0 player; root will inspect the resulting player screen.

## RFC-CX-004 · C1 opening patrol circuit (2026-09-11)

- Trigger: user requested continued game development after T0 M2.
- Scope decision: implement canonical c1-b1 only, preserving the existing T0 path and saves. The remaining C1 beats retain their authored status; no full-chapter or measured-duration claim is made.
- Evidence: planning/campaign.json C1/c1-b1; synopsis/chapter-beats.md c1-b1; production/codex-c1-m3-status.md.
- Proposed transaction: preview branch changes without granting access; accepted confirmation durably records gate access, the existing source-attribution condition, and checkpoint cp-c1-b1. Invalid configurations remain explainable and reversible. Planner contract and systems compatibility review are pending.
- Resource: original Blender patrol distribution panel; runtimeEligible:false until visual review. Existing uncommitted renderer/occlusion work is preserved and identified separately.

### RFC-CX-004 addendum — canonical transaction and prop target

Director ACK after planner review: one chapter confirmation/save applies branch state, gate access, source-attribution journal condition and cp-c1-b1 together. Circuit folding remains reversible preview; free bypass recovers a safe preview and never grants progress or replaces observations. This follows the existing wiring/interaction save doctrine and avoids unauthored partial completion states.

The Blender asset `c1-patrol-panel` depicts the existing 수문 계통판 in canonical clue c1-b1-c2; its ID is an internal production name. There is no new lore term. A dedicated concept sheet is absent: for this scoped blockout, the canonical clue, current concept/style-guide.md and director brief define the visual target. This deviation does not approve final concept art or campaign G4/G5.

### RFC-CX-004 addendum — save compatibility and implementation ACK

Director ACKs systems architecture: import a separate validated c1-b1 packet with explicit nonempty requirements; do not append raw campaign beats that could auto-complete. Keep existing T0 command identities and snapshot hash semantics, namespace C1 preview, and derive stage/beat/save metadata from the journal. Add explicit Continue to C1, stage-aware evidence presentation and a clear current-demo endpoint; hub-only drawer interactions remain gated to hub.

Save schema v2 is approved with explicit backup-preserving v1→v2 migration and future-version refusal. Rationale: an older T0 binary must not treat a C1 command as replay corruption and silently fall back to a stale v1 backup. Tests must cover an actual completed prior v1 save copied into an isolated fixture, invalid branch/observation guards, atomic commit/undo/replay/retry and bypass noncompletion.

### RFC-CX-004 addendum — initial branch preview

Director ACKs initial lighting=true and reader=true as a prototype initialization choice: it exposes the authored shared-supply conflict and makes folding the lighting branch meaningful. Planner records preview.initialConfiguration in the packet; systems reads it from data. This is an implementation choice consistent with the authored inference, not a measured pressure value or new narrative fact. Free bypass remains lighting=false, reader=true and grants no completion effects.

### RFC-CX-004 addendum — Blender generation inspection

Root operator executed Blender 5.1.2 with a fresh factory scene. First attempt failed during material-group joining (StructRNA reference removal); failed textures/log/source snapshot are preserved in assets/generated/3d/c1-patrol-panel-r01-attempt1. The corrected recipe completed with --python-exit-code 1, SUCCESS.json and a terminal success marker.

Successful resource:6 meshes, 2,092 triangles, four 1024-square maps totaling 1,615,402 bytes. Root inspected preview.png: round lamp housing and rectangular readout are distinguishable, both connect to the central supply, material wear follows the maritime bluegrey target, and no baked labels/numeric pressure values are present. All output/recipe/provenance hashes verified. This approves native diagnostic import only; runtime promotion awaits the native Unity capture.

### Native panel promotion — 2026-09-11

[OBSERVED] Director inspected diagnostic #3 front and alternate PNGs in systems/tech-verification/c1-m3/panel. The adapter is upright, retains the six meshes and texture maps, shows the shared connection clearly, and separates the warm lamp lens from the readout recess. Approve this exact static c1-b1 panel/adapter for runtime use. Blank plaques and readout are decorative; the UI remains authoritative for switch/readout state. Full G4/G5 and performance remain unmeasured. Original generation provenance and SUCCESS pair are preserved under assets/generated/3d/c1-patrol-panel-r01/generation-receipt/. The live provenance records capture hashes and this scoped promotion.

## RFC-CX-005 · C1 signature reader and art continuity (2026-09-11)

- lanes: planner, systems, presentation, modeling, QA; balance/economy carried because no currencies or authored numeric balance changes.
- request: Continue c1-b2 with resources/art, consistency, presentation and logic as explicit priorities.
- scope: c1-b2 only, continuing from shipped c1-b1 (ae098e0); no c1-b3/C4 identity reveal and no chapter-duration or full-game gate claim.
- canonical evidence: planning/campaign.json c1-b2; current reader/save specs; concept/style-guide.md.
- decision: ACK planner's three ordinal humidity choices (low/medium/high), initially unset, with low enabling separation and other levels producing reversible ink-risk preview. This is a labeled implementation inference, not a humidity percentage or additional lore. Canonical lowest-step hint remains opt-in; default guidance must not reveal the correct step or branch.
- first observation preserves the joined source; both post-separation verified copies must be made explicitly, inherit signature-annex, and never count as independent proof. Plate-zero comparison requires the distinct plate observation and explicit player comparison; no inferred signature identity is revealed.
- hidden region: approve a document-local normalized lower-sheet rectangle x=0.12, y=0.70, width=0.76, height=0.22, origin top-left. These are authored presentation coordinates for the visible paper surface, not a measurement of fictional historical evidence. Mark the same region on original/copy with a hatch and plain unresolved label; preserve obscuration after completion.
- completion: accepted-save receipt atomically publishes both copy receipts, unresolved region annotation, distinct-source plate-band link, completed c1-b2 and cp-c1-b2. All preview/recovery is free; no premature completion on failed/cancelled save or direct immediate submission.
- save architecture: ACK systems proposal for schema v3, backup-preserving v2 and v1 migration, byte/identity preservation of existing commands, and future-schema refusal. A shipped v2 binary must not silently replay new c1-b2 commands and fall back to a stale save.
- art intent: approve presentation lead's blank damp paper substrate via Higgsfield and static Blender reader/humidity tray assembly in existing palette/material language. Exact state reveal remains in Unity renderer driven only by authoritative snapshot; no generated lettering/numerals/signatures, no static prop presented as live simulation. Assets start runtimeEligible:false; native visual inspection precedes scoped promotion.
- evidence boundaries: include native edit/play tests, actual prior-v2 standalone route/restart, 150% text and reduced-motion capture; production G4/G5/performance/fun remain unmeasured.
- continuity: preserve the pre-existing unstaged C1PlayModeTests.cs non-spoiler regression (observed mtime 2026-09-11 02:08) and complete its intended fix; no concurrent coding process found at intake, Blender UI left untouched.
- status: implemented and independently verified within c1-b2; final isolated source and native restart approved.
- memory: mex-agent unavailable pending identity check; no zg index mutation authorized.


### RFC-CX-005 resource receipt — Higgsfield paper

[OBSERVED] Higgsfield 1.1.23, GPT Image 2.5 high/1k, job 54b52722-337a-42fc-baa4-e289c64db09e completed. Requested and actual 1024×1024; cost quote 3.5 credits and account delta 120.68 → 117.18 = 3.5. Exact prompt, schema and response are preserved in assets/generated/2d/texture/c1-signature-paper-r01/. Full-resolution visual source inspection found no generated text, numerals, signatures or clue marks. Approved for diagnostic import only; runtime eligibility remains false pending native inspection and provider terms review.

### RFC-CX-005 r02 geometry correction

[OBSERVED] Independent native review found a disconnected swing arm in r01 (rotation sign). ACK the minimal r02 derivative: same source recipe except revision/output path and arm Z rotation −0.47 → +0.47. Preserve r01 as rejected diagnostic evidence. Root re-executed and hash-verified r02; native promotion must reference r02 and final Unity adapter. No new lore, motion or gameplay state is introduced.

### RFC-CX-005 native reader r02 approval

[OBSERVED] Director inspected the r02 native Metal capture in systems/tech-verification/c1-m4/reader/reader-front.png (SHA-256 e22f401da9e2a3751448056d23ad66eb31ec83959dc520be16435634da769c45). The elevated fixed front view exposes the circular cradle, lens opening and right treatment tray; corrected arm connects the rear post to the lens neck. Muted weathered materials load correctly on a neutral dark background. Approve this exact static r02 housing/Unity adapter for c1-b2. Paper and its state layers require separate native inspection. This is scoped asset approval, not G4/G5/performance certification.

### RFC-CX-005 native paper approval

[OBSERVED] Director exercised diagnostic-3 standalone at 1280×800 requested viewport, 150% text and reduced motion using actual previous v2 save. Original, separated, individually copied, marked, compared and completed paper states kept the lower obscuration without exposing a name. Labels and mask remained inside the viewport after layout repair; original/individual copies were distinguishable. Approve the exact Higgsfield blank paper (SHA-256 0b35b75810e2bb052b580b4671541e06a92e68169e2c70ee3af3e66f02e66efe) and this UI state adapter for c1-b2. Evidence: systems/tech-verification/c1-m4/player-final-smoke/native-observation.json. The static reader r02 approval remains in force. Normal resource view may now be enabled and final app built; later pointer/fixed-summary corrections require native spot checks. This is not human accessibility/fun or performance proof.

### RFC-CX-005 final delivery verification

[OBSERVED] Isolated HEAD+65 approved Unity source passed EditMode36/36, PlayMode39/39 and serialized boot1/1. Concurrent external patrol fallback/string/test edits remain in the workspace and are excluded through two isolated index blobs and HEAD T0Strings. Final copied app is348,163,868 bytes (316 files; bundle fingerprint af69e25e5c1f677c9203cee0600ca68998ee25c4d2bbaf2430c1b7b38af51738). Ordinary-runtime restart restored c1-b2 completion, two copied papers, unresolved lower mask and correct resumed status with unchanged save bytes. Scope-approved implementation/resources may be committed and pushed per the user’s standing request. Human playtime/fun/accessibility, physical pad, auditory approval, performance and full G4/G5 remain unmeasured.

## RFC-CX-006 — Image generation uses god-tibo-imagen (2026-09-11)

- Authority: explicit user instruction to generate images with the god-tibo-imagen skill; this supersedes the earlier MuAPI/Higgsfield selection for new images only.
- Blender 3D and Higgsfield video workflows continue. Preserve every existing asset's actual provenance and runtime approval. No automatic replacement of approved art.
- [OBSERVED] GTI 0.3.0 and valid local Codex login verified without exposing credentials; dry-run and generation succeeded using requested model `gpt-6-astra`. No image-model identity or quota charge was returned.
- [OBSERVED] Original blank-paper candidate: `assets/generated/2d/texture/c1-signature-paper-gti-r01/`. Requested 1024×1024; actual 1254×1254, 2,017,998 bytes. SHA256 `5dd394fe188e287f1a81f73e8d5761d1807c6b9d5f711f15d93c0ef8b1c80cae`. Source and revised prompts, sanitized dry-run, execution and output receipts retained.
- [OBSERVED] Independent visual review passed the blank-substrate criteria. Size differs from request. Keep `runtimeEligible:false`; native readability/import and rights review remain unmeasured. This RFC authorizes the provider and candidate generation, not runtime promotion.
- Handoff: `production/image-provider-gti-20260911.md`.

## RFC-CX-007 — Video-led intro and gameplay direction (2026-09-11)

- Authority: user requested intro/gameplay videos, then direction/resource derivation and gameplay application. Images use GTI; video uses Higgsfield. This is a bounded M5 continuation, not full campaign completion.
- [OBSERVED] Generated two Seedance2mini clips; each actual6.041667s/1280×720/24fps/noaudio. Intro job`5c8b8815-fece-4459-807d-c9cef3ba2959`; C1job`8254dc3e-b7cc-405c-9019-ee33f4205f4d`. Quotes15+15credits; account balance117.18→87.18(delta30), not an itemized transaction audit.
- [OBSERVED] Intro measuredcut3.125s adopted as native3125+2875=6000ms two-context presentation. Use clean GTI hubr03 only. Reject generated postcut linedtraypaper/dialmarkings, and log horizonapprox25% versus requested40%.
- [OBSERVED] C1 cuts2.083333/4.041667 informed observation/trial/record spatial orientation. Reject automaticpaper-clearing, inventedinventory, knobaction andcameradrift. Actual lower mask, evidence and receipt state stay authoritative.
- [OBSERVED] New GTI UI surface was generated after the completed C1video, from its2.5s frame. Source video/frame/output hashes and execution timestamps are in `systems/tech-verification/intro-gameplay-m5/art-and-video-receipt.json`.
- [APPROVED DESIGN] Fresh-onlyskippable intro with settings access; existing-save bypass; release latch; reduced-motion-static information;150%text; staticC1orientation reads actualselection/trial/acceptedrecord. No runtime movie dependency, animation-gatedprogress, camera travel or new solverorder.
- [INTERNAL DIAGNOSTIC ONLY] Clean hubr03 and darkmetal surface may be imported into isolatedM5 for native inspection. `runtimeEligible:false` and profile runtimeApproved=false until native acceptance is recorded below. Rights review separates output ownership from GTI support/route and commercial clearance; no Steam-release clearance claimed.
- Sources: `presentation/intro-gameplay-m5.json`, `presentation/intro-gameplay-m5-video-review.md`, `qa/intro-gameplay-m5-review.md`, `qa/intro-gameplay-m5-rights-review.md`.

### RFC-CX-007 native prototype promotion — 2026-09-11T07:13:55.671Z

[OBSERVED] Diagnostic native build fingerprint 18066f11cd0a556758916d9c654e7b293d44cfc7b26cb0e683783090ca92e629 passed fresh intro150%/reduced-motion readability and settings retry, v2 direct continuation/one-observation trial/mask retention, and v3 completed-save restart (57 commands; save hash unchanged). Automated diagnostic suites: EditMode36/36, PlayMode53/53, separate boot1/1.

[DECISION] Promote only GTI hub source r03 (24a2e45c733fba2a43bffd602906358191479f47e183355001de36c691d7a5e2) and video-derived UI surface (52c4b3cc65cc1432bd94c5e7207ea35a50cbd5d8507f98e9c27f760db590b414) for bounded internal prototype runtime. Preserve pre-runtime provenance; commercialReleaseEligible remains false. Generated films and other stills remain previz-only. The UI source1254² is imported with max1024; intro1672×941 retains source dimensions.

[PENDING] Final runtimeApproved:true suites and ordinary native build/capture must pass before publication. No human-playtest or performance claim. Native evidence: systems/tech-verification/intro-gameplay-m5/native-acceptance.json; rights boundary: qa/intro-gameplay-m5-rights-review.md.

### RFC-CX-007 final ordinary delivery

[OBSERVED] Final ordinary build integrates upstream2ece517 with runtimeApproved:true. EditMode36/36, PlayMode54/54, serialized boot1/1 passed with no diagnostic override. App fingerprint484ab029f0bf29b4af6ffdd2bb9587ca6e08a432e09dde979d3635517b84d736 (316files;353421261bytes). Fresh intro naturally completed without creating gameplay save; C1 recording reached one accepted confirmation (54commands); completed v3 restart preserved57commands and save bytes. Native films are window-only recordings, separately labelled from generated previz.

The approved-profile requirement is fulfilled. Final native/video receipts retain diagnostic and ordinary build identities separately. Prototype-only source promotion remains bounded; generated footage is not gameplay evidence. Graphify updated; mex-agent unavailable remains [UNGRAPHED].
