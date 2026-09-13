using UnityEngine;
namespace Tide.Presentation {
    // M20 lane WorkSurface: the archival work-surface candidate (GTI r02) as a SINGLE full-bleed,
    // NON-TILED backing for the T0 "Work Surface" panel only.
    // Contract: _workspace/current/presentation/t0-work-surface-m20.md
    //
    // Why this is a separate profile instead of a field on M7UiSkinProfile: M7's backings are tiled
    // (RawImage.uvRect = paperTilesAcross) and its texture is imported wrapMode=Repeat. r02 is a single
    // 1672x941 composition with a reserved readable centre, so repeating it exposes that centre as a
    // grid. Keeping it out of M7UiSkinProfile also leaves M7's approved values and runtimeApproved flag
    // untouched (contract N11).
    //
    // runtimeApproved stays false: promotion is director-only and decision-log audited. M20 never writes it.
    [CreateAssetMenu(menuName="Unknown/M20 Work Surface")]
    public sealed class M20WorkSurfaceProfile:ScriptableObject {
        // Promotion flag. M20 imports the candidate with this false and never flips it.
        public bool runtimeApproved;
        // The single non-tiled full-bleed candidate. Null = nothing to show, gate stays closed.
        public Texture2D workSurface;
        // RawImage tint. White keeps the imported pixels as authored.
        public Color tint=Color.white;
        // In-memory diagnostic activation for the Editor and tests. [NonSerialized] so it can never be
        // saved into the asset: turning the candidate on for one run must not leave an approval on disk.
        // This exists so the diagnostic gate-on contract is testable without persisting approval.
        [System.NonSerialized] public bool diagnosticOverride;
    }
}
