# Execute with Blender --background --factory-startup --python <new-script.py>.
from pathlib import Path
import bpy, json
from mathutils import Vector

out = Path('/Users/jangyoung/orca/unknown/assets/generated/3d/hub-view-drawer-r03')
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

# r03 authored surface correction. No new geometry; diffuse/roughness atlases survive GLB export.
import math, hashlib, random
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

def smooth_noise(x, y, seed):
    ix, iy = math.floor(x), math.floor(y)
    fx, fy = x-ix, y-iy
    fx, fy = fx*fx*(3-2*fx), fy*fy*(3-2*fy)
    a = noise(ix, iy, seed)*(1-fx) + noise(ix+1, iy, seed)*fx
    b = noise(ix, iy+1, seed)*(1-fx) + noise(ix+1, iy+1, seed)*fx
    return a*(1-fy) + b*fy

def prepare_patterns(f, seed):
    # Seeded positions/sizes, with overlapping lobes, absent regions and broken subclusters.
    rng = random.Random(seed+4001)
    f['patches'], f['drips'], f['salt_bins'] = [], [], {}
    if not f['vertical']:
        for _ in range(rng.randint(2, 4)):
            centre_u, centre_v = rng.uniform(.05,.95), rng.uniform(.06,.94)
            for j in range(rng.randint(2,4)):
                f['patches'].append((centre_u+rng.gauss(0,.035), centre_v+rng.gauss(0,.034),
                                     rng.uniform(.025,.085), rng.uniform(.018,.066)))
    else:
        for _ in range(rng.randint(2,5)):
            u, head = rng.uniform(.04,.96), rng.uniform(.50,.99)
            length, width = rng.uniform(.13,.83), rng.uniform(.006,.024)
            offsets = [rng.uniform(-.010,.010) for _ in range(5)]
            f['drips'].append((u,head,length,width,offsets))
            if rng.random() < .45:
                # Occasional joined shorter run from the same deposit, not another stamped mark.
                f['drips'].append((u+rng.uniform(-.017,.017), head-rng.uniform(.02,.17),
                                   length*rng.uniform(.28,.61), width*rng.uniform(.35,.72), offsets))
    # Salt grains are seeded points, not a cell lattice. Only selected discontinuous edge clusters.
    selected_edges = rng.sample(range(4), rng.randint(1,3))
    for edge in selected_edges:
        along_extent = f['w'] if edge < 2 else f['h']
        for _ in range(rng.randint(1,2)):
            centre = rng.uniform(.06,.94)*along_extent
            spread = rng.uniform(3, max(4,min(22,along_extent*.065)))
            for _ in range(rng.randint(8,29)):
                if rng.random() < .22:
                    continue
                along = centre + rng.gauss(0,spread)
                inset = abs(rng.gauss(1.5,1.15))
                if edge == 0: px,py = along,inset
                elif edge == 1: px,py = along,f['h']-1-inset
                elif edge == 2: px,py = inset,along
                else: px,py = f['w']-1-inset,along
                if 0 <= px < f['w'] and 0 <= py < f['h']:
                    point=(px,py,rng.uniform(.48,1.12),rng.uniform(.40,.73))
                    key=(math.floor(px/4),math.floor(py/4))
                    f['salt_bins'].setdefault(key,[]).append(point)

def paint_face(u, v, f, seed):
    px, py = u*(f['w']-1), v*(f['h']-1)
    grain = noise(math.floor(px), math.floor(py), seed)
    cloud = smooth_noise(u*5.3, v*4.7, seed+31)
    fine = smooth_noise(u*21.9, v*19.1, seed+61)
    edge_px = min(px, f['w']-1-px, py, f['h']-1-py)
    # Recover structural #36565C versus recessed #173238 through correct encoding and restrained wear.
    # No clean untouched field: even the lightest metal has an irregular worn-coating treatment.
    base = DARK if f['material'] == 1 else FRAME
    wear = .05 + .21*cloud + .035*grain
    color = mix(base, DARK, wear if f['material'] != 1 else .035)
    color = tuple(c*(.95+.05*fine) for c in color)
    if edge_px < 1.65 and grain > .43 and fine > .40:
        color = mix(color, DARK, .42)
    corrosion = False
    if f['vertical']:
        for centre,head,length,width,offsets in f['drips']:
            t=(head-v)/length
            if 0 <= t <= 1:
                k=min(3,math.floor(t*4))
                q=t*4-k
                wander=offsets[k]*(1-q)+offsets[k+1]*q
                broken_width=width*(1-.86*t)*(.48+.75*fine)
                if abs(u-centre-wander) < broken_width and grain > .10:
                    corrosion = True
    else:
        for centre_u,centre_v,rx,ry in f['patches']:
            distance=((u-centre_u)/rx)**2 + ((v-centre_v)/ry)**2
            if distance < .70+.62*fine and grain > .07:
                corrosion = True
                break
    if corrosion:
        color = mix(DARK, CORROSION, .67+.26*fine)
    salt = False
    if edge_px < 7:
        bucket_x,bucket_y=math.floor(px/4),math.floor(py/4)
        for bx in range(bucket_x-1,bucket_x+2):
            for by in range(bucket_y-1,bucket_y+2):
                for sx,sy,radius,strength in f['salt_bins'].get((bx,by),[]):
                    dx,dy=abs(px-sx),abs(py-sy)
                    if dx <= radius and .5*dx+.8660254*dy <= radius:
                        color=mix(color,SALT,strength)
                        salt=True
                        break
    roughness = .89+.07*grain if salt else (.78+.13*grain if corrosion else .76+.12*cloud)
    return color, roughness, corrosion and not salt, salt

def srgb_byte(value):
    value=max(0.0,min(1.0,value))
    encoded=12.92*value if value <= .0031308 else 1.055*(value**(1/2.4))-.055
    return round(encoded*255)

# Explicit PNG encoding avoids r02's generated byte-buffer quantization of linear values as sRGB.
assert tuple(srgb_byte(c) for c in FRAME) == (54,86,92)
assert tuple(srgb_byte(c) for c in DARK) == (23,50,56)

def write_texture_png(path, pixels, color_map):
    import struct, zlib
    def chunk(tag, data):
        return struct.pack('>I',len(data))+tag+data+struct.pack('>I',zlib.crc32(tag+data)&0xffffffff)
    channels=4 if color_map else 1
    scanlines=bytearray()
    # Blender UV/image buffers start at the bottom; PNG scanlines start at the top.
    for y in reversed(range(SIZE)):
        scanlines.append(0)
        for x in range(SIZE):
            i=(y*SIZE+x)*4
            if color_map:
                scanlines.extend(srgb_byte(pixels[i+j]) for j in range(3))
                scanlines.append(round(max(0,min(1,pixels[i+3]))*255))
            else:
                scanlines.append(round(max(0,min(1,pixels[i]))*255))
    payload=b'\x89PNG\r\n\x1a\n'
    payload+=chunk(b'IHDR',struct.pack('>IIBBBBB',SIZE,SIZE,8,6 if color_map else 0,0,0,0))
    if color_map:
        payload+=chunk(b'sRGB',b'\x00')
    payload+=chunk(b'IDAT',zlib.compress(bytes(scanlines),6))+chunk(b'IEND',b'')
    path.write_bytes(payload)

texture_receipts = []
for object_index, obj in enumerate(mesh_objects):
    faces, density = face_layout(obj)
    rgba = array('f', [0.0]) * (SIZE*SIZE*4)
    rough = array('f', [0.82]) * (SIZE*SIZE*4)
    count = {'painted_texels': 0, 'corrosion_texels': 0, 'salt_texels': 0}
    uv = obj.data.uv_layers.new(name='UV_Drawer_Surface')
    for f in faces:
        prepare_patterns(f, 101*object_index+f['poly'])
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
        texture_path = texture_dir / (name + '.png')
        write_texture_png(texture_path, pixels, suffix == 'BaseColor')
        img = bpy.data.images.load(str(texture_path), check_existing=False)
        img.colorspace_settings.name = space
        images.append(img)
    mat = bpy.data.materials.new('MAT_' + obj.name + '_Worn_r03')
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
measurements = {'asset_id': asset, 'revision': 'r03', 'status': 'blender-measured',
    'runtimeEligible': False, 'blender_version': bpy.app.version_string, 'initial_scene': initial,
    'tri_budget_target': 1500, 'texture_files': 4, 'objects': [],
    'material_target': {'frame_corrosion_max': .15, 'frame_salt_max': .12,
                        'frame_unworn_metal_max': .05, 'measured_frame_coverage': None},
    'texture_mask_receipts': texture_receipts,
    'texture_encoding': {'baseColor': 'explicit linear-to-sRGB PNG; sRGB import',
                         'roughness': 'linear grayscale PNG; Non-Color import'},
    'preview_lighting': 'unchanged from r02; texture encoding correction isolated',
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
camera_data = bpy.data.cameras.new('CAM_Drawer_r03_Preview')
camera = bpy.data.objects.new(camera_data.name, camera_data)
collection.objects.link(camera)
camera.location = (1.3,-1.8,1.13)
camera.rotation_euler = (Vector((0,0,.12))-camera.location).to_track_quat('-Z','Y').to_euler()
camera_data.type = 'ORTHO'
camera_data.ortho_scale = 1.58
scene.camera = camera
for name, pos, power, size in [('Key',(-1.7,-2.5,3),180,3), ('Fill',(2,1,2),90,2.5)]:
    data = bpy.data.lights.new('LIGHT_Drawer_r03_'+name, 'AREA')
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
provenance = {'assetId': asset, 'revision': 'r03', 'provider': 'Blender CLI',
    'version': bpy.app.version_string, 'recipe': str(Path(__file__).resolve()),
    'recipe_sha256': hashlib.sha256(Path(__file__).read_bytes()).hexdigest(),
    'recipeSource': '_workspace/current/modeling/pipeline.md section 16.5',
    'decision': 'RFC-CX-003; director r03 texture/material correction instruction',
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
