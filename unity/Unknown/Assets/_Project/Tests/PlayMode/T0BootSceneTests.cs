#if TIDE_TEST_FRAMEWORK
using System;
using System.Collections;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Tide.App;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
namespace Tide.Tests
{
    public sealed class T0BootSceneTests
    {
        T0GameSession game;
        string directory;
        [UnityTest] public IEnumerator SerializedBootLoadsHubAndInitializesVisibleStartScreen()
        {
            var args=Environment.GetCommandLineArgs();int index=Array.IndexOf(args,"--t0-save-dir");
            if(index<0||index+1>=args.Length)Assert.Ignore("Run with an explicit isolated --t0-save-dir for serialized boot verification.");
            var candidate=Path.GetFullPath(args[index+1]);Assert.IsTrue(candidate.StartsWith("/tmp/unknown-c1-m4-boot-"),"Boot verification requires its task-specific temporary prefix.");
            Assert.IsFalse(Directory.Exists(candidate),"Boot test needs a newly owned directory.");Directory.CreateDirectory(candidate);directory=candidate;
            File.Copy(Path.Combine(Application.dataPath,"_Project/Tests/Fixtures/C1PatrolCompletedV2.json"),Path.Combine(directory,"save.json"));
            yield return SceneManager.LoadSceneAsync("boot",LoadSceneMode.Single);
            var deadline=Time.realtimeSinceStartup+15;
            while(Time.realtimeSinceStartup<deadline)
            {
                game=UnityEngine.Object.FindObjectsByType<T0GameSession>().FirstOrDefault();if(game!=null&&game.Interface!=null&&game.Interface.ActionIds!=null)break;yield return null;
            }
            Assert.IsNotNull(game,"T0Entry did not initialize the game session");Assert.IsTrue(SceneManager.GetSceneByName("ui-root").isLoaded);Assert.IsTrue(SceneManager.GetSceneByName("hub").isLoaded);
            Assert.IsNotNull(game.Simulation);Assert.IsNotNull(game.Journal);CollectionAssert.Contains(game.Interface.ActionIds,"start");var canvas=game.GetComponentsInChildren<Canvas>().Single(c=>c.gameObject.activeInHierarchy&&c.name=="Interface Canvas");Assert.IsTrue(canvas.enabled);var start=game.GetComponentsInChildren<UnityEngine.UI.Button>().Single(b=>b.name=="start"&&b.gameObject.activeInHierarchy);Assert.IsTrue(start.enabled&&start.interactable);var rect=((RectTransform)start.transform).rect;Assert.Greater(rect.width,0);Assert.Greater(rect.height,0);Assert.IsFalse(string.IsNullOrEmpty(start.GetComponentInChildren<UnityEngine.UI.Text>().text));Assert.IsTrue(game.PatrolComplete);Assert.IsFalse(game.SignatureActive);
        }
        [UnityTearDown] public IEnumerator TearDown()
        {
            if(game!=null){var flush=game.FlushSaves();while(!flush.IsCompleted)yield return null;UnityEngine.Object.Destroy(game.gameObject);yield return null;}
            var cleanup=SceneManager.CreateScene("Boot verification cleanup");SceneManager.SetActiveScene(cleanup);
            foreach(var name in new[]{"boot","ui-root","hub"}){var scene=SceneManager.GetSceneByName(name);if(scene.IsValid()&&scene.isLoaded)yield return SceneManager.UnloadSceneAsync(scene);}
            if(directory!=null&&Directory.Exists(directory))Directory.Delete(directory,true);
        }
    }
}
#endif
