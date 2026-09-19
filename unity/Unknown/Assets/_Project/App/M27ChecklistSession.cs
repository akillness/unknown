using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Tide.Sim;
using Tide.UI;
using UnityEngine;
namespace Tide.App {
    // M27 (RFC-CX-M27-20260918) 실마리 명료화: the current beat's requirements projected as a checklist, a stage marker,
    // and a data-driven "next" line. Everything here is a READ-ONLY projection over Simulation.IsSatisfied / IsComplete
    // (CLAUDE.md §9): it never adds a fact, never narrows a requirement, never names a record.
    // Labels/captions/steps come from beats.json completionPredicate.requires[] (emit-tables.mjs, validator T0-06);
    // C1 beats derive their items from the patrol / signature packets. No beat id literal is branched on below.
    public sealed partial class T0GameSession {
        bool M27Enabled=>m25!=null&&(m25.m27Approved||Environment.GetCommandLineArgs().Contains("--m27-resources-diagnostic"));
        Texture2D M27ChecklistGlyph(string state)=>M27Enabled?m25.ChecklistGlyph(state):null;
        static bool IsC1Beat(string id)=>id==C1PatrolDefinition.BeatId||id==C1SignatureDefinition.BeatId;
        // Stage order = definition order of the non-C1 beats (the same source BeatFor walks).
        IEnumerable<BeatDefinition> T0Beats=>Definition.Beats.Where(b=>!IsC1Beat(b.Id));
        string BeatTitle(string beatId){
            if(beatId==C1PatrolDefinition.BeatId)return PatrolText("c1.patrol.title");
            if(beatId==C1SignatureDefinition.BeatId)return SignatureText("c1.signature.title",beatId);
            var title=(string)beats?["rows"]?.FirstOrDefault(r=>(string)r["id"]==beatId)?["title"];
            return string.IsNullOrEmpty(title)?beatId:title;
        }
        // "단계 i/n · 제목" — never the raw beat id. C1 shows its own 1/2 · 2/2.
        public string StageLabel(){
            if(SignatureActive)return string.Format(L("stageC1"),2,2,BeatTitle(C1SignatureDefinition.BeatId));
            if(PatrolActive)return string.Format(L("stageC1"),1,2,BeatTitle(C1PatrolDefinition.BeatId));
            var ids=T0Beats.Select(b=>b.Id).ToList();int index=Math.Max(0,ids.IndexOf(CurrentBeat));
            return string.Format(L("stageLine"),index+1,ids.Count,BeatTitle(CurrentBeat));
        }
        public string StageShort(){
            if(SignatureActive)return string.Format(L("stageShort"),2,2)+" C1";
            if(PatrolActive)return string.Format(L("stageShort"),1,2)+" C1";
            var ids=T0Beats.Select(b=>b.Id).ToList();return string.Format(L("stageShort"),Math.Max(0,ids.IndexOf(CurrentBeat))+1,ids.Count);
        }
        // Current beat's requirements in authored step order with their player labels. Disclosure guard: a label that
        // contains a record display name falls back to its caption (same rule as CaseObjective).
        public List<ChecklistItem> CaseChecklist(){
            var items=new List<ChecklistItem>();
            if(SignatureActive){
                int observed=Definition.Signature.Observations.Count(id=>SignatureHas("observed:"+id));
                int copies=Definition.Signature.Copies.Count(id=>SignatureHas("copy:"+id));
                items.Add(new ChecklistItem{Label=string.Format(L("c1SigTaskObserve"),observed),Caption=string.Format(L("c1SigTaskObserve"),observed),Done=observed>=Definition.Signature.Observations.Count});
                items.Add(new ChecklistItem{Label=L("c1SigTaskTrial"),Caption=L("c1SigTaskTrial"),Done=SignatureValue("trial")!=null});
                items.Add(new ChecklistItem{Label=L("c1SigTaskSeparate"),Caption=L("c1SigTaskSeparate"),Done=SignatureHas("separated")});
                items.Add(new ChecklistItem{Label=string.Format(L("c1SigTaskCopies"),copies),Caption=string.Format(L("c1SigTaskCopies"),copies),Done=copies>=Definition.Signature.Copies.Count});
                items.Add(new ChecklistItem{Label=L("c1SigTaskCompare"),Caption=L("c1SigTaskCompare"),Done=SignatureComplete});
            }else if(PatrolActive){
                foreach(var item in patrolPacket["observations"]){
                    var id=(string)item["id"];var name=PatrolText((string)item["labelKey"]);
                    items.Add(new ChecklistItem{Label=string.Format(L("c1TaskObserve"),name),Caption=name,Done=Journal.State.Has("c1:observed:"+id)});
                }
                items.Add(new ChecklistItem{Label=L("c1TaskCondition"),Caption=L("c1TaskCondition"),Done=Journal.State.Get("c1:conditionCandidate")==Definition.Patrol.ConditionId});
                items.Add(new ChecklistItem{Label=L("c1TaskConfirm"),Caption=L("c1TaskConfirm"),Done=PatrolComplete});
            }else{
                var beat=Definition.Beats.FirstOrDefault(b=>b.Id==CurrentBeat);if(beat==null)return items;
                var rows=(beats?["rows"]?.FirstOrDefault(r=>(string)r["id"]==CurrentBeat)?["completionPredicate"]?["requires"] as JArray)?.ToList()??new List<JToken>();
                var names=Definition.Records.Keys.Select(Name).ToArray();
                var ordered=beat.Requirements.Select((r,i)=>(r,row:i<rows.Count?rows[i]:null,i))
                    .OrderBy(x=>(int?)x.row?["step"]??x.i+1).ThenBy(x=>x.i);
                foreach(var (r,row,i) in ordered){
                    string caption=(string)row?["caption"];string label=(string)row?["label"];
                    if(string.IsNullOrEmpty(caption))caption=r.Type;
                    if(string.IsNullOrEmpty(label)||names.Any(label.Contains))label=caption;
                    items.Add(new ChecklistItem{Label=label,Caption=caption,Done=Simulation.IsSatisfied(Journal.State,r)});
                }
            }
            var current=items.FirstOrDefault(x=>!x.Done);if(current!=null)current.Current=true;
            foreach(var item in items)item.Glyph=M27ChecklistGlyph(item.Done?"done":item.Current?"current":"open");
            return items;
        }
        public string ConditionsLine(){var items=CaseChecklist();return string.Format(L("conditionsLine"),items.Count(x=>x.Done),items.Count);}
        // T0 progress lines: "조건 d/t" always; "필요한 인용 n/m" only for a beat that requires citations (data-driven).
        string ProgressLine(){
            var beat=Definition.Beats.FirstOrDefault(b=>b.Id==CurrentBeat);
            var pins=beat==null?Array.Empty<CompletionRequirement>():beat.Requirements.Where(r=>r.Type=="citationPinned").ToArray();
            string line=ConditionsLine();
            if(pins.Length>0)line+=" · "+string.Format(L("caseProgress"),pins.Count(r=>Simulation.IsSatisfied(Journal.State,r)),pins.Length);
            return line;
        }
        // "다음:" = the first open requirement's label; every T0 beat complete → review copy; saving → wait copy.
        string CaseThreadNext(){
            if(SavePending)return L("caseWait");
            if(T0Beats.All(b=>Simulation.IsComplete(Journal.State,b.Id)))return L("caseReview");
            var next=CaseChecklist().FirstOrDefault(x=>x.Current);
            return next!=null?next.Label:L("caseReview");
        }
        string CaseThreadText(){
            if(SignatureActive)return SignatureCaseThread();
            if(PatrolActive)return PatrolCaseThread();
            bool complete=T0Beats.All(b=>Simulation.IsComplete(Journal.State,b.Id));
            return L("caseTitle")+" · "+StageLabel()+"\n"+(complete?L("caseComplete"):ProgressLine())+"\n"
                +CaseObjective(beats,CurrentBeat,Definition.Records.Keys.Select(Name),L("caseObjective"))+"\n"+L("caseNext")+CaseThreadNext();
        }
        // Resume copy names the stage and the open-condition count instead of a generic sentence (C6-F4).
        string ResumeStatus(){
            if(SignatureActive)return SignatureComplete?"저장된 대조 기록을 복원했습니다.":"저장된 서명지 작업을 복원했습니다.";
            if(PatrolActive)return "저장된 순찰 기록을 복원했습니다.";
            if(T0Beats.All(b=>Simulation.IsComplete(Journal.State,b.Id)))return L("caseReview");
            if(!T0Beats.Any(b=>Simulation.IsComplete(Journal.State,b.Id)))return L("welcome");
            return string.Format(L("resumeStage"),L("resumeInProgress"),StageLabel()+" · "+ConditionsLine());
        }
        void FillChecklist(GameScreen s){
            s.CaseStage=StageLabel();
            s.Checklist.Clear();
            foreach(var item in CaseChecklist())s.Checklist.Add(item);
        }
    }
}
