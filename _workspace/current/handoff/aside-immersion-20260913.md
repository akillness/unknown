---
updated: 2026-09-13
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-production-director
---

# OMP 인수: 리서치·오프닝·GTI 후보 업데이트

## Done and ownership

[OBSERVED] OMP 세션 `01a09984-ff92-7000-b047-a64aca285590`, 저장소 `/Users/jangyoung/orca/unknown`를 확인했다. OMP는 M14~M22 및 Blender 인물/양손 모션을 진행 중이다. 이 문서는 파일 기반 인수이며 OMP의 읽음/ACK를 뜻하지 않는다.

Aside의 실제 수정은 아래 3개이며 각각 전후 해시가 `systems/tech-verification/aside-immersion-20260913/implementation-receipt.json`에 있다.

1. `unity/Unknown/Assets/_Project/Resources/M5Direction.asset`: 오프닝 4문구만.
2. `unity/Unknown/Assets/_Project/Presentation/M5DirectionProfile.cs`: 같은 4개 필드 기본값.
3. `unity/Unknown/Assets/_Project/Resources/T0Strings.json`: `caseObjective.ko` 단일 값. 166개 키와 다른 모든 항목 보존.

추가한 네이티브 테스트: `Tests/EditMode/AsideImmersionContractTests.cs`와 `.meta`, 실제 C# `CaseObjective` 및 Unity `SerializedObject`를 사용하는 4케이스. 아직 컴파일/실행 영수증 없음.

- 첫 컷: **마지막 당직 / 폐국 전날 밤, 마지막 당직을 맡았다.**
- 둘째 컷: **목록과 서랍 / 목록과 서랍을 대조해, 무엇을 남길지 정한다.**
- 안전 fallback: **현재 기록을 대조하고 다음 근거를 확인**
- 기존 이미지·재질 GUID, runtimeApproved, 두 컷 3.125/2.875초, intro/motto, 세이브·시뮬레이션·힌트·크랭크는 Aside가 변경하지 않았다.

## Research applied, not blindly copied

정본: `presentation/aside-immersion-20260913/decision-and-implementation.md` 및 연구 3편. Frictional의 상황 기반 동기, Mobius의 질문-관찰 앵커, Obra Dinn/Golden Idol의 판정 피드백 트레이드오프, Valve의 시각 노이즈 억제, XAG 대비·동작 선택권을 비교했다.

밸런스 허용 오차·플레이 시간은 임의 조정하지 않았다. `nearMissLimitMinutes=8` 제안은 읽는 런타임 경로가 없으므로 보류했다. 조위정합을 구현할 때 원천 스키마와 emitter, 소비 코드 및 경계 테스트를 함께 다뤄야 한다. OMP의 힌트 cadence/성공 판독 연출과 중복 구현하지 않는다.

## Resource update

- `assets/generated/2d/texture/aside-immersion-20260913/`: `nav-quiet-field-candidate.png` 1024×1536, 정적 저노이즈 에나멜 바탕 후보, 프롬프트·실제 revisedPrompt·해시·provider receipt 보존.
- `assets/generated/2d/concept/aside-immersion-20260913/`: 오프닝 이미지 1672×941, **요청 구도/닫힌 장부/읽기 여백 불충족으로 수정 필요**. 성공 자산으로 소개하거나 런타임에 넣지 않는다.
- 양쪽 `runtimeEligible:false`, `commercialReleaseEligible:false`. 사용자의 기존 승인 리소스만 적용 선택을 유지한다. GTI 내부 생성 횟수/크레딧은 unknown이다.
- 새 패널 원본의 중앙 84% 픽셀에서 종이색 #E7E3D8 최저 대비 **7.76:1**, 약화색 #A4AAA5는 **4.20:1로 4.5 미달**. 종이색만 후보 조건을 통과했다. 약화색에는 별도 검증한 불투명 배킹이 필요하다. 이는 실제 UI/합성/축소/색관리 검증이 아니다.

## Verification

- [OBSERVED] source/data contract: RED 7pass/10fail → GREEN **17/17**.
- [OBSERVED] campaign validator **50/50**. 기존 campaign은 무변경.
- [OBSERVED] 독립 QA: 소스 범위·해시·스포일러 상한·기존 승인 아트 불변 확인. 리뷰 전문 `systems/tech-verification/aside-immersion-20260913/independent-review.md`.
- [blocked] Unity 6000.5.6f1은 Aside 실행 환경에서 **No valid Unity Editor license found**, exit198. 테스트 전에 종료했고 XML0, PlayMode 미실행. OMP의 과거 테스트 결과로 이 변경을 검증했다고 하지 않는다.

정적 재검증:

```bash
node _workspace/current/systems/tech-verification/aside-immersion-20260913/verify-immersion.mjs
node _workspace/current/planning/validate-campaign.mjs
```

유효한 라이선스가 있는 OMP 실행 환경에서, 다른 Unity 실행이 없을 때만:

```bash
U=/Applications/Unity/Hub/Editor/6000.5.6f1/Unity.app/Contents/MacOS/Unity
E="$PWD/_workspace/current/systems/tech-verification/aside-immersion-20260913"
"$U" -batchmode -nographics -burst-disable-compilation -projectPath "$PWD/unity/Unknown" -runTests -testPlatform EditMode -testFilter Tide.Tests.AsideImmersionContractTests -testResults "$E/omp-editmode.xml" -logFile "$E/omp-editmode.log"
"$U" -batchmode -nographics -burst-disable-compilation -projectPath "$PWD/unity/Unknown" -runTests -testPlatform PlayMode -testFilter 'Tide.Tests.M5DirectionPlayModeTests;Tide.Tests.T0IntroReplayReducedMotionTests' -testResults "$E/omp-playmode.xml" -logFile "$E/omp-playmode.log"
```

최신 소스 해시와 결과 생성 시각/컴파일 입력을 대조한다. 인트로 150% 글자·모션축소·건너뛰기/재보기/복귀 및 새 슬롯/기존 슬롯 불변을 실제 창에서도 확인해야 한다.

## QA follow-through

- AIM-01: `T0CaseThreadTests.cs`의 default branch에 옛 fallback literal이 남는다. 현재 도달 불가라 실회귀는 아니지만, OMP 소유 테스트 정리 때 런타임 리소스 또는 새 값에 맞출 것.
- AIM-02: `planning/objective-copy-m13.md` 및 테스트 주석의 “초반 fallback이 실제 발동한다”는 예전 설명은 현재 데이터와 다르다. 현재 3비트 모두 정상 objective를 사용한다. 관련 소유자가 최신 설명으로 정정할 것.
- AIM-03: 새 테스트 `.meta` 추가로 해소.
- AIM-04: C#/JS 금지어 10종을 정렬했고 content guard와 실제 분기 검증의 경계를 주석으로 명시해 해소. Native 미실행 경계는 유지.

## Completion boundary

Aside는 커밋/푸시/워크트리 삭제·r02 승격·새 GTI 기본 적용을 실행하지 않았다. 원본 영상/과거 영수증도 재작성하지 않았다. 새로운 사람 몰입도·재미·플레이시간 측정 없음. 라이선스가 풀린 네이티브 검증과 새 후보 사용 범위 결정만 별도 게이트다.
