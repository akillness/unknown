---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-planner
---

# C1 M4 · 겹쳐 붙은 서명지

[OBSERVED] 이 계약은 `campaign.json`의 `C1 / c1-b2` 한 비트만 구체화한다. 앞선 `c1-b1` 패킷을 대체하지 않으며 동일 문서의 이전 버전이 없어 `supersedes: null`이다. `c1-b3`의 필압·잉크·봉인 완료 접점과 이후 이름·필자·의도 공개는 범위 밖이다.

## 목표와 플레이어 경험

[TARGET] 원본을 보존하는 기록 담당자로서 손끝으로 서명지를 분리하고, 서로 다른 매체가 같은 밤을 가리키는지 스스로 대조한다.

[CARRIED] 소금에 눌어붙은 부속 서명지 두 장을 손상 없이 분리해 사본으로 뜨고, 판 #0의 번호대가 같은 철에 걸리는지 확인한다.

기록의 출처는 손으로 선택하고, 습도 시험의 피드백을 보고 분리를 결정한다. 자료 표면과 읽기 쉬운 글자가 중심이며, 화면의 기본 안내는 위 목표 한 문장이다. 첫 화면에 정답 단수·완성 순서·최종 추론을 전시하지 않는다.

## 권위와 추적

| 항목 | 근거 |
|---|---|
| 비트·관찰·결과·3층 힌트 | `planning/campaign.json` → `stages[id=C1].beats[id=c1-b2]` |
| 허브의 판 #0 영구 슬롯 | `synopsis/chapter-beats.md` §4-4, `t0-b1.consequence` |
| K2 확정과 후반 재확인 | `synopsis/continuity.md` §5 (`c1-b2` → `c4-b2`) |
| 첫 판독 보존·사본 무제한 | `worldview/worldview-bible.md` §3 법2; `systems/system-specs/plate-readout.md` P-R2/P-R9 |
| 서로 다른 매체와 루트 | `systems/interaction-rules.md` §3; `systems/data-schemas/beats.md` |
| 구현 추론 승인 | `production/decision-log.md` RFC-CX-005 (2026-09-11) |

## 준비 상태와 확정

[INFERENCE, RFC-CX-005 ACK] 완료 술어는 다음의 논리곱이다.

```text
c1-b1 완료
AND c1-b2-c1 / c1-b2-c2 실제 관찰
AND 안전한 습도 시험 후 명시적으로 두 장 분리
AND 첫째·둘째 장 검증 사본을 각각 명시적으로 보존
AND 둘째 사본의 가려진 영역 1개 표시
AND 판 #0 번호대 명시적 대조
AND log(signature-annex) × plate(plate-zero) 독립 근거 쌍 선택
AND 현재 요청의 성공 저장 영수증 수락
```

[TARGET] 성공 저장 영수증 전에는 완료·체크포인트·확정 사건판 연결을 부여하지 않는다. 미리보기의 분리·사본·영역·비교 준비 상태는 성공을 미리 확정한 것으로 취급하지 않는다. 최종 확정은 6효과(검증 사본 묶음, 미해결 영역, 번호대 연결, 독립 인용, 비트 완료, `cp-c1-b2`)를 1트랜잭션으로 적용한다. 취소·실패·오래된 영수증·중복 영수증·즉시 명령 우회는 부분 적용 0건을 보장해야 한다.

## 분리와 사본 계보

[INFERENCE, RFC-CX-005 ACK] 습도는 미선택에서 시작한다. `low / medium / high` 3단의 서열만 사용하며 실제 습도 %나 잉크 손상 수치를 만들지 않는다. 1단을 명시적으로 시험하면 안전한 분리를 허용하고, 나머지 2단은 가역적 잉크 번짐 위험만 표시한다. 단수 변경은 시험 결과를 무효화하지만 이미 분리한 사실과 사본을 훼손하지 않는다.

[CARRIED] 첫 관찰은 원본 내용을 자동 보존한다. 이 안전 사본은 작업 목표의 두 장별 검증 사본 수에 포함하지 않는다. 두 장을 분리한 뒤 각 장에 대해 별도 보존 액션이 필요하며 같은 장을 반복 눌러도 사본 수는 증가하지 않는다.

| 사본 id / leaf originId | copiedFrom | rootOriginId | sourceType |
|---|---|---|---|
| `c1-b2-copy-1` | `signature-annex` | `signature-annex` | `log` |
| `c1-b2-copy-2` | `signature-annex` | `signature-annex` | `log` |

[INFERENCE] leaf id는 런타임 장부 식별자이며 새로운 극중 출처가 아니다. 독립 근거 수는 `rootOriginId`와 `sourceType`으로 판정한다. 두 서명지 사본 또는 자동 사본과 원본을 별개 독립 근거로 세지 않는다.

## 미해결 영역과 번호대

[CARRIED] 둘째 서명란 아래쪽은 계속 가려진 미해결 영역이다. 이를 눌러도 문자가 복원되거나 이름이 나타나지 않는다.

[INFERENCE, RFC-CX-005 ACK] 사본 로컬 좌상단 기준 정규화 사각형 `x=0.12, y=0.70, width=0.76, height=0.22`를 한 번 표시한다. 이는 UI 주석 좌표이며 서명의 실제 길이·각도·측정값이 아니다. UI 확대·화면 위치와 무관하게 같은 사본 영역을 가리킨다.

[CARRIED] 번호대 대조는 서명지철과 판 #0을 같은 사건의 대역으로 연결하고 명령 대기 구간의 존재만 기록한다. 12시간 대역은 기존 캐논이다. 번호 숫자·절대 시작/끝 시각·곡선 표본·대기 이유·필자 신원을 새로 만들지 않는다. `t0-b1`에서 정한 판 #0 슬롯과 `t0-b3`의 출처 정체성은 그대로 이어받는다.

## 복구·힌트·표현

[TARGET] 습도 시험은 비용·횟수·시뮬레이션 시간 증가가 없다. 시험 상태 되돌리기는 단수 선택과 시험 결과만 초기화한다. 관찰, 자동 보존, 기존 분리, 검증 사본, 영역 표시, 번호대 대조를 보존하며 누락된 작업을 자동 수행하지 않는다. 실패한 시험은 진행 손실을 만들지 않는다.

[TARGET] 힌트는 0층에서 시작하고 요청 1회당 최대 1층만 공개한다. `narrative.action / inference / hints`는 기본 UI에서 제외한다. 정답 단수가 명시된 캐논 3층 힌트는 3번째 명시적 요청 후에만 보인다. 시험 피드백은 플레이어가 해당 시험을 실행한 뒤 보여준다. 차단 사유는 현재 누락 요건을 설명하며 조작 순서나 안전한 단수를 먼저 알려주지 않는다.

[TARGET] 아트의 종이·염분·금속은 현재 컨셉의 재질/팔레트를 따르고, generated texture에 판독 가능한 서명·이름·번호를 넣지 않는다. 관찰·출처·미해결 상태는 별도 UI 텍스트와 형태로 표현한다. 장식 기하나 색만으로 논리 판정을 만들지 않는다. 100%/150% 텍스트, 포인터·키보드·패드에 같은 액션이 있어야 한다.

[TARGET] 저장은 schema v3를 쓰고 실제 v1/v2 저장을 원본 백업을 보존하며 이전한다. T0 명령 identity/스냅샷 해시와 c1-b1 완료/출처 조건/열람 권한을 유지한다. 알 수 없는 미래 버전은 거부하고, c1-b2 완료가 c1-b3 완료를 만들지 않는다.

## 수용 기준

모든 행은 [TARGET] 런타임/QA 기준이다. 아래 정적 검사 결과와 구분한다.

| id | 검증 상황 | 수치 기대값 |
|---|---|---|
| C1-S01 | Prerequisite c1-b1 absent: entry/confirm blocked | `{"entryGrants":0,"commits":0}` |
| C1-S02 | Observe exact authored records, preserve both on first read, duplicate observation idempotent | `{"observations":2,"sourceMismatches":0,"automaticBackups":2,"verifiedSheetCopies":0}` |
| C1-S03 | Unselected and three ordinal humidity states; explicit low trial permits separation only | `{"totalStates":4,"safeLevels":1,"unsafeLevels":2,"unselectedSeparationGrants":0}` |
| C1-S04 | Medium/high trials then reset/retry do not destroy originals, backups, copies or prior progress | `{"originalDamage":0,"lostBackups":0,"cost":0,"completionGrants":0}` |
| C1-S05 | Separation before source observation or successful trial rejected; valid repeated separation idempotent | `{"invalidGrants":0,"validSeparationCount":1}` |
| C1-S06 | Two explicit per-sheet copies after separation; duplicate sheet action not a third copy | `{"distinctCopies":2,"duplicateAddedCopies":0,"independentRoots":1}` |
| C1-S07 | One copy-local lower region marked only after second copy exists; remains unreadable | `{"regionCount":1,"unresolvedRegionCount":1,"newReadableNames":0}` |
| C1-S08 | Explicit comparison requires both observations and copies and preserves exact origins | `{"comparisonCount":1,"sourceMismatches":0,"inventedNumericSamples":0}` |
| C1-S09 | Valid log/plate pair accepted; same-root copies and same-type pairs rejected | `{"acceptedPairCount":1,"sameRootAccepted":0,"sameTypeAccepted":0}` |
| C1-S10 | Missing any readiness predicate blocks confirmation even when all others are ready | `{"partialApplications":0,"completionGrants":0}` |
| C1-S11 | Current successful save receipt commits all effects exactly once | `{"atomicEffectCount":6,"completedBeatsAdded":1,"checkpointsAdded":1}` |
| C1-S12 | Immediate submission, cancel, failed write, stale/duplicate receipt cannot commit or partially apply | `{"prematureCommits":0,"partialApplications":0,"duplicateApplications":0}` |
| C1-S13 | Whole citation undo/redo preserves backup and prior T0/c1-b1 state | `{"partialStates":0,"lostAutomaticBackups":0,"priorStateMismatches":0}` |
| C1-S14 | Actual v1/v2 fixture migration to v3 retains command identity/hash; future versions refused | `{"supportedReadVersions":3,"priorIdentityMismatches":0,"discardedOriginalBackups":0,"futureVersionsAccepted":0}` |
| C1-S15 | Level-zero default omits solution; each explicit hint reveal advances at most one layer and changes no puzzle state | `{"defaultHintLevel":0,"hintLayers":3,"maxIncrement":1,"stateMutations":0,"defaultSolutionLines":0}` |
| C1-S16 | 150% text remains readable; pointer keyboard pad expose actions; art does not reveal name | `{"textScale":1.5,"requiredInputPaths":3,"missingLocalizationKeys":0,"colorOnlyStates":0,"readableHiddenNames":0}` |
| C1-S17 | Native save/restart preserves completed c1-b2 evidence and shows bounded endpoint | `{"completedSignatureBeats":1,"checkpointMismatches":0,"c1b3AutoCompletions":0}` |

## 저작 검증 영수증

[OBSERVED 2026-09-11] Node 인라인 정적 검사 44/44 PASS: 캠페인 7개 서사 필드와 2개 관찰의 원문·출처 일치, 비트/권한/체크포인트, 3단 중 안전 1단·초기 미선택, 장별 사본 2개·동일 루트·계보 무자기참조, 정규화 사각형, log/plate 독립쌍, 6효과, 수락 저장 경계, schema v3 호환 선언, 3층 순차 힌트, 참조 localization key 0누락, 17개 수치 수용 기준을 확인했다.

이는 JSON 저작 일관성 검사이며 Unity 실행·실제 입력·저장 이전·아트 판독성·성능·사람 플레이테스트를 입증하지 않는다. 설계 13분은 측정된 플레이 시간이 아니다. 생성 리소스 출처와 승격은 별도 provenance 및 디렉터 판정에 따른다.
