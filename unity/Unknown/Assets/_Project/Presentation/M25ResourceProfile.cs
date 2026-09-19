using UnityEngine;
using UnityEngine.Video;
namespace Tide.Presentation {
    // M25 (RFC-CX-M25-20260918): Higgsfield-generated backgrounds, portraits, tool icons and the opening
    // motion clip. Imported by Tide.EditorTools.M25ResourceProjectBuilder with runtimeApproved=false; the
    // director flips the gate after the promotion audit. Null fields fall back to the committed look.
    [CreateAssetMenu(menuName="Unknown/M25 Resources")]
    public sealed class M25ResourceProfile:ScriptableObject {
        public bool runtimeApproved;
        [Tooltip("bg-harbor-night: replaces M5Direction.openingImage while the gate is on.")] public Texture2D openingImage;
        [Tooltip("mo-opening-harbor: optional 5 s clip drawn over the opening image; never under reduced motion.")] public VideoClip openingClip;
        [Tooltip("bg-hub-watchroom: painted backdrop for the empty navigation panel of the start screen.")] public Texture2D startBackdrop;
        public string[] zoneIds=new string[0];
        public string[] zoneLabels=new string[0];
        public Texture2D[] zoneBackdrops=new Texture2D[0];
        public string[] toolIds=new string[0];
        public Texture2D[] toolIcons=new Texture2D[0];
        public string[] portraitIds=new string[0];
        public Texture2D[] portraits=new Texture2D[0];
        // M26 (RFC-CX-M26-20260918): question-card backing, T0 receipt envelope, record-media silhouettes
        // (plate/log/ledger) and the four structural-state glyphs. Same gate, same import path, same audit.
        public bool m26Approved;
        public Texture2D questionCard;
        public Texture2D receiptEnvelope;
        public string[] mediaIds=new string[0];
        public Texture2D[] mediaIcons=new Texture2D[0];
        public string[] stateIds=new string[0];
        public Texture2D[] stateGlyphs=new Texture2D[0];
        public Texture2D ToolIcon(string toolId)=>Lookup(toolIds,toolIcons,toolId);
        public Texture2D Portrait(string id)=>Lookup(portraitIds,portraits,id);
        public Texture2D ZoneBackdrop(string zoneId)=>Lookup(zoneIds,zoneBackdrops,zoneId);
        public Texture2D MediaIcon(string sourceType)=>Lookup(mediaIds,mediaIcons,sourceType);
        public Texture2D StateGlyph(string stateId)=>Lookup(stateIds,stateGlyphs,stateId);
        static Texture2D Lookup(string[] keys,Texture2D[] values,string key){
            if(keys==null||values==null||key==null)return null;
            for(int i=0;i<keys.Length&&i<values.Length;i++)if(keys[i]==key)return values[i];
            return null;
        }
    }
}
