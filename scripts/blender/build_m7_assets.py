"""Build M7 concept-first 3D asset candidates in Blender (headless).

Targets from _workspace/current/handoff/concept-first-m7-resources.json:
  M7-PROP-optical-reader, M7-ENV-watchroom, M7-PROP-record-set, M7-ENV-gate-three
Textures: assets/generated/2d/texture/m7-*-r01/ (GTI basecolor + derived candidates).
Output:   assets/generated/3d/concept-first-m7/concept-first-m7.blend + renders/.

Run:
  Blender --background --factory-startup --python scripts/blender/build_m7_assets.py
"""
import bpy, bmesh, math, os, json, sys

ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
TEX = os.path.join(ROOT, "assets/generated/2d/texture")
OUT = os.path.join(ROOT, "assets/generated/3d/concept-first-m7")
RENDERS = os.path.join(OUT, "renders")
os.makedirs(RENDERS, exist_ok=True)

# ---------------------------------------------------------------- scene reset
bpy.ops.wm.read_factory_settings(use_empty=True)
scene = bpy.context.scene
scene.unit_settings.system = 'METRIC'

COLLECTIONS = {}
def coll(name):
    if name not in COLLECTIONS:
        c = bpy.data.collections.new(name)
        scene.collection.children.link(c)
        COLLECTIONS[name] = c
    return COLLECTIONS[name]

def link_to(obj, cname):
    for c in obj.users_collection:
        c.objects.unlink(obj)
    coll(cname).objects.link(obj)

# ---------------------------------------------------------------- materials
def _img(path):
    img = bpy.data.images.load(path, check_existing=True)
    return img

def make_tex_material(name, folder, scale=1.0, metallic=0.0, bump_channel=None,
                      bump_strength=0.3, transmission=0.0, ior=1.45, rough_default=None):
    mat = bpy.data.materials.new(name)
    mat.use_nodes = True
    nt = mat.node_tree
    bsdf = nt.nodes["Principled BSDF"]
    bsdf.inputs["Metallic"].default_value = metallic
    if transmission > 0.0:
        bsdf.inputs["Transmission Weight"].default_value = transmission
        bsdf.inputs["IOR"].default_value = ior

    tc = nt.nodes.new("ShaderNodeTexCoord")
    mp = nt.nodes.new("ShaderNodeMapping")
    mp.inputs["Scale"].default_value = (scale, scale, scale)
    nt.links.new(tc.outputs["Object"], mp.inputs["Vector"])

    def tex_node(fname, non_color=False):
        n = nt.nodes.new("ShaderNodeTexImage")
        n.image = _img(os.path.join(folder, fname))
        n.projection = 'BOX'
        n.projection_blend = 0.25
        if non_color:
            n.image.colorspace_settings.name = 'Non-Color'
        nt.links.new(mp.outputs["Vector"], n.inputs["Vector"])
        return n

    base = tex_node("basecolor.png")
    nt.links.new(base.outputs["Color"], bsdf.inputs["Base Color"])

    rough_path = os.path.join(folder, "roughness.png")
    if os.path.exists(rough_path) and rough_default is None:
        rough = tex_node("roughness.png", non_color=True)
        nt.links.new(rough.outputs["Color"], bsdf.inputs["Roughness"])
    elif rough_default is not None:
        bsdf.inputs["Roughness"].default_value = rough_default

    if bump_channel and os.path.exists(os.path.join(folder, bump_channel + ".png")):
        h = tex_node(bump_channel + ".png", non_color=True)
        bump = nt.nodes.new("ShaderNodeBump")
        bump.inputs["Strength"].default_value = bump_strength
        nt.links.new(h.outputs["Color"], bump.inputs["Height"])
        nt.links.new(bump.outputs["Normal"], bsdf.inputs["Normal"])
    return mat

def make_plain(name, color, rough=0.5, metallic=0.0, emission=0.0):
    mat = bpy.data.materials.new(name)
    mat.use_nodes = True
    b = mat.node_tree.nodes["Principled BSDF"]
    b.inputs["Base Color"].default_value = (*color, 1.0)
    b.inputs["Roughness"].default_value = rough
    b.inputs["Metallic"].default_value = metallic
    if emission > 0:
        b.inputs["Emission Color"].default_value = (*color, 1.0)
        b.inputs["Emission Strength"].default_value = emission
    return mat

M = {}
M["bronze"]   = make_tex_material("M7-bronze", os.path.join(TEX, "m7-bronze-r01"), scale=2.2, metallic=1.0)
M["steel"]    = make_tex_material("M7-perforated-steel", os.path.join(TEX, "m7-perforated-steel-r01"), scale=1.6, metallic=0.85)
M["concrete"] = make_tex_material("M7-salt-concrete", os.path.join(TEX, "m7-salt-concrete-r01"), scale=0.45, bump_channel="height", bump_strength=0.35)
M["crystal"]  = make_tex_material("M7-salt-crystal", os.path.join(TEX, "m7-salt-crystal-r01"), scale=4.0, bump_channel="microdetail", bump_strength=0.15, transmission=0.65, ior=1.44)
M["paper"]    = make_tex_material("M7-rag-paper", os.path.join(TEX, "m7-rag-paper-r01"), scale=3.0)
M["wood"]     = make_plain("M7-wood-ochre", (0.45, 0.30, 0.10), rough=0.55)
M["darksteel"]= make_plain("M7-dark-steel", (0.035, 0.045, 0.05), rough=0.45, metallic=0.9)
M["glass"]    = make_plain("M7-glass", (0.85, 0.9, 0.92), rough=0.05)
M["glass"].node_tree.nodes["Principled BSDF"].inputs["Transmission Weight"].default_value = 1.0
M["water"]    = make_plain("M7-water", (0.008, 0.02, 0.032), rough=0.22)
M["gauge"]    = make_plain("M7-gauge-face", (0.85, 0.83, 0.75), rough=0.6)
M["bulb"]     = make_plain("M7-bulb", (1.0, 0.75, 0.45), rough=0.3, emission=14.0)

# ---------------------------------------------------------------- primitives
def _finish(obj, name, mat, cname, smooth=False):
    obj.name = name
    if mat: obj.data.materials.append(M[mat])
    if smooth:
        for p in obj.data.polygons: p.use_smooth = True
    link_to(obj, cname)
    return obj

def box(name, size, loc, mat, cname, rot=(0, 0, 0)):
    bpy.ops.mesh.primitive_cube_add(size=1, location=loc, rotation=rot)
    o = bpy.context.active_object
    o.scale = (size[0] / 2, size[1] / 2, size[2] / 2)
    bpy.ops.object.transform_apply(scale=True)
    return _finish(o, name, mat, cname)

def cyl(name, r, depth, loc, mat, cname, rot=(0, 0, 0), verts=48, smooth=True):
    bpy.ops.mesh.primitive_cylinder_add(radius=r, depth=depth, location=loc, rotation=rot, vertices=verts)
    return _finish(bpy.context.active_object, name, mat, cname, smooth=smooth and verts > 8)

def torus(name, r, minor, loc, mat, cname, rot=(0, 0, 0)):
    bpy.ops.mesh.primitive_torus_add(major_radius=r, minor_radius=minor, location=loc, rotation=rot)
    return _finish(bpy.context.active_object, name, mat, cname, smooth=True)

def cone(name, r1, r2, depth, loc, mat, cname, rot=(0, 0, 0)):
    bpy.ops.mesh.primitive_cone_add(radius1=r1, radius2=r2, depth=depth, location=loc, rotation=rot)
    return _finish(bpy.context.active_object, name, mat, cname, smooth=True)

def sphere(name, r, loc, mat, cname):
    bpy.ops.mesh.primitive_uv_sphere_add(radius=r, location=loc, segments=24, ring_count=16)
    return _finish(bpy.context.active_object, name, mat, cname, smooth=True)

def apply_mods(obj):
    with bpy.context.temp_override(object=obj, active_object=obj, selected_objects=[obj]):
        for m in list(obj.modifiers):
            bpy.ops.object.modifier_apply(modifier=m.name)

summary = {"builtTargets": [], "errors": []}

# ============================================================ M7-ENV-watchroom
# Room 6.4m(X) x 5.0m(Y) x 3.0m(Z). Sea-facing window wall at +Y.
BENCH_H = 0.85
try:
    CN = "M7-ENV-watchroom"
    RX, RY, RZ, T = 6.4, 5.0, 3.0, 0.15
    box("wr-floor", (RX, RY, T), (0, 0, -T / 2), "concrete", CN)
    box("wr-ceiling", (RX, RY, T), (0, 0, RZ + T / 2), "concrete", CN)
    box("wr-wall-back", (RX, T, RZ), (0, -RY / 2 - T / 2, RZ / 2), "concrete", CN)
    box("wr-wall-left", (T, RY, RZ), (-RX / 2 - T / 2, 0, RZ / 2), "concrete", CN)
    box("wr-wall-right", (T, RY, RZ), (RX / 2 + T / 2, 0, RZ / 2), "concrete", CN)

    # window wall (+Y): low long horizontal band 1.05..2.25m
    ww = box("wr-wall-window", (RX, T, RZ), (0, RY / 2 + T / 2, RZ / 2), "concrete", CN)
    cutter = box("wr-window-cutter", (RX - 0.8, T * 3, 1.2), (0, RY / 2 + T / 2, 1.65), None, CN)
    mod = ww.modifiers.new("cut", 'BOOLEAN'); mod.object = cutter; mod.operation = 'DIFFERENCE'
    apply_mods(ww)
    bpy.data.objects.remove(cutter)
    # mullions + glass
    for i, x in enumerate([-2.1, -0.7, 0.7, 2.1]):
        box(f"wr-mullion-{i}", (0.06, 0.08, 1.2), (x, RY / 2 + T / 2, 1.65), "darksteel", CN)
    box("wr-window-sill", (RX - 0.7, 0.14, 0.06), (0, RY / 2 + T / 2, 1.02), "darksteel", CN)
    box("wr-window-head", (RX - 0.7, 0.14, 0.06), (0, RY / 2 + T / 2, 2.28), "darksteel", CN)
    box("wr-glass", (RX - 0.8, 0.02, 1.2), (0, RY / 2 + T / 2, 1.65), "glass", CN)

    # main perforated workbench, centred, facing window
    top = box("wr-bench-top", (2.2, 0.95, 0.05), (0, 0.4, BENCH_H - 0.025), "steel", CN)
    hole = cyl("wr-hole-proto", 0.011, 0.2, (-1.0, 0.02, BENCH_H - 0.025), None, CN, verts=12, smooth=False)
    a1 = hole.modifiers.new("ax", 'ARRAY'); a1.count = 39
    a1.use_relative_offset = False; a1.use_constant_offset = True; a1.constant_offset_displace = (0.052, 0, 0)
    a2 = hole.modifiers.new("ay", 'ARRAY'); a2.count = 16
    a2.use_relative_offset = False; a2.use_constant_offset = True; a2.constant_offset_displace = (0, 0.052, 0)
    apply_mods(hole)
    mod = top.modifiers.new("perf", 'BOOLEAN'); mod.object = hole; mod.operation = 'DIFFERENCE'
    apply_mods(top)
    bpy.data.objects.remove(hole)
    for i, (lx, ly) in enumerate([(-1.0, 0.02), (1.0, 0.02), (-1.0, 0.78), (1.0, 0.78)]):
        box(f"wr-bench-leg-{i}", (0.07, 0.07, BENCH_H - 0.05), (lx, ly, (BENCH_H - 0.05) / 2), "darksteel", CN)
    box("wr-bench-shelf", (2.05, 0.8, 0.04), (0, 0.4, 0.25), "darksteel", CN)

    # cabinets along window wall with drawer fronts
    for i, x in enumerate([-2.2, -0.9, 0.4, 1.7]):
        box(f"wr-cab-{i}", (1.25, 0.55, 0.92), (x, RY / 2 - 0.35, 0.46), "steel", CN)
        for r in range(3):
            for c in range(2):
                box(f"wr-cab-{i}-dr-{r}{c}", (0.5, 0.04, 0.22),
                    (x - 0.28 + c * 0.56, RY / 2 - 0.655, 0.2 + r * 0.28), "darksteel", CN)
                cyl(f"wr-cab-{i}-kn-{r}{c}", 0.012, 0.05,
                    (x - 0.28 + c * 0.56, RY / 2 - 0.70, 0.2 + r * 0.28), "bronze", CN,
                    rot=(math.pi / 2, 0, 0), verts=12)

    # brine pipes on right wall with flanges + salt crust at joints
    for j, y in enumerate([-1.6, -0.4, 0.8]):
        cyl(f"wr-pipe-{j}", 0.05, RZ, (RX / 2 - 0.12, y, RZ / 2), "bronze", CN)
        for k, z in enumerate([0.7, 1.5, 2.3]):
            torus(f"wr-pipe-{j}-fl-{k}", 0.07, 0.018, (RX / 2 - 0.12, y, z), "bronze", CN, rot=(0, math.pi / 2, 0))
            sphere(f"wr-pipe-{j}-salt-{k}", 0.055, (RX / 2 - 0.14, y + 0.04, z + 0.05), "crystal", CN)

    # practical lamp over bench
    cyl("wr-lamp-stem", 0.015, 0.5, (-0.7, 0.15, RZ - 0.25), "bronze", CN)
    cone("wr-lamp-shade", 0.16, 0.05, 0.18, (-0.7, 0.15, RZ - 0.55), "bronze", CN)
    sphere("wr-lamp-bulb", 0.05, (-0.7, 0.15, RZ - 0.62), "bulb", CN)

    # wall clock (right wall, like concept)
    cyl("wr-clock-body", 0.28, 0.06, (1.8, -RY / 2 + 0.08, 2.2), "bronze", CN, rot=(math.pi / 2, 0, 0))
    cyl("wr-clock-face", 0.24, 0.02, (1.8, -RY / 2 + 0.115, 2.2), "gauge", CN, rot=(math.pi / 2, 0, 0))
    summary["builtTargets"].append("M7-ENV-watchroom")
except Exception as e:
    summary["errors"].append(f"watchroom: {e!r}")

# ======================================================== M7-PROP-optical-reader
# On the bench, centre ~(0.15, 0.35). Bench top z = BENCH_H.
try:
    CN = "M7-PROP-optical-reader"
    ox, oy, oz = 0.15, 0.35, BENCH_H

    for i, (fx, fy) in enumerate([(-0.2, -0.2), (0.2, -0.2), (-0.2, 0.2), (0.2, 0.2)]):
        cyl(f"rd-foot-{i}", 0.03, 0.03, (ox + fx, oy + fy, oz + 0.015), "bronze", CN, verts=16)
    base = cyl("rd-base", 0.30, 0.11, (ox, oy, oz + 0.085), "bronze", CN, verts=64)
    torus("rd-base-rim", 0.30, 0.012, (ox, oy, oz + 0.14), "bronze", CN)
    cyl("rd-ring", 0.245, 0.05, (ox, oy, oz + 0.16), "bronze", CN, verts=64)
    torus("rd-ring-top", 0.245, 0.01, (ox, oy, oz + 0.185), "bronze", CN)

    # hexagonal crystal plate
    cyl("rd-hex-plate", 0.195, 0.025, (ox, oy, oz + 0.195), "crystal", CN, verts=6, smooth=False)
    # rim clamps
    for i in range(4):
        a = i * math.pi / 2 + math.pi / 4
        box(f"rd-clamp-{i}", (0.045, 0.03, 0.04),
            (ox + 0.225 * math.cos(a), oy + 0.225 * math.sin(a), oz + 0.20), "bronze", CN, rot=(0, 0, a))
    # thumbscrew front
    cyl("rd-thumbscrew", 0.022, 0.05, (ox, oy - 0.25, oz + 0.17), "bronze", CN, rot=(math.pi / 2, 0, 0), verts=20)

    # right optical pillar
    px = ox + 0.42
    cyl("rd-pillar-base", 0.075, 0.06, (px, oy, oz + 0.03), "bronze", CN, verts=32)
    cyl("rd-pillar", 0.045, 0.46, (px, oy, oz + 0.29), "bronze", CN, verts=32)
    torus("rd-pillar-collar", 0.05, 0.012, (px, oy, oz + 0.42), "bronze", CN)
    cyl("rd-pillar-cap", 0.055, 0.05, (px, oy, oz + 0.545), "bronze", CN, verts=32)

    # articulated arm: pillar top -> elbow -> magnifier over plate centre
    elbx = ox + 0.16
    box("rd-arm1", (px - elbx + 0.04, 0.055, 0.055), ((px + elbx) / 2, oy, oz + 0.52), "bronze", CN)
    cyl("rd-elbow", 0.045, 0.08, (elbx, oy, oz + 0.52), "bronze", CN, rot=(math.pi / 2, 0, 0), verts=24)
    box("rd-arm2", (0.05, 0.05, 0.16), (elbx - 0.05, oy, oz + 0.44), "bronze", CN, rot=(0, 0.5, 0))
    # magnifier ring + glass, tilted over plate
    mx, mz = elbx - 0.13, oz + 0.375
    torus("rd-mag-ring", 0.085, 0.016, (mx, oy, mz), "bronze", CN, rot=(0, 0.45, 0))
    cyl("rd-mag-glass", 0.075, 0.008, (mx, oy, mz), "glass", CN, rot=(0, 0.45, 0), verts=48)
    cyl("rd-mag-hinge", 0.02, 0.06, (elbx - 0.035, oy, oz + 0.40), "bronze", CN, rot=(math.pi / 2, 0, 0), verts=16)

    # vertical probe above plate
    cyl("rd-probe-body", 0.022, 0.10, (ox + 0.02, oy + 0.02, oz + 0.30), "bronze", CN, verts=20)
    torus("rd-probe-knurl", 0.026, 0.008, (ox + 0.02, oy + 0.02, oz + 0.335), "bronze", CN)
    cone("rd-probe-needle", 0.001, 0.008, 0.07, (ox + 0.02, oy + 0.02, oz + 0.215), "bronze", CN)

    # handcrank +X side
    cyl("rd-crank-boss", 0.05, 0.07, (ox + 0.315, oy, oz + 0.12), "bronze", CN, rot=(0, math.pi / 2, 0), verts=24)
    cyl("rd-crank-axle", 0.018, 0.10, (ox + 0.38, oy, oz + 0.12), "bronze", CN, rot=(0, math.pi / 2, 0), verts=16)
    box("rd-crank-arm", (0.035, 0.035, 0.16), (ox + 0.43, oy, oz + 0.185), "bronze", CN)
    cyl("rd-crank-handle", 0.023, 0.11, (ox + 0.43, oy, oz + 0.27), "wood", CN, rot=(0, math.pi / 2, 0), verts=20)
    summary["builtTargets"].append("M7-PROP-optical-reader")
except Exception as e:
    summary["errors"].append(f"reader: {e!r}")

# ========================================================== M7-PROP-record-set
# On bench left of reader.
try:
    CN = "M7-PROP-record-set"
    bx, bz = -0.72, BENCH_H
    # preserved copy: small hex salt plate in shallow bronze tray
    cyl("rc-tray", 0.135, 0.03, (bx, 0.22, bz + 0.015), "bronze", CN, verts=6, smooth=False)
    cyl("rc-tray-cav", 0.115, 0.03, (bx, 0.22, bz + 0.033), "darksteel", CN, verts=6, smooth=False)
    cyl("rc-copy-hex", 0.105, 0.018, (bx, 0.22, bz + 0.05), "crystal", CN, verts=6, smooth=False)

    # ledger: dark cover + paper block
    box("rc-ledger-cover", (0.30, 0.42, 0.012), (bx - 0.02, 0.62, bz + 0.006), "darksteel", CN, rot=(0, 0, 0.06))
    box("rc-ledger-pages", (0.28, 0.40, 0.05), (bx - 0.02, 0.62, bz + 0.037), "paper", CN, rot=(0, 0, 0.06))
    box("rc-ledger-top", (0.30, 0.42, 0.012), (bx - 0.02, 0.62, bz + 0.068), "darksteel", CN, rot=(0, 0, 0.06))

    # loose paper stack
    box("rc-stack", (0.26, 0.36, 0.045), (bx + 0.33, 0.65, bz + 0.0225), "paper", CN, rot=(0, 0, -0.09))

    # review stand: inclined easel with blank card
    sx, sy = bx - 0.05, -0.08
    box("rc-stand-base", (0.26, 0.18, 0.02), (sx, sy, bz + 0.01), "darksteel", CN)
    box("rc-stand-board", (0.24, 0.015, 0.30), (sx, sy + 0.05, bz + 0.16), "darksteel", CN, rot=(-0.45, 0, 0))
    box("rc-stand-card", (0.19, 0.008, 0.24), (sx, sy + 0.035, bz + 0.155), "paper", CN, rot=(-0.45, 0, 0))
    box("rc-stand-lip", (0.24, 0.03, 0.02), (sx, sy + 0.115, bz + 0.035), "darksteel", CN, rot=(-0.45, 0, 0))

    # blank review cards + bronze clip
    for i, (cx, cy, rot) in enumerate([(0.35, 0.1, 0.12), (0.38, 0.14, -0.07), (0.33, 0.18, 0.03)]):
        box(f"rc-card-{i}", (0.14, 0.095, 0.004), (bx + cx, cy, bz + 0.002 + i * 0.004), "paper", CN, rot=(0, 0, rot))
    box("rc-clip", (0.05, 0.025, 0.018), (bx + 0.30, 0.085, bz + 0.02), "bronze", CN)
    summary["builtTargets"].append("M7-PROP-record-set")
except Exception as e:
    summary["errors"].append(f"records: {e!r}")

# =========================================================== M7-ENV-gate-three
# Exterior set at +X 25m. Two concrete piers, closed gate leaf, winch, water.
try:
    CN = "M7-ENV-gate-three"
    GX = 25.0
    PW, PD, PH = 1.6, 2.2, 5.2       # pier width/depth/height
    GAP = 3.4                        # gate opening
    WATER = 1.1                      # water level z
    for s, tag in [(-1, "L"), (1, "R")]:
        px = GX + s * (GAP / 2 + PW / 2)
        box(f"gt-pier-{tag}", (PW, PD, PH), (px, 0, PH / 2), "concrete", CN)
        box(f"gt-pier-{tag}-cap", (PW + 0.15, PD + 0.15, 0.18), (px, 0, PH + 0.09), "concrete", CN)
        # vertical brine pipes on outer faces
        for j, yy in enumerate([-0.6, 0.6]):
            cyl(f"gt-pipe-{tag}-{j}", 0.055, PH, (px + s * (PW / 2 + 0.10), yy, PH / 2), "bronze", CN)
            for k, zz in enumerate([1.4, 2.6, 3.8]):
                torus(f"gt-pipe-{tag}-{j}-fl-{k}", 0.075, 0.018,
                      (px + s * (PW / 2 + 0.10), yy, zz), "bronze", CN, rot=(0, math.pi / 2, 0))
                sphere(f"gt-salt-{tag}-{j}-{k}", 0.06,
                       (px + s * (PW / 2 + 0.12), yy + 0.05, zz + 0.06), "crystal", CN)

    # side walls extending outward
    box("gt-wall-L", (7.0, 1.6, 2.6), (GX - GAP / 2 - PW - 3.5, 0, 1.3), "concrete", CN)
    box("gt-wall-R", (7.0, 1.6, 2.6), (GX + GAP / 2 + PW + 3.5, 0, 1.3), "concrete", CN)

    # closed gate leaf with horizontal ribs + guide rails
    box("gt-leaf", (GAP, 0.22, 3.8), (GX, 0, 1.9), "steel", CN)
    for i, z in enumerate([1.1, 2.1, 3.1]):
        box(f"gt-rib-{i}", (GAP - 0.2, 0.10, 0.22), (GX, -0.16, z), "steel", CN)
    for s, tag in [(-1, "L"), (1, "R")]:
        box(f"gt-rail-{tag}", (0.12, 0.30, 4.4), (GX + s * (GAP / 2 - 0.06), 0, 2.2), "darksteel", CN)

    # overhead winch platform + drum + chains
    box("gt-platform", (GAP + PW * 2, 1.4, 0.12), (GX, 0, PH + 0.24), "darksteel", CN)
    drum = cyl("gt-drum", 0.24, 1.6, (GX, 0, PH + 0.75), "darksteel", CN, rot=(0, math.pi / 2, 0), verts=32)
    for i in range(7):
        torus(f"gt-rope-{i}", 0.245, 0.022, (GX - 0.55 + i * 0.18, 0, PH + 0.75), "bronze", CN, rot=(0, math.pi / 2, 0))
    for s in (-1, 1):
        box(f"gt-drum-mount-{'L' if s < 0 else 'R'}", (0.12, 0.5, 0.55), (GX + s * 0.95, 0, PH + 0.55), "darksteel", CN)
    for s in (-1, 1):
        cyl(f"gt-chain-{'L' if s < 0 else 'R'}", 0.03, PH + 0.75 - 3.8, (GX + s * (GAP / 2 - 0.35), 0, (PH + 0.75 + 3.8) / 2), "darksteel", CN, verts=10)
    # railing on platform
    for s in (-1, 1):
        box(f"gt-rail-top-{'L' if s < 0 else 'R'}", (GAP + PW * 2 - 0.3, 0.03, 0.03), (GX, s * 0.62, PH + 1.25), "darksteel", CN)
        for i in range(6):
            cyl(f"gt-baluster-{'L' if s < 0 else 'R'}-{i}", 0.015, 0.95, (GX - (GAP + PW * 2) / 2 + 0.4 + i * (GAP + PW * 2 - 0.8) / 5, s * 0.62, PH + 0.78), "darksteel", CN, verts=10)

    # gauges + handwheel on left pier face (-Y toward camera)
    fpx = GX - GAP / 2 - PW / 2
    for i, gz in enumerate([3.55, 3.0]):
        cyl(f"gt-gauge-{i}", 0.16 - i * 0.02, 0.07, (fpx - 0.25 + i * 0.55, -PD / 2 - 0.035, gz), "bronze", CN, rot=(math.pi / 2, 0, 0), verts=32)
        cyl(f"gt-gauge-face-{i}", 0.125 - i * 0.02, 0.02, (fpx - 0.25 + i * 0.55, -PD / 2 - 0.075, gz), "gauge", CN, rot=(math.pi / 2, 0, 0), verts=32)
    hw = torus("gt-handwheel", 0.22, 0.028, (fpx, -PD / 2 - 0.12, 2.2), "bronze", CN, rot=(math.pi / 2, 0, 0))
    for i in range(4):
        a = i * math.pi / 2
        box(f"gt-spoke-{i}", (0.42, 0.03, 0.03), (fpx, -PD / 2 - 0.12, 2.2), "bronze", CN, rot=(0, a, math.pi / 2))
    cyl("gt-hw-hub", 0.05, 0.10, (fpx, -PD / 2 - 0.10, 2.2), "bronze", CN, rot=(math.pi / 2, 0, 0), verts=20)

    # lamp on platform
    cyl("gt-lamp-post", 0.03, 0.9, (GX - GAP / 2 - 0.8, -0.5, PH + 0.75), "darksteel", CN)
    cone("gt-lamp-shade", 0.14, 0.04, 0.14, (GX - GAP / 2 - 0.8, -0.5, PH + 1.28), "bronze", CN)
    sphere("gt-lamp-bulb", 0.045, (GX - GAP / 2 - 0.8, -0.5, PH + 1.22), "bulb", CN)

    # water + wet line marker geometry (wet zone darker band is in material; keep sea plane)
    box("gt-water", (26, 18, 0.05), (GX, -5.5, WATER), "water", CN)
    summary["builtTargets"].append("M7-ENV-gate-three")
except Exception as e:
    summary["errors"].append(f"gate: {e!r}")

# ---------------------------------------------------------------- environment
try:
    CN = "M7-lighting"
    # sea backdrop + horizon glow behind watchroom window
    box("env-sea", (40, 30, 0.05), (0, 20, 0.4), "water", CN)
    box("env-breakwater", (30, 1.2, 1.1), (0, 14, 0.9), "concrete", CN)

    w = bpy.data.worlds.new("M7-world")
    scene.world = w
    w.use_nodes = True
    bg = w.node_tree.nodes["Background"]
    bg.inputs["Color"].default_value = (0.012, 0.02, 0.032, 1.0)
    bg.inputs["Strength"].default_value = 0.35

    def light(name, kind, loc, energy, color=(1, 1, 1), size=1.0, rot=(0, 0, 0)):
        ld = bpy.data.lights.new(name, kind)
        ld.energy = energy
        ld.color = color
        if kind == 'AREA': ld.size = size
        if kind == 'SPOT': ld.spot_size = 1.2
        lo = bpy.data.objects.new(name, ld)
        lo.location = loc; lo.rotation_euler = rot
        coll(CN).objects.link(lo)
        return lo

    # watchroom: cool sea light through window + warm practical
    light("wl-sea", 'AREA', (0, 3.4, 1.9), 550, (0.55, 0.72, 1.0), size=5.0, rot=(math.radians(-95), 0, 0))
    light("wl-lamp", 'POINT', (-0.7, 0.15, 2.32), 90, (1.0, 0.72, 0.42))
    light("wl-fill", 'AREA', (-2.5, -1.8, 2.7), 90, (0.7, 0.8, 1.0), size=3.0, rot=(math.radians(-35), math.radians(-25), 0))
    light("wl-bench", 'AREA', (0.1, 0.3, 2.6), 60, (0.9, 0.85, 0.75), size=1.6, rot=(0, 0, 0))
    # gate: moon/storm key + warm lamp + rim
    light("gl-key", 'AREA', (25, -9, 9.5), 2200, (0.6, 0.75, 1.0), size=10.0, rot=(math.radians(45), 0, 0))
    light("gl-rim", 'AREA', (25, 6, 7.0), 2500, (0.45, 0.6, 0.95), size=8.0, rot=(math.radians(-60), 0, 0))
    light("gl-lamp", 'POINT', (25 - 1.7 - 0.8, -0.5, 5.2 + 1.18), 120, (1.0, 0.72, 0.42))
except Exception as e:
    summary["errors"].append(f"lighting: {e!r}")

# ---------------------------------------------------------------- cameras
def cam(name, loc, rot, lens=35):
    cd = bpy.data.cameras.new(name)
    cd.lens = lens
    co = bpy.data.objects.new(name, cd)
    co.location = loc; co.rotation_euler = rot
    coll("M7-cameras").objects.link(co)
    return co

cam_key = cam("CAM-watchroom-key", (-1.7, -1.95, 1.55), (math.radians(76), 0, math.radians(-30)), 35)
cam_reader = cam("CAM-reader-close", (-0.52, -0.68, 1.42), (math.radians(65), 0, math.radians(-33)), 45)
cam_gate = cam("CAM-gate-three", (25.0, -11.5, 3.0), (math.radians(87), 0, 0), 28)

# ---------------------------------------------------------------- render
scene.render.engine = 'CYCLES'
scene.cycles.samples = 96
scene.cycles.use_denoising = True
prefs = bpy.context.preferences.addons.get("cycles")
if prefs:
    cp = prefs.preferences
    try:
        cp.compute_device_type = 'METAL'
        cp.get_devices()
        for d in cp.devices: d.use = True
        scene.cycles.device = 'GPU'
    except Exception:
        scene.cycles.device = 'CPU'
scene.render.resolution_x = 1280
scene.render.resolution_y = 720

for camera, fname in [(cam_key, "watchroom-key.png"), (cam_reader, "reader-close.png"), (cam_gate, "gate-three.png")]:
    scene.camera = camera
    scene.render.filepath = os.path.join(RENDERS, fname)
    bpy.ops.render.render(write_still=True)
    summary.setdefault("renders", []).append(fname)

# save blend (pack textures for portability)
bpy.ops.file.pack_all()
blend_path = os.path.join(OUT, "concept-first-m7.blend")
bpy.ops.wm.save_as_mainfile(filepath=blend_path)
summary["blend"] = blend_path
summary["objectCount"] = len(bpy.data.objects)

print("M7_BUILD_SUMMARY=" + json.dumps(summary))
