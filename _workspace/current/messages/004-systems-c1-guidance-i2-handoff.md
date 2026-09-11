---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
from: Aside session 4YZkiMgTsPz5dKGJ (systems lane, read-only after 13:13 KST)
to: C1 M4 session (director/systems, RFC-CX-005)
decision-basis: user choice 2026-09-11 13:12 KST — "M4 세션이 마무리"
---

# C1 기본 안내 정답 비노출(증분 #2) 인계 — M4 세션이 마무리한다

[OBSERVED] 사용자가 13:12 KST에 "M4 세션이 마무리"를 선택했다. 이 시점 이후 Aside 세션은 `unity/Unknown/Assets/_Project/**` 공유 소스를 더 편집하지 않는다. 아래는 M4가 이어받아야 할 사실·잔여 결함·제안·붙여넣기용 테스트다. 저장소 커밋/푸시는 하지 않았고 요청하지도 않는다.

## 1. 지금까지 실제로 일어난 일

| 시각(KST) | 주체 | 사실 |
|---|---|---|
| 02:08 | Aside | `Tests/PlayMode/C1PlayModeTests.cs`에 `DefaultPatrolGuidanceDoesNotRevealSolutionOrMutateState` 1건 추가(현재 파일 sha256 `fb31cfb9…9f7ebc`). 목적: C1 첫 진입 화면의 전체 `UI.Text`에 정답 행동 문구(`조명 분기를 접`, `판독 분기만 남`)와 단서 출처 id(`id`/`originId`/`rootOriginId`)가 없고, `Render()` 재호출이 상태 해시·HeadSeq·save.json 바이트·포커스·액션 목록·영수증 수를 바꾸지 않는다. |
| 02:08 → 12:21 | Aside | Aside 샌드박스에서 Unity 배치 실행 2회 시도, 둘 다 **테스트 도달 전** 실패(IL 후처리 프로세스: FSEvents 재귀 → `getdomainname` EPERM). LaunchServices `open`도 거부. 즉 **Aside가 만든 깨끗한 RED 영수증은 없다.** 상세: `systems/tech-verification/c1-guidance-i2-20260911.md`. |
| 12:50 → 13:08 | M4 | M4의 PlayMode 실행이 위 테스트를 포함했다: `c1-m4/results/playmode-2,3,5,6,7.xml` **Passed**, `playmode-4.xml` **Failed**(setup `continue-c1` 단계, 정답 노출 단정이 아님). |
| 12:56 | M4 | `App/C1GameSession.cs:70` 기본 본문의 조합 이유 문장을 `PatrolText("c1.patrol.preview")`로 교체 → **첫 진입 누출은 해소됨.** |

## 2. 잔여 결함 [OBSERVED — 13:13 KST 소스 정적 판독, 실행 미확인]

두 단서를 모두 관찰한 뒤 저작된 초기 조합(조명 켜짐·판독 켜짐)에서:

- `App/C1GameSession.cs:87-88` `c1-confirm` 액션의 `Detail` = `PatrolDiagnostic(verdict)` → `PatrolText("c1.patrol.invalid.sharedSupply")` → `Resources/C1PatrolContract.json:234` **"조명과 판독이 같은 배전을 쓰므로 조명 분기를 접어야 한다."** 가 메인 화면 본문 영역(`T0Interface.FlowText`)에 그대로 렌더된다.
- 같은 문자열이 `App/T0GameSession.cs:270`(확정/미리보기 오버레이의 stale proposal 진단)과 상태 문구 경로(`:114`, `:124`, `:128`)로도 나갈 수 있다.

따라서 증분 #2의 두 번째 요건 — **관찰된 잘못된 조합은 "무엇이 모순이라 확정이 막히는가"만 설명하고, 직접 교정 행동은 기존 힌트 계층(`narrative.hints[2]`, 스포일러 경고 뒤)만 준다** — 는 아직 미충족이다. `c1.patrol.invalid.readerOff`("판독 분기가 접혀 있어 수문 자료를 열람할 수 없다.")와 `c1.patrol.valid.readerOnly`는 모순/상태 서술이므로 그대로 둬도 된다.

참고: 관찰 전 `c1-confirm` 상세는 `c1.patrol.blocked.observations`("당직일지와 수문 계통판의 단서를 확인해야 한다.")로 "무엇을 살펴볼지" 안내로 적합하다. 본문이 `preview` 라벨만 남아 관찰 전 안내가 약해졌는지는 M4 판단.

## 3. 제안 — 계약 JSON을 건드리지 않는 최소 변경 (채택 여부는 M4)

1. **UI 문자열 표에 덮어쓰기 항목** (`Resources/T0Strings.json`, 손으로 관리되는 표이며 `emit-tables.mjs` 대상이 아님. `L()`은 `en` 부재 시 `ko` 폴백):
   ```json
   "c1.patrol.invalid.sharedSupply": { "ko": "조명과 판독이 같은 배전을 쓴다. 지금 조합으로는 열람을 확정할 수 없다." }
   ```
   독립 재검수(13:2x KST, 별도 서브에이전트) 판정: 이 문구는 직접 행동·출처 id를 말하지 않고 모순만 설명한다(누출 없음). "두 분기가 함께 켜진 지금 조합으로는 …" 변형도 누출은 아니지만 약간 더 유도적이라 짧은 쪽을 추천한다. 같은 재검수에서 §4의 금지 부분문자열(`조명 분기를 접`, `판독 분기만 남`)이 허용 상태 서술 "조명 분기는 접히고 판독 분기는 남아 있다."와 문자 단위로 불일치함(`를`/`는`, `만`/`는`)을 확인했다.
2. **`PatrolText`가 UI 표를 우선** (`App/C1GameSession.cs:23`):
   ```csharp
   string PatrolText(string key)=>strings?[key]!=null?L(key):(string)patrolPacket["localization"]?[key]??key;
   ```
   → `PatrolDiagnostic`·확정 상세·오버레이 본문·상태 문구가 한 번에 모순 문장으로 바뀐다. `planning/c1-patrol-contract.json`과 Resources 사본, `narrative.hints`는 무수정.
3. (선택) 본문 안내를 관찰 상태로 분기: 미관찰 → `c1.patrol.blocked.observations`, 관찰 완료 → 현재 조합 `ReasonKey`(덮어쓰기 적용; 유효 조합이면 `valid.readerOnly` 상태 서술). 현재 `preview` 라벨 유지도 요건 위반은 아니다.
4. 새 퍼즐 규칙·출처 특정 해답·서사 사실은 추가하지 않는다. 직접 행동 문장은 `narrative.hints[2]`에만 남는다.

## 4. 두 번째 회귀 테스트 (붙여넣기용, `C1PlayModeTests` 내부)

현재 코드에서는 첫 `DoesNotContain("조명 분기를 접")`에서 **RED**(확정 버튼 상세의 행동 문장)이어야 하고, §3 적용 후 GREEN이 기대된다. `Contains("같은 배전")`·`Contains("확정할 수 없다")` 두 줄은 §3의 제안 문구 기준이므로 채택 문구에 맞춰 바꾸되 `DoesNotContain` 가드는 유지한다.

```csharp
        [UnityTest] public IEnumerator ObservedInvalidRoutingExplainsContradictionWithoutPrescribingAction()
        {
            Click("continue-c1");
            foreach(var id in game.Definition.Patrol.Observations){Click("c1-open-"+id);Click("c1-observe-"+id);Click("c1-close-observation");}
            yield return Wait(game.FlushSaves());
            Assert.IsTrue(game.Definition.Patrol.Observations.All(id=>game.Journal.State.Has("c1:observed:"+id)));
            Assert.IsTrue(C1PatrolDefinition.Enabled(game.Journal.State,"lighting")&&C1PatrolDefinition.Enabled(game.Journal.State,"reader"),"Authored initial preview keeps both branches enabled");
            var contract=Newtonsoft.Json.Linq.JObject.Parse(Resources.Load<TextAsset>("C1PatrolContract").text);
            string Screen()=>string.Join("\n",host.GetComponentsInChildren<UnityEngine.UI.Text>().Select(t=>t.text));
            var text=Screen();
            StringAssert.DoesNotContain("조명 분기를 접",text,"Observed invalid routing must explain the contradiction without prescribing the action");
            StringAssert.DoesNotContain("판독 분기만 남",text,"Observed invalid routing must explain the contradiction without prescribing the action");
            StringAssert.Contains("같은 배전",text,"Observed invalid routing must name the shared-supply contradiction");
            StringAssert.Contains("확정할 수 없다",text,"Observed invalid routing must say why confirmation is blocked");
            StringAssert.DoesNotContain((string)contract["localization"]["c1.patrol.blocked.observations"],text,"Inspection guidance must stop once both clues are observed");
            Assert.IsFalse(game.Interface.Focus("c1-confirm"),"Invalid routing stays blocked");
            var head=game.Journal.HeadSeq;var direct=(string)contract["narrative"]["hints"][2];
            Click("hints");Assert.AreEqual("hints",game.Surface);StringAssert.DoesNotContain(direct,Screen());
            Click("hint-next");Click("hint-next");StringAssert.DoesNotContain(direct,Screen(),"Levels 1-2 stay directional");
            Click("hint-next");Assert.AreEqual("hintWarning",game.Surface);Click("hint-reveal");Assert.AreEqual("hints",game.Surface);
            StringAssert.Contains(direct,Screen(),"Only the existing spoiler-gated hint layer gives the direct corrective action");
            Assert.AreEqual(head,game.Journal.HeadSeq,"Hint browsing must not append commands");
            game.Back();Assert.AreEqual("shell",game.Surface);
            Click("c1-toggle-lighting");yield return Wait(game.FlushSaves());
            text=Screen();
            StringAssert.Contains((string)contract["localization"]["c1.patrol.valid.readerOnly"],text,"Valid routing describes the current state");
            StringAssert.DoesNotContain("확정할 수 없다",text,"Valid routing no longer shows the contradiction");
            Click("c1-condition");Assert.IsTrue(game.Interface.Focus("c1-confirm"),"Valid routing with acknowledged condition enables confirmation");
        }
```

## 5. 사용자가 이 증분에 요구한 검증 순서 (원문 요약)

focused RED(의미 있는 실패) → 최소 변경 → focused GREEN → 전체 EditMode/PlayMode → BuildMac → 런타임 UI 변경이 빌드에 들어갔으면 네이티브 스모크 → 범위 한정 증거에 정확한 결과와 **자동 테스트의 한계** 명시 → **미커밋 상태로 정지** → 응답 전 변경된 안내 문구의 정답 누출을 독립 재검수.

독립 재검수 방법 제안: 기본 화면 / 두 단서 관찰 후(양쪽 켜짐) / 조명 접은 뒤 세 화면의 모든 `UI.Text`를 덤프해 `조명 분기를 접`, `판독 분기만 남`, `watchlog-bureau`, `brine-log-gate3`, `c1-b1-c1`, `c1-b1-c2` 부재를 확인한다. Aside의 §2 판정은 소스 정적 판독이며 실제 렌더 문자열은 미확인이다.

## 6. decision-log 초안 (append는 M4가; Aside는 병행 편집 충돌을 피해 미편집)

```markdown
## RFC-CX-006 · C1 기본 안내 정답 비노출 소유권 (2026-09-11)
- lane: systems (Aside 4YZkiMgTsPz5dKGJ) → systems/director (C1 M4)
- question: C1 첫 진입·관찰 후 안내에서 정답 행동/출처를 어느 세션이 제거·검증하는가
- decision: 사용자 13:12 KST "M4 세션이 마무리". Aside가 추가한 PlayMode 회귀 1건은 M4 실행에 이미 포함(playmode-2/3/5/6/7 Passed). 잔여: 관찰 후 잘못된 조합의 확정 상세가 계약 문자열(행동 지시)을 그대로 표시 → M4가 messages/004 §3~§4로 해소.
- evidence: messages/004-systems-c1-guidance-i2-handoff.md, systems/tech-verification/c1-guidance-i2-20260911.md
```
