using UnityEngine;
namespace Tide.Presentation {
    // RFC-CX-013 lane UiSkin. Palette = concept/style-guide.md:29-36 (deep ink #0E1F26 · confirmed ink #173238 · record paper #E7E3D8 · ochre #E2AF62).
    // runtimeApproved stays false until the director approves after native inspection (Tools/M7/Approve UI skin).
    [CreateAssetMenu(menuName="Unknown/M7 UI Skin")]
    public sealed class M7UiSkinProfile:ScriptableObject {
        public bool runtimeApproved;
        public Texture2D paperPanel;
        public Texture2D bronzeFrame;
        public Color ink=new Color(23/255f,50/255f,56/255f);
        public Color deepInk=new Color(14/255f,31/255f,38/255f);
        public Color paper=new Color(231/255f,227/255f,216/255f);
        public Color brass=new Color(226/255f,175/255f,98/255f);
        public Color header=new Color(14/255f,31/255f,38/255f,.94f);
        public Color navigation=new Color(23/255f,50/255f,56/255f,.92f);
        public Color toolbar=new Color(14/255f,31/255f,38/255f,.96f);
        public Color workSurfaceTint=new Color(1,1,1,.97f);
        public Color buttonHighlight=new Color(226/255f,175/255f,98/255f);
        // RFC-CX-016: the committed status literal (.38,.18,.07) was authored against dark panels. With the skin on, the
        // status line lands on the rag-paper Work Surface, where measured glyph-scale contrast against the darkest paper
        // fibres was only 3.53:1 measured on the rendered glyph core. Korean 16px strokes only reach ~0.70 coverage, so the
        // nominal colour has to sit well below the AA target: this deeper ochre measured 4.83:1 against the same paper
        // while staying a warm accent distinct from the cool ink body text. Gate off still uses the committed literal.
        public Color statusOnPaper=new Color(.18f,.08f,.03f);
        public float paperTilesAcross=2.5f;
        public float frameTilesAcross=6f;
    }
}
