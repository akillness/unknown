---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-qa
---

# RFC-CX-012 독립 재검증 — draft→current 승격 판정 (2026-09-11, QA · 2차 재검증 반영)

## 0. 지위 · 범위 · 방법

- **왜 있는가**: `production/decision-log.md` RFC-CX-012 [DECISION] "전부 status: draft — QA 독립 재검증 후 승격 판정 별도"의 그 재검증이다. 레인 보고(ACK-a `planning/field-classification-20260911.md` · ACK-b `worldview/term-audit-20260911.md` 머리 + `glossary.md` R8/R9 · synopsis `t0-records.md`/`narrative-flow-bridges.md` ACK · ACK-c `systems/rfc-cx-012-ack.md`)를 **입력이 아니라 피검사 주장**으로 두고, 검증기·git·기계 스캔으로 다시 쟀다.
- **회차** [OBSERVED]: **1차**(23:0x~23:3x) — S1 0 · S2 3 · S3 3, HOLD 2. 디렉터가 정정 3레인(PlannerFix D-01/02 · SystemsApply D-04 + t0 2차 재생성 · WorldviewFix D-03/05/06 → glossary R9·timeline·term-audit)을 돌린 뒤 **2차**(23:4x~) — 같은 검사 전량 재실행. 아래 표는 2차 값이 정본이고 1차 값은 "(1차: …)"로 남긴다.
- **QA는 재고 판정만 한다** [OBSERVED]: 이 문서 외 쓰기 0건(2차도 이 문서 제자리 갱신만, RFC-Q2). `emit-evidence-graph.mjs` 2회 실행은 결정론 검사이며 두 회차 모두 실행 전·1회·2회 파일 sha가 동일해 저장소 내용 변화 0(§1 R1-4). `unity/`·`assets/`·git add/commit·포매터 무접촉. 음성 시험은 `/tmp` 사본에서만 실행 후 삭제.
- **승격 규칙**(CLAUDE.md §10.1 C4/C5 승격 규칙): 본 문서 §3의 PASS 행만 소유 레인이 같은 `cycle` 값으로 `status: current`로 올린다. S1 = 승격 차단.
- **sha 고정 숫자는 재기재하지 않는다**(RFC-Q1) — 아래 "일치"는 검증기 출력 문자열끼리의 동일성이다. D-01/02 정정으로 campaign sha가 1차와 달라졌고, t0·EG provenance는 2차 sha로 다시 묶였다(R5-3).
- 실행 환경: 저장소 루트, M9 세션(RFC-CX-011)이 `unity/**`를 병행 편집 중(§1 R7).

## 1. 검사 표 (R1~R7) — 2차 값

| # | 검사 | 명령 / 방법 | 관찰 [OBSERVED] | 판정 |
|---|---|---|---|---|
| R1-1 | validate-campaign 49검사 | `node _workspace/current/planning/validate-campaign.mjs` | `{'checks': 49, 'pass': 49, 'fail': 0, 'verdict': 'PASS'}` · exit 0 · beats 33 · clues 73 · totalMinutes 480 · originCatalogSize 31 (1차 동일) | PASS |
| R1-2 | `--pairs` | `… validate-campaign.mjs --pairs` | exit 0 · `proofRequiredBeats 17` · pairs 17 · `beatsWithoutPair []` | PASS |
| R1-3 | `--t0` 5검사 | `… validate-campaign.mjs --t0 _workspace/current/systems/data/t0` | `{'checks': 5, 'pass': 5, 'fail': 0, 'verdict': 'PASS'}` · `sourceShaMatchesLiveCampaign True` · T0-01~05 PASS · exit 0 — **2차 재생성본이 D-01/02 정정 후 campaign sha에 묶여 있음** | PASS |
| R1-4 | evidence-graph 결정론 | shasum → `node …/emit-evidence-graph.mjs` → shasum → 재실행 → shasum | 2차: 실행 전·1회 후·2회 후 sha **3값 동일**(1차와는 다른 값 — campaign sha 변경에 따른 정상 재발행). stdout 2회 `nodes 152 edges 277 (beat 33, clue 73, source 31, stage 9, tool 6)` · exit 0 | PASS |
| R1-5 | validate-evidence-graph 18검사 | `node _workspace/current/planning/validate-evidence-graph.mjs` | `EG-SUMMARY checks 18 pass 18 fail 0 PASS` · exit 0. 비공허 확인(1차 `/tmp` 음성 시험: 유령 비트·유령 originId 주입 → `EG-PRE-01 FAIL count=1 c1-b1->zz-b9` · `EG-REACH-01 FAIL count=1` · exit 2 · 저장소 무변경) | PASS |
| R1-6 | K-06 값 | R1-1 출력 + 필드 위치 스캔 | `{'c4-b3': False, 'c6-b4': True}` = expected. 문장 위치 `c6-b4.inference` 1곳, `c4-b3` 부재 (1차 동일) | PASS |
| R1-7 | validate-game-ui.py | 계약 경로 `_workspace/current/systems/validate-game-ui.py` **부재** → `python3 ~/.claude/skills/game-ui-ux/scripts/validate-game-ui.py _workspace/current/systems/game-ui-contract.json`(ACK-c 인용 경로 `~/.aside/u/0/skills/user/game-ui-ux/scripts/…`와 sha 동일) | `PASS: valid game UI contract` · exit 0 | PASS |
| R2-1 | 교체 대상 어휘 표시 필드 잔존 | python 스캔: `title/objective/subtasks[]/hints[]/recovery/clues[].description` × 22어(습도판·보관함·판정대·가상 운전·만조 피크·교차표·임시 운전 서류·제출 접수 대장·부두 냉동창고·자동 백업·훈련 화면·요약 화면·훈련 모드·플레이어·플래그·씬·에필로그·역매핑·확대 뷰·미리보기·T-19·체크포인트) + B-12/B-14 어구(`c3-b2 정합`·`c7-b3 대조표`·`T0에서`·`안내 표시가`·`안내가 붙`·`안내 없이`·`같은 화면`·`화면`) | 표시 필드 잔존 **0건**. completion 잔존 1건(`t0-b3.completion` "T0에서") = B-11/B-12 술어 분류 보존 — 위반 아님. 저작 주석 필드 잔존 22건 — 범위 밖 관찰(ACK-a §3과 일치) | PASS |
| R2-2 | 교체 실행 구조 대조 | `git show HEAD:…/campaign.json` vs live 리프 평탄화 diff | 변경 리프 **62**(subtasks 19 · objective 13 · recovery 11 · hints 7 · clues.description 7 · completion 5) · 키 추가/삭제 0 · 비문자열 0 · id 0 — ACK-a "62곳" 일치. D-01/02 정정은 기존 변경 리프 2개 안의 조사 수정이라 리프 수 불변 | PASS |
| R2-3 | 교체 문장 조사 일치 | 교체어 24종 뒤 조사(로/으로·을/를·이/가·은/는) 받침 규칙 자동 검사, 전 표시+completion 필드 | **불일치 0건**. `c4-b2.subtasks[3]` "판독기 확대 판독**으로** 드러난 획을" · `c6-b4.objective` "결론 문장의 역대조**로** 수치화하고" 정정 확인 (1차: 2건 FAIL → D-01·D-02) | PASS |
| R3-1 | glossary 신설 행 실재 | glossary 표 행 파싱 HEAD vs live | 추가 행 **12** = R8 9(§2 판독대·조습기·구역 지도·서명 요건표·1호기 / §3 확정 전 보존·복귀 지점·연습 압착·연습 서식) + **R9 3**(§3 보호 지정·공통 종결부·종결부 후일담). 삭제 0. §7 머리글 파생 규칙 2줄(R8 「철」 · R9 「어간 파생 면제」) 실재 | PASS |
| R3-2 | 유지 13행 침해 0 | (i) §C 27명사 행 HEAD↔live (ii) campaign 표시+completion 출현 횟수 HEAD↔live | (i) 26/27 문자열 동일. **1행 변경 = 「공통 조위 피크」**: 명칭·정의 본문("두 계통이 함께 기록한 조위 극값. 정합의 기준점(3개 필요)") 그대로, 뒤에 "(문맥에서 '공통'이 확정된 뒤는 「조위 피크」로 축약 가능 — R9 · D-CX012-06)" 1절 **부기** — 개명 0·재정의 0, 축약 허용 주석 추가(디렉터 판정, RFC-Q2 제자리). (ii) 변동 확정+8·연습+6·판독+1·조위정합+1·공통 조위 피크+1만(1차 동일) | PASS(주석) |
| R3-3 | 교체어·노출 고유명사 glossary 등재 | 교체로 표시 필드에 들어간 어휘 24종 + t0-records 노출 2행 + bridges §2 사용분 멤버십 | 24/24 해소: 행 22(1차 19 + R9 3: 보호 지정·공통 종결부·종결부 후일담) + **§7 어간 파생 면제 규칙** 명문화로 「역대조」(대조 어간)·「확대 판독」(판독 어간) 면제(규칙문에 예시로 열거). 서명대·청문 접수부 등재. (1차: 5종 미등재 FAIL → D-03) | PASS |
| R4-1 | K-06/의도 문장 | R1-6 | `c6-b4` 존재 · `c4-b3` 부재 | PASS |
| R4-2 | bridges §2 표본 6행 ↔ timeline §7 상한 | `t0-b1`·`c1-b3`·`c2-b3`·`c4-b2`·`c6-b3`·`c7-b4` 세 줄 vs B01/B06/B10/B17/B26/B31 (bridges 2차 무변경, mtime 23:18 = 1차 시점) | t0-b1 이름 "서린" 0 ✓ · c1-b3 접점 시각 0 ✓ · c2-b3 지시자 신원·동기 0 ✓ · c4-b2 "한서린" 복원(허용)·의도 0 ✓ · c6-b3 순서 앵커까지, **고장 은폐·동기·R2 재해석 0** ✓ · c7-b4 에필로그 선공개 0 ✓ | PASS |
| R4-3 | bridges 기계 검사 재실행 | 행 수·순서·형식·규칙 명명 집계·잔존어·서린 최초 행·캐논 시각 최초 행 | 33행 · 순서 = campaign id 배열 · 빈 셀 0 · 형식 33/33 · 집계 {G-a 9, 법6 8, 법1 5, 법2 2, 법4 2, G-b 2, G-c 2, 법3 1, 법5 1, G-d 1} = 33 · 잔존/메타/B#/T-/폐기 시각 0 · 「서린」 최초 `c4-b2` 유일 · H-1:00/H+3:00=`t0-b3` · H-1:40=`c4-b1` · H-1:24/H-1:04=`c6-b3` | PASS |
| R5-1 | t0 재생성 diff 범위 | `git diff --stat -- systems/data/t0/ systems/pipeline/emit-tables.mjs` + 리프 diff | `11 files changed, 57 insertions(+), 57 deletions(-)`(t0 10 + emit-tables 1). beats.json **5리프**(sourceSha256 · `rows[1](t0-b2).objective` 안내문 삭제 · `rows[2](t0-b3).completion`/`_srcCompletion` 보관→증거 · `rows[2].completionPredicate.requires[3]._src` 보관→증거 = emit-tables L896 주석 정정 반영) · hints 1(sha) · records 4(sha 2 · hb-l1 · hb-l4) · tools/zones **0** · receipt 10(emittedUtc·sha·bytes) | PASS |
| R5-2 | 수치·id 변화 0 | 비문자열 변경 목록 | 비문자열 = 영수증 `source.bytes`·`tables[0].bytes`·`tables[4].bytes`뿐. id·rows 수·노브·카메라 0 | PASS |
| R5-3 | 출처 sha 정합(EG-PROV) | beats/records `sourceSha256` · records `recordsDocSha256` · receipt `source.sha256` · EG `provenance.inputs[campaign]` vs 현재 파일 재계산 | 5값 전부 **2차 campaign/t0-records sha와 일치** · receipt `emittedUtc 2026-09-11T14:41:04Z`(2차 재생성) · `scopeSource --scope` · `EG-PROV-01 PASS count=0` | PASS |
| R6-1 | frontmatter 5필드 | 19파일 파싱(planning meta 4 · field-classification · term-audit · glossary · **timeline** · term-decision · bridges · t0-records · rfc-cx-012-ack · game-ui-contract.meta · data/t0 meta 6) | 19/19 완비 · `updated: 2026-09-11` 19/19 | PASS |
| R6-2 | cycle 값 | 동일 파싱, HEAD 대조 | c7 16 · c3 2(glossary·timeline, status current, HEAD와 동일 값 — 제자리 갱신 RFC-Q2) · c5 1(game-ui-contract.meta, draft, HEAD 동일). status: draft 10 · current 9 | PASS(주석) |
| R7-1 | 범위 밖 파일에 CX-012 유입 | `git diff -- unity/ concept/ handoff/ systems/tech-verification/` grep 교체어·RFC-CX-012 | unity 매치 = `T0Strings.json` 한 줄 −/+ 양쪽 「증거함」(HEAD 2 = live 2, 기존). 그 외 0. `unity/**/Data/Tables/`·`Data/Authoring/` git status **clean** | PASS |
| R7-2 | 세션 귀속 구분 | `git status --short` + mtime | 범위 밖 M(`unity/**`·`concept/`·`handoff/`·`m5-direction/`·`.mex`·`graphify-out`·`production/*`)은 CX-012 레인 mtime과 겹쳐 **mtime만으로 귀속 판정 불가** — 내용 기준(R7-1) 유입 0만 확정. M5/M7/M9(RFC-CX-011)/디렉터 병행 세션 소유로 **구분만** | 판정 불가(구분만) |

집계(2차): 소항목 23 = PASS **22** · FAIL **0** · 판정 불가 1(R7-2). (1차: PASS 20 · FAIL 2 · 판정 불가 1.)

## 2. 결함 등록

| id | 등급 | 위치 | 내용 | 재현 / 확인 | 소유 레인 | 상태 |
|---|---|---|---|---|---|---|
| D-CX012-01 | S2 | `planning/campaign.json` `c4-b2.subtasks[3]` | B-09 교체가 조사를 남겨 "확대 판독**로**"(받침 뒤 「로」) — 표시 필드 문법 회귀 | 2차 [OBSERVED]: "판독기 확대 판독**으로** 드러난 획을 옮겨 적는다" · R2-3 자동 검사 0건 · 49/49·18/18 재PASS · ACK-a §3 정정 주석 기재 | planner | **closed** |
| D-CX012-02 | S2 | `planning/campaign.json` `c6-b4.objective` | B-08 교체가 「으로」를 남겨 "역대조**으로**"(모음 뒤 「으로」) — objective = CaseThread 카드 직접 출력 | 2차 [OBSERVED]: "결론 문장의 역대조**로** 수치화하고" · K-06 불변 | planner | **closed** |
| D-CX012-03 | S2 | `term-decision` §2 B-05~B-09 ↔ `glossary.md` | 표시 문자열 신규 어휘 5종(보호 지정·공통 종결부·종결부 후일담·역대조·확대 판독) glossary 행/언급 0 — L11 규칙 대비 미등재 | 2차 [OBSERVED]: glossary **R9** — §3 신설 3행(보호 지정·공통 종결부·종결부 후일담, first cycle c7) + §7 「파생 면제 규칙 (R9 · D-CX012-03)」 1줄(역대조=대조 어간 · 확대 판독=판독 어간, 별도 등재 없음). 5/5 해소 | director → worldview | **closed** |
| D-CX012-04 | S3 | `systems/rfc-cx-012-ack.md` §2 diff 표 | 라벨 오기 2건(`t0-b1`→`t0-b2` · `B-01`→`A-03`) | 2차 [OBSERVED]: L48 "`t0-b2.objective`(`rows[1]`, L163) … (B-14) · … 「보관함에」→「증거함에」 (A-03)" · L102 정정 기록 · 부기 emit-tables L896 `_src` 「증거함에」 정정 + `beats.json requires[3]._src` 반영 | systems | **closed** |
| D-CX012-05 | S3 | `worldview/term-audit-20260911.md` §E.2·E.3 | ACK-a 이후 스테일 문장 2곳 | 2차 [OBSERVED]: "여전히 planner 회신 대기"·"무변경 유지가 EvidenceGraph와 합의" grep 0건 · §E.2 (a) "해소: planner 회신 ACK-a …"로 재작성 | worldview | **closed** |
| D-CX012-06 | S3 | A-10 적용 형태 · `timeline.md` §7 B06/B12 | 판정 문언 「공통 조위 피크」로 통일 vs 실제 7/8 「조위 피크」; timeline 셀 "만조 피크"·"(훈련 화면)" 잔존 | 2차 [OBSERVED]: glossary §4 「공통 조위 피크」 행에 축약 허용 부기(명칭·정의 본문 불변, R3-2) · timeline B12 "만조→조위 피크" · B06 "(훈련 화면)→(연습 서식)" · timeline `만조 피크` 0 · `훈련 화면` 0 · frontmatter updated 2026-09-11(cycle c3·current 제자리) | director · worldview | **closed** |
| D-CX012-07 | S3 | `production/term-decision-aside-20260911.md` §4 · §1 A-10 행 | 2차 신규(기록 정합): 문서 무변경(mtime 19:59)이라 §4 "신설 등재 대기 목록"이 R9 3행·§7 면제 규칙·§4 축약 부기를 담지 않고, A-10 행 문언("「공통 조위 피크」로 통일")이 실제 적용형(「조위 피크」 축약)과 다르다. 판정 자체는 유효하고 정본(glossary R9)에 반영됐으므로 기록 부기만 필요 — R9 반영 1문단 제자리 추가 권고(RFC-Q2) | director | open (비차단) |
| D-CX012-08 | S3 | `synopsis/narrative-flow-bridges.md` §3.3 | 2차 신규(기록 정합): "「보호 지정」은 … 용어집 신설 대상이 아니다"·"glossary R8 개정본 기준"이 R9로 스테일(보호 지정은 이제 §3 정본 행, 역대조는 §7 면제 규칙 적용). §2 33행 자체는 무변경·검사 전 항목 PASS(R4-3) — 자기 검사 문단 1줄 갱신 권고 | synopsis | open (비차단) |

S1 **0** · S2 **0**(3건 closed) · S3 **2 open**(D-07·D-08, 둘 다 기록 정합 부기 — 승격 비차단) · S3 closed 3.

## 3. 파일별 승격 판정 (2차)

| # | 파일 | 현재 status | 판정 | 근거 |
|---|---|---|---|---|
| 1 | `planning/emit-evidence-graph.meta.md` | draft | **PASS** | R1-4 결정론(2차 3 sha 동일)·R1-5 18/18·R6 완비. meta 주장(nodes 152 edges 277, 2회 byte-동일) 재실행 stdout과 일치 |
| 2 | `planning/evidence-graph.meta.md` | draft | **PASS** | `EG-SUMMARY checks 18 pass 18 fail 0 PASS` 재현·EG-PROV-01 PASS·sha 재기재 없음(RFC-Q1) |
| 3 | `planning/evidence-graph-overlay.meta.md` | draft | **PASS** | EG-DISC-01/02/03 PASS + ACK-b §1 6팩트 timeline §7 전수 일치·R4-2 표본에서 R3 회수=c6-b4·순서 앵커=c6-b3 재확인 |
| 4 | `planning/validate-evidence-graph.meta.md` | draft | **PASS** | 18/18·49/49 병존·음성 시험 재현(주입 결함 FAIL, 저장소 무변경) |
| 5 | `planning/field-classification-20260911.md` (ACK-a) | draft | **PASS** (1차 HOLD → 해제) | 분류표·62곳·항목별 예상=실제·유지 13행 카운트 전부 재현. D-01·D-02 정정 확인(R2-3 0건) + ACK-a §3에 정정 주석·49/49·18/18 재검사 기재 [OBSERVED]. 해제 조건 충족 |
| 6 | `worldview/term-audit-20260911.md` (ACK-b 기록) | draft | **PASS** | R3-1·R3-2 재현·§C 무침해. D-05 스테일 2문장 정정 확인 |
| 7 | `worldview/glossary.md` (R8+R9) | **current**(c3, 제자리 갱신) | **PASS**(승격 대상 아님) | 신설 12행·정의 확장 1·축약 부기 1·§7 규칙 2줄·삭제 0·개명 0. R8/R9 주석이 diff와 일치 |
| 8 | `production/term-decision-aside-20260911.md` | draft | **PASS** (1차 HOLD → 해제, S3 부기) | HOLD 사유 D-03(S2)이 정본 glossary R9(등재 3 + 면제 규칙)로 닫혔고 D-06도 §4 축약 부기·timeline 정정으로 닫혔다. §1·§2 판정은 campaign·UI·t0-records·glossary에 정확히 적용됨(R2·R3). 남는 것은 D-07(S3, 이 문서 §4·A-10 행의 R9 미반영 부기) — 판정 기록의 후행 부기이지 판정 오류가 아니므로 비차단 |
| 9 | `synopsis/narrative-flow-bridges.md` | draft | **PASS** (S3 부기) | R4-2 상한 위반 0·R4-3 전 항목 재현·사용 신설어 등재 확인. D-08(§3.3 한 줄 R9 스테일) 비차단 |
| 10 | `synopsis/t0-records.md` | **current**(제자리 갱신) | **PASS**(승격 대상 아님) | hb-l1·hb-l4 2행 델타·records.json `recordsDocSha256` 현재 파일 sha 일치(R5-3) |
| 11 | `systems/rfc-cx-012-ack.md` (ACK-c) | draft | **PASS** | §1 UI 1행·§2 2차 재생성 영수증(`--t0` 5/5·shaMatch true·tools/zones 0리프·11파일 57/57)·§3 Unity 사본 stale 판정 재현. D-04 라벨 정정 확인 |
| 12 | `systems/game-ui-contract.meta.md` (+ `game-ui-contract.json` 개정 8) | draft(c5) | **PASS**(개정 8 델타 한정) | L360 1행(체크포인트→복귀 지점·스테이지→장(章), term-audit B-15 후보 ①)·재파싱 `13 19 20 13`·43637 B·잔여 「체크포인트」 9곳(셸)·validate-game-ui PASS·「복귀 지점」 §3 등재. **주의**: c5부터 draft(supersedes archive c4) — 문서 전체 current 승격은 선행 C5 검증 소관, 본 판정은 개정 8 델타 한정 |
| 13 | `systems/data/t0/{beats,hints,records,tools,zones,tables-receipt}.meta.md` (+ `.json` 6) | **current**(generated) | **PASS**(승격 대상 아님) | 손 편집 0·2차 재생성·R5-1/2/3 PASS·`--t0` 5/5·2차 campaign sha 정합 |
| 14 | `worldview/timeline.md` (§7 B06/B12 셀 2건, D-06) | **current**(c3, 제자리 갱신) | **PASS**(델타 한정) | diff = frontmatter updated + B06 "(훈련 화면)→(연습 서식)" + B12 "만조→조위 피크" 2셀. 상한·법·씨앗/회수·금지열 무변경 |

집계(2차): 14행 = PASS **14** · HOLD **0**. draft 10건 기준 PASS **10** · HOLD **0**. (1차: 13행 PASS 11 · HOLD 2.) S1 없음 — 승격 차단 0.

## 4. 미증명 경계 (이미 보고된 항목 재진술 포함)

- **런타임 미검증**: 문서·데이터 정합만 쟀다. Unity 임포트 0회·빌드 0회·플레이 표본 n=0. 교체 문자열의 실제 카드/힌트 오버레이 표시(줄바꿈·길이) 미측정. G1~G7 어느 게이트도 이 문서로 오르지 않는다.
- **Unity `Data/Tables/` 사본 stale** [OBSERVED]: `unity/Unknown/Assets/_Project/Data/Tables/tables-receipt.json` `source.sha256` ≠ 현재 campaign sha(emittedUtc 2026-09-10). `T0AssetImporter.Import()`는 다음 임포트에서 fail-closed — 의도된 신선도 게이트. 재복사·재임포트는 M9/다음 회차 몫이며 **이제 campaign sha가 안정됐으므로 한 번에** 할 수 있다.
- **저작 주석 필드 잔존 22건 + `c1-b3` 「훈련」 3건**(title·completion·clues[c1-b3-c2]): ACK-a §3이 후속(worldview 감사 확장)으로 넘김. 화면 비노출/술어/비트 제목 인용 위험 — 범위 밖 재진술.
- **`c6-b3.consequence`의 R3 문장**(ACK-b §2 긴장): 저작 주석 필드, timeline §7 B26 상한보다 한 비트 이름. 범위 밖 — consistency-audit 이관 상태 유지.
- **검증기 견고성 관찰**(결함 아님): 손상 그래프 음성 시험에서 두 검사 FAIL 뒤 exit 2로 종료 — fail-closed 유지되나 18검사 전체 보고 대신 예외로 끝난다. 정상 경로 무관.
- **decision-log 종합 미기재**: ACK-a/b/c 모두 "디렉터 종합 대기". 본 판정 반영·D-07/D-08 처리·RFC-CX-012 닫힘 기록은 디렉터의 후속 블록 소관.
- **세션 귀속**: R7-2대로 범위 밖 변경은 병행 세션 소유로 구분만 했고 귀속을 판정하지 않았다. CX-012 문자열 유입 0만 확정.
