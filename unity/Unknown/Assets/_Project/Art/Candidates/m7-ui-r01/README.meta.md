---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: draft
supersedes: null
owner: game-presentation-director
artifact: unity/Unknown/Assets/_Project/Art/Candidates/m7-ui-r01/
---

# m7-ui-r01 — M7 uGUI 스킨 후보 (RFC-CX-013 UiSkin)

`Tools/M7/Import UI skin candidates`(`Editor/M7UiSkinProjectBuilder.cs`)가 `assets/generated/2d/texture/m7-rag-paper-r01/basecolor.png` → `UI_M7_PaperPanel.png`, `assets/generated/2d/texture/m7-bronze-r01/basecolor.png` → `UI_M7_BronzeFrame.png`로 복사(각 폴더 `provenance.json`의 `assets[].sha256` 대조 후, Sprite·Single·sRGB·mip 없음·Repeat·max 1024·Compressed)하고 `Resources/M7UiSkin.asset`(`Presentation/M7UiSkinProfile.cs`)의 `paperPanel`/`bronzeFrame`에 연결한다. 런타임 `App/M7UiSession.cs`는 `runtimeApproved || --m7-ui-diagnostic`일 때만 `GameScreen.Skin`을 채우고, `UI/T0Interface.cs`가 `Work Surface` 첫 자식 `M7 paper` RawImage 1개와 `Header`·`Toolbar` 첫 자식 `M7 frame` RawImage 2개(모두 raycastTarget=false, `uvRect` 타일링 → Repeat 필수)를 깐다. Skin이 null이면 커밋된 리터럴 색(Header `(.05,.12,.15,.94)` 등)이 그대로 유지된다.

`runtimeEligible:false` · `runtimeApproved:false`. 승격은 네이티브 검수 후 `Tools/M7/Approve UI skin (director only)` + decision-log 감사로만. 원본 출처·해시: `assets/generated/2d/texture/m7-{rag-paper,bronze}-r01/provenance.json`, 임포트 영수증: `Builds/m7-ui-import-audit.json`.
