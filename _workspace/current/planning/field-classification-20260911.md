---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-planner
---

# campaign.json 필드 분류 판정 — 표시 문자열 / 검증 술어 (2026-09-11)

## 0. 지위 · 왜 있는가

RFC-CX-012 선결 질문 (a)의 planner 회신이다. `worldview/term-audit-20260911.md` B-11이 "completion/objective 필드가 표시 문자열인지 검증 술어인지의 재분류가 이 감사의 최우선 RFC 질문"으로 올렸고, `production/term-decision-aside-20260911.md` §2 B-11 행이 이 판정에 B-11·B-12 completion 건·B-14를 종속시켰다. 판정 근거는 전부 **소비처 증거**다 — 파이프라인(`systems/pipeline/emit-tables.mjs`), 파생 테이블(`systems/data/t0/beats.json`·`hints.json`), Unity 런타임 코드(READ-ONLY grep), decision-log RFC-CX-011 S-D·RFC-C7-001.

## 1. 분류표

| 필드 | 분류 | 소비처 근거 [OBSERVED] |
|---|---|---|
| `title` | **표시** | 스테이지 title이 t0 `beats.json.stage.title`로 파생되고 CaseThread 카드가 "C1 · "+title 형으로 표기한다 (`C1GameSession.cs:41`, `C1SignatureGameSession.cs:23`). |
| `objective` | **표시** | ① RFC-CX-011 S-D "CaseThread 목표를 비트별 objective로" (decision-log). ② `T0GameSession.CaseObjective()`가 `beats.json rows[].objective`를 **그대로 화면 카드에 출력**한다 (`T0GameSession.cs:196-202`, 주석 "Objective text is data-owned"). ③ EditMode 테스트가 "A disclosure-clean authored objective is shown verbatim"을 단언한다 (`M9CoreTests.cs:56`). ④ C1도 `narrative.objective`를 `screen.Body`에 직접 넣는다 (`C1GameSession.cs:68`), `C1SignatureContract.json`의 `entryShows: "objective-only"`. |
| `subtasks[]` | **표시** | emit-tables가 비트 전체를 `beats.json rows`로 spread하여 런타임 데이터에 실린다. RFC-CX-011 G7(비트 목표 미노출) 해소 방향이 목표·하위 과업의 화면 노출이다. 표시 계열로 분류한다. |
| `hints[]` | **표시** | emit-tables `hintRowsFor()`가 각 힌트를 `sourceTextKo` + `textKey: hint.<beat>.l<n>`로 투영 (`emit-tables.mjs:125-148`), 런타임이 `sourceTextKo`를 힌트 오버레이·guided 티칭 문구로 직접 표기한다 (`T0GameSession.cs:209-212, 325`). |
| `recovery` | **표시** | 실패·복구 안내 산문. 좌절 방지 안내로서 표시 계열이며 검증 술어 구조가 전혀 없다. 술어 파생(`completionPredicate`)은 recovery를 읽지 않는다 [OBSERVED, `emit-tables.mjs buildBeats`]. |
| `clues[].description` | **표시** | C1 관찰 패킷의 `observation.description`이 `screen.Body`로 직접 표기된다 (`C1GameSession.cs:62`, `C1SignatureGameSession.cs:53`). campaign 단서 description이 그 저작 원본이다. |
| `completion` | **술어(조건 서술)** | ① `emit-tables.mjs buildBeats()`는 completion 문자열을 **구조화 술어로 손-파생**하고 원문을 `completionPredicate._srcCompletion`으로만 보존한다 (`emit-tables.mjs:856-901`). ② Unity 로더는 비트에서 `id`·`prerequisites`·`completionPredicate.requires`만 역직렬화한다 — `T0Json.BeatJson`에 completion 필드 자체가 없다 (`T0Json.cs:6`, `T0DataLoader.cs:26`). ③ 런타임 코드 전체에서 completion 산문을 읽는 곳 0건 [OBSERVED, `unity/**/*.cs` grep `\[."?completion"?\]|narrative\.completion|\.completion\b` 매치 0]. ④ 완료 표시는 `T0Strings.json`의 `L("complete")`·`L("caseComplete")` 별도 문자열이 담당한다 (`T0GameSession.cs:172,185`). |
| `inference` / `action` / `consequence` / `authorEstimateBasis` | 저작 주석(비감사 대상) | term-audit §0 스캔 범위(453건) 밖. 이번 판정·교체 대상 아님. |

### 예상 방향 대비

과업 브리핑의 예상 방향 [INFERENCE]과 판정 결과가 일치한다: objective·subtasks·hints·recovery·title·clues.description = 표시, completion = 술어. 증거는 위 표대로 전부 [OBSERVED]다.

## 2. 판정의 파급 (B-11 · B-12 · B-14)

- **B-11 (t0-b2.completion·t0-b3.completion 데이터 토큰)**: completion이 술어이므로 **위반 아님 — 문자열 보존**. `circuit`·`reader`·`plate-standard-hub`·`proofRequired: true` 등 토큰은 술어 파생의 근거 산문으로 남는다. `--pairs`·emit-tables `_srcCompletion` 대조(T0-01)가 이 원문에 걸려 있으므로 보존이 안전하다.
- **B-12 completion 건 (`t0-b3.completion`의 "T0에서")**: 같은 이유로 보존. B-12 중 **표시 필드 4건만** 교체한다(c3-b1.recovery · c7-b4.subtasks[1] · e0-b2.subtasks[3] · e0-b2.clues[e0-b2-c2]).
- **B-14 (objective 안내문 9곳)**: objective가 표시이므로 **위반 — 제거**. 동일 정보는 `toolTeaching` 필드가 보유함을 재확인했다 [OBSERVED]: guided 4건(t0-b2 circuit · c2-b2 corrosion · c3-b2 alignment · c5-b2 routing), unguided 5건(c4-b2 corrosion · c4-b3 seal · c6-b2 circuit · c6-b3 alignment · c7-b2 routing) — 안내문 9곳과 1:1 대응. 런타임 guided 티칭도 이미 `toolTeaching`을 소비한다 (`T0GameSession.cs:207-208`).
- **completion 필드 안의 무조건 통일 판정(A-03·A-09·A-10·B-01·B-05)**: B-11 재분류에 종속되지 않는 **용어 통일** 건이므로 completion 안이라도 교체한다(term-decision 표가 위치를 명시 지목). 단 이 교체는 화면에 나가지 않고 `_srcCompletion` 대조 체인에만 영향을 준다 — systems 재생성 필요(§ACK 참조).

## 3. 교체 실행 결과 — 건수 표 [OBSERVED]

교체어 정본: `production/term-decision-aside-20260911.md` §1·§2. 위치 정본: `worldview/term-audit-20260911.md`. 실행: 2026-09-11, 필드 전문(全文) 일치 assert 후 교체(스크립트는 저장소 밖 임시 실행 — 산출물 아님).

| 항목 | 교체 | 예상 | 실제 | 잔존 grep |
|---|---|---|---|---|
| A-01 판독대 | 없음 — 「판독대」 §2 신설 등재로 해소 | 0 | 0 | (4곳 보존) |
| A-02 습도판→조습기 | c1-b2.subtasks[1]·hints[2] | 2 | 2 | 노출 필드 0 |
| A-03 보관함→증거함 | t0-b3.completion | 1 | 1 | 0 |
| A-05 판정대→"정합기 판정 칸에" | c3-b3.subtasks[2] | 1 | 1 | 노출 필드 0 |
| A-09 가상 운전→가상 완주 | c5-b2 ×3 · c7-b2 ×2 | 5 | 5 | 노출 필드 0 |
| A-10 만조 피크→조위 피크 | c3-b1 ×5 · c3-b2 ×3 | 8 | 8 | 노출 필드 0 |
| A-11 교차표→대조표 | c2-b4 ×3 | 3 | 3 | 노출 필드 0 |
| A-12 재작성(+B-13) | c6-b1.clues[c6-b1-c2] | 1 | 1 | 0 |
| A-13 제출 접수 대장→청문 접수 대장 | c7-b4.clues[c7-b4-c2] | 1 | 1 | 0 |
| A-14 문맥별 | c3-b1.subtasks[0] 건물 · c3-b4.hints[0] 구역 | 2 | 2 | 0 |
| A-16 접수부 | 제외 — t0-records 소유(synopsis) | 0 | 0 | — |
| B-01 자동 백업→확정 전 보존 | 9곳 문장 재작성 | 9 | 9 | 0 |
| B-02 훈련 화면→연습 서식 · 요약 화면→결과 예고 · 같은 화면에→나란히 | 7곳 | 7 | 7 | 0 |
| B-03 훈련 모드→연습 압착 (+c1-b3.recovery 정렬) | 2+1 | 3 | 3 | 0 |
| B-04 플레이어→"당직자의 어떤 행동으로도" | c5-b3.recovery | 1 | 1 | 노출 필드 0 |
| B-05 보호 지정 | c5-b4.completion "보호 지정 1건이 확정되며" · e0-b1.subtasks[3] "5장의 보호 지정에 따라" | 2 | 2 | 노출 필드 0 |
| B-06 씬→공통 종결부 | e0-b1.objective | 1 | 1 | 노출 필드 0 |
| B-07 에필로그→종결부 후일담 | c5-b4.subtasks[2] | 1 | 1 | 0 |
| B-08 역매핑→역대조 | c6-b4 ×3 (K-06 문장 보존 확인) | 3 | 3 | 노출 필드 0 |
| B-09 확대 뷰→확대 판독 | c4-b2.subtasks[3] | 1 | 1 | 0 |
| B-10 미리보기→결과 예고 | c1-b1.recovery | 1 | 1 | 0 |
| B-11 completion 토큰 | 무변경(§2 판정) | 0 | 0 | — |
| B-12 표시 필드 건 | c3-b1.recovery "다음 조위정합" · c7-b4.subtasks[1] "앞서 만든 대조표" · e0-b2 ×2 "인수 때" | 4 | 4 | 노출 필드 0 |
| B-13 T-19→"19년 전을 끝으로" | (A-12와 같은 문장) | 1 | 1 | 노출 필드 0 |
| B-14 안내문 제거 | objective 9곳 | 9 | 9 | 0 |
| B-15 체크포인트 | 제외 — UI 계약 소유(systems) | 0 | 0 | — |
| (부기) 훈련→연습 흡수 정렬 | c1-b3.subtasks[4] "훈련 로그"→"연습 로그" — B-02/B-03 흡수 판정("동의어 「훈련」을 세우지 않는다")의 일관 적용. 감사 건수 밖이라 별도 행으로 기록 | +1 | +1 | — |

QA 정정(D-CX012-01/02) 반영 — B-09 c4-b2.subtasks[3] 「확대 판독로」→「확대 판독으로」 · B-08 c6-b4.objective 「역대조으로」→「역대조로」(조사 2곳, 교체어 자체는 불변) — 재검사 49/49·18/18 [OBSERVED], K-06 `{c4-b3: false, c6-b4: true}` 유지, evidence-graph provenance 재발행.

잔존 grep 주석 [OBSERVED, 교체 후 재스캔]: "노출 필드 0"은 감사 스캔 범위(title/objective/subtasks/hints/recovery/completion/clues.description)에서 잔존 0을 뜻한다 — 위 표의 감사 대상 문자열 전부 노출 필드 잔존 0 확인. 저작 주석 필드(action·consequence·inference·authorEstimateBasis)에는 습도판 1·가상 운전 3·만조 피크 2·교차표 2·역매핑 2·판정대 1·플레이어 1·플래그 2·씬 2·확대 뷰 2·요약 화면 1·훈련 계열 3이 남아 있다 — 감사 범위 밖(§1 분류표)이며 화면에 나가지 않는다. 일괄 정리는 worldview 감사 확장 여부와 함께 후속 판정 대상으로 넘긴다. 감사 목록 밖 표시 필드의 「훈련」 잔존 3건을 관찰로 기록한다: ① `c1-b3.title` "봉인대 훈련: …"(비트 제목 — 타 레인 인용 위험이 있어 단독 개명하지 않음), ② `c1-b3.completion` "훈련 로그로 시연된다"(술어 분류로 보존), ③ `c1-b3.clues[c1-b3-c2].description` "봉인대 훈련 로그: …". 셋 다 훈련→연습 흡수 판정의 후속 감사 대상으로 worldview에 넘긴다.

> (2026-09-11 RFC-CX-013) 위 문단의 저작 주석 잔존 22건과 `c1-b3` 「훈련」 3건은 **해소됐다** — 실측·교체 결과는 말미 `## RFC-CX-013 ACK` §1·§2. 문단은 RFC-CX-012 시점 기록으로 보존한다.

## 4. 검증 [OBSERVED]

검증 명령(RFC-CX-012 게이트 연결)과 관찰 결과:

- `node _workspace/current/planning/validate-campaign.mjs` → `{"checks": 49, "pass": 49, "fail": 0, "verdict": "PASS"}` [OBSERVED]. K-06 `{c4-b3: false, c6-b4: true}` = expected — RFC-W4 의도 문장 위치 보존.
- `--pairs` → exit 0, 독립쌍 17비트 출력(t0-b3 포함) [OBSERVED].
- `--t0 _workspace/current/systems/data/t0` → `{"checks": 5, "pass": 4, "fail": 1, "verdict": "FAIL"}`: **T0-01 FAIL 2행**("t0-b3: completion ≠ campaign.completion" · "t0-b3: completionPredicate._srcCompletion ≠ campaign.completion"), T0-02~T0-05 PASS [OBSERVED]. 원인은 기존 t0 데이터가 교체 전 campaign 파생물인 것 — ACK 5항.
- `node _workspace/current/planning/emit-evidence-graph.mjs` → "nodes 152 edges 277" 재생성, provenance가 새 campaign sha·bytes로 갱신됨 [OBSERVED].
- `node _workspace/current/planning/validate-evidence-graph.mjs` → `EG-SUMMARY checks 18 pass 18 fail 0 PASS` [OBSERVED].

## RFC-CX-012 ACK

**ACK-a (planner) — 회신.**

1. **재분류 판정**: completion = **검증 술어(조건 서술)** — 런타임 소비처 0건, 구조화 술어(`completionPredicate.requires`)의 저작 원문. objective·subtasks·hints·recovery·title·clues.description = **표시 문자열**. 근거는 본 문서 §1 [OBSERVED].
2. **파급 적용**: B-11 위반 아님(문자열 보존) · B-12는 표시 필드 4건만 교체 · B-14는 위반으로 9곳 제거(`toolTeaching` 보유 확인). 종결.
3. **campaign.json 교체 완료**: term-decision §1·§2 표 그대로 — 교체 연산 65건 / 고유 위치 필드 62곳(이중 편집 3곳: c5-b2.recovery A-09+B-01 · c3-b1.recovery A-10+B-12 · c3-b2.objective A-10+B-14). 교체 항목별 예상/실제 건수는 §3 표와 같다(전 항목 예상=실제). 유지 13행(term-audit §C) 침해 0 [OBSERVED, 교체 전후 카운트 대조]: 법1·법6·이중서명·염판·판 #0·부식 시험·부식예산·오차띠·unknown 등 전부 무변동, 확정 +8(「확정 전 보존」 신설 사용)·연습 +6(훈련 흡수)·판독 +1(「확대 판독」)·조위정합 +1(B-12 "다음 조위정합")은 전부 정본 사용 증가이지 개명·재정의가 아니다. A-10은 §4 정본 「공통 조위 피크」를 노출 문자열이 따르게 한 방향. K-06 문장 '방패가 아니라 잠금장치'(c6-b4 존재·c4-b3 부재) 보존 — K-06 PASS [OBSERVED].
4. **검증 결과** [OBSERVED]: validate-campaign **49/49 PASS**(sha 갱신 — 고정 숫자는 재기재하지 않는다, RFC-Q1) · `--pairs` 정상 출력 · evidence-graph 재생성(provenance 갱신) 후 validate-evidence-graph **18/18 PASS**. 전문은 §4.
5. **systems 레인 재생성 필요** (`--t0` 대조) [OBSERVED]: 기존 `systems/data/t0/*`는 교체 전 campaign에서 생성된 것이라 `--t0` T0-01이 정확히 2행(t0-b3 completion 보관함→증거함 · `_srcCompletion` 동일 건)으로 FAIL했다(T0-02~05 PASS) — 검증기 주석 그대로 "FAIL은 systems로 돌려보낸다". **systems가 `emit-tables.mjs --out _workspace/current/systems/data/t0` 재실행으로 재생성해야 한다**(t0-b2.objective 안내문 제거·t0-b3.completion 1어 교체가 반영된 새 sourceSha 포함; scope=all `Data/Tables/` 사본도 동일하게 재생성 대상). 부기: `emit-tables.mjs`의 `autoCopyCreated` `_src` 주석 문자열("보관함에 생성")이 교체 후 원문과 1어 어긋난다 — 주석 층위라 동작 무관, systems 재량 정정 대상.
6. **본 문서가 ACK-a의 기록 위치다** — `production/decision-log.md`는 직접 편집하지 않았다(디렉터 종합 대기).

## RFC-CX-013 ACK

**ACK-a (planner) — RFC-CX-012 [CARRIED] 이월 해소 회신.** 기준 커밋 `fc737c1`(=`HEAD 1bd0097`, `campaign.json`·`validate-campaign.mjs` 두 파일은 두 커밋 사이 diff 0 [OBSERVED]). 편집 파일: `planning/campaign.json` · `planning/validate-campaign.mjs`(K-07) · `planning/validate-campaign.meta.md` · `planning/content-matrix.md`·`planning/game-draft-v1.md`(제목 인용 2행) · `planning/evidence-graph.json`(emit 재실행으로만) · 본 문서. `unity/**`·`assets/**`·`production/decision-log.md`·타 레인 파일 무접촉 [OBSERVED].

### 1. 저작 주석 잔존 교체 — 실측 [OBSERVED]

grep 대상: `action`·`consequence`·`inference`·`authorEstimateBasis` 4필드 × 교체어 12군(계약 맵). §3 주석의 집계 "22건"은 **필드-히트 22곳 / 어휘 출현 23회**였다(`c7-b4.consequence`에 「씬」 2회). 실측과 교체:

| 교체어(맵) | 필드 위치 [OBSERVED] | 예상 | 실제 | 교체 문형 |
|---|---|---|---|---|
| 습도판→조습기 | `c1-b2.action` | 1 | 1 | "판독기 조습기의 단수를" |
| 가상 운전→가상 완주 | `c5-b2.authorEstimateBasis`·`action`·`consequence` | 3 | 3 | "각각 가상 완주하는 조작" · "가상 완주로 끝까지 돌리고" · "가상 완주는 실제 계통을" |
| 만조 피크→조위 피크 | `c3-b1.consequence` · `c3-b2.inference` | 2 | 2 | "조위 피크 3개를" · "조위 피크는 공유된다" |
| 교차표→대조표 | `c2-b4.authorEstimateBasis`·`action` | 2 | 2 | "대조표를 만들(고/어)" |
| 역매핑→역대조 | `c6-b4.authorEstimateBasis`·`action` | 2 | 2 | "역대조해" ×2 |
| 판정대→정합기 판정 칸 | `c3-b3.action` | 1 | 1 | "두 쌍을 정합기 판정 칸에 올려" |
| 플레이어→당직자 | `c1-b3.consequence` | 1 | 1 | "당직자의 지식으로 등록된다" |
| 플래그→보호 지정 | `c5-b4.consequence` · `e0-b1.consequence` | 2 | 2 | "보호 지정 1건(propertyProtection)이" · "보호 지정(propertyProtection)은" — 데이터 키 `propertyProtection`은 저작 주석의 참조 정보라 괄호로 보존 |
| 씬→종결부 | `c7-b4.consequence` ×2 · `e0-b1.consequence` | 2(필드) | **3(어휘)** | "같은 공통 종결부로 들어가고" · "종결부는 갈라지지 않는다" · "공통 종결부는 하나이며" |
| 확대 뷰→확대 판독 | `c4-b2.authorEstimateBasis`·`action` | 2 | 2 | "확대 판독 전사 1회" · "확대 판독으로 옮겨 적는다" |
| 요약 화면→결과 예고 | `c7-b3.consequence` | 1 | 1 | "확정 전 결과 예고가 열린다"(「활성화」도 기기 어휘라 「열린다」로 정렬) |
| 훈련 계열→연습 계열 | `t0-b2.action` · `c1-b3.authorEstimateBasis`·`action` | 3 | 3 | "회로 지도 연습이다" · "봉인대 연습 압착 1회" · "빈 서식에 연습 압착을 한 번 해"(`c1-b3.subtasks[2]` 표시 문형과 정렬) |
| **합계** | 21필드(`c1-b3.authorEstimateBasis`와 `action`은 각 1필드) | 22 | **22 필드-히트 / 23 어휘** | — |

필드별 교체 리프: `action` 8 · `authorEstimateBasis` 5 · `consequence` 7 · `inference` 1 (= 21; §3·§4 R3 이관 4리프와 c1-b3 3리프를 더한 전체 변경 리프 28 — 아래 §5). **재검색 [OBSERVED]**: 4필드 × 12군 잔존 **0** · campaign.json 전문(全文) 잔존 **0**(「훈련」 포함 12군 전부 0).

### 2. `c1-b3` 「훈련」 3건 + 외부 인용 [OBSERVED]

| 위치 | 전 | 후 |
|---|---|---|
| `c1-b3.title` | 봉인대 훈련: 필압과 완료 접점 | **봉인대 연습: 필압과 완료 접점** |
| `c1-b3.completion` | …접점 개념이 훈련 로그로 시연된다. | …접점 개념이 **연습 로그**로 시연된다. (술어 필드이나 어휘 통일 건 — §2 마지막 항 원칙) |
| `c1-b3.clues[c1-b3-c2].description` | 봉인대 훈련 로그: 압착이 끝나면… | **봉인대 연습 로그**: 압착이 끝나면… |

제목 개명 전 `_workspace/current/` 전문 grep(「봉인대 훈련」) 히트와 처리:

| 파일:행 | 레인 | 처리 |
|---|---|---|
| `planning/content-matrix.md:123` (제목 열) | planner | **갱신** |
| `planning/game-draft-v1.md:128` "씨앗 B06(`c1-b3`, 봉인대 훈련)" | planner | **갱신** |
| `planning/evidence-graph.json:1561` (node title) | planner(파생) | emit 재실행으로 **갱신** — node title = "봉인대 연습: 필압과 완료 접점" [OBSERVED] |
| `planning/campaign.meta.md:738` §5 F1 "씨앗은 `c1-b3` 봉인대 훈련" | planner | **보존** — C3 검토 처리 기록(이력 문단) |
| `planning/evidence-graph-overlay.json:39` basis "§8 표(씨앗 c1-b3 봉인대 훈련…)" · `evidence-graph-overlay.meta.md:22` | planner | **재도출 완료** [OBSERVED] — `timeline.md` §8 원문 행 인용이라 worldview 갱신(아래 행) 확인 후 「봉인대 연습」으로 재도출, emit 재실행 → EG 18/18, graph 전문 「봉인대 훈련」 0 / 「봉인대 연습」 2 |
| `planning/field-classification-20260911.md:75` | planner(본 문서) | 보존 + 해소 포인터 추가(위 §3 말미 인용문) |
| `worldview/timeline.md:181` §8 표 "씨앗은 B06(`c1-b3`) 봉인대 훈련" | worldview | **넘김 → 갱신 완료** [OBSERVED, WorldviewCarry IRC 회신 + 원문 재확인 "봉인대 연습"] |
| `worldview/term-audit-20260911.md:276` overlay 정합표 "봉인대 훈련" · `:143`·`:147` 감사 후보 원문 | worldview | **넘김 → 276 갱신 완료**(WorldviewCarry 회신); 143·147은 감사 시점 원문 인용으로 보존(worldview 판정) |
| `synopsis/campaign.md:42` F1 "씨앗은 c1-b3 봉인대 훈련에서 플레이어가 직접…" | synopsis | **넘김** — 「봉인대 연습에서 당직자가 직접」 갱신 제안(플레이어도 B-04 계열) |
| `synopsis/chapter-beats.md:156` "봉인대 훈련 로그: 압착이 끝나면…" (단서 c2 인용) | synopsis | **넘김** — 「봉인대 연습 로그」로 갱신 요청 |
| `synopsis/narrative-flow-bridges.md:146` "훈련→연습 압착…" | synopsis | 보존 — 갱신 이력 문장(교체 전 어휘를 이력으로 인용) |
| `qa/rfc-cx-012-review.md:31` · `production/term-decision-aside-20260911.md:43-44` · `worldview/term-audit-20260911.md:145-149` | qa·production·worldview | 보존 — 판정·심사 원문 |

정본 우선 원칙에 따라 제목은 개명했고 파생 문서는 위 표대로 따라온다.

### 3. `c6-b3` R3 이관 → `c6-b4` — 전후 원문 [OBSERVED]

worldview ACK-b 판정(§7 상한 우선, RFC-W4 선례)의 planner 편집 형태 = **이관**. `timeline.md` §7 B26(`c6-b3`) "순서 앵커 회수 — 밸브가 봉인 완료보다 20분 앞서 돌았다. 집행이 이중서명 완성 전에 이뤄졌다" · B27(`c6-b4`) "[R3 회수] 실제 동기는 1호기 고장 은폐, '창고 보호'는 사후 설명. R2 의도 재해석: 무효 서명은 도연의 거부 수단"과 일치하도록 4리프를 편집했다.

**`c6-b3.consequence`** 전:
> R3 확정. 실제 동기는 1호기 고장 은폐다. 동시에 R2가 재해석돼 무효 서명이 도연의 거부 수단이었음이 사건판에 기록된다.

후:
> 순서 앵커가 확정 항목으로 오른다 — 밸브가 봉인 완료보다 20분 앞서 돌았고, 집행은 이중서명이 완성되기 전에 이뤄졌다. 네 날짜의 순서표가 사건판에 붙는다. 그 이유의 확정은 다음 비트로 넘어간다.

**`c6-b3.inference`** 전:
> 두 각인의 간격 20분은 총 오차폭 8분보다 크므로 순서가 확정된다 — 밸브는 봉인 완료보다 20분 앞서 돌았고, 집행은 이중서명이 완성되기 전에 이뤄졌다. 왜 그랬는지는 두 개의 다른 서류철이 말한다. 사유란이 봉인 완료 시각을 기준으로 심사철에 넘어가므로, 밸브를 먼저 돌리고 봉인을 늦추면 1호기 정지가 사유란에 남지 않는다. 증설 승인 조건이 배수 정격이었으니 그것이 지켜야 했던 것이다. 창고 보호는 복구 뒤에 만들어진 설명이다. 같은 논리로 도연의 무효 서명은 방패가 아니라 완성 자체를 막으려던 잠금장치였다.

후:
> 두 각인의 간격 20분은 총 오차폭 8분보다 크므로 순서가 확정된다 — 밸브는 봉인 완료보다 20분 앞서 돌았고, 집행은 이중서명이 완성되기 전에 이뤄졌다. 정비 대장에 그날 시험 기록이 없으니 그 접점은 시험이 아니라 실제 봉인이다. 사유란이 봉인 완료 시각을 기준으로 심사철에 옮겨 적힌다는 규칙은 서류가 말하는 사실이고, 그 순서가 무엇을 사유란 밖으로 밀어냈는지는 순서표가 선 다음에 따진다.

**`c6-b4.consequence`** 전:
> 세 관점의 제출 초안이 열리고 청문 검증 절차가 예고된다. 남는 질문은 무엇이 사실인가가 아니라 무엇을 남길 것인가로 바뀐다.

후(말미에 이관 문장 부착 — 기존 `c6-b4`에 같은 문장 없음 확인):
> 세 관점의 제출 초안이 열리고 청문 검증 절차가 예고된다. 남는 질문은 무엇이 사실인가가 아니라 무엇을 남길 것인가로 바뀐다. R3 확정. 실제 동기는 1호기 고장 은폐이고 "창고 보호"는 사후 설명이다. 동시에 R2가 재해석돼 무효 서명이 도연의 거부 수단이었음이 사건판에 기록된다.

**`c6-b4.inference`** 전:
> 결론이 현장 기록에 뿌리를 두지 않고 그날로 청구권이 닫혔다면, 그 결론은 발견된 것이 아니라 필요해서 쓰인 것이다. 도연이 고른 무효 서명도 같은 성격이다 — 그것은 방패가 아니라 잠금장치다.

후(`c6-b3.inference`에서 뺀 **동기 기제** 3문장을 앞에 붙임 — "방패가 아니라 잠금장치" K-06 문장은 그대로):
> 사유란이 봉인 완료 시각을 기준으로 심사철에 넘어가므로, 밸브를 먼저 돌리고 봉인을 늦추면 1호기 정지가 사유란에 남지 않는다. 증설 승인 조건이 배수 정격이었으니 그것이 지켜야 했던 것이고, 창고 보호는 복구 뒤에 만들어진 설명이다. 결론이 현장 기록에 뿌리를 두지 않고 그날로 청구권이 닫혔다면, 그 결론은 발견된 것이 아니라 필요해서 쓰인 것이다. 도연이 고른 무효 서명도 같은 성격이다 — 그것은 방패가 아니라 잠금장치다.

범위 한정 [OBSERVED]: `c6-b3`의 `objective`("…이유를 서로 다른 출처로 밝힌다")·`completion`("동기는 …두 기관의 서류로 각각 확정된다")·`hints[2]`("동기는 밸브가 아니라 서류에 있다…")·`clues[c6-b3-c4]`(심사철 규칙)은 **무변경** — 이들은 R3 동기 **문구**(고장 은폐·창고 보호·거부 수단)를 담지 않고 서류 기제까지만 말하며, 표시 필드는 RFC-CX-012 감사 범위였다. 다만 `objective`의 "이유를 …밝힌다"와 `completion`의 "동기는 …확정된다"는 B26 상한("순서 앵커 회수")과 **문장 차원의 긴장**이 남는다 — 비트 설계(subtasks[2]·[4]와 단서 c3·c4가 두 기관 서류를 `c6-b3`에서 열게 함)를 건드리는 재-인테이크 건이라 이 회차에 흡수하지 않고 디렉터 종합에 올린다 [INFERENCE: K-07 어구 검사로는 잡히지 않는 층위].

### 4. `K-07` 정의·결과 [OBSERVED]

`validate-campaign.mjs` `EXPECT`에 `r3Phrases: ['고장 은폐','창고 보호','거부 수단']` · `r3AbsentBeat: 'c6-b3'` · `r3AbsentFields: ['consequence','inference','objective']` · `r3PresentBeat: 'c6-b4'` 신설, K-06 꼴로 `check('K-07', 'RFC-CX-013 R3 회수 위치 = c6-b4 (timeline §7 B27) — …')`. 부재 = 세 필드 연결문에 세 문구 **중 하나라도** 있으면 true(→FAIL), 존재 = `c6-b4` 전문에 세 문구 **전부**. `summary.checks`는 `checks.length` 자동 집계 — 하드코딩 49 없음(`.mjs` grep 0건); `validate-campaign.meta.md` §3 표제·§4 첫 줄 산문 49→50, K-07 행 등재, §5.4 개정 블록, `updated: 2026-09-11`.

- live: `K-07 PASS {"c6-b3":false,"c6-b4":true}` · `K-06 PASS {"c4-b3":false,"c6-b4":true}`.
- 음성 시험: 교체 전 campaign(`git show fc737c1:…/campaign.json` → 저장소 밖 임시 경로) → `{"checks":50,"pass":49,"fail":1,"verdict":"FAIL"}` · `K-07 FAIL {"c6-b3":true,"c6-b4":false}` · exit 1. 나머지 49검사는 교체 전 데이터에서도 PASS(K-07만 갈린다).

### 5. 검증 출력 [OBSERVED]

- `node _workspace/current/planning/validate-campaign.mjs` → `{"checks":50,"pass":50,"fail":0,"verdict":"PASS"}` · exit 0. `bytes` 124221. sha는 검증기 출력값(RFC-Q1 — 고정 숫자 재기재하지 않음).
- `--pairs` → exit 0, 독립쌍 **17비트**(t0-b3·c1-b2·c1-b3·c2-b3·c2-b4·c3-b1·c3-b2·c3-b3·c4-b1·c4-b2·c4-b3·c5-b1·c5-b3·c6-b1·c6-b2·c6-b3·c7-b2) — RFC-CX-012 QA와 동일 집합.
- `--t0 _workspace/current/systems/data/t0` → `{"checks":5,"pass":5,"fail":0,"verdict":"PASS"}` · exit 0 — **예상(T0-01 FAIL)과 다르다**: T0-01은 T0 3비트의 completion·술어 원문·단서 id만 대조하는데 이번 편집은 T0 비트의 표시·completion 필드를 건드리지 않았다(`t0-b2.action`만 — 비-emit 필드). 다만 `sourceShaMatchesLiveCampaign: false`(선언 sha = 교체 전 campaign sha, live sha ≠) — **systems 재생성 대상**(`emit-tables.mjs --out …/systems/data/t0`; scope=all `Data/Tables/` 사본 포함). 검사 FAIL이 아니라 provenance 플래그다.
- `node _workspace/current/planning/emit-evidence-graph.mjs` → "nodes 152 edges 277 (beat 33, clue 73, source 31, stage 9, tool 6)", provenance inputs가 새 campaign sha·bytes 124221로 갱신 [OBSERVED].
- `node _workspace/current/planning/validate-evidence-graph.mjs` → `EG-SUMMARY checks 18 pass 18 fail 0 PASS` · exit 0.
- 구조 diff(HEAD↔live, 리프 평탄화): 리프 1502→1502 · 추가 0 · 삭제 0 · **변경 28**(action 8 · consequence 9 · authorEstimateBasis 5 · inference 3 · title 1 · completion 1 · clues.description 1) · 비문자열 0 · id 0.
- 유지 13행(term-audit §C, 31명사 전문 카운트) 교체 전후 대조: 감소 **0**, 증가 4(확정 87→89 · 연습 7→13 · 이중서명 5→6 · 판독 59→61) — 전부 정본 사용 증가(연습 흡수·"이중서명이 완성되기 전" 재서술·「확대 판독」)이지 개명·재정의 아님. 법1·법6·염판·조위정합·부식 시험·판 #0·부식예산·오차띠·unknown·공통 조위 피크·대조의 밤 등 무변동.

### 6. 이월·넘김

- systems: `data/t0/*` provenance sha 재생성(위 §5 `--t0` 행). Unity `Data/Tables/` 사본 동반.
- worldview: `timeline.md:181`·`term-audit-20260911.md:276` 인용 갱신 **완료**(WorldviewCarry 회신 — 그가 K-07·50/50·EG 18/18을 직접 재실행 확인, `consistency-audit.md` A44 = pass 마감). `c6-b3.objective`/`completion`의 "이유·동기" 문장 긴장(§3 범위 한정)은 디렉터 종합 안건.
- planner(후속, Main 지시): **overlay 인용 재파생 완료** [OBSERVED] — `evidence-graph-overlay.json:39`·`.meta.md:22`의 timeline §8 verbatim 인용을 갱신 원문 「봉인대 연습」으로 재파생(2줄). overlay sha 변경(교체 전 `87d6…`→live, provenance inputs가 live sha 추적) → graph는 **불변이 아니다**(basis 문자열이 reveals 블록에 실린다) → `emit-evidence-graph.mjs` 재실행 → `validate-evidence-graph.mjs` **18/18 PASS**, graph 전문 「봉인대 훈련」 0 · 「봉인대 연습」 2.
- synopsis: `campaign.md:42` · `chapter-beats.md:156` 인용 갱신(§2 표).
- 본 문서가 ACK-a(RFC-CX-013)의 기록 위치다 — `production/decision-log.md`는 편집하지 않았다(디렉터 종합 대기).
