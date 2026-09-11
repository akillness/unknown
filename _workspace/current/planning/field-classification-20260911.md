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
