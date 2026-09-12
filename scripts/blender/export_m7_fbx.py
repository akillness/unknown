"""Export the 4 M7 collections as FBX import candidates for Unity.

GLB stays the handoff canon (glb-export-report.json). Unity 6000.5.6f1 has no built-in
glTF importer and this repo's existing Art/Candidates use FBX, so FBX is the *import-time*
candidate format. Same Smart-UV + material rewire as export_m7_glb.py (box projection is
not representable outside Blender). FBX args follow handoff/asset-runbook.md §1.2.

Run (repo root):
  Blender --background --factory-startup --python scripts/blender/export_m7_fbx.py
"""
import bpy, os, sys, json, math, hashlib

ROOT = os.getcwd()
OUT_DIR = os.path.join(ROOT, "assets", "generated", "3d", "concept-first-m7")
FBX_DIR = os.path.join(OUT_DIR, "fbx")
BLEND = os.path.join(OUT_DIR, "concept-first-m7.blend")
REPORT = os.path.join(FBX_DIR, "fbx-export-report.json")
os.makedirs(FBX_DIR, exist_ok=True)

TARGETS = [
    ("M7-ENV-watchroom", "SM_Env_Watchroom.fbx"),
    ("M7-PROP-optical-reader", "SM_Prop_OpticalReader.fbx"),
    ("M7-PROP-record-set", "SM_Prop_RecordSet.fbx"),
    ("M7-ENV-gate-three", "SM_Env_GateThree.fbx"),
]

def sha256(p):
    h = hashlib.sha256()
    with open(p, "rb") as f:
        for chunk in iter(lambda: f.read(1 << 20), b""):
            h.update(chunk)
    return h.hexdigest()

def rewire_materials_for_uv(materials):
    n = 0
    for mat in materials:
        if not mat or not mat.use_nodes:
            continue
        nt = mat.node_tree
        for node in nt.nodes:
            if node.type == 'TEX_IMAGE' and node.projection == 'BOX':
                node.projection = 'FLAT'; n += 1
        for link in list(nt.links):
            if link.from_node.type == 'TEX_COORD' and link.from_socket.name == 'Object':
                to_sock = link.to_socket
                uv_out = link.from_node.outputs['UV']
                nt.links.remove(link)
                nt.links.new(uv_out, to_sock)
    return n

def smart_uv(meshes):
    if not meshes:
        return 0
    bpy.ops.object.select_all(action='DESELECT')
    for o in meshes:
        o.hide_set(False); o.select_set(True)
    bpy.context.view_layer.objects.active = meshes[0]
    bpy.ops.object.mode_set(mode='EDIT')
    bpy.ops.mesh.select_all(action='SELECT')
    bpy.ops.uv.smart_project(angle_limit=math.radians(66.0), island_margin=0.02)
    bpy.ops.object.mode_set(mode='OBJECT')
    return len(meshes)

def fbx_kwargs(filepath):
    wanted = dict(
        filepath=filepath, use_selection=True, global_scale=1.0, apply_unit_scale=True,
        apply_scale_options='FBX_SCALE_NONE', axis_forward='-Z', axis_up='Y',
        bake_space_transform=False, object_types={'MESH'}, use_mesh_modifiers=True,
        mesh_smooth_type='FACE', path_mode='COPY', embed_textures=False,
        use_custom_props=False, add_leaf_bones=False, bake_anim=False,
    )
    props = bpy.ops.export_scene.fbx.get_rna_type().properties.keys()
    kept = {k: v for k, v in wanted.items() if k == "filepath" or k in props}
    return kept, sorted(set(wanted) - set(kept))

def export_target(coll_name, fbx_name):
    bpy.ops.wm.open_mainfile(filepath=BLEND)
    coll = bpy.data.collections.get(coll_name)
    if coll is None:
        raise RuntimeError(f"collection not found: {coll_name}")
    objs = list(coll.objects)
    meshes = [o for o in objs if o.type == 'MESH']
    uv = smart_uv(meshes)
    mats = {s.material for o in meshes for s in o.material_slots if s.material}
    rewired = rewire_materials_for_uv(mats)
    bpy.ops.object.select_all(action='DESELECT')
    for o in meshes:
        o.select_set(True)
    bpy.context.view_layer.objects.active = meshes[0]
    fp = os.path.join(FBX_DIR, fbx_name)
    kw, dropped = fbx_kwargs(fp)
    bpy.ops.export_scene.fbx(**kw)
    tris = sum(sum(len(p.vertices) - 2 for p in o.data.polygons) for o in meshes)
    return dict(collection=coll_name, fbx=fbx_name, fileBytes=os.path.getsize(fp),
                sha256=sha256(fp), meshObjects=len(meshes), trisApprox=tris,
                uvUnwrapped=uv, boxNodesRewired=rewired, droppedKwargs=dropped,
                materials=sorted(m.name for m in mats))

def main():
    before = sha256(BLEND)
    results, errors = [], []
    for coll, name in TARGETS:
        try:
            results.append(export_target(coll, name))
        except Exception as e:
            errors.append(f"{coll}: {e!r}")
    after = sha256(BLEND)
    report = {
        "lane": "m7-fbx-export", "status": "pass" if not errors else "fail",
        "sourceBlendSha256": {"before": before, "after": after, "unchanged": before == after},
        "exportOperator": "bpy.ops.export_scene.fbx", "exportArgsSource": "handoff/asset-runbook.md §1.2",
        "observations": [
            "[OBSERVED] FBX is the Unity import-time candidate format matching existing Art/Candidates/*.fbx; GLB remains handoff canon",
            "[OBSERVED] Smart-UV Project (66deg/0.02) replaces Blender box projection; visual parity NOT guaranteed",
            "[OBSERVED] textures not embedded (embed_textures=False); Unity materials are authored in-editor from GTI PNGs",
        ],
        "untested": ["Unity import success", "axis/scale in Unity", "tri/drawcall budget in-engine"],
        "artifacts": [f"fbx/{n}" for _, n in TARGETS], "targets": results, "errors": errors,
    }
    with open(REPORT, "w") as f:
        json.dump(report, f, indent=2)
    print("M7_FBX_SUMMARY=" + json.dumps({k: report[k] for k in ("status", "errors")}
                                         | {"targets": [(r["fbx"], r["fileBytes"], r["trisApprox"]) for r in results]}))
    sys.exit(0 if not errors else 1)

main()
