---
updated: 2026-09-09
cycle: 20260909-preproduction-c4
status: superseded
supersedes: null
owner: game-systems-designer
---

# Unity 구현 계약 — C4 (실행 가능한 구현 계약, 게임 존재 주장 아님)

## 0. 정직성 경계
- 이 문서는 **무엇을 어떻게 만들 것인가의 계약**이다. 빌드 0건, 플레이 0건, 프로파일 0건.
- G2/G4/G5/G6/G7은 `NOT-MEASURED`. 성능·용량·시간 수치는 전부 `[TARGET]`이며 관측치로 인용 금지.
- `[OBSERVED]`는 이 머신에서 명령으로 확인한 것만이다.

## 1. 엔진과 패키지
- `[OBSERVED] 2026-09-09 23:32 KST` — `ls /Applications/Unity/Hub/Editor/` → `2022.3.32f1-x86_64`, `2022.3.62f2`, `6000.5.6f1`.
- 채택 후보: **Unity 6000.5.6f1**(설치 확인됨). **LTS 여부는 확인하지 않았고 LTS라고 주장하지 않는다.** 채택 확정은 슬라이스 T0가 이 버전에서 실제로 열린 뒤.
- 대상: **Windows 우선**. macOS는 개발·검증용이며 성능 목표 대상이 아니다(목표 사양 미정 = NOT-MEASURED).
- 패키지 **버전은 발명 금지**. 아래는 이름과 용도만이며, 값은 실제 resolve 후 `Packages/manifest.json` + `packages-lock.json`에서 그대로 옮겨 적는다.

| package | 용도 | version |
|---|---|---|
| com.unity.inputsystem | KB/마우스 + 패드 통합 입력, 리바인딩 | `[PIN-AFTER-RESOLVE]` |
| com.unity.localization | KO/EN 문자열·폰트·에셋 테이블 | `[PIN-AFTER-RESOLVE]` |
| com.unity.ugui | UI 토대(문서 기준 UGUI 단일 스택) | `[PIN-AFTER-RESOLVE]` |
| com.unity.render-pipelines.universal | 2.5D 디오라마 렌더 | `[PIN-AFTER-RESOLVE]` |
| com.unity.cinemachine | 고정 시점 간 컷/돌리 | `[PIN-AFTER-RESOLVE]` (후보) |
| com.unity.addressables | 구역 단위 로드 | `[PIN-AFTER-RESOLVE]` (후보) |
| com.unity.test-framework | EditMode/PlayMode 인수 테스트 | `[PIN-AFTER-RESOLVE]` |

- 유료 에셋·외주·외부 API·네트워크 의존성 **0건**. 텔레메트리 송신 없음. 오프라인 단독 실행.

## 2. 모듈 경계 (sim/render 분리)
| assembly | UnityEngine 참조 | 책임 |
|---|---|---|
| `Tide.Domain` | **없음** | 퍼즐 상태·이벤트·리듀서·그래프·판정. 순수 C#, 테스트가 엔진 없이 돈다 |
| `Tide.Data` | 있음 | ScriptableObject 저작본 → Domain DTO 변환, 임포트 검증 |
| `Tide.App` | 있음 | 저장/로드, 로컬라이제이션, 입력, 씬·노드 전환, 설정 |
| `Tide.Presentation` | 있음 | 스냅샷 구독 렌더. **sim 상태를 쓰지 않는다(읽기 전용)** |
| `Tide.Tests` | 있음 | 인수 테스트 |

- 규칙: Presentation → Domain 단방향. Domain은 `UnityEngine`, `Time`, `Random`, `Application` 어느 것도 참조하지 않는다(어셈블리 정의로 강제).

## 3. 이벤트 기반 불변 퍼즐 상태
- `PuzzleState`는 불변 레코드. 변경은 `Reduce(state, evt) -> newState` 뿐이며 in-place 수정 없음.
- `EventLog`는 append-only. `(seq, evtId, payload, causeCommandId)`. 스냅샷 = `state + seq + logHash`.
- **되돌림은 파괴가 아니라 포인터 이동**이다. `undoPointer`를 낮추면 이후 이벤트는 남아 있고 재적용 가능 → 무제한 되돌림.
- **연습은 로그의 분기 사본**에서 수행하고 확정하지 않으면 폐기한다. 실패 횟수 제한·자원 소모 없음.
- 확정된 필수 단서는 `autoKeptClueIds`에 들어가며 **어떤 이벤트로도 제거되지 않는다**(법2 자동 사본).
- 전역 실시간 타이머 없음. `Time.deltaTime`은 연출 보간에만 쓰이고 도메인에 들어가지 않는다.

## 4. 명령 프리뷰/확정
```
interface IPuzzleCommand {
  ValidationResult Validate(PuzzleState s);   // 실패 시 사유 코드 + 사람이 읽는 이유
  PreviewReport   Preview(PuzzleState s);     // 바뀌는 필드, 비용, 되돌림 가능성, 영향 구역
  IReadOnlyList<PuzzleEvent> Commit(PuzzleState s);
}
```
- UI는 `Validate`가 통과하기 전에는 확정 버튼을 활성화하지 않고, **비활성 이유를 항상 문장으로 보여준다**.
- `Commit` 직전 자동 체크포인트를 만든다(선택 재시도 보장).
- `Preview`는 부작용이 없어야 하며 테스트가 이를 검증한다(같은 입력 2회 호출 시 상태 해시 불변).

## 5. 결정론적 퍼즐 그래프
- 노드/간선은 JSON 저작, 임포트 시 검증. 순회는 `id` 정렬 고정, 딕셔너리 순서 의존 금지, 난수 없음.
- 임포트 시 강제하는 불변식:
  1. 모든 확정 결론에 **겹치지 않는 매체 경로 2개 이상**, 그중 1개는 어떤 플레이어 행동으로도 파괴되지 않는다(C2 F5 대응).
  2. 임의 도달 가능 상태에서 **세 엔딩 모두 재시작 없이 접근 가능**.
  3. 고아 노드·순환 참조·미사용 단서 0건.
  4. 모든 노드에 로컬라이제이션 키가 KO/EN 양쪽에 존재.
- 위반 시 **임포트 실패(fail-closed)**. 경고로 통과시키지 않는다.

## 6. 데이터 임포트 검증
- 저작: ScriptableObject(구역·도구·단서·노드) + JSON(퍼즐 그래프·연표·힌트).
- 검증기는 에디터 메뉴와 배치 테스트 양쪽에서 같은 코드를 호출한다. 스키마·ID 중복·참조 무결성·수치 범위·문자열 키 존재를 본다.
- 검증기는 **읽기 전용**이며 저작 자산을 자동 수정하지 않는다.

## 7. 저장 스키마 v1
```json
{ "schemaVersion": 1, "saveId": "...", "createdUtc": "...", "updatedUtc": "...",
  "chapter": 3, "dayIndex": 2, "tidePhase": "spring-high",
  "eventSeq": 812, "eventLogHash": "sha256:...",
  "autoKeptClueIds": ["..."], "propertyProtection": false,
  "hintLevelUsed": { "p-gate-03": 2 }, "settingsRef": "settings.json",
  "checksum": "sha256:..." }
```
- **원자적 쓰기**: `save.tmp` 기록 → flush/fsync → 기존 `save.json`을 `save.bak`으로 회전 → `rename(tmp, save.json)`. 부분 파일이 정본 자리에 오는 경로가 없다.
- **복구 순서**: `save.json` 체크섬 실패 → `save.bak` → 마지막 자동 체크포인트 → 세 개 모두 실패 시 **덮어쓰지 않고** 읽기 전용 복구 패널을 띄운다.
- **마이그레이션**: `schemaVersion`이 낮으면 등록된 마이그레이터 체인을 순서대로 적용하고 결과를 새 파일로 쓴다. **더 높거나 미등록 버전이면 절대 덮어쓰지 않고** 복구 패널로 이동한다(구버전 클라이언트가 신버전 세이브를 파괴하지 못한다).
- 필드 개명은 마이그레이터 없이는 금지(save 호환 불변식).

## 8. 카메라·씬 구성 — 걷는 아바타 없음
- 구성: **노드 기반 2.5D 디오라마 + 고정 조사 시점**. 구역당 시점 노드 6~10개 `[TARGET]`.
- 이동은 노드 간 **컷 또는 짧은 돌리**이며, `CharacterController`·NavMesh·경로탐색·충돌 이동을 **쓰지 않는다**.
- 근거: 저작 대상이 "걸어 다닐 수 있는 연속 공간"이 아니라 "완성해야 할 시점 N개"로 한정되어 **에셋 비용 상한이 셀 수 있게** 된다.
- 씬 단위: `hub`, `gate`, `lowland`, `dock`, `pump` + `boot`, `ui-root`(가산 로드).

## 9. 성능 예산 `[TARGET only]`
| 항목 | 목표 | 상태 |
|---|---|---|
| 프레임 시간 1920x1080 | 16.7 ms | NOT-MEASURED |
| 시점 전환 응답 | 100 ms 이내 첫 프레임 | NOT-MEASURED |
| 구역 전환 로드 | 3 s 이내 | NOT-MEASURED |
| 저장 쓰기 | 200 ms 이내 | NOT-MEASURED |
| 기준 하드웨어 | 미정 | NOT-MEASURED |
목표는 프로파일 결과가 아니다. 기준 PC가 정의되기 전에는 통과/실패를 말할 수 없다.

## 10. 수직 슬라이스 T0 (20~30분)
포함: `hub` + `gate`, 도구 `reader`/`alignment`/`seal`, 결론 1건, 3단계 힌트, 확정 프리뷰·체크포인트, 저장/로드/손상 복구, KO/EN, KB/마우스 + 패드.
제외: 나머지 3구역, `circuit`/`routing`/`corrosion` 전체 깊이, 엔딩 3종 본편 분량, 480분 콘텐츠.
**480분 전체 생산은 사람 실측(최소 12명/5유형) 이후에만 게이트를 넘는다.**

## 11. 기술 인수 테스트 (공개 상태 단언, 문자열 검색 금지)
| id | 단언 |
|---|---|
| T-01 | `Reduce`는 입력 상태 인스턴스를 변경하지 않는다(해시 불변) |
| T-02 | 같은 이벤트 열을 재적용하면 동일 상태 해시가 나온다(결정론) |
| T-03 | `Preview` 2회 호출 후 상태 해시가 같다(부작용 없음) |
| T-04 | `Validate` 실패 시 `Commit` 호출은 예외 없이 이벤트 0개를 반환한다 |
| T-05 | 임의 시드 무작위 행동 10k 스텝 후에도 `AutoKeptClues ⊆ CurrentClues` |
| T-06 | 임의 도달 상태에서 그래프 탐색이 엔딩 3종 각각에 도달 가능 |
| T-07 | **소프트락 없음** = 모든 도달 상태에 `AvailableCommands.Count > 0`이고 진행 가능한 확정 경로가 최소 1개 (문자열 매칭이 아니라 상태 술어) |
| T-08 | 부분 기록 세이브 주입 시 로더가 `.bak`으로 복구하고 정본을 덮어쓰지 않는다 |
| T-09 | `schemaVersion=0` 세이브가 마이그레이션 후 v1 스키마를 만족한다 |
| T-10 | `schemaVersion=2` 세이브는 거부되고 파일 바이트가 변하지 않는다 |
| T-11 | 임포트 검증기가 불변식 위반 픽스처 4종을 전부 실패시킨다 |
| T-12 | 모든 로컬라이제이션 키가 KO/EN에 존재하고 미해결 키 0건 |
| T-13 | 되돌림 포인터를 0까지 내린 뒤 재적용하면 원래 상태 해시로 복귀 |
| T-14 | `propertyProtection` 두 값 모두에서 필수 단서 집합과 엔딩 3종 접근성이 동일 |

## 12. 한계
- 이 계약은 문서다. **어떤 항목도 실행으로 검증되지 않았다.**
- 패키지 버전, 기준 하드웨어, 프레임·로드 실측, 씬 노드 실제 개수, 슬라이스 소요 시간은 전부 미정이다.
- Unity 6000.5.6f1의 LTS 지위, Steam Deck 검증, 컨트롤러 실제 매핑은 확인되지 않았다.
