---
updated: 2026-09-09
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
