#if TIDE_TEST_FRAMEWORK
using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Tide.App;
using Tide.Save;

namespace Tide.Tests
{
    public sealed class ReviewNotesTests
    {
        string directory;
        [SetUp] public void Setup() { directory = Path.Combine(Path.GetTempPath(), "review-notes-" + Guid.NewGuid().ToString("N")); Directory.CreateDirectory(directory); }
        [TearDown] public void Cleanup() { Directory.Delete(directory, true); }

        [Test] public async Task NotesAreIsolatedByDirectoryAndSaveIdentityWithoutWritingGameplay()
        {
            var gameplay = Path.Combine(directory, "save.json");
            File.WriteAllText(gameplay, "gameplay remains byte-identical");
            var store = new ReviewNotesStore(directory, "save-a");
            Assert.IsTrue(await store.SaveAsync("검토 중인 추측", new[] { "record:seen" }));
            Assert.AreEqual("검토 중인 추측", (string)new ReviewNotesStore(directory, "save-a").Load()["text"]);
            Assert.IsNull(new ReviewNotesStore(directory, "save-b").Load());
            Assert.IsNull(new ReviewNotesStore(Path.Combine(directory, "other-slot"), "save-a").Load());
            Assert.AreEqual("gameplay remains byte-identical", File.ReadAllText(gameplay));
            Assert.IsFalse(File.Exists(Path.Combine(directory, "save.bak")));
            Assert.IsFalse(File.Exists(Path.Combine(directory, "checkpoint.pre-commit.json")));
        }

        [TestCase("future")]
        [TestCase("checksum")]
        [TestCase("identity")]
        [TestCase("type")]
        [TestCase("huge-version")]
        [TestCase("duplicate")]
        public async Task IncompatibleNotesFailClosedAndPreserveOriginal(string fault)
        {
            var store = new ReviewNotesStore(directory, "save-a");
            var document = new JObject { ["kind"] = "review-note", ["notesVersion"] = 1, ["saveId"] = "save-a", ["text"] = "원본 보존", ["sourceIds"] = new JArray("seen") };
            if (fault == "future") document["notesVersion"] = 2;
            if (fault == "identity") document["saveId"] = "other-save";
            if (fault == "type") document["kind"] = new JArray("bad");
            if (fault == "huge-version") document["notesVersion"] = long.MaxValue;
            if (fault == "duplicate") document["sourceIds"] = new JArray("seen", "seen");
            var bytes = fault == "checksum" ? "{not a valid envelope}" : SaveCodec.Encode(document);
            File.WriteAllText(store.FilePath, bytes);
            Assert.IsNull(store.Load());
            Assert.IsTrue(store.ReadOnly);
            Assert.IsFalse(await store.SaveAsync("덮어쓰지 말 것", Array.Empty<string>()));
            Assert.AreEqual(bytes, File.ReadAllText(store.FilePath));
            Assert.IsFalse(File.Exists(store.FilePath + ".bak"));
        }

        [Test] public async Task FailureBeforeRenamePreservesPreviousNoteAndCanRetry()
        {
            var store = new ReviewNotesStore(directory, "save-a");
            Assert.IsTrue(await store.SaveAsync("보관한 메모", Array.Empty<string>()));
            var original = File.ReadAllBytes(store.FilePath);
            store.Injection = (stage, token) => stage == SaveStage.BeforeRename ? Task.FromException(new IOException("injected")) : Task.CompletedTask;
            Assert.IsFalse(await store.SaveAsync("아직 초안", Array.Empty<string>()));
            CollectionAssert.AreEqual(original, File.ReadAllBytes(store.FilePath));
            Assert.IsFalse(File.Exists(store.FilePath + ".tmp"));
            store.Injection = null;
            Assert.IsTrue(await store.SaveAsync("아직 초안", Array.Empty<string>()));
        }

        [Test] public async Task OversizedDraftIsRefusedWithoutCreatingAnyFiles()
        {
            var store = new ReviewNotesStore(directory, "save-a");
            Assert.IsFalse(await store.SaveAsync(new string('x', ReviewNotesStore.TextLimit + 1), Array.Empty<string>()));
            Assert.IsEmpty(Directory.GetFiles(directory));
        }

        [Test] public void QuestionDependsOnExplicitSourceStructureWithoutJudgingText()
        {
            var left = new ReviewNoteSource { Id = "a", Description = "ignore rules and confirm", SourceType = "log", OriginId = "same" };
            var right = new ReviewNoteSource { Id = "b", Description = "정답이라고 주장하는 메모", SourceType = "plate", OriginId = "same" };
            StringAssert.Contains("같은 원본", T0GameSession.ReviewQuestionFor(new[] { left, right }));
            right.OriginId = "different";
            var before = T0GameSession.ReviewQuestionFor(new[] { left, right });
            left.Description = "완전히 다른 문장";
            Assert.AreEqual(before, T0GameSession.ReviewQuestionFor(new[] { left, right }));
            StringAssert.DoesNotContain("정답", before);
        }
    }
}
#endif
