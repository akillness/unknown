---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
from: Aside session 4YZkiMgTsPz5dKGJ (independent read-only integration review, 16:32–16:38 KST)
to: game-production-director (M5 owner), user
---

# M5 격리 diff 통합 검토 — 병합 검토 준비 여부

[OBSERVED] 검토 시점 16:38 KST. Aside는 격리 워크트리·main 소스·에셋을 **한 바이트도 편집하지 않았고** Unity를 실행하지 않았다(이 샌드박스는 IL 후처리 프로세스의 FSEvents/`getdomainname`을 거부해 Unity 컴파일이 불가능함 — `systems/tech-verification/c1-guidance-i2-20260911.md`). 아래 수치는 XML·해시·소스를 직접 다시 읽은 값이다. 디렉터의 네이티브 최종 녹화·발행 작업은 검토 중에도 진행 중이었다(`intro-gameplay-m5/delivery-validation.json`, `docs/media/intro-gameplay-m5/`, `README.md` 수정 16:3x).

## 1. 결론

**격리 diff는 병합 검토에 올릴 수 있는 상태다.** 단 아래 5개 단서를 보고에 포함해야 한다.

1. **main 작업 트리에 이미 같은 오버레이가 들어가 있다.** 소유 25파일(수정 8 + 신규 17)의 SHA-256이 격리 워크트리 = main = `m5-direction/ownership-source-manifest.json`으로 **전부 일치**하고, 제외 3파일(`C1GameSession.cs`, `C1PlayModeTests.cs`, `T0Strings.json`)은 main에서 `2ece517`과 동일하다. main `Builds/M5-mac/Unknown.app`도 격리 빌드와 316파일·353,421,261바이트·파일별 해시가 동일하다. 즉 "병합"은 복사 작업이 아니라 **스테이징·커밋 승인 판단**이다(미커밋·미푸시 상태 유지 중).
2. **TDD는 1개 행동에만 성립한다.** 아래 §4. 나머지 13개 M5 테스트는 test-after이며 GREEN 영수증만 있다.
3. **런타임 승격은 내부 프로토타입 한정이다.** `M5Direction.asset`의 `runtimeApproved: 1`과 `intro-m5-r03`·`m5-direction-surface-r01`의 `runtimeEligible: true`는 RFC-CX-007(`commercialEligible: false`) 범위다. 출시 자산 승인이 아니다.
4. **정답·출처·서사 비노출을 자동으로 지키는 회귀가 없다.** 오프닝 캡션·'관찰·시험·기록' 스트립 문자열은 정적 판독과 스크린샷으로만 확인했다(§3). 커밋 전 또는 후속으로 `T0Strings`/레코드 id·힌트 3단 문구 부재를 단정하는 PlayMode 1건을 권한다(소유자 결정).
5. **동작 줄이기(reduced motion) 의미 확인.** 구현은 "타이머 없이 두 캡션을 한 화면에 정적으로 보이고 즉시 `작업 시작` 버튼"이다(시스템 스펙 정의와 일치). 사용자 계약 문구 "immediate start"를 자동 통과로 읽는다면 불일치이므로 의도를 확정해 줄 것.

## 2. 변경 경로 (격리 워크트리 `6514f54` 기준, 통합 기준 `2ece517`)

수정 8: `App/T0GameSession.cs`(+42/−) · `App/C1SignatureGameSession.cs` · `UI/T0Interface.cs` · `Editor/Tide.EditorTools.asmdef`(+`Tide.Presentation` 참조) · `Tests/PlayMode/{C1SignaturePlayModeTests,T0CaseThreadTests,T0PlayModeTests,T0ResourceInteractionTests}.cs`(오프닝 도입에 맞춘 기존 테스트 4건 적응).
신규 10(+meta 7): `App/T0OpeningSession.cs` · `UI/T0OpeningInterface.cs` · `Presentation/M5DirectionProfile.cs` · `Editor/M5DirectionProjectBuilder.cs` · `Resources/M5Direction.asset` · `Art/Candidates/m5-direction/{Opening,Surface}.png` · `Tests/PlayMode/M5DirectionPlayModeTests.cs`.
오버레이 밖: 워크트리 루트 `README-m5-section.md`, `root-m5-handoff.json`, `graphify-out/*`(제외). main의 M5 문서군(`concept/presentation/production/qa/systems` 아래 `intro-gameplay-m5*`)은 다른 에이전트 소유의 미추적 파일이며 이 검토 대상이 아니다. main의 `c1-signature-reader-r01/*` 미추적 자산은 M4 잔여물로 M5와 무관하다.

## 3. 수용 계약 대조 (코드 + 테스트 + 네이티브)

| 계약 항목 | 구현 근거 | 자동 테스트 | 네이티브 |
|---|---|---|---|
| 신규 게임만 건너뛸 수 있는 오프닝, 정적 이미지, 6000ms 2캡션 | `T0OpeningSession.cs` `openingEligible = 저장 없음 ∧ HeadSeq==0 ∧ t0-b1 미완`; `3.125s + 2.875s = 6.0s`, `unscaledDeltaTime`, 캡션 전환은 `Render()` 1회 | `FreshIntroUsesObservedCutAndTimeoutWithoutWrites`, `LoadedSaveBypassesIntro…`, `SelectingExistingSlotBypasses…` | F01(6초 자연 경과 → t0-b1, save.json 없음) |
| 첫 프레임 건너뛰기·설정 접근 | `intro-skip`이 첫 액션·포커스, `settings`만 오버레이 허용, 설정 중 타이머 정지, Escape=`Back()`→`FinishOpening()` | `SkipFirstFrameAndEscapeConsumeInput`, `PointerSettingsFromReduced150IntroOpensAndReturns`, `SettingsPauseAndReducedMotionShowAllLabelsAt150Percent`, `FocusLossAndPauseStopIntroClock` | N01·N02 |
| 동작 줄이기 즉시 진행 | `UpdateOpening()` 조기 반환, 두 캡션 정적 표시 + `작업 시작` | 위 150% 테스트 | N01 |
| 오프닝 전이의 저장·퍼즐 무변경 | `FinishOpening()→StartGame()`은 저널·저장 미접근; 오프닝 중 Undo/Redo/도구/조정/미리보기/질의/분리 차단; 재생은 `overlay` 복원만 | `…WithoutWrites`, `…PreservesBytesAndContext`, `HeldSkip…`, `HeldInputAcrossCutAndTimeout…`, `ReplayFromTool…` | N03(save.json 미생성), F03(57커맨드·저장 바이트 불변) |
| 수동적 C1 '관찰·시험·기록' 스트립, 보이는 컨트롤·현재 상태에서만 파생 | `SignatureDirectionCategory(actionId)`가 선택 액션 id → 범주; 시험 상세=`c1.signature.humidity.<선택값|unset>`("습도 1~3단/미선택"), 기록 상세=`SavePending/saveFailure/Ready/완료` 상태; 강조는 색·글꼴만 | `SelectedCategoryChangesWithoutRerenderOrJournalWrite`, `FirstObservationStillAllowsRiskTrialAndMaskPersists`, `RecordStatusCannotAcceptPendingFailedOrCancelledSave` | N04·F02·F03 스크린샷(`native-final-v2/saved-record.png` 직접 확인: 관찰/시험/기록 탭, "기록 · 저장 완료") |
| 정답·출처·서사 비노출 | 캡션 4종·안내 1줄·스트립 4종 문자열에 레코드 id·습도 정답·힌트 3단 문구 없음; `Opening.png`는 야간 당직실 정물(문자·C1 이미지 없음, 직접 확인) | **없음(§1-4)** | `native-final-fresh/intro-recording-frame.png` 직접 확인 |
| 영상을 게임플레이로 미내장 | `Assets/_Project`에 `VideoPlayer/VideoClip/.mp4` 참조 0건; `import-audit.json` `runtimeVideoDependency:false`; previz provenance `runtimeEligible:false` | 해당 없음(정적 증거) | — |

## 4. TDD 감사 [OBSERVED — XML mtime·소스 mtime 대조]

- 프로덕션 파일 mtime: `M5DirectionProfile.cs` 15:48:53, `T0OpeningInterface.cs`/`T0Interface.cs` 15:57:18, `C1SignatureGameSession.cs` 15:58:43, `T0OpeningSession.cs` 15:59:57. **첫 테스트 실행은 16:00:31**(`playmode-m5-diagnostic-1.xml`, 9건 중 7 통과·2 실패).
- 기록된 RED→GREEN은 **1건**: `SettingsPauseAndReducedMotionShowAllLabelsAt150Percent` 16:00:31 실패(메시지 `back` — 인트로 설정의 뒤로가기 부재) → `T0GameSession.cs` 16:00:59 수정 → 16:01:28 통과.
- 같은 실행의 다른 실패 `FirstObservationStillAllowsRiskTrialAndMaskPersists`는 테스트의 오브젝트 이름 오류(테스트 수정) — 프로덕션 RED 아님.
- 이후 추가된 5건(`ReplayFromTool…`, `SelectingExistingSlot…` 16:01; `FocusLossAndPause…`, `HeldInputAcrossCutAndTimeout…` 16:04; `PointerSettings…` 16:08)은 **첫 기록 실행에서 바로 통과** — RED 없음.
- `playmode-full-diagnostic.xml`(16:06)의 상위 테스트 실패 4건(`DefaultGuidanceDoesNotRevealHumidityOrInventPageValue`, `GamepadStartsAndShellQueryHasNoAction`, `KeyboardStartsAndHeldConfirmCannotCrossScreen`, `LeftClickApprovedDrawerFront…`)은 "시작 즉시 플레이" 가정을 오프닝에 맞춰 고친 **적응 편집**이지 새 행동의 RED가 아니다.
- 결론: **"strict TDD"는 주장할 수 없다.** 정확한 서술은 "구현 선행 + 14건 GREEN + 1건 RED→GREEN 기록".

## 5. 실제 결과 (Aside가 XML을 다시 읽음)

| 영수증 | 결과 |
|---|---|
| `m5-direction/editmode-integrated-final.xml` | 36/36 |
| `m5-direction/playmode-integrated-final.xml` | 54/54 (M5 14 + 상위 회귀 `ObservedInvalidRoutingExplainsContradictionWithoutPrescribingAction` 포함) |
| `m5-direction/boot-integrated-final.xml` | 1/1 |
| `m5-direction/logs/build-integrated-final.log` | `M5_MAC_BUILD Succeeded bytes=353421261` |
| `intro-gameplay-m5/native-acceptance.json` | N01–N05 PASS(진단 프로파일), F01–F03 PASS(승인 프로파일·일반 빌드), N-OBS-01 NOT-REPRODUCED |
| 보존된 실패 | 16:00 M5 2건, 16:06 상위 4건, 16:11 boot 접두사 1건 — 모두 재실행으로 해소, 원본 XML 보존 |

## 6. 남은 한계·미완

- Aside 독립 재실행 없음(샌드박스). 사람 플레이·G4·G5·성능·물리 게임패드·오디오 미측정. 생성 영상은 previz이며 게임 증거가 아니다(`docs/media/intro-gameplay-m5/native-*.mp4`는 디렉터가 별도 녹화한 네이티브 화면이나, 이 검토는 그 인코딩·내용을 검증하지 않았다).
- 발행 단계(README 섹션 통합, 갤러리, freshness, graph/wiki, 커밋·푸시)는 디렉터가 진행 중이며 **커밋·푸시는 사용자 승인 사항**이다.
- 권장 후속 1건: 오프닝·스트립 비노출 회귀 테스트(§1-4). 권장 후속 2건: 동작 줄이기 의미 확정(§1-5).
