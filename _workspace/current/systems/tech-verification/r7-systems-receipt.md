---
updated: 2026-09-10
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
---

# r7-systems-receipt — R7 종료 수정 회차 systems 레인 자기 검사표

> **명령 + 관측 결과.** 재현되지 않는 `[OBSERVED]` 는 결함이다(C7-F38 유형) — 아래 모든 값은 적힌 명령을 그대로 실행해 얻은 것이다.
> **이 회차가 올린 게이트는 0개다.** Unity 실행 0회 · 빌드 0회 · 키 입력 0회 · 사람 표본 n=0.
> 승격 대상 문서(`interaction-rules.md` · `unity-implementation.md` · `game-ui-contract.meta.md`)의 `status` 는 **그대로 두었다**(C3-F33 — 승격은 QA 검증 후 소유 레인이 한다).

---

## 0. 이 회차에 배정된 것과 그 처리

| 결함 | 등급 | 무엇을 했나 | 어디에 |
|---|---|---|---|
| **C7-F1** | **S1** | T0 인스턴스 데이터를 **생성기로** 만들었다(손으로 쓴 값 0건) | `systems/data/t0/**` · 영수증 `tech-verification/r7-t0-data.md` |
| **C7-F8** | S2 | T0 확정 = `reader` 인용 고정으로 확정하고 **DoD 6·8 재작성**, 완료 술어 3건 신설 | 브리프 §⑤-3(B)·§⑤-6·§⑩-3 · `interaction-rules.md` §2.2 · `data/t0/beats.json` |
| **C7-F14** | S2 | `ReasonCode` **enum 을 실패 모드에서 도출한 표**로 신설, DoD #5 를 「집합 일치 + `strings/ko.json` 존재」로 완화 | 브리프 §⑤-7 · §⑩-3 #5 · `data-schemas/tools.md` `T-I9` |
| **C7-F4** | S2 | asmdef **7분할 확정**(RFC-S2), `unity-implementation.md` §2 정정, README 읽기 순서 current > draft | `unity-implementation.md` §2 · `architecture-contract.md` §2·§2.2·§14 · `handoff/README.md` §1 |
| **C6-F9** | S2 | 본 생산 조건을 계약 「Base production gate」 **인용만** 하도록 | 브리프 §① · `verification-plan.md` 머리글·§4-4 |
| **C6-F11** | S2 | 착수 전 결정 4건 표 신설(기준 HW 미정 · Input System new + `activeInputHandler=2` · URP · asmdef 7) | 브리프 **§⑦-0** |
| **C6-F12** | S2 | 「되돌림 상한 없음」에 **로그 상한 + `SV-F6` 접힘** 병기 | 브리프 §⑤-2 · U-3 · U-8 · N-12 |
| **C6-F5** | S2 | 관측 지표 **H-10 `manipulation_share`** · **H-11 「손 조작을 재미로 꼽은 응답 수」** 추가 | `verification-plan.md` §1.3·§1.5 · `ops/telemetry-contract.md` §4 |
| **C4-F12** | S2 | `accessibility{}` 를 gdd §8 **13행 전건**으로 넓히고 `gdd_coverage` 맵 신설 + 메타 해시 갱신 | `game-ui-contract.json` · `.meta.md` 개정 6 |
| **C4-F20** | S3 | 지속시간 분기 바인딩 **3건**을 모드 분리/모디파이어로 교체 | `interaction-rules.md` §0-11·§1·§1-2 · `dual-seal.md` L30 · `tide-alignment.md` L30 |
| **C7-F38** | S3 | 재현 불가 `[OBSERVED]` 2건 제거 + **측정 대상 교체**(등장 횟수 → 배정 행 수) | `data-schemas/zones.md` **§2.1** · `tech-verification/c6-c7-fixloop2-systems.md` §1.2 정정 |
| **RFC-S2 / S3 / S5** | — | 세 판정 반영 및 재실행 | 아래 §3 |
| Unity 정정 | — | 「헤드리스 열림 미확인」 → **2차 실행으로 확인됨**(영수증 인용) | 브리프 §①-1 |

**같은 편집에서 함께 닫힌 것**(배정 밖이지만 같은 파일을 열었으므로): **C7-F33**(패키지 게이트 `grep -c` 과다 계수) · **C7-F34**(`Tide.EditorTools` 경계표 미등재) · **C7-F18**(액션 맵 이름 미정) · `dual-seal.md` 같은 파일 자기모순.

---

## 1. 레인 경계 — 다른 레인 파일 쓰기 0건 [OBSERVED]

`git status --short` 는 `_workspace/current/systems/` 와 `handoff/` 를 **디렉터리 한 줄 `??`** 로만 내므로 **파일 단위 쓰기를 보여 주지 못한다**(C4-F22(b) 가 지적한 그 한계). 그래서 mtime 으로 센다.

```
$ cd /Users/jangyoung/orca/unknown
$ find _workspace/current -type f -newermt "2026-09-10 09:35" | wc -l
35
$ find _workspace/current -type f -newermt "2026-09-10 09:35" \
    | grep -v "^_workspace/current/systems/\|^_workspace/current/handoff/" | wc -l
0
$ find assets unity docs scripts .claude .mex -type f -newermt "2026-09-10 09:35" | wc -l
0
```

| 검사 | 값 [OBSERVED 2026-09-10] |
|---|---|
| 이 세션이 수정한 `_workspace/current/` 파일 | **35** (이 영수증 자신을 포함한다 — 이 파일을 다시 고치면 늘어난다. **주장은 이 수가 아니라 아래 두 줄의 0이다**) |
| 그중 `systems/` · `handoff/` **밖** | **0** |
| `assets/` · `unity/` · `docs/` · `scripts/` · `.claude/` · `.mex/` 수정 | **0** |
| `_workspace/archive/` 수정 | **0**(위 두 명령에 포함 — archive 는 `_workspace/current` 밖이며 별도로도 0) |
| `mex` 실행 | **0회**(이번 회차 지시가 금지) |
| git commit / push / add | **0회** |

**삭제·이동 0건.** 폐기된 서술은 지우지 않고 「정정」·「역사로만 읽는다」 표시로 남겼다(CLAUDE.md §2).

---

## 2. 자기 검사표 — 명령과 값

| id | 결함 | 명령 | 값 [OBSERVED 2026-09-10] |
|---|---|---|---|
| **S-1** | C7-F1 | `node _workspace/current/systems/pipeline/emit-tables.mjs --out _workspace/current/systems/data/t0` | exit **0** · 5테이블 + 영수증 + `.meta.md` 6 생성. 전문 = `r7-t0-data.md` |
| **S-2** | C7-F1 | 같은 명령을 `--scope t0` 드라이런으로 재실행해 디스크 파일과 sha256 대조 | **5/5 결정론 일치**(재실행이 같은 바이트를 낸다 · `emittedUtc` 만 다르다) |
| **S-3** | C7-F1 | 생성기의 문서↔규칙 대조 | 염판 앵커 **33/33** · 조위대장 앵커 **28/28** · T0 단서 id·originId·매체 **6/6** 일치 |
| **S-4** | C7-F1 | fail-closed 픽스처 (a) `t0-b1.zoneId` 스테이지 밖 (b) `t0-records.md` §5.3 앵커 1칸 조작 | (a) **exit 1** (b) **exit 2**, 둘 다 **출력 디렉터리 미생성** |
| **S-5** | C7-F8 | `node -e` 로 `data/t0/beats.json` 의 `commitCommandBeats` · `proofRequiredBeats` 비교 | 둘 다 `["t0-b3"]` — **일치**. 불일치 시 생성기가 exit 2 |
| **S-6** | C4-F12 | `awk` 로 `planning/gdd.md` §8 표 행 수 ↔ `node -e` 로 계약 `accessibility` 키·`gdd_coverage` 행 | gdd **13행** ↔ coverage **13행** · `accessibility` 키 **15** · **대응 키 부재 0** |
| **S-7** | C4-F12 | `python3 <skill>/game-ui-ux/scripts/validate-game-ui.py --self-test` 및 계약 파일 | 자체 시험 **3/3 PASS · exit 0** · 계약 **`PASS: valid game UI contract` · exit 0** |
| **S-8** | C4-F12 | `node -e` 재파싱 | 최상위 **13키** · `screens` **19** · `matrix` **20** · `decisions` **13** — 개정 5 직후와 동일(**행 삭제·수정 0건**) |
| **S-9** | C4-F20 | `grep -rnE '짧게 *(=\|다음)\|길게 *(=\|이전)\|(확정\|서명\|기준선).*길게 [0-9]' systems/ handoff/` 에서 정정·변경로그 문장을 제외 | **1행** — 그 1행은 `tech-verification/c4-fixloop2-input-binding.md:69` 의 **결함 서술(역사)** 이며 지시문 0건 |
| **S-10** | C7-F38 | `zones.md` §2.1 의 awk 명령 그대로 (`E` 배정 행) | **0행**(출력 없음) |
| **S-11** | C7-F38 | 같은 명령의 `Q` 판 | **2행** — `tide-alignment.md` §1 「선후 판정 조회」 · 브리프 §⑤-3(A) 「근거 유효성 조회」, 둘 다 `ToolPanel(i)` 조회 배정으로 정본과 일치. **행 번호(31 · 350)는 편집에 따라 움직이므로 인용하지 않는다** |
| **S-12** | C7-F4 | `grep -rn "Tide.Domain" systems/ handoff/` 에서 개명·역사 서술 제외 | **3행**, 전부 **이름 매핑표·RFC 질문문**(역사). 살아 있는 5분할 규칙 **0건** |
| **S-13** | RFC-S3 | `grep -rn '"dayIndex"\|"chapter"' systems/ handoff/` | **3행**, 전부 「이전 판이 갖고 있었다」는 **정정 서술**. 저장 스키마 조각의 실제 필드 **0건** |
| **S-14** | RFC-S5 | `node planning/validate-campaign.mjs` · `--pairs` | `C-07` **PASS** · `proofRequired` **17건** · 독립쌍 존재 **17/17** · 없는 비트 **0** → **예외 없이 성립** |
| **S-15** | Unity 정정 | `grep -c "Exiting batchmode successfully" production/receipts/unity-batchmode/*.log` | `create` **1** · `open-validate` **0**(34행에서 끝) · `open-validate-2` **1** → 「헤드리스 열림」은 **2차만** 증명한다 |
| **S-16** | C7-F33 | `grep -o '"com\.unity\.[a-z0-9.-]*"' packages-lock.json \| sort \| uniq -c \| sort -rn` | 최다 **7회**(`modules.unitywebrequest`) · 6회 2건 — 중첩 `dependencies` 로 **과다 계수 확인**. 게이트를 최상위 키 계수 `node -e` 로 교체 |
| **S-17** | 해시 | `cd systems && shasum -a 256 game-ui-contract.json interaction-rules.md unity-implementation.md ../animation/animation-contract.md` | 앞 3파일 **변경**, `animation-contract.md` **불변**(`a9a5d3d5…`) — 이 레인이 animation 을 건드리지 않았다는 기계 증거. 값은 `game-ui-contract.meta.md` 해시 표가 소유한다 |

---

## 3. 디렉터 판정 반영 확인 (RFC-S2 · S3 · S5)

| RFC | 판정 | 이 회차의 반영 | 재현 |
|---|---|---|---|
| **RFC-S2** | 7분할이 정본 | `unity-implementation.md` §2 전면 재작성(5→7) · `architecture-contract.md` §2.2·§14 에 판정 기록 · §2 표에 `Tide.EditorTools` 등재(C7-F34) · 브리프 §③ 미결 표기 제거 · README §4 「판정됨」 | **S-12** |
| **RFC-S3** | `chapter`/`dayIndex` 제거 승인, **마이그레이션 대상 아님** | 브리프 §⑥-5 재작성(마이그레이터를 만들지 않는다 · 「일차」 UI 라벨 금지) · `architecture-contract.md` §14 판정 기록 · README §4 | **S-13** |
| **RFC-S5** | 예외 없음 유지 | `interaction-rules.md` §3.1 재실행 — 근거였던 「15/15」가 **17/17** 로 바뀌었고 **예외 없이도 PASS**. 본문의 고정 sha·바이트·검사 수를 명령 인용으로 교체(RFC-Q1) | **S-14** |

---

## 4. counter — 배정문과 어긋난 것 1건

**C6-F12 의 「50k/8MB」**. 디렉터 배정표는 로그 상한을 「50,000 엔트리 / 8 MB」로 적었으나, 그 짝은 `payload` 가 없던 시절의 값이고 **C7-F7 해소에서 이미 재산정·폐기**됐다. 현행 정본은 `data-schemas/save.md` §3.2 · `save-undo.md` §9 의 **`byteCap` 6 MiB / `entryCap` 20,000**(세이브 전체 ≤ 8 MB 중 명령 로그 몫)이다 — 엔트리 평균 ≈280 B `[INFERENCE]` 에서 50,000 × 280 = 14 MB 가 되어 8 MB 예산과 모순되기 때문이다.

**처리**: 병기 요구는 이행하되(브리프 §⑤-2 · U-3 · U-8 · N-12 에 `SV-F6` 접힘과 함께 명시) **숫자는 현행 정본을 썼다**. 50,000 을 다시 적으면 같은 저장소에 두 개의 상한이 생긴다. 배정문 수치의 정정은 QA·디렉터 판정 대상으로 올린다.

---

## 5. 이 회차가 만든 열린 항목

| id | 내용 | 대상 |
|---|---|---|
| **OPEN-S8** | `zones.json` 의 Blender Z-up → Unity 좌표 변환식이 `[INFERENCE]`. 인수 `T-Z1` 신설 필요 | systems(실행자) |
| **OPEN-S9** | `systemIds: ["hub"]` 은 구역 토큰 재사용. 캐논은 계통에 고유명을 주지 않는다 | worldview |
| **OPEN-S10** | `hub-uncovered-1/2/3` 표시 이름 = **RFC-N9** 대기. 그 전까지 `displayNameKey: null` | worldview |
| **OPEN-S11** | `hub-view-drawer` 대상 프롭(사물 서랍) 미모델링 | modeling |
| **OPEN-S12** | 신설 텔레메트리 키 `tool_panel_active_min` 의 실측 n=0. `manipulation_share` 는 T0 이후에만 값을 갖는다 | systems · QA |

---

## 6. 이 영수증이 주장하지 않는 것

- **게임이 존재한다고 주장하지 않는다.** Unity 실행 0회 · asmdef 파일 0개 · C# 0줄 · 키 입력 0회.
- **`data/t0/**` 는 저작 산출물이지 런타임 자산이 아니다.** Unity 가 읽은 적이 없고 `ZoneAsset`/`RecordAsset`/`ToolAsset` 변환도 0회다.
- **접근성 8키는 계약 문장이지 측정이 아니다.** 색약 팔레트 3종·채널 볼륨 3채널이 실제로 구분 가능한지는 T0 사람 검증에서만 확인된다.
- **`ReasonCode` 표는 코드가 아니다.** enum 이 실제로 그 집합인지는 `T-I9` 가 빌드에서 검사할 일이며 현재 검사 대상 코드가 없다.
- **모디파이어 레이어(`LB`+`RB`)가 두 명령을 실제로 가르는지 확인되지 않았다.** `matrix[17]`·`matrix[19]` 는 여전히 **미실행**이다.
- **승격하지 않았다.** `interaction-rules.md` · `unity-implementation.md` · `game-ui-contract.meta.md` 의 `status` 는 건드리지 않았다(C3-F33).
