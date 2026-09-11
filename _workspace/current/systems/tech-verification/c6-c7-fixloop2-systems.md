---
updated: 2026-09-10
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
---

# 기술 검증 — C6/C7 수정 루프 2 (systems 레인 · C7-F35)

**Unity 실행 0회 · 빌드 0회 · 프레임타임 캡처 0건 · 사람 플레이 표본 n = 0 · 키 입력 실측 0건 · 패드 실측 0건.**
이 문서의 어떤 값도 게임 측정치가 아니며 **어떤 게이트도 PASS 로 올리지 않는다.** 여기 적힌 것은 전부 **문서·데이터 정합 재측정**과 **스크립트 실행 결과**다. `matrix[19]` 는 이번 루프에서도 **미실행**이다 — 문안을 넓힌 것이지 돌린 것이 아니다.

## 0. 배정과 처리

| 결함 | 심각도 | 상태 | 한 줄 |
|---|---|---|---|
| **C7-F35** | **S2** | **해소** | `data-schemas/zones.md` L53 의 `Q`/`E` 노드 순회를 §1 정본(`Tab`/`Shift+Tab` + `Enter`)으로 정정 → `Q` 는 `ToolPanel(i)` 조회 전용 확정. 채택 근거의 재현 불가 `[OBSERVED]` 문장을 실측으로 교체. **QA 지정 (a) 안** |

**QA 가 확인해야 하는 것**: §1 명령을 그대로 다시 돌려 같은 출력이 나오는지, §2 의 「네 곳이 한 문장인가」 대조가 참인지, 그리고 §4 「해소되지 않은 것」이 정직한지.

## 1. 실행한 명령과 관측 결과 [OBSERVED 2026-09-10 R8]

### 1.1 결함이 주장한 재현 실패의 재현 (선행 상태)

`interaction-rules.md` L127 이 `Q` 채택 근거로 적은 문장은 「`Q`는 저장소 전체에서 미사용 토큰이었다 [OBSERVED 2026-09-10: `grep` 결과 0건]」이었다. 편집 **전** 실측:

```
$ cd /Users/jangyoung/orca/unknown
$ grep -rn '`Q`' _workspace/current/ | wc -l
$ grep -rn '`Q`' _workspace/current/systems/ _workspace/current/handoff/ | wc -l
$ grep -rn '`Q`' _workspace/current/systems/data-schemas/ | wc -l
$ grep -rn '`E`' _workspace/current/ | wc -l
$ grep -rn '`E`' _workspace/current/systems/ _workspace/current/handoff/ | wc -l
```

| 범위 | 편집 전 행 수 | 판정 |
|---|---|---|
| `_workspace/current/` 전체 `` `Q` `` | **30** | 「0건」 **재현 실패** |
| `systems/` + `handoff/` `` `Q` `` | **22** | 대부분 R7 이 만든 바인딩 문서 자신 |
| `data-schemas/` `` `Q` `` | **1** | `zones.md` L53 — **선행 점유** (그 파일 `cycle: 20260909-preproduction-c3`, R7 보다 앞선다) |
| `_workspace/current/` 전체 `` `E` `` | **6** | |
| `systems/` + `handoff/` `` `E` `` | **1** | 같은 `zones.md` L53 |

- 그 1행의 내용: `` | `neighbors` | string[] | no | `Q`/`E` 순회 순서를 결정 | `` — `Shell` 표면의 **시점 노드 이동**을 `Q` 에 걸고 있었다.
- 충돌 상대 2곳: `interaction-rules.md` §1 L29 「시점 노드 이동 … `Tab`/`Shift+Tab` 노드 초점 + `Enter`」 · 같은 파일 §1-3.2 L139 「`Q` @ `Shell` = 없음(발행할 명령 0개)」.
- **R7 의 grep 이 왜 0을 냈는지는 조사하지 않았다.** 지금 측정 가능한 것은 재현 실패라는 사실뿐이며, 원인을 추정해 적지 않는다.

### 1.2 편집 후 재측정

```
$ grep -rnE '`[A-Z]`|`(Tab|Enter|Space|Esc|Delete|F1|RS|LB|RB|LT|D-Pad|back)`' _workspace/current/systems/data-schemas/
```

| 항목 | 편집 후 | 의미 |
|---|---|---|
| `data-schemas/` 안의 키 토큰 행 | ~~**2**~~ → **재현 불가** | ↓ 아래 정정 |
| `data-schemas/` 안의 `Q`·`E` **배정** | **0** | `E` 는 이 정정 뒤 실제로 어떤 표면에도 배정이 없다 (이 줄은 참이며 재현된다) |

> **정정 `[C7-F38 · 2026-09-10 R7 종료]` — 이 절이 적은 「2」는 재현되지 않는다.** 위 명령을 그대로 실행하면 `zones.md` 안에서 **3행**(L53 `neighbors` 정정 · L59 `Q`/`E` 불릿 · L116 변경 로그)이 나온다. R8 측정 시점 이후 같은 파일의 변경 로그 행이 같은 패턴에 걸렸기 때문이며, **정정을 서술하는 문장이 늘어날수록 이 수는 계속 커진다.** 같은 값이 `data-schemas/zones.md` L58 에도 박혀 있었고 그것이 **C7-F38** 로 열렸다.
> **결론(배정 0건 · `E` 무배정)은 참이고 거짓인 것은 인용된 출력값뿐**이다. 등장 횟수는 지표가 될 수 없으므로 측정 대상을 **배정 행 수**로 바꿨다 — 재현 명령과 값은 `data-schemas/zones.md` **§2.1(Z-6·Z-7)** 이 소유한다. 이 절의 숫자는 **역사로만 읽고 인용하지 않는다**(CLAUDE.md §2: 삭제는 없다).

### 1.3 계약 JSON 재파싱·스키마 검증

```
$ cd _workspace/current/systems
$ python3 -c "import json;d=json.load(open('game-ui-contract.json'));print(len(d),len(d['verification']['matrix']),len(d['decisions']))"
$ python3 ~/.claude/skills/game-ui-ux/scripts/validate-game-ui.py game-ui-contract.json; echo exit=$?
```

| 검사 | 결과 |
|---|---|
| `json.load` | 통과 |
| 최상위 키 | **13** (C6 검토 Y-14 값과 동일) |
| `verification.matrix` 행 | **20** (행 추가·삭제 0건) |
| `decisions` | **13** (불변) |
| 스키마 검증기 | `PASS: valid game UI contract` · **exit 0** |

- 편집 방식은 **텍스트 치환 1곳**이다(R7 의 서식 정규화 경로를 쓰지 않았다). `matrix[19].expected` 외 어떤 문자열도 바뀌지 않았다.

### 1.4 해시 재측정

```
$ cd _workspace/current/systems
$ shasum -a 256 game-ui-contract.json interaction-rules.md unity-implementation.md ../animation/animation-contract.md data-schemas/zones.md ../handoff/codex-unity-brief.md
$ wc -c  같은 파일들   #  문자 수는 LC_ALL=en_US.UTF-8 wc -m
```

| 파일 | sha256 (R8) | `wc -c` | 문자 수 | R7 대비 |
|---|---|---|---|---|
| `systems/game-ui-contract.json` | `11cb5c17…0fbf` | 40,605 B | 24,548 | 바뀜 (`f9cda866…`) |
| `systems/interaction-rules.md` | `40280587…723c` | 50,215 B | 28,734 | 바뀜 (`71720699…`) |
| `systems/data-schemas/zones.md` | `6bac0e60…9d79` | 9,815 B | 6,866 | 바뀜 (R7 에 없던 표 대상) |
| `handoff/codex-unity-brief.md` | `1681bbea…09f5` | 76,665 B | 52,152 | 바뀜 |
| `systems/unity-implementation.md` | `7d0a9afa…2c45` | 15,421 B | 9,511 | **불변** (건드리지 않음) |
| `animation/animation-contract.md` | `a9a5d3d5…8682` | 2,623 B | 1,537 | **불변** (systems 소유 아님) |

- 전체 해시는 인용 시점마다 재측정한다(RFC-Q1: 고정 숫자 재기재 금지). 위 축약 8자는 **식별용**이며 정본은 `game-ui-contract.meta.md` §해시 표다.

## 2. 「네 곳이 한 문장인가」 대조 `[C7-F35 required_fix]`

결함이 요구한 것은 "세 문서와 `matrix[19]` 가 한 문장을 말해야 한다"이다. 대상을 4곳으로 세고 대조했다.

| # | 문서 | 위치 | 편집 후 문장 | 일치 |
|---|---|---|---|---|
| 1 | `systems/interaction-rules.md` | §1 「시점 노드 이동」 행 (바인딩 **정본**) | 키보드 `Tab`/`Shift+Tab` 초점 + `Enter` · 패드 좌스틱 + `A` · 마우스 노드 클릭 | 기준 (변경 없음) |
| 2 | `systems/interaction-rules.md` | §1-3.2 `Q` 행 + 아래 불릿 2 | `Q` @ `Shell` = 명령 0개 — **시점 노드 이동이 아니다**. `E` 는 어떤 표면에도 배정 없음 | ● |
| 3 | `systems/data-schemas/zones.md` | §2 `neighbors` 행 + 불릿 2 + §7 | `neighbors` 는 **순서만** 정하고 키를 배정하지 않는다. 입력 정본은 §1. **`Q`/`E` 가 아니다** | ● |
| 4 | `handoff/codex-unity-brief.md` | ⑦-1 노드 이동 행 · `K-9` · `T-26c` | 같은 문장 + `T-26c` 에 「`Shell` 에서 `Q`·`E` 를 눌러도 노드가 이동하지 않는다」 인수 조건 추가 | ● |
| 5 | `systems/game-ui-contract.json` | `verification.matrix[19].expected` | 「… 셸의 시점 노드 이동은 탭과 시프트 탭 초점 이동 후 엔터로만 이뤄지며 Q나 E로는 이뤄지지 않는다」 | ● |

재현 명령:

```
$ grep -rn 'C7-F35' _workspace/current/systems/ _workspace/current/handoff/
$ grep -n '시점 노드 이동' _workspace/current/systems/interaction-rules.md _workspace/current/handoff/codex-unity-brief.md
$ python3 -c "import json;print(json.load(open('_workspace/current/systems/game-ui-contract.json'))['verification']['matrix'][19]['expected'])"
```

## 3. 채택안과 기각안

| 안 | 내용 | 판정 | 근거 |
|---|---|---|---|
| **(a)** | `zones.md` L53 을 §1 정본으로 정정하고 `Q` 를 도구 패널 조회 전용으로 확정 | **채택** | `Q` 는 이미 스펙 2종(`wiring-trace.md` · `tide-alignment.md`) · `interaction-rules.md` 2절 · 브리프 4곳 · `matrix[19]` 에 심어져 있다. 충돌은 **스키마 문서 한 행**뿐이고, 그 행은 애초에 바인딩을 소유할 자격이 없다(스키마는 순서만 소유) |
| (b) | 제3의 미사용 키를 조회에 새로 배정 | **기각** | 위 7곳을 전부 다시 맞춰야 하며 얻는 것이 없다. 그리고 **「미사용 키를 고른다」는 절차 자체가 이번 결함의 원인**이다 — R7 이 그 절차를 밟다 틀렸다. 키를 바꾸는 대신 **정본이 둘로 갈라지는 경로**를 닫는 것이 재발을 막는다 |

**재발 방지로 새로 세운 경계** (`zones.md` §2 불릿 1): 데이터 스키마는 *순회 순서·연결성*만 소유하고 *키 배정*은 소유하지 않는다. 스키마 행은 `interaction-rules.md` 를 **인용**할 뿐이다. 편집 후 실측으로 `data-schemas/` 안의 키 배정은 0건이다(§1.2).

## 4. 해소되지 않은 것 / 미측정

| id | 내용 |
|---|---|
| U1 | **`matrix[19]` 미실행.** 「`Shell` 에서 `Q`·`E` 가 노드를 이동시키지 않는다」는 **설계 주장**이며 관측이 아니다. Unity 실행 0회이므로 이번 회차에 관측으로 바꿀 방법이 없다. `matrix[12]`·`matrix[17]`·`matrix[18]` 도 미실행 그대로다 |
| U2 | **`Q` 는 한 번도 눌린 적이 없다** (R7 고지 그대로 유효). 재매핑 UI 가 `Q` 를 다루는지, 비-QWERTY 배열(AZERTY 의 `A` 자리 · Dvorak)에서 손가락 위치가 타당한지는 **UI/UX 미확인** — C6 검토가 「유효 · 미해소」로 남긴 항목이며 이번 루프도 닫지 못했다 |
| U3 | **R7 grep 의 0건 원인 미조사.** 재현 실패만 기록했다. 같은 종류(정본 두 벌)의 재발 여부는 QA 의 `grep` 대조로 계속 봐야 한다 |
| U4 | `zones.md` §6 미측정은 그대로다 — `loadCostMb` · `viewNodes` 실제 개수 · 씬 용량 **n = 0** |
| U5 | 이번 편집으로 **새로 측정된 게임 값 0건.** 바뀐 것은 문서 정합뿐이다 |
| U6 | `mex` 미실행 (세션 지시). 그래프·메모리 영수증 없음 — `[UNGRAPHED]` |
