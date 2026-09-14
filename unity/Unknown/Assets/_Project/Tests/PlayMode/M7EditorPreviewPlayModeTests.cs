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
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;
namespace Tide.Tests {
 // RFC-CX-013 Editor verification switch (App/M7EditorPreview.cs + Editor/M7EditorPreviewMenu.cs).
 //
 // The switch widened all three M7 gates to `runtimeApproved || diagnosticOverride || M7EditorPreview
 // || --m7-<lane>-diagnostic`. Two things have to stay true afterwards:
 //  1. It cannot reach a headless run. `M7EditorPreview` is `!Application.isBatchMode && EditorPrefs...`,
 //     so a machine whose EditorPref is set must still measure the committed look in CI — otherwise every
 //     GateOff contract in M7Hub/M7UiSkin/M7ReaderPlayModeTests would silently start passing on the wrong look.
 //  2. `diagnosticOverride` — the [NonSerialized] field the switch drives — opens one lane and only that
 //     lane, and never promotes `runtimeApproved`. Promotion stays a director audit (Tools/M7/Approve …).
 //
 // Every test here drives the gate through `diagnosticOverride` alone — never `runtimeApproved`, and never
 // the Editor pref, which is read but never written so the machine's own preference survives the run. SetUp
 // forces all three approvals to false so "no approval" is an established precondition rather than an
 // assumption; TearDown restores both fields from the snapshot and asserts nothing, so a failing body cannot
 // leak the session host, the additively loaded hub scene, RenderSettings or the temp save directory.
 public sealed class M7EditorPreviewPlayModeTests {
  const string Paper="M7 paper",Frame="M7 frame";
  string directory;GameObject host;T0GameSession game;Scene hubScene;
  M7HubProfile hubProfile;M7UiSkinProfile uiProfile;M7ReaderStageProfile readerProfile;
  bool hubApproved,hubOverride,uiApproved,uiOverride,readerApproved,readerOverride;
  AmbientMode originalAmbientMode;Color originalAmbientLight;
  static bool HubDiagnostic=>Environment.GetCommandLineArgs().Contains("--m7-hub-diagnostic");
  static bool UiDiagnostic=>Environment.GetCommandLineArgs().Contains("--m7-ui-diagnostic");
  static bool ReaderDiagnostic=>Environment.GetCommandLineArgs().Contains("--m7-reader-diagnostic");
  [UnitySetUp] public IEnumerator Setup(){
   Assert.IsFalse(SceneManager.GetSceneByName("hub").isLoaded,"Fixture must load the committed hub itself");
   yield return SceneManager.LoadSceneAsync("hub",LoadSceneMode.Additive);
   hubScene=SceneManager.GetSceneByName("hub");
   Assert.IsNotNull(Camera.main,"The committed hub must provide the MainCamera the reader stage is posed on");
   directory=Path.Combine(Path.GetTempPath(),"m7-editor-preview-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(directory);
   hubProfile=Resources.Load<M7HubProfile>("M7Hub");
   uiProfile=Resources.Load<M7UiSkinProfile>("M7UiSkin");
   readerProfile=Resources.Load<M7ReaderStageProfile>("M7ReaderStage");
   // Snapshot both fields, then clear approval on every lane. Clearing it here rather than per body is what
   // keeps the gate-on tests honest on a machine whose assets are already approved: without it the gate would
   // open for the wrong reason (approval, not override) and their "runtimeApproved still false" assertion
   // would fail spuriously. TearDown puts both fields back from these snapshots.
   if(hubProfile!=null){hubApproved=hubProfile.runtimeApproved;hubOverride=hubProfile.diagnosticOverride;hubProfile.runtimeApproved=false;}
   if(uiProfile!=null){uiApproved=uiProfile.runtimeApproved;uiOverride=uiProfile.diagnosticOverride;uiProfile.runtimeApproved=false;}
   if(readerProfile!=null){readerApproved=readerProfile.runtimeApproved;readerOverride=readerProfile.diagnosticOverride;readerProfile.runtimeApproved=false;}
   originalAmbientMode=RenderSettings.ambientMode;originalAmbientLight=RenderSettings.ambientLight;
  }
  [UnityTearDown] public IEnumerator TearDown(){
   // Purely restorative, and never asserts: a throw here would skip the host/scene/directory cleanup below
   // and cascade into the next test's SetUp. `diagnosticOverride` is [NonSerialized], so only this restore
   // clears it; `runtimeApproved` returns to the snapshot SetUp took before it forced every lane unapproved.
   // "The preview path never promotes runtimeApproved" is asserted per lane inside the three gate-on bodies.
   if(hubProfile!=null){hubProfile.diagnosticOverride=hubOverride;hubProfile.runtimeApproved=hubApproved;}
   if(uiProfile!=null){uiProfile.diagnosticOverride=uiOverride;uiProfile.runtimeApproved=uiApproved;}
   if(readerProfile!=null){readerProfile.diagnosticOverride=readerOverride;readerProfile.runtimeApproved=readerApproved;}
   if(game!=null)yield return Wait(game.FlushSaves());
   if(host!=null)UnityEngine.Object.Destroy(host);
   RenderSettings.ambientMode=originalAmbientMode;RenderSettings.ambientLight=originalAmbientLight;
   yield return null;
   if(hubScene.IsValid()&&hubScene.isLoaded)yield return SceneManager.UnloadSceneAsync(hubScene);
   if(directory!=null&&Directory.Exists(directory))Directory.Delete(directory,true);
  }
  void Create(){host=new GameObject("M7 editor preview test");game=host.AddComponent<T0GameSession>();game.Initialize(Resources.Load<T0RuntimeConfig>("T0Runtime"),directory);}
  IEnumerator Wait(Task task){while(!task.IsCompleted)yield return null;if(task.IsFaulted)throw task.Exception;}
  void Click(string id){Assert.IsTrue(game.Interface.Activate(id),"Missing UI action "+id);}
  void AssertBooted()=>Assert.Contains("start",game.Interface.ActionIds.ToArray(),"Session must boot to the start screen");
  void RequireHub(){
   if(hubProfile==null)Assert.Ignore("M7Hub.asset not imported — run Tools/M7/Import all M7 candidates first");
   if(hubProfile.floorAndWall==null||hubProfile.workbench==null||hubProfile.plateShelf==null)Assert.Ignore("M7Hub.asset materials incomplete — rerun Tools/M7/Import all M7 candidates");
  }
  void RequireUi(){
   if(uiProfile==null)Assert.Ignore("M7UiSkin.asset not imported — run Tools/M7/Import all M7 candidates first");
   if(uiProfile.paperPanel==null||uiProfile.bronzeFrame==null)Assert.Ignore("M7UiSkin.asset textures incomplete — rerun Tools/M7/Import all M7 candidates");
  }
  void RequireReader(){
   if(readerProfile==null)Assert.Ignore("M7ReaderStage.asset not imported — run Tools/M7/Import all M7 candidates first");
   if(readerProfile.reader==null)Assert.Ignore("M7ReaderStage.asset reader prefab missing — rerun Tools/M7/Import all M7 candidates");
  }
  GameObject HubRoot(string name)=>hubScene.GetRootGameObjects().Single(x=>x.name==name);
  // Same route M7ReaderPlayModeTests takes to the reader tool: start the run, hub node, then open-reader.
  IEnumerator Boot(){Create();yield return null;game.StartGame();yield return null;Assert.IsFalse(game.PatrolActive,"Fresh save must not start inside C1");}
  void OpenReader(){Click("hub-view-reader");Click("open-reader");Assert.AreEqual("reader",game.Surface,"The reader tool must open; only its 3D stage is gated");}
  Transform ReaderVisual()=>game.transform.Cast<Transform>().FirstOrDefault(t=>t.name==T0GameSession.M7ReaderVisualName);
  // Presence is measured on the live screen only (Render() deactivates the previous root before its deferred
  // Destroy), but absence is measured across inactive roots too, so a dying screen cannot hide a backing.
  RawImage[] Backings(string name)=>host.GetComponentsInChildren<RawImage>().Where(r=>r.name==name).ToArray();
  void AssertNoSkinBackings(string where){
   Assert.IsFalse(host.GetComponentsInChildren<Transform>(true).Any(t=>t.name==Paper),"A closed UI skin gate must not insert "+Paper+" on the "+where+" screen");
   Assert.IsFalse(host.GetComponentsInChildren<Transform>(true).Any(t=>t.name==Frame),"A closed UI skin gate must not insert "+Frame+" on the "+where+" screen");
  }
  void AssertSwapped(string root,Material expected,string prefix){
   var renderers=HubRoot(root).GetComponentsInChildren<Renderer>(true);Assert.IsNotEmpty(renderers,root);
   foreach(var renderer in renderers){Assert.AreSame(expected,renderer.sharedMaterial,root);StringAssert.StartsWith(prefix,renderer.sharedMaterial.name,root);}
  }
  // Contract 1: the switch is Editor-only and `Application.isBatchMode` forces it closed, so a headless run
  // with no approval and no override still boots the committed pre-M7 look on all three lanes.
  [UnityTest] public IEnumerator BatchModeGuardKeepsEveryLaneClosedWithoutApprovalOrOverride(){
   RequireHub();RequireUi();RequireReader();
   if(HubDiagnostic||UiDiagnostic||ReaderDiagnostic)Assert.Ignore("--m7-*-diagnostic forces a lane on; the closed-gate contract is not observable in this run");
   // The guard itself. Without `!Application.isBatchMode` this fails on any machine whose Tools/M7/Editor
   // preview pref is set, which is exactly the regression that would quietly void every GateOff test.
   Assert.IsFalse(Application.isBatchMode&&T0GameSession.M7EditorPreview,"Application.isBatchMode must force T0GameSession.M7EditorPreview false");
   bool preview=T0GameSession.M7EditorPreview;
   if(preview)Assert.Ignore("Tools/M7/Editor preview is on in this interactive Editor session and opens all three lanes; the closed-gate contract is only observable headless");
   hubProfile.diagnosticOverride=false;uiProfile.diagnosticOverride=false;readerProfile.diagnosticOverride=false;
   Create();yield return null;AssertBooted();
   Assert.AreEqual(0,T0GameSession.M7HubLampCount(hubScene),"A closed hub gate must not add the M7 hub lamp");
   Assert.AreEqual(originalAmbientMode,RenderSettings.ambientMode,"A closed hub gate must not force flat ambient");
   Assert.AreEqual(originalAmbientLight,RenderSettings.ambientLight,"A closed hub gate must not recolour the ambient light");
   AssertNoSkinBackings("start");
   game.StartGame();yield return null;
   Assert.IsFalse(game.PatrolActive,"Fresh save must not start inside C1");
   OpenReader();yield return null;yield return null;
   Assert.IsNull(ReaderVisual(),"A closed reader gate must not build the "+T0GameSession.M7ReaderVisualName);
   Assert.IsFalse(game.M7ReaderStageActive,"A closed reader gate must keep the hub stage");
   AssertNoSkinBackings("reader");
   Assert.AreEqual(0,T0GameSession.M7HubLampCount(hubScene),"Reaching the reader must not open the hub lane either");
  }
  // Contract 2: the hub lane opens on diagnosticOverride alone — the in-memory field the Editor switch drives.
  [UnityTest] public IEnumerator HubDiagnosticOverrideOpensTheHubLaneWithoutApproval(){
   RequireHub();
   hubProfile.diagnosticOverride=true;
   Create();yield return null;AssertBooted();
   foreach(var name in hubProfile.floorAndWallRoots)AssertSwapped(name,hubProfile.floorAndWall,"MAT_M7_Hub_SaltConcrete");
   foreach(var name in hubProfile.workbenchRoots)AssertSwapped(name,hubProfile.workbench,"MAT_M7_Hub_PerforatedSteel");
   foreach(var name in hubProfile.plateShelfRoots)AssertSwapped(name,hubProfile.plateShelf,"MAT_M7_Hub_Bronze");
   Assert.AreEqual(1,T0GameSession.M7HubLampCount(hubScene),"Exactly one M7 hub lamp root");
   var lamp=HubRoot(T0GameSession.M7HubLampName).GetComponentsInChildren<Light>(true);
   Assert.AreEqual(1,lamp.Length,"Exactly one light under the M7 hub lamp");
   Assert.AreEqual(LightType.Point,lamp[0].type);Assert.AreEqual(LightShadows.None,lamp[0].shadows);
   Assert.AreEqual(hubProfile.lampColor,lamp[0].color);Assert.AreEqual(hubProfile.lampIntensity,lamp[0].intensity);Assert.AreEqual(hubProfile.lampRange,lamp[0].range);
   Assert.AreEqual(hubProfile.lampLocalPosition,lamp[0].transform.localPosition);
   Assert.AreEqual(AmbientMode.Flat,RenderSettings.ambientMode);Assert.AreEqual(hubProfile.ambient,RenderSettings.ambientLight);
   Assert.IsFalse(hubProfile.runtimeApproved,"Previewing the hub candidate must not promote M7Hub.runtimeApproved");
  }
  // Contract 3: the UI skin lane opens on diagnosticOverride alone.
  [UnityTest] public IEnumerator UiSkinDiagnosticOverrideOpensTheUiLaneWithoutApproval(){
   RequireUi();
   uiProfile.diagnosticOverride=true;
   Create();yield return null;AssertBooted();
   var paper=Backings(Paper);Assert.AreEqual(1,paper.Length,"Exactly one "+Paper+" backing");
   Assert.IsFalse(paper[0].raycastTarget,Paper+" must not intercept clicks");
   Assert.AreEqual(0,paper[0].transform.GetSiblingIndex(),Paper+" must be the first child");
   Assert.AreEqual("Work Surface",paper[0].transform.parent.name,Paper+" parent");
   Assert.AreEqual(2,Backings(Frame).Length,"Exactly two "+Frame+" backings");
   Assert.IsFalse(uiProfile.runtimeApproved,"Previewing the UI skin candidate must not promote M7UiSkin.runtimeApproved");
  }
  // Contract 4: the reader lane opens on diagnosticOverride alone, and leaving the tool still tears the stage down.
  [UnityTest] public IEnumerator ReaderDiagnosticOverrideOpensTheReaderLaneWithoutApproval(){
   RequireReader();
   readerProfile.diagnosticOverride=true;
   yield return Boot();
   OpenReader();yield return null;yield return null;
   var visual=ReaderVisual();Assert.IsNotNull(visual,"The override must build the "+T0GameSession.M7ReaderVisualName);
   Assert.AreEqual(1,game.transform.Cast<Transform>().Count(t=>t.name==T0GameSession.M7ReaderVisualName),"Exactly one "+T0GameSession.M7ReaderVisualName+" root");
   Assert.AreEqual(2,visual.GetComponentsInChildren<Light>(true).Length,"Exactly two lights under the "+T0GameSession.M7ReaderVisualName);
   Assert.IsTrue(game.M7ReaderStageActive,"The override must show the reader stage");
   game.Back();yield return null;yield return null;yield return null;
   Assert.AreEqual("shell",game.Surface);
   Assert.IsNull(ReaderVisual(),"Leaving the reader must tear down the "+T0GameSession.M7ReaderVisualName);
   Assert.IsFalse(game.M7ReaderStageActive,"Leaving the reader must return to the hub stage");
   Assert.AreEqual(0,host.GetComponentsInChildren<Light>(true).Length,"No reader lights may survive the exit");
   Assert.IsFalse(readerProfile.runtimeApproved,"Previewing the reader candidate must not promote M7ReaderStage.runtimeApproved");
  }
  // Contract 5: the three gates are separate. One lane's override must not open the other two — the Editor
  // switch opens all three by design, but the per-lane field it writes may only reach its own gate.
  [UnityTest] public IEnumerator OneLaneOverrideLeavesTheOtherTwoGatesClosed(){
   RequireHub();RequireUi();RequireReader();
   if(UiDiagnostic||ReaderDiagnostic)Assert.Ignore("--m7-ui-diagnostic or --m7-reader-diagnostic forces another lane on; lane independence is not observable in this run");
   bool preview=T0GameSession.M7EditorPreview;
   if(preview)Assert.Ignore("Tools/M7/Editor preview opens all three lanes at once; lane independence is only observable headless");
   hubProfile.diagnosticOverride=true;uiProfile.diagnosticOverride=false;readerProfile.diagnosticOverride=false;
   Create();yield return null;AssertBooted();
   Assert.AreEqual(1,T0GameSession.M7HubLampCount(hubScene),"The hub override must open its own lane");
   AssertNoSkinBackings("start");
   game.StartGame();yield return null;
   Assert.IsFalse(game.PatrolActive,"Fresh save must not start inside C1");
   OpenReader();yield return null;yield return null;
   Assert.IsNull(ReaderVisual(),"The hub override must not open the reader lane");
   Assert.IsFalse(game.M7ReaderStageActive,"The hub override must not enter the reader stage");
   AssertNoSkinBackings("reader");
   Assert.AreEqual(1,T0GameSession.M7HubLampCount(hubScene),"The hub lane stays open across the visit to the reader tool");
  }
 }
}
#endif
