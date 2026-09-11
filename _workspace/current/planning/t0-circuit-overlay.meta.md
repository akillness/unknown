---
updated: 2026-09-10
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-planner
---

# T0 회로 지도 투명지 — 좌표 저작 패킷

`planning/t0-circuit-overlay.json`의 메타데이터다. **[TARGET · 신규 저작 결정, RFC-CX-003]** 기존 `t0-b2`의 세 점 투명지 정렬에 필요한 기하를 기획이 명시한다. 좌표는 이번에 선택한 퍼즐 도식 값이며, 과거 항구의 실측 위치·거리·센서 설치 위치·관측 사실을 추출한 값이 아니다. **[OBSERVED · 2026-09-10]** §5의 필수 교차 레인 ACK를 수집한 뒤 디렉터가 저작 데이터에 한정해 current 승격을 승인했다. 런타임·사람 플레이·캠페인 게이트 판정은 이 승격의 범위 밖이다.

## 1. 목적과 범위

```yaml
feature_id: t0-circuit-overlay
beat_id: t0-b2
tool_id: circuit
goal: 세 대응점을 비교하며 투명지 전체를 평행 이동해 배선 도식을 맞춘다.
player_fantasy: 같은 세 지점을 직접 겹쳐 보고 무엇이 기록 범위 안인지 읽을 준비를 한다.
layer: sandbox-only
authoring_authority: RFC-CX-003
coordinate_claim: newly-authored-diagram-layout
touched_lanes: [systems, balance, economy, presentation, worldview, synopsis, concept, qa]
acceptance_criteria:
  exact_anchor_count: 3
  unique_anchor_ids: 3
  distinct_target_points: 3
  distinct_overlay_points: 3
  shared_translation_solutions: 1
  grid_step: 1
  fine_grid_step: null
  initial_aligned_points: 0
  solved_aligned_points: 3
  minimum_cardinal_grid_moves: 2
  campaign_beat_changes: 0
  campaign_minutes_changes: 0
  reward_changes: 0
  new_story_reveals: 0
```

위 수치 중 격자 1칸과 3점 일치는 기존 명세의 요구 [CARRIED]이며, 좌표·기술 ID·시작 오프셋은 이번 저작값 [TARGET]이다. 최소 이동 수와 정렬점 수는 좌표의 수학적 파생값이며 플레이어 수행 시간·난이도 실측이 아니다. 정밀 스텝은 승인된 숫자가 없어 JSON `null`로 둔다. 이를 `0`이나 임의의 분수로 바꾸거나 정밀 입력 검증 완료로 기록하지 않는다.

## 2. 좌표와 해의 의미

**[TARGET]** 단위는 회로 지도 로컬 격자다. `x`의 양수 방향은 오른쪽, `y`의 양수 방향은 위쪽이다. 화면 픽셀·Unity 월드 미터·항구 방위를 뜻하지 않는다. 카메라와 UI는 이 좌표를 표현할 뿐이다.

- `target`은 고정 도면의 대응점, `overlay`는 투명지 자체의 로컬 대응점이다.
- `initialOffset`은 `BeginOverlay`의 최초 시작값이다. 재개·취소·닫기의 상태 보존은 기존 FSM을 따른다.
- `SetOverlayOffset(dx,dy)`의 값은 투명지 전체에 더하는 **절대 오프셋**이다. 이동 의도는 현재 오프셋에 스텝을 더한 다음 그 값을 전달한다.
- 정렬 판정식은 모든 대응점에 대해 `overlay + offset == target`이다. 회전·배율·점별 독립 이동을 추가하지 않는다.
- 기술 ID `anchor-hub`, `anchor-gate`, `anchor-dock`는 이 도식의 대응점 키다. `zoneId`나 출처 `systemId`/`stationId`로 추론·대체하지 않는다.

| 대응점 | 표시명 [CARRIED] | target [TARGET] | overlay [TARGET] | target − overlay [파생] |
|---|---|---|---|---|
| `anchor-hub` | 당직실 | (0, 0) | (1, −1) | (−1, +1) |
| `anchor-gate` | 제3수문 | (3, 0) | (4, −1) | (−1, +1) |
| `anchor-dock` | 부두사무소 | (1, 2) | (2, 1) | (−1, +1) |

세 차이 벡터가 모두 `(-1,+1)`이므로 이 평행 이동 하나가 세 점을 모두 맞춘다. 첫 대응점만으로 이동 벡터가 고정되므로 다른 해는 없다. 세 target은 일직선에 있지 않다. 최초 `(0,0)`에서 왼쪽 한 칸과 위 한 칸을 어느 순서로 수행해도 같은 해에 도달한다. 정렬 허용오차·흡착 반경·정밀 스텝을 새 밸런스 수치로 만들지 않는다.

## 3. FSM·실패·회복 계약

**[CARRIED]** `systems/system-specs/wiring-trace.md` §2의 `Idle → Tracing → Overlaying → Marking → Resolved`를 사용한다. 세 점이 모두 일치한 `Overlaying`에서만 `AnchorOverlay → Marking`이 가능하다. 이 패킷만으로 `Resolved`나 `t0-b2` 완료를 만들지 않는다. 기존 완료 술어는 구획 세 곳의 표시와 각 근거 매체 연결이다.

- W-F2: 틀린 오프셋이면 AnchorOverlay를 비활성화하고 어긋난 점을 식별 가능한 문자·형태로 표시한다. 색만으로 구별하지 않는다.
- W-R5/W-R6: 비용·부식·실패 횟수 제한·세계 확정·진행 잠금이 없다. `Cancel`은 투명지 편집을 버리고 `Tracing`으로 돌아가며, 다시 시작할 수 있다.
- W-F5/CloseTool: 닫기와 재개는 기존 마지막 오프셋 보존 의미를 유지한다. 새 JSON 값으로 재개 상태를 덮어쓰지 않는다.
- 데이터는 유한한 수, 정확히 세 대응점, 중복 없는 ID, 하나의 공통 해를 요구한다. 비유한 값·불일치 데이터는 임포트에서 거부하며 정답을 코드에서 보정하지 않는다.
- 기존 명세는 이동 범위 상·하한을 지정하지 않는다 [OBSERVED]. 이 저작 패킷은 새 게임플레이 이동 상한을 추가하지 않는다. [OBSERVED · systems ACK, 디렉터 전달 2026-09-10] 초기 위치 복원을 항상 제공하므로 유한 값 검증 이외의 게임플레이 상한은 필요 없다. 렌더링 AABB는 데이터에서 파생하고 화면 잘림은 초점 복구·카메라/도면 표시 문제로 처리한다.

## 4. 근거와 계보

| 근거 | 이번 패킷에 제공한 것 |
|---|---|
| `production/decision-log.md` RFC-CX-003 | planner의 좌표 명시 저작 권한, 동일 오프셋 해, 정밀 스텝 발명 금지 |
| `planning/campaign.json` `t0-b2.subtasks[0]`, `objective`, `completion` | 투명지와 세 지점 정렬, 안내가 있는 유일한 회로 지도 훈련, 기존 완료 술어 |
| `planning/feature-specs/verb-01-circuit-trace.md` R2/R5/E6 | 색 외 식별, 판독 전용, 키보드·패드 경로 |
| `systems/system-specs/wiring-trace.md` §1-A D-3 / §2 / W-F2 / W-F5 | 격자 1칸, 3점 AnchorOverlay, 실패 표시와 취소·재개 |
| `systems/interaction-rules.md` §1-3.4 D-3 | 방향키 스텝 경로와 정밀 입력의 명시값 요구 |
| `worldview/glossary.md` §1 | 당직실·제3수문·부두사무소의 기존 표시명 |
| `synopsis/t0-records.md` §5.6 대응 규칙 주석·§8.1 | 압력 구간→옥외 구획 대응의 별도 소유, T0 공개 상한 |
| `_workspace/archive/20260909-preproduction-c3/planning/gdd.md` 「현재 결정」·「세 가지 기둥」 | 손으로 도식을 조작하는 기획 계보. 이 신규 좌표 패킷의 전신은 아니므로 `supersedes: null` |

이 대응점 세 개는 **옥외 미배선 구획 세 개가 아니다**. 압력 곡선의 평평한 구간→옥외 구획 대응 규칙(RFC-N9)을 채우거나 바꾸지 않는다. 통화 개시 시각·계통만 기록된다는 범위와 조위 기준 관측소도 서로 대체하지 않는다.

## 5. 교차 레인 ACK와 인수 체크

| 레인 | 전달 내용 | ACK 상태 |
|---|---|---|
| systems | 절대 오프셋 덧셈 의미, 필드 형식, null 정밀값, 임포트 거부와 FSM 유지 | ACK 수신(디렉터 전달, 2026-09-10): `target=overlay+offset`, 초기 위치 복원, 유한 값 외 이동 상한 불필요, 렌더링 AABB 파생, `fineGridStep:null`에서 Shift로 새 스텝 생성 금지 |
| balance | 두 정수 스텝은 저작 기하의 파생값. 첫 조작 시간·힌트 기준·분 배분을 바꾸지 않음 | [balance/t0-circuit-review.md](../balance/t0-circuit-review.md) — scoped ACK, 독립 좌표 검사와 저작 데이터 승격 의존성 승인 |
| economy | 비용·보상·부식·횟수 제한이 없는 sandbox 계약 유지 | [economy/t0-circuit-review.md](../economy/t0-circuit-review.md) — 회로 좌표 경제 계약에 한정한 ACK |
| presentation | x 오른쪽/y 위 도식, 문자+형태 식별, 전체 투명지 이동 | [presentation/t0-circuit-vfx-review.md](../presentation/t0-circuit-vfx-review.md) — 저작 도식과 연출 계약 scoped ACK |
| worldview | 기존 지명만 사용, 지리·센서·출처 귀속으로 해석 금지 | 디렉터에 통지 |
| synopsis | 새 공개·사건·단서·압력 구간 귀속 없음 | `t0_record_authoring` ACK 수신(2026-09-10): 기존 세 표시명은 §6 `wb-l2`·용어집 §1·bible §2에 있으며 신규 지명/로어 없음. 구간→옥외 구획 귀속은 승인 범위 밖 |
| concept | 좌표는 런타임 도식 배치. 지리 정본이나 이미지 속 텍스트 저작 입력이 아님 | 디렉터에 통지 |
| QA | 아래 수학 검사와 기존 FSM 회귀·실제 입력 검증을 구분 | [qa/t0-m2-source-resource-review.md](../qa/t0-m2-source-resource-review.md) §3 — 회로 좌표 소스 수준 ACK. 그 문서의 draft 표기는 승격 전 검토 스냅샷 |

**[OBSERVED · 2026-09-10, RFC-CX-003 디렉터 판정]** systems와 위 balance/economy/presentation/QA의 필수 ACK를 수집해 저작 데이터 current 승격을 승인했다. 같은 사이클의 상태 승격이며 `supersedes: null`과 JSON 좌표는 유지한다. 이 판정은 실제 입력·저장·렌더링·성능·사람 플레이·캠페인 게이트를 승격하지 않는다.

**RFC-CX-001 planning ACK.** `worldview/glossary.md` §6-2의 표준판 `system-hub` / `station-bureau-standard`, 조위대장 `systemId:null` / 같은 기준 관측소 연결은 `t0-b3`의 기존 인용 고정 범위와 양립한다. 비교 기준 연결을 과거 측정 생산지로 읽지 않고, 비인용 자료로 전파하지 않으며, 기존 독립 출처 판정·캠페인·분 배분·보상을 유지하는 조건으로 ACK한다. 이 기획 ACK가 다른 레인의 ACK를 대체하지 않는다.

| 검사 | 기대 | 현재 증거 |
|---|---|---|
| JSON 형식·필드·유한 값 | 정상 | [OBSERVED] 읽기 전용 수학 검사 PASS, §6 |
| 고유 ID·서로 다른 점·비공선 | 3개 / 3개 / 비공선 | [OBSERVED] 읽기 전용 수학 검사 PASS, §6 |
| 공동 해·시작 오정렬·2스텝 도달 | 해 1개 / 시작 0점 일치 / 해에서 3점 일치 | [OBSERVED] 읽기 전용 수학 검사 PASS, §6 |
| FSM·취소·재개·비용·완료 술어 | 기존 계약 유지 | systems 구현·QA 회귀 대기 |
| 키보드·패드·포인터·화면 밖 복구 | 실제 조작 완결 | NOT-MEASURED |
| 정밀 입력 | 승인값과 실제 검증 | 미정, `fineGridStep:null` |
| 사람 이해·첫 조작 시간·재미 | 기존 검증계획 적용 | NOT-MEASURED |

## 6. 검증 영수증

[OBSERVED · 2026-09-10] 저장소 루트에서 `rtk proxy python3`의 읽기 전용 인라인 스크립트로 JSON을 읽어 검사했다. 추가 패키지·검사 프레임워크·생성 테이블 수정은 없다.

```yaml
command: rtk proxy python3 (read-only inline coordinate validation)
input: _workspace/current/planning/t0-circuit-overlay.json
checks: 16
passed: 16
failed: 0
exit_code: 0
solution: [-1, 1]
minimum_cardinal_moves: 2
checked:
  - expected-schema
  - three-anchors
  - unique-ids
  - canonical-labels
  - finite-coordinates
  - integer-coordinates
  - declared-steps
  - distinct-target
  - noncollinear-target
  - distinct-overlay
  - noncollinear-overlay
  - one-shared-solution
  - initial-zero-matches
  - solved-three-matches
  - cardinal-paths
  - neighbor-offsets-rejected
```

문서 검증은 Unity 입력·렌더링·성능·사람 플레이 검증을 대체하지 않는다. `mex-agent`는 세션 시작에서 unavailable로 확인됐으며 TeX 동명 바이너리는 실행하지 않았다. 이번 패킷은 코드 변경이 없어 코드 그래프 갱신 대상이 아니다. 교차 레인 승인과 G8 전체 판정은 디렉터가 별도로 기록한다.

[OBSERVED · 2026-09-10] `rtk proxy bash .claude/skills/game-ops-harness/scripts/freshness-check.sh` → exit 0, `0 finding(s) across 128 markdown artifact(s)`. 이는 frontmatter·supersedes 구조 검사만이며 시점 신선도와 memory_sync는 미측정이다.

[OBSERVED · RFC-CX-003 디렉터 후속 지시, 2026-09-10] 개발 통합 허용에 이어 §5의 명시 ACK가 모두 수집되어 저작 데이터 current 승격을 승인했다. 좌표 JSON의 내용·해시와 승인된 범위는 변경하지 않는다.

[OBSERVED · 승격 후 검증, 2026-09-10] 읽기 전용 Python 검사에서 current frontmatter, 필수 ACK 링크 4건의 실제 파일 존재, GDD current 표기, 좌표 JSON SHA-256 불변과 공백 검사가 모두 통과했다. 위 freshness 명령 재실행은 exit 0, `0 finding(s) across 136 markdown artifact(s)`이며 구조 검사의 경계는 동일하다.
