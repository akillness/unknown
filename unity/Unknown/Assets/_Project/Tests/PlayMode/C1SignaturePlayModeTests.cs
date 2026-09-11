#if TIDE_TEST_FRAMEWORK
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Newtonsoft.Json.Linq;
using Tide.App;
using Tide.Save;
using Tide.Sim;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
namespace Tide.Tests
{
    public sealed class C1SignaturePlayModeTests
    {
        string directory;GameObject host;T0GameSession game;
        [UnitySetUp] public IEnumerator Setup()
        {
            directory=Path.Combine(Path.GetTempPath(),"c1-signature-play-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(directory);
            File.Copy(Path.Combine(Application.dataPath,"_Project/Tests/Fixtures/C1PatrolCompletedV2.json"),Path.Combine(directory,"save.json"));Create();yield return null;game.StartGame();yield return null;
        }
        void Create(){host=new GameObject("Signature session test");game=host.AddComponent<T0GameSession>();game.Initialize(Resources.Load<T0RuntimeConfig>("T0Runtime"),directory);}
        [UnityTearDown] public IEnumerator TearDown(){yield return Wait(game.FlushSaves());UnityEngine.Object.Destroy(host);yield return null;Directory.Delete(directory,true);}
        IEnumerator Wait(Task task){while(!task.IsCompleted)yield return null;if(task.IsFaulted)throw task.Exception;}
        void Click(string id){Assert.IsTrue(game.Interface.Focus(id),id);game.Interface.BeginActivation();game.Interface.EndActivation();}
        void Do(string id,string subject=null,string value=null){var v=game.SubmitImmediate(new PuzzleCommand(id,subject,value));Assert.IsTrue(v.IsValid,v.DataDiagnostic);}
        void Enter(){Click("continue-c1-signature");}
        void Ready()
        {
            Enter();var def=game.Definition.Signature;foreach(var id in def.Observations)Do("ObserveSignature",id);Do("SetSignatureHumidity","low");Do("TrialSignature");Do("SeparateSignature");foreach(var id in def.Copies)Do("CopySignature",id);Do("MarkSignature",def.RegionId);Do("CompareSignature",def.ComparisonId);Do("SelectSignatureProof",def.LeftClue,def.RightClue);
        }
        PuzzleCommand Confirm()=>new PuzzleCommand("ConfirmSignature",game.Definition.Signature.ComparisonId,game.Definition.Signature.RegionId);
        Text[] Texts()=>host.GetComponentsInChildren<Text>();
        bool HasMask()=>host.GetComponentsInChildren<Transform>().Any(t=>t.name=="Signature lower obscuration");
        [UnityTest] public IEnumerator DefaultGuidanceDoesNotRevealHumidityOrInventPageValue()
        {
            Assert.IsFalse(game.SignatureActive);Assert.IsTrue(game.PatrolComplete);Click("settings");for(int i=0;i<5;i++)Click("text-scale");Click("back");host.GetComponentsInChildren<ScrollRect>().Single(s=>s.viewport.name=="Viewport").verticalNormalizedPosition=0;Enter();yield return Wait(game.FlushSaves());yield return null;
            Assert.IsTrue(game.SignatureActive);Assert.IsFalse(game.SignatureComplete);Assert.That(host.GetComponentsInChildren<ScrollRect>().Single(s=>s.viewport.name=="Viewport").verticalNormalizedPosition,Is.EqualTo(1).Within(.00001f));var text=string.Join("\n",Texts().Select(t=>t.text));StringAssert.DoesNotContain("가장자리가 풀리고",text);StringAssert.DoesNotContain("1단이",text);StringAssert.DoesNotContain("같은 출처",text);Assert.IsFalse(Texts().Any(t=>t.name=="Single page number"));Assert.IsTrue(HasMask());
            Click("c1-signature-open-"+game.Definition.Signature.LeftClue);yield return null;Assert.That(host.GetComponentsInChildren<ScrollRect>().Single(s=>s.viewport.name=="Viewport").verticalNormalizedPosition,Is.EqualTo(1).Within(.00001f));Do("ObserveSignature",game.Definition.Signature.LeftClue);yield return null;Assert.AreEqual("번호 표기",Texts().Single(t=>t.name=="Single page number").text);
        }
        [UnityTest] public IEnumerator MaskPersistsAcrossSeparationCopiesComparisonAndReducedMotionLargeTextRender()
        {
            Enter();Assert.IsTrue(HasMask());Do("ObserveSignature",game.Definition.Signature.LeftClue);Do("SetSignatureHumidity","low");Do("TrialSignature");Do("SeparateSignature");Assert.IsTrue(HasMask());Assert.IsFalse(host.GetComponentsInChildren<Transform>().Any(t=>t.name=="Salt adhesion edge"));foreach(var id in game.Definition.Signature.Copies)Do("CopySignature",id);Do("ObserveSignature",game.Definition.Signature.RightClue);Do("MarkSignature",game.Definition.Signature.RegionId);Do("CompareSignature",game.Definition.Signature.ComparisonId);
            Click("settings");for(int i=0;i<5;i++)Click("text-scale");Click("reduced-motion");Click("back");Assert.AreEqual(1.5f,game.Interface.TextScale);
            yield return Wait(game.FlushSaves());var hash=game.Journal.State.StateHash;var count=game.Journal.HeadSeq;var bytes=File.ReadAllBytes(Path.Combine(directory,"save.json"));var actions=game.Interface.ActionIds.ToArray();var focus=game.Interface.CurrentFocusId;
            game.Render();yield return null;Assert.IsTrue(HasMask());var mask=host.GetComponentsInChildren<RectTransform>().Single(r=>r.name=="Signature lower obscuration");var viewport=host.GetComponentsInChildren<RectTransform>().Single(r=>r.name=="Navigation Viewport");var mc=new Vector3[4];var vc=new Vector3[4];mask.GetWorldCorners(mc);viewport.GetWorldCorners(vc);Assert.GreaterOrEqual(mc[0].y,vc[0].y);Assert.LessOrEqual(mc[2].y,vc[2].y);foreach(var label in host.GetComponentsInChildren<RectTransform>().Where(r=>new[]{"Copy status 1","Copy status 2","Unresolved region label","Signature sheet state","Band comparison"}.Contains(r.name))){var corners=new Vector3[4];label.GetWorldCorners(corners);Assert.GreaterOrEqual(corners[0].y,vc[0].y-.5f,label.name);Assert.LessOrEqual(corners[2].y,vc[2].y+.5f,label.name);}Assert.AreEqual(hash,game.Journal.State.StateHash);Assert.AreEqual(count,game.Journal.HeadSeq);CollectionAssert.AreEqual(bytes,File.ReadAllBytes(Path.Combine(directory,"save.json")));CollectionAssert.AreEqual(actions,game.Interface.ActionIds);Assert.AreEqual(focus,game.Interface.CurrentFocusId);Assert.AreEqual(0,game.SuccessfulReceipts);
        }
        [UnityTest] public IEnumerator ImmediateSubmissionCannotPublishSignatureCompletion()
        {
            Ready();yield return Wait(game.FlushSaves());var hash=game.Journal.State.StateHash;Assert.IsFalse(game.SubmitImmediate(Confirm()).IsValid);Assert.AreEqual(hash,game.Journal.State.StateHash);Assert.IsFalse(game.SignatureComplete);Assert.IsFalse(game.Journal.State.Has("checkpoint:cp-c1-b2"));
        }
        [UnityTest] public IEnumerator FailedSaveRetryHasOneReceiptAndRestartsAtSignatureEndpoint()
        {
            Ready();yield return Wait(game.FlushSaves());var hash=game.Journal.State.StateHash;var original=File.ReadAllBytes(Path.Combine(directory,"save.json"));int renames=0;game.Store.Injection=(stage,token)=>stage==SaveStage.BeforeRename&&++renames==2?Task.FromException(new IOException("signature candidate-save failure")):Task.CompletedTask;var failed=game.CommitAsync(Confirm());yield return Wait(failed);Assert.IsFalse(failed.Result);Assert.AreEqual(hash,game.Journal.State.StateHash);Assert.AreEqual(0,game.SuccessfulReceipts);Assert.AreEqual(2,renames);Assert.IsTrue(File.Exists(Path.Combine(directory,"checkpoint.pre-commit.json")));CollectionAssert.AreEqual(original,File.ReadAllBytes(Path.Combine(directory,"save.json")));Assert.AreEqual(hash,JournalSave.Decode(game.Store.Load().Document,game.Simulation).State.StateHash);
            game.Store.Injection=null;var success=game.CommitAsync(Confirm());yield return Wait(success);Assert.IsTrue(success.Result);Assert.IsTrue(game.SignatureComplete);Assert.AreEqual(1,game.SuccessfulReceipts);Assert.IsTrue(HasMask());var second=game.CommitAsync(Confirm());yield return Wait(second);Assert.IsFalse(second.Result);Assert.AreEqual(1,game.SuccessfulReceipts);
            var saved=game.Store.Load().Document;Assert.AreEqual("c1-b2",(string)saved["beatId"]);CollectionAssert.Contains(saved["progress"]["checkpoints"].Values<string>().ToArray(),"cp-c1-b2");var complete=game.Journal.State.StateHash;
            UnityEngine.Object.Destroy(host);yield return null;Create();yield return null;game.StartGame();yield return null;Assert.IsTrue(game.SignatureComplete);Assert.AreEqual(complete,game.Journal.State.StateHash);Assert.IsTrue(HasMask());Assert.IsFalse(game.Interface.ActionIds.Any(id=>id.Contains("c1-b3")));var resumedText=string.Join("\n",Texts().Select(t=>t.text));StringAssert.Contains("저장된 대조 기록을 복원했습니다.",resumedText);StringAssert.DoesNotContain("각서와 목록을 열람하고 상시 슬롯을 준비하세요.",resumedText);
        }
        [UnityTest] public IEnumerator PendingUndoCancelsAllSixSignatureEffectsAndReceipt()
        {
            Ready();yield return Wait(game.FlushSaves());var release=new TaskCompletionSource<bool>();int renames=0;game.Store.Injection=(stage,token)=>stage==SaveStage.BeforeRename&&++renames==2?release.Task:Task.CompletedTask;
            var pending=game.CommitAsync(Confirm());while(renames<2)yield return null;game.Undo();game.Store.Injection=null;release.SetResult(true);yield return Wait(pending);yield return Wait(game.FlushSaves());Assert.IsFalse(pending.Result);Assert.IsFalse(game.SignatureComplete);Assert.IsFalse(game.Journal.State.Has("checkpoint:cp-c1-b2"));Assert.IsFalse(C1SignatureDefinition.Has(game.Journal.State,"copiesFiled"));Assert.AreEqual(0,game.SuccessfulReceipts);Assert.IsTrue(HasMask());
        }
        [UnityTest] public IEnumerator TwoStepConfirmationDoesNotCommitEarly()
        {
            Ready();yield return Wait(game.FlushSaves());game.SetConfirmMode("two-step");Click("c1-signature-confirm");Assert.AreEqual("preview",game.Surface);Assert.IsFalse(game.SignatureComplete);Click("preview-next");Assert.AreEqual("confirm",game.Surface);Assert.IsFalse(game.SignatureComplete);Click("confirm-submit");while(game.SavePending)yield return null;Assert.IsTrue(game.SignatureComplete);Assert.AreEqual(1,game.SuccessfulReceipts);
        }
        [UnityTest] public IEnumerator NonfocusedSourceOpensOnOneMouseClick(){yield return MouseClickSource(false);}
        [UnityTest] public IEnumerator NonfocusedSourceOpensOnOneFastMouseClick(){yield return MouseClickSource(true);}
        [UnityTest] public IEnumerator PointerSelectedButtonBecomesKeyboardTargetWithoutRerenderOrScroll()
        {
            Enter();yield return null;var hash=game.Journal.State.StateHash;
            var target=host.GetComponentsInChildren<Button>().Single(b=>b.name=="c1-signature-open-"+game.Definition.Signature.RightClue);
            var scroll=host.GetComponentsInChildren<ScrollRect>().Single(s=>s.viewport.name=="Viewport");var position=scroll.verticalNormalizedPosition;
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(target.gameObject);yield return null;
            Assert.AreEqual(position,scroll.verticalNormalizedPosition);Assert.AreEqual(hash,game.Journal.State.StateHash);Assert.IsTrue(target.gameObject.activeInHierarchy);
            game.Interface.BeginActivation();game.Interface.EndActivation();Assert.AreEqual("판 #0",game.Interface.CurrentTitle);
        }
        [UnityTest] public IEnumerator ExecutedTrialOutcomeStaysVisibleWhileLowerControlsAreFocused()
        {
            Click("settings");for(int i=0;i<5;i++)Click("text-scale");Click("back");Enter();Do("ObserveSignature",game.Definition.Signature.LeftClue);
            Do("SetSignatureHumidity","medium");Do("TrialSignature");Assert.IsTrue(game.Interface.Focus("c1-signature-reset"));yield return null;
            var card=Texts().Single(t=>t.name=="CaseThread");StringAssert.Contains("습도 2단",card.text);StringAssert.Contains("잉크 번짐 위험",card.text);
            Assert.LessOrEqual(card.preferredHeight,((RectTransform)card.transform).rect.height+1,"Trial feedback must fit its persistent card at150%");
            Assert.IsFalse(game.SignatureComplete);Assert.IsFalse(game.Simulation.Validate(game.Journal.State,new PuzzleCommand("SeparateSignature")).IsValid);
        }
        IEnumerator MouseClickSource(bool sameFrame)
        {
            var background=InputSystem.settings.backgroundBehavior;InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;
#if UNITY_EDITOR
            var editor=InputSystem.settings.editorInputBehaviorInPlayMode;InputSystem.settings.editorInputBehaviorInPlayMode=InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
#endif
            var mouse=InputSystem.AddDevice<Mouse>();mouse.MakeCurrent();
            try
            {
                Click("settings");for(int i=0;i<5;i++)Click("text-scale");Click("back");Enter();yield return null;
                Assert.IsTrue(game.Interface.Focus("c1-signature-open-"+game.Definition.Signature.LeftClue));yield return null;
                var button=host.GetComponentsInChildren<Button>().Single(b=>b.name=="c1-signature-open-"+game.Definition.Signature.RightClue);var rect=(RectTransform)button.transform;
                var point=(Vector2)rect.TransformPoint(rect.rect.center);var state=game.Journal.State.StateHash;
                InputSystem.QueueStateEvent(mouse,new MouseState{position=point});yield return null;
                InputSystem.QueueStateEvent(mouse,new MouseState{position=point}.WithButton(MouseButton.Left));if(!sameFrame)yield return null;
                InputSystem.QueueStateEvent(mouse,new MouseState{position=point});yield return null;yield return null;
                Assert.AreEqual("판 #0",game.Interface.CurrentTitle);Assert.AreEqual(state,game.Journal.State.StateHash,"Opening a clue must not observe it automatically");
            }
            finally
            {
                InputSystem.RemoveDevice(mouse);InputSystem.settings.backgroundBehavior=background;
#if UNITY_EDITOR
                InputSystem.settings.editorInputBehaviorInPlayMode=editor;
#endif
            }
        }
    }
}
#endif
