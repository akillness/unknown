---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-production-director
---

# M5: intro and gameplay direction delivery

[OBSERVED] Final ordinary build integrates upstream2ece517 with runtimeApproved:true. EditMode36/36, PlayMode54/54, serialized boot1/1 passed with no diagnostic override. App fingerprint484ab029f0bf29b4af6ffdd2bb9587ca6e08a432e09dde979d3635517b84d736 (316files;353421261bytes). Fresh intro naturally completed without creating gameplay save; C1 recording reached one accepted confirmation (54commands); completed v3 restart preserved57commands and save bytes. Native films are window-only recordings, separately labelled from generated previz.

## Delivered

- Two Higgsfield previz clips, each6.041667s at1280×720/24fps, silent; requested6s. Observed two-job balance delta30credits. GTI quota cost unreported.
- Video-reviewed static context, measured3125+2875ms intro, observation/trial/record direction. Invented props, automatic knobs/camera drift and mask-clearing are rejected.
- GTI hubr03 and the UI surface derived from the completed C1 clip are prototype-runtime approved under RFC-CX-007. Other stills and videos remain previz-only. CommercialReleaseEligible remains false.
- Fresh intro, skip/settings/Escape/replay/reduced-motion/input-release behavior; existing-save bypass; state-derived C1 directions and accepted-save distinction.
- Actual native intro and C1 excerpts: [gallery](../../../docs/media/intro-gameplay-m5/index.html). C1 movie is three takes with cuts; not an uninterrupted input trace.
- Tested25 owned Unity files copied by hash to canonical workspace; existing3 upstream files preserved. Finalapp: unity/Unknown/Builds/M5-mac/Unknown.app (local ignored build).

## Evidence and limits

Finaltest/build commands and hashes: systems/tech-verification/m5-direction/final-validation.json. Source boundary: ownership-source-manifest.json and upstream-integration.json in that directory. Native: systems/tech-verification/intro-gameplay-m5/native-acceptance.json and native-video-receipt.json. Independent QA: qa/intro-gameplay-m5-integrated-final-review.json. Art/lineage: presentation/intro-gameplay-m5-video-review.md and systems/tech-verification/intro-gameplay-m5/art-and-video-receipt.json.

Graphify isolated final refresh:10403nodes/11110edges/1260communities, exit0. mex-agent unavailable after checking5existing candidates: [UNGRAPHED], no fabricated mex check/log. Frontmatter/topology freshness passed; this is not complete memory-stack or human-playtest evidence. No physical-gamepad, performance, commercial-clearance, full-game, or human-fun claim.
