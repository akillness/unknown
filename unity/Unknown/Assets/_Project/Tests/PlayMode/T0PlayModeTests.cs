#if TIDE_TEST_FRAMEWORK
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Tide.App;
using Tide.Save;
using Tide.Sim;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.TestTools;
namespace Tide.Tests {
 public sealed class T0PlayModeTests {
  #if UNITY_EDITOR
  InputSettings.EditorInputBehaviorInPlayMode originalEditorInput;
  #endif
  InputSettings.BackgroundBehavior originalBackground;T0GameSession game;GameObject host;Keyboard keyboard;Gamepad pad;string directory;
  T0RuntimeConfig hintConfig;TextAsset hintTools;
  [UnitySetUp] public IEnumerator SetUp(){
#if UNITY_EDITOR
originalEditorInput=InputSystem.settings.editorInputBehaviorInPlayMode;InputSystem.settings.editorInputBehaviorInPlayMode=InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
#endif
originalBackground=InputSystem.settings.backgroundBehavior;InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;directory=Path.Combine(Path.GetTempPath(),"t0-play-"+Guid.NewGuid().ToString("N"));keyboard=InputSystem.AddDevice<Keyboard>();pad=InputSystem.AddDevice<Gamepad>();host=new GameObject("T0 Play test");game=host.AddComponent<T0GameSession>();game.Initialize(Resources.Load<T0RuntimeConfig>("T0Runtime"),directory);yield return null;}
  [UnityTearDown] public IEnumerator TearDown(){var task=game.FlushSaves();while(!task.IsCompleted)yield return null;UnityEngine.Object.Destroy(host);InputSystem.RemoveDevice(keyboard);InputSystem.RemoveDevice(pad);InputSystem.settings.backgroundBehavior=originalBackground;
if(hintConfig!=null)UnityEngine.Object.Destroy(hintConfig);if(hintTools!=null)UnityEngine.Object.Destroy(hintTools);
#if UNITY_EDITOR
InputSystem.settings.editorInputBehaviorInPlayMode=originalEditorInput;
#endif
yield return null;if(Directory.Exists(directory))Directory.Delete(directory,true);}
  IEnumerator PressKey(Key key){InputSystem.QueueStateEvent(keyboard,new KeyboardState(key));yield return null;InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return null;}
  IEnumerator Pad(GamepadButton key){InputSystem.QueueStateEvent(pad,new GamepadState().WithButton(key));yield return null;InputSystem.QueueStateEvent(pad,new GamepadState());yield return null;}
  void Click(string id){Assert.IsTrue(game.Interface.Activate(id),"Missing UI action "+id);if(id=="start"&&game.OpeningActive)Assert.IsTrue(game.Interface.Activate("intro-skip"));}
  void Intake(){Click("start");Click("handover");foreach(var id in new[]{"hb-l1","hb-l2","hb-l3"})Click("line-"+id);Click("close-document");Click("transfer");foreach(var id in new[]{"tl-r1","tl-r2","tl-r3"})Click("row-"+id);Click("decision-written");Click("close-document");Click("load-plate-zero");}
  void Circuit(){Click("hub-view-circuitmap");Click("open-circuit");Click("trace-hub");Click("begin-overlay");Click("offset-left");Click("offset-up");Click("anchor-overlay");foreach(var area in game.Definition.UncoveredAreas){Click("area-"+area);Click("area-evidence-"+area);Click("attach-rec-watchlog-bureau");Click("overlay-back");}}
  IEnumerator Wait(Task task){while(!task.IsCompleted)yield return null;if(task.IsFaulted)throw task.Exception;}
  IEnumerator ReaderReady(){Intake();Circuit();Click("hub-view-reader");Click("open-reader");Click("load-rec-plate-standard-hub");Click("read");yield return Wait(game.FlushSaves());}
  [UnityTest] public IEnumerator KeyboardStartsAndHeldConfirmCannotCrossScreen(){Assert.AreEqual("shell",game.Surface);InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.Enter));yield return null;yield return null;Assert.IsTrue(game.Interface.ActionIds.Contains("start"));InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return null;if(game.OpeningActive)yield return InputClick("intro-skip",false);Assert.IsTrue(game.Interface.ActionIds.Contains("handover"));InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return null;Assert.IsTrue(game.Interface.ActionIds.Contains("handover"));}
  [UnityTest] public IEnumerator GamepadStartsAndShellQueryHasNoAction(){yield return Pad(GamepadButton.South);if(game.OpeningActive)yield return InputClick("intro-skip",true);Assert.IsTrue(game.Watch.LastDeviceIsGamepad);Assert.IsTrue(game.Interface.ActionIds.Contains("handover"));yield return PressKey(UnityEngine.InputSystem.Key.Q);Assert.IsFalse(game.Watch.LastDeviceIsGamepad);Assert.AreEqual("shell",game.Surface);yield return Pad(GamepadButton.RightStick);Assert.AreEqual("shell",game.Surface);yield return PressKey(UnityEngine.InputSystem.Key.I);Assert.AreEqual("evidence",game.Surface);yield return PressKey(UnityEngine.InputSystem.Key.F1);Assert.AreEqual("hints",game.Surface);}
  [UnityTest] public IEnumerator UiActionAdapterCompletesCanonicalT0AndReopensSavedState(){yield return ReaderReady();foreach(var id in new[]{"rec-plate-standard-hub","rec-tide-ledger-bureau"}){if(game.Journal.State.LoadedRecordId!=id){Click("select-record");Click("choose-"+id);Click("read");}var verdict=game.SubmitImmediate(new PuzzleCommand("SetWindow",value:"H-1:00",otherValue:"H+3:00"));Assert.IsTrue(verdict.IsValid);Click("cite");Assert.AreEqual("preview",game.Surface);Click("preview-next");Click("confirm-submit");while(game.SavePending)yield return null;}Assert.IsTrue(game.Simulation.IsComplete(game.Journal.State,"t0-b3"));Assert.AreEqual(2,game.SuccessfulReceipts);yield return Wait(game.FlushSaves());var loaded=game.Store.Load(validate:d=>JournalSave.Decode(d,game.Simulation));var restored=JournalSave.Decode(loaded.Document,game.Simulation);Assert.AreEqual(game.Journal.State.StateHash,restored.State.StateHash);}
  [UnityTest] public IEnumerator ThreeConfirmationModesRequireTheirOwnGesture(){yield return ReaderReady();game.SetConfirmMode("hold");Click("read-original");Assert.IsTrue(game.Interface.Focus("confirm-submit"));game.Interface.BeginActivation();yield return new WaitForSecondsRealtime(.1f);game.Interface.EndActivation();Assert.IsFalse(game.SavePending);Assert.AreEqual(0,game.Journal.State.ReadCount("rec-plate-standard-hub"));game.Interface.BeginActivation();yield return new WaitForSecondsRealtime(.45f);game.Interface.EndActivation();while(game.SavePending)yield return null;Assert.AreEqual(1,game.Journal.State.ReadCount("rec-plate-standard-hub"));game.SetConfirmMode("two-step");Click("read-original");Assert.IsTrue(game.Interface.ActionIds.Contains("preview-next"));Assert.IsFalse(game.Interface.ActionIds.Contains("confirm-submit"));Click("preview-next");Click("confirm-submit");while(game.SavePending)yield return null;Assert.AreEqual(2,game.Journal.State.ReadCount("rec-plate-standard-hub"));game.SetConfirmMode("confirm-dialog");Click("read-original");Click("confirm-submit");while(game.SavePending)yield return null;Assert.AreEqual(3,game.Journal.State.ReadCount("rec-plate-standard-hub"));}
  [UnityTest] public IEnumerator PendingAllowsOverlayUndoAndSuppressesStaleReceipt(){yield return ReaderReady();var release=new TaskCompletionSource<bool>();game.Store.Injection=(stage,token)=>stage==SaveStage.BeforeRename?release.Task:Task.CompletedTask;var pending=game.CommitAsync(new PuzzleCommand("ReadOriginal"));yield return null;Assert.IsTrue(game.SavePending);Assert.IsFalse(awaitResult(game.CommitAsync(new PuzzleCommand("ReadOriginal"))));yield return PressKey(UnityEngine.InputSystem.Key.F1);Assert.AreEqual("hints",game.Surface);game.Undo();Assert.IsFalse(game.SavePending);game.Store.Injection=null;release.SetResult(true);yield return Wait(pending);yield return Wait(game.FlushSaves());Assert.AreEqual(0,game.SuccessfulReceipts);Assert.AreEqual(0,game.Journal.State.ReadCount("rec-plate-standard-hub"));}
  [UnityTest] public IEnumerator CanonicalToolRoutingAndRebindingRetainIntent(){yield return ReaderReady();yield return PressKey(UnityEngine.InputSystem.Key.Q);Assert.AreEqual("reader",game.Surface);int before=game.Journal.State.ReadCount("rec-plate-standard-hub");yield return Pad(GamepadButton.West);Assert.AreEqual(before,game.Journal.State.ReadCount("rec-plate-standard-hub"));Assert.AreEqual("reader",game.Surface);game.SubmitImmediate(new PuzzleCommand("SetWindow",value:"H-1:00",otherValue:"H+3:00"));yield return Pad(GamepadButton.North);Assert.AreEqual("preview",game.Surface);Click("preview-next");Assert.AreEqual("confirm",game.Surface);game.Back();game.Back();yield return Pad(GamepadButton.North);Assert.AreEqual("toolWheel",game.Surface);game.Back();var action=game.Watch.Actions.FindAction("Overlay");var index=action.bindings.Select((b,i)=>new{b,i}).First(x=>x.b.name=="hypothesis").i;action.ApplyBindingOverride(index,"<Keyboard>/j");yield return PressKey(UnityEngine.InputSystem.Key.J);Assert.AreEqual("hypothesis",game.Surface);}
  IEnumerator InputClick(string id,bool controller){int moves=0;while(game.Interface.CurrentFocusId!=id){Assert.Less(moves++,100,"Unreachable focus "+id);if(controller){InputSystem.QueueStateEvent(pad,new GamepadState{leftStick=new Vector2(0,-1)});yield return null;InputSystem.QueueStateEvent(pad,new GamepadState());yield return null;}else yield return PressKey(UnityEngine.InputSystem.Key.Tab);}if(controller)yield return Pad(GamepadButton.South);else yield return PressKey(UnityEngine.InputSystem.Key.Enter);if(id=="start"&&game.OpeningActive)yield return InputClick("intro-skip",controller);}
  IEnumerator FullInputRun(bool controller){
   yield return InputClick("start",controller);yield return InputClick("handover",controller);foreach(var id in new[]{"hb-l1","hb-l2","hb-l3"})yield return InputClick("line-"+id,controller);yield return InputClick("close-document",controller);yield return InputClick("transfer",controller);foreach(var id in new[]{"tl-r1","tl-r2","tl-r3"})yield return InputClick("row-"+id,controller);yield return InputClick("decision-blank",controller);yield return InputClick("close-document",controller);yield return InputClick("load-plate-zero",controller);
   yield return InputClick("hub-view-circuitmap",controller);yield return InputClick("open-circuit",controller);yield return InputClick("begin-overlay",controller);yield return InputClick("offset-left",controller);yield return InputClick("offset-up",controller);yield return InputClick("anchor-overlay",controller);
   foreach(var area in game.Definition.UncoveredAreas){yield return InputClick("area-"+area,controller);yield return InputClick("area-evidence-"+area,controller);yield return InputClick("attach-rec-watchlog-bureau",controller);yield return InputClick("overlay-back",controller);}
   yield return InputClick("hub-view-reader",controller);yield return InputClick("open-reader",controller);yield return InputClick("load-rec-plate-standard-hub",controller);
   foreach(var id in new[]{"rec-plate-standard-hub","rec-tide-ledger-bureau"}){if(game.Journal.State.LoadedRecordId!=id){yield return InputClick("select-record",controller);yield return InputClick("choose-"+id,controller);}yield return InputClick("read",controller);yield return InputClick("pick-start",controller);int pages=0;while(!game.Interface.ActionIds.Contains("phase-H-1:00")){Assert.Less(pages++,30);yield return InputClick("phase-next",controller);}yield return InputClick("phase-H-1:00",controller);yield return InputClick("pick-end",controller);pages=0;while(!game.Interface.ActionIds.Contains("phase-H+3:00")){Assert.Less(pages++,30);yield return InputClick("phase-prev",controller);}yield return InputClick("phase-H+3:00",controller);yield return InputClick("cite",controller);Assert.AreEqual("preview",game.Surface);yield return InputClick("preview-next",controller);yield return InputClick("confirm-submit",controller);while(game.SavePending)yield return null;}
   Assert.IsTrue(game.Simulation.IsComplete(game.Journal.State,"t0-b3"));Assert.AreEqual(2,game.SuccessfulReceipts);
  }
  [UnityTest] public IEnumerator KeyboardOnlyCompletesAllThreeBeats(){yield return FullInputRun(false);}
  [UnityTest] public IEnumerator GamepadOnlyCompletesAllThreeBeats(){yield return FullInputRun(true);}
  IEnumerator Restart(){yield return Wait(game.FlushSaves());UnityEngine.Object.Destroy(host);yield return null;host=new GameObject("Restarted T0");game=host.AddComponent<T0GameSession>();game.Initialize(Resources.Load<T0RuntimeConfig>("T0Runtime"),directory);yield return null;}
  [UnityTest] public IEnumerator RecoveryNewSlotIsSelectableAfterSessionRestart(){var future=JournalSave.Encode(game.Journal,"future",20000,6291456);future["schemaVersion"]=4;yield return Wait(game.Store.WriteAsync(future));var original=File.ReadAllBytes(Path.Combine(directory,"save.json"));yield return Restart();Assert.AreEqual("recovery",game.Surface);Click("recovery-new-slot");Click("start");Click("handover");Click("line-hb-l1");yield return Wait(game.FlushSaves());Assert.AreNotEqual(directory,game.Store.DirectoryPath);yield return Restart();Assert.AreEqual("recovery",game.Surface);Click("select-slot-1");Click("start");Assert.AreNotEqual("recovery",game.Surface);Assert.IsTrue(game.Journal.State.Has("line:rec-handover-brief:hb-l1"));CollectionAssert.AreEqual(original,File.ReadAllBytes(Path.Combine(directory,"save.json")));}
  [UnityTest] public IEnumerator ValidatedRetryClearsRefusalAndPreservesSaveIdentity(){var valid=JournalSave.Encode(game.Journal,"valid",20000,6291456);yield return Wait(game.Store.WriteAsync(valid));var future=(Newtonsoft.Json.Linq.JObject)valid.DeepClone();future["schemaVersion"]=4;future["commitIdempotencyKey"]="future";yield return Wait(game.Store.WriteAsync(future));yield return Restart();Assert.AreEqual("recovery",game.Surface);File.Copy(Path.Combine(directory,"save.bak"),Path.Combine(directory,"save.json"),true);Click("recovery-retry");Click("start");Assert.AreNotEqual("recovery",game.Surface);var commit=game.CommitAsync(new PuzzleCommand("ViewLine","rec-handover-brief","hb-l1"));yield return Wait(commit);Assert.IsTrue(commit.Result);var loaded=game.Store.Load();Assert.AreEqual((string)valid["saveId"],(string)loaded.Document["saveId"]);Assert.AreEqual((string)valid["createdUtc"],(string)loaded.Document["createdUtc"]);}
  static bool awaitResult(Task<bool> task){Assert.IsTrue(task.IsCompleted);return task.Result;}
  [UnityTest] public IEnumerator ReaderWaveformUsesViewportClipAndCullsOutsideScrollArea(){
   yield return ReaderReady();yield return null;yield return null;var chart=host.GetComponentInChildren<Tide.UI.ReaderComparisonChart>();Assert.IsNotNull(chart);Canvas.ForceUpdateCanvases();Assert.IsTrue(chart.canvasRenderer.hasRectClipping,"Waveform must participate in RectMask2D clipping");
   var scroll=chart.GetComponentInParent<UnityEngine.UI.ScrollRect>();scroll.verticalNormalizedPosition=0;Canvas.ForceUpdateCanvases();yield return null;yield return null;
   var chartCorners=new Vector3[4];var viewCorners=new Vector3[4];chart.rectTransform.GetWorldCorners(chartCorners);scroll.viewport.GetWorldCorners(viewCorners);Assert.Greater(chartCorners[2].y,viewCorners[2].y,"The scrolled waveform extends above the viewport");Assert.IsTrue(chart.canvasRenderer.hasRectClipping,"Partially outside waveform still needs the viewport clip");
   scroll.movementType=UnityEngine.UI.ScrollRect.MovementType.Unrestricted;scroll.inertia=false;var localDelta=scroll.content.parent.InverseTransformVector(new Vector3(0,viewCorners[2].y-chartCorners[0].y+20,0));scroll.content.anchoredPosition+=new Vector2(0,localDelta.y);Canvas.ForceUpdateCanvases();yield return null;yield return null;chart.rectTransform.GetWorldCorners(chartCorners);Assert.Greater(chartCorners[0].y,viewCorners[2].y);Assert.IsTrue(chart.canvasRenderer.cull,"Waveform wholly outside the work area must be culled");
  }
  [UnityTest] public IEnumerator DrawerCameraRendersInsideUnobscuredSceneWindow(){
   var previous=Camera.main;bool wasEnabled=previous!=null&&previous.enabled;if(previous!=null)previous.enabled=false;
   var cameraHost=new GameObject("Drawer viewport test camera",typeof(Camera));cameraHost.tag="MainCamera";var camera=cameraHost.GetComponent<Camera>();
   try {Click("start");game.GoNode("hub-view-drawer");yield return null;yield return null;
    Assert.AreSame(camera,Camera.main);var window=game.Interface.SceneViewport;Assert.AreEqual(window,camera.rect);
    var center=camera.WorldToScreenPoint(new Vector3(0,.45f,1.1f));Assert.Greater(center.z,0);Assert.IsTrue(window.Contains(new Vector2(center.x/Screen.width,center.y/Screen.height)),"Drawer center must be inside the unobscured scene window");
    var work=host.GetComponentsInChildren<RectTransform>().First(x=>x.name=="Work Surface");var nav=host.GetComponentsInChildren<RectTransform>().First(x=>x.name=="Navigation");var corners=new Vector3[4];work.GetWorldCorners(corners);Assert.Less(window.xMax,corners[0].x/Screen.width);nav.GetWorldCorners(corners);Assert.Greater(window.yMin,corners[2].y/Screen.height);
    var data=Newtonsoft.Json.Linq.JObject.Parse(Resources.Load<T0RuntimeConfig>("T0Runtime").zones.text);float fov=(float)data["rows"][0]["viewNodes"].First(x=>(string)x["nodeId"]=="hub-view-drawer")["cameraPose"]["fovDeg"];Assert.AreEqual(fov,Camera.VerticalToHorizontalFieldOfView(camera.fieldOfView,camera.aspect),.001f);
   } finally {UnityEngine.Object.Destroy(cameraHost);if(previous!=null)previous.enabled=wasEnabled;}
  }
  [UnityTest] public IEnumerator AccessibilityOptionsPreserveHoldRangeAndLargeTextNavigation(){
   Click("settings");Click("hold-duration");var preferences=SaveCodec.Decode(File.ReadAllText(Path.Combine(directory,"settings.json")));Assert.AreEqual(.5f,(float)preferences["holdSeconds"]);
   for(int i=0;i<10;i++)Click("hold-duration");preferences=SaveCodec.Decode(File.ReadAllText(Path.Combine(directory,"settings.json")));Assert.AreEqual(1.5f,(float)preferences["holdSeconds"]);
   Click("hold-duration");preferences=SaveCodec.Decode(File.ReadAllText(Path.Combine(directory,"settings.json")));Assert.AreEqual(.2f,(float)preferences["holdSeconds"]);
   for(int i=0;i<5;i++)Click("text-scale");game.Back();Click("start");yield return null;yield return null;
   Assert.AreEqual(1.5f,game.Interface.TextScale);Assert.That(host.GetComponentsInChildren<UnityEngine.UI.ScrollRect>().Length,Is.GreaterThanOrEqualTo(2));
   Click("handover");yield return null;yield return null;foreach(var label in host.GetComponentsInChildren<Tide.UI.WrappedButtonHeight>()){Assert.That(label.Label.rectTransform.rect.height+.6f,Is.GreaterThanOrEqualTo(label.Label.preferredHeight),label.name+" must contain its wrapped label");}
  }
  [UnityTest] public IEnumerator ConfirmationInkUsesPresentationClockAndClearsOnContextExit(){
   var feedback=game.GetComponent<Tide.Presentation.T0CommitFeedback>();float priorStep=Time.captureDeltaTime;
   try {
    Time.captureDeltaTime=.025f;yield return null;
    feedback.Present("failed",false,false);Assert.IsFalse(feedback.IsPlaying);Assert.IsFalse(feedback.IsMarkVisible);
    feedback.Present("timed",true,false);Assert.IsTrue(feedback.IsPlaying);Assert.IsFalse(feedback.IsMarkVisible);
    bool sawAwait=false,sawImpact=false;int frames=0;
    while(feedback.IsPlaying&&frames++<20){yield return null;float elapsed=feedback.PresentationElapsed;
     Assert.AreEqual(elapsed>=.15f&&elapsed<.3f,feedback.IsMarkVisible,"Ink must follow the +150 ms marker and 300 ms envelope");
     if(elapsed<.15f)sawAwait=true;if(elapsed>=.15f&&elapsed<.3f)sawImpact=true;
    }
    Assert.IsTrue(sawAwait);Assert.IsTrue(sawImpact);Assert.IsFalse(feedback.IsPlaying);Assert.IsFalse(feedback.IsMarkVisible);Assert.That(feedback.PresentationElapsed,Is.InRange(.3f,.3251f));
    feedback.Present("timed",true,false);Assert.AreEqual(1,feedback.PresentedCount);Assert.IsFalse(feedback.IsPlaying);
    feedback.Present("reduced",true,true);Assert.IsFalse(feedback.IsPlaying);Assert.IsFalse(feedback.IsMarkVisible);Assert.AreEqual(1,feedback.PresentedCount);
    feedback.Present("context",true,false);Assert.IsTrue(feedback.IsPlaying);Click("start");Assert.IsFalse(feedback.IsPlaying);Assert.IsFalse(feedback.IsMarkVisible);
   } finally {Time.captureDeltaTime=priorStep;}
  }
  [UnityTest] public IEnumerator SaveFailureNeverEmitsSuccessAndRetryEmitsOnce(){yield return ReaderReady();game.Store.Injection=(stage,token)=>stage==SaveStage.BeforeRename?Task.FromException(new IOException("injected")):Task.CompletedTask;var failed=game.CommitAsync(new PuzzleCommand("ReadOriginal"));yield return Wait(failed);Assert.AreEqual(0,game.SuccessfulReceipts);Assert.AreEqual(0,game.Journal.State.ReadCount("rec-plate-standard-hub"));game.Store.Injection=null;Click("retry-save");while(game.SavePending)yield return null;Assert.AreEqual(1,game.SuccessfulReceipts);Assert.AreEqual(1,game.Journal.State.ReadCount("rec-plate-standard-hub"));}
  [UnityTest] public IEnumerator WorkingSaveCannotOverwriteAConfirmationPreview(){
   yield return ReaderReady();
   var before=File.ReadAllBytes(Path.Combine(directory,"save.json"));var release=new TaskCompletionSource<bool>();
   game.Store.Injection=(stage,token)=>stage==SaveStage.BeforeRename?release.Task:Task.CompletedTask;
   try{
    Click("start-next");yield return null;
    Assert.AreEqual(ActionFeedbackStage.Accepted,game.FeedbackStage);
    CollectionAssert.AreEqual(before,File.ReadAllBytes(Path.Combine(directory,"save.json")));
    Click("read-original");Assert.AreEqual(ActionFeedbackStage.Preview,game.FeedbackStage);
   }finally{game.Store.Injection=null;release.TrySetResult(true);}
   yield return Wait(game.FlushSaves());
   Assert.AreEqual(ActionFeedbackStage.Preview,game.FeedbackStage,"An older autosave must not replace the later unapplied preview");
   Assert.AreEqual(0,game.Journal.State.ReadCount("rec-plate-standard-hub"));
   Click("overlay-back");Assert.AreEqual(ActionFeedbackStage.None,game.FeedbackStage,"The visible Back control must dismiss the preview status too");
   Click("start-prev");yield return Wait(game.FlushSaves());
   Assert.AreEqual(ActionFeedbackStage.WorkSaved,game.FeedbackStage);
   Assert.AreEqual(0,game.SuccessfulReceipts);
  }
  [UnityTest] public IEnumerator SavedFeedbackRemainsVisibleAtDeepEvidenceFocus(){
   Click("settings");for(int i=0;i<5;i++)Click("text-scale");game.Back();yield return ReaderReady();
   var release=new TaskCompletionSource<bool>();game.Store.Injection=(stage,token)=>stage==SaveStage.BeforeRename?release.Task:Task.CompletedTask;
   try{
    Click("start-next");Click("pick-start");
    var lastPhase=game.Interface.ActionIds.Last(id=>id.StartsWith("phase-H",StringComparison.Ordinal));
    Assert.IsTrue(game.Interface.Focus(lastPhase));yield return null;Canvas.ForceUpdateCanvases();yield return null;
    var feedback=host.GetComponentsInChildren<UnityEngine.UI.Text>().Single(text=>text.name=="Action feedback");
    Assert.IsFalse(feedback.canvasRenderer.cull,"Working-save feedback must remain visible while the evidence list is scrolled");
    release.SetResult(true);yield return Wait(game.FlushSaves());yield return null;
    Assert.AreEqual(ActionFeedbackStage.WorkSaved,game.FeedbackStage);Assert.AreEqual(lastPhase,game.Interface.CurrentFocusId,"Finishing an autosave must not move the player's evidence focus");
    Assert.IsFalse(feedback.canvasRenderer.cull);Assert.GreaterOrEqual(feedback.rectTransform.rect.height+.5f,feedback.preferredHeight,"150% feedback text must fit its visible region");
   }finally{game.Store.Injection=null;release.TrySetResult(true);}
  }
  [UnityTest] public IEnumerator LatestQueuedHintSaveFailureRemainsVisible(){
   yield return ReaderReady();
   var first=new TaskCompletionSource<bool>();var second=new TaskCompletionSource<bool>();var enteredSecond=new TaskCompletionSource<bool>();int writes=0;
   game.Store.Injection=(stage,token)=>{
    if(stage!=SaveStage.BeforeRename)return Task.CompletedTask;
    if(++writes==1)return first.Task;
    enteredSecond.TrySetResult(true);return second.Task;
   };
   try{
    Click("start-next");var firstSave=game.FlushSaves();
    game.OpenOverlay("hints");Click("hint-next");var latestSave=game.FlushSaves();
    first.SetResult(true);yield return Wait(firstSave);yield return Wait(enteredSecond.Task);
    game.RequestConfirm(new PuzzleCommand("ReadOriginal"));Assert.AreEqual(ActionFeedbackStage.Preview,game.FeedbackStage);
    second.SetException(new IOException("m23 newer hint autosave failure"));yield return Wait(latestSave);
    Assert.AreEqual(ActionFeedbackStage.SaveFailed,game.FeedbackStage,"Neither older success nor a view change may hide the newest queued save failure");
    Click("overlay-back");Assert.AreEqual(ActionFeedbackStage.SaveFailed,game.FeedbackStage,"Closing a preview must retain a newer persistence warning");
    Assert.AreEqual(0,(int?)game.Store.Load().Document["progress"]["hintLevelUsed"]["t0-b3"]??0);
    game.Store.Injection=null;game.OpenOverlay("hints");Click("hint-next");
    if(game.Interface.ActionIds.Contains("hint-reveal"))Click("hint-reveal");
    yield return Wait(game.FlushSaves());Assert.AreEqual(ActionFeedbackStage.WorkSaved,game.FeedbackStage);
    Assert.AreEqual(2,(int?)game.Store.Load().Document["progress"]["hintLevelUsed"]["t0-b3"]??0);
   }finally{game.Store.Injection=null;first.TrySetResult(true);second.TrySetResult(true);}
  }
  [UnityTest] public IEnumerator CancelledCommitRequeuesWorkingStateWhenRedoHasNoEffect(){
   yield return ReaderReady();var before=File.ReadAllBytes(Path.Combine(directory,"save.json"));var gate=new TaskCompletionSource<bool>();
   game.Store.Injection=(stage,token)=>stage==SaveStage.BeforeRename?gate.Task:Task.CompletedTask;
   try{
    Click("start-next");var head=game.Journal.HeadSeq;
    var pending=game.CommitAsync(new PuzzleCommand("ReadOriginal"));game.Redo();
    Assert.AreEqual(head,game.Journal.HeadSeq,"Redo at the tip is not a new gameplay command");
    gate.SetException(new IOException("m23 interrupted working-save failure"));
    yield return Wait(pending);yield return Wait(game.FlushSaves());
    Assert.IsFalse(pending.Result);Assert.AreEqual(0,game.SuccessfulReceipts);
    Assert.AreEqual(ActionFeedbackStage.SaveFailed,game.FeedbackStage);
    CollectionAssert.AreEqual(before,File.ReadAllBytes(Path.Combine(directory,"save.json")));
   }finally{game.Store.Injection=null;gate.TrySetResult(true);}
  }
  [UnityTest] public IEnumerator FeedbackSeparatesFailedAutosaveFromDurableConfirmation(){
   yield return ReaderReady();var before=File.ReadAllBytes(Path.Combine(directory,"save.json"));
   game.Store.Injection=(stage,token)=>stage==SaveStage.BeforeRename?Task.FromException(new IOException("m23 autosave failure")):Task.CompletedTask;
   try{Click("start-next");yield return Wait(game.FlushSaves());
    Assert.AreEqual(ActionFeedbackStage.SaveFailed,game.FeedbackStage);
    CollectionAssert.AreEqual(before,File.ReadAllBytes(Path.Combine(directory,"save.json")));
    Assert.AreEqual(0,game.SuccessfulReceipts);
   }finally{game.Store.Injection=null;}
   Click("start-prev");yield return Wait(game.FlushSaves());
   var release=new TaskCompletionSource<bool>();game.Store.Injection=(stage,token)=>stage==SaveStage.BeforeRename?release.Task:Task.CompletedTask;
   var pending=game.CommitAsync(new PuzzleCommand("ReadOriginal"));
   try{yield return null;Assert.AreEqual(ActionFeedbackStage.CommitSaving,game.FeedbackStage);Assert.AreEqual(0,game.Journal.State.ReadCount("rec-plate-standard-hub"));}
   finally{game.Store.Injection=null;release.TrySetResult(true);}
   yield return Wait(pending);Assert.IsTrue(pending.Result);
   Assert.AreEqual(ActionFeedbackStage.CommitSaved,game.FeedbackStage);
   var restored=JournalSave.Decode(game.Store.Load().Document,game.Simulation);
   Assert.AreEqual(1,restored.State.ReadCount("rec-plate-standard-hub"));
  }
  [UnityTest] public IEnumerator AlignmentPracticeIsolatesRealShortcutsAndAllWorkingState(){
   yield return ReaderReady();var before=File.ReadAllBytes(Path.Combine(directory,"save.json"));
   var hash=game.Journal.State.StateHash;var head=game.Journal.HeadSeq;
   Click("settings");for(int i=0;i<5;i++)Click("text-scale");Click("alignment-practice");
   Assert.IsTrue(game.AlignmentPracticeActive);Assert.AreEqual(1.5f,game.Interface.TextScale);
   for(int i=0;i<3;i++){Click("practice-slot-"+i);Click("practice-a-"+i);Click("practice-b-"+i);}
   Click("practice-propose");Click("practice-apply");Click("practice-lock");Click("practice-lock");
   yield return null;Canvas.ForceUpdateCanvases();
   var work=host.GetComponentsInChildren<UnityEngine.UI.ScrollRect>().Single(s=>s.viewport.name=="Viewport");
   var summary=host.GetComponentsInChildren<RectTransform>().Single(t=>t.name=="Practice correspondence summary");
   Assert.Greater(work.content.rect.height,work.viewport.rect.height,"This case must exercise the overflowing 150% readout");
   Assert.IsTrue(game.Interface.Focus("practice-read-down"));yield return PressKey(Key.Enter);Canvas.ForceUpdateCanvases();
   var corners=new Vector3[4];var viewCorners=new Vector3[4];summary.GetWorldCorners(corners);work.viewport.GetWorldCorners(viewCorners);
   Assert.GreaterOrEqual(corners[0].y,viewCorners[0].y-.5f);Assert.LessOrEqual(corners[2].y,viewCorners[2].y+.5f);
   Assert.IsTrue(game.Interface.Focus("practice-read-up"));yield return PressKey(Key.Enter);Canvas.ForceUpdateCanvases();
   host.GetComponentInChildren<Tide.UI.AlignmentPracticeChart>().GetComponent<RectTransform>().GetWorldCorners(corners);
   Assert.GreaterOrEqual(corners[0].y,viewCorners[0].y-.5f);Assert.LessOrEqual(corners[2].y,viewCorners[2].y+.5f);
   foreach(var key in new[]{Key.Z,Key.Y,Key.I,Key.H,Key.F1,Key.Q,Key.Tab,Key.Space,Key.Digit1,Key.Digit3})yield return PressKey(key);
   game.Undo();game.Redo();game.OpenTool("circuit");game.GoNode("hub-view-desk");
   Assert.IsFalse(game.SubmitImmediate(new PuzzleCommand("Read")).IsValid);
   Assert.IsFalse(awaitResult(game.CommitAsync(new PuzzleCommand("ReadOriginal"))));
   game.RequestConfirm(new PuzzleCommand("ReadOriginal"));
   Assert.IsTrue(game.AlignmentPracticeActive);Assert.IsFalse(game.SavePending);
   Click("practice-transfer");Click("practice-reset");
   yield return PressKey(Key.Escape);Assert.AreEqual("reader",game.Surface);
   Assert.IsNull(game.Practice);yield return Wait(game.FlushSaves());
   Assert.AreEqual(hash,game.Journal.State.StateHash);Assert.AreEqual(head,game.Journal.HeadSeq);
   CollectionAssert.AreEqual(before,File.ReadAllBytes(Path.Combine(directory,"save.json")));
  }
  const System.Reflection.BindingFlags PrivateInstance=System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic;
  void HintCall(string method,params object[] args)=>typeof(T0GameSession).GetMethod(method,PrivateInstance).Invoke(game,args);
  void HintTick(float now)=>HintCall("UpdateHintCadence",now);
  void AgeHintIdle(float seconds)=>typeof(T0GameSession).GetField("lastActivity",PrivateInstance).SetValue(game,Time.unscaledTime-seconds);
  IEnumerator HintSession(float idle,float cooldown){
   yield return Wait(game.FlushSaves());UnityEngine.Object.Destroy(host);yield return null;
   hintConfig=UnityEngine.Object.Instantiate(Resources.Load<T0RuntimeConfig>("T0Runtime"));
   var table=Newtonsoft.Json.Linq.JObject.Parse(hintConfig.tools.text);
   table["knobs"]["idleHintOfferSeconds"]["value"]=idle;table["knobs"]["hintOfferCooldownSeconds"]["value"]=cooldown;
   hintTools=new TextAsset(table.ToString());hintConfig.tools=hintTools;
   host=new GameObject("Hint cadence test");game=host.AddComponent<T0GameSession>();game.Initialize(hintConfig,directory);
   game.enabled=false;game.StartGame();
  }
  [UnityTest] public IEnumerator HintIdleThresholdAndDismissCooldownAreIndependentAndNeverReveal(){
   yield return HintSession(10,40);Click("hints");Click("hint-next");Click("overlay-back");yield return Wait(game.FlushSaves());
   var before=game.Store.Load().Document["progress"]["hintLevelUsed"].DeepClone();var head=game.Journal.HeadSeq;
   float now=Mathf.Ceil(Time.unscaledTime);typeof(T0GameSession).GetField("lastActivity",PrivateInstance).SetValue(game,now);
   HintTick(now+9);Assert.IsFalse(game.HintOfferVisible);HintTick(now+10);Assert.IsTrue(game.HintOfferVisible);
   HintCall("DismissHintOffer",now+10);Assert.IsFalse(game.HintOfferVisible);
   HintTick(now+49);Assert.IsFalse(game.HintOfferVisible,"The idle threshold cannot shorten the independent cooldown");
   HintTick(now+50);Assert.IsTrue(game.HintOfferVisible);
   Assert.AreEqual(head,game.Journal.HeadSeq);
   Assert.IsTrue(game.SubmitImmediate(new PuzzleCommand("ViewLine","rec-handover-brief","hb-l1")).IsValid);yield return Wait(game.FlushSaves());
   Assert.IsTrue(Newtonsoft.Json.Linq.JToken.DeepEquals(before,game.Store.Load().Document["progress"]["hintLevelUsed"]));
  }
  [UnityTest] public IEnumerator ShortCooldownCannotBypassIdleThreshold(){
   yield return HintSession(40,10);
   // Exact boundary checks need an exactly representable synthetic clock origin.
   float now=Mathf.Ceil(Time.unscaledTime);typeof(T0GameSession).GetField("lastActivity",PrivateInstance).SetValue(game,now);
   HintTick(now+39);Assert.IsFalse(game.HintOfferVisible);HintTick(now+40);Assert.IsTrue(game.HintOfferVisible);
   HintCall("DismissHintOffer",now+40);typeof(T0GameSession).GetField("lastActivity",PrivateInstance).SetValue(game,now+40);
   HintTick(now+51);Assert.IsFalse(game.HintOfferVisible,"Cooldown expiry alone must not offer");
   HintTick(now+80);Assert.IsTrue(game.HintOfferVisible);
  }
  [UnityTest] public IEnumerator KeyboardControllerAndPointerActivityResetIdleWithoutRebuildingScreen(){
   yield return HintSession(10,0);int renders=0;game.Interface.ScreenChanged+=()=>renders++;
   AgeHintIdle(20);yield return PressKey(Key.Tab);HintTick(Time.unscaledTime+9);Assert.IsFalse(game.HintOfferVisible);
   AgeHintIdle(20);yield return Pad(GamepadButton.DpadDown);HintTick(Time.unscaledTime+9);Assert.IsFalse(game.HintOfferVisible);
   var mouse=InputSystem.AddDevice<Mouse>();
   try{
    AgeHintIdle(20);InputSystem.QueueStateEvent(mouse,new MouseState{delta=new Vector2(3,2),scroll=new Vector2(0,1)});yield return null;
    HintTick(Time.unscaledTime+9);Assert.IsFalse(game.HintOfferVisible);
    HintTick(Time.unscaledTime+11);Assert.IsTrue(game.HintOfferVisible);
    yield return PressKey(Key.Tab);Assert.IsTrue(game.HintOfferVisible,"Focus traversal must leave the offered actions reachable");
    yield return PressKey(Key.Escape);Assert.IsFalse(game.HintOfferVisible);Assert.AreEqual(1,game.HintOfferDismissedCount);
    Assert.AreEqual(0,renders,"Hint activity and offer visibility must not cancel input ownership by rebuilding the screen");
   }finally{InputSystem.RemoveDevice(mouse);}
  }
  [UnityTest] public IEnumerator OfferedHintIsFocusableWithoutStealingFocusAndOpensOnlyOnActivation(){
   yield return HintSession(10,40);Assert.IsTrue(game.Interface.Focus("handover"));
   var head=game.Journal.HeadSeq;var focus=game.Interface.CurrentFocusId;
   int renders=0;game.Interface.ScreenChanged+=()=>renders++;
   HintTick(Time.unscaledTime+11);Assert.IsTrue(game.HintOfferVisible);Assert.AreEqual(focus,game.Interface.CurrentFocusId);
   Assert.IsTrue(game.Interface.Focus("hint-offer-dismiss"));yield return PressKey(Key.Enter);
   Assert.IsFalse(game.HintOfferVisible);Assert.AreEqual(1,game.HintOfferDismissedCount);Assert.AreEqual("shell",game.Surface);
   Assert.AreEqual(0,renders,"Enter must dismiss the offer without rebuilding or activating the underlying screen");
   Assert.AreEqual(head,game.Journal.HeadSeq,"Enter dismissal must not submit an underlying gameplay command");
   Assert.AreEqual(focus,game.Interface.CurrentFocusId,"Dismissing the offer must restore the previous ordinary focus");
   Assert.IsFalse(game.Interface.Focus("hint-offer-dismiss"),"Hidden offer controls must leave the focus ring");
   HintTick(Time.unscaledTime+39);Assert.IsFalse(game.HintOfferVisible);
   HintTick(Time.unscaledTime+41);Assert.IsTrue(game.HintOfferVisible);
   yield return InputClick("hint-offer-open",true);
   Assert.AreEqual("hints",game.Surface);Assert.IsFalse(game.HintOfferVisible);
   Assert.AreEqual(1,game.HintOfferDismissedCount,"Opening help is not a dismissal");
   Assert.AreEqual(head,game.Journal.HeadSeq);
  }
  [UnityTest] public IEnumerator LargeTextHintOfferStaysOutsideHeadingsAndCancelDoesNotLeaveTheTool(){
   yield return HintSession(10,0);game.Interface.TextScale=1.5f;game.OpenTool("reader");
   var head=game.Journal.HeadSeq;int renders=0;game.Interface.ScreenChanged+=()=>renders++;
   HintTick(Time.unscaledTime+11);yield return null;Canvas.ForceUpdateCanvases();
   var offer=host.GetComponentsInChildren<RectTransform>().Single(x=>x.name=="Hint offer");
   var offerCorners=new Vector3[4];offer.GetWorldCorners(offerCorners);
   foreach(var heading in host.GetComponentsInChildren<UnityEngine.UI.Text>().Where(x=>x.name=="Title"||x.name=="Subtitle")){
    var corners=new Vector3[4];heading.rectTransform.GetWorldCorners(corners);
    Assert.Less(corners[2].x,offerCorners[0].x,"Offer controls must not cover either heading");
    Assert.GreaterOrEqual(heading.rectTransform.rect.height+.6f,heading.preferredHeight,heading.name+" must remain readable");
   }
   foreach(var label in offer.GetComponentsInChildren<UnityEngine.UI.Text>())
    Assert.GreaterOrEqual(label.rectTransform.rect.height+.6f,label.preferredHeight,"Offered actions must not clip at 150%");
   yield return PressKey(Key.Escape);
   Assert.IsFalse(game.HintOfferVisible);Assert.AreEqual("reader",game.Surface);Assert.AreEqual(1,game.HintOfferDismissedCount);
   Assert.AreEqual(0,renders,"Escape must dismiss the offer without rebuilding the reader");
   Assert.AreEqual(head,game.Journal.HeadSeq,"Escape dismissal must not submit a gameplay command");
   HintTick(Time.unscaledTime+11);Assert.IsTrue(game.HintOfferVisible);
   yield return Pad(GamepadButton.East);
   Assert.IsFalse(game.HintOfferVisible);Assert.AreEqual("reader",game.Surface);Assert.AreEqual(2,game.HintOfferDismissedCount);
   Assert.AreEqual(0,renders,"Controller cancel must dismiss the offer without rebuilding the reader");
   Assert.AreEqual(head,game.Journal.HeadSeq,"Controller dismissal must not submit a gameplay command");
  }
  [UnityTest] public IEnumerator DocumentsOverlaysAndPendingCommitDoNotBankIdleTime(){
   yield return HintSession(10,0);Click("handover");HintTick(Time.unscaledTime+1000);Assert.IsFalse(game.HintOfferVisible);
   Click("close-document");HintTick(Time.unscaledTime+9);Assert.IsFalse(game.HintOfferVisible);
   Click("hints");HintTick(Time.unscaledTime+1000);Assert.IsFalse(game.HintOfferVisible);
   game.Back();HintTick(Time.unscaledTime+9);Assert.IsFalse(game.HintOfferVisible);
   var release=new TaskCompletionSource<bool>();game.Store.Injection=(stage,token)=>stage==SaveStage.BeforeRename?release.Task:Task.CompletedTask;
   var pending=game.CommitAsync(new PuzzleCommand("ViewLine","rec-handover-brief","hb-l1"));
   try{
    Assert.IsTrue(game.SavePending);Click("overlay-back");Assert.AreEqual("shell",game.Surface);
    HintTick(Time.unscaledTime+1000);Assert.IsFalse(game.HintOfferVisible,"A dismissed pending overlay must not allow offers during the durable write");
   }finally{game.Store.Injection=null;release.SetResult(true);}
   yield return Wait(pending);Assert.IsTrue(pending.Result);HintTick(Time.unscaledTime+9);Assert.IsFalse(game.HintOfferVisible);
   HintTick(Time.unscaledTime+11);Assert.IsTrue(game.HintOfferVisible);
  }
 }
}
#endif
