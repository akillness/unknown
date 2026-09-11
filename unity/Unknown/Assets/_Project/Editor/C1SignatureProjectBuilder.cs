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
    public static class C1SignatureProjectBuilder
    {
        const string Destination="Assets/_Project/Art/Candidates/c1-signature-reader-r02/";
        const string ViewPath="Assets/_Project/Resources/C1SignatureView.asset";
        static string Repo=>Path.GetFullPath(Path.Combine(Application.dataPath,"../../.."));
        static string Output=>Path.Combine(Repo,"_workspace/current/systems/tech-verification/c1-m4/reader");
        public static void ImportAndCapture()
        {
            var source=Path.Combine(Repo,"assets/generated/3d/c1-signature-reader-r02");Directory.CreateDirectory(Destination+"textures");
            File.Copy(Path.Combine(source,"SM_C1_Signature_Reader.fbx"),Destination+"Reader.fbx",true);
            foreach(var file in Directory.GetFiles(Path.Combine(source,"textures"),"*.png"))File.Copy(file,Destination+"textures/"+Path.GetFileName(file),true);
            File.Copy(Path.Combine(Repo,"assets/generated/2d/texture/c1-signature-paper-r01/paper.png"),Destination+"SignaturePaper.png",true);
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            foreach(var file in Directory.GetFiles(Destination,"*.png",SearchOption.AllDirectories))
            {
                var importer=(TextureImporter)AssetImporter.GetAtPath(file);importer.sRGBTexture=!file.Contains("Roughness");importer.textureCompression=TextureImporterCompression.Uncompressed;importer.SaveAndReimport();
            }
            var bytes=File.ReadAllBytes(Path.Combine(source,"SM_C1_Signature_Reader.glb"));var gltf=JObject.Parse(System.Text.Encoding.UTF8.GetString(bytes,20,BitConverter.ToInt32(bytes,12)));
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
            var reader=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(Destination+"Reader.fbx"));
            var originalRotation=reader.transform.rotation;reader.transform.rotation=originalRotation;reader.name="C1 signature reader diagnostic";
            var audit=new JObject{["runtimeEligible"]=false,["sourceClaim"]="Static reader housing plus independently rendered document state",["meshes"]=new JArray()};
            foreach(var renderer in reader.GetComponentsInChildren<Renderer>())
            {
                var name=renderer.name.Replace("SM_C1_Signature_Reader_","MAT_C1_Reader_");var gltfMaterial=gltf["materials"].Single(m=>(string)m["name"]==name);var pbr=gltfMaterial["pbrMetallicRoughness"];
                var factor=pbr?["baseColorFactor"]?.Values<float>().ToArray()??new[]{1f,1f,1f,1f};float metallic=(float?)pbr?["metallicFactor"]??1f,roughness=(float?)pbr?["roughnessFactor"]??1f;
                var material=new Material(Shader.Find("Universal Render Pipeline/Lit"));var materialPath=Destination+name+".mat";
                var baseMap=AssetDatabase.LoadAssetAtPath<Texture2D>(Destination+"textures/"+name+"_BaseColor.png");var roughMap=AssetDatabase.LoadAssetAtPath<Texture2D>(Destination+"textures/"+name+"_Roughness.png");
                material.SetTexture("_BaseMap",baseMap);material.SetColor("_BaseColor",new Color(factor[0],factor[1],factor[2],factor[3]));material.SetFloat("_Metallic",metallic);material.SetFloat("_Smoothness",1-roughness);
                if(roughMap!=null)
                {
                    var importer=(TextureImporter)AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(roughMap));importer.isReadable=true;importer.SaveAndReimport();
                    roughMap=AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GetAssetPath(roughMap));var map=new Texture2D(roughMap.width,roughMap.height,TextureFormat.RGBA32,false,true);
                    var pixels=roughMap.GetPixels();for(int i=0;i<pixels.Length;i++)pixels[i]=new Color(metallic,0,0,1-pixels[i].r*roughness);map.SetPixels(pixels);map.Apply();
                    var mapPath=Destination+name+"_MetallicSmoothness.asset";if(AssetDatabase.LoadAssetAtPath<Texture2D>(mapPath)==null)AssetDatabase.CreateAsset(map,mapPath);else {EditorUtility.CopySerialized(map,AssetDatabase.LoadAssetAtPath<Texture2D>(mapPath));UnityEngine.Object.DestroyImmediate(map);}
                    material.SetTexture("_MetallicGlossMap",AssetDatabase.LoadAssetAtPath<Texture2D>(mapPath));material.EnableKeyword("_METALLICSPECGLOSSMAP");material.SetFloat("_Smoothness",1);
                }
                if(AssetDatabase.LoadAssetAtPath<Material>(materialPath)==null)AssetDatabase.CreateAsset(material,materialPath);else {EditorUtility.CopySerialized(material,AssetDatabase.LoadAssetAtPath<Material>(materialPath));UnityEngine.Object.DestroyImmediate(material);}
                renderer.sharedMaterial=AssetDatabase.LoadAssetAtPath<Material>(materialPath);
                var mesh=renderer.GetComponent<MeshFilter>().sharedMesh;((JArray)audit["meshes"]).Add(new JObject{["name"]=renderer.name,["triangles"]=mesh.triangles.Length/3,["material"]=name,["metallicFactor"]=metallic,["roughnessFactor"]=roughness});
            }
            var bounds=reader.GetComponentsInChildren<Renderer>()[0].bounds;foreach(var item in reader.GetComponentsInChildren<Renderer>())bounds.Encapsulate(item.bounds);
            var prefab=PrefabUtility.SaveAsPrefabAsset(reader,Destination+"SignatureReader.prefab");var view=AssetDatabase.LoadAssetAtPath<C1SignatureViewSettings>(ViewPath);
            if(view==null){view=ScriptableObject.CreateInstance<C1SignatureViewSettings>();AssetDatabase.CreateAsset(view,ViewPath);}
            view.reader=prefab;view.paper=AssetDatabase.LoadAssetAtPath<Texture2D>(Destination+"SignaturePaper.png");view.horizontalFov=54;view.lookAt=bounds.center;view.background=new Color(.035f,.065f,.075f);
            view.cameraPosition=bounds.center+new Vector3(.68f,.945f,1.10f).normalized*Mathf.Max(bounds.size.x,bounds.size.y*(1600f*.43f/(900f*.35f)))/(2*Mathf.Tan(view.horizontalFov*.5f*Mathf.Deg2Rad))*1.7f;
            view.runtimeApproved=false;EditorUtility.SetDirty(view);AssetDatabase.SaveAssets();
            var camera=new GameObject("C1 signature camera",typeof(Camera)).GetComponent<Camera>();camera.transform.position=view.cameraPosition;camera.transform.LookAt(view.lookAt);camera.clearFlags=CameraClearFlags.SolidColor;camera.backgroundColor=view.background;camera.fieldOfView=Camera.HorizontalToVerticalFieldOfView(view.horizontalFov,4f/3f);
            RenderSettings.ambientMode=AmbientMode.Flat;RenderSettings.ambientLight=new Color(.35f,.4f,.43f);var light=new GameObject("C1 signature key",typeof(Light)).GetComponent<Light>();light.type=LightType.Directional;light.intensity=1.7f;light.transform.rotation=Quaternion.Euler(40,-30,0);
            ShaderUtil.allowAsyncCompilation=false;Shader.WarmupAllShaders();Directory.CreateDirectory(Output);Capture(camera,Output+"/reader-front.png");camera.transform.position=bounds.center+new Vector3(-.68f,.945f,-1.10f).normalized*Vector3.Distance(view.cameraPosition,bounds.center);camera.transform.LookAt(bounds.center);Capture(camera,Output+"/reader-alternate.png");
            audit["importedEuler"]=JArray.FromObject(new[]{originalRotation.eulerAngles.x,originalRotation.eulerAngles.y,originalRotation.eulerAngles.z});audit["sourceFront"]= "Blender -Y to Unity +Z, original imported orientation";audit["cameraPosition"]=JArray.FromObject(new[]{view.cameraPosition.x,view.cameraPosition.y,view.cameraPosition.z});
            audit["boundsCenter"]=JArray.FromObject(new[]{bounds.center.x,bounds.center.y,bounds.center.z});audit["boundsSize"]=JArray.FromObject(new[]{bounds.size.x,bounds.size.y,bounds.size.z});audit["paperSize"]=JArray.FromObject(new[]{view.paper.width,view.paper.height});
            File.WriteAllText(Output+"/import-audit.json",audit.ToString());UnityEngine.Object.DestroyImmediate(reader);Debug.Log("C1_SIGNATURE_NATIVE_DIAGNOSTIC "+Output);
        }
        public static void ApproveAndBuildMac(){var view=AssetDatabase.LoadAssetAtPath<C1SignatureViewSettings>(ViewPath);if(view==null||view.reader==null||view.paper==null)throw new InvalidOperationException("Missing signature resources");view.runtimeApproved=true;EditorUtility.SetDirty(view);AssetDatabase.SaveAssets();BuildMac();}
        public static void BuildMac(){var report=BuildPipeline.BuildPlayer(EditorBuildSettings.scenes.Where(s=>s.enabled).Select(s=>s.path).ToArray(),"Builds/C1-M4-mac/Unknown.app",BuildTarget.StandaloneOSX,BuildOptions.Development);Debug.Log("C1_M4_MAC_BUILD "+report.summary.result+" bytes="+report.summary.totalSize);if(report.summary.result!=BuildResult.Succeeded)throw new InvalidOperationException("M4 build failed");}
        static void Capture(Camera camera,string path)
        {
            if(SystemInfo.graphicsDeviceType==GraphicsDeviceType.Null)throw new InvalidOperationException("Native graphics device required");var rt=RenderTexture.GetTemporary(1280,960,24);var previous=RenderTexture.active;
            for(int pass=0;pass<3;pass++)RenderPipeline.SubmitRenderRequest(camera,new UniversalRenderPipeline.SingleCameraRequest{destination=rt});RenderTexture.active=rt;var texture=new Texture2D(1280,960,TextureFormat.RGB24,false);texture.ReadPixels(new Rect(0,0,1280,960),0,0);texture.Apply();File.WriteAllBytes(path,texture.EncodeToPNG());RenderTexture.active=previous;UnityEngine.Object.DestroyImmediate(texture);RenderTexture.ReleaseTemporary(rt);
        }
    }
}
#endif
