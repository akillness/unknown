---
updated: 2026-09-14
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
| 네이티브 검증 + 독립 QA | director, QA | P4 | completeness-m9/verification.md·verification.json, qa/completeness-m9-review.md | QA R1: S1 0·S2 1·S3 10 → FIX 1 수리 6·판정 3·이월 2; FIX 1 검증 EditMode 53/53 · PlayMode 69/69 · boot 1/1 · macOS build Succeeded 354230869B · 트랜스크립트 실행 영수증; 창 가독성 캡처 미완; QA R2: 수리 6 CLOSED·판정 3 수용, 신규 S2 D-M9-13(도구 열림 중 원문 열기) → FIX 2 `tool==null` 게이트 + 회귀 테스트; R3: D-M9-13 CLOSED·S2 재개방 없음·잔여 S3 1(planner 범위 밖)·S4 1 정정 → **범위 내 PASS(자동화·문서 수준; G4/G7 런타임 PASS 아님)** |

[OBSERVED] 이월(RFC-CX-011 명시 + QA FIX 판정): hintOfferCooldownSeconds 노브 분리, EN 힌트 textKey, alignment 신규 동사(기획 재-인테이크), ReadOriginal 마모 undo 판정(개방 RFC), t0-b1 objective 무스포일러 재작성 + t0-b2 objective 저작 지시문 제거(planner·campaign.json, D-M9-11), 프리뷰 문장 영향 구역·근거 2종(D-M9-07), 검토 노트 출처 선택 100ms 윤곽(D-M9-03), 티칭 헤더 hint-system.md 등재(D-M9-06), 도구 열림 중 원문 열기 허용(D-M9-13 축소 수리의 확장안).

## M10 — TRACE-RPG methodology: evidence-graph contract, terminology, narrative bridges (2026-09-11)

| task | owner | phase | artifact | status |
|---|---|---|---|---|
| Evidence-unlock typed graph + deterministic validator (TRACE-RPG 6-family adaptation) | planner | P2/P4 | planning/emit-evidence-graph.mjs, planning/evidence-graph.json, planning/evidence-graph-overlay.json, planning/validate-evidence-graph.mjs (+4 .meta.md) | **current** (QA PASS); EG-SUMMARY 18/18, byte-deterministic, negative test non-vacuous, provenance re-bound to post-replacement campaign (RFC-CX-012 G-A) |
| Worldview terminology audit + aside consultation + director decision + application | worldview, planner, systems, synopsis, director | P2/P4 | worldview/term-audit-20260911.md, production/term-decision-aside-20260911.md, planning/field-classification-20260911.md, systems/rfc-cx-012-ack.md; campaign.json 65+2 edits, glossary R8+R9 (+12 rows, 0 deleted), t0-records 2 rows, ui-contract 1, data/t0 regenerated ×2 | **current** (QA PASS); ACK a/b/c resolved, completion=predicate / 6 display fields, 유지 13행 침해 0, 49/49 · --t0 5/5 · EG 18/18 (RFC-CX-012 G-B) |
| 33-beat narrative bridge layer (ledger grammar adaptation: 이음/해금 예고/보류) | synopsis | P2 | synopsis/narrative-flow-bridges.md | **current** (QA PASS); 33×3 rows realigned post-replacement (8 rows/10 cells), ceiling violations 0, misconception negations 0, unregistered nouns 0 (R9 basis), rule naming 33/33 (RFC-CX-012 G-C) |
| Independent QA verification ×2 + promotion | qa, director | P4 | qa/rfc-cx-012-review.md | complete; S1 0 · S2 0 · S3 closed 5, promotion 14/14 PASS, director re-run 49/5/18 + freshness 0/600 |
| ~~Carried~~: Unity Data/Tables re-copy, authoring-annotation residue 22, c1-b3 훈련 ×3, c6-b3.consequence R3 alignment, timeline B13 | systems, worldview, planner | — | → M11 | **closed in RFC-CX-013** (all 5 resolved; see M11) |

## M11 — RFC-CX-012 carry-over resolution (2026-09-11, RFC-CX-013)

| task | owner | phase | artifact | status |
|---|---|---|---|---|
| Authoring-annotation residue (23 terms/21 fields) + c1-b3 훈련→연습 ×3 + c6-b3→c6-b4 R3 transfer + K-07 validator | planner | P2/P4 | planning/campaign.json (28 leaves, 0 add/del/id), planning/validate-campaign.mjs (K-07, 50 checks), field-classification-20260911.md §RFC-CX-013 | **current**; 50/50, K-06·K-07 PASS, K-07 negative test FAIL on fc737c1 copy, EG 18/18 |
| timeline B13/§8 citation, consistency-audit A44 (pass), term-audit carry closure | worldview | P2 | worldview/timeline.md, consistency-audit.md (44 = 41 pass·3 open·0 violation), term-audit-20260911.md | **current**; glossary unchanged this cycle |
| Unity Data/Tables re-emit (--scope t0) + guarded headless Import + T0Verification.RunBatch | systems | P3/P4 | unity/Unknown/Assets/_Project/Data/{Tables,Authoring}/, unity/Unknown/results/t0-m1-contract-checks.xml, systems/tech-verification/rfc-cx-013-tables-resync.md | **current**; receipt sha == live campaign, Import exit 0 (no "Producer source differs"), T0_M1_CHECKS 21/0, compile errors 0 — fail-closed import resolved |
| Synopsis citations (campaign.md, chapter-beats.md 표 B c6-b3/c6-b4 + 16 term rows, bridges L18/L139) | synopsis | P2 | synopsis/campaign.md, chapter-beats.md, narrative-flow-bridges.md | **current**; QA D-CX013-02/03/04 fixed |
| Independent QA + director re-run | qa, director | P4 | qa/rfc-cx-013-review.md | complete; 30 checks, S1 0 · S2 0 · S3 0 after fixes; 50/50 · --t0 5/5 (sha match) · EG 18/18 · freshness 0/602 |
| **Re-intake (open, not S-graded)**: c6-b3 objective/completion/subtasks[2][4]/clues c3·c4 open the motive mechanism inside B26 — (A) redefine B26/B27 bound or (B) move motive sub-beats to c6-b4 (clues·pairs·EG re-derive) | director → planner, worldview | content-update intake | timeline.md §7, campaign.json c6-b3/c6-b4 | open — beat design change, not a carry fix |
| Carried (non-blocking): continuity.md K10 row B26→B27, glossary §2 「정합기」 "공통 피크" abbreviation, consistency-audit §3 table re-run | synopsis, worldview | next | — | open |

## M12 — 네이티브 실동작 플레이테스트·모니터링·게임플레이 영상 (2026-09-11, RFC-CX-015)

| task | owner | phase | artifact | status |
|---|---|---|---|---|
| 실빌드 플레이어 전구간 창 조작 + 런타임 모니터링(T0 b1→b3 완주·프리뷰→확정·되돌림/다시·OS 재시작 재개·C1 순찰·검토 노트) | director, qa | P4 | systems/tech-verification/native-playtest-m9/{shots,rec-shots}/, save-dir.txt, record-save-dir.txt | complete; 스크린샷 shots 184 + rec-shots 63+ [OBSERVED 작성 시각 66, 녹화 세션 진행중], Player.log 예외 0 · save.json 실시간 대조(`hintLevelUsed={"t0-b1":2}` 유지, headSeq 31→30→31, OS 재시작 후 sha 불변) |
| 실동작 전용 결함 3건 수리 + D-M9-11 실증 (D-M9-15 질문 패널 앵커 아래 렌더 / D-M9-16 `ResumeStatus()` 3분기 / D-M9-17 내부 id 4개소 → `ReviewMediaName(sourceType)`) | systems | P3/P4 | App/ReviewNotesSession.cs, UI/T0ReviewNotesInterface.cs, UI/T0Interface.cs (RFC-CX-011 파일 집합 내 소형 수리) | complete; 재빌드 후 재검증 EditMode 53/53 · PlayMode 73/73 (+6 skipped, total 79) · serialized boot 1/1 · `T0_MAC_BUILD Succeeded bytes=354247240` |
| 코어루프·게임플레이 영상 2편(`core-loop` 80.6s / `gameplay` 191.0s) — M7 `gameplay-method.mp4` 편집 관례(1280x800 1:1 · 24fps · ASS 챕터 카드 · poster · edit-timeline) | presentation, director | P3 | docs/media/gameplay-m9/, assets/generated/video/gameplay-m9/ | complete; 네이티브 창 녹화(previz 아님·무음), 원본 3세그먼트 sha 보존 |
| 이월(비차단): 자동 보존 단서 id 표시명(`t0-b2-c1` 등 이름 데이터 부재) · t0-b2 objective 저작 지시문 노출(D-M9-11 유지) · 눈금 선택기 휠 스크롤 미도달(드래그로 도달 가능, 자동화 특유 오클릭) · M7 프리팹 재구성(회로 지도/판독기 노드 그레이박스) · 사람 플레이테스트 | planner, presentation, modeling, qa | next | — | open |

[OBSERVED] 조작 방식의 경계: 본 회차 입력은 **cliclick 포인터 조작**이며 합성 키 이벤트는 Unity Input System에 닿지 않았다 → 키보드·컨트롤러 동등성은 PlayMode 테스트에 위임되고, 실기기 IME·컨트롤러 검수는 미완이다. 사람 플레이테스트·몰입·재미·8시간 완주는 **미측정**(n=0)이며, 본 절의 어떤 행도 G4/G7 런타임 PASS를 주장하지 않는다.

## M13 — M7 컨셉 리소스 런타임 승격 · 남은 작업 마감 · 레거시 제거 (2026-09-11, RFC-CX-016)

| task | owner | phase | artifact | status |
|---|---|---|---|---|
| 문서 id 충돌 해소: RFC-CX-013 3중 충돌 → 014로 재번호(인용 없던 블록), 마일스톤 M10~M13 확정 | director | P4 | production/decision-log.md | 완료 · 실제 git 병합 충돌은 0건(단일 워크트리·마커 0) |
| M7 3레인 + M8 카드 런타임 승격(`runtimeApproved` false→true ×4) | systems, presentation | P3 | Resources/{M7Hub,M7UiSkin,M7ReaderStage,M8ReviewNotes}.asset | 완료 · 근거 = 루트 디렉터 지시(RFC-CX-016) |
| 승급 후 결함 수리(프로브 없는 씬의 고메탈 재질, T0Interface 상태줄·자산 필드, 리더 스테이지 복원 훅) | systems | P3 | M7ReaderSession.cs, T0Interface.cs, M7UiSkinProfile.cs, MAT_M7_Hub_*.mat, Editor/M7*.cs | 승급 레인이 1h21m에 중단 — 수리 분류는 ui-promotion-m13/verification.md 참조 |
| 미측정 마감: M7 PlayMode 5건 skipped(자산 미임포트)→passed 전환 | systems, qa | P4 | playmode-final.xml | 완료 · 83총 82passed 1skipped(부트=설계상 격리) |
| 이월 3건: 단서 id 노출 수리 · D-M9-11 오류 이월 종결 · 휠 스크롤 계측 한계 판정 + 회귀 3건 신설 | planner, systems | P3/P4 | remaining-m13/notes.md, planning/objective-copy-m13.md, ScrollReachabilityTests.cs | 완료 · 신규 3건 전부 통과 |
| 레거시 제거 4건(34파일 20,845,750B) + hub.unity 씬 수술(95줄) + 배선 정리 | director | P3 | legacy-purge-m13/{purge-plan.md,purge-receipt.json} | 완료 · 끊긴 참조 0건(516파일×needle 11) · assets/generated 558→558 무삭제 |
| 전수 재검증 + 빌드 + 네이티브 스모크 | director, qa | P4 | ui-promotion-m13/verification.md | EditMode53/53 · PlayMode82/83 · boot1/1 · build Succeeded 403,838,357B · 예외 0 |
| 이월(비차단): t0-b1 objective 무스포일러 재작성(교정안 3안 대기) · 빌드 403MB 용량 검토 · 퍼지 단독 기여분 미분리 | planner, product | - | planning/objective-copy-m13.md | 열림 |

## M22 — 힌트 리듬·판독 연출·작업면 리소스 통합 (2026-09-13, RFC-CX-017)

| task | owner | phase | artifact | status |
|---|---|---|---|---|
| M14~M21 기준선 보존·게임 요소 분석 | director | P1 | intake/production-brief.md, decision-log.md RFC-CX-017 | complete; main 작업 1개, 기존 미커밋 변경 보존 |
| 무료 힌트 제안/쿨다운과 입력 경로 동등성 | systems, balance | P3 | T0GameSession, WatchInput, T0Interface, tools 스키마·발행 테이블 | complete; 180/180 유지, 접근 가능한 보기·닫기와 150% 레이아웃/도구 보존/세션 내 닫기 계수 회귀 통과; 강제 힌트 공개/소모 변경 없음 |
| 실제 판독 성공 기반 크랭크·모션 축소 즉시 반영 | presentation, motion, systems | P3 | M7ReaderSession, M22EmbodimentVisual, T0ReaderVfx | complete; 수락 시점 monotonic 시계 유지, 앱 중단/포커스 상실 취소·백그라운드 요청 차단·복귀 후 미재생 회귀 통과 |
| 기존 승인 작업면 개선; r02 기본 적용 보류 | concept, presentation, director | P3/P4 | M7UiSkin, M20WorkSurface 진단 경계 | complete; 승인 표면 alpha 0.30, M20 r02 미승격 유지 |
| 통합 네이티브 검증·main 최신화·워크트리 정리 | director, qa | P4/P5 | systems/tech-verification/m22/verification.json, docs/media/gameplay-m22/ | 로컬 검증 완료: 56 EditMode + 100 PlayMode + 1 격리 부팅(고유 157); main 작업트리 1개/삭제 0; commit/push 사용자 전용 |
| 유사작 5종의 플레이 원리와 독자적 적용 | director, planning | P1/P2 | planning/t0-research-to-implementation-r01.md M22 확장 | complete; 관찰·대조·의도적 조작만 적용, 원작 서사/캐릭터/고유 자산 복제 없음 |
| Blender MCP 한서린·양손·Generic 리그·원본 모션 | modeler, motion | P3 | scripts/blender/build-seorin-m22.py, assets/generated/3d/seorin-m22 | complete; 실제 MCP 45회, 6종 의미 동작/11 FBX take, 원본 보존 범위는 QA 영수증 참조 |
| 실제 조작과 손 접촉·확정 모션의 런타임 결합 | systems, presentation, motion | P3/P4 | M22EmbodimentProfile/Visual/ProjectBuilder, 세션 훅 | complete; 기본 빌드 적용과 접촉/복귀/취소 검증; 판정·저장 불변, 상업/사람 플레이 승격 아님 |

## Aside scoped immersion follow-up (2026-09-13)

| task | owner | phase | artifact | gate | status | beat |
|---|---|---|---|---|---|---|
| 개발자 출처 기반 서사·밸런스·연출 연구 및 적용/보류 판정 | director, synopsis, worldview, balance, presentation | P2 | presentation/aside-immersion-20260913/ | scoped source review | done, independent QA ACK | t0-b1 |
| 마지막 당직 오프닝 4문구와 안전 objective fallback | systems | P3/P4 | handoff/aside-immersion-20260913.md | source 17/17; campaign 50/50 | source verified; native blocked (license198) | t0-b1/b2 |
| GTI 2회 생성과 실제 파일·프롬프트·비용 unknown 기록 | concept | P3 | assets/generated/2d/{concept,texture}/aside-immersion-20260913/ | source pixels/parent visual | 1 revision-required + 1 reviewed candidate; both runtime false | candidate only |
| Unity 네이티브 4케이스 + 기존 오프닝 회귀 | QA, OMP launcher | P4 | systems/tech-verification/aside-immersion-20260913/ | Native | blocked: license unavailable before tests; no XML | T0 |
| OMP 통합 결과 1회 후속 확인 | director | P4 | heartbeat WcMNqf5Pe5Ia7THL | read-only evidence | scheduled 2026-09-13 16:50 KST | follow-through |

## Aside rules/core-loop research (2026-09-13, RFC-CX-ASIDE-CORE-20260913)

| task | owner | phase | artifact | status |
|---|---|---|---|---|
| 정본·현재 구현·사람 검증 범위 지정 후 Aside ultrabrowse 조사 | director, planner | P1/P2 | handoff/aside-core-loop-research-20260913.md, planning/aside-core-loop-research-20260913.md | done; 원문 12건·권고 10개·실험 3개, deep credit 실패 후 기본 프로필 완료 |
| 원문·Reader/Signature 코드 대조 및 개발 우선순위 판정 | director | P2 | handoff/aside-core-loop-results-20260913.md | done; 첫 후보 T0 두 자료 비교, 정본/런타임 구현 승인 아님 |
| 독립 출처 이해의 단일 변수·반증·첫 노출 분리 계약 | planner, systems, QA | P2 | planning/core-loop-research-20260913/game-design-hypothesis.json | 구조/self-test PASS; 프로토타입·사람 실행 없음 |

## M23 — Aside 권고 구현·디버깅·소스 전달 (2026-09-14)

| task | owner | phase | artifact | status |
|---|---|---|---|---|
| R1 현재/고정 두 기록 동시 비교 | systems, QA | P3/P4 | ReaderComparisonSession/View, m23/verification.json | done;150% 진단 대비·비교 저장 불변·Undo 비가시 출처 pin 해제/Redo 비복원 |
| R2 공개 출처 수동 선택·후보와 기록 구분 | systems, QA | P3/P4 | C1SignatureGameSession/Data/Definition | done; 같은 루트 거부·명시 인용/확정·구 save replay/복구 |
| R3 기존 M8 계보·요청형 질문 | systems, QA | P3/P4 | ReviewNotesSession | done; 사본2/원본1 표시·노트 재시작 바이트 보존; 기존 원문 복귀 경계 유지 |
| R4 문구/감사 강도 정정과 생성물 동기 | canon, planner, systems | P3/P4 | m23-r4-canon-consistency.md, preservation-audit.json | correction done; campaign50/graph18/Node7; 강한 P1/P2 exit3 blocked |
| R5 별도 정합 시편·150% 키보드·저장 격리 | systems, QA | P3/P4 | AlignmentPractice/Session/View | done; 직접 API6개 격리·진단 mesh/라벨/문단 대비·양 끝 피크·native 저장/설정 불변. 사람 이해 아님 |
| R6 한 빌드 실제12명/5유형 평가 | QA, operations | P4 | qa/m23-human-evaluation.md | blocked; n=0, 수동 창 녹화 약8초만 확인, 전체 producer/45분 미검증 |
| R7 저장 큐/취소/미리보기/고정 가시성 | systems, QA | P3/P4 | T0ActionFeedbackSession, T0GameSession, T0Interface | done; 초기 Q1–Q5 RED→교정·실제 스크롤/내구 확정, 후속 최종121 PlayMode에도 통과 |
| R8 재방문 생산 | planning, systems | P3 | Base production gate | blocked; 사람·개념·실제 노력·게이트 미충족 |
| R9 증언/3제출 생산 | synopsis, systems | P3 | Base production gate | blocked; 준비 패널은 후반 생산 완료가 아님 |
| R10 전체 완주/목표 장치 | QA, production | P4 | m23/verification.json | partial; 현재 슬라이스 복구/Mac만 검증. 전체450–540분·Windows 미측정 |
| 통합 빌드·독립 리뷰·cleanup | Main, reviewers | P4 | m23/verification.json, qa/m23-integration-review.md | done; 초기35ebdc5 후 Q6–Q11 교정, 프로젝트 Unity186·Mac410875885B·정확한 빌드별 native. 새 producer 없음·소유 플레이어 종료 |
| 소스 전달과 최종 개선 계획 | director | P5 | handoff/m23-results-and-improvement-plan.md | 최신 요청의 명시적 commit/normal push 승인; 기존 M22/Aside 보존. 실제 커밋/원격 결과는 Git 기록을 따름 |
