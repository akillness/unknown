---
updated: 2026-09-12
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-production-director
---

# M13 레거시 제거 — 참조 전수 감사와 삭제 판정 (RFC-CX-016)

[OBSERVED] 범위는 `unity/Unknown/Assets/_Project/Art/Candidates/` 안의 **Unity 사본**과 그 사본을 참조하던 **씬 노드·에디터 배선**이다. RFC-CX-016 의 `[DECISION·레거시 제거]` 불릿(`production/decision-log.md` — 그 파일은 Main 이 계속 append 중이라 줄 번호 대신 RFC id + 불릿 라벨로 가리킨다)이 지명한 4건을 대상으로 삼고, 지우기 **전에** 참조를 전수 조사했다. `assets/generated/` 원본 계보는 한 파일도 건드리지 않는다(§6 입증). 기계 판독본은 [purge-receipt.json](purge-receipt.json).

[OBSERVED] **4건 전부 제거 완료**다. 다만 경위가 두 단계였고 그 판단 이력을 남긴다: 1차 감사에서 `hub-greybox.fbx` 는 **산 참조가 있어 보류**했고, 그 보고를 받은 Main 이 hub 씬 편집권을 부여하며 이 회차 완료를 지시해 **씬 수술 후 제거**했다(§3.4).

이 문서는 **삭제 판정과 실행의 근거**이며 Unity 재검증 영수증이 아니다. 엔진 검증은 Main 이 일괄 수행한다(§7).

## 1. 감사 방법

[OBSERVED] 저장소 루트 `/Users/jangyoung/orca/unknown`, Unity `6000.5.6f1`, URP `17.5.0`. `CAND=unity/Unknown/Assets/_Project/Art/Candidates`.

Unity 자산의 참조는 **경로 문자열**과 **GUID** 두 경로로 생긴다. 이름만 grep 하면 씬·프리팹·ScriptableObject 의 GUID 참조를 놓치므로 둘 다 조사했다.

```
# (a) 대상 GUID 추출 (.meta 첫 guid)
awk '/^guid:/{print $2; exit}' $CAND/<target>.meta

# (b) GUID 역참조 — Library(재생성 캐시)와 대상 자신을 제외
grep -rEl '<guid1>|<guid2>|…' unity/Unknown \
  | grep -v '^./Library' | grep -v '<대상 폴더>'

# (c) 이름 참조 — 코드·씬·데이터·워크스페이스·문서 전역
grep -rE 'c1-signature-reader-r01|drawer-r01|hub-greybox|stamp-confirm' \
  unity/Unknown _workspace scripts docs

# (d) 경로 배선 참조 — 에디터·런타임·테스트
grep -rn 'Art/Candidates' unity/Unknown/Assets/_Project/{Editor,Tests,App,Presentation,UI}

# (e) 재생·소비 경로 확인
grep -rE 't0-import-audit|Imported candidates|AudioClip|AudioSource' \
  unity/Unknown/Assets/_Project/{Tests,App,UI,Presentation,Sim,Data}

# (f) 원본 보존 동일성 (사본 vs assets/generated 원본)
shasum -a 256 <unity 사본> <assets/generated 원본>

# (g) git 추적 상태
git ls-files <대상>
```

[OBSERVED] 대상 GUID:

| 대상 | GUID |
|---|---|
| `c1-signature-reader-r01/` (폴더) | `c04f4ee61e71748f48dee06376074fd6` |
| `drawer-r01.fbx` | `1f1becb186a7e40da9bf88441f3fba66` |
| `hub-greybox.fbx` | `6b61a6ef877b6496cb6fdc8ca2e6a486` |
| `stamp-confirm.wav` | `eae9e501ce8814fddbbc9473b412ca78` |

[OBSERVED] `c1-signature-reader-r01/` 은 폴더이므로 **안쪽 14개 자산 GUID 를 각각** 역참조했다(폴더 GUID 만 조사하면 안쪽 프리팹·머티리얼 참조를 놓친다): `0373c127588b64dfe8627a91ee4d5488` `15519da7ad4834e7091e85db0ca9818a` `35f91eb6be57c4066a9c4680a66a0aa3` `a565b79598d8a4542b0af83be340dcec` `e55ef588a799f46b184264c5063037cb` `8c40b630d344044449d5ad9e62350f7c` `799d38599b39b47b98cb320df2409f70` `b943ee49a3d8640eb82673ac34c9f840` `7efc9d32cafc54082bee4c275726670b` `2f4e722fecb1e4370bdb493e22d3e363` `140e92dafcf224a80a46dc453ebbbfc7` `b3c2b88d1245a49709d898428ce1e954` `cfe51f13322e8477f9fa3ec908b96b7f` `23ca1086c02c74cfab66f7dccac4a558`.

## 2. 판정 요약

| 대상 | 1차 감사의 산 참조 | 대체 자산 | 최종 판정 |
|---|---|---|---|
| `c1-signature-reader-r01/` (27파일 + 폴더 meta) | **없음** | `c1-signature-reader-r02/` (`Resources/C1SignatureView.asset` 이 직접 가리킨다) | **삭제** |
| `drawer-r01.fbx` (+meta) | **없음** | `drawer-r03/DrawerDiagnostic.prefab` (hub 씬이 참조) | **삭제** |
| `stamp-confirm.wav` (+meta) | **없음** | 없음 — 재생 경로 자체가 없다 | **삭제** |
| `hub-greybox.fbx` (+meta) | **있음** — `Scenes/hub.unity` 의 프리팹 인스턴스 | M7 당직실 셸(`m7-hub-r01`) | **삭제** (씬 노드 선행 제거 후, §3.4) |

[OBSERVED] 총 **34파일 / 20,845,750 B** 삭제. `Art/Candidates/` 에 남은 것은 전부 산 후보 8개다: `c1-patrol-panel-r01` `c1-signature-reader-r02` `drawer-r03` `m5-direction` `m7-hub-r01` `m7-reader-r01` `m7-ui-r01` `m8-review-card`.

## 3. 대상별 상세

### 3.1 `c1-signature-reader-r01/` — 삭제

[OBSERVED] 안쪽 14개 GUID 전부를 `unity/Unknown` 전역(Library 제외, 자기 폴더 제외)에서 역참조 → **0건**:

```
grep -rEl '<14개 guid>' unity/Unknown \
  | grep -v '^./Library' | grep -v 'Art/Candidates/c1-signature-reader-r01/'
→ (출력 없음)
```

[OBSERVED] 경로 배선도 없다. `grep -rn 'Art/Candidates' .../Editor` 결과 `Editor/C1SignatureProjectBuilder.cs:17` 의 `Destination` 은 **`c1-signature-reader-r02/`** 이고, r01 을 가리키는 상수·리터럴은 어느 빌더에도 없다.

[OBSERVED] 대체 관계가 런타임 설정에서 확인된다 — `Resources/C1SignatureView.asset:15` 의 `reader` 는 `guid: c92f1b0b223404c0cb07bf1bf3869126`(= `c1-signature-reader-r02/SignatureReader.prefab`), `:16` 의 `paper` 는 `guid: 6362ff236cd624632b2ace3c50c2e802`(= r02 의 `SignaturePaper.png`)이며 `:21` 이 `runtimeApproved: 1` 이다. **산 것은 r02 하나뿐**이다.

[OBSERVED] r01 과 r02 의 Unity 파일 집합은 이름 기준 **완전히 동일**하다(`diff <(find r01) <(find r02)` → 차이 0). 즉 r01 은 이름·구조가 겹치는 **죽은 사본**이다.

[OBSERVED] 이름 참조는 문서 3곳뿐이며 전부 **역사 기술**이다: `concept/c1-signature-art-brief.md:49`(자산 id 제안), `production/c1-signature-operator-review.md:11-13`(r01 생성 영수증), `messages/005-systems-m5-integration-review.md:29`("main의 `c1-signature-reader-r01/*` 미추적 자산은 M4 잔여물"). 문서는 자산을 로드하지 않으므로 삭제로 깨지지 않고, RFC-CX-016 의 「provenance와 아카이브 기록은 보존」 지시에 따라 **문서는 손대지 않는다**.

[OBSERVED] 원본 보존: `assets/generated/3d/c1-signature-reader-r01/` 이 그대로 있다(최상위 9파일 + `textures/` 4장 = 13파일). `Reader.fbx` ↔ `SM_C1_Signature_Reader.fbx`, `SignaturePaper.png` ↔ `assets/generated/2d/texture/c1-signature-paper-r01/paper.png`, 텍스처 4장이 각각 sha256 **일치**(§6).

[OBSERVED] 경계 하나는 명시해 둔다. r01 의 `.mat` 4개(`Casting`·`Patina`·`Recess`·`Salt`) · `*_MetallicSmoothness.asset` 2개 · `SignatureReader.prefab` 1개는 **에디터가 만든 파생물이라 `assets/generated/` 에 원본이 없다**. 복원 경로는 `Editor/C1SignatureProjectBuilder.cs` 의 `Destination` 을 r01 로 바꿔 재실행하는 것이고, 입력(FBX·텍스처·glTF metallic/roughness 계수)은 전부 보존돼 있다. 재임포트 시 **GUID 는 새로 발급된다** — 영수증에 삭제 시점 GUID 를 남긴 이유다.

### 3.2 `drawer-r01.fbx` — 삭제

[OBSERVED] GUID `1f1becb186a7e40da9bf88441f3fba66` 역참조 → **자기 `.meta` 1건뿐**. 씬·프리팹·`Resources/*.asset` 어디에도 없다.

[OBSERVED] 유일한 참조는 에디터 배선 2곳이었다: `Editor/T0ProjectBuilder.cs` 의 `hub-view-drawer-r01` 탐색 + 복사(당시 L30-31), 그리고 임포트 감사 루프의 배열 항목(당시 L34).

[OBSERVED] 대체 자산은 r03 이며 **실제로 씬에 들어가 있다**: `Editor/T0ProjectBuilder.cs` 의 `MakeApprovedWorkbench` 가 `Art/Candidates/drawer-r03/DrawerDiagnostic.prefab` 을 로드하고, 그 GUID `cc0dedc970e924231afddddfd6a665f5` 는 `Scenes/hub.unity` 에서 참조된다(`T0 approved r03 drawer`). RFC-CX-003 addendum 의 T0 승인 대상도 r03 이다(`modeling/asset-manifest.md:215`).

[OBSERVED] 원본 보존: `assets/generated/3d/hub-view-drawer-r01/SM_Hub_Workbench_Drawer.fbx` 와 sha256 **일치**. r01/r02/r03 소스 3세대 전부 보존돼 있다.

[DECISION·배선] 복사 줄은 **제거했다**(§4). 존재 가드로 두면 안 되는 이유: 이미 `Directory.Exists`/`File.Exists` 가드가 걸려 있었지만 **가드가 보는 것은 `assets/generated/` 원본**이고 그 원본은 살아 있다. 배선을 남기면 다음 `Prepare` 실행이 삭제한 파일을 **그대로 되살린다**. 즉 가드는 이 퍼지를 무효화한다.

### 3.3 `stamp-confirm.wav` — 삭제

[OBSERVED] GUID `eae9e501ce8814fddbbc9473b412ca78` 역참조 → **자기 `.meta` 1건뿐**.

[OBSERVED] 재생 경로가 존재하지 않는다. `grep -rE 'AudioClip|AudioSource' .../{Tests,App,UI,Presentation,Sim,Data}` → **0건**. 프로젝트 전체에서 오디오 관련 코드는 hub 카메라에 붙는 `AudioListener` 하나뿐이며, **듣는 쪽만 있고 내는 쪽이 없다**. 기존 기록도 같은 사실을 적었다 — `systems/tech-verification/t0-resource-integration-20260910.md:75` "provenance 는 `runtimeEligible:false`; 청취 승인 대기이며 재생 비활성".

[OBSERVED] 유일한 참조는 `Editor/T0ProjectBuilder.cs` 의 복사 한 줄(당시 L32)이었다. 대체 자산 없음 — 사운드는 이 회차에서도 제작하지 않았고 재생 계약도 없다(대체가 아니라 **미사용 반입물**의 정리다).

[OBSERVED] 원본 보존: `assets/generated/audio/higgsfield-stamp-r01/stamp-confirm.wav` 와 sha256 **일치**. Higgsfield 크레딧으로 생성된 자산이므로 원본 보존이 특히 중요하다(재생성은 유료).

### 3.4 `hub-greybox.fbx` — 1차 보류 → 씬 수술 후 삭제

#### 3.4.1 1차 감사에서 찾은 산 참조

[OBSERVED] GUID `6b61a6ef877b6496cb6fdc8ca2e6a486` 가 `unity/Unknown/Assets/_Project/Scenes/hub.unity` 에서 프리팹 인스턴스로 쓰이고 있었다(줄 번호는 **삭제 전** 기준):

| 앵커 | 내용 |
|---|---|
| `hub.unity:403` | `--- !u!1001 &307389563` PrefabInstance 시작 |
| `hub.unity:409` | `m_TransformParent: {fileID: 410818390}` — 후보 루트에 붙어 있었다 |
| `hub.unity:411-454` | 같은 GUID 를 target 으로 하는 `m_Modifications` **11개**(위치·회전 10개 + `m_Name: hub-greybox`) |
| `hub.unity:459` | `m_SourcePrefab: {fileID: 100100000, guid: 6b61a6ef…, type: 3}` |
| `hub.unity:460-464` | stripped `Transform &307389564`, `m_CorrespondingSourceObject` 가 같은 GUID, `m_PrefabInstance: 307389563` |
| `hub.unity:465-480` | 부모 `GameObject &410818389` = `"Imported candidates — pending review"`, `m_Component: [410818390]`, `m_IsActive: 0` |
| `hub.unity:481-496` | `Transform &410818390`, `m_GameObject: 410818389`, `m_Children: [307389564]`, `m_Father: {fileID: 0}`(= 씬 루트) |
| `hub.unity:1364` | `SceneRoots.m_Roots` 에 `- {fileID: 410818390}` 등재 |
| `Editor/T0ProjectBuilder.cs:46-49` | 이 노드를 만드는 코드(`InstantiatePrefab` → 부모에 붙이고 `SetActive(false)`) |

[OBSERVED] 노드는 **비활성**이라 화면에 보이지 않았다. 그러나 프리팹 인스턴스는 씬에 직렬화돼 있어 **빌드에 포함**된다. FBX 만 지우면 hub 씬은 소스 프리팹을 잃은 인스턴스를 갖게 된다.

[DECISION·1차] 감사 규칙(「산 참조가 하나라도 있으면 지우지 않는다」)대로 **보류**하고, 해제에 필요한 절차와 fileID 를 적어 Main 에게 보고했다. 보류 사유는 RFC 판정을 뒤집는 것이 아니라 **선행 조건(씬 노드 제거)이 1차 배치의 소유 경계 밖**이라는 사실 기록이었다.

#### 3.4.2 Main 의 해제 지시와 권한 부여

[OBSERVED] Main 판정: 사용자가 "이전 레거시들은 제거할꺼야"라고 명시했고 이것은 M7 당직실 셸이 대체한 **가장 명백한 초기 블록아웃**이며, 비활성 노드라도 씬에 있으면 빌드에 실린다. 따라서 이 회차에 완료한다. `Scenes/hub.unity` 편집권을 이 배치에 부여했고, `Prepare` 통째 재실행은 금지했다 — `WireBeats` 위의 주석이 그 이유를 이미 적어 뒀다("full Prepare would rebuild scenes owned by later milestone builders", 편집 전 `T0ProjectBuilder.cs:58` · 편집 후 `:47`).

#### 3.4.3 부모 노드 처리 판단

[DECISION] **부모까지 제거**했다. 근거 둘:

1. `Transform &410818390` 의 `m_Children` 가 `[307389564]` **단일 항목**이었다(삭제 전 `hub.unity:493-494`) — 담고 있던 후보가 hub-greybox 하나뿐이다.
2. `grep -n 'm_Father: {fileID: 410818390}' hub.unity` → **0건**. 그 밑에 parented 된 다른 객체가 없다.

즉 인스턴스를 빼면 부모는 **빈 루트**가 된다. `m_Children` 만 `[]` 로 비우고 부모를 남기는 대안은 출하 씬에 의미 없는 노드를 남기므로 택하지 않았다.

#### 3.4.4 실제 제거 범위

[OBSERVED] 네 객체가 파일에서 **연속 구간**이었으므로 한 범위로 제거했다.

| 제거한 원본 줄 | 줄 수 | 내용 |
|---|---:|---|
| `403-496` | 94 | PrefabInstance `&307389563`(+`m_Modifications` 11개) · stripped Transform `&307389564` · GameObject `&410818389` · Transform `&410818390` |
| `1364` | 1 | `SceneRoots.m_Roots` 의 `- {fileID: 410818390}` |
| **합계** | **95** | |

[OBSERVED] 제거 전 사전 스윕으로 네 fileID 의 **모든** 등장 위치를 열거했고, `1364` 를 뺀 전부가 `403-496` 안에 있음을 확인한 뒤 범위를 정했다 — 즉 이 두 범위가 참조 집합을 **빠짐없이** 덮는다.

#### 3.4.5 YAML 앵커 정합성 입증

[OBSERVED] 앵커(`--- !u!T &ID`)와 내부 참조(`{fileID: N}` 중 같은 줄에 `guid:` 가 **없는** 것 = 씬 내부 참조, `0` = null)를 파싱해 대조했다. 파서 오탐을 배제하려고 **HEAD 기준선에도 같은 파서를 돌렸다**.

| 항목 | HEAD 기준선 | 제거 후 |
|---|---:|---:|
| 객체 수 | 57 | **53** |
| `m_Roots` 항목 수 | 12 | **11** |
| 끊긴 내부 참조 | 0 | **0** |

[OBSERVED] 사라진 앵커는 정확히 `307389563` `307389564` `410818389` `410818390` **4개**이고 **추가된 앵커는 0개**다. 씬 내 GUID `6b61a6ef…` 등장 **0회**, `"Imported candidates"` 문자열 **0회**, 파일 끝 개행 **보존**.

| 파일 상태 | sha256 | 바이트 | 줄 |
|---|---|---:|---:|
| 삭제 전 | `3c8129416d75b1756ef668a10d32aabccbe9f054aeab4a1b070c9ca2b7e23346` | 38,956 | 1,373 |
| 삭제 후 | `5ae9824392ce89fc02c5dc7d6a8046884fc066f4aef16d3bac91cb5e5bbe37e0` | 35,423 | 1,278 |

[OBSERVED] 1,373 − 95 = 1,278 — 제거 줄 수와 결과 줄 수가 일치한다.

[OBSERVED] 참조가 실재했음을 못박아 둔다: `git show HEAD:…/Scenes/hub.unity | grep -c 6b61a6ef877b6496cb6fdc8ca2e6a486` = **13**, 워킹트리 동일 grep = **0**. 병행 세션이 "커밋된 씬에 참조가 없다"고 보고했으나 그것은 **내 씬 편집 이후의 워킹트리**를 본 것이다. 커밋본에는 13곳이 있었고 씬 제거는 **필수였다**.

[OBSERVED] 참고 — `zones.json:14`, `Data/Authoring/Zones/hub.asset:25`, `handoff/README.md:98` 등이 말하는 `hub-greybox.glb` 는 **다른 파일**이다(`assets/generated/3d/` 의 GLB 정본, 좌표 변환 대조용 OPEN-S8). 이번 대상은 Unity 사본 `hub-greybox.fbx` 하나이며, 두 파일을 혼동해 데이터·문서를 고치는 일은 하지 않았다.

## 4. 배선 정리 (`Editor/T0ProjectBuilder.cs`)

[OBSERVED] 소유 정정 이력: 이 배치의 배정에는 이 파일이 내 소유로 적혀 있었고, Main 이 이후 UiUxPromote 소유라고 정정했다. 1차 편집 후 irc 로 통보했고 UiUxPromote 가 **"추가로 손댈 것 없다 · 이 회차에 더 수정하지 않는다"**고 확정 회신했다(그쪽이 쓰는 것은 `BuildMacAt` 뿐이며 무변경).

### 4.1 1차 — 삭제 3건의 되살아남 차단

| 변경 전 | 변경 후 | 사유 |
|---|---|---|
| `hub-view-drawer-r01` 탐색 + `drawer-r01.fbx` 복사(L30-31) | **제거** | 남기면 다음 `Prepare` 가 삭제 파일을 되살린다(§3.2) |
| `stamp-confirm.wav` 복사(L32) | **제거** | 동일(§3.3) |
| 감사 배열 `{"hub-greybox.fbx","drawer-r01.fbx"}`(L34) | `{"hub-greybox.fbx"}` | `drawer-r01.fbx` 는 `asset==null` 로 조용히 건너뛰는 죽은 항목이 된다 |

### 4.2 2차 — hub-greybox 해제에 따른 정리

| 변경 전 | 변경 후 | 사유 |
|---|---|---|
| `var repo=…` + `CopyCandidate(repo,"assets/generated/3d/hub-greybox.fbx",…)`(L28-29) | **제거** | 사본을 지웠으므로 복사도 없앤다. `repo` 는 다른 사용처가 없었다 |
| 감사 일체: `JObject audit` · FBX 측정 루프 · `Builds/t0-import-audit.json` 기록 · `T0_IMPORT_AUDIT` 로그(L33-40) | **제거**, 자리에 `AssetDatabase.Refresh();` | FBX 후보가 **0개**가 됐다. C# 은 빈 배열 리터럴(`new[]{}`)을 컴파일하지 못하고, `new string[0]` 로 두면 `{}` 만 쓰는 죽은 루프가 남는다. 없는 후보를 감사한다고 주장하지 않는 쪽을 택했다 |
| 후보 루트 블록: `"Imported candidates — pending review"` 생성 + `InstantiatePrefab` + `SetActive(false)`(L46-49) | **제거** | §3.4 씬 노드를 만드는 코드. 남기면 다음 `Prepare` 가 노드를 다시 넣는다 |
| `CopyCandidate` 헬퍼(L68) | **제거** | 호출자가 0이 됐다 |
| `using Newtonsoft.Json.Linq;`(L5) | **제거** | `JObject`/`JArray` 사용처가 감사 블록뿐이었다. `Newtonsoft.Json.Formatting` 은 완전한정명이었으므로 이 using 에 의존하지 않았다 |

[OBSERVED] using 정합성 확인: 제거 후 `File.`·`Path.` 사용 0이지만 `Directory.` 1건이 남아 `System.IO` 는 계속 필요하다. `System.Linq`(`Select`·`Where`·`Skip`·`ToArray`) · `System`(`InvalidOperationException`) · `UnityEditor.Build.Reporting`(`BuildResult`) 도 계속 쓰인다 → **미사용 using 0**.

[OBSERVED] 부작용 — `Prepare` 는 이제 `Builds/t0-import-audit.json` 을 쓰지 않고 `T0_IMPORT_AUDIT` 를 로깅하지 않는다. 그 토큰·파일을 읽는 테스트·스크립트·문서는 **없다**(등장처는 `_workspace/current/systems/tech-verification/t0-m2/logs/` 의 보관된 M2 실행 로그뿐). `Builds/` 아래에 쓰는 다른 빌더는 **모두 스스로** `Directory.CreateDirectory("Builds")` 를 호출하므로 `Prepare` 의 호출을 뺀 것이 다른 레인을 깨지 않는다. 감사 장치가 다시 필요해지면 git 에서 세 블록을 복원하면 된다.

## 5. 실행 모델 — 무엇이 언제 실효되는가

[OBSERVED] 이 회차에 **`Prepare` 를 실행하지 않았다**. UiUxPromote 가 Unity 락을 들고 있고, 전체 `Prepare` 는 `boot`/`ui-root`/`hub` 를 재생성해 후속 마일스톤이 소유한 커밋 씬을 덮기 때문이다. 그래서 층위별로 실효 시점이 다르다.

| 층위 | 수행 방법 | 실효 |
|---|---|---|
| Unity 사본(`Art/Candidates/`) | 임포터 재실행이 아니라 **직접 파일 삭제** | **즉시** |
| hub 씬 노드 | YAML 정밀 제거(4객체 + `SceneRoots` 등재) | **즉시** |
| `T0ProjectBuilder` 코드 경로 | 복사·감사·인스턴스화 블록 제거 | **다음 `Prepare` 때만.** 이번 회차의 관측 변화가 아니라, 다음 `Prepare` 가 삭제 사본을 되살리고 후보 노드를 다시 넣는 것을 막는 **지속성 보증**이다 |

## 6. 보존 입증 — `assets/generated/` 무삭제

[OBSERVED] 삭제 직전 `find assets/generated -type f | wc -l` = **558**. 전부 삭제한 직후 같은 명령 = **558**. 차이 0. `git status --porcelain assets` 의 `D`(삭제) 항목 = **0건**(출력은 `?? assets/generated/video/gameplay-m9/` 한 줄뿐이며, 병행 RFC-CX-015 영상 레인이 추가한 미추적 디렉터리다 — 삭제가 아니다). 추적 파일 수 `git ls-files assets | wc -l` = **449** 불변.

[OBSERVED] 삭제한 34파일의 분류: 바이너리 입력 **9**개는 `assets/generated/` 원본과 sha256 **전부 일치**(영수증 `originBytesIdentical: true` ×9), `.meta` 사이드카 **18**개(재임포트 시 재생성되며 GUID 는 새로 발급), 에디터 파생물 **7**개(`.mat`×4 · `*_MetallicSmoothness.asset`×2 · `.prefab`×1 — 복원 경로는 §3.1). 9+18+7 = 34.

| 삭제 사본 | 보존 원본 |
|---|---|
| `c1-signature-reader-r01/Reader.fbx` | `assets/generated/3d/c1-signature-reader-r01/SM_C1_Signature_Reader.fbx` |
| `c1-signature-reader-r01/SignaturePaper.png` | `assets/generated/2d/texture/c1-signature-paper-r01/paper.png` |
| `c1-signature-reader-r01/textures/*.png` ×4 | `assets/generated/3d/c1-signature-reader-r01/textures/*.png` ×4 |
| `drawer-r01.fbx` | `assets/generated/3d/hub-view-drawer-r01/SM_Hub_Workbench_Drawer.fbx` |
| `stamp-confirm.wav` | `assets/generated/audio/higgsfield-stamp-r01/stamp-confirm.wav` |
| `hub-greybox.fbx` | `assets/generated/3d/hub-greybox.fbx` |

[OBSERVED] 삭제는 `git rm` 이 아니라 파일 삭제로 했다. 스테이징·커밋은 Main 이 일괄 수행한다. 대상 34파일은 전부 git 추적 상태였으므로 **git 이력으로도 복원 가능**하다 — 원본 보존과 별도의 두 번째 복원 경로다.

## 6.1 이 배치가 실제로 검증한 것

[OBSERVED] Unity 는 띄우지 않았다(락은 UiUxPromote 가 보유). 엔진 밖에서 확인 가능한 것만 확인했고, 결과는 아래가 전부다.

| 검사 | 명령 | 결과 |
|---|---|---|
| 대상 소멸 | 34개 경로 각각 `test -e` | 34/34 부재. `c1-signature-reader-r01/` 과 그 `textures/` 디렉터리도 제거 |
| 잔존 후보 | `ls Art/Candidates` | 산 후보 8개만 남음(§2) |
| 끊긴 참조 0건 | `unity/Unknown/{Assets,ProjectSettings,Packages}` **516파일**을 파싱해 삭제 GUID 7개 + 제거 fileID 4개 = **needle 11개** 조회 | **전부 0건** |
| 씬 앵커 정합성 | 앵커·내부참조 파서(HEAD 기준선 동시 실행) | 57객체/12루트/dangling 0 → **53객체/11루트/dangling 0**. 제거 앵커 정확히 4개, 추가 0개, 끝 개행 보존 |
| 커밋본 대조 | `git show HEAD:…/hub.unity \| grep -c <guid>` vs 워킹트리 | **HEAD 13 / 워킹트리 0** — 산 참조의 실재와 제거를 동시에 입증 |
| git 삭제 수 | `git status --porcelain …/Art/Candidates \| grep -c '^ D'` | **34** |
| 배선 C# 구문 (1차) | `mcs -target:library -d:UNITY_EDITOR T0ProjectBuilder.cs` (`git show HEAD:` 판에도 동일 실행) | 오류 프로파일이 HEAD 기준선과 **동일** — `CS0246`×11 + `CS0234`×2, **경고 0 · CS1xxx 0** |
| 배선 C# 구문 (2차) | 같은 명령 | `CS0246`×**10** + `CS0234`×2, **경고 0 · CS1xxx 0**. 1차보다 `CS0246` 이 정확히 1개 줄었고 이는 제거한 `Newtonsoft.Json.Linq` 와 일치한다 |
| 잔여 식별자 | `grep` for `drawerDir`·`drawer-r01`·`stamp-confirm`·`audio`·`JObject`·`JArray`·`CopyCandidate`·`Newtonsoft`·`Imported candidates` | 설명 주석 외 **0건**. 중괄호·괄호 균형 0 |

[OBSERVED] `mcs` 검사의 한계: `CS0246`/`CS0234` 는 Unity 참조 어셈블리가 없는 단독 컴파일에서 당연히 나는 **의미 오류**다. 이 검사가 입증하는 것은 **구문이 유효하고 편집이 새 오류 부류를 만들지 않았다**는 것이며, **Unity 컴파일 성공은 입증하지 않는다**.

[OBSERVED] 부수 관측 — `git status` 에 `?? Art/Candidates/m8-review-card.meta` 와 `M Editor/M7ProjectBuilder.cs`, `M Resources/M7*.asset`, `M Resources/M8ReviewNotes.asset` 이 함께 보인다. 전부 **다른 레인 소유**이며 이 배치가 건드리지 않았다. M7 4종 승급(`runtimeApproved: 1`)은 UiUxPromote 의 결과이고 이 배치의 산출이 아니다. `m8-review-card.meta` 스테이징은 Main 이 처리한다.

## 7. 이 문서가 입증하지 않는 것

[CARRIED] 아래는 이 배치가 **측정하지 않았다**. 측정한 것처럼 읽히지 않게 명시한다.

- **삭제 후 Unity 재검증.** 이 배치는 Unity 를 띄우지 않았다. EditMode/PlayMode/직렬화 부트/빌드는 Main 이 일괄 실행한다. 직전 베이스라인은 EditMode 53/53, PlayMode 73/73(+6 skipped), boot 1/1, 빌드 354,247,240B 다.
- **hub 씬이 에디터에서 정상 열린다는 것.** 앵커 정합성은 **파서로** 입증했고 Unity 로는 확인하지 않았다.
- **빌드 바이트.** 검사 가능한 예측으로 적는다 — **빌드가 354,247,240B 보다 줄어야 한다.** `hub-greybox.fbx` 는 hub 씬에서 도달 가능했으므로 노드가 비활성이어도 메시가 빌드에 실려 있었다. 나머지 3건은 어느 씬·`Resources` 에서도 도달 불가였으므로 기여분이 0이다. 즉 **감소분은 hub-greybox 몫**이어야 한다. 이것은 예측이며 측정값이 아니다.
- 사람 플레이테스트·재미·몰입·25분 예산·8시간 완주 — **여전히 미측정**.

## 8. 추가 레거시 후보 — 제안만 (이 배치에서 지우지 않음)

[TARGET] RFC-CX-016 이 지명하지 않은 항목은 **제안으로만** 남긴다. 각 항목에 판정 근거가 될 조사 결과를 붙였다.

| 후보 | 조사 결과 | 제안 |
|---|---|---|
| `Editor/M7ProjectBuilder.cs` 의 `Probe()` | 디버그 진단 전용 진입점. 테스트·파이프라인이 호출하지 않음 | M7 승급이 끝난 뒤 제거 검토. **승급 레인이 쓸 수 있어 손대지 않았다** |
| `Art/Candidates/m8-review-card.meta` | git 미추적. 짝인 `m8-review-card/CardPaper.png` 는 추적됨 | Main 이 스테이징에서 처리(회신으로 확인) |
| `assets/generated/3d/c1-patrol-panel-r01-attempt1/` | 실패 시도 아카이브 | **삭제 금지.** 원본 계보이며 CLAUDE.md §2("삭제는 없다")에 걸린다 |
| `assets/generated/3d/*.meta`, `renders.meta`, `scripts.meta` 등 | 저장소 루트를 Unity 로 열었을 때 생긴 임포트 사이드카. 루트 `.gitignore` 의 `/assets/**/*.meta` 로 이미 추적 제외 | 조치 불필요. 관측 기록만 |
| `unity/Unknown/Builds/` (1.7 GB) | `unity/Unknown/.gitignore:6` 의 `[Bb]uilds/` 로 추적 제외 확인 | 조치 불필요. Main 의 `git add` 가 삼킬 위험 없음을 확인한 기록 |

[OBSERVED] **레거시가 아님을 확인한 것**(오판 방지용): `c1-patrol-panel-r01/`(= `Resources/C1View.asset:15` 가 `guid: 8e19111b18d774732af1b8aa88b4b2a7` 로 직접 참조, 빌더 `Destination` 도 r01), `drawer-r03/`(hub 씬 참조), `c1-signature-reader-r02/`·`m5-direction/`·`m7-hub-r01/`·`m7-ui-r01/`·`m7-reader-r01/`·`m8-review-card/`(각 빌더·프로파일이 참조). **이름에 `r01` 이 붙었다고 레거시가 아니다** — 패트롤 패널은 r01 이 현행이다.
