using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tide.UI
{
    public sealed class ViewAction
    {
        public string Id,Label,Detail,SectionKey,SectionTitle,SectionDetail,DirectionCategory;
        public bool Enabled=true;
        public Action Activate;
        public float HoldSeconds;
    }
    public sealed class GameScreen
    {
        public string Title,Subtitle,Body,Status,Footer,CaseThread;
        public readonly List<ViewAction> Navigation=new List<ViewAction>();
        public readonly List<ViewAction> Actions=new List<ViewAction>();
        public readonly List<ViewAction> Toolbar=new List<ViewAction>();
        public float[] Chart;
        public string ChartLabel;
        public Vector2[] AnchorTargets,AnchorOverlay;
        public string[] AnchorLabels;
        public SignaturePaperView SignaturePaper;
        public Texture2D OpeningImage,SectionSurface;
        public string OpeningHeading;
        public bool ShowDirection,ShowOpening;
        public bool ResetScroll;
    }
    public sealed partial class T0Interface:MonoBehaviour
    {
        private Canvas canvas;
        private Font font;
        private RectTransform root;
        private readonly List<Selectable> focus=new List<Selectable>();
        private readonly Dictionary<string,Selectable> keyed=new Dictionary<string,Selectable>();
        private int focusIndex;
        private ScrollRect scroll;
        private ScrollRect navigationScroll; private Text footerText;
        public void SetFooter(string value){if(footerText!=null)footerText.text=value;}
        public UnityEngine.Rect SceneViewport {get;private set;}
        private string focusKey;
        private float scale=1;
        public float TextScale { get=>scale; set { scale=value; } }
        public event Action ScreenChanged;
        public string CurrentTitle { get; private set; }
        public string CurrentFocusId=>EventSystem.current?.currentSelectedGameObject?.name;
        public IReadOnlyList<string> ActionIds { get; private set; }
        private readonly Color ink=new Color(.1f,.18f,.2f),paper=new Color(.91f,.9f,.82f),brass=new Color(.73f,.56f,.28f);
        public void Initialize()
        {
            font=Font.CreateDynamicFontFromOSFont(new[]{"Apple SD Gothic Neo","Malgun Gothic","Noto Sans CJK KR","Arial Unicode MS"},20);
            var go=new GameObject("Interface Canvas",typeof(RectTransform),typeof(Canvas),typeof(CanvasScaler),typeof(GraphicRaycaster));
            go.transform.SetParent(transform,false); canvas=go.GetComponent<Canvas>(); canvas.renderMode=RenderMode.ScreenSpaceOverlay;
            var scaler=go.GetComponent<CanvasScaler>(); scaler.uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution=new Vector2(1600,900); scaler.matchWidthOrHeight=.5f;
        }
        public void Render(GameScreen model)
        {
            if(canvas==null) Initialize();
            if(EventSystem.current?.currentSelectedGameObject!=null)
                focusKey=EventSystem.current.currentSelectedGameObject.name;
            if(root!=null) { root.gameObject.SetActive(false); Destroy(root.gameObject); }
            focus.Clear(); keyed.Clear(); directionMarks.Clear();CurrentDirectionCategory=null;
            root=Rect("Screen",canvas.transform,new Vector2(.02f,.025f),new Vector2(.98f,.975f));
            CurrentTitle=model.Title; ActionIds=model.Actions.ConvertAll(x=>x.Id).AsReadOnly();
            if(model.ShowOpening){RenderOpening(model);return;}
            var header=Panel("Header",root,new Vector2(0,.88f),new Vector2(1,1),new Color(.05f,.12f,.15f,.94f));
            Text("Title",header,model.Title,28,new Color(.95f,.91f,.78f),new Vector2(.02f,.42f),new Vector2(.8f,.95f));
            Text("Subtitle",header,model.Subtitle,15,new Color(.72f,.81f,.78f),new Vector2(.02f,.04f),new Vector2(.97f,.44f));
            var sceneWindow=Rect("Scene viewport",root,new Vector2(0,.515f),new Vector2(.43f,.865f));
            var left=Panel("Navigation",root,new Vector2(0,.12f),new Vector2(.43f,.5f),new Color(.08f,.17f,.2f,.92f));
            var navigationViewport=Rect("Navigation Viewport",left,new Vector2(.025f,.02f),new Vector2(.975f,.98f));navigationViewport.gameObject.AddComponent<RectMask2D>();
            var leftFlow=Flow(navigationViewport,12);leftFlow.anchorMin=new Vector2(0,1);leftFlow.anchorMax=Vector2.one;leftFlow.pivot=new Vector2(.5f,1);leftFlow.offsetMin=leftFlow.offsetMax=Vector2.zero;leftFlow.gameObject.AddComponent<ContentSizeFitter>().verticalFit=ContentSizeFitter.FitMode.PreferredSize;
            navigationScroll=left.gameObject.AddComponent<ScrollRect>();navigationScroll.viewport=navigationViewport;navigationScroll.content=leftFlow;navigationScroll.horizontal=false;navigationScroll.vertical=true;navigationScroll.scrollSensitivity=30;
            foreach(var a in model.Navigation) Button(leftFlow,a);
            float contentTop=.865f;
            if(model.SignaturePaper!=null)RenderSignaturePaper(leftFlow,model.SignaturePaper);
            if(!string.IsNullOrEmpty(model.CaseThread)) {
                float height=.185f*scale;
                var card=Panel("Case thread",root,new Vector2(.46f,.865f-height),new Vector2(1,.865f),ink);
                card.GetComponent<Image>().raycastTarget=false;
                Text("CaseThread",card,model.CaseThread,20,paper,new Vector2(.025f,.04f),new Vector2(.975f,.96f));
                card.Find("CaseThread").GetComponent<Text>().raycastTarget=false;
                float directionHeight=model.ShowDirection ? .055f*Mathf.Max(1,scale) : 0;
                if(model.ShowDirection)RenderDirectionStrip(.865f-height,directionHeight,model.SectionSurface);
                contentTop=.85f-height-directionHeight;
            }
            var contentPanel=Panel("Work Surface",root,new Vector2(.46f,.12f),new Vector2(1,contentTop),new Color(paper.r,paper.g,paper.b,.97f));
            var viewport=Rect("Viewport",contentPanel,new Vector2(.025f,.035f),new Vector2(.975f,.97f));
            viewport.gameObject.AddComponent<RectMask2D>();
            var content=Rect("Content",viewport,Vector2.zero,Vector2.one);
            content.anchorMin=new Vector2(0,1); content.anchorMax=Vector2.one; content.pivot=new Vector2(.5f,1);
            var layout=content.gameObject.AddComponent<VerticalLayoutGroup>(); layout.spacing=12; layout.padding=new RectOffset(6,10,4,18);
            layout.childControlWidth=true; layout.childControlHeight=true; layout.childForceExpandHeight=false;
            var fitter=content.gameObject.AddComponent<ContentSizeFitter>(); fitter.verticalFit=ContentSizeFitter.FitMode.PreferredSize;
            scroll=contentPanel.gameObject.AddComponent<ScrollRect>(); scroll.viewport=viewport; scroll.content=content;
            scroll.horizontal=false; scroll.vertical=true; scroll.scrollSensitivity=30;
            FlowText(content,model.Body,20,ink);
            if(model.Chart!=null)
            {
                FlowText(content,model.ChartLabel,15,ink);
                var chart=Rect("Signal",content,Vector2.zero,Vector2.one);
                chart.gameObject.AddComponent<LayoutElement>().preferredHeight=160;
                var graphic=chart.gameObject.AddComponent<SignalChart>(); graphic.Values=model.Chart; graphic.color=ink;
            }
            if(model.AnchorTargets!=null)
            {
                var diagram=Rect("Overlay",content,Vector2.zero,Vector2.one);
                diagram.gameObject.AddComponent<LayoutElement>().preferredHeight=190;
                var graphic=diagram.gameObject.AddComponent<AnchorDiagram>();
                graphic.Targets=model.AnchorTargets; graphic.Overlay=model.AnchorOverlay; graphic.color=brass;
                if(model.AnchorLabels!=null) FlowText(content,string.Join(" · ",model.AnchorLabels),15,ink);
            }
            RectTransform actionParent=content;
            foreach(var action in model.Actions)
            {
                if(!string.IsNullOrEmpty(action.SectionKey))actionParent=DirectionSection(content,action,model.SectionSurface);
                if(!string.IsNullOrEmpty(action.Detail)) FlowText(actionParent,action.Detail,17,actionParent==content?ink:paper);
                Button(actionParent,action);
            }
            if(!string.IsNullOrEmpty(model.Status)) FlowText(content,model.Status,16,new Color(.38f,.18f,.07f));
            var bar=Panel("Toolbar",root,new Vector2(0,0),new Vector2(1,.105f),new Color(.05f,.12f,.15f,.96f));
            var tools=Rect("Tools",bar,new Vector2(.01f,.34f),new Vector2(.99f,.96f));
            var horizontal=tools.gameObject.AddComponent<HorizontalLayoutGroup>(); horizontal.spacing=8;
            horizontal.childControlWidth=true; horizontal.childForceExpandWidth=true; horizontal.childControlHeight=true;
            foreach(var action in model.Toolbar) Button(tools,action);
            Text("Footer",bar,model.Footer,13,new Color(.78f,.84f,.8f),new Vector2(.015f,0),new Vector2(.985f,.3f));
            footerText=bar.Find("Footer").GetComponent<Text>();
            Canvas.ForceUpdateCanvases();
            var sceneCorners=new Vector3[4];sceneWindow.GetWorldCorners(sceneCorners);SceneViewport=new UnityEngine.Rect(sceneCorners[0].x/Screen.width,sceneCorners[0].y/Screen.height,(sceneCorners[2].x-sceneCorners[0].x)/Screen.width,(sceneCorners[2].y-sceneCorners[0].y)/Screen.height);
            focusIndex=0;
            if(focusKey!=null && keyed.TryGetValue(focusKey,out var selected)) focusIndex=focus.IndexOf(selected);
            SelectFocus();
            if(model.ResetScroll){scroll.verticalNormalizedPosition=1;navigationScroll.verticalNormalizedPosition=1;}
            ScreenChanged?.Invoke();
        }
        public void Navigate(int delta)
        {
            if(focus.Count==0) return;
            focusIndex=(focusIndex+delta+focus.Count)%focus.Count; SelectFocus();
        }
        public bool Focus(string id)
        {
            if(!keyed.TryGetValue(id,out var target)||!target.interactable)return false;
            focusIndex=focus.IndexOf(target); SelectFocus(); return true;
        }
        private void SelectFocus()
        {
            if(focus.Count==0 || EventSystem.current==null) return;
            focusIndex=Mathf.Clamp(focusIndex,0,focus.Count-1);
            var target=focus[focusIndex]; EventSystem.current.SetSelectedGameObject(target.gameObject); focusKey=target.name;
            var activeScroll=navigationScroll!=null&&target.transform.IsChildOf(navigationScroll.content)?navigationScroll:scroll;
            if(activeScroll!=null && target.transform.IsChildOf(activeScroll.content))
            {
                Canvas.ForceUpdateCanvases();
                var rect=(RectTransform)target.transform;
                float y=-activeScroll.content.InverseTransformPoint(rect.TransformPoint(rect.rect.center)).y,extent=activeScroll.content.rect.height-activeScroll.viewport.rect.height;
                if(extent>0) activeScroll.verticalNormalizedPosition=1-Mathf.Clamp01((y-activeScroll.viewport.rect.height*.4f)/extent);
            }
        }
        public void BeginActivation()
        {
            if(focus.Count==0) return;
            var hold=focus[focusIndex].GetComponent<HoldButton>();
            if(hold!=null) hold.Begin();
        }
        public void EndActivation()
        {
            if(focus.Count==0) return;
            var selected=focus[focusIndex]; var hold=selected.GetComponent<HoldButton>();
            if(hold!=null) hold.End(); else selected.GetComponent<Button>()?.onClick.Invoke();
        }
        public bool Activate(string id)
        {
            if(!keyed.TryGetValue(id,out var button) || !button.interactable) return false;
            button.GetComponent<Button>().onClick.Invoke(); return true;
        }
        private RectTransform Rect(string name,Transform parent,Vector2 min,Vector2 max)
        {
            var r=new GameObject(name,typeof(RectTransform)).GetComponent<RectTransform>(); r.SetParent(parent,false);
            r.anchorMin=min; r.anchorMax=max; r.offsetMin=Vector2.zero; r.offsetMax=Vector2.zero; return r;
        }
        private RectTransform Panel(string name,Transform parent,Vector2 min,Vector2 max,Color c)
        { var r=Rect(name,parent,min,max); r.gameObject.AddComponent<Image>().color=c; return r; }
        private RectTransform Flow(Transform parent,int padding)
        {
            var r=Rect("Flow",parent,new Vector2(.025f,.02f),new Vector2(.975f,.98f));
            var g=r.gameObject.AddComponent<VerticalLayoutGroup>(); g.spacing=10; g.padding=new RectOffset(padding,padding,padding,padding);
            g.childControlWidth=true; g.childControlHeight=true; g.childForceExpandHeight=false; return r;
        }
        private void Text(string name,Transform parent,string value,int size,Color c,Vector2 min,Vector2 max)
        {
            var r=Rect(name,parent,min,max); var text=r.gameObject.AddComponent<Text>(); text.font=font;text.text=value??"";
            text.fontSize=Mathf.RoundToInt(size*scale); text.color=c; text.horizontalOverflow=HorizontalWrapMode.Wrap;
        }
        private void FlowText(Transform parent,string value,int size,Color c)
        {
            if(string.IsNullOrEmpty(value)) return;
            var r=Rect("Text",parent,Vector2.zero,Vector2.one); var text=r.gameObject.AddComponent<Text>();
            text.font=font;text.text=value;text.fontSize=Mathf.RoundToInt(size*scale);text.color=c;
            text.horizontalOverflow=HorizontalWrapMode.Wrap; text.verticalOverflow=VerticalWrapMode.Overflow;
            r.gameObject.AddComponent<LayoutElement>().minHeight=size*scale*1.7f;
        }
        private void Button(Transform parent,ViewAction action)
        {
            var r=Panel(action.Id,parent,Vector2.zero,Vector2.one,ink);
            var item=r.gameObject.AddComponent<LayoutElement>(); item.minHeight=48*scale; item.preferredHeight=48*scale;
            var button=r.gameObject.AddComponent<Button>(); button.interactable=action.Enabled;
            var colors=button.colors; colors.normalColor=Color.white; colors.highlightedColor=new Color(1,.85f,.55f);
            colors.selectedColor=new Color(1,.85f,.55f); colors.disabledColor=new Color(.45f,.45f,.45f,.65f); button.colors=colors;
            Text("Label",r,action.Label,17,paper,new Vector2(.04f,.1f),new Vector2(.97f,.9f));
            var sizing=r.gameObject.AddComponent<WrappedButtonHeight>();sizing.Label=r.Find("Label").GetComponent<Text>();sizing.Item=item;sizing.Minimum=48*scale;
            button.onClick.AddListener(()=>{if(action.Enabled) action.Activate?.Invoke();});
            if(action.HoldSeconds>0)
            {
                button.onClick.RemoveAllListeners();
                var hold=r.gameObject.AddComponent<HoldButton>(); hold.Duration=action.HoldSeconds;hold.Confirmed=action.Activate;
                var ring=Rect("Hold progress",r,new Vector2(.91f,.15f),new Vector2(.98f,.85f));ring.gameObject.AddComponent<CanvasRenderer>();hold.Progress=ring.gameObject.AddComponent<HoldProgressRing>();hold.Progress.color=brass;hold.Progress.raycastTarget=false;
            }
            if(action.Enabled) {
                focus.Add(button); keyed[action.Id]=button;
                var selection=r.gameObject.AddComponent<FocusSelection>();
                selection.Selected=()=>{focusIndex=focus.IndexOf(button);focusKey=button.name;UpdateDirectionSelection(action.DirectionCategory);};
            }
        }
    }
    public sealed class FocusSelection:MonoBehaviour,ISelectHandler
    {
        public Action Selected;
        public void OnSelect(BaseEventData data)=>Selected?.Invoke();
    }
    public sealed class HoldButton:MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IPointerExitHandler
    {
        public float Duration; public Action Confirmed; public HoldProgressRing Progress; private float began; private bool held;
        public void Begin() { began=Time.unscaledTime; held=true; }
        public void End() { bool accept=held && Time.unscaledTime-began>=Duration; held=false;if(accept) Confirmed?.Invoke(); }
        public void OnPointerDown(PointerEventData e)=>Begin();
        public void OnPointerUp(PointerEventData e)=>End();
        public void OnPointerExit(PointerEventData e) { held=false; }
        private void OnDisable() { held=false; }
        private void Update(){if(Progress!=null)Progress.Amount=held?Mathf.Clamp01((Time.unscaledTime-began)/Duration):0;}
    }
    public sealed class WrappedButtonHeight:MonoBehaviour {
        public Text Label;public LayoutElement Item;public float Minimum;
        void LateUpdate(){if(Label==null||Item==null)return;float height=Mathf.Max(Minimum,Label.preferredHeight/.8f);if(Mathf.Abs(Item.preferredHeight-height)>.5f)Item.preferredHeight=height;}
    }
    [RequireComponent(typeof(CanvasRenderer))]
    public sealed class HoldProgressRing:MaskableGraphic {
        float amount;public float Amount {set {if(Mathf.Approximately(amount,value))return;amount=value;SetVerticesDirty();}}
        protected override void OnPopulateMesh(VertexHelper vh){vh.Clear();if(amount<=0)return;float radius=Mathf.Min(rectTransform.rect.width,rectTransform.rect.height)*.5f;int segments=Mathf.CeilToInt(amount*48);for(int i=0;i<segments;i++){float a=Mathf.PI*.5f-i/48f*Mathf.PI*2,b=Mathf.PI*.5f-Mathf.Min((i+1)/48f,amount)*Mathf.PI*2;int n=vh.currentVertCount;foreach(var point in new[]{new Vector2(Mathf.Cos(a),Mathf.Sin(a))*(radius-2),new Vector2(Mathf.Cos(a),Mathf.Sin(a))*radius,new Vector2(Mathf.Cos(b),Mathf.Sin(b))*radius,new Vector2(Mathf.Cos(b),Mathf.Sin(b))*(radius-2)})vh.AddVert(point,color,Vector2.zero);vh.AddTriangle(n,n+1,n+2);vh.AddTriangle(n,n+2,n+3);}}
    }
    [RequireComponent(typeof(CanvasRenderer))]
    public sealed class SignalChart:MaskableGraphic
    {
        public float[] Values;
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear(); if(Values==null || Values.Length<2) return;
            float min=float.PositiveInfinity,max=float.NegativeInfinity;
            foreach(var v in Values) if(!float.IsNaN(v)) {min=Mathf.Min(min,v);max=Mathf.Max(max,v);}
            if(float.IsInfinity(min))return;
            var rect=rectTransform.rect; float range=Mathf.Max(max-min,float.Epsilon);
            for(int i=1;i<Values.Length;i++)
            {
                if(float.IsNaN(Values[i-1])||float.IsNaN(Values[i]))continue;
                var a=new Vector2(rect.xMin+(i-1f)/(Values.Length-1)*rect.width,rect.yMin+(Values[i-1]-min)/range*rect.height);
                var b=new Vector2(rect.xMin+i/(Values.Length-1f)*rect.width,rect.yMin+(Values[i]-min)/range*rect.height);
                Line(vh,a,b,color,2);
            }
        }
        internal static void Line(VertexHelper vh,Vector2 a,Vector2 b,Color color,float thickness)
        {
            var n=(b-a).normalized; var p=new Vector2(-n.y,n.x)*thickness;
            int index=vh.currentVertCount; vh.AddVert(a-p,color,Vector2.zero);vh.AddVert(a+p,color,Vector2.zero);
            vh.AddVert(b+p,color,Vector2.zero);vh.AddVert(b-p,color,Vector2.zero);
            vh.AddTriangle(index,index+1,index+2);vh.AddTriangle(index,index+2,index+3);
        }
    }
    [RequireComponent(typeof(CanvasRenderer))]
    public sealed class AnchorDiagram:MaskableGraphic
    {
        public Vector2[] Targets,Overlay;
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear(); if(Targets==null||Overlay==null)return;
            var all=new List<Vector2>(Targets); all.AddRange(Overlay);
            float minX=float.PositiveInfinity,minY=minX,maxX=float.NegativeInfinity,maxY=maxX;
            foreach(var p in all){minX=Mathf.Min(minX,p.x);maxX=Mathf.Max(maxX,p.x);minY=Mathf.Min(minY,p.y);maxY=Mathf.Max(maxY,p.y);}
            var r=rectTransform.rect; Func<Vector2,Vector2> map=p=>new Vector2(Mathf.Lerp(r.xMin+14,r.xMax-14,(p.x-minX)/Mathf.Max(1,maxX-minX)),Mathf.Lerp(r.yMin+14,r.yMax-14,(p.y-minY)/Mathf.Max(1,maxY-minY)));
            for(int i=0;i<Targets.Length;i++)
            {
                SignalChart.Line(vh,map(Targets[i]),map(Targets[(i+1)%Targets.Length]),new Color(.12f,.22f,.25f),3);
                SignalChart.Line(vh,map(Overlay[i]),map(Overlay[(i+1)%Overlay.Length]),color,2);
            }
        }
    }
}
