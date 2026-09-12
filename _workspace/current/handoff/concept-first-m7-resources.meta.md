---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-production-director
---

# M7 resource rebuild handoff

RFC-CX-009 applies the latest user correction: original lore and concept art define the new film target; the film target informs GTI texture/resource work and prefab reconstruction. This JSON is a future production contract. Five material groups and four environment/prop groups plus a separately reviewed hand are specified. Existing game resources are forbidden visual parents. Runtime promotion needs fresh native validation.

[OBSERVED · 2026-09-11 집행] `execution20260911` 블록 추가: 5개 재질 GTI 텍스처 후보와 Blender 블록아웃(4 타깃)·pre-runtime QA(타일 이음새/가동/GLB 라운드트립)가 생성됐다. 전부 `runtimeEligible:false` 후보이며 인엔진 검사·무조명 base colour 승인·네이티브 검증은 열려 있다. 상세는 decision-log의 RFC-CX-009 delivery evidence와 각 provenance.json.

[OBSERVED · 2026-09-11 runtime] RFC-CX-013으로 `execution20260911.runtime` 블록 추가: 위 후보들이 Unity 6000.5.6f1 / URP 17.5.0 `unity/Unknown`에 임포트되어 T0 스테이지1 런타임에 레인별 진단 게이트 뒤로 배선됐다 — HubShell(`Resources/M7Hub.asset` · `App/M7HubSession.cs` · textures=6 materials=3), ReaderStage(`Resources/M7ReaderStage.asset` · `App/M7ReaderSession.cs` · fbx=2 textures=8 materials=7, 카메라는 임포트 bounds 실측), UiSkin(`Resources/M7UiSkin.asset` · `App/M7UiSession.cs` · textures=2). 검증 EditMode 53/53 · PlayMode 78/78(신규 M7 8건 포함, skipped=0 → `Assert.Ignore` 폴백 미발동) · 네이티브 캡처 7장 1280x800(`Builds/m7-diagnostics/`, crankStrokeDeg 150.0, readerTriangles 8028). 세 프로파일 전부 `runtimeApproved:false`이고 게이트가 꺼진 상태에서는 커밋된 허브 씬·UI 리터럴·스테이지 흐름이 무변경이다(GateOff 3테스트 실증). 승격은 `Tools/M7/Approve …(director only)` + decision-log 감사(asset-runbook §3.1 8항) 이후이며 사람 플레이(n=0)·G4/G5/G6/G7·인엔진 타일 이음새 실측·라이선스는 열려 있다. 상세는 decision-log의 RFC-CX-013 delivery evidence와 `systems/tech-verification/concept-first-m7/runtime/verification.{md,json}`.
