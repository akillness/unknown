#!/usr/bin/env python3
"""gen-higgsfield.py — batch Higgsfield CLI generation with provenance (M25, RFC-CX-M25-20260918).

Usage:
  scripts/gen-higgsfield.py <jobs.json> [--only id1,id2] [--parallel 3] [--dry-run] [--force]

jobs.json:
  {"jobs":[{"id":"bg-hub","set":"backgrounds","kind":"image|video","model":"gpt_image_2",
            "prompt":"...","params":{"aspect_ratio":"16:9","resolution":"2k"},
            "refs":["assets/generated/2d/concept/x.png"],          # --image (image_references)
            "start_image":"assets/generated/2d/m25/backgrounds/bg-hub.png"}]}  # video only

Outputs (never hand-edited):
  images → assets/generated/2d/m25/<set>/<id>.png  (+ <id>.job.json, provenance.json per set)
  videos → assets/generated/video/m25/<id>.mp4     (+ <id>.job.json, provenance.json)
Every entry starts runtimeEligible:false, license UNVERIFIED (Higgsfield ToS). Credits are read
before/after each job from `higgsfield account status`; the delta is recorded per asset.
"""
import argparse, concurrent.futures, datetime, hashlib, json, os, re, subprocess, sys, threading, time, urllib.request

ROOT = os.path.abspath(os.path.join(os.path.dirname(__file__), ".."))
LOCK = threading.Lock()

def sha256(path):
    h = hashlib.sha256()
    with open(path, "rb") as f:
        for chunk in iter(lambda: f.read(1 << 20), b""):
            h.update(chunk)
    return h.hexdigest()

def credits():
    out = subprocess.run(["higgsfield", "account", "status"], capture_output=True, text=True, timeout=60).stdout
    m = re.search(r"([\d.]+)\s+credits", out)
    return float(m.group(1)) if m else None

def out_paths(job):
    if job["kind"] == "video":
        d = os.path.join(ROOT, "assets/generated/video", job.get("root","m25"))
        return d, os.path.join(d, job["id"] + ".mp4")
    d = os.path.join(ROOT, "assets/generated/2d", job.get("root","m25"), job["set"])
    return d, os.path.join(d, job["id"] + ".png")

def build_cmd(job):
    cmd = ["higgsfield", "generate", "create", job["model"], "--prompt", job["prompt"]]
    for k, v in job.get("params", {}).items():
        cmd += ["--" + k, str(v).lower() if isinstance(v, bool) else str(v)]
    for r in job.get("refs", []):
        cmd += ["--image", os.path.join(ROOT, r)]
    if job.get("start_image"):
        cmd += ["--start-image", os.path.join(ROOT, job["start_image"])]
    if job.get("end_image"):
        cmd += ["--end-image", os.path.join(ROOT, job["end_image"])]
    cmd += ["--wait", "--wait-timeout", job.get("timeout", "20m"), "--wait-interval", "5s", "--json"]
    return cmd

def find_result_url(obj):
    if isinstance(obj, list) and obj:
        obj = obj[0]
    if isinstance(obj, dict):
        for key in ("result_url", "video_url", "url"):
            v = obj.get(key)
            if isinstance(v, str) and v.startswith("http"):
                return v, obj
        for key in ("results", "outputs"):
            v = obj.get(key)
            if isinstance(v, list):
                for item in v:
                    if isinstance(item, dict):
                        for k2 in ("url", "result_url"):
                            if isinstance(item.get(k2), str):
                                return item[k2], obj
                    elif isinstance(item, str) and item.startswith("http"):
                        return item, obj
    return None, obj if isinstance(obj, dict) else {}

def run_job(job, force, dry):
    out_dir, out_file = out_paths(job)
    os.makedirs(out_dir, exist_ok=True)
    if os.path.exists(out_file) and not force:
        return job["id"], "skip (exists)"
    cmd = build_cmd(job)
    if dry:
        return job["id"], "DRY " + " ".join(c if " " not in c else repr(c[:60] + "…") for c in cmd)
    with LOCK:
        before = credits()
    started = datetime.datetime.now(datetime.timezone.utc).isoformat()
    proc = subprocess.run(cmd, capture_output=True, text=True, stdin=subprocess.DEVNULL)
    log_base = os.path.join(out_dir, job["id"])
    with open(log_base + ".job.json", "w") as f:
        f.write(proc.stdout)
    if proc.stderr.strip():
        with open(log_base + ".stderr.log", "w") as f:
            f.write(proc.stderr)
    if proc.returncode != 0:
        return job["id"], "FAILED exit=%d %s" % (proc.returncode, proc.stderr.strip()[-300:])
    try:
        data = json.loads(proc.stdout)
    except json.JSONDecodeError as e:
        return job["id"], "FAILED json: %s" % e
    url, meta = find_result_url(data)
    if not url:
        return job["id"], "FAILED no result url (status=%s)" % meta.get("status")
    urllib.request.urlretrieve(url, out_file)
    with LOCK:
        after = credits()
    entry = {
        "id": job["id"], "file": os.path.basename(out_file), "set": job.get("set"), "kind": job["kind"],
        "category": job.get("category", "m25-" + job["kind"]),
        "tool": "higgsfield CLI 1.1.25", "model": job["model"], "job_id": meta.get("id"), "job_status": meta.get("status"),
        "params": job.get("params", {}), "prompt": job["prompt"], "prompt_sha256": hashlib.sha256(job["prompt"].encode()).hexdigest(),
        "references": [{"path": r, "sha256": sha256(os.path.join(ROOT, r))} for r in job.get("refs", [])],
        "start_image": job.get("start_image"), "start_image_sha256": sha256(os.path.join(ROOT, job["start_image"])) if job.get("start_image") else None,
        "source_url": url, "output_sha256": sha256(out_file), "bytes": os.path.getsize(out_file),
        "credits_before": before, "credits_after": after, "credits_delta": (round(before - after, 2) if before is not None and after is not None else None),
        "started_at": started, "generated_at": datetime.datetime.now(datetime.timezone.utc).isoformat(),
        "license": "UNVERIFIED (Higgsfield ToS)", "runtimeEligible": False, "promoted_by": None,
        "rfc": "RFC-CX-M25-20260918",
        "claim": "[OBSERVED] Higgsfield-generated %s; concept/previz candidate; NOT gameplay" % job["kind"],
    }
    if job["kind"] == "image":
        try:
            from PIL import Image
            with Image.open(out_file) as im:
                entry["actual_size"] = "%dx%d" % im.size
        except Exception:
            pass
    with LOCK:
        prov = os.path.join(out_dir, "provenance.json")
        d = {"schema": "provenance/v1", "assets": []}
        if os.path.exists(prov):
            d = json.load(open(prov))
        d["assets"] = [a for a in d["assets"] if a["id"] != job["id"]]
        d["assets"].append(entry)
        json.dump(d, open(prov, "w"), indent=2, ensure_ascii=False)
    return job["id"], "ok %s Δ=%s" % (entry.get("actual_size", ""), entry["credits_delta"])

def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("jobs"); ap.add_argument("--only"); ap.add_argument("--parallel", type=int, default=3)
    ap.add_argument("--dry-run", action="store_true"); ap.add_argument("--force", action="store_true")
    a = ap.parse_args()
    jobs = json.load(open(a.jobs))["jobs"]
    if a.only:
        wanted = set(a.only.split(","))
        jobs = [j for j in jobs if j["id"] in wanted]
    print("credits at start:", credits(), file=sys.stderr)
    with concurrent.futures.ThreadPoolExecutor(max_workers=a.parallel) as ex:
        for jid, result in ex.map(lambda j: run_job(j, a.force, a.dry_run), jobs):
            print(jid, "→", result, flush=True)
    print("credits at end:", credits(), file=sys.stderr)

if __name__ == "__main__":
    main()
