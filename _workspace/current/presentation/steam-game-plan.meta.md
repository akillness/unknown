---
updated: 2026-09-10
cycle: 20260909-preproduction-c5
status: current
supersedes: null
owner: game-presentation-director
describes: _workspace/current/presentation/steam-game-plan.html
revision: r7 (R7d 최종 덱 재빌드 — QA 재검증 6 이후 대장을 s19 63칸에 재대조. 생성기 로직 변경 0건). r3·r4·r5·r6 절은 역사로 남긴다
---

# steam-game-plan.html 메타 (r7)

## 소유와 생성
- 생성기: `_workspace/current/presentation/generate-deck.mjs`.
- 개요: `_workspace/current/presentation/deck-outline.md`.
- HTML은 **생성물**이다. 내용을 바꾸려면 HTML이 아니라 생성기를 고치고 다시 빌드한다.
- 이 작업에서 손댄 파일은 넷뿐이다: `generate-deck.mjs`, `deck-outline.md`, `steam-game-plan.html`, 이 메타. 그리고 아티팩트 사본 1개. 다른 파일은 읽기만 했다. **`planning/campaign.json` 을 포함해 어떤 원본도 수정하지 않았다.**
- r7(2026-09-10 R7d)은 더 적다: **`steam-game-plan.html` 과 이 메타 둘뿐이다.** 생성기 `generate-deck.mjs` 는 sha 가 r6 과 같은 `3bf99037…` 로 **한 바이트도 바꾸지 않았다** `[OBSERVED]`. `production/cycle-ledger.json` 은 `scripts/regen-cycle-ledger.py` 로 재생성했고 실행 전후 sha 가 `819e563a…` 로 동일해 **이 루프는 대장도 바꾸지 않았다** `[OBSERVED]`. `qa/defect-register.md` 는 읽지도 쓰지도 않았다(스크립트가 읽는다).
- r6(2026-09-10 R7c)도 같다: `generate-deck.mjs`, `steam-game-plan.html`, `deck-outline.md`, 이 메타 넷뿐이다. `production/cycle-ledger.json` 과 `qa/defect-register.md` 는 **읽기만** 했다 — 대장은 `scripts/regen-cycle-ledger.py` 로 재생성했고 실행 전후 sha 가 `f0c901e6…` 로 같아 내용은 바뀌지 않았다 `[OBSERVED]`.
- r5(2026-09-10 C6)도 같다: `generate-deck.mjs`, `steam-game-plan.html`, `deck-outline.md`, 이 메타 넷뿐이다. `systems/unity-implementation.md`(11절 파생 원본·L69 정본 표현)와 `product/business-model.md`(라벨 상호 참조 대상)는 **읽기만** 했다.
- r4(2026-09-10 R5)도 같다: `generate-deck.mjs`, `deck-outline.md`, `steam-game-plan.html`, 이 메타 넷뿐이다. `planning/campaign.json`·`worldview/glossary.md`·`systems/unity-implementation.md` 는 **읽기만** 했다(파생 원본·대조 대상).

## 빌드 명령 `[관측]`
```
node _workspace/current/presentation/generate-deck.mjs \
  --out <절대경로>/steam-game-plan.html \
  [--out <다른 절대경로>.html]
```
- `import.meta.url` 기준으로 `_workspace/current`를 찾으므로 **어느 cwd에서 실행해도** 같은 결과가 나온다. 실제로 `/`, `/tmp`, 저장소 루트에서 각각 실행해 확인했다.
- 사용자 경로 하드코딩 없음. 출력 경로는 CLI 인자로만 받는다.
- 외부 패키지 0개. Node 내장 `fs`, `path`, `url`만 쓴다. 네트워크 접근 없음. 설치 0건.
- 실행 환경 Node `v26.8.1` `[관측] 2026-09-10 (node -v)`. 종료 코드 0, 자체 점검 error 0건, 경고 1건.
- r2 빌드는 Node `v26.5.0`에서 수행했다 `[CARRIED]`.

## r7에서 바뀐 것 `[OBSERVED] 2026-09-10` — R7d 최종 덱 재빌드 (생성기 무변경)

같은 사이클 안의 제자리 개정이다(`cycle` 불변 · `supersedes: null` 유지). **생성기 로직 변경 0건.** 이번 회차가 고친 것은 코드가 아니라 **렌더된 값이 대장보다 옛것이었다는 상태**다.

| # | 무엇이 틀렸나 | 어떻게 고쳤나 | 근거 |
|---|---|---|---|
| 1 | r6 빌드 뒤 **QA 재검증 6** 이 `qa/defect-register.md` 의 C5·C7 상태 칸을 닫았는데, 배포 경로의 html 은 여전히 r6 시점 값(C5 `11/9` · 열린 S2 1 · `fix-in-progress`, C7 `47/18` · 열린 S2 2 · `fix-in-progress`)을 싣고 있었다 | 대장을 먼저 재생성해 최신임을 확인한 뒤 덱을 다시 빌드했다. 현재 렌더 값은 **C5 `11/10` · 열린 S2 0 · `reviewed-and-revised`**, **C7 `47/20` · 열린 S2 0 · `reviewed-and-revised`**, **C6 는 열린 S2 1 유지**(`fix-in-progress`) | 아래 63칸 대조 · `production/cycle-ledger.json` |
| 2 | — (재발 없음) | 생성기는 열지 않았다. `cycleSlideBody()` 의 v2 소비부와 `schemaVersion !== 2` 게이트는 r6 것을 그대로 쓴다 | 생성기 sha `3bf99037…` 가 r6 과 동일 `[OBSERVED]` |

- **덱이 손으로 적은 회차 숫자는 여전히 0개.** s19 의 63칸 전부가 대장 파생이고, 유일한 파생 문장 `열린 S1 합계 0건` 도 대장 `open_S1` 의 단순 합이다 `[OBSERVED]`.
- 이 표가 말하지 않는 것은 r6 과 같다: 회차가 문서로 남았다는 사실뿐이며 **그 회차가 게임을 개선했는지는 여전히 미측정**이다(플레이 표본 n=0).

### r7 재현 명령 `[OBSERVED] 2026-09-10` — 대장 회차별 값 ↔ 렌더된 s19 셀 대조

```
cd <저장소 루트>
python3 scripts/regen-cycle-ledger.py     # stderr: parsed 157 rows, dropped 0: []
node _workspace/current/presentation/generate-deck.mjs \
  --out "$PWD/_workspace/current/presentation/steam-game-plan.html"

diff <(python3 -c "import json;[print('|'.join(str(c[k]) for k in ['id','total','closed','open_S1','open_S2','open_S3plus','open_rfc','status','review']))for c in json.load(open('_workspace/current/production/cycle-ledger.json'))['cycles']]") \
     <(grep -o '<tr><td>C[0-9]</td>\(<td>[^<]*</td>\)\{8\}</tr>' _workspace/current/presentation/steam-game-plan.html \
       | sed -e 's|</td><td>|\||g' -e 's|<tr><td>||' -e 's|</td></tr>||')
```

- 대장 재생성 stderr 원문: `parsed 157 rows, dropped 0: []` — **dropped 0** `[OBSERVED]`. 종료 코드 0.
- 대장은 실행 전후 sha 가 `819e563ac1b97d7e5254073c18ec8c0b52afd5a7dbb89f89ad06f0fa17fb7214` 로 **동일**했다 — QA 재검증 6 의 결과가 이미 대장에 반영되어 있었고 이 실행은 그것을 **확인**했을 뿐 바꾸지 않았다 `[OBSERVED]`.
- diff **종료 코드 0 · 출력 0줄**. 7회차 × 9칸 = **63칸 전부 일치** `[OBSERVED] 2026-09-10`.
- 각주 대조: `grep -o 'cycle-ledger.json · generated_at [^<]*' steam-game-plan.html` → `generated_at 2026-09-10 · source qa/defect-register.md (single source, RFC-C6-002) · generator scripts/regen-cycle-ledger.py` `[OBSERVED]`.
- v1 필드 잔존 확인: `grep -c 'c\.findings\|c\.fixed\|c\.remaining' generate-deck.mjs` → **0** `[OBSERVED]`.
- 대조된 렌더 값(대장과 동일, 63칸):

| 회차 | 총 | closed | 열린 S1 | 열린 S2 | 열린 S3+ | open-rfc | 상태 | 검토 파일 |
|---|---|---|---|---|---|---|---|---|
| C1 | 0 | 0 | 0 | 0 | 0 | 0 | reviewed(parent session, ids not in register) | qa/c1-review.md |
| C2 | 0 | 0 | 0 | 0 | 0 | 0 | reviewed(parent session, ids not in register) | qa/c2-review.md |
| C3 | 36 | 33 | 0 | 0 | 1 | 2 | reviewed-and-revised | qa/c3-review.md |
| C4 | 22 | 18 | 0 | 0 | 4 | 0 | reviewed-and-revised | qa/c4-review.md |
| C5 | 11 | **10** | 0 | **0** | 1 | 0 | **reviewed-and-revised** | qa/c5-review.md |
| C6 | 41 | 17 | 0 | **1** | 21 | 2 | fix-in-progress | qa/c6-review.md |
| C7 | 47 | **20** | 0 | **0** | 26 | 1 | **reviewed-and-revised** | qa/c6-review.md |

- 굵은 6칸이 r6 표 대비 바뀐 전부다. 나머지 57칸은 r6 과 같다 `[OBSERVED]`.
- 빌드 결정성: 같은 입력으로 두 번째 빌드(scratchpad 사본) sha 가 `73b4c73f…` 로 본 경로와 동일 `[OBSERVED]`.

## r6에서 바뀐 것 `[OBSERVED] 2026-09-10` — R7c 수정 루프 (C7-F47 · C5-F2 잔여)

같은 사이클 안의 제자리 개정이다(`cycle` 불변 · `supersedes: null` 유지). 담당 결함 **C7-F47 (S2, presentation)** 과 **C5-F2 (S2) 의 잔여 1건**(덱 s19 재빌드).

| # | 무엇이 틀렸나 | 어떻게 고쳤나 | 근거 |
|---|---|---|---|
| 1 | `production/cycle-ledger.json` 은 `schemaVersion 2`(`total`·`closed`·`open_S1`·`open_S2`·`open_S3plus`·`open_rfc`)인데 생성기 `cycleSlideBody()` 는 v1 필드 `c.findings`·`c.fixed`·`c.remaining` 을 읽었다. 한 파일에 두 계약이 걸려 있었고 **어느 쪽도 `schemaVersion` 을 검사하지 않았다**. 그대로 재빌드하면 s19 의 21칸이 전부 `미기록` 이 된다 | 대장 소비부를 v2 로 교체했다. 표를 「회차 \| 총 \| closed \| 열린 S1 \| 열린 S2 \| 열린 S3+ \| open-rfc \| 상태 \| 검토 파일」 **9열**로 재구성하고 9칸 전부 대장에서만 읽는다. 숫자 칸은 `typeof v === 'number' && Number.isFinite(v)` 일 때만 렌더하고 아니면 `미기록` | `qa/defect-register.md` C7-F47 · `production/cycle-ledger.json` |
| 2 | 스키마 검사가 없어 계약이 갈라져도 조용히 틀린 값을 실었다 | **`schemaVersion !== 2` 면 표를 비우고 값 렌더를 중단한다.** 이전 빌드 값을 되살리지 않고 재생성 명령을 슬라이드 본문에 적는다. 파일 부재·빈 `cycles[]` 도 같은 처리 | 같음 |
| 3 | 렌더된 s19 가 **옛 손기입 값**(C3 36/27 · C4 22/9 · C5 11/6 · `reviewed-and-revised` 5건)을 싣고 있었고 html 이 대장보다 옛것이었다 | 대장을 먼저 재생성해 최신임을 확인(`dropped 0`)한 뒤 덱을 다시 빌드했다. 잔여를 닫는 근거는 mtime 순서가 아니라 **대장 63칸 ↔ 렌더 63칸 완전 일치**다(아래 재현 명령, 차이 0줄) | `qa/defect-register.md` C5-F2 |
| 4 | 슬라이드에 대장의 생성 시각·출처가 없어 표가 언제 무엇에서 나왔는지 알 수 없었다 | 각주(`notes.detail`·`notes.source`)에 대장의 `generated_at`·`source`·`generator` 를 그대로 넣는다 | 렌더 결과 |
| 5 | 대장 부재 분기가 **손으로 적은 5회차 계획표**(C3~C7)를 렌더했다. 대장에는 7회차가 있어 이 분기 자체가 드리프트 원인이었다 | 부재 분기의 회차 행을 전부 삭제하고 빈 표 + 「회차를 세지 않는다」 문장만 남겼다 | 같음 |

- **덱이 손으로 적은 회차 숫자 0개.** 생성기 소스에 s19 용 회차 상수는 남지 않았다. 유일한 파생값은 본문의 `열린 S1 합계` 이며 대장 `open_S1` 의 단순 합이다.
- 대장 파일은 **읽기만** 했다. `scripts/regen-cycle-ledger.py` 는 실행 전후 파일 sha 가 `f0c901e6…` 로 동일 — 이미 최신이었고 이 루프는 대장을 바꾸지 않았다 `[OBSERVED]`.
- 이 표가 말하지 않는 것: 회차가 문서로 남았다는 사실뿐이고, 그 회차가 **게임을 개선했는지는 여전히 미측정**이다(플레이 표본 n=0).

### r6 재현 명령 `[OBSERVED] 2026-09-10` — 대장 회차별 값 ↔ 렌더된 s19 셀 대조

```
cd <저장소 루트>
python3 scripts/regen-cycle-ledger.py     # stderr: parsed 157 rows, dropped 0: []
node _workspace/current/presentation/generate-deck.mjs \
  --out "$PWD/_workspace/current/presentation/steam-game-plan.html"

diff <(python3 -c "import json;[print('|'.join(str(c[k]) for k in ['id','total','closed','open_S1','open_S2','open_S3plus','open_rfc','status','review']))for c in json.load(open('_workspace/current/production/cycle-ledger.json'))['cycles']]") \
     <(grep -o '<tr><td>C[0-9]</td>\(<td>[^<]*</td>\)\{8\}</tr>' _workspace/current/presentation/steam-game-plan.html \
       | sed -e 's|</td><td>|\||g' -e 's|<tr><td>||' -e 's|</td></tr>||')
```

- 결과 **차이 0줄**. 7회차 × 9칸 = **63칸 전부 일치** `[OBSERVED] 2026-09-10`.
- 각주 대조: `grep -o 'production/cycle-ledger.json · generated_at [^<]*' steam-game-plan.html` → `generated_at 2026-09-10 · source qa/defect-register.md (single source, RFC-C6-002) · generator scripts/regen-cycle-ledger.py` `[OBSERVED]`.
- v1 필드 잔존 확인: `grep -n "c\.findings\|c\.fixed\|c\.remaining" generate-deck.mjs` → **0건** `[OBSERVED]`.
- 대조된 렌더 값(대장과 동일):

| 회차 | 총 | closed | 열린 S1 | 열린 S2 | 열린 S3+ | open-rfc | 상태 | 검토 파일 |
|---|---|---|---|---|---|---|---|---|
| C1 | 0 | 0 | 0 | 0 | 0 | 0 | reviewed(parent session, ids not in register) | qa/c1-review.md |
| C2 | 0 | 0 | 0 | 0 | 0 | 0 | reviewed(parent session, ids not in register) | qa/c2-review.md |
| C3 | 36 | 33 | 0 | 0 | 1 | 2 | reviewed-and-revised | qa/c3-review.md |
| C4 | 22 | 18 | 0 | 0 | 4 | 0 | reviewed-and-revised | qa/c4-review.md |
| C5 | 11 | 9 | 0 | 1 | 1 | 0 | fix-in-progress | qa/c5-review.md |
| C6 | 41 | 17 | 0 | 1 | 21 | 2 | fix-in-progress | qa/c6-review.md |
| C7 | 47 | 18 | 0 | 2 | 26 | 1 | fix-in-progress | qa/c6-review.md |

- 빌드 결정성 재확인: 같은 실행에서 `--out` 2개(본 경로 + scratchpad 사본) sha 동일 `f2028d3b…` `[OBSERVED]`.

## r4에서 바뀐 것 `[OBSERVED] 2026-09-10` — R5 수정 루프 (C5-F1)

같은 사이클 안의 제자리 개정이다(RFC-Q2, `cycle` 불변 · `supersedes: null` 유지). 담당 결함 **C5-F1 (S2)**.

| # | 슬라이드 | 무엇이 틀렸나 | 어떻게 고쳤나 | 근거 |
|---|---|---|---|---|
| 1 | 26 생산 | 덱이 **같은 파일 안에서 두 개의 T0 를 주장**했다. 26번은 손으로 적은 `허브와 제3수문 / reader·alignment·seal / 제외 3구역`(상수 `SLICE_ZONE_COUNT = 2` 파생)이었다. 26번은 T0 착수 승인을 요청하는 생산 슬라이드다 | 상수를 삭제하고 전부 live 데이터에서 파생: 구역 `stages[0].zoneIds`, 도구 `stages[0]` 비트 도구 합집합, 결론 `proofRequired` 비트 수, 힌트 `hints` 최대 단계, 제외 `allZones.length - zoneIds.length` | `qa/defect-register.md` C5-F1, `systems/unity-implementation.md` 10절, live `planning/campaign.json` |
| 2 | 11 구조 | 구역 이름 `허브`가 손으로 적혀 있었다. 값은 맞았지만 데이터가 바뀌면 조용히 틀린다 | 26번과 같은 `sliceZoneText` 를 쓴다. 두 슬라이드가 한 원본을 공유하므로 다시 갈라질 수 없다 | 같음 |
| 3 | 11·26 | 구역의 한국어 표시명을 덱이 스스로 정했다 | `worldview/glossary.md` **6-1절 `zoneId` 대응표**를 빌드 시점에 파싱한다(6법 표와 같은 방식). live `zoneId` 중 대응이 없으면 빌드 실패 | `worldview/glossary.md` 6-1 |
| 4 | 자체 점검 | 하드코딩이 되살아나도 빌드가 통과했다 | (a) 생성기 소스 금지 문자열 3종 추가, (b) 11·26 파생 문장 렌더 확인, (c) **`unity-implementation.md` 10절 T0 구역·도구·분 대조**, (d) `zoneId` 표시명 연결 확인. 빌드 리포트에 `sliceT0` 필드 추가 | 아래 자체 점검 절 |

### r4 재측정 명령과 결과 `[OBSERVED] 2026-09-10`
```
node -v
node _workspace/current/planning/validate-campaign.mjs
shasum -a 256 _workspace/current/planning/campaign.json
node _workspace/current/presentation/generate-deck.mjs \
  --out _workspace/current/presentation/steam-game-plan.html --out <scratchpad>/new.html
shasum -a 256 _workspace/current/presentation/steam-game-plan.html <scratchpad>/new.html
wc -c < _workspace/current/presentation/steam-game-plan.html
shasum -a 256 _workspace/current/presentation/generate-deck.mjs
```

| 항목 | 값 |
|---|---|
| Node | `v26.8.1` |
| `planning/campaign.json` sha256 | `92301c0a5ecfc7e142646f5f17c29c21609e2b2de7c90408de7bffc9d815ae23` · 121,457 bytes (검증기 출력에서 읽음) |
| 검증기 판정 | `checks 47 / pass 47 / fail 0` |
| 빌드 리포트 `sliceT0` | `{ id: "T0", minutes: 25, zoneIds: ["hub"], tools: ["circuit","reader"], proofBeats: 1, hintTiers: 3, excludedZones: 4 }` |
| 렌더된 11번 문장 | `T0 마지막 당직 인수, 25분, 당직실에서만 진행한다.` |
| 렌더된 26번 문장 | `포함은 당직실, 도구 circuit 와 reader, 결론 1건, 힌트 3단계.` / `제외는 나머지 4구역과 나머지 도구의 전체 깊이, 엔딩 본편 분량, 480분 콘텐츠.` |
| 정본 대조 | `systems/unity-implementation.md` 10절 = `hub` 1개 · `circuit`+`reader` 2개 · 25분 → 빌드 게이트 PASS |
| 빌드 종료 코드 | 0, error 0건, 경고 1건(외부 링크 16장, 의도된 상태) |

### r4 게이트 음성 시험 `[OBSERVED] 2026-09-10` (사본으로 수행 · live 파일 불변)
scratchpad 에 `_workspace/current` 의 원본 폴더를 심볼릭 링크한 미러를 만들고 생성기 **사본**을 실행했다. live 산출물은 건드리지 않았다.

| 시험 | 주입한 변형 | 결과 |
|---|---|---|
| N1 하드코딩 복귀 | 26번 줄을 옛 문장(`허브와 제3수문 …`)으로 되돌림 | **종료 코드 1** · error 3건: `생성기에 하드코딩된 단계 값: 허브와 제3수문` / `… reader 와 alignment 와 seal` / `26번 T0 포함 범위 문장이 파생값으로 렌더되지 않았다` |
| N2 정본 드리프트 | 미러의 `unity-implementation.md` 10절을 `hub`+`gate` / `reader`+`seal` / 30분 으로 변형 | **종료 코드 1** · error 3건: 구역·도구·분 각각 `정본 10절과 다르다` |

두 시험 모두 **양성 대조**(수정본 그대로 빌드 = 종료 코드 0)와 짝을 이룬다. 게이트가 실제로 막는다는 뜻이며, 시험 자체는 게임 플레이를 증명하지 않는다.

## r3에서 바뀐 것 `[관측] 2026-09-10` — C3 종료 수정 루프

담당 결함 **C3-F24**(RFC-P3-014). 같은 빌드에서 발견한 대외 노출 오류 3건을 함께 고쳤다. 모든 수정은 HTML이 아니라 `generate-deck.mjs`에서 했고 덱은 재생성했다.

| # | 슬라이드 | 무엇이 틀렸나 | 어떻게 고쳤나 | 근거 |
|---|---|---|---|---|
| 1 | 8 세계 | `4 용량 / 이번 조수에 보호 용량이 부족하다` 는 **기각된 재작성문**인데 출처를 바이블로 표기했다 (C3-F24) | 6법 표를 **빌드 시점에 `worldview/worldview-bible.md` 3절에서 파싱해 그대로 렌더**한다. 덱 소스에 법 문구를 적어두지 않는다. 파싱이 6행이 아니면 빌드 실패 | RFC-P3-014, `qa/defect-register.md` C3-F24 |
| 2 | 18 검증 | 수용 조건을 `중앙값 420분 이상 / 하위 25% 360분 이상 / 상위 25% 600분 이하` 로 적었다. 세션 P가 기각하고 디렉터가 폐기한 값 | 판정 키 `total_minus_afk_min`, 목표 밴드 중앙값 450~540분, 420·360은 **철회 트리거**, 상위 25% 조건 삭제. 빠른 322분·신중한 673분은 시나리오 경계로만 표기 | RFC-P3-011, 계약 `## Time acceptance` |
| 3 | 12 시스템 | `확정만 길게 누름 0.4초` 로 홀드를 기본값처럼 적었다 | 기본값은 2단계 확정, 홀드 0.4초는 접근성 설정 opt-in. 부식예산은 전역 상한이며 확정으로 소모되지 않는다는 문장 추가 | RFC-P3-015 (F10), RFC-P3-009 |
| 4 | 15 UI | `data_bindings 9`, `검증 매트릭스 10행` 이 계약 파일과 달랐다 (실제 10 / 17) | `systems/game-ui-contract.json` 에서 빌드 시점에 계산한다. 파일이 없으면 숫자를 지어내지 않고 `미측정` 으로 렌더 | `systems/game-ui-contract.json` 재측정 |
| 5 | 12·18 | 도구·시간 집계를 문장으로만 말했다 | 도구별 등장 비트(`circuit 10, reader 11, alignment 8, routing 3, corrosion 3, seal 7`), 도구 없는 비트 5개(`t0-b1, c1-b4, c3-b4, c7-b1, e0-b1`), 빠른/신중한 합을 live `campaign.json` 에서 계산 | `node planning/validate-campaign.mjs` PASS 44/44 |
| 6 | 자체 점검 | 정본 문구가 되살아나도 빌드가 통과했다 | 6법 정본 문구·기각 문구·시간 수용 문구·확정 방식 게이트를 추가. 계약 본문과 시간 상수 대조까지 빌드에서 검사 | 아래 자체 점검 절 |

### r3 재측정 명령과 결과 `[관측] 2026-09-10`
```
node /Users/jangyoung/orca/unknown/_workspace/current/planning/validate-campaign.mjs
shasum -a 256 _workspace/current/planning/campaign.json
node _workspace/current/presentation/generate-deck.mjs --out <절대경로>/steam-game-plan.html
shasum -a 256 _workspace/current/presentation/steam-game-plan.html
```

| 항목 | 값 |
|---|---|
| `planning/campaign.json` sha256 | `fdabf1d421932d679429870a06eeb31e9527562370c6a107aec77c2c884fb2b7` · 120,479 bytes |
| 검증기 판정 | `checks 44 / pass 44 / fail 0 / PASS` |
| 장별 분 | `25 / 50 / 55 / 65 / 65 / 70 / 75 / 65 / 10` 합계 480 |
| 비트 · 단서 | 33 · 73 |
| 빠른 / 신중한 합 | 322 / 673 (시나리오 경계, 표본 통계 아님) |
| 도구별 등장 비트 | circuit 10 · reader 11 · alignment 8 · routing 3 · corrosion 3 · seal 7 |
| UI 계약 | screens 19 · player_decisions 7 · data_bindings 10 · verification.matrix 17행 |

계보 주의 `[OBSERVED]`: C3 지시문이 인용한 sha `775a984c…` 는 이 세션 시점의 live 파일과 일치하지 않는다. planner 가 C3 수정 루프에서 같은 계보(계보 B) 위에서 파일을 고쳤고, 위 `fdabf1d4…` 가 2026-09-10 실측값이다. 장별 분 배분은 지시문과 동일하다.

## r2에서 바뀐 것
| # | 변경 | 이유 |
|---|---|---|
| 1 | **표지 재구성** | 면책 5줄이 표지를 약하게 만들었다. 제목 + 한 문장 + 수치 3개 + 한 줄 캡션만 남겼다 |
| 2 | **면책 전문 이동** | 표지 노트와 2번 상태 슬라이드로 옮겼다. 표지 본문 불릿 0개 |
| 3 | **단계 예산 완전 동적화** | 고정 문자열 `30분` / `450분` / `아홉 장` / `나머지 3구역` / `두 가지만` 제거 |
| 4 | **등록비 회계 서술 정정** | `고정비가 아니다`는 잘못된 회계 주장이라 삭제 |
| 5 | **노트 창 추가** | `window.open` 기반 별도 발표자 노트 창 |
| 6 | **할인 상한 계산화** | `39%`를 하한 9,000원과 표시가에서 계산 |
| 7 | **기간 계산화** | `17.9개월`을 `totalDays / 20` 에서 계산 |
| 8 | **자체 점검 확장** | 위 항목들이 되돌아오면 빌드가 실패하도록 게이트 추가 |

### 1. 표지 (슬라이드 1)
- 제목 `조수기록국: 마지막 당직` (h1)
- 한 문장 `기록을 복원하고, 항구의 물길을 바꾸는 밤.`
- 수치 3개 `480분 / 설계 분량`, `본편 완결 / 결말 3종을 본편에서 닫는다`, `Unity / 사전제작 단계`
- 캡션 한 줄 `가제 · 사람 플레이 0 · 가격 미승인`
- 본문 불릿 **0개** `[관측]`. 조작 안내는 표지에도 어디에도 없다.
- 480은 `campaign.json` 합계에서 계산되므로 예산이 바뀌면 표지 수치도 따라 바뀐다.
- 면책 전문(승인·계약·공개·결제 근거 아님, 빌드 0, 플레이 n=0, 등록 0, 가격 미승인, 배분율 가정, 회계·세무 자문 아님, 가제 사용 제한)은 표지 노트에 그대로 있다 `[관측]` 527자.

### 3. 단계 예산 동적화 `[관측]`
먼저 **원본을 건드리지 않고** 세션 tmp에 최소 미러(`product`, `production`, `planning`, `presentation` 4개 파일)를 만들어 개정 예산 `[25, 50, 55, 65, 65, 70, 75, 65, 10]` 으로 빌드해 검증했다.

그 뒤 **2026-09-09 00:23에 실제 `planning/campaign.json` 이 같은 값으로 개정되었고**(병행 세션 작업), 재빌드만으로 덱이 그대로 따라갔다. 생성기 수정은 필요 없었다. 현재 덱의 장 표는 `25 / 50 / 55 / 65 / 65 / 70 / 75 / 65 / 10`, 합계 480분, 비트 33개다.

| 확인 항목 | 결과 |
|---|---|
| 10번 장 표 | `25 / 50 / 55 / 65 / 65 / 70 / 75 / 65 / 10` 으로 갱신 |
| 11번 튜토리얼 본문 | T0 분이 25로 갱신, 도구 개수도 데이터에서 계산 |
| 11번 노트 | `이 25분이 실제로 열리고 측정되어야 나머지 455분` 으로 자동 갱신 |
| 26번 슬라이스 | `설계상 25분 구간이다` 로 자동 갱신 |
| 잔존 고정값 | `450분`, `30분이 실제로` 검색 결과 **0건** |
| 슬라이드 수 / 6줄 규약 | 36장, 최대 6줄 유지, error 0건 |

빌드마다 다시 계산하는 값: 장 수, 장별 분, 합계 분, 잔여 분, 비트 수, 종류별 분, 구역 수, 슬라이스 제외 구역 수, 도구 종수, T0 도구 목록과 개수, 플레이테스트 n.

### 4. 등록비 회계 서술 정정
삭제한 문장(모두 잘못된 회계 주장이라 빌드 게이트로 재발을 막는다):
- `100달러는 비용이 아니라 조건부 회수 항목이다`
- `다른 고정비와 섞지 않고`
- `이 항목을 고정비로 처리하면 손익분기가 왜곡되고`

대체한 서술(29번 본문, 22번과 23번과 29번 노트에 동일 취지로 반영):
- 등록 시 **반드시 선지출하는 현금 비용**이다. 앱 하나당 100달러이고 환불되지 않는다.
- **AGR 조건을 충족해 회수되면 그때 차감된다.** 조건은 조정총매출 1,000달러 달성이다.
- 현재 **원화 표에는 환율이 설정되어 있지 않아** 이 100달러가 들어 있지 않다.
- 따라서 원화 계산 밖에서 **100달러와 적용 세금, 은행 비용을 별도로 더한다.**
- 비용을 포함하면 손익분기가 왜곡된다는 주장은 하지 않는다.
- **이 덱은 회계 자문도 세무 자문도 아니다.** 해당 문구가 7곳에 있다 `[관측]`.
- 배분율은 여전히 `가정이며 Steam이 공개한 사실이 아니다`로만 표기한다. 70 대 30을 사실로 쓰지 않는다.

### 5. 발표자 노트 창
- 조작 바 버튼 8개: `이전 | 다음 | 목차 | 노트 | 노트 창 | 전체화면 | 자동 넘김 꺼짐 | 인쇄` `[관측]`
- `노트` 는 기존 다이얼로그, `노트 창` 은 `window.open("","deckNotesWin","popup=yes,width=560,height=780")` 으로 여는 **별도 창**이다. 슬라이드를 넘기면 창 내용도 따라 갱신되고, 창을 닫으면 갱신을 멈춘다.
- 팝업이 차단되면 버튼 라벨이 `노트 창 차단됨`으로 바뀐다.
- **전체화면 상태에서는 이 버튼을 비활성으로 만든다**(`fullscreenchange` 처리). 전체화면과 팝업 창을 함께 쓰는 동작은 **검증하지 않았고 동작한다고 주장하지 않는다.**
- 자동화 환경에서는 사용자 제스처가 없어 `window.open` 결과를 확인할 수 없었다. **팝업 창이 실제로 뜨는지는 사람이 눌러 확인해야 한다.**

## 산출물 `[OBSERVED] 2026-09-10` (r7 재측정)
| 항목 | 값 | 측정 명령 |
|---|---|---|
| 슬라이드 수 | 36 (`<section class="slide` 36, `data-goto=` 36) | `grep -o '<section class="slide' … \| wc -l` |
| 파일 크기 | **101,263 bytes** | `wc -c < steam-game-plan.html` |
| sha256 | **`73b4c73fefa61e3904a3683ea57e912e8d7acf75eeebe9fddea077cac06b8eb6`** | `shasum -a 256` |
| 빌드 시각 | 2026-09-10 **11:26:12 KST** (파일 mtime). 대장 재생성은 같은 재현 명령 안에서 8초 앞선 **11:26:04**. **mtime 순서는 신선도 판정 기준이 아니다**(재현할 때마다 갱신된다). 판정은 위 r7 절의 63칸 내용 일치로 한다 | `stat -f '%Sm' -t '%Y-%m-%d %H:%M:%S'` |
| 빌드 결정성 | 같은 입력으로 2회 빌드 시 sha 동일 확인 — 본 경로와 scratchpad 사본 모두 `73b4c73f…` (별개 실행 2회) | `shasum -a 256` 2개 비교 |
| 생성기 크기 / sha256 | 110,835 bytes / `3bf990376e70dc331fa4b58e7d93935982f7db5f1852bf7caaa494010666461f` — **r6 과 동일. 이번 회차는 생성기를 열지 않았다** | `wc -c`, `shasum -a 256` |
| 인라인 SVG | 4개, 전부 직접 작성, 외부 자산 0건 | `grep -o '<svg' \| wc -l` |
| 표 | 7개 | `grep -o '<table' \| wc -l` |
| 본문 최대 줄 수 | 6줄 (빌드 리포트 `maxBodyLines`) | 빌드 리포트 |
| 외부 공식 출처 링크 | 16개 슬라이드 (경고 1건은 의도된 상태) | 빌드 리포트 |
| 노트 | 36장 전부 세부·불확실성·출처 3항목 | 빌드 리포트 |
| 아티팩트 사본 | **이번 회차에도 만들지 않았다.** 배포 대상은 위 경로 1개이며, scratchpad 사본은 결정성 확인용으로 만들었다 배포물이 아니다 | - |

r6 값 `[CARRIED · 2026-09-10]`: 101,252 bytes · `f2028d3b…` · 생성기 110,835 bytes / `3bf99037…`. **r6 → r7 은 11 bytes 늘었다 `[OBSERVED]`** — 생성기가 동일하므로 차이는 전부 대장 값이다. 산술도 정확히 맞는다: C5 closed `9`→`10` (+1자), C5·C7 상태 `fix-in-progress`(15자)→`reviewed-and-revised`(20자) 2곳 (+10자), 나머지 바뀐 칸은 한 자리 수끼리라 0 `[INFERENCE — 산술 분해]`.
r5 값 `[CARRIED · 2026-09-10]`: 100,806 bytes · `585f4a04…` · 생성기 109,579 bytes / `3b2ef9e2…`. **r5 → r6 은 446 bytes 늘었다** — s19 표가 6열에서 9열로 늘고(7행 × 3칸) 각주에 대장 스탬프 한 줄이 더해진 차이다 [OBSERVED].
r4 값 `[CARRIED · 2026-09-10]`: 100,041 bytes · `0468eab2…` · 생성기 107,237 bytes / `cc88f17f…`. **r4 → r5 는 765 bytes 늘었다** — 17번 노트의 정본 표현 치환(길어짐)과 23번 기준 라벨 2줄·표 머리 라벨이 더해진 차이다 [OBSERVED].
r3 값 `[CARRIED · 2026-09-10]`: 100,060 bytes · `67c6592a…` · 생성기 102,298 bytes · `41ce2545…`. r2 값 `[CARRIED · 2026-09-09]`: 97,780 bytes · `fb1d7277…` · 생성기 96,200 bytes · `b052f9f5…`. 이 값들은 역사이며 현재 파일과 비교 대조용으로만 남긴다. **r3 → r4 는 19 bytes 줄었다** — 26번 한 줄이 손으로 적은 긴 문장에서 파생 문장으로 바뀐 차이다(구역 1개·도구 2종).

## 자체 점검 `[OBSERVED]` (error 시 종료 코드 1)
r5에서 추가된 게이트 (C5-F5 재발 방지, 음성 시험 N1·N2 로 확인함):
- **폐기 용어 게이트**: 렌더된 HTML에 `매체`+`경로` 연결 문자열이 있으면 실패. 정본 표현은 `systems/unity-implementation.md` L69 의 **`sourceType` 상이 AND `originId` 상이**다. 검사 문자열은 조각으로 조립해 게이트 자신이 렌더 대상이 되지 않게 한다. 음성 시험 N1(17번 노트에 용어 재삽입) → `폐기 용어 발견: 세 번째 용어(매체+경로)` 로 종료 [OBSERVED].
- **인수 테스트 수 파생 확인**: 17번의 `기술 인수 테스트 <N>개` 가 11절 파싱값으로 렌더되지 않으면 실패하고, 생성기 소스에 `인수 테스트 14개` 문자열이 있으면 하드코딩 게이트가 함께 실패한다. 음성 시험 N2 → 오류 2건으로 종료 [OBSERVED]. 현재 파싱값 **27**(`unity-implementation.md` 11절 T-01~T-27, `R1`·`R2`·`R3` 접두 행 포함).

r4에서 추가된 게이트 (C5-F1 재발 방지, 음성 시험 N1·N2 로 확인함):
- **T0 범위 하드코딩 금지**: 생성기 소스에 `허브와 제3수문`, `reader 와 alignment 와 seal`, `SLICE_ZONE_COUNT` 문자열이 있으면 실패. 검사 문자열은 조각으로 조립해 자기 자신을 오탐하지 않는다.
- **파생 렌더 확인**: 11번의 `<구역>에서만 진행한다`, 26번의 `포함은 <구역>` 이 파생값으로 렌더되지 않으면 실패. 상수로 되돌리면 여기서도 걸린다.
- **정본 대조**: `systems/unity-implementation.md` 10절의 `T0 범위` 행에서 구역·도구 토큰과 분을 파싱해 live `campaign.json stages[0]` 과 대조한다. 셋 중 하나라도 다르면 실패하고, 어느 쪽 값인지 함께 출력한다. 10절 행을 못 찾으면 숫자를 지어내지 않고 경고로 남긴다.
- **구역 표시명 연결**: live `zoneId` 5종 전부가 `worldview/glossary.md` 6-1절 대응표에 있어야 한다. 없으면 실패.
- 빌드 리포트에 `sliceT0` 객체(구역·도구·분·결론·힌트·제외 구역 수)를 찍어 QA 가 명령 출력만으로 대조할 수 있게 했다.

r3에서 추가된 게이트 (정본 문구가 되살아나면 빌드가 실패한다):
- **6법 정본**: `worldview-bible.md` 3절에서 읽은 6개 법의 이름과 규칙이 출력에 전부 있어야 한다. 표 파싱이 6행이 아니면 렌더 전에 예외로 중단된다.
- **기각 문구 금지**: `이번 조수에 보호 용량`, `법 4 용량`, `4 용량` 이 출력에 있으면 실패 (C3-F24 재발 방지).
- **시간 수용**: `중앙값 420분 이상`, `600분 이하여야`, `상위 25%가 600분` 이 있으면 실패. `total_minus_afk_min` 과 목표 밴드 문구가 없으면 실패.
- **계약 대조**: `premium-preproduction-contract.md` 본문에 `450~540`, `중앙값 < 420`, `하위 25% < 360` 이 남아 있는지 확인한다. 계약이 바뀌면 덱 빌드가 먼저 실패한다.
- **확정 방식**: `확정만 길게 누름` 이 있으면 실패 (RFC-P3-015 F10).
- **도구 없는 비트**: 그 비트 중 `kind: puzzle` 이 하나라도 생기면 12번 슬라이드 문장이 거짓이 되므로 실패.

기존 항목에 더해 r2에서 추가된 게이트:
- 1번이 표지 구조인지, `lead` / `stats` 3개 / `caption` / `h1` 이 있는지.
- 금지된 회계 서술 3종(`고정비가 아니`, `고정비와 섞지`, `비용이 아니라 조건부`)이 출력에 있으면 실패.
- 배분율을 `가정` 표기 없이 적으면 실패.
- **생성기 소스 자체**를 다시 읽어 고정된 단계 값 문자열 5종이 있으면 실패(검사 문자열은 조각으로 조립해 자기 자신을 오탐하지 않는다).
- `campaign.json` 의 모든 단계 분이 실제로 표에 렌더되었는지, 합이 총계와 같은지.
- 노트 창 버튼과 스크립트 존재.
- 기존 항목 유지: 36장, 6줄, 노트 3항목, 출처 존재, em dash / en dash / 이모지 0건, 태그 균형, 외부 자산 참조 0건.

현재 결과: **error 0건**, 경고 1건(외부 링크가 붙은 슬라이드가 16장). 경고는 의도된 상태다. 나머지는 내부 문서를 출처로 쓰는 설계 슬라이드이고, Steam 정책과 금액을 다루는 슬라이드에는 전부 공식 URL 각주가 있다.

## 기하 측정 `[관측] 2026-09-10` (시각 판정 아님)
r3 바이트(100,060 B)로 다시 측정했다. 이번에는 브라우저 뷰포트 에뮬레이션에서 36장을 하나씩 활성화해 각 슬라이드 `.frame` 의 `scrollHeight - clientHeight`, `scrollWidth - clientWidth` 를 읽었다. 8번 슬라이드의 표가 3열 6행에서 정본 원문으로 길어졌기 때문에 재측정이 필요했다.

| 뷰포트 | 넘침 슬라이드 (r3) | 넘침 슬라이드 (r2) |
|---|---|---|
| 1280 x 720 | 0장 | 0장 |
| 1440 x 900 | 0장 | 0장 |
| 1920 x 1080 | 0장 | 0장 |
| 390 x 844 | 0장 | 0장 |

8번 슬라이드는 1280x720 스크린샷으로 육안 확인도 했다. 법 이름 열이 두 줄로 접히지만 잘리지 않는다.

**레이아웃 넘침 측정이지 시각 품질 판정이 아니다.** 가독성, 대비, 표지 인상, 인쇄 결과는 사람이 확인해야 한다.

## 조작 계약 `[관측]`
- 로드 시 1번 활성, 나머지 35장에 `inert` + `aria-hidden="true"`.
- 화살표 4방향, PageUp / PageDown, Space, Home, End, `T` 목차, `N` 노트, `F` 전체화면.
- 목차 다이얼로그 36개 버튼(`01. 제목` 라벨), 클릭 시 이동 후 닫힘.
- 자동 넘김 **기본 꺼짐**, 켜도 수동 조작이 계속 가능하고 수동 조작이 타이머를 초기화한다.
- 모바일 좌우 스와이프.
- 시작 화면에 조작 안내 없음. 안내는 별도 README로 분리한다.
- 인쇄는 `@media print` 로 36장을 A4 가로 한 장씩. **실제 인쇄 결과는 확인하지 않았다.**

## 계산 검산 `[관측]`
기본값 14,900원, 할인 10%, k=1.00, 환불 8%, 차지백 0, 배분 0.7, 준비금 500원.
```
13,410 / 1.1 = 12,190.909...  x 0.92 = 11,215.636...  x 0.70 = 7,850.945...  - 500 = 7,350.945...
```
- 현금 1,000만원 회수 `ceil(10,000,000 / 7,350.945...)` = **1,361본**
- 인건비 107,700,000원 회수 `ceil(107,700,000 / 7,350.945...)` = **14,652본**
- 생산 견적 287인일, 위험여유 25% 적용 359인일, 1인일 30만원 가정 시 107,700,000원, 1인 월 20작업일 가정 시 약 17.9개월(모두 JSON에서 계산).
- 할인 상한 39%는 `floor((1 - 9,000 / 14,900) x 100)` 계산값이고 시뮬레이터 옵션에도 그대로 들어간다 `[관측] 0% 10% 20% 30% 39% 40%`.
- **위 금액에 100달러 등록비, 환율, 적용 세금, 은행 비용은 들어 있지 않다.**

## 정직성 경계
- 게임 빌드 0건, 사람 플레이 n=0, Steam 등록 행위 0건.
- 480분은 설계 예산이고 관측 완주 시간이 아니다.
- 가격 3안 전부 미승인, 배분율은 가정.
- 19번 사이클 표는 `production/cycle-ledger.json` 을 그대로 옮긴 것이고, 이번 빌드 값도 완료가 아니다(검토 반영 2건, 독립 검토 진행 1건, 초안 준비 2건).
- 미확인으로 명시한 것: 데모 추가 등록비 여부, 한미 조세조약 실제 세율, KRW 최저 기준가, 한국 자체등급분류사업자 지정 여부, Steamworks 2FA 필수 여부. 축제 일정은 공식 일자 미확인이라 날짜를 적지 않았다.
- 이 덱은 회계 자문도 세무 자문도 아니며 결제, 서명, 공개, 가격 확정의 근거가 아니다.

## r4 이후 남은 것 `[OBSERVED]` — 이번 루프에서 닫지 않은 내 레인 결함
- **C5-F4 (S3)**: 위 r3 절의 재측정 표가 `fdabf1d4…` · 120,479 B · `44/44` 를 고정 기재하고 있다. r4 절이 현행 실측(`92301c0a…` · 121,457 B · `47/47`)을 새로 기록했지만, **r3 표 자체는 이번 배정 밖이라 손대지 않았다**. r3 표는 그 시점의 측정으로 읽어야 한다.
- **C5-F5 (S3)**: 17번 슬라이드의 `기술 인수 테스트 14개`(정본 §11 = 27개)와 폐기 용어 `매체 경로` 는 이번 배정 밖이라 그대로다. 다음 루프 대상.
- **C4-F18 (S3)**: `presentation/video-study.md` 의 `cycle: …-c2` · `status: draft` 와 채택 순서 불일치도 그대로다.

## r3 이후 남은 것 `[OBSERVED]`
- 덱의 **시각 품질**(가독성, 대비, 인상, 인쇄)은 여전히 사람이 봐야 한다. 넘침 0장은 잘림이 없다는 뜻이지 잘 읽힌다는 뜻이 아니다.
- 8번 표의 `법` 열은 정본 원문이라 길다. 폭을 줄이려면 바이블 3절을 먼저 고쳐야 하며 덱에서 임의로 줄이지 않는다.
- 슬라이드 15의 `verification.evidence` 는 빈 배열이 아니라 "증거 없음"을 적은 문장이다. 덱은 이를 증거로 세지 않는다.
- 노트 창 팝업 동작, 인쇄 결과, 전체화면 조합은 이번에도 사람이 눌러 확인하지 않았다.

## 다음 소유자
사람 시각 검수 후 `game-qa` 독립 검토. 지적은 HTML이 아니라 `generate-deck.mjs` 를 고쳐 재빌드한다. C3 단계 예산이 실제로 개정되면 **재빌드만** 하면 되고 생성기 수정은 필요 없다.
