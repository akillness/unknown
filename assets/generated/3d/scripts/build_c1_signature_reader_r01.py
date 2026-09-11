"""Original static C1 reader and shallow treatment tray.
Run ONLY in an isolated factory-startup Blender background process.
The modeler authored this recipe; root operator executes and reviews the outputs.
No network, external image imports, or modification of a user's open Blender scene.
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
import sys
from datetime import datetime, timezone
from mathutils import Vector

REPO = Path(__file__).resolve().parents[4]
ASSET_ID = "c1-signature-reader"
ASSET = "SM_C1_Signature_Reader"
REVISION = "r01"
SIZE = 1024
SEED = 2026091104
# A failed attempt must be moved/preserved by the operator; never overwrite it.
OUT = REPO / "assets/generated/3d/c1-signature-reader-r01"
SOURCE_PROMPT = (
    "Original static plate reader with a shallow circular left cradle, short rear "
    "swing arm and magnifier ring, short hand crank, and a shallow rectangular "
    "paper-treatment tray at right. Overall metre bounds <= 0.72 x 0.42 x 0.28; "
    "pivot at base bottom centre. Worn blue-grey #36565C, dark #0E1F26/#173238, "
    "irregular dark-based verdigris #4F7A6B and restrained matte #C8D6D3 salt. "
    "Use original geometry with quiet blank label recesses and matte opaque lens. "
    "No text, glyphs, numbers, signatures, page content, scales, logos, LEDs, "
    "emission, moisture animation, hidden evidence or puzzle state. Static art only."
)
CONCEPT_REFS = [
    "_workspace/current/concept/c1-signature-art-brief.md",
    "_workspace/current/presentation/c1-signature-presentation.md",
    "_workspace/current/concept/prompts/tool-reader-hero.txt",
    "assets/generated/2d/concept/tool-reader-hero.png",
    "_workspace/current/worldview/glossary.md#reader",
    "_workspace/current/production/decision-log.md#rfc-cx-005",
]
assert bpy.app.background, "Use a separate background process, never the user's GUI."
assert not bpy.data.filepath, "Use --factory-startup; never load a user's scene."
assert not OUT.exists(), "Preserve prior outputs; choose a new attempt/revision."
INITIAL = [{"name": o.name, "type": o.type} for o in bpy.data.objects]
OUT.mkdir(parents=True)
(OUT / "textures").mkdir()
# Inspect the factory scene before adding assets. Hide, never remove its objects.
for obj in bpy.data.objects:
    obj.hide_render = True
scene = bpy.context.scene
scene.unit_settings.system = "METRIC"
scene.unit_settings.scale_length = 1.0
collection = bpy.data.collections.new("C1_SIGNATURE_READER_CANDIDATE")
scene.collection.children.link(collection)
root = bpy.data.objects.new("ROOT_C1_Signature_Reader", None)
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


iron = make_surface("MAT_C1_Reader_Casting", "36565C")
bronze = make_surface("MAT_C1_Reader_Patina", "4F7A6B", True)
dark = plain_material("MAT_C1_Reader_Recess", "0E1F26", .87, .03)
ceramic = plain_material("MAT_C1_Reader_Salt", "C8D6D3", .94)

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


# Original geometry, front -Y/up +Z. All elements stay inside the approved box.
box("Base", (0, 0, .012), (.700, .390, .024), iron, .006)
for x in (-.284, .284):
    for y in (-.147, .147):
        cylinder("Foot", (x, y, .004), .022, .008, dark, 16, "Z")

# A low round left cradle; upper recess is blank and has no baked evidence.
cylinder("Cradle_Casting", (-.183, -.015, .049), .150, .060, iron, 48, "Z")
cylinder("Cradle_Recess", (-.183, -.015, .079), .131, .006, dark, 48, "Z")
cylinder("Cradle_Plate", (-.183, -.015, .083), .113, .004, bronze, 48, "Z")

def ring(name, pos, radius, tube_radius, material=bronze, axis="Z", major=48, minor=6):
    rotation = (math.pi/2, 0, 0) if axis == "Y" else ((0, math.pi/2, 0) if axis == "X" else (0, 0, 0))
    bpy.ops.mesh.primitive_torus_add(major_segments=major, minor_segments=minor,
        location=pos, rotation=rotation, major_radius=radius, minor_radius=tube_radius)
    obj = adopt(bpy.context.object, ASSET + "_" + name, material)
    bpy.ops.object.transform_apply(location=False, rotation=True, scale=True)
    for face in obj.data.polygons:
        face.use_smooth = True
    return obj

ring("Cradle_Lip", (-.183, -.015, .083), .139, .005, bronze)
# Keep quiet uncluttered surfaces: three sparse clamps, no tick marks or labels.
for angle in (-math.pi/2, math.pi/6, 5*math.pi/6):
    x, y = -.183 + .126*math.cos(angle), -.015 + .126*math.sin(angle)
    clamp = box("Retainer", (x,y,.092), (.030,.018,.012), bronze,.002)
    clamp.rotation_euler.z = angle
    cylinder("Retainer_Rivet", (x,y,.101), .006, .006, dark, 8,"Z")

# Rear pedestal and an articulated-looking but permanently static short arm.
cylinder("Arm_Foot", (-.250,.116,.055), .035,.045, bronze,24,"Z")
cylinder("Arm_Post", (-.250,.116,.141), .019,.173, iron,20,"Z")
cylinder("Arm_Cap", (-.250,.116,.231), .025,.014, bronze,20,"Z")
box("Swing_Arm", (-.210,.038,.226), (.032,.164,.018), iron,.003).rotation_euler.z=-.47
cylinder("Arm_Hinge", (-.246,.109,.229), .014,.022, bronze,16,"Z")
box("Lens_Neck", (-.164,-.043,.225), (.034,.057,.016), bronze,.003)
ring("Magnifier_Ring", (-.164,-.078,.229), .061,.007, bronze)
# Opaque lens avoids transparency sorting and never reveals evidence geometry.
cylinder("Matte_Lens", (-.164,-.078,.229), .055,.004, dark,40,"Z")

# Short front crank remains inside the footprint and never animates.
cylinder("Crank_Hub", (-.063,-.126,.064), .021,.039, bronze,20,"Y")
box("Crank_Lever", (-.063,-.152,.047), (.016,.016,.050), iron,.003)
cylinder("Crank_Grip", (-.063,-.171,.031), .010,.035, dark,16,"Y")

# The tray is part of the reader working surface, not a new instrument.
box("Tray_Casting", (.186,.001,.039), (.309,.330,.035), iron,.005)
box("Tray_Bed", (.186,.001,.058), (.276,.294,.005), dark,.002)
for x in (.041,.331):
    box("Tray_Side_Rim", (x,.001,.066), (.014,.315,.018), bronze,.003)
for y in (-.150,.152):
    box("Tray_End_Rim", (.186,y,.066), (.278,.014,.018), bronze,.003)
# Low ribs only at the outer edge, leaving both UI paper planes uncluttered.
for x in (.076,.296):
    box("Tray_Support_Rib", (x,.001,.063), (.007,.260,.008), iron,.002)
box("Blank_Label_Recess", (.186,-.177,.028), (.128,.014,.007), dark,.002)

# One constant-diameter short coupling with a matte salt ring.
def cylinder_x(name,pos,radius,depth,material=bronze,vertices=16):
    obj=cylinder(name,pos,radius,depth,material,vertices,"Z")
    obj.rotation_euler.y=math.pi/2
    bpy.ops.object.transform_apply(location=False,rotation=True,scale=True)
    return obj
cylinder_x("Tray_Coupling", (.017,.117,.071), .010,.067, bronze,16)
ring("Coupling_Salt_Ring", (.029,.117,.071), .011,.0018, ceramic,"X",24,6)
for x,y in [(-.309,-.105),(-.297,.058),(.047,.133),(.323,-.132)]:
    cylinder("Sparse_Bolt", (x,y,.083 if x<0 else .079), .0055,.004, bronze,8,"Z")
# A few tiny hexagonal deposits on mechanical edges, not paper/occlusion truth.
for x,y,z in [(-.295,-.101,.084),(-.299,-.097,.084),(-.305,-.095,.083),(.316,.134,.077),(.311,.136,.077),(.308,.132,.077)]:
    cylinder("Salt_Edge_Grain", (x,y,z), .0016,.0008, ceramic,6,"Z")

def unwrap_world(obj):
    # Dominant-axis planar projection, vertical faces preserve downward V streaks.
    uv = obj.data.uv_layers.new(name="UVMap")
    for face in obj.data.polygons:
        axis = max(range(3), key=lambda i: abs(face.normal[i]))
        for li in face.loop_indices:
            co = obj.matrix_world @ obj.data.vertices[obj.data.loops[li].vertex_index].co
            if axis == 1:
                u,v = (co.x+.36)/.72, co.z/.28
            elif axis == 0:
                u,v = (co.y+.21)/.42, co.z/.28
            else:
                u,v = (co.x+.36)/.72, (co.y+.21)/.42
            uv.data[li].uv = (min(.995,max(.005,u)),min(.995,max(.005,v)))

bpy.context.view_layer.update()
for obj in meshes:
    unwrap_world(obj)
# Resolve all memberships before any join; joined Blender datablocks are invalid.
material_groups = [(mat,[obj for obj in meshes if obj.data.materials[0]==mat])
                   for mat in (iron,bronze,dark,ceramic)]
joined_meshes=[]
for mat,members in material_groups:
    if not members:
        continue
    bpy.ops.object.select_all(action="DESELECT")
    for obj in members:
        obj.select_set(True)
    bpy.context.view_layer.objects.active=members[0]
    bpy.ops.object.join()
    joined=bpy.context.object
    joined.name=ASSET+"_"+mat.name.removeprefix("MAT_C1_Reader_")
    bpy.ops.object.transform_apply(location=True,rotation=True,scale=True)
    joined_meshes.append(joined)
meshes=joined_meshes
bpy.context.view_layer.update()

def mesh_measure(objects):
    records=[]
    corners=[]
    for obj in objects:
        obj.data.calc_loop_triangles()
        records.append({"name":obj.name,"triangles":len(obj.data.loop_triangles),
            "vertices":len(obj.data.vertices),"materials":[m.name for m in obj.data.materials if m],
            "dimensions_m":list(obj.dimensions),"uv_layers":len(obj.data.uv_layers)})
        corners.extend(obj.matrix_world@Vector(c) for c in obj.bound_box)
    lo=[min(p[i] for p in corners) for i in range(3)]
    hi=[max(p[i] for p in corners) for i in range(3)]
    return {"objects":records,"mesh_count":len(records),
        "material_count":len({m for r in records for m in r["materials"]}),
        "triangles":sum(r["triangles"] for r in records),
        "bounds_m":{"min":lo,"max":hi,"size":[hi[i]-lo[i] for i in range(3)]}}

measurements=mesh_measure(meshes)
measurements.update({"asset_id":ASSET_ID,"canonical_name":"판독기","glossary_id":"reader",
    "revision":REVISION,"status":"blender-measured","runtimeEligible":False,
    "blender_version":bpy.app.version_string,"initial_scene":INITIAL,"unit":"metre",
    "front_axis_blender":"-Y","up_axis_blender":"+Z","pivot":"base bottom centre",
    "rig":None,"lod_levels":[0],"collision":"none; static visual prop only",
    "tri_budget_target":6000,"mesh_budget_target":8,"material_budget_target":4,
    "dimension_budget_m":[.72,.42,.28],"performance_budget_passed":False,
    "gameplay_visibility_review":"not performed","measured_frame_palette_coverage":None})
measurements["texture_files"]=[{"path":str(p.relative_to(OUT)),"bytes":p.stat().st_size,
    "width":SIZE,"height":SIZE,"color_space":"sRGB" if "BaseColor" in p.name else "linear"}
    for p in sorted((OUT/"textures").glob("*.png"))]
measurements["texture_bytes"]=sum(t["bytes"] for t in measurements["texture_files"])
assert measurements["triangles"]<=6000, "Triangle target exceeded."
assert measurements["mesh_count"]<=8, "Mesh target exceeded."
assert measurements["material_count"]<=4, "Material target exceeded."
assert len(measurements["texture_files"])<=4, "Texture target exceeded."
assert all(d<=limit+1e-5 for d,limit in zip(measurements["bounds_m"]["size"],[.72,.42,.28])), "Dimension target exceeded."
assert abs(measurements["bounds_m"]["min"][2])<1e-5, "Bottom pivot mismatch."
measurements["authored_budget_checks_passed"]=True

bpy.ops.object.select_all(action="DESELECT")
for obj in [root]+meshes:
    obj.select_set(True)
bpy.context.view_layer.objects.active=meshes[0]
glbpath=OUT/(ASSET+".glb")
fbxpath=OUT/(ASSET+".fbx")
bpy.ops.export_scene.gltf(filepath=str(glbpath),export_format="GLB",use_selection=True,export_yup=True,export_apply=True)
bpy.ops.export_scene.fbx(filepath=str(fbxpath),use_selection=True,object_types={"MESH","EMPTY"},
    axis_forward="-Z",axis_up="Y",global_scale=1,apply_unit_scale=True,bake_anim=False,path_mode="RELATIVE")

# Measure the canonical GLB index accessors rather than assuming source totals.
glbbytes=glbpath.read_bytes()
magic,version,total=struct.unpack_from("<4sII",glbbytes,0)
assert magic==b"glTF" and version==2 and total==len(glbbytes)
jsonlen,jsontype=struct.unpack_from("<II",glbbytes,12)
assert jsontype==0x4e4f534a
manifest=json.loads(glbbytes[20:20+jsonlen])
glbtri=0
for mesh in manifest.get("meshes",[]):
    for primitive in mesh["primitives"]:
        assert primitive.get("mode",4)==4
        count=manifest["accessors"][primitive["indices"]]["count"]
        assert count%3==0
        glbtri+=count//3
assert glbtri==measurements["triangles"], "GLB triangle count differs from source."
measurements["canonical_glb"]={"path":glbpath.name,"triangles":glbtri,
    "mesh_count":len(manifest.get("meshes",[])),"material_count":len(manifest.get("materials",[])),
    "bytes":glbpath.stat().st_size,"basis":"glTF 2 index accessors"}

# FBX is an engine-import adapter. Round-trip it in a separate owned scratch scene.
verify_scene=bpy.data.scenes.new("VERIFY_ONLY_FBX_ADAPTER")
bpy.context.window.scene=verify_scene
bpy.ops.import_scene.fbx(filepath=str(fbxpath),use_anim=False)
bpy.context.view_layer.update()
imported=[o for o in verify_scene.objects if o.type=="MESH"]
assert imported, "FBX adapter round-trip produced no meshes."
fbxmeasure=mesh_measure(imported)
fbxmeasure.update({"path":fbxpath.name,"bytes":fbxpath.stat().st_size,
    "basis":"Blender FBX export then fresh scene reimport; not Unity validation"})
assert fbxmeasure["triangles"]==measurements["triangles"], "FBX adapter triangle mismatch."
assert all(abs(a-b)<1e-4 for a,b in zip(fbxmeasure["bounds_m"]["size"],measurements["bounds_m"]["size"])), "FBX metre bounds mismatch."
measurements["fbx_adapter"]=fbxmeasure
# Preserve the scratch verification objects, never destructively modify user state.
bpy.context.window.scene=scene

# Product render stage stays outside exported asset selection, never gameplay proof.
scene.render.engine="CYCLES"
scene.cycles.samples=48
scene.render.resolution_x=scene.render.resolution_y=1024
scene.render.resolution_percentage=100
scene.render.image_settings.file_format="PNG"
scene.render.film_transparent=False
scene.view_settings.view_transform="Standard"
scene.view_settings.look="None"
scene.view_settings.exposure=0
scene.view_settings.gamma=1
world=bpy.data.worlds.new("PREVIEW_ONLY_C1_Reader_World")
world.use_nodes=True
world.node_tree.nodes["Background"].inputs["Color"].default_value=(*linear_hex("36565C"),1)
world.node_tree.nodes["Background"].inputs["Strength"].default_value=.40
scene.world=world
bpy.ops.mesh.primitive_plane_add(size=200,location=(0,0,-.003))
floor=bpy.context.object
floor.name="PREVIEW_ONLY_Ground"
floor.data.materials.append(plain_material("PREVIEW_ONLY_Ground_Mat","173238"))
for name,loc,energy,size,color in [
    ("Key",(-.8,-1.1,1.45),130,1.25,(1,.97,.91)),
    ("Fill",(.95,-.6,.85),55,1.15,(.79,.92,1)),
    ("Rim",(.2,.8,1.15),90,1.0,(.77,.91,1)),
]:
    data=bpy.data.lights.new("PREVIEW_ONLY_"+name,"AREA")
    data.energy,data.shape,data.size,data.color=energy,"DISK",size,color
    obj=bpy.data.objects.new(data.name,data)
    scene.collection.objects.link(obj)
    obj.location=loc
    obj.rotation_euler=(Vector((0,0,.10))-obj.location).to_track_quat("-Z","Y").to_euler()
camdata=bpy.data.cameras.new("PREVIEW_ONLY_Camera")
camera=bpy.data.objects.new(camdata.name,camdata)
scene.collection.objects.link(camera)
camera.location=(.68,-1.10,1.03)
camera.rotation_euler=(Vector((0,0,.085))-camera.location).to_track_quat("-Z","Y").to_euler()
camdata.type="ORTHO"
camdata.ortho_scale=.88
scene.camera=camera
scene.render.filepath=str(OUT/"preview.png")
# File points to the asset/preview scene; verification scene is kept as explicit evidence.
bpy.ops.wm.save_as_mainfile(filepath=str(OUT/(ASSET+".blend")))
bpy.ops.render.render(write_still=True)
(OUT/"measurements.json").write_text(json.dumps(measurements,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
(OUT/"prompt.txt").write_text(SOURCE_PROMPT+"\n",encoding="utf8")

def sha(path):
    return hashlib.sha256(path.read_bytes()).hexdigest()

outputs=[{"path":str(p.relative_to(OUT)),"bytes":p.stat().st_size,"sha256":sha(p)}
         for p in sorted(OUT.rglob("*")) if p.is_file()]
reference_receipts=[]
for reference in CONCEPT_REFS:
    path=REPO/reference.split("#",1)[0]
    reference_receipts.append({"path":reference,"sha256":sha(path) if path.is_file() else None})
provenance={"asset_id":ASSET_ID,"canonical_name":"판독기","glossary_id":"reader",
    "revision":REVISION,"created_utc":datetime.now(timezone.utc).isoformat(),
    "runtimeEligible":False,"status":"candidate-pending-director-native-review",
    "source_type":"original-procedural-blender","generator":"Blender CLI root operator",
    "authoring_lane":"game-modeler recipe/spec only; no modeler execution claim",
    "generator_version":bpy.app.version_string,"source_prompt":SOURCE_PROMPT,
    "prompt_sha256":hashlib.sha256((SOURCE_PROMPT+"\n").encode()).hexdigest(),
    "recipe_path":str(Path(__file__).resolve().relative_to(REPO)),"recipe_sha256":sha(Path(__file__)),
    "seed":SEED,"concept_ref":CONCEPT_REFS,"reference_receipts":reference_receipts,
    "third_party_assets":[],"imported_external_media":[],"fees":0,
    "concept_scope":"RFC-CX-005: original reader geometry from approved M4 brief and existing reader concept",
    "formats":{"authoring":"blend","canonical_delivery":"GLB",
        "unity_candidate":"FBX adapter with sidecar textures; native importer/material validation pending"},
    "preview_kind":"Blender product render; not gameplay or Unity capture",
    "preview_requested_size":[1024,1024],"preview_actual_size":[scene.render.resolution_x,scene.render.resolution_y],
    "runtime_review":"pending","g4_passed":False,"g5_passed":False,"outputs":outputs}
(OUT/"provenance.json").write_text(json.dumps(provenance,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
# Final receipt only after successful measurements, both exports, source save and render.
success={"status":"complete","asset_id":ASSET_ID,"runtimeEligible":False,
    "provenance_sha256":sha(OUT/"provenance.json"),"recipe_sha256":sha(Path(__file__)),
    "output_count":len(outputs),"blender_version":bpy.app.version_string}
(OUT/"SUCCESS.json").write_text(json.dumps(success,indent=2)+"\n",encoding="utf8")
print("C1_SIGNATURE_READER_GENERATION_COMPLETE "+json.dumps({"output":str(OUT),
    "triangles":measurements["triangles"],"meshes":measurements["mesh_count"],
    "texture_bytes":measurements["texture_bytes"],"runtimeEligible":False}))
