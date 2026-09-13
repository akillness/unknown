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
    // M20 lane WorkSurface: imports the parent-owned GTI archival work-surface candidate
    // (assets/generated/2d/ui/m20-archival-work-surface-r02.png) as a diagnostic candidate and never approves it.
    // Contract: _workspace/current/presentation/t0-work-surface-m20.md
    //
    // Differences from Editor/M7UiSkinProjectBuilder.cs, both deliberate (contract §2 non-tile rule):
    //  * wrapMode = Clamp, not Repeat  -> repeating the surface is impossible at the engine level (T4)
    //  * maxTextureSize = 2048         -> 1672x941 is kept as one full-bleed composition, not downsampled to a tile (T1)
    // r01 (m19-interview-control-surface-r01) is deliberately NOT imported: it stays unapproved and unintegrated (N13).
    public static class M20WorkSurfaceProjectBuilder {
        const string Folder="Assets/_Project/Art/Candidates/m20-ui-r02";
        const string ProfilePath="Assets/_Project/Resources/M20WorkSurface.asset";
        const string AuditPath="Builds/m20-work-surface-import-audit.json";
        const string SourceRelative="assets/generated/2d/ui/m20-archival-work-surface-r02.png";
        const string ProvenanceRelative="assets/generated/2d/ui/provenance.json";
        const string ProvenanceId="m20-archival-work-surface-r02";
        const string Target="UI_M20_ArchivalWorkSurface.png";
        static string TexturePath=>Folder+"/"+Target;
        static string Argument(string key,string fallback){var args=Environment.GetCommandLineArgs();int i=Array.IndexOf(args,key);return i>=0&&i+1<args.Length?args[i+1]:fallback;}
        static string Hash(string path){using(var sha=SHA256.Create())using(var stream=File.OpenRead(path))return BitConverter.ToString(sha.ComputeHash(stream)).Replace("-","").ToLowerInvariant();}
        [MenuItem("Tools/M20/Import work surface candidate")]
        public static void ImportWorkSurface(){
            var sourceRoot=Argument("--m20-source-root",Path.GetFullPath(Path.Combine(Application.dataPath,"../../..")));
            var source=Path.Combine(sourceRoot,SourceRelative);
            if(!File.Exists(source)){Debug.LogWarning("M20_SOURCE_MISSING "+source+" — nothing imported; the runtime keeps the committed M7 work-surface path.");return;}
            // Provenance is read-only here: the hash must match before a single byte is copied, and
            // runtimeEligible must still be false. M20 never edits assets/generated/** (contract N15).
            var provenance=JObject.Parse(File.ReadAllText(Path.Combine(sourceRoot,ProvenanceRelative)));
            var entry=provenance["assets"].FirstOrDefault(a=>(string)a["id"]==ProvenanceId);
            if(entry==null)throw new InvalidOperationException("M20 provenance entry missing: "+ProvenanceId);
            var expected=(string)entry["output_sha256"];
            var hash=Hash(source);
            if(hash!=expected)throw new InvalidOperationException("M20 work surface hash mismatch: expected "+expected+" got "+hash);
            if((bool)entry["runtimeEligible"])throw new InvalidOperationException("M20 refuses to import a promoted source: runtimeEligible must stay false");
            Directory.CreateDirectory(Folder);AssetDatabase.Refresh();
            File.Copy(source,TexturePath,true);
            AssetDatabase.ImportAsset(TexturePath,ImportAssetOptions.ForceSynchronousImport|ImportAssetOptions.ForceUpdate);
            var importer=(TextureImporter)AssetImporter.GetAtPath(TexturePath);
            importer.textureType=TextureImporterType.Default;importer.sRGBTexture=true;
            importer.npotScale=TextureImporterNPOTScale.None;importer.mipmapEnabled=false;
            importer.wrapMode=TextureWrapMode.Clamp;importer.maxTextureSize=2048;
            importer.textureCompression=TextureImporterCompression.Compressed;importer.SaveAndReimport();
            var texture=AssetDatabase.LoadAssetAtPath<Texture2D>(TexturePath);
            var profile=AssetDatabase.LoadAssetAtPath<M20WorkSurfaceProfile>(ProfilePath);
            if(profile==null){profile=ScriptableObject.CreateInstance<M20WorkSurfaceProfile>();AssetDatabase.CreateAsset(profile,ProfilePath);}
            profile.workSurface=texture;profile.tint=Color.white;
            // Candidate only. There is deliberately NO approval menu in this builder.
            profile.runtimeApproved=false;
            EditorUtility.SetDirty(profile);AssetDatabase.SaveAssets();
            Directory.CreateDirectory("Builds");
            File.WriteAllText(AuditPath,new JObject{
                ["scope"]="M20 archival work-surface diagnostic candidate (single full-bleed, non-tiled)",
                ["runtimeApproved"]=profile.runtimeApproved,
                ["sourceRuntimeEligible"]=(bool)entry["runtimeEligible"],
                ["profile"]=ProfilePath,
                ["source"]=SourceRelative,
                ["sourceSha256"]=hash,
                ["provenanceId"]=ProvenanceId,
                ["unityPath"]=TexturePath,
                ["width"]=texture.width,
                ["height"]=texture.height,
                ["maxSize"]=2048,
                ["sRGB"]=importer.sRGBTexture,
                ["wrapMode"]=importer.wrapMode.ToString(),
                ["tiled"]=false,
                ["gate"]="--m20-ui-surface-diagnostic (or profile.diagnosticOverride in-memory); runtimeApproved stays false",
                ["r01"]="m19-interview-control-surface-r01 deliberately not imported — unapproved, unintegrated",
                ["approvalPath"]="director-only promotion via decision-log audit; no approval menu exists in this builder"
            }.ToString());
            Debug.Log("M20_WORK_SURFACE_IMPORTED "+texture.width+"x"+texture.height+" wrap="+importer.wrapMode+" runtimeApproved=false");
        }
    }
}
#endif
