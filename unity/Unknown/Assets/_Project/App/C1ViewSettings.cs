using UnityEngine;
namespace Tide.App
{
    public sealed class C1ViewSettings:ScriptableObject
    {
        public GameObject panel;
        public Vector3 cameraPosition,lookAt;
        public float horizontalFov;
        public Color background;
        public bool runtimeApproved;
    }
}
