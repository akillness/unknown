import bpy, math
from pathlib import Path
from mathutils import Vector
out = Path('/Users/jangyoung/orca/unknown/assets/generated/3d/hub-view-drawer-r01')
bpy.ops.wm.open_mainfile(filepath=str(out/'SM_Hub_Workbench_Drawer.blend'))
scene=bpy.context.scene
scene.render.engine='CYCLES'
scene.cycles.samples=32
scene.render.resolution_x=960
scene.render.resolution_y=640
scene.render.resolution_percentage=100
scene.render.image_settings.file_format='PNG'
scene.render.filepath=str(out/'preview.png')
scene.world.color=(0.12,0.12,0.12)
cam_data=bpy.data.cameras.new('DrawerAuditCamera')
cam=bpy.data.objects.new('DrawerAuditCamera',cam_data)
scene.collection.objects.link(cam)
cam.location=(1.5,-2.1,1.6)
cam.rotation_euler=(Vector((0,0,0.12))-cam.location).to_track_quat('-Z','Y').to_euler()
cam_data.type='ORTHO'
cam_data.ortho_scale=1.8
scene.camera=cam
for name,loc,power,size in [('Key',(-1,-2,3),500,3),('Fill',(2,1,2),350,2)]:
 d=bpy.data.lights.new('DrawerAudit'+name,'AREA'); d.energy=power; d.shape='DISK'; d.size=size
 o=bpy.data.objects.new('DrawerAudit'+name,d); scene.collection.objects.link(o); o.location=loc
 o.rotation_euler=(Vector((0,0,0.1))-o.location).to_track_quat('-Z','Y').to_euler()
scene.render.film_transparent=False
bpy.ops.render.render(write_still=True)
