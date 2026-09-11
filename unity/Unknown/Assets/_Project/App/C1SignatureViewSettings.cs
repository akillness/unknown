using UnityEngine;
namespace Tide.App
{
    public sealed class C1SignatureViewSettings:ScriptableObject
    {
        public GameObject reader;
        public Texture2D paper;
        public Vector3 cameraPosition,lookAt;
        public float horizontalFov=54;
        public Color background=new Color(.035f,.065f,.075f);
        public bool runtimeApproved;
    }
}
