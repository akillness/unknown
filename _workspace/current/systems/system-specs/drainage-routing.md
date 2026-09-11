---
updated: 2026-09-10
cycle: 20260909-preproduction-c3
status: current
supersedes: null
owner: game-systems-designer
---

# 시스템 스펙 — drainage-routing (도구 `routing`, 법4 "이번 조수에는 보호 용량이 부족하다")

> **법 호명 정본** [OBSERVED · RFC-P3-014 · RFC-W3]: 6법의 호명 문구는 `worldview/worldview-bible.md` §3 표(= 아카이브 c3 세션 P 원문) 하나뿐이다. 이 스펙은 인용만 하고 다시 쓰지 않는다. 폐기된 C2 호명 문구는 `worldview/consistency-audit.md` 의 **「사용 금지 문구」 절**(그 안의 표 **「폐기된 6법 호명 문구」** — RFC-W3 이 "사용 금지 문구 원장"이라 부르는 절)에 보존돼 있다. **절 번호로 인용하지 않는다** — 감사 문서의 절 번호가 바뀌어도 이 인용은 살아 있어야 한다(RFC-W3, `qa/c3-review.md` §9.3 q-4 의 QA 대안).

전부 `[TARGET]`. 이 도구만이 **구역 상태를 실제로 바꾸는 확정**을 만든다. 따라서 sandbox/commit 분기와 도달성 보증이 가장 강하게 걸린다.

| 항목 | 값 |
|---|---|
| 도구 id | `routing` |
| 도입 비트 / 미안내 재문제 | `c5-b2` / `c7-b2` [OBSERVED] |
| 등장 비트 수 | **3 / 33** [OBSERVED] — `c5-b2` `c5-b4` `c7-b2`. 적게 등장하지만 영향이 가장 크다 |
| 실제 계통 변경 | **`c5-b4` 1곳** [INFERENCE — `c5-b2`는 가상 운전, `c7-b2`는 12년 전 조건 시연] |
| 부식 관계 | 확정 게이트가 **전역 상한 9**를 조회한다. **차감하지 않는다** (RFC-P3-009) |
| 선택 축 | `propertyProtection` 이진 플래그 1개 (`lowland` \| `dock`) [OBSERVED: campaign.meta.md §5] |

## 1. 입력

| 입력 | KB/마우스 | 패드 | 결과 |
|---|---|---|---|
| 노드 연결 | 드래그 | 좌스틱 + `A` | `AddEdge(from,to)` |
| 연결 해제 | 우클릭 / `Delete`(키보드 단독) | **패널 안에서만 `Y`** | `RemoveEdge` |
| 밸브 상태 토글 | 클릭 | `A` | `SetValve(id, open)` |
| 가상 시험 | `Space` | `X` | `Preview` 실행(무제한·무료) |
| 확정 | 확정 버튼 초점 후 `Enter` (기본 `two-step`) | 초점 후 `A` | `CommitRouting`. **길게 누름은 `hold` opt-in에서만**(`interaction-rules.md` §1-1, RFC-P3-015 F10) |
| 연습 시작/폐기 | `Ctrl+N` / `Esc` | `LB+Y` / `B` | `Fork` / `DiscardBranch` |

## 1-A. 키보드 파생 `[C4-F21]`

**신설 2026-09-10 R6 (C6/C7).** §1 표에서 KB/마우스 열이 포인터 조작만 적은 행의 키보드 단독 경로다. 규칙 id 는 `systems/interaction-rules.md` §1-3.4(D-1~D-7)를 인용한다. §0-8(키보드 단독 완결)이 전역 보장이고, 이 표는 그 보장이 **행 단위로** 어떻게 성립하는지를 적는다.

| 위 표의 행 | 파생 규칙 | 키보드 단독 경로 |
|---|---|---|
| 노드 연결 (드래그) | **D-2** | 출발 커넥터에 초점(`Tab`) 후 `Enter` → 도착 커넥터에 초점 후 `Enter`. `interaction-rules.md` §1 「커넥터 연결」 정본 행 |
| 밸브 상태 토글 (클릭) | **D-1** | 밸브 노드에 초점(`Tab`/방향키) 후 `Enter` |
| 연결 해제 · 가상 시험 · 확정 · 연습 시작/폐기 | — | 이미 키보드 경로다(`Delete` · `Space` · `Enter` · `Ctrl+N`/`Esc`) |

- D-3 대상 행 **0건**(이 패널의 값은 전부 이산 노드·간선이다).

## 2. 상태기계

상태 변수: `edges`, `valves`, `previewReport`, `configCorrosionCost`, `violations`, `branchId`.
`configCorrosionCost`는 **현재 구성안의 함수**이며 잔액이 아니다. 되돌림·구성 변경 시 다시 계산된다.

| 현재 | 이벤트 | 다음 | 부작용 |
|---|---|---|---|
| `Idle` | `OpenTool` | `Editing` | 현재 커밋된 구성 로드 |
| `Editing` | `Fork` | `Sandbox` | 새 `branchId`, 자원 소모 0 |
| `Sandbox`/`Editing` | `AddEdge`/`RemoveEdge`/`SetValve` | 동일 | 통수 화살표·예상 수위 갱신, `violations` 재계산 |
| 동일 | `Preview` | `Previewing` | 결과 요약: 바뀌는 구역, 되돌림 가능성, 부식 비용, 잃는 접근 |
| `Previewing` | `violations == 0 && configCorrosionCost ≤ 9` | `Committable` | 확정 버튼 활성 |
| `Previewing` | 위반 존재 | `Editing` | 위반 구간 붉은 점선 + 위반 이름 |
| `Committable` | `CommitRouting` | `Committed` | **체크포인트 먼저**, 그다음 구역 상태 전이, `propertyProtection` 세팅. **부식 차감 0** |
| `Committed` | `LoadCheckpoint` | `Editing` | 확정 직전으로 복귀(선택 재시도 보장) |
| `Sandbox` | `DiscardBranch` | `Editing` | 브랜치 폐기, main 무변화 |

## 3. 규칙

| id | 규칙 |
|---|---|
| R-R1 | 보호는 제로섬이다. 한 구역을 보호하면 다른 구역이 잠긴다. 두 구역 동시 보호 구성은 `violations`로 거부된다 |
| R-R2 | **잠긴 구역은 증거를 영구히 잃지 않는다.** 잃는 것은 접근 경로이며, 모든 필수 확정에는 **잠기지 않는 자료 경로**(그 확정의 독립쌍 — `sourceType` 상이 AND 루트 `originId` 상이 — 중 한쪽)가 최소 1개 남는다(임포트 검증으로 강제 · 검증기 `C-07`) |
| R-R3 | 확정 전에 항상 `Preview`가 강제된다. `Preview` 없이 `Commit`이 활성화되는 경로는 없다 |
| R-R4 | `Commit` **직전에** 자동 체크포인트를 만든다. 선택 재시도는 언제나 가능하다 |
| R-R5 | `propertyProtection`은 **후일담 문단 두 쌍만** 바꾼다. 필수 단서 집합과 엔딩 3종 접근성은 불변 |
| R-R6 | 부식 한도 초과 구성은 확정이 열리지 않는다(`corrosion-budget` 스펙 참조). 상한은 **전역 단일 9**이며 계통별 한도는 없다 |
| R-R9 | **확정은 부식을 소모하지 않는다** (RFC-P3-009). 확정 전후로 같은 구성안의 비용은 동일하다. 두 번째 `routing` 확정이 첫 확정 때문에 비싸지는 경로는 없다 |
| R-R10 | 두 보호 선택지의 비용은 `lowland` 7 · `dock` 8이고 상한이 9이므로 **둘 다 언제나 확정 가능**하다. 예산이 선택을 강제하지 않는 것이 법4의 공정성 요건이다 [OBSERVED: `prototype/model.mjs:63-65`, `:79`] |
| R-R7 | 막다른 구성은 **무료 우회관**으로 항상 복구된다(구성안마다 1회, 우회관 자체는 고갈되지 않음). 영구 도구 상실 없음 |
| R-R8 | 실시간 침수 압박·강제 대기 없음. 이야기 시계는 장 완료로만 전진한다 |

## 4. 실패 모드

| id | 상황 | 시스템 반응 | 플레이어 손실 |
|---|---|---|---|
| R-F1 | 두 구역 동시 보호 시도 | 위반 표시 "법4: 동시 보호 불가" + 확정 비활성 | 없음 |
| R-F2 | 순환 경로 구성 | 위반 표시 + 순환 구간 하이라이트 | 없음 |
| R-F3 | 확정 후 후회 | 체크포인트 로드로 재선택 | 없음(시간만) |
| R-F4 | 잠긴 구역의 자료가 필요해짐 | 대체 자료 경로 안내(다른 `sourceType`·다른 루트 `originId` · 불변식 R-R2가 존재를 보증) | 접근 지연 |
| R-F5 | 데이터에 대체 경로 없는 확정이 존재 | **임포트 실패**(fail-closed), 빌드 중단 | 없음 |
| R-F6 | sandbox 중 강제 종료 | sandbox 폐기, main 무손상 | 연습 내용(고지됨) |

## 5. 데이터 스키마 참조

- `systems/data-schemas/zones.md` — `floodState`, `accessState`, `drainEdges[]`, `valves[]`, `protectionAxis`
- `systems/data-schemas/tools.md` — `routing` 행(확정 있음, 프리뷰 강제)
- `systems/data-schemas/save.md` — `propertyProtection`, `committedRouting`, `checkpointId`
- `systems/data-schemas/beats.md` — `c5-b2`, `c7-b2` 외 1건

## 6. 텔레메트리 필드

| 키 | 타입 | 의미 |
|---|---|---|
| `routing_edit_count` | int | 연결·밸브 조작 수 |
| `routing_preview_count` | int | 가상 시험 횟수 |
| `routing_violation_count` | int | 위반 발생 수(유형별 분해) |
| `routing_commit_count` | int | 확정 수 |
| `routing_config_corrosion` | int | 확정 시 구성안 비용(0~12). **잔량이 아니라 그 구성안의 값** |
| `routing_checkpoint_reload` | int | 확정 후 재선택 |
| `sandbox_time_min` | float | 연습 시간(공통 키) |
| `commit_time_min` | float | 확정 경로 시간(공통 키) |
| `property_protection_choice` | enum | `lowland` \| `dock` |

## 7. 성능 예산 [TARGET] — NOT-MEASURED

| 항목 | 목표 |
|---|---|
| 위반 재계산(노드 ≤ 64, 간선 ≤ 128) | ≤ 3 ms |
| `Preview` 전체(수위 시뮬 포함) | ≤ 16 ms |
| `Commit` + 체크포인트 기록 | ≤ 200 ms |
| 구역 상태 전이 후 씬 갱신 | ≤ 3000 ms (씬 재로드 포함) |

## 8. 인수 기준

### 문서 단계 (D)
| id | 기준 |
|---|---|
| D-R1 | 확정 경로에 `Preview` → 체크포인트 → 전이 순서가 명시됐는가 |
| D-R2 | `propertyProtection` 두 값 모두에서 엔딩 3종 접근성이 같다고 스키마가 보증하는가 |
| D-R3 | 영구 손실이 스펙 어디에도 없는가 (C2 F5) |
| D-R4 | 부식 **차감·잔량·리셋**을 뜻하는 문장이 0건인가 (RFC-P3-009) |
| D-R5 | 확정 입력이 `two-step` 기본으로 적혀 있는가 (RFC-P3-015 F10) |

### 빌드 후 (B)
| id | 기준 | 방법 |
|---|---|---|
| B-R1 | `propertyProtection` 두 값에서 필수 단서 집합과 엔딩 3종 도달성이 동일 | 도달성 탐색 |
| B-R2 | `Preview` 2회 호출 후 `stateHash` 불변(부작용 없음) | EditMode 테스트 |
| B-R3 | 확정 직전 체크포인트가 항상 존재 | PlayMode 테스트 |
| B-R5 | 확정 전후 같은 구성안의 부식 비용이 동일(무차감) | EditMode 테스트 |
| B-R4 | 대체 경로 없는 확정 픽스처가 임포트에서 실패 | 임포트 검증 테스트 |

## 변경 로그 (같은 사이클 제자리 개정 · RFC-Q2)

| 날짜 | 회차 | 결함 | 절 | 정정 |
|---|---|---|---|---|
| 2026-09-10 | R4 수정 루프 2 | **C4-F19** (S2) | §1 「연결 해제」 행 | 패드 ~~`X`~~ → **패널 안에서만 `Y`**, KB 열에 **`Delete`** 명시. 근거 정본: `interaction-rules.md` L41(「`X` 단독은 모든 화면에서 프리뷰 전용 … 배선 해제는 `X`를 쓰지 않는다」) · 같은 파일 §1 「연결 해제」 행(KB `Delete` / 패드 **패널 안에서만 `Y`**) · §2.4(「해제는 우클릭 / `Delete` / 패널 내 `Y`」). 같은 표 L31 「가상 시험 \| `Space` \| `X`」와의 **한 표 안 겸용**(§0-11 「명시되지 않은 겸용은 결함」)이 소멸한다 |

- **「가상 시험」 행은 바꾸지 않았다** — `X`·`Space` 는 프리뷰 계열이며 `interaction-rules.md` §1-2 「`X` = 프리뷰(모든 화면)」와 같은 의미다. 도구 패널 표면의 우선순위 규칙 부재는 **C4-F16**(이번 배정 밖)이며, 그 규칙이 서기 전까지 이 행의 정합성은 `[INFERENCE]`다.
- **키보드 단독 경로가 이 표 안에서 처음 성립한다** — 이전 판은 KB 열이 「우클릭」뿐이라 이 표만 읽으면 §0-8(키보드 단독 완결)의 경로가 보이지 않았다. `Delete` 는 새 바인딩이 아니라 `interaction-rules.md` §1 정본 행의 인용이다.
- **명령·스키마 영향 0**: `RemoveEdge` 의 이름·인자·의미가 불변이므로 §2 상태기계, §3 규칙, §4 실패 모드, §5 스키마 참조, §6 텔레메트리, §7 예산, §8 인수 기준은 **한 글자도 바뀌지 않았다**. 저장 필드 변경 없음(CLAUDE.md §9).
- **계약 JSON 대조 [OBSERVED 2026-09-10, `grep -n "해제" game-ui-contract.json`]**: `game-ui-contract.json` L153 「패널이 열린 동안 X는 프리뷰 Y는 해제로 고정된다」 · L528 · L620 — **계약 쪽은 이미 정본이었고 스테일은 본 스펙 한 행뿐**이었다. 이 수정으로 md↔json 드리프트가 닫힌다.
- `cycle` 값 불변 — 제자리 개정, `supersedes: null` 유지(RFC-Q2). `status` 는 이번 루프에서 바꾸지 않았다. **새로 측정된 런타임 값 0건**(빌드·플레이·성능 캡처 0).
- **잔여 해소 [2026-09-10 R6 · C5-F5 systems 몫]**: §3 R-R2 와 §4 R-F4 의 폐기 용어 「매체 경로」 2곳을 정본 표현(`sourceType` 상이 AND 루트 `originId` 상이)으로 다시 썼다. 재측정 `grep -c "매체 경로" drainage-routing.md` → **0** [OBSERVED 2026-09-10]. presentation 소유분(덱 2곳)과 economy·balance 소유분은 이 편집의 대상이 아니다.
