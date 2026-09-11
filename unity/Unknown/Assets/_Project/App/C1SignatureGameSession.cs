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
        void PrepareSignatureSurface(GameScreen s,string key){s.ResetScroll=signatureSurface!=key;signatureSurface=key;}
        public bool SignatureActive=>C1SignatureDefinition.Has(Journal.State,"entered");
        public bool SignatureComplete=>Simulation.IsComplete(Journal.State,C1SignatureDefinition.BeatId);
        string SignatureText(string key,string fallback=null)=>(string)signaturePacket["localization"]?[key]??fallback??L("unavailable");
        bool SignatureHas(string key)=>C1SignatureDefinition.Has(Journal.State,key);
        string SignatureValue(string key)=>C1SignatureDefinition.Get(Journal.State,key);
        public void ContinueToSignature(){signatureSurface=null;CloseTool();document=null;overlay=null;SubmitImmediate(new PuzzleCommand("EnterSignature"));}
        PuzzleCommand SignatureConfirmation()=>new PuzzleCommand("ConfirmSignature",Definition.Signature.ComparisonId,Definition.Signature.RegionId);
        string SignatureCaseThread()=>"C1 · "+SignatureText("c1.signature.title")+"\n관찰"+" "+Definition.Signature.Observations.Count(id=>SignatureHas("observed:"+id))+" / 2 · 사본 "+Definition.Signature.Copies.Count(id=>SignatureHas("copy:"+id))+" / 2\n"+
            (SignatureComplete?"번호대 연결 기록 · 아래쪽은 미해결":SignatureValue("humidity")!=null?SignatureText("c1.signature.humidity."+SignatureValue("humidity"))+" · "+(SignatureValue("trial")=="safe"?"가장자리 풀림":SignatureValue("trial")=="risk"?"잉크 번짐 위험":"시험 전"):SignatureHas("separated")?"두 사본과 판의 번호대를 확인한다.":"기록을 살펴보고 습도 시험을 선택한다.");
        string SignatureConfirmationText()=>"서명지 두 장의 사본, 가려진 영역 표시, 판 #0의 번호대 연결과 대조 근거를 함께 저장합니다.\n\n두 매체는 같은 사건을 가리키지만, 가려진 서명란은 미해결로 남습니다.";
        void SignatureScreen(GameScreen s)
        {
            PrepareSignatureSurface(s,document??"workspace");
            s.Title="C1 · "+SignatureText("c1.signature.title");
            var state=Journal.State;var def=Definition.Signature;
            if(SignatureComplete)
            {
                s.Body=SignatureText("c1.signature.complete")+"\n\n가려진 서명란 아래쪽은 여전히 미해결입니다.\n현재 구현된 구간을 마쳤습니다.";
                s.Actions.Add(A("c1-signature-review",L("evidence"),()=>OpenOverlay("evidence")));AttachSignaturePaper(s);return;
            }
            if(document!=null)
            {
                var observation=signaturePacket["observations"].FirstOrDefault(o=>(string)o["id"]==document);
                if(observation!=null)
                {
                    var id=(string)observation["id"];s.Title=SignatureText((string)observation["labelKey"]);s.Body=(string)observation["description"];
                    s.Actions.Add(A("c1-signature-observe-"+id,(SignatureHas("observed:"+id)?"✓ ":"")+"관찰 기록 · 원형 사본 보존",()=>SubmitImmediate(new PuzzleCommand("ObserveSignature",id))));
                    s.Actions.Add(A("c1-signature-close",L("back"),()=>{document=null;Render();}));AttachSignaturePaper(s);return;
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
            foreach(var item in signaturePacket["humidity"]["levels"])
            {
                var id=(string)item["id"];s.Actions.Add(A("c1-signature-humidity-"+id,(level==id?"● ":"")+SignatureText((string)item["labelKey"]),()=>SubmitImmediate(new PuzzleCommand("SetSignatureHumidity",id))));
            }
            SignatureAction(s,"trial","c1.signature.trial",new PuzzleCommand("TrialSignature"));
            SignatureAction(s,"separate","c1.signature.separate",new PuzzleCommand("SeparateSignature"));
            for(int i=0;i<def.Copies.Count;i++){var id=def.Copies[i];SignatureAction(s,"copy-"+(i+1),"c1.signature.copy."+(i+1),new PuzzleCommand("CopySignature",id),SignatureHas("copy:"+id));}
            SignatureAction(s,"mark","c1.signature.mark",new PuzzleCommand("MarkSignature",def.RegionId),SignatureHas("marked:"+def.RegionId));
            SignatureAction(s,"compare","c1.signature.compare",new PuzzleCommand("CompareSignature",def.ComparisonId),SignatureHas("compared:"+def.ComparisonId));
            SignatureAction(s,"cite","c1.signature.cite",new PuzzleCommand("SelectSignatureProof",def.LeftClue,def.RightClue),SignatureHas("proofSelected"));
            s.Actions.Add(A("c1-signature-reset",SignatureText("c1.signature.reset"),()=>SubmitImmediate(new PuzzleCommand("ResetSignatureTrial"))));
            var confirm=SignatureConfirmation();s.Actions.Add(A("c1-signature-confirm","대조 기록 확정",()=>RequestConfirm(confirm),Simulation.Validate(state,confirm).IsValid,
                Definition.Signature.Ready(state)?"사본·가림·번호대 연결을 함께 저장":"관찰, 사본, 가림 표시와 대조 근거를 확인하세요."));
            AttachSignaturePaper(s);
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
                s.Body="서명지와 판 #0\n"+string.Join("\n",signaturePacket["observations"].Where(o=>SignatureHas("observed:"+(string)o["id"])).Select(o=>SignatureText((string)o["labelKey"])+" · 원형 사본 보존"));
                if(Definition.Signature.AllCopied(Journal.State))s.Body+="\n\n사본은 같은 원본의 두 장입니다. 두 사본끼리는 독립 대조 근거가 되지 않습니다.";
                if(SignatureHas("compared:"+Definition.Signature.ComparisonId))s.Body+="\n서명지철과 판 #0: 같은 사건의 번호대 · 서로 다른 매체";
                if(SignatureComplete)s.Body+="\n대조 기록 저장 완료";AttachSignaturePaper(s);return true;
            }
            return false;
        }
    }
}
