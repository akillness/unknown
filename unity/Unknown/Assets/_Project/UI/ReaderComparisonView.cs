using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace Tide.UI
{
    public sealed class ReaderComparisonView
    {
        public ReaderComparisonRange Current,Pinned;
        public string Relationship;
    }

    public sealed class ReaderComparisonRange
    {
        public string RecordId,Title,Source,Start,End,Channel,Unit,Missing;
        public float[] Values,Times;
        public float Minimum,Maximum;
        public int StepMinutes;
        public string AxisLabel=>Channel+" ("+Unit+")\n"+(float.IsNaN(Minimum)?"수치 없음":
            "아래 "+Minimum.ToString("0.###",CultureInfo.InvariantCulture)+" · 위 "+Maximum.ToString("0.###",CultureInfo.InvariantCulture));
    }

    public sealed partial class T0Interface
    {
        void RenderReaderComparison(RectTransform parent,ReaderComparisonView view)
        {
            if(view==null)return;
            var row=Rect("Reader comparison",parent,Vector2.zero,Vector2.one);
            var layout=row.gameObject.AddComponent<HorizontalLayoutGroup>();
            layout.spacing=16;layout.childControlWidth=true;layout.childControlHeight=true;
            layout.childForceExpandWidth=true;layout.childForceExpandHeight=false;
            layout.childAlignment=TextAnchor.UpperLeft;
            RenderReaderComparisonRange(row,"현재 비교 선택",view.Current);
            if(view.Pinned!=null)RenderReaderComparisonRange(row,"사용자 고정",view.Pinned);
            else
            {
                var empty=Flow(row,0);empty.gameObject.AddComponent<LayoutElement>().flexibleWidth=1;
                FlowText(empty,"고정한 비교창 없음\n현재 선택을 고정한 뒤 다른 자료나 구간을 선택하세요.",16,ink);
            }
            FlowText(parent,"각 그래프는 자체 세로축 · 가로축은 조위 위상 H±시:분 · 결손은 연결하지 않음"+
                (string.IsNullOrEmpty(view.Relationship)?"":"\n"+view.Relationship),15,ink);
        }

        void RenderReaderComparisonRange(RectTransform parent,string label,ReaderComparisonRange range)
        {
            var card=Flow(parent,0);card.name=label;
            var sizing=card.gameObject.AddComponent<LayoutElement>();sizing.minWidth=0;sizing.flexibleWidth=1;
            FlowText(card,label+" · "+range.Title,16,ink);
            FlowText(card,range.Start+" → "+range.End,15,ink);
            FlowText(card,range.AxisLabel,15,ink);
            var plot=Rect("Compared signal",card,Vector2.zero,Vector2.one);
            plot.gameObject.AddComponent<LayoutElement>().preferredHeight=76;
            var graphic=plot.gameObject.AddComponent<ReaderComparisonChart>();graphic.Range=range;graphic.color=ink;graphic.raycastTarget=false;
            FlowText(card,range.Missing,15,ink);
            FlowText(card,range.Source+" · "+range.StepMinutes+"분 격자",15,ink);
        }
    }

    // A time-based, independently scaled plot. A missing value never becomes a zero or a joining segment.
    [RequireComponent(typeof(CanvasRenderer))]
    public sealed class ReaderComparisonChart:MaskableGraphic
    {
        public ReaderComparisonRange Range {get;set;}
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();var range=Range;
            if(range==null||range.Values.Length==0||float.IsNaN(range.Minimum))return;
            var rect=rectTransform.rect;float width=Mathf.Max(0,rect.width-8),height=Mathf.Max(0,rect.height-8);
            float duration=range.Times[range.Times.Length-1]-range.Times[0],span=range.Maximum-range.Minimum;
            Vector2 previous=default;bool previousPresent=false;
            for(int i=0;i<range.Values.Length;i++)
            {
                float value=range.Values[i];
                if(float.IsNaN(value)){previousPresent=false;continue;}
                float x=duration==0?.5f:(range.Times[i]-range.Times[0])/duration;
                float y=span==0?.5f:(value-range.Minimum)/span;
                var point=new Vector2(rect.xMin+4+x*width,rect.yMin+4+y*height);
                if(previousPresent&&range.Times[i]-range.Times[i-1]<=range.StepMinutes)
                    SignalChart.Line(vh,previous,point,color,1);
                // An isolated finite endpoint beside a gap, or a one-sample window, remains visible.
                int n=vh.currentVertCount;
                vh.AddVert(point+new Vector2(-1,-1),color,Vector2.zero);vh.AddVert(point+new Vector2(-1,1),color,Vector2.zero);
                vh.AddVert(point+new Vector2(1,1),color,Vector2.zero);vh.AddVert(point+new Vector2(1,-1),color,Vector2.zero);
                vh.AddTriangle(n,n+1,n+2);vh.AddTriangle(n,n+2,n+3);
                previous=point;previousPresent=true;
            }
        }
    }
}
