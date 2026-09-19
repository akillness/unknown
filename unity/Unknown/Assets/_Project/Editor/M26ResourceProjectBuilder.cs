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
    // M26 (RFC-CX-M26-20260918): imports the second Higgsfield set (question-card backing, receipt envelope,
    // record-media silhouettes, structural-state glyphs) from assets/generated/2d/m26 into
    // Art/Candidates/m26 and binds it to the SAME Resources/M25Resources.asset behind its own gate
    // (m26Approved). SHA must match provenance and the source must still be runtimeEligible:false.
    public static class M26ResourceProjectBuilder {
        const string Folder="Assets/_Project/Art/Candidates/m26";
        const string ProfilePath="Assets/_Project/Resources/M25Resources.asset";
        static readonly string[] MediaIds={"plate","log","ledger"};
        static readonly string[] StateIds={"unreviewed","one-medium","two-media","counterexample"};
        static string Argument(string key,string fallback){var args=Environment.GetCommandLineArgs();int i=Array.IndexOf(args,key);return i>=0&&i+1<args.Length?args[i+1]:fallback;}
        static string Hash(string path){using(var sha=SHA256.Create())using(var stream=File.OpenRead(path))return BitConverter.ToString(sha.ComputeHash(stream)).Replace("-","").ToLowerInvariant();}
        static string SourceRoot=>Argument("--m26-source-root",Path.GetFullPath(Path.Combine(Application.dataPath,"../../..")));
        static JObject Verified(string sourceRoot,string relativeDir,string id){
            var provenance=JObject.Parse(File.ReadAllText(Path.Combine(sourceRoot,relativeDir,"provenance.json")));
            var entry=provenance["assets"].FirstOrDefault(a=>(string)a["id"]==id);
            if(entry==null)throw new InvalidOperationException("M26 provenance entry missing: "+relativeDir+"/"+id);
            var source=Path.Combine(sourceRoot,relativeDir,(string)entry["file"]);
            var hash=Hash(source);
            if(hash!=(string)entry["output_sha256"])throw new InvalidOperationException("M26 hash mismatch for "+id+": expected "+entry["output_sha256"]+" got "+hash);
            if((bool)entry["runtimeEligible"])throw new InvalidOperationException("M26 refuses to import a promoted source: runtimeEligible must stay false for "+id);
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
        [MenuItem("Tools/M26/Import Higgsfield resources")]
        public static void Import(){
            var root=SourceRoot;var audit=new JObject{["scope"]="M26 Higgsfield resources (RFC-CX-M26-20260918)",["items"]=new JArray()};
            var items=(JArray)audit["items"];
            Directory.CreateDirectory(Folder);AssetDatabase.Refresh();
            Texture2D Tex(string dir,string id,string target,int maxSize){
                var v=Verified(root,dir,id);var texture=ImportTexture((string)v["path"],Folder+"/"+target,maxSize);
                items.Add(new JObject{["id"]=id,["source"]=v["source"],["sha256"]=v["sha256"],["unityPath"]=Folder+"/"+target,["width"]=texture.width,["height"]=texture.height,["maxSize"]=maxSize});
                return texture;
            }
            var profile=AssetDatabase.LoadAssetAtPath<M25ResourceProfile>(ProfilePath);
            if(profile==null)throw new InvalidOperationException("M25Resources.asset missing — run Tools/M25/Import first");
            profile.questionCard=Tex("assets/generated/2d/m26/surfaces","card-question","UI_M26_QuestionCard.png",2048);
            profile.receiptEnvelope=Tex("assets/generated/2d/m26/surfaces","prop-transfer-envelope","PROP_M26_TransferEnvelope.png",1024);
            profile.mediaIds=MediaIds.ToArray();
            profile.mediaIcons=MediaIds.Select(id=>Tex("assets/generated/2d/m26/icons","media-"+id,"ICON_Media_"+id+".png",256)).ToArray();
            profile.stateIds=StateIds.ToArray();
            profile.stateGlyphs=StateIds.Select(id=>Tex("assets/generated/2d/m26/glyphs","state-"+id,"GLYPH_State_"+id+".png",256)).ToArray();
            profile.m26Approved=false;
            EditorUtility.SetDirty(profile);AssetDatabase.SaveAssets();
            audit["m26Approved"]=false;audit["profile"]=ProfilePath;audit["gate"]="--m26-resources-diagnostic; m26Approved stays false until Approve()";
            var evidence=Argument("--m26-evidence-dir",Path.Combine(root,"_workspace/current/systems/tech-verification/m26"));
            Directory.CreateDirectory(evidence);File.WriteAllText(Path.Combine(evidence,"import-audit.json"),audit.ToString());
            Debug.Log("M26_RESOURCES_IMPORTED items="+items.Count+" m26Approved=false");
        }
        public static void Approve(){
            var profile=AssetDatabase.LoadAssetAtPath<M25ResourceProfile>(ProfilePath);
            if(profile==null||profile.questionCard==null||profile.receiptEnvelope==null||profile.mediaIcons.Any(t=>t==null)||profile.stateGlyphs.Any(t=>t==null))throw new InvalidOperationException("M26 resources incomplete; refusing to approve");
            profile.m26Approved=true;EditorUtility.SetDirty(profile);AssetDatabase.SaveAssets();
            Debug.Log("M26_RESOURCES_APPROVED local development profile only; commercial licence UNVERIFIED");
        }
    }
}
#endif
