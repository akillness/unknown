#if TIDE_TEST_FRAMEWORK
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Tide.App;
using Tide.Presentation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
namespace Tide.Tests {
 // RFC-CX-013 ReaderStage: gate off = hub untouched, no stage; gate on = "M7 optical reader" root with exactly two lights, torn down on Back; reduced motion = crank stays at rest.
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
  static void AssertColor(Color expected,Color actual,string message){
   Assert.AreEqual(expected.r,actual.r,.001f,message+" (r)");Assert.AreEqual(expected.g,actual.g,.001f,message+" (g)");
   Assert.AreEqual(expected.b,actual.b,.001f,message+" (b)");Assert.AreEqual(expected.a,actual.a,.001f,message+" (a)");
  }
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
   var lamp=lights.Single(l=>l.name=="M7 reader lamp");var fill=lights.Single(l=>l.name=="M7 reader fill");
   Assert.AreEqual(LightType.Point,lamp.type);Assert.AreEqual(LightShadows.None,lamp.shadows);AssertColor(profile.lampColor,lamp.color,"lamp colour");
   Assert.AreEqual(profile.lampIntensity,lamp.intensity,.001f);Assert.AreEqual(profile.lampRange,lamp.range,.001f);Assert.Less(Vector3.Distance(profile.lampOffset,lamp.transform.localPosition),.0001f,"lamp sits at profile.lampOffset");
   Assert.AreEqual(LightType.Point,fill.type);Assert.AreEqual(LightShadows.None,fill.shadows);AssertColor(profile.fillColor,fill.color,"fill colour");
   Assert.AreEqual(profile.fillIntensity,fill.intensity,.001f);Assert.AreEqual(profile.fillRange,fill.range,.001f);Assert.Less(Vector3.Distance(profile.fillOffset,fill.transform.localPosition),.0001f,"fill sits at profile.fillOffset");
   Assert.IsTrue(visual.GetComponentsInChildren<Renderer>(true).Length>0,"Instantiated reader must carry renderers");
   AssertColor(profile.background,camera.backgroundColor,"Reader stage background");
   Assert.AreEqual(CameraClearFlags.SolidColor,camera.clearFlags);
   Assert.Less(Vector3.Distance(profile.cameraPosition,camera.transform.position),.001f,"Camera is posed once at profile.cameraPosition");
   foreach(var root in roots)Assert.IsFalse(root.activeSelf,root.name+" must be hidden while the reader stage is shown");
   // Leaving the tool the way every other test does: Back() closes the reader, the stage returns to the hub and the visual is torn down.
   game.Back();yield return null;yield return null;yield return null;
   Assert.AreEqual("shell",game.Surface);
   Assert.IsNull(FindVisual(),"Leaving the reader must tear down the M7 optical reader");
   Assert.IsFalse(game.M7ReaderStageActive,"Leaving the reader must return to the hub stage");
   Assert.AreEqual(0,host.GetComponentsInChildren<Light>(true).Length,"No reader lights may survive the exit");
   foreach(var root in roots)Assert.IsTrue(root.activeSelf,root.name+" must be re-shown after leaving the reader");
  }
  [UnityTest] public IEnumerator GateOnReducedMotionKeepsCrankAtRest(){
   if(profile==null)Assert.Ignore("M7ReaderStage.asset not imported — run Tools/M7/Import reader stage candidates first");
   if(profile.reader==null)Assert.Ignore("M7ReaderStage.asset reader prefab missing — rerun Tools/M7/Import reader stage candidates");
   profile.runtimeApproved=true;
   yield return Boot();
   game.OpenOverlay("settings");Click("reduced-motion");game.Back();
   Assert.AreEqual("shell",game.Surface);
   OpenReader();yield return null;
   var visual=FindVisual();Assert.IsNotNull(visual,"Gate on must build the M7 optical reader under the session");
   var pivot=visual.GetComponentsInChildren<Transform>(true).FirstOrDefault(t=>t.name==profile.crankPivotName);
   if(pivot==null)Assert.Ignore("Reader prefab has no '"+profile.crankPivotName+"' — rerun Tools/M7/Import reader stage candidates");
   for(int frame=0;frame<5;frame++){
    Assert.Less(Quaternion.Angle(Quaternion.identity,pivot.localRotation),.01f,"Reduced motion must keep the crank at rest (frame "+frame+")");
    yield return null;
   }
   Assert.IsTrue(game.M7ReaderStageActive,"Reader stage stays shown while the crank rests");
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
   var driver=visual.GetComponent<M7ReaderStageVisual>();
   Assert.IsNotNull(driver,"The stage visual carries the restore hook");
   CollectionAssert.AreEquivalent(enabledBefore,driver.SuppressedHubLights.ToArray(),"Exactly the previously enabled hub lights are the ones borrowed");
   // The stage's own two lights are untouched by the suppression sweep.
   Assert.AreEqual(2,visual.GetComponentsInChildren<Light>(true).Count(l=>l.enabled),"Both stage lights stay enabled");
   game.Back();yield return null;yield return null;yield return null;
   Assert.AreEqual("shell",game.Surface);
   Assert.IsNull(FindVisual(),"Leaving the reader must tear down the M7 optical reader");
   foreach(var light in enabledBefore)Assert.IsTrue(light.enabled,light.name+" must be lit again after leaving the reader stage");
  }
 }
}
#endif
