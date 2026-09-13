"""RFC-CX-018: original Han Seorin, authored through the live Blender MCP.

Call run_stage('setup'|'model'|'rig'|'animate'|'export'|'render'). No factory
reset, global save, downloads, or external textures. Persistent production state
is stored on M22_Seorin_Production; the caller's scene/context is restored.
The render stage advances one of seven previews per call (persistent cursor),
keeping each live MCP response bounded; call render seven times for the set.
"""
import bpy
import math
import json
import hashlib
import shutil
import struct
from pathlib import Path
from datetime import datetime, timezone
from mathutils import Vector, Matrix

ROOT = Path('/Users/jangyoung/orca/unknown')
OUT = ROOT / 'assets/generated/3d/seorin-m22/r01'
SCENE = 'M22_Seorin_Production'
PREFIX = 'M22_'
PALETTE = {
    'Skin': ('#ae8971', .7, 0), 'SkinShade': ('#896956', .76, 0),
    'Nail': ('#c5a28a', .52, 0), 'Shirt': ('#465a67', .88, 0),
    'Cuff': ('#667781', .93, 0), 'Seam': ('#81908d', .88, 0),
    'Apron': ('#283936', .94, 0), 'ApronEdge': ('#52605a', .92, 0),
    'Trouser': ('#4c514b', .96, 0), 'Boot': ('#282b2a', .85, 0),
    'Sole': ('#171e1d', .92, 0), 'Brass': ('#948361', .52, .62),
    'Paper': ('#afa28a', .92, 0), 'Leather': ('#695743', .89, 0),
    'Hair': ('#192423', .73, 0), 'HairLight': ('#35443f', .81, 0),
    'EyeWhite': ('#b1aea0', .56, 0), 'Iris': ('#343731', .48, 0),
    'Lip': ('#89665c', .83, 0),
}
CLIPS = {'Seorin_Idle': (120, None), 'Hands_Rest': (60, None),
         'Hands_Insert': (15, 12), 'Hands_Align': (12, 6),
         'Hands_Grip': (24, 18), 'Hands_Seal': (18, 9)}


def sha(path):
    return hashlib.sha256(Path(path).read_bytes()).hexdigest()


def archive(reason):
    """Snapshot our complete candidate before replacing any prior output."""
    existing = [p for p in OUT.iterdir() if p.name != 'history'] if OUT.exists() else []
    if not existing:
        return None
    history = OUT / 'history'
    history.mkdir(exist_ok=True)
    dest = history / ('candidate-%03d' % (len(list(history.iterdir())) + 1))
    dest.mkdir()
    receipt = {'reason': reason, 'files': {}}
    for p in existing:
        target = dest / p.name
        if p.is_dir():
            shutil.copytree(p, target)
        else:
            shutil.copy2(p, target)
        for f in ([target] if target.is_file() else target.rglob('*')):
            if f.is_file():
                receipt['files'][str(f.relative_to(dest))] = sha(f)
    recipe = dest / 'build-seorin-m22.py'
    shutil.copy2(ROOT / 'scripts/blender/build-seorin-m22.py', recipe)
    receipt['recipeSha256'] = sha(recipe)
    (dest / 'preservation-receipt.json').write_text(json.dumps(receipt, indent=2))
    return str(dest.relative_to(ROOT))


def baseline():
    return {'filepath': bpy.data.filepath, 'scenes': {
        s.name: {'objects': {o.name: {'type': o.type,
            'matrix': [list(row) for row in o.matrix_world],
            'hidden': o.hide_viewport, 'renderHidden': o.hide_render}
            for o in s.objects}, 'frame': s.frame_current,
            'camera': s.camera.name if s.camera else None}
        for s in bpy.data.scenes if not s.name.startswith(PREFIX)}}


def scene():
    return bpy.data.scenes[SCENE]
def original_structure():
    result = baseline()
    for name,entry in result['scenes'].items():
        original = bpy.data.scenes[name]
        entry['world'] = {'name':original.world.name if original.world else None,
                          'color':list(original.world.color) if original.world else None}
        if original.world and original.world.use_nodes:
            entry['world']['nodes'] = [
                {'name':n.name,'type':n.bl_idname,'inputs':{
                    p.name:(list(p.default_value) if hasattr(p.default_value,'__len__') and not isinstance(p.default_value,str)
                            else p.default_value)
                    for p in n.inputs if hasattr(p,'default_value') and
                    isinstance(p.default_value,(float,int,str,bpy.types.bpy_prop_array))}}
                for n in original.world.node_tree.nodes]
        for obj in original.objects:
            d=entry['objects'][obj.name]
            d['dataName']=obj.data.name if obj.data else None
            d['materials']=[m.name if m else None for m in getattr(obj.data,'materials',[])]
            if obj.type=='MESH':
                geometry={'vertices':[list(v.co) for v in obj.data.vertices],
                          'polygons':[list(p.vertices) for p in obj.data.polygons]}
                d['geometrySha256']=hashlib.sha256(json.dumps(geometry,sort_keys=True).encode()).hexdigest()
            elif obj.type=='LIGHT':
                d.update(energy=obj.data.energy,color=list(obj.data.color),lightType=obj.data.type)
            elif obj.type=='CAMERA':
                d.update(lens=obj.data.lens,sensorWidth=obj.data.sensor_width,clipStart=obj.data.clip_start,clipEnd=obj.data.clip_end)
    return result




def material(key):
    name = PREFIX + 'MAT_' + key
    m = bpy.data.materials.get(name)
    if m:
        return m
    rgb, rough, metal = PALETTE[key]
    def linear(v):
        return v / 12.92 if v <= .04045 else ((v + .055) / 1.055) ** 2.4
    color = tuple(linear(int(rgb[i:i+2], 16) / 255) for i in (1, 3, 5)) + (1,)
    m = bpy.data.materials.new(name)
    m.diffuse_color = color
    m.use_nodes = True
    p = m.node_tree.nodes.get('Principled BSDF')
    p.inputs['Base Color'].default_value = color
    p.inputs['Roughness'].default_value = rough
    p.inputs['Metallic'].default_value = metal
    return m


class Surface:
    """Small explicit mesh authoring helper; skin weights travel with vertices."""
    def __init__(self):
        self.vertices, self.faces, self.weights, self.materials = [], [], [], []
        self.keys = list(PALETTE)

    def vertex(self, p, weights):
        self.vertices.append(tuple(p))
        self.weights.append(dict(weights))
        return len(self.vertices) - 1

    def face(self, indices, mat):
        self.faces.append(indices)
        self.materials.append(self.keys.index(mat))

    def tube(self, points, radii, mat, weights, sides=12, axis=None):
        start = len(self.vertices)
        for k, p in enumerate(points):
            tangent = Vector(points[min(k+1, len(points)-1)]) - Vector(points[max(k-1, 0)])
            tangent.normalize()
            u = Vector(axis or (1, 0, 0))
            if abs(tangent.dot(u)) > .95:
                u = Vector((0, 1, 0))
            u = (u - tangent * u.dot(tangent)).normalized()
            v = tangent.cross(u).normalized()
            rx, ry = radii[k] if isinstance(radii[k], (list, tuple)) else (radii[k], radii[k])
            w = weights[k] if isinstance(weights, list) else weights
            for j in range(sides):
                a = 2 * math.pi * j / sides
                self.vertex(Vector(p) + u * (math.cos(a) * rx) + v * (math.sin(a) * ry), w)
        for k in range(len(points)-1):
            for j in range(sides):
                a = start + k*sides+j
                b = start + k*sides+(j+1)%sides
                self.face((a, b, b+sides, a+sides), mat)
        self.face(tuple(start+j for j in reversed(range(sides))), mat)
        self.face(tuple(start+(len(points)-1)*sides+j for j in range(sides)), mat)

    def ellipsoid(self, center, scale, mat, weights, sides=16, rings=10):
        pts, rs = [], []
        for k in range(rings+1):
            a = -math.pi/2 + math.pi*k/rings
            pts.append((center[0], center[1], center[2]+scale[2]*math.sin(a)))
            rs.append((max(.0001, scale[0]*math.cos(a)), max(.0001, scale[1]*math.cos(a))))
        self.tube(pts, rs, mat, weights, sides)

    def box(self, center, size, mat, weights, bevel=.003):
        x, y, z = center
        sx, sy, sz = (v/2 for v in size)
        b = min(bevel, sx*.4, sy*.4, sz*.4)
        ring = [(-sx+b,-sy), (sx-b,-sy), (sx,-sy+b), (sx,sy-b),
                (sx-b,sy), (-sx+b,sy), (-sx,sy-b), (-sx,-sy+b)]
        start = len(self.vertices)
        for zz, inset in [(-sz,b),(-sz+b,0),(sz-b,0),(sz,b)]:
            for xx, yy in ring:
                self.vertex((x+xx*(1-inset/max(sx,.001)), y+yy*(1-inset/max(sy,.001)), z+zz), weights)
        for k in range(3):
            for j in range(8):
                self.face((start+k*8+j,start+k*8+(j+1)%8,start+(k+1)*8+(j+1)%8,start+(k+1)*8+j),mat)
        self.face(tuple(start+j for j in reversed(range(8))),mat)
        self.face(tuple(start+24+j for j in range(8)),mat)

    def ribbon(self, points, width, mat, weights):
        # A narrow garment ribbon with a real front/back thickness.
        for a, b in zip(points, points[1:]):
            self.tube([a,b],[(width/2,.002)]*2,mat,weights,8)

    def object(self, name, collection):
        mesh = bpy.data.meshes.new(PREFIX+'MSH_'+name)
        mesh.from_pydata(self.vertices, [], self.faces)
        mesh.update()
        obj = bpy.data.objects.new(PREFIX+name, mesh)
        collection.objects.link(obj)
        for key in self.keys:
            mesh.materials.append(material(key))
        for polygon, index in zip(mesh.polygons,self.materials):
            polygon.material_index = index
            polygon.use_smooth = True
        groups = {}
        for index, weights in enumerate(self.weights):
            for name, weight in weights.items():
                if name not in groups:
                    groups[name] = obj.vertex_groups.new(name=name)
                groups[name].add([index],weight,'REPLACE')
        return obj


def collection(name):
    existing = bpy.data.collections.get(PREFIX+name)
    if existing:
        return existing
    c = bpy.data.collections.new(PREFIX+name)
    scene().collection.children.link(c)
    return c


def hand(surface, side, transform=None, bone_prefix=''):
    """Dorsal +Z, fingers -Y, wrist origin. Five separately articulated digits."""
    mirror = -1 if side == 'Right' else 1
    start = len(surface.vertices)
    B = lambda n: bone_prefix+n
    bones = [(B('Forearm'),(0,.255,0),(0,0,0),None),
             (B('Wrist'),(0,0,0),(0,-.085,0),B('Forearm'))]
    w = {B('Forearm'):1}
    surface.tube([(0,.26,0),(0,.21,0),(0,.14,0),(0,.06,0),(0,.015,0),(0,-.01,0)],
        [(.048,.04),(.046,.037),(.039,.031),(.029,.024),(.025,.021),(.026,.02)],
        'Skin',[w,w,w,w,{B('Forearm'):.5,B('Wrist'):.5},{B('Wrist'):1}],16)
    surface.tube([(0,.263,0),(0,.25,0),(0,.239,0),(0,.226,0)],
        [(.053,.045),(.055,.046),(.055,.045),(.05,.042)],'Cuff',w,16)
    surface.tube([(0,.255,.001),(0,.249,.001)],[(.056,.047)]*2,'Seam',w,16)
    surface.tube([(0,.274,0),(0,.261,0)],[(.051,.043),(.052,.044)],'Shirt',w,16)
    surface.tube([(0,.006,0),(0,-.02,.001),(0,-.05,.002),(0,-.079,.001),(0,-.087,0)],
        [(.026,.02),(.035,.024),(.041,.022),(.039,.018),(.033,.013)],'Skin',{B('Wrist'):1},20)
    # Thenar eminence and thumb's oblique metacarpal distinguish a human hand.
    surface.ellipsoid((mirror*.028,-.023,-.008),(.02,.031,.018),'Skin',{B('Wrist'):1},12,8)
    digits = [('Index',-.027,.083),('Middle',-.008,.094),('Ring',.013,.087),('Little',.031,.068)]
    for index,(name,x,length) in enumerate(digits):
        x = x * (-mirror)
        y0 = -.078 + (.007 if name=='Little' else 0)
        drift = x*.11
        joints = [(x,y0,0),(x+drift*.4,y0-length*.45,-.002),
                  (x+drift*.75,y0-length*.76,-.004),(x+drift,y0-length,-.006)]
        names = [B(name+'%02d'%j) for j in (1,2,3)]
        for j in range(3):
            bones.append((names[j],joints[j],joints[j+1],B('Wrist') if j==0 else names[j-1]))
        points, radii, weights = [], [], []
        r = [.010,.0105,.010,.008][index]
        for j in range(3):
            a,b = Vector(joints[j]),Vector(joints[j+1])
            for t in (0,.22,.78):
                points.append(tuple(a.lerp(b,t)))
                radii.append((r*(1-.14*j)*(1+.05*math.sin(t*math.pi)),r*.83*(1-.12*j)))
                if t==0 and j>0:
                    weights.append({names[j-1]:.5,names[j]:.5})
                elif t==0 and j==0:
                    weights.append({B('Wrist'):.25,names[j]:.75})
                else:
                    weights.append({names[j]:1})
        points += [joints[-1],tuple(Vector(joints[-1])+Vector((0,-.002,0)))]
        radii += [(r*.58,r*.48),(.0015,.0015)]
        weights += [{names[-1]:1}]*2
        surface.tube(points,radii,'Skin',weights,10,axis=(1,0,0))
        surface.ellipsoid((joints[2][0],joints[2][1]-.008,.004),
                          (r*.66,length*.09,.002),'Nail',{names[2]:1},10,5)
    thumb = [(mirror*.026,-.017,-.004),(mirror*.05,-.043,-.007),
             (mirror*.063,-.063,-.009),(mirror*.066,-.087,-.012)]
    for j in range(3):
        n=B('Thumb%02d'%(j+1))
        bones.append((n,thumb[j],thumb[j+1],B('Wrist') if j==0 else B('Thumb%02d'%j)))
    p,r,w=[],[],[]
    for j in range(3):
        a,b=Vector(thumb[j]),Vector(thumb[j+1]); n=B('Thumb%02d'%(j+1))
        for t in (0,.25,.8):
            p.append(tuple(a.lerp(b,t))); r.append(.014*(1-j*.2))
            prev = B('Wrist') if j==0 else B('Thumb%02d'%j)
            w.append({prev:.4,n:.6} if t==0 else {n:1})
    p += [thumb[-1]]; r += [.004]; w += [{B('Thumb03'):1}]
    surface.tube(p,r,'Skin',w,12,axis=(0,0,1))
    surface.ellipsoid((thumb[-1][0],thumb[-1][1]+.007,thumb[-1][2]+.007),(.007,.009,.002),
                       'Nail',{B('Thumb03'):1},10,5)
    bones.append((B('Contact'),(0,-.1,-.04),(0,-.125,-.04),B('Wrist')))
    if transform:
        for i in range(start,len(surface.vertices)):
            surface.vertices[i]=tuple(transform @ Vector(surface.vertices[i]))
        bones=[(n,tuple(transform@Vector(a)),tuple(transform@Vector(b)),p) for n,a,b,p in bones]
    return bones


def build_character(c):
    s=Surface()
    bones=[('Root',(0,0,0),(0,0,.15),None),('Hips',(0,0,.86),(0,0,1.04),'Root'),
        ('Spine',(0,0,1.04),(0,0,1.23),'Hips'),('Chest',(0,0,1.23),(0,0,1.40),'Spine'),
        ('Neck',(0,0,1.40),(0,-.009,1.49),'Chest'),('Head',(0,-.009,1.49),(0,-.017,1.69),'Neck')]
    s.tube([(0,0,.92),(0,0,1.02),(0,.008,1.13),(0,.007,1.24),(0,.005,1.34),(0,0,1.40)],
           [(.15,.105),(.14,.10),(.135,.096),(.16,.103),(.179,.092),(.145,.073)],
           'Shirt',[{'Hips':1},{'Hips':.5,'Spine':.5},{'Spine':1},{'Spine':.4,'Chest':.6},{'Chest':1},{'Chest':1}],24)
    s.tube([(0,0,1.38),(0,-.002,1.44),(0,-.012,1.50)],[(.049,.044),(.044,.04),(.043,.043)],'Skin',{'Neck':1},16)
    # Head contour has explicit chin, jaw, cheek, temple and crown stations.
    s.tube([(0,-.028,1.473),(0,-.024,1.486),(0,-.016,1.52),(0,-.01,1.558),
            (0,-.006,1.59),(0,0,1.63),(0,.006,1.671),(0,.009,1.694)],
           [(.023,.031),(.041,.047),(.059,.06),(.071,.066),(.073,.068),(.071,.064),(.051,.048),(.012,.015)],
           'Skin',{'Head':1},28)
    for sign in (-1,1):
        s.ellipsoid((sign*.070,.002,1.561),(.013,.015,.025),'Skin',{'Head':1},12,8)
        s.ellipsoid((sign*.075,-.010,1.561),(.005,.004,.014),'SkinShade',{'Head':1},10,6)
        # Narrow tired eyes with sculpted upper/lower lids, not ball eyes.
        x=sign*.031
        s.ellipsoid((x,-.069,1.582),(.020,.006,.007),'EyeWhite',{'Head':1},14,6)
        s.ellipsoid((x,-.074,1.582),(.0065,.002,.006),'Iris',{'Head':1},12,6)
        s.tube([(x-.019,-.072,1.582),(x-.009,-.075,1.588),(x+.009,-.075,1.588),(x+.019,-.071,1.583)],
               [.0022]*4,'SkinShade',{'Head':1},6)
        s.tube([(x-.018,-.071,1.579),(x,-.074,1.575),(x+.017,-.071,1.579)],
               [.0016]*3,'SkinShade',{'Head':1},6)
        s.tube([(x-.017,-.07,1.602),(x,-.074,1.606),(x+.017,-.069,1.603)],
               [.0032,.004,.002],'Hair',{'Head':1},6)
    # Nose planes, nostril wings, understated closed lips.
    s.tube([(0,-.066,1.595),(0,-.080,1.566),(0,-.087,1.549),(0,-.077,1.545)],
           [(.006,.003),(.006,.005),(.008,.007),(.01,.002)],'Skin',{'Head':1},10)
    for x in (-.009,.009):
        s.ellipsoid((x,-.075,1.547),(.006,.006,.004),'SkinShade',{'Head':1},10,6)
    s.tube([(-.021,-.070,1.524),(-.008,-.076,1.526),(0,-.077,1.524),(.008,-.076,1.525),(.020,-.070,1.524)],
           [.001,.003,.0025,.003,.001],'Lip',{'Head':1},8)
    s.tube([(-.018,-.070,1.521),(0,-.076,1.519),(.018,-.070,1.521)],
           [.001,.003,.001],'Lip',{'Head':1},8)
    # Asymmetric bob cap: custom swept panels leave face open, with longer left fringe.
    s.ellipsoid((0,.009,1.66),(.075,.066,.049),'Hair',{'Head':1},24,10)
    for j in range(18):
        a=2*math.pi*j/18
        front=max(0,-math.sin(a))
        bottom=1.515 if front<.25 else (1.60 if math.cos(a)>0 else 1.55)
        pts=[(.006,.008,1.709),(.052*math.cos(a)+.005,.056*math.sin(a)+.008,1.689),
             (.076*math.cos(a)+.003,.073*math.sin(a)+.007,1.65),
             (.081*math.cos(a),.072*math.sin(a)+.011,1.60),
             (.070*math.cos(a)-.004,.065*math.sin(a)+.013,bottom)]
        if front>.55:
            pts=pts[:3]+[(pts[2][0]-.018,pts[2][1]-.009,bottom)]
        s.tube(pts,[(.007,.005),(.019,.008),(.019,.009),(.011,.006),(.002,.002)][:len(pts)],
               'Hair' if j%4 else 'HairLight',{'Head':1},8,axis=(math.cos(a),math.sin(a),0))
    # Deliberately swept fringe from right crown to the left temple.
    for j in range(5):
        s.tube([(.035+j*.005,-.041,1.684),(.012+j*.004,-.074,1.663),
                (-.03+j*.004,-.083,1.623),(-.062+j*.003,-.07,1.559-j*.004)],
               [(.013,.006),(.013,.006),(.009,.004),(.0015,.001)],'Hair',{'Head':1},8)
    for sign,label in ((-1,'Right'),(1,'Left')):
        hip=(sign*.086,0,.93); knee=(sign*.102,-.008,.52); ankle=(sign*.112,.008,.13)
        bones += [(label+'Thigh',hip,knee,'Hips'),(label+'Shin',knee,ankle,label+'Thigh'),
                  (label+'Foot',ankle,(sign*.112,-.15,.07),label+'Shin')]
        s.tube([(sign*.086,0,.95),(sign*.095,0,.86),(sign*.1,0,.68),(sign*.102,-.008,.53),
                (sign*.108,.002,.40),(sign*.112,.008,.22),(sign*.112,.008,.15)],
               [(.09,.10),(.095,.106),(.078,.089),(.07,.075),(.066,.078),(.065,.068),(.060,.059)],
               'Trouser',[{label+'Thigh':1}]*3+[{label+'Thigh':.5,label+'Shin':.5}]+[{label+'Shin':1}]*3,16)
        s.box((sign*.175,-.008,.72),(.031,.105,.135),'Trouser',{label+'Thigh':1},.012)
        s.box((sign*.178,-.014,.782),(.036,.109,.024),'ApronEdge',{label+'Thigh':1},.005)
        for z in (.185,.214,.493,.555):
            b=label+'Shin' if z<.51 else label+'Thigh'
            s.tube([(sign*.108,-.067,z),(sign*.143,-.063,z+.008),(sign*.158,-.032,z+.018)],
                   [.003,.004,.0015],'ApronEdge',{b:1},6)
        s.box((sign*.112,-.043,.055),(.151,.267,.059),'Sole',{label+'Foot':1},.015)
        s.ellipsoid((sign*.112,-.06,.095),(.071,.127,.060),'Boot',{label+'Foot':1},16,8)
        s.tube([(sign*.112,.009,.085),(sign*.112,.012,.17),(sign*.112,.009,.204)],
               [(.062,.071),(.06,.063),(.058,.058)],'Boot',{label+'Foot':1},14)
        for z in (.115,.14,.165,.188):
            s.tube([(sign*.112-.027,-.053,z),(sign*.112+.027,-.057,z+.009)],
                   [.002]*2,'Leather',{label+'Foot':1},6)
        for dx in (-.06,.06):
            for dy in (-.12,-.04,.04):
                s.box((sign*.112+dx,dy,.032),(.018,.038,.018),'Boot',{label+'Foot':1},.003)
        shoulder=(sign*.164,0,1.361); elbow=(sign*.232,-.008,1.14); wrist=(sign*.264,-.041,.924)
        bones += [(label+'UpperArm',shoulder,elbow,'Chest'),(label+'Forearm',elbow,wrist,label+'UpperArm')]
        s.ellipsoid((sign*.151,0,1.347),(.082,.065,.061),'Shirt',
                    {'Chest':.55,label+'UpperArm':.45},16,8)
        s.tube([shoulder,(sign*.19,.002,1.318),(sign*.211,-.003,1.247),(sign*.231,-.009,1.161),
                (sign*.238,-.015,1.123)],
               [(.063,.064),(.07,.067),(.058,.060),(.055,.055),(.052,.049)],
               'Shirt',{label+'UpperArm':1},16)
        s.tube([(sign*.23,-.009,1.172),(sign*.237,-.014,1.143),(sign*.24,-.017,1.126)],
               [(.061,.058),(.062,.057),(.054,.05)],'Cuff',{label+'UpperArm':1},16)
        s.tube([(sign*.234,-.012,1.148),(sign*.237,-.014,1.141)],
               [(.063,.058)]*2,'Seam',{label+'UpperArm':1},16)
        # Local forearm +Y follows wrist->elbow, local fingers continue downwards.
        y=(Vector(elbow)-Vector(wrist)).normalized(); x=Vector((1,0,0)); x=(x-y*x.dot(y)).normalized(); z=x.cross(y)
        matrix=Matrix(((x.x,y.x,z.x,wrist[0]),(x.y,y.y,z.y,wrist[1]),(x.z,y.z,z.z,wrist[2]),(0,0,0,1)))
        start=len(s.vertices)
        hb=hand(s,label,matrix,label)
        # Character hand's forearm bone is the already-authored anatomical chain.
        bones += [b for b in hb if b[0]!=label+'Forearm']
        # Cuff/forearm authored for standalone length .26: fit character elbow distance.
        # The upper cuff is hidden under the rolled shirt, preserving the common hand.
    # Narrow apron panels shaped around waist, with split hem and stitched edges.
    for sign in (-1,1):
        verts=[(sign*.012,-.113,.63),(sign*.145,-.098,.65),(sign*.14,-.113,.96),
               (sign*.111,-.119,1.27),(sign*.008,-.124,1.275)]
        ids=[s.vertex(v,{'Spine':1} if v[2]>1 else {'Hips':1}) for v in verts]
        s.face(ids if sign>0 else list(reversed(ids)),'Apron')
        back=[s.vertex((v[0],v[1]+.008,v[2]),{'Spine':1} if v[2]>1 else {'Hips':1}) for v in verts]
        s.face(list(reversed(back)) if sign>0 else back,'Apron')
        for i in range(5):
            j=(i+1)%5; s.face((ids[i],ids[j],back[j],back[i]),'ApronEdge')
        s.tube([verts[0],verts[1],verts[2],verts[3]], [.0017]*4,'ApronEdge',{'Hips':1},6)
        s.ribbon([(sign*.097,-.12,1.265),(sign*.117,-.095,1.359),(sign*.119,-.012,1.405),
                  (sign*.10,.081,1.345),(sign*.068,.105,1.10)],.025,'Apron',{'Chest':1})
        s.box((sign*.098,-.126,1.291),(.033,.009,.045),'Brass',{'Chest':1},.003)
        s.box((sign*.098,-.133,1.292),(.023,.005,.028),'Apron',{'Chest':1},.001)
    s.box((0,-.12,1.117),(.025,.008,.306),'Apron',{'Spine':1},.002)
    s.tube([(-.043,-.038,1.398),(0,-.056,1.390),(.043,-.038,1.398)],
           [.006,.007,.006],'Cuff',{'Chest':1},8)
    for sign in (-1,1):
        s.tube([(sign*.058,-.073,1.393),(sign*.126,-.065,1.377),(sign*.181,-.050,1.341)],
               [.0017]*3,'Seam',{'Chest':1},6)
    s.box((0,-.126,.99),(.29,.014,.033),'ApronEdge',{'Hips':1},.003)
    s.box((-.013,-.132,.902),(.147,.018,.128),'ApronEdge',{'Hips':1},.008)
    s.box((-.013,-.143,.909),(.135,.007,.111),'Apron',{'Hips':1},.004)
    s.tube([(-.08,-.147,.962),(.054,-.147,.962)],[.0018]*2,'Seam',{'Hips':1},6)
    s.box((.164,-.072,.928),(.071,.044,.136),'Leather',{'Hips':1},.005)
    s.box((.163,-.097,.928),(.059,.008,.122),'Paper',{'Hips':1},.002)
    s.box((.164,-.104,.928),(.071,.007,.136),'Leather',{'Hips':1},.002)
    s.box((.164,-.11,.925),(.076,.005,.012),'Brass',{'Hips':1},.001)
    for z in (.886,.897,.908,.941,.953):
        s.tube([(.137,-.103,z),(.188,-.103,z)],[.0006]*2,'Paper',{'Hips':1},6)
    obj=s.object('CharacterMesh',c)
    obj['asset']='Seorin_Character'
    return obj,bones


def stage_setup():
    OUT.mkdir(parents=True,exist_ok=True)
    if SCENE in bpy.data.scenes:
        return {'scene':SCENE,'alreadyExists':True,'baseline':json.loads(scene()['baseline'])}
    original=baseline()
    s=bpy.data.scenes.new(SCENE)
    s.unit_settings.system='METRIC'; s.unit_settings.scale_length=1
    s.render.fps=60; s.frame_start=1; s.frame_end=121
    s['baseline']=json.dumps(original)
    s['rfc']='RFC-CX-018'; s['runtimeEligible']=False
    s['productionStages']='[]'
    return {'scene':SCENE,'blenderVersion':bpy.app.version_string,'baseline':original}


def stage_model():
    if scene().get('modeled'):
        archive('Refining shoulder seam closure and anatomically opposed thumb; preserving first candidate')
        for obj in list(scene().objects):
            if obj.get('asset') and obj.type in {'MESH','ARMATURE'}:
                data=obj.data
                bpy.data.objects.remove(obj,do_unlink=True)
                if data.users==0:
                    (bpy.data.meshes if isinstance(data,bpy.types.Mesh) else bpy.data.armatures).remove(data)
        for action in list(bpy.data.actions):
            if action.name.startswith(PREFIX+'Seorin_'):
                bpy.data.actions.remove(action)
    c=collection('Character'); obj,bones=build_character(c)
    obj['bones']=json.dumps(bones)
    for side in ('Left','Right'):
        c=collection(side+'Hand'); s=Surface(); bones=hand(s,side)
        obj=s.object(side+'HandMesh',c); obj['asset']='Seorin_'+side+'Hand'; obj['bones']=json.dumps(bones)
    scene()['modeled']=True
    return {'meshes':[measure(o) for o in scene().objects if o.type=='MESH']}


def stage_rig():
    rigs=[]
    for obj in list(scene().objects):
        if obj.type!='MESH' or not obj.get('bones'):
            continue
        name=obj['asset']+'_Rig'
        if PREFIX+name in bpy.data.objects:
            raise RuntimeError('Rig already exists: '+name)
        data=bpy.data.armatures.new(PREFIX+name)
        arm=bpy.data.objects.new(PREFIX+name,data)
        obj.users_collection[0].objects.link(arm)
        bpy.context.view_layer.objects.active=arm
        arm.select_set(True)
        bpy.ops.object.mode_set(mode='EDIT')
        for bn,a,b,parent in json.loads(obj['bones']):
            bone=data.edit_bones.new(bn); bone.head=a; bone.tail=b
            if parent:
                bone.parent=data.edit_bones[parent]
            bone.use_deform=True
            # For a -Y finger, X is the flexion axis; align roll to dorsal +Z.
            bone.align_roll(Vector((0,0,1)))
        bpy.ops.object.mode_set(mode='OBJECT')
        obj.parent=arm
        mod=obj.modifiers.new('AuthoredSkin','ARMATURE'); mod.object=arm
        arm['asset']=obj['asset']; arm['unityRig']='Generic'; arm['rootMotion']=False
        arm.select_set(False); rigs.append({'name':arm.name,'bones':len(data.bones),'names':list(data.bones.keys())})
    return {'rigs':rigs}


def reset_pose(arm):
    for p in arm.pose.bones:
        p.rotation_mode='XYZ'; p.location=(0,0,0); p.rotation_euler=(0,0,0); p.scale=(1,1,1)


def pose_hand(arm,clip,t):
    # Angles are radians around authored local finger axes. Root stays fixed.
    reset_pose(arm)
    side=1 if 'Left' in arm['asset'] else -1
    if clip=='Hands_Rest':
        curl=.10+.015*math.sin(t*math.tau); wrist=0
    elif clip=='Hands_Grip':
        curl=.12+1.0*min(1,t/.75); wrist=.055*min(1,t/.75)
    elif clip=='Hands_Insert':
        pulse=math.sin(min(1,t/.8)*math.pi/2) if t<.8 else (1-t)/.2
        curl=.12+.13*pulse; wrist=-.12*pulse
    elif clip=='Hands_Seal':
        pulse=math.sin(math.pi*t)
        curl=.12+.43*pulse; wrist=.18*pulse
    else:
        pulse=math.sin(math.pi*t)
        curl=.12+.12*pulse; wrist=.06*pulse
    arm.pose.bones['Wrist'].rotation_euler=(wrist,0,side*.025*math.sin(math.pi*t))
    for digit in ('Index','Middle','Ring','Little'):
        for j in (1,2,3):
            p=arm.pose.bones[digit+'%02d'%j]
            p.rotation_euler.x=-curl*(.78 if j==1 else (1.10 if j==2 else .74))
    for j in (1,2,3):
        p=arm.pose.bones['Thumb%02d'%j]
        p.rotation_euler.x=-curl*.65
        p.rotation_euler.y=-side*curl*.15 if j==1 else 0
        p.rotation_euler.z=-side*curl*(.85 if j==1 else .10)


def stage_animate():
    clips=[]
    for arm in [o for o in scene().objects if o.type=='ARMATURE']:
        arm.animation_data_create()
        if len(arm.animation_data.nla_tracks):
            raise RuntimeError('Animation already exists: '+arm.name)
        names=['Seorin_Idle'] if arm['asset']=='Seorin_Character' else list(CLIPS)[1:]
        for name in names:
            end,contact=CLIPS[name]
            action=bpy.data.actions.new(PREFIX+arm['asset']+'__'+name)
            arm.animation_data.action=action
            for f in range(end+1):
                t=f/end
                reset_pose(arm)
                if name=='Seorin_Idle':
                    breath=math.sin(t*math.tau)
                    arm.pose.bones['Chest'].rotation_euler.x=.008*breath
                    arm.pose.bones['Neck'].rotation_euler.x=-.004*breath
                    arm.pose.bones['Head'].rotation_euler.z=.006*breath
                    for side in ('Left','Right'):
                        for digit in ('Index','Middle','Ring','Little'):
                            for j in (1,2,3):
                                arm.pose.bones[side+digit+'%02d'%j].rotation_euler.x=-.10
                else:
                    pose_hand(arm,name,t)
                for bone in arm.pose.bones:
                    bone.keyframe_insert(data_path='rotation_euler',frame=f+1,group=bone.name)
                    bone.keyframe_insert(data_path='location',frame=f+1,group=bone.name)
            action.use_fake_user=True
            arm.animation_data.action=None
            track=arm.animation_data.nla_tracks.new(); track.name=name
            strip=track.strips.new(name,1,action); strip.name=name
            strip.action_frame_start=1; strip.action_frame_end=end+1
            track.mute=True
            clips.append({'asset':arm['asset'],'clip':name,'action':action.name,
                          'startFrame':1,'endFrame':end+1,'durationSeconds':end/60,
                          'contactFrame':contact+1 if contact is not None else None,
                          'contactMs':contact/60*1000 if contact is not None else None})
        reset_pose(arm)
    scene()['clips']=json.dumps(clips)
    scene().frame_set(1)
    return {'fps':60,'clips':clips}


def measure(obj):
    mesh=obj.data
    mesh.calc_loop_triangles()
    weighted=[sum(1 for g in v.groups if g.weight>0) for v in mesh.vertices]
    bounds=[[min(v.co[i] for v in mesh.vertices),max(v.co[i] for v in mesh.vertices)] for i in range(3)]
    return {'asset':obj.get('asset'), 'object':obj.name,'vertices':len(mesh.vertices),
        'triangles':len(mesh.loop_triangles),'polygons':len(mesh.polygons),
        'boundsBlenderXYZ':bounds,'weightedVertices':sum(n>0 for n in weighted),
        'maxWeightsPerVertex':max(weighted,default=0),'vertexGroups':len(obj.vertex_groups),
        'skinned':any(m.type=='ARMATURE' for m in obj.modifiers),
        'materials':list(dict.fromkeys(mesh.materials[p.material_index].name for p in mesh.polygons))}


def selected_asset(arm):
    for obj in scene().objects:
        obj.select_set(False)
    arm.select_set(True)
    for child in arm.children:
        child.select_set(True)
    bpy.context.view_layer.objects.active=arm


def glb_info(path):
    data=Path(path).read_bytes()
    length,kind=struct.unpack_from('<II',data,12)
    doc=json.loads(data[20:20+length])
    bin_start=20+length+8
    def values(index):
        accessor=doc['accessors'][index]; view=doc['bufferViews'][accessor['bufferView']]
        width={'SCALAR':1,'VEC3':3,'VEC4':4}[accessor['type']]
        offset=bin_start+view.get('byteOffset',0)+accessor.get('byteOffset',0)
        stride=view.get('byteStride',width*4)
        return [struct.unpack_from('<'+'f'*width,data,offset+i*stride) for i in range(accessor['count'])]
    clips=[]
    for a in doc.get('animations',[]):
        durations=[doc['accessors'][s['input']]['max'][0]-doc['accessors'][s['input']]['min'][0]
                   for s in a['samplers']]
        moving=[]
        root_translation_delta=0
        for channel in a['channels']:
            target=channel['target']; node=doc['nodes'][target['node']]['name']
            samples=values(a['samplers'][channel['sampler']]['output'])
            delta=max(abs(v[j]-samples[0][j]) for v in samples for j in range(len(v)))
            if delta>1e-6:
                moving.append(node+':'+target['path'])
            if node in ('Root','Forearm') and target['path']=='translation':
                root_translation_delta=max(root_translation_delta,delta)
        clips.append({'name':a.get('name'),'durationSeconds':max(durations),
                      'channels':len(a['channels']),'movingChannels':moving,
                      'rootTranslationMaxDeltaMeters':root_translation_delta})
    return {'clips':clips,'skins':len(doc.get('skins',[])),
            'skinJointCounts':[len(s['joints']) for s in doc.get('skins',[])],
            'meshes':len(doc.get('meshes',[]))}


def fbx_info(path):
    from io_scene_fbx import parse_fbx
    root,version=parse_fbx.parse(str(path))
    stacks=[]
    def visit(elem):
        if elem.id==b'AnimationStack':
            name=elem.props[1].decode('utf8').split('\x00')[0]
            values={}
            for child in elem.elems:
                if child.id==b'Properties70':
                    for p in child.elems:
                        if p.id==b'P':
                            values[p.props[0].decode('utf8')]=p.props[-1]
            stacks.append({'name':name,
                'firstFrame':values.get('LocalStart',0)/46186158000*60,
                'lastFrame':values.get('LocalStop',0)/46186158000*60,
                'durationSeconds':(values.get('LocalStop',0)-values.get('LocalStart',0))/46186158000})
        for child in elem.elems:
            visit(child)
    visit(root)
    return {'version':version,'clips':stacks}


def manifest():
    s=scene()
    return {'rfc':'RFC-CX-018','runtimeEligible':False,'promotedBy':None,
        'tool':'Blender MCP execute_blender_code via execute_m22_blender_stage',
        'blenderVersion':bpy.app.version_string,'method':'Original procedural mesh authoring and authored skeletal animation; no third-party mesh or texture',
        'sourceScript':'scripts/blender/build-seorin-m22.py',
        'sourceScriptSha256':sha(ROOT/'scripts/blender/build-seorin-m22.py'),
        'conceptRef':'assets/generated/2d/concept/char-seorin-sheet.png',
        'conceptSha256':sha(ROOT/'assets/generated/2d/concept/char-seorin-sheet.png'),
        'license':{'authorship':'Original geometry/materials/animation authored for this project',
                   'thirdPartyAssetsEmbedded':False,'externalLicenseClearance':'Not claimed; supplied generated concept used only as visual reference'},
        'generatedAt':datetime.now(timezone.utc).isoformat(),'palette':PALETTE,
        'materials':[{'name':material(k).name,'color':list(material(k).diffuse_color),
                      'roughness':v[1],'metallic':v[2]} for k,v in PALETTE.items()],
        'units':'meters','blenderAxes':{'up':'+Z','forward':'-Y'},
        'fbxSettings':{'axis_up':'Y','axis_forward':'-Z','global_scale':1,'bake_space_transform':False,
            'add_leaf_bones':False,'use_armature_deform_only':True,'bake_anim':True,
            'bake_anim_use_all_actions':False,'bake_anim_use_nla_strips':True,'bake_anim_simplify_factor':0},
        'unityRig':'Generic','rootMotion':False,'fps':60,
        'geometry':[measure(o) for o in s.objects if o.type=='MESH' and o.get('asset')],
        'rigs':[{'asset':o['asset'],'armature':o.name,'boneCount':len(o.data.bones),
            'file':o['asset']+'.fbx','rootBone':'Root' if o['asset']=='Seorin_Character' else 'Forearm',
            'gripBone':None if o['asset']=='Seorin_Character' else 'Contact','gripPoint':[0,0,0],
            'gripPoseTime':.4,
            'clips':[{'name':c['name'],'takeName':c['name'],'firstFrame':c['firstFrame'],
                      'lastFrame':c['lastFrame'],'durationSeconds':c['durationSeconds']}
                     for c in json.loads(s.get('exports','{}')).get(o['asset'],{}).get('fbx',{}).get('clips',[])],
            'bones':[{'name':b.name,'parent':b.parent.name if b.parent else None,
              'headBlender':list(b.head_local),'tailBlender':list(b.tail_local),
              'restMatrix':[list(row) for row in b.matrix_local],'deform':b.use_deform} for b in o.data.bones]}
            for o in s.objects if o.type=='ARMATURE'],
        'authoredClips':json.loads(s.get('clips','[]')),
        'handFrames':{'wristOriginBlender':[0,0,0],'fingersBlender':[0,-1,0],
            'dorsalBlender':[0,0,1],'forearmTowardElbowBlender':[0,1,0],
            'contactBone':'Contact','contactCenterRestBlender':[0,-.1,-.04],
            'gripAxisRestBlender':[1,0,0],
            'note':'Contact is wrist-parented, not a fingertip. Runtime anchors actual posed Contact to crank handle, not wrist to handle.'},
        'preservation':{'baseline':json.loads(s['baseline']),'observedAfter':baseline(),
                        'unchanged':json.loads(s['baseline'])==baseline()},
        'structuralPreservation':{'baselineScope':'Supplemental capture after first production candidate; initial setup captured transforms/names/visibility/filepath only.',
            'beforeFinalProduction':json.loads(s.get('structuralBaseline','null')),
            'afterFinalProduction':original_structure(),
            'unchanged':json.loads(s.get('structuralBaseline','null'))==original_structure(),
            'sessionDirty':bpy.data.is_dirty,'note':'New M22 datablocks may dirty session; original filepath never saved.'},
        'productionStages':json.loads(s.get('productionStages','[]')),
        'knownRisks':['Stylized solid-material original, not a photoreal reconstruction.',
            'Human review and Unity native import/contact inspection still required; no runtime promotion claimed.']}


def write_manifest():
    m=manifest()
    m['exports']=json.loads(scene().get('exports','{}'))
    m['renders']=json.loads(scene().get('renders','[]'))
    m['files']=[{'file':str(p.relative_to(OUT)),'sha256':sha(p),'bytes':p.stat().st_size}
                for p in OUT.rglob('*') if p.is_file() and 'history' not in p.parts
                and p.name not in ('manifest.json','provenance.json')]
    (OUT/'manifest.json').write_text(json.dumps(m,indent=2))
    (OUT/'provenance.json').write_text(json.dumps({k:m[k] for k in
        ('rfc','runtimeEligible','promotedBy','tool','method','sourceScript','sourceScriptSha256',
         'conceptRef','conceptSha256','license','generatedAt','files')},indent=2))
    return m


def stage_export():
    preserved=archive('Export candidate regeneration')
    result={}
    for arm in [o for o in scene().objects if o.type=='ARMATURE']:
        selected_asset(arm); reset_pose(arm)
        tracks=arm.animation_data.nla_tracks
        for t in tracks:
            t.mute=False
        base=OUT/arm['asset']
        bpy.ops.export_scene.gltf(filepath=str(base)+'.glb',export_format='GLB',
            use_selection=True,use_active_scene=True,export_yup=True,export_animations=True,
            export_animation_mode='NLA_TRACKS',export_force_sampling=True,
            export_skins=True,export_def_bones=True,export_all_influences=False)
        bpy.ops.export_scene.fbx(filepath=str(base)+'.fbx',use_selection=True,
            object_types={'ARMATURE','MESH'},global_scale=1,apply_unit_scale=True,
            apply_scale_options='FBX_SCALE_NONE',axis_forward='-Z',axis_up='Y',
            bake_space_transform=False,add_leaf_bones=False,use_armature_deform_only=True,
            bake_anim=True,bake_anim_use_all_bones=True,bake_anim_use_nla_strips=True,
            bake_anim_use_all_actions=False,bake_anim_step=1,bake_anim_simplify_factor=0,
            mesh_smooth_type='FACE',path_mode='AUTO')
        for t in tracks:
            t.mute=True
        reset_pose(arm)
        result[arm['asset']]={'glb':glb_info(str(base)+'.glb'),'fbx':fbx_info(str(base)+'.fbx')}
    scene()['exports']=json.dumps(result)
    scene().frame_set(1)
    bpy.data.libraries.write(str(OUT/'Seorin_Character.blend'),{scene()},fake_user=True,compress=True)
    m=write_manifest()
    return {'exports':result,'geometry':m['geometry'],'preservedCandidate':preserved,
            'scenePreserved':m['preservation']['unchanged']}


def aim(obj,target):
    obj.rotation_euler=(Vector(target)-obj.location).to_track_quat('-Z','Y').to_euler()


def stage_render(part='next'):
    archive('Preview render generation with complete candidate preservation')
    s=scene()
    views=('character','face','Hands_Rest','Hands_Grip','Hands_Insert','Hands_Seal','Hands_Align')
    if part=='next':
        index=int(s.get('renderCursor',0))%len(views)
        part=views[index]
        s['renderCursor']=index+1
    studio=bpy.data.collections.get(PREFIX+'Studio')
    if not studio:
        studio=collection('Studio')
        world=bpy.data.worlds.new(PREFIX+'World'); world.use_nodes=True
        world.node_tree.nodes['Background'].inputs[0].default_value=(.16,.19,.18,1)
        world.node_tree.nodes['Background'].inputs[1].default_value=.5; s.world=world
        for name,loc,power,size,color in [
            ('Key',(-2.5,-3.5,4),650,3,(1,.86,.71)),
            ('Fill',(3,-1,2.5),480,3,(.71,.84,1)),
            ('Rim',(1,2,3),800,2,(.90,1,.93))]:
            data=bpy.data.lights.new(PREFIX+name,'AREA'); data.energy=power; data.shape='DISK'; data.size=size; data.color=color
            obj=bpy.data.objects.new(PREFIX+name,data); studio.objects.link(obj); obj.location=loc; aim(obj,(0,0,.9))
        data=bpy.data.cameras.new(PREFIX+'PreviewCamera'); cam=bpy.data.objects.new(PREFIX+'PreviewCamera',data)
        studio.objects.link(cam); data.type='ORTHO'; s.camera=cam
    cam=s.camera
    s.world.node_tree.nodes['Background'].inputs[1].default_value=.28
    for name,power in [('Key',360),('Fill',220),('Rim',460)]:
        bpy.data.lights[PREFIX+name].energy=power
    s.render.engine='CYCLES'; s.cycles.samples=32; s.cycles.use_denoising=True
    s.render.resolution_percentage=100; s.render.image_settings.file_format='PNG'
    s.render.film_transparent=False; s.view_settings.view_transform='AgX'
    renders=[]
    rigs=[o for o in s.objects if o.type=='ARMATURE']
    for arm in rigs:
        reset_pose(arm)
        arm.hide_render=arm['asset']!='Seorin_Character'
        for child in arm.children: child.hide_render=arm.hide_render
    cam.location=(2.5,-5,2.45); aim(cam,(0,0,.87)); cam.data.ortho_scale=1.98
    s.render.resolution_x=900;s.render.resolution_y=1200
    if part in ('all','character'):
        path=OUT/'preview-character.png'; s.render.filepath=str(path); bpy.ops.render.render(write_still=True); renders.append(path.name)
    cam.location=(.95,-2.6,1.72); aim(cam,(0,-.01,1.49)); cam.data.ortho_scale=.62
    s.render.resolution_x=1000;s.render.resolution_y=1000
    if part in ('all','face'):
        path=OUT/'preview-face.png';s.render.filepath=str(path);bpy.ops.render.render(write_still=True);renders.append(path.name)
    # Single close-up frame per required pose, both left and right hands visible.
    for arm in rigs:
        visible=arm['asset']!='Seorin_Character'
        arm.hide_render=not visible
        for child in arm.children: child.hide_render=not visible
        if visible: arm.location.x=-.115 if 'Right' in arm['asset'] else .115
    cam.location=(.12,-.48,.64); aim(cam,(0,.03,0)); cam.data.ortho_scale=.57
    s.render.resolution_x=1200;s.render.resolution_y=900
    for name,t in [('Hands_Rest',0),('Hands_Grip',1),('Hands_Insert',.8),('Hands_Seal',.5),('Hands_Align',.5)]:
        if part not in ('all',name):
            continue
        for arm in rigs:
            if arm['asset']!='Seorin_Character': pose_hand(arm,name,t)
        bpy.context.view_layer.update()
        path=OUT/('preview-'+name.lower().replace('hands_','hands-')+'.png')
        s.render.filepath=str(path);bpy.ops.render.render(write_still=True);renders.append(path.name)
    for arm in rigs:
        arm.location=(0,0,0);arm.hide_render=False;reset_pose(arm)
        for child in arm.children: child.hide_render=False
    s['renders']=json.dumps(sorted(set(json.loads(s.get('renders','[]'))+renders)))
    bpy.data.libraries.write(str(OUT/'Seorin_Character.blend'),{s},fake_user=True,compress=True)
    m=write_manifest()
    return {'renders':renders,'scenePreserved':m['preservation']['unchanged']}


def run_stage(stage):
    stages={'setup':stage_setup,'model':stage_model,'rig':stage_rig,
            'animate':stage_animate,'export':stage_export,'render':stage_render}
    if stage not in stages:
        raise ValueError('Unknown stage '+stage)
    original_scene=bpy.context.window.scene
    original_active=bpy.context.view_layer.objects.active
    selected=list(bpy.context.selected_objects)
    before=baseline()
    structure_before=original_structure()
    if SCENE in bpy.data.scenes and not scene().get('structuralBaseline'):
        scene()['structuralBaseline']=json.dumps(structure_before)
    try:
        if stage!='setup':
            bpy.context.window.scene=scene()
        result=stages[stage]()
        if SCENE in bpy.data.scenes:
            log=json.loads(scene().get('productionStages','[]'))
            log.append({'stage':stage,'at':datetime.now(timezone.utc).isoformat()})
            scene()['productionStages']=json.dumps(log)
            if stage=='export' or stage.startswith('render'):
                write_manifest()
        return {'stage':stage,'result':result,'originalSceneUnchanged':before==baseline(),
                'originalStructureUnchanged':structure_before==original_structure()}
    finally:
        if bpy.context.object and bpy.context.object.mode!='OBJECT':
            bpy.ops.object.mode_set(mode='OBJECT')
        bpy.context.window.scene=original_scene
        bpy.context.view_layer.objects.active=original_active
        for o in selected:
            if o.name in original_scene.objects: o.select_set(True)
