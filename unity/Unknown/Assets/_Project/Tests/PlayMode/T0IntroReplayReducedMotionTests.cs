#if TIDE_TEST_FRAMEWORK
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Tide.App;
using Tide.Presentation;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
namespace Tide.Tests {
 // M17. presentation/intro-gameplay-m5.json: reducedMotion = "one static frame, all three verb labels
 // immediately visible, explicit continue, no timed wait" and replayExplicitOnly = true.
 // systems/system-specs/intro-gameplay-m5.md: "Settings offers explicit replay while no save is pending.
 // Replay ... returns to the prior settings overlay."
 // A reduced-motion player who explicitly asks to re-read the onboarding guidance must therefore get that
 // static frame. Fresh-start reduced motion keeps its verified immediate-start behaviour
 // (M5DirectionPlayModeTests.ReducedMotionStartsFreshGameImmediatelyWithoutWrites) and is not touched here.
 public sealed class T0IntroReplayReducedMotionTests {
  string directory;GameObject host;T0GameSession game;M5DirectionProfile profile;
  [UnitySetUp] public IEnumerator Setup(){
   directory=Path.Combine(Path.GetTempPath(),"m17-intro-replay-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(directory);
   profile=Resources.Load<M5DirectionProfile>("M5Direction");Assert.IsNotNull(profile,"M5Direction profile must be present");
   host=new GameObject("M17 intro replay test");game=host.AddComponent<T0GameSession>();
   game.Initialize(Resources.Load<T0RuntimeConfig>("T0Runtime"),directory);yield return null;
  }
  [UnityTearDown] public IEnumerator Teardown(){
   yield return Wait(game.FlushSaves());UnityEngine.Object.Destroy(host);yield return null;
   if(Directory.Exists(directory))Directory.Delete(directory,true);
  }
  IEnumerator Wait(Task task){while(!task.IsCompleted)yield return null;if(task.IsFaulted)throw task.Exception;}
  void Click(string id){Assert.IsTrue(game.Interface.Focus(id),id);game.Interface.BeginActivation();game.Interface.EndActivation();}
  string Text()=>string.Join("\n",host.GetComponentsInChildren<Text>().Select(t=>t.text));
  bool OpeningVisible()=>host.GetComponentsInChildren<Image>(true).Any(i=>i.name=="Opening black");

  [UnityTest] public IEnumerator ExplicitIntroReplayUnderReducedMotionPresentsStaticGuidanceAndReturnsWithoutWrites(){
   // Setup: turn reduced motion on from the accessibility overlay and stay there.
   Click("settings");Click("reduced-motion");
   Assert.AreEqual("settings",game.Surface,"reduced motion must be togglable from the accessibility overlay");
   StringAssert.Contains("모션 축소: True",Text(),"reduced motion must actually be enabled before replay");
   Assert.IsTrue(game.Interface.ActionIds.Contains("intro-replay"),"explicit replay must be offered while no save is pending");
   var hash=game.Journal.State.StateHash;var head=game.Journal.HeadSeq;

   // The defect: explicit replay under reduced motion must present the static guidance frame.
   Click("intro-replay");yield return null;
   Assert.IsTrue(game.OpeningActive,"explicit replay under reduced motion must present the opening guidance");
   Assert.IsTrue(OpeningVisible(),"the static opening frame must actually be on screen");
   foreach(var label in new[]{"관찰","시험","기록"})
    StringAssert.Contains(label,Text(),"all three verb labels must be immediately visible under reduced motion");
   Assert.IsTrue(game.Interface.ActionIds.Contains("intro-skip"),"reduced motion requires an explicit continue");

   // "no timed wait": the static frame must not auto-advance or auto-dismiss.
   float elapsed=game.OpeningElapsed;yield return new WaitForSecondsRealtime(.2f);
   Assert.AreEqual(elapsed,game.OpeningElapsed,"reduced motion must not run the intro clock");
   Assert.IsTrue(game.OpeningActive,"the static frame must wait for an explicit continue");

   // Explicit continue returns to the prior settings overlay and writes nothing.
   Click("intro-skip");
   Assert.IsFalse(game.OpeningActive);
   Assert.AreEqual("settings",game.Surface,"replay must return to the prior settings overlay");
   Assert.AreEqual(hash,game.Journal.State.StateHash);
   Assert.AreEqual(head,game.Journal.HeadSeq);
   Assert.IsFalse(File.Exists(Path.Combine(directory,"save.json")),"replay must not serialize a save");
  }
 }
}
#endif
