---
updated: 2026-09-10
cycle: 20260909-preproduction-c4
status: draft
supersedes: null
owner: game-qa
---

# C4 독립 검토 — game-qa

- 검토일: 2026-09-09 / cycle `20260909-preproduction-c4`
- 읽은 것(전부): `systems/{unity-implementation.md, interaction-rules.md, game-ui-contract.json}`, `balance/puzzle-balance.md`, `economy/resources-and-fairness.md`, `concept/art-direction.md`, `modeling/asset-budget.md`, `animation/animation-contract.md`, `motion/motion-contract.md`, `vfx/vfx-budget.md`
- 읽지 않은 것: campaign / worldview (타 에이전트 개정 중)
- **런타임 증거 0건은 결함으로 세지 않았다.** 빌드·플레이·프로파일 부재는 세 문서가 명시적으로 `NOT-MEASURED`로 선언했고 그 선언은 정직하다. 아래 4건은 전부 **문서 간 계약이 서로 모순되어 구현 전에 결정이 필요한 항목**이다.

## 판정: **FIX** (material 4건 / blocker 2건)

---

## F1 — 입력맵 충돌 + 길게 누름 대안 부재 + 키보드 단독 경로 없음 (blocker)

**증거**
- `interaction-rules.md:29` 프리뷰 = 컨트롤러 **`X`** / `interaction-rules.md:56` routing 입력 = "드래그로 배선, 우클릭/**`X`**로 해제". 두 조작은 `game-ui-contract.json` `screens[9] tool-panel` 상태 `경로 구성`에서 **동시에 활성**이다(같은 화면, 같은 버튼, 서로 다른 결과).
- `interaction-rules.md:30,34` 확정 = `Enter`/`A` **길게 누름 0.4s**, "확정만 길게 누름이다" / `screens[13].initial_focus` "확정은 길게 누름을 요구한다". 그런데 `navigation.destructive_behavior`는 "길게 누름 **또는 별도 확인**"이라고 선택지를 선언한다 — **"별도 확인" 경로를 구현한 화면·상태·설정이 한 곳도 없다.**
- `accessibility{}` 키는 `signals / settings_entry / motion / media_alternatives` 4개뿐. **길게 누름 대체·홀드 시간 조절 항목 없음.** `screens[3] input-remap`도 "각 행동의 키와 패드 버튼을 다시 지정"만 하고 홀드 시간은 재매핑 대상이 아니다.
- `interaction-rules.md:26` 대상 조사 KB = **좌클릭만**, `:28` 미세 조절 KB = 드래그/`Shift`/휠(전부 포인터). 반면 `navigation.methods[1]`은 "키보드 방향키와 탭 순회"를 **독립 조작 방식으로 선언**한다.

**왜 material인가**: 확정은 `interaction-rules.md:17`(전역 불변식 4)·`:84~86`에 따라 **모든 진행의 유일한 관문**이다. 0.4s 홀드를 유지할 수 없는 플레이어는 대안이 없어 게임을 끝낼 수 없는데, `verification.acceptance[0]`("모든 화면이 두 입력 방식으로 도달·이탈 가능")은 **화면 도달만 보므로 통과한다** — 인수 기준이 이 실패를 구조적으로 못 잡는다.

**수정**
1. `interaction-rules.md:56` routing 해제를 `X`가 아닌 버튼으로 이동(예: 컨트롤러 `Y` 단독 또는 `LB+A`), KB 해제 키도 신설(현재 우클릭 전용).
2. `accessibility{}`에 `hold_alternative` 추가: "길게 누름 → 확인 대화 1회" 토글 + 홀드 시간 0.2~1.5s 슬라이더, `screens[3]`의 재매핑 대상에 포함.
3. `interaction-rules.md:26,28`에 키보드 바인딩 신설(조사=초점 대상에 `Enter`, 미세 조절=방향키 1스텝 / `Shift`+방향키 정밀).
4. `verification.matrix`에 2행 추가: (a) `input: 키보드 단독` × `state: 작업대 도구 조작`, (b) `setting: 길게 누름 대체 켜짐` × `state: 확정 프리뷰`.

**플레이 영향**: 홀드 대안 부재 = 해당 플레이어 기준 **480분 전량 진행 불가**. `X` 충돌은 routing 도구가 쓰이는 제3수문·양수장 2구역(`art-direction.md:20`, 양수장 행)에서 프리뷰를 누를 때마다 배선이 지워지는 오조작을 유발한다(되돌림으로 회복되지만 확정 리듬이 매번 깨진다).

---

## F2 — 확정 트랜잭션의 저장 실패 경로 없음 + 체크포인트가 저장 아티팩트로 정의되지 않음 (blocker)

**증거**
- `unity-implementation.md:89` 복구 순서는 **3단계**: `save.json` → `save.bak` → **마지막 자동 체크포인트**. `screens[5] load-recovery`도 이를 독립 경로로 취급한다(상태: "백업 사용 가능" / "체크포인트만 사용 가능").
- 그런데 `unity-implementation.md` §7 저장 스키마 v1에 **체크포인트 파일·경로·개수 필드가 없고**, `interaction-rules.md:92`는 "수동 저장 3슬롯 + **자동 1슬롯**"으로 못 박는다. 자동 슬롯이 1개면 "마지막 자동 체크포인트"는 `save.json` 자신이며 **3단계 복구는 사실상 2단계로 붕괴**한다.
- 커밋 시점 쓰기 실패 경로가 없다: `screens[14] result-checkpoint` entry는 "확정 직후 자동 진입", 상태 3개(`구역 상태 변경`/`보호 선택 반영`/`필수 단서 보존 표시`)가 **전부 성공 상태**다. `저장 중`·`저장 실패` 상태 없음.
- `animation-contract.md` 도장 확인 행의 진실 소유자는 "**save receipt 이후 UI**" — 즉 저장 영수증을 기다리는 대기 상태가 필요한데 그 상태를 선언한 화면이 없다. 같은 문서는 "입력 잠금은 확정 트랜잭션 **최대 1프레임**"이라 하고, `unity-implementation.md` §9는 저장 쓰기 목표를 **200ms**로 잡는다(16.7ms 프레임 예산의 12배).
- 테스트 T-08/T-09/T-10은 **전부 로드 측**이다. 쓰기 실패 주입 테스트 0건.

**왜 material인가**: `Reduce`가 적용된 뒤 rename 전에 실패하면 **메모리 상태는 전진했는데 디스크는 이전**이고, 플레이어는 성공 결과 패널을 본다. `interaction-rules.md:86`이 약속한 체크포인트 라벨을 신뢰한 상태에서 재시작하면 그 확정이 통째로 사라진다.

**수정**
1. 체크포인트를 링 버퍼 파일로 명시(예: `checkpoint.0~2.json`), 저장 스키마에 `checkpointRefs[]` 추가 → `:89` 3단계 복구가 실제로 3개 파일을 갖게 한다. 또는 `interaction-rules.md:92`의 "자동 1슬롯"을 "자동 1슬롯 + 체크포인트 링 3"으로 정정.
2. `screens[14]`에 상태 2개 추가: `저장 중(영수증 대기)`, `저장 실패(재시도/무시 없이 이전 상태 유지)`.
3. 인수 테스트 **T-15** 신설: "쓰기 실패를 rename 직전에 주입하면 이벤트 로그와 UI가 확정 이전 상태로 롤백되고 `save.json` 바이트가 변하지 않는다."
4. `animation-contract`의 "1프레임 입력 잠금"과 §9의 "200ms 저장" 중 하나를 정정 — 확정 커밋을 비동기로 두고 도장 애니는 영수증 도착 시 재생, 실패 시 미재생으로 계약을 맞춘다.

**플레이 영향**: 자동 저장 지점이 `interaction-rules.md:91`상 "장 경계·구역 이동·**모든 확정 직전**"이므로 480분 플레이에서 수백 회 발생하는 경로다. 확률이 낮아도 발생 시 손실 단위가 "직전 확정 1건 + 그 사이 조사 전량"이다.

---

## F3 — 근거 독립성이 "매체 종류"와 "출처" 사이에서 갈린다 (게임 핵심 규칙)

**증거** — 같은 문단 안에서 기준이 두 번 바뀐다.
- `interaction-rules.md:71` 문장1: "독립은 **매체 종류가 다를 때** 성립한다." 문장2: "염판 원본, 그 표면 부식, 복제 스캔은 모두 같은 **출처 1개**로 센다." → 판정 기준이 *종류*에서 *출처*로 이동.
- 실제로 강제되는 검사는 **종류 전용**이다: `interaction-rules.md:67` seal은 "매체 종류 뱃지(**염판/일지/대장**) … 같은 종류가 두 번 들어가면 두 번째 슬롯이 즉시 회색" — 3값 enum 비교. `game-ui-contract.json` `data_bindings[3] 근거 슬롯 독립성 배지`의 `source`도 "가설 그래프의 **매체 종류 판정**".
- 세 번째 용어가 또 있다: `unity-implementation.md:68` 불변식 1은 "겹치지 않는 **매체 경로** 2개 이상". `경로`는 어디에도 정의돼 있지 않다.
- 테스트가 이 구멍을 덮지 못한다: `T-11`은 "불변식 위반 픽스처 4종"인데 §5 불변식 4개 중 **출처 동일성을 인코딩한 것이 없고**, `verification.matrix[3]`도 "같은 매체 두 번 투입"(=종류)만 본다.

**왜 material인가**: 두 방향 모두 게임을 깬다. (a) 파생 스캔에 별도 종류 뱃지가 붙으면 → **원본 + 그 원본의 스캔**으로 서명이 열려 이중서명의 존재 이유가 사라진다. (b) 스캔이 `염판` 뱃지를 물려받으면 → **다른 관측소의 정당한 두 번째 염판**이 잘못 거부된다. (b)는 `T-07` 소프트락 검사(다른 명령이 남아 있으면 통과)로 절대 안 잡힌다.

**수정**
1. 자료 스키마에 `mediumType`(염판/일지/대장)과 **`originId`(물리 출처 식별자)** 를 분리 저장하고, 독립 판정을 `originId 상이 AND mediumType 상이`로 한 문장으로 확정 — `:71`의 두 문장 중 하나는 삭제.
2. `unity-implementation.md:68`의 "매체 경로"를 `originId`로 치환(용어 3개 → 1개).
3. `T-11` 픽스처에 "원본 + 파생 사본 쌍" 추가, `verification.matrix`에 "원본+파생 스캔 투입 → 거부 사유 문장 표시" 행 추가.
4. seal 슬롯 뱃지에 종류와 출처를 **둘 다** 표기(`:67` 갱신).

**플레이 영향**: seal은 모든 결론의 확정 관문(`:68`)이고 결말 3종의 근거 서사가 여기에 걸려 있다. (a)면 플레이어가 단일 물증으로 전 챕터를 통과해 추리 게임의 명제가 무너지고, (b)면 정당한 근거를 든 플레이어가 확정 화면 앞에서 사유 없이 막힌다(힌트 3단계도 §4상 "정답 값"을 주지 못해 탈출 불가).

---

## F4 — T0 범위 · 에셋 수량 · 공간별 기능 3자 불일치

**증거**
- `unity-implementation.md:110` T0 포함: **`hub` + `gate`(셸 2개)**, 도구 **`reader`/`alignment`/`seal`(3종)**.
- `modeling/asset-budget.md:24` 단계별 신규 자산: **`T0: 셸1·도구2·초상1`** → 셸 1개, 도구 2개.
  → **셸 −1, 도구 −1 부족.** 같은 표의 자체 단가(`:15` 셸 5개=25인일 → 5인일/개, `:16` 도구 6개=18인일 → 3인일/개)로 환산하면 T0 모델링 견적이 약 12.6인일 → 20.6인일, **+63% 과소 산정**이다(총계 셸5/도구6은 후속 단계가 흡수해 그대로라 표만 보면 오류가 보이지 않는다).
- 더 근본적인 불일치: `art-direction.md:20`은 **제3수문(`gate`)의 플레이 기능을 "선 연결·압력"** = `routing`으로 규정하는데, `unity-implementation.md:110`은 T0에서 routing을 **제외**한다. 반대로 T0에 들어간 `alignment`(조위정합)의 홈 공간은 `art-direction.md:22`상 **부두(`dock`)** 이고 부두는 T0에 없다. 즉 **T0는 자기가 쓰지 않을 공간을 만들고, 쓸 도구의 공간은 만들지 않는다.**
- 파생 결함: seal 확정 조건(`:68`)은 "각 근거가 **배선 범위 안**"을 요구하고, 배선 범위는 circuit이 산출한다(`:42` "다른 도구의 확정에서 배선 범위 밖 근거는 자동 무효"). circuit은 T0 제외 목록에 있다 → **T0의 seal은 T0에 없는 도구의 술어에 의존**한다.

**수정**
1. `asset-budget.md:24` T0 행을 **`셸2·도구3·초상1`** 로 정정하고 C1~C6 행에서 각 1개씩 차감(합계 셸5/도구6/초상5 유지).
2. T0 공간을 `hub` + **`dock`** 으로 바꾸거나(= alignment 홈 공간 확보), `gate`를 유지하려면 routing을 T0 포함으로 올린다. 셋 중 하나를 `:110`에 명시.
3. circuit을 "판독 전용·확대뷰 없음"으로 T0에 얇게 포함하거나, `:68` seal 조건의 배선 항목을 **T0 한정 데이터 고정값**으로 대체한다고 명시하고 이를 `balance/puzzle-balance.md`의 T0 실측 프로토콜에 **알려진 이탈**로 기록.

**플레이 영향**: T0는 `:110`에 따라 **"480분 전체 생산은 사람 실측(최소 12명/5유형) 이후에만 게이트를 넘는다"** 의 그 게이트다. 지금 계약대로 만들면 T0 플레이어는 `gate`에서 routing 없이 서 있고, seal의 확정 조건 절반이 스텁이라 `puzzle-balance.md`의 T0 목표(`첫조작≤60초`, `목표설명 10/12명`)가 **실제 게임이 아닌 축소판을 측정**하게 된다. 즉 게이트 통과 여부와 무관하게 그 측정치가 480분 생산 결정의 근거가 되지 못한다.

---

## 결함으로 올리지 않은 것 (판단 근거 기록)

- **성능 수치 전량**: `unity-implementation.md` §9 5개 항목, `asset-budget.md:21`(≤300k tri/≤150 draw/≤512MiB), `vfx-budget.md`(emitter≤8, 투명입자≤500, GPU시간 null), `motion-contract.md`(650/220/180/140ms) — 전부 `[TARGET]`·`NOT-MEASURED`로 정확히 라벨돼 있다. **기준 하드웨어 미정 상태에서 통과/실패를 말할 수 없다는 문서 자신의 선언이 옳다.** 다만 F2의 "1프레임 잠금 vs 200ms 저장"처럼 **두 문서가 서로 다른 수치를 전제하는 경우**만 결함으로 올렸다.
- **패키지 버전 `[PIN-AFTER-RESOLVE]`**: 발명 금지 원칙이 지켜진 것으로 본다.
- **에셋 수량 총계**: 셸5 / 도구6 / 초상5×3 / UI프레임1은 `art-direction.md`(공간 5·인물 5·6도구 실루엣시트), `unity-implementation.md:8절`(hub/gate/lowland/dock/pump), `screens[9]`(도구 6상태)와 **전부 일치**한다. 불일치는 총계가 아니라 F4의 **단계 배분**에만 있다.
- **경제·공정성**: `economy/resources-and-fairness.md`의 무과금·무그라인드 선언과 `interaction-rules.md` §4 힌트 무료·무제한, `data_bindings[4]`의 "예산은 게임 내 제약이며 결제와 연결되지 않는다"가 서로 모순 없이 맞물린다.

## 다음 소유자

`game-systems-designer` (F1·F2·F3 계약 정정) → `game-modeler` + `game-concept-artist` (F4 단계 배분) → 정정 후 `game-qa` 재검토. **F1·F2는 T0 착수 전 해소 권장**(둘 다 T0 포함 항목인 "확정 프리뷰·체크포인트 / 저장·로드·손상 복구 / KB+패드"에 직접 걸린다).

---

# 재검증 (2026-09-10, R4)

> 같은 사이클 안의 제자리 개정이다(RFC-Q2) — `cycle`·`supersedes`·`status` 는 그대로 두고 이 절만 append 했다. 위 1차 본문은 **당시 기록으로 보존**한다(CLAUDE.md §2 삭제 금지).
> 정본 우선순위: 디렉터 RFC > 세션 P 명시 결정 > live `planning/campaign.json` > `current/worldview/*` > 기타. 인용 키는 campaign id.
> **런타임 증거는 이번에도 0건이다.** Unity 실행 0회 · 빌드 0줄 · 플레이 표본 n=0 · 프레임/저장 캡처 0건. 아래 판정은 전부 **문서·데이터 대조**이며 어떤 게이트도 PASS 로 올리지 않는다.

## R4.0 재측정 명령 원문 [OBSERVED 2026-09-10 05:0x KST]

QA 가 직접 실행했다. `systems/tech-verification/c4-self-check.md` §1 의 레인 자체 확인(V1~V8)을 **믿지 않고 다시 돌린 것**이며, 아래 Q# 가 QA 측 원문이다.

| # | 명령 | 결과 [OBSERVED] | 레인 주장과 대조 |
|---|---|---|---|
| Q1 | `shasum -a 256 systems/game-ui-contract.json` · `wc -c` | `83b92c822383ca2dc464a03664b8177b11e54b46f63b81869d74aaf598417730` / **37,990 B** | `game-ui-contract.meta.md` L21 · V3 과 **문자 일치** |
| Q2 | `python3 -c "json.load(...)"` + 구조 집계 | `screens 19` · `accessibility` 키 **6종**(`signals·settings_entry·hold_alternative·keyboard_only·motion·media_alternatives`) · `verification.matrix` **17행** · `acceptance` **8항** · `data_bindings` **10** · `decisions` **13** | V2·V4 와 일치 |
| Q3 | `node planning/validate-campaign.mjs` | `{checks 47, pass 47, fail 0, verdict PASS}` · sha `92301c0a5ecfc7e142646f5f17c29c21609e2b2de7c90408de7bffc9d815ae23` · **121,457 B** · clues **73** · `sourceTypeDist {log 27, ledger 22, plate 24}` · `stageMinutes [25,50,55,65,65,70,75,65,10] = 480` | V5 · `interaction-rules.md` L122 와 일치 |
| Q4 | `node -e` T0 판독 | `T0.minutes 25` · `zoneIds ["hub"]` · `t0-b1 tools []` · `t0-b2 ["circuit"]` · `t0-b3 ["reader","circuit"]` · 3비트 전부 `zoneId hub` | V6 과 일치 |
| Q5 | `node -e` 구역·도구 전수 | zone id 5종 `dock gate hub lowland pump` · tool id 6종 `alignment circuit corrosion reader routing seal` | — |
| Q6 | `cd systems/prototype && node test-model.mjs` | **37 통과 / 0 실패** | V8 과 일치 |
| Q7 | `shasum -a 256` × 4 (`game-ui-contract.json` · `unity-implementation.md` · `interaction-rules.md` · `animation/animation-contract.md`) | `83b92c82…` / `453f8481…`(7,909 chars) / **`98ad78d6…`(9,131 chars)** / `ec3ad8c1…`(1,535 chars) | **`interaction-rules.md` 만 meta L26 기록(`af906dfb…` · 6,236 chars)과 불일치** → C4-F13 |
| Q8 | `shasum -a 256` × 4 (`prototype/{model,test-model,build-prototype}.mjs`, `README.md`) | `53a9d3a4…` · `c8a133cf…` · `cfa2437e…` · `89a7ff4b…` | `prototype.meta.md` L29~32 와 **4/4 문자 일치** |
| Q9 | `bash .claude/skills/game-ops-harness/scripts/freshness-check.sh` | `0 finding(s) across 92 markdown artifact(s)` · **exit 0** | 재검증 3(91) → **92** (아티팩트 1건 증가) |
| Q10 | `grep -n "\`X\`\|\`Y\`" systems/interaction-rules.md` | X = L34(프리뷰)·L40(전용 선언) 뿐 · Y = L30(패널 밖 도구 휠)·L33(패널 안 해제)·L41(모드 분리)·L73 | **X/Y 동시 활성 0건** |
| Q11 | `grep -n "길게 누름" systems/game-ui-contract.json` | 10행 전부 "옵션·선택 기능·의무 아님" 문맥. **"확정은 길게 누름을 요구한다" 0행** | meta L23 개정 확인 |
| Q12 | `grep -c "^\| <term> " worldview/glossary.md` × 12 | `배선 추적 0` · `배수 편성 0` · `부식 시험 0` · `경로 구성 0` · `판독대 0` · `조위관측판 0` · `성에선 0` / `회로 지도 1` · `판독기 1` · `정합기 1` · `편성기 1` · `부식예산 1` | → C4-F9 · C4-F15 |
| Q13 | `grep -c "볼륨\|색약\|색각\|이산" systems/game-ui-contract.json` | `볼륨 0` · `색약 0` · `색각 1`(matrix 설정값 1회) · `이산 0` | → C4-F12 |
| Q14 | `find assets/generated/2d -name "*.png" \| wc -l` | **45** · provenance 6종 | `asset-manifest.md` §0 "`concept/sheets/` 파일 수 = 0" 과 모순 → C4-F10 |

## R4.1 F1~F4 판정

| id | 1차 판정 | R4 판정 | 근거 (실물 위치) |
|---|---|---|---|
| **F1** 입력맵 충돌 · 홀드 강제 · 키보드 단독 부재 | blocker | **closed** | 아래 §R4.1.1 |
| **F2** 저장 실패 경로 부재 · 체크포인트 미정의 | blocker | **closed** | 아래 §R4.1.2 |
| **F3** 독립성이 매체/출처로 갈림 | material | **closed** | 아래 §R4.1.3 |
| **F4** T0 범위 · 에셋 수량 · 공간 기능 3자 불일치 | material | **closed** | 아래 §R4.1.4 |

**열린 S1 = 0** 이 C4 에서도 유지된다. 다만 아래 §R4.2 의 신규 8건(S2)이 열려 있어 **문서 수준에서도 PASS 가능한 게이트는 0개**다.

### R4.1.1 F1 — closed

- **X/Y 충돌 0건 [OBSERVED, Q10]**: `interaction-rules.md` L34 `X`=프리뷰 · L40 "`X` 는 모든 화면에서 프리뷰 전용, 배선 해제는 `X` 를 쓰지 않는다" · L33 해제 = 우클릭 / `Delete` / **패널 안에서만 `Y`** · L41 "`Y` 모드 분리 … 두 의미가 동시에 활성인 상태는 없다". `game-ui-contract.json` `screens[9].decision` 이 같은 문장을 담고 `matrix[12]` 가 그것을 검사 행으로 세웠다.
- **홀드는 옵션이다 [OBSERVED, Q11]**: §0 불변식 **9번**(문서 전체의 전제 자리)에 "길게 누름은 선택 기능", §1-1 세 값(`two-step` 기본 / `hold` opt-in / `confirm-dialog`), `accessibility.hold_alternative`("의무가 아니다 … 0.2~1.5초 조절 … 확인 대화 방식도"), `screens[3].states` "확정 방식 세 가지 중 선택", `navigation.destructive_behavior` 가 "기본값 = 두 단계"로 정정됐다. 1차가 지적한 **"별도 확인 경로가 구현된 화면이 없다"는 소멸**했다.
- **키보드 단독 경로 [OBSERVED]**: §1 표에 「키보드 단독(항상 성립)」 열이 **전 10행에 존재** — 조사 `Enter`, 미세 조절 `방향키 1스텝 / Shift+방향키`, 커넥터 연결 **2단계 `Enter`**, 해제 `Delete`. §0 불변식 8번이 이를 전역 불변식으로 올렸다.
- **인수 기준의 구멍이 닫혔다**: 1차의 핵심 지적은 "`acceptance[0]` 이 화면 도달만 보므로 이 실패를 구조적으로 못 잡는다"였다. 지표를 바꿔치기하지 않고 **늘렸다** — `acceptance[5]`("퍼즐 단위 완주가 키보드 단독으로 가능") + `matrix[10]`("**화면 도달이 아니라 퍼즐 종료까지 도달**") + `matrix[11]`(3방식 각각 완주) + `T-24`·`T-25`·`T-26`·`T-27`. 이것이 QA 가 요구한 것보다 넓다.
- **판정 근거의 한계**: 위는 전부 **계약 문장의 존재**다. 홀드를 유지할 수 없는 실제 플레이어가 완주했다는 증거는 **0건**이며 `matrix` 17행·`T-24`~`T-27` 은 **미실행**이다. F1 을 closed 로 하는 것은 "설계 모순이 사라졌다"는 뜻이지 "접근성이 검증됐다"가 아니다.
- **잔여**: §R4.2 C4-F5(컨트롤러 `LB` 이중 의미 · 3기능 2바인딩) · C4-F16(`Space` 우선순위). 둘 다 **키보드 단독 경로를 훼손하지 않으므로 blocker 가 아니다**.

### R4.1.2 F2 — closed

- **체크포인트가 실제 파일이 됐다**: `unity-implementation.md` §7 `"checkpointRefs": ["checkpoint.pre-commit.json"]`, §7·`interaction-rules.md` §6 이 **4갈래 분리**(`save.json` / `checkpoint.pre-commit.json` / `save.bak` / 수동 3슬롯)를 같은 문장으로 반복한다. 1차가 지적한 "수동 3슬롯 + **자동 1슬롯**" 문장은 **현재 파일에 0행**[OBSERVED, `grep -n "슬롯" interaction-rules.md` → L83·L84·L145 3행, 전부 4갈래 서술과 seal 근거 슬롯]. `screens[5].states` 도 백업 / 사전 체크포인트 / 수동 슬롯 / 복구 불가 **4상태**로 갈렸다 — 복구 단계가 실제로 서로 다른 파일을 갖는다.
- **저장 실패가 상태로 존재한다**: `screens[14].states` 에 **「저장 진행 중 영수증 대기」·「저장 실패 재시도 또는 뒤로」**. `data_bindings[9]`(신규) 가 영수증·멱등 키·지연 완료 폐기까지 바인딩으로 못 박았다.
- **1프레임 vs 200ms 모순이 해소됐다**: §9 표가 **축을 나눴다**(`확정 입력 렌더 응답(ack) 1프레임 · 렌더` / `저장 디스크 완료 200 ms · I/O`) + L112 "저장이 프레임 예산 안에 끝나야 한다는 요구는 이 계약에 없다". `animation/animation-contract.md` L24 가 **이전 판의 "1프레임 입력 잠금" 을 명시적으로 폐기**하고 L26 에서 "도장 애니는 렌더 ack 가 아니라 **저장 영수증**에 걸린다"로 다시 썼다. 두 문서가 같은 결론을 말한다.
- **T-15 가 신설됐고 그 이상이다**: `T-15`(rename 직전 실패) 외에 `T-16`(중간 절단) · `T-17`(멱등) · `T-18`(지연 완료) · `T-19`(rename 후 크래시) · `T-20`(`SavePending` 중 부분 입력) **6건**. 1차가 지적한 "쓰기 실패 주입 테스트 0건"이 6건이 됐다.
- **핵심 명제가 불변식이 됐다**: §0-10 `R2` "**확정은 저장이 성공해야 확정이다. 저장 전 화면은 후보(candidate)이며 권위 있는 상태가 아니다**" + `acceptance[6]`. 1차가 "메모리는 전진했는데 디스크는 이전"이라 부른 창은 계약상 닫혔다.
- **잔여**: C4-F6(세이브 v1 필드가 `data-schemas/save.md`(status current)와 다르다 — **`unity-implementation.md` 승격 차단**) · C4-F7(같은 영수증 게이팅이 VFX·모션 계약에 없다).

### R4.1.3 F3 — closed

- **판정이 한 문장이 됐다**: `interaction-rules.md` §3 "`sourceType` 이 서로 다르고 `originId` 도 서로 다를 때만 독립(AND)" · `unity-implementation.md` §5 임포트 불변식 1 동일 문장. 1차가 지적한 §71 의 두 모순 문장 중 하나(*종류* 기준)는 삭제되지 않고 **AND 로 흡수**됐다 — QA 가 요구한 "둘 중 하나 삭제"보다 나은 처리다.
- **세 번째 용어가 명시적으로 폐기됐다**: `unity-implementation.md` L69 괄호 — "C4 의 모호한 '매체 경로'라는 세 번째 용어는 폐기하고 이 두 필드로만 말한다".
- **1차의 (b) 지적은 부분적으로 거짓 양성이었다 — QA 자기정정 [OBSERVED]**: `interaction-rules.md` L102 의 반박("서로 다른 관측소의 정당한 염판 2점이 거부되는 것은 인식론이 아니라 **장르 규칙**")을 **수용한다.** 세계관 §2 의 "독립 매체 2종"이 캐논인 이상 매체 중복 거부는 의도된 동작이며, 1차가 이를 "정당한 근거가 사유 없이 막힌다"로 쓴 것은 과했다. 다만 §3 L103 이 **거부 사유를 종류/출처로 구분 표기**하고 `T-22`·`matrix[16]` 이 그것을 검사 대상으로 세웠으므로, 1차가 실제로 막고자 한 실패(사유 없는 차단)는 닫혔다.
- **데이터가 규칙을 이미 만족한다 [OBSERVED, Q3]**: 검증기 `C-07` = `proofRequired` **15비트 전건 독립쌍 존재 PASS 15/15**, 실패 목록 `[]`. 규칙이 문서에만 있는 것이 아니다.
- **잔여(결함 아님)**: `interaction-rules.md` §7-1 **RFC-S5**(확정 사본의 루트 승계 예외 여부)는 여전히 **디렉터 미판정**이다. 레인 제안 "예외 없음"에 QA 는 **동의**한다 — 근거는 C-07 이 예외 없이도 15/15 PASS 라 예외를 둘 실익이 없다는 것이고, 예외를 두면 §3 L101 대로 법6 의 전제(두 개의 *서명*이 아니라 두 개의 *출처*)가 무너진다.

### R4.1.4 F4 — closed, 다만 QA 1차의 전제 하나를 정정한다

- **범위를 늘리는 대신 줄여서 닫았다**: `unity-implementation.md` §10 "T0 범위(**확정**): 공간 `hub` 1개, 도구 `circuit` + `reader` 2개, 목표 길이 25분". 1차가 제시한 세 선택지(dock 추가 / routing 승격 / circuit 얇게 포함) 중 **어느 것도 아닌 네 번째 해법**이며, 결과가 더 낫다 — `gate`(routing 홈)도 `dock`(alignment 홈)도 T0 에 없으므로 "쓰지 않을 공간을 만들고 쓸 도구의 공간은 만들지 않는다"는 지적 자체가 소멸한다.
- **수량 3자 일치 [OBSERVED]**: `unity-implementation.md` §10(셸 1 = hub · 도구 2 = circuit·reader · 25분) ↔ live `campaign.json`(Q4: `T0.minutes 25` · `zoneIds ["hub"]` · 비트 도구 `[] / ["circuit"] / ["reader","circuit"]`) ↔ `modeling/asset-budget.md` L24(`T0: 셸1·도구2·초상1`). QA 재검산: 셸 `1+1+1+1+1 = 5` · 도구 `2+1+1+1+1 = 6` · 초상 `1+1+1+1+1 = 5` — **합계 계약(셸5/도구6/초상5)과 일치**하며 1차가 계산한 "+63% 과소 산정"은 소멸했다. `modeling` 문서는 수정되지 않았고 **systems 가 범위를 줄여 맞춘 것**이다.
- **QA 1차 전제 정정 [OBSERVED]**: 이번 회차 지시는 "T0 = hub + circuit/reader 25분이 **asset-budget / art-direction / unity-implementation 세 곳**에서 일치하는가"였다. 실측 결과 **`concept/art-direction.md` 와 `concept/style-guide.md` 는 T0 를 한 번도 언급하지 않는다**[OBSERVED, `grep -rn "T0" concept/` → 0행]. 따라서 세 번째 자리는 art-direction 이 아니라 **live `campaign.json`** 이며, 위 3자 일치는 그 조합에서 성립한다. 1차가 근거로 든 `art-direction.md:20`(제3수문 = "선 연결·압력")은 **T0 와 무관해졌다** — gate 가 T0 에서 빠졌기 때문이다. 그 행이 `circuit` 을 뜻하는지 `routing` 을 뜻하는지는 여전히 문서에 없지만, 이제 **T0 게이트를 막지 않는다**(C1 이후 과제).
- **seal 의 배선 술어 스텁 문제도 구조적으로 소멸**: §10 "T0 의 결론 판정은 이 두 도구로 닫히는 것만 쓰며, 배선 범위 술어는 `circuit` 이 실제로 산출한다(스텁 아님)". 스텁을 두는 대신 스텁이 필요한 도구를 뺐다.
- **alignment 검증 경로가 생겼다**: §10 "alignment 는 별도 그레이박스 스파이크 … `C3-b2`/`C3-b3` 만 떼어 독립 측정, 추가 완성 에셋 0" + "본 생산 게이트는 (a) T0 사람 검증 (b) alignment 스파이크 **두 증거를 모두** 요구한다". 1차가 걱정한 "축소판을 측정한다"는 **두 증거 요구로 상쇄**된다. 해당 비트의 도구는 데이터에 실재한다(`c3-b2 ["alignment"]` · `c3-b3 ["alignment","reader"]`).
- **잔여**: C4-F8(`tech-verification/README.md` 가 `status: current` 인데 "T0 **30분**" · `-captureScenes hub,gate` 로 스테일).

## R4.2 신규 결함 (C4-F5 ~ C4-F18)

전부 **문서 대조**다. 런타임 재현 절차를 가진 것은 0건이다.

### C4-F5 — 컨트롤러 잔여 바인딩 충돌 (S2 · systems)

- [OBSERVED] `interaction-rules.md` L30 "도구 열기 … 패널 밖에서 `Y` 휠, **`LB`/`RB` 순환**" ↔ L36 "되돌림 / 다시 … **`LB+X` / `LB+B`**". `LB` 가 **단독 기능(도구 순환)** 과 **모디파이어** 두 역할을 동시에 갖는데, `Y` 에 대해 L41 이 세운 "두 의미가 동시에 활성인 상태는 없다"에 해당하는 분리 규칙이 `LB` 에는 **없다**. 패널 밖에서 되돌림을 하려고 `LB` 를 누르는 순간 도구가 먼저 순환한다.
- [OBSERVED] L37 "증거함·가설판·힌트 | 탭 클릭 | `I` · `H` · `F1` | **`back` 탭 · `RS`**" — 기능 **3개**에 컨트롤러 바인딩 **2개**. 어느 것이 어디에 붙는지 문서에 없다.
- 왜 material 인가: F1 이 닫은 것과 **같은 유형의 결함**이며, F1 의 수정이 `X`·`Y` 만 훑고 `LB`·`back`/`RS` 를 훑지 않았음을 보여준다. `matrix[12]` 는 `X`/`Y` 만 검사한다.
- blocker 가 아닌 이유: §0-8 불변식과 §1 표의 키보드 열이 **전 행에 독립 경로를 보장**하므로 진행 불가로 이어지지 않는다.
- 요구 수정: (1) `LB` 를 모디파이어 전용으로 내리고 순환을 `RB` 단독 + `RB` 롱프레스 역순으로 바꾸거나, 되돌림을 `LB+X` 가 아닌 조합으로 옮긴다. (2) L37 컨트롤러 열을 3기능 3바인딩으로 채운다. (3) `verification.matrix` 에 "패널 밖 `LB` 입력 → 도구 순환과 되돌림이 동시에 발행되지 않는다" 행 추가.

### C4-F6 — 세이브 v1 필드가 두 `current`/`draft` 문서에서 다르다 (S2 · systems · open-rfc RFC-S3) **[승격 차단]**

- [OBSERVED] `unity-implementation.md` §7 = `"chapter": 3, "dayIndex": 2, "propertyProtection": false`, 루트 평면(`eventSeq`·`eventLogHash`·`autoKeptClueIds`·`hintLevelUsed`).
- [OBSERVED] `systems/data-schemas/save.md`(**`status: current`**, c3) §1·§2 = `stageId`/`beatId`/`storyClock`, "**`dayIndex` 없음**(단일 야간)", `propertyProtection` = `lowland | dock | null` **enum**, 진행 상태는 `progress`/`commandLog` 하위. `checkpointRefs` 는 `save.md` 에 **없다**.
- 왜 material 인가: CLAUDE.md §9 불변식("저장 데이터 필드 이름 변경은 플레이어 세이브를 고아로 만든다")의 정면 대상이다. 코드가 0줄이라 지금 고치는 비용은 0이지만, `unity-implementation.md` 를 이 상태로 `current` 로 올리면 **`status: current` 두 문서가 세이브 v1 을 서로 다르게 정의**하게 된다.
- 레인은 이미 자체 발견했다(`c4-self-check.md` §4 SC-1) — QA 는 그 판단(임의로 한쪽을 지우지 않고 RFC-S3 판정을 기다린다)이 **옳다**고 본다. 이 행은 "고치라"가 아니라 **"판정 전에는 승격 불가"**를 기록하는 것이다.
- 요구: 디렉터 RFC-S3 판정 → 채택 시 §7 을 `save.md` 스키마로 교체(`checkpointRefs` 는 `save.md` 에 추가) → 그 다음 승격.

### C4-F7 — 저장 영수증 게이팅이 애니 계약에만 있고 VFX·모션 계약에는 없다 (S2 · vfx / motion)

- [OBSERVED] `animation/animation-contract.md` L28 "권위 있는 상태 변화로 읽히는 연출(**도장·봉인·구역 수면 확정**)은 영수증 이후에만 재생한다" · L27 "저장 실패 시 `stamp_down` 은 재생하지 않고 확정 사운드도 내지 않는다".
- [OBSERVED] `vfx/vfx-budget.md` 는 `seal_confirm(짧은 잉크)` · `water_rise(수면1장)` · `brine_flow` 를 6효과로 선언하면서 **영수증·`SavePending`·저장 실패를 한 번도 언급하지 않는다**(`grep -c "영수증\|저장\|SavePending" vfx-budget.md` → 0). 이 세 효과는 animation-contract 가 방금 이름으로 지목한 "봉인·구역 수면 확정" 바로 그 연출이다.
- [OBSERVED] `motion/motion-contract.md` 는 "확정 패널 180ms" 를 두고 "차례: **상태가 먼저 확정** → 결과 텍스트/소리 → 선택적 장식모션" 이라고만 한다. `SavePending`(진행 표시) 상태의 모션 규격이 없고, "상태가 먼저 확정"이 §0-10(저장 성공 전에는 후보) 이후에도 유효한지 문장에 없다.
- 왜 material 인가: F2 가 닫은 계약은 **"성공 연출을 먼저 보여주고 나중에 되돌리지 않는다"** 다. 그 계약을 지킬 책임이 있는 세 레인 중 **한 레인만 문서를 고쳤다.** `screens[14]` 에 「저장 진행 중」 상태가 생겼는데 그 상태를 그릴 모션·이펙트 규격이 없다.
- 요구 수정: (1) `vfx-budget.md` 에 "`seal_confirm`·`water_rise`·`brine_flow` 는 저장 영수증 이후에만 발행, 실패 시 미발행" 한 줄. (2) `motion-contract.md` 에 `SavePending` 진행 표시 규격(저감모션 시 대체 포함)과 "확정 패널은 영수증 이후" 명시. (3) 두 문서에 `R2` 태그.

### C4-F8 — `tech-verification/README.md`(status: current)가 T0 정본과 어긋난다 (S2 · systems)

- [OBSERVED] `systems/tech-verification/README.md` §2.6 "대상: **T0 30분** + `c3-b2`~`c3-b3` 정합 구간" ↔ 정본 25분(§10 · `T0.minutes 25` · `campaign-time-budget.md` L359 "T0 25분 + 34분 = 59분").
- [OBSERVED] 같은 문서 §2.5 프레임타임 캡처 명령 `-captureScenes hub,gate` — `gate` 는 T0 에 없다(F4 로 제외 확정).
- 왜 material 인가: 이 문서는 `status: current` 이고 **Codex 핸드오프가 실제로 돌릴 명령**을 담는다. draft 초안의 오기가 아니라 정본의 오기다. 30분으로 계측하면 25분 설계와 비교 불가능한 첫 값이 나온다.
- 요구 수정: §2.6 `T0 30분` → `T0 25분`, §2.5 `-captureScenes hub` (또는 "T0 는 hub 만, gate 는 C1 이후" 주석).

### C4-F9 — 도구 6종 표시명이 세 벌이고 그중 넷은 용어집 미등록 (S2 · worldview / planner / systems / concept)

[OBSERVED, Q12 · `grep -c`]

| toolId | `planning/gdd.md` §4 · `concept/style-guide.md` §9 · 루트 `README.md` | `systems/interaction-rules.md` §2 · `game-ui-contract.json` `screens[9].states` | `worldview/glossary.md` 등록 행 |
|---|---|---|---|
| `circuit` | **배선 추적** (Circuit Trace) | **회로 지도** | `회로 지도` (Circuit Map) ✔ / `배선 추적` ✘ |
| `reader` | **판독** | **판독기** | `판독기` (Plate Reader) ✔ / `판독` ✘ |
| `alignment` | 조위정합 | 조위정합 | `조위정합` ✔ (`정합기` 도 별도 등록) |
| `routing` | **배수 편성** | **경로 구성** | `편성기` (Router Bench) ✔ / `배수 편성` ✘ / `경로 구성` ✘ |
| `corrosion` | **부식 시험** | **부식예산** | `부식 시험대` ✔ / `부식 시험` ✘ / `부식예산` = **자원·제약의 이름** |
| `seal` | 이중서명 | 이중서명 | `이중서명` ✔ |

- 왜 material 인가 셋. (1) `worldview/glossary.md` 머리 규칙은 "여기에 없는 고유명사는 시놉시스·대사·**UI 문자열**·에셋 파일명에 쓸 수 없다"이고 `screens[9].states` 는 **UI 상태 라벨**이다. (2) `corrosion` 의 systems 라벨 `부식예산` 은 **자원 이름과 같다** — `planning/gdd.md` §4.1 이 "부식 확정·되돌림 → **부식 시험**"으로 바꾼 이유를 문서에 남기며 정확히 이 혼동을 막으려 했고(RFC-P3-009 로 부식은 소모되지 않는 제약이 됐다), systems 라벨이 그 결정을 되돌린다. (3) `status: current` 인 `gdd.md`·`style-guide.md` 와 승격 대상인 `interaction-rules.md`·`game-ui-contract.json` 이 서로 다른 이름을 말한다 — 승격하면 current↔current 모순이 된다.
- 요구 수정: worldview 가 도구 6종의 **표시명 1벌**을 용어집에 등록(제안: gdd/style-guide/README 3자가 이미 일치하는 `배선 추적·판독·조위정합·배수 편성·부식 시험·이중서명`, 기존 `회로 지도`·`판독기`·`정합기`·`편성기`·`부식 시험대`는 **기구 이름**으로 정의를 분리) → systems 가 `interaction-rules.md` §2 제목과 `screens[9].states` 를 그 1벌로 교체(JSON 수정 시 `game-ui-contract.meta.md` 해시 동시 갱신).

### C4-F10 — `modeling/asset-manifest.md`(status: current)의 스테일 주장 3건이 착수 차단선을 유지한다 (S2 · modeling)

- [OBSERVED] L135 OPEN-M3 "`concept/style-guide.md` **가 없어** … 6색을 그레이박스 전용으로 지어 썼다" ↔ `concept/style-guide.md` 는 `status: current` · `updated: 2026-09-10` 로 **존재**하며 §2 팔레트 8색 + §9 도구 6종 시각 키워드를 이미 준다.
- [OBSERVED] L21 "`concept/sheets/` 파일 수 = **0** … 즉 **컨셉 시트가 있는 자산은 하나도 없다**" ↔ `concept/sheets/README.md`(14.8 KB, `status: current`) 존재 · `find assets/generated/2d -name "*.png" | wc -l` = **45**(Q14) · `concept/generation-manifest.md` 가 45/45 성공과 provenance 6종을 기록.
- [OBSERVED] L133 OPEN-M1 "zone 토큰 `Hub` 는 용어집 EN(`Watch Room`) 파생이 아니다. worldview 판단 필요" ↔ `worldview/glossary.md` L149·L154 가 `Hub↔hub` · **`Quay↔dock`** · `Gate3↔gate` · `Pump1↔pump` 매핑을 이미 확정했다.
- 왜 material 인가: 이 세 주장이 §0 의 **차단선**("실루엣·재질·디테일 모델링은 `concept/sheets/*.md` 가 생기기 전에는 시작하지 않는다")과 **OPEN-M4**("컨셉 시트 0개 상태에서 셸 4종·소품 30종·초상 5종은 착수할 수 없다")를 떠받친다. 전제가 이미 해소됐는데 차단이 남아 있으면 C5 생산 계획이 근거 없이 멈춘다. 이것은 C3-F28·C3-F34 와 **같은 유형(스테일 `[OBSERVED]`)** 이며, 그 유형이 C4 레인에서 재발했다.
- 요구 수정: L21·L133·L135 를 실측으로 재작성(시트 색인 = `concept/sheets/README.md`, 강조색 = `style-guide.md` §2·§9, zone 토큰 = glossary L154), OPEN-M1·M3 closed, OPEN-M4 는 "시트 존재 → 차단 해제, 다만 §9 재생성 우선순위 1~3 은 선행" 으로 재판정.
- 부기(결함 아님): 합계는 맞는다 — `5 + 6 + 5 + 30 + 1 = 47` 이 §6 표·`asset-budget.md` L13~19 와 일치한다.

### C4-F11 — style-guide §10 위반 후보 1순위 자산이 루트 README 에 게시돼 있다 (S2 · concept / presentation / director)

- [OBSERVED] `concept/sheets/README.md` §6 `readme-verb-seal` 검수 결과: "**우측 책자의 손글씨가 문자에 가깝게 렌더됨 — §10-4 위반**", 같은 문서 §9 재생성 우선순위 **1위**("§10-4 금지 위반 가능성 최고").
- [OBSERVED] 그 파생본 `docs/media/verb-seal.jpg` 가 저장소 `README.md` L38 에 임베드돼 있다(`ls docs/media/` 확인).
- [OBSERVED] `concept/style-guide.md` §10 머리말 = "**위반 시 REDO, 예외 없음**", 4항 = "이미지 내 텍스트·숫자·로고·간판".
- 왜 material 인가: 계약 `Unity / 저장소 배치` 는 "하네스는 **push-ready 상태**(.gitignore, README, 미디어 등록)까지 만든다"고 했고 사용자가 직접 push 한다. 자기 레인이 "예외 없음" 금지 위반 후보로 표시한 이미지가 push-ready 상태의 **첫 화면**에 들어가 있다.
- 요구 수정: 셋 중 하나 — (a) `readme-verb-seal` 재생성 후 교체(§9 비용 1장 ≈ 91초), (b) README 에서 해당 행의 이미지를 제거하고 텍스트만 남김, (c) 디렉터가 "손글씨 추상 파선은 §10-4 의 '텍스트'가 아니다"를 판정으로 남기고 `sheets/README.md` §6·§9 를 그 판정으로 갱신. **결정 없이 push 하지 않는다.**

### C4-F12 — 접근성 계약이 `gdd.md` §8 목록 13행 중 3행을 덮지 않는다 (S2 · systems / planner)

`planning/gdd.md` §8 은 소유자를 "**systems(`game-ui-contract.json` `accessibility`)**"로 명시한다. 그 계약을 13행 전건 대조했다 [OBSERVED, Q2·Q13].

| gdd §8 행 | 계약 커버 | 위치 |
|---|---|---|
| 전 항목 키/버튼 재매핑 | ✔ | `screens[3].decision`(키·패드·확정 방식·홀드 시간) |
| 길게 누름 → 토글 전환 | ✔ | `accessibility.hold_alternative` · `screens[3].states` |
| 연속값 조작의 **이산 대안**(정합·편성·부식 3동사) | **△ 부분** | `keyboard_only` 가 총론으로만 덮는다. JSON 에 `이산` **0회**, 3동사별 판정 근거 없음 |
| 텍스트 크기 3단 이상 · UI 배율 | ✔ | `layout.text_scaling`(100~150) · `matrix[1]`·`[9]` · `acceptance[4]` |
| 색 단독 금지 | ✔ | `accessibility.signals` · `media_alternatives` |
| **색약 대체 팔레트(적록·청황)** | **✘ 없음** | JSON 전체에서 `색약` **0회**, `색각` **1회**(그나마 `matrix[3].setting` "색각 보조 켜짐"이라는 **설정값 이름**뿐). 어떤 팔레트인지·`style-guide.md` §2.1 세트 A/B 와 어떻게 연결되는지 계약에 없다. gdd 판정 기준은 "**팔레트 3종 모두에서 구분 가능**"인데 3종을 도는 matrix 행이 0개 |
| 모션 축소 | ✔ | `accessibility.motion` · `matrix[4]` |
| 전체 자막 · 화자 이름 · 효과음 문자 알림 | ✔(화자 이름은 △) | `media_alternatives` |
| **채널별 볼륨(대사/효과/환경) 3채널 독립** | **✘ 없음** | JSON 전체에서 `볼륨` **0회**. `screens[2] settings` 도 "표시 자막 입력 접근성"만 든다 |
| 무료 힌트 3단 · 무제한 되돌림 | ✔ | `screens[12]` · `data_bindings[6]` · `matrix[7]` |
| 실시간 압박 없음 | ✔(간접) | `interaction-rules.md` §0-1. 계약에는 `타이머` 0회 |
| 접근성 설정을 플레이 시작 전 도달 | ✔ | `accessibility.settings_entry` · `matrix[0]` |
| 한국어 / 영어 | ✔ | `localization` 블록 전체 |

- 왜 material 인가: 결손 3행이 전부 **본편 무료 제공 계약**(`product/business-model.md` 기능 인질 금지 목록 연동)이고, 색약 팔레트는 `concept/style-guide.md` §2.1 이 이미 세트 A/B 를 정의해 둔 상태라 **계약 쪽만 비어 있다.** G4·G6 의 문서 근거가 그만큼 얇다.
- 요구 수정: `accessibility{}` 에 `color_vision`(세트 A/B 참조 + "색+형태+위치 3중 부호" 재확인)·`audio_channels`(3채널 독립)·`discrete_input`(정합·편성·부식 3동사의 이산 스텝) 3키 추가, `verification.matrix` 에 색약 3팔레트 순회 1행 추가, `screens[2] settings` 상태에 볼륨 탭 반영.

### C4-F13 — `game-ui-contract.meta.md` 해시 표가 한 행 스테일이고 표가 끊겨 있다 (S3 · systems)

- [OBSERVED, Q7] 기록 L26 `interaction-rules.md` = `af906dfb37648bb19d91ea325a07e1b18e67391303f103ed18287d5d03b7e7d9` / 6,236 chars ↔ 실측 `98ad78d60f14aab22612bcecfbf5332aeec0f964d0a3cb06be56085663a296e4` / **9,131 chars**(16,865 B). 같은 표의 L25(`unity-implementation.md` `453f8481…`/7,909)·L27(`animation-contract.md` `ec3ad8c1…`/1,535)·L21(JSON `83b92c82…`/37,990 B)은 **전부 일치**한다.
- [OBSERVED] 아카이브 c4 판본은 `d9b4056b…` 로 둘 다와 다르다 → `af906dfb…` 는 C5 중간 상태이며 그 뒤의 R4 편집(§3 C3-F12 명문화·§3.1 C-07 절·§7-1 RFC-S5)이 반영되지 않았다. 참고로 `c4-self-check.md` §4-1 은 이 파일 편집을 "**1행만**"이라 적었는데 실측 증가분은 **+2,895 chars** 다 — 표기와 실물이 다르다.
- [OBSERVED] 표 구조: 헤더 L19~20 다음 L21 한 행, 그 뒤 L23~24 문단이 끼어들고 L25~27 이 다시 표 행으로 이어져 **렌더 시 세 행이 표에서 분리**된다.
- 요구 수정: L26 을 실측값으로 교체(명령: `shasum -a 256 _workspace/current/systems/interaction-rules.md`), L25~27 을 L21 과 한 표로 합치고 개정 문단은 표 아래로 이동, `c4-self-check.md` §4-1 의 "1행만" 을 실제 편집 범위로 정정.

### C4-F14 — systems 자체 발견 SC-2 · SC-3 · SC-4 (S3 · systems / director) — **QA 승인**

레인이 `c4-self-check.md` §4 에 숨기지 않고 적었고, QA 가 재확인해 **셋 다 실재**한다. 처리 권한만 필요했던 항목이므로 승인한다.

| 원 id | 관측 [OBSERVED] | QA 판정 |
|---|---|---|
| SC-2 | `game-ui-contract.json` `matrix[12].expected` "도구 **휴**은 패널 밖에서만" (`휠` 오타) · `matrix[15].expected` "사유가 출처 중복**로**" | **수정 승인.** 두 글자 수정 + `game-ui-contract.meta.md` L21 해시 재측정을 **한 번의 편집**으로. C4-F9·C4-F12 수정과 같은 편집에 묶으면 해시 갱신이 1회로 끝난다 |
| SC-3 | `unity-implementation.md` §10 alignment 스파이크 대상이 `C3-b2`/`C3-b3` **대문자** ↔ live 비트 id 는 `c3-b2`·`c3-b3`(Q5·V7) | **수정 승인.** C3-F30("문서 간 인용 키는 campaign id 뿐")의 직접 적용 |
| SC-4 | RFC-P3-015 (F20)이 180초 자동 제안의 정본을 "`interaction-rules` §4" 로 인용 ↔ 그 파일에 `180` **0행**(Q 재확인). 규칙 실물은 `system-specs/hint-system.md` | **디렉터 몫.** 내용 결손이 아니라 RFC 본문의 인용 위치 오기다. QA 제안: RFC 인용을 `system-specs/hint-system.md` 로 정정하고, `interaction-rules.md` §4 에 정본 위치를 가리키는 한 줄을 더한다(둘 다 하면 중복 정의가 아니라 상호참조가 된다) |

### C4-F15 — 용어집 미등록 명사 3종이 정본 데이터·컨셉 문서에서 쓰인다 (S3 · worldview / planner / concept)

- [OBSERVED, Q12] `판독대` — 용어집 등록 행 **0**. 사용처: `planning/campaign.json` L294·L778·L811·L817(**정본 데이터**), `planning/content-matrix.md` §2 `hub` 행("작업대·**판독대**·봉인대"), `planning/feature-specs/verb-02-plate-read.md` R1. 용어집에는 `판독기`(Plate Reader, 장치)만 있다 — **대(臺)와 기(器)가 다른 사물인지 같은 사물의 두 이름인지 정의가 없다.**
- [OBSERVED] `조위관측판` — 등록 **0**. 사용처: `concept/art-direction.md` L22, `concept/style-guide.md` §8 부두 행, `concept/sheets/README.md` §4(`space-wharf-mood` 검수문 "무문자 조위관측판").
- [OBSERVED] `성에선` — 등록 **0**. 사용처: `art-direction.md` L22, `style-guide.md` §8.
- 왜 S3 인가: 진행을 막지 않지만 용어집 머리 규칙("UI 문자열·에셋 파일명에 쓸 수 없다")의 대상이고, `조위관측판`·`성에선`은 **이미 생성된 이미지의 검수 기준 문장**에 들어가 있다.
- 요구 수정: worldview 가 3종을 §2(매체·장치) 또는 §1(장소)에 등록하거나, `판독대` → `판독기` 로 데이터·문서를 통일한다(전자 권장 — `campaign.json` 이 정본이고 수정 반경이 넓다).

### C4-F16 — `Space` 전역 프리뷰와 초점 활성화의 우선순위가 없다 (S3 · systems)

- [OBSERVED] `interaction-rules.md` L34 프리뷰 키보드 = **`Space`**(장치·화면 조건 없음) ↔ L35 확정 = "확정 버튼으로 초점 이동 후 `Enter`". `screens[13].initial_focus` 는 "확정은 별도 초점 이동 후 **한 번 누름**". 표준 UI 관례에서 `Space` 는 초점 컨트롤 활성화 키인데, 초점이 확정 버튼에 있을 때 `Space` 가 프리뷰인지 확정인지 문서에 없다.
- C4-F5 와 같은 유형(같은 화면·같은 키·두 결과)이며 F1 이 `X` 에 대해 세운 규율이 `Space` 에는 적용되지 않았다.
- 요구 수정: L40 옆에 한 줄 — "`Space` 는 **초점이 확정 버튼에 있을 때 활성화로 동작하지 않는다**(프리뷰 전용). 확정 발행 키는 `Enter` 뿐이다." + `matrix` 에 검사 행 1개.

### C4-F17 — VFX 6효과와 애니 클립 3종이 에셋 예산·매니페스트에 자리가 없다 (S3 · vfx / modeling / animation)

- [OBSERVED] `modeling/asset-budget.md` L13~19 라이브러리 표 = 셸5 / 도구6 / 공용소품30 / 초상5×3 / UI프레임1 — **VFX 카테고리가 없고 견적 인일도 0**. `asset-manifest.md` 47행도 마찬가지다. `vfx/vfx-budget.md` 는 6효과(`brine_flow`·`pressure_pulse`·`salt_reveal`·`water_rise`·`seal_confirm`·`rain_window`)를 선언한다.
- [OBSERVED] 두 예산의 접점이 없다: asset-budget "드로콜 ≤150 / 뷰" ↔ vfx "동시 emitter ≤8 · 투명입자 ≤500". 에미터가 드로콜을 얼마나 먹는지 배분 규칙이 어느 문서에도 없다.
- [OBSERVED] `animation/animation-contract.md` 재사용 클립 10종 중 대상 프롭이 `asset-manifest.md` 공용소품 30 목록에 **없는 것 3종**: `drawer_open`(서랍) · `shutter_raise`(셔터 — `style-guide.md` §8 부두 "직사각 셔터 격자"가 요구) · `water_level`(수면 — VFX `water_rise` 와 소유가 겹친다). 나머지 7종은 대응 확인(`valve_turn`→`SM_Kit_Wheel_Valve`, `lever_throw`→`SM_Kit_Lever_Long`, `pump_spin`→`SM_Kit_Crank`, `gauge_settle`→`SM_Kit_Panel_Gauge`, `connector_snap`→`SM_Kit_Pipe_Flange`, `plate_insert`→`SM_Tool_reader`, `stamp_down`→`SM_Tool_seal`).
- 요구 수정: (1) `asset-budget.md` 에 VFX 행(6효과 · 견적 인일 · 드로콜 배분 상한) 추가, `asset-manifest.md` §6 합계를 47 → 47+VFX 로 재도출. (2) 공용소품 30 에 서랍·셔터를 넣거나(다른 항목과 교체) 두 클립을 셸 소유로 재배정. (3) `water_level` 의 소유를 animation ↔ vfx 중 하나로 확정.

### C4-F18 — 프리비즈 9프레임과 채택 연출 순서의 55~60초 구간이 어긋난다 (S3 · presentation / concept)

- [OBSERVED] `presentation/video-study.md` §채택: "… 45~55초 세 장소 대비, **55~60초 제목/플랫폼**".
- [OBSERVED] `concept/sheets/README.md` §7 은 열 제목을 "시퀀스 위치(**`video-study.md` 채택 순서**)"라 적고 `previz-f08-signing-desk` = "55~60초 · 서명대", `previz-f09-signing-close` = "55~60초 · 서명" 으로 배정했다.
- 즉 채택 순서에는 **서명 구간이 없고**, 생성된 9프레임에는 **제목/플랫폼 프레임이 없다**. 두 문서 중 하나가 상대를 잘못 인용한다.
- 부기 [OBSERVED]: 나머지 7프레임(f01~f07)은 §채택 6구간과 정확히 대응한다. 또 `video-study.md` 는 `cycle: 20260909-preproduction-c2` · `status: draft` 인데 `style-guide.md`(c4 · current)가 이를 **캐논 출처**로 인용한다 — C3-F33 이 지적한 "draft 를 정본으로 인용" 유형이며 C4 에서 재발했다.
- 요구 수정: presentation 이 §채택을 7구간으로 늘려 서명 구간을 넣거나(권장 — 게임의 확정 게이트가 서명이다), concept 가 f08·f09 를 "채택 순서 밖 · 보조 프레임"으로 재표기. 어느 쪽이든 `video-study.md` 의 cycle·status 를 C4 기준으로 재도출한다.

## R4.3 결함으로 올리지 않은 것 (판단 근거 기록)

- **런타임 수치 전량**: `unity-implementation.md` §9 6항 · `asset-budget.md` §C4 실측표 · `vfx-budget.md` · `motion-contract.md`(650/220/180/140/600 ms) — 전부 `[TARGET]`/`NOT-MEASURED` 로 정확히 라벨돼 있다. `asset-budget.md` 는 "**드로콜은 측정하지 못했다** … Blender 오브젝트 수(12)는 드로콜이 아니다"까지 스스로 적었고 §"G5가 아직 통과할 수 없는 이유"를 문서 안에 뒀다 — 이런 자기 제한은 결함이 아니라 이 회차에서 가장 정직한 서술이다.
- **`motion-contract.md` 650 ms ↔ `unity-implementation.md` 100 ms**: 축이 다르다(전환 총 길이 vs 첫 프레임 지연). F2 의 "1프레임 vs 200 ms" 와 달리 서로를 전제하지 않으므로 올리지 않았다. 다만 §9 L112 같은 축 분리 문장이 motion 쪽에 없어 C4-F7 요구에 포함했다.
- **`balance/puzzle-balance.md`**: 스스로 "이 파일은 `status: draft` 이며 **정본이 아니다** … 다른 레인 문서는 이 초안이 아니라 밸런스 시트를 인용해야 한다"고 머리에 고지했고, 본문 "3분 무진전 감지"는 RFC-P3-015 의 180초와 일치한다. T0 프로토콜(`첫조작≤60초`·`목표설명 10/12`·`진행불가 0/12`·`실제 n=0`)도 `product/assumption-tests.md`(20~30분 슬라이스)와 25분에서 모순이 없다. **결함 0건**.
- **`economy/resources-and-fairness.md`**: 부식예산을 "제약을 보여주는 UI"로 정의해 RFC-P3-009 정본과 일치하고, `interaction-rules.md` §2.5(전역 상한 9 · 소모·차감·잔량 없음 · `lowland` 7 · `dock` 8)·`data_bindings[4]`·`zones.md` Z-I8 과 4자 무모순. DLC 무차이 선언도 `acceptance`·`screens` 와 충돌 없음. **결함 0건**.
- **"gameplay" 단정 문구 0건 [OBSERVED]**: 루트 `README.md`·`docs/media/provenance.json`(13항 전건)·`concept/*`·`modeling/pipeline.md`·`presentation/deck-outline.md` 를 전수 grep 했다. 모든 등장이 부정문("게임플레이 아님", "NOT gameplay", "실제 캡처 전에는 게임플레이라 부르지 않는다")이거나 Steam 문서 인용이다. 계약 `Honesty gates` 의 요구가 지켜졌다.
- **`asset-manifest.md` 47 vs `asset-budget.md` 합계**: 지시받은 대조 결과 **일치**한다(5+6+5+30+1 = 47, §6 표와 L13~19 표가 같은 값). 불일치는 수량이 아니라 C4-F10 의 **스테일 서술**에 있다.
- **`style-guide.md` 팔레트 ↔ `art-direction.md` 의미색**: 4색 전건 문자 일치(`#E7E3D8` 종이 · `#36565C` 금속 · `#E2AF62` 변경예정 · `#173238` 확정), 윤곽 3 px 도 일치. 공간 5·인물 5·도구 6 의 대응도 일치. **결함 0건** — 다만 도구의 *이름*은 C4-F9 다.
- **`systems/prototype/*`**: `test-model.mjs` **37/37 통과**(Q6), 해시 4/4 문자 일치(Q8), `prototype.meta.md` 가 "모형의 통과는 캠페인의 통과가 아니다 … T0·C3 에서 뽑은 규칙만 덮는다"를 스스로 적었다. **결함 0건**.
- **`c4-self-check.md` 자체**: 레인이 자기 확인표를 만들고 §4 에 미수정 4건을 숨기지 않은 것은 **모범**이다. QA 는 그 표를 신뢰하지 않고 Q1~Q14 로 다시 쟀으며 V1~V8 주장은 **전건 재현됐다**. 단 §4-1 의 "`interaction-rules.md` 1행만 편집" 표기는 실측과 다르다(C4-F13).

## R4.4 승격 판정 (C3-F33 · RFC-Q2)

C3-F33 은 "R4 에서 QA 가 검증한 문서는 소유 레인이 같은 `cycle` 값 그대로 `status: current` 로 올린다"고 했다. **QA 는 F1~F4 해소만으로 자동 승격하지 않는다** — 승격 후 `status: current` 문서끼리 모순이 생기면 그 자체가 다음 회차 결함이 되기 때문이다. 판정은 아래와 같다.

| 파일 | 판정 | 사유 |
|---|---|---|
| `_workspace/current/animation/animation-contract.md` | **승격 가능** | F2 대응 완결(1프레임 잠금 폐기 명시 + 영수증 게이팅), 다른 `current` 문서와 모순 0. C4-F17 의 클립 3종은 modeling 소유 |
| `_workspace/current/economy/resources-and-fairness.md` | **승격 가능** | RFC-P3-009 정본과 4자 무모순, 결함 0건 |
| `_workspace/current/systems/prototype/README.md` · `prototype.meta.md` | **승격 가능** | 37/37 재현 · 해시 4/4 일치 · 자기 한계 명시 |
| `_workspace/current/systems/interaction-rules.md` | **차단** | C4-F9(도구 표시명이 `gdd.md`·`style-guide.md`(둘 다 current)와 다르다) · C4-F5 · C4-F16 |
| `_workspace/current/systems/unity-implementation.md` | **차단** | C4-F6(세이브 v1 필드가 `data-schemas/save.md`(current)와 다르다 — CLAUDE.md §9 불변식 구역, RFC-S3 판정 선행) · C4-F14 SC-3 |
| `_workspace/current/systems/game-ui-contract.json` | **차단** | C4-F12(gdd §8 3행 미커버) · C4-F9(`screens[9]` UI 라벨) · C4-F14 SC-2 |
| `_workspace/current/systems/game-ui-contract.meta.md` | **차단** | C4-F13(해시 스테일) + 기술 대상 JSON 이 차단 상태 |
| `_workspace/current/motion/motion-contract.md` · `_workspace/current/vfx/vfx-budget.md` | **차단** | C4-F7(§0-10 영수증 게이팅 미반영) |
| `_workspace/current/balance/puzzle-balance.md` | **차단(의도)** | 문서 스스로 "정본이 아니다"를 선언했다. 승격하면 `balance-sheet.md` 의 소유와 충돌한다 — draft 유지가 맞다 |
| `_workspace/current/presentation/video-study.md` | **차단** | `cycle: …-c2` · C4-F18. c4 기준 재도출과 구간 정정 후 재신청 |

이미 `status: current` 인 파일(`concept/art-direction.md`·`concept/style-guide.md`·`modeling/{asset-budget,asset-manifest,pipeline}.md`·`systems/tech-verification/c4-self-check.md`)은 승격 대상이 아니며, 그중 `asset-manifest.md` 는 C4-F10, `tech-verification/README.md`(같은 레인 current)는 C4-F8 로 **현 상태에서 정정 대상**이다.

## R4.5 브로드캐스트 (dependency-matrix ● 항목)

`feedback-requested-by: 2026-09-11`

| 받는 레인 | 결함 | 요청 |
|---|---|---|
| game-production-director | C4-F6(RFC-S3) · C4-F11 · C4-F14 SC-4 · RFC-S5 | RFC-S3(세이브 `dayIndex`·`propertyProtection` 형) 판정 — 코드 0줄인 지금이 가장 싸다. RFC-S5 는 QA 도 "예외 없음"에 동의(C-07 15/15 가 예외의 실익을 없앤다). C4-F11 은 **push 전 결정 필요**. SC-4 는 RFC-P3-015 본문의 인용 위치 정정 |
| game-systems-designer | C4-F5 · C4-F6 · C4-F8 · C4-F12 · C4-F13 · C4-F14(SC-2·SC-3) · C4-F16 · C4-F9(수용 측) | `LB` 모디파이어 분리 + L37 3바인딩 · `tech-verification/README.md` 25분/hub · `accessibility{}` 3키 신설 + matrix 색약 3팔레트 행 · meta L26 해시 재측정 · JSON 오타 2건 + `c3-b2` 소문자 · `Space` 우선순위 1줄. **JSON 수정은 한 번에 묶고 meta 해시를 같은 편집에서 갱신할 것** |
| game-worldview-architect | C4-F9(정본 측) · C4-F15 | 도구 6종 **표시명 1벌**을 용어집에 등록하고 기존 `회로 지도`·`판독기`·`정합기`·`편성기`·`부식 시험대` 는 **기구 이름**으로 정의 분리. `판독대`·`조위관측판`·`성에선` 등록 또는 `판독대`→`판독기` 통일 판정 |
| game-modeler | C4-F10 · C4-F17 | `asset-manifest.md` L21·L133·L135 실측 재작성 → OPEN-M1·M3 closed, OPEN-M4 재판정. `asset-budget.md` 에 VFX 행 + 드로콜 배분. 서랍·셔터 프롭 배정 |
| game-concept-artist | C4-F11 · C4-F18 · C4-F15 | `readme-verb-seal` 재생성 여부 결정(§9 1순위) · §7 시퀀스 표의 55~60초 재표기 · 컨셉 문서의 미등록 명사 |
| game-presentation-director | C4-F18 | `video-study.md` §채택 7구간화 여부 + `cycle`·`status` 를 C4 기준으로 재도출 |
| game-vfx-artist | C4-F7 · C4-F17 | `seal_confirm`·`water_rise`·`brine_flow` 의 **저장 영수증 게이팅** 명문화 · 드로콜 배분 · `water_level` 소유 확정 |
| game-motion-designer | C4-F7 | `SavePending` 진행 표시 규격(저감모션 대체 포함) · "확정 패널은 영수증 이후" · 650 ms↔100 ms 축 분리 주석 |
| game-animator | (요청 없음) | F2 대응이 이 회차에서 가장 깨끗했다. 승격 가능 판정. C4-F17 의 클립 3종은 modeling 회신 대기 |
| game-planner | C4-F9 · C4-F12 · C4-F15 | `gdd.md` §4 표시명이 정본 후보임을 worldview 에 제출 · §8 13행의 소유자 지정 유지 여부 · `판독대` 데이터 통일 시 `campaign.json` 4곳 영향 |
| game-balance-designer / game-economy-designer | — | 이번 회차 결함 0건. economy 는 승격 가능 |

## R4.6 이 재검증이 증명하지 않는 것 [OBSERVED]

- **F1~F4 closed 는 "계약 모순이 사라졌다"이지 "동작한다"가 아니다.** Unity `Assets` 파일 수 0, 빌드 0줄, `matrix` 17행·`T-15`~`T-27` 전건 미실행.
- **접근성은 여전히 실사용 검증 0명이다.** 홀드를 유지할 수 없는 플레이어·색각 이상 사용자·보조기기 사용자 표본 전부 0.
- **T0 3자 일치는 수량과 id 의 일치**다. 25분이 실제 소요라는 증거도, 재미있다는 증거도 없다. `observedMedianMinutes: null` · `humanPlaytests: []`.
- **성능·저장 실측 0건.** G4·G5·G6 은 이번 회차에도 `runtime: NOT-MEASURED` 로 남는다(`qa/gate-measurements.md#g4`·`#g5`·`#g6`).
- **익스플로잇·회귀 매트릭스·몰입 점수·플레이테스트 보고서는 이번에도 작성하지 않았다.** 빌드와 표본이 없는 상태에서 그 문서를 만드는 것은 측정 위장이다(사유는 `qa/defect-register.md` §4 유지).


---

# 재검증 1 (2026-09-10, R4 수정 루프 1 이후) — game-qa

> 같은 사이클 안의 **제자리 개정**이다(RFC-Q2). `cycle`·`status`·`supersedes` 는 그대로 두고 이 절만 append 했다. 위 1차 본문과 「재검증 (2026-09-10, R4)」 절은 **당시 기록으로 보존**한다(CLAUDE.md §2 삭제 금지).
> 대상은 디렉터 배정 **C4-F5 · C4-F8 · C4-F10** 3건이다. 레인 보고를 옮기지 않고 **파일을 다시 열고 명령으로 다시 쟀다** — 아래 `W#` 가 QA 측 원문이다.
> **런타임 증거는 이번에도 0건이다.** Unity 실행 0회 · 빌드 0줄 · 패드 실측 0건 · 프레임/저장 캡처 0건 · 플레이 표본 n=0. 아래 `closed` 는 "문서·데이터의 모순이 사라졌다"는 뜻이며 **"동작한다"가 아니다.** 어떤 게이트도 PASS 로 올리지 않는다.

## V1.0 재측정 명령 원문 [OBSERVED 2026-09-10, QA 직접 실행]

| # | 명령 | 결과 | 레인 주장과 대조 |
|---|---|---|---|
| W1 | `node _workspace/current/planning/validate-campaign.mjs` | `{checks 47, pass 47, fail 0, verdict PASS}` · sha `92301c0a5ecfc7e142646f5f17c29c21609e2b2de7c90408de7bffc9d815ae23` · **121,457 B** · `aggregates.stageMinutes[0] = 25` | systems·modeling·presentation 3레인 주장과 **문자 일치**. RFC-Q1 대로 sha 는 검증기 출력에서 읽었다 |
| W2 | `node -e` 로 `stages[0]` 직접 판독 | `T0.id=T0 · minutes=25 · zoneIds=["hub"]` · T0 비트 도구 합집합 `["circuit","reader"]` · `proofRequired` 비트 **1** · 전 구역 5(`dock gate hub lowland pump`) | C4-F8·C5-F1 판정의 공통 정본 |
| W3 | `grep -n "LB\|RB\|RS\|back\|모디파이어" systems/interaction-rules.md` | L31 「도구 열기 … `RB` 단독 순환」 · L37 「되돌림/다시 … `LB`(모디파이어)+`X` / `LB`+`B`」 · L38 「증거함·가설판·힌트 … `back` · `LB`+`back` · `LB`+`RS` — 3기능 3바인딩」 · L43 「`LB` 에는 **단독 기능이 없다**」 · §1-2 표 L68 「`LB` 단독 의미 = **없음(모디파이어 전용)**」 | C4-F5 요구 (1)(2) 이행 확인 |
| W4 | `python3 -c "json.load(...)"` 구조 집계 | `verification.matrix` **18행**(R4 시점 17행) · `matrix[17].state` = "패널 밖에서 LB를 눌렀다 뗀 뒤 LB와 X로 되돌림" · `.expected` = "LB 단독으로는 아무 명령도 발행되지 않아 … 도구 순환과 되돌림이 동시에 발행되지 않는다" · `screens 19` · `verification.acceptance 8` · `data_bindings 10` | C4-F5 요구 (3) 이행 확인. 기존 17행 **삭제 0건** |
| W5 | `python3 ~/.claude/skills/game-ui-ux/scripts/validate-game-ui.py systems/game-ui-contract.json` | `PASS: valid game UI contract` · exit 0 | 행 추가로 스키마가 깨지지 않았다 |
| W6 | `shasum -a 256` 7종 + `wc -c` | `interaction-rules.md` `3c901743…` **25,545 B** · `game-ui-contract.json` `9c89e9ae…` **38,429 B** · `tech-verification/README.md` `ace675a5…` · `hint-system.md` `239361f0…` · `plate-readout.md` `e284dd5e…` · `unity-implementation.md` `453f8481…`(불변) · `animation-contract.md` `ec3ad8c1…`(불변) | `game-ui-contract.meta.md` 해시 표 **4행 전건 문자 일치**(R4 의 1행 스테일 소멸) |
| W7 | `grep -n "30분\|captureScenes" systems/tech-verification/README.md` | §2.5 명령 = `-captureScenes hub`(L207) · "30분" 잔존 2건은 **정정 로그의 서술**(L218 "이전 판은 … 였다" · L246 변경 이력) | C4-F8 요구 2건 이행 확인. 지시문이 아닌 이력 문장은 정정 대상이 아니다 |
| W8 | 정본 3자 대조 | `unity-implementation.md` §10 L115 "목표 길이 **25분**" · W1 `stageMinutes[0]=25` · `campaign-time-budget.md` L359 "T0 25분 + 34분 = 59분" | 3자 일치 재현 |
| W9 | `find` 3종 | `concept/sheets` 파일 **1**(`README.md`) · `assets/generated/2d -name '*.png'` **45** · `provenance.json` **6** · `presentation/scene-boards` **0** | `asset-manifest.md` §0 재작성 값과 일치. scene-boards 0 은 여전히 참 |
| W10 | `sed -n '148,158p' worldview/glossary.md` | §7 파생 규칙 = "zone 토큰은 `zoneId` 영문 토큰 그대로 허용 … `modeling/pipeline.md` 의 `Hub` 는 그대로 유효 … **OPEN-M1 닫힘**" · 읽는 법 `Hub↔hub`·`Gate3↔gate`·`Pump1↔pump`·**`Quay↔dock`**·`Lowland↔lowland` | OPEN-M1 closed 근거 재현 |
| W11 | `python3` 로 `build_hub_greybox.py` 의 `ACCENT` 6색 + `GREY_*` 3색과 `style-guide.md` 팔레트 hex 집합 비교 | 그레이박스 9색 `3F8EA8 · 7E6FB0 · 4FA07A · C4703F · B0566B · 8A8F5C · 9A9A96 · 7E7E7A · 8C8C88` ↔ 팔레트 교집합 **0/9** | 모델러 주장(0/9) **재현됨** — 임시 6색을 "재질 후보"에서 내린 판단은 측정에 근거한다 |
| W12 | `python3 -c "math.degrees(math.atan((5.501-0.95)/(1.20-(-5.30))))"` | **34.998** | OPEN-M6 의 부감각 차 17.0° 재현 |
| W13 | `bash .claude/skills/game-ops-harness/scripts/freshness-check.sh` | `0 finding(s) across 95 markdown artifact(s)` · **exit 0** | R5 검토 종료 시점(95)과 동일 — 이번 루프의 편집은 전부 제자리 개정이라 신규 산출물 0 |
| W14 | `system-specs/*.md` 의 §1 입력 표 전수 파싱(`X`·`Y`·`Space`·`H`·`I`·`RS` 및 키보드 토큰) | 표 41행 중 **키보드 토큰 없는 행 21** · `X` 가 6개 스펙에서 **6가지 의미** · `H`(tide-alignment L31) · `I`(wiring-trace L31) 충돌 | → 아래 V1.5 신규 3건 · C4-F16 확대 |
| W15 | `cd systems/prototype && node test-model.mjs` | **37 통과 / 0 실패** | R4 승격 가능 판정의 근거가 이번에도 재현됨 |

## V1.1 판정 요약

| id | severity | R4 판정 | 재검증 1 판정 | 근거 |
|---|---|---|---|---|
| **C4-F5** 컨트롤러 잔여 바인딩 충돌 | S2 · systems | open | **closed** | §V1.2 |
| **C4-F8** `tech-verification/README.md` 가 T0 정본과 어긋남 | S2 · systems | open | **closed** | §V1.3 |
| **C4-F10** `asset-manifest.md` 스테일 주장 3건 | S2 · modeling | open | **closed** (QA 요구 문구 1건은 **자기정정**) | §V1.4 |
| **C4-F13** meta 해시 표 스테일 + 표 끊김 | S3 · systems | open | **closed** (배정 밖이나 같은 편집에서 해소, W6 로 확인) | §V1.3 말미 |
| **C4-F16** 도구 패널 표면 우선순위 부재 | S3 · systems | open | **open (범위 확대)** | §V1.6 |
| 신규 **C4-F19** | S2 · systems | — | open | §V1.6 |
| 신규 **C4-F20** | S3 · systems | — | open | §V1.6 |
| 신규 **C4-F21** | S3 · systems | — | open | §V1.6 |

**열린 S1 = 0** 이 유지된다. 그러나 open S2 는 **7 → 6**(F5·F8 closed, F19 신규)로만 줄었고, `NOT-MEASURED` 가 그대로이므로 **PASS 가능한 게이트는 여전히 0개**다.

## V1.2 C4-F5 — closed

요구 3항을 하나씩 실물에서 확인했다.

- **(1) `LB` 모디파이어 전용화 [OBSERVED, W3]**: §1 표 「도구 열기」 컨트롤러 열이 `LB`/`RB` 순환 → **`RB` 단독 순환**으로 바뀌었고, §1 불릿과 §1-2 표(L68)가 "`LB` 에는 단독 기능이 없다 · `LB` 를 누르는 것만으로는 어떤 명령도 발행되지 않는다"를 두 번 못 박았다. QA 1차가 지목한 실패 경로("패널 밖에서 되돌림하려고 `LB` 를 누르는 순간 도구가 먼저 순환한다")는 **문장 수준에서 소멸**했다.
- **(2) L37(현 L38) 3기능 3바인딩 [OBSERVED, W3]**: `back`(증거함) · `LB`+`back`(가설판) · `LB`+`RS`(힌트). 요구는 "3기능 3바인딩"이었고 그대로다. `RS` 단독을 오버레이에서 빼고 도구 패널 표면의 조회 기능(`wiring-trace` §1 · `tide-alignment` §1)에 남긴 선택은 **기존 두 스펙을 건드리지 않고** 충돌을 없애므로 QA 요구보다 보존적이다.
- **(3) `verification.matrix` 검수 행 [OBSERVED, W4·W5]**: 18번째 행(`matrix[17]`)이 QA 가 적은 문장("패널 밖 `LB` 입력 → 도구 순환과 되돌림이 동시에 발행되지 않는다")을 그대로 검사 대상으로 세웠다. 기존 17행 **수정·삭제 0건**, 스키마 검증기 exit 0.
- **요구를 넘어선 부분(가산)**: §0-11 불변식("한 입력 = 한 명령 … 명시되지 않은 겸용은 결함이다")과 §1-2 의 **누른 순간 고정(press-time latch)** 규칙. QA 는 개별 바인딩 정정만 요구했는데 레인은 **같은 결함이 다시 들어올 자리를 규칙으로 막았다**. 이 불변식은 아래 V1.6 신규 3건의 판정 근거로 QA 가 그대로 인용한다 — 레인이 스스로 세운 잣대로 잰다.
- **판정의 한계 [OBSERVED]**: 위는 전부 **계약 문장과 JSON 행의 존재**다. `matrix[17]` 는 **미실행**이고 패드 실측은 0건이다. C4-F5 closed 는 "설계 모순이 사라졌다"이지 "실제 패드에서 오발행이 없다"가 아니다. `interaction-rules.md` L209 가 이 한계를 스스로 적어둔 것을 확인했다.

## V1.3 C4-F8 — closed

- **§2.6 [OBSERVED, W7·W8]**: "대상: **T0 25분**(정본) + `c3-b2`~`c3-b3` 정합 구간(`dock`)". 정본 3자(§10 L115 · live `T0.minutes` · `campaign-time-budget.md` L359)가 일치한다. 판정 키 `total_minus_afk_min`(RFC-P3-011) 한 줄이 함께 들어왔고, "목표 밴드 450~540분은 본편 완주의 밴드이며 T0 25분과 비교 대상이 아니다"라는 축 분리까지 적혔다 — QA 가 요구하지 않았으나 C3-F14 계열 혼동을 막는다.
- **§2.5 [OBSERVED, W7]**: 캡처 명령이 `-captureScenes hub` 하나로 줄고, `gate` 의 첫 등장이 C1 `c1-b1` 이라는 데이터 근거와 "alignment 구간 씬은 `gate` 가 아니라 `dock`"이 주석으로 붙었다. QA 가 제시한 두 선택지(정정 또는 주석) 중 **둘 다** 이행됐다.
- **잔존 "30분" 2건은 결함이 아니다 [OBSERVED, W7]**: L218·L246 은 "이전 판은 30분이었다"는 **정정 이력 서술**이다. 이력을 지우면 CLAUDE.md §2(삭제 금지)와 충돌한다. 지시문(§2.5 명령 블록·§2.6 대상 행)에는 30분이 **0건**이다.
- **레인의 자기 제한을 QA 가 확인함**: §5 가 "고친 것은 *실행되지 않은 명령의 인자*이며 **새로 측정된 런타임 값은 0건**"이라고 스스로 적었다. QA 도 같은 판정이다 — 이 정정은 **G6·G5 를 1 mm 도 움직이지 않는다**. `PRE-1`(기준 PC `hw_profile_id`) 미정은 `[CARRIED]` 로 남는다.
- **C4-F13 도 함께 closed [OBSERVED, W6]**: `game-ui-contract.meta.md` 의 해시 표가 (a) 4행 한 표로 다시 렌더되고 (b) `interaction-rules.md` 행이 재측정값 `3c901743…`·25,545 B 로 교체됐다. QA 가 `shasum` 4종을 다시 돌려 **4/4 문자 일치**를 확인했다. 단위도 `wc -c` 바이트와 문자 수 두 열로 분리됐다. C4-F13 은 배정 밖이었으나 같은 편집에서 해소됐고 측정으로 확인되므로 **닫는다** — 열어 두면 등록부가 실물보다 나쁘게 틀린다.

## V1.4 C4-F10 — closed, 다만 QA 요구 문구 하나를 자기정정한다

- **L21 재작성 [OBSERVED, W9]**: "`concept/sheets/` 파일 수 = 0 … 컨셉 시트가 있는 자산은 하나도 없다"가 실측 3종(sheets 1 · PNG 45 · provenance 6)으로 교체됐고, 여전히 참인 값(`scene-boards` 0)은 **그대로 유지**됐다. 스테일을 지우면서 참인 것까지 지우지 않았다.
- **L135 / OPEN-M3 closed [OBSERVED]**: `style-guide.md` 는 존재한다. 여기서 레인이 QA 보다 한 걸음 더 갔다 — 정본을 읽은 결과 **"도구별 강조색"이라는 개념 자체가 기각**된다(§2 난색 악센트 1색 + 예비 1 "무단 확장 금지", §8 `color_only_encoding: forbidden`, 도구 구분의 정본은 §9 형상·글리프 6종). QA 가 `W11` 로 그레이박스 9색과 팔레트의 교집합이 **0/9** 임을 독립 재현했다. 임시 6색을 "최종 재질 후보"에서 "뷰포트 디버그 값"으로 격하한 것은 **측정에 근거한 판단**이다.
- **L133 / OPEN-M1 closed [OBSERVED, W10]**: glossary §7 파생 규칙이 이미 판정했고, 같은 문장이 `pipeline.md` L71 에도 있어 두 곳을 함께 닫은 것은 옳다 — 한쪽만 닫으면 같은 레인 안에서 current↔current 모순이 된다.
- **OPEN-M4 재판정 [OBSERVED]**: §0.1 대응표가 행별 소유권을 갖는다. 해제 = 셸 4 + 도구 4, 보류 = `SM_Gate3_Shell`·alignment/corrosion 최종 형상·공용 소품 30·초상 5/UI 1(레인 밖). 보류 사유가 "시트가 없어서"에서 "**시트가 스스로 재생성을 요구해서 / 전용 시트가 없어서**"로 좁혀진 것이 이 결함이 요구한 전부다.
- **수량 계약 불변 [OBSERVED]**: §6 표 `5+6+5+30+1 = 47` 이 `asset-budget.md` L13~19 와 계속 일치한다. 이번 개정이 수량을 건드리지 않았음을 확인했다.
- **QA 자기정정 — 레인 counter 수용**: QA 요구 문구는 "OPEN-M4 를 '시트 존재 → 차단 해제, 다만 sheets/README §9 재생성 우선순위 **1~3** 선행'으로 재판정"이었다. 모델러의 반박이 옳다 [OBSERVED]: §9 1순위 `readme-verb-seal` 은 **저장소 README 삽화**(§6), 2순위 `capsule-library-candidate` 는 **Steam 캡슐 후보**(§5)이며 두 자산에 대응하는 매니페스트 행은 47행 중 **0개**다. 모델링 착수를 실제로 막는 것은 3순위 `space-gate-three-mood` 하나(= `SM_Gate3_Shell` 의 유일한 `concept_ref`)다. **QA 의 "1~3" 은 과잉이었고 §0.1 의 좁힌 판정이 정확하다.** 1·2순위는 여전히 살아 있지만 그 소유는 concept/presentation 레인(C4-F11)이다.
- **레인이 자율 정정한 레인 밖 1건 — 수용**: `pipeline.md` L134 "`animation/rig-requirements.md` 는 아직 없다"는 실제로 존재(21.6 KB · `status: draft` · owner game-animator)하므로 스테일이었다. **문구만 실측으로 교체하고 Mixamo 표 재정렬·ack 는 하지 않은 것**이 옳은 처리다 — draft 를 근거로 current 표를 재정렬하면 C3-F33 이 막으려던 유형이 된다. "편차 검사 실적 0건"을 명시한 것도 정확하다.

## V1.5 파생 편집 2건 — C4-F5 해소의 일부로 **수용**

레인이 판정을 요청한 항목이다. `system-specs/hint-system.md` L25(`RS` → `LB`+`RS`)와 `system-specs/plate-readout.md` L29(`LB/RB` → `D-Pad ←/→`)는 **C4-F5 해소의 일부로 받는다.** 근거 셋.

1. 두 줄을 고치지 않으면 §1-2 의 규칙이 `interaction-rules.md` **안에서만 참**이 된다 — 힌트가 `RS` 단독으로 남으면 3기능 3바인딩이 도구 패널 표면에서 깨지고, `LB` 단독 스텝이 남으면 "`LB` 는 모디파이어 전용"이 데이터와 모순된다. 결함의 정의가 "문서 간 모순"인 이상 **한 파일만 고친 상태는 미해소**다.
2. 두 파일 모두 **같은 레인 소유 · 같은 `cycle` 값 제자리 개정**이며 `status`·`supersedes` 무변경 — RFC-Q2 요건을 만족한다.
3. QA 가 W6 로 두 파일 해시를 재측정했고, 두 파일의 **키보드 열은 변경 0**이다(`F1`·타임라인 드래그 그대로). 접근성 축에 부작용이 없다.

**다만 별도 확인 대상으로도 남긴다**: 두 파일은 `status: current` 이므로, 이 개정으로 C4-F16·C4-F19(아래)가 가리키는 표면 충돌이 *같은 표에서* 여전히 열려 있다. 이번 수용은 "이 두 줄이 C4-F5 를 닫았다"는 뜻이지 "두 파일이 검증됐다"가 아니다.

## V1.6 신규 결함 3건 + C4-F16 범위 확대 — 레인 질문에 대한 QA 판정

레인은 "키보드 축 발견(`wiring-trace` `I`)을 C4-F5 잔여로 볼지 신규로 열지" 판정을 요청했다. **판정: C4-F5 잔여가 아니다.** C4-F5 의 요구 3항은 전건 이행됐고 그 축(`LB`·`back`/`RS`)에서 닫혔다. 새로 측정된 것은 **다른 축(도구 패널 표면 · 키보드 · 지속시간)** 이며, 아래처럼 **C4-F16 을 확대하고 신규 3건을 연다**. 판정 잣대는 레인이 이번에 세운 `interaction-rules.md` **§0-11**("명시되지 않은 겸용은 결함이다")과 기존 **§0-8**(키보드 단독 완결) · **§0-9**(홀드는 옵션)다.

### C4-F16 — 범위 확대 (S3 유지 · systems)

1차 범위는 "`Space` 전역 프리뷰와 초점 활성화의 우선순위 부재"였다. **키보드·표면 축 전체로 넓힌다** [OBSERVED, W14]:

| 입력 | `interaction-rules.md` 의 단독 의미 | 도구 스펙에서의 다른 의미 |
|---|---|---|
| `X`(패드) | 프리뷰(§1-2 "모든 화면") | `drainage-routing` L31 가상 시험 · `corrosion-budget` L48 시험 실행 · `dual-seal` L29 프리뷰 · `tide-alignment` L29 자동 제안 · `wiring-trace` L30 구획 접기 · `plate-readout` L31 재생(판독) — **6가지** |
| `Space` | 프리뷰 | 위 6행의 KB 열 대부분 = 각 도구의 다른 동사 |
| `Y`(패드) | 패널 안 = 연결 해제 | `plate-readout` L32 인용 고정 |
| `H` | 가설판 열기 | `tide-alignment` L31 선후 판정 조회 |
| `I` | 증거함 열기 | `wiring-trace` L31 근거 유효성 조회 |

`interaction-rules.md` §1-2 L93 이 이 잔여를 스스로 "**아직 없다**"고 적었고 QA 는 그 정직성을 인정한다. 다만 **범위가 `Space` 한 키가 아니라 5키·6스펙**이라는 것이 이번 측정으로 확정됐다. 요구 수정: 초점 스코프(셸 표면 / 도구 패널 표면) **우선순위 규칙 1절**을 §1-2 옆에 세우고, 각 도구 스펙 §1 이 그 절을 인용한다.

### C4-F19 — `drainage-routing.md`(status: current) L29 가 `X` 금지 문장과 정면 충돌 (S2 · systems)

- [OBSERVED, W14] `system-specs/drainage-routing.md` L29 「연결 해제 | 우클릭 | **`X`** | `RemoveEdge`」.
- [OBSERVED] `interaction-rules.md` L41 = "**`X` 단독은 모든 화면에서 프리뷰 전용**이다. **배선 해제는 `X` 를 쓰지 않는다**" · §1 표 「연결 해제」 = KB `Delete` / 패드 **패널 안에서만 `Y`**.
- [OBSERVED] 같은 파일 L31 「가상 시험 | `Space` | **`X`**」 — **한 표면·한 표 안에서 `X` 가 해제와 프리뷰 두 의미**를 갖는다. 이것은 §0-11 이 "결함"이라고 정의한 바로 그 상태이며, F1·C4-F5 가 `X`/`Y`·`LB` 에서 닫은 것과 **같은 유형의 3번째 재발**이다.
- [OBSERVED] 같은 행의 KB 열이 **"우클릭"뿐** — 키보드 단독 경로가 그 행에 없다(§0-8). `interaction-rules.md` §1 이 `Delete` 를 주므로 **전역으로는 성립**하지만, 도구 스펙 표만 읽는 구현자는 그 경로를 찾지 못한다.
- 왜 S2 인가: 이 파일은 `status: draft` 가 아니라 **`status: current`** 이고, 승격 대기 중인 `interaction-rules.md` 가 올라가는 순간 **current↔current 정면 모순**이 된다(C4-F9 와 같은 구조). blocker 가 아닌 이유는 §1 이 키보드 경로를 전역으로 보장하기 때문이다.
- 요구 수정: L29 패드 열 `X` → **`Y`(패널 안)** 로 교체하고 KB 열에 `Delete` 를 명시. `routing` 의 가상 시험은 `X` 로 남긴다(프리뷰 계열이므로 §1-2 와 일치).

### C4-F20 — 지속시간(홀드·롱프레스) 축이 §0-11 의 해소 방식에 없다 (S3 · systems)

- [OBSERVED] §0-11 이 인정하는 겸용 해소는 **(a) 모드 분리 · (b) 모디파이어 레이어** 둘뿐인데, 실제 문서에는 **지속시간으로 분기하는 바인딩이 3건** 있다: `interaction-rules.md` §1-2 L69 「`RB` | 도구 순환(**짧게 다음 · 길게 이전**) | — | **단일**」(해소 방식을 "단일"이라 적었으나 명령은 2개) · `dual-seal.md` L30 「서명(확정) | `Enter` **길게 0.4 s** | `A` **길게 0.4 s**」 · `tide-alignment.md` L30 「기준선 확정 | `Enter` **길게 0.4 s** | `A` **길게 0.4 s**」.
- [OBSERVED] 뒤 두 건은 §0-9("**길게 누름은 선택 기능이다.** 기본값은 2단계 확정")와 충돌한다. `dual-seal.md` 는 **같은 파일 L77** 에서 "길게 누름은 필수가 아니다 — 확정 기본값은 `two-step`, 홀드는 opt-in"이라고 적어 **자기모순**이다. `tide-alignment.md` 에는 그런 주석조차 없다. 올바른 형태의 본보기는 `drainage-routing.md` L32(확정 행에 "길게 누름은 `hold` opt-in 에서만" 주석)다.
- 왜 S3 인가: 전역 불변식(§0-9)과 계약 `accessibility.hold_alternative` 가 이미 옵션임을 선언하므로 **접근성 결론은 뒤집히지 않는다**. 틀린 것은 두 스펙의 표기다. 다만 F1(blocker)이 닫힌 근거가 바로 그 불변식이므로 **표기를 고치지 않으면 F1 의 closed 가 스펙 층에서 약해진다**.
- 요구 수정: (1) §0-11 에 세 번째 해소 방식(**지속시간 레이어** — 짧게/길게가 서로 다른 명령일 때의 조건)을 명시하거나 `RB` 롱프레스 역순을 삭제한다. (2) `dual-seal.md` L30 · `tide-alignment.md` L30 을 `drainage-routing.md` L32 형태(기본 `two-step` + 홀드 opt-in)로 통일한다.

### C4-F21 — 도구 스펙 §1 입력 표 41행 중 21행에 키보드 경로가 없다 (S3 · systems)

- [OBSERVED, W14] 8개 스펙의 §1 입력 표 **41행** 중 **21행**의 KB 열에 키보드 토큰이 하나도 없다(드래그·클릭·우클릭·휠·hover·"메뉴 → 슬롯"·"증거함 → 좌·우 트랙" 등). 파일별: `drainage-routing` 3 · `dual-seal` 3 · `plate-readout` 3 · `save-undo` 4 · `tide-alignment` 2 · `wiring-trace` 2 · `hint-system` 2 · `corrosion-budget` 2.
- **과장하지 않는다**: `interaction-rules.md` §1 이 전역 대응(조사 = 초점 이동 후 `Enter`, 값 미세 조절 = 방향키 1스텝, 다중선택은 하나씩)을 주므로 **다수 행은 파생 가능**하다. 결함은 "키보드로 못 한다"가 아니라 **"어느 행이 어떤 전역 규칙으로 파생되는지 스펙에 없다"**는 것이며, 특히 연속값 3행(`plate-readout` L29 타임라인 드래그 · L30 휠 배율 · `tide-alignment` L27 곡선 위 클릭 ×3)은 **C4-F12 가 지적한 "연속값 이산 대안 미기재"와 같은 구멍**이다.
- 요구 수정: 각 스펙 §1 표에 "키보드 단독 경로는 `interaction-rules.md` §1 의 어느 행에서 파생되는가"를 1열 또는 각주로 붙인다. 파생 불가능한 행(연속값 3행)은 C4-F12 의 `accessibility` 신설 3키와 **같은 편집에서** 처리한다.

## V1.7 승격 판정 갱신 (C3-F33 · RFC-Q2) — §R4.4 를 대체하지 않고 **덮어쓰는 델타**

| 파일 | R4 판정 | 재검증 1 판정 | 사유 |
|---|---|---|---|
| `systems/interaction-rules.md` | 차단 | **차단 유지** | C4-F5 는 closed 됐으나 **C4-F9**(도구 표시명이 `gdd.md`·`style-guide.md` 두 current 와 다름)와 **C4-F16 확대**가 그대로다. C4-F20 (1)도 이 파일 소유 |
| `systems/game-ui-contract.json` | 차단 | **차단 유지** | C4-F12 미착수 · C4-F9 · **C4-F14 SC-2 오타 2건 잔존 재확인**(`matrix[12]` "도구 **휴**은" · `matrix[15]` "출처 중복**로**", W4 로 문자 확인) |
| `systems/game-ui-contract.meta.md` | 차단 | **차단 유지** | 자기 결함 C4-F13 은 closed 이나 기술 대상 JSON 이 차단이다. meta 의 「미묶음 경고」는 정확한 자기 고지다 |
| `systems/unity-implementation.md` | 차단 | **차단 유지** | C4-F6(RFC-S3 디렉터 미판정) · C4-F14 SC-3. 이번 루프에서 이 파일은 **불변**(W6 해시 동일) |
| `animation/animation-contract.md` | 승격 가능 | **승격 가능 [CARRIED]** | 이번 루프에서 **불변**(`ec3ad8c1…` W6). R4 판정이 그대로 유효 |
| `economy/resources-and-fairness.md` | 승격 가능 | **승격 가능 [CARRIED]** | 이번 루프 편집 대상 아님. R4 판정 유지 |
| `systems/prototype/README.md` · `prototype.meta.md` | 승격 가능 | **승격 가능** | W15 로 37/37 재현 |
| `motion/motion-contract.md` · `vfx/vfx-budget.md` | 차단 | **차단 유지** | C4-F7 미착수 |
| `balance/puzzle-balance.md` · `presentation/video-study.md` | 차단 | **차단 유지** | 사유 불변 |

**이미 `status: current` 인 파일의 정정 상태**: `systems/tech-verification/README.md` **정정 완료**(C4-F8 closed) · `modeling/asset-manifest.md`·`pipeline.md` **정정 완료**(C4-F10 closed). 반대로 `system-specs/drainage-routing.md`·`dual-seal.md`·`tide-alignment.md`·`wiring-trace.md` 는 **이번 측정으로 새 정정 대상이 됐다**(C4-F19·F20·F16).

## V1.8 브로드캐스트 (dependency-matrix ● 항목) · `feedback-requested-by: 2026-09-11`

| 받는 레인 | 항목 | 요청 |
|---|---|---|
| game-systems-designer | C4-F19(S2) · C4-F16 확대 · C4-F20 · C4-F21 | `drainage-routing.md` L29 를 `Y`/`Delete` 로 · 표면 우선순위 1절 신설 · 확정 홀드 표기 2건 통일 · §1 표에 키보드 파생 근거 열. **다음 루프에 C4-F9/F12/F14 SC-2 와 묶어 JSON 을 한 번만 고칠 것**(meta 해시 갱신 총 2회로 끝난다 — meta 「미묶음 경고」와 같은 결론) |
| game-production-director | C4-F6(RFC-S3) · C4-F14 SC-4 · C4-F11 | 판정 미제출 상태 그대로. C4-F5·F8·F10 closed 는 이 셋을 움직이지 않는다 |
| game-modeler | OPEN-M5 · OPEN-M6 | QA 는 두 신규 항목을 **결함으로 올리지 않는다** — 스스로 열고 소유자를 밖으로 지목했다. 다만 OPEN-M6 은 concept·presentation 회신 없이는 닫히지 않으므로 RFC 로 올릴 것 |
| game-concept-artist | OPEN-M5 · C4-F11 | 공용 소품 30 의 (a) 소품 키트 시트 생성 또는 (b) "§8 constraints + 공간 무드로 충분" 명시 회신. 모델러가 (b)를 대신 선언하지 않는 것은 옳다 |

## V1.9 이 재검증이 증명하지 않는 것 [OBSERVED]

- **`matrix[17]` 는 미실행이다.** C4-F5 closed 는 문장·JSON 행의 무모순이며, 실제 패드에서 `LB`+`X` 가 도구 순환을 발행하지 않는다는 증거는 **0건**이다.
- **C4-F8 은 실행되지 않은 명령의 인자를 고쳤다.** 프레임타임 캡처 0건, `PRE-1` 기준기 미정. G5·G6 은 `NOT-MEASURED` 그대로다.
- **C4-F10 은 차단선을 옮겼을 뿐 자산을 만들지 않았다.** 47종 중 greybox 7 · pending 40 · `runtimeEligible:false` 전건. 컨셉 45장은 **컨셉·프리비즈이며 게임플레이가 아니다**.
- **이번 루프에서 새로 측정된 런타임 값은 0건이다.** G1~G8 은 **0/8 PASS 유지**이며 이 절은 `qa/gate-measurements.md` 의 어떤 값도 PASS 방향으로 올리지 않는다.
- **익스플로잇·회귀 매트릭스·몰입 점수·플레이테스트 보고서는 이번에도 없다.** 빌드와 표본이 0인 상태에서 그 문서를 만드는 것은 측정 위장이다(`qa/defect-register.md` §4 유지).

---

# 재검증 2 (2026-09-10, R4 수정 루프 2 이후) — game-qa

> 같은 사이클 안의 **제자리 개정**이다(RFC-Q2). `cycle`·`status`·`supersedes` 는 그대로 두고 이 절만 append 했다. 위 1차 본문 · 「재검증 (2026-09-10, R4)」 · 「재검증 1」 절은 **당시 기록으로 보존**한다(CLAUDE.md §2 삭제 금지).
> **배정**: `C4-F19`(S2 · systems) **1건**. 레인 회신 = 「이행, 반론 0건」 + 자기보고 질문 3건.
> **방법**: 레인이 보고한 숫자는 근거로 쓰지 않는다. 아래 **X1~X14 는 전부 QA 가 직접 실행**했고, 값이 어긋난 곳은 어긋난 대로 적는다.

## V2.0 재측정 명령 원문 [OBSERVED 2026-09-10, QA 직접 실행]

| id | 명령 | 결과 |
|---|---|---|
| X1 | `git status --short` | `_workspace/current/systems/` 는 **디렉터리 한 줄 `??`**(전체 미추적), `_workspace/current/qa/` 는 파일별 `??`. 본 세션 외 동시 편집 표시 없음 |
| X2 | `grep -n "" system-specs/drainage-routing.md \| sed -n '24,35p'` | L29 = `\| 연결 해제 \| 우클릭 / `Delete`(키보드 단독) \| **패널 안에서만 `Y`** \| `RemoveEdge` \|` · L31 = `\| 가상 시험 \| `Space` \| `X` \| Preview 실행 \|` — **행 번호 불변** |
| X3 | `grep -n "" interaction-rules.md \| sed -n '30,60p'` | L34 정본 = 「연결 해제 \| 우클릭 \| `Delete` \| **패널 안에서만 `Y`**」 · **L41 금지 문장 원문 불변** |
| X4 | `sed -n '127p' interaction-rules.md` (§2.4) | 「해제는 우클릭 / `Delete` / 패널 내 `Y`」 — 스펙 L29 와 3자 일치 |
| X5 | `for f in system-specs/*.md; do awk '/^## 1\. 입력/…' "$f"; done \| wc -l` | **41행 / 8파일** — corrosion 4 · drainage 6 · dual-seal 6 · hint 4 · plate 5 · save-undo 5 · tide 6 · wiring 5. `interaction-rules.md` L85 의 분포와 **문자 일치** |
| X6 | 같은 출력 → `awk -F'\|' '$3 ~ /X/'` (문서가 적은 필터 그대로) | **8행** — 단독 `X` 6행 + **`LB+X` 2행**(`dual-seal` L31 해제 · `save-undo` L32 되돌림/다시). 문서가 말한 6행이 나오려면 `&& $3 !~ /LB\+X/` 가 **필요하다** → **신규 C4-F22** |
| X7 | 같은 출력 → `$3 ~ /`X`/ && $3 !~ /LB\+X/` | **6행** — corrosion L48 · drainage L31 · dual-seal L29 · plate L31 · tide L29 · wiring L30. §105 의 6행 **열거·행번호·이름 전부 일치** |
| X8 | 같은 출력 → 패드 열 `` `Y` `` | **3행** — drainage L29(신규) · L33(`LB+Y`) · plate L32. 「연결 해제 = `X`」 행은 **0건** |
| X9 | `shasum -a 256 interaction-rules.md system-specs/drainage-routing.md game-ui-contract.json` | `4f6b25468e06f476…4ef5` · `389834d30d060e67…ffef` · `9c89e9aee0961342…bc5da`(**JSON 불변**) |
| X10 | `wc -c` + `LC_ALL=en_US.UTF-8 wc -m` (같은 3파일) | 28,808 B / 15,975자 · 11,165 B / 7,077자 · 38,429 B / 23,006자 — `game-ui-contract.meta.md` 해시 표 **4행 전부 문자 일치**(스테일 0) |
| X11 | `bash .claude/skills/game-ops-harness/scripts/freshness-check.sh` | `0 finding(s) across 96 markdown artifact(s)` · **exit 0** (재검증 1 = 95, 증가분 1 = 신설 검증 파일) |
| X12 | `node _workspace/current/planning/validate-campaign.mjs` | `checks 47 · pass 47 · fail 0 · PASS`, sha `92301c0a5ecfc7e1…ae23` · **121,457 B** · exit 0 — **정본 입력 불변** |
| X13 | `node _workspace/current/systems/prototype/test-model.mjs` | **37 통과 / 0 실패** · exit 0 — 코드 0줄 변경 대조 |
| X14 | `sed -n '153p;528p;620p' game-ui-contract.json` | L153 「패널이 열린 동안 X는 프리뷰 Y는 해제로 고정된다」(**정본 쪽**) · L528 「도구 **휴은**」(SC-2 기존) · L620 「우클릭과 **덴리트**와 패널 내 Y로 **옥긴다**」(**신규 오타 2건**) |
| X15 | 같은 41행 → KB 열에 백틱 키 토큰이 없는 행 | 기계 필터 **18행**. 의미 기준으로는 `corrosion-budget` L47 `` `routing` ``·L50 `` `reader` ``(백틱이지만 **키가 아니다**) 2행을 더해 **20행** — 재검증 1 의 **21행에서 1행 감소**(drainage L29 가 `Delete` 를 얻었다) |

## V2.1 판정 요약

| id | severity | 재검증 1 판정 | **재검증 2 판정** | 근거 |
|---|---|---|---|---|
| **C4-F19** | S2 · systems | open (신규) | **closed** | X2·X3·X4·X8 — 요구 3항 전부 이행, 잔여 겸용 0 |
| C4-F14 SC-2 | S3 · systems | open (오타 2건) | **open · 범위 확대(오타 4건)** | X14 — L620 「덴리트」·「옥긴다」 실재 확인 |
| C4-F21 | S3 · systems | open (41행 중 21행) | **open · 20행으로 축소** | X15 — 요구는 "행별 파생 근거"이므로 감소로 닫히지 않는다 |
| C4-F16 | S3 · systems | open (5키·6스펙) | **open · 불변** | X7 — 단독 `X` 6행 그대로. C4-F19 가 닫은 것은 이 6행 밖의 1행 |
| C4-F20 | S3 · systems | open | **open · 불변** | `dual-seal` L30 · `tide-alignment` L30 「길게 0.4 s」 문자 잔존 |
| **C4-F22** | S3 · systems | — | **open (신규)** | X6 · X1 — 문서가 적은 재도출 명령이 문서가 적은 결론을 내지 않는다(2곳) |

**열린 S1 = 0** 유지. **이번 루프에서 새로 측정된 런타임 값 0건** → `qa/gate-measurements.md` 의 어떤 값도 움직이지 않았다(G1~G8 **0/8 PASS 유지**).

## V2.2 C4-F19 — **closed**

QA 가 결함에 적은 5개 주장을 하나씩 다시 잰다.

| # | 주장 | 재검증 2 실측 | 판정 |
|---|---|---|---|
| 1 | L29 패드 열이 `X` | X2 — 「**패널 안에서만 `Y`**」로 치환, **L29 행 번호 불변** | **해소** |
| 2 | L41 「배선 해제는 `X` 를 쓰지 않는다」와 정면 충돌 | X3·X8 — 스펙 41행 어디에도 「연결/배선 해제 = `X`」 **0건**. L41 원문은 손대지 않았다 | **해소** |
| 3 | 같은 표 L31(가상 시험 `X`)과 한 표 안 겸용 | X2 — L31 유지, L29 는 `Y`. 한 표 안에서 `X` 가 두 명령을 내는 상태 소멸 | **해소** (지시대로 L31 유지) |
| 4 | 그 행 KB 열에 키보드 경로 없음(§0-8) | X2 — 「우클릭 / `Delete`(키보드 단독)」. `Delete` 는 §1 정본 행의 인용이지 새 바인딩이 아니다(X3) | **해소** |
| 5 | `status: current` 파일이라 승격 시 current↔current 모순 | 레인이 **QA 전제 하나를 정정**했다: `interaction-rules.md` 는 `current` 가 아니라 **`draft`** 다. 모순은 "지금"이 아니라 "승격되는 순간" 성립한다 — 결함 등급 S2 는 바뀌지 않지만 **QA 문장이 부정확했다** | **QA 자기정정** |

- **md↔json 드리프트도 함께 닫혔다** [OBSERVED X14]: 계약 JSON L153 은 이미 「X는 프리뷰 Y는 해제」 쪽이었다. 즉 C4-F19 는 JSON 결함이 아니라 **스펙 md 단독 스테일**이었고, 이제 md 3곳(스펙 L29 · 규칙 §1 L34 · 규칙 §2.4 L127)과 JSON 1곳이 같은 문장을 말한다.
- **부수 확인**: `dual-seal` L31 「해제 \| `Ctrl+Z` \| `LB+X`」는 §1-2 의 `LB+X`=되돌림과 **의미가 같다**(같은 행 결과 열 = "확정 취소") — X6 이 잡아낸 2행은 충돌이 아니다.
- **닫힌 것의 크기**: 41행 중 **1행**이다. 이 절은 `drainage-routing.md` 를 "검증된 파일"로 만들지 않는다 — 같은 파일에 C4-F16(L31) · C4-F21(L28·L30) · C5-F5 잔존(§3 R-R2·§4 R-F4 「매체 경로」 2곳)이 열려 있다.

## V2.3 레인 자기보고 질문 3건 — QA 판정

**(1) `LB`+`Y` 를 §1-2 전건 표에 올린 편집 → 수용. 별도 결함으로 열지 않는다.**
- 근거: `drainage-routing.md` L33 이 이미 `LB+Y`(Fork)를 쓰고 있고 **같은 절의 대조 표가 그 행을 「일치」로 판정**하고 있었다(X5 출력에 L33 존재). 즉 전건 표의 "—"는 **표가 자기 대조 결과와 어긋난 상태**였고, 이 편집은 없던 바인딩을 만든 것이 아니라 **있는 바인딩을 표에 적은 것**이다. 스펙 파일 변경 0(X9 — drainage 해시 변경분은 L29 + 변경 로그로 설명된다).
- **관찰(결함 아님)**: §1 본문의 모디파이어 목록(L37·L38)에는 `LB+Y` 가 없다. 그러나 §1-2 L60 이 스스로 "이 표는 **패드 입력 전건**"이라고 선언하므로 전건의 소재지는 §1-2 이며 모순이 아니다. 다음 회차가 §1 만 읽고 "`LB+Y` 는 없다"고 결론내지 않도록 여기 남긴다.

**(2) `game-ui-contract.json` L620 오타 2건 → 신규 id 를 열지 않고 `C4-F14 SC-2` 를 확대한다.**
- [OBSERVED X14] L620 = 「R1 X는 항상 프리뷰로 고정하고 해제를 우클릭과 **덴리트**와 패널 내 Y로 **옥긴다**」 — `덴리트` = `Delete`, `옥긴다` = `옮긴다`. `matrix[12]` 「도구 휴은」·`matrix[15]` 「출처 중복로」와 **같은 파일·같은 계열**이다.
- 판정 근거: 등록부와 V1.8 이 이미 "**JSON 은 C4-F9/F12/F14 SC-2 와 묶어 한 번만 수정**"을 지시했다. 새 id 를 열면 같은 파일에 대해 닫는 단위가 하나 더 늘고 meta 해시 갱신 횟수만 늘어난다. 따라서 **SC-2 의 대상을 오타 2건 → 4건으로 확대**하고 등록부 evidence 를 갱신한다.
- 레인은 이번 배정 밖이라 JSON 을 건드리지 않았다 — **옳은 판단**이다(X9 로 JSON 해시 불변 확인).

**(3) C4-F21 잔여 3행 → 2행 축소 → 사실로 수용, 그러나 C4-F21 은 `open` 유지.**
- [OBSERVED X15] 전체 **21행 → 20행**, `drainage-routing` 만 3 → 2(노드 연결 드래그 · 밸브 토글 클릭).
- C4-F21 의 요구는 "**행별 파생 근거를 스펙에 적을 것**"이며 행 수 감소가 아니다. 두 행이 §1 「커넥터 연결 = 2단계 선택」·「대상 조사 = 초점 후 `Enter`」로 파생 가능하다는 레인의 판단은 **QA 도 동의**하지만, 그 파생이 **스펙에 적히기 전까지는 `[INFERENCE]`** 다.
- **배정 요청에 대한 답**: 행 단위 파생 표기를 **C4-F16 표면 우선순위 절과 같은 편집에 묶는 것을 승인**한다. 두 작업 모두 6~8개 스펙의 §1 표를 동시에 건드리므로 나누면 같은 표를 두 번 여는 셈이 된다.

## V2.4 신규 결함 — C4-F22 (S3 · systems)

### C4-F22 — 문서에 적힌 재도출 명령이 문서에 적힌 결론을 내지 않는다 (2곳)

- **(a)** [OBSERVED X6] `interaction-rules.md` **L86** = 「(같은 출력을 `awk -F'\|'` 로 **패드 열에 `` `X` `` 가 있는 행만 거르면 6행**)」 · `tech-verification/c4-fixloop2-input-binding.md` **W-2** = 「패드 열에 `` `X` `` 가 있는 행만 필터 → **6행**」. 문자 그대로 실행하면 **8행**이다 — `dual-seal` L31(`LB+X`) · `save-undo` L32(`LB+X`) 가 함께 걸린다. 6행이 되려면 **`&& $3 !~ /LB\+X/`(단독 `X` 한정)** 조건이 필요하며 두 문서 어디에도 그 조건이 없다.
- **왜 결함인가**: §105 는 그 6행을 「**`X` 잔여 전수**」라고 부른다. 다음 회차가 명령을 그대로 돌리면 8행을 보고 "전수가 틀렸다"고 읽거나, 반대로 6행을 믿고 `LB+X` 2행을 **패드 `X` 축에서 누락**한다. 이것은 C4-F19 를 낳은 실패 유형(루프 1의 거짓 「전건 대조」)과 **같은 축**이되, 이번엔 **결론(6행 열거)이 정확하고 서술만 부정확**하다 — 그래서 S2 가 아니라 S3 다.
- **(b)** [OBSERVED X1] 같은 검증 파일 §5 = 「다른 레인 파일 쓰기 0건(`git status --short` 로 확인)」. 실제 `git status --short` 는 `_workspace/current/systems/` 를 **디렉터리 한 줄 `??`** 로만 출력한다(전체 미추적) — 그 명령으로는 어느 파일이 쓰였는지 **보이지 않는다**. 결론 자체는 X9·X10 해시로 QA 가 독립 확인했으므로 **사실은 맞다**; 틀린 것은 **근거로 든 명령**이다.
- **왜 `status: current` 파일이라 문제인가**: `c4-fixloop2-input-binding.md` 는 `status: current` 로 태어난 영수증이다. C4-F8(`tech-verification/README.md`)·C4-F13(meta 해시 표)과 같은 위치의 문서이며, **영수증의 명령이 재현되지 않으면 영수증이 아니다**.
- 요구 수정: (1) L86·W-2 의 필터를 조건까지 적거나(`$3 ~ /`X`/ && $3 !~ /LB\+X/`), **8행을 적고 그중 단독 6 · 모디파이어 2로 나눠** 적는다. (2) §5 의 근거를 `shasum` 전후 비교 또는 `find _workspace/current -newermt` 로 바꾼다. **어느 쪽도 결론을 바꾸지 않는다 — 바뀌는 것은 재현 가능성뿐이다.**
- owner: `game-systems-designer`.

## V2.5 승격 판정 갱신 — §R4.4·§V1.7 을 대체하지 않고 **덮어쓰는 델타**

| 파일 | 재검증 1 판정 | **재검증 2 판정** | 사유 |
|---|---|---|---|
| `systems/system-specs/drainage-routing.md` | (새 정정 대상) | **C4-F19 정정 완료 · 그러나 검증 완료 아님** | 이미 `current` 라 승격 대상이 아니다. 같은 파일에 C4-F16(L31) · C4-F21(L28·L30) · C5-F5 잔존(「매체 경로」 2곳)이 열려 있다 |
| `systems/interaction-rules.md` | 차단 유지 | **차단 유지 (+1)** | C4-F9 · C4-F16 확대 · C4-F20(1) 그대로 + **신규 C4-F22(a)**. 다만 §1-2 의 거짓 `[OBSERVED]` 가 제거된 것은 **개선으로 기록**한다 |
| `systems/game-ui-contract.json` | 차단 유지 | **차단 유지 (오타 2 → 4건)** | X14. C4-F12 · C4-F9 · C4-F14 SC-2 확대. 이번 루프 **불변**(X9) |
| `systems/game-ui-contract.meta.md` | 차단 유지 | **차단 유지 · 자체 스테일 0 재확인** | X10 으로 표 4행 전부 문자 일치. 측정 로케일 명시는 **RFC-Q1 이 요구하는 재현성의 모범**이다. 기술 대상 JSON 이 차단이라 판정은 그대로 |
| `systems/tech-verification/c4-fixloop2-input-binding.md` | (신규 파일) | **`status: current` 이나 C4-F22(a)(b) 보유** | 내용의 실질(§0 5항 인정 · §1 전후 원문 · §4 미해소 4건)은 QA 재측정과 **전건 일치**. 고칠 것은 두 문장의 명령 서술뿐 |
| 그 외 (`unity-implementation.md` · `animation-contract.md` · `motion/*` · `vfx/*` · `balance/*` · `presentation/*`) | — | **[CARRIED]** | 이번 루프 편집 대상 아님. X9 로 `453f8481…` · `ec3ad8c1…` 불변 확인 |

## V2.6 브로드캐스트 (dependency-matrix ● 항목) · `feedback-requested-by: 2026-09-11`

| 받는 레인 | 항목 | 요청 |
|---|---|---|
| game-systems-designer | **C4-F22(신규 S3)** · C4-F14 SC-2 확대(오타 4건) · C4-F16 + C4-F21 묶음 승인 | (1) L86·W-2 필터 조건 명시 + §5 근거 교체 — **한 편집**이면 끝난다. (2) JSON 은 **여전히 한 번만** 열 것: C4-F9 · C4-F12 · SC-2 **4건** 을 같은 편집에, meta 해시 동시 갱신. (3) 표면 우선순위 절(C4-F16)과 §1 행별 파생 근거(C4-F21)는 **같은 편집으로 묶는 것을 QA 가 승인**함 |
| game-production-director | C4-F19 closed · C4-F22 open · C4-F6(RFC-S3) · C4-F11 · C4-F14 SC-4 | C4-F19 는 닫혔으나 **게이트는 하나도 움직이지 않았다**. 미판정 3건은 그대로다 |
| game-presentation-director | (참고) | 이번 루프는 presentation 산출물을 건드리지 않았다 — `qa/c5-review.md` 「재검증 2」 절 참조 |

## V2.7 이 재검증이 증명하지 않는 것 [OBSERVED]

- **패드로 `Y` 를 눌러 배선이 끊기는 것을 본 적이 없다.** 검수 시나리오 `matrix[12]`(`X`/`Y` 축)·`matrix[17]`(`LB` 축) 은 **둘 다 미실행**이며 패드 실측 **0건**이다. C4-F19 closed 는 **네 문서가 같은 문장을 말한다**는 뜻뿐이다.
- **「가상 시험 `X` 는 프리뷰 계열이라 충돌이 아니다」는 `[INFERENCE]`** 다. 표면 우선순위 규칙(C4-F16)이 서기 전까지 `[OBSERVED]` 로 승격되지 않는다.
- **`drainage-routing.md` 의 나머지 §2~§8 은 이번에 검사하지 않았다.** 레인이 "한 글자도 바뀌지 않았다"고 적었고 해시 변화분이 L29+변경 로그로 설명되지만, 그 절들의 **내용 정합은 이번 배정 밖**이다.
- **G1~G8 은 0/8 PASS 유지.** 빌드 0회 · 플레이 표본 n=0 · 프레임타임 캡처 0건. 이 절은 `qa/gate-measurements.md` 의 어떤 값도 PASS 방향으로 올리지 않는다.
- **익스플로잇·회귀 매트릭스·몰입 점수·플레이테스트 보고서는 이번에도 없다**(`qa/defect-register.md` §4 유지).
