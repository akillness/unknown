#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using Tide.Presentation;
using UnityEditor;
using UnityEngine;
namespace Tide.EditorTools {
    // RFC-CX-013 lane HubShell: imports the M7 concept-first shell textures as candidates and never approves them.
    public static class M7HubProjectBuilder {
        const string Folder="Assets/_Project/Art/Candidates/m7-hub-r01";
        const string TextureFolder=Folder+"/textures";
        const string ProfilePath="Assets/_Project/Resources/M7Hub.asset";
        const string ShaderName="Tide/Candidate Roughness";
        const string AuditPath="Builds/m7-hub-import-audit.json";
        sealed class Candidate {
            public string Material,SourceFolder;public float Metallic;public Vector2 Tiling;
            public string MaterialPath=>Folder+"/MAT_M7_Hub_"+Material+".mat";
            public string TexturePath(string map)=>TextureFolder+"/M7_Hub_"+Material+"_"+map+".png";
        }
        // RFC-CX-016 metallic/tiling correction. The committed hub has no reflection probe and no skybox: with
        // RenderSettings.ambientMode=Flat the only environment term UniversalFragmentPBR can sample is the flat
        // ambient colour. A high metallic there folds the albedo into F0 and scales the diffuse term by (1-metallic),
        // so the perforated-steel workbench measured 42.6/53.6/55.6 mean against the 75.0/84.6/86.0 salt-concrete
        // wall in the same shot — a flat dark mass with no readable perforation (the defect RFC-CX-013 carried as
        // "승격 전에 타일링·밝기 조정이 필요하다"). Metallic is lowered so the authored albedo reads, and tiling is
        // raised for texel density on the 0.9-1.2 m workbench/shelf boxes (Unity cube UVs are 0..1 per face, so the
        // old 2x repeat stretched the perforation past recognition). The alternative fix — adding a reflection probe
        // to the hub scene — is carried: that scene is rebuilt by T0ProjectBuilder.Prepare and is not this lane's.
        static readonly Candidate[] Candidates={
            new Candidate{Material="SaltConcrete",SourceFolder="assets/generated/2d/texture/m7-salt-concrete-r01",Metallic=0f,Tiling=new Vector2(2,2)},
            new Candidate{Material="PerforatedSteel",SourceFolder="assets/generated/2d/texture/m7-perforated-steel-r01",Metallic=.25f,Tiling=new Vector2(4,4)},
            new Candidate{Material="Bronze",SourceFolder="assets/generated/2d/texture/m7-bronze-r01",Metallic=.45f,Tiling=new Vector2(4,4)}
        };
        static string Argument(string key,string fallback){var args=Environment.GetCommandLineArgs();int i=Array.IndexOf(args,key);return i>=0&&i+1<args.Length?args[i+1]:fallback;}
        static string Hash(string path){using(var sha=SHA256.Create())using(var stream=File.OpenRead(path))return BitConverter.ToString(sha.ComputeHash(stream)).Replace("-","").ToLowerInvariant();}
        [MenuItem("Tools/M7/Import hub shell candidates")]
        public static void ImportHubShell(){
            var sourceRoot=Argument("--m7-source-root",Path.GetFullPath(Path.Combine(Application.dataPath,"../../..")));
            foreach(var candidate in Candidates){
                var folder=Path.Combine(sourceRoot,candidate.SourceFolder);
                if(!Directory.Exists(folder)){Debug.LogWarning("M7_HUB_SOURCE_MISSING "+folder+" — nothing imported; runtime keeps the committed hub shell.");return;}
            }
            Directory.CreateDirectory(TextureFolder);AssetDatabase.Refresh();
            var shader=Shader.Find(ShaderName);
            if(shader==null)throw new InvalidOperationException("Shader "+ShaderName+" not found — Rendering/CandidateRoughness.shader must import before the M7 hub shell");
            var textures=new JArray();var materials=new JArray();
            var profile=AssetDatabase.LoadAssetAtPath<M7HubProfile>(ProfilePath);
            if(profile==null){profile=ScriptableObject.CreateInstance<M7HubProfile>();AssetDatabase.CreateAsset(profile,ProfilePath);}
            foreach(var candidate in Candidates){
                var provenance=JObject.Parse(File.ReadAllText(Path.Combine(sourceRoot,candidate.SourceFolder,"provenance.json")));
                var baseMap=ImportTexture(sourceRoot,candidate,provenance,"basecolor.png","BaseColor",true,textures);
                var roughness=ImportTexture(sourceRoot,candidate,provenance,"roughness.png","Roughness",false,textures);
                var material=AssetDatabase.LoadAssetAtPath<Material>(candidate.MaterialPath);
                if(material==null){material=new Material(shader);AssetDatabase.CreateAsset(material,candidate.MaterialPath);}
                material.shader=shader;
                material.SetTexture("_BaseMap",baseMap);material.SetTexture("_RoughnessMap",roughness);material.SetFloat("_Metallic",candidate.Metallic);
                // The candidate shader transforms both samples with _BaseMap_ST; the roughness scale is mirrored for inspector honesty.
                material.SetTextureScale("_BaseMap",candidate.Tiling);material.SetTextureScale("_RoughnessMap",candidate.Tiling);
                EditorUtility.SetDirty(material);
                materials.Add(new JObject{["material"]=candidate.Material,["unityPath"]=candidate.MaterialPath,["shader"]=ShaderName,["metallic"]=candidate.Metallic,["tiling"]=new JArray(candidate.Tiling.x,candidate.Tiling.y),["baseMapBound"]=baseMap!=null,["roughnessBound"]=roughness!=null});
                if(candidate.Material=="SaltConcrete")profile.floorAndWall=material;
                else if(candidate.Material=="PerforatedSteel")profile.workbench=material;
                else if(candidate.Material=="Bronze")profile.plateShelf=material;
            }
            // Candidate only: approval is a separate director-only menu item (Tools/M7/Approve hub shell).
            profile.runtimeApproved=false;
            EditorUtility.SetDirty(profile);AssetDatabase.SaveAssets();
            Directory.CreateDirectory("Builds");
            File.WriteAllText(AuditPath,new JObject{
                ["scope"]="M7 concept-first hub shell candidate (RFC-CX-013 HubShell)",
                ["runtimeApproved"]=profile.runtimeApproved,
                ["profile"]=ProfilePath,
                ["textures"]=textures,
                ["materials"]=materials,
                ["rootMapping"]=new JObject{["floorAndWall"]=JArray.FromObject(profile.floorAndWallRoots),["workbench"]=JArray.FromObject(profile.workbenchRoots),["plateShelf"]=JArray.FromObject(profile.plateShelfRoots)},
                ["approvalPath"]="Tools/M7/Approve hub shell (director only) after native inspection of the hub view"
            }.ToString());
            Debug.Log("M7_HUB_SHELL_IMPORTED textures="+textures.Count+" materials="+materials.Count+" runtimeApproved=false");
        }
        static Texture2D ImportTexture(string sourceRoot,Candidate candidate,JObject provenance,string sourceFile,string map,bool srgb,JArray audit){
            var sourceRelative=candidate.SourceFolder+"/"+sourceFile;
            var source=Path.Combine(sourceRoot,sourceRelative);
            if(!File.Exists(source))throw new InvalidOperationException("M7 hub source missing: "+source);
            var expected=(string)provenance["assets"].First(a=>(string)a["file"]==sourceFile)["sha256"];
            var hash=Hash(source);
            if(hash!=expected)throw new InvalidOperationException("M7 hub texture hash mismatch for "+sourceRelative+": expected "+expected+" got "+hash);
            var target=candidate.TexturePath(map);
            File.Copy(source,target,true);AssetDatabase.ImportAsset(target,ImportAssetOptions.ForceSynchronousImport|ImportAssetOptions.ForceUpdate);
            // Import rules mirror Editor/T0ResourceDiagnostics.cs: sRGB only for BaseColor, maxSize 1024, uncompressed; tiled shell maps repeat.
            var importer=(TextureImporter)AssetImporter.GetAtPath(target);
            importer.textureType=TextureImporterType.Default;importer.sRGBTexture=srgb;importer.maxTextureSize=1024;
            importer.textureCompression=TextureImporterCompression.Uncompressed;importer.wrapMode=TextureWrapMode.Repeat;importer.SaveAndReimport();
            var texture=AssetDatabase.LoadAssetAtPath<Texture2D>(target);
            audit.Add(new JObject{["source"]=sourceRelative,["sha256"]=hash,["unityPath"]=target,["width"]=texture.width,["height"]=texture.height,["maxSize"]=1024,["sRGB"]=importer.sRGBTexture});
            return texture;
        }
        [MenuItem("Tools/M7/Approve hub shell (director only)")]
        public static void ApproveHubShell(){
            var profile=AssetDatabase.LoadAssetAtPath<M7HubProfile>(ProfilePath);
            if(profile==null||profile.floorAndWall==null||profile.workbench==null||profile.plateShelf==null)throw new InvalidOperationException("M7 hub shell resources incomplete — run Tools/M7/Import hub shell candidates first");
            profile.runtimeApproved=true;EditorUtility.SetDirty(profile);AssetDatabase.SaveAssets();
            Debug.Log("M7_HUB_SHELL_APPROVED runtimeApproved=true");
        }
    }
}
#endif
