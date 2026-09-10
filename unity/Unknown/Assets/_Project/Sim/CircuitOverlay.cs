using System;
using System.Collections.Generic;
using System.Linq;

namespace Tide.Sim
{
    public sealed class CircuitAnchor
    {
        public string Id { get; }
        public string Label { get; }
        public double TargetX { get; } public double TargetY { get; }
        public double OverlayX { get; } public double OverlayY { get; }
        public CircuitAnchor(string id,string label,double tx,double ty,double ox,double oy)
        { Id=id; Label=label; TargetX=tx; TargetY=ty; OverlayX=ox; OverlayY=oy; }
    }
    public sealed class CircuitOverlay
    {
        public double GridStep { get; }
        public double? FineGridStep { get; }
        public double InitialX { get; } public double InitialY { get; }
        public IReadOnlyList<CircuitAnchor> Anchors { get; }
        public CircuitOverlay(double step,double? fine,double x,double y,IEnumerable<CircuitAnchor> anchors)
        {
            GridStep=step; FineGridStep=fine; InitialX=x; InitialY=y;
            Anchors=Array.AsReadOnly(anchors.ToArray());
            if(!Finite(step)||step<=0||!Finite(x)||!Finite(y)||Anchors.Count!=3||
                Anchors.Select(a=>a.Id).Distinct().Count()!=3||
                Anchors.Any(a=>!Finite(a.TargetX)||!Finite(a.TargetY)||!Finite(a.OverlayX)||!Finite(a.OverlayY))||
                (fine.HasValue && (!Finite(fine.Value)||fine.Value<=0))) throw new InvalidOperationException("Invalid circuit overlay");
            var first=Anchors[0];
            if(!Aligned(first.TargetX-first.OverlayX,first.TargetY-first.OverlayY)) throw new InvalidOperationException("Inconsistent circuit anchors");
        }
        public bool Aligned(double x,double y)=>Anchors.All(a=>a.TargetX==a.OverlayX+x && a.TargetY==a.OverlayY+y);
        public static bool Finite(double x)=>!double.IsNaN(x)&&!double.IsInfinity(x);
    }
}
