---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
---

# RFC-CX-013 — T0 파생 데이터 3차 재생성 · Unity 사본 재발행 · 가드드 헤드리스 임포트/검증 영수증

## 0. 지위 · 왜 있는가

RFC-CX-012 이월 해소(RFC-CX-013) wave 2 의 systems 레인 산출물. planner 가 campaign.json 28리프를 교체하고 K-07 을 신설(validate-campaign **50/50**)한 뒤, T0 파생물(`systems/data/t0/*`)과 Unity 사본(`unity/Unknown/Assets/_Project/Data/Tables/*`)이 교체 전 campaign sha 를 들고 있어 `--t0` 의 `sourceShaMatchesLiveCampaign=false`, Unity `T0AssetImporter.Import()` fail-closed(`Producer source differs from campaign.json`) 상태였다. 이 문서는 ① t0 3차 재생성 ② Unity 사본 재발행 ③ Unity 프로세스 가드 뒤 헤드리스 임포트·T0 검증 배치 — 세 가지의 명령 원문·관측·diff 범위를 기록한다. **어떤 게이트도 이 문서로 오르지 않는다.** 모든 sha 는 각 `tables-receipt.json` 이 갖고 여기엔 앞 12자리만 인용한다(RFC-Q1).

선행 확인 [OBSERVED]: `.claude/agents/game-systems-designer.md`(소유 경로 = `systems/**` + 코드). main = `1bd0097`. 실행 전 `git status --porcelain -- unity/` = `?? …/Art/Candidates/m8-review-card.meta` 1건(M9 잔여, 본 레인 무관).

## 1. 스코프 확정 — Unity 사본은 `--scope t0` [OBSERVED]

`rfc-cx-012-ack.md` §3 은 사본 명령을 `emit-tables.mjs --out …/Data/Tables` 로만 적었고, 상위 지시는 `--scope all` 로 추정했다. 세 증거로 **`--scope t0`** 로 확정:

| 증거 | 관측 |
|---|---|
| 사본 영수증 `Data/Tables/tables-receipt.json` L5-6 (09-10 판) | `"scope": "t0"` · `"scopeSource": "--scope"` |
| 사본 파일 집합 | `beats/hints/tools/zones/records.json` 5종 + `.meta.md` 6종 — `emit-tables.mjs` 머리 주석 L10 대로 `scope=all` 은 beats+hints **2종만** 낸다 |
| `.meta.md` §1 의 `--out` 만 있는 명령 | 생성기 L1011 템플릿이 `--scope` 를 항상 생략하고 찍는 문자열 → 증거 아님 |

## 2. t0 3차 재생성 [OBSERVED 2026-09-11]

```
$ node _workspace/current/systems/pipeline/emit-tables.mjs --scope t0 --out _workspace/current/systems/data/t0
exit 0 · validator checks 50 / pass 50 / fail 0 / PASS · scope t0 · scopeSource --scope
· rows beats 3 · hints 9 · tools 2 · zones 1 · records 5 · plateAnchors 33/33 · ledgerAnchors 28/28 · toolInvariants T-I1/I3/I4/I7 PASS
```

`git diff --stat -- _workspace/current/systems/data/t0/` → **10 files changed, 28 insertions(+), 28 deletions(-)** (HEAD `1bd0097` 대비; 2차 판이 HEAD 에 커밋되어 있으므로 2차→3차 차분).

| 파일 | 바뀐 것 |
|---|---|
| `beats.json` | `t0-b2.action`(`rows[1]`, L181) 「…유일한 회로 지도 **훈련**이다.」→「…**연습**이다.」 — campaign.json L127 원문 그대로(planner 훈련→연습 3건 중 1건이 T0 범위) · `sourceSha256` |
| `hints.json` · `records.json` | `sourceSha256` 만(records 는 `recordsDocSha256` 불변) |
| `tools.json` · `zones.json` | **바이트 동일**(stat 부재) |
| `tables-receipt.json` | `emittedUtc` · `source.sha256/bytes` · `validator.checks/pass` 49→**50** · `tables[beats].sha256/bytes` · `tables[hints/records].sha256` |
| `*.meta.md` ×6 | `updated` · §2 sha/바이트/검증기 판정 `checks 49`→`50` · §3 `campaign.json` 행 |

상위 지시의 "T0 범위 문자열 변화 0 예상" 은 **1건 어긋남** — 위 `t0-b2.action` 1리프. 수치·id 변화 0.

검증:
```
$ node _workspace/current/planning/validate-campaign.mjs _workspace/current/planning/campaign.json --t0 _workspace/current/systems/data/t0
summary {'checks': 5, 'pass': 5, 'fail': 0, 'verdict': 'PASS'} · sourceShaMatchesLiveCampaign True · exit 0
T0-01 PASS · T0-02 PASS · T0-03 PASS · T0-04 PASS · T0-05 PASS
```

## 3. Unity 사본 재발행 [OBSERVED 2026-09-11]

```
$ node _workspace/current/systems/pipeline/emit-tables.mjs --scope t0 --out unity/Unknown/Assets/_Project/Data/Tables
exit 0 · validator 50/50 PASS · scope t0 · scopeSource --scope · emittedUtc 2026-09-11T15:43:39Z
· 5 테이블 sha = §2 t0 출력과 전부 동일(beats d2d1af05c99f… · hints d35ada6fd327… · tools 1cfa4654173b… · zones 3c3d043f6f45… · records 2bb67911e6e4…)
```

`git diff --stat -- unity/Unknown/Assets/_Project/Data/Tables/` → **10 files changed, 65 insertions(+), 65 deletions(-)** (09-10 판 → 오늘). 문자열 변화 전건(`git diff -U0` 에서 sha·날짜·바이트·checks 행 제외):

| 파일 | 리프 | 전 → 후 | 출처 |
|---|---|---|---|
| `beats.json` | `rows[1].objective` (t0-b2) | 말미 「안내 표시가 각 단계에 붙는다.」 삭제 | B-14 |
| `beats.json` | `rows[1].action` (t0-b2) | 「훈련이다」→「연습이다」 | planner 훈련→연습 |
| `beats.json` | `rows[2].completion` · `completionPredicate._srcCompletion` (t0-b3) | 「보관함에」→「증거함에」 | A-03 |
| `beats.json` | `rows[2].completionPredicate.requires[3]._src` | 「보관함에 생성」→「증거함에 생성」 | A-03(emitter L896, 2차 회차) |
| `records.json` | `rows[0].lines[0].text` (hb-l1) | 「판독기와 봉인대를」→「판독기와 서명대를」 | synopsis |
| `records.json` | `rows[0].lines[3].text` (hb-l4) | 「이관 시각까지 접수부에」→「이관 시각까지 청문 접수부에」 | synopsis |
| `tools.json` · `zones.json` | — | **바이트 동일** | — |

**수치·id 변화 0 [OBSERVED — python 리프 대조]**: HEAD 사본 vs 새 사본을 JSON 리프 단위로 대조(구조 키 집합 동일 assert 통과). `beats 211리프 / hints 103 / tools 185 / zones 259 / records 2084` 에서 **비문자열 리프 변화 0 · `*Id` 리프 변화 0** · 문자열 변화 = 위 표 7리프 정확히 · provenance 리프(`sourceSha256` ×3 · `recordsDocSha256` ×1)만 추가 이동. 되돌리지 않고 적용.

**sha 일치 표 [OBSERVED — python `hashlib` 독립 재계산]**:

| 대상 | 값(앞 12) | == live `campaign.json` sha | 테이블 sha == 파일 실측 |
|---|---|---|---|
| live `_workspace/current/planning/campaign.json` (124221 B) | `044780ef4349…` | — | — |
| `systems/data/t0/tables-receipt.json` `source.sha256` | `044780ef4349…` | **True** | 5/5 True |
| `unity/…/Data/Tables/tables-receipt.json` `source.sha256` | `044780ef4349…` | **True** | 5/5 True |
| (참고) 09-10 Unity 사본 `source.sha256` | `8a43d334f8f6…` (124007 B) | False — 교체 전 | — |

## 4. 가드드 헤드리스 — 임포트 · T0 검증 배치 [OBSERVED 2026-09-11]

**가드**: `pgrep -f "Unity.app/Contents/MacOS/Unity"` → **0건(exit 1)**, 각 실행 직전 재확인. 살아 있던 것은 VS Code Roslyn/`UnityCodeModel.dll`·MCP 서버뿐. Editor 바이너리 `/Applications/Unity/Hub/Editor/6000.5.6f1/Unity.app/Contents/MacOS/Unity` 존재(`ProjectVersion.txt` = 6000.5.6f1). 임포트 메서드 네임스페이스는 `Editor/T0AssetImporter.cs` L9-16 에서 `Tide.EditorTools.T0AssetImporter.Import` 로 확인.

### 4a. 임포트

```
$ /Applications/Unity/Hub/Editor/6000.5.6f1/Unity.app/Contents/MacOS/Unity -batchmode -nographics \
    -projectPath /Users/jangyoung/orca/unknown/unity/Unknown \
    -executeMethod Tide.EditorTools.T0AssetImporter.Import -logFile /tmp/unknown-cx013-import.log -quit
unity_exit=0 · 벽시계 15.2 s · 로그 52454 B
```

로그 인용(`/tmp/unknown-cx013-import.log`):
- L467 `T0 diagnostic catalog imported. Citation blockers: 0`
- L507 `Exiting batchmode successfully now!`
- `grep -c 'Exception'` → **0** · `grep -cE 'error CS'` → **0** · `Producer source differs` → **부재**(fail-closed 게이트 통과 = 사본 receipt sha == live campaign 의 Unity 측 재확인).

`Data/Authoring/` 갱신 파일(Unity 가 쓴 것만) — `git diff --stat` **6 files changed, 32 insertions(+), 32 deletions(-)**:

| 파일 | 바뀐 것 |
|---|---|
| `Authoring/T0Catalog.asset` | 내장 `tables-receipt.json` 문자열(emittedUtc · source sha/bytes · checks 49→50 · 테이블 sha/bytes · authoringSources records/manifest sha) |
| `Authoring/Records/rec-handover-brief.asset` · `rec-plate-standard-hub.asset` · `rec-tide-ledger-bureau.asset` · `rec-transfer-list.asset` · `rec-watchlog-bureau.asset` | 내장 `records.json` 문자열(`sourceSha256` · `recordsDocSha256` · hb-l1 「봉인대→서명대」 · hb-l4 「접수부→청문 접수부」 — 5 자산 모두 같은 records.json 전문을 내장하므로 동일 diff) |
| `Authoring/Tools/{circuit,reader}.asset` · `Authoring/Zones/hub.asset` | **무변경**(tools/zones.json 바이트 동일 → `Configure` 가 같은 문자열을 다시 씀) |

### 4b. T0 검증 배치

```
$ /Applications/Unity/Hub/Editor/6000.5.6f1/Unity.app/Contents/MacOS/Unity -batchmode -nographics \
    -projectPath /Users/jangyoung/orca/unknown/unity/Unknown \
    -executeMethod Tide.EditorTools.T0Verification.RunBatch -logFile /tmp/unknown-cx013-t0verify.log -quit
unity_exit=0 · 벽시계 12.6 s · 로그 33786 B
```

로그 인용(`/tmp/unknown-cx013-t0verify.log`):
- L371 `T0 diagnostic catalog imported. Citation blockers: 0` (`RunBatch` L273 이 `Import()` 를 다시 호출 — 2회째 임포트도 게이트 통과)
- L384 **`T0_M1_CHECKS tests=21 failures=0; full T0 remains blocked by provenance and unfinished UI/save.`**
- `Exception` **0** · `error CS` **0**. "Exiting batchmode" 행은 없다 — `RunBatch` L292 가 `EditorApplication.Exit(code)` 로 직접 종료하며 `code = failures==0 ? 0 : 1` → **exit 0 = PASS 21 / FAIL 0**.

**부기 — 소유 밖 산출물 1건 [OBSERVED, 되돌림]**: `RunBatch` L286 은 `unity/Unknown/results/t0-m1-contract-checks.xml`(git 추적, `.gitignore` 밖)을 다시 쓴다. 실행 후 `git diff --stat` → `1 file changed, 1 insertion(+), 1 deletion(-)` — 내용은 **testcase 1건 개명뿐**(`Actual_missing_provenance_blocks_citation_and_b3` → `Canonical_provenance_and_negative_missing_fixture`; tests 21 · failures 0 · 나머지 20 이름 동일). 새 이름은 HEAD 의 `T0Verification.cs` L155 에 이미 있고(`git log -S` → `68967c3`), XML 은 그보다 **뒤** 커밋 `a425dad` 이후 갱신되지 않았다 → HEAD 의 XML 이 HEAD 의 소스 대비 원래 스테일했던 것이지 이번 세션의 드리프트가 아니다. 상위 지시(unity/ 변경은 Data/Tables·Data/Authoring 밖 0)에 따라 새 XML 을 `/tmp/unknown-cx013-t0-m1-contract-checks.xml`(sha `171fe77d7b28…`) 로 보존한 뒤 `git checkout -- unity/Unknown/results/t0-m1-contract-checks.xml` 로 되돌렸다. **XML 재커밋 여부는 Main/M9 판단** — 다음 RunBatch 회차가 어차피 같은 내용을 다시 쓴다.

**재커밋 판정(director) [OBSERVED 2026-09-11, Main IRC]**: 추적 XML 은 현재 소스 상태(개명 반영)를 담아야 하므로 **재커밋 대상** — `/tmp` 보존본을 `cp` 로 원위치 복원(sha `171fe77d7b28…` 동일), `git diff --stat -- unity/Unknown/results/` → `1 file changed, 1 insertion(+), 1 deletion(-)` · testcase 21→21 · failures 0→0 · 차이 = 위 개명 1건뿐 재확인. 되돌림 문구는 이 판정으로 대체된다.

## 5. 재생성 후 검증 3종 [OBSERVED 2026-09-11]

```
$ node _workspace/current/planning/validate-campaign.mjs _workspace/current/planning/campaign.json
summary {'checks': 50, 'pass': 50, 'fail': 0, 'verdict': 'PASS'} · exit 0
$ node _workspace/current/planning/validate-campaign.mjs _workspace/current/planning/campaign.json --t0 _workspace/current/systems/data/t0
summary {'checks': 5, 'pass': 5, 'fail': 0, 'verdict': 'PASS'} · sourceShaMatchesLiveCampaign True · exit 0
$ node _workspace/current/planning/validate-evidence-graph.mjs
EG-SUMMARY checks 18 pass 18 fail 0 PASS · exit 0
```

## 6. unity/ 변경 경계 [OBSERVED — 최종 `git status --porcelain -- unity/`]

18행 = `Data/Tables/` **10**(생성기) + `Data/Authoring/` **6**(Unity 임포트) + `results/t0-m1-contract-checks.xml` **1**(`RunBatch` 출력, §4b director 판정으로 재커밋 대상) + `?? …/Art/Candidates/m8-review-card.meta` **1**(실행 전부터 있던 M9 잔여, 본 레인 무관). **Data/Tables·Data/Authoring·results XML 밖 본 레인 변경 0**. Unity 소스·씬·테스트·`ProjectSettings/` 쓰기 0건. `Library/`·`Logs/` 는 `.gitignore`.

## 7. 하지 않은 것 · 재지 않은 것

- 손 편집 0건(t0·Unity 사본은 생성기 출력만, Authoring 은 Unity 출력만). 삭제·`git add`/`commit`·포매터·전체 테스트 실행 0건. `production/decision-log.md`·`planning/**`·`synopsis/**`·`worldview/**` 쓰기 0건. `emit-tables.mjs`·`T0AssetImporter.cs`·`T0Verification.cs` 무편집.
- 컴파일 에러 0건이라 "고치지 않고 보고" 경로는 발동하지 않았다.
- 빌드 0회 · 플레이 표본 n=0. `T0_M1_CHECKS 21/0` 은 에디터 계약 검사이며 로그 자체가 "full T0 remains blocked by provenance and unfinished UI/save" 라고 말한다 — G 게이트 판정 아님.
- 09-10 판 사본 `.meta.md` 의 `--out` 만 있는 명령 문자열(생성기 템플릿 L1011)은 그대로 다시 찍혔다 — 재현 시 `--scope t0` 를 붙여야 `scopeSource: --scope` 가 유지된다(ACK-c §2 부기와 같은 함정). 템플릿 수정은 emitter 편집이라 이번 범위 밖.
