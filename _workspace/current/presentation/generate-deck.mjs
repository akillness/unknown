#!/usr/bin/env node
/**
 * generate-deck.mjs
 *
 * 내부 사전제작 + Steam 등록/수익화 브리핑 덱(36장)을 의존성 없이 생성한다.
 *
 * 사용법 (어느 cwd에서 실행해도 동작한다):
 *   node <이 파일 경로> --out /absolute/output.html [--out /another/copy.html]
 *
 * 규칙
 * - 원본 문서/JSON은 import.meta.url 기준 경로로 찾는다. 사용자 경로 하드코딩 없음.
 * - 출력 경로는 CLI 인자로만 받는다. 인자가 없으면 모듈 옆에 기본 파일을 쓴다.
 * - Node 내장 fs/path/url만 사용한다. 네트워크 접근 없음.
 * - 수치는 빌드 시점에 JSON에서 다시 읽어 계산한다. 슬라이드에 손으로 적지 않는다.
 */

import { readFileSync, existsSync, writeFileSync, mkdirSync } from 'node:fs';
import path from 'node:path';
import { fileURLToPath } from 'node:url';

const HERE = path.dirname(fileURLToPath(import.meta.url));
const ROOT = path.resolve(HERE, '..'); // _workspace/current

/* ------------------------------------------------------------------ *
 * 1. 입력
 * ------------------------------------------------------------------ */

function readJson(rel) {
  const p = path.join(ROOT, rel);
  if (!existsSync(p)) throw new Error('필수 원본 없음: ' + rel);
  return JSON.parse(readFileSync(p, 'utf8'));
}
function readJsonOptional(rel) {
  const p = path.join(ROOT, rel);
  if (!existsSync(p)) return null;
  try {
    return JSON.parse(readFileSync(p, 'utf8'));
  } catch (e) {
    return null;
  }
}

const eco = readJson('product/economics.json');
const est = readJson('production/production-estimate.json');
const cam = readJson('planning/campaign.json');
const ledger = readJsonOptional('production/cycle-ledger.json');

/* ------------------------------------------------------------------ *
 * 2. 계산 (전체 정밀도 유지, 표시할 때만 반올림)
 * ------------------------------------------------------------------ */

const VAT = eco.vatKR;
const DISC = eco.launchDiscount;
const REFUND = eco.refundRate;
const CHARGE = eco.chargebackRate;
const SHARE = eco.developerShareAssumption;
const RESERVE = eco.perUnitReserve;

function netPerUnit(price, discount, k, refund, charge, share, reserve) {
  return (
    (price * (1 - discount)) / (1 + VAT) * k * (1 - refund - charge) * share - reserve
  );
}
function breakeven(budget, net) {
  if (!(net > 0)) return null;
  return Math.ceil(budget / net);
}

const PRICES = eco.prices.slice();
const KS = eco.regionalFactors.slice();
const DEFAULT_PRICE = PRICES[0];

const unitTable = PRICES.map((p) => ({
  price: p,
  paid: p * (1 - DISC),
  nets: KS.map((k) => netPerUnit(p, DISC, k, REFUND, CHARGE, SHARE, RESERVE)),
}));

// 생산 견적
const DAY_FIELDS = [
  'designDays',
  'writingDays',
  'implementationDays',
  'artDays',
  'presentationDays',
  'qaDays',
];
const rowDays = est.rows.reduce(
  (a, r) => a + DAY_FIELDS.reduce((x, f) => x + (Number(r[f]) || 0), 0),
  0
);
const baseDays = rowDays + (est.coordinationDays || 0) + (est.launchAdminDays || 0);
const contDays = baseDays * (1 + est.contingencyRate);
const totalDays = Math.ceil(contDays);
const laborCost = totalDays * est.blendedLaborCostPerDay;
const artDays = est.rows.reduce((a, r) => a + (Number(r.artDays) || 0), 0);

// 캠페인
const stages = cam.stages.map((s) => ({
  id: s.id,
  title: s.title,
  minutes: s.minutes,
  zones: (s.zoneIds || []).join(', '),
  phase: s.storyPhase,
  beats: (s.beats || []).length,
  tools: Array.from(new Set((s.beats || []).flatMap((b) => b.tools || []))),
}));
const campaignMinutes = stages.reduce((a, s) => a + s.minutes, 0);
const beatCount = stages.reduce((a, s) => a + s.beats, 0);
const kindMinutes = {};
for (const s of cam.stages)
  for (const b of s.beats || [])
    kindMinutes[b.kind] = (kindMinutes[b.kind] || 0) + (b.minutes || 0);
const allTools = Array.from(new Set(stages.flatMap((s) => s.tools))).sort();
const allZones = Array.from(
  new Set(cam.stages.flatMap((s) => s.zoneIds || []))
).sort();
const playtestN = Array.isArray(cam.humanPlaytests) ? cam.humanPlaytests.length : 0;
const firstStage = stages[0];
const restMinutes = campaignMinutes - firstStage.minutes;

// --- C3 수정 루프(2026-09-10) 추가 집계. 전부 live 원본에서 빌드 시점에 다시 계산한다. ---
const allBeats = cam.stages.flatMap((s) => s.beats || []);
const toolBeatCounts = {};
for (const b of allBeats)
  for (const t of b.tools || []) toolBeatCounts[t] = (toolBeatCounts[t] || 0) + 1;
const toollessBeats = allBeats.filter((b) => !(b.tools || []).length);
const toollessIds = toollessBeats.map((b) => b.id);
const clueCount = allBeats.reduce((a, b) => a + (b.clues || []).length, 0);
const fastSum = allBeats.reduce((a, b) => a + (b.fastMinutes || 0), 0);
const deliberateSum = allBeats.reduce((a, b) => a + (b.deliberateMinutes || 0), 0);

// UI 계약 집계. 파일이 없거나 키가 없으면 숫자를 지어내지 않고 미측정으로 적는다.
const uic = readJsonOptional('systems/game-ui-contract.json');
const uiCount = (k) =>
  !uic || !uic[k] ? null : Array.isArray(uic[k]) ? uic[k].length : Object.keys(uic[k]).length;
const uiScreens = uiCount('screens');
const uiDecisions = uiCount('player_decisions');
const uiBindings = uiCount('data_bindings');
const uiMatrixRows =
  uic && uic.verification && Array.isArray(uic.verification.matrix)
    ? uic.verification.matrix.length
    : null;
const nOrX = (v) => (v === null ? '미측정' : String(v));

// 불가침 6법. RFC-P3-014: 호명 문구의 정본은 worldview/worldview-bible.md 3절 표 하나뿐이다.
// 덱은 그 표를 빌드 시점에 읽어 그대로 옮기고, 여기서 다시 쓰지 않는다.
function readLaws() {
  const p = path.join(ROOT, 'worldview/worldview-bible.md');
  if (!existsSync(p)) throw new Error('필수 원본 없음: worldview/worldview-bible.md');
  const md = readFileSync(p, 'utf8');
  const sec = md.split('\n## ').find((x) => /^3\.\s/.test(x));
  if (!sec) throw new Error('worldview-bible.md 3절(불가침 6법)을 찾지 못했다');
  const laws = sec
    .split('\n')
    .filter((l) => /^\|\s*[1-6]\s/.test(l))
    .map((l) => l.replace(/^\|/, '').replace(/\|\s*$/, '').split('|').map((c) => c.trim()))
    .filter((r) => r.length === 4)
    .map((r) => ({ name: r[0], rule: r[1], act: r[2], recovery: r[3] }));
  if (laws.length !== 6) throw new Error('6법 정본 표 파싱 실패: ' + laws.length + '행');
  return laws;
}
const LAWS = readLaws();

// 구역 표시명. C5-F1: T0 포함 범위를 슬라이드에 손으로 적지 않는다.
// zoneId 값의 소유자는 planning/campaign.json 이고, KO 명사는 worldview/glossary.md 6-1절
// 대응표가 유일한 정본이다. 덱은 두 원본을 빌드 시점에 읽어 연결만 한다.
function readZoneNames() {
  const p = path.join(ROOT, 'worldview/glossary.md');
  if (!existsSync(p)) throw new Error('필수 원본 없음: worldview/glossary.md');
  const md = readFileSync(p, 'utf8');
  const sec = md.split('\n## ').find((x) => /^6-1\./.test(x));
  if (!sec) throw new Error('glossary.md 6-1절(zoneId 대응표)을 찾지 못했다');
  const map = {};
  for (const l of sec.split('\n')) {
    const m = l.match(/^\|\s*`([a-z]+)`\s*\|([^|]+)\|/);
    if (m) map[m[1]] = m[2].trim().replace(/\s*\([^)]*\)\s*$/, '');
  }
  if (!Object.keys(map).length) throw new Error('zoneId 대응표 파싱 실패');
  return map;
}
const ZONE_KO = readZoneNames();
const zoneKo = (id) => ZONE_KO[id] || id;

// 기술 인수 테스트 수. C5-F5: 슬라이드에 숫자를 손으로 적지 않는다(deck-outline.md L13).
// 정본은 systems/unity-implementation.md 11절 표 하나뿐이고, 덱은 빌드 시점에 그 표의
// T-NN id 를 세기만 한다. 표가 늘거나 줄면 슬라이드가 따라간다.
function readAcceptanceTestCount() {
  const p = path.join(ROOT, 'systems/unity-implementation.md');
  if (!existsSync(p)) throw new Error('필수 원본 없음: systems/unity-implementation.md');
  const md = readFileSync(p, 'utf8');
  const sec = md.split('\n## ').find((x) => /^11\.\s/.test(x));
  if (!sec) throw new Error('unity-implementation.md 11절(기술 인수 테스트)을 찾지 못했다');
  const ids = new Set();
  for (const l of sec.split('\n')) {
    const m = l.match(/^\|\s*(?:`R\d`\s*)?(T-\d{2})\s*\|/);
    if (m) ids.add(m[1]);
  }
  if (!ids.size) throw new Error('11절 인수 테스트 표 파싱 실패');
  return ids.size;
}
const ACCEPT_TESTS = readAcceptanceTestCount();
// 받침에 따라 와/과를 고른다. 라틴 토큰은 받침 없음으로 보아 기존 표기(' 와 ')를 유지한다.
const hasJong = (w) => {
  const c = String(w).charCodeAt(String(w).length - 1);
  return c >= 0xac00 && c <= 0xd7a3 ? (c - 0xac00) % 28 !== 0 : false;
};
const joinKo = (arr) =>
  arr.reduce((acc, cur, i) => (i === 0 ? cur : acc + (hasJong(arr[i - 1]) ? ' 과 ' : ' 와 ') + cur), '');

// 시간 수용 상수. 정본은 production/premium-preproduction-contract.md 의 Time acceptance 절이며
// RFC-P3-011 판정이다. 숫자를 문자열로 굳히지 않고, 계약 본문이 같은 값을 유지하는지 빌드에서 검사한다.
const TIME = { lo: 450, hi: 540, withdrawMedian: 420, withdrawP25: 360, key: 'total_minus_afk_min' };
const TIME_BAND_TEXT = TIME.lo + '분에서 ' + TIME.hi + '분';
const contractText = existsSync(path.join(ROOT, 'production/premium-preproduction-contract.md'))
  ? readFileSync(path.join(ROOT, 'production/premium-preproduction-contract.md'), 'utf8')
  : '';
// 수직 슬라이스 T0의 포함 범위는 상수가 아니라 live planning/campaign.json stages[0] 에서만
// 파생한다 (C5-F1). 정본 서술은 systems/unity-implementation.md 10절이며, 두 원본이 어긋나면
// 아래 selfCheck 의 T0 범위 게이트가 빌드를 실패시킨다. 구역 수를 손으로 세지 않는다.
const sliceStage = cam.stages[0];
const sliceZoneIds = (sliceStage.zoneIds || []).slice();
const sliceTools = firstStage.tools.slice();
const sliceBeats = sliceStage.beats || [];
// 결론 = 근거 제시가 요구되는 비트(proofRequired). 10절의 "T0의 결론 판정" 과 같은 대상이다.
const sliceProofBeats = sliceBeats.filter((b) => b.proofRequired).length;
const sliceHintTiers = sliceBeats.reduce((a, b) => Math.max(a, (b.hints || []).length), 0);
const sliceZoneText = joinKo(sliceZoneIds.map(zoneKo));
const sliceToolText = sliceTools.length ? joinKo(sliceTools) : '없음';
const otherZoneCount = Math.max(allZones.length - sliceZoneIds.length, 0);
const monthsSolo = Math.floor((totalDays / 20) * 10) / 10;

// 하한 9,000원을 지키는 최대 할인율 (표시가별로 다르다)
const FLOOR = eco.launchFloor;
function maxDiscountPct(price) {
  return Math.floor((1 - FLOOR / price) * 100);
}
const capB1 = maxDiscountPct(DEFAULT_PRICE);

// 기본 시뮬레이터 값
const defNet = netPerUnit(DEFAULT_PRICE, DISC, KS[0], REFUND, CHARGE, SHARE, RESERVE);
const defBeCash = breakeven(eco.cashBudgets[0], defNet);
const defBeLabor = breakeven(laborCost, defNet);

/* ------------------------------------------------------------------ *
 * 3. 표기 도우미
 * ------------------------------------------------------------------ */

const nf = new Intl.NumberFormat('ko-KR');
const won = (n) => nf.format(Math.round(n)) + '원';
const num = (n) => nf.format(n);
const pct = (x) => Math.round(x * 1000) / 10 + '%';
const esc = (s) =>
  String(s).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');

/* ------------------------------------------------------------------ *
 * 4. 원본 도해 (전부 이 파일에서 직접 작성한 인라인 SVG)
 * ------------------------------------------------------------------ */

function svgHarbor() {
  return [
    '<svg class="dia" viewBox="0 0 640 380" role="img" aria-label="은포항 3중 방어와 허브 그리고 4구역의 관계 도해">',
    '<defs><pattern id="hatch" width="8" height="8" patternUnits="userSpaceOnUse" patternTransform="rotate(35)">',
    '<line x1="0" y1="0" x2="0" y2="8" stroke="#3F7C77" stroke-width="1.2" opacity="0.55"/></pattern></defs>',
    // 바다
    '<rect x="0" y="0" width="640" height="96" fill="url(#hatch)" opacity="0.5"/>',
    '<path d="M0 74 C 80 62, 160 88, 240 74 S 400 60, 480 76 S 600 88, 640 72" fill="none" stroke="#6FB3A8" stroke-width="2.4"/>',
    '<path d="M0 92 C 90 82, 170 104, 250 92 S 410 80, 500 96 S 610 104, 640 90" fill="none" stroke="#6FB3A8" stroke-width="1.2" opacity="0.6"/>',
    '<text x="16" y="30" class="dl dl-s">외해 / 대조차 9.2 m</text>',
    // 1차 방조제
    '<path d="M20 128 C 160 100, 480 100, 620 128" fill="none" stroke="#E9E4D8" stroke-width="6" stroke-linecap="round"/>',
    '<text x="20" y="150" class="dl">1차 외곽 방조제</text>',
    // 2차 갑문/수문
    '<rect x="292" y="150" width="56" height="34" rx="3" fill="none" stroke="#E0AE63" stroke-width="2.6"/>',
    '<line x1="320" y1="150" x2="320" y2="184" stroke="#E0AE63" stroke-width="2"/>',
    '<text x="360" y="172" class="dl">2차 갑문과 수문</text>',
    // 3차 양수장
    '<circle cx="118" cy="286" r="26" fill="none" stroke="#E0AE63" stroke-width="2.6"/>',
    '<line x1="100" y1="268" x2="136" y2="304" stroke="#E0AE63" stroke-width="1.8"/>',
    '<line x1="136" y1="268" x2="100" y2="304" stroke="#E0AE63" stroke-width="1.8"/>',
    '<text x="60" y="332" class="dl">3차 제1양수장</text>',
    // 염선 (허브에서 4구역으로)
    '<g stroke="#6FB3A8" stroke-width="2" stroke-dasharray="7 6" fill="none">',
    '<path d="M320 244 L 320 190"/>',
    '<path d="M320 244 C 250 250, 170 264, 130 272"/>',
    '<path d="M320 244 C 380 252, 452 260, 506 258"/>',
    '<path d="M320 244 C 300 286, 260 316, 216 330"/>',
    '</g>',
    // 허브
    '<rect x="266" y="212" width="108" height="46" rx="6" fill="#12262B" stroke="#E9E4D8" stroke-width="2.4"/>',
    '<text x="320" y="240" class="dl dl-c dl-b">당직실 허브</text>',
    // 구역 노드
    '<rect x="272" y="164" width="96" height="0" fill="none"/>',
    '<rect x="470" y="238" width="112" height="40" rx="6" fill="none" stroke="#6FB3A8" stroke-width="2"/>',
    '<text x="526" y="263" class="dl dl-c">부두 냉동창고</text>',
    '<rect x="160" y="318" width="112" height="40" rx="6" fill="none" stroke="#6FB3A8" stroke-width="2"/>',
    '<text x="216" y="343" class="dl dl-c">저지대 주거지</text>',
    '<rect x="264" y="150" width="0" height="0" fill="none"/>',
    '<rect x="56" y="252" width="0" height="0" fill="none"/>',
    '<text x="386" y="204" class="dl dl-s">제3수문 구역</text>',
    '<text x="20" y="352" class="dl dl-s">염선: 압력과 염도가 신호다</text>',
    '</svg>',
  ].join('');
}

function svgLoop() {
  return [
    '<svg class="dia" viewBox="0 0 640 300" role="img" aria-label="미세 루프와 사건 루프의 두 겹 구조 도해">',
    '<circle cx="188" cy="150" r="104" fill="none" stroke="#3F7C77" stroke-width="2" stroke-dasharray="6 6"/>',
    '<circle cx="188" cy="150" r="62" fill="none" stroke="#6FB3A8" stroke-width="2.6"/>',
    '<text x="188" y="144" class="dl dl-c dl-b">미세 루프</text>',
    '<text x="188" y="166" class="dl dl-c dl-s">60 - 180초</text>',
    '<text x="188" y="34" class="dl dl-c">관측</text>',
    '<text x="308" y="156" class="dl dl-c">작은 조작</text>',
    '<text x="188" y="272" class="dl dl-c">결과</text>',
    '<text x="66" y="156" class="dl dl-c">되돌림</text>',
    '<path d="M348 150 L 396 150" stroke="#E0AE63" stroke-width="2.4"/>',
    '<path d="M388 143 L 400 150 L 388 157 Z" fill="#E0AE63"/>',
    '<rect x="410" y="60" width="214" height="180" rx="8" fill="none" stroke="#E9E4D8" stroke-width="2"/>',
    '<text x="517" y="92" class="dl dl-c dl-b">사건 루프</text>',
    '<text x="517" y="114" class="dl dl-c dl-s">8 - 20분 목표</text>',
    '<text x="517" y="146" class="dl dl-c">조사 · 자료 대조</text>',
    '<text x="517" y="174" class="dl dl-c">가설 시험 · 확인</text>',
    '<text x="517" y="202" class="dl dl-c">대화와 공간 변화</text>',
    '<text x="517" y="228" class="dl dl-c dl-s">반복은 콘텐츠 수로 세지 않는다</text>',
    '</svg>',
  ].join('');
}

function svgPriceBars() {
  const max = Math.max.apply(null, PRICES);
  const rows = PRICES.map((p, i) => {
    const y = 40 + i * 62;
    const w = (p / max) * 420;
    const paid = p * (1 - DISC);
    const pw = (paid / max) * 420;
    return [
      '<rect x="118" y="' + y + '" width="' + w.toFixed(1) + '" height="30" rx="3" fill="#12262B" stroke="#3F7C77" stroke-width="1.6"/>',
      '<rect x="118" y="' + y + '" width="' + pw.toFixed(1) + '" height="30" rx="3" fill="#3F7C77" opacity="0.55"/>',
      '<text x="110" y="' + (y + 21) + '" class="dl" text-anchor="end">B' + (i + 1) + '</text>',
      '<text x="' + (124 + w).toFixed(1) + '" y="' + (y + 21) + '" class="dl">' + won(p) + ' / 할인 후 ' + won(paid) + '</text>',
    ].join('');
  }).join('');
  return [
    '<svg class="dia" viewBox="0 0 640 240" role="img" aria-label="가격 후보 3안과 출시 할인 적용 결제가 막대 도해">',
    '<line x1="118" y1="24" x2="118" y2="226" stroke="#E9E4D8" stroke-width="1.4" opacity="0.6"/>',
    rows,
    '<text x="118" y="20" class="dl dl-s">막대 길이는 표시가, 진한 구간은 할인 ' + pct(DISC) + ' 적용 결제가</text>',
    '</svg>',
  ].join('');
}

function svgTimeline() {
  const bar = (y, x, w, color, label) =>
    '<rect x="' + x + '" y="' + y + '" width="' + w + '" height="26" rx="4" fill="none" stroke="' + color + '" stroke-width="2.4"/>' +
    '<text x="' + (x + 10) + '" y="' + (y + 18) + '" class="dl">' + label + '</text>';
  return [
    '<svg class="dia" viewBox="0 0 640 250" role="img" aria-label="등록비 결제 시점부터 출시 가능 시점까지의 대기 구간 도해">',
    '<line x1="24" y1="212" x2="616" y2="212" stroke="#E9E4D8" stroke-width="1.6"/>',
    '<text x="24" y="234" class="dl dl-s">결제일</text>',
    '<text x="616" y="234" class="dl dl-s" text-anchor="end">출시 가능 시점</text>',
    bar(28, 24, 300, '#E0AE63', '30일 초기 대기 (대체 불가)'),
    bar(70, 150, 200, '#6FB3A8', 'Coming Soon 2주 이상 (대체 불가)'),
    bar(112, 150, 150, '#6FB3A8', '스토어 페이지 리뷰 3 - 5영업일'),
    bar(154, 330, 170, '#6FB3A8', '빌드 리뷰, 버퍼 7영업일 고정'),
    '<line x1="500" y1="24" x2="500" y2="212" stroke="#E0AE63" stroke-width="1.6" stroke-dasharray="5 5"/>',
    '<text x="508" y="200" class="dl dl-s">사람이 출시 버튼을 직접 누른다</text>',
    '</svg>',
  ].join('');
}

/* ------------------------------------------------------------------ *
 * 5. 표 도우미
 * ------------------------------------------------------------------ */

function table(headers, rows, cls) {
  return (
    '<table class="tb ' + (cls || '') + '"><thead><tr>' +
    headers.map((h) => '<th>' + h + '</th>').join('') +
    '</tr></thead><tbody>' +
    rows
      .map((r) => '<tr>' + r.map((c) => '<td>' + c + '</td>').join('') + '</tr>')
      .join('') +
    '</tbody></table>'
  );
}

/* ------------------------------------------------------------------ *
 * 6. 출처
 * ------------------------------------------------------------------ */

const S = {
  pricing: ['Steam 가격 정책', 'https://partner.steamgames.com/doc/store/pricing'],
  minimums: ['KRW 최저 기준가 (로그인 필요)', 'https://partner.steamgames.com/pricing/minimums'],
  discounts: ['Steam 할인 규칙', 'https://partner.steamgames.com/doc/marketing/discounts'],
  payments: ['Steam 정산과 지급', 'https://partner.steamgames.com/doc/finance/payments_salesreporting'],
  taxfaq: ['Steam 세무 FAQ', 'https://partner.steamgames.com/doc/finance/taxfaq'],
  refunds: ['Steam 환불 정책', 'https://store.steampowered.com/steam_refunds/?l=korean'],
  appfee: ['Steam Direct 등록비', 'https://partner.steamgames.com/doc/gettingstarted/appfee'],
  onboarding: ['Steamworks 온보딩', 'https://partner.steamgames.com/doc/gettingstarted/onboarding'],
  direct: ['Steam Direct 안내', 'https://partner.steamgames.com/steamdirect'],
  survey: ['콘텐츠 설문과 생성형 AI 공개', 'https://partner.steamgames.com/doc/gettingstarted/contentsurvey'],
  review: ['스토어 리뷰 절차', 'https://partner.steamgames.com/doc/store/review_process'],
  coming: ['Coming Soon 페이지', 'https://partner.steamgames.com/doc/store/coming_soon'],
  dlc: ['Steam DLC 문서', 'https://partner.steamgames.com/doc/store/application/dlc'],
  demos: ['Steam 데모 문서', 'https://partner.steamgames.com/doc/store/application/demos'],
  upload: ['SteamPipe 업로드', 'https://partner.steamgames.com/doc/sdk/uploading'],
  trailer: ['Steam 트레일러 가이드', 'https://partner.steamgames.com/doc/store/trailer'],
  charts: ['Steam 매출 차트', 'https://store.steampowered.com/charts/topselling/global'],
  unity: ['Unity 요금제 비교', 'https://unity.com/products/compare-plans'],
  gcrb: ['게임콘텐츠등급분류위원회', 'https://www.gcrb.or.kr/Institution/EtcForm01.aspx'],
  grac: ['게임물관리위원회', 'https://www.grac.or.kr/'],
  law: ['게임산업법 원문', 'https://www.law.go.kr/LSW/lsInfoP.do?lsId=010196'],
  nts: ['국세청', 'https://www.nts.go.kr/'],
  video: ['참고 영상 표본', 'https://www.youtube.com/watch?v=CQFxvMg87Cs'],
};
const D = (p) => [p, null]; // 내부 문서 경로 (링크 아님)

/* ------------------------------------------------------------------ *
 * 7. 슬라이드 19: cycle-ledger.json(schemaVersion 2) 을 그대로 옮긴다.
 *    스키마가 2가 아니거나 파일이 없으면 표를 비운다. 손기입 값 0.
 * ------------------------------------------------------------------ */

function cycleSlideBody() {
  const HEAD = ['회차', '총', 'closed', '열린 S1', '열린 S2', '열린 S3+', 'open-rfc', '상태', '검토 파일'];
  const stampOf = (l) =>
    'production/cycle-ledger.json · generated_at ' +
    (l && l.generated_at ? l.generated_at : '미기록') +
    ' · source ' +
    (l && l.source ? l.source : '미기록') +
    ' · generator ' +
    (l && l.generator ? l.generator : '미기록');

  if (!ledger || !Array.isArray(ledger.cycles) || !ledger.cycles.length) {
    return {
      lines: [
        'production/cycle-ledger.json 이 없거나 cycles 항목이 비어 있다.',
        '회차별 발견과 수정을 파일로 확인할 수 없으므로 이 덱은 회차를 세지 않는다.',
        '다섯 회차가 완료되었다고 말하지 않는다. 완료 주장 0건.',
        '대장이 생기면 이 표는 파일 값으로 자동 대체된다. 손으로 채워 넣지 않는다.',
      ],
      fig: table(HEAD, [], 'small'),
      note: 'cycle-ledger.json 부재 또는 빈 cycles[]. 표를 비운 채로 둔다.',
      source: 'production/cycle-ledger.json (부재), production/premium-preproduction-contract.md',
    };
  }

  const sv = ledger.schemaVersion;
  if (sv !== 2) {
    const seen = sv === null || sv === undefined ? '미기록' : esc(String(sv));
    return {
      lines: [
        '대장의 schemaVersion 이 ' + seen + ' 이라 이 생성기의 기대(2)와 계약이 다르다.',
        '계약이 다른 파일에서 숫자를 읽어 옮기면 조용한 불일치가 된다. 그래서 옮기지 않는다.',
        '표는 비운다. 이전 빌드의 값을 되살려 채우지도 않는다.',
        'scripts/regen-cycle-ledger.py 로 대장을 재생성한 뒤 이 덱을 다시 빌드한다.',
      ],
      fig: table(HEAD, [], 'small'),
      note: 'schemaVersion 불일치(기대 2, 실제 ' + seen + '). 값 렌더를 중단했다. 스탬프: ' + stampOf(ledger) + '.',
      source: stampOf(ledger),
    };
  }

  const num = (v) => (typeof v === 'number' && Number.isFinite(v) ? esc(String(v)) : '미기록');
  const txt = (v) => (v === null || v === undefined || v === '' ? '미기록' : esc(String(v)));
  const rows = ledger.cycles.map((c) => [
    txt(c.id),
    num(c.total),
    num(c.closed),
    num(c.open_S1),
    num(c.open_S2),
    num(c.open_S3plus),
    num(c.open_rfc),
    txt(c.status),
    txt(c.review),
  ]);
  const sumS1 = ledger.cycles.reduce(
    (a, c) => a + (typeof c.open_S1 === 'number' ? c.open_S1 : 0),
    0,
  );
  return {
    lines: [
      'production/cycle-ledger.json (schemaVersion 2) 을 빌드 시점에 읽어 그대로 옮겼다.',
      '표의 아홉 칸은 전부 대장에서만 읽는다. 이 덱이 손으로 적은 숫자는 0개다.',
      '대장은 qa/defect-register.md 를 세어 만든 파생물이고 이 덱의 판정이 아니다.',
      '대장 기준 열린 S1 합계 ' + sumS1 + '건. 빈 값은 미기록으로 두고 채워 넣지 않는다.',
    ],
    fig: table(HEAD, rows, 'small'),
    note:
      'cycle-ledger.json 존재. 회차 ' +
      ledger.cycles.length +
      '건을 그대로 옮겼다. 스탬프: ' +
      stampOf(ledger) +
      '.',
    source: stampOf(ledger),
  };
}
const cyc = cycleSlideBody();

/* ------------------------------------------------------------------ *
 * 8. 36장 정의
 * ------------------------------------------------------------------ */

const slides = [];
const add = (o) => slides.push(o);

// 1
const COVER_LEAD = '기록을 복원하고, 항구의 물길을 바꾸는 밤.';
add({
  kicker: '내부 브리핑',
  cover: true,
  title: '조수기록국: 마지막 당직',
  lead: COVER_LEAD,
  stats: [
    { v: campaignMinutes + '분', k: '설계 분량' },
    { v: '본편 완결', k: '결말 3종을 본편에서 닫는다' },
    { v: 'Unity', k: '사전제작 단계' },
  ],
  caption: '가제 · 사람 플레이 0 · 가격 미승인',
  lines: [COVER_LEAD],
  notes: {
    detail:
      '표지는 제목과 한 문장과 세 수치만 남기고 면책 조항은 전부 이 노트와 2번 상태 슬라이드로 옮겼다. 전문: 이 덱은 내부 검토용이며 승인, 계약, 공개, 결제의 근거가 아니다. 게임 빌드 0건, 사람 플레이테스트 n=' +
      playtestN + ', Steam 등록 행위 0건이다. 가격 후보 ' + PRICES.length +
      '안은 전부 미승인이고 개발사 배분율은 가정이며 Steam이 공개한 수치가 아니다. 가제와 영어명 TIDE ARCHIVE는 상표와 동명 게임 확인 전까지 외부, 폴더명, 번들명에 쓰지 않는다. 이 덱은 회계 자문도 세무 자문도 아니다. 기준 사이클 ' +
      cam.cycle + ', 기준일 2026-09-09.',
    uncertainty:
      '표지의 세 수치는 설계 값과 단계 이름일 뿐 성과가 아니다. ' + campaignMinutes +
      '분은 설계 예산이고 관측 완주 시간이 아니며, 본편 완결과 Unity 사전제작도 문서상의 계약이지 실행된 결과가 아니다.',
    source:
      'planning/gdd.md, planning/campaign.json, production/premium-preproduction-contract.md',
  },
  srcs: [D('planning/gdd.md'), D('planning/campaign.json'), D('production/premium-preproduction-contract.md')],
});

// 2
add({
  kicker: '정직성',
  title: '지금 없는 것을 먼저 말한다',
  lines: [
    '게임 빌드 0건. 이 덱의 어떤 화면도 실제 플레이 캡처가 아니다.',
    '사람 플레이테스트 n=' + playtestN + '. 완주 시간, 난이도, 재미의 관측치 0건.',
    campaignMinutes + '분은 설계 예산이고 관측 완주 시간이 아니다.',
    'Steam 등록 행위 0건. 계정, 서명, 결제, 페이지, 빌드 모두 미실행.',
    '가격 후보 3안 전부 미승인. 배분율 70 대 30은 가정이며 확인된 사실이 아니다.',
    '성능, 로드, 저장 시간은 전부 NOT-MEASURED.',
  ],
  notes: {
    detail:
      '이 슬라이드가 나머지 34장의 해석 규칙이다. 뒤에 나오는 모든 수치는 목표이거나 가정이고, 관측이라고 표시한 것만 관측이다.',
    uncertainty:
      '문서가 아무리 정합해도 게임의 재미와 시장 수용은 여기서 아무것도 증명되지 않았다. 이 덱의 금액 계산은 감도 확인용이며 회계 자문도 세무 자문도 아니다. 100달러 등록비는 등록 시 반드시 선지출하는 현금 비용이고 AGR 조건을 충족해 회수되면 그때 차감되며, 원화 표에는 환율이 설정되어 있지 않아 들어 있지 않다.',
    source:
      'planning/campaign.json (humanPlaytests 배열 길이), product/steam-registration-guide.md, systems/unity-implementation.md',
  },
  srcs: [D('planning/campaign.json'), D('product/steam-registration-guide.md')],
});

// 3
add({
  kicker: '시장',
  title: '관측 범위와 그 한계',
  lines: [
    '2026-09-09 Steam 글로벌 매출순 차트와 공식 상점 설명을 직접 열어 관측했다.',
    '표본이 성공작에 치우쳐 성공 확률과 평균 매출을 추정할 수 없다.',
    '리뷰 수는 리뷰 수일 뿐 판매량이 아니다.',
    '태그 검색 결과 수와 상위 판매 결과 수의 비율은 노출 확률이 아니다.',
    '연도가 다른 가격 표본을 환산해 가격 하락 추세라고 단언하지 않는다.',
  ],
  notes: {
    detail:
      '차트에는 무료 게임, 할인, 기존 브랜드가 섞여 있다. 관측 예로 R.E.P.O. 71위, PEAK 85위, Slay the Spire 2 79위를 봤지만 장르 수요로 일반화하지 않는다.',
    uncertainty:
      '차트 순위는 시점 값이다. 같은 URL을 다시 열면 다른 결과가 나온다.',
    source: 'planning/market-decision.md, Steam 매출 차트',
  },
  srcs: [S.charts, D('planning/market-decision.md')],
});

// 4
add({
  kicker: '시장',
  title: '비교작 관측 가격 8종',
  lines: [
    '아래는 2026-09-09 한국 상점 페이지에서 읽은 현재가이고 정가표가 아니다.',
    '실제 클리어 시간, 튜토리얼 효능, 엔진은 확인하지 못했다.',
  ],
  fig: table(
    ['비교작', 'KR 현재가', '배울 점', '복제 금지'],
    [
      ['The Case of the Golden Idol', '20,500원', '독립 사건에서 전체 재해석으로', '빈칸 문장 양식과 고유 사건'],
      ['The Rise of the Golden Idol', '21,500원', '챕터형 콘텐츠 팩', '사건과 패스 약속 복제'],
      ['Strange Horticulture', '17,500원', '책상 도구가 세계를 설명', '식물과 손님 구조'],
      ['Return of the Obra Dinn', '21,500원', '표현 제약이 곧 정체성', '사망 재생과 60인 명부'],
      ['The Roottrees are Dead', '21,000원', 'UI 자체가 추리 행위', '가계도와 가짜 OS 스킨'],
      ['The Incident at Galley House', '21,000원', '기계로 흔적을 읽는 구성', '장치와 사건 구조'],
      ['Duck Detective', '11,000원', '짧아도 완결된 사건의 가치', '길면 비싸다는 추론'],
      ['No Case Should Remain Unsolved', '7,800원', '9,000원은 시장 법칙이 아니다', '저가 성공을 우리 예측으로 전용'],
    ],
    'small'
  ),
  notes: {
    detail:
      'Desktop Explorer는 정가 19,300원에 20% 할인 15,440원으로 표기되어 있었다. 즉 표에 적힌 값 중 일부는 할인 중 가격일 수 있다.',
    uncertainty:
      '정가와 상시 할인폭 표는 여전히 미측정이다. 이 표로 우리 가격을 정당화하지 않는다.',
    source: 'planning/market-decision.md, 각 앱 store.steampowered.com 페이지 (2026-09-09)',
  },
  srcs: [D('planning/market-decision.md'), S.charts],
});

// 5
add({
  kicker: '시장',
  title: '세 후보 중 작업대 추리를 골랐다',
  lines: [
    '협동 물리 원정 3.05, 전술 덱빌딩 3.35, 작업대 공간 추리 4.10.',
    '이 점수는 디렉터의 주관 평가이고 승률이나 수요 모델이 아니다.',
    '가중치는 제작가능성 30, 상호작용 25, 서사 20, 확장 15, 시장신호 10.',
    '협동 물리는 네트워크와 동기화 검증 부담으로 첫 작품에서 보류했다.',
    '덱빌딩은 조합 밸런스와 반복 피로, 수요 증거 공백으로 보류했다.',
    '선택은 심화 대상 결정이지 성공 확정이 아니다.',
  ],
  notes: {
    detail:
      '제작 인력이 미확정이고 온라인 서비스를 운영하지 않는다는 가정에서 나온 가중치다. 팀과 자금이 달라지면 가중치를 다시 고른다.',
    uncertainty: '점수 자체가 관측이 아니라 판단이다. 재현 가능한 지표가 아니다.',
    source: 'planning/market-decision.md 의 C1 QA 반영 절',
  },
  srcs: [D('planning/market-decision.md')],
});

// 6
add({
  kicker: '콘셉트',
  title: '은포항, 한 허브와 네 구역',
  lines: [
    '가상 도시 은포항. 대조차 9.2 m, 하루 두 번 갯벌이 드러난다.',
    '3중 방어는 외곽 방조제, 갑문과 수문, 양수장 순서다.',
    '신호는 전자 로그가 아니라 염선의 압력과 염도로 흐른다.',
    '새 대륙 8개 대신 허브 1곳과 구역 4곳을 상태 변화로 재사용한다.',
    '게임은 폐국 전 마지막 밤 21시부터 5시까지를 다룬다.',
  ],
  fig: svgHarbor(),
  notes: {
    detail:
      '도해는 이 파일에서 직접 그린 원본 SVG이고 외부 이미지나 타 게임 자산을 쓰지 않았다. 구역 재사용은 에셋 비용 상한을 셀 수 있게 만드는 장치다.',
    uncertainty:
      '기술 설정은 창작이며 현실의 수문 안전 매뉴얼이 아니다. 실제 씬 노드 개수는 미정이다.',
    source: 'worldview/worldview-bible.md 1절, planning/gdd.md',
  },
  srcs: [D('worldview/worldview-bible.md'), D('planning/gdd.md')],
});

// 7
add({
  kicker: '콘셉트',
  title: '루프는 두 겹이다',
  lines: [
    '미세 루프는 관측, 작은 조작, 결과, 되돌림으로 60초에서 180초.',
    '사건 루프는 조사, 자료 대조, 가설 시험, 확인, 공간 변화로 8분에서 20분 목표.',
    '미세 루프 반복은 고유 콘텐츠 수로 세지 않는다.',
    '총시간은 탐색, 추론, 조작, 대화, 결과의 중복 없는 합으로 모델링한다.',
    '무작위 드롭과 반복 채집은 없다.',
  ],
  fig: svgLoop(),
  notes: {
    detail:
      '두 겹으로 나눈 이유는 분량 착시를 막기 위해서다. 걷기와 대기를 늘려 8시간을 채우지 않겠다는 계약이 여기에 걸린다.',
    uncertainty:
      '60에서 180초, 8에서 20분은 전부 목표다. 실제 소요 시간은 측정된 적이 없다.',
    source: 'planning/gdd.md 코어 루프 절',
  },
  srcs: [D('planning/gdd.md')],
});

// 8
add({
  kicker: '세계',
  title: '불가침 6법이 곧 실패 계약이다',
  lines: [
    '법마다 규칙, 플레이어 행동, 실패와 회복을 함께 정의했다.',
    '어떤 실패도 필수 단서, 진행, 접근 권한을 빼앗지 않는다.',
    '아래 표는 세계관 바이블 3절 정본 표를 빌드 시점에 읽어 그대로 옮긴 것이다.',
  ],
  fig: table(
    ['법', '규칙', '실패와 회복'],
    LAWS.map((l) => [l.name, l.rule, l.recovery]),
    'small'
  ),
  notes: {
    detail:
      '법 4의 재산 보호는 이진 플래그 1개이며 후일담 텍스트 2쌍만 바꾼다. 4구역 곱하기 3엔딩의 별도 씬을 만들지 않는다. 법 5의 부식예산은 경로 구성안 총비용에 걸리는 전역 상한이고 확정으로 소모되지 않는다. 표에서 뺀 열은 대응 플레이어 행동 하나이며 바이블 3절에 남아 있다.',
    uncertainty:
      '규칙이 재미를 보장하지 않는다. 좌절 지점과 난이도는 플레이 측정 전까지 알 수 없다.',
    source:
      'worldview/worldview-bible.md 3절 정본 표 (RFC-P3-014), systems/interaction-rules.md 0절',
  },
  srcs: [
    D('worldview/worldview-bible.md'),
    D('systems/interaction-rules.md'),
    D('production/decision-log.md'),
  ],
});

// 9
add({
  kicker: '세계',
  title: '다섯 사람, 아무도 전체를 모른다',
  lines: [
    '전지적 화자와 해설 NPC를 두지 않는다. 각자 진실의 일부만 안다.',
  ],
  fig: table(
    ['인물', '원하는 것', '모순'],
    [
      ['한서린 (기록복원사)', '폐국 전에 결손 4시간 복원', '무결성을 말하면서 미봉인 판 #0을 숨겼다'],
      ['문재화 (수문 계장)', '사고 없이 자동화 이관 완료', '안전을 위해 기록을 정리한다'],
      ['오은정 (주민회 총무)', '공식 인정과 이주 보상', '유리한 사본만 제공하고 일지 두 장을 태웠다'],
      ['표성찬 (냉동창고 운영주)', '이관 후에도 하역권 유지', '먼저 협조하지만 자료가 시간축을 흐린다'],
      ['한도연 (당시 당직 주임)', '딸이 이 일을 그만두기', '그만두라면서 열쇠와 좌표를 흘린다'],
    ],
    'small'
  ),
  notes: {
    detail:
      '도연은 편리한 전지적 증인이 아니다. 기억은 검증 대상이고 혼자서 사실을 확정하지 못하며 제출을 막는 열쇠 NPC도 아니다.',
    uncertainty:
      '인물 매력과 대사 품질은 문서로 증명되지 않는다. 성우와 연기는 계획에 없다.',
    source: 'worldview/worldview-bible.md 4절과 7절',
  },
  srcs: [D('worldview/worldview-bible.md')],
});

// 10
add({
  kicker: '구조',
  title: stages.length + '개 장, 설계 ' + campaignMinutes + '분',
  lines: [
    '아래 표는 planning/campaign.json 을 빌드 시점에 읽어 만든 것이다.',
    '필수 비트 ' + beatCount + '개, 종류별 분은 퍼즐 ' + (kindMinutes.puzzle || 0) + ', 대화 ' +
      (kindMinutes.dialogue || 0) + ', 보상 ' + (kindMinutes.payoff || 0) + ', 탐색 ' + (kindMinutes.exploration || 0) + '.',
    '합계 ' + campaignMinutes + '분은 설계 예산이며 완주 관측치가 아니다.',
  ],
  fig: table(
    ['장', '제목', '분', '구역', '비트'],
    stages.map((s) => [s.id, esc(s.title), String(s.minutes), esc(s.zones), String(s.beats)]),
    'small'
  ),
  notes: {
    detail:
      '장별 분은 비트 분의 합과 정확히 일치한다. 비트마다 빠른 플레이와 신중한 플레이의 분이 따로 적혀 있어 뒤에서 밴드로 환산할 수 있다.',
    uncertainty:
      '단순 합계는 플레이 검증이 아니다. 사람이 실제로 이 순서를 이 시간에 통과하는지는 미측정이다.',
    source: 'planning/campaign.json, planning/gdd.md 초기 8시간 예산 절',
  },
  srcs: [D('planning/campaign.json'), D('planning/gdd.md')],
});

// 11
add({
  kicker: '구조',
  title: '튜토리얼은 규칙을 손으로 배우게 한다',
  lines: [
    'T0 마지막 당직 인수, ' + stages[0].minutes + '분, ' + sliceZoneText + '에서만 진행한다.',
    '오늘 밤 이관될 매체 3종의 위치와 취급 규칙을 직접 확인하는 것이 목표다.',
    '별도 강의 화면이나 강제 대사 없이 첫 판독에서 사본 보존을 체감시킨다.',
    '도구는 ' + (firstStage.tools.join(', ') || '없음') + ' ' + firstStage.tools.length +
      '종만 열어 학습 부하를 줄인다.',
    '실패해도 자원과 진행을 잃지 않는다는 것을 튜토리얼 안에서 보여준다.',
  ],
  notes: {
    detail:
      'T0는 동시에 수직 슬라이스의 기준 구간이다. 이 ' + firstStage.minutes +
      '분이 실제로 열리고 측정되어야 나머지 ' + restMinutes +
      '분의 생산 판단이 시작된다. 두 수치 모두 planning/campaign.json 의 단계 예산에서 빌드 시점에 계산된다.',
    uncertainty:
      '튜토리얼 효능은 비교작에서도 확인하지 못했다. 우리 것도 당연히 미측정이다.',
    source: 'planning/campaign.json T0 단계, systems/unity-implementation.md 10절',
  },
  srcs: [D('planning/campaign.json'), D('systems/unity-implementation.md')],
});

// 12
add({
  kicker: '시스템',
  title: '도구 ' + allTools.length + '종은 기능 수다',
  lines: [
    '캠페인 데이터에서 실제로 쓰이는 도구는 ' + allTools.length +
      '종이며, 이것은 기능의 수이지 UI 프레임의 수가 아니다.',
    '공용 작업대 셸 하나에 도구 패널 여섯 종이 들어간다.',
    '도구별 등장 비트는 ' +
      allTools.map((t) => t + ' ' + (toolBeatCounts[t] || 0)).join(', ') + ' 이다.',
    '도구를 쓰지 않는 비트 ' + toollessIds.length + '개(' + toollessIds.join(', ') +
      ')는 퍼즐이 아닌 탐색과 대화와 결과 장면이다.',
  ],
  fig: table(
    ['도구', '입력', '확정 조건'],
    [
      ['circuit 회로 지도', '계통 선을 따라 센서 노드 선택', '판독 전용, 범위 밖 근거를 자동 무효화'],
      ['reader 판독기', '매체를 올리고 시간 범위 드래그', '가설판에 인용 고정할 때 출처 기록'],
      ['alignment 조위정합', '공통 피크 3개 지정 후 오프셋 조절', '잔차 절댓값 4분 이하일 때만 활성'],
      ['routing 경로 구성', '밸브와 수문 경로를 노드로 연결', '위반 0건에 부식 한도 내, 프리뷰 확인'],
      ['corrosion 부식예산', '구성안을 가상 시험대에서 무제한 시험', '전역 한도 이내에서만 routing 확정이 열리고 확정으로 소모되지 않는다'],
      ['seal 이중서명', '근거 슬롯 2개와 검증자 확인', '서로 다른 매체 2종일 때만 서명 활성'],
    ],
    'small'
  ),
  notes: {
    detail:
      '확정 기본값은 프리뷰 뒤 한 번 누르는 2단계 확정이고, 길게 누름 0.4초는 접근성 설정에서 켜는 선택지다. 되돌림은 확정 전 무제한이다. 부식예산은 경로 구성안 총비용에 걸리는 전역 상한이며 판독과 회로와 봉인 확정은 부식을 차감하지 않는다. 모든 바인딩은 재매핑 가능하고 마지막 사용 장치에 따라 글리프가 바뀐다.',
    uncertainty:
      '조작감, 반응 시간, 난이도는 전부 미측정이다. 즉시나 부드럽게 같은 표현은 목표이지 관측이 아니다.',
    source:
      'systems/interaction-rules.md 1-1절과 2절, production/decision-log.md RFC-P3-015와 RFC-P3-009',
  },
  srcs: [D('systems/interaction-rules.md'), D('production/decision-log.md')],
});

// 13
add({
  kicker: '서사',
  title: '반증으로 밀고 나가는 공개 설계',
  lines: [
    '결손 4분은 재화의 근무일에만 반복된다. 단일 자료로는 확정되지 않는다.',
    '판 #0은 주인공이 숨긴 미봉인 염판이고 각인한 사람은 아직 모른다.',
    '조위정합 전 관측소 간 오차는 40분, 정합 후 잔차는 4분이다.',
    '두 사건 간격이 오차폭 합 8분보다 클 때만 선후를 확정한다.',
    '겹치면 판정은 미확정으로 남고 그 상태로도 진행은 막히지 않는다.',
    '과거는 재생되지 않고 추론된다. 유령, 예지, 시간 이동은 이 세계에 없다.',
  ],
  notes: {
    detail:
      '기록의 물리적 한계가 곧 퍼즐 규칙이다. 남는 것은 밸브 개폐, 압력, 염도, 수위, 문 개폐, 당직 호출이고 얼굴과 의도와 대화 내용은 남지 않는다.',
    uncertainty:
      '추리 난이도가 적정한지는 알 수 없다. 힌트 3단계는 플레이어가 순서대로 여는 무료 장치이고 자동 승격은 없다.',
    source: 'worldview/worldview-bible.md 2절, systems/interaction-rules.md 2.3절',
  },
  srcs: [D('worldview/worldview-bible.md'), D('systems/interaction-rules.md')],
});

// 14
add({
  kicker: '서사',
  title: '결말 세 갈래는 본편 안에서 닫힌다',
  lines: [
    '완전 복원 제출, 계통 결함 중심 제출, 불완전 인정 제출.',
    '선악이 아니라 무엇을 잃는가의 선택이다.',
    '세 갈래 모두 에필로그에서 청문 결과와 4구역 최종 상태, 판 #0의 처리를 명시한다.',
    '최종 제출 화면에서 관점을 바꿔 같은 회차 안에서 재확정할 수 있다.',
    '미해결 떡밥을 DLC 판매 근거로 남기지 않는다.',
  ],
  notes: {
    detail:
      '공통 종결 씬 1개에 관점별 기록 패널 3개를 더하는 구조라 씬 수가 폭발하지 않는다. 크레딧 후 마지막 체크포인트로 돌아가 다른 관점을 확정할 수 있다.',
    uncertainty:
      '세 결말의 정서적 무게가 실제로 다른지는 플레이로만 확인된다.',
    source: 'worldview/worldview-bible.md 6절, systems/interaction-rules.md 7절',
  },
  srcs: [D('worldview/worldview-bible.md'), D('systems/interaction-rules.md')],
});

// 15
add({
  kicker: 'UI',
  title: '화면 계약 ' + nOrX(uiScreens) + '종은 계획이지 증거가 아니다',
  lines: [
    'screens ' + nOrX(uiScreens) + ', player_decisions ' + nOrX(uiDecisions) +
      ', data_bindings ' + nOrX(uiBindings) + '를 계약 파일에 정의했다.',
    '검증 매트릭스 ' + nOrX(uiMatrixRows) +
      '행은 실행 계획이고 verification.evidence 는 증거가 아니라 증거 없음 진술이다.',
    '색, 소리, 움직임 단독으로 결정적 상태를 전달하지 않고 항상 문자 라벨을 붙인다.',
    '확정 버튼은 검증 통과 전까지 비활성이고 비활성 이유를 문장으로 보여준다.',
    '제안 해상도군 1280x720, 1920x1080, 3440x1440, 1280x800은 실기 검증 0건.',
  ],
  notes: {
    detail:
      '스키마는 계정 스킬 game-ui-ux 의 검증기를 통과했고 두 번째 실행에서 error 0건이었다. 그 통과는 스키마 적합성이지 사용성의 증거가 아니다.',
    uncertainty:
      '실사용 접근성 검증, 패드 조작감, 실제 초점 순서는 전부 NOT-MEASURED이다.',
    source: 'systems/game-ui-contract.meta.md, systems/game-ui-contract.json',
  },
  srcs: [D('systems/game-ui-contract.meta.md')],
});

// 16
add({
  kicker: '아트',
  title: '2.5D 디오라마와 읽히는 색',
  lines: [
    '고정 관찰점의 항구 디오라마, 인물은 2D 초상. 걷는 주인공과 립싱크를 만들지 않는다.',
    '종이는 따뜻한 회백, 금속은 청회, 변경 예정은 황토, 확정은 짙은 잉크.',
    '색과 함께 아이콘, 문자, 선형을 쓴다. 색만으로 상태를 말하지 않는다.',
    '바다는 붉은 경고등이 아니라 수평 수위선으로 읽힌다.',
    '승인 산출물은 키샷 2장, 도구 실루엣 시트, 초상 시트이며 현재 확보 0건.',
  ],
  notes: {
    detail:
      '인물마다 표정 3종을 공유 변형으로 만든다. 선악을 외모로 부호화하지 않고 영어 간판을 텍스처에 굽지 않는다.',
    uncertainty:
      '최종 아트와 권리 확보는 0건이다. 타 게임 스크린샷은 참고자료일 뿐 자산으로 복제하지 않는다.',
    source: 'concept/art-direction.md',
  },
  srcs: [D('concept/art-direction.md')],
});

// 17
add({
  kicker: '기술',
  title: 'Unity 계약은 있고 빌드는 없다',
  lines: [
    '에디터 6000.5.6f1 설치를 이 머신에서 확인했다. LTS라고 주장하지 않는다.',
    '패키지 버전은 하나도 적지 않았다. resolve 후 lock 파일 값을 그대로 옮긴다.',
    'Tide.Domain 은 UnityEngine 을 참조하지 않는다. sim 과 render 를 분리한다.',
    '퍼즐 상태는 불변 레코드이고 되돌림은 파괴가 아니라 포인터 이동이다.',
    '기술 인수 테스트 ' + ACCEPT_TESTS + '개를 상태 술어로 정의했고 실행은 0건이다.',
    '프레임, 로드, 저장 시간과 기준 하드웨어는 전부 NOT-MEASURED.',
  ],
  notes: {
    detail:
      '임포트 검증은 fail-closed 다. 필수 확정마다 sourceType 상이 AND originId 상이 자료쌍 1개 이상, 그중 1개는 파괴 불가, 엔딩 3종 도달, 고아 노드 0건, KO와 EN 로컬라이제이션 키 존재를 위반하면 임포트가 실패한다.',
    uncertainty:
      'Unity Personal 무료 조건은 최근 12개월 매출과 펀딩 20만 달러 미만이며 초과 시 Pro 요금이 발생한다. 우리 상황은 아직 판정 대상이 아니다.',
    source: 'systems/unity-implementation.md, Unity 요금제 비교',
  },
  srcs: [D('systems/unity-implementation.md'), S.unity],
});

// 18
add({
  kicker: '검증',
  title: '시간은 이렇게 잰다',
  lines: [
    '설계 분량 합계와 관측 완주 중앙값은 서로 다른 키로 관리한다.',
    '판정 키는 ' + TIME.key + ' 이고 자리비움 구간은 회고로 판별한다.',
    '목표 밴드는 완주 중앙값 ' + TIME_BAND_TEXT + '이다. 무입력 자동 제외 규칙은 두지 않는다.',
    '중앙값이 ' + TIME.withdrawMedian + '분에 못 미치거나 하위 25%가 ' + TIME.withdrawP25 +
      '분에 못 미치면 8시간 주장을 철회한다.',
    '표본 최소 12명 5유형, 공략 없이 처음 플레이한 사람, 탈락 포함 보고.',
    '빠른 ' + fastSum + '분과 신중한 ' + deliberateSum +
      '분은 시나리오 경계이고 표본 통계와 비교하지 않는다.',
  ],
  notes: {
    detail:
      TIME.withdrawMedian + '분과 ' + TIME.withdrawP25 + '분은 통과선이 아니라 철회 트리거다. 목표를 낮춰 자동 통과시키지 않는다는 조항이 계약에 명시되어 있고, 상위 25퍼센트 600분 이하 조건은 시나리오 경계와 표본 통계를 섞는 범주 오류라 삭제했다. 선택 콘텐츠, 회차 반복, 수집 100퍼센트, 로딩, 일시정지, 자리비움, 재시작 대기는 본편 목표에 합산하지 않는다. 두 경계값은 campaign.json 의 비트별 값에서 빌드 시점에 합산한다.',
    uncertainty:
      '현재 표본 n=' + playtestN + '. 위 조건 중 어느 것도 시험된 적이 없다.',
    source:
      'production/premium-preproduction-contract.md 의 Time acceptance 절, production/decision-log.md RFC-P3-011',
  },
  srcs: [
    D('production/premium-preproduction-contract.md'),
    D('production/decision-log.md'),
  ],
});

// 19
add({
  kicker: '공정',
  title: '개선 사이클 상태',
  lines: cyc.lines,
  fig: cyc.fig,
  notes: {
    detail: cyc.note + ' 이 슬라이드는 빌드 시점에 파일 존재 여부로 갈린다.',
    uncertainty:
      '회차가 문서로 남았다는 것과 그 회차가 게임을 개선했다는 것은 다른 주장이다. 후자는 플레이 측정 뒤에만 말할 수 있다.',
    source: cyc.source + ' · production/premium-preproduction-contract.md',
  },
  srcs: [D('production/cycle-ledger.json'), D('production/premium-preproduction-contract.md')],
});

// 20
add({
  kicker: '상품',
  title: '무엇에 값을 매기는가',
  lines: [
    '1회 구매로 본편 서사, 도구 전부, 결말 3종 접근권이 열린다.',
    '시간, 회차, 힌트, 저장은 과금 대상이 아니다.',
    '가치 약속은 셋이다. 손으로 바꾼 결과가 같은 공간을 다르게 만든다.',
    '본편만으로 사건과 책임과 인물 관계가 닫힌다.',
    '힌트와 접근성은 무료이고 되돌림은 무제한이다.',
    '8시간이라는 분량은 가치 서술이지 가격 근거가 아니다.',
  ],
  notes: {
    detail:
      '공식 문서 어디에도 플레이타임 대비 가격 기준이 없다. 8시간에서 적정가를 도출하지 않는다는 것이 이 프로젝트의 명시 금지 항목이다.',
    uncertainty:
      '지불 의사와 전환은 관측 0건이다. 가치 약속이 시장에서 통하는지 알 수 없다.',
    source: 'product/business-model.md 1절',
  },
  srcs: [D('product/business-model.md'), S.pricing],
});

// 21
add({
  kicker: '가격',
  title: '후보 세 개, 전부 미승인',
  lines: [
    '표시가는 부가세 10% 포함이고 부가세 제외 총매출은 표시가를 1.1로 나눈 값이다.',
    '사용자 조건인 9,000원은 정가가 아니라 실제 결제 금액의 하한이다.',
    'B1을 고르면 정책 상한 40%보다 내부 상한 ' + capB1 + '%가 먼저 걸린다.',
    '출시 후 30일은 할인이 금지되고 유일한 예외는 출시 전에 설정한 런치 할인이다.',
    'KRW 최저 기준가의 정확한 숫자는 파트너 로그인이 필요해 미확인이다.',
  ],
  fig: svgPriceBars(),
  notes: {
    detail:
      '런치 할인은 7일에서 14일, 최대 40%이고 공식 권장은 10에서 15%다. 일반 할인은 10%에서 95%, 1일에서 14일, 할인 간 30일 간격이 필요하다. 95%는 9,000원선 위반이라 내부적으로 쓸 수 없다.',
    uncertainty:
      '세 후보 중 어느 것도 고를 근거가 아직 없다. 선택은 지불 의사 응답이 실제로 모인 뒤 디렉터가 닫는다.',
    source: 'product/economics.json, Steam 가격 정책, Steam 할인 규칙',
  },
  srcs: [S.pricing, S.discounts, S.minimums, D('product/economics.json')],
});

// 22
add({
  kicker: '가격',
  title: '단가 시뮬레이터',
  lines: [
    '입력을 바꾸면 본당 수취액과 손익분기 판매량이 즉시 다시 계산된다.',
    '기본값은 ' + won(DEFAULT_PRICE) + ' 표시가에 출시 할인 ' + pct(DISC) + '.',
    '배분 ' + pct(SHARE) + '는 가정이며 Steam이 공개한 사실이 아니다.',
    '계산은 economics.json 의 산식을 전체 정밀도로 쓰고 표시할 때만 반올림한다.',
    '손익분기 판매량은 항상 올림한다. 소수 본은 팔 수 없다.',
    '100달러 등록비와 환율, 적용 세금, 은행 비용은 이 계산에 들어 있지 않다.',
  ],
  fig: simulatorHtml(),
  notes: {
    detail:
      '산식은 표시가에서 할인을 빼고 부가세를 제한 뒤 지역계수를 곱하고 환불과 차지백을 뺀 다음 배분율을 곱하고 본당 준비금을 빼는 순서다. 준비금 ' +
      won(RESERVE) + '은 근거 없는 자리표시자다. 100달러 등록비는 등록 시 반드시 선지출하는 현금 비용이고 AGR 조건을 충족해 회수되면 그때 차감되는데, 이 시뮬레이터는 원화 환율을 설정하지 않았으므로 그 100달러와 적용 세금, 은행 비용을 별도로 더해서 보아야 한다.',
    uncertainty:
      '지역계수, 환불률, 배분율, 원천징수는 전부 미측정이다. 이 화면은 예측이 아니라 감도 확인 도구이며 회계 자문도 세무 자문도 아니다.',
    source: 'product/economics.json 의 unitFormula, Steam 정산 문서',
  },
  srcs: [D('product/economics.json'), S.payments, S.taxfaq],
});

// 23
add({
  kicker: '가격',
  title: '현금 회수는 흑자가 아니다',
  lines: [
    '아래 표의 기준은 표시가(부가세 10% 포함) · 할인 ' + pct(DISC) + ' 적용 · 배분 ' + pct(SHARE) +
      ' 가정 · 환불 ' + pct(REFUND) + ' 가정이다.',
    'product/business-model.md 5절 표는 할인 0% 정가 기준이라 같은 이름의 값이 다르다. 라벨을 보고 읽는다.',
    '현금 예산 ' + won(eco.cashBudgets[0]) + ' 회수에 필요한 본수와 인건비 기회비용 ' +
      won(laborCost) + ' 회수에 필요한 본수를 나란히 둔다.',
    '제작자 인건비를 무료로 놓으면 수익성이 부풀려진다.',
    '이 원화 표에는 환율이 설정되어 있지 않아 100달러 등록비가 들어 있지 않다.',
    '별도로 100달러와 적용 세금, 은행 비용을 더해서 보아야 한다. 회계 자문이 아니다.',
  ],
  fig: table(
    ['후보 · 표시가', '본당 수취 (k=' + KS[0] + ', 할인 ' + pct(DISC) + ')', '본당 수취 (k=' + KS[1] + ', 할인 ' + pct(DISC) + ')', '현금 ' + num(eco.cashBudgets[0] / 10000) + '만원 회수', '인건비 ' + num(Math.round(laborCost / 10000)) + '만원 회수'],
    unitTable.map((r, i) => [
      'B' + (i + 1) + ' ' + won(r.price),
      won(r.nets[0]),
      won(r.nets[1]),
      num(breakeven(eco.cashBudgets[0], r.nets[0])) + '본',
      num(breakeven(laborCost, r.nets[0])) + '본',
    ]),
    'small'
  ),
  notes: {
    detail:
      '이 표는 할인 ' + pct(DISC) + ' 기준이고 product/business-model.md 5절 표는 할인 0% 정가 기준이다. 두 표를 섞어 읽으면 같은 이름의 본당 수취가 다른 값으로 보인다. 100달러 등록비는 등록 시 반드시 선지출하는 현금 비용이며 AGR 조건을 충족해 회수되면 그때 차감된다. 위 원화 표에는 환율이 설정되어 있지 않아 들어 있지 않으므로, 별도로 100달러와 적용 세금, 은행 비용을 더해서 보아야 한다. 지급 임계는 월 100달러이고 지급은 판매월 더하기 30일이므로 손익분기와 현금 도달 시점은 다른 문제다. 이 덱은 회계 자문도 세무 자문도 아니다.',
    uncertainty:
      '이 표는 판매 예측이 아니라 역산이다. 그만큼 팔린다는 근거는 어디에도 없다. 배분율이 바뀌면 표 전체가 선형으로 흔들린다.',
    source: 'product/economics.json, production/production-estimate.json, Steam 정산 문서',
  },
  srcs: [D('product/economics.json'), D('production/production-estimate.json'), S.payments],
});

// 24
add({
  kicker: '상품',
  title: 'DLC 기록정지는 독립 사건이다',
  lines: [
    '이관 2년 후, 자동 기록되는 항구에서 주민들이 기록 정지를 청원한다.',
    '새 주인공, 새 구역, 새 딜레마. 본편의 어느 결말에서 시작해도 성립한다.',
    '분량과 가격 후보는 2시간에서 3시간, 5,900원에서 7,900원이고 미승인이다.',
    '본편 잔여 결말을 파는 것을 금지한다. DLC는 본편 결말을 바꾸지 않는다.',
    '출시일 동시 판매를 하지 않는다. 공식 문서가 출시일 DLC를 권장하지 않는다.',
    '힌트, 접근성, 저장, 되돌림, 버그 수정은 영원히 무료다.',
  ],
  notes: {
    detail:
      '증분 콘텐츠는 최대 3개로 못박았다. 새 기록 매체와 새 조작 동사, 새 구역과 인물 세트, 자체 결말 분기다. 본편 도구의 재사용으로 채우지 않는다.',
    uncertainty:
      'DLC 수용 전환율은 본편 출시 후에만 측정할 수 있다. 현재 n=0이고 본편 시급 환산으로 가격을 정당화하지 않는다.',
    source: 'product/business-model.md 7절, Steam DLC 문서',
  },
  srcs: [S.dlc, D('product/business-model.md')],
});

// 25
add({
  kicker: '생산',
  title: '견적 ' + num(baseDays) + '인일에 위험여유 ' + pct(est.contingencyRate),
  lines: [
    '단계 합 ' + num(rowDays) + '인일에 조정 ' + num(est.coordinationDays) + '과 출시행정 ' +
      num(est.launchAdminDays) + '을 더해 ' + num(baseDays) + '인일이다.',
    '위험여유를 반영하면 ' + num(totalDays) + '인일이고 1인일은 6시간 기준이다.',
    '1인일 ' + won(est.blendedLaborCostPerDay) + '의 기회비용 가정이면 인건비는 ' + won(laborCost) + '이다.',
    '이 수치는 예시 기회비용이지 시장 견적이나 외주 단가가 아니다.',
    '아트 ' + num(artDays) + '인일은 에셋 예산 문서와 일치한다. 외주와 현지화와 마케팅은 별도다.',
    '1명이 월 20작업일을 쓴다고 가정하면 약 ' + monthsSolo + '개월, 2명이라도 정확히 반이 되지 않는다.',
  ],
  notes: {
    detail:
      '범위 조정 규칙이 있다. 슬라이스 실측이 예상보다 50%를 넘으면 전체 생산을 멈추고 도구를 6개에서 4개로 줄이거나 장 구조를 재설계한다.',
    uncertainty:
      '이 견적은 기획 산정이고 납기 약속이 아니다. 인력이 미확정이라 어떤 출시일도 약속하지 않는다.',
    source: 'production/production-estimate.json, production/production-estimate.meta.md',
  },
  srcs: [D('production/production-estimate.json'), D('production/production-estimate.meta.md')],
});

// 26
add({
  kicker: '생산',
  title: '전체 생산 앞에 슬라이스가 있다',
  lines: [
    '먼저 20분에서 30분짜리 수직 슬라이스 ' + firstStage.id + '만 만든다. 설계상 ' +
      firstStage.minutes + '분 구간이다.',
    '포함은 ' + sliceZoneText + ', 도구 ' + sliceToolText + ', 결론 ' +
      sliceProofBeats + '건, 힌트 ' + sliceHintTiers + '단계.',
    '저장, 로드, 손상 복구, 한국어와 영어, 키보드와 패드까지 포함한다.',
    '제외는 나머지 ' + otherZoneCount + '구역과 나머지 도구의 전체 깊이, 엔딩 본편 분량, ' + campaignMinutes + '분 콘텐츠.',
    '전체 콘텐츠 생산은 사람 실측 12명 5유형 이후에만 게이트를 넘는다.',
    '검증 전 유료 에셋, 외주, 장기 일정 승인은 0건이다.',
  ],
  notes: {
    detail:
      '슬라이스가 이 프로젝트의 첫 실제 증거다. 지금까지의 모든 문서는 슬라이스가 열리기 전까지 가설로 남는다.',
    uncertainty:
      '슬라이스 소요 시간 자체가 미정이다. 견적의 오차가 처음 드러나는 지점도 여기다.',
    source: 'systems/unity-implementation.md 10절, production/production-estimate.meta.md',
  },
  srcs: [D('systems/unity-implementation.md'), D('production/production-estimate.meta.md')],
});

// 27
add({
  kicker: '등록',
  title: '계정과 결제는 사람에게 귀속된다',
  lines: [
    '지금까지 수행된 등록 행위는 0건이다. 아래는 순서이지 실행 기록이 아니다.',
    '등록비를 결제한 개인 계정에 app credit 이 귀속되고 그 사람만 활성화할 수 있다.',
    '결제자와 실제 운영자를 다르게 두면 안 된다.',
    'Steam 지갑 잔액으로는 결제할 수 없고 지역에서 지원되는 다른 결제수단이 필요하다.',
    '세금계산서는 결제한 개인 계정 명의로 발행되고 이중 인보이스는 없다.',
    '2단계 인증 필수 여부는 공식 문서에서 문구를 찾지 못했다. 미확인으로 둔다.',
  ],
  notes: {
    detail:
      '2026-09-09에 Steam Direct 안내와 온보딩 문서 본문을 전문 조회했으나 authenticator 나 two-factor 문구가 없었다. 필수다 또는 아니다를 어느 쪽으로도 단정하지 않는다. 운영 권고로는 켜 두는 편이 안전하다.',
    uncertainty:
      '계정 생성과 연동, 결제는 전부 사용자 승인이 필요한 되돌리기 어려운 행위다.',
    source: 'product/steam-registration-guide.md 1절, Steam Direct 등록비 문서',
  },
  srcs: [S.appfee, S.direct, S.onboarding],
});

// 28
add({
  kicker: '등록',
  title: '명의와 은행과 세무를 맞춘다',
  lines: [
    '회사 법적 명의는 제품을 소유하거나 배포 권리를 가진 주체여야 한다. 별칭 사용 금지.',
    '개인이 소유 주체면 Sole Proprietorship 으로 등록하고 법적 성명을 쓴다.',
    '은행 계좌 명의는 온보딩에 제출한 이름과 일치해야 한다. 개인 계좌도 가능하다.',
    '사업용 계좌가 반드시 필요하다고 이 문서는 말하지 않는다. 명의 일치가 조건이다.',
    '세무 정보는 제3자 검증을 거치며 2에서 7영업일이 걸리고 그 사이 수정할 수 없다.',
    '원천징수는 0에서 30%이고 미국 원천 매출에만 적용된다.',
  ],
  notes: {
    detail:
      '조세조약 적용 세율을 낮추려면 W-8BEN 에 준하는 정보와 TIN 이 필요하고, 이미 납부된 원천세는 환급되지 않는다. 비미국 납세자는 연 1회 1042-S 를 받는다.',
    uncertainty:
      '한국과 미국 조세조약의 실제 적용 세율은 공개 문서에 없고 세무 인터뷰 결과 화면에서만 확인된다. 국내 사업자등록과 게임제작업 등록 범위도 미확인이며 국세청 확인 사항이다.',
    source: 'Steamworks 온보딩, Steam 세무 FAQ, 국세청',
  },
  srcs: [S.onboarding, S.taxfaq, S.nts],
});

// 29
add({
  kicker: '등록',
  title: '100달러는 반드시 선지출하는 현금이다',
  lines: [
    '등록 시 반드시 선지출하는 현금 비용이다. 앱 하나당 100달러이고 환불되지 않는다.',
    'AGR 조정총매출 1,000달러를 달성한 이후 지급분에서 회수되면 그때 차감된다.',
    '회수분은 월간 리포트에 별도 라인 항목으로 표시된다.',
    '현재 원화 표에는 환율이 설정되어 있지 않아 이 100달러가 들어 있지 않다.',
    '따라서 원화 계산 밖에서 100달러와 적용 세금, 은행 비용을 별도로 더한다.',
    '지급 임계는 월 100달러, 지급은 판매월 더하기 30일. 회계 자문이 아니다.',
  ],
  notes: {
    detail:
      '정확한 서술은 이것이다. 등록 시 반드시 선지출하는 현금 비용이고, AGR 조건을 충족해 회수되면 그때 차감된다. 현재 원화 표에는 환율이 설정되어 있지 않아 별도로 100달러와 적용 세금, 은행 비용을 추가해야 한다. 손익분기와 현금 도달 시점은 서로 다른 문제다. 이 덱은 회계 자문도 세무 자문도 아니고, 이 비용을 포함하는 것이 손익분기를 왜곡한다고 주장하지도 않는다.',
    uncertainty:
      '데모를 만들 경우 별도 App ID 가 되는데 그때 추가 100달러가 발생하는지는 확인하지 못했다. 34번 슬라이드에서 다시 다룬다.',
    source: 'Steam Direct 등록비 문서, Steam 정산과 지급',
  },
  srcs: [S.appfee, S.payments],
});

// 30
add({
  kicker: '등록',
  title: '대기 구간은 병렬은 되지만 생략은 안 된다',
  lines: [
    '등록비 결제와 출시 사이에 30일 초기 대기가 있다.',
    'Coming Soon 페이지를 최소 2주 공개해야 출시할 수 있다.',
    '스토어 페이지 리뷰는 통상 3에서 5영업일이고 빌드 리뷰도 같다.',
    '같은 문서가 최소 7영업일을 계획하라고 권고하므로 버퍼 7영업일을 고정한다.',
    '온보딩 문서는 같은 리뷰를 1에서 5일로 적는다. 두 수치를 섞지 않고 보수적으로 잡는다.',
    '리뷰를 통과해도 출시는 자동이 아니다. 사람이 직접 버튼을 누른다.',
  ],
  fig: svgTimeline(),
  notes: {
    detail:
      '최초 승인 이후의 업데이트는 재심사 대상이 아니다. 한 번 리뷰하고 이후에는 언제든 갱신한다는 원칙이다.',
    uncertainty:
      '영업일 계산은 공휴일과 리뷰 대기열에 따라 달라진다. 위 구간을 일정 약속으로 쓰지 않는다.',
    source: 'Steamworks 온보딩, Coming Soon 문서, 스토어 리뷰 절차',
  },
  srcs: [S.onboarding, S.coming, S.review],
});

// 31
add({
  kicker: '등록',
  title: '스토어와 트레일러는 실제 빌드를 따라간다',
  lines: [
    '스토어 페이지에는 출시 시점에 실제 제공되는 기능만 적는다.',
    '캡슐에는 판독 가능한 타이틀, 스크린샷은 게임플레이만, 외부 링크는 금지다.',
    '빌드는 표기한 모든 OS 에서 실행되어야 하고 업로드는 SteamPipe 로 한다.',
    '트레일러는 게임플레이 위주로 하고 HUD 노출을 권장한다.',
    '무음 상태에서도 짧은 시간 안에 할 일이 이해되도록 구성한다.',
    '실제 Unity 캡처 전에는 어떤 영상도 게임플레이라고 부르지 않는다.',
  ],
  notes: {
    detail:
      '우리 영상 계획은 동사, 공간 변화, 새 질문 순서다. 0에서 6초 배선 연결과 수위 하강, 6에서 18초 두 기록 대조, 18에서 32초 밸브 시험과 되돌림, 32에서 45초 침수 선택의 대가, 45에서 55초 세 장소 대비, 55에서 60초 제목과 플랫폼.',
    uncertainty:
      '참고 영상은 과거 공개본이라 현재 출시 시점의 튜토리얼이나 성능을 대표하지 않는다. 조회수에서 구매로 이어지는 인과를 주장하지 않는다.',
    source: 'presentation/video-study.md, Steam 트레일러 가이드, 스토어 리뷰 절차',
  },
  srcs: [S.trailer, S.review, S.upload, D('presentation/video-study.md')],
});

// 32
add({
  kicker: '등록',
  title: 'AI 설문과 등급분류는 다른 축이다',
  lines: [
    '콘텐츠 설문은 일반, 성인, 생성형 AI 세 섹션이고 리뷰 제출 전에 전부 완료해야 한다.',
    'AI 섹션의 대상은 게임에 동봉되어 플레이어가 소비하는 콘텐츠다.',
    '개발 효율화 도구 사용은 이 섹션의 대상이 아니다.',
    '실시간 생성은 설계에서 제외했지만 사전 생성 신고 여부는 제작 이력에 달렸다.',
    '따라서 제작 착수 시점부터 에셋별 생성 이력을 기록해 두어야 한다.',
    '설문 답변이 면책이 되지 않는다. 공개 여부와 무관하게 콘텐츠 규칙을 지켜야 한다.',
  ],
  notes: {
    detail:
      '한국 등급분류는 별개 축이다. 국내에 유통하는 PC 게임물은 GCRB 등급분류 대상이라고 GCRB 가 안내한다. Steam 이 한국 자체등급분류사업자로 지정되었는지는 원문을 확보하지 못해 어느 쪽으로도 단정하지 않는다.',
    uncertainty:
      'Steam 설문이 만드는 지역 등급 표시가 각국 법정 등급분류를 대체한다고 공식 문서가 말하지 않는다. 확인 창구는 게임물관리위원회다.',
    source: '콘텐츠 설문 문서, GCRB, 게임물관리위원회, 게임산업법 원문',
  },
  srcs: [S.survey, S.gcrb, S.grac, S.law],
});

// 33
add({
  kicker: '등록',
  title: '할인과 환불의 실제 규칙',
  lines: [
    '출시 후 30일은 할인이 금지된다. 예외는 출시 전에 설정한 런치 할인뿐이다.',
    '런치 할인은 7일에서 14일, 최대 40%, 공식 권장은 10에서 15%다.',
    '일반 할인은 10%에서 95%, 1일에서 14일, 할인 사이 30일 간격이 필요하다.',
    '환불은 통상 구매 14일 이내이고 플레이 2시간 이내일 때 요청할 수 있다.',
    '이것은 요청 기준이지 모든 요청이 승인된다는 보장이 아니다.',
    '8시간 분량이라 2시간 규칙이 방어선이 된다는 말은 해석일 뿐이다.',
  ],
  notes: {
    detail:
      '환불률은 게임별 실측값이고 우리 값은 미측정이다. 22번 슬라이드의 ' + pct(REFUND) +
      '는 감도 확인용 가정치다. 2시간을 노리고 분량을 채우는 설계는 하지 않는다.',
    uncertainty:
      '환불 정책 페이지의 최종 수정일은 2024-04-23 표기였다. 인용 시점 이후 변경 가능성이 있다.',
    source: 'Steam 할인 규칙, Steam 환불 정책',
  },
  srcs: [S.discounts, S.refunds],
});

// 34
add({
  kicker: '등록',
  title: '데모는 별도 App ID 다',
  lines: [
    '데모를 내면 별도 App ID 이고 뎁포와 빌드를 따로 구성해야 한다.',
    '추가로 100달러가 드는지 여부는 확인하지 못했다. 든다고도 안 든다고도 가정하지 않는다.',
    '데모 공개 후 베이스 게임 스토어 페이지를 수동으로 재게시해야 버튼이 노출된다.',
    'Coming Soon 공개 즉시 커뮤니티 허브가 열리고 위시리스트 수집이 시작된다.',
    '즉 공개 시점은 마케팅 결정이자 되돌리기 어려운 행위다.',
    '축제 참가 일정은 현재 공식 일자를 확인하지 못해 이 덱에서 날짜를 적지 않는다.',
  ],
  notes: {
    detail:
      '데모 완주율은 우리가 정의할 게이트 지표 중 하나이지만 목표치조차 아직 설정되지 않았고 관측은 0건이다. 완주 로그만 세고 감상 응답은 제외한다.',
    uncertainty:
      '축제 등록 조건과 일정은 시점에 따라 바뀐다. 확인하지 않은 이벤트 날짜를 슬라이드에 적으면 그 자체가 오류가 된다.',
    source: 'Steam 데모 문서, Coming Soon 문서, product/business-model.md 9절',
  },
  srcs: [S.demos, S.coming, D('product/business-model.md')],
});

// 35
add({
  kicker: '판정',
  title: '지금 열려 있는 게이트는 없다',
  lines: [
    '슬라이스가 실제로 열리고 20분에서 30분이 측정되어야 첫 게이트가 움직인다.',
    '12명 5유형의 완주 측정이 있어야 ' + campaignMinutes + '분 주장을 유지할 수 있다.',
    '지불 의사 응답이 모여야 가격 후보 중 하나를 권장으로 승격할 수 있다.',
    '문서 간 숫자와 이름의 모순이 0이어야 상품 문서가 닫힌다.',
    '등록은 사용자 승인 후에만 시작한다. 결제, 서명, 공개는 되돌리기 어렵다.',
    '이 다섯 중 하나라도 비어 있으면 출시 일정을 말하지 않는다.',
  ],
  notes: {
    detail:
      '게이트는 문서 검증과 실제 게임 검증을 분리해 관리한다. 시간 합계가 맞아도 플레이 검증을 통과시키지 않는다는 것이 계약의 핵심 조항이다.',
    uncertainty:
      '현재 실제 게임 게이트 다수가 NOT-MEASURED 이며 이 덱은 어떤 게이트도 올리지 않는다.',
    source: 'production/premium-preproduction-contract.md 의 Honesty gates 절',
  },
  srcs: [D('production/premium-preproduction-contract.md')],
});

// 36
add({
  kicker: '판정',
  title: '결정해 주셔야 하는 것',
  lines: [
    '1. 슬라이스 T0 착수 승인. 전체 콘텐츠 생산이 아니라 20분에서 30분 검증분만.',
    '2. 가격 실험 채널 선택. 응답을 어디서 어떻게 모을지 정해야 후보를 닫을 수 있다.',
    '3. 등록 준비 범위. 명의와 세무 확인까지만 할지, 결제까지 갈지.',
    '4. 예산 상한. 현재 세 시나리오는 입력값이지 우리 예산이 아니다.',
    '5. 인력 구성. 1명인지 2명인지에 따라 기간 가정이 통째로 달라진다.',
    '이 다섯 중 어느 것도 이 덱이 대신 결정하지 않는다.',
  ],
  notes: {
    detail:
      '승인이 필요한 되돌리기 어려운 행위는 계정 생성과 연동, 전자서명, 신원과 은행과 세무 제출, 100달러 결제, 스토어 페이지 공개, 빌드 업로드, 리뷰 제출, 출시 버튼이다.',
    uncertainty:
      '승인 범위는 이월되지 않는다. 한 항목의 승인이 다른 항목으로 번지지 않는다.',
    source: 'product/steam-registration-guide.md 0절, production/premium-preproduction-contract.md 의 Release safety 절',
  },
  srcs: [D('product/steam-registration-guide.md'), D('production/premium-preproduction-contract.md')],
});

/* ------------------------------------------------------------------ *
 * 9. 시뮬레이터 마크업
 * ------------------------------------------------------------------ */

function simulatorHtml() {
  const discSteps = Array.from(
    new Set([0, 0.1, 0.2, 0.3, capB1 / 100, 0.4])
  ).sort((a, b) => a - b);
  const discOpts = discSteps
    .map(
      (d) =>
        '<option value="' + d + '"' + (Math.abs(d - DISC) < 1e-9 ? ' selected' : '') + '>' +
        Math.round(d * 1000) / 10 + '%</option>'
    )
    .join('');
  const priceOpts = PRICES.map(
    (p, i) =>
      '<option value="' + p + '"' + (i === 0 ? ' selected' : '') + '>B' + (i + 1) + ' ' + won(p) + '</option>'
  ).join('');
  const budgetOpts = eco.cashBudgets
    .map(
      (b, i) =>
        '<option value="' + b + '"' + (i === 0 ? ' selected' : '') + '>현금 ' + num(b / 10000) + '만원</option>'
    )
    .join('') +
    '<option value="' + laborCost + '">인건비 기회비용 ' + num(Math.round(laborCost / 10000)) + '만원</option>';
  const kOpts = KS.map(
    (k, i) => '<option value="' + k + '"' + (i === 0 ? ' selected' : '') + '>k = ' + k + '</option>'
  ).join('');
  return [
    '<div class="sim" id="sim">',
    '<div class="sim-in">',
    '<label>표시가<select id="simPrice">' + priceOpts + '</select></label>',
    '<label>할인<select id="simDisc">' + discOpts + '</select></label>',
    '<label>지역계수<select id="simK">' + kOpts + '</select></label>',
    '<label>환불 가정<select id="simRef"><option value="0">0%</option><option value="0.05">5%</option><option value="0.08" selected>8%</option><option value="0.15">15%</option></select></label>',
    '<label>배분 가정<select id="simShare"><option value="0.7" selected>70% (가정)</option><option value="0.65">65%</option><option value="0.8">80%</option></select></label>',
    '<label>회수 대상<select id="simBudget">' + budgetOpts + '</select></label>',
    '</div>',
    '<div class="sim-out">',
    '<div class="kv"><span>결제가</span><b id="simPaid">-</b></div>',
    '<div class="kv"><span>본당 수취</span><b id="simNet">-</b></div>',
    '<div class="kv"><span>손익분기 판매량</span><b id="simBe">-</b></div>',
    '<p class="sim-warn" id="simWarn"></p>',
    '</div>',
    '</div>',
  ].join('');
}

/* ------------------------------------------------------------------ *
 * 10. 렌더
 * ------------------------------------------------------------------ */

const CSS = `
:root{
  --ink:#0B1A1E; --ink2:#12262B; --ink3:#173238;
  --sea:#6FB3A8; --sea2:#3F7C77; --paper:#E9E4D8; --ochre:#E0AE63; --muted:#9DB3B1;
  --u: min(1vw, 1.7778vh);
}
*{box-sizing:border-box}
html,body{margin:0;padding:0;background:var(--ink);color:var(--paper);
  font-family:-apple-system,BlinkMacSystemFont,"Apple SD Gothic Neo","Noto Sans KR","Malgun Gothic",system-ui,sans-serif;
  -webkit-font-smoothing:antialiased;overflow:hidden}
body{height:100dvh}
.deck{position:relative;width:100vw;height:100dvh;display:flex;align-items:center;justify-content:center}
.slide{position:absolute;inset:0;display:flex;align-items:center;justify-content:center;
  visibility:hidden;opacity:0;pointer-events:none;transition:opacity .22s ease}
.slide.is-on{visibility:visible;opacity:1;pointer-events:auto}
.frame{width:min(100vw, 177.78vh);height:min(56.25vw, 100dvh);
  display:grid;grid-template-rows:auto 1fr auto;
  padding:calc(4*var(--u)) calc(6*var(--u)) calc(3.2*var(--u));
  background:
    radial-gradient(120% 90% at 12% 0%, rgba(63,124,119,.20), transparent 60%),
    linear-gradient(180deg, var(--ink2), var(--ink));
  border-top:calc(.35*var(--u)) solid var(--sea2)}
.shead{display:flex;justify-content:space-between;align-items:baseline;
  font-size:calc(1.35*var(--u));letter-spacing:.14em;color:var(--sea);text-transform:uppercase}
.shead .num{color:var(--muted);letter-spacing:.2em}
.sbody{display:flex;gap:calc(4*var(--u));align-items:center;min-height:0;padding:calc(1.6*var(--u)) 0}
.col{display:flex;flex-direction:column;justify-content:center;min-width:0;flex:1 1 52%}
.col.wide{flex:1 1 100%}
.figwrap{flex:1 1 48%;min-width:0;display:flex;align-items:center;justify-content:center}
h2{margin:0 0 calc(1.8*var(--u));font-size:calc(4.6*var(--u));line-height:1.16;letter-spacing:-.015em;font-weight:800}
.cover .sbody{align-items:stretch}
.cover-wrap{display:flex;flex-direction:column;justify-content:center;gap:calc(2.2*var(--u));width:100%}
.cover h1{margin:0;font-size:calc(8.6*var(--u));line-height:1.02;letter-spacing:-.03em;font-weight:800}
.cover .lead{margin:0;font-size:calc(2.9*var(--u));line-height:1.4;color:var(--sea);font-weight:600}
.cover .stats{display:flex;gap:calc(6*var(--u));margin-top:calc(1.4*var(--u));
  border-top:1px solid rgba(233,228,216,.2);padding-top:calc(1.8*var(--u))}
.cover .stat{display:flex;flex-direction:column;gap:calc(.4*var(--u))}
.cover .stat b{font-size:calc(3.6*var(--u));line-height:1;letter-spacing:-.01em}
.cover .stat span{font-size:calc(1.3*var(--u));color:var(--muted);letter-spacing:.02em}
.cover .cap{margin:0;font-size:calc(1.4*var(--u));color:var(--muted);letter-spacing:.05em}
.lines{list-style:none;margin:0;padding:0;display:flex;flex-direction:column;gap:calc(.9*var(--u))}
.lines li{position:relative;padding-left:calc(2.1*var(--u));font-size:calc(1.95*var(--u));line-height:1.5;color:#D9D6CB}
.lines li::before{content:"";position:absolute;left:0;top:calc(.85*var(--u));width:calc(1.1*var(--u));height:calc(.22*var(--u));background:var(--sea)}
.cover .lines li{font-size:calc(2.1*var(--u))}
.sfoot{display:flex;flex-wrap:wrap;gap:calc(1.4*var(--u));align-items:center;
  border-top:1px solid rgba(233,228,216,.18);padding-top:calc(1.1*var(--u));font-size:calc(1.15*var(--u))}
.sfoot .lbl{color:var(--sea);letter-spacing:.14em}
.sfoot a{color:var(--paper);text-decoration:underline;text-underline-offset:.25em;text-decoration-color:var(--sea2)}
.sfoot a:hover,.sfoot a:focus{color:var(--ochre);text-decoration-color:var(--ochre)}
.sfoot .doc{color:var(--muted)}
.col.with-fig .lines li{font-size:calc(1.76*var(--u));line-height:1.45}
.col.with-fig h2{font-size:calc(4.1*var(--u));margin-bottom:calc(1.4*var(--u))}
.dia{width:100%;height:auto;max-height:calc(40*var(--u))}
.dl{fill:var(--paper);font-size:13px;font-family:inherit}
.dl-s{fill:var(--muted);font-size:11.5px}
.dl-c{text-anchor:middle}
.dl-b{font-weight:700}
.tb{width:100%;border-collapse:collapse;font-size:calc(1.45*var(--u));line-height:1.35}
.tb.small{font-size:calc(1.28*var(--u))}
.tb th{text-align:left;color:var(--sea);font-weight:600;letter-spacing:.04em;
  border-bottom:1px solid rgba(111,179,168,.5);padding:calc(.6*var(--u)) calc(.7*var(--u))}
.tb td{border-bottom:1px solid rgba(233,228,216,.12);padding:calc(.55*var(--u)) calc(.7*var(--u));vertical-align:top;color:#D9D6CB}
.sim{display:flex;flex-direction:column;gap:calc(1.4*var(--u));width:100%}
.sim-in{display:grid;grid-template-columns:repeat(3,minmax(0,1fr));gap:calc(1*var(--u))}
.sim-in label{display:flex;flex-direction:column;gap:calc(.4*var(--u));font-size:calc(1.2*var(--u));color:var(--sea)}
.sim-in select{font:inherit;font-size:calc(1.4*var(--u));padding:calc(.5*var(--u)) calc(.6*var(--u));
  background:var(--ink);color:var(--paper);border:1px solid var(--sea2);border-radius:calc(.4*var(--u))}
.sim-out{display:flex;flex-direction:column;gap:calc(.5*var(--u));border:1px solid var(--sea2);
  border-radius:calc(.5*var(--u));padding:calc(1.2*var(--u))}
.kv{display:flex;justify-content:space-between;align-items:baseline;font-size:calc(1.5*var(--u));color:var(--muted)}
.kv b{font-size:calc(2.6*var(--u));color:var(--paper)}
.sim-warn{margin:calc(.4*var(--u)) 0 0;font-size:calc(1.2*var(--u));color:var(--ochre);min-height:calc(1.7*var(--u))}
.notes{display:none}
.bar{position:fixed;left:50%;transform:translateX(-50%);bottom:calc(1.2*var(--u));
  display:flex;gap:calc(.6*var(--u));align-items:center;padding:calc(.6*var(--u)) calc(.9*var(--u));
  background:rgba(11,26,30,.86);border:1px solid rgba(111,179,168,.35);border-radius:999px;z-index:20;
  backdrop-filter:blur(6px)}
.bar button{font:inherit;font-size:calc(1.25*var(--u));padding:calc(.45*var(--u)) calc(1*var(--u));
  background:transparent;color:var(--paper);border:1px solid transparent;border-radius:999px;cursor:pointer}
.bar button:hover{border-color:var(--sea2);color:var(--sea)}
.bar button:focus-visible{outline:2px solid var(--ochre);outline-offset:2px}
.bar .count{font-size:calc(1.25*var(--u));color:var(--muted);padding:0 calc(.6*var(--u));min-width:calc(6*var(--u));text-align:center}
.bar button[aria-pressed="true"]{border-color:var(--ochre);color:var(--ochre)}
.bar button[disabled]{opacity:.42;cursor:not-allowed}
.bar button[disabled]:hover{border-color:transparent;color:var(--paper)}
dialog{background:var(--ink2);color:var(--paper);border:1px solid var(--sea2);border-radius:10px;
  max-width:min(92vw,980px);width:100%;padding:24px;font-size:15px;line-height:1.6}
dialog::backdrop{background:rgba(3,10,12,.78)}
dialog h3{margin:0 0 14px;font-size:18px;color:var(--sea);letter-spacing:.08em}
.toc-grid{display:grid;grid-template-columns:repeat(auto-fill,minmax(210px,1fr));gap:8px;max-height:64vh;overflow:auto}
.toc-grid button{font:inherit;font-size:13.5px;text-align:left;padding:9px 11px;background:var(--ink);
  color:var(--paper);border:1px solid rgba(111,179,168,.3);border-radius:6px;cursor:pointer}
.toc-grid button:hover{border-color:var(--sea);color:var(--sea)}
.toc-grid button b{color:var(--muted);font-weight:600;margin-right:8px}
.nt h4{margin:16px 0 4px;font-size:13px;letter-spacing:.12em;color:var(--sea)}
.nt p{margin:0;color:#D9D6CB}
.nt .src{color:var(--muted);font-size:13.5px}
.dlg-foot{display:flex;justify-content:flex-end;margin-top:18px}
.dlg-foot button{font:inherit;font-size:14px;padding:8px 16px;background:transparent;color:var(--paper);
  border:1px solid var(--sea2);border-radius:6px;cursor:pointer}
@media (max-width:900px), (orientation:portrait){
  html,body{overflow:auto}
  .deck{height:auto;display:block}
  .slide{position:static;visibility:visible;opacity:1;pointer-events:auto;display:none}
  .slide.is-on{display:flex}
  .frame{width:100%;height:auto;min-height:100dvh;padding:22px 18px 92px}
  .shead{font-size:11px}
  h2{font-size:30px;margin-bottom:14px}
  .cover h1{font-size:34px}
  .cover .lead{font-size:17px}
  .cover .stats{flex-direction:column;gap:12px;padding-top:14px}
  .cover .stat b{font-size:22px}
  .cover .stat span{font-size:12px}
  .cover .cap{font-size:12px}
  .sbody{flex-direction:column;align-items:stretch;gap:18px;padding:6px 0}
  .lines li,.cover .lines li{font-size:15.5px;padding-left:14px}
  .lines li::before{width:8px;height:2px;top:10px}
  .tb,.tb.small{font-size:12.5px}
  .sim-in{grid-template-columns:repeat(2,minmax(0,1fr))}
  .sim-in select{font-size:14px}
  .kv{font-size:13px}.kv b{font-size:20px}
  .sfoot{font-size:11.5px;gap:10px}
  .bar{bottom:12px;padding:6px 8px}
  .bar button{font-size:12px;padding:7px 10px}
  .bar .count{font-size:12px;min-width:56px}
  .dia{max-height:none}
}
@media print{
  @page{size:A4 landscape;margin:8mm}
  html,body{overflow:visible;background:#fff}
  .bar,dialog{display:none !important}
  .deck{display:block;width:auto;height:auto}
  .slide{position:static;display:block;visibility:visible !important;opacity:1 !important;
    break-after:page;page-break-after:always}
  .slide:last-child{break-after:auto;page-break-after:auto}
  .frame{--u:1.75mm;width:100%;height:172mm;background:#0B1A1E;-webkit-print-color-adjust:exact;print-color-adjust:exact}
  .sfoot a{color:#E9E4D8}
}
`;

function renderSlide(s, i) {
  const n = i + 1;
  const pad = String(n).padStart(2, '0');
  const hasFig = Boolean(s.fig);
  const lines = (s.lines || []).map((l) => '<li>' + l + '</li>').join('');
  const foot =
    '<span class="lbl">출처</span>' +
    (s.srcs || [])
      .map((x) =>
        x[1]
          ? '<a href="' + x[1] + '" target="_blank" rel="noopener noreferrer">' + esc(x[0]) + '</a>'
          : '<span class="doc">' + esc(x[0]) + '</span>'
      )
      .join('');
  if (s.cover) {
    const stats = (s.stats || [])
      .map(
        (x) =>
          '<div class="stat"><b>' + x.v + '</b><span>' + esc(x.k) + '</span></div>'
      )
      .join('');
    return [
      '<section class="slide cover" id="s' + n + '" data-n="' + n +
        '" role="group" aria-roledescription="슬라이드" aria-label="' + n + '번 슬라이드. ' + esc(stripTags(s.title)) + '">',
      '<div class="frame">',
      '<header class="shead"><span class="kick">' + esc(s.kicker) + '</span><span class="num">' + pad + ' / 36</span></header>',
      '<div class="sbody"><div class="cover-wrap">',
      '<h1>' + s.title + '</h1>',
      '<p class="lead">' + esc(s.lead) + '</p>',
      '<div class="stats">' + stats + '</div>',
      '<p class="cap">' + esc(s.caption) + '</p>',
      '</div></div>',
      '<footer class="sfoot">' + foot + '</footer>',
      '</div>',
      '<div class="notes" data-notes>',
      '<h4>세부</h4><p>' + esc(s.notes.detail) + '</p>',
      '<h4>불확실성</h4><p>' + esc(s.notes.uncertainty) + '</p>',
      '<h4>출처</h4><p class="src">' + esc(s.notes.source) + '</p>',
      '</div>',
      '</section>',
    ].join('');
  }
  const body = hasFig
    ? '<div class="col with-fig">' + '<h2>' + s.title + '</h2><ul class="lines">' + lines + '</ul></div>' +
      '<div class="figwrap">' + s.fig + '</div>'
    : '<div class="col wide"><h2>' + s.title + '</h2><ul class="lines">' + lines + '</ul></div>';
  return [
    '<section class="slide' + (s.cover ? ' cover' : '') + '" id="s' + n + '" data-n="' + n +
      '" role="group" aria-roledescription="슬라이드" aria-label="' + n + '번 슬라이드. ' + esc(stripTags(s.title)) + '">',
    '<div class="frame">',
    '<header class="shead"><span class="kick">' + esc(s.kicker) + '</span><span class="num">' + pad + ' / 36</span></header>',
    '<div class="sbody">' + body + '</div>',
    '<footer class="sfoot">' + foot + '</footer>',
    '</div>',
    '<div class="notes" data-notes>',
    '<h4>세부</h4><p>' + esc(s.notes.detail) + '</p>',
    '<h4>불확실성</h4><p>' + esc(s.notes.uncertainty) + '</p>',
    '<h4>출처</h4><p class="src">' + esc(s.notes.source) + '</p>',
    '</div>',
    '</section>',
  ].join('');
}

function stripTags(s) {
  return String(s).replace(/<[^>]*>/g, '');
}

const PAGE_SCRIPT = [
  '(function(){',
  '"use strict";',
  'var deck=document.getElementById("deck");',
  'var slides=Array.prototype.slice.call(deck.querySelectorAll(".slide"));',
  'var total=slides.length;',
  'var idx=0, autoOn=false, timer=null;',
  'var elCount=document.getElementById("count");',
  'var tocDlg=document.getElementById("tocDlg");',
  'var noteDlg=document.getElementById("noteDlg");',
  'var noteBody=document.getElementById("noteBody");',
  'var noteHead=document.getElementById("noteHead");',
  'function apply(){',
  '  for(var i=0;i<total;i++){',
  '    var on=(i===idx), s=slides[i];',
  '    s.classList.toggle("is-on",on);',
  '    if(on){ s.removeAttribute("inert"); s.removeAttribute("aria-hidden"); }',
  '    else { s.setAttribute("inert",""); s.setAttribute("aria-hidden","true"); }',
  '  }',
  '  elCount.textContent=(idx+1)+" / "+total;',
  '  if(noteDlg.open) fillNotes();',
  '  updateNoteWin();',
  '  if(history.replaceState) history.replaceState(null,"","#s"+(idx+1));',
  '  window.scrollTo(0,0);',
  '}',
  'function go(n){ if(n<0)n=0; if(n>total-1)n=total-1; idx=n; apply(); resetAuto(); }',
  'function next(){ go(idx+1); } function prev(){ go(idx-1); }',
  'function fillNotes(){',
  '  var src=slides[idx].querySelector("[data-notes]");',
  '  noteHead.textContent=(idx+1)+" / "+total+" 발표자 노트";',
  '  noteBody.innerHTML=src?src.innerHTML:"";',
  '}',
  'var noteWin=null;',
  'function noteWinAlive(){ return noteWin && !noteWin.closed && noteWin.document; }',
  'function updateNoteWin(){',
  '  if(!noteWinAlive()) return;',
  '  var d=noteWin.document;',
  '  var src=slides[idx].querySelector("[data-notes]");',
  '  var h=d.getElementById("h"), b=d.getElementById("b");',
  '  if(h) h.textContent=(idx+1)+" / "+total+" 발표자 노트";',
  '  if(b) b.innerHTML=src?src.innerHTML:"";',
  '}',
  'function openNoteWin(){',
  '  var btn=document.getElementById("btnNoteWin");',
  '  if(noteWinAlive()){ noteWin.focus(); updateNoteWin(); return; }',
  '  noteWin=window.open("","deckNotesWin","popup=yes,width=560,height=780");',
  '  if(!noteWin){ btn.textContent="노트 창 차단됨"; return; }',
  '  btn.textContent="노트 창";',
  '  var d=noteWin.document;',
  '  d.open();',
  '  d.write("<!doctype html><html lang=ko><head><meta charset=utf-8><title>발표자 노트</title>"+',
  '    "<style>body{margin:0;padding:20px;background:#0B1A1E;color:#E9E4D8;font:15px/1.65 system-ui,sans-serif}"+',
  '    "h3{margin:0 0 14px;font-size:14px;letter-spacing:.12em;color:#6FB3A8}"+',
  '    "h4{margin:18px 0 4px;font-size:12px;letter-spacing:.14em;color:#6FB3A8}"+',
  '    "p{margin:0;color:#D9D6CB}.src{color:#9DB3B1;font-size:13px}</style></head>"+',
  '    "<body><h3 id=h></h3><div id=b></div></body></html>");',
  '  d.close();',
  '  updateNoteWin();',
  '}',
  'function resetAuto(){ if(timer){clearInterval(timer);timer=null;} if(autoOn){ timer=setInterval(function(){ if(idx<total-1){go(idx+1);} else {toggleAuto(false);} },25000); } }',
  'function toggleAuto(force){',
  '  autoOn=(typeof force==="boolean")?force:!autoOn;',
  '  var b=document.getElementById("btnAuto");',
  '  b.setAttribute("aria-pressed",autoOn?"true":"false");',
  '  b.textContent=autoOn?"자동 넘김 켜짐":"자동 넘김 꺼짐";',
  '  resetAuto();',
  '}',
  'document.getElementById("btnPrev").addEventListener("click",prev);',
  'document.getElementById("btnNext").addEventListener("click",next);',
  'document.getElementById("btnAuto").addEventListener("click",function(){toggleAuto();});',
  'document.getElementById("btnPrint").addEventListener("click",function(){window.print();});',
  'document.getElementById("btnToc").addEventListener("click",function(){ tocDlg.showModal(); });',
  'document.getElementById("btnNotes").addEventListener("click",function(){ fillNotes(); noteDlg.showModal(); });',
  'document.getElementById("btnNoteWin").addEventListener("click",openNoteWin);',
  'document.addEventListener("fullscreenchange",function(){',
  '  var b=document.getElementById("btnNoteWin");',
  '  var fs=!!document.fullscreenElement;',
  '  b.disabled=fs;',
  '  b.title=fs?"전체화면에서는 노트 창 버튼이 비활성이다":"별도 창으로 노트를 연다";',
  '});',
  'document.getElementById("btnFull").addEventListener("click",function(){',
  '  if(document.fullscreenElement){ document.exitFullscreen(); }',
  '  else if(document.documentElement.requestFullscreen){ document.documentElement.requestFullscreen(); }',
  '});',
  'Array.prototype.forEach.call(document.querySelectorAll("[data-close]"),function(b){',
  '  b.addEventListener("click",function(){ var d=b.closest("dialog"); if(d) d.close(); });',
  '});',
  'Array.prototype.forEach.call(document.querySelectorAll("[data-goto]"),function(b){',
  '  b.addEventListener("click",function(){ go(parseInt(b.getAttribute("data-goto"),10)-1); tocDlg.close(); });',
  '});',
  'document.addEventListener("keydown",function(e){',
  '  if(e.target && /^(SELECT|INPUT|TEXTAREA)$/.test(e.target.tagName)) return;',
  '  var k=e.key;',
  '  if(k==="ArrowRight"||k==="ArrowDown"||k==="PageDown"||k===" "){ e.preventDefault(); next(); }',
  '  else if(k==="ArrowLeft"||k==="ArrowUp"||k==="PageUp"){ e.preventDefault(); prev(); }',
  '  else if(k==="Home"){ e.preventDefault(); go(0); }',
  '  else if(k==="End"){ e.preventDefault(); go(total-1); }',
  '  else if(k==="t"||k==="T"){ if(!tocDlg.open){ e.preventDefault(); tocDlg.showModal(); } }',
  '  else if(k==="n"||k==="N"){ if(!noteDlg.open){ e.preventDefault(); fillNotes(); noteDlg.showModal(); } }',
  '  else if(k==="f"||k==="F"){ e.preventDefault(); document.getElementById("btnFull").click(); }',
  '});',
  'var tx=null,ty=null;',
  'deck.addEventListener("touchstart",function(e){ if(e.touches.length===1){ tx=e.touches[0].clientX; ty=e.touches[0].clientY; } },{passive:true});',
  'deck.addEventListener("touchend",function(e){',
  '  if(tx===null) return; var t=e.changedTouches[0];',
  '  var dx=t.clientX-tx, dy=t.clientY-ty;',
  '  if(Math.abs(dx)>60 && Math.abs(dx)>Math.abs(dy)*1.4){ if(dx<0) next(); else prev(); }',
  '  tx=null; ty=null;',
  '},{passive:true});',
  // 시뮬레이터
  'var VAT=SIM_VAT, RESERVE=SIM_RESERVE, CHARGE=SIM_CHARGE, FLOOR=SIM_FLOOR;',
  'function fmt(n){ return Math.round(n).toLocaleString("ko-KR"); }',
  'function sim(){',
  '  var p=parseFloat(document.getElementById("simPrice").value);',
  '  var d=parseFloat(document.getElementById("simDisc").value);',
  '  var k=parseFloat(document.getElementById("simK").value);',
  '  var r=parseFloat(document.getElementById("simRef").value);',
  '  var s=parseFloat(document.getElementById("simShare").value);',
  '  var b=parseFloat(document.getElementById("simBudget").value);',
  '  var paid=p*(1-d);',
  '  var net=paid/(1+VAT)*k*(1-r-CHARGE)*s-RESERVE;',
  '  document.getElementById("simPaid").textContent=fmt(paid)+"원";',
  '  document.getElementById("simNet").textContent=(net>0?fmt(net):"0")+"원";',
  '  document.getElementById("simBe").textContent=(net>0?fmt(Math.ceil(b/net))+"본":"계산 불가");',
  '  var w=[];',
  '  if(paid<FLOOR) w.push("결제가가 하한 "+fmt(FLOOR)+"원 미만이다. 이 조합은 사용 금지.");',
  '  if(net<=0) w.push("본당 수취가 0 이하다. 준비금과 배분 가정을 다시 본다.");',
  '  w.push("배분율은 가정이며 Steam 공개 사실이 아니다.");',
  '  document.getElementById("simWarn").textContent=w.join(" ");',
  '}',
  'Array.prototype.forEach.call(document.querySelectorAll("#sim select"),function(el){ el.addEventListener("change",sim); });',
  'sim();',
  'var h=(location.hash||"").match(/^#s(\\d+)$/);',
  'if(h){ var n=parseInt(h[1],10); if(n>=1&&n<=total) idx=n-1; }',
  'toggleAuto(false);',
  'apply();',
  '})();',
]
  .join('\n')
  .replace('SIM_VAT', String(VAT))
  .replace('SIM_RESERVE', String(RESERVE))
  .replace('SIM_CHARGE', String(CHARGE))
  .replace('SIM_FLOOR', String(eco.launchFloor));

function renderPage() {
  const tocButtons = slides
    .map(
      (s, i) =>
        '<button type="button" data-goto="' + (i + 1) + '"><b>' +
        String(i + 1).padStart(2, '0') + '</b>' + esc(stripTags(s.title)) + '</button>'
    )
    .join('');
  return [
    '<!doctype html>',
    '<html lang="ko">',
    '<head>',
    '<meta charset="utf-8">',
    '<meta name="viewport" content="width=device-width, initial-scale=1, viewport-fit=cover">',
    '<title>조수기록국: 마지막 당직 - 사전제작과 Steam 등록 브리핑</title>',
    '<meta name="description" content="내부 검토용 사전제작 브리핑. 게임 미제작, 플레이테스트 0건, 가격 후보 전부 미승인.">',
    '<meta name="robots" content="noindex, nofollow">',
    '<style>' + CSS + '</style>',
    '</head>',
    '<body>',
    '<main class="deck" id="deck">',
    slides.map(renderSlide).join('\n'),
    '</main>',
    '<nav class="bar" aria-label="슬라이드 조작">',
    '<button type="button" id="btnPrev" aria-label="이전 슬라이드">이전</button>',
    '<span class="count" id="count" aria-live="polite">1 / 36</span>',
    '<button type="button" id="btnNext" aria-label="다음 슬라이드">다음</button>',
    '<button type="button" id="btnToc">목차</button>',
    '<button type="button" id="btnNotes">노트</button>',
    '<button type="button" id="btnNoteWin">노트 창</button>',
    '<button type="button" id="btnFull">전체화면</button>',
    '<button type="button" id="btnAuto" aria-pressed="false">자동 넘김 꺼짐</button>',
    '<button type="button" id="btnPrint">인쇄</button>',
    '</nav>',
    '<dialog id="tocDlg" aria-label="슬라이드 목차">',
    '<h3>목차</h3>',
    '<div class="toc-grid">' + tocButtons + '</div>',
    '<div class="dlg-foot"><button type="button" data-close>닫기</button></div>',
    '</dialog>',
    '<dialog id="noteDlg" aria-label="발표자 노트">',
    '<h3 id="noteHead">발표자 노트</h3>',
    '<div class="nt" id="noteBody"></div>',
    '<div class="dlg-foot"><button type="button" data-close>닫기</button></div>',
    '</dialog>',
    '<script>' + PAGE_SCRIPT + '</script>',
    '</body>',
    '</html>',
  ].join('\n');
}

/* ------------------------------------------------------------------ *
 * 11. 자체 점검
 * ------------------------------------------------------------------ */

function selfCheck(html) {
  const errs = [];
  const warns = [];
  if (slides.length !== 36) errs.push('슬라이드 수가 36이 아니다: ' + slides.length);
  slides.forEach((s, i) => {
    const n = i + 1;
    const lc = (s.lines || []).length;
    if (lc < 1) errs.push(n + '번 슬라이드 본문 0줄');
    if (lc > 6) errs.push(n + '번 슬라이드 본문 ' + lc + '줄 (최대 6줄 초과)');
    if (!s.notes || !s.notes.detail || !s.notes.uncertainty || !s.notes.source)
      errs.push(n + '번 슬라이드 노트 3항목 미완');
    if (!s.srcs || !s.srcs.length) errs.push(n + '번 슬라이드 출처 없음');
    if (!s.title) errs.push(n + '번 슬라이드 제목 없음');
  });
  const linked = slides.filter((s) => (s.srcs || []).some((x) => x[1])).length;
  if (linked < 20) warns.push('외부 링크가 있는 슬라이드가 ' + linked + '장뿐이다');

  // 표지 구조 검사
  const cover = slides[0];
  if (!cover.cover) errs.push('1번 슬라이드가 표지가 아니다');
  if (!cover.lead || !cover.caption || !(cover.stats || []).length)
    errs.push('표지에 lead / stats / caption 이 없다');
  if ((cover.stats || []).length !== 3) errs.push('표지 수치가 3개가 아니다');
  if (html.indexOf('<h1>') === -1) errs.push('표지 h1 없음');
  // 잘못된 회계 서술 금지
  ['고정비가 아니', '고정비와 섞지', '비용이 아니라 조건부'].forEach((bad) => {
    if (html.indexOf(bad) !== -1) errs.push('금지된 회계 서술 발견: ' + bad);
  });
  if (html.indexOf('70 대 30') !== -1 && html.indexOf('가정') === -1)
    errs.push('배분율을 사실처럼 적었다');
  // 단계 예산 하드코딩 금지: 생성기 소스에 고정 분 문자열이 남아 있으면 실패
  try {
    const own = readFileSync(fileURLToPath(import.meta.url), 'utf8');
    const banned = [
      '4' + '50분',
      '아' + '홉 장',
      '이 3' + '0분이',
      '나머지 ' + '3구역',
      '두 ' + '가지만',
      // C5-F1: T0 슬라이스 범위를 손으로 적은 문자열이 다시 들어오면 실패한다.
      '허브와 ' + '제3수문',
      'reader 와 ' + 'alignment 와 seal',
      'SLICE_ZONE' + '_COUNT',
      // C5-F5: 인수 테스트 수를 슬라이드 소스에 다시 적으면 실패한다.
      '인수 테스트 ' + '14개',
    ];
    banned.forEach((bad) => {
      if (own.indexOf(bad) !== -1) errs.push('생성기에 하드코딩된 단계 값: ' + bad);
    });
  } catch (e) {
    warns.push('생성기 자기 검사 생략: ' + e.message);
  }
  // 렌더된 분 합이 campaign 합계와 일치하는지
  const renderedSum = stages.reduce((a, st) => a + st.minutes, 0);
  if (renderedSum !== campaignMinutes) errs.push('단계 분 합 불일치');
  // 캠페인 단계 값이 실제로 렌더되었는지 확인
  stages.forEach((st) => {
    if (html.indexOf('<td>' + st.minutes + '</td>') === -1)
      errs.push(st.id + ' 단계 분(' + st.minutes + ')이 표에 없다');
  });
  // 6법 정본 문구 게이트 (RFC-P3-014)
  LAWS.forEach((l) => {
    if (html.indexOf(l.name) === -1) errs.push('6법 정본 문구 누락: ' + l.name);
    if (html.indexOf(l.rule) === -1) errs.push('6법 정본 규칙 누락: ' + l.name);
  });
  ['이번 조수에 보호 용량', '법 4 용량', '4 용량'].forEach((bad) => {
    if (html.indexOf(bad) !== -1) errs.push('기각된 6법 문구 발견: ' + bad);
  });
  // 시간 수용 판정 게이트 (RFC-P3-011)
  ['중앙값 420분 이상', '600분 이하여야', '상위 25%가 600분'].forEach((bad) => {
    if (html.indexOf(bad) !== -1) errs.push('폐기된 시간 수용 조건 발견: ' + bad);
  });
  if (html.indexOf(TIME.key) === -1) errs.push('시간 판정 키가 덱에 없다');
  if (html.indexOf(TIME_BAND_TEXT) === -1) errs.push('목표 밴드 문구가 덱에 없다');
  // 계약 본문이 같은 값을 유지하는지 (정본은 계약이고 덱은 인용이다)
  if (contractText) {
    [TIME.lo + '~' + TIME.hi, '중앙값 < ' + TIME.withdrawMedian, '하위 25% < ' + TIME.withdrawP25]
      .forEach((tok) => {
        if (contractText.indexOf(tok) === -1)
          errs.push('계약 Time acceptance 와 덱 상수 불일치: ' + tok);
      });
  } else {
    warns.push('계약 파일을 찾지 못해 시간 상수 대조를 건너뛰었다');
  }
  // 확정 방식 게이트 (RFC-P3-015)
  if (html.indexOf('확정만 길게 누름') !== -1) errs.push('폐기된 확정 방식 서술 발견');
  // 폐기 용어 게이트 (C5-F5). 정본 = unity-implementation.md L69: sourceType 상이 AND originId 상이.
  // 검사 문자열은 조각으로 조립해 이 게이트 자신이 렌더 대상이 되지 않게 한다.
  if (html.indexOf('매체 ' + '경로') !== -1) errs.push('폐기 용어 발견: 세 번째 용어(매체+경로)');
  // 인수 테스트 수는 11절에서 세어 렌더한다. 손으로 적은 값이 다시 들어오면 여기서 걸린다.
  if (html.indexOf('기술 인수 테스트 ' + ACCEPT_TESTS + '개') === -1)
    errs.push('17번 인수 테스트 수가 11절 파생값으로 렌더되지 않았다');
  // T0 수직 슬라이스 범위 게이트 (C5-F1). 덱은 live campaign.json 에서 파생하고,
  // systems/unity-implementation.md 10절(정본 서술)과 어긋나면 빌드가 실패한다.
  const uiSpecPath = path.join(ROOT, 'systems/unity-implementation.md');
  if (existsSync(uiSpecPath)) {
    const specLine = readFileSync(uiSpecPath, 'utf8')
      .split('\n')
      .find((l) => /T0 범위/.test(l));
    if (!specLine) {
      warns.push('unity-implementation.md 에서 T0 범위 행을 찾지 못해 대조를 건너뛰었다');
    } else {
      const toks = Array.from(specLine.matchAll(/`([a-z]+)`/g)).map((m) => m[1]);
      const specZones = toks.filter((t) => allZones.includes(t)).sort();
      const specTools = toks.filter((t) => allTools.includes(t)).sort();
      const specMin = specLine.match(/(\d+)\s*분/);
      if (specZones.join(',') !== sliceZoneIds.slice().sort().join(','))
        errs.push(
          'T0 구역이 정본 10절과 다르다: 데이터 ' + sliceZoneIds.join('/') +
            ' vs 정본 ' + specZones.join('/')
        );
      if (specTools.join(',') !== sliceTools.slice().sort().join(','))
        errs.push(
          'T0 도구가 정본 10절과 다르다: 데이터 ' + sliceTools.join('/') +
            ' vs 정본 ' + specTools.join('/')
        );
      if (specMin && Number(specMin[1]) !== firstStage.minutes)
        errs.push('T0 분이 정본 10절과 다르다: 데이터 ' + firstStage.minutes + ' vs 정본 ' + specMin[1]);
    }
  } else {
    warns.push('unity-implementation.md 를 찾지 못해 T0 범위 대조를 건너뛰었다');
  }
  // 파생 문장이 실제로 렌더되었는지 (상수 복귀 시 여기서 걸린다)
  if (html.indexOf('포함은 ' + sliceZoneText) === -1)
    errs.push('26번 T0 포함 범위 문장이 파생값으로 렌더되지 않았다');
  if (html.indexOf(sliceZoneText + '에서만 진행한다') === -1)
    errs.push('11번 T0 구역 문장이 파생값으로 렌더되지 않았다');
  // live zoneId 전부가 용어집 6-1절에 연결돼 있어야 한다
  allZones.forEach((z) => {
    if (!ZONE_KO[z]) errs.push('용어집 6-1절에 zoneId 대응이 없다: ' + z);
  });
  // 도구 없는 비트가 퍼즐이면 12번 문장이 거짓이 된다
  if (toollessBeats.some((b) => b.kind === 'puzzle'))
    errs.push('도구 없는 비트에 퍼즐이 있다: 12번 문장 재작성 필요');
  if (html.indexOf('id="btnNoteWin"') === -1) errs.push('노트 창 버튼 없음');
  if (html.indexOf('deckNotesWin') === -1) errs.push('노트 창 스크립트 없음');
  if (html.indexOf('\u2014') !== -1) errs.push('em dash 발견');
  if (html.indexOf('\u2013') !== -1) errs.push('en dash 발견');
  const emoji = html.match(
    /[\u{1F300}-\u{1FAFF}\u{2600}-\u{27BF}\u{FE0F}\u{1F000}-\u{1F0FF}]/gu
  );
  if (emoji) errs.push('이모지 ' + emoji.length + '건 발견');
  const secs = (html.match(/<section class="slide/g) || []).length;
  if (secs !== 36) errs.push('렌더된 section 수 ' + secs);
  const tocs = (html.match(/data-goto="/g) || []).length;
  if (tocs !== 36) errs.push('목차 버튼 수 ' + tocs);
  // 태그 균형 (간단 검사)
  const openDiv = (html.match(/<div\b/g) || []).length;
  const closeDiv = (html.match(/<\/div>/g) || []).length;
  if (openDiv !== closeDiv) errs.push('div 태그 불균형 ' + openDiv + ' / ' + closeDiv);
  const openSec = (html.match(/<section\b/g) || []).length;
  const closeSec = (html.match(/<\/section>/g) || []).length;
  if (openSec !== closeSec) errs.push('section 태그 불균형');
  const openTbl = (html.match(/<table\b/g) || []).length;
  const closeTbl = (html.match(/<\/table>/g) || []).length;
  if (openTbl !== closeTbl) errs.push('table 태그 불균형');
  const openSvg = (html.match(/<svg\b/g) || []).length;
  const closeSvg = (html.match(/<\/svg>/g) || []).length;
  if (openSvg !== closeSvg) errs.push('svg 태그 불균형');
  if (/https?:\/\/[^"'\s]*\.(png|jpg|jpeg|gif|webp|svg|woff2?|css|js)/i.test(html))
    errs.push('외부 자산 참조 발견');
  return { errs, warns, linked, secs, openSvg, openTbl };
}

/* ------------------------------------------------------------------ *
 * 12. main
 * ------------------------------------------------------------------ */

function parseArgs(argv) {
  const outs = [];
  for (let i = 0; i < argv.length; i++) {
    if (argv[i] === '--out' || argv[i] === '-o') {
      const v = argv[i + 1];
      if (!v) throw new Error('--out 뒤에 경로가 필요하다');
      outs.push(v);
      i++;
    }
  }
  if (!outs.length) outs.push(path.join(HERE, 'steam-game-plan.html'));
  return outs;
}

function main() {
  const outs = parseArgs(process.argv.slice(2));
  const html = renderPage();
  const check = selfCheck(html);
  for (const o of outs) {
    const abs = path.isAbsolute(o) ? o : path.resolve(process.cwd(), o);
    mkdirSync(path.dirname(abs), { recursive: true });
    writeFileSync(abs, html, 'utf8');
  }
  const report = {
    slides: slides.length,
    renderedSections: check.secs,
    tocButtons: 36,
    bytes: Buffer.byteLength(html, 'utf8'),
    maxBodyLines: Math.max.apply(null, slides.map((s) => (s.lines || []).length)),
    slidesWithExternalLinks: check.linked,
    inlineSvg: check.openSvg,
    tables: check.openTbl,
    cycleLedger: ledger ? 'present' : 'absent (계획/진행 표기)',
    campaignMinutes,
    beats: beatCount,
    clues: clueCount,
    fastMinutesSum: fastSum,
    deliberateMinutesSum: deliberateSum,
    lawsFromBible: LAWS.length,
    sliceT0: {
      id: firstStage.id,
      minutes: firstStage.minutes,
      zoneIds: sliceZoneIds,
      tools: sliceTools,
      proofBeats: sliceProofBeats,
      hintTiers: sliceHintTiers,
      excludedZones: otherZoneCount,
    },
    uiContract: {
      screens: uiScreens,
      player_decisions: uiDecisions,
      data_bindings: uiBindings,
      verificationMatrixRows: uiMatrixRows,
    },
    baseDays,
    totalDaysWithContingency: totalDays,
    laborCostKRW: laborCost,
    defaultNetPerUnit: Math.round(defNet * 100) / 100,
    defaultBreakevenCash: defBeCash,
    defaultBreakevenLabor: defBeLabor,
    errors: check.errs,
    warnings: check.warns,
    outputs: outs,
  };
  process.stdout.write(JSON.stringify(report, null, 2) + '\n');
  if (check.errs.length) process.exitCode = 1;
}

main();
