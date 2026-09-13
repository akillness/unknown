#if TIDE_TEST_FRAMEWORK
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Newtonsoft.Json.Linq;
using Tide.App;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tide.Tests
{
 // M19 — one global presentation slice for every T0 action plate plus the three text roles.
 // Contract: _workspace/current/presentation/t0-action-plate-m19.md
 //
 // Why the completed-C1 patrol screen: it is the only genuinely reachable screen that renders all
 // three text roles at once on live buttons — body (GameScreen.Body), action label (ViewAction.Label)
 // and contextual helper (ViewAction.Detail) — and it boots from the same committed fixture the
 // C1/M5/M8/M18 suites already use. No GameScreen is hand-built and no journal state is written:
 // T0Interface renders the session's own model, so every assertion below reads real Unity component
 // state (Image.color, Graphic.raycastTarget, Text.fontSize/fontStyle, Selectable.colors, activeSelf).
 public sealed class T0ActionPlatePlayModeTests
 {
  // Observed committed order on the completed-C1 patrol surface (C1GameSession.PatrolScreen).
  static readonly string[] ExpectedActionOrder={"continue-c1-signature","c1-review","c1-interview-prep"};
  const string Helper="c1-interview-prep";

  string directory;GameObject host;T0GameSession game;

  [UnitySetUp] public IEnumerator SetUp()
  {
   directory=Path.Combine(Path.GetTempPath(),"t0-action-plate-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(directory);
   // The real completed-C1 fixture the existing suites use — not fabricated state.
   File.Copy(Path.Combine(Application.dataPath,"_Project/Tests/Fixtures/C1PatrolCompletedV2.json"),Path.Combine(directory,"save.json"));
   host=new GameObject("T0 action plate session");game=host.AddComponent<T0GameSession>();
   game.Initialize(Resources.Load<T0RuntimeConfig>("T0Runtime"),directory);
   yield return null;game.StartGame();yield return null;
  }
  [UnityTearDown] public IEnumerator TearDown()
  {
   if(game!=null)yield return Wait(game.FlushSaves());
   if(host!=null)UnityEngine.Object.Destroy(host);
   yield return null;
   if(directory!=null&&Directory.Exists(directory))Directory.Delete(directory,true);
  }
  IEnumerator Wait(Task task){while(!task.IsCompleted)yield return null;if(task.IsFaulted)throw task.Exception;}

  // Live screen only: Render() deactivates the previous root before its deferred Destroy, and
  // GetComponentsInChildren<T>() skips inactive objects, so stale copies are never counted.
  Button[] Buttons()=>host.GetComponentsInChildren<Button>();
  Button Btn(string id)=>Buttons().Single(b=>b.name==id);
  Image Plate(string id)=>Btn(id).GetComponent<Image>();
  Text Label(string id)=>Btn(id).GetComponentsInChildren<Text>().Single(t=>t.name=="Label");
  string[] Ids()=>game.Interface.ActionIds.ToArray();
  Dictionary<string,string> LabelSnapshot()=>Ids().ToDictionary(id=>id,id=>Label(id).text);

  // FlowText names every flowed paragraph "Text". The body is the first one under "Content";
  // a ViewAction.Detail is emitted as the sibling immediately before its own button.
  Text Body()
  {
   var content=host.GetComponentsInChildren<RectTransform>().First(r=>r.name=="Content");
   for(int i=0;i<content.childCount;i++){var t=content.GetChild(i).GetComponent<Text>();if(t!=null&&t.name=="Text")return t;}
   return null;
  }
  Text HelperOf(string id)
  {
   var button=Btn(id).transform;var parent=button.parent;int index=button.GetSiblingIndex();
   if(index==0)return null;
   var previous=parent.GetChild(index-1);
   return previous.GetComponent<Button>()!=null?null:previous.GetComponent<Text>();
  }

  static float Channel(float v)=>v<=.04045f?v/12.92f:Mathf.Pow((v+.055f)/1.055f,2.4f);
  static float Lum(Color c)=>.2126f*Channel(c.r)+.7152f*Channel(c.g)+.0722f*Channel(c.b);

  string KoString(string key)=>(string)JObject.Parse(Resources.Load<T0RuntimeConfig>("T0Runtime").strings.text)[key]["ko"];

  Image Ornament(string buttonId,string child)
  {
   var found=Btn(buttonId).transform.Find(child);
   Assert.IsNotNull(found,buttonId+" must carry a '"+child+"' plate ornament so the action reads as an instrument plate, not a flat dark rectangle");
   var image=found.GetComponent<Image>();
   Assert.IsNotNull(image,buttonId+"/"+child+" must be an Image drawn from the resolved M7 skin colours");
   return image;
  }

  // Every enabled plate carries the brass rule + bevel; ornaments never intercept input; the root
  // Image stays the raycast surface; a disabled plate drops its rule so "pressable" has no marker.
  void AssertPlateAffordance()
  {
   foreach(var button in Buttons())
   {
    var root=button.GetComponent<Image>();
    Assert.IsTrue(root.raycastTarget,button.name+" root Image must stay raycastable");
    Assert.IsNull(button.GetComponent<LayoutGroup>(),button.name+" root must have no LayoutGroup so ornaments cannot displace Label/Hold progress");
    var bevel=Ornament(button.name,"Bevel");
    Assert.IsFalse(bevel.raycastTarget,button.name+"/Bevel must not intercept clicks");
    Assert.IsTrue(bevel.gameObject.activeSelf,button.name+"/Bevel is the plate edge and is always drawn");
    var rule=Ornament(button.name,"Rule");
    Assert.IsFalse(rule.raycastTarget,button.name+"/Rule must not intercept clicks");
    Assert.AreEqual(button.interactable,rule.gameObject.activeSelf,
     button.name+"/Rule must be visible exactly when the action is enabled");
    Assert.AreEqual(1,button.GetComponentsInChildren<Text>().Count(t=>t.name=="Label"),button.name+" keeps exactly one Label");
   }
  }

  // The committed defect: selectedColor(brass) multiplied the ink plate and made focus DARKER, and
  // vertex colours clamp so a multiply can never brighten. Focus must therefore invert the plate.
  void AssertFocusInversion(string focused)
  {
   Assert.IsTrue(game.Interface.Focus(focused),"focusable action "+focused);
   var plate=Plate(focused);var label=Label(focused);
   var colors=Btn(focused).colors;
   Assert.AreEqual(Color.white,colors.selectedColor,focused+": selectedColor must no longer multiply the plate darker");
   Assert.AreEqual(0f,colors.fadeDuration,1e-4f,focused+": focus state must read immediately");
   Assert.Less(Lum(label.color),Lum(plate.color),focused+" is focused: its label must be dark-on-light");
   foreach(var other in Ids().Where(id=>id!=focused))
   {
    Assert.Greater(Lum(Label(other).color),Lum(Plate(other).color),other+" is unfocused: its label must be light-on-dark");
    Assert.Greater(Lum(plate.color)-Lum(Plate(other).color),.12f,
     "focused plate "+focused+" must be measurably brighter than unfocused plate "+other);
   }
  }

  [UnityTest] public IEnumerator CompletedC1ActionPlatesCarryAffordanceFocusInversionAndThreeTextTiersWithoutChangingActionsLabelsOrReflow()
  {
   // ---- the real completed-C1 surface is genuinely reached before anything M19 is asserted
   Assert.IsTrue(game.PatrolActive,"fixture must be inside C1");
   Assert.IsTrue(game.PatrolComplete,"fixture must be the COMPLETED C1 patrol surface");
   Assert.IsFalse(game.SignatureActive,"must still be the C1 patrol surface, not the next beat");
   Assert.AreEqual("shell",game.Surface,"no overlay may be open");
   CollectionAssert.AreEqual(ExpectedActionOrder,Ids(),"committed action id order");

   // ---- N3/N4/N1/N10: ids, enabled state, label content and focusability recorded BEFORE any M19 claim
   foreach(var id in ExpectedActionOrder)Assert.IsTrue(Btn(id).interactable,id+" must stay enabled");
   Assert.AreEqual("서명지철 조사로 이동",Label("continue-c1-signature").text,"committed label");
   Assert.AreEqual(KoString("evidence"),Label("c1-review").text,"label resolved from the runtime strings table");
   Assert.AreEqual("면접 준비 · 형식 안내",Label(Helper).text,"committed label");
   var labelsAtScale1=LabelSnapshot();
   var focusOrder=Ids().Select(id=>{Assert.IsTrue(game.Interface.Focus(id),id+" must be focusable");return game.Interface.CurrentFocusId;}).ToArray();
   CollectionAssert.AreEqual(ExpectedActionOrder,focusOrder,"focus reaches each action by its own id");
   var stateHash=game.Journal.State.StateHash;var head=game.Journal.HeadSeq;var receipts=game.SuccessfulReceipts;

   // ---- P1: action affordance (first M19 assertion — this is where RED lands)
   AssertPlateAffordance();

   // ---- P2: focused plate is legible, and the inversion follows the focus
   AssertFocusInversion("continue-c1-signature");
   AssertFocusInversion(Helper);

   // ---- P4: three distinct text tiers, by size AND weight AND tone
   var body=Body();Assert.IsNotNull(body,"the completed-C1 screen renders a body paragraph");
   var helper=HelperOf(Helper);Assert.IsNotNull(helper,Helper+" renders its ViewAction.Detail helper paragraph");
   var label=Label(Helper);
   Assert.Greater(body.fontSize,label.fontSize,"body must stay the largest reading tier");
   Assert.Greater(label.fontSize,helper.fontSize,"action label must outrank contextual helper text");
   Assert.AreEqual(FontStyle.Bold,label.fontStyle,"action labels carry the action weight");
   Assert.AreEqual(FontStyle.Normal,helper.fontStyle,"helper text must not compete with the action label");
   Assert.AreEqual(FontStyle.Normal,body.fontStyle,"body weight is unchanged");
   Assert.AreNotEqual(body.color,helper.color,"helper text must be toned down away from body text");
   Assert.Greater(Lum(helper.color),Lum(body.color),"helper text is muted toward the work surface");

   // ---- N6/N7: rendering this slice wrote nothing
   Assert.IsFalse(game.SavePending,"presentation must not queue a save");
   Assert.AreEqual(stateHash,game.Journal.State.StateHash,"simulation state hash unchanged");
   Assert.AreEqual(head,game.Journal.HeadSeq,"journal head unchanged");
   Assert.AreEqual(receipts,game.SuccessfulReceipts,"receipt count unchanged");

   // ---- N11: text scale 1.5 keeps every invariant and does not truncate a label
   game.Interface.Activate("settings");yield return null;
   for(int i=0;i<5;i++)game.Interface.Activate("text-scale");
   game.Back();yield return null;yield return null;
   Assert.AreEqual(1.5f,game.Interface.TextScale,"five text-scale steps reach 1.5");
   Assert.AreEqual("shell",game.Surface,"back returns to the completed-C1 surface");
   CollectionAssert.AreEqual(ExpectedActionOrder,Ids(),"action id order survives the text scale change");
   CollectionAssert.AreEqual(labelsAtScale1,LabelSnapshot(),"label content is identical at text scale 1.5");
   AssertPlateAffordance();
   AssertFocusInversion(Helper);
   var body150=Body();var helper150=HelperOf(Helper);var label150=Label(Helper);
   Assert.Greater(body150.fontSize,label150.fontSize,"tier order holds at 1.5");
   Assert.Greater(label150.fontSize,helper150.fontSize,"tier order holds at 1.5");
   Assert.Greater(label150.fontSize,label.fontSize,"labels actually scale with the text scale");
   Canvas.ForceUpdateCanvases();
   foreach(var wrapped in host.GetComponentsInChildren<Tide.UI.WrappedButtonHeight>())
    Assert.That(wrapped.Label.rectTransform.rect.height+.6f,Is.GreaterThanOrEqualTo(wrapped.Label.preferredHeight),
     wrapped.name+" must contain its wrapped label at text scale 1.5");
  }
 }
}
#endif
