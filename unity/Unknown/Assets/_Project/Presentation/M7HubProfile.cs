using UnityEngine;
namespace Tide.Presentation {
    // RFC-CX-013 lane HubShell: M7 concept-first watchroom shell candidate (Art/Candidates/m7-hub-r01).
    // runtimeApproved stays false until the director approves via Tools/M7/Approve hub shell; the diagnostic flag is --m7-hub-diagnostic.
    [CreateAssetMenu(menuName="Unknown/M7 Hub")]
    public sealed class M7HubProfile:ScriptableObject {
        public bool runtimeApproved;
        public Material floorAndWall;
        public Material workbench;
        public Material plateShelf;
        // style-guide.md: wet metal #36565C as flat ambient, ochre accent #E2AF62 on the single work lamp (warm, ≤8%).
        public Color ambient=new Color(.212f,.337f,.361f);
        public Color lampColor=new Color(.886f,.686f,.384f);
        public float lampIntensity=1.4f;
        public float lampRange=4f;
        public Vector3 lampLocalPosition=new Vector3(-.7f,2.3f,1.1f);
        // Committed hub root names (Editor/T0ProjectBuilder.cs:50-53,77-81); serialized so a later revision retargets without code.
        public string[] floorAndWallRoots={"Authored room floor","Authored back wall"};
        public string[] workbenchRoots={"Workbench left jamb","Workbench right jamb","Workbench above drawer","Workbench below drawer","Workbench behind drawer"};
        public string[] plateShelfRoots={"Authored plate shelf"};
    }
}
