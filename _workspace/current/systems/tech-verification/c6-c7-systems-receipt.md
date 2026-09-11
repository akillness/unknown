---
updated: 2026-09-10
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
---

# 기술 검증 — C6/C7 systems 레인 (핸드오프 3종 + 잔여 S3 5건)

**Unity 실행 0회 · 빌드 0회 · 프레임타임 캡처 0건 · 사람 플레이 표본 n = 0.**
이 문서의 어떤 값도 게임 측정치가 아니며 **어떤 게이트도 PASS 로 올리지 않는다.** 여기 적힌 것은 전부 **문서·데이터 정합 재측정**이다.

## 0. 이번 회차가 한 일

| # | 배정 | 산출 |
|---|---|---|
| A | 핸드오프 색인 | `_workspace/current/handoff/README.md` (신규) |
| B | Codex 착수 브리프 | `_workspace/current/handoff/codex-unity-brief.md` (신규 · ①~⑫) |
| C | T0 검증 계획 | `_workspace/current/handoff/verification-plan.md` (신규) |
| D-1 | **C4-F9** 도구 제목 통일 | `interaction-rules.md` §2 · `game-ui-contract.json` 3곳 · **RFC-S6** 신설 |
| D-2 | **C4-F14 SC-2·SC-3** 오타 | JSON 4건 + `unity-implementation.md` 대문자 비트 id 1건 |
| D-3 | **C4-F16 + C4-F21** | `interaction-rules.md` **§1-3 신설** + 8개 스펙 **§1-A 키보드 파생** 신설 |
| D-4 | **C4-F22** | `interaction-rules.md` 필터 서술 · `c4-fixloop2-input-binding.md` W-2 · §5 근거 교체 |
| D-5 | **C5-F5**(systems 몫) | 폐기 용어 「매체 경로」 규칙 본문 **7곳** 교체 + `c4-self-check.md` F3 2행 갱신 |
| D-6 | 메타 해시 | `game-ui-contract.meta.md` 표 4행 재측정 + 「개정 3」 + 「미묶음 경고」 해소 기록 |

## 1. 명령과 결과 [OBSERVED 2026-09-10]

| # | 명령 | 결과 |
|---|---|---|
| V-1 | `git status --short` (편집 전) | 다른 세션 변경 다수 관측(`worldview/` · `qa/` · `production/` · `animation/` 등). **본 레인은 읽기만 하고 systems/ 와 handoff/ 밖을 쓰지 않았다** |
| V-2 | `node _workspace/current/planning/validate-campaign.mjs` | `{ checks 47, pass 47, fail 0, verdict PASS }` · `stages 9` · `beats 33` · `clues 73` · `originCatalogSize 31` · `toolBeatCounts { circuit 10, reader 11, alignment 8, routing 3, corrosion 3, seal 7 }` · `designMinutes 480` · `observedMedianMinutes null` · `humanPlaytests []` · exit 0. **해시는 검증기 출력에서 읽는다(RFC-Q1) — 이 문서에 숫자로 재기재하지 않는다** |
| V-3 | `python3 ~/.claude/skills/game-ui-ux/scripts/validate-game-ui.py --self-test` | `PASS: valid accepted` / `PASS: missing back behavior rejected` / `PASS: placeholder rejected` · exit 0 |
| V-4 | `python3 …/validate-game-ui.py game-ui-contract.json` | `PASS: valid game UI contract` · exit 0 (JSON 5곳 편집 **후**) |
| V-5 | `python3 -c "json.loads(...)"` | 편집 전·후 모두 파싱 성공 |
| V-6 | `node _workspace/current/systems/prototype/test-model.mjs` | **37 통과 / 0 실패** · exit 0 — 코드 0줄 변경의 대조 영수증 |
| V-7 | `bash .claude/skills/game-ops-harness/scripts/freshness-check.sh` | `0 finding(s)` · exit 0 (아티팩트 수는 §4) |
| V-8 | §1 입력표 41행 재측정 (아래 명령) | **41행 / 8파일** — corrosion 4 · drainage 6 · dual-seal 6 · hint 4 · plate 5 · save-undo 5 · tide 6 · wiring 5. **§1-A 신설이 이 수를 바꾸지 않았다**(§2 참조) |
| V-9 | `awk -F'\|' '$3 ~ /X/'` | **8행** — 단독 `X` 6 + `LB+X` 2 |
| V-10 | `awk -F'\|' '$3 ~ /X/ && $3 !~ /LB\+X/'` | **6행** — C4-F22 가 요구한 조건이 실제로 6을 낸다 |
| V-11 | KB 토큰 없는 행 ↔ §1-A 대응 검사(파이썬 파서) | **20행 / 20행 대응** · 누락 **0건** |
| V-12 | `grep -rn "매체 경로" _workspace/current/systems` | 본 영수증을 뺀 **5행 / 5파일** — 전부 「폐기했다」는 **선언·기록 문장**이다(`unity-implementation.md` §5 1항 = 폐기 선언 · `plate-readout.md` P-R4 = "이 용어는 쓰지 않는다" · `drainage-routing.md` 변경 로그 · `c4-self-check.md` F3 · `c4-fixloop2-input-binding.md` §4). **규칙 본문 사용 0건**. 용어 원장은 지우지 않는다 — 폐기 사실을 기록하는 문장까지 지우면 다음 회차가 그 용어를 다시 만든다 |
| V-13 | `grep -n "휴은\|중복로\|덴리트\|옥긴다\|즐이고\|부식예산 잔여" game-ui-contract.json` | **0행** |
| V-14 | `grep -n "C3-b2\|C3-b3" unity-implementation.md` | **0행**(소문자 `c3-b2`/`c3-b3` 로 교체 확인) |
| V-15 | `shasum -a 256` (편집 파일 전건) | §3 표 |
| V-16 | `sed -n '1,26p' unity/Unknown/Logs/Editor.log` | `Unity Editor version: 6000.5.6f1 (0e0577a1a2ac)` · `COMMAND LINE ARGUMENTS` = `-projectpath … -useHub -hubIPC …` |
| V-17 | `grep -l batchmode unity/Unknown/Logs/*.log` | **0파일** — **배치모드 실행 기록 없음** |
| V-18 | `ls unity/Unknown/Packages/` | `manifest.json` · `packages-lock.json`(존재) |

§1 입력표 재측정 명령 원문(그대로 재현 가능):

```
$ cd _workspace/current/systems/system-specs
$ for f in *.md; do awk -v F="$f" '/^## 1\. 입력/{ins=1;next} ins && /^## /{ins=0} \
    ins && /^\|/ && $0 !~ /^\|---/ && $0 !~ /^\| *입력 *\|/ {split($0,a,"|"); printf "%s:%d |%s|%s|\n",F,NR,a[2],a[4]}' "$f"; done > rows.txt
$ wc -l < rows.txt                                   # 41
$ awk -F'|' '$3 ~ /X/' rows.txt | wc -l              # 8
$ awk -F'|' '$3 ~ /X/ && $3 !~ /LB\+X/' rows.txt | wc -l   # 6
```

## 2. §1-A 를 **별도 절**로 넣은 이유 (측정 도구를 깨지 않기 위해)

처음에는 「키보드 파생」 표를 §1 표 바로 아래 **같은 절 안**에 넣었다. 그러면 위 awk 의 `ins` 구간에 새 표의 행이 포함되어 **41행 측정이 무너진다** — C4-F21·C4-F22 가 인용하는 좌표계가 그 명령이므로, 결함을 고치는 편집이 결함의 측정 도구를 깨는 셈이 된다.
그래서 블록을 **`## 1-A. 키보드 파생`** 이라는 자체 절로 올렸다(`/^## /` 에서 `ins=0` 이 되므로 구간이 §1 표에서 끝난다). `wiring-trace.md` 는 §1 표 뒤의 「Sim 에 들어가는 것은 …」 문단을 §1 안에 남기기 위해 블록을 **§2 직전**으로 옮겼다.
재측정 결과 41행·8행·6행이 전부 재현된다(V-8·V-9·V-10).

## 3. 편집 파일 해시 [OBSERVED 2026-09-10 · 모든 편집 완료 후]

| 파일 | sha256 |
|---|---|
| `systems/interaction-rules.md` | `8c32f5f3030d10dcc71cb662165b6d81acbedace04ff4693f1291305dd8c7d5a` (41,946 B) |
| `systems/game-ui-contract.json` | `eaf04dae09ac2b488aff15b4b064bebd80b1b3e346bc79bb975fdcf17049e56c` (38,472 B) |
| `systems/game-ui-contract.meta.md` | `0acc60be9cb0bde93c373b1d860e10a696da80648fe38b841f5d69c24a90e135` |
| `systems/unity-implementation.md` | `5e88950a197f36cd5cb8098fd00b5466343793d5953556a8f975d33cae963e54` (13,095 B — 대문자→소문자 1글자쌍, 크기 불변) |
| `systems/system-specs/corrosion-budget.md` | `3723d6b52b59b443fe60dbfef6594c10024c396664070c8bdef6e066a9a58118` |
| `systems/system-specs/drainage-routing.md` | `c187adf8cdb52682ad5142601530b5db8cc24b93f34fb32e4255e41b14544892` |
| `systems/system-specs/dual-seal.md` | `611f6f12d564dacc5ca51357fef4c2f68278ab6f6264cfe913c078961cdf144b` |
| `systems/system-specs/hint-system.md` | `4eb668cc7174041ff785a24ed7335f80e3ee7876de88eb3a6dc8e678c6fa6fc5` |
| `systems/system-specs/plate-readout.md` | `347949be01c0717e546bf90893bf7c554e3136abf70ffbfe01d7d3eff1e5d9a0` |
| `systems/system-specs/save-undo.md` | `9468b31135b966bf08c389b7c3d7990608f3eb98278ec4731f2669a2119016ce` |
| `systems/system-specs/tide-alignment.md` | `6eb7e6408edeaa9a354e323bdfd111fd30f99a7d290efa917617e93cf69b82bf` |
| `systems/system-specs/wiring-trace.md` | `8b4c97530d305f67dbc1e128d62fa212a9166b8f83d45599170669d2e8c620eb` |
| `systems/data-schemas/beats.md` | `88e77bc3650076772967f140eba278955e999086928d81d5972368115f4c4788` |
| `systems/tech-verification/c4-self-check.md` | `ba1876ed0dcd9691ef44e43d48dad1442166f1cd2b0231102aa71af94891ccc2` |
| `systems/tech-verification/c4-fixloop2-input-binding.md` | `f7b2cb8efe8be4a4ac5c5369f7bdcea8478c5cdc9926631b18eccba57211b08e` |
| `handoff/README.md` · `codex-unity-brief.md` · `verification-plan.md` | 신규 3종(해시는 인용 시점에 재측정 · RFC-Q1) |

> 해시는 `shasum -a 256` 로 인용 시점마다 재측정한다. 축약 8자리를 `[OBSERVED]` 로 쓰지 않는다(C3-F2 재발 방지).
> **다른 레인이 같은 회차에 쓴 것**: `animation/animation-contract.md` 가 `ec3ad8c1…`(2,621 B) → `a9a5d3d5…`(2,623 B)로 바뀌었다 — **본 레인의 편집이 아니다**. `handoff/asset-runbook.md`(`owner: game-modeler`)도 이 회차에 다른 레인이 이 폴더에 넣은 파일이며 본 레인은 **읽고 색인에만 올렸다**(`handoff/README.md` §1 행 3.5).

## 4. 신선도

```
$ bash .claude/skills/game-ops-harness/scripts/freshness-check.sh
freshness: 0 finding(s) across 103 markdown artifact(s) under _workspace/current   (exit 0)   # 본 파일 생성 후 재실행
```

- 루프 2 시점 **96** → 핸드오프 3종 생성 후 **102** → 본 파일 생성 후 **103**. 증가분 7 중 **systems 소유는 4**(`handoff/README.md` · `codex-unity-brief.md` · `verification-plan.md` · 본 파일)이고 나머지 3은 같은 회차 다른 레인 산출물이다(`handoff/asset-runbook.md` 포함). 삭제·이동 **0건**.
- **다른 레인 파일 쓰기 0건의 근거**: `git status --short` 는 미추적 디렉터리를 한 줄로 접으므로 파일 단위 증거가 되지 못한다(C4-F22 (b)의 교훈). 근거는 **위 §3 해시 목록** 이며, 그 목록 밖의 파일에 대해 본 레인은 쓰기를 수행하지 않았다.

## 5. 결함별 판정 (QA 재검증 대상)

| 결함 | 요구 | 이번 회차 처리 | 남은 것 |
|---|---|---|---|
| **C4-F9** (S2) | 도구 표시명 **한 벌**로 통일 · 용어집 미등록 해소 | 표시명 정본을 `gdd.md` §4 = `style-guide.md` §9 (6/6 문자 일치 [OBSERVED])로 채택하고 `interaction-rules.md` §2 제목 6개 + 머리 표 신설, `game-ui-contract.json` `screens[9].states`·시나리오 1·결정 1 교체. 자원 `부식예산` ↔ 도구 `부식 시험` 충돌 해소 | **용어집 등재는 worldview 소유** — `배선 추적 / 판독 / 배수 편성 / 부식 시험` 4건이 `glossary.md` 에 **없다** [OBSERVED]. **RFC-S6**(`interaction-rules.md` §7-2)로 올렸다. 등재 전까지 이 4개 표시명은 `[INFERENCE]` 이며 **최종 UI 문자열이 아니다** |
| **C4-F14 SC-2** (S3) | JSON 오타 4건 | 4건 교체(`휠은` · `중복으로` · `Delete` · `옮긴다`) + **자기 발견 1건**(`즐이고`→`줄이고`) + **자기 발견 2곳**(「부식예산 **잔여**」 → 구성안 총비용/상한 9 — RFC-P3-009 위반 표현) | 없음 |
| **C4-F14 SC-3** (S3) | `C3-b2`/`C3-b3` 대문자 | 소문자 교체(V-14) | 없음 |
| **C4-F14 SC-4** | RFC-P3-015 인용 위치 | **디렉터 몫** — 손대지 않았다 | 디렉터 판정 |
| **C4-F16** (S3) | 표면 스코프 우선순위 1절 신설 후 각 스펙이 인용 | `interaction-rules.md` **§1-3 신설**(표면 3종 · 배달 우선순위 · 표면별 키 전수 · 도구별 `Space`/`X`·`Y` 배정 · `reader` 예외). §1-2 대조표의 OPEN 2행을 **해소**로 갱신 | `verification.matrix` **미실행**(패드 실측 0건) — 문서상 무모순일 뿐이다 |
| **C4-F21** (S3) | **행별** 키보드 파생 근거 | `interaction-rules.md` §1-3.4 **D-1~D-7** + 8개 스펙 **§1-A** 신설. KB 토큰 없는 **20행 전건**이 파생 id 를 갖는다(V-11) | D-3 스텝 크기 중 `reader` **배율 단계 목록**은 값 미정 `[TARGET]` — 데이터 노브로 남겼다 |
| **C4-F22** (S3) | 필터 조건 명시 · §5 근거 교체 | 8행/6행 사실과 조건 `&& $3 !~ /LB\+X/` 를 두 문서에 적었고(V-9·V-10 재현), §5 근거를 `git status` → **해시 전후 비교**로 교체 | 없음 |
| **C5-F5** (S3, systems 몫) | 폐기 용어 「매체 경로」 교체 | 규칙 본문 **7곳** 교체 — `plate-readout` 3(P-R4 · P-F1 · B-P2) · `drainage-routing` 2(R-R2 · R-F4) · `beats.md` 1(B-I11) · **`wiring-trace` 1(W-F4 · QA 미지목분)**. `c4-self-check.md` F3 2행 갱신. 재측정 V-12 = 규칙 본문 사용 **0건** | **다른 레인 소유분은 남아 있다** — presentation 2곳(+렌더 1) · economy 2곳 · balance 1곳. 「폐기 문구 게이트에 `매체 경로` 추가」를 켜려면 그 4곳이 먼저 닫혀야 한다 |
| **C5-F6** (관련) | "README 가 없는 브리프를 지시" | `handoff/README.md` 를 만들어 **색인·읽는 순서·RFC 규칙**을 세웠다 | 판정은 QA/디렉터 |
| **기획 counter** (`gdd.md` §4.1) | `plate-readout` P-R2 차단형 서술 · L78 `plateOriginalWear` 폐기 별칭 | 둘 다 정정(P-R2 는 P-R9/P-R3/§2 `Degraded` 와 정합, §5 는 `readBudget`+`readCounts` 두 이름만 남김) | 없음 |

## 6. 이번 회차에 **하지 않은 것**

| 항목 | 상태 | 사유 |
|---|---|---|
| `mex` 실행 | **[SKIPPED: 이번 회차 mex 실행 금지]** | 세션 지시 · CLAUDE.md §10(TeX 동명 바이너리 위험, 정체 검증된 mex-agent 만 실행) |
| `graphify update .` | **[UNGRAPHED]** | 이번 회차 **코드 변경 0줄**(Markdown·JSON 문서만). 코드 그래프의 갱신 대상이 없다. Unity 코드가 생기는 즉시 실행 대상이 된다 |
| `zg index --rebuild` | **미실행** | `handoff/` 폴더가 새로 생겨 레이아웃이 바뀌었다. 사이클 종료 절차(CLAUDE.md §7-3)에서 디렉터가 수행한다 |
| Unity 배치모드 실행 | **0회** | `-batchmode` 로그 0파일(V-17). 첫 실행은 실행자(Codex)가 브리프 §⑩-2 명령으로 수행한다 |
| 성능·플레이 측정 | **0건** | 기준 하드웨어 미정(PRE-1) · 사람 표본 n=0 |
| 용어집 편집 | **0건** | worldview 소유. RFC-S6 로 올렸다 |
| 덱·economy·balance 의 「매체 경로」 | **0건** | 다른 레인 소유 |

## 7. 이 문서가 주장하지 않는 것

- **게임이 존재한다고 주장하지 않는다.** 문서·데이터 정합만 다시 쟀다.
- `47/47 PASS` 는 **저작 JSON 의 구조 검사**다. Unity 임포트·런타임 불변식은 여전히 0건 검증이다.
- `test-model.mjs 37/0` 은 **참조 모형의 내부 일관성**이며 재미·조작감·플레이 시간의 증거가 아니다.
- 핸드오프 3종이 존재한다는 사실은 **G6·G7 을 올리지 않는다.** 계획은 측정이 아니다.
- 인계 지시문의 "헤드리스 열림 확인"은 로그가 뒷받침하지 않는다 — 로그가 증명하는 것은 **Hub 경유 GUI 열림**이며, 배치모드는 아직 0회다(V-16·V-17).
