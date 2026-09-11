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
  [UnitySetUp] public IEnumerator SetUp(){
#if UNITY_EDITOR
originalEditorInput=InputSystem.settings.editorInputBehaviorInPlayMode;InputSystem.settings.editorInputBehaviorInPlayMode=InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
#endif
originalBackground=InputSystem.settings.backgroundBehavior;InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;directory=Path.Combine(Path.GetTempPath(),"t0-play-"+Guid.NewGuid().ToString("N"));keyboard=InputSystem.AddDevice<Keyboard>();pad=InputSystem.AddDevice<Gamepad>();host=new GameObject("T0 Play test");game=host.AddComponent<T0GameSession>();game.Initialize(Resources.Load<T0RuntimeConfig>("T0Runtime"),directory);yield return null;}
  [UnityTearDown] public IEnumerator TearDown(){var task=game.FlushSaves();while(!task.IsCompleted)yield return null;UnityEngine.Object.Destroy(host);InputSystem.RemoveDevice(keyboard);InputSystem.RemoveDevice(pad);InputSystem.settings.backgroundBehavior=originalBackground;
#if UNITY_EDITOR
InputSystem.settings.editorInputBehaviorInPlayMode=originalEditorInput;
#endif
yield return null;if(Directory.Exists(directory))Directory.Delete(directory,true);}
  IEnumerator PressKey(Key key){InputSystem.QueueStateEvent(keyboard,new KeyboardState(key));yield return null;InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return null;}
  IEnumerator Pad(GamepadButton key){InputSystem.QueueStateEvent(pad,new GamepadState().WithButton(key));yield return null;InputSystem.QueueStateEvent(pad,new GamepadState());yield return null;}
  void Click(string id){Assert.IsTrue(game.Interface.Activate(id),"Missing UI action "+id);}
  void Intake(){Click("start");Click("handover");foreach(var id in new[]{"hb-l1","hb-l2","hb-l3"})Click("line-"+id);Click("close-document");Click("transfer");foreach(var id in new[]{"tl-r1","tl-r2","tl-r3"})Click("row-"+id);Click("decision-written");Click("close-document");Click("load-plate-zero");}
  void Circuit(){Click("hub-view-circuitmap");Click("open-circuit");Click("trace-hub");Click("begin-overlay");Click("offset-left");Click("offset-up");Click("anchor-overlay");foreach(var area in game.Definition.UncoveredAreas){Click("area-"+area);Click("area-evidence-"+area);Click("attach-rec-watchlog-bureau");Click("overlay-back");}}
  IEnumerator Wait(Task task){while(!task.IsCompleted)yield return null;if(task.IsFaulted)throw task.Exception;}
  IEnumerator ReaderReady(){Intake();Circuit();Click("hub-view-reader");Click("open-reader");Click("load-rec-plate-standard-hub");Click("read");yield return Wait(game.FlushSaves());}
  [UnityTest] public IEnumerator KeyboardStartsAndHeldConfirmCannotCrossScreen(){Assert.AreEqual("shell",game.Surface);InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.Enter));yield return null;yield return null;Assert.IsTrue(game.Interface.ActionIds.Contains("start"));InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return null;Assert.IsTrue(game.Interface.ActionIds.Contains("handover"));InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return null;Assert.IsTrue(game.Interface.ActionIds.Contains("handover"));}
  [UnityTest] public IEnumerator GamepadStartsAndShellQueryHasNoAction(){yield return Pad(GamepadButton.South);Assert.IsTrue(game.Watch.LastDeviceIsGamepad);Assert.IsTrue(game.Interface.ActionIds.Contains("handover"));yield return PressKey(UnityEngine.InputSystem.Key.Q);Assert.IsFalse(game.Watch.LastDeviceIsGamepad);Assert.AreEqual("shell",game.Surface);yield return Pad(GamepadButton.RightStick);Assert.AreEqual("shell",game.Surface);yield return PressKey(UnityEngine.InputSystem.Key.I);Assert.AreEqual("evidence",game.Surface);yield return PressKey(UnityEngine.InputSystem.Key.F1);Assert.AreEqual("hints",game.Surface);}
  [UnityTest] public IEnumerator UiActionAdapterCompletesCanonicalT0AndReopensSavedState(){yield return ReaderReady();foreach(var id in new[]{"rec-plate-standard-hub","rec-tide-ledger-bureau"}){if(game.Journal.State.LoadedRecordId!=id){Click("select-record");Click("choose-"+id);Click("read");}var verdict=game.SubmitImmediate(new PuzzleCommand("SetWindow",value:"H-1:00",otherValue:"H+3:00"));Assert.IsTrue(verdict.IsValid);Click("cite");Click("confirm-submit");while(game.SavePending)yield return null;}Assert.IsTrue(game.Simulation.IsComplete(game.Journal.State,"t0-b3"));Assert.AreEqual(2,game.SuccessfulReceipts);yield return Wait(game.FlushSaves());var loaded=game.Store.Load(validate:d=>JournalSave.Decode(d,game.Simulation));var restored=JournalSave.Decode(loaded.Document,game.Simulation);Assert.AreEqual(game.Journal.State.StateHash,restored.State.StateHash);}
  [UnityTest] public IEnumerator ThreeConfirmationModesRequireTheirOwnGesture(){yield return ReaderReady();game.SetConfirmMode("hold");Click("read-original");Assert.IsTrue(game.Interface.Focus("confirm-submit"));game.Interface.BeginActivation();yield return new WaitForSecondsRealtime(.1f);game.Interface.EndActivation();Assert.IsFalse(game.SavePending);Assert.AreEqual(0,game.Journal.State.ReadCount("rec-plate-standard-hub"));game.Interface.BeginActivation();yield return new WaitForSecondsRealtime(.45f);game.Interface.EndActivation();while(game.SavePending)yield return null;Assert.AreEqual(1,game.Journal.State.ReadCount("rec-plate-standard-hub"));game.SetConfirmMode("two-step");Click("read-original");Assert.IsTrue(game.Interface.ActionIds.Contains("preview-next"));Assert.IsFalse(game.Interface.ActionIds.Contains("confirm-submit"));Click("preview-next");Click("confirm-submit");while(game.SavePending)yield return null;Assert.AreEqual(2,game.Journal.State.ReadCount("rec-plate-standard-hub"));game.SetConfirmMode("confirm-dialog");Click("read-original");Click("confirm-submit");while(game.SavePending)yield return null;Assert.AreEqual(3,game.Journal.State.ReadCount("rec-plate-standard-hub"));}
  [UnityTest] public IEnumerator PendingAllowsOverlayUndoAndSuppressesStaleReceipt(){yield return ReaderReady();var release=new TaskCompletionSource<bool>();game.Store.Injection=(stage,token)=>stage==SaveStage.BeforeRename?release.Task:Task.CompletedTask;var pending=game.CommitAsync(new PuzzleCommand("ReadOriginal"));yield return null;Assert.IsTrue(game.SavePending);Assert.IsFalse(awaitResult(game.CommitAsync(new PuzzleCommand("ReadOriginal"))));yield return PressKey(UnityEngine.InputSystem.Key.F1);Assert.AreEqual("hints",game.Surface);game.Undo();Assert.IsFalse(game.SavePending);game.Store.Injection=null;release.SetResult(true);yield return Wait(pending);yield return Wait(game.FlushSaves());Assert.AreEqual(0,game.SuccessfulReceipts);Assert.AreEqual(0,game.Journal.State.ReadCount("rec-plate-standard-hub"));}
  [UnityTest] public IEnumerator CanonicalToolRoutingAndRebindingRetainIntent(){yield return ReaderReady();yield return PressKey(UnityEngine.InputSystem.Key.Q);Assert.AreEqual("reader",game.Surface);int before=game.Journal.State.ReadCount("rec-plate-standard-hub");yield return Pad(GamepadButton.West);Assert.AreEqual(before,game.Journal.State.ReadCount("rec-plate-standard-hub"));Assert.AreEqual("reader",game.Surface);game.SubmitImmediate(new PuzzleCommand("SetWindow",value:"H-1:00",otherValue:"H+3:00"));yield return Pad(GamepadButton.North);Assert.AreEqual("confirm",game.Surface);game.Back();game.Back();yield return Pad(GamepadButton.North);Assert.AreEqual("toolWheel",game.Surface);game.Back();var action=game.Watch.Actions.FindAction("Overlay");var index=action.bindings.Select((b,i)=>new{b,i}).First(x=>x.b.name=="hypothesis").i;action.ApplyBindingOverride(index,"<Keyboard>/j");yield return PressKey(UnityEngine.InputSystem.Key.J);Assert.AreEqual("hypothesis",game.Surface);}
  IEnumerator InputClick(string id,bool controller){int moves=0;while(game.Interface.CurrentFocusId!=id){Assert.Less(moves++,100,"Unreachable focus "+id);if(controller){InputSystem.QueueStateEvent(pad,new GamepadState{leftStick=new Vector2(0,-1)});yield return null;InputSystem.QueueStateEvent(pad,new GamepadState());yield return null;}else yield return PressKey(UnityEngine.InputSystem.Key.Tab);}if(controller)yield return Pad(GamepadButton.South);else yield return PressKey(UnityEngine.InputSystem.Key.Enter);}
  IEnumerator FullInputRun(bool controller){
   yield return InputClick("start",controller);yield return InputClick("handover",controller);foreach(var id in new[]{"hb-l1","hb-l2","hb-l3"})yield return InputClick("line-"+id,controller);yield return InputClick("close-document",controller);yield return InputClick("transfer",controller);foreach(var id in new[]{"tl-r1","tl-r2","tl-r3"})yield return InputClick("row-"+id,controller);yield return InputClick("decision-blank",controller);yield return InputClick("close-document",controller);yield return InputClick("load-plate-zero",controller);
   yield return InputClick("hub-view-circuitmap",controller);yield return InputClick("open-circuit",controller);yield return InputClick("begin-overlay",controller);yield return InputClick("offset-left",controller);yield return InputClick("offset-up",controller);yield return InputClick("anchor-overlay",controller);
   foreach(var area in game.Definition.UncoveredAreas){yield return InputClick("area-"+area,controller);yield return InputClick("area-evidence-"+area,controller);yield return InputClick("attach-rec-watchlog-bureau",controller);yield return InputClick("overlay-back",controller);}
   yield return InputClick("hub-view-reader",controller);yield return InputClick("open-reader",controller);yield return InputClick("load-rec-plate-standard-hub",controller);
   foreach(var id in new[]{"rec-plate-standard-hub","rec-tide-ledger-bureau"}){if(game.Journal.State.LoadedRecordId!=id){yield return InputClick("select-record",controller);yield return InputClick("choose-"+id,controller);}yield return InputClick("read",controller);yield return InputClick("pick-start",controller);int pages=0;while(!game.Interface.ActionIds.Contains("phase-H-1:00")){Assert.Less(pages++,30);yield return InputClick("phase-next",controller);}yield return InputClick("phase-H-1:00",controller);yield return InputClick("pick-end",controller);pages=0;while(!game.Interface.ActionIds.Contains("phase-H+3:00")){Assert.Less(pages++,30);yield return InputClick("phase-prev",controller);}yield return InputClick("phase-H+3:00",controller);yield return InputClick("cite",controller);yield return InputClick("confirm-submit",controller);while(game.SavePending)yield return null;}
   Assert.IsTrue(game.Simulation.IsComplete(game.Journal.State,"t0-b3"));Assert.AreEqual(2,game.SuccessfulReceipts);
  }
  [UnityTest] public IEnumerator KeyboardOnlyCompletesAllThreeBeats(){yield return FullInputRun(false);}
  [UnityTest] public IEnumerator GamepadOnlyCompletesAllThreeBeats(){yield return FullInputRun(true);}
  IEnumerator Restart(){yield return Wait(game.FlushSaves());UnityEngine.Object.Destroy(host);yield return null;host=new GameObject("Restarted T0");game=host.AddComponent<T0GameSession>();game.Initialize(Resources.Load<T0RuntimeConfig>("T0Runtime"),directory);yield return null;}
  [UnityTest] public IEnumerator RecoveryNewSlotIsSelectableAfterSessionRestart(){var future=JournalSave.Encode(game.Journal,"future",20000,6291456);future["schemaVersion"]=4;yield return Wait(game.Store.WriteAsync(future));var original=File.ReadAllBytes(Path.Combine(directory,"save.json"));yield return Restart();Assert.AreEqual("recovery",game.Surface);Click("recovery-new-slot");Click("start");Click("handover");Click("line-hb-l1");yield return Wait(game.FlushSaves());Assert.AreNotEqual(directory,game.Store.DirectoryPath);yield return Restart();Assert.AreEqual("recovery",game.Surface);Click("select-slot-1");Click("start");Assert.AreNotEqual("recovery",game.Surface);Assert.IsTrue(game.Journal.State.Has("line:rec-handover-brief:hb-l1"));CollectionAssert.AreEqual(original,File.ReadAllBytes(Path.Combine(directory,"save.json")));}
  [UnityTest] public IEnumerator ValidatedRetryClearsRefusalAndPreservesSaveIdentity(){var valid=JournalSave.Encode(game.Journal,"valid",20000,6291456);yield return Wait(game.Store.WriteAsync(valid));var future=(Newtonsoft.Json.Linq.JObject)valid.DeepClone();future["schemaVersion"]=4;future["commitIdempotencyKey"]="future";yield return Wait(game.Store.WriteAsync(future));yield return Restart();Assert.AreEqual("recovery",game.Surface);File.Copy(Path.Combine(directory,"save.bak"),Path.Combine(directory,"save.json"),true);Click("recovery-retry");Click("start");Assert.AreNotEqual("recovery",game.Surface);var commit=game.CommitAsync(new PuzzleCommand("ViewLine","rec-handover-brief","hb-l1"));yield return Wait(commit);Assert.IsTrue(commit.Result);var loaded=game.Store.Load();Assert.AreEqual((string)valid["saveId"],(string)loaded.Document["saveId"]);Assert.AreEqual((string)valid["createdUtc"],(string)loaded.Document["createdUtc"]);}
  static bool awaitResult(Task<bool> task){Assert.IsTrue(task.IsCompleted);return task.Result;}
  [UnityTest] public IEnumerator ReaderWaveformUsesViewportClipAndCullsOutsideScrollArea(){
   yield return ReaderReady();yield return null;yield return null;var chart=host.GetComponentInChildren<Tide.UI.SignalChart>();Assert.IsNotNull(chart);Canvas.ForceUpdateCanvases();Assert.IsTrue(chart.canvasRenderer.hasRectClipping,"Waveform must participate in RectMask2D clipping");
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
 }
}
#endif
