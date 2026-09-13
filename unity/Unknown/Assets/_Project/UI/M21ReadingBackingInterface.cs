using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tide.UI
{
    // M21 — the readability correction for the M20 diagnostic gate.
    //
    // Observed defect (m20-ui-surface/verification.md §0, native capture pid 87336 / window 5311):
    // with the M20 gate ON the work-surface ground becomes the dark r02 archival candidate while the
    // committed body/instruction text stays dark `ink`, so the measured glyph:ground ratio fell to
    // 1.11:1 against an M19 baseline of 3.53:1 — the body line disappeared into the surface.
    //
    // What this fixes and what it refuses to touch: it changes the READING GROUND only. One
    // translucent, non-raycast, layout-ignored paper band is inserted behind each paragraph that
    // FlowText draws straight onto the work surface, sized to that paragraph. Copy, text colours,
    // font sizes, weights, action ids/order/labels/availability, input routes, focus, simulation and
    // saves are untouched, the M19 instrument plates keep their own opaque faces (a button already has
    // a plate, so it gets no band), and r02 stays fully visible around every band and 18% through it.
    // runtimeApproved / runtimeEligible are never written: this is a correction inside the diagnostic
    // gate, not a promotion.
    //
    // Gate: identical to M20's. `surface` is non-null only behind runtimeApproved || diagnosticOverride
    // || --m20-ui-surface-diagnostic, so the committed default runtime creates zero band objects.
    public sealed partial class T0Interface
    {
        internal const string ReadingBandName="M21 reading";
        // Enough paper to undo the collapse, little enough that the archival surface still modulates it.
        // At .82 the committed body ink reaches ~7.0:1 over the darkest possible ground (flat sRGB
        // arithmetic, not an accessibility claim) while 18% of r02 still shows through the band.
        internal const float ReadingBandAlpha=.82f;
        internal static readonly Vector2 ReadingBandPadding=new Vector2(8f,3f);

        // Called from Render() after Canvas.ForceUpdateCanvases(), so each paragraph already has a
        // resolved rect and preferredHeight for the first placement.
        void ApplyM21ReadingBacking(RectTransform content)
        {
            if(surface==null||surface.workSurface==null||content==null) return;
            var paragraphs=new List<RectTransform>();
            for(int i=0;i<content.childCount;i++)
            {
                var child=content.GetChild(i);
                // FlowText names every paragraph it flows onto the surface "Text". Buttons (own ink
                // plate) and direction sections (own panel) are different objects and stay excluded.
                if(child.name!="Text"||child.GetComponent<Text>()==null) continue;
                paragraphs.Add((RectTransform)child);
            }
            foreach(var paragraph in paragraphs)
            {
                var band=Rect(ReadingBandName,content,Vector2.zero,Vector2.one);
                // Inserted at the paragraph's current index, i.e. as the sibling immediately BEFORE it,
                // so the band is drawn first and the glyphs stay on top of it.
                band.SetSiblingIndex(paragraph.GetSiblingIndex());
                var image=band.gameObject.AddComponent<Image>();
                image.color=new Color(paper.r,paper.g,paper.b,ReadingBandAlpha);
                image.raycastTarget=false;
                // ignoreLayout is what keeps this correction content-safe: the vertical layout group
                // never sees the band, so no committed paragraph or button moves by a pixel.
                band.gameObject.AddComponent<LayoutElement>().ignoreLayout=true;
                var follow=band.gameObject.AddComponent<ReadingBandFollow>();
                follow.Target=paragraph; follow.Paragraph=paragraph.GetComponent<Text>();
                follow.Padding=ReadingBandPadding;
                follow.Apply();
            }
        }
    }

    // The band cannot copy its paragraph's rect once and stop: FlowText only sets a minHeight, so a
    // wrapped paragraph overflows its layout rect, and WrappedButtonHeight keeps reflowing the column
    // on later frames. Mirroring in LateUpdate keeps the ground under the glyphs wherever they settle.
    // Diagnostic-gate only — gate off, none of these components exist.
    public sealed class ReadingBandFollow:MonoBehaviour
    {
        public RectTransform Target; public Text Paragraph; public Vector2 Padding;
        RectTransform self;
        void Awake(){self=(RectTransform)transform;}
        void LateUpdate(){Apply();}
        public void Apply()
        {
            if(self==null||Target==null) return;
            // Copying anchors, pivot and both offsets reproduces the paragraph's rect exactly under any
            // anchor mode; the padding is then applied in the same offset space, in pixels.
            self.anchorMin=Target.anchorMin; self.anchorMax=Target.anchorMax; self.pivot=Target.pivot;
            self.offsetMin=Target.offsetMin; self.offsetMax=Target.offsetMax;
            // Overflowing lines grow downward from the paragraph's top edge, so the band grows with them.
            float overflow=Paragraph==null?0f:Mathf.Max(0f,Paragraph.preferredHeight-Target.rect.height);
            self.offsetMin+=new Vector2(-Padding.x,-Padding.y-overflow);
            self.offsetMax+=new Vector2(Padding.x,Padding.y);
        }
    }
}
