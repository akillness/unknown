---
updated: 2026-09-10
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
---

# 기술 검증 — C6/C7 수정 루프 1 (systems 레인 결함 7건)

**Unity 실행 0회 · 빌드 0회 · 프레임타임 캡처 0건 · 사람 플레이 표본 n = 0 · 패드 실측 0건.**
이 문서의 어떤 값도 게임 측정치가 아니며 **어떤 게이트도 PASS 로 올리지 않는다.** 여기 적힌 것은 전부 **문서·데이터 정합 재측정**과 **Node 스크립트 실행 결과**다.

## 0. 배정과 처리

| 결함 | 심각도 | 상태 | 한 줄 |
|---|---|---|---|
| C6-F13 | S2 | **해소** | 저작 원본 → 런타임 테이블 파이프라인을 **실행되는 생성기**로 정의하고 `mediaType` → `sourceType` 통일 |
| C7-F2 | S2 | **해소** | 필수 패키지 4종 추가 절차(§③-1a) 신설 + `addressables` 전이 의존을 N-10 예외로 등재 |
| C7-F6 | S2 | **해소** | `Tables/`↔`Authoring/` 형태를 한 벌로, 씬·아트를 `_Project/` 안으로, 런북 §3.2 정정 |
| C7-F7 | S2 | **해소** | 재생 단위 = **명령 소싱** 확정, `payload`·`commitIdempotencyKey`·`byteCap` 신설, 상한 재산정 |
| C7-F9 | S2 | **해소** | T-26 재기술 + T0 실체화 2행 + 정본 3곳(§11 · 브리프 · JSON) 동기화 |
| C7-F10 | S2 | **해소** | 도구 패널 조회를 **`Q`** 로 분리해 `I`·`H` 겸용 소멸 |
| C7-F11 | S2 | **해소** | `telemetry-contract.md` **§4.1 도구 키 절** 신설 + `command_count`/`entries_count` 정의 + 브리프에 퍼즐 6구간 키 추가 |

**QA 가 확인해야 하는 것**: 아래 §1 명령을 그대로 다시 돌려 같은 출력이 나오는지, 그리고 §3 의 "해소되지 않은 것"이 정직한지.

## 1. 실행한 명령과 관측 결과 [OBSERVED 2026-09-10]

### 1.1 저작 원본 검증기 (규칙의 단일 출처)

```
$ node _workspace/current/planning/validate-campaign.mjs
```
`summary { checks 47, pass 47, fail 0, verdict PASS }` · `stages 9` · `beats 33` · `clues 73` · `totalMinutes 480` · `sourceTypeDist { log 27, ledger 22, plate 24 }` · `toolBeatCounts { circuit 10, reader 11, alignment 8, routing 3, corrosion 3, seal 7 }` · `proofRequiredBeats 15` · `zoneBeatCounts { hub 15, gate 3, pump 6, dock 5, lowland 4 }`.
sha256·바이트 수는 **여기 옮겨 적지 않는다** — 필요할 때 검증기를 다시 돌려 `sha256` 필드를 읽는다(RFC-Q1).

### 1.2 신설 생성기 — 드라이런 · 실제 생성 · fail-closed 픽스처

```
$ node _workspace/current/systems/pipeline/emit-tables.mjs                       # 드라이런: 쓰기 0건
$ node _workspace/current/systems/pipeline/emit-tables.mjs --out <scratch>/tables-test
$ shasum -a 256 _workspace/current/planning/campaign.json <scratch>/tables-test/beats.json
$ node .../emit-tables.mjs --source <scratch>/broken.json --out <scratch>/broken-out   # Z-01 위반 픽스처
```

| # | 관측 | 값 |
|---|---|---|
| 1 | 드라이런 | exit 0 · 영수증 JSON 을 stdout 으로 출력 · **파일 0건 생성** |
| 2 | 실제 생성 | `beats.json` 121,457 B · `hints.json` 40,724 B · `tables-receipt.json` 2,491 B |
| 3 | **바이트 동일 사본 증명** | `shasum` 두 줄이 **같은 해시**를 출력 — `sha256(beats.json) == sha256(campaign.json)`. 이것이 `beats.md` M2 「변환 계층 0」의 기계 증거다 |
| 4 | `hints.json` 행 수 | **99** (33비트 × 3단) |
| 5 | **fail-closed** | `t0-b1.zoneId` 를 `lowland`(스테이지 밖)로 바꾼 사본 → 검증기 `Z-01` FAIL → 생성기 `exit 1` · **출력 디렉터리 미생성**(`ls: No such file or directory`) |

**이번 회차에 `unity/Unknown/Assets/` 에 쓴 파일은 0건이다.** 출력은 스크래치 디렉터리뿐이며 레인 밖 쓰기 금지를 지켰다.

### 1.3 UI 계약 스키마 검증

```
$ python3 /Users/jangyoung/.aside/u/0/skills/user/game-ui-ux/scripts/validate-game-ui.py \
    _workspace/current/systems/game-ui-contract.json
PASS: valid game UI contract        # exit 0
```
`json.load` 도 통과. `verification.matrix` **18행 → 20행**(기존 행 삭제 0건).

### 1.4 편집 후 해시 [OBSERVED 2026-09-10 R7]

| 파일 | sha256(앞 12) |
|---|---|
| `systems/data-schemas/beats.md` | `2e2ed53d487b…` |
| `systems/data-schemas/plates.md` | `cc5f06ce7bbb…` |
| `systems/data-schemas/save.md` | `dfd5a1c4a4a9…` |
| `systems/data-schemas/tools.md` | `bc5790e8f913…` |
| `systems/data-schemas/hints.md` | `967399260759…` |
| `systems/data-schemas/zones.md` | `0dfc7251a541…` |
| `systems/system-specs/save-undo.md` | `d597ce5b603f…` |
| `systems/system-specs/wiring-trace.md` | `60d6f2b15844…` |
| `systems/system-specs/tide-alignment.md` | `59000f45e3ec…` |
| `systems/system-specs/dual-seal.md` | `bad199dd5bf1…` |
| `systems/system-specs/plate-readout.md` | `aed89e5cbac5…` |
| `systems/ops/telemetry-contract.md` | `8a594caa553a…` |
| `systems/architecture-contract.md` | `d309e967a828…` |
| `systems/pipeline/emit-tables.mjs` | `a98c37463ed7…` |
| `handoff/codex-unity-brief.md` | `2a057de99017…` |
| `handoff/asset-runbook.md` | `926e995096e1…` |
| `handoff/README.md` | `562b10ef60d1…` |
| `systems/game-ui-contract.json` | `f9cda866d552…` (40,476 B) |
| `systems/interaction-rules.md` | `71720699515f…` (45,540 B) |
| `systems/unity-implementation.md` | `7d0a9afa0c3b…` (15,421 B) |

명령: `shasum -a 256 <files>` · `wc -c <files>`. 재현 시 값이 다르면 **그 사이에 누가 고친 것**이다.

## 2. 결함별 처리 상세

### 2.1 C6-F13 — 파이프라인·이름 두 벌

- **신설**: `systems/pipeline/emit-tables.mjs`(생성기, Node 내장 모듈만) + `emit-tables.meta.md`.
- **구조 결정**: 검증 규칙은 `planning/validate-campaign.mjs` **한 곳**. 생성기는 그것을 **호출**하고, 임포터는 **영수증 해시(`V-1`~`V-4`)** 를 대조한 뒤 **런타임 전용 4건(`R-1`~`R-4`)** 만 재구현한다.
- **철회한 지시**: 브리프 §④-2 의 「그 규칙을 C# 으로 다시 구현하고」. 규칙이 두 벌이면 반드시 갈라진다.
- **개명**: `plates.md` `mediaType` → **`sourceType`**. 파생 정정 `dual-seal.md`(§2 상태기계·`S-R1`·§7 참조) · `plate-readout.md`(§7 참조). 저장소에 남은 `mediaType` 은 **QA 문서의 결함 서술 2곳뿐**이다 [OBSERVED `grep`].
- **형태 표 신설**: `plates.md` §0-1 — `beats`·`hints` = JSON(생성기 출력), `zones`·`plates`·`tools` = ScriptableObject(엔진 오브젝트 참조). `zones.md`·`tools.md`·`hints.md` 머리줄을 그 표에 맞췄다.
- **고친 문서**: `beats.md` §1(M1~M6 재작성 · §1-2 파이프라인 · §1-3 임포터 분담 · §1-4 개명) · `plates.md` · `zones.md` · `tools.md` · `hints.md` · `architecture-contract.md` §3 · 브리프 §④-1·§④-2·§⑩-2·§⑩-3.

### 2.2 C7-F2 — 패키지 0/4

- **관측 재확인**: `manifest.json` 비-모듈 의존 = `com.unity.multiplayer.center` 1건. `packages-lock.json` 37 의존 중 비-builtin **0건** [OBSERVED].
- **신설 §③-1a**: `Assets/_Project/Editor/PackageBootstrap.cs`(버전 미지정 `Client.AddAndRemove`) + `§⑩-2 #0` 배치 명령 + **하드 게이트**(`packages-lock.json` 에 4종 존재 확인 후에만 다음 단계).
- **정직성**: 이 절차는 **실행되지 않았다**. `-batchmode` 안에서 `Client` 요청이 진행되지 않을 가능성을 `[INFERENCE]` 로 적고 **GUI Package Manager 폴백**을 함께 적었다.
- **N-10 예외 등재**: `com.unity.localization` 의 전이 의존으로 `com.unity.addressables` 가 lock 에 나타나는 것은 허용, **API 직접 사용은 여전히 RFC 대상**.

### 2.3 C7-F6 — 레이아웃 자기모순

| 어긋나 있던 곳 | 정정 |
|---|---|
| 브리프 §③ 트리 `Tables/ zones.json, plates.json, tools.json` ↔ §④-1 ScriptableObject | 트리에서 셋을 빼고 `Authoring/ Zones·Records·Tools` 로 |
| 브리프 §③ `Assets/Scenes/`(밖) ↔ 바로 아래 줄 「`_Project/` 밖에는 서드파티만」 | `Scenes/` 를 `_Project/` 안으로 |
| `asset-runbook.md` §3.2 `Assets/Art/**`(제3의 위치) | `Assets/_Project/Art/**` 로 통일 |
| `architecture-contract.md` §3 도 같은 두 문제 | 같은 편집에서 정정 · **예외 0건**을 명시 |

비용 0인 근거: `unity/Unknown/Assets/` 파일 **0개**, 승격 자산 **0종**(런북 §3.1 「현 시점 승격 가능 자산은 0종」) — 옮길 파일도 끊길 `.meta` GUID 도 없다 [OBSERVED].

### 2.4 C7-F7 — 재생 단위 두 벌

- **결정: 명령 소싱.** `CommandEntry` 에 **정규화 `payload`** 를 넣고 `payloadHash` 는 무결성 전용으로 격하. 이벤트는 `Commit` 의 파생물이며 **세이브에 저장되지 않는다**.
- **근거**: 되돌림 1스텝 = 플레이어 행동 1개 · `Sandbox→Commit` 이 **명령 재검증**을 요구 · 저장 바이트가 작다 · 문서 3곳이 이미 명령 소싱.
- **대가**: `Commit` **순수성**이 하드 요구가 됐다(`S-I10`). `Tide.Sim` 의 엔진 참조 0(`T-B1`)이 강제 수단.
- **신설 필드**: `commitIdempotencyKey`(루트 · `T-17`·`T-18`·`T-19` 가 단언하는데 스키마에 **없던** 필드) · `payload` · `byteCap`. 셋 다 **개명 금지 목록에 편입**.
- **상한 재산정**: 「50,000 / 8 MB」 폐기 → **`byteCap` 6 MiB 또는 `entryCap` 20,000, 먼저 닿는 쪽**. 평균 엔트리 ≈280 B 는 **[INFERENCE]** 이며 `save_file_bytes / entries_count` · `command_count` 로 재파생한다(`B-SV9`).
- **부수 정정**: `unity-implementation.md` §7 저장 조각에서 `chapter`·`dayIndex`(RFC-S3) · `eventSeq`·`eventLogHash`(이벤트 소싱 잔재) 제거, 그 조각이 스키마 정본이 아님을 명시. `save-undo.md` §0 RFC-S3 문단을 "해소"로 갱신.
- **신설 검사**: `S-I10`·`S-I11`·`S-I12` · `SV-R11`·`SV-R12` · `SV-F8`·`SV-F9` · `D-SV6`·`D-SV7` · `B-SV7`~`B-SV9` · 브리프 `S-1b`·`U-4b`.

### 2.5 C7-F9 — T-26 문안

- 이전 문안(「`X` 는 프리뷰만, `Y` 는 해제만」)은 **`routing` 에서만 우연히 참**이었다. T0 두 도구에서는 거짓이다 — `circuit` 의 `X` = 구획 접기, `reader` 의 `X` = 사본 재생, `Y` 는 `reader` = 인용 고정 / `circuit` = **없음**.
- **재기술**: 「패널 안 `X` = 그 패널이 §1-3.2 에 등재한 부작용 없는 실행 1개, `Y` = 등재 의미 1개, 등재가 없으면 명령 0개, 동시 활성 0건」.
- **실체화 신설**: `T-26a`(circuit) · `T-26b`(reader) · `T-26c`(`Q` 표면 스코프).
- **정본 4곳 동기화**: 브리프 §⑩-1 · `unity-implementation.md` §11 · `game-ui-contract.json` `matrix[12]` · 같은 JSON `decisions[]`. 브리프 `K-3` 불릿도 「T0 두 도구에서는 '프리뷰'라는 말이 성립하지 않는다」로 보강.

### 2.6 C7-F10 — `I`·`H` 겸용

- **채택안 B**: 도구 패널 조회에 **`Q`**(패드 `RS` 단독의 키보드 짝)를 준다. ~~`Q` 는 저장소 전체 미사용 토큰이었다 [OBSERVED `grep` 0건].~~
  - **`[C7-F35 정정 2026-09-10 R8]` 취소선 문장은 재현되지 않는다.** 편집 전 실측은 `_workspace/current/` 전체 **30행** · `systems/`+`handoff/` **22행** · `data-schemas/` **1행**이었고, 그 1행(`data-schemas/zones.md` L53 「`Q`/`E` 순회 순서를 결정」, `cycle: c3` — R7 보다 앞선 문서)이 **선행 점유**였다. 채택안 B 자체는 유지되지만 **근거는 「비어 있었다」가 아니라 「충돌 1건을 정본으로 눌러 비웠다」**로 바뀐다. 정정 내용과 재현 명령은 `tech-verification/c6-c7-fixloop2-systems.md` §1·§3, 결정 문장은 `interaction-rules.md` §1-3.1 (B)안 불릿이 정본이다. 이 줄은 **기록 보존을 위해 지우지 않고 취소선으로 남긴다**.
- **기각안 A**(예외에서 `I`·`H` 제외): 패드는 `back`/`LB`+`back` 으로 패널 안에서도 증거함·가설판에 가는데 키보드만 못 가게 된다 → §0-8 **비대칭 손실**.
- **고친 곳**: `interaction-rules.md` §1-3.1 불릿 신설 · §1-3.2 키 표 4행 + **조회 배정 전수표** 신설 · `wiring-trace.md` §1·§1-A · `tide-alignment.md` §1·§1-A · 브리프 §⑤-3(A) · §⑦-1(행 신설) · `K-9` 신설 · `matrix[19]` 신설.
- 남은 `I` 사용처 3곳(`dual-seal`·`tide-alignment`·`plate-readout` 의 「증거함을 열고 …」)은 **오버레이 진입점 용법**이므로 이제 일관된다.

### 2.7 C7-F11 — 텔레메트리 키 미정의

- **신설 `telemetry-contract.md` §4.1**: 도구 6종 키 **전건 등재**(circuit 6 · reader 7 · alignment 7 · routing 7 · corrosion 6 · seal 7 · hint 파생 4). 소유 규칙 = 의미는 스펙 §6, 등재는 계약.
- **신설 §4 키 2개**: `command_count`(확정 명령 수 · sandbox 미포함 · 감소하지 않음) · `entries_count`(세이브 안 로그 길이).
- **`T-I6` 는 완화하지 않았다** — 계약 쪽을 채웠다. `tools.md` `T-I6` 행에 등재부 위치를 적었다.
- **브리프 §⑨-2 보강**: 퍼즐 6구간 키(`puzzle_enter`·**`first_valid_action`**·`clue_seen`·`hypothesis_preview`·`confirm`·`exit`) 행 신설 — H-1 판정의 핵심 키가 브리프에 0건이었다. `read_budget_exhausted`·`ending_reached`·`command_count`·`entries_count` 도 추가. DoD 9 에 `T-I6` 통과 조건 명시.

## 3. 해소되지 않은 것 / 이 회차가 주장하지 않는 것

| # | 내용 |
|---|---|
| U1 | **아무것도 실행되지 않았다** — Unity 배치모드 0회, 임포트 0회, 패키지 추가 0회. §③-1a 의 `Client.AddAndRemove` 배치모드 동작은 **[INFERENCE]** 이며 폴백을 함께 적었다 |
| U2 | `R-1`(로컬라이즈 미해결 키 0건 · `T-12`)은 **EN 문자열이 0건이라 지금 통과할 수 없다.** 번역 테이블은 아직 없고 그 사실을 숨기지 않았다 [OPEN-S7] |
| U3 | 엔트리 평균 **280 B** 와 확정 명령 수 **1,000~4,000건**은 **[INFERENCE]**. `entryCap` 20,000 은 그 추정에서 파생한 값이므로 T0 실측 후 재파생 대상(`B-SV9`) |
| U4 | `Q` 바인딩은 **한 번도 눌린 적이 없다**. `matrix[18]`·`matrix[19]` 는 시나리오를 세운 것이며 **미실행**. `matrix[12]`·`matrix[17]` 도 미실행 그대로 |
| U5 | **C4-F12**(색약 팔레트 3종 순회 · 채널별 볼륨 · 연속값 이산 대안 3동사)는 이번 배정 밖이라 **열지 않았다**. 그 항목을 열 때 `game-ui-contract.json` 해시가 한 번 더 갱신된다 |
| U6 | `game-ui-contract.json` 편집이 **공백 서식 정규화(+약 0.8 KB)를 함께 일으켰다**. 값·키·순서 손실 0(파싱→수정→직렬화 1회 · 스키마 검증 통과)이나 다음 diff 가 한 번은 넓게 보인다 — `game-ui-contract.meta.md` 「개정 4」에 같은 고지를 적었다 |
| U7 | `mex` 실행 금지 회차이므로 개명 영향 범위 조회 영수증 = `[SKIPPED: mex 금지]`. `graphify update .` 도 실행하지 않았다 — **이 회차의 변경은 전부 문서·Node 스크립트이며 Unity 코드는 0줄**이지만, 그래프 갱신이 없다는 사실 자체는 기록한다 |
| U8 | 어떤 게이트도 올리지 않았다. G2/G4/G5/G6/G7 은 여전히 **NOT-MEASURED** |

## 4. RFC (디렉터·타 레인 판정 요청)

| id | 대상 레인 | 질문 | 제안 |
|---|---|---|---|
| **RFC-S8** | planner, director | `planning/validate-campaign.mjs` 에 `--emit` 을 넣어 검증기가 직접 테이블을 낼 것인가, 아니면 systems 의 `emit-tables.mjs` 가 계속 검증기를 **호출**할 것인가 | **호출 유지**를 제안한다. 규칙(planner)과 산출(systems)의 소유가 갈려 있고, 호출 방식이면 planner 파일을 건드리지 않고도 파이프라인이 성립한다. 이미 그렇게 동작한다 [OBSERVED] |
| **RFC-S9** | balance, director | 명령 로그 `entryCap` 20,000 / `byteCap` 6 MiB 는 [INFERENCE] 280 B 에서 파생했다. T0 실측 전까지 이 값을 계약 수치로 인용해도 되는가 | **인용 금지**를 제안한다. `[TARGET·INFERENCE]` 표기를 유지하고 T0 계측 후 한 번 재파생한다 |
| **RFC-S10** | worldview, planner, director | `hints.json` 의 EN 문자열이 0건이라 `T-12`(미해결 키 0건)가 구조적으로 실패한다. T0 DoD 에서 이 항목을 **KO 전용으로 한정**할 것인가, 아니면 EN 번역을 T0 범위에 넣을 것인가 | **KO 전용 한정 + 실패를 보고서에 적는 것**을 제안한다. T0 는 슬라이스이고 번역은 본 생산 범위다 |

RFC-S6(도구 표시명 용어집 등재)·RFC-S2(asmdef 5분할 vs 7분할)·RFC-S3(`chapter`/`dayIndex`)는 이 회차에서 **닫히지 않았고 그대로 열려 있다**. RFC-S3 는 `unity-implementation.md` §7 조각에서 필드를 걷어내면서 **문서 간 모순은 사라졌으나 디렉터 판정은 아직 없다**.
