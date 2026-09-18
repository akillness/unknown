using System;
using System.Collections.Generic;
using Tide.Presentation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Tide.UI
{
    public sealed class ViewAction
    {
        public string Id,Label,Detail,SectionKey,SectionTitle,SectionDetail,DirectionCategory;
        public bool Enabled=true;
        public Action Activate;
        public float HoldSeconds;
    }
    // M25: a captioned picture inside a guide section (portrait, tool icon, zone backdrop). Aspect = width/height.
    public sealed class ScreenFigure
    {
        public Texture2D Texture; public string Caption; public float Aspect=1f;
    }
    // M25: a headed block under the body — heading (Section tier, Bold), paragraph (Body tier), optional figure row.
    public sealed class ScreenSection
    {
        public string Heading,Body; public float FigureHeight=110; public readonly List<ScreenFigure> Figures=new List<ScreenFigure>();
    }
    public sealed class GameScreen
    {
        public string Title,Subtitle,Body,Status,Footer,CaseThread;
        public string ActionFeedback;
        public ReaderComparisonView ReaderComparison;
        public AlignmentPracticeView AlignmentPractice;
        public readonly List<ViewAction> Navigation=new List<ViewAction>();
        public readonly List<ViewAction> Actions=new List<ViewAction>();
        public readonly List<ViewAction> Toolbar=new List<ViewAction>();
        public readonly List<ScreenSection> Sections=new List<ScreenSection>();
        public float[] Chart;
        public string ChartLabel;
        public Vector2[] AnchorTargets,AnchorOverlay;
        public string[] AnchorLabels;
        public SignaturePaperView SignaturePaper;
  public ReviewNotesView ReviewNotes;
        public Texture2D OpeningImage,SectionSurface;
        // M25: optional motion clip drawn over OpeningImage (null = the committed static opening); painted
        // backdrop for the navigation panel while it lists nothing (start screen).
        public VideoClip OpeningClip;
        public Texture2D NavigationBackdrop;
        public string OpeningHeading;
        public bool ShowDirection,ShowOpening;
        public bool ResetScroll;
        public bool ResetWorkScroll;
        // RFC-CX-013 UiSkin: null = committed literals (T0GameSession.ApplyM7UiSkin fills it only behind runtimeApproved || --m7-ui-diagnostic).
        public M7UiSkinProfile Skin;
        // M20 WorkSurface: null = committed M7 work-surface path untouched. T0GameSession.ApplyM20WorkSurface
        // fills it only behind runtimeApproved || diagnosticOverride || --m20-ui-surface-diagnostic.
        public M20WorkSurfaceProfile WorkSurface;
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
        private Text actionFeedbackText;
        private RectTransform hintOfferHost,hintOfferPanel;
        private Selectable hintOfferOpen,hintOfferDismiss;
        private string hintReturnFocusKey;
        public void SetHintOffer(ViewAction open,ViewAction dismiss)
        {
            if(open==null||dismiss==null)
            {
                if(hintOfferPanel==null||!hintOfferPanel.gameObject.activeSelf)return;
                var selected=EventSystem.current?.currentSelectedGameObject;
                bool restore=selected!=null&&selected.transform.IsChildOf(hintOfferPanel);
                focus.Remove(hintOfferOpen);focus.Remove(hintOfferDismiss);
                keyed.Remove(hintOfferOpen.name);keyed.Remove(hintOfferDismiss.name);
                hintOfferPanel.gameObject.SetActive(false);
                if(restore)
                {
                    if(hintReturnFocusKey!=null&&Focus(hintReturnFocusKey))return;
                    if(focus.Count>0){focusIndex=Mathf.Clamp(focusIndex,0,focus.Count-1);SelectFocus();}
                    else EventSystem.current?.SetSelectedGameObject(null);
                }
                return;
            }
            if(hintOfferHost==null||hintOfferPanel!=null&&hintOfferPanel.gameObject.activeSelf)return;
            hintReturnFocusKey=CurrentFocusId;
            if(hintOfferPanel==null)
            {
                hintOfferPanel=Panel("Hint offer",hintOfferHost,new Vector2(.64f,.08f),new Vector2(.985f,.92f),paper);
                hintOfferPanel.GetComponent<Image>().raycastTarget=false;
                var row=hintOfferPanel.gameObject.AddComponent<HorizontalLayoutGroup>();
                row.padding=new RectOffset(8,8,6,6);row.spacing=8;
                row.childControlWidth=row.childControlHeight=true;
                row.childForceExpandWidth=row.childForceExpandHeight=true;
                Button(hintOfferPanel,open);Button(hintOfferPanel,dismiss);
                hintOfferOpen=keyed[open.Id];hintOfferDismiss=keyed[dismiss.Id];
            }
            else
            {
                focus.Add(hintOfferOpen);focus.Add(hintOfferDismiss);
                keyed[hintOfferOpen.name]=hintOfferOpen;keyed[hintOfferDismiss.name]=hintOfferDismiss;
                hintOfferPanel.gameObject.SetActive(true);
            }
        }
        public void SetFooter(string value){if(footerText!=null)footerText.text=value;}
        public void SetActionFeedback(string value)
        {
            if(actionFeedbackText==null)return;
            actionFeedbackText.text=value??"";
            actionFeedbackText.transform.parent.gameObject.SetActive(!string.IsNullOrEmpty(value));
        }
        public UnityEngine.Rect SceneViewport {get;private set;}
        private string focusKey;
        private float scale=1;
        public float TextScale { get=>scale; set { scale=value; } }
        public event Action ScreenChanged;
        public string CurrentTitle { get; private set; }
        public string CurrentFocusId=>EventSystem.current?.currentSelectedGameObject?.name;
        public IReadOnlyList<string> ActionIds { get; private set; }
        private static readonly Color inkLiteral=new Color(.1f,.18f,.2f),paperLiteral=new Color(.91f,.9f,.82f),brassLiteral=new Color(.73f,.56f,.28f);
        // Resolved once per Render: skin colours when GameScreen.Skin is set, otherwise the literals above (byte-identical committed look).
        private Color ink=inkLiteral,paper=paperLiteral,brass=brassLiteral;
        private M7UiSkinProfile skin;
        // Resolved once per Render alongside `skin`; null keeps the committed M7 work-surface path.
        private M20WorkSurfaceProfile surface;
        private readonly List<TiledBacking> backings=new List<TiledBacking>();
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
   ClearReviewEditor();
            if(EventSystem.current?.currentSelectedGameObject!=null)
                focusKey=EventSystem.current.currentSelectedGameObject.name;
            if(root!=null) { root.gameObject.SetActive(false); Destroy(root.gameObject); }
            ReleaseOpeningClip();
            hintOfferHost=null;hintOfferPanel=null;hintOfferOpen=null;hintOfferDismiss=null;hintReturnFocusKey=null;
            actionFeedbackText=null;
            focus.Clear(); keyed.Clear(); directionMarks.Clear();CurrentDirectionCategory=null;
            skin=model.Skin; ink=skin==null?inkLiteral:skin.ink; paper=skin==null?paperLiteral:skin.paper; brass=skin==null?brassLiteral:skin.brass; backings.Clear();
            surface=model.WorkSurface;
            root=Rect("Screen",canvas.transform,new Vector2(.02f,.025f),new Vector2(.98f,.975f));
            CurrentTitle=model.Title; ActionIds=model.Actions.ConvertAll(x=>x.Id).AsReadOnly();
            if(model.ShowOpening){RenderOpening(model);return;}
            var header=Panel("Header",root,new Vector2(0,.88f),new Vector2(1,1),skin==null?new Color(.05f,.12f,.15f,.94f):skin.header);
            SkinBacking("M7 frame",header,skin?.bronzeFrame,skin==null?0:skin.frameTilesAcross,new Color(.5f,.5f,.5f,.72f));
            Text("Title",header,model.Title,TypeScale.Display,new Color(.95f,.91f,.78f),new Vector2(.02f,.42f),new Vector2(.62f,.95f),FontStyle.Bold);
            Text("Subtitle",header,model.Subtitle,TypeScale.Meta,new Color(.72f,.81f,.78f),new Vector2(.02f,.04f),new Vector2(.62f,.44f));
            hintOfferHost=header;
            float feedbackHeight=model.AlignmentPractice==null ? .055f*scale : 0;
            var feedbackRow=Panel("Action feedback surface",root,new Vector2(.46f,.115f),new Vector2(1,.115f+feedbackHeight),ink);
            feedbackRow.GetComponent<Image>().raycastTarget=false;
            Text("Action feedback",feedbackRow,"",16,paper,new Vector2(.015f,.05f),new Vector2(.985f,.95f),FontStyle.Bold);
            actionFeedbackText=feedbackRow.Find("Action feedback").GetComponent<Text>();actionFeedbackText.raycastTarget=false;
            SetActionFeedback(model.ActionFeedback);
            var sceneWindow=Rect("Scene viewport",root,new Vector2(0,.515f),new Vector2(.43f,.865f));
            var left=Panel("Navigation",root,new Vector2(0,.12f),new Vector2(.43f,.5f),skin==null?new Color(.08f,.17f,.2f,.92f):skin.navigation);
            // M25: the start screen lists no view nodes, so the empty navigation panel carries the painted watch room instead.
            if(model.NavigationBackdrop!=null&&model.Navigation.Count==0) FullBleedBacking("M25 backdrop",left,model.NavigationBackdrop,Color.white);
            var navigationViewport=Rect("Navigation Viewport",left,new Vector2(.025f,.02f),new Vector2(.975f,.98f));navigationViewport.gameObject.AddComponent<RectMask2D>();
            var leftFlow=Flow(navigationViewport,12);leftFlow.anchorMin=new Vector2(0,1);leftFlow.anchorMax=Vector2.one;leftFlow.pivot=new Vector2(.5f,1);leftFlow.offsetMin=leftFlow.offsetMax=Vector2.zero;leftFlow.gameObject.AddComponent<ContentSizeFitter>().verticalFit=ContentSizeFitter.FitMode.PreferredSize;
            navigationScroll=left.gameObject.AddComponent<ScrollRect>();navigationScroll.viewport=navigationViewport;navigationScroll.content=leftFlow;navigationScroll.horizontal=false;navigationScroll.vertical=true;navigationScroll.scrollSensitivity=30;
            foreach(var a in model.Navigation) Button(leftFlow,a);
            float contentTop=.865f;
            if(model.SignaturePaper!=null)RenderSignaturePaper(leftFlow,model.SignaturePaper);
            if(!string.IsNullOrEmpty(model.CaseThread)&&model.ReaderComparison==null) {
                float height=.185f*scale;
                var card=Panel("Case thread",root,new Vector2(.46f,.865f-height),new Vector2(1,.865f),ink);
                card.GetComponent<Image>().raycastTarget=false;
                Text("CaseThread",card,model.CaseThread,TypeScale.Section,paper,new Vector2(.025f,.04f),new Vector2(.975f,.96f));
                card.Find("CaseThread").GetComponent<Text>().raycastTarget=false;
                float directionHeight=model.ShowDirection ? .055f*Mathf.Max(1,scale) : 0;
                if(model.ShowDirection)RenderDirectionStrip(.865f-height,directionHeight,model.SectionSurface);
                contentTop=.85f-height-directionHeight;
            }
            var contentPanel=Panel("Work Surface",root,new Vector2(.46f,.12f+feedbackHeight),new Vector2(1,contentTop),new Color(paper.r,paper.g,paper.b,.97f));
            // M20: the diagnostic candidate is ONE full-bleed, non-tiled backing and therefore REPLACES the tiled
            // M7 paper backing on this panel instead of stacking over it (t0-work-surface-m20.md §2 T1/T3/T5).
            // Gate closed -> this line is the committed M7 call, unchanged.
            if(surface!=null) FullBleedBacking("M20 work surface",contentPanel,surface.workSurface,surface.tint);
            else SkinBacking("M7 paper",contentPanel,skin?.paperPanel,skin==null?0:skin.paperTilesAcross,skin==null?Color.white:skin.workSurfaceTint);
            var viewport=Rect("Viewport",contentPanel,new Vector2(.025f,.035f),new Vector2(.975f,.97f));
            viewport.gameObject.AddComponent<RectMask2D>();
            var content=Rect("Content",viewport,Vector2.zero,Vector2.one);
            content.anchorMin=new Vector2(0,1); content.anchorMax=Vector2.one; content.pivot=new Vector2(.5f,1);
            var layout=content.gameObject.AddComponent<VerticalLayoutGroup>(); layout.spacing=12; layout.padding=new RectOffset(6,10,4,18);
            layout.childControlWidth=true; layout.childControlHeight=true; layout.childForceExpandHeight=false;
            var fitter=content.gameObject.AddComponent<ContentSizeFitter>(); fitter.verticalFit=ContentSizeFitter.FitMode.PreferredSize;
            scroll=contentPanel.gameObject.AddComponent<ScrollRect>(); scroll.viewport=viewport; scroll.content=content;
            scroll.horizontal=false; scroll.vertical=true; scroll.scrollSensitivity=30;
            RenderReaderComparison(content,model.ReaderComparison);
            RenderAlignmentPractice(content,model.AlignmentPractice);
            if(model.ReaderComparison!=null&&!string.IsNullOrEmpty(model.CaseThread))
            {
                var noteParent=content;
                if(surface!=null&&surface.workSurface!=null)
                {
                    noteParent=Flow(content,0);
                    var ground=noteParent.gameObject.AddComponent<Image>();
                    ground.color=new Color(paper.r,paper.g,paper.b,ReadingBandAlpha);
                    ground.raycastTarget=false;
                }
                FlowText(noteParent,model.CaseThread,TypeScale.Helper,ink);
                var note=noteParent.GetChild(noteParent.childCount-1).GetComponent<Text>();
                note.name="CaseThread";note.raycastTarget=false;
            }
            FlowText(content,model.Body,TypeScale.Body,ink);
            // M25 guide sections: Section-tier bold heading, Body-tier paragraph, fixed-size figure row.
            foreach(var section in model.Sections)
            {
                FlowText(content,section.Heading,TypeScale.Section,ink,"Section heading",FontStyle.Bold);
                FlowText(content,section.Body,TypeScale.Body,ink);
                FigureRow(content,section);
            }
   RenderReviewNotes(content,model.ReviewNotes);
            if(model.Chart!=null)
            {
                FlowText(content,model.ChartLabel,TypeScale.Meta,ink);
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
                if(model.AnchorLabels!=null) FlowText(content,string.Join(" · ",model.AnchorLabels),TypeScale.Meta,ink);
            }
            RectTransform actionParent=content;
            foreach(var action in model.Actions)
            {
                if(!string.IsNullOrEmpty(action.SectionKey))actionParent=DirectionSection(content,action,model.SectionSurface);
                // M19: helper text steps down a size and tones toward the surface so it cannot be
                // mistaken for the action label above it (presentation/t0-action-plate-m19.md §1.1).
                if(!string.IsNullOrEmpty(action.Detail)) FlowText(actionParent,action.Detail,TypeScale.Helper,actionParent==content?Muted(ink,paper):Muted(paper,ink));
                Button(actionParent,action);
                // The review question answers the button above it; rendering it here keeps the answer in view after the focus scroll (D-M9-15).
                if(model.ReviewNotes!=null&&action.Id==model.ReviewNotes.QuestionAnchorId) RenderReviewQuestion(actionParent,model.ReviewNotes);
            }
            // RFC-CX-016: the committed literal is authored for the dark Work Surface; with the skin on the same line sits
            // on rag paper, so the skin supplies a deeper ochre. skin==null keeps the committed literal byte-identical.
            if(!string.IsNullOrEmpty(model.Status)) FlowText(content,model.Status,TypeScale.Status,skin==null?new Color(.38f,.18f,.07f):skin.statusOnPaper,"Text",FontStyle.Bold);
            var bar=Panel("Toolbar",root,new Vector2(0,0),new Vector2(1,.105f),skin==null?new Color(.05f,.12f,.15f,.96f):skin.toolbar);
            SkinBacking("M7 frame",bar,skin?.bronzeFrame,skin==null?0:skin.frameTilesAcross,new Color(.5f,.5f,.5f,.72f));
            var tools=Rect("Tools",bar,new Vector2(.01f,.34f),new Vector2(.99f,.96f));
            var horizontal=tools.gameObject.AddComponent<HorizontalLayoutGroup>(); horizontal.spacing=8;
            horizontal.childControlWidth=true; horizontal.childForceExpandWidth=true; horizontal.childControlHeight=true;
            foreach(var action in model.Toolbar) Button(tools,action);
            Text("Footer",bar,model.Footer,TypeScale.Meta,new Color(.78f,.84f,.8f),new Vector2(.015f,0),new Vector2(.985f,.3f));
            footerText=bar.Find("Footer").GetComponent<Text>();
            Canvas.ForceUpdateCanvases();
            foreach(var backing in backings) backing.Apply();
            // M21: the r02 ground is dark, so the committed ink paragraphs on this surface need a reading
            // ground. Gate closed -> `surface` is null and this call creates nothing (M21ReadingBackingInterface.cs).
            ApplyM21ReadingBacking(content);
            var sceneCorners=new Vector3[4];sceneWindow.GetWorldCorners(sceneCorners);SceneViewport=new UnityEngine.Rect(sceneCorners[0].x/Screen.width,sceneCorners[0].y/Screen.height,(sceneCorners[2].x-sceneCorners[0].x)/Screen.width,(sceneCorners[2].y-sceneCorners[0].y)/Screen.height);
            focusIndex=0;
            if(focusKey!=null && keyed.TryGetValue(focusKey,out var selected)) focusIndex=focus.IndexOf(selected);
            SelectFocus();
            if(model.ResetScroll){scroll.verticalNormalizedPosition=1;navigationScroll.verticalNormalizedPosition=1;}
            else if(model.ResetWorkScroll)scroll.verticalNormalizedPosition=1;
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
        public void ScrollWork(float pages)
        {
            if(scroll==null)return;
            Canvas.ForceUpdateCanvases();
            float extent=scroll.content.rect.height-scroll.viewport.rect.height;
            if(extent>0)scroll.verticalNormalizedPosition=Mathf.Clamp01(scroll.verticalNormalizedPosition-pages*scroll.viewport.rect.height*.85f/extent);
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
        // RFC-CX-013 UiSkin: one non-raycast RawImage inserted as the panel's first child (behind every later child).
        // uvRect is resolved in Apply() after Canvas.ForceUpdateCanvases so the tile count follows the panel's real aspect.
        private void SkinBacking(string name,RectTransform panel,Texture2D texture,float tilesAcross,Color tint)
        {
            if(texture==null||tilesAcross<=0) return;
            var r=Rect(name,panel,Vector2.zero,Vector2.one); r.SetAsFirstSibling();
            var image=r.gameObject.AddComponent<RawImage>(); image.texture=texture; image.color=tint; image.raycastTarget=false;
            backings.Add(new TiledBacking{Image=image,Panel=panel,TilesAcross=tilesAcross});
        }
        // M20 WorkSurface: same placement and input rules as SkinBacking — non-raycast, first child, so text and
        // buttons draw over it — but deliberately NOT registered in `backings`, so uvRect stays the default
        // (0,0,1,1). That is the non-tile rule: a single 1672x941 composition must never repeat.
        private void FullBleedBacking(string name,RectTransform panel,Texture2D texture,Color tint)
        {
            if(texture==null) return;
            var r=Rect(name,panel,Vector2.zero,Vector2.one); r.SetAsFirstSibling();
            var image=r.gameObject.AddComponent<RawImage>(); image.texture=texture; image.color=tint; image.raycastTarget=false;
        }
        private sealed class TiledBacking
        {
            public RawImage Image; public RectTransform Panel; public float TilesAcross;
            public void Apply()
            {
                if(Image==null||Panel==null) return;
                var size=Panel.rect.size; var tilesDown=size.x>0?TilesAcross*size.y/size.x:TilesAcross;
                Image.uvRect=new UnityEngine.Rect(0,0,TilesAcross,tilesDown);
            }
        }
        private RectTransform Flow(Transform parent,int padding)
        {
            var r=Rect("Flow",parent,new Vector2(.025f,.02f),new Vector2(.975f,.98f));
            var g=r.gameObject.AddComponent<VerticalLayoutGroup>(); g.spacing=10; g.padding=new RectOffset(padding,padding,padding,padding);
            g.childControlWidth=true; g.childControlHeight=true; g.childForceExpandHeight=false; return r;
        }
        private void Text(string name,Transform parent,string value,int size,Color c,Vector2 min,Vector2 max,FontStyle style=FontStyle.Normal)
        {
            var r=Rect(name,parent,min,max); var text=r.gameObject.AddComponent<Text>(); text.font=font;text.text=value??"";
            text.fontSize=Mathf.RoundToInt(size*scale); text.color=c; text.horizontalOverflow=HorizontalWrapMode.Wrap;
            text.fontStyle=style; text.lineSpacing=TypeScale.LineSpacing;
        }
        private void FlowText(Transform parent,string value,int size,Color c,string name="Text",FontStyle style=FontStyle.Normal)
        {
            if(string.IsNullOrEmpty(value)) return;
            var r=Rect(name,parent,Vector2.zero,Vector2.one); var text=r.gameObject.AddComponent<Text>();
            text.font=font;text.text=value;text.fontSize=Mathf.RoundToInt(size*scale);text.color=c;
            text.fontStyle=style; text.lineSpacing=TypeScale.LineSpacing;
            text.horizontalOverflow=HorizontalWrapMode.Wrap; text.verticalOverflow=VerticalWrapMode.Overflow;
            r.gameObject.AddComponent<LayoutElement>().minHeight=size*scale*1.7f;
        }
        // M25 figure row: fixed canvas-unit cells (pictures do not follow the text scale), each cell = picture over
        // a Meta-tier caption. No AspectRatioFitter inside the layout group — the cell width is preferredHeight×aspect.
        private void FigureRow(Transform parent,ScreenSection section)
        {
            if(section.Figures.Count==0) return;
            var row=Rect("Figures",parent,Vector2.zero,Vector2.one);
            float captionHeight=TypeScale.Meta*scale*1.6f, cell=section.FigureHeight;
            row.gameObject.AddComponent<LayoutElement>().preferredHeight=cell+captionHeight;
            var group=row.gameObject.AddComponent<HorizontalLayoutGroup>(); group.spacing=10; group.childAlignment=TextAnchor.UpperLeft;
            group.childControlWidth=true; group.childControlHeight=true; group.childForceExpandWidth=false; group.childForceExpandHeight=true;
            foreach(var figure in section.Figures)
            {
                var item=Panel("Figure "+figure.Caption,row,Vector2.zero,Vector2.one,new Color(ink.r,ink.g,ink.b,.08f));
                item.GetComponent<Image>().raycastTarget=false;
                var element=item.gameObject.AddComponent<LayoutElement>(); element.preferredWidth=cell*Mathf.Max(.2f,figure.Aspect); element.flexibleWidth=0;
                var picture=Rect("Picture",item,Vector2.zero,Vector2.one); picture.offsetMin=new Vector2(0,captionHeight); picture.offsetMax=Vector2.zero;
                var image=picture.gameObject.AddComponent<RawImage>(); image.texture=figure.Texture; image.raycastTarget=false;
                image.color=figure.Texture==null?new Color(ink.r,ink.g,ink.b,.35f):Color.white;
                Text("Caption",item,figure.Caption,TypeScale.Meta,ink,Vector2.zero,Vector2.one);
                var caption=item.Find("Caption").GetComponent<Text>(); caption.alignment=TextAnchor.LowerCenter; caption.raycastTarget=false;
                ((RectTransform)caption.transform).offsetMax=new Vector2(0,-cell);
            }
        }
        // M25 opening clip: a VideoPlayer renders into a RenderTexture that replaces the still only once the clip is
        // prepared; any error keeps the still. Released on the next Render and on destroy (no leaked RenderTexture).
        private VideoPlayer openingPlayer; private RenderTexture openingRender;
        private void AttachOpeningClip(RawImage graphic,VideoClip clip,Texture2D still)
        {
            if(clip==null||graphic==null||Application.isBatchMode) return;
            openingRender=new RenderTexture((int)Mathf.Max(16,clip.width),(int)Mathf.Max(16,clip.height),0){name="M25 opening clip"};
            openingPlayer=graphic.gameObject.AddComponent<VideoPlayer>();
            openingPlayer.playOnAwake=false; openingPlayer.source=VideoSource.VideoClip; openingPlayer.clip=clip;
            openingPlayer.renderMode=VideoRenderMode.RenderTexture; openingPlayer.targetTexture=openingRender;
            openingPlayer.isLooping=true; openingPlayer.audioOutputMode=VideoAudioOutputMode.None; openingPlayer.skipOnDrop=true;
            openingPlayer.prepareCompleted+=player=>{ if(graphic!=null&&openingRender!=null){ graphic.texture=openingRender; player.Play(); } };
            openingPlayer.errorReceived+=(player,message)=>{ if(graphic!=null) graphic.texture=still; Debug.LogWarning("M25 opening clip fell back to the still: "+message); };
            openingPlayer.Prepare();
        }
        private void ReleaseOpeningClip()
        {
            if(openingPlayer!=null){ openingPlayer.Stop(); openingPlayer=null; }
            if(openingRender!=null){ openingRender.Release(); Destroy(openingRender); openingRender=null; }
        }
        private void OnDestroy(){ ReleaseOpeningClip(); }
        // M19 plate ornament: a non-raycast child drawn from the colours Render() already resolved
        // (same rule as SkinBacking — ornament never intercepts input, never adds an asset).
        private Image Ornament(string name,Transform parent,Vector2 min,Vector2 max,Color c)
        {
            var r=Rect(name,parent,min,max); var image=r.gameObject.AddComponent<Image>();
            image.color=c; image.raycastTarget=false; return image;
        }
        // Subordinate tone: pull the foreground a third of the way toward its own background.
        private static Color Muted(Color fore,Color back)=>Color.Lerp(fore,back,.32f);
        private void Button(Transform parent,ViewAction action)
        {
            var r=Panel(action.Id,parent,Vector2.zero,Vector2.one,ink);
            var item=r.gameObject.AddComponent<LayoutElement>(); item.minHeight=48*scale; item.preferredHeight=48*scale;
            var button=r.gameObject.AddComponent<Button>(); button.interactable=action.Enabled;
            var colors=button.colors; colors.normalColor=Color.white; colors.highlightedColor=skin==null?new Color(1,.85f,.55f):skin.buttonHighlight;
            // M19: focus now inverts the plate below, so the selected tint must stop multiplying the ink
            // plate *darker*. Vertex colours clamp at 1, so a multiply can never brighten it back.
            colors.selectedColor=Color.white; colors.disabledColor=new Color(.45f,.45f,.45f,.65f); colors.fadeDuration=0; button.colors=colors;
            // The plate: a lit top edge plus a brass action rule, so a control is not a flat rectangle.
            // The rule is the "pressable" marker and is therefore absent while the action is disabled.
            Ornament("Bevel",r,new Vector2(0,.965f),new Vector2(1,1),new Color(brass.r,brass.g,brass.b,.32f));
            var rule=Ornament("Rule",r,new Vector2(.010f,.2f),new Vector2(.024f,.8f),brass);
            rule.gameObject.SetActive(action.Enabled);
            Text("Label",r,action.Label,18,paper,new Vector2(.075f,.1f),new Vector2(.97f,.9f));
            var labelText=r.Find("Label").GetComponent<Text>(); labelText.fontStyle=FontStyle.Bold;
            var sizing=r.gameObject.AddComponent<WrappedButtonHeight>();sizing.Label=labelText;sizing.Item=item;sizing.Minimum=48*scale;
            button.onClick.AddListener(()=>{if(action.Enabled) action.Activate?.Invoke();});
            if(action.HoldSeconds>0)
            {
                button.onClick.RemoveAllListeners();
                var hold=r.gameObject.AddComponent<HoldButton>(); hold.Duration=action.HoldSeconds;hold.Confirmed=action.Activate;
                var ring=Rect("Hold progress",r,new Vector2(.91f,.15f),new Vector2(.98f,.85f));ring.gameObject.AddComponent<CanvasRenderer>();hold.Progress=ring.gameObject.AddComponent<HoldProgressRing>();hold.Progress.color=brass;hold.Progress.raycastTarget=false;
            }
            if(action.Enabled) {
                focus.Add(button); keyed[action.Id]=button;
                var plate=r.GetComponent<Image>();
                var selection=r.gameObject.AddComponent<FocusSelection>();
                // Focused plate inverts: brass face, ink label and ink rule. Deselect restores the face.
                selection.Selected=()=>{focusIndex=focus.IndexOf(button);focusKey=button.name;UpdateDirectionSelection(action.DirectionCategory);
                    if(hintOfferPanel!=null&&!r.IsChildOf(hintOfferPanel))hintReturnFocusKey=button.name;
                    plate.color=brass;labelText.color=ink;rule.color=ink;};
                selection.Deselected=()=>{plate.color=ink;labelText.color=paper;rule.color=brass;};
            }
        }
    }
    public sealed class FocusSelection:MonoBehaviour,ISelectHandler,IDeselectHandler
    {
        public Action Selected,Deselected;
        public void OnSelect(BaseEventData data)=>Selected?.Invoke();
        public void OnDeselect(BaseEventData data)=>Deselected?.Invoke();
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
