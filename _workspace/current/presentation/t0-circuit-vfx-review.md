---
updated: 2026-09-10
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-presentation-director
---

# T0 회로·저장 확인 VFX 연출 검토

[OBSERVED · RFC-CX-003] **연출 레인 scoped ACK**: `planning/t0-circuit-overlay.json`과 `vfx/vfx-spec.json`의 문서 계약은 아래 범위에서 기존 T0 서사·시각·타이밍 기준과 양립한다. 이 ACK는 디렉터의 교차 레인 통합 판단에 제공하며, 원본 패킷의 상태·런타임 적격성·게이트를 직접 승격하지 않는다.

```yaml
review_id: t0-circuit-vfx-presentation-r1
rfc: RFC-CX-003
verdict: ack
scope: authored-data-and-presentation-contract
scene: hub
beat: t0-b2
circuit_packet: planning/t0-circuit-overlay.json
vfx_packet: vfx/vfx-spec.json
new_beats: 0
new_geographic_claims: 0
new_timing_values: 0
runtime_capture: null
human_playtest_count: 0
immersion_score: null
g4_g5_pass: false
```

## 회로의 의도와 가독성

[CARRIED] `synopsis/chapter-beats.md`의 `t0-b2`는 당직실에서 회로 도구로 기록 범위를 배우는 비트다. [TARGET · 기획 패킷 인용] 세 대응점을 직접 겹쳐 기록 범위를 읽을 준비를 한다는 의도에 ACK한다. 좌표는 도면 내부 배치이며 항구 방위·실측 거리·센서 설치 위치가 아니다. 새 사건·구역 이동·단서 공개를 추가하지 않는다.

| 기존 표시명 | 고정점 | 투명지 점 | 공통 이동 |
|---|---|---|---|
| 당직실 | (0, 0) | (1, −1) | (−1, +1) |
| 제3수문 | (3, 0) | (4, −1) | (−1, +1) |
| 부두사무소 | (1, 2) | (2, 1) | (−1, +1) |

[OBSERVED · JSON 직접 비교] 세 점은 서로 다르고 일직선에 놓이지 않으며, 하나의 평행이동으로 대응한다. [CARRIED/TARGET 구분] `gridStep: 1`과 세 점 일치는 기존 요구이며, 위 좌표와 시작 오프셋 `(0,0)`은 RFC-CX-003 신규 저작값이다. `fineGridStep: null`은 미정 값을 나타내며 정밀 스텝을 새로 정하지 않는다.

[TARGET · 구현 수용 조건] 투명지는 세 점을 함께 이동한다. 고정 도면과 이동 도면을 구별하고, 대응 표시명과 정렬 여부는 색 외에 선·형태·문자로도 식별되게 한다(`concept/style-guide.md`의 접근성 원칙). 선·표시명·입력 초점을 효과가 가리지 않아야 한다. 데이터에서 도면 표시 범위를 유도하고 초기 위치 복원을 제공한다는 systems ACK를 인용하며, 게임플레이 이동 상한이나 새로운 카메라 동작·타이밍은 추가하지 않는다. 실제 화면 크기, 글자 겹침, 패드 초점, 확대/복원 가독성은 런타임 확인이 필요하다.

## 저장 확인의 시간·권위

[CARRIED] `animation/anim-list.md`의 `stamp_down`은 **300ms 고정**, 충격 및 `seal_confirm` 마커는 **+150ms**다. `vfx/vfx-budget.md`는 저장 성공 영수증 이후 같은 프레임에 도장 구간을 시작한다. 검토한 패킷은 0–150ms 대기, 150–300ms 정적 잉크 표시로 이 계약을 따른다. 다른 애니메이션의 길이를 확인 구간에 전용하지 않는다.

[TARGET · VFX 패킷 인용] 현재 확정 내용이 저장됐다는 사실을 절제된 잉크 표시로 전달한다는 의도에 ACK한다. `tCommitReceipt`의 성공뿐 아니라 현재 시도·명령·컨텍스트 일치 및 중복 소비 방지를 모두 확인해야 한다. 패널 이탈은 연출 토큰을 무효화하고 재생을 정리한다. 과거 명령 재생, 로드 복구, 오래되거나 중복된 영수증은 확인 연출을 재발행하지 않는다. `seal_confirm`이라는 효과 ID는 T0에 별도 seal 도구나 새 확정 명령을 추가하는 근거가 아니다.

| 항목 | 검토한 계약 |
|---|---|
| SavePending / 저장 실패 | 확인 VFX·사운드 미발행, 새 emitter·particle·VFX draw call 0 |
| 정상 표시 | 결과 패널 내부, 본문 뒤, 도구 선·조작 요소 밖에 정적 잉크 1장 |
| 개별 효과 예산 | particle 0, emitter 0, draw call 최대 1 |
| 화면 개입 | bloom 0, screen flash 0, default camera shake 0 |
| 저감 모션 / 낮은 품질 | 도장 변형·일시 잉크층 생략, 동일한 영수증 조건의 정적 성공 문구/체크 표시 |
| 사운드 | 감사·런타임 승격된 원본만 사용, 허용된 영수증당 최대 1 voice, 실패 0; 에셋이 없으면 무음 |

[OBSERVED] 패킷은 `runtimeEligible: false`이며 systems hook ACK와 에셋별 감사를 별도 조건으로 남긴다. 본 리뷰는 그 조건을 해제하지 않는다.

## 검증과 한계

[OBSERVED · 2026-09-10] 다음 문서 검증을 실행했다.

```text
python3 /Users/jangyoung/.agents/skills/game-vfx/scripts/validate_vfx_spec.py _workspace/current/vfx/vfx-spec.json
exit_code: 0
output: valid: particles=0 draw_calls=1
```

[OBSERVED] 현재 `presentation/`에는 `presentation-spec.md`, `camera-timing.md`, `ui-flow.md`가 없다. 따라서 기존 `presentation/media-direction.md`는 미디어 표현 범위로만 참고하고, 게임 내 동작 기준은 `synopsis/chapter-beats.md`, `concept/style-guide.md`, `animation/anim-list.md`, `vfx/vfx-budget.md` 및 기획 패킷을 대조했다. 없는 연출 문서를 검증한 것으로 기록하지 않는다. 이 신규 리뷰는 누락된 전 씬 연출 스펙을 대체하지 않는다.

- [OBSERVED] 담당 산출물은 이 파일 하나이며 frontmatter와 RFC 인용을 갖췄다. 같은 사이클 신규 리뷰이므로 대체·아카이브 대상은 없다.
- [OBSERVED] 작성 후 `freshness-check.sh`는 exit 0, `0 finding(s) across 136 markdown artifact(s)`였다(`/tmp/t0-presentation-freshness.log`). 이 검사는 frontmatter/supersedes 구조에 한정된다.
- [OBSERVED] 수치는 기존 계약 또는 신규 저작 패킷에서 인용했으며 성능·난이도·몰입 실측으로 사용하지 않았다.
- [OBSERVED] 본 파일은 연출 ACK만 제공한다. 다른 의존 레인 ACK와 디렉터의 전체 판정은 각 소유 기록에 남긴다.
- [OBSERVED] 코드 변경은 없으므로 코드 그래프 갱신은 수행하지 않았다. 전체 memory_sync는 디렉터 통합 작업에 속한다.
- [OBSERVED] 세션 시작에서 mex-agent 정체성 검증 실패로 mex check는 skipped였다. TeX 동명 바이너리는 실행하지 않았다.

[TARGET] 실제 성공/실패·늦은 콜백·중복 영수증·패널 이탈·일시정지·저감 모션·무음 경로를 실행해 확인하고, 150/300ms 타이밍과 가독성을 캡처로 검증해야 한다. 이번 문서 검사로 G4/G5 또는 사람 플레이테스트를 통과했다고 주장하지 않는다.
