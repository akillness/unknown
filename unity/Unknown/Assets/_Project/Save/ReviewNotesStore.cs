using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Tide.Save
{
    // This envelope is deliberately separate from JournalSave and gameplay recovery.
    public sealed class ReviewNotesStore
    {
        public const int Version = 1, TextLimit = 4096, SourceLimit = 32;
        readonly AtomicSaveStore writer;
        readonly string saveId;
        readonly SemaphoreSlim gate = new SemaphoreSlim(1, 1);
        public string FileName { get; }
        public string FilePath => Path.Combine(writer.DirectoryPath, FileName);
        public bool ReadOnly { get; private set; }
        public string Failure { get; private set; }
        public Func<SaveStage, CancellationToken, Task> Injection { get => writer.Injection; set => writer.Injection = value; }

        public ReviewNotesStore(string directory, string identity)
        {
            if (string.IsNullOrWhiteSpace(identity)) throw new ArgumentException("Notes require a save identity.");
            saveId = identity;
            writer = new AtomicSaveStore(directory);
            FileName = "review-notes-" + SaveCodec.Hash(identity) + ".json";
        }

        public JObject Load()
        {
            if (!File.Exists(FilePath)) return null;
            try
            {
                if (new FileInfo(FilePath).Length > 65536) throw new InvalidDataException();
                var note = SaveCodec.Decode(File.ReadAllText(FilePath));
                Validate(note);
                return note;
            }
            catch (Exception e) when (e is InvalidDataException || e is IOException || e is UnauthorizedAccessException || e is InvalidOperationException || e is Newtonsoft.Json.JsonException || e is ArgumentException)
            {
                ReadOnly = true;
                Failure = "NOTES_DOCUMENT_REFUSED";
                return null;
            }
        }

        void Validate(JObject note)
        {
            if (note["notesVersion"]?.Type != JTokenType.Integer || note["notesVersion"].ToString() != Version.ToString() ||
                note["kind"]?.Type != JTokenType.String || (string)note["kind"] != "review-note" ||
                note["saveId"]?.Type != JTokenType.String || (string)note["saveId"] != saveId ||
                note["text"]?.Type != JTokenType.String || ((string)note["text"]).Length > TextLimit ||
                !(note["sourceIds"] is JArray ids) || ids.Count > SourceLimit ||
                ids.Any(id => id.Type != JTokenType.String || string.IsNullOrEmpty((string)id) || ((string)id).Length > 256) ||
                ids.Select(id => (string)id).Distinct(StringComparer.Ordinal).Count() != ids.Count)
                throw new InvalidDataException();
        }

        public async Task<bool> SaveAsync(string text, IEnumerable<string> sourceIds)
        {
            var note = new JObject { ["kind"] = "review-note", ["notesVersion"] = Version, ["saveId"] = saveId,
                ["text"] = text ?? "", ["sourceIds"] = new JArray(sourceIds ?? Array.Empty<string>()) };
            await gate.WaitAsync();
            try
            {
                if (ReadOnly) return false;
                Load(); // Refuse an incompatible document even if it appeared after this session opened.
                if (ReadOnly) return false;
                Validate(note);
                await writer.WriteAsync(note, FileName);
                Failure = null;
                return true;
            }
            catch (Exception e) when (e is InvalidDataException || e is IOException || e is UnauthorizedAccessException || e is InvalidOperationException || e is Newtonsoft.Json.JsonException || e is ArgumentException)
            {
                Failure = "NOTES_SAVE_FAILED";
                return false;
            }
            finally { gate.Release(); }
        }
    }
}
