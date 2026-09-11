#if TIDE_TEST_FRAMEWORK
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Tide.App;
using Tide.Save;
using Tide.Sim;
using UnityEngine;
namespace Tide.Tests {
 // RFC-CX-011 M9 core slices: hint persistence (S-A), two-step default (S-B), preview diff (S-C), beat objectives (S-D).
 public sealed class M9CoreTests {
  T0Definition data;T0Simulation sim;
  [SetUp] public void SetUp(){data=Resources.Load<T0RuntimeConfig>("T0Runtime").catalog.Load();sim=new T0Simulation(data);}
  static void Do(CommandJournal j,PuzzleCommand c){var v=j.Submit(c);Assert.IsTrue(v.IsValid,v.DataDiagnostic??v.Reason?.ToString());}
  CommandJournal Intake(){var j=new CommandJournal(sim);foreach(var r in data.Beats[0].Requirements){foreach(var id in r.Ids){if(r.Type=="recordLinesViewed")Do(j,new PuzzleCommand("ViewLine",r.RecordId,id));if(r.Type=="recordRowsViewed")Do(j,new PuzzleCommand("ViewRow",r.RecordId,id));}if(r.Type=="slotLoaded")Do(j,new PuzzleCommand("PlaceInSlot",r.Id,r.RecordId));if(r.Type=="decisionRecorded")Do(j,new PuzzleCommand("RecordDecision",r.Id,r.Ids[0]));}return j;}
  CommandJournal ReaderReady(){var j=Intake();Do(j,new PuzzleCommand("OpenTool","circuit"));Do(j,new PuzzleCommand("BeginOverlay"));Do(j,new PuzzleCommand("SetOverlayOffset",value:"-1",otherValue:"1"));Do(j,new PuzzleCommand("AnchorOverlay"));foreach(var area in data.UncoveredAreas){Do(j,new PuzzleCommand("ToggleUncovered",area));Do(j,new PuzzleCommand("AttachAreaEvidence",area,"rec-watchlog-bureau"));}Do(j,new PuzzleCommand("LoadRecord","rec-plate-standard-hub"));Do(j,new PuzzleCommand("Read"));Do(j,new PuzzleCommand("SetWindow",value:"H-1:00",otherValue:"H+3:00"));return j;}
  [Test] public void EncodePersistsHintLevelsUnderTheExistingFieldNameAndStaysDecodable(){
   var j=Intake();
   var doc=JournalSave.Encode(j,"hints",20000,6291456,hintLevels:new Dictionary<string,int>{["t0-b1"]=2,["t0-b2"]=0});
   var used=(JObject)doc["progress"]["hintLevelUsed"];
   Assert.AreEqual(2,(int)used["t0-b1"]);
   Assert.IsNull(used["t0-b2"],"Unrevealed beats must not be persisted");
   Assert.AreEqual(1,used.Count);
   Assert.AreEqual(j.State.StateHash,JournalSave.Decode(doc,sim).State.StateHash,"A populated hintLevelUsed must not break replay validation");
  }
  [Test] public void EncodeWithoutHintLevelsKeepsTheEmptyObjectCompatibility(){
   var doc=JournalSave.Encode(Intake(),"empty",20000,6291456);
   var used=(JObject)doc["progress"]["hintLevelUsed"];
   Assert.AreEqual(0,used.Count,"Legacy encodes keep hintLevelUsed as an empty object");
   var explicitEmpty=JournalSave.Encode(Intake(),"empty2",20000,6291456,hintLevels:new Dictionary<string,int>());
   Assert.AreEqual(0,((JObject)explicitEmpty["progress"]["hintLevelUsed"]).Count);
  }
  [Test] public void DefaultConfirmationModeIsTwoStep(){
   Assert.AreEqual("two-step",(string)T0GameSession.DefaultSettings()["confirmMode"]);
  }
  [Test] public void PreviewDifferenceSentencesDescribeCitationAndOriginalReadChanges(){
   var j=ReaderReady();
   var text=new Dictionary<string,string>{["previewFactCitation"]="{0} 인용 {1}→{2}",["previewFactCopy"]="{0} 사본 보관",["previewFactGeneric"]="{0}",["previewReadCount"]="{0} 원본 {1}→{2}"};
   Func<string,string> t=k=>text[k];Func<string,string> n=id=>"NAME:"+id;
   var cite=T0GameSession.PreviewDifferenceSentences(j.State,sim.Preview(j.State,new PuzzleCommand("CiteToBoard","rec-plate-standard-hub")),t,n);
   Assert.AreEqual(1,cite.Count);
   Assert.AreEqual("NAME:rec-plate-standard-hub 인용 H-1:00→H+3:00",cite[0]);
   var readOriginal=T0GameSession.PreviewDifferenceSentences(j.State,sim.Preview(j.State,new PuzzleCommand("ReadOriginal")),t,n);
   Assert.AreEqual(1,readOriginal.Count);
   Assert.AreEqual("NAME:rec-plate-standard-hub 원본 0→1",readOriginal[0]);
   Assert.AreEqual(0,T0GameSession.PreviewDifferenceSentences(j.State,j.State,t,n).Count,"An identical candidate yields no sentences");
  }
  [Test] public void CaseObjectiveFollowsTheBeatsTableThroughTheDisclosureGuard(){
   var beats=JObject.Parse(File.ReadAllText(Path.Combine(Application.dataPath,"_Project/Data/Tables/beats.json")));
   var records=JObject.Parse(Resources.Load<T0RuntimeConfig>("T0Runtime").records.text);
   var names=records["rows"].Select(r=>(string)r["displayNameKo"]).ToArray();
   var authored=(string)beats["rows"].First(r=>(string)r["id"]=="t0-b2")["objective"];
   Assert.AreEqual(authored,T0GameSession.CaseObjective(beats,"t0-b2",names,"fallback"),"A disclosure-clean authored objective is shown verbatim");
   StringAssert.Contains("인수 각서",(string)beats["rows"].First(r=>(string)r["id"]=="t0-b1")["objective"],"Guard precondition: the t0-b1 objective names records");
   Assert.AreEqual("fallback",T0GameSession.CaseObjective(beats,"t0-b1",names,"fallback"),"Objectives naming records fall back (CaseThread disclosure contract)");
   Assert.AreEqual("fallback",T0GameSession.CaseObjective(null,"t0-b2",names,"fallback"),"An unwired beats table preserves the legacy objective");
   Assert.AreEqual("fallback",T0GameSession.CaseObjective(beats,"no-such-beat",names,"fallback"));
  }
 }
}
#endif
