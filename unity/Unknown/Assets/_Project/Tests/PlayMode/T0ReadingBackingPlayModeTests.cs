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
using Tide.UI;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tide.Tests
{
 // M21 — the one defect the M20 native capture exposed: with the M20 diagnostic gate ON the committed
 // dark `ink` body/instruction text sits on the dark r02 archival surface, so the measured glyph:ground
 // ratio collapsed to 1.11:1 (M19 baseline 3.53:1, m20-ui-surface/verification.md §0). This suite fixes
 // the READING GROUND, never the copy or the text colour.
 //
 // Same fixture and same discipline as the M20 suite: the committed completed-C1 patrol surface, booted
 // twice into two private save roots, no mock GameScreen, no source-text assertion. Every assertion
 // reads real Unity component state (Image.color/raycastTarget, LayoutElement.ignoreLayout,
 // RectTransform world corners + sibling index, Text.color/fontSize/fontStyle, Selectable.colors,
 // RawImage.uvRect/texture) and the contrast numbers are computed from those component colours.
 //
 // The gate is driven ONLY through the M20 profile's [NonSerialized] diagnosticOverride. runtimeApproved
 // is never written — M21 is a readability correction inside the diagnostic gate, not a promotion.
 public sealed class T0ReadingBackingPlayModeTests
 {
  static readonly string[] ExpectedActionOrder={"continue-c1-signature","c1-review","c1-interview-prep"};
  const string Helper="c1-interview-prep";
  const string Band="M21 reading";      // the correction under test
  const string M20Backing="M20 work surface";
  const string M7Backing="M7 paper";

  // Flat-arithmetic acceptance. NOT an accessibility standard (see §"limits" in the M21 receipt):
  // these are sRGB formula values over component colours, blind to glyph anti-aliasing, Korean stroke
  // weight and display gamma. They only have to show the collapse is materially undone.
  const float MinBodyRatio=4.0f;        // committed body ink over the band, worst-case ground
  const float MinBaselineFraction=.55f; // each backed role vs its own gate-off ground
  const float MaxBandAlpha=.85f;        // above this the band stops being a low-noise strip

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
    Assert.AreEqual(originalApproved,profile.runtimeApproved,"M21 must never write runtimeApproved");
   }
   foreach(var session in sessions) if(session!=null) yield return Wait(session.FlushSaves());
   foreach(var host in hosts) if(host!=null) UnityEngine.Object.Destroy(host);
   yield return null;
   foreach(var directory in directories) if(Directory.Exists(directory)) Directory.Delete(directory,true);
  }
  IEnumerator Wait(Task task){while(!task.IsCompleted)yield return null;if(task.IsFaulted)throw task.Exception;}

  IEnumerator Boot(System.Action<GameObject,T0GameSession> ready)
  {
   var directory=Path.Combine(Path.GetTempPath(),"t0-reading-backing-"+Guid.NewGuid().ToString("N"));
   Directory.CreateDirectory(directory);directories.Add(directory);
   File.Copy(Path.Combine(Application.dataPath,"_Project/Tests/Fixtures/C1PatrolCompletedV2.json"),Path.Combine(directory,"save.json"));
   var host=new GameObject("T0 reading backing session");hosts.Add(host);
   var game=host.AddComponent<T0GameSession>();sessions.Add(game);
   game.Initialize(Resources.Load<T0RuntimeConfig>("T0Runtime"),directory);
   yield return null;game.StartGame();yield return null;
   // The band mirrors its paragraph's resolved rect in LateUpdate, and WrappedButtonHeight reflows the
   // column over the following frames, so settle before measuring geometry.
   yield return null;yield return null;
   ready(host,game);
  }

  static RectTransform Live(GameObject host,string name)
  {
   var found=host.GetComponentsInChildren<RectTransform>().Where(r=>r.name==name).ToArray();
   Assert.AreEqual(1,found.Length,"exactly one live '"+name+"'");
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
  static float Ratio(Color a,Color b)
  {
   float x=Lum(a),y=Lum(b);float hi=Mathf.Max(x,y),lo=Mathf.Min(x,y);
   return (hi+.05f)/(lo+.05f);
  }
  // A translucent ground is only as bright as what shows through it. Compositing over BLACK is the
  // darkest ground any surface can present, so the ratio computed this way is a LOWER BOUND on the
  // real ratio over r02 — it can be measured from component colours alone, without reading the
  // compressed candidate texture back off the GPU.
  static Color OverBlack(Color c)=>new Color(c.r*c.a,c.g*c.a,c.b*c.a,1);

  // Every paragraph FlowText draws straight onto the work surface: a direct child of "Content" named
  // "Text". Buttons (own ink plate) and direction sections (own panel) are deliberately excluded.
  static List<Text> SurfaceParagraphs(GameObject host)
  {
   var content=Live(host,"Content");var found=new List<Text>();
   for(int i=0;i<content.childCount;i++)
   {
    var child=content.GetChild(i);if(child.name!="Text")continue;
    var text=child.GetComponent<Text>();if(text!=null)found.Add(text);
   }
   return found;
  }
  static Image[] Bands(GameObject host)=>
   Live(host,"Content").GetComponentsInChildren<Image>(true).Where(x=>x.name==Band).ToArray();
  static Image BandOf(Text paragraph)
  {
   var t=paragraph.transform;int index=t.GetSiblingIndex();
   if(index==0)return null;
   var previous=t.parent.GetChild(index-1);
   return previous.name==Band?previous.GetComponent<Image>():null;
  }
  static Rect WorldRect(RectTransform r)
  {
   var c=new Vector3[4];r.GetWorldCorners(c);
   return Rect.MinMaxRect(c[0].x,c[0].y,c[2].x,c[2].y);
  }
  static Text HelperOf(GameObject host,string id)
  {
   var button=Btn(host,id).transform;int index=button.GetSiblingIndex();
   if(index==0)return null;
   var previous=button.parent.GetChild(index-1);
   return previous.GetComponent<Button>()!=null?null:previous.GetComponent<Text>();
  }
  static Text Body(GameObject host)=>SurfaceParagraphs(host).FirstOrDefault();

  // The M19 plate contract, which this correction must leave completely alone in both halves.
  static void AssertM19PlatesIntact(GameObject host,T0GameSession game,string where)
  {
   foreach(var button in Buttons(host))
   {
    Assert.IsTrue(button.GetComponent<Image>().raycastTarget,where+": "+button.name+" root Image must stay raycastable");
    var bevel=button.transform.Find("Bevel");
    Assert.IsNotNull(bevel,where+": "+button.name+" must keep its M19 'Bevel'");
    Assert.IsFalse(bevel.GetComponent<Image>().raycastTarget,where+": "+button.name+"/Bevel must not intercept clicks");
    var rule=button.transform.Find("Rule");
    Assert.IsNotNull(rule,where+": "+button.name+" must keep its M19 'Rule'");
    Assert.AreEqual(button.interactable,rule.gameObject.activeSelf,where+": "+button.name+"/Rule visible exactly when enabled");
    Assert.AreEqual(1,button.GetComponentsInChildren<Text>().Count(t=>t.name=="Label"),where+": "+button.name+" keeps exactly one Label");
    Assert.AreEqual(0,button.GetComponentsInChildren<RawImage>().Length,where+": "+button.name+" must carry no RawImage");
    // The correction is a reading ground for the surface paragraphs. A button already owns an opaque
    // plate, so a band inside one would be a button restyle — exactly what this milestone must not do.
    Assert.AreEqual(0,button.GetComponentsInChildren<Image>(true).Count(x=>x.name==Band),
     where+": "+button.name+" must carry no reading band — M19 plates are untouched");
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
  }

  [UnityTest] public IEnumerator PracticeCorrespondenceAt150PercentKeepsItsReadingGroundUnderTheDiagnosticSurface()
  {
   Assert.IsNotNull(profile,"M20WorkSurface.asset missing");
   Assert.IsNotNull(profile.workSurface,"M20 diagnostic surface must have its imported candidate");
   Assert.IsFalse(profile.runtimeApproved,"the diagnostic candidate must remain unapproved");
   if(CommandLineDiagnostic)
    Assert.Ignore("--m20-ui-surface-diagnostic forces the gate on; the gate-off ink baseline is not observable");

   Color baselineInk=default;float baselineRatio=0;
   foreach(bool diagnostic in new[]{false,true})
   {
    profile.diagnosticOverride=diagnostic;
    GameObject host=null;T0GameSession game=null;
    yield return Boot((h,g)=>{host=h;game=g;});
    Assert.IsTrue(game.PatrolComplete,"use the real completed-C1 fixture");
    Assert.IsTrue(game.Interface.Activate("settings"));
    for(int i=0;i<5;i++)Assert.IsTrue(game.Interface.Activate("text-scale"));
    Assert.IsTrue(game.Interface.Activate("alignment-practice"));
    Assert.IsTrue(game.AlignmentPracticeActive);
    Assert.AreEqual(1.5f,game.Interface.TextScale);
    for(int i=0;i<3;i++)
    {
     Assert.IsTrue(game.Interface.Activate("practice-slot-"+i));
     Assert.IsTrue(game.Interface.Activate("practice-a-"+i));
     Assert.IsTrue(game.Interface.Activate("practice-b-"+i));
    }
    yield return null;yield return null;yield return null;
    Canvas.ForceUpdateCanvases();

    // Select the actual readout by content, independently of the paragraph identity used by M21.
    var paragraph=host.GetComponentsInChildren<Text>().Single(t=>t.text.StartsWith("대응 1: A",StringComparison.Ordinal));
    Assert.Greater(paragraph.preferredHeight,paragraph.fontSize,"exercise the multiline correspondence readout");
    var surface=Live(host,"Work Surface");
   var tracks=host.GetComponentsInChildren<AlignmentPracticeChart>().Single();
   var trackMesh=tracks.canvasRenderer.GetMesh();
   Assert.IsNotNull(trackMesh,"exercise the actual rendered track mesh");
    var plotRect=tracks.GetPixelAdjustedRect();
    foreach(var vertex in trackMesh.vertices)
    {
     Assert.GreaterOrEqual(vertex.x,plotRect.xMin-.01f,"keep the complete first peak inside the chart");
     Assert.LessOrEqual(vertex.x,plotRect.xMax+.01f,"keep the complete last peak inside the chart");
    }
    if(!diagnostic)
    {
     baselineInk=Body(host).color;
     Assert.AreEqual(baselineInk,paragraph.color,"correspondence uses the committed body ink");
     baselineRatio=Ratio(paragraph.color,OverBlack(surface.GetComponent<Image>().color));
    Assert.AreEqual(baselineInk,tracks.color,"paired tracks use the committed body ink");
     Assert.IsNull(BandOf(paragraph),"gate off must preserve the unbacked default");
     continue;
    }

    Assert.AreEqual(1,Backings(surface,M20Backing).Length,"the actual M20 diagnostic ground must be present");
    Assert.AreSame(profile.workSurface,Backings(surface,M20Backing)[0].texture);
    Assert.AreEqual(baselineInk,Body(host).color,"body ink must not change to repair contrast");
    Assert.AreEqual(baselineInk,paragraph.color,"correspondence ink must not change to repair contrast");
    var band=BandOf(paragraph);
    Assert.IsNotNull(band,"the practice correspondence paragraph must have a reading ground behind its glyphs");
    Assert.IsFalse(band.raycastTarget,"the reading ground must not intercept practice input");
    Assert.IsTrue(band.GetComponent<LayoutElement>().ignoreLayout,"the ground must not displace the readout");
    Assert.Greater(band.color.a,0f);
    Assert.LessOrEqual(band.color.a,MaxBandAlpha,"preserve the translucent reading strip");
    var bandRect=WorldRect(band.rectTransform);
    var textRect=WorldRect(paragraph.rectTransform);
    Assert.LessOrEqual(bandRect.xMin,textRect.xMin+.01f,"cover the resolved left edge");
    Assert.GreaterOrEqual(bandRect.xMax,textRect.xMax-.01f,"cover the resolved right edge");
    Assert.GreaterOrEqual(bandRect.yMax,textRect.yMax-.01f,"cover the resolved top edge");
    Assert.LessOrEqual(bandRect.yMin,textRect.yMin+.01f,"cover the resolved bottom edge");
    Assert.GreaterOrEqual(band.rectTransform.rect.height,
     Mathf.Max(paragraph.rectTransform.rect.height,paragraph.preferredHeight)-.01f,
     "cover every flowed line, including glyphs below the assigned layout rect at 150%");
    float ratio=Ratio(paragraph.color,OverBlack(band.color));
    Assert.Greater(ratio,MinBodyRatio,"correspondence ink must meet the existing worst-case body contrast acceptance");
    Assert.Greater(ratio,MinBaselineFraction*baselineRatio,"recover the existing fraction of gate-off contrast");
   var tracksGround=tracks.transform.parent.GetComponent<Image>();
   Assert.IsNotNull(tracksGround,"paired tracks need their own reading ground over the dark diagnostic surface");
   Assert.IsFalse(tracksGround.raycastTarget,"the track ground must not intercept practice input");
   var tracksRect=WorldRect(tracks.rectTransform);
   var tracksGroundRect=WorldRect(tracksGround.rectTransform);
   Assert.LessOrEqual(tracksGroundRect.xMin,tracksRect.xMin+.01f);
   Assert.GreaterOrEqual(tracksGroundRect.xMax,tracksRect.xMax-.01f);
   Assert.LessOrEqual(tracksGroundRect.yMin,tracksRect.yMin+.01f);
   Assert.GreaterOrEqual(tracksGroundRect.yMax,tracksRect.yMax-.01f);
   Assert.AreEqual(baselineInk,tracks.color,"do not recolor tracks to repair their contrast");
   Assert.Greater(Ratio(tracks.color,OverBlack(tracksGround.color)),MinBodyRatio,
    "paired tracks must meet the existing worst-case contrast acceptance");
   bool hasOpaqueStroke=false;
   foreach(var vertex in trackMesh.colors32)
   {
    if(vertex.a!=255)continue; // uncertainty fills are translucent; their outlines carry the shape
    hasOpaqueStroke=true;
    Assert.Greater(Ratio(vertex,OverBlack(tracksGround.color)),MinBodyRatio,
     "the rendered mesh, not only Graphic.color, must retain contrasting track outlines");
   }
   Assert.IsTrue(hasOpaqueStroke,"the paired-track mesh must render opaque outlines");
   foreach(var label in tracks.GetComponentsInChildren<Text>())
   {
    Assert.AreEqual(baselineInk,label.color);
    Assert.Greater(Ratio(label.color,OverBlack(tracksGround.color)),MinBodyRatio);
   }
    Assert.AreEqual(originalApproved,profile.runtimeApproved,"a reading-ground fix must never promote the candidate");
   }
  }

  [UnityTest] public IEnumerator M21GateOnGivesWorkSurfaceBodyTextAContentSafeReadingBandWithoutTouchingCopyColoursOrPlates()
  {
   Assert.IsNotNull(profile,"M20WorkSurface.asset missing — run Tools/M20/Import work surface candidate");
   Assert.IsNotNull(profile.workSurface,"M20WorkSurface.asset has no imported candidate texture");
   Assert.IsFalse(profile.runtimeApproved,"the r02 candidate must stay unapproved: M21 corrects readability inside the diagnostic gate, it does not promote");
   if(CommandLineDiagnostic)
    Assert.Ignore("--m20-ui-surface-diagnostic forces the gate on for this run; the gate-off baseline is not observable");

   // ---------- gate OFF: the committed surface, and no reading band anywhere ----------
   profile.diagnosticOverride=false;
   GameObject offHost=null;T0GameSession offGame=null;
   yield return Boot((h,g)=>{offHost=h;offGame=g;});

   Assert.IsTrue(offGame.PatrolActive,"fixture must be inside C1");
   Assert.IsTrue(offGame.PatrolComplete,"fixture must be the COMPLETED C1 patrol surface");
   Assert.AreEqual(ExpectedActionOrder,Ids(offGame),"committed action order");

   var offSurface=Live(offHost,"Work Surface");
   var offPlate=offSurface.GetComponent<Image>().color;
   var offViewport=Live(offHost,"Viewport");
   var offViewportRect=offViewport.rect;
   var offViewportIndex=offViewport.GetSiblingIndex();
   var offSurfaceChildren=offSurface.childCount;
   var offLabels=LabelSnapshot(offHost,offGame);
   var offEnabled=Ids(offGame).ToDictionary(id=>id,id=>Btn(offHost,id).interactable);
   var offFocusable=Ids(offGame).Count(id=>offGame.Interface.Focus(id));
   var offParagraphs=SurfaceParagraphs(offHost);
   var offRoles=offParagraphs.Select(t=>new{t.name,t.color,t.fontSize,t.fontStyle,t.text}).ToArray();
   var offBody=Body(offHost).color;
   var offHelper=HelperOf(offHost,Helper).color;

   Assert.AreEqual(0,Bands(offHost).Length,
    "gate off: the committed work surface must carry no reading band — the default look stays byte-identical");
   Assert.AreEqual(1,Backings(offSurface,M7Backing).Length,"gate off: the committed tiled M7 paper backing is still the work surface");
   Assert.AreEqual(0,Backings(offSurface,M20Backing).Length,"gate off: no r02 backing");
   Assert.Greater(offParagraphs.Count,1,"gate off: the surface must carry more than one paragraph to correct");
   AssertM19PlatesIntact(offHost,offGame,"gate off");

   // ---------- gate ON: r02 stays, and every surface paragraph gains a reading ground ----------
   profile.diagnosticOverride=true;
   GameObject onHost=null;T0GameSession onGame=null;
   yield return Boot((h,g)=>{onHost=h;onGame=g;});

   Assert.IsTrue(onGame.PatrolComplete,"gate on: the same completed C1 surface must be reached");
   var onSurface=Live(onHost,"Work Surface");

   // M20 is unchanged underneath: one full-bleed, non-tiled r02 backing, still behind everything.
   var r02=Backings(onSurface,M20Backing);
   Assert.AreEqual(1,r02.Length,"gate on: the r02 candidate must still be applied exactly once");
   Assert.AreSame(profile.workSurface,r02[0].texture,"gate on: the backing must still draw the imported r02 candidate");
   Assert.AreEqual(new Rect(0,0,1,1),r02[0].uvRect,"gate on: r02 must stay untiled");
   Assert.AreEqual(0,r02[0].transform.GetSiblingIndex(),"gate on: r02 must stay the first child of the panel");
   Assert.IsFalse(r02[0].raycastTarget,"gate on: r02 must not intercept clicks");
   Assert.AreEqual(0,Backings(onSurface,M7Backing).Length,"gate on: r02 still replaces the tiled M7 paper backing");

   // ---- the correction itself ----
   var onParagraphs=SurfaceParagraphs(onHost);
   Assert.AreEqual(offParagraphs.Count,onParagraphs.Count,"gate on: the same paragraphs must render");
   var bands=Bands(onHost);
   Assert.AreEqual(onParagraphs.Count,bands.Length,
    "gate on: every work-surface paragraph must sit on a content-safe reading band, because the committed "+
    "ink text is unreadable directly on the dark r02 ground (measured glyph:ground 1.11:1)");

   var panelRect=WorldRect(onSurface);
   foreach(var paragraph in onParagraphs)
   {
    var band=BandOf(paragraph);
    Assert.IsNotNull(band,"gate on: '"+paragraph.text.Substring(0,Mathf.Min(18,paragraph.text.Length))+
     "…' must have its reading band as the sibling immediately before it, so the band draws BEHIND the glyphs");
    Assert.IsFalse(band.raycastTarget,"gate on: the reading band must not intercept clicks");
    Assert.IsNull(band.sprite,"gate on: the reading band must be a flat colour, not a new image asset");
    var element=band.GetComponent<LayoutElement>();
    Assert.IsNotNull(element,"gate on: the band needs a LayoutElement to stay out of the column");
    Assert.IsTrue(element.ignoreLayout,
     "gate on: the band must be ignored by the layout group so no committed paragraph or button moves");

    // Low-noise strip, not an opaque card: r02 still shows through it and around it.
    Assert.Greater(band.color.a,0f,"gate on: the band must actually cover the reading ground");
    Assert.Less(band.color.a,1f,"gate on: the band must stay translucent so the archival surface reads through it");
    Assert.LessOrEqual(band.color.a,MaxBandAlpha,"gate on: the band must not become an opaque card");

    // Containment is checked in world space on all four sides. (The canvas is ScaleWithScreenSize
    // 1600x900, so canvas-local units are NOT world pixels — the flowed-height check below therefore
    // stays in the shared local space of `content`'s children.)
    var bandRect=WorldRect((RectTransform)band.transform);
    var textRect=WorldRect((RectTransform)paragraph.transform);
    Assert.LessOrEqual(bandRect.xMin,textRect.xMin+.01f,"gate on: the band must reach the left edge of its paragraph");
    Assert.GreaterOrEqual(bandRect.xMax,textRect.xMax-.01f,"gate on: the band must reach the right edge of its paragraph");
    Assert.GreaterOrEqual(bandRect.yMax,textRect.yMax-.01f,"gate on: the band must reach the top of its paragraph");
    Assert.LessOrEqual(bandRect.yMin,textRect.yMin+.01f,"gate on: the band must reach the bottom of its paragraph");
    // The flowed paragraph, not just its layout rect: FlowText only pins a minHeight, so the band must
    // cover whichever is taller — the assigned rect or the text the generator actually produced.
    var bandLocal=((RectTransform)band.transform).rect;
    var textLocal=((RectTransform)paragraph.transform).rect;
    Assert.GreaterOrEqual(bandLocal.height,Mathf.Max(textLocal.height,paragraph.preferredHeight)-.01f,
     "gate on: the band must cover the whole flowed paragraph, including lines that overflow its layout rect");
    // r02 preserved around it: a strip behind one paragraph, never the panel.
    Assert.Less(bandRect.width,panelRect.width,"gate on: the band must be narrower than the work surface");
    Assert.Less(bandRect.height,panelRect.height*.6f,"gate on: a per-paragraph strip must not become a full-panel plate");
   }

   // ---- readability actually restored, computed from the live component colours ----
   var onBodyText=Body(onHost);var onHelperText=HelperOf(onHost,Helper);
   Assert.IsNotNull(onHelperText,"gate on: the contextual helper must render");
   var bodyGround=OverBlack(BandOf(onBodyText).color);
   var helperGround=OverBlack(BandOf(onHelperText).color);
   var offGround=OverBlack(offPlate);
   float bodyRatio=Ratio(onBodyText.color,bodyGround),helperRatio=Ratio(onHelperText.color,helperGround);
   float bodyBaseline=Ratio(offBody,offGround),helperBaseline=Ratio(offHelper,offGround);

   Assert.Greater(bodyRatio,MinBodyRatio,
    "gate on: over the reading band the committed body ink must reach at least "+MinBodyRatio+
    ":1 on the darkest possible ground; it measured 1.11:1 directly on r02");
   Assert.Greater(bodyRatio,MinBaselineFraction*bodyBaseline,
    "gate on: the body must recover most of its gate-off ground ("+bodyBaseline.ToString("0.00")+":1)");
   Assert.Greater(helperRatio,MinBaselineFraction*helperBaseline,
    "gate on: the instruction helper must recover most of its gate-off ground ("+helperBaseline.ToString("0.00")+":1)");

   // ---- no copy, colour, role, action, input, state or save mutation ----
   var onRoles=onParagraphs.Select(t=>new{t.name,t.color,t.fontSize,t.fontStyle,t.text}).ToArray();
   Assert.AreEqual(offRoles,onRoles,
    "gate on: paragraph copy, colour, size and weight must all be identical — the ground changed, not the text");
   Assert.AreEqual(offBody,onBodyText.color,"gate on: body ink must not be recoloured");
   Assert.AreEqual(offHelper,onHelperText.color,"gate on: helper tone must not be recoloured");
   Assert.AreEqual(ExpectedActionOrder,Ids(onGame),"gate on: action order unchanged");
   Assert.AreEqual(offLabels,LabelSnapshot(onHost,onGame),"gate on: every action label unchanged");
   foreach(var pair in offEnabled)
    Assert.AreEqual(pair.Value,Btn(onHost,pair.Key).interactable,"gate on: "+pair.Key+" availability unchanged");
   Assert.AreEqual(offFocusable,Ids(onGame).Count(id=>onGame.Interface.Focus(id)),"gate on: the focus ring size is unchanged");
   Assert.AreEqual(offPlate,onSurface.GetComponent<Image>().color,"gate on: the Work Surface plate colour is unchanged");
   Assert.AreEqual(offSurfaceChildren,onSurface.childCount,"gate on: the Work Surface child count cannot grow");
   var onViewport=Live(onHost,"Viewport");
   Assert.AreEqual(offViewportIndex,onViewport.GetSiblingIndex(),"gate on: the committed Viewport keeps its sibling index");
   Assert.AreEqual(offViewportRect,onViewport.rect,"gate on: the Viewport rect must not move or resize");
   AssertM19PlatesIntact(onHost,onGame,"gate on");

   // Input still reaches the plates through the band, and the correction survives a re-render.
   Assert.IsTrue(onGame.Interface.Focus("c1-review"),"gate on: c1-review must be focusable");
   Assert.AreEqual("c1-review",onGame.Interface.CurrentFocusId,"gate on: focus must land on the focused action");
   Assert.IsTrue(onGame.Interface.Activate("c1-review"),"gate on: activating through the band must work");
   yield return null;
   Assert.AreNotEqual(ExpectedActionOrder,Ids(onGame),"gate on: the click must actually change the screen");
   yield return null;
   var afterClick=Bands(onHost);
   Assert.AreEqual(SurfaceParagraphs(onHost).Count,afterClick.Length,"gate on: the band count still matches the paragraphs after a re-render");
   foreach(var band in afterClick) Assert.IsFalse(band.raycastTarget,"gate on: a re-rendered band still must not intercept clicks");

   Assert.IsFalse(onGame.SavePending,"gate on: a reading ground must not dirty the save");
  }
 }
}
#endif
