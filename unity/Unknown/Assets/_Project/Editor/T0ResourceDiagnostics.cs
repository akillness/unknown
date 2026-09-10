#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
namespace Tide.EditorTools {
 public static class T0ResourceDiagnostics {
  const string Destination="Assets/_Project/Art/Candidates/drawer-r03/";
  public static void ImportAndCapture(){
   var repo=Path.GetFullPath(Path.Combine(Application.dataPath,"../../.."));var source=Path.Combine(repo,"assets/generated/3d/hub-view-drawer-r03");Directory.CreateDirectory(Destination+"textures");
   File.Copy(Path.Combine(source,"SM_Hub_Workbench_Drawer.fbx"),Destination+"Drawer.fbx",true);
   foreach(var path in Directory.GetFiles(Path.Combine(source,"textures"),"*.png"))File.Copy(path,Destination+"textures/"+Path.GetFileName(path),true);
   AssetDatabase.Refresh();var audit=new JObject{["revision"]="r03",["runtimeEligible"]=false,["colorSpace"]=QualitySettings.activeColorSpace.ToString(),["graphicsDevice"]=SystemInfo.graphicsDeviceType.ToString(),["textures"]=new JArray()};
   foreach(var path in Directory.GetFiles(Destination+"textures","*.png")){
    var importer=(TextureImporter)AssetImporter.GetAtPath(path);bool srgb=path.Contains("BaseColor");importer.sRGBTexture=srgb;importer.maxTextureSize=1024;importer.textureCompression=TextureImporterCompression.Uncompressed;importer.SaveAndReimport();var texture=AssetDatabase.LoadAssetAtPath<Texture2D>(path);
    ((JArray)audit["textures"]).Add(new JObject{["file"]=Path.GetFileName(path),["width"]=texture.width,["height"]=texture.height,["sRGB"]=importer.sRGBTexture});
   }
   byte[] bytes=File.ReadAllBytes(Path.Combine(source,"SM_Hub_Workbench_Drawer.glb"));var gltf=JObject.Parse(System.Text.Encoding.UTF8.GetString(bytes,20,BitConverter.ToInt32(bytes,12)));
   var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);var model=AssetDatabase.LoadAssetAtPath<GameObject>(Destination+"Drawer.fbx");var drawer=(GameObject)PrefabUtility.InstantiatePrefab(model);drawer.transform.rotation=Quaternion.Euler(0,180,0)*drawer.transform.rotation;audit["unityAdapterYawDeg"]=180;var parts=new JArray();
   foreach(var renderer in drawer.GetComponentsInChildren<Renderer>()){
    string part=renderer.name.Contains("Tray")?"Tray":"Casing";var material=AssetDatabase.LoadAssetAtPath<Material>(Destination+part+".mat");if(material==null){material=new Material(Shader.Find("Tide/Candidate Roughness"));AssetDatabase.CreateAsset(material,Destination+part+".mat");}material.shader=Shader.Find("Tide/Candidate Roughness");
    var baseMap=AssetDatabase.LoadAssetAtPath<Texture2D>(Destination+"textures/SM_Hub_Workbench_Drawer_"+part+"_BaseColor.png");var roughness=AssetDatabase.LoadAssetAtPath<Texture2D>(Destination+"textures/SM_Hub_Workbench_Drawer_"+part+"_Roughness.png");
    material.SetTexture("_BaseMap",baseMap);material.SetTexture("_RoughnessMap",roughness);float metallic=(float)gltf["materials"].First(m=>((string)m["name"]).Contains(part))["pbrMetallicRoughness"]["metallicFactor"];material.SetFloat("_Metallic",metallic);renderer.sharedMaterial=material;EditorUtility.SetDirty(material);
    parts.Add(new JObject{["renderer"]=renderer.name,["center"]=new JArray(renderer.bounds.center.x,renderer.bounds.center.y,renderer.bounds.center.z),["size"]=new JArray(renderer.bounds.size.x,renderer.bounds.size.y,renderer.bounds.size.z),["baseMapBound"]=baseMap!=null,["roughnessBound"]=roughness!=null,["metallic"]=metallic,["shaderSupported"]=material.shader.isSupported});
   }
   audit["materials"]=parts;var meshes=drawer.GetComponentsInChildren<MeshFilter>();audit["meshCount"]=meshes.Length;audit["triangles"]=meshes.Sum(m=>m.sharedMesh.triangles.Length/3);PrefabUtility.SaveAsPrefabAsset(drawer,Destination+"DrawerDiagnostic.prefab");AssetDatabase.SaveAssets();
   var light=new GameObject("Diagnostic key",typeof(Light)).GetComponent<Light>();light.type=LightType.Directional;light.intensity=1.6f;light.transform.rotation=Quaternion.Euler(45,-25,0);RenderSettings.ambientMode=AmbientMode.Flat;RenderSettings.ambientLight=new Color(.38f,.43f,.44f);
   var camera=new GameObject("Diagnostic camera",typeof(Camera)).GetComponent<Camera>();camera.clearFlags=CameraClearFlags.SolidColor;camera.backgroundColor=new Color(.08f,.13f,.15f);camera.nearClipPlane=.05f;camera.farClipPlane=30;camera.fieldOfView=42;camera.transform.position=new Vector3(.8f,.6f,-1.5f);camera.transform.LookAt(new Vector3(0,.13f,.02f));
   string output="Builds/t0-diagnostics";Directory.CreateDirectory(output);
   if(SystemInfo.graphicsDeviceType==GraphicsDeviceType.Null)throw new InvalidOperationException("Diagnostic capture requires a graphics device");
   Capture(camera,output+"/drawer-r03-standalone.png");
   drawer.transform.position=new Vector3(0,.32f,1.1f);var solid=Box("Original solid workbench",new Vector3(0,.45f,1.2f),new Vector3(2.4f,.9f,.9f));camera.fieldOfView=Camera.HorizontalToVerticalFieldOfView(54,4f/3f);camera.transform.position=new Vector3(0,1.55f,-.482f);camera.transform.LookAt(new Vector3(0,.45f,1.2f));Capture(camera,output+"/drawer-r03-solid-interference.png");
   solid.SetActive(false);var renderers=drawer.GetComponentsInChildren<Renderer>();var bounds=renderers[0].bounds;foreach(var r in renderers.Skip(1))bounds.Encapsulate(r.bounds);float lo=bounds.min.x,hi=bounds.max.x,low=bounds.min.y,high=bounds.max.y,back=bounds.max.z;
   // Diagnostic geometry subtracts the imported AABB from the authored solid workbench.
   Box("Left body",new Vector3((-1.2f+lo)/2,.45f,1.2f),new Vector3(lo+1.2f,.9f,.9f));Box("Right body",new Vector3((hi+1.2f)/2,.45f,1.2f),new Vector3(1.2f-hi,.9f,.9f));Box("Above drawer",new Vector3(0,(high+.9f)/2,1.2f),new Vector3(hi-lo,.9f-high,.9f));Box("Below drawer",new Vector3(0,low/2,1.2f),new Vector3(hi-lo,low,.9f));Box("Behind drawer",new Vector3(0,(low+high)/2,(back+1.65f)/2),new Vector3(hi-lo,high-low,1.65f-back));
   Capture(camera,output+"/drawer-r03-front-adaptation.png");audit["fitAdaptation"]="Diagnostic AABB subtraction from authored workbench; no runtime promotion";audit["drawerPosition"]=new JArray(0,.32,1.1);File.WriteAllText(output+"/import-material-fit-audit.json",audit.ToString());Debug.Log("T0_R03_DIAGNOSTICS "+audit.ToString(Newtonsoft.Json.Formatting.None));
  }
  static GameObject Box(string name,Vector3 position,Vector3 size){var obj=GameObject.CreatePrimitive(PrimitiveType.Cube);obj.name=name;obj.transform.position=position;obj.transform.localScale=size;var mat=new Material(Shader.Find("Universal Render Pipeline/Lit"));mat.color=new Color(.21f,.337f,.36f);obj.GetComponent<Renderer>().sharedMaterial=mat;return obj;}
  static void Capture(Camera camera,string path){var rt=RenderTexture.GetTemporary(1280,960,24);var previous=RenderTexture.active;RenderPipeline.SubmitRenderRequest(camera,new UniversalRenderPipeline.SingleCameraRequest{destination=rt});RenderTexture.active=rt;var image=new Texture2D(1280,960,TextureFormat.RGB24,false);image.ReadPixels(new Rect(0,0,1280,960),0,0);image.Apply();File.WriteAllBytes(path,image.EncodeToPNG());RenderTexture.active=previous;UnityEngine.Object.DestroyImmediate(image);RenderTexture.ReleaseTemporary(rt);}
 }
}
#endif
