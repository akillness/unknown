#if UNITY_EDITOR
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using Tide.UI;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
namespace Tide.EditorTools {
 public static class T0ViewportDiagnostics {
  public static void Capture(){
   EditorSceneManager.OpenScene("Assets/_Project/Scenes/hub.unity");var camera=Camera.main;
   var data=JObject.Parse(File.ReadAllText("Assets/_Project/Data/Tables/zones.json"));var node=data["rows"][0]["viewNodes"].First(n=>(string)n["nodeId"]=="hub-view-drawer");var pose=node["cameraPose"];
   camera.transform.position=Point(pose["pos"]);camera.transform.LookAt(Point(pose["lookAt"]));
   var uiCamera=new GameObject("Diagnostic UI camera",typeof(Camera)).GetComponent<Camera>();uiCamera.clearFlags=CameraClearFlags.SolidColor;uiCamera.backgroundColor=Color.clear;uiCamera.cullingMask=1<<5;uiCamera.nearClipPlane=.01f;uiCamera.farClipPlane=10;camera.cullingMask&=~(1<<5);
   var host=new GameObject("Diagnostic UI");var ui=host.AddComponent<T0Interface>();ui.Initialize();var model=new GameScreen{Title="서랍 시점 · Drawer viewport",Subtitle="Native diagnostic — actual UI layout",Body="서랍칸의 판 #0을 조사합니다.",Footer="Tab 초점 · Enter 선택"};foreach(var v in data["rows"][0]["viewNodes"])model.Navigation.Add(new ViewAction{Id=(string)v["nodeId"],Label=(string)v["label"]});model.Actions.Add(new ViewAction{Id="drawer-slot",Label="서랍칸 판 #0 조사"});ui.Render(model);
   foreach(var t in host.GetComponentsInChildren<Transform>())t.gameObject.layer=5;var canvas=host.GetComponentInChildren<Canvas>();canvas.renderMode=RenderMode.ScreenSpaceCamera;canvas.worldCamera=uiCamera;canvas.planeDistance=.3f;
   var output="Builds/t0-viewport-diagnostics";Directory.CreateDirectory(output);var rt=RenderTexture.GetTemporary(1280,800,24);uiCamera.targetTexture=rt;Canvas.ForceUpdateCanvases();var viewport=ui.SceneViewport;
   camera.rect=new Rect(0,0,1,1);camera.aspect=1280f/800;camera.fieldOfView=Camera.HorizontalToVerticalFieldOfView((float)pose["fovDeg"],camera.aspect);CaptureFrame(camera,uiCamera,rt,output+"/drawer-before-ui-occlusion.png");
   camera.rect=viewport;camera.aspect=1280*viewport.width/(800*viewport.height);camera.fieldOfView=Camera.HorizontalToVerticalFieldOfView((float)pose["fovDeg"],camera.aspect);CaptureFrame(camera,uiCamera,rt,output+"/drawer-after-viewport.png");
   File.WriteAllText(output+"/viewport.json",new JObject{["viewport"]=new JArray(viewport.x,viewport.y,viewport.width,viewport.height),["horizontalFov"]=(float)pose["fovDeg"],["width"]=1280,["height"]=800,["graphicsDevice"]=SystemInfo.graphicsDeviceType.ToString(),["scope"]="Native scene + same T0Interface layout with diagnostic labels; director actual standalone smoke is separate"}.ToString());uiCamera.targetTexture=null;RenderTexture.ReleaseTemporary(rt);Debug.Log("T0_VIEWPORT_DIAGNOSTICS "+output);
  }
  static Vector3 Point(JToken p)=>new Vector3((float)p["x"],(float)p["z"],(float)p["y"]);
  static void CaptureFrame(Camera scene,Camera ui,RenderTexture target,string path){
   var previous=RenderTexture.active;RenderTexture.active=target;GL.Clear(true,true,scene.backgroundColor);RenderTexture.active=previous;
   RenderPipeline.SubmitRenderRequest(scene,new UniversalRenderPipeline.SingleCameraRequest{destination=target});
   var uiTarget=RenderTexture.GetTemporary(target.width,target.height,24,RenderTextureFormat.ARGB32);ui.targetTexture=uiTarget;Canvas.ForceUpdateCanvases();RenderPipeline.SubmitRenderRequest(ui,new UniversalRenderPipeline.SingleCameraRequest{destination=uiTarget});
   RenderTexture.active=target;GL.PushMatrix();GL.LoadPixelMatrix(0,target.width,target.height,0);Graphics.DrawTexture(new Rect(0,0,target.width,target.height),uiTarget);GL.PopMatrix();
   var image=new Texture2D(target.width,target.height,TextureFormat.RGB24,false);image.ReadPixels(new Rect(0,0,target.width,target.height),0,0);image.Apply();File.WriteAllBytes(path,image.EncodeToPNG());RenderTexture.active=previous;Object.DestroyImmediate(image);ui.targetTexture=null;RenderTexture.ReleaseTemporary(uiTarget);
  }
 }
}
#endif
