/*
 * model.mjs — 상호작용 검증 모형 (순수 리듀서)
 * cycle: 20260909-preproduction-c4 · owner: game-systems-designer
 *
 * 이 파일은 게임이 아니다. Unity 빌드도, 데모도, 세로 슬라이스도 아니다.
 * `_workspace/current/systems/interaction-rules.md` 의 확정 조건이 서로 모순 없이
 * 성립하는지를 실행으로 확인하기 위한 **가상의 훈련용 작업대 1개**를 모형화한다.
 * 재미·연출·플레이 시간에 대해 아무것도 주장하지 않는다.
 *
 * 의존성 0개. import 없음. HTML 생성기와 테스트가 이 파일 하나를 그대로 공유한다.
 */

export const SCHEMA_VERSION = 1;
export const MODEL_ID = 'tide-training-bench';
export const BANNER = '상호작용 검증 모형 · Unity 빌드 아님 · 플레이시간 미측정';

/* ------------------------------------------------------------------ *
 * 1. 고정 설정 — 훈련용 작업대 1개의 경계
 * ------------------------------------------------------------------ */

export const MEDIA = Object.freeze([
  Object.freeze({
    id: 'plate-archive', label: '기록국 표준 염판(원본)', medium: 'plate', mediumLabel: '염판',
    originId: 'ORG-A', originLabel: '기록국 표준 계통', wired: true,
    clueId: 'clue-gap-4h', clueLabel: '결손 4시간 구간', clueMandatory: true,
  }),
  Object.freeze({
    id: 'plate-archive-scan', label: '같은 판의 복제 스캔', medium: 'plate', mediumLabel: '염판',
    originId: 'ORG-A', originLabel: '기록국 표준 계통', wired: true,
    clueId: 'clue-gap-4h', clueLabel: '결손 4시간 구간', clueMandatory: true,
  }),
  Object.freeze({
    id: 'plate-dock', label: '부두사무소 염판', medium: 'plate', mediumLabel: '염판',
    originId: 'ORG-B', originLabel: '부두 계통', wired: true,
    clueId: 'clue-dock-peak', clueLabel: '부두 압력 봉우리', clueMandatory: true,
  }),
  Object.freeze({
    id: 'ledger-dock', label: '부두 조위대장', medium: 'ledger', mediumLabel: '조위대장',
    originId: 'ORG-C', originLabel: '부두 관측소', wired: true,
    clueId: 'clue-ledger-peaks', clueLabel: '만조 피크 3개', clueMandatory: true,
  }),
  Object.freeze({
    id: 'log-outdoor', label: '옥외 당직일지', medium: 'log', mediumLabel: '당직일지',
    originId: 'ORG-D', originLabel: '옥외(배선 범위 밖)', wired: false,
    clueId: 'clue-outdoor', clueLabel: '옥외 메모', clueMandatory: false,
  }),
]);

/** 정합용 공통 피크 후보. delta = 대장시각 − 염판시각(분). pX 는 서로 다른 사건의 거짓 대응. */
export const PEAKS = Object.freeze([
  Object.freeze({ id: 'p1', label: '만조 1', plateMin: 120, ledgerMin: 158, real: true }),
  Object.freeze({ id: 'p2', label: '만조 2', plateMin: 492, ledgerMin: 533, real: true }),
  Object.freeze({ id: 'p3', label: '만조 3', plateMin: 864, ledgerMin: 899, real: true }),
  Object.freeze({ id: 'pX', label: '봉우리 X(대응 불명)', plateMin: 300, ledgerMin: 301, real: false }),
]);

export const EVENT_PAIRS = Object.freeze([
  Object.freeze({ id: 'pair-20', label: '밸브 개폐 각인 ↔ 봉인 완료 접점 각인', gapMin: 20 }),
  Object.freeze({ id: 'pair-6', label: '대장 기입 두 건', gapMin: 6 }),
]);

export const ROUTES = Object.freeze([
  Object.freeze({ id: 'lowland', label: '저지대 주거지 우선', corrosion: 7, protects: 'lowland' }),
  Object.freeze({ id: 'dock', label: '부두 냉동창고 우선', corrosion: 8, protects: 'dock' }),
  Object.freeze({ id: 'dock-express', label: '부두 급속 증설안', corrosion: 12, protects: 'dock' }),
]);

export const ENDINGS = Object.freeze([
  Object.freeze({ id: 'record', label: '기록 무결성 관점' }),
  Object.freeze({ id: 'residents', label: '주민 청구 관점' }),
  Object.freeze({ id: 'operations', label: '운영 연속성 관점' }),
]);

export const LIMITS = Object.freeze({
  requiredPeaks: 3,
  residualLimitMin: 4,
  stationUncertaintyMin: 4,
  combinedUncertaintyMin: 8,
  corrosionLimit: 9,
  offsetMin: -45,
  offsetMax: 45,
  maxUndo: 32,
  sealSlots: 2,
});

export const STEPS = Object.freeze([
  Object.freeze({ n: 1, id: 'read', title: '판독 · 사본 보존', asks: '어떤 매체를 판독할 것인가' }),
  Object.freeze({ n: 2, id: 'wiring', title: '배선 범위 확인', asks: '어떤 근거가 센서 밖인가' }),
  Object.freeze({ n: 3, id: 'align', title: '조위정합(공통 피크 3)', asks: '잔차를 4분 이하로 내릴 수 있는가' }),
  Object.freeze({ n: 4, id: 'order', title: '선후 판정', asks: '오차폭 8분보다 간격이 큰가' }),
  Object.freeze({ n: 5, id: 'route', title: '경로 프리뷰 · 확정', asks: '무엇을 보호하고 무엇을 감수하는가' }),
  Object.freeze({ n: 6, id: 'seal', title: '이중서명 · 관점 제출', asks: '독립 매체 2종이 서 있는가' }),
]);

const MEDIA_BY_ID = new Map(MEDIA.map((m) => [m.id, m]));
const PEAK_BY_ID = new Map(PEAKS.map((p) => [p.id, p]));
const ROUTE_BY_ID = new Map(ROUTES.map((r) => [r.id, r]));
const ENDING_IDS = ENDINGS.map((e) => e.id);
export const MANDATORY_CLUE_IDS = Object.freeze(
  [...new Set(MEDIA.filter((m) => m.clueMandatory).map((m) => m.clueId))].sort(),
);

export function mediaById(id) { return MEDIA_BY_ID.get(id) || null; }
export function routeById(id) { return ROUTE_BY_ID.get(id) || null; }

/* ------------------------------------------------------------------ *
 * 2. 차단 사유 — 코드마다 사람이 읽는 한국어 문장이 붙는다
 * ------------------------------------------------------------------ */

export const REASONS = Object.freeze({
  UNKNOWN_ACTION: '알 수 없는 조작이다.',
  UNKNOWN_TARGET: '작업대에 없는 대상이다.',
  NOT_READ: '판독 사본이 없다. 1단계에서 먼저 판독해야 한다.',
  ALREADY_READ: '이미 판독해 사본이 보존되어 있다.',
  PEAK_LIMIT: `공통 피크는 ${LIMITS.requiredPeaks}개까지만 물릴 수 있다.`,
  PEAK_COUNT_SHORT: `공통 피크가 ${LIMITS.requiredPeaks}개 미만이다. 두 점만 맞추면 축이 기울어 잔차가 남는다.`,
  RESIDUAL_OVER: `정합 후 잔차가 ${LIMITS.residualLimitMin}분을 넘는다. 기준선을 확정할 수 없다.`,
  ALIGN_NOT_PINNED: '기준선이 확정되지 않았다. 정합 전 시계는 믿지 않는다.',
  ALIGN_LOCKED: '기준선이 확정된 뒤에는 피크·오프셋을 바꾸기 전에 고정핀을 먼저 뽑아야 한다.',
  OFFSET_RANGE: `오프셋은 ${LIMITS.offsetMin}~${LIMITS.offsetMax}분 범위여야 한다.`,
  ORDER_NOT_JUDGED: '선후 판정을 아직 실행하지 않았다.',
  NO_ROUTE: '경로가 선택되지 않았다.',
  NO_PREVIEW: '프리뷰를 확인하지 않았다. 확정 전 항상 프리뷰다.',
  CORROSION_OVER: '부식예산 한도를 초과했다. 초과 구성은 확정 전에 막힌다.',
  ALREADY_COMMITTED: '이미 확정되어 체크포인트가 생성됐다. 되돌림으로만 바꾼다.',
  ROUTE_LOCKED: '확정된 뒤에는 경로를 바꿀 수 없다. 되돌림을 쓰라.',
  SEAL_FULL: `근거 슬롯은 ${LIMITS.sealSlots}개뿐이다.`,
  SEAL_INCOMPLETE: `근거 슬롯 ${LIMITS.sealSlots}개가 모두 차야 한다.`,
  SEAL_LOCKED: '서명한 뒤에는 슬롯을 바꿀 수 없다. 서명을 먼저 취소하라.',
  SAME_ORIGIN: '같은 출처의 사본이다. 원본·표면 부식·복제 스캔은 모두 출처 1개로 센다.',
  SAME_MEDIUM: '같은 매체 종류다. 서로 다른 매체 2종이 필요하다.',
  OUT_OF_WIRING: '배선 범위 밖 근거다. 기록의 침묵은 부재의 증거가 아니며 확정 근거도 아니다.',
  WIRING_UNCHECKED: '배선 범위를 아직 확인하지 않았다. 2단계를 먼저 끝내라.',
  ALREADY_SIGNED: '이미 서명되어 있다.',
  NOT_SIGNED: '아직 서명되지 않았다.',
  NOTHING_TO_UNDO: '되돌릴 이전 상태가 없다.',
  NO_AUTOSAVE: '메모리에 보관된 자동 저장이 없다.',
  SCHEMA_MISSING: '스키마 버전이 없다. 이 파일은 이 모형의 저장 형식이 아니다.',
  SCHEMA_TOO_NEW: '등록되지 않은 상위 스키마다. 거부하고 파일을 그대로 둔다.',
  SCHEMA_INVALID: '저장 형식이 손상됐다(필드 누락 또는 형식 불일치).',
  CHECKSUM_FAILED: '체크섬이 맞지 않는다. 손상 파일은 덮어쓰지 않는다.',
});

function block(code) { return { ok: false, code, reason: REASONS[code] || code }; }
function pass(extra) { return Object.assign({ ok: true, code: null, reason: null }, extra || {}); }

/* ------------------------------------------------------------------ *
 * 3. 상태
 * ------------------------------------------------------------------ */

export function initialCore() {
  return {
    copies: [],            // 판독한 매체 id (정렬)
    keptClues: [],         // 보존된 단서 id (정렬) — 어떤 조작으로도 줄지 않는다
    wiringChecked: false,
    peaks: [],             // 물린 공통 피크 id (정렬)
    offsetMin: 0,          // 시간축 오프셋(분)
    aligned: false,        // 고정핀
    orderJudged: false,
    route: null,
    previewed: false,
    committed: false,
    propertyProtection: null, // null | 'lowland' | 'dock'  (§0.5 이진 플래그)
    seal: [],              // 근거 슬롯에 놓인 매체 id (정렬, 최대 2)
    signed: false,
    ending: null,
  };
}

function cloneCore(c) {
  return {
    copies: c.copies.slice(), keptClues: c.keptClues.slice(), wiringChecked: c.wiringChecked,
    peaks: c.peaks.slice(), offsetMin: c.offsetMin, aligned: c.aligned, orderJudged: c.orderJudged,
    route: c.route, previewed: c.previewed, committed: c.committed,
    propertyProtection: c.propertyProtection, seal: c.seal.slice(), signed: c.signed, ending: c.ending,
  };
}

/** 필수 단서는 어떤 조작으로도 사라지지 않는다 (interaction-rules §0.3). */
function preserveClues(next, prev) {
  const union = new Set(next.keptClues);
  for (const id of prev.keptClues) union.add(id);
  next.keptClues = [...union].sort();
  const copies = new Set(next.copies);
  for (const id of prev.copies) copies.add(id); // 사본도 남는다 (불가침 법2)
  next.copies = [...copies].sort();
  return next;
}

/* ------------------------------------------------------------------ *
 * 4. 파생 판정 — UI 와 테스트가 같은 함수를 쓴다
 * ------------------------------------------------------------------ */

export function deltasFor(peakIds) {
  return peakIds.map((id) => {
    const p = PEAK_BY_ID.get(id);
    return p ? p.ledgerMin - p.plateMin : 0;
  });
}

/** 잔차 = 선택한 피크들의 (대장−염판) 편차 중 오프셋에서 가장 먼 값. */
export function residualFor(peakIds, offsetMin) {
  if (peakIds.length === 0) return null;
  return Math.max(...deltasFor(peakIds).map((d) => Math.abs(d - offsetMin)));
}

/** 자동 맞춤: 편차 중앙범위. 피크가 없으면 0. */
export function autoFitOffset(peakIds) {
  if (peakIds.length === 0) return 0;
  const d = deltasFor(peakIds);
  return Math.round((Math.max(...d) + Math.min(...d)) / 2);
}

export function alignmentStatus(core) {
  const residual = residualFor(core.peaks, core.offsetMin);
  const detail = { peakCount: core.peaks.length, residualMin: residual, pinned: core.aligned };
  if (!core.copies.includes('plate-archive') || !core.copies.includes('ledger-dock')) {
    return Object.assign(block('NOT_READ'), detail);
  }
  if (core.peaks.length < LIMITS.requiredPeaks) return Object.assign(block('PEAK_COUNT_SHORT'), detail);
  if (residual > LIMITS.residualLimitMin) return Object.assign(block('RESIDUAL_OVER'), detail);
  return Object.assign(pass(), detail);
}

/** 선후 판정: 간격이 두 관측소 오차폭 합(8분)보다 클 때만 확정한다. */
export function verdictFor(gapMin) {
  return gapMin > LIMITS.combinedUncertaintyMin ? 'ordered' : 'unknown';
}

export function orderStatus(core) {
  if (!core.aligned) return Object.assign(block('ALIGN_NOT_PINNED'), { verdicts: null });
  const verdicts = EVENT_PAIRS.map((p) => ({
    id: p.id, label: p.label, gapMin: p.gapMin,
    verdict: core.orderJudged ? verdictFor(p.gapMin) : null,
  }));
  return Object.assign(pass(), { verdicts });
}

export function commitStatus(core) {
  const r = core.route ? ROUTE_BY_ID.get(core.route) : null;
  const detail = {
    corrosion: r ? r.corrosion : null, limit: LIMITS.corrosionLimit,
    protects: r ? r.protects : null,
  };
  if (core.committed) return Object.assign(block('ALREADY_COMMITTED'), detail);
  if (!r) return Object.assign(block('NO_ROUTE'), detail);
  if (!core.previewed) return Object.assign(block('NO_PREVIEW'), detail);
  if (r.corrosion > LIMITS.corrosionLimit) return Object.assign(block('CORROSION_OVER'), detail);
  return Object.assign(pass(), detail);
}

/** 두 근거의 독립성. 같은 출처 사본은 세지 않고, 같은 매체 종류도 세지 않는다. */
export function independencePair(aId, bId) {
  const a = MEDIA_BY_ID.get(aId); const b = MEDIA_BY_ID.get(bId);
  if (!a || !b) return block('UNKNOWN_TARGET');
  if (a.originId === b.originId) return block('SAME_ORIGIN');
  if (a.medium === b.medium) return block('SAME_MEDIUM');
  return pass();
}

export function sealStatus(core) {
  const slots = core.seal.map((id) => {
    const m = MEDIA_BY_ID.get(id);
    return { id, label: m.label, mediumLabel: m.mediumLabel, originLabel: m.originLabel, wired: m.wired };
  });
  const detail = { slots, signed: core.signed };
  if (core.seal.length < LIMITS.sealSlots) return Object.assign(block('SEAL_INCOMPLETE'), detail);
  for (const id of core.seal) {
    if (!core.copies.includes(id)) return Object.assign(block('NOT_READ'), detail);
  }
  if (!core.wiringChecked) return Object.assign(block('WIRING_UNCHECKED'), detail);
  for (const id of core.seal) {
    if (!MEDIA_BY_ID.get(id).wired) return Object.assign(block('OUT_OF_WIRING'), detail);
  }
  const ind = independencePair(core.seal[0], core.seal[1]);
  if (!ind.ok) return Object.assign(block(ind.code), detail);
  if (!core.aligned) return Object.assign(block('ALIGN_NOT_PINNED'), detail); // 시간 근거 조항
  return Object.assign(pass(), detail);
}

/**
 * 잠기는 엔딩은 없다. 재산 보호 플래그는 후일담 텍스트만 바꾸고
 * 엔딩 3종 접근성을 건드리지 않는다 (interaction-rules §0.5 / §0.6).
 */
export function lockedEndings(_core) { return []; }

export function selectableEndings(core) {
  const locked = new Set(lockedEndings(core));
  return ENDINGS.filter((e) => !locked.has(e.id));
}

export function summary(core) {
  const a = alignmentStatus(core);
  return {
    copies: core.copies.length,
    keptClues: core.keptClues.length,
    mandatoryKept: MANDATORY_CLUE_IDS.filter((c) => core.keptClues.includes(c)).length,
    mandatoryTotal: MANDATORY_CLUE_IDS.length,
    wiringChecked: core.wiringChecked,
    peakCount: core.peaks.length,
    residualMin: a.residualMin,
    aligned: core.aligned,
    orderJudged: core.orderJudged,
    route: core.route,
    committed: core.committed,
    propertyProtection: core.propertyProtection,
    signed: core.signed,
    ending: core.ending,
    selectableEndings: selectableEndings(core).length,
  };
}

/* ------------------------------------------------------------------ *
 * 5. 순수 리듀서 — 차단은 예외를 던지지 않고 사유를 돌려준다
 * ------------------------------------------------------------------ */

export function reduce(core, action) {
  const a = action || {};
  const blocked = (code) => ({ core, ok: false, code, reason: REASONS[code] || code, changed: false });
  const done = (next) => ({ core: next, ok: true, code: null, reason: null, changed: true });

  switch (a.type) {
    case 'read': {
      const m = MEDIA_BY_ID.get(a.mediaId);
      if (!m) return blocked('UNKNOWN_TARGET');
      if (core.copies.includes(m.id)) return blocked('ALREADY_READ');
      const next = cloneCore(core);
      next.copies = [...core.copies, m.id].sort();
      if (!next.keptClues.includes(m.clueId)) next.keptClues = [...next.keptClues, m.clueId].sort();
      return done(next);
    }
    case 'checkWiring': {
      if (core.wiringChecked) return blocked('UNKNOWN_ACTION');
      const next = cloneCore(core); next.wiringChecked = true; return done(next);
    }
    case 'togglePeak': {
      if (!PEAK_BY_ID.has(a.peakId)) return blocked('UNKNOWN_TARGET');
      if (core.aligned) return blocked('ALIGN_LOCKED');
      const next = cloneCore(core);
      if (core.peaks.includes(a.peakId)) next.peaks = core.peaks.filter((p) => p !== a.peakId);
      else {
        if (core.peaks.length >= LIMITS.requiredPeaks) return blocked('PEAK_LIMIT');
        next.peaks = [...core.peaks, a.peakId].sort();
      }
      return done(next);
    }
    case 'setOffset': {
      if (core.aligned) return blocked('ALIGN_LOCKED');
      const v = Number(a.offsetMin);
      if (!Number.isInteger(v) || v < LIMITS.offsetMin || v > LIMITS.offsetMax) return blocked('OFFSET_RANGE');
      if (v === core.offsetMin) return blocked('UNKNOWN_ACTION');
      const next = cloneCore(core); next.offsetMin = v; return done(next);
    }
    case 'autoFit': {
      if (core.aligned) return blocked('ALIGN_LOCKED');
      const v = autoFitOffset(core.peaks);
      if (v === core.offsetMin) return blocked('UNKNOWN_ACTION');
      const next = cloneCore(core); next.offsetMin = v; return done(next);
    }
    case 'pinAlignment': {
      if (core.aligned) return blocked('UNKNOWN_ACTION');
      const st = alignmentStatus(core);
      if (!st.ok) return blocked(st.code);
      const next = cloneCore(core); next.aligned = true; return done(next);
    }
    case 'unpinAlignment': {
      if (!core.aligned) return blocked('UNKNOWN_ACTION');
      const next = cloneCore(core);
      next.aligned = false; next.orderJudged = false;
      if (core.signed) next.signed = false; // 시간 근거가 풀리면 서명도 선다
      return done(next);
    }
    case 'judgeOrder': {
      if (!core.aligned) return blocked('ALIGN_NOT_PINNED');
      if (core.orderJudged) return blocked('UNKNOWN_ACTION');
      const next = cloneCore(core); next.orderJudged = true; return done(next);
    }
    case 'selectRoute': {
      if (!ROUTE_BY_ID.has(a.routeId)) return blocked('UNKNOWN_TARGET');
      if (core.committed) return blocked('ROUTE_LOCKED');
      if (core.route === a.routeId) return blocked('UNKNOWN_ACTION');
      const next = cloneCore(core);
      next.route = a.routeId; next.previewed = false; // 경로가 바뀌면 프리뷰는 무효
      return done(next);
    }
    case 'clearRoute': {
      if (core.committed) return blocked('ROUTE_LOCKED');
      if (core.route === null) return blocked('UNKNOWN_ACTION');
      const next = cloneCore(core); next.route = null; next.previewed = false; return done(next);
    }
    case 'preview': {
      if (core.committed) return blocked('ALREADY_COMMITTED');
      if (!core.route) return blocked('NO_ROUTE');
      if (core.previewed) return blocked('UNKNOWN_ACTION');
      const next = cloneCore(core); next.previewed = true; return done(next);
    }
    case 'commitRoute': {
      const st = commitStatus(core);
      if (!st.ok) return blocked(st.code);
      const next = cloneCore(core);
      next.committed = true;
      next.propertyProtection = ROUTE_BY_ID.get(core.route).protects;
      return done(next);
    }
    case 'toggleSeal': {
      const m = MEDIA_BY_ID.get(a.mediaId);
      if (!m) return blocked('UNKNOWN_TARGET');
      if (core.signed) return blocked('SEAL_LOCKED');
      const next = cloneCore(core);
      if (core.seal.includes(m.id)) { next.seal = core.seal.filter((s) => s !== m.id); return done(next); }
      if (!core.copies.includes(m.id)) return blocked('NOT_READ');
      if (core.seal.length >= LIMITS.sealSlots) return blocked('SEAL_FULL');
      next.seal = [...core.seal, m.id].sort();
      return done(next);
    }
    case 'sign': {
      if (core.signed) return blocked('ALREADY_SIGNED');
      const st = sealStatus(core);
      if (!st.ok) return blocked(st.code);
      const next = cloneCore(core); next.signed = true; return done(next);
    }
    case 'cancelSign': {
      if (!core.signed) return blocked('NOT_SIGNED');
      const next = cloneCore(core); next.signed = false; return done(next);
    }
    case 'selectEnding': {
      if (!ENDING_IDS.includes(a.endingId)) return blocked('UNKNOWN_TARGET');
      if (!selectableEndings(core).some((e) => e.id === a.endingId)) return blocked('UNKNOWN_TARGET');
      if (core.ending === a.endingId) return blocked('UNKNOWN_ACTION');
      const next = cloneCore(core); next.ending = a.endingId; return done(next);
    }
    case 'clearEnding': {
      if (core.ending === null) return blocked('UNKNOWN_ACTION');
      const next = cloneCore(core); next.ending = null; return done(next);
    }
    default:
      return blocked('UNKNOWN_ACTION');
  }
}

/** BFS 가 쓰는 유한한 조작 알파벳. 무효 분기를 일부러 포함한다. */
export function actionAlphabet() {
  const acts = [];
  for (const m of MEDIA) acts.push({ type: 'read', mediaId: m.id });
  acts.push({ type: 'checkWiring' });
  for (const p of PEAKS) acts.push({ type: 'togglePeak', peakId: p.id });
  for (const v of [0, 35, 38, 41]) acts.push({ type: 'setOffset', offsetMin: v });
  acts.push({ type: 'autoFit' });
  acts.push({ type: 'pinAlignment' }, { type: 'unpinAlignment' }, { type: 'judgeOrder' });
  for (const r of ROUTES) acts.push({ type: 'selectRoute', routeId: r.id });
  acts.push({ type: 'clearRoute' }, { type: 'preview' }, { type: 'commitRoute' });
  for (const m of MEDIA) acts.push({ type: 'toggleSeal', mediaId: m.id });
  acts.push({ type: 'sign' }, { type: 'cancelSign' });
  for (const e of ENDINGS) acts.push({ type: 'selectEnding', endingId: e.id });
  acts.push({ type: 'clearEnding' });
  return acts;
}

export function describeAction(a) {
  switch (a.type) {
    case 'read': return `판독: ${MEDIA_BY_ID.get(a.mediaId).label}`;
    case 'togglePeak': return `피크 물림/해제: ${PEAK_BY_ID.get(a.peakId).label}`;
    case 'setOffset': return `오프셋 ${a.offsetMin}분`;
    case 'selectRoute': return `경로 선택: ${ROUTE_BY_ID.get(a.routeId).label}`;
    case 'toggleSeal': return `근거 슬롯: ${MEDIA_BY_ID.get(a.mediaId).label}`;
    case 'selectEnding': return `관점 선택: ${a.endingId}`;
    default: return a.type;
  }
}

/* ------------------------------------------------------------------ *
 * 6. 세션 계층 — 자동 저장(메모리)·되돌림·저장/불러오기
 *    리듀서는 여전히 순수하다. 세션 함수도 새 객체를 돌려준다.
 * ------------------------------------------------------------------ */

export function initialSession() {
  return { core: initialCore(), past: [], autosave: null, slot: null, lastEvent: null };
}

const AUTOSAVE_BEFORE = new Set(['commitRoute', 'sign']);

export function sessionApply(session, action) {
  const a = action || {};
  const fail = (code) => ({ session, ok: false, code, reason: REASONS[code] || code });

  if (a.type === 'undo') {
    if (session.past.length === 0) return fail('NOTHING_TO_UNDO');
    const past = session.past.slice();
    const prev = past.pop();
    const restored = preserveClues(cloneCore(prev), session.core);
    return { session: { ...session, core: restored, past, lastEvent: 'undo' }, ok: true, code: null, reason: null };
  }
  if (a.type === 'reset') {
    const fresh = preserveClues(initialCore(), session.core);
    return { session: { ...session, core: fresh, past: [], lastEvent: 'reset' }, ok: true, code: null, reason: null };
  }
  if (a.type === 'restoreAutosave') {
    if (!session.autosave) return fail('NO_AUTOSAVE');
    const restored = preserveClues(cloneCore(session.autosave), session.core);
    const past = pushPast(session.past, session.core);
    return { session: { ...session, core: restored, past, lastEvent: 'restoreAutosave' }, ok: true, code: null, reason: null };
  }
  if (a.type === 'save') {
    return { session: { ...session, slot: serialize(session.core), lastEvent: 'save' }, ok: true, code: null, reason: null };
  }
  if (a.type === 'load') {
    const payload = a.payload !== undefined ? a.payload : session.slot;
    const res = deserialize(payload);
    if (!res.ok) {
      // 손상 파일은 덮어쓰지 않는다. 세션 상태도 바꾸지 않는다.
      return { session, ok: false, code: res.code, reason: res.reason, recovery: recoveryOptions(session) };
    }
    const past = pushPast(session.past, session.core);
    const loaded = preserveClues(res.core, session.core);
    return { session: { ...session, core: loaded, past, lastEvent: 'load' }, ok: true, code: null, reason: null };
  }

  const before = session.core;
  const r = reduce(before, a);
  if (!r.ok) return { session, ok: false, code: r.code, reason: r.reason };
  const autosave = AUTOSAVE_BEFORE.has(a.type) ? cloneCore(before) : session.autosave;
  return {
    session: { ...session, core: r.core, past: pushPast(session.past, before), autosave, lastEvent: a.type },
    ok: true, code: null, reason: null,
  };
}

function pushPast(past, core) {
  const next = [...past, cloneCore(core)];
  return next.length > LIMITS.maxUndo ? next.slice(next.length - LIMITS.maxUndo) : next;
}

export function recoveryOptions(session) {
  return [
    { id: 'autosave', label: '메모리 자동 저장으로 열기', available: session.autosave !== null },
    { id: 'checkpoint', label: '마지막 확정 체크포인트로', available: session.past.length > 0 },
    { id: 'new', label: '새로 시작(보존 단서는 유지)', available: true },
  ];
}

/* ------------------------------------------------------------------ *
 * 7. 저장 형식 — 체크섬·스키마 검사
 * ------------------------------------------------------------------ */

export function checksum(str) {
  let h = 0x811c9dc5;
  for (let i = 0; i < str.length; i += 1) {
    h ^= str.charCodeAt(i);
    h = Math.imul(h, 0x01000193) >>> 0;
  }
  return h.toString(16).padStart(8, '0');
}

const CORE_KEYS = Object.keys(initialCore()).sort();

function canonical(core) {
  const o = {};
  for (const k of CORE_KEYS) o[k] = core[k];
  return JSON.stringify(o);
}

export function serialize(core) {
  const body = canonical(core);
  return { modelId: MODEL_ID, schemaVersion: SCHEMA_VERSION, checksum: checksum(body), data: JSON.parse(body) };
}

export function deserialize(payload) {
  if (!payload || typeof payload !== 'object') return block('SCHEMA_INVALID');
  if (payload.schemaVersion === undefined || payload.schemaVersion === null) return block('SCHEMA_MISSING');
  if (!Number.isInteger(payload.schemaVersion)) return block('SCHEMA_INVALID');
  if (payload.schemaVersion > SCHEMA_VERSION) return block('SCHEMA_TOO_NEW');
  if (payload.schemaVersion < SCHEMA_VERSION) return block('SCHEMA_INVALID');
  if (payload.modelId !== MODEL_ID) return block('SCHEMA_INVALID');
  const d = payload.data;
  if (!d || typeof d !== 'object') return block('SCHEMA_INVALID');
  for (const k of CORE_KEYS) if (!(k in d)) return block('SCHEMA_INVALID');
  const shaped = {};
  for (const k of CORE_KEYS) shaped[k] = d[k];
  if (typeof payload.checksum !== 'string') return block('SCHEMA_INVALID');
  if (checksum(JSON.stringify(shaped)) !== payload.checksum) return block('CHECKSUM_FAILED');
  const core = initialCore();
  for (const k of CORE_KEYS) core[k] = Array.isArray(shaped[k]) ? shaped[k].slice() : shaped[k];
  return { ok: true, code: null, reason: null, core };
}

/* ------------------------------------------------------------------ *
 * 8. 유한 도달 상태 탐색 — 숫자 키 BFS
 * ------------------------------------------------------------------ */

const OFFSET_INDEX = new Map();
function offsetIdx(v) {
  if (!OFFSET_INDEX.has(v)) OFFSET_INDEX.set(v, OFFSET_INDEX.size);
  return OFFSET_INDEX.get(v);
}
const MEDIA_BIT = new Map(MEDIA.map((m, i) => [m.id, 1 << i]));
const PEAK_BIT = new Map(PEAKS.map((p, i) => [p.id, 1 << i]));
const CLUE_IDS = [...new Set(MEDIA.map((m) => m.clueId))].sort();
const CLUE_BIT = new Map(CLUE_IDS.map((c, i) => [c, 1 << i]));
const ROUTE_IDX = new Map(ROUTES.map((r, i) => [r.id, i + 1]));
const ENDING_IDX = new Map(ENDINGS.map((e, i) => [e.id, i + 1]));

function mask(ids, table) { let m = 0; for (const id of ids) m |= table.get(id); return m; }

export function encodeCore(core) {
  let k = mask(core.copies, MEDIA_BIT);
  k = k * 16 + mask(core.keptClues, CLUE_BIT);
  k = k * 2 + (core.wiringChecked ? 1 : 0);
  k = k * 16 + mask(core.peaks, PEAK_BIT);
  k = k * 64 + offsetIdx(core.offsetMin);
  k = k * 2 + (core.aligned ? 1 : 0);
  k = k * 2 + (core.orderJudged ? 1 : 0);
  k = k * 4 + (core.route ? ROUTE_IDX.get(core.route) : 0);
  k = k * 2 + (core.previewed ? 1 : 0);
  k = k * 2 + (core.committed ? 1 : 0);
  k = k * 4 + (core.propertyProtection === null ? 0 : core.propertyProtection === 'lowland' ? 1 : 2);
  k = k * 32 + mask(core.seal, MEDIA_BIT);
  k = k * 2 + (core.signed ? 1 : 0);
  k = k * 4 + (core.ending ? ENDING_IDX.get(core.ending) : 0);
  return k;
}

/**
 * 초기 상태에서 도달 가능한 모든 상태를 폭넓이 우선으로 전개한다.
 * visit(core) 이 false 를 돌려주면 즉시 중단한다.
 * 되돌림/저장·불러오기는 코어 상태를 새로 만들지 않으므로(이전 도달 상태로만 돌아간다)
 * 이 탐색의 알파벳에서 제외한다 — 해당 경로는 표적 테스트가 따로 덮는다.
 */
export function exploreReachable(options) {
  const opts = options || {};
  const maxStates = opts.maxStates || 5_000_000;
  const alphabet = opts.alphabet || actionAlphabet();
  const start = initialCore();
  const seen = new Set([encodeCore(start)]);
  const queue = [start];
  const stats = { states: 0, transitions: 0, allowed: 0, blocked: 0, blockedByCode: {}, complete: false, truncated: false };
  let head = 0;
  while (head < queue.length) {
    const core = queue[head]; head += 1;
    stats.states += 1;
    if (opts.visit && opts.visit(core) === false) return stats;
    for (const action of alphabet) {
      const r = reduce(core, action);
      stats.transitions += 1;
      if (!r.ok) {
        stats.blocked += 1;
        stats.blockedByCode[r.code] = (stats.blockedByCode[r.code] || 0) + 1;
        if (opts.onBlocked) opts.onBlocked(core, action, r);
        continue;
      }
      stats.allowed += 1;
      const key = encodeCore(r.core);
      if (seen.has(key)) continue;
      if (seen.size >= maxStates) { stats.truncated = true; return stats; }
      seen.add(key);
      queue.push(r.core);
    }
    if (head > 4_000_000) { stats.truncated = true; return stats; }
  }
  stats.complete = true;
  stats.uniqueStates = seen.size;
  return stats;
}

/* ------------------------------------------------------------------ *
 * 9. 불변식 — 도달 가능한 모든 상태에서 성립해야 한다
 * ------------------------------------------------------------------ */

export const INVARIANTS = Object.freeze([
  Object.freeze({
    id: 'INV1', label: '판독한 필수 단서는 보존된다',
    check(core) {
      for (const id of core.copies) {
        const m = MEDIA_BY_ID.get(id);
        if (m.clueMandatory && !core.keptClues.includes(m.clueId)) return `필수 단서 누락: ${m.clueId}`;
      }
      return null;
    },
  }),
  Object.freeze({
    id: 'INV2', label: '엔딩 3종은 항상 선택 가능하다',
    check(core) {
      const n = selectableEndings(core).length;
      return n === 3 ? null : `선택 가능 엔딩 ${n}종`;
    },
  }),
  Object.freeze({
    id: 'INV3', label: '부식예산을 넘긴 구성은 확정되지 않는다',
    check(core) {
      if (!core.committed) return null;
      const r = ROUTE_BY_ID.get(core.route);
      if (!r) return '확정됐는데 경로가 없다';
      return r.corrosion <= LIMITS.corrosionLimit ? null : `한도 초과 확정: ${r.corrosion}/${LIMITS.corrosionLimit}`;
    },
  }),
  Object.freeze({
    id: 'INV4', label: '확정은 프리뷰를 거친 뒤에만 존재한다',
    check(core) { return !core.committed || core.previewed ? null : '프리뷰 없이 확정됨'; },
  }),
  Object.freeze({
    id: 'INV5', label: '서명은 배선 안 독립 매체 2종 + 확정된 기준선에서만 선다',
    check(core) {
      if (!core.signed) return null;
      if (core.seal.length !== LIMITS.sealSlots) return '슬롯이 2개가 아닌데 서명됨';
      for (const id of core.seal) {
        if (!core.copies.includes(id)) return '판독하지 않은 근거로 서명됨';
        if (!MEDIA_BY_ID.get(id).wired) return '배선 범위 밖 근거로 서명됨';
      }
      if (!core.wiringChecked) return '배선 확인 없이 서명됨';
      const ind = independencePair(core.seal[0], core.seal[1]);
      if (!ind.ok) return `독립성 위반으로 서명됨: ${ind.code}`;
      if (!core.aligned) return '기준선 미확정 상태로 서명됨';
      return null;
    },
  }),
  Object.freeze({
    id: 'INV6', label: '기준선 확정은 피크 3개 + 잔차 4분 이하에서만 존재한다',
    check(core) {
      if (!core.aligned) return null;
      if (core.peaks.length !== LIMITS.requiredPeaks) return `피크 ${core.peaks.length}개로 확정됨`;
      const res = residualFor(core.peaks, core.offsetMin);
      return res <= LIMITS.residualLimitMin ? null : `잔차 ${res}분으로 확정됨`;
    },
  }),
  Object.freeze({
    id: 'INV7', label: '선후 판정은 기준선 확정 뒤에만 존재하고 오차폭 규칙을 따른다',
    check(core) {
      if (!core.orderJudged) return null;
      if (!core.aligned) return '정합 없이 판정됨';
      const st = orderStatus(core);
      for (const v of st.verdicts) {
        const want = v.gapMin > LIMITS.combinedUncertaintyMin ? 'ordered' : 'unknown';
        if (v.verdict !== want) return `${v.id} 판정 불일치: ${v.verdict} ≠ ${want}`;
      }
      return null;
    },
  }),
  Object.freeze({
    id: 'INV8', label: '재산 보호 플래그는 확정된 경로와만 일치한다',
    check(core) {
      if (!core.committed) return core.propertyProtection === null ? null : '미확정인데 보호 플래그가 있다';
      const r = ROUTE_BY_ID.get(core.route);
      return core.propertyProtection === r.protects ? null : '보호 플래그가 확정 경로와 다르다';
    },
  }),
  Object.freeze({
    id: 'INV9', label: '피크는 3개를 넘지 않고 슬롯은 2개를 넘지 않는다',
    check(core) {
      if (core.peaks.length > LIMITS.requiredPeaks) return `피크 ${core.peaks.length}개`;
      if (core.seal.length > LIMITS.sealSlots) return `슬롯 ${core.seal.length}개`;
      return null;
    },
  }),
]);

export function checkInvariants(core) {
  const failures = [];
  for (const inv of INVARIANTS) {
    const msg = inv.check(core);
    if (msg) failures.push({ id: inv.id, label: inv.label, detail: msg });
  }
  return failures;
}
