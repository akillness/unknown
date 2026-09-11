---
updated: 2026-09-10
cycle: 20260909-preproduction-c3
status: current
supersedes: null
owner: game-systems-designer
---

# 데이터 스키마 — zones (구역)

런타임 저작본: `unity/Unknown/Assets/_Project/Data/Authoring/Zones/*.asset` (ScriptableObject) [TARGET, 미생성] — 형태 근거는 `plates.md` §0-1 표(`viewNodes[].cameraPose`가 씬 트랜스폼을 가리키므로 JSON 테이블이 아니다 · C7-F6)
**T0 인스턴스 값**: `_workspace/current/systems/data/t0/zones.json` (`hub` 1행 · `viewNodes` 6) — `systems/pipeline/emit-tables.mjs` 출력이며 **손으로 만들지 않는다**. 각 필드가 `_src` 로 출처를 갖는다(C7-F1 · RFC-C7-001). 좌표계는 그레이박스 프레임(Blender Z-up, m)이고 Unity 변환은 `poseFrame.unityConversion` 의 `[INFERENCE]` 다.
행 수: 5 (`hub`, `gate`, `lowland`, `dock`, `pump`) [OBSERVED: planning/campaign.json `zoneIds`]

## 0. 표기 규약 (6개 스키마 공통)

| 대상 | 규약 | 근거 |
|---|---|---|
| 직렬화 필드(JSON / ScriptableObject) | **camelCase 단일 채택** | `planning/campaign.json`(해시·크기는 `planning/validate-campaign.mjs` 출력을 읽는다 — 고정값 재기재 금지, RFC-Q1)과 `systems/unity-implementation.md` save v1이 이미 camelCase [OBSERVED] |
| C# 공개 멤버 | PascalCase | 변환은 **첫 글자만 소문자화**하는 결정적 1:1 규칙. 약어 대문자 유지 등 그 외 변형 금지 |
| id 값 | 소문자 kebab 또는 소문자 단어 (`t0-b2`, `hub`, `circuit`) | campaign.json 실제 값 [OBSERVED] |
| 텔레메트리 키 | snake_case (**예외**) | 로그 파이프라인 관례. `ops/telemetry-contract.md`가 소유 |

디렉터 지시는 `snake_case | PascalCase` 두 선택지를 제시했다. 위 선택은 그 밖이므로 **RFC-S1**로 제기한다(각 문서 말미).

`tunable` 열: `no` = 코드/스키마 상수, `balance` = `game-balance-designer` 소유, `economy` = `game-economy-designer` 소유, `narrative` = `worldview`/`synopsis` 소유. **tunable ≠ no 인 값을 코드에 하드코딩하면 결함이다**(CLAUDE.md §9).

## 1. Zone

| 필드 | 타입 | tunable | 설명 |
|---|---|---|---|
| `zoneId` | string (id) | no | `hub` \| `gate` \| `lowland` \| `dock` \| `pump` |
| `displayNameKey` | string | narrative | 로컬라이제이션 키. 문자열 직접 저장 금지 |
| `sceneName` | string | no | 가산 로드 씬 이름 |
| `addressableLabel` | string? | no | Addressables 채택 시에만 사용 |
| `viewNodes` | ViewNode[] | no | 고정 조사 시점. 구역당 6~10개 [TARGET] |
| `systemIds` | string[] | no | 이 구역을 지나는 계통 |
| `sensorCoverage` | CoverageArea[] | no | 법1 판정의 입력 |
| `uncoveredAreaIds` | string[] | no | `circuit`이 표시할 미배선 구획 후보 |
| `drainEdges` | DrainEdge[] | no | 법4 경로 구성 그래프 |
| `valves` | Valve[] | no | 밸브 노드 |
| `protectionAxis` | enum? | no | `lowland` \| `dock` 중 하나에만 존재. 제로섬 축 |
| `systemLimits` | map<string,float>? | **economy** | **표시 분해 전용**(법5). 어느 계통이 구성안 비용을 얼마나 먹는지 보여주는 UI 라벨이며 **확정 가능성을 바꾸지 않는다**. 차단하는 한도는 전역 `corrosionLimit = 9` 하나뿐이다(RFC-P3-009). 현재 데이터에 값이 없어 `null`이며, 차단 규칙으로 승격하려면 `prototype/model.mjs:687` `INV3` 일반화가 선행돼야 한다 |
| `initialFloodState` | enum | narrative | `dry` \| `partial` \| `flooded` |
| `initialAccessState` | enum | narrative | `open` \| `restricted` \| `locked` |
| `stageIds` | string[] | no | 이 구역이 등장하는 장 (`T0`…`E0`) |

## 2. ViewNode

| 필드 | 타입 | tunable | 설명 |
|---|---|---|---|
| `nodeId` | string | no | 구역 내 유일 |
| `cameraPose` | { pos, rot, fovDeg, fovAxis, lookAt, standDistanceM } | no | 고정. 자유 이동 없음. **`fovDeg` 는 `concept/style-guide.md` §5 의 「수평 화각 약 54°」이므로 `fovAxis: "horizontal"` 을 함께 싣는다** — Unity `Camera.fieldOfView` 는 **수직** 각이므로 임포터가 대상 종횡비에서 변환한다. 축을 적지 않으면 두 수가 조용히 뒤바뀐다 `[C7-F1 · 2026-09-10 R7 종료]`. `lookAt`·`standDistanceM` 은 파생 근거이며 런타임 입력이 아니다 |
| `neighbors` | string[] | no | **시점 노드 초점 순회의 순서**를 결정한다(순서만 — 입력 바인딩을 정하지 않는다). 입력 정본은 `interaction-rules.md` §1 「시점 노드 이동」 행: 키보드 `Tab`/`Shift+Tab` 초점 + `Enter` · 패드 좌스틱 + `A` · 마우스 노드 클릭. `[C7-F35 정정 2026-09-10 R8]` **`Q`/`E` 가 아니다** — 이전 판은 이 칸에 `Q`/`E` 순회라고 적어 §1-3.2 「`Q` @ `Shell` = 발행할 명령 0개」와 정면 충돌했다 |
| `interactables` | string[] | no | 조사 대상 id |
| `transitionType` | enum | no | `cut` \| `dolly`. 모션 축소 시 전부 `cut` |
| `loadCostMb` | float | no | [TARGET] 에셋 예산 G5 입력 |

- `[C7-F35]` **이 표는 입력 바인딩의 출처가 아니다.** 스키마는 *순서*와 *연결성*만 정의하고, 그 순서를 어떤 물리 입력으로 밟는지는 `interaction-rules.md` §1(바인딩 정본) · §1-3.1(표면 우선순위) · §1-3.2(표면별 키 전수)가 소유한다. 스키마 문서가 키를 적으면 바인딩 정본이 둘이 되고, 실제로 그렇게 갈라진 것이 C7-F35다. 이후 어떤 스키마 행도 키를 **배정하지** 않는다 — 정본을 **인용**할 뿐이다(위 `neighbors` 칸이 그 형태다).
- `[C7-F35]` **`Q` 는 `ToolPanel(i)` 조회 전용**이며(`circuit` 근거 유효성 · `alignment` 선후 판정 · 나머지 4종 없음) `Shell`·`Overlay` 표면에서는 명령 0개다. **`E` 는 어떤 표면에도 배정이 없다.** 검수 행은 `game-ui-contract.json` `verification.matrix[19]`(**미실행** — 키 입력 실측 0건).

### 2.1 위 두 문장을 재는 방법 `[C7-F38 정정 2026-09-10 R7 종료]`

**이전 판은 여기에 재현되지 않는 `[OBSERVED]` 두 개를 박아 두었다.** 「`data-schemas/` 안의 키 토큰 = **2행**」과 「백틱 `E` 를 `systems/`·`handoff/` 에서 `grep -rn` 하면 부정 문장 **1건 외 0건**」이 그것이다. 실제로 실행하면 각각 **3행**과 **28행 / 7파일**이 나온다(Z-4·Z-5). **결론(배정 0건)은 참이었고 거짓인 것은 인용된 출력값**이었다 — 정정 자체가 문장을 늘리는 동안 그 수가 계속 커졌기 때문이다.

그래서 **세는 대상을 바꾼다.** 「키가 몇 번 등장하는가」는 이 문서가 고쳐질 때마다 변하는 수라 주장의 근거가 될 수 없다. 주장은 「**배정이 몇 건인가**」이며, 배정 = **표의 한 셀이 정확히 그 키 하나인 행**이다.

```
$ cd /Users/jangyoung/orca/unknown
$ awk -F'|' 'FNR==1{f=FILENAME} /^\|/ { for(i=2;i<=NF;i++){ c=$i; gsub(/\*/,"",c); gsub(/^[ \t]+|[ \t]+$/,"",c);
    if (c=="`E`") printf "%s:%d %s\n", f, FNR, substr($0,1,60) } }' \
  _workspace/current/systems/interaction-rules.md \
  _workspace/current/systems/data-schemas/*.md \
  _workspace/current/systems/system-specs/*.md \
  _workspace/current/handoff/codex-unity-brief.md
(출력 없음)
```

| id | 검사 | 값 [OBSERVED 2026-09-10 R7 종료] |
|---|---|---|
| **Z-6** | 위 명령 그대로 — 바인딩·스펙·브리프 표에서 **`E` 배정 행** | **0행**(출력 없음) |
| **Z-7** | 같은 명령에서 비교 문자열만 `E` → `Q` 로 바꾼 판 — **`Q` 배정 행** | **2행**, 둘 다 `ToolPanel(i)` 조회 배정으로 정본과 일치: `system-specs/tide-alignment.md` **§1 「선후 판정 조회」** · `handoff/codex-unity-brief.md` **§⑤-3(A) 「근거 유효성 조회」**(실행 시점 행 번호 31 · 350 — **행 번호는 편집에 따라 움직이므로 명령 출력이 정본이다**) |

- **등장 횟수는 지표가 아니다.** 백틱 `E` 를 `_workspace/current/systems/`·`_workspace/current/handoff/` 에서 `grep -rn` 하면 이 정정을 서술하는 문장까지 세므로 그 수는 **편집마다 커진다**. 그 수를 어떤 문서에도 상수로 적지 않는다 — 그것이 C7-F38 의 교훈이다.
- Z-6·Z-7 은 **문서를 고쳐도 값이 변하지 않는 검사**다. 배정이 늘면 값이 늘고, 그때는 `interaction-rules.md` §1-3.2 전수표가 함께 늘어야 한다.

## 3. CoverageArea

| 필드 | 타입 | tunable | 설명 |
|---|---|---|---|
| `areaId` | string | no | |
| `covered` | bool | no | `false` = 법1의 "기록 밖" |
| `systemId` | string? | no | `covered == true` 일 때 필수 |
| `hasLine` | bool | no | 회선 3개소(hub/gate/dock)만 `true` — 통화 개시 시각·계통만 기록 |

**`noRecord` vs `noEvent`**: Sim은 두 값을 서로 다른 열거값으로 유지한다(`wiring-trace.md` W-R4). 스키마에 `covered` 하나만 두고 UI에서 두 라벨을 만든다.

## 4. DrainEdge / Valve

| 필드 | 타입 | tunable | 설명 |
|---|---|---|---|
| `edgeId` | string | no | |
| `from` / `to` | string | no | 노드 id |
| `capacity` | float | **balance** | 통수량 |
| `partCosts` | map<string,float> | **balance** | 부식 비용(법5) |
| `valveId` | string | no | |
| `defaultOpen` | bool | narrative | 초기 상태 |
| `lockedBy` | string? | narrative | 잠금 조건 비트 id |

## 5. 불변식 (임포트 검증, fail-closed)

| id | 검사 |
|---|---|
| Z-I1 | `zoneId` 5종만 존재, 중복 0 |
| Z-I2 | 모든 `systemIds`가 `plates` 저작본의 `systemId`에 존재 |
| Z-I3 | `viewNodes`의 `neighbors`가 양방향이고 고아 노드 0 |
| Z-I4 | `drainEdges`에 순환 없음(DAG) 또는 순환이 명시적 위반 규칙으로 등록됨 |
| Z-I5 | `protectionAxis`를 가진 구역이 정확히 2개 (`lowland`, `dock`) |
| Z-I6 | `systemLimits`가 존재하면 `Σ systemLimits ≥ corrosionLimit(9)`. **전역보다 조이면 두 보호 선택 중 하나가 닫혀 법4가 붕괴하므로 임포트 실패**. 값이 없으면(`null`) 검사를 건너뛴다 — 표시 라벨이므로 부재가 정상이다 |
| Z-I8 | 두 보호 선택지 비용(`lowland` 7 · `dock` 8)이 **둘 다** `corrosionLimit` 이하. 하나라도 초과하면 임포트 실패(법4 공정성) |
| Z-I7 | 모든 `displayNameKey`가 KO/EN 테이블에 존재 |
| Z-I9 | **모든 `Beat.zoneId`가 이 표의 5행 중 하나**이며 해당 비트의 스테이지 `zoneIds`에 포함 — 저작 원본 검사는 `validate-campaign.mjs` `Z-01`·`Z-02`(2026-09-10 R4 실행: 33/33 · 위반 0 [OBSERVED]). 스키마 정의는 `data-schemas/beats.md` §4 |

## 6. 미측정

`loadCostMb`, `viewNodes` 실제 개수, 씬 용량은 **n = 0**. C4 슬라이스에서 `hub`+`gate` 2구역만 실측한다.

## RFC-S1 (표기 규약)

| 항목 | 내용 |
|---|---|
| 대상 레인 | game-production-director, game-balance-designer, game-economy-designer, game-planner |
| 질문 | 직렬화 필드 표기를 camelCase로 통일해도 되는가 |
| 제안 | camelCase 채택. snake_case로 가면 `planning/campaign.json` 전체 키를 변환해야 하고 변환 계층이 하나 더 생긴다 |
| 증거 | `planning/campaign.json`의 `schemaVersion / designMinutes / fastMinutes / zoneIds / sourceType` (2026-09-10 재확인) [OBSERVED] |
| 예외 | `systems/game-ui-contract.json`은 외부 스킬 스키마(snake_case)를 따르는 별개 계약이며 런타임 데이터가 아니다 |

## 7. 변경 로그 (같은 사이클 제자리 개정 · RFC-Q2)

| 날짜 | 회차 | 결함 | 바뀐 절 | 내용 |
|---|---|---|---|---|
| 2026-09-10 | **R7 종료 수정** | **C7-F38** (S3) | §2 불릿 2개에서 재현 불가 `[OBSERVED]` 2건 제거 · **§2.1 신설**(Z-6·Z-7) | 「키 토큰 2행」·「`E` 는 부정 문장 1건 외 0건」이 실측 **3행**·**28행/7파일** 과 어긋났다. 결론은 참이고 인용된 출력값이 거짓이었다. **세는 대상을 등장 횟수에서 배정 행 수로 바꿔** 문서가 고쳐져도 값이 변하지 않게 했다 |
| 2026-09-10 | **R8 수정 루프 2 (C6/C7)** | **C7-F35** (S2) | §2 `neighbors` 행 1개 · §2 아래 불릿 2개 신설 · 이 절 신설 | `neighbors` 칸이 시점 노드 이동을 **`Q`/`E` 순회**로 적어, 같은 `status: current` 셋 안에서 `interaction-rules.md` §1 L29 정본(`Tab`/`Shift+Tab` + `Enter`) · §1-3.2 「`Q` @ `Shell` = 명령 0개」와 **세 문서가 서로 다른 말**을 하고 있었다. QA 가 지정한 **(a) 안 채택** — 스키마 쪽을 정본으로 정정하고 `Q` 를 `ToolPanel(i)` 조회 전용으로 확정. 스키마는 *순서*만 소유하고 *키 배정*은 소유하지 않는다는 경계를 불릿으로 명문화해 재발을 막았다. `E` 는 이 정정으로 실제 미사용 토큰이 됐다 |

- 이 개정은 `cycle` 값을 바꾸지 않는다(RFC-Q2: 같은 사이클 제자리 갱신 = 개정, `supersedes` 유지).
- 이번 개정으로 새로 **측정된 게임 값은 0건**이다. 바뀐 것은 문서 정합뿐이며 `viewNodes` 실제 개수 · `loadCostMb` 는 여전히 n = 0(§6).

