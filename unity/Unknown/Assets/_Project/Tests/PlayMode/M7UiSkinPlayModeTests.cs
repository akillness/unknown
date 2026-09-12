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
using UnityEngine.TestTools;
using UnityEngine.UI;
namespace Tide.Tests {
 // RFC-CX-013 UiSkin: gate off = committed literals, no backings; gate on = one "M7 paper" + two "M7 frame" non-raycast RawImages that never displace committed children.
 public sealed class M7UiSkinPlayModeTests {
  static readonly Color HeaderLiteral=new Color(.05f,.12f,.15f,.94f),NavigationLiteral=new Color(.08f,.17f,.2f,.92f),ToolbarLiteral=new Color(.05f,.12f,.15f,.96f),WorkSurfaceLiteral=new Color(.91f,.9f,.82f,.97f);
  string directory;GameObject host;T0GameSession game;M7UiSkinProfile profile;bool originalApproved;
  static bool Diagnostic=>Environment.GetCommandLineArgs().Contains("--m7-ui-diagnostic");
  [UnitySetUp] public IEnumerator Setup(){
   directory=Path.Combine(Path.GetTempPath(),"m7-ui-skin-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(directory);
   profile=Resources.Load<M7UiSkinProfile>("M7UiSkin");if(profile!=null)originalApproved=profile.runtimeApproved;
   yield return null;
  }
  void Create(){host=new GameObject("M7 UI skin test");game=host.AddComponent<T0GameSession>();game.Initialize(Resources.Load<T0RuntimeConfig>("T0Runtime"),directory);}
  IEnumerator Wait(Task task){while(!task.IsCompleted)yield return null;if(task.IsFaulted)throw task.Exception;}
  [UnityTearDown] public IEnumerator TearDown(){
   if(profile!=null)profile.runtimeApproved=originalApproved;
   if(game!=null)yield return Wait(game.FlushSaves());
   if(host!=null)UnityEngine.Object.Destroy(host);
   yield return null;
   if(directory!=null&&Directory.Exists(directory))Directory.Delete(directory,true);
  }
  void Click(string id){Assert.IsTrue(game.Interface.Activate(id),"Missing UI action "+id);}
  void AssertBooted()=>Assert.Contains("start",game.Interface.ActionIds.ToArray(),"Session must boot to the start screen");
  // Live screen only: Render() deactivates the previous root before its deferred Destroy, so inactive copies are never counted.
  RectTransform[] Live()=>host.GetComponentsInChildren<RectTransform>();
  RectTransform Panel(string name)=>Live().Single(r=>r.name==name);
  RawImage[] Backings(string name)=>host.GetComponentsInChildren<RawImage>().Where(r=>r.name==name).ToArray();
  static void AssertColor(Color expected,Color actual,string message){
   Assert.That(actual.r,Is.EqualTo(expected.r).Within(1e-3f),message+" r");Assert.That(actual.g,Is.EqualTo(expected.g).Within(1e-3f),message+" g");
   Assert.That(actual.b,Is.EqualTo(expected.b).Within(1e-3f),message+" b");Assert.That(actual.a,Is.EqualTo(expected.a).Within(1e-3f),message+" a");
  }
  static void AssertFillsPanel(RectTransform backing,RectTransform panel){
   var b=new Vector3[4];var p=new Vector3[4];backing.GetWorldCorners(b);panel.GetWorldCorners(p);
   for(int i=0;i<4;i++)Assert.That(Vector3.Distance(b[i],p[i]),Is.LessThan(.01f),backing.name+" corner "+i+" must coincide with "+panel.name);
   Assert.IsNull(panel.GetComponent<LayoutGroup>(),panel.name+" has no layout group, so a first child cannot displace committed children");
  }
  void AssertNoBackings(){
   Assert.IsFalse(host.GetComponentsInChildren<Transform>(true).Any(t=>t.name=="M7 paper"),"Gate off must not insert M7 paper");
   Assert.IsFalse(host.GetComponentsInChildren<Transform>(true).Any(t=>t.name=="M7 frame"),"Gate off must not insert M7 frame");
  }
  // Shared gate-on contract: 1 paper under Work Surface, 2 frames under Header + Toolbar, all first sibling, none raycastable, committed first children pushed to index 1.
  void AssertSkinApplied(){
   var paper=Backings("M7 paper");Assert.AreEqual(1,paper.Length,"Exactly one M7 paper backing");
   Assert.IsFalse(paper[0].raycastTarget,"M7 paper must not intercept clicks");Assert.AreEqual(0,paper[0].transform.GetSiblingIndex(),"M7 paper must be the first child");
   Assert.AreEqual("Work Surface",paper[0].transform.parent.name);Assert.AreSame(profile.paperPanel,paper[0].texture);
   Assert.AreEqual(profile.workSurfaceTint,paper[0].color);Assert.AreEqual(profile.paperTilesAcross,paper[0].uvRect.width,1e-4f,"paper tiles across");Assert.Greater(paper[0].uvRect.height,0,"paper tiles down");
   AssertFillsPanel((RectTransform)paper[0].transform,Panel("Work Surface"));
   Assert.AreEqual("Viewport",Panel("Work Surface").GetChild(1).name,"Committed Work Surface children keep their order behind the backing");
   var frames=Backings("M7 frame");Assert.AreEqual(2,frames.Length,"Exactly two M7 frame backings");
   CollectionAssert.AreEquivalent(new[]{"Header","Toolbar"},frames.Select(f=>f.transform.parent.name).ToArray(),"M7 frame parents");
   foreach(var frame in frames){
    Assert.IsFalse(frame.raycastTarget,"M7 frame must not intercept clicks");Assert.AreEqual(0,frame.transform.GetSiblingIndex(),frame.transform.parent.name+" frame must be the first child");
    Assert.AreSame(profile.bronzeFrame,frame.texture);Assert.AreEqual(profile.frameTilesAcross,frame.uvRect.width,1e-4f,frame.transform.parent.name+" frame tiles across");
    AssertFillsPanel((RectTransform)frame.transform,(RectTransform)frame.transform.parent);
   }
   Assert.AreEqual("Title",Panel("Header").GetChild(1).name);Assert.AreEqual("Tools",Panel("Toolbar").GetChild(1).name);
   Assert.AreEqual(profile.header,Panel("Header").GetComponent<Image>().color,"Header uses profile.header when the gate is on");
   Assert.AreEqual(profile.navigation,Panel("Navigation").GetComponent<Image>().color);Assert.AreEqual(profile.toolbar,Panel("Toolbar").GetComponent<Image>().color);
   foreach(var button in host.GetComponentsInChildren<Button>())Assert.IsTrue(button.GetComponent<Image>().raycastTarget,button.name+" button must stay raycastable");
  }
  [UnityTest] public IEnumerator GateOffKeepsCommittedLiteralsAndNoSkinBackings(){
   if(Diagnostic)Assert.Ignore("--m7-ui-diagnostic forces the gate on; the gate-off contract is not observable in this run");
   if(profile!=null)profile.runtimeApproved=false;
   Create();yield return null;AssertBooted();
   AssertNoBackings();
   AssertColor(HeaderLiteral,Panel("Header").GetComponent<Image>().color,"Header keeps its committed literal");
   AssertColor(NavigationLiteral,Panel("Navigation").GetComponent<Image>().color,"Navigation keeps its committed literal");
   AssertColor(ToolbarLiteral,Panel("Toolbar").GetComponent<Image>().color,"Toolbar keeps its committed literal");
   AssertColor(WorkSurfaceLiteral,Panel("Work Surface").GetComponent<Image>().color,"Work Surface keeps its committed literal");
   Assert.AreEqual("Viewport",Panel("Work Surface").GetChild(0).name);Assert.AreEqual("Title",Panel("Header").GetChild(0).name);Assert.AreEqual("Tools",Panel("Toolbar").GetChild(0).name);
   // A second explicit Render keeps the committed look; no backing may appear on re-render either.
   game.Render();yield return null;
   AssertNoBackings();AssertColor(HeaderLiteral,Panel("Header").GetComponent<Image>().color,"Header keeps its committed literal after re-render");
  }
  [UnityTest] public IEnumerator GateOnInsertsNonRaycastBackingsAndKeepsButtonsClickable(){
   if(profile==null)Assert.Ignore("M7UiSkin.asset not imported — run Tools/M7/Import UI skin candidates first");
   if(profile.paperPanel==null||profile.bronzeFrame==null)Assert.Ignore("M7UiSkin.asset textures incomplete — rerun Tools/M7/Import UI skin candidates");
   profile.runtimeApproved=true;
   Create();yield return null;AssertBooted();
   AssertSkinApplied();
   // Buttons behind the backings still activate: the start screen's settings action opens the overlay and re-renders.
   var before=game.Interface.ActionIds.ToArray();int renders=0;game.Interface.ScreenChanged+=()=>renders++;
   Click("settings");yield return null;
   Assert.AreEqual("settings",game.Surface,"Activating settings through the skinned screen must open the overlay");
   Assert.AreEqual(1,renders,"Activation re-renders the screen once");
   CollectionAssert.AreNotEquivalent(before,game.Interface.ActionIds.ToArray(),"Overlay actions replace the start actions");
   Assert.Contains("text-scale",game.Interface.ActionIds.ToArray());
   AssertSkinApplied();
   game.Back();yield return null;AssertBooted();AssertSkinApplied();
  }
  // Anchor-driven committed rects on the start screen; layout-group children are excluded because they settle over later frames.
  static readonly string[] CommittedRects={"Screen","Header","Title","Subtitle","Scene viewport","Navigation","Navigation Viewport","Case thread","Work Surface","Viewport","Toolbar","Tools","Footer"};
  Dictionary<string,Vector3[]> Layout(){
   var live=Live();var result=new Dictionary<string,Vector3[]>();
   foreach(var name in CommittedRects){var rect=live.Single(r=>r.name==name);var corners=new Vector3[4];rect.GetWorldCorners(corners);result[name]=corners;}
   return result;
  }
  [UnityTest] public IEnumerator GateOnAtTextScale150RendersWithoutLayoutChange(){
   if(Diagnostic)Assert.Ignore("--m7-ui-diagnostic forces the gate on; the gate-off baseline is not observable in this run");
   if(profile==null)Assert.Ignore("M7UiSkin.asset not imported — run Tools/M7/Import UI skin candidates first");
   if(profile.paperPanel==null||profile.bronzeFrame==null)Assert.Ignore("M7UiSkin.asset textures incomplete — rerun Tools/M7/Import UI skin candidates");
   // Baseline: committed UI at text scale 1.5 (five text-scale steps, persisted to settings.json by SaveSettings).
   profile.runtimeApproved=false;
   Create();yield return null;AssertBooted();
   Click("settings");for(int i=0;i<5;i++)Click("text-scale");game.Back();yield return null;yield return null;
   Assert.AreEqual(1.5f,game.Interface.TextScale,"Five text-scale steps reach 1.5");AssertBooted();AssertNoBackings();
   var baseline=Layout();
   yield return Wait(game.FlushSaves());UnityEngine.Object.Destroy(host);host=null;game=null;yield return null;
   // Gate on: the second session boots straight at 1.5 from settings.json; the skin must not move any committed rect.
   profile.runtimeApproved=true;
   Create();yield return null;yield return null;AssertBooted();
   Assert.AreEqual(1.5f,game.Interface.TextScale,"Persisted text scale survives the restart");
   AssertSkinApplied();
   var skinned=Layout();
   foreach(var pair in baseline)for(int i=0;i<4;i++)Assert.That(Vector3.Distance(pair.Value[i],skinned[pair.Key][i]),Is.LessThan(.01f),pair.Key+" corner "+i+" must not move when the skin is applied at text scale 1.5");
   // An explicit re-render at 1.5 keeps the contract and throws nothing.
   game.Render();yield return null;
   Assert.AreEqual(1.5f,game.Interface.TextScale);AssertSkinApplied();
   foreach(var pair in Layout())for(int i=0;i<4;i++)Assert.That(Vector3.Distance(baseline[pair.Key][i],pair.Value[i]),Is.LessThan(.01f),pair.Key+" corner "+i+" must not move on re-render");
  }
 }
}
#endif
