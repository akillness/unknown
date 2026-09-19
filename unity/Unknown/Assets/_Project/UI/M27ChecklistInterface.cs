using UnityEngine;
using UnityEngine.UI;
namespace Tide.UI {
    // M27 (RFC-CX-M27-20260918): the current beat's requirements as a row of chips under the inquiry strip.
    // Each chip = a state glyph (done / current / open; text fallback ✓ ▶ ○ when the resource gate is off) + a short
    // caption. Never colour alone: the glyph shape and the caption weight carry the state. Non-raycast, no Selectable.
    public sealed partial class T0Interface {
        public const string ChecklistName="Checklist";
        void RenderChecklistBand(float top,float height,GameScreen model){
            var band=Panel("Checklist band",root,new Vector2(.46f,top-height),new Vector2(1,top),new Color(paper.r,paper.g,paper.b,.97f));
            band.GetComponent<Image>().raycastTarget=false;
            RenderChecklist(band,model);
        }
        // Fills any rect (fixed band under the card, or a flowed row in the reader layout) with the same chips.
        void RenderChecklist(RectTransform band,GameScreen model){
            int n=model.Checklist.Count;if(n==0)return;
            float gap=.01f,x=.02f,width=Mathf.Min(.24f,(.96f-gap*(n-1))/n);
            for(int i=0;i<n;i++){
                var item=model.Checklist[i];
                string state=item.Done?"done":item.Current?"current":"open";
                float alpha=item.Done||item.Current?1f:.55f;   // glyph only; text stays full ink (M21 reading-surface contrast contract)
                var chip=Panel("Chip "+state,band,new Vector2(x,.12f),new Vector2(x+width,.88f),new Color(ink.r,ink.g,ink.b,item.Current?.16f:.07f));
                chip.GetComponent<Image>().raycastTarget=false;
                if(item.Glyph!=null){
                    // Same structure as InquiryFigure: the fitter lives on a child of a fixed cell, so FitInParent fits the cell, not the chip.
                    var cell=Rect("Glyph cell",chip,new Vector2(.03f,.1f),new Vector2(.2f,.9f));
                    var picture=Rect("Glyph",cell,Vector2.zero,Vector2.one);
                    var image=picture.gameObject.AddComponent<RawImage>();image.texture=item.Glyph;image.raycastTarget=false;image.color=new Color(1,1,1,alpha);
                    var fit=picture.gameObject.AddComponent<AspectRatioFitter>();fit.aspectMode=AspectRatioFitter.AspectMode.FitInParent;fit.aspectRatio=1;
                }
                // The text form cue is always present, so state never depends on the glyph or on colour alone.
                Text(ChecklistName,chip,(item.Done?"✓ ":item.Current?"▶ ":"○ ")+item.Caption,TypeScale.Meta,ink,
                    new Vector2(item.Glyph!=null?.22f:.06f,0),new Vector2(.97f,1),item.Current?FontStyle.Bold:FontStyle.Normal);
                var caption=chip.Find(ChecklistName).GetComponent<Text>();caption.alignment=TextAnchor.MiddleLeft;caption.raycastTarget=false;
                caption.horizontalOverflow=HorizontalWrapMode.Overflow;caption.verticalOverflow=VerticalWrapMode.Truncate;
                x+=width+gap;
            }
        }
    }
}
