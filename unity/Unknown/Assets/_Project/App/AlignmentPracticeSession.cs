using System;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Linq;
using Tide.Sim;
using Tide.UI;
using UnityEngine;

namespace Tide.App
{
    public static class AlignmentPracticeData
    {
        public static AlignmentPracticeDefinition Load(JObject packet)
        {
            if ((int?)packet["schemaVersion"] != 1 || (string)packet["scope"] != "isolated-practice")
                throw new InvalidOperationException("Invalid isolated alignment practice data.");
            var limits = packet["limits"];
            return new AlignmentPracticeDefinition((int)limits["residualLimitMinutes"], (int)limits["rawErrorMinutes"],
                (int)limits["stepMinutes"], (int)limits["minOffsetMinutes"], (int)limits["maxOffsetMinutes"], (int)limits["initialOffsetMinutes"],
                packet["arrangements"].Select(a => new AlignmentArrangement((string)a["label"],
                    a["trackA"].Select(Peak), a["trackB"].Select(Peak), (int)a["eventA"], (int)a["eventB"])));
        }
        static AlignmentPeak Peak(JToken peak) => new AlignmentPeak((string)peak["label"], (string)peak["shape"], (int)peak["minute"]);
    }

    public sealed partial class T0GameSession
    {
        AlignmentPractice alignmentPractice;
        int alignmentAnchorSlot;
        bool alignmentLockPreview;
        public AlignmentPractice Practice => alignmentPractice;
        public bool AlignmentPracticeActive => overlay == "alignmentPractice";
        bool AlignmentPracticeAvailable
        {
            get
            {
#if UNITY_EDITOR
                return true;
#else
                return Environment.GetCommandLineArgs().Contains("--m23-alignment-practice");
#endif
            }
        }

        public void OpenAlignmentPractice()
        {
            if (!AlignmentPracticeAvailable || !started || OpeningActive || SavePending) return;
            if (alignmentPractice == null)
                alignmentPractice = new AlignmentPractice(AlignmentPracticeData.Load(JObject.Parse(Resources.Load<TextAsset>("M23AlignmentPractice").text)));
            alignmentLockPreview = false;
            overlay = "alignmentPractice";
            Render();
        }

        void ResetAlignmentPractice()
        {
            alignmentPractice = null;
            alignmentAnchorSlot = 0;
            alignmentLockPreview = false;
        }

        void PracticeChange(Action change)
        {
            if (!AlignmentPracticeActive) return;
            alignmentLockPreview = false;
            change();
            Render();
        }

        void AlignmentPracticeScreen(GameScreen screen)
        {
            if (alignmentPractice == null)
                alignmentPractice = new AlignmentPractice(AlignmentPracticeData.Load(JObject.Parse(Resources.Load<TextAsset>("M23AlignmentPractice").text)));
            var practice = alignmentPractice;
            screen.Title = "조위 정합 · 별도 연습장";
            screen.Subtitle = practice.Arrangement.Label + " · 가상 시편 · 저장하지 않음";
            screen.Body = null;
            screen.Status = "세 대응점의 A와 B 피크를 직접 고르세요. 제안은 수동 적용 전까지 시간축을 바꾸지 않습니다.\n작은 잔차는 시계 정합의 근거일 뿐 인과관계의 증거가 아닙니다. 연습은 원본·읽기 횟수·진행·저장을 바꾸지 않습니다.";
            screen.Footer = "Tab / 방향키: 조작 선택 · Enter: 선택 실행 · Esc: 연습장 나가기";
            screen.Navigation.Clear(); screen.Toolbar.Clear(); screen.Actions.Clear();
            screen.CaseThread = null; screen.Chart = null; screen.ShowDirection = false; screen.SignaturePaper = null;
            screen.AlignmentPractice = new AlignmentPracticeView
            {
                TrackA = practice.Arrangement.TrackA.ToArray(), TrackB = practice.Arrangement.TrackB.ToArray(),
                OffsetMinutes = practice.OffsetMinutes, EventA = practice.EventAMinutes, EventB = practice.EventBMinutes,
                ErrorMinutes = practice.EventErrorMinutes, RawErrorMinutes = practice.Definition.RawErrorMinutes,
                BaselineLocked = practice.BaselineLocked,
                StateLabel = PracticeStatus(practice), OrderLabel = PracticeOrder(practice),
                AnchorLabel = string.Join("\n", Enumerable.Range(0, 3).Select(i => "대응 " + (i + 1) + ": A "
                    + PracticePeak(practice.Arrangement.TrackA, practice.AnchorA(i)) + " ↔ B " + PracticePeak(practice.Arrangement.TrackB, practice.AnchorB(i))))
            };
            screen.Navigation.Add(A("practice-read-up", "판독 설명 한 쪽 위로", () => Interface.ScrollWork(-1)));
            screen.Navigation.Add(A("practice-read-down", "판독 설명 한 쪽 아래로", () => Interface.ScrollWork(1)));
            for (int i = 0; i < 3; i++)
            {
                int slot = i;
                screen.Navigation.Add(A("practice-slot-" + i, (alignmentAnchorSlot == i ? "선택 중 · " : "") + "대응 " + (i + 1),
                    () => PracticeChange(() => alignmentAnchorSlot = slot), !practice.BaselineLocked));
            }
            AddPracticeCandidates(screen, practice, true);
            AddPracticeCandidates(screen, practice, false);
            screen.Navigation.Add(A("practice-minus", "B 오프셋 −" + practice.Definition.StepMinutes + "분",
                () => PracticeChange(() => practice.AdjustOffset(-1)), !practice.BaselineLocked && practice.OffsetMinutes > practice.Definition.MinOffsetMinutes));
            screen.Navigation.Add(A("practice-plus", "B 오프셋 +" + practice.Definition.StepMinutes + "분",
                () => PracticeChange(() => practice.AdjustOffset(1)), !practice.BaselineLocked && practice.OffsetMinutes < practice.Definition.MaxOffsetMinutes));
            bool allDistinct = Enumerable.Range(0, 3).All(i => practice.AnchorA(i) >= 0 && practice.AnchorB(i) >= 0)
                && Enumerable.Range(0, 3).Select(practice.AnchorA).Distinct().Count() == 3 && Enumerable.Range(0, 3).Select(practice.AnchorB).Distinct().Count() == 3;
            screen.Navigation.Add(A("practice-propose", "선택한 대응점으로 오프셋 제안 보기",
                () => PracticeChange(() => practice.ProposeOffset()), allDistinct && !practice.BaselineLocked));
            screen.Navigation.Add(A("practice-apply", practice.ProposedOffsetMinutes.HasValue ? "제안 " + Signed(practice.ProposedOffsetMinutes.Value) + "분 수동 적용" : "제안 없음 · 먼저 대응점 세 개 선택",
                () => PracticeChange(() => practice.ApplyProposal()), practice.ProposedOffsetMinutes.HasValue && !practice.BaselineLocked));
            screen.Navigation.Add(A("practice-lock", alignmentLockPreview ? "기준선 잠금 확정 · 연습에만 적용" : "기준선 잠금 검토",
                () => { if (alignmentLockPreview) PracticeChange(() => practice.LockBaseline()); else { alignmentLockPreview = true; Render(); } },
                practice.Status == AlignmentBaselineStatus.Ready));
            screen.Navigation.Add(A("practice-unlock", "기준선 잠금 해제 · 다시 정합",
                () => PracticeChange(practice.UnlockBaseline), practice.BaselineLocked));
            screen.Navigation.Add(A("practice-clear", "대응점 지우기", () => PracticeChange(practice.ClearAnchors)));
            screen.Toolbar.Add(A("practice-transfer", "다른 배치", () => PracticeChange(() => { practice.SelectArrangement((practice.ArrangementIndex + 1) % practice.Definition.Arrangements.Count); alignmentAnchorSlot = 0; })));
            screen.Toolbar.Add(A("practice-reset", "현재 시편 초기화", () => PracticeChange(() => { practice.Reset(); alignmentAnchorSlot = 0; })));
            screen.Toolbar.Add(A("practice-exit", "연습장 나가기", Back));
        }

        void AddPracticeCandidates(GameScreen screen, AlignmentPractice practice, bool trackA)
        {
            var peaks = trackA ? practice.Arrangement.TrackA : practice.Arrangement.TrackB;
            string track = trackA ? "A" : "B";
            int selected = trackA ? practice.AnchorA(alignmentAnchorSlot) : practice.AnchorB(alignmentAnchorSlot);
            for (int i = 0; i < peaks.Count; i++)
            {
                int candidate = i;
                screen.Navigation.Add(A("practice-" + track.ToLowerInvariant() + "-" + i,
                    "대응 " + (alignmentAnchorSlot + 1) + " · " + track + " " + peaks[i].Label + " " + Phase(peaks[i].Minute) + (selected == i ? " · 선택됨" : ""),
                    () => PracticeChange(() => practice.SelectAnchor(alignmentAnchorSlot, trackA, candidate)), !practice.BaselineLocked));
            }
        }
        static string PracticePeak(System.Collections.Generic.IReadOnlyList<AlignmentPeak> peaks, int index) => index < 0 ? "미선택" : peaks[index].Label;
        static string Signed(int value) => value.ToString("+0;-0;0", CultureInfo.InvariantCulture);
        static string Phase(int minute) => "H" + Signed(minute) + "분";
        static string PracticeStatus(AlignmentPractice practice)
        {
            string residual = practice.ResidualMinutes.HasValue ? practice.ResidualMinutes.Value + "분" : "미계산";
            string summary = "B 오프셋 " + Signed(practice.OffsetMinutes) + "분 · 최대 잔차 " + residual + " / 한도 " + practice.Definition.ResidualLimitMinutes + "분\n";
            switch (practice.Status)
            {
                case AlignmentBaselineStatus.MissingAnchors: return summary + "미정합 · 세 대응점의 A와 B를 모두 선택하세요.";
                case AlignmentBaselineStatus.DuplicateAnchors: return summary + "잠금 불가 · 같은 피크를 여러 대응점에 사용했습니다.";
                case AlignmentBaselineStatus.MismatchedPeaks: return summary + "잠금 불가 · 대응점의 피크 형태가 서로 다릅니다.";
                case AlignmentBaselineStatus.ResidualExceeded: return summary + "잠금 불가 · 잔차가 한도를 초과했습니다.";
                case AlignmentBaselineStatus.Ready: return summary + "잠금 가능 · 아직 기준선이 확정되지 않았습니다.";
                default: return summary + "기준선 잠김 · 각 사건에는 별도의 보수적 오차폭을 적용합니다.";
            }
        }
        static string PracticeOrder(AlignmentPractice practice)
        {
            if (!practice.BaselineLocked) return "판정 불가 · 기준선 미확정. 조절 중인 B 좌표는 비교용 미리보기입니다.";
            if (practice.Order == AlignmentOrder.Indeterminate) return "판정 불가 · 사건 구간이 겹치거나 닿습니다. 실패가 아니라 남아 있는 불확실성입니다.";
            return (practice.Order == AlignmentOrder.ABeforeB ? "사건 A가 사건 B보다 앞섭니다." : "사건 B가 사건 A보다 앞섭니다.") + " 선후만 판정하며 원인은 알 수 없습니다.";
        }
    }
}
