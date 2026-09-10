# Execute with Blender --background --factory-startup --python <new-script.py>.
from pathlib import Path
import bpy, json
from mathutils import Vector

out = Path('/Users/jangyoung/orca/unknown/assets/generated/3d/hub-view-drawer-r02')
asset = 'SM_Hub_Workbench_Drawer'
assert not bpy.data.filepath, 'Use a fresh factory scene, not an existing project'
assert not out.exists(), 'Preserve previous output; choose a new revision directory'
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

# r02 authored surface correction. No new geometry; diffuse/roughness atlases survive GLB export.
import math, hashlib
from array import array
SIZE = 1024
PAD = 3
texture_dir = out / 'textures'
texture_dir.mkdir()

def linear_hex(code):
    def linear(c):
        v = int(c, 16) / 255.0
        return v / 12.92 if v <= .04045 else ((v + .055) / 1.055) ** 2.4
    return tuple(linear(code[i:i+2]) for i in (0, 2, 4))

FRAME = linear_hex('36565C')
DARK = linear_hex('173238')
CORROSION = linear_hex('4F7A6B')
SALT = linear_hex('C8D6D3')

def mix(a, b, t):
    return tuple(a[i] * (1-t) + b[i] * t for i in range(3))

def noise(x, y, seed):
    v = math.sin(x * 127.1 + y * 311.7 + seed * 19.19) * 43758.5453
    return v - math.floor(v)

def face_layout(obj):
    faces = []
    for poly in obj.data.polygons:
        # UV V is world/local +Z on vertical faces, so the authored streaks flow down.
        axis = max(range(3), key=lambda i: abs(poly.normal[i]))
        axes = (0, 1) if axis == 2 else ((1, 2) if axis == 0 else (0, 2))
        points = [obj.data.vertices[i].co for i in poly.vertices]
        lo = [min(p[a] for p in points) for a in axes]
        hi = [max(p[a] for p in points) for a in axes]
        faces.append({'poly': poly.index, 'axes': axes, 'lo': lo,
                      'span': [hi[j]-lo[j] for j in range(2)],
                      'vertical': axis != 2, 'material': poly.material_index})
    # Shelf pack the physical face extents; large visible faces get proportionate texel area.
    density = 700.0
    for attempt in range(30):
        for f in faces:
            f['w'] = max(4, math.ceil(f['span'][0] * density))
            f['h'] = max(4, math.ceil(f['span'][1] * density))
        x = y = row_height = 0
        fits = True
        for f in sorted(faces, key=lambda f: (-f['h'], -f['w'], f['poly'])):
            w, h = f['w'] + 2*PAD, f['h'] + 2*PAD
            if x + w > SIZE:
                y += row_height
                x, row_height = 0, 0
            if w > SIZE or y + h > SIZE:
                fits = False
                break
            f['x'], f['y'] = x+PAD, y+PAD
            x += w
            row_height = max(row_height, h)
        if fits:
            return faces, density
        density *= .92
    raise RuntimeError('Atlas packing failed')

def paint_face(u, v, f, seed):
    # All surfaces receive low-gloss mottled wear, with no untouched uniform paint field.
    grain = noise(math.floor(u*f['w']), math.floor(v*f['h']), seed)
    cloud = .5 + .5 * math.sin(u*18 + seed) * math.sin(v*13 + .7*seed)
    edge_px = min(u*f['w'], (1-u)*f['w'], v*f['h'], (1-v)*f['h'])
    wear = .18 + .23*cloud + .12*grain
    base = DARK if f['material'] == 1 else FRAME
    color = mix(base, DARK, wear if f['material'] != 1 else .05)
    color = tuple(c*(.88 + .12*grain) for c in color)
    if edge_px < 1.7 and grain > .34:
        color = mix(color, DARK, .45)  # chipped dark supporting layer, not bright bare metal
    corrosion = False
    if f['vertical']:
        # Local V decreases from each deposit head; tails narrow as they descend.
        for j in range(4):
            centre = .10 + .8*noise(j, 1, seed)
            head = .68 + .27*noise(j, 2, seed)
            length = .22 + .43*noise(j, 3, seed)
            t = (head-v)/length
            if 0 <= t <= 1:
                width = (.017 + .008*noise(j, 4, seed))*(1-.8*t)
                wandering = .003*math.sin(v*22 + j + seed)
                if abs(u-centre-wandering) < width and grain > .14:
                    corrosion = True
    else:
        # Broken verdigris deposits remain small; horizontal surfaces have no fake flow direction.
        patch = math.sin(u*28+seed) * math.sin(v*23+.3*seed)
        corrosion = patch > .87 and grain > .2
    if corrosion:
        color = mix(DARK, CORROSION, .62 + .30*grain)
    # Fine matte hexagonal grains clustered at physical component edges; no raised white blobs.
    cell = 5.0
    px, py = u*f['w'], v*f['h']
    cx, cy = math.floor(px/cell), math.floor(py/cell)
    dx, dy = abs(px-(cx+.5)*cell), abs(py-(cy+.5)*cell)
    radius = .65 + .35*noise(cx, cy, seed+21)
    hexagon = dx <= radius and .5*dx + .8660254*dy <= radius
    cluster = noise(math.floor(cx/3), math.floor(cy/3), seed+7) > .48
    salt = edge_px < 4.8 and hexagon and cluster and noise(cx, cy, seed+8) > .28
    if salt:
        color = mix(color, SALT, .62 + .22*grain)
    roughness = .86 + .10*grain if salt else (.76 + .15*grain if corrosion else .74 + .15*cloud)
    return color, roughness, corrosion and not salt, salt

texture_receipts = []
for object_index, obj in enumerate(mesh_objects):
    faces, density = face_layout(obj)
    rgba = array('f', [0.0]) * (SIZE*SIZE*4)
    rough = array('f', [0.82]) * (SIZE*SIZE*4)
    count = {'painted_texels': 0, 'corrosion_texels': 0, 'salt_texels': 0}
    uv = obj.data.uv_layers.new(name='UV_Drawer_Surface')
    for f in faces:
        for loop_id in obj.data.polygons[f['poly']].loop_indices:
            p = obj.data.vertices[obj.data.loops[loop_id].vertex_index].co
            local_uv = [(p[a]-f['lo'][j])/f['span'][j] for j, a in enumerate(f['axes'])]
            uv.data[loop_id].uv = ((f['x']+.5 + local_uv[0]*(f['w']-1))/SIZE,
                                  (f['y']+.5 + local_uv[1]*(f['h']-1))/SIZE)
        for yy in range(-PAD, f['h']+PAD):
            for xx in range(-PAD, f['w']+PAD):
                # Clamp padding to edge pixels to prevent atlas seams under filtering/mips.
                u = max(0, min(f['w']-1, xx))/max(1, f['w']-1)
                v = max(0, min(f['h']-1, yy))/max(1, f['h']-1)
                color, r, corrosion, salt = paint_face(u, v, f, 101*object_index+f['poly'])
                offset = ((f['y']+yy)*SIZE + f['x']+xx)*4
                rgba[offset:offset+4] = array('f', (*color, 1.0))
                rough[offset:offset+4] = array('f', (r, r, r, 1.0))
                if 0 <= xx < f['w'] and 0 <= yy < f['h']:
                    count['painted_texels'] += 1
                    count['corrosion_texels'] += int(corrosion)
                    count['salt_texels'] += int(salt)
    images = []
    for suffix, pixels, space in [('BaseColor', rgba, 'sRGB'), ('Roughness', rough, 'Non-Color')]:
        name = obj.name + '_' + suffix
        img = bpy.data.images.new(name, width=SIZE, height=SIZE, alpha=True)
        img.colorspace_settings.name = space
        img.pixels.foreach_set(pixels)
        img.filepath_raw = str(texture_dir / (name + '.png'))
        img.file_format = 'PNG'
        img.save()
        images.append(img)
    mat = bpy.data.materials.new('MAT_' + obj.name + '_Worn_r02')
    mat.use_nodes = True
    mat.diffuse_color = (*FRAME, 1)
    bsdf = mat.node_tree.nodes.get('Principled BSDF')
    bsdf.inputs['Metallic'].default_value = .08
    for img, socket in zip(images, ['Base Color', 'Roughness']):
        node = mat.node_tree.nodes.new('ShaderNodeTexImage')
        node.image = img
        node.interpolation = 'Linear'
        mat.node_tree.links.new(node.outputs['Color'], bsdf.inputs[socket])
    obj.data.materials.clear()
    obj.data.materials.append(mat)
    for poly in obj.data.polygons:
        poly.material_index = 0
    count.update({'object': obj.name, 'atlas_size': [SIZE, SIZE], 'pixels_per_metre': density,
                  'corrosion_mask_fraction': count['corrosion_texels']/count['painted_texels'],
                  'salt_mask_fraction': count['salt_texels']/count['painted_texels'],
                  'scope': 'Authored UV mask counts; NOT frame coverage or a visual pass'})
    texture_receipts.append(count)

bpy.context.view_layer.update()
measurements = {'asset_id': asset, 'revision': 'r02', 'status': 'blender-measured',
    'runtimeEligible': False, 'blender_version': bpy.app.version_string, 'initial_scene': initial,
    'tri_budget_target': 1500, 'texture_files': 4, 'objects': [],
    'material_target': {'frame_corrosion_max': .15, 'frame_salt_max': .12,
                        'frame_unworn_metal_max': .05, 'measured_frame_coverage': None},
    'texture_mask_receipts': texture_receipts,
    'runtime_texture_memory_bytes': None,
    'texture_memory_rgba8_no_mips_upper_bound_bytes': 4*SIZE*SIZE*4}
for obj in mesh_objects:
    obj.data.calc_loop_triangles()
    measurements['objects'].append({'name': obj.name, 'vertices': len(obj.data.vertices),
        'triangles': len(obj.data.loop_triangles), 'polygons': len(obj.data.polygons),
        'material_slots': len(obj.material_slots), 'location': list(obj.location),
        'rotation': list(obj.rotation_euler), 'scale': list(obj.scale),
        'bounds_local': [list(Vector(c)) for c in obj.bound_box],
        'uv_layers': [layer.name for layer in obj.data.uv_layers]})
measurements['triangles'] = sum(o['triangles'] for o in measurements['objects'])
assert len(mesh_objects) == 2 and measurements['triangles'] <= 1500
measurements['texture_disk_bytes'] = sum(p.stat().st_size for p in texture_dir.glob('*.png'))
(out / 'measurements.json').write_text(json.dumps(measurements, indent=2) + '\n')

# Export only the unchanged drawer hierarchy. GLB embeds textures; FBX points to the sidecar folder.
bpy.ops.object.select_all(action='DESELECT')
for obj in (root, tray, casing, moving):
    obj.select_set(True)
bpy.context.view_layer.objects.active = casing
bpy.ops.export_scene.gltf(filepath=str(out / (asset + '.glb')), export_format='GLB',
    use_selection=True, export_apply=True, export_yup=True, export_animations=False)
bpy.ops.export_scene.fbx(filepath=str(out / (asset + '.fbx')), use_selection=True,
    object_types={'MESH', 'EMPTY'}, axis_up='Y', axis_forward='-Z', global_scale=1.0,
    apply_unit_scale=True, apply_scale_options='FBX_SCALE_NONE', mesh_smooth_type='FACE',
    path_mode='RELATIVE', embed_textures=False)

# Isolated inspectable preview: no unrelated mesh, emission, bloom, screen flash or compositor FX.
scene.render.engine = 'CYCLES'
scene.cycles.samples = 32
scene.cycles.use_denoising = True
scene.render.resolution_x = 960
scene.render.resolution_y = 640
scene.render.resolution_percentage = 100
scene.render.image_settings.file_format = 'PNG'
scene.render.film_transparent = False
scene.world.use_nodes = True
scene.world.node_tree.nodes['Background'].inputs[0].default_value = (.028,.04,.042,1)
scene.world.node_tree.nodes['Background'].inputs[1].default_value = .7
scene.view_settings.view_transform = 'Standard'
scene.view_settings.exposure = 0
scene.view_settings.gamma = 1
camera_data = bpy.data.cameras.new('CAM_Drawer_r02_Preview')
camera = bpy.data.objects.new(camera_data.name, camera_data)
collection.objects.link(camera)
camera.location = (1.3,-1.8,1.13)
camera.rotation_euler = (Vector((0,0,.12))-camera.location).to_track_quat('-Z','Y').to_euler()
camera_data.type = 'ORTHO'
camera_data.ortho_scale = 1.58
scene.camera = camera
for name, pos, power, size in [('Key',(-1.7,-2.5,3),180,3), ('Fill',(2,1,2),90,2.5)]:
    data = bpy.data.lights.new('LIGHT_Drawer_r02_'+name, 'AREA')
    data.energy, data.shape, data.size = power, 'DISK', size
    obj = bpy.data.objects.new(data.name, data)
    collection.objects.link(obj)
    obj.location = pos
    obj.rotation_euler = (Vector((0,0,.12))-obj.location).to_track_quat('-Z','Y').to_euler()
scene.render.filepath = str(out / 'preview.png')
# Store relative image paths in the blend after exporting to make the review bundle portable.
for image in bpy.data.images:
    if image.filepath_raw and Path(image.filepath_raw).parent == texture_dir:
        image.filepath = '//textures/' + Path(image.filepath_raw).name
bpy.ops.wm.save_as_mainfile(filepath=str(out / (asset + '.blend')))
bpy.ops.render.render(write_still=True)
provenance = {'assetId': asset, 'revision': 'r02', 'provider': 'Blender CLI',
    'version': bpy.app.version_string, 'recipe': str(Path(__file__).resolve()),
    'recipe_sha256': hashlib.sha256(Path(__file__).read_bytes()).hexdigest(),
    'recipeSource': '_workspace/current/modeling/pipeline.md section 16.4',
    'decision': 'RFC-CX-003; director r02 texture/material correction instruction',
    'concept_ref': ['_workspace/current/concept/t0-source-drawer-review.md',
                    '_workspace/current/concept/style-guide.md'],
    'author': 'game-modeler recipe; director executes and audits',
    'license': 'Original procedural geometry and textures; no third-party source media',
    'runtimeEligible': False, 'promoted_by': None,
    'classification': 'Generated material-correction candidate; visual/import audit pending',
    'canonical_export': asset + '.glb', 'derived_import_candidate': asset + '.fbx',
    'hashes': {str(p.relative_to(out)): hashlib.sha256(p.read_bytes()).hexdigest()
               for p in sorted(out.rglob('*')) if p.is_file() and p.name != 'provenance.json'}}
(out / 'provenance.json').write_text(json.dumps(provenance, indent=2) + '\n')
print(json.dumps(measurements))
