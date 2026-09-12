#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using Tide.App;
using Tide.Presentation;
using Tide.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Tide.EditorTools {
 // RFC-CX-013 orchestration: one import entry point for the three M7 lanes and a native proof-shot capture.
 // Capture applies the candidates IN MEMORY only (never saves the hub scene, never flips runtimeApproved).
 public static class M7ProjectBuilder {
  const string Output="Builds/m7-diagnostics";
  [MenuItem("Tools/M7/Import all M7 candidates")]
  public static void ImportAll(){
   M7HubProjectBuilder.ImportHubShell();
   M7UiSkinProjectBuilder.ImportUiSkin();
   M7ReaderProjectBuilder.ImportReaderStage();
   AssetDatabase.Refresh();
   Debug.Log("M7_IMPORT_ALL_DONE");
  }
  // RFC-CX-016 promotion: the director asked for all four lanes at once, so this only *calls* the four existing
  // director-only approval menus (nothing here writes runtimeApproved itself) and records the gate state either side.
  // -executeMethod entry point because the menu items are not reachable from batchmode.
  [MenuItem("Tools/M7/Approve all lanes (director only)")]
  public static void ApproveAll(){
   var audit=new JObject{["scope"]="RFC-CX-016 runtime promotion of the four gated concept profiles",
    ["approvalPath"]="Tools/M7/Approve hub shell · Tools/M7/Approve UI skin · Tools/M7/Approve reader stage · Tools/M8/Approve review card",
    ["before"]=GateState()};
   Debug.Log("M7_GATES_BEFORE "+audit["before"].ToString(Newtonsoft.Json.Formatting.None));
   M7HubProjectBuilder.ApproveHubShell();
   M7UiSkinProjectBuilder.ApproveUiSkin();
   M7ReaderProjectBuilder.ApproveReaderStage();
   M8ReviewNotesProjectBuilder.ApproveReviewCard();
   AssetDatabase.SaveAssets();AssetDatabase.Refresh();
   audit["after"]=GateState();
   Debug.Log("M7_GATES_AFTER "+audit["after"].ToString(Newtonsoft.Json.Formatting.None));
   var path=Argument("--m7-approval-audit","Builds/m7-approval-audit.json");
   Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(path)));
   File.WriteAllText(path,audit.ToString());
   Debug.Log("M7_APPROVE_ALL_DONE "+path);
  }
  static string Argument(string key,string fallback){var args=Environment.GetCommandLineArgs();int i=Array.IndexOf(args,key);return i>=0&&i+1<args.Length?args[i+1]:fallback;}
  // Both load routes are recorded: the runtime and the PlayMode fixtures use Resources.Load, the approval menus use AssetDatabase.
  static JObject Lane<T>(string resource,string assetPath,Func<T,bool> approved,Func<T,string> refs) where T:ScriptableObject{
   var viaResources=Resources.Load<T>(resource);var viaDatabase=AssetDatabase.LoadAssetAtPath<T>(assetPath);
   var probe=viaResources??viaDatabase;
   return new JObject{["resourcesLoad"]=viaResources!=null,["assetDatabaseLoad"]=viaDatabase!=null,
    ["runtimeApproved"]=probe!=null&&approved(probe),["references"]=probe==null?"profile missing":refs(probe)};
  }
  static JObject GateState()=>new JObject{
   ["M7Hub"]=Lane<M7HubProfile>("M7Hub","Assets/_Project/Resources/M7Hub.asset",p=>p.runtimeApproved,
    p=>"floorAndWall="+(p.floorAndWall!=null)+" workbench="+(p.workbench!=null)+" plateShelf="+(p.plateShelf!=null)),
   ["M7UiSkin"]=Lane<M7UiSkinProfile>("M7UiSkin","Assets/_Project/Resources/M7UiSkin.asset",p=>p.runtimeApproved,
    p=>"paperPanel="+(p.paperPanel!=null)+" bronzeFrame="+(p.bronzeFrame!=null)),
   ["M7ReaderStage"]=Lane<M7ReaderStageProfile>("M7ReaderStage","Assets/_Project/Resources/M7ReaderStage.asset",p=>p.runtimeApproved,
    p=>"reader="+(p.reader!=null)+" recordSet="+(p.recordSet!=null)),
   ["M8ReviewNotes"]=Lane<M8ReviewNotesProfile>("M8ReviewNotes","Assets/_Project/Resources/M8ReviewNotes.asset",p=>p.runtimeApproved,
    p=>"cardPaper="+(p.cardPaper!=null))};
  // Cheap -nographics check that the saved profiles resolve their references (catches dangling prefab/material fileIDs).
  public static void Probe(){
   var hub=Resources.Load<M7HubProfile>("M7Hub");var ui=Resources.Load<M7UiSkinProfile>("M7UiSkin");var reader=Resources.Load<M7ReaderStageProfile>("M7ReaderStage");
   var readerPrefab=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_Project/Art/Candidates/m7-reader-r01/OpticalReader.prefab");
   var mat=AssetDatabase.LoadAssetAtPath<Material>("Assets/_Project/Art/Candidates/m7-hub-r01/MAT_M7_Hub_SaltConcrete.mat");
   Debug.Log("M7_PROBE hub="+(hub!=null)+" hub.floorAndWall="+(hub!=null&&hub.floorAndWall!=null)+" hub.workbench="+(hub!=null&&hub.workbench!=null)
    +" ui.paper="+(ui!=null&&ui.paperPanel!=null)+" reader.reader="+(reader!=null&&reader.reader!=null)+" reader.recordSet="+(reader!=null&&reader.recordSet!=null)
    +" prefabDirect="+(readerPrefab!=null)+" matDirect="+(mat!=null)+" matShader="+(mat!=null&&mat.shader!=null?mat.shader.name:"null"));
  }
  public static void Capture(){
   Directory.CreateDirectory(Output);
   var report=new JObject{["scope"]="RFC-CX-013 native proof shots (Unity render, candidates applied in memory; assets stay runtimeApproved:false)",["graphicsDevice"]=SystemInfo.graphicsDeviceType.ToString(),["width"]=1280,["height"]=800};
   var shots=new JArray();report["shots"]=shots;
   // Profiles are (re)loaded AFTER each scene switch: OpenScene/NewScene unload assets that are only reachable
   // through managed references, which turned the profiles' material/prefab fields into null in the first run.
   M7HubProfile hubProfile=null;M7UiSkinProfile uiProfile=null;M7ReaderStageProfile readerProfile=null;
   void Reload(){hubProfile=Resources.Load<M7HubProfile>("M7Hub");uiProfile=Resources.Load<M7UiSkinProfile>("M7UiSkin");readerProfile=Resources.Load<M7ReaderStageProfile>("M7ReaderStage");}
   Reload();
   report["assets"]=new JObject{["M7Hub"]=hubProfile!=null,["M7UiSkin"]=uiProfile!=null,["M7ReaderStage"]=readerProfile!=null,
    ["runtimeApproved"]=new JObject{["hub"]=hubProfile!=null&&hubProfile.runtimeApproved,["ui"]=uiProfile!=null&&uiProfile.runtimeApproved,["reader"]=readerProfile!=null&&readerProfile.runtimeApproved}};
   var rt=RenderTexture.GetTemporary(1280,800,24);
   try{
    // 1) Hub: committed look, then M7 shell applied through the runtime's own static path.
    EditorSceneManager.OpenScene("Assets/_Project/Scenes/hub.unity");Reload();
    var hub=SceneManager.GetActiveScene();var camera=Camera.main;
    var ui=BuildUi(null,out var uiCamera);camera.cullingMask&=~(1<<5);
    Frame(camera,54f);T0ViewportDiagnostics.CaptureFrame(camera,uiCamera,rt,Output+"/hub-before.png");shots.Add("hub-before.png");
    if(hubProfile!=null&&hubProfile.floorAndWall!=null){
     T0GameSession.ApplyM7HubShell(hub,hubProfile);
     T0ViewportDiagnostics.CaptureFrame(camera,uiCamera,rt,Output+"/hub-after-m7-shell.png");shots.Add("hub-after-m7-shell.png");
     if(uiProfile!=null&&uiProfile.paperPanel!=null){
      UnityEngine.Object.DestroyImmediate(ui.gameObject);UnityEngine.Object.DestroyImmediate(uiCamera.gameObject);
      ui=BuildUi(uiProfile,out uiCamera);
      T0ViewportDiagnostics.CaptureFrame(camera,uiCamera,rt,Output+"/hub-after-m7-shell-and-ui-skin.png");shots.Add("hub-after-m7-shell-and-ui-skin.png");
      // Same as the runtime: scene camera confined to the UI's Scene viewport hole (T0GameSession.ApplySceneViewport).
      Viewport(camera,ui.SceneViewport,54f);T0ViewportDiagnostics.CaptureFrame(camera,uiCamera,rt,Output+"/hub-after-m7-viewport.png");shots.Add("hub-after-m7-viewport.png");
     }
    }else report["hubSkipped"]="M7Hub.asset or its materials missing — run Tools/M7/Import all M7 candidates";
    // 2) Reader stage: empty scene, profile camera/lights, prefabs at profile offsets; rest pose then 150deg crank.
    if(readerProfile!=null&&readerProfile.reader!=null){
     EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);Reload();
     RenderSettings.ambientMode=UnityEngine.Rendering.AmbientMode.Flat;RenderSettings.ambientLight=new Color(.10f,.14f,.16f);
     var stageCamera=new GameObject("M7 capture camera",typeof(Camera)).GetComponent<Camera>();
     stageCamera.clearFlags=CameraClearFlags.SolidColor;stageCamera.backgroundColor=readerProfile.background;stageCamera.nearClipPlane=.05f;stageCamera.farClipPlane=50f;
     stageCamera.transform.position=readerProfile.cameraPosition;stageCamera.transform.LookAt(readerProfile.lookAt);
     var root=new GameObject(T0GameSession.M7ReaderVisualName);
     var reader=UnityEngine.Object.Instantiate(readerProfile.reader,root.transform);reader.transform.localPosition=readerProfile.readerLocalPosition;
     if(readerProfile.recordSet!=null){var records=UnityEngine.Object.Instantiate(readerProfile.recordSet,root.transform);records.transform.localPosition=readerProfile.recordSetLocalPosition;}
     Light(root.transform,"M7 reader lamp",readerProfile.lampColor,readerProfile.lampIntensity,readerProfile.lampRange,readerProfile.lampOffset);
     Light(root.transform,"M7 reader fill",readerProfile.fillColor,readerProfile.fillIntensity,readerProfile.fillRange,readerProfile.fillOffset);
     var stageUi=BuildUi(uiProfile!=null&&uiProfile.paperPanel!=null?uiProfile:null,out var stageUiCamera,"판독기 · Reader",ReaderScreenBody());stageCamera.cullingMask&=~(1<<5);
     Frame(stageCamera,readerProfile.horizontalFov);
     T0ViewportDiagnostics.CaptureFrame(stageCamera,stageUiCamera,rt,Output+"/reader-stage-rest.png");shots.Add("reader-stage-rest.png");
     Viewport(stageCamera,stageUi.SceneViewport,readerProfile.horizontalFov);T0ViewportDiagnostics.CaptureFrame(stageCamera,stageUiCamera,rt,Output+"/reader-stage-viewport.png");shots.Add("reader-stage-viewport.png");
     Frame(stageCamera,readerProfile.horizontalFov);
     var pivot=root.GetComponentsInChildren<Transform>(true).FirstOrDefault(t=>t.name==readerProfile.crankPivotName);
     var vfx=Resources.Load<TextAsset>("T0ReaderVfx");var stroke=vfx!=null?(float?)JObject.Parse(vfx.text)["crank_stroke_deg"]??150f:150f;
     if(pivot!=null){pivot.localRotation=Quaternion.AngleAxis(stroke,Vector3.right);T0ViewportDiagnostics.CaptureFrame(stageCamera,stageUiCamera,rt,Output+"/reader-stage-crank-"+Mathf.RoundToInt(stroke)+"deg.png");shots.Add("reader-stage-crank-"+Mathf.RoundToInt(stroke)+"deg.png");}
     report["crankPivotFound"]=pivot!=null;report["crankStrokeDeg"]=stroke;
     var bounds=root.GetComponentsInChildren<Renderer>().Select(r=>r.bounds).Aggregate((a,b)=>{a.Encapsulate(b);return a;});
     report["readerStageBounds"]=new JObject{["center"]=new JArray(bounds.center.x,bounds.center.y,bounds.center.z),["size"]=new JArray(bounds.size.x,bounds.size.y,bounds.size.z)};
     report["readerTriangles"]=root.GetComponentsInChildren<MeshFilter>().Sum(m=>m.sharedMesh!=null?m.sharedMesh.triangles.Length/3:0);
    }else report["readerSkipped"]="M7ReaderStage.asset or its reader prefab missing — run Tools/M7/Import all M7 candidates";
   }finally{RenderTexture.ReleaseTemporary(rt);}
   File.WriteAllText(Output+"/m7-diagnostics.json",report.ToString());
   Debug.Log("M7_DIAGNOSTICS "+Output+" shots="+shots.Count);
  }
  static void Frame(Camera camera,float horizontalFov){camera.rect=new Rect(0,0,1,1);camera.aspect=1280f/800;camera.fieldOfView=Camera.HorizontalToVerticalFieldOfView(horizontalFov,camera.aspect);}
  // Runtime parity: T0GameSession.ApplySceneViewport confines the scene camera to the UI's Scene viewport hole.
  static void Viewport(Camera camera,Rect viewport,float horizontalFov){
   camera.rect=viewport;camera.aspect=1280f*viewport.width/(800f*viewport.height);
   camera.fieldOfView=Camera.HorizontalToVerticalFieldOfView(horizontalFov,camera.aspect);
  }
  static void Light(Transform parent,string name,Color color,float intensity,float range,Vector3 offset){
   var light=new GameObject(name,typeof(Light)).GetComponent<Light>();light.transform.SetParent(parent,false);light.transform.localPosition=offset;
   light.type=LightType.Point;light.color=color;light.intensity=intensity;light.range=range;light.shadows=LightShadows.None;
  }
  static string ReaderScreenBody()=>"관찰: 흔적을 읽는다.\n시험: 크랭크를 전진→멈춤→원위치. 표면과 각인은 그대로 둔다.\n기록: 서로 다른 자료를 대조하고 모르는 부분은 빈칸으로 남긴다.";
  // Same T0Interface the game uses, rendered by a UI-only camera on layer 5 and composited by CaptureFrame.
  static T0Interface BuildUi(M7UiSkinProfile skin,out Camera uiCamera,string title="조수기록국 · 당직 인수",string body="서랍칸의 판 #0을 조사합니다."){
   uiCamera=new GameObject("M7 diagnostic UI camera",typeof(Camera)).GetComponent<Camera>();uiCamera.clearFlags=CameraClearFlags.SolidColor;uiCamera.backgroundColor=Color.clear;uiCamera.cullingMask=1<<5;uiCamera.nearClipPlane=.01f;uiCamera.farClipPlane=10;
   var host=new GameObject("M7 diagnostic UI");var ui=host.AddComponent<T0Interface>();ui.Initialize();
   var model=new GameScreen{Title=title,Subtitle="21:00 · 마지막 당직 — native diagnostic",Body=body,Footer="Tab 초점 · Enter 선택 · Esc 뒤로",Skin=skin,Status="검토 카드 미제출"};
   foreach(var (id,label) in new[]{("hub-view-desk","작업대"),("hub-view-drawer","서랍"),("hub-view-reader","판독기"),("hub-view-window","창")})model.Navigation.Add(new ViewAction{Id=id,Label=label});
   model.Actions.Add(new ViewAction{Id="observe",Label="관찰: 흔적을 읽는다"});model.Actions.Add(new ViewAction{Id="trial",Label="시험: 비교하고 되돌린다"});model.Actions.Add(new ViewAction{Id="record",Label="기록: 자료를 대조한다",Enabled=false});
   foreach(var (id,label) in new[]{("action.reader","판독기"),("action.circuit","벽 회로 지도"),("settings","설정")})model.Toolbar.Add(new ViewAction{Id=id,Label=label});
   ui.Render(model);
   foreach(var t in host.GetComponentsInChildren<Transform>(true))t.gameObject.layer=5;
   var canvas=host.GetComponentInChildren<Canvas>();canvas.renderMode=RenderMode.ScreenSpaceCamera;canvas.worldCamera=uiCamera;canvas.planeDistance=.3f;
   Canvas.ForceUpdateCanvases();return ui;
  }
 }
}
#endif
