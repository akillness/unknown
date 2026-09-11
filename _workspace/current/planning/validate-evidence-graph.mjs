#!/usr/bin/env node
// validate-evidence-graph.mjs — planning 레인 소유 증거 그래프 검증기 (TRACE-RPG 6가족 번안, C7)
//
// 목적: emit-evidence-graph.mjs 가 만든 evidence-graph.json 이 (1) 입력 정본과 일치하고
//       (2) TRACE-RPG(neural_symbolic_in_game)의 6검사 가족을 이 게임 데이터로 번안한
//       구조 불변식을 지키는지 기계로 판정한다.
//       가족 대응: precondition→EG-PRE · quest stage→EG-STAGE · reachability→EG-REACH ·
//       NPC knowledge→EG-KNOW · action policy→EG-TOOL · disclosure→EG-DISC.
//       provenance 감사(EG-PROV)는 TRACE-RPG C3(audit-linked commit) 번안이다.
//
// 이 검증기는 validate-campaign.mjs(캠페인 유일 측정 명령, 49검사)를 대체하지 않는다.
// 어떤 경우에도 파일을 쓰지 않는다 — 실패해도 상태 불변 (TRACE-RPG: 거부된 후보는
// 변경 없는 이전 상태로 폴백).
//
// 의존성: Node ≥18 내장 모듈만.
// 실행:   node _workspace/current/planning/validate-evidence-graph.mjs
// 출력:   검사마다 `<id> <이름> <PASS|FAIL> count=<위반수> [위반 id…]` 1줄 + EG-SUMMARY 1줄.
// 종료:   0 = 전 검사 PASS, 1 = FAIL 존재, 2 = 실행 오류.

import { readFileSync, existsSync } from 'node:fs';
import { createHash } from 'node:crypto';
import { dirname, join } from 'node:path';
import { fileURLToPath } from 'node:url';

const HERE = dirname(fileURLToPath(import.meta.url));
const GRAPH_PATH = join(HERE, 'evidence-graph.json');
const CAMPAIGN_PATH = join(HERE, 'campaign.json');
const OVERLAY_PATH = join(HERE, 'evidence-graph-overlay.json');

const CANON_TOOLS = ['circuit', 'reader', 'alignment', 'routing', 'corrosion', 'seal']; // glossary §3-1
const STAGE_ORDER = ['T0', 'C1', 'C2', 'C3', 'C4', 'C5', 'C6', 'C7', 'E0'];
const ROOT_BEAT = 't0-b1';

const sha256 = (buf) => createHash('sha256').update(buf).digest('hex');

const results = [];
function check(id, name, violations) {
  const v = [...violations];
  results.push({ id, name, pass: v.length === 0, violations: v });
  const tail = v.length ? ' ' + v.slice(0, 8).join(' ') + (v.length > 8 ? ` …(+${v.length - 8})` : '') : '';
  process.stdout.write(`${id} ${name} ${v.length === 0 ? 'PASS' : 'FAIL'} count=${v.length}${tail}\n`);
}

try {
  const graph = JSON.parse(readFileSync(GRAPH_PATH, 'utf8'));
  const nodes = graph.nodes;
  const edges = graph.edges;
  const byType = (t) => nodes.filter((n) => n.type === t);
  const edgesOf = (t) => edges.filter((e) => e.type === t);
  const beatNodes = byType('beat');
  const clueNodes = byType('clue');
  const sourceIds = new Set(byType('source').map((n) => n.id));
  const beatIds = new Set(beatNodes.map((n) => n.id));
  const docIndex = new Map(beatNodes.map((n) => [n.id, n.docIndex]));

  // ── EG-PROV — provenance 감사 (TRACE-RPG C3 번안) ────────────────────
  // 그래프가 기록한 입력 sha256/bytes 를 지금 디스크의 입력에서 재계산해 대조한다.
  {
    const bad = [];
    for (const inp of graph.provenance.inputs) {
      const p = inp.path === 'planning/campaign.json' ? CAMPAIGN_PATH
        : inp.path === 'planning/evidence-graph-overlay.json' ? OVERLAY_PATH : null;
      if (!p || !existsSync(p)) { bad.push(`${inp.path}:missing`); continue; }
      const buf = readFileSync(p);
      if (sha256(buf) !== inp.sha256) bad.push(`${inp.path}:sha-mismatch`);
      if (buf.length !== inp.bytes) bad.push(`${inp.path}:bytes-mismatch`);
    }
    check('EG-PROV-01', 'provenance 입력 sha256·bytes 재계산 일치 (그래프가 지금 입력의 파생임)', bad);
  }

  // ── EG-PRE — precondition (가족 1 번안) ──────────────────────────────
  // prerequisites(requires 엣지)가 실존 beat 를 가리키고, requires 그래프에 사이클이 없다.
  const requires = edgesOf('requires'); // from(비트) → to(선행 비트)
  check('EG-PRE-01', 'requires 엣지 양끝이 실존 beat', requires
    .filter((e) => !beatIds.has(e.from) || !beatIds.has(e.to))
    .map((e) => `${e.from}->${e.to}`));
  const prereqOf = new Map([...beatIds].map((b) => [b, []]));
  for (const e of requires) if (beatIds.has(e.from) && beatIds.has(e.to)) prereqOf.get(e.from).push(e.to);
  // DFS 착색으로 사이클 검출
  let cycles = [];
  {
    const color = new Map(); // 0 미방문 1 진행중 2 완료
    const visit = (b, path) => {
      color.set(b, 1);
      for (const p of prereqOf.get(b)) {
        if (color.get(p) === 1) { cycles.push([...path, b, p].join('->')); continue; }
        if (!color.get(p)) visit(p, [...path, b]);
      }
      color.set(b, 2);
    };
    for (const b of beatIds) if (!color.get(b)) visit(b, []);
  }
  check('EG-PRE-02', 'requires 그래프가 DAG (사이클 0)', cycles);

  // ── EG-STAGE — quest stage (가족 6·7 번안) ───────────────────────────
  // requires 가 스테이지 순서(T0<C1<…<C7<E0)를 역행하지 않고, beat 의 stage 귀속이
  // contains 엣지와 stageId 필드 양쪽에서 일치한다.
  const stageIdx = new Map(STAGE_ORDER.map((s, i) => [s, i]));
  const stageOfBeat = new Map();
  for (const e of edgesOf('contains')) stageOfBeat.set(e.to, e.from);
  check('EG-STAGE-01', 'requires 선행 비트의 스테이지 ≤ 후행 비트의 스테이지 (역행 0)', requires
    .filter((e) => stageIdx.get(stageOfBeat.get(e.to)) > stageIdx.get(stageOfBeat.get(e.from)))
    .map((e) => `${e.from}(${stageOfBeat.get(e.from)})->${e.to}(${stageOfBeat.get(e.to)})`));
  check('EG-STAGE-02', 'beat.stageId == contains 소속 스테이지 (전건, 정확히 1 스테이지)', beatNodes
    .filter((b) => stageOfBeat.get(b.id) !== b.stageId ||
      edgesOf('contains').filter((e) => e.to === b.id).length !== 1)
    .map((b) => b.id));

  // ── EG-REACH — reachability (가족 3 번안) ────────────────────────────
  // (a) campaign 문서 순서(docIndex)가 requires DAG 의 위상 순서다 — 모든 선행 비트의
  //     docIndex 가 더 작다. 이후 검사들의 "위상 순서" 비교는 이 검증된 docIndex 를 쓴다.
  // (b) 모든 beat 가 t0-b1 에서 requires 역방향(선행→후행) 도달 가능 — 고아 beat 0.
  // (c) 모든 clue 가 정확히 한 beat 에서 yields 된다.
  check('EG-REACH-01', 'docIndex 가 requires DAG 의 위상 순서 (선행 docIndex < 후행 docIndex)', requires
    .filter((e) => !(docIndex.get(e.to) < docIndex.get(e.from)))
    .map((e) => `${e.to}(${docIndex.get(e.to)})!<${e.from}(${docIndex.get(e.from)})`));
  {
    const dependents = new Map([...beatIds].map((b) => [b, []]));
    for (const e of requires) dependents.get(e.to).push(e.from);
    const seen = new Set([ROOT_BEAT]);
    const queue = [ROOT_BEAT];
    while (queue.length) {
      const b = queue.shift();
      for (const d of dependents.get(b) ?? []) if (!seen.has(d)) { seen.add(d); queue.push(d); }
    }
    check('EG-REACH-02', `모든 beat 가 ${ROOT_BEAT} 기점 도달 가능 (고아 0)`, [...beatIds].filter((b) => !seen.has(b)));
  }
  {
    const yieldCount = new Map();
    for (const e of edgesOf('yields')) yieldCount.set(e.to, (yieldCount.get(e.to) ?? 0) + 1);
    check('EG-REACH-03', '모든 clue 가 정확히 1개 beat 에서 yields', clueNodes
      .filter((c) => (yieldCount.get(c.id) ?? 0) !== 1).map((c) => c.id));
  }

  // ── EG-KNOW — knowledge (가족 4 번안: 미신고 사실 금지 → 무근거 출처 금지) ──
  // 모든 clue.originId 가 source 노드로 실존하고, source 집합이 **정본 campaign.json 에서
  // 독립 재파생한 카탈로그**와 정확히 일치하며(그래프 자기참조로 공허 통과하지 않도록),
  // 카탈로그 크기가 glossary §7 계약(31종)과 같고, 출처 계보(originId→copiedFrom,
  // validate-campaign C-04/C-05 와 같은 파생)가 모순·사이클 없이 루트에 닿는다.
  check('EG-KNOW-01', '모든 clue.originId 가 source 노드로 실존', clueNodes
    .filter((c) => !sourceIds.has(c.originId)).map((c) => `${c.id}:${c.originId}`));
  {
    const campaign = JSON.parse(readFileSync(CAMPAIGN_PATH, 'utf8'));
    const canonCatalog = new Set(
      campaign.stages.flatMap((s) => s.beats.flatMap((b) => b.clues.map((c) => c.originId))));
    const bad = [];
    for (const s of sourceIds) if (!canonCatalog.has(s)) bad.push(`graph-only:${s}`);
    for (const s of canonCatalog) if (!sourceIds.has(s)) bad.push(`campaign-only:${s}`);
    if (canonCatalog.size !== 31) bad.push(`catalog-size:${canonCatalog.size}!=31`);
    check('EG-KNOW-02', 'source 집합 == campaign 재파생 카탈로그 (glossary §7 계약: 31종)', bad);
  }
  check('EG-KNOW-03', 'copied_from 대상이 source 노드로 실존', edgesOf('copied_from')
    .filter((e) => !sourceIds.has(e.to)).map((e) => `${e.from}->${e.to}`));
  {
    // clue 가 주장한 계보를 source 지도로 파생: originId → copiedFrom
    const copyOf = new Map();
    const conflicts = [];
    for (const c of clueNodes) {
      if (!c.copiedFrom) continue;
      if (copyOf.has(c.originId) && copyOf.get(c.originId) !== c.copiedFrom) conflicts.push(c.originId);
      else copyOf.set(c.originId, c.copiedFrom);
    }
    check('EG-KNOW-04', 'originId→copiedFrom 계보 지도 모순 없음', conflicts);
    const cyc = [];
    for (const s of sourceIds) {
      const seen = new Set();
      let cur = s;
      while (copyOf.has(cur)) {
        if (seen.has(cur)) { cyc.push(s); break; }
        seen.add(cur);
        cur = copyOf.get(cur);
      }
    }
    check('EG-KNOW-05', '계보 체인이 사이클 없이 루트 출처에 닿음', cyc);
  }

  // ── EG-TOOL — action policy (가족: 외부 행동 정책 번안) ──────────────
  // uses_tool 대상이 canon 6종 안이고, 각 도구의 첫 사용(uses_tool 최소 docIndex)보다
  // 교습(teaches_tool 최소 docIndex)이 위상 순서상 같거나 앞이다.
  const uses = edgesOf('uses_tool');
  const teaches = edgesOf('teaches_tool');
  check('EG-TOOL-01', `uses_tool·teaches_tool 대상이 canon 6종 안 (${CANON_TOOLS.join('/')})`,
    [...uses, ...teaches].filter((e) => !CANON_TOOLS.includes(e.to)).map((e) => `${e.from}->${e.to}`));
  {
    const firstUse = new Map();
    for (const e of uses) {
      const d = docIndex.get(e.from);
      if (!firstUse.has(e.to) || d < firstUse.get(e.to)) firstUse.set(e.to, d);
    }
    const firstTeach = new Map();
    for (const e of teaches) {
      const d = docIndex.get(e.from);
      if (!firstTeach.has(e.to) || d < firstTeach.get(e.to)) firstTeach.set(e.to, d);
    }
    const bad = [];
    for (const [tool, useIdx] of firstUse) {
      const teachIdx = firstTeach.get(tool);
      if (teachIdx === undefined) bad.push(`${tool}:never-taught`);
      else if (!(teachIdx <= useIdx)) bad.push(`${tool}:teach@${teachIdx}>use@${useIdx}`);
    }
    check('EG-TOOL-02', '각 도구의 첫 교습이 첫 사용과 같거나 앞 (위상 순서 = 검증된 docIndex)', bad);
  }

  // ── EG-DISC — disclosure (가족 5 번안: 금지 사실 공개 금지) ──────────
  // 오버레이(reveals)의 각 fact 에 대해 seedBeats·revealBeat 실존 + 모든 seed 가
  // reveal 보다 위상 순서상 앞임을 검사한다.
  // **구조 검사만이다**: clue description·objective 텍스트의 의미(어느 문장이 어느 반전을
  // 미리 말하는지)는 여기서 판정하지 않는다 — 텍스트 상한·금지 문자열의 의미 판정은
  // timeline.md §7 상한표와 validate-campaign.mjs 의 텍스트 검사가 소유한다.
  {
    const reveals = graph.overlay?.reveals ?? [];
    check('EG-DISC-01', '오버레이 존재 + fact 별 seedBeats·revealBeat 가 실존 beat',
      reveals.length === 0 ? ['overlay-missing-or-empty'] : reveals.flatMap((r) =>
        [...r.seedBeats, r.revealBeat].filter((b) => !beatIds.has(b)).map((b) => `${r.factId}:${b}`)));
    check('EG-DISC-02', '모든 seed 가 reveal 보다 위상 순서상 앞 (구조 검사만 — 텍스트 의미 판정 없음)',
      reveals.flatMap((r) => r.seedBeats
        .filter((s) => !(docIndex.get(s) < docIndex.get(r.revealBeat)))
        .map((s) => `${r.factId}:${s}!<${r.revealBeat}`)));
    check('EG-DISC-03', 'reveal 비트가 seed 전부에서 requires 경로로 도달 가능 (씨앗 없이 회수 불가)',
      reveals.flatMap((r) => {
        // revealBeat 의 선행 폐포(조상 집합)에 모든 seed 가 들어 있는가
        const anc = new Set();
        const stack = [r.revealBeat];
        while (stack.length) {
          const b = stack.pop();
          for (const p of prereqOf.get(b) ?? []) if (!anc.has(p)) { anc.add(p); stack.push(p); }
        }
        return r.seedBeats.filter((s) => !anc.has(s)).map((s) => `${r.factId}:${s}-not-ancestor-of-${r.revealBeat}`);
      }));
  }

  // ── 요약 ─────────────────────────────────────────────────────────────
  const pass = results.filter((r) => r.pass).length;
  const fail = results.length - pass;
  process.stdout.write(`EG-SUMMARY checks ${results.length} pass ${pass} fail ${fail} ${fail === 0 ? 'PASS' : 'FAIL'}\n`);
  process.exit(fail === 0 ? 0 : 1);
} catch (err) {
  process.stderr.write(`validate-evidence-graph: ERROR ${err && err.message ? err.message : String(err)}\n`);
  process.exit(2);
}
