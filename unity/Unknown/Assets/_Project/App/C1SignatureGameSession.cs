using System.Linq;
using Newtonsoft.Json.Linq;
using Tide.Sim;
using Tide.UI;
using UnityEngine;

namespace Tide.App
{
    public sealed partial class T0GameSession
    {
        JObject signaturePacket;
        string signatureSurface;
        string signatureProofLeft,signatureProofRight,signatureProofContext;
        void ResetSignatureProofSelection(){signatureProofLeft=signatureProofRight=signatureProofContext=null;}
        void PrepareSignatureSurface(GameScreen s,string key){s.ResetScroll=signatureSurface!=key;signatureSurface=key;}
        public bool SignatureActive=>C1SignatureDefinition.Has(Journal.State,"entered");
        public bool SignatureComplete=>Simulation.IsComplete(Journal.State,C1SignatureDefinition.BeatId);
        string SignatureText(string key,string fallback=null)=>(string)signaturePacket["localization"]?[key]??fallback??L("unavailable");
        bool SignatureHas(string key)=>C1SignatureDefinition.Has(Journal.State,key);
        string SignatureValue(string key)=>C1SignatureDefinition.Get(Journal.State,key);
        public void ContinueToSignature(){if(AlignmentPracticeActive)return;signatureSurface=null;CloseTool();document=null;overlay=null;SubmitImmediate(new PuzzleCommand("EnterSignature"));}
        PuzzleCommand SignatureConfirmation()=>new PuzzleCommand("ConfirmSignature",Definition.Signature.ComparisonId,Definition.Signature.RegionId);
        string SignatureCaseThread(){
            string context=SignatureComplete?"번호대 연결 기록 · 아래쪽은 미해결":SignatureValue("humidity")!=null?SignatureText("c1.signature.humidity."+SignatureValue("humidity"))+" · "+(SignatureValue("trial")=="safe"?"가장자리 풀림":SignatureValue("trial")=="risk"?"잉크 번짐 위험":"시험 전"):SignatureHas("separated")?"두 사본과 판의 번호대를 확인한다.":"기록을 살펴보고 습도 시험을 선택한다.";
            return "C1 · "+SignatureText("c1.signature.title")+"\n관찰 "+Definition.Signature.Observations.Count(id=>SignatureHas("observed:"+id))+" / 2 · 사본 "+Definition.Signature.Copies.Count(id=>SignatureHas("copy:"+id))+" / 2\n"+context+(DirectionEnabled?"\n"+SignatureRecordContext():"");
        }
        string SignatureDirectionCategory(string id){
            if(id.StartsWith("c1-signature-open-")||id.StartsWith("c1-signature-observe-"))return "observe";
            if(id.StartsWith("c1-signature-humidity-")||id=="c1-signature-trial"||id=="c1-signature-separate"||id=="c1-signature-reset")return "trial";
            if(id.StartsWith("c1-signature-copy-")||id.StartsWith("c1-signature-proof-")||id=="c1-signature-notes"||id=="c1-signature-mark"||id=="c1-signature-compare"||id=="c1-signature-cite"||id=="c1-signature-confirm"||id=="c1-signature-review"||id=="preview-next"||id=="confirm-submit"||id=="retry-save")return "record";
            return null;
        }
        string SignatureRecordContext()=>SignatureComplete?"기록 · 저장 완료":SavePending?"기록 · 저장 확인 중":overlay=="saveFailure"?"기록 · 저장 실패":signatureProofLeft!=null||signatureProofRight!=null?"기록 · 선택안 미인용":Definition.Signature.Ready(Journal.State)?"기록 · 확정 준비":"기록 · 준비 중";
        void SignatureSection(GameScreen screen,int first,string key,string title,string detail=null){
            if(!DirectionEnabled||first>=screen.Actions.Count)return;
            var action=screen.Actions[first];action.SectionKey=key;action.SectionTitle=title;action.SectionDetail=detail;
        }
        string SignatureRecordedProofText()
        {
            var def=Definition.Signature;var sources=ObservedSignatureSources().ToArray();
            string Label(string id)=>sources.FirstOrDefault(source=>source.Id=="signature:"+id)?.Label??"확인할 수 없는 출처";
            return "기록된 대조 근거: "+(SignatureHas("proofSelected")?Label(def.RecordedProofLeft(Journal.State))+" ↔ "+Label(def.RecordedProofRight(Journal.State)):"없음");
        }
        string SignatureConfirmationText()=>"서명지 두 장의 사본, 가려진 영역 표시, 판 #0의 번호대 연결과 기록된 대조 근거를 함께 저장합니다.\n\n"+SignatureRecordedProofText()+"\n원본과 매체의 구분은 형식 확인이며 해석의 정답 판정이 아닙니다. 가려진 서명란은 미해결로 남습니다.";
        void SignatureScreen(GameScreen s)
        {
            PrepareSignatureSurface(s,document??"workspace");
            s.Title="C1 · "+SignatureText("c1.signature.title");
            var state=Journal.State;var def=Definition.Signature;
            if(DirectionEnabled)s.SectionSurface=directionProfile.sectionSurface;
            if(SignatureComplete)
            {
                s.Body=SignatureText("c1.signature.complete")+"\n\n"+SignatureRecordedProofText()+"\n가려진 서명란 아래쪽은 여전히 미해결입니다.\n현재 구현된 구간을 마쳤습니다.";
                s.Actions.Add(A("c1-signature-review",L("evidence"),()=>OpenOverlay("evidence")));if(DirectionEnabled)SignatureSection(s,0,"record",directionProfile.recordTitle,SignatureRecordContext());AttachSignaturePaper(s);return;
            }
            if(document!=null)
            {
                var observation=signaturePacket["observations"].FirstOrDefault(o=>(string)o["id"]==document);
                if(observation!=null)
                {
                    var id=(string)observation["id"];s.Title=SignatureText((string)observation["labelKey"]);s.Body=(string)observation["description"];
                    s.Actions.Add(A("c1-signature-observe-"+id,(SignatureHas("observed:"+id)?"✓ ":"")+"관찰 기록 · 원형 사본 보존",()=>SubmitImmediate(new PuzzleCommand("ObserveSignature",id))));
                    s.Actions.Add(A("c1-signature-close",L("back"),()=>{document=null;Render();}));if(DirectionEnabled)SignatureSection(s,0,"observe",directionProfile.observeTitle);AttachSignaturePaper(s);return;
                }
            }
            s.Body=SignatureText("c1.signature.guidance.default");
            var level=SignatureValue("humidity");s.Body+="\n\n"+SignatureText("c1.signature.humidity."+(level??"unset"));
            var trial=SignatureValue("trial");if(trial!=null)s.Body+="\n"+SignatureText(trial=="safe"?"c1.signature.preview.saltReleased":"c1.signature.preview.inkRisk",trial=="safe"?"가장자리의 소금이 풀렸습니다. 아래쪽 가림은 남습니다.":"잉크 번짐 위험이 보입니다. 다른 단수로 다시 시험할 수 있습니다.");
            if(SignatureHas("separated"))s.Body+="\n가장자리 분리 완료 · 둘째 장 아래쪽 가림 유지";
            foreach(var item in signaturePacket["observations"])
            {
                var id=(string)item["id"];s.Actions.Add(A("c1-signature-open-"+id,(SignatureHas("observed:"+id)?"✓ ":"")+SignatureText((string)item["labelKey"]),()=>{document=id;Render();}));
            }
            if(DirectionEnabled)SignatureSection(s,0,"observe",directionProfile.observeTitle);
            int trialFirst=s.Actions.Count;
            foreach(var item in signaturePacket["humidity"]["levels"])
            {
                var id=(string)item["id"];s.Actions.Add(A("c1-signature-humidity-"+id,(level==id?"● ":"")+SignatureText((string)item["labelKey"]),()=>SubmitImmediate(new PuzzleCommand("SetSignatureHumidity",id))));
            }
            if(DirectionEnabled)SignatureSection(s,trialFirst,"trial",directionProfile.trialTitle,SignatureText("c1.signature.humidity."+(level??"unset")));
            SignatureAction(s,"trial","c1.signature.trial",new PuzzleCommand("TrialSignature"));
            SignatureAction(s,"separate","c1.signature.separate",new PuzzleCommand("SeparateSignature"));
            s.Actions.Add(A("c1-signature-reset",SignatureText("c1.signature.reset"),()=>SubmitImmediate(new PuzzleCommand("ResetSignatureTrial"))));
            int recordFirst=s.Actions.Count;
            for(int i=0;i<def.Copies.Count;i++){var id=def.Copies[i];SignatureAction(s,"copy-"+(i+1),"c1.signature.copy."+(i+1),new PuzzleCommand("CopySignature",id),SignatureHas("copy:"+id));}
            if(DirectionEnabled)SignatureSection(s,recordFirst,"record",directionProfile.recordTitle,directionProfile.recordPreparation);
            SignatureAction(s,"mark","c1.signature.mark",new PuzzleCommand("MarkSignature",def.RegionId),SignatureHas("marked:"+def.RegionId));
            SignatureAction(s,"compare","c1.signature.compare",new PuzzleCommand("CompareSignature",def.ComparisonId),SignatureHas("compared:"+def.ComparisonId));
            SignatureProofSelectionScreen(s);
            AttachSignaturePaper(s);
        }
        void SignatureProofSelectionScreen(GameScreen screen)
        {
            var context=Store.DirectoryPath+"\n"+saveId;
            if(signatureProofContext!=context){ResetSignatureProofSelection();signatureProofContext=context;}
            var def=Definition.Signature;var state=Journal.State;
            var sources=ObservedSignatureSources().ToArray();
            string Label(string id)=>sources.FirstOrDefault(source=>source.Id=="signature:"+id)?.Label??"선택하지 않음";
            var recordedLeft=def.RecordedProofLeft(state);var recordedRight=def.RecordedProofRight(state);
            bool candidate=signatureProofLeft!=null||signatureProofRight!=null;
            screen.Body+="\n\n기록된 대조 근거: "+(recordedLeft==null?"없음":Label(recordedLeft)+" ↔ "+Label(recordedRight));
            screen.Body+="\n현재 선택안 · 아직 인용하지 않음: 왼쪽 "+Label(signatureProofLeft)+" / 오른쪽 "+Label(signatureProofRight);
            var format=def.ValidateProofSelection(state,signatureProofLeft,signatureProofRight);
            if(candidate)screen.Body+="\n"+SignatureProofReason(format);
            screen.Body+="\n매체와 원본이 다르다는 형식 확인은 해석이 맞다는 판정이 아닙니다.";
            foreach(var source in sources)
            {
                var item=source;var id=item.Id.Substring("signature:".Length);
                screen.Actions.Add(A("c1-signature-proof-left-"+id,(signatureProofLeft==id?"선택됨 · ":"")+"왼쪽 근거 · "+item.Label,
                    ()=>{signatureProofLeft=id;Render();},true,ReviewSourceDetail(item,sources)));
                screen.Actions.Add(A("c1-signature-proof-right-"+id,(signatureProofRight==id?"선택됨 · ":"")+"오른쪽 근거 · "+item.Label,
                    ()=>{signatureProofRight=id;Render();},true,ReviewSourceDetail(item,sources)));
            }
            var cite=new PuzzleCommand("SelectSignatureProof",signatureProofLeft,signatureProofRight,C1SignatureDefinition.SourceSelectionVersion);
            bool canCite=format.IsValid&&Simulation.Validate(state,cite).IsValid;
            screen.Actions.Add(A("c1-signature-cite",SignatureText("c1.signature.cite"),()=>{
                if(SubmitImmediate(cite,false).IsValid)signatureProofLeft=signatureProofRight=null;
                Render();
            },canCite,format.IsValid?"관찰·사본·대조를 마친 뒤 이 선택안을 근거로 기록합니다.":SignatureProofReason(format)));
            if(candidate)screen.Actions.Add(A("c1-signature-proof-discard",recordedLeft==null?"선택안 지우기":"선택안 버리기 · 기록된 근거 유지",
                ()=>{signatureProofLeft=signatureProofRight=null;Render();},true,"선택안만 지웁니다. 이미 기록한 대조 근거는 바뀌지 않습니다."));
            var confirm=SignatureConfirmation();
            screen.Actions.Add(A("c1-signature-confirm","대조 기록 확정",()=>RequestConfirm(confirm),
                !candidate&&Simulation.Validate(state,confirm).IsValid,
                candidate?"선택안을 인용하거나 버린 뒤 기록된 근거로 확정하세요.":def.Ready(state)?"기록된 근거와 사본·가림·번호대 연결을 함께 저장":"관찰, 사본, 가림 표시와 대조 근거를 확인하세요."));
            screen.Actions.Add(A("c1-signature-notes","검토 노트 · 출처와 대조 메모",OpenReviewNotes));
        }
        static string SignatureProofReason(ValidationResult verdict)
        {
            if(verdict.IsValid)return "형식 확인: 서로 다른 원본 · 서로 다른 매체. 내용의 해석은 직접 대조하세요.";
            switch(verdict.DataDiagnostic)
            {
                case "c1.signature.proof.sameSource":return "같은 자료를 양쪽에 골랐습니다. 한 출처를 두 근거로 셀 수 없습니다.";
                case "c1.signature.proof.sameRootAndMedia":return "두 자료는 같은 원본에서 나왔습니다. 사본도 원본을 함께 셉니다.\n매체도 같습니다. 독립 근거는 원본과 매체가 모두 달라야 합니다.";
                case "c1.signature.proof.sameRoot":return "같은 원본에서 나온 자료입니다. 매체가 달라도 독립 근거로 세지 않습니다.";
                case "c1.signature.proof.sameMedia":return "원본은 다르지만 매체가 같습니다. 서로 다른 매체의 근거가 필요합니다.";
                default:return "관찰한 원문 또는 만든 사본을 왼쪽과 오른쪽에 직접 고르세요.";
            }
        }
        void SignatureAction(GameScreen s,string id,string label,PuzzleCommand c,bool complete=false)=>s.Actions.Add(A("c1-signature-"+id,(complete?"✓ ":"")+SignatureText(label),()=>SubmitImmediate(c),Simulation.Validate(Journal.State,c).IsValid));
        void AttachSignaturePaper(GameScreen s)
        {
            var rect=signaturePacket["obscuredRegion"]["rectangle"];var view=Resources.Load<C1SignatureViewSettings>("C1SignatureView");
            bool approved=view!=null&&(view.runtimeApproved||System.Environment.GetCommandLineArgs().Contains("--c1-signature-diagnostic"));
            s.SignaturePaper=new SignaturePaperView{Observed=SignatureHas("observed:"+Definition.Signature.LeftClue),Texture=approved?view.paper:null,Separated=SignatureHas("separated"),FirstCopied=SignatureHas("copy:"+Definition.Signature.Copies[0]),SecondCopied=SignatureHas("copy:"+Definition.Signature.Copies[1]),
                Marked=SignatureHas("marked:"+Definition.Signature.RegionId),Compared=SignatureHas("compared:"+Definition.Signature.ComparisonId),
                LowerRegion=new Rect((float)rect["x"],(float)rect["y"],(float)rect["width"],(float)rect["height"])};
        }
        bool SignatureOverlay(GameScreen s)
        {
            if(!SignatureActive)return false;
            PrepareSignatureSurface(s,"overlay:"+overlay);
            if(overlay=="toolWheel") {s.Body="판독기 · "+SignatureText("c1.signature.title");s.Actions.Add(A("c1-signature-tool","판독 작업",()=>{overlay=null;document=null;Render();}));return true;}
            if(overlay=="evidence")
            {
                var sources=ObservedSignatureSources().ToArray();
                s.Body="관찰한 출처\n"+string.Join("\n\n",sources.Select(source=>source.Label+"\n"+ReviewSourceDetail(source,sources)))+"\n\n"+SignatureRecordedProofText();
                if(Definition.Signature.AllCopied(Journal.State))s.Body+="\n\n사본은 같은 원본의 두 장입니다. 두 사본끼리는 독립 대조 근거가 되지 않습니다.";
                if(SignatureHas("compared:"+Definition.Signature.ComparisonId))s.Body+="\n번호대 대조 기록 · 위의 인용 근거 선택과는 별도입니다.";
                if(SignatureComplete)s.Body+="\n대조 기록 저장 완료";AttachSignaturePaper(s);return true;
            }
            return false;
        }
    }
}
