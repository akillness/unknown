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


## M7 — original-concept-first art target (2026-09-11)

| task | owner | phase | artifact | status |
|---|---|---|---|---|
| Originalart sourceallowlist and GTI opticalkeyframe | concept, director | P2/P3 | concept/concept-first-m7-sources.json; assets/generated/2d/concept/m7-optical-workbench-r01 | complete; inputhashes bound, currentruntime sources excluded |
| Higgsfield MCP4scene target with1handrevision | presentation, director | P3 | assets/generated/previz/concept-first-m7 | complete;5jobs/4selected,20observedcredits |
|24s cinema/32s method target and independent source/visual review | systems, presentation, QA | P4 | docs/media/concept-first-m7; systems/tech-verification/concept-first-m7 | scoped media delivery complete;576/768frames, strictsourceancestry, decode and sampledart checks pass |
| GTI texturemaps and prefab reconstruction to approvedartdirection | concept, modeling, systems | next production | handoff/concept-first-m7-resources.json | planned; no texturepack/prefab implementation claimed in M7 |

## M8 — article-informed review notes (2026-09-11)

| task | owner | phase | artifact | status |
|---|---|---|---|---|
| Public article review and scoped adaptation | director, independent reviewer, presentation | P2 | systems/tech-verification/ai-native-m8/source-review.json; planning/ai-native-m8-reference-application.md | complete; secondary-source metrics not independently verified |
| Optional offline notes and source reflection | systems | P3/P4 | Unity App/UI/Input/Save and scoped tests | implemented; M8 PlayMode 10/10, journal and unlock authority unchanged |
| M7-consistent note staging and future GTI material brief | presentation | P2 | presentation/ai-native-m8-direction.md | design complete; no assets generated |
| Isolated native checks and publication | director, QA | P4 | systems/tech-verification/ai-native-m8/verification.md | scoped checks complete: EditMode46/46, M8 PlayMode10/10, boot1/1; broad64/65 with same baseline M5 failure; complete in this change; macOS build and native note-save smoke passed |

## M9 — 완성도 hop: 코어루프·튜토리얼·리소스 반영 연출·밸런스 (2026-09-11, RFC-CX-011)

| task | owner | phase | artifact | status |
|---|---|---|---|---|
| 3레인 병렬 감사(코어루프/튜토리얼·밸런스/리소스·연출, READ-ONLY) → 범위 판정 | systems, balance, presentation, director | P1/P2 | production/decision-log.md RFC-CX-011 | complete; 갭 목록·슬라이스 근거 RFC에 인용 |
| S-A~S-D·S-G 코어루프·튜토리얼·밸런스 계약 이행(힌트 영속화, two-step 기본, 프리뷰 diff, guided 티칭+비트 목표, snapshotInterval·유휴 타이머) | systems | P3 | unity/Unknown App/Save/Editor/Resources, Tests/EditMode/M9CoreTests.cs, systems/tech-verification/completeness-m9/impl-syscore.md | implemented; 판정 B(비공개 가드) 반영 |
| S-E·S-F M8 TARGET 3건 + 검토 노트 연출 계층(질문 버튼·원문 복귀·매체 분기·CanvasGroup 전환(180/140/120ms)·프로필 게이트) | systems, presentation | P3 | App/ReviewNotesSession.cs, UI/T0ReviewNotesInterface.cs, Presentation/M8ReviewNotesProfile.cs, Resources/M8ReviewVfx.json, Editor/M8ReviewNotesProjectBuilder.cs, completeness-m9/impl-sysm8.md | implemented; 서브에이전트 보고 단계 유실로 디렉터가 diff 검수 후 보고 재구성 |
| S-H GTI 검토 카드 텍스처 생성·임포트 | director, concept | P3 | assets/generated/2d/texture/m8-review-card-r01/, Art/Candidates/m8-review-card/, Resources/M8ReviewNotes.asset | candidate; runtimeEligible:false·runtimeApproved:false, 네이티브 창 가독성 검수 후 승격 |
| 네이티브 검증 + 독립 QA | director, QA | P4 | completeness-m9/verification.md·verification.json, qa/completeness-m9-review.md | QA R1: S1 0·S2 1·S3 10 → FIX 1 수리 6·판정 3·이월 2; FIX 1 검증 EditMode 53/53 · PlayMode 69/69 · boot 1/1 · macOS build Succeeded 354230869B · 트랜스크립트 실행 영수증; 창 가독성 캡처 미완; QA R2: 수리 6 CLOSED·판정 3 수용, 신규 S2 D-M9-13(도구 열림 중 원문 열기) → FIX 2 `tool==null` 게이트 + 회귀 테스트; R3 확인 대기 |

[OBSERVED] 이월(RFC-CX-011 명시 + QA FIX 판정): hintOfferCooldownSeconds 노브 분리, EN 힌트 textKey, alignment 신규 동사(기획 재-인테이크), ReadOriginal 마모 undo 판정(개방 RFC), t0-b1 objective 무스포일러 재작성 + t0-b2 objective 저작 지시문 제거(planner·campaign.json, D-M9-11), 프리뷰 문장 영향 구역·근거 2종(D-M9-07), 검토 노트 출처 선택 100ms 윤곽(D-M9-03), 티칭 헤더 hint-system.md 등재(D-M9-06), 도구 열림 중 원문 열기 허용(D-M9-13 축소 수리의 확장안).

## M10 — TRACE-RPG methodology: evidence-graph contract, terminology, narrative bridges (2026-09-11)

| task | owner | phase | artifact | status |
|---|---|---|---|---|
| Evidence-unlock typed graph + deterministic validator (TRACE-RPG 6-family adaptation) | planner | P2/P4 | planning/emit-evidence-graph.mjs, planning/evidence-graph.json, planning/evidence-graph-overlay.json, planning/validate-evidence-graph.mjs (+4 .meta.md) | **current** (QA PASS); EG-SUMMARY 18/18, byte-deterministic, negative test non-vacuous, provenance re-bound to post-replacement campaign (RFC-CX-012 G-A) |
| Worldview terminology audit + aside consultation + director decision + application | worldview, planner, systems, synopsis, director | P2/P4 | worldview/term-audit-20260911.md, production/term-decision-aside-20260911.md, planning/field-classification-20260911.md, systems/rfc-cx-012-ack.md; campaign.json 65+2 edits, glossary R8+R9 (+12 rows, 0 deleted), t0-records 2 rows, ui-contract 1, data/t0 regenerated ×2 | **current** (QA PASS); ACK a/b/c resolved, completion=predicate / 6 display fields, 유지 13행 침해 0, 49/49 · --t0 5/5 · EG 18/18 (RFC-CX-012 G-B) |
| 33-beat narrative bridge layer (ledger grammar adaptation: 이음/해금 예고/보류) | synopsis | P2 | synopsis/narrative-flow-bridges.md | **current** (QA PASS); 33×3 rows realigned post-replacement (8 rows/10 cells), ceiling violations 0, misconception negations 0, unregistered nouns 0 (R9 basis), rule naming 33/33 (RFC-CX-012 G-C) |
| Independent QA verification ×2 + promotion | qa, director | P4 | qa/rfc-cx-012-review.md | complete; S1 0 · S2 0 · S3 closed 5, promotion 14/14 PASS, director re-run 49/5/18 + freshness 0/600 |
| **Carried**: Unity Data/Tables re-copy (stale sha → Import fail-closed), authoring-annotation residue 22, c1-b3 훈련 ×3, c6-b3.consequence R3 alignment, timeline B13 "공통 피크" | systems, worldview, planner | next | unity/Unknown/Assets/_Project/Data/Tables/, campaign.json, timeline.md | open — next editing cycle |
