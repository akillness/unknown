#!/usr/bin/env node
// emit-evidence-graph.mjs — planning 레인 소유 증거 그래프 생성기 (TRACE-RPG 번안, C7)
//
// 목적: `planning/campaign.json`(정본)에서 typed 노드/엣지 그래프를 **파생**한다.
//       TRACE-RPG(neural_symbolic_in_game)의 kg-ontology 방식을 따라, 검증은 별도
//       파일(validate-evidence-graph.mjs)이 그래프+원본을 대조해서 수행한다.
//       이 생성기는 campaign.json / validate-campaign.mjs 를 대체하지 않는다.
//
// 입력:  planning/campaign.json               (정본, 읽기 전용)
//        planning/evidence-graph-overlay.json (선택 저작 오버레이 [TARGET] — 반전 씨앗/회수)
// 출력:  planning/evidence-graph.json         (파생물 — 손으로 편집하지 않는다)
//
// 결정론: 노드/엣지 정렬 + 재귀 키 정렬. 동일 입력 → byte-동일 출력.
//         provenance 에 입력 sha256/bytes 를 기록한다 (TRACE-RPG C3 audit-linked).
// 의존성: Node ≥18 내장 모듈만. 실행: node _workspace/current/planning/emit-evidence-graph.mjs
// 종료:   0 = 생성 성공, 2 = 실행 오류. stderr 는 오류 시에만 쓴다.

import { readFileSync, writeFileSync, existsSync } from 'node:fs';
import { createHash } from 'node:crypto';
import { dirname, join, basename } from 'node:path';
import { fileURLToPath } from 'node:url';

const HERE = dirname(fileURLToPath(import.meta.url));
const CAMPAIGN_PATH = join(HERE, 'campaign.json');
const OVERLAY_PATH = join(HERE, 'evidence-graph-overlay.json');
const OUT_PATH = join(HERE, 'evidence-graph.json');

function sha256(buf) {
  return createHash('sha256').update(buf).digest('hex');
}

// ── 재귀 키 정렬 (결정론) ──────────────────────────────────────────────
function sortKeysDeep(v) {
  if (Array.isArray(v)) return v.map(sortKeysDeep);
  if (v && typeof v === 'object') {
    const out = {};
    for (const k of Object.keys(v).sort()) out[k] = sortKeysDeep(v[k]);
    return out;
  }
  return v;
}

try {
  // ── 입력 적재 + provenance ─────────────────────────────────────────
  const campaignBuf = readFileSync(CAMPAIGN_PATH);
  const campaign = JSON.parse(campaignBuf.toString('utf8'));
  const inputs = [
    { path: 'planning/' + basename(CAMPAIGN_PATH), sha256: sha256(campaignBuf), bytes: campaignBuf.length },
  ];
  let overlay = null;
  if (existsSync(OVERLAY_PATH)) {
    const overlayBuf = readFileSync(OVERLAY_PATH);
    overlay = JSON.parse(overlayBuf.toString('utf8'));
    inputs.push({ path: 'planning/' + basename(OVERLAY_PATH), sha256: sha256(overlayBuf), bytes: overlayBuf.length });
  }

  // ── 노드 ───────────────────────────────────────────────────────────
  const nodes = [];
  const edges = [];

  // stage(9) — order = campaign.json 등장 순서 (timeline §7-0 파생 순서와 동일)
  campaign.stages.forEach((s, si) => {
    nodes.push({
      type: 'stage',
      id: s.id,
      title: s.title,
      order: si + 1,
      minutes: s.minutes,
      zoneIds: [...s.zoneIds].sort(),
      beatCount: s.beats.length,
    });
  });

  // beat(33) — docIndex = campaign.json 전역 등장 순서 1..33
  //            (timeline.md §7-0 B# 정의와 같은 파생이지만, 인용 키는 campaign id 만 쓴다)
  let docIndex = 0;
  const beats = [];
  for (const s of campaign.stages) {
    for (const b of s.beats) {
      docIndex += 1;
      beats.push({ stageId: s.id, docIndex, ...b });
      nodes.push({
        type: 'beat',
        id: b.id,
        title: b.title,
        kind: b.kind,
        stageId: s.id,
        zoneId: b.zoneId,
        minutes: b.minutes,
        proofRequired: b.proofRequired,
        docIndex,
      });
      edges.push({ type: 'contains', from: s.id, to: b.id });
    }
  }

  // clue(73) + source(유니크 originId) + copied_from
  // [OBSERVED] campaign.json 의 clue.copiedFrom 값은 clue id 가 아니라 자료 카탈로그의
  // source id(originId)다 (예: c4-b1-c1 → tide-ledger-bureau). 따라서 copied_from 엣지는
  // clue → source 로 발행하고, source 간 계보(originId → copiedFrom)는 검증기가
  // validate-campaign.mjs C-04/C-05 와 같은 방식으로 파생한다.
  const sourceMap = new Map(); // originId → { sourceTypes:Set, clueCount }
  for (const b of beats) {
    for (const c of b.clues) {
      nodes.push({
        type: 'clue',
        id: c.id,
        sourceType: c.sourceType,
        originId: c.originId,
        copiedFrom: c.copiedFrom ?? null,
        beatId: b.id,
      });
      edges.push({ type: 'yields', from: b.id, to: c.id });
      edges.push({ type: 'from_source', from: c.id, to: c.originId });
      if (c.copiedFrom) edges.push({ type: 'copied_from', from: c.id, to: c.copiedFrom });
      if (!sourceMap.has(c.originId)) sourceMap.set(c.originId, { sourceTypes: new Set(), clueCount: 0 });
      const s = sourceMap.get(c.originId);
      s.sourceTypes.add(c.sourceType);
      s.clueCount += 1;
    }
  }
  for (const [originId, s] of sourceMap) {
    nodes.push({
      type: 'source',
      id: originId,
      sourceTypes: [...s.sourceTypes].sort(),
      clueCount: s.clueCount,
    });
  }

  // tool — 캠페인에 실제 등장하는 도구 id 만 (tools ∪ toolTeaching.tool)
  const toolIds = new Set();
  for (const b of beats) {
    for (const t of b.tools) toolIds.add(t);
    for (const tt of b.toolTeaching) toolIds.add(tt.tool);
  }
  for (const t of [...toolIds].sort()) nodes.push({ type: 'tool', id: t });

  // requires / uses_tool / teaches_tool
  for (const b of beats) {
    for (const p of b.prerequisites) edges.push({ type: 'requires', from: b.id, to: p });
    for (const t of b.tools) edges.push({ type: 'uses_tool', from: b.id, to: t });
    for (const tt of b.toolTeaching) edges.push({ type: 'teaches_tool', from: b.id, to: tt.tool, mode: tt.mode });
  }

  // ── 정렬 (결정론) ──────────────────────────────────────────────────
  const TYPE_ORDER = ['stage', 'beat', 'clue', 'source', 'tool'];
  const EDGE_ORDER = ['contains', 'yields', 'from_source', 'copied_from', 'requires', 'uses_tool', 'teaches_tool'];
  nodes.sort((a, b) =>
    TYPE_ORDER.indexOf(a.type) - TYPE_ORDER.indexOf(b.type) || a.id.localeCompare(b.id, 'en'));
  edges.sort((a, b) =>
    EDGE_ORDER.indexOf(a.type) - EDGE_ORDER.indexOf(b.type) ||
    a.from.localeCompare(b.from, 'en') || a.to.localeCompare(b.to, 'en'));

  // ── 카운트 ─────────────────────────────────────────────────────────
  const countBy = (arr, key) => {
    const m = {};
    for (const x of arr) m[x[key]] = (m[x[key]] ?? 0) + 1;
    return Object.fromEntries(Object.entries(m).sort(([a], [b]) => a.localeCompare(b, 'en')));
  };
  const counts = {
    nodes: { total: nodes.length, byType: countBy(nodes, 'type') },
    edges: { total: edges.length, byType: countBy(edges, 'type') },
  };

  // ── 출력 ───────────────────────────────────────────────────────────
  const graph = sortKeysDeep({
    schemaVersion: 1,
    kind: 'evidence-graph',
    note: '파생물 — emit-evidence-graph.mjs 가 campaign.json(+overlay)에서 생성한다. 손으로 편집하지 않는다. 검증: node _workspace/current/planning/validate-evidence-graph.mjs',
    provenance: {
      generator: 'emit-evidence-graph.mjs',
      inputs,
    },
    counts,
    overlay: overlay ? { reveals: overlay.reveals } : null,
    nodes,
    edges,
  });
  writeFileSync(OUT_PATH, JSON.stringify(graph, null, 2) + '\n');
  process.stdout.write(
    `evidence-graph.json written: nodes ${counts.nodes.total} edges ${counts.edges.total} ` +
    `(${Object.entries(counts.nodes.byType).map(([k, v]) => `${k} ${v}`).join(', ')})\n`);
  process.exit(0);
} catch (err) {
  process.stderr.write(`emit-evidence-graph: ERROR ${err && err.message ? err.message : String(err)}\n`);
  process.exit(2);
}
