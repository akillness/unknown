#!/usr/bin/env node
// Read-only contract validator. This never validates human playtime or Unity runtime.
import fs from 'node:fs';
import path from 'node:path';
import { fileURLToPath } from 'node:url';
import { createHash } from 'node:crypto';
const ROOT = path.resolve(path.dirname(fileURLToPath(import.meta.url)), '../../../..');
const CUR = path.join(ROOT, '_workspace/current');
const load = p => JSON.parse(fs.readFileSync(path.join(CUR, p), 'utf8'));
// RFC C7-F43 (2026-09-10): QA may merge C6+C7 into qa/c6-review.md when its head declares C7 coverage.
function mergedReview(cur, rel) {
  if (rel !== 'qa/c7-review.md') return false;
  const alt = path.join(cur, 'qa/c6-review.md');
  return fs.existsSync(alt) && /C7/.test(fs.readFileSync(alt, 'utf8').slice(0, 4000));
}
export function validateCampaign(c) {
  const errors = [], checks = [];
  const check = (name, value) => { checks.push({ name, pass: !!value }); if (!value) errors.push(name); };
  check('schema v1', c.schemaVersion === 1);
  check('nine stages', Array.isArray(c.stages) && c.stages.length === 9);
  if (!Array.isArray(c.stages)) return { checks, errors };
  const beats = c.stages.flatMap(s => s.beats ?? []), ids = new Set(), clueIds = new Set();
  check('33 required beats', beats.length === 33);
  check('480 design minutes', c.designMinutes === 480 && c.stages.reduce((n,s) => n + s.minutes, 0) === 480);
  check('at least 20 puzzle beats', beats.filter(b => b.kind === 'puzzle').length >= 20);
  check('unknown full-play median when sample empty', c.humanPlaytests?.length > 0 || c.observedMedianMinutes === null);
  const tools = new Set(['circuit','reader','alignment','routing','corrosion','seal']);
  const media = new Set(['plate','log','ledger']);
  const learning = new Map([...tools].map(t => [t, new Set()]));
  for (const s of c.stages) {
    check(`${s.id}: beat time sum`, s.beats.reduce((n,b)=>n+b.minutes,0) === s.minutes);
    for (const b of s.beats) {
      check(`${b.id}: unique ID`, !ids.has(b.id));
      check(`${b.id}: dependencies already reachable`, b.prerequisites.every(id => ids.has(id)));
      ids.add(b.id);
      check(`${b.id}: scenario bounds`, 0 < b.fastMinutes && b.fastMinutes < b.minutes && b.minutes < b.deliberateMinutes);
      check(`${b.id}: narrative/action contract`, ['objective','inference','action','consequence','completion','recovery','checkpoint'].every(k => typeof b[k] === 'string' && b[k].trim().length > 0));
      check(`${b.id}: three nonempty hints`, b.hints?.length === 3 && b.hints.every(h => typeof h === 'string' && h.trim()));
      check(`${b.id}: valid tools`, b.tools.every(t => tools.has(t)));
      check(`${b.id}: activity time budget`, b.activityBudget && Math.abs(Object.values(b.activityBudget).reduce((n,v)=>n+v,0)-b.minutes)<0.001);
      check(`${b.id}: three concrete subtasks`, Array.isArray(b.subtasks) && b.subtasks.length >= 3 && b.subtasks.length <= 5);
      check(`${b.id}: explicit proofRequired`, typeof b.proofRequired === 'boolean');
      check(`${b.id}: estimate rationale`, typeof b.authorEstimateBasis === 'string' && b.authorEstimateBasis.length > 10);
      for (const cl of b.clues) {
        check(`${cl.id}: unique reference`, !clueIds.has(cl.id)); clueIds.add(cl.id);
        check(`${cl.id}: source provenance`, media.has(cl.sourceType) && typeof cl.originId === 'string' && cl.originId.length > 0);
      }
      if (b.proofRequired) check(`${b.id}: two origins and media`, new Set(b.clues.map(v=>v.originId)).size >= 2 && new Set(b.clues.map(v=>v.sourceType)).size >= 2);
      for (const t of b.toolTeaching ?? []) if (learning.has(t.tool)) learning.get(t.tool).add(t.mode);
    }
  }
  for (const [tool,modes] of learning) check(`${tool}: guided and transfer task`, modes.has('guided') && modes.has('unguided'));
  return { checks, errors, summary: { stages:c.stages.length, beats:beats.length, puzzles:beats.filter(b=>b.kind==='puzzle').length, clues:clueIds.size, designMinutes:c.designMinutes, fastMinutes:beats.reduce((n,b)=>n+b.fastMinutes,0), deliberateMinutes:beats.reduce((n,b)=>n+b.deliberateMinutes,0), observedMedianMinutes:c.observedMedianMinutes, humanPlaytests:c.humanPlaytests.length } };
}
const c = load('planning/campaign.json');
if (process.argv.includes('--self-test')) {
  const mutations = [
    ['time mismatch', x=>{x.stages[0].minutes++;}],
    ['forward dependency', x=>{x.stages[0].beats[0].prerequisites=['not-authored'];}],
    ['missing hint', x=>{x.stages[0].beats[0].hints.pop();}],
    ['unmeasured median claim', x=>{x.observedMedianMinutes=480;}],
    ['missing provenance', x=>{x.stages[0].beats[0].clues[0].originId='';}],
    ['missing activity budget', x=>{delete x.stages[0].beats[0].activityBudget;}],
    ['invalid tool', x=>{x.stages[0].beats[0].tools.push('magic');}],
    ['missing transfer', x=>{for(const s of x.stages)for(const b of s.beats)b.toolTeaching=[];}],
  ];
  const results = [{name:'current positive',pass:validateCampaign(c).errors.length===0}, ...mutations.map(([name,fn])=>{const x=structuredClone(c);fn(x);return{name,pass:validateCampaign(x).errors.length>0};})];
  console.log(JSON.stringify({scope:'validator positive/negative regression, not gameplay',results,passed:results.filter(r=>r.pass).length,total:results.length},null,2));
  process.exit(results.every(r=>r.pass)?0:1);
}
const out = validateCampaign(c), e = load('product/economics.json'), p = load('production/production-estimate.json');
const keys = ['designDays','writingDays','implementationDays','artDays','presentationDays','qaDays'];
const personDays = p.rows.reduce((n,r)=>n+keys.reduce((s,k)=>s+r[k],0),0)+p.coordinationDays+p.launchAdminDays;
const net = (price,regional=1,discount=e.launchDiscount) => price*(1-discount)/(1+e.vatKR)*regional*(1-e.refundRate-e.chargebackRate)*e.developerShareAssumption-e.perUnitReserve;
const economy = e.prices.map(price=>({price,launchPrice:price*(1-e.launchDiscount),netAssumption:net(price),breakEvenCash:e.cashBudgets.map(cash=>({cash,units:Math.ceil(cash/net(price))}))}));
if (economy.some(v=>v.launchPrice<e.launchFloor || v.netAssumption<=0)) out.errors.push('launch floor or unit margin');
if(e.shareVerified!==false)out.errors.push('unverified revenue share must not be marked verified');
if(Math.abs(p.rows.reduce((n,r)=>n+r.artDays,0)-63)>.001)out.errors.push('art person-days mismatch with 63-day asset library');
const ledgerPath=path.join(CUR,'production/cycle-ledger.json');
const ledger=JSON.parse(fs.readFileSync(ledgerPath,'utf8'));
for(const cy of ledger.cycles) {
  const n=cy.id.toLowerCase();
  for(const relative of [`qa/${n}-review.md`,`production/cycles/${n}-development.md`])if(!fs.existsSync(path.join(CUR,relative)) && !mergedReview(CUR,relative))out.errors.push(`missing cycle artifact ${relative}`);
}
const watched=['planning/campaign.json','product/economics.json','production/production-estimate.json','systems/game-ui-contract.json','production/cycle-ledger.json'];
const files=watched.map(rel=>{const bytes=fs.readFileSync(path.join(CUR,rel));return{path:rel,bytes:bytes.length,sha256:createHash('sha256').update(bytes).digest('hex')};});
console.log(JSON.stringify({status:out.errors.length?'FIX':'SPEC-PASS',runtimeStatus:'NOT-MEASURED',scope:'document/schema/arithmetic only; not whole-game reachability, market fit or player time',observedAt:new Date().toISOString(),checks:out.checks.length,passed:out.checks.filter(v=>v.pass).length,errors:out.errors,summary:out.summary,economy,production:{personDays,withContingency:Math.ceil(personDays*(1+p.contingencyRate)),laborOpportunityCost:Math.ceil(personDays*(1+p.contingencyRate))*p.blendedLaborCostPerDay},files},null,2));
process.exitCode=out.errors.length?1:0;
