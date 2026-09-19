using UnityEngine;
using UnityEngine.UI;
namespace Tide.UI {
    // M26 (D2 / D4): the inquiry strip — a question line on the left, pinned media then still-needed media on the
    // right. Non-raycast, read-only, no Selectable. Icons are fixed canvas units so they never follow text scale.
    public sealed partial class T0Interface {
        public const string InquiryName="Inquiry";
        void RenderInquiryStrip(float top,float height,GameScreen model){
            var strip=Panel("Inquiry strip",root,new Vector2(.46f,top-height),new Vector2(1,top),new Color(paper.r,paper.g,paper.b,.97f));
            strip.GetComponent<Image>().raycastTarget=false;
            RenderInquiryStrip(strip,model);
        }
        // Fills any rect (fixed band under the card, or a flowed row in the reader layout) with the same strip.
        void RenderInquiryStrip(RectTransform strip,GameScreen model){
            if(model.InquiryBacking!=null){
                FullBleedBacking("M26 question card",strip,model.InquiryBacking,Color.white);
                // M27: the 21:9 card is centre-cropped to the strip's aspect instead of being stretched non-uniformly.
                Canvas.ForceUpdateCanvases();var image=strip.Find("M26 question card")?.GetComponent<RawImage>();
                if(image!=null&&strip.rect.height>0){
                    float stripAspect=strip.rect.width/strip.rect.height,textureAspect=model.InquiryBacking.width/(float)model.InquiryBacking.height;
                    image.uvRect=stripAspect>textureAspect?new UnityEngine.Rect(0,(1-textureAspect/stripAspect)/2,1,textureAspect/stripAspect):new UnityEngine.Rect((1-stripAspect/textureAspect)/2,0,stripAspect/textureAspect,1);
                }
            }
            int figures=model.InquiryPinned.Count+model.InquiryNeeded.Count;
            float textRight=figures==0?.985f:Mathf.Max(.5f,.985f-.075f*figures);
            Text(InquiryName,strip,model.Inquiry,TypeScale.Helper,ink,new Vector2(.02f,.08f),new Vector2(textRight-.01f,.92f));
            var text=strip.Find(InquiryName).GetComponent<Text>();text.raycastTarget=false;text.alignment=TextAnchor.MiddleLeft;
            float x=textRight;
            foreach(var figure in model.InquiryPinned){InquiryFigure(strip,figure,x,1f);x+=.075f;}
            foreach(var figure in model.InquiryNeeded){InquiryFigure(strip,figure,x,.38f);x+=.075f;}
        }
        void InquiryFigure(RectTransform strip,ScreenFigure figure,float x,float alpha){
            var cell=Rect("Inquiry "+figure.Caption,strip,new Vector2(x,.1f),new Vector2(x+.07f,.9f));
            if(figure.Texture!=null){
                var picture=Rect("Picture",cell,new Vector2(.15f,.08f),new Vector2(.85f,.92f));
                var image=picture.gameObject.AddComponent<RawImage>();image.texture=figure.Texture;image.raycastTarget=false;
                image.color=new Color(1,1,1,alpha);
                var fit=picture.gameObject.AddComponent<AspectRatioFitter>();fit.aspectMode=AspectRatioFitter.AspectMode.FitInParent;fit.aspectRatio=Mathf.Max(.2f,figure.Aspect);
            }else{
                // Gate off / icon missing: the media name still reads as a form cue (a bordered chip), never colour alone.
                var chip=Panel("Chip",cell,new Vector2(.05f,.2f),new Vector2(.95f,.8f),new Color(ink.r,ink.g,ink.b,.1f*alpha+.05f));
                chip.GetComponent<Image>().raycastTarget=false;
                Text("Caption",chip,figure.Caption,TypeScale.Meta,new Color(ink.r,ink.g,ink.b,Mathf.Max(.45f,alpha)),Vector2.zero,Vector2.one);
                var caption=chip.Find("Caption").GetComponent<Text>();caption.alignment=TextAnchor.MiddleCenter;caption.raycastTarget=false;
            }
        }
    }
}
