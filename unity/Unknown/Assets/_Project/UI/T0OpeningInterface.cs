using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Tide.UI {
    public sealed partial class T0Interface {
        readonly Dictionary<string,Image> directionMarks=new Dictionary<string,Image>();
        public string CurrentDirectionCategory {get;private set;}
        void RenderDirectionStrip(float top,float height,Texture2D texture){
            var strip=Panel("Direction orientation",root,new Vector2(.46f,top-height),new Vector2(1,top),ink);
            if(texture!=null){var surface=Rect("Direction backdrop",strip,Vector2.zero,Vector2.one);var image=surface.gameObject.AddComponent<RawImage>();image.texture=texture;image.raycastTarget=false;}
            string[] keys={"observe","trial","record"},labels={"관찰","시험","기록"};
            for(int i=0;i<keys.Length;i++){
                var mark=Panel("Direction "+keys[i],strip,new Vector2(i/3f,.06f),new Vector2((i+1)/3f,.94f),Color.clear);
                mark.GetComponent<Image>().raycastTarget=false;directionMarks[keys[i]]=mark.GetComponent<Image>();
                Text("Label",mark,labels[i],18,paper,new Vector2(.05f,.12f),new Vector2(.95f,.88f));
                mark.Find("Label").GetComponent<Text>().alignment=TextAnchor.MiddleCenter;
                mark.Find("Label").GetComponent<Text>().raycastTarget=false;
            }
            UpdateDirectionSelection(null);
        }
        void UpdateDirectionSelection(string category){
            CurrentDirectionCategory=category!=null&&directionMarks.ContainsKey(category)?category:null;
            foreach(var pair in directionMarks)if(pair.Value!=null){
                bool selected=pair.Key==CurrentDirectionCategory;
                pair.Value.color=selected?new Color(brass.r,brass.g,brass.b,.2f):Color.clear;
                var label=pair.Value.transform.Find("Label").GetComponent<Text>();label.color=selected?brass:paper;
                label.fontStyle=selected?FontStyle.Bold:FontStyle.Normal;
            }
        }
        void RenderOpening(GameScreen model){
            Panel("Opening black",root,Vector2.zero,Vector2.one,new Color(.02f,.035f,.045f));
            if(model.OpeningImage!=null){
   var picture=Rect("Opening image",root,Vector2.zero,Vector2.one);
            var graphic=picture.gameObject.AddComponent<RawImage>();graphic.texture=model.OpeningImage;graphic.raycastTarget=false;
            var ratio=picture.gameObject.AddComponent<AspectRatioFitter>();ratio.aspectMode=AspectRatioFitter.AspectMode.FitInParent;ratio.aspectRatio=(float)model.OpeningImage.width/model.OpeningImage.height;
   }
            Text("Opening game title",root,model.Title,16,paper,new Vector2(.655f,.86f),new Vector2(.965f,.95f));
            var caption=Rect("Opening caption area",root,new Vector2(.655f,.30f),new Vector2(.965f,.83f));
            caption.gameObject.AddComponent<RectMask2D>();
   var captionFlow=Flow(caption,6);
   captionFlow.anchorMin=new Vector2(0,1);captionFlow.anchorMax=Vector2.one;captionFlow.pivot=new Vector2(.5f,1);captionFlow.offsetMin=captionFlow.offsetMax=Vector2.zero;
   captionFlow.gameObject.AddComponent<ContentSizeFitter>().verticalFit=ContentSizeFitter.FitMode.PreferredSize;
   var captionScroll=caption.gameObject.AddComponent<ScrollRect>();captionScroll.viewport=caption;captionScroll.content=captionFlow;captionScroll.horizontal=false;captionScroll.vertical=true;captionScroll.scrollSensitivity=30;
            FlowText(captionFlow,model.OpeningHeading,26,paper);
            FlowText(captionFlow,model.Body,21,paper);
            var actionArea=Rect("Opening actions",root,new Vector2(.655f,.07f),new Vector2(.965f,.28f));
            var actionFlow=Flow(actionArea,6);
            foreach(var action in model.Actions)Button(actionFlow,action);
            Text("Footer",root,model.Footer,13,paper,new Vector2(.025f,.008f),new Vector2(.975f,.055f));
            footerText=root.Find("Footer").GetComponent<Text>();
            scroll=null;navigationScroll=null;SceneViewport=UnityEngine.Rect.zero;
            Canvas.ForceUpdateCanvases();focusIndex=0;
            if(focusKey!=null&&keyed.TryGetValue(focusKey,out var selected))focusIndex=focus.IndexOf(selected);
            SelectFocus();ScreenChanged?.Invoke();
        }
        RectTransform DirectionSection(Transform parent,ViewAction action,Texture2D texture){
            var section=Panel("Section "+action.SectionKey,parent,Vector2.zero,Vector2.one,new Color(.065f,.105f,.12f));
            if(texture!=null){
                var surface=Rect("Direction surface",section,Vector2.zero,Vector2.one);
                var image=surface.gameObject.AddComponent<RawImage>();image.texture=texture;image.raycastTarget=false;
                surface.gameObject.AddComponent<LayoutElement>().ignoreLayout=true;
            }
            var layout=section.gameObject.AddComponent<VerticalLayoutGroup>();layout.padding=new RectOffset(12,12,12,14);layout.spacing=8;
            layout.childControlWidth=true;layout.childControlHeight=true;layout.childForceExpandHeight=false;
            section.gameObject.AddComponent<ContentSizeFitter>().verticalFit=ContentSizeFitter.FitMode.PreferredSize;
            FlowText(section,action.SectionTitle,18,paper);
            if(!string.IsNullOrEmpty(action.SectionDetail))FlowText(section,action.SectionDetail,16,paper);
            return section;
        }
    }
}
