#if TIDE_TEST_FRAMEWORK
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Tide.App;
using Tide.Presentation;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
namespace Tide.Tests {
 // RFC-CX-013 HubShell: gate off = committed hub byte-identical; gate on = listed roots swap, one lamp, flat ambient.
 public sealed class M7HubPlayModeTests {
  static readonly string[] CommittedRoots={"Authored room floor","Authored back wall","Authored plate shelf","Workbench left jamb","Workbench right jamb","Workbench above drawer","Workbench below drawer","Workbench behind drawer"};
  string directory;GameObject host;T0GameSession game;Scene hub;M7HubProfile profile;bool originalApproved;
  AmbientMode originalAmbientMode;Color originalAmbientLight;
  static bool Diagnostic=>Environment.GetCommandLineArgs().Contains("--m7-hub-diagnostic");
  [UnitySetUp] public IEnumerator Setup(){
   Assert.IsFalse(SceneManager.GetSceneByName("hub").isLoaded,"Fixture must load the committed hub itself");
   yield return SceneManager.LoadSceneAsync("hub",LoadSceneMode.Additive);
   hub=SceneManager.GetSceneByName("hub");
   directory=Path.Combine(Path.GetTempPath(),"m7-hub-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(directory);
   profile=Resources.Load<M7HubProfile>("M7Hub");if(profile!=null)originalApproved=profile.runtimeApproved;
   originalAmbientMode=RenderSettings.ambientMode;originalAmbientLight=RenderSettings.ambientLight;
  }
  void Create(){host=new GameObject("M7 hub test");game=host.AddComponent<T0GameSession>();game.Initialize(Resources.Load<T0RuntimeConfig>("T0Runtime"),directory);}
  IEnumerator Wait(Task task){while(!task.IsCompleted)yield return null;if(task.IsFaulted)throw task.Exception;}
  [UnityTearDown] public IEnumerator TearDown(){
   if(profile!=null)profile.runtimeApproved=originalApproved;
   if(game!=null)yield return Wait(game.FlushSaves());
   if(host!=null)UnityEngine.Object.Destroy(host);
   RenderSettings.ambientMode=originalAmbientMode;RenderSettings.ambientLight=originalAmbientLight;
   yield return null;
   if(hub.IsValid()&&hub.isLoaded)yield return SceneManager.UnloadSceneAsync(hub);
   if(directory!=null&&Directory.Exists(directory))Directory.Delete(directory,true);
  }
  GameObject Root(string name)=>hub.GetRootGameObjects().Single(x=>x.name==name);
  Dictionary<Renderer,Material[]> Snapshot()=>hub.GetRootGameObjects().SelectMany(r=>r.GetComponentsInChildren<Renderer>(true)).ToDictionary(r=>r,r=>r.sharedMaterials.ToArray());
  int HubLights()=>hub.GetRootGameObjects().Sum(r=>r.GetComponentsInChildren<Light>(true).Length);
  void AssertBooted()=>Assert.Contains("start",game.Interface.ActionIds.ToArray(),"Session must boot to the start screen");
  void AssertSwapped(string root,Material expected,string prefix){
   var renderers=Root(root).GetComponentsInChildren<Renderer>(true);Assert.IsNotEmpty(renderers,root);
   foreach(var renderer in renderers){Assert.AreSame(expected,renderer.sharedMaterial,root);StringAssert.StartsWith(prefix,renderer.sharedMaterial.name,root);}
  }
  void AssertShellApplied(){
   foreach(var name in profile.floorAndWallRoots)AssertSwapped(name,profile.floorAndWall,"MAT_M7_Hub_SaltConcrete");
   foreach(var name in profile.workbenchRoots)AssertSwapped(name,profile.workbench,"MAT_M7_Hub_PerforatedSteel");
   foreach(var name in profile.plateShelfRoots)AssertSwapped(name,profile.plateShelf,"MAT_M7_Hub_Bronze");
  }
  [UnityTest] public IEnumerator GateOffLeavesCommittedHubShellUntouched(){
   if(Diagnostic)Assert.Ignore("--m7-hub-diagnostic forces the gate on; the gate-off contract is not observable in this run");
   if(profile!=null)profile.runtimeApproved=false;
   foreach(var name in CommittedRoots)Assert.IsNotNull(Root(name),name);
   var before=Snapshot();int roots=hub.rootCount;int lights=HubLights();
   Create();yield return null;AssertBooted();
   var after=Snapshot();
   CollectionAssert.AreEquivalent(before.Keys,after.Keys,"Gate off must not add or remove hub renderers");
   foreach(var pair in before)CollectionAssert.AreEqual(pair.Value,after[pair.Key],pair.Key.name+" must keep its committed sharedMaterials");
   foreach(var name in CommittedRoots)StringAssert.DoesNotStartWith("MAT_M7_Hub_",Root(name).GetComponent<Renderer>().sharedMaterial.name,name);
   Assert.AreEqual(0,T0GameSession.M7HubLampCount(hub),"Gate off must not add the M7 hub lamp");
   Assert.AreEqual(roots,hub.rootCount,"Gate off must not add hub roots");Assert.AreEqual(lights,HubLights(),"Gate off must not add lights");
   Assert.AreEqual(originalAmbientMode,RenderSettings.ambientMode);Assert.AreEqual(originalAmbientLight,RenderSettings.ambientLight);
  }
  [UnityTest] public IEnumerator GateOnSwapsListedRootsAddsOneLampAndFlatAmbientOnce(){
   if(profile==null)Assert.Ignore("M7Hub.asset not imported — run Tools/M7/Import hub shell candidates first");
   if(profile.floorAndWall==null||profile.workbench==null||profile.plateShelf==null)Assert.Ignore("M7Hub.asset materials incomplete — rerun Tools/M7/Import hub shell candidates");
   profile.runtimeApproved=true;
   var listed=new HashSet<string>(profile.floorAndWallRoots.Concat(profile.workbenchRoots).Concat(profile.plateShelfRoots));
   var before=Snapshot();int lights=HubLights();
   Create();yield return null;AssertBooted();
   AssertShellApplied();
   foreach(var pair in before)if(!listed.Contains(pair.Key.transform.root.name))CollectionAssert.AreEqual(pair.Value,pair.Key.sharedMaterials,pair.Key.transform.root.name+"/"+pair.Key.name+" is not listed and must keep its committed sharedMaterials");
   foreach(var renderer in Root("T0 approved r03 drawer").GetComponentsInChildren<Renderer>(true))CollectionAssert.AreEqual(before[renderer],renderer.sharedMaterials,"Approved r03 drawer keeps its committed materials");
   Assert.AreEqual(1,T0GameSession.M7HubLampCount(hub),"Exactly one M7 hub lamp root");
   var lamp=Root(T0GameSession.M7HubLampName).GetComponentsInChildren<Light>(true);
   Assert.AreEqual(1,lamp.Length,"Exactly one light under the M7 hub lamp");Assert.AreEqual(lights+1,HubLights(),"Gate on adds exactly one light to the hub");
   Assert.AreEqual(LightType.Point,lamp[0].type);Assert.AreEqual(LightShadows.None,lamp[0].shadows);
   Assert.AreEqual(profile.lampColor,lamp[0].color);Assert.AreEqual(profile.lampIntensity,lamp[0].intensity);Assert.AreEqual(profile.lampRange,lamp[0].range);
   Assert.AreEqual(profile.lampLocalPosition,lamp[0].transform.localPosition);
   Assert.AreEqual(AmbientMode.Flat,RenderSettings.ambientMode);Assert.AreEqual(profile.ambient,RenderSettings.ambientLight);
   // A second session boot against the already-dressed hub must be idempotent: same materials, still one lamp.
   yield return Wait(game.FlushSaves());UnityEngine.Object.Destroy(host);host=null;game=null;yield return null;
   Create();yield return null;AssertBooted();
   AssertShellApplied();
   Assert.AreEqual(1,T0GameSession.M7HubLampCount(hub),"Second bind must not duplicate the M7 hub lamp");
   Assert.AreEqual(1,Root(T0GameSession.M7HubLampName).GetComponentsInChildren<Light>(true).Length);Assert.AreEqual(lights+1,HubLights());
  }
 }
}
#endif
