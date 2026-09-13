using System;
using System.Linq;
using Tide.Presentation;
using Tide.UI;
using UnityEngine;

namespace Tide.App
{
    // M20 lane WorkSurface: the archival work-surface candidate (GTI r02) as ONE full-bleed, non-tiled
    // backing for the T0 "Work Surface" panel. Contract: _workspace/current/presentation/t0-work-surface-m20.md
    //
    // Gate: profile.runtimeApproved || profile.diagnosticOverride || --m20-ui-surface-diagnostic.
    // Fallback = the committed M7 work-surface path, unchanged. The committed default runtime keeps
    // runtimeApproved:false and passes no flag, so the gate is closed unless someone opens it explicitly.
    public sealed partial class T0GameSession
    {
        public const string M20SurfaceDiagnosticArg="--m20-ui-surface-diagnostic";
        M20WorkSurfaceProfile m20SurfaceProfile;
        bool m20SurfaceProfileLoaded;
        M20WorkSurfaceProfile M20SurfaceProfile
        {
            get
            {
                if(!m20SurfaceProfileLoaded){m20SurfaceProfileLoaded=true;m20SurfaceProfile=Resources.Load<M20WorkSurfaceProfile>("M20WorkSurface");}
                return m20SurfaceProfile;
            }
        }
        // workSurface is part of the gate: a profile with no imported candidate must not blank the panel.
        bool M20WorkSurfaceEnabled=>M20SurfaceProfile!=null&&M20SurfaceProfile.workSurface!=null&&
            (M20SurfaceProfile.runtimeApproved||M20SurfaceProfile.diagnosticOverride||Environment.GetCommandLineArgs().Contains(M20SurfaceDiagnosticArg));
        // Only side effect: s.WorkSurface. T0Interface keeps the committed M7 backing when it is null.
        void ApplyM20WorkSurface(GameScreen s)
        {
            s.WorkSurface=M20WorkSurfaceEnabled?M20SurfaceProfile:null;
        }
    }
}
