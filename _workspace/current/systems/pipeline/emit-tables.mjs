#!/usr/bin/env node
// emit-tables.mjs — systems 레인 소유 런타임 테이블 생성기 (C6-F13 · C7-F1)
//
// 목적: 저작 원본(planning/campaign.json + synopsis/t0-records.md + 레인 문서) →
//       런타임 테이블의 생성 시점·생성기·영수증을 하나로 고정한다. 검증 규칙은 **한 곳에만** 산다 —
//       `planning/validate-campaign.mjs`(planner 소유). 이 생성기는 그 검증기를 호출하고,
//       verdict 가 PASS 일 때만 테이블을 낸다(fail-closed). 규칙을 재구현하지 않는다.
//
// 두 개의 스코프
//   scope=all  (기본)  : Unity `Data/Tables/` 용 전 캠페인 테이블 — beats.json(바이트 동일 사본) · hints.json(투영)
//   scope=t0   (스테이지): T0 수직 슬라이스 **인스턴스 데이터** — beats/hints/tools/zones/records.json + 각 .meta.md
//                          (RFC-C7-001 (2). 손으로 쓰지 않는다 — 값은 전부 저작 문서에서 파생한다)
//
// 스코프 결정 규칙(결정적 · 영수증에 `scope`/`scopeSource` 로 찍힌다)
//   1) `--scope <all|t0|c1|…>` 가 있으면 그 값
//   2) 없고 `--out` 의 마지막 경로 조각이 스테이지 id 모양(`^[a-z]\d$`)이면 그 값
//   3) 그 외에는 `all`
//
// 값의 출처(발명 0건). 파싱이 실패하면 **테이블을 만들지 않고 exit 2** 한다.
//   beats/hints  ← planning/campaign.json (t0 서브셋 + 완료 술어)
//   tools        ← systems/data-schemas/tools.md (도구 표·노브) + system-specs/{wiring-trace,plate-readout}.md
//   zones        ← concept/style-guide.md §5(카메라) · modeling/{specs/hub-watchroom.md,asset-manifest.md}(치수·배치)
//                  · worldview/worldview-bible.md §2(회선 3개소·분해능)
//   records      ← synopsis/t0-records.md (표 형식 고정) + campaign 단서(루트 originId)
//
// 의존성: Node 내장 모듈만. 설치 0건.
// 실행:
//   node _workspace/current/systems/pipeline/emit-tables.mjs                              # 드라이런(쓰기 0건)
//   node _workspace/current/systems/pipeline/emit-tables.mjs --out <dir>                  # scope=all
//   node _workspace/current/systems/pipeline/emit-tables.mjs --out _workspace/current/systems/data/t0
//   node _workspace/current/systems/pipeline/emit-tables.mjs --scope t0                   # 드라이런 t0
// 종료 코드: 0 = 생성/드라이런 성공 · 1 = 검증기 FAIL(테이블 미생성) · 2 = 실행 오류(파싱 실패 포함).

import { readFileSync, writeFileSync, mkdirSync, existsSync } from 'node:fs';
import { createHash } from 'node:crypto';
import { execFileSync } from 'node:child_process';
import { resolve, dirname, join, basename } from 'node:path';
import { fileURLToPath } from 'node:url';

const HERE = dirname(fileURLToPath(import.meta.url));
const CURRENT = resolve(HERE, '..', '..'); // _workspace/current

function arg(name, fallback) {
  const i = process.argv.indexOf(name);
  return i > -1 && process.argv[i + 1] ? process.argv[i + 1] : fallback;
}
const has = (name) => process.argv.includes(name);

const SOURCE = resolve(arg('--source', join(CURRENT, 'planning', 'campaign.json')));
const VALIDATOR = resolve(arg('--validator', join(CURRENT, 'planning', 'validate-campaign.mjs')));
const OUT = has('--out') ? resolve(arg('--out', '')) : null;
const DOC_DATE = arg('--date', new Date().toISOString().slice(0, 10));

const sha = (buf) => createHash('sha256').update(buf).digest('hex');

function die(code, msg, extra) {
  process.stderr.write(`emit-tables: ${msg}\n`);
  if (extra) process.stderr.write(`${extra}\n`);
  process.exit(code);
}

if (!existsSync(SOURCE)) die(2, `저작 원본 없음: ${SOURCE}`);
if (!existsSync(VALIDATOR)) die(2, `검증기 없음: ${VALIDATOR}`);

// ── 1. 검증기 호출 (규칙의 단일 출처). FAIL 이면 아무것도 쓰지 않는다.
function runValidator(extraArgs) {
  let report;
  let exitCode = 0;
  try {
    const stdout = execFileSync(process.execPath, [VALIDATOR, SOURCE, ...extraArgs], {
      encoding: 'utf8',
      maxBuffer: 64 * 1024 * 1024,
    });
    report = JSON.parse(stdout);
  } catch (e) {
    exitCode = typeof e.status === 'number' ? e.status : 2;
    const out = e.stdout ? String(e.stdout) : '';
    try { report = JSON.parse(out); } catch { report = null; }
    if (!report) die(2, `검증기 실행 실패 (exit ${exitCode})`, e.stderr ? String(e.stderr) : '');
  }
  return { report, exitCode };
}

const { report, exitCode: validatorExit } = runValidator([]);
if (report.summary?.verdict !== 'PASS' || validatorExit !== 0) {
  die(1, `검증기 verdict=${report.summary?.verdict} fail=${report.summary?.fail} — 테이블을 생성하지 않는다(fail-closed)`);
}

// ── 2. 입력 로드
const sourceBuf = readFileSync(SOURCE);
const sourceSha = sha(sourceBuf);
if (report.sha256 && report.sha256 !== sourceSha) {
  die(2, `검증기가 읽은 파일과 생성기가 읽은 파일의 sha256 불일치 — 동시 편집 의심`);
}
const campaign = JSON.parse(sourceBuf.toString('utf8'));

// ── 3. 스코프 결정
const STAGE_IDS = campaign.stages.map((s) => String(s.id).toLowerCase());
const scopeArg = arg('--scope', null);
let scope, scopeSource;
if (scopeArg) { scope = scopeArg.toLowerCase(); scopeSource = '--scope'; }
else if (OUT && /^[a-z]\d$/.test(basename(OUT))) { scope = basename(OUT); scopeSource = 'out-dir-basename'; }
else { scope = 'all'; scopeSource = 'default'; }
if (scope !== 'all' && !STAGE_IDS.includes(scope)) {
  die(2, `알 수 없는 scope=${scope} (허용: all, ${STAGE_IDS.join(', ')})`);
}

// ── 4. 공통 파생: 루트 originId (copiedFrom 체인)
const clueById = new Map();
for (const st of campaign.stages) for (const b of st.beats) for (const c of b.clues) clueById.set(c.id, c);
function rootOriginOf(clue) {
  const seen = new Set();
  let cur = clue;
  while (cur && cur.copiedFrom) {
    if (seen.has(cur.id)) die(2, `copiedFrom 순환: ${cur.id}`);
    seen.add(cur.id);
    const next = clueById.get(cur.copiedFrom);
    if (!next) die(2, `copiedFrom 고아 참조: ${cur.id} → ${cur.copiedFrom}`);
    cur = next;
  }
  return cur.originId;
}

// ── 5. hints 투영 (스코프 공통)
function hintRowsFor(stages) {
  const rows = [];
  for (const stage of stages) {
    for (const beat of stage.beats) {
      beat.hints.forEach((text, idx) => {
        const level = idx + 1;
        rows.push({
          hintId: `${beat.id}-h${level}`,
          beatId: beat.id,
          level,
          textKey: `hint.${beat.id}.l${level}`,
          revealScope: ['direction', 'procedure', 'solution'][idx],
          sourceTextKo: text,
          revealsValues: level === 3,
          warnsBeforeReveal: level === 3,
          cost: 0,
          achievementPenalty: 0,
          affectsEnding: false,
        });
      });
    }
  }
  return rows;
}

// ══════════════════════════════════════════════════════════════════════
//  Markdown 파싱 유틸 — 저작 문서의 "형식 고정" 표를 읽는다
// ══════════════════════════════════════════════════════════════════════

function readDoc(relPath) {
  const p = resolve(CURRENT, relPath);
  if (!existsSync(p)) die(2, `저작 문서 없음: ${p}`);
  return { path: relPath, text: readFileSync(p, 'utf8'), sha256: sha(readFileSync(p)) };
}

/** 제목 문자열(부분 일치)로 시작해 같은/상위 수준 제목 직전까지의 구간. */
function section(text, headingStartsWith) {
  const lines = text.split('\n');
  let start = -1, level = 0;
  for (let i = 0; i < lines.length; i++) {
    const m = lines[i].match(/^(#{2,6})\s+(.*)$/);
    if (!m) continue;
    if (start === -1 && m[2].startsWith(headingStartsWith)) { start = i; level = m[1].length; continue; }
    if (start !== -1 && m[1].length <= level) return lines.slice(start, i).join('\n');
  }
  if (start === -1) die(2, `절을 찾지 못했다: "${headingStartsWith}"`);
  return lines.slice(start).join('\n');
}

const cleanCell = (s) =>
  String(s)
    .replace(/\*\*/g, '')
    .replace(/`/g, '')
    .trim()
    .replace(/^["“”]|["“”]$/g, '')
    .trim();

/** 구간 안의 n번째(기본 0) 파이프 표를 [행][셀] 로 돌려준다(구분선·헤더 제외). */
function table(sectionText, index = 0) {
  const blocks = [];
  let cur = null;
  for (const line of sectionText.split('\n')) {
    if (/^\s*\|/.test(line)) { (cur ||= []).push(line); }
    else if (cur) { blocks.push(cur); cur = null; }
  }
  if (cur) blocks.push(cur);
  const blk = blocks[index];
  if (!blk) die(2, `표 #${index} 를 찾지 못했다`);
  const rows = blk
    .map((l) => l.trim().replace(/^\|/, '').replace(/\|$/, '').split('|').map(cleanCell))
    .filter((cells) => !cells.every((c) => /^:?-{2,}:?$/.test(c) || c === ''));
  return { header: rows[0], rows: rows.slice(1) };
}

/** `H-6:00` / `H+3:00` / `H+0:00` → H 기준 분(정수). */
function parsePhase(s) {
  const m = cleanCell(s).match(/H([+-])(\d+):(\d{2})/);
  if (!m) die(2, `조위 위상 파싱 실패: ${s}`);
  const v = Number(m[2]) * 60 + Number(m[3]);
  return m[1] === '-' ? -v : v;
}
const fmtPhase = (t) => `H${t < 0 ? '-' : '+'}${Math.floor(Math.abs(t) / 60)}:${String(Math.abs(t) % 60).padStart(2, '0')}`;

const num = (s, label) => {
  const m = cleanCell(s).replace(/−/g, '-').match(/-?\d+(\.\d+)?/);
  if (!m) die(2, `수치 파싱 실패(${label}): ${s}`);
  return Number(m[0]);
};
const round1 = (x) => Math.round(x * 10) / 10;

// ══════════════════════════════════════════════════════════════════════
//  T0 인스턴스 데이터 (RFC-C7-001)
// ══════════════════════════════════════════════════════════════════════

function buildT0(stageId) {
  const stage = campaign.stages.find((s) => String(s.id).toLowerCase() === stageId);
  if (!stage) die(2, `스테이지 없음: ${stageId}`);
  if (stage.zoneIds.length !== 1 || stage.zoneIds[0] !== 'hub') {
    die(2, `이 생성기의 zones 파생은 허브 1구역 스테이지에만 정의돼 있다 (실제: ${stage.zoneIds.join(',')})`);
  }

  const docs = {
    records: readDoc('synopsis/t0-records.md'),
    toolsSchema: readDoc('systems/data-schemas/tools.md'),
    zonesSchema: readDoc('systems/data-schemas/zones.md'),
    styleGuide: readDoc('concept/style-guide.md'),
    hubModel: readDoc('modeling/specs/hub-watchroom.md'),
    manifest: readDoc('modeling/asset-manifest.md'),
    bible: readDoc('worldview/worldview-bible.md'),
    wiring: readDoc('systems/system-specs/wiring-trace.md'),
    plateReadout: readDoc('systems/system-specs/plate-readout.md'),
    interaction: readDoc('systems/interaction-rules.md'),
    circuitOverlay: readDoc('planning/t0-circuit-overlay.json'),
  };

  const records = buildRecords(docs, stage);
  const assignmentBlock = docs.records.text.match(/<!-- T0-SOURCE-ASSIGNMENTS:START -->\s*```json\s*([\s\S]*?)```\s*<!-- T0-SOURCE-ASSIGNMENTS:END -->/);
  if (!assignmentBlock) die(2, 'Missing canonical T0 source assignments');
  const assignments = JSON.parse(assignmentBlock[1]);
  const assigned = new Set();
  for (const assignment of assignments) {
    const row = records.rows.find(r => r.recordId === assignment.recordId);
    if (!row || assigned.has(row.recordId) || !assignment.stationId ||
        (row.sourceType === 'plate' && !assignment.systemId)) die(2, 'Invalid T0 source assignment');
    assigned.add(row.recordId);
    row.systemId = assignment.systemId;
    row.stationId = assignment.stationId;
    row.sourceAssignment = assignment;
    row._src.citationProvenance = 'synopsis/t0-records.md §12 (RFC-CX-001 / RFC-CX-003)';
  }
  const zones = buildZones(docs, stage);
  const tools = buildTools(docs, stage);
  const overlay = JSON.parse(docs.circuitOverlay.text).circuitOverlay;
  if (!overlay || !(overlay.gridStep > 0) || overlay.fineGridStep !== null || overlay.anchors.length !== 3)
    die(2, 'Invalid canonical T0 circuit overlay');
  tools.circuitOverlay = overlay;
  const beats = buildBeats(stage, records, zones);
  const hints = {
    schemaVersion: 1,
    scope: stageId,
    generatedFrom: 'planning/campaign.json',
    sourceSha256: sourceSha,
    rows: hintRowsFor([stage]),
  };

  return { docs, records, zones, tools, beats, hints };
}

// ── records.json ──────────────────────────────────────────────────────
function buildRecords(docs, stage) {
  const md = docs.records.text;

  // §2 표 1 — 레코드 색인 (형식 고정)
  const idx = table(section(md, '2. 표 1 — 레코드 색인'));
  const expectHead = ['recordId', 'medium', 'originId', 'clueIds', 'visibleAt', 'zoneId'];
  expectHead.forEach((h, i) => {
    if (cleanCell(idx.header[i]) !== h) die(2, `t0-records §2 표 헤더가 바뀌었다: ${idx.header.join(' | ')}`);
  });

  // 캠페인 단서 색인(루트 originId 파생)
  const cluesOfStage = new Map();
  for (const b of stage.beats) for (const c of b.clues) cluesOfStage.set(c.id, { beatId: b.id, clue: c });

  const rows = idx.rows.map((cells) => {
    const [recordId, medium, originId, clueIdsRaw, visibleAtRaw, zoneId, koName] = cells;
    const clueIds = clueIdsRaw.split(',').map((s) => cleanCell(s)).filter(Boolean);
    const visibleAt = visibleAtRaw.split(',').map((s) => cleanCell(s)).filter(Boolean);
    // 정합: 단서 id·originId·매체가 campaign 과 일치하는지 (planner T0-01 의 생성기 쪽 절반)
    const roots = new Set();
    for (const cid of clueIds) {
      const hit = cluesOfStage.get(cid);
      if (!hit) die(2, `t0-records §2 의 clueId 가 캠페인 ${stage.id} 에 없다: ${cid}`);
      if (hit.clue.originId !== originId) die(2, `originId 불일치: ${cid} 문서=${originId} live=${hit.clue.originId}`);
      if (hit.clue.sourceType !== medium) die(2, `매체 불일치: ${cid} 문서=${medium} live=${hit.clue.sourceType}`);
      roots.add(rootOriginOf(hit.clue));
    }
    if (roots.size !== 1) die(2, `루트 originId 가 갈린다: ${recordId} → ${[...roots].join(',')}`);
    return {
      recordId,
      medium,
      sourceType: medium, // plates.md §1 은 sourceType 을 쓴다(mediaType 개명 · C6-F13)
      originId,
      rootOriginId: [...roots][0],
      clueIds,
      visibleAt,
      zoneId,
      displayNameKo: koName,
      _src: {
        recordId: 'synopsis/t0-records.md §2 표 1 (행 1:1)',
        sourceType: 'planning/campaign.json clues[].sourceType (문서와 일치 검사 통과)',
        originId: 'planning/campaign.json clues[].originId',
        rootOriginId: 'copiedFrom 체인 파생 (interaction-rules.md §3 — 사본은 원본의 루트를 물려받는다, RFC-S5 예외 없음)',
        displayNameKo: 'worldview/glossary.md §7 카탈로그 (t0-records §2 표의 KO 명칭 열)',
      },
    };
  });

  const byId = Object.fromEntries(rows.map((r) => [r.recordId, r]));

  // §3.1 인수 각서 본문 행
  byId['rec-handover-brief'].lines = table(section(md, '3.1 본문 행')).rows.map(([lineId, kind, text, flag]) => ({
    lineId, kind, text, flag: flag === '—' ? null : flag,
  }));
  byId['rec-handover-brief']._src.lines = 'synopsis/t0-records.md §3.1 (형식 고정 표)';

  // §4.1 이관 목록 수치 행
  byId['rec-transfer-list'].rows = table(section(md, '4.1 수치 행')).rows.map(([rowId, no, item, mediaKind, qty, unit, note]) => ({
    rowId,
    no: no === '—' ? null : Number(no),
    item,
    mediaKind: mediaKind === '—' ? null : mediaKind,
    qty: qty === '—' ? null : Number(qty),
    unit: unit === '—' ? null : unit,
    note,
  }));
  byId['rec-transfer-list']._src.rows = 'synopsis/t0-records.md §4.1 (형식 고정 표)';

  // §6 근무 규정 필사본
  byId['rec-watchlog-bureau'].lines = table(section(md, '6. `rec-watchlog-bureau`')).rows.map(([lineId, kind, text, flag]) => ({
    lineId, kind, text, flag: flag === '—' ? null : flag,
  }));
  byId['rec-watchlog-bureau']._src.lines = 'synopsis/t0-records.md §6 (형식 고정 표)';

  // §5 당직실 표준판 — 링·채널·규칙·샘플·각인·부식층
  const plate = byId['rec-plate-standard-hub'];
  const s50 = table(section(md, '5.0 링 창과 격자'));
  const ring = Object.fromEntries(s50.rows.map(([k, v]) => [k, v]));
  const ringWindowM = ring['ringWindow'].split('~').map(parsePhase);
  plate.ringWindow = { startMin: ringWindowM[0], endMin: ringWindowM[1], display: ring['ringWindow'] };
  plate.resolutionMin = num(ring['resolutionMin'], 'resolutionMin');
  plate.sampleCount = num(ring['sampleCount'], 'sampleCount');
  const authoredM = ring['authoredRange'].split('~').map(parsePhase);
  plate.authoredRange = { startMin: authoredM[0], endMin: authoredM[1], display: ring['authoredRange'] };
  plate.phaseFormat = ring['phaseFormat'];

  plate.channels = table(section(md, '5.1 채널')).rows.map(([channelId, ko, unit, t0Readable, note]) => ({
    channelId, ko, unit, readableInT0: t0Readable === '가능', note: note || null,
  }));

  // §5.2 결정적 규칙 — 코드 펜스에서 계수를 읽는다(값을 손으로 옮기지 않는다)
  const ruleText = section(md, '5.2 곡선 생성 규칙').replace(/−/g, '-').replace(/·/g, '*');
  const g = (re, label) => { const m = ruleText.match(re); if (!m) die(2, `§5.2 규칙 파싱 실패: ${label}`); return m; };
  const mTide = g(/tide\(t\)\s*=\s*(\d+(?:\.\d+)?)\s*\+\s*(\d+(?:\.\d+)?)\s*\*\s*cos\(2π\s*\*\s*t\s*\/\s*(\d+)\)/, 'tide');
  const mPress = g(/pressure\(t\)\s*=\s*(\d+(?:\.\d+)?)\s*\+\s*(\d+(?:\.\d+)?)\s*\*\s*\(tide\(t\)\s*-\s*(\d+)\)/, 'pressure');
  const mSal = g(/salinity\(t\)\s*=\s*(\d+(?:\.\d+)?)\s*\+\s*(\d+(?:\.\d+)?)\s*\*\s*\(tide\(t\)\s*-\s*(\d+)\)/, 'salinity');
  const mWater = g(/waterLevel\(t\)\s*=\s*(\d+(?:\.\d+)?)\s*\*\s*tide\(t\)\s*\+\s*(\d+(?:\.\d+)?)/, 'waterLevel');
  const mFlat = g(/FLAT\s*=\s*\[(.+?)\]\s*#/, 'FLAT');
  const mMiss = g(/MISSING\s*=\s*\[(-?\d+),\s*(-?\d+)\]/, 'MISSING');

  const K = {
    tideBase: Number(mTide[1]), tideAmp: Number(mTide[2]), periodMin: Number(mTide[3]),
    pressBase: Number(mPress[1]), pressSlope: Number(mPress[2]), pressPivot: Number(mPress[3]),
    salBase: Number(mSal[1]), salSlope: Number(mSal[2]), salPivot: Number(mSal[3]),
    waterSlope: Number(mWater[1]), waterBase: Number(mWater[2]),
  };
  const flatBands = [...mFlat[1].matchAll(/\[(-?\d+),\s*(-?\d+)\]/g)].map((m) => [Number(m[1]), Number(m[2])]);
  const missingBand = [Number(mMiss[1]), Number(mMiss[2])];
  if (flatBands.length !== 3) die(2, `§5.2 FLAT 구간 수가 3이 아니다: ${flatBands.length}`);

  const tideRaw = (t) => K.tideBase + K.tideAmp * Math.cos((2 * Math.PI * t) / K.periodMin);
  const pressRaw = (t) => K.pressBase + K.pressSlope * (tideRaw(t) - K.pressPivot);
  const salRaw = (t) => K.salBase + K.salSlope * (tideRaw(t) - K.salPivot);
  const waterRaw = (t) => K.waterSlope * tideRaw(t) + K.waterBase;
  const inBand = (t, [a, b]) => t >= a && t <= b;

  plate.rule = {
    ...K,
    flatBands, missingBand,
    flatRule: 'FLAT 구간 [a,b] 안에서는 pressure(t) := pressure(a) 로 고정한다(다른 채널은 그대로)',
    missingRule: 'MISSING 구간 안에서는 4채널 전부 null 이고 state = "missing"',
    rounding: { tide: 0, pressure: 1, salinity: 1, waterLevel: 0, note: '표시 반올림은 raw 값에서 한 번만 한다' },
    _src: 'synopsis/t0-records.md §5.2 코드 펜스 (계수·구간을 파싱 · 손으로 옮기지 않음)',
  };

  const samples = [];
  for (let t = plate.ringWindow.startMin; t <= plate.ringWindow.endMin; t += plate.resolutionMin) {
    const missing = inBand(t, missingBand);
    const flat = flatBands.find((b) => inBand(t, b));
    samples.push({
      t,
      phase: fmtPhase(t),
      tide: Math.round(tideRaw(t)),
      pressure: missing ? null : round1(flat ? pressRaw(flat[0]) : pressRaw(t)),
      salinity: missing ? null : round1(salRaw(t)),
      waterLevel: missing ? null : Math.round(waterRaw(t)),
      state: missing ? 'missing' : flat ? 'flat' : 'normal',
    });
  }
  if (samples.length !== plate.sampleCount) {
    die(2, `링 샘플 수 불일치: 생성 ${samples.length} ≠ §5.0 sampleCount ${plate.sampleCount}`);
  }
  plate.samples = samples;
  plate.samplesDerivation = 'rule';
  plate._src.samples = 'synopsis/t0-records.md §5.2 규칙으로 생성(저장된 표가 아니라 규칙이 정본) · §5.3 앵커와 전건 대조';

  // §5.3 앵커 표 ↔ 생성 샘플 전건 대조 (기계 검사)
  const anchorRows = table(section(md, '5.3 샘플 앵커 표')).rows;
  const anchorChecks = [];
  for (const [tRaw, phase, tide, press, sal, water, state] of anchorRows) {
    const t = num(tRaw, 'anchor t');
    const gen = samples.find((s) => s.t === t);
    if (!gen) die(2, `§5.3 앵커 t=${t} 가 링 창 밖이다`);
    const exp = {
      tide: num(tide, 'tide'),
      pressure: press === '—' ? null : num(press, 'pressure'),
      salinity: sal === '—' ? null : num(sal, 'salinity'),
      waterLevel: water === '—' ? null : num(water, 'waterLevel'),
    };
    const ok = gen.tide === exp.tide && gen.pressure === exp.pressure && gen.salinity === exp.salinity && gen.waterLevel === exp.waterLevel && fmtPhase(t) === cleanCell(phase);
    anchorChecks.push({ t, ok, state });
    if (!ok) die(2, `§5.3 앵커 불일치 t=${t}: 생성 ${JSON.stringify(gen)} ≠ 문서 ${JSON.stringify(exp)} — 표가 틀렸거나 규칙이 바뀌었다`);
  }

  const eng = table(section(md, '5.4 각인 이벤트')).rows.filter((r) => cleanCell(r[0]) !== '—');
  plate.engravings = eng.map(([eventId, phase, channel, content, basis]) => ({ eventId, phaseMin: parsePhase(phase), channel, content, basis }));
  plate.engravingsNote = '그 밤 허브 계통 밸브 개폐 각인 0건 — 부재의 기록이며 부재의 증거가 아니다(법1)';
  plate._src.engravings = 'synopsis/t0-records.md §5.4';

  plate.layers = table(section(md, '5.5 표면 부식 무늬 층')).rows.map(([layerId, range, observation, visibleAt]) => ({
    layerId, range, observation, visibleAt: visibleAt.split(',').map(cleanCell).filter(Boolean), meaning: null,
  }));
  plate._src.layers = 'synopsis/t0-records.md §5.5 (의미는 저작되지 않는다 — 상한 B02)';

  // §7 조위대장
  const ledger = byId['rec-tide-ledger-bureau'];
  ledger.columns = table(section(md, '7.1 열 정의')).rows.map(([columnId, ko, unit, note]) => ({ columnId, ko, unit, note }));
  const rangeText = section(md, '7.2 값 생성 규칙');
  const rangeM = rangeText.match(/기입 범위[^:]*:\s*`?(H[+-]\d+:\d{2})`?\s*~\s*`?(H[+-]\d+:\d{2})`?,\s*(\d+)분 간격\s*\*\*(\d+)칸\*\*/);
  if (!rangeM) die(2, '§7.2 기입 범위 파싱 실패');
  const lStart = parsePhase(rangeM[1]), lEnd = parsePhase(rangeM[2]), lStep = Number(rangeM[3]), lCells = Number(rangeM[4]);
  const ledgerRows = [];
  for (let t = lStart; t <= lEnd; t += lStep) ledgerRows.push({ t, phase: fmtPhase(t), tideHeight: Math.round(tideRaw(t)), scribe: 'written' });
  if (ledgerRows.length !== lCells) die(2, `조위대장 칸 수 불일치: 생성 ${ledgerRows.length} ≠ §7.2 ${lCells}`);
  ledger.entryRange = { startMin: lStart, endMin: lEnd, stepMin: lStep, cells: lCells, blanks: 0 };
  ledger.samples = ledgerRows;
  ledger.samplesDerivation = 'rule';
  ledger.rule = { formula: 'tideHeight(t) = round(tideBase + tideAmp*cos(2π*t/periodMin))', sharedWith: 'rec-plate-standard-hub', ...K };
  ledger._src.samples = 'synopsis/t0-records.md §7.2 규칙(§5.2 tide 와 같은 함수) · §7.3 앵커와 전건 대조';

  const ledgerAnchors = table(section(md, '7.3 앵커 행')).rows;
  for (const [phase, height] of ledgerAnchors) {
    const t = parsePhase(phase);
    const gen = ledgerRows.find((r) => r.t === t);
    if (!gen) die(2, `§7.3 앵커 ${phase} 가 기입 범위 밖이다`);
    if (gen.tideHeight !== num(height, 'tideHeight')) {
      die(2, `§7.3 앵커 불일치 ${phase}: 생성 ${gen.tideHeight} ≠ 문서 ${height}`);
    }
  }

  return {
    schemaVersion: 1,
    scope: String(stage.id).toLowerCase(),
    generatedFrom: ['planning/campaign.json', 'synopsis/t0-records.md'],
    sourceSha256: sourceSha,
    recordsDocSha256: docs.records.sha256,
    fieldOwnership: {
      content: 'game-synopsis-writer (synopsis/t0-records.md — 매체 내용)',
      shape: 'game-systems-designer (systems/data-schemas/plates.md — 필드·불변식)',
      idsAndMedium: 'game-planner (planning/campaign.json — clue id · originId · sourceType)',
    },
    crossChecks: {
      plateAnchors: `${anchorChecks.length}/${anchorChecks.length} 일치`,
      ledgerAnchors: `${ledgerAnchors.length}/${ledgerAnchors.length} 일치`,
      clueIdOriginMedium: '문서 ↔ campaign 전건 일치(불일치 시 exit 2)',
    },
    notMeasured: ['플레이 0회 · 판독 화면 0회 · 이 값들이 읽히는지 n=0'],
    rows,
  };
}

// ── zones.json ────────────────────────────────────────────────────────
function buildZones(docs, stage) {
  // (a) 카메라 상수 — concept/style-guide.md §5
  const sg = section(docs.styleGuide.text, '5. 2.5D 고정 시점 카메라 규칙').replace(/−/g, '-');
  const pick = (re, label) => { const m = sg.match(re); if (!m) die(2, `style-guide §5 파싱 실패: ${label}`); return m; };
  const fovH = Number(pick(/수평 화각 약 (\d+(?:\.\d+)?)°/, 'fov')[1]);
  const eyeHeight = Number(pick(/시선 높이 \*\*(\d+(?:\.\d+)?) m\*\*/, 'eyeHeight')[1]);
  const pitch = Number(pick(/피치 \*\*(-?\d+(?:\.\d+)?)°\*\*/, 'pitch')[1]);
  const roll = Number(pick(/롤 \*\*(-?\d+(?:\.\d+)?)°\*\*/, 'roll')[1]);
  const yawM = pick(/요는 노드별 (\d+)° 또는 ±(\d+)°/, 'yaw');
  const yawOptions = [-Number(yawM[2]), Number(yawM[1]), Number(yawM[2])];

  // (b) 실측 배치 — modeling/specs/hub-watchroom.md §2 + asset-manifest.md §2
  const hub2 = section(docs.hubModel.text, '2. 치수와 좌표계');
  const room = hub2.match(/방 (\d+\.\d+) m \(X\) × (\d+\.\d+) m \(Y\), 벽 높이 (\d+\.\d+) m, 벽 두께 (\d+\.\d+) m/);
  if (!room) die(2, 'hub-watchroom §2 방 치수 파싱 실패');
  const desk = hub2.match(/작업대 (\d+\.\d+) × (\d+\.\d+) × (\d+\.\d+), 중심 \((-?\d+\.\d+), (-?\d+\.\d+)\), 상면 z = (\d+\.\d+)/);
  if (!desk) die(2, 'hub-watchroom §2 작업대 파싱 실패');
  const shelf = hub2.match(/염판 선반 (\d+\.\d+) × (\d+\.\d+) × (\d+\.\d+), 중심 \((-?\d+\.\d+), (-?\d+\.\d+)\)/);
  if (!shelf) die(2, 'hub-watchroom §2 선반 파싱 실패');
  const openFace = /남\(-Y\)과 천장은 없다/.test(hub2);
  if (!openFace) die(2, 'hub-watchroom §2 개방면(-Y) 서술을 찾지 못했다 — 카메라 배치 규칙의 전제');

  const roomX = Number(room[1]), roomY = Number(room[2]), wallT = Number(room[4]);
  const bounds = { xMin: -roomX / 2 + wallT, xMax: roomX / 2 - wallT, yMin: -roomY / 2, yMax: roomY / 2 - wallT };

  const toolRows = table(section(docs.manifest.text, '2. 영웅 도구 6'), 1).rows;
  const tools = {};
  for (const [id, dims, pos, rotZ, install] of toolRows) {
    const d = dims.split(/[×x]/).map(Number);
    const p = pos.replace(/[()]/g, '').split(',').map((s) => Number(s.trim()));
    if (d.length !== 3 || p.length !== 3) die(2, `asset-manifest §2 배치 파싱 실패: ${id}`);
    tools[id] = { id, size: { x: d[0], y: d[1], z: d[2] }, pos: { x: p[0], y: p[1], z: p[2] }, rotZ: num(rotZ, 'rotZ'), install };
  }
  for (const need of ['SM_Tool_reader', 'SM_Tool_seal', 'SM_Tool_circuit']) {
    if (!tools[need]) die(2, `asset-manifest §2 에 ${need} 행이 없다`);
  }

  // (c) 시점 노드 배치 규칙 (결정적)
  const FILL_RATIO = 0.70; // [TARGET] 대상의 최대 수평 치수가 프레임 폭에서 차지하는 비율
  const halfFov = (fovH / 2) * (Math.PI / 180);
  function nodeFor({ nodeId, label, target, size, interactables, assetStatus, srcNote }) {
    const widest = Math.max(size.x, size.z);
    let D = widest / FILL_RATIO / (2 * Math.tan(halfFov));
    const yaw = target.x <= -0.8 ? yawOptions[0] : target.x >= 0.8 ? yawOptions[2] : yawOptions[1];
    const rad = (yaw * Math.PI) / 180;
    let clampedTo = null;
    for (let guard = 0; guard < 64; guard++) {
      const cx = target.x - D * Math.sin(rad);
      const cy = target.y - D * Math.cos(rad);
      if (cx >= bounds.xMin && cx <= bounds.xMax && cy >= bounds.yMin && cy <= bounds.yMax) break;
      D = Math.round((D - 0.05) * 100) / 100;
      clampedTo = D;
      if (D <= 0.4) die(2, `노드 ${nodeId}: 방 안에서 성립하는 거리를 찾지 못했다`);
    }
    return {
      nodeId,
      label,
      cameraPose: {
        pos: {
          x: Math.round((target.x - D * Math.sin(rad)) * 1000) / 1000,
          y: Math.round((target.y - D * Math.cos(rad)) * 1000) / 1000,
          z: eyeHeight,
        },
        rot: { pitchDeg: pitch, yawDeg: yaw, rollDeg: roll },
        fovDeg: fovH,
        fovAxis: 'horizontal',
        lookAt: target,
        standDistanceM: Math.round(D * 1000) / 1000,
        distanceClampedToM: clampedTo,
      },
      neighbors: [],
      interactables,
      transitionType: 'cut',
      loadCostMb: null,
      assetStatus,
      _src: {
        'cameraPose.pos.z': `concept/style-guide.md §5 시선 높이 ${eyeHeight} m`,
        'cameraPose.rot': `concept/style-guide.md §5 피치 ${pitch}° · 롤 ${roll}° · 요 ${yawOptions.join('/')}°`,
        'cameraPose.fovDeg': `concept/style-guide.md §5 수평 화각 약 ${fovH}°`,
        'cameraPose.lookAt': srcNote,
        'cameraPose.standDistanceM': `파생 규칙: D = max(w,h)/${FILL_RATIO} / (2·tan(${fovH}°/2)), 방 경계(hub-watchroom §2) 안으로 0.05 m 단위 클램프`,
        transitionType: 'concept/style-guide.md §5 「장면 전환은 노드 컷」 · 모션 축소 시 전부 cut',
        loadCostMb: 'NOT-MEASURED (data-schemas/zones.md §6 — n=0)',
      },
    };
  }

  const deskCenter = { x: Number(desk[4]), y: Number(desk[5]), z: Number(desk[6]) };
  const deskSize = { x: Number(desk[1]), y: Number(desk[2]), z: Number(desk[3]) };
  const shelfCenter = { x: Number(shelf[4]), y: Number(shelf[5]), z: Number(shelf[3]) / 2 };
  const shelfSize = { x: Number(shelf[1]), y: Number(shelf[2]), z: Number(shelf[3]) };

  const nodes = [
    nodeFor({
      nodeId: 'hub-view-desk', label: '작업대 전경',
      target: deskCenter, size: deskSize,
      interactables: ['workbench-standing-slot', 'rec-handover-brief', 'rec-transfer-list'],
      assetStatus: 'greybox',
      srcNote: 'modeling/specs/hub-watchroom.md §2 작업대 중심·상면 z',
    }),
    nodeFor({
      nodeId: 'hub-view-drawer', label: '사물 서랍',
      target: { x: deskCenter.x, y: deskCenter.y, z: Math.round((deskCenter.z / 2) * 1000) / 1000 },
      size: { x: deskSize.x / 2, y: deskSize.y, z: deskCenter.z },
      interactables: ['drawer', 'plate-zero'],
      assetStatus: 'not-modeled',
      srcNote: 'planning/campaign.json t0-b1.subtasks 「서랍에서 판 #0을 꺼내」 — 그레이박스에 서랍 오브젝트 0개(modeling/specs/hub-watchroom.md §2), 작업대 하부로 배치한 [TARGET]',
    }),
    nodeFor({
      nodeId: 'hub-view-reader', label: '판독기',
      target: { x: tools['SM_Tool_reader'].pos.x, y: tools['SM_Tool_reader'].pos.y, z: Math.round((tools['SM_Tool_reader'].pos.z + tools['SM_Tool_reader'].size.z / 2) * 1000) / 1000 },
      size: tools['SM_Tool_reader'].size,
      interactables: ['tool-reader', 'rec-plate-standard-hub', 'rec-tide-ledger-bureau'],
      assetStatus: 'greybox',
      srcNote: 'modeling/asset-manifest.md §2 SM_Tool_reader 배치',
    }),
    nodeFor({
      nodeId: 'hub-view-sealdesk', label: '서명대·봉인대',
      target: { x: tools['SM_Tool_seal'].pos.x, y: tools['SM_Tool_seal'].pos.y, z: Math.round((tools['SM_Tool_seal'].pos.z + tools['SM_Tool_seal'].size.z / 2) * 1000) / 1000 },
      size: tools['SM_Tool_seal'].size,
      interactables: ['tool-seal'],
      assetStatus: 'greybox',
      srcNote: 'modeling/asset-manifest.md §2 SM_Tool_seal 배치',
    }),
    nodeFor({
      nodeId: 'hub-view-plateshelf', label: '염판 선반',
      target: shelfCenter, size: shelfSize,
      interactables: ['plate-shelf'],
      assetStatus: 'greybox',
      srcNote: 'modeling/specs/hub-watchroom.md §2 염판 선반 중심',
    }),
    nodeFor({
      nodeId: 'hub-view-circuitmap', label: '벽 회로 지도',
      target: tools['SM_Tool_circuit'].pos,
      size: tools['SM_Tool_circuit'].size,
      interactables: ['tool-circuit', 'rec-watchlog-bureau'],
      assetStatus: 'greybox',
      srcNote: 'modeling/asset-manifest.md §2 SM_Tool_circuit 배치(북쪽 벽 패널)',
    }),
  ];
  // 순회 링(양방향 · 고아 0 — zones.md Z-I3)
  for (let i = 0; i < nodes.length; i++) {
    const prev = nodes[(i - 1 + nodes.length) % nodes.length].nodeId;
    const next = nodes[(i + 1) % nodes.length].nodeId;
    nodes[i].neighbors = [prev, next];
  }
  for (const n of nodes) {
    for (const nb of n.neighbors) {
      const other = nodes.find((x) => x.nodeId === nb);
      if (!other || !other.neighbors.includes(n.nodeId)) die(2, `Z-I3 위반: ${n.nodeId} ↔ ${nb} 가 양방향이 아니다`);
    }
  }

  // (d) 센서 커버리지 — worldview-bible.md §2 회선 3개소
  const bible2 = section(docs.bible.text, '2. 기록의 물리');
  const lineM = bible2.match(/회선이 깔린 3개소\(([^)]+)\)/);
  if (!lineM) die(2, 'worldview-bible §2 회선 3개소 파싱 실패');
  const lineStations = lineM[1].split('·').map((s) => s.trim());
  if (lineStations.length !== 3) die(2, `회선 개소 수가 3이 아니다: ${lineStations.join(',')}`);
  const resM = bible2.match(/분해능 (\d+)분/);
  if (!resM) die(2, 'worldview-bible §2 분해능 파싱 실패');

  const uncoveredIds = ['hub-uncovered-1', 'hub-uncovered-2', 'hub-uncovered-3'];
  const coverage = [
    {
      areaId: 'hub-indoor', covered: true, systemId: 'hub', hasLine: true,
      _src: `worldview/worldview-bible.md §2 「회선이 깔린 3개소(${lineStations.join('·')})」 — 당직실이 그 하나`,
    },
    ...uncoveredIds.map((areaId, i) => ({
      areaId, covered: false, systemId: null, hasLine: false, displayNameKey: null,
      pendingRfc: 'RFC-N9',
      _src: `planning/campaign.json t0-b2.subtasks[2] 「센서가 닿지 않는 옥외 구획 세 곳」(구획 ${i + 1}/3). **이름은 캐논 미등재** — synopsis/t0-records.md §5.6 이 명명을 거부하고 RFC-N9 로 올렸다. 이 id 는 기술 식별자이며 용어집 명사가 아니다`,
    })),
  ];

  const stageIds = campaign.stages.filter((s) => s.zoneIds.includes('hub')).map((s) => s.id);

  return {
    schemaVersion: 1,
    scope: String(stage.id).toLowerCase(),
    generatedFrom: ['concept/style-guide.md', 'modeling/specs/hub-watchroom.md', 'modeling/asset-manifest.md', 'worldview/worldview-bible.md', 'planning/campaign.json'],
    poseFrame: {
      name: 'greybox-blender-zup-m',
      description: '단위 m · Blender Z-up · 원점 = 방 중앙 바닥면(hub-watchroom §2). 좌표는 실제로 만들어진 그레이박스의 값이다',
      unityConversion: '[INFERENCE] glTF Y-up 경유 임포트 규칙에 따라 Unity 좌표로 변환된다. 변환식을 이 파일이 확정하지 않는다 — 임포터가 hub-greybox.glb 임포트 결과와 대조해 확인한다(인수 T-Z1)',
    },
    cameraConstants: { fovDeg: fovH, fovAxis: 'horizontal', eyeHeightM: eyeHeight, pitchDeg: pitch, rollDeg: roll, yawOptionsDeg: yawOptions, fillRatio: FILL_RATIO, _src: 'concept/style-guide.md §5' },
    roomBounds: { ...bounds, openFace: '-Y', _src: 'modeling/specs/hub-watchroom.md §2' },
    resolutionMinutes: Number(resM[1]),
    notMeasured: ['viewNodes 실제 개수·프레이밍·loadCostMb 는 n=0 (data-schemas/zones.md §6)', 'Unity 임포트 0회 · 카메라 실측 0회'],
    rows: [
      {
        zoneId: 'hub',
        displayNameKey: 'zone.hub.name',
        sceneName: 'hub',
        addressableLabel: null,
        viewNodes: nodes,
        systemIds: ['hub'],
        sensorCoverage: coverage,
        uncoveredAreaIds: uncoveredIds,
        drainEdges: [],
        valves: [],
        protectionAxis: null,
        systemLimits: null,
        initialFloodState: 'dry',
        initialAccessState: 'open',
        stageIds,
        _src: {
          zoneId: 'planning/campaign.json T0.zoneIds = ["hub"]',
          displayNameKey: 'worldview/glossary.md 「당직실 / Watch Room」 — 문자열 직접 저장 금지(zones.md §1)',
          sceneName: 'handoff/codex-unity-brief.md §③ Scenes/ 트리 (boot · ui-root · hub)',
          systemIds: '계통 id 는 구역 토큰을 그대로 쓴다 — 캐논은 계통에 고유명을 주지 않는다(worldview-bible §2 는 채널만 열거). 새 명사 0건이며 고유명이 필요해지면 worldview 판정 대상이다',
          drainEdges: '법4는 T0 범위 밖(campaign 의 routing 도입 비트는 c5-b2) → 빈 배열',
          valves: '허브 계통 밸브 개폐 각인 0건(synopsis/t0-records.md §5.4) → 빈 배열',
          protectionAxis: 'data-schemas/zones.md Z-I5 — lowland·dock 에만 존재',
          systemLimits: 'RFC-P3-009 — 표시 전용이며 현재 데이터에 값 없음(null)',
          initialFloodState: 'planning/campaign.json T0 zoneRun 은 hub 뿐이고 침수 서술 0건',
          initialAccessState: 'planning/campaign.json t0-b1 이 당직실 인수로 시작한다',
          stageIds: 'planning/campaign.json stages[].zoneIds 파생',
        },
      },
    ],
  };
}

// ── tools.json ────────────────────────────────────────────────────────
function buildTools(docs, stage) {
  // 도구 표(§1) 파싱 — 6종 중 T0 는 circuit·reader 2종만 채운다
  const t1 = table(section(docs.toolsSchema.text, '1. 도구 표'));
  const specRows = {};
  for (const [toolId, law, intro, unguided, beatCount, hasCommitDoc, spec] of t1.rows) {
    if (!/^[a-z]+$/.test(toolId)) continue;
    specRows[toolId] = { toolId, lawId: num(law, 'law'), introBeatId: intro, unguidedBeatId: unguided, beatCount: num(beatCount, 'beats'), hasCommitDoc, spec };
  }
  const knobs = Object.fromEntries(table(section(docs.toolsSchema.text, '4. 튜닝 노브')).rows.map((r) => [r[0], { value: r[1], owner: r[2] }]));

  const t0Tools = [...new Set(stage.beats.flatMap((b) => b.tools))];
  const cmd = (commandId, kind, opts = {}) => ({
    commandId, kind,
    validationRules: opts.validationRules || [],
    reasonCodes: opts.reasonCodes || [],
    checkpointBefore: kind === 'commit',
    telemetryKeys: opts.telemetryKeys || [],
    _src: opts._src,
  });

  const defs = {
    circuit: {
      displayNameKey: 'tool.circuit.name',
      panelPrefab: 'ToolPanel_Circuit',
      hasCommit: false,
      requiresPreview: false,
      consumesBudget: 'none',
      sandboxDisplaysOnly: false,
      practiceLayer: { mode: 'always-free', note: '연습층과 확정층이 갈리지 않는다 — 판독 전용 도구이며 확정 명령이 없다', _src: 'system-specs/wiring-trace.md 확정 조건 없음 · interaction-rules.md §2.1' },
      commitCondition: null,
      commands: [
        cmd('TraceSystem', 'inspect', { _src: 'handoff §⑤-3(A) 입력→의도 표 · system-specs/wiring-trace.md §1' }),
        cmd('BeginOverlay', 'edit', { _src: 'system-specs/wiring-trace.md §2 상태기계 Tracing→Overlaying' }),
        cmd('SetOverlayOffset', 'edit', { _src: '동상 · 키보드 파생 D-3(격자 1칸)' }),
        cmd('AnchorOverlay', 'edit', { validationRules: ['W-F2'], reasonCodes: ['ANCHOR_INCOMPLETE'], _src: 'system-specs/wiring-trace.md §4 W-F2' }),
        cmd('ToggleUncovered', 'edit', { _src: 'interaction-rules.md §1-3.2 「circuit = 구획 접기 토글」 · W-R5 무제한 토글' }),
        cmd('QueryEvidenceValidity', 'inspect', { reasonCodes: ['OUT_OF_COVERAGE', 'ZONE_LOCKED'], _src: 'interaction-rules.md §1-3.2 조회 표 — `Q` / 패드 `RS`' }),
        cmd('Cancel', 'edit', { _src: 'system-specs/wiring-trace.md §2 Overlaying→Tracing (상태 손실 0)' }),
      ],
    },
    reader: {
      displayNameKey: 'tool.reader.name',
      panelPrefab: 'ToolPanel_Reader',
      hasCommit: true,
      requiresPreview: true,
      consumesBudget: 'readBudget',
      sandboxDisplaysOnly: true,
      practiceLayer: {
        mode: 'two-layer',
        practice: '사본 재생(Read)은 비용 0 · 횟수 제한 0 — 연습층',
        commit: '인용 고정(CiteToBoard)이 T0 의 유일한 확정 명령이다',
        _src: 'RFC-C7-001 (1) · planning/gdd.md §4 reader 확정 층 · interaction-rules.md §1-3.3',
      },
      commitCondition: {
        commandId: 'CiteToBoard',
        rule: '인용에 sourceType · 계통 · 관측소 출처가 모두 채워졌을 때만 고정된다',
        goesThrough: ['프리뷰', '확정 입력(two-step 기본)', '사전 체크포인트', '저장', '되돌림'],
        _src: 'planning/gdd.md §4 표 reader 확정 층 · RFC-C7-001 (1) · interaction-rules.md §0-4 · §5',
      },
      commands: [
        cmd('LoadRecord', 'inspect', { _src: 'system-specs/plate-readout.md §2 Empty→Loaded' }),
        cmd('SetWindow', 'edit', { _src: '동상 Loaded→Loaded (재생 아님 · 카운터 0)' }),
        cmd('SetZoom', 'edit', { _src: 'handoff §⑤-3(B) 배율 행 · 키보드 파생 D-3(1칸)' }),
        cmd('Read', 'preview', { _src: 'interaction-rules.md §1-3.3 — 사본 재생. 부작용 0 · Space/X 배정' }),
        cmd('ReadOriginal', 'commit', { validationRules: ['P-R9'], reasonCodes: ['ORIGINAL_DEGRADED'], _src: 'interaction-rules.md §1-3.3 — Space 가 아니라 확정 흐름을 거친다. readCounts +1(상한 readBudget)' }),
        cmd('CiteToBoard', 'commit', { validationRules: ['P-F2', 'P-F4'], reasonCodes: ['MEDIA_DUPLICATE', 'INDETERMINATE'], _src: 'RFC-C7-001 (1) — T0 의 확정(인용 고정). interaction-rules.md §1-3.2 「패널 안 Y = reader 인용 고정」' }),
      ],
    },
  };

  const rows = t0Tools.map((toolId) => {
    const s = specRows[toolId];
    if (!s) die(2, `tools.md §1 에 ${toolId} 행이 없다`);
    const d = defs[toolId];
    if (!d) die(2, `이 생성기는 ${toolId} 의 T0 정의를 갖고 있지 않다`);
    return {
      toolId,
      lawId: s.lawId,
      displayNameKey: d.displayNameKey,
      panelPrefab: d.panelPrefab,
      hasCommit: d.hasCommit,
      requiresPreview: d.requiresPreview,
      commitHoldSeconds: Number(String(knobs['commitHoldSeconds'].value).match(/[\d.]+/)[0]),
      introBeatId: s.introBeatId,
      unguidedBeatId: s.unguidedBeatId,
      undoable: true,
      consumesBudget: d.consumesBudget,
      sandboxDisplaysOnly: d.sandboxDisplaysOnly,
      practiceLayer: d.practiceLayer,
      commitCondition: d.commitCondition,
      commands: d.commands,
      _src: {
        lawId: 'systems/data-schemas/tools.md §1 도구 표',
        introBeatId: '동상 (도입 비트)',
        unguidedBeatId: '동상 (미안내 재문제 비트 — T0 범위 밖의 비트를 가리킨다)',
        commitHoldSeconds: `systems/data-schemas/tools.md §4 노브 (기본 ${knobs['commitHoldSeconds'].value}, 소유 ${knobs['commitHoldSeconds'].owner}) — hold opt-in 에서만 쓰인다`,
        undoable: 'systems/data-schemas/tools.md §2 「항상 true」',
      },
    };
  });

  const stubs = Object.keys(specRows)
    .filter((id) => !t0Tools.includes(id))
    .map((id) => ({
      toolId: id, lawId: specRows[id].lawId, stub: true,
      validateReturns: 'NOT_IMPLEMENTED_IN_T0', commitReturns: 'events[0]',
      _src: 'handoff/codex-unity-brief.md §⑤-4 — 나머지 4도구는 인터페이스 스텁만',
    }));

  return {
    schemaVersion: 1,
    scope: String(stage.id).toLowerCase(),
    generatedFrom: ['systems/data-schemas/tools.md', 'systems/system-specs/wiring-trace.md', 'systems/system-specs/plate-readout.md', 'planning/campaign.json'],
    knobs,
    invariantsChecked: {
      'T-I1': `toolId 중복 0 · 6종 등록(구현 ${rows.length} + 스텁 ${stubs.length})`,
      'T-I3': rows.every((r) => !r.hasCommit || (r.requiresPreview && r.commands.filter((c) => c.kind === 'commit').every((c) => c.checkpointBefore)))
        ? 'PASS' : 'FAIL',
      'T-I4': rows.every((r) => r.undoable) ? 'PASS' : 'FAIL',
      'T-I7': rows.every((r) => r.consumesBudget === 'none' || r.sandboxDisplaysOnly) ? 'PASS' : 'FAIL',
    },
    notMeasured: ['도구 전환 시간 · 패널 열기 지연 · 조작감 n=0 (tools.md §6)'],
    rows,
    stubs,
  };
}

// ── beats.json (t0 서브셋 + 완료 술어) ────────────────────────────────
function buildBeats(stage, records, zones) {
  const { report: pairReport } = runValidator(['--pairs']);
  const pairOf = (beatId) => (pairReport.pairs || []).find((p) => p.beat === beatId) || null;
  const recordByOrigin = Object.fromEntries(records.rows.map((r) => [r.originId, r]));
  const uncovered = zones.rows[0].uncoveredAreaIds;

  const predicates = {
    't0-b1': (beat) => ({
      kind: 'viewing',
      emitsCommitCommand: false,
      requires: [
        { type: 'recordLinesViewed', recordId: 'rec-handover-brief', lineIds: ['hb-l1', 'hb-l2', 'hb-l3'], _src: '「인수 각서 3항 … 열람」 · records §3.1 clause 3행' },
        { type: 'recordRowsViewed', recordId: 'rec-transfer-list', rowIds: ['tl-r1', 'tl-r2', 'tl-r3'], _src: '「이관 목록 3줄 … 열람」 · records §4.1 no 1~3' },
        { type: 'slotLoaded', slotId: 'workbench-standing-slot', recordId: 'plate-zero', _src: '「판 #0 이 작업대 상시 슬롯에 적재」 · worldview/glossary.md 「상시 슬롯」' },
        { type: 'decisionRecorded', decisionId: 'transfer-list-entry', rowId: 'tl-r4', values: ['written', 'blank'], atLeastOnce: true, reversible: true, _src: '「처리 여부(기입 또는 공란)가 한 번 기록」 · records §4.1 tl-r4 공란' },
      ],
      _srcCompletion: beat.completion,
    }),
    't0-b2': (beat) => ({
      kind: 'marking',
      emitsCommitCommand: false,
      requires: [
        { type: 'uncoveredAreasMarked', zoneId: 'hub', areaIds: uncovered, count: uncovered.length, _src: '「회로 지도에 음영 3구획이 모두 지정」 · zones.uncoveredAreaIds' },
        { type: 'eachAreaHasEvidence', mediumsPerArea: 1, _src: '「각 구획에 근거 매체가 하나씩 붙는다」' },
      ],
      _srcCompletion: beat.completion,
    }),
    't0-b3': (beat) => {
      const pair = pairOf(beat.id);
      if (!pair) die(2, `검증기 --pairs 에 ${beat.id} 독립쌍이 없다 — proofRequired 와 어긋난다`);
      return {
        kind: 'citationPinned',
        emitsCommitCommand: true,
        commitCommandId: 'CiteToBoard',
        checkpointBefore: true,
        requires: [
          ...pair.independentPair.map((c) => ({
            type: 'citationPinned',
            clueId: c.clueId,
            sourceType: c.sourceType,
            originId: c.originId,
            rootOriginId: c.rootOriginId,
            recordId: recordByOrigin[c.originId] ? recordByOrigin[c.originId].recordId : null,
            _src: 'planning/validate-campaign.mjs --pairs (C-07 독립쌍 출력 · 손으로 고르지 않음)',
          })),
          { type: 'independentPair', rule: 'sourceType 상이 AND rootOriginId 상이', validatorCheck: 'C-07', _src: 'interaction-rules.md §3 · RFC-S5(예외 없음)' },
          { type: 'autoCopyCreated', count: 1, recordId: 'rec-plate-standard-hub', _src: '「첫 판독의 검증 사본 1점이 증거함에 생성」 · plate-readout.md P-R1' },
          { type: 'gapEndpointsFixed', recordId: 'rec-plate-standard-hub', startPhase: 'H-1:00', endPhase: 'H+3:00', _src: 't0-b3.consequence 「결손 구간 H-1:00~H+3:00(정확히 4시간)」 · records §5.2 MISSING' },
        ],
        _srcCompletion: beat.completion,
      };
    },
  };

  const rows = stage.beats.map((b) => {
    const mk = predicates[b.id];
    if (!mk) die(2, `완료 술어가 정의되지 않은 비트: ${b.id}`);
    return { ...b, completionPredicate: mk(b) };
  });

  const commitBeats = rows.filter((r) => r.completionPredicate.emitsCommitCommand).map((r) => r.id);
  const proofBeats = stage.beats.filter((b) => b.proofRequired).map((b) => b.id);
  for (const id of proofBeats) {
    if (!commitBeats.includes(id)) die(2, `proofRequired 비트 ${id} 에 확정 명령이 없다 — DoD 6·8 이 검사할 대상이 사라진다`);
  }

  return {
    schemaVersion: 1,
    scope: String(stage.id).toLowerCase(),
    generatedFrom: 'planning/campaign.json',
    sourceSha256: sourceSha,
    derivation: 'subset+predicate',
    stage: { id: stage.id, title: stage.title, zoneIds: stage.zoneIds, storyPhase: stage.storyPhase, minutes: stage.minutes },
    commitCommandBeats: commitBeats,
    proofRequiredBeats: proofBeats,
    notMeasured: ['이 술어가 실제 플레이에서 판정되는지 n=0 — Unity 실행 0회'],
    rows,
  };
}

// ══════════════════════════════════════════════════════════════════════
//  출력
// ══════════════════════════════════════════════════════════════════════

const buf = (obj) => Buffer.from(JSON.stringify(obj, null, 2) + '\n', 'utf8');

let tables;
let t0 = null;
if (scope === 'all') {
  tables = [
    { file: 'beats.json', derivation: 'copy', buf: sourceBuf, rows: report.aggregates.beats },
    { file: 'hints.json', derivation: 'projection', buf: buf({ schemaVersion: 1, scope: 'all', generatedFrom: 'planning/campaign.json', sourceSha256: sourceSha, rows: hintRowsFor(campaign.stages) }), rows: hintRowsFor(campaign.stages).length },
  ];
} else {
  t0 = buildT0(scope);
  tables = [
    { file: 'beats.json', derivation: 'subset+predicate', buf: buf(t0.beats), rows: t0.beats.rows.length },
    { file: 'hints.json', derivation: 'projection', buf: buf(t0.hints), rows: t0.hints.rows.length },
    { file: 'tools.json', derivation: 'schema-projection', buf: buf(t0.tools), rows: t0.tools.rows.length },
    { file: 'zones.json', derivation: 'authoring-derivation', buf: buf(t0.zones), rows: t0.zones.rows.length },
    { file: 'records.json', derivation: 'markdown-parse+rule', buf: buf(t0.records), rows: t0.records.rows.length },
  ];
}

const receipt = {
  emitter: 'systems/pipeline/emit-tables.mjs',
  emitterOwner: 'game-systems-designer',
  emittedUtc: new Date().toISOString(),
  scope,
  scopeSource,
  dryRun: OUT === null,
  source: { path: 'planning/campaign.json', sha256: sourceSha, bytes: sourceBuf.length },
  validator: {
    path: 'planning/validate-campaign.mjs',
    owner: 'game-planner',
    exitCode: validatorExit,
    checks: report.summary.checks,
    pass: report.summary.pass,
    fail: report.summary.fail,
    verdict: report.summary.verdict,
  },
  aggregates: {
    stages: report.aggregates.stages,
    beats: report.aggregates.beats,
    clues: report.aggregates.clues,
    sourceTypeDist: report.aggregates.sourceTypeDist,
    toolBeatCounts: report.aggregates.toolBeatCounts,
    zoneBeatCounts: report.aggregates.zoneBeatCounts,
    proofRequiredBeats: report.aggregates.proofRequiredBeats,
    totalMinutes: report.aggregates.totalMinutes,
  },
  tables: tables.map((t) => ({ file: t.file, derivation: t.derivation, sha256: sha(t.buf), bytes: t.buf.length, rows: t.rows })),
  importerContract: {
    mustVerify: [
      'validator.verdict === "PASS"',
      ...tables.map((t) => `sha256(${t.file}) === receipt.tables[${t.file}].sha256`),
      'receipt.source.sha256 === sha256(저작 원본) — 원본에 접근 가능할 때만',
    ],
    mustReimplement: [
      'R-1 로컬라이즈 미해결 키 0건 — **KO 필수 · EN 선택**(RFC-S6). 용어집 등재 여부만 fail-closed',
      'R-2 고아 0건 (Unity 에셋 참조 그래프가 있어야 판정 가능)',
      'R-3 독립쌍 중 파괴 불가 경로 ≥ 1 (런타임 규칙)',
      'R-4 임의 도달 상태에서 엔딩 3종 도달 (런타임 상태공간 탐색)',
    ],
    mustNotReimplement: '저작 시점 검사 전건(검증기 checks). C# 으로 다시 쓰면 두 구현이 갈라진다',
    ignoredFields: ['emittedUtc'],
  },
  notMeasured: [
    'Unity 임포트 0회 · 빌드 0회 · 사람 플레이 표본 n=0',
    '이 영수증은 데이터 정합만 증명한다. 어떤 게이트도 PASS 로 올리지 않는다',
  ],
};

if (scope !== 'all') {
  receipt.authoringSources = Object.fromEntries(Object.entries(t0.docs).map(([k, v]) => [k, { path: v.path, sha256: v.sha256 }]));
  receipt.crossChecks = t0.records.crossChecks;
  receipt.toolInvariants = t0.tools.invariantsChecked;
}

function metaFor(t, outDir) {
  const rel = `${outDir.replace(/^.*_workspace\/current\//, '')}/${t.file}`;
  const cmd = `node _workspace/current/systems/pipeline/emit-tables.mjs --out ${outDir.replace(/^.*_workspace\//, '_workspace/')}`;
  const srcLines = (receipt.authoringSources
    ? Object.values(receipt.authoringSources).map((s) => `| \`${s.path}\` | \`${s.sha256.slice(0, 12)}…\` |`)
    : []).join('\n');
  return Buffer.from(
    `---
updated: ${DOC_DATE}
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
describes: _workspace/current/${rel}
generated: true
---

# \`${t.file}\` 메타 — T0 인스턴스 데이터 (RFC-C7-001 · C7-F1)

**이 파일도 \`${t.file}\` 도 손으로 쓰지 않는다.** 둘 다 아래 명령의 출력이며, 값의 출처는 각 필드 옆 \`_src\` 가 갖는다.

## 1. 생성 명령

\`\`\`
${cmd}
\`\`\`

## 2. 이번 출력 [OBSERVED ${DOC_DATE}]

| 항목 | 값 |
|---|---|
| 파생 | \`${t.derivation}\` |
| sha256 | \`${sha(t.buf)}\` |
| 바이트 (\`wc -c\`) | ${t.buf.length} |
| 행 수 | ${t.rows} |
| 검증기 판정 | \`verdict ${receipt.validator.verdict} · checks ${receipt.validator.checks} · fail ${receipt.validator.fail}\` (exit ${receipt.validator.exitCode}) |

> 해시·바이트는 저작 원본이 바뀌면 달라진다. 인용할 때 옮겨 적지 말고 생성기를 다시 돌린다(RFC-Q1).

## 3. 저작 출처 (이번 실행이 읽은 파일)

| 파일 | sha256(앞 12) |
|---|---|
| \`planning/campaign.json\` | \`${sourceSha.slice(0, 12)}…\` |
${srcLines}

## 4. runtime 관측

**NOT-MEASURED.** Unity 임포트 0회 · 빌드 0회 · 플레이 표본 n=0 · 프레임타임 캡처 0건.
이 파일은 데이터 정합만 증명하며 어떤 게이트도 올리지 않는다.
`,
    'utf8',
  );
}

if (OUT) {
  mkdirSync(OUT, { recursive: true });
  for (const t of tables) writeFileSync(join(OUT, t.file), t.buf);
  writeFileSync(join(OUT, 'tables-receipt.json'), JSON.stringify(receipt, null, 2) + '\n');
  if (scope !== 'all') {
    for (const t of tables) writeFileSync(join(OUT, t.file.replace(/\.json$/, '.meta.md')), metaFor(t, OUT));
    writeFileSync(join(OUT, 'tables-receipt.meta.md'), metaFor({ file: 'tables-receipt.json', derivation: 'receipt', buf: Buffer.from(JSON.stringify(receipt, null, 2) + '\n', 'utf8'), rows: tables.length }, OUT));
  }
}
process.stdout.write(JSON.stringify(receipt, null, 2) + '\n');
process.exit(0);
