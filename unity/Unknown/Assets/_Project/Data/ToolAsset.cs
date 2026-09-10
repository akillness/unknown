using UnityEngine;
namespace Tide.Data
{
    public sealed class ToolAsset : ScriptableObject
    {
        [SerializeField] private string toolId;
        [SerializeField,TextArea] private string generatedJson;
        public string ToolId => toolId;
        public string GeneratedJson => generatedJson;
#if UNITY_EDITOR
        public void Configure(string id,string sourceJson) { toolId=id; generatedJson=sourceJson; }
#endif
    }
}

