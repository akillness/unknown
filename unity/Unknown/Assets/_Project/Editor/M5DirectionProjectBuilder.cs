using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using Tide.Presentation;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
namespace Tide.EditorTools {
    public static class M5DirectionProjectBuilder {
        const string Folder="Assets/_Project/Art/Candidates/m5-direction";
        const string ProfilePath="Assets/_Project/Resources/M5Direction.asset";
        static string Argument(string key,string fallback){var args=Environment.GetCommandLineArgs();int i=Array.IndexOf(args,key);return i>=0&&i+1<args.Length?args[i+1]:fallback;}
        static string Hash(string path){using(var sha=SHA256.Create())using(var stream=File.OpenRead(path))return BitConverter.ToString(sha.ComputeHash(stream)).Replace("-","").ToLowerInvariant();}
        static Texture2D Import(string source,string target,int maxSize){
            File.Copy(source,target,true);AssetDatabase.ImportAsset(target,ImportAssetOptions.ForceSynchronousImport|ImportAssetOptions.ForceUpdate);
            var importer=(TextureImporter)AssetImporter.GetAtPath(target);
            importer.textureType=TextureImporterType.Default;importer.npotScale=TextureImporterNPOTScale.None;
            importer.mipmapEnabled=false;importer.wrapMode=TextureWrapMode.Clamp;importer.maxTextureSize=maxSize;
            importer.textureCompression=TextureImporterCompression.Compressed;importer.SaveAndReimport();
            return AssetDatabase.LoadAssetAtPath<Texture2D>(target);
        }
        public static void ImportResources(){
            var sourceRoot=Argument("--m5-source-root",Path.GetFullPath(Path.Combine(Application.dataPath,"../../../..")));
            var evidence=Argument("--m5-evidence-dir",Path.Combine(sourceRoot,"_workspace/current/systems/tech-verification/m5-direction"));
            Directory.CreateDirectory(Folder);AssetDatabase.Refresh();
            var openingSource=Path.Combine(sourceRoot,"assets/generated/2d/concept/intro-m5-r03/image.png");
            var surfaceSource=Path.Combine(sourceRoot,"assets/generated/2d/ui/m5-direction-surface-r01/image.png");
            var opening=Import(openingSource,Folder+"/Opening.png",2048);
            var surface=Import(surfaceSource,Folder+"/Surface.png",1024);
            var profile=AssetDatabase.LoadAssetAtPath<M5DirectionProfile>(ProfilePath);
            if(profile==null){profile=ScriptableObject.CreateInstance<M5DirectionProfile>();AssetDatabase.CreateAsset(profile,ProfilePath);}
            profile.openingImage=opening;profile.sectionSurface=surface;profile.firstShotSeconds=3.125f;profile.secondShotSeconds=2.875f;profile.runtimeApproved=false;
            EditorUtility.SetDirty(profile);AssetDatabase.SaveAssets();
            Directory.CreateDirectory(evidence);
            File.WriteAllText(Path.Combine(evidence,"import-audit.json"),new JObject{
                ["scope"]="M5 diagnostic static opening and C1 direction surface",
                ["runtimeApproved"]=profile.runtimeApproved,
                ["opening"]=new JObject{["source"]="assets/generated/2d/concept/intro-m5-r03/image.png",["sha256"]=Hash(openingSource),["unityPath"]=Folder+"/Opening.png",["width"]=opening.width,["height"]=opening.height,["maxSize"]=2048},
                ["surface"]=new JObject{["source"]="assets/generated/2d/ui/m5-direction-surface-r01/image.png",["sha256"]=Hash(surfaceSource),["unityPath"]=Folder+"/Surface.png",["width"]=surface.width,["height"]=surface.height,["maxSize"]=1024},
                ["firstShotMs"]=3125,["secondShotMs"]=2875,["providerVideoSeconds"]=6.041667,
                ["cameraMotion"]="none; static image and hard caption cut",["runtimeVideoDependency"]=false
            }.ToString());
            Debug.Log("M5_RESOURCES_IMPORTED opening="+opening.width+"x"+opening.height+" surface="+surface.width+"x"+surface.height+" runtimeApproved=false");
        }
        public static void ApproveAndBuildMac(){
            var profile=AssetDatabase.LoadAssetAtPath<M5DirectionProfile>(ProfilePath);
            if(profile==null||profile.openingImage==null||profile.sectionSurface==null)throw new InvalidOperationException("M5 resources incomplete");
            profile.runtimeApproved=true;EditorUtility.SetDirty(profile);AssetDatabase.SaveAssets();BuildMac();
        }
        public static void BuildMac(){
            var report=BuildPipeline.BuildPlayer(EditorBuildSettings.scenes.Where(s=>s.enabled).Select(s=>s.path).ToArray(),"Builds/M5-mac/Unknown.app",BuildTarget.StandaloneOSX,BuildOptions.Development);
            Debug.Log("M5_MAC_BUILD "+report.summary.result+" bytes="+report.summary.totalSize);
            if(report.summary.result!=BuildResult.Succeeded)throw new InvalidOperationException("M5 build failed");
        }
    }
}
