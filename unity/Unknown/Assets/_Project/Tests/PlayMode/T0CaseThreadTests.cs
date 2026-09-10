#if TIDE_TEST_FRAMEWORK
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Tide.App;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tide.Tests {
 public sealed class T0CaseThreadTests {
  T0GameSession game;
  GameObject host;
  Keyboard keyboard;
  Mouse mouse;
  string directory;
  bool checkDisclosure;
  InputSettings.BackgroundBehavior originalBackground;
#if UNITY_EDITOR
  InputSettings.EditorInputBehaviorInPlayMode originalEditorInput;
#endif

  [UnitySetUp] public IEnumerator SetUp() {
   checkDisclosure=false;
#if UNITY_EDITOR
   originalEditorInput=InputSystem.settings.editorInputBehaviorInPlayMode;
   InputSystem.settings.editorInputBehaviorInPlayMode=InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
#endif
   originalBackground=InputSystem.settings.backgroundBehavior;
   InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;
   keyboard=InputSystem.AddDevice<Keyboard>();
   mouse=InputSystem.AddDevice<Mouse>();
   directory=Path.Combine(Path.GetTempPath(),"t0-case-thread-"+Guid.NewGuid().ToString("N"));
   host=new GameObject("T0 case thread test");
   game=host.AddComponent<T0GameSession>();
   game.Initialize(Resources.Load<T0RuntimeConfig>("T0Runtime"),directory);
   yield return null;
  }

  [UnityTearDown] public IEnumerator TearDown() {
   if(game!=null){game.Store.Injection=null;yield return Wait(game.FlushSaves());}
   if(host!=null)UnityEngine.Object.Destroy(host);
   if(keyboard!=null)InputSystem.RemoveDevice(keyboard);
   if(mouse!=null)InputSystem.RemoveDevice(mouse);
   InputSystem.settings.backgroundBehavior=originalBackground;
#if UNITY_EDITOR
   InputSystem.settings.editorInputBehaviorInPlayMode=originalEditorInput;
#endif
   yield return null;
   if(directory!=null&&Directory.Exists(directory))Directory.Delete(directory,true);
  }

  [UnityTest] public IEnumerator CaseThreadAfterCircuitNeverNamesTheSolutionSource() {
   Click("start");Click("handover");
   foreach(var id in new[]{"hb-l1","hb-l2","hb-l3"})Click("line-"+id);
   Click("close-document");Click("transfer");
   foreach(var id in new[]{"tl-r1","tl-r2","tl-r3"})Click("row-"+id);
   Click("decision-written");Click("close-document");Click("load-plate-zero");
   Click("hub-view-circuitmap");Click("open-circuit");Click("trace-hub");
   Click("begin-overlay");Click("offset-left");Click("offset-up");Click("anchor-overlay");
   foreach(var area in game.Definition.UncoveredAreas){
    Click("area-"+area);Click("area-evidence-"+area);Click("attach-rec-watchlog-bureau");Click("overlay-back");
   }
   Assert.IsTrue(game.Simulation.IsComplete(game.Journal.State,"t0-b2"),"Reach the live post-circuit state before checking disclosure");
   Click("hub-view-reader");Click("open-reader");
   yield return Wait(game.FlushSaves());
   AssertCard(0,false);
   var card=host.GetComponentsInChildren<Text>().Single(t=>t.name=="CaseThread").text;
   var next=card.Substring(card.LastIndexOf("다음: ",StringComparison.Ordinal)+4);
   Assert.AreEqual("근거를 살펴보고 인용을 고정하세요.",next,
    "Post-t0-b2 next action must give generic objective guidance, never a record/action label");
  }
  [UnityTest] public IEnumerator CaseThreadFollowsLiveCitationsWithoutMutatingPuzzleOrNavigation() {
   checkDisclosure=true;
   AssertNoDisclosure();
   yield return KeyboardClick("start");
   AssertCard(0,false); // The pre-implementation RED must fail on the missing visible card.
   AssertNext("기록을 읽고 근거를 살펴보세요.");
   yield return AssertRenderOnly(0,false);

   // Existing intake/circuit actions establish prerequisites; no fabricated state or journal writes.
   Click("settings");for(int i=0;i<5;i++)Click("text-scale");Click("overlay-back");
   Click("handover");
   AssertCard(0,false); // The document guidance fits at the existing maximum text scale.
   Click("settings");Click("text-scale");Click("overlay-back");
   foreach(var id in new[]{"hb-l1","hb-l2","hb-l3"})Click("line-"+id);
   Click("close-document");Click("transfer");
   foreach(var id in new[]{"tl-r1","tl-r2","tl-r3"})Click("row-"+id);
   Click("decision-written");Click("close-document");Click("load-plate-zero");
   Click("hub-view-circuitmap");Click("open-circuit");AssertNext("근거를 대조하고 정렬해 보세요.");Click("trace-hub");
   Click("begin-overlay");Click("offset-left");Click("offset-up");Click("anchor-overlay");
   foreach(var area in game.Definition.UncoveredAreas){
    Click("area-"+area);Click("area-evidence-"+area);Click("attach-rec-watchlog-bureau");Click("overlay-back");
   }
   Assert.IsTrue(game.Simulation.IsComplete(game.Journal.State,"t0-b2"));
   yield return KeyboardClick("hub-view-reader");
   yield return PointerClick("open-reader");
   AssertCard(0,false); // Post-t0-b2, before any source is selected.
   yield return KeyboardClick("load-rec-plate-standard-hub");
   yield return PointerClick("read");
   AssertCard(0,false);
   AssertNext("근거를 살펴보고 인용을 고정하세요.");
   Assert.IsFalse(game.Journal.State.Has("citation:rec-plate-standard-hub"));
   yield return AssertRenderOnly(0,false);
   yield return Window("H-1:00","H+2:00");
   yield return KeyboardClick("cite");
   Assert.AreEqual("confirm",game.Surface);
   yield return AssertRenderOnly(0,false);
   var saveGate=new TaskCompletionSource<bool>();
   game.Store.Injection=(stage,token)=>saveGate.Task;
   try {
    yield return KeyboardClick("confirm-submit");
    Assert.IsTrue(game.SavePending);
    AssertNoDisclosure(); // The transient saving phase is exercised deterministically.
   }finally{game.Store.Injection=null;saveGate.TrySetResult(true);}
   while(game.SavePending)yield return null;
   yield return Wait(game.FlushSaves());
   Assert.IsTrue(game.Journal.State.Has("citation:rec-plate-standard-hub"));
   AssertCard(1,false);
   Assert.AreEqual(1,game.SuccessfulReceipts);
   yield return AssertRenderOnly(1,false);
   game.Undo();yield return Wait(game.FlushSaves());AssertCard(0,false);
   game.Redo();yield return Wait(game.FlushSaves());AssertCard(1,false);

   yield return KeyboardClick("select-record");
   yield return KeyboardClick("choose-rec-tide-ledger-bureau");
   yield return PointerClick("read");
   AssertCard(1,false); // Loading/reading another medium is not a citation.
   yield return Window("H-1:00","H+3:00");
   yield return KeyboardClick("cite");yield return KeyboardClick("confirm-submit");
   while(game.SavePending)yield return null;
   yield return Wait(game.FlushSaves());
   Assert.IsTrue(game.Journal.State.Has("citation:rec-tide-ledger-bureau"));
   AssertCard(2,false); // Two required pins alone do not satisfy the exact endpoint predicate.
   Assert.AreEqual("H+2:00",game.Journal.State.Get("citationEnd:rec-plate-standard-hub"));
   yield return KeyboardClick("select-record");
   yield return KeyboardClick("choose-rec-plate-standard-hub");
   yield return Window("H-1:00","H+3:00");
   Assert.AreEqual("H+3:00",game.Journal.State.Get("windowEnd:rec-plate-standard-hub"));
   Assert.AreEqual("H+2:00",game.Journal.State.Get("citationEnd:rec-plate-standard-hub"));
   yield return AssertRenderOnly(2,false); // Preview changes must never silently re-pin a citation.
   yield return KeyboardClick("cite");yield return KeyboardClick("confirm-submit");
   while(game.SavePending)yield return null;
   yield return Wait(game.FlushSaves());
   AssertCard(2,true);
   Assert.AreEqual(3,game.SuccessfulReceipts);
   yield return AssertRenderOnly(2,true);

   game.Undo();yield return Wait(game.FlushSaves());AssertCard(2,false);
   game.Redo();yield return Wait(game.FlushSaves());AssertCard(2,true);
   var state=game.Journal.State.StateHash;
   yield return PressKey(Key.I);
   Assert.AreEqual("evidence",game.Surface);
   yield return AssertRenderOnly(2,true);
   yield return PressKey(Key.Escape);
   Assert.AreEqual("reader",game.Surface);
   Assert.AreEqual(state,game.Journal.State.StateHash,"Overlay navigation must not alter the puzzle");
   AssertCard(2,true);
   Click("settings");
   for(int i=0;i<5;i++)Click("text-scale");
   AssertCard(2,true); // Existing maximum text scale must still contain the passive card.
   Click("text-scale");Click("overlay-back");
  }

  void AssertNext(string objective) {
   StringAssert.EndsWith("다음: "+objective,host.GetComponentsInChildren<Text>().Single(t=>t.name=="CaseThread").text);
  }
  void AssertNoDisclosure() {
   var card=host.GetComponentsInChildren<Text>().Single(t=>t.name=="CaseThread").text;
   var next=card.Split('\n').Last();
   CollectionAssert.Contains(new[]{
    "다음: 기록을 읽고 근거를 살펴보세요.","다음: 근거를 대조하고 정렬해 보세요.",
    "다음: 근거를 살펴보고 인용을 고정하세요.","다음: 고정한 근거를 검토하세요.","다음: 저장 완료 기다리기"
   },next,"Every card phase must use generic objective guidance only");
   var records=JObject.Parse(Resources.Load<T0RuntimeConfig>("T0Runtime").records.text);
   foreach(var record in records["rows"]){
    StringAssert.DoesNotContain((string)record["recordId"],card);
    StringAssert.DoesNotContain((string)record["displayNameKo"],card);
   }
   foreach(var button in host.GetComponentsInChildren<Button>()){
    Assert.AreNotEqual("다음: "+button.GetComponentsInChildren<Text>().Single(t=>t.name=="Label").text,next,
     "The card must not copy a live action label");
   }
   foreach(var banned in new[]{"rec-","H-","H+","당직실 표준판","조위장부","시간창","단일 사고","유지 실패","서명 순서","책임 사슬"})
    StringAssert.DoesNotContain(banned,card);
  }

  void AssertCard(int count,bool completed) {
   var cards=host.GetComponentsInChildren<Text>().Where(t=>t.name=="CaseThread").ToArray();
   Assert.AreEqual(1,cards.Length,"A visible Korean CaseThread must explain the existing T0 objective and citation progress");
   var card=cards[0];
   if(checkDisclosure)AssertNoDisclosure();
   Assert.IsTrue(card.enabled&&card.gameObject.activeInHierarchy);
   StringAssert.Contains("결손 4시간의 양 끝을 두 기록으로 고정",card.text);
   StringAssert.Contains("필요한 인용 "+count+"/2",card.text);
   StringAssert.Contains("다음:",card.text);
   StringAssert.DoesNotContain("H-1",card.text);
   StringAssert.DoesNotContain("H+3",card.text);
   Assert.AreEqual(completed,card.text.Contains("근거 쌍이 고정됨"),"Completion copy must reflect the full t0-b3 predicate");
   Assert.AreEqual(completed,game.Simulation.IsComplete(game.Journal.State,"t0-b3"));
   Assert.IsFalse(card.raycastTarget,"Read-only objective text must not intercept pointer input");
   Canvas.ForceUpdateCanvases();
   Assert.LessOrEqual(card.preferredHeight,card.rectTransform.rect.height+1f,"The visible case thread must not truncate its text");
   Assert.IsFalse(card.transform.parent.GetComponent<Image>().raycastTarget,"The card panel must not intercept pointer input");
   Assert.IsNull(card.GetComponent<Selectable>(),"The case thread must not create a navigation action");
   Assert.IsFalse(game.Interface.ActionIds.Contains("CaseThread"));
   foreach(var banned in new[]{"단일 사고","유지 실패","서명 순서","책임 사슬"})StringAssert.DoesNotContain(banned,card.text);
  }

  IEnumerator AssertRenderOnly(int count,bool complete) {
   yield return Wait(game.FlushSaves());
   var state=game.Journal.State.StateHash;
   var head=game.Journal.HeadSeq;
   var entries=game.Journal.Entries.ToArray();
   var loaded=game.Journal.State.LoadedRecordId;
   var focus=game.Interface.CurrentFocusId;
   var actions=game.Interface.ActionIds.ToArray();
   var receipts=game.SuccessfulReceipts;
   var files=FileHashes();
   var writes=0;
   game.Store.Injection=(stage,token)=>{Interlocked.Increment(ref writes);return Task.CompletedTask;};
   try {
    for(int i=0;i<3;i++){game.Render();yield return null;AssertCard(count,complete);}
    yield return Wait(game.FlushSaves());
    Assert.AreEqual(state,game.Journal.State.StateHash,"Rendering must not change any puzzle facts, copies, windows or pins");
    Assert.AreEqual(head,game.Journal.HeadSeq);
    CollectionAssert.AreEqual(entries,game.Journal.Entries,"Rendering must not append commands");
    Assert.AreEqual(loaded,game.Journal.State.LoadedRecordId,"Rendering must not load/select a record");
    Assert.AreEqual(focus,game.Interface.CurrentFocusId,"Rendering must preserve existing focus");
    CollectionAssert.AreEqual(actions,game.Interface.ActionIds);
    Assert.AreEqual(receipts,game.SuccessfulReceipts);
    Assert.AreEqual(0,writes,"Rendering must not schedule a save");
    CollectionAssert.AreEqual(files,FileHashes(),"Rendering must not write save or settings files");
   }finally{game.Store.Injection=null;}
  }

  string[] FileHashes() {
   using(var sha=SHA256.Create())return Directory.Exists(directory)
    ?Directory.GetFiles(directory,"*",SearchOption.AllDirectories).OrderBy(p=>p,StringComparer.Ordinal)
     .Select(p=>p+":"+Convert.ToBase64String(sha.ComputeHash(File.ReadAllBytes(p)))).ToArray()
    :Array.Empty<string>();
  }
  void Click(string id){Assert.IsTrue(game.Interface.Activate(id),"Missing existing action "+id);if(checkDisclosure)AssertNoDisclosure();}
  IEnumerator Wait(Task task){while(!task.IsCompleted)yield return null;if(task.IsFaulted)throw task.Exception;}
  IEnumerator PressKey(Key key){
   InputSystem.QueueStateEvent(keyboard,new KeyboardState(key));yield return null;
   InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return null;
  }
  IEnumerator KeyboardClick(string id){
   int steps=0;
   while(game.Interface.CurrentFocusId!=id){Assert.Less(steps++,100,"Keyboard cannot reach "+id);yield return PressKey(Key.Tab);}
   yield return PressKey(Key.Enter);
   if(checkDisclosure)AssertNoDisclosure();
  }
  IEnumerator PointerClick(string id){
   Assert.IsTrue(game.Interface.Focus(id),"Missing pointer target "+id);
   yield return null;Canvas.ForceUpdateCanvases();
   var button=host.GetComponentsInChildren<Button>().Single(b=>b.name==id);
   var rect=(RectTransform)button.transform;
   var point=RectTransformUtility.WorldToScreenPoint(null,rect.TransformPoint(rect.rect.center));
   InputSystem.QueueStateEvent(mouse,new MouseState{position=point});yield return null;
   if(checkDisclosure)AssertNoDisclosure();
   InputSystem.QueueStateEvent(mouse,new MouseState{position=point}.WithButton(MouseButton.Left));yield return null;
   InputSystem.QueueStateEvent(mouse,new MouseState{position=point});yield return null;
  }
  IEnumerator Window(string start,string end){
   foreach(var endpoint in new[]{(pick:"pick-start",phase:start),(pick:"pick-end",phase:end)}){
    yield return KeyboardClick(endpoint.pick);
    AssertCard(game.Journal.State.Has("citation:rec-tide-ledger-bureau")?2:game.Journal.State.Has("citation:rec-plate-standard-hub")?1:0,false);
    var phases=game.Definition.Records[game.Journal.State.LoadedRecordId].Phases.ToList();
    int target=phases.IndexOf(endpoint.phase),pages=0;
    Assert.GreaterOrEqual(target,0,"Sample must exist in the production grid");
    while(!game.Interface.ActionIds.Contains("phase-"+endpoint.phase)){
     Assert.Less(pages++,30,"Phase picker cannot reach authored sample");
     int first=game.Interface.ActionIds.Where(a=>a.StartsWith("phase-")&&a!="phase-prev"&&a!="phase-next")
      .Select(a=>phases.IndexOf(a.Substring(6))).Min();
     Click(target<first?"phase-prev":"phase-next");
    }
    yield return KeyboardClick("phase-"+endpoint.phase);
   }
  }
 }
}
#endif
