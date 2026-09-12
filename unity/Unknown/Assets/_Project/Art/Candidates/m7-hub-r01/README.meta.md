---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: draft
supersedes: null
owner: game-modeler
artifact: unity/Unknown/Assets/_Project/Art/Candidates/m7-hub-r01/
---

# m7-hub-r01 — M7 당직실 셸 재질 후보 (RFC-CX-013 HubShell)

`Tools/M7/Import hub shell candidates`(`Editor/M7HubProjectBuilder.cs`)가 `assets/generated/2d/texture/m7-{salt-concrete,perforated-steel,bronze}-r01/{basecolor,roughness}.png`(provenance sha 대조 후)를 `textures/`로 복사하고 `Tide/Candidate Roughness` 재질 3종과 `Resources/M7Hub.asset`을 만든다. 런타임 `App/M7HubSession.cs`는 `runtimeApproved || --m7-hub-diagnostic`일 때만 허브 루트 8개(`Authored-*` 3·`Workbench-*` 5)의 sharedMaterial을 교체하고 Flat ambient + 황토 point light 1개를 더한다. 승인 r03 서랍은 대상이 아니다.

`runtimeEligible:false` · `runtimeApproved:false`. 승격은 네이티브 검수 후 `Tools/M7/Approve hub shell (director only)` + decision-log 감사로만. 원본 출처·해시: `assets/generated/2d/texture/m7-*-r01/provenance.json`, 임포트 영수증: `Builds/m7-hub-import-audit.json`.
