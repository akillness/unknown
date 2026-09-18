using System;
using System.Linq;
using Tide.Presentation;
using Tide.UI;
using UnityEngine;
using UnityEngine.Video;
namespace Tide.App {
    // M25 (RFC-CX-M25-20260918): Higgsfield resources behind one gate + the `guide` overlay.
    // The profile only decorates: it never touches puzzle state, saves, hint levels or beat data.
    public sealed partial class T0GameSession {
        M25ResourceProfile m25;
        public const string GuideOverlay="guide";
        static readonly string[] GuideToolIds={"circuit","reader","alignment","routing","corrosion","seal"};
        bool M25Enabled=>m25!=null&&(m25.runtimeApproved||Environment.GetCommandLineArgs().Contains("--m25-resources-diagnostic"));
        void BindM25Resources(){m25=Resources.Load<M25ResourceProfile>("M25Resources");}
        Texture2D M25OpeningImage=>M25Enabled?m25.openingImage:null;
        VideoClip M25OpeningClip=>M25Enabled&&!ReducedMotion?m25.openingClip:null;
        Texture2D M25StartBackdrop=>M25Enabled?m25.startBackdrop:null;
        Texture2D M25ToolIcon(string id)=>M25Enabled?m25.ToolIcon(id):null;
        Texture2D M25Portrait(string id)=>M25Enabled?m25.Portrait(id):null;
        Texture2D M25ZoneBackdrop(string id)=>M25Enabled?m25.ZoneBackdrop(id):null;
        // Tool wheel: one icon per canonical tool above the six wheel actions (colour + form + position, not colour alone).
        void AddToolWheelFigures(GameScreen s){
            var section=new ScreenSection{FigureHeight=96};
            foreach(var id in GuideToolIds)section.Figures.Add(new ScreenFigure{Texture=M25ToolIcon(id),Caption=L("action."+id),Aspect=1});
            if(section.Figures.Any(f=>f.Texture!=null))s.Sections.Add(section);
        }
        // Guided teaching header (S-D, RFC-CX-011): the level-1 hint stays data-owned; M25 only structures the frame around it.
        string TeachingHeader(string beat,int remaining)=>string.Format(L("teachingHeader"),beat,remaining);
        // The guide gathers what a new player needs on one surface without revealing anything past the current beat:
        // objective (data-owned, record-name guarded), next action, tool procedures, controls, rules, T0-public cast.
        void GuideScreen(GameScreen s){
            s.Title=L("guide");s.ResetWorkScroll=true;
            var state=Journal.State;
            string stage=SignatureActive?L("guideStageC1"):PatrolActive?L("guideStageC1"):Simulation.IsComplete(state,"t0-b3")?L("caseComplete"):CurrentBeat;
            var objective=SignatureActive||PatrolActive?L("guideC1Objective"):CaseObjective(beats,CurrentBeat,Definition.Records.Keys.Select(Name),L("caseObjective"));
            s.Body=L("guideIntro");
            var now=new ScreenSection{Heading=L("guideNow")+" · "+stage,Body=objective+"\n"+L("caseNext")+(SignatureActive||PatrolActive?L("guideC1Next"):CaseThreadNext())};
            s.Sections.Add(now);
            var steps=new ScreenSection{Heading=L("guideSteps"),Body=string.Join("\n",new[]{
                Step(1,Simulation.IsComplete(state,"t0-b1"),L("guideStep1")),
                Step(2,Simulation.IsComplete(state,"t0-b2"),L("guideStep2")),
                Step(3,Simulation.IsComplete(state,"t0-b3"),L("guideStep3"))})};
            s.Sections.Add(steps);
            var tools=new ScreenSection{Heading=L("guideTools"),Body=L("guideToolCircuit")+"\n"+L("guideToolReader")+"\n"+L("guideToolLater"),FigureHeight=96};
            foreach(var id in GuideToolIds)tools.Figures.Add(new ScreenFigure{Texture=M25ToolIcon(id),Caption=L("action."+id),Aspect=1});
            if(tools.Figures.All(f=>f.Texture==null))tools.Figures.Clear();
            s.Sections.Add(tools);
            s.Sections.Add(new ScreenSection{Heading=L("guideControls"),Body=ControlFooter()+"\n"+L("guideControlsDetail")});
            s.Sections.Add(new ScreenSection{Heading=L("guideRules"),Body=L("guideRule1")+"\n"+L("guideRule2")+"\n"+L("guideRule3")+"\n"+L("guideRule4")});
            var cast=new ScreenSection{Heading=L("guideCast"),Body=L("guideCastDetail"),FigureHeight=150};
            foreach(var id in new[]{"seorin","doyeon"})cast.Figures.Add(new ScreenFigure{Texture=M25Portrait(id),Caption=L("cast."+id),Aspect=1});
            if(cast.Figures.All(f=>f.Texture==null))cast.Figures.Clear();
            s.Sections.Add(cast);
            if(m25!=null&&M25Enabled&&m25.zoneIds.Length>0){
                var zones=new ScreenSection{Heading=L("guideZones"),Body=L("guideZonesDetail"),FigureHeight=100};
                for(int i=0;i<m25.zoneIds.Length;i++)zones.Figures.Add(new ScreenFigure{Texture=m25.ZoneBackdrop(m25.zoneIds[i]),Caption=i<m25.zoneLabels.Length?m25.zoneLabels[i]:m25.zoneIds[i],Aspect=16f/9f});
                if(zones.Figures.Any(f=>f.Texture!=null))s.Sections.Add(zones);
            }
            s.Actions.Add(A("guide-hints",L("hints"),()=>OpenOverlay("hints")));
            s.Actions.Add(A("guide-settings",L("settings"),()=>OpenOverlay("settings")));
            if(!started)s.Actions.Add(A("guide-start",L("start"),()=>{overlay=null;BeginOpeningOrStart();}));
        }
        static string Step(int number,bool done,string text)=>(done?"✓ ":"○ ")+number+". "+text;
    }
}
