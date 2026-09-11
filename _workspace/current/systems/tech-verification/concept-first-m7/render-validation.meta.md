---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
---

# M7 concept-only media validation

RFC-CX-009. render-films.py reads only the four selected M7 MCP clips. Run it through RTK to recreate both films and validation.json. Each141frame/24fps source is padded by3heldframes to144(6seconds), uniformlyscaled and topcropped, then captioned. Explicit24fps CFR timebase avoids concat timing drift. Output576/768frames and full decode are asserted. Cache is reproducible and excluded fromGit. The32second edit adds8seconds of a darkened M7 record-context still and manual instruction text. No current/native/prefab visual input is used.

source-ancestry-check.json checks every submitted visual parent against the4original-image hash allowlist or the separatelyapproved GTI derivative, including binding uploadedmediaIDs. optical-source-approval.json records derivative review. These checks are not runtime/physics/humanimmersion proof. Providerandartreview receipts report remaining source and motion limits.
