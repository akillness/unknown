---
updated: 2026-09-10
cycle: 20260909-preproduction-c5
status: current
supersedes: _workspace/archive/20260909-preproduction-c4/systems/unity-implementation.md
owner: game-systems-designer
---

# Unity 구현 계약 — C5 (실행 가능한 구현 계약, 게임 존재 주장 아님)

## 0. 정직성 경계
- 이 문서는 **무엇을 어떻게 만들 것인가의 계약**이다. 빌드 0건, 플레이 0건, 프로파일 0건.
- G2/G4/G5/G6/G7은 `NOT-MEASURED`. 성능·용량·시간 수치는 전부 `[TARGET]`이며 관측치로 인용 금지.
- `[OBSERVED]`는 이 머신에서 명령으로 확인한 것만이다.
- C4 독립 검토(`qa/c4-review.md`) 대응은 `R1`~`R4`로 표시한다.

## 1. 엔진과 패키지
- `[OBSERVED] 2026-09-09 23:32 KST` — `ls /Applications/Unity/Hub/Editor/` → `2022.3.32f1-x86_64`, `2022.3.62f2`, `6000.5.6f1`.
- 채택 후보: **Unity 6000.5.6f1**(설치 확인됨). **LTS 여부는 확인하지 않았고 LTS라고 주장하지 않는다.**
- 대상: **Windows 우선**. macOS는 개발·검증용이며 성능 목표 대상이 아니다.
- 패키지 **버전은 발명 금지**. 값은 실제 resolve 후 `Packages/manifest.json` + `packages-lock.json`에서 그대로 옮긴다.

| package | 용도 | version |
|---|---|---|
| com.unity.inputsystem | KB/마우스 + 패드, 재매핑, 릴리스 래치 | `[PIN-AFTER-RESOLVE]` |
| com.unity.localization | KO/EN 문자열·폰트 테이블 | `[PIN-AFTER-RESOLVE]` |
| com.unity.ugui | UI 토대(단일 스택) | `[PIN-AFTER-RESOLVE]` |
| com.unity.render-pipelines.universal | 2.5D 디오라마 렌더 | `[PIN-AFTER-RESOLVE]` |
| com.unity.cinemachine | 고정 시점 간 컷 | `[PIN-AFTER-RESOLVE]` (후보) |
| com.unity.addressables | 구역 단위 로드 | `[PIN-AFTER-RESOLVE]` (후보) |
| com.unity.test-framework | EditMode/PlayMode 인수 테스트 | `[PIN-AFTER-RESOLVE]` |

- 유료 에셋·외주·외부 API·네트워크 의존성 **0건**. 텔레메트리 송신 없음.

## 2. 모듈 경계 (sim/render 분리) `[RFC-S2 판정 반영 2026-09-10 R7 · C7-F4]`

**정본은 `systems/architecture-contract.md` §2 의 7분할이다.** 이 절의 이전 판은 5분할(`Tide.Domain / Data / App / Presentation / Tests`)을 적어 같은 레인의 두 문서가 다른 말을 하고 있었다(C7-F4). 디렉터 판정 `production/decision-log.md` 「RFC-S2 / C7-F4 · asmdef 분할」에 따라 **7분할로 정정**한다. 코드 0줄이므로 개명 비용은 0이다.

| assembly | UnityEngine 참조 | 책임 | 이전 판에서의 위치 |
|---|---|---|---|
| `Tide.Sim` | **없음** | 규칙·퍼즐 상태·이벤트·리듀서·명령 로그·판정 | `Tide.Domain` 개명(저장소 불변식 "sim/render 분리"와 이름 일치) |
| `Tide.Data` | 있음 | ScriptableObject/JSON 저작본 → Sim DTO 변환, 임포트 검증 | 동일 |
| `Tide.Save` | 있음 | 직렬화·원자적 쓰기·체크섬·마이그레이션·복구 | `Tide.App` 에서 분리 |
| `Tide.Input` | 있음 | 장치 추상화·포커스 모델·리바인딩·액션 → 의도 | `Tide.App` 에서 분리 |
| `Tide.Presentation` | 있음 | 스냅샷 구독 렌더·카메라·연출 보간. **sim 상태를 쓰지 않는다** | 동일 |
| `Tide.UI` | 있음 | 화면·패널·바인딩·로컬라이즈 표시 | 신설(Presentation 안에 있으면 "규칙 재구현" 회귀를 막을 경계가 없다) |
| `Tide.App` | 있음 | 합성 루트·씬 전환·설정·수명주기 | 축소(저장·입력이 빠졌다) |
| `Tide.Tests.Sim` / `Tide.Tests.Play` | Sim쪽 없음 / Play쪽 있음 | 인수 테스트 | `Tide.Tests` 분할 |
| `Tide.EditorTools` | 있음(Editor 전용) | `PackageBootstrap` · 테이블 생성 메뉴 | 신설(C7-F2) |

- **"7분할" = 생산 어셈블리 7개**(`Sim/Data/Save/Input/Presentation/UI/App`). 테스트 2종과 `EditorTools` 를 더한 asmdef **파일 수는 10**이다.
- `Tide.Sim`은 `UnityEngine`, `Time`, `Random`, `Application`, 파일 I/O, 문화권 의존 파싱을 참조하지 않는다(asmdef `references: []` + `noEngineReferences: true` 로 강제). 역방향 참조는 **컴파일 실패**여야 한다(인수 `T-B1`).
- 허용 의존 방향과 모듈별 금지 목록은 `architecture-contract.md` §2·§2.1 이 소유하며 이 표는 그 요약이다.

## 3. 이벤트 기반 불변 퍼즐 상태 `[C7-F7 정정 2026-09-10 R7]`
- `PuzzleState`는 불변 레코드. 변경은 `Reduce(state, evt) -> newState` 뿐.
- **지속되는 재생 단위는 명령이다.** `CommandLog`가 append-only이며 `CommandEntry(seq, branchId, parentSeq, commandId, payload, payloadHash, committed)`를 담는다 — **정본은 `data-schemas/save.md` §3.0~§3.2**.
- `EventLog`는 `Commit`이 만들어 내는 **메모리 안의 파생 구조**이며 **세이브 파일에 나타나지 않는다.** 이전 판은 이 자리에 `EventLog (seq, evtId, payload, causeCommandId)`를 지속 구조처럼 적어 `save.md` §3과 두 모델이 공존했다(C7-F7). 저장되는 것은 명령뿐이고 이벤트는 재생으로 다시 만들어진다.
- 그 대가로 **`Commit`은 순수 함수여야 한다** — 같은 `(state, commandId, payload)`는 항상 같은 이벤트 열을 낸다(`save.md` `S-I10`). 난수·시계·문화권 파싱·딕셔너리 순회 순서 의존 0건이며, `Tide.Domain`이 `UnityEngine`·`Time`·`Random`을 참조하지 않는 것(§2)이 그 강제 수단이다.
- 스냅샷 = `state + seq + logHash`, 200 커밋 간격.
- 되돌림은 파괴가 아니라 `undoPointer` 이동이다. 연습은 폐기되는 분기 사본에서 수행한다.
- `autoKeptClueIds`는 어떤 이벤트로도 축소되지 않는다.
- 전역 실시간 타이머 없음. `Time.deltaTime`은 연출 보간에만 쓰인다.

## 4. 명령 프리뷰/확정
```
interface IPuzzleCommand {
  ValidationResult Validate(PuzzleState s);   // 실패 사유 코드 + 사람이 읽는 문장
  PreviewReport   Preview(PuzzleState s);     // 후보 결과, 비용, 되돌림 가능성, 영향 구역
  IReadOnlyList<PuzzleEvent> Commit(PuzzleState s);
}
```
- `Validate` 통과 전에는 확정을 활성화하지 않고 **비활성 이유를 문장으로** 보여준다.
- `Preview`는 부작용이 없다(같은 입력 2회 호출 후 상태 해시 불변, T-03).
- `R2` **`Commit`의 결과는 저장이 성공해야 권위를 얻는다**(§7).

## 5. 결정론적 퍼즐 그래프 `R3`
- 노드/간선은 JSON 저작. 순회는 `id` 정렬 고정, 난수 없음, 딕셔너리 순서 의존 금지.
- 자료 스키마는 `sourceType`(염판/일지/대장)과 `originId`(물리 출처)를 분리하고 파생본은 `copiedFrom`으로 루트를 가리킨다.
- 임포트 시 강제하는 불변식:
  1. `proofRequired` 결론마다 **`sourceType` 상이 AND `originId` 상이**를 만족하는 자료쌍이 최소 1개 존재하고, 그중 1개 경로는 어떤 플레이어 행동으로도 파괴되지 않는다. (C4의 모호한 "매체 경로"라는 세 번째 용어는 폐기하고 이 두 필드로만 말한다.)
  2. 임의 도달 상태에서 **세 엔딩 모두 재시작 없이 접근 가능**.
  3. 고아 노드·미사용 단서 0건.
  4. 모든 노드의 로컬라이제이션 키가 KO/EN 양쪽에 존재.
  5. `copiedFrom` 참조가 **모두 해석 가능**하고 **순환이 없다**.
- 위반 시 **임포트 실패(fail-closed)**. 경고 통과 없음.

## 6. 데이터 임포트 검증 `R3`
- 저작: ScriptableObject(구역·도구·단서·노드) + JSON(그래프·연표·힌트).
- 검증기는 에디터와 배치 테스트에서 같은 코드를 호출하며 **읽기 전용**이다.
- 별칭·파생 루트 해석을 포함한다: `copiedFrom` 체인을 끝까지 따라가 루트 `originId`를 계산하고, **참조 누락과 순환은 둘 다 실패**로 처리한다. 표시 라벨이 다르다는 사실은 독립성 증명이 아니다.

## 7. 저장 스키마 v1 — 내구 후보 트랜잭션 `R2`
```json
{ "schemaVersion": 1, "saveId": "...", "createdUtc": "...", "updatedUtc": "...",
  "stageId": "C3", "beatId": "c3-b2", "storyClock": "23:00", "tidePhase": "spring-high",
  "commandLog": { "headSeq": 812, "branchId": "main", "logHash": "sha256:...",
                  "entries": [ { "seq": 812, "branchId": "main", "parentSeq": 811,
                                 "commandId": "AlignBaseline",
                                 "payload": { "offsetMinutes": 8 },
                                 "payloadHash": "sha256:...", "committed": true } ],
                  "snapshots": [], "entryCap": 20000, "byteCap": 6291456 },
  "commitIdempotencyKey": "0f2c…",
  "progress": { "autoKeptClueIds": ["..."], "propertyProtection": null,
                "hintLevelUsed": { "c3-b2": 2 } },
  "settingsRef": "settings.json", "checksum": "sha256:..." }
```
> **이 조각은 `data-schemas/save.md`의 축약 예시이며 필드 목록의 정본이 아니다.** 이전 판은 여기에 `chapter`/`dayIndex`(단일 야간 캐논에서 죽은 필드 · RFC-S3)와 `eventSeq`/`eventLogHash`(이벤트 소싱 잔재 · C7-F7)를 적어 스키마 문서와 어긋나 있었다. 위 조각은 `save.md` §1·§3과 문자 단위로 맞춘 것이며, 어긋나면 **`save.md`가 이긴다**.
- **파일 분리**: `save.json`(현재 자동 저장) · `checkpoint.pre-commit.json`(확정 직전) · `save.bak`(직전 세대) · 수동 슬롯 3개. 복구 3단계가 실제로 서로 다른 파일을 갖는다.
- **쓰기 절차**: 임시 파일 기록 → **플랫폼이 지원하면 fsync** → 원자적 rename. 부분 파일이 정본 자리에 오지 않는다.
- **`SavePending`**: 진행 표시 + **중복 커밋만 차단**. 되돌림·설정·힌트·열람 등 나머지 UI는 응답을 유지한다. 전역 입력 잠금 없음.
- **실패**: 메모리 상태와 디스크를 둘 다 확정 이전으로 유지하고 성공 연출을 재생하지 않으며 `재시도`/`뒤로`를 제시한다.
- **rename 성공 직후 UI 갱신 전 크래시**: 다음 실행에서 정본이 새 세대이므로 복구 로드로 이어받고, `commitIdempotencyKey`가 같은 커밋의 중복 적용을 막는다.
- **지연 완료(stale completion)**: 이미 취소·이탈한 커밋의 완료 콜백이 늦게 도착하면 키 불일치로 폐기하고 UI를 바꾸지 않는다.
- 마이그레이션: 낮은 버전은 등록된 체인을 적용하고, **상위·미등록 버전은 거부하고 파일을 그대로 둔다**.

## 8. 카메라·씬 구성 — 걷는 아바타 없음 `R4`
- **노드 기반 2.5D 디오라마 + 고정 조사 시점**. 구역당 시점 노드 6~10개 `[TARGET]`.
- 이동은 컷/짧은 돌리이며 `CharacterController`·NavMesh·경로탐색·충돌 이동을 **쓰지 않는다**. 로코모션 클립 0개.
- 씬 단위: `hub`, `gate`, `lowland`, `dock`, `pump` + `boot`, `ui-root`(가산 로드).

## 9. 성능 예산 `[TARGET only]` `R2`
| 항목 | 목표 | 축 | 상태 |
|---|---|---|---|
| 프레임 시간 1920x1080 | 16.7 ms | 렌더 | NOT-MEASURED |
| **확정 입력 렌더 응답(ack)** | **1프레임 이내** | 렌더 | NOT-MEASURED |
| **저장 디스크 완료** | **200 ms** | I/O | NOT-MEASURED |
| 시점 전환 첫 프레임 | 100 ms | 렌더 | NOT-MEASURED |
| 구역 전환 로드 | 3 s | I/O | NOT-MEASURED |
| 기준 하드웨어 | 미정 | – | NOT-MEASURED |
- 두 수치는 **서로 다른 축**이다. 1프레임은 "눌림이 화면에 보이기까지", 200 ms는 "디스크 커밋 완료까지"다. 저장이 프레임 예산 안에 끝나야 한다는 요구는 이 계약에 없다.

## 10. 수직 슬라이스 T0와 게이트 `R4`
- **T0 범위(확정)**: 공간 **`hub` 1개**, 도구 **`circuit` + `reader` 2개**, 목표 길이 **25분**.
- alignment·routing·corrosion·seal은 **T0에 없다**. T0의 결론 판정은 이 두 도구로 닫히는 것만 쓰며, 배선 범위 술어는 `circuit`이 실제로 산출한다(스텁 아님).
- `systems/prototype/`의 **브라우저 참조 모형(출처 독립성·정합·서명 3규칙)은 T0 게임이 아니다.** 규칙 무모순성 확인용 참조이며 조작감·재미·플레이 시간을 주장하지 않는다.
- **alignment는 별도 그레이박스 스파이크**로 검증한다: 캠페인 비트 `c3-b2`/`c3-b3`만 떼어 **독립 측정**하며, 전체 캠페인 중앙값을 산출하지 않는다. 그레이박스는 **추가 완성 에셋을 만들지 않는다**(기존 프리미티브·플레이스홀더만).
- **본 생산 게이트는 두 증거를 모두 요구한다**: (a) T0 사람 검증, (b) alignment 스파이크 사람 개념 검증. 하나만으로는 통과하지 않는다.
- **T0 25분은 8시간을 증명하지 않는다.** 슬라이스 측정치로 480분 완주를 주장하지 않으며, `observedMedianMinutes`는 여전히 `null`이다.
- 25분 목표는 C3(비트 33개 개정 중)로 넘기는 **새 입력값**이며, 비트 배분 확정은 C3 소유다.

## 11. 기술 인수 테스트 (공개 상태 단언, 문자열 검색 금지)
| id | 단언 |
|---|---|
| T-01 | `Reduce`는 입력 상태를 변경하지 않는다(해시 불변) |
| T-02 | 같은 이벤트 열 재적용 시 동일 상태 해시(결정론) |
| T-03 | `Preview` 2회 호출 후 상태 해시 동일(부작용 없음) |
| T-04 | `Validate` 실패 시 `Commit`은 이벤트 0개를 반환한다 |
| T-05 | 무작위 10k 스텝 후에도 `AutoKeptClues`가 축소되지 않는다 |
| T-06 | 임의 도달 상태에서 엔딩 3종 각각에 도달 가능 |
| T-07 | 소프트락 없음 = 모든 도달 상태에 진행 가능한 확정 경로 1개 이상(상태 술어) |
| T-08 | 부분 기록 저장 주입 시 `.bak`으로 복구하고 정본을 덮어쓰지 않는다 |
| T-09 | `schemaVersion=0` 저장이 마이그레이션 후 v1을 만족한다 |
| T-10 | `schemaVersion=2` 저장은 거부되고 파일 바이트가 변하지 않는다 |
| T-11 | 임포트 검증기가 불변식 위반 픽스처 5종을 전부 실패시킨다 |
| T-12 | 로컬라이제이션 미해결 키 0건 |
| T-13 | 되돌림 포인터를 0까지 내린 뒤 재적용하면 원래 해시로 복귀 |
| T-14 | `propertyProtection` 두 값에서 필수 단서와 엔딩 접근성이 동일 |
| `R2` T-15 | **rename 직전 쓰기 실패 주입** → 상태와 디스크가 확정 이전이고 성공 연출이 재생되지 않는다 |
| `R2` T-16 | **중간 쓰기 실패 주입**(임시 파일 절단) → 정본 바이트 불변, 복구 패널 경로 제시 |
| `R2` T-17 | **재시도 멱등성**: 같은 `commitIdempotencyKey`로 2회 성공해도 이벤트가 한 번만 적용된다 |
| `R2` T-18 | **지연 완료**: 취소된 커밋의 늦은 성공 콜백이 UI·상태를 바꾸지 않는다 |
| `R2` T-19 | **rename 후 크래시 재현**: 재시작 시 새 세대를 이어받고 커밋이 중복 적용되지 않는다 |
| `R2` T-20 | `SavePending` 중 확정만 비활성이고 되돌림·설정·힌트 입력은 계속 처리된다 |
| `R3` T-21 | 원본 + 파생 스캔(라벨만 다름) 쌍은 `copiedFrom` 루트 해석으로 거부된다 |
| `R3` T-22 | 서로 다른 `originId`의 염판 2점은 `sourceType` 중복으로 **의도적으로 거부**되고 사유가 구분 표기된다 |
| `R3` T-23 | `proofRequired`가 아닌 판독·경로 프리뷰는 서명 없이 진행 가능하다 |
| `R1` T-24 | **키보드 단독 전 퍼즐 완주**: T0 퍼즐 전체를 포인터·홀드 없이 완료한다(화면 도달이 아니라 퍼즐 종료로 판정) |
| `R1` T-25 | 확정 방식 3값(`two-step`/`hold`/`confirm-dialog`) 각각으로 같은 퍼즐을 완주할 수 있다 |
| `R1` T-26 | **도구 패널이 열린 동안** `X`는 그 패널이 `interaction-rules.md` §1-3.2에 **등재한 부작용 없는 실행 1개**만, `Y`는 그 패널이 **등재한 의미 1개**만 수행한다(등재가 없으면 **명령 0개**). 동시 활성 0건 [C7-F9 정정 2026-09-10 R7] |
| `R1` T-26a | **T0 실체화 — `circuit`**: `X` = 구획 접기 토글(`ToggleUncovered`), `Y` = **등재 없음 → 명령 0개**. 「프리뷰」도 「해제」도 발행되지 않는다 |
| `R1` T-26b | **T0 실체화 — `reader`**: `X` = 사본 재생(`Read`, 부작용 0 · `ReadOriginal`이 아니다), `Y` = 인용 고정(`CiteToBoard`) |
| `R1` T-27 | 화면 전환 직후 눌린 상태가 유지돼도 확정이 발행되지 않는다(릴리스 래치) |

- 저장 실패·중간 실패·크래시는 **주입(injection)으로만 재현**한다. 실제 디스크를 채우거나 물리 장치를 손상시키는 테스트는 만들지 않는다.

## 11-A. 변경 로그 (같은 사이클 제자리 개정 · RFC-Q2)

| 날짜 | 회차 | 결함 | 바뀐 절 | 내용 |
|---|---|---|---|---|
| 2026-09-10 | **R7 종료 수정** | **C7-F4** · RFC-S2 | §2 전면 재작성 | 모듈 경계를 5분할(`Tide.Domain/Data/App/Presentation/Tests`) → **7분할**(`Sim/Data/Save/Input/Presentation/UI/App` + 테스트 2 + `EditorTools`)로 정정했다. 디렉터 판정 「RFC-S2 / C7-F4 · asmdef 분할」이 `architecture-contract.md` §2 를 정본으로 확정했고, 같은 레인 두 문서가 다른 말을 하던 상태가 닫힌다. **코드 0줄이므로 개명 비용 0.** 「7분할 = 생산 어셈블리 7개, asmdef 파일 10개」라는 구분을 함께 적었다 |

- `cycle` 값 불변(RFC-Q2). `status` 는 QA 재검증 전까지 `draft`(C3-F33).
- 이번 개정으로 **새로 측정된 값 0건** — 빌드 0회, asmdef 파일 0개 생성.

## 12. 한계
- 이 계약은 문서다. **어떤 항목도 실행으로 검증되지 않았다.**
- 패키지 버전, 기준 하드웨어, 프레임·저장 실측, 시점 노드 실제 개수, T0 실제 소요는 미정이다.
- Unity 6000.5.6f1의 LTS 지위, Steam Deck 검증, 실제 패드·보조기기 매핑은 확인되지 않았다.
- alignment·routing·corrosion·seal의 조작감은 T0로 검증되지 않는다. 별도 스파이크 전까지 미검증이다.
