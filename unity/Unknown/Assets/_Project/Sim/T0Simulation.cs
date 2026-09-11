using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace Tide.Sim
{
    public sealed class T0Simulation
    {
        private readonly T0Definition data;
        public T0Simulation(T0Definition definition) { data=definition??throw new ArgumentNullException(nameof(definition)); }

        public ValidationResult Validate(PuzzleState state,PuzzleCommand command)
        {
            if(state==null || command==null) throw new ArgumentNullException();
            if(C1SignatureDefinition.Handles(command.CommandId))return data.Signature==null?ValidationResult.InvalidData("C1_SIGNATURE_NOT_PACKAGED"):data.Signature.Validate(state,command,IsComplete(state,C1PatrolDefinition.BeatId));
            if(C1PatrolDefinition.Handles(command.CommandId))return data.Patrol==null?ValidationResult.InvalidData("C1_NOT_PACKAGED"):data.Patrol.Validate(state,command,IsComplete(state,"t0-b3"));
            var id=command.SubjectId;
            bool exists=id!=null && data.Records.TryGetValue(id,out _);
            var record=exists?data.Records[id]:null;
            bool visible=record!=null && record.VisibleAt.Any(b=>IsAvailable(state,b));
            switch(command.CommandId)
            {
                case "OpenTool": return Check(id=="circuit" && IsMarkingAvailable(state),"Circuit is not available");
                case "CloseTool": return Check(id=="circuit","Unknown tool");
                case "BeginOverlay": return Check(Mode(state)=="Tracing","Overlay requires tracing");
                case "SetOverlayOffset": return Check(Mode(state)=="Overlaying" && Finite(command.Value,out _) && Finite(command.OtherValue,out _),"Invalid overlay offset or state");
                case "AnchorOverlay":
                    return Check(Mode(state)=="Overlaying" && data.Overlay!=null &&
                        data.Overlay.Aligned(Number(state.Get("overlayX")),Number(state.Get("overlayY"))),"The three anchors are not aligned");
                case "Cancel": return Check(Mode(state)=="Overlaying","No circuit overlay to cancel");
                case "ViewLine": return Check(visible && record.LineIds.Contains(command.Value),"Unknown or unavailable record line");
                case "ViewRow": return Check(visible && record.RowIds.Contains(command.Value),"Unknown or unavailable record row");
                case "PlaceInSlot":
                    return Check(ActiveRequirements(state,"slotLoaded").Any(r=>r.Id==id && r.RecordId==command.Value),"Unknown or unavailable slot assignment");
                case "RecordDecision":
                    return Check(ActiveRequirements(state,"decisionRecorded").Any(r=>r.Id==id && r.Ids.Contains(command.Value)),"Unknown decision or value");
                case "TraceSystem": return Check(data.SystemIds.Contains(id) && (data.Overlay==null || Mode(state)=="Tracing"),"Unknown system or invalid trace state");
                case "ToggleUncovered":
                    return Check(IsMarkingAvailable(state) && CanMark(state) && data.UncoveredAreas.Contains(id),"Unknown or unavailable coverage area");
                case "AttachAreaEvidence":
                    return Check(IsMarkingAvailable(state) && CanMark(state) && data.UncoveredAreas.Contains(id) &&
                        data.Records.TryGetValue(command.Value??"",out var evidence) && evidence.VisibleAt.Any(b=>IsAvailable(state,b)),
                        "Unknown or unavailable area evidence");
                case "LoadRecord": return Check(visible,"Unknown or unavailable record");
                case "Read":
                    return Check(state.LoadedRecordId!=null,"Reader has no loaded record");
                case "ReadOriginal":
                    if(state.LoadedRecordId==null || !state.Has("copy:"+state.LoadedRecordId))
                        return ValidationResult.InvalidData("Original read requires a verified copy");
                    if(state.ReadCount(state.LoadedRecordId)>=data.ReadBudget)
                        return ValidationResult.Reject(ReasonCode.ORIGINAL_DEGRADED);
                    return ValidationResult.Success();
                case "SetWindow":
                    if(state.LoadedRecordId==null) return ValidationResult.InvalidData("Reader has no loaded record");
                    var phases=data.Records[state.LoadedRecordId].Phases;
                    return Check(phases.Contains(command.Value) && phases.Contains(command.OtherValue) &&
                        Index(phases,command.Value)<=Index(phases,command.OtherValue),"Unknown or reversed sample window");
                case "CiteToBoard":
                    if(!visible || state.LoadedRecordId!=id || !state.Has("copy:"+id))
                        return ValidationResult.InvalidData("Citation requires a loaded, read record");
                    // RFC-CX-001 / RFC-CX-003: only explicit authoring assignments permit citations.
                    if(!record.HasCitationProvenance)
                        return ValidationResult.InvalidData("Missing citation provenance: "+id+" systemId/stationId");
                    var start=state.Get("windowStart:"+id);
                    var end=state.Get("windowEnd:"+id);
                    if(start==null || end==null)
                        return ValidationResult.InvalidData("Citation requires an authored sample window");
                    // Sample positions come from the generated grid; no rounded or invented times.
                    if(Index(record.Phases,end)-Index(record.Phases,start)<=0)
                        return ValidationResult.Reject(ReasonCode.INDETERMINATE);
                    foreach(var cited in data.Records.Values.Where(r=>state.Has("citation:"+r.Id) && r.Id!=id))
                    {
                        if(cited.SourceType==record.SourceType) return ValidationResult.Reject(ReasonCode.MEDIA_DUPLICATE);
                        if(data.ResolveRoot(cited.Id)==data.ResolveRoot(id))
                            return ValidationResult.InvalidData("Citation pair has the same root origin");
                    }
                    return ValidationResult.Success();
                default:
                    if(data.StubToolIds.Contains(command.CommandId)) return ValidationResult.Reject(ReasonCode.NOT_IMPLEMENTED_IN_T0);
                    return ValidationResult.InvalidData("Unknown command: "+command.CommandId);
            }
        }

        public CommandResult Commit(PuzzleState state,PuzzleCommand command)
        {
            var verdict=Validate(state,command);
            if(!verdict.IsValid) return new CommandResult(verdict,Array.Empty<PuzzleEvent>());
            if(C1SignatureDefinition.Handles(command.CommandId))command=data.Signature.Materialize(state,command);
            if(C1PatrolDefinition.Handles(command.CommandId))command=data.Patrol.Materialize(command);
            IEnumerable<string> kept=null;
            if(command.CommandId=="Read") kept=data.Records[state.LoadedRecordId].ClueIds;
            if(command.CommandId=="BeginOverlay") command=new PuzzleCommand(command.CommandId,command.SubjectId,
                data.Overlay.InitialX.ToString(CultureInfo.InvariantCulture),data.Overlay.InitialY.ToString(CultureInfo.InvariantCulture));
            return new CommandResult(verdict,new[]{new PuzzleEvent(command,kept,
                command.CommandId=="CiteToBoard"?state.Get("windowStart:"+command.SubjectId):null,
                command.CommandId=="CiteToBoard"?state.Get("windowEnd:"+command.SubjectId):null)});
        }

        // Preview computes a candidate without publishing or mutating the original state.
        public PuzzleState Preview(PuzzleState state,PuzzleCommand command)
        {
            var result=Commit(state,command);
            return result.Events.Aggregate(state,Reduce);
        }

        public static PuzzleState Reduce(PuzzleState state,PuzzleEvent evt)
        {
            var facts=new HashSet<string>(state.Facts,StringComparer.Ordinal);
            var values=new Dictionary<string,string>(state.Values,StringComparer.Ordinal);
            var counts=new Dictionary<string,int>(state.Counts,StringComparer.Ordinal);
            var c=evt.Command;
            if(C1SignatureDefinition.Handles(c.CommandId))C1SignatureDefinition.Reduce(c,facts,values);
            else if(C1PatrolDefinition.Handles(c.CommandId))C1PatrolDefinition.Reduce(c,facts,values);
            else switch(c.CommandId)
            {
                case "OpenTool": values["circuitMode"]="Tracing"; break;
                case "CloseTool": values["circuitMode"]="Idle"; break;
                case "BeginOverlay": values["circuitMode"]="Overlaying"; values["overlayX"]=c.Value; values["overlayY"]=c.OtherValue; break;
                case "SetOverlayOffset": values["overlayX"]=c.Value; values["overlayY"]=c.OtherValue; break;
                case "AnchorOverlay": values["circuitMode"]="Marking"; facts.Add("overlayAligned"); break;
                case "Cancel": values["circuitMode"]="Tracing"; values.Remove("overlayX"); values.Remove("overlayY"); break;
                case "ViewLine": facts.Add("line:"+c.SubjectId+":"+c.Value); break;
                case "ViewRow": facts.Add("row:"+c.SubjectId+":"+c.Value); break;
                case "PlaceInSlot": values["slot:"+c.SubjectId]=c.Value; break;
                case "RecordDecision": values["decision:"+c.SubjectId]=c.Value; break;
                case "TraceSystem": values["traced"]=c.SubjectId; break;
                case "ToggleUncovered":
                    if(!facts.Add("area:"+c.SubjectId)) facts.Remove("area:"+c.SubjectId);
                    break;
                case "AttachAreaEvidence": facts.Add("evidence:"+c.SubjectId+":"+c.Value); break;
                case "LoadRecord": values["loaded"]=c.SubjectId; break;
                case "Read":
                    facts.Add("copy:"+state.LoadedRecordId);
                    foreach(var clue in evt.ProtectedClues) facts.Add("kept:"+clue);
                    break;
                case "ReadOriginal":
                    if(!state.IsSandbox) counts[state.LoadedRecordId]=state.ReadCount(state.LoadedRecordId)+1;
                    break;
                case "SetWindow":
                    values["windowStart:"+state.LoadedRecordId]=c.Value;
                    values["windowEnd:"+state.LoadedRecordId]=c.OtherValue;
                    break;
                case "CiteToBoard":
                    facts.Add("citation:"+c.SubjectId);
                    values["citationStart:"+c.SubjectId]=evt.CitationStart;
                    values["citationEnd:"+c.SubjectId]=evt.CitationEnd;
                    break;
                default: throw new InvalidOperationException("Cannot reduce an unsupported event");
            }
            return new PuzzleState(facts,values,counts,state.IsSandbox);
        }

        public PuzzleState Replay(IEnumerable<PuzzleCommand> commands,bool sandbox=false)
        {
            var state=new PuzzleState(sandbox);
            foreach(var command in commands)
            {
                var result=Commit(state,command);
                if(!result.Validation.IsValid) throw new InvalidOperationException("Replay validation failed: "+result.Validation.DataDiagnostic);
                state=result.Events.Aggregate(state,Reduce);
            }
            return state;
        }

        public bool IsAvailable(PuzzleState state,string beatId)
        {
            var beat=data.Beats.FirstOrDefault(b=>b.Id==beatId);
            return beat!=null && beat.Prerequisites.All(p=>IsComplete(state,p));
        }
        public bool IsComplete(PuzzleState state,string beatId)
        {
            var beat=data.Beats.FirstOrDefault(b=>b.Id==beatId);
            return beat!=null && IsAvailable(state,beatId) && beat.Requirements.All(r=>Satisfied(state,r));
        }
        // Read-only completion predicate for render-side projections (single source; CLAUDE.md §9).
        public bool IsSatisfied(PuzzleState state,CompletionRequirement requirement) => Satisfied(state,requirement);

        private bool Satisfied(PuzzleState s,CompletionRequirement r)
        {
            switch(r.Type)
            {
                case "signatureFiled": return C1SignatureDefinition.Has(s,"committed") && C1SignatureDefinition.Has(s,"copiesFiled") && C1SignatureDefinition.Has(s,"regionFiled:"+data.Signature.RegionId) && C1SignatureDefinition.Has(s,"bandFiled:"+data.Signature.ComparisonId) && C1SignatureDefinition.Has(s,"proofFiled") && s.Has("checkpoint:"+C1SignatureDefinition.CheckpointId);
                case "patrolAccessGranted": return s.Has("c1:gateAccess") && s.Has("c1:journal:"+r.Id) && s.Has("checkpoint:"+C1PatrolDefinition.CheckpointId);
                case "recordLinesViewed": return r.Ids.All(id=>s.Has("line:"+r.RecordId+":"+id));
                case "recordRowsViewed": return r.Ids.All(id=>s.Has("row:"+r.RecordId+":"+id));
                case "slotLoaded": return s.Get("slot:"+r.Id)==r.RecordId;
                case "decisionRecorded": return r.Ids.Contains(s.Get("decision:"+r.Id));
                case "uncoveredAreasMarked": return r.Ids.All(id=>s.Has("area:"+id));
                case "eachAreaHasEvidence":
                    return data.UncoveredAreas.All(area=>data.Records.Values
                        .Where(record=>s.Has("evidence:"+area+":"+record.Id)).Select(record=>record.SourceType).Distinct().Count()>=r.Count);
                case "citationPinned": return s.Has("citation:"+r.RecordId);
                case "independentPair":
                    var cited=data.Records.Keys.Where(id=>s.Has("citation:"+id)).ToArray();
                    return cited.Any(a=>cited.Any(b=>a!=b && data.IsIndependentPair(a,b)));
                case "autoCopyCreated": return s.Has("copy:"+r.RecordId);
                case "gapEndpointsFixed":
                    return s.Get("citationStart:"+r.RecordId)==r.Value && s.Get("citationEnd:"+r.RecordId)==r.OtherValue;
                default: throw new InvalidOperationException("Unsupported completion predicate: "+r.Type);
            }
        }
        private IEnumerable<CompletionRequirement> ActiveRequirements(PuzzleState s,string type) =>
            data.Beats.Where(b=>IsAvailable(s,b.Id)).SelectMany(b=>b.Requirements).Where(r=>r.Type==type);
        private bool IsMarkingAvailable(PuzzleState s) => ActiveRequirements(s,"uncoveredAreasMarked").Any();
        private bool CanMark(PuzzleState s)=>data.Overlay==null || Mode(s)=="Marking" || Mode(s)=="Resolved";
        public string CircuitMode(PuzzleState s)
        {
            var mode=s.Get("circuitMode")??"Idle";
            if(mode=="Marking" && data.UncoveredAreas.All(a=>s.Has("area:"+a) && data.Records.Keys.Any(r=>s.Has("evidence:"+a+":"+r)))) return "Resolved";
            return mode;
        }
        private string Mode(PuzzleState s)=>CircuitMode(s);
        private static bool Finite(string text,out double value)=>double.TryParse(text,NumberStyles.Float,CultureInfo.InvariantCulture,out value)&&CircuitOverlay.Finite(value);
        private static double Number(string text)=>Finite(text,out var value)?value:double.NaN;
        private static ValidationResult Check(bool valid,string diagnostic) =>
            valid?ValidationResult.Success():ValidationResult.InvalidData(diagnostic);
        private static int Index(IReadOnlyList<string> list,string value)
        { for(int i=0;i<list.Count;i++) if(list[i]==value) return i; return -1; }
    }
}
