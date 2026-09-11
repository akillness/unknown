using System;
using System.IO;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using Tide.Presentation;
using UnityEditor;
using UnityEngine;
namespace Tide.EditorTools {
    public static class M8ReviewNotesProjectBuilder {
        const string Folder="Assets/_Project/Art/Candidates/m8-review-card";
        const string TexturePath=Folder+"/CardPaper.png";
        const string ProfilePath="Assets/_Project/Resources/M8ReviewNotes.asset";
        const string SourceRelative="assets/generated/2d/texture/m8-review-card-r01/image.png";
        const string SourceSha256="65fc439b6e9de394b9e68519ced237ebfe7d53ee973fb2ca700858abd24d9367";
        static string Argument(string key,string fallback){var args=Environment.GetCommandLineArgs();int i=Array.IndexOf(args,key);return i>=0&&i+1<args.Length?args[i+1]:fallback;}
        static string Hash(string path){using(var sha=SHA256.Create())using(var stream=File.OpenRead(path))return BitConverter.ToString(sha.ComputeHash(stream)).Replace("-","").ToLowerInvariant();}
        [MenuItem("Tools/M8/Import review card")]
        public static void ImportReviewCard(){
            var sourceRoot=Argument("--m8-source-root",Path.GetFullPath(Path.Combine(Application.dataPath,"../../..")));
            var source=Path.Combine(sourceRoot,SourceRelative);
            if(!File.Exists(source)){Debug.LogWarning("M8_REVIEW_CARD_MISSING "+source+" — nothing imported; runtime keeps the safe no-texture fallback.");return;}
            var hash=Hash(source);
            if(hash!=SourceSha256)throw new InvalidOperationException("M8 review card hash mismatch: expected "+SourceSha256+" got "+hash);
            Directory.CreateDirectory(Folder);AssetDatabase.Refresh();
            File.Copy(source,TexturePath,true);AssetDatabase.ImportAsset(TexturePath,ImportAssetOptions.ForceSynchronousImport|ImportAssetOptions.ForceUpdate);
            var importer=(TextureImporter)AssetImporter.GetAtPath(TexturePath);
            importer.textureType=TextureImporterType.Default;importer.npotScale=TextureImporterNPOTScale.None;
            importer.mipmapEnabled=false;importer.wrapMode=TextureWrapMode.Clamp;importer.maxTextureSize=2048;
            importer.textureCompression=TextureImporterCompression.Compressed;importer.SaveAndReimport();
            var texture=AssetDatabase.LoadAssetAtPath<Texture2D>(TexturePath);
            var profile=AssetDatabase.LoadAssetAtPath<M8ReviewNotesProfile>(ProfilePath);
            if(profile==null){profile=ScriptableObject.CreateInstance<M8ReviewNotesProfile>();AssetDatabase.CreateAsset(profile,ProfilePath);}
            profile.cardPaper=texture;profile.runtimeApproved=false;
            EditorUtility.SetDirty(profile);AssetDatabase.SaveAssets();
            var evidence=Argument("--m8-evidence-dir",Path.Combine(sourceRoot,"_workspace/current/systems/tech-verification/completeness-m9"));
            Directory.CreateDirectory(evidence);
            File.WriteAllText(Path.Combine(evidence,"import-audit.json"),new JObject{
                ["scope"]="M8 review-note card backing candidate (RFC-CX-011 S-H)",
                ["runtimeApproved"]=profile.runtimeApproved,
                ["card"]=new JObject{["source"]=SourceRelative,["sha256"]=hash,["unityPath"]=TexturePath,["width"]=texture.width,["height"]=texture.height,["maxSize"]=2048},
                ["approvalPath"]="Tools/M8/Approve review card after native readability inspection"
            }.ToString());
            Debug.Log("M8_REVIEW_CARD_IMPORTED "+texture.width+"x"+texture.height+" runtimeApproved=false");
        }
        [MenuItem("Tools/M8/Approve review card")]
        public static void ApproveReviewCard(){
            var profile=AssetDatabase.LoadAssetAtPath<M8ReviewNotesProfile>(ProfilePath);
            if(profile==null||profile.cardPaper==null)throw new InvalidOperationException("M8 review card resources incomplete — run Tools/M8/Import review card first");
            profile.runtimeApproved=true;EditorUtility.SetDirty(profile);AssetDatabase.SaveAssets();
            Debug.Log("M8_REVIEW_CARD_APPROVED runtimeApproved=true");
        }
    }
}
