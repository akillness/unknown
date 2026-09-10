#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using Tide.App;
using Tide.Data;
using Tide.Sim;
using UnityEditor;
using UnityEngine;

namespace Tide.EditorTools
{
    public static class T0Verification
    {
        public sealed class CaseResult { public string Name; public string Failure; }
        private static void Assert(bool condition,string message) { if(!condition) throw new InvalidOperationException(message); }
        private static void Reject(Action action)
        {
            bool rejected=false;
            try { action(); } catch(InvalidOperationException) { rejected=true; }
            Assert(rejected,"Invalid input was accepted");
        }
        private static Dictionary<string,byte[]> Files() => Directory.GetFiles(T0AssetImporter.TablePath,"*.json")
            .Where(p=>Path.GetFileName(p)!="tables-receipt.json").ToDictionary(Path.GetFileName,File.ReadAllBytes,StringComparer.Ordinal);

        public static IReadOnlyList<CaseResult> RunAll()
        {
            var results=new List<CaseResult>();
            Action<string,Action> test=(name,run)=>
            {
                var result=new CaseResult{Name=name};
                try { run(); } catch(Exception e) { result.Failure=e.ToString(); }
                results.Add(result);
            };
            var files=Files();
            var receipt=File.ReadAllBytes(T0AssetImporter.TablePath+"/tables-receipt.json");
            var data=T0DataLoader.LoadVerified(files,receipt,receipt);
            var sim=new T0Simulation(data);
            var sequence=ExplorationAndCircuit(data).ToArray();
            var throughB2=sim.Replay(sequence);

            test("T01_Reduce_does_not_mutate_input",()=>
            {
                var initial=new PuzzleState(); var hash=initial.StateHash;
                var evt=sim.Commit(initial,sequence[0]).Events.Single();
                var next=T0Simulation.Reduce(initial,evt);
                Assert(initial.StateHash==hash && next.StateHash!=hash,"Reducer mutated input or failed to apply event");
            });
            test("T02_Command_replay_is_deterministic",()=>
                Assert(sim.Replay(sequence).StateHash==sim.Replay(sequence).StateHash,"Replay hash differs"));
            test("T03_Preview_is_repeatable_and_has_no_side_effects",()=>
            {
                var state=new PuzzleState(); var hash=state.StateHash;
                var a=sim.Preview(state,sequence[0]); var b=sim.Preview(state,sequence[0]);
                Assert(state.StateHash==hash && a.StateHash==b.StateHash,"Preview mutated original or is nondeterministic");
            });
            test("T04_Invalid_commit_emits_zero_events",()=>
            {
                var result=sim.Commit(new PuzzleState(),new PuzzleCommand("ViewLine","unknown","line"));
                Assert(!result.Validation.IsValid && result.Events.Count==0,"Invalid commit produced events");
                foreach(var stub in data.StubToolIds)
                {
                    result=sim.Commit(new PuzzleState(),new PuzzleCommand(stub));
                    Assert(result.Validation.Reason==ReasonCode.NOT_IMPLEMENTED_IN_T0 && result.Events.Count==0,"Stub changed state");
                }
            });
            test("T0_Actual_data_satisfies_exploration_and_marking_predicates",()=>
            {
                Assert(sim.IsComplete(throughB2,"t0-b1"),"Exploration predicate incomplete");
                Assert(sim.IsComplete(throughB2,"t0-b2"),"Circuit predicate incomplete");
                Assert(!sim.IsComplete(throughB2,"t0-b3"),"Unplayed reader falsely completed");
            });
            test("T0_Prerequisites_prevent_early_reader_access",()=>
            {
                var result=sim.Commit(new PuzzleState(),new PuzzleCommand("LoadRecord","rec-tide-ledger-bureau"));
                Assert(!result.Validation.IsValid && result.Events.Count==0,"Later record accessible before prerequisites");
            });
            test("T0_Unmarking_coverage_revokes_completion",()=>
            {
                var changed=Apply(sim,throughB2,new PuzzleCommand("ToggleUncovered",data.UncoveredAreas[0]));
                Assert(!sim.IsComplete(changed,"t0-b2") && !sim.IsComplete(changed,"t0-b3"),"Marking predicate latched permanently");
                Assert(sim.IsComplete(throughB2,"t0-b2"),"Prior snapshot changed");
            });
            test("T05_Seeded_10000_steps_preserve_auto_kept_clues",()=>
            {
                var state=Apply(sim,throughB2,new PuzzleCommand("LoadRecord","rec-plate-standard-hub"));
                state=Apply(sim,state,new PuzzleCommand("Read"));
                var random=new System.Random(271828); // Test-only deterministic seed; Sim has no RNG.
                for(int i=0;i<10000;i++)
                {
                    var clues=state.AutoKeptClues.ToArray(); var hash=state.StateHash;
                    var commands=new[]{new PuzzleCommand("Read"),new PuzzleCommand("ToggleUncovered",data.UncoveredAreas[random.Next(data.UncoveredAreas.Count)]),
                        new PuzzleCommand("CiteToBoard","rec-plate-standard-hub"),new PuzzleCommand("ViewLine","unknown","bad"),
                        new PuzzleCommand("SetWindow",value:"H-1:00",otherValue:"H+3:00")};
                    var result=sim.Commit(state,commands[random.Next(commands.Length)]);
                    state=result.Events.Aggregate(state,T0Simulation.Reduce);
                    Assert(clues.All(state.AutoKeptClues.Contains),"Protected clues shrank at step "+i);
                    if(!result.Validation.IsValid) Assert(state.StateHash==hash,"Rejected command changed state");
                }
                Assert(state.ReadCount("rec-plate-standard-hub")==0,"Free reading consumed original state");
            });
            test("Reader_first_read_creates_copy_and_repeated_read_is_immutable",()=>
            {
                var state=Apply(sim,throughB2,new PuzzleCommand("LoadRecord","rec-plate-standard-hub"));
                var originalHash=state.StateHash;
                state=Apply(sim,state,new PuzzleCommand("Read"));
                Assert(state.Has("copy:rec-plate-standard-hub"),"First read did not create a copy");
                var copyHash=state.StateHash;
                state=Apply(sim,state,new PuzzleCommand("Read"));
                Assert(state.StateHash==copyHash && state.ReadCount("rec-plate-standard-hub")==0,"Repeat read changed protected content");
                Assert(originalHash!=copyHash,"First read did not change state");
            });
            test("Reader_original_counter_uses_data_cap_without_destroying_copy",()=>
            {
                var state=Apply(sim,throughB2,new PuzzleCommand("LoadRecord","rec-plate-standard-hub"));
                state=Apply(sim,state,new PuzzleCommand("Read"));
                for(int i=0;i<data.ReadBudget;i++) state=Apply(sim,state,new PuzzleCommand("ReadOriginal"));
                var denied=sim.Commit(state,new PuzzleCommand("ReadOriginal"));
                Assert(denied.Events.Count==0 && denied.Validation.Reason==ReasonCode.ORIGINAL_DEGRADED,"Counter cap ignored");
                Assert(sim.Commit(state,new PuzzleCommand("Read")).Validation.IsValid && state.Has("copy:rec-plate-standard-hub"),"Original cap blocked free copy");
            });
            test("Reader_sandbox_original_read_does_not_consume_counter",()=>
            {
                var state=sim.Replay(sequence,true);
                state=Apply(sim,state,new PuzzleCommand("LoadRecord","rec-plate-standard-hub"));
                state=Apply(sim,state,new PuzzleCommand("Read"));
                for(int i=0;i<data.ReadBudget+1;i++) state=Apply(sim,state,new PuzzleCommand("ReadOriginal"));
                Assert(state.ReadCount("rec-plate-standard-hub")==0,"Sandbox consumed original state");
            });
            test("Reader_window_requires_authored_grid_and_order",()=>
            {
                var state=Apply(sim,throughB2,new PuzzleCommand("LoadRecord","rec-plate-standard-hub"));
                Assert(!sim.Validate(state,new PuzzleCommand("SetWindow",value:"H-0:01",otherValue:"H+3:00")).IsValid,"Off-grid sample accepted");
                Assert(!sim.Validate(state,new PuzzleCommand("SetWindow",value:"H+3:00",otherValue:"H-1:00")).IsValid,"Reversed sample window accepted");
                state=Apply(sim,state,new PuzzleCommand("SetWindow",value:"H-1:00",otherValue:"H+3:00"));
                Assert(state.Get("windowStart:rec-plate-standard-hub")=="H-1:00","Authored window not selected");
            });
            test("T21_Synthetic_copy_lineage_is_not_independent",()=>
            {
                var synthetic=LineageFixture();
                Assert(!synthetic.IsIndependentPair("original","scan"),"Copied evidence became independent");
                Assert(!synthetic.IsIndependentPair("original","anotherPlate"),"Same-medium evidence accepted");
                Assert(synthetic.IsIndependentPair("original","ledger"),"Independent synthetic pair rejected");
            });
            test("Runtime_lineage_cycle_and_orphan_are_rejected",()=>
            {
                var cycle=new T0Definition(new[]{Fixture("a","plate","oa","b"),Fixture("b","log","ob","a")},Array.Empty<BeatDefinition>(),Array.Empty<string>(),Array.Empty<string>(),Array.Empty<string>(),data.ReadBudget,data.ResolutionMinutes);
                Reject(()=>cycle.ResolveRoot("a"));
                var orphan=new T0Definition(new[]{Fixture("a","plate","oa","missing")},Array.Empty<BeatDefinition>(),Array.Empty<string>(),Array.Empty<string>(),Array.Empty<string>(),data.ReadBudget,data.ResolutionMinutes);
                Reject(()=>orphan.ResolveRoot("a"));
            });
            test("Canonical_provenance_and_negative_missing_fixture",()=>
            {
                Assert(T0DataLoader.CitationBlockers(data).Count==0,"Approved source assignments not imported");
                var missing=new T0Definition(data.Records.Values.Select(r=>new RecordDefinition(r.Id,r.SourceType,r.OriginId,r.RootOriginId,r.CopiedFrom,null,null,r.ClueIds,r.LineIds,r.RowIds,r.Phases,r.VisibleAt)),data.Beats,data.UncoveredAreas,data.SystemIds,data.StubToolIds,data.ReadBudget,data.ResolutionMinutes,data.Overlay);
                var missingSim=new T0Simulation(missing);
                var state=Apply(missingSim,throughB2,new PuzzleCommand("LoadRecord","rec-plate-standard-hub"));
                state=Apply(missingSim,state,new PuzzleCommand("Read"));
                var result=missingSim.Commit(state,new PuzzleCommand("CiteToBoard","rec-plate-standard-hub"));
                Assert(!result.Validation.IsValid && result.Events.Count==0 && result.Validation.DataDiagnostic.Contains("Missing citation provenance"),"Missing provenance guessed");
                Assert(result.Validation.Reason==null,"A new gameplay reason invented");
            });
            test("Synthetic_provenance_citation_captures_window_at_commit",()=>
            {
                // Synthetic metadata exists only in this fixture; generated source bytes remain untouched.
                var synthetic=new T0Definition(data.Records.Values.Select(r=>new RecordDefinition(r.Id,r.SourceType,r.OriginId,r.RootOriginId,
                    r.CopiedFrom,"synthetic-system","synthetic-station",r.ClueIds,r.LineIds,r.RowIds,r.Phases,r.VisibleAt)),
                    data.Beats,data.UncoveredAreas,data.SystemIds,data.StubToolIds,data.ReadBudget,data.ResolutionMinutes,data.Overlay);
                var engine=new T0Simulation(synthetic);
                var state=engine.Replay(sequence);
                state=Apply(engine,state,new PuzzleCommand("LoadRecord","rec-plate-standard-hub"));
                state=Apply(engine,state,new PuzzleCommand("Read"));
                Assert(!engine.Validate(state,new PuzzleCommand("CiteToBoard","rec-plate-standard-hub")).IsValid,"Citation without a window was accepted");
                state=Apply(engine,state,new PuzzleCommand("SetWindow",value:"H-1:00",otherValue:"H-1:00"));
                Assert(engine.Validate(state,new PuzzleCommand("CiteToBoard","rec-plate-standard-hub")).Reason==ReasonCode.INDETERMINATE,"Zero-width time order accepted");
                state=Apply(engine,state,new PuzzleCommand("SetWindow",value:"H-0:56",otherValue:"H+3:00"));
                state=Apply(engine,state,new PuzzleCommand("CiteToBoard","rec-plate-standard-hub"));
                state=Apply(engine,state,new PuzzleCommand("SetWindow",value:"H-1:00",otherValue:"H+3:00"));
                Assert(state.Get("citationStart:rec-plate-standard-hub")=="H-0:56","Window edit silently rewrote pinned citation");
                state=Apply(engine,state,new PuzzleCommand("LoadRecord","rec-tide-ledger-bureau"));
                state=Apply(engine,state,new PuzzleCommand("Read"));
                state=Apply(engine,state,new PuzzleCommand("SetWindow",value:"H-1:00",otherValue:"H+3:00"));
                state=Apply(engine,state,new PuzzleCommand("CiteToBoard","rec-tide-ledger-bureau"));
                Assert(!engine.IsComplete(state,"t0-b3"),"Changing preview window retroactively completed b3");
                state=Apply(engine,state,new PuzzleCommand("LoadRecord","rec-plate-standard-hub"));
                state=Apply(engine,state,new PuzzleCommand("CiteToBoard","rec-plate-standard-hub"));
                Assert(engine.IsComplete(state,"t0-b3"),"Explicit corrected re-citation did not complete synthetic b3");
            });
            test("V1_Producer_failure_receipt_is_rejected",()=>
            {
                var bad=Encoding.UTF8.GetBytes(Encoding.UTF8.GetString(receipt).Replace("\"verdict\": \"PASS\"","\"verdict\": \"FAIL\""));
                Reject(()=>ReceiptVerifier.Verify(files,bad,bad));
            });
            test("V2_V3_All_five_byte_tamper_fixtures_are_rejected",()=>
            {
                foreach(var name in files.Keys)
                {
                    var bad=new Dictionary<string,byte[]>(files);
                    bad[name]=(byte[])files[name].Clone(); bad[name][0]^=1;
                    Reject(()=>ReceiptVerifier.Verify(bad,receipt,receipt));
                }
            });
            test("V4_Changed_contract_is_rejected_but_timestamp_is_ignored",()=>
            {
                var text=Encoding.UTF8.GetString(receipt);
                var timestamp=JsonUtility.FromJson<ReceiptJson>(text).emittedUtc;
                ReceiptVerifier.Verify(files,Encoding.UTF8.GetBytes(text.Replace(timestamp,"2000-01-01T00:00:00Z")),receipt);
                Reject(()=>ReceiptVerifier.Verify(files,Encoding.UTF8.GetBytes(text.Replace("\"scope\": \"t0\"","\"scope\": \"all\"")),receipt));
            });
            test("Replay_invalid_command_stops_instead_of_skipping",()=>
                Reject(()=>sim.Replay(sequence.Concat(new[]{new PuzzleCommand("unknown")}))));
            test("State_hash_is_culture_independent",()=>
            {
                var before=System.Globalization.CultureInfo.CurrentCulture;
                try
                {
                    System.Globalization.CultureInfo.CurrentCulture=new System.Globalization.CultureInfo("tr-TR");
                    var hash=sim.Replay(sequence).StateHash;
                    System.Globalization.CultureInfo.CurrentCulture=new System.Globalization.CultureInfo("ko-KR");
                    Assert(sim.Replay(sequence).StateHash==hash,"Culture changed state hash");
                }
                finally { System.Globalization.CultureInfo.CurrentCulture=before; }
            });
            return results;
        }

        private static PuzzleState Apply(T0Simulation sim,PuzzleState state,PuzzleCommand command)
        {
            var result=sim.Commit(state,command);
            Assert(result.Validation.IsValid,result.Validation.DataDiagnostic??result.Validation.Reason?.ToString());
            return result.Events.Aggregate(state,T0Simulation.Reduce);
        }
        private static IEnumerable<PuzzleCommand> ExplorationAndCircuit(T0Definition data)
        {
            foreach(var r in data.Beats[0].Requirements)
                switch(r.Type)
                {
                    case "recordLinesViewed": foreach(var id in r.Ids) yield return new PuzzleCommand("ViewLine",r.RecordId,id); break;
                    case "recordRowsViewed": foreach(var id in r.Ids) yield return new PuzzleCommand("ViewRow",r.RecordId,id); break;
                    case "slotLoaded": yield return new PuzzleCommand("PlaceInSlot",r.Id,r.RecordId); break;
                    case "decisionRecorded": yield return new PuzzleCommand("RecordDecision",r.Id,r.Ids[0]); break;
                }
            if(data.Overlay!=null) yield return new PuzzleCommand("OpenTool","circuit");
            yield return new PuzzleCommand("TraceSystem",data.SystemIds[0]);
            if(data.Overlay!=null) {
                yield return new PuzzleCommand("BeginOverlay");
                var anchor=data.Overlay.Anchors[0];
                yield return new PuzzleCommand("SetOverlayOffset",value:(anchor.TargetX-anchor.OverlayX).ToString(System.Globalization.CultureInfo.InvariantCulture),otherValue:(anchor.TargetY-anchor.OverlayY).ToString(System.Globalization.CultureInfo.InvariantCulture));
                yield return new PuzzleCommand("AnchorOverlay");
            }
            foreach(var area in data.UncoveredAreas)
            {
                yield return new PuzzleCommand("ToggleUncovered",area);
                yield return new PuzzleCommand("AttachAreaEvidence",area,"rec-watchlog-bureau");
            }
        }
        private static RecordDefinition Fixture(string id,string medium,string origin,string copied=null) =>
            new RecordDefinition(id,medium,origin,null,copied,"synthetic-system","synthetic-station",
                Array.Empty<string>(),null,null,null,null);
        private static T0Definition LineageFixture() =>
            new T0Definition(new[]{Fixture("original","plate","root-a"),Fixture("scan","log","scan-label","original"),
                Fixture("anotherPlate","plate","root-b"),Fixture("ledger","ledger","root-c")},
                Array.Empty<BeatDefinition>(),Array.Empty<string>(),Array.Empty<string>(),Array.Empty<string>(),0,1);

        public static void RunBatch()
        {
            int code=1;
            try
            {
                T0AssetImporter.Import();
                var results=RunAll();
                var failures=results.Count(r=>r.Failure!=null);
                var dir=Path.GetFullPath(Path.Combine(Application.dataPath,"../results"));
                Directory.CreateDirectory(dir);
                var xml=new StringBuilder("<testsuite name=\"T0 M1 native editor contract checks\" tests=\""+results.Count+"\" failures=\""+failures+"\">");
                foreach(var r in results)
                {
                    xml.Append("<testcase name=\"").Append(SecurityElement.Escape(r.Name)).Append("\">");
                    if(r.Failure!=null) xml.Append("<failure>").Append(SecurityElement.Escape(r.Failure)).Append("</failure>");
                    xml.Append("</testcase>");
                }
                xml.Append("</testsuite>");
                File.WriteAllText(Path.Combine(dir,"t0-m1-contract-checks.xml"),xml.ToString());
                Debug.Log("T0_M1_CHECKS tests="+results.Count+" failures="+failures+"; full T0 remains blocked by provenance and unfinished UI/save.");
                foreach(var r in results.Where(r=>r.Failure!=null)) Debug.LogError(r.Name+": "+r.Failure);
                code=failures==0?0:1;
            }
            catch(Exception e) { Debug.LogException(e); }
            EditorApplication.Exit(code);
        }
    }
}
#endif
