#if TIDE_TEST_FRAMEWORK
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Tide.App;
using Tide.Presentation;
using Tide.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.TestTools;
using UnityEngine.UI;
namespace Tide.Tests {
 // M25 (RFC-CX-M25-20260918): the guide overlay, the F2 route, the type-scale contract and the resource gate.
 public sealed class M25GuidePlayModeTests {
  string directory;GameObject host;T0GameSession game;Keyboard keyboard;M25ResourceProfile profile;bool originalApproved;InputSettings.BackgroundBehavior originalBackground;
#if UNITY_EDITOR
  InputSettings.EditorInputBehaviorInPlayMode originalEditorInput;
#endif
  [UnitySetUp] public IEnumerator Setup(){
   directory=Path.Combine(Path.GetTempPath(),"m25-guide-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(directory);
#if UNITY_EDITOR
   originalEditorInput=InputSystem.settings.editorInputBehaviorInPlayMode;InputSystem.settings.editorInputBehaviorInPlayMode=InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
#endif
   originalBackground=InputSystem.settings.backgroundBehavior;InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;
   keyboard=InputSystem.AddDevice<Keyboard>();
   profile=Resources.Load<M25ResourceProfile>("M25Resources");if(profile!=null)originalApproved=profile.runtimeApproved;
   host=new GameObject("M25 guide test");game=host.AddComponent<T0GameSession>();game.Initialize(Resources.Load<T0RuntimeConfig>("T0Runtime"),directory);
   yield return null;
  }
  IEnumerator Wait(Task task){while(!task.IsCompleted)yield return null;if(task.IsFaulted)throw task.Exception;}
  [UnityTearDown] public IEnumerator TearDown(){
   if(profile!=null)profile.runtimeApproved=originalApproved;
   if(game!=null)yield return Wait(game.FlushSaves());
   if(host!=null)UnityEngine.Object.Destroy(host);
   if(keyboard!=null)InputSystem.RemoveDevice(keyboard);
   InputSystem.settings.backgroundBehavior=originalBackground;
#if UNITY_EDITOR
   InputSystem.settings.editorInputBehaviorInPlayMode=originalEditorInput;
#endif
   yield return null;
   if(directory!=null&&Directory.Exists(directory))Directory.Delete(directory,true);
  }
  void Click(string id){Assert.IsTrue(game.Interface.Activate(id),"Missing UI action "+id);}
  Text[] Texts()=>host.GetComponentsInChildren<Text>();
  string Visible()=>string.Join("\n",Texts().Select(t=>t.text));
  IEnumerator Press(Key key){InputSystem.QueueStateEvent(keyboard,new KeyboardState(key));yield return null;InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return null;}

  [Test] public void TypeScaleTiersAreStrictlyOrdered(){
   Assert.Greater(TypeScale.Display,TypeScale.Title);Assert.Greater(TypeScale.Title,TypeScale.Body);
   Assert.Greater(TypeScale.Body,TypeScale.Label,"body outranks the action label");
   Assert.GreaterOrEqual(TypeScale.Label,TypeScale.Section);Assert.Greater(TypeScale.Section,TypeScale.Status);
   Assert.Greater(TypeScale.Status,TypeScale.Helper,"status is more important than helper text");
   Assert.Greater(TypeScale.Helper,TypeScale.Meta);Assert.Greater(TypeScale.LineSpacing,1f);
  }
  [UnityTest] public IEnumerator StartScreenOffersGuideAndGuideNeverWritesState(){
   Assert.IsTrue(game.Interface.ActionIds.Contains("guide"),"the start screen must offer the guide");
   var hash=game.Journal.State.StateHash;var head=game.Journal.HeadSeq;
   Click("guide");yield return null;
   Assert.AreEqual(T0GameSession.GuideOverlay,game.Surface);
   var text=Visible();
   foreach(var expected in new[]{"지금 할 일","당직 인수의 세 단계","도구 절차","조작","규칙","F2","힌트는 무료"})StringAssert.Contains(expected,text);
   var headings=Texts().Where(t=>t.name=="Section heading").ToArray();
   Assert.GreaterOrEqual(headings.Length,5,"every guide section carries its own heading");
   foreach(var heading in headings){Assert.AreEqual(FontStyle.Bold,heading.fontStyle,heading.text+" heading is bold");Assert.AreEqual(Mathf.RoundToInt(TypeScale.Section*game.Interface.TextScale),heading.fontSize);}
   var body=Texts().First(t=>t.name=="Text"&&t.transform.parent.name=="Content");
   Assert.AreEqual(Mathf.RoundToInt(TypeScale.Body*game.Interface.TextScale),body.fontSize,"the guide body reads at the Body tier");
   Assert.Greater(body.fontSize,headings[0].fontSize,"body outranks section headings in size; headings win by weight");
   Assert.AreEqual(hash,game.Journal.State.StateHash,"the guide is decoration: no puzzle state change");
   Assert.AreEqual(head,game.Journal.HeadSeq);Assert.IsFalse(File.Exists(Path.Combine(directory,"save.json")),"opening the guide must not write a save");
   game.Back();yield return null;Assert.AreEqual("shell",game.Surface);Assert.IsTrue(game.Interface.ActionIds.Contains("start"));
  }
  [UnityTest] public IEnumerator F2OpensGuideDuringPlayAndToolbarListsIt(){
   game.StartGame();yield return null;
   Assert.IsTrue(host.GetComponentsInChildren<Button>().Any(b=>b.name=="guide"),"the toolbar lists the guide next to hints");
   yield return Press(Key.F2);
   Assert.AreEqual(T0GameSession.GuideOverlay,game.Surface,"F2 routes to the guide overlay");
   StringAssert.Contains("t0-b1",Visible(),"the guide names the current beat before any objective is met");
   StringAssert.Contains("○ 1.",Visible(),"unfinished steps are marked open");
   yield return Press(Key.Escape);
   Assert.AreEqual("shell",game.Surface,"Esc closes the guide like any overlay");
  }
  [UnityTest] public IEnumerator GateOffKeepsCommittedOpeningAndNoBackdrop(){
   if(profile==null)Assert.Ignore("M25Resources.asset not imported");
   profile.runtimeApproved=false;game.Render();yield return null;
   Assert.IsFalse(host.GetComponentsInChildren<RawImage>().Any(r=>r.name=="M25 backdrop"),"gate off must not insert the M25 backdrop");
   Click("guide");yield return null;
   Assert.IsFalse(host.GetComponentsInChildren<RawImage>().Any(r=>r.name=="Picture"&&r.texture!=null),"gate off renders no M25 figures");
  }
  [UnityTest] public IEnumerator GateOnDrawsBackdropIconsAndPortraitsWithoutRaycasts(){
   if(profile==null||profile.startBackdrop==null)Assert.Ignore("M25Resources.asset not imported");
   profile.runtimeApproved=true;game.Render();yield return null;
   var backdrop=host.GetComponentsInChildren<RawImage>().Single(r=>r.name=="M25 backdrop");
   Assert.IsFalse(backdrop.raycastTarget);Assert.AreEqual(0,backdrop.transform.GetSiblingIndex(),"the backdrop sits behind the navigation viewport");
   Assert.AreEqual("Navigation",backdrop.transform.parent.name);
   Click("guide");yield return null;
   var pictures=host.GetComponentsInChildren<RawImage>().Where(r=>r.name=="Picture").ToArray();
   Assert.GreaterOrEqual(pictures.Count(p=>p.texture!=null),6+2,"six tool icons and two portraits are drawn");
   foreach(var picture in pictures)Assert.IsFalse(picture.raycastTarget,"figures never intercept input");
   foreach(var banned in new[]{"문재화","오은정","표성찬"})StringAssert.DoesNotContain(banned,Visible(),"only the T0-public cast is named");
   Assert.IsFalse(File.Exists(Path.Combine(directory,"save.json")));
  }
  [UnityTest] public IEnumerator CircuitTeachingHeaderNamesBeatAndRemainingConditions(){
   Click("start");if(game.OpeningActive)Click("intro-skip");
   Click("handover");foreach(var id in new[]{"hb-l1","hb-l2","hb-l3"})Click("line-"+id);Click("close-document");
   Click("transfer");foreach(var id in new[]{"tl-r1","tl-r2","tl-r3"})Click("row-"+id);Click("decision-written");Click("close-document");Click("load-plate-zero");
   yield return Wait(game.FlushSaves());
   game.OpenTool("circuit");yield return null;
   var text=Visible();
   StringAssert.Contains("▶ 안내 · 단계 t0-b2 · 남은 조건",text,"the guided teaching header names the beat and the remaining conditions");
   StringAssert.Contains("F2 전체 안내",text,"the header points at the full guide");
   StringAssert.DoesNotContain("남은 조건 0개",text,"a freshly opened circuit still has open conditions");
  }
 }
}
#endif
