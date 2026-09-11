---
updated: 2026-09-10
cycle: 20260909-preproduction-c3
status: current
supersedes: null
owner: game-systems-designer
---

# 데이터 스키마 — save (세이브 v1)

파일 위치: `%USERPROFILE%/AppData/LocalLow/<company>/<product>/saves/slot{n}.json` [TARGET]
관련 스펙: `systems/system-specs/save-undo.md`, 절차 불변식은 `systems/architecture-contract.md` §8.

> **이 스키마의 필드 개명은 플레이어 세이브를 고아로 만든다.** 마이그레이터 없이는 거부한다(CLAUDE.md §9).

## 0. 표기 규약 (6개 스키마 공통)

| 대상 | 규약 | 근거 |
|---|---|---|
| 직렬화 필드(JSON / ScriptableObject) | **camelCase 단일 채택** | `planning/campaign.json`(해시·크기는 `planning/validate-campaign.mjs` 출력을 읽는다 — 고정값 재기재 금지, RFC-Q1)과 `systems/unity-implementation.md` save v1이 이미 camelCase [OBSERVED] |
| C# 공개 멤버 | PascalCase | 변환은 **첫 글자만 소문자화**하는 결정적 1:1 규칙. 약어 대문자 유지 등 그 외 변형 금지 |
| id 값 | 소문자 kebab 또는 소문자 단어 (`t0-b2`, `hub`, `circuit`) | campaign.json 실제 값 [OBSERVED] |
| 텔레메트리 키 | snake_case (**예외**) | 로그 파이프라인 관례. `ops/telemetry-contract.md`가 소유 |

디렉터 지시는 `snake_case | PascalCase` 두 선택지를 제시했다. 위 선택은 그 밖이므로 **RFC-S1**로 제기한다(각 문서 말미).

`tunable` 열: `no` = 코드/스키마 상수, `balance` = `game-balance-designer` 소유, `economy` = `game-economy-designer` 소유, `narrative` = `worldview`/`synopsis` 소유. **tunable ≠ no 인 값을 코드에 하드코딩하면 결함이다**(CLAUDE.md §9).


## 1. 루트

| 필드 | 타입 | tunable | 설명 |
|---|---|---|---|
| `schemaVersion` | int | no | 1. 정수 단조 증가 |
| `saveId` | string (uuid) | no | 슬롯 간 유일 |
| `createdUtc` / `updatedUtc` | string (ISO8601, UTC) | no | 문화권 의존 포맷 금지 |
| `appVersion` | string | no | 진단용. 로드 판정에는 쓰지 않는다 |
| `stageId` | string | no | `T0` `C1`…`E0`. **`dayIndex` 없음**(단일 야간) |
| `beatId` | string | no | 현재 비트 |
| `storyClock` | string | no | `21:00` … `05:00` |
| `tidePhase` | string | no | 조위 위상 라벨 |
| `playSeconds` | float | no | 슬롯 카드 표시용 |
| `progress` | Progress | no | §2 |
| `commandLog` | CommandLog | no | §3 |
| `commitIdempotencyKey` | string (uuid) \| null | no | **확정 트랜잭션 1건의 멱등 키** [C7-F7 · 2026-09-10 R7 등재]. `Commit`이 시작될 때 발급하고 그 커밋이 들어간 세대의 파일에 함께 기록한다. 재시도·지연 완료·rename 후 크래시에서 **같은 커밋이 두 번 적용되는 것을 막는 유일한 수단**이며 `T-17`·`T-18`·`T-19`가 이 필드를 단언한다. 진행 중인 확정이 없으면 `null`. 근거·절차는 `unity-implementation.md` §7 |
| `settingsRef` | string | no | `settings.json` (별도 파일, 세이브와 분리) |
| `checksum` | string | no | `sha256(키 사전순 정규화 본문)` |

## 2. Progress

| 필드 | 타입 | tunable | 설명 |
|---|---|---|---|
| `autoKeptClueIds` | string[] | no | 자동 사본. **어떤 이벤트로도 제거되지 않음** |
| `discoveredClueIds` | string[] | no | |
| `readCounts` | map<recordId,int> | no | 법2 **원본에 가한 파괴적 절차 횟수**(상한 3, 비차단). `plates.md` `readBudget`과 짝 |
| `bypassUsed` | string[] | no | 무료 우회관 사용 구성 id |
| `alignedPairs` | AlignedPair[] | no | 자료쌍 → 확정 오프셋·잔차 |
| `committedRouting` | RoutingConfig? | no | 확정된 경로 구성 |
| `propertyProtection` | enum? | no | `lowland` \| `dock` \| `null`. **이진 선택 1개** |
| `sealedConclusions` | SealedConclusion[] | no | 확정된 결론 |
| `witnessChoices` | map<conclusionId,personId> | no | |
| `submissionPerspective` | enum? | no | `full_restoration` \| `system_defect` \| `incomplete_acknowledged` |
| `plateZeroNameKnown` | bool | no | `c4` 이전 `false` 고정 |
| `hintLevelUsed` | map<beatId,int> | no | 0~3. **엔딩·보상에 영향 0** |
| `checkpoints` | Checkpoint[] | no | 라벨: 장·조위 위상·플레이 시간 |
| `dlcFlags` | map<string,bool> | no | **없어도 본편 로드·엔딩 3종 도달 성립** |

### 2.1 부식 저장 필드 없음 — 부식은 구성안 시험의 파생값 [RFC-P3-009 · C3-F27(c) · 2026-09-10]

**`Progress`에 부식 관련 저장 필드는 하나도 없다.** 잔량·누계·계통별 잔여를 뜻하는 필드(`operationalCorrosion`, `corrosionRemaining`, `corrosionBySystem`, `corrosionPerChapter` 등)를 **추가하지 않는다**. 부식 수치는 **저장 대상이 아니라 파생값**이다:

```
부식비용(표시) = f(committedRouting)            # 확정된 구성안 1개에서 매번 계산
부식비용(시험) = f(sandbox 구성안)               # 저장되지 않는 sandbox 상태에서 계산, 커밋 전에는 세이브에 없음
상한 = 9 (전역 단일, corrosion-budget.md C-R2)   # 데이터 테이블 상수이며 세이브 필드가 아니다
```

| 왜 | 근거 |
|---|---|
| 소모가 없으므로 누계가 존재하지 않는다 | RFC-P3-009(전역 상한 9 · 확정마다 소모 없음 · 리셋 개념 불성립) |
| 저장하면 폐기된 누적 모델이 데이터 층에서 되살아난다 | §4 `operationalCorrosion` 제거 절 |
| 무제한·무료 시험이 성립하려면 시험 상태가 세이브에 남아선 안 된다 | `interaction-rules.md` §2.5 · `system-specs/corrosion-budget.md` |
| 계통별 표시는 UI 분해이지 저장 모델이 아니다 | `data-schemas/zones.md`(표시 분해 전용) · `economy/resources-and-fairness.md` §4.1 |

**economy 레인에 대한 계약 [C3-F27(c)]**: `economy/resources-and-fairness.md` §4.1의 계통별 표시 모델은 **저장 필드에 걸 수 없다**. 그 표는 `committedRouting`(또는 현재 sandbox 구성안)에서 파생한 **표시 전용 분해**로만 성립하며, 세이브 스키마는 그 분해를 저장하지도 복원하지도 않는다. 로드 직후 표시 분해는 `committedRouting` 재계산으로 **결정적으로 동일하게 재생**된다(§6 복구 규칙과 같은 원칙).

**재측정 [OBSERVED 2026-09-10]**: `grep -nE "corrosion|부식" data-schemas/save.md` → 본 절과 §4 제거 기록뿐, 필드 표 행 **0건**.

## 3. CommandLog

### 3.0 재생 단위는 **명령** 하나다 `[C7-F7 · 2026-09-10 R7]`

이전 판은 두 모델이 한 레인 안에 공존했다: `save.md` §3은 `CommandEntry`(명령 소싱)를, `unity-implementation.md` §3은 `EventLog (seq, evtId, payload, causeCommandId)`(이벤트 소싱)를 정본처럼 적었다. 그런데 `CommandEntry`는 `payloadHash`만 갖고 **페이로드 자체를 저장하지 않았다** — 해시로는 명령을 되살릴 수 없으므로 "되돌림 = 명령 로그 재생"(`save-undo.md` §3·§4)도 `T-13`(포인터 0까지 내린 뒤 재적용 시 원래 해시 복귀)도 **구현 불가능**했다.

**결정: 지속되는 재생 단위는 `CommandEntry` 하나다.** 이벤트는 `Commit`이 만들어 내는 **파생물이며 저장하지 않는다**.

| 왜 명령 소싱인가 | 근거 |
|---|---|
| 되돌림 1스텝 = 플레이어 행동 1개여야 한다 | 한 명령이 여러 이벤트를 내므로 이벤트 단위 되돌림은 입도가 어긋난다(`save-undo.md` §4 `AtHead → InPast`) |
| `Sandbox → Commit`이 **main에서 재검증**을 요구한다 | 재검증의 대상은 명령(의도)이지 이미 확정된 이벤트가 아니다(`save-undo.md` §4 · 브리프 §⑤-1 F-3) |
| 저장 바이트가 작다 | 명령 수 < 이벤트 수 |
| 문서 3곳이 이미 명령 소싱으로 적혀 있다 | `save-undo.md` §3·§4, `interaction-rules.md` §6, `architecture-contract.md` §8 |

**성립 조건(하드)**: `Commit`은 **순수 함수**여야 한다 — 같은 `(state, commandId, payload)`는 항상 같은 이벤트 열을 낸다. 난수·시계·문화권 파싱·딕셔너리 순회 순서 의존이 하나라도 들어오면 재생이 깨진다. `Tide.Sim`이 `UnityEngine`·`Time`·`Random`을 참조하지 않는 것(`architecture-contract.md` §2 · 인수 `T-B1`)이 이 조건의 강제 수단이다. 불변식 `S-I10`.

`unity-implementation.md` §3의 `EventLog`는 **메모리 안의 파생 구조**로 위치를 낮춘다(같은 편집에서 정정). 이벤트는 세이브 파일에 나타나지 않는다.

| 필드 | 타입 | tunable | 설명 |
|---|---|---|---|
| `headSeq` | long | no | 되돌림 포인터. 하향해도 항목은 삭제되지 않음 |
| `branchId` | string | no | 현재 브랜치. `main` 기본 |
| `entries` | CommandEntry[] | no | append-only |
| `snapshots` | Snapshot[] | no | 200 커밋마다 1개 |
| `logHash` | string | no | 재생 검증용. `entries`를 순서대로 이은 해시 사슬 |
| `sandboxDiscardedCount` | int | no | 폐기된 연습 브랜치 수 |
| `entryCap` | int | no | **20,000** [TARGET] — §3.2에서 재산정. 초과 시 접기 |
| `byteCap` | int | no | **6,291,456**(6 MiB) [TARGET] — §3.2. 접기의 **실제 방아쇠** |

### CommandEntry

| 필드 | 타입 | 설명 |
|---|---|---|
| `seq` | long | 단조 증가 |
| `branchId` | string | |
| `parentSeq` | long | 분기 부모 |
| `commandId` | string | `tools` 저작본의 `commandId` |
| **`payload`** | object | **정규화 JSON 페이로드** [C7-F7 신설]. 그 명령을 다시 실행하는 데 필요한 인자 전부(예: `SetOverlayOffset {dx, dy}` · `LoadRecord {recordId}` · `SetWindow {startPhase, endPhase}`). **키 사전순 정규화**로 직렬화하며 표시 문자열·로컬라이즈 결과·타임스탬프를 넣지 않는다. 이 필드가 없으면 재생이 불가능하다 |
| `payloadHash` | string | `sha256(정규화 payload)`. **무결성 전용**이며 재생 입력이 아니다. `payload`에서 파생되므로 `logHash` 사슬과 변조 탐지에만 쓴다. §3.2의 바이트 예산이 실측에서 빠듯하면 **가장 먼저 뺄 후보**다(파생값이므로 삭제해도 정보 손실 0) |
| `committed` | bool | sandbox 항목은 저장되지 않으므로 저장 파일 안에서는 항상 `true` |

### 3.1 재생 절차 (`T-13` · `S-I7`의 대상)

```
state = snapshots.last(seq <= headSeq)                  # 없으면 초기 상태
for e in entries where e.branchId ∈ 현재 계보 and e.seq <= headSeq, seq 오름차순:
    validation = Validate(state, e.commandId, e.payload)   # 실패하면 재생 중단 → 복구 패널
    events     = Commit  (state, e.commandId, e.payload)   # 순수 함수 (S-I10)
    for ev in events: state = Reduce(state, ev)
assert Hash(entries) == logHash                          # 불일치 → 복구 패널 (SV-F5)
```

재생 중 `Validate` 실패는 **데이터 손상 또는 버전 불일치**를 뜻한다. 조용히 건너뛰지 않고 `RecoveryPanel`로 간다(`save-undo.md` §3).

### 3.2 상한 재산정 `[C7-F7]`

이전 판의 「8 MB / 50,000 엔트리」는 **`payload`가 없던 시절의 짝**이며 지금은 서로 모순된다. 재산정한다.

| 항목 | 값 | 성격 |
|---|---|---|
| 세이브 파일 전체 | ≤ **8 MB** | [TARGET] `save-undo.md` §9 유지 |
| `commandLog` 직렬화 | ≤ **6 MiB** (`byteCap`) | [TARGET] 나머지 2 MB는 `progress`·`snapshots`·루트 몫 |
| 엔트리 평균 바이트 추정 | **≈ 280 B** | **[INFERENCE]** `payloadHash` 74 + `commandId` ~20 + `branchId` ~8 + `seq`/`parentSeq`/`committed` ~40 + `payload` 40~120 |
| `entryCap` 파생값 | `6 MiB / 280 B` ≈ **22,400** → 안전 여유를 두고 **20,000** | [TARGET · INFERENCE에서 파생] |
| 접기 방아쇠 | `serializedBytes ≥ byteCap` **또는** `entries.length ≥ entryCap` (**먼저 닿는 쪽**) | 바이트가 진짜 제약, 개수는 싼 가드 |

**이 상한은 예상 사용량이 아니라 안전 난간이다.** 480분 본편 1회의 확정 명령 수는 **1,000~4,000건 [INFERENCE]**(design `manipulation` 168분 · `reasoning` 190분 기준)이며 상한의 1/5에도 닿지 않는다. 상한에 닿는 것은 비정상 반복이거나 추정이 틀린 경우다.

**측정 과제(미완)**: T0에서 `save_file_bytes / entries_count`(둘 다 `ops/telemetry-contract.md` §6·§6.1에 이미 정의됨)와 `command_count`(§4 신설)로 **실제 평균 엔트리 바이트**를 재고, 그 값으로 `entryCap`을 다시 파생한다. **그 전까지 280 B는 [INFERENCE]이며 관측으로 인용하지 않는다.**

## 4. 개명 금지 목록 (v1 확정 시점부터 하드)

`schemaVersion`, `saveId`, `stageId`, `beatId`, `autoKeptClueIds`, `readCounts`, `propertyProtection`, `sealedConclusions`, `submissionPerspective`, `hintLevelUsed`, `checkpoints`, `dlcFlags`, `commandLog.headSeq`, `commandLog.entries`, **`commandLog.entries[].commandId`**, **`commandLog.entries[].payload`**, **`commitIdempotencyKey`**, `checksum`.

**2026-09-10 R7 추가 3건 [C7-F7]**: `commitIdempotencyKey`·`entries[].commandId`·`entries[].payload`를 목록에 **편입한다**. 이유는 하나다 — 셋 다 없으면 기존 세이브를 *재생할 수 없다*. `commitIdempotencyKey`가 개명되면 구 세이브의 중복 커밋 방어가 사라지고(`T-17`·`T-19`), `payload`가 개명되면 명령 로그 전체가 해석 불가가 된다. 개명 비용은 **지금 0**(출시 0회·세이브 파일 0개)이고 v1 확정 이후에는 마이그레이터 필수다.

### `operationalCorrosion` 제거 [RFC-P3-009 · 2026-09-10]

> 제거 이후의 항구적 규칙은 **§2.1 「부식 저장 필드 없음 — 부식은 구성안 시험의 파생값」**이다. 이 절은 제거의 근거·비용 기록이고, §2.1 이 "다시 넣지 않는다"를 정의한다.

부식은 **소모되지 않으므로 누계가 존재하지 않는다.** 비용은 현재 `routing` 구성안에서 매번 계산하는 **파생값**이며, 세이브에 잔량을 넣으면 폐기된 누적 모델이 데이터 층에서 되살아난다. 따라서 이 필드를 스키마에서 **제거**한다.

| 항목 | 판단 |
|---|---|
| 세이브 호환 위험 | **없음** — 출시 0회, 빌드 0회, 임포터 0줄, 플레이 표본 n=0. 이 스키마를 읽은 세이브 파일이 세상에 존재하지 않는다 [OBSERVED] |
| 마이그레이션 | 불필요. 지금 비용 0, 출시 후 비용은 마이그레이터 1개 (RFC-S3와 같은 논리) |
| 대체 | 부식 상태를 보고 싶으면 `committedRouting`에서 비용을 재계산한다. 저장하지 않는다 |
| `bypassUsed[]` | **유지** — 우회관 사용은 소모가 아니라 사용 이력이며 UI 표기에 쓴다 |

개명이 필요하면 순서는 **(1) 영향 범위 조회 → (2) 마이그레이터 작성 → (3) 픽스처 테스트 통과 → (4) 개명**이다. 이번 회차는 v1 신규 정의이므로 개명 0건이고, `mex` 조회 영수증은 `[SKIPPED: 이번 회차 mex 실행 금지]`.

## 5. 마이그레이션 규칙

| id | 규칙 |
|---|---|
| MG1 | 마이그레이터는 `v{n} → v{n+1}` 단방향. 체인으로 연속 적용 |
| MG2 | 적용 전 원본을 `slot{n}.v{old}.bak`으로 보존 |
| MG3 | 결과는 **새 파일로 기록**하고 성공 후에만 정본을 교체 |
| MG4 | `schemaVersion`이 실행 파일보다 **높거나 미등록**이면 열지도 덮어쓰지도 않는다 |
| MG5 | 마이그레이터마다 픽스처 테스트가 존재해야 한다. 없으면 등록 거부 |
| MG6 | 필드 삭제는 마이그레이터에서 **읽고 버리기**로만. 조용한 무시 금지 |

## 6. 복구 규칙

| 단계 | 대상 | 실패 시 |
|---|---|---|
| 1 | `slot{n}.json` 체크섬 | 2단계로 |
| 2 | `slot{n}.bak` | 3단계로 |
| 3 | 마지막 자동 체크포인트 | 4단계로 |
| 4 | **읽기 전용 복구 패널** (백업으로 열기 / 마지막 체크포인트 / 새로 시작) | — |

손상 파일은 어떤 단계에서도 **덮어쓰지 않는다**. 복구 경로는 `save_recovery_path`로 기록한다.

## 7. 불변식 (로드/세이브 테스트)

| id | 검사 |
|---|---|
| S-I1 | 정본에 부분 파일이 오는 경로 0개 (tmp → fsync → 회전 → rename) |
| S-I2 | `schemaVersion > 현재`면 파일 바이트 불변 |
| S-I3 | `autoKeptClueIds ⊆ discoveredClueIds` 항상 성립 |
| S-I4 | `propertyProtection` 두 값 모두에서 엔딩 3종 도달성 동일 |
| S-I5 | `hintLevelUsed`가 어떤 진행 상태에도 영향 0 |
| S-I6 | `dlcFlags`가 전부 `false`여도 본편 로드·엔딩 3종 도달 |
| S-I7 | 로그 재생 후 `logHash` 일치 |
| S-I8 | `checksum` 계산이 키 순서에 무관하게 결정론적 |
| S-I9 | **부식 필드 0** — 세이브 스키마·직렬화 결과 어디에도 부식 잔량/누계 키가 없고(§2.1), 로드 직후 표시 분해가 `committedRouting` 재계산으로 저장 전과 동일 |
| **S-I10** | **`Commit` 순수성** — 같은 `(state, commandId, payload)`가 항상 같은 이벤트 열을 낸다. 난수·시계·문화권 파싱·딕셔너리 순회 순서 의존 0건. 인수 `T-02`·`T-13`의 전제이며 위반하면 재생이 깨진다 [C7-F7] |
| **S-I11** | **페이로드 왕복** — 직렬화 → 역직렬화한 `payload`로 재생한 상태 해시가 원본과 동일. 세이브 파일에 **이벤트가 0건**임을 함께 단언한다(이벤트는 파생물이며 저장 대상이 아니다) [C7-F7] |
| **S-I12** | **멱등 키** — 같은 `commitIdempotencyKey`로 두 번 성공해도 `entries`에 항목이 한 번만 늘어난다. 인수 `T-17`·`T-19` [C7-F7] |

## 8. 미측정

세이브 파일 실제 크기, 쓰기·로드 시간, 손상 발생률 전부 **n = 0**. 목표는 `system-specs/save-undo.md` §9.

§3.2의 **엔트리 평균 280 B는 [INFERENCE]**이며 관측이 아니다. `entryCap` 20,000은 그 추정에서 파생한 값이므로, T0에서 `save_file_bytes / entries_count`와 `command_count`를 재는 순간 **재파생 대상**이다. 480분 1회의 확정 명령 수 1,000~4,000건도 [INFERENCE]다(플레이 표본 n=0).

## RFC-S1 (표기 규약)

| 항목 | 내용 |
|---|---|
| 대상 레인 | game-production-director, game-balance-designer, game-economy-designer, game-planner |
| 질문 | 직렬화 필드 표기를 camelCase로 통일해도 되는가 |
| 제안 | camelCase 채택. snake_case로 가면 `planning/campaign.json` 전체 키를 변환해야 하고 변환 계층이 하나 더 생긴다 |
| 증거 | `planning/campaign.json`의 `schemaVersion / designMinutes / fastMinutes / zoneIds / sourceType` (2026-09-10 재확인) [OBSERVED] |
| 예외 | `systems/game-ui-contract.json`은 외부 스킬 스키마(snake_case)를 따르는 별개 계약이며 런타임 데이터가 아니다 |

### 3.3 T0 M2 Snapshot 내부 형식 — RFC-CX-003 director 승인

Snapshot은 `{seq,stateHash,stateBlob:{facts,values,readCounts}}`만 저장한다. 새 루트 필드나 gameplay 수치는 추가하지 않는다. `facts`는 ordinal 정렬한 string 배열, `values`와 `readCounts`는 키 사전순 정규화 map이다. 원본 컬렉션은 복사하고 외부에는 읽기 전용 사본만 제공한다.

- 주기: 기존 계약대로 200 커밋. `seq`는 같은 append-only 명령 로그의 고유 seq를 참조하며 새 branchId 필드를 만들지 않는다.
- 루트 checksum은 checksum 필드를 제외한 전체 정규화 JSON의 SHA-256이다. `logHash`는 순서대로 `sha256(previousHash + canonical(entry))`를 적용하는 사슬이며 각 payloadHash도 검사한다.
- 복원은 snapshot stateHash를 먼저 검사한다. 아직 남은 원명령 경로가 있는 스냅샷은 그 경로를 Validate→Commit으로 재생해 같은 상태인지 검증한다. 되돌림은 현재 head의 선조 스냅샷에서 시작하며 불변인 자동 보관 단서는 유지한다.
- SV-F6의 20,000 entries 또는 6,291,456 bytes 상한 초과 시 활성 경로의 오래된 구간을 스냅샷으로 접는다. 접힌 구간은 보관한 체크포인트 단위 undo/redo이며 UI에 입도 변경을 고지한다. 남은 명령 구간은 개별 undo/redo다.
- 저장 progress 값은 `progress` 아래에 두고 root stateHash를 추가하지 않는다. saveId는 UUID, createdUtc는 UTC ISO-8601이다.

구현: `unity/Unknown/Assets/_Project/Sim/CommandJournal.cs`, `Save/JournalSave.cs`, `Save/SaveCodec.cs`. 실제 검증 결과는 `systems/tech-verification/t0-m2-native.md`에 기록한다.

### 3.4 T0 M2 사용자 설정 (세이브 루트와 분리)

- [OBSERVED] `settingsRef`가 가리키는 `settings.json`은 checksum을 가진 별도 설정 파일이다. 게임 진행 세이브의 필드명을 바꾸지 않는다.
- [OBSERVED] 선택 필드 `holdSeconds`는 정본 `systems/interaction-rules.md` §1-1 / `motion/motion-contract.md` §4의 0.2–1.5초 범위를 UI에 노출한다. 누락 시 생성 테이블 `tools.knobs.commitHoldSeconds.value`의 0.4초 기본값을 사용한다. 선택 UI 범위/단계는 런타임 `Assets/_Project/Resources/HoldOptions.json`에 있다.
- [OBSERVED] 기존 `language`, `textScale`, `confirmMode`, `reducedMotion`, `bindings`와 함께 저장한다. 원본 게임 진행은 이 옵션 변경으로 다시 계산되지 않는다.
