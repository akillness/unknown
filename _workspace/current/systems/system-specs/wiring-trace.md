---
updated: 2026-09-10
cycle: 20260909-preproduction-c3
status: current
supersedes: null
owner: game-systems-designer
---

# 시스템 스펙 — wiring-trace (도구 `circuit`, 법1 "배선된 것만 남는다")

> **법 호명 정본** [OBSERVED · RFC-P3-014 · RFC-W3]: 6법의 호명 문구는 `worldview/worldview-bible.md` §3 표(= 아카이브 c3 세션 P 원문) 하나뿐이다. 이 스펙은 인용만 하고 다시 쓰지 않는다. **법1은 두 계보가 동일해 폐기 문구가 없다**(감사 「사용 금지 문구」 절 각주). 나머지 5법의 폐기된 C2 호명 문구는 `worldview/consistency-audit.md` 의 **「사용 금지 문구」 절**(그 안의 표 **「폐기된 6법 호명 문구」** — RFC-W3 이 "사용 금지 문구 원장"이라 부르는 절)에 보존돼 있다. **절 번호로 인용하지 않는다** — 감사 문서의 절 번호가 바뀌어도 이 인용은 살아 있어야 한다(RFC-W3, `qa/c3-review.md` §9.3 q-4 의 QA 대안).

전부 `[TARGET]`. 구현 0줄, 플레이 0건. 도구 id·비트 id는 `planning/campaign.json` 실제 값 `[OBSERVED]`.

| 항목 | 값 |
|---|---|
| 도구 id | `circuit` |
| 세계관 근거 | 법1 (`worldview/worldview-bible.md` §3) |
| 도입 비트 / 미안내 재문제 | `t0-b2` / `c6-b2` [OBSERVED] |
| 등장 비트 수 | 10 / 33 [OBSERVED] |
| 성격 | **판독 도구**(확정 없음). 다른 도구의 확정에 근거 유효성을 공급한다 |

## 1. 입력

| 입력 | KB/마우스 | 패드 | 결과 |
|---|---|---|---|
| 계통 선 추적 | 선 위 좌클릭·드래그 | 좌스틱 + `A` | 해당 `systemId` 하이라이트, 연결 노드 목록 갱신 |
| 노드 다중 선택 | `Shift`+클릭 | `LT` 홀드 + `A` | 범위 비교 모드 |
| 투명지(분기도) 정렬 | 드래그, `Shift` 정밀 | 스틱, `LT` 정밀, D-Pad 1스텝 | 오프셋 갱신 |
| 구획 접기(미배선 표시) | 구획 클릭 | `X` | `uncoveredAreas`에 추가/제거 (토글, 되돌림 자유) |
| 근거 유효성 조회 | 단서 카드 위 hover / **`Q`** | `RS` | "배선 안/밖" 배지 + 사유. **`I`가 아니다** — `I`는 표면과 무관하게 증거함이다(`interaction-rules.md` §1-3.1 예외 · C7-F10 정정 2026-09-10 R7) |

## 1-A. 키보드 파생 `[C4-F21]`

**신설 2026-09-10 R6 (C6/C7).** §1 표에서 KB/마우스 열이 포인터 조작만 적은 행의 키보드 단독 경로다. 규칙 id 는 `systems/interaction-rules.md` §1-3.4(D-1~D-7)를 인용한다. §0-8(키보드 단독 완결)이 전역 보장이고, 이 표는 그 보장이 **행 단위로** 어떻게 성립하는지를 적는다.

| 위 표의 행 | 파생 규칙 | 키보드 단독 경로 |
|---|---|---|
| 계통 선 추적 (선 위 좌클릭·드래그) | **D-1** | 계통 목록에서 `Tab`/방향키 초점 후 `Enter` → `TraceSystem(systemId)`. 선을 직접 짚는 것은 포인터 편의다 |
| 노드 다중 선택 (`Shift`+클릭) | **D-6** | 노드를 하나씩 초점 `Enter` 로 토글한다. 결과는 동일하다 |
| 투명지(분기도) 정렬 (드래그) | **D-3** · **스텝 = 격자 1칸** | 방향키 1칸, `Shift`+방향키 정밀. 3점 정렬 판정(`AnchorOverlay`)은 스텝 방식과 무관하다 |
| 구획 접기 (구획 클릭) | **D-1** | 구획에 초점 후 `Enter` 로 토글(되돌림 자유 · W-R5) |
| 근거 유효성 조회 (`Q`) | — | 이미 키보드 경로다. **`Q`는 도구 패널 표면에서만 발행되는 조회 키**이며 셸·오버레이에서는 명령이 없다. `I`(증거함)와 겸용하지 않는다(`interaction-rules.md` §1-3.1 · §1-3.2 조회 배정표 · C7-F10) |


Sim에 들어가는 것은 **의도(Intent)** 뿐이다: `TraceSystem(systemId)`, `ToggleUncovered(areaId)`, `SetOverlayOffset(dx,dy)`.

## 2. 상태기계

상태 변수: `traceMode`, `selectedSystemId`, `overlayAligned`, `uncoveredAreas: Set<areaId>`.

| 현재 | 이벤트 | 다음 | 부작용 |
|---|---|---|---|
| `Idle` | `OpenTool` | `Tracing` | 구역 평면도 로드, 마지막 선택 복원 |
| `Tracing` | `TraceSystem(id)` | `Tracing` | `selectedSystemId` 갱신, 연결 노드 하이라이트 |
| `Tracing` | `BeginOverlay` | `Overlaying` | 분기 투명지 표시 |
| `Overlaying` | `SetOverlayOffset` | `Overlaying` | 정렬 오차 표시 갱신 |
| `Overlaying` | `AnchorOverlay` (3점 일치) | `Marking` | `overlayAligned = true` |
| `Overlaying` | `Cancel` | `Tracing` | 오프셋 폐기, 상태 손실 없음 |
| `Marking` | `ToggleUncovered(areaId)` | `Marking` | 해칭 토글, 근거 매체 슬롯 요구 |
| `Marking` | 모든 미배선 구획에 근거 1종 | `Resolved` | 벽 지도 음영 확정, 이후 판독 화면에 음영 상속 |
| `Resolved` | `ToggleUncovered` | `Marking` | **되돌림 자유**, `Resolved` 해제 |
| any | `CloseTool` | `Idle` | 상태 보존(세션 내) |

`Resolved`는 확정(commit)이 아니다. 세이브에는 `uncoveredAreas`만 남고 진행 잠금이 없다.

## 3. 규칙

| id | 규칙 |
|---|---|
| W-R1 | 어떤 단서든 `sensorCoverage == false` 구역에서 유래하면 `evidenceValidity = out_of_coverage` |
| W-R2 | `out_of_coverage` 근거는 `dual-seal`의 근거 슬롯을 채울 수 없다. 비활성 사유가 항상 문장으로 보인다 |
| W-R3 | 회선 3개소(`hub`, `gate`, `dock` 사무소)만 통화의 **개시 시각·계통**을 남긴다. 내용·상대·의도는 어떤 조작으로도 복원되지 않는다 |
| W-R4 | "기록의 침묵"은 부재의 증거가 아니다. Sim은 `no_record` 와 `no_event` 를 **서로 다른 값**으로 유지하고 UI에서 다른 라벨을 쓴다 |
| W-R5 | 미배선 구획 표시는 무제한 토글이며 비용·부식·시간이 발생하지 않는다 |
| W-R6 | `circuit`은 상태를 파괴하지 않는다. 이 도구만으로 도달 불가 상태가 생기지 않는다 |

## 4. 실패 모드

| id | 상황 | 시스템 반응 | 플레이어 손실 |
|---|---|---|---|
| W-F1 | 미배선 구획을 근거로 결론 제출 시도 | `seal` 슬롯 거부 + `out_of_coverage` 사유 | 없음 (제출 자체가 막힘) |
| W-F2 | 투명지 3점 정렬 실패 | `AnchorOverlay` 비활성 + 어느 점이 어긋났는지 표시 | 없음 |
| W-F3 | 데이터 테이블에 `systemId` 고아 참조 | **임포트 실패**(fail-closed), 빌드 중단 | 없음(플레이어에게 도달 안 함) |
| W-F4 | 구역 침수로 평면도 일부 접근 불가(법4 결과) | 해당 구역 노드는 회색, **그 확정의 독립쌍 중 한쪽(다른 `sourceType`·다른 루트 `originId`)은 항상 생존**(불변식 · 검증기 `C-07`) | 접근 지연, 진행 불가 아님 |
| W-F5 | 오버레이 정렬 중 도구 강제 종료 | 마지막 오프셋 복원, 손실 0 | 없음 |

**진행 불가(soft-lock) 0건**이 이 도구의 하드 요구다. `qa/c2-review.md` F5 대응.

## 5. 데이터 스키마 참조

- `systems/data-schemas/zones.md` — `sensorCoverage`, `systemIds`, `viewNodes`, `uncoveredAreaIds`
- `systems/data-schemas/tools.md` — `circuit` 행(패널 id, 명령 목록, 확정 없음 표기)
- `systems/data-schemas/plates.md` — `systemId`, `stationId`(근거 출처 판정용)
- `systems/data-schemas/beats.md` — `tools` 배열에 `circuit`을 가진 10개 비트

## 6. 텔레메트리 필드

| 키 | 타입 | 의미 |
|---|---|---|
| `circuit_open_count` | int | 도구 열기 횟수 |
| `circuit_trace_count` | int | 계통 추적 시도 |
| `circuit_overlay_attempts` | int | 정렬 시도 |
| `circuit_uncovered_marked` | int | 표시한 구획 수 |
| `circuit_out_of_coverage_blocked` | int | W-R2로 막힌 제출 시도 |
| `circuit_time_min` | float | 도구 체류 시간(afk_gap 규칙 적용) |
| `beat_reached` | (beat_id, t) | 공통 키, `ops/telemetry-contract.md` |

## 7. 성능 예산 [TARGET] — NOT-MEASURED

| 항목 | 목표 |
|---|---|
| 도구 패널 열기 → 첫 프레임 | ≤ 120 ms |
| 계통 하이라이트 갱신 | ≤ 1 프레임 (16.7 ms) |
| 오프셋 드래그 응답 | ≤ 1 프레임 |
| 근거 유효성 재계산(전 단서) | ≤ 8 ms |
| 평면도 메모리(구역 1개) | ≤ 24 MB |

기준 하드웨어 미정이므로 통과/실패 판정 불가.

## 8. 인수 기준

### 문서 단계 (D, 지금 판정 가능)
| id | 기준 |
|---|---|
| D-W1 | `circuit`이 등장하는 10개 비트 전부에서 이 스펙의 상태기계로 설명 가능한가 |
| D-W2 | `no_record`와 `no_event`가 스키마에서 서로 다른 값으로 정의됐는가 |
| D-W3 | 이 도구 단독으로 도달 불가 상태를 만드는 전이가 표에 없는가 |

### 빌드 후 (B, C4 이후)
| id | 기준 | 방법 |
|---|---|---|
| B-W1 | 임의 조작 10k 스텝 후에도 `AvailableCommands.Count > 0` | 퍼즈 테스트 |
| B-W2 | `out_of_coverage` 근거가 `seal` 슬롯에 들어가는 경로가 0개 | 그래프 탐색 테스트 |
| B-W3 | 고아 `systemId`가 있는 픽스처가 임포트에서 실패 | 임포트 검증 테스트 |
| B-W4 | 패널 열기 120 ms 목표 대비 실측 | 프레임타임 캡처 |
