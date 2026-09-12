---
updated: 2026-09-12
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-planner
---

# t0 objective 표시 문면 교정안 (M13) — planner 적용 지시 1건

[OBSERVED] RFC-CX-016 이월 항목 "t0-b2 objective 저작 지시문 제거 + t0-b1 무스포일러 재작성"(`production/task-manifest.md:106,137`, D-M9-11)을 조사한 결과 **두 항목의 상태가 서로 다르다**. 이 문서는 `planning/campaign.json`이 병행 편집 중이라 **직접 수정하지 않고** 남기는 적용 지시다. 기술 근거는 [`systems/tech-verification/remaining-m13/notes.md`](../systems/tech-verification/remaining-m13/notes.md) §2.

소유: planner(`campaign.json` 문면) + systems(생성·임포트·테스트 리터럴). 실행 전 디렉터 판정 필요 — **테스트 리터럴 3곳이 함께 바뀐다**(§4).

## 1. t0-b2 — 조치 불필요 (이미 해소)

- 현행 `campaign.json` `stages[0].beats[1].objective` / `Data/Tables/beats.json` `rows[1].objective`:
  > 회로 지도에 오늘 밤 판독 가능한 범위와 "기록 밖" 구획을 직접 접어 표시하고 법1을 손으로 익힌다.
- 문제였던 둘째 문장 `안내 표시가 각 단계에 붙는다.`는 **이미 없다**. RFC-CX-012 B-14가 objective 산문의 튜토리얼 스캐폴딩 지시문 9곳을 제거하고(`worldview/term-audit-20260911.md:207-210` 지적 → `systems/rfc-cx-012-ack.md:48`), RFC-CX-013이 Unity 사본까지 재동기했다(`systems/tech-verification/rfc-cx-013-tables-resync.md:66`, 커밋 `6dccbc6`).
- **적용할 것 없음.** D-M9-11은 오류 이월로 종결한다(디렉터 확인 2026-09-12). 정보는 데이터의 `toolTeaching.mode: guided`가 갖고 있고 런타임은 `GuidedTeachingText()`가 그것으로 티칭 헤더를 조립한다 — 산문에 지시문을 병기할 필요가 없다.

## 2. t0-b1 — **열린 항목** (무스포일러 재작성)

### (a) 현행 문면 — 화면에 한 번도 나오지 않는다

`campaign.json` `stages[0].beats[0].objective` (154자):

> 당직실을 인수하고, 자신이 구 서고에서 빼내 숨겨둔 판 #0을 오늘 밤 **이관 목록**에 올릴지 결정한다. **인수 각서**가 오늘 밤의 산출물을 한 줄로 못박는다 — 이 당직이 끝나면 남는 것은 청문에 낼 제출 문서 1건이며, 그 문서에 무엇을 근거로 적을지가 오늘 밤 내내의 결정이다.

[OBSERVED] 레코드 표시명 2개(`인수 각서`·`이관 목록`)를 담고 있어 CaseThread disclosure 가드(`T0GameSession.CaseObjective`, `App/T0GameSession.cs:203-207`)가 고정 문구로 폴백시킨다. 플레이어가 t0-b1에서 실제로 보는 목표는 `T0Strings.json` `caseObjective` = `결손 4시간의 양 끝을 두 기록으로 고정`(22자)이며, **이는 t0-b3의 과제**다. 즉 현행은 (i) 저작 문면이 사장되고 (ii) 첫 비트에 마지막 비트의 목표가 표시되는 이중 결함이다.

### (b) 제안 문면 — 무스포일러 · 가드 통과 · 카드 예산 준수

**채택 권고: S2** (49자)

> 당직실을 인수하고, 숨겨 둔 판 #0의 이관 여부와 오늘 밤 제출 문서의 근거를 정한다.

대안:

| 안 | 자수 | 문면 |
|---|---|---|
| S1 | 43 | 당직실을 인수하고, 숨겨 둔 판 #0을 오늘 밤 이관 처리에 올릴지 결정한다. |
| **S2** | **49** | **당직실을 인수하고, 숨겨 둔 판 #0의 이관 여부와 오늘 밤 제출 문서의 근거를 정한다.** |
| S3 | 52 | 당직실을 인수해 오늘 밤의 기록을 열람하고, 숨겨 둔 판 #0을 이관 처리에 올릴지 결정한다. |

세 안 모두 [OBSERVED] 레코드 표시명 5종(`인수 각서`·`이관 목록`·`당직실 표준판`·`근무 규정 필사본`·`기록국 조위대장`)을 **부분문자열로도 포함하지 않는다** → 가드를 통과해 **문면 그대로 표시된다**(python 대입 검증).

설계 근거:

1. **무스포일러**: "어느 문서를 읽어라"를 목표문에서 뺐다. 어떤 기록이 근거가 되는지는 플레이어가 찾는 것이고, 다음 행동 안내는 이미 `caseNext`+`caseRead`(`기록을 읽고 근거를 살펴보세요.`)가 담당한다. 목표문은 **의도**(판 #0 처리 · 제출 문서의 근거)를 담는다.
2. **길이 예산** [OBSERVED — 이게 S안을 짧게 만든 이유]: 사건 흐름 카드는 고정 높이(`UI/T0Interface.cs:93` `height=.185f*scale`)에 4행(제목·진척 / 목표 / 상세 / 다음 행동)을 담고, `T0CaseThreadTests.cs:226`이 `card.preferredHeight <= rect.height+1`로 **잘림 0**을 단정한다. 표시되는 형제 목표는 t0-b2 **57자** · t0-b3 **48자**다. 현행 154자를 폴백 해제만 하면 `textScale 1.5`에서 이 단정을 깨뜨릴 위험이 크다 — 그래서 제안은 48~57자 대역에 맞췄다.
3. **사장된 서술의 이전**: 빠진 프레이밍("인수 각서가 산출물을 한 줄로 못박는다 — 남는 것은 청문 제출 문서 1건")은 버리지 말고 **가드 밖 표면**으로 옮긴다. 후보: `beats[0].hints[0]`(힌트 1단은 레코드명 허용 표면이다) 또는 `T0Strings.json` `intro`(이미 `21:00. 인수 각서와 이관 목록을 살핀 뒤…`로 레코드명을 쓴다) 또는 `inference`. 목표문이 아니라 도입·힌트가 맥락을 지는 쪽이 disclosure 계약과 맞는다.

### (c) 적용 절차

planner가 (b)의 한 안을 확정한 뒤, **순서대로**:

```
# 1) 저작 원본 1리프 교체 (planner)
#    _workspace/current/planning/campaign.json  stages[0].beats[0].objective
#    — objective 1개만. clues/action/inference/completion/consequence/hints 불변.

# 2) 규칙 검증 (planner 몫 · fail-closed)
node _workspace/current/planning/validate-campaign.mjs

# 3) systems 데이터 재생성
node _workspace/current/systems/pipeline/emit-tables.mjs --scope t0 \
  --out _workspace/current/systems/data/t0

# 4) Unity 사본 재발행 (같은 생성기 · 다른 --out)
node _workspace/current/systems/pipeline/emit-tables.mjs --scope t0 \
  --out unity/Unknown/Assets/_Project/Data/Tables

# 5) Unity 재임포트 — 반드시 4) 뒤에. 임포터가 사본 영수증의 source.sha256 을
#    살아 있는 campaign.json 과 대조하므로, 4)를 건너뛰면 fail-closed 로 막힌다
#    ("Producer source differs from campaign.json", Editor/T0AssetImporter.cs L22-25).
UNITY=/Applications/Unity/Hub/Editor/6000.5.6f1/Unity.app/Contents/MacOS/Unity
PROJ=/Users/jangyoung/orca/unknown/unity/Unknown
$UNITY -batchmode -nographics --burst-disable-compilation -projectPath $PROJ \
  -executeMethod Tide.EditorTools.T0AssetImporter.Import -logFile /tmp/t0-b1-import.log
```

[OBSERVED] `Data/Tables/*`·`systems/data/t0/*`는 **생성물**이므로 손으로 쓰지 않는다(`systems/architecture-contract.md:113`, `systems/data-schemas/beats.md` M1·M4). `beats.json`은 `campaign.json`의 바이트 동일 사본이라(M2) objective 1리프 외의 diff가 나오면 그 자체가 오류 신호다.

## 3. 런타임 가드는 넣지 않는다

[DECISION] 저작 지시문을 런타임에서 걸러내는 가드(예: objective 둘째 문장 버리기)는 **구현하지 않았다**. (i) t0-b2는 데이터에서 이미 해소됐으므로 걸러낼 대상이 없고, (ii) 문장 단위 휴리스틱은 `M9CoreTests.CaseObjectiveFollowsTheBeatsTableThroughTheDisclosureGuard`가 지키는 "disclosure-clean 문면은 그대로 표시" 계약을 약화시키며, (iii) 기존 `CaseObjective` 가드의 책임은 **레코드 표시명 노출 차단**이고 저작 지시문 검출은 그 경계 밖이다. 데이터 수정이 정석이고, 런타임 가드는 데이터를 고칠 수 없을 때의 임시방편이다 — 지금은 그 조건이 아니다.

## 4. §2 적용 시 함께 바뀌는 테스트 리터럴 3곳 (systems)

[OBSERVED] t0-b1이 가드를 **통과하게** 되면 현행 기대값 3개가 낡는다. 적용 회차에 같이 고쳐야 하며, **테스트를 지우거나 느슨하게 만드는 방식은 금지**다:

| 위치 | 현행 | 적용 후 |
|---|---|---|
| `Tests/EditMode/M9CoreTests.cs:57` | `StringAssert.Contains("인수 각서", t0-b1 objective, "Guard precondition: …")` | 저작 데이터 의존을 끊고 **합성 문자열**로 음성 케이스를 만든다 — 예: `CaseObjective`에 `"…인수 각서…"`를 담은 인라인 `JObject`를 넘겨 폴백을 단정. 가드의 음성 경로 커버리지를 유지하는 것이 목적이므로 단정을 삭제하지 않는다 |
| `Tests/EditMode/M9CoreTests.cs:58` | `Assert.AreEqual("fallback", CaseObjective(beats,"t0-b1",…))` | 위 합성 케이스로 이전. 실데이터 t0-b1은 이제 **문면 그대로** 단정으로 바뀐다 |
| `Tests/PlayMode/T0CaseThreadTests.cs:203-208` `ExpectedObjective()` | `default:` → `"결손 4시간의 양 끝을 두 기록으로 고정"` | `case "t0-b1":` → 새 t0-b1 문면. `default:`는 폴백 문구로 남겨 둔다(가드 폴백 경로가 여전히 존재) |

추가 확인 1건: 적용 후 `T0CaseThreadTests.cs:226`의 잘림 단정을 `textScale` 최대(1.5)에서 통과하는지 실행으로 확인한다 — (b)-2의 길이 예산이 근거지만 **실측이 아니다** `[INFERENCE]`.

## 5. 경계

- [OBSERVED] 이 문서는 지시서다. `planning/campaign.json`은 **읽기만** 했고 쓰지 않았다(병행 편집 중). `Data/Tables/*`·`systems/data/t0/*` 쓰기 0건, 생성기 실행 0회, Unity 임포트 0회.
- [OBSERVED] 부수 관측(결함 아님): `Resources/C1SignatureContract.json`의 `narrative.action`에 `이번에는 안내 표시가 붙지 않는다.`가 남아 있으나 **`narrative.action`을 읽는 런타임 코드는 0건**이다(패킷에서 화면으로 나가는 필드는 `narrative.objective`(순찰만) · `narrative.hints` · `observations[].description` · `localization.*`). 노출 없음 → 설계 필드로 유지. `campaign.json:330`의 같은 문장도 동일.
- [CARRIED] §2 문안 확정(planner) → §4 테스트 리터럴 동반 수정(systems) → §2(c) 5단계 실행. 이 회차에서는 **어느 단계도 실행하지 않았다.**
