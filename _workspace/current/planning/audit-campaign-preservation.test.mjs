import test from 'node:test';
import assert from 'node:assert/strict';
import { auditPreservation } from './audit-campaign-preservation.mjs';

const clue = (id, sourceType, originId, copiedFrom = null) => ({ id, sourceType, originId, copiedFrom });
const campaign = (...beats) => ({ stages: [{ id: 'test', beats }] });
const beat = (id, ...clues) => ({ id, proofRequired: true, clues });
const table = (first, second, preservation = '첫 판독 자동 사본') => `### 5.1 K 표 — fixture\n| K1 | fixture claim | t0-b3 | true | ${first} | ${second} | ${preservation} | A |\n### 5.2 불변식\n`;

test('a minimum independent pair cannot pass an absent mandatory-claim audit', () => {
  const report = auditPreservation(campaign(beat('a', clue('a-c1', 'plate', 'plate'), clue('a-c2', 'ledger', 'ledger'))), '');
  assert.equal(report.minimalPairs[0].status, 'PASS');
  assert.equal(report.verdict, 'blocked');
  assert.equal(report.claimCoverage.status, 'unknown');
  assert.ok(report.claimCoverage.missingClaims.includes('K1'));
});

test('reconfirmation of identical roots is not a second alternative root pair', () => {
  const report = auditPreservation(campaign(
    beat('a', clue('a-c1', 'plate', 'plate'), clue('a-c2', 'ledger', 'ledger')),
    beat('b', clue('b-c1', 'plate', 'plate'), clue('b-c2', 'ledger', 'ledger')),
  ), table('`a-c1` × `a-c2`', '`b-c1` × `b-c2`'));
  assert.equal(report.claims[0].distinctCandidateRootPairs, 1);
  assert.equal(report.claims[0].repeatedRootPair, true);
  assert.equal(report.claims[0].p1.status, 'unknown');
  assert.equal(report.verdict, 'blocked');
});

test('two structural candidates and a one-member preservation note cannot prove P1 or P2', () => {
  const report = auditPreservation(campaign(
    beat('a', clue('a-c1', 'plate', 'plate-a'), clue('a-c2', 'ledger', 'ledger-a')),
    beat('b', clue('b-c1', 'log', 'log-b'), clue('b-c2', 'ledger', 'ledger-b')),
  ), table('`a-c1` × `a-c2`', '`b-c1` × `b-c2`', 'plate-a 원본 잠금 보관'));
  assert.equal(report.claims[0].distinctCandidateRootPairs, 2);
  assert.equal(report.claims[0].p1.status, 'unknown');
  assert.equal(report.claims[0].p2.status, 'unknown');
  assert.equal(report.verdict, 'blocked');
});

test('a beat-only reconfirmation reference must not auto-select that beat first pair', () => {
  const report = auditPreservation(campaign(
    beat('a', clue('a-c1', 'plate', 'plate'), clue('a-c2', 'ledger', 'ledger')),
    beat('b', clue('b-c1', 'log', 'log'), clue('b-c2', 'ledger', 'other')),
  ), table('`a-c1` × `a-c2`', '`b` 재확인'));
  assert.equal(report.claims[0].candidatePaths[1].status, 'unknown');
  assert.equal(report.claims[0].distinctCandidateRootPairs, 1);
});

test('a multi-hop copy cannot form an independent pair with its original', () => {
  const report = auditPreservation(campaign(
    beat('a', clue('a-c1', 'plate', 'original'), clue('a-c2', 'ledger', 'copy', 'middle')),
    { id: 'catalog', clues: [clue('catalog-c1', 'ledger', 'middle', 'original')] },
  ), table('`a-c1` × `a-c2`', '없음'));
  assert.equal(report.minimalPairs[0].status, 'FAIL');
  assert.equal(report.claims[0].candidatePaths[0].status, 'FAIL');
  assert.equal(report.verdict, 'FAIL');
});

test('unmapped, conflicting, or cyclic lineage never invents independent roots', () => {
  const cases = [
    [clue('a-c1', 'plate', 'copy', 'missing'), clue('a-c2', 'ledger', 'ledger')],
    [clue('a-c1', 'plate', 'same'), clue('a-c2', 'ledger', 'same', 'ledger'), clue('a-c3', 'ledger', 'ledger')],
    [clue('a-c1', 'plate', 'one', 'two'), clue('a-c2', 'ledger', 'two', 'one')],
  ];
  for (const cs of cases) {
    const report = auditPreservation(campaign(beat('a', ...cs)), table('`a-c1` × `a-c2`', '없음'));
    assert.equal(report.minimalPairs[0].status, 'unknown');
    assert.equal(report.claims[0].candidatePaths[0].status, 'unknown');
    assert.equal(report.verdict, 'FAIL');
  }
});

test('different roots on the same medium still cannot satisfy minimum proof', () => {
  const report = auditPreservation(campaign(
    beat('a', clue('a-c1', 'plate', 'plate-a'), clue('a-c2', 'plate', 'plate-b')),
  ), table('`a-c1` × `a-c2`', '없음'));
  assert.equal(report.minimalPairs[0].status, 'FAIL');
  assert.equal(report.claims[0].candidatePaths[0].status, 'FAIL');
});
