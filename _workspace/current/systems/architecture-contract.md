---
updated: 2026-09-10
cycle: 20260909-preproduction-c3
status: current
supersedes: null
owner: game-systems-designer
---

# 아키텍처 계약 — C3 (Unity 6000.5.6f1)

이 문서는 **모듈 경계와 불변식의 소유 문서**다. 코드는 아직 0줄이고 빌드·플레이·프로파일 실측도 0건이다.
여기 적힌 성능·용량 수치는 전부 `[TARGET]`이며 어떤 게이트(G4~G7)도 올리지 않는다.

## 0. 정직성 경계

| 구분 | 내용 |
|---|---|
| `[OBSERVED]` | 이 저장소의 파일 내용과 Unity 프로젝트 설정 파일에서 읽은 값만 |
| `[TARGET]` | 아직 만들지 않은 것에 대한 설계 목표 |
| `[INFERENCE]` | 관측에서 추론한 것, 검증 계획을 함께 표기 |
| 측정 | 프레임타임·로드·세이브 시간·메모리 **n = 0**. `systems/tech-verification/README.md` 참조 |

## 1. 관측된 프로젝트 사실 [OBSERVED]

| 항목 | 값 | 출처 파일 |
|---|---|---|
| 에디터 버전 | `6000.5.6f1 (0e0577a1a2ac)` | `unity/Unknown/ProjectSettings/ProjectVersion.txt` |
| `Assets/` 내용 | 파일 0개, 하위 폴더 0개 | `unity/Unknown/Assets/` |
| 패키지 의존성 | `com.unity.multiplayer.center 1.0.1` + 내장 모듈 34종만 | `unity/Unknown/Packages/manifest.json` |
| Input System | **없음** (`activeInputHandler: 0` = 레거시 Input Manager) | `ProjectSettings/ProjectSettings.asset:682` |
| 렌더 파이프라인 | `m_CustomRenderPipeline: {fileID: 0}` = 내장 파이프라인 | `ProjectSettings/GraphicsSettings.asset:49` |
| productName / companyName | `Unknown` / `DefaultCompany` | `ProjectSettings/ProjectSettings.asset:15-16` |

[INFERENCE] LTS 여부는 확인하지 않았고 LTS라고 주장하지 않는다. 채택 확정은 C4 슬라이스가 이 버전에서 실제로 열린 뒤다.
[OBSERVED] `com.unity.multiplayer.center`는 신규 프로젝트 기본값이다. 본작에 멀티플레이가 없으므로 C4에서 제거 대상으로 표기한다(제거 자체는 코드 회차 작업).

## 2. 모듈 경계

| 모듈 | 어셈블리 | UnityEngine 참조 | 책임 | 금지 |
|---|---|---|---|---|
| Sim | `Tide.Sim` | **없음** | 규칙·상태·판정·명령 로그·리듀서. 순수 C# | 엔진 타입, `Time`, `Random`, `Application`, 파일 I/O, 문화권 의존 파싱 |
| Data | `Tide.Data` | 있음 | 저작본(ScriptableObject/JSON) → Sim DTO 변환, 임포트 검증 | Sim 상태를 직접 만들지 않음(항상 DTO 경유), 런타임 중 저작본 수정 |
| Save | `Tide.Save` | 있음 | 직렬화, 원자적 쓰기, 체크섬, 마이그레이션, 복구 | Sim 규칙 판단, UI 표시 문자열 생성 |
| Input | `Tide.Input` | 있음 | 장치 추상화, 포커스 모델, 리바인딩, 액션 → 의도(Intent) | Sim 상태 직접 변경(항상 명령 제출) |
| Presentation | `Tide.Presentation` | 있음 | 스냅샷 구독 렌더, 카메라, 연출 보간 | **Sim 상태 쓰기 금지**, Sim 내부 자료구조 참조 |
| UI | `Tide.UI` | 있음 | 화면·패널·바인딩·로컬라이즈 표시 | 규칙 판정 재구현(항상 `Validate`/`Preview` 결과 표시) |
| App | `Tide.App` | 있음 | 합성 루트, 씬 전환, 설정, 수명주기 | 규칙·직렬화 로직 보유 |
| Tests | `Tide.Tests.Sim` / `Tide.Tests.Play` | Sim쪽 없음 / Play쪽 있음 | 인수 테스트 | 프로덕션 빌드 포함 |
| **EditorTools** `[C7-F34 등재 2026-09-10 R7 종료]` | `Tide.EditorTools` | 있음(**Editor 전용** — `includePlatforms: ["Editor"]`) | 패키지 부트스트랩(`PackageBootstrap`), 테이블 생성 메뉴, 저작 데이터 임포트 도구 | **런타임 어셈블리가 이것을 참조하지 않는다**(단방향: `EditorTools → Data/Sim` 읽기만). 플레이어 빌드에 포함되지 않으므로 게임 로직·규칙 판정을 여기에 두지 않는다 |

- **"7분할" = 생산 어셈블리 7개**(`Sim`·`Data`·`Save`·`Input`·`Presentation`·`UI`·`App`). 테스트 2종과 `EditorTools` 를 더한 **asmdef 파일 수는 10**이다. RFC-S2 판정문의 「7분할」은 앞의 수를 가리킨다.
- `[C7-F34]` 이전 판은 `Tide.EditorTools` 를 §3 폴더 트리와 브리프 §③ 두 곳에만 적고 **이 경계표에는 넣지 않았다** — 참조 허용 범위·금지·의존 방향이 정의되지 않은 어셈블리가 하나 있었다는 뜻이다. 위 행이 그 공백을 메운다.

### 2.1 허용 의존 방향 (단방향)

```
Tide.App ──> Tide.UI ──> Tide.Presentation ──┐
   │           │                             ├──> Tide.Sim   (읽기 전용 스냅샷)
   ├──> Tide.Input ──────────────────────────┘
   ├──> Tide.Save ──> Tide.Sim  (스냅샷 직렬화 / 명령 로그 직렬화)
   └──> Tide.Data ──> Tide.Sim  (DTO 주입)
```

```
Tide.EditorTools ──> Tide.Data ──> Tide.Sim      (Editor 전용 · 역방향 참조 없음)
```

- `Tide.Sim`은 어떤 모듈도 참조하지 않는다. asmdef의 `references`가 비어 있어야 하고 `noEngineReferences: true`로 강제한다.
- 역방향 참조(예: Sim → Presentation)는 asmdef 수준에서 **컴파일 불가**여야 한다. 문서 규칙이 아니라 빌드 규칙이다.
- Presentation과 UI는 `WorldSnapshot`(불변 구조체 그래프)만 읽는다. 세터·컬렉션 변경 메서드를 노출하지 않는다.

### 2.2 병행 편집 중인 초안과의 이름 매핑 (RFC-S2)

`systems/unity-implementation.md`는 같은 레인의 병행 산출물이다. **작업 중 이 파일이 cycle `c4` draft → cycle `c5` draft로 갱신되는 것을 관측했다**(다른 세션이 같은 폴더에서 앞선 회차를 돌고 있다) [OBSERVED 2026-09-10]. 두 판본 모두 `Tide.Domain / Tide.Data / Tide.App / Tide.Presentation / Tide.Tests` 5개를 쓴다. 본 계약은 그 문서를 대체하지 않고 **경계를 더 쪼갠다**. 코드가 0줄이므로 개명 비용은 0이며, 어느 쪽을 채택할지는 디렉터가 판정한다.

| `unity-implementation.md` (c4/c5 draft) | 본 계약 | 사유 |
|---|---|---|
| `Tide.Domain` | `Tide.Sim` | "sim/render 분리"라는 저장소 불변식(CLAUDE.md §9)과 이름을 일치 |
| `Tide.App` | `Tide.App` + `Tide.Save` + `Tide.Input` | 세이브 마이그레이션과 입력 포커스는 서로 다른 실패 모드를 가지며 테스트 경계도 다르다 |
| (없음) | `Tide.UI` | UI가 Presentation 안에 있으면 "규칙 재구현" 회귀를 막을 경계가 없다 |

→ **RFC-S2 판정됨 — 7분할 채택** `[2026-09-10 R7 종료 · C7-F4]`. `production/decision-log.md` 「RFC-S2 / C7-F4 · asmdef 분할」: 「**7분할(`architecture-contract.md` §2, status current)이 정본**. `unity-implementation.md` §2 의 5분할 서술을 7분할로 정정(systems)」. **그 정정은 완료됐다** — `unity-implementation.md` §2 는 이제 위 표와 같은 경계를 적고 §11-A 변경 로그가 그 사실을 기록한다. 따라서 이 절은 **"병행 편집 중인 초안과의 이름 매핑"에서 "해소된 이름 매핑의 역사"로 지위가 바뀐다.** 표는 다음 회차가 옛 이름을 만났을 때를 위해 남긴다.

## 3. 폴더 레이아웃 [TARGET]

```
unity/Unknown/Assets/_Project/
  Sim/            (asmdef Tide.Sim, noEngineReferences)
    State/        PuzzleState, ZoneState, RecordState, SealState
    Commands/     ICommand, Validate/Preview/Commit 구현
    Log/          CommandLog, BranchId, Snapshotter
    Rules/        Law1..Law6 판정기 (법 = 클래스 1개씩)
  Data/           (asmdef Tide.Data)
    Authoring/    ScriptableObject 저작본 — Zones/ · Records/ · Tools/
    Tables/       beats.json · hints.json · tables-receipt.json  ← 생성기 출력(손으로 만들지 않음)
    Import/       ReceiptVerifier(V-1~V-4), RuntimeInvariants(R-1~R-4), DTO 매퍼
  Save/           (asmdef Tide.Save) Writer, Reader, Migrations/v0_to_v1.cs, Recovery
  Input/          (asmdef Tide.Input) Actions.inputactions, FocusModel, DeviceGlyphs
  Presentation/   (asmdef Tide.Presentation) SnapshotBinder, Cameras, Fx
  UI/             (asmdef Tide.UI) Screens/, Panels/, Bindings/
  App/            (asmdef Tide.App) Bootstrap, SceneRouter, Settings
  Editor/         (asmdef Tide.EditorTools, Editor 전용) PackageBootstrap, EmitTablesMenu
  Localization/   ko.csv, en.csv (키만, 문자열은 코드·아트에 굽지 않음)
  Art/            Meshes/ · Textures/ · UI/ · Portraits/   ← 승격된 에셋의 유일한 자리
  Scenes/         boot.unity, ui-root.unity, hub/gate/lowland/dock/pump.unity
  Tests/
    EditMode/     (asmdef Tide.Tests.Sim, Sim만 참조)
    PlayMode/     (asmdef Tide.Tests.Play)
```

- `Assets/_Project/` 밖에는 서드파티·패키지 샘플만 둔다. 프로젝트 코드가 루트에 흩어지면 asmdef 경계가 무너진다.
- **`[C7-F6]` 예외 0건 — 2026-09-10 R7 정정**: 이전 판은 이 줄 바로 위에 `Assets/Scenes/`(밖)를 두었고 `handoff/asset-runbook.md` §3.2 는 승격 경로를 `Assets/Art/**`(역시 밖)로 적어 **규칙과 두 그림이 어긋나 있었다**. 씬·아트·에디터 도구를 전부 `_Project/` 안으로 들여 규칙을 문자 그대로 참이 되게 했다. 비용 0 — `unity/Unknown/Assets/` 는 파일 0개이고 승격 자산 0종이다 [OBSERVED]. 이 규칙을 깨는 경로가 필요하면 **여기를 먼저 고친다**(브리프·런북은 이 절을 인용한다).
- **`Data/Tables/` 는 생성기 출력 전용이다**(C6-F13). 저작 원본 → `planning/validate-campaign.mjs`(규칙 단일 출처) → `systems/pipeline/emit-tables.mjs` → `Tables/` + `tables-receipt.json` → 임포터는 **영수증 대조 + 런타임 전용 불변식만**. 절차 정본 = `data-schemas/beats.md` §1-2·§1-3.
- 씬 5개(구역) + `boot` + `ui-root`는 가산 로드(`Assets/_Project/Scenes/`). 근거: `planning/campaign.json`의 `zoneIds` = `hub, gate, lowland, dock, pump` 5종 [OBSERVED].

## 4. 패키지 [TARGET] — 버전은 발명하지 않는다

| 패키지 | 필수/선택 | 이유 | 버전 |
|---|---|---|---|
| `com.unity.inputsystem` | **필수** | 마우스·키보드·패드 동시 지원과 런타임 리바인딩이 요구사항(`gdd.md` 범위). 현재 `activeInputHandler: 0`이라 전환 작업이 실재한다 | `[PIN-AFTER-RESOLVE]` |
| `com.unity.ugui` (TextMeshPro 포함) | **필수** | 텍스트 밀도가 높은 판독·서명 UI. 한글 글리프 폰트 대체 필요 | `[PIN-AFTER-RESOLVE]` |
| `com.unity.localization` | **필수** | KO/EN 2종이 범위. 문자열 키 부재를 임포트 실패로 잡으려면 테이블이 필요 | `[PIN-AFTER-RESOLVE]` |
| `com.unity.test-framework` | **필수** | 배치모드 인수 테스트(§13, tech-verification) 없이는 G6 입력이 없다 | `[PIN-AFTER-RESOLVE]` |
| `com.unity.render-pipelines.universal` | **선택(권장)** | 2.5D 디오라마 조명·포스트. 내장 파이프라인으로도 성립하나 젖은 금속 표현 비용이 커진다 | `[PIN-AFTER-RESOLVE]` |
| `com.unity.cinemachine` | **선택** | 고정 시점 노드 간 컷/짧은 돌리. 노드가 10개 이하면 수동 카메라로도 가능 | `[PIN-AFTER-RESOLVE]` |
| `com.unity.addressables` | **선택** | 구역 단위 로드. 씬 5개 규모에서는 `SceneManager` 가산 로드로 대체 가능 | `[PIN-AFTER-RESOLVE]` |
| `com.unity.multiplayer.center` | **제거** | 멀티플레이 없음. 기본 템플릿 잔재 | 현재 `1.0.1` [OBSERVED] |

- 유료 에셋·외주·외부 API·네트워크 의존성 0건. 텔레메트리 **원격 송신 없음**(로컬 파일만, `ops/telemetry-contract.md` §7).
- 버전 값은 실제 resolve 후 `Packages/manifest.json`과 `packages-lock.json`에서 그대로 옮겨 적는다. 지금 숫자를 쓰면 그것은 발명이다.

## 5. 연습(sandbox) / 확정(commit) 분기

```
main 브랜치 (커밋된 진실)
   │
   ├─ Fork(sandboxId) ──> 사본 상태 위에서 무제한 조작·되돌림
   │                        · 세이브에 반영되지 않음
   │                        · 부식·재생 예산은 "가상 소모"로만 표시
   │
   └─ Commit(sandboxId) ──> 검증 통과분만 main에 재적용, 체크포인트 생성
```

| 규칙 | 내용 |
|---|---|
| S1 | `Fork`는 `PuzzleState`를 **복사하지 않는다**. 현재 `headSeq`를 부모로 하는 새 `branchId`만 만든다(O(1)) |
| S2 | sandbox 브랜치의 명령은 `committed: false`로 로그에 append된다. Sim은 브랜치별 상태를 지연 계산한다 |
| S3 | `Commit`은 sandbox 명령열 전체를 main 상태에 **다시 `Validate`** 한 뒤 통과할 때만 적용한다. sandbox에서 통과한 것이 main에서 통과한다고 가정하지 않는다 |
| S4 | 확정된 것만 세이브의 `commandLog`에 남는다. 폐기된 sandbox는 개수(`sandboxDiscardedCount`)와 총 시간(`sandboxTimeMin`)만 남긴다 |
| S5 | sandbox에서는 어떤 자원도 실제로 줄지 않는다. **원본 상태 카운터**(법2)·**부식 상한**(법5)은 **표시만** 된다. 부식은 sandbox 밖에서도 소모되지 않는다(RFC-P3-009) |
| S6 | sandbox 중 저장하면 sandbox는 저장되지 않는다. UI가 "연습 내용은 저장되지 않습니다"를 확정적으로 고지한다 |

## 6. 무제한 되돌림 = 명령 로그 재생

```csharp
// Tide.Sim (개념 서명, 구현 아님)
// [C7-F7] Payload 신설 — 이것이 없으면 명령 로그를 재생할 수 없다(해시로는 명령을 되살리지 못한다).
readonly record struct CommandEntry(long Seq, string BranchId, long ParentSeq,
                                    string CommandId, JsonObject Payload,
                                    string PayloadHash, bool Committed);
```

**재생 단위는 명령 하나다** `[C7-F7 · 2026-09-10 R7]`. 이벤트는 `Commit`의 파생물이며 **세이브에 저장되지 않는다**. 성립 조건은 `Commit`의 **순수성**(같은 `(state, commandId, payload)` → 같은 이벤트 열)이고, `Tide.Sim`이 `Time`·`Random`을 참조하지 않는 것(§2)이 그 강제 수단이다. 절차 정본 = `data-schemas/save.md` §3.0~§3.2.

| 규칙 | 내용 |
|---|---|
| U1 | 되돌림은 **삭제가 아니라 `headSeq` 하향**이다. 로그 항목은 지워지지 않는다 |
| U2 | 되돌린 뒤 새 명령을 넣으면 **새 브랜치로 분기**한다. 앞의 이력은 계속 접근 가능하다 |
| U3 | 되돌림 **횟수 제한 0**, 비용 0, 페널티 0 (`economy/resources-and-fairness.md` 준수). 프로토타입의 `LIMITS.maxUndo: 32`는 **탐색 편의 상수이며 사양이 아니다**(RFC-P3-015 F23, `prototype/prototype.meta.md`) |
| U4 | 상태 재구성은 `for e in entries: state = Reduce*(state, Commit(state, e.commandId, e.payload))`. 같은 로그는 항상 같은 상태 해시를 만든다(결정론). **저장된 것은 명령이고 이벤트는 재생으로 다시 만든다** `[C7-F7]` |
| **U4b** | 재생 중 `Validate` 실패는 조용히 건너뛰지 않고 복구 패널로 간다(`save-undo.md` `SV-F9`) |
| U5 | 로그가 길어지는 비용을 막기 위해 **200 커밋마다 스냅샷**을 저장한다. 로드 시 최신 스냅샷 이후만 재생 |
| U6 | 되돌림 불가 구간은 **없다**. 다만 `Commit`은 **프리뷰 + 확정 입력 + 사전 체크포인트** 3중 확인을 요구한다. 확정 입력의 기본값은 **`two-step`**이며 길게 누름은 opt-in이다(`interaction-rules.md` §1-1, RFC-P3-015 F10) — 홀드를 확정의 전제로 적지 않는다 |
| U7 | 텔레메트리 `undo_count`는 U1 이벤트만 센다. 브랜치 폐기는 `sandbox_discarded`로 따로 센다 |

[TARGET] 로그 상한 **`byteCap` 6 MiB 또는 `entryCap` 20,000 — 먼저 닿는 쪽** `[C7-F7 재산정]`. 「50,000 엔트리 / 8 MB」는 `payload`가 없던 시절의 짝이며 지금은 서로 모순된다(평균 ≈280 B [INFERENCE] × 50,000 = 14 MB > 8 MB). 재산정 근거와 측정 과제는 `data-schemas/save.md` §3.2. 초과 시 가장 오래된 스냅샷 이전 구간을 **압축(스냅샷으로 접기)** 하고, 접힌 구간의 되돌림은 체크포인트 단위로 낮춘다. 33비트 × 예상 명령 수(1,000~4,000건)로는 상한에 닿지 않는다는 것은 여전히 **[INFERENCE]**이며 T0에서 실측한다.

## 7. Sim / Presentation 계약 (CLAUDE.md §9 불변식)

| 규칙 | 내용 |
|---|---|
| R1 | Presentation·UI는 `WorldSnapshot`만 받는다. 스냅샷은 불변이며 `seq`와 `stateHash`를 포함한다 |
| R2 | 렌더는 Sim에 쓰지 않는다. 사용자 조작은 **Intent → Command 제출** 경로만 존재한다 |
| R3 | `Time.deltaTime`은 보간·연출에만 쓴다. Sim에 시간이 들어가지 않는다(전역 실시간 타이머 없음) |
| R4 | 스냅샷의 `seq`가 UI가 마지막으로 본 `seq`보다 낮으면 UI는 **stale 처리**하고 확정 버튼을 비활성화한다 |
| R5 | Sim은 표시 문자열을 만들지 않는다. 사유는 `ReasonCode` enum이고 문자열은 로컬라이제이션 테이블에서 UI가 만든다 |

## 8. 세이브 호환 불변식 (하드)

1. 지속 필드의 **개명 금지**. 개명이 필요하면 마이그레이터를 먼저 작성하거나 거부한다(CLAUDE.md §9). 목록은 `data-schemas/save.md` §4이며 2026-09-10 R7에 `commitIdempotencyKey`·`commandLog.entries[].commandId`·`commandLog.entries[].payload` 3건이 편입됐다 `[C7-F7]`.
1-bis. **확정 트랜잭션마다 `commitIdempotencyKey`** 를 발급해 세이브에 기록한다. 재시도·지연 완료·rename 후 크래시에서 중복 적용을 막는 유일한 수단이며 `T-17`·`T-18`·`T-19`가 이것을 단언한다.
2. `schemaVersion`은 정수 단조 증가. 낮은 버전은 마이그레이터 체인으로 올린다.
3. **더 높거나 미등록 버전은 절대 열지 않고 절대 덮어쓰지 않는다.** 구버전 클라이언트가 신버전 세이브를 파괴하는 경로를 만들지 않는다.
4. 쓰기는 원자적: `save.tmp` 기록 → flush/fsync → 기존 정본을 `save.bak`으로 회전 → `rename(tmp, save.json)`.
5. 체크섬 실패 시 정본을 덮어쓰지 않고 읽기 전용 복구 패널로 간다.
6. 모든 마이그레이터는 **양방향이 아니라 단방향**이며, 마이그레이션 전 원본을 `save.v{n}.bak`으로 보존한다.
7. DLC 플래그가 없어도 본편 로드·결말 3종 도달이 성립해야 한다(`economy/resources-and-fairness.md`).

상세 필드는 `systems/data-schemas/save.md`, 절차는 `systems/system-specs/save-undo.md`.

## 9. 데이터 주도 규칙

- 밸런스·경제 수치는 **데이터 테이블에만** 산다. 코드는 노브를 노출할 뿐 값을 갖지 않는다.
- 튜닝 대상 필드는 스키마에서 `tunable: yes`로 표시하고, 소유 레인(balance / economy)을 명시한다.
- 코드에 숫자 리터럴이 필요한 경우는 (a) 스키마 버전, (b) 배열 인덱스, (c) 물리 상수 없음 — 그 외는 테이블 참조.
- 임포트 검증은 **fail-closed**. 경고로 통과시키지 않는다.

## 10. 성능 예산 배분 [TARGET] — 전부 NOT-MEASURED

프레임 예산 16.7 ms(60 fps @1920×1080) 기준 배분 가설:

| 구간 | 예산 | 근거 |
|---|---|---|
| Sim 갱신 | 2.0 ms | 이벤트 구동, 매 프레임 전체 재계산 없음 |
| 스냅샷 생성·디프 | 1.0 ms | 변경 필드만 |
| UI 레이아웃·바인딩 | 3.5 ms | 텍스트 밀도가 높음 |
| 렌더(디오라마) | 8.0 ms | 고정 시점, 동적 광원 제한 |
| 여유 | 2.2 ms | |

| 항목 | 목표 | 상태 |
|---|---|---|
| 명령 `Validate` | ≤ 8 ms | NOT-MEASURED |
| 명령 `Preview` | ≤ 16 ms | NOT-MEASURED |
| `Commit` + 체크포인트 | ≤ 200 ms | NOT-MEASURED |
| 되돌림 1스텝 | ≤ 16 ms | NOT-MEASURED |
| 세이브 쓰기(fsync 포함) | ≤ 200 ms | NOT-MEASURED |
| 세이브 로드 + 로그 재생 | ≤ 1500 ms | NOT-MEASURED |
| 구역 씬 전환 | ≤ 3000 ms | NOT-MEASURED |
| 시점 노드 전환 첫 프레임 | ≤ 100 ms | NOT-MEASURED |
| 기준 하드웨어 | **미정** | NOT-MEASURED |

기준 PC가 정의되기 전에는 통과/실패를 말할 수 없다. 목표는 프로파일 결과가 아니다.

## 11. 인수 기준

### 11.1 문서 단계 (D, 이번 회차에 판정 가능)

| id | 기준 |
|---|---|
| D-A1 | 8개 시스템 스펙 각각이 입력·상태기계·규칙·실패모드·스키마참조·텔레메트리·예산·인수기준 8절을 모두 갖는다 |
| D-A2 | 6개 데이터 스키마의 모든 필드가 타입과 `tunable` 표기를 갖는다 |
| D-A3 | 스펙이 참조하는 도구 id·구역 id가 `planning/campaign.json`의 실제 값과 일치한다 |
| D-A4 | 세이브 스키마의 어떤 필드도 이번 회차에 개명되지 않았다(신규 정의이므로 자명, 이후 회차부터 검사) |
| D-A5 | Sim이 참조하는 어셈블리 목록이 비어 있음이 문서로 명시된다 |

### 11.2 빌드 후 (B, C4 이후에만 판정 가능)

| id | 기준 | 검증 방법 |
|---|---|---|
| B-A1 | `Tide.Sim`이 `UnityEngine`을 참조하면 컴파일 실패 | asmdef `noEngineReferences` + 배치 빌드 |
| B-A2 | 같은 명령열 재생 시 `stateHash` 동일 | EditMode 테스트 |
| B-A3 | `Preview` 2회 호출 후 `stateHash` 불변 | EditMode 테스트 |
| B-A4 | 부분 기록 세이브 주입 시 정본 바이트 불변 | PlayMode 테스트 + 파일 해시 |
| B-A5 | 프레임 예산 위반 구간을 캡처로 특정 | 프레임타임 캡처(§tech-verification) |

## 12. 미해결 / 위험

| id | 내용 | 완화 |
|---|---|---|
| RISK-S1 | 기준 하드웨어 미정이라 모든 성능 목표가 판정 불가 | C4에서 기준 PC 1대 고정 후 캡처 |
| RISK-S2 | Input System 전환(`activeInputHandler: 0` → Both/New)이 미실행 | C4 슬라이스 첫 작업으로 배치 |
| RISK-S3 | 명령 로그 상한이 [INFERENCE]. 실제 명령 밀도·엔트리 바이트 미측정 | T0 슬라이스에서 `command_count`/분 + `save_file_bytes / entries_count` 수집 후 `entryCap` 재파생(`save.md` §3.2 · `save-undo.md` `B-SV9`). 세 키 모두 `ops/telemetry-contract.md` §4·§6에 정의됨(`command_count`는 2026-09-10 R7 신설 · C7-F11) |
| RISK-S4 | URP 채택 여부가 미결이라 에셋 예산(G5)이 두 갈래 | C4 연출 레인과 합동 결정 |

## 13. 이 회차에 하지 않은 것

- 코드 작성 0줄. Unity 에디터 실행 0회. 패키지 설치 0건. 빌드 0회.
- `mex` 실행 0회(저장소 규칙에 따라 시스템 레인 이번 회차 금지). 그래프 갱신 영수증 없음 → `[UNGRAPHED]`.
- 다른 레인 폴더·`archive/`·`production/`·`qa/` **읽기만** 했고 수정 0건.

## 14. RFC (디렉터 판정 요청)

append-only 기록은 `production/decision-log.md`가 소유한다. 본 레인은 그 파일을 편집하지 않고 여기에 초안을 남긴다.

### RFC-S1 — 직렬화 필드 표기 규약
| 항목 | 내용 |
|---|---|
| 영향 레인 | director, planner, balance, economy |
| 질문 | 직렬화 필드를 **camelCase**로 통일해도 되는가 (디렉터 지시는 `snake_case` 또는 `PascalCase` 두 선택지였다) |
| 제안 | camelCase 채택. C# 공개 멤버는 PascalCase, 변환은 첫 글자 소문자화 1:1 |
| 증거 | `planning/campaign.json`이 이미 camelCase (명령 `shasum -a 256` · `wc -c` · `node validate-campaign.mjs`. R3 관측 `fdabf1d4…`/120479 B, **R4 재실측 `92301c0a…`/121457 B** — 해시가 바뀌어도 표기 규약은 불변이며, 인용은 검증기 출력을 읽는다: RFC-Q1), `systems/unity-implementation.md` save v1도 camelCase |
| 대안 비용 | snake_case로 가면 33비트 저작 원본 전 키를 변환하는 계층이 하나 더 생기고, 이후 개명 금지 대상이 두 벌이 된다 |

### RFC-S2 — 어셈블리 경계 분할
| 항목 | 내용 |
|---|---|
| 영향 레인 | director, presentation, qa |
| 질문 | `Tide.Domain` 5분할(병행 초안) 대신 `Tide.Sim / Data / Save / Input / Presentation / UI / App` 7분할을 채택하는가 |
| 제안 | 7분할. 세이브 마이그레이션과 입력 포커스는 실패 모드와 테스트 경계가 다르다 |
| 증거 | 코드 0줄이므로 개명 비용 0. `systems/unity-implementation.md`가 c4→c5 draft로 갱신되는 동안에도 5분할 유지 [OBSERVED 2026-09-10] |
| **판정** | **승인 — 7분할** (`production/decision-log.md` 「RFC-S2 / C7-F4 · asmdef 분할」 · 2026-09-10). 반영: 본 문서 §2(+`Tide.EditorTools` 등재) · `unity-implementation.md` §2 · `handoff/codex-unity-brief.md` §③·DoD 1 · `handoff/README.md` §4 |

### RFC-S3 — 세이브의 `dayIndex` 제거
| 항목 | 내용 |
|---|---|
| 영향 레인 | director, worldview, planner |
| 질문 | 세이브 스키마에서 `chapter` / `dayIndex`를 빼고 `stageId` + `storyClock`을 쓰는가 |
| 제안 | 제거. 캐논이 **단일 야간**(21:00→05:00)이므로 `dayIndex`는 항상 0인 죽은 필드다 |
| 증거 | `worldview/timeline.md` 장별 시각 매핑 [OBSERVED]; `systems/unity-implementation.md` 저장 스키마의 `"chapter": 3, "dayIndex": 2` [OBSERVED] |
| 긴급도 | **출시 후에는 개명 금지 대상**이 된다. 지금 비용 0, 나중 비용은 마이그레이터 1개 |
| **판정** | **승인 — 제거** (`production/decision-log.md` 「RFC-S3 · 세이브 `dayIndex`」 · 2026-09-10): 「캐논은 단일 야간이므로 `dayIndex`·`chapter` 는 저장하지 않는다. 스테이지 진행은 `storyPhase` 로 충분. **마이그레이션 대상 아님**(v1 이전에 제거)」. 반영: `data-schemas/save.md` §2 · `handoff/codex-unity-brief.md` §⑥-5 · `handoff/README.md` §4 |

### RFC-S4 — 저작 원본 해시 드리프트 · **종결 (RFC-P3-008)**
| 항목 | 내용 |
|---|---|
| 영향 레인 | planner, qa, director |
| 원 질문 | `planning/campaign.meta.md`의 sha256이 현재 `campaign.json`과 다르다. 누가 언제 갱신하는가 |
| 판정 | **RFC-P3-008이 계보 B(live `planning/campaign.json`)를 유일 정본으로 확정**했고, planner가 `planning/validate-campaign.mjs`를 두어 해시·집계를 실행 시점에 재계산해 출력하게 했다. 따라서 손으로 적은 해시를 신뢰하는 구조가 사라졌다 |
| 종결 영수증 [OBSERVED 2026-09-10 R3] | `shasum -a 256 …/campaign.json` → `fdabf1d421932d679429870a06eeb31e9527562370c6a107aec77c2c884fb2b7` / `wc -c` → 120479 / `node …/validate-campaign.mjs` → 44 검사 44 PASS 0 FAIL |
| 재확인 [OBSERVED 2026-09-10 R4] | 같은 명령 재실행 → sha256 `92301c0a5ecfc7e142646f5f17c29c21609e2b2de7c90408de7bffc9d815ae23` / 121457 bytes / **47 검사 47 PASS 0 FAIL**(`Z-01`·`Z-02`·`K-06` 추가). 값이 바뀐 것은 planner 가 비트 `zoneId`(C3-F22)를 넣었기 때문이며, **바로 이 변동이 RFC-S4 의 종결 근거**다 — 문서가 해시를 손으로 들고 있지 않으므로 저작 원본이 바뀌어도 스테일이 생기지 않는다. 인용 규칙은 RFC-Q1(검증기 출력을 읽는다, 고정 숫자 재기재 금지) |
| 잔여 규칙 | 이후 이 저장소의 **어떤 문서도 축약 해시(8자리)를 [OBSERVED]로 쓰지 않는다.** 전체 64자리를 적거나 검증기 출력을 인용한다 (C3-F2 재발 방지) |

### 병행 편집 관측 (RFC 아님, 사실 보고)
작업 중 `_workspace/current/systems/`의 `interaction-rules.md` · `unity-implementation.md` · `game-ui-contract.meta.md`가 cycle `c4` draft → `c5` draft로 갱신되고 `_workspace/archive/20260909-preproduction-c{3,4,5-prep}/`가 새로 생기는 것을 관측했다 [OBSERVED 2026-09-10]. 다른 세션이 같은 워크스페이스에서 앞선 회차를 돌고 있다. 본 레인은 그 파일들을 **읽기만** 했고 수정·이동·삭제 0건이다. C3 산출물이 C5 진행분과 어떤 순서로 놓이는지는 디렉터가 판정해야 한다.

## C1-b2 runtime addendum · RFC-CX-005 · 2026-09-11

[OBSERVED] `C1SignatureData` validates the canonical packet; `C1SignatureDefinition` owns deterministic commands and readiness; `C1SignatureGameSession` adapts commands to the existing accepted-save transaction; `SignaturePaperView` reads presentation flags and never writes simulation state. `T0Interface` owns scroll and focus state only. No assembly boundary changes are required.

[OBSERVED] This increment uses save schema v3. T0/patrol command IDs and persisted names remain unchanged; versioned backups preserve original v1/v2 bytes before replay migration. Future versions reject before fallback. The signature command/data contract is `systems/data-schemas/c1-signature-runtime.md`, its state rules are `systems/system-specs/c1-signature-runtime.md`, and measured native/test evidence is `systems/tech-verification/c1-m4-native.md`.
