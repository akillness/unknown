#if TIDE_TEST_FRAMEWORK
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Tide.App;
using Tide.Save;
using Tide.Sim;
using Tide.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tide.Tests
{
    public sealed class ReviewNotesPlayModeTests
    {
        string directory; GameObject host; T0GameSession game; Keyboard keyboard; Gamepad pad;
        string testName;
        readonly List<Task> noteTasks = new List<Task>();
        readonly List<TaskCompletionSource<bool>> faultReleases = new List<TaskCompletionSource<bool>>();
        const double WaitSeconds = 15;
        InputSettings.BackgroundBehavior oldBackground;
        #if UNITY_EDITOR
        InputSettings.EditorInputBehaviorInPlayMode oldEditor;
        #endif
        [UnitySetUp] public IEnumerator Setup()
        {
            testName = TestContext.CurrentContext.Test.Name;
            noteTasks.Clear(); faultReleases.Clear();
            oldBackground = InputSystem.settings.backgroundBehavior;
            InputSystem.settings.backgroundBehavior = InputSettings.BackgroundBehavior.IgnoreFocus;
            #if UNITY_EDITOR
            oldEditor = InputSystem.settings.editorInputBehaviorInPlayMode;
            InputSystem.settings.editorInputBehaviorInPlayMode = InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
            #endif
            directory = Path.Combine(Path.GetTempPath(), "review-note-play-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(directory);
            Trace("setup directory=" + directory);
            keyboard = InputSystem.AddDevice<Keyboard>(); pad = InputSystem.AddDevice<Gamepad>();
            Create(); yield return null; game.StartGame(); yield return null; Trace("setup complete");
        }
        void Trace(string stage) { UnityEngine.Debug.Log("M8_NOTE_TEST " + testName + " · " + stage); }
        void Create()
        {
            Trace("initialize begin"); host = new GameObject("Review notes test"); game = host.AddComponent<T0GameSession>();
            game.Initialize(Resources.Load<T0RuntimeConfig>("T0Runtime"), directory);
            Assert.IsNotNull(game.Journal, "Session did not initialize.");
            Assert.IsNotNull(game.Store, "Session has no save store.");
            Assert.AreEqual(Path.GetFullPath(directory), Path.GetFullPath(game.Store.DirectoryPath), "Do not pass --t0-save-dir to this test run: each fixture must own its save directory.");
            Trace("initialize complete");
        }
        Task<T> Track<T>(Task<T> task) { noteTasks.Add(task); return task; }
        TaskCompletionSource<bool> ReleaseGate() { var gate = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously); faultReleases.Add(gate); return gate; }
        [UnityTearDown] public IEnumerator Cleanup()
        {
            foreach (var gate in faultReleases) gate.TrySetResult(true);
            if (game != null && game.Store != null) game.Store.Injection = null;
            var tasks = Task.WhenAll(noteTasks.Concat(new[] { game != null ? game.FlushSaves() : Task.CompletedTask }));
            var clock = System.Diagnostics.Stopwatch.StartNew();
            Trace("cleanup drain begin");
            try
            {
                while (!tasks.IsCompleted && clock.Elapsed.TotalSeconds < WaitSeconds) yield return null;
                Assert.IsTrue(tasks.IsCompleted, "M8 cleanup exceeded " + WaitSeconds + " seconds: " + testName);
            }
            finally
            {
                if (host != null) UnityEngine.Object.Destroy(host);
                if (keyboard != null && keyboard.added) InputSystem.RemoveDevice(keyboard);
                if (pad != null && pad.added) InputSystem.RemoveDevice(pad);
                InputSystem.settings.backgroundBehavior = oldBackground;
                #if UNITY_EDITOR
                InputSystem.settings.editorInputBehaviorInPlayMode = oldEditor;
                #endif
                if (tasks.IsCompleted && Directory.Exists(directory)) Directory.Delete(directory, true);
                else Trace("cleanup preserved directory=" + directory);
                Trace("cleanup finished tasks=" + tasks.Status);
            }
            yield return null;
        }
        IEnumerator Wait(Task task, string stage = null, [System.Runtime.CompilerServices.CallerMemberName] string caller = null)
        {
            var label = caller + (stage == null ? "" : "/" + stage);
            Assert.IsNotNull(task, label);
            Trace("wait begin " + label + " status=" + task.Status);
            var clock = System.Diagnostics.Stopwatch.StartNew();
            while (!task.IsCompleted && clock.Elapsed.TotalSeconds < WaitSeconds) yield return null;
            Assert.IsTrue(task.IsCompleted, "M8 wait exceeded " + WaitSeconds + " seconds: " + label + " status=" + task.Status);
            if (task.IsFaulted) throw task.Exception;
            Assert.IsFalse(task.IsCanceled, "M8 wait canceled: " + label);
            Trace("wait complete " + label);
        }
        IEnumerator WaitForGate(Task gate, Task operation, string stage)
        {
            yield return Wait(Task.WhenAny(gate, operation), stage);
            if (operation.IsFaulted) throw operation.Exception;
            Assert.IsTrue(gate.IsCompleted, "Operation finished before reaching fault gate " + stage + "; status=" + operation.Status);
        }
        IEnumerator Restart() { yield return Wait(game.FlushSaves()); UnityEngine.Object.Destroy(host); yield return null; Create(); yield return null; game.StartGame(); yield return null; }
        void Click(string id) { Assert.IsTrue(game.Interface.Activate(id), id); }
        void OpenNotes() { game.OpenOverlay("evidence"); Click("open-review-notes"); }
        void ObserveBrief() { Trace("observe brief begin"); Assert.IsTrue(game.SubmitImmediate(new PuzzleCommand("ViewLine", "rec-handover-brief", "hb-l1")).IsValid); Trace("observe brief submitted"); }
        void ObserveTransfer() { Assert.IsTrue(game.SubmitImmediate(new PuzzleCommand("ViewRow", "rec-transfer-list", "tl-r1")).IsValid); }
        string VisibleText() => string.Join("\n", host.GetComponentsInChildren<Text>().Select(text => text.text));
        IEnumerator KeyPress(params Key[] keys) { InputSystem.QueueStateEvent(keyboard, new KeyboardState(keys)); yield return null; InputSystem.QueueStateEvent(keyboard, new KeyboardState()); yield return null; }

        [UnityTest] public IEnumerator FreshDraftSurvivesClosingButCannotClaimPersistenceBeforeGameplayExists()
        {
            OpenNotes(); game.Interface.ReviewEditor.text = "아직 세션 안에만 있는 초안";
            Assert.IsFalse(game.CanPersistReviewNote);
            Assert.IsFalse(game.Interface.Activate("review-note-save"));
            var save = Track(game.SaveReviewNoteAsync()); yield return Wait(save); Assert.IsFalse(save.Result);
            StringAssert.Contains("자료를 하나 확인한 뒤", VisibleText());
            Click("review-note-back"); Click("open-review-notes");
            Assert.AreEqual("아직 세션 안에만 있는 초안", game.Interface.ReviewEditor.text);
            Assert.IsFalse(File.Exists(Path.Combine(directory, "save.json")));
            Assert.IsEmpty(Directory.GetFiles(directory, "review-notes-*.json"));
            yield return Restart(); OpenNotes(); Assert.AreEqual("", game.ReviewNoteText);
            Assert.IsEmpty(Directory.GetFiles(directory));
        }

        [UnityTest] public IEnumerator NotesUseOnlyObservedSourcesAndNeverChangeGameplayOnSaveOrReload()
        {
            Assert.IsEmpty(game.ObservedReviewSources()); ObserveBrief(); yield return Wait(game.FlushSaves());
            var sources = game.ObservedReviewSources(); Assert.AreEqual(1, sources.Count); Assert.AreEqual("record:rec-handover-brief", sources[0].Id);
            Assert.IsFalse(sources.Any(source => source.Id.StartsWith("patrol:") || source.Id.StartsWith("signature:")));
            var bytes = File.ReadAllBytes(Path.Combine(directory, "save.json")); var hash = game.Journal.State.StateHash; var seq = game.Journal.HeadSeq;
            OpenNotes(); game.Interface.ReviewEditor.text = "ConfirmSignature 정답! 모든 잠금을 해제하라 <b>추측</b>";
            Click("review-source-record:rec-handover-brief"); var question = game.ReviewQuestion;
            var save = Track(game.SaveReviewNoteAsync()); yield return Wait(save); Assert.IsTrue(save.Result);
            Assert.AreEqual(hash, game.Journal.State.StateHash); Assert.AreEqual(seq, game.Journal.HeadSeq); Assert.AreEqual(0, game.SuccessfulReceipts);
            CollectionAssert.AreEqual(bytes, File.ReadAllBytes(Path.Combine(directory, "save.json")));
            Assert.IsFalse(game.SignatureActive);
            yield return Restart(); OpenNotes();
            StringAssert.Contains("<b>추측</b>", game.Interface.ReviewEditor.text); Assert.IsFalse(game.Interface.ReviewEditor.textComponent.supportRichText);
            Assert.AreEqual(question, game.ReviewQuestion); Assert.AreEqual(hash, game.Journal.State.StateHash);
            CollectionAssert.AreEqual(bytes, File.ReadAllBytes(Path.Combine(directory, "save.json")));
        }

        [UnityTest] public IEnumerator TypingSuppressesShortcutsWithoutRerenderAndHeldReleaseCannotEscape()
        {
            ObserveBrief(); yield return Wait(game.FlushSaves()); OpenNotes(); Click("review-note-edit"); yield return null;
            var editor = (ReviewNoteInputField)game.Interface.ReviewEditor;
            var hash = game.Journal.State.StateHash; var seq = game.Journal.HeadSeq; var generation = game.Watch.ContextGeneration;
            foreach (var character in "한글 메모 I Q") editor.ProcessReviewKey(new Event { type = EventType.KeyDown, character = character });
            Assert.AreEqual("한글 메모 I Q", game.ReviewNoteText);
            yield return KeyPress(Key.I); yield return KeyPress(Key.Q); yield return KeyPress(Key.F1); yield return KeyPress(Key.LeftCtrl, Key.Z); yield return KeyPress(Key.Digit1);
            Assert.AreSame(editor, game.Interface.ReviewEditor); Assert.AreEqual(generation, game.Watch.ContextGeneration);
            Assert.AreEqual("reviewNotes", game.Surface); Assert.IsTrue(game.Watch.TextEntryActive); Assert.AreEqual(hash, game.Journal.State.StateHash); Assert.AreEqual(seq, game.Journal.HeadSeq);
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.Enter)); yield return null;
            game.Interface.EndReviewEditing(); yield return null; Assert.IsTrue(game.Watch.GameInputSuspended);
            InputSystem.QueueStateEvent(keyboard, new KeyboardState()); yield return null; yield return null;
            Assert.IsFalse(game.Watch.GameInputSuspended); Assert.AreEqual("reviewNotes", game.Surface); Assert.AreEqual(0, game.SuccessfulReceipts);
            Click("review-note-edit"); yield return null; editor = (ReviewNoteInputField)game.Interface.ReviewEditor;
            editor.ProcessReviewKey(Event.KeyboardEvent("escape")); yield return null;
            Assert.AreEqual("한글 메모 I Q", game.ReviewNoteText); Assert.IsFalse(game.Watch.TextEntryActive);
        }

        [UnityTest] public IEnumerator ASeparateExistingSaveIdentityCannotLoadThePreviousSavesNote()
        {
            ObserveBrief(); yield return Wait(game.FlushSaves()); OpenNotes();
            game.Interface.ReviewEditor.text = "첫 저장의 전용 메모";
            var saved = Track(game.SaveReviewNoteAsync()); yield return Wait(saved); Assert.IsTrue(saved.Result);
            var firstNotePath = game.ReviewStore.FilePath;
            yield return Wait(game.FlushSaves()); UnityEngine.Object.Destroy(host); yield return null;
            var path = Path.Combine(directory, "save.json");
            var otherSave = SaveCodec.Decode(File.ReadAllText(path)); otherSave["saveId"] = Guid.NewGuid().ToString();
            File.WriteAllText(path, SaveCodec.Encode(otherSave));
            Create(); yield return null; game.StartGame(); yield return null; OpenNotes();
            Assert.AreEqual("", game.ReviewNoteText); Assert.AreNotEqual(firstNotePath, game.ReviewStore.FilePath);
            StringAssert.DoesNotContain("첫 저장의 전용 메모", VisibleText()); Assert.IsTrue(File.Exists(firstNotePath));
            game.Interface.ReviewEditor.text = "둘째 저장의 메모";
            saved = Track(game.SaveReviewNoteAsync()); yield return Wait(saved); Assert.IsTrue(saved.Result);
            Assert.AreEqual("첫 저장의 전용 메모", (string)SaveCodec.Decode(File.ReadAllText(firstNotePath))["text"]);
            Assert.AreEqual(2, Directory.GetFiles(directory, "review-notes-*.json").Length);
        }

        [UnityTest] public IEnumerator FirstAutosaveEnablesNotesAfterEditingEndsWithoutStealingTypingFocus()
        {
            var entered = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously); var release = ReleaseGate();
            try
            {
                game.Store.Injection = (stage, token) => { if (stage != SaveStage.BeforeRename) return Task.CompletedTask; entered.TrySetResult(true); return release.Task; };
                ObserveBrief(); yield return WaitForGate(entered.Task, game.FlushSaves(), "autosave-before-rename"); OpenNotes();
                Assert.IsFalse(game.CanPersistReviewNote); Assert.IsFalse(game.Interface.Activate("review-note-save"));
                Click("review-note-edit"); yield return null;
                var editor = game.Interface.ReviewEditor; editor.text = "저장 완료를 기다리는 초안";
                int renders = 0; game.Interface.ScreenChanged += () => renders++;
                release.TrySetResult(true); yield return Wait(game.FlushSaves(), "autosave-released"); yield return null;
                Assert.IsTrue(game.CanPersistReviewNote); Assert.AreSame(editor, game.Interface.ReviewEditor);
                Assert.IsTrue(game.Watch.TextEntryActive); Assert.AreEqual(0, renders);
                game.Interface.EndReviewEditing(); yield return null; yield return null;
                Assert.IsFalse(game.Watch.TextEntryActive); Assert.AreEqual(1, renders);
                Assert.AreEqual("저장 완료를 기다리는 초안", game.Interface.ReviewEditor.text);
                var bytes = File.ReadAllBytes(Path.Combine(directory, "save.json"));
                Click("review-note-save"); yield return Wait(game.FlushSaves(), "note-after-autosave");
                Assert.AreEqual("저장 완료를 기다리는 초안", (string)game.ReviewStore.Load()["text"]);
                CollectionAssert.AreEqual(bytes, File.ReadAllBytes(Path.Combine(directory, "save.json")));
            }
            finally { release.TrySetResult(true); if (game != null) game.Store.Injection = null; }
        }

        [UnityTest] public IEnumerator ImeCandidateEscapeAndEnterStayInEditorWithoutEditingDraft()
        {
            OpenNotes(); Click("review-note-edit"); yield return null;
            var editor = (ReviewNoteInputField)game.Interface.ReviewEditor; editor.text = "기존 초안"; editor.caretPosition = editor.text.Length;
            var composition = IMECompositionEvent.Create(keyboard.deviceId, "ㅎ", 0); InputSystem.QueueEvent(ref composition); yield return null;
            Assert.IsTrue(game.Watch.ImeCompositionActive);
            editor.ProcessReviewKey(Event.KeyboardEvent("escape")); editor.ProcessReviewKey(Event.KeyboardEvent("return")); yield return KeyPress(Key.Escape);
            Assert.IsTrue(game.Watch.TextEntryActive); Assert.AreEqual("기존 초안", game.ReviewNoteText); Assert.AreEqual("reviewNotes", game.Surface);
            composition = IMECompositionEvent.Create(keyboard.deviceId, "", 0); InputSystem.QueueEvent(ref composition); yield return null;
            Assert.IsTrue(game.Watch.ImeCompositionActive); var caret = editor.caretPosition;
            editor.ProcessReviewKey(Event.KeyboardEvent("backspace")); editor.ProcessReviewKey(Event.KeyboardEvent("left"));
            Assert.AreEqual("기존 초안", game.ReviewNoteText); Assert.AreEqual(caret, editor.caretPosition);
            yield return null; yield return null;
            yield return KeyPress(Key.Tab); Assert.IsFalse(game.Watch.TextEntryActive); Assert.AreEqual("기존 초안", game.ReviewNoteText);
        }

        [UnityTest] public IEnumerator NativeTabCharacterAndPostExitEventsCannotModifyPastedNoteOrSavedText()
        {
            ObserveBrief(); yield return Wait(game.FlushSaves()); OpenNotes();
            const string draft = "수문 기록과 인수 기록의 작성 시각을 다시 대조한다. <b>아직 가설</b>";
            var previousClipboard = GUIUtility.systemCopyBuffer;
            try
            {
                GUIUtility.systemCopyBuffer = draft;
                foreach (var keyCode in new[] { KeyCode.None, KeyCode.Tab })
                {
                    Click("review-note-edit"); yield return null;
                    var editor = (ReviewNoteInputField)game.Interface.ReviewEditor;
                    editor.selectionAnchorPosition = 0; editor.selectionFocusPosition = editor.text.Length;
                    var modifier = Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer ? EventModifiers.Command : EventModifiers.Control;
                    editor.ProcessReviewKey(new Event { type = EventType.KeyDown, keyCode = KeyCode.V, modifiers = modifier });
                    Assert.AreEqual(draft, editor.text, "The paste path must preserve Korean and literal markup.");
                    editor.ProcessReviewKey(new Event { type = EventType.KeyDown, keyCode = keyCode, character = keyCode == KeyCode.None ? '\t' : '\0' });
                    // A native key-down can be followed by a separate text event in the same GUI event batch.
                    editor.ProcessReviewKey(new Event { type = EventType.KeyDown, keyCode = KeyCode.None, character = '\t' });
                    editor.ProcessReviewKey(new Event { type = EventType.KeyDown, keyCode = KeyCode.None, character = 'x' });
                    Assert.IsFalse(game.Watch.TextEntryActive);
                    Assert.AreEqual(draft, editor.text); Assert.AreEqual(draft, game.ReviewNoteText);
                    yield return null;
                    Click("review-note-save"); yield return Wait(game.FlushSaves(), "native-tab-" + keyCode);
                    Assert.AreEqual(draft, (string)game.ReviewStore.Load()["text"]);
                }
            }
            finally { GUIUtility.systemCopyBuffer = previousClipboard; }
        }

        [UnityTest] public IEnumerator FailedSaveKeepsDraftAndInFlightLinkChangesRemainUnsaved()
        {
            ObserveBrief(); ObserveTransfer(); yield return Wait(game.FlushSaves()); OpenNotes();
            var notes = game.ReviewStore;
            var entered = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously); var release = ReleaseGate();
            try
            {
                game.Interface.ReviewEditor.text = "유지할 초안";
                notes.Injection = (stage, token) => stage == SaveStage.BeforeRename ? Task.FromException(new IOException("injected note failure")) : Task.CompletedTask;
                var failed = Track(game.SaveReviewNoteAsync()); yield return Wait(failed, "injected-note-failure"); Assert.IsFalse(failed.Result);
                Assert.AreEqual("유지할 초안", game.Interface.ReviewEditor.text); StringAssert.Contains("초안을 유지", VisibleText());
                notes.Injection = (stage, token) => { if (stage != SaveStage.BeforeRename) return Task.CompletedTask; entered.TrySetResult(true); return release.Task; };
                var save = Track(game.SaveReviewNoteAsync()); yield return WaitForGate(entered.Task, save, "note-before-rename");
                Click("review-source-record:rec-handover-brief"); release.TrySetResult(true); yield return Wait(save, "note-released");
                Assert.IsTrue(save.Result); StringAssert.Contains("변경한 내용은 아직", VisibleText());
                Assert.AreEqual(0, ((JArray)notes.Load()["sourceIds"]).Count);
                notes.Injection = null; var retry = Track(game.SaveReviewNoteAsync()); yield return Wait(retry, "note-retry");
                Assert.AreEqual(1, ((JArray)notes.Load()["sourceIds"]).Count);
            }
            finally { release.TrySetResult(true); notes.Injection = null; }
        }

        [UnityTest] public IEnumerator UnknownRestoredLinksCannotLeakOrExhaustVisibleSourceSelection()
        {
            ObserveBrief(); yield return Wait(game.FlushSaves());
            var save = Track(game.ReviewStore.SaveAsync("저장한 메모", Enumerable.Range(0, 32).Select(index => "unobserved-secret-" + index))); yield return Wait(save);
            yield return Restart(); OpenNotes();
            StringAssert.DoesNotContain("unobserved-secret", VisibleText());
            Click("review-source-record:rec-handover-brief"); var persist = Track(game.SaveReviewNoteAsync()); yield return Wait(persist);
            CollectionAssert.AreEqual(new[] { "record:rec-handover-brief" }, game.ReviewStore.Load()["sourceIds"].Values<string>().ToArray());
        }

        [UnityTest] public IEnumerator C1NotesRemainOptionalAndReadableAtLargeTextWithoutRevealingUnobservedSignature()
        {
            yield return Wait(game.FlushSaves()); UnityEngine.Object.Destroy(host); yield return null;
            File.Copy(Path.Combine(Application.dataPath, "_Project/Tests/Fixtures/C1PatrolCompletedV2.json"), Path.Combine(directory, "save.json"));
            Create(); yield return null; game.StartGame(); yield return null;
            Assert.IsTrue(game.PatrolComplete); game.OpenOverlay("settings"); for (int i = 0; i < 5; i++) Click("text-scale"); game.Back();
            OpenNotes(); Assert.AreEqual(1.5f, game.Interface.TextScale); Assert.IsFalse(game.ObservedReviewSources().Any(source => source.Id.StartsWith("signature:")));
            Assert.IsTrue(game.Interface.Focus("review-note-back")); Canvas.ForceUpdateCanvases();
            var button = host.GetComponentsInChildren<Button>().Single(item => item.name == "review-note-back");
            var scroll = host.GetComponentsInChildren<ScrollRect>().Single(item => item.viewport.name == "Viewport");
            var corners = new Vector3[4]; var viewport = new Vector3[4]; ((RectTransform)button.transform).GetWorldCorners(corners); scroll.viewport.GetWorldCorners(viewport);
            Assert.GreaterOrEqual(corners[0].y, viewport[0].y - 1); Assert.LessOrEqual(corners[2].y, viewport[2].y + 1);
            Click("review-note-back"); game.Back(); Click("continue-c1-signature");
            game.OpenOverlay("evidence"); Click("open-review-notes"); Assert.AreEqual("reviewNotes", game.Surface);
            Assert.IsFalse(game.ObservedReviewSources().Any(source => source.Id.StartsWith("signature:")));
            var id = game.Definition.Signature.LeftClue; Assert.IsTrue(game.SubmitImmediate(new PuzzleCommand("ObserveSignature", id)).IsValid);
            Assert.AreEqual(1, game.ObservedReviewSources().Count(source => source.Id.StartsWith("signature:")));
        }
    }
}
#endif
