#if TIDE_TEST_FRAMEWORK
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Newtonsoft.Json.Linq;
using Tide.App;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tide.Tests
{
 // M18: the C1 interview *preparation* surface is display-only. It states that an interview format
 // exists; it carries no canonical dialogue. Disclosure exclusions are derived from the live data the
 // runtime itself reads (patrol contract actor name, records table display names) so this test stays
 // correct as canon grows instead of pinning canonical strings here.
 public sealed class C1InterviewPrepPlayModeTests
 {
  const string Entry="c1-interview-prep";
  const string Return="c1-interview-prep-back";
  string directory;GameObject host;T0GameSession game;
  [UnitySetUp] public IEnumerator SetUp()
  {
   directory=Path.Combine(Path.GetTempPath(),"c1-interview-prep-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(directory);
   // The real completed-C1 fixture the C1/M5/M8 suites already use — not fabricated state.
   File.Copy(Path.Combine(Application.dataPath,"_Project/Tests/Fixtures/C1PatrolCompletedV2.json"),Path.Combine(directory,"save.json"));
   Create();yield return null;game.StartGame();yield return null;
  }
  void Create(){host=new GameObject("C1 interview prep session");game=host.AddComponent<T0GameSession>();game.Initialize(Resources.Load<T0RuntimeConfig>("T0Runtime"),directory);}
  [UnityTearDown] public IEnumerator TearDown(){yield return Wait(game.FlushSaves());UnityEngine.Object.Destroy(host);yield return null;Directory.Delete(directory,true);}
  IEnumerator Wait(Task task){while(!task.IsCompleted)yield return null;if(task.IsFaulted)throw task.Exception;}
  void Click(string id){Assert.IsTrue(game.Interface.Focus(id),"Missing focusable action "+id);game.Interface.BeginActivation();game.Interface.EndActivation();}
  string[] Ids()=>game.Interface.ActionIds.ToArray();
  string VisibleText()=>string.Join("\n",host.GetComponentsInChildren<Text>().Select(t=>t.text));
  // FlowText names both the body and the status line "Text"; Button names its label "Label" under a
  // panel named after the action id. The M18 body is identified by its own lane text so the session
  // status line is never mistaken for M18-authored content.
  string PanelBody()
  {
   var flowed=host.GetComponentsInChildren<Text>().Where(t=>t.name=="Text").Select(t=>t.text).ToArray();
   return flowed.FirstOrDefault(t=>t.Contains("발화자 A"))??string.Join("\n",flowed);
  }
  string LabelOf(string id)=>host.GetComponentsInChildren<Button>().Single(b=>b.name==id).GetComponentsInChildren<Text>().Single(t=>t.name=="Label").text;
  string ActorDisplayName()=>(string)JObject.Parse(Resources.Load<TextAsset>("C1PatrolContract").text)["journalCondition"]["actorDisplayName"];
  string[] RecordDisplayNames()=>JObject.Parse(Resources.Load<T0RuntimeConfig>("T0Runtime").records.text)["rows"]
   .Select(r=>(string)r["displayNameKo"]).Where(n=>!string.IsNullOrEmpty(n)).Distinct().ToArray();

  [UnityTest] public IEnumerator CompletedPatrolOpensAnonymousInterviewFormatPanelAndReturnsWithoutAnyMutation()
  {
   // ---- the completed C1 patrol surface is genuinely reached before anything is asserted about M18
   Assert.IsTrue(game.PatrolActive,"fixture must be inside C1");
   Assert.IsTrue(game.PatrolComplete,"fixture must be the COMPLETED C1 patrol surface");
   Assert.IsFalse(game.SignatureActive,"must still be the C1 patrol surface, not the next beat");
   CollectionAssert.Contains(Ids(),"continue-c1-signature","completed patrol surface must be live");
   var surfaceBefore=game.Surface;Assert.AreNotEqual("interviewPrep",surfaceBefore);

   yield return Wait(game.FlushSaves());
   var hash=game.Journal.State.StateHash;var head=game.Journal.HeadSeq;
   var bytes=File.ReadAllBytes(Path.Combine(directory,"save.json"));var receipts=game.SuccessfulReceipts;

   // ---- RED anchor: the explicit interview action does not exist on the completed C1 surface yet
   CollectionAssert.Contains(Ids(),Entry,"completed C1 patrol must offer the interview preparation action");
   Assert.IsTrue(host.GetComponentsInChildren<Button>().Single(b=>b.name==Entry).interactable,"the interview action must be usable, not inert");
   var entryLabel=LabelOf(Entry); // captured here: the overlay clears the patrol screen's own actions

   Click(Entry);yield return null;
   Assert.AreEqual("interviewPrep",game.Surface,"the explicit action must open the interview surface");

   // ---- the player-visible promise: two anonymous speaker lanes, a neutral comparison prompt, a return
   var body=PanelBody();
   StringAssert.Contains("발화자 A",body,"lane A must be visible and anonymous");
   StringAssert.Contains("발화자 B",body,"lane B must be visible and anonymous");
   StringAssert.Contains("나란히 비교",body,"a neutral comparison prompt must be visible");
   CollectionAssert.Contains(Ids(),Return,"the panel must offer an explicit return");
   Assert.IsTrue(host.GetComponentsInChildren<Button>().Single(b=>b.name==Return).interactable,"return must be usable");
   Assert.IsNull(host.GetComponentsInChildren<Text>().FirstOrDefault(t=>t.name=="CaseThread"),
    "the patrol case card must not render over the interview panel");

   // ---- disclosure exclusions, derived from live data (design contract c1-interview-prep-m18.md §2).
   // The title is pinned exactly instead of digit-scanned: its only digit is the structural stage id
   // "C1", which every C1 screen already carries and which is not a value or a time.
   Assert.AreEqual("C1 · 면접 준비",game.Interface.CurrentTitle,"the panel title is pinned to the authored format label");
   var authored=body+"\n"+entryLabel+"\n"+LabelOf(Return);
   var all=VisibleText();
   StringAssert.DoesNotContain(ActorDisplayName(),all,"E1: no named speaker may appear");
   foreach(var recordName in RecordDisplayNames())
    StringAssert.DoesNotContain(recordName,all,"E2: no record/source display name may appear: "+recordName);
   Assert.IsFalse(authored.Any(char.IsDigit),"E3: M18-authored text must carry no exact value or time");
   StringAssert.DoesNotContain("“",authored,"E4: no canonical quoted statement");
   StringAssert.DoesNotContain("”",authored,"E4: no canonical quoted statement");

   // ---- display-only: opening the panel wrote nothing
   Assert.IsFalse(game.SavePending,"the panel must not queue a save");
   Assert.AreEqual(hash,game.Journal.State.StateHash,"no state hash change");
   Assert.AreEqual(head,game.Journal.HeadSeq,"no journal head change");
   Assert.AreEqual(receipts,game.SuccessfulReceipts,"no receipt change");
   CollectionAssert.AreEqual(bytes,File.ReadAllBytes(Path.Combine(directory,"save.json")),"no save bytes change");

   // ---- explicit return lands back on the same completed C1 surface
   Click(Return);yield return null;
   Assert.AreEqual(surfaceBefore,game.Surface,"return must restore the pre-entry surface");
   Assert.IsTrue(game.PatrolActive&&game.PatrolComplete,"return must land on the completed C1 patrol surface");
   Assert.IsFalse(game.SignatureActive,"return must not advance the beat");
   CollectionAssert.Contains(Ids(),"continue-c1-signature","the completed patrol actions must be intact");
   CollectionAssert.Contains(Ids(),"c1-review","the completed patrol actions must be intact");
   CollectionAssert.Contains(Ids(),Entry,"the interview action must remain available after returning");

   yield return Wait(game.FlushSaves());
   Assert.IsFalse(game.SavePending,"returning must not queue a save");
   Assert.AreEqual(hash,game.Journal.State.StateHash,"no state hash change across the whole round trip");
   Assert.AreEqual(head,game.Journal.HeadSeq,"no journal head change across the whole round trip");
   Assert.AreEqual(receipts,game.SuccessfulReceipts,"no receipt change across the whole round trip");
   CollectionAssert.AreEqual(bytes,File.ReadAllBytes(Path.Combine(directory,"save.json")),"no save bytes change across the whole round trip");
  }
 }
}
#endif
