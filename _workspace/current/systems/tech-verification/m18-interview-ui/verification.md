---
updated: 2026-09-13
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
---

# M18 — C1 면접(대화) 준비 화면: 표시 전용 UI 기초 슬라이스

> **이 문서가 무엇인가**: 플레이어에게 보이는 **한 조각의 UI 기초**를 TDD로 추가하고, Unity 자동
> 테스트와 플레이어 빌드로 검증한 영수증이다. 정본 `c1-b4` 대화 비트의 구현이 **아니고**, 실제
> 대화 비트의 대체물도 **아니며**, 접근성 인증 · 사람 플레이테스트 · 성능 측정 · 네이티브 캡처
> 주장도 **아니다**. 어떤 게이트도 올리지 않는다.

같은 사이클 안의 제자리 추가 (RFC-Q2: `cycle` 불변 → `supersedes: null`, 아카이브 의무 없음).

연출 계약(소유 산출물): `_workspace/current/presentation/c1-interview-prep-m18.md` (`status: draft`)

## 0. 결론

[OBSERVED] C1 순찰을 **완료한** 플레이어에게만 보이는 명시적 액션 **`면접 준비 · 형식 안내`**가
추가되었고, 누르면 익명 발화자 레인 2개 + 중립 비교 프롬프트 + 명시적 복귀로 구성된
**표시 전용 패널**이 열린다. 복귀하면 같은 완료된 C1 순찰 화면으로 돌아오며, 전 구간에서
상태 해시 · 저널 head · `save.json` 바이트 · 성공 영수증 수가 **불변**이다.

| 단계 | 결과 | 영수증 |
|---|---|---|
| 집중 RED | 1 case, 0 passed, **1 failed** (액션 부재 단언) | `red.xml`, `red.log` |
| 집중 GREEN | 1 case, **1 passed**, 0 failed | `green.xml`, `green.log` |
| EditMode 전체 | 54 total, **54 passed**, 0 failed, 0 skipped | `editmode.xml`, `editmode.log` |
| PlayMode 전체 | 86 total, **85 passed**, 0 failed, 1 skipped(기존) | `playmode.xml`, `playmode.log` |
| macOS 플레이어 빌드 | `T0_MAC_BUILD Succeeded bytes=403839508` | `build-mac.log:6022` |
| M14~M17 보존 | 35/35 베이스라인 파일 무손실, HEAD 불변 | §6 |

## 1. 변경 (정확한 경로)

| 경로 | 변경 | sha256 |
|---|---|---|
| `unity/Unknown/Assets/_Project/App/C1InterviewPrepSession.cs` | **신규** — 게이트 · 열기/닫기 · 패널 화면 (partial `T0GameSession`) | `894123f6b9d27a9a61ddfd7e4efb0cdc2a6d05f4d23a55ce554e2f9afeb4beec` |
| `unity/Unknown/Assets/_Project/App/C1GameSession.cs` | **+2 / −0** — 완료된 `PatrolScreen`에 진입 액션 1개 | `499a95456d01a6c18fea7d8a6ee6073c95b3fdbb0d70fb8be5a37868fc2b16a8` |
| `unity/Unknown/Assets/_Project/App/T0GameSession.cs` | **+4 / −1** (M18 몫) — 오버레이 디스패치 1행 + 사건 카드 억제 1행 + 주석 2행 | `a61290acc193df9bc79c6861b1dc9dded95b45c8cb4a9b2c06d68c1ce3da0c92` |
| `unity/Unknown/Assets/_Project/Tests/PlayMode/C1InterviewPrepPlayModeTests.cs` | **신규** — 집중 PlayMode 테스트 1 case | `07fca34133f2d40bf29ae94f87a073126587f3331bbad45efdd179bcd1821bf7` |
| `…/C1InterviewPrepSession.cs.meta`, `…/C1InterviewPrepPlayModeTests.cs.meta` | 신규, Unity가 임포트 시 생성 | — |
| `_workspace/current/presentation/c1-interview-prep-m18.md` | 신규 소유 연출 계약 | `1b29b29ef9e142724deffb97005cc88832617ce80b70875d589095d9b03a5964` |

M18 기능 diff 전문:

```diff
# C1GameSession.cs — PatrolScreen, PatrolComplete 분기 (기존 두 액션 뒤에 append)
+                screen.Actions.Add(A("c1-interview-prep","면접 준비 · 형식 안내",OpenInterviewPrep,InterviewPrepAvailable,
+                    "다음 단계의 면접 형식만 보여줍니다. 기록은 바뀌지 않습니다."));

# T0GameSession.cs — OverlayScreen 디스패치
+   if(overlay==InterviewPrepOverlay){InterviewPrepScreen(s);return;}

# T0GameSession.cs — 사건 카드 억제 (reviewNotes 선례와 동일)
-   s.CaseThread=OpeningActive||overlay=="reviewNotes"?null:CaseThreadText();
+   s.CaseThread=OpeningActive||overlay=="reviewNotes"||overlay==InterviewPrepOverlay?null:CaseThreadText();
```

왜 이것이 최소 구현인가: 표시 전용 패널의 기존 선례(`overlay="reviewNotes"`)를 그대로 따랐다.
`Surface=>overlay??tool??"shell"`, `s.Actions.Clear()`+`OverlayScreen`, 일반 `Back()`의
`overlay!=null` 경로, `ApplyM7UiSkin`이 **이미** 존재하므로 신규 상태 기계 · 신규 입력 배선 ·
신규 위젯 · 신규 데이터 · 신규 저장 필드가 **0건**이다. 닫기는 기존 일반 Back 경로도 함께 쓴다.

[OBSERVED] 불변식 유지: 저장 필드 이름 변경·삭제 0건(이 경로는 쓰기 자체가 0건이므로
CLAUDE.md §9 마이그레이션 불변식 무관) · 밸런스/재화 숫자 0건 · 시뮬레이션 상태 쓰기 0건 ·
데이터 테이블(`Data/Tables/*`, `systems/data/t0/*`) 0건 · `T0Strings.json` 0건 ·
에셋 생성/구매 0건 · `runtimeEligible`/`runtimeApproved` 승격 0건 · 공유 진실 산출물 편집 0건.

### 1.1 RFC 판정

RFC 블록을 쓰지 **않았다**. 근거:
`references/dependency-matrix.md`의 presentation 행은 `syn/vfx/anim/mot/mod/qa ●`를 요구하지만,
RFC는 디렉터 소유 공유 진실 파일 `production/decision-log.md`에 기록되며 이번 작업 지시는 기존
공유 진실 산출물 편집을 금지했다. 또한 CLAUDE.md §11은 가상의 ack 생성을 금지한다. 따라서
matrix가 스스로 정한 처리(“required acks가 없으면 `status: draft`, 게이트 투입 불가”)를 적용해
연출 계약을 **`status: draft`** 로 두고 미해결 ● ack를 그 문서 §0에 명시했다. 이 영수증
(`systems/tech-verification/*`)은 관측 사실 기록이므로 M17 선례대로 `status: current`이며,
게이트를 올리지 않는다.

## 2. 게이트 (언제 보이는가)

```csharp
public bool InterviewPrepAvailable=>started&&!OpeningActive&&PatrolActive&&PatrolComplete&&!SignatureActive;
public void OpenInterviewPrep(){if(!InterviewPrepAvailable)return;OpenOverlay(InterviewPrepOverlay);}
```

[OBSERVED] 액션은 완료된 C1 순찰 화면의 `PatrolComplete` 분기 안에서만 추가되고, 열기 함수도
같은 술어로 한 번 더 방어한다. 신규 저장, 순찰 진행 중, 서명지 단계 진입 이후, 오프닝 중,
T0 기본 순찰/서명 기본 UI에는 **액션 자체가 존재하지 않는다**. RED 로그가 이를 실측으로 뒷받침한다:
완료 화면의 액션 집합은 `< "continue-c1-signature", "c1-review" >` 였고 M18 이전에는 면접 액션이
없었다.

## 3. TDD 증거

러너: `Unity 6000.5.6f1 -batchmode -nographics -runTests`,
필터 `Tide.Tests.C1InterviewPrepPlayModeTests`.

**RED은 설정 오류가 아니라 부재 단언에서 실패했다** (`red.xml`):

```
completed C1 patrol must offer the interview preparation action
  Expected: collection containing "c1-interview-prep"
  But was:  < "continue-c1-signature", "c1-review" >
```

[OBSERVED] 이 단언 **앞의 모든 단언이 통과**했다는 점이 harness가 실제 완료된 C1 화면에
도달했음을 증명한다 — `PatrolActive` 참, `PatrolComplete` 참, `SignatureActive` 거짓,
액션 집합에 `continue-c1-signature` 존재. 즉 "도착하지 못해서" 실패한 것이 아니라
**면접 표면이 없어서** 실패했다.

상태는 조작하지 않았다. 기존 C1/M5/M8 스위트가 쓰는 실제 완료 저장 픽스처
`_Project/Tests/Fixtures/C1PatrolCompletedV2.json`를 복사해 부팅했다. 목(mock) 0건, 수기 상태 0건.

테스트가 단언하는 플레이어 관측 사실:

| 항목 | 단언 |
|---|---|
| 진입 | 액션이 존재하고 `interactable`이며, 활성화 시 `Surface=="interviewPrep"` |
| 익명 레인 2개 | 렌더된 본문 텍스트에 `발화자 A` · `발화자 B` 포함 |
| 중립 비교 프롬프트 | 렌더된 본문에 `나란히 비교` 포함 |
| 명시적 복귀 | `c1-interview-prep-back` 존재 + `interactable` |
| 사건 카드 누출 차단 | `CaseThread` Text 컴포넌트가 계층에 **부재** |
| 복귀 | `Surface`가 진입 이전 값으로 복원, `PatrolComplete` 유지, `SignatureActive` 거짓, `continue-c1-signature`·`c1-review`·`c1-interview-prep` 모두 재존재 |
| 무변이 | `SavePending` 거짓, 상태 해시·저널 head·성공 영수증 수 동일, `save.json` 바이트 `CollectionAssert.AreEqual` — **진입 직후와 복귀 후 두 번** 측정 |

## 4. 공개 배제 검증 (연출 계약 §2)

배제 문자열을 **런타임이 읽는 실데이터에서 파생**시켰다. 정본 문자열을 테스트에 박아넣지
않았으므로 캐논이 자라도 검사가 계속 유효하다.

| # | 배제 대상 | 방법 | 결과 |
|---|---|---|---|
| E1 | 등장인물/화자 실명 | `C1PatrolContract`의 `journalCondition.actorDisplayName` 부재를 렌더 계층 전체에서 확인 | **부재** |
| E2 | 기록/출처 표시명 | 런타임 `records` 테이블의 `displayNameKo` **5건** 전부 부재를 렌더 계층 전체에서 확인 | **5/5 부재** |
| E3 | 정확한 값·시각 | 본문 + M18 액션 라벨에 숫자 문자 전면 부재 | **숫자 0개** |
| E4 | 정본 진술문 | 인용부호 `“` `”` 부재 | **부재** |

[OBSERVED] 추가 교차 검증 (`campaign.json`의 `c1-b4` 비트 문자열 33건 대조):
M18 플레이어 가시 텍스트에 **정본 문자열 일치 0건**. 2자 이상 한국어 토큰 중 겹치는 것은
`같은` · `아직` · `진술` 3개뿐이며, 모두 형식을 서술하는 일반 어휘다. `c1-b4`의 제목·단서·
행동·해법·도구·완료조건 어휘는 하나도 나타나지 않는다.

[OBSERVED] 패널이 노출하는 것은 **형식**뿐이다: 면접 절차의 존재, 익명 레인 2개, 같은 기준의
비교, 그리고 "상대와 진술 내용은 아직 열리지 않았습니다"라는 명시적 고지. 상대 정체 · 발언 내용 ·
시각 · 수치 · 조치 · 판정 규칙 · 해법 경로는 없다.

**경계 [OBSERVED]**: 헤더 부제의 `21:00 · <beatId>` 시계는 M18 저작 텍스트가 아니라 모든 화면에
이미 렌더되는 기존 크롬이다. E3는 M18 저작 본문·라벨에 대한 주장이며, 이 기존 크롬 제거를
주장하지 않는다. 패널 제목 `C1 · 면접 준비`의 숫자 `1`은 구조적 스테이지 id이며 문자열 등가
비교로 고정했다.

## 5. 전체 회귀

| 스위트 | 이번 실행 | M17 베이스라인 | 델타 |
|---|---|---|---|
| EditMode | 54 total, 54 passed, 0 failed | 54 / 54 | **0** — case 집합 동일 |
| PlayMode | 86 total, 85 passed, 0 failed, 1 skipped | 85 total, 84 passed, 0 failed, 1 skipped | **+1** = 이번 테스트 |

[OBSERVED] M17의 `playmode.xml`과 leaf case 집합 대조:
- M17에 있고 지금 **사라진 case: 0건**
- 추가된 case: `Tide.Tests.C1InterviewPrepPlayModeTests.CompletedPatrolOpensAnonymousInterviewFormatPanelAndReturnsWithoutAnyMutation` **1건뿐**
- 상태가 바뀐 case: **0건**
- 유일한 skip은 `T0BootSceneTests.SerializedBootLoadsHubAndInitializesVisibleStartScreen`으로
  M14/M15/M17 베이스라인에서도 skip이다. 기존 항목이며 이번에 생긴 것이 아니다.

[OBSERVED] 이 변경이 깨뜨릴 수 있었던 인접 표면이 모두 통과했다: `C1PlayModeTests`(완료된 순찰의
액션 집합과 원자적 확정), `C1SignaturePlayModeTests`(완료 화면 → 서명지 진입), `ReviewNotesPlayModeTests`
(같은 오버레이 기구를 쓰는 표시 전용 선례), `T0CaseThreadTests`(사건 카드 공개 가드),
`ScrollReachabilityTests`(액션 1개 추가 후에도 도달성 유지).

## 6. 범위 준수 — M14/M15/M16/M17 보존

[OBSERVED] 첫 쓰기 전에 `git status --porcelain` 43행을 기록하고 추적 변경 파일 35건을
sha256으로 베이스라인화했다. 모든 실행(테스트 3회 + 빌드) 이후 재검증:

| 검사 | 결과 |
|---|---|
| HEAD | `52ad5510522eca6d5cfd8d743d85f28cb254635f` **불변** |
| 베이스라인 43행 중 사라진 항목 | **0건** |
| 삭제된 베이스라인 파일 | **0건** |
| 내용이 바뀐 베이스라인 파일 | **1건** — `App/T0GameSession.cs` (의도적 배선, 아래 증명) |
| 나머지 베이스라인 파일 | **34/34 바이트 동일** |
| `graphify-out/*` (4건) | **불변** — 재생성·수기 편집 모두 없음 |
| `.mex/*` | **불변** — `mex` 계열 미실행 |

[OBSERVED] **`T0GameSession.cs`가 순수 추가임을 기계적으로 증명했다.** 현재 파일에서 M18이 넣은
4행(주석 2 + 디스패치 1 + `CaseThread` 조건의 `||overlay==InterviewPrepOverlay`)만 되돌리면
베이스라인 sha256이 정확히 재현된다:

```
baseline sha256 : 2436d0ae945cf7e6073cc67f96a364f9e9edfa2249d65804ec976a93d5ed934f
reconstructed   : 2436d0ae945cf7e6073cc67f96a364f9e9edfa2249d65804ec976a93d5ed934f
```

즉 M15가 이 파일에 넣은 설정 경로 수정과 그 외 모든 선행 내용이 **바이트 단위로 보존**되었다.
M18의 표면은 선행 작업과 분리되어 있다 — M14는 저작 목표 데이터·생성 테이블, M15는
`T0GameSession`의 설정 **경로**, M17은 `T0OpeningSession`의 **조건**, M18은 `OverlayScreen`
**디스패치**와 `CaseThread` **억제** + 신규 파일 2개.

[OBSERVED] `reset` · `stash` · `revert` · `clean` · `checkout` · `commit` · `push` **미실행**.
외부 공개 0건 · 에셋 구매/생성 0건 · 2D/3D/이미지/영상 생성 0건 · 런타임 승인 플래그 변경 0건 ·
기존 증거 디렉터리(`m14-objective/`, `m15-settings-slot/`, `m16-runtime-capture/`,
`m17-unity-polish/`) 편집 0건. `BuildMac`은 `Prepare()`를 호출하지 않으므로 씬/에셋을
재생성하지 않았고 `Builds/ Library/ Temp/`는 gitignore 대상이다.

M18이 추가한 작업 트리 항목은 7개이며 전부 M18 소유다:

```
 M unity/Unknown/Assets/_Project/App/C1GameSession.cs
?? unity/Unknown/Assets/_Project/App/C1InterviewPrepSession.cs (+ .meta)
?? unity/Unknown/Assets/_Project/Tests/PlayMode/C1InterviewPrepPlayModeTests.cs (+ .meta)
?? _workspace/current/presentation/c1-interview-prep-m18.md
?? _workspace/current/systems/tech-verification/m18-interview-ui/
```

## 7. 빌드 영수증

[OBSERVED] 표준 규칙대로 빌드 **직전에** 게이트를 측정했다:

| 항목 | 값 | 판정 |
|---|---|---|
| 빌드 전 여유 디스크 | **11.95 GiB** (12,527,040 KiB, `/System/Volumes/Data`) | ≥ 10 GiB → 빌드 허용 |
| Unity Editor 프로세스 | **없음** (`Unity.app` / `Unity Hub` / `UnityShaderCompiler`) | 허용 |
| `Temp/UnityLockfile` | **부재** | 허용 |
| 빌드 후 여유 디스크 | 11.94 GiB | — |

| 항목 | 값 |
|---|---|
| 메서드 | `Tide.EditorTools.T0ProjectBuilder.BuildMac` |
| 결과 | `T0_MAC_BUILD Succeeded bytes=403839508` (`build-mac.log:6022`), exit 0 |
| 디스크 실측 | **403,839,508 바이트 / 315 파일 — 보고값과 정확히 일치** |
| 메인 바이너리 | `Unknown.app/Contents/MacOS/Unknown T0`, 67,916 바이트 |
| 메인 바이너리 sha256 | `aeb397f5b1b118f78334fa182dd091ac707649a986ea6f5f603f1be3fa628e3d` |

[OBSERVED] 빌드는 이 변경이 표준 플레이어로 **컴파일·링크된다**는 증거다. 실행도, 캡처도,
성능 결과도 아니다. 플레이어를 **실행하지 않았다**.

## 8. 캡처 한계 (기존 조건 유지)

[OBSERVED] M16이 기록한 네이티브 진입 후 캡처 공백은 **macOS TCC Accessibility 거부**
(`accessibility: not-granted`)이며 합성 입력이 전달되지 않는 OS 권한 문제다. 게임 코드 결함이
아니고, 이번 증분에서 게임플레이 결함으로 재분류하지 않았다. 우회를 시도하지 않았고 승인 상태를
변경하지 않았으며, 어떤 네이티브 캡처도 주장하지 않는다. 이번 M18의 플레이어 관측 증거는
**PlayMode 렌더 계층 단언 + 컴파일/링크 빌드**로 한정된다.

## 9. 그래프/메모리 신선도 한계 (G8 주장 없음)

[OBSERVED] `graphify update .` 와 `mex` 계열(`mex graph`, `mex check`, `mex sync`, `mex log`)을
**의도적으로 실행하지 않았다.** 이유: `graphify-out/`의 4개 파일과 `.mex/` 산출물이 이미
커밋되지 않은 선행 작업 상태이며, 이번 지시가 그 보존을 요구했다. 재생성은 그 미커밋 산출물을
덮어쓴다.

**정확한 결과**: 이번 코드 변경(신규 파일 2개, 수정 파일 2개)은 코드 그래프와 프로젝트 메모리에
**반영되지 않았다**. 즉 `graphify-out/graph.json`·`GRAPH_REPORT.md`와 `.mex/`는 M18 이전 상태를
가리키며, **그래프/메모리 신선도는 갱신되지 않았다**. CLAUDE.md §5의 시스템 레인 순서 중
편집 이후 단계(`graphify update .` → `mex graph` → `mex check` → `mex log`)는 미이행이다.
`game-systems-designer` 정의에 따라 이 파일에 **`[UNGRAPHED]`** 로 표시한다.

- **G8을 PASS로 주장하지 않는다.** 신선도·메모리 게이트는 이번 증분으로 움직이지 않았다.
- `mex`는 이 머신에서 여전히 TeX 바이너리이며 `mex-agent` 신원 확인이 실패한다
  (session-start 출력: `mex: skipped — mex-agent not found or failed identity probe`).
  따라서 memory_sync 영수증은 **`skipped`** 이며, 이는 도구 부재 + 보존 요구의 **중첩** 결과다.
- 후속 작업: 선행 미커밋 산출물의 소유 세션이 정리된 후 `graphify update .` 와 mex 동기화를
  한 번에 수행해야 한다. 그때까지 그래프 기반 영향 분석은 M18 표면을 알지 못한다.

## 10. 한계 — 주장하지 않는 것

1. **UI 기초 한 조각일 뿐이다.** 정본 `c1-b4` 대화·심문 비트의 구현이 아니고, 그 비트의
   대체물도 아니다. 실제 진술 내용 · 분기 · 판정 · 압박 · 증거 제시는 이 조각에 없다.
2. **대화 시스템 설계가 아니다.** 상태 기계 0개, 신규 데이터 0건, 신규 저장 필드 0건.
3. **사람 플레이테스트 n=0.** 자동 테스트와 컴파일/링크 빌드만 수행했다. 플레이어 미실행.
4. **성능 주장 0건.** 프레임 시간 · 메모리 · GPU · 발열 측정 없음.
5. **네이티브 캡처 0건.** 실제 키보드/마우스/패드 입력 0회. 스크린샷 0장. §8 참조.
6. **접근성 인증이 아니다.** 기존 텍스트 배율·저감모션 설정을 상속만 하며, 이 패널을 어떤
   표준으로도 인증하지 않았다. 텍스트 배율별 레이아웃 적합성은 이번에 측정하지 않았다.
7. **G4–G7 NOT-MEASURED 유지, G8 주장 없음(§9).** 게이트 이동 0건.
8. 연출 계약은 **`status: draft`** 이며 미해결 ● ack(synopsis, vfx, animation, motion,
   modeling, qa)가 남아 있다. 게이트에 투입할 수 없다.
9. 복귀 경로는 패널의 명시적 복귀 액션으로 단언했다. 일반 `Back()`(키보드/패드/툴바)도 기존
   `overlay!=null` 경로로 같은 결과를 내지만 그것은 **구조에 의한 [INFERENCE]** 이며 별도
   단언 대상이 아니다.
10. 텍스트는 한국어 저작본만 있다. 현지화·번역 검수 범위 밖이다.
