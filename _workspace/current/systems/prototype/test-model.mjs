/*
 * test-model.mjs — 표준 라이브러리(node:assert)만 쓰는 검증기
 * 실행: node test-model.mjs
 *
 * 여기서 증명하는 것은 **모형의 내부 일관성**뿐이다.
 * 게임이 재미있다거나, 퍼즐이 깊다거나, 캠페인의 소프트락이 해결됐다거나,
 * 플레이 시간이 얼마라는 주장은 하나도 하지 않는다.
 */

import assert from 'node:assert/strict';
import * as M from './model.mjs';

let passed = 0;
const failures = [];
const receipts = [];

function test(name, fn) {
  try {
    const note = fn();
    passed += 1;
    console.log(`  ok ${String(passed).padStart(2, ' ')}  ${name}${note ? ` — ${note}` : ''}`);
  } catch (err) {
    failures.push({ name, err });
    console.log(`  FAIL   ${name}\n         ${err && err.message}`);
  }
}
function receipt(k, v) { receipts.push([k, v]); }

/** 조작 열을 순서대로 적용하고, 각 단계의 통과/차단을 그대로 돌려준다. */
function run(actions, start) {
  let core = start || M.initialCore();
  const trace = [];
  for (const a of actions) {
    const r = M.reduce(core, a);
    trace.push({ action: a, ok: r.ok, code: r.code, reason: r.reason });
    if (r.ok) core = r.core;
  }
  return { core, trace };
}

const READ_ALL_WIRED = [
  { type: 'read', mediaId: 'plate-archive' },
  { type: 'read', mediaId: 'ledger-dock' },
];
const ALIGN_OK = [
  { type: 'togglePeak', peakId: 'p1' },
  { type: 'togglePeak', peakId: 'p2' },
  { type: 'togglePeak', peakId: 'p3' },
  { type: 'autoFit' },
  { type: 'pinAlignment' },
];

console.log('\n== 1. 설정 경계 ==');

test('작업대는 매체 5점 · 피크 후보 4개 · 경로 3개 · 엔딩 3종 · UI 6단계로 유한하다', () => {
  assert.equal(M.MEDIA.length, 5);
  assert.equal(M.PEAKS.length, 4);
  assert.equal(M.ROUTES.length, 3);
  assert.equal(M.ENDINGS.length, 3);
  assert.equal(M.STEPS.length, 6);
  assert.equal(M.EVENT_PAIRS.length, 2);
  receipt('매체/피크/경로/엔딩/단계', '5 / 4 / 3 / 3 / 6');
  return '5 / 4 / 3 / 3 / 6';
});

test('배선 범위 밖 매체는 정확히 1점이고 같은 출처 사본 쌍은 정확히 1쌍이다', () => {
  const unwired = M.MEDIA.filter((m) => !m.wired);
  assert.equal(unwired.length, 1);
  assert.equal(unwired[0].id, 'log-outdoor');
  const byOrigin = new Map();
  for (const m of M.MEDIA) byOrigin.set(m.originId, (byOrigin.get(m.originId) || 0) + 1);
  const shared = [...byOrigin.values()].filter((n) => n > 1);
  assert.deepEqual(shared, [2]);
  return '미배선 1점 · 동일출처 2점 1쌍';
});

console.log('\n== 2. 6단계 정상 경로 ==');

test('1~6단계를 순서대로 통과하면 서명과 관점 제출까지 도달한다', () => {
  const { core, trace } = run([
    ...READ_ALL_WIRED,
    { type: 'checkWiring' },
    ...ALIGN_OK,
    { type: 'judgeOrder' },
    { type: 'selectRoute', routeId: 'lowland' },
    { type: 'preview' },
    { type: 'commitRoute' },
    { type: 'toggleSeal', mediaId: 'plate-archive' },
    { type: 'toggleSeal', mediaId: 'ledger-dock' },
    { type: 'sign' },
    { type: 'selectEnding', endingId: 'residents' },
  ]);
  const blockedSteps = trace.filter((t) => !t.ok);
  assert.deepEqual(blockedSteps, [], `차단된 단계: ${JSON.stringify(blockedSteps)}`);
  assert.equal(core.signed, true);
  assert.equal(core.committed, true);
  assert.equal(core.propertyProtection, 'lowland');
  assert.equal(core.ending, 'residents');
  assert.deepEqual(M.checkInvariants(core), []);
  const a = M.alignmentStatus(core);
  receipt('정상 경로 조작 수', String(trace.length));
  receipt('정상 경로 잔차', `${a.residualMin}분 (한도 ${M.LIMITS.residualLimitMin}분)`);
  return `${trace.length}개 조작 전부 통과 · 잔차 ${a.residualMin}분`;
});

test('자동 맞춤은 오프셋 38분을 내고 잔차 3분으로 한도 아래에 든다', () => {
  const { core } = run([...READ_ALL_WIRED, { type: 'togglePeak', peakId: 'p1' },
    { type: 'togglePeak', peakId: 'p2' }, { type: 'togglePeak', peakId: 'p3' }, { type: 'autoFit' }]);
  assert.equal(core.offsetMin, 38);
  assert.equal(M.residualFor(core.peaks, core.offsetMin), 3);
  return '오프셋 38분 · 잔차 3분';
});

console.log('\n== 3. 무효 분기(모두 사유가 표시된다) ==');

test('판독하지 않은 매체는 근거 슬롯에 올라가지 않는다 — NOT_READ', () => {
  const r = M.reduce(M.initialCore(), { type: 'toggleSeal', mediaId: 'plate-archive' });
  assert.equal(r.ok, false);
  assert.equal(r.code, 'NOT_READ');
  assert.ok(r.reason.length > 0);
  return r.reason;
});

test('공통 피크 2개로는 기준선을 확정할 수 없다 — PEAK_COUNT_SHORT', () => {
  const { core } = run([...READ_ALL_WIRED, { type: 'togglePeak', peakId: 'p1' },
    { type: 'togglePeak', peakId: 'p2' }, { type: 'autoFit' }]);
  const res = M.residualFor(core.peaks, core.offsetMin);
  assert.ok(res <= M.LIMITS.residualLimitMin, '두 점만으로도 잔차 자체는 작다');
  const r = M.reduce(core, { type: 'pinAlignment' });
  assert.equal(r.ok, false);
  assert.equal(r.code, 'PEAK_COUNT_SHORT');
  return `잔차 ${res}분이어도 피크 2개면 차단`;
});

test('거짓 대응 피크가 섞이면 잔차가 한도를 넘어 확정이 막힌다 — RESIDUAL_OVER', () => {
  const { core } = run([...READ_ALL_WIRED, { type: 'togglePeak', peakId: 'p1' },
    { type: 'togglePeak', peakId: 'p2' }, { type: 'togglePeak', peakId: 'pX' }, { type: 'autoFit' }]);
  const res = M.residualFor(core.peaks, core.offsetMin);
  assert.ok(res > M.LIMITS.residualLimitMin);
  const r = M.reduce(core, { type: 'pinAlignment' });
  assert.equal(r.ok, false);
  assert.equal(r.code, 'RESIDUAL_OVER');
  return `잔차 ${res}분 > ${M.LIMITS.residualLimitMin}분`;
});

test('기준선 없이 선후 판정은 실행되지 않는다 — ALIGN_NOT_PINNED', () => {
  const { core } = run(READ_ALL_WIRED);
  const r = M.reduce(core, { type: 'judgeOrder' });
  assert.equal(r.ok, false);
  assert.equal(r.code, 'ALIGN_NOT_PINNED');
  return r.reason;
});

test('출처 독립성: 같은 판의 복제 스캔은 두 번째 근거로 세지 않는다 — SAME_ORIGIN', () => {
  const { core } = run([{ type: 'read', mediaId: 'plate-archive' }, { type: 'read', mediaId: 'plate-archive-scan' },
    { type: 'checkWiring' }, { type: 'toggleSeal', mediaId: 'plate-archive' },
    { type: 'toggleSeal', mediaId: 'plate-archive-scan' }]);
  assert.equal(core.seal.length, 2);
  const st = M.sealStatus(core);
  assert.equal(st.ok, false);
  assert.equal(st.code, 'SAME_ORIGIN');
  const r = M.reduce(core, { type: 'sign' });
  assert.equal(r.ok, false);
  assert.equal(r.code, 'SAME_ORIGIN');
  return st.reason;
});

test('출처가 달라도 매체 종류가 같으면 세지 않는다 — SAME_MEDIUM', () => {
  const { core } = run([{ type: 'read', mediaId: 'plate-archive' }, { type: 'read', mediaId: 'plate-dock' },
    { type: 'checkWiring' }, { type: 'toggleSeal', mediaId: 'plate-archive' },
    { type: 'toggleSeal', mediaId: 'plate-dock' }]);
  const st = M.sealStatus(core);
  assert.equal(st.code, 'SAME_MEDIUM');
  return st.reason;
});

test('배선 범위 밖 근거는 서명을 세우지 못한다 — OUT_OF_WIRING', () => {
  const { core } = run([{ type: 'read', mediaId: 'plate-archive' }, { type: 'read', mediaId: 'log-outdoor' },
    { type: 'checkWiring' }, { type: 'toggleSeal', mediaId: 'plate-archive' },
    { type: 'toggleSeal', mediaId: 'log-outdoor' }]);
  const st = M.sealStatus(core);
  assert.equal(st.code, 'OUT_OF_WIRING');
  return st.reason;
});

test('배선 범위 확인 전에는 서명이 서지 않는다 — WIRING_UNCHECKED', () => {
  const { core } = run([...READ_ALL_WIRED, ...ALIGN_OK,
    { type: 'toggleSeal', mediaId: 'plate-archive' }, { type: 'toggleSeal', mediaId: 'ledger-dock' }]);
  const r = M.reduce(core, { type: 'sign' });
  assert.equal(r.ok, false);
  assert.equal(r.code, 'WIRING_UNCHECKED');
  return r.reason;
});

test('독립 매체 2종이 서도 기준선이 없으면 서명이 막힌다 — ALIGN_NOT_PINNED', () => {
  const { core } = run([...READ_ALL_WIRED, { type: 'checkWiring' },
    { type: 'toggleSeal', mediaId: 'plate-archive' }, { type: 'toggleSeal', mediaId: 'ledger-dock' }]);
  const r = M.reduce(core, { type: 'sign' });
  assert.equal(r.ok, false);
  assert.equal(r.code, 'ALIGN_NOT_PINNED');
  return r.reason;
});

test('유효한 근거 쌍은 정확히 3쌍뿐이다(사본 쌍·동일 매체 쌍·미배선 쌍 제외)', () => {
  const valid = [];
  for (let i = 0; i < M.MEDIA.length; i += 1) {
    for (let j = i + 1; j < M.MEDIA.length; j += 1) {
      const a = M.MEDIA[i]; const b = M.MEDIA[j];
      const ind = M.independencePair(a.id, b.id);
      if (ind.ok && a.wired && b.wired) valid.push(`${a.id}+${b.id}`);
    }
  }
  assert.equal(valid.length, 3, `유효 쌍: ${valid.join(', ')}`);
  receipt('유효 근거 쌍', `${valid.length}쌍 (${valid.join(' / ')})`);
  return `${valid.length}쌍`;
});

console.log('\n== 4. 판정 불가(unknown)는 무사건이 아니다 ==');

test('간격 20분은 확정, 6분은 unknown — 오차폭 합 8분 기준', () => {
  assert.equal(M.LIMITS.combinedUncertaintyMin, M.LIMITS.stationUncertaintyMin * 2);
  const { core } = run([...READ_ALL_WIRED, ...ALIGN_OK, { type: 'judgeOrder' }]);
  const st = M.orderStatus(core);
  assert.equal(st.ok, true);
  const byId = Object.fromEntries(st.verdicts.map((v) => [v.id, v.verdict]));
  assert.equal(byId['pair-20'], 'ordered');
  assert.equal(byId['pair-6'], 'unknown');
  receipt('선후 판정', 'pair-20=ordered · pair-6=unknown');
  return '20분 ordered · 6분 unknown';
});

test('unknown 판정이 나도 이후 확정·서명·엔딩 선택이 막히지 않는다', () => {
  const { core, trace } = run([...READ_ALL_WIRED, { type: 'checkWiring' }, ...ALIGN_OK, { type: 'judgeOrder' },
    { type: 'selectRoute', routeId: 'dock' }, { type: 'preview' }, { type: 'commitRoute' },
    { type: 'toggleSeal', mediaId: 'plate-archive' }, { type: 'toggleSeal', mediaId: 'ledger-dock' },
    { type: 'sign' }]);
  assert.deepEqual(trace.filter((t) => !t.ok), []);
  assert.equal(core.signed, true);
  assert.equal(M.selectableEndings(core).length, 3);
  return 'unknown 이후에도 진행 가능';
});

console.log('\n== 5. 프리뷰 · 확정 · 안전하지 않은 확정 차단 ==');

test('프리뷰 없이 확정은 막힌다 — NO_PREVIEW', () => {
  const { core } = run([{ type: 'selectRoute', routeId: 'lowland' }]);
  const r = M.reduce(core, { type: 'commitRoute' });
  assert.equal(r.ok, false);
  assert.equal(r.code, 'NO_PREVIEW');
  return r.reason;
});

test('경로를 바꾸면 프리뷰가 무효가 되어 확정이 다시 막힌다', () => {
  const { core } = run([{ type: 'selectRoute', routeId: 'lowland' }, { type: 'preview' },
    { type: 'selectRoute', routeId: 'dock' }]);
  assert.equal(core.previewed, false);
  assert.equal(M.reduce(core, { type: 'commitRoute' }).code, 'NO_PREVIEW');
  return '프리뷰 무효화 확인';
});

test('부식예산을 넘긴 구성은 프리뷰까지 되어도 확정 직전에 막힌다 — CORROSION_OVER', () => {
  const { core } = run([{ type: 'selectRoute', routeId: 'dock-express' }, { type: 'preview' }]);
  const st = M.commitStatus(core);
  assert.equal(st.ok, false);
  assert.equal(st.code, 'CORROSION_OVER');
  assert.equal(st.corrosion, 12);
  assert.equal(st.limit, 9);
  const r = M.reduce(core, { type: 'commitRoute' });
  assert.equal(r.ok, false);
  assert.equal(core.committed, false);
  receipt('초과 구성 차단', `부식 ${st.corrosion} / 한도 ${st.limit}`);
  return `부식 ${st.corrosion} > 한도 ${st.limit}`;
});

test('저지대·부두 두 경로 모두 확정 가능하고 보호 플래그만 달라진다', () => {
  const out = ['lowland', 'dock'].map((id) => {
    const { core } = run([{ type: 'selectRoute', routeId: id }, { type: 'preview' }, { type: 'commitRoute' }]);
    assert.equal(core.committed, true);
    assert.equal(M.selectableEndings(core).length, 3);
    return `${id}→${core.propertyProtection}`;
  });
  return out.join(' · ');
});

console.log('\n== 6. 엔딩 3종은 재산 보호 플래그와 무관하다 ==');

test('보호 플래그 3가지 값(null·lowland·dock) 모두에서 엔딩 3종이 선택 가능하다', () => {
  const cores = [M.initialCore()];
  for (const id of ['lowland', 'dock']) {
    cores.push(run([{ type: 'selectRoute', routeId: id }, { type: 'preview' }, { type: 'commitRoute' }]).core);
  }
  const flags = cores.map((c) => c.propertyProtection);
  assert.deepEqual(flags, [null, 'lowland', 'dock']);
  for (const c of cores) {
    assert.equal(M.selectableEndings(c).length, 3);
    for (const e of M.ENDINGS) assert.equal(M.reduce(c, { type: 'selectEnding', endingId: e.id }).ok, true);
  }
  return '3 × 3 = 9가지 조합 모두 선택 가능';
});

test('확정 후에도 관점을 바꿔 다시 선택할 수 있다(재시작 요구 없음)', () => {
  const { core } = run([{ type: 'selectRoute', routeId: 'dock' }, { type: 'preview' }, { type: 'commitRoute' },
    { type: 'selectEnding', endingId: 'record' }, { type: 'selectEnding', endingId: 'operations' }]);
  assert.equal(core.ending, 'operations');
  return 'record → operations 전환 확인';
});

console.log('\n== 7. 되돌림 · 취소 · 자동 저장(메모리) ==');

test('되돌림은 직전 상태로 돌아가되 보존된 필수 단서와 사본은 유지한다', () => {
  let s = M.initialSession();
  for (const a of [...READ_ALL_WIRED, { type: 'checkWiring' }]) s = M.sessionApply(s, a).session;
  const cluesBefore = s.core.keptClues.slice();
  const copiesBefore = s.core.copies.slice();
  s = M.sessionApply(s, { type: 'togglePeak', peakId: 'p1' }).session;
  const u = M.sessionApply(s, { type: 'undo' });
  assert.equal(u.ok, true);
  assert.deepEqual(u.session.core.peaks, []);
  assert.deepEqual(u.session.core.keptClues, cluesBefore);
  assert.deepEqual(u.session.core.copies, copiesBefore);
  return `단서 ${cluesBefore.length}개 · 사본 ${copiesBefore.length}점 유지`;
});

test('되돌릴 것이 없으면 사유와 함께 거부한다 — NOTHING_TO_UNDO', () => {
  const r = M.sessionApply(M.initialSession(), { type: 'undo' });
  assert.equal(r.ok, false);
  assert.equal(r.code, 'NOTHING_TO_UNDO');
  return r.reason;
});

test('초기화는 작업을 비우되 판독 사본과 필수 단서는 남긴다', () => {
  let s = M.initialSession();
  for (const a of [...READ_ALL_WIRED, { type: 'checkWiring' }, ...ALIGN_OK]) s = M.sessionApply(s, a).session;
  const r = M.sessionApply(s, { type: 'reset' });
  assert.equal(r.session.core.aligned, false);
  assert.deepEqual(r.session.core.peaks, []);
  assert.equal(r.session.core.wiringChecked, false);
  assert.deepEqual(r.session.core.copies, s.core.copies);
  assert.deepEqual(r.session.core.keptClues, s.core.keptClues);
  for (const c of M.MANDATORY_CLUE_IDS.filter((id) => s.core.keptClues.includes(id))) {
    assert.ok(r.session.core.keptClues.includes(c));
  }
  return `사본 ${r.session.core.copies.length}점 · 단서 ${r.session.core.keptClues.length}개 잔존`;
});

test('취소 조작(서명 취소·경로 해제·고정핀 해제·관점 해제)이 모두 되돌아간다', () => {
  let s = M.initialSession();
  const seq = [...READ_ALL_WIRED, { type: 'checkWiring' }, ...ALIGN_OK,
    { type: 'toggleSeal', mediaId: 'plate-archive' }, { type: 'toggleSeal', mediaId: 'ledger-dock' },
    { type: 'sign' }, { type: 'selectEnding', endingId: 'record' },
    { type: 'selectRoute', routeId: 'dock' }];
  for (const a of seq) { const r = M.sessionApply(s, a); assert.equal(r.ok, true, `${a.type} 차단됨: ${r.reason}`); s = r.session; }
  for (const a of [{ type: 'cancelSign' }, { type: 'clearEnding' }, { type: 'clearRoute' }, { type: 'unpinAlignment' }]) {
    const r = M.sessionApply(s, a); assert.equal(r.ok, true, `${a.type}: ${r.reason}`); s = r.session;
  }
  assert.equal(s.core.signed, false);
  assert.equal(s.core.ending, null);
  assert.equal(s.core.route, null);
  assert.equal(s.core.aligned, false);
  assert.deepEqual(s.core.keptClues.length > 0, true);
  return '4가지 취소 모두 반영 · 단서 유지';
});

test('확정과 서명 직전에 메모리 자동 저장이 잡히고 복구된다', () => {
  let s = M.initialSession();
  for (const a of [{ type: 'selectRoute', routeId: 'lowland' }, { type: 'preview' }]) s = M.sessionApply(s, a).session;
  assert.equal(s.autosave, null);
  s = M.sessionApply(s, { type: 'commitRoute' }).session;
  assert.notEqual(s.autosave, null);
  assert.equal(s.autosave.committed, false, '자동 저장은 확정 직전 상태다');
  const r = M.sessionApply(s, { type: 'restoreAutosave' });
  assert.equal(r.ok, true);
  assert.equal(r.session.core.committed, false);
  assert.equal(r.session.core.propertyProtection, null);
  return '확정 직전 상태로 복귀';
});

console.log('\n== 8. 저장 / 불러오기 — 스키마·체크섬·복구 ==');

test('정상 저장은 그대로 되살아난다(왕복 일치)', () => {
  const { core } = run([...READ_ALL_WIRED, { type: 'checkWiring' }, ...ALIGN_OK, { type: 'judgeOrder' }]);
  const payload = M.serialize(core);
  const res = M.deserialize(payload);
  assert.equal(res.ok, true);
  assert.deepEqual(res.core, core);
  return `schemaVersion ${payload.schemaVersion} · checksum ${payload.checksum}`;
});

test('스키마 버전이 없으면 거부한다 — SCHEMA_MISSING', () => {
  const p = M.serialize(M.initialCore()); delete p.schemaVersion;
  const r = M.deserialize(p);
  assert.equal(r.ok, false); assert.equal(r.code, 'SCHEMA_MISSING');
  return r.reason;
});

test('등록되지 않은 상위 스키마는 거부하고 파일을 그대로 둔다 — SCHEMA_TOO_NEW', () => {
  const p = M.serialize(M.initialCore()); p.schemaVersion = M.SCHEMA_VERSION + 1;
  const r = M.deserialize(p);
  assert.equal(r.code, 'SCHEMA_TOO_NEW');
  let s = M.initialSession();
  s = M.sessionApply(s, { type: 'read', mediaId: 'plate-archive' }).session;
  const before = JSON.stringify(s.core);
  const load = M.sessionApply(s, { type: 'load', payload: p });
  assert.equal(load.ok, false);
  assert.equal(JSON.stringify(load.session.core), before, '거부된 로드가 현재 상태를 덮어쓰면 안 된다');
  return '거부 + 현재 상태 무변경';
});

test('필드가 빠진 저장은 형식 오류로 거부한다 — SCHEMA_INVALID', () => {
  const p = M.serialize(M.initialCore()); delete p.data.seal;
  assert.equal(M.deserialize(p).code, 'SCHEMA_INVALID');
  const p2 = M.serialize(M.initialCore()); p2.modelId = 'other-model';
  assert.equal(M.deserialize(p2).code, 'SCHEMA_INVALID');
  assert.equal(M.deserialize(null).code, 'SCHEMA_INVALID');
  assert.equal(M.deserialize('문자열').code, 'SCHEMA_INVALID');
  return '누락 필드 · 다른 모형 · null · 문자열 4종 거부';
});

test('변조된 저장은 체크섬으로 걸리고 복구 선택지 3개를 준다 — CHECKSUM_FAILED', () => {
  let s = M.initialSession();
  for (const a of [{ type: 'selectRoute', routeId: 'lowland' }, { type: 'preview' }, { type: 'commitRoute' }]) {
    s = M.sessionApply(s, a).session;
  }
  s = M.sessionApply(s, { type: 'save' }).session;
  const tampered = JSON.parse(JSON.stringify(s.slot));
  tampered.data.committed = false;
  const r = M.sessionApply(s, { type: 'load', payload: tampered });
  assert.equal(r.ok, false);
  assert.equal(r.code, 'CHECKSUM_FAILED');
  assert.equal(r.session.core.committed, true, '손상 파일이 현재 상태를 덮어쓰지 않는다');
  assert.equal(r.recovery.length, 3);
  assert.deepEqual(r.recovery.map((o) => o.id), ['autosave', 'checkpoint', 'new']);
  assert.equal(r.recovery.filter((o) => o.available).length, 3);
  receipt('복구 선택지', r.recovery.map((o) => o.label).join(' / '));
  return `복구 3종 제시 · 원본 무변경`;
});

console.log('\n== 9. 유한 도달 상태 전수 탐색 ==');

let searchStats = null;
const witness = { signed: 0, committedLowland: 0, committedDock: 0, aligned: 0, judged: 0, endingSelected: 0, sealPairs: new Set() };
let roundTripChecked = 0;
let resetChecked = 0;
const invFailures = [];
const RT_STRIDE = 97; // 결정적 표본 간격

test('초기 상태에서 도달 가능한 모든 상태를 남김없이 전개한다', () => {
  let index = 0;
  const t0 = Date.now();
  searchStats = M.exploreReachable({
    visit(core) {
      index += 1;
      const f = M.checkInvariants(core);
      if (f.length && invFailures.length < 5) invFailures.push({ core, f });
      if (core.signed) { witness.signed += 1; witness.sealPairs.add(core.seal.join('+')); }
      if (core.committed && core.propertyProtection === 'lowland') witness.committedLowland += 1;
      if (core.committed && core.propertyProtection === 'dock') witness.committedDock += 1;
      if (core.aligned) witness.aligned += 1;
      if (core.orderJudged) witness.judged += 1;
      if (core.ending) witness.endingSelected += 1;
      if (index % RT_STRIDE === 0) {
        const back = M.deserialize(M.serialize(core));
        assert.equal(back.ok, true);
        assert.deepEqual(back.core, core);
        roundTripChecked += 1;
        const reset = M.sessionApply({ core, past: [], autosave: null, slot: null }, { type: 'reset' }).session.core;
        for (const id of core.copies) {
          const m = M.mediaById(id);
          assert.ok(reset.copies.includes(id), '초기화가 사본을 지웠다');
          if (m.clueMandatory) assert.ok(reset.keptClues.includes(m.clueId), '초기화가 필수 단서를 지웠다');
        }
        resetChecked += 1;
      }
      return true;
    },
  });
  const ms = Date.now() - t0;
  assert.equal(searchStats.complete, true, '탐색이 끝나지 않았다');
  assert.equal(searchStats.truncated, false, '탐색이 잘렸다');
  receipt('도달 상태 수', searchStats.states.toLocaleString('en-US'));
  receipt('전이 수', searchStats.transitions.toLocaleString('en-US'));
  receipt('허용 전이 / 차단 전이', `${searchStats.allowed.toLocaleString('en-US')} / ${searchStats.blocked.toLocaleString('en-US')}`);
  receipt('탐색 소요', `${ms} ms`);
  return `상태 ${searchStats.states.toLocaleString('en-US')}개 · 전이 ${searchStats.transitions.toLocaleString('en-US')}개 · ${ms} ms`;
});

test('도달 가능한 모든 상태에서 불변식 9개가 성립한다', () => {
  assert.equal(invFailures.length, 0, `위반 예시: ${JSON.stringify(invFailures[0])}`);
  assert.equal(M.INVARIANTS.length, 9);
  receipt('불변식 검사', `${M.INVARIANTS.length}개 × ${searchStats.states.toLocaleString('en-US')}상태 = 위반 0건`);
  return `${M.INVARIANTS.length}개 불변식 × ${searchStats.states.toLocaleString('en-US')}상태 = 위반 0건`;
});

test('차단 사유 코드가 탐색 중 하나도 빠짐없이 실제로 발생한다', () => {
  const observed = Object.keys(searchStats.blockedByCode);
  const expected = ['NOT_READ', 'ALREADY_READ', 'PEAK_LIMIT', 'PEAK_COUNT_SHORT', 'RESIDUAL_OVER',
    'ALIGN_NOT_PINNED', 'ALIGN_LOCKED', 'NO_ROUTE', 'NO_PREVIEW', 'CORROSION_OVER', 'ALREADY_COMMITTED',
    'ROUTE_LOCKED', 'SEAL_FULL', 'SEAL_INCOMPLETE', 'SEAL_LOCKED', 'SAME_ORIGIN', 'SAME_MEDIUM',
    'OUT_OF_WIRING', 'WIRING_UNCHECKED', 'ALREADY_SIGNED', 'NOT_SIGNED'];
  const missing = expected.filter((c) => !observed.includes(c));
  assert.deepEqual(missing, [], `발생하지 않은 사유: ${missing.join(', ')}`);
  const top = expected.map((c) => `${c}=${searchStats.blockedByCode[c].toLocaleString('en-US')}`);
  receipt('차단 사유 종류', `${expected.length}종 전부 발생`);
  receipt('차단 사유 표본', top.slice(0, 6).join(' · '));
  return `${expected.length}종 전부 발생`;
});

test('전수 탐색 어디에도 안전하지 않은 확정·무효 서명 상태가 없다', () => {
  assert.ok(witness.signed > 0, '서명 상태에 도달하지 못했다');
  assert.ok(witness.committedLowland > 0 && witness.committedDock > 0);
  assert.equal(witness.sealPairs.size, 3, `서명된 근거 쌍: ${[...witness.sealPairs].join(', ')}`);
  for (const pair of witness.sealPairs) {
    const [a, b] = pair.split('+');
    assert.equal(M.independencePair(a, b).ok, true);
    assert.ok(M.mediaById(a).wired && M.mediaById(b).wired);
  }
  receipt('서명 도달 상태', `${witness.signed.toLocaleString('en-US')}개 · 근거 쌍 ${witness.sealPairs.size}종`);
  receipt('확정 도달 상태', `저지대 ${witness.committedLowland.toLocaleString('en-US')} · 부두 ${witness.committedDock.toLocaleString('en-US')} · 초과안 0`);
  return `서명 ${witness.signed.toLocaleString('en-US')}상태 · 근거 쌍 ${witness.sealPairs.size}종 전부 독립`;
});

test('표본 상태에서 저장 왕복과 초기화 단서 보존이 유지된다', () => {
  assert.ok(roundTripChecked > 1000, `표본 부족: ${roundTripChecked}`);
  assert.equal(roundTripChecked, resetChecked);
  receipt('저장 왕복 표본', `${roundTripChecked.toLocaleString('en-US')}개 (간격 ${RT_STRIDE})`);
  return `${roundTripChecked.toLocaleString('en-US')}개 표본 통과 (간격 ${RT_STRIDE})`;
});

/* ------------------------------------------------------------------ */

console.log('\n== 영수증 ==');
for (const [k, v] of receipts) console.log(`  ${k.padEnd(22, ' ')} ${v}`);

console.log('\n== 이 테스트가 주장하지 않는 것 ==');
for (const line of [
  '게임·데모·Unity 빌드가 존재한다는 주장 없음.',
  '퍼즐 난이도·콘텐츠 깊이·재미에 대한 주장 없음.',
  '플레이 시간(30분 등)에 대한 주장 없음 — 측정 0건.',
  '캠페인 전체의 소프트락이 해결됐다는 주장 없음. 이 모형은 훈련용 작업대 1개만 덮는다.',
  '접근성·성능·조작감 실사용 검증 없음. 브라우저 QA는 별도 담당이다.',
]) console.log(`  · ${line}`);

console.log(`\n== 결과: ${passed} 통과 / ${failures.length} 실패 ==\n`);
if (failures.length > 0) process.exit(1);
