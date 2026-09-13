#if TIDE_TEST_FRAMEWORK
using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using Tide.App;
using Tide.Save;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;
namespace Tide.Tests {
 // M15: accessibility settings are player-owned and must not depend on which save slot is active.
 // The recovery overlay moves T0GameSession.Store off the save root, so a settings write anchored to
 // the store lands in a directory that boot never reads back.
 public sealed class T0SettingsSlotPersistenceTests {
  #if UNITY_EDITOR
  InputSettings.EditorInputBehaviorInPlayMode originalEditorInput;
  #endif
  InputSettings.BackgroundBehavior originalBackground;T0GameSession game;GameObject host;Keyboard keyboard;string directory;
  [UnitySetUp] public IEnumerator SetUp(){
#if UNITY_EDITOR
originalEditorInput=InputSystem.settings.editorInputBehaviorInPlayMode;InputSystem.settings.editorInputBehaviorInPlayMode=InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
#endif
originalBackground=InputSystem.settings.backgroundBehavior;InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;directory=Path.Combine(Path.GetTempPath(),"t0-settings-slot-"+Guid.NewGuid().ToString("N"));keyboard=InputSystem.AddDevice<Keyboard>();Create();yield return null;}
  [UnityTearDown] public IEnumerator TearDown(){var task=game.FlushSaves();while(!task.IsCompleted)yield return null;UnityEngine.Object.Destroy(host);InputSystem.RemoveDevice(keyboard);InputSystem.settings.backgroundBehavior=originalBackground;
#if UNITY_EDITOR
InputSystem.settings.editorInputBehaviorInPlayMode=originalEditorInput;
#endif
yield return null;if(Directory.Exists(directory))Directory.Delete(directory,true);}
  void Create(){host=new GameObject("T0 settings slot test");game=host.AddComponent<T0GameSession>();game.Initialize(Resources.Load<T0RuntimeConfig>("T0Runtime"),directory);}
  IEnumerator Wait(Task task){while(!task.IsCompleted)yield return null;if(task.IsFaulted)throw task.Exception;}
  IEnumerator Restart(){yield return Wait(game.FlushSaves());UnityEngine.Object.Destroy(host);yield return null;Create();yield return null;}
  void Click(string id){Assert.IsTrue(game.Interface.Activate(id),"Missing UI action "+id);}
  [UnityTest] public IEnumerator TextScaleChosenInARecoverySlotSurvivesRestart(){
   // Make the root save unreadable so the recovery overlay is the player's real entry point.
   var future=JournalSave.Encode(game.Journal,"future",20000,6291456);future["schemaVersion"]=4;
   yield return Wait(game.Store.WriteAsync(future));
   yield return Restart();
   Assert.AreEqual("recovery",game.Surface,"A future-schema root save must open the recovery overlay");
   Click("recovery-new-slot");
   Assert.AreNotEqual(Path.GetFullPath(directory),Path.GetFullPath(game.Store.DirectoryPath),"Taking a new slot must move the save store off the save root");
   // A player who needs larger text raises it after taking that slot.
   Click("settings");for(int i=0;i<5;i++)Click("text-scale");game.Back();
   yield return null;
   Assert.AreEqual(1.5f,game.Interface.TextScale,"Text scale must apply in the session that set it");
   yield return Restart();
   Assert.AreEqual(1.5f,game.Interface.TextScale,"Text scale chosen while a recovery slot is active must survive a restart");
  }
 }
}
#endif
