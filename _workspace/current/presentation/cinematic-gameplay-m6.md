---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-presentation-director
---

# M6 — 시네마틱 플레이 영화와 해금을 설명하는 플레이 방법 영화

[OBSERVED] 최신 요청은 **Higgsfield MCP**로 인트로·플레이·단계 해금·몰입을 담은 영상을 만드는 것이다. 영상 속 시네마틱 카메라 언어는 허용되지만 실제 Unity의 고정 시점 계약을 변경하라는 요청은 아니다. [TARGET] 네이티브 코드 변경 없이 **약36초 시네마틱**과 **약54초 방법 안내**를 만든다. 네 개의6초 raw shot(요청 합계24초)을 재사용하고 실제 네이티브 화면/읽을 수 있는 수동 타이포그래피를 결합한다. 실제 길이는 최종 편집 결과에 맞춰 각각30–40초/45–60초에서 조정한다.

[OBSERVED] 디렉터는 공식 `https://mcp.higgsfield.ai/mcp` 연결을 확인했다고 전달했다. 이 문서가 도구를 호출하지는 않았다. 실제 tool/model/job/요금은 실행 영수증으로 기록한다. M5의15credits/shot을 새 모델 가격으로 가정하지 않는다. 신규 keyframe이 필요하면 GTI를 사용하고 기존 리더·hub r03의 일관성을 지킨다.

## 이미 구현된 해금과 아직 계획인 구간

[OBSERVED] 정본 `planning/campaign.json`, 생성 데이터 `systems/data/t0/beats.json`, `C1PatrolDefinition`, `C1SignatureDefinition`, `T0Simulation`, `T0GameSession.CommitAsync`를 대조했다. M5 실제 납품 상태는 `production/intro-gameplay-m5-status.md`; M6 제작 완료 증거가 아니다.

| 구간 | 현재 완료/진입 조건 | 영상에서 해금을 표시하는 조건 |
|---|---|---|
| t0-b1→t0-b2 | 인수 각서3항·이관 목록3줄 열람, 판#0 상시 슬롯 적재, 목록을 기입/공란 중 하나로1회 기록 | 열람 술어. **별도 확정 명령 없음**. 가짜 봉인/도장을 만들지 않음 |
| t0-b2→t0-b3 | 음영3구획 모두 표시하고 각 구획에 근거1개 | 표시 술어. circuit은 판독 전용이고 **확정 명령 없음** |
| t0-b3→c1-b1 | 다른 sourceType·rootOriginId의 판/대장 인용, 검증 사본, 결손 양끝 고정 | `CiteToBoard` 후보의 저장 수락 후 실제 Journal에 반영된 완료 상태. 그 뒤 플레이어의 **C1 이동/EnterPatrol** |
| c1-b1→c1-b2 | 관찰2개, 유효한 현재 분기 구성, 출처 표기 조건 ACK, 최신 프리뷰 | `ConfirmPatrol` 저장 수락이 gateAccess·일지 조건·cp-c1-b1을 원자적으로 발행. 그 뒤 **명시적 EnterSignature** |
| c1-b2→현재 슬라이스 끝 | 관찰2개, 분리, 사본2개, 가려진 영역 표시, 명시적 대조, 근거 선택 | `ConfirmSignature` 저장 수락으로 사본/영역/번호대/근거/cp-c1-b2 발행. **c1-b3 플레이 해금을 표시하지 않음** |

[OBSERVED] C1 순찰로의 정확한 정답 조합은 코드/데이터에 있으나 공개 영상 문구에 수치/분기 답을 쓰지 않는다. 서명지 적정 습도도 공개하지 않는다. 두 사본은 같은 철의 출처이며 독립 매체2종이라고 표현하지 않는다.

[OBSERVED] `T0GameSession.CommitAsync`는 후보 Journal로 검증하고 `Store.WriteAsync`를 기다린 뒤 `Journal=candidate`로 공개한다. 저장대기·실패·취소·오래된 영수증에서 완료/해금으로 보이는 이펙트를 발행하지 않는다. **영상만의 해금표시도 이 실제 수락 증거 이후**에 놓는다. T0b1/b2에 존재하지 않는 확정 버튼을 추가하지 않는다.

[OBSERVED] `c1-b3`, `c1-b4` 및 C2–C7/E0는 campaign의 **계획**이다. 두 최종 영화에서 열리는 플레이 구간으로 보여주지 않는다. 선택적 로드맵이 필요하면 영화 바깥에 `계획·미구현`으로 분리한다. C1b2를 끝낸 뒤 문이 열리거나 C1b3 버튼이 활성화되는 합성도 금지한다.

## 네 개의 생성 샷 — 요청24초

모든 시간·각도·카메라 이동폭·몰입점수는 [TARGET]이다. 실제 모델이 재현했는지는 결과 프레임에서 측정한다. 정확한 프롬프트는 JSON `shots[].prompt`다. 모든 게임 글자는 provider에 그리게 하지 않는다.

### m6-s1-duty-room — 6초

```yaml
scene: m6-s1-duty-room
intent: 조용한 당직실이 아직 시작하지 않은 일의 책임을 느끼게 한다.
beats:
  - {start_ms: 0, end_ms: 1500, event: 빈 당직실과 검은 창밖을 읽을 여유}
  - {start_ms: 1500, end_ms: 4500, event: 아주 느린 접근으로 닫힌 무지 장부·수화기·빈 트레이에 주의}
  - {start_ms: 4500, end_ms: 6000, event: 움직임을 멈추고 다음 실제 화면을 위한 정적 여백}
camera: single restrained push-in; target apparent frame scale change <=3%; no cut; native camera unchanged
audio_cue: coastal-room-tone; no narration necessary
vfx_ref: rain_window atmosphere reference only; no simulated water-level change
anim_ref: static-props
motion_ref: previz-only-push-in
immersion_target: 3
measurement: not-measured
```

참고 원본: `assets/generated/2d/concept/intro-m5-r03/image.png`.

### m6-s2-observe — 6초

```yaml
scene: m6-s2-observe
intent: 재료를 먼저 살피는 행위가 질문을 만든다는 감각.
beats:
  - {start_ms: 0, end_ms: 2000, event: 연결된 낮은 판독기와 붙은 두 장을 함께 제시}
  - {start_ms: 2000, end_ms: 4500, event: 빛과 재질로 관찰 영역만 읽게 함; 가림은 그대로}
  - {start_ms: 4500, end_ms: 6000, event: 고정 홀드로 실제 관찰 UI를 읽을 여백}
camera: restrained lateral move <=3% frame width; stable prop geometry; native camera unchanged
audio_cue: room-tone plus one quiet paper-touch foley only on actual input insert
vfx_ref: none; opaque mask invariant
anim_ref: static-overlapped-papers
motion_ref: previz-only-lateral-drift-target
immersion_target: 4
measurement: not-measured
```

참고 원본: `assets/generated/2d/concept/c1-playflow-m5-r01/image.png`.

### m6-s3-trial-surface — 6초

```yaml
scene: m6-s3-trial-surface
intent: 판단 전에 조건을 시험하고 돌아올 수 있다는 절제된 손맛.
beats:
  - {start_ms: 0, end_ms: 2000, event: 눈금 없는 조절부와 청흑 금속 면}
  - {start_ms: 2000, end_ms: 4000, event: 실제 UI 입력을 별도 합성할 저대비 여백 유지}
  - {start_ms: 4000, end_ms: 6000, event: 제어는 원위치·상태는 그대로; 가역 시험을 설명할 홀드}
camera: locked macro frame; focus stays on control and material; native camera unchanged
audio_cue: one soft relay/knob foley only when paired with visible actual input
vfx_ref: none; no result implied by light
anim_ref: stationary-control
motion_ref: locked-frame
immersion_target: 3
measurement: not-measured
```

참고 원본: `assets/generated/2d/ui/m5-direction-surface-r01/reference-frame.png`.

### m6-s4-record-context — 6초

```yaml
scene: m6-s4-record-context
intent: 작업의 결과는 화려한 보상이 아니라 남겨지는 기록이라는 무게.
beats:
  - {start_ms: 0, end_ms: 2000, event: 빈 사각 기록 카드와 어두운 주변, 종이 미해결 부분은 가림}
  - {start_ms: 2000, end_ms: 4000, event: 빈 기록 공간으로 느리게 주의를 모음; 생성 글자 없음}
  - {start_ms: 4000, end_ms: 6000, event: 정적 홀드; 상태 문구는 편집에서 검증된 UI로만 추가}
camera: gentle approach to empty record holder <=3% scale; all props stationary; native camera unchanged
audio_cue: quiet paper settle; restrained tonal swell allowed only over accepted-receipt edit
vfx_ref: none; no seal_confirm or unlock effect in generated layer
anim_ref: blank-record-holder-static
motion_ref: previz-only-push-in
immersion_target: 4
measurement: not-measured
```

참고 원본: `assets/generated/2d/concept/c1-playflow-m5-r01/image.png`.


## 영화 A — 36초 시네마틱 인트로와 진행

| 편집 시간 | 영상 | 의미/수동 문구 |
|---|---|---|
| 0–6초 | S1 당직실 | 첫 당직의 긴장. `마지막 당직이 시작됩니다.` 실제 단서·결손·인물 정보 없음 |
| 6–12초 | 실제 T0b3 저장/명시적 C1 이동 발췌 | `저장된 기록에서 다음 작업으로.` 실제 수락 전에는 해금 문구 없음 |
| 12–18초 | 실제 C1b1 저장/서명지 이동 발췌 | `순찰 기록 저장 후, 서명지 작업으로.` 장면은 플레이 시간 압축 편집임을 표시 |
| 18–24초 | S2 관찰 | 붙은 두 장과 가림. `관찰` |
| 24–30초 | S3 조절부 | 가역 시험의 촉감. `시험` |
| 30–36초 | S4 기록 자리 + 검증된 실제 상태 오버레이 | `기록은 남고, 미해결은 가려진 채 남습니다.` 가림 제거/완료 도장 없음 |

[TARGET] 영화 전체에 `시네마틱 연출 프리비즈 · 실제 게임 화면 포함 · 편집 구성`의 출처 구분을 둔다. generated 부분의 카메라는 영화용이며 native 삽입 화면을 새 카메라 움직임으로 왜곡하지 않는다. 6초 인트로만 따로 배포할 때는 pre-t0-b1 공개 상한을 유지한다.

## 영화 B — 54초 플레이 방법과 실제 해금

| 편집 시간 | 영상 | 읽어야 하는 동사/상태 |
|---|---|---|
| 0–6초 | S1 + 수동 제목 | `관찰 · 시험 · 기록` |
| 6–15초 | 실제 T0b3 저장→C1이동 | 저장 완료가 먼저, 이동 입력이 다음. `기록 저장 완료 → C1 이동` |
| 15–24초 | 실제 C1b1 관찰·프리뷰·저장→서명지 이동 | `관찰과 출처 조건 확인 → 저장 → 서명지 작업` 정답 분기/값 설명 없음 |
| 24–30초 | S2 + 실제 관찰 UI | `자료를 선택해 살펴보세요.` 실제 선택과 현재 공개 상태만 표시 |
| 30–36초 | S3 + 실제 조건/시험 UI | `조건을 시험하고 되돌릴 수 있습니다.` 실제 입력 없이 움직이는 손잡이 없음 |
| 36–42초 | S4 + 실제 사본/가림/대조 UI | `사본과 미해결 부분을 남기고 대조합니다.` |
| 42–51초 | 실제 C1b2 후보→저장대기→수락 | UI의 `저장 확인 중`과 `저장 완료`를 구별. 새로 실패 화면을 발명하지 않음 |
| 51–54초 | 검증된 C1b2 최종 스틸 + 수동 문구 | `현재 구현: T0부터 C1 서명지 작업까지` 후속 구간 해금 없음 |

[TARGET] 실제 UI는 고정된 큰 패널로 배치하고 입력/초점/상태를 읽을 수 있게 한다. 생성 배경 움직임이 글자를 방해하면 해당 구간을 홀드 프레임으로 사용한다. 영화 길이는 게임의 클리어 시간이나 무중단 입력 기록이 아니다. 원본 native take와 편집 컷을 영수증에 연결한다.

## 해금 오버레이와 타이포그래피

[TARGET] 모든 제목·장 이름·화살표·저장/해금 문구는 네이티브 캡처 또는 수동 편집 글자다. provider 텍스트는 폐기한다. 실제 해금에는 **이전 상태 → 수락된 저장 영수증 → 이후 상태 → 명시적 이동**을 연결할 근거가 있어야 한다. 기존 M5 캡처에 그 구간이 없으면 해당 입력을 새로 캡처하거나 그 해금 주장을 뺀다. 생성 장면만 보고 해금이 입증됐다고 쓰지 않는다.

- 720p 출력 기준 공개 설명글 목표32px 이상, 최대2줄, 독해 최소3초. 실제 합성 대비4.5:1과 한글 글리프를 검사한다.
- `저장 확인 중` 위에 성공 색/도장/해금 제목을 덮지 않는다. 보상/문/빛이 먼저 움직여도 안 된다. 실제 수락을 보여준 뒤 최종 단계에만 매우 얕은 강조를 붙인다.
- 순찰로 수치·습도 정답·인물·후속 필적을 본문/자막/음성에서 선공개하지 않는다. 네 시간 결손은 t0-b3 실제 발견 이후 맥락에서만 가능하며 인트로에는 없다.
- 생성/실제 화면의 경계, 편집으로 시간이 압축된 사실을 구별한다. 경계 표시는 몰입을 방해하지 않는 일관된 작은 캡션으로 유지하되 판독 가능해야 한다.

## 절제된 오디오와 몰입

[TARGET] 비/바다의 낮은 당직실 룸톤 → 실제 자료 입력에 종이 소리 → 실제 제어 입력에 작은 릴레이/조절부 폴리 → **수락된 저장** 뒤1.2초 이하의 낮은 음색 상승 순서다. 점프스케어·강한 베이스 히트·승리 팡파르·도장 성공음을 가짜로 붙이지 않는다. 음성 내레이션은 기본 없음. 어떤 단서도 소리로만 전달하지 않는다.

[TARGET] 공식 Higgsfield MCP schema가 오디오 생성을 지원하면 원본 룸톤·폴리·톤을 생성하고 프롬프트/작업/비용/청취 검수를 남긴다. 없으면 이미 청취 승인된 원본만 재사용하거나 무음판으로 완료한다. 기존 M5의 미청취 오디오 후보를 자동 활성화하지 않는다. 영화 오디오 사용은 게임 런타임 오디오 승격과 별개다.

[TARGET] 최종 믹스 가이드: 통합−20 LUFS, true peak−1 dBTP 이하, 룸톤은 폴리보다 약8dB 낮게, swell 최대1200ms/퇴장500ms. 이는 측정 전 목표이며 최종 파일 측정·헤드폰/스피커 청취 후 조정한다. 무음판도 같은 정보를 전달해야 한다.

## 생성 결함에서 채택할 것과 버릴 것

[OBSERVED] M5 실제 검수는 카메라 드리프트·종이 자동 세척·새 줄무늬 문서·다이얼 문자 발명을 발견했다. M6는 그 실패를 프롬프트 금지와 편집 검수에 반영한다. 종이 하단 가림이 한 프레임이라도 사라지는 샷은 성공 표현으로 쓰지 않는다. 필요하면 종이가 없는 금속/기록 홀더 영역만 쓰거나 실제 네이티브 화면으로 교체하고 편집 사실을 기록한다. 원본 실패는 보존한다.

[TARGET] 원본→clip→검수 프레임→채택된 편집 구간/오디오→최종영화의 파일 해시와 시간대를 연결한다. GTI keyframe을 추가하면 영상 입력으로서의 출처를 기록한다. 영화에서 장식 자원을 다시 도출하더라도 미해결 마스크·근거 텍스트·저장 상태의 권위는 기존 게임 시스템에 남는다.

## 인수와 남은 검증

정확한15개 기준은 JSON `acceptance`이며 초기 저작 시점에는 모두 **pending**이었다. 생성4건의 실측 길이/모델/요금, 두 해금의 실제 영수증, 하단 가림, 수동 글자 가독성, 영상/실제 구분, muted 이해가능성, 청취·믹스, 최종30–40/45–60초 범위를 확인한다. 이미지/영상/오디오의 상업적 권리와 native runtime 자격은 별도 결정이다.

[OBSERVED] M5가 내부 프로토타입 사용만 승인한 자산은 M6에서 자동으로 상업 배포 허가를 얻지 않는다. `qa/intro-gameplay-m5-rights-review.md`의 GTI 경로/산출물 권리 구분을 유지한다. [OBSERVED] 이 저작 시점 M6 생성/최종렌더/청취 검증은 미실행이며, 인간 몰입 n=0이다. 몰입목표3/4와 멋있어 보이는 영화는 게임의 G4·재미·성과 증거가 아니다.

## 실제 편집 결정 — 원본 검사 후 적용

[OBSERVED · 디렉터 결정] 초기 목표/타임라인은 위에 보존했다. `docs/media/intro-gameplay-m5/native-gameplay.mp4`는 저장된 C1 순찰 완료 상태에서 시작하므로 **새 T0b3/C1b1 저장 수락 순간을 담고 있지 않다**. 앞선 해금은 **규칙 설명** 카드로 바꾸고, 실제10–19초는 **저장 상태 복원 후 서명지 이동**의 예시로 쓴다. native156–162.9초는 **저장된 C1b2 결과**이며 확정 클릭이 원본 take 경계에 걸쳐 있으므로 live click/pending/receipt 연속 증거라고 부르지 않는다.

편집 구간·정답 값 가림·캡션 변경은 `systems/tech-verification/cinematic-gameplay-m6/editorial-review.md` 및 `.json`이 정리한다. 방법 영화의 동사는 native23–29초 관찰 /60–66초 시험(선택값 가림)/88–94초 사본(요약 정답값 가림)으로 실제 예시를 제공한다. 초기 M6-P05의 새 저장 전이 증거 목표는 달성으로 처리하지 않고 **정확한 규칙 설명 + 복원 상태 예시**로 대체한다. 이 원본 구간 결정 시점의 최종 영화 검수는 대기였으며, 아래 최종 제작·검수 상태가 현재 판정이다.


## 최종 제작·검수 상태 — 2026-09-11

[OBSERVED] 공식 Higgsfield MCP4건 완료와 실제 비용/파일은 `systems/tech-verification/cinematic-gameplay-m6/mcp-production-receipt.json`에 기록했다. 최종 시네마틱36초/1080프레임, 방법54초/1620프레임을 독립 해시·메타데이터 및 편집 프레임 표본으로 확인했다. 실제 컷은 `docs/media/cinematic-gameplay-m6/edit-timeline.json`, 판정은 `systems/tech-verification/cinematic-gameplay-m6/editorial-review.json`이다. 원래 shot/film/audio 수치는 저작 목표로 보존한다.

[OBSERVED] JSON `acceptance`는 개별 판정으로 갱신했다. 모든 기준의 일괄 PASS는 아니다. 새 T0b3/C1b1 저장 전이 목표는 규칙 설명·복원 예시로 조정했고, 주요32px 자막은 확인했으나 연속3초 완전 가독시간·수치 대비는 미측정이다. 오디오는 로컬 저작 필터 노이즈를−27LUFS 목표로 믹스했으며 생성 오디오 채택·청취 승인·저모션 별도판은 완료로 처리하지 않는다. G4 인간 몰입, runtime 성능, 상업 권리와 게임 적용은 별도 미검증이다.
