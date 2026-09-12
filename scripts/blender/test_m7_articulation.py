"""M7 optical-reader articulation test (shot S02: forward -> stop -> return).

Headless: /Applications/Blender.app/Contents/MacOS/Blender --background \
    --factory-startup --python scripts/blender/test_m7_articulation.py

Opens the authoritative blend READ-ONLY, builds pivot empties for the
handcrank and magnifier arm, animates 48 frames @24fps, asserts exact
return-to-origin at frame 48, runs bbox collision sanity at frames 12/24,
renders 4 check frames, and saves a DERIVED scene (never the source).
Prints one line: M7_ARTIC_SUMMARY=<json>
"""
import hashlib
import json
import math
import os
import sys
import traceback

import bpy
from mathutils import Matrix, Vector

REPO = os.path.abspath(os.path.join(os.path.dirname(os.path.abspath(__file__)), "..", ".."))
SRC_BLEND = os.path.join(REPO, "assets/generated/3d/concept-first-m7/concept-first-m7.blend")
OUT_DIR = os.path.join(REPO, "assets/generated/3d/concept-first-m7/articulation")
DERIVED_BLEND = os.path.join(REPO, "assets/generated/3d/concept-first-m7/concept-first-m7-articulation.blend")
REPORT = os.path.join(OUT_DIR, "articulation-report.json")

CRANK_CHILDREN = ["rd-crank-axle", "rd-crank-arm", "rd-crank-handle"]
MAG_CHILDREN = ["rd-mag-ring", "rd-mag-glass"]
STATIC_REFS = ["rd-crank-boss", "rd-mag-hinge", "rd-base", "wr-bench-top"]
CAMERA = "CAM-reader-close"
RENDER_FRAMES = [1, 12, 24, 40]
TOL_PER_COMPONENT = 1e-4


def sha256_of(path):
    h = hashlib.sha256()
    with open(path, "rb") as f:
        for chunk in iter(lambda: f.read(1 << 20), b""):
            h.update(chunk)
    return h.hexdigest()


def world_aabb(obj_eval):
    mw = obj_eval.matrix_world
    pts = [mw @ Vector(c) for c in obj_eval.bound_box]
    lo = Vector((min(p.x for p in pts), min(p.y for p in pts), min(p.z for p in pts)))
    hi = Vector((max(p.x for p in pts), max(p.y for p in pts), max(p.z for p in pts)))
    return lo, hi


def aabb_relation(a, b):
    """Return (overlap: bool, distance: float). Negative distance = penetration depth."""
    (alo, ahi), (blo, bhi) = a, b
    gaps, overlaps = [], []
    for i in range(3):
        gap = max(alo[i] - bhi[i], blo[i] - ahi[i])
        if gap > 0.0:
            gaps.append(gap)
        else:
            overlaps.append(-gap)
    if gaps:
        return False, math.sqrt(sum(g * g for g in gaps))
    return True, -min(overlaps)


def action_fcurves(obj):
    ad = obj.animation_data
    if not ad or not ad.action:
        return []
    act = ad.action
    try:
        fcs = list(act.fcurves)
        if fcs:
            return fcs
    except Exception:
        pass
    try:  # slotted actions (Blender >= 4.4 layered API)
        fcs = []
        for layer in act.layers:
            for strip in layer.strips:
                cb = strip.channelbag(ad.action_slot, ensure=False)
                if cb:
                    fcs.extend(cb.fcurves)
        return fcs
    except Exception:
        return []


def make_pivot(name, location, spin_axis_world, collection):
    """Empty whose local X is spin_axis_world; euler XYZ so animating the X
    channel spins about local X (R' = R @ Rx(delta))."""
    pivot = bpy.data.objects.new(name, None)
    pivot.empty_display_type = "PLAIN_AXES"
    pivot.empty_display_size = 0.12
    collection.objects.link(pivot)
    q = Vector((1.0, 0.0, 0.0)).rotation_difference(spin_axis_world.normalized())
    pivot.rotation_mode = "XYZ"
    pivot.matrix_world = Matrix.Translation(location) @ q.to_matrix().to_4x4()
    return pivot


def parent_keep_transform(child, pivot, pivot_world):
    child.parent = pivot
    child.matrix_parent_inverse = pivot_world.inverted()


def key_spin(pivot, base_x, keys):
    """keys: list of (frame, degrees offset about local X)."""
    for frame, deg in keys:
        pivot.rotation_euler[0] = base_x + math.radians(deg)
        pivot.keyframe_insert(data_path="rotation_euler", index=0, frame=frame)
    linear = True
    fcs = action_fcurves(pivot)
    if not fcs:
        linear = False
    for fc in fcs:
        for kp in fc.keyframe_points:
            kp.interpolation = "LINEAR"
    return linear


def main():
    src_sha_before = sha256_of(SRC_BLEND)
    os.makedirs(OUT_DIR, exist_ok=True)

    bpy.ops.wm.open_mainfile(filepath=SRC_BLEND)
    scene = bpy.context.scene

    missing = [n for n in CRANK_CHILDREN + MAG_CHILDREN + STATIC_REFS + [CAMERA]
               if n not in bpy.data.objects]
    if missing:
        raise RuntimeError(f"missing objects: {missing}")

    objs = {n: bpy.data.objects[n] for n in CRANK_CHILDREN + MAG_CHILDREN + STATIC_REFS}
    coll = bpy.data.collections.get("M7-PROP-optical-reader") or scene.collection

    # ---- record pre-animation (and pre-parenting) world matrices
    bpy.context.view_layer.update()
    original = {n: objs[n].matrix_world.copy() for n in CRANK_CHILDREN + MAG_CHILDREN}

    # ---- crank pivot: origin + spin axis from the axle's own matrix (cyl local Z)
    axle = objs["rd-crank-axle"]
    crank_loc = axle.matrix_world.translation.copy()
    crank_axis = (axle.matrix_world.to_3x3() @ Vector((0.0, 0.0, 1.0))).normalized()
    if crank_axis.x < 0:
        crank_axis = -crank_axis  # canonical +X-ish spin axis
    crank_pivot = make_pivot("rd-crank-pivot", crank_loc, crank_axis, coll)

    # ---- magnifier pivot at hinge origin, swing axis = world Y
    hinge = objs["rd-mag-hinge"]
    mag_loc = hinge.matrix_world.translation.copy()
    mag_pivot = make_pivot("rd-mag-pivot", mag_loc, Vector((0.0, 1.0, 0.0)), coll)

    bpy.context.view_layer.update()
    for n in CRANK_CHILDREN:
        parent_keep_transform(objs[n], crank_pivot, crank_pivot.matrix_world.copy())
    for n in MAG_CHILDREN:
        parent_keep_transform(objs[n], mag_pivot, mag_pivot.matrix_world.copy())
    bpy.context.view_layer.update()

    parent_dev = max(
        max(abs(objs[n].matrix_world[i][j] - original[n][i][j])
            for i in range(4) for j in range(4))
        for n in CRANK_CHILDREN + MAG_CHILDREN
    )

    # ---- animation: 48 frames @ 24fps
    scene.render.fps = 24
    scene.frame_start, scene.frame_end = 1, 48
    crank_linear = key_spin(crank_pivot, crank_pivot.rotation_euler[0],
                            [(1, 0.0), (24, 180.0), (32, 180.0), (48, 0.0)])
    mag_linear = key_spin(mag_pivot, mag_pivot.rotation_euler[0],
                          [(1, 0.0), (16, -20.0), (32, -20.0), (48, 0.0)])

    def eval_matrix(name, frame):
        scene.frame_set(frame)
        deps = bpy.context.evaluated_depsgraph_get()
        return objs[name].evaluated_get(deps).matrix_world.copy()

    # ---- frame 48: return-to-origin assertion
    deviations = {}
    for n in CRANK_CHILDREN + MAG_CHILDREN:
        m48 = eval_matrix(n, 48)
        deviations[n] = max(abs(m48[i][j] - original[n][i][j])
                            for i in range(4) for j in range(4))
    max_dev = max(deviations.values())
    return_ok = max_dev <= TOL_PER_COMPONENT

    # ---- collision sanity at frames 12 and 24
    collisions = []
    for frame in (12, 24):
        scene.frame_set(frame)
        deps = bpy.context.evaluated_depsgraph_get()
        hb = world_aabb(objs["rd-crank-handle"].evaluated_get(deps))
        for ref in ("rd-base", "wr-bench-top"):
            rb = world_aabb(objs[ref].evaluated_get(deps))
            overlap, dist = aabb_relation(hb, rb)
            collisions.append({
                "frame": frame,
                "pair": f"rd-crank-handle vs {ref}",
                "overlap": overlap,
                "distanceM": round(dist, 5),
                "handleBboxZ": [round(hb[0].z, 4), round(hb[1].z, 4)],
            })

    # ---- renders
    scene.camera = bpy.data.objects[CAMERA]
    scene.render.engine = "CYCLES"
    scene.render.resolution_x, scene.render.resolution_y = 640, 360
    scene.render.resolution_percentage = 100
    scene.render.image_settings.file_format = "PNG"
    scene.cycles.samples = 32
    device = "CPU"
    try:
        prefs = bpy.context.preferences.addons["cycles"].preferences
        prefs.compute_device_type = "METAL"
        try:
            prefs.refresh_devices()
        except Exception:
            prefs.get_devices()
        for d in prefs.devices:
            d.use = d.type in {"METAL", "CPU"}
        scene.cycles.device = "GPU"
        device = "GPU-METAL"
    except Exception:
        scene.cycles.device = "CPU"
        device = "CPU"

    rendered = []
    for frame in RENDER_FRAMES:
        scene.frame_set(frame)
        out = os.path.join(OUT_DIR, f"frame-{frame:02d}.png")
        scene.render.filepath = out
        try:
            bpy.ops.render.render(write_still=True)
        except Exception:
            if device != "CPU":  # Metal failed mid-run -> CPU fallback, retry all
                scene.cycles.device = "CPU"
                device = "CPU-fallback"
                bpy.ops.render.render(write_still=True)
            else:
                raise
        rendered.append(os.path.relpath(out, REPO))

    # ---- save derived scene (never the source)
    assert os.path.abspath(DERIVED_BLEND) != os.path.abspath(SRC_BLEND)
    bpy.ops.wm.save_as_mainfile(filepath=DERIVED_BLEND)
    src_sha_after = sha256_of(SRC_BLEND)

    summary = {
        "maxReturnDeviation": max_dev,
        "returnWithinTolerance": return_ok,
        "perObjectDeviation": {k: round(v, 9) for k, v in deviations.items()},
        "parentingPreservationDeviation": parent_dev,
        "collisions": collisions,
        "renderedFiles": rendered,
        "renderDevice": device,
        "linearInterpolationApplied": bool(crank_linear and mag_linear),
        "sourceShaBefore": src_sha_before,
        "sourceShaAfter": src_sha_after,
        "sourceUnchanged": src_sha_before == src_sha_after,
    }
    print("M7_ARTIC_SUMMARY=" + json.dumps(summary))

    status = "pass" if (return_ok and summary["sourceUnchanged"]) else "fail"
    obs = [
        f"[OBSERVED] frame-48 max world-matrix component deviation {max_dev:.3e} "
        f"across {len(deviations)} animated objects (tolerance {TOL_PER_COMPONENT:.0e}) — "
        f"crank +180deg forward (f1-24), hold (f25-32), return (f33-48); "
        f"mag -20deg swing (f1-16), hold, return by f48; return-to-origin "
        f"{'holds' if return_ok else 'FAILS'}",
        f"[OBSERVED] parenting with keep-transform preserved world matrices to "
        f"{parent_dev:.3e} max component deviation before animation",
        f"[OBSERVED] crank spin axis derived from rd-crank-axle matrix_world: "
        f"({crank_axis.x:.4f}, {crank_axis.y:.4f}, {crank_axis.z:.4f}), pivot at "
        f"({crank_loc.x:.3f}, {crank_loc.y:.3f}, {crank_loc.z:.3f}); mag pivot at "
        f"({mag_loc.x:.3f}, {mag_loc.y:.3f}, {mag_loc.z:.3f}) swing axis world Y",
    ]
    for c in collisions:
        state = (f"OVERLAP penetration {-c['distanceM']:.3f} m" if c["overlap"]
                 else f"clear, min gap {c['distanceM']:.3f} m")
        obs.append(f"[OBSERVED] frame {c['frame']}: {c['pair']} world-AABB {state} "
                   f"(handle z-range {c['handleBboxZ']})")
    obs.append(f"[OBSERVED] rendered {len(rendered)} check frames at 640x360 Cycles "
               f"32 samples on {device}")
    obs.append(f"[OBSERVED] source blend sha256 unchanged: "
               f"{summary['sourceUnchanged']} ({src_sha_before[:16]}...)")

    report = {
        "lane": "m7-articulation",
        "status": status,
        "observations": obs,
        "artifacts": [os.path.relpath(p, REPO) for p in
                      [DERIVED_BLEND, REPORT]] + rendered,
        "untested": [
            "load-bearing rig / skinned deformation for runtime",
            "gear-ratio linkage between crank rotation and hex-plate rotation",
            "runtime engine (Unity) animation playback and interpolation",
            "mesh-level (non-AABB) collision between handle and bench/base",
            "magnifier arm reachable range beyond the tested -20deg swing",
        ],
    }
    with open(REPORT, "w") as f:
        json.dump(report, f, indent=2)
    return 0 if status == "pass" else 1


if __name__ == "__main__":
    try:
        sys.exit(main())
    except Exception:
        traceback.print_exc()
        sys.exit(1)
