#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using Tide.App;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Tide.EditorTools
{
    public static class C1ProjectBuilder
    {
        const string Destination="Assets/_Project/Art/Candidates/c1-patrol-panel-r01/";
        static string Repo=>Path.GetFullPath(Path.Combine(Application.dataPath,"../../.."));
        public static void ImportAndCapture()
        {
            var source=Path.Combine(Repo,"assets/generated/3d/c1-patrol-panel-r01");Directory.CreateDirectory(Destination+"textures");
            File.Copy(Path.Combine(source,"SM_C1_Gate_System_Panel.fbx"),Destination+"Panel.fbx",true);
            foreach(var path in Directory.GetFiles(Path.Combine(source,"textures"),"*.png"))File.Copy(path,Destination+"textures/"+Path.GetFileName(path),true);
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            foreach(var path in Directory.GetFiles(Destination+"textures","*.png"))
            {
                var importer=(TextureImporter)AssetImporter.GetAtPath(path);importer.sRGBTexture=path.Contains("BaseColor");
                importer.textureCompression=TextureImporterCompression.Uncompressed;importer.SaveAndReimport();
            }
            var bytes=File.ReadAllBytes(Path.Combine(source,"SM_C1_Gate_System_Panel.glb"));
            var gltf=JObject.Parse(System.Text.Encoding.UTF8.GetString(bytes,20,BitConverter.ToInt32(bytes,12)));
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
            var panel=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(Destination+"Panel.fbx"));
            panel.name="C1 static system panel diagnostic";var importedRotation=panel.transform.rotation;panel.transform.rotation=Quaternion.Euler(0,180,0)*importedRotation;
            var audit=new JObject{["runtimeEligible"]=false,["sourceClaim"]="static authored panel; no live readings or branch animations",["materials"]=new JArray()};
            foreach(var renderer in panel.GetComponentsInChildren<Renderer>())
            {
                var materialName=renderer.name.Replace("SM_C1_Gate_System_Panel_","MAT_C1_Panel_");
                var authored=gltf["materials"].First(m=>(string)m["name"]==materialName)["pbrMetallicRoughness"];
                var baseMap=AssetDatabase.LoadAssetAtPath<Texture2D>(Destination+"textures/"+materialName+"_BaseColor.png");
                var rough=AssetDatabase.LoadAssetAtPath<Texture2D>(Destination+"textures/"+materialName+"_Roughness.png");
                var path=Destination+materialName+".mat";var material=AssetDatabase.LoadAssetAtPath<Material>(path);
                if(material==null){material=new Material(Shader.Find(baseMap!=null?"Tide/Candidate Roughness":"Universal Render Pipeline/Lit"));AssetDatabase.CreateAsset(material,path);}
                // Convert glTF constant factors into exact 1x1 material inputs for the same verified URP shader as textured parts.
                if(baseMap==null){var c=authored["baseColorFactor"];baseMap=FactorTexture(materialName+"-base",new Color((float)c[0],(float)c[1],(float)c[2],(float)c[3]).gamma,false);var value=(float?)authored["roughnessFactor"]??1f;rough=FactorTexture(materialName+"-roughness",new Color(value,value,value,1),true);}
                material.shader=Shader.Find("Tide/Candidate Roughness");material.SetTexture("_BaseMap",baseMap);material.SetTexture("_RoughnessMap",rough);
                material.SetFloat("_Metallic",(float?)authored["metallicFactor"]??0);renderer.sharedMaterial=material;EditorUtility.SetDirty(material);
                ((JArray)audit["materials"]).Add(new JObject{["name"]=materialName,["baseMapBound"]=baseMap!=null,["roughnessBound"]=rough!=null,["shaderSupported"]=material.shader.isSupported});
            }
            var meshes=panel.GetComponentsInChildren<MeshFilter>();audit["meshCount"]=meshes.Length;audit["triangles"]=meshes.Sum(m=>m.sharedMesh.triangles.Length/3);
            var bounds=panel.GetComponentsInChildren<Renderer>().Select(r=>r.bounds).Aggregate((a,b)=>{a.Encapsulate(b);return a;});
            var prefab=PrefabUtility.SaveAsPrefabAsset(panel,Destination+"PanelDiagnostic.prefab");
            var view=AssetDatabase.LoadAssetAtPath<C1ViewSettings>("Assets/_Project/Resources/C1View.asset");
            if(view==null){view=ScriptableObject.CreateInstance<C1ViewSettings>();AssetDatabase.CreateAsset(view,"Assets/_Project/Resources/C1View.asset");}
            view.panel=prefab;view.horizontalFov=54;view.lookAt=bounds.center;view.background=new Color(.04f,.085f,.1f);
            view.cameraPosition=bounds.center+new Vector3(bounds.size.x*.08f,bounds.size.y*.10f,-Mathf.Max(bounds.size.x,bounds.size.y*(1600f*.43f/(900f*.35f)))/(2*Mathf.Tan(view.horizontalFov*.5f*Mathf.Deg2Rad))*1.15f);
            view.runtimeApproved=false;EditorUtility.SetDirty(view);AssetDatabase.SaveAssets();
            var camera=new GameObject("C1 diagnostic camera",typeof(Camera)).GetComponent<Camera>();camera.transform.position=view.cameraPosition;camera.transform.LookAt(view.lookAt);
            camera.backgroundColor=view.background;camera.clearFlags=CameraClearFlags.SolidColor;camera.fieldOfView=Camera.HorizontalToVerticalFieldOfView(view.horizontalFov,4f/3f);
            RenderSettings.ambientMode=AmbientMode.Flat;RenderSettings.ambientLight=new Color(.35f,.4f,.43f);
            var light=new GameObject("C1 diagnostic key",typeof(Light)).GetComponent<Light>();light.type=LightType.Directional;light.intensity=1.7f;light.transform.rotation=Quaternion.Euler(40,-30,0);
            var output=Path.Combine(Repo,"_workspace/current/systems/tech-verification/c1-m3/panel");Directory.CreateDirectory(output);
            audit["boundsCenter"]=JArray.FromObject(new[]{bounds.center.x,bounds.center.y,bounds.center.z});audit["boundsSize"]=JArray.FromObject(new[]{bounds.size.x,bounds.size.y,bounds.size.z});
            Capture(camera,output+"/panel-front.png");panel.transform.rotation=importedRotation;Capture(camera,output+"/panel-alternate.png");
            File.WriteAllText(output+"/import-audit.json",audit.ToString());UnityEngine.Object.DestroyImmediate(panel);
            Debug.Log("C1_PANEL_NATIVE_DIAGNOSTIC "+audit);
        }
        public static void ApprovePanel()
        {
            var view=AssetDatabase.LoadAssetAtPath<C1ViewSettings>("Assets/_Project/Resources/C1View.asset");
            if(view==null||view.panel==null)throw new InvalidOperationException("C1 panel diagnostic missing");
            view.runtimeApproved=true;EditorUtility.SetDirty(view);AssetDatabase.SaveAssets();
        }
        public static void ApprovePanelAndBuildMac(){ApprovePanel();BuildMac();}
        public static void BuildMac()
        {
            var report=BuildPipeline.BuildPlayer(EditorBuildSettings.scenes.Where(s=>s.enabled).Select(s=>s.path).ToArray(),
                "Builds/C1-M3-mac/Unknown.app",BuildTarget.StandaloneOSX,BuildOptions.Development);
            Debug.Log("C1_MAC_BUILD "+report.summary.result+" bytes="+report.summary.totalSize);
            if(report.summary.result!=BuildResult.Succeeded)throw new InvalidOperationException("C1 build failed");
        }
        static Texture2D FactorTexture(string name,Color value,bool linear)
        {
            var path=Destination+name+".asset";var texture=AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if(texture==null){texture=new Texture2D(1,1,TextureFormat.RGBA32,false,linear);AssetDatabase.CreateAsset(texture,path);}
            texture.SetPixel(0,0,value);texture.Apply();EditorUtility.SetDirty(texture);return texture;
        }
        static void Capture(Camera camera,string path)
        {
            if(SystemInfo.graphicsDeviceType==GraphicsDeviceType.Null)throw new InvalidOperationException("Native diagnostic requires graphics");
            var rt=RenderTexture.GetTemporary(1280,960,24);var previous=RenderTexture.active;
            RenderPipeline.SubmitRenderRequest(camera,new UniversalRenderPipeline.SingleCameraRequest{destination=rt});RenderTexture.active=rt;
            var texture=new Texture2D(1280,960,TextureFormat.RGB24,false);texture.ReadPixels(new Rect(0,0,1280,960),0,0);texture.Apply();File.WriteAllBytes(path,texture.EncodeToPNG());
            RenderTexture.active=previous;UnityEngine.Object.DestroyImmediate(texture);RenderTexture.ReleaseTemporary(rt);
        }
    }
}
#endif
