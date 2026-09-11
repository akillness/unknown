#if TIDE_TEST_FRAMEWORK
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Tide.App;
using Tide.Data;
using Tide.Presentation;
using Tide.Save;
using Tide.Sim;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.TestTools;
using UnityEngine.UI;
namespace Tide.Tests {
 public sealed class M5DirectionPlayModeTests {
  string directory;GameObject host;T0GameSession game;Keyboard keyboard;Mouse mouse;M5DirectionProfile profile;Texture2D openingImage;
  InputSettings.BackgroundBehavior oldBackground;
#if UNITY_EDITOR
  InputSettings.EditorInputBehaviorInPlayMode oldEditor;
#endif
  [UnitySetUp] public IEnumerator Setup(){
#if UNITY_EDITOR
   oldEditor=InputSystem.settings.editorInputBehaviorInPlayMode;InputSystem.settings.editorInputBehaviorInPlayMode=InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
#endif
   oldBackground=InputSystem.settings.backgroundBehavior;InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;
   keyboard=InputSystem.AddDevice<Keyboard>();mouse=InputSystem.AddDevice<Mouse>();directory=Path.Combine(Path.GetTempPath(),"m5-direction-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(directory);
   profile=Resources.Load<M5DirectionProfile>("M5Direction");Assert.IsNotNull(profile);openingImage=profile.openingImage;Create();yield return null;
  }
  void Create(){host=new GameObject("M5 direction test");game=host.AddComponent<T0GameSession>();game.Initialize(Resources.Load<T0RuntimeConfig>("T0Runtime"),directory);}
  IEnumerator Wait(Task task){while(!task.IsCompleted)yield return null;if(task.IsFaulted)throw task.Exception;}
  [UnityTearDown] public IEnumerator Teardown(){profile.openingImage=openingImage;yield return Wait(game.FlushSaves());UnityEngine.Object.Destroy(host);InputSystem.RemoveDevice(keyboard);InputSystem.RemoveDevice(mouse);InputSystem.settings.backgroundBehavior=oldBackground;
#if UNITY_EDITOR
   InputSystem.settings.editorInputBehaviorInPlayMode=oldEditor;
#endif
   yield return null;if(Directory.Exists(directory))Directory.Delete(directory,true);
  }
  void Click(string id){Assert.IsTrue(game.Interface.Focus(id),id);game.Interface.BeginActivation();game.Interface.EndActivation();}
  string Text()=>string.Join("\n",host.GetComponentsInChildren<Text>().Select(t=>t.text));
  void AssertNoSave(string hash,long head){Assert.AreEqual(hash,game.Journal.State.StateHash);Assert.AreEqual(head,game.Journal.HeadSeq);Assert.IsFalse(File.Exists(Path.Combine(directory,"save.json")));}
  IEnumerator RestartWithSignatureSave(){yield return Wait(game.FlushSaves());UnityEngine.Object.Destroy(host);yield return null;File.Copy(Path.Combine(Application.dataPath,"_Project/Tests/Fixtures/C1PatrolCompletedV2.json"),Path.Combine(directory,"save.json"),true);Create();yield return null;}
  void Do(string id,string subject=null,string value=null){var verdict=game.SubmitImmediate(new PuzzleCommand(id,subject,value));Assert.IsTrue(verdict.IsValid,verdict.DataDiagnostic);}
  void Ready(){Click("continue-c1-signature");var def=game.Definition.Signature;foreach(var id in def.Observations)Do("ObserveSignature",id);Do("SetSignatureHumidity","low");Do("TrialSignature");Do("SeparateSignature");foreach(var id in def.Copies)Do("CopySignature",id);Do("MarkSignature",def.RegionId);Do("CompareSignature",def.ComparisonId);Do("SelectSignatureProof",def.LeftClue,def.RightClue);}
  [UnityTest] public IEnumerator FreshIntroUsesObservedCutAndTimeoutWithoutWrites(){
   string hash=game.Journal.State.StateHash;long head=game.Journal.HeadSeq;Click("start");Assert.IsTrue(game.OpeningActive);Assert.AreEqual("intro-skip",game.Interface.CurrentFocusId);Assert.AreEqual(3.125f,profile.firstShotSeconds);Assert.AreEqual(2.875f,profile.secondShotSeconds);
   yield return new WaitForSecondsRealtime(2.9f);Assert.IsTrue(game.OpeningActive);Assert.AreEqual(0,game.OpeningCaptionIndex);
   while(game.OpeningElapsed<3.14f)yield return null;Assert.AreEqual(1,game.OpeningCaptionIndex);AssertNoSave(hash,head);
   float deadline=Time.realtimeSinceStartup+4;while(game.OpeningActive&&Time.realtimeSinceStartup<deadline)yield return null;
   Assert.IsFalse(game.OpeningActive);Assert.IsTrue(game.Interface.ActionIds.Contains("handover"));AssertNoSave(hash,head);
  }
  IEnumerator PointerClick(string id){
   yield return null;Canvas.ForceUpdateCanvases();var button=host.GetComponentsInChildren<Button>().Single(b=>b.name==id);var rect=(RectTransform)button.transform;var point=RectTransformUtility.WorldToScreenPoint(null,rect.TransformPoint(rect.rect.center));InputSystem.QueueStateEvent(mouse,new MouseState{position=point});yield return null;InputSystem.QueueStateEvent(mouse,new MouseState{position=point,buttons=1});yield return null;InputSystem.QueueStateEvent(mouse,new MouseState{position=point});yield return null;
  }
  [UnityTest] public IEnumerator PointerSettingsFromReduced150IntroOpensAndReturns(){
   Click("settings");for(int i=0;i<5;i++)Click("text-scale");Click("reduced-motion");Click("back");Click("start");Assert.IsTrue(game.OpeningActive);Assert.AreEqual("intro-skip",game.Interface.CurrentFocusId);Debug.Log("M5_POINTER_SCREEN "+Screen.width+"x"+Screen.height);yield return PointerClick("settings");Assert.AreEqual("settings",game.Surface);Assert.IsTrue(game.OpeningActive);yield return PointerClick("back");Assert.IsTrue(game.OpeningActive);Assert.IsTrue(game.Interface.ActionIds.Contains("intro-skip"));yield return PointerClick("intro-skip");Assert.IsFalse(game.OpeningActive);Assert.IsTrue(game.Interface.ActionIds.Contains("handover"));Assert.AreEqual(0,game.Journal.HeadSeq);
  }
  [UnityTest] public IEnumerator SkipFirstFrameAndEscapeConsumeInput(){
   var hash=game.Journal.State.StateHash;Click("start");Assert.IsTrue(game.OpeningActive);Click("intro-skip");Assert.IsFalse(game.OpeningActive);AssertNoSave(hash,0);
   Click("settings");Click("intro-replay");Assert.IsTrue(game.OpeningActive);InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.Escape));yield return null;Assert.IsFalse(game.OpeningActive);Assert.AreEqual("settings",game.Surface);InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return null;
  }
  [UnityTest] public IEnumerator HeldSkipCannotActivateGameplayUntilFreshPress(){
   Click("start");Assert.IsTrue(game.Interface.Focus("intro-skip"));InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.Enter));yield return null;InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return null;Assert.IsFalse(game.OpeningActive);
   Assert.IsTrue(game.Interface.Focus("handover"));InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.Enter));yield return null;
   InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.Enter));yield return null;Assert.AreEqual(0,game.Journal.HeadSeq);
   InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return null;Assert.IsTrue(game.Interface.ActionIds.Any(id=>id.StartsWith("line-hb-")));Assert.AreEqual(0,game.Journal.HeadSeq);
  }
  [UnityTest] public IEnumerator HeldInputAcrossCutAndTimeoutDoesNotLeakOnRelease(){
   Click("start");while(game.OpeningElapsed<2.9f)yield return null;InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.Enter));yield return null;while(game.OpeningElapsed<3.2f)yield return null;Assert.AreEqual(1,game.OpeningCaptionIndex);InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return null;Assert.IsTrue(game.OpeningActive);
   while(game.OpeningElapsed<5.7f)yield return null;InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.Enter));yield return null;while(game.OpeningActive)yield return null;int renders=0;game.Interface.ScreenChanged+=()=>renders++;InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return null;Assert.AreEqual(0,renders);Assert.AreEqual(0,game.Journal.HeadSeq);Assert.IsTrue(game.Interface.Focus("handover"));InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.Enter));yield return null;InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return null;Assert.IsTrue(game.Interface.ActionIds.Any(id=>id.StartsWith("line-hb-")));
  }
  [UnityTest] public IEnumerator FocusLossAndPauseStopIntroClock(){
   Click("start");game.SendMessage("OnApplicationFocus",false);float before=game.OpeningElapsed;yield return new WaitForSecondsRealtime(.2f);Assert.AreEqual(before,game.OpeningElapsed);game.SendMessage("OnApplicationFocus",true);game.SendMessage("OnApplicationPause",true);yield return new WaitForSecondsRealtime(.2f);Assert.AreEqual(before,game.OpeningElapsed);game.SendMessage("OnApplicationPause",false);yield return null;Assert.Greater(game.OpeningElapsed,before);Click("intro-skip");Assert.AreEqual(0,game.Journal.HeadSeq);
  }
  [UnityTest] public IEnumerator SettingsPauseAndReducedMotionShowAllLabelsAt150Percent(){
   Click("settings");for(int i=0;i<5;i++)Click("text-scale");Click("back");Click("start");Click("settings");float elapsed=game.OpeningElapsed;yield return new WaitForSecondsRealtime(.2f);Assert.AreEqual(elapsed,game.OpeningElapsed);
   Click("reduced-motion");Click("back");Assert.AreEqual(1.5f,game.Interface.TextScale);yield return null;Canvas.ForceUpdateCanvases();var caption=host.GetComponentsInChildren<ScrollRect>().Single(c=>c.viewport.name=="Opening caption area");Debug.Log("M5_150_LAYOUT screen="+Screen.width+"x"+Screen.height+" caption="+caption.content.rect.height+" viewport="+caption.viewport.rect.height);Assert.LessOrEqual(caption.content.rect.height,caption.viewport.rect.height,"All reduced-motion caption text must fit for keyboard access at150%");foreach(var label in new[]{"관찰","시험","기록"})StringAssert.Contains(label,Text());elapsed=game.OpeningElapsed;yield return new WaitForSecondsRealtime(.2f);Assert.AreEqual(elapsed,game.OpeningElapsed);
   var skip=host.GetComponentsInChildren<Button>().Single(b=>b.name=="intro-skip");var corners=new Vector3[4];((RectTransform)skip.transform).GetWorldCorners(corners);Assert.GreaterOrEqual(corners[0].y,0);Assert.LessOrEqual(corners[2].y,Screen.height);Click("intro-skip");Assert.IsFalse(game.OpeningActive);Assert.IsFalse(File.Exists(Path.Combine(directory,"save.json")));
  }
  [UnityTest] public IEnumerator ReplayFromToolKeepsKeyboardNavigationAndReturnContext(){
   game.StartGame();game.OpenTool("reader");Click("settings");Click("intro-replay");Assert.IsTrue(game.OpeningActive);Assert.IsFalse(game.Watch.ToolPanel);Assert.IsTrue(game.Interface.Focus("intro-skip"));
   InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.DownArrow));yield return null;InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return null;Assert.AreEqual("settings",game.Interface.CurrentFocusId);game.Back();Assert.AreEqual("settings",game.Surface);game.Back();Assert.AreEqual("reader",game.Surface);Assert.AreEqual(0,game.Journal.HeadSeq);
  }
  [UnityTest] public IEnumerator SelectingExistingSlotBypassesAndCreatingNewSlotRestoresFreshIntro(){
   var slot=Path.Combine(directory,"recovery-existing");Directory.CreateDirectory(slot);File.Copy(Path.Combine(Application.dataPath,"_Project/Tests/Fixtures/C1PatrolCompletedV2.json"),Path.Combine(slot,"save.json"));var bytes=File.ReadAllBytes(Path.Combine(slot,"save.json"));
   Click("settings");Click("saved-slots");Click("select-slot-1");Click("start");Assert.IsFalse(game.OpeningActive);Assert.Greater(game.Journal.HeadSeq,0);Click("settings");Click("saved-slots");Click("recovery-new-slot");Click("start");Assert.IsTrue(game.OpeningActive);Assert.AreEqual(0,game.Journal.HeadSeq);Click("settings");Assert.IsFalse(game.Interface.ActionIds.Contains("saved-slots"));Click("back");Click("intro-skip");CollectionAssert.AreEqual(bytes,File.ReadAllBytes(Path.Combine(slot,"save.json")));yield return null;
  }
  [UnityTest] public IEnumerator MissingImageKeepsReadableInkFallbackAndSkip(){
   profile.openingImage=null;var hash=game.Journal.State.StateHash;Click("start");Assert.IsTrue(game.OpeningActive);Assert.IsTrue(game.Interface.ActionIds.Contains("intro-skip"));Assert.IsTrue(host.GetComponentsInChildren<Image>().Any(i=>i.name=="Opening black"));foreach(var label in new[]{"관찰","시험","기록"})StringAssert.Contains(label,Text());Click("intro-skip");AssertNoSave(hash,0);yield return null;
  }
  [UnityTest] public IEnumerator LoadedSaveBypassesIntroAndExplicitReplayPreservesBytesAndContext(){
   yield return RestartWithSignatureSave();var bytes=File.ReadAllBytes(Path.Combine(directory,"save.json"));var hash=game.Journal.State.StateHash;long head=game.Journal.HeadSeq;Click("start");Assert.IsFalse(game.OpeningActive);Click("settings");Click("intro-replay");Assert.IsTrue(game.OpeningActive);game.Undo();game.Redo();game.OpenTool("circuit");Assert.IsTrue(game.OpeningActive);game.Back();Assert.IsFalse(game.OpeningActive);Assert.AreEqual("settings",game.Surface);Assert.AreEqual(hash,game.Journal.State.StateHash);Assert.AreEqual(head,game.Journal.HeadSeq);CollectionAssert.AreEqual(bytes,File.ReadAllBytes(Path.Combine(directory,"save.json")));
  }
  [UnityTest] public IEnumerator SelectedCategoryChangesWithoutRerenderOrJournalWrite(){
   yield return RestartWithSignatureSave();Click("start");Click("continue-c1-signature");var hash=game.Journal.State.StateHash;long head=game.Journal.HeadSeq;int renders=0;game.Interface.ScreenChanged+=()=>renders++;
   Assert.IsTrue(game.Interface.Focus("c1-signature-open-"+game.Definition.Signature.Observations.First()));Assert.AreEqual("observe",game.Interface.CurrentDirectionCategory);
   Assert.IsTrue(game.Interface.Focus("c1-signature-humidity-high"));Assert.AreEqual("trial",game.Interface.CurrentDirectionCategory);Assert.IsTrue(game.Interface.Focus("settings"));Assert.IsNull(game.Interface.CurrentDirectionCategory);Assert.AreEqual(0,renders);Assert.AreEqual(hash,game.Journal.State.StateHash);Assert.AreEqual(head,game.Journal.HeadSeq);StringAssert.DoesNotContain("가장자리가 풀리고",Text());
  }
  [UnityTest] public IEnumerator FirstObservationStillAllowsRiskTrialAndMaskPersists(){
   yield return RestartWithSignatureSave();Click("start");Click("continue-c1-signature");Do("ObserveSignature",game.Definition.Signature.Observations.First());Do("SetSignatureHumidity","high");Do("TrialSignature");Assert.IsFalse(game.SignatureComplete);Assert.IsTrue(host.GetComponentsInChildren<Image>().Any(i=>i.name=="Signature lower obscuration"));yield return Wait(game.FlushSaves());
  }
  [UnityTest] public IEnumerator RecordStatusCannotAcceptPendingFailedOrCancelledSave(){
   yield return RestartWithSignatureSave();Click("start");Ready();yield return Wait(game.FlushSaves());var hash=game.Journal.State.StateHash;var release=new TaskCompletionSource<bool>();game.Store.Injection=(stage,token)=>stage==SaveStage.BeforeRename?release.Task:Task.CompletedTask;
   var command=new PuzzleCommand("ConfirmSignature",game.Definition.Signature.ComparisonId,game.Definition.Signature.RegionId);var pending=game.CommitAsync(command);yield return null;Assert.IsTrue(game.SavePending);StringAssert.Contains("기록 · 저장 확인 중",Text());StringAssert.DoesNotContain("기록 · 저장 완료",Text());game.Undo();release.SetResult(true);yield return Wait(pending);game.Store.Injection=null;yield return Wait(game.FlushSaves());Assert.IsFalse(game.SignatureComplete);StringAssert.DoesNotContain("기록 · 저장 완료",Text());
   Do("SelectSignatureProof",game.Definition.Signature.LeftClue,game.Definition.Signature.RightClue);yield return Wait(game.FlushSaves());game.Store.Injection=(stage,token)=>stage==SaveStage.BeforeRename?Task.FromException(new IOException("M5 expected save failure")):Task.CompletedTask;var failed=game.CommitAsync(command);yield return Wait(failed);Assert.IsFalse(failed.Result);StringAssert.Contains("기록 · 저장 실패",Text());StringAssert.DoesNotContain("기록 · 저장 완료",Text());game.Store.Injection=null;Click("retry-save");while(game.SavePending)yield return null;Assert.IsTrue(game.SignatureComplete);StringAssert.Contains("기록 · 저장 완료",Text());Assert.AreEqual(1,game.SuccessfulReceipts);
  }
 }
}
#endif
