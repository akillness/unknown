---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-qa
---

# M7/M8 컨셉 프로파일 런타임 승격 — 승격 영수증 (RFC-CX-016 · M13)

[OBSERVED] 게이트 뒤에 있던 컨셉 프로파일 4종(`M7Hub`·`M7UiSkin`·`M7ReaderStage`·`M8ReviewNotes`)을 `runtimeApproved:true`로 올리고, 승격된 상태에서 **전수 재검증 → macOS 플레이어 빌드 → 출하 플레이어 네이티브 스모크**를 실행했다. 이 문서는 그 실행에 대한 **범위 한정 기술 영수증**이며, 재미·몰입·성능에 대한 판정이 아니고 G4/G7 런타임 승격도 아니다. 기계 판독본은 [verification.json](verification.json).

이 영수증의 모든 수치는 이 폴더의 산출물에서 직접 읽었다. 승격 레인이 1시간 21분 지점에 중단돼 자기 영수증을 쓰지 못했으므로, 본 문서는 **후속 QA가 산출물과 `git diff`로 역산해 작성**한 것이다(§7·§8).

## 1. 범위와 권한

[OBSERVED] 승격 권한의 근거는 `production/decision-log.md:562-570`의 RFC-CX-016 블록이다.

- 각 레인의 승격 메뉴는 설계상 **"director only"**다(`concept-first-m7/runtime/verification.md:147`).
- 루트 디렉터인 사용자가 2026-09-11 지시로 **명시 승격을 요청**했고, 디렉터가 그 지시를 decision-log에 승격 감사 기록으로 append했다(`decision-log.md:566`).
- 실행자는 decision-log를 직접 편집하지 않는다. 본 문서는 그 감사의 **기술 입력**이며 판정 자체가 아니다.

[OBSERVED] **경계** — `decision-log.md:570`이 명시한 그대로다.

- 승격은 **T0 stage-1 범위**다.
- 상업 출시 자격 `commercialReleaseEligible`은 **변경하지 않았다**.
- 시각 원전은 여전히 원본 컨셉·M7 계보(RFC-CX-009)이며, **현재 런타임 화면을 새 아트 기준으로 삼지 않는다**.

## 2. 승격 전후 게이트 상태

[OBSERVED] 승격은 `Tools/M7/Approve all lanes (director only)`(신규 `Editor/M7ProjectBuilder.cs:ApproveAll`)로 실행했다. 이 진입점은 **스스로 `runtimeApproved`를 쓰지 않고** 기존 레인별 디렉터 전용 메뉴 4개를 호출만 하며, 호출 전후의 게이트 상태를 `Resources.Load`와 `AssetDatabase` **두 경로 모두**로 기록한다.

| 프로파일 | 이전 | 현재 | 산출물(프로파일 자산) | 게이트 오프 폴백 계약 | 폴백 유효? |
|---|---|---|---|---|---|
| `M7Hub` | `false` | `true` | `Resources/M7Hub.asset:15` · `floorAndWall`/`workbench`/`plateShelf` 3개 참조 해결 | hub 씬 **무변경** | **유효** — `GateOffLeavesCommittedHubShellUntouched` Passed |
| `M7UiSkin` | `false` | `true` | `Resources/M7UiSkin.asset:15` · `paperPanel`/`bronzeFrame` 참조 해결 | 기존 **리터럴 색** 유지 | **유효** — `GateOffKeepsCommittedLiteralsAndNoSkinBackings` Passed |
| `M7ReaderStage` | `false` | `true` | `Resources/M7ReaderStage.asset:15` · `reader`/`recordSet` 참조 해결 | 스테이지 **미생성** | **유효** — `GateOffNeverEntersReaderStage` Passed |
| `M8ReviewNotes` | `false` | `true` | `Resources/M8ReviewNotes.asset:16` · `cardPaper` 참조 해결 | 검토 카드 미삽입 | **유효** — `ReviewNotesPlayModeTests` 15/15 Passed |

[OBSERVED] "이전 = false" 4건의 근거는 **두 갈래로 독립 확인**된다.

1. `approve-all.log:485`의 `M7_GATES_BEFORE` — 네 프로파일 전부 `"runtimeApproved":false`.
2. `git diff` — 네 자산 모두 `-  runtimeApproved: 0` → `+  runtimeApproved: 1`. HEAD 커밋 상태가 전부 0이다.

[OBSERVED] **영수증 정합성 주의 — 이 폴더의 `m7-approval-audit.json`은 "이전=전부 false"를 보여주지 않는다.** 그 파일의 `before` 블록은 `M7Hub`만 false이고 나머지 셋은 이미 true다(`m7-approval-audit.json:4-29`). 이는 결함이 아니라 **덮어쓰기**의 결과다. 승격이 2회 실행됐고 같은 경로에 기록했다:

| 순서 | 로그 | `M7_GATES_BEFORE` | `M7_GATES_AFTER` |
|---|---|---|---|
| 1회차 10:29 | `approve-all.log:485,557` | hub·ui·reader·card **전부 false** | 전부 true |
| — 11:21 | `hub-reimport.log:582` | `M7_HUB_SHELL_IMPORTED textures=6 materials=3 **runtimeApproved=false**` | — |
| 2회차 11:21 | `approve-all-2.log:376,448` | **hub만 false**, 나머지 셋 true | 전부 true |

[OBSERVED] 인과는 로그에서 그대로 읽힌다. §7의 금속감·타일링 수정 때문에 허브 재질을 **재임포트**해야 했고, 임포터는 설계대로 `runtimeApproved=false`를 강제 기록한다(임포트는 승격이 아니다 — `concept-first-m7/runtime/verification.md:17,120`). 그래서 `M7Hub`만 false로 되돌아갔고 2회차 승격이 필요했다. 디스크의 `m7-approval-audit.json`은 **2회차 스냅샷**이므로, 이 회차 전체의 참 기준선은 `approve-all.log:485`다.

## 3. 검증 표

[OBSERVED] 전부 승격 **이후** 상태에서 실행했다. Unity 6000.5.6f1 / URP 17.5.0 / `unity/Unknown`.

| 검사 항목 | 결과 | 영수증 (파일 · sha256 앞16) |
|---|---|---|
| 게이트 승격 감사 (2회차) | `M7_APPROVE_ALL_DONE` · 4레인 false→true | `m7-approval-audit.json` `7a7e86e7435f6d7c` |
| 게이트 승격 감사 (1회차, 참 기준선) | `M7_GATES_BEFORE` 4건 전부 false | `approve-all.log` `fff46ff86cb5d579` |
| 허브 재질 재임포트 | `M7_HUB_SHELL_IMPORTED textures=6 materials=3 runtimeApproved=false` | `hub-reimport.log` `56a88f5b90d080dd` |
| 프로파일 참조 프로브 #1 | **8/8 Passed**, 실패0, 건너뜀0 | `m7-probe.xml` `ef636ce3c7a6a14e` |
| 프로파일 참조 프로브 #2 | **9/9 Passed**, 실패0, 건너뜀0 | `m7-probe2.xml` `3ab2a060269ec26b` |
| EditMode `Tide.Tests.Sim` | **53 총 · 53 passed · 0 failed · 0 skipped** | `editmode-final.xml` `8c33068761d40d29` |
| PlayMode `Tide.Tests.Play` | **83 총 · 82 passed · 0 failed · 1 skipped** | `playmode-final.xml` `d64bee308ee6a033` |
| 격리 부트 (3회차) | **1 총 · 1 passed · 0 failed · 0 skipped** | `boot-final.xml` `4cd85496193a4b4d` |
| macOS 플레이어 빌드 | `T0_MAC_BUILD Succeeded bytes=403838357` (`:5987`) | `build-final.log` `f4cbd7a593af59be` |
| 네이티브 스모크 (출하 플레이어) | Player.log 예외 **0건** · 부트 마커 `ui-root-loaded`→`hub-loaded`→`session-initialized` · 스크린샷 14장 | `smoke/` 14 png — **Player.log 자체는 미아카이브**(아래) |

[OBSERVED] **영수증 공백 1건.** 위 표의 다른 모든 행은 이 폴더의 파일을 직접 열어 확인했으나, **네이티브 스모크의 `Player.log`는 이 레인에 보관되지 않았다.** 출하 플레이어의 로그는 `~/Library/Logs/` 아래에 남고 레인으로 복사되지 않았다. 따라서 "예외 0건 · 부트 마커 3종"은 **Main의 실행 관측**이며, 재검증 가능한 영수증은 같은 세션에서 나온 `smoke/` 14장뿐이다. 다음 네이티브 회차에서는 `Player.log`를 레인에 복사해 sha와 함께 남긴다(§9 이월).

[OBSERVED] PlayMode의 유일한 skip은 `Tide.Tests.T0BootSceneTests.SerializedBootLoadsHubAndInitializesVisibleStartScreen` 1건이다. §4에서 설계 계약임을 보인다.

### 3.1 격리 부트 3회차 경위 — 실패 2회는 테스트 결함이 아니라 실행 인자 오류였다

[OBSERVED] 부트 검사는 3회 시도해 3회차에 통과했다. 앞선 2회는 **하네스(실행 인자) 오류**이며 제품 코드나 테스트의 결함이 아니다. 귀속을 흐리지 않기 위해 그대로 적는다.

| 시도 | 넘긴 인자 | 실패 양상 | 귀속 |
|---|---|---|---|
| 1 | `-testFilter '*SerializedBootLoadsHub*'` | NUnit이 `*`를 **정규식 수량자**로 파싱 → `ArgumentException`, **XML 미생성** | 하네스 오류 (Main 실행 인자) |
| 2 | `--t0-save-dir /tmp/unknown-m13-boot-$$` | 테스트가 `T0BootSceneTests.cs:21`에서 접두사 `/tmp/unknown-c1-m4-boot-`를 단정 → 실패 | 하네스 오류 (Main 실행 인자) |
| 3 | 정확한 전체 이름 + `--t0-save-dir /tmp/unknown-c1-m4-boot-m13-19162` | **Passed 1/1** | — |

[OBSERVED] 3회차의 실제 인자는 `boot-final.log:24-25`에 `--t0-save-dir` / `/tmp/unknown-c1-m4-boot-m13-19162`로 남아 있다 — 규정 접두사를 만족한다. 접두사 단정은 `T0BootSceneTests.cs:22`의 `Assert.IsFalse(Directory.Exists(candidate))`와 짝을 이뤄 **테스트가 자기 디렉터리를 새로 소유**하게 강제하는 안전장치다. 2회차 실패는 그 안전장치가 **설계대로 작동한 것**이다.

## 4. 베이스라인 대조 (M12 → M13)

[OBSERVED] 직전 회차는 `native-playtest-m9/`(M12)다. 양쪽 XML을 직접 파싱해 대조했다.

| 검사 | M12 (`native-playtest-m9/`) | M13 (현재) | 증감 |
|---|---|---|---|
| EditMode | 53 총 · 53 passed · 0 skipped | 53 총 · 53 passed · 0 skipped | 변화 없음 |
| PlayMode | **79 총 · 73 passed · 6 skipped** | **83 총 · 82 passed · 1 skipped** | 총 **+4** · passed **+9** · skipped **−5** |
| 격리 부트 | 1/1 Passed | 1/1 Passed | 변화 없음 |
| macOS 빌드 | 354,247,240 B | 403,838,357 B | **+49,591,117 B** (§6) |

[OBSERVED] 총 검사 수 증가 +4의 분해 — 두 XML의 `fullname` 집합 차분 결과다. **제거된 검사는 0건**이다.

| 증가분 | 건수 | 검사 |
|---|---:|---|
| 신규 — 눈금 선택기 도달성 회귀 (`ScrollReachabilityTests`) | 3 | `PointerWheelOverTheWorkSurfaceScrollsTheLongestList` · `RealMouseWheelReachesTheListThroughTheInputModule` · `KeyboardAloneReachesEveryTickRowWithTheScrollFollowing` |
| 신규 — M7 판독기 조명 복원 계약 | 1 | `M7ReaderPlayModeTests.GateOnSuppressesForeignHubLightsAndRestoresThemOnExit` |
| **합계** | **+4** | 79 + 4 = **83** |

[OBSERVED] passed 증가 +9의 분해 = 신규 4건 + **skipped→passed 전환 5건**.

| 전환된 검사 (M12 Skipped → M13 Passed) |
|---|
| `M7HubPlayModeTests.GateOnSwapsListedRootsAddsOneLampAndFlatAmbientOnce` |
| `M7UiSkinPlayModeTests.GateOnInsertsNonRaycastBackingsAndKeepsButtonsClickable` |
| `M7UiSkinPlayModeTests.GateOnAtTextScale150RendersWithoutLayoutChange` |
| `M7ReaderPlayModeTests.GateOnBuildsStageWithTwoLightsAndTearsDownOnExit` |
| `M7ReaderPlayModeTests.GateOnReducedMotionKeepsCrankAtRest` |

[OBSERVED] 이 5건은 M12에서 자산 미임포트·게이트 오프로 `Assert.Ignore` 폴백이 걸려 있었다. 승격과 임포트로 **게이트 온 경로가 실제로 실행 가능**해졌으므로 전환됐다. 즉 이 5건은 "새로 통과한 것"이 아니라 **처음으로 실제 측정된 것**이다.

### 남은 skip 1건은 결함이 아니라 설계 계약이다

[OBSERVED] `T0BootSceneTests.SerializedBootLoadsHubAndInitializesVisibleStartScreen`는 `T0BootSceneTests.cs:19-20`에서 이렇게 자기를 건너뛴다.

```csharp
var args=Environment.GetCommandLineArgs();int index=Array.IndexOf(args,"--t0-save-dir");
if(index<0||index+1>=args.Length)Assert.Ignore("Run with an explicit isolated --t0-save-dir for serialized boot verification.");
```

[OBSERVED] 이 검사는 **자기 소유의 격리 저장 디렉터리를 요구**한다(`:21-22`에서 접두사와 비존재를 단정하고, `:38`에서 teardown이 그 디렉터리를 통째로 삭제한다). 전수 PlayMode 실행에는 `--t0-save-dir`를 주지 않으므로 **설계대로 건너뛴다**. 그 계약을 만족시키는 별도 격리 실행이 `boot-final.xml`의 1/1 Passed이며, 이는 M12에서도 동일했다(M12 skipped 6건 중 1건이 같은 검사였다). **결함 이월이 아니다.**

## 5. 콘셉 흐름 대조

[OBSERVED] 대조 대상은 RFC-CX-015 영상 `docs/media/gameplay-m9/gameplay.mp4`의 4개 챕터 카드다. 챕터명은 `gameplay.ass:16,19,22,25`와 `index.html:45-48`에서 그대로 읽었다. 화면 측 증거는 **이번 회차 네이티브 스모크(출하 플레이어, 실제 창)** 14장에서만 골랐다.

| 영상 챕터 | 영상이 보여주는 것 | 대응 스모크 샷 | 스모크에서 실제로 확인되는 것 |
|---|---|---|---|
| **01 당직 인수** | 각서와 목록을 열람하고 상시 슬롯 준비 | `smoke/09-hub-nodes.png` | M5 오프닝 **원본 컨셉 컷** — 야간 당직실 회화(창·달빛 바다·램프·책상·전화기·트레이)와 모토 문장. 원전 그대로의 연출 |
| **01 당직 인수** (승격 후 실동작) | 〃 | `smoke/11-hub-live.png` | M7 허브 셸(소금 콘크리트 벽·작업대) + **UI 스킨 켜짐**(넝마종이 문서 패널·청동 툴바) + 인수 각서 7행 본문 가독 + 상태줄이 종이 위에서 읽힘 |
| **02 시간 눈금** | 눈금을 끌어 내려 결손 구간의 끝 선택 | — | **이월** (§9). 이번 스모크는 `t0-b1` 인수 절차 구간에서 종료돼 `t0-b3` 눈금 선택기 화면을 촬영하지 않았다 |
| **03 판독 · 대조** | 표준판을 판독해 사본을 남기고 시간창 고정 | `smoke/13-reader-node.png` | 허브 노드 목록에서 **판독기 노드 선택** · 시점 이동. 사건 흐름 패널이 "결손 4시간의 양 끝을 두 기록으로 고정" 유지 |
| **03 판독 · 대조** | 〃 | `smoke/14-reader-stage.png` | **광학 판독기 스테이지 실물** — 확대경 암·육각 판·기록 세트(장부·종잇장)·청동 기둥. 배경이 어둡고 스테이지 조명 2개만 살아 있다(§7 조명 복원 수정의 시각 확인) |
| **04 자료 교체** | 매체명으로 자료를 바꿔 다시 판독하고 인용 | 부분 — `smoke/14-reader-stage.png` | 우측 패널 헤더가 **"자료 선택 / 교체"**로 떠 있다. 다만 **교체 후 재판독까지 진행한 샷은 없다** → 이월 (§9) |

[OBSERVED] 흐름 대조에서 **메우지 못한 차이**는 위 표의 "이월" 2건이다. 이 회차의 스모크는 `t0-b1` 구간에 머물렀고 `t0-b3`의 눈금 선택기·인용 확정·자료 교체 후 재판독은 촬영되지 않았다. **새 이미지를 생성해 메우지 않는다** — 그 구간의 화면 증거는 직전 회차 `native-playtest-m9/`의 `shots/`에 이미 있으나, 그것은 **승격 전** 화면이므로 이 표에 승격 후 증거로 올릴 수 없다. 다음 네이티브 회차에서 `t0-b3`까지 진행해 촬영하는 것이 해소 경로다.

## 6. 빌드 크기 — 교란된 예측

[OBSERVED] 자매 레인 `legacy-purge-m13/`은 검사 가능한 예측을 냈다: 레거시 후보를 제거하면 **빌드가 354,247,240 B보다 줄어야 한다**. 실측은 **403,838,357 B, 즉 +49,591,117 B(+47.3 MiB) 증가**다.

[OBSERVED] 이 예측은 **반증되지 않았고 교란(confounded)됐다.** 같은 빌드에 퍼지와 **M7 3레인 + M8 승격이 동시에** 들어갔기 때문이다. 승격으로 소스 아트가 빌드 경로에 편입된 규모와 제거된 레거시의 규모는 자릿수가 다르다.

| 항목 | 실측 | 측정 방법 |
|---|---:|---|
| `m7-reader-r01` (판독기 FBX·텍스처·재질) | 49,256 KB | `du -sk` |
| `m7-hub-r01` (허브 셸 텍스처·재질) | 13,720 KB | `du -sk` |
| `m7-ui-r01` (넝마종이·청동 UI 스킨) | 6,596 KB | `du -sk` |
| `m8-review-card` (검토 카드) | 3,316 KB | `du -sk` |
| **승격으로 편입된 소스 아트 합** | **72,888 KB ≈ 71.2 MiB** | 합산 |
| 제거된 `hub-greybox.fbx` | **51,148 B ≈ 49.95 KiB** | `git cat-file -s HEAD:…/hub-greybox.fbx` |

[OBSERVED] 편입된 소스 아트는 제거된 greybox보다 **약 1,459배 크다** — 예측이 기대한 감소분보다 3자릿수 큰 증가 요인이 같은 빌드에 들어갔다. 퍼지가 지운 총 바이트는 34개 파일 20,845,750 B이지만(`legacy-purge-m13/purge-receipt.json`), 그중 대부분(`c1-signature-reader-r01`)은 **애초에 플레이어 빌드에 포함되지 않는 미참조 후보**였으므로 삭제 소스 바이트가 빌드 절감분과 같지 않다.

[OBSERVED] 따라서 **퍼지 단독 기여분은 이 회차에서 분리되지 않았다.** "빌드가 줄었다"고도, "퍼지 예측이 틀렸다"고도 단정할 수 없다.

[CARRIED] **분리 방법** — 현재 승격 상태를 유지한 채 퍼지 커밋만 되돌린 대조 빌드 1회를 돌리고 `T0_MAC_BUILD bytes`를 현재 403,838,357 B와 비교하면 퍼지 단독 기여분이 나온다.

## 7. 수리 목록 — `git diff` 역산

[OBSERVED] 승격 레인이 중단 전까지 실제로 무엇을 고쳤는지 작업 트리 diff에서 역산했다. 각 변경은 코드 주석에 RFC-CX-016 근거와 **측정값**이 함께 남아 있어 의도를 읽을 수 있다. 아래는 diff에서 읽힌 것만 적는다.

| 파일 | diff 규모 | 읽힌 의도 | 판정 |
|---|---|---|---|
| `App/M7ReaderSession.cs` | +31/−? | `SuppressHubLights()` 신설 — 판독기 스테이지 진입 시 허브의 외래 조명을 전부 끄고 목록을 `M7ReaderStageVisual`에 넘긴다. `OnDestroy()`가 복원. 주석에 원인 측정 기록: 커밋 `Watch room lamp`는 **Renderer 없는 Light**여서 렌더러 기준 루트 숨김이 놓쳤고, 뷰포트의 5.5–5.7%가 전 채널 255로 클리핑됐다 | 의도 명확 |
| `Presentation/M7UiSkinProfile.cs` | +6 | 신규 필드 `statusOnPaper = (.18,.08,.03)`. 주석에 측정: 커밋 리터럴 `(.38,.18,.07)`은 넝마종이 위에서 **3.53:1**, 새 색은 같은 종이에서 **4.83:1** | 의도 명확 |
| `UI/T0Interface.cs` | +4/−2 | 상태줄 색을 `skin==null ? 커밋 리터럴 : skin.statusOnPaper`로 분기. **게이트 오프는 바이트 동일 유지** | 의도 명확 |
| `Editor/M7HubProjectBuilder.cs` | +13/−4 | 금속감·타일링 교정. PerforatedSteel `Metallic .85→.25` `Tiling 2→4`, Bronze `Metallic 1→.45` `Tiling 3→4`. 주석에 원인: 허브 씬에 **반사 프로브도 스카이박스도 없어** flat ambient만 샘플링되므로 높은 metallic이 albedo를 F0로 접고 diffuse를 (1−metallic)로 깎는다. 측정 기록: 작업대 42.6/53.6/55.6 vs 벽 75.0/84.6/86.0 | 의도 명확 |
| `Art/Candidates/m7-hub-r01/MAT_M7_Hub_PerforatedSteel.mat` | +3/−3 | 위 교정의 **직렬화 결과** — `_Metallic .85→.25`, `m_Scale 2→4` | 의도 명확 |
| `Art/Candidates/m7-hub-r01/MAT_M7_Hub_Bronze.mat` | +3/−3 | 위 교정의 직렬화 결과 — `_Metallic 1→.45`, `m_Scale 3→4` | 의도 명확 |
| `Editor/M7ProjectBuilder.cs` | +38 | `ApproveAll()` 신규 — 4개 디렉터 전용 메뉴를 호출만 하고 전후 게이트를 두 로드 경로로 감사(§2). batchmode에서 메뉴 항목에 닿지 못하므로 `-executeMethod` 진입점 | 의도 명확 |
| `Tests/PlayMode/M7ReaderPlayModeTests.cs` | +27 | 신규 `GateOnSuppressesForeignHubLightsAndRestoresThemOnExit` — 진입 시 허브 조명 전부 off, 빌려간 목록이 **진입 전 켜져 있던 집합과 정확히 일치**, 스테이지 자체 조명 2개는 유지, 이탈 후 전부 복원 | 의도 명확 |
| `Resources/M7ReaderStage.asset` | +3/−3 | `runtimeApproved 0→1` 외에 `lampIntensity 2.2→1.25`, `fillIntensity 0.6→0.5` | 조명 과다 노출 대응의 일부로 읽히나, **diff 자체에는 이 두 값에 대한 근거 주석이 없다** → 부분적으로 의도 미확인 |
| `ProjectSettings/URPProjectSettings.asset` | 신규 | `UniversalProjectSettings` · `m_LastMaterialVersion: 10` · `m_ProjectSettingFolderPath: URPDefaultResources` | **diff에서 의도 미확인** — 내용상 URP 패키지가 재질 업그레이드 시 자동 생성하는 설정 파일로 읽히나, 승격 레인이 의도적으로 추가한 것인지 부산물인지 diff만으로는 단정할 수 없다 |

[OBSERVED] **같은 작업 트리에 있으나 승격 레인 소유가 아닌 변경** — 귀속을 섞지 않기 위해 분리해 적는다.

| 파일 | 소유 레인 | 근거 |
|---|---|---|
| `Editor/T0ProjectBuilder.cs` (−26) · `Scenes/hub.unity` (−95) | `legacy-purge-m13/` | 코드 주석이 "RFC-CX-016 (M13 legacy purge)"를 명시하고 `purge-receipt.json`의 `sceneEdit`가 `hub.unity` 403–496행 제거를 기록 |
| `App/T0GameSession.cs` (`KeptClueLines()`) · `Resources/T0Strings.json` (`keptClue*` 4키) · 신규 `Tests/PlayMode/ScrollReachabilityTests.cs` | `remaining-m13/` | `remaining-m13/notes.md:81`이 변경 파일 목록으로 자기 소유를 선언 |

## 8. 중단된 작업 — 마지막 발화 3건의 완결 여부

[OBSERVED] 승격 레인은 **1시간 21분 지점에 중단**됐고 자신의 `verification.md`를 쓰지 못했다. 중단 직전 마지막 발화가 남긴 작업 항목은 3건이었다. 각 항목이 실제로 완결됐는지를 `git diff`와 테스트 결과로 판정한다.

**판정 원칙** — 전수 통과는 "**깨지지 않음**"의 증거이지 "**의도한 모양이 나왔음**"의 증거가 아니다. 코드 반영과 시각 결과를 분리해 판정한다.

| # | 마지막 발화 항목 | 코드·자산 반영 | 검사 증거 | 시각 확인 | **판정** |
|---|---|---|---|---|---|
| 1 | **T0Interface 상태줄 · 자산 필드** | 완료 — `M7UiSkinProfile.cs`에 `statusOnPaper` 신설, `T0Interface.cs:139`가 skin 유무로 분기, `M7UiSkin.asset:25`에 `statusOnPaper: {r:.18,g:.08,b:.03,a:1}` 직렬화 | `M7UiSkinPlayModeTests` 3/3 Passed (게이트 오프 리터럴 보존 포함) | `smoke/11-hub-live.png`·`13-reader-node.png`에서 상태줄 "각서와 목록을 열람하고 상시 슬롯을 준비하세요."가 넝마종이 위에서 읽힌다 | **완료** |
| 2 | **프로브 없는 씬의 고메탈 재질** | 완료 — 빌더 후보값과 `.mat` 직렬화 양쪽 교정, 허브 재질 재임포트 실행(`hub-reimport.log:582`) | `M7HubPlayModeTests` 2/2 Passed — 그러나 이 검사는 **루트 교체·램프 1개·평탄 앰비언트 1회**를 단정할 뿐 **재질 가독성을 단정하지 않는다** | **부정적 관측**: 교정 후 촬영한 `shots/fix-03-hub.png`와 `smoke/11-hub-live.png` 모두에서 작업대는 여전히 **타공이 읽히지 않는 평탄한 어두운 덩어리**다. 벽의 소금 콘크리트는 띠가 또렷이 읽힌다 | **불확실** |
| 3 | **`M7ReaderStageVisual` 복원 훅** | 완료 — `SuppressHubLights()` + `OnDestroy()` 복원, `SuppressedHubLights` 공개 프로퍼티로 계약 노출 | 신규 `GateOnSuppressesForeignHubLightsAndRestoresThemOnExit` **Passed**. 진입 전 켜져 있던 집합과 빌려간 집합의 **동치**까지 단정 | `smoke/14-reader-stage.png`에서 배경이 어둡고 스테이지 조명 2개만 살아 있다 — 과노출 해소가 화면에서 확인된다 | **완료** |

[OBSERVED] **#2를 "불확실"로 두는 이유.** RFC-CX-013이 이월한 결함 서술은 "타공 강판 작업대가 현재 타일링·조명값으로는 거의 평탄한 어두운 덩어리로 읽힌다. 승격 전에 타일링·밝기 조정이 필요하다"였다(`concept-first-m7/runtime/verification.md:129`). 수정은 원인 분석과 측정을 갖춰 들어갔고 재임포트도 실행됐으나, **승격 후 대조 계측 영수증이 이 폴더에 없다** — 주석에 기록된 42.6/53.6/55.6 대 75.0/84.6/86.0은 **수정 전** 값이고, 수정 후 같은 지표를 다시 잰 기록이 없다. 그리고 내가 직접 열어본 수정 후 스크린샷 2장에서 작업대는 여전히 평탄하게 읽힌다. 통과한 검사들은 이 속성을 측정하지 않으므로 "전수 통과"가 이 항목을 닫지 못한다.

[CARRIED] **#2 해소 경로** — 기존 도구 `M7ProjectBuilder.Capture`(`-nographics` 없이, hub 씬 미저장)로 작업대·벽 영역의 평균 채널값을 수정 전/후로 다시 재고, 빌더 주석의 두 측정값과 같은 지표로 대조한다. 그래도 평탄하면 근본 원인은 타일링이 아니라 **허브 씬에 반사 프로브가 없다**는 쪽이며, 그 수정은 빌더 주석이 이미 명시적으로 이월했다("The alternative fix — adding a reflection probe to the hub scene — is carried: that scene is rebuilt by T0ProjectBuilder.Prepare and is not this lane's").

## 9. 미측정과 경계

[CARRIED] 이 회차가 **측정하지 않은 것**. 아래를 측정했다고 적지 않으며, 승격이나 전수 통과를 근거로 승격시키지 않는다.

- **사람 플레이테스트 n=0.** 재미·몰입·난이도에 대한 판정 없음.
- **성능 미측정** — 프레임타임·메모리·로드 시간 계측 0회.
- **25분 세션 예산 · 8시간 완주** 미측정.
- **G4/G7 런타임 PASS 주장 없음.** 승격은 T0 stage-1 범위이며 `commercialReleaseEligible` 미변경이다.
- **실기기 마우스 휠 · IME · 컨트롤러** 검수 n=0.

[OBSERVED] **입력 경계.** 이번 네이티브 스모크도 직전 회차와 동일하게 `cliclick` **포인터 전용**이었고, 합성 키 이벤트는 Unity Input System에 닿지 않는다(`native-playtest-m9/verification.md:24`, `decision-log.md:508`). 따라서 키보드·휠의 **사람 조작 결과는 여전히 n=0**이다.

[OBSERVED] 다만 `RealMouseWheelReachesTheListThroughTheInputModule`이 PlayMode에서 **통과**했다. 이 검사는 `InputSystemUIInputModule`의 `scrollWheel` 바인딩 존재와 **실제 `Mouse` 디바이스 상태 이벤트**가 리스트를 움직이는 것까지 단정한다. 그러므로 **"휠은 입력 모듈 경로까지 코드 검증됨"**이라고는 쓸 수 있다 — 그리고 그 이상은 쓸 수 없다. 사람이 실물 마우스 휠을 돌린 결과는 측정되지 않았다(`remaining-m13/notes.md:68`의 과대 주장 금지 경계를 그대로 승계한다).

[CARRIED] 이 회차에서 **이월되는 것**.

1. **콘셉 흐름 대조 공백 2건** — 영상 챕터 `02 시간 눈금`에 대응하는 승격 후 화면 증거 없음. `04 자료 교체`는 헤더까지만 확인되고 교체 후 재판독 화면 없음. 다음 네이티브 회차에서 `t0-b3`까지 진행해 촬영.
2. **작업대 타공 가독성(§8 #2)** — 수정 후 대조 계측 미실시. 평탄하면 허브 씬 반사 프로브 추가로 이어지며, 그 씬은 `T0ProjectBuilder.Prepare` 소유다.
3. **빌드 크기 퍼지 단독 기여분(§6)** — 승격 유지·퍼지만 되돌린 대조 빌드 1회로 분리.
4. **`URPProjectSettings.asset`의 귀속(§7)** — 의도적 추가인지 URP 재질 업그레이드 부산물인지 미확인. 커밋 전에 판별 필요.
5. `remaining-m13/notes.md:83`이 넘긴 인테이크 — t0-b1 objective 무스포일러 재작성 · 증거함 보존 단서 문자열 전용 단정 · "자동 사본"/"보존 단서" 용어 통일.
6. **네이티브 스모크 `Player.log` 미아카이브(§3)** — "예외 0건"이 재검증 가능한 영수증을 갖도록 다음 회차부터 플레이어 로그를 레인에 복사하고 sha를 남긴다.

## 실행 경계 (이 문서 작성 기준)

[OBSERVED] 본 영수증 작성 과정에서 **Unity 실행 0회 · git 커밋/푸시 0회 · 코드 수정 0회**다. 생성한 파일은 이 폴더의 `verification.md`와 `verification.json` 2개뿐이며, `production/*`·`qa/*`·`unity/**`·다른 tech-verification 폴더는 **읽기만** 했다.
