#if TIDE_TEST_FRAMEWORK
using System;
using System.Collections;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Tide.App;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tide.Tests {
 public sealed class T0ResourceInteractionTests {
  T0GameSession game;
  GameObject host, drawer, blocker;
  Scene hub;
  Mouse mouse;
  Keyboard keyboard;
  string directory;
  InputSettings.BackgroundBehavior originalBackground;
#if UNITY_EDITOR
  InputSettings.EditorInputBehaviorInPlayMode originalEditorInput;
#endif

  [UnitySetUp]
  public IEnumerator SetUp() {
#if UNITY_EDITOR
   originalEditorInput=InputSystem.settings.editorInputBehaviorInPlayMode;
   InputSystem.settings.editorInputBehaviorInPlayMode=InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
#endif
   originalBackground=InputSystem.settings.backgroundBehavior;
   InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;
   mouse=InputSystem.AddDevice<Mouse>();
   keyboard=InputSystem.AddDevice<Keyboard>();
   Assert.IsFalse(SceneManager.GetSceneByName("hub").isLoaded,"Fixture must load the committed hub itself");
   yield return SceneManager.LoadSceneAsync("hub",LoadSceneMode.Additive);
   hub=SceneManager.GetSceneByName("hub");
   drawer=hub.GetRootGameObjects().Single(x=>x.name=="T0 approved r03 drawer");
   directory=Path.Combine(Path.GetTempPath(),"t0-resource-"+Guid.NewGuid().ToString("N"));
   host=new GameObject("T0 resource interaction test");
   game=host.AddComponent<T0GameSession>();
   game.Initialize(Resources.Load<T0RuntimeConfig>("T0Runtime"),directory);
   yield return null;
  }

  [UnityTearDown]
  public IEnumerator TearDown() {
   if(game!=null){var saves=game.FlushSaves();while(!saves.IsCompleted)yield return null;}
   if(blocker!=null)UnityEngine.Object.Destroy(blocker);
   if(host!=null)UnityEngine.Object.Destroy(host);
   if(mouse!=null)InputSystem.RemoveDevice(mouse);
   if(keyboard!=null)InputSystem.RemoveDevice(keyboard);
   InputSystem.settings.backgroundBehavior=originalBackground;
#if UNITY_EDITOR
   InputSystem.settings.editorInputBehaviorInPlayMode=originalEditorInput;
#endif
   yield return null;
   if(hub.IsValid()&&hub.isLoaded)yield return SceneManager.UnloadSceneAsync(hub);
   if(directory!=null&&Directory.Exists(directory))Directory.Delete(directory,true);
  }

  Vector2 DrawerFront() {
   var tray=drawer.GetComponentsInChildren<Renderer>().Single(x=>x.name=="SM_Hub_Workbench_Drawer_Tray");
   var point=Camera.main.WorldToScreenPoint(new Vector3(tray.bounds.center.x,tray.bounds.center.y,tray.bounds.min.z));
   Assert.Greater(point.z,0,"Approved drawer front must be in front of the committed camera");
   Assert.IsTrue(Camera.main.pixelRect.Contains(point),"Approved drawer front must be in the scene viewport");
   return point;
  }

  IEnumerator Click(Vector2 point, MouseButton button=MouseButton.Left) {
   InputSystem.QueueStateEvent(mouse,new MouseState{position=point});yield return null;
   InputSystem.QueueStateEvent(mouse,new MouseState{position=point}.WithButton(button));yield return null;
   InputSystem.QueueStateEvent(mouse,new MouseState{position=point});yield return null;
  }

  IEnumerator Enter() {
   InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.Enter));yield return null;
   InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return null;
  }

  void AssertDesk(){Assert.Contains("handover",game.Interface.ActionIds.ToArray());Assert.IsFalse(game.Interface.ActionIds.Contains("inspect-plate-zero"));}

  [UnityTest]
  public IEnumerator LeftClickApprovedDrawerFrontSelectsExistingInvestigationWithoutChangingResource() {
   var transforms=drawer.GetComponentsInChildren<Transform>();
   var matrices=transforms.Select(x=>x.localToWorldMatrix).ToArray();
   var renderers=drawer.GetComponentsInChildren<Renderer>();
   var materials=renderers.Select(x=>x.sharedMaterials).ToArray();
   var meshes=drawer.GetComponentsInChildren<MeshFilter>().Select(x=>x.sharedMesh).ToArray();
   Assert.AreEqual(2,meshes.Length,"Exercise both committed r03 meshes");
   yield return Click(DrawerFront());
   Assert.Contains("start",game.Interface.ActionIds.ToArray(),"World click must not dismiss the start screen");
   Assert.IsTrue(game.Interface.Focus("start"));yield return Enter();AssertDesk();
   var state=game.Journal.State.StateHash;
   yield return Click(DrawerFront(),MouseButton.Right);AssertDesk();
   yield return Click(DrawerFront());
   Assert.Contains("inspect-plate-zero",game.Interface.ActionIds.ToArray(),"Left-click on the visible approved r03 drawer must select its existing investigation");
   Assert.IsTrue(game.Interface.Activate("inspect-plate-zero"));
   Assert.AreEqual(state,game.Journal.State.StateHash,"Investigation navigation must not mutate puzzle state");
   CollectionAssert.AreEqual(matrices,transforms.Select(x=>x.localToWorldMatrix).ToArray());
   CollectionAssert.AreEqual(meshes,drawer.GetComponentsInChildren<MeshFilter>().Select(x=>x.sharedMesh).ToArray());
   for(int i=0;i<renderers.Length;i++)CollectionAssert.AreEqual(materials[i],renderers[i].sharedMaterials);
   Assert.IsTrue(game.Interface.Activate("hub-view-desk"));AssertDesk();
   Assert.IsTrue(game.Interface.Focus("hub-view-drawer"));yield return Enter();
   Assert.Contains("inspect-plate-zero",game.Interface.ActionIds.ToArray(),"Keyboard navigation remains available");
  }

  [UnityTest]
  public IEnumerator SceneClickDoesNotCrossOverlayToolDocumentOrUi() {
   game.StartGame();yield return null;
   game.OpenOverlay("hints");yield return Click(DrawerFront());Assert.AreEqual("hints",game.Surface);
   game.Back();AssertDesk();
   game.OpenTool("reader");yield return Click(DrawerFront());Assert.AreEqual("reader",game.Surface);
   game.Back();AssertDesk();
   Assert.IsTrue(game.Interface.Activate("handover"));yield return Click(DrawerFront());
   Assert.Contains("close-document",game.Interface.ActionIds.ToArray());
   game.Back();AssertDesk();
   var point=DrawerFront();
   var canvas=host.GetComponentInChildren<Canvas>();
   blocker=new GameObject("Test UI occluder",typeof(RectTransform),typeof(Image));
   var rect=blocker.GetComponent<RectTransform>();rect.SetParent(canvas.transform,false);
   rect.anchorMin=rect.anchorMax=Vector2.zero;rect.sizeDelta=new Vector2(120,120);rect.position=point;
   yield return null;yield return Click(point);AssertDesk();
   UnityEngine.Object.Destroy(blocker);blocker=null;yield return null;
   yield return Click(new Vector2(-10,-10));AssertDesk();
   yield return Click(DrawerFront());
   Assert.Contains("inspect-plate-zero",game.Interface.ActionIds.ToArray(),"Removing all input blockers restores drawer interaction");
  }

  [UnityTest]
  public IEnumerator NonReadableDrawerPickingRejectsHiddenOrMissedBounds() {
   Assert.IsTrue(drawer.GetComponentsInChildren<MeshFilter>().All(x=>!x.sharedMesh.isReadable),"Committed r03 meshes must remain non-readable");
   game.StartGame();yield return null;
   var renderers=drawer.GetComponentsInChildren<Renderer>();
   var point=DrawerFront();
   foreach(var renderer in renderers)renderer.enabled=false;
   yield return Click(point);AssertDesk();
   foreach(var renderer in renderers)renderer.enabled=true;
   drawer.SetActive(false);yield return Click(point);AssertDesk();drawer.SetActive(true);
   var camera=Camera.main;int mask=camera.cullingMask;
   try {camera.cullingMask=0;yield return Click(point);AssertDesk();}
   finally {camera.cullingMask=mask;}
   var miss=new Vector2(camera.pixelRect.xMin+1,camera.pixelRect.yMax-1);
   Assert.IsTrue(renderers.All(x=>!x.bounds.IntersectRay(camera.ScreenPointToRay(miss))),"Miss fixture must avoid both approved renderers");
   yield return Click(miss);AssertDesk();
   yield return Click(DrawerFront());
   Assert.Contains("inspect-plate-zero",game.Interface.ActionIds.ToArray(),"Visible approved renderers remain selectable without collider cooking");
  }

  [UnityTest]
  public IEnumerator CloserRendererWithoutColliderBlocksDrawerSelection() {
   game.StartGame();yield return null;
   var point=DrawerFront();var camera=Camera.main;var ray=camera.ScreenPointToRay(point);
   blocker=GameObject.CreatePrimitive(PrimitiveType.Cube);blocker.name="Test renderer-only occluder";
   UnityEngine.Object.Destroy(blocker.GetComponent<Collider>());
   blocker.transform.position=ray.GetPoint(.3f);blocker.transform.localScale=Vector3.one*.15f;
   yield return null;Physics.SyncTransforms();
   Assert.IsEmpty(blocker.GetComponentsInChildren<Collider>(),"The regression must exercise geometry without a Collider");
   var renderer=blocker.GetComponent<Renderer>();
   Assert.IsTrue(renderer.enabled&&renderer.gameObject.activeInHierarchy);
   Assert.IsTrue(GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(camera),renderer.bounds),"Occluder must be inside the interaction camera frustum");
   Assert.IsTrue(renderer.bounds.IntersectRay(ray,out var blockerDistance));
   var drawerDistance=drawer.GetComponentsInChildren<Renderer>().Select(x=>x.bounds.IntersectRay(ray,out var distance)?distance:float.PositiveInfinity).Min();
   Assert.Less(blockerDistance,drawerDistance,"Renderer-only geometry must precede the drawer on the same screen ray");
   Assert.IsFalse(Physics.Raycast(ray,drawerDistance,camera.cullingMask,QueryTriggerInteraction.Ignore),"A Physics raycast must not mask the renderer-only regression");
   yield return Click(point);
   Assert.IsFalse(game.Interface.ActionIds.Contains("inspect-plate-zero"),"A nearer visible renderer without a Collider must block drawer selection");
   AssertDesk();
   Assert.IsTrue(game.Interface.Focus("hub-view-drawer"));yield return Enter();
   Assert.Contains("inspect-plate-zero",game.Interface.ActionIds.ToArray(),"Occlusion must not disable keyboard navigation");
   Assert.IsTrue(game.Interface.Activate("hub-view-desk"));AssertDesk();
   UnityEngine.Object.Destroy(blocker);blocker=null;yield return null;
   yield return Click(DrawerFront());
   Assert.Contains("inspect-plate-zero",game.Interface.ActionIds.ToArray(),"Removing renderer-only geometry restores visible drawer selection");
  }

  [UnityTest]
  public IEnumerator NonVisibleOrFartherRenderersDoNotBlockDrawerSelection() {
   game.StartGame();Assert.IsTrue(game.Interface.Activate("hub-view-desk"));yield return null;
   var point=DrawerFront();var camera=Camera.main;var ray=camera.ScreenPointToRay(point);int mask=camera.cullingMask;
   blocker=GameObject.CreatePrimitive(PrimitiveType.Cube);blocker.name="Test filtered renderer-only occluder";
   UnityEngine.Object.Destroy(blocker.GetComponent<Collider>());blocker.layer=31;
   blocker.transform.localScale=Vector3.one*.15f;yield return null;
   Assert.IsTrue((mask&(1<<blocker.layer))!=0,"The camera must initially include the occluder layer so each visibility filter is independently exercised");
   Assert.IsEmpty(blocker.GetComponentsInChildren<Collider>());
   var renderer=blocker.GetComponent<Renderer>();
   var drawerDistance=drawer.GetComponentsInChildren<Renderer>().Select(x=>x.bounds.IntersectRay(ray,out var distance)?distance:float.PositiveInfinity).Min();
   var cases=new (string label,Action change)[]{
    ("disabled",()=>renderer.enabled=false),
    ("inactive",()=>blocker.SetActive(false)),
    ("forceRenderingOff",()=>renderer.forceRenderingOff=true),
    ("camera-excluded layer",()=>camera.cullingMask=mask&~(1<<blocker.layer)),
    ("shadows only",()=>renderer.shadowCastingMode=UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly),
    ("behind drawer",()=>blocker.transform.position=ray.GetPoint(drawerDistance+1)),
    ("behind camera",()=>blocker.transform.position=camera.transform.position-camera.transform.forward)
   };
   try {
    foreach(var test in cases){
     blocker.SetActive(true);renderer.enabled=true;renderer.forceRenderingOff=false;
     renderer.shadowCastingMode=UnityEngine.Rendering.ShadowCastingMode.On;
     camera.cullingMask=mask;blocker.transform.position=ray.GetPoint(.3f);test.change();
     yield return Click(point);
     Assert.Contains("inspect-plate-zero",game.Interface.ActionIds.ToArray(),test.label+" geometry must not block visible drawer selection");
     Assert.IsTrue(game.Interface.Activate("hub-view-desk"));AssertDesk();
    }
   } finally {camera.cullingMask=mask;}
  }

  [UnityTest]
  public IEnumerator CloserSceneGeometryBlocksDrawerSelection() {
   game.StartGame();yield return null;
   var point=DrawerFront();var ray=Camera.main.ScreenPointToRay(point);
   blocker=GameObject.CreatePrimitive(PrimitiveType.Cube);blocker.name="Test scene occluder";
   blocker.transform.position=ray.GetPoint(.3f);blocker.transform.localScale=Vector3.one*.15f;
   Physics.SyncTransforms();yield return Click(point);AssertDesk();
   UnityEngine.Object.Destroy(blocker);blocker=null;yield return null;
   yield return Click(DrawerFront());
   Assert.Contains("inspect-plate-zero",game.Interface.ActionIds.ToArray(),"An unobscured drawer must remain selectable");
  }
 }
}
#endif
