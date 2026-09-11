---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-production-director
---

# Task Manifest (R7 종료 · T0 M1 코어 구현·검증)

2026-09-10 사용자 후속 요청: README/media `13a4133`, M1 경과 README `75102fd` 푸시 완료. T0 M1 코어 native 검사 21/0 및 독립 QA 완료; 출처 C7-F50과 디스크 공간 문제는 열려 있다. 상세 범위·영수증은 `production/codex-t0-m1-status.md`에서 추적한다. 본편 production gate는 그대로 유지한다.

| task | owner | phase | artifact | gate | status | beat |
|---|---|---|---|---|---|---|
| C3 세계관 재기반·planner REDO·레인 재도출·QA 재검증 3회 | 전 설계 레인 + QA | P4 | worldview/ planning/ synopsis/ systems/ balance/ economy/ qa/c3-review.md | D-G1/G7 | done (S1 0·S2 0·S3 1·rfc 2) | C3 |
| C4 재검증 + 승격 | systems, modeling, visual lanes, QA | P4 | qa/c4-review.md, 승격 5건 | D-G4/G5/G6 | done (S2 1 잔여) | C4 |
| C5 독립 검토 + 수정 | product, presentation, systems, QA | P4 | qa/c5-review.md | D-G3/G7 | done (S2 1 = 대장, 재생성 완료) | C5 |
| C6 통합 초안 v1 + 5렌즈 판정 + 수정 | planner, director, QA | P4 | planning/game-draft-v1.md qa/c6-review.md | D-all | done (S2 2 → R7b) | C6 |
| C7 핸드오프 + T0 인스턴스 데이터 + 반박 3렌즈 | systems, modeling, synopsis, planner, QA | P5 | handoff/ systems/data/t0/ | D-G8 | done (S1 0·S2 1 → R7b) | C7 |
| 2D 45장·3D 그레이박스·영상 2클립·README 미디어 | concept, modeling, director | P3 | assets/ docs/media/ README.md | D-G5 | done (runtimeEligible:false) | C7 |
| Unity 6000.5.6f1 in-repo 스켈레톤 | systems | P3 | unity/Unknown/ | – | done (헤드리스 열기 영수증 2차) | C7 |
| R7b 한 줄 수정 + 승격(interaction-rules·UI meta·business-model) | systems, product, planner, QA | P5 | 해당 파일 | – | in-progress | C7 |
| 사이클 종료: 회고·freshness·graphify·zg·vault·CLAUDE.md | director | P5 | retrospectives/ | G8 | in-progress | C7 |
| **다음: T0 구현(Codex) → 사람 검증 12명 → Base production gate** | Codex + QA + director | – | unity/Unknown, handoff/verification-plan.md | G4~G7 | open | 다음 회차 |

## T0 M2 continuation (2026-09-10)

See `production/codex-t0-m2-status.md` for current scope and evidence; it supersedes the M1-only current-status paragraph above, while preserving historical receipts.

| task | owner | phase | artifact | gate | status | beat |
|---|---|---|---|---|---|---|
| T0 M2 save, undo, input, UI and playable scene | systems, QA | P3/P4 | unity/Unknown/, systems/tech-verification/t0-m2-native.md, systems/tech-verification/t0-m2-player-smoke.md | T0 technical checks | verified: EditMode 18/18, PlayMode 15/15, macOS build success; pointer T0 completion and OS restart; corrected-player visual smoke PASS | t0-b1/b2/b3 |
| Citation and circuit authored source packets | worldview, synopsis, planner, QA | P2/P4 | synopsis/t0-records.md, planning/t0-circuit-overlay.json | scoped source QA | scoped source ACK approved and runtime integration verified; campaign gates remain separate | t0-b2/b3 |
| Blender r03 drawer and Higgsfield stamp candidate | modeling, director, QA | P3 | assets/generated/3d/hub-view-drawer-r03/, assets/generated/audio/higgsfield-stamp-r01/ | asset-specific import/inspection | r03 approved for T0 and visible in native player; audio listening review pending, runtime playback disabled | t0-b1 |

## C1 M3 continuation (2026-09-11)

See `production/codex-c1-m3-status.md` and RFC-CX-004 for bounded c1-b1 implementation.

| task | owner | phase | artifact | status |
|---|---|---|---|---|
| C1 first-beat authored contract | planner | P2 | planning/c1-patrol-contract.json | current; static review passed |
| C1 entry, circuit preview, atomic chapter save and v1 migration | systems, QA | P3/P4 | unity/Unknown/ | complete: EditMode 27/27, PlayMode 28/28, native restart verified |
| Original patrol panel | modeler, director | P3 | assets/generated/3d/c1-patrol-panel-r01 | complete: generated, native-inspected and scoped runtime-approved (RFC-CX-004) |

## C1 M4 continuation (2026-09-11)

See RFC-CX-005 and production/codex-c1-m4-status.md.

| task | owner | phase | artifact | status |
|---|---|---|---|---|
| Signature reader authored packet | planner | P2 | planning/c1-signature-contract.json | complete — scoped evidence recorded |
| State-driven paper/reader art and reveal | presentation, modeling, director | P2/P3 | presentation/c1-signature-presentation.md | complete — scoped evidence recorded |
| c1-b2 runtime, safe saves and native verification | systems, QA | P3/P4 | unity/Unknown/ | complete — scoped evidence recorded |

## M5 intro and gameplay direction continuation — 2026-09-11

| Slice | Owner | Status | Acceptance |
|---|---|---|---|
| GTI stills + Higgsfield intro/C1 previz | production + presentation | complete | source/clip provenance, no premature disclosures, reviewed timestamps |
| Video-derived native intro and C1 orientation | systems | complete | skip/settings/reduced-motion, state-only direction, save/input invariance |
| Isolated build, native capture and QA | QA + production | complete | new source tests, previous-save restart, actual window-only recording |

Baseline `6514f549ac7b2e71846005c75a4ff9227882c47c`; scope and chronology in `production/intro-gameplay-m5-status.md`. Existing source/frame material remains preserved.

[OBSERVED] Final ordinary build integrates upstream2ece517 with runtimeApproved:true. EditMode36/36, PlayMode54/54, serialized boot1/1 passed with no diagnostic override. App fingerprint484ab029f0bf29b4af6ffdd2bb9587ca6e08a432e09dde979d3635517b84d736 (316files;353421261bytes). Fresh intro naturally completed without creating gameplay save; C1 recording reached one accepted confirmation (54commands); completed v3 restart preserved57commands and save bytes. Native films are window-only recordings, separately labelled from generated previz.


## M6 — cinematic and gameplay-method video delivery (2026-09-11)

| task | owner | phase | artifact | status |
|---|---|---|---|---|
| Official Higgsfield MCP four-shot production + GTI references | director, concept, modeling | P3 | assets/generated/previz/cinematic-gameplay-m6/; two m6 concept keyframes | complete; four jobs and provenance verified; previz-only |
| 36s cinematic +54s method edit and accessible manual-play gallery | presentation, systems | P3/P4 | docs/media/cinematic-gameplay-m6/ | complete; exact1080/1620frames, full decode and10asset links pass |
| Source semantics, masks, accepted-save boundary and caption review | QA, presentation | P4 | systems/tech-verification/cinematic-gameplay-m6/editorial-review.md | scoped editorial review complete; target gaps explicit; no humanG4/Unity claims |
