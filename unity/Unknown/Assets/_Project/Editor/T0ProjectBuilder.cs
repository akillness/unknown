#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using Tide.App;
using Tide.Data;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
namespace Tide.EditorTools {
 public static class T0ProjectBuilder {
  const string Root="Assets/_Project/";
  public static void Prepare(){
   Directory.CreateDirectory(Root+"Resources");Directory.CreateDirectory(Root+"Scenes");Directory.CreateDirectory(Root+"Art/Candidates");Directory.CreateDirectory(Root+"Rendering");
   var settings=new SerializedObject(Unsupported.GetSerializedAssetInterfaceSingleton("PlayerSettings"));settings.FindProperty("activeInputHandler").intValue=2;settings.ApplyModifiedPropertiesWithoutUndo();
   PlayerSettings.companyName="TideRegistry";PlayerSettings.productName="Unknown T0";PlayerSettings.defaultScreenWidth=1600;PlayerSettings.defaultScreenHeight=900;PlayerSettings.fullScreenMode=FullScreenMode.Windowed;
   var renderer=AssetDatabase.LoadAssetAtPath<UniversalRendererData>(Root+"Rendering/T0Renderer.asset");if(renderer==null){renderer=ScriptableObject.CreateInstance<UniversalRendererData>();AssetDatabase.CreateAsset(renderer,Root+"Rendering/T0Renderer.asset");}
   var pipeline=AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(Root+"Rendering/T0URP.asset");if(pipeline==null){pipeline=UniversalRenderPipelineAsset.Create(renderer);AssetDatabase.CreateAsset(pipeline,Root+"Rendering/T0URP.asset");}
   pipeline.supportsHDR=false;pipeline.msaaSampleCount=1;GraphicsSettings.defaultRenderPipeline=pipeline;QualitySettings.renderPipeline=pipeline;EditorUtility.SetDirty(pipeline);
   AssetDatabase.Refresh();T0AssetImporter.Import();
   var config=AssetDatabase.LoadAssetAtPath<T0RuntimeConfig>(Root+"Resources/T0Runtime.asset");if(config==null){config=ScriptableObject.CreateInstance<T0RuntimeConfig>();AssetDatabase.CreateAsset(config,Root+"Resources/T0Runtime.asset");}
   config.catalog=AssetDatabase.LoadAssetAtPath<T0CatalogAsset>(T0AssetImporter.CatalogPath);
   config.records=Text("Data/Tables/records.json");config.zones=Text("Data/Tables/zones.json");config.tools=Text("Data/Tables/tools.json");config.hints=Text("Data/Tables/hints.json");config.bindings=Text("Resources/WatchBindings.json");config.strings=Text("Resources/T0Strings.json");config.savePolicy=Text("Resources/SavePolicy.json");EditorUtility.SetDirty(config);
   var repo=Path.GetFullPath(Path.Combine(Application.dataPath,"../../.."));
   CopyCandidate(repo,"assets/generated/3d/hub-greybox.fbx","hub-greybox.fbx");
   var drawerDir=Path.Combine(repo,"assets/generated/3d/hub-view-drawer-r01");var drawer=Directory.Exists(drawerDir)?Directory.GetFiles(drawerDir,"*.fbx").FirstOrDefault():null;
   if(drawer!=null)File.Copy(drawer,Root+"Art/Candidates/drawer-r01.fbx",true);
   var audio=Path.Combine(repo,"assets/generated/audio/higgsfield-stamp-r01/stamp-confirm.wav");if(File.Exists(audio))File.Copy(audio,Root+"Art/Candidates/stamp-confirm.wav",true);
   AssetDatabase.Refresh();var audit=new JObject();
   foreach(var fbx in new[]{"hub-greybox.fbx","drawer-r01.fbx"}){
    var path=Root+"Art/Candidates/"+fbx;var asset=AssetDatabase.LoadAssetAtPath<GameObject>(path);if(asset==null)continue;
    var instance=UnityEngine.Object.Instantiate(asset);var meshes=instance.GetComponentsInChildren<MeshFilter>();var renderers=instance.GetComponentsInChildren<Renderer>();var bounds=new Bounds();if(renderers.Length>0){bounds=renderers[0].bounds;foreach(var r in renderers.Skip(1))bounds.Encapsulate(r.bounds);}
    audit[fbx]=new JObject{["meshCount"]=meshes.Length,["triangles"]=meshes.Sum(m=>m.sharedMesh.triangles.Length/3),["size"]=new JArray(bounds.size.x,bounds.size.y,bounds.size.z),["center"]=new JArray(bounds.center.x,bounds.center.y,bounds.center.z),["missingMesh"]=meshes.Any(m=>m.sharedMesh==null),["runtimeEligible"]=false};
    UnityEngine.Object.DestroyImmediate(instance);
   }
   Directory.CreateDirectory("Builds");File.WriteAllText("Builds/t0-import-audit.json",audit.ToString());Debug.Log("T0_IMPORT_AUDIT "+audit.ToString(Newtonsoft.Json.Formatting.None));
   var boot=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);new GameObject("T0 Bootstrap",typeof(T0Entry));EditorSceneManager.SaveScene(boot,Root+"Scenes/boot.unity");
   var ui=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);new GameObject("UI Scene Root");EditorSceneManager.SaveScene(ui,Root+"Scenes/ui-root.unity");
   var hub=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
   var camera=new GameObject("Watch camera",typeof(Camera),typeof(AudioListener));camera.tag="MainCamera";camera.transform.position=new Vector3(0,1.55f,-2.164f);camera.transform.LookAt(new Vector3(0,.9f,1.2f));camera.GetComponent<Camera>().fieldOfView=Camera.HorizontalToVerticalFieldOfView(54,16f/9f);camera.GetComponent<Camera>().backgroundColor=new Color(.055f,.122f,.149f);
   var light=new GameObject("Watch room lamp",typeof(Light));light.GetComponent<Light>().type=LightType.Directional;light.GetComponent<Light>().intensity=1.2f;light.transform.rotation=Quaternion.Euler(48,-30,0);RenderSettings.ambientLight=new Color(.35f,.42f,.43f);
   // Candidate imports remain isolated until the director's import/fit audit promotes them.
   var candidates=new GameObject("Imported candidates — pending review");
   var imported=AssetDatabase.LoadAssetAtPath<GameObject>(Root+"Art/Candidates/hub-greybox.fbx");if(imported!=null){var obj=(GameObject)PrefabUtility.InstantiatePrefab(imported);obj.transform.SetParent(candidates.transform,false);}
   candidates.SetActive(false);
   MakeBox("Authored room floor",new Vector3(6,.12f,8),new Vector3(0,-.06f,0),new Color(.212f,.337f,.361f));
   MakeBox("Authored back wall",new Vector3(6,2.6f,.12f),new Vector3(0,1.3f,3.94f),new Color(.212f,.337f,.361f));
   MakeApprovedWorkbench();
   MakeBox("Authored plate shelf",new Vector3(.36f,1.15f,1.8f),new Vector3(2.55f,.575f,.75f),new Color(.212f,.337f,.361f));
   EditorSceneManager.SaveScene(hub,Root+"Scenes/hub.unity");
   EditorBuildSettings.scenes=new[]{"boot","ui-root","hub"}.Select(n=>new EditorBuildSettingsScene(Root+"Scenes/"+n+".unity",true)).ToArray();
   AssetDatabase.SaveAssets();Debug.Log("T0_M2_PREPARED: Both input, URP, boot/ui-root/hub, external data");
  }
  static TextAsset Text(string path)=>AssetDatabase.LoadAssetAtPath<TextAsset>(Root+path);
  static void CopyCandidate(string repo,string source,string target){var file=Path.Combine(repo,source);if(File.Exists(file))File.Copy(file,Root+"Art/Candidates/"+target,true);}
  static void MakeBox(string name,Vector3 size,Vector3 pos,Color color){var go=GameObject.CreatePrimitive(PrimitiveType.Cube);go.name=name;go.transform.localScale=size;go.transform.position=pos;var material=new Material(Shader.Find("Universal Render Pipeline/Lit"));material.color=color;var path=Root+"Rendering/"+name.Replace(" ","-")+".mat";if(AssetDatabase.LoadAssetAtPath<Material>(path)!=null)AssetDatabase.DeleteAsset(path);AssetDatabase.CreateAsset(material,path);go.GetComponent<Renderer>().sharedMaterial=material;}
  // T0-only approval: r03 diagnostic prefab already retains FBX axis conversion plus yaw 180.
  static void MakeApprovedWorkbench(){
   var prefab=AssetDatabase.LoadAssetAtPath<GameObject>(Root+"Art/Candidates/drawer-r03/DrawerDiagnostic.prefab");
   if(prefab==null)throw new InvalidOperationException("Approved r03 drawer prefab is missing");
   var drawer=(GameObject)PrefabUtility.InstantiatePrefab(prefab);drawer.name="T0 approved r03 drawer";drawer.transform.position=new Vector3(0,.32f,1.1f);
   var renderers=drawer.GetComponentsInChildren<Renderer>();var bounds=renderers[0].bounds;foreach(var renderer in renderers.Skip(1))bounds.Encapsulate(renderer.bounds);
   float lo=bounds.min.x,hi=bounds.max.x,low=bounds.min.y,high=bounds.max.y,back=bounds.max.z;var color=new Color(.21f,.337f,.36f);
   MakeBox("Workbench left jamb",new Vector3(lo+1.2f,.9f,.9f),new Vector3((-1.2f+lo)/2,.45f,1.2f),color);
   MakeBox("Workbench right jamb",new Vector3(1.2f-hi,.9f,.9f),new Vector3((hi+1.2f)/2,.45f,1.2f),color);
   MakeBox("Workbench above drawer",new Vector3(hi-lo,.9f-high,.9f),new Vector3(0,(high+.9f)/2,1.2f),color);
   MakeBox("Workbench below drawer",new Vector3(hi-lo,low,.9f),new Vector3(0,low/2,1.2f),color);
   MakeBox("Workbench behind drawer",new Vector3(hi-lo,high-low,1.65f-back),new Vector3(0,(low+high)/2,(back+1.65f)/2),color);
  }
  public static void BuildMac()=>BuildMacAt("Builds/T0-mac/Unknown.app");
  public static void BuildMacFramingFix()=>BuildMacAt("Builds/T0-mac-framing/Unknown.app");
  static void BuildMacAt(string output){var report=BuildPipeline.BuildPlayer(EditorBuildSettings.scenes.Where(s=>s.enabled).Select(s=>s.path).ToArray(),output,BuildTarget.StandaloneOSX,BuildOptions.Development);Debug.Log("T0_MAC_BUILD "+report.summary.result+" bytes="+report.summary.totalSize);if(report.summary.result!=BuildResult.Succeeded)throw new InvalidOperationException("T0 player build failed");}
 }
}
#endif
