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
using Tide.Sim;
using Tide.UI;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
namespace Tide.Tests {
 // M27 (RFC-CX-M27-20260918) 실마리 명료화: requirement checklist, stage marker, data-driven next, guide task list,
 // C1 checklist, hint-offer toggle, glyph gate. Everything asserted against Simulation / the emitted tables, never literals.
 public sealed class M27ChecklistPlayModeTests {
  string directory;GameObject host;T0GameSession game;M25ResourceProfile profile;bool originalM27;
  [UnitySetUp] public IEnumerator Setup(){
   directory=Path.Combine(Path.GetTempPath(),"m27-checklist-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(directory);
   profile=Resources.Load<M25ResourceProfile>("M25Resources");if(profile!=null)originalM27=profile.m27Approved;
   host=new GameObject("M27 checklist test");game=host.AddComponent<T0GameSession>();game.Initialize(Resources.Load<T0RuntimeConfig>("T0Runtime"),directory);
   yield return null;
  }
  IEnumerator Wait(Task task){while(!task.IsCompleted)yield return null;if(task.IsFaulted)throw task.Exception;}
  [UnityTearDown] public IEnumerator TearDown(){
   if(profile!=null)profile.m27Approved=originalM27;
   if(game!=null)yield return Wait(game.FlushSaves());
   if(host!=null)UnityEngine.Object.Destroy(host);
   yield return null;
   if(directory!=null&&Directory.Exists(directory))Directory.Delete(directory,true);
  }
  void Click(string id){Assert.IsTrue(game.Interface.Activate(id),"Missing UI action "+id);}
  Text[] Texts()=>host.GetComponentsInChildren<Text>();
  string Card()=>Texts().Single(t=>t.name=="CaseThread").text;
  Text[] Chips()=>Texts().Where(t=>t.name==T0Interface.ChecklistName).ToArray();
  JObject Beats()=>JObject.Parse(Resources.Load<T0RuntimeConfig>("T0Runtime").beats.text);
  void Begin(){Click("start");if(game.OpeningActive)Click("intro-skip");}
  void Intake(){Click("handover");foreach(var id in new[]{"hb-l1","hb-l2","hb-l3"})Click("line-"+id);Click("close-document");Click("transfer");foreach(var id in new[]{"tl-r1","tl-r2","tl-r3"})Click("row-"+id);Click("decision-written");Click("close-document");Click("load-plate-zero");}
  void Circuit(){Click("hub-view-circuitmap");Click("open-circuit");Click("trace-hub");Click("begin-overlay");Click("offset-left");Click("offset-up");Click("anchor-overlay");foreach(var area in game.Definition.UncoveredAreas){Click("area-"+area);Click("area-evidence-"+area);Click("attach-rec-watchlog-bureau");Click("overlay-back");}}
  IEnumerator OpenReader(){Click("hub-view-reader");Click("open-reader");Click("load-rec-plate-standard-hub");yield return null;}
  IEnumerator Cite(string record){
   if(game.Journal.State.LoadedRecordId!=record){Click("select-record");Click("choose-"+record);}
   Click("read");
   Assert.IsTrue(game.SubmitImmediate(new PuzzleCommand("SetWindow",value:"H-1:00",otherValue:"H+3:00")).IsValid);
   yield return Wait(game.CommitAsync(new PuzzleCommand("CiteToBoard",record)));
  }

  // The checklist is exactly the current beat's requirements, in authored step order, with IsSatisfied as the only truth.
  [UnityTest] public IEnumerator ChecklistMirrorsTheCurrentBeatRequirementsAndStepOrder(){
   Begin();yield return null;
   foreach(var beatId in new[]{"t0-b1","t0-b2","t0-b3"}){
    Assert.AreEqual(beatId,game.CurrentBeat,"walk follows the authored beat order");
    var beat=game.Definition.Beats.Single(b=>b.Id==beatId);
    var items=game.CaseChecklist();
    Assert.AreEqual(beat.Requirements.Count,items.Count,beatId+": one chip per requirement");
    Assert.AreEqual(beat.Requirements.Count,Chips().Length,beatId+": every requirement is drawn as a chip");
    var rows=(JArray)Beats()["rows"].First(r=>(string)r["id"]==beatId)["completionPredicate"]["requires"];
    var expectedCaptions=rows.OrderBy(r=>(int)r["step"]).Select(r=>(string)r["caption"]).ToArray();
    CollectionAssert.AreEqual(expectedCaptions,items.Select(i=>i.Caption).ToArray(),beatId+": chips follow beats.json step order");
    Assert.AreEqual(1,items.Count(i=>i.Current),beatId+": exactly one current item while the beat is open");
    Assert.IsFalse(items.First(i=>i.Current).Done);
    StringAssert.Contains("다음: "+items.First(i=>i.Current).Label,Card(),beatId+": the next line is the first open requirement's label");
    StringAssert.Contains("단계 "+(Array.IndexOf(new[]{"t0-b1","t0-b2","t0-b3"},beatId)+1)+"/3 · ",Card(),beatId+": stage marker");
    foreach(var chip in Chips()){Assert.IsFalse(chip.raycastTarget);Assert.IsNull(chip.GetComponentInParent<Selectable>(),"chips never become actions");}
    if(beatId=="t0-b1"){Intake();}
    else if(beatId=="t0-b2"){Circuit();}
    else{yield return OpenReader();yield return Cite("rec-plate-standard-hub");yield return Cite("rec-tide-ledger-bureau");yield return Wait(game.FlushSaves());}
    yield return null;
   }
   Assert.IsTrue(game.Simulation.IsComplete(game.Journal.State,"t0-b3"));
   Assert.IsTrue(game.CaseChecklist().All(i=>i.Done),"a complete beat has no open chip");
   StringAssert.Contains("근거 쌍이 고정됨",Card());
  }

  // Two pins with the wrong plate window used to hide behind "2/2": the gap requirement now stays visibly open.
  [UnityTest] public IEnumerator WrongWindowLeavesTheGapRequirementOpenOnTheCard(){
   Begin();Intake();Circuit();yield return OpenReader();
   Click("read");
   Assert.IsTrue(game.SubmitImmediate(new PuzzleCommand("SetWindow",value:"H-1:00",otherValue:"H+2:00")).IsValid);
   yield return Wait(game.CommitAsync(new PuzzleCommand("CiteToBoard","rec-plate-standard-hub")));
   yield return Cite("rec-tide-ledger-bureau");yield return Wait(game.FlushSaves());yield return null;
   var items=game.CaseChecklist();
   Assert.AreEqual(2,items.Count(i=>i.Caption.EndsWith("인용")&&i.Done),"both citations are pinned");
   var gap=items.Single(i=>i.Caption=="결손 구간");
   Assert.IsFalse(gap.Done);Assert.IsTrue(gap.Current,"the endpoint requirement is the visible next step");
   StringAssert.Contains("필요한 인용 2/2",Card());StringAssert.Contains("다음: "+gap.Label,Card());
   StringAssert.DoesNotContain("H-1",Card());StringAssert.DoesNotContain("H+3",Card());
   StringAssert.DoesNotContain("H+2",Card());
  }

  // The guide lists the same conditions as full sentences with the same ✓ ▶ ○ form cue.
  [UnityTest] public IEnumerator GuideTaskSectionListsEveryConditionWithFormCues(){
   Begin();Intake();yield return null;
   game.OpenOverlay("guide");yield return null;
   var visible=string.Join("\n",Texts().Select(t=>t.text));
   foreach(var item in game.CaseChecklist())StringAssert.Contains((item.Done?"✓ ":item.Current?"▶ ":"○ ")+item.Label,visible,"guide lists "+item.Caption);
   StringAssert.Contains("이 단계의 조건 · 조건 0/2",visible);
   StringAssert.Contains("단계 2/3 · ",visible,"the guide names the stage, not the beat id");
   StringAssert.DoesNotContain("t0-b2",visible);
  }

  // C1: the checklist and the guide derive from the patrol packet (observations, condition, confirmation).
  [UnityTest] public IEnumerator PatrolChecklistFollowsThePacketAndTheGuideShowsIt(){
   Begin();Intake();Circuit();yield return OpenReader();
   yield return Cite("rec-plate-standard-hub");yield return Cite("rec-tide-ledger-bureau");yield return Wait(game.FlushSaves());
   game.Back();yield return null;if(game.Surface!="shell"){game.Back();yield return null;}
   Click("continue-c1");yield return null;
   Assert.IsTrue(game.PatrolActive);
   var items=game.CaseChecklist();
   Assert.AreEqual(game.Definition.Patrol.Observations.Count+2,items.Count,"observations + condition + confirmation");
   Assert.IsTrue(items.Take(game.Definition.Patrol.Observations.Count).All(i=>!i.Done));
   StringAssert.Contains("C1 1/2 · ",Card());StringAssert.Contains("관찰 0 / "+game.Definition.Patrol.Observations.Count,Card());
   StringAssert.Contains("다음: 두 기록 확인",Card());
   var first=game.Definition.Patrol.Observations[0];
   Click("c1-open-"+first);Click("c1-observe-"+first);Click("c1-close-observation");yield return null;
   Assert.IsTrue(game.CaseChecklist()[0].Done,"observing marks the first chip done");
   game.OpenOverlay("guide");yield return null;
   var visible=string.Join("\n",Texts().Select(t=>t.text));
   StringAssert.Contains("✓ "+game.CaseChecklist()[0].Label,visible);
   StringAssert.DoesNotContain("사건 흐름 카드와 힌트가 이 구간의 목표를 안내합니다.",visible,"the C1 placeholder copy is gone");
  }

  // The idle hint offer can be switched off; the knobs stay data-owned and the setting survives a save.
  [UnityTest] public IEnumerator HintOfferSettingTogglesAndPersists(){
   Begin();yield return null;
   Assert.IsTrue(game.HintOfferEnabled,"default on");
   Click("settings");Click("hint-offer");yield return null;
   Assert.IsFalse(game.HintOfferEnabled);
   var settings=JObject.Parse(File.ReadAllText(Path.Combine(directory,"settings.json")));
   Assert.AreEqual(false,(bool)settings["hintOffer"],"the setting is persisted");
   Assert.IsFalse(game.HintOfferVisible);
   Click("hint-offer");yield return null;Assert.IsTrue(game.HintOfferEnabled);
  }

  // Gate on: chips draw the checklist glyph textures; gate off: the ✓ ▶ ○ text cue alone carries the state.
  [UnityTest] public IEnumerator ChecklistGlyphsFollowTheResourceGate(){
   if(profile==null||profile.checklistGlyphs.Length==0||profile.checklistGlyphs.Any(t=>t==null))Assert.Ignore("M27 glyphs not imported in this project");
   Begin();yield return null;
   profile.m27Approved=true;game.Render();yield return null;
   var pictures=host.GetComponentsInChildren<RawImage>().Where(r=>r.name=="Glyph").ToArray();
   Assert.AreEqual(game.CaseChecklist().Count,pictures.Length,"one glyph per chip when the gate is on");
   Assert.IsTrue(pictures.All(p=>!p.raycastTarget));
   profile.m27Approved=false;game.Render();yield return null;
   Assert.AreEqual(0,host.GetComponentsInChildren<RawImage>().Count(r=>r.name=="Glyph"),"gate off draws no glyph");
   Assert.IsTrue(Chips().All(c=>c.text.StartsWith("✓ ")||c.text.StartsWith("▶ ")||c.text.StartsWith("○ ")),"text form cue is always present");
  }
 }
}
#endif
