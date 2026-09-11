# build_hub_greybox.py
# 허브(당직실) 그레이박스 + 도구 프롭 블록아웃 — 컨셉/프리비즈용. 게임플레이 자산이 아니다.
# [OBSERVED] Blender 5.1.2 / Python 3.13.9 에서 MCP(execute_blender_code)로 단계별 실행.
# owner: game-modeler / cycle: 20260909-preproduction-c4
# 안전: 사용자 오브젝트(Cube/Light/Camera)를 삭제하지 않는다. 숨기고 새 컬렉션에서 작업한다.
# 캐논: 도구 id 6종 = circuit reader alignment routing corrosion seal (planning/gdd.md L87 [OBSERVED])
# 명명: SM_<zone>_<name> / SM_Tool_<toolId>. 피벗 = 프롭 바닥 중심(도구 원점). 단위 1m.

import bpy, os, json, math
from mathutils import Vector

ROOT = "/Users/jangyoung/orca/unknown"
OUT3D = os.path.join(ROOT, "assets/generated/3d")
RENDERS = os.path.join(OUT3D, "renders")
TURNTABLE = os.path.join(RENDERS, "turntable")
COLL = "HUB_GREYBOX"

# 그레이박스 전용 디버그 색 [OBSERVED 2026-09-10 · RFC-M3 주석 정정]:
# concept/style-guide.md 는 **존재하며**(§2 팔레트 기본 8색 + §2.1 색약 대체 세트) 최종 재질의 정본이다.
# 아래 ACCENT 6 + GREY_* 3 = 9 값은 뷰포트에서 도구/셸/바닥을 구분하기 위한 **디버그 전용**이며
# 최종 재질 후보가 아니다. style-guide 팔레트와 교집합 0 (재현: asset-runbook.md §7-4 스크립트,
# 2026-09-10 실행 결과 intersection = []). 도구 구분의 정본 부호는 style-guide §9 형상·글리프이고
# 색 단독 부호화는 금지다(런북 §5-8). 이 주석 정정으로 색 값·로직은 바뀌지 않는다 — 재실행 불요.
ACCENT = {
    "circuit":   "3F8EA8",
    "reader":    "7E6FB0",
    "alignment": "4FA07A",
    "routing":   "C4703F",
    "corrosion": "B0566B",
    "seal":      "8A8F5C",
}
GREY_SHELL   = "9A9A96"
GREY_FLOOR   = "7E7E7A"
GREY_FIXTURE = "8C8C88"


def srgb_to_linear(c):
    c = c / 255.0
    return c / 12.92 if c <= 0.04045 else ((c + 0.055) / 1.055) ** 2.4


def hex_rgba(h, a=1.0):
    return (srgb_to_linear(int(h[0:2], 16)), srgb_to_linear(int(h[2:4], 16)),
            srgb_to_linear(int(h[4:6], 16)), a)


def get_coll():
    if COLL in bpy.data.collections:
        return bpy.data.collections[COLL]
    c = bpy.data.collections.new(COLL)
    bpy.context.scene.collection.children.link(c)
    return c


def make_material(name, hexcol):
    mat = bpy.data.materials.get(name) or bpy.data.materials.new(name)
    mat.use_nodes = True
    bsdf = mat.node_tree.nodes.get("Principled BSDF")
    rgba = hex_rgba(hexcol)
    if bsdf:
        bsdf.inputs["Base Color"].default_value = rgba
        if "Roughness" in bsdf.inputs:
            bsdf.inputs["Roughness"].default_value = 0.75
        if "Metallic" in bsdf.inputs:
            bsdf.inputs["Metallic"].default_value = 0.0
    mat.diffuse_color = rgba          # 뷰포트 솔리드 표시색
    return mat


def make_box(name, size, location, material, rot_z_deg=0.0):
    """size=(x,y,z) 미터. 피벗 = 바닥 중심. 트랜스폼은 마지막에 apply 하여 identity 로 만든다."""
    sx, sy, sz = size
    hx, hy = sx / 2.0, sy / 2.0
    verts = [(-hx, -hy, 0), (hx, -hy, 0), (hx, hy, 0), (-hx, hy, 0),
             (-hx, -hy, sz), (hx, -hy, sz), (hx, hy, sz), (-hx, hy, sz)]
    faces = [(0, 1, 2, 3), (4, 7, 6, 5), (0, 4, 5, 1),
             (1, 5, 6, 2), (2, 6, 7, 3), (3, 7, 4, 0)]
    me = bpy.data.meshes.new("MSH_" + name)
    me.from_pydata(verts, [], faces)
    me.update()
    ob = bpy.data.objects.new(name, me)
    ob.location = location
    ob.rotation_euler = (0.0, 0.0, math.radians(rot_z_deg))
    get_coll().objects.link(ob)
    ob.data.materials.append(material)
    return ob


def look_at(ob, target):
    d = Vector(target) - ob.location
    ob.rotation_euler = d.to_track_quat('-Z', 'Y').to_euler()


def tri_count(ob):
    """폴리곤 팬 기준 삼각형 수 = 익스포터가 삼각화했을 때의 수. 추정이 아니라 계산값."""
    dg = bpy.context.evaluated_depsgraph_get()
    ev = ob.evaluated_get(dg)
    me = ev.to_mesh()
    n = sum(len(p.vertices) - 2 for p in me.polygons)
    ev.to_mesh_clear()
    return n


def greybox_objects():
    c = bpy.data.collections.get(COLL)
    return [o for o in c.objects if o.type == 'MESH'] if c else []


def select_only(objs):
    bpy.ops.object.select_all(action='DESELECT')
    for o in objs:
        o.select_set(True)
    if objs:
        bpy.context.view_layer.objects.active = objs[0]


def purge_greybox():
    """이 스크립트가 만든 HUB_GREYBOX 컬렉션의 내용만 비운다.
    사용자 오브젝트(Cube/Light/Camera)는 이 컬렉션 밖에 있으므로 대상이 아니다."""
    c = bpy.data.collections.get(COLL)
    if c is None:
        return []
    names = [o.name for o in list(c.objects)]
    for o in list(c.objects):
        bpy.data.objects.remove(o, do_unlink=True)
    for me in list(bpy.data.meshes):
        if me.users == 0 and me.name.startswith("MSH_SM_"):
            bpy.data.meshes.remove(me)
    return names


def build_all():
    """클린 리빌드: 스크립트 1회 실행 == 저장되는 .blend 상태."""
    purge_greybox()
    out = {"stage_00": stage_00_setup(), "stage_10": stage_10_materials(),
           "stage_20": stage_20_shell(), "stage_30": stage_30_fixtures(),
           "stage_40": stage_40_tools(), "stage_50": stage_50_camera_lights()}
    out["stage_60"] = stage_60_measure()
    return out


# ---------------------------------------------------------------- stages
def stage_00_setup():
    sc = bpy.context.scene
    sc.unit_settings.system = 'METRIC'
    sc.unit_settings.scale_length = 1.0
    sc.unit_settings.length_unit = 'METERS'
    # 사용자 기본 오브젝트는 삭제하지 않고 숨긴다
    hidden = []
    for n in ("Cube", "Light", "Camera"):
        o = bpy.data.objects.get(n)
        if o:
            o.hide_viewport = True
            o.hide_render = True
            hidden.append(n)
    get_coll()
    # 새 월드(기존 World 데이터블록을 건드리지 않는다)
    w = bpy.data.worlds.get("WLD_HubGreybox") or bpy.data.worlds.new("WLD_HubGreybox")
    w.use_nodes = True
    bg = w.node_tree.nodes.get("Background")
    if bg:
        bg.inputs[0].default_value = hex_rgba("2F353A")
        bg.inputs[1].default_value = 1.0
    sc.world = w
    sc.render.engine = 'BLENDER_EEVEE'
    sc.render.image_settings.file_format = 'PNG'
    for d in (OUT3D, RENDERS, TURNTABLE, os.path.join(OUT3D, "scripts")):
        os.makedirs(d, exist_ok=True)
    return {"hidden_user_objects": hidden, "collection": COLL}


def stage_10_materials():
    mats = {"MAT_Grey_Shell": GREY_SHELL, "MAT_Grey_Floor": GREY_FLOOR,
            "MAT_Grey_Fixture": GREY_FIXTURE}
    for tid, h in ACCENT.items():
        mats["MAT_Tool_" + tid] = h
    for n, h in mats.items():
        make_material(n, h)
    return sorted(mats.keys())


def stage_20_shell():
    """바닥 6(X) x 8(Y) m, 벽 3면(북/서/동). 남쪽은 2.5D 카메라를 위해 열어 둔다."""
    floor = bpy.data.materials["MAT_Grey_Floor"]
    shell = bpy.data.materials["MAT_Grey_Shell"]
    made = []
    made.append(make_box("SM_Hub_Floor", (6.0, 8.0, 0.10), (0, 0, -0.10), floor))
    made.append(make_box("SM_Hub_Wall_N", (6.0, 0.12, 2.60), (0, 3.94, 0.0), shell))
    made.append(make_box("SM_Hub_Wall_W", (0.12, 8.0, 2.60), (-2.94, 0, 0.0), shell))
    made.append(make_box("SM_Hub_Wall_E", (0.12, 8.0, 2.60), (2.94, 0, 0.0), shell))
    return [o.name for o in made]


def stage_30_fixtures():
    fx = bpy.data.materials["MAT_Grey_Fixture"]
    made = []
    # 작업대 2.4 x 0.9 x 0.9 — 허브의 기준 오브젝트(카메라가 정면으로 본다)
    made.append(make_box("SM_Hub_Workbench", (2.40, 0.90, 0.90), (0.0, 1.20, 0.0), fx))
    # 염판 선반 — 동쪽 벽면
    made.append(make_box("SM_Hub_PlateShelf", (0.36, 1.80, 1.15), (2.55, 0.75, 0.0), fx))
    return [o.name for o in made]


def stage_40_tools():
    """도구 6종. 피벗은 각 프롭 바닥 중심(= 애니 피벗 = 도구 원점)."""
    M = lambda t: bpy.data.materials["MAT_Tool_" + t]
    made = []
    # 작업대 위: 판독기 / 서명대 (작업대 상면 z=0.90)
    made.append(make_box("SM_Tool_reader", (0.80, 0.50, 0.34), (-0.62, 1.22, 0.90), M("reader")))
    made.append(make_box("SM_Tool_seal",   (0.70, 0.44, 0.22), (0.68, 1.18, 0.90), M("seal")))
    # 벽 패널: 회로 지도(북) / 배수 편성(서)
    made.append(make_box("SM_Tool_circuit", (2.00, 0.09, 1.20), (0.0, 3.79, 1.05), M("circuit")))
    made.append(make_box("SM_Tool_routing", (0.09, 1.60, 1.00), (-2.83, 0.60, 1.10), M("routing")))
    # 바닥 설치: 조위정합 콘솔(동쪽, 15° 틀어 카메라 쪽) / 부식 시험대(서쪽)
    made.append(make_box("SM_Tool_alignment", (1.20, 0.60, 1.00), (1.85, 2.55, 0.0), M("alignment"), rot_z_deg=-15.0))
    made.append(make_box("SM_Tool_corrosion", (1.00, 0.70, 0.85), (-2.00, 0.45, 0.0), M("corrosion"), rot_z_deg=12.0))
    # 회전/스케일을 메시에 굽는다 → 오브젝트 트랜스폼 identity, 원점(피벗)은 그대로
    select_only(greybox_objects())
    bpy.ops.object.transform_apply(location=False, rotation=True, scale=True)
    return [o.name for o in made]


def stage_50_camera_lights():
    """2.5D 고정 시점 카메라 1대(약 35° 부감, 작업대 정면) + 3점 조명."""
    target = Vector((0.0, 1.20, 0.95))       # 작업대 상면 중심
    pitch, dist = 35.0, 6.5
    cam_data = bpy.data.cameras.get("CAMDATA_Hub_Fixed") or bpy.data.cameras.new("CAMDATA_Hub_Fixed")
    cam_data.lens = 34.0
    cam = bpy.data.objects.get("CAM_Hub_Fixed")
    if cam is None:
        cam = bpy.data.objects.new("CAM_Hub_Fixed", cam_data)
        get_coll().objects.link(cam)
    cam.location = (0.0, target.y - dist, target.z + dist * math.tan(math.radians(pitch)))
    look_at(cam, target)
    bpy.context.scene.camera = cam

    # 턴테이블 전용 카메라(같은 렌즈/피치, 작업대 중심을 30°씩 공전)
    tt_data = bpy.data.cameras.get("CAMDATA_Hub_Turntable") or bpy.data.cameras.new("CAMDATA_Hub_Turntable")
    tt_data.lens = 34.0
    tt = bpy.data.objects.get("CAM_Hub_Turntable")
    if tt is None:
        tt = bpy.data.objects.new("CAM_Hub_Turntable", tt_data)
        get_coll().objects.link(tt)
    tt.location = cam.location
    look_at(tt, target)

    specs = [
        ("LGT_Hub_Key",  'AREA', 190.0, (-2.10, -1.60, 2.90), 2.6),
        ("LGT_Hub_Fill", 'AREA', 80.0, (2.60, -2.30, 2.30), 3.4),
        ("LGT_Hub_Rim",  'AREA', 130.0, (0.60, 3.10, 2.45), 2.0),
    ]
    made = []
    for name, ltype, power, loc, size in specs:
        ld = bpy.data.lights.get("LDATA_" + name) or bpy.data.lights.new("LDATA_" + name, type=ltype)
        ld.type = ltype
        ld.energy = power
        ld.size = size
        ob = bpy.data.objects.get(name)
        if ob is None:
            ob = bpy.data.objects.new(name, ld)
            get_coll().objects.link(ob)
        ob.location = loc
        look_at(ob, target)
        made.append(name)
    return {"camera": cam.name, "turntable_cam": tt.name, "lights": made,
            "cam_location": [round(v, 3) for v in cam.location], "lens_mm": 34.0,
            "pitch_deg": pitch}


def stage_60_measure():
    bpy.context.view_layer.update()
    per = {o.name: tri_count(o) for o in sorted(greybox_objects(), key=lambda x: x.name)}
    return {"per_object_tris": per, "total_tris": sum(per.values()),
            "mesh_objects": len(per), "materials": len([m for m in bpy.data.materials
                                                        if m.name.startswith("MAT_")])}


def stage_70_export():
    out = {}
    objs = greybox_objects()
    select_only(objs)
    glb = os.path.join(OUT3D, "hub-greybox.glb")
    bpy.ops.export_scene.gltf(filepath=glb, export_format='GLB', use_selection=True,
                              export_apply=True, export_yup=True)
    out["glb"] = glb
    fbx = os.path.join(OUT3D, "hub-greybox.fbx")
    select_only(objs)
    bpy.ops.export_scene.fbx(filepath=fbx, use_selection=True, global_scale=1.0,
                             apply_unit_scale=True, apply_scale_options='FBX_SCALE_NONE',
                             axis_forward='-Z', axis_up='Y', bake_space_transform=False,
                             object_types={'MESH'}, use_mesh_modifiers=True,
                             mesh_smooth_type='FACE', path_mode='COPY')
    out["fbx"] = fbx
    out["fbx_operator"] = "bpy.ops.export_scene.fbx"
    # 도구 프롭 개별 GLB — 원점으로 옮겨 내보내고 되돌린다(프롭 로컬 좌표계)
    tools = []
    for o in objs:
        if not o.name.startswith("SM_Tool_"):
            continue
        keep = tuple(o.location)
        o.location = (0.0, 0.0, 0.0)
        select_only([o])
        p = os.path.join(OUT3D, o.name + ".glb")
        bpy.ops.export_scene.gltf(filepath=p, export_format='GLB', use_selection=True,
                                  export_apply=True, export_yup=True)
        o.location = keep
        tools.append(p)
    out["tool_glb"] = tools
    return out


def stage_80_render():
    sc = bpy.context.scene
    sc.render.engine = 'BLENDER_EEVEE'
    try:
        sc.eevee.taa_render_samples = 64
    except Exception:
        pass
    sc.render.image_settings.file_format = 'PNG'
    sc.render.film_transparent = False
    # 1) 고정 카메라 키샷 1920x1080
    sc.camera = bpy.data.objects["CAM_Hub_Fixed"]
    sc.render.resolution_x, sc.render.resolution_y = 1920, 1080
    sc.render.resolution_percentage = 100
    hero = os.path.join(RENDERS, "hub-cam.png")
    sc.render.filepath = hero
    bpy.ops.render.render(write_still=True)
    # 2) 턴테이블 12프레임 960x540 (작업대 중심 30° 간격)
    target = Vector((0.0, 1.20, 0.95))
    tt = bpy.data.objects["CAM_Hub_Turntable"]
    sc.camera = tt
    sc.render.resolution_x, sc.render.resolution_y = 960, 540
    r, h = 6.5, 0.95 + 6.5 * math.tan(math.radians(35.0))
    frames = []
    for i in range(12):
        a = math.radians(-90.0 + i * 30.0)     # i=0 은 고정 카메라와 같은 남쪽 방향
        tt.location = (target.x + r * math.cos(a), target.y + r * math.sin(a), h)
        look_at(tt, target)
        p = os.path.join(TURNTABLE, "frame_%02d.png" % i)
        sc.render.filepath = p
        bpy.ops.render.render(write_still=True)
        frames.append(p)
    sc.camera = bpy.data.objects["CAM_Hub_Fixed"]
    sc.render.resolution_x, sc.render.resolution_y = 1920, 1080
    return {"hero": hero, "turntable": frames}


def stage_90_save():
    p = os.path.join(OUT3D, "hub-greybox.blend")
    bpy.ops.wm.save_as_mainfile(filepath=p)
    return {"blend": p, "filepath_now": bpy.data.filepath}
