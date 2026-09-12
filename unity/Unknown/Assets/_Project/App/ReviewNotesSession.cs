using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Tide.Presentation;
using Tide.Save;
using Tide.UI;
using UnityEngine;

namespace Tide.App
{
    public sealed class ReviewNoteSource
    {
        public string Id, Label, Description, OriginId, SourceType;
    }

    public sealed partial class T0GameSession
    {
        ReviewNotesStore reviewStore;
        string reviewIdentity, reviewDraft = "", reviewStatus = "";
        readonly HashSet<string> reviewLinks = new HashSet<string>(StringComparer.Ordinal);
        Task reviewBackground = Task.CompletedTask;
        bool reviewSaving, reviewAvailabilityMayChange, reviewCanSaveAtRender;
        string reviewShownQuestion, reviewShownStructure, reviewShownSummary, reviewPreviousOverlay;
        bool reviewReturnPending; string reviewReturnFocusId, reviewReturnNode;
        bool reviewPanelPlaying, reviewPanelClosing, reviewQuestionPlaying;
        float reviewPanelElapsed, reviewPanelDuration, reviewQuestionElapsed, reviewQuestionDuration;
        string reviewPanelRestoreOverlay;
        JObject reviewVfx; bool reviewVfxLoaded;
        M8ReviewNotesProfile reviewProfile; bool reviewProfileLoaded;
        public string ReviewNoteText => reviewDraft;
        public ReviewNotesStore ReviewStore { get { BindReviewNotes(); return reviewStore; } }
        public string ReviewQuestion => ReviewQuestionFor(ObservedReviewSources().Where(source => reviewLinks.Contains(source.Id)).ToArray());
        public string ReviewShownQuestion => reviewShownQuestion;
        public bool ReviewPanelTransitionActive => reviewPanelPlaying;

        void BindReviewNotes()
        {
            var identity = Path.GetFullPath(Store.DirectoryPath) + "\n" + saveId;
            if (identity == reviewIdentity) return;
            reviewIdentity = identity;
            reviewStore = new ReviewNotesStore(Store.DirectoryPath, saveId);
            reviewDraft = "";
            reviewStatus = "";
            reviewLinks.Clear();
            reviewSaving = false;
            reviewShownQuestion = reviewShownStructure = reviewShownSummary = null;
            reviewPreviousOverlay = null;
            reviewReturnPending = false;
            reviewReturnFocusId = null;
            reviewPanelPlaying = reviewPanelClosing = reviewQuestionPlaying = false;
            reviewPanelRestoreOverlay = null;
            var note = reviewStore.Load();
            if (note == null) return;
            reviewDraft = (string)note["text"];
            foreach (var id in note["sourceIds"]) reviewLinks.Add((string)id);
        }

        public bool CanPersistReviewNote
        {
            get
            {
                if (saveReadOnly) return false;
                try
                {
                    var path = Path.Combine(Store.DirectoryPath, "save.json");
                    return File.Exists(path) && (string)SaveCodec.Decode(File.ReadAllText(path))["saveId"] == saveId;
                }
                catch (Exception e) when (e is IOException || e is UnauthorizedAccessException || e is InvalidOperationException || e is Newtonsoft.Json.JsonException) { return false; }
            }
        }

        void UpdateReviewNotesAvailability()
        {
            AdvanceReviewTransitions();
            ResumeAfterReviewSourceOriginal();
            if (!reviewAvailabilityMayChange) return;
            if (overlay != "reviewNotes") { reviewAvailabilityMayChange = false; return; }
            if (Interface.TextEntryActive || reviewSaving) return;
            reviewAvailabilityMayChange = false;
            if (reviewCanSaveAtRender != CanPersistReviewNote) Render();
        }

        public void OpenReviewNotes()
        {
            if (OpeningActive) return;
            reviewReturnPending = false;
            reviewReturnFocusId = null;
            if (overlay != "reviewNotes") reviewPreviousOverlay = overlay;
            overlay = "reviewNotes";
            Render();
            StartReviewPanelTransition(closing: false);
        }

        void CloseReviewNotes()
        {
            if (reviewPanelPlaying && reviewPanelClosing) return;
            reviewReturnPending = false;
            reviewReturnFocusId = null;
            var target = reviewPreviousOverlay ?? "evidence";
            reviewPreviousOverlay = null;
            if (ReviewVfxMs("close_ms") <= 0) { OpenOverlay(target); return; }
            reviewPanelRestoreOverlay = target;
            StartReviewPanelTransition(closing: true);
            var group = Interface.ReviewPanelGroup;
            if (group != null) { group.interactable = false; group.blocksRaycasts = false; }
        }

        void StartReviewPanelTransition(bool closing)
        {
            float ms = ReviewVfxMs(closing ? "close_ms" : "open_ms");
            if (ms <= 0)
            {
                reviewPanelPlaying = reviewPanelClosing = false;
                var settled = Interface.ReviewPanelGroup;
                if (settled != null) settled.alpha = 1;
                return;
            }
            reviewPanelPlaying = true;
            reviewPanelClosing = closing;
            reviewPanelElapsed = 0;
            reviewPanelDuration = ms / 1000f;
            var group = Interface.ReviewPanelGroup;
            if (group != null) group.alpha = closing ? 1 : 0;
        }

        // Question text is pinned when the player asks; screen rebuilds and link toggles never recompute it.
        void ShowReviewQuestion()
        {
            var selected = ObservedReviewSources().Where(source => reviewLinks.Contains(source.Id)).ToArray();
            reviewShownQuestion = ReviewQuestionFor(selected);
            reviewShownStructure = ReviewStructureKey(selected);
            reviewShownSummary = "연결 출처 " + selected.Length + "개 · 확인된 원본 " + selected.Where(source => !string.IsNullOrEmpty(source.OriginId)).Select(source => source.OriginId).Distinct().Count() + "개";
            Render();
            float ms = ReviewVfxMs("question_ms");
            if (ms <= 0) { reviewQuestionPlaying = false; return; }
            reviewQuestionPlaying = true;
            reviewQuestionElapsed = 0;
            reviewQuestionDuration = ms / 1000f;
            var group = Interface.ReviewQuestionGroup;
            if (group != null) group.alpha = 0;
        }

        // QA D-M9-01/D-M9-13: C1 stage screens and open tool panels both render ahead of DocumentScreen, and an open tool would
        // keep receiving Adjust/Query/Disconnect input under the document — refuse instead of leaving `document` set.
        bool ReviewSourceOriginalAvailable => !PatrolActive && !SignatureActive && tool == null;
        void OpenReviewSourceOriginal(string recordId, string sourceId)
        {
            if (!ReviewSourceOriginalAvailable) return;
            reviewReturnPending = true;
            reviewReturnFocusId = "review-source-" + sourceId;
            reviewReturnNode = node;
            reviewPanelPlaying = reviewPanelClosing = false;
            reviewPanelRestoreOverlay = null;
            overlay = null;
            document = recordId;
            Render();
        }

        // No new Update loop: the existing per-frame availability call observes the document field.
        void ResumeAfterReviewSourceOriginal()
        {
            if (!reviewReturnPending) return;
            if (overlay != null || tool != null || node != reviewReturnNode) { reviewReturnPending = false; reviewReturnFocusId = null; return; }
            if (document != null) return;
            reviewReturnPending = false;
            var focusId = reviewReturnFocusId;
            reviewReturnFocusId = null;
            overlay = "reviewNotes";
            Render();
            StartReviewPanelTransition(closing: false);
            if (focusId != null) Interface.Focus(focusId);
        }

        void AdvanceReviewTransitions()
        {
            if (reviewPanelPlaying)
            {
                reviewPanelElapsed += Time.unscaledDeltaTime;
                float progress = reviewPanelDuration <= 0 ? 1 : Mathf.Clamp01(reviewPanelElapsed / reviewPanelDuration);
                var group = Interface.ReviewPanelGroup;
                if (group != null) group.alpha = reviewPanelClosing ? 1 - progress : progress;
                if (progress >= 1)
                {
                    bool closing = reviewPanelClosing;
                    reviewPanelPlaying = reviewPanelClosing = false;
                    var target = reviewPanelRestoreOverlay;
                    reviewPanelRestoreOverlay = null;
                    if (closing && target != null && overlay == "reviewNotes") OpenOverlay(target);
                }
            }
            if (reviewQuestionPlaying)
            {
                reviewQuestionElapsed += Time.unscaledDeltaTime;
                float progress = reviewQuestionDuration <= 0 ? 1 : Mathf.Clamp01(reviewQuestionElapsed / reviewQuestionDuration);
                var group = Interface.ReviewQuestionGroup;
                if (group != null) group.alpha = progress;
                if (progress >= 1) reviewQuestionPlaying = false;
            }
        }

        // Timings are authored in Resources/M8ReviewVfx.json; a missing or invalid asset falls back to instant transitions.
        float ReviewVfxMs(string key)
        {
            if (!reviewVfxLoaded)
            {
                reviewVfxLoaded = true;
                var authored = Resources.Load<TextAsset>("M8ReviewVfx");
                if (authored != null) try { reviewVfx = JObject.Parse(authored.text); } catch (Newtonsoft.Json.JsonException) { reviewVfx = null; }
            }
            if (reviewVfx == null) return 0;
            return (float?)reviewVfx[ReducedMotion ? "reduced_motion_ms" : key] ?? 0;
        }

        M8ReviewNotesProfile ReviewProfile
        {
            get
            {
                if (!reviewProfileLoaded) { reviewProfileLoaded = true; reviewProfile = Resources.Load<M8ReviewNotesProfile>("M8ReviewNotes"); }
                return reviewProfile;
            }
        }

        bool ReviewDirectionEnabled => ReviewProfile != null && (ReviewProfile.runtimeApproved || Environment.GetCommandLineArgs().Contains("--m8-review-notes-diagnostic"));

        static string ReviewStructureKey(IReadOnlyList<ReviewNoteSource> sources) =>
            string.Join("\n", sources.Select(source => source.Id + "|" + source.OriginId + "|" + source.SourceType).OrderBy(key => key, StringComparer.Ordinal));

        public IReadOnlyList<ReviewNoteSource> ObservedReviewSources()
        {
            var result = new List<ReviewNoteSource>();
            foreach (var record in records["rows"].OfType<JObject>())
            {
                var id = (string)record["recordId"];
                if (!Definition.Records.TryGetValue(id, out var definition)) continue;
                var excerpts = new List<string>();
                foreach (var line in record["lines"] as JArray ?? new JArray())
                    if (Journal.State.Has("line:" + id + ":" + (string)line["lineId"])) excerpts.Add((string)line["text"]);
                foreach (var row in record["rows"] as JArray ?? new JArray())
                    if (Journal.State.Has("row:" + id + ":" + (string)row["rowId"])) excerpts.Add((string)row["item"] + " · " + (string)row["note"]);
                if (Journal.State.Has("citation:" + id)) excerpts.Add("핀으로 보관한 구간: " + Journal.State.Get("citationStart:" + id) + " → " + Journal.State.Get("citationEnd:" + id));
                if (excerpts.Count == 0) continue;
                result.Add(new ReviewNoteSource { Id = "record:" + id, Label = (string)record["displayNameKo"],
                    Description = string.Join("\n", excerpts), OriginId = Definition.ResolveRoot(id), SourceType = definition.SourceType });
            }
            foreach (var observation in patrolPacket["observations"])
            {
                var id = (string)observation["id"];
                if (!Journal.State.Has("c1:observed:" + id)) continue;
                result.Add(new ReviewNoteSource { Id = "patrol:" + id, Label = PatrolText((string)observation["labelKey"]),
                    Description = (string)observation["description"], OriginId = (string)observation["rootOriginId"] ?? (string)observation["originId"], SourceType = (string)observation["sourceType"] });
            }
            foreach (var observation in signaturePacket["observations"])
            {
                var id = (string)observation["id"];
                if (!SignatureHas("observed:" + id)) continue;
                result.Add(new ReviewNoteSource { Id = "signature:" + id, Label = SignatureText((string)observation["labelKey"]),
                    Description = (string)observation["description"], OriginId = (string)observation["rootOriginId"] ?? (string)observation["originId"], SourceType = (string)observation["sourceType"] });
            }
            return result;
        }

        public static string ReviewQuestionFor(IReadOnlyList<ReviewNoteSource> sources)
        {
            if (sources.Count == 0) return "지금 확인한 기록 중 메모와 함께 다시 볼 출처는 무엇인가요?";
            if (sources.Count == 1) return "이 출처에서 직접 관찰한 내용과 메모의 추측은 어떻게 나눌 수 있을까요?";
            if (sources.Where(source => !string.IsNullOrEmpty(source.OriginId)).GroupBy(source => source.OriginId).Any(group => group.Count() > 1))
                return "같은 원본에서 나온 기록을 따로 세고 있지는 않나요?";
            if (sources.Where(source => !string.IsNullOrEmpty(source.SourceType)).Select(source => source.SourceType).Distinct(StringComparer.Ordinal).Count() < 2)
                return "서로 다른 매체에서도 이 해석을 확인할 수 있나요?";
            return "연결한 출처들의 관찰 내용과 메모를 나란히 놓았을 때, 다시 확인할 부분은 무엇인가요?";
        }

        void ReviewNotesScreen(GameScreen screen)
        {
            BindReviewNotes();
            var sources = ObservedReviewSources();
            var selected = sources.Where(source => reviewLinks.Contains(source.Id)).ToArray();
            screen.Title = "검토 노트 · 대조 메모";
            screen.Body = "관찰한 출처를 연결하고, 직접 본 내용과 추측을 나누어 적어 보세요.\n메모는 정답 판정이나 진행 조건에 사용되지 않습니다. 비워 두어도 계속 진행할 수 있습니다.";
            screen.Status = reviewStatus;
            screen.CaseThread = null;
            bool staleQuestion = reviewShownQuestion != null && reviewShownStructure != ReviewStructureKey(selected);
            screen.ReviewNotes = new ReviewNotesView { Text = reviewDraft, Placeholder = "직접 본 내용 / 아직 추측인 내용",
                Help = "최대 4,096자 · Tab 또는 Esc로 편집 마침 · 보관 버튼으로 저장",
                Changed = value => { reviewDraft = value; reviewStatus = "보관하지 않은 초안"; },
                CardTexture = ReviewDirectionEnabled ? ReviewProfile.cardPaper : null,
                QuestionLabel = reviewShownQuestion == null ? null : staleQuestion ? "이전 질문 · 출처 연결이 바뀌었습니다. 검토 질문 보기를 다시 누르면 갱신합니다." : "검토 질문",
                Question = reviewShownQuestion == null ? null : reviewShownSummary + "\n" + reviewShownQuestion,
                QuestionAnchorId = "review-note-question" };
            screen.Actions.Add(A("review-note-edit", "메모 편집", Interface.BeginReviewEditing));
            var canSave = CanPersistReviewNote;
            reviewCanSaveAtRender = canSave;
            string explanation = reviewStore.ReadOnly ? "기존 메모 파일을 읽을 수 없어 보존했습니다. 현재 초안은 이 화면에만 남습니다." :
                !canSave ? (sources.Count == 0 ? "자료를 하나 확인한 뒤 메모를 보관할 수 있습니다. 지금 초안은 이 실행 중에만 유지됩니다." : "진행 기록이 보관되면 메모를 보관할 수 있습니다. 지금 초안은 이 실행 중에만 유지됩니다.") :
                "메모와 연결한 출처만 별도로 보관합니다.";
            screen.Actions.Add(A("review-note-save", reviewSaving ? "메모 보관 중" : "메모 보관", () => { reviewBackground = SaveReviewNoteAsync(); }, canSave && !reviewStore.ReadOnly && !reviewSaving, explanation));
            screen.Actions.Add(A("review-note-question", "검토 질문 보기", ShowReviewQuestion, true,
                "연결한 출처의 관계 요약과 저작된 검토 질문 하나를 보여줍니다. 누를 때에만 갱신하며 입력한 문장을 판정하지 않습니다."));
            foreach (var source in sources)
            {
                var item = source;
                var linked = reviewLinks.Contains(item.Id);
                screen.Actions.Add(A("review-source-" + item.Id, (linked ? "연결 해제 · " : "출처 연결 · ") + item.Label,
                    () => { if (!reviewLinks.Remove(item.Id)) reviewLinks.Add(item.Id); reviewStatus = "보관하지 않은 초안"; Render(); },
                    linked || selected.Length < ReviewNotesStore.SourceLimit,
                    item.Description + "\n매체: " + ReviewMediaName(item.SourceType) +
                    (!string.IsNullOrEmpty(item.OriginId) && selected.Any(other => other.Id != item.Id && other.OriginId == item.OriginId) ? " · 연결한 다른 출처와 원본이 같습니다." : "")));
                if (ReviewSourceOriginalAvailable && item.Id.StartsWith("record:", StringComparison.Ordinal))
                {
                    var recordId = item.Id.Substring("record:".Length);
                    screen.Actions.Add(A("review-open-" + item.Id, "원문 열기 · " + item.Label, () => OpenReviewSourceOriginal(recordId, item.Id)));
                }
            }
            if (sources.Count == 0) screen.Body += "\n\n아직 확인한 출처가 없습니다. 자료를 살펴본 뒤 이곳에서 연결할 수 있습니다.";
            screen.Actions.Add(A("review-note-back", "자료 검토로 돌아가기", CloseReviewNotes));
            screen.Actions.Add(A("overlay-back", L("back"), CloseReviewNotes));
        }

        internal static string ReviewMediaName(string sourceType)
        {
            switch (sourceType)
            {
                case "plate": return "기록판";
                case "log": return "문서 기록";
                case "ledger": return "장부";
                default: return "기록 매체";
            }
        }

        public async Task<bool> SaveReviewNoteAsync()
        {
            BindReviewNotes();
            if (reviewSaving || !CanPersistReviewNote || reviewStore.ReadOnly) return false;
            var target = reviewStore;
            var text = reviewDraft;
            var links = ObservedReviewSources().Where(source => reviewLinks.Contains(source.Id)).Select(source => source.Id).ToArray();
            reviewSaving = true;
            var success = await target.SaveAsync(text, links);
            if (target != reviewStore) return success;
            reviewSaving = false;
            var currentLinks = ObservedReviewSources().Where(source => reviewLinks.Contains(source.Id)).Select(source => source.Id);
            bool unchanged = reviewDraft == text && new HashSet<string>(links, StringComparer.Ordinal).SetEquals(currentLinks);
            reviewStatus = success ? (unchanged ? "메모를 보관했습니다." : "이전 초안을 보관했습니다. 변경한 내용은 아직 보관하지 않았습니다.") : "메모를 보관하지 못했습니다. 초안을 유지했습니다. 다시 시도해 주세요.";
            if (this != null && overlay == "reviewNotes" && !Interface.TextEntryActive) Render();
            return success;
        }
    }
}
