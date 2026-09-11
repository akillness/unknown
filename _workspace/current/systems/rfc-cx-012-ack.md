---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
---

# RFC-CX-012 ACK-c — systems 레인 적용 기록 (2026-09-11)

## 0. 지위 · 왜 있는가

`production/decision-log.md` RFC-CX-012 [DECISION] 적용 경계 (c) — "systems — UI 계약 `information_hierarchy` 「체크포인트」→「복귀 지점」 반영 여부" — 의 회신이다. 선결인 ACK-a(planner, `planning/field-classification-20260911.md` — campaign.json 문자열 교체 완료)·ACK-b(worldview, `worldview/term-audit-20260911.md` 머리 + `glossary.md` R8)·synopsis `t0-records.md` 2행 갱신이 끝난 뒤에 실행했다. **`production/decision-log.md` 는 직접 편집하지 않았다** — 본 문서가 ACK-c 의 기록 위치다(디렉터 종합 대기).

이 문서는 세 가지를 기록한다: ① UI 계약 교체 1곳 ② T0 파생 데이터 재생성 영수증 ③ Unity 런타임 파급 판정. 이 편집은 **무엇도 재지 않았고 어떤 게이트도 올리지 않는다.**

## RFC-CX-012 ACK

### 1. UI 계약 교체 — B-15 [OBSERVED]

| 파일 | 위치 | 전 | 후 |
|---|---|---|---|
| `systems/game-ui-contract.json` | `information_hierarchy.contextual[3]` (L360) | 「**체크포인트** 목록과 각 지점의 **스테이지** 조위 위상」 | 「**복귀 지점** 목록과 각 지점의 **장(章)** 조위 위상」 |

- 교체어 정본: `production/term-decision-aside-20260911.md` §2 B-15 행(「복귀 지점」 — 「보존」은 행위·사본 축, 「복귀」는 위치 축). 후보 문안은 `worldview/term-audit-20260911.md` §B-15 교체 후보 ① 그대로. 「복귀 지점」은 `glossary.md` §3 R8 등재 완료 [OBSERVED, glossary L101 「복귀 지점 | Return Point | 당직 기록상 되돌아갈 수 있는 지점」].
- 텍스트 치환 1행 · 서식 정규화 없음. `git diff --stat` → **`1 file changed, 1 insertion(+), 1 deletion(-)`**. 재파싱 **최상위 13키 · screens 19 · matrix 20 · decisions 13** — 개정 7 직후와 동일.
- 스키마 검증: `python3 …/game-ui-ux/scripts/validate-game-ui.py game-ui-contract.json` → **`PASS: valid game UI contract` · exit 0**.
- **건드리지 않은 것**: 같은 파일의 나머지 「체크포인트」 9곳(L138·272·281·310·338·416·517·526·620)은 셸 화면(load-recovery·result-checkpoint·ending 등)·검증 매트릭스·데이터 바인딩의 계약 산문이며 term-audit §B-15 「범위 주의」대로 **위반 아님**. 저장·슬롯·버튼도 동일. `layout.anchors[0]` 「스테이지와 조위 위상 표시」는 감사 대상 밖이라 무변경. 유지 13행(term-audit §C) 침해 0.
- 메타 갱신: `systems/game-ui-contract.meta.md` `updated: 2026-09-11` + 「개정 8」 절(치환 표·재현 명령·크기). 해시 표의 계약 JSON 행은 스테일 선언만 하고 숫자를 옮겨 적지 않았다(RFC-Q1).

### 2. T0 파생 데이터 재생성 영수증 [OBSERVED 2026-09-11]

**동기**: ACK-a/synopsis 교체 뒤 `systems/data/t0/*.json` 이 교체 전 문자열을 들고 있어 `validate-campaign.mjs --t0` **T0-01 FAIL 2행**(`t0-b3: completion ≠ campaign.completion` · `t0-b3: completionPredicate._srcCompletion ≠ campaign.completion`) · `sourceShaMatchesLiveCampaign: false` 상태였다(재생성 전 관측).

**명령** (`emit-tables.mjs` 파일 머리 주석 · `data/t0/*.meta.md` §1 의 기존 호출 방식 그대로 — 손 편집 0건):
```
$ node _workspace/current/systems/pipeline/emit-tables.mjs --scope t0 --out _workspace/current/systems/data/t0
exit 0
```
- 생성기가 호출한 검증기: `validator.verdict PASS · checks 49 · pass 49 · fail 0 · exitCode 0` (fail-closed 통과 후에만 씀).
- 영수증 `scope: t0` · `scopeSource: --scope`(이전 판과 동일 값이 되도록 `--scope` 명시 — 첫 시도를 `--out` 만으로 돌렸더니 `scopeSource: out-dir-basename` 으로 찍혀 영수증 diff 1행이 더 생겼고, 두 실행의 테이블 sha 는 전부 동일했다). 결정론 확인: 같은 입력 2회 실행 → 5테이블 sha 동일, `emittedUtc` 만 다름(영수증 `ignoredFields` 명시 항목).
- 교차 검사: `plateAnchors 33/33 · ledgerAnchors 28/28 · clueIdOriginMedium 전건 일치` · `toolInvariants T-I1/I3/I4/I7 PASS`.

**diff 범위** (`git diff --stat -- _workspace/current/systems/data/t0/` → **10 files changed, 55 insertions(+), 55 deletions(-)**; `git diff --word-diff` 로 단어 단위 확인):

| 파일 | 바뀐 것 | 성격 |
|---|---|---|
| `beats.json` | `t0-b2.objective`(`rows[1]`, L163) 말미 「안내 표시가 각 단계에 붙는다.」 삭제 (B-14) · `t0-b3.completion` + `completionPredicate._srcCompletion` 「보관함에」→「증거함에」 (A-03) · `sourceSha256` | 교체 문자열 + 출처 sha |
| `hints.json` | `sourceSha256` 만 | 출처 sha(힌트 9행 문자열 불변) |
| `records.json` | `hb-l1` 「판독기와 봉인대를」→「판독기와 서명대를」 · `hb-l4` 「이관 시각까지 접수부에」→「이관 시각까지 청문 접수부에」 · `sourceSha256` · `recordsDocSha256` | 교체 문자열(synopsis 2행 그대로) + 출처 sha |
| `tools.json` · `zones.json` | **바이트 동일**(diff 0) | — |
| `tables-receipt.json` | `emittedUtc` · `source.sha256/bytes` · `tables[beats/hints/records].sha256/bytes` · `authoringSources.records.sha256` · `authoringSources.manifest.sha256` | 영수증 |
| `*.meta.md` ×6 | `updated: 2026-09-11` · §2 관측 날짜/sha/바이트 · §3 출처 표 3행(`campaign.json` · `t0-records.md` · `asset-manifest.md`) | 생성기 출력 |

- **수치·id 변화 0건** [OBSERVED]: 비트 id·단서 id·rows 수(beats 3 · hints 9 · tools 2 · zones 1 · records 5)·노브 값·카메라 포즈·염판/조위대장 곡선 전부 불변(`tools.json`·`zones.json` 바이트 동일, `records.json` 은 두 text 행 + sha 2행 외 diff 0). 따라서 되돌리지 않고 적용했다.
- **부기 — 예상 밖이나 무해한 sha 이동 1건** [OBSERVED]: 출처 표의 `modeling/asset-manifest.md` sha 가 09-10 영수증과 다르다. `git status` 상 그 파일은 미수정이고 `git log` 상 09-10 영수증과 같은 커밋(`ed1ec73`)에 들어 있다 → 09-10 영수증은 modeling 레인의 커밋 전 편집 이전 상태를 찍은 것이다 [INFERENCE]. 그 문서에서 파생되는 `zones.json` 이 바이트 동일이므로 파생값 영향 0.

**검증 (재생성 후)** [OBSERVED 2026-09-11]:
```
$ node _workspace/current/planning/validate-campaign.mjs _workspace/current/planning/campaign.json --t0 _workspace/current/systems/data/t0
summary {'checks': 5, 'pass': 5, 'fail': 0, 'verdict': 'PASS'}   · sourceShaMatchesLiveCampaign True · exit 0
T0-01 PASS  beats.json ⊂ campaign T0 비트 · completion(및 술어 원문) 존재·일치
T0-02 PASS  campaign 힌트 33/33 3단 · t0 3비트 × 3단이 hints.json 과 문자열 일치
T0-03 PASS  records.clueIds·originId·sourceType ↔ campaign 단서 1:1 · 매체 2종
T0-04 PASS  zones.hub.systemIds ⊂ 계통 목록 · 전 viewNode cameraPose = style-guide §5 수치
T0-05 PASS  tools circuit/reader 확정 조건이 gdd §4 표와 일치

$ node _workspace/current/planning/validate-campaign.mjs _workspace/current/planning/campaign.json
summary {'checks': 49, 'pass': 49, 'fail': 0, 'verdict': 'PASS'}   · exit 0

$ node _workspace/current/planning/validate-evidence-graph.mjs
EG-SUMMARY checks 18 pass 18 fail 0 PASS   · exit 0
```

#### 2.1 2차 재생성 — QA D-CX012-01/02 정정 반영 [OBSERVED 2026-09-11, 1차 이후]

**동기**: QA 독립 검증(`qa/rfc-cx-012-review.md` R2-3)이 campaign.json 조사 불일치 2건을 냈고 planner(PlannerFix)가 제자리 정정했다 — `c4-b2.subtasks[3]` 「확대 판독로」→「확대 판독으로」 · `c6-b4.objective` 「역대조으로」→「역대조로」. campaign sha 가 다시 움직였으므로 T0 파생 데이터의 `sourceSha256` 이 살아 있는 원본과 어긋난다. 같은 회차에 QA 부기(§"기타 관찰")의 systems 재량 건 1개를 디렉터 승인(IRC) 하에 포함했다.

**같은 회차 systems 편집 1건** [OBSERVED]: `systems/pipeline/emit-tables.mjs` L896 `autoCopyCreated._src` 인용문 「첫 판독의 검증 사본 1점이 **보관함**에 생성」→「… **증거함**에 생성」(A-03 — campaign `t0-b3.completion` 정본과 어긋난 출처 주석 1어). `git diff --stat` → `1 file changed, 1 insertion(+), 1 deletion(-)` · `node --check` 통과. 이 문자열은 `beats.json` `completionPredicate.requires[].autoCopyCreated._src` 로 출력되는 주석 층위이며 검증기 검사 대상 필드가 아니다.

**명령** (1차와 동일):
```
$ node _workspace/current/systems/pipeline/emit-tables.mjs --scope t0 --out _workspace/current/systems/data/t0
exit 0 · validator checks 49 / pass 49 / fail 0 / PASS · scopeSource --scope · rows beats 3 · hints 9 · tools 2 · zones 1 · records 5
· plateAnchors 33/33 · ledgerAnchors 28/28 · clueIdOriginMedium 전건 일치
```

**diff 범위 (HEAD 대비)**: `git diff --stat -- _workspace/current/systems/data/t0/` → **10 files changed, 56 insertions(+), 56 deletions(-)** — 1차(55/55) 대비 **+1행 = `beats.json` L314 `_src` 「보관함에」→「증거함에」**(위 emitter 편집의 출력). 그 밖의 문자열 diff 는 1차 표와 **동일 4건**(t0-b2 objective · t0-b3 completion/_srcCompletion · hb-l1 · hb-l4). planner 정정 2곳(c4-b2 · c6-b4)은 **T0 밖 비트**라 T0 테이블 문자열에 나타나지 않고 `sourceSha256`(3파일) · `tables-receipt.json` `source.sha256` · 각 `.meta.md` §3 `campaign.json` 행만 움직였다. `tools.json`·`zones.json` **바이트 동일**(diff 0행). 수치·id 변화 **0건** → 되돌리지 않고 적용.

**검증 (2차 재생성 후)** [OBSERVED 2026-09-11]:
```
$ node _workspace/current/planning/validate-campaign.mjs _workspace/current/planning/campaign.json --t0 _workspace/current/systems/data/t0
summary {'checks': 5, 'pass': 5, 'fail': 0, 'verdict': 'PASS'} · sourceShaMatchesLiveCampaign True · exit 0
T0-01 PASS · T0-02 PASS · T0-03 PASS · T0-04 PASS · T0-05 PASS
$ node _workspace/current/planning/validate-campaign.mjs _workspace/current/planning/campaign.json
summary {'checks': 49, 'pass': 49, 'fail': 0, 'verdict': 'PASS'} · exit 0
$ node _workspace/current/planning/validate-evidence-graph.mjs
EG-SUMMARY checks 18 pass 18 fail 0 PASS · exit 0
```

- §3 의 런타임 판정은 그대로다 — `unity/**` 쓰기 0건, Unity 사본은 여전히 교체 전 campaign sha 를 들고 있으며 재복사는 M9/다음 회차 몫. 재복사 시 반영될 문자열은 위 4건 + `_src` 1행이다.
- **D-CX012-04 정정** [OBSERVED]: §2 diff 표 `beats.json` 행의 라벨 오기 2건(`t0-b1`→**`t0-b2`**(`rows[1]`) · `B-01`→**`A-03`**)과 §3 의 같은 참조 1곳을 제자리 정정했다. 데이터·동작 무관, 기록 정확성만.

### 3. Unity 런타임 파급 판정 [OBSERVED — `unity/Unknown/**` READ-ONLY grep, 쓰기 0건]

- **`unity/Unknown` 은 `_workspace/current/systems/data/t0/` 를 읽지 않는다.** 경로 문자열 `systems/data/t0` · `_workspace/current/systems/data` 의 참조 **0건**. Unity 가 읽는 것은 **별도 생성된 사본** `unity/Unknown/Assets/_Project/Data/Tables/{beats,hints,tools,zones,records,tables-receipt}.json`(각 `.meta.md` §1 명령: `emit-tables.mjs --out …/Assets/_Project/Data/Tables` · `emittedUtc 2026-09-10T07:54:33Z` · `source.sha256 8a43d334…` = **교체 전 campaign**) 과 그로부터 임포트된 ScriptableObject(`Data/Authoring/T0Catalog.asset` · `Records/*.asset` · `Tools/*.asset` · `Zones/hub.asset` — 옛 생성 JSON 을 문자열로 내장).
- **판정: 사본이며, 이번 재생성의 런타임 파급은 0.** `git status -- unity/` 의 변경 26건은 병행 중인 M9 세션의 것이고 `Data/Tables/**` · `Data/Authoring/**` 는 그 목록에 없다(내가 쓴 것도 0건).
- **단, 다음 임포트 회차에 fail-closed 로 걸린다** [OBSERVED, `Editor/T0AssetImporter.cs` L22-25]: `Import()` 는 사본 영수증의 `source.sha256` 을 **살아 있는** `../../../_workspace/current/planning/campaign.json` 의 sha 와 대조하고 다르면 `InvalidOperationException("Producer source differs from campaign.json")` 을 던진다. campaign 이 ACK-a 로 바뀌었으므로 현재 사본으로 `Tools/T0/Import generated tables` 를 다시 돌리면 **실패한다**(의도된 신선도 게이트 — 조용한 부패 아님). 반면 `T0Verification.RunAll` → `T0DataLoader.LoadVerified(files, receipt, receipt)` 는 사본과 사본 영수증만 대조하므로(L26-40) 기존 임포트 자산의 플레이모드·검증은 영향 없다.
- **unity 측 재복사(`emit-tables.mjs --out …/Data/Tables` 재실행 + 재임포트)는 M9 세션/다음 회차 몫이다.** 본 레인은 `unity/`·`assets/` 를 건드리지 않았다. 재복사 시 사본에 반영될 문자열은 위 §2 diff 표의 4건(t0-b2 objective · t0-b3 completion · hb-l1 · hb-l4)이며 수치·id 변화가 없으므로 `RecordAsset`/`ZoneAsset`/`ToolAsset` 스키마 변경은 불필요하다 [INFERENCE — 실제 재임포트 0회].

### 4. 소유 파일 밖 무변경 · 하지 않은 것

- 쓴 파일: `systems/game-ui-contract.json`(1행) · `systems/game-ui-contract.meta.md` · `systems/data/t0/*.json` + `*.meta.md`(생성기 출력 10파일 — 손 편집 0) · `systems/pipeline/emit-tables.mjs`(§2.1 `_src` 1어, 디렉터 IRC 승인) + `emit-tables.meta.md`(변경 로그 1행) · 이 문서. `unity/**` · `assets/**` · `production/decision-log.md` · `planning/**` · `synopsis/**` · `worldview/**` 쓰기 **0건**. 삭제·git add/commit·포매터·전체 테스트 실행 0건.
- 재지 않은 것: Unity 임포트 0회 · 빌드 0회 · 플레이 표본 n=0. G1~G7 어느 게이트도 이 문서로 오르지 않는다.
- 고정 sha 숫자는 옮겨 적지 않았다(RFC-Q1) — 값은 `systems/data/t0/tables-receipt.json` 과 각 `.meta.md` 가 갖고, 인용 시 생성기를 다시 돌린다.
