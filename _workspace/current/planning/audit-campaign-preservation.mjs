#!/usr/bin/env node
// Separate from C-07: candidate structure is not mandatory-claim or preservation proof.
// Read-only CLI: [campaign.json] [continuity.md]; exits 1 invalid, 2 error, 3 blocked.
import { readFileSync } from 'node:fs';
import { createHash } from 'node:crypto';
import { dirname, join, resolve } from 'node:path';
import { fileURLToPath } from 'node:url';

const sourceTypes = new Set(['plate', 'log', 'ledger']);
const nonempty = (s) => typeof s === 'string' && s.trim().length > 0;

export function auditPreservation(campaign, continuity) {
  if (!Array.isArray(campaign.stages) || campaign.stages.length === 0)
    throw new Error('Missing campaign stages; no coverage can be inferred');
  const beats = campaign.stages.flatMap((s) => {
    if (!Array.isArray(s.beats)) throw new Error('Missing stage beats');
    return s.beats;
  });
  const clues = beats.flatMap((b) => b.clues ?? []);
  const byId = new Map();
  const parents = new Map();
  const issues = [];
  for (const c of clues) {
    if (!nonempty(c.id) || byId.has(c.id)) issues.push(`Invalid/duplicate clue id: ${c.id}`);
    byId.set(c.id, c);
    if (!nonempty(c.originId) || !sourceTypes.has(c.sourceType))
      issues.push(`Missing origin or invalid sourceType: ${c.id}`);
    // Explicit null is authored root evidence; absent copiedFrom is not a new root.
    if (c.copiedFrom !== null && !nonempty(c.copiedFrom))
      issues.push(`Missing/invalid copiedFrom: ${c.id}`);
    if (parents.has(c.originId) && parents.get(c.originId) !== c.copiedFrom)
      issues.push(`Conflicting lineage: ${c.originId}`);
    parents.set(c.originId, c.copiedFrom);
  }
  function root(id) {
    const seen = new Set();
    while (nonempty(id) && parents.has(id) && !seen.has(id)) {
      seen.add(id);
      const parent = parents.get(id);
      if (parent === null) return id;
      id = parent;
    }
    return null;
  }
  for (const c of clues) if (root(c.originId) === null) issues.push(`Unresolved lineage: ${c.id}`);
  const lineageValid = issues.length === 0;
  function pair(left, right) {
    if (!left || !right || left.id === right.id) return null;
    const roots = [root(left.originId), root(right.originId)];
    if (!lineageValid || roots.includes(null) || roots[0] === roots[1] || left.sourceType === right.sourceType)
      return null;
    return { clueIds: [left.id, right.id], sourceTypes: [left.sourceType, right.sourceType], roots };
  }
  const minimalPairs = beats.filter((b) => b.proofRequired === true).map((b) => {
    const candidates = [];
    const cs = b.clues ?? [];
    for (let i = 0; i < cs.length; i++) {
      for (let j = i + 1; j < cs.length; j++) {
        const p = pair(cs[i], cs[j]);
        if (p) candidates.push(p);
      }
    }
    return { beatId: b.id, status: !lineageValid ? 'unknown' : candidates.length ? 'PASS' : 'FAIL', candidates };
  });

  // Only the explicit table is consumed, never narrative recovery prose or a beat's first pair.
  const table = continuity.match(/^### 5\.1 K 표[^\n]*\n([\s\S]*?)(?=^### |^## |$(?![\s\S]))/m)?.[1] ?? '';
  const rows = table.split('\n').filter((line) => /^\| K\d+ \|/.test(line));
  const claims = rows.map((line) => {
    const cells = line.split('|').slice(1, -1).map((s) => s.trim());
    const [id, claim] = cells;
    const candidatePaths = [cells[4], cells[5]].map((cell = '') => {
      const ids = [...cell.matchAll(/`([^`]+-c\d+)`/g)].map((m) => m[1]);
      if (ids.length !== 2) return { status: 'unknown', clueIds: ids, reason: 'No explicit two-clue mapping; beat-only references are not substituted' };
      const p = pair(byId.get(ids[0]), byId.get(ids[1]));
      return p ? { status: 'candidate', ...p } : { status: lineageValid ? 'FAIL' : 'unknown', clueIds: ids, reason: 'Unresolved clue or non-independent pair' };
    });
    const candidates = candidatePaths.filter((p) => p.status === 'candidate');
    const rootPairs = new Set(candidates.map((p) => JSON.stringify([...p.roots].sort())));
    return {
      id, claim, candidatePaths,
      distinctCandidateRootPairs: rootPairs.size,
      repeatedRootPair: candidates.length > rootPairs.size,
      p1: {
        status: 'unknown',
        reason: 'Candidate pairs do not prove two alternative paths to this same claim; complete authored claim/path conditions and reachability evidence are missing',
      },
      p2: {
        status: 'unknown',
        authoredPreservationNote: cells[6] ?? null,
        reason: 'No per-member proof of a fully indestructible pair across all player actions, including access before first read; a copy or one surviving member is insufficient',
      },
    };
  });
  const expectedClaims = Array.from({ length: 10 }, (_, i) => `K${i + 1}`);
  const missingClaims = expectedClaims.filter((id) => !claims.some((c) => c.id === id));
  const duplicateClaims = claims.filter((c, i) => claims.findIndex((other) => other.id === c.id) !== i).map((c) => c.id);
  const invalid = issues.length > 0 || duplicateClaims.length > 0 || minimalPairs.some((b) => b.status === 'FAIL') ||
    claims.some((c) => c.candidatePaths.some((p) => p.status === 'FAIL'));
  return {
    verdict: invalid ? 'FAIL' : 'blocked',
    minimalPairScope: 'C-07 rule only: one independent pair per proofRequired beat; not P1/P2',
    minimalPairs, lineageIssues: issues,
    claimCoverage: { source: 'synopsis/continuity.md §5.1 K1–K10', missingClaims, duplicateClaims, status: missingClaims.length || duplicateClaims.length ? 'unknown' : 'enumerated-only' },
    claims,
    blockers: [
      ...(minimalPairs.length ? [] : ['No proofRequired beats; minimum coverage is unknown']),
      ...(missingClaims.length ? ['Mandatory claim table missing/incomplete; no vacuous PASS'] : []),
      'P1: complete authored alternative-path/claim attribution and reachability evidence absent',
      'P2: both-member preservation/action coverage evidence absent',
    ],
    owners: { p1: ['game-worldview-architect', 'game-planner', 'game-synopsis-writer'], p2: ['game-worldview-architect', 'game-systems-designer', 'game-qa'] },
    notMeasured: ['Full G1', 'Runtime preservation/reachability', 'Human discovery/comprehension', 'Production gate'],
  };
}

if (process.argv[1] && resolve(process.argv[1]) === fileURLToPath(import.meta.url)) {
  try {
    const here = dirname(fileURLToPath(import.meta.url));
    const paths = [resolve(process.argv[2] ?? join(here, 'campaign.json')), resolve(process.argv[3] ?? join(here, '..', 'synopsis', 'continuity.md'))];
    const inputs = paths.map((path) => {
      const raw = readFileSync(path);
      return { path, raw, bytes: raw.length, sha256: createHash('sha256').update(raw).digest('hex') };
    });
    const report = auditPreservation(JSON.parse(inputs[0].raw.toString('utf8')), inputs[1].raw.toString('utf8'));
    process.stdout.write(JSON.stringify({ validator: 'planning/audit-campaign-preservation.mjs', inputs: inputs.map(({ raw, ...metadata }) => metadata), ...report }, null, 2) + '\n');
    process.exitCode = report.verdict === 'FAIL' ? 1 : 3;
  } catch (error) {
    process.stdout.write(JSON.stringify({ validator: 'planning/audit-campaign-preservation.mjs', error: error.message }) + '\n');
    process.exitCode = 2;
  }
}
