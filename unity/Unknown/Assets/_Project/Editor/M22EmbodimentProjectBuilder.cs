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

namespace Tide.EditorTools
{
    public static class M22EmbodimentProjectBuilder
    {
        const string Folder="Assets/_Project/Art/M22Seorin";
        const string ProfilePath="Assets/_Project/Resources/M22Embodiment.asset";
        const string SourceRelative="assets/generated/3d/seorin-m22/r01";
        static readonly string[] RequiredFiles={"Seorin_Character.blend","Seorin_Character.glb","Seorin_Character.fbx","Seorin_LeftHand.glb","Seorin_LeftHand.fbx","Seorin_RightHand.glb","Seorin_RightHand.fbx"};
        static readonly string[] HandClips={"Hands_Rest","Hands_Insert","Hands_Align","Hands_Grip","Hands_Seal"};
        static string Argument(string key,string fallback){var args=Environment.GetCommandLineArgs();int i=Array.IndexOf(args,key);return i>=0&&i+1<args.Length?args[i+1]:fallback;}
        static string Hash(string path){using(var sha=SHA256.Create())using(var stream=File.OpenRead(path))return BitConverter.ToString(sha.ComputeHash(stream)).Replace("-","").ToLowerInvariant();}
        static InvalidOperationException Invalid(string message)=>new InvalidOperationException("M22_EMBODIMENT_IMPORT: "+message);
        static JArray ArrayField(JToken token,string key)=>token[key] as JArray??throw Invalid("manifest requires array "+key);
        static Vector3 Vector(JToken token)=>token is JArray values&&values.Count==3?new Vector3((float)values[0],(float)values[1],(float)values[2]):throw Invalid("expected a three-component vector");

        [MenuItem("Tools/M22/Import Seorin embodiment candidate")]
        public static void Import()
        {
            var sourceRoot=Argument("--m22-source-root",Path.GetFullPath(Path.Combine(Application.dataPath,"../../..")));
            var source=Path.Combine(sourceRoot,SourceRelative);var manifestPath=Path.Combine(source,"manifest.json");
            if(!File.Exists(manifestPath))throw Invalid("missing artist manifest: "+manifestPath);
            var manifest=JObject.Parse(File.ReadAllText(manifestPath));
            if((bool?)manifest["runtimeEligible"]!=false)throw Invalid("artist manifest must remain runtimeEligible:false");
            var files=ArrayField(manifest,"files");var rigs=ArrayField(manifest,"rigs");var palette=ArrayField(manifest,"materials");
            // Complete source and hashes are checked before changing any Unity asset. No generated stand-ins.
            foreach(var name in RequiredFiles)
            {
                var entry=files.SingleOrDefault(f=>(string)f["file"]==name)??throw Invalid("missing file receipt: "+name);
                var path=Path.Combine(source,name);if(!File.Exists(path)||new FileInfo(path).Length==0)throw Invalid("missing/empty export: "+path);
                if(!string.Equals(Hash(path),(string)entry["sha256"],StringComparison.OrdinalIgnoreCase))throw Invalid("SHA256 mismatch: "+name);
            }
            foreach(var file in new[]{"Seorin_Character.fbx","Seorin_LeftHand.fbx","Seorin_RightHand.fbx"})
            {
                var rig=rigs.SingleOrDefault(r=>(string)r["file"]==file)??throw Invalid("missing rig: "+file);
                if(string.IsNullOrEmpty((string)rig["rootBone"]))throw Invalid("missing rootBone: "+file);
                var clips=ArrayField(rig,"clips");
                foreach(var clip in file=="Seorin_Character.fbx"?new[]{"Seorin_Idle"}:HandClips)
                    if(clips.Count(c=>(string)c["name"]==clip)!=1)throw Invalid("missing/duplicate clip "+file+" / "+clip);
            }
            if(palette.Count==0)throw Invalid("empty material palette");
            var shader=Shader.Find("Universal Render Pipeline/Lit");if(shader==null)throw Invalid("URP Lit shader unavailable");
            // Close the review gate before replacing referenced assets, including on a failed reimport.
            var profile=AssetDatabase.LoadAssetAtPath<M22EmbodimentProfile>(ProfilePath);
            if(profile!=null){profile.runtimeApproved=false;EditorUtility.SetDirty(profile);AssetDatabase.SaveAssets();}
            Directory.CreateDirectory(Folder);Directory.CreateDirectory("Assets/_Project/Resources");AssetDatabase.Refresh();
            var materials=ImportPalette(palette,shader);
            var character=ImportRig(source,(JObject)rigs.Single(r=>(string)r["file"]=="Seorin_Character.fbx"),materials,20000,65);
            var left=ImportRig(source,(JObject)rigs.Single(r=>(string)r["file"]=="Seorin_LeftHand.fbx"),materials,12000,22);
            var right=ImportRig(source,(JObject)rigs.Single(r=>(string)r["file"]=="Seorin_RightHand.fbx"),materials,12000,22);
            if(left.triangles+right.triangles>12000)throw Invalid("combined hand triangle budget exceeded");
            bool fresh=profile==null;
            if(fresh){profile=ScriptableObject.CreateInstance<M22EmbodimentProfile>();AssetDatabase.CreateAsset(profile,ProfilePath);}
            profile.runtimeApproved=false;profile.character=character.prefab;profile.idle=character.clips["Seorin_Idle"];
            profile.left=Hand(left);profile.right=Hand(right);profile.alignMs=profile.left.align.length*1000;
            if(!profile.Complete)throw Invalid("profile incomplete after import");
            EditorUtility.SetDirty(profile);AssetDatabase.SaveAssets();
            File.Copy(manifestPath,Folder+"/manifest.json",true);AssetDatabase.ImportAsset(Folder+"/manifest.json");
            Directory.CreateDirectory("Builds");
            File.WriteAllText("Builds/m22-embodiment-import-audit.json",new JObject{
                ["rfc"]="RFC-CX-018",["runtimeApproved"]=false,["source"]=SourceRelative,["manifestSha256"]=Hash(manifestPath),
                ["profile"]=ProfilePath,["rigs"]=new JArray(character.audit,left.audit,right.audit),
                ["handsCombinedTriangles"]=left.triangles+right.triangles,["gate"]="--m22-seorin-diagnostic",
                ["alignment"]="Native review required: title facing/framing, reader grip orientation, support offset, foreground wrist/cuff framing. Pose fields remain editable; reimport preserves native alignment."
            }.ToString());
            Debug.Log("M22_EMBODIMENT_IMPORTED runtimeApproved=false; diagnostic --m22-seorin-diagnostic; audit Builds/m22-embodiment-import-audit.json");
        }
        static Dictionary<string,Material> ImportPalette(JArray palette,Shader shader)
        {
            var result=new Dictionary<string,Material>(StringComparer.Ordinal);
            foreach(var item in palette)
            {
                string name=(string)item["name"];
                if(string.IsNullOrEmpty(name)||name.IndexOfAny(Path.GetInvalidFileNameChars())>=0||name.Contains("/")||name.Contains("\\"))throw Invalid("invalid material name");
                var rgba=ArrayField(item,"color");if(rgba.Count!=4)throw Invalid("material color requires RGBA: "+name);
                var path=Folder+"/"+name+".mat";var material=AssetDatabase.LoadAssetAtPath<Material>(path);
                if(material==null){material=new Material(shader);AssetDatabase.CreateAsset(material,path);}
                // Artist palette is linear; Unity's serialized Color property is authored in sRGB.
                material.shader=shader;material.SetColor("_BaseColor",new Color((float)rgba[0],(float)rgba[1],(float)rgba[2],(float)rgba[3]).gamma);
                material.SetFloat("_Metallic",(float?)item["metallic"]??0);material.SetFloat("_Smoothness",1-((float?)item["roughness"]??.7f));
                EditorUtility.SetDirty(material);result.Add(name,material);
            }
            return result;
        }
        sealed class ImportedRig
        {
            public GameObject prefab;
            public Bounds bounds;
            public int triangles;
            public Dictionary<string,AnimationClip> clips;
            public JObject manifest,audit;
        }
        static ImportedRig ImportRig(string source,JObject spec,Dictionary<string,Material> materials,int triangleLimit,int boneLimit)
        {
            string file=(string)spec["file"],path=Folder+"/"+file;
            File.Copy(Path.Combine(source,file),path,true);AssetDatabase.ImportAsset(path,ImportAssetOptions.ForceSynchronousImport|ImportAssetOptions.ForceUpdate);
            var importer=AssetImporter.GetAtPath(path) as ModelImporter;if(importer==null)throw Invalid("not a model: "+path);
            importer.animationType=ModelImporterAnimationType.Generic;importer.avatarSetup=ModelImporterAvatarSetup.CreateFromThisModel;
            importer.motionNodeName=(string)spec["rootBone"];importer.optimizeGameObjects=false;importer.preserveHierarchy=true;
            importer.useFileScale=true;importer.bakeAxisConversion=false;importer.globalScale=1;
            importer.importCameras=false;importer.importLights=false;importer.importAnimation=true;importer.importBlendShapes=false;
            importer.isReadable=false;importer.meshCompression=ModelImporterMeshCompression.Off;importer.animationCompression=ModelImporterAnimationCompression.Off;
            importer.materialImportMode=ModelImporterMaterialImportMode.ImportStandard;
            foreach(var material in materials)importer.AddRemap(new AssetImporter.SourceAssetIdentifier(typeof(Material),material.Key),material.Value);
            importer.SaveAndReimport();
            var takes=importer.defaultClipAnimations;
            var configured=new List<ModelImporterClipAnimation>();
            foreach(var item in ArrayField(spec,"clips"))
            {
                string name=(string)item["name"],take=(string)item["takeName"]??name;
                var original=takes.SingleOrDefault(c=>c.takeName==take||c.name==take);
                if(original==null)throw Invalid("missing FBX take "+take+" in "+file+"; imported: "+string.Join(", ",takes.Select(c=>c.takeName)));
                original.name=name;original.firstFrame=(float?)item["firstFrame"]??original.firstFrame;original.lastFrame=(float?)item["lastFrame"]??original.lastFrame;
                original.loopTime=name=="Seorin_Idle";original.loopPose=false;original.keepOriginalPositionY=true;original.keepOriginalPositionXZ=true;original.keepOriginalOrientation=true;
                original.lockRootPositionXZ=true;original.lockRootHeightY=true;original.lockRootRotation=true;
                original.events=System.Array.Empty<AnimationEvent>();configured.Add(original);
            }
            importer.clipAnimations=configured.ToArray();importer.SaveAndReimport();
            var model=AssetDatabase.LoadAssetAtPath<GameObject>(path);if(model==null)throw Invalid("failed model import: "+file);
            var clips=AssetDatabase.LoadAllAssetsAtPath(path).OfType<AnimationClip>().Where(c=>!c.name.StartsWith("__preview__",StringComparison.Ordinal)).ToDictionary(c=>c.name,StringComparer.Ordinal);
            foreach(var clip in configured)if(!clips.TryGetValue(clip.name,out var animation)||animation.length<=0)throw Invalid("empty/missing animation: "+clip.name);
            var instance=UnityEngine.Object.Instantiate(model);
            try
            {
                var renderers=instance.GetComponentsInChildren<SkinnedMeshRenderer>(true);if(renderers.Length==0)throw Invalid("no skinned mesh: "+file);
                var bones=new HashSet<Transform>();int triangles=0;var bounds=renderers[0].bounds;
                foreach(var renderer in renderers)
                {
                    if(renderer.sharedMesh==null||renderer.bones.Length==0)throw Invalid("unskinned geometry: "+renderer.name);
                    foreach(var bone in renderer.bones)if(bone!=null)bones.Add(bone);
                    var mesh=renderer.sharedMesh;for(int sub=0;sub<mesh.subMeshCount;sub++)triangles+=checked((int)mesh.GetIndexCount(sub)/3);
                    bounds.Encapsulate(renderer.bounds);
                    var slots=renderer.sharedMaterials;
                    for(int i=0;i<slots.Length;i++)
                    {
                        if(slots[i]==null||!materials.TryGetValue(slots[i].name,out var material))throw Invalid("material absent from artist palette: "+renderer.name+" slot "+i);
                        slots[i]=material;
                    }
                    renderer.sharedMaterials=slots;renderer.updateWhenOffscreen=true;
                }
                if(triangles>triangleLimit||bones.Count>boneLimit)throw Invalid("rig exceeds budget: "+file+" triangles="+triangles+" bones="+bones.Count);
                if(!instance.GetComponentsInChildren<Transform>(true).Any(t=>t.name==(string)spec["rootBone"]))throw Invalid("missing root bone: "+file);
                var contact=(string)spec["gripBone"];
                if(file!="Seorin_Character.fbx"&&(string.IsNullOrEmpty(contact)||!instance.GetComponentsInChildren<Transform>(true).Any(t=>t.name==contact)))throw Invalid("missing grip contact bone: "+file);
                var animator=instance.GetComponent<Animator>();if(animator==null||animator.avatar==null||!animator.avatar.isValid)throw Invalid("invalid Generic avatar: "+file);
                animator.applyRootMotion=false;animator.runtimeAnimatorController=null;
                var prefabPath=Folder+"/"+Path.GetFileNameWithoutExtension(file)+".prefab";
                var prefab=PrefabUtility.SaveAsPrefabAsset(instance,prefabPath);
                return new ImportedRig{prefab=prefab,bounds=bounds,triangles=triangles,clips=clips,manifest=spec,audit=new JObject{
                    ["file"]=file,["sha256"]=Hash(Path.Combine(source,file)),["prefab"]=prefabPath,["triangles"]=triangles,["deformBones"]=bones.Count,
                    ["boundsCenter"]=JArray.FromObject(new[]{bounds.center.x,bounds.center.y,bounds.center.z}),["boundsSize"]=JArray.FromObject(new[]{bounds.size.x,bounds.size.y,bounds.size.z}),
                    ["clips"]=new JArray(clips.Keys),["gripBone"]=contact,["rootMotion"]=false
                }};
            }
            finally{UnityEngine.Object.DestroyImmediate(instance);}
        }
        static M22EmbodimentProfile.HandRig Hand(ImportedRig rig)
        {
            return new M22EmbodimentProfile.HandRig{
                prefab=rig.prefab,rest=rig.clips["Hands_Rest"],insert=rig.clips["Hands_Insert"],align=rig.clips["Hands_Align"],grip=rig.clips["Hands_Grip"],seal=rig.clips["Hands_Seal"],
                contactBone=(string)rig.manifest["gripBone"],contactPoint=Vector(rig.manifest["gripPoint"]),
                gripPoseTime=(float?)rig.manifest["gripPoseTime"]??rig.clips["Hands_Grip"].length
            };
        }
    }
}
#endif
