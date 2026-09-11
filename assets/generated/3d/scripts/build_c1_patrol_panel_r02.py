"""Non-destructive r02 detail pass for the C1 gate-system-panel candidate.

Reads only the generated r01 candidate, preserves it, and writes a new r02
candidate. This is a visual/topology-detail candidate, not a runtime approval.
"""
from pathlib import Path
from collections import defaultdict
from datetime import datetime, timezone
import bpy, json, hashlib, shutil, math
from mathutils import Vector

REPO = Path(__file__).resolve().parents[4]
SRC = REPO / "assets/generated/3d/c1-patrol-panel-r01"
OUT = REPO / "assets/generated/3d/c1-patrol-panel-r02"
ASSET = "SM_C1_Gate_System_Panel"

assert SRC.is_dir() and (SRC / f"{ASSET}.blend").is_file(), "r01 candidate is required"
assert not OUT.exists(), "Never overwrite a candidate revision"
OUT.mkdir(parents=True)
(OUT / "textures").mkdir()

# Load a generated candidate only; never open a user scene or alter r01.
bpy.ops.wm.open_mainfile(filepath=str(SRC / f"{ASSET}.blend"))
collection = bpy.data.collections.get("C1_GATE_SYSTEM_PANEL_CANDIDATE")
root = bpy.data.objects.get("ROOT_C1_Gate_System_Panel")
assert collection and root, "Expected r01 candidate collection/root"

# Duplicate textures and retarget the material images, keeping r02 self-contained.
for src in sorted((SRC / "textures").glob("*.png")):
    dst = OUT / "textures" / src.name
    shutil.copy2(src, dst)
for image in bpy.data.images:
    if image.filepath:
        name = Path(bpy.path.abspath(image.filepath)).name
        candidate = OUT / "textures" / name
        if candidate.exists():
            image.filepath = str(candidate)

mats = {m.name: m for m in bpy.data.materials}
iron = mats["MAT_C1_Panel_Casting"]
bronze = mats["MAT_C1_Panel_Patina"]
dark = mats["MAT_C1_Panel_Recess"]
paper = mats["MAT_C1_Panel_Blank_Paper"]
ceramic = mats["MAT_C1_Panel_Salt_Porcelain"]
amber = mats["MAT_C1_Panel_Lamp_Lens"]

# r01 kept six material delivery groups. New geometry is joined back into these
# groups so r02 preserves that contract while strengthening visible construction.
groups = {m.name: next(o for o in collection.objects if o.type == "MESH" and o.data.materials and o.data.materials[0] == m) for m in (iron, bronze, dark, paper, ceramic, amber)}
added = defaultdict(list)

def adopt(obj, name, material):
    obj.name = f"{ASSET}_{name}"
    for col in list(obj.users_collection):
        col.objects.unlink(obj)
    collection.objects.link(obj)
    obj.parent = root
    obj.data.materials.append(material)
    added[material.name].append(obj)
    return obj

def box(name, pos, size, material, bevel=0.0):
    bpy.ops.mesh.primitive_cube_add(size=1, location=pos)
    obj = adopt(bpy.context.object, name, material)
    obj.dimensions = size
    bpy.ops.object.transform_apply(location=False, rotation=False, scale=True)
    if bevel:
        mod = obj.modifiers.new("R02_edge_hierarchy", "BEVEL")
        mod.width, mod.segments = bevel, 1
        bpy.context.view_layer.objects.active = obj
        bpy.ops.object.modifier_apply(modifier=mod.name)
    return obj

def cylinder(name, pos, radius, depth, material, vertices=16, axis="Y"):
    rotation = (math.pi / 2, 0, 0) if axis == "Y" else (0, 0, 0)
    bpy.ops.mesh.primitive_cylinder_add(vertices=vertices, radius=radius, depth=depth, end_fill_type="NGON", location=pos, rotation=rotation)
    obj = adopt(bpy.context.object, name, material)
    bpy.ops.object.transform_apply(location=False, rotation=True, scale=True)
    for face in obj.data.polygons:
        face.use_smooth = len(face.vertices) == 4
    return obj

def tube(name, points, radius, material):
    curve = bpy.data.curves.new(name, "CURVE")
    curve.dimensions = "3D"
    curve.resolution_u = 1
    curve.bevel_depth, curve.bevel_resolution, curve.use_fill_caps = radius, 1, True
    spline = curve.splines.new("POLY")
    spline.points.add(len(points) - 1)
    for p, xyz in zip(spline.points, points):
        p.co = (*xyz, 1)
    obj = bpy.data.objects.new(name, curve)
    collection.objects.link(obj)
    bpy.context.view_layer.objects.active = obj
    obj.select_set(True)
    bpy.ops.object.convert(target="MESH")
    return adopt(bpy.context.object, name, material)

# Graph contract application: chassis -> seams; supply -> branches; branch forms
# remain legible by silhouette without text, digits, logos, or status emission.
# 1) Sheet-metal door/lip hierarchy: thin, restrained structural shadow lines.
for x, z, sx, sz in [
    (0, .91, 1.01, .014), (0, .055, 1.01, .014),
    (-.49, .48, .014, .80), (.49, .48, .014, .80),
]:
    box("R02_Door_Lip", (x, -.135, z), (sx, .012, sz), bronze, .0015)
for x in (-.275, .275):
    box("R02_Bay_Seam", (x, -.132, .575), (.385, .009, .505), iron, .001)

# 2) The weak readout zone gets deep bezel/retainer layers, not UI text.
box("R02_Readout_Outer_Bezel", (.275, -.221, .605), (.236, .020, .196), bronze, .003)
box("R02_Readout_Dark_Glass", (.275, -.234, .615), (.196, .008, .146), dark, .001)
box("R02_Readout_Paper_Recess", (.275, -.240, .616), (.159, .004, .109), paper, .0005)
for x in (.168, .382):
    box("R02_Readout_Guard_Rail", (x, -.246, .615), (.012, .014, .171), ceramic, .001)
for z in (.550, .680):
    box("R02_Readout_Retainer", (.275, -.247, z), (.183, .014, .012), bronze, .001)

# 3) Lens ring receives non-textual detents/lock silhouette.
cylinder("R02_Lens_Retaining_Ring", (-.275, -.224, .615), .094, .014, bronze, 24)
for angle in range(0, 360, 45):
    r = math.radians(angle)
    x, z = -.275 + math.cos(r) * .103, .615 + math.sin(r) * .103
    notch = box("R02_Lens_Detent", (x, -.232, z), (.012, .010, .025), ceramic, .001)
    notch.rotation_euler[1] = -r
box("R02_Lens_Lock_Tab", (-.275, -.238, .735), (.050, .014, .021), bronze, .002)

# 4) Every visible pipe/branch receives physical attachment grammar.
for x in (-.275, .275):
    for z in (.286, .347):
        cylinder("R02_Conduit_Collar", (x, -.150, z), .026, .015, ceramic, 12, "Z")
    box("R02_Conduit_Clamp", (x, -.158, .314), (.058, .018, .016), bronze, .002)
for x in (-.39, .39):
    box("R02_Service_Mount", (x, -.146, .177), (.045, .024, .030), bronze, .002)

# 5) Integrated support feet read as mechanical mounts, not detached blocks.
for x in (-.36, .36):
    box("R02_Foot_Brace", (x, -.038, .073), (.186, .125, .024), bronze, .003)
    cylinder("R02_Foot_Bolt", (x, -.105, .050), .012, .014, ceramic, 10)

# UV projection for added geometry keeps existing texture direction coherent.
def unwrap_world(obj):
    uv = obj.data.uv_layers.new(name="UVMap") if not obj.data.uv_layers else obj.data.uv_layers.active
    for face in obj.data.polygons:
        n = face.normal
        axis = max(range(3), key=lambda i: abs(n[i]))
        for li in face.loop_indices:
            co = obj.matrix_world @ obj.data.vertices[obj.data.loops[li].vertex_index].co
            if axis == 1: u, v = (co.x + .6) / 1.2, co.z
            elif axis == 0: u, v = (co.y + .3) / .6, co.z
            else: u, v = (co.x + .6) / 1.2, (co.y + .3) / .6
            uv.data[li].uv = (min(.99, max(.01, u)), min(.99, max(.01, v)))
for objects in added.values():
    for obj in objects: unwrap_world(obj)

# Join additions by their material to retain the six-mesh delivery structure.
for mat_name, objects in added.items():
    target = groups[mat_name]
    bpy.ops.object.select_all(action="DESELECT")
    target.select_set(True)
    for obj in objects: obj.select_set(True)
    bpy.context.view_layer.objects.active = target
    bpy.ops.object.join()
    target.name = f"{ASSET}_{mat_name.removeprefix('MAT_C1_Panel_')}"

# Recompute visible mesh measurements and validate the triangle limit.
bpy.context.view_layer.update()
mesh_objects = [o for o in collection.objects if o.type == "MESH"]
all_corners, members = [], []
for obj in mesh_objects:
    obj.data.calc_loop_triangles()
    all_corners.extend(obj.matrix_world @ Vector(c) for c in obj.bound_box)
    members.append({"name":obj.name,"vertices":len(obj.data.vertices),"triangles":len(obj.data.loop_triangles),"uv_layers":len(obj.data.uv_layers)})
lo = [min(p[i] for p in all_corners) for i in range(3)]
hi = [max(p[i] for p in all_corners) for i in range(3)]
triangles = sum(x["triangles"] for x in members)
assert triangles <= 5000, f"Triangle budget exceeded: {triangles}"

# Export selection excludes preview-only studio objects, exactly as the r01 contract.
bpy.ops.object.select_all(action="DESELECT")
root.select_set(True)
for obj in mesh_objects: obj.select_set(True)
bpy.context.view_layer.objects.active = mesh_objects[0]
bpy.ops.export_scene.gltf(filepath=str(OUT / f"{ASSET}.glb"), export_format="GLB", use_selection=True, export_yup=True, export_apply=True)
bpy.ops.export_scene.fbx(filepath=str(OUT / f"{ASSET}.fbx"), use_selection=True, object_types={"MESH","EMPTY"}, axis_forward="-Z", axis_up="Y", global_scale=1, apply_unit_scale=True, bake_anim=False, path_mode="RELATIVE")

# Existing r01 preview camera/lights remain preview-only. Render same composition for comparison.
scene = bpy.context.scene
scene.render.filepath = str(OUT / "preview.png")
bpy.ops.wm.save_as_mainfile(filepath=str(OUT / f"{ASSET}.blend"))
bpy.ops.render.render(write_still=True)

def sha(path): return hashlib.sha256(path.read_bytes()).hexdigest()
files = [p for p in sorted(OUT.rglob("*")) if p.is_file()]
measurements = {
    "asset_id":"c1-patrol-panel", "canonical_name":"수문 계통판", "revision":"r02",
    "source_revision":"r01", "runtimeEligible":False, "status":"blender-measured-candidate",
    "detail_graph":"_workspace/current/modeling/graphs/c1-patrol-panel-r02-detail-contract.dot",
    "triangle_budget_target":5000, "triangles":triangles, "mesh_count":len(mesh_objects),
    "objects":members, "bounds_m":{"min":lo,"max":hi,"size":[hi[i]-lo[i] for i in range(3)]},
    "texture_files":[{"path":str(p.relative_to(OUT)),"bytes":p.stat().st_size,"width":1024,"height":1024,"color_space":"sRGB" if "BaseColor" in p.name else "linear"} for p in sorted((OUT/"textures").glob("*.png"))],
    "topology_scope":"Measured triangle/vertex/UV delivery metadata only; no claim about all self-intersections, normals, or production performance.",
    "visual_scope":"Same preview camera/light composition as r01; preview is not Unity gameplay or a runtime approval.",
    "created_utc":datetime.now(timezone.utc).isoformat(),
}
(OUT / "measurements.json").write_text(json.dumps(measurements, indent=2))
(OUT / "prompt.txt").write_text("r02 graph-engineered detail pass: layered readout bezel, non-textual lens detents, service seams, conduit clamps, and integrated mounts. No text, numbers, logos, thresholds, status emission, or new canon.\n")
(OUT / "SUCCESS.json").write_text(json.dumps({"ok":True,"triangles":triangles,"mesh_count":len(mesh_objects),"utc":datetime.now(timezone.utc).isoformat()}, indent=2))
provenance = {"tool":"Blender", "blender_version":bpy.app.version_string, "source_revision":str(SRC.relative_to(REPO)), "recipe":str(Path(__file__).relative_to(REPO)), "runtimeEligible":False, "outputs":{str(p.relative_to(OUT)):sha(p) for p in sorted(OUT.rglob("*")) if p.is_file()}}
(OUT / "provenance.json").write_text(json.dumps(provenance, indent=2))
print("C1_PANEL_R02_GENERATION_COMPLETE", json.dumps({"triangles":triangles,"mesh_count":len(mesh_objects),"out":str(OUT)}))
