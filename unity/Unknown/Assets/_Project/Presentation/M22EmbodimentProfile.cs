using System;
using UnityEngine;

namespace Tide.Presentation
{
    [CreateAssetMenu(menuName="Unknown/M22 Embodiment")]
    public sealed class M22EmbodimentProfile:ScriptableObject
    {
        public bool runtimeApproved;
        [NonSerialized] public bool diagnosticOverride;
        public GameObject character;
        public AnimationClip idle;
        public HandRig left=new HandRig(),right=new HandRig();
        public Vector3 characterPosition=new Vector3(0,-.023f,.038605f),characterEuler=new Vector3(0,180,0);
        public Vector3 cameraPosition=new Vector3(0,1.4f,-1.65f),lookAt=new Vector3(0,1.32f,0);
        public float horizontalFov=50;
        public Color background=new Color(.055f,.12f,.15f);
        public float readerLeftScale=1,readerRightScale=1;
        public HandPose foregroundLeft=new HandPose{position=new Vector3(-.24f,-.23f,.7f),euler=new Vector3(-65,0,-20)};
        public HandPose foregroundRight=new HandPose{position=new Vector3(.24f,-.23f,.7f),euler=new Vector3(-65,0,20)};
        public Vector3 readerGripEuler=new Vector3(0,0,345);
        [Tooltip("Crank-handle mesh-local coordinates, including the imported hierarchy scale.")]
        public Vector3 readerGripOffset;
        public bool rotateGripWithHandle;
        public string crankHandlePrefix="rd-crank-handle";
        public string readerSupportPrefix="rd-hex-plate";
        [Tooltip("Support mesh-local coordinates, including the imported hierarchy scale.")]
        public Vector3 readerSupportOffset;
        public Vector3 readerSupportEuler=new Vector3(0,0,334.44f);
        [Tooltip("Camera-relative metres from support contact to the left hand's ready pose; not mesh-local coordinates.")]
        public Vector3 readerLeftRestOffset=new Vector3(-.08f,-.06f,-.25f);
        public float handRecoveryMs=150;
        public float insertMs=250,insertContactMs=200,sealMs=300,sealContactMs=150;
        public float alignMs;
        public bool showReaderHands=true,showCircuitHands=true,showRecordHands=true;

        [Serializable] public sealed class HandRig
        {
            public GameObject prefab;
            public AnimationClip rest,insert,align,grip,seal;
            public string contactBone;
            public Vector3 contactPoint;
            public float gripPoseTime;
            public bool Complete=>prefab!=null&&rest!=null&&insert!=null&&align!=null&&grip!=null&&seal!=null&&!string.IsNullOrEmpty(contactBone);
        }
        [Serializable] public sealed class HandPose
        {
            public Vector3 position,euler;
            public float scale=1;
        }
        public bool Complete=>character!=null&&idle!=null&&left!=null&&right!=null&&left.Complete&&right.Complete&&alignMs>0;
    }
}
