using UnityEngine;

namespace Tide.App
{
    // RFC-CX-013 Editor verification switch.
    //
    // Why this exists: the three M7 gates only read `runtimeApproved` (director-only promotion) and
    // `Environment.GetCommandLineArgs()`. Unity Hub cannot pass custom player arguments, so opening the
    // project and pressing Play showed the committed pre-M7 look with no way to preview the candidates.
    // `diagnosticOverride` is [NonSerialized] and therefore reset by the domain reload that Play mode
    // performs, so an Editor menu toggle alone cannot survive into the play session.
    //
    // Contract:
    //  - Editor only. The #else branch compiles a `false` constant into player builds, so nothing here
    //    can ship or alter a built player.
    //  - Never writes `runtimeApproved`. Promotion stays a director audit (Tools/M7/Approve …).
    //  - Ignored under `Application.isBatchMode`, so headless EditMode/PlayMode runs stay deterministic
    //    and the GateOff contract tests keep measuring the committed behaviour.
    //  - Toggled from Tools/M7/Editor preview (this machine only); stored in EditorPrefs, i.e. per machine,
    //    never in the repo.
    public sealed partial class T0GameSession
    {
        public const string M7EditorPreviewPref = "Tide.M7.EditorPreview";
#if UNITY_EDITOR
        public static bool M7EditorPreview =>
            !Application.isBatchMode && UnityEditor.EditorPrefs.GetBool(M7EditorPreviewPref, false);
#else
        public const bool M7EditorPreview = false;
#endif
    }
}
