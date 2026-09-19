using System;
using System.Collections.Generic;
using System.Linq;
using Tide.Sim;
using Tide.UI;
using UnityEngine;
namespace Tide.App {
    // M26 (RFC-CX-M26-20260918) — adopted from the similar-games research (D2 / D3 / D4 / D10):
    //   D2  one "current inquiry" line under the case card: a question, never an answer, plus the media
    //       already pinned and the media still needed (icons = colour + form + position, not colour alone).
    //   D3  the hypothesis board shows a STRUCTURAL state — 미검토 / 한 매체 근거 / 서로 다른 매체 2종 / 반례 고정 —
    //       never a truth probability. "두 매체" is submission readiness, not correctness, and says so.
    //   D4  the same media silhouettes wherever a citation is listed (evidence box, board, receipt).
    //   D10 a T0 receipt (확정한 것 / 보류한 것 / 다음 질문) offered after the closing save; reopenable, idempotent.
    // Everything here is a projection of existing state plus one player-owned annotation (counter:<record>).
    public sealed partial class T0GameSession {
        public const string ReceiptOverlay="receipt";
        public const string StateUnreviewed="unreviewed",StateOneMedium="one-medium",StateTwoMedia="two-media",StateCounterexample="counterexample";
        bool M26Enabled=>m25!=null&&(m25.m26Approved||Environment.GetCommandLineArgs().Contains("--m26-resources-diagnostic"));
        Texture2D M26MediaIcon(string sourceType)=>M26Enabled?m25.MediaIcon(sourceType):null;
        Texture2D M26StateGlyph(string state)=>M26Enabled?m25.StateGlyph(state):null;
        Texture2D M26QuestionCard=>M26Enabled?m25.questionCard:null;
        Texture2D M26ReceiptEnvelope=>M26Enabled?m25.receiptEnvelope:null;

        IEnumerable<string> PinnedRecordIds()=>Definition.Records.Keys.Where(id=>Journal.State.Has("citation:"+id));
        // The T0 pin requirement names which media the closing pair must span; the strip only ever names media types.
        IEnumerable<string> RequiredMediaTypes()=>Definition.Beats.Single(b=>b.Id=="t0-b3").Requirements.Where(r=>r.Type=="citationPinned").Select(r=>Definition.Records[r.RecordId].SourceType).Distinct(StringComparer.Ordinal);
        // Structural state of the pinned set. Computed from lineage + media only; a player's own counterexample mark wins.
        public string StructuralState(){
            var pins=PinnedRecordIds().ToArray();
            if(pins.Any(id=>Journal.State.Has("counter:"+id)))return StateCounterexample;
            if(pins.Length==0)return StateUnreviewed;
            bool independent=false;
            for(int i=0;i<pins.Length&&!independent;i++)for(int j=i+1;j<pins.Length;j++)if(Definition.IsIndependentPair(pins[i],pins[j])){independent=true;break;}
            return independent?StateTwoMedia:StateOneMedium;
        }
        string InquiryText(){
            if(SignatureActive)return L("inquiry.c1-b2");
            if(PatrolActive)return L("inquiry.c1-b1");
            if(Simulation.IsComplete(Journal.State,"t0-b3"))return L("inquiry.t0-done");
            return L("inquiry."+CurrentBeat);
        }
        // D2 strip: question + pinned media + still-needed media. Shown wherever the case card is shown.
        void FillInquiry(GameScreen s){
            if(OpeningActive||AlignmentPracticeActive||overlay!=null)return;
            s.Inquiry=InquiryText();
            if(SignatureActive||PatrolActive)return;
            var pinnedTypes=PinnedRecordIds().Select(id=>Definition.Records[id].SourceType).Distinct(StringComparer.Ordinal).ToArray();
            foreach(var type in pinnedTypes)s.InquiryPinned.Add(new ScreenFigure{Texture=M26MediaIcon(type),Caption=ReviewMediaName(type),Aspect=1});
            foreach(var type in RequiredMediaTypes().Where(t=>!pinnedTypes.Contains(t)))s.InquiryNeeded.Add(new ScreenFigure{Texture=M26MediaIcon(type),Caption=ReviewMediaName(type),Aspect=1});
            s.InquiryBacking=M26QuestionCard;
        }
        // Figure caption = media name only (fits a 72 px cell); the record name goes in the section body.
        ScreenFigure MediaFigure(string recordId)=>new ScreenFigure{Texture=M26MediaIcon(Definition.Records[recordId].SourceType),Caption=ReviewMediaName(Definition.Records[recordId].SourceType),Aspect=1};
        string MediaLine(string recordId)=>Name(recordId)+" · "+ReviewMediaName(Definition.Records[recordId].SourceType);
        // D4: the evidence box lists pinned citations with their media silhouette (T0 only; C1 keeps its own anonymous surfaces).
        void AddEvidenceMediaStrip(GameScreen s){
            if(PatrolActive||SignatureActive)return;
            var pins=PinnedRecordIds().ToArray();if(pins.Length==0)return;
            var strip=new ScreenSection{Heading=L("evidenceMedia"),Body=string.Join("\n",pins.Select(MediaLine)),FigureHeight=72};
            foreach(var id in pins)strip.Figures.Add(MediaFigure(id));
            s.Sections.Add(strip);
        }
        // D3: the hypothesis board = structural state + legend + one toggle per pinned citation. No verdicts.
        void HypothesisBoard(GameScreen s){
            if(PatrolActive||SignatureActive)return;
            var state=StructuralState();
            var current=new ScreenSection{Heading=L("boardState"),Body=L("state."+state)+"\n"+L("boardCaveat"),FigureHeight=72};
            current.Figures.Add(new ScreenFigure{Texture=M26StateGlyph(state),Caption=L("state."+state+".short"),Aspect=1});
            s.Sections.Add(current);
            var legend=new ScreenSection{Heading=L("boardLegend"),FigureHeight=56};
            foreach(var id in new[]{StateUnreviewed,StateOneMedium,StateTwoMedia,StateCounterexample})legend.Figures.Add(new ScreenFigure{Texture=M26StateGlyph(id),Caption=L("state."+id+".short"),Aspect=1});
            if(legend.Figures.Any(f=>f.Texture!=null))s.Sections.Add(legend);
            foreach(var id in PinnedRecordIds()){
                var rid=id;bool marked=Journal.State.Has("counter:"+rid);
                s.Actions.Add(A("counter-"+rid,(marked?"✓ ":"")+string.Format(L(marked?"counterClear":"counterMark"),Name(rid)),()=>SubmitImmediate(new PuzzleCommand("MarkCounterexample",rid)),!SavePending,L("counterDetail")));
            }
        }
        // D10: the T0 receipt. Settled = what the closing save holds; deferred = the player's own counterexample marks;
        // next = the C1 question without any name that C1 has not yet disclosed.
        void ReceiptScreen(GameScreen s){
            s.Title=L("receiptTitle");s.ResetWorkScroll=true;
            var state=Journal.State;
            var settled=new List<string>();
            foreach(var id in PinnedRecordIds())settled.Add(string.Format(L("receiptCitation"),Name(id),state.Get("citationStart:"+id),state.Get("citationEnd:"+id)));
            foreach(var pair in state.ValueSnapshot.Where(p=>p.Key.StartsWith("decision:",StringComparison.Ordinal)))settled.Add(string.Format(L("receiptDecision"),L(pair.Value)));
            var deferred=PinnedRecordIds().Where(id=>state.Has("counter:"+id)).Select(id=>string.Format(L("receiptDeferred"),Name(id))).ToList();
            s.Body=L("receiptIntro");
            var envelope=new ScreenSection{FigureHeight=150};
            envelope.Figures.Add(new ScreenFigure{Texture=M26ReceiptEnvelope,Caption=L("receiptEnvelope"),Aspect=4f/3f});
            if(envelope.Figures.All(f=>f.Texture==null))envelope.Figures.Clear();
            s.Sections.Add(envelope);
            s.Sections.Add(new ScreenSection{Heading=L("receiptSettled"),Body=settled.Count==0?L("receiptNone"):string.Join("\n",settled)});
            s.Sections.Add(new ScreenSection{Heading=L("receiptHeld"),Body=deferred.Count==0?L("receiptNoneHeld"):string.Join("\n",deferred)});
            s.Sections.Add(new ScreenSection{Heading=L("receiptNext"),Body=L("inquiry.t0-next")});
            var strip=new ScreenSection{Heading=L("evidenceMedia"),Body=string.Join("\n",PinnedRecordIds().Select(MediaLine)),FigureHeight=72};
            foreach(var id in PinnedRecordIds())strip.Figures.Add(MediaFigure(id));
            if(strip.Figures.Count>0)s.Sections.Add(strip);
            s.Actions.Add(A("receipt-board",L("hypothesis"),()=>OpenOverlay("hypothesis")));
            s.Actions.Add(A("receipt-continue","C1 · 두 개의 필적으로 이동",()=>{overlay=null;ContinueToPatrol();},!SavePending));
        }
    }
}
