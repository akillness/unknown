---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-qa
---

# M7 원본 컨셉 우선 리소스 — T0 stage-1 런타임 배선 검증 (RFC-CX-013)

[OBSERVED] M7 원본 컨셉에서 파생된 **후보** 리소스 3종(당직실 셸 재질, 넝마종이·청동 UI 스킨, 광학 판독기 3D 스테이지)을 T0 stage-1 런타임에 **게이트 뒤로** 배선했다. 세 프로파일 전부 `runtimeApproved:false`이고 생성 자산 전부 `runtimeEligible:false`이므로 **커밋된 기본 동작은 게이트 오프**다 — hub 씬, UI 리터럴 색, 스테이지 전환 흐름이 그대로 유지된다. 이 문서는 헤드리스 자동 검사와 네이티브 캡처에 대한 **범위 한정 기술 영수증**이며 G4/G5/G6/G7 런타임 승격이 아니다. 기계 판독본은 [verification.json](verification.json).

## 실제 배선

- `T0GameSession` 부분 클래스 3개(`M7HubSession`/`M7UiSession`/`M7ReaderSession`)가 각자 프로파일을 읽고 게이트를 평가한다. 게이트 식은 세 레인 모두 `profile.runtimeApproved || --m7-{hub,ui,reader}-diagnostic`이다.
- 게이트 오프 폴백은 레인별로 명시적이다. hub = 커밋 씬 무변경, ui = 기존 리터럴 색 유지(`Skin`이 null이면 `T0Interface`가 커밋 리터럴 전부 사용), reader = hub 표시 유지·스테이지 미생성.
- 임포터(`Tools/M7/Import all M7 candidates`)는 후보만 들여오고 `runtimeApproved=false`를 강제 기록한다. 승격은 레인별 **디렉터 전용** 별도 메뉴다.
- 진단 캡처(`M7ProjectBuilder.Capture`)는 후보를 **메모리에만** 적용하며 hub 씬을 저장하지 않는다.

## 실행 명령

[OBSERVED] Unity **6000.5.6f1**, URP **17.5.0**, 프로젝트 `unity/Unknown`. 별도 표기 없으면 전부 `-batchmode -nographics --burst-disable-compilation`이다.

```
UNITY=/Applications/Unity/Hub/Editor/6000.5.6f1/Unity.app/Contents/MacOS/Unity
PROJ=/Users/jangyoung/orca/unknown/unity/Unknown
EVID=_workspace/current/systems/tech-verification/concept-first-m7/runtime

# 1) 임포트 (후보 반입; exit 0)
$UNITY -batchmode -nographics --burst-disable-compilation -projectPath $PROJ \
  -executeMethod Tide.EditorTools.M7ProjectBuilder.ImportAll \
  -logFile $EVID/m7-import.log

# 2) EditMode (exit 0)
$UNITY -batchmode -nographics --burst-disable-compilation -projectPath $PROJ \
  -runTests -testPlatform EditMode -assemblyNames Tide.Tests.Sim \
  -testResults $EVID/editmode.xml -logFile $EVID/editmode.log

# 3) PlayMode — 10개 클래스 (exit 0)
$UNITY -batchmode -nographics --burst-disable-compilation -projectPath $PROJ \
  -runTests -testPlatform PlayMode \
  -testFilter Tide.Tests.T0PlayModeTests;Tide.Tests.T0CaseThreadTests;Tide.Tests.T0ResourceInteractionTests;Tide.Tests.C1PlayModeTests;Tide.Tests.C1SignaturePlayModeTests;Tide.Tests.M5DirectionPlayModeTests;Tide.Tests.ReviewNotesPlayModeTests;Tide.Tests.M7HubPlayModeTests;Tide.Tests.M7UiSkinPlayModeTests;Tide.Tests.M7ReaderPlayModeTests \
  -testResults $EVID/playmode.xml -logFile $EVID/playmode.log

# 4) 네이티브 캡처 — -nographics 없음 (그래픽 필요; exit 0)
$UNITY -batchmode --burst-disable-compilation -projectPath $PROJ \
  -executeMethod Tide.EditorTools.M7ProjectBuilder.Capture \
  -logFile $EVID/m7-capture.log
```

[OBSERVED] 출력 규약: 테스트 실행은 `-testResults <이 영수증 디렉터리>/<run>.xml` + `-logFile <...>/<run>.log`, 임포트·캡처는 로그만 남긴다. 임포트 감사 JSON은 `unity/Unknown/Builds/`, 캡처 산출물은 `unity/Unknown/Builds/m7-diagnostics/`로 빌더가 직접 기록한다.

## 결과

| 검사 | 실제 결과 | 영수증 |
|---|---|---|
| EditMode (`Tide.Tests.Sim`) | exit 0 · **53/53 통과**, 실패0, 건너뜀0 | [XML](editmode.xml) · [log](editmode.log) |
| PlayMode (10개 클래스) | exit 0 · **78/78 통과**, 실패0, 건너뜀0, 미결론0 | [XML](playmode.xml) · [log](playmode.log) |

[OBSERVED] PlayMode 78개 클래스별 내역. M7 신규 8개는 이 78개에 포함되므로 중복 합산하지 않는다.

| 클래스 | 검사 수 |
|---|---:|
| `T0PlayModeTests` | 15 |
| `M5DirectionPlayModeTests` | 15 |
| `ReviewNotesPlayModeTests` | 15 |
| `C1SignaturePlayModeTests` | 10 |
| `C1PlayModeTests` | 7 |
| `T0ResourceInteractionTests` | 6 |
| `M7UiSkinPlayModeTests` | 3 |
| `M7ReaderPlayModeTests` | 3 |
| `M7HubPlayModeTests` | 2 |
| `T0CaseThreadTests` | 2 |
| 합계 | **78** |

### M7 신규 8개 — 전부 Passed

| 클래스 | 검사 | 입증 내용 |
|---|---|---|
| `M7HubPlayModeTests` | `GateOffLeavesCommittedHubShellUntouched` | 게이트 오프에서 커밋 hub 셸 무변경 |
| `M7HubPlayModeTests` | `GateOnSwapsListedRootsAddsOneLampAndFlatAmbientOnce` | 게이트 온에서 명시 루트만 교체 · 램프 1개 · 평탄 앰비언트 1회 |
| `M7UiSkinPlayModeTests` | `GateOffKeepsCommittedLiteralsAndNoSkinBackings` | 게이트 오프에서 커밋 리터럴 유지 · 스킨 배경 미삽입 |
| `M7UiSkinPlayModeTests` | `GateOnInsertsNonRaycastBackingsAndKeepsButtonsClickable` | 배경이 raycast를 먹지 않고 버튼 클릭 유지 |
| `M7UiSkinPlayModeTests` | `GateOnAtTextScale150RendersWithoutLayoutChange` | 검사명이 지정한 텍스트 배율 150에서 레이아웃 변형 없음 |
| `M7ReaderPlayModeTests` | `GateOffNeverEntersReaderStage` | 게이트 오프에서 스테이지 진입 자체가 없음 |
| `M7ReaderPlayModeTests` | `GateOnBuildsStageWithTwoLightsAndTearsDownOnExit` | 조명 정확히 2개로 구성 · 이탈 시 해체 |
| `M7ReaderPlayModeTests` | `GateOnReducedMotionKeepsCrankAtRest` | reduced-motion에서 크랭크 정지 유지 |

[OBSERVED] **건너뜀0**이 핵심이다. 후보 자산이 없으면 `Assert.Ignore` 폴백이 걸리게 되어 있는데 그것이 한 건도 발동하지 않았으므로, 세 레인의 후보가 실제로 존재했고 게이트 오프/게이트 온 **두 경로가 모두 실제로 실행**됐다. 게이트 오프 3개 검사가 커밋 동작 보존을 직접 입증한다.

## 네이티브 캡처

[OBSERVED] `-nographics` 없이 실행해 exit 0, graphicsDevice **Metal**, **1280x800** 7장을 `unity/Unknown/Builds/m7-diagnostics/`에 기록했다. 보고서는 같은 폴더의 `m7-diagnostics.json`이다. 캡처는 후보를 메모리에만 적용하고 **hub 씬을 저장하지 않는다**. [log](m7-capture.log)

| 샷 | 입증 내용 |
|---|---|
| `hub-before.png` | 게이트 오프 기준선 — 커밋 hub의 실제 화면 |
| `hub-after-m7-shell.png` | 셸 재질만 적용한 상태의 차이 |
| `hub-after-m7-shell-and-ui-skin.png` | 셸 + UI 스킨 동시 적용 시의 대비·가독성 |
| `hub-after-m7-viewport.png` | 실제 플레이 뷰포트 구도에서의 셸 판독 |
| `reader-stage-rest.png` | 판독기 스테이지 정지 포즈 구성 |
| `reader-stage-viewport.png` | 해결된 카메라로 본 스테이지 구도 |
| `reader-stage-crank-150deg.png` | 크랭크 150° 스트로크 끝단 |

[OBSERVED] `m7-diagnostics.json` 수치: `crankPivotFound:true`, `crankStrokeDeg:150.0`, `readerStageBounds.size [1.48191333, 0.6113224, 0.8704778]` m, `readerTriangles 8028`, `assets.runtimeApproved {hub:false, ui:false, reader:false}`. 런타임 관측 삼각형 8028은 Blender 후보 근사치 합(판독기 7824 + 기록 세트 204)과 일치한다.

[OBSERVED] 판독기 카메라는 **임포트된 인스턴스의 실측 바운드에서 해결**했고 `Builds/m7-reader-import-audit.json`에 기록됐다. `lookAt [0.0914999843, 0.195000052, -2.98023224e-08]`, `cameraPosition [-0.458500028, 0.615, 1.15]`, `horizontalFov 48`.

## 임포트 영수증

[OBSERVED] `ImportAll` exit 0. 로그 [m7-import.log](m7-import.log)의 실제 줄:

```
M7_HUB_SHELL_IMPORTED textures=6 materials=3 runtimeApproved=false
M7_UI_SKIN_IMPORTED textures=2 runtimeApproved=false
M7_READER_STAGE_IMPORTED fbx=2 textures=8 materials=7 runtimeApproved=false
M7_IMPORT_ALL_DONE
```

감사 JSON은 `unity/Unknown/Builds/m7-hub-import-audit.json`, `m7-ui-import-audit.json`, `m7-reader-import-audit.json`이다. 세 줄 모두 `runtimeApproved=false`를 그대로 찍는다 — 임포트는 승격이 아니다.

[OBSERVED] FBX 후보는 `scripts/blender/export_m7_fbx.py`(Blender 5.1.2 헤드리스)로 4개 내보냈고 보고서는 `assets/generated/3d/concept-first-m7/fbx/fbx-export-report.json`이다. 근사 삼각형은 `SM_Env_Watchroom` 30818, `SM_Prop_OpticalReader` 7824, `SM_Prop_RecordSet` 204, `SM_Env_GateThree` 34928. 원본 blend sha는 내보내기 전후 **변하지 않았고** 텍스처는 **임베드하지 않았다**. 런타임이 실제로 인스턴스화한 것은 판독기와 기록 세트 2개(`fbx=2`)다.

## 실패·원인 규명 이력

1. [OBSERVED] `-batchmode`를 **그래픽과 함께** 돌리면 Burst 백그라운드 컴파일러에서 segv가 났다(`Burst.Compiler.IL...` 네이티브 스택, exit 139). 그래서 M7 배치 실행 전부 `--burst-disable-compilation`을 붙인다. 프로젝트 코드가 원인이 아니다.
2. [OBSERVED] `EditorSceneManager.OpenScene`/`NewScene`이 매니지드 참조로만 도달하는 자산을 언로드해, 로드된 프로파일의 `Material`/`GameObject` 필드가 null이 되고 첫 캡처가 5장 대신 **1장만** 남겼다. 수정은 씬 전환마다 세 프로파일을 다시 `Resources.Load`하는 것(`Editor/M7ProjectBuilder.cs`의 `Reload()`)이다.
3. [OBSERVED] FBX 축 변환이 X를 미러링한다. 그래서 판독기 스테이지 카메라 방향을 Blender 좌표 가정이 아니라 **임포트된 인스턴스에서 측정**한다 — 크랭크 쪽은 피벗의 월드 x, 정면은 `rd-thumbscrew`의 z로 잡는다.
4. [OBSERVED] `hub-after-m7-viewport.png`에서 타공 강판 작업대가 현재 타일링·조명값으로는 거의 평탄한 어두운 덩어리로 읽힌다. **승격 전에 타일링·밝기 조정이 필요하다.** 소금 콘크리트 벽·바닥과 조위 띠는 제대로 읽힌다.
5. [OBSERVED] 청동 프레임 배경 틴트를 `(.5,.5,.5,.72)`로 낮춰 헤더·툴바 텍스트가 텍스처 위에서 대비를 유지하게 했다.

## 경계와 후속 작업

[OBSERVED] 커밋된 기본 동작은 **게이트 오프**다. 게이트 오프에서 hub 씬·UI 리터럴·스테이지 흐름은 손대지 않으며, 이는 위 게이트 오프 3개 검사로 입증됐다. 모든 M7 프로파일은 `runtimeApproved:false`, 모든 생성 자산은 `runtimeEligible:false`로 남는다.

[CARRIED] 이 검사로 **입증하지 않은 것**:

- 사람 플레이테스트 (n=0).
- G4/G5/G6/G7 런타임 승격.
- 엔진 내 타일 심·밉·텍셀 밀도 측정. 수행된 타일링 QA는 **엔진 이전** 단계였다.
- 라이선스 — GTI 백엔드는 `UNVERIFIED`다.
- 메시 단위 충돌.
- 크랭크–판 기어 연결.
- 손 리그.
- gate-three의 런타임 사용 (`SM_Env_GateThree`는 내보냈지만 런타임이 인스턴스화하지 않는다).

[CARRIED] 승격 경로는 **디렉터 전용 승인 + 별도 감사**다. 레인별 메뉴는 `Tools/M7/Approve hub shell (director only)`, `Tools/M7/Approve UI skin (director only)`, `Tools/M7/Approve reader stage (director only)`이며, 그 전에 [`handoff/asset-runbook.md`](../../../../handoff/asset-runbook.md) §3.1의 **8개 감사 항목**을 채운 RFC를 `handoff/rfc-inbox/`에 제출해 디렉터가 `production/decision-log.md`에 판정을 append해야 한다. 실행자는 `decision-log.md`를 직접 편집하지 않는다. 이 문서는 그 감사의 기술 입력이며 판정 자체가 아니다.
