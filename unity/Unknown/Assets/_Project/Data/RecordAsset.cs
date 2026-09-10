using UnityEngine;
namespace Tide.Data
{
    public sealed class RecordAsset : ScriptableObject
    {
        [SerializeField] private string recordId;
        [SerializeField,TextArea] private string generatedJson;
        public string RecordId => recordId;
        public string GeneratedJson => generatedJson;
#if UNITY_EDITOR
        public void Configure(string id,string sourceJson) { recordId=id; generatedJson=sourceJson; }
#endif
    }
}

