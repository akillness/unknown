using UnityEngine;
namespace Tide.Presentation {
    // RFC-CX-013 lane ReaderStage: M7 optical reader 3D stage candidate (Art/Candidates/m7-reader-r01).
    // runtimeApproved stays false until the director approves via Tools/M7/Approve reader stage; diagnostic flag --m7-reader-diagnostic.
    // Camera is fixed after entry (presentation/cinematic-gameplay-m6.md:11: native camera never pans/zooms/shakes).
    [CreateAssetMenu(menuName="Unknown/M7 Reader Stage")]
    public sealed class M7ReaderStageProfile:ScriptableObject {
        public bool runtimeApproved;
        public GameObject reader;
        public GameObject recordSet;
        // Stage space: reader prefab at origin, bench top = y 0. Blender (x,y,z) -> Unity (x,z,y); values are placeholders the builder overwrites from imported bounds.
        public Vector3 readerLocalPosition=Vector3.zero;
        public Vector3 recordSetLocalPosition=new Vector3(-.87f,0f,.13f);
        public Vector3 cameraPosition=new Vector3(-.55f,.62f,-.75f);
        public Vector3 lookAt=new Vector3(.15f,.2f,.35f);
        public float horizontalFov=42f;
        // style-guide.md: deep ink #0E1F26 background, ochre #E2AF62 lamp (warm, small), wet metal #36565C cool fill.
        public Color background=new Color(14/255f,31/255f,38/255f);
        public Color lampColor=new Color(226/255f,175/255f,98/255f);
        public float lampIntensity=2.2f;
        public float lampRange=3f;
        public Vector3 lampOffset=new Vector3(-.6f,.9f,-.2f);
        public Color fillColor=new Color(54/255f,86/255f,92/255f);
        public float fillIntensity=.6f;
        public float fillRange=5f;
        public Vector3 fillOffset=new Vector3(1.2f,1.6f,-1f);
        // Crank pivot created by the builder; stroke timings/angle live in Resources/T0ReaderVfx.json (<=150deg per RFC-CX-009 evidence).
        public string crankPivotName="M7 crank pivot";
    }
}
