#!/usr/bin/env python3
"""write_provenance.py — assets/generated/3d/provenance.json 생성기.
provenance.json 은 손으로 쓰지 않는다(assets/README.md 규칙). 해시·시각은 파일에서 읽는다.
owner: game-modeler / cycle: 20260909-preproduction-c4
"""
import os, json, hashlib, datetime

OUT3D = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
SRC = "assets/generated/3d/scripts/build_hub_greybox.py"
TOOL = "Blender 5.1.2 via MCP"
CLAIM = "[OBSERVED] greybox; not final art; not gameplay"
LICENSE = "original greybox"

# stage_60_measure() 계산값 [OBSERVED] — 오브젝트 12개 x 12 tris = 144
TRIS = 12
DIMS = {
    "SM_Hub_Floor": [6.00, 8.00, 0.10], "SM_Hub_Wall_N": [6.00, 0.12, 2.60],
    "SM_Hub_Wall_W": [0.12, 8.00, 2.60], "SM_Hub_Wall_E": [0.12, 8.00, 2.60],
    "SM_Hub_Workbench": [2.40, 0.90, 0.90], "SM_Hub_PlateShelf": [0.36, 1.80, 1.15],
    "SM_Tool_reader": [0.80, 0.50, 0.34], "SM_Tool_seal": [0.70, 0.44, 0.22],
    "SM_Tool_circuit": [2.00, 0.09, 1.20], "SM_Tool_routing": [0.09, 1.60, 1.00],
    "SM_Tool_alignment": [1.20, 0.60, 1.00], "SM_Tool_corrosion": [1.00, 0.70, 0.85],
}
TOOL_IDS = ["reader", "seal", "circuit", "routing", "alignment", "corrosion"]


def sha256(p):
    h = hashlib.sha256()
    with open(p, "rb") as f:
        for b in iter(lambda: f.read(1 << 20), b""):
            h.update(b)
    return h.hexdigest()


def mtime_iso(p):
    return datetime.datetime.fromtimestamp(
        os.path.getmtime(p), datetime.timezone.utc).isoformat()


def entry(rel, asset_id, method, extra=None):
    p = os.path.join(OUT3D, rel)
    e = {"id": rel.replace("/", "-").replace(".", "-"), "file": rel, "asset_id": asset_id,
         "tool": TOOL, "method": method, "source_script": SRC,
         "sha256": sha256(p), "bytes": os.path.getsize(p), "generated_at": mtime_iso(p),
         "license": LICENSE, "runtimeEligible": False, "promoted_by": None, "claim": CLAIM}
    if extra:
        e.update(extra)
    return e


def main():
    a = []
    a.append(entry("scripts/build_hub_greybox.py", "SM_Hub_Greybox", "authoring script (python/bpy)",
                   {"claim": "[OBSERVED] source script actually executed via Blender MCP"}))
    a.append(entry("hub-greybox.blend", "SM_Hub_Greybox", "primitive greybox",
                   {"note": "scene source; collection HUB_GREYBOX; user default Cube/Light/Camera hidden, not deleted"}))
    a.append(entry("hub-greybox.glb", "SM_Hub_Greybox", "primitive greybox",
                   {"export": {"operator": "bpy.ops.export_scene.gltf", "format": "GLB",
                               "export_apply": True, "export_yup": True, "use_selection": True},
                    "objects": sorted(DIMS.keys()), "tris_total": TRIS * len(DIMS),
                    "dims_m": DIMS, "units": "1 blender unit = 1 m"}))
    a.append(entry("hub-greybox.fbx", "SM_Hub_Greybox", "primitive greybox",
                   {"export": {"operator": "bpy.ops.export_scene.fbx", "axis_up": "Y",
                               "axis_forward": "-Z", "global_scale": 1.0,
                               "apply_scale_options": "FBX_SCALE_NONE", "apply_unit_scale": True,
                               "bake_space_transform": False, "object_types": ["MESH"],
                               "mesh_smooth_type": "FACE"},
                    "tris_total": TRIS * len(DIMS)}))
    for t in TOOL_IDS:
        n = "SM_Tool_" + t
        a.append(entry(n + ".glb", n, "primitive blockout",
                       {"export": {"operator": "bpy.ops.export_scene.gltf", "format": "GLB",
                                   "export_apply": True, "export_yup": True, "use_selection": True},
                        "dims_m": DIMS[n], "tris": TRIS,
                        "pivot": "base center (도구 원점) — 프롭을 월드 원점으로 옮겨 내보낸 뒤 씬 위치 복원",
                        "tool_id": t,
                        "note": "greybox proxy volume only; no mechanism, no readable surface"}))
    a.append(entry("renders/hub-cam.png", "SM_Hub_Greybox", "EEVEE render (previz)",
                   {"render": {"engine": "BLENDER_EEVEE", "resolution": [1920, 1080],
                               "samples": 64, "camera": "CAM_Hub_Fixed", "lens_mm": 34.0,
                               "pitch_deg": 35.0, "camera_location_m": [0.0, -5.3, 5.501],
                               "aim_m": [0.0, 1.20, 0.95]},
                    "claim": "[OBSERVED] previz render of greybox; not final art; not gameplay"}))
    for i in range(12):
        rel = "renders/turntable/frame_%02d.png" % i
        a.append(entry(rel, "SM_Hub_Greybox", "EEVEE render (previz turntable)",
                       {"render": {"engine": "BLENDER_EEVEE", "resolution": [960, 540],
                                   "samples": 64, "camera": "CAM_Hub_Turntable",
                                   "orbit_deg": i * 30, "radius_m": 6.5,
                                   "aim_m": [0.0, 1.20, 0.95]},
                        "note": "3면 벽·천장 없음 구조라 북측(orbit 90~270) 프레임은 근접 벽이 하단을 가린다",
                        "claim": "[OBSERVED] previz render of greybox; not final art; not gameplay"}))
    doc = {"schema": "provenance/v1", "folder": "assets/generated/3d",
           "generator": "assets/generated/3d/scripts/write_provenance.py",
           "written_at": datetime.datetime.now(datetime.timezone.utc).isoformat(),
           "owner": "game-modeler", "cycle": "20260909-preproduction-c4",
           "promotion_rule": "runtimeEligible 는 production/decision-log.md 감사로만 true 가 된다 (CLAUDE.md 9)",
           "assets": a}
    p = os.path.join(OUT3D, "provenance.json")
    json.dump(doc, open(p, "w"), indent=2, ensure_ascii=False)
    print(json.dumps({"written": p, "entries": len(a)}, ensure_ascii=False))


if __name__ == "__main__":
    main()
