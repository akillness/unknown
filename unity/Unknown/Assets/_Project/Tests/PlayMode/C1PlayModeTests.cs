#if TIDE_TEST_FRAMEWORK
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Tide.App;
using Tide.Save;
using Tide.Sim;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.TestTools;

namespace Tide.Tests
{
    public sealed class C1PlayModeTests
    {
        string directory;
        GameObject host;
        T0GameSession game;
        Keyboard keyboard;
        Gamepad pad;
        InputSettings.BackgroundBehavior previousBackground;
#if UNITY_EDITOR
        InputSettings.EditorInputBehaviorInPlayMode previousEditorInput;
#endif
        [UnitySetUp] public IEnumerator Setup()
        {
            directory=Path.Combine(Path.GetTempPath(),"c1-play-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(directory);
            File.Copy(Path.Combine(Application.dataPath,"_Project/Tests/Fixtures/T0CompletedV1.json"),Path.Combine(directory,"save.json"));
            previousBackground=InputSystem.settings.backgroundBehavior;InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;
#if UNITY_EDITOR
            previousEditorInput=InputSystem.settings.editorInputBehaviorInPlayMode;InputSystem.settings.editorInputBehaviorInPlayMode=InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
#endif
            keyboard=InputSystem.AddDevice<Keyboard>();pad=InputSystem.AddDevice<Gamepad>();Create();yield return null;game.StartGame();yield return null;
        }
        void Create(){host=new GameObject("C1 session test");game=host.AddComponent<T0GameSession>();game.Initialize(Resources.Load<T0RuntimeConfig>("T0Runtime"),directory);}
        [UnityTearDown] public IEnumerator Teardown()
        {
            yield return Wait(game.FlushSaves());UnityEngine.Object.Destroy(host);yield return null;
            InputSystem.RemoveDevice(keyboard);InputSystem.RemoveDevice(pad);InputSystem.settings.backgroundBehavior=previousBackground;
#if UNITY_EDITOR
            InputSystem.settings.editorInputBehaviorInPlayMode=previousEditorInput;
#endif
            Directory.Delete(directory,true);
        }
        IEnumerator Wait(Task task){while(!task.IsCompleted)yield return null;if(task.IsFaulted)throw task.Exception;}
        void Click(string id){Assert.IsTrue(game.Interface.Focus(id),id);game.Interface.BeginActivation();game.Interface.EndActivation();}
        void Ready()
        {
            Click("continue-c1");
            foreach(var id in game.Definition.Patrol.Observations){Click("c1-open-"+id);Click("c1-observe-"+id);Click("c1-close-observation");}
            Click("c1-bypass");Click("c1-condition");
        }
        PuzzleCommand Confirm()=>new PuzzleCommand("ConfirmPatrol",game.Definition.Patrol.ConditionId,"false","true");
        [UnityTest] public IEnumerator KeyboardAndPadUseTwoStepConfirmationAndRestartAtTerminalSlice()
        {
            Ready();yield return Wait(game.FlushSaves());Assert.IsTrue(game.Interface.Focus("c1-confirm"));
            InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.Enter));yield return null;
            InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return null;
            Assert.AreEqual("preview",game.Surface);Assert.IsFalse(game.PatrolComplete);Assert.IsTrue(game.Interface.Focus("preview-next"));
            InputSystem.QueueStateEvent(pad,new GamepadState().WithButton(GamepadButton.South));yield return null;
            InputSystem.QueueStateEvent(pad,new GamepadState());yield return null;
            Assert.AreEqual("confirm",game.Surface);Click("confirm-submit");while(game.SavePending)yield return null;
            Assert.IsTrue(game.PatrolComplete);Assert.AreEqual(1,game.SuccessfulReceipts);Assert.IsTrue(game.Interface.ActionIds.Contains("c1-review"));
            var doc=game.Store.Load().Document;Assert.AreEqual("C1",(string)doc["stageId"]);Assert.AreEqual("c1-b1",(string)doc["beatId"]);
            var final=game.Journal.State.StateHash;yield return Wait(game.FlushSaves());UnityEngine.Object.Destroy(host);yield return null;Create();yield return null;game.StartGame();yield return null;
            Assert.AreEqual(final,game.Journal.State.StateHash);Assert.IsTrue(game.PatrolComplete);Assert.IsTrue(game.Interface.ActionIds.Contains("c1-review"));
        }
        [UnityTest] public IEnumerator FailedSaveHasNoChapterEffectsAndRetryCommitsOnce()
        {
            Ready();yield return Wait(game.FlushSaves());var before=game.Journal.State.StateHash;
            int renames=0;game.Store.Injection=(stage,token)=>stage==SaveStage.BeforeRename&&++renames==2?Task.FromException(new IOException("c1 injected candidate save failure")):Task.CompletedTask;
            var failed=game.CommitAsync(Confirm());yield return Wait(failed);Assert.IsFalse(failed.Result);Assert.AreEqual(before,game.Journal.State.StateHash);
            Assert.AreEqual(2,renames);Assert.AreEqual(before,JournalSave.Decode(game.Store.Load().Document,game.Simulation).State.StateHash);
            Assert.AreEqual(0,game.SuccessfulReceipts);Assert.IsFalse(game.PatrolComplete);Assert.IsFalse(game.Journal.State.Has("c1:gateAccess"));
            Assert.IsFalse(game.Journal.State.Has("c1:journal:"+game.Definition.Patrol.ConditionId));Assert.IsFalse(game.Journal.State.Has("checkpoint:cp-c1-b1"));
            Assert.IsNull(game.Journal.State.Get("c1:committed:lighting"));Assert.IsNull(game.Journal.State.Get("c1:committed:reader"));
            game.Store.Injection=null;Click("retry-save");while(game.SavePending)yield return null;
            Assert.IsTrue(game.PatrolComplete);Assert.AreEqual(1,game.SuccessfulReceipts);
        }
        [UnityTest] public IEnumerator PublicImmediateSubmissionCannotBypassAcceptedSaveBoundary()
        {
            Ready();yield return Wait(game.FlushSaves());var before=game.Journal.State.StateHash;var head=game.Journal.HeadSeq;
            game.Store.Injection=(stage,token)=>Task.FromException(new IOException("c1 immediate boundary failure"));
            var rejected=game.SubmitImmediate(Confirm());Assert.IsFalse(rejected.IsValid);Assert.AreEqual(before,game.Journal.State.StateHash);
            Assert.AreEqual(head,game.Journal.HeadSeq);Assert.AreEqual(0,game.SuccessfulReceipts);Assert.IsFalse(game.PatrolComplete);
            game.Store.Injection=null;yield return null;
        }
        [UnityTest] public IEnumerator PendingUndoCancelsAllChapterEffectsAndSuppressesReceipt()
        {
            Ready();yield return Wait(game.FlushSaves());var release=new TaskCompletionSource<bool>();
            int renames=0;game.Store.Injection=(stage,token)=>stage==SaveStage.BeforeRename&&++renames==2?release.Task:Task.CompletedTask;
            var pending=game.CommitAsync(Confirm());while(renames<2)yield return null;Assert.IsTrue(game.SavePending);game.Undo();
            game.Store.Injection=null;release.SetResult(true);yield return Wait(pending);yield return Wait(game.FlushSaves());
            Assert.IsFalse(pending.Result);Assert.IsFalse(game.SavePending);Assert.IsFalse(game.PatrolComplete);Assert.AreEqual(0,game.SuccessfulReceipts);
            Assert.IsFalse(game.Journal.State.Has("c1:gateAccess"));Assert.IsFalse(game.Journal.State.Has("checkpoint:cp-c1-b1"));
        }
        [UnityTest] public IEnumerator HoldModeRequiresFullGestureForChapterConfirmation()
        {
            Ready();game.SetConfirmMode("hold");Click("c1-confirm");Assert.AreEqual("confirm",game.Surface);Assert.IsTrue(game.Interface.Focus("confirm-submit"));
            game.Interface.BeginActivation();yield return new WaitForSecondsRealtime(.1f);game.Interface.EndActivation();Assert.IsFalse(game.PatrolComplete);Assert.IsFalse(game.SavePending);
            game.Interface.BeginActivation();yield return new WaitForSecondsRealtime(.5f);game.Interface.EndActivation();while(game.SavePending)yield return null;
            Assert.IsTrue(game.PatrolComplete);Assert.AreEqual(1,game.SuccessfulReceipts);
        }
    }
}
#endif
