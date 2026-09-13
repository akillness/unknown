#if TIDE_TEST_FRAMEWORK
using System.Linq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Tide.App;
using Tide.Sim;
using UnityEngine;

namespace Tide.Tests
{
    public sealed class AlignmentPracticeTests
    {
        static AlignmentPracticeDefinition Fixture() => AlignmentPracticeData.Load(JObject.Parse(Resources.Load<TextAsset>("M23AlignmentPractice").text));
        static void SelectThree(AlignmentPractice practice)
        {
            for (int i = 0; i < 3; i++)
            {
                Assert.IsTrue(practice.SelectAnchor(i, true, i));
                Assert.IsTrue(practice.SelectAnchor(i, false, i));
            }
        }
        static AlignmentPractice PerfectFit(int gap, int limit = 4)
        {
            var peaks = new[] { new AlignmentPeak("Wide", "wide", 0), new AlignmentPeak("Narrow", "narrow", 40), new AlignmentPeak("Shoulder", "shoulder", 100) };
            var arrangement = new AlignmentArrangement("Generic", peaks, peaks, 50, 50 + gap);
            var practice = new AlignmentPractice(new AlignmentPracticeDefinition(limit, 40, 1, -40, 40, 0, new[] { arrangement, arrangement }));
            SelectThree(practice);
            return practice;
        }
        [Test]
        public void SuggestionNeverAppliesOrLocksAndEveryValidDefaultOffsetKeepsOverlapUnknown()
        {
            var definition = Fixture(); var practice = new AlignmentPractice(definition); SelectThree(practice);
            Assert.IsFalse(practice.LockBaseline()); int original = practice.OffsetMinutes;
            Assert.IsTrue(practice.ProposeOffset()); Assert.AreEqual(original, practice.OffsetMinutes);
            Assert.AreEqual(AlignmentOrder.Indeterminate, practice.Order);
            Assert.IsTrue(practice.ApplyProposal()); Assert.IsFalse(practice.BaselineLocked); Assert.AreNotEqual(original, practice.OffsetMinutes);
            Assert.IsTrue(practice.LockBaseline()); Assert.AreEqual(AlignmentOrder.Indeterminate, practice.Order);
            int validOffsets = 0;
            for (int offset = definition.MinOffsetMinutes; offset <= definition.MaxOffsetMinutes; offset++)
            {
                practice.Reset(); SelectThree(practice); Assert.IsTrue(practice.AdjustOffset(offset - practice.OffsetMinutes));
                if (!practice.LockBaseline()) continue;
                validOffsets++;
                Assert.AreEqual(AlignmentOrder.Indeterminate, practice.Order, "An allowed baseline must not manufacture order for the overlapping fixture.");
            }
            Assert.Greater(validOffsets, 0, "The fixture must remain solvable.");
        }
        [Test]
        public void ClosedIntervalsRequireStrictSeparationAndALockedBaselineEvenWithZeroResidual()
        {
            foreach (int gap in new[] { -9, -8, -7, 0, 7, 8, 9 })
            {
                var practice = PerfectFit(gap);
                Assert.AreEqual(0, practice.ResidualMinutes);
                Assert.AreEqual(AlignmentOrder.Indeterminate, practice.Order);
                Assert.IsTrue(practice.LockBaseline());
                Assert.AreEqual(gap > 8 ? AlignmentOrder.ABeforeB : gap < -8 ? AlignmentOrder.BBeforeA : AlignmentOrder.Indeterminate, practice.Order);
                Assert.AreEqual(4, practice.EventErrorMinutes, "Fit residual is not the uncertainty floor.");
            }
        }
        [Test]
        public void MaximumResidualUsesAuthoredInclusiveLimitRatherThanAverageCancellation()
        {
            var practice = PerfectFit(20, 3);
            Assert.IsTrue(practice.AdjustOffset(3)); Assert.IsTrue(practice.LockBaseline());
            practice.UnlockBaseline(); Assert.IsTrue(practice.AdjustOffset(1)); Assert.IsFalse(practice.LockBaseline());
            var peaksA = new[] { new AlignmentPeak("W", "wide", 0), new AlignmentPeak("N", "narrow", 40), new AlignmentPeak("S", "shoulder", 100) };
            var peaksB = new[] { new AlignmentPeak("W", "wide", -5), new AlignmentPeak("N", "narrow", 40), new AlignmentPeak("S", "shoulder", 105) };
            var arrangement = new AlignmentArrangement("Generic", peaksA, peaksB, 50, 70);
            practice = new AlignmentPractice(new AlignmentPracticeDefinition(4, 40, 1, -40, 40, 0, new[] { arrangement, arrangement }));
            SelectThree(practice); practice.ProposeOffset(); practice.ApplyProposal();
            Assert.AreEqual(5, practice.ResidualMinutes); Assert.IsFalse(practice.LockBaseline());
        }
        [Test]
        public void ReusingOnePeakCannotProduceAValidBaselineEvenWithZeroResidual()
        {
            var practice = PerfectFit(20);
            for (int i = 0; i < 3; i++) { practice.SelectAnchor(i, true, 0); practice.SelectAnchor(i, false, 0); }
            Assert.AreEqual(0, practice.ResidualMinutes);
            Assert.AreEqual(AlignmentBaselineStatus.DuplicateAnchors, practice.Status);
            Assert.IsFalse(practice.ProposeOffset()); Assert.IsFalse(practice.LockBaseline());
        }
        [Test]
        public void WrongCorrespondenceCannotLockDespiteSmallResidual()
        {
            var a = new[] { new AlignmentPeak("W", "wide", 0), new AlignmentPeak("N", "narrow", 1), new AlignmentPeak("S", "shoulder", 2) };
            var arrangement = new AlignmentArrangement("Generic", a, a, 50, 70);
            var practice = new AlignmentPractice(new AlignmentPracticeDefinition(4, 40, 1, -40, 40, 0, new[] { arrangement, arrangement }));
            SelectThree(practice); practice.SelectAnchor(0, false, 1); practice.SelectAnchor(1, false, 0);
            Assert.AreEqual(1, practice.ResidualMinutes);
            Assert.AreEqual(AlignmentBaselineStatus.MismatchedPeaks, practice.Status); Assert.IsFalse(practice.LockBaseline());
        }
        [Test]
        public void LockedStateRejectsEditsAndTransferRequiresFreshCorrespondences()
        {
            var practice = new AlignmentPractice(Fixture()); SelectThree(practice); practice.ProposeOffset(); practice.ApplyProposal(); Assert.IsTrue(practice.LockBaseline());
            int offset = practice.OffsetMinutes;
            Assert.IsFalse(practice.AdjustOffset(1)); Assert.IsFalse(practice.SelectAnchor(0, false, 1));
            Assert.IsFalse(practice.ProposeOffset()); Assert.IsFalse(practice.ApplyProposal()); Assert.AreEqual(offset, practice.OffsetMinutes);
            Assert.IsTrue(practice.SelectArrangement(1)); Assert.IsFalse(practice.BaselineLocked); Assert.IsNull(practice.ProposedOffsetMinutes);
            Assert.IsTrue(Enumerable.Range(0, 3).All(i => practice.AnchorA(i) == -1 && practice.AnchorB(i) == -1));
            Assert.AreEqual(AlignmentOrder.Indeterminate, practice.Order); Assert.IsFalse(practice.LockBaseline());
            SelectThree(practice); practice.ProposeOffset(); practice.ApplyProposal(); Assert.IsTrue(practice.LockBaseline());
            Assert.AreEqual(AlignmentOrder.BBeforeA, practice.Order);
            practice.UnlockBaseline(); Assert.AreEqual(AlignmentOrder.Indeterminate, practice.Order);
            Assert.IsTrue(practice.AdjustOffset(1)); practice.Reset();
            Assert.AreEqual(AlignmentBaselineStatus.MissingAnchors, practice.Status); Assert.IsNull(practice.ProposedOffsetMinutes);
            Assert.AreEqual(practice.Definition.InitialOffsetMinutes, practice.OffsetMinutes);
        }
    }
}
#endif
