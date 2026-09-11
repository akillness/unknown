---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-presentation-director
---

# M5 — 인트로와 플레이 방식 프리비즈에서 런타임 연출로

[OBSERVED] 사용자는 인트로와 플레이 영상을 먼저 만들고 그 영상에서 연출·리소스를 도출해 플레이 방식에 적용하도록 요청했다. 이미지 제공자는 최신 지시대로 GTI, 영상은 Higgsfield다. [TARGET] 이번 패킷은 영상 두 편과 작은 네이티브 인트로·상태 방향표 적용을 정의한다. 생성 성공, 실제 게임플레이, 인간 몰입 검증을 주장하지 않는다. 기계가 읽는 정본은 `intro-gameplay-m5.json`이다.

## 캐논과 재사용 경계

- [OBSERVED] `synopsis/scenes-and-dialogue.md` S1 및 `campaign.json:t0-b1`: 마지막 당직의 당직실, 인수 각서·목록·수화기·미봉인 염판 #0. 고정 당직실에서 작업대 뷰로 컷한다. 전체 S1 대사나 70초 장면을 6초 영상이 대체하지 않는다.
- [OBSERVED] `campaign.json:c1-b2` 및 `c1-signature-presentation.md`: 붙은 두 장의 가역 조건 시험, 개별 사본, 같은 철에서 나온 출처, 판 #0 대조. 하단 가림은 유지된다. 이번 프리비즈는 이 절차의 동사만 보여주고 적정 단수·인물 이름·대조 답·후속 필적을 보여주지 않는다.
- [OBSERVED] 기존 `video-study.md`의 채택 원칙은 조작 동사를 읽을 수 있게 하고 실제 Unity 캡처 전 생성 영상을 게임플레이라고 부르지 않는 것이다. `vfx-budget.md`의 `rain_window` 시각 문법을 참고하며 새 VFX 발행을 약속하지 않는다. `anim-list.md`를 확인했으며 `plate_insert`, `stamp_down` 등 승인이 필요한 클립을 신규 저작하거나 구현했다고 주장하지 않는다.
- [TARGET] 신규 인트로는 **pre-t0-b1 공개 상한**이다. 네 시간 결손은 t0-b3에서 발견하므로 인트로의 문장·이미지·도표·소리로 확립하지 않는다. 이름·값·재구성도 없다.
- [TARGET] C1 플레이 방식 영상은 기존 승인 판독기 r02를 참고한다. 인트로는 hub wide만 참고하고 닫힌 장부·수화기·빈 트레이만 사용한다. C1 서명지·판독기 프레임을 인트로 끝 프레임으로 결합하지 않는다. 두 영상은 각자의 시간대에 속하며 별도로 배포한다. 종이·금속 팔레트는 공유한다. 생성 영상의 잘못된 기계 구조를 새 캐논으로 삼지 않는다. 영상은 조형의 참고이며 근거 판정 엔진이 아니다.

## 영상 타임라인

이 절의 ms는 **생성 전 원래 영상 요청 목표**이며 몰입 점수는 [TARGET]이다. 최종 네이티브 시간은 아래 채택 계약의 **3125 / 2875 / 6000 ms**가 정본이다. `0 ms` 컷은 표시 프레임에서 정적으로 바뀐다는 저작 규칙이며 측정된 지연이 아니다. 요청 6000 ms와 실제 제공자 결과 길이는 각각 보관한다. 지원 모델이 5초나 8초만 허용하면 디렉터가 요청값·컷 위치를 비례 조정해 실제 프롬프트를 별도 영수증에 보존한다.

```yaml
scene: m5-intro
intent: 빈 작업대가 마지막 당직의 책임을 기다린다고 느끼게 한다.
beats:
  - {start_ms: 0, end_ms: 2500, shot: hub-wide, event: 낮은 창·수화기·젖은 책상의 공간 맥락}
  - {start_ms: 2500, end_ms: 6000, shot: workbench-fixed, event: 닫힌 장부·수화기·빈 트레이; 판독기·염판·서명지는 없음}
camera: 고정 노드 두 개; 하드 컷 1회; 팬·줌·흔들림·초점 이동 없음
audio_cue: none
vfx_ref: rain_window 문법만 참고; 물높이 변화 없음
anim_ref: static-pose
motion_ref: hard-cut-0ms
immersion_target: 3
measurement: not-measured
```

```yaml
scene: m5-gameplay-previz
intent: 관찰하고 가역 시험한 뒤 근거를 기록하는 절차를 이해하되 답은 직접 찾게 한다.
beats:
  - {start_ms: 0, end_ms: 2000, shot: observe, event: 붙은 두 장과 하단 가림을 관찰}
  - {start_ms: 2000, end_ms: 4000, shot: reversible-operation, event: 눈금 없는 조절 손잡이를 조금 돌렸다 복귀}
  - {start_ms: 4000, end_ms: 6000, shot: unconfirmed-record, event: 빈 기록 자리와 서로 다른 매체 외곽}
camera: 고정 노드 세 개; 하드 컷 2회; 팬·줌·흔들림 없음
audio_cue: none
vfx_ref: none; 하단 불투명 가림 유지
anim_ref: neutral-knob-previz-only; 런타임 신규 클립 아님
motion_ref: hard-cut-0ms
immersion_target: 4
measurement: not-measured
```

각 샷 안에서 오브젝트가 생기거나 사라지지 않는다. 시험 손잡이가 가리키는 단수는 없고 종이는 끝까지 붙어 있다. 마지막 빈 카드는 사본 확보·증거 확정·저장 영수증을 뜻하지 않는다. 생성 텍스트/HUD/도장을 넣지 않는다. 한국어 동사나 설명이 필요하면 네이티브 UI 또는 별도로 기록된 편집 자막으로만 렌더한다.

## 정확한 영상 프롬프트

### Higgsfield intro

Create a six-second, silent visual previsualization for a restrained fixed-view archival investigation game. This is concept previs, not gameplay footage. Two locked camera shots with one hard cut at 2.5 seconds, no camera movement. From 0 to 2.5 seconds show an unoccupied coastal duty room at night, low ceiling, low horizontal window, black water horizon at forty percent of frame height, worn blue-grey metal desk, an old telephone handset, a closed rectangular ledger and a blank paper tray. A small steady ochre work lamp is the only warm accent. From 2.5 to 6 seconds hard-cut to a fixed closer view of the same generic desk: the same closed ledger, old telephone handset and empty paper tray under the steady lamp. No reader, salt plate, signature sheets, experimental controls, case evidence or chapter-one equipment appears. Preserve object geometry and positions within each shot. The desk awaits inspection; no finding or ownership is established and nothing resolves itself. Weathered matte surfaces, fine salt rings at joins, downward corrosion streaks, quiet pressure, legible large silhouettes, deep ink #0E1F26, wet metal #36565C, patina #4F7A6B, salt #C8D6D3, paper #E7E3D8, ochre #E2AF62 under eight percent of the frame. Thirty-five millimetre equivalent locked lens, level roll, slightly downward angle. Tiny rain trails on the distant window only; no changing water level. No text, letters, numerals, signatures, logos, watermarks, titles, subtitles, HUD, menus, readable documents, hidden names, named people, solved puzzles, success stamps, flashing lights, red warnings, combat, weapons, holograms, neon, camera movement, zoom, pan, shake, orbit, rack focus, dissolves, floating parts, or new objects appearing inside a shot.

### Higgsfield gameplay previz

Create a six-second, silent mechanics previsualization for a fixed-view archival investigation game. This is a staged concept of observation, reversible operation and recording, not captured gameplay and not a puzzle solution. Three locked views with hard cuts exactly at two and four seconds, with no camera movement or transition effects. Use the same weathered blue-grey circular reader, connected swing arm, shallow support and low ochre lamp throughout. From 0 to 2 seconds: a broad fixed workbench view, two blank rectangular paper sheets with offset corners and a salt-bound edge, an opaque irregular salt shadow across the lower quarter of the top paper; the shadow never clears. From 2 to 4 seconds: a fixed closer view of one unmarked mechanical adjustment knob beside that same shallow support; the knob turns a small neutral amount and returns to its original position, the papers stay overlapped and no setting or solution is disclosed. From 4 to 6 seconds: a locked workbench composition with the unchanged papers on the left and a separate empty rectangular note card in a metal holder on the right; an unlettered hexagonal plate remains visually distinct from the rectangular paper. The empty note card is an unconfirmed place to record, never a success receipt. No paper separation, copy creation, stamp or factual revelation occurs. Large stable silhouettes, tactile matte salt crystals and worn metal with downward patina, restrained archival atmosphere, deep ink #0E1F26, wet metal #36565C, patina #4F7A6B, salt #C8D6D3, paper #E7E3D8, ochre #E2AF62 under eight percent. No text, letters, numerals, signatures, logos, watermarks, titles, subtitles, HUD, menus, readable documents, hidden names, named people, solved puzzles, success stamps, flashing lights, red warnings, combat, weapons, holograms, neon, camera movement, zoom, pan, shake, orbit, rack focus, dissolves, floating parts, or new objects appearing inside a shot.

## 영상에서 실제 적용으로 넘어가는 순서

0. [TARGET] GTI 원본 스틸 두 장(hub wide, workbench close)을 영상의 입력 참고로 생성한다. 정확한 프롬프트는 JSON `sourceStills`다. 이것은 영상 관찰 후 채택할 런타임 자산과 구분한다. 원본 스틸도 검수·승격 전 후보 상태다.
1. [TARGET] 디렉터가 실제 영상의 시작·컷 전후·끝 프레임을 검사하고 고정 시점, 읽기 영역, 기계 연결, 텍스트·스포일러, 하단 가림을 기록한다. 요청된 컷이 결과에 없으면 있는 것처럼 적지 않는다.
2. [OBSERVED] 채택할 프레임의 시간·해시·이유를 검수 기록에 연결했다. 실제 C1 영상 **2.5초** 프레임을 참고해 GTI가 **글자 없는 어두운 금속 UI 표면 1장**을 새로 만들었다. 이미지와 영상은 각각 실제 출처를 기록하며 시작 상태는 `runtimeEligible:false`, 승격은 디렉터가 별도로 소유한다.
3. [OBSERVED · 디렉터 채택] 인트로는 기존 깨끗한 hub r03 정적 이미지를 사용한다. 실제 C1 영상 2.5초 프레임에서 **어두운 금속 UI 표면**을 GTI로 새로 도출했다. [TARGET] 이 두 이미지의 승인 범위는 내부 프로토타입이며 provenance 승격은 디렉터가 별도로 기록한다. 영상 자체는 런타임에 임포트하지 않는다.
4. [TARGET] 네이티브 입력으로 인트로 진입·건너뛰기·복귀를 캡처하고 기존 관찰→시험→기록 조작을 실제로 실행한다. 생성 프리비즈와 네이티브 캡처를 별도 파일·캡션으로 배포한다.

## 작은 네이티브 적용 제안

[TARGET] **읽기 전용 인트로와 지속되는 관찰·시험·기록 방향표**를 적용한다. 인트로는 신규 시작 시 표시한다. 일반 모드의 최대 자동 진행 시간은 6000 ms, 최소 대기 시간은 0 ms다. **hub wide** 이미지 한 장과 `관찰 · 시험 · 기록` 세 라벨을 쓴다. 텍스트는 첫 프레임부터 모두 읽을 수 있고 캡션/문맥은 **첫 3125 ms + 다음 2875 ms**로 바뀌며 전체 6000 ms다. 3125 ms는 실제 인트로 영상 컷에서 채택했다. 실제 제공자 영상의 6041.667 ms와 네이티브 총 6000 ms를 구분한다. 현재 강조는 정적인 윤곽만 바꾼다. 이 강조는 단계를 완료했다는 배지와 구분한다. 이 화면에서 퍼즐 조작을 대신 수행하지 않는다.

- 안내 문장: **자료를 살피고, 조건을 시험하고, 근거를 기록하세요.** 인물/장/해답을 추가하지 않는 조작 설명이다.
- 접근성 설정은 인트로 도중에도 접근 가능하다. 설정을 위해 영상이나 타이머가 끝날 때까지 기다리게 하지 않는다.
- `건너뛰기`/`시작`은 첫 프레임부터 포인터와 키보드로 작동한다. Escape/기존 뒤로 입력은 닫기이며, 그 입력이 뒤의 게임 버튼을 함께 누르지 않도록 소비한다. 초점은 보이는 제어에 놓인다.
- 기존 세이브로 재개하면 인트로를 강제하지 않는다. 다시 보기만 명시적 입력으로 허용한다. 확인을 기록하려고 세이브 버전을 바꾸지 않는다.
- 저감모션은 이미지 하나와 세 라벨 전체를 바로 보여주고 **시간을 기다리지 않는 명시적 시작**을 사용한다. 장식 모션·깜빡임·파티클·오디오 재생은 없다.
- 150% 텍스트에서도 라벨·문장·건너뛰기·시작이 잘리지 않도록 줄바꿈/스크롤한다. 그림 없이도 동일하게 읽고 조작할 수 있다. 이미지 로드 실패 시 심해 잉크 단색 배경으로 같은 제어를 제공한다.
- 자동 시간 종료·건너뛰기·재생 종료·이미지 로드·초점 이동은 시뮬레이션, 단서, 명령 이력, 세이브, 체크포인트를 변경하지 않는다. 입력 잠금은 인트로 UI의 뒤쪽 입력 차단에만 적용하며 플레이 중 SavePending 전역 잠금으로 확장하지 않는다.

## 플레이 중 지속되는 방향표

[TARGET] 인트로가 닫힌 뒤에도 기존 C1 서명지 UI의 작은 띠에 `관찰 · 시험 · 기록`을 남긴다. **선택한 제어의 종류**와 **이미 플레이어에게 보이는 현재 상태**에만 정적으로 반응한다. 텍스트/윤곽을 함께 써 색 단독 표시는 피한다. 이 띠 자체는 초점을 빼앗거나 별도의 게임 명령을 만들지 않는다.

- 문서/자료 선택은 `관찰`, 조건/시험 제어 선택은 `시험`, 사본/대조/확정 제어 선택은 `기록`을 강조한다. 선택하지 않았으면 세 라벨을 중립적으로 둔다. 다음에 눌러야 할 정답 버튼을 추측해 강조하지 않는다.
- 상태 줄은 기존 뷰가 이미 읽는 관찰 여부·선택값·시험 결과·후보/저장 대기/실패/성공 상태를 재사용한다. 새로운 퍼즐 완료 술어를 UI에 복제하지 않는다. 올바른 습도 값·후속 인물·미관찰 단서는 덧붙이지 않는다.
- 포인터로 선택한 제어와 키보드로 선택한 제어가 같은 방향표를 낸다. 표시 갱신은 0 ms 정적 컷이며 키보드 초점/스크롤 위치를 재설정하지 않는다. 저감모션에서도 동일한 정보다.
- 성공 문구는 승인된 저장 영수증이 현재 상태에 반영된 뒤에만 나온다. 연출 타이머/영상 컷/완주 여부로 라벨을 성공으로 바꾸지 않는다.

## 동사와 게임 로직의 대응

| 프리비즈 동사 | 네이티브에서 의미하는 것 | 연출이 절대로 대신하지 않는 것 |
|---|---|---|
| 관찰 | 실제 선택 자료의 현 스냅샷과 읽을 수 있는 본문 | 미관찰 단서 공개, 정답 위치 강조 |
| 시험 | 플레이어가 선택한 조건을 명시적으로 시험; 결과와 되돌림 읽기 | 시간 경과로 조작, 정답 단수 제시, 원본 영구 손실 |
| 기록 | 실제 사본·대조 후보 및 저장 상태를 구별하는 기존 UI | 두 사본을 독립 출처로 취급, 미성공 저장의 완료 표시 |

## 인수와 영향 패스

정확한 13개 기준은 JSON `acceptance`다. 핵심은 첫 프레임 스킵/입력 소비, 저감모션 즉시 시작, 150% 가독성, 재개 강제 없음, 미디어 실패 복구, open/skip/timeout/replay 전후 상태와 세이브 바이트 동일성이다. `intro-gameplay-m5.json`의 목표 시간을 실제 측정값으로 전사하지 않는다.

[TARGET] 전후 비교는 **기존 즉시 진입 화면 → M5 인트로/안내 → 동일 상태로 복귀**의 실제 캡처로 한다. 영상의 관찰/시험/기록 순서가 플레이어가 무엇을 할지 이해하는지 확인한다. [OBSERVED] 이 문서 작성 시 사람 평가 n=0, G4 몰입 점수 없음, G5 프레임시간/GPU 메모리 측정 없음. 목표 점수 3/4는 PASS가 아니다.

## 실제 영상 검수 연결

[OBSERVED] 두 영상의 실제 파일/시간/해시/프레임 검수와 채택·기각은 `intro-gameplay-m5-video-review.md`에 기록했다. 인트로 컷은 실제 3125 ms, C1 컷은 2083.333/4041.667 ms이며 위 저작 목표와 다르다. 네이티브는 깨끗한 hub r03 정적 이미지의 두 캡션 단계와 C1 방향표로 적용한다. 생성 영화의 줄무늬 종이·다이얼 표시·카메라 드리프트·자동 가림 제거는 기각한다. [OBSERVED · 디렉터 채택] 실제 인트로 컷 3125 ms를 수용하고 두 번째 캡션은 2875 ms로 정해 네이티브 합계 6000 ms를 유지한다. 영상 이후 GTI 금속 UI 표면도 생성·시각 검수했다. [TARGET] 네이티브 합성 화면/입력/저장 검증은 별도이며 이 패킷은 구현 PASS를 주장하지 않는다.

## 최종 승인된 자산 연결과 제작 순서

[OBSERVED · 디렉터 채택] **GTI 참고 스틸 → Higgsfield 영상 두 편 → 실제 컷/프레임 검수 → GTI 금속 UI 표면 → 네이티브 정적 연출 적용 계약**의 순서다. 영상 이전 원본 이미지를 영상 이후 새로 만들었다고 부르지 않는다.

| 실제 입력 | 도출/선택 리소스 | 네이티브 적용 계약 | 남은 확인 |
|---|---|---|---|
| 인트로 영상의 공간→작업대 컷 3.125초 | `assets/generated/2d/concept/intro-m5-r03/image.png`의 깨끗한 원본을 유지 | 고정 배경 1장, 캡션 3125/2875 ms; 합계 6000 ms | 실제 스킵·설정·저감모션·재개·150% |
| C1 영상 2.5초의 금속 조절부 근접 프레임 | `assets/generated/2d/ui/m5-direction-surface-r01/image.png`를 영상 이후 GTI로 생성 | 관찰/시험/기록 방향표의 조용한 장식 표면; 현재 선택과 실제 영수증 상태만 읽음 | 합성 대비·키보드 초점·상태/세이브 불변성 |

[OBSERVED] 새 표면은 1254×1254이며 문자·단서·기계·종이 형상 없는 저대비 청흑 표면으로 시각 검수했다. [TARGET] 배경이나 표면의 시각 승인만으로 런타임 사용 자격·상업 배포 권리·성능 승인을 부여하지 않는다. provenance 승격은 디렉터가 소유한다.
