/*
 * build-prototype.mjs — model.mjs 를 그대로 삽입한 단일 HTML 산출물 생성기
 *
 * 실행: node build-prototype.mjs --out <절대 출력 경로>
 * 세션·사용자 경로를 소스에 고정하지 않는다.
 *
 * 규칙:
 *  - 네트워크 접근 0회, 설치 0회. Node 표준 라이브러리만 쓴다.
 *  - 브라우저 쪽 판정 로직을 새로 쓰지 않는다. model.mjs 원문을 바이트 그대로 인라인한다.
 *  - 외부 CDN·폰트·이미지 참조 0개. 생성 후 자체 검사한다.
 */

import { readFile, writeFile, mkdir } from 'node:fs/promises';
import { createHash } from 'node:crypto';
import { fileURLToPath } from 'node:url';
import path from 'node:path';

const HERE = path.dirname(fileURLToPath(import.meta.url));
const USAGE = 'Usage: node build-prototype.mjs --out <absolute-output.html>'; 

/* ------------------------------------------------------------------ *
 * 화면 코드 — 함수로 작성하고 toString() 으로 삽입한다.
 * 인라인된 model.mjs 의 최상위 식별자를 그대로 참조한다.
 * ------------------------------------------------------------------ */
function view() {
  const $ = (id) => document.getElementById(id);
  const esc = (s) => String(s).replace(/[&<>"']/g, (c) => ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c]));

  let session = initialSession();
  let message = { ok: true, text: '작업대 준비. 1단계 판독부터 시작한다.' };
  let pendingOffset = session.core.offsetMin;

  const badge = (ok, okText, noText) =>
    `<span class="badge ${ok ? 'good' : 'stop'}">${ok ? '●' : '▲'} ${esc(ok ? okText : noText)}</span>`;

  function apply(action) {
    const r = sessionApply(session, action);
    session = r.session;
    if (r.ok) {
      message = { ok: true, text: `적용됨 — ${describeAction(action)}` };
    } else {
      let t = `차단됨 — ${r.reason}`;
      if (r.recovery) t += ` 복구 선택지: ${r.recovery.filter((o) => o.available).map((o) => o.label).join(' / ')}`;
      message = { ok: false, text: t };
    }
    pendingOffset = session.core.offsetMin;
    render();
  }

  /* ---- 각 단계 ---- */

  function stepHead(n) {
    const s = STEPS[n - 1];
    return `<h2><span class="num">${s.n}단계</span> ${esc(s.title)}</h2>
      <p class="asks">묻는 것: ${esc(s.asks)}</p>`;
  }

  function step1(c) {
    const items = MEDIA.map((m) => {
      const on = c.copies.includes(m.id);
      return `<button type="button" class="toggle" data-act="read" data-arg="${m.id}" aria-pressed="${on}">
        <span class="tick">${on ? '판독함' : '미판독'}</span>
        <span class="tl">${esc(m.label)}</span>
        <span class="sub">매체 ${esc(m.mediumLabel)} · 출처 ${esc(m.originLabel)}</span>
      </button>`;
    }).join('');
    const clues = c.keptClues.length
      ? c.keptClues.map((id) => {
        const m = MEDIA.find((x) => x.clueId === id);
        const must = MANDATORY_CLUE_IDS.includes(id);
        return `<li>${esc(m ? m.clueLabel : id)} <span class="badge ${must ? 'good' : 'flat'}">${must ? '● 필수 · 보존됨' : '○ 참고'}</span></li>`;
      }).join('')
      : '<li class="muted">아직 보존된 단서가 없다.</li>';
    return `${stepHead(1)}
      <div class="grid5">${items}</div>
      <h3>보존된 단서 (되돌림·초기화로 사라지지 않음)</h3>
      <ul class="clues">${clues}</ul>`;
  }

  function step2(c) {
    const rows = MEDIA.map((m) => `<tr>
        <th scope="row">${esc(m.label)}</th>
        <td>${esc(m.mediumLabel)}</td>
        <td>${c.wiringChecked ? badge(m.wired, '배선 안', '배선 밖') : '<span class="badge flat">○ 미확인</span>'}</td>
      </tr>`).join('');
    return `${stepHead(2)}
      <p><button type="button" class="primary" data-act="checkWiring" aria-pressed="${c.wiringChecked}">배선 범위 확인${c.wiringChecked ? ' (완료)' : ''}</button></p>
      <table><caption class="sr-only">매체별 배선 범위</caption>
        <thead><tr><th scope="col">매체</th><th scope="col">종류</th><th scope="col">센서 범위</th></tr></thead>
        <tbody>${rows}</tbody></table>
      <p class="note">배선 범위 밖 근거는 단서로 남지만 6단계 서명의 슬롯을 채우지 못한다.</p>`;
  }

  function step3(c) {
    const st = alignmentStatus(c);
    const peaks = PEAKS.map((p) => {
      const on = c.peaks.includes(p.id);
      const d = p.ledgerMin - p.plateMin;
      return `<button type="button" class="toggle" data-act="togglePeak" data-arg="${p.id}" aria-pressed="${on}">
        <span class="tick">${on ? '물림' : '해제'}</span>
        <span class="tl">${esc(p.label)}</span>
        <span class="sub">편차 ${d}분</span>
      </button>`;
    }).join('');
    const res = st.residualMin;
    return `${stepHead(3)}
      <div class="grid4">${peaks}</div>
      <div class="rangeRow">
        <label for="offset">시간축 오프셋 (분) — 방향키로 1분씩 조절</label>
        <input type="range" id="offset" min="${LIMITS.offsetMin}" max="${LIMITS.offsetMax}" step="1"
               value="${c.offsetMin}" aria-describedby="offsetOut"
               ${c.aligned ? 'aria-disabled="true"' : ''}>
        <output id="offsetOut">${c.offsetMin}분 · 잔차 ${res === null ? '—' : res + '분'} / 한도 ${LIMITS.residualLimitMin}분</output>
        <button type="button" data-act="autoFit">자동 맞춤</button>
      </div>
      <p class="statline">피크 ${c.peaks.length}/${LIMITS.requiredPeaks}개 · 잔차 ${res === null ? '—' : res + '분'} ·
        ${badge(st.ok, '기준선 확정 가능', '기준선 확정 불가')}</p>
      ${st.ok ? '' : `<p class="why">확정할 수 없는 이유: ${esc(st.reason)}</p>`}
      <p>
        <button type="button" class="primary" data-act="pinAlignment">기준선 확정(고정핀)</button>
        <button type="button" data-act="unpinAlignment">고정핀 뽑기</button>
        ${c.aligned ? '<span class="badge good">● 확정됨</span>' : '<span class="badge flat">○ 미확정</span>'}
      </p>`;
  }

  function step4(c) {
    const st = orderStatus(c);
    let body;
    if (!st.ok) {
      body = `<p class="why">판정할 수 없는 이유: ${esc(st.reason)}</p>`;
    } else {
      body = `<table><caption class="sr-only">사건 쌍 선후 판정</caption>
        <thead><tr><th scope="col">사건 쌍</th><th scope="col">간격</th><th scope="col">오차폭</th><th scope="col">판정</th></tr></thead>
        <tbody>${st.verdicts.map((v) => `<tr>
          <th scope="row">${esc(v.label)}</th><td>${v.gapMin}분</td><td>${LIMITS.combinedUncertaintyMin}분</td>
          <td>${v.verdict === null ? '<span class="badge flat">○ 미판정</span>'
        : v.verdict === 'ordered' ? '<span class="badge good">● 선후 확정</span>'
          : '<span class="badge warn">◆ 판정 불가(unknown)</span>'}</td></tr>`).join('')}</tbody></table>`;
    }
    return `${stepHead(4)}
      <p><button type="button" class="primary" data-act="judgeOrder" aria-pressed="${c.orderJudged}">선후 판정 실행</button></p>
      ${body}
      <p class="note">판정 불가는 사건이 없었다는 뜻이 아니라 이 자료로 순서를 말할 수 없다는 뜻이다. 이후 단계를 막지 않는다.</p>`;
  }

  function step5(c) {
    const st = commitStatus(c);
    const opts = ROUTES.map((r, i) => {
      const sel = c.route === r.id;
      const over = r.corrosion > LIMITS.corrosionLimit;
      return `<button type="button" role="radio" aria-checked="${sel}" tabindex="${sel || (!c.route && i === 0) ? 0 : -1}"
        class="radio" data-act="selectRoute" data-arg="${r.id}">
        <span class="tick">${sel ? '선택됨' : '선택 안 됨'}</span>
        <span class="tl">${esc(r.label)}</span>
        <span class="sub">부식 ${r.corrosion} / 한도 ${LIMITS.corrosionLimit} ${over ? '· 한도 초과안' : ''}</span>
      </button>`;
    }).join('');
    const prev = c.route
      ? `<div class="preview"><h3>프리뷰</h3><ul>
          <li>보호 대상: ${esc(routeById(c.route).protects === 'lowland' ? '저지대 주거지' : '부두 냉동창고')}</li>
          <li>부식 비용: ${routeById(c.route).corrosion} / 한도 ${LIMITS.corrosionLimit}</li>
          <li>되돌릴 수 있는가: 확정 전 무제한 · 확정 후에는 되돌림으로만</li>
          <li>보존되는 것: 판독 사본과 필수 단서 전부</li>
          <li>엔딩 접근성: 변하지 않음(3종 유지)</li>
        </ul></div>`
      : '';
    return `${stepHead(5)}
      <div role="radiogroup" aria-label="경로 선택" class="grid3 rg" id="routeGroup">${opts}</div>
      <p>
        <button type="button" data-act="preview" aria-pressed="${c.previewed}">프리뷰 보기</button>
        <button type="button" class="primary" data-act="commitRoute">확정</button>
        ${c.committed ? '<span class="badge good">● 확정됨 · 체크포인트 생성</span>' : '<span class="badge flat">○ 미확정</span>'}
      </p>
      ${c.previewed ? prev : ''}
      ${st.ok ? '<p class="statline">' + badge(true, '확정 가능', '') + '</p>'
        : `<p class="why">확정할 수 없는 이유: ${esc(st.reason)}</p>`}
      <p class="statline">재산 보호 플래그: ${c.propertyProtection === null ? '없음'
        : esc(c.propertyProtection === 'lowland' ? '저지대' : '부두')} · 엔딩 접근성 영향 없음</p>`;
  }

  function step6(c) {
    const st = sealStatus(c);
    const slots = MEDIA.map((m) => {
      const on = c.seal.includes(m.id);
      return `<button type="button" class="toggle" data-act="toggleSeal" data-arg="${m.id}" aria-pressed="${on}">
        <span class="tick">${on ? '슬롯에 있음' : '슬롯 밖'}</span>
        <span class="tl">${esc(m.label)}</span>
        <span class="sub">${esc(m.mediumLabel)} · ${esc(m.originLabel)}${m.wired ? '' : ' · 배선 밖'}</span>
      </button>`;
    }).join('');
    const eop = ENDINGS.map((e, i) => {
      const sel = c.ending === e.id;
      return `<button type="button" role="radio" aria-checked="${sel}" tabindex="${sel || (!c.ending && i === 0) ? 0 : -1}"
        class="radio" data-act="selectEnding" data-arg="${e.id}">
        <span class="tick">${sel ? '제출 관점' : '선택 안 됨'}</span><span class="tl">${esc(e.label)}</span></button>`;
    }).join('');
    return `${stepHead(6)}
      <h3>근거 슬롯 ${c.seal.length}/${LIMITS.sealSlots}</h3>
      <div class="grid5">${slots}</div>
      <p>
        <button type="button" class="primary" data-act="sign">이중서명</button>
        <button type="button" data-act="cancelSign">서명 취소</button>
        ${c.signed ? '<span class="badge good">● 서명됨</span>' : '<span class="badge flat">○ 미서명</span>'}
      </p>
      ${st.ok ? '<p class="statline">' + badge(true, '서명 가능', '') + '</p>'
        : `<p class="why">서명할 수 없는 이유: ${esc(st.reason)}</p>`}
      <h3>제출 관점 — 선택 가능 ${selectableEndings(c).length}종 / 전체 ${ENDINGS.length}종</h3>
      <div role="radiogroup" aria-label="제출 관점 선택" class="grid3 rg" id="endingGroup">${eop}</div>
      <p class="note">재산 보호 플래그가 어떤 값이든 세 관점은 잠기지 않는다. 재시작 없이 바꿔 다시 제출할 수 있다.</p>`;
  }

  function statusPanel(c) {
    const s = summary(c);
    const rows = [
      ['판독 사본', `${s.copies}점`],
      ['보존 필수 단서', `${s.mandatoryKept} / ${s.mandatoryTotal}개`],
      ['배선 확인', s.wiringChecked ? '완료' : '미완료'],
      ['공통 피크', `${s.peakCount} / ${LIMITS.requiredPeaks}개`],
      ['잔차', s.residualMin === null ? '—' : `${s.residualMin}분 / 한도 ${LIMITS.residualLimitMin}분`],
      ['기준선', s.aligned ? '확정' : '미확정'],
      ['선후 판정', s.orderJudged ? '실행됨' : '미실행'],
      ['경로 확정', s.committed ? `확정(${s.route})` : '미확정'],
      ['재산 보호 플래그', s.propertyProtection === null ? '없음' : s.propertyProtection],
      ['이중서명', s.signed ? '서명됨' : '미서명'],
      ['선택 가능 엔딩', `${s.selectableEndings}종`],
      ['제출 관점', s.ending === null ? '미선택' : s.ending],
      ['메모리 자동 저장', session.autosave ? '있음(확정/서명 직전)' : '없음'],
      ['되돌림 스택', `${session.past.length}단계`],
      ['저장 슬롯', session.slot ? `있음 · checksum ${session.slot.checksum}` : '비어 있음'],
    ];
    return `<h2>상태판</h2><table class="status"><caption class="sr-only">현재 모형 상태</caption><tbody>
      ${rows.map(([k, v]) => `<tr><th scope="row">${esc(k)}</th><td>${esc(v)}</td></tr>`).join('')}
    </tbody></table>`;
  }

  const RELATIONS = [
    ['1 → 3', '판독한 표준 염판과 조위대장이 있어야 정합을 시작한다'],
    ['1 → 6', '판독하지 않은 매체는 근거 슬롯에 들어가지 않는다'],
    ['2 → 6', '배선 범위를 확인해야 범위 밖 근거를 걸러 서명할 수 있다'],
    ['3 → 4', '기준선이 확정돼야 선후 판정이 실행된다'],
    ['3 → 6', '시간 근거이므로 잔차 4분 이하 기준선이 서명 조건이다'],
    ['5 ∥ 나머지', '경로 확정은 시간 판정과 독립이다. 서로를 막지 않는다'],
  ];

  function render() {
    const c = session.core;
    const active = document.activeElement ? document.activeElement.id : null;
    $('step1').innerHTML = step1(c);
    $('step2').innerHTML = step2(c);
    $('step3').innerHTML = step3(c);
    $('step4').innerHTML = step4(c);
    $('step5').innerHTML = step5(c);
    $('step6').innerHTML = step6(c);
    $('status').innerHTML = statusPanel(c);
    $('msg').textContent = message.text;
    $('msg').className = message.ok ? 'msg good' : 'msg stop';
    if (active) { const el = document.getElementById(active); if (el) el.focus(); }
  }

  /* ---- 입력 ---- */

  document.addEventListener('click', (ev) => {
    const el = ev.target.closest('[data-act]');
    if (!el) return;
    const type = el.getAttribute('data-act');
    const arg = el.getAttribute('data-arg');
    const map = {
      read: () => ({ type: 'read', mediaId: arg }),
      togglePeak: () => ({ type: 'togglePeak', peakId: arg }),
      selectRoute: () => ({ type: 'selectRoute', routeId: arg }),
      toggleSeal: () => ({ type: 'toggleSeal', mediaId: arg }),
      selectEnding: () => ({ type: 'selectEnding', endingId: arg }),
      load: () => ({ type: 'load', payload: demoPayload(arg) }),
    };
    apply(map[type] ? map[type]() : { type });
  });

  function demoPayload(kind) {
    if (kind === 'corrupt') {
      if (!session.slot) return { nope: true };
      const t = JSON.parse(JSON.stringify(session.slot));
      t.data.committed = !t.data.committed;
      return t;
    }
    if (kind === 'newer') {
      const t = serialize(session.core); t.schemaVersion = SCHEMA_VERSION + 1; return t;
    }
    return session.slot;
  }

  document.addEventListener('input', (ev) => {
    if (ev.target.id !== 'offset') return;
    const v = Number(ev.target.value);
    const out = $('offsetOut');
    const res = residualFor(session.core.peaks, v);
    if (out) out.textContent = `${v}분 · 잔차 ${res === null ? '—' : res + '분'} / 한도 ${LIMITS.residualLimitMin}분`;
  });
  document.addEventListener('change', (ev) => {
    if (ev.target.id !== 'offset') return;
    apply({ type: 'setOffset', offsetMin: Number(ev.target.value) });
  });

  // 라디오 그룹: 방향키 순회
  document.addEventListener('keydown', (ev) => {
    const group = ev.target.closest ? ev.target.closest('[role="radiogroup"]') : null;
    if (!group) return;
    const keys = ['ArrowRight', 'ArrowDown', 'ArrowLeft', 'ArrowUp'];
    if (!keys.includes(ev.key)) return;
    const items = [...group.querySelectorAll('[role="radio"]')];
    const i = items.indexOf(ev.target);
    if (i < 0) return;
    ev.preventDefault();
    const dir = (ev.key === 'ArrowRight' || ev.key === 'ArrowDown') ? 1 : -1;
    const next = items[(i + dir + items.length) % items.length];
    next.focus();
    next.click();
  });

  render();
}

/* ------------------------------------------------------------------ *
 * 스타일 — 다크 · 시폼. 색만으로 상태를 전달하지 않는다(항상 문자 라벨 동반).
 * ------------------------------------------------------------------ */
const CSS = `
:root{
  --bg:#0b1416; --panel:#122024; --line:#28454a; --ink:#e6f2ef; --muted:#9fb8b3;
  --seafoam:#86dcc6; --seafoam-ink:#04211c; --stop:#ffb3a7; --warn:#ffd79a; --focus:#bff3e4;
}
*{box-sizing:border-box}
html{color-scheme:dark}
body{margin:0;background:var(--bg);color:var(--ink);
  font-family:"Pretendard","Apple SD Gothic Neo","Noto Sans KR","Malgun Gothic",system-ui,sans-serif;
  font-size:16px;line-height:1.7;-webkit-text-size-adjust:100%}
.sr-only{position:absolute;width:1px;height:1px;padding:0;margin:-1px;overflow:hidden;clip:rect(0 0 0 0);white-space:nowrap;border:0}
#banner{position:sticky;top:0;z-index:10;background:var(--seafoam);color:var(--seafoam-ink);
  font-weight:700;padding:.7rem 1rem;border-bottom:3px solid #4fbfa3;letter-spacing:-.01em}
header.head{padding:1.4rem 1rem .4rem;max-width:1180px;margin:0 auto}
header.head h1{font-size:1.45rem;margin:0 0 .35rem}
header.head p{margin:.2rem 0;color:var(--muted);font-size:.95rem}
main{max-width:1180px;margin:0 auto;padding:1rem;display:grid;grid-template-columns:minmax(0,1fr) 320px;gap:1.2rem;align-items:start}
@media (max-width:900px){main{grid-template-columns:1fr}}
section.step,aside.side{background:var(--panel);border:1px solid var(--line);border-radius:12px;padding:1rem 1.1rem;margin:0 0 1.1rem}
aside.side{position:sticky;top:4.2rem}
h2{font-size:1.12rem;margin:.1rem 0 .3rem;display:flex;gap:.5rem;align-items:baseline;flex-wrap:wrap}
h3{font-size:.98rem;margin:1rem 0 .4rem;color:var(--seafoam)}
.num{background:#1d3439;border:1px solid var(--line);color:var(--seafoam);border-radius:999px;padding:.05rem .6rem;font-size:.82rem}
.asks{margin:.1rem 0 .8rem;color:var(--muted);font-size:.9rem}
.note{color:var(--muted);font-size:.88rem;margin:.7rem 0 0}
.statline{margin:.6rem 0 .2rem}
.why{margin:.5rem 0;padding:.55rem .7rem;border-left:4px solid var(--stop);background:#241a19;color:var(--ink);border-radius:0 8px 8px 0}
.grid5,.grid4,.grid3{display:grid;gap:.55rem}
.grid5{grid-template-columns:repeat(auto-fit,minmax(190px,1fr))}
.grid4{grid-template-columns:repeat(auto-fit,minmax(170px,1fr))}
.grid3{grid-template-columns:repeat(auto-fit,minmax(200px,1fr))}
button{font:inherit;color:var(--ink);background:#17282d;border:1px solid var(--line);border-radius:9px;
  padding:.5rem .8rem;cursor:pointer;text-align:left}
button:hover{border-color:var(--seafoam)}
button.primary{background:var(--seafoam);color:var(--seafoam-ink);border-color:var(--seafoam);font-weight:700}
button.toggle,button.radio{display:flex;flex-direction:column;gap:.12rem;width:100%}
button[aria-pressed="true"],button[aria-checked="true"]{border-color:var(--seafoam);background:#16332e;box-shadow:inset 3px 0 0 var(--seafoam)}
.tick{font-size:.84rem;color:var(--seafoam);font-weight:700;letter-spacing:.02em}
button[aria-pressed="false"] .tick,button[aria-checked="false"] .tick{color:var(--muted);font-weight:400}
.tl{font-weight:600}
.sub{font-size:.85rem;color:var(--muted)}
:focus-visible{outline:3px solid var(--focus);outline-offset:2px}
table{border-collapse:collapse;width:100%;margin:.5rem 0;font-size:.92rem}
th,td{border-bottom:1px solid var(--line);padding:.4rem .5rem;text-align:left;vertical-align:top}
thead th{color:var(--seafoam);font-size:.85rem}
table.status th{width:46%;color:var(--muted);font-weight:500}
.badge{display:inline-block;padding:.05rem .5rem;border-radius:999px;font-size:.85rem;border:1px solid currentColor;white-space:nowrap}
.badge.good{color:var(--seafoam)}
.badge.stop{color:var(--stop)}
.badge.warn{color:var(--warn)}
.badge.flat{color:var(--muted)}
.clues{margin:.3rem 0;padding-left:1.1rem}
.muted{color:var(--muted)}
.rangeRow{display:grid;gap:.35rem;margin:.9rem 0;padding:.7rem;background:#0f1d21;border:1px solid var(--line);border-radius:9px}
.rangeRow label{font-size:.9rem}
input[type=range]{width:100%;accent-color:var(--seafoam);height:1.9rem}
output{font-variant-numeric:tabular-nums;color:var(--seafoam);font-size:.92rem}
.preview{margin:.6rem 0;padding:.6rem .8rem;background:#0f1d21;border:1px dashed var(--line);border-radius:9px}
.preview ul{margin:.3rem 0;padding-left:1.1rem}
.msg{margin:0;padding:.6rem .8rem;border-radius:9px;border:1px solid var(--line);font-size:.93rem}
.msg.good{color:var(--seafoam)}
.msg.stop{color:var(--stop);border-color:var(--stop)}
.controls{display:flex;flex-wrap:wrap;gap:.45rem;margin:.6rem 0}
footer{max-width:1180px;margin:0 auto;padding:0 1rem 3rem;color:var(--muted);font-size:.88rem}
footer h2{color:var(--ink);font-size:1rem}
footer ul{padding-left:1.1rem}
@media (prefers-reduced-motion:reduce){*{transition:none!important;animation:none!important}}
`;

/* ------------------------------------------------------------------ */

function buildHtml(modelSource) {
  const bannerText = '상호작용 검증 모형 · Unity 빌드 아님 · 플레이시간 미측정';
  return `<!DOCTYPE html>
<html lang="ko">
<head>
<meta charset="utf-8">
<meta name="viewport" content="width=device-width, initial-scale=1">
<title>상호작용 검증 모형 — 훈련용 작업대</title>
<meta name="description" content="interaction-rules.md 의 확정 조건이 서로 모순 없이 성립하는지 확인하는 실행 가능한 참조 모형. 게임 빌드가 아니다.">
<meta name="color-scheme" content="dark">
<style>${CSS}</style>
</head>
<body>
<p id="banner" role="note">${bannerText}</p>

<header class="head">
  <h1>상호작용 검증 모형 — 가상의 훈련용 작업대 1개</h1>
  <p>이 페이지는 <code>_workspace/current/systems/interaction-rules.md</code> 의 확정 조건이 서로 모순 없이 성립하는지 손으로 확인하는 참조 모형이다. 게임도, 데모도, 세로 슬라이스도 아니다.</p>
  <p>판정 로직은 <code>model.mjs</code> 원문을 그대로 인라인한 것이며, Node 테스트와 정확히 같은 순수 리듀서를 쓴다. 외부 요청 0개.</p>
  <p id="msg" class="msg good" role="status" aria-live="polite">준비됨</p>
  <div class="controls">
    <button type="button" data-act="undo">되돌림</button>
    <button type="button" data-act="reset">초기화(단서 보존)</button>
    <button type="button" data-act="save">저장</button>
    <button type="button" data-act="load" data-arg="ok">불러오기</button>
    <button type="button" data-act="load" data-arg="corrupt">손상 파일 불러오기(시연)</button>
    <button type="button" data-act="load" data-arg="newer">상위 스키마 불러오기(시연)</button>
    <button type="button" data-act="restoreAutosave">자동 저장 복구</button>
  </div>
</header>

<main>
  <div>
    <section class="step" id="step1" aria-labelledby="h1s"></section>
    <section class="step" id="step2"></section>
    <section class="step" id="step3"></section>
    <section class="step" id="step4"></section>
    <section class="step" id="step5"></section>
    <section class="step" id="step6"></section>
  </div>
  <aside class="side" id="status" aria-label="상태판"></aside>
</main>

<footer>
  <h2>단계 사이의 관계</h2>
  <table>
    <caption class="sr-only">단계 의존 관계</caption>
    <thead><tr><th scope="col">관계</th><th scope="col">이유</th></tr></thead>
    <tbody>
      <tr><th scope="row">1 → 3</th><td>판독한 표준 염판과 조위대장이 있어야 정합을 시작한다</td></tr>
      <tr><th scope="row">1 → 6</th><td>판독하지 않은 매체는 근거 슬롯에 들어가지 않는다</td></tr>
      <tr><th scope="row">2 → 6</th><td>배선 범위를 확인해야 범위 밖 근거를 걸러 서명할 수 있다</td></tr>
      <tr><th scope="row">3 → 4</th><td>기준선이 확정돼야 선후 판정이 실행된다</td></tr>
      <tr><th scope="row">3 → 6</th><td>시간 근거이므로 잔차 4분 이하 기준선이 서명 조건이다</td></tr>
      <tr><th scope="row">5 · 독립</th><td>경로 확정은 시간 판정과 독립이며 서로를 막지 않는다</td></tr>
    </tbody>
  </table>

  <h2>이 모형이 주장하지 않는 것</h2>
  <ul>
    <li>게임·데모·Unity 빌드가 존재한다는 주장 없음. 빌드 0건.</li>
    <li>퍼즐 난이도나 콘텐츠 깊이에 대한 주장 없음.</li>
    <li>플레이 시간에 대한 주장 없음 — 측정 0건.</li>
    <li>캠페인 전체의 진행 불가 문제가 해결됐다는 주장 없음. 이 모형은 훈련용 작업대 1개만 덮는다.</li>
    <li>접근성·성능 실사용 검증 없음. 브라우저 검수는 별도 담당이다.</li>
    <li>비용·수익 모형은 이 산출물과 완전히 별개다. 여기에는 어떤 금액도 없다.</li>
  </ul>
  <p>조작: <kbd>Tab</kbd> 이동 · 경로/관점은 <kbd>←</kbd><kbd>→</kbd><kbd>↑</kbd><kbd>↓</kbd> 순회 · 오프셋 슬라이더는 방향키 1분 단위 · <kbd>Enter</kbd>/<kbd>Space</kbd> 실행. 대화상자·자동 재생 없음.</p>
</footer>

<script type="module">
${modelSource}

/* ---- 화면 계층 (판정 로직 없음, 위 모형만 호출한다) ---- */
(${view.toString()})();
</script>
</body>
</html>
`;
}

/* ------------------------------------------------------------------ */

function selfCheck(html) {
  const problems = [];
  if (!html.includes('상호작용 검증 모형 · Unity 빌드 아님 · 플레이시간 미측정')) problems.push('상시 배너 문구 누락');
  if (/<script[^>]+src=/i.test(html)) problems.push('외부 script src 발견');
  if (/<link[^>]+rel=["']?stylesheet/i.test(html)) problems.push('외부 스타일시트 링크 발견');
  if (/<img|<iframe|<video|<audio/i.test(html)) problems.push('외부 미디어 요소 발견');
  if (/@import/i.test(html)) problems.push('CSS @import 발견');
  if (/url\(\s*['"]?https?:/i.test(html)) problems.push('CSS 원격 url() 발견');
  if (/\b(?:https?:)?\/\/(?!\s)/.test(html.replace(/\/\/[^\n]*/g, (m, o) => (/https?:\/\//.test(m) ? m : '')))) {
    if (/https?:\/\//.test(html)) problems.push('절대 URL 문자열 발견');
  }
  if (/\b(fetch|XMLHttpRequest|importScripts|WebSocket)\s*\(/.test(html)) problems.push('네트워크 API 호출 발견');
  if (/\b(alert|confirm|prompt)\s*\(/.test(html)) problems.push('대화상자 호출 발견');
  if (/autoplay/i.test(html)) problems.push('autoplay 속성 발견');
  if (!/lang="ko"/.test(html)) problems.push('lang 속성 누락');
  return problems;
}

async function main() {
  const argv = process.argv.slice(2);
  const outIdx = argv.indexOf('--out');
  const out = outIdx >= 0 ? argv[outIdx + 1] : null;
  if (!out || !path.isAbsolute(out)) throw new Error(USAGE);

  const modelPath = path.join(HERE, 'model.mjs');
  const modelSource = await readFile(modelPath, 'utf8');
  const modelHash = createHash('sha256').update(modelSource).digest('hex');

  const html = buildHtml(modelSource);
  const problems = selfCheck(html);
  if (problems.length || !html.includes(modelSource)) throw new Error(problems.join('; ') || 'Model byte mismatch');

  await mkdir(path.dirname(out), { recursive: true });
  await writeFile(out, html, 'utf8');
  const htmlHash = createHash('sha256').update(html).digest('hex');

  console.log('생성 완료');
  console.log(`  출력           ${out}`);
  console.log(`  크기           ${Buffer.byteLength(html, 'utf8').toLocaleString('en-US')} bytes`);
  console.log(`  html sha256    ${htmlHash}`);
  console.log(`  model.mjs      ${modelPath}`);
  console.log(`  model sha256   ${modelHash}`);
  console.log(`  모형 인라인     ${html.includes(modelSource) ? '원문 바이트 일치' : '불일치(결함)'}`);
  console.log(`  자체 검사       ${problems.length === 0 ? '통과 — 외부 참조·네트워크·대화상자·자동재생 0건' : '실패'}`);
  for (const p of problems) console.log(`    - ${p}`);
  if (problems.length > 0 || !html.includes(modelSource)) process.exit(1);
}

main().catch((err) => { console.error(err); process.exit(1); });
