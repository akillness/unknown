#if TIDE_TEST_FRAMEWORK
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Tide.App;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;
using UnityEngine.TestTools;
using UnityEngine.UI;
namespace Tide.Tests {
 // RFC-CX-016 carried item 3 (M9 observation, decision-log.md:517): the tick selector shows ~8 of its 30
 // rows per page and "the wheel never reached the list"; that pass recovered only by ScrollRect drag.
 // It drove the shipped player with cliclick, whose synthetic events never reached the Unity Input System
 // (native-playtest-m9/verification.md:24 — the same boundary that blocked its synthetic keys), and the
 // wheel path carried no automated coverage at all. These tests pin the contracts the observation touches:
 // the UGUI wheel chain over the work surface resolves to the content ScrollRect and moves it, a real
 // device wheel tick reaches it through the input module, and the keyboard alone reaches every row of the
 // longest list with the scroll following it (interaction-rules §0-8).
 public sealed class ScrollReachabilityTests {
#if UNITY_EDITOR
  InputSettings.EditorInputBehaviorInPlayMode originalEditorInput;
#endif
  InputSettings.BackgroundBehavior originalBackground;T0GameSession game;GameObject host;Keyboard keyboard;Mouse mouse;string directory;
  [UnitySetUp] public IEnumerator SetUp(){
#if UNITY_EDITOR
   originalEditorInput=InputSystem.settings.editorInputBehaviorInPlayMode;InputSystem.settings.editorInputBehaviorInPlayMode=InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
#endif
   originalBackground=InputSystem.settings.backgroundBehavior;InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;
   directory=Path.Combine(Path.GetTempPath(),"t0-scroll-"+Guid.NewGuid().ToString("N"));
   keyboard=InputSystem.AddDevice<Keyboard>();mouse=InputSystem.AddDevice<Mouse>();
   host=new GameObject("T0 scroll test");game=host.AddComponent<T0GameSession>();
   game.Initialize(Resources.Load<T0RuntimeConfig>("T0Runtime"),directory);yield return null;
  }
  [UnityTearDown] public IEnumerator TearDown(){
   var task=game.FlushSaves();while(!task.IsCompleted)yield return null;
   UnityEngine.Object.Destroy(host);InputSystem.RemoveDevice(keyboard);InputSystem.RemoveDevice(mouse);
   InputSystem.settings.backgroundBehavior=originalBackground;
#if UNITY_EDITOR
   InputSystem.settings.editorInputBehaviorInPlayMode=originalEditorInput;
#endif
   yield return null;if(Directory.Exists(directory))Directory.Delete(directory,true);
  }
  IEnumerator Wait(Task task){while(!task.IsCompleted)yield return null;if(task.IsFaulted)throw task.Exception;}
  IEnumerator PressKey(Key key){InputSystem.QueueStateEvent(keyboard,new KeyboardState(key));yield return null;InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return null;}
  void Click(string id){Assert.IsTrue(game.Interface.Activate(id),"Missing UI action "+id);if(id=="start"&&game.OpeningActive)Assert.IsTrue(game.Interface.Activate("intro-skip"));}
  void Intake(){Click("start");Click("handover");foreach(var id in new[]{"hb-l1","hb-l2","hb-l3"})Click("line-"+id);Click("close-document");Click("transfer");foreach(var id in new[]{"tl-r1","tl-r2","tl-r3"})Click("row-"+id);Click("decision-written");Click("close-document");Click("load-plate-zero");}
  void Circuit(){Click("hub-view-circuitmap");Click("open-circuit");Click("trace-hub");Click("begin-overlay");Click("offset-left");Click("offset-up");Click("anchor-overlay");foreach(var area in game.Definition.UncoveredAreas){Click("area-"+area);Click("area-evidence-"+area);Click("attach-rec-watchlog-bureau");Click("overlay-back");}}
  // The tick selector is the longest list in T0: 180 phases paged 30 at a time, far past one viewport.
  IEnumerator TickSelector(){
   Intake();Circuit();Click("hub-view-reader");Click("open-reader");Click("load-rec-plate-standard-hub");Click("read");
   yield return Wait(game.FlushSaves());
   Click("pick-start");
   Assert.AreEqual("phasePicker",game.Surface);
   yield return null;Canvas.ForceUpdateCanvases();yield return null;
  }
  ScrollRect Content()=>host.GetComponentsInChildren<ScrollRect>().Single(s=>s.viewport.name=="Viewport");
  static Vector2 ScreenCenter(RectTransform rect)=>RectTransformUtility.WorldToScreenPoint(null,rect.TransformPoint(rect.rect.center));
  static (float bottom,float top) Band(RectTransform rect){var corners=new Vector3[4];rect.GetWorldCorners(corners);return (corners[0].y,corners[2].y);}

  [UnityTest] public IEnumerator PointerWheelOverTheWorkSurfaceScrollsTheLongestList(){
   yield return TickSelector();
   var scroll=Content();
   Assert.Greater(scroll.content.rect.height,scroll.viewport.rect.height,"The tick selector must overflow one viewport for scrolling to mean anything");
   Assert.IsTrue(scroll.vertical,"The work surface list scrolls vertically");
   Assert.Greater(scroll.scrollSensitivity,0,"A zero sensitivity would swallow every wheel tick");

   // The wheel chain: a raycast over the work surface must hit something, and IScrollHandler resolution
   // from that hit must land on the ScrollRect. A non-raycastable ancestor chain is what would break it.
   var point=ScreenCenter(scroll.viewport);
   var hits=new List<RaycastResult>();
   EventSystem.current.RaycastAll(new PointerEventData(EventSystem.current){position=point},hits);
   Assert.IsNotEmpty(hits,"The work surface must be raycastable or no wheel event is ever dispatched");
   var handler=ExecuteEvents.GetEventHandler<IScrollHandler>(hits[0].gameObject);
   Assert.AreSame(scroll.gameObject,handler,"The wheel handler above the pointer must be the content ScrollRect");

   scroll.verticalNormalizedPosition=1;Canvas.ForceUpdateCanvases();
   var data=new PointerEventData(EventSystem.current){position=point,scrollDelta=new Vector2(0,-10)};
   ExecuteEvents.ExecuteHierarchy(handler,data,ExecuteEvents.scrollHandler);
   var scrolledDown=scroll.verticalNormalizedPosition;
   Assert.Less(scrolledDown,1f-.005f,"A wheel-down tick must move the list away from the top");
   data.scrollDelta=new Vector2(0,10);
   ExecuteEvents.ExecuteHierarchy(handler,data,ExecuteEvents.scrollHandler);
   Assert.Greater(scroll.verticalNormalizedPosition,scrolledDown+.001f,"A wheel-up tick must move the list back");
  }

  [UnityTest] public IEnumerator RealMouseWheelReachesTheListThroughTheInputModule(){
   yield return TickSelector();
   var module=EventSystem.current.GetComponent<InputSystemUIInputModule>();
   Assert.IsNotNull(module,"The session wires InputSystemUIInputModule at Initialize");
   Assert.IsNotNull(module.scrollWheel,"AssignDefaultActions must leave a scroll-wheel reference bound");
   Assert.IsNotNull(module.scrollWheel.action,"An unbound scroll action would drop every wheel tick");

   var scroll=Content();
   var point=ScreenCenter(scroll.viewport);
   scroll.verticalNormalizedPosition=1;Canvas.ForceUpdateCanvases();
   // Position first, then the same position carrying wheel ticks: the module raycasts on the settled
   // pointer and dispatches the scroll to whatever owns IScrollHandler above it. The magnitude is large
   // enough that both the raw and the per-tick-scaled reading of the device value move the list.
   InputSystem.QueueStateEvent(mouse,new MouseState{position=point});yield return null;
   InputSystem.QueueStateEvent(mouse,new MouseState{position=point,scroll=new Vector2(0,-1200)});yield return null;
   Assert.Less(scroll.verticalNormalizedPosition,1f-.005f,"A device wheel tick must reach the content ScrollRect");
  }

  [UnityTest] public IEnumerator KeyboardAloneReachesEveryTickRowWithTheScrollFollowing(){
   yield return TickSelector();
   var scroll=Content();
   var wanted=new HashSet<string>(game.Interface.ActionIds,StringComparer.Ordinal);
   Assert.Greater(wanted.Count,30,"Page one of the tick selector plus its paging and back actions");
   var visited=new HashSet<string>(StringComparer.Ordinal);
   for(int press=0;press<240&&!wanted.All(visited.Contains);press++){
    yield return PressKey(Key.Tab);
    var selected=EventSystem.current.currentSelectedGameObject;
    if(selected==null||!wanted.Contains(selected.name)||!selected.transform.IsChildOf(scroll.content))continue;
    if(!visited.Add(selected.name))continue;
    Canvas.ForceUpdateCanvases();
    var band=Band(scroll.viewport);
    var row=Band((RectTransform)selected.transform);
    var center=(row.bottom+row.top)*.5f;
    Assert.That(center,Is.GreaterThan(band.bottom).And.LessThan(band.top),"Keyboard focus on "+selected.name+" must scroll it into the viewport");
   }
   CollectionAssert.IsEmpty(wanted.Except(visited).ToArray(),"Tab alone must reach every action of the longest list");
  }
 }
}
#endif
