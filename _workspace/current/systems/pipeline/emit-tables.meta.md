---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
describes: _workspace/current/systems/pipeline/emit-tables.mjs
---

# emit-tables.mjs 메타 — 저작 원본 → 런타임 테이블 파이프라인 (C6-F13)

## 1. 왜 있는가

C6-F13: `beats.md` §1 M1 은 "`beats.json` 은 `campaign.json` 에서 **생성**된다"고만 적고 **생성기·시점·해시 대조**를 정의하지 않았다. 그 공백 때문에 브리프 §④-2 는 Unity 임포터에게 **검증기 47검사를 C# 으로 재구현**하라고 지시하고 있었다. 두 구현이 갈라지는 순간 어느 쪽이 정본인지 말할 수 없다.

이 파일이 그 공백을 메운다. **검증 규칙은 한 곳(`planning/validate-campaign.mjs`, planner 소유)에만 산다.** 이 생성기는 그것을 **호출**하며 규칙을 복제하지 않는다.

## 2. 계약

| 항목 | 값 |
|---|---|
| 소유 | `game-systems-designer` (레인 `systems/`) |
| 입력 | `planning/campaign.json` (planner 소유 · 읽기 전용) |
| 규칙 출처 | `planning/validate-campaign.mjs` (planner 소유 · 호출만) |
| 출력 (`scope=all`) | `beats.json` · `hints.json` · `tables-receipt.json` |
| 출력 (`scope=t0`) | `beats/hints/tools/zones/records.json` + `tables-receipt.json` + **각 `.meta.md`** |
| 기본 동작 | **드라이런**(쓰기 0건). `--out <dir>` 을 줘야 파일이 생긴다 |
| 실패 정책 | **fail-closed** — 검증기 `verdict != PASS` 면 exit 1 이고 **출력 디렉터리를 만들지 않는다** |
| 의존성 | Node 내장 모듈만. 설치 0건 |

### 2.0 스코프 — `all` 과 스테이지 `[C7-F1 신설 2026-09-10 R7]`

RFC-C7-001 (2) 가 T0 **인스턴스 데이터**를 저장소에 만들라고 판정했다. 같은 생성기가 두 층을 낸다.

| scope | 대상 | 내용 |
|---|---|---|
| `all`(기본) | Unity `Assets/_Project/Data/Tables/` | 전 캠페인. `beats.json` 은 저작 원본의 **바이트 동일 사본**, `hints.json` 은 99행 투영. **`.meta.md` 를 만들지 않는다**(브리프 §④-1 의 3파일 계약) |
| `t0`(스테이지 id) | `_workspace/current/systems/data/t0/` | T0 서브셋 5종 + 영수증 + `.meta.md` 6건 |

스코프 결정은 **결정적**이며 영수증의 `scope`/`scopeSource` 로 찍힌다: ① `--scope <값>` 이 있으면 그 값 ② 없고 `--out` 의 마지막 경로 조각이 스테이지 id 모양(`^[a-z]\d$`)이면 그 값 ③ 그 외 `all`. 브리프 §⑩-2 #0b 의 기존 명령은 ③ 으로 떨어지므로 **동작이 바뀌지 않는다**.

### 2.0a `t0` 스코프는 값을 발명하지 않는다

값의 출처와 파싱 대상은 `tech-verification/r7-t0-data.md` §2 표가 소유한다. 요지: **정규식·표 파서로 저작 문서에서 읽고, 읽지 못하면 `exit 2` 로 멈춘다.** 손으로 채우는 경로가 코드에 없다. 염판·조위대장 곡선은 `synopsis/t0-records.md` §5.2 코드 펜스의 **계수를 파싱해 규칙으로 생성**하며, 같은 문서의 앵커 표와 전건 대조해 어긋나면 실패한다(§5.3 33행 · §7.3 28행).

각 필드는 `_src` 로 출처를 갖는다. 다음 회차가 "이 숫자는 어디서 왔나"를 물을 때 파일 자신이 답한다.

### 2.1 `beats.json` 은 바이트 동일 사본이다 (`scope=all` 한정)

`beats.md` M2("필드명을 변환하지 않는다. 변환 계층 0")를 문서 주장이 아니라 **기계 성질**로 만든다: `sha256(beats.json) == sha256(campaign.json)`. 임포터는 이 한 줄로 미러 무결성을 확인한다.

### 2.2 `hints.json` 은 파생 투영이다

저작 원본의 `beats[].hints[3]` 99건을 `hints.md` §1 필드 모양으로 편다. 문자열은 `sourceTextKo` 로 옮기고 **로컬라이즈 키를 결정론 규칙**(`hint.{beatId}.l{level}`)으로 함께 낸다. **EN 문자열은 저작 원본에 없다** — 따라서 `T-12`(미해결 키 0건)는 번역 테이블이 생기기 전까지 통과할 수 없고, 그 사실을 숨기지 않는다. [OPEN-S7]

## 3. 실행 영수증 [OBSERVED 2026-09-10 · 본 레인이 실제로 실행]

> **R7 종료 갱신**: 아래 표는 `scope=all` 회차의 관측이며 숫자는 그 시점 입력의 값이다. `scope=t0` 회차의 명령·출력 원문은 **`tech-verification/r7-t0-data.md`** 가 보유한다(고정 sha 를 두 문서에 중복 기재하지 않는다 — RFC-Q1).

```
$ node _workspace/current/systems/pipeline/emit-tables.mjs                      # 드라이런
$ node _workspace/current/systems/pipeline/emit-tables.mjs --out <scratch>/tables-test
$ shasum -a 256 _workspace/current/planning/campaign.json <scratch>/tables-test/beats.json
```

| 관측 | 값 |
|---|---|
| 검증기 판정 | `checks 47 / pass 47 / fail 0 / verdict PASS` · exit 0 |
| `beats.json` | `derivation: copy` · 121,457 B · 33행 · **sha256 이 저작 원본과 동일** [OBSERVED, 두 줄이 같은 해시로 출력됨] |
| `hints.json` | `derivation: projection` · 40,724 B · **99행** |
| `tables-receipt.json` | 2,491 B |
| **fail-closed 픽스처** | `t0-b1.zoneId` 를 `lowland`(스테이지 밖)로 바꾼 사본 → 검증기 `Z-01` FAIL → 생성기 **exit 1 · 출력 디렉터리 미생성** [OBSERVED] |

해시 숫자는 저작 원본이 바뀌면 달라진다. 인용할 때는 옮겨 적지 말고 생성기를 다시 돌린다(RFC-Q1).

## 4. 임포터가 재구현하는 것 / 하지 않는 것

영수증의 `importerContract` 가 기계가 읽는 형태로 같은 내용을 담는다.

| 임포터 몫 | 이유 |
|---|---|
| `R-1` 로컬라이즈 미해결 키 0건 | KO/EN 테이블은 저작 JSON 밖에 있다 |
| `R-2` 고아 0건 | Unity 에셋 참조 그래프가 있어야 판정된다 |
| `R-3` 독립쌍 중 **파괴 불가 경로 ≥ 1** | 런타임 규칙(`beats.md` B-I11 의 미검증 절반) |
| `R-4` 임의 도달 상태에서 엔딩 3종 도달 | 런타임 상태공간 탐색(`beats.md` B-I12) |

**임포터가 하지 않는 것**: 저작 시점 검사 전건(현재 47). 영수증 해시 대조로 대신한다.

## 5. 이 파일이 주장하지 않는 것

- Unity 에서 임포트해 본 적 **0회**. `unity/Unknown/Assets/` 에 테이블을 쓴 적 **0회**(이번 회차 출력은 스크래치 디렉터리뿐이며 레인 밖 쓰기 금지를 지켰다).
- 어떤 게이트도 올리지 않는다. 이것은 데이터 정합 도구이지 게임이 아니다.
- `hints.json` 의 `sourceTextKo` 를 런타임이 직접 표시해도 된다는 뜻이 아니다. 표시는 로컬라이즈 테이블 경유가 정본이다(`hints.md` §1 `textKey`).
- **`systems/data/t0/` 는 저작 산출물이지 런타임 자산이 아니다.** Unity 가 읽은 적 0회이며 `ZoneAsset`/`RecordAsset`/`ToolAsset` 변환도 0회다. `viewNodes` 6개·프레이밍·`loadCostMb` 는 n=0 이다.

## 6. 변경 로그 (같은 사이클 제자리 개정 · RFC-Q2)

| 날짜 | 회차 | 결함 | 바뀐 절 | 내용 |
|---|---|---|---|---|
| 2026-09-10 | **R7 종료 수정** | **C7-F1**(S1) · C7-F8 | §2 계약표 · §2.0 · §2.0a 신설 · §3 머리 · §5 · 이 절 | 생성기에 `--scope` 를 더해 T0 인스턴스 데이터 5종 + `.meta.md` 를 내게 했다. `scope=all` 경로는 한 줄도 바뀌지 않았고 그 사실을 드라이런 해시로 확인했다 |
| 2026-09-11 | **RFC-CX-012 ACK-c** | QA `rfc-cx-012-review.md` 부기(systems 재량) · A-03 | L896 `autoCopyCreated._src` 1어 | 출처 인용문 「보관함에 생성」→「증거함에 생성」 — campaign `t0-b3.completion` 정본(ACK-a 교체)과 어긋난 주석 층위 1어. 로직·파싱·스코프 결정 무변경(`node --check` 통과 · 재생성 후 `--t0` 5/5 · 본체 49/49 PASS). 영수증: `systems/rfc-cx-012-ack.md` §2.1 |

- `cycle` 값 불변(RFC-Q2). **새로 측정된 런타임 값 0건.**
