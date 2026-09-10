#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using Tide.Data;
using UnityEditor;
using UnityEngine;

namespace Tide.EditorTools
{
    public static class T0AssetImporter
    {
        public const string TablePath="Assets/_Project/Data/Tables";
        public const string CatalogPath="Assets/_Project/Data/Authoring/T0Catalog.asset";
        [MenuItem("Tools/T0/Import generated tables")]
        public static void Import()
        {
            var files=Directory.GetFiles(TablePath,"*.json").Where(p=>Path.GetFileName(p)!="tables-receipt.json")
                .ToDictionary(p=>Path.GetFileName(p),File.ReadAllBytes,StringComparer.Ordinal);
            var receiptBytes=File.ReadAllBytes(TablePath+"/tables-receipt.json");
            var definition=T0DataLoader.LoadVerified(files,receiptBytes,receiptBytes);
            var campaignPath=Path.GetFullPath(Path.Combine(Application.dataPath,"../../../_workspace/current/planning/campaign.json"));
            var receipt=JsonUtility.FromJson<ReceiptJson>(System.Text.Encoding.UTF8.GetString(receiptBytes));
            if(!File.Exists(campaignPath) || ReceiptVerifier.Sha256(File.ReadAllBytes(campaignPath))!=receipt.source.sha256)
                throw new InvalidOperationException("Producer source differs from campaign.json");
            Directory.CreateDirectory("Assets/_Project/Data/Authoring");
            AssetDatabase.Refresh();
            var tableAssets=files.Keys.OrderBy(x=>x,StringComparer.Ordinal).Select(n=>AssetDatabase.LoadAssetAtPath<TextAsset>(TablePath+"/"+n)).ToArray();
            var receiptAsset=AssetDatabase.LoadAssetAtPath<TextAsset>(TablePath+"/tables-receipt.json");
            var catalog=AssetDatabase.LoadAssetAtPath<T0CatalogAsset>(CatalogPath);
            if(catalog==null) { catalog=ScriptableObject.CreateInstance<T0CatalogAsset>(); AssetDatabase.CreateAsset(catalog,CatalogPath); }
            catalog.Configure(tableAssets,receiptAsset);
            EditorUtility.SetDirty(catalog);
            foreach(var record in definition.Records.Values)
            {
                var asset=GetOrCreate<RecordAsset>("Records/"+record.Id);
                asset.Configure(record.Id,System.Text.Encoding.UTF8.GetString(files["records.json"]));
                EditorUtility.SetDirty(asset);
            }
            var zones=JsonUtility.FromJson<ZonesJson>(System.Text.Encoding.UTF8.GetString(files["zones.json"]));
            foreach(var zone in zones.rows)
            {
                var asset=GetOrCreate<ZoneAsset>("Zones/"+zone.zoneId);
                asset.Configure(zone.zoneId,System.Text.Encoding.UTF8.GetString(files["zones.json"]));
                EditorUtility.SetDirty(asset);
            }
            var tools=JsonUtility.FromJson<ToolsJson>(System.Text.Encoding.UTF8.GetString(files["tools.json"]));
            foreach(var tool in tools.rows)
            {
                var asset=GetOrCreate<ToolAsset>("Tools/"+tool.toolId);
                asset.Configure(tool.toolId,System.Text.Encoding.UTF8.GetString(files["tools.json"]));
                EditorUtility.SetDirty(asset);
            }
            AssetDatabase.SaveAssets();
            Debug.Log("T0 diagnostic catalog imported. Citation blockers: "+T0DataLoader.CitationBlockers(definition).Count);
        }
        private static T GetOrCreate<T>(string relative) where T:ScriptableObject
        {
            var path="Assets/_Project/Data/Authoring/"+relative+".asset";
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            AssetDatabase.Refresh();
            var asset=AssetDatabase.LoadAssetAtPath<T>(path);
            if(asset==null) { asset=ScriptableObject.CreateInstance<T>(); AssetDatabase.CreateAsset(asset,path); }
            return asset;
        }
    }
}
#endif
