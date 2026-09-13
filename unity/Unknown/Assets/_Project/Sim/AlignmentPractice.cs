using System;
using System.Collections.Generic;
using System.Linq;

namespace Tide.Sim
{
    public sealed class AlignmentPeak
    {
        public string Label { get; }
        public string Shape { get; }
        public int Minute { get; }
        public AlignmentPeak(string label, string shape, int minute) { Label = label; Shape = shape; Minute = minute; }
    }

    public sealed class AlignmentArrangement
    {
        public string Label { get; }
        public IReadOnlyList<AlignmentPeak> TrackA { get; }
        public IReadOnlyList<AlignmentPeak> TrackB { get; }
        public int EventA { get; }
        public int EventB { get; }
        public AlignmentArrangement(string label, IEnumerable<AlignmentPeak> trackA, IEnumerable<AlignmentPeak> trackB, int eventA, int eventB)
        {
            Label = label; TrackA = Array.AsReadOnly(trackA.ToArray()); TrackB = Array.AsReadOnly(trackB.ToArray());
            EventA = eventA; EventB = eventB;
            if (TrackA.Count != 3 || TrackB.Count != 3 || !ValidPeaks(TrackA) || !ValidPeaks(TrackB)
                || !new HashSet<string>(TrackA.Select(p => p.Shape)).SetEquals(TrackB.Select(p => p.Shape)))
                throw new ArgumentException("Alignment practice requires three distinct corresponding peak shapes.");
        }
        static bool ValidPeaks(IReadOnlyList<AlignmentPeak> peaks) => peaks.All(p => p != null && !string.IsNullOrWhiteSpace(p.Label) && !string.IsNullOrWhiteSpace(p.Shape))
            && peaks.Select(p => p.Shape).Distinct().Count() == 3 && peaks.Select(p => p.Minute).Distinct().Count() == 3;
    }

    public sealed class AlignmentPracticeDefinition
    {
        public int ResidualLimitMinutes { get; }
        public int RawErrorMinutes { get; }
        public int StepMinutes { get; }
        public int MinOffsetMinutes { get; }
        public int MaxOffsetMinutes { get; }
        public int InitialOffsetMinutes { get; }
        public IReadOnlyList<AlignmentArrangement> Arrangements { get; }
        public AlignmentPracticeDefinition(int residualLimitMinutes, int rawErrorMinutes, int stepMinutes, int minOffsetMinutes,
            int maxOffsetMinutes, int initialOffsetMinutes, IEnumerable<AlignmentArrangement> arrangements)
        {
            if (residualLimitMinutes <= 0 || rawErrorMinutes < residualLimitMinutes || stepMinutes != 1
                || minOffsetMinutes >= maxOffsetMinutes || initialOffsetMinutes < minOffsetMinutes || initialOffsetMinutes > maxOffsetMinutes)
                throw new ArgumentException("Invalid alignment practice limits.");
            ResidualLimitMinutes = residualLimitMinutes; RawErrorMinutes = rawErrorMinutes; StepMinutes = stepMinutes;
            MinOffsetMinutes = minOffsetMinutes; MaxOffsetMinutes = maxOffsetMinutes; InitialOffsetMinutes = initialOffsetMinutes;
            Arrangements = Array.AsReadOnly(arrangements.ToArray());
            if (Arrangements.Count < 2) throw new ArgumentException("A transfer arrangement is required.");
        }
    }

    public enum AlignmentBaselineStatus { MissingAnchors, DuplicateAnchors, MismatchedPeaks, ResidualExceeded, Ready, Locked }
    public enum AlignmentOrder { Indeterminate, ABeforeB, BBeforeA }

    // Deliberately has no campaign state, command journal, or persistence dependency.
    public sealed class AlignmentPractice
    {
        readonly int[] anchorsA = { -1, -1, -1 }, anchorsB = { -1, -1, -1 };
        public AlignmentPracticeDefinition Definition { get; }
        public int ArrangementIndex { get; private set; }
        public AlignmentArrangement Arrangement => Definition.Arrangements[ArrangementIndex];
        public int OffsetMinutes { get; private set; }
        public int? ProposedOffsetMinutes { get; private set; }
        public bool BaselineLocked { get; private set; }
        public int EventErrorMinutes => BaselineLocked ? Definition.ResidualLimitMinutes : Definition.RawErrorMinutes;
        public int EventAMinutes => Arrangement.EventA;
        public int EventBMinutes => Arrangement.EventB + OffsetMinutes;
        public AlignmentPractice(AlignmentPracticeDefinition definition) { Definition = definition ?? throw new ArgumentNullException(nameof(definition)); Reset(); }
        public int AnchorA(int slot) => anchorsA[slot];
        public int AnchorB(int slot) => anchorsB[slot];
        bool Complete => anchorsA.All(a => a >= 0) && anchorsB.All(a => a >= 0);
        bool Distinct => anchorsA.Distinct().Count() == anchorsA.Length && anchorsB.Distinct().Count() == anchorsB.Length;
        public int? ResidualMinutes
        {
            get
            {
                if (!Complete) return null;
                int residual = 0;
                for (int i = 0; i < anchorsA.Length; i++)
                    residual = Math.Max(residual, Math.Abs(Arrangement.TrackA[anchorsA[i]].Minute - Arrangement.TrackB[anchorsB[i]].Minute - OffsetMinutes));
                return residual;
            }
        }
        public AlignmentBaselineStatus Status
        {
            get
            {
                if (BaselineLocked) return AlignmentBaselineStatus.Locked;
                if (!Complete) return AlignmentBaselineStatus.MissingAnchors;
                if (!Distinct) return AlignmentBaselineStatus.DuplicateAnchors;
                for (int i = 0; i < anchorsA.Length; i++)
                    if (Arrangement.TrackA[anchorsA[i]].Shape != Arrangement.TrackB[anchorsB[i]].Shape) return AlignmentBaselineStatus.MismatchedPeaks;
                return ResidualMinutes <= Definition.ResidualLimitMinutes ? AlignmentBaselineStatus.Ready : AlignmentBaselineStatus.ResidualExceeded;
            }
        }
        public AlignmentOrder Order
        {
            get
            {
                if (!BaselineLocked) return AlignmentOrder.Indeterminate;
                // Closed intervals that merely touch are not ordered. A small fit residual never reduces the authored uncertainty floor.
                int gap = EventBMinutes - EventAMinutes;
                if (Math.Abs(gap) <= 2 * EventErrorMinutes) return AlignmentOrder.Indeterminate;
                return gap > 0 ? AlignmentOrder.ABeforeB : AlignmentOrder.BBeforeA;
            }
        }
        public bool SelectAnchor(int slot, bool trackA, int candidate)
        {
            if (BaselineLocked || slot < 0 || slot >= anchorsA.Length || candidate < 0 || candidate >= 3) return false;
            (trackA ? anchorsA : anchorsB)[slot] = candidate;
            ProposedOffsetMinutes = null;
            return true;
        }
        public bool AdjustOffset(int steps)
        {
            if (BaselineLocked) return false;
            long next = (long)OffsetMinutes + (long)steps * Definition.StepMinutes;
            if (next < Definition.MinOffsetMinutes || next > Definition.MaxOffsetMinutes) return false;
            OffsetMinutes = (int)next;
            return true;
        }
        public bool ProposeOffset()
        {
            if (BaselineLocked || !Complete || !Distinct) return false;
            double sum = 0;
            for (int i = 0; i < anchorsA.Length; i++) sum += Arrangement.TrackA[anchorsA[i]].Minute - Arrangement.TrackB[anchorsB[i]].Minute;
            int proposal = (int)Math.Round(sum / anchorsA.Length, MidpointRounding.AwayFromZero);
            if (proposal < Definition.MinOffsetMinutes || proposal > Definition.MaxOffsetMinutes) { ProposedOffsetMinutes = null; return false; }
            ProposedOffsetMinutes = proposal;
            return true;
        }
        public bool ApplyProposal()
        {
            if (BaselineLocked || !ProposedOffsetMinutes.HasValue) return false;
            OffsetMinutes = ProposedOffsetMinutes.Value;
            return true;
        }
        public bool LockBaseline()
        {
            if (Status != AlignmentBaselineStatus.Ready) return false;
            BaselineLocked = true;
            return true;
        }
        public void UnlockBaseline() { BaselineLocked = false; }
        public void ClearAnchors()
        {
            BaselineLocked = false; ProposedOffsetMinutes = null;
            for (int i = 0; i < anchorsA.Length; i++) anchorsA[i] = anchorsB[i] = -1;
        }
        public void Reset() { ClearAnchors(); OffsetMinutes = Definition.InitialOffsetMinutes; }
        public bool SelectArrangement(int index)
        {
            if (index < 0 || index >= Definition.Arrangements.Count) return false;
            ArrangementIndex = index; Reset(); return true;
        }
    }
}
