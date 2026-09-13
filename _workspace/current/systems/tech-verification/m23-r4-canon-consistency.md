---
updated: 2026-09-14
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
---

# M23 R4 — 정본 문구와 보존 감사 강도 영수증

## 범위·권한

[OBSERVED] frozen `handoff/m23-core-loop-implementation.md` R4를 읽고 편집 전 Main에 ACK했다. `.claude/agents/game-systems-designer.md`, `game-worldview-architect.md`, `game-planner.md`를 읽었다. Main은 IRC로 **t0-b3 힌트 한 문장**과 **continuity §5.2의 현재 강한 통과 주장 정정**을 승인했다. GoalReview는 반복 루트 재확인 및 첫 읽기 사본만으로 P1/P2 PASS를 주지 않는 경계에 ACK했다.

이번 작업은 새 출처·사건·관측값·캠페인 경로를 저작하지 않는다. 바이블·연표·K 매핑·게임 상태/저장 코드는 변경하지 않았다. 기존 생성 테이블과 과거 실행 영수증을 손으로 고치지 않았다.

## 정확히 정정한 주장

| 대상 | 현재 정정 | 근거 및 보존 경계 |
|---|---|---|
| `README.md` 한 줄 소개 | “3주 전에 폐국을 고지한 … 이관 전 마지막 야간 당직” | bible §1/§3-bis.4: D-21 고지, D-1 마지막 밤 21:00~05:00. “폐국을 3주 앞둔”만 정정; README M3/M4 v1/v2 역사 기록 유지 |
| `Resources/T0Strings.json` `confirm-dialog` | KO/EN의 잘못된 `(기본)/(default)` 제거 | interaction-rules §1-1의 기본 `two-step`; 옵션 동작·선택 상태·바인딩 변경 없음 |
| `systems/system-specs/save-undo.md` 및 `systems/data-schemas/save.md` | 현행 버전 v3, 지원 구버전 0/1/2, 상위 버전 거부; 현행 로드 표·B-SV2/3 정정 | `Save/AtomicSaveStore.cs` 지원 범위 및 v3 이전. v1 개명 금지/M2 기록, 초기 “0→v1/v2 거부”는 역사로 명시해 보존. 새 스키마나 마이그레이터 구현 주장 아님 |
| `planning/campaign.json` `t0-b3.hints[2]` | “대장에서 마지막으로 확인되는 시각” | clue c2는 H+3:00까지 끊김 없음. `synopsis/t0-records.md` §5.3은 H+3:00 복구 첫 표준판 샘플, §7.2/7.3은 대장 저작 범위 끝. 원인·후속 시각·136칸·기존 4시간 불변 |
| `synopsis/t0-records.md` §7.3 | 위 끝점의 의미를 한 문단으로 명시 | 표·수식·고정 형식 데이터 불변; 새 사건성 단절을 암시하지 않음 |
| `systems/interaction-rules.md` §3 | C-07 비트별 최소 1쌍, P1 필수 명제별 대체 확정 2쌍, P2 그중 1쌍 **양쪽 매체 모두** 불파괴를 분리 | P1/P2 약화 없음. 이전 “한쪽” 또는 “한 경로” 표현을 강한 약속의 증명으로 쓰지 않음 |
| `planning/validate-campaign.mjs` | C-07/--pairs 출력은 최소 구조 검사임을 명시, 별도 감사 안내 | 기존 최소 검사 수·조건을 새 강한 보장으로 포장하지 않음 |
| `synopsis/continuity.md` §5.2 | 현재 P1/P2는 unknown/blocked, 옛 “9/10 기계”·“10/10 충족”은 강한 증거로 철회 | §5.0 과거 최소 검사 기록, §5.1 K 표 자체 보존. K8 한 건 보강만으로 10/10 회복한다는 주장도 철회 |

## 별도 감사의 측정 범위

새 `planning/audit-campaign-preservation.mjs`는 Node 내장 모듈만 쓰는 읽기 전용 CLI다. JSON 출력은 입력 파일·bytes·SHA-256, 비트 최소 독립쌍 후보, K 명제의 **명시적으로 인용된 두 clue** 후보, 루트쌍 중복, unknown/blocked 사유를 분리한다.

- 독립성은 기존 AND 규칙: `sourceType` 상이 **그리고** 재귀 해석 루트 상이. 원본/다단 사본은 다른 루트가 아니다.
- 누락 루트·불명 `copiedFrom`·순환·계보 충돌을 새 루트로 만들지 않는다. 명시적 `copiedFrom: null`만 루트 종점이다.
- K 칸에 비트 이름만 있으면 그 비트의 첫 적격 쌍을 자동 선택하지 않는다. 없는 명제 표는 공집합 PASS가 아닌 unknown이다.
- 서로 다른 비트에서 같은 루트쌍을 인용하면 후보 반복으로 표시한다. 후보쌍이 두 개여도 같은 명제에 대한 충분성·대체 경로·해금 조건·도달성 증거는 별도다.
- P2의 자연어 보존 메모는 원문으로 출력할 뿐 `보존/사본/위와 같음` 키워드를 PASS로 변환하지 않는다. 양쪽의 최초 접근 및 모든 행동 이후 보존에 대한 전수 증거가 없다.
- **현재 입력 형식에서는 P1/P2를 PASS로 낼 수 없다.** 이것은 미구현 캠페인의 대체 사실을 만들어 넣는 대신 현재 저작이 허용하는 감사 상한을 명시한 것이다. 임의 확장 필드나 미래 저작 매핑을 이미 지원한다고 주장하지 않는다.
- 종료 코드: `1` 무효 계보/명시적 참조·최소쌍 실패, `2` 읽기/파싱 실행 오류, `3` 증거 부족 blocked. 코드 3을 일반 검증 PASS로 합산하지 말고 별도 생산 게이트 공백으로 기록한다.

## 남은 생산/정본 게이트

[OBSERVED · 소스 대조, 실행 아님] K1의 t0-b3/c2-b3와 K2의 c1-b2/c4-b2는 각기 같은 루트쌍이다. K3 재확인 칸은 beat-only이며, K8은 재확인 없음이라고 쓰여 있다. 이를 새 경로나 루트로 만들지 않았다.

- **P1: unknown/blocked.** worldview/planner/synopsis가 동일 필수 명제별 독립 대체 경로의 충분성·범위·완결 조건과 접근 조건을 정본으로 승인해야 한다. K 표의 명제 위치 자체도 현재 캠페인 공개 상한과 재대조가 필요하다. 특히 K10의 c6-b3 동기 귀속은 현행 캠페인의 c6-b4 공개 계약과 다를 수 있어 이 감사가 자동 이식하지 않는다.
- **P2: unknown/blocked.** worldview/systems/QA의 각 쌍 양쪽 보존 및 첫 판독 전 접근·후속 선택/파괴 행동 전수 증거가 필요하다. T0/C1 사본 불변성을 전 캠페인에 외삽하지 않는다.
- 전체 G1, P3/P4/P5 전수 재감사, 전 캠페인 런타임 도달, 인간 이해/발견, Base 본 생산 승인은 **이번 산출물의 PASS 대상이 아니다**.
- `t0-records.md`의 기존 RFC-N9/H+0:12 다른 열의 소유 문제는 이번 끝점 정정으로 해결했다고 주장하지 않는다.

## 실행 상태 및 Main 명령

**[NOT-RUN]** 지시대로 코드/테스트/검증기/emitter/Unity/포매터/린터/빌드/그래프 갱신을 실행하지 않았다. 파일 읽기와 편집만 수행했다. 아래는 완료 영수증이 아니라 Main의 통합 검증 명령이다. 테스트는 실제 거짓 PASS 위험 7개만 작성했으며, 문구 스냅샷 테스트는 없다.

```sh
node --test _workspace/current/planning/audit-campaign-preservation.test.mjs
node _workspace/current/planning/validate-campaign.mjs
node _workspace/current/planning/validate-campaign.mjs --pairs
node _workspace/current/planning/audit-campaign-preservation.mjs
```

마지막 명령의 정상적인 **현재 증거 부족 예상값은 exit 3 / blocked**다 [INFERENCE, 미실행]. 출력에 FAIL/exit 1이 있으면 무효 참조/계보를 확인해야 하며, 정본 사실을 날조해 초록으로 바꾸지 않는다. 출력은 Main의 M23 검증 패킷에 저장하고 최소 검사 PASS와 별도 집계한다.

힌트 원본이 바뀌었으므로, 모든 writer가 끝난 뒤 Main이 다음 생성 명령을 실행한다. `--scope t0`를 두 출력 모두에 명시한다.

```sh
node _workspace/current/systems/pipeline/emit-tables.mjs --scope t0 --out _workspace/current/systems/data/t0
node _workspace/current/systems/pipeline/emit-tables.mjs --scope t0 --out unity/Unknown/Assets/_Project/Data/Tables
node _workspace/current/planning/validate-campaign.mjs --t0 _workspace/current/systems/data/t0
node _workspace/current/planning/validate-campaign.mjs --t0 unity/Unknown/Assets/_Project/Data/Tables
```

Main은 생성된 source SHA·원문과 Unity 실제 힌트 3단계/설정 라벨을 확인한다. 새 게임 및 v1/v2/v3 replay/Undo/Redo는 기존 통합 검증을 유지하며, 이 문구 작업은 이를 실행했다고 주장하지 않는다. 생성기가 만든 메타데이터·테이블은 생성기 소유이며 이 레인은 손대지 않았다.

[UNGRAPHED] mex-agent는 사용할 수 없고 PATH의 mex는 TeX라는 공통 인계를 따랐다. Main이 세션 시작 검색을 수행했으며 이 레인은 targeted grep/read를 사용했다. 그래프 갱신·검증은 Main 통합 단계 소유다.

## Main 통합 실행 영수증

[OBSERVED 2026-09-14] 위 레인의 NOT-RUN은 작성 당시의 이력이다. 이후 Main이 기존 emitter로 양쪽 T0 출력을 재발행하고 campaign50/50·각 T0검사5/5·보존 감사 Node회귀7/7을 확인했다. `Tide.EditorTools.T0AssetImporter.Import`로 producer receipt를 검증/임포트했다. 승인 씬을 재생성하는 Prepare는 실행하지 않았다.

현재 campaign은123944B/SHA-256 `baa3ef6d3729e5dbc8d2d80c4e231e269b3701981c03029cda3859decaf24afe`;33비트/73단서/480설계분을 유지한다. 기존 증거 그래프 생성기/검증기도 실행하여152노드/277간선과18/18을 확인했다. 정합성 PASS는 플레이 시간/강한 보존 증명이 아니다.

강한 감사는 실제 **exit3/blocked**다. 원문 JSON과 입력 지문은 `systems/tech-verification/m23/preservation-audit.json`, 실행 통합은 동 폴더 `verification.json`에 있다.17비트 최소쌍은 P1명제별 대안 경로와 P2양쪽 매체 전 행동 보존을 입증하지 못한다. 부족한 authored mapping과 그 소유자를 유지했다.
