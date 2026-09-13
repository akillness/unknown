using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Tide.App
{
    public enum ActionFeedbackStage { None, Accepted, WorkSaved, Preview, CommitSaving, CommitSaved, SaveFailed }

    public sealed partial class T0GameSession
    {
        public ActionFeedbackStage FeedbackStage { get; private set; }
        int feedbackRevision,autosaveSequence;
        JObject feedbackStrings;

        string ActionFeedbackText
        {
            get
            {
                if (FeedbackStage == ActionFeedbackStage.None) return null;
                if (feedbackStrings == null) feedbackStrings = JObject.Parse(Resources.Load<TextAsset>("M23Feedback").text);
                var row = feedbackStrings[FeedbackStage.ToString()];
                return (string)row[(string)settings["language"]] ?? (string)row["ko"];
            }
        }

        void SetActionFeedback(ActionFeedbackStage stage)
        {
            FeedbackStage = stage;
            feedbackRevision++;
            Interface.SetActionFeedback(overlay == "alignmentPractice" ? null : ActionFeedbackText);
        }

        void ResetWorkspaceViews()
        {
            autosaveSequence++;
            ResetReaderComparison();
            ResetSignatureProofSelection();
            ResetAlignmentPractice();
            SetActionFeedback(ActionFeedbackStage.None);
        }
    }
}
