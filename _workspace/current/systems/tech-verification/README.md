---
updated: 2026-09-10
cycle: 20260909-preproduction-c3
status: current
supersedes: null
owner: game-systems-designer
---

# 기술 검증 — C3 (Unity 실측 0건 선언 + 데이터 재측정 영수증 + C4 실행 계획)

## 0. 이번 회차 실측 결과

**Unity 측정 0건.** 이 폴더에 빌드·테스트 결과 파일이 없다는 것이 사실 그대로다.
**데이터 재측정은 수행했다** — §0-1. 두 가지를 섞지 않는다: 저작 데이터의 해시·집계를 다시 잰 것이지 게임을 실행한 것이 아니다.

| 항목 | 상태 |
|---|---|
| Unity 에디터 실행 | **0회** |
| 빌드 | **0회** |
| 테스트 실행 | **0회** |
| 프레임타임 캡처 | **0회** |
| 세이브 손상 주입 테스트 | **0회** |
| 사람 플레이 표본 | **n = 0** |
| `mex` 실행 | **금지(이번 회차 · TeX 동명 바이너리 위험)** → 그래프/메모리 영수증 `[SKIPPED: mex 금지]` |
| `graphify update` | **미실행** — 이번 회차 코드 변경 0건(문서만 수정)이므로 대상 없음 → `[UNGRAPHED]` |
| 데이터 재측정 | **수행** (§0-1). `shasum` · `wc` · `node validate-campaign.mjs` · `grep` |

따라서 G4·G5·G6·G7은 `NOT-MEASURED`이고, 이 문서의 어떤 문장도 게이트를 올리지 않는다. §0-1의 재측정은 **D 게이트(문서 정합)** 입력이며 G 게이트 입력이 아니다.

## 0-1. 데이터 재측정 영수증 — C3-F2 / C3-F15 대응 [OBSERVED 2026-09-10]

> **회차 표기 [R4 추가]**: 이 절의 값은 **2026-09-10 R3 시점**의 관측이다. 그 뒤 planner 가 비트 `zoneId`(C3-F22)를 넣고 systems 가 `model.mjs` 라벨(C3-F36)을 고치면서 해시·검사 수가 달라졌다. **현행 값은 §0-2**이며, 이 절은 역사로 읽는다 — 값을 옮겨 적지 말고 §0-2 또는 검증기를 다시 돌린다(RFC-Q1).

C3 검토가 지적한 "저장소에 존재하지 않는 sha256을 9개 문서가 [OBSERVED]로 인용" 결함을 닫기 위해, 본 레인이 실제로 실행한 명령과 그 출력이다. 복사해 그대로 재현할 수 있다.

### 0-1.1 저작 원본 해시·크기

```
$ shasum -a 256 _workspace/current/planning/campaign.json
fdabf1d421932d679429870a06eeb31e9527562370c6a107aec77c2c884fb2b7  _workspace/current/planning/campaign.json

$ wc -c _workspace/current/planning/campaign.json
  120479 _workspace/current/planning/campaign.json
```

### 0-1.2 구조 검증기

```
$ node _workspace/current/planning/validate-campaign.mjs
```

| 출력 항목 | 값 |
|---|---|
| `summary.checks` / `pass` / `fail` / `verdict` | 44 / 44 / 0 / **PASS** |
| `stages` · `beats` | 9 · 33 |
| `stageMinutes` | 25 · 50 · 55 · 65 · 65 · 70 · 75 · 65 · 10 (합 480) |
| `clues` | **73** |
| `sourceTypeDist` | log 27 · ledger 22 · plate 24 |
| `originCatalogSize` | 31 |
| `fastMinutesSum` / `deliberateMinutesSum` | **322 / 673** |
| `toolBeatCounts` | circuit 10 · reader 11 · alignment 8 · routing 3 · corrosion 3 · seal 7 (합 42) |
| `toollessBeats` | `t0-b1` `c1-b4` `c3-b4` `c7-b1` `e0-b1` (5건) |
| `proofRequiredBeats` | 15 |
| `timeConfidenceCounts` | low 12 · medium 21 · high 0 |
| `kindCounts` | puzzle 21 · exploration 2 · dialogue 5 · payoff 5 |
| **C-07** (proofRequired 독립쌍) | **PASS · 실패 0건 / 15건 중** — C3-F12의 기계 증거 |
| F-03 / F-04 (실측 표본) | `observedMedianMinutes = null` · `humanPlaytests = []` → **n = 0** |

비트 키 합집합은 검증기 출력에 없어 별도 집계했다 (명령: `python3`로 `campaign.json`을 읽어 33비트의 키 합집합을 셈):
**23키**이며 23키 전부 33/33 비트에 존재한다. 이전 문서의 "실제 17개 키"는 폐기했다.

### 0-1.3 프로토타입 파일 해시

```
$ cd _workspace/current/systems/prototype
$ shasum -a 256 model.mjs test-model.mjs build-prototype.mjs README.md
```

| 파일 | sha256 | 09-09 대비 |
|---|---|---|
| `model.mjs` | `10c13ac9fe9a1afaa7b941ba80198e26028a58c9db3031c48b06532d062e4227` | 동일 |
| `test-model.mjs` | `c8a133cf97770e801f006aed4333c3eff95b24a16d781fd6dbae9d4589321b45` | 동일 |
| `build-prototype.mjs` | `cfa2437ebeec9a2b008a3761c05bb972bb455b111f1581f5dd9a52e8786792d6` | **변경** (이전 `33bb4e1e…`) |
| `README.md` | `89a7ff4bc55d5fabf388565855efe4f25cd7b960dfb3a0269c0eb2839ae8d4c3` | 동일 |
| `artifacts/interaction-prototype.html` | — | **부재** (`find _workspace -name interaction-prototype.html` → 0건) |

프로토타입 테스트(`node test-model.mjs`)는 **이번 회차에 재실행하지 않았다.** 따라서 `prototype.meta.md`의 테스트 수치는 `[CARRIED]`로 강등했다.

### 0-1.4 소스 상수 확인 (인용 정확성)

```
$ grep -n "corrosionLimit\|maxUndo\|id: 'INV3'" _workspace/current/systems/prototype/model.mjs
79:  corrosionLimit: 9,
82:  maxUndo: 32,
687:    id: 'INV3', label: '부식예산을 넘긴 구성은 확정되지 않는다',
```
`ROUTES` 비용은 `model.mjs:63-65` — `lowland` 7 · `dock` 8 · `dock-express` 12 [OBSERVED].

### 0-1.5 이 재측정이 주장하지 않는 것

- 해시가 맞다는 것은 **문서가 데이터와 일치한다**는 뜻이지 **게임이 재미있거나 8시간이라는 뜻이 아니다.**
- 검증기 44/44 PASS는 **저작 JSON의 구조 검사**다. Unity 임포트·런타임 불변식은 여전히 0건 검증이다.
- C-07 PASS는 *데이터에 독립쌍이 존재함*만 말한다. 그 쌍의 **불파괴 보장**은 런타임 규칙이며 미검증이다.

## 0-2. R4 재측정 영수증 [OBSERVED 2026-09-10 R4]

§0-1 과 **같은 명령**을 다시 실행한 결과다. 다른 것은 입력이 바뀌었기 때문이고, 무엇이 바뀌었는지도 함께 적는다.

```
$ shasum -a 256 _workspace/current/planning/campaign.json
92301c0a5ecfc7e142646f5f17c29c21609e2b2de7c90408de7bffc9d815ae23
$ wc -c _workspace/current/planning/campaign.json
  121457
$ node _workspace/current/planning/validate-campaign.mjs      # summary
{ "checks": 47, "pass": 47, "fail": 0, "verdict": "PASS" }
$ cd _workspace/current/systems/prototype && node test-model.mjs
== 결과: 37 통과 / 0 실패 ==   (exit 0)
```

| 항목 | R3 (§0-1) | **R4 (현행)** | 차이의 원인 |
|---|---|---|---|
| `campaign.json` sha256 | `fdabf1d4…` | **`92301c0a…`** | planner 가 33비트에 `zoneId` 추가 (C3-F22) |
| 크기 | 120,479 B | **121,457 B** | 같음 |
| 검증기 | 44 / 44 PASS | **47 / 47 PASS** | `Z-01`(zoneId ∈ stage.zoneIds) · `Z-02`(전 비트 존재) · `K-06`(RFC-W4 의도 문장 위치) 추가 |
| 비트 키 합집합 | 23키 | **24키** (전건 33/33) | `zoneId` |
| `model.mjs` sha256 | `10c13ac9…` | **`53a9d3a4…`** (35,149 B / 762 lines) | C3-F36 라벨 1줄 교체 (`prototype.meta.md` 해당 절) |
| `node test-model.mjs` | 재실행 없음 (`[CARRIED]`) | **재실행 · 37 통과 / 0 실패 / exit 0** | 라벨 교체 후 회귀 확인 |
| 구역별 비트 수 (신규 집계) | — | `hub` 15 · `pump` 6 · `dock` 5 · `lowland` 4 · `gate` 3 = 33 | 검증기 `aggregates.zoneBeatCounts` |

**이 재측정도 게이트를 올리지 않는다.** Unity 실행 0회 · 빌드 0회 · 프레임타임 캡처 0건 · 사람 플레이 n=0 은 §0 그대로다. `test-model.mjs` 37 통과는 **참조 모형의 내부 일관성**이며 게임·재미·플레이 시간의 증거가 아니다.

## 1. 이번 회차에 실제로 관측한 것 [OBSERVED]

명령을 실행해 확인한 것만 적는다. 성능·품질과는 무관한 존재 확인 수준이다.

| 명령 | 관측 결과 |
|---|---|
| `ls /Applications/Unity/Hub/Editor/` | `2022.3.32f1-x86_64/`, `2022.3.62f2/`, `6000.5.6f1/` |
| `ls -d /Applications/Unity/Hub/Editor/6000.5.6f1/Unity.app/Contents/MacOS/Unity` | 존재 (98.4 MB 실행 파일) |
| `cat unity/Unknown/ProjectSettings/ProjectVersion.txt` | `6000.5.6f1 (0e0577a1a2ac)` |
| `find unity/Unknown/Assets -type f` | 파일 0개 |
| `cat unity/Unknown/Packages/manifest.json` | `com.unity.multiplayer.center 1.0.1` + 내장 모듈 34종 |
| `grep activeInputHandler ProjectSettings.asset` | `activeInputHandler: 0` (레거시 입력) |
| `grep m_CustomRenderPipeline GraphicsSettings.asset` | `{fileID: 0}` (내장 파이프라인) |
| `command -v ffmpeg / magick` | `/opt/homebrew/bin/ffmpeg`, `/opt/homebrew/bin/magick` |

**LTS 여부는 확인하지 않았고 주장하지 않는다.** 위 관측은 "에디터가 설치돼 있다"는 것 이상을 말하지 않는다.

## 2. C4에서 실행할 검증 계획 (미실행)

아래는 **계획된 명령**이다. 지금 실행하지 않았고, 실행 전에는 결과를 인용할 수 없다.

### 2.1 프로젝트 열기 / 패키지 해석

```
UNITY=/Applications/Unity/Hub/Editor/6000.5.6f1/Unity.app/Contents/MacOS/Unity
PROJ=<repo>/unity/Unknown

"$UNITY" -batchmode -quit -nographics \
  -projectPath "$PROJ" \
  -logFile "<repo>/_workspace/current/systems/tech-verification/logs/c4-open.log"
```
- 기대 산출: `Packages/packages-lock.json`에 실제 버전이 채워진다 → `architecture-contract.md` §4의 `[PIN-AFTER-RESOLVE]` 를 그 값으로 대체한다.
- 실패 시 기록: 로그 그대로 보존, 버전을 발명하지 않는다.

### 2.2 EditMode 테스트 (Sim 순수 로직)

```
"$UNITY" -batchmode -runTests -nographics \
  -projectPath "$PROJ" \
  -testPlatform EditMode \
  -testResults "<...>/results/editmode.xml" \
  -logFile "<...>/logs/editmode.log"
```
- 대상: `architecture-contract.md` §11.2 B-A1~B-A3, 각 스펙의 B-* 결정론·경계값 항목.
- 인수: 실패 0건 + `results/editmode.xml`을 이 폴더에 커밋.

### 2.3 PlayMode 테스트 (세이브·복구)

```
"$UNITY" -batchmode -runTests \
  -projectPath "$PROJ" \
  -testPlatform PlayMode \
  -testResults "<...>/results/playmode.xml" \
  -logFile "<...>/logs/playmode.log"
```
- 대상: `save-undo.md` B-SV1~B-SV6. 부분 기록 주입, 상위 버전 거부, 100회 강제 종료.
- 인수: 정본 파일 바이트 불변이 **파일 해시로** 증명될 것. 로그 문자열 검색으로 대체하지 않는다.

### 2.4 데이터 임포트 검증 (fail-closed 확인)

```
"$UNITY" -batchmode -quit -nographics \
  -projectPath "$PROJ" \
  -executeMethod Tide.Data.Import.ValidatorCli.Run \
  -logFile "<...>/logs/import.log"
```
- 대상: `zones/plates/tools/beats/hints` 5개 스키마의 불변식 표 전부.
- **위반 픽스처 4종 이상**을 준비해 전부 실패하는 것을 확인한다. 통과만 확인하는 것은 검증이 아니다.

### 2.5 프레임타임 캡처

```
# 기준 PC 확정 후에만 유효
"$UNITY" -batchmode -projectPath "$PROJ" \
  -executeMethod Tide.Tests.Perf.CaptureCli.Run \
  -captureScenes hub -captureSeconds 120 \
  -out "<...>/results/frametime-{hw_profile_id}.csv"
```
- **`[C4-F8 정정 2026-09-10]` 씬 목록은 `hub` 하나다.** 이전 판은 `-captureScenes hub,gate` 였으나 `gate` 는 **T0 에 없다** — T0 `zoneIds` = `["hub"]` 이고 `gate` 의 첫 등장은 **C1 `c1-b1`** 이다 [OBSERVED 2026-09-10, `planning/campaign.json`]. `gate` 캡처는 **C1 슬라이스가 생긴 뒤**에 별도 실행한다. §2.6 의 alignment 정합 구간(`c3-b2`~`c3-b3`)을 함께 재는 경우의 씬은 `gate` 가 아니라 **`dock`** 이다(같은 관측).
- 산출: 씬·도구별 `frame_time_ms_p50/p95/p99`, `zone_load_ms`, `peak_memory_mb`.
- **`hw_profile_id`가 비어 있으면 결과를 게이트에 쓰지 않는다**(`ops/telemetry-contract.md` §7).
- Windows 우선 대상이므로 macOS 캡처는 참고값으로만 표기하고 목표 판정에 쓰지 않는다.

### 2.6 검증 슬라이스 플레이 계측

- 대상: **T0 25분**(정본) + `c3-b2`~`c3-b3` 정합 구간(`dock`).
- **`[C4-F8 정정 2026-09-10]`** 이전 판은 "T0 **30분**" 이었다. 정본은 **25분** 이며 출처 3자가 일치한다 [OBSERVED 2026-09-10]: `unity-implementation.md` §10 L115("목표 길이 **25분**") · live `planning/campaign.json` `T0.minutes = 25`(검증기 `aggregates.stageMinutes[0]`) · `planning/campaign-time-budget.md` L359("T0 25분 + 34분 = 59분"). **30분으로 계측하면 25분 설계와 비교할 수 없는 첫 값이 나온다** — 이 문서는 Codex 핸드오프가 실제로 돌릴 명령이므로 오기의 비용이 문서 내부에서 끝나지 않는다.
- 수집: `beat_reached`, `hint_used`, `undo_count`, `sandbox_time_min`, `commit_time_min`, `afk_gap`.
- 산출: `observed_completion_min`의 **첫 값**. 이 값 1건은 중앙값이 아니며 8시간 주장을 지지하지 않는다.
- 판정 키는 `total_minus_afk_min` 이다(RFC-P3-011). 목표 밴드 450~540분은 **본편 완주**의 밴드이며 T0 25분과 비교 대상이 아니다.

## 3. 선행 조건 (이것이 없으면 위 명령은 무의미)

| id | 조건 | 현재 |
|---|---|---|
| PRE-1 | 기준 PC 1대 고정 (`hw_profile_id`) | **미정 — 그리고 그것이 결정이다** [C6-F11 ①]. 개발 참조 기기는 이 세션의 Apple Silicon Mac(16 GB)이며 **성능 수치는 캡처만 하고 PASS/FAIL 판정을 하지 않는다** |
| PRE-2 | Input System 활성화 (`activeInputHandler` 변경) | **값 결정됨 = `2`(Both)** [C6-F11 ②] · 액션 맵 이름 **`Watch`** · 실행은 미실행(현재 `0`) |
| PRE-3 | `Assets/_Project/` 폴더·asmdef 생성 | **분할 결정됨 = 7분할**(RFC-S2 · 생산 어셈블리 7 + 테스트 2 + `EditorTools` 1 = asmdef 파일 10) · 실행은 미실행 |
| PRE-4 | 데이터 테이블 5종 생성 | **T0 분은 생성됨** [C7-F1 · 2026-09-10 R7 종료] — `_workspace/current/systems/data/t0/{beats,hints,tools,zones,records}.json` + 영수증 + `.meta.md` 6. 영수증 `r7-t0-data.md`. **Unity 임포트는 여전히 0회** |
| PRE-5 | URP 채택 여부 결정 | **결정됨 — URP 채택** [C6-F11 ③]. 버전은 `[PIN-AFTER-RESOLVE]`, 설치는 미실행. 필수 패키지가 4종 → **5종**이 됐다 |
| PRE-6 | `com.unity.multiplayer.center` 제거 | 미실행 |

## 4. 결과 파일 규약

- 결과는 이 폴더에 `{name}.md`(명령 + 관측 결과) + 원본 산출물(`results/*.xml`, `*.csv`, `logs/*.log`)로 남긴다.
- 파일이 없으면 측정이 없었던 것이다. **요약만 쓰고 원본을 남기지 않는 것은 금지**한다.
- 실패한 실행도 남긴다. 성공한 실행만 남기면 그것은 기록이 아니라 광고다.
- 비-Markdown 산출물은 같은 basename의 `.meta.md`를 갖는다(CLAUDE.md §10).

## 5. 정정 로그 (같은 사이클 제자리 개정 · RFC-Q2)

| 날짜 | 회차 | 결함 | 절 | 정정 |
|---|---|---|---|---|
| 2026-09-10 | R4 수정 루프 1 | **C4-F8** (S2) | §2.5 | `-captureScenes hub,gate` → **`-captureScenes hub`**. `gate` 는 T0 에 없다(첫 등장 C1 `c1-b1`). alignment 구간 캡처 씬은 `dock` |
| 2026-09-10 | R4 수정 루프 1 | **C4-F8** (S2) | §2.6 | "T0 **30분**" → **"T0 25분"**. 정본 3자(`unity-implementation.md` §10 · live JSON `T0.minutes` · `campaign-time-budget.md` L359) 일치 확인 |
| 2026-09-10 | **R7 수정 루프 1 (C6/C7)** | C6-F13 · C7-F2 · C7-F6 · C7-F7 · C7-F9 · C7-F10 · C7-F11 | (신규 파일) | 7건의 처리·명령·관측을 **`tech-verification/c6-c7-fixloop1-systems.md`** 에 남겼다. 신설 실행물은 `systems/pipeline/emit-tables.mjs`(+`.meta.md`) 하나이며 **Unity 실행·빌드·임포트는 여전히 0회**다. 이 README 의 §0~§4 수치는 이번 루프에서 바뀌지 않았다 |
| 2026-09-10 | **R8 수정 루프 2 (C6/C7)** | **C7-F35** | (신규 파일) | C7-F10 이 배정한 `Q` 가 **미사용 토큰이 아니었다**는 사실(선행 점유 `data-schemas/zones.md` L53 `Q`/`E` 노드 순회)과 그 정정을 **`tech-verification/c6-c7-fixloop2-systems.md`** 에 남겼다. 채택은 QA 지정 **(a) 안** — 스키마 쪽을 `interaction-rules.md` §1 정본으로 되돌리고 `Q` 를 `ToolPanel(i)` 조회 전용으로 확정했다. 실행물 신설 **0건**, **Unity 실행·빌드·키 입력은 여전히 0회**이며 `matrix[19]` 는 미실행 그대로다. 이 README 의 §0~§4 수치는 이번 루프에서도 바뀌지 않았다 |

| 2026-09-10 | **R7 종료 수정** | **C7-F1**(S1) · C7-F4 · C7-F8 · C7-F14 · C7-F38 · C6-F5 · C6-F9 · C6-F11 · C6-F12 · C4-F12 · C4-F20 · RFC-S2/S3/S5 | §3 PRE 표 · (신규 파일 2) | T0 인스턴스 데이터를 **생성기로** 만들고(`systems/data/t0/**`) 그 명령·출력·대조를 **`r7-t0-data.md`** 에, 회차 전체 자기 검사표를 **`r7-systems-receipt.md`** 에 남겼다. §3 PRE-1~PRE-5 는 「미정/미결」에서 **결정 반영**으로 갱신됐다(실행은 여전히 미실행). **Unity 실행·빌드·키 입력 0회**는 그대로다. 이 README 의 §0~§2 수치는 이번 회차에 바뀌지 않았으며, 그 안의 sha·검사 수는 **R4 시점의 관측**이라 현행 입력과 다르다 — 인용하지 말고 검증기를 다시 돌린다(RFC-Q1 · C7-F25) |

재측정 명령과 결과 [OBSERVED 2026-09-10 R4 · **현행 아님**. 아래 숫자는 R4 시점 입력의 값이며 그 뒤 planner 가 `campaign.json` 을 갱신해 sha·바이트·검사 수가 모두 달라졌다. 재현은 명령을 다시 돌려서 한다]:

```
$ node _workspace/current/planning/validate-campaign.mjs
  sha256 92301c0a5ecfc7e142646f5f17c29c21609e2b2de7c90408de7bffc9d815ae23 · 121,457 B
  summary { checks: 47, pass: 47, fail: 0, verdict: "PASS" }
  aggregates.stageMinutes[0] = 25          # T0
$ python3 -c "…campaign.json…"            # 스테이지별 zoneIds·비트 zoneId 덤프
  T0 25 ['hub']  t0-b1/hub · t0-b2/hub(circuit) · t0-b3/hub(reader,circuit)
  C1 50 ['hub','gate']  c1-b1/gate(circuit) …      # gate 의 첫 등장
  C3 65 ['dock']  c3-b2/dock(alignment) · c3-b3/dock(alignment,reader)
```

- 이 정정은 `cycle` 값(`20260909-preproduction-c3`)을 바꾸지 않는다 — RFC-Q2 에 따라 같은 사이클 제자리 **개정**이며 `supersedes: null` 을 유지한다. `status: current` 도 그대로다(문서의 지위가 아니라 명령의 값이 틀렸던 것이므로 강등 대상이 아니다).
- **이 정정으로 새로 측정된 런타임 값은 0건이다.** §0 의 표(Unity 실행 0회 · 빌드 0회 · 프레임타임 캡처 0건 · 사람 표본 n=0)는 그대로다. 고친 것은 *실행되지 않은 명령의 인자*이며, 고쳤다는 사실이 그 명령을 실행한 것으로 읽히면 안 된다.
- 잔여 [CARRIED]: §2.5 는 여전히 `PRE-1`(기준 PC `hw_profile_id`) 미정에 막혀 있다. 씬 이름이 맞아도 기준기가 없으면 결과를 게이트에 쓸 수 없다(`ops/telemetry-contract.md` §7).
