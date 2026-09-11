using UnityEngine;
using UnityEngine.UI;

namespace Tide.UI
{
    // Render data only. It contains no commands, journal or save references.
    public sealed class SignaturePaperView
    {
        public Texture Texture;
        public bool Observed,Separated,FirstCopied,SecondCopied,Marked,Compared;
        public Rect LowerRegion;
    }
    public sealed partial class T0Interface
    {
        void RenderSignaturePaper(RectTransform parent,SignaturePaperView model)
        {
            var stage=Rect("Signature document view",parent,Vector2.zero,Vector2.one);
            stage.gameObject.AddComponent<LayoutElement>().preferredHeight=300;
            Text("Signature sheet state",stage,model.Separated?"두 장 분리 · 가림 유지":"두 장 · 가장자리 부착",18,paper,new Vector2(0,.86f),Vector2.one);
            Sheet(stage,model,1,model.Separated?new Vector2(.02f,.22f):new Vector2(.07f,.22f),model.Separated?new Vector2(.46f,.84f):new Vector2(.51f,.84f),model.FirstCopied);
            Sheet(stage,model,2,model.Separated?new Vector2(.53f,.22f):new Vector2(.41f,.22f),model.Separated?new Vector2(.97f,.84f):new Vector2(.85f,.84f),model.SecondCopied);
            if(!model.Separated)
            {
                var salt=Panel("Salt adhesion edge",stage,new Vector2(.42f,.25f),new Vector2(.455f,.86f),new Color(.78f,.80f,.72f,.94f));salt.GetComponent<Image>().raycastTarget=false;
            }
            Text("Band comparison",stage,model.Compared?"번호대 대조 기록 · 가림 유지":model.FirstCopied&&model.SecondCopied?"원본 유지 · 두 사본은 같은 출처":"두 장 · 가장자리와 아래쪽 확인",12,paper,new Vector2(.02f,0),new Vector2(.98f,.08f));
        }
        void Sheet(RectTransform parent,SignaturePaperView model,int number,Vector2 min,Vector2 max,bool copied)
        {
            var sheet=Rect("Signature sheet "+number,parent,min,max);var image=sheet.gameObject.AddComponent<RawImage>();image.texture=model.Texture;image.color=model.Texture!=null?Color.white:new Color(.80f,.77f,.65f);image.raycastTarget=false;
            // Authored resource stays blank. Numbering belongs to UI and only the first sheet is numbered.
            if(number==1&&model.Observed)Text("Single page number",sheet,"번호 표기",12,ink,new Vector2(.15f,.84f),new Vector2(.93f,.98f));
            Text("Copy status "+number,parent,copied?"원본 · 사본 보존":"원본",14,paper,new Vector2(min.x,.09f),new Vector2(max.x,.22f));
            if(number!=2)return;
            var r=model.LowerRegion;var mask=Panel("Signature lower obscuration",sheet,new Vector2(r.x,1-r.y-r.height),new Vector2(r.x+r.width,1-r.y),new Color(.30f,.31f,.28f,1));
            mask.GetComponent<Image>().raycastTarget=false;
            for(int i=0;i<5;i++){float y=(i+1)/6f;var stripe=Panel("Unresolved hatch "+i,mask,new Vector2(0,y-.015f),new Vector2(1,y+.015f),new Color(.52f,.52f,.43f));stripe.GetComponent<Image>().raycastTarget=false;}
            if(model.Marked){var outline=mask.gameObject.AddComponent<Outline>();outline.effectColor=brass;outline.effectDistance=new Vector2(3,-3);}
            Text("Unresolved region label",sheet,model.Marked?"가림 표시됨":"가림",13,paper,new Vector2(r.x,1-r.y-r.height),new Vector2(r.x+r.width,1-r.y));
        }
    }
}
