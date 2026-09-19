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
    // M27 (RFC-CX-M27-20260918): imports the requirement-checklist glyphs (done / current / open) from
    // assets/generated/2d/m27/glyphs into Art/Candidates/m27 and binds them to Resources/M25Resources.asset behind
    // their own gate (m27Approved). SHA must match provenance and the source must still be runtimeEligible:false.
    public static class M27ResourceProjectBuilder {
        const string Folder="Assets/_Project/Art/Candidates/m27";
        const string ProfilePath="Assets/_Project/Resources/M25Resources.asset";
        static readonly string[] StateIds={"done","current","open"};
        static string Argument(string key,string fallback){var args=Environment.GetCommandLineArgs();int i=Array.IndexOf(args,key);return i>=0&&i+1<args.Length?args[i+1]:fallback;}
        static string Hash(string path){using(var sha=SHA256.Create())using(var stream=File.OpenRead(path))return BitConverter.ToString(sha.ComputeHash(stream)).Replace("-","").ToLowerInvariant();}
        static string SourceRoot=>Argument("--m27-source-root",Path.GetFullPath(Path.Combine(Application.dataPath,"../../..")));
        static JObject Verified(string sourceRoot,string relativeDir,string id){
            var provenance=JObject.Parse(File.ReadAllText(Path.Combine(sourceRoot,relativeDir,"provenance.json")));
            var entry=provenance["assets"].FirstOrDefault(a=>(string)a["id"]==id);
            if(entry==null)throw new InvalidOperationException("M27 provenance entry missing: "+relativeDir+"/"+id);
            var source=Path.Combine(sourceRoot,relativeDir,(string)entry["file"]);
            var hash=Hash(source);
            if(hash!=(string)entry["output_sha256"])throw new InvalidOperationException("M27 hash mismatch for "+id+": expected "+entry["output_sha256"]+" got "+hash);
            if((bool)entry["runtimeEligible"])throw new InvalidOperationException("M27 refuses to import a promoted source: runtimeEligible must stay false for "+id);
            return new JObject{["source"]=Path.Combine(relativeDir,(string)entry["file"]).Replace('\\','/'),["sha256"]=hash,["path"]=source};
        }
        static Texture2D ImportTexture(string source,string target,int maxSize){
            File.Copy(source,target,true);
            AssetDatabase.ImportAsset(target,ImportAssetOptions.ForceSynchronousImport|ImportAssetOptions.ForceUpdate);
            var importer=(TextureImporter)AssetImporter.GetAtPath(target);
            importer.textureType=TextureImporterType.Default;importer.sRGBTexture=true;importer.alphaIsTransparency=true;
            importer.npotScale=TextureImporterNPOTScale.None;importer.mipmapEnabled=false;importer.wrapMode=TextureWrapMode.Clamp;
            importer.maxTextureSize=maxSize;importer.textureCompression=TextureImporterCompression.Compressed;importer.SaveAndReimport();
            return AssetDatabase.LoadAssetAtPath<Texture2D>(target);
        }
        [MenuItem("Tools/M27/Import checklist glyphs")]
        public static void Import(){
            var root=SourceRoot;var audit=new JObject{["scope"]="M27 checklist glyphs (RFC-CX-M27-20260918)",["items"]=new JArray()};
            var items=(JArray)audit["items"];
            Directory.CreateDirectory(Folder);AssetDatabase.Refresh();
            var profile=AssetDatabase.LoadAssetAtPath<M25ResourceProfile>(ProfilePath);
            if(profile==null)throw new InvalidOperationException("M25Resources.asset missing — run Tools/M25/Import first");
            profile.checklistIds=StateIds.ToArray();
            profile.checklistGlyphs=StateIds.Select(id=>{
                var v=Verified(root,"assets/generated/2d/m27/glyphs","check-"+id);
                var target=Folder+"/GLYPH_Check_"+id+".png";var texture=ImportTexture((string)v["path"],target,256);
                items.Add(new JObject{["id"]="check-"+id,["source"]=v["source"],["sha256"]=v["sha256"],["unityPath"]=target,["width"]=texture.width,["height"]=texture.height,["maxSize"]=256});
                return texture;
            }).ToArray();
            profile.m27Approved=false;
            EditorUtility.SetDirty(profile);AssetDatabase.SaveAssets();
            audit["m27Approved"]=false;audit["profile"]=ProfilePath;audit["gate"]="--m27-resources-diagnostic; m27Approved stays false until Approve()";
            var evidence=Argument("--m27-evidence-dir",Path.Combine(root,"_workspace/current/systems/tech-verification/m27"));
            Directory.CreateDirectory(evidence);File.WriteAllText(Path.Combine(evidence,"import-audit.json"),audit.ToString());
            Debug.Log("M27_RESOURCES_IMPORTED items="+items.Count+" m27Approved=false");
        }
        public static void Approve(){
            var profile=AssetDatabase.LoadAssetAtPath<M25ResourceProfile>(ProfilePath);
            if(profile==null||profile.checklistGlyphs.Length!=StateIds.Length||profile.checklistGlyphs.Any(t=>t==null))throw new InvalidOperationException("M27 resources incomplete — import first");
            profile.m27Approved=true;EditorUtility.SetDirty(profile);AssetDatabase.SaveAssets();
            Debug.Log("M27_RESOURCES_APPROVED local development profile only; commercial licence UNVERIFIED");
        }
    }
}
#endif
