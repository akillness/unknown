---
updated: 2026-09-10
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-qa
---

# T0 M2 저작 데이터·리소스 독립 QA

**[OBSERVED · 통합 후속, 2026-09-10 08:34:08 UTC]** 아래 07:56 UTC 생성기 미반영 스냅샷은 최신 근거로 해소됐다. 현재 생성 records와 Unity runtime records 바이트, synopsis/records 영수증 해시가 일치하며 PlayMode3에서 실제 표준판·대장 인용 쌍으로 t0-b3 완료·성공 영수증 2개·저장 재로드 StateHash 일치를 확인했다. C7-F50은 closed이며 정확한 테스트 방식과 해시는 [코드 QA §7](t0-m2-code-review.md#7-playmode3-복구인용-실행-영수증)에 기록했다. 아래 원래 시점의 기록은 보존한다. 이 후속은 생성 리소스 runtimeEligible 승격이나 전체 릴리스 판정이 아니다.

**[OBSERVED] 판정: 출처 귀속과 회로 좌표의 소스 수준 QA ACK.** Blender 산출물은 파일 구조·해시·정지 미리보기 검사를 통과했다. 이 ACK는 런타임 사용 승격, Unity 임포트, 저장/입력 구현, 플레이테스트 또는 캠페인 게이트 PASS가 아니다. 리소스 `runtimeEligible:false`와 회로 패킷의 draft 지위를 유지한다.

## 1. 범위와 신선도

검토자 `/root/t0_resource_qa`는 다른 레인의 파일을 수정하거나 Unity를 실행하지 않았다. 검토는 2026-09-10 07:51–07:57 UTC에 현재 작업 디렉터리의 파일을 직접 읽어 수행했다. 수치와 판정은 아래 입력 스냅샷에만 유효하다.

| 입력 | 확인한 지위 | 범위 |
|---|---|---|
| `synopsis/t0-records.md` §12 | `status: current`, `supersedes: null`; 절은 TARGET 저작 결정 | 두 인용 자료의 기계 판독 귀속 |
| `worldview/glossary.md` §6-2 | `status: current`; supersedes 대상 `_workspace/archive/20260909-preproduction-c2/worldview/glossary.md` 실제 존재 | 비교 기준과 관측 생산지의 구분 |
| `planning/t0-circuit-overlay.json` + `.meta.md` | 메타데이터 `status: draft`, `supersedes: null` | 좌표·라벨·평행 이동 해 |
| `assets/generated/3d/hub-view-drawer-r01/` | `runtimeEligible:false` | 실제 GLB, FBX/Blend 존재·해시, 측정 파일, 미리보기 |
| `assets/generated/audio/higgsfield-stamp-r01/` | `runtimeEligible:false` | WAV PCM·해시·생성 출처 |

```yaml
measured:
  timestamp: 2026-09-10T07:54:22.864Z
  session: /root/t0_resource_qa
  session_start_command: rtk run 'bash .claude/skills/game-ops-harness/scripts/session-start.sh "T0 M2 authored packet and resource independent QA"'
  session_start_log: /tmp/unknown-t0-m2-resource-qa-session.log
  freshness_output: "freshness: 0 finding(s) across 128 markdown artifact(s) under _workspace/current"
  freshness_scope: frontmatter and supersedes topology only
  staleness_measurement: NOT-MEASURED
  memory_sync: NOT-MEASURED
```

세션 시작 도구는 mex-agent를 찾지 못해 해당 검사를 건너뛰었다. 이 결과로 G8 전체 PASS를 주장하지 않는다. 서류가 current여도 새 TARGET 귀속의 교차 레인 승인과 런타임 검증이 완료되었다는 뜻은 아니다.

## 2. 출처 귀속 QA ACK

**[OBSERVED]** `T0-SOURCE-ASSIGNMENTS:START/END` 사이 fenced JSON을 실제 `JSON.parse`로 읽었다. 배열은 정확히 아래 두 행이며 다른 레코드로 전파되지 않는다.

| recordId | systemId | stationId | QA 의미 검토 |
|---|---|---|---|
| `rec-plate-standard-hub` | `system-hub` | `station-bureau-standard` | 기존 허브 계통에 기술 ID를 부여하고, 관측소는 이번에 채택한 비교 기준으로 연결 |
| `rec-tide-ledger-bureau` | `null` | `station-bureau-standard` | 기존 기록국 표준 조위 기준에 기술 ID를 부여; 염판 계통은 비해당 |

`systems/data-schemas/plates.md`의 `systemId`는 `string?`이며 `sourceType == plate`일 때만 필수다. 따라서 수치대장의 null은 누락된 염판 계통과 구분된다. `stationId`는 기준 관측소·조위정합 입력이다. 표준판의 압력·염도·갑문 내측 수위를 이 관측소가 생산했다는 관측 사실로 해석하지 않는다.

`rec-handover-brief`, `rec-transfer-list`, `rec-watchlog-bureau`는 패킷에 없다. 미정인 귀속을 null 비해당으로 덮어쓰거나 `zoneId`·회선명·공유 수치로 추정할 근거는 없다. 회선 세 곳의 통화 범위는 조위 기준 관측소를 대신하지 않는다. 출처 독립성의 `sourceType + rootOriginId` 기준은 유지된다.

```yaml
measured:
  timestamp: 2026-09-10T07:54:00Z
  method: node filesystem read; marker extraction; JSON.parse; exact record/id equality checks
  command_session: mcp__node_repl__js in /root/t0_resource_qa
  expected_assignments: 2
  parsed_assignments: 2
  unexpected_record_assignments: 0
  source_schema_rows_verified: [systemId, stationId]
  verdict: ACK-SOURCE-ONLY
```

**[OBSERVED · 통합 대기]** 07:56 UTC에 읽은 `systems/data/t0/records.json`은 아직 새 `systemId`/`stationId` 필드가 없었고 `recordsDocSha256`가 최신 synopsis 원문의 SHA-256과 일치하지 않았다. 원본 `sourceType`·`originId`·`rootOriginId` 다섯 행은 campaign의 T0 단서 신원과 일치했다. 이는 진행 중인 emitter 통합의 스냅샷이며 새 소스 결함으로 중복 등록하지 않는다. **C7-F50은 이 QA ACK만으로 닫지 않는다.** 생성기 재실행·영수증·임포트·런타임 검증은 systems 후속 증거가 필요하다.

## 3. 회로 저작 패킷 QA ACK

**[OBSERVED]** 표시명 `당직실`·`제3수문`·`부두사무소`는 용어집 §1의 기존 명사와 일치한다. 좌표는 새로 저작한 도식 격자이며 지리·센서·거리·관측값으로 제시하지 않는다. 메타데이터의 전체 투명지 평행 이동 의미는 각 점의 `target - overlay` 계산과 일치한다.

| anchorId | target | overlay | 공통 정렬 오프셋 |
|---|---|---|---|
| `anchor-hub` | (0, 0) | (1, -1) | (-1, +1) |
| `anchor-gate` | (3, 0) | (4, -1) | (-1, +1) |
| `anchor-dock` | (1, 2) | (2, 1) | (-1, +1) |

```yaml
measured:
  timestamp: 2026-09-10T07:54:22.864Z
  method: JSON.parse; finite coordinates; set uniqueness; target-minus-overlay; determinant; exact equality
  command_session: mcp__node_repl__js in /root/t0_resource_qa
  anchors: 3
  unique_ids: 3
  distinct_targets: 3
  distinct_overlays: 3
  finite_coordinates: true
  target_noncollinearity_determinant: 6
  shared_translation_solution_count: 1
  solution: [-1, 1]
  initial_offset: [0, 0]
  initial_matches: 0
  solved_matches: 3
  neighbor_offset_matches: [0, 0, 0, 0]
  grid_step: 1
  fine_grid_step: null
  minimum_cardinal_grid_moves: 2
  verdict: ACK-SOURCE-ONLY
```

이동 수는 수학적 파생값이며 플레이 시간·난이도 측정이 아니다. 미승인 정밀 스텝은 null로 유지한다. `OpenTool → TraceSystem → BeginOverlay → SetOverlayOffset → AnchorOverlay → Marking` 실제 입력/FSM, 취소·재개·저장 복구는 이 검토에서 실행하지 않았다. QA ACK는 저작 수학과 의미에 한정하며 다른 레인의 승인 또는 디렉터 승격을 대신하지 않는다.

## 4. Blender 산출물 검토

**[OBSERVED]** `SM_Hub_Workbench_Drawer.glb`의 이진 헤더와 JSON chunk/accessor를 직접 해석했다. glTF 2.0이며 헤더의 파일 길이가 실제 바이트와 일치한다. 장면의 루트는 drawer 루트 하나이고 두 메시는 casing/tray다. factory Cube·Camera·Light는 GLB 장면에 없다. 실제 인덱스 수로 센 삼각형 수가 Blender 측정 파일과 일치한다.

```yaml
measured:
  timestamp: 2026-09-10T07:54:22.864Z
  command_session: mcp__node_repl__js in /root/t0_resource_qa
  method: GLB header/JSON/accessor decode; SHA-256 of exported files
  glb_bytes: 11952
  mesh_count: 2
  triangles: 156
  triangle_budget_target: 1500
  embedded_images: 0
  animations: 0
  matching_provenance_hashes: 5
  mismatching_provenance_hashes: 0
  runtime_eligible: false
  verdict: ACK-STRUCTURE-AND-STATIC-PREVIEW-ONLY
```

해시 일치 대상은 `.blend`, `.glb`, `.fbx`, `measurements.json`, `preview.png`다. `.blend`/`.fbx`는 존재·해시만 독립 확인했으며 다시 임포트하지 않았다. 캐논 GLB를 유지하고 FBX는 RFC-CX-003의 Unity 임포트 후보 파생본으로 취급한다.

**[OBSERVED]** `functions.view_image`로 `preview.png`를 확인했다. 닫힌 케이싱·서랍 전면·중앙 손잡이가 분리되어 읽히며, 이 시점의 단독 시점에서는 앞면 가림이 보이지 않는다. 작업대 안에 배치했을 때의 가림·실제 접근성·콜라이더·Unity 재질·축·애니메이션은 증명하지 않는다. 기존 작업대의 solid-box 형태 때문에 통합 시 앞면 보정 또는 별도 검사 시점이 필요하다는 제한을 유지한다. 서랍은 기존 작업대 부착물이며 새로운 제작 신원으로 집계하지 않는다.

## 5. Higgsfield 음원 검토

**[OBSERVED]** WAV의 RIFF/fmt/data chunk와 실제 PCM 표본을 읽었다. 음원 SHA-256은 `provenance.json`과 일치한다. 모델은 `seed_audio`, 작업 ID는 `fe592003-237f-4fc5-a836-f5d33ed7381c`이며 원본 프롬프트가 존재한다. 비용 견적은 2.2 credits, 실제 청구량은 null이므로 확정 비용으로 보고하지 않는다.

```yaml
measured:
  timestamp: 2026-09-10T07:55:00Z
  command_session: mcp__node_repl__js in /root/t0_resource_qa
  method: RIFF/PCM decode; SHA-256; normalized signed-16-bit sample peak and RMS
  format: PCM16
  channels: 2
  sample_rate_hz: 24000
  duration_seconds: 3.5
  bytes: 336044
  absolute_peak: 0.94622802734375
  rms: 0.033815205853855286
  clipped_samples: 0
  hash_matches: true
  auditory_review: NOT-PERFORMED
  runtime_eligible: false
  verdict: ACK-METADATA-ONLY
```

이 검토의 도구 경로에서는 청취하지 못했다. 발화·음악 부재, 도장 질감, 잔향·노이즈 적합성, 게임 믹스 및 권리 조건은 판정하지 않는다. 음원은 성공한 저장 영수증 뒤의 확인용 후보이며 SavePending 또는 저장 실패 때 확인음으로 재생하면 안 된다. PCM 클리핑 부재가 청각적 품질 검증을 대신하지 않는다.

## 6. 통보와 후속 경계

| 항목 | 상태 | 후속 책임 |
|---|---|---|
| 귀속 두 행·비교 기준 의미 | QA 소스 ACK | 디렉터가 나머지 교차 레인 ACK와 통합 범위 기록 |
| 회로 세 점 수학·기존 라벨 | QA 소스 ACK | 기획의 draft 승격과 systems 입력/FSM 검증 별도 |
| C7-F50 생성 데이터 반영 | 기존 open 유지 | systems 생성기·영수증·런타임 검사 |
| Drawer 구조/해시/정지 시점 | 범위 한정 ACK | modeler 독립 감사와 systems Unity 임포트·fit 검증 |
| 음원 PCM/해시 | 메타데이터 ACK, 청취 미실시 | 청취·권리·Unity 믹스·저장 성공 연동 감사 |
| G1–G8 전체·사람 플레이·기준 하드웨어 성능 | 이 리뷰에서 NOT-MEASURED | 이후 독립 런타임 QA와 실제 플레이테스트 |

새 S1–S4 소스 결함은 이 범위에서 발견하지 않았다. 개발 중인 통합과 미실시 검사를 PASS로 올리지 않았다. director `/root`와 `/root/t0_systems`에 소스 ACK, 리소스 범위, 생성 테이블 스냅샷의 통합 대기를 전송했다. inactive planner 직접 통보는 `agent thread limit reached`로 전달되지 않아 director에게 중계를 요청했다.

`feedback-requested-by: 2026-09-10`

**[OBSERVED · 최종 문서 검증, 2026-09-10 07:57:35 UTC]** `rtk run 'bash .claude/skills/game-ops-harness/scripts/freshness-check.sh'`는 exit 0과 `freshness: 0 finding(s) across 132 markdown artifact(s) under _workspace/current`를 반환했다. 소유 보고서의 필수 frontmatter, 단일 H1, 미측정/승격 제한 명시도 읽기 전용 검사로 확인했다. 이 최종 구조 검증에도 memory_sync·staleness·G8 전체 PASS는 포함하지 않는다.
