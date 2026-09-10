# Execute with Blender --background --factory-startup --python <new-script.py>.
from pathlib import Path
import bpy, json
from mathutils import Vector

out = Path('/Users/jangyoung/orca/unknown/assets/generated/3d/hub-view-drawer-r01')
asset = 'SM_Hub_Workbench_Drawer'
assert not bpy.data.filepath, 'Use a fresh factory scene, not an existing project'
assert not any((out / (asset + ext)).exists() for ext in ('.blend', '.glb', '.fbx'))
out.mkdir(parents=True, exist_ok=True)
initial = [{'name': o.name, 'type': o.type} for o in bpy.data.objects]
for o in bpy.data.objects:
    o.hide_render = True  # Preserve the factory objects, do not delete them.
scene = bpy.context.scene
scene.unit_settings.system = 'METRIC'
scene.unit_settings.scale_length = 1.0
collection = bpy.data.collections.new('HUB_DRAWER_CANDIDATE')
scene.collection.children.link(collection)

def empty(name, parent=None):
    o = bpy.data.objects.new(name, None)
    collection.objects.link(o)
    o.parent = parent
    return o

def material(name, hexcode):
    def linear(v):
        c = int(v, 16) / 255
        return c / 12.92 if c <= .04045 else ((c + .055) / 1.055) ** 2.4
    m = bpy.data.materials.new(name)
    m.use_nodes = True
    color = tuple(linear(hexcode[i:i+2]) for i in (0, 2, 4)) + (1,)
    m.diffuse_color = color
    bsdf = m.node_tree.nodes.get('Principled BSDF')
    bsdf.inputs['Base Color'].default_value = color
    bsdf.inputs['Roughness'].default_value = .8
    return m

mats = [material('MAT_Hub_Drawer_Casing', '36565C'),
        material('MAT_Hub_Drawer_Inset', '173238'),
        material('MAT_Hub_Drawer_Handle', '4F7A6B')]
root = empty('ROOT_Hub_Workbench_Drawer')
tray = empty('JNT_Hub_Workbench_Drawer_tray', root)
tray.lock_location = (True, False, True)
tray.lock_rotation = (True, True, True)
tray.lock_scale = (True, True, True)

# Each component tuple = (centre XYZ metres, dimensions XYZ metres, material index).
# Disconnected boxes are joined into two mesh objects, keeping one moving rigid body.
def mesh_boxes(name, components, parent):
    verts, faces, indices = [], [], []
    quad = [(0,3,2,1),(4,5,6,7),(0,1,5,4),(1,2,6,5),(2,3,7,6),(3,0,4,7)]
    signs = [(-1,-1,-1),(1,-1,-1),(1,1,-1),(-1,1,-1),
             (-1,-1,1),(1,-1,1),(1,1,1),(-1,1,1)]
    for centre, dimensions, mi in components:
        offset = len(verts)
        verts.extend(tuple(centre[j] + s[j]*dimensions[j]/2 for j in range(3)) for s in signs)
        faces.extend(tuple(offset + i for i in face) for face in quad)
        indices.extend([mi]*6)
    data = bpy.data.meshes.new('MSH_' + name)
    data.from_pydata(verts, [], faces)
    data.update()
    obj = bpy.data.objects.new(name, data)
    collection.objects.link(obj)
    obj.parent = parent
    for mat in mats:
        data.materials.append(mat)
    for poly, index in zip(data.polygons, indices):
        poly.material_index = index
    return obj

casing = mesh_boxes(asset + '_Casing', [
    ((0,0,.015),(1.10,.70,.03),0), ((0,0,.245),(1.10,.70,.03),0),
    ((-.535,0,.13),(.03,.70,.20),0), ((.535,0,.13),(.03,.70,.20),0),
    ((0,.335,.13),(1.04,.03,.20),0)], root)
moving = mesh_boxes(asset + '_Tray', [
    ((0,-.322,.13),(1.02,.036,.18),0), ((0,-.010,.055),(.98,.59,.02),1),
    ((-.48,-.010,.115),(.02,.59,.10),1), ((.48,-.010,.115),(.02,.59,.10),1),
    ((0,.275,.115),(.98,.02,.10),1),
    ((-.15,-.355,.13),(.024,.05,.024),2), ((.15,-.355,.13),(.024,.05,.024),2),
    ((0,-.383,.13),(.324,.024,.024),2)], tray)
mesh_objects = [casing, moving]
bpy.context.view_layer.update()
measurements = {'asset_id': asset, 'status': 'blender-measured', 'runtimeEligible': False,
                'blender_version': bpy.app.version_string, 'initial_scene': initial,
                'tri_budget_target': 1500, 'texture_files': 0, 'objects': []}
for obj in mesh_objects:
    obj.data.calc_loop_triangles()
    measurements['objects'].append({'name': obj.name, 'vertices': len(obj.data.vertices),
        'triangles': len(obj.data.loop_triangles), 'polygons': len(obj.data.polygons),
        'material_slots': len(obj.material_slots), 'location': list(obj.location),
        'rotation': list(obj.rotation_euler), 'scale': list(obj.scale),
        'bounds_local': [list(Vector(c)) for c in obj.bound_box]})
measurements['triangles'] = sum(o['triangles'] for o in measurements['objects'])
assert measurements['triangles'] <= measurements['tri_budget_target']
(out / 'measurements.json').write_text(json.dumps(measurements, indent=2) + '\n')
bpy.ops.object.select_all(action='DESELECT')
for obj in (root, tray, casing, moving):
    obj.select_set(True)
bpy.context.view_layer.objects.active = casing
bpy.ops.export_scene.gltf(filepath=str(out / (asset + '.glb')), export_format='GLB',
    use_selection=True, export_apply=True, export_yup=True, export_animations=False)
bpy.ops.export_scene.fbx(filepath=str(out / (asset + '.fbx')), use_selection=True,
    object_types={'MESH', 'EMPTY'}, axis_up='Y', axis_forward='-Z', global_scale=1.0,
    apply_unit_scale=True, apply_scale_options='FBX_SCALE_NONE', bake_space_transform=False,
    bake_anim=False, add_leaf_bones=False, mesh_smooth_type='FACE', path_mode='AUTO')
bpy.ops.wm.save_as_mainfile(filepath=str(out / (asset + '.blend')))
print(json.dumps(measurements))
