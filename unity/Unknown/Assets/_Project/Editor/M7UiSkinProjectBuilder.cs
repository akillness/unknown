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
    // RFC-CX-013 lane UiSkin: imports the M7 rag-paper / bronze basecolors as uGUI skin candidates and never approves them.
    public static class M7UiSkinProjectBuilder {
        const string Folder="Assets/_Project/Art/Candidates/m7-ui-r01";
        const string ProfilePath="Assets/_Project/Resources/M7UiSkin.asset";
        const string AuditPath="Builds/m7-ui-import-audit.json";
        const string SourceFile="basecolor.png";
        sealed class Candidate {
            public string Slot,SourceFolder,Target;
            public string SourceRelative=>SourceFolder+"/"+SourceFile;
            public string TexturePath=>Folder+"/"+Target;
        }
        static readonly Candidate[] Candidates={
            new Candidate{Slot="paperPanel",SourceFolder="assets/generated/2d/texture/m7-rag-paper-r01",Target="UI_M7_PaperPanel.png"},
            new Candidate{Slot="bronzeFrame",SourceFolder="assets/generated/2d/texture/m7-bronze-r01",Target="UI_M7_BronzeFrame.png"}
        };
        static string Argument(string key,string fallback){var args=Environment.GetCommandLineArgs();int i=Array.IndexOf(args,key);return i>=0&&i+1<args.Length?args[i+1]:fallback;}
        static string Hash(string path){using(var sha=SHA256.Create())using(var stream=File.OpenRead(path))return BitConverter.ToString(sha.ComputeHash(stream)).Replace("-","").ToLowerInvariant();}
        [MenuItem("Tools/M7/Import UI skin candidates")]
        public static void ImportUiSkin(){
            var sourceRoot=Argument("--m7-source-root",Path.GetFullPath(Path.Combine(Application.dataPath,"../../..")));
            foreach(var candidate in Candidates){
                var source=Path.Combine(sourceRoot,candidate.SourceRelative);
                if(!File.Exists(source)){Debug.LogWarning("M7_UI_SOURCE_MISSING "+source+" — nothing imported; runtime keeps the committed literal UI colours.");return;}
            }
            Directory.CreateDirectory(Folder);AssetDatabase.Refresh();
            var textures=new JArray();
            var profile=AssetDatabase.LoadAssetAtPath<M7UiSkinProfile>(ProfilePath);
            if(profile==null){profile=ScriptableObject.CreateInstance<M7UiSkinProfile>();AssetDatabase.CreateAsset(profile,ProfilePath);}
            foreach(var candidate in Candidates){
                var provenance=JObject.Parse(File.ReadAllText(Path.Combine(sourceRoot,candidate.SourceFolder,"provenance.json")));
                var texture=ImportTexture(sourceRoot,candidate,provenance,textures);
                if(candidate.Slot=="paperPanel")profile.paperPanel=texture;
                else if(candidate.Slot=="bronzeFrame")profile.bronzeFrame=texture;
            }
            // Candidate only: approval is a separate director-only menu item (Tools/M7/Approve UI skin).
            profile.runtimeApproved=false;
            EditorUtility.SetDirty(profile);AssetDatabase.SaveAssets();
            Directory.CreateDirectory("Builds");
            File.WriteAllText(AuditPath,new JObject{
                ["scope"]="M7 rag-paper / bronze uGUI skin candidate (RFC-CX-013 UiSkin)",
                ["runtimeApproved"]=profile.runtimeApproved,
                ["profile"]=ProfilePath,
                ["textures"]=textures,
                ["approvalPath"]="Tools/M7/Approve UI skin (director only) after native inspection of the Header / Toolbar / Work Surface backings"
            }.ToString());
            Debug.Log("M7_UI_SKIN_IMPORTED textures="+textures.Count+" runtimeApproved=false");
        }
        static Texture2D ImportTexture(string sourceRoot,Candidate candidate,JObject provenance,JArray audit){
            var sourceRelative=candidate.SourceRelative;
            var source=Path.Combine(sourceRoot,sourceRelative);
            if(!File.Exists(source))throw new InvalidOperationException("M7 UI skin source missing: "+source);
            var expected=(string)provenance["assets"].First(a=>(string)a["file"]==SourceFile)["sha256"];
            var hash=Hash(source);
            if(hash!=expected)throw new InvalidOperationException("M7 UI skin texture hash mismatch for "+sourceRelative+": expected "+expected+" got "+hash);
            var target=candidate.TexturePath;
            File.Copy(source,target,true);AssetDatabase.ImportAsset(target,ImportAssetOptions.ForceSynchronousImport|ImportAssetOptions.ForceUpdate);
            // Import rules mirror Editor/M8ReviewNotesProjectBuilder.cs (UI texture: NPOT kept, no mips, compressed) as a single sRGB Sprite;
            // wrap is Repeat because UI/T0Interface.cs tiles the backing through RawImage.uvRect (paperTilesAcross / frameTilesAcross).
            var importer=(TextureImporter)AssetImporter.GetAtPath(target);
            importer.textureType=TextureImporterType.Sprite;importer.spriteImportMode=SpriteImportMode.Single;
            importer.sRGBTexture=true;importer.npotScale=TextureImporterNPOTScale.None;importer.mipmapEnabled=false;
            importer.wrapMode=TextureWrapMode.Repeat;importer.maxTextureSize=1024;
            importer.textureCompression=TextureImporterCompression.Compressed;importer.SaveAndReimport();
            var texture=AssetDatabase.LoadAssetAtPath<Texture2D>(target);
            audit.Add(new JObject{["source"]=sourceRelative,["sha256"]=hash,["unityPath"]=target,["width"]=texture.width,["height"]=texture.height,["maxSize"]=1024,["sRGB"]=importer.sRGBTexture});
            return texture;
        }
        [MenuItem("Tools/M7/Approve UI skin (director only)")]
        public static void ApproveUiSkin(){
            var profile=AssetDatabase.LoadAssetAtPath<M7UiSkinProfile>(ProfilePath);
            if(profile==null||profile.paperPanel==null||profile.bronzeFrame==null)throw new InvalidOperationException("M7 UI skin resources incomplete — run Tools/M7/Import UI skin candidates first");
            profile.runtimeApproved=true;EditorUtility.SetDirty(profile);AssetDatabase.SaveAssets();
            Debug.Log("M7_UI_SKIN_APPROVED runtimeApproved=true");
        }
    }
}
#endif
