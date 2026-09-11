---
updated: 2026-09-10
cycle: 20260909-preproduction-c4
status: current
supersedes: null
owner: game-systems-designer
---

# 기술 검증 — C4 수정 루프 2 (C4-F19 · 입력 바인딩 겸용 3번째 재발)

대상 결함: **C4-F19 (S2, systems)** 1건. 수정 파일 **3개**(스펙 1 · 규칙 1 · 메타 1). 이 문서는 **문서 정합 재측정 영수증**이다.
**Unity 실행·빌드 0회 · 패드 실측 0건 · 사람 플레이 표본 n=0.** 여기의 어떤 값도 게임 측정치가 아니며, 어떤 게이트도 PASS 로 올리지 않는다.

## 0. 결함이 주장한 것과 실제 대조 결과

| # | QA 주장 (`defect-register.md` C4-F19) | 파일 재열람 결과 | 판정 |
|---|---|---|---|
| 1 | `drainage-routing.md` L29 「연결 해제 \| 우클릭 \| `X`」 | 문자 일치 (수정 전) | **인정** |
| 2 | `interaction-rules.md` L41 「`X` 단독은 모든 화면에서 프리뷰 전용 … 배선 해제는 `X`를 쓰지 않는다」와 정면 충돌 | L41 문자 일치. §1 정본 행 = KB `Delete` / 패드 **패널 안에서만 `Y`**, §2.4 = 「해제는 우클릭 / `Delete` / 패널 내 `Y`」 | **인정** |
| 3 | 같은 파일 L31(가상 시험 `X`)과 **한 표 안 겸용** = §0-11 위반 | L31 문자 일치. 같은 표에서 `X` 가 두 명령(`RemoveEdge` · `Preview`)을 발행 | **인정** |
| 4 | 같은 행 KB 열이 "우클릭"뿐이라 그 표만 읽으면 §0-8 키보드 경로가 없다 | 문자 일치 | **인정** |
| 5 | 파일이 `status: current` 이므로 `interaction-rules.md` 승격 시 current↔current 모순 | frontmatter 확인: 스펙 = `current`, 규칙 = `draft` | **인정** (승격 전에 닫는 것이 맞다) |

**반론 0건.** 요구된 수정(패드 `X` → `Y`(패널 안) · KB 열에 `Delete` · 가상 시험 행은 `X` 유지)을 그대로 이행했다.

## 1. 수정 전/후 (명령과 원문)

```
$ sed -n '29p' _workspace/current/systems/system-specs/drainage-routing.md
```

```
수정 전:  | 연결 해제 | 우클릭                        | `X`                   | `RemoveEdge` |
수정 후:  | 연결 해제 | 우클릭 / `Delete`(키보드 단독) | **패널 안에서만 `Y`** | `RemoveEdge` |
```

- 행 번호 **L29 불변**(제자리 치환) — QA 가 인용한 좌표가 다음 회차에도 그대로 선다.
- `RemoveEdge` 의 **이름·인자·의미 불변** → §2 상태기계 · §3 규칙 · §4 실패 모드 · §5 스키마 참조 · §6 텔레메트리 · §7 예산 · §8 인수 기준 **한 글자도 바뀌지 않았다**. 저장 필드 변경 0(CLAUDE.md §9 세이브 불변식 비관여).
- 「가상 시험 \| `Space` \| `X`」 행은 **유지**. 프리뷰 계열이므로 §1-2 「`X` = 프리뷰」와 같은 의미다 `[INFERENCE]` — 표면 우선순위 규칙(C4-F16)이 서기 전까지 이 판단은 측정이 아니다.

## 2. 재측정 명령과 결과 [OBSERVED 2026-09-10]

| # | 명령 | 결과 |
|---|---|---|
| W-1 | `for f in system-specs/*.md; do awk '/^## 1\. 입력/{ins=1;next} ins && /^## /{ins=0} ins && /^\|/ && …' "$f"; done \| wc -l` | **41행 / 8파일** (corrosion 4 · drainage 6 · dual-seal 6 · hint 4 · plate 5 · save-undo 5 · tide 6 · wiring 5) — C4-F21 의 41 과 일치. 루프 1 의 "6개 도구 스펙 전건 대조" 문장은 **파일 수부터 틀렸다** |
| W-2 | 같은 출력에서 패드 열 필터 — **문서가 적어야 하는 명령은 `awk -F'|' '$3 ~ /X/ && $3 !~ /LB\+X/'` 이다** `[2026-09-10 R6 · C4-F22 정정]` | 조건 없이 `$3 ~ /X/` 만 쓰면 **8행**이다(단독 `X` 6행 + `LB+X` 2행 — `dual-seal` L31 해제 · `save-undo` L32 되돌림/다시). 앞선 판은 조건을 적지 않고 결과만 「6행」이라 적어 **재현되지 않았다**. 단독 6행은 — `dual-seal` L29 프리뷰 · `drainage-routing` L31 가상 시험 · `corrosion-budget` L48 가상 시험 실행 · `tide-alignment` L29 자동 최소자승 제안 · `wiring-trace` L30 구획 접기 · `plate-readout` L31 재생. **수정 전에는 7행**이었고 7번째가 `drainage-routing` L29(배선 해제)였다 |
| W-3 | 같은 출력에서 패드 열에 `` `Y` `` 가 있는 행만 필터 | **3행** — `drainage-routing` L29(신규, 연결 해제) · L33(`LB+Y` 연습 시작) · `plate-readout` L32(인용 고정, **C4-F16 OPEN**) |
| W-4 | `grep -n "해제" _workspace/current/systems/game-ui-contract.json` | L153 「패널이 열린 동안 X는 프리뷰 Y는 해제로 고정된다」 · L528 · L620 — **계약 JSON 은 이미 정본 쪽**이었다. C4-F19 는 md 단독 스테일이며 이 수정으로 md↔json 드리프트가 닫힌다 |
| W-5 | `shasum -a 256` + `wc -c` + `LC_ALL=en_US.UTF-8 wc -m` (4파일 + 스펙 1) | `interaction-rules.md` `4f6b2546…` · 28,808 B · 15,975자 (루프 1: `3c901743…` · 25,545 B) · `drainage-routing.md` `389834d3…` · 11,165 B · 7,077자 · **`game-ui-contract.json` `9c89e9ae…` 불변** · `unity-implementation.md` `453f8481…` 불변 · `animation-contract.md` `ec3ad8c1…` 불변 |
| W-6 | `bash .claude/skills/game-ops-harness/scripts/freshness-check.sh` | `0 finding(s)` · exit `0` (아티팩트 수는 본 파일 생성 후 재측정 — §5) |
| W-7 | `node _workspace/current/planning/validate-campaign.mjs` | `checks 47 · pass 47 · fail 0 · verdict PASS`, live `campaign.json` sha **`92301c0a5ecfc7e142646f5f17c29c21609e2b2de7c90408de7bffc9d815ae23`** · 121,457 B, exit 0 — **입력 정본 불변**(이번 수정은 JSON 을 건드리지 않는다) |
| W-8 | `node _workspace/current/systems/prototype/test-model.mjs` | **37 통과 / 0 실패** — 코드 0줄 변경의 대조 영수증 |

## 3. 같은 편집에서 함께 고친 것 (자기 보고)

| 위치 | 무엇 | 왜 같은 편집인가 |
|---|---|---|
| `interaction-rules.md` §1-2 대조 절 도입 문장 | "6개 도구 스펙의 §1 입력표와 **전건 대조**했다" → **8파일 41행**을 세는 **명령**과, 그 대조가 L29 를 놓쳐 C4-F19 가 됐다는 자기 고백으로 교체 | 그 문장이 **거짓 `[OBSERVED]`** 였다. 결함만 고치고 문장을 두면 "전건 대조했다"가 다음 회차에도 근거로 인용된다 |
| 같은 절 대조 표 | `drainage-routing.md` **L29 행 신설**(정정함) | 대조 표에 그 행이 없던 것이 누락의 형태였다 |
| 같은 절 | 「`X` 잔여 전수」 6행 목록 추가 | C4-F19 가 닫는 것이 **1행**이고 C4-F16 이 여전히 **3행**(자동 제안·구획 접기·재생)이라는 사실을 수치로 남긴다. 닫힌 것처럼 보이게 하지 않기 위해서다 |
| 같은 절 전건 표 `Y` 행 | 모디파이어 열 `—` → `` `LB`+`Y` = 연습 시작(Fork) `` | 같은 절의 대조 표가 `drainage-routing.md` L33 `LB+Y` 를 **이미 「일치」로 판정**하면서 전건 표에는 "없음"으로 적혀 있었다 — 표가 자기 대조 결과와 어긋나 있었다. **새 바인딩을 만든 것이 아니라 있는 것을 적었다**; 스펙 파일 변경 0 |
| `game-ui-contract.meta.md` 해시 표 | `interaction-rules.md` 행 재측정 교체 + 루프 2 노트 + **측정 로케일 명시** | C4-F13 의 교훈(편집과 해시 갱신을 같은 편집에 묶는다). `wc -m` 은 기본 로케일에서 바이트와 같은 값을 내므로 「문자 수」 열은 `LC_ALL=en_US.UTF-8` 없이는 재현되지 않는다 |

## 4. 이 수정이 **닫지 않은** 것 [OPEN]

| 결함 | 남은 상태 | 왜 이번에 손대지 않았는가 |
|---|---|---|
| **C4-F16** | 도구 패널 표면의 `X`·`Y`·`Space` 우선순위 규칙 부재 — `tide-alignment` L29 · `wiring-trace` L30 · `plate-readout` L31/L32 **4행** | 규칙 1절 신설은 6스펙을 동시에 건드리는 설계 변경이며 이번 배정 밖. C4-F19 는 그중 **정면 충돌 1행**만 닫는다 |
| **C4-F20** | `dual-seal` L30 · `tide-alignment` L30 「확정 `Enter` 길게 0.4 s」 ↔ §0-9(홀드는 opt-in) · §1-2 `RB` 롱프레스 | 지속시간 레이어는 §0-11 개정 사안(디렉터 판정 필요). **`drainage-routing.md` L32 는 이미 `two-step` 기본**이라 본보기로 남는다 |
| **C4-F21** | `drainage-routing` §1 의 KB 미기재 행 — 이번 수정으로 3행 → **2행**(노드 연결 드래그 · 밸브 토글 클릭) | 나머지 2행은 §1 「커넥터 연결 = 2단계 선택」·「대상 조사 = 초점 후 `Enter`」 로 파생되지만 **행별 파생 근거를 스펙에 적는 것**이 요구사항이며 배정 밖 |
| **C5-F5 잔존** | `drainage-routing.md` §3 R-R2 · §4 R-F4 의 폐기 용어 「매체 경로」 2곳 | 이 루프에서는 배정 밖이었다. **2026-09-10 R6(C6/C7)에서 해소** — systems 소유 6곳 + 미지목 1곳(`wiring-trace` W-F4)을 정본 표현으로 교체했다(`tech-verification/c6-c7-systems-receipt.md`) |

## 5. 신선도 · 산출물 수

```
$ bash .claude/skills/game-ops-harness/scripts/freshness-check.sh
```
- 수정 전: `0 finding(s) across 95 markdown artifact(s)` · exit `0`.
- 본 파일 생성 후: `0 finding(s) across **96** markdown artifact(s)` · exit `0` [OBSERVED 2026-09-10]. 증가분 1 = **이 검증 파일**이며 삭제·이동 0건.
- **다른 레인 파일 쓰기 0건의 근거 `[2026-09-10 R6 · C4-F22 정정]`**: 앞선 판은 이 결론의 근거로 `git status --short` 를 들었으나, 그 명령은 미추적 디렉터리를 **`?? _workspace/current/systems/` 한 줄**로 접어 내므로 파일 단위 쓰기를 보여주지 못한다. 재현 가능한 근거는 **편집 전후 `shasum -a 256` 비교**다 — 편집 대상 3파일만 해시가 바뀌고 다른 레인 파일의 해시는 불변임을 보이는 방식이며, W-5 가 실제로 그렇게 측정했다(`unity-implementation.md` · `animation-contract.md` 불변 확인). 결론은 바뀌지 않고 **근거 문장만 교체**한다.

## 6. 이 문서가 주장하지 않는 것

- 패드로 `Y` 를 눌러 배선이 실제로 끊기는 것을 **본 적이 없다.** 입력 계약은 문서이며, 검수 시나리오 `game-ui-contract.json` `verification.matrix[12]`(`X`/`Y` 축)와 `matrix[17]`(`LB` 축)은 **둘 다 미실행**이다.
- 「프리뷰 계열이라 충돌이 아니다」는 `[INFERENCE]` 다. 표면 우선순위 규칙(C4-F16)이 서야 `[OBSERVED]` 가 될 수 있다.
- G6(운영 안정성)·G7(피처 수용) 어느 쪽도 이 수정으로 움직이지 않는다.
