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

namespace Tide.Tests
{
 // M20 — the archival work-surface candidate (GTI r02) as a gated diagnostic backing for the
 // T0 "Work Surface" panel only. Contract: _workspace/current/presentation/t0-work-surface-m20.md
 //
 // Both halves boot the same committed completed-C1 fixture the C1/M5/M8/M18/M19 suites already use, so
 // the screen is genuinely reachable and carries live buttons plus all three M19 text roles. No
 // GameScreen is hand-built, no journal state is written and no source text is asserted: every
 // assertion reads real Unity component state (RawImage.texture/uvRect, Graphic.raycastTarget,
 // RectTransform anchors/rect, sibling index, Image.color, Text.fontSize/fontStyle, Selectable.colors).
 //
 // Exercise the diagnostic off/on states in memory, restoring the original profile after each test.
 // No AssetDatabase save or provenance write: r02 remains diagnostic-only.
 public sealed class T0WorkSurfacePlayModeTests
 {
  // Observed committed order on the completed-C1 patrol surface (C1GameSession.PatrolScreen).
  static readonly string[] ExpectedActionOrder={"continue-c1-signature","c1-review","c1-interview-prep"};
  const string Helper="c1-interview-prep";
  const string M20Backing="M20 work surface";
  const string M7Backing="M7 paper";

  M20WorkSurfaceProfile profile;bool originalOverride,originalApproved;
  readonly List<string> directories=new List<string>();
  readonly List<GameObject> hosts=new List<GameObject>();
  readonly List<T0GameSession> sessions=new List<T0GameSession>();

  static bool CommandLineDiagnostic=>Environment.GetCommandLineArgs().Contains("--m20-ui-surface-diagnostic");

  [UnitySetUp] public IEnumerator SetUp()
  {
   profile=Resources.Load<M20WorkSurfaceProfile>("M20WorkSurface");
   if(profile!=null){originalApproved=profile.runtimeApproved;originalOverride=profile.diagnosticOverride;}
   yield return null;
  }
  [UnityTearDown] public IEnumerator TearDown()
  {
   if(profile!=null)
   {
    profile.diagnosticOverride=originalOverride;
    profile.runtimeApproved=originalApproved;
   }
   foreach(var session in sessions) if(session!=null) yield return Wait(session.FlushSaves());
   foreach(var host in hosts) if(host!=null) UnityEngine.Object.Destroy(host);
   yield return null;
   foreach(var directory in directories) if(Directory.Exists(directory)) Directory.Delete(directory,true);
  }
  IEnumerator Wait(Task task){while(!task.IsCompleted)yield return null;if(task.IsFaulted)throw task.Exception;}

  // Each half gets its own save root holding a fresh copy of the committed fixture, so the two
  // sessions start from byte-identical state and neither can observe the other's writes.
  IEnumerator Boot(System.Action<GameObject,T0GameSession> ready)
  {
   var directory=Path.Combine(Path.GetTempPath(),"t0-work-surface-"+Guid.NewGuid().ToString("N"));
   Directory.CreateDirectory(directory);directories.Add(directory);
   File.Copy(Path.Combine(Application.dataPath,"_Project/Tests/Fixtures/C1PatrolCompletedV2.json"),Path.Combine(directory,"save.json"));
   var host=new GameObject("T0 work surface session");hosts.Add(host);
   var game=host.AddComponent<T0GameSession>();sessions.Add(game);
   game.Initialize(Resources.Load<T0RuntimeConfig>("T0Runtime"),directory);
   yield return null;game.StartGame();yield return null;
   ready(host,game);
  }

  // Live screen only: Render() deactivates the previous root before its deferred Destroy and
  // GetComponentsInChildren<T>() skips inactive objects, so stale copies are never counted.
  static RectTransform Panel(GameObject host,string name)
  {
   var found=host.GetComponentsInChildren<RectTransform>().Where(r=>r.name==name).ToArray();
   Assert.AreEqual(1,found.Length,"exactly one live '"+name+"' panel");
   return found[0];
  }
  static RawImage[] Backings(RectTransform panel,string name)=>
   panel.GetComponentsInChildren<RawImage>().Where(x=>x.name==name).ToArray();
  static Button[] Buttons(GameObject host)=>host.GetComponentsInChildren<Button>();
  static Button Btn(GameObject host,string id)=>Buttons(host).Single(b=>b.name==id);
  static Text Label(GameObject host,string id)=>Btn(host,id).GetComponentsInChildren<Text>().Single(t=>t.name=="Label");
  static string[] Ids(T0GameSession game)=>game.Interface.ActionIds.ToArray();
  static Dictionary<string,string> LabelSnapshot(GameObject host,T0GameSession game)=>
   Ids(game).ToDictionary(id=>id,id=>Label(host,id).text);

  static float Channel(float v)=>v<=.04045f?v/12.92f:Mathf.Pow((v+.055f)/1.055f,2.4f);
  static float Lum(Color c)=>.2126f*Channel(c.r)+.7152f*Channel(c.g)+.0722f*Channel(c.b);

  // A ViewAction.Detail is emitted by FlowText as the sibling immediately before its own button.
  static Text HelperOf(GameObject host,string id)
  {
   var button=Btn(host,id).transform;int index=button.GetSiblingIndex();
   if(index==0)return null;
   var previous=button.parent.GetChild(index-1);
   return previous.GetComponent<Button>()!=null?null:previous.GetComponent<Text>();
  }
  static Text Body(GameObject host)
  {
   var content=Panel(host,"Content");
   for(int i=0;i<content.childCount;i++){var t=content.GetChild(i).GetComponent<Text>();if(t!=null&&t.name=="Text")return t;}
   return null;
  }

  // The M19 surface that must survive M20 untouched, gate off AND gate on: instrument plates with a
  // brass bevel + enabled-only rule, focus inversion instead of a darkening multiply, and the three
  // text tiers (body > label > helper, label alone bold, helper toned toward its background).
  static void AssertM19SurfaceIntact(GameObject host,T0GameSession game,string where)
  {
   foreach(var button in Buttons(host))
   {
    var root=button.GetComponent<Image>();
    Assert.IsTrue(root.raycastTarget,where+": "+button.name+" root Image must stay raycastable");
    var bevel=button.transform.Find("Bevel");
    Assert.IsNotNull(bevel,where+": "+button.name+" must keep its M19 'Bevel'");
    Assert.IsFalse(bevel.GetComponent<Image>().raycastTarget,where+": "+button.name+"/Bevel must not intercept clicks");
    var rule=button.transform.Find("Rule");
    Assert.IsNotNull(rule,where+": "+button.name+" must keep its M19 'Rule'");
    Assert.AreEqual(button.interactable,rule.gameObject.activeSelf,where+": "+button.name+"/Rule visible exactly when enabled");
    Assert.AreEqual(1,button.GetComponentsInChildren<Text>().Count(t=>t.name=="Label"),where+": "+button.name+" keeps exactly one Label");
    // T2: r02 is a work-surface background, never a button texture.
    Assert.AreEqual(0,button.GetComponentsInChildren<RawImage>().Length,
     where+": "+button.name+" must carry no RawImage — r02 must never become a button texture");
   }
   var focused=ExpectedActionOrder[0];
   Assert.IsTrue(game.Interface.Focus(focused),where+": "+focused+" must stay focusable");
   var colors=Btn(host,focused).colors;
   Assert.AreEqual(Color.white,colors.selectedColor,where+": selectedColor must not multiply the plate darker");
   Assert.AreEqual(0f,colors.fadeDuration,1e-4f,where+": focus must read immediately");
   Assert.Less(Lum(Label(host,focused).color),Lum(Btn(host,focused).GetComponent<Image>().color),
    where+": the focused plate must stay dark-on-light");
   foreach(var other in Ids(game).Where(id=>id!=focused))
    Assert.Greater(Lum(Label(host,other).color),Lum(Btn(host,other).GetComponent<Image>().color),
     where+": unfocused plate "+other+" must stay light-on-dark");
   var body=Body(host);var label=Label(host,Helper);var helper=HelperOf(host,Helper);
   Assert.IsNotNull(body,where+": the body paragraph must render");
   Assert.IsNotNull(helper,where+": the contextual helper must render above its action");
   Assert.Greater(body.fontSize,label.fontSize,where+": body outranks the action label");
   Assert.Greater(label.fontSize,helper.fontSize,where+": the action label outranks the helper");
   Assert.AreEqual(FontStyle.Bold,label.fontStyle,where+": only the action label is bold");
   Assert.AreEqual(FontStyle.Normal,helper.fontStyle,where+": the helper stays normal weight");
   Assert.AreNotEqual(body.color,helper.color,where+": the helper must be toned away from body ink");
  }

  // Header and Toolbar keep their two tiled M7 bronze frames in both halves: M20 touches one panel.
  static void AssertM7FramesUntouched(GameObject host,string where)
  {
   foreach(var name in new[]{"Header","Toolbar"})
   {
    var frames=Backings(Panel(host,name),"M7 frame");
    Assert.AreEqual(1,frames.Length,where+": "+name+" keeps exactly one tiled M7 frame backing");
    Assert.IsFalse(frames[0].raycastTarget,where+": "+name+" frame must not intercept clicks");
   }
  }

  [UnityTest] public IEnumerator M20GateOffKeepsTheM19WorkSurfaceAndGateOnAppliesR02OnceAsANonTiledFullBleedBacking()
  {
   Assert.IsNotNull(profile,"M20WorkSurface.asset missing — run Tools/M20/Import work surface candidate");
   Assert.IsNotNull(profile.workSurface,"M20WorkSurface.asset has no imported candidate texture");
   if(CommandLineDiagnostic)
    Assert.Ignore("--m20-ui-surface-diagnostic forces the gate on for this run; the gate-off baseline is not observable");

   // ---------- gate OFF: the committed M19 work surface, byte-for-byte behaviour ----------
   profile.runtimeApproved=false;profile.diagnosticOverride=false;
   GameObject offHost=null;T0GameSession offGame=null;
   yield return Boot((h,g)=>{offHost=h;offGame=g;});

   Assert.IsTrue(offGame.PatrolActive,"fixture must be inside C1");
   Assert.IsTrue(offGame.PatrolComplete,"fixture must be the COMPLETED C1 patrol surface");
   Assert.AreEqual(ExpectedActionOrder,Ids(offGame),"committed action order");

   var offSurface=Panel(offHost,"Work Surface");
   var offPlateColor=offSurface.GetComponent<Image>().color;
   var offViewport=Panel(offHost,"Viewport");
   var offViewportRect=offViewport.rect;
   var offViewportIndex=offViewport.GetSiblingIndex();
   var offSurfaceChildCount=offSurface.childCount;
   var offLabels=LabelSnapshot(offHost,offGame);
   var offEnabled=Ids(offGame).ToDictionary(id=>id,id=>Btn(offHost,id).interactable);
   var offFocusable=Ids(offGame).Count(id=>offGame.Interface.Focus(id));

   Assert.AreEqual(0,Backings(offSurface,M20Backing).Length,
    "gate off: the Work Surface must carry no M20 backing at all");
   var offPaper=Backings(offSurface,M7Backing);
   Assert.AreEqual(1,offPaper.Length,"gate off: the committed tiled M7 paper backing must still be the work surface");
   Assert.Greater(offPaper[0].uvRect.width,1f,"gate off: the committed M7 backing is tiled (uvRect wider than one tile)");
   AssertM7FramesUntouched(offHost,"gate off");
   AssertM19SurfaceIntact(offHost,offGame,"gate off");

   // ---------- gate ON: r02 replaces that one backing, and nothing else moves ----------
   profile.diagnosticOverride=true;
   GameObject onHost=null;T0GameSession onGame=null;
   yield return Boot((h,g)=>{onHost=h;onGame=g;});

   Assert.IsTrue(onGame.PatrolComplete,"gate on: the same completed C1 surface must be reached");
   var onSurface=Panel(onHost,"Work Surface");

   var applied=Backings(onSurface,M20Backing);
   Assert.AreEqual(1,applied.Length,
    "gate on: the Work Surface must carry exactly one '"+M20Backing+"' backing so the r02 candidate is applied once");
   var backing=applied[0];
   Assert.AreSame(profile.workSurface,backing.texture,"gate on: the backing must draw the imported r02 candidate");
   Assert.AreEqual(1672,backing.texture.width,"gate on: r02 keeps its authored width");
   Assert.AreEqual(941,backing.texture.height,"gate on: r02 keeps its authored height");
   Assert.AreEqual(TextureWrapMode.Clamp,backing.texture.wrapMode,
    "gate on: the candidate is imported Clamp so repeating it is impossible at the engine level");

   // T3 — the non-tile rule, the whole point of a separate lane: default UVs, never TiledBacking's.
   Assert.AreEqual(new UnityEngine.Rect(0,0,1,1),backing.uvRect,
    "gate on: the backing must use untiled full-texture UVs, not TiledBacking's repeated uvRect");
   // T1 — one full-bleed layer covering the panel.
   Assert.AreEqual(Vector2.zero,((RectTransform)backing.transform).anchorMin,"gate on: full-bleed anchorMin");
   Assert.AreEqual(Vector2.one,((RectTransform)backing.transform).anchorMax,"gate on: full-bleed anchorMax");
   Assert.AreEqual(Vector2.zero,((RectTransform)backing.transform).offsetMin,"gate on: full-bleed offsetMin");
   Assert.AreEqual(Vector2.zero,((RectTransform)backing.transform).offsetMax,"gate on: full-bleed offsetMax");
   // H1/H2 — behind every later child, and never in the input path.
   Assert.AreEqual(0,backing.transform.GetSiblingIndex(),"gate on: the backing must be the first child so text and buttons draw over it");
   Assert.IsFalse(backing.raycastTarget,"gate on: the backing must not intercept clicks");
   // T5 — a single work-surface background, not r02 stacked on top of the tiled M7 paper.
   Assert.AreEqual(0,Backings(onSurface,M7Backing).Length,
    "gate on: the tiled M7 paper backing must be replaced, not stacked under r02");
   // H3/H4 — the panel's own plate colour is unchanged, and because M20 REPLACES the committed backing
   // rather than adding one, the Work Surface child count and the Viewport's index are identical to the
   // gate-off surface. (Observed: the gate-off baseline already carries the approved M7 paper backing, so
   // "pushed one index" would mean r02 was stacked on top of it — exactly what T5 forbids.)
   Assert.AreEqual(offPlateColor,onSurface.GetComponent<Image>().color,"gate on: the Work Surface plate colour is unchanged");
   Assert.AreEqual(offSurfaceChildCount,onSurface.childCount,"gate on: r02 replaces the committed backing, so the child count cannot grow");
   var onViewport=Panel(onHost,"Viewport");
   Assert.AreEqual(offViewportIndex,onViewport.GetSiblingIndex(),"gate on: the committed Viewport keeps its sibling index");
   Assert.AreEqual(offViewportRect,onViewport.rect,"gate on: the Viewport rect must not move or resize");
   AssertM7FramesUntouched(onHost,"gate on");

   // N2/N3/N4 — actions, labels and availability are identical to the gate-off surface.
   Assert.AreEqual(ExpectedActionOrder,Ids(onGame),"gate on: action order unchanged");
   Assert.AreEqual(offLabels,LabelSnapshot(onHost,onGame),"gate on: every action label unchanged");
   foreach(var pair in offEnabled)
    Assert.AreEqual(pair.Value,Btn(onHost,pair.Key).interactable,"gate on: "+pair.Key+" availability unchanged");
   Assert.AreEqual(offFocusable,Ids(onGame).Count(id=>onGame.Interface.Focus(id)),"gate on: the focus ring size is unchanged");
   AssertM19SurfaceIntact(onHost,onGame,"gate on");

   // N6 — a click still reaches a button through the backing, and the gate survives the re-render.
   Assert.IsTrue(onGame.Interface.Focus("c1-review"),"gate on: c1-review must be focusable");
   Assert.AreEqual("c1-review",onGame.Interface.CurrentFocusId,"gate on: focus must land on the clicked action");
   Assert.IsTrue(onGame.Interface.Activate("c1-review"),"gate on: activating through the backing must work");
   yield return null;
   Assert.AreNotEqual(ExpectedActionOrder,Ids(onGame),"gate on: the click must actually change the screen");
   var afterClick=Backings(Panel(onHost,"Work Surface"),M20Backing);
   Assert.AreEqual(1,afterClick.Length,"gate on: the backing stays applied exactly once after a re-render");
   Assert.IsFalse(afterClick[0].raycastTarget,"gate on: the re-rendered backing still must not intercept clicks");

   // N7 — presentation only: no simulation write, no pending save.
   Assert.IsFalse(onGame.SavePending,"gate on: a background candidate must not dirty the save");
  }
 }
}
#endif
