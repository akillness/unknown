---
updated: 2026-09-10
cycle: 20260909-preproduction-c3
status: current
supersedes: null
owner: game-balance-designer
---

# 시뮬레이션 결과 색인

## 0. 현재 상태 [OBSERVED]

| 항목 | 값 |
|---|---|
| 이 폴더의 결과 파일 수 | **0** |
| 입력 파일 (정본) | `_workspace/current/planning/campaign.json` — sha256 `fdabf1d421932d679429870a06eeb31e9527562370c6a107aec77c2c884fb2b7`, 120479 B [OBSERVED 2026-09-10, `shasum -a 256` · `wc -c`] |
| 폐기된 입력 해시 | `2bfe4d52…` — 저장소 전수 0건(C3-F2). 이 해시를 인용한 §2·§5 문장을 정정했다 |
| 실행한 시뮬레이션 | **0회** |
| 시뮬 하네스 존재 여부 | **없음** — `_workspace/current/systems/prototype/`·`tech-verification/`에 밸런스 시뮬 스크립트 미확인, 빌드 0건 |
| 따라서 | `balance/balance-sheet.md`의 모든 밴드는 **미검증**이다. 부식 안전 조건 `9 ≥ max(7,8)+1`·원본 카운터 상한 3·판정 창 4분·힌트 임계 180초는 계산·인용으로 유도한 [TARGET]이며 실행 결과가 아니다 |

시뮬 도구가 없으므로 본 문서의 계획은 계약 `## Honesty gates`에 따라 **[INFERENCE] 라벨**을 달고, 하네스 추가는 systems 레인 과제로 넘긴다(RFC-B3).

## 1. 결과 파일 규칙 (실행 후)

- 경로: `balance/sim-results/{YYYY-MM-DD}-{topic}.md`
- 필수 절: `방법` / `시드` / `명령(그대로 재현 가능한 한 줄)` / `입력 파일 해시` / `측정값 vs 밴드` / `판정(PASS·FIX·REDO)` / `증명하지 못한 것`
- 재튜닝은 **전/후 시뮬 1쌍**을 같은 시드로 동봉해야 한다(`patch-deltas.md` §2).
- 입력 해시가 바뀌면(예: planning이 비트의 `tools`/`clues`를 재저작) 기존 결과 문서는 **자동으로 만료**된다. 만료된 결과를 근거로 델타를 쓰지 않는다.

## 2. S1 — 33비트 자원 소모 시뮬 (최우선)

**RFC-P3-009 반영 재정의**: 이전 판의 S1은 "재생 예산 여유율 ≥1.50"과 "계통별 부식 잔여율 ≥20%"를 쟀다. 두 지표 모두 폐기된 모델의 것이다(`balance-sheet.md` §11). 소모·누적이 없으므로 **잴 것은 잔여율이 아니라 개방성과 비차단성**이다.

| 항목 | 내용 |
|---|---|
| 목적 | (a) 부식 상한 9 아래에서 법4의 두 보호 선택이 항상 열리는가 (b) 원본 상태 카운터가 어떤 경로에서도 진행을 막지 않는가 (c) 33비트 DAG에 도달 불가 비트가 없는가 |
| 입력 | `planning/campaign.json` sha256 `fdabf1d4…`(33비트 DAG·도구·단서), `systems/prototype/model.mjs`(`ROUTES`·`LIMITS`), `balance-sheet.md` §3·§4 값 |
| 모델 | 비트를 위상정렬로 소비. **확정은 자원을 차감하지 않는다.** `routing` 구성안마다 `cost = Σ partCost` 를 상한 9와 비교하고, 원본 직접 절차만 `readBudget` 카운터를 +1 |
| 프로파일(가정, 실측 아님) | `expert` 오확정률 0.05 / `median` 0.20 / `careless` 0.45 / `worst` 결정론적 최대 재시도 — 오확정은 **시간 비용만** 만든다 |
| 출력 | 스테이지별 개방 경로 수, 원본 카운터 상한 도달 비율, 차단 발생 지점, 결말 도달 3/3 여부 |
| 합격 조건 | (a) 모든 `routing` 비트에서 상한 이내 보호 선택 **≥ 2** (b) **모든** 프로파일에서 진행 차단 0건 (c) 세 결말 도달 가능 3/3 (d) 원본 카운터 상한 도달이 (a)(b)(c) 중 무엇도 바꾸지 않음 |

명령(스크립트 **미작성** — 실행 시 이 줄을 그대로 결과 문서에 기록):

```
python3 _workspace/current/balance/sim/route_openness_sim.py \
  --campaign _workspace/current/planning/campaign.json \
  --model _workspace/current/systems/prototype/model.mjs \
  --corrosion-limit 9 --read-budget 3 \
  --profile median --runs 10000 --seed 20260909 \
  --out _workspace/current/balance/sim-results/2026-09-10-route-openness.md
```

시드: `20260909`(기본), `20260910`, `424242` 3회 교차 확인. 시드마다 결과가 다르면 표본 수를 10배로 올린 뒤 재실행한다. `worst` 프로파일은 결정론이므로 시드 무관(`--profile worst --runs 1`).

## 3. S2 — 조위정합 판정 창 시뮬

| 항목 | 내용 |
|---|---|
| 목적 | 21슬롯·잔차 ≤4분 규칙이 (a) 추측으로 뚫리지 않고 (b) 정답 경로에서 항상 도달 가능한지 확인 |
| 모델 | 관측소 오프셋을 [-40, 40] 4분 격자에서 균등 추출, 두 자료의 위상 앵커 공유 여부를 켜고/끄고 비교 |
| 대조군 | 무작위 클릭 에이전트(슬롯 균등 선택) |
| 합격 조건 | 무작위 에이전트의 2자료 동시 통과율 ≤ 0.5%(이론값 1/441=0.23%), 앵커 공유 시 결정론 풀이 100%, 앵커 없는 구성 0건 |

```
python3 _workspace/current/balance/sim/alignment_window_sim.py \
  --resolution 4 --drift 40 --pass-residual 4 --sources 2 \
  --runs 200000 --seed 20260909 \
  --out _workspace/current/balance/sim-results/2026-09-10-alignment-window.md
```

## 4. S3 — 힌트 임계 민감도

| 항목 | 내용 |
|---|---|
| 목적 | **단일** 자동 제안 임계(180초)와 쿨다운(180초)이 비트 길이 분포(설계 4~22분 [OBSERVED])에서 조기 노출·과다 반복을 만드는지 확인 (RFC-P3-015) |
| 모델 | 비트별 소요시간을 `fastMinutes`~`deliberateMinutes` 삼각분포로 추출(측정 아님), 무진전 구간 비율을 0.1~0.5로 스윕. **단계 승격은 시뮬 대상이 아니다** — 플레이어 입력으로만 열린다 |
| 합격 조건 | 비트당 자동 제안 노출 ≤ 2회, 유효 조작 진행 중 제안 0건, 제안 수락률 밴드(≥5%)를 판정할 수 있는 표본 크기 확보 |

```
python3 _workspace/current/balance/sim/hint_threshold_sim.py \
  --campaign _workspace/current/planning/campaign.json \
  --offer-idle 180 --offer-cooldown 180 --idle-ratio-sweep 0.1:0.5:0.05 \
  --runs 5000 --seed 424242 \
  --out _workspace/current/balance/sim-results/2026-09-10-hint-thresholds.md
```

## 5. 실행 순서와 선행 조건

| 순서 | 시뮬 | 선행 조건 |
|---|---|---|
| 1 | S1 | `campaign.json` 고정(**[2026-09-10 재측정] sha256 `92301c0a…`, 121457 B, 47/47 PASS [OBSERVED `validate-campaign.mjs`]** — 직전 기재 `fdabf1d4…`/120479 B는 같은 사이클 이전 개정 값이고, 밸런스 입력 집계는 전건 불변이다 `balance-sheet.md` §7), `routes.json` 스키마 신설(RFC-B4 · `balance-sheet.md` §10 Q1), **C3-F4는 RFC-P3-009로 해소됨** |
| 2 | S2 | 관측소 오프셋 값이 세계관/시놉시스에서 확정될 것, **그리고 `model.mjs` ±45 와 캐논 ±40 불일치 해소**(`balance-sheet.md` §10 Q6) |
| 3 | S3 | S1 통과 후(개방성·비차단성이 흔들리면 힌트 임계 논의가 무의미) |

## 6. 시뮬이 증명하지 못하는 것 [OBSERVED]

- 실제 플레이타임·완주율·이탈 지점. `observedMedianMinutes`는 여전히 `null`이며 시뮬로 채울 수 없다.
- 체감 난도·재미·서사 설득력. 난이도 지수는 대리 지표다.
- Unity 런타임 성능, 저장/복구 안정성, 접근성.
- 판매·위시리스트·가격 수용성.

- 난이도 지수(§7)의 **체감 대응**. 지수는 대리 지표이며 시뮬로 검증되지 않는다.

시뮬 전부가 PASS해도 G2 외의 게이트는 오르지 않으며, G2조차 "밴드가 내부적으로 성립한다"까지만 말한다. 사람 플레이 표본 **n=0**은 그대로다. 현재는 그 이전 단계다 — **[2026-09-10 개정]** `balance-sheet.md` §7.1의 상승 폭 FIX(C5→C6 = +3)는 디렉터 판정 **C3-F31**(미측정 열 제외 → 재계산 +2)로 닫혔으나, 같은 절의 **하락 구간 임계가 미판정(RFC-B5)** 이라 문서 게이트는 여전히 전건 PASS가 아니다.
