#if TIDE_TEST_FRAMEWORK
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Tide.App;
using Tide.Presentation;
using Tide.Sim;
using Tide.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.TestTools;
using UnityEngine.UI;
namespace Tide.Tests {
 // M26 (RFC-CX-M26-20260918): inquiry strip (D2), structural state + counterexample (D3), media strip (D4), receipt (D10).
 public sealed class M26InquiryPlayModeTests {
  string directory;GameObject host;T0GameSession game;Keyboard keyboard;M25ResourceProfile profile;bool originalM26;InputSettings.BackgroundBehavior originalBackground;
#if UNITY_EDITOR
  InputSettings.EditorInputBehaviorInPlayMode originalEditorInput;
#endif
  static readonly string[] Banned={"rec-","H-","H+","당직실 표준판","조위장부","시간창","단일 사고","유지 실패","서명 순서","책임 사슬","문재화","오은정","표성찬"};
  [UnitySetUp] public IEnumerator Setup(){
   directory=Path.Combine(Path.GetTempPath(),"m26-inquiry-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(directory);
#if UNITY_EDITOR
   originalEditorInput=InputSystem.settings.editorInputBehaviorInPlayMode;InputSystem.settings.editorInputBehaviorInPlayMode=InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
#endif
   originalBackground=InputSystem.settings.backgroundBehavior;InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;
   keyboard=InputSystem.AddDevice<Keyboard>();
   profile=Resources.Load<M25ResourceProfile>("M25Resources");if(profile!=null)originalM26=profile.m26Approved;
   host=new GameObject("M26 inquiry test");game=host.AddComponent<T0GameSession>();game.Initialize(Resources.Load<T0RuntimeConfig>("T0Runtime"),directory);
   yield return null;
  }
  IEnumerator Wait(Task task){while(!task.IsCompleted)yield return null;if(task.IsFaulted)throw task.Exception;}
  [UnityTearDown] public IEnumerator TearDown(){
   if(profile!=null)profile.m26Approved=originalM26;
   if(game!=null)yield return Wait(game.FlushSaves());
   if(host!=null)UnityEngine.Object.Destroy(host);
   if(keyboard!=null)InputSystem.RemoveDevice(keyboard);
   InputSystem.settings.backgroundBehavior=originalBackground;
#if UNITY_EDITOR
   InputSystem.settings.editorInputBehaviorInPlayMode=originalEditorInput;
#endif
   yield return null;
   if(directory!=null&&Directory.Exists(directory))Directory.Delete(directory,true);
  }
  IEnumerator Press(Key key){InputSystem.QueueStateEvent(keyboard,new KeyboardState(key));yield return null;InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return null;}
  void Click(string id){Assert.IsTrue(game.Interface.Activate(id),"Missing UI action "+id);}
  Text[] Texts()=>host.GetComponentsInChildren<Text>();
  string Visible()=>string.Join("\n",Texts().Select(t=>t.text));
  // Overlay-owned copy only: the case card / tool panel behind an overlay are not the overlay's words.
  string Content(){var content=host.GetComponentsInChildren<RectTransform>().First(r=>r.name=="Content");return string.Join("\n",content.GetComponentsInChildren<Text>().Select(t=>t.text));}
  Text Inquiry()=>Texts().SingleOrDefault(t=>t.name==T0Interface.InquiryName);
  void Intake(){Click("start");if(game.OpeningActive)Click("intro-skip");Click("handover");foreach(var id in new[]{"hb-l1","hb-l2","hb-l3"})Click("line-"+id);Click("close-document");Click("transfer");foreach(var id in new[]{"tl-r1","tl-r2","tl-r3"})Click("row-"+id);Click("decision-written");Click("close-document");Click("load-plate-zero");}
  void Circuit(){Click("hub-view-circuitmap");Click("open-circuit");Click("trace-hub");Click("begin-overlay");Click("offset-left");Click("offset-up");Click("anchor-overlay");foreach(var area in game.Definition.UncoveredAreas){Click("area-"+area);Click("area-evidence-"+area);Click("attach-rec-watchlog-bureau");Click("overlay-back");}}
  IEnumerator Cite(string record){
   if(game.Journal.State.LoadedRecordId!=record){Click("select-record");Click("choose-"+record);}
   Click("read");
   Assert.IsTrue(game.SubmitImmediate(new PuzzleCommand("SetWindow",value:"H-1:00",otherValue:"H+3:00")).IsValid);
   yield return Wait(game.CommitAsync(new PuzzleCommand("CiteToBoard",record)));
  }
  IEnumerator OpenReader(){Click("hub-view-reader");Click("open-reader");Click("load-rec-plate-standard-hub");yield return null;}

  [UnityTest] public IEnumerator InquiryStripFollowsTheBeatAndNeverDisclosesRecords(){
   Assert.IsNull(Inquiry(),"no inquiry before the watch starts");
   Click("start");if(game.OpeningActive)Click("intro-skip");yield return null;
   var strip=Inquiry();Assert.IsNotNull(strip,"the inquiry strip appears with the case card");
   Assert.IsFalse(strip.raycastTarget);Assert.IsNull(strip.GetComponent<Selectable>());
   StringAssert.Contains("지금 묻는 것",strip.text);StringAssert.Contains("넘겨받았고",strip.text,"t0-b1 asks about the handover");
   foreach(var banned in Banned)StringAssert.DoesNotContain(banned,strip.text,banned+" must not appear in an inquiry");
   var records=JObject.Parse(Resources.Load<T0RuntimeConfig>("T0Runtime").records.text);
   foreach(var record in records["rows"]){StringAssert.DoesNotContain((string)record["recordId"],strip.text);StringAssert.DoesNotContain((string)record["displayNameKo"],strip.text);}
   Assert.AreEqual(Mathf.RoundToInt(TypeScale.Helper*game.Interface.TextScale),strip.fontSize,"the inquiry reads at the Helper tier under the card");
   Assert.AreEqual(2,host.GetComponentsInChildren<RectTransform>().Count(r=>r.name.StartsWith("Inquiry ")&&r.name!="Inquiry strip"),"t0-b1: both required media are still needed");
   Click("handover");foreach(var id in new[]{"hb-l1","hb-l2","hb-l3"})Click("line-"+id);Click("close-document");Click("transfer");foreach(var id in new[]{"tl-r1","tl-r2","tl-r3"})Click("row-"+id);Click("decision-written");Click("close-document");Click("load-plate-zero");yield return null;
   StringAssert.Contains("닿는 범위",Inquiry().text,"t0-b2 asks about reach");
   Circuit();yield return null;StringAssert.Contains("같은 밤",Inquiry().text,"t0-b3 asks whether two media speak of the same night");
   game.OpenOverlay("hints");yield return null;Assert.IsNull(Inquiry(),"overlays hide the strip");game.Back();yield return null;
  }
  [UnityTest] public IEnumerator StructuralStateWalksFourStatesAndCounterexampleIsUndoable(){
   Intake();Circuit();yield return null;
   Assert.AreEqual(T0GameSession.StateUnreviewed,game.StructuralState());
   yield return OpenReader();yield return Cite("rec-plate-standard-hub");
   Assert.AreEqual(T0GameSession.StateOneMedium,game.StructuralState(),"one pinned record = one medium");
   yield return Cite("rec-tide-ledger-bureau");
   Assert.AreEqual(T0GameSession.StateTwoMedia,game.StructuralState(),"plate + ledger from different roots = two media");
   Assert.IsTrue(game.Simulation.IsComplete(game.Journal.State,"t0-b3"));
   game.OpenOverlay("hypothesis");yield return null;
   var text=Content();StringAssert.Contains("주장의 구조 상태",text);StringAssert.Contains("서로 다른 매체 2종",text);StringAssert.Contains("참이라는 판정이 아닙니다",text,"the board carries the caveat");
   Assert.IsTrue(game.Interface.ActionIds.Contains("counter-rec-plate-standard-hub"),"each pinned citation offers a counterexample toggle; actions="+string.Join(",",game.Interface.ActionIds)+" pending="+game.SavePending);
   long head=game.Journal.HeadSeq;
   Click("counter-rec-plate-standard-hub");yield return null;
   Assert.AreEqual(T0GameSession.StateCounterexample,game.StructuralState(),"the player's mark wins");
   Assert.IsTrue(game.Simulation.IsComplete(game.Journal.State,"t0-b3"),"a counterexample mark never changes completion");
   StringAssert.Contains("반례 고정",Content());
   game.Undo();yield return null;
   Assert.AreEqual(head,game.Journal.HeadSeq);Assert.AreEqual(T0GameSession.StateTwoMedia,game.StructuralState(),"undo releases the mark");
   game.Redo();yield return null;Assert.AreEqual(T0GameSession.StateCounterexample,game.StructuralState());
   Assert.AreEqual("reader",game.Surface,"undo/redo close the board like every overlay");game.OpenOverlay("hypothesis");yield return null;
   Click("counter-rec-plate-standard-hub");yield return null;Assert.AreEqual(T0GameSession.StateTwoMedia,game.StructuralState(),"toggling again releases the mark");
  }
  [UnityTest] public IEnumerator SameLineageCopiesStayOneMediumAndInvalidMarksAreRefused(){
   Intake();Circuit();yield return null;
   Assert.IsFalse(game.SubmitImmediate(new PuzzleCommand("MarkCounterexample","rec-plate-standard-hub")).IsValid,"nothing pinned → no counterexample");
   yield return OpenReader();yield return Cite("rec-plate-standard-hub");
   Assert.IsFalse(game.Simulation.Validate(game.Journal.State,new PuzzleCommand("MarkCounterexample","rec-tide-ledger-bureau")).IsValid,"an unpinned record cannot be marked");
   Assert.AreEqual(T0GameSession.StateOneMedium,game.StructuralState());
  }
  [UnityTest] public IEnumerator ReceiptIsOfferedAfterClosingSaveReopensIdempotentlyAndNamesNoOne(){
   Intake();Circuit();yield return null;
   Assert.IsFalse(game.Interface.ActionIds.Contains("t0-receipt"),"no receipt before t0-b3");
   yield return OpenReader();yield return Cite("rec-plate-standard-hub");yield return Cite("rec-tide-ledger-bureau");
   yield return Wait(game.FlushSaves());var bytes=File.ReadAllBytes(Path.Combine(directory,"save.json"));
   game.Back();yield return null;if(game.Surface!="shell"){game.Back();yield return null;}Assert.AreEqual("shell",game.Surface);
   Assert.IsTrue(game.Interface.ActionIds.Contains("t0-receipt"),"the closing save offers the receipt");
   Click("t0-receipt");yield return null;
   Assert.AreEqual(T0GameSession.ReceiptOverlay,game.Surface);
   var text=Content();
   foreach(var heading in new[]{"확정한 것","보류한 것","다음 질문"})StringAssert.Contains(heading,text);
   StringAssert.Contains("인용 H-1:00 → H+3:00",text,"settled lists the pinned windows the player chose");
   StringAssert.Contains("보류한 관계가 없습니다",text);
   foreach(var banned in new[]{"문재화","오은정","표성찬","한도연","서명","봉인"})StringAssert.DoesNotContain(banned,text,banned+" must not be pre-announced by the receipt");
   game.Back();yield return null;Click("t0-receipt");yield return null;
   Assert.AreEqual(text,Content(),"reopening the receipt is idempotent");
   CollectionAssert.AreEqual(bytes,File.ReadAllBytes(Path.Combine(directory,"save.json")),"viewing the receipt writes nothing");
   Assert.IsTrue(game.Interface.ActionIds.Contains("receipt-continue"));
  }
  [UnityTest] public IEnumerator EvidenceBoxListsMediaOfPinsAndGateOnDrawsIcons(){
   if(profile==null||profile.mediaIcons.Length==0)Assert.Ignore("M26 icons not imported");
   Intake();Circuit();yield return OpenReader();yield return Cite("rec-plate-standard-hub");
   profile.m26Approved=false;game.OpenOverlay("evidence");yield return null;
   StringAssert.Contains("고정한 근거의 매체",Content());
   Assert.IsFalse(host.GetComponentsInChildren<RawImage>().Any(r=>r.name=="Picture"&&r.texture!=null),"gate off draws no M26 textures");
   game.Back();yield return null;
   profile.m26Approved=true;game.OpenOverlay("evidence");yield return null;
   var pictures=host.GetComponentsInChildren<RawImage>().Where(r=>r.name=="Picture"&&r.texture!=null).ToArray();
   Assert.GreaterOrEqual(pictures.Length,1,"gate on draws the plate silhouette");
   foreach(var p in pictures)Assert.IsFalse(p.raycastTarget);
   game.Back();yield return null;Assert.AreEqual("reader",game.Surface);
   var strip=host.GetComponentsInChildren<RawImage>().FirstOrDefault(r=>r.name=="M26 question card");
   Assert.IsNotNull(strip,"gate on backs the inquiry strip with the question card");Assert.IsFalse(strip.raycastTarget);
  }
  // M26 U1 (improvement plan): R is a keyboard route to the receipt, gated exactly like the plate. Before the closing save
  // of t0-b3 it only leaves a status line (no overlay, no record names); after it, R opens the receipt and Esc closes it.
  [UnityTest] public IEnumerator RKeyOpensReceiptOnlyAfterT0Completion(){
   Intake();yield return null;var surface=game.Surface;
   yield return Press(Key.R);
   Assert.AreEqual(surface,game.Surface,"R before completion must not open anything");
   var texts=Visible();StringAssert.Contains("영수증",texts,"R before completion answers with the receipt status line");
   foreach(var banned in new[]{"문재화","오은정","표성찬"})StringAssert.DoesNotContain(banned,texts);
   Circuit();yield return OpenReader();yield return Cite("rec-plate-standard-hub");yield return Cite("rec-tide-ledger-bureau");
   yield return Wait(game.FlushSaves());yield return null;
   Assert.AreEqual("reader",game.Surface,"completion happens in the reader; R must work from here without walking back to the shell plate");
   var bytes=File.ReadAllBytes(Path.Combine(directory,"save.json"));
   yield return Press(Key.R);
   Assert.AreEqual(T0GameSession.ReceiptOverlay,game.Surface,"R after completion opens the receipt from the reader");
   StringAssert.Contains("당직 영수증",Visible(),"the receipt title is on screen");
   yield return Press(Key.Escape);yield return null;
   Assert.AreNotEqual(T0GameSession.ReceiptOverlay,game.Surface,"Esc closes the receipt");
   CollectionAssert.AreEqual(bytes,File.ReadAllBytes(Path.Combine(directory,"save.json")),"opening the receipt by key writes nothing");
  }
 }
}
#endif
