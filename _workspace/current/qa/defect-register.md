---
updated: 2026-09-10
cycle: 20260909-preproduction-c3
status: current
supersedes: null
owner: game-qa
---

# 결함 등록부 (C3 · C4 · C5)

- 심각도: **S1** 차단(어떤 게이트도 PASS 불가) · **S2** 수정 필수(같은 회차 내) · **S3** 경미(다음 회차 허용) · **S4** 관찰(추적만).
- `status`: `open` 미해소 · `open-rfc` RFC 판정 대기 · `closed` 해소 확인 · `S?` 재현 불가(닫지 않음).
- 상세 근거·요구 수정 전문은 `qa/c3-review.md` 동일 id 절. 게이트 영향은 `qa/gate-measurements.md#g{n}`.
- 과거 C3~R7d 회차의 repro는 **문서 대조**다. 당시 빌드·플레이 표본 n=0이므로 런타임 재현 절차를 가진 결함은 0건이었다. M1 코드 검토·native editor 재검증은 §14에 별도로 기록한다.
- **재검증 1 (2026-09-10)**: 1차 24건을 파일 재열람 + 명령 재측정으로 다시 판정하고 신규 9건(C3-F25~F33)을 추가했다. 판정 근거·재측정 명령 원문은 `qa/c3-review.md` **§7**. 정본 입력은 `planning/campaign.json` sha256 `fdabf1d4…`(120479 B), `node validate-campaign.mjs` 44/44 PASS.
- **재검증 2 (2026-09-10)**: 열린 S1 1건(C3-F3)과 open S2 2건(C3-F26 · C3-F28)을 파일 재열람 + 명령 8종 재측정으로 다시 판정해 **3건 전부 closed**, 신규 3건(C3-F34~F36)을 추가했다. **열린 S1 = 0** — C3 전 회차 중 처음이다. 판정 근거·명령 원문은 `qa/c3-review.md` **§8**. 정본 입력 재측정: sha `fdabf1d4…`(120479 B) · `node validate-campaign.mjs` `{checks 44, pass 44, fail 0, verdict PASS}` · `freshness-check.sh` 0 finding / **91** artifacts / exit 0.
- **재검증 3 (2026-09-10)**: worldview 소유 open S2 **1건(C3-F34)**을 파일 재열람 + 명령 7종(T1~T7)으로 다시 판정해 **closed**. 신규 결함 **0건**. 감사 §1이 문서 안에 남긴 집계 재도출 명령을 QA가 그대로 실행해 `pass 37 / violation 0 / open 4 · 41행`을 재현했고(T3), `_workspace/current/systems`의 폐기 6법 문구 **0파일/0행**(T1) · 호명 **6/6 문자 일치**(T4)를 독립 확인했다. 판정 근거·명령 원문은 `qa/c3-review.md` **§9**. 나머지 open 4건(F27·F29·F35·F36)은 이번 회차 대상이 아니며 재검증 2 값을 `[CARRIED]`로 이월한다.
- **재검증 4 (2026-09-10, C6/C7 통합 검토)**: C6 초안 5렌즈 판정단(51건)과 C7 핸드오프 3렌즈 반박(45건)을 **병합·중복 제거**하고 근거를 명령 16종(X-1~X-16)으로 직접 재측정해 신규 **C6-F1~F41 · C7-F1~F32**를 부여했다. 같은 재측정으로 기존 open 20건을 재판정 — **closed 15 · 하향 2(C3-F35·C4-F9 = S2→S3) · 근거 갱신 3**. 근거가 서지 않은 렌즈 발견은 **`qa/c6-review.md` §4에서 기각**했다(대표: `proofRequired ⇒ seal` 전제 — 스키마는 `proofRequired`를 '독립 쌍 존재'로 정의하며 15건 중 `seal` 보유는 3건뿐). 정본 입력 [OBSERVED]: `node validate-campaign.mjs` `{checks 47, pass 47, fail 0, verdict PASS}` · `freshness-check.sh` 0 finding / **103** artifacts / exit 0 (본 갱신 후 재실행 = **104**) · `validate-preproduction.mjs` `512/512 passed` · errors 4(c6/c7 아티팩트). 판정 근거·명령 원문은 `qa/c6-review.md`. **열린 S1 = 1건(C7-F1)** — C3·C4에서 유지되던 'S1 0'이 깨졌다.
- **R5 = C5 독립 검토 (2026-09-10)**: 상품·생산·회귀 축을 명령 12종(M1~M12)으로 검사해 **신규 11건(C5-F1~C5-F11)** 을 등록했다(§5 표). 판정 **SPEC-FIX**, 열린 S1 **0**. 근거·재현 명령 원문은 `qa/c5-review.md`. C3 이월분(총 36 / closed 27 / open 4 / open-rfc 5)은 **판정을 옮기지 않고 `[CARRIED]`** 로 유지한다. 정본 입력 재측정: `planning/campaign.json` sha256 `92301c0a…`(**121457 B**) · `node validate-campaign.mjs` `{checks 47, pass 47, fail 0, PASS}` · `freshness-check.sh` 0 finding / **92 artifacts** (검토 시작 시점) → 재실행 **95** / exit 0 · 덱 재빌드 바이트 동일(`67c6592a…`, diff 0행). 증가분 3 = 본 검토 산출물 1 + **병행 R4 회차가 쓴 2건**(`animation/*` 2) 이며 `qa/c4-review.md` 는 확장(신규 파일 아님). **산출물 수는 측정 시각 종속이라 회귀 신호가 아니다** — `qa/c5-review.md` §1-a.
- **재검증 1 (2026-09-10, R4/R5 수정 루프 1 이후)**: 디렉터 배정 **C4-F5 · C4-F8 · C4-F10 · C5-F1** 4건을 파일 재열람 + 명령 재측정으로 다시 판정해 **4건 전부 closed**. 같은 편집에서 해소된 **C4-F13** 도 재측정으로 확인해 closed. 신규 **C4-F19(S2) · C4-F20(S3) · C4-F21(S3)** 3건 추가, **C4-F16 범위 확대**(`Space` 1키 → 5키·6스펙). 판정 근거·명령 원문(W1~W15 · D1~D12)은 `qa/c4-review.md` **「재검증 1」** 절과 `qa/c5-review.md` **「재검증 1」** 절. 정본 입력 재측정: sha `92301c0a…` · 121,457 B · `47/47 PASS` · `freshness-check.sh` 0 finding / **95 artifacts** / exit 0 · 덱 재빌드 **바이트 동일**(`0468eab2…` · 100,041 B) · `prototype/test-model.mjs` 37/0. **이번 루프에서 새로 측정된 런타임 값은 0건**이며 어떤 게이트도 PASS 로 올라가지 않았다.
- **재검증 2 (2026-09-10, R4/R5 수정 루프 2 이후)**: 디렉터 배정 **C4-F19(S2)** 1건을 파일 재열람 + 명령 15종(X1~X15)으로 다시 판정해 **closed**. 신규 **C4-F22(S3 · systems)** 1건 추가(문서에 적힌 재도출 명령이 문서에 적힌 결론을 내지 않는다 — 2곳). **C4-F14 SC-2 범위 확대**(JSON 오타 2건 → **4건**, `game-ui-contract.json` L620 「덴리트」·「옥긴다」) · **C5-F5 범위 확대**(폐기 용어 「매체 경로」 본문 사용 **12곳 / 9파일** — 1차가 지목한 3파일은 부분집합) · **C4-F21 축소**(41행 중 21행 → **20행**, 요구는 행별 파생 근거이므로 닫히지 않는다). 판정 근거·명령 원문은 `qa/c4-review.md` **「재검증 2」**(V2.0~V2.7)와 `qa/c5-review.md` **「재검증 2」**(D2-0~D2-4). 정본 입력 재측정: sha `92301c0a…` · 121,457 B · `47/47 PASS` · `freshness-check.sh` 0 finding / **96 artifacts** / exit 0(증가분 1 = systems 검증 영수증) · 덱 HTML **바이트 동일**(`0468eab2…` · 100,041 B) · `prototype/test-model.mjs` 37/0. **열린 S1 = 0 유지 · 새로 측정된 런타임 값 0건 · G1~G8 0/8 PASS 유지.**
- **재검증 5 (2026-09-10, R7c 디렉터 처리분)**: 디렉터 배정 **C4-F11 · C5-F2 · C7-F43 · C7-F44** 4건을 파일 재열람 + 명령 14종(R7c-1~R7c-14)으로 다시 판정해 **closed 3 · open 유지(범위 축소) 1**. 신규 **C7-F46(S2) · C7-F47(S2)** 2건 추가 — 둘 다 「조용한 불일치」 유형이다. 판정 근거·명령 원문은 `qa/c6-review.md` **「재검증 5」**. 정본 입력 재측정: `validate-preproduction.mjs` **SPEC-PASS · 514/514 · errors []** · 등록부 **155행 = 생성기 155행 · dropped 0** · `freshness-check.sh` **0 finding / 120 artifacts / exit 0**. **열린 S1 = 0 유지**, 열린 S2 3 → **4**.
- 1차 검토가 적었던 `sourceType` 분포 `log 26 · ledger 22 · plate 21`(합 69 ≠ 72)은 **QA 자체 오기**였다(C3-F32). 수정 전 정확값은 `log 27 · ledger 22 · plate 23 = 72`, 현재는 `log 27 · ledger 22 · plate 24 = 73`이다. 아래 C3-F2 행과 §3의 염판 수치를 그에 맞게 정정했다.

## 1. 결함 표

| id | severity | lane | repro (파일·절) | evidence | status | owner |
|---|---|---|---|---|---|---|
| C3-F1 | S1 | planner / synopsis / systems / balance / economy | `planning/gdd.md` §10 · `planning/content-matrix.md` §3 · `planning/campaign-time-budget.md` §1 · `synopsis/chapter-beats.md` 표 A · `systems/ops/telemetry-contract.md` §1이 인용한 분 배분(30·50·55·60·65·65·70·60·25)을 `planning/campaign.json`과 대조 | live 스테이지 분 = 25·50·55·65·65·70·75·65·10, `cycle: c4` [OBSERVED node 집계]. 해당 값은 `_workspace/archive/20260909-preproduction-c3/planning/campaign.json`에만 존재 | **closed** (RFC-P3-008, 재검증1) | game-production-director |
| C3-F2 | S1 | systems / balance | `systems/data-schemas/*.md` §0 · `beats.md` §1.1 · `architecture-contract.md` L263 · `balance/balance-sheet.md` L17·L20 · `patch-deltas.md` L25 · `sim-results/README.md` L88의 sha256 `2bfe4d52…`를 실측과 대조 | `shasum -a 256`: live `775a984c…`(120087B), archive `5029d44a…`(74342B). `find _workspace -name '*.json'` 전수에 `2bfe4d52` **0건**. 파생 오류: fast 321→**322**, clue 70→**73**, reader 8→**11**, seal 6→**7**, plate→**24** | **closed** (재검증1) | game-systems-designer, game-balance-designer |
| C3-F3 | S1 | planner → systems | `planning/gdd.md` §4 "대응 법" 열 및 `planning/feature-specs/verb-0{2,3,4,5,6}.md` `law:` 필드를 `worldview/worldview-bible.md` §3 및 `worldview/consistency-audit.md` §4와 대조 | 법2·4·5·6의 문구가 audit §4의 **기각된 재작성문**과 동일. `systems/system-specs/*` 5파일 9행 + `interaction-rules.md` L101이 폐기 C2 문구 사용 → 같은 법을 두 레인이 다르게 호명. **재검증 2 실측**: 6곳 전부 bible §3 정본으로 교체, `_workspace/current/systems` 잔존 **0파일**(`qa/c3-review.md` §8.1 S5) | **closed** (재검증 2 — systems가 6곳 교체, 실측 0) | game-systems-designer |
| C3-F4 | S1 | balance / economy | `balance/balance-sheet.md` §4.1~§4.3과 `economy/currency-map.md` §4.1 · `sink-source-ledger.md` §3.1을 나란히 읽는다 | balance = 6계통 한도 14/14/12/9/9/9 · 리셋 없음 · circuit/reader/seal/routing 소모. economy = **전역 단일 9** · 장 경계 전액 리셋 · `routing` 1종 소모 · "계통별 한도는 데이터에 존재하지 않는다 [OBSERVED]". `glossary.md` §4는 "계통별" | **closed** (RFC-P3-009, 재검증1) | game-production-director |
| C3-F5 | S1 | planner / synopsis | `planning/campaign.json` `c6-b3`·`c4-b1` 시각 문자열을 `worldview/timeline.md` §2·§8과 대조 | live에 `H-1:24`·`H-1:04`·`H+0:12` 존재, `H-1:20` **0건**. 캐논은 H-1:40 호출 → H-1:20 밸브, H+0:10 침수. `chapter-beats.md` B17(current)은 "호출이 먼저, 밸브가 20분 뒤". `campaign.meta.md` §7-2가 "적용 대기"라 적었으나 데이터엔 이미 적용 | **closed** (RFC-P3-013) · 잔여 → F25·F26 | game-planner, game-worldview-architect |
| C3-F6 | S1 | planner / synopsis | `planning/campaign.json` `t0-b1`(subtasks[0], clues[0], objective) 및 `c1-b4.consequence`를 `worldview/timeline.md` §7 B01·B07 금지 열, `synopsis/continuity.md` §4와 대조 | `t0-b1`에 '한도연'·'판 #0' 노출 [OBSERVED 전수]. continuity §4는 최초 등장 B18 강제, `chapter-beats.md` §4-6은 "튜토리얼 공개는 상한 초과". `c1-b4.consequence`의 "누군가 도연을 도왔다"는 continuity §4가 저자 표기로 금지한 문자열 | **closed** (RFC-P3-012, 재검증1) | game-planner, game-synopsis-writer |
| C3-F7 | S1 | synopsis | `synopsis/scenes-and-dialogue.md` S6 마지막 서린 대사 | "…그리고 **그걸 누가 언제 확인했는지**." — bible §2 "남지 않는 것", `synopsis/synopsis.md` §7 금지 7, `campaign.meta.md` §7-1("사람이 무엇을 확인했는지"를 남지 않는 것에 추가) 위반. 이전 C3 F1이 지적한 명제의 재발 | **closed** (재검증1) | game-synopsis-writer |
| C3-F8 | S2 | worldview / director | `_workspace/archive/20260909-preproduction-c3/worldview/*.md`의 frontmatter와 `_workspace/current/worldview/*.md`의 `supersedes`를 대조 | 두 문서가 모두 `supersedes: …/c2/…` → 포크. `grep "archive/…-c3/worldview" _workspace/current` = **0건**(고아). `campaign.meta.md` §7이 C4 후속본 소실을 기록 | **closed** (RFC-P3-010, 재검증1) | game-production-director |
| C3-F9 | S2 | worldview / balance / systems | `worldview-bible.md` §3-bis.2(봉인 시점 차감)와 `balance/balance-sheet.md` §4.1(도구 확정 시점 차감)을 `synopsis/chapter-beats.md` 표 A의 T0 행에 적용 | T0에 `seal`이 없는데 B02·B03이 brine_line/power_bus/reader_head를 소모. 봉인이 없으므로 bible 규정상 비용 발생 불가. `continuity.md` §7 RFC-N5 동일 지적 | **closed** (RFC-P3-009 — 소모 개념 소멸) | game-worldview-architect |
| C3-F10 | S2 | planner / systems | `planning/gdd.md` §3.3 층B·§5와 `systems/interaction-rules.md` §0-9·§1-1 대조 | gdd "0.4초 길게 누름은 확정 전용" vs interaction-rules "기본값 `two-step`, `hold`는 opt-in". 부가: gdd(current)가 interaction-rules(c5 **draft**)·puzzle-balance(c4 **draft**)를 정본으로 인용 | **closed** (RFC-P3-015) · 부가 → F33 | game-planner |
| C3-F11 | S2 | planner | `planning/campaign.json` 33비트에 대해 매체 2종·`tools`·`hints`·`recovery`·`checkpoint` 전수 검사 | 매체 2종 미달 **`c1-b4`**(log/log). `tools: []` **`t0-b1` `c1-b4` `c3-b4` `c7-b1` `e0-b1`**. 힌트/복구/저장은 33/33. 거짓이 된 주장: `content-matrix.md` §4.2 "단일 매체 0건", `verb-02` D3 "33/33 PASS", `beats.md` B-I5 "33/33"·B-I3 "clue 70"·§4 "17개 키"(live 23키) | **closed** (재검증1) | game-planner |
| C3-F12 | S2 | synopsis / systems | `synopsis/continuity.md` §5 K1·K3·K6·K9의 "경로 2"에 `systems/interaction-rules.md` §3 독립성 규칙(루트 `originId` 상속)을 적용 | 접수부 확정 사본·주민회 사본이 원본의 루트를 물려받으면 "원본 × 그 사본" 쌍은 독립 판정에서 거부된다. 그런데 §5는 P2 "10/10 충족 — 전부 사본 포함"으로 결론. G7 대체 검증(진행 막힘 0)의 유일한 문서 근거가 흔들림 | **closed** (재검증1) | game-synopsis-writer, game-systems-designer |
| C3-F13 | S2 | worldview | 명사 22종을 `worldview/glossary.md` 행 머리와 기계 대조 후 사용처 역추적 | 미수록 14건 이상: 가설판·증거함(`game-ui-contract.json` 패널명)·사건판·자동 사본·8분 분해능·봉인 완료 접점·인수 각서·이관 목록·근무표·당직 자격 명부·소금 그늘·계통판·결번·상시 슬롯. glossary 머리 규칙 "여기에 없는 고유명사는 … UI 문자열에 쓸 수 없다" 위반. `continuity.md` §6.1은 3건만 인지 | **closed** (재검증1) | game-worldview-architect |
| C3-F14 | S2 | director / qa | 계약 `## Time acceptance`와 `systems/ops/telemetry-contract.md` §5 AF3·AF4 대조 | 수용 조건이 `total_min` / `afk_total_min` / `total_minus_afk_min` 중 무엇인지 미정의 → 표본이 생겨도 판정 불가. 부가: fast 322 < 360 · deliberate 673 > 600 분쟁이 `campaign.meta.md` §6-1(범주 오류)과 `campaign-time-budget.md` §9.3(위험 유지)로 양립 중 | **closed** (RFC-P3-011, 재검증1) | game-production-director |
| C3-F15 | S2 | systems / balance / planner / economy | `balance/balance-sheet.md` §2 · `verb-02-plate-read.md` D5 · `economy/sink-source-ledger.md` §6 · `planning/update-scope.md` P3·P7의 [OBSERVED] 부재 주장을 `ls`와 대조 | `systems/data-schemas/` 6파일 존재, `systems/ops/telemetry-contract.md` 존재. update-scope P7의 "실물 74341 / 2bfe4d52…"도 실물(120087 / 775a984c…)과 불일치. 특히 economy §6의 "G3 PASS 불가" 결론이 부재 근거 위에 서 있음 | **closed** (재검증1) | game-balance-designer, game-planner, game-economy-designer |
| C3-F16 | S3 | economy | `economy/reward-bands.md` §1 채널 D 예시와 `sink-source-ledger.md` §3.3의 인용문을 `planning/campaign.json`에서 `grep -cF` | "거래를 수락해도 같은 경로가 열린다" → live **0건** / archive 1건. live `c3-b4.consequence`는 "수락·거절 어느 쪽이든 경로는 열리며 …" | **closed** (재검증1) | game-economy-designer |
| C3-F17 | S3 | synopsis | `synopsis/continuity.md` §5 K2 행과 `synopsis/synopsis.md` §5 결말 요건 목록 대조 | K2 "필요한 결말: A·B" 인데 A(K1·K3·K6·K7+K4)·B(K1·K4·K8·K9·K10) 어디에도 K2 없음 | **closed** (재검증1) | game-synopsis-writer |
| C3-F18 | S3 | synopsis | `synopsis/chapter-beats.md` 표 A "저장" 열과 표 B B08·B21 결과 열 대조 | 표 A 33/33 "예" vs 표 B "(연습 시각화, 저장 안 됨)". 열 이름이 같아 오독 유발. `continuity.md` §1과 대조하면 의미는 다름(체크포인트 vs 연습 상태 잔존) | **closed** (재검증1) | game-synopsis-writer |
| C3-F19 | S3 | synopsis / balance | `synopsis/chapter-beats.md` §3 차이 1과 같은 문서 표 A의 `seal` 배정 집계 | 문서는 "seal 6회", 표 A 실제 7회(확정 5 + 연습 2). `content-matrix.md` §8-M6이 이미 정정 요청. live JSON 집계도 7 | **closed** (재검증1) | game-synopsis-writer |
| C3-F20 | S3 | systems / balance | `systems/system-specs/hint-system.md` §2 상태기계와 `balance/balance-sheet.md` §6 임계표 대조 | systems = 180초 단일 제안 + 180초 쿨다운. balance = T1 180 / T2 누적 420 + 오확정 2 / T3 누적 900 + 오확정 4 + 확인 클릭. 자동 제안 모델이 두 개 | **closed** (RFC-P3-015, 재검증1) | game-systems-designer |
| C3-F21 | S3 | synopsis | `synopsis/scenes-and-dialogue.md` S1 오브젝트 목록 · S2 헤더를 `consistency-audit.md` §4, `continuity.md` §7, `content-matrix.md` §8-M1과 대조 | S1 "번호만 찍힌 염판 #0" = audit §4 기각 문구이며 OPEN-2 저작 보류 위반. S2 헤더 구역 `gate` vs 확정 배정 `hub` | **closed** · 잔여 구역 → F29 | game-synopsis-writer |
| C3-F22 | S4 | planner | `planning/campaign.json` 비트 객체 키 목록 확인 | 33/33 비트에 `zoneId` 부재. `zoneIds`는 스테이지 단위. 비트-구역 단일 출처가 `content-matrix.md` §3 표에만 존재 | open-rfc (RFC-P3-004 / RFC-N1) — C4 이월 확정 | game-planner |
| C3-F23 | S4 | systems / economy | `systems/prototype/model.mjs:82`와 `systems/interaction-rules.md` §0-2 · `planning/gdd.md` §8 대조 | `maxUndo: 32` vs "되돌림 무제한". 32단 초과가 진행을 막는 경로 미탐색 | **closed** (RFC-P3-015, 재검증1) | game-systems-designer |
| C3-F24 | S4 | presentation (검토 범위 밖 관찰) | `presentation/steam-game-plan.html` 슬라이드 8 · `generate-deck.mjs:549` | "법 4 용량 — 이번 조수에 보호 용량이 부족하다"를 `worldview/worldview-bible.md` 출처로 표기. C3-F3과 같은 뿌리이며 대외 슬라이드라 노출 위험이 크다 | **closed** (RFC-P3-014, 재검증1) | game-presentation-director |
| C3-F25 | S2 | worldview / planner / director | `production/decision-log.md` L55 RFC-P3-013 decision의 폐기 시각 목록과 `worldview/timeline.md` L43 · live `campaign.json` · `synopsis/continuity.md` L219 대조 | RFC 본문은 "**H-1:40**/H-1:20/H+0:10은 폐기"라 적으나 timeline L43은 H-1:40을 현행 캐논(도연 서명)으로 유지하고 live JSON에 **3건**, continuity B17이 "H-1:40 부근 좌표화"로 사용. 검증기 `K-04`는 H-1:20·H+0:10만 검사해 이 모순을 잡지 못한다 [OBSERVED `validate-campaign.mjs` L42] | **closed** (재검증 4 — 디렉터 C3 종료 판정: H-1:40 캐논 유지, 폐기는 H-1:20·H+0:10 둘뿐) | game-production-director |
| C3-F26 | S2 | systems | `systems/system-specs/tide-alignment.md` §3 규칙표 A-R3 행을 `worldview/timeline.md` L50 폐기 목록과 대조 | A-R3 = "캐논 사례: 서명(H-1:40) → 밸브(**H-1:20**) 간격 20분 … [OBSERVED]". 폐기 기록이 아니라 **현행 규칙표 본문**이며 RFC-P3-013 캐논(밸브 = H-1:24)과 반대. 전 워크스페이스 `H-1:20` 17히트 중 **유일한 본문 사용** | **closed** (재검증 2 — A-R3이 캐논 쌍으로 교체, `tide-alignment.md` 내 `H-1:20` 0건) | game-systems-designer |
| C3-F27 | S2 | balance / economy | `balance-sheet.md` L223·L480 · `currency-map.md` L86·L91·L96·L177·L185 · `negotiation-record.md` N-14의 [OBSERVED] 주장을 systems 실물과 대조 | (a) `corrosion-budget.md` C-R2는 **이미 "전역 단일 9"**(L89)인데 balance·economy가 "계통별"이라 주장 (b) `zones.md` L42는 **이미 "표시 분해 전용"**인데 "차단형 전제"라 주장 (c) **`save.md` L94에서 `operationalCorrosion`이 제거됐는데** economy §4.1 계통별 표시 모델이 그 필드에 걸려 있다 — (c)는 문장이 아니라 **모델 충돌** | **closed** (재검증 4 — `save.md` §2.1 「부식 저장 필드 없음」 신설 + economy 3문서 일치) | game-balance-designer, game-economy-designer |
| C3-F28 | S2 | worldview | `worldview/glossary.md` L12·L138 · `consistency-audit.md` L79의 집계 영수증을 `shasum -a 256` + `node validate-campaign.mjs` 결과와 대조 | 두 문서가 "단서 **72** / `plate` **23**"과 입력 sha `775a984c…`를 적는다. 실측은 단서 **73** / `plate` **24** / `fdabf1d4…`. originId 31종은 불변이므로 **C3-F13 수록 판정에는 영향 없음** — 틀린 것은 영수증 숫자와 입력 해시. `campaign.meta.md` L729가 이미 브로드캐스트함 | **closed** (재검증 2 — 영수증 3문서 재측정 교체, QA 독립 집계와 전건 일치) | game-worldview-architect |
| C3-F29 | S2 | planner / synopsis | live `campaign.json` `subtasks[0]` 본문과 `content-matrix.md` §3(L106·L108, 스스로 "비트-구역 단일 출처" 선언) · synopsis 표를 3자 대조 | `c4-b1` 데이터 "저지대 주민회 사무실"(lowland) ↔ 표 `hub`, `c4-b3` 데이터 "당직실 봉인대"(hub) ↔ 표 `lowland` — **단일 출처 표와 데이터가 서로 뒤바뀜**. `c1-b4` L426 "제3수문에서"(gate) ↔ 배정 `hub`. 스테이지 `zoneIds`가 두 구역을 모두 포함해 검증기가 못 잡는다. C3-F22와 같은 뿌리 | **closed** (재검증 4 — `c4-b1`/`c4-b3`/`c1-b4` 해소 확인. 잔여 실체는 본문↔`zoneId` 4비트 → **C6-F17로 승계**) | game-planner, game-synopsis-writer |
| C3-F30 | S3 | planner / synopsis / worldview | `content-matrix.md` §3 · `chapter-beats.md` 표 A의 B# 부여와 `worldview/timeline.md` §7의 B# 부여를 비트 id로 대조 | 두 체계가 공존한다 — `c4-b1` = **B17**(planner/synopsis) vs **B16**(timeline), `c4-b3` = **B16** vs **B18**. T0·C6·C7·E0에서만 일치. 같은 "B17"이 문서에 따라 다른 비트를 가리킨다 | **closed** (재검증 4 — 디렉터 판정 완료. 잔여 파생 불일치는 `consistency-audit.md` **A42**로 이관) | game-production-director |
| C3-F31 | S2 | balance / planner | `balance/balance-sheet.md` §7 난이도 지수 3열을 live 데이터로 재계산 | 관측 2열(최대 단서·최대 도구)은 QA 재측정과 **전건 일치**하나, 세 번째 열 `동시 가설 수`는 `[INFERENCE]`이며 balance 스스로 "이번 회차 재측정 안 함"이라 적는다. **그 열을 빼면 지수가 `[4,5,5,4,4,5,7,4,3]`이고 C5→C6 = +2로 규칙을 통과한다** [OBSERVED] — 위반 판정의 부호가 미측정 열 하나로 뒤집힌다. A안(planner 재저작)·B안(규칙 완화) 모두 현재 근거로는 실행 불가 | **closed** (재검증 3 · R7 — §7 이 미측정 열 제외 후 관측 2열 재도출, §7.2 `[TARGET]` 분리. QA 가 `node -e` 3종 재실행해 `[4,5,5,4,4,5,7,4,3]` + 포락=실부하 재현. C5→C6 **+2** 통과. **영수증 값은 재현 실패 → C7-F42**) | game-balance-designer |
| C3-F32 | S3 | qa | 1차 `c3-review.md` §1.1 live 열의 `sourceType` 분포를 같은 행의 clue 수와 대조 | `log 26 · ledger 22 · plate 21` = **69 ≠ 72**(내부 모순). 정확값은 수정 전 `27 / 22 / 23 = 72`, 현재 `27 / 22 / 24 = 73` — 근거 3중(현재 실측 · planner가 더한 단서가 plate 1건뿐 · glossary·audit 독립 집계). balance §10.2 Q7과 planner counter는 이 정정으로 해소 | **closed** (재검증1 자기정정) | game-qa |
| C3-F33 | S3 | systems / planner / director | `systems/interaction-rules.md`(c5 **draft**) · `balance/puzzle-balance.md`(c4 **draft**)의 frontmatter와 이를 정본으로 인용하는 `gdd.md` · `data-schemas/*.md` · `continuity.md`(전부 current) 대조 | RFC-P3-015가 draft 문서 §1-1을 **정본으로 지정**했으나 `status`는 draft 그대로다. `freshness-check.sh`는 인용 방향을 검사하지 않는다. 1차 C3-F10의 "부가" 지적이 승격된 것 | open-rfc | game-production-director |
| C3-F34 | S2 | worldview | `worldview/consistency-audit.md` L26 §1 · L64 A33 · L86 §3 · L106 §4-1 · L125 §5를 `systems/system-specs/*` 실물과 대조 | 다섯 위치가 A33(폐기 6법 문구가 systems 5행에 살아 있다)을 현행 `[OBSERVED]`로 유지하나 실측은 **0파일 / 0행**. L86은 표제가 **"[OBSERVED, 2026-09-10 2차 재측정]"**인 기계 검사 표의 행이다. §1 집계가 `violation 1`을 유지해 G1 자체 감사 수치가 실제보다 나쁘다. **재검증 3 실측**: 다섯 위치 전부 재작성 확인 — §1 = `pass 37 / violation 0 / open 4`(문서 내 재도출 명령을 QA가 실행해 재현, T3) · §2 A33 = `pass` · §3 「법 문구 유통」 3행이 3차 실측으로 교체 + 행별 회차 표기 규칙 신설 · §4-1 = "해소 이력"으로 문맥 전환(문구 5행 보존) · §5·§6 기록. `_workspace/current/systems` 잔존 **0파일/0행**(T1), 호명 **6/6 문자 일치**(T4) | **closed** (재검증 3 — QA 독립 재측정 T1~T5가 문서의 새 수치와 전건 일치) | game-worldview-architect |
| C3-F35 | S3 | planner / economy | `economy/currency-map.md` L101 ↔ L179 · `economy/negotiation-record.md` L102 · `planning/gdd.md` L112 ③ · `planning/feature-specs/verb-02-plate-read.md` L39 R3a ↔ `systems/data-schemas/{plates,save}.md` | economy가 **철회**한 필드명 `plateOriginalWear`(L102·L179)를 같은 문서 L101이 여전히 "채택안(옵션 B)"으로 적고, 그것을 근거로 planner 2문서가 카운터 이름을 확정 인용. 실제 스키마는 `readBudget`(상한, `plates.md` L39) + `readCounts`(누계, `save.md` L54)이며 `save.md` L92 v1 목록에 `plateOriginalWear` **없음**. CLAUDE.md §9 저장 필드 불변식 · C4 핸드오프 위험 | open (재검증 4 — economy 이행 완료. 잔여는 `data-schemas/plates.md` **L39 한 줄**뿐 → **S2 → S3 하향**) | game-systems-designer |
| C3-F36 | S3 | systems | `systems/prototype/model.mjs` L58 라벨을 `worldview/timeline.md` §8 앵커 표와 대조 | `{ id: 'pair-20', label: '봉인 호출 ↔ 밸브 대기', gapMin: 20 }` — 캐논 쌍은 "밸브 개폐 각인 ↔ 봉인 완료 접점 각인"이고 라벨 순서가 캐논 선후와 반대로 읽힌다. `gapMin: 20`은 일치. 재검증 1이 "도메인 명사"로 PASS했으나 F26·plates 수정 뒤 **앵커 쌍을 폐기 용어로 부르는 마지막 위치**. 완화: `prototype.meta.md` `status: draft` | **closed** (재검증 4 — `model.mjs` L58 = `밸브 개폐 각인 ↔ 봉인 완료 접점 각인`) | game-systems-designer |

## 2. 집계 (재검증 3 기준 · 2026-09-10)

| severity | 1차 | 재검증1 신규 | 재검증2 신규 | 재검증3 신규 | 합 | closed | open | open-rfc |
|---|---|---|---|---|---|---|---|---|
| S1 | 7 | 0 | 0 | 0 | 7 | **7** | **0** | 0 |
| S2 | 8 | 6 | 2 | 0 | 16 | **11** | **3** (F27·F29·F35) | 2 (F25·F31) |
| S3 | 6 | 3 | 1 | 0 | 10 | 7 | 1 (F36) | 2 (F30·F33) |
| S4 | 3 | 0 | 0 | 0 | 3 | 2 | 0 | 1 (F22) |
| **합계** | **24** | **9** | **3** | **0** | **36** | **27** | **4** | **5** |

회차별 closed 추이 [OBSERVED]: 1차 0 → 재검증1 23 → 재검증2 26 → **재검증3 27**. 재검증 3은 **신규 결함 0건**이며, 이는 C3 전 회차 중 처음이다(1차 24 · 재검증1 +9 · 재검증2 +3 · 재검증3 **+0**) — 수정 범위가 1파일·지목 절로 한정됐기 때문이다 [INFERENCE].

**열린 S1 = 0.** C3 전 회차 중 처음이며(1차 7 → 재검증1 1 → 재검증2 **0**), CLAUDE.md §6의 "열린 S1이 모든 게이트 PASS를 막는다"는 **차단 사유가 소멸**했다. 다만 게이트 PASS는 여전히 불가다 — **open S2 3건**(F27·F29·F35 · 그중 F27(c)는 스키마 충돌, F35는 저장 필드명), open-rfc 5건, open S3 1건(F36), 그리고 **빌드 0줄 · 표본 n=0 · 시뮬 0회**로 G2~G8이 전부 `NOT-MEASURED`이기 때문이다. 수치는 `qa/gate-measurements.md#g1`~`#g8`.

**재검증 2 판정 요약**: C3-F3(S1) closed — systems가 5스펙 9행 + `interaction-rules.md` L101 = **6곳**을 bible §3 정본으로 교체(QA 인용은 5곳이었다, 자기정정). C3-F26 closed — A-R3이 RFC-P3-013 확정 시각 쌍(H-1:24 → H-1:04)만 쓰도록 교체돼 **C3-F25 판정에 비종속**. C3-F28 closed — 영수증이 QA 독립 집계(단서 73 · `log 27 · ledger 22 · plate 24` · 31종)와 전건 일치.

**재검증 3 판정 요약**: C3-F34(S2) closed — 감사가 지목된 다섯 위치를 **실측 위에서** 다시 썼고 QA 독립 재측정과 전건 일치. 특기 사항 둘. (1) 폐기 문구 5행을 **지우지 않고** §4-1의 성격만 "위반 목록 → 재유입 방지 원장"으로 바꿨다 — 삭제했다면 그 절을 인용주로 쓰는 `systems/system-specs/*` 6종 L11이 동시에 고아가 된다. (2) §1이 행 번호가 아닌 `A01`·`A41` **앵커 기반 재도출 명령**을 문서에 남겨, QA가 그 명령을 그대로 실행해 집계를 재현했다(T3) — "재도출했다는 주장에 명령이 없다"는 재검증 1·2의 반복 지적에 대한 첫 구조적 해소다. **G1의 문서 violation 은 2건 → 1건(C3-F29)**.

**C4 인계 우선순위**: F27(c) → **F35** → F25 → F29 → F31 → F33 → F30 → **F36** → F22. (F34 제거)

## 3. 브로드캐스트 (dependency-matrix ● 항목)

`feedback-requested-by: 2026-09-11` (재검증 **3** 기준. 1차·재검증 1·재검증 2 브로드캐스트는 `qa/c3-review.md` §2 각 결함 절 · §7.7 · §8.8에 보존)

| 받는 레인 | 열린 결함 | 요청 |
|---|---|---|
| game-production-director | F25, F30, F31, F33, F22, RFC-Q1, **RFC-Q2**, q-3, q-4 | H-1:40 지위 판정(**worldview·QA 실측이 (a) "H-1:20·H+0:10만 폐기"를 지지** — live JSON H-1:40 3건, 검증기 L42가 두 값만 검사) · B# 체계 지정 · 난이도 지수 처리 · draft 인용 정책(`interaction-rules.md` RFC-S5 포함) · 비트 `zoneId` · **RFC-P3-008 본문의 sha(`fdabf1d4…`)·단서 수(73) 갱신** · **RFC-Q2**(같은 경로 제자리 대체 문서의 `supersedes` 처리 — `status: current` + `supersedes: null` **53건**, QA 자신 포함) · q-3 RFC-W3 소멸을 `decision-log.md`에 기재할지 · q-4 `consistency-audit.md` §4-1 **절 번호 고정**을 불변식으로 승격할지(QA 대안: 인용을 절 번호 대신 제목 문자열로) |
| game-worldview-architect | F25 (판정 대기) | **C3-F34 closed (재검증 3).** 다섯 위치 수정 확인 — 추가 요청 없음. 유지할 것: §3 **행별 회차 표기 규칙**, §6 자기 인계 "타 레인이 닫은 결함의 감사 행 재측정", §1의 앵커 기반 재도출 명령. H-1:40(F25)은 디렉터 판정 대기 |
| game-economy-designer | **F35**, F27(c) | `currency-map.md` **L101** "채택안(옵션 B) `plateOriginalWear`"를 L179(철회)·`negotiation-record.md` L102와 일치시켜 `readCounts`/`readBudget`으로 정정 · `operationalCorrosion` 제거(`save.md` L94)를 §5 자원표에 반영 |
| game-planner | **F35**, F29, F22, F30 | `gdd.md` L112 ③·`verb-02` L39 R3a의 `plateOriginalWear` → `readCounts`(누계)/`readBudget`(상한) · L118·L280 counter를 "차단형 문구 잔존"에서 **"이름 불일치"**로 축소(P-R9·P-R11이 이미 비차단) · `content-matrix.md` §3 `c4-b1`↔`c4-b3` 구역 정정 · `c1-b4.subtasks[0]` "제3수문" 정정 |
| game-systems-designer | **F36**, F27(c) | `prototype/model.mjs` L58 라벨을 캐논 쌍 "밸브 개폐 각인 ↔ 봉인 완료 접점 각인"으로 교체(1줄, id·값·테스트 불변) · `operationalCorrosion` 제거를 economy와 합의 |
| game-balance-designer | F27(a)(b), F31 | `corrosion-budget.md` C-R2·`zones.md` L42에 대한 스테일 주장 철회 · `동시 가설 수` 산출 규칙을 재현 가능한 명령으로 정의하거나 지수에서 분리 |
| game-synopsis-writer | F29, F30 | 씬·표의 구역 표기를 데이터와 맞춤 · 문서 간 인용은 `campaign id`로 |
| game-presentation-director | — | C3-F24 닫힘. 다음 배정은 C4 |

**QA 자기정정 (재검증 3) [OBSERVED]**: (1) `gate-measurements.md` L47 `worldview_self_audit_claim`이 감사 수정 후에도 `pass 36 / violation 1 / open 4`를 인용해 **스테일이었다** → `pass 37 / violation 0 / open 4`로 교체. (2) 같은 파일 `#g1 violation_open` 에 닫힌 C3-F34가 남아 있었다 → 제거, violation 2건 → **1건(C3-F29)**. (3) 위 §1 표 C3-F3 행의 lane 열이 `planner` 단독이었다 → `planner → systems`로 정정(감사 §5와 일치). 세 건 모두 **감사 문서가 아니라 QA 문서가 스테일 측**이었던 경우이며, 레인이 open question 으로 올려 준 것을 QA가 수용한 결과다.

**QA 자기정정 (레인 counter 수용) [OBSERVED]**: (1) C3-F3 요구 수정의 폐기 문구 보존 위치 "§4-2"는 **§4-1**의 오기다(`consistency-audit.md` L95 = "4-1 폐기된 6법 호명 문구", L109 = "4-2 폐기된 장치·전제"). (2) "5줄로 닫힌다"는 과소 계산이었고 실제는 **6곳**이었다 — 문자열 결함의 재측정 범위를 결함이 인용한 파일이 아니라 **소유 레인 폴더 전체**로 넓히는 것으로 규칙을 고쳤다. (3) C3-F26의 뿌리도 한 줄이 아니었다 — `data-schemas/plates.md` L77이 폐기 문자열 없이 **폐기 산술("서명 → 밸브 20분")만** 남겨 문자열 grep을 통과했다. 시각 결함은 문자열 + **간격 산술(쌍 + 분)** 두 축으로 검사한다.

**RFC-Q2 (qa → director) [신규, 재검증 3]**: `status: current` + `supersedes: null` 문서 **53건**(전체 current `.md` 95 · 아카이브 경로를 가리키는 것 15) [OBSERVED] 중 최소 2건 — `worldview/consistency-audit.md` §0 과 **`qa/c3-review.md` §0** — 이 본문에서 "같은 경로의 앞선 판을 대체했다"고 스스로 적는다. CLAUDE.md §2는 대체 시 `archive-cycle.sh` 경유 + `supersedes:` 연결을 요구하지만 `freshness-check.sh`는 이 축을 검사하지 않는다(그래서 exit 0). 판정 요청: **같은 사이클 안의 초안 제자리 갱신을 §2의 "대체"로 볼 것인가.** 아니면 계약에 예외를 명문화하고, 맞으면 `freshness-check.sh`에 검사를 추가해야 한다 — 지금은 어느 쪽도 아니어서 규칙이 조용히 지켜지지 않는다. **QA가 위반 당사자에 포함되므로 결함 id 를 열지 않고 판정을 요청한다.** 근거: `qa/c3-review.md` §9.5.

**RFC-Q1 (qa → director)**: RFC-P3-008 본문의 정본 sha `775a984c…` / `clue 72`는 판정 시점 값이며, 현재 실물은 `fdabf1d4…`(120479 B) / `clue 73`이다(6개 레인 독립 확인 + QA 재측정 2명령 일치). 계보 판정 자체는 유효하나 본문 숫자를 갱신하지 않으면 다음 세션이 같은 계보 검증에서 다시 막힌다.

## 4. 재현 불가 / 미측정 [OBSERVED]

**재검증 1·재검증 2·재검증 3(2026-09-10)에서 바뀐 것 없음.** 세 회차 모두 새로 측정된 런타임 값은 **0건**이다. 열린 S1이 0이 된 것은 문서가 서로 모순되지 않게 됐다는 뜻이지 게임이 동작한다는 뜻이 아니다. 아래 5행은 1차 판정을 그대로 유지한다 — 빌드와 표본이 없는 상태에서 이 문서들을 작성하면 측정 위장이 된다.

| 항목 | 상태 |
|---|---|
| 런타임 결함 | **0건 등록.** 빌드 0줄이므로 발견될 수 없다 — "없다"가 아니라 "찾을 수단이 없다" |
| 익스플로잇 | `qa/exploit-register.md` 미작성. 전투·유료 재화·멀티플레이가 없고 실행 빌드도 없어 익스플로잇 정의 자체가 성립하지 않는다. 빌드 후 "무료 힌트로 정답 즉시 도달"·"되돌림으로 확정 비용 회피"를 첫 후보로 검사한다 |
| 회귀 매트릭스 | `qa/regression-matrix.md` 미작성. 회귀 대상은 문서뿐이며 §1 표의 `repro` 열이 그 역할을 대신한다 |
| 플레이테스트 | `qa/playtest-report.md` 미작성. 표본 n=0 · 아키타입 0/5 |
| 몰입 점수 | `qa/immersion-scores.md` 미작성. 빌드·표본 없이 점수를 만드는 것은 측정 위장 |

---

## 5. C5 (R5) 결함 표 — 상품 · 생산 · 회귀 [2026-09-10]

근거·재현 명령 전문은 `qa/c5-review.md` 동일 id 절. 게이트 영향은 `qa/gate-measurements.md#g3`·`#g7`·`#g8`.
모든 repro 는 **문서·파일 대조**다. 빌드 0줄 · 표본 n=0 이므로 런타임 재현 절차를 가진 결함은 **0건**이다.

| id | severity | lane | repro (파일·절) | evidence | status | owner |
|---|---|---|---|---|---|---|
| C5-F1 | S2 | presentation | 덱 11번 ↔ 26번 텍스트 대조 후 `systems/unity-implementation.md` §10 · live `campaign.json` `stages[0]` 과 삼자 대조. `grep -n "제3수문\|SLICE_ZONE_COUNT" presentation/generate-deck.mjs` | (재검증 1 · 2026-09-10) **closed**. 이전 관측: 26번(`generate-deck.mjs:1033` 하드코딩) = "허브와 **제3수문**, 도구 **reader·alignment·seal**", 제외 구역 = `allZones 5 - SLICE_ZONE_COUNT 2` = **3**. 정본 §10 = "**hub 1개 + circuit·reader 2개 + 25분**, alignment·routing·corrosion·seal 은 T0 에 없다", live `T0.zoneIds=["hub"]` → 제외는 **4구역**. 11번은 데이터 파생이라 정본과 일치 — **같은 덱이 두 T0 를 동시에 주장**한다. `generate-deck.mjs:172` 주석이 근거로 §10 을 인용하나 §10 은 반대를 적는다. 26번은 **T0 착수 승인 슬라이드**. **재검증 1 [OBSERVED 2026-09-10]**: `SLICE_ZONE_COUNT` **0행**, 11·26번이 같은 `sliceZoneText` 파생, 빌드 리포트 `sliceT0 = {25분, ["hub"], ["circuit","reader"], 제외 4구역}` = live 데이터와 문자 일치, 렌더 문장 "포함은 당직실, 도구 circuit 와 reader" · "제외는 나머지 4구역". live HTML 이 재빌드본과 **바이트 동일**(`0468eab2…` · 100,041 B). QA 독립 음성 시험: N1(하드코딩 복귀) exit 1 · errors 4 / N2(정본 §10 을 hub+gate·30분으로 흔듦) exit 1 · errors 3 / 양성 대조 exit 0 — **게이트가 C4-F8 계열 정본 드리프트까지 잡는다** | **closed** | game-presentation-director |
| C5-F2 | S2 | director / production | `python3 -c "import json;print(json.load(open('production/cycle-ledger.json'))['cycles'])"` ↔ `qa/c3-review.md` §9.6 · `qa/c4-review.md` 판정 절 | 대장 = C3 `independent-review-running`/null/null · C4 `draft-preparation`/null/null · C5 `draft-preparation`/null/null. 실제 = C3 **SPEC-FIX, 36건/closed 27/open 4/open-rfc 5**, C4 **독립 검토 완료(FIX, material 4)**, C5 **검토 진행(본 회차)**. 덱 19번이 이 표를 그대로 렌더해 "C4 = draft-preparation 미기록"으로 브리핑에 전파 — **사실보다 나쁘게 틀렸다**. `cycle-ledger.meta.md` "실제연속검토와수정상태" 도 동반 오류 | open (재검증 3 · R7 — **원인 확정.** 대장은 생성기 출력과 완전 일치 → 틀린 것은 생성기다(**C7-F44**: 등록부 148행 중 4행 조용히 탈락). 추가로 **덱 19번 슬라이드가 손으로 적은 옛 값**을 렌더(C3 27↔32 · C4 9↔12 · C5 6↔9 · C4·C5 `reviewed-and-revised`). R7-18·R7-19) → **open (범위 축소)** (재검증 5 · R7c — **대장↔등록부 모순은 해소**: `dropped 0` · 155 = 155 · 회차별 전건 일치. 잔여는 **덱 재빌드 1건** — s19 가 아직 C3 36/**27** · C4 22/**9** · C5 11/**6** 과 `reviewed-and-revised` 5건을 렌더하고 html **06:28** < ledger **10:56**. 다만 지금 재빌드하면 카운트 21칸이 전부 `미기록` 이 된다 → **C7-F47 선행 필요**. R7c-6·R7c-7·R7c-11·R7c-12) → **closed** (재검증 6 · R7d — 잔여 1건(덱 s19 재빌드) 해소. 판정 기준으로 mtime 대신 **내용 일치**(presentation 제안)를 수용한다: 대장 9키 × 7회차 = **63칸** ↔ 렌더된 s19 63칸 `diff` **0줄**, 각주 스탬프도 대장 `generated_at`·`source`·`generator` 와 일치. 더해 체크인된 html 과 현재 대장으로부터의 재빌드가 **바이트 동일**(`f2028d3b…`)이라 손기입 값이 남을 자리가 없다 — mtime 은 재현할 때마다 갱신되므로 신선도 기준이 될 수 없다는 반론을 값으로 대체한 것이다. R7d-8~R7d-10) | game-production-director |
| C5-F3 | S2 | director / production | `node .claude/skills/game-ops-harness/scripts/validate-preproduction.mjs \| head -12` · `ls _workspace/current/production/cycles/` | 검증기 `errors: ["missing cycle artifact qa/c5-review.md", "missing cycle artifact production/cycles/**c5-development.md**"]`. `cycles/` 에 c1~c4 만 존재. CLAUDE.md §10 "회차마다 이전 문서·변경·근거·미측정 항목을 남긴다" 미충족 — C5 산출물은 존재하는데 그것을 만든 회차 기록만 없다. 첫 error 는 본 등록으로 해소 | **closed** (재검증 4 — `c5-development.md` 존재, 검증기 오류가 c6/c7로 이동 → 후속 **C6-F25**) | game-production-director |
| C5-F4 | S3 | presentation | `node planning/validate-campaign.mjs \| head -12` ↔ `sed -n '52,62p' presentation/steam-game-plan.meta.md` | 메타 r3 표가 `fdabf1d4…`·**120,479 B**·`44/44` 를 고정 기재. live = `92301c0a…`·**121,457 B**·**47/47**. RFC-Q1 "고정 숫자 재기재 금지" 위반. **완화**: HTML 자체는 스테일 아님 — 재빌드본이 **바이트 동일**(`67c6592a…`·100,060 B, diff 0행). 스테일한 것은 산출물이 아니라 영수증 절 | **closed** (재검증 4 — r5 절이 현행 실측 기재, 산출물 해시 2행 `shasum` 실측과 2/2 일치) | game-presentation-director |
| C5-F5 | S3 | presentation | `grep -oE "T-[0-9]{2}" systems/unity-implementation.md \| sort -u \| wc -l` ↔ `grep -n "인수 테스트 14\|매체 경로" presentation/generate-deck.mjs` | 덱 17번 = "기술 인수 테스트 **14개**", 정본 §11 = **27개**(T-01~T-27). 같은 슬라이드가 systems 가 **명시적으로 폐기**한 용어 "**매체 경로**"(`unity-implementation.md:69` 괄호 · `tech-verification/c4-self-check.md` F3 2행 = "있음(명시적 폐기)")를 사용. 정본 표현은 `sourceType 상이 AND originId 상이`. `deck-outline.md` L13 "슬라이드에 숫자를 손으로 적어 넣지 않는다" 자기 규약 위반. **재검증 2 범위 확대 [OBSERVED 2026-09-10 · Y5]**: 폐기 용어 「매체 경로」의 본문 사용은 덱 2곳이 아니라 **12곳 / 9파일**이다 — `plate-readout` L60·L70·L118 · `drainage-routing` L57·L74 · `beats` L161(1차 지목) + **`wiring-trace` L72 · `economy/currency-map` L131 · `economy/sink-source-ledger` L122(`INV-P3` 불변식 문장) · `balance/balance-sheet` L494(`safety_two_disjoint_media_paths` 설명)**(미지목) + `generate-deck.mjs` L866 및 그 렌더 `steam-game-plan.html` L167. 요구 수정의 「폐기 문구 게이트에 `매체 경로` 추가」를 지금 켜면 **4개 레인이 동시에 열린다** — 켜기 전에 12곳을 알고 배정할 것. severity 는 S3 유지(정본 표현이 `unity-implementation.md` L69 에 서 있고 데이터·게이트는 그 필드로 동작한다 — 틀린 것은 문서의 용어다) | **closed** (재검증 4 — 규칙 본문 사용 **0건**, 렌더 HTML 0건, 17번 = 「기술 인수 테스트 27개」 파생) | game-presentation-director(덱 2) · game-systems-designer(6) · game-economy-designer(2) · game-balance-designer(1) |
| C5-F6 | S2 | director / systems | `ls -la _workspace/current/handoff/` ↔ `sed -n '64,81p' README.md` | `handoff/` **파일 0개**(빈 디렉터리)인데 README L70 이 "핸드오프 브리프 · 검증 계획 · 리소스 런북"을 목차에 적고 L80 이 "그 브리프 순서를 따릅니다"라고 지시한다. 핸드오프는 **C7 산출물**이며 미착수. 계약 Honesty gates 축. 단, L80 의 T0 서술("허브 + circuit/reader, 25분")은 **정본과 일치** — 덱 26번보다 정확하다 | **closed** (재검증 4 — `handoff/` 4파일 존재, README 서술과 일치) | game-production-director |
| C5-F7 | S3 | product | `ls scripts/` · `ls scripts/validate-preproduction.mjs` | `economics.meta.md` "계산은 **`scripts/validate-preproduction.mjs`** 에서 재생성한다" → **파일 없음**. `scripts/` = `gen-2d.sh · gen-video-higgsfield.sh · make-previz-gif.sh · refresh-2d-provenance.py`. 실제 경로는 `.claude/skills/game-ops-harness/scripts/validate-preproduction.mjs`(실행 확인, exit 0). "계산 원본" 문서의 재현 명령이 실행되지 않는다 | **closed** (재검증 4 — `.claude/skills/game-ops-harness/scripts/validate-preproduction.mjs` 실재·실행 확인) | game-product-manager |
| C5-F8 | S3 | product | `business-model.md` §5 가정 줄 ↔ 덱 23번 표 ↔ `validate-preproduction.mjs` economy 절 | §5 가정 = `r=0.08, c=0, s=0.70, v=500` — **할인율 없음**(실제 정가 기준). 덱·검증기는 **할인 10% 기준**. 같은 이름의 값이 둘: `B2 본당 수취(k=1)` **9,745**(문서) ↔ **8,721**(덱), `B1 현금 3,000만 회수` **3,649본** ↔ **4,082본**. QA 재계산 결과 **양쪽 다 산술은 정확** — 라벨만 없다. 문서 말미 "정가 기준 표와 할인 기준을 섞지 않는다"가 정작 §5 자신에는 적용되지 않았다 | **closed** (재검증 4 — §5 라벨 + §5-1 병기. **QA 독립 재계산 36값 중 표본 12/12 일치**) | game-product-manager |
| C5-F9 | S3 | product | `grep -rn "14,900\|14900" planning/gdd.md` (0건) ↔ `grep -rn "14,900" _workspace/archive/*/planning/gdd.md` | `business-model.md` §2 B1 근거 = "**GDD 상품가설 승계**". live `gdd.md` 에 그 숫자 **0건** — 같은 문서 L24·L265 가 "가격 숫자 삭제, PM 문서 인용으로 대체"를 명시. 실제 출처는 `_workspace/archive/20260909-preproduction-c{2,3}/planning/gdd.md` **L29**. 계보는 실재하나 인용이 끊겼다 | **closed** (재검증 4 — live `gdd.md` §11 인용 + 아카이브 c3 계보 병기) | game-product-manager |
| C5-F10 | S3 | production (modeling 인용) | `python3` 로 `production-estimate.json` `newAssets`·`artDays` 합산 ↔ `modeling/asset-manifest.md` §6 ↔ `find assets/generated/2d -type f ! -name '*.json' \| wc -l` | 수량·인일은 **전건 일치**(newAssets 합 **47** = 셸5/도구6/초상5/공용30/UI1, artDays 합 **63** = 25+18+9+8+3, 단계 합 267+12+8=**287**, ×1.25=**359**, ×300,000=**107,700,000**, 17.9/12.8개월). 문제는 **진척 문장의 부재** — 47종 중 `greybox 7 / pending 40 / 생산 완료 0`(asset-manifest §6, 전건 `runtimeEligible:false`)이 견적 문서에서 읽히지 않는다. 생성된 2D **45장**·3D 12메시 **144 tris**·영상 **2클립**은 컨셉·프리비즈이며 47종의 진척이 아니다(`concept/generation-manifest.md`: "전량 컨셉/프리비즈다. 게임플레이가 아니다") | open | game-production-director |
| C5-F11 | S4 | product | `grep -n "포커스" presentation/steam-game-plan.meta.md` · `sed -n '/## 기하 측정/,/조작 계약/p'` | `skill-application.md` "measured-ui-callouts … **넘침과포커스** 검수" 중 **포커스 축의 측정 기록이 0건**이다. 메타 「기하 측정」은 넘침만(4뷰포트 × 36장 → 0장), 「조작 계약」은 `inert`/`aria-hidden` **선언**이지 측정이 아니다. 넘침 축은 실제로 측정됐으므로 관찰로 둔다 | **closed** (재검증 4 — "포커스 순서·초점 트랩은 측정하지 않았다"로 정정) | game-product-manager |

### C5 집계

| severity | 신규 | closed | open | open-rfc |
|---|---:|---:|---:|---:|
| S1 | **0** | 0 | **0** | 0 |
| S2 | 4 | **1** (F1) | 3 (F2·F3·F6) | 0 |
| S3 | 6 | 0 | 6 (F4·F5·F7·F8·F9·F10) | 0 |
| S4 | 1 | 0 | 1 (F11) | 0 |
| **합계** | **11** | **1** | **10** | **0** |

이전 집계(재검증 1 이전) `[CARRIED]`: 신규 11 · closed 0 · open 11.

**재검증 2 [OBSERVED 2026-09-10]**: R5 축 배정 **0건**, 상태 변경 **0건**(위 표 그대로 `[CARRIED]`). 바뀐 것은 **C5-F5 의 범위**뿐이다 — 폐기 용어 「매체 경로」 본문 사용이 덱 2곳이 아니라 **12곳 / 9파일**(systems 6 · economy 2 · balance 1 · presentation 2 + 렌더 1)이며 소유 레인이 4개로 늘었다. 덱 HTML 은 **바이트 동일**(`0468eab2…` · 100,041 B) — R4 루프 2 편집은 R5 산출물을 움직이지 않았다. 근거·명령은 `qa/c5-review.md` **「재검증 2」** D2-0~D2-4.

**판정 SPEC-FIX.** (재검증 1 후 open S2 **3건**: F2·F3·F6 — 전부 director 소유.) 열린 S1 0건이지만 open S2 4건이 전부 *대외로 나가는 문서*(브리핑 덱 · 사이클 대장 · 루트 README)에 있고, 그중 둘은 **없는 것을 있다고 적거나 폐기된 범위로 착수 승인을 요청**한다. SPEC-REDO 가 아닌 이유: 산술·데이터 파생 축은 **전건 일치**했고 문제는 손으로 굳힌 문자열 5~6곳이다.

**C6 인계 우선순위(재검증 1 갱신)**: ~~C5-F1~~(closed) → **C5-F2** → C5-F3 → C5-F6 → **C5-F5 + C5-F4 를 한 묶음으로**(둘 다 덱 재빌드 필요 — 묶으면 HTML sha 갱신이 한 번으로 끝난다) → C5-F8 → C5-F7 → C5-F9 → C5-F10 → C5-F11.

이전 우선순위 `[CARRIED]`: **C5-F1** → C5-F2 → C5-F3 → C5-F6 → C5-F5 → C5-F4 → C5-F8 → C5-F7 → C5-F9 → C5-F10 → C5-F11. (F1·F2 는 덱 재빌드가 필요하므로 묶어 처리)

### C5 브로드캐스트 (dependency-matrix ● 항목) · `feedback-requested-by: 2026-09-11`

| 받는 레인 | 결함 | 요청 |
|---|---|---|
| game-presentation-director | **C5-F1**, C5-F4, C5-F5 | 26번 T0 범위 데이터 파생화 + `SLICE_ZONE_COUNT` 삭제 · 17번 테스트 수 동적화 · "매체 경로" 정본 표현 교체 + 폐기 문구 게이트 추가 · 재빌드 후 메타 r3 campaign 행 재측정 |
| game-production-director | **C5-F2**, **C5-F3**, C5-F6, C5-F10, RFC-Q3 | 대장 갱신 후 덱 재빌드 · `c5-development.md` 작성 · README `handoff/` 서술 정정 · 견적 메타에 진척 문장 1줄 |
| game-product-manager | C5-F7, C5-F8, C5-F9, C5-F11 | 재생성 경로 정정 · §5 표에 "할인 0% · 정가 기준" 명시 · B1 근거를 아카이브 경로로 · "포커스 검수" 문구 측정 또는 삭제 |
| game-systems-designer | C5-F5(잔존) | `system-specs/{plate-readout,drainage-routing}.md` · `data-schemas/beats.md` 의 "매체 경로" 3곳은 systems 소유. 폐기 선언(§5 L69)과 실제 사용의 어긋남을 판정하고 `c4-self-check.md` F3 2행에 반영 |
| game-modeler | (참조) | C4-review **F4 는 T0 축소로 해소** — `asset-budget.md:24` 수정 **불필요**. C5-F10 은 `asset-manifest.md` §6 을 *인용*하는 형태라 modeling 문서 수정 없음 |
| game-planner | (참조) | live `campaign.json` = `92301c0a…` · 121,457 B · **47/47**. 고정 숫자 재기재 금지, 검증기 출력 인용(RFC-Q1) |

**RFC-Q3 (qa → director) [신규, R5]**: **루트 `README.md` 의 소유 레인이 계약에 없다.** CLAUDE.md §1 표는 `_workspace/current/` 폴더만 배정하고 루트 README 는 2차 요청으로 생겼다. README 는 13개 레인의 수치를 한곳에 모으는 대외 문서인데 소유자가 없어 C5-F6 같은 스테일이 **어느 레인의 회귀 대상도 아니다**. 판정 요청: README 를 디렉터 소유로 두고 §7 사이클 종료 체크리스트에 "README 주장 재측정"을 넣을 것인가. QA 는 README 를 쓰지 않으므로 스스로 정할 수 없다.

**QA 자기정정 (R5) [OBSERVED]**: `gate-measurements.md` §0 의 "정본 입력" 줄과 `#g7` `command` 블록이 C3 종료 시점 값(`fdabf1d4…` · 120479 · 44/44)을 그대로 인용해 **스테일이었다** → live 재측정값(`92301c0a…` · 121457 · 47/47)으로 교체했다. 같은 문서 `#g8` 의 `artifacts_scanned: 91` 도 재실행값 **92**(본 파일 작성 후 **93**)로 갱신했다. 세 건 모두 **다른 레인이 아니라 QA 문서가 스테일 측**이었다 — 재검증 3에 이어 두 회차 연속이며, 원인은 같다(값을 고정 문자열로 적었다). RFC-Q1 의 "검증기 출력을 인용하고 고정 숫자를 재기재하지 않는다"를 QA 문서에도 적용해야 한다는 뜻으로 읽는다.

---

## 6. C4 (R4) 결함 표 — 상호작용 · 연출 · Unity 슬라이스 · 에셋 [2026-09-10]

- 근거 전문: `qa/c4-review.md` **「재검증 (2026-09-10, R4)」** 절, 동일 id.
- **`qa/c4-review.md` F1~F4 는 4건 전부 `closed`** [OBSERVED, R4.1]. 아래는 그 재검증 중 새로 발견한 것이다.
- repro 는 전부 **문서·데이터 대조**다. 빌드 0줄 · 플레이 표본 n=0 이므로 런타임 재현 절차를 가진 결함은 **0건**이다.
- 재측정 명령 원문은 `c4-review.md` §R4.0 의 **Q1~Q14**. 정본 입력 [OBSERVED 2026-09-10]: `node planning/validate-campaign.mjs` → `{checks 47, pass 47, fail 0, verdict PASS}` · sha `92301c0a…` · 121,457 B · `freshness-check.sh` 0 finding / **92** artifacts / exit 0 · `prototype/test-model.mjs` 37/0.

### 6.1 C4 1차 결함의 재판정

| id | severity | lane | repro (파일·절) | evidence | status | owner |
|---|---|---|---|---|---|---|
| C4-F1 | S1(1차 blocker) | systems | `interaction-rules.md` §0-8·§0-9 · §1 표 전 10행 · §1-1 · `game-ui-contract.json` `accessibility.{hold_alternative,keyboard_only}` · `screens[3].states` · `matrix[10~12]` · `acceptance[5]` | Q10: `X` = 프리뷰 전용 2행뿐, 해제는 `Delete`/패널 내 `Y` — **동시 활성 0건**. Q11: "확정은 길게 누름을 요구한다" **0행**. 키보드 열 전 행 존재. 인수 기준은 바꿔치기 없이 `acceptance[5]` 추가로 확장 | **closed** | game-systems-designer |
| C4-F2 | S1(1차 blocker) | systems / animation | `unity-implementation.md` §7·§9(L112)·§11(T-15~T-20) · `interaction-rules.md` §0-10·§5·§6 · `animation-contract.md` L24~L28 · `screens[5]`(4상태)·`screens[14]`(저장 중·저장 실패) · `data_bindings[9]` | 체크포인트가 실제 파일(`checkpoint.pre-commit.json` + `checkpointRefs[]`), "자동 1슬롯" 문장 **0행**, 1프레임 잠금 **명시적 폐기**, 축 분리(렌더 ack 1프레임 / I-O 200 ms), 쓰기 실패 주입 테스트 **6건** | **closed** | game-systems-designer |
| C4-F3 | S2 | systems | `interaction-rules.md` §3·§3.1 · `unity-implementation.md` §5 불변식1·L69·§6 · `T-21~T-23` · `matrix[15][16]` · `acceptance[7]` · `data_bindings[3]` | 판정이 한 문장(`sourceType` 상이 **AND** `originId` 상이), "매체 경로" 세 번째 용어 **명시적 폐기**, Q3: 검증기 `C-07` = `proofRequired` 15비트 **전건 독립쌍 존재 PASS 15/15**. 1차의 (b) 지적은 QA 자기정정으로 **부분 철회**(매체 중복 거부는 장르 규칙) | **closed** | game-systems-designer |
| C4-F4 | S2 | systems / modeling / concept | `unity-implementation.md` §10 · live `campaign.json` T0 · `modeling/asset-budget.md` L24 | Q4: `T0.minutes 25` · `zoneIds ["hub"]` · `t0-b2 ["circuit"]` · `t0-b3 ["reader","circuit"]`. 3자 일치. QA 재검산 셸 5 / 도구 6 / 초상 5 = 합계 계약과 일치 → "+63% 과소 산정" 소멸. **QA 1차 전제 정정**: `concept/*` 는 T0 를 0회 언급하므로 세 번째 자리는 art-direction 이 아니라 **live JSON** | **closed** | game-systems-designer |

### 6.2 R4 신규 결함

| id | severity | lane | repro (파일·절) | evidence | status | owner |
|---|---|---|---|---|---|---|
| C4-F5 | S2 | systems | (재검증 1) `grep -n "LB\|RB\|RS\|back"` + `python3 json.load(game-ui-contract.json)` + `validate-game-ui.py` | 요구 3항 전건 이행 [OBSERVED 2026-09-10]: (1) `LB` 단독 기능 제거·순환은 `RB` 단독(L31·L43·§1-2 L68) (2) `back`/`LB`+`back`/`LB`+`RS` **3기능 3바인딩**(L38) (3) `verification.matrix` **18행**으로 확장, `matrix[17]` 이 "도구 순환과 되돌림이 동시에 발행되지 않는다"를 검사 행으로 세움(기존 17행 삭제 0건, 스키마 검증기 exit 0). 가산: §0-11 불변식·§1-2 누른 순간 고정 규칙 신설. 파생 정정 2건(`hint-system.md` L25 · `plate-readout.md` L29)도 해소의 일부로 **수용**. **`matrix[17]` 는 미실행**(패드 실측 0건) | **closed** | game-systems-designer |
| C4-F6 | S2 | systems | `unity-implementation.md` §7 저장 스키마 ↔ `systems/data-schemas/save.md`(**status: current**) §1·§2 | `chapter`/`dayIndex`/`propertyProtection: false` ↔ `stageId`/`beatId`/"`dayIndex` 없음"/`propertyProtection` enum(`lowland\|dock\|null`). `checkpointRefs` 는 `save.md` 에 부재. CLAUDE.md §9 세이브 필드 불변식 구역. 레인 자체 발견 SC-1 과 동일 | **closed** (재검증 3 · R7 — RFC-S3 적용, `unity-implementation.md` §7 이 `save.md` §1·§3 과 일치. 잔여 UI 라벨 「일차」는 **C7-F40** 으로 분리) | game-production-director |
| C4-F7 | S2 | vfx / motion | `vfx/vfx-budget.md` 전문 · `motion/motion-contract.md` 전문을 `interaction-rules.md` §0-10·§5, `animation-contract.md` L28 과 대조 | animation 만 "권위 있는 연출은 영수증 이후" 를 명문화했다. vfx 는 `seal_confirm`·`water_rise`·`brine_flow` 를 선언하면서 영수증·`SavePending`·저장 실패를 **0회** 언급, motion 은 `SavePending` 진행 표시 규격이 없고 "상태가 먼저 확정" 문구가 §0-10 이후 유효한지 미기재 | **closed** (재검증 3 · R7 — `vfx-budget.md` §1~§2 영수증 게이팅·저장 실패 미발행·상태명 고정, `motion-contract.md` §2 옛 순서 폐기 + §3 `SavePending` 진행 표시 규격) | game-vfx-artist · game-motion-designer |
| C4-F8 | S2 | systems | (재검증 1) `grep -n "30분\|captureScenes" tech-verification/README.md` ↔ 정본 3자 | §2.6 = "**T0 25분**(정본) + `c3-b2`~`c3-b3`(`dock`)" · §2.5 = `-captureScenes hub` [OBSERVED 2026-09-10]. 정본 3자 재현: §10 L115 "25분" · 검증기 `stageMinutes[0]=25` · `campaign-time-budget.md` L359. 판정 키 `total_minus_afk_min`(RFC-P3-011) 추가 · §5 정정 로그 신설. 잔존 "30분" 2건은 **정정 이력 서술**이며 지시문에는 0건. **고친 것은 실행되지 않은 명령의 인자** — 새 런타임 값 0건, `PRE-1` 미정 `[CARRIED]` | **closed** | game-systems-designer |
| C4-F9 | S3 | worldview / planner / systems / concept | `gdd.md` §4 · `style-guide.md` §9 · 루트 `README.md` L33~38 ↔ `interaction-rules.md` §2 제목 · `game-ui-contract.json` `screens[9].states` ↔ `worldview/glossary.md` 행 머리 `grep -c` | 도구 표시명 **세 벌**. 용어집 미등록: `배선 추적 0` · `배수 편성 0` · `부식 시험 0` · `경로 구성 0`(Q12). systems 라벨 `부식예산` 은 **자원 이름과 충돌**하며 `gdd.md` §4.1 이 그 혼동을 막으려 바꾼 이름을 되돌린다. 용어집 규칙은 "UI 문자열에 미등록 명사 금지" | **closed** (재검증 3 · R7 — 표시명 6/6 + 용어집 등재 완료. `부식예산` 은 자원 이름 자리에만 남음. R7-23) | game-worldview-architect |
| C4-F10 | S2 | modeling | (재검증 1) `find` 3종 · `sed -n '148,158p' worldview/glossary.md` · 그레이박스 9색 ↔ 팔레트 집합 비교 · `atan` 재계산 | 스테일 3건 실측 교체 확인 [OBSERVED 2026-09-10]: sheets 파일 **1** · PNG **45** · provenance **6**(참인 값 `scene-boards 0` 은 유지) · glossary §7 파생 규칙 "OPEN-M1 닫힘" · `style-guide.md` 존재. OPEN-M1·M2·M3 closed, OPEN-M4 **부분 해제**(§0.1 대응표가 행별 소유), OPEN-M5·M6 신설. QA 독립 재현: 그레이박스 9색 ↔ 팔레트 교집합 **0/9** · 부감각 **34.998°**. 수량 계약 `5+6+5+30+1=47` 불변. **QA 자기정정**: 요구 문구 "§9 1~3 선행"은 과잉 — 1·2순위(README 삽화·Steam 캡슐)는 모델링 입력이 아니고 매니페스트 대응 행 0개, 실제 차단은 3순위 하나(레인 counter 수용) | **closed** | game-modeler |
| C4-F11 | S2 | concept / presentation / director | `concept/sheets/README.md` §6 `readme-verb-seal` 행 · §9 1순위 ↔ `ls docs/media/` ↔ 루트 `README.md` L38 | 자기 레인이 "§10-4 위반, 재생성 우선순위 1위"로 표시한 이미지의 파생본 `docs/media/verb-seal.jpg` 가 **push-ready README 에 게시**돼 있다. `style-guide.md` §10 = "위반 시 REDO, **예외 없음**" | **closed** (재검증 5 · R7c — 파생본 sha `c685b2c0…` 가 provenance 기재값과 **일치**, mtime **10:47 > 원본 08:57**, 원본과 픽셀 차 **1.266/255** → 옛 이미지가 아니다. 책자 펼침면 확대 판정 **글자꼴·자소 0** 이라 §10-4 위반 사유 소멸. R7c-1~R7c-5) | game-concept-artist |
| C4-F12 | S2 | systems / planner | `planning/gdd.md` §8 13행 ↔ `game-ui-contract.json` `accessibility{}`·`layout`·`localization`·`screens` 전수 대조 | 미커버 3행: **색약 대체 팔레트**(`색약` 0회 · `색각` 1회는 `matrix[3].setting` 뿐, 3팔레트 순회 행 0) · **채널별 볼륨 3채널**(`볼륨` **0회**) · 연속값 **이산 대안** 3동사(`이산` 0회, 총론만). gdd §8 이 소유자를 `accessibility` 로 지정했으므로 결손은 계약 쪽 | **closed** (재검증 3 · R7 — `accessibility{}` 가 gdd §8 13행 전건 커버 + `gdd_coverage` 1:1 맵. R7-9) | game-systems-designer |
| C4-F13 | S3 | systems | `game-ui-contract.meta.md` L19~L27 표를 `shasum -a 256` 4종과 대조(Q7) | (재검증 1) 해시 표가 **4행 한 표로 복구**되고 `interaction-rules.md` 행이 재측정값 `3c901743…`·25,545 B 로 교체됨. QA `shasum -a 256` 4종 재측정 → **4/4 문자 일치** [OBSERVED 2026-09-10]. 단위도 `wc -c` 바이트 + 문자 수 두 열로 분리. 배정 밖이었으나 같은 편집에서 해소돼 측정으로 확인됨 | **closed** | game-systems-designer |
| C4-F14 | S3 | systems / director | `c4-self-check.md` §4 SC-2·SC-3·SC-4 를 실물에서 재확인 | SC-2 `matrix[12]` "도구 **휴**은"·`matrix[15]` "출처 중복**로**" 오타 2건 → **수정 승인**. **재검증 2 확대 [OBSERVED 2026-09-10 · X14]**: `sed -n '620p' game-ui-contract.json` = 「R1 X는 항상 프리뷰로 고정하고 해제를 우클릭과 **덴리트**와 패널 내 Y로 **옥긴다**」 — `덴리트`=`Delete` · `옥긴다`=`옮긴다` **오타 2건 추가 → 총 4건**. systems 레인 자기보고를 QA 가 원문으로 확인했고 **새 id 를 열지 않고 SC-2 에 합친다**(JSON 은 C4-F9·C4-F12 와 묶어 **한 번만** 열 것 — meta 해시 갱신 횟수를 늘리지 않기 위해서다). SC-3 §10 `C3-b2`/`C3-b3` 대문자 ↔ live `c3-b2`/`c3-b3`(C3-F30) → **수정 승인**. SC-4 RFC-P3-015 가 180초 정본을 `interaction-rules §4` 로 인용하나 그 파일에 `180` **0행**(실물은 `system-specs/hint-system.md`) → **디렉터 몫** | open (재검증 4 — SC-2 오타 4건·SC-3 대문자 **0행 확인**. 잔여 = **SC-4(디렉터 몫)**만) | game-systems-designer (SC-4 는 game-production-director) |
| C4-F15 | S3 | worldview / planner / concept | 용어집 행 머리 `grep -c` 후 사용처 역추적(Q12) | 미등록 3종: **`판독대`**(`campaign.json` L294·L778·L811·L817 = 정본 데이터 · `content-matrix.md` §2 · `verb-02` R1 — 용어집에는 `판독기`만) · **`조위관측판`** · **`성에선`**(둘 다 `art-direction.md` L22 · `style-guide.md` §8 · `sheets/README.md` §4) | open | game-worldview-architect |
| C4-F16 | S3 | systems | (재검증 1 확대) `system-specs/*.md` §1 입력 표 전수 파싱 ↔ `interaction-rules.md` §1·§1-2 | **범위가 `Space` 1키에서 5키·6스펙으로 넓혀졌다** [OBSERVED 2026-09-10]: `X` 가 6개 스펙에서 6가지 의미(가상 시험·시험 실행·프리뷰·자동 제안·구획 접기·재생) · `Y`(`plate-readout` L32 인용 고정) · `H`(`tide-alignment` L31 ↔ 가설판) · `I`(`wiring-trace` L31 ↔ 증거함) · `Space` 동상. `interaction-rules.md` §1-2 L93 이 이 잔여를 스스로 "아직 없다"고 고지. 요구: 표면 스코프(셸/도구 패널) 우선순위 1절 신설 후 각 스펙이 인용 | **closed** (재검증 4 — `interaction-rules.md` §1-3 신설 확인. 다만 §1-3.1 예외 문단이 §1-3.2 전수표와 자기모순 → 신규 **C7-F10**) | game-systems-designer |
| C4-F17 | S3 | vfx / modeling / animation | `asset-budget.md` L13~19 · `asset-manifest.md` §6 ↔ `vfx-budget.md` 6효과 ↔ `animation-contract.md` 클립 10종 | 예산표에 **VFX 카테고리·견적 인일 부재**. `드로콜 ≤150`(asset) ↔ `emitter ≤8 · 투명입자 ≤500`(vfx) 사이 배분 규칙 0. 클립 대상 프롭 미존재 3종: `drawer_open`(서랍) · `shutter_raise`(셔터) · `water_level`(수면, vfx `water_rise` 와 소유 중복). 나머지 7종은 대응 확인 | open | game-modeler · game-vfx-artist |
| C4-F18 | S3 | presentation / concept | `presentation/video-study.md` §채택 ↔ `concept/sheets/README.md` §7 시퀀스 표 | §채택 = "55~60초 **제목/플랫폼**" ↔ §7 = `previz-f08`·`f09` "55~60초 **서명대·서명**". 채택 순서에 서명 구간이 없고 9프레임에 제목 프레임이 없다. f01~f07 은 일치. 부수: `video-study.md` 는 `cycle: …-c2` · `status: draft` 인데 `style-guide.md`(c4 current)가 **캐논 출처**로 인용(C3-F33 유형의 C4 재발) | open | game-presentation-director |
| C4-F19 | S2 | systems | (재검증 1 신규) `system-specs/drainage-routing.md` L29·L31 ↔ `interaction-rules.md` L41 · §1 「연결 해제」 행 | L29 「연결 해제 \| 우클릭 \| **`X`**」 ↔ L41 "**`X` 단독은 모든 화면에서 프리뷰 전용** … **배선 해제는 `X` 를 쓰지 않는다**" · §1 정본 = KB `Delete` / 패드 패널 안 `Y`. 같은 파일 L31 「가상 시험 \| `Space` \| **`X`**」와 **한 표 안에서 겸용** — §0-11("명시되지 않은 겸용은 결함")이 정의한 상태이며 F1·C4-F5 와 **같은 유형의 3번째 재발**. 같은 행 KB 열이 "우클릭"뿐이라 그 표만 읽으면 키보드 경로가 없다(§0-8). 파일이 `status: current` 이므로 `interaction-rules.md` 승격 시 current↔current 모순. 요구: L29 패드 `X`→`Y`(패널 안), KB 열에 `Delete` 명시. **재검증 2 [OBSERVED 2026-09-10]**: L29 = 「연결 해제 \| 우클릭 / `Delete`(키보드 단독) \| **패널 안에서만 `Y`** \| `RemoveEdge`」 — **행 번호 불변**(제자리 치환), L31 「가상 시험 \| `Space` \| `X`」 유지. 스펙 41행 전수에서 「연결/배선 해제 = `X`」 **0건**(X8), 패드 `Y` 3행(drainage L29·L33 · plate L32). §2.4 L127 「해제는 우클릭 / `Delete` / 패널 내 `Y`」 및 계약 JSON L153 과 **4자 일치** — 계약 쪽은 이미 정본이었고 md 단독 스테일이었다(X14). **QA 전제 1건 자기정정**: `interaction-rules.md` 는 `current` 가 아니라 **`draft`** 이므로 모순은 승격 시점에 성립한다(등급 불변) | **closed** (재검증 2) | game-systems-designer |
| C4-F20 | S3 | systems | (재검증 1 신규) `grep -rn "길게" system-specs/` ↔ `interaction-rules.md` §0-9·§0-11·§1-2 L69 | 지속시간으로 분기하는 바인딩 **3건**이 §0-11 의 해소 방식 2가지(모드 분리·모디파이어) 어디에도 없다: §1-2 L69 「`RB` \| 도구 순환(짧게 다음·길게 이전) \| — \| **단일**」(명령 2개인데 "단일"로 표기) · `dual-seal.md` L30 · `tide-alignment.md` L30 「확정 \| `Enter` **길게 0.4 s**」. 뒤 두 건은 §0-9("길게 누름은 선택 기능")와 충돌하고 `dual-seal.md` 는 **같은 파일 L77** 과 자기모순. 본보기는 `drainage-routing.md` L32. 요구: §0-11 에 지속시간 레이어 명시 또는 `RB` 롱프레스 삭제 + 두 스펙 확정 행 통일 | **closed** (재검증 3 · R7 — 지속시간 분기 3건 → 모드 분리/모디파이어, 「누름 시간은 해소 방식이 아니다」 §0-11 명문화) | game-systems-designer |
| C4-F21 | S3 | systems | (재검증 1 신규) `system-specs/*.md` §1 입력 표 41행 KB 열 파싱 | **41행 중 21행**에 키보드 토큰이 없다(드래그·클릭·우클릭·휠·hover·"메뉴 → 슬롯") — `drainage-routing` 3 · `dual-seal` 3 · `plate-readout` 3 · `save-undo` 4 · `tide-alignment` 2 · `wiring-trace` 2 · `hint-system` 2 · `corrosion-budget` 2. **과장 금지**: `interaction-rules.md` §1 전역 대응으로 다수는 파생 가능하며, 결함은 "행별 파생 근거가 스펙에 없다"는 것. 파생 불가 3행(타임라인 드래그·휠 배율·곡선 클릭 ×3)은 **C4-F12 의 연속값 이산 대안 구멍과 동일** → 같은 편집에서 처리. **재검증 2 [OBSERVED 2026-09-10 · X15]**: C4-F19 수정으로 `drainage-routing` 3행 → **2행**(노드 연결 드래그 · 밸브 토글 클릭), 전체 **21행 → 20행**. 기계 필터(KB 열 백틱 없음)는 18행이며 `corrosion-budget` L47 `` `routing` ``·L50 `` `reader` ``(백틱이지만 키가 아니다) 2행을 더해야 20이 된다. **행 수 감소로 닫히지 않는다** — 요구는 행별 파생 근거 표기다. QA 는 이 작업을 **C4-F16 표면 우선순위 절과 같은 편집으로 묶는 것을 승인**했다(`c4-review.md` §V2.3-(3)) | **closed** (재검증 4 — §1-3.4 D-1~D-7 + 8스펙 §1-A 신설, KB 미표기 20행 전건 대응) | game-systems-designer |
| C4-F22 | S3 | systems | (재검증 2 신규) `interaction-rules.md` L86 · `tech-verification/c4-fixloop2-input-binding.md` W-2 의 필터 서술을 그대로 실행 ↔ 같은 문서의 결론 · `git status --short` ↔ 같은 파일 §5 의 주장 | **문서에 적힌 재도출 명령이 문서에 적힌 결론을 내지 않는다(2곳)** [OBSERVED 2026-09-10 · X6·X1]. (a) 두 문서가 「패드 열에 `` `X` `` 가 있는 행만 거르면 **6행**」이라 적었으나 문자 그대로 실행하면 **8행**이다(`dual-seal` L31 · `save-undo` L32 의 `LB+X` 가 함께 걸린다). 6행이 되려면 `&& $3 !~ /LB\+X/`(단독 `X` 한정)가 필요하고 두 문서 어디에도 그 조건이 없다. §105 가 그 6행을 「`X` 잔여 **전수**」라 부르므로 다음 회차는 전수가 틀렸다고 읽거나 `LB+X` 2행을 누락한다. (b) 같은 검증 파일 §5 「다른 레인 파일 쓰기 0건(`git status --short` 로 확인)」 — 실제 출력은 `_workspace/current/systems/` 를 **디렉터리 한 줄 `??`** 로만 내므로 그 명령으로 파일 단위 쓰기는 보이지 않는다(결론 자체는 해시로 QA 가 독립 확인 — **사실은 맞고 근거만 틀렸다**). **결론이 정확하고 서술만 부정확하므로 S3**. 다만 검증 파일은 `status: current` 이며 **재현되지 않는 영수증은 영수증이 아니다**(C4-F8·C4-F13 과 같은 위치). 요구: 필터에 조건을 적거나 **8행을 적고 단독 6 · 모디파이어 2로 나눠** 적을 것 · §5 근거를 `shasum` 전후 비교로 교체 | **closed** (재검증 4 — 필터 조건 `&& $3 !~ /LB\+X/` 명시 + 8행/6행 기재, §5 근거를 해시 전후 비교로 교체) | game-systems-designer |

### 6.3 C4 집계 [OBSERVED 2026-09-10 · 재검증 2 기준]

| severity | 1차(F1~F4) | R4 신규 | 재검증 1 신규 | 재검증 2 신규 | 합 | closed | open | open-rfc |
|---|---|---|---|---|---|---|---|---|
| S1 (1차 blocker) | 2 | 0 | 0 | 0 | 2 | **2** | **0** | 0 |
| S2 | 2 | 8 | 1 (F19) | 0 | 11 | **6** (F3·F4·F5·F8·F10·**F19**) | **4** (F7·F9·F11·F12) | 1 (C4-F6) |
| S3 | 0 | 6 | 2 (F20·F21) | 1 (**F22**) | 9 | **1** (F13) | **8** (F14·F15·F16·F17·F18·F20·F21·**F22**) | 0 |
| **합계** | **4** | **14** | **3** | **1** | **22** | **9** | **12** | **1** |

이전 집계(재검증 1) `[CARRIED]`: 총 21 · closed 8 · open 12 · open-rfc 1. 이전 집계(재검증 1 이전) `[CARRIED]`: 총 18 · closed 4 · open 13 · open-rfc 1.

**재검증 2 판정 요약**: C4-F19(S2) closed — 요구 3항(패드 `X`→`Y` · KB `Delete` · 가상 시험 `X` 유지)을 문자 그대로 이행했고 스펙 41행 전수에 「해제 = `X`」 **0건**. **open S2 가 5건 → 4건**이 됐다. 신규 C4-F22(S3)는 **수정 자체가 아니라 그 수정을 증명한 명령 서술**에서 나왔다 — 결론은 맞고 재현 절차가 틀린 유형이며, 값이 아니라 문장 2개를 고치면 닫힌다. **open 총수는 12로 변하지 않았다**(닫힌 1 · 열린 1).

**열린 S1 = 0** (C3 에 이어 C4 도). CLAUDE.md §6 의 S1 차단 사유는 없다. 그러나 `NOT-MEASURED`(빌드 0줄 · 표본 n=0 · `matrix` 18행 전건 미실행)와 open S2 5건이 남아 **PASS 가능한 게이트는 0개**다.

**C4 → C5 인계 우선순위(재검증 2 갱신)**: C4-F6(RFC-S3 판정) → C4-F11(push 전 결정) → ~~C4-F19~~ **closed** → **C4-F9 + C4-F12 + C4-F14 SC-2(오타 4건) = JSON 한 편집** → **C4-F16 + C4-F21 = §1 표 한 편집**(QA 승인) → **C4-F22**(문장 2개 교체, 가장 싸다) → C4-F7 → C4-F20 → C4-F17 → C4-F18 → C4-F15. (C4-F9·F12·F14 SC-2 는 **JSON 한 번 수정으로 묶고** meta 해시를 같은 편집에서 갱신할 것 — `game-ui-contract.meta.md` 「미묶음 경고」와 같은 결론)

### 6.4 승격 판정 요약 (C3-F33 · RFC-Q2) — 전문은 `c4-review.md` §R4.4

| 판정 | 파일 |
|---|---|
| **승격 가능** | `animation/animation-contract.md` · `economy/resources-and-fairness.md` · `systems/prototype/README.md` · `systems/prototype/prototype.meta.md` |
| **차단** | `systems/interaction-rules.md`(C4-F9·F5·F16) · `systems/unity-implementation.md`(C4-F6·F14) · `systems/game-ui-contract.json`(C4-F12·F9·F14) · `systems/game-ui-contract.meta.md`(C4-F13) · `motion/motion-contract.md`·`vfx/vfx-budget.md`(C4-F7) · `balance/puzzle-balance.md`(자체 "정본 아님" 선언, 의도된 draft 유지) · `presentation/video-study.md`(cycle c2 · C4-F18) |

**재검증 1 갱신 (2026-09-10) — 전문은 `c4-review.md` §V1.7 · `c5-review.md` §D3**

| 판정 | 파일 | 사유 |
|---|---|---|
| **승격 가능** (변동 없음) | `animation/animation-contract.md` `[CARRIED]`(이번 루프 불변, 해시 `ec3ad8c1…` 동일) · `economy/resources-and-fairness.md` `[CARRIED]` · `systems/prototype/README.md` · `prototype.meta.md`(37/37 재현) | — |
| **차단 유지** | `systems/interaction-rules.md` | C4-F5 closed 이나 C4-F9 · C4-F16 확대 · C4-F20(1) |
| **차단 유지** | `systems/game-ui-contract.json` · `game-ui-contract.meta.md` | (재검증 2: 해시 `9c89e9ae…` **불변**, meta 표 4행 문자 일치로 자체 스테일 0) C4-F12 · C4-F9 · **C4-F14 SC-2 오타 4건**(L620 「덴리트」·「옥긴다」 추가) **잔존 재확인**(`matrix[12]` "도구 휴은" · `matrix[15]` "출처 중복로"). meta 자체 결함 C4-F13 은 closed 이나 기술 대상이 차단 |
| **차단 유지** | `systems/unity-implementation.md` · `motion/motion-contract.md` · `vfx/vfx-budget.md` · `balance/puzzle-balance.md` · `presentation/video-study.md` | 사유 불변 |
| **차단 유지** | `presentation/generate-deck.mjs` · `steam-game-plan.html` · `deck-outline.md` · `steam-game-plan.meta.md` | C5-F1 closed 이나 **C5-F5**(17번 "기술 인수 테스트 14개" ↔ 정본 27 · 폐기 용어 "매체 경로" 1회, 재측정 확인) · **C5-F4**(메타 r3 표 고정 숫자) |
| **정정 완료 (이미 current)** | `systems/tech-verification/README.md`(C4-F8) · `modeling/asset-manifest.md` · `modeling/pipeline.md`(C4-F10) | 승격 대상이 아니라 정정 대상이었고 정정됐다 |
| **새 정정 대상 (이미 current)** | ~~`systems/system-specs/drainage-routing.md`(C4-F19)~~ **정정 완료 · 다만 같은 파일에 C4-F16(L31)·C4-F21(L28·L30)·C5-F5(「매체 경로」 2곳)가 열려 있어 검증 완료 아님** · `dual-seal.md` · `tide-alignment.md`(C4-F20) · `wiring-trace.md`·`plate-readout.md`(C4-F16) · **`systems/tech-verification/c4-fixloop2-input-binding.md`(C4-F22 · `status: current` 로 태어난 영수증)** | 재검증 1 측정으로 새로 열림 |

### 6.5 C4 브로드캐스트

`feedback-requested-by: 2026-09-11` · 레인별 요청 전문은 `qa/c4-review.md` **§R4.5**.

**QA 자기정정 (R4) [OBSERVED]**: (1) `c4-review.md` 1차 F3 의 (b) 지적("정당한 두 번째 염판이 잘못 거부된다")은 **부분 철회**한다 — 매체 중복 거부는 세계관 §2 "독립 매체 2종"에서 나온 **의도된 장르 규칙**이고, 실제로 막아야 했던 실패(사유 없는 차단)는 §3 L103 + `T-22` + `matrix[16]` 으로 닫혔다. (2) 1차 F4 가 "`art-direction.md:20` 이 T0 의 gate 기능을 규정한다"고 쓴 것은 과잉 연결이었다 — `concept/*` 는 T0 를 **한 번도 언급하지 않는다**(`grep -rn "T0" concept/` → 0행). 세 번째 대조 자리는 live `campaign.json` 이다.

---

## 7. C6 (통합 초안) 결함 표 — 5렌즈 판정단 병합 [2026-09-10]

- 근거 전문·evidence 원문은 **`qa/c6-review.md` §2**(동일 id). repro는 전부 문서·데이터 대조이며 런타임 재현 절차를 가진 결함은 **0건**이다.
- 대상: `planning/game-draft-v1.md`(405행) 및 그것이 인용하는 정본.

| id | severity | lane | 요지 | status | owner |
|---|---|---|---|---|---|
| C6-F1 | S2 | planner / synopsis | 첫 30분(T0+`c1-b1`)에 밤의 목표("제출 문서 1건")가 없다 — 「청문」 최초 등장 `c3-b4` · 「제출」 `c5-b4` | **closed** (재검증 3 · R7 — `t0-b1.objective` 에 「청문에 낼 제출 문서 1건」, 초안 §2.6 공개 상한 표에 등재) | game-planner |
| C6-F2 | S2 | planner / product-manager / synopsis | 상품 약속 "결론이 바뀌는 추리" ↔ 캠페인 전 분기 「비확장」(6/6 씬, `c7-b4` "새 씬은 갈라지지 않는다") | **closed** (재검증 3 · R7 — 정본 문구가 초안 §0·§2.3 · `business-model.md` §1 · `assumption-tests.md` L13 에 동일 문장. 「분기·멀티 엔딩」 금지 명문화) | game-production-director (판정) |
| C6-F3 | S2 | planner / balance | `hints[0]` 다수가 1단 정의를 어기고 퍼즐 인과·자료를 지정. 검증기 H-01은 빈 문자열만 검사 | **closed** (재검증 3 · R7 — 검증기 **`H-04`** 신설 + `hints[0]` 전수 재작성. QA 독립 금지어휘 스캔 **0건**. R7-10) | game-planner |
| C6-F4 | S2 | planner / systems | 세션 재개 요약이 어디에도 스펙되지 않음(`재개\|리캡\|resume` 0건) | **closed** (재검증 3 · R7 — `planning/feature-specs/recap-panel.md` 신설(4블록 · R1~R11 · E1~E8 · A/B 인수 분리), `gdd.md` §7 인용) | game-planner |
| C6-F5 | S2 | balance / planner | `subtasks`에 표·칸 포함 **21/33 비트**. C7 클라이맥스 손 조작 1비트. `routing`·`corrosion` 각 3비트 | **closed(위험 등록으로)** (재검증 3 · R7 — **R-T0-1** 등록 + `H-10`·`H-11` + `tool_panel_active_min`(판정선 없음). **판정문 「61 %」 재현 불가 → C7-F41**) | game-balance-designer |
| C6-F6 | S2 | product-manager / planner | 포지셔닝·가격 한 문장 부재. §8 가격 행 숫자 없음, `market-decision.md` 인용 0건 | **closed** (재검증 3 · R7 — `business-model.md` §1-1 포지셔닝 한 문장 + §2 가격 3숫자「미승인」 + `market-decision.md` 인용) | game-product-manager |
| C6-F7 | S2 | production-director | **decision-log에 RFC-C6-001·002·Q3·S2·S3·S5·S6·B6·M3·CX 전건 0건**인데 초안·핸드오프가 개설된 것처럼 인용 | open (재검증 3 · R7 — **범위 축소 · S3 하향 제안.** 10건 중 **9건 개설**, **`RFC-Q3` 만 0건**(루트 README 소유 레인 미정). R7-21) | game-production-director |
| C6-F8 | S2 | product-manager / concept / planner | 초안 위험 표에 Steam 생성형 AI 공개·라이선스 UNVERIFIED **0행** | **closed** (재검증 3 · R7 — `business-model.md` §9-1 위험 2행. AI 공개는 `[TARGET] 확인 과제`, 라이선스는 provenance 실측 인용 `[OBSERVED]` — QA 가 3행 문자 일치 확인) | game-product-manager |
| C6-F9 | S2 | production-director / systems / planner | 본 생산 승인 조건이 3문서 분산(초안 `handoff` 0회 · 50% STOP 0회) · alignment 스파이크 프로토콜 부재 | **closed** (재검증 3 · R7 — 계약 「## Base production gate」 절 신설, 초안 §6·브리프 §①·`verification-plan.md` 는 **인용만**(재서술 0곳)) | game-production-director |
| C6-F10 | S2 | planner / systems | T0 도구 2종 모두 `hasCommit=아니오`인데 §6가 확정·저장 롤백 테스트를 T0 인수로 올림 | **closed** (재검증 3 · R7 — `tools.json` `reader.hasCommit true` / `CiteToBoard`(`T0-05`), 초안 §6.1 이 `T-25`·`T-27` T0 범위 밖까지 명시) | game-planner |
| C6-F11 | S2 | systems / production-director | T0 착수 전 결정 4건(기준 HW·Input System·URP·asmdef) 초안 문자열 전건 0회 | **closed** (재검증 3 · R7 — 브리프 **§⑦-0** 「되묻지 않는다」 표 4행. 초안 문자열은 여전히 0회이나 실행자 독서 대상이 `handoff/` 로 확정됨. 초안 §12 색인 잔여는 **C6-F25**) | game-systems-designer |
| C6-F12 | S2 | systems / planner | "되돌림 상한 없음"이 로그 상한 50k/8MB·SV-F6 접힘을 누락 | open (재검증 3 · R7 — **범위 축소.** 브리프 §⑤-2·U-3·U-8·N-12 이행 확인. **초안은 `되돌림 상한`·`로그 상한`·`SV-F6`·`byteCap` 전건 0회** → 초안은 planner 소유이므로 **배정 레인 재지정 필요**. 상한 수치 counter 채택: 정본은 `byteCap` 6 MiB / `entryCap` 20,000) → **closed** (재검증 4 · R7b — planner 가 초안 §4.5 **L264** 에 병기: 되돌림 무제한 ↔ 로그 상한 별개 축 · `entryCap` 20,000 / `byteCap` 6 MiB · SV-F6 접힘 · 「50,000/8MB」 폐기 명시. R7b-6·R7b-7) | game-systems-designer |
| C6-F13 | S2 | systems | 저작 원본 → 런타임 테이블 파이프라인 미정의 · `mediaType`↔`sourceType` 이름 두 벌 | **closed** (재검증 1 · 2026-09-10) | game-systems-designer |
| C6-F14 | S2 | planner | `two-step` 순서 서술 반전(정본: 프리뷰 → 확정 버튼) | **closed** (재검증 1 · 2026-09-10) | game-planner |
| C6-F15 | S2 | planner | 연표·캐논 시각·순서 앵커 **0행** — 3장·6장 정답 판정 기준을 초안으로 알 수 없다 | **closed** (재검증 1 · 2026-09-10) | game-planner |
| C6-F16 | S2 | planner / synopsis | T0 서사 공개 상한(RFC-P3-012) **0행** | **closed** (재검증 3 · R7 — 초안 §2.6 허용/금지 2열 표, 값 소유는 worldview 로 남김) | game-planner |
| C6-F17 | S2 | planner / synopsis | 본문 장소 ≠ `zoneId` **4비트**(`c2-b4`·`c6-b4`는 스테이지 `zoneIds` 밖, `c1-b2`·`c5-b2`는 안). **C3-F29 승계, G1 유일 violation** | **closed** (재검증 3 · R7 — 스테이지 `zoneIds` 확장 + 검증기 **`Z-03`** 신설, `Z-01`·`Z-02`·`Z-03` 전건 PASS. R7-11. **G1 유일 violation 소멸**) | game-planner |
| C6-F18 | S3 | production-director (README 소유 미정) | README L11 "폐국을 3주 앞둔" ↔ 캐논 D-21 고지 / D-1 플레이 | open | game-production-director |
| C6-F19 | S3 | planner | 초안 §4.2·§11.2 #1의 C4-F9 서술이 정본보다 뒤처짐(표시명은 이미 6/6 통일) | open | game-planner |
| C6-F20 | S3 | planner | §2.1 "인용, 재작성 금지" 표의 2개 열이 축약 재작성됨 | open | game-planner |
| C6-F21 | S3 | planner | 무대 문장에 "사람이 무엇을 확인했는지"(K-01 대상) 누락 | open | game-planner |
| C6-F22 | S3 | planner | §2.3 결말 표 「무엇을 잃는가」가 파생문인데 출처 §6 · `[INFERENCE]` 미표기 | open | game-planner |
| C6-F23 | S3 | production-director / planner | §9에 일정·인력·첫 증거 비용·null 가정·원본 draft 표기 전부 없음 | open | game-production-director |
| C6-F24 | S3 | planner / product-manager | §11.3 미측정 목록에 법무·라이선스·파이프라인·플랫폼 범주 부재 | open | game-planner |
| C6-F25 | S3 | planner / production-director | §6가 draft·승격차단 정본을 확정처럼 제시 · §12에 `handoff/` 없음 · `c6-development.md` 부재 | open (재검증 4 · R7b — **범위 축소.** 3항 중 2항 해소: `cycles/c1~c7-development.md` 전건 존재 · 초안 §12 에 `handoff/` **3행** 등재. 잔여 1항 = §6 이 `systems/unity-implementation.md`(status `draft`)를 「(C4/C5 검증 대기)」 표기 없이 인용. R7b-9) | game-production-director |
| C6-F26 | S3 | product-manager / planner / balance | 환불 2시간 창 시점 분석 0건(초안 `환불` 0회) | open | game-product-manager |
| C6-F27 | S3 | concept / presentation / product-manager | 상점 자산 준비도 0(캡슐 로고 0 · 스크린샷 0 · 트레일러 0 · 29/45 해상도 불일치) | open | game-concept-artist |
| C6-F28 | S3 | product-manager | 상표·동명 검색 영수증 0건 · GRAC 경로 미수행이 위험 표에 없음 | open | game-product-manager |
| C6-F29 | S3 | planner | "숫자 재기재 않는다" 선언 ↔ DLC 가격은 기재하고 본편 가격만 누락 | open | game-planner |
| C6-F30 | S3 | planner | 인수 테스트 묶음 26/27(**T-12 누락**) · T-11·T-13 오분류 | open | game-planner |
| C6-F31 | S3 | systems | 저장 파일 명명 세 벌 + `save.md` 경로가 Windows 전용 | open | game-systems-designer |
| C6-F32 | S3 | systems | 확정 1회당 디스크 쓰기 횟수·스레드·예산 미정의 | open | game-systems-designer |
| C6-F33 | S3 | economy → worldview / synopsis | 에필로그 "보존 등급 문장 1줄" 축이 바이블 §6·A25에 없음 | open-rfc | game-economy-designer |
| C6-F34 | S3 | qa | `#g1` measured 값 스테일(F25 closed · F30 판정 완료) — **QA 문서가 스테일 측인 세 번째 회차** | **closed** (본 회차 교체) | game-qa |
| C6-F35 | S4 | balance / systems | 무진전 180초 제안을 끄는 옵션 부재 · 세션 상한 `null` | open | game-balance-designer |
| C6-F36 | S4 | production-director / systems | `routing`·`corrosion`(유일한 세계 상태 변경 동사)이 사람 검증 없이 본 생산 진입 | open-rfc | game-production-director |
| C6-F37 | S4 | planner | 인물 인지 범위(timeline §5)·세력 3행(bible §5) 미인용 | open | game-planner |
| C6-F38 | S4 | product-manager / production-director | 데모 정책 미정(퍼널은 데모 전제, 초안 언급 0) | open | game-product-manager |
| C6-F39 | S4 | production-director / synopsis | 현지화 자수 산정 0건 · `externalCosts: null` | open | game-production-director |
| C6-F40 | S4 | planner | Appendix A에 착수 차단 목록 없음 | open | game-planner |
| C6-F41 | S4 | product-manager / planner | (제안) 인건비 회수 판매량 역산 한 줄 부재 | open | game-product-manager |

### 7.1 C6 집계 [OBSERVED 2026-09-10]

| severity | 신규 | closed | open | open-rfc |
|---|---:|---:|---:|---:|
| S1 | 0 | 0 | **0** | 0 |
| S2 | 17 | 0 | 17 | 0 |
| S3 | 17 | 1 (F34) | 15 | 1 (F33) |
| S4 | 7 | 0 | 6 | 1 (F36) |
| **합계** | **41** | **1** | **38** | **2** |

---

## 8. C7 (Unity 핸드오프) 결함 표 — 3렌즈 반박 병합 [2026-09-10]

- 근거 전문은 **`qa/c6-review.md` §3**(동일 id). 대상: `handoff/{README,codex-unity-brief,verification-plan,asset-runbook}.md`.
- **C7 닫는 질문 "외부 실행자가 추가 질문 없이 착수할 수 있는가" = 아니오** (C7-F1).

| id | severity | lane | 요지 | status | owner |
|---|---|---|---|---|---|
| C7-F1 | **S1** | systems / planner / production-director | **T0 퍼즐의 인스턴스 데이터가 저장소에 없다** — `zones/plates/tools/hints/beats.json` `find` 0건, `campaign.json` 최상위에 없음. DoD 4는 캐논·수치 발명을 요구하고 README §2가 그것을 금지한다 | **closed** (재검증 3 · R7 — `systems/data/t0/**` 5테이블 + `.meta.md`, `--t0` **5/5 PASS**, QA 독립 재생성 **5/5 바이트 일치**. `qa/c6-review.md` §12.1 R7-3·R7-8) | game-production-director (판정) |
| C7-F2 | S2 | systems | 필수 패키지 **0/4 설치** · 추가 절차 미정의 · localization→addressables 전이 의존이 N-10과 충돌 | **closed** (재검증 1 · 2026-09-10) | game-systems-designer |
| C7-F3 | S2 | modeling / systems | 브리프 GLB 임포트 ↔ 런북 FBX 단일화 ↔ **`SM_Tool_*.fbx` 0건** | **closed** (재검증 3 · R7 — 런북 §3.3-0 「GLB 정본 · `SM_Tool_*.fbx` 만들지 않는다」, 실측 GLB 7 · FBX 1 · blend 1 로 판정과 일치) | game-modeler |
| C7-F4 | S2 | production-director / systems | asmdef **7분할 ↔ 5분할**(`Tide.Sim`↔`Tide.Domain`) · README 우선순위가 draft 문서를 current 위에 세움 · RFC-S2 미개설 | **closed** (재검증 3 · R7 — `unity-implementation.md` §2 7분할 재작성, `handoff/README.md` §1 이 `status` 열 명시하고 current 를 draft 위로) | game-production-director |
| C7-F5 | S2 | planner / systems / worldview | `campaign.json` EN 문자열 **0건** + T0 도구명 용어집 미등재인데 I-9·T-12가 fail-closed → DoD 4·7 도달 불가 | **closed** (재검증 3 · R7 — 용어집 §3 4건 신설 + §3-1 id↔KO↔EN 표(6/6), 브리프 `I-9`→`R-1`·`T-12` 완화. R7-23) | game-planner |
| C7-F6 | S2 | systems | 폴더 레이아웃 자기모순(§③ Tables JSON ↔ §④-1 ScriptableObject) + `_Project/` ↔ `Assets/Art/` | **closed** (재검증 1 · 2026-09-10) | game-systems-designer |
| C7-F7 | S2 | systems | `CommandEntry`에 **`payload` 없음**(해시만) + **`commitIdempotencyKey`가 `save.md`에 0건** → T-13/T-17/T-19 구현 불가. 명령 소싱/이벤트 소싱 이중 서술 | **closed** (재검증 1 · 2026-09-10) | game-systems-designer |
| C7-F8 | S2 | systems / planner | T0 확정 명령 0개인데 DoD 6·8이 확정 테스트 요구 · `t0-b1`~`b3` 완료 술어 0행 | **closed** (재검증 3 · R7 — `T0-01` 이 `commitCommandBeats ["t0-b3"]`·`completion` 비어있지 않음을 기계 검사. 초안 §6.1 완료 술어 3행) | game-systems-designer |
| C7-F9 | S2 | systems | T-26 문안이 T0 두 도구(`circuit`·`reader`)에서 성립 불가 — 정본 3곳이 같은 틀린 문안 공유 | **closed** (재검증 1 · 2026-09-10) | game-systems-designer |
| C7-F10 | S2 | systems | `interaction-rules.md` **한 파일 안 자기모순** — §1-3.1 오버레이 예외 ↔ §1-3.2 전수표(`I`·`H` **2키**) | **closed** (재검증 1 · 2026-09-10) | game-systems-designer |
| C7-F11 | S2 | systems | 브리프 텔레메트리 키 **7종 + `command_count`가 계약에 0건** → T-I6 fail-closed로 T0 도구 임포트 실패. `first_valid_action`은 반대로 브리프에 0건 | **closed** (재검증 1 · 2026-09-10) | game-systems-designer |
| C7-F12 | S2 | production-director / modeling | 런북이 실행자에게 decision-log 쓰기를 3곳에서 지시 ↔ README §3 "쓰지 않는다" | **closed** (재검증 3 · R7 — 런북 L92·L179·L381 + 영문 요약 L391 통일, `handoff/rfc-inbox/README.md` 신설) | game-production-director |
| C7-F13 | S3 | modeling | `FORCE=1`·`stage_90_save()`가 미추적 원본·provenance 항목을 덮어써 CLAUDE.md §2 위반 | open (S2→S3 · modeling 몫 해소 · 런북 2줄 잔여) | game-modeler |
| C7-F14 | S2 | systems / worldview | 브리프 §⑤-3 KO 실패 문장 7건·`ReasonCode`가 발명 문자열인데 DoD #5가 "그대로 구현" 요구 | **closed** (재검증 3 · R7 — 브리프 §⑤-7 실패모드→`ReasonCode` 1:1 표, KO 는 `strings/ko.json` 한 곳, DoD #5 완화) | game-systems-designer |
| C7-F15 | S3 | systems | README §2 "패키지 추가는 되묻는다" ↔ 브리프 DoD #2(같은 레인 두 문서) | open | game-systems-designer |
| C7-F16 | S3 | production-director / modeling | Higgsfield 크레딧 승인 주체가 "디렉터"(에이전트) — 사용자 승인이 필요한 유료 경로 | open | game-production-director |
| C7-F17 | S3 | systems | T-B1 빌드 명령·타깃·모듈 부재(`buildTarget` 0건), 호스트 macOS ↔ Windows 우선 | open | game-systems-designer |
| C7-F18 | S3 | systems | 액션 맵·액션·컨트롤 스킴 이름 미정(0건) → T-24~T-27 참조 식별자 없음, DoD 3 판정 불가 | open | game-systems-designer |
| C7-F19 | S3 | systems | `Tide.Tests.Perf`가 §③ 레이아웃에 없음 · 에디터 배치 캡처는 플레이어 프레임타임 아님 | open | game-systems-designer |
| C7-F20 | S3 | systems | 브리프 U-3 경로 오류 `prototype/prototype.meta.md`(실제 `systems/prototype/…`) — 인용 25건 중 유일 | open | game-systems-designer |
| C7-F21 | S3 | systems / economy / balance | `readBudget` 소유 레인이 둘 → 실행자가 RFC 보낼 레인 미결정 | open | game-systems-designer |
| C7-F22 | S3 | production-director | README가 `status: draft`인 `asset-runbook.md`를 "필독"으로 지정 | open | game-production-director |
| C7-F23 | S3 | systems / production-director | README §3 쓰기 표에 `assets/generated/**`·`docs/media/` 없음(런북은 쓰라고 지시) · RFC-M3 미판정 | open | game-systems-designer |
| C7-F24 | S3 | systems | `idleSeconds`·`afk_gap` 계층 미지정 → `Tide.Sim`에 시간이 들어가면 S-2/R-3 위반 | open | game-systems-designer |
| C7-F25 | S3 | systems | T-IMP-1이 고정 숫자 단언을 요구 ↔ 같은 문서의 RFC-Q1 선언 | open | game-systems-designer |
| C7-F26 | S3 | systems / planner | `beats.json`을 `campaign.json` **복사본**으로 지시 → 진실 둘(RFC-S4 드리프트 재발) | open | game-systems-designer |
| C7-F27 | S4 | systems | `offerMaxPerSession` 미정 노브가 브리프에 없음 · 체크포인트 라벨 「일차」(제거된 `dayIndex`) 잔존 | open | game-systems-designer |
| C7-F28 | S4 | systems | `design_*` 문서 상수를 T0 수집 키로 열거(완화: "문서 상수" 라벨 + 금지문 존재) | open | game-systems-designer |
| C7-F29 | S4 | production-director | `Tide.*` 접두어가 가제 파생 · G8 판정 기록 없음(위반은 아님) | open-rfc | game-production-director |
| C7-F30 | S4 | systems | §⑪-3이 맨 `mex` 실행 지시(완화: `[SKIPPED]` 탈출구 존재). 래퍼 `mex-agent-bin.sh` 경로 명시 필요 | open | game-systems-designer |
| C7-F31 | S4 | planner | `verification-plan.md` §1.1이 접근성 요구(민감 속성) 유형을 기록 — 텔레메트리 기록 금지·동의문 명시 필요 | open | game-planner |
| C7-F32 | S4 | systems | 실행 명령이 `$OUT/logs`·`$OUT/results` 존재를 전제(`mkdir` 0건) · `<repo>` 치환 미설명 | open | game-systems-designer |

**재검증 1 신규 5건 [2026-09-10]** — 근거 전문은 `qa/c6-review.md` §10.3. 전건 **수정 루프 1의 산물**이며 렌즈 발견이 아니라 QA 재측정에서 나왔다.

| id | severity | lane | 요지 | status | owner |
|---|---|---|---|---|---|
| **C7-F35** | **S2** | systems | **C7-F10 해소가 배정한 `Q`가 미사용 토큰이 아니다** — `zones.md` L53 `neighbors` = 「`Q`/`E` 순회」 ↔ `interaction-rules.md` §1-3.2 L139 `Q`@`Shell` = 명령 0개 ↔ 같은 파일 §1 L29 노드 이동 = `Tab`/`Shift+Tab`. 모순이 `matrix[19]` 인수 조건에 박혔다 | **closed** (재검증 2 · 2026-09-10 · `qa/c6-review.md` §11.2) | game-systems-designer |
| C7-F33 | S3 | systems | 브리프 §⑩-2 #0 하드 게이트 `grep -c … packages-lock.json` 이 중첩 `dependencies` 를 함께 세어 **과다 계수**(현 파일에서도 17개 id 가 2회 이상 등장) → 성공을 실패로 읽는다 `[INFERENCE]` | open | game-systems-designer |
| C7-F34 | S3 | systems | C7-F2/F6 해소가 추가한 `Tide.EditorTools` 가 `architecture-contract.md` **§2 모듈 경계표에 없다** — 참조 허용 범위·금지·의존 방향 미정의(트리 2곳에만 등장) | open | game-systems-designer |
| C7-F36 | S4 | systems | `tech-verification/c6-c7-fixloop1-systems.md` L100 의 `[OBSERVED]` "잔존 `mediaType` 2곳뿐" 이 재현되지 않는다(실측 8행/8파일, 전건 개명 이력 서술) | open | game-systems-designer |
| C7-F37 | S4 | systems | `save.md` §3.2 단위 혼용 — 「전체 ≤ 8 MB」 + 「`byteCap` 6 MiB」 + 「나머지 2 MB」 는 총량이 8 MiB 일 때만 성립 | open | game-systems-designer |
| **C7-F38** | **S3** | systems | **C7-F35 해소 편집이 자기 검증 영수증에 재현되지 않는 `[OBSERVED]` 를 새로 넣었다** — `data-schemas/zones.md` L58 「키 토큰 **2행**」 ↔ 실측 **3행**(Z-4) · 같은 파일 L59 「``grep -rn '`E`' systems/ handoff/`` = 부정 문장 1건 외 **0건**」 ↔ 실측 **28행 / 7파일**(Z-5) · 같은 값이 `tech-verification/c6-c7-fixloop2-systems.md` §1.2 에도 「2」로 박혀 있다. **결론(배정 0건 · `E` 무배정)은 QA 독립 재현으로 참**이며 거짓인 것은 인용된 **명령 출력값**이다 | **closed** (재검증 3 · R7 — `zones.md` §2.1 이 등장 횟수 → **배정 행 수**로 측정 대상 교체. QA 재실행 `E` **0행** · `Q` **2행** 문자 일치. R7-12) | game-systems-designer |

### 8.1 C7 집계 [OBSERVED 2026-09-10]

| severity | 신규 | closed | open | open-rfc |
|---|---:|---:|---:|---:|
| S1 | 1 | 0 | **1** (F1) | 0 |
| S2 | 13 + **1**(F35) | **7** (F2·F6·F7·F9·F10·F11·**F35**) | **6** | 0 |
| S3 | 12 + **2**(F33·F34) + **1**(F13 하향) + **1**(F38) | 0 | **16** | 0 |
| S4 | 6 + **2**(F36·F37) | 0 | **7** | 1 (F29) |
| **합계** | **38** | **7** | **30** | **1** |

**재검증 1 반영 [OBSERVED 2026-09-10]**: 신규 32 → **37**(+5, §8 표 말미). closed 0 → **6**. C7-F13 은 **S2 → S3 하향**(modeling 몫 해소 · `handoff/asset-runbook.md` 2줄 잔여). 검산 — S2 open = 13 − 6(closed) − 1(하향) + 1(F35) = **7** · S3 open = 12 + 2(F33·F34) + 1(F13) = **15** · S4 open = 5 + 2(F36·F37) = **7** · S1 open = 1 → 합 **30 open**. 총계 37 = closed 6 + open 30 + open-rfc 1(F29).

**재검증 2 반영 [OBSERVED 2026-09-10]**: 배정 1건(**C7-F35 S2**) → **closed**(`qa/c6-review.md` §11.2 — 5곳이 한 문장, `matrix[19]` 문안 확장, 채택 근거 `[OBSERVED]` 취소선 정정). 같은 편집이 **C7-F38(S3)** 를 새로 열었다 — 해소 편집 자신이 재현 불가 `[OBSERVED]` 2줄을 만들었다. 검산 — 신규 37 → **38**(+F38) · closed 6 → **7**(+F35) · S2 open 7 → **6** · S3 open 15 → **16** · open 합 **30 불변**(−F35 +F38). 총계 38 = closed 7 + open 30 + open-rfc 1(F29). **열린 S1은 1건(C7-F1)으로 불변**이며 이번 루프도 **어떤 게이트도 올리지 않았다**(Unity 실행 0회 · 키 입력 실측 0건 · `matrix[19]` 미실행).

---

## 9. 전 회차 통합 집계 [OBSERVED 2026-09-10 · 재검증 4 기준]

| 회차 | 총 | closed | open | open-rfc | 열린 S1 |
|---|---:|---:|---:|---:|---:|
| C3 | 36 | **32** | 1 (F35) | 3 (F22·F31·F33) | 0 |
| C4 | 22 | **12** | 9 (F7·F9·F11·F12·F14·F15·F17·F18·F20) | 1 (F6) | 0 |
| C5 | 11 | **9** | 2 (F2·F10) | 0 | 0 |
| **C6** | 41 | **4** (F34·F13·F14·F15) | **35** | 2 (F33·F36) | 0 |
| **C7** | **38** | **7** (F2·F6·F7·F9·F10·F11·**F35**) | **30** | 1 (F29) | **1 (F1)** |
| **합계** | **148** | **64** | **77** | **7** | **1** |

검산 [OBSERVED · 재검증 2 반영 2026-09-10]: 36+22+11+41+**38** = **148** · closed 32+12+9+**4**+**7** = **64** · open 1+9+2+**35**+**30** = **77** · open-rfc 3+1+0+2+1 = **7** · 64+77+7 = 148. C3 열린 항목은 **C3-F35 1건뿐**(S3으로 하향, `data-schemas/plates.md` L39 한 줄) — C3-F25·F27·F29·F30·F36은 이전 회차 closed.

**재검증 1(2026-09-10) 이동분**: C6 +3 closed(F13·F14·F15) · C7 +6 closed(F2·F6·F7·F9·F10·F11) · C7 +5 신규(F35 S2 · F33·F34 S3 · F36·F37 S4) · C7-F13 **S2→S3 하향**. **열린 S1은 1건(C7-F1)으로 불변**이며 어떤 게이트도 PASS로 올라가지 않았다. 전문 = `qa/c6-review.md` §10.

**재검증 2(2026-09-10) 이동분**: C7 **+1 closed**(F35) · C7 **+1 신규**(F38 S3). C7 총 37 → **38** · closed 6 → **7** · open **30 불변** · 전 회차 합계 147 → **148**, closed 63 → **64**. **열린 S1은 1건(C7-F1)으로 불변.** 전문 = `qa/c6-review.md` §11.

**판정 SPEC-FIX (재검증 2 기준 유지).** 열린 S1 1건은 **재작성이 아니라 디렉터 판정 1건 + 데이터 저작 1건**으로 닫힌다. 그러나 그 판정 전에는 `handoff/`가 목적을 수행하지 못하므로 **"Codex 착수 가능"이라고 적어서는 안 된다.**

**C6/C7 → 다음 회차 인계 우선순위**: **C7-F1**(디렉터 판정) → **C6-F7**(RFC 8건 실제 개설 — 다른 12건의 해소 경로가 여기에 걸린다) → C7-F5 + C7-F14 + C4-F9 잔여(**RFC-S6 한 편집**) → C7-F7 + C4-F6(**RFC-S3 한 편집**) → C7-F4(RFC-S2) → C7-F2 + C7-F3(착수 물리 차단) → C6-F2(약속↔설계 판정) → C7-F10 + C7-F11(systems 한 편집) → **C7-F38**(`zones.md` 괄호 2개 + `fixloop2` §1.2 한 편집 — S2 해소의 영수증이 걸려 있어 값이 싸다) → C6-F17(검증기 Z-03 추가와 묶음) → C6-F3(검증기 H-02와 묶음) → 나머지 S3/S4.

---

## 10. 재검증 3 (2026-09-10, R7 종료 수정 회차) — 최종 동기화

근거·명령 원문은 `qa/c6-review.md` **§12**(R7-1 ~ R7-23). **QA 는 이 회차에 `qa/` 밖의 파일을 쓰지 않았다.**

> **편집 사고 공시 [OBSERVED 2026-09-10]**: 이 절의 상태 동기화 1차 스크립트가 열 인덱스를 잘못 잡아 **30행의 `owner` 셀을 상태 문자열로 덮어썼다**. 같은 세션에서 되돌렸으며, `owner` 값은 (a) 덮어쓰기 **전에** QA 가 읽어 둔 행 원문과 (b) `C4-F20` 만 `qa/c4-review.md` **L521 배정 표**에서 복원했다. 이 파일은 git 미추적이라 **되돌림 근거는 이 문장과 위 두 출처뿐**이다 — `owner` 열에 이견이 있으면 해당 레인이 정정한다. 덮어쓰기 전 `status` 문자열(주로 `open` · `open-rfc` · R4 주석)은 **복원하지 않았다**; 현행 R7 판정으로 대체됐고 이전 판정 이력은 이 문서의 「재검증 N 반영」 문단과 `qa/c{3,4,5,6}-review.md` 가 갖는다.

### 10.1 이동분

- **closed 26건**: C7-F1(**S1**) · C7-F3 · C7-F4 · C7-F5 · C7-F8 · C7-F12 · C7-F14 · C7-F38 · C6-F1 · C6-F2 · C6-F3 · C6-F4 · C6-F5 · C6-F6 · C6-F8 · C6-F9 · C6-F10 · C6-F11 · C6-F16 · C6-F17 · C4-F6(open-rfc→closed) · C4-F7 · C4-F9 · C4-F12 · C4-F20 · C3-F31(open-rfc→closed).
- **open 유지 4건(범위 축소)**: C4-F11(파생본 1개) · C6-F12(초안 병기) · C6-F7(RFC-Q3 1건) · C5-F2(생성기 + 덱).
- **신규 6건**: C7-F39(S3 planner) · **C7-F40(S2 systems)** · C7-F41(S3 director) · C7-F42(S3 balance) · C7-F43(S3 director, open-rfc) · C7-F44(S3 director).

### 10.2 신규 결함 행

| id | severity | lane | repro (파일·절) | evidence | status | owner |
|---|---|---|---|---|---|---|
| C7-F39 | S3 | planner | `grep -rn "RFC-P4-001" _workspace/current` (R7-14) | **RFC id 충돌.** `decision-log.md` L73 `RFC-P4-001` = **Mixamo 적용 범위**이고 12행이 그 뜻으로 쓴다. `planning/feature-specs/recap-panel.md` L27·L103 이 같은 id 를 「인용 고정의 저장 필드」라는 **다른 주제**로 연다 | open | game-planner |
| C7-F40 | **S2** | systems | `grep -rn "일차" systems/ handoff/ planning/` (R7-13) | **RFC-S3 반영 잔여 3행.** 브리프 L557 「UI 라벨에도 「일차」·「N일째」를 쓰지 않는다」 ↔ `interaction-rules.md` **L315** · `game-ui-contract.json` **L360**·**L396**. 두 파일 모두 승격 후보이며 `gdd.md` §7·`recap-panel.md` R2 는 장·조위 위상·플레이 시간 3항이다 | **closed** (재검증 4 · R7b — 지목 3행 전건 정정, `grep -rn "일차"` 잔여는 금지 규칙 인용문뿐. 영수증 문장 1줄은 **C7-F45** 로 분리) | game-systems-designer |
| C7-F41 | S3 | production-director | `python3` 두 분모 재계산 (R7-15) | **「61 %」가 어느 분모로도 재현되지 않는다** — 비트 **21/33 = 63.6 %** · 하위과제 **35/149 = 23.5 %** · 하위과제별 활동 분류 필드 **부재**. 초안 §11.3 이 「분모가 다르다」로 **오류를 확대**했고 같은 문자열이 `decision-log.md` L162 · `verification-plan.md` L74 · `telemetry-contract.md` L120(「61 %(21/33 비트)」 자기모순)까지 4문서에 전파 | open (재검증 4 · R7b — **범위 축소.** 초안 §11.3 L454 가 정본 **63.6 %(21/33)** + 「61 % 재현 불가」 로 정정돼 오설명 소멸. 잔여 = 소유 레인의 **3문서 문자열**(`decision-log.md` L162 · `verification-plan.md` L74 · `telemetry-contract.md` L120). R7b-8) | game-production-director |
| C7-F42 | S3 | balance | `balance-sheet.md` L58 ↔ L386·L396·L400·L463 ↔ `validate-campaign.mjs` 재실행 (R7-2·R7-16) | **「이 문서에는 고정 해시가 남지 않는다」(L58)가 거짓.** `92301c0a…`·`121457` 3곳 잔존이며 live 는 `8a43d334…`·**124,007 B**·**49검사**. L400 의 「R7 재실행 **동일 출력** 확인(47/47 · sha 불변)」 `[OBSERVED]` **재현 실패**. §7 대조 행 `proofRequired 15` ↔ live **17**. **난이도 지수 자체는 재현되므로 C3-F31 결론은 유효** — 틀린 것은 영수증 | open | game-balance-designer |
| C7-F43 | S3 | production-director | `node .claude/skills/game-ops-harness/scripts/validate-preproduction.mjs` (R7-5) | `status: FIX` 의 유일한 사유가 **errors 3**(`cycles/c6-development.md` · **`qa/c7-review.md`** · `cycles/c7-development.md`). QA 는 C6/C7 을 `c6-review.md` **한 문서로 병합**했다고 §0 에 고지했으므로 `c7-review.md` 를 만들지 않았다 — (a) 분리 문서 작성 (b) 검증기 목록이 병합 허용, 둘 중 하나를 판정해야 한다 | **closed** (재검증 5 · R7c — RFC 선택지 **(b) 검증기 병합 허용** 채택·구현: `mergedReview()` L11–15 + `production/cycles/c6·c7-development.md` 신설 → `status: SPEC-PASS` · `checks 514 / passed 514` · **`errors: []`** · exit 0. R7c-9·R7c-10) | game-production-director |
| C7-F44 | S3 | production-director | `scripts/regen-cycle-ledger.py` 정규식·파서 **읽기 전용 복제 실행** (R7-18) | **생성기가 등록부 4행을 조용히 버린다.** ① `^\| (C[1-7])-F(\d+)[^\|]*\|` 가 굵은 id 행(`| **C7-F35** |`·`| **C7-F38** |`)에 매칭 실패 ② severity 셀 `S1(1차 blocker)` 인 `C4-F1`·`C4-F2` 가 동치 비교에서 탈락. 등록부 **148행 → 생성기 144행**, 대장 C4 20(실측 22)·closed 10(12) · C7 36(38)·closed 6(7). **경고 없이 누락된다** | **closed** (재검증 5 · R7c — 등록 범위인 **4행 탈락**이 두 사유 모두 해소. 굵은 id(`**S2**`·`**S3**`) 와 `S1(1차 blocker)` 전부 파싱 · stderr **`dropped 0`** · 등록부 **155행 = 생성기 155행** · 회차별 total 전건 일치. **집계 오분류는 등록 범위 밖 → 신규 C7-F46 으로 분리.** R7c-6~R7c-8) | game-production-director |

### 10.3 전 회차 통합 집계 [OBSERVED 2026-09-10 · 재검증 3 기준]

| 회차 | 총 | closed | open S2 | open S3 | open S4 | open-rfc | 열린 S1 |
|---|---:|---:|---:|---:|---:|---:|---:|
| C3 | 36 | **33** | 0 | 1 (F35) | 0 | 2 (F22·F33) | 0 |
| C4 | 22 | **17** | 1 (F11) | 4 (F14·F15·F17·F18) | 0 | 0 | 0 |
| C5 | 11 | 9 | 1 (F2) | 1 (F10) | 0 | 0 | 0 |
| C6 | 41 | **16** | 2 (F7·F12) | 15 | 6 | 2 (F33·F36) | 0 |
| C7 | **44** | **15** | 1 (**F40**) | 19 | 7 | 2 (F29·F43) | **0** |
| **합계** | **154** | **90** | **5** | **40** | **13** | **6** | **0** |

검산: 36+22+11+41+**44** = **154** · closed 33+17+9+16+15 = **90** · open 5+40+13 = **58** · open-rfc 6 → 90+58+6 = **154** ✓ (재현 명령은 `qa/c6-review.md` §12.1 R7-17 의 엄격 파서)
직전(재검증 2) 대비: 총 148 → **154**(+6 신규) · closed 64 → **90**(+26) · **열린 S1 1 → 0**.

### 10.4 열린 S1 = 0 이 뜻하는 것과 뜻하지 않는 것

- **뜻하는 것**: CLAUDE.md §6 의 "열린 S1 이 PASS 를 막는다" 조항과 계약 「Base production gate」 ④의 절반(`열린 S1 0`)이 **문서 수준에서** 충족됐다. `freshness-check.sh` **exit 0** 과 함께 ④는 **충족**이다.
- **뜻하지 않는 것**: ①(T0 사람 검증 H-1~H-3, 표본 12명/5유형)과 ②(조위정합 스파이크 개념 검증)는 **시작조차 하지 않았다** — 표본 **n = 0** · 빌드 **0줄**. ③(150 % STOP)은 T0 착수 전이므로 평가 대상이 아니다. **네 조건 중 하나만 충족됐다.**
- **어떤 게이트도 이 회차에 PASS 로 올라가지 않았다** — `qa/gate-measurements.md` 최종 표 **0 / 8**. 열린 S2 **5건**이 남아 있다.

### 10.5 다음 회차 인계 우선순위

**C7-F40**(승격 2건이 이 한 줄에 걸려 있다) → **C7-F44 → 대장 재생성 → 덱 재빌드**(C5-F2 를 닫는 유일한 묶음) → **C7-F41**(4문서 한 문자열) → **C7-F42**(영수증만 교체) → **C4-F11**(파생본 1개) → **C6-F12**(초안 한 줄, 레인 재지정 후) → **RFC-Q3**(C6-F7) → **C7-F43**(판정) → C7-F39 → 나머지 S3/S4.

## 11. 재검증 4 (2026-09-10, R7b 마무리) — 승격 판정 회차

근거·명령 원문은 `qa/c6-review.md` **§13**(R7b-1 ~ R7b-11). **QA 는 이 회차에도 `qa/` 밖의 파일을 쓰지 않았다.** `owner` 열은 **한 셀도 건드리지 않았다**(§10 편집 사고 재발 방지 — 이번 동기화는 `status` 셀 전문 치환 4건 + 신규 행 1건뿐).

### 11.1 이동분

- **closed 2건**: **C7-F40(S2)** · **C6-F12(S2)**.
- **open 유지 · 범위 축소 2건**: **C7-F41**(잔여 = 디렉터 소유 3문서 문자열) · **C6-F25**(잔여 = 초안 §6 draft 인용 표기).
- **신규 1건**: **C7-F45(S4, systems)**.

### 11.2 신규 결함 행

| id | severity | lane | repro (파일·절) | evidence | status | owner |
|---|---|---|---|---|---|---|
| C7-F45 | S4 | systems | `cd _workspace/current/systems && grep -rn "일차" .` (R7b-1) | `game-ui-contract.meta.md` **L61** 의 `[OBSERVED]` 재현 주석 「→ `r7-systems-receipt.md:96` **한 행만 남음**」 ↔ 실제 출력 **6행**(자기 자신 L50·L54·L55·L57·L61 포함). **실질 판정(UI 라벨 0행)은 옳고 틀린 것은 영수증 문장**이다(C7-F42 와 같은 유형). 이 한 줄이 `game-ui-contract.meta.md` 의 `status: current` 승격을 조건부로 막는다 | open | game-systems-designer |

### 11.3 전 회차 통합 집계 [OBSERVED 2026-09-10 · 재검증 4 기준]

| 회차 | 총 | closed | open S2 | open S3 | open S4 | open-rfc | 열린 S1 |
|---|---:|---:|---:|---:|---:|---:|---:|
| C3 | 36 | 33 | 0 | 1 (F35) | 0 | 2 (F22·F33) | 0 |
| C4 | 22 | 17 | 1 (F11) | 4 (F14·F15·F17·F18) | 0 | 0 | 0 |
| C5 | 11 | 9 | 1 (F2) | 1 (F10) | 0 | 0 | 0 |
| C6 | 41 | **17** | **1 (F7)** | 15 | 6 | 2 (F33·F36) | 0 |
| C7 | **45** | **16** | **0** | 19 | **8** | 2 (F29·F43) | **0** |
| **합계** | **155** | **92** | **3** | **40** | **14** | **6** | **0** |

검산: 36+22+11+41+**45** = **155** · closed 33+17+9+17+16 = **92** · open 3+40+14 = **57** · open-rfc 6 → 92+57+6 = **155** ✓
직전(재검증 3) 대비: 총 154 → **155**(+1 신규) · closed 90 → **92**(+2) · **열린 S2 5 → 3** · 열린 S1 **0 유지**.

### 11.4 승격 판정 (근거는 `c6-review.md` §13.3)

| 파일 | 판정 |
|---|---|
| `systems/interaction-rules.md` | **승격 가능** |
| `systems/game-ui-contract.json` | **승격 가능** |
| `systems/game-ui-contract.meta.md` | **1행 조건부 차단** — C7-F45 |
| `product/business-model.md` | **1행 조건부 차단** — H1 「(C5 초안 준비물)」 ↔ `status: current` 모순(§0 본문은 정정 확인) |
| `planning/game-draft-v1.md` | 대상 아님(이미 `status: current`) |

### 11.5 다음 회차 인계 우선순위 (§10.5 갱신)

**C7-F45 + business-model H1**(승격 2건이 이 두 줄에 걸려 있다) → **C7-F44 → 대장 재생성 → 덱 재빌드**(C5-F2) → **C7-F41**(3문서 한 문자열) → **C7-F42**(영수증) → **C4-F11** → **C6-F25**(초안 §6 표기) → **RFC-Q3**(C6-F7) → **C7-F43**(판정) → C7-F39 → 나머지 S3/S4.

---

## 12. 재검증 5 (2026-09-10, R7c 디렉터 처리분) — 4건 판정 회차

근거·명령 원문은 `qa/c6-review.md` **「재검증 5 (R7c, 디렉터 처리분)」**(R7c-1 ~ R7c-14). **QA 는 `qa/` 밖의 파일을 쓰지 않았다** — 유일한 예외는 디렉터가 실행을 지시한 `scripts/regen-cycle-ledger.py` 의 부작용(`production/cycle-ledger.json` 재생성)이며 이는 명령 자체가 만든 것이다. `owner` 열은 **한 셀도 건드리지 않았다**(§10 편집 사고 재발 방지 — 이번 동기화는 `status` 셀 4건 + 신규 행 2건뿐).

### 12.1 이동분

- **closed 3건**: **C4-F11(S2)** · **C7-F43(S3, open-rfc)** · **C7-F44(S3)**.
- **open 유지 · 범위 축소 1건**: **C5-F2(S2)** — 잔여 = 덱 s19 재빌드 1건(선행: C7-F47).
- **신규 2건**: **C7-F46(S2)** · **C7-F47(S2)**.

### 12.2 신규 결함 행

| id | severity | lane | repro (파일·절) | evidence | status | owner |
|---|---|---|---|---|---|---|
| C7-F46 | S2 | director / production | 등록부 status 셀의 판정 토큰 다중 출현 스캔 — 생성기 정규식 읽기 전용 복제 (R7c-13) | **생성기가 첫 판정 토큰만 취해 서술형 셀을 오분류한다.** `C6-F12` 의 셀은 `open (재검증 3 …) → **closed** (재검증 4 …)` 인데 status 정규식이 **선두 `open`** 에 걸린다 → 대장 C6 `closed 16 / open_S2 2`, 등록부 §11.3 `closed 17 / open_S2 1`. **행이 버려지지 않으므로 `dropped` 경고도 뜨지 않는다** — C7-F44 와 같은 「조용한 불일치」의 다른 얼굴. 반대 방향(`closed → open` 서술)이 생기면 **닫힌 것으로 잘못 집계**되므로 방향 위험은 대칭이 아니다 | open → **closed** (재검증 6 · R7d — 생성기 규칙이 「첫 cell 이 상태 토큰으로 시작 + 그 cell 의 마지막 토큰」 으로 교체됐다. `python3 scripts/regen-cycle-ledger.py` 재실행 stderr `parsed 157 rows, dropped 0: []` · exit 0 · 등록부 행 157 = parsed 157 · 규칙 복제로 `C6-F12` 셀이 마지막 토큰을 취해 대장 C6 = `closed` 17 / S2 잔여 1 로 등록부 §11.3 과 일치. R7d-1~R7d-3) | game-production-director |
| C7-F47 | S2 | presentation / director | `presentation/generate-deck.mjs` L426–431 ↔ `production/cycle-ledger.json` `cycles[]` 키 · 두 파일 mtime (R7c-11·R7c-12) | **두 생성기가 같은 파일에 다른 스키마 계약을 쓴다.** 대장은 `schemaVersion 2`(`total·closed·open_S1·open_S2·open_S3plus·open_rfc`)인데 덱 생성기는 v1 필드 `c.findings`·`c.fixed`·`c.remaining` 을 읽고, **어느 쪽도 `schemaVersion` 을 검사하지 않는다**. 지금 재빌드하면 s19 의 발견·수정·잔여 **21칸이 전부 `미기록`**. 현재 html(06:28)은 ledger(10:56)보다 옛것이라 **옛 손기입 값**을 싣고 있다 → **C5-F2 의 잔여는 이것을 고치기 전에는 해소 불가**(재빌드가 곧 회귀) | open → **closed** (재검증 6 · R7d — `cycleSlideBody()` 가 대장 v2 9키(`total`·`closed`·`open_S1`·`open_S2`·`open_S3plus`·`open_rfc`·`status`·`review`)를 읽고 L448–459 에 `schemaVersion !== 2` 게이트를 둔다. v1 필드 3종 grep **0건**. 격리 사본에 `schemaVersion 3` 을 넣은 음성 시험에서 회차 행 **0** · 「schemaVersion 불일치(기대 2, 실제 3)」 렌더로 게이트 발동을 확인했다. 메타 r6 의 html 101,252 B / `f2028d3b…` · 생성기 110,835 B / `3bf99037…` 는 실측과 전건 일치. R7d-4~R7d-7) | game-presentation-director |

### 12.3 전 회차 통합 집계 [OBSERVED 2026-09-10 · 재검증 5 기준]

| 회차 | 총 | closed | open S2 | open S3 | open S4 | open-rfc | 열린 S1 |
|---|---:|---:|---:|---:|---:|---:|---:|
| C3 | 36 | 33 | 0 | 1 (F35) | 0 | 2 (F22·F33) | 0 |
| C4 | 22 | **18** | **0** | 4 (F14·F15·F17·F18) | 0 | 0 | 0 |
| C5 | 11 | 9 | 1 (F2) | 1 (F10) | 0 | 0 | 0 |
| C6 | 41 | 17 | 1 (F7) | 15 | 6 | 2 (F33·F36) | 0 |
| C7 | **47** | **18** | **2 (F46·F47)** | **18** | 8 | **1 (F29)** | 0 |
| **합계** | **157** | **95** | **4** | **39** | **14** | **5** | **0** |

검산: 36+22+11+41+**47** = **157** · closed 33+18+9+17+18 = **95** · open 4+39+14 = **57** · open-rfc 5 → 95+57+5 = **157** ✓
직전(재검증 4) 대비: 총 155 → **157**(+2 신규) · closed 92 → **95**(+3) · 열린 S2 3 → **4**(−1 C4-F11, +2 신규) · open-rfc 6 → **5**(−1 C7-F43) · **열린 S1 0 유지**.

### 12.4 대장(`production/cycle-ledger.json`)과의 차이 — 디렉터 조치 필요

R7c-6 으로 재생성된 대장은 **이 표보다 이전 상태**이고, 재생성해도 **C6 한 회차가 어긋난 채로 남는다**.

생성기 로직을 **읽기 전용으로 복제**해 편집 후 등록부에 돌려 본 값이다(파일은 쓰지 않았다 — 대장 재생성은 디렉터 몫). 복제 결과: `parsed 157 / dropped 0`.

| 구분 | 이 표(등록부 정본) | 읽기 전용 복제가 낸 값 | 차이 원인 |
|---|---|---|---|
| C3 | 36 / 33 / S3 1 / rfc 2 | **동일** | — |
| C4 | 22 / **18** / S2 **0** / S3 4 | **동일** | — |
| C5 | 11 / 9 / S2 1 / S3 1 | **동일** | — |
| C6 closed / open S2 | **17 / 1** | **16 / 2** | **C7-F46**(`C6-F12` 오분류) |
| C7 | 47 / 18 / S2 2 / S3 18 / S4 8 / rfc 1 | **동일** | — |
| 합계 closed | **95** | **94** | 위 1행 |

조치 순서(변경 없이는 다시 갈라진다): **C7-F46 수정 → `python3 scripts/regen-cycle-ledger.py` → C7-F47 수정 → 덱 재빌드 → C5-F2 재판정**. 세 단계를 한 묶음으로 하지 않으면 대장·덱·등록부가 다시 셋으로 갈라진다.

### 12.5 이 회차가 바꾸지 않은 것

- **게이트 0 / 8 불변.** 이번 회차의 측정은 전부 해시·픽셀·문서이며 **런타임 값은 0건**이다(`validate-preproduction.mjs` 자신이 `runtimeStatus: NOT-MEASURED`).
- `freshness-check.sh` **exit 0 / 0 finding / 120 artifacts** 는 프론트매터 계약과 `supersedes` 위상만 본 값이다 — `--since` 미지정으로 **시점 신선도 미측정**, `memory_sync` 영수증 **미검사** → G8 전체 PASS 로 읽지 않는다.

## 13. 재검증 6 (2026-09-10, R7d 최종 동기화) — 3건 판정 회차

근거·명령 원문은 `qa/c6-review.md` **「재검증 6 (R7d, 최종 동기화)」**(R7d-1 ~ R7d-11). **동기화 전용 회차라 신규 결함 0건**이다. **QA 는 `qa/` 밖의 파일을 쓰지 않았다** — 유일한 예외는 지시된 `scripts/regen-cycle-ledger.py` 의 부작용(대장 재생성)이며 그 결과는 실행 전후 **바이트 동일**이었다. 덱 재빌드는 `--out` 을 scratchpad 로 돌려 `presentation/` 을 쓰지 않았다. `owner` 열은 **한 셀도 건드리지 않았다** — 이번 편집은 **`status` 셀 3건뿐**이다.

### 13.1 이동분

- **closed 3건**: **C7-F46(S2)** · **C7-F47(S2)** · **C5-F2(S2)**.
- **신규 0건.**
- **기준 변경 수용 1건**: **C5-F2** 는 mtime 순서 대신 **내용 일치**(대장 63칸 ↔ 렌더 63칸 `diff` 0줄 + 체크인 html ↔ 재빌드본 바이트 동일)를 판정 기준으로 채택했다. presentation 레인 제안을 QA 가 값으로 확인한 뒤 수용한 것이다.

### 13.2 전 회차 통합 집계 [OBSERVED 2026-09-10 · 재검증 6 기준]

| 회차 | 총 | closed | open S2 | open S3 | open S4 | open-rfc | 열린 S1 |
|---|---:|---:|---:|---:|---:|---:|---:|
| C3 | 36 | 33 | 0 | 1 (F35) | 0 | 2 (F22·F33) | 0 |
| C4 | 22 | 18 | 0 | 4 (F14·F15·F17·F18) | 0 | 0 | 0 |
| C5 | 11 | **10** | **0** | 1 (F10) | 0 | 0 | 0 |
| C6 | 41 | 17 | **1 (F7)** | 15 | 6 | 2 (F33·F36) | 0 |
| C7 | 47 | **20** | **0** | 18 | 8 | 1 (F29) | 0 |
| **합계** | **157** | **98** | **1** | **39** | **14** | **5** | **0** |

검산: 36+22+11+41+47 = **157** · closed 33+18+10+17+20 = **98** · open 1+39+14 = **54** · open-rfc 5 → 98+54+5 = **157** ✓
직전(재검증 5) 대비: 총 **157 불변** · closed 95 → **98**(+3) · 열린 S2 4 → **1**(−3) · open-rfc **5 불변** · **열린 S1 0 유지**. 열린 S2 잔여는 **C6-F7 한 건**뿐이다.

### 13.3 대장(`production/cycle-ledger.json`)과의 차이 — 디렉터 인계

R7d-1 로 재생성한 시점의 대장은 **이 동기화 이전 상태**다(C5 `closed 9` · C7 `closed 18 / open_S2 2`). 생성기 로직 **읽기 전용 복제**(R7d-11, 파일 미기록)가 낸 재생성 예정값은 아래와 같고 위 §13.2 와 **전건 일치**한다.

| 구분 | 이 표(등록부 정본) | 재생성 예정값(읽기 전용 복제) | 디스크 대장(현재) |
|---|---|---|---|
| C5 총/closed | 11 / **10** | 11 / **10** | 11 / 9 |
| C7 총/closed/open S2 | 47 / **20** / **0** | 47 / **20** / **0** | 47 / 18 / 2 |
| C3·C4·C6 | 변화 없음 | **동일** | **동일** |
| 합계 closed | **98** | **98** | 95 |

조치 순서(둘을 한 묶음으로 — 나누면 대장과 덱이 다시 갈라진다): **`python3 scripts/regen-cycle-ledger.py` → `node _workspace/current/presentation/generate-deck.mjs --out <저장소>/…/steam-game-plan.html` → 63칸 `diff` 재확인**. 덱 s19 는 대장에서만 읽으므로 대장만 갱신하면 렌더 63칸이 즉시 어긋난다.

### 13.4 이 회차가 바꾸지 않은 것

- **게이트 0 / 8 불변.** 이번 판정의 측정은 전부 해시·바이트·문자열 대조이며 **런타임·플레이 값은 0건**(표본 n=0).
- `freshness-check.sh` **exit 0** 은 프론트매터 계약과 `supersedes` 위상만 본 값이다 — `--since` 미지정으로 **시점 신선도 미측정**, `memory_sync` 영수증 **미검사** → G8 전체 PASS 로 읽지 않는다.
- **열린 S1 = 0 유지.** 이 값은 「S1 을 찾지 못했다」는 뜻이지 「S1 이 없다」는 뜻이 아니다.

## 14. M1 코드 검토·native editor 재검증 (2026-09-10, C7 착수)

[OBSERVED] 기존 C7 핸드오프의 첫 구현을 QA가 독립 읽기 검토하고, systems가 수정 후 실행한 Unity 6000.5.6f1 native editor 계약검사 원문과 XML을 확인했다. 신규 3건: closed 2 · open S2 1. 과거 회차 판정·집계는 당시 측정값으로 보존한다. 앞머리의 「모든 repro는 문서 대조」는 이 M1 이전 회차를 뜻하며, M1 구현 재검증에는 적용하지 않는다. 결함 상태의 정본은 아래 표이고, 상세 근거와 source SHA는 [M1 독립 리뷰](t0-m1-review.md)에 있다.

| id | severity | lane | repro (파일·절) | evidence | status | owner |
|---|---|---|---|---|---|---|
| C7-F48 | S2 | systems | QA-M1-01. 초기 `unity/Unknown/Assets/_Project/Sim/T0Simulation.cs`의 CiteToBoard는 창 없는 인용을 허용하고 gapEndpointsFixed는 인용 시점 대신 변경 가능한 현재 창을 조회했다. 합성 출처에서 LoadRecord → Read → CiteToBoard → SetWindow로 확정 구간을 뒤늦게 채우거나 동일 끝점을 확정할 수 있음을 코드 읽기/상태 전이 추적으로 발견했다. | 수정본은 창 필수·동일 끝점 INDETERMINATE, 인용 이벤트의 start/end 스냅샷, 완료 술어의 스냅샷 조회를 구현했다. [XML](../systems/tech-verification/t0-m1/results/t0-m1-contract-checks.xml) `Synthetic_provenance_citation_captures_window_at_commit` 성공: 창 편집만으로 완료 불가, 명시 재인용 후 완료. [로그](../systems/tech-verification/t0-m1/logs/native-checks-3.log) · [source SHA](t0-m1-review.md#final-verification-receipt) | **closed** | game-systems-designer |
| C7-F49 | S3 | systems / qa | QA-M1-02. 초기 `Editor/T0Verification.cs` T05 루프는 Read/ToggleUncovered 10,000회 교대였으므로 [브리프 T-05](../handoff/codex-unity-brief.md)의 무작위 10k 요구와 달랐다. 코드 읽기 검토로 stress 범위와 인수 요구 차이를 확인했다. | 수정본은 테스트 영역 고정 random seed로 합법·거부 명령 5종을 선택하고 매 스텝 자동 보관 단서 불감소 및 거부 상태 hash 불변을 검사한다. [XML](../systems/tech-verification/t0-m1/results/t0-m1-contract-checks.xml) `T05_Seeded_10000_steps_preserve_auto_kept_clues` 성공. 생산 Sim RNG 추가 없음. [로그](../systems/tech-verification/t0-m1/logs/native-checks-3.log) | **closed** | game-systems-designer |
| C7-F50 | S2 | worldview / synopsis / systems / director | QA-M1-DATA-01. 실제 `systems/data/t0/records.json` 5행은 systemId/stationId가 누락돼 [Record 스키마](../systems/data-schemas/plates.md)와 [판독 명세](../systems/system-specs/plate-readout.md)의 인용 출처 조건을 충족하지 않는다. campaign sourceType/originId만으로 안정 관측소 ID·귀속을 확정할 수 없다. 실제 데이터 LoadRecord → Read → CiteToBoard는 거부되어 t0-b3와 전체 T0 시작이 차단된다. | [코드 QA §7](t0-m2-code-review.md#7-playmode3-복구인용-실행-영수증). 최신 synopsis/records receipt 해시와 생성/runtime records 바이트 일치, 명시 plate/ledger 귀속·Resources catalog 참조 확인. PlayMode3(2026-09-10 08:30:49–53 UTC) PointerUiCompletesCanonicalT0AndReopensSavedState Passed: 두 자료 인용으로 b3 완료·성공 영수증 2개·재로드 StateHash 동일. 원래 미반영 스냅샷 해소; 물리 포인터/최종 routing 검증 아님. | **closed** | game-worldview-architect |

### 14.1 실측과 해석 범위

- [OBSERVED 2026-09-10T05:34:19.214Z] `Tide.EditorTools.T0Verification.RunBatch` native editor 계약검사 21 / 실패 0. 원명령·실행 종료코드 0은 [systems 영수증](../systems/tech-verification/t0-m1-native.md), 원문은 [durable 로그](../systems/tech-verification/t0-m1/logs/native-checks-3.log), 항목별 결과는 [durable XML](../systems/tech-verification/t0-m1/results/t0-m1-contract-checks.xml)에 있다. QA는 durable 사본을 실행 원본과 바이트 단위로 대조해 동일함을 확인했다.
- NUnit 실행·Player build·플레이테스트·전체 T0 DoD가 아니다. 필수 패키지 설치는 ENOSPC로 미완료다. circuit overlay/3점 AnchorOverlay 및 전체 도구 FSM, UI/save/input, T-07과 실제 b3 완료는 미구현·미검증 또는 C7-F50으로 차단된다.
- 기존 게이트 판정 변경 0건. C7-F48·F49 해소는 한정된 구현/검사 회귀의 해소이며 G1~G8 PASS를 올리지 않는다. 과거 통합 집계는 각 표의 측정 시점 값으로 보존하고 최신 집계는 director가 `scripts/regen-cycle-ledger.py`로 이 정본에서 재생성한다.
- 방송: 발견·수정·남은 차단을 director와 systems에 2026-09-10 공유했으며 source 소유 레인 판정은 RFC-CX-001로 연결했다. `feedback-requested-by: 2026-09-10`.

## 15. T0 M2 독립 코드 검토 (2026-09-10)

[OBSERVED · 소스 검토] 아래는 /root/t0_resource_qa가 App/Save/Sim 및 테스트 소스를 읽어 도출한 결정적 실패 경로다. 이 리뷰의 Unity 실행 수는 0이며 새 실행 회귀 영수증 전에는 닫지 않는다. 상세 검토 스냅샷·필수 회귀는 [T0 M2 코드 리뷰](t0-m2-code-review.md)에 기록했다. director와 systems에 통보했다. `feedback-requested-by: 2026-09-10`.

| id | severity | lane | repro (파일·절) | evidence | status | owner |
|---|---|---|---|---|---|---|
| C7-F51 | S2 | systems | CommandJournal.cs:37. A/B/C(main) → Undo → D(branch) → Undo 두 번 → Redo하면 공통 조상 B의 이전 branch ID 때문에 자식을 찾지 못해 진행 불가. | [코드 리뷰 §6](t0-m2-code-review.md#6-editmode6-실행-증거에-따른-후속-판정). EditMode6 XML(2026-09-10 08:24:06–07 UTC) RedoFollowsSelectedBranchAcrossSharedAncestors Passed: 공통 조상 b와 선택 분기 d로 두 번 Redo·head seq 일치. 원래 짧은 저널 실패 해소; 압축/재로드 조합은 별도. | **closed** | game-systems-designer |
| C7-F52 | S2 | systems | T0GameSession.cs:193, Initialize:39–40. 실패 세이브에서 새 recovery-{guid} 슬롯 → 진행 저장 → 앱 재시작 시 원래 실패 경로를 다시 열며 새 슬롯이 발견되지 않음. | [코드 QA §7](t0-m2-code-review.md#7-playmode3-복구인용-실행-영수증). PlayMode3 RecoveryNewSlotIsSelectableAfterProcessEquivalentRestart Passed: 새 슬롯 저장→GameSession 재생성→select-slot-1로 진행 복원, 원래 v2 primary 바이트 불변. 실제 OS 프로세스 종료 테스트는 아님. | **closed** | game-systems-designer |
| C7-F53 | S2 | systems | T0GameSession.cs:50, :60, :85, :193. 초기 로드 실패 후 유효 파일 복원·재시도 성공에도 saveReadOnly가 true로 남아 시작/확정을 계속 거부. | [코드 QA §7](t0-m2-code-review.md#7-playmode3-복구인용-실행-영수증). PlayMode3 ValidatedRetryClearsRefusalAndPreservesSaveIdentity Passed: 유효 backup 복원→retry/start→새 CommitAsync true→saveId/createdUtc 원본과 일치. | **closed** | game-systems-designer |
| C7-F54 | S2 | systems | AtomicSaveStore.cs:79–81, :92. checksum 유효한 미등록/음수 schemaVersion primary와 정상 backup에서 버전 거부 예외가 복구 catch에 삼켜져 writable backup 로드·후속 primary 교체 가능. | [코드 리뷰 §6](t0-m2-code-review.md#6-editmode6-실행-증거에-따른-후속-판정). EditMode6 UnknownVersionsRefuseEvenWithValidBackup 및 VersionZeroMigratesWithBackupAndVersionTwoIsRefused Passed: null/-1/1.5/string1/v2 Refused와 primary 불변. 정수/범위 직접 반환 guard 확인; 유효 backup은 unknown loop 첫 사례에만 보장. | **closed** | game-systems-designer |

| C7-F55 | S2 | systems | SaveCodec.Decode의 JObject.Parse가 ISO createdUtc 문자열을 날짜 값으로 자동 해석해 재정규화 표기와 checksum이 달라짐. 유효한 자체 생성 세이브 로드·backup·멱등성 대조 실패. systems 발견. | [코드 리뷰 §6 및 F55](t0-m2-code-review.md#6-editmode6-실행-증거에-따른-후속-판정). EditMode4 저장/멱등성/복구 실패 영수증과 원인 코드 대조; DateParseHandling.None 적용 후 EditMode6(18/18)에서 createdUtc Encode→Decode 동일 문자열 단언·저장 회귀 Passed. run4의 기존 Canonical 테스트 자체는 Passed였고 당시 왕복 단언은 없었음. | **closed** | game-systems-designer |
