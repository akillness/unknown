using UnityEngine;
namespace Tide.Data
{
    public sealed class ZoneAsset : ScriptableObject
    {
        [SerializeField] private string zoneId;
        [SerializeField,TextArea] private string generatedJson;
        public string ZoneId => zoneId;
        public string GeneratedJson => generatedJson;
#if UNITY_EDITOR
        public void Configure(string id,string sourceJson) { zoneId=id; generatedJson=sourceJson; }
#endif
    }
}

