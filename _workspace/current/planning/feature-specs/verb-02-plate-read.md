---
updated: 2026-09-10
cycle: 20260909-preproduction-c3
status: current
supersedes: null
owner: game-planner
---

# verb-02 · 판독 (Plate Read)

```yaml
verb_id: reader
name_ko: 판독
name_en: Plate Read
law: "법2 원본은 닳지만 사본은 남는다"
layer: sandbox + commit      # 연습=사본 재생 무제한 / 확정=가설판 인용 고정
introduced_at: t0-b3
unguided_reprise: c1-b2
required_beats: 11          # live campaign.json 재측정 (RFC-P3-008)
design_minutes_touched: 165  # 그 비트들의 분 합(중복 계상). 480의 분해가 아니다
goal: >
  기록을 "재생"이 아니라 "판독"으로 체험하게 만든다. 화면에 과거가 재생되지 않고
  곡선과 각인이 나오며, 그것을 읽어 의미로 바꾸는 일은 전부 플레이어 몫이다.
player_fantasy: >
  낡은 판을 판독대에 올리고 배율을 올려, 남들이 잡음이라 부른 자국에서 4분의 공백을 찾아내는 사람.
touched_lanes: [systems, presentation, vfx, worldview, qa]
```

> **R3a 필드명 정정 (2026-09-10 · R4 · C3-F35)** [OBSERVED]: 이전 판은 카운터를 `plateOriginalWear`(상한 3, `tunable: balance`)로 적었다. 그 이름은 economy가 **철회**한 제안이라 스키마에 없고(`economy/currency-map.md` §4.2 폐기 기록 · `negotiation-record.md` N-10), 노브 소유도 balance가 아니라 **economy**다(`systems/data-schemas/plates.md:39` `tunable: economy`). 저장 필드명을 스펙이 잘못 확정 인용하면 C4 핸드오프에서 마이그레이션 없는 개명이 된다(CLAUDE.md §9). 값(상한 3)과 비차단 모델은 바뀌지 않았다.
> **draft 인용 표기 (C3-F33)** [OBSERVED]: 아래 규칙표가 인용하는 `systems/interaction-rules.md`는 `status: draft`(cycle c5)다. 디렉터 판정으로 **정본 후보**이며 R4·R5 QA 검증 후 소유 레인이 `status: current`로 올린다(RFC-Q2). 그때까지 그 인용은 **(C4/C5 검증 대기)**로 읽는다.
> **`law:` 필드의 출처** [OBSERVED, RFC-P3-014]: 6법 호명 문구의 정본은 `worldview/worldview-bible.md` §3 표 하나뿐이며 위 값은 그 문구를 **문자 그대로 인용**한 것이다(2026-09-10 전건 대조, 6/6 일치). `worldview/consistency-audit.md` §4에 보존된 폐기 문구는 쓰지 않는다.
> **집계의 출처** [OBSERVED]: `required_beats`·`design_minutes_touched`는 live `planning/campaign.json`을 `node planning/validate-campaign.mjs`로 집계한 값이다(2026-09-10 R4 재실행 = sha256 `92301c0a…` · 121457 B · `zoneId` 추가분, 집계는 **불변**). 이후 인용은 고정 sha를 다시 적지 말고 **검증기 출력의 `sha256`을 쓴다**(RFC-Q1). 이전 판의 값은 아카이브 c3 계보였다(C3-F1 / RFC-P3-008).

## rules

| # | 규칙 | 근거 |
|---|---|---|
| R1 | 염판·당직일지·조위대장을 판독대에 올리고 시간 범위를 드래그, 배율은 휠/스틱 | `systems/interaction-rules.md` §2.2 **(C4/C5 검증 대기)** |
| R2 | **첫 판독 시 검증 사본이 자동 보존**된다. 토스트 + 증거함 항목 생성. 이후 재생은 전부 사본 | 법2, GDD §3.3 원칙 3 |
| R3 | **사본** 재생은 횟수 제한 0 · 비용 0이며 원본을 소모하지 않는다 | 법2 개정(C3), systems `plate-readout.md` 상태기계 |
| R3a | **원본**은 **직접 가하는 파괴적 절차**만 카운터를 올린다 — 누계 **`readCounts`**(`systems/data-schemas/save.md:54`, `map<recordId,int>`), 상한 **`readBudget` = 3**(`systems/data-schemas/plates.md:39`, **[tunable: economy]**). 사본 판독·재생·되돌림은 이 값을 건드리지 않는다 | `economy/currency-map.md` §4.2 **옵션 B(채택)**, RFC-P3-009 정합 |
| R3b | **카운터는 진행을 차단하지 않는다.** 상한 3에 닿아도 사본 경로로 모든 필수 확정이 가능하며, 바뀌는 것은 에필로그 기록 패널의 **보존 등급 문장 1줄**뿐이다. 잔량 0에서 판독을 막는 차단형 예산은 **거부**됐다(economy §4.2 옵션 C — 법2 위반·진행 불가 경로 생성) | economy §4.2, systems P-R3·P-R4 |
| R3c | 카운터는 **연습 층에서 오르지 않는다.** 연습은 표시만 한다 | systems P-R7 |
| R3d | UI 표기는 "**원본 상태**"다. "예산"이라는 단어는 **부식에만** 쓴다 | economy §4.2 (RFC-E3) |
| R4 | 분해능 **4분** 눈금이 항상 보인다. 4분보다 짧은 사건은 판독으로 분해되지 않는다 | `worldview/worldview-bible.md` §2 |
| R5 | 판독 자체는 확정이 아니다. 결과를 **가설판에 인용으로 고정**할 때만 출처(매체·계통·관측소)가 기록된다 | `systems/interaction-rules.md` §2.2 **(C4/C5 검증 대기)** |
| R6 | 염판 원본 · 그 표면 부식 · 복제 스캔은 **같은 출처 1개**로 센다 | `systems/interaction-rules.md` §3 **(C4/C5 검증 대기)** |
| R7 | 염판은 얼굴·음성·의도·사람 위치를 복원하지 않는다. 이 금지는 캐논이며 레트콘 불가 | 세계관 §2 |

## edge_cases

| # | 상황 | 요구 동작 |
|---|---|---|
| E1 | 플레이어가 같은 출처를 근거 슬롯 2개에 넣으려 한다 | 두 번째 슬롯 즉시 회색 + "독립 매체 2종 필요" |
| E2 | 4분 미만 간격의 두 사건을 구분하려 한다 | "분해능 4분" 라벨 + 판정 `indeterminate`. 진행은 막히지 않는다 |
| E3 | 자동 사본 생성 도중 저장이 끼어든다 | 사본 생성은 원자적. 실패 시 판독 자체를 롤백하고 원본을 건드리지 않는다 |
| E4 | 손상 세이브를 로드해 증거함이 비어 있다 | 복구 패널 3선택. 손상 파일 덮어쓰기 금지 |
| E5 | 플레이어가 자동 사본을 지우려 시도(정리·버리기) | 필수 단서 사본에는 삭제 조작 자체를 노출하지 않는다 |
| E6 | 인용을 잘못 고정했다 | 인용 해제는 무료·무제한. 확정 층이지만 되돌림 비용 0 (예외적으로 체크포인트를 만들지 않는다) |
| E7 | 스크린리더·저시력 사용자가 곡선을 못 읽는다 | 곡선의 수치 표(시각·값)를 텍스트로 병기 |

## acceptance_criteria

### D — 문서 단계에서 지금 측정 가능

| id | 기준 | 현재 값 | 판정 |
|---|---|---|---|
| D1 | 필수 비트 ≥ 6건 | 8건 [OBSERVED] | PASS |
| D2 | 도입/미안내 재문제 존재 | `t0-b3` / `c1-b2` [OBSERVED] | PASS |
| D3 | 33/33 비트가 매체 2종 이상 | **33/33** [OBSERVED — `node planning/validate-campaign.mjs` 검사 `C-06`, `aggregates.mediaKindsPerBeatMin = 2`] | PASS **(2026-09-10 수정 후)** |
| D4 | 영구 손실 문구 0건 — 모든 비트의 `recovery`가 무료 복구 명시 | 33/33 [OBSERVED] | PASS |
| D5 | 자동 사본 대상(`autoKeptClueIds`) 목록이 문서에 존재 | [OBSERVED 2026-09-10 재측정] `systems/data-schemas/`에 **6파일 존재**(`beats` `hints` `plates` `save` `tools` `zones`). 이전 판의 "`zones.md`만 존재"는 스테일 주장이었다(C3-F15). 목록 자체의 수록 여부는 systems `save.md` 내용 확인 필요 | **PASS(파일 존재)** / 내용 확인 → systems |
| D6 | 4분 분해능 위반 인과(4분 미만 간격에 의존하는 확정) 0건 | 규칙은 systems P-R5가 강제하나 **비트별 시각 간격 데이터가 `campaign.json`에 없어 검사 불가** [OBSERVED] | FIX → C4 synopsis |

> **D3의 이력** [OBSERVED]: 이전 판의 "33/33 PASS"는 `campaign.meta.md` §4의 **손으로 옮겨 적은 표**를 인용한 값이었고 실제로는 `c1-b4`가 log/log 1건 미달이었다(C3-F11). 2026-09-10에 `c1-b4`에 염판 단서 1건을 더해 해소했고(`campaign.meta.md` §10.1), 위 값은 **수정 후 기계 검사 결과**다. 검사 실패 시 검증기는 `exit=1`과 함께 미달 비트 id를 낸다.

### B — 빌드 후에만 측정 가능 (n=0)

| id | 기준 | 표본 |
|---|---|---|
| B1 | "사본이 보존됐다"를 인지 ≥ 10/12명 (토스트 직후 질문) | 12명/5유형 |
| B2 | 원본 손실 공포로 재생을 아끼는 행동 관측 0건 [TARGET] | 동일 |
| B3 | `c1-b2` 미안내 판독 성공률 ≥ 75% [TARGET, balance 승인 필요] | 동일 |
| B4 | 손상 세이브 복구 성공률 100% | 인위적 손상 파일 10종 |

## telemetry_fields

| 필드 | 타입 | 출처 | 이 스펙에서의 용도 |
|---|---|---|---|
| `read_count_total` | int | systems plate-readout | 원본 판독 총 횟수 |
| `read_budget_exhausted` | int | systems plate-readout | 예산 소진 건수 (B2와 직접 연결) |
| `auto_copy_created` | int | systems plate-readout | 자동 사본 생성 수 |
| `citation_count` | int | systems plate-readout | 가설판 인용 고정 수 |
| `indeterminate_shown` | int | systems plate-readout | 4분 미만 판정 회피 표시 |
| `alt_path_offered` | int | systems plate-readout | P-F1 대체 경로 안내 횟수 |
| `reader_time_min` | float | systems plate-readout | 체류 시간 |
| `hint_used` | (level, beat_id, t) | systems hint-system | B3 |
| `autocopy_toast_seen_ms` | int | **[신규 제안]** | B1 사본 보존 인지율 |

> **명명 규칙.** 아래 키는 `systems/system-specs/*.md` §6이 이미 정의한 이름을 그대로 쓴다. 기획이 새로 요구하는 키만 **[신규 제안]**으로 표시하며, 채택 여부는 systems가 판정한다. 공통 키(`beat_reached`, `hint_used`, `sandbox_time_min`, `commit_time_min`, `afk_gap_sec`)의 정의는 `systems/ops/telemetry-contract.md`가 소유한다 — [OBSERVED 2026-09-10 재측정] **이 파일은 존재한다**(`ls systems/ops/` → `telemetry-contract.md`, cycle c3 · status current). 이전 판의 "아직 없다"는 스테일 주장이었다(C3-F15).
수집 원칙: 연습(sandbox) 층 로그는 **분석 전용**이며 게임 내 판정·엔딩·업적에 쓰지 않는다(GDD §3.3 원칙 4).
