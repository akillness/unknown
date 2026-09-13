using System;
using System.Globalization;
using Tide.Sim;
using UnityEngine;
using UnityEngine.UI;

namespace Tide.UI
{
    public sealed class AlignmentPracticeView
    {
        public AlignmentPeak[] TrackA, TrackB;
        public int OffsetMinutes, EventA, EventB, ErrorMinutes, RawErrorMinutes;
        public bool BaselineLocked;
        public string StateLabel, OrderLabel, AnchorLabel;
    }

    public sealed partial class T0Interface
    {
        void RenderAlignmentPractice(RectTransform parent, AlignmentPracticeView view)
        {
            if (view == null) return;
            FlowText(parent, "두 트랙 공통 축 · H±분 · 삼각선: 피크 / 사각 띠: 사건 가능 구간", 16, ink);
            var chart = Rect("Alignment practice paired tracks", parent, Vector2.zero, Vector2.one);
            chart.gameObject.AddComponent<LayoutElement>().preferredHeight = 230 * scale;
            var graphic = chart.gameObject.AddComponent<AlignmentPracticeChart>();
            graphic.View = view; graphic.color = ink; graphic.raycastTarget = false;
            Text("Track A label", chart, "A · 사건 A " + PracticePhase(view.EventA) + " ±" + view.ErrorMinutes + "분", 17, ink, new Vector2(0, .87f), Vector2.one);
            Text("Track B label", chart, "B · 사건 B " + PracticePhase(view.EventB) + " ±" + view.ErrorMinutes + "분", 17, ink, new Vector2(0, .39f), new Vector2(1, .52f));
            chart.Find("Track A label").GetComponent<Text>().raycastTarget = false;
            chart.Find("Track B label").GetComponent<Text>().raycastTarget = false;
            FlowText(parent, "공통 범위 " + PracticePhase(graphic.Minimum) + " ~ " + PracticePhase(graphic.Maximum)
                + (view.BaselineLocked ? " · 정합 후 오차띠" : " · 원시 ±" + view.RawErrorMinutes + "분 오차띠 · B 조절 미리보기"), 16, ink);
            FlowText(parent, view.StateLabel, 18, ink);
            FlowText(parent, view.OrderLabel, 18, ink);
            FlowText(parent, view.AnchorLabel, 16, ink, "Practice correspondence summary");
        }
        static string PracticePhase(int minute) => "H" + minute.ToString("+0;-0;0", CultureInfo.InvariantCulture) + "분";
    }

    // Both labelled lanes share one immutable render snapshot and one range. MaskableGraphic preserves scroll clipping.
    [RequireComponent(typeof(CanvasRenderer))]
    public sealed class AlignmentPracticeChart : MaskableGraphic
    {
        public AlignmentPracticeView View {get;set;}
        public int Minimum
        {
            get
            {
                int result = Math.Min(View.EventA - View.RawErrorMinutes, View.EventB - View.RawErrorMinutes);
                foreach (var peak in View.TrackA) result = Math.Min(result, peak.Minute);
                foreach (var peak in View.TrackB) result = Math.Min(result, peak.Minute + View.OffsetMinutes);
                return result;
            }
        }
        public int Maximum
        {
            get
            {
                int result = Math.Max(View.EventA + View.RawErrorMinutes, View.EventB + View.RawErrorMinutes);
                foreach (var peak in View.TrackA) result = Math.Max(result, peak.Minute);
                foreach (var peak in View.TrackB) result = Math.Max(result, peak.Minute + View.OffsetMinutes);
                return result;
            }
        }
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear(); if (View == null) return;
            var r = GetPixelAdjustedRect(); int min = Minimum, max = Maximum;
            float X(int minute) => r.xMin + r.width * (.025f + .95f * (minute - min) / Math.Max(1f, max - min));
            float Y(float fraction) => r.yMin + r.height * fraction;
            void Box(float x0, float y0, float x1, float y1, Color tint)
            {
                int start = vh.currentVertCount;
                vh.AddVert(new Vector3(x0, y0), tint, Vector2.zero); vh.AddVert(new Vector3(x0, y1), tint, Vector2.zero);
                vh.AddVert(new Vector3(x1, y1), tint, Vector2.zero); vh.AddVert(new Vector3(x1, y0), tint, Vector2.zero);
                vh.AddTriangle(start, start + 1, start + 2); vh.AddTriangle(start, start + 2, start + 3);
            }
            void Line(float x0, float y0, float x1, float y1)
            {
                var a = new Vector2(x0, y0); var b = new Vector2(x1, y1); var normal = new Vector2(-(b - a).y, (b - a).x).normalized;
                int start = vh.currentVertCount;
                vh.AddVert(a - normal, color, Vector2.zero); vh.AddVert(a + normal, color, Vector2.zero);
                vh.AddVert(b + normal, color, Vector2.zero); vh.AddVert(b - normal, color, Vector2.zero);
                vh.AddTriangle(start, start + 1, start + 2); vh.AddTriangle(start, start + 2, start + 3);
            }
            void Lane(AlignmentPeak[] peaks, int offset, int eventMinute, float y)
            {
                Box(X(min), Y(y), X(max), Y(y) + 1, color);
                foreach (var peak in peaks)
                {
                    float width = peak.Shape == "wide" ? .033f : .016f;
                    float x = X(peak.Minute + offset), baseY = Y(y), topY = Y(y + .10f);
                    Line(x - r.width * width, baseY, x, topY);
                    if (peak.Shape == "shoulder")
                    {
                        Line(x, topY, x + r.width * width, Y(y + .06f));
                        Line(x + r.width * width, Y(y + .06f), x + r.width * width * 2, baseY);
                    }
                    else Line(x, topY, x + r.width * width, baseY);
                }
                float left = X(eventMinute - View.ErrorMinutes), right = X(eventMinute + View.ErrorMinutes), bottom = Y(y - .12f), top = Y(y - .045f);
                Box(left, bottom, right, top, new Color(color.r, color.g, color.b, .25f));
                Box(left, bottom, left + 2, top, color); Box(right - 2, bottom, right, top, color);
                Box(left, bottom, right, bottom + 1, color); Box(left, top - 1, right, top, color);
                Box(X(eventMinute) - 1, bottom, X(eventMinute) + 1, top, color);
            }
            Lane(View.TrackA, 0, View.EventA, .70f);
            Lane(View.TrackB, View.OffsetMinutes, View.EventB, .22f);
        }
    }
}
