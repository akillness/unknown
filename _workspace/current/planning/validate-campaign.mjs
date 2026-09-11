#!/usr/bin/env node
// validate-campaign.mjs — planning 레인 소유 검증기 (C3-F11 / RFC-P3-008 대응)
//
// 목적: `planning/campaign.meta.md` §4 "검증 영수증"이 주장하는 검사를 사람이 아니라
//       기계가 실제로 수행하게 만든다. 이전 판의 §4는 생성기 자체 검사 결과를 손으로
//       옮겨 적은 것이라 live 파일과 어긋났다(qa/c3-review.md C3-F11).
//
// 의존성: Node 내장 모듈만 (node:fs, node:path, node:crypto, node:url). 설치 0건.
// 실행:   node _workspace/current/planning/validate-campaign.mjs
//         node _workspace/current/planning/validate-campaign.mjs <campaign.json 경로>
//         node _workspace/current/planning/validate-campaign.mjs --pairs   # A37 파생용
//         node _workspace/current/planning/validate-campaign.mjs --t0 <dir> # RFC-C7-001 (2) T0 인스턴스 검사
// 출력:   JSON (stdout). 종료 코드 0 = 전 검사 PASS, 1 = FAIL 존재, 2 = 실행 오류.
//         --t0 는 캠페인 검사 대신 T0 인스턴스 데이터(systems/data/t0/*.json)를 campaign.json ·
//         gdd §4 · style-guide §5 와 대조한다(T0-01~T0-05). 이 모드는 읽기만 하며 FAIL 은 systems 로 돌려보낸다.
//         --pairs 는 검사 대신 proofRequired 비트별 독립쌍(clue id · 루트 originId · sourceType)을
//         찍는다. `synopsis/continuity.md` §5 K 표는 이 출력에서 파생한다(A37 / C3-F12).
//
// 이 스크립트는 문서 정합만 검사한다. 플레이 표본 n=0이며 어떤 G 게이트도 올리지 않는다.

import { readFileSync } from 'node:fs';
import { createHash } from 'node:crypto';
import { resolve, dirname, join } from 'node:path';
import { fileURLToPath } from 'node:url';

const HERE = dirname(fileURLToPath(import.meta.url));
const ARGS = process.argv.slice(2);
const WANT_PAIRS = ARGS.includes('--pairs');
// --t0 <dir> | --t0=<dir> | --t0 (기본 ../systems/data/t0). <dir> 는 위치 인자로 세지 않는다.
const T0_IDX = ARGS.findIndex((a) => a === '--t0' || a.startsWith('--t0='));
const WANT_T0 = T0_IDX >= 0;
const T0_INLINE = WANT_T0 && ARGS[T0_IDX].startsWith('--t0=') ? ARGS[T0_IDX].slice(5) : null;
const T0_NEXT = WANT_T0 && !T0_INLINE && ARGS[T0_IDX + 1] && !ARGS[T0_IDX + 1].startsWith('--') ? ARGS[T0_IDX + 1] : null;
const T0_DIR = WANT_T0
  ? resolve(T0_INLINE ?? T0_NEXT ?? join(HERE, '..', 'systems', 'data', 't0'))
  : null;
const POSITIONAL = ARGS.filter((a, i) => !a.startsWith('--') && !(T0_NEXT !== null && i === T0_IDX + 1));
const TARGET = resolve(POSITIONAL[0] ?? join(HERE, 'campaign.json'));

// ── 고정 상수 (문서 계약) ──────────────────────────────────────────────
const EXPECT = {
  stages: 9,
  stageMinutes: [25, 50, 55, 65, 65, 70, 75, 65, 10], // RFC-P3-008 계보 B
  totalMinutes: 480,
  beats: 33,
  beatsPerStage: { t0: 3, c1: 4, c2: 4, c3: 4, c4: 4, c5: 4, c6: 4, c7: 4, e0: 2 },
  kinds: { puzzle: 21, exploration: 2, dialogue: 5, payoff: 5 },
  toolIds: ['circuit', 'reader', 'alignment', 'routing', 'corrosion', 'seal'],
  zoneIds: ['hub', 'gate', 'pump', 'dock', 'lowland'],
  toolTeachingTotal: 12,          // 도구 6종 × (guided 1 + unguided 1)
  hintLevels: 3,
  minMediaKinds: 2,               // 모든 비트: 서로 다른 sourceType ≥ 2
  subtasksMin: 3,
  subtasksMax: 5,
  timeConfidence: ['low', 'medium'], // 'high'는 표본 0이라 사용 금지
  sourceTypes: ['log', 'ledger', 'plate'],
  // RFC-P3-013 사건 시각 캐논
  canonTimesPresent: ['H-1:24', 'H-1:04', 'H+0:12'],
  canonTimesAbsent: ['H-1:20', 'H+0:10'],
  // C3 1차 검토 F1 회귀: '사람이 확인했다'는 기록 불가 명제가 다시 들어오지 않았는지
  forbiddenStrings: ['서명 확인 전', '서명 봉인 호출', '밸브 명령 대기 흔적'],
  requiredStrings: ['봉인 완료 접점'],
  // RFC-P3-012 공개 상한: 서명란의 이름 '한서린'은 c4-b2까지 미공개
  firstMention: { '도연': 't0-b1', '한서린': 'c4-b2' },
  // RFC-W4 R2 의도 공개 시점: 효력 판정은 c4-b3, 의도 문장은 c6-b4에서만 확정된다
  intentPhrase: '방패가 아니라 잠금장치',
  intentAbsentBeat: 'c4-b3',
  intentPresentBeat: 'c6-b4',
};

// ── 유틸 ──────────────────────────────────────────────────────────────
const checks = [];
function check(id, name, pass, expected, actual, note) {
  checks.push({ id, name, status: pass ? 'PASS' : 'FAIL', expected, actual, ...(note ? { note } : {}) });
}
// 키 순서에 의존하지 않는 비교 (객체는 키를 정렬해 정규화한다)
function norm(v) {
  if (Array.isArray(v)) return v.map(norm);
  if (v && typeof v === 'object') {
    return Object.keys(v).sort().reduce((o, k) => { o[k] = norm(v[k]); return o; }, {});
  }
  return v;
}
const eq = (a, b) => JSON.stringify(norm(a)) === JSON.stringify(norm(b));
const uniq = (a) => [...new Set(a)];
const lc = (o) => Object.fromEntries(Object.entries(o).map(([k, v]) => [String(k).toLowerCase(), v]));

let raw, bytes, sha256, campaign;
try {
  raw = readFileSync(TARGET);
  bytes = raw.length;
  sha256 = createHash('sha256').update(raw).digest('hex');
  campaign = JSON.parse(raw.toString('utf8'));
} catch (err) {
  process.stdout.write(JSON.stringify({ file: TARGET, error: String(err && err.message || err) }, null, 2) + '\n');
  process.exit(2);
}

const stages = campaign.stages ?? [];
const beats = stages.flatMap((s) => (s.beats ?? []).map((b) => ({ ...b, _stage: s.id })));
const clues = beats.flatMap((b) => (b.clues ?? []).map((c) => ({ ...c, _beat: b.id })));
const wholeText = raw.toString('utf8');

// ── 1. 파일 사실 ───────────────────────────────────────────────────────
check('F-01', 'schemaVersion = 1', campaign.schemaVersion === 1, 1, campaign.schemaVersion);
check('F-02', 'designMinutes = 480 (문서 상수 [TARGET])',
  campaign.designMinutes === EXPECT.totalMinutes, EXPECT.totalMinutes, campaign.designMinutes);
check('F-03', 'observedMedianMinutes = null (실측 n=0)',
  campaign.observedMedianMinutes === null, null, campaign.observedMedianMinutes);
check('F-04', 'humanPlaytests = [] (모집·실행 0회)',
  Array.isArray(campaign.humanPlaytests) && campaign.humanPlaytests.length === 0, 0,
  Array.isArray(campaign.humanPlaytests) ? campaign.humanPlaytests.length : campaign.humanPlaytests);

// ── 2. 스테이지 · 분 배분 ─────────────────────────────────────────────
const stageMinutes = stages.map((s) => s.minutes);
const stageBeatSums = stages.map((s) => (s.beats ?? []).reduce((n, b) => n + b.minutes, 0));
const sumStage = stageMinutes.reduce((a, b) => a + b, 0);
const sumBeats = beats.reduce((n, b) => n + b.minutes, 0);

check('S-01', '스테이지 9개', stages.length === EXPECT.stages, EXPECT.stages, stages.length);
check('S-02', '스테이지 분 배분', eq(stageMinutes, EXPECT.stageMinutes), EXPECT.stageMinutes, stageMinutes);
check('S-03', '스테이지 분 총합 480', sumStage === EXPECT.totalMinutes, EXPECT.totalMinutes, sumStage);
check('S-04', '비트 분 총합 480', sumBeats === EXPECT.totalMinutes, EXPECT.totalMinutes, sumBeats);
check('S-05', '스테이지별 (분 = 소속 비트 분 합)', eq(stageMinutes, stageBeatSums), stageMinutes, stageBeatSums);
check('S-06', '스테이지 zoneIds ⊂ 고정 5구역',
  stages.every((s) => (s.zoneIds ?? []).every((z) => EXPECT.zoneIds.includes(z))),
  EXPECT.zoneIds, uniq(stages.flatMap((s) => s.zoneIds ?? [])));

// ── 2-bis. 비트 단위 구역 (C3-F22 · 디렉터 C3 종료 판정) ──────────────
// 비트-구역 매핑의 단일 출처는 이 JSON이다. `planning/content-matrix.md` §3은 여기서 파생한다.
const stageZones = new Map(stages.map((s) => [s.id, s.zoneIds ?? []]));
const zoneMissing = beats.filter((b) => typeof b.zoneId !== 'string' || b.zoneId.trim().length === 0);
const zoneOutside = beats.filter((b) => typeof b.zoneId === 'string' && b.zoneId.trim().length > 0
  && !(stageZones.get(b._stage) ?? []).includes(b.zoneId));

check('Z-01', 'beat.zoneId ∈ 소속 stage.zoneIds', zoneOutside.length === 0, 0,
  zoneOutside.map((b) => `${b.id}:${b.zoneId}∉${JSON.stringify(stageZones.get(b._stage) ?? [])}`));
check('Z-02', '전 비트 zoneId 존재', zoneMissing.length === 0, 0, zoneMissing.map((b) => b.id));

// Z-03 (C6-F17 · 디렉터 R7 배정): 본문 ↔ zoneId. Z-01/Z-02는 zoneId 필드끼리만 대조하므로
// 비트 본문이 다른 구역을 말해도 통과했다(qa/c6-review.md X-8 전수 스캔에서 4비트 적발).
// 규칙: 첫 하위과제(subtasks[0])가 구역 명사를 하나라도 말하면 그중 하나는 beat.zoneId여야
// 한다. 구역 명사를 말하지 않는 비트(설비명으로 시작)는 이 검사의 대상이 아니다.
// 한계 [INFERENCE]: subtasks[0] 한 줄만 본다. 뒤 하위과제·consequence의 구역 이동은 판정하지 않는다.
const ZONE_KO = { hub: ['당직실'], gate: ['제3수문'], pump: ['양수장'], dock: ['부두'], lowland: ['저지대'] };
const zoneCue = (text) => Object.entries(ZONE_KO)
  .filter(([, words]) => words.some((w) => String(text).includes(w)))
  .map(([z]) => z);
const zoneBodyBad = beats
  .map((b) => ({ id: b.id, zoneId: b.zoneId, cues: zoneCue((b.subtasks ?? [])[0] ?? '') }))
  .filter((r) => r.cues.length > 0 && !r.cues.includes(r.zoneId));
check('Z-03', '본문(subtasks[0])이 말하는 구역 ∋ beat.zoneId (구역 무언급은 대상 외)',
  zoneBodyBad.length === 0, 0, zoneBodyBad.map((r) => `${r.id}:${r.zoneId}≠${r.cues.join('|')}`));

// ── 3. 비트 수 · 유일성 · 종류 ────────────────────────────────────────
// 스테이지 id는 데이터에서 대문자(T0·C1…)다. 대소문자에 판정이 걸리지 않게 정규화한다.
const beatsPerStage = lc(Object.fromEntries(stages.map((s) => [s.id, (s.beats ?? []).length])));
const kindCounts = {};
for (const b of beats) kindCounts[b.kind] = (kindCounts[b.kind] ?? 0) + 1;

check('B-01', '비트 33개', beats.length === EXPECT.beats, EXPECT.beats, beats.length);
check('B-02', '스테이지별 비트 수 (T0 3 · C1~C7 4 · E0 2)',
  eq(beatsPerStage, EXPECT.beatsPerStage), EXPECT.beatsPerStage, beatsPerStage);
check('B-03', 'beat id 유일', uniq(beats.map((b) => b.id)).length === beats.length, beats.length, uniq(beats.map((b) => b.id)).length);
check('B-04', 'clue id 유일', uniq(clues.map((c) => c.id)).length === clues.length, clues.length, uniq(clues.map((c) => c.id)).length);
check('B-05', 'checkpoint id 유일', uniq(beats.map((b) => b.checkpoint)).length === beats.length, beats.length, uniq(beats.map((b) => b.checkpoint)).length);
check('B-06', 'kind 분포', eq(kindCounts, EXPECT.kinds), EXPECT.kinds, kindCounts);

// ── 4. 시간 구조 (activityBudget · fast/deliberate) ───────────────────
const abBad = beats.filter((b) => {
  const ab = b.activityBudget ?? {};
  const keys = ['exploration', 'reasoning', 'manipulation', 'dialogue', 'payoff'];
  if (!keys.every((k) => Number.isInteger(ab[k]) && ab[k] >= 0)) return true;
  return keys.reduce((n, k) => n + ab[k], 0) !== b.minutes;
});
const envBad = beats.filter((b) => !(b.fastMinutes < b.minutes && b.minutes < b.deliberateMinutes));
const tcBad = beats.filter((b) => !EXPECT.timeConfidence.includes(b.timeConfidence));
const basisBad = beats.filter((b) => typeof b.authorEstimateBasis !== 'string' || b.authorEstimateBasis.trim().length === 0);
const stBad = beats.filter((b) => !Array.isArray(b.subtasks) || b.subtasks.length < EXPECT.subtasksMin || b.subtasks.length > EXPECT.subtasksMax);

const activitySums = ['exploration', 'reasoning', 'manipulation', 'dialogue', 'payoff'].reduce((o, k) => {
  o[k] = beats.reduce((n, b) => n + (b.activityBudget?.[k] ?? 0), 0); return o;
}, {});
const fastSum = beats.reduce((n, b) => n + b.fastMinutes, 0);
const delibSum = beats.reduce((n, b) => n + b.deliberateMinutes, 0);

check('T-01', 'activityBudget 5키 · 음수 없음 · 합 = minutes', abBad.length === 0, 0, abBad.map((b) => b.id));
check('T-02', 'activityBudget 범주 합 = 480',
  Object.values(activitySums).reduce((a, b) => a + b, 0) === EXPECT.totalMinutes, EXPECT.totalMinutes,
  Object.values(activitySums).reduce((a, b) => a + b, 0), JSON.stringify(activitySums));
check('T-03', 'fastMinutes < minutes < deliberateMinutes', envBad.length === 0, 0, envBad.map((b) => b.id));
check('T-04', "timeConfidence ∈ {low, medium} ('high' 금지 — 표본 0)", tcBad.length === 0, 0, tcBad.map((b) => b.id));
check('T-05', 'authorEstimateBasis 전건 존재', basisBad.length === 0, 0, basisBad.map((b) => b.id));
check('T-06', `subtasks ${EXPECT.subtasksMin}~${EXPECT.subtasksMax}개`, stBad.length === 0, 0, stBad.map((b) => b.id));

// ── 5. 힌트 · 복구 · 저장 ─────────────────────────────────────────────
const hintBad = beats.filter((b) => !Array.isArray(b.hints) || b.hints.length !== EXPECT.hintLevels || b.hints.some((h) => !h || !h.trim()));
const recBad = beats.filter((b) => typeof b.recovery !== 'string' || b.recovery.trim().length === 0);
const cpBad = beats.filter((b) => typeof b.checkpoint !== 'string' || b.checkpoint.trim().length === 0);
check('H-01', '힌트 3단 (빈 문자열 없음)', hintBad.length === 0, 0, hintBad.map((b) => b.id));
check('H-02', 'recovery 비어있지 않음', recBad.length === 0, 0, recBad.map((b) => b.id));
check('H-03', 'checkpoint 존재', cpBad.length === 0, 0, cpBad.map((b) => b.id));

// H-04 (C6-F3 · 디렉터 R7 배정): 1단 힌트 어휘 제약.
// 정본 정의 = `planning/gdd.md` §6 표. 1단(방향)이 주는 것 = "지금 무엇을 결정해야 하는지,
// 어느 구역·도구가 관련 있는지" / 주지 않는 것 = "어떤 자료인지". 2단이 매체 종류와 도구
// 사용 순서를, 3단이 정확한 자료와 조작 값을 준다. H-01은 빈 문자열만 검사했기 때문에
// 1단이 자료명·정답값·인과 단정·조작 지시를 담아도 통과했다(qa/c6-review.md C6-F3).
// 네 부류를 금지한다 — A 자료·매체 지목 / B 정답값·수량 / C 정답 단정 / D 조작 지시.
// 구역 명사와 도구 표시명(배선 추적·판독·조위정합·배수 편성·부식 시험·이중서명)과 그
// 설비명(회로 지도·판독기·판독대·정합기·편성기·시험대·봉인대)은 1단이 줘도 되는 것이므로
// 금지 어휘에 넣지 않는다.
// 한계 [INFERENCE]: 어휘 바닥(lexical floor)이다. 금지 어휘를 쓰지 않으면서 정답 통찰을
// 문장으로 흘리는 1단은 이 검사로 걸리지 않는다 — 그 판정은 사람 검토(QA 렌즈)가 한다.
const HINT1_MEDIA = ['염판', '조위대장', '대장', '일지', '각서', '시편', '사본', '필사본', '인쇄본',
  '보고서', '통지', '증언', '장부', '도면', '서류', '서명지', '요약', '표지', '각인', '곡선',
  '봉우리', '무늬', '투명지', '근무표', '명세', '전표', '수치', '기록지', '표본'];
const HINT1_OP = ['겹쳐', '정렬', '물려', '물린', '접어', '끼워', '맞춰', '걸어', '계산하',
  '대조하', '세어라', '세라', '재라', '적어라', '뽑아', '올려'];
const HINT1_ASSERT = [/아니(라|다)/, /때문/, /뿐이/, /만\s*(보|읽|세|물|봐|걸|골)/];
const HINT1_VALUE = [/[0-9]/, /H[+\-]/,
  /(한|두|세|네|다섯|여섯|일곱|여덟|아홉|열)\s*(개|곳|줄|점|장|번|쌍|칸|구간|가지|명|시간|분|항|종)/];
// 구역 고유명 자체에 숫자가 들어간다(제3수문·제1양수장). 1단은 구역을 말해도 되므로
// 값·수량 검사 전에 이 고유명만 지운다.
const HINT1_ZONE_PROPER = ['제3수문', '제1양수장'];
const hint1Bad = beats.map((b) => {
  const h = String((b.hints ?? [])[0] ?? '');
  const hValue = HINT1_ZONE_PROPER.reduce((s, w) => s.split(w).join(''), h);
  const hits = [
    ...HINT1_MEDIA.filter((w) => h.includes(w)).map((w) => 'A:' + w),
    ...HINT1_VALUE.filter((r) => r.test(hValue)).map(() => 'B:값·수량'),
    ...HINT1_ASSERT.filter((r) => r.test(h)).map((r) => 'C:' + r.source),
    ...HINT1_OP.filter((w) => h.includes(w)).map((w) => 'D:' + w),
  ];
  return { id: b.id, hits };
}).filter((r) => r.hits.length > 0);
check('H-04', '1단 힌트에 자료명·정답값·정답 단정·조작 지시 없음 (gdd §6 1단 정의)',
  hint1Bad.length === 0, 0, hint1Bad.map((r) => `${r.id}(${r.hits.join(',')})`),
  '어휘 바닥 검사 — 의미 유출 일반은 판정하지 않는다');

// ── 6. 단서 · 매체 · 출처 독립성 ──────────────────────────────────────
// 6.1 originId → copiedFrom 지도 (루트 해석용). 표시 라벨이 아니라 계보로 판정한다.
const copyOf = new Map();
const originTypes = new Map();
for (const c of clues) {
  if (c.copiedFrom) {
    if (copyOf.has(c.originId) && copyOf.get(c.originId) !== c.copiedFrom) {
      copyOf.set(c.originId, '__CONFLICT__');
    } else copyOf.set(c.originId, c.copiedFrom);
  }
  if (!originTypes.has(c.originId)) originTypes.set(c.originId, new Set());
  originTypes.get(c.originId).add(c.sourceType);
}
function rootOrigin(id) {
  const seen = new Set();
  let cur = id;
  while (copyOf.has(cur) && copyOf.get(cur) !== '__CONFLICT__') {
    if (seen.has(cur)) return '__CYCLE__';
    seen.add(cur);
    cur = copyOf.get(cur);
  }
  return cur;
}
const catalog = uniq(clues.map((c) => c.originId)).sort();

const typeBad = clues.filter((c) => !EXPECT.sourceTypes.includes(c.sourceType));
const originBad = clues.filter((c) => typeof c.originId !== 'string' || !c.originId);
const copyRefBad = clues.filter((c) => c.copiedFrom && !catalog.includes(c.copiedFrom));
const copyConflict = [...copyOf.entries()].filter(([, v]) => v === '__CONFLICT__').map(([k]) => k);
const copyCycle = catalog.filter((o) => rootOrigin(o) === '__CYCLE__');

// 6.2 모든 비트: 서로 다른 sourceType ≥ 2 (C3-F11 차단 조건)
const mediaPerBeat = beats.map((b) => ({ id: b.id, kinds: uniq((b.clues ?? []).map((c) => c.sourceType)) }));
const mediaShort = mediaPerBeat.filter((m) => m.kinds.length < EXPECT.minMediaKinds);

// 6.3 proofRequired 비트: 루트 originId 상이 AND sourceType 상이인 쌍이 1쌍 이상
function independentPair(b) {
  const cs = b.clues ?? [];
  for (let i = 0; i < cs.length; i++) {
    for (let j = i + 1; j < cs.length; j++) {
      if (cs[i].sourceType !== cs[j].sourceType && rootOrigin(cs[i].originId) !== rootOrigin(cs[j].originId)) {
        return [cs[i].id, cs[j].id];
      }
    }
  }
  return null;
}
const proofBeats = beats.filter((b) => b.proofRequired === true);
const proofFail = proofBeats.filter((b) => independentPair(b) === null);

const sourceTypeDist = EXPECT.sourceTypes.reduce((o, t) => { o[t] = clues.filter((c) => c.sourceType === t).length; return o; }, {});

check('C-01', 'clue sourceType ∈ {log, ledger, plate}', typeBad.length === 0, 0, typeBad.map((c) => c.id));
check('C-02', 'clue originId 전건 부착', originBad.length === 0, 0, originBad.map((c) => c.id));
check('C-03', 'copiedFrom 참조가 카탈로그 안에 있음', copyRefBad.length === 0, 0, copyRefBad.map((c) => c.id));
check('C-04', 'originId → copiedFrom 지도 모순 없음', copyConflict.length === 0, 0, copyConflict);
check('C-05', 'copiedFrom 계보에 순환 없음', copyCycle.length === 0, 0, copyCycle);
check('C-06', `모든 비트 매체 종류 ≥ ${EXPECT.minMediaKinds}`, mediaShort.length === 0, 0,
  mediaShort.map((m) => `${m.id}(${m.kinds.join('/')})`));
check('C-07', 'proofRequired 비트의 독립 쌍 (루트 originId 상이 AND sourceType 상이)',
  proofFail.length === 0, 0, proofFail.map((b) => b.id), `proofRequired 비트 ${proofBeats.length}건`);

// ── 7. 도구 · 학습 이벤트 ─────────────────────────────────────────────
const toolCounts = EXPECT.toolIds.reduce((o, t) => { o[t] = beats.filter((b) => (b.tools ?? []).includes(t)).length; return o; }, {});
const toolIdBad = uniq(beats.flatMap((b) => b.tools ?? [])).filter((t) => !EXPECT.toolIds.includes(t));
const toolless = beats.filter((b) => (b.tools ?? []).length === 0).map((b) => b.id);
const teaching = beats.flatMap((b) => (b.toolTeaching ?? []).map((t) => ({ ...t, beat: b.id })));
const teachByTool = EXPECT.toolIds.reduce((o, t) => {
  o[t] = {
    guided: teaching.filter((x) => x.tool === t && x.mode === 'guided').map((x) => x.beat),
    unguided: teaching.filter((x) => x.tool === t && x.mode === 'unguided').map((x) => x.beat),
  };
  return o;
}, {});
const teachBad = EXPECT.toolIds.filter((t) => teachByTool[t].guided.length !== 1 || teachByTool[t].unguided.length !== 1);
// 도구가 쓰이는 비트의 분 합 (중복 계상 — 480의 분해가 아님)
const toolMinutes = EXPECT.toolIds.reduce((o, t) => {
  o[t] = beats.filter((b) => (b.tools ?? []).includes(t)).reduce((n, b) => n + b.minutes, 0); return o;
}, {});

check('V-01', '도구 id 고정 6종', toolIdBad.length === 0, 0, toolIdBad);
check('V-02', `toolTeaching 총 ${EXPECT.toolTeachingTotal}건`,
  teaching.length === EXPECT.toolTeachingTotal, EXPECT.toolTeachingTotal, teaching.length);
check('V-03', '도구 6종 각각 guided 1 + unguided 1', teachBad.length === 0, 0, teachBad, JSON.stringify(teachByTool));
check('V-04', 'toolTeaching의 도구가 그 비트 tools에 포함',
  teaching.every((t) => (beats.find((b) => b.id === t.beat)?.tools ?? []).includes(t.tool)),
  true, teaching.filter((t) => !(beats.find((b) => b.id === t.beat)?.tools ?? []).includes(t.tool)).map((t) => `${t.beat}:${t.tool}`));

// ── 8. 선행 조건 위상 ─────────────────────────────────────────────────
const order = beats.map((b) => b.id);
const idx = Object.fromEntries(order.map((id, i) => [id, i]));
const preqUnknown = beats.flatMap((b) => (b.prerequisites ?? []).filter((p) => !(p in idx)).map((p) => `${b.id}→${p}`));
const preqForward = beats.flatMap((b) => (b.prerequisites ?? []).filter((p) => (p in idx) && idx[p] >= idx[b.id]).map((p) => `${b.id}→${p}`));
// 위상 정렬로 순환 검출 (전방 참조가 없어도 독립 검사로 남긴다)
function hasCycle() {
  const state = new Map();
  const dfs = (id) => {
    if (state.get(id) === 1) return true;
    if (state.get(id) === 2) return false;
    state.set(id, 1);
    for (const p of (beats.find((b) => b.id === id)?.prerequisites ?? [])) {
      if (p in idx && dfs(p)) return true;
    }
    state.set(id, 2);
    return false;
  };
  return order.some((id) => dfs(id));
}
check('P-01', 'prerequisites 참조 비트 존재', preqUnknown.length === 0, 0, preqUnknown);
check('P-02', 'prerequisites 전건이 앞선 비트를 가리킴', preqForward.length === 0, 0, preqForward);
check('P-03', 'prerequisites 무순환', !hasCycle(), false, hasCycle());

// ── 9. 캐논 문자열 회귀 (RFC-P3-013 · 이전 C3 F1) ─────────────────────
const forbiddenHit = EXPECT.forbiddenStrings.filter((s) => wholeText.includes(s));
const requiredMiss = EXPECT.requiredStrings.filter((s) => !wholeText.includes(s));
const canonMissing = EXPECT.canonTimesPresent.filter((s) => !wholeText.includes(s));
const canonStale = EXPECT.canonTimesAbsent.filter((s) => wholeText.includes(s));
function firstBeatMentioning(term) {
  const hit = beats.find((b) => JSON.stringify(b).includes(term));
  return hit ? hit.id : null;
}
const mentionActual = Object.fromEntries(Object.keys(EXPECT.firstMention).map((k) => [k, firstBeatMentioning(k)]));

check('K-01', '기록 불가 명제 문자열 부재 (이전 C3 F1 회귀)', forbiddenHit.length === 0, 0, forbiddenHit);
check('K-02', "'봉인 완료 접점' 존재", requiredMiss.length === 0, 0, requiredMiss);
check('K-03', 'RFC-P3-013 캐논 시각 존재 (H-1:24 · H-1:04 · H+0:12)', canonMissing.length === 0, 0, canonMissing);
check('K-04', 'RFC-P3-013 폐기 시각 부재 (H-1:20 · H+0:10)', canonStale.length === 0, 0, canonStale);
check('K-05', 'RFC-P3-012 공개 순서 (도연 = t0-b1 · 한서린 = c4-b2)',
  eq(mentionActual, EXPECT.firstMention), EXPECT.firstMention, mentionActual);

// RFC-W4: 4장은 효력 판정까지, 도연의 의도는 6장에서 확정한다.
const beatText = (id) => JSON.stringify(beats.find((b) => b.id === id) ?? {});
const intentAt = (id) => beatText(id).includes(EXPECT.intentPhrase);
const intentExpected = { [EXPECT.intentAbsentBeat]: false, [EXPECT.intentPresentBeat]: true };
const intentActual = { [EXPECT.intentAbsentBeat]: intentAt(EXPECT.intentAbsentBeat), [EXPECT.intentPresentBeat]: intentAt(EXPECT.intentPresentBeat) };
check('K-06', `RFC-W4 의도 문장 위치 ('${EXPECT.intentPhrase}' — ${EXPECT.intentAbsentBeat} 부재 · ${EXPECT.intentPresentBeat} 존재)`,
  eq(intentActual, intentExpected), intentExpected, intentActual);

// ── 10. 구역 파생 집계 (content-matrix §1·§3·§4.3의 단일 출처) ────────
const zoneBeatCounts = EXPECT.zoneIds.reduce((o, z) => { o[z] = beats.filter((b) => b.zoneId === z).length; return o; }, {});
const zoneBeatMinutes = EXPECT.zoneIds.reduce((o, z) => {
  o[z] = beats.filter((b) => b.zoneId === z).reduce((n, b) => n + b.minutes, 0); return o;
}, {});
// D1 = 그 구역이 등장하는 스테이지 수, D2 = 비트 배열 순서에서의 연속 구간(런) 수
const zoneStageSpan = EXPECT.zoneIds.reduce((o, z) => {
  o[z] = uniq(beats.filter((b) => b.zoneId === z).map((b) => b._stage)).length; return o;
}, {});
const zoneRunSeq = beats.reduce((acc, b) => {
  const last = acc[acc.length - 1];
  if (!last || last.zone !== b.zoneId) acc.push({ zone: b.zoneId, from: b.id, to: b.id, beats: 1 });
  else { last.to = b.id; last.beats += 1; }
  return acc;
}, []);
const zoneRuns = EXPECT.zoneIds.reduce((o, z) => { o[z] = zoneRunSeq.filter((r) => r.zone === z).length; return o; }, {});

// ── 11. --pairs (A37 / C3-F12 불파괴 자료쌍 파생) ─────────────────────
if (WANT_PAIRS) {
  const clueInfo = (b, cid) => {
    const c = (b.clues ?? []).find((x) => x.id === cid);
    if (!c) return null;
    return { clueId: c.id, sourceType: c.sourceType, originId: c.originId, rootOriginId: rootOrigin(c.originId), copiedFrom: c.copiedFrom ?? null };
  };
  const pairs = proofBeats.map((b) => {
    const p = independentPair(b);
    return {
      beat: b.id, stage: b._stage, zoneId: b.zoneId ?? null, title: b.title,
      clueCount: (b.clues ?? []).length,
      independentPair: p ? [clueInfo(b, p[0]), clueInfo(b, p[1])] : null,
    };
  });
  const missing = pairs.filter((p) => p.independentPair === null).map((p) => p.beat);
  process.stdout.write(JSON.stringify({
    validator: 'planning/validate-campaign.mjs --pairs',
    purpose: 'A37 / C3-F12 — synopsis/continuity.md §5 K 표의 파생 출처',
    rule: 'C-07과 같은 규칙: 루트 originId 상이 AND sourceType 상이. 사본은 루트를 물려받으므로 원본×사본은 쌍이 되지 못한다',
    file: TARGET, bytes, sha256,
    proofRequiredBeats: proofBeats.length,
    zoneBeatCounts,
    zoneBeatMinutes,
    zoneStageSpan,
    zoneRuns,
    zoneRunCount: zoneRunSeq.length,
    zoneRunSequence: zoneRunSeq.map((r) => `${r.zone}:${r.from}→${r.to}(${r.beats})`),
    beatsWithoutPair: missing,
    pairs,
    notMeasured: ['쌍의 존재는 문서 정합이며 플레이어가 실제로 그 쌍을 찾아내는지는 미측정(n=0)'],
  }, null, 2) + '\n');
  process.exit(missing.length === 0 ? 0 : 1);
}

// ── 12. --t0 <dir> (RFC-C7-001 (2) · T0 인스턴스 데이터 ↔ 캠페인 정합) ──
// 소유: planner. 검사 대상은 systems 소유 파일(`systems/data/t0/*.json`)이며 이 모드는
// 읽기만 한다 — FAIL 이 나면 planner 가 고치지 않고 systems 로 돌려보낸다.
// 정본 순서: 디렉터 RFC > live campaign.json > current 문서(gdd §4 · style-guide §5).
// 이 모드도 문서 정합만 본다. Unity 실행 0회, 사람 표본 n=0.
if (WANT_T0) {
  const tchecks = [];
  const tcheck = (id, name, pass, expected, actual, note) => {
    tchecks.push({ id, name, status: pass ? 'PASS' : 'FAIL', expected, actual, ...(note ? { note } : {}) });
  };
  const files = {};
  const loaded = {};
  let loadErr = null;
  for (const n of ['beats', 'hints', 'records', 'zones', 'tools']) {
    const p = join(T0_DIR, `${n}.json`);
    files[n] = p;
    try { loaded[n] = JSON.parse(readFileSync(p, 'utf8')); }
    catch (err) { loadErr = loadErr ?? `${p}: ${String(err && err.message || err)}`; }
  }
  if (loadErr) {
    process.stdout.write(JSON.stringify({ validator: 'planning/validate-campaign.mjs --t0', dir: T0_DIR, error: loadErr }, null, 2) + '\n');
    process.exit(2);
  }

  // 캠페인 t0 서브셋 (stage.id 는 대문자 'T0', 비트 id 는 소문자 't0-*')
  const t0Stage = stages.find((s) => String(s.id).toLowerCase() === 't0');
  const t0Beats = (t0Stage?.beats ?? []).map((b) => ({ ...b, _stage: t0Stage.id }));
  const t0BeatIds = t0Beats.map((b) => b.id);
  const t0Clues = t0Beats.flatMap((b) => (b.clues ?? []).map((c) => ({ ...c, _beat: b.id })));
  const beatById = new Map(t0Beats.map((b) => [b.id, b]));
  const clueById = new Map(t0Clues.map((c) => [c.id, c]));

  // ── T0-01 · beats ⊂ campaign t0 비트 · completion 존재 ───────────────
  const bRows = loaded.beats.rows ?? [];
  const bIds = bRows.map((r) => r.id);
  const t01 = [];
  for (const id of bIds) if (!t0BeatIds.includes(id)) t01.push(`${id}: campaign T0 에 없는 비트`);
  for (const id of uniq(bIds).filter((id) => bIds.filter((x) => x === id).length > 1)) t01.push(`${id}: beats.json 중복 행`);
  for (const r of bRows) {
    const src = beatById.get(r.id);
    if (!src) continue;
    if (typeof r.completion !== 'string' || r.completion.trim().length === 0) t01.push(`${r.id}: completion 없음/빈 문자열`);
    else if (r.completion !== src.completion) t01.push(`${r.id}: completion ≠ campaign.completion`);
    if (!r.completionPredicate || typeof r.completionPredicate !== 'object') t01.push(`${r.id}: completionPredicate 없음`);
    else if (r.completionPredicate._srcCompletion !== src.completion) t01.push(`${r.id}: completionPredicate._srcCompletion ≠ campaign.completion`);
    for (const k of ['kind', 'zoneId', 'minutes', 'proofRequired']) {
      if (!eq(r[k], src[k])) t01.push(`${r.id}.${k}: ${JSON.stringify(r[k])} ≠ campaign ${JSON.stringify(src[k])}`);
    }
  }
  // 확정(인용 고정) 명령을 내는 비트 = proofRequired 비트 (RFC-C7-001 (1)·(3))
  const emits = bRows.filter((r) => r.completionPredicate?.emitsCommitCommand === true).map((r) => r.id);
  const proofT0 = t0Beats.filter((b) => b.proofRequired === true).map((b) => b.id);
  if (!eq(emits, proofT0)) t01.push(`emitsCommitCommand ${JSON.stringify(emits)} ≠ campaign proofRequired ${JSON.stringify(proofT0)}`);
  if (!eq(loaded.beats.commitCommandBeats ?? [], emits)) t01.push(`commitCommandBeats ${JSON.stringify(loaded.beats.commitCommandBeats)} ≠ emitsCommitCommand ${JSON.stringify(emits)}`);
  if (!eq(loaded.beats.stage?.zoneIds ?? [], t0Stage?.zoneIds ?? [])) t01.push(`stage.zoneIds ${JSON.stringify(loaded.beats.stage?.zoneIds)} ≠ campaign ${JSON.stringify(t0Stage?.zoneIds)}`);
  tcheck('T0-01', 'beats.json ⊂ campaign T0 비트 · completion(및 술어 원문) 존재·일치',
    t01.length === 0, { beats: t0BeatIds, completionNonEmpty: true }, t01.length === 0 ? { beats: bIds, commitCommandBeats: emits } : t01);

  // ── T0-02 · hints 33/33 · t0 3비트 × 3단 일치 ────────────────────────
  const allBeats33 = beats;
  const hint33 = allBeats33.filter((b) => Array.isArray(b.hints) && b.hints.length === EXPECT.hintLevels && b.hints.every((h) => h && h.trim()));
  const hRows = loaded.hints.rows ?? [];
  const t02 = [];
  if (hint33.length !== allBeats33.length) t02.push(`campaign 3단 힌트 ${hint33.length}/${allBeats33.length} (33/33 아님)`);
  if (hRows.length !== t0BeatIds.length * EXPECT.hintLevels) t02.push(`hints.json 행 ${hRows.length} ≠ ${t0BeatIds.length * EXPECT.hintLevels}`);
  const SCOPE = { 1: 'direction', 2: 'procedure', 3: 'solution' };
  for (const bid of t0BeatIds) {
    for (let lv = 1; lv <= EXPECT.hintLevels; lv += 1) {
      const row = hRows.find((r) => r.beatId === bid && r.level === lv);
      if (!row) { t02.push(`${bid} ${lv}단 행 없음`); continue; }
      if (row.hintId !== `${bid}-h${lv}`) t02.push(`${row.hintId} ≠ ${bid}-h${lv}`);
      const want = (beatById.get(bid)?.hints ?? [])[lv - 1];
      if (row.sourceTextKo !== want) t02.push(`${bid}-h${lv}: sourceTextKo ≠ campaign hints[${lv - 1}]`);
      if (row.revealScope !== SCOPE[lv]) t02.push(`${bid}-h${lv}: revealScope ${row.revealScope} ≠ ${SCOPE[lv]}`);
      if (lv === 1 && row.revealsValues !== false) t02.push(`${bid}-h1: revealsValues 는 false 여야 한다 (gdd §6 1단 정의 · H-04)`);
    }
  }
  const strayHint = hRows.filter((r) => !t0BeatIds.includes(r.beatId)).map((r) => r.hintId);
  if (strayHint.length) t02.push(`t0 밖 비트 참조: ${strayHint.join(',')}`);
  tcheck('T0-02', 'campaign 힌트 33/33 3단 · t0 3비트 × 3단이 hints.json 과 문자열 일치',
    t02.length === 0, { campaign: `${allBeats33.length}/${allBeats33.length}`, t0Rows: t0BeatIds.length * EXPECT.hintLevels },
    t02.length === 0 ? { campaign: `${hint33.length}/${allBeats33.length}`, t0Rows: hRows.length } : t02);

  // ── T0-03 · records ↔ campaign 단서 1:1 · 매체 2종 ───────────────────
  const rRows = loaded.records.rows ?? [];
  const t03 = [];
  const refd = rRows.flatMap((r) => r.clueIds ?? []);
  const dupRef = uniq(refd.filter((c) => refd.filter((x) => x === c).length > 1));
  const unknownRef = refd.filter((c) => !clueById.has(c));
  const uncovered = t0Clues.map((c) => c.id).filter((c) => !refd.includes(c));
  if (dupRef.length) t03.push(`두 record 가 같은 단서를 주장: ${dupRef.join(',')}`);
  if (unknownRef.length) t03.push(`campaign T0 에 없는 단서 참조: ${unknownRef.join(',')}`);
  if (uncovered.length) t03.push(`record 가 없는 단서: ${uncovered.join(',')}`);
  for (const r of rRows) {
    for (const cid of r.clueIds ?? []) {
      const c = clueById.get(cid);
      if (!c) continue;
      if (r.sourceType !== c.sourceType) t03.push(`${r.recordId}/${cid}: sourceType ${r.sourceType} ≠ ${c.sourceType}`);
      if (r.originId !== c.originId) t03.push(`${r.recordId}/${cid}: originId ${r.originId} ≠ ${c.originId}`);
      if (r.medium !== c.sourceType) t03.push(`${r.recordId}/${cid}: medium ${r.medium} ≠ sourceType ${c.sourceType}`);
      if (r.rootOriginId !== rootOrigin(c.originId)) t03.push(`${r.recordId}/${cid}: rootOriginId ${r.rootOriginId} ≠ ${rootOrigin(c.originId)} (사본은 루트 승계 · RFC-S5)`);
    }
    if (!EXPECT.sourceTypes.includes(r.sourceType)) t03.push(`${r.recordId}: sourceType ${r.sourceType} ∉ ${JSON.stringify(EXPECT.sourceTypes)}`);
  }
  // 매체 2종: T0 전체 그리고 비트별로 서로 다른 sourceType 이 2종 이상
  const mediaAll = uniq(rRows.map((r) => r.sourceType));
  if (mediaAll.length < EXPECT.minMediaKinds) t03.push(`T0 records 매체 ${mediaAll.length}종 < ${EXPECT.minMediaKinds}`);
  const mediaPerBeatT0 = t0BeatIds.map((bid) => {
    const kinds = uniq(rRows.filter((r) => (r.clueIds ?? []).some((c) => clueById.get(c)?._beat === bid)).map((r) => r.sourceType));
    return { beat: bid, kinds };
  });
  for (const m of mediaPerBeatT0) if (m.kinds.length < EXPECT.minMediaKinds) t03.push(`${m.beat}: records 매체 ${m.kinds.length}종 < ${EXPECT.minMediaKinds}`);
  tcheck('T0-03', 'records.clueIds·originId·sourceType ↔ campaign 단서 1:1 · 매체 2종',
    t03.length === 0,
    { clues: t0Clues.map((c) => c.id), minMediaKinds: EXPECT.minMediaKinds },
    t03.length === 0 ? { records: rRows.map((r) => r.recordId), mediaKinds: mediaAll, mediaPerBeat: mediaPerBeatT0 } : t03);

  // ── T0-04 · zones.hub systemIds ⊂ 계통 목록 · cameraPose = style-guide §5 ──
  // 계통 목록 [INFERENCE]: `systems/data-schemas/zones.md` 는 계통을 열거하지 않고
  // Z-I2("systemIds ⊂ plates 저작본 systemId")만 둔다. zones.json `_src.systemIds` 는
  // "계통 id 는 구역 토큰을 그대로 쓴다(캐논은 계통에 고유명을 주지 않는다)"라고 파생 규칙을
  // 적었으므로, 이 검사는 계통 목록 = 캠페인 고정 5구역 토큰으로 본다. 고유명이 생기면 정본은
  // worldview 이며 이 상수를 고쳐야 한다.
  const CAM = { fovDeg: 54, fovAxis: 'horizontal', eyeHeightM: 1.55, pitchDeg: -18, rollDeg: 0, yawOptionsDeg: [-30, 0, 30], fillRatio: 0.7 };
  const zRows = loaded.zones.rows ?? [];
  const t04 = [];
  const hub = zRows.find((z) => z.zoneId === 'hub');
  if (!hub) t04.push('zones.json 에 hub 행 없음');
  const badZone = zRows.map((z) => z.zoneId).filter((z) => !EXPECT.zoneIds.includes(z));
  if (badZone.length) t04.push(`zoneId ∉ 고정 5구역: ${badZone.join(',')}`);
  if (hub) {
    const outside = (hub.systemIds ?? []).filter((s) => !EXPECT.zoneIds.includes(s));
    if (outside.length) t04.push(`hub.systemIds ∉ 계통 목록(구역 토큰): ${outside.join(',')}`);
    if ((hub.systemIds ?? []).length === 0) t04.push('hub.systemIds 비어 있음');
    for (const v of hub.viewNodes ?? []) {
      const p = v.cameraPose;
      if (!p) { t04.push(`${v.nodeId}: cameraPose 없음`); continue; }
      if (p.fovDeg !== CAM.fovDeg) t04.push(`${v.nodeId}.fovDeg ${p.fovDeg} ≠ ${CAM.fovDeg}`);
      if (p.fovAxis !== CAM.fovAxis) t04.push(`${v.nodeId}.fovAxis ${p.fovAxis} ≠ ${CAM.fovAxis}`);
      if (p.pos?.z !== CAM.eyeHeightM) t04.push(`${v.nodeId}.pos.z ${p.pos?.z} ≠ 시선 높이 ${CAM.eyeHeightM}`);
      if (p.rot?.pitchDeg !== CAM.pitchDeg) t04.push(`${v.nodeId}.pitchDeg ${p.rot?.pitchDeg} ≠ ${CAM.pitchDeg}`);
      if (p.rot?.rollDeg !== CAM.rollDeg) t04.push(`${v.nodeId}.rollDeg ${p.rot?.rollDeg} ≠ ${CAM.rollDeg}`);
      if (!CAM.yawOptionsDeg.includes(p.rot?.yawDeg)) t04.push(`${v.nodeId}.yawDeg ${p.rot?.yawDeg} ∉ ${JSON.stringify(CAM.yawOptionsDeg)}`);
    }
    if ((hub.viewNodes ?? []).length === 0) t04.push('hub.viewNodes 비어 있음');
  }
  const cc = loaded.zones.cameraConstants ?? {};
  for (const k of ['fovDeg', 'fovAxis', 'eyeHeightM', 'pitchDeg', 'rollDeg', 'fillRatio']) {
    if (!eq(cc[k], CAM[k])) t04.push(`cameraConstants.${k} ${JSON.stringify(cc[k])} ≠ style-guide §5 ${JSON.stringify(CAM[k])}`);
  }
  if (!eq(cc.yawOptionsDeg, CAM.yawOptionsDeg)) t04.push(`cameraConstants.yawOptionsDeg ${JSON.stringify(cc.yawOptionsDeg)} ≠ ${JSON.stringify(CAM.yawOptionsDeg)}`);
  tcheck('T0-04', 'zones.hub.systemIds ⊂ 계통 목록 · 전 viewNode cameraPose = style-guide §5 수치',
    t04.length === 0, CAM,
    t04.length === 0 ? { systemIds: hub?.systemIds, viewNodes: (hub?.viewNodes ?? []).length, yawUsed: uniq((hub?.viewNodes ?? []).map((v) => v.cameraPose?.rot?.yawDeg)) } : t04,
    '계통 목록 = 캠페인 고정 5구역 토큰 [INFERENCE] — zones.md 는 계통을 열거하지 않는다(Z-I2 만 있다)');

  // ── T0-05 · tools circuit/reader 확정 조건 ↔ gdd §4 표 ───────────────
  const t05 = [];
  let gdd = '';
  try { gdd = readFileSync(join(HERE, 'gdd.md'), 'utf8'); }
  catch (err) { t05.push(`gdd.md 읽기 실패: ${String(err && err.message || err)}`); }
  const gddCell = (toolId) => {
    const line = gdd.split('\n').find((l) => l.startsWith('|') && l.includes('`' + toolId + '`') && /^\|\s*\d+\s*\|/.test(l));
    if (!line) return null;
    const cells = line.split('|').map((c) => c.trim());
    return cells[cells.length - 2] ?? null; // 마지막 열 = 확정 조건(1문장)
  };
  const gddCircuit = gddCell('circuit');
  const gddReader = gddCell('reader');
  if (!gddCircuit) t05.push('gdd §4 표에서 circuit 행을 찾지 못했다');
  if (!gddReader) t05.push('gdd §4 표에서 reader 행을 찾지 못했다');
  const toolRow = (id) => (loaded.tools.rows ?? []).find((r) => r.toolId === id);
  const circuit = toolRow('circuit');
  const reader = toolRow('reader');
  if (!circuit) t05.push('tools.json 에 circuit 행 없음');
  if (!reader) t05.push('tools.json 에 reader 행 없음');
  // circuit: gdd 확정 조건 셀이 "— (해당 없음)" = 확정 명령 없음
  if (circuit && gddCircuit) {
    const gddNone = /해당\s*없음/.test(gddCircuit) || gddCircuit === '—';
    if (!gddNone) t05.push(`gdd circuit 확정 조건이 '해당 없음' 이 아니다: ${JSON.stringify(gddCircuit)}`);
    if (circuit.hasCommit !== false) t05.push(`circuit.hasCommit ${circuit.hasCommit} ≠ false (gdd §4 「확정 없음(판독 전용)」)`);
    if (circuit.commitCondition !== null) t05.push(`circuit.commitCondition ${JSON.stringify(circuit.commitCondition)} ≠ null`);
  }
  // reader: gdd 확정 조건 셀의 근거 슬롯 3개가 tools 규칙 문자열에 모두 있어야 한다.
  // '매체' 는 스키마 필드명 'sourceType' 으로 표기되는 것을 허용한다(같은 축, campaign 필드명).
  const SLOT_ALIAS = { '매체': ['매체', 'sourceType'], '계통': ['계통'], '관측소': ['관측소'] };
  let readerRule = null;
  if (reader && gddReader) {
    if (reader.hasCommit !== true) t05.push(`reader.hasCommit ${reader.hasCommit} ≠ true (RFC-C7-001 (1) — T0 유일 확정 명령)`);
    readerRule = reader.commitCondition?.rule ?? null;
    if (reader.commitCondition?.commandId !== 'CiteToBoard') t05.push(`reader.commitCondition.commandId ${JSON.stringify(reader.commitCondition?.commandId)} ≠ 'CiteToBoard'`);
    if (typeof readerRule !== 'string' || !readerRule.trim()) t05.push('reader.commitCondition.rule 없음');
    else {
      for (const [slot, aliases] of Object.entries(SLOT_ALIAS)) {
        const inGdd = gddReader.includes(slot);
        const inTool = aliases.some((a) => readerRule.includes(a));
        if (inGdd && !inTool) t05.push(`reader 확정 조건 슬롯 '${slot}' 이 tools.json 규칙에 없다`);
        if (!inGdd && inTool) t05.push(`tools.json 이 gdd §4 에 없는 슬롯 '${slot}' 을 요구한다`);
      }
      if (!/출처/.test(readerRule)) t05.push("reader 규칙에 '출처' 없음 (gdd §4 「… 출처가 채워졌을 때」)");
      const goes = reader.commitCondition?.goesThrough ?? [];
      for (const step of ['사전 체크포인트', '저장', '되돌림']) {
        if (!goes.some((g) => g.includes(step.replace('사전 ', '')))) t05.push(`goesThrough 에 '${step}' 없음 (RFC-C7-001 (1) — 인용 고정은 체크포인트·저장·롤백·되돌림을 모두 거친다)`);
      }
    }
  }
  const literalMatch = readerRule !== null && gddReader !== null && readerRule === gddReader;
  tcheck('T0-05', 'tools circuit/reader 확정 조건이 gdd §4 표와 일치',
    t05.length === 0,
    { circuit: gddCircuit, reader: gddReader },
    t05.length === 0 ? { circuit: { hasCommit: circuit?.hasCommit, commitCondition: circuit?.commitCondition }, reader: { hasCommit: reader?.hasCommit, commandId: reader?.commitCondition?.commandId, rule: readerRule }, literalMatch } : t05,
    "일치 판정은 슬롯 단위다(매체·계통·관측소 + '출처'). tools.json 은 '매체'를 스키마 필드명 'sourceType' 으로 적으므로 문자 단위 동일은 아니다 — literalMatch 필드로 보고한다 [OBSERVED]");

  const tFailed = tchecks.filter((c) => c.status === 'FAIL');
  process.stdout.write(JSON.stringify({
    validator: 'planning/validate-campaign.mjs --t0',
    contract: 'RFC-C7-001 (2) — T0 인스턴스 데이터 ↔ campaign.json / gdd §4 / style-guide §5 정합',
    owner: 'planner (검사) · systems (피검사 파일)',
    campaignFile: TARGET,
    campaignBytes: bytes,
    campaignSha256: sha256,
    t0Dir: T0_DIR,
    t0Files: files,
    sourceShaDeclared: {
      beats: loaded.beats.sourceSha256 ?? null,
      hints: loaded.hints.sourceSha256 ?? null,
      records: loaded.records.sourceSha256 ?? null,
    },
    sourceShaMatchesLiveCampaign: [loaded.beats.sourceSha256, loaded.hints.sourceSha256, loaded.records.sourceSha256]
      .filter((s) => typeof s === 'string').every((s) => s === sha256),
    summary: { checks: tchecks.length, pass: tchecks.length - tFailed.length, fail: tFailed.length, verdict: tFailed.length === 0 ? 'PASS' : 'FAIL' },
    checks: tchecks,
    notMeasured: [
      'Unity 실행 0회 — 이 데이터가 런타임에 로드되는지·술어가 실제로 판정되는지 미측정',
      '사람 플레이 표본 n=0 — 힌트 실효성·카메라 프레이밍 가독성·조작감 전부 미측정',
      'loadCostMb · 프레임 시간 등 성능 값은 이 검사 대상이 아니다(hw_profile_id 미확정 · PRE-1)',
    ],
  }, null, 2) + '\n');
  process.exit(tFailed.length === 0 ? 0 : 1);
}

// ── 출력 ──────────────────────────────────────────────────────────────
const failed = checks.filter((c) => c.status === 'FAIL');
const report = {
  validator: 'planning/validate-campaign.mjs',
  contract: 'RFC-P3-008 계보 B (live planning/campaign.json)',
  file: TARGET,
  bytes,
  sha256,
  summary: {
    checks: checks.length,
    pass: checks.length - failed.length,
    fail: failed.length,
    verdict: failed.length === 0 ? 'PASS' : 'FAIL',
  },
  aggregates: {
    stages: stages.length,
    stageMinutes,
    totalMinutes: sumBeats,
    beats: beats.length,
    kindCounts,
    clues: clues.length,
    sourceTypeDist,
    originCatalogSize: catalog.length,
    activityBudgetSums: activitySums,
    fastMinutesSum: fastSum,
    deliberateMinutesSum: delibSum,
    toolBeatCounts: toolCounts,
    toolBeatMinutes: toolMinutes,
    toollessBeats: toolless,
    proofRequiredBeats: proofBeats.length,
    zoneBeatCounts,
    zoneBeatMinutes,
    zoneStageSpan,
    zoneRuns,
    zoneRunCount: zoneRunSeq.length,
    zoneRunSequence: zoneRunSeq.map((r) => `${r.zone}:${r.from}→${r.to}(${r.beats})`),
    mediaKindsPerBeatMin: Math.min(...mediaPerBeat.map((m) => m.kinds.length)),
    timeConfidenceCounts: { low: beats.filter((b) => b.timeConfidence === 'low').length, medium: beats.filter((b) => b.timeConfidence === 'medium').length, high: beats.filter((b) => b.timeConfidence === 'high').length },
  },
  checks,
  notMeasured: [
    '사람 플레이 표본 n=0 — 완주 분·힌트 실효성·도달성·이탈률 전부 미측정',
    '이 검증기는 문서 정합만 본다. G2/G4/G5/G6/G7을 올리지 않는다',
  ],
};
process.stdout.write(JSON.stringify(report, null, 2) + '\n');
process.exit(failed.length === 0 ? 0 : 1);
