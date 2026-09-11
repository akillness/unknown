---
updated: 2026-09-10
cycle: 20260909-preproduction-c3
status: current
supersedes: null
owner: game-systems-designer
---

# 기술 검증 — C3 수정 루프 2 (RFC-P3-013 / RFC-P3-014 정본 정렬)

대상 결함: **C3-F3(S1)** · **C3-F26(S2)** + 같은 계열 자체 발견 **S-1 · S-2**(§3-1). 수정 파일 8개. 이 문서는 문서 정합 재측정 영수증이다.
**Unity 실행·빌드·테스트 0회, 사람 플레이 표본 n=0.** 여기의 어떤 값도 게임 측정치가 아니다.

## 0. 정본 근거 (읽은 것)

| 정본 | 경로 | 확인한 것 |
|---|---|---|
| RFC-P3-014 | `production/decision-log.md` L58~L61 | 6법 호명 문구 = 아카이브 c3 `worldview-bible.md` §3 표 하나뿐 [OBSERVED] |
| RFC-P3-013 | `production/decision-log.md` L53~L56 | 밸브 개폐 H-1:24 · 봉인 완료 접점 H-1:04(간격 20분 > 오차폭 8분) · 침수 H+0:12 확정 [OBSERVED] |
| 6법 정본 표 | `worldview/worldview-bible.md` L44~L49 (§3) | 아카이브 c3 §3 표와 호명 문구 6/6 문자 일치 [OBSERVED] |
| 순서 앵커 근거 | `worldview/timeline.md` L35 · §8 (L153~L161) | "정합 후 잔차 관측소별 ±4분 → 총 오차폭 8분. 간격 20분 > 8분" [OBSERVED] |
| 폐기 시각 기록 | `worldview/timeline.md` L50 | 폐기 시각 2종의 정본 보존 위치. 이 스펙·검증 파일은 그 문자열을 재기입하지 않고 위치만 인용한다 [OBSERVED] |
| 폐기 호명 문구 기록 | `worldview/consistency-audit.md` **§4-1** (L94~) | 폐기 6법 문구의 정본 보존 위치. §4-2는 폐기된 *장치·전제*이며 법 문구가 아니다 [OBSERVED] |

## 1. 재측정 — 명령과 관측치

명령은 `qa/gate-measurements.md` L58~L60의 검증 명령을 그대로 쓴다. 다만 **이 검증 파일에 폐기 문구를 다시 적으면 그 자체가 재유입**이므로, 패턴은 정본 검증 명령 줄에서 읽어 쓴다.

```sh
# (1) 폐기 6법 호명 문구 잔존 — 패턴 원문은 qa/gate-measurements.md L60
PAT=$(sed -n '60p' _workspace/current/qa/gate-measurements.md | sed 's/.*grep -rn "//; s/" _workspace\/current.*//')
grep -rnE "$PAT" _workspace/current/systems | wc -l          # systems 레인
grep -rnE "$PAT" _workspace/current | cut -d: -f1 | sort | uniq -c   # 전체 파일별

# (2) 폐기 시각 잔존 — 문자열 원문은 worldview/timeline.md L50 (폐기 기록).
#     이 파일에 폐기 시각을 재기입하지 않으려고 거기서 읽어 쓴다.
BAD=$(sed -n '50p' _workspace/current/worldview/timeline.md | grep -o 'H-1:[0-9][0-9]')
grep -rn "$BAD" _workspace/current/systems | wc -l

# (3) 9행 호명 6종
for f in wiring-trace plate-readout tide-alignment drainage-routing corrosion-budget dual-seal; do
  sed -n '9p' _workspace/current/systems/system-specs/$f.md
done

# (4) 데이터 회귀
node _workspace/current/planning/validate-campaign.mjs
```

| 측정 | 수정 전 [OBSERVED 2026-09-10] | 수정 후 [OBSERVED 2026-09-10] |
|---|---|---|
| 폐기 6법 문구 — **systems 레인** | **6** = `system-specs` 5파일 L9 + `interaction-rules.md` L101 | **0** |
| 폐기 6법 문구 — 워크스페이스 전체 | **19** (systems 6 · `planning/gdd.md` 출처주 1 · `qa/c3-review.md` 6 · `qa/gate-measurements.md` 검증 명령 1 · `worldview/consistency-audit.md` §4-1 5) | **13** — `gdd.md` 1 · `c3-review.md` 6 · `gate-measurements.md` 1 · `consistency-audit.md` 5. **전부 기록·검증 명령이고 본문 사용 0** (본 검증 파일은 문구를 재기입하지 않으므로 집계에서 자기참조가 없다) |
| 폐기 밸브 시각(`$BAD`, 문자열 원문 = `timeline.md` L50) — systems 레인 | **1** = `tide-alignment.md` L54 A-R3 (전 워크스페이스 유일한 본문 사용) | **0** |
| 같은 시각 — 워크스페이스 **본문 사용** | **1** (위와 동일) | **0**. 남은 히트는 폐기 기록(`timeline.md` L50) · 검증기 금지 문자열(`validate-campaign.mjs` K-04) · 검토/감사 인용뿐이며, 본 검증 파일도 문자열을 재기입하지 않는다 |
| `validate-campaign.mjs` | 44/44 PASS | **44/44 PASS · fail 0 · exit 0**. 본 수정은 `campaign.json`을 건드리지 않으므로 회귀 없음 확인용 |

수정 후 system-specs 9행 제목 6/6 [OBSERVED]:

| 파일 | 9행 호명 | 정본 일치 |
|---|---|---|
| `wiring-trace.md` | 법1 "배선된 것만 남는다" | ✓ (수정 없음, 원래 일치) |
| `plate-readout.md` | 법2 "원본은 닳지만 사본은 남는다" | ✓ |
| `tide-alignment.md` | 법3 "정합 전 시계는 믿지 않는다" | ✓ |
| `drainage-routing.md` | 법4 "이번 조수에는 보호 용량이 부족하다" | ✓ |
| `corrosion-budget.md` | 법5 "소금은 비용으로 보인다" | ✓ |
| `dual-seal.md` | 법6 "원본 책임과 제출을 나눈다" | ✓ |

## 2. 수정 후 파일 해시

`shasum -a 256 <path>` [OBSERVED 2026-09-10]

| 파일 | sha256 | 바이트 |
|---|---|---|
| `systems/system-specs/plate-readout.md` | `bf761ddc0a95d3c9…` | 8215 |
| `systems/system-specs/tide-alignment.md` | `8ac81c856209507f…` | 7518 |
| `systems/system-specs/drainage-routing.md` | `c16f8ec5724ab79f…` | 8122 |
| `systems/system-specs/corrosion-budget.md` | `5ecb8fb0d2b55225…` | 11583 |
| `systems/system-specs/dual-seal.md` | `4fe111b602f9e46b…` | 6844 |
| `systems/interaction-rules.md` | `c5175cceaee2fec1…` | 16616 |
| `systems/data-schemas/plates.md` | `af697e1561918a1d…` | 6472 |
| `systems/system-specs/wiring-trace.md` (미수정 대조) | `3610f07584b00d67…` | — |
| `worldview/worldview-bible.md` (정본 입력, 미수정) | `3ffdcafa755c43de…` | — |

## 3. C3-F26을 어떻게 닫았는가 — 두 선택지 중 (b)

QA는 두 대안을 제시했다: (a) `서명(H-1:40) → 밸브(H-1:24)` 간격 16분, (b) `밸브(H-1:24) → 봉인 완료 접점(H-1:04)` 간격 20분.
**(b)를 채택했다.** 근거:

| 이유 | 내용 |
|---|---|
| C3-F25 비종속 | (a)는 `H-1:40`을 쓰므로 C3-F25 (a)/(b) 판정에 따라 다시 열린다. (b)는 RFC-P3-013이 **확정**한 두 시각만 써서 어느 판정에도 재작성 대상이 아니다 [INFERENCE] |
| 캐논 문자 일치 | `timeline.md` L35·L142(B26)·§8이 정확히 이 쌍과 "20분 > 8분"을 캐논 사례로 적는다 [OBSERVED] |
| 산술 보존 | (a)는 간격 20→16으로 바뀌어 A-R3·D-A2의 `20 > 8` 예시를 동시에 재작성해야 한다. (b)는 20분이 유지돼 D-A2 부등식이 그대로 성립 [OBSERVED] |
| 스펙 성격 | A-R3은 *선후 판정 규칙의 예시*다. 게임이 실제로 순서 앵커를 회수하는 비트는 `c6-b3`(B26)이며 그 비트가 쓰는 쌍이 곧 (b)다 [OBSERVED: `timeline.md` L142] |

부수 정정: A-R2 문구 "두 **관측소** 오차폭의 합" → "두 **근거** 각각의 오차폭 합". 새 캐논 사례가 같은 계통 로그의 두 각인이라 "관측소"라는 낱말이 규칙과 예시를 어긋나게 만들었다. 캐논이 같은 계통에도 총 오차폭 8분을 보수적으로 적용하는 근거를 규칙 안에 명시했다 [OBSERVED: `timeline.md` §8 "왜 확정되나"].

## 3-1. QA가 지목하지 않은 같은 계열 결함 2건 (본 레인 자체 발견)

`grep`을 결함이 인용한 5파일이 아니라 **레인 전체**로 돌려 같은 계열 2건을 더 찾았다. 둘 다 내 소유 파일이므로 함께 닫는다.

| # | 위치 | 무엇이 틀렸나 | 조치 |
|---|---|---|---|
| S-1 | `systems/interaction-rules.md` L101 | 본문이 법6을 폐기된 C2 문구로 호명. C3-F3 evidence는 `system-specs` 5행만 세어 **6번째 본문 사용을 놓쳤다**. QA의 `required_fix`가 "5줄 수정으로 닫힌다"고 적은 근거가 실제 잔존 수와 다르다 [OBSERVED] | 정본 문구로 교체 |
| S-2 | `systems/data-schemas/plates.md` L77 (§4 캐논 고정값) | `\| 서명 → 밸브 간격 \| 20분 \|`을 `[OBSERVED: worldview/timeline.md]` 표제 아래 둔다. RFC-P3-013 캐논에서 서명(H-1:40) → 밸브(H-1:24)는 **16분**이고, 20분은 밸브 → 봉인 완료 접점이다. C3-F26과 **같은 뿌리**(폐기 시각쌍이 남긴 잔여 산술)이며 QA 검토 범위 밖이었다 [OBSERVED] | 행을 "순서 앵커 간격 = 20분, 밸브 개폐 각인 → 봉인 완료 접점 각인"으로 교체하고 근거 줄 인용 추가 |

S-2를 고치지 않으면 `plates.md` §4가 A-R3과 **다른 쌍**으로 같은 20분을 주장해 C3-F26이 데이터 스키마 쪽에 그대로 남는다.

## 4. 재발 방지

수정한 5개 스펙(`plate-readout` · `tide-alignment` · `drainage-routing` · `corrosion-budget` · `dual-seal`)의 제목 바로 아래에 **법 호명 정본 인용주**를 넣었다(문구를 다시 쓰지 말고 `worldview-bible.md` §3을 인용하라는 지시 + 폐기 문구 보존 위치 `consistency-audit.md` §4-1). 정본이 또 바뀌면 스펙 5개가 아니라 이 한 줄이 가리키는 표만 보면 된다.
`tide-alignment.md` §3 끝에 **A-R3 이력** 주석을 남겨 폐기 사례가 무엇이었는지 추적 가능하게 했다. 폐기 시각 문자열 자체는 이 스펙에 다시 쓰지 않는다(검증기 금지 문자열 재유입 방지).

## 5. NOT-MEASURED (이 회차에서 재지 않은 것)

| 항목 | 상태 |
|---|---|
| Unity 에디터 실행 / 빌드 / 테스트 | **0회** |
| 정합 퍼즐 실제 플레이·잔차 조작 시간 | **n=0** |
| §7 성능 예산(2 ms / 4 ms / 20 ms / 200 ms) | 전부 `[TARGET]`, 프레임타임 캡처 0건 |
| `graphify update .` | **skipped** — 이번 수정은 소스코드 0줄, `_workspace` 마크다운 6개 파일만 변경. 코드 그래프 대상 없음 |
| `mex` 계열 | **skipped** — CLAUDE.md §10, 이 환경의 `mex`는 TeX 동명 바이너리이며 실행 금지 |

---

## 후속 (2026-09-10 R4) — 이 영수증을 그대로 읽으면 안 되는 두 지점 [OBSERVED]

이 문서는 **수정 루프 2 시점의 기록**이며 지우지 않는다. 다만 그 뒤 두 가지가 바뀌었으므로 현행 상태와 구분한다.

| 이 문서가 적은 것 | R4 현행 [OBSERVED 2026-09-10] | 바뀐 이유 |
|---|---|---|
| 인용 대상 = `worldview/consistency-audit.md` **§4-1** (3곳: 위 표·집계 행·마무리 문단) | 인용은 **절 번호를 쓰지 않는다.** 6종 스펙은 제목 문자열 「사용 금지 문구」 절 / 그 안의 표 「폐기된 6법 호명 문구」로 가리킨다 | **RFC-W3**(대상 소멸로 닫으며 "systems 스펙은 절 번호가 아니라 제목 문자열로 인용" 채택) · `qa/c3-review.md` §9.3 q-4 의 QA 대안 |
| "수정한 **5개** 스펙 … 제목 바로 아래에 법 호명 정본 인용주" | **6개**. `wiring-trace.md` 에 없던 인용주를 R4 에서 신설했다(법1은 두 계보가 동일해 폐기 문구가 없다는 사실을 함께 적음) | `consistency-audit.md` A33 이 "스펙 **6종**의 L11 이 인용" 이라고 적었는데 실측은 5종이었다 — 감사의 주장과 파일을 일치시켰다 |
| `validate-campaign.mjs` **44/44 PASS** · `campaign.json` `fdabf1d4…` | **47/47 PASS** · `92301c0a…`(121,457 B) | planner 가 비트 `zoneId` 를 넣어(C3-F22) `Z-01`·`Z-02`·`K-06` 3검사가 늘었다. 이 문서의 44/44 는 당시 값이며 현행은 `tech-verification/README.md` §0-2 |

재측정 명령과 결과:

```
$ grep -rn "§4-1" _workspace/current/systems/system-specs          # → 0행
$ grep -lc "사용 금지 문구" _workspace/current/systems/system-specs/*.md | wc -l   # → 6파일
```

이 문서 본문의 §4-1 표기 3곳은 **당시 기록**이므로 고치지 않는다. 현행 인용 규칙은 위 표가 정본이다.
