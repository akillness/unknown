"""Export the 4 M7 collections as separate GLB handoff candidates.

Run:
  /Applications/Blender.app/Contents/MacOS/Blender --background --factory-startup \
      --python scripts/blender/export_m7_glb.py

Per target collection the authoritative blend is re-opened fresh (no state bleed),
mesh objects get export-only UVs via Smart UV Project (angle_limit 66deg,
island_margin 0.02), materials are rewired in-memory (TexCoord.Object->UV,
TexImage BOX->FLAT) because glTF cannot represent box projection, then exactly the
collection's objects are exported with use_selection. The source blend is NEVER
saved. A validation pass re-imports each GLB into a fresh empty scene and counts
objects/meshes/materials/images. Prints M7_GLB_SUMMARY=<json> and writes
assets/generated/3d/concept-first-m7/glb-export-report.json.
"""
import bpy
import hashlib
import json
import math
import os
import sys
import traceback

ROOT = os.getcwd()
OUT_DIR = os.path.join(ROOT, "assets", "generated", "3d", "concept-first-m7")
BLEND = os.path.join(OUT_DIR, "concept-first-m7.blend")
REPORT = os.path.join(OUT_DIR, "glb-export-report.json")

TARGETS = [
    ("M7-ENV-watchroom", "SM_Env_Watchroom.glb"),
    ("M7-PROP-optical-reader", "SM_Prop_OpticalReader.glb"),
    ("M7-PROP-record-set", "SM_Prop_RecordSet.glb"),
    ("M7-ENV-gate-three", "SM_Env_GateThree.glb"),
]


def sha256(path):
    h = hashlib.sha256()
    with open(path, "rb") as f:
        for chunk in iter(lambda: f.read(1 << 20), b""):
            h.update(chunk)
    return h.hexdigest()


def rewire_materials_for_uv(materials):
    """BOX/Object-space projection is not representable in glTF. For export only:
    set TexImage projection FLAT and feed Mapping nodes from TexCoord.UV."""
    rewired = 0
    for mat in materials:
        if not mat or not mat.use_nodes:
            continue
        nt = mat.node_tree
        for node in nt.nodes:
            if node.type == 'TEX_IMAGE' and node.projection == 'BOX':
                node.projection = 'FLAT'
                rewired += 1
        for link in list(nt.links):
            if (link.from_node.type == 'TEX_COORD'
                    and link.from_socket.name == 'Object'):
                to_sock = link.to_socket
                uv_out = link.from_node.outputs['UV']
                nt.links.remove(link)
                nt.links.new(uv_out, to_sock)
    return rewired


def smart_uv(mesh_objects):
    """Smart UV Project all mesh objects (multi-object edit mode)."""
    if not mesh_objects:
        return 0
    bpy.ops.object.select_all(action='DESELECT')
    for o in mesh_objects:
        o.hide_set(False)
        o.select_set(True)
    bpy.context.view_layer.objects.active = mesh_objects[0]
    bpy.ops.object.mode_set(mode='EDIT')
    bpy.ops.mesh.select_all(action='SELECT')
    bpy.ops.uv.smart_project(angle_limit=math.radians(66.0), island_margin=0.02)
    bpy.ops.object.mode_set(mode='OBJECT')
    return len(mesh_objects)


def gltf_export_kwargs(filepath):
    """Build kwargs, keeping only properties this Blender's exporter accepts."""
    wanted = {
        "filepath": filepath,
        "export_format": 'GLB',
        "use_selection": True,
        "export_apply": True,
        "export_yup": True,
    }
    props = bpy.ops.export_scene.gltf.get_rna_type().properties.keys()
    kept = {k: v for k, v in wanted.items() if k == "filepath" or k in props}
    dropped = sorted(set(wanted) - set(kept))
    return kept, dropped


def export_target(coll_name, glb_name):
    bpy.ops.wm.open_mainfile(filepath=BLEND)
    coll = bpy.data.collections.get(coll_name)
    if coll is None:
        raise RuntimeError(f"collection not found: {coll_name}")

    objs = list(coll.objects)
    by_type = {}
    for o in objs:
        by_type[o.type] = by_type.get(o.type, 0) + 1
    meshes = [o for o in objs if o.type == 'MESH']

    uv_count = smart_uv(meshes)
    mats = {slot.material for o in meshes for slot in o.material_slots if slot.material}
    rewired = rewire_materials_for_uv(mats)

    bpy.ops.object.select_all(action='DESELECT')
    for o in objs:
        o.select_set(True)
    if objs:
        bpy.context.view_layer.objects.active = objs[0]

    filepath = os.path.join(OUT_DIR, glb_name)
    kwargs, dropped = gltf_export_kwargs(filepath)
    bpy.ops.export_scene.gltf(**kwargs)

    return {
        "collection": coll_name,
        "glb": glb_name,
        "fileBytes": os.path.getsize(filepath),
        "sourceObjects": len(objs),
        "sourceObjectsByType": by_type,
        "uvUnwrapped": uv_count,
        "boxNodesRewired": rewired,
        "droppedExportKwargs": dropped,
    }


def validate_target(entry):
    filepath = os.path.join(OUT_DIR, entry["glb"])
    bpy.ops.wm.read_factory_settings(use_empty=True)
    # purge anything factory scene might carry
    for img in list(bpy.data.images):
        bpy.data.images.remove(img)
    for mat in list(bpy.data.materials):
        bpy.data.materials.remove(mat)
    pre_objs = len(bpy.data.objects)
    bpy.ops.import_scene.gltf(filepath=filepath)
    entry.update({
        "importedObjects": len(bpy.data.objects) - pre_objs,
        "meshes": len(bpy.data.meshes),
        "materials": len(bpy.data.materials),
        "images": len([i for i in bpy.data.images if i.name != 'Render Result']),
    })
    entry["objectCountMatch"] = entry["importedObjects"] == entry["sourceObjects"]
    return entry


def main():
    sha_before = sha256(BLEND)
    results, errors = [], []

    for coll_name, glb_name in TARGETS:
        try:
            results.append(export_target(coll_name, glb_name))
        except Exception as e:
            traceback.print_exc()
            errors.append(f"{coll_name}: {e!r}")

    for entry in results:
        try:
            validate_target(entry)
        except Exception as e:
            traceback.print_exc()
            errors.append(f"validate {entry['glb']}: {e!r}")

    sha_after = sha256(BLEND)

    all_exported = len(results) == len(TARGETS)
    all_sized = all(r["fileBytes"] > 100 * 1024 for r in results)
    all_match = all(r.get("objectCountMatch") for r in results)
    blend_untouched = sha_before == sha_after
    ok = all_exported and all_sized and all_match and blend_untouched and not errors

    summary = {t[1]: {k: r.get(k) for k in (
        "fileBytes", "sourceObjects", "importedObjects", "meshes", "materials", "images")}
        for t, r in zip(TARGETS, results)} if all_exported else {
        r["glb"]: r for r in results}

    observations = [
        "[OBSERVED] UVs are Smart-UV-Project generated (angle_limit 66deg, island_margin "
        "0.02) for export only; the authoritative blend keeps its box-projection "
        "materials and was never saved.",
        "[OBSERVED] Source materials use BOX projection on Object coordinates, which "
        "glTF cannot represent; materials were rewired in-memory (BOX->FLAT, "
        "TexCoord.Object->UV) before export, so box-projection visual parity is NOT "
        "guaranteed - tiling frequency and seam placement differ from the blend.",
        "[OBSERVED] These GLBs are blockout handoff candidates (runtimeEligible:false), "
        "not final art.",
        f"[OBSERVED] source blend sha256 before={sha_before} after={sha_after} "
        f"({'byte-identical' if blend_untouched else 'CHANGED - INVESTIGATE'}).",
    ]
    for r in results:
        observations.append(
            "[OBSERVED] {glb}: {fileBytes} bytes, sourceObjects={sourceObjects} "
            "({types}), importedObjects={imported}, meshes={meshes}, "
            "materials={materials}, images={images}, objectCountMatch={match}.".format(
                glb=r["glb"], fileBytes=r["fileBytes"],
                sourceObjects=r["sourceObjects"],
                types=", ".join(f"{k}:{v}" for k, v in sorted(r["sourceObjectsByType"].items())),
                imported=r.get("importedObjects"), meshes=r.get("meshes"),
                materials=r.get("materials"), images=r.get("images"),
                match=r.get("objectCountMatch")))
        if r["droppedExportKwargs"]:
            observations.append(
                f"[OBSERVED] {r['glb']}: exporter does not accept kwargs "
                f"{r['droppedExportKwargs']} in this Blender; they were omitted.")

    report = {
        "lane": "glb-export",
        "status": "pass" if ok else ("partial" if results else "fail"),
        "observations": observations + [f"[OBSERVED] error: {e}" for e in errors],
        "artifacts": [os.path.relpath(os.path.join(OUT_DIR, t[1]), ROOT)
                      for t in TARGETS
                      if os.path.exists(os.path.join(OUT_DIR, t[1]))]
                     + [os.path.relpath(REPORT, ROOT)],
        "untested": [
            "Unity import",
            "texel density",
            "lightmap UVs",
            "LODs",
            "visual parity of rewired UV materials vs blend box projection",
            "normal/bump channel fidelity (Bump node is not exportable to glTF)",
        ],
    }
    with open(REPORT, "w") as f:
        json.dump(report, f, indent=2)

    print("M7_GLB_SUMMARY=" + json.dumps(summary))
    print("M7_GLB_STATUS=" + report["status"])
    sys.exit(0 if ok else 1)


main()
