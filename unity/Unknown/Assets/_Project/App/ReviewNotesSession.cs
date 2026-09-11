using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Tide.Save;
using Tide.UI;

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
        public string ReviewNoteText => reviewDraft;
        public ReviewNotesStore ReviewStore { get { BindReviewNotes(); return reviewStore; } }
        public string ReviewQuestion => ReviewQuestionFor(ObservedReviewSources().Where(source => reviewLinks.Contains(source.Id)).ToArray());

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
            if (!reviewAvailabilityMayChange) return;
            if (overlay != "reviewNotes") { reviewAvailabilityMayChange = false; return; }
            if (Interface.TextEntryActive || reviewSaving) return;
            reviewAvailabilityMayChange = false;
            if (reviewCanSaveAtRender != CanPersistReviewNote) Render();
        }

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
            screen.ReviewNotes = new ReviewNotesView { Text = reviewDraft, Placeholder = "직접 본 내용 / 아직 추측인 내용",
                Help = "최대 4,096자 · Tab 또는 Esc로 편집 마침 · 보관 버튼으로 저장",
                Changed = value => { reviewDraft = value; reviewStatus = "보관하지 않은 초안"; } };
            screen.Actions.Add(A("review-note-edit", "메모 편집", Interface.BeginReviewEditing));
            var canSave = CanPersistReviewNote;
            reviewCanSaveAtRender = canSave;
            string explanation = reviewStore.ReadOnly ? "기존 메모 파일을 읽을 수 없어 보존했습니다. 현재 초안은 이 화면에만 남습니다." :
                !canSave ? (sources.Count == 0 ? "자료를 하나 확인한 뒤 메모를 보관할 수 있습니다. 지금 초안은 이 실행 중에만 유지됩니다." : "진행 기록이 보관되면 메모를 보관할 수 있습니다. 지금 초안은 이 실행 중에만 유지됩니다.") :
                "메모와 연결한 출처만 별도로 보관합니다.";
            screen.Actions.Add(A("review-note-save", reviewSaving ? "메모 보관 중" : "메모 보관", () => { reviewBackground = SaveReviewNoteAsync(); }, canSave && !reviewStore.ReadOnly && !reviewSaving, explanation));
            screen.Actions.Add(A("review-note-question", "다음 검토 질문", () => { }, false,
                ReviewQuestionFor(selected) + "\n연결 출처 " + selected.Length + "개 · 확인된 원본 " + selected.Where(source => !string.IsNullOrEmpty(source.OriginId)).Select(source => source.OriginId).Distinct().Count() + "개"));
            foreach (var source in sources)
            {
                var item = source;
                var linked = reviewLinks.Contains(item.Id);
                screen.Actions.Add(A("review-source-" + item.Id, (linked ? "연결 해제 · " : "출처 연결 · ") + item.Label,
                    () => { if (!reviewLinks.Remove(item.Id)) reviewLinks.Add(item.Id); reviewStatus = "보관하지 않은 초안"; Render(); },
                    linked || selected.Length < ReviewNotesStore.SourceLimit,
                    item.Description + "\n매체: " + ReviewMediaName(item.SourceType) +
                    (!string.IsNullOrEmpty(item.OriginId) && selected.Any(other => other.Id != item.Id && other.OriginId == item.OriginId) ? " · 연결한 다른 출처와 원본이 같습니다." : "")));
            }
            if (sources.Count == 0) screen.Body += "\n\n아직 확인한 출처가 없습니다. 자료를 살펴본 뒤 이곳에서 연결할 수 있습니다.";
            screen.Actions.Add(A("review-note-back", "자료 검토로 돌아가기", () => OpenOverlay("evidence")));
        }

        static string ReviewMediaName(string sourceType)
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
