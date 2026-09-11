#!/usr/bin/env python3
"""Regenerate _workspace/current/production/cycle-ledger.json from qa/defect-register.md (RFC-C6-002).
Counts per cycle prefix C1..C7: total / closed / open S1 / open S2 / open S3+ / open-rfc. Never edits the register."""
import re, json, os, datetime, collections
R=os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
reg=open(f"{R}/_workspace/current/qa/defect-register.md",encoding="utf-8").read()
rows=[]
seen=set()
dropped=[]
for line in reg.splitlines():
    m=re.match(r"^\| \**(C[1-7])-F(\d+)\**[^|]*\|", line)
    if not m: continue
    cells=[c.strip() for c in line.strip().strip('|').split('|')]
    fid=f"{m.group(1)}-F{m.group(2)}"
    if fid in seen: continue
    seen.add(fid)
    sev=next((mm.group(1) for c in cells[1:3] for mm in [re.search(r'\b(S[1-4])\b', c.replace('*',''))] if mm), None)
    # C7-F46: the status cell is the first cell that STARTS with a status token; inside it the LAST token wins
    # (a cell may narrate a transition such as "open (...) → **closed** (...)").
    st=None
    for c in cells:
        t=c.replace('*','').strip()
        if re.match(r'^(open-rfc|open|closed|rejected)\b', t):
            toks=re.findall(r'\b(open-rfc|open|closed|rejected)\b', t); st=toks[-1]; break
    if sev and st: rows.append((m.group(1),sev,st))
    else: import sys; print(f"WARN unparsed row: {fid} sev={sev} st={st}", file=sys.stderr); dropped.append(fid)
agg=collections.OrderedDict((f"C{i}",{"total":0,"closed":0,"open_S1":0,"open_S2":0,"open_S3plus":0,"open_rfc":0}) for i in range(1,8))
for cyc,sev,st in rows:
    a=agg[cyc]; a["total"]+=1
    if st.startswith("closed"): a["closed"]+=1
    elif st.startswith("open-rfc"): a["open_rfc"]+=1
    elif st.startswith("open"):
        a["open_S1" if sev=="S1" else "open_S2" if sev=="S2" else "open_S3plus"]+=1
focus={"C1":"시장·범위","C2":"인과·세계관","C3":"캠페인·시간","C4":"상호작용·Unity","C5":"상품·생산·회귀","C6":"통합 게임 초안 v1","C7":"Codex Unity 핸드오프"}
review={"C1":"qa/c1-review.md","C2":"qa/c2-review.md","C3":"qa/c3-review.md","C4":"qa/c4-review.md","C5":"qa/c5-review.md","C6":"qa/c6-review.md","C7":"qa/c6-review.md"}
out={"schemaVersion":2,"generated_at":datetime.date.today().isoformat(),"source":"qa/defect-register.md (single source, RFC-C6-002)","generator":"scripts/regen-cycle-ledger.py","cycles":[]}
for c,a in agg.items():
    status = "reviewed-and-revised" if a["open_S1"]==0 and a["open_S2"]==0 and a["total"]>0 else ("fix-in-progress" if a["total"]>0 else "reviewed(parent session, ids not in register)")
    out["cycles"].append({"id":c,"focus":focus[c],"review":review[c],"status":status,**a})
p=f"{R}/_workspace/current/production/cycle-ledger.json"
json.dump(out,open(p,"w",encoding="utf-8"),ensure_ascii=False,indent=2)
print(json.dumps({c:{k:v for k,v in a.items()} for c,a in agg.items()},ensure_ascii=False))
import sys; print(f"parsed {len(rows)} rows, dropped {len(dropped)}: {dropped}", file=sys.stderr)
