---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: draft
supersedes: null
owner: game-presentation-director
artifact: unity/Unknown/Assets/_Project/Art/Candidates/m7-reader-r01/
---

# m7-reader-r01 — M7 광학 판독기 3D 스테이지 후보 (RFC-CX-013 ReaderStage)

`Tools/M7/Import reader stage candidates`(`Editor/M7ReaderProjectBuilder.cs`)가 `assets/generated/3d/concept-first-m7/fbx/{SM_Prop_OpticalReader,SM_Prop_RecordSet}.fbx`(`fbx-export-report.json` `targets[].sha256` 대조 후)를 `OpticalReader.fbx`/`RecordSet.fbx`로, `assets/generated/2d/texture/m7-{bronze,perforated-steel,salt-crystal,rag-paper}-r01/{basecolor,roughness}.png`(각 폴더 `provenance.json` sha 대조 후)를 `textures/M7_Reader_*_{BaseColor,Roughness}.png`로 복사한다. FBX 임포터는 `c1-signature-reader-r02/Reader.fbx.meta`와 같이 파일 스케일 유지·축 변환 미베이크·카메라/라이트/애니메이션 미임포트·읽기 불가·메시 압축 없음이며 재질은 에디터에서 URP Lit 7종(`MAT_M7_Reader_{Bronze,Steel,Crystal,Paper,Wood,DarkSteel,Glass}`, 텍스처 재질은 `*_MetallicSmoothness.asset` 패킹)으로 만든다. `OpticalReader.prefab`에는 `rd-crank-axle` 월드 바운즈 중심에 `M7 crank pivot`(로컬 X = 회전축, 휴지 자세 identity)을 만들고 `rd-crank-axle`·`rd-crank-arm`·`rd-crank-handle`을 그 아래로 옮긴다. `Resources/M7ReaderStage.asset`(`Presentation/M7ReaderStageProfile.cs`)에 두 프리팹과 `rd-hex-plate` 중심 `lookAt`, 고정 `cameraPosition`을 기록한다.

런타임 `App/M7ReaderSession.cs`는 `runtimeApproved || --m7-reader-diagnostic`일 때만 `M7 optical reader` 루트(프리팹 2개 + point light 2개 `M7 reader lamp`/`M7 reader fill`)를 세션 아래에 만들고 카메라를 진입 시 한 번만 고정한다. 크랭크 스트로크는 `Resources/T0ReaderVfx.json`(forward 420ms → hold 160ms → return 520ms, `Time.unscaledDeltaTime`)을 따르며 각도는 150° 이하로 제한한다 — 180°에서는 핸들이 벤치 상판을 뚫는 것이 RFC-CX-009 관절 검증에서 확인됐기 때문. `reducedMotion`이면 `reduced_motion_ms` 0 = 휴지 자세로 즉시 스냅, 애니메이션 없음. 블룸/플래시/셰이크/카메라 이동 없음.

`runtimeEligible:false` · `runtimeApproved:false`. 승격은 네이티브 검수 후 `Tools/M7/Approve reader stage (director only)` + decision-log 감사로만. 원본 출처·해시: `assets/generated/3d/concept-first-m7/fbx/fbx-export-report.json`, `assets/generated/2d/texture/m7-*-r01/provenance.json`, 임포트 영수증: `Builds/m7-reader-import-audit.json`.
