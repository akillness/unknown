#!/usr/bin/env python3
"""qa-loop-report.py <run-dir> <loop-dir> [--skip <reason>]

Turns one qa-loop.sh run folder into: <run>/summary.json, <run>/triage.md, and the tracked ledger under
_workspace/current/qa/loop/ (ledger.md + ledger.jsonl appended, latest.json + latest-triage.md replaced).
Deterministic: NUnit XML + build log + inventory only. No model, no git writes, no gate promotion."""
import sys, os, re, json, hashlib, datetime, xml.etree.ElementTree as ET

run, loop = sys.argv[1], sys.argv[2]
skip = sys.argv[4] if len(sys.argv) > 4 and sys.argv[3] == "--skip" else None
run_id = os.path.basename(run.rstrip("/"))
os.makedirs(loop, exist_ok=True)
now = datetime.datetime.now(datetime.timezone.utc).strftime("%Y-%m-%dT%H:%M:%SZ")

def suite(name):
    p = os.path.join(run, name + ".xml")
    if not os.path.exists(p): return {"present": False, "total": 0, "passed": 0, "failed": 0, "skipped": 0, "failures": []}
    r = ET.parse(p).getroot(); out = {"present": True, "failures": []}
    for k in ("total", "passed", "failed", "skipped"): out[k] = int(r.get(k) or 0)
    for c in r.iter("test-case"):
        if c.get("result") == "Passed": continue
        m = c.find("failure/message"); st = c.find("failure/stack-trace")
        frame = ""
        if st is not None and st.text:
            hit = re.search(r"(Assets/_Project/[^\s:]+\.cs):(\d+)", st.text); frame = f"{hit.group(1)}:{hit.group(2)}" if hit else ""
        out["failures"].append({"name": c.get("fullname"), "result": c.get("result"), "message": ((m.text if m is not None else "") or "")[:400].strip(), "frame": frame})
    return out

def step_exits():
    d = {}
    p = os.path.join(run, "steps.txt")
    if os.path.exists(p):
        for line in open(p):
            m = re.match(r"(\w+) exit=(\d+) s=(\d+)", line.strip())
            if m: d[m.group(1)] = {"exit": int(m.group(2)), "seconds": int(m.group(3))}
    return d

def build():
    out = {"ok": False, "bytes": None, "files": None, "digest": None}
    p = os.path.join(run, "build.txt")
    if os.path.exists(p):
        m = re.search(r"T0_MAC_BUILD (\w+) bytes=(\d+)", open(p).read())
        if m: out["ok"] = m.group(1) == "Succeeded"; out["bytes"] = int(m.group(2))
    app = os.path.join(os.path.dirname(os.path.dirname(run)), "qa-loop", "Unknown.app")
    if out["ok"] and os.path.isdir(app):
        entries = []
        for dp, dn, fn in os.walk(app):
            for f in fn:
                fp = os.path.join(dp, f); entries.append((os.path.relpath(fp, app), os.path.getsize(fp), hashlib.sha256(open(fp, "rb").read()).hexdigest()))
        entries.sort(); out["files"] = len(entries)
        out["digest"] = hashlib.sha256("\n".join(f"{a} {b} {c}" for a, b, c in entries).encode()).hexdigest()
    return out

def tree(name):
    p = os.path.join(run, name)
    return (open(p).read().split() + ["?", "?", "?"])[:3] if os.path.exists(p) else ["?", "?", "?"]
git, git_end = tree("git.txt"), tree("git-end.txt")
drift = (not skip) and git_end[0] != "?" and (git_end[0] != git[0] or git_end[2] != git[2])
summary = {"runId": run_id, "at": now, "git": {"head": git[0], "dirtyPaths": int(git[1]) if git[1].isdigit() else None, "treeChangedDuringRun": drift}, "skip": skip}
if not skip:
    summary["steps"] = step_exits()
    summary["editmode"], summary["playmode"], summary["boot"] = suite("editmode"), suite("playmode"), suite("boot")
    summary["build"] = build()
    fails = [f for s in ("editmode", "playmode", "boot") for f in summary[s]["failures"] if f["result"] != "Skipped"]
    # The PlayMode run of T0BootSceneTests without --t0-save-dir is a conditional Ignore by design; the isolated boot run covers it.
    summary["failingTests"] = sorted(f["name"] for f in fails)
    summary["verdict"] = "GREEN" if not fails and summary["build"]["ok"] and all(summary[s]["present"] for s in ("editmode", "playmode", "boot")) else "RED"
    if drift: summary["verdict"] += "·MIXED"   # the working tree changed between the first and last step; re-measure on a quiet tree
else:
    summary["verdict"] = "SKIPPED"

# --- delta vs previous cycle -----------------------------------------------------------------------------------
prev_path = os.path.join(loop, "latest.json")
prev = json.load(open(prev_path)) if os.path.exists(prev_path) else None
if prev and not skip and not prev.get("skip"):
    pf, cf = set(prev.get("failingTests", [])), set(summary["failingTests"])
    summary["delta"] = {"previousRun": prev["runId"], "newlyFailing": sorted(cf - pf), "newlyPassing": sorted(pf - cf),
                        "totalTests": sum(summary[s]["total"] for s in ("editmode", "playmode", "boot")) - sum(prev[s]["total"] for s in ("editmode", "playmode", "boot") if s in prev),
                        "buildBytes": (summary["build"]["bytes"] or 0) - (prev.get("build", {}).get("bytes") or 0),
                        "digestChanged": summary["build"]["digest"] != prev.get("build", {}).get("digest"),
                        "sameHead": summary["git"]["head"] == prev.get("git", {}).get("head")}
else:
    summary["delta"] = None
json.dump(summary, open(os.path.join(run, "summary.json"), "w"), indent=1, ensure_ascii=False)

# --- triage --------------------------------------------------------------------------------------------------
def lane(frame):
    if not frame: return "qa (no project frame - read the log)"
    if "/Tests/" in frame: return "qa · test scaffolding"
    if "/Sim/" in frame or "/Save/" in frame or "/Data/" in frame: return "systems (sim/save/data)"
    if "/UI/" in frame: return "systems + presentation (UI)"
    if "/App/" in frame or "/Input/" in frame: return "systems (session/input)"
    if "/Editor/" in frame: return "systems (editor tooling)"
    return "systems"
lines = [f"# QA loop triage · {run_id} · {summary['verdict']}", "", f"- at {now} · HEAD `{summary['git']['head']}` · dirty paths {summary['git']['dirtyPaths']}"]
if drift: lines.append("- **MIXED**: the working tree changed while the cycle ran (a session was editing); the suites and the build did not all see the same tree. Re-run on a quiet tree before trusting this row.")
if skip:
    lines.append(f"- skipped: **{skip}** (no measurement this cycle)")
else:
    for s in ("editmode", "playmode", "boot"):
        x = summary[s]; lines.append(f"- {s}: {x['passed']}/{x['total']} passed · {x['failed']} failed · {x['skipped']} skipped · exit {summary['steps'].get(s, {}).get('exit')} · {summary['steps'].get(s, {}).get('seconds')}s")
    b = summary["build"]; lines.append(f"- build (Builds/qa-loop/Unknown.app): {'OK' if b['ok'] else 'FAILED'} · {b['bytes']} B · {b['files']} files · digest `{(b['digest'] or '')[:12]}…` · {summary['steps'].get('build', {}).get('seconds')}s")
    d = summary["delta"]
    if d:
        lines.append(f"- vs {d['previousRun']}: newly failing {len(d['newlyFailing'])} · newly passing {len(d['newlyPassing'])} · tests {d['totalTests']:+d} · build bytes {d['buildBytes']:+d} · digest {'changed' if d['digestChanged'] else 'same'} · {'same HEAD' if d['sameHead'] else 'HEAD moved'}")
        if d["digestChanged"] and d["sameHead"] and summary["git"]["dirtyPaths"] == prev["git"].get("dirtyPaths"): lines.append("  - digest changed on an unchanged tree → build output is not byte-deterministic; not a regression by itself")
    if fails:
        lines += ["", "## Failing tests → owner lane", ""]
        for f in fails: lines += [f"- `{f['name']}` — {f['result']}", f"  - frame: `{f['frame'] or '-'}` → **{lane(f['frame'])}**", f"  - message: {f['message'].splitlines()[0] if f['message'] else '-'}"]
        lines += ["", "Next: reproduce with the same `-testFilter`, fix at the frame, re-run the filter, then the full receipts. Record in `qa/defect-register.md` if it survives one session."]
    elif summary["verdict"].startswith("GREEN"): lines += ["", "No failing tests; nothing to triage. Improvement queue: `_workspace/current/handoff/m26-results-and-improvement-plan.md`."]
    else: lines += ["", "RED without failing tests → a suite or the build did not produce results; read the step logs in the run folder."]
lines += ["", f"Run folder: `unity/Unknown/Builds/qa-loop/{run_id}/` (untracked; last 8 kept)."]
open(os.path.join(run, "triage.md"), "w").write("\n".join(lines) + "\n")
open(os.path.join(loop, "latest-triage.md"), "w").write("\n".join(lines) + "\n")
json.dump(summary, open(prev_path, "w"), indent=1, ensure_ascii=False)

# --- ledger --------------------------------------------------------------------------------------------------
led = os.path.join(loop, "ledger.md")
if not os.path.exists(led):
    open(led, "w").write("---\nupdated: 2026-09-18\ncycle: 20260918-content-update-m26\nstatus: current\nsupersedes: null\nowner: game-qa\n---\n\n# QA loop ledger (3 h cadence · `scripts/qa-loop.sh`)\n\nOne row per cycle, appended by `scripts/qa-loop-report.py`. Verdict GREEN = all suites present, 0 failures, build OK. SKIPPED rows carry the reason. Digest is the QA build (`Builds/qa-loop/Unknown.app`), not the release artifact.\n\n| run (UTC) | HEAD | dirty | editmode | playmode | boot | build | digest | Δ vs prev | verdict |\n|---|---|---|---|---|---|---|---|---|---|\n")
def cell(s):
    if skip: return "-"
    x = summary[s]; return f"{x['passed']}/{x['total']}" + (f" ({x['failed']}F)" if x["failed"] else "") + (f" ({x['skipped']}S)" if x["skipped"] else "")
if skip: bcell, dg, dl = "-", "-", "-"
else:
    b = summary["build"]; bcell = f"{'OK' if b['ok'] else 'FAIL'} {b['bytes'] or 0:,}B"; dg = f"`{(b['digest'] or '-')[:8]}`"
    d = summary["delta"]; dl = "first" if not d else f"+{len(d['newlyFailing'])}F/−{len(d['newlyPassing'])}F · {d['buildBytes']:+,}B"
verdict = summary["verdict"] + (f" ({skip})" if skip else "")
open(led, "a").write(f"| {run_id} | `{summary['git']['head']}` | {summary['git']['dirtyPaths']} | {cell('editmode')} | {cell('playmode')} | {cell('boot')} | {bcell} | {dg} | {dl} | **{verdict}** |\n")
open(os.path.join(loop, "ledger.jsonl"), "a").write(json.dumps({k: summary[k] for k in summary if k not in ("editmode", "playmode", "boot")} | {"counts": {s: {k: summary[s][k] for k in ("total", "passed", "failed", "skipped")} for s in ("editmode", "playmode", "boot")} if not skip else None}, ensure_ascii=False) + "\n")
print(f"{run_id} {verdict}")
