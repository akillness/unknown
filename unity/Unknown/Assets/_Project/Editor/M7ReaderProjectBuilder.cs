#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using Tide.Presentation;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
namespace Tide.EditorTools {
    // RFC-CX-013 lane ReaderStage: imports the M7 optical reader + record set FBX and concept-first textures as candidates and never approves them.
    public static class M7ReaderProjectBuilder {
        const string Folder="Assets/_Project/Art/Candidates/m7-reader-r01";
        const string TextureFolder=Folder+"/textures";
        const string ProfilePath="Assets/_Project/Resources/M7ReaderStage.asset";
        const string ShaderName="Universal Render Pipeline/Lit";
        const string AuditPath="Builds/m7-reader-import-audit.json";
        const string FbxSourceFolder="assets/generated/3d/concept-first-m7/fbx";
        const string FbxReport=FbxSourceFolder+"/fbx-export-report.json";
        const string ReaderFbx="SM_Prop_OpticalReader.fbx";
        const string RecordSetFbx="SM_Prop_RecordSet.fbx";
        sealed class TextureSet {
            public string Material,SourceFolder;
            public string TexturePath(string map)=>TextureFolder+"/M7_Reader_"+Material+"_"+map+".png";
        }
        static readonly TextureSet[] TextureSets={
            new TextureSet{Material="Bronze",SourceFolder="assets/generated/2d/texture/m7-bronze-r01"},
            new TextureSet{Material="Steel",SourceFolder="assets/generated/2d/texture/m7-perforated-steel-r01"},
            new TextureSet{Material="Crystal",SourceFolder="assets/generated/2d/texture/m7-salt-crystal-r01"},
            new TextureSet{Material="Paper",SourceFolder="assets/generated/2d/texture/m7-rag-paper-r01"}
        };
        sealed class MaterialSpec {
            public string Name,Textures;public float Metallic,Smoothness;public Color BaseColor=Color.white;public bool Transparent;
            public string Path=>Folder+"/MAT_M7_Reader_"+Name+".mat";
        }
        static readonly MaterialSpec[] Materials={
            new MaterialSpec{Name="Bronze",Textures="Bronze",Metallic=1f},
            new MaterialSpec{Name="Steel",Textures="Steel",Metallic=.85f},
            new MaterialSpec{Name="Crystal",Textures="Crystal",Metallic=0f,BaseColor=Color.white},
            new MaterialSpec{Name="Paper",Textures="Paper",Metallic=0f},
            new MaterialSpec{Name="Wood",Metallic=0f,Smoothness=.45f,BaseColor=new Color(.54f,.36f,.16f)},
            new MaterialSpec{Name="DarkSteel",Metallic=.9f,Smoothness=.55f,BaseColor=new Color(.035f,.045f,.05f)},
            new MaterialSpec{Name="Glass",Metallic=0f,Smoothness=.95f,BaseColor=new Color(.85f,.9f,.92f,.25f),Transparent=true}
        };
        static string Argument(string key,string fallback){var args=Environment.GetCommandLineArgs();int i=Array.IndexOf(args,key);return i>=0&&i+1<args.Length?args[i+1]:fallback;}
        static string Hash(string path){using(var sha=SHA256.Create())using(var stream=File.OpenRead(path))return BitConverter.ToString(sha.ComputeHash(stream)).Replace("-","").ToLowerInvariant();}
        static JArray Vec(Vector3 v)=>JArray.FromObject(new[]{v.x,v.y,v.z});
        [MenuItem("Tools/M7/Import reader stage candidates")]
        public static void ImportReaderStage(){
            var sourceRoot=Argument("--m7-source-root",Path.GetFullPath(Path.Combine(Application.dataPath,"../../..")));
            var fbxFolder=Path.Combine(sourceRoot,FbxSourceFolder);
            if(!Directory.Exists(fbxFolder)||!File.Exists(Path.Combine(sourceRoot,FbxReport))){Debug.LogWarning("M7_READER_SOURCE_MISSING "+fbxFolder+" — nothing imported; runtime keeps the reader stage disabled.");return;}
            foreach(var set in TextureSets){
                var folder=Path.Combine(sourceRoot,set.SourceFolder);
                if(!Directory.Exists(folder)){Debug.LogWarning("M7_READER_SOURCE_MISSING "+folder+" — nothing imported; runtime keeps the reader stage disabled.");return;}
            }
            Directory.CreateDirectory(TextureFolder);AssetDatabase.Refresh();
            var shader=Shader.Find(ShaderName);
            if(shader==null)throw new InvalidOperationException("Shader "+ShaderName+" not found — URP must be active before the M7 reader stage import");
            var report=JObject.Parse(File.ReadAllText(Path.Combine(sourceRoot,FbxReport)));
            var fbxAudit=new JArray();var textures=new JArray();var materials=new JArray();
            // (1)+(2) FBX copy with sha256 verification and importer settings mirrored from Art/Candidates/c1-signature-reader-r02/Reader.fbx.meta.
            var readerFbxPath=ImportFbx(sourceRoot,report,ReaderFbx,"OpticalReader");
            var recordSetFbxPath=ImportFbx(sourceRoot,report,RecordSetFbx,"RecordSet");
            // (1) textures: sRGB only for BaseColor, maxSize 1024, uncompressed, wrap Repeat (Editor/M7HubProjectBuilder.cs idiom).
            foreach(var set in TextureSets){
                var provenance=JObject.Parse(File.ReadAllText(Path.Combine(sourceRoot,set.SourceFolder,"provenance.json")));
                ImportTexture(sourceRoot,set,provenance,"basecolor.png","BaseColor",true,textures);
                ImportTexture(sourceRoot,set,provenance,"roughness.png","Roughness",false,textures);
            }
            // (3) URP Lit materials; textured ones get a packed *_MetallicSmoothness.asset (R=metallic, A=1-roughness) exactly like Editor/C1SignatureProjectBuilder.cs.
            foreach(var spec in Materials)BuildMaterial(shader,spec,materials);
            // (4)+(5) prefabs assembled in an empty scene; crank pivot inserted into the reader hierarchy.
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
            var readerInstance=UnityEngine.Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(readerFbxPath));readerInstance.name=Path.GetFileNameWithoutExtension(ReaderFbx);
            var readerBounds=WorldBounds(readerInstance);
            // Stage space (Presentation/M7ReaderStageProfile.cs): reader at origin, bench top = y 0. Blender exports absolute bench coordinates, so the FBX instance is shifted inside the prefab.
            var readerAnchor=new Vector3(readerBounds.center.x,readerBounds.min.y,readerBounds.center.z);
            var readerRoot=new GameObject("OpticalReader");readerInstance.transform.SetParent(readerRoot.transform,true);readerInstance.transform.position-=readerAnchor;
            foreach(var renderer in readerInstance.GetComponentsInChildren<Renderer>())renderer.sharedMaterial=Load(ReaderMaterial(renderer.name));
            var hexPlate=readerInstance.GetComponentsInChildren<Renderer>().First(r=>r.name.StartsWith("rd-hex-plate"));
            var lookAt=hexPlate.bounds.center;
            var axle=readerInstance.GetComponentsInChildren<Renderer>().First(r=>r.name.StartsWith("rd-crank-axle"));
            var size=axle.bounds.size;var longest=size.x>=size.y&&size.x>=size.z?Vector3.right:size.y>=size.z?Vector3.up:Vector3.forward;
            var pivot=new GameObject("M7 crank pivot").transform;pivot.position=axle.bounds.center;pivot.rotation=longest==Vector3.right?Quaternion.identity:Quaternion.FromToRotation(Vector3.right,longest);
            pivot.SetParent(readerRoot.transform,true);
            if(longest!=Vector3.right)Debug.LogWarning("M7_READER_CRANK_AXIS_NOT_X longest="+longest+" — pivot rest localRotation is not identity; runtime crank spin assumes local X");
            foreach(var part in readerInstance.GetComponentsInChildren<Transform>().Where(t=>t.name.StartsWith("rd-crank-axle")||t.name.StartsWith("rd-crank-arm")||t.name.StartsWith("rd-crank-handle")).ToArray())part.SetParent(pivot,true);
            var readerPrefabBounds=WorldBounds(readerRoot);
            fbxAudit.Add(FbxEntry(sourceRoot,report,ReaderFbx,readerFbxPath,readerRoot,readerPrefabBounds,Folder+"/OpticalReader.prefab"));
            var readerPrefab=PrefabUtility.SaveAsPrefabAsset(readerRoot,Folder+"/OpticalReader.prefab");
            var recordInstance=UnityEngine.Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(recordSetFbxPath));recordInstance.name=Path.GetFileNameWithoutExtension(RecordSetFbx);
            var recordBounds=WorldBounds(recordInstance);
            var recordAnchor=new Vector3(recordBounds.center.x,recordBounds.min.y,recordBounds.center.z);
            var recordRoot=new GameObject("RecordSet");recordInstance.transform.SetParent(recordRoot.transform,true);recordInstance.transform.position-=recordAnchor;
            foreach(var renderer in recordInstance.GetComponentsInChildren<Renderer>())renderer.sharedMaterial=Load(RecordSetMaterial(renderer.name));
            var recordPrefabBounds=WorldBounds(recordRoot);
            fbxAudit.Add(FbxEntry(sourceRoot,report,RecordSetFbx,recordSetFbxPath,recordRoot,recordPrefabBounds,Folder+"/RecordSet.prefab"));
            var recordPrefab=PrefabUtility.SaveAsPrefabAsset(recordRoot,Folder+"/RecordSet.prefab");
            var crankPivotWorldPosition=pivot.position;UnityEngine.Object.DestroyImmediate(readerRoot);UnityEngine.Object.DestroyImmediate(recordRoot);
            // (6) profile: placeholders overwritten from imported bounds; both FBX share Blender bench space so the record set keeps its authored offset from the reader.
            var profile=AssetDatabase.LoadAssetAtPath<M7ReaderStageProfile>(ProfilePath);
            if(profile==null){profile=ScriptableObject.CreateInstance<M7ReaderStageProfile>();AssetDatabase.CreateAsset(profile,ProfilePath);}
            profile.reader=readerPrefab;profile.recordSet=recordPrefab;
            profile.readerLocalPosition=Vector3.zero;profile.recordSetLocalPosition=recordAnchor-readerAnchor;
            // Camera side is measured, not assumed: FBX axis conversion mirrors X, so "crank side" and "front" (thumbscrew) are read from the imported instance.
            var crankSide=Mathf.Sign(crankPivotWorldPosition.x-lookAt.x);if(crankSide==0)crankSide=1;
            var thumb=readerPrefab.GetComponentsInChildren<Renderer>(true).FirstOrDefault(r=>r.name.StartsWith("rd-thumbscrew"));
            var frontZ=thumb!=null?Mathf.Sign(thumb.transform.position.z-lookAt.z):-1f;if(frontZ==0)frontZ=-1f;
            profile.lookAt=lookAt;profile.cameraPosition=lookAt+new Vector3(crankSide*.55f,.42f,frontZ*1.15f);profile.horizontalFov=48f;
            // Candidate only: approval is a separate director-only menu item (Tools/M7/Approve reader stage).
            profile.runtimeApproved=false;
            EditorUtility.SetDirty(profile);AssetDatabase.SaveAssets();
            // (7) audit
            Directory.CreateDirectory("Builds");
            File.WriteAllText(AuditPath,new JObject{
                ["scope"]="M7 concept-first optical reader stage candidate (RFC-CX-013 ReaderStage)",
                ["runtimeApproved"]=profile.runtimeApproved,
                ["profile"]=ProfilePath,
                ["fbx"]=fbxAudit,
                ["textures"]=textures,
                ["materials"]=materials,
                ["stageAnchor"]=new JObject{["readerAnchorBlenderSpace"]=Vec(readerAnchor),["recordSetAnchorBlenderSpace"]=Vec(recordAnchor),["readerLocalPosition"]=Vec(profile.readerLocalPosition),["recordSetLocalPosition"]=Vec(profile.recordSetLocalPosition)},
                ["crankPivotWorldPosition"]=Vec(crankPivotWorldPosition),
                ["crankAxis"]=Vec(longest),
                ["crankPivotRestLocalRotationIdentity"]=longest==Vector3.right,
                ["crankPivotName"]=profile.crankPivotName,
                ["lookAt"]=Vec(profile.lookAt),
                ["cameraPosition"]=Vec(profile.cameraPosition),
                ["horizontalFov"]=profile.horizontalFov,
                ["approvalPath"]="Tools/M7/Approve reader stage (director only) after native inspection of the reader stage"
            }.ToString());
            Debug.Log("M7_READER_STAGE_IMPORTED fbx="+fbxAudit.Count+" textures="+textures.Count+" materials="+materials.Count+" runtimeApproved=false");
        }
        static string ImportFbx(string sourceRoot,JObject report,string fbxName,string unityName){
            var source=Path.Combine(sourceRoot,FbxSourceFolder,fbxName);
            if(!File.Exists(source))throw new InvalidOperationException("M7 reader FBX source missing: "+source);
            var expected=(string)report["targets"].First(t=>(string)t["fbx"]==fbxName)["sha256"];
            var hash=Hash(source);
            if(hash!=expected)throw new InvalidOperationException("M7 reader FBX hash mismatch for "+fbxName+": expected "+expected+" got "+hash);
            var target=Folder+"/"+unityName+".fbx";
            File.Copy(source,target,true);AssetDatabase.ImportAsset(target,ImportAssetOptions.ForceSynchronousImport|ImportAssetOptions.ForceUpdate);
            // Mirrors Art/Candidates/c1-signature-reader-r02/Reader.fbx.meta: file scale, no axis bake, no cameras/lights, not readable, mesh compression off; materials authored in-editor.
            var importer=(ModelImporter)AssetImporter.GetAtPath(target);
            importer.materialImportMode=ModelImporterMaterialImportMode.None;importer.useFileScale=true;importer.bakeAxisConversion=false;
            importer.importCameras=false;importer.importLights=false;importer.isReadable=false;importer.meshCompression=ModelImporterMeshCompression.Off;importer.importAnimation=false;
            importer.SaveAndReimport();
            return target;
        }
        static JObject FbxEntry(string sourceRoot,JObject report,string fbxName,string unityPath,GameObject root,Bounds bounds,string prefabPath){
            var meshes=new JArray();
            foreach(var renderer in root.GetComponentsInChildren<Renderer>()){
                var filter=renderer.GetComponent<MeshFilter>();var mesh=filter!=null?filter.sharedMesh:null;
                meshes.Add(new JObject{["name"]=renderer.name,["triangles"]=mesh!=null?mesh.triangles.Length/3:0,["material"]=renderer.sharedMaterial!=null?renderer.sharedMaterial.name:null});
            }
            return new JObject{["source"]=FbxSourceFolder+"/"+fbxName,["sha256"]=(string)report["targets"].First(t=>(string)t["fbx"]==fbxName)["sha256"],["unityPath"]=unityPath,["prefab"]=prefabPath,["meshes"]=meshes,["boundsCenter"]=Vec(bounds.center),["boundsSize"]=Vec(bounds.size)};
        }
        static Bounds WorldBounds(GameObject root){var renderers=root.GetComponentsInChildren<Renderer>();if(renderers.Length==0)throw new InvalidOperationException("M7 reader FBX has no renderers: "+root.name);var bounds=renderers[0].bounds;foreach(var item in renderers)bounds.Encapsulate(item.bounds);return bounds;}
        static Material Load(string materialName)=>AssetDatabase.LoadAssetAtPath<Material>(Folder+"/MAT_M7_Reader_"+materialName+".mat");
        static string ReaderMaterial(string name){
            if(name.StartsWith("rd-crank-handle"))return "Wood";
            if(name.StartsWith("rd-mag-glass"))return "Glass";
            if(name.StartsWith("rd-hex-plate"))return "Crystal";
            return "Bronze";
        }
        static string RecordSetMaterial(string name){
            if(name.Contains("pages")||name.Contains("stack")||name.Contains("card"))return "Paper";
            if(name.StartsWith("rc-copy-hex"))return "Crystal";
            if(name.StartsWith("rc-ledger-cover")||name.StartsWith("rc-ledger-top")||name.StartsWith("rc-stand-")||name.StartsWith("rc-tray-cav"))return "DarkSteel";
            return "Bronze";
        }
        static Texture2D ImportTexture(string sourceRoot,TextureSet set,JObject provenance,string sourceFile,string map,bool srgb,JArray audit){
            var sourceRelative=set.SourceFolder+"/"+sourceFile;
            var source=Path.Combine(sourceRoot,sourceRelative);
            if(!File.Exists(source))throw new InvalidOperationException("M7 reader source missing: "+source);
            var expected=(string)provenance["assets"].First(a=>(string)a["file"]==sourceFile)["sha256"];
            var hash=Hash(source);
            if(hash!=expected)throw new InvalidOperationException("M7 reader texture hash mismatch for "+sourceRelative+": expected "+expected+" got "+hash);
            var target=set.TexturePath(map);
            File.Copy(source,target,true);AssetDatabase.ImportAsset(target,ImportAssetOptions.ForceSynchronousImport|ImportAssetOptions.ForceUpdate);
            var importer=(TextureImporter)AssetImporter.GetAtPath(target);
            importer.textureType=TextureImporterType.Default;importer.sRGBTexture=srgb;importer.maxTextureSize=1024;
            importer.textureCompression=TextureImporterCompression.Uncompressed;importer.wrapMode=TextureWrapMode.Repeat;importer.SaveAndReimport();
            var texture=AssetDatabase.LoadAssetAtPath<Texture2D>(target);
            audit.Add(new JObject{["source"]=sourceRelative,["sha256"]=hash,["unityPath"]=target,["width"]=texture.width,["height"]=texture.height,["maxSize"]=1024,["sRGB"]=importer.sRGBTexture});
            return texture;
        }
        static void BuildMaterial(Shader shader,MaterialSpec spec,JArray audit){
            var material=new Material(shader);
            material.SetColor("_BaseColor",spec.BaseColor);material.SetFloat("_Metallic",spec.Metallic);material.SetFloat("_Smoothness",spec.Smoothness);
            string packedPath=null;
            if(spec.Textures!=null){
                var set=TextureSets.First(s=>s.Material==spec.Textures);
                var baseMap=AssetDatabase.LoadAssetAtPath<Texture2D>(set.TexturePath("BaseColor"));var roughMap=AssetDatabase.LoadAssetAtPath<Texture2D>(set.TexturePath("Roughness"));
                material.SetTexture("_BaseMap",baseMap);
                if(roughMap!=null){
                    var importer=(TextureImporter)AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(roughMap));importer.isReadable=true;importer.SaveAndReimport();
                    roughMap=AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GetAssetPath(roughMap));var map=new Texture2D(roughMap.width,roughMap.height,TextureFormat.RGBA32,false,true);
                    var pixels=roughMap.GetPixels();for(int i=0;i<pixels.Length;i++)pixels[i]=new Color(spec.Metallic,0,0,1-pixels[i].r);map.SetPixels(pixels);map.Apply();
                    packedPath=Folder+"/MAT_M7_Reader_"+spec.Name+"_MetallicSmoothness.asset";if(AssetDatabase.LoadAssetAtPath<Texture2D>(packedPath)==null)AssetDatabase.CreateAsset(map,packedPath);else {EditorUtility.CopySerialized(map,AssetDatabase.LoadAssetAtPath<Texture2D>(packedPath));UnityEngine.Object.DestroyImmediate(map);}
                    material.SetTexture("_MetallicGlossMap",AssetDatabase.LoadAssetAtPath<Texture2D>(packedPath));material.EnableKeyword("_METALLICSPECGLOSSMAP");material.SetFloat("_Smoothness",1);
                }
            }
            if(spec.Transparent){
                // URP Lit transparent surface (alpha blend): what BaseShaderGUI writes when Surface Type = Transparent, Blend = Alpha.
                material.SetFloat("_Surface",1);material.SetFloat("_Blend",0);material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                material.SetFloat("_SrcBlend",(float)UnityEngine.Rendering.BlendMode.SrcAlpha);material.SetFloat("_DstBlend",(float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);material.SetFloat("_ZWrite",0);
                material.SetOverrideTag("RenderType","Transparent");material.renderQueue=3000;
            }
            if(AssetDatabase.LoadAssetAtPath<Material>(spec.Path)==null)AssetDatabase.CreateAsset(material,spec.Path);else {EditorUtility.CopySerialized(material,AssetDatabase.LoadAssetAtPath<Material>(spec.Path));UnityEngine.Object.DestroyImmediate(material);}
            var saved=AssetDatabase.LoadAssetAtPath<Material>(spec.Path);
            audit.Add(new JObject{["material"]=saved.name,["unityPath"]=spec.Path,["shader"]=ShaderName,["metallic"]=spec.Metallic,["smoothness"]=saved.GetFloat("_Smoothness"),["baseColor"]=JArray.FromObject(new[]{spec.BaseColor.r,spec.BaseColor.g,spec.BaseColor.b,spec.BaseColor.a}),["baseMapBound"]=saved.GetTexture("_BaseMap")!=null,["metallicSmoothnessMap"]=packedPath,["transparent"]=spec.Transparent,["renderQueue"]=saved.renderQueue});
        }
        [MenuItem("Tools/M7/Approve reader stage (director only)")]
        public static void ApproveReaderStage(){
            var profile=AssetDatabase.LoadAssetAtPath<M7ReaderStageProfile>(ProfilePath);
            if(profile==null||profile.reader==null||profile.recordSet==null)throw new InvalidOperationException("M7 reader stage resources incomplete — run Tools/M7/Import reader stage candidates first");
            profile.runtimeApproved=true;EditorUtility.SetDirty(profile);AssetDatabase.SaveAssets();
            Debug.Log("M7_READER_STAGE_APPROVED runtimeApproved=true");
        }
    }
}
#endif
