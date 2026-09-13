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
using Tide.Save;
using Tide.Sim;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
namespace Tide.Tests {
 // RFC-CX-017: deliberate successful reads own crank motion; navigation and failed saves cannot imitate reading.
 public sealed class M7ReaderPlayModeTests {
  string directory;GameObject host;T0GameSession game;Scene hub;M7ReaderStageProfile profile;bool originalApproved;
  static bool Diagnostic=>Environment.GetCommandLineArgs().Contains("--m7-reader-diagnostic");
  [UnitySetUp] public IEnumerator Setup(){
   Assert.IsFalse(SceneManager.GetSceneByName("hub").isLoaded,"Fixture must load the committed hub itself");
   yield return SceneManager.LoadSceneAsync("hub",LoadSceneMode.Additive);
   hub=SceneManager.GetSceneByName("hub");
   Assert.IsNotNull(Camera.main,"The committed hub must provide the MainCamera the stage is posed on");
   directory=Path.Combine(Path.GetTempPath(),"m7-reader-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(directory);
   profile=Resources.Load<M7ReaderStageProfile>("M7ReaderStage");if(profile!=null)originalApproved=profile.runtimeApproved;
  }
  void Create(){host=new GameObject("M7 reader test");game=host.AddComponent<T0GameSession>();game.Initialize(Resources.Load<T0RuntimeConfig>("T0Runtime"),directory);}
  IEnumerator Wait(Task task){while(!task.IsCompleted)yield return null;if(task.IsFaulted)throw task.Exception;}
  [UnityTearDown] public IEnumerator TearDown(){
   if(profile!=null)profile.runtimeApproved=originalApproved;
   if(game!=null)yield return Wait(game.FlushSaves());
   if(host!=null)UnityEngine.Object.Destroy(host);
   yield return null;
   if(hub.IsValid()&&hub.isLoaded)yield return SceneManager.UnloadSceneAsync(hub);
   if(directory!=null&&Directory.Exists(directory))Directory.Delete(directory,true);
  }
  void Click(string id){Assert.IsTrue(game.Interface.Activate(id),"Missing UI action "+id);}
  // Same route T0PlayModeTests.ReaderReady takes to the reader tool: hub node, then open-reader.
  IEnumerator Boot(){Create();yield return null;game.StartGame();yield return null;Assert.IsFalse(game.PatrolActive,"Fresh save must not start inside C1");}
  // hub-view-reader is a GoNode: it re-poses the camera itself, so gate-off pose snapshots are taken after it and before open-reader.
  void OpenReader(){Click("hub-view-reader");Click("open-reader");Assert.AreEqual("reader",game.Surface);}
  Transform FindVisual()=>game.transform.Cast<Transform>().FirstOrDefault(t=>t.name==T0GameSession.M7ReaderVisualName);
  GameObject[] ActiveHubRoots()=>hub.GetRootGameObjects().Where(g=>g.activeSelf&&g.GetComponentsInChildren<Renderer>().Length>0).ToArray();
  Light[] HubLights()=>hub.GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<Light>(true)).ToArray();
  [UnityTest] public IEnumerator GateOffNeverEntersReaderStage(){
   if(Diagnostic)Assert.Ignore("--m7-reader-diagnostic forces the gate on; the gate-off contract is not observable in this run");
   if(profile!=null)profile.runtimeApproved=false;
   yield return Boot();
   Click("hub-view-reader");yield return null;
   var camera=Camera.main;var position=camera.transform.position;var rotation=camera.transform.rotation;var background=camera.backgroundColor;var clear=camera.clearFlags;
   var roots=ActiveHubRoots();Assert.IsNotEmpty(roots,"Committed hub must expose rendered roots");
   Click("open-reader");Assert.AreEqual("reader",game.Surface);yield return null;yield return null;
   Assert.IsNull(FindVisual(),"Gate off must not build the M7 optical reader");
   Assert.IsFalse(game.M7ReaderStageActive,"Gate off must keep the hub stage");
   CollectionAssert.AreEquivalent(roots,ActiveHubRoots(),"Gate off must not hide or add hub roots");
   foreach(var root in roots)Assert.IsTrue(root.activeSelf,root.name+" must stay active");
   Assert.Less(Vector3.Distance(position,camera.transform.position),.0001f,"Gate off must not move the camera");Assert.Less(Quaternion.Angle(rotation,camera.transform.rotation),.01f,"Gate off must not turn the camera");
   Assert.AreEqual(background,camera.backgroundColor);Assert.AreEqual(clear,camera.clearFlags);
   Assert.AreEqual(0,host.GetComponentsInChildren<Light>(true).Length,"Gate off must not add lights under the session");
  }
  [UnityTest] public IEnumerator GateOnBuildsStageWithTwoLightsAndTearsDownOnExit(){
   if(profile==null)Assert.Ignore("M7ReaderStage.asset not imported — run Tools/M7/Import reader stage candidates first");
   if(profile.reader==null)Assert.Ignore("M7ReaderStage.asset reader prefab missing — rerun Tools/M7/Import reader stage candidates");
   profile.runtimeApproved=true;
   yield return Boot();
   var camera=Camera.main;var roots=ActiveHubRoots();
   OpenReader();yield return null;yield return null;
   var visual=FindVisual();Assert.IsNotNull(visual,"Gate on must build the M7 optical reader under the session");
   Assert.AreEqual(1,game.transform.Cast<Transform>().Count(t=>t.name==T0GameSession.M7ReaderVisualName),"Exactly one M7 optical reader root");
   Assert.IsTrue(game.M7ReaderStageActive,"Gate on must show the reader stage");
   var lights=visual.GetComponentsInChildren<Light>(true);
   Assert.AreEqual(2,lights.Length,"Exactly two lights under the M7 optical reader");
   Assert.Less(Quaternion.Angle(profile.reader.GetComponentsInChildren<Transform>(true).First(t=>t.name==profile.crankPivotName).localRotation,
    visual.GetComponent<M7ReaderStageVisual>().CrankPivot.localRotation),.01f,"Entering the reader must preserve its authored rest pose");
   foreach(var root in roots)Assert.IsFalse(root.activeSelf,root.name+" must be hidden while the reader stage is shown");
   // Leaving the tool the way every other test does: Back() closes the reader, the stage returns to the hub and the visual is torn down.
   game.Back();yield return null;yield return null;yield return null;
   Assert.AreEqual("shell",game.Surface);
   Assert.IsNull(FindVisual(),"Leaving the reader must tear down the M7 optical reader");
   Assert.IsFalse(game.M7ReaderStageActive,"Leaving the reader must return to the hub stage");
   Assert.AreEqual(0,host.GetComponentsInChildren<Light>(true).Length,"No reader lights may survive the exit");
   foreach(var root in roots)Assert.IsTrue(root.activeSelf,root.name+" must be re-shown after leaving the reader");
  }
  void RequireReader(){
   Assert.IsNotNull(profile,"Committed reader profile is required");
   Assert.IsNotNull(profile.reader,"Committed reader prefab is required");
   profile.runtimeApproved=true;
  }
  void PrepareReader(){
   Click("hub-view-desk");Click("handover");foreach(var id in new[]{"hb-l1","hb-l2","hb-l3"})Click("line-"+id);Click("close-document");
   Click("transfer");foreach(var id in new[]{"tl-r1","tl-r2","tl-r3"})Click("row-"+id);Click("decision-written");Click("close-document");Click("load-plate-zero");
   Click("hub-view-circuitmap");Click("open-circuit");Click("trace-hub");Click("begin-overlay");Click("offset-left");Click("offset-up");Click("anchor-overlay");
   foreach(var area in game.Definition.UncoveredAreas){Click("area-"+area);Click("area-evidence-"+area);Click("attach-rec-watchlog-bureau");Click("overlay-back");}
   OpenReader();Click("load-rec-plate-standard-hub");
  }
  M7ReaderStageVisual Driver()=>FindVisual().GetComponent<M7ReaderStageVisual>();
  IEnumerator ObserveStroke(M7ReaderStageVisual driver){
   Assert.IsTrue(driver.StrokePlaying,"A successful deliberate read starts one response");
   var state=game.Journal.State;var camera=Camera.main;var position=camera.transform.position;var rotation=camera.transform.rotation;
   var rest=profile.reader.GetComponentsInChildren<Transform>(true).First(t=>t.name==profile.crankPivotName).localRotation;
   bool moved=false;float deadline=Time.realtimeSinceStartup+3;
   while(driver.StrokePlaying&&Time.realtimeSinceStartup<deadline){
    yield return null;moved|=driver.CrankAngleDeg>0;Assert.LessOrEqual(driver.CrankAngleDeg,150);
   }
   Assert.IsTrue(moved,"The response must actually move the crank");
   Assert.IsFalse(driver.StrokePlaying,"The response must finish");
   Assert.Less(Quaternion.Angle(rest,driver.CrankPivot.localRotation),.01f);
   Assert.AreSame(state,game.Journal.State,"Presentation cannot write simulation state");
   Assert.Less(Vector3.Distance(position,camera.transform.position),.0001f);Assert.Less(Quaternion.Angle(rotation,camera.transform.rotation),.01f);
   yield return null;yield return null;Assert.IsFalse(driver.StrokePlaying,"Rendering cannot replay a finished read");
  }
  [UnityTest] public IEnumerator ImportedGripTracksTheHandleThroughoutARead(){
   RequireReader();
   var embodiment=Resources.Load<M22EmbodimentProfile>("M22Embodiment");Assert.IsNotNull(embodiment);
   bool diagnostic=embodiment.diagnosticOverride;embodiment.diagnosticOverride=true;
   try{
    yield return Boot();PrepareReader();
    var hands=host.GetComponentInChildren<M22EmbodimentVisual>();Assert.IsNotNull(hands);
    var probe=host.AddComponent<M22GripProbe>();probe.Hands=hands;probe.Reader=Driver();probe.Origin=hands.HandleContactPosition;
    Click("read");yield return ObserveStroke(Driver());
    Assert.Greater(probe.Samples,2,"Observe rendered poses, not only initialization");
    Assert.Greater(probe.Travel,.02f,"The grip must follow the moving handle, not stay at its rest position");
    Assert.Less(probe.MaxError,.0001f,"Contact must remain pinned after LateUpdate throughout the stroke");
   }finally{embodiment.diagnosticOverride=diagnostic;}
  }
  [UnityTest] public IEnumerator InsertTouchesTheSupportAndReturnsWithinReach(){
   RequireReader();
   var embodiment=Resources.Load<M22EmbodimentProfile>("M22Embodiment");Assert.IsNotNull(embodiment);
   bool diagnostic=embodiment.diagnosticOverride;embodiment.diagnosticOverride=true;
   try{
    yield return Boot();PrepareReader();
    var hands=host.GetComponentInChildren<M22EmbodimentVisual>();Assert.IsNotNull(hands);
    var ready=hands.LeftReadyContactPosition;
    float reach=Vector3.Distance(ready,hands.SupportContactPosition);
    Assert.Greater(reach,.1f,"Ready must release the hardware rather than stay planted");
    Assert.Less(reach,.4f,"A scaled hardware hierarchy must not multiply the actor's reach");
    float closest=float.MaxValue,deadline=Time.realtimeSinceStartup+2;
    while(hands.CurrentAction!=M22EmbodimentVisual.Action.None&&Time.realtimeSinceStartup<deadline){
     yield return null;
     closest=Mathf.Min(closest,Vector3.Distance(hands.LeftContactPosition,hands.SupportContactPosition));
    }
    Assert.Less(closest,.001f,"A successful insertion must actually contact the support");
    Assert.Less(Vector3.Distance(hands.LeftContactPosition,ready),.001f,"The hand must release and return without a navigation action");
    Assert.IsFalse(Driver().StrokePlaying,"Inserting is not a successful crank read");
   }finally{embodiment.diagnosticOverride=diagnostic;}
  }
  [UnityTest] public IEnumerator ApplicationSuspensionCancelsHandsAndCrankWithoutDeferredReplay(){
   RequireReader();yield return Boot();PrepareReader();
   var driver=Driver();var hands=host.GetComponentInChildren<M22EmbodimentVisual>();Assert.IsNotNull(hands);
   var rest=driver.CrankPivot.localRotation;
   foreach(var signal in new[]{"OnApplicationPause","OnApplicationFocus"}){
    Click("read");float deadline=Time.realtimeSinceStartup+1;
    while(driver.CrankAngleDeg<.1f&&Time.realtimeSinceStartup<deadline)yield return null;
    Assert.GreaterOrEqual(driver.CrankAngleDeg,.1f);var state=game.Journal.State;
    bool suspendedValue=signal=="OnApplicationPause";
    driver.SendMessage(signal,suspendedValue);hands.SendMessage(signal,suspendedValue);
    driver.PlayStroke();hands.Play(M22EmbodimentVisual.Action.Read);
    yield return new WaitForSecondsRealtime(.05f);
    Assert.IsFalse(driver.StrokePlaying);Assert.AreEqual(M22EmbodimentVisual.Action.None,hands.CurrentAction);
    Assert.Less(Quaternion.Angle(rest,driver.CrankPivot.localRotation),.01f);
    driver.SendMessage(signal,!suspendedValue);hands.SendMessage(signal,!suspendedValue);yield return null;
    Assert.IsFalse(driver.StrokePlaying);Assert.AreEqual(M22EmbodimentVisual.Action.None,hands.CurrentAction);
    Assert.AreSame(state,game.Journal.State);
   }
   Click("read");yield return ObserveStroke(driver);
  }
  [UnityTest] public IEnumerator OnlySuccessfulDeliberateReadsMoveCrank(){
   RequireReader();yield return Boot();OpenReader();yield return null;yield return null;
   Assert.IsFalse(Driver().StrokePlaying,"Stage entry is not a read");
   Assert.IsFalse(game.SubmitImmediate(new PuzzleCommand("Read")).IsValid);
   yield return null;Assert.IsFalse(Driver().StrokePlaying,"Failed Read is not a visual action");
   PrepareReader();yield return null;Assert.IsFalse(Driver().StrokePlaying,"Loading a record is not a read");
   Click("read");yield return ObserveStroke(Driver());
   int count=game.Journal.State.ReadCount("rec-plate-standard-hub");
   Click("read");yield return ObserveStroke(Driver());
   Assert.AreEqual(count,game.Journal.State.ReadCount("rec-plate-standard-hub"),"Preserved-copy reading still animates without consuming an original");
   game.Undo();yield return null;Assert.IsFalse(Driver().StrokePlaying,"Undoing a read cannot present another read");
   game.Redo();yield return null;Assert.IsFalse(Driver().StrokePlaying,"Replaying a journal read cannot present deliberate input");
   Click("pick-start");yield return null;Assert.IsFalse(Driver().StrokePlaying);
   Click("phase-next");yield return null;Assert.IsFalse(Driver().StrokePlaying,"Pagination is not reading");
   Click("phase-prev");yield return null;Assert.IsFalse(Driver().StrokePlaying);
   game.Back();Click("pick-end");yield return null;Assert.IsFalse(Driver().StrokePlaying,"Choosing a window end is not reading");
   game.Back();Click("start-next");yield return null;Assert.IsFalse(Driver().StrokePlaying);
   game.Undo();yield return null;Assert.IsFalse(Driver().StrokePlaying);
   game.Redo();yield return null;Assert.IsFalse(Driver().StrokePlaying);
  }
  [UnityTest] public IEnumerator OriginalReadRespondsOnlyAfterDurableSuccess(){
   RequireReader();yield return Boot();PrepareReader();Click("read");yield return ObserveStroke(Driver());yield return Wait(game.FlushSaves());
   var release=new TaskCompletionSource<bool>();
   game.Store.Injection=(stage,token)=>stage==SaveStage.BeforeRename?release.Task:Task.CompletedTask;
   var failed=game.CommitAsync(new PuzzleCommand("ReadOriginal"));
   yield return null;yield return null;Assert.IsTrue(game.SavePending);Assert.IsFalse(Driver().StrokePlaying,"Pending storage cannot claim a read");
   release.SetException(new IOException("reader motion regression"));
   yield return Wait(failed);Assert.IsFalse(failed.Result);Assert.IsFalse(Driver().StrokePlaying,"Failed storage cannot claim a read");
   Assert.AreEqual(0,game.Journal.State.ReadCount("rec-plate-standard-hub"));
   game.Store.Injection=null;game.Back();
   var succeeded=game.CommitAsync(new PuzzleCommand("ReadOriginal"));yield return Wait(succeeded);
   Assert.IsTrue(succeeded.Result);Assert.AreEqual(1,game.Journal.State.ReadCount("rec-plate-standard-hub"));
   yield return ObserveStroke(Driver());
  }
  [UnityTest] public IEnumerator ReducedMotionSwitchCancelsReadImmediatelyAndDoesNotReplay(){
   RequireReader();yield return Boot();PrepareReader();var driver=Driver();var rest=driver.CrankPivot.localRotation;
   Click("read");yield return null;yield return null;Assert.Greater(driver.CrankAngleDeg,0);
   var state=game.Journal.State;game.OpenOverlay("settings");Click("reduced-motion");
   Assert.IsFalse(driver.StrokePlaying,"The setting takes effect in the same UI action, not after a stroke completes");
   Assert.Less(Quaternion.Angle(rest,driver.CrankPivot.localRotation),.01f);Assert.AreSame(state,game.Journal.State);
   game.Back();Click("read");yield return null;Assert.IsFalse(driver.StrokePlaying);Assert.Less(Quaternion.Angle(rest,driver.CrankPivot.localRotation),.01f);
   game.OpenOverlay("settings");Click("reduced-motion");game.Back();yield return null;Assert.IsFalse(driver.StrokePlaying,"Disabling reduced motion cannot replay a cancelled read");
   Click("read");yield return ObserveStroke(driver);
  }
  [UnityTest] public IEnumerator AuthoredRestAndBorrowedLightsSurviveCancellationAndDisable(){
   host=new GameObject("Authored crank fixture");var pivot=new GameObject("Crank").transform;pivot.SetParent(host.transform,false);
   var rest=Quaternion.Euler(17,28,39);pivot.localRotation=rest;bool reduced=false;
   var light=new GameObject("Borrowed light",typeof(Light)).GetComponent<Light>();light.transform.SetParent(host.transform);light.enabled=false;
   var driver=host.AddComponent<M7ReaderStageVisual>();
   driver.Initialize(()=>true,()=>reduced,pivot,JObject.Parse(Resources.Load<TextAsset>("T0ReaderVfx").text),new[]{light});
   driver.PlayStroke();float deadline=Time.realtimeSinceStartup+1;
   while(driver.StrokePlaying&&Quaternion.Angle(rest,pivot.localRotation)<.1f&&Time.realtimeSinceStartup<deadline)yield return null;
   Assert.GreaterOrEqual(Quaternion.Angle(rest,pivot.localRotation),.1f);
   reduced=true;driver.RefreshPresentation();Assert.IsFalse(driver.StrokePlaying);Assert.Less(Quaternion.Angle(rest,pivot.localRotation),.01f);
   reduced=false;driver.PlayStroke();yield return null;host.SetActive(false);
   Assert.IsFalse(driver.StrokePlaying);Assert.Less(Quaternion.Angle(rest,pivot.localRotation),.01f);Assert.IsTrue(light.enabled,"Disable must return borrowed lights synchronously");
   host.SetActive(true);yield return null;Assert.IsFalse(driver.StrokePlaying,"No orphan animation may resume on enable");Assert.Less(Quaternion.Angle(rest,pivot.localRotation),.01f);
  }
  // RFC-CX-016: the committed "Watch room lamp" is a Light with no Renderer, so ApplyStagePresentation's
  // renderer-based root hiding left it lighting the reader stage and blew out 5.5-5.7% of the viewport to pure
  // white. The stage now borrows exclusivity and must hand it back: every hub light is disabled while the stage
  // is shown and enabled again on exit, so the hub is lit exactly as it was before the visit.
  [UnityTest] public IEnumerator GateOnSuppressesForeignHubLightsAndRestoresThemOnExit(){
   if(profile==null)Assert.Ignore("M7ReaderStage.asset not imported — run Tools/M7/Import reader stage candidates first");
   if(profile.reader==null)Assert.Ignore("M7ReaderStage.asset reader prefab missing — rerun Tools/M7/Import reader stage candidates");
   profile.runtimeApproved=true;
   yield return Boot();
   var hubLights=HubLights();
   Assert.IsNotEmpty(hubLights,"The committed hub must ship at least one light for this contract to mean anything");
   var enabledBefore=hubLights.Where(l=>l.enabled).ToArray();
   Assert.IsNotEmpty(enabledBefore,"At least one committed hub light must be enabled before the stage is entered");
   OpenReader();yield return null;yield return null;
   var visual=FindVisual();Assert.IsNotNull(visual,"Gate on must build the M7 optical reader under the session");
   foreach(var light in hubLights)Assert.IsFalse(light.enabled,light.name+" must be suppressed while the reader stage owns the frame");
   // The stage's own two lights are untouched by the suppression sweep.
   Assert.AreEqual(2,visual.GetComponentsInChildren<Light>(true).Count(l=>l.enabled),"Both stage lights stay enabled");
   game.Back();
   foreach(var light in enabledBefore)Assert.IsTrue(light.enabled,light.name+" must be returned before another stage enters");
   OpenReader();
   foreach(var light in enabledBefore)Assert.IsFalse(light.enabled,light.name+" must be borrowed by same-frame re-entry");
   yield return null;yield return null;
   foreach(var light in enabledBefore)Assert.IsFalse(light.enabled,"Destroying the old stage cannot restore a new owner's light");
   Assert.IsFalse(Driver().StrokePlaying,"Re-entry is not reading");
   game.Back();yield return null;yield return null;
   Assert.IsNull(FindVisual(),"Leaving the reader must tear down the M7 optical reader");
   foreach(var light in enabledBefore)Assert.IsTrue(light.enabled,light.name+" must be lit after the final exit");
  }
 }
 // Sample after hand placement; a normal UnityTest coroutine resumes before LateUpdate.
 [DefaultExecutionOrder(1000)]
 public sealed class M22GripProbe:MonoBehaviour {
  public M22EmbodimentVisual Hands;public M7ReaderStageVisual Reader;public Vector3 Origin;
  public float MaxError,Travel;public int Samples;
  void LateUpdate(){
   if(Hands==null||Reader==null||!Reader.StrokePlaying)return;
   Samples++;MaxError=Mathf.Max(MaxError,Vector3.Distance(Hands.RightContactPosition,Hands.HandleContactPosition));
   Travel=Mathf.Max(Travel,Vector3.Distance(Origin,Hands.RightContactPosition));
  }
 }
}
#endif
