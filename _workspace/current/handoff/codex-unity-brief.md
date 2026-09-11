---
updated: 2026-09-10
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
---

# Codex(GPT-6 Astra) Unity 착수 브리프 — T0 수직 슬라이스

> 이 문서 하나로 착수할 수 있게 쓴다. **여기에 없는 결정은 추측하지 말고 `handoff/README.md` §2 의 RFC 형식으로 되묻는다.**
> 브리프와 소유 문서가 어긋나면 **소유 문서가 이긴다**(`systems/architecture-contract.md` · `systems/unity-implementation.md` · `systems/interaction-rules.md` · `systems/system-specs/*` · `systems/data-schemas/*` · `systems/ops/telemetry-contract.md`).
> 정직성 표기: `[OBSERVED]` = 이 저장소에서 명령으로 확인한 것 · `[TARGET]` = 설계 목표 · `[INFERENCE]` = 추론 · `[CARRIED]` = 이전 회차 값 이월.
> **빌드 0회 · 플레이 표본 n=0 · 프레임타임 캡처 0건.** 이 문서의 어떤 문장도 게임이 존재한다는 증거가 아니다.

---

## ① 목표 — T0 수직 슬라이스 하나. 본 생산이 아니다

| 항목 | 값 | 출처 |
|---|---|---|
| 만드는 것 | **T0 수직 슬라이스**: 구역 `hub` 1개 + 도구 `circuit`·`reader` 2개 | `unity-implementation.md` §10 |
| 설계 길이 | **25분**(`campaign.json` `T0.minutes = 25`) [OBSERVED · 검증기 `aggregates.stageMinutes[0]`] | 검증기 출력 |
| 대상 비트 | `t0-b1`(탐색 5분) · `t0-b2`(퍼즐 10분 · `circuit` 안내 도입) · `t0-b3`(퍼즐 10분 · `reader` 안내 도입 + `circuit` 재사용 · **`proofRequired: true`**) [OBSERVED] | `planning/campaign.json` |
| **하지 않는 것** | `alignment` · `routing` · `corrosion` · `seal` 구현, `gate`/`lowland`/`dock`/`pump` 4구역, 33비트 전체, 엔딩 3종, DLC, 스토어 빌드 | `unity-implementation.md` §10 |
| 다음 단계 | **본 생산 착수 조건의 정본은 `production/premium-preproduction-contract.md` 「## Base production gate」 한 절뿐이다** `[C6-F9 · 2026-09-10 R7 종료]`. 이 브리프는 그 절을 **인용만** 하고 조건을 다시 쓰지 않는다 — 네 조건(T0 사람 검증 H-1~H-3 · 조위정합 스파이크 개념 검증 · **T0 실제 소요가 슬라이스 견적의 150% 초과 시 STOP** · 열린 S1 0 · G8 exit 0)이 **모두** 파일로 증명될 때만 본 생산이 시작된다 | 계약 「Base production gate」 |

**T0 25분은 8시간을 증명하지 않는다.** 슬라이스 측정치로 본편 480분 완주를 주장하지 않으며 `observedMedianMinutes` 는 계속 `null` 이다.

### ①-1 현재 프로젝트 상태 [OBSERVED 2026-09-10]

| 항목 | 값 | 확인 명령 |
|---|---|---|
| 에디터 버전 | `6000.5.6f1 (0e0577a1a2ac)` | `cat unity/Unknown/ProjectSettings/ProjectVersion.txt` |
| `Assets/` | 파일 0개 · 하위 폴더 0개 | `find unity/Unknown/Assets -type f` |
| 패키지 | `com.unity.multiplayer.center 1.0.1` + 내장 모듈 34종 | `cat unity/Unknown/Packages/manifest.json` |
| 잠금 파일 | `Packages/packages-lock.json` **존재**(7.3 KB) — 프로젝트가 최소 1회 열려 resolve 됐다는 뜻 | `ls unity/Unknown/Packages/` |
| 입력 | `activeInputHandler: 0` = **레거시 Input Manager** | `grep activeInputHandler ProjectSettings/ProjectSettings.asset` |
| 렌더 | `m_CustomRenderPipeline: {fileID: 0}` = **내장 파이프라인** | `grep m_CustomRenderPipeline ProjectSettings/GraphicsSettings.asset` |
| 프로젝트 열림 기록 | `Logs/Editor.log` 머리 = `Unity Editor version: 6000.5.6f1`, `COMMAND LINE ARGUMENTS` = `-projectpath … -useHub -hubIPC` (2026-09-10 05:39) | `sed -n '1,26p' unity/Unknown/Logs/Editor.log` |
| **배치모드 실행 기록** | `unity/Unknown/Logs/` 안에는 **0파일** — `grep -l batchmode unity/Unknown/Logs/*.log` = **0** [OBSERVED 2026-09-10]. 배치 로그는 그 폴더가 아니라 `production/receipts/unity-batchmode/` 에 보존돼 있다 | 같은 명령 + `ls _workspace/current/production/receipts/unity-batchmode/` |
| **헤드리스 열기** | **확인됨(2차 실행)** [OBSERVED] — `production/receipts/unity-batchmode/README.meta.md` 「재실행」 절 · `open-validate-2.full.log` 에 `Exiting batchmode successfully now!` **1건** | `grep -c "Exiting batchmode successfully" _workspace/current/production/receipts/unity-batchmode/open-validate-2.full.log` |

> **정정 고지 `[C7 · 2026-09-10 R7 종료]`**: 이 자리의 이전 문장은 「`-batchmode` 실행은 아직 0회」였다. **틀렸다.** 정본은 `production/receipts/unity-batchmode/README.meta.md` 이며 그 문서가 세 실행을 구분한다 — 프로젝트 **생성** 성공(`create.full.log`), 열기 검증 **1차 미확인**(`open-validate.full.log` 34행에서 끝나고 성공 문구 **0건** [OBSERVED]), 열기 검증 **2차 성공**(`open-validate-2.full.log`, 성공 문구 1건 [OBSERVED]). 「헤드리스 열림」을 증명하는 것은 **2차뿐**이다.
> 여전히 참인 구분: 열림이 확인됐다는 것은 **프로젝트가 열린다**만 뜻한다 — 코드 0줄 · 테스트 0건 · 성능 캡처 0건이며, 실행자의 §⑩ 실행은 그것과 **다른 주장**을 만드는 첫 실행이다. 로그는 `systems/tech-verification/logs/` 에 남긴다.

---

## ② 저장소 규칙 요약 (CLAUDE.md §8 · §9 · §10.1) — 위반은 되돌리기 비싼 것들

| # | 규칙 | 실행자에게 의미하는 것 |
|---|---|---|
| G1 | **커밋·푸시는 사용자가 한다** | 코드를 작성하되 `git commit` / `git push` 를 실행하지 않는다. push-ready 상태(빌드 가능 · `.gitignore` 정비 · 검증 영수증)까지 만든다 |
| G2 | **명시적 pathspec 스테이징** | `git add -A` / `git add .` 금지. 스테이징이 필요하면 파일을 하나씩 적는다. force-push 금지, 다른 세션 변경 되돌리기 금지 |
| G3 | 편집 전·후 `git status --short` | 예상치 못한 변경은 **다른 세션의 작업**으로 취급하고 건드리지 않는다 |
| G4 | **sim/render 분리** | 렌더·연출·UI 는 시뮬레이션 **스냅샷을 읽되 시뮬레이션 상태를 쓰지 않는다**. asmdef 로 강제한다(§③) |
| G5 | **데이터 테이블 튜닝** | 밸런스·경제 숫자는 데이터에만 산다. 코드는 노브를 노출할 뿐 값을 갖지 않는다. 숫자 리터럴이 허용되는 곳은 스키마 버전·배열 인덱스뿐 |
| G6 | **세이브 필드명 불변** | 지속 필드 개명은 플레이어 세이브를 고아로 만든다. 마이그레이터 없이 개명 금지(개명 금지 목록 = `data-schemas/save.md` §4) |
| G7 | **에셋 승격 감사** | 생성·구매 에셋은 `provenance.json` + `runtimeEligible:false` 로 시작한다. `true` 승격은 decision-log 감사로만 |
| G8 | **가제 문자열 금지** | 상표·동명 확인 전 가제("조수기록국" / "TIDE ARCHIVE")를 **폴더명·번들명·상점명·이미지 내 텍스트**에 쓰지 않는다. 코드네임은 저장소명 `Unknown` 이고 `productName` 은 그대로 둔다 |
| G9 | 무시 대상 | `Library/ Temp/ Logs/ obj/ UserSettings/` 는 `.gitignore`(이미 존재). 새로 생기는 산출물(`*.csproj`, `*.sln`, `Builds/`)도 추가한다 |
| G10 | 유료 의존 0건 | 유료 에셋·외주·외부 API·네트워크·분석 SDK 를 추가하지 않는다. 텔레메트리는 **로컬 파일만** |

---

## ③ 폴더 · asmdef 레이아웃 — `systems/architecture-contract.md` §2·§3 인용

실제 경로는 `unity/Unknown/Assets/_Project/` 아래다.

```
unity/Unknown/Assets/_Project/
  Sim/            asmdef Tide.Sim         noEngineReferences: true, references: []
    State/        PuzzleState, ZoneState, RecordState, SealState
    Commands/     ICommand, Validate / Preview / Commit
    Log/          CommandLog, BranchId, Snapshotter
    Rules/        Law1..Law6 판정기 (법 = 클래스 1개씩)
  Data/           asmdef Tide.Data        저작본 → Sim DTO, 임포트 검증(fail-closed)
    Authoring/    ScriptableObject 저작본 — Zones/ · Records/ · Tools/
    Tables/       beats.json · hints.json · tables-receipt.json   ← **생성기 출력. 손으로 만들지 않는다**
    Import/       ReceiptVerifier, RuntimeInvariants, DTO 매퍼
  Save/           asmdef Tide.Save        Writer, Reader, Migrations/, Recovery
  Input/          asmdef Tide.Input       Actions.inputactions, FocusModel, DeviceGlyphs
  Presentation/   asmdef Tide.Presentation SnapshotBinder, Cameras, Fx
  UI/             asmdef Tide.UI          Screens/, Panels/, Bindings/
  App/            asmdef Tide.App         Bootstrap, SceneRouter, Settings
  Editor/         asmdef Tide.EditorTools  PackageBootstrap, EmitTablesMenu  (Editor 전용)
  Localization/   ko.csv, en.csv          (키만. 문자열을 코드·아트에 굽지 않는다)
  Art/            Meshes/ · Textures/ · UI/ · Portraits/   ← 승격된 에셋의 **유일한** 자리
  Scenes/         boot.unity, ui-root.unity, hub.unity     ← T0 는 이 3개만
  Tests/
    EditMode/     asmdef Tide.Tests.Sim   (Sim·Data·Save 참조, 엔진 최소)
    PlayMode/     asmdef Tide.Tests.Play
```

> **`[C7-F6]` 이 트리는 2026-09-10 R7에 세 곳을 고쳤다.**
> (1) `Tables/` 에서 `zones.json`·`plates.json`·`tools.json` 을 **뺐다** — 이 셋은 §④-1 대로 **ScriptableObject** 이며 `Authoring/` 에 산다(형태 근거표는 `data-schemas/plates.md` §0-1). 이전 판은 같은 문서 안에서 JSON 과 SO 두 말을 했다.
> (2) `Scenes/` 와 `Art/` 를 **`_Project/` 안으로 옮겼다.** 이전 판은 `Assets/Scenes/` 를 `_Project/` 밖에 두면서 바로 아래 줄에 "`Assets/_Project/` 밖에는 서드파티만"이라고 적었고(자기모순), `asset-runbook.md` §3.2 는 승격 경로를 `Assets/Art/**` 로 적어(제3의 위치) 셋이 어긋나 있었다. 지금 옮기는 비용은 **0** — `Assets/` 는 파일 0개다 [OBSERVED].
> (3) `Editor/` 를 명시했다 — §③-1 의 패키지 부트스트랩이 살 자리다.

**허용 의존 방향(단방향, asmdef 로 강제)**

```
Tide.App ──> Tide.UI ──> Tide.Presentation ──┐
   │           │                             ├──> Tide.Sim   (읽기 전용 스냅샷)
   ├──> Tide.Input ──────────────────────────┘
   ├──> Tide.Save ──> Tide.Sim
   └──> Tide.Data ──> Tide.Sim
```

- `Tide.Sim` 의 asmdef `references` 는 **비어 있어야** 하고 `noEngineReferences: true` 다. `UnityEngine` · `Time` · `Random` · `Application` · 파일 I/O · 문화권 의존 파싱 금지.
- 역방향 참조(Sim → Presentation 등)는 **컴파일 실패**여야 한다. 문서 규칙이 아니라 빌드 규칙이다(인수 테스트 `T-B1`).
- `Assets/_Project/` 밖에는 서드파티·패키지 샘플만 둔다. **예외 0건** — 씬·아트·에디터 도구까지 전부 `_Project/` 안이다(위 트리 · C7-F6). 이 규칙과 어긋나는 경로를 만들려면 `architecture-contract.md` §3을 먼저 고친다.
- 씬은 **가산 로드**: `boot` → `ui-root` + 구역 씬 (`Assets/_Project/Scenes/`). T0 는 구역 씬이 `hub` 하나다(`campaign.json` `T0.zoneIds = ["hub"]` [OBSERVED]).
- **RFC-S2 판정됨 — 7분할이 정본** `[C7-F4 · 2026-09-10 R7 종료]`: `production/decision-log.md` 「RFC-S2 / C7-F4 · asmdef 분할」 = 「**7분할(`architecture-contract.md` §2, status current)이 정본**. `unity-implementation.md` §2 의 5분할 서술을 7분할로 정정(systems)」. 그 정정은 **완료됐다** — `unity-implementation.md` §2 는 이제 `Tide.Sim / Data / Save / Input / Presentation / UI / App`(+ `Tests`, `EditorTools`)를 적는다. 되묻지 않고 위 트리대로 만든다.
- 생산 어셈블리 **7개** = `Tide.Sim` · `Tide.Data` · `Tide.Save` · `Tide.Input` · `Tide.Presentation` · `Tide.UI` · `Tide.App`. 여기에 테스트 2종(`Tide.Tests.Sim` · `Tide.Tests.Play`)과 에디터 전용 `Tide.EditorTools` 가 더해져 asmdef 파일은 **10개**다 — "7분할"은 **생산 어셈블리 수**를 가리키는 말이며 파일 수가 아니다(DoD 1 도 이 뜻으로 읽는다).

### ③-1 패키지 — 왜 필요한지와 함께. **버전은 발명하지 않는다**

| 패키지 | 필수/선택 | 이유 | 버전 |
|---|---|---|---|
| `com.unity.inputsystem` | **필수** | 키보드·마우스·패드 동시 지원과 **런타임 리바인딩**이 접근성 계약이다(`gdd.md` §8). 현재 `activeInputHandler: 0` 이라 전환 작업이 실재한다 | `[PIN-AFTER-RESOLVE]` |
| `com.unity.ugui` (TextMeshPro 포함) | **필수** | 판독·서명 UI 의 텍스트 밀도가 높고 한글 글리프 폰트 대체가 필요하다 | `[PIN-AFTER-RESOLVE]` |
| `com.unity.localization` | **필수** | KO/EN 2종이 범위. **미해결 키 0건**(T-12)을 테이블 없이 기계 검사할 수 없다 | `[PIN-AFTER-RESOLVE]` |
| `com.unity.test-framework` | **필수** | 배치모드 인수 테스트(§⑩) 없이는 G6 입력이 0이다 | `[PIN-AFTER-RESOLVE]` |
| `com.unity.render-pipelines.universal` | **필수 — 채택 확정** `[C6-F11 ③ · 2026-09-10 R7 종료]` | **URP 로 판정됐다**(`decision-log.md` 「C6-F11 … T0 착수 전 결정 4건」 ③): 2.5D 고정 시점이라 라이트 프로브가 필요 없고 포스트프로세싱은 최소다. 이전 판의 「선택(권장) · 채택 판단을 미뤄도 된다」는 **철회**한다 — 미룬 채로 셸을 만들면 파이프라인 전환 비용이 나중에 붙는다 | `[PIN-AFTER-RESOLVE]` |
| `com.unity.cinemachine` | **선택** | 고정 시점 노드 간 컷/짧은 돌리. 노드 10개 이하면 수동 카메라로 충분 | `[PIN-AFTER-RESOLVE]` |
| `com.unity.addressables` | **선택 — 단, 전이 의존으로 들어올 수 있다** | 구역 단위 로드. 씬 3개 규모에서는 `SceneManager` 가산 로드로 대체. `com.unity.localization` 이 요구하므로 `packages-lock.json` 에 자동으로 나타날 수 있다 `[INFERENCE]` — **그 등장은 N-10 위반이 아니다**(§⑩-4 예외). API 를 직접 쓰는 것은 여전히 RFC 대상 | `[PIN-AFTER-RESOLVE]` |
| `com.unity.multiplayer.center` | **제거** | 멀티플레이 없음. 신규 템플릿 잔재 | 현재 `1.0.1` [OBSERVED] |

**`[PIN-AFTER-RESOLVE]` 처리 절차**: 패키지를 추가하고 에디터가 resolve 한 뒤, `Packages/manifest.json` 과 `packages-lock.json` 의 **실제 값을 그대로** `systems/architecture-contract.md` §4 와 `systems/unity-implementation.md` §1 에 옮겨 적는다. 지금 숫자를 쓰면 그것은 발명이다. **선택 패키지를 추가하기 전에 RFC 로 되묻는다**(§⑩ 금지 목록).

#### ③-1a 필수 패키지 **추가 절차** — 0/5 설치 상태에서 시작한다 `[C7-F2 신설 2026-09-10 R7 · C6-F11 ③ 로 URP 추가]`

**현재 0/5 다** [OBSERVED]: `manifest.json` 의 비-모듈 의존은 `com.unity.multiplayer.center` **하나뿐**이고 `packages-lock.json` 도 같다. 즉 **`-runTests` 는 `com.unity.test-framework` 없이 실행되지 않는다.** 이전 판의 §⑩-2 는 #1(프로젝트 열기) 다음에 곧바로 #2/#3(`-runTests`)을 시켰고, 그 사이에 패키지가 어떻게 생기는지를 **적지 않았다**. 여기서 메운다.

**버전을 발명하지 않으면서 추가하는 방법은 하나다 — 버전 미지정 `Client.Add`.** `manifest.json` 은 버전 문자열을 요구하므로 손으로 적으면 그것이 곧 발명이다. Package Manager API 는 버전 없이 이름만 주면 **에디터가 호환 버전을 골라** `packages-lock.json` 에 박아 준다.

1. `Assets/_Project/Editor/PackageBootstrap.cs` 를 만든다 (Editor 전용 · 내장 모듈만으로 컴파일된다):

```csharp
// Assets/_Project/Editor/PackageBootstrap.cs  — Editor 전용. 한 번 쓰고 남겨 둔다(재현용).
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;

public static class PackageBootstrap {
    static readonly string[] Add = {
        "com.unity.inputsystem", "com.unity.ugui",
        "com.unity.localization", "com.unity.test-framework",
        "com.unity.render-pipelines.universal",   // C6-F11 ③ 로 채택 확정
    };                                    // 버전 미지정 = 에디터가 고른다
    static readonly string[] Remove = { "com.unity.multiplayer.center" };

    public static void Run() {
        Request req = Client.AddAndRemove(Add, Remove);
        while (req.Status == StatusCode.InProgress) System.Threading.Thread.Sleep(100);
        if (req.Status != StatusCode.Success) {
            UnityEngine.Debug.LogError($"PackageBootstrap FAILED: {req.Error?.message}");
            EditorApplication.Exit(1);           // 실패를 조용히 넘기지 않는다
        }
        AssetDatabase.Refresh();
        EditorApplication.Exit(0);
    }
}
```

2. `§⑩-2` **#0** 으로 실행한다(그 절에 명령을 추가했다).
3. 결과를 **`packages-lock.json` 에서 읽어** `[PIN-AFTER-RESOLVE]` 자리에 옮긴다.

**인수 조건**: 실행 후 `packages-lock.json` 의 **최상위 `dependencies` 키**에 위 5개 id 가 존재하고 `com.unity.multiplayer.center` 가 사라진다. 5/5 가 아니면 **다음 단계로 가지 않는다** — `-runTests` 가 조용히 아무 것도 안 하고 성공처럼 끝나는 것이 가장 나쁜 결과다.

> **세는 방법 정정 `[C7-F33 해소 2026-09-10 R7 종료]`**: 이전 판의 게이트는 `grep -c '"com.unity.inputsystem"\|…' packages-lock.json` 이었다. `packages-lock.json` 은 **중첩 `dependencies` 안에서 같은 id 를 반복**하므로 그 명령은 **행 수를 과다 계수**한다 — 현행 파일에서도 `com.unity.modules.unitywebrequest` 가 **7회**, `com.unity.modules.physics`·`com.unity.modules.jsonserialize` 가 **6회** 등장한다 [OBSERVED 2026-09-10, `grep -o '"com\.unity\.[a-z0-9.-]*"' packages-lock.json | sort | uniq -c | sort -rn`]. 성공을 실패로도, 실패를 성공으로도 읽을 수 있으므로 **최상위 키만 세는 명령**으로 바꿨다(§⑩-2 #0).

**`[INFERENCE]` 경고 — 이 절은 실행되지 않았다**: `Client.AddAndRemove` 를 `-batchmode -executeMethod` 안에서 동기 대기시키는 위 형태는 **이 저장소에서 한 번도 돌려 본 적이 없다**(배치모드 실행 기록 0건, §①-1). 요청이 배치모드에서 진행되지 않고 멈추면 **폴백**: Unity Hub 로 프로젝트를 한 번 GUI 로 열고 `Window > Package Manager` 에서 같은 4종을 추가한 뒤 `packages-lock.json` 값을 옮긴다. 어느 경로를 썼는지 `systems/tech-verification/t0-*.md` 에 적는다.

**`com.unity.addressables` 는 전이 의존으로 들어올 수 있다 — 그것은 N-10 위반이 아니다.** `com.unity.localization` 이 Addressables 를 요구하므로 위 5종만 추가해도 `packages-lock.json` 에 `com.unity.addressables` 가 나타날 수 있다 `[INFERENCE]`. §⑩-4 N-10 에 이 예외를 등재했다. 전이로 들어온 Addressables 를 **직접 API 로 쓰기 시작하는 것**은 여전히 별도 RFC 대상이다 — 의존으로 존재하는 것과 채택하는 것은 다르다.

---

## ④ 데이터 — 스키마 6종 → Unity 표현 매핑과 임포터

### ④-1 매핑표

> **T0 의 실제 값은 이미 저장소에 있다** `[C7-F1 해소 2026-09-10 R7 종료]`. `_workspace/current/systems/data/t0/` 의 **`beats.json` · `hints.json` · `tools.json` · `zones.json` · `records.json`**(+ `tables-receipt.json` + 각 `.meta.md`)가 T0 인스턴스 데이터이며, 전부 `emit-tables.mjs --out _workspace/current/systems/data/t0` 의 출력이다. **손으로 만들지도, 손으로 고치지도 않는다** — 값이 틀렸으면 저작 문서를 고치고 생성기를 다시 돌린다. 값의 출처는 각 필드 옆 **`_src`** 가 갖고, 생성 명령·대조 결과는 `systems/tech-verification/r7-t0-data.md` 가 갖는다.
> 따라서 아래 표의 "저작 주체" 열이 말하는 것은 **누가 원문을 소유하는가**이고, 실행자가 실제로 임포트하는 파일은 `systems/data/t0/*.json` 이다. DoD 4 가 요구하는 값의 발명은 이제 필요 없다.

| 스키마 문서 | 런타임 표현 | T0 입력 파일(실행자가 읽는 것) | 최종 파일 위치 | 저작 주체 | 비고 |
|---|---|---|---|---|---|
| `data-schemas/zones.md` | **ScriptableObject** `ZoneAsset` + `ViewNode`/`CoverageArea`/`DrainEdge`/`Valve` 내부 클래스 | **`systems/data/t0/zones.json`** (1행 = `hub`, `viewNodes` 6) | `Data/Authoring/Zones/*.asset` | systems(파생) + modeling(치수) + concept(카메라) | `cameraPose` 는 **그레이박스 좌표계(Blender Z-up, m)**. Unity 변환은 `poseFrame.unityConversion` 의 `[INFERENCE]` 이며 임포트 실측(`T-Z1`)이 확인한다 |
| `data-schemas/plates.md` | **ScriptableObject** `RecordAsset`(`Segment[]`, `CorrosionPattern[]`) | **`systems/data/t0/records.json`** (5행) | `Data/Authoring/Records/*.asset` | synopsis(내용) + systems(형태) + planner(id) | `resolutionMinutes: 4` · `ringHours: 12` 는 **캐논 상수**. 매체 종류 필드는 **`sourceType`**(`mediaType` 은 R7 에 개명 · C6-F13). 곡선 180샘플·대장 136칸은 **규칙에서 생성**되며 문서 앵커와 33/33·28/28 대조된다 |
| `data-schemas/tools.md` | **ScriptableObject** `ToolAsset`(`CommandDef[]`) | **`systems/data/t0/tools.json`** (2행 + 스텁 4) | `Data/Authoring/Tools/*.asset` | systems | T0 는 `circuit`·`reader` 2개만 실제 채우고 4개는 스텁(§⑤-4). **`reader` 는 확정 명령을 갖는다**(인용 고정 · RFC-C7-001) |
| `data-schemas/beats.md` | **JSON** — 생성기 출력 | T0: **`systems/data/t0/beats.json`**(3행 + **완료 술어**) · 전편: `Data/Tables/beats.json`(저작 원본의 **바이트 동일 사본**) | `Data/Tables/beats.json` ← `emit-tables.mjs` | planner(저작) / systems(생성기) | **손으로 만들지 않는다.** 전편 사본은 `sha256(beats.json) == sha256(campaign.json)` [OBSERVED]. T0 판은 서브셋 + `completionPredicate`(§⑤-6) |
| `data-schemas/hints.md` | **JSON** — 생성기 출력(파생 투영) | T0: **`systems/data/t0/hints.json`**(9행) · 전편: `Data/Tables/hints.json`(99행) | `Data/Tables/hints.json` ← `emit-tables.mjs` | balance + planner(저작) / systems(생성기) | `idleHintOfferSeconds: 180` · `hintCost: 0`(0 아니면 임포트 실패) |
| — | **JSON** — 생성 영수증 | `systems/data/t0/tables-receipt.json` · `Data/Tables/tables-receipt.json` | 같은 폴더 | systems | 임포터가 대조하는 해시·판정·계약이 여기 있다(§④-2) |

> **형태가 갈리는 근거**: `zones`·`plates`·`tools` 는 필드가 Unity 에셋·프리팹·트랜스폼을 가리키므로 ScriptableObject, `beats`·`hints` 는 저작 원본이 `planning/campaign.json` 하나이고 엔진 참조가 없으므로 JSON 이다. 표 전문은 `data-schemas/plates.md` §0-1 [C7-F6].
| `data-schemas/save.md` | **런타임 직렬화 대상**(저작본 아님) | 세이브 파일(§⑥) | systems | 필드 개명 금지 목록이 걸려 있다 |

- **표기 규약**: 직렬화 키는 **camelCase**(RFC-S1 제안 · `campaign.json` 이 이미 camelCase [OBSERVED]). C# 공개 멤버는 PascalCase, 변환은 첫 글자 소문자화 1:1.
- **튜닝 노브(코드 하드코딩 금지)** — `data-schemas/tools.md` §4: `commitHoldSeconds` 0.4 [balance] · `readBudget` 3 [balance] · `residualLimitMinutes` 4 [balance] · `corrosionLimit` 9 [economy] · `routeCorrosionCost[*]` lowland 7 · dock 8 · dock-express 12 [economy] · `systemLimits[*]` `null`(**표시 전용, 차단 금지**) · `idleHintOfferSeconds` 180 [balance] · `hintCost` 0 [economy].

### ④-2 임포터 — 규칙을 재구현하지 **않는다**. 영수증을 대조한다 `[C6-F13 재작성 2026-09-10 R7]`

**이전 판은 여기서 "그 규칙을 C# 으로 다시 구현하고"라고 지시했다. 그 지시를 철회한다.** 규칙이 두 벌이 되는 순간 어느 쪽이 정본인지 말할 수 없고, 저작 원본이 바뀔 때마다 두 구현이 조용히 갈라진다.

**파이프라인 (정본: `data-schemas/beats.md` §1-2)**

```
planning/campaign.json ─▶ planning/validate-campaign.mjs   ← 규칙의 유일한 출처(planner 소유)
                              verdict != PASS → 여기서 멈춤 (exit 1, 출력 0건)
                         ─▶ systems/pipeline/emit-tables.mjs (systems 소유, 검증기를 호출만)
                              Data/Tables/{beats.json, hints.json, tables-receipt.json}
                         ─▶ Unity 임포터: 영수증 대조 + 런타임 전용 불변식만
```

**실행 영수증 [OBSERVED 2026-09-10 · systems 레인이 실제로 돌림]**: 검증기 `verdict PASS · fail 0`(검사 수·sha·바이트는 **출력이 보유한다** — 옮겨 적지 않는다, RFC-Q1) · `beats.json` `derivation: copy` 이고 **저작 원본과 sha256 동일** · `hints.json` `projection` 99행 · `t0-b1.zoneId` 를 스테이지 밖으로 바꾼 픽스처에서 **exit 1 · 출력 디렉터리 미생성**. T0 스코프 실행의 명령·출력 원문·대조 결과는 **`systems/tech-verification/r7-t0-data.md`** 가 보유한다.

**임포터가 반드시 하는 것**

| id | 검사 |
|---|---|
| `V-1` | `tables-receipt.json` 의 `validator.verdict == "PASS"` |
| `V-2` | `sha256(Data/Tables/beats.json)` == 영수증 `tables[beats.json].sha256` |
| `V-3` | `sha256(Data/Tables/hints.json)` == 영수증 `tables[hints.json].sha256` |
| `V-4` | 영수증의 `emittedUtc` 는 **대조 대상이 아니다**(매 실행 바뀐다). 그 외 필드는 전부 대조 |

**임포터가 재구현하는 것 — 런타임 전용 4건뿐**

| id | 규칙 | 왜 저작 시점에 못 하는가 |
|---|---|---|
| `R-1` | 로컬라이즈 미해결 키 0건(`T-12`) — **KO 필수 · EN 선택**(RFC-S6) | KO 테이블이 저작 JSON 밖에 있다. **EN 부재는 실패가 아니다**(KO 폴백). 실패 조건은 ① KO 키가 없거나 ② 그 KO 명사가 `worldview/glossary.md` 에 미등재일 때. [OPEN-S7 은 이 완화로 "EN 회차 과제"로 내려간다] |
| `R-2` | 고아 0건 | Unity 에셋 참조 그래프가 있어야 판정된다 |
| `R-3` | 독립쌍 중 **파괴 불가 경로 ≥ 1** | 런타임 규칙(`beats.md` `B-I11` 의 미검증 절반) |
| `R-4` | 임의 도달 상태에서 엔딩 3종 도달 | 런타임 상태공간 탐색(`beats.md` `B-I12`) |

**임포터가 하지 않는 것**: 아래 I-1~I-8(저작 시점 검사). 검증기가 이미 했고 영수증이 그 판정을 옮긴다. 참고용으로 남기되 **C# 으로 다시 쓰지 않는다**.

현행 입력은 **검증기 출력을 읽어서 안다**(고정 숫자 재기재 금지 · RFC-Q1·RFC-B6):
```
$ node _workspace/current/planning/validate-campaign.mjs        # summary · aggregates · sha256
$ node _workspace/current/planning/validate-campaign.mjs --pairs # C-07 독립쌍(비트별 루트·매체)
```
읽을 필드: `summary.{checks,pass,fail,verdict}` · `aggregates.{stages,beats,clues,originCatalogSize,sourceTypeDist,toolBeatCounts,proofRequiredBeats,totalMinutes}` · `sha256` · `bytes`.
**검사 수는 늘어난다** — 이전 판이 적어 둔 「47검사」는 그 시점의 값이었고 planner 가 `Z-01`·`H-04` 계열을 더하면서 바뀌었다. 어떤 문서도 그 수를 상수로 적지 않는다.

**저작 시점 규칙 (검증기가 수행 · 임포터는 재구현하지 않는다 · 참고용)**

| id | 규칙 | 검증기 대응 |
|---|---|---|
| I-1 | **독립쌍**: `proofRequired == true` 인 비트마다, `sourceType` 이 **서로 다르고** 루트 `originId` 도 **서로 다른** 단서 쌍이 최소 1개 존재한다(AND 조건) | `C-07` |
| I-2 | **루트 해석**: `copiedFrom` 체인을 끝까지 따라가 루트 `originId` 를 계산한다. **표시 라벨은 증명이 아니다.** 참조 누락·순환은 **둘 다 실패** | `C-03` `C-04` `C-05` |
| I-3 | **매체 다양성**: 모든 비트의 `clues[].sourceType` 종류 수 ≥ 2 | `C-06` |
| I-4 | **`zoneId`**: 모든 비트에 `zoneId` 가 존재하고 **소속 스테이지의 `zoneIds` 안**에 있다 | `Z-01` `Z-02` |
| I-5 | **`prerequisites`**: 참조된 비트 id 가 모두 존재하고 **자기 자신·순환을 만들지 않는다**. T0 는 `t0-b1 → t0-b2 → t0-b3` 사슬 [OBSERVED] | 검증기 `P-*` 계열 + 임포터 추가 검사 |
| I-6 | **id 유일**: beat id · clue id · checkpoint id 각각 중복 0 | `B-03` `B-04` `B-05` |
| I-7 | **도구 id**: `tools[]` ⊂ `{circuit, reader, alignment, routing, corrosion, seal}` | `V-01` |
| I-8 | **힌트**: 모든 비트가 빈 문자열 없는 3단 힌트를 갖는다 | `H-01` |
| ~~I-9~~ → `R-1` | **로컬라이즈** `[C7-F5 · RFC-S6 완화 2026-09-10 R7 종료]`: **KO 필수 · EN 선택.** 모든 표시 문자열이 **KO 키를 갖고 그 KO 명사가 `worldview/glossary.md` 에 등재**돼 있으면 통과한다. EN 필드가 없으면 **KO 로 폴백**하며 그것만으로 임포트를 실패시키지 않는다 — **T0 는 KO 전용**이고 EN 문자열은 로컬라이제이션 회차의 산출물이다(`decision-log.md` 「RFC-S6 / RFC-C6-001 / C4-F9 / C7-F5」). **fail-closed 로 남는 것은 「용어집 등재 여부」 하나뿐이다.** | **임포터 고유**(위 `R-1`) |
| ~~I-10~~ → `R-2` | **고아 0**: 참조되지 않는 단서·노드 0건 | **임포터 고유**(위 `R-2`) |

**위반 시 임포트 실패로 빌드를 멈춘다.** 경고로 통과시키면 그 규칙은 존재하지 않는 것과 같다. 이는 `V-1~V-4`(영수증 대조)와 `R-1~R-4`(런타임 검사) 둘 다에 적용된다 — **영수증 해시가 어긋나면 그 자리에서 실패한다.**

### ④-3 `autoKeptClueIds`(필수 단서 자동 보존)

- 정의: **첫 판독에서 자동 보존되는 검증 사본의 단서 id 집합**. `save.md` `progress.autoKeptClueIds`(string[]) 이며 **어떤 이벤트로도 제거되지 않는다**(`interaction-rules.md` §0-3 · 인수 테스트 T-05 · `save.md` S-I3 `autoKeptClueIds ⊆ discoveredClueIds`).
- **런타임에 채워진다.** 저작 JSON 에 초기 목록이 없으므로 임포터가 하드코딩하지 않는다.
- T0 에서 채워지는 경로: `t0-b3` 이 `proofRequired: true` 이고 그 독립쌍은 [OBSERVED]
  `t0-b3-c1`(`sourceType: plate`, `originId: plate-standard-hub`) × `t0-b3-c2`(`sourceType: ledger`, `originId: tide-ledger-bureau`) — 종류·루트가 모두 다르므로 I-1 을 만족한다.
  두 단서를 처음 판독하는 순간 각각 자동 사본이 생기고 `autoKeptClueIds` 에 들어간다(`plate-readout.md` P-R1).
- **필수 단서 목록 자체의 소유자는 `worldview/timeline.md` §7** 이다. 임포터가 그 목록을 만들지 않는다.

---

## ⑤ Sim 규칙 — T0 의 두 도구는 상세, 나머지 넷은 인터페이스 스텁

### ⑤-0 공통 골격

```csharp
// Tide.Sim — 개념 서명(구현 아님). 엔진 타입 0개.
public interface IPuzzleCommand {
    ValidationResult Validate(PuzzleState s);   // 실패 사유 = ReasonCode enum (문자열 아님)
    PreviewReport    Preview (PuzzleState s);   // 부작용 0. 2회 호출 후 stateHash 불변 (T-03)
    IReadOnlyList<PuzzleEvent> Commit(PuzzleState s);
}
// [C7-F7] 지속되는 재생 단위는 **명령**이다. 이벤트는 Commit 의 파생물이며 저장되지 않는다.
//         Payload 가 없으면 재생(T-13)도 되돌림도 구현할 수 없다. 정본 = data-schemas/save.md §3.0~§3.2
readonly record struct CommandEntry(long Seq, string BranchId, long ParentSeq,
                                    string CommandId, JsonObject Payload,
                                    string PayloadHash, bool Committed);
```

| 규칙 | 내용 |
|---|---|
| S-0 | `PuzzleState` 는 **불변**. 변경은 `Reduce(state, evt) -> newState` 하나뿐. `Reduce` 는 입력 상태를 바꾸지 않는다(T-01) |
| S-1 | 같은 이벤트 열을 재적용하면 **같은 상태 해시**(결정론 · T-02). 딕셔너리 순회 순서 의존 금지, 난수 0개, `id` 정렬 고정 |
| **S-1b** | **`Commit` 은 순수 함수다** `[C7-F7]` — 같은 `(state, commandId, payload)` 는 **항상 같은 이벤트 열**을 낸다. 시계·난수·문화권 파싱·순회 순서 의존 0건. 명령 소싱의 성립 조건이며 깨지면 재생이 깨진다(`save.md` `S-I10`) |
| S-2 | **전역 실시간 타이머 없음.** `Time.deltaTime` 은 연출 보간에만 쓴다. Sim 에 시간이 들어가지 않는다 |
| S-3 | `Validate` 통과 전에는 확정 버튼을 **활성화하지 않고 비활성 사유를 문장으로** 보여준다(문자열은 UI 가 로컬라이즈 테이블에서 만든다) |
| S-4 | Sim 은 표시 문자열을 만들지 않는다. 사유는 `ReasonCode` enum 이다 |

### ⑤-1 연습(sandbox) / 확정(commit) 2층

```
main 브랜치 (커밋된 진실)
   ├─ Fork(sandboxId) ──> 사본 상태 위에서 무제한 조작·되돌림 (세이브에 반영 안 됨)
   └─ Commit(sandboxId) ──> 검증 통과분만 main 에 재적용, 체크포인트 생성
```

| 규칙 | 내용 |
|---|---|
| F-1 | `Fork` 는 상태를 **복사하지 않는다.** 현재 `headSeq` 를 부모로 하는 새 `branchId` 만 만든다(O(1)) |
| F-2 | sandbox 명령은 `committed: false` 로 append 되고 브랜치별 상태는 **지연 계산**한다 |
| F-3 | `Commit` 은 sandbox 명령열 전체를 main 상태에 **다시 `Validate`** 한 뒤 통과할 때만 적용한다. sandbox 통과가 main 통과를 뜻한다고 가정하지 않는다 |
| F-4 | **폐기된 sandbox 는 저장되지 않는다.** 남는 것은 개수(`sandboxDiscardedCount`)와 시간(`sandboxTimeMin`)뿐 |
| F-5 | **sandbox 에서는 어떤 자원도 실제로 줄지 않는다.** 원본 상태 카운터·부식 상한은 **표시만** 된다 |
| F-6 | sandbox 중 저장하면 sandbox 는 저장되지 않는다. UI 가 "연습 내용은 저장되지 않습니다"를 확정적으로 고지한다 |

### ⑤-2 명령 로그 되돌림 — **되돌림 횟수 상한 없음 / 로그 크기 상한 있음** `[C6-F12 병기 2026-09-10 R7 종료]`

**두 문장을 항상 함께 읽는다.** 「되돌림 상한 없음」은 *횟수·비용·페널티*의 이야기이고(U-3 · N-12), 명령 로그에는 별개로 **크기 상한**이 있다(U-8). 상한에 닿으면 되돌림이 **막히는 것이 아니라 입도가 낮아진다** — 오래된 구간을 스냅샷으로 접고 그 구간의 되돌림을 체크포인트 단위로 내린다. 이 접힘의 정본은 **`system-specs/save-undo.md` §6 `SV-F6`**(「명령 로그 상한 초과(**바이트 또는 개수, 먼저 닿는 쪽**) → 오래된 구간을 스냅샷으로 접고 되돌림 입도를 체크포인트 단위로 낮춤 · 플레이어 손실 = 세밀한 되돌림 입도(고지)」)이며 UI 는 그 고지를 반드시 낸다.

> **디렉터 배정문의 숫자에 대한 정정 `[counter · 증거 첨부]`**: `decision-log.md` 「C4-F7 / … / C6-F12 … 레인 배정 (R7)」 표는 상한을 **「50k/8MB」**로 적었다. 그 짝은 `payload` 가 없던 시절의 값이며 **C7-F7 해소에서 재산정돼 이미 폐기됐다** — 현행 정본은 `data-schemas/save.md` §3.2 · `save-undo.md` §9 의 **`byteCap` 6 MiB / `entryCap` 20,000**(세이브 파일 전체 ≤ 8 MB 중 명령 로그 몫)이다. 엔트리 평균 ≈280 B `[INFERENCE]` 에서 50,000 × 280 = 14 MB 가 되어 8 MB 예산과 모순되기 때문이다. 여기에 50,000 을 다시 적으면 같은 저장소에 두 개의 상한이 생긴다 → **병기 요구는 이행하되 숫자는 현행 정본을 쓴다.** 배정문의 수치 정정은 QA·디렉터 판정 대상으로 올린다.

| 규칙 | 내용 |
|---|---|
| U-1 | 되돌림은 **삭제가 아니라 `headSeq` 하향**이다. 로그 항목은 지워지지 않는다 |
| U-2 | 되돌린 뒤 새 명령을 넣으면 **새 브랜치로 분기**한다. 앞의 이력은 계속 접근 가능하다 |
| U-3 | **되돌림 횟수 제한 0 · 비용 0 · 페널티 0.** 프로토타입의 `LIMITS.maxUndo: 32` 는 **탐색 편의 상수이며 사양이 아니다**(RFC-P3-015 F23 · `prototype/prototype.meta.md`). 코드에 **횟수** 상한을 넣지 않는다. **로그 크기 상한(U-8)은 이 규칙의 예외가 아니라 다른 축이다** — 크기 상한은 되돌림을 막지 않고 입도만 낮춘다(`SV-F6`) |
| U-4 | 상태 재구성은 `for e in entries: events = Commit(state, e.commandId, e.payload); state = Reduce*(state, events)`. 같은 로그 → 같은 해시. **저장된 것은 명령이고 이벤트는 재생으로 다시 만든다** `[C7-F7]` — 절차 전문은 `save.md` §3.1 |
| **U-4b** | 재생 중 `Validate` 실패는 **조용히 건너뛰지 않는다.** 데이터 손상·버전 불일치를 뜻하므로 복구 패널로 간다(`save-undo.md` `SV-F9`) |
| U-5 | 로그가 길어지는 비용은 **200 커밋마다 스냅샷**으로 접는다. 로드 시 최신 스냅샷 이후만 재생 |
| U-6 | 되돌림 불가 구간은 **없다**. 다만 `Commit` 은 프리뷰 + 확정 입력 + **사전 체크포인트** 3중 확인을 요구한다 |
| U-7 | 텔레메트리 `undo_count` 는 U-1 이벤트만 센다. 브랜치 폐기는 `sandbox_discarded` 로 따로 |
| U-8 | 로그 상한 `[TARGET]` **`byteCap` 6 MiB 또는 `entryCap` 20,000, 먼저 닿는 쪽** `[C7-F7 재산정]` — 초과 시 동작은 **`save-undo.md` §6 `SV-F6`** 이 소유한다(스냅샷 접기 + 입도 하향 + 고지). 「50,000」은 `payload` 가 없던 시절의 짝이라 폐기했다 — 엔트리 평균 **≈280 B `[INFERENCE]`** 로는 50,000 × 280 = 14 MB 라 8 MB 예산과 모순된다. 재산정 근거·측정 과제는 `save.md` §3.2. 초과 시 오래된 구간을 스냅샷으로 접고 그 구간의 되돌림은 체크포인트 단위로 낮춘다. **T0 에서 상한에 닿지 않는다는 것은 `[INFERENCE]`** 이며 T0 계측(`command_count` · `save_file_bytes / entries_count`)으로 확인한다(RISK-S3) |

### ⑤-3 T0 도구 상세

#### (A) `circuit` — 배선 추적 (법1 "배선된 것만 남는다") · 스펙 `system-specs/wiring-trace.md`

**성격**: 판독 도구. **확정(commit) 없음.** 다른 도구의 확정에 근거 유효성을 공급한다.

*입력 → 의도(Intent)* — Sim 에 들어가는 것은 의도뿐이다.

| 입력 | 마우스 | 키보드 단독 | 패드 | 의도 |
|---|---|---|---|---|
| 계통 선 추적 | 선 위 좌클릭·드래그 | 계통 목록 초점 후 `Enter`(D-1) | 좌스틱 + `A` | `TraceSystem(systemId)` |
| 노드 다중 선택 | `Shift`+클릭 | 항목별 반복 토글(D-6) | `LT` 홀드 + `A` | 범위 비교 모드 |
| 투명지 정렬 | 드래그 | 방향키 **격자 1칸**, `Shift` 정밀(D-3) | 스틱 · `LT` · D-Pad | `SetOverlayOffset(dx,dy)` |
| 구획 접기 | 구획 클릭 | 구획 초점 후 `Enter`(D-1) | `X` | `ToggleUncovered(areaId)` |
| 근거 유효성 조회 | hover | **`Q`** | `RS` | 조회(상태 변화 없음). **`I` 가 아니다** — `I`·`H`·`F1` 은 표면과 무관하게 증거함·가설판·힌트다(`interaction-rules.md` §1-3.1 예외 · C7-F10) |

*상태기계*: `Idle → Tracing → Overlaying → Marking → Resolved` (그리고 `any → Idle` on `CloseTool`, 상태는 세션 내 보존).

| 현재 | 이벤트 | 다음 | 부작용 |
|---|---|---|---|
| `Idle` | `OpenTool` | `Tracing` | 구역 평면도 로드, 마지막 선택 복원 |
| `Tracing` | `TraceSystem(id)` | `Tracing` | `selectedSystemId` 갱신, 연결 노드 하이라이트 |
| `Tracing` | `BeginOverlay` | `Overlaying` | 분기 투명지 표시 |
| `Overlaying` | `SetOverlayOffset` | `Overlaying` | 정렬 오차 표시 갱신 |
| `Overlaying` | `AnchorOverlay`(3점 일치) | `Marking` | `overlayAligned = true` |
| `Overlaying` | `Cancel` | `Tracing` | 오프셋 폐기, **상태 손실 0** |
| `Marking` | `ToggleUncovered(areaId)` | `Marking` | 해칭 토글 + 근거 매체 슬롯 요구 |
| `Marking` | 모든 미배선 구획에 근거 1종 | `Resolved` | 벽 지도 음영 확정, 이후 판독 화면에 음영 상속 |
| `Resolved` | `ToggleUncovered` | `Marking` | **되돌림 자유**, `Resolved` 해제 |

`Resolved` 는 확정이 아니다. 세이브에는 `uncoveredAreas` 만 남고 진행 잠금이 없다.

*확정 조건*: **없다**(판독 도구). 대신 다른 도구의 확정에서 `evidenceValidity == out_of_coverage` 근거를 **자동 무효화**한다.

*실패 문장(플레이어가 보는 문장 — 로컬라이즈 키로 만든다)*

| 상황 | 사유 코드 | 문장(KO 초안) | 손실 |
|---|---|---|---|
| 미배선 구획 근거로 결론 제출 | `OUT_OF_COVERAGE` | "이 근거는 센서가 닿지 않는 구획에서 나왔습니다. 배선 범위 밖의 기록은 결론을 세우지 못합니다." | 없음(제출 자체가 막힘) |
| 투명지 3점 정렬 실패 | `ANCHOR_INCOMPLETE` | "세 점 중 {n}점이 어긋났습니다. 어긋난 점을 다시 맞춰 주세요." | 없음 |
| 데이터에 고아 `systemId` | (임포트 실패) | 플레이어에게 도달하지 않는다 | 없음 |
| 구역 침수로 평면도 접근 불가 | `ZONE_LOCKED` | "이 구역은 지금 닿을 수 없습니다. 같은 결론을 세울 다른 자료가 남아 있습니다." | 접근 지연, **진행 불가 아님** |

*하드 요구*: **진행 불가(soft-lock) 0건.** 이 도구만으로 도달 불가 상태가 생기지 않는다(W-R6 · 인수 `B-W1`).
*법1 파생 규칙*: `no_record` 와 `no_event` 를 **서로 다른 값**으로 유지하고 UI 라벨도 다르게 쓴다(W-R4). "기록의 침묵"은 부재의 증거가 아니다.

#### (B) `reader` — 판독 (법2 "원본은 닳지만 사본은 남는다") · 스펙 `system-specs/plate-readout.md`

**성격**: 판독 도구이며 **T0 의 유일한 확정(commit) 경로를 갖는다** `[RFC-C7-001 (1) · C7-F8 정정 2026-09-10 R7 종료]`.

> **이전 판의 문장을 철회한다.** 여기에는 「확정은 `seal` 에서만 일어난다 … **T0 에서 확정되는 것은 없다**」고 적혀 있었고, 그 때문에 DoD 6·8(확정·저장 롤백 테스트)이 **검사할 대상이 없는 요구**가 됐다(C7-F8 · C6-F10).
> 디렉터 판정: **T0 의 확정 명령은 `reader` 의 「인용 고정(pin citation)」이다.** 정본은 `planning/gdd.md` §4 표의 `reader` 확정 층(「판독 결과를 가설판에 인용으로 고정, 확정 조건: 매체·계통·관측소 출처가 채워졌을 때」)이며, `interaction-rules.md` §2.2 「판독 자체는 확정이 아니다」와 **모순되지 않는다** — 판독(`Read`)과 인용 고정(`CiteToBoard`)은 다른 명령이다.
> **`seal`(이중서명)은 여전히 T0 범위 밖**이다. T0 에서 확정되는 것은 *결론*이 아니라 *인용*이다.

*입력 → 의도*

| 입력 | 마우스 | 키보드 단독 | 패드 | 의도 |
|---|---|---|---|---|
| 매체 적재 | 증거함 → 판독대 드래그 | `I` → 자료 `Enter`(집기) → 판독대 `Enter`(적재) (D-2+D-7) | `A` → `A` | `LoadRecord(id)`. 여기의 `I` 는 **오버레이(증거함) 진입점**이며 패널 안에서도 같은 일을 한다 |
| 시간 범위 지정 | 타임라인 드래그 | `←`/`→` **4분 스텝**, `Shift` 정밀 (D-3) | 좌스틱 + D-Pad 4분 | `SetWindow(start,end)` |
| 배율 | 휠 | `↑`/`↓` 배율 단계 1칸 (D-3) | `LT`/`RT` | `zoom` |
| 재생(판독) | 버튼 | `Space` | `X` | `Read` — **사본 재생. 부작용 0** |
| 원본 직접 절차 | — | **확정 흐름**(§⑦ `two-step`) | 동상 | `ReadOriginal` — `readCounts[id] += 1` |
| 인용 고정 | 결과 위 `Ctrl`+클릭 | 결과 항목 초점 후 `Enter`(D-1) | `Y` | `CiteToBoard` |

> `Space`/`X` 는 **부작용 없는 실행에만** 배정된다. `ReadOriginal` 을 `Space` 에 걸지 않는다 — `interaction-rules.md` §1-3.3.

*상태기계*: `Empty → Loaded → Read`(그리고 상한 초과 시 `Degraded`), `any → Empty` on `Unload`(상태 보존).

| 현재 | 이벤트 | 다음 | 부작용 |
|---|---|---|---|
| `Empty` | `LoadRecord(id)` | `Loaded` | 4분 분해능 눈금 표시 |
| `Loaded` | `SetWindow` | `Loaded` | 미리보기 갱신(**재생 아님**, 카운터 0) |
| `Loaded` | `Read`(최초, `autoCopyTaken == false`) | `Read` | **검증 사본 생성** + `autoCopyTaken = true` + 토스트 |
| `Loaded` | `Read`(재판독, 사본 존재) | `Read` | **사본에서 읽음. `readCount` 증가 없음** |
| `Read` | `CiteToBoard` | `Read` | 인용 카드 생성(매체·계통·관측소 기록) |
| `Read` | `ReadOriginal`(사본에 없는 구간) | `Read` | `readCount[id] += 1` |
| `Read` | `readCount == 3` 상태에서 `ReadOriginal` | `Degraded` | 원본 결정 붕괴. **사본은 그대로 유지되고 진행은 막히지 않는다** |
| `Degraded` | `Read` | `Read` | 사본 기반 판독은 **무제한** 계속 가능 |

*확정 조건*: **판독(`Read`) 자체는 확정이 아니다.** 확정은 **`CiteToBoard`(인용 고정)** 이며 조건은 **인용에 `sourceType` · 계통 · 관측소 출처가 모두 채워졌을 때**다(`gdd.md` §4 reader 확정 층).
확정이므로 다음을 **전부** 거친다 — 프리뷰 → 확정 입력(`two-step` 기본) → **사전 체크포인트**(`checkpointBefore: true`) → 저장 트랜잭션(§⑥) → 되돌림 가능. `T-15`~`T-20`(rename 직전 실패·중간 실패·멱등성·지연 완료·크래시·`SavePending`)의 **T0 실행 대상이 이 명령이다.**
`ReadOriginal`(원본 직접 절차)도 상태를 바꾸므로 확정 흐름을 거친다(`interaction-rules.md` §1-3.3) — `Space`/`X` 에 걸지 않는다.
*예산 규칙(오해가 잦은 곳)*: 사본 재생은 **횟수 제한 0 · 비용 0**. 세는 것은 **원본에 직접 가하는 파괴적 절차**뿐이고 상한은 `readBudget = 3`(데이터 노브)이다. 상한 도달은 **진행을 막지 않으며** 바뀌는 것은 에필로그 기록 패널의 **보존 등급 문장 1줄**뿐이다(P-R9 · P-R11). UI 표기는 **"원본 상태"** 이고 "예산"이라는 단어는 부식에만 쓴다(P-R10).

*실패 문장*

| 상황 | 사유 코드 | 문장(KO 초안) | 손실 |
|---|---|---|---|
| 상한 도달 후 원본 신규 구간 판독 | `ORIGINAL_DEGRADED` | "이 원본은 더 이상 새 구간을 내주지 않습니다. 보존된 사본으로 계속 읽을 수 있고, 같은 결론을 세울 다른 자료도 남아 있습니다." | 그 구간의 **원본** 판독만 |
| 같은 매체를 근거 2슬롯에 | `MEDIA_DUPLICATE` | "두 근거의 매체 종류가 같습니다. 서로 다른 종류의 자료가 필요합니다." | 없음 |
| 4분 미만 간격으로 선후 확정 | `INDETERMINATE` | "두 시각의 간격이 기록의 분해능(4분)보다 작아 선후를 말할 수 없습니다." | 없음, 진행 계속 |
| 사본 파괴 명령이 데이터에 존재 | (임포트 실패) | 플레이어에게 도달하지 않는다 | 없음 |

*분해능 규칙*: Sim 은 4분 미만 간격을 **`indeterminate` 로 유지하고 반올림하지 않는다**(P-R5). `resolutionMinutes` 는 데이터 상수이며 코드 하드코딩 대상이 아니다.
*연습 층*: 카운터는 sandbox 에서 오르지 않는다(P-R7).

### ⑤-4 나머지 4도구 — **인터페이스 스텁만**

`alignment` · `routing` · `corrosion` · `seal` 은 T0 범위 밖이다. 다음만 만든다.

- `ToolAsset` 6종의 **id 와 `lawId` · `hasCommit` · `consumesBudget` 필드**(데이터에 존재해야 임포트 검증이 6종을 셀 수 있다).
- `IPuzzleCommand` 를 구현하되 `Validate` 가 `ReasonCode.NOT_IMPLEMENTED_IN_T0` 를 돌려주고 `Commit` 이 **이벤트 0개**를 돌려주는 스텁 클래스.
- UI 는 도구 패널 6칸 중 4칸을 **비활성 + 사유 문장**("이 도구는 이번 슬라이스에 없습니다")으로 표시한다. 칸 자체를 숨기지 않는다 — 6칸 레이아웃이 T0 에서 실제로 검증돼야 하기 때문이다.
- **스텁이 상태를 바꾸는 경로를 만들지 않는다.** `Commit` 이 빈 목록을 돌려주는 것은 `T-04`(Validate 실패 시 Commit 이벤트 0개)와 같은 계약이다.

### ⑤-5 힌트

- **3단(방향 / 절차 / 해답), 무료·무제한, 순서대로만 열린다.** 사용 기록은 저장되지만 엔딩·보상·평가에 **영향 0**.
- **무진전 자동 제안**: `idleSeconds ≥ 180` 이면 **비강제 토스트 1종**을 1회 띄운다. 무시하면 **180초 쿨다운** 후 같은 토스트를 다시 낸다.
- **단계 자동 승격 금지**: 어떤 경과 시간·오확정 횟수도 `revealedLevel` 을 스스로 올리지 못한다(H-R9). 자동으로 일어나는 것은 `Closed → Offered` 하나뿐이다.
- 180 은 `[tunable: balance]` 이며 제안 임계와 쿨다운 **두 값**이다. 값을 바꿔도 단계 수·승격 여부는 바뀌지 않는다(H-R10).
- 토스트는 **초점을 빼앗지 않는다.** `Tab` 순회에 끼어들 뿐이다.

### ⑤-6 완료 술어 — 무엇이 일어나면 그 비트가 끝나는가 `[RFC-C7-001 (3) · C7-F8]`

**세 비트의 완료 조건은 문장이 아니라 데이터다.** `systems/data/t0/beats.json` 의 각 행이 `completionPredicate` 를 갖고, 그 안의 `_srcCompletion` 이 `campaign.json` 의 `completion` 원문을 함께 싣는다. 구현은 **문장을 해석하지 말고 술어를 검사**한다.

| 비트 | `kind` | 확정 명령 | 완료 요구(요약 — 정본은 `beats.json`) |
|---|---|---|---|
| `t0-b1` | `viewing` | **없음** | `rec-handover-brief` 3항(`hb-l1~l3`) 열람 · `rec-transfer-list` 3줄(`tl-r1~r3`) 열람 · 상시 슬롯에 `plate-zero` 적재 · `tl-r4` 처리 여부(기입/공란) 1회 기록(**가역**) |
| `t0-b2` | `marking` | **없음** | `hub` 미배선 구획 **3개** 지정 + 구획당 근거 매체 **1건** |
| `t0-b3` | `citationPinned` | **`CiteToBoard`** | 인용 고정 **2건**(`t0-b3-c1` `plate`/`plate-standard-hub` × `t0-b3-c2` `ledger`/`tide-ledger-bureau`) · 독립쌍 성립(`C-07`: `sourceType` 상이 AND 루트 `originId` 상이) · 자동 사본 **1점** · 결손 양 끝(`H-1:00` / `H+3:00`) 고정 |

- **`t0-b3` 의 독립쌍은 손으로 고른 것이 아니다** — `node planning/validate-campaign.mjs --pairs` 의 `C-07` 출력을 생성기가 그대로 옮긴다. planner 가 `proofRequired` 를 바꾸면 술어도 바뀐다.
- 생성기는 **`proofRequired` 비트에 확정 명령이 없으면 exit 2** 한다. DoD 6·8 이 검사할 대상이 사라지는 상태를 데이터 층에서 막는다.
- `t0-b1`·`t0-b2` 는 확정 명령을 발행하지 않는다. **그 두 비트에서 `T-15`~`T-20` 을 돌리지 않는다** — 돌릴 대상이 없다.

### ⑤-7 `ReasonCode` — **enum 은 systems 소유 정본이며 `system-specs` 실패 모드에서 도출한다** `[C7-F14 · C6-F11 ⑤]`

`ReasonCode` 는 UI 문자열이 아니라 **코드**다(S-4 · R-5). 아래 표는 각 코드가 **어느 스펙의 어느 실패 모드 행에서 나왔는지**를 1:1로 밝힌다 — 발명한 문자열이 아니라는 증명이 이 열이다.
KO 문장은 **`[TARGET] 자리표시자**이며 **`Assets/_Project/Localization/strings/ko.json` 한 곳에만** 둔다. 코드·프리팹·아트에 문자열을 굽지 않는다.

| `ReasonCode` | 출처(실패 모드 행) | 쓰는 도구 | T0 |
|---|---|---|---|
| `OUT_OF_COVERAGE` | `wiring-trace.md` **W-F1** · `dual-seal.md` **S-F2** | circuit, seal | **필수** |
| `ANCHOR_INCOMPLETE` | `wiring-trace.md` **W-F2** | circuit | **필수** |
| `ZONE_LOCKED` | `wiring-trace.md` **W-F4** · `drainage-routing.md` **R-F4** | circuit, routing | **필수** |
| `ORIGINAL_DEGRADED` | `plate-readout.md` **P-F1** | reader | **필수** |
| `MEDIA_DUPLICATE` | `plate-readout.md` **P-F2** · `dual-seal.md` **S-F1** | reader, seal | **필수** |
| `INDETERMINATE` | `plate-readout.md` **P-F4** | reader, alignment | **필수** |
| `RESIDUAL_EXCEEDED` | `tide-alignment.md` **A-F1** | alignment | 스텁 |
| `NO_SHARED_TIDE_EVENT` | `tide-alignment.md` **A-F2** | alignment | 스텁 |
| `NOT_ALIGNED` | `tide-alignment.md` **A-F3** · `dual-seal.md` **S-F3** | alignment, seal | 스텁 |
| `ANCHOR_SPREAD_INSUFFICIENT` | `tide-alignment.md` **A-F4** | alignment | 스텁 |
| `DUAL_PROTECTION_FORBIDDEN` | `drainage-routing.md` **R-F1** | routing | 스텁 |
| `ROUTE_CYCLE` | `drainage-routing.md` **R-F2** | routing | 스텁 |
| `CORROSION_LIMIT_EXCEEDED` | `corrosion-budget.md` **C-F1** | corrosion, routing | 스텁 |
| `NO_VALID_ROUTING` | `corrosion-budget.md` **C-F2**(무료 우회관 제안) | corrosion, routing | 스텁 |
| `WITNESS_DECLINED` | `dual-seal.md` **S-F4** | seal | 스텁 |
| `SAVE_CHECKSUM_FAILED` | `save-undo.md` **SV-F2** | save | **필수** |
| `SAVE_VERSION_REFUSED` | `save-undo.md` **SV-F3** | save | **필수**(`T-10`) |
| `WRITE_FAILED` | `save-undo.md` **SV-F4** | save | **필수**(`T-16`) |
| `REPLAY_HASH_MISMATCH` | `save-undo.md` **SV-F5** | save | **필수** |
| `LOG_CAP_EXCEEDED` | `save-undo.md` **SV-F6** | save | **필수**(고지 · 접힘) |
| `DUPLICATE_COMMIT_SUPPRESSED` | `save-undo.md` **SV-F8** | save | **필수**(`T-17`·`T-18`·`T-19`) |
| `REPLAY_VALIDATE_FAILED` | `save-undo.md` **SV-F9** | save | **필수**(`U-4b`) |
| `NOT_IMPLEMENTED_IN_T0` | 브리프 §⑤-4(스텁 4도구) | 스텁 4종 | **필수** |

- **임포트 실패로만 나타나는 실패 모드**(`W-F3` · `P-F3` · `C-F3` · `C-F4` · `R-F5` · `S-F6` · `H-F2` · `H-F4`)는 **`ReasonCode` 를 갖지 않는다** — 플레이어에게 도달하지 않기 때문이다. 임포터의 실패 메시지는 개발자용이며 로컬라이즈 대상이 아니다.
- **KO 문장은 지금 확정하지 않는다.** §⑤-3 의 「문장(KO 초안)」 열은 `[TARGET] 자리표시자`이며 최종 문안은 worldview 용어집 등재(RFC-S6) 이후다. 실행자는 그 초안을 `strings/ko.json` 에 넣되 **그것을 캐논으로 인용하지 않는다**.
- **EN 은 T0 에서 만들지 않는다**(RFC-S6). `strings/en.json` 이 없으면 KO 로 폴백한다.

---

## ⑥ Save / Undo — 파일 4갈래 · 원자적 rename · `SavePending` · 실패 롤백

### ⑥-1 파일 구성

| 파일 | 언제 쓰이나 |
|---|---|
| `save.json` | 현재 자동 저장(정본) |
| `checkpoint.pre-commit.json` | **확정 직전** 스냅샷 |
| `save.bak` | 직전 세대 |
| 수동 슬롯 **3개** | 플레이어가 직접 저장 |

네 갈래가 **실제로 다른 파일**이므로 복구 3단계가 존재한다. `settings.json` 은 세이브와 **분리**한다.

### ⑥-2 쓰기 절차 (하드)

```
1) save.tmp 기록  →  2) 플랫폼이 지원하면 flush/fsync  →  3) 기존 정본을 save.bak 으로 회전  →  4) rename(tmp, save.json)
```

- **부분 파일이 정본 자리에 오는 경로가 없어야 한다.**
- 체크섬(`sha256(키 사전순 정규화 본문)`) 실패 시 **정본을 덮어쓰지 않고** 읽기 전용 복구 패널로 간다.
- `schemaVersion` 은 정수 단조 증가. **더 높거나 미등록 버전은 절대 열지 않고 절대 덮어쓰지 않는다.**
- 마이그레이터는 **단방향**이며, 마이그레이션 전 원본을 `save.v{n}.bak` 으로 보존한다.
- DLC 플래그가 없어도 본편 로드·엔딩 3종 도달이 성립해야 한다.

### ⑥-3 `SavePending` 상태

| 규칙 | 내용 |
|---|---|
| SP-1 | 진행 표시를 띄우고 **중복 커밋만 차단**한다 |
| SP-2 | **되돌림·설정·힌트·증거함 열람 등 나머지 UI 는 계속 응답한다. 전역 입력 잠금 없음** |
| SP-3 | 성공 연출(결과 패널·도장 애니·확정 사운드)은 **저장 영수증이 도착한 뒤에만** 재생한다 |
| SP-4 | `commitIdempotencyKey` 로 같은 커밋의 중복 적용을 막는다 |
| SP-5 | **지연 완료**: 이미 취소·이탈한 커밋의 늦은 완료 콜백은 키 불일치로 폐기하고 UI 를 바꾸지 않는다 |

### ⑥-4 실패 시 롤백 (T-15 의 대상)

- **메모리 상태와 디스크를 둘 다 확정 이전으로 유지**한다.
- **성공 연출을 재생하지 않는다.**
- `재시도` / `뒤로` 두 선택지를 문장과 함께 제시한다. 조용한 실패·자동 무시 없음.
- `rename` 직후·UI 갱신 전 크래시: 다음 실행에서 정본이 이미 새 세대이므로 **복구 로드로 이어받고** 멱등 키가 중복 적용을 막는다.
- 손상 감지 시 복구 패널은 `백업으로 열기 / 사전 체크포인트 / 새로 시작` 을 제시하고 **손상 파일을 덮어쓰지 않는다**. 초기 초점은 **`백업으로 열기`** 이며 파괴적 선택에 초기 초점을 두지 않는다.

### ⑥-5 필드 (개명 금지 주의)

루트: `schemaVersion` `saveId` `createdUtc` `updatedUtc` `appVersion` `stageId` `beatId` `storyClock` `tidePhase` `playSeconds` `progress` `commandLog` **`commitIdempotencyKey`** `settingsRef` `checksum`.
`progress`: `autoKeptClueIds` `discoveredClueIds` `readCounts` `bypassUsed` `alignedPairs` `committedRouting` `propertyProtection` `sealedConclusions` `witnessChoices` `submissionPerspective` `plateZeroNameKnown` `hintLevelUsed` `checkpoints` `dlcFlags`.
`commandLog`: `headSeq` `branchId` `entries` `snapshots` `logHash` `sandboxDiscardedCount` `entryCap` **`byteCap`**.
`commandLog.entries[]`: `seq` `branchId` `parentSeq` `commandId` **`payload`** `payloadHash` `committed`.

- **개명 금지 목록**(v1 확정 시점부터 하드): `schemaVersion`, `saveId`, `stageId`, `beatId`, `autoKeptClueIds`, `readCounts`, `propertyProtection`, `sealedConclusions`, `submissionPerspective`, `hintLevelUsed`, `checkpoints`, `dlcFlags`, `commandLog.headSeq`, `commandLog.entries`, **`commandLog.entries[].commandId`**, **`commandLog.entries[].payload`**, **`commitIdempotencyKey`**, `checksum`.
- **`[C7-F7]` 세 필드가 2026-09-10 R7 에 신설·등재됐다**: `payload`(정규화 JSON — 이것이 없으면 명령 로그를 재생할 수 없다), `commitIdempotencyKey`(루트 · `T-17`·`T-18`·`T-19` 가 단언하는 필드인데 스키마에 없었다), `byteCap`. `payloadHash` 는 **무결성 전용**으로 격하됐다(payload 에서 파생되므로 재생 입력이 아니다).
- **세이브 파일에 이벤트는 0건이다.** 이벤트 소싱 흔적(`eventSeq`·`eventLogHash`)을 넣지 않는다 — `unity-implementation.md` §7 조각의 그 필드들은 같은 회차에 걷어냈다.
- **부식 저장 필드는 없다.** `operationalCorrosion` · `corrosionRemaining` · `corrosionBySystem` 같은 필드를 **추가하지 않는다** — 부식은 `committedRouting`(또는 sandbox 구성안)에서 **매번 계산하는 파생값**이다(RFC-P3-009 · `save.md` §2.1).
- **RFC-S3 판정됨 — `chapter` / `dayIndex` 는 없다** `[2026-09-10 R7 종료]`: `production/decision-log.md` 「RFC-S3 · 세이브 `dayIndex`」 = 「**제거 승인.** 캐논은 단일 야간(21:00→05:00)이므로 `dayIndex`·`chapter` 는 저장하지 않는다. 스테이지 진행은 `storyPhase`(스테이지 id)로 충분. **마이그레이션 대상 아님**(v1 이전에 제거)」.
  - 따라서 **마이그레이터를 만들지 않는다.** v1 이 두 필드를 가진 적이 없으므로 `schemaVersion=0 → 1` 승격(`T-09`)에서도 두 필드를 읽거나 채우지 않는다.
  - 진행 위치의 정본은 루트 `stageId`(+ `beatId`)다. `save.md` §2 「`dayIndex` 없음(단일 야간)」과 같은 말이며, **UI 라벨에도 「일차」·「N일째」를 쓰지 않는다** — 체크포인트 라벨은 스테이지·비트 이름으로 만든다(C7-F27 잔여분).

---

## ⑦ Input — `interaction-rules.md` §1 바인딩표 (T0 필수분)

### ⑦-0 착수 전 결정 4건 — **되묻지 않는다** `[C6-F11 · PRE-1 · 2026-09-10 R7 종료]`

C6-F11: 「T0 착수 전 결정 4건(기준 HW·Input System·URP·asmdef)이 초안 문자열 전건 0회」. 판정문은 `production/decision-log.md` 「C6-F11 / PRE-1 / C7-F14 / C7-F12 / C7-F3 · T0 착수 전 결정 4건 + 실행자 규칙」이며 아래는 그 **인용**이다.

| # | 결정 | 값 | 실행자에게 의미하는 것 |
|---|---|---|---|
| ① | **기준 하드웨어** | **미정 유지**(PRE-1). 개발 참조 기기는 이 세션의 **Apple Silicon Mac(16 GB)** | 성능 수치를 **캡처는 하되 PASS/FAIL 로 판정하지 않는다.** `hw_profile_id` 가 빈 결과는 게이트 입력이 아니다. Windows 우선 대상이고 macOS 캡처는 참고값이다 |
| ② | **입력** | **Unity Input System 패키지(new)** 사용. `ProjectSettings` **`activeInputHandler = 2`(Both)** 로 T0 를 시작(레거시 UI 호환). 액션 맵 이름 **`Watch`** | 현재 값은 `0`(레거시)이다 [OBSERVED §①-1] → **`2` 로 바꾼다.** 액션 이름은 아래 ⑦-1 행에 1:1 대응하는 9종: 노드 이동 · 조사 · 도구 · 프리뷰 · 확정 · 되돌림 · 증거함 · 힌트 · 뒤로. **`C7-F18`(액션 맵·액션 이름 미정)은 이 행으로 닫힌다** |
| ③ | **렌더** | **URP**(2.5D 고정 시점 · 라이트 프로브 불필요 · 포스트프로세싱 최소). 버전은 `[PIN-AFTER-RESOLVE]` | §③-1 표에서 「선택(권장)」이 **「필수 — 채택 확정」**으로 바뀌었다. 부트스트랩 필수 패키지가 **5종**이 된다(§③-1a) |
| ④ | **asmdef** | **7분할**(RFC-S2) | §③ 트리대로. `unity-implementation.md` §2 의 5분할 서술은 정정 완료 |

> ①은 **미정이 결정**이다. 「기준기가 없으니 대충 이 기계로 판정한다」가 아니라 **판정을 하지 않는다**는 뜻이다. 성능 표를 채우되 결론 칸은 비운다.

### ⑦-1 바인딩표

| 행동 | 마우스(편의) | **키보드 단독(항상 성립)** | 컨트롤러 |
|---|---|---|---|
| 시점 노드 이동 `[C7-F35]` | 노드 클릭 | `Tab`/`Shift+Tab` 초점 + `Enter` — **`Q`/`E` 가 아니다** | 좌스틱 + `A` |
| 대상 조사 | 좌클릭 | 초점 이동 후 `Enter` | `A` |
| 도구 열기 `[C4-F20]` | 도구 휠 클릭 | **`1`~`6` 직접 지정** | 패널 밖 `Y` 휠 · **`RB` = 다음 도구 · `LB`+`RB` = 이전 도구**(지속시간으로 갈리지 않는다) |
| 값 미세 조절 | 드래그·휠 | 방향키 1스텝, `Shift`+방향키 정밀 | 스틱 · D-Pad 1스텝 · `LT` 정밀 |
| 커넥터 연결 | 드래그 | **2단계 선택**: 출발 `Enter` → 도착 `Enter` | `A` 두 번 |
| 연결 해제 | 우클릭 | `Delete` | **패널 안에서만 `Y`** |
| 프리뷰 | 프리뷰 버튼 | `Space` | **`X`** |
| 확정 | 확정 버튼 클릭 | 확정 버튼 초점 후 `Enter` | 초점 후 `A` |
| 되돌림 / 다시 | 버튼 | `Ctrl+Z` / `Ctrl+Y` | `LB`+`X` / `LB`+`B` |
| 증거함 · 가설판 · 힌트 | 탭 클릭 | `I` · `H` · `F1` — **표면과 무관하게 같은 일**(도구 패널 안에서도) | `back` · `LB`+`back` · `LB`+`RS` |
| **도구 패널 조회** `[C7-F10]` | hover | **`Q`** — 그 패널이 등재한 조회 1개(`circuit` 근거 유효성 · `alignment` 선후 판정 · 나머지 4종 없음) | `RS` 단독 |
| 뒤로/취소 | 닫기 버튼 | `Esc` | `B` |

### ⑦-2 불변식 (전부 T0 필수)

| # | 규칙 |
|---|---|
| K-1 | **키보드 단독으로 모든 도구 조작이 완결된다.** 포인터·홀드·다중선택은 편의 경로일 뿐 유일 경로가 아니다(인수 테스트 **T-24**) |
| K-2 | **확정 기본값은 `two-step`**, 홀드는 **opt-in**, `confirm-dialog` 는 세 번째 값. 세 값 모두로 같은 퍼즐을 완주할 수 있어야 한다(**T-25**) |
| K-3 | **`X` 단독 = 「부작용 없는 실행」 전용.** 셸에서는 확정 프리뷰, 도구 패널에서는 그 패널이 등재한 시험·제안·재생 1개. **T0 두 도구에서는 「프리뷰」라는 말이 성립하지 않는다** — `circuit` = 구획 접기, `reader` = 사본 재생이다(`T-26a`·`T-26b`) |
| K-4 | **`Y` 모드 분리**: 패널이 닫혀 있으면 도구 휠, 열려 있으면 **그 패널이 등재한 의미 1개**(`routing` = 연결 해제 · `reader` = 인용 고정 · 나머지 4종 없음) |
| K-5 | **`LB` 는 모디파이어 전용**(단독 기능 없음). 모디파이어 판정은 **누른 순간 고정**(press-time latch) — `X` 를 누를 때 `LB` 가 눌려 있었으면 끝까지 되돌림이다 |
| K-6 | **입력 릴리스 래치**: 확정 입력은 **놓는 순간**에만 발행된다. 화면 전환 직후 눌린 상태가 유지돼도 확정이 발행되지 않는다(**T-27**) |
| K-7 | **표면 배달 우선순위**: `Overlay` > `ToolPanel(i)` > `Shell`. 입력은 가장 위 표면 **하나에만** 배달되며 아래로 흘러내리지 않는다. 도구 패널은 동시에 하나만 열린다 |
| **K-9** `[C7-F10]` | **오버레이 3진입점은 진짜 예외다**: `I`·`H`·`F1`(패드 `back`·`LB`+`back`·`LB`+`RS`)은 **어느 표면에서도 같은 일**을 한다. 도구 패널 안이라고 뜻이 바뀌지 않는다. 도구 패널의 조회는 **`Q`**(패드 `RS` 단독)라는 자기 키를 갖는다. 이전 판은 `I`·`H` 를 패널 안에서 조회로 재배정해 §0-11(한 입력 = 한 명령)을 깼다. `[C7-F35]` **`Q` 는 `Shell` 에서 명령 0개**이며 시점 노드 이동은 위 ⑦-1 첫 행(`Tab`/`Shift+Tab` + `Enter`)이다 — `data-schemas/zones.md` `neighbors` 는 그 초점 순회의 **순서**만 정하고 키를 배정하지 않는다. **`E` 는 어떤 표면에도 배정이 없다** |
| K-8 | **모든 바인딩 재매핑 가능**, 마지막 사용 장치에 따라 프롬프트 글리프가 즉시 바뀐다. 재매핑 UI 는 **한 입력에 두 명령이 걸리는 배치를 거부**하고 사유를 문장으로 표시한다 |

### ⑦-3 접근성 — T0 필수분 (`gdd.md` §8 중 T0 범위)

| 범주 | 옵션 | T0 판정 기준 |
|---|---|---|
| 입력 | 전 항목 키/버튼 재매핑 | 재매핑 불가 항목 **0개** |
| 입력 | 길게 누름 → 토글/2단계 전환 | **홀드가 유일 경로인 바인딩 0개** — 지속시간으로 명령이 갈리는 바인딩도 **0개**(§0-11 · C4-F20) |
| 입력 | **연속값 조작의 이산 대안** | `reader` 시간 범위(4분 스텝)·배율(1칸), `circuit` 오프셋(격자 1칸) 전건 존재 |
| 표시 | 텍스트 크기 3단 이상 · UI 배율 | 최소 단계에서도 잘림 **0건** |
| 표시 | **색 단독 금지 — 문자 라벨 병기** | 결정적 상태 전달에 색 단독 사용 **0건** |
| 표시 | 모션 축소 | 켜면 시점 전환이 **즉시 컷**, 상태 정보 손실 0 |
| 진행 | 무료 힌트 3단 · 무제한 되돌림 | 힌트로 해소되지 않는 진행 막힘 **0건** |
| 진행 | 실시간 압박 없음 | **전역 타이머 0개** |
| 접근 경로 | 접근성 설정을 **플레이 시작 전** 타이틀/부팅 화면에서 도달 | 선언한 입력 방식 전부로 도달 가능 |
| 언어 | 한국어 / 영어 | **미해결 로컬라이즈 키 0건**(T-12) |

T0 에서 **뒤로 미뤄도 되는 것**: 색약 대체 팔레트 3종(데이터만 준비), 채널별 볼륨, 전체 자막(T0 에 음성이 없다면 해당 없음 — 있으면 필수).

---

## ⑧ Presentation — 2.5D 고정 노드 카메라 · 스냅샷 읽기 전용 · 그레이박스

### ⑧-1 카메라 수치 [OBSERVED · `concept/style-guide.md` §5]

| 항목 | 값 |
|---|---|
| 시점 | **고정 관찰점(노드)만.** 카메라 이동·팬·줌 **없음**. 장면 전환은 노드 컷 |
| 렌즈 | **35 mm 상당 · 수평 화각 약 54°**, 배럴 왜곡 0 |
| 시선 높이 | **1.55 m** |
| 피치 | **−18°**(내려봄) · 롤 **0°** · 더치 앵글 금지 |
| 요 | 노드별 **0° 또는 ±30°** 고정 |
| 노드 수 | 구역당 **6~10개** `[TARGET]`(`zones.md` `viewNodes`) |
| 전환 | `cut` \| `dolly`. **모션 축소 옵션이 켜지면 전부 `cut`** |
| 이동 캐릭터 | **없다.** `CharacterController`·NavMesh·경로탐색·충돌 이동을 쓰지 않는다. 로코모션 클립 0개 |

카메라 포즈는 **데이터**(`ViewNode.cameraPose {pos, rot, fov}`)에 산다. 씬에 손으로 놓은 카메라가 데이터와 다르면 데이터가 이긴다.

### ⑧-2 sim/render 계약 (CLAUDE.md §9 불변식)

| 규칙 | 내용 |
|---|---|
| R-1 | Presentation·UI 는 **`WorldSnapshot` 만** 받는다. 스냅샷은 불변이며 `seq` 와 `stateHash` 를 포함한다 |
| R-2 | 렌더는 Sim 에 쓰지 않는다. 사용자 조작은 **Intent → Command 제출** 경로만 존재한다 |
| R-3 | `Time.deltaTime` 은 보간·연출에만. Sim 에 시간이 들어가지 않는다 |
| R-4 | 스냅샷 `seq` 가 UI 가 마지막으로 본 `seq` 보다 낮으면 UI 는 **stale 처리**하고 확정 버튼을 비활성화한다 |
| R-5 | Sim 은 표시 문자열을 만들지 않는다. `ReasonCode` → 로컬라이제이션 테이블 → UI |

스냅샷 타입에 **세터·컬렉션 변경 메서드를 노출하지 않는다.**

### ⑧-3 그레이박스 GLB 임포트

- 출처: `assets/generated/3d/`(Blender 로 저작) — `hub-greybox.glb` · `SM_Tool_{circuit,reader,alignment,routing,corrosion,seal}.glb` [OBSERVED].
- 동반 파일: `assets/generated/3d/provenance.json` — 모든 항목이 **`runtimeEligible: false`** 이고 `promoted_by: null` [OBSERVED].
- **임시 사용 규칙**: T0 그레이박스로 쓰되 임포트한 프리팹·머티리얼·에셋 이름에 **`placeholder` 태그를 붙인다**(예: `SM_Hub_Greybox_placeholder`). 이것은 **승격이 아니다.**
- **승격(`runtimeEligible: true`)은 `production/decision-log.md` 감사로만** 일어난다. 실행자가 `provenance.json` 을 편집하지 않는다.
- 에셋·오브젝트 이름의 구역 토큰은 `zoneId` 영문 토큰(`hub` 등)을 그대로 쓴다(worldview OPEN-M1 판정). 용어집에 없는 KO/EN 고유명사와 **상표 미확인 가제**를 파일명·오브젝트명·이미지 내 텍스트에 넣지 않는다.

---

## ⑨ Telemetry — 키 이름과 정직성 규율 (`systems/ops/telemetry-contract.md`)

### ⑨-1 전송·저장

| 항목 | 값 |
|---|---|
| 원격 전송 | **없음.** 서버·클라우드·분석 SDK 0건 |
| 저장 | 로컬 `telemetry/session-{uuid}.jsonl` `[TARGET]` |
| 개인정보 | 이름·계정·IP·하드웨어 식별자 저장 **금지**. 세션 id 는 임의 UUID |
| 기본값 | 정본 빌드에서 **꺼짐**. 플레이테스트 빌드에서만 켠다 |

### ⑨-2 키 (T0 에서 수집할 것)

| 축 | 키 | 성격 |
|---|---|---|
| 시간(설계) | `design_budget_min` = 480 · `design_fast_min` = 322 · `design_deliberate_min` = 673 | **문서 상수** — 실측과 **같은 축에 그리지 않는다** |
| 시간(실측) | `total_min` · `afk_total_min` · **`total_minus_afk_min`(판정 키)** · `observed_completion_min` | 현재 전부 값 없음(n=0) |
| 진행 | `beat_reached {beat_id, t}` · `beat_completed {beat_id, t, duration_sec}` · `stage_completed` · `beat_revisit_count` | |
| 힌트 | `hint_used {level, beat_id, t}` · `hint_offer_shown` · `hint_offer_dismissed` · `time_to_first_hint_sec` · **`stuck_after_l3`(0이어야 한다)** | |
| 연습/확정 | **`sandbox_time_min`** 과 **`commit_time_min`** 은 **분리 보고**한다 · `sandbox_discarded` · **`undo_count`** · `redo_count` · `branch_created` · `checkpoint_created`/`checkpoint_loaded` · **`command_count`** · **`entries_count`** | `command_count`/`entries_count` 는 2026-09-10 R7 에 계약 §4 에 신설됐다 [C7-F11]. `save_file_bytes / entries_count` 로 `entryCap` 을 재파생한다 [C7-F7] |
| 도구 | `circuit_open_count` · `circuit_trace_count` · `circuit_overlay_attempts` · `circuit_uncovered_marked` · `circuit_out_of_coverage_blocked` · `circuit_time_min` / `read_count_total` · `read_budget_exhausted` · `auto_copy_created` · `citation_count` · `indeterminate_shown` · `reader_time_min` · `alt_path_offered` | **출처 = `telemetry-contract.md` §4.1**(2026-09-10 R7 신설). 그 전에는 이 키들이 계약에 **0건**이라 `tools.md` `T-I6` 가 T0 도구 2종 임포트를 막았을 것이다 [C7-F11] |
| **퍼즐 구간** | **`puzzle_enter`** · **`first_valid_action`** · **`clue_seen`** · **`hypothesis_preview`** · **`confirm`** · **`exit`** (6구간 타임스탬프) | `telemetry-contract.md` §2 · `balance/puzzle-balance.md` 소유. **H-1 판정의 핵심 키가 `first_valid_action`** 인데 이전 판 브리프에 **0건**이었다 [C7-F11]. 6개를 한 벌로 기록한다 — 하나라도 빠지면 구간 분해가 성립하지 않는다 |
| **엔딩** | `ending_reached` (enum) | T0 범위 밖(엔딩 없음). 키만 정의하고 발행하지 않는다 |
| AFK | `afk_gap {start_t, duration_sec, beat_id, last_input, verdict: null}` | **`verdict` 는 수집 시점에 항상 `null`** |

### ⑨-3 정직성 규율 (코드에도 반영한다)

- **`design_*` 와 `observed_*` 를 같은 필드에 쓰지 않는다.** 같은 표에 넣을 때는 열 이름에 출처를 붙인다.
- `afk_gap` 은 60초 이상 무입력 구간을 **기록(marking)** 할 뿐 **자동으로 제외하지 않는다**. 제외는 **세션 후 회고**로 확인된 구간만(`verdict: away`). **코드가 `verdict` 를 스스로 채우는 경로를 만들지 않는다.**
- 보고는 항상 `total_min` · `afk_total_min` · `total_minus_afk_min` **세 값을 함께** 낸다.
- `design_budget_min` 을 "예상 플레이 시간"으로 부르지 않는다. `design_fast_min` ~ `design_deliberate_min` 을 "분포"·"신뢰구간"으로 부르지 않는다.
- **`hw_profile_id` 가 비어 있으면 성능 결과를 게이트에 쓰지 않는다.**

---

## ⑩ 인수 테스트 · 실행 명령 · DoD · 금지 목록

### ⑩-1 T-01~T-27 → EditMode / PlayMode 매핑

| id | 단언 | 플랫폼 | T0 범위 |
|---|---|---|---|
| T-01 | `Reduce` 는 입력 상태를 변경하지 않는다(해시 불변) | EditMode | **필수** |
| T-02 | 같은 이벤트 열 재적용 시 동일 상태 해시(결정론) | EditMode | **필수** |
| T-03 | `Preview` 2회 호출 후 상태 해시 동일(부작용 없음) | EditMode | **필수** |
| T-04 | `Validate` 실패 시 `Commit` 은 이벤트 0개를 반환한다 | EditMode | **필수**(스텁 4도구 포함) |
| T-05 | 무작위 10k 스텝 후에도 `AutoKeptClues` 가 축소되지 않는다 | EditMode | **필수** |
| T-06 | 임의 도달 상태에서 엔딩 3종 각각에 도달 가능 | EditMode | T0 범위 밖(엔딩 없음) → **보류** |
| T-07 | 소프트락 없음 = 모든 도달 상태에 진행 가능한 확정 경로 ≥ 1 | EditMode | **필수**(T0 상태공간에 한정) |
| T-08 | 부분 기록 저장 주입 시 `.bak` 으로 복구하고 정본을 덮어쓰지 않는다 | PlayMode | **필수** |
| T-09 | `schemaVersion=0` 저장이 마이그레이션 후 v1 을 만족한다 | PlayMode | **필수**(픽스처로) |
| T-10 | `schemaVersion=2` 저장은 거부되고 **파일 바이트가 변하지 않는다** | PlayMode | **필수** |
| T-11 | 임포트 검증기가 불변식 위반 픽스처 **5종을 전부 실패시킨다** | EditMode | **필수** |
| T-12 | 로컬라이제이션 미해결 키 0건 | EditMode | **필수** |
| T-13 | 되돌림 포인터를 0까지 내린 뒤 재적용하면 원래 해시로 복귀 | EditMode | **필수** |
| T-14 | `propertyProtection` 두 값에서 필수 단서·엔딩 접근성 동일 | EditMode | T0 범위 밖 → **보류** |
| T-15 | **rename 직전 쓰기 실패 주입** → 상태·디스크가 확정 이전이고 성공 연출이 재생되지 않는다 | PlayMode | **필수** |
| T-16 | **중간 쓰기 실패 주입**(임시 파일 절단) → 정본 바이트 불변, 복구 패널 경로 제시 | PlayMode | **필수** |
| T-17 | **재시도 멱등성**: 같은 `commitIdempotencyKey` 로 2회 성공해도 이벤트가 한 번만 적용된다 | PlayMode | **필수** |
| T-18 | **지연 완료**: 취소된 커밋의 늦은 성공 콜백이 UI·상태를 바꾸지 않는다 | PlayMode | **필수** |
| T-19 | **rename 후 크래시 재현**: 재시작 시 새 세대를 이어받고 커밋이 중복 적용되지 않는다 | PlayMode | **필수** |
| T-20 | `SavePending` 중 확정만 비활성이고 되돌림·설정·힌트 입력은 계속 처리된다 | PlayMode | **필수** |
| T-21 | 원본 + 파생 스캔(라벨만 다름) 쌍은 `copiedFrom` 루트 해석으로 **거부**된다 | EditMode | **필수** |
| T-22 | 서로 다른 `originId` 의 염판 2점은 `sourceType` 중복으로 **의도적으로 거부**되고 사유가 구분 표기된다 | EditMode | **필수** |
| T-23 | `proofRequired` 가 아닌 판독·경로 프리뷰는 서명 없이 진행 가능하다 | EditMode | **필수** |
| T-24 | **키보드 단독 전 퍼즐 완주**: T0 퍼즐 전체를 포인터·홀드 없이 완료(화면 도달이 아니라 **퍼즐 종료**로 판정) | PlayMode | **필수** |
| T-25 | 확정 방식 3값(`two-step`/`hold`/`confirm-dialog`) 각각으로 같은 퍼즐 완주 | PlayMode | **필수** |
| T-26 | **`[C7-F9 재기술]`** 도구 패널이 열린 동안 `X` 는 그 패널이 `interaction-rules.md` §1-3.2 에 **등재한 부작용 없는 실행 1개**만, `Y` 는 **등재한 의미 1개**만 수행한다. 등재가 없으면 **명령 0개**. 동시 활성 0건 | PlayMode | **필수** |
| **T-26a** | **T0 실체화 · `circuit`**: `X` = 구획 접기 토글(`ToggleUncovered`) · `Y` = **등재 없음 → 명령 0개**. 「프리뷰」도 「해제」도 발행되지 않는다 | PlayMode | **필수** |
| **T-26b** | **T0 실체화 · `reader`**: `X` = 사본 재생(`Read`, 부작용 0 · `ReadOriginal` 이 아니다) · `Y` = 인용 고정(`CiteToBoard`) | PlayMode | **필수** |
| **T-26c** | **`Q` 표면 스코프** `[C7-F10]` `[C7-F35]`: `Q` 는 `ToolPanel(circuit)`·`ToolPanel(alignment)` 에서만 조회를 발행하고 `Shell`·`Overlay` 에서는 **명령 0개**. `I`·`H` 는 어느 표면에서도 증거함·가설판이다. **추가**: `Shell` 에서 `Q` 또는 `E` 를 눌러도 **시점 노드가 이동하지 않으며**, 노드 이동은 `Tab`/`Shift+Tab` 초점 + `Enter`(패드 좌스틱 + `A`)로만 발생한다. `neighbors` 는 그 초점 순서를 정할 뿐이다 | PlayMode | **필수** |
| T-27 | 화면 전환 직후 눌린 상태가 유지돼도 확정이 발행되지 않는다(릴리스 래치) | PlayMode | **필수** |

**추가(이번 브리프에서 신설 — 실행자가 만든다)**

| id | 단언 | 플랫폼 |
|---|---|---|
| **T-B1** | `Tide.Sim` 이 `UnityEngine` 을 참조하면 **컴파일 실패**한다(asmdef `noEngineReferences` + 배치 빌드) | 빌드 |
| **T-IMP-1** | **영수증 대조** `[C6-F13 재정의]`: `tables-receipt.json` 의 `verdict == PASS` 이고 `Data/Tables/*.json` 의 sha256 이 영수증과 **전건 일치**한다. 한 바이트라도 다르면 임포트 실패. (이전 판의 "임포터↔검증기 동치"는 재구현을 전제했으므로 폐기) | EditMode |
| **T-IMP-1b** | **변조 픽스처**: `beats.json` 을 1바이트 고친 사본 → `V-2` 실패로 임포트가 멈춘다 | EditMode |
| **T-IMP-2** | **생성 단계 fail-closed**: 위반 픽스처 (a) `zoneId` 가 스테이지 밖 (b) `copiedFrom` 순환 (c) `proofRequired` 인데 독립쌍 없음 (d) `prerequisites` 고아 (e) `hintCost != 0` — `emit-tables.mjs` 가 **5종 전부 exit 1 로 거부하고 테이블을 만들지 않는다**. (a) 는 2026-09-10 에 실제로 확인됨 [OBSERVED] | Node(Unity 밖) |
| **T-IMP-3** | 런타임 전용 `R-1`~`R-4` 각각의 위반 픽스처가 임포트를 실패시킨다. **`R-1` 은 EN 문자열이 생기기 전까지 실패가 정상이다**(숨기지 않는다) | EditMode |

- 저장 실패·중간 실패·크래시는 **주입(injection)으로만 재현**한다. 실제 디스크를 채우거나 장치를 손상시키는 테스트를 만들지 않는다.
- 인수는 **공개 상태 단언**으로 한다. **로그 문자열 검색으로 대체하지 않는다.**

### ⑩-2 실행 명령 (배치모드)

```bash
UNITY=/Applications/Unity/Hub/Editor/6000.5.6f1/Unity.app/Contents/MacOS/Unity
PROJ=<repo>/unity/Unknown
OUT=<repo>/_workspace/current/systems/tech-verification

# 0) 패키지 부트스트랩 — 필수 5종 추가 + multiplayer.center 제거  [C7-F2 · C6-F11 ③]
#    이것을 건너뛰면 아래 2)·3) 의 -runTests 는 test-framework 가 없어 실행되지 않는다.
#    선행 조건: Assets/_Project/Editor/PackageBootstrap.cs 가 존재할 것 (§③-1a)
"$UNITY" -batchmode -nographics -projectPath "$PROJ" \
  -executeMethod PackageBootstrap.Run \
  -logFile "$OUT/logs/t0-packages.log"
#    확인(하드 게이트): 출력이 정확히 `5` 여야 다음 단계로 간다 (5/5 아니면 중단)  [C7-F33]
#    grep -c 는 중첩 dependencies 때문에 과다 계수한다 → **최상위 키만** 센다
node -e 'const j=require(process.argv[1]);const need=["com.unity.inputsystem","com.unity.ugui","com.unity.localization","com.unity.test-framework","com.unity.render-pipelines.universal"];const have=need.filter(k=>k in j.dependencies);console.log(have.length);if(have.length!==need.length){console.error("missing:",need.filter(k=>!(k in j.dependencies)).join(","));process.exit(1)}' \
  "$PROJ/Packages/packages-lock.json"
#    제거 확인: 출력이 `false` 여야 한다
node -e 'const j=require(process.argv[1]);console.log("com.unity.multiplayer.center" in j.dependencies)' \
  "$PROJ/Packages/packages-lock.json"

# 0b) 런타임 테이블 생성 — 저작 원본 → Data/Tables + 영수증  [C6-F13]
#     검증기 verdict != PASS 면 여기서 멈춘다(fail-closed). 임포터는 저작 시점 검사를 재구현하지 않는다.
node <repo>/_workspace/current/systems/pipeline/emit-tables.mjs \
  --out "$PROJ/Assets/_Project/Data/Tables"

# 1) 프로젝트 열기 / 패키지 resolve  → packages-lock.json 의 실제 버전을 문서로 옮긴다
"$UNITY" -batchmode -quit -nographics -projectPath "$PROJ" \
  -logFile "$OUT/logs/t0-open.log"

# 2) EditMode (Sim 순수 로직 · 임포트 검증)
"$UNITY" -batchmode -runTests -nographics -projectPath "$PROJ" \
  -testPlatform EditMode \
  -testResults "$OUT/results/editmode.xml" \
  -logFile "$OUT/logs/editmode.log"

# 3) PlayMode (세이브·복구·입력)
"$UNITY" -batchmode -runTests -projectPath "$PROJ" \
  -testPlatform PlayMode \
  -testResults "$OUT/results/playmode.xml" \
  -logFile "$OUT/logs/playmode.log"

# 4) 데이터 임포트 검증 (fail-closed 확인)
"$UNITY" -batchmode -quit -nographics -projectPath "$PROJ" \
  -executeMethod Tide.Data.Import.ValidatorCli.Run \
  -logFile "$OUT/logs/import.log"

# 5) 프레임타임 캡처 — 기준 PC(hw_profile_id) 확정 후에만 유효
"$UNITY" -batchmode -projectPath "$PROJ" \
  -executeMethod Tide.Tests.Perf.CaptureCli.Run \
  -captureScenes hub -captureSeconds 120 \
  -out "$OUT/results/frametime-{hw_profile_id}.csv"

# 6) 저작 데이터 회귀 (Unity 밖, Node 내장 모듈만)
node <repo>/_workspace/current/planning/validate-campaign.mjs
```

- **캡처 씬은 `hub` 하나다.** `gate` 는 T0 에 없다(첫 등장 C1 `c1-b1`) [OBSERVED].
- 실패한 실행도 남긴다. 성공한 실행만 남기면 그것은 기록이 아니라 광고다.

### ⑩-3 완료 정의 (DoD) — 전부 충족해야 "T0 코드 완료"다

1. `Assets/_Project/` 에 **생산 어셈블리 7개**(`Sim/Data/Save/Input/Presentation/UI/App` · RFC-S2 판정) + 테스트 2 + `EditorTools` 1 = asmdef **파일 10개** 생성, **`Tide.Sim` 이 엔진 참조 0** (`T-B1` 통과).
2. `com.unity.multiplayer.center` **제거**, 필수 패키지 **5종**(입력·UI·로컬라이제이션·테스트 프레임워크·**URP**) 추가(**절차 = §③-1a**, 현재 **0/5** [OBSERVED]), `packages-lock.json` 의 **최상위 `dependencies`** 에 5종 존재 확인(세는 명령은 §⑩-2 #0 · C7-F33), `[PIN-AFTER-RESOLVE]` 를 **실제 버전으로 교체**한 커밋 가능한 diff. `com.unity.addressables` 가 전이 의존으로 딸려 오는 것은 허용(N-10 예외).
3. `activeInputHandler` 를 **`2`(Both)** 로 전환하고(§⑦-0 ②), `Actions.inputactions` 에 **액션 맵 `Watch`** 와 §⑦-1 전건이 들어 있으며 **재매핑이 실제로 동작**한다.
4. `hub` 씬 + `boot` + `ui-root` 가산 로드로 `t0-b1 → t0-b2 → t0-b3` 을 **처음부터 끝까지 플레이 가능**하다.
5. `circuit`·`reader` 가 §⑤-3 의 상태기계와 확정 조건을 구현했고 나머지 4도구는 스텁으로 존재한다. **실패 사유는 `ReasonCode` 로 판정한다** `[C7-F14 완화 2026-09-10 R7 종료]` — 인수 조건은 **(a) 코드의 `ReasonCode` enum 집합이 §⑤-7 표의 집합과 일치**하고 **(b) `Assets/_Project/Localization/strings/ko.json` 이 존재하며 그 키 집합이 같을 것**, 둘뿐이다. **KO 문장의 문안 일치는 요구하지 않는다** — §⑤-3 의 문장은 `[TARGET] 자리표시자`이며 최종 문안은 용어집 등재(RFC-S6) 이후다. 이전 판의 「실패 문장을 그대로 구현」은 발명 문자열을 캐논처럼 굳히므로 철회한다. `strings/en.json` 은 T0 에서 만들지 않는다.
6. 저장 4갈래 + 원자적 rename + `SavePending` + 실패 롤백이 동작하고 **T-08~T-10 · T-15~T-20 이 `reader` 의 `CiteToBoard`(T0 의 확정 명령 · §⑤-3(B) · RFC-C7-001) 위에서** 통과한다 `[C7-F8 재작성]`. 이전 판은 T0 에 확정 명령이 0개라고 적힌 채 확정·롤백 테스트를 요구해 **검사 대상이 없는 요구**였다. 확정이 일어나는 지점은 **`t0-b3` 하나**이며(`beats.json` `commitCommandBeats`), `t0-b1`·`t0-b2` 에서는 이 테스트들을 돌리지 않는다.
7. **생성기 → 영수증 → 임포터** 사슬이 선다: `emit-tables.mjs` 가 `Data/Tables/` 를 만들고(§⑩-2 #0b), 임포터가 `V-1`~`V-4`(영수증 대조)와 `R-1`~`R-4`(런타임 전용)를 fail-closed 로 강제하며 `T-IMP-1`·`T-IMP-1b`·`T-IMP-2`·`T-IMP-3` 이 통과한다. **저작 시점 검사(검증기 `summary.checks`)를 C# 으로 재구현하지 않았다**(C6-F13). 그 수는 planner 가 검사를 더할 때마다 바뀌므로 문서에 상수로 적지 않는다. 단 `R-1`(로컬라이즈)은 EN 문자열이 없어 **현재 통과 불가**이며 그 사실을 보고서에 적는다.
8. **키보드 단독 완주(T-24)** 와 **확정 3방식 완주(T-25)** 가 통과한다 — 여기서 "완주"는 **`beats.json` 의 `completionPredicate` 3건이 모두 충족되는 것**이며 화면 도달이 아니다 `[C7-F8 재작성]`. `T-25` 의 세 값(`two-step` / `hold` / `confirm-dialog`)은 **`CiteToBoard` 에 적용**된다(T0 의 유일한 확정). 홀드는 opt-in 이므로 기본 경로는 `two-step` 이고, 세 값 모두로 `t0-b3` 이 끝나야 한다.
9. 텔레메트리 키가 §⑨-2 이름 그대로 로컬 jsonl 로 기록되고 `afk_gap.verdict` 는 항상 `null` 이다. **`ToolAsset.telemetryKeys` 전건이 `telemetry-contract.md` 에 정의돼 있어 `T-I6` 가 통과한다**(도구 키 = §4.1) [C7-F11].
10. `systems/tech-verification/t0-*.md` 에 **명령 + 관측 결과**가 있고 `results/*.xml` · `logs/*.log` 원본이 함께 있다.
11. `graphify update .` · `zg` 인덱스 갱신 영수증이 §⑪ 형식으로 남아 있다.
12. **사람 검증(`verification-plan.md`)은 아직 시작되지 않았다** — DoD 는 "검증 준비 완료"까지다.

### ⑩-4 금지 목록 (하나라도 어기면 REDO)

| # | 금지 | 이유 |
|---|---|---|
| N-1 | **가제 문자열**("조수기록국" / "TIDE ARCHIVE" 등)을 폴더명·번들명·`productName`·상점명·이미지 내 텍스트에 넣기 | 상표·동명 조사 전 |
| N-2 | 생성 에셋의 **`runtimeEligible` 을 `true` 로 승격** | decision-log 감사 사항 |
| N-3 | **밸런스·경제 숫자 하드코딩**(9 · 3 · 4 · 180 · 0.4 · 7 · 8 · 12 등) | 데이터 테이블 소유 |
| N-4 | **전역 실시간 타이머** 도입, 또는 읽는 속도로 벌하는 장치 | §0-1 불변식 |
| N-5 | **유료 재화·구매·과금 UI**, 외부 API·네트워크·분석 SDK | 본작에 없다 |
| N-6 | 세이브 지속 필드 **개명**(마이그레이터 없이) | CLAUDE.md §9 |
| N-7 | 렌더·UI 에서 **Sim 상태 쓰기** | CLAUDE.md §9 |
| N-8 | `git commit` / `git push` / `git add -A` | 사용자 소유 |
| N-9 | 임포트 검증을 **경고로 통과** | fail-closed 계약 |
| N-10 | 문서화되지 않은 **선택 패키지 추가**, 유료 에셋 도입 | RFC 로 되묻는다. **예외 [C7-F2]**: 필수 4종(`inputsystem`·`ugui`·`localization`·`test-framework`)은 §③-1a 절차로 **추가하는 것이 지시**이며 금지 대상이 아니다. `com.unity.addressables` 가 `com.unity.localization` 의 **전이 의존**으로 `packages-lock.json` 에 나타나는 것도 허용한다 — 전이 의존과 채택은 다르며, Addressables **API 를 직접 쓰는 것**은 여전히 RFC 대상이다 |
| N-11 | 프리비즈 이미지·영상을 **"게임플레이"로 표기** | 계약 Honesty gates |
| N-12 | 되돌림 **횟수 상한**(`maxUndo` 류) 도입, 또는 되돌림에 비용·페널티 부여 | U-3. **로그 크기 상한(U-8 `byteCap`/`entryCap`)은 금지 대상이 아니다** — 그것은 접힘 규칙(`SV-F6`)이지 되돌림 제한이 아니다 |

---

## ⑪ 검증 보고 양식

### ⑪-1 파일 규약

- 파일: `_workspace/current/systems/tech-verification/t0-{주제}.md` — frontmatter(`updated` / `cycle` / `status` / `supersedes` / `owner`) 필수.
- 본문은 **명령 + 관측 결과**를 나란히 적는다. 요약만 쓰고 원본을 남기지 않는 것은 금지.
- 원본 산출물: `results/*.xml` · `*.csv` · `logs/*.log` 를 같은 폴더에 둔다. **파일이 없으면 측정이 없었던 것이다.**
- 비-Markdown 산출물은 같은 basename 의 `.meta.md` 를 갖는다(CLAUDE.md §10).

### ⑪-2 최소 서식

```markdown
## 0. 이번 실행이 주장하는 것 / 주장하지 않는 것
- 주장: (예) EditMode 47 케이스 통과
- 주장하지 않음: 재미 · 플레이 시간 · 성능 판정(기준 PC 미정)

## 1. 명령과 결과 [OBSERVED YYYY-MM-DD]
| # | 명령 | 결과 |
|---|---|---|
| 1 | `"$UNITY" -batchmode -runTests …` | `Tests: 47, Passed: 47, Failed: 0` · exit 0 · `results/editmode.xml` |

## 2. 실패한 실행 (있으면 반드시)
## 3. 그래프·검색면 갱신 영수증
| 도구 | 명령 | 결과 |
|---|---|---|
| graphify | `graphify update .` | (출력 요약) |
| zg | `zg index --rebuild` 또는 `zg status` | (출력 요약) |
| mex | (이번 회차 정책에 따름 — 미실행이면 `[SKIPPED: 사유]`) | |

## 4. 이 실행이 올린 게이트
- 없음 / G6 입력 일부 …  ← 올리지 않았으면 "없음"이라고 적는다
```

### ⑪-3 코드 작업 순서 (시스템 레인 규칙 — 실행자에게도 적용)

```
mex graph scope "<과제>"            (도구가 없거나 정체 불명이면 [SKIPPED: 사유] 를 남기고 건너뛴다)
  → zg query "<의도>"  /  zg query --rg -F "<심볼>"
  → graphify query "<영향 흐름 질문>"
  → 편집
  → graphify update .
  → mex graph && mex check         (같은 정책)
  → mex log "<한 줄>"
```

- **파일 전체를 열어 탐색하지 않는다.** 검색 → 인용된 줄 읽기 → 편집.
- **그래프가 모르는 변경은 미완이다.** 코드 변경이 있었는데 `graphify update .` 영수증이 없으면 그 작업은 끝나지 않았다.
- 도구가 없으면 `[UNGRAPHED]` 를 검증 파일에 적고 디렉터에게 알린다. 조용히 건너뛰지 않는다.

---

## ⑫ English summary — the 9 things that matter

1. **Scope.** Build one vertical slice only: hub zone, two tools (`circuit` = circuit trace, `reader` = plate read), three beats `t0-b1/b2/b3`, **25 design minutes**. Everything else (4 tools, 4 zones, 33 beats, endings, DLC) is out of scope until a human playtest of T0 passes.
2. **Sim is engine-free.** Assembly `Tide.Sim` has zero `UnityEngine` references (`noEngineReferences: true`); rendering and UI read an immutable `WorldSnapshot` and never write sim state. Player input becomes an Intent, then a Command. This is enforced by asmdef, not by convention.
3. **Command sourcing — the persisted replay unit is the command, not the event.** `CommandEntry` carries a normalized `payload`; events are what `Commit` derives and are **never written to the save file**. This requires `Commit` to be pure: same `(state, commandId, payload)` always yields the same event sequence. State changes only through `Reduce(state, evt)`. Undo lowers `headSeq`; it never deletes. **There is no undo limit** — the prototype's `maxUndo: 32` is a debugging constant, not a spec. Rehearsal (`Fork`) and commit are two layers; discarded rehearsals are never saved. Log caps were re-derived: **6 MiB `byteCap` or 20,000 `entryCap`, whichever comes first** — the old "50,000 / 8 MB" pair contradicted itself once payloads were stored.
4. **Data owns the numbers.** All tuning values (corrosion limit 9, read budget 3, residual 4 minutes, hint idle 180 s, hold 0.4 s, route costs 7/8/12) live in tables. Code exposes knobs and holds no tuning literals. Package versions are `[PIN-AFTER-RESOLVE]` — copy the resolved values, never invent them.
5. **Saves are atomic and never renamed.** Write temp → fsync → rotate `save.bak` → `rename`. Four distinct files (`save.json`, `checkpoint.pre-commit.json`, `save.bak`, 3 manual slots). Higher or unknown `schemaVersion` is refused without touching the file. **Renaming a persisted field orphans player saves — write a migration or refuse.**
6. **Keyboard-only must complete every puzzle.** Two-step confirm is the default; hold is opt-in. `X`/`Space` is reserved for side-effect-free execution; surface delivery order is Overlay > ToolPanel > Shell, with one tool panel open at a time. **`Q` is the tool-panel lookup key and nothing else** — it issues a command only inside `ToolPanel(circuit)` and `ToolPanel(alignment)`, and issues **no command at all** on the `Shell` and `Overlay` surfaces. View-node movement is `Tab`/`Shift+Tab` focus + `Enter` (pad: left stick + `A`); it is **not** `Q`/`E`, and `E` is bound on no surface. `zones.md`'s `neighbors` field fixes only the *order* of that focus traversal — data schemas own ordering, never key bindings. Acceptance row: `matrix[19]` / `T-26c` (**not yet run**).
7. **Import is fail-closed — and you do NOT re-implement the validator.** The rules live in exactly one place: `planning/validate-campaign.mjs`. Run `systems/pipeline/emit-tables.mjs --out <proj>/Assets/_Project/Data/Tables` first; it calls the validator, refuses to emit anything unless the verdict is PASS, and writes `beats.json` (a **byte-identical copy** of the authoring source), `hints.json` (99 rows) and `tables-receipt.json`. The C# importer verifies the receipt hashes (`V-1`~`V-4`) and re-implements **only the four runtime-only invariants** (`R-1` localization, `R-2` orphans, `R-3` indestructible path, `R-4` ending reachability). Two copies of a rule set will diverge; one copy cannot. Warnings never pass.
8. **Packages are 0/4 installed right now.** `manifest.json`'s only non-module dependency is `com.unity.multiplayer.center`, so `-runTests` will not run until `com.unity.test-framework` exists. Add the four required packages with a **version-less** `Client.AddAndRemove` from an Editor bootstrap (§③-1a), then copy the resolved versions out of `packages-lock.json` — never invent a version. `com.unity.addressables` arriving as a transitive dependency of `com.unity.localization` is explicitly allowed; adopting its API is not.
9. **Honesty rules apply to code and reports.** Telemetry is local-only, never transmitted; `design_*` constants and `observed_*` measurements never share a field; AFK gaps are marked but never auto-excluded (a human retrospective fills `verdict`). No build, no playtest, and no frame capture exists yet — do not write any report sentence that implies otherwise. **Anything this brief does not decide must come back as an RFC (see `handoff/README.md` §2), not as a guess.**
