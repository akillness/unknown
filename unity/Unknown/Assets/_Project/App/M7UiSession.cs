using System;
using System.Linq;
using Tide.Presentation;
using Tide.UI;
using UnityEngine;

namespace Tide.App
{
    // RFC-CX-013 lane UiSkin: M7 rag-paper / bronze UI skin.
    // Gate: profile.runtimeApproved || --m7-ui-diagnostic. Fallback = existing literal colours.
    public sealed partial class T0GameSession
    {
        M7UiSkinProfile uiSkinProfile;
        bool uiSkinProfileLoaded;
        M7UiSkinProfile UiSkinProfile
        {
            get
            {
                if(!uiSkinProfileLoaded){uiSkinProfileLoaded=true;uiSkinProfile=Resources.Load<M7UiSkinProfile>("M7UiSkin");}
                return uiSkinProfile;
            }
        }
        bool UiSkinEnabled=>UiSkinProfile!=null&&(UiSkinProfile.runtimeApproved||Environment.GetCommandLineArgs().Contains("--m7-ui-diagnostic"));
        // Only side effect: s.Skin. T0Interface keeps every committed literal when Skin is null.
        void ApplyM7UiSkin(GameScreen s)
        {
            s.Skin=UiSkinEnabled?UiSkinProfile:null;
        }
    }
}
