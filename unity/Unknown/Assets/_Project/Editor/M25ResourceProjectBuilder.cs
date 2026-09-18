#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using Tide.Presentation;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;
namespace Tide.EditorTools {
    // M25 (RFC-CX-M25-20260918): imports the Higgsfield-generated backgrounds, portraits, tool icons and
    // the opening clip from assets/generated/{2d/m25,video/m25} into Art/Candidates/m25 and binds them
    // to Resources/M25Resources.asset. Every source hash must match its provenance.json entry and the
    // source must still be runtimeEligible:false (the copy is the candidate; promotion is a separate
    // director step — Approve()). The importer never edits assets/generated/**.
    public static class M25ResourceProjectBuilder {
        const string Folder="Assets/_Project/Art/Candidates/m25";
        const string ProfilePath="Assets/_Project/Resources/M25Resources.asset";
        static readonly string[] ZoneIds={"gate","lowland","dock","pump"};
        static readonly string[] ZoneLabels={"제3수문","구염전 저지대","냉동창고 부두","제1양수장"};
        static readonly string[] ZoneSources={"bg-gate-three","bg-lowland","bg-quay","bg-pump-one"};
        static readonly string[] ToolIds={"circuit","reader","alignment","routing","corrosion","seal"};
        static readonly string[] PortraitIds={"seorin","jaehwa","eunjeong","seongchan","doyeon"};
        static string Argument(string key,string fallback){var args=Environment.GetCommandLineArgs();int i=Array.IndexOf(args,key);return i>=0&&i+1<args.Length?args[i+1]:fallback;}
        static string Hash(string path){using(var sha=SHA256.Create())using(var stream=File.OpenRead(path))return BitConverter.ToString(sha.ComputeHash(stream)).Replace("-","").ToLowerInvariant();}
        static string SourceRoot=>Argument("--m25-source-root",Path.GetFullPath(Path.Combine(Application.dataPath,"../../..")));
        static JObject Verified(string sourceRoot,string relativeDir,string id){
            var provenance=JObject.Parse(File.ReadAllText(Path.Combine(sourceRoot,relativeDir,"provenance.json")));
            var entry=provenance["assets"].FirstOrDefault(a=>(string)a["id"]==id);
            if(entry==null)throw new InvalidOperationException("M25 provenance entry missing: "+relativeDir+"/"+id);
            var source=Path.Combine(sourceRoot,relativeDir,(string)entry["file"]);
            var hash=Hash(source);
            if(hash!=(string)entry["output_sha256"])throw new InvalidOperationException("M25 hash mismatch for "+id+": expected "+entry["output_sha256"]+" got "+hash);
            if((bool)entry["runtimeEligible"])throw new InvalidOperationException("M25 refuses to import a promoted source: runtimeEligible must stay false for "+id);
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
        static VideoClip ImportClip(string source,string target){
            File.Copy(source,target,true);
            AssetDatabase.ImportAsset(target,ImportAssetOptions.ForceSynchronousImport|ImportAssetOptions.ForceUpdate);
            return AssetDatabase.LoadAssetAtPath<VideoClip>(target);
        }
        [MenuItem("Tools/M25/Import Higgsfield resources")]
        public static void Import(){
            var root=SourceRoot;var audit=new JObject{["scope"]="M25 Higgsfield resources (RFC-CX-M25-20260918)",["items"]=new JArray()};
            var items=(JArray)audit["items"];
            Directory.CreateDirectory(Folder);AssetDatabase.Refresh();
            Texture2D Tex(string dir,string id,string target,int maxSize){
                var v=Verified(root,dir,id);var texture=ImportTexture((string)v["path"],Folder+"/"+target,maxSize);
                items.Add(new JObject{["id"]=id,["source"]=v["source"],["sha256"]=v["sha256"],["unityPath"]=Folder+"/"+target,["width"]=texture.width,["height"]=texture.height,["maxSize"]=maxSize});
                return texture;
            }
            var profile=AssetDatabase.LoadAssetAtPath<M25ResourceProfile>(ProfilePath);
            if(profile==null){profile=ScriptableObject.CreateInstance<M25ResourceProfile>();AssetDatabase.CreateAsset(profile,ProfilePath);}
            profile.openingImage=Tex("assets/generated/2d/m25/backgrounds","bg-harbor-night","BG_Opening_HarborNight.png",2048);
            profile.startBackdrop=Tex("assets/generated/2d/m25/backgrounds","bg-hub-watchroom","BG_Start_HubWatchroom.png",2048);
            profile.zoneIds=ZoneIds.ToArray();profile.zoneLabels=ZoneLabels.ToArray();
            profile.zoneBackdrops=ZoneSources.Select((src,i)=>Tex("assets/generated/2d/m25/backgrounds",src,"BG_Zone_"+ZoneIds[i]+".png",1024)).ToArray();
            profile.toolIds=ToolIds.ToArray();
            profile.toolIcons=ToolIds.Select(id=>Tex("assets/generated/2d/m25/tools","icon-"+id,"ICON_Tool_"+id+".png",512)).ToArray();
            profile.portraitIds=PortraitIds.ToArray();
            profile.portraits=PortraitIds.Select(id=>Tex("assets/generated/2d/m25/portraits","pt-"+id,"PT_"+id+".png",1024)).ToArray();
            var clipSource=Path.Combine(root,"assets/generated/video/m25/mo-opening-harbor.mp4");
            if(File.Exists(clipSource)){
                var v=Verified(root,"assets/generated/video/m25","mo-opening-harbor");
                profile.openingClip=ImportClip((string)v["path"],Folder+"/MO_Opening_Harbor.mp4");
                items.Add(new JObject{["id"]="mo-opening-harbor",["source"]=v["source"],["sha256"]=v["sha256"],["unityPath"]=Folder+"/MO_Opening_Harbor.mp4",["seconds"]=profile.openingClip==null?0:profile.openingClip.length,["frameRate"]=profile.openingClip==null?0:profile.openingClip.frameRate});
            }else{profile.openingClip=null;items.Add(new JObject{["id"]="mo-opening-harbor",["skipped"]="source missing"});}
            profile.runtimeApproved=false;
            EditorUtility.SetDirty(profile);AssetDatabase.SaveAssets();
            audit["runtimeApproved"]=false;audit["profile"]=ProfilePath;audit["gate"]="--m25-resources-diagnostic; runtimeApproved stays false until Approve()";
            var evidence=Argument("--m25-evidence-dir",Path.Combine(root,"_workspace/current/systems/tech-verification/m25"));
            Directory.CreateDirectory(evidence);File.WriteAllText(Path.Combine(evidence,"import-audit.json"),audit.ToString());
            Debug.Log("M25_RESOURCES_IMPORTED items="+items.Count+" runtimeApproved=false");
        }
        // Director-only step after the promotion audit (decision-log RFC-CX-M25-20260918 · 승격 감사).
        public static void Approve(){
            var profile=AssetDatabase.LoadAssetAtPath<M25ResourceProfile>(ProfilePath);
            if(profile==null||profile.openingImage==null||profile.startBackdrop==null||profile.toolIcons.Any(t=>t==null)||profile.portraits.Any(p=>p==null))throw new InvalidOperationException("M25 resources incomplete; refusing to approve");
            profile.runtimeApproved=true;EditorUtility.SetDirty(profile);AssetDatabase.SaveAssets();
            Debug.Log("M25_RESOURCES_APPROVED local development profile only; commercial licence UNVERIFIED");
        }
    }
}
#endif
