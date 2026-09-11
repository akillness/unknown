---
updated: 2026-09-10
cycle: 20260909-preproduction-c5
status: draft
supersedes: _workspace/archive/20260909-preproduction-c4/systems/game-ui-contract.meta.md
owner: game-systems-designer
describes: _workspace/current/systems/game-ui-contract.json
---

# game-ui-contract.json 메타 (C5 — C4 검토 대응본)

## 소유·출처
- 소유 레인: `game-systems-designer`.
- 스키마 출처: 계정 스킬 `game-ui-ux` v1.0, 검증기 `scripts/validate-game-ui.py`(읽기 전용, 표준 라이브러리 전용).
- 개정 근거: `_workspace/current/qa/c4-review.md` (F1·F2·F3·F4). 세계·기획 근거는 `worldview/worldview-bible.md`, `planning/gdd.md`, `production/premium-preproduction-contract.md`.
- 형제 문서: `systems/unity-implementation.md`, `systems/interaction-rules.md`, `animation/animation-contract.md`(동기화 정정분).

## 해시 — 2026-09-10 **R7b 종료(C7-F40) 재측정** [OBSERVED]

명령(그대로 재현 가능):
```
$ cd _workspace/current/systems
$ shasum -a 256 game-ui-contract.json interaction-rules.md unity-implementation.md ../animation/animation-contract.md
$ wc -c   같은 파일들
```

| 파일 | sha256 | 크기 (`wc -c`) | 문자 수 |
|---|---|---|---|
| `systems/game-ui-contract.json` | `5b3ffdb15ee30d6a06b8960d3b99900ef64e647d15fd78baaf2e0ae9f9cdabc0` | 43,643 B | 26,162 |
| `systems/interaction-rules.md` | `75bc0fd26ad225c11722726cb2c4e17469315271be563fd1e726d2ebb9ef8874` | 55,479 B | 31,754 |
| `systems/unity-implementation.md` | `0fcf0e8c6929bf8c3a1ce7e97cf62d050a0163226ea6c201d3177dcbf7b90ec5` | 18,022 B | 11,186 |
| `animation/animation-contract.md` | `a9a5d3d577abfde968e2d69ff6a9bead1393f0166c340439acb2c800239a8682` | 2,623 B | 1,537 |

> 위 네 값은 **R7 종료 수정의 모든 편집이 끝난 뒤** 측정한 것이다 [OBSERVED 2026-09-10]. 직전(R8) 값은 `11cb5c17…`(40,605 B) · `40280587…`(50,215 B) · `7d0a9afa…`(15,421 B) · `a9a5d3d5…`(불변)이다 — 이번 회차에 바뀐 것은 **앞의 세 파일**이고 `animation-contract.md` 는 **건드리지 않았으며 해시도 불변**이다(이 레인의 편집이 아니다).
> 같은 편집에 속하지만 이 표의 대상이 아닌 파일: `systems/data-schemas/{zones,tools,plates,beats,hints}.md` · `systems/system-specs/{dual-seal,tide-alignment,plate-readout}.md` · `systems/architecture-contract.md` · `systems/ops/telemetry-contract.md` · `systems/pipeline/emit-tables.{mjs,meta.md}` · `systems/data/t0/**`(생성기 출력 12파일) · `handoff/{README,codex-unity-brief,verification-plan}.md`. 각 파일의 값은 필요할 때 `shasum -a 256` 을 다시 돌려 읽는다(RFC-Q1 — 고정 숫자를 여러 문서에 복제하지 않는다).
>
> **R8 원문(보존)**: 위 네 값은 **R8 수정 루프 2의 모든 편집이 끝난 뒤** 측정한 것이다 [OBSERVED 2026-09-10 R8]. 직전(R7) 값은 `f9cda866…`(40,476 B) · `71720699…`(45,540 B) · `7d0a9afa…`(불변) · `a9a5d3d5…`(불변)이다 — 이번 회차에 바뀐 것은 앞의 두 파일뿐이고 `unity-implementation.md` · `animation-contract.md` 는 **건드리지 않았으며 해시도 불변**이다. 같은 편집에 속하지만 이 표의 대상이 아닌 두 파일: `systems/data-schemas/zones.md` `6bac0e60cee83ddfd69be565be38ffe90db217b3906ddb3e0554dab0a4da9d79` · 9,815 B · 6,866자 · `handoff/codex-unity-brief.md` `1681bbea16e74c6e7024757fac8396ef61c0654708790b2fdcbd52b7152b09f5` · 76,665 B · 52,152자. 「문자 수」 열은 `LC_ALL=en_US.UTF-8 wc -m` 값이다.
>
> **R7 원문(보존)**: 위 네 값은 **R7 수정 루프 1의 모든 편집이 끝난 뒤** 측정한 것이다. 직전(R6) 값은 각각 `eaf04dae…`(38,472 B) · `8c32f5f3…`(41,946 B) · `5e88950a…`(13,095 B) · `a9a5d3d5…`(불변)이다. `animation-contract.md` 는 이 회차에 systems 가 **건드리지 않았고** 해시도 불변이다.

- **표 구조 정정 [C4-F5 편집 부수]**: 이전 판은 개정 문단이 표 중간에 끼어 네 파일 행이 한 표로 렌더되지 않았다. 행을 한 표로 모으고 개정 노트를 아래로 내렸다. **값·행 삭제 0건.**
- **단위 정정**: 이전 판의 "chars"는 문자 수였고 `wc -c`(바이트)와 다르다. 두 열을 모두 적어 다음 인용이 단위를 혼동하지 않게 한다.
- **스테일 1행 정정 [C4-F14 SC-2 관련]**: `interaction-rules.md` 행은 `af906dfb…`·6,236 chars 였고 이는 09-09 판본 값이다(R4 편집 **전**에도 이미 9,131 chars 로 어긋나 있었다). 위 값으로 재측정 교체했다. `unity-implementation.md`·`animation-contract.md` 두 행은 재측정 결과 **해시 일치** — 스테일이 아니었다.
- **C4-F5 파생 정정으로 함께 바뀐 파일**(이 표의 대상은 아니지만 같은 편집에 속한다) [OBSERVED 2026-09-10 R4]: `system-specs/hint-system.md` `239361f0…` · `system-specs/plate-readout.md` `e284dd5e…` · `tech-verification/README.md` `ace675a5…`(C4-F8).
- 해시는 인용 시점마다 재측정한다(RFC-Q1: 고정 숫자 재기재 금지). 위 `interaction-rules.md` 값은 **본 루프의 모든 편집이 끝난 뒤** 측정한 최종값이다.
- **2026-09-10 R4 수정 루프 2 재측정 [OBSERVED · C4-F19]**: `interaction-rules.md` 행이 위 표에서 다시 바뀌었다 — 루프 1 값 `3c901743…` · 25,545 B · 14,010자 → **`4f6b2546…` · 28,808 B · 15,975자**. 편집 범위는 **§1-2 한 절**(스펙 대조 절 재작성 + 전건 표 `Y` 행 모디파이어 열 정정)이며 §0·§1·§1-1 및 §2 이후는 불변이다. 같은 편집에 속하는 스펙 파일(이 표의 대상은 아니다): `system-specs/drainage-routing.md` **`389834d30d060e678b4a134879cf8590c10e8d093cbb9a24aab6f8328d41ffef`** · 11,165 B · 7,077자 — §1 「연결 해제」 행 1줄 + 변경 로그 절 신설.
- **`game-ui-contract.json` 은 이번 루프에서 편집하지 않았다** [OBSERVED]: 재측정 해시 `9c89e9ae…` **불변**. C4-F19 는 계약 JSON 이 아니라 스펙 md 쪽 스테일이었다 — JSON L153 「패널이 열린 동안 X는 프리뷰 Y는 해제로 고정된다」 · L528 · L620 이 이미 정본이다. `unity-implementation.md` `453f8481…` · `animation-contract.md` `ec3ad8c1…` 도 재측정 결과 불변.
- **문자 수 열의 측정 로케일 [OBSERVED]**: `wc -m` 은 기본 로케일(`C`)에서 바이트 수와 같은 값을 낸다. 이 표의 「문자 수」는 `LC_ALL=en_US.UTF-8 wc -m` 값이다. 로케일을 적지 않으면 다음 회차가 "재현 불가"로 읽는다.

**2026-09-10 개정 7 [C7-F40 · R7b 마무리]**: 「일차」 UI 라벨 금지(RFC-S3 — 캐논은 단일 야간, `dayIndex` 없음)를 **텍스트 치환 두 곳**으로 반영했다(서식 정규화 없음).

| # | 위치 | 전 | 후 | 결함 |
|---|---|---|---|---|
| 1 | `screens[].visible` 체크포인트 항목 (L360) | 「체크포인트 목록과 각 지점의 장 **일차** 조위 위상」 | 「체크포인트 목록과 각 지점의 **스테이지** 조위 위상」 | **C7-F40** |
| 2 | `layout.anchors[0]` (L396) | 「상단 좌측에 장과 **일차**와 조위 위상 표시」 | 「상단 좌측에 **스테이지**와 조위 위상 표시」 | **C7-F40** |

- 형제 문서 동시 정정: `systems/interaction-rules.md` **L315** 「체크포인트 라벨(장·일차·조위 위상)」 → 「체크포인트 라벨(**스테이지**·조위 위상)」. 이 세 행이 C7-F40 의 전부이며 다른 행·키·순서는 **건드리지 않았다**.
- 재현 [OBSERVED 2026-09-10 R7b]:
```
$ cd _workspace/current/systems
$ grep -rn "일차" .            # → tech-verification/r7-systems-receipt.md:96 한 행만 남음(금지 규칙을 인용한 영수증 문장이며 UI 라벨이 아니다)
$ python3 .../validate-game-ui.py game-ui-contract.json   # PASS: valid game UI contract (exit 0)
$ python3 -c "import json;j=json.load(open('game-ui-contract.json'));print(len(j),len(j['screens']),len(j['verification']['matrix']),len(j['decisions']))"
13 19 20 13        # 개정 6 직후와 동일 — 행 삭제·추가 0건
$ shasum -a 256 game-ui-contract.json interaction-rules.md ; wc -c ; LC_ALL=en_US.UTF-8 wc -m
```
- **이 편집은 무엇도 재지 않았다.** 라벨 문면 정합일 뿐이며 어떤 게이트도 올리지 않는다. `unity-implementation.md`·`animation/animation-contract.md` 는 이번에 **건드리지 않았고 해시도 불변**이다(위 표의 두 행 그대로).

**2026-09-10 개정 6 [C4-F12 · R7 종료 수정]**: `accessibility{}` 를 **텍스트 치환 한 곳**으로 넓혔다(서식 정규화 없음).

| # | 위치 | 전 | 후 | 결함 |
|---|---|---|---|---|
| 1 | `accessibility` | 키 **6**(`signals` `settings_entry` `hold_alternative` `keyboard_only` `motion` `media_alternatives`) | 키 **15** — 위 6 + `remapping` · `discrete_alternatives` · `text_scale` · `color_vision_palettes` · `audio_channel_volumes` · `progress_support` · `no_time_pressure` · `languages` + **`gdd_coverage`**(13행 대응표) | **C4-F12** |
| 2 | `accessibility.media_alternatives` | 「… 자막과 문자 알림을 제공한다」 | 「… 자막과 **화자 이름**과 문자 알림을 제공한다」 | C4-F12 — `gdd.md` §8 「전체 자막 · **화자 이름** · 효과음 문자 알림」의 누락분 |

- **무엇이 결함이었나**: `planning/gdd.md` §8 이 접근성 항목의 소유자를 `game-ui-contract.json` `accessibility` 로 지정했는데 그 객체가 13행 중 **3행을 전혀 덮지 않았다**(색약 대체 팔레트 · 채널별 볼륨 · 연속값 이산 대안) — 「`색약` 0회 · `볼륨` 0회 · `이산` 0회」. 이번 편집은 그 3행을 포함해 **13행 전건**에 키를 배정하고, 대응을 사람이 눈으로 세지 않도록 **`gdd_coverage` 맵**을 함께 넣었다.
- **재현 [OBSERVED 2026-09-10]**: `node -e "const j=require('./game-ui-contract.json'); console.log(Object.keys(j.accessibility).length, Object.keys(j.accessibility.gdd_coverage).length-1)"` → `15 13`. 대응표의 13개 값이 모두 같은 객체의 실재 키인지도 같은 명령 계열로 확인 가능하다.
- **스키마 검증** [OBSERVED 2026-09-10]: `python3 <skill>/game-ui-ux/scripts/validate-game-ui.py --self-test` → **3/3 PASS · exit 0** · 같은 검증기로 계약 파일 → **`PASS: valid game UI contract` · exit 0** · `json.load` 통과. **최상위 13키 · `screens` 19 · `matrix` 20 · `decisions` 13 — 개정 5 직후와 동일**(행 삭제·수정 0건).
- **이 편집은 무엇도 재지 않았다.** 새 키 8개는 전부 `[TARGET]` 계약 문장이며 **접근성 실사용 검증은 여전히 0건**이다(아래 「관측과 목표의 구분」 NOT-MEASURED 목록 그대로). 색약 팔레트 3종·채널 볼륨 3채널이 실제로 구분 가능한지는 T0 사람 검증(§G4)에서만 확인된다.
- 이전 판의 「C4-F12 는 이번에도 포함하지 않았다 … 그 항목을 열 때 해시가 한 번 더 갱신된다」는 **이 개정으로 이행됐다**. 예고한 대로 해시를 갱신했고, 같은 편집에서 C4-F20(패드 `RB` 겸용) 파생 문서도 함께 처리해 갱신 횟수를 늘리지 않았다.

**2026-09-10 개정 5 [C7-F35 · R8 수정 루프 2]**: JSON 을 **텍스트 치환 한 곳**만 고쳤다(서식 정규화 없음).

| # | 위치 | 전 | 후 | 결함 |
|---|---|---|---|---|
| 1 | `verification.matrix[19].expected` | 「… 셸에서 Q는 아무 명령도 발행하지 않는다」 | 「… 셸에서 Q는 아무 명령도 발행하지 않고 **셸의 시점 노드 이동은 탭과 시프트 탭 초점 이동 후 엔터로만 이뤄지며 Q나 E로는 이뤄지지 않는다**」 | **C7-F35** — 이전 문안은 `Q` 의 **부재**만 주장하고 노드 이동의 **정본**을 적지 않아, `data-schemas/zones.md` L53(「`Q`/`E` 순회」)과 `interaction-rules.md` §1 L29(`Tab`/`Shift+Tab` + `Enter`)의 모순이 이 인수 조건 안에서 **판정 불가**였다. 인수 조건이 둘 중 어느 쪽을 검사하는지 이제 한 문장으로 정해진다 |

- 다른 `matrix` 행 · `decisions[]` · 최상위 키는 **건드리지 않았다.** 재파싱 결과 **최상위 13키 · `matrix` 20행 · `decisions` 13** — 개정 4 직후와 동일 [OBSERVED 2026-09-10 R8].
- 스키마 검증 `python3 ~/.claude/skills/game-ui-ux/scripts/validate-game-ui.py game-ui-contract.json` → `PASS: valid game UI contract` · **exit 0** [OBSERVED 2026-09-10 R8].
- **`matrix[19]` 는 여전히 미실행이다.** 문안을 넓힌 것이지 돌린 것이 아니다 — `Q`·`E`·`Tab` 어느 키도 눌린 적이 없고 Unity 실행 0회다. `matrix[12]`·`matrix[17]`·`matrix[18]` 도 미실행 그대로다.
- 같은 결함으로 함께 바뀐 형제 문서: `systems/data-schemas/zones.md`(§2 `neighbors` 행 · 불릿 2 · §7 변경 로그 신설) · `systems/interaction-rules.md`(§1-3.1 (B)안 근거 실측 정정 · §1-3.2 `Q` 행 + 불릿 2 · §9 R8 행) · `handoff/codex-unity-brief.md`(⑦-1 노드 이동 행 · K-9 · `T-26c`). 네 곳이 한 문장을 말하는지는 `tech-verification/c6-c7-fixloop2-systems.md` §1 의 대조 명령으로 재현한다.

**2026-09-10 개정 4 [C7-F9 · C7-F10 · R7 수정 루프 1]**: JSON 을 **한 번의 편집으로** 네 곳 고쳤다.

| # | 위치 | 전 | 후 | 결함 |
|---|---|---|---|---|
| 1 | `verification.matrix[12].expected` | 「X는 프리뷰만 하고 Y는 해제만 하며 …」 | 「X는 그 패널이 **등재한 부작용 없는 실행 하나**만 하고 Y는 **등재한 의미 하나**만 하며 배수 편성에서는 그것이 각각 가상 시험과 연결 해제이고 …」 | **C7-F9** — 이전 문안은 `routing` 에서만 우연히 참이었고 T0 두 도구(`circuit` X=구획 접기 · `reader` X=재생, Y는 `circuit` **없음**)에서는 거짓이었다 |
| 2 | `verification.matrix[12].state` | 「도구 패널이 열린 배수 편성」 | 「… (등재 의미 X=가상 시험 Y=연결 해제)」 | C7-F9 — 이 행이 *어느 도구의* 등재를 검사하는지 명시 |
| 3 | `verification.matrix` **19·20번째 행 추가** | — | (19) T0 두 도구의 X/Y 실체화 (20) `Q` 조회 키와 `I`·`H`·`F1` 오버레이 진입점의 분리 | **C7-F9 · C7-F10** — 기존 행 삭제 **0건**. `matrix` 18행 → **20행** |
| 4 | `decisions[]` 도구 패널 결정문 | 「패널이 열린 동안 X는 프리뷰 Y는 해제로 고정된다」 | 「… X는 **등재한 부작용 없는 실행 하나** Y는 **등재한 의미 하나**로 고정되고 **등재가 없으면 아무 명령도 발행되지 않는다**」 | C7-F9 — 같은 틀린 문안이 세 번째로 살아 있던 자리 |

- 스키마 검증 `python3 …/validate-game-ui.py game-ui-contract.json` → `PASS: valid game UI contract` · **exit 0** [OBSERVED 2026-09-10 R7]. `json.load` 도 통과.
- **두 새 행은 미실행이다.** `matrix[18]`·`matrix[19]` 는 시나리오를 **세운 것**이지 돌린 것이 아니다. 패드·키보드 실측은 여전히 **0건**이며 `matrix[12]`·`matrix[17]` 도 미실행 그대로다.
- **서식 정규화 고지 [정직성]**: 이 편집은 `json.loads(object_pairs_hook=OrderedDict)` → 값 수정 → `json.dumps(indent=2, ensure_ascii=False)` 경로로 수행했다. 그 결과 **내용 변경(+약 1.1 KB) 외에 공백 서식이 함께 정규화됐다(약 +0.8 KB)** — 이전 판은 짧은 문자열 배열을 한 줄로 유지하는 **손 서식**이었고 지금은 전 배열이 펼쳐져 있다. **값·키·순서의 손실은 0** 이다(파싱→수정→직렬화 한 번에 수행했으므로 위 4곳 외에는 구조적으로 바뀔 수 없고, 스키마 검증이 통과했다). 다만 다음 회차의 diff 가 이 한 번은 넓게 보인다는 사실을 감추지 않는다. 이후 편집은 **텍스트 치환**으로 하여 서식을 유지한다(개정 4의 #4 는 그 방식으로 했다).
- **C4-F12**(`accessibility{}` 3키 · 색약 팔레트 순회 행 · 채널별 볼륨 · 연속값 이산 대안)는 **이번에도 포함하지 않았다** — 이번 회차 배정 밖이다. 그 항목을 열 때 해시가 한 번 더 갱신된다. 배정되면 같은 편집에서 처리한다.

**2026-09-10 개정 3 [C4-F9 · C4-F14 SC-2 · R6(C6/C7)]**: JSON 을 **한 번의 편집으로** 다섯 곳 고쳤다.

| # | 위치 | 전 | 후 | 결함 |
|---|---|---|---|---|
| 1 | `screens[9].states`(도구 패널 6종 라벨) | 회로 지도 · 판독기 · 조위정합 · 경로 구성 · 부식예산 · 이중서명 | **배선 추적 · 판독 · 조위정합 · 배수 편성 · 부식 시험 · 이중서명** | C4-F9 |
| 2 | `verification.matrix` 시나리오 `state` | "도구 패널이 열린 경로 구성" | "도구 패널이 열린 **배수 편성**" | C4-F9 |
| 3 | `decisions[]` R4 T0 항목 | "회로 지도와 판독기 두 도구로 **즐이고**" | "**배선 추적과 판독** 두 도구로 **줄이고**" | C4-F9 + 자기 발견 오타 1건 |
| 4 | `matrix[12].expected` · `matrix[15].expected` · `decisions[]` R1 항목 | 도구 **휴**은 / 출처 중복**로** / **덴리트** · **옥긴다** | 도구 **휠**은 / 출처 중복**으로** / **Delete** · **옮긴다** | C4-F14 SC-2 (오타 4건) |
| 5 | `player_decisions[].required_information` · `information_hierarchy` 항목 | "부식예산 **잔여**" | "구성안 총 부식 비용과 **전역 상한 9**" | **자기 발견** — RFC-P3-009(소모·잔량 없음)와 어긋난 표현 2곳 |

- 검증 [OBSERVED 2026-09-10]: `python3 <skill>/game-ui-ux/scripts/validate-game-ui.py --self-test` → 3 PASS / exit 0 · 같은 검증기로 계약 파일 → `PASS: valid game UI contract` / exit 0 · `json.loads` 편집 전후 통과.
- 표시명 정본과 **용어집 미등재 4건**의 처리는 `interaction-rules.md` §2 머리 표와 **RFC-S6**(같은 파일 §7-2)에 있다. 용어집 편집은 worldview 소유이므로 systems 는 하지 않았다.
- `animation/animation-contract.md` 행의 해시·크기 변동(2,621 → 2,623 B)은 **이 레인의 편집이 아니다** — 같은 회차에 animation 레인이 자기 파일을 고친 결과이며 여기서는 재측정값만 옮겨 적는다 [OBSERVED 2026-09-10].

**2026-09-10 개정 2 [C4-F5 · R4 수정 루프 1]**: `verification.matrix` 에 **18번째 행**을 추가했다 — "패널 밖에서 LB를 눌렀다 뗀 뒤 LB와 X로 되돌림 → 도구 순환과 되돌림이 동시에 발행되지 않는다". 기존 `matrix[12]` 는 `X`/`Y` 만 검사해 컨트롤러 `LB` 겸용 축을 덮지 못했다(C4 검토 F5). 근거·규칙 본문은 `interaction-rules.md` §1-2. 변경 1곳(행 추가), 기존 행 **수정·삭제 0건**. 이전 해시 `83b92c82…`(37,990 bytes)는 아래 「개정 1」 직후 판본이다.
명령·결과 [OBSERVED 2026-09-10 R4]:
```
$ python3 <skill>/game-ui-ux/scripts/validate-game-ui.py --self-test
PASS: valid accepted / PASS: missing back behavior rejected / PASS: placeholder rejected   (exit 0)
$ python3 <skill>/game-ui-ux/scripts/validate-game-ui.py game-ui-contract.json
PASS: valid game UI contract   (exit 0)
$ shasum -a 256 game-ui-contract.json ; wc -c game-ui-contract.json
9c89e9aee096134d6a249609e0002b80bc5c29c7e827ff290994747d156bc5da  38429
```
> **미묶음 경고 — 2026-09-10 R6(C6/C7)에서 해소 · R7 재확인**: 위 묶음 중 **C4-F9(도구 표시명)와 C4-F14 SC-2(오타 4건)를 한 편집으로 처리**했고 해시를 같은 편집에서 갱신했다(위 표 · 아래 「개정 3」). **C4-F12**(`accessibility{}` 3키 · 색약 팔레트 matrix 행)는 이번에도 **포함하지 않았다** — 배정 밖이며 다음에 그 항목을 열 때 해시가 **한 번 더** 갱신된다.

**2026-09-10 개정 1 [RFC-P3-015 F10]**: 계약 안에 남아 있던 "확정은 길게 누름을 요구한다" 두 항목을 정본(`two-step` 기본 · `hold` opt-in)으로 정정했다. 변경 2곳 — `screens[].states` 검수 시나리오의 `expected` 1건, `decisions[]`의 `decision`/`basis` 1건. `json.loads` 편집 전후 통과 확인. 이전 해시 `2c1ed740…`(37,744 bytes)는 09-09 판본이다.
명령: `shasum -a 256 _workspace/current/systems/game-ui-contract.json` · `wc -c`.

> 이 개정 1 문단 아래에 흩어져 있던 형제 문서 3행(`unity-implementation` · `interaction-rules` · `animation-contract`)은 **위 「해시」 표로 합쳤다** — 값 손실 없음(해시 3건 그대로, `interaction-rules` 만 재측정 교체). 중복 표기를 남기면 같은 파일에 두 개의 크기 단위가 공존한다.

JSON에는 frontmatter를 넣지 않았다(파서 보호). 메타는 이 파일이 단독 보유한다.

## 검증 실행 `[OBSERVED]`
```
python3 /Users/jangyoung/.aside/u/0/skills/user/game-ui-ux/scripts/validate-game-ui.py --self-test
python3 /Users/jangyoung/.aside/u/0/skills/user/game-ui-ux/scripts/validate-game-ui.py game-ui-contract.json
```
- 환경: Python 3.13.12(기존 설치), macOS, 네트워크 접근 없음, 설치·결제 없음.
- 자체 시험: `valid accepted` / `missing back behavior rejected` / `placeholder rejected` → **3/3 PASS, exit 0**.
- 계약 검증: **`PASS: valid game UI contract`, error 0건, exit 0**.
- 검증기는 대상 파일과 스킬 디렉터리를 수정하지 않는다.

## 계약 수량 (C4 → C5)
| 항목 | C4 | C5 |
|---|---|---|
| screens | 19 | **19** (화면을 늘리지 않고 상태만 추가) |
| player_decisions | 7 | 7 |
| data_bindings | 9 | **10** (확정 저장 트랜잭션 상태 추가) |
| verification.matrix | 10 | **18** (C4-F5 컨트롤러 겸용 검수 행 추가) |
| verification.acceptance | 5 | **8** |
| decisions | 7 | **13** (R1~R4 결정 6건 추가) |
- `navigation`에 `press_release_latch`·`pointer_optional`, `accessibility`에 `hold_alternative`·`keyboard_only` 키를 추가했다(스키마 필수 키는 그대로).
- 상태 추가 위치: `input-remap`(확정 방식 선택), `load-recovery`(백업/사전 체크포인트/수동 슬롯 분리), `confirm-preview`(두 단계 기본·홀드 옵션), `result-checkpoint`(**저장 진행 중·저장 실패**).

## 해소 ID
| ID | 대상 | 요지 |
|---|---|---|
| `R1` | F1 | `X`=프리뷰 고정, 해제=우클릭/`Delete`/패널 내 `Y`, 키보드 단독 전 경로, 홀드는 옵션(기본 두 단계), 릴리스 래치, 퍼즐 단위 테스트 |
| `R2` | F2 | 내구 후보 트랜잭션, `SavePending`은 중복 확정만 차단, 실패 시 상태·디스크 유지 및 성공 연출 미재생, 파일 4갈래 분리, fsync 후 원자적 rename, 렌더 ack와 디스크 200ms 축 분리 |
| `R3` | F3 | `sourceType` 상이 **AND** 루트 `originId` 상이, `copiedFrom` 루트 해석, 라벨은 증명이 아님, `proofRequired`에만 요구, 임포트에서 누락·순환 실패 |
| `R4` | F4 | T0 = `hub` 1곳 + `circuit`/`reader` 2도구 + 25분, alignment는 별도 그레이박스 스파이크, 본 생산 게이트는 두 증거 모두 요구 |
| `C4-F5` | C4-F5 | `LB` 모디파이어 전용(도구 순환은 `RB` 단독), 증거함·가설판·힌트 3기능 3바인딩(`back` · `LB`+`back` · `LB`+`RS`), 누른 순간 고정 판정, `matrix[17]`(18번째 행) 검수 행 추가. 파생 정정: `system-specs/hint-system.md` L25 · `system-specs/plate-readout.md` L29 |

## 관측과 목표의 구분
| 구분 | 항목 |
|---|---|
| `[OBSERVED]` | Unity `6000.5.6f1` 설치(2026-09-09 23:32 KST), Python 3.13.12, 위 검증 결과·수량·해시 |
| `[TARGET]` | 화면·초점·확정 조건, 홀드 시간 범위, 해상도군 4종, 성능 예산, T0 25분 |
| `NOT-MEASURED` | 프레임·저장 실측, 패드·보조기기 조작, Steam Deck, 완주 시간, 접근성 실사용 |

`verification.evidence`는 여전히 "증거 없음"을 선언한다. matrix **18행**은 **실행 계획**이며 관측 결과가 아니다. 새로 더한 18번째 행도 **미실행**이다(패드 실측 0건).

## 정직성 경계
- 실행 가능한 구현 계약이지 게임 존재 주장이 아니다. 빌드 0건, 플레이 표본 0건.
- G2/G4/G5/G6/G7은 `NOT-MEASURED`이며 이 개정은 어떤 게이트도 올리지 않는다.
- **T0 25분은 480분을 증명하지 않는다.** `observedMedianMinutes`는 `null`이다.
- 패키지 버전은 여전히 `[PIN-AFTER-RESOLVE]`이며 하나도 적지 않았다.
- `systems/prototype/`의 브라우저 참조 모형은 T0 게임이 아니다(규칙 무모순 확인용).
- git commit/push 미실행. 소유 5개 파일 외 수정 0건.
