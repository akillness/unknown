"""Original C1 gate-system-panel candidate; run in a fresh Blender factory scene.

Operator (not the spec-only modeler) executes:
  Blender --background --factory-startup --python-exit-code 1 --python <this-file>

No scene deletion, external images, add-ons or network calls. Never overwrites a revision.
"""
from pathlib import Path
from array import array
import bpy
import math
import json
import hashlib
import random
import struct
import zlib
from datetime import datetime, timezone
from mathutils import Vector


REPO = Path(__file__).resolve().parents[4]
OUT = REPO / "assets/generated/3d/c1-patrol-panel-r01"
ASSET = "SM_C1_Gate_System_Panel"
SIZE = 1024
SEED = 20260911
SOURCE_PROMPT = (
    "Original compact weathered maritime gate system panel for C1 patrol: two "
    "visually distinct branch modules, lighting and readout, connected to a shared "
    "central supply. Blue-grey worn metal, irregular downward verdigris and salt "
    "rings, quiet archival-industrial silhouette. Lighting is a guarded round "
    "amber lens; readout is a rectangular pale-paper recess. Blank labels only; "
    "no lettering, numbers, threshold scales, logos, external reference media, "
    "digital LED displays or invented canon. Static prototype prop."
)
CONCEPT_REFS = [
    "_workspace/current/concept/style-guide.md",
    "_workspace/current/planning/campaign.json#c1-b1-c2",
    "_workspace/current/production/decision-log.md#rfc-cx-004",
    "_workspace/current/modeling/specs/c1-patrol-panel.md",
]

assert not bpy.data.filepath, "Use --factory-startup; never load a user scene."
assert not OUT.exists(), "Preserve prior revisions; choose a new output revision."
OUT.mkdir(parents=True)
(OUT / "textures").mkdir()
INITIAL = [{"name": o.name, "type": o.type} for o in bpy.data.objects]
for old in bpy.data.objects:
    old.hide_render = True
scene = bpy.context.scene
scene.unit_settings.system = "METRIC"
scene.unit_settings.scale_length = 1.0
collection = bpy.data.collections.new("C1_GATE_SYSTEM_PANEL_CANDIDATE")
scene.collection.children.link(collection)
root = bpy.data.objects.new("ROOT_C1_Gate_System_Panel", None)
collection.objects.link(root)
meshes = []


def linear_hex(code):
    def cv(v):
        s = int(v, 16) / 255.0
        return s / 12.92 if s <= .04045 else ((s + .055) / 1.055) ** 2.4
    return tuple(cv(code[i:i + 2]) for i in (0, 2, 4))


def srgb_byte(value):
    v = min(1.0, max(0.0, value))
    return round(255 * (12.92 * v if v <= .0031308 else 1.055 * v ** (1 / 2.4) - .055))


assert tuple(srgb_byte(v) for v in linear_hex("36565C")) == (54, 86, 92)


def png(path, pixels, color):
    channels = 3 if color else 1
    rows = bytearray()
    for y in range(SIZE - 1, -1, -1):
        rows.append(0)
        for x in range(SIZE):
            i = (y * SIZE + x) * channels
            if color:
                rows.extend(srgb_byte(pixels[i + c]) for c in range(3))
            else:
                rows.append(round(255 * min(1, max(0, pixels[i]))))
    def chunk(tag, data):
        return struct.pack(">I", len(data)) + tag + data + struct.pack(">I", zlib.crc32(tag + data) & 0xffffffff)
    payload = b"\x89PNG\r\n\x1a\n"
    payload += chunk(b"IHDR", struct.pack(">IIBBBBB", SIZE, SIZE, 8, 2 if color else 0, 0, 0, 0))
    if color:
        payload += chunk(b"sRGB", b"\x00")
    payload += chunk(b"IDAT", zlib.compress(rows, 8)) + chunk(b"IEND", b"")
    path.write_bytes(payload)


def value_noise(x, y, seed):
    def h(ix, iy):
        n = ((ix * 374761393 + iy * 668265263 + seed * 69069) ^ (ix << 13)) & 0xffffffff
        n = ((n ^ (n >> 13)) * 1274126177) & 0xffffffff
        return (n ^ (n >> 16)) / 4294967295.0
    ix, iy = math.floor(x), math.floor(y)
    u, v = x - ix, y - iy
    u, v = u * u * (3 - 2 * u), v * v * (3 - 2 * v)
    a = h(ix, iy) * (1 - u) + h(ix + 1, iy) * u
    b = h(ix, iy + 1) * (1 - u) + h(ix + 1, iy + 1) * u
    return a * (1 - v) + b * v


def mix(a, b, t):
    return tuple(x * (1 - t) + y * t for x, y in zip(a, b))


def make_surface(name, base_hex, bronze=False):
    base, patina, salt = linear_hex(base_hex), linear_hex("4F7A6B"), linear_hex("C8D6D3")
    dark = linear_hex("173238")
    rng = random.Random(SEED + (1 if bronze else 0))
    runs = [(rng.random(), rng.uniform(.4, .99), rng.uniform(.13, .57), rng.uniform(.002, .012)) for _ in range(19)]
    color, rough = array("f"), array("f")
    for py in range(SIZE):
        v = py / (SIZE - 1)
        for px in range(SIZE):
            u = px / (SIZE - 1)
            n = value_noise(u * 13, v * 11, SEED)
            fine = value_noise(u * 93, v * 97, SEED + 8)
            scratch = value_noise(u * 182, v * 18, SEED + 23)
            c = tuple(k * (.76 + .39 * n + .10 * fine) for k in base)
            corrosion = max(0, (n - .56) * 2.1) * (.35 + .65 * fine)
            c = mix(c, patina if not bronze else dark, min(.7, corrosion))
            # Globally vertical V coordinate: these streaks run down from a start.
            streak = 0.0
            for start_u, start_v, length, width in runs:
                dy = start_v - v
                if 0 < dy < length:
                    bend = .006 * math.sin(dy * 33 + start_u * 81)
                    d = abs(u - start_u - bend)
                    if d < width:
                        streak = max(streak, (1 - d / width) * (1 - dy / length))
            c = mix(c, dark, streak * .55)
            edge = max(0, 1 - min(u, v, 1 - u, 1 - v) * 48)
            deposit = max(0, (fine - .61) * 2) * (edge * .8 + corrosion * .24)
            c = mix(c, salt, min(.72, deposit))
            if scratch > .74 and n > .51:
                c = mix(c, salt, .16)
            color.extend(c)
            rough.append(min(.95, .57 + .23 * n + .08 * fine + .07 * deposit))
    basepath = OUT / "textures" / (name + "_BaseColor.png")
    roughpath = OUT / "textures" / (name + "_Roughness.png")
    png(basepath, color, True)
    png(roughpath, rough, False)
    mat = plain_material(name, base_hex, .73, .08)
    nodes = mat.node_tree.nodes
    bsdf = nodes.get("Principled BSDF")
    for path, socket, space in [(basepath, "Base Color", "sRGB"), (roughpath, "Roughness", "Non-Color")]:
        tex = nodes.new("ShaderNodeTexImage")
        tex.image = bpy.data.images.load(str(path), check_existing=False)
        tex.image.colorspace_settings.name = space
        tex.interpolation = "Linear"
        mat.node_tree.links.new(tex.outputs["Color"], bsdf.inputs[socket])
    return mat


def plain_material(name, hexcode, roughness=.8, metallic=0):
    mat = bpy.data.materials.new(name)
    mat.use_nodes = True
    rgba = (*linear_hex(hexcode), 1)
    mat.diffuse_color = rgba
    bsdf = mat.node_tree.nodes.get("Principled BSDF")
    bsdf.inputs["Base Color"].default_value = rgba
    bsdf.inputs["Roughness"].default_value = roughness
    bsdf.inputs["Metallic"].default_value = metallic
    return mat


iron = make_surface("MAT_C1_Panel_Casting", "36565C")
bronze = make_surface("MAT_C1_Panel_Patina", "4F7A6B", True)
dark = plain_material("MAT_C1_Panel_Recess", "173238")
paper = plain_material("MAT_C1_Panel_Blank_Paper", "E7E3D8")
ceramic = plain_material("MAT_C1_Panel_Salt_Porcelain", "C8D6D3")
amber = plain_material("MAT_C1_Panel_Lamp_Lens", "E2AF62", .48)


def adopt(obj, name, material):
    obj.name = name
    for col in list(obj.users_collection):
        col.objects.unlink(obj)
    collection.objects.link(obj)
    obj.parent = root
    obj.data.materials.append(material)
    meshes.append(obj)
    return obj


def box(name, pos, size, material=iron, bevel=.004):
    bpy.ops.mesh.primitive_cube_add(size=1, location=pos)
    obj = adopt(bpy.context.object, ASSET + "_" + name, material)
    obj.dimensions = size
    bpy.ops.object.transform_apply(location=False, rotation=False, scale=True)
    if bevel:
        mod = obj.modifiers.new("Authored_edge_wear_profile", "BEVEL")
        mod.width, mod.segments = bevel, 1
        bpy.context.view_layer.objects.active = obj
        bpy.ops.object.modifier_apply(modifier=mod.name)
    return obj


def cylinder(name, pos, radius, depth, material=bronze, vertices=16, axis="Y"):
    rotation = (math.pi / 2, 0, 0) if axis == "Y" else (0, 0, 0)
    bpy.ops.mesh.primitive_cylinder_add(vertices=vertices, radius=radius, depth=depth, end_fill_type="NGON", location=pos, rotation=rotation)
    obj = adopt(bpy.context.object, ASSET + "_" + name, material)
    bpy.ops.object.transform_apply(location=False, rotation=True, scale=True)
    for face in obj.data.polygons:
        face.use_smooth = len(face.vertices) == 4
    return obj


def tube(name, points, radius=.013):
    curve = bpy.data.curves.new(name, "CURVE")
    curve.dimensions, curve.resolution_u = "3D", 1
    curve.bevel_depth, curve.bevel_resolution, curve.resolution_u = radius, 1, 1
    curve.use_fill_caps = True
    spline = curve.splines.new("POLY")
    spline.points.add(len(points) - 1)
    for p, xyz in zip(spline.points, points):
        p.co = (*xyz, 1)
    obj = bpy.data.objects.new(ASSET + "_" + name, curve)
    collection.objects.link(obj)
    bpy.ops.object.select_all(action="DESELECT")
    obj.select_set(True)
    bpy.context.view_layer.objects.active = obj
    bpy.ops.object.convert(target="MESH")
    return adopt(bpy.context.object, obj.name, bronze)


# Enclosure: broad quiet silhouette, blue-grey casting and dark inset panels.
box("Enclosure", (0, 0, .48), (1.10, .18, .88), bevel=.012)
box("Left_Inset", (-.275, -.098, .59), (.405, .028, .54), dark)
box("Right_Inset", (.275, -.098, .59), (.405, .028, .54), dark)
for x in (-.51, .51):
    box("Upright", (x, -.111, .48), (.035, .024, .80))
for z in (.065, .895):
    box("Cross_Rim", (0, -.112, z), (1.045, .022, .022))
for x in (-.36, .36):
    box("Foot", (x, .012, .022), (.22, .29, .044))

# Blank pale plates are reserved for runtime UI; no baked labels or local lore.
for suffix, x in (("Lighting", -.275), ("Readout", .275)):
    box(suffix + "_Plate_Frame", (x, -.137, .797), (.265, .034, .069), bronze)
    box(suffix + "_Blank_Plate", (x, -.159, .797), (.231, .008, .043), paper, .002)

# Round guarded light module. Amber is a material, not an emissive status claim.
cylinder("Lighting_Socket", (-.275, -.142, .615), .131, .07, bronze, 24)
cylinder("Lighting_Porcelain_Ring", (-.275, -.183, .615), .106, .022, ceramic, 24)
cylinder("Lighting_Amber_Lens", (-.275, -.204, .615), .081, .029, amber, 24)
for xoff in (-.042, .042):
    tube("Lighting_Guard", [(-.275 + xoff, -.198, .515), (-.275 + xoff, -.242, .548), (-.275 + xoff, -.242, .682), (-.275 + xoff, -.198, .715)], .006)
box("Lighting_Switch_Base", (-.275, -.149, .418), (.152, .054, .073), bronze)
box("Lighting_Switch", (-.275, -.193, .429), (.074, .032, .020), ceramic)

# Readout branch: rectilinear paper recess, side rails and solid contact handle.
box("Readout_Housing", (.275, -.146, .585), (.277, .075, .261), iron, .009)
box("Readout_Aperture", (.275, -.188, .604), (.206, .012, .167), dark)
box("Readout_Blank_Slip", (.275, -.198, .611), (.174, .005, .135), paper, 0)
for x in (.163, .387):
    box("Readout_Rail", (x, -.207, .586), (.015, .034, .214), bronze, .002)
box("Readout_Paper_Retainer", (.275, -.214, .554), (.207, .022, .022), bronze, .002)
box("Readout_Contact_Base", (.275, -.149, .418), (.152, .054, .073), bronze)
box("Readout_Contact_Handle", (.275, -.193, .418), (.094, .032, .027), ceramic)

# Shared central feed and two visible branches. No pressure scale or threshold.
cylinder("Shared_Supply", (0, -.126, .233), .095, .097, bronze, 24)
cylinder("Shared_Supply_Face", (0, -.183, .233), .069, .018, dark, 24)
cylinder("Shared_Supply_Hub", (0, -.201, .233), .024, .023, ceramic, 16)
tube("Shared_Inlet", [(0, -.14, .05), (0, -.14, .138)], .024)
for side, x in (("Lighting", -.275), ("Readout", .275)):
    sign = -1 if x < 0 else 1
    tube(side + "_Branch_Feed", [(sign * .075, -.147, .233), (x - sign * .034, -.147, .233), (x, -.147, .267), (x, -.147, .381)], .015)
    cylinder(side + "_Salt_Collar", (x, -.147, .366), .022, .017, ceramic, 12, "Z")
    cylinder(side + "_Union", (x, -.147, .342), .027, .028, bronze, 12, "Z")
cylinder("Supply_Salt_Collar", (0, -.14, .104), .03, .012, ceramic, 16, "Z")

for x in (-.503, .503):
    for z in (.105, .855):
        cylinder("Corner_Fastener", (x, -.137, z), .012, .014, bronze, 8)
        box("Fastener_Slot", (x, -.147, z), (.013, .004, .0025), dark, 0)


def unwrap_world(obj):
    """Dominant-plane UVs keep vertical faces aligned with downward texture V."""
    uv = obj.data.uv_layers.new(name="UVMap") if not obj.data.uv_layers else obj.data.uv_layers.active
    for face in obj.data.polygons:
        n = face.normal
        axis = max(range(3), key=lambda i: abs(n[i]))
        for li in face.loop_indices:
            co = obj.matrix_world @ obj.data.vertices[obj.data.loops[li].vertex_index].co
            if axis == 1:
                u, v = (co.x + .6) / 1.2, co.z
            elif axis == 0:
                u, v = (co.y + .3) / .6, co.z
            else:
                u, v = (co.x + .6) / 1.2, (co.y + .3) / .6
            uv.data[li].uv = (min(.99, max(.01, u)), min(.99, max(.01, v)))


bpy.context.view_layer.update()
for obj in meshes:
    unwrap_world(obj)

# Collapse by material to six static delivery meshes; the two branches remain
# distinct visually, not as false functional/animated mechanisms.
groups = []
# Joining removes the other source Object datablocks. Resolve every material
# membership before the first join, then never inspect that stale source list.
material_groups = [
    (mat, [obj for obj in meshes if obj.data.materials[0] == mat])
    for mat in (iron, bronze, dark, paper, ceramic, amber)
]
for mat, members in material_groups:
    if not members:
        continue
    bpy.ops.object.select_all(action="DESELECT")
    for obj in members:
        obj.select_set(True)
    bpy.context.view_layer.objects.active = members[0]
    bpy.ops.object.join()
    joined = bpy.context.object
    joined.name = ASSET + "_" + mat.name.removeprefix("MAT_C1_Panel_")
    bpy.ops.object.transform_apply(location=True, rotation=True, scale=True)
    groups.append(joined)
meshes = groups
bpy.context.view_layer.update()

measurements = {
    "asset_id": "c1-patrol-panel", "canonical_name": "수문 계통판", "revision": "r01",
    "runtimeEligible": False, "status": "blender-measured", "blender_version": bpy.app.version_string,
    "initial_scene": INITIAL, "unit": "metre", "front_axis_blender": "-Y", "up_axis_blender": "+Z",
    "pivot": "bottom-centre", "rig": None, "lod_levels": [0], "collision": "not authored; static import adapter required",
    "tri_budget_target": 5000, "performance_budget_passed": False, "objects": [],
    "measured_frame_palette_coverage": None, "gameplay_visibility_review": "not performed",
    "concept_sheet_deviation": "RFC-CX-004 scoped first-beat prototype; use style guide and canonical campaign clue",
}
all_corners = []
for obj in meshes:
    obj.data.calc_loop_triangles()
    measurements["objects"].append({"name": obj.name, "vertices": len(obj.data.vertices), "triangles": len(obj.data.loop_triangles), "materials": [m.name for m in obj.data.materials], "dimensions_m": list(obj.dimensions), "uv_layers": len(obj.data.uv_layers)})
    all_corners.extend(obj.matrix_world @ Vector(c) for c in obj.bound_box)
lo = [min(p[i] for p in all_corners) for i in range(3)]
hi = [max(p[i] for p in all_corners) for i in range(3)]
measurements["bounds_m"] = {"min": lo, "max": hi, "size": [hi[i] - lo[i] for i in range(3)]}
measurements["triangles"] = sum(o["triangles"] for o in measurements["objects"])
measurements["mesh_count"] = len(meshes)
measurements["texture_files"] = [{"path": str(p.relative_to(OUT)), "bytes": p.stat().st_size, "width": SIZE, "height": SIZE, "color_space": "sRGB" if "BaseColor" in p.name else "linear"} for p in sorted((OUT / "textures").glob("*.png"))]
measurements["texture_bytes"] = sum(t["bytes"] for t in measurements["texture_files"])
assert measurements["triangles"] <= measurements["tri_budget_target"], "Revise geometry; never relabel an overrun as a pass."

bpy.ops.object.select_all(action="DESELECT")
for obj in [root] + meshes:
    obj.select_set(True)
bpy.context.view_layer.objects.active = meshes[0]
bpy.ops.export_scene.gltf(filepath=str(OUT / (ASSET + ".glb")), export_format="GLB", use_selection=True, export_yup=True, export_apply=True)
bpy.ops.export_scene.fbx(filepath=str(OUT / (ASSET + ".fbx")), use_selection=True, object_types={"MESH", "EMPTY"}, axis_forward="-Z", axis_up="Y", global_scale=1, apply_unit_scale=True, bake_anim=False, path_mode="RELATIVE")

# Product-preview stage is outside the exported selection; not a native capture.
scene.render.engine = "CYCLES"
scene.cycles.samples = 48
scene.render.resolution_x = scene.render.resolution_y = 1024
scene.render.resolution_percentage = 100
scene.render.image_settings.file_format = "PNG"
scene.render.film_transparent = False
scene.view_settings.view_transform = "Standard"
scene.view_settings.look = "Medium High Contrast" if "Medium High Contrast" in [i.name for i in bpy.types.ColorManagedViewSettings.bl_rna.properties['look'].enum_items] else "None"
scene.view_settings.exposure = 0
scene.view_settings.gamma = 1
world = bpy.data.worlds.new("C1_Studio_World")
world.use_nodes = True
world.node_tree.nodes["Background"].inputs["Color"].default_value = (*linear_hex("36565C"), 1)
world.node_tree.nodes["Background"].inputs["Strength"].default_value = .55
scene.world = world
bpy.ops.mesh.primitive_plane_add(size=200, location=(0, 0, -.01))
floor = bpy.context.object
floor.name = "PREVIEW_ONLY_Ground"
floor.data.materials.append(plain_material("PREVIEW_ONLY_Ground_Mat", "173238"))
for name, loc, energy, size, color in [
    ("Key", (-2.4, -3.0, 3.6), 520, 3.0, (1, .97, .91)),
    ("Fill", (2.5, -2.0, 1.6), 300, 2.5, (.79, .92, 1)),
    ("Rim", (.6, 1.4, 2.8), 430, 2.0, (.77, .91, 1)),
]:
    data = bpy.data.lights.new("PREVIEW_ONLY_" + name, "AREA")
    data.energy, data.shape, data.size, data.color = energy, "DISK", size, color
    obj = bpy.data.objects.new(data.name, data)
    scene.collection.objects.link(obj)
    obj.location = loc
    obj.rotation_euler = (Vector((0, 0, .48)) - obj.location).to_track_quat("-Z", "Y").to_euler()
camdata = bpy.data.cameras.new("PREVIEW_ONLY_Camera")
camera = bpy.data.objects.new(camdata.name, camdata)
scene.collection.objects.link(camera)
camera.location = (1.18, -2.86, 1.48)
camera.rotation_euler = (Vector((0, -.02, .47)) - camera.location).to_track_quat("-Z", "Y").to_euler()
camdata.lens = 57
scene.camera = camera
scene.render.filepath = str(OUT / "preview.png")
bpy.ops.wm.save_as_mainfile(filepath=str(OUT / (ASSET + ".blend")))
bpy.ops.render.render(write_still=True)
(OUT / "measurements.json").write_text(json.dumps(measurements, ensure_ascii=False, indent=2) + "\n")
(OUT / "prompt.txt").write_text(SOURCE_PROMPT + "\n", encoding="utf8")


def sha(path):
    return hashlib.sha256(path.read_bytes()).hexdigest()


outputs = [{"path": str(p.relative_to(OUT)), "bytes": p.stat().st_size, "sha256": sha(p)} for p in sorted(OUT.rglob("*")) if p.is_file()]
provenance = {
    "asset_id": "c1-patrol-panel", "canonical_name": "수문 계통판", "revision": "r01", "created_utc": datetime.now(timezone.utc).isoformat(),
    "runtimeEligible": False, "status": "candidate-pending-director-visual-review", "source_type": "original-procedural-blender",
    "generator": "Blender CLI root operator; deterministic authored Python recipe", "generator_version": bpy.app.version_string,
    "source_prompt": SOURCE_PROMPT, "prompt_sha256": hashlib.sha256((SOURCE_PROMPT + "\n").encode()).hexdigest(),
    "recipe_path": str(Path(__file__).resolve().relative_to(REPO)), "recipe_sha256": sha(Path(__file__)), "seed": SEED,
    "concept_ref": CONCEPT_REFS, "external_reference_media": [], "third_party_assets": [], "fees": 0,
    "concept_deviation": "RFC-CX-004: canonical C1/c1-b1 clue c1-b1-c2 plus style guide and director brief substitute for absent sheet only for this prototype",
    "formats": {"canonical": "GLB", "unity_candidate": "FBX with sidecar textures; importer validation required", "authoring": "blend"},
    "preview_kind": "Blender product render; not gameplay or Unity capture",
    "runtime_review": "pending", "g4_passed": False, "g5_passed": False, "outputs": outputs,
}
(OUT / "provenance.json").write_text(json.dumps(provenance, ensure_ascii=False, indent=2) + "\n")
# Blender can otherwise exit 0 after a Python exception. The CLI flag above and
# this last-written receipt are both required; absence is an incomplete attempt.
success = {
    "status": "complete", "asset_id": "c1-patrol-panel", "runtimeEligible": False,
    "provenance_sha256": sha(OUT / "provenance.json"),
    "recipe_sha256": sha(Path(__file__)), "output_count": len(outputs),
}
(OUT / "SUCCESS.json").write_text(json.dumps(success, indent=2) + "\n")
print("C1_PANEL_GENERATION_COMPLETE " + json.dumps({"output": str(OUT), "triangles": measurements["triangles"], "meshes": len(meshes), "texture_bytes": measurements["texture_bytes"], "runtimeEligible": False}))
