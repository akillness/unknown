---
updated: 2026-09-10
cycle: 20260909-preproduction-c3
status: current
supersedes: null
owner: game-qa
---

# 게이트 측정 (C3) — 단일 출처

## 0. 이 문서의 규율

- 게이트 수치의 **단일 출처**다. 다른 문서는 `qa/gate-measurements.md#g{n}`으로 인용만 하고 값을 재기재하지 않는다.
- **본 문서는 어떤 게이트도 PASS로 올리지 않는다.** 판정은 디렉터가 하며, 열린 S1 결함·`NOT-MEASURED`·숫자 자리의 `[TARGET]`은 PASS를 막는다(CLAUDE.md §6).
- 빌드 0줄, 사람 플레이 **n = 0**, 시뮬레이션 실행 0회, 성능 캡처 0건. 따라서 **런타임 값은 전부 `NOT-MEASURED`**이며 아래 `value`는 전부 **문서 검토 결과**다.
- `method: '문서 검토'`가 뜻하는 것: 파일을 열어 읽고 대조했다는 것뿐이다. 게임을 실행한 적이 없다.
- **재검증 1 (2026-09-10)**: 1차 24건 중 22건이 실측으로 닫혔고 신규 9건이 추가돼 총 33건이 됐다(`qa/defect-register.md` §2, 근거 `qa/c3-review.md` §7). 열린 S1은 **1건**(C3-F3) → **G1·G2·G3·G7은 문서 수준에서도 여전히 PASS 불가.**
- **재검증 2 (2026-09-10)**: C3-F3(S1) · C3-F26 · C3-F28을 명령 8종으로 재측정해 **3건 전부 closed**, 신규 3건(C3-F34~F36) 추가 → 총 36건 / closed 26 / open 5 / open-rfc 5. **열린 S1 = 0** → CLAUDE.md §6의 S1 차단 사유는 소멸했으나, `NOT-MEASURED`(빌드 0줄·표본 n=0)와 open S2가 남아 **PASS 가능한 게이트는 여전히 0개**다. 근거 `qa/c3-review.md` §8.
- **재검증 3 (2026-09-10)**: worldview 소유 open S2 1건(**C3-F34**)을 명령 7종(T1~T7)으로 재판정해 **closed**, 신규 결함 **0건** → 총 36건 / closed **27** / open **4** / open-rfc 5. 열린 S1 = 0 유지. **`#g1`의 문서 violation 이 2건 → 1건(C3-F29)** 이 됐고, 아래 `worldview_self_audit_claim`의 스테일 인용을 QA 자기정정으로 교체했다. 근거 `qa/c3-review.md` **§9**.
- **R4 = C4 재검증 (2026-09-10)**: `qa/c4-review.md` **F1~F4 를 4건 전부 `closed`** 로 판정하고(입력맵 충돌 0 · 홀드 옵션화 · 저장 실패 경로와 체크포인트 파일 분리 · `sourceType` AND `originId` · T0 3자 일치), 재검증 중 신규 **14건(C4-F5~F18, S2 8 · S3 6)** 을 등록했다. 명령 원문 **Q1~Q14** 는 `qa/c4-review.md` §R4.0. **열린 S1 = 0** 유지. 이 회차의 갱신 대상은 **#g4·#g5·#g6 의 문서 수준 값**이며 런타임 값은 **여전히 0건 측정**이다. 승격 판정(C3-F33)은 `c4-review.md` §R4.4 — 승격 가능 4파일 · 차단 8파일.
- **R5 = C5 독립 검토 (2026-09-10)**: 상품·생산·회귀 축을 명령 12종(M1~M12)으로 검사해 신규 **11건(C5-F1~C5-F11)** 을 등록했다. **병행 R4 회차가 같은 세션에서 `qa/c4-review.md`·`animation/*` 를 쓰고 있었다** — 본 회차의 M1~M11 은 그 이전 값이며 검토 대상 파일은 하나도 겹치지 않는다(`qa/c5-review.md` §1-a). R4 가 `systems/unity-implementation.md` 를 승격 차단으로 판정했으므로 본 회차의 §10 검증은 **그 차단을 뒤집지 않는다**(`qa/defect-register.md` §5, 근거 `qa/c5-review.md`). 열린 S1 **0**, 판정 **SPEC-FIX**. 갱신 대상은 `#g3`(상품 경제 ↔ 게임 내 경제 경계) · `#g7`(정본 입력 재측정 · C5 문서 축) · `#g8`(신선도 재실행 · 회차 산출물 부재)이며 **어떤 값도 PASS 방향으로 올리지 않았다**.
- **R6 = C6/C7 통합 검토 (2026-09-10)**: C6 초안 5렌즈 판정단(51건)과 C7 핸드오프 3렌즈 반박(45건)을 병합·중복 제거하고 명령 **16종(X-1~X-16)**으로 재측정해 신규 **C6-F1~F41 · C7-F1~F32**를 등록하고 기존 open 20건을 재판정했다(closed 15 · 하향 2 · 근거 갱신 3). 근거가 서지 않은 렌즈 발견은 `qa/c6-review.md` §4에서 **기각**했다. **열린 S1 = 1건(C7-F1 — T0 인스턴스 데이터 부재)** → CLAUDE.md §6의 S1 차단 사유가 C3·C4 이후 처음으로 다시 성립한다. 정본 입력 재측정: `node validate-campaign.mjs` `{checks 47, pass 47, fail 0, verdict PASS}` · `freshness-check.sh` 0 finding / **103** artifacts / exit 0 (본 갱신 후 재실행 = **104**) · `validate-preproduction.mjs` `512/512 passed` · errors 4(c6/c7 아티팩트 부재). **이번 회차에도 새로 측정된 런타임 값은 0건이다.**
- **이번 회차에 새로 측정된 런타임 값은 0건이다.** 아래 갱신은 #g1·#g3·#g7·#g8의 문서 근거와 영수증뿐이며, `runtime: NOT-MEASURED`는 8개 게이트 전건 유지된다.
- 정본 입력 [OBSERVED 2026-09-10 · R5 재측정]: `planning/campaign.json` sha256 `92301c0a5ecfc7e142646f5f17c29c21609e2b2de7c90408de7bffc9d815ae23` (**121457 B**) · `node _workspace/current/planning/validate-campaign.mjs` → **47/47 PASS**, exit 0.
  **QA 자기정정**: 직전 판까지 이 줄은 C3 종료 시점 값(`fdabf1d4…` · 120479 B · 44/44)을 고정 문자열로 적어 **스테일이었다**. planner 가 같은 계보(계보 B) 위에서 `zoneId` 추가(C3-F22)·RFC-W4 의도 문장 이동을 반영해 파일이 커졌고 검사도 3개 늘었다. **집계값(비트 33 · 단서 73 · 480분 · 322/673 · 도구별 비트)은 불변**이므로 이 갱신은 어떤 게이트 판정도 바꾸지 않는다. RFC-Q1 의 "고정 숫자 재기재 금지"는 QA 문서에도 적용된다 — 다음 회차부터 검증기 출력을 인용한다.

---

## #g1 — 세계관 일관성

```yaml
measured:
  value: "FAIL (문서 수준) — violation 1건 (**C6-F17**) / open-rfc 1건 (C6-F33) / 감사 open 3건 (A25 · A29 · A42) / 서사 인용 결함 4건 (C6-F15 · F20 · F21 · F22) [R6, 2026-09-10]"
  previous_value: "FAIL — violation 1건 (F29) / open-rfc 2건 (F25 · F30) / 관찰 1건 (F36) [재검증 3] ← violation 2건 (F29 · F34, 재검증 2) ← violation 7건 (2026-09-09 1차). 해소 경과는 detail 참조"
  detail:
    canon_retcon_in_live_data: 0     # 해소. validator K-01·K-02·K-03·K-04·K-05 전건 PASS
    disclosure_cap_violation: 0      # 해소. RFC-P3-012로 T0 공개가 캐논이 됨 (K-05 PASS)
    law1_violation_in_dialogue: 0    # 해소. scenes-and-dialogue S6 L137 재작성
    glossary_unregistered_nouns: 0   # 해소. 지목 14종 전건 grep -c = 1
    orphaned_archive_branch: 0       # 해소. bible/timeline이 archive c3를 supersedes (8파일이 인용)
    deprecated_law_phrase_body_use: 0    # 재검증 1 = 5행 → 재검증 2 = 0 (C3-F3 closed, systems 6곳 교체)
    deprecated_time_string_body_use: 0   # 재검증 1 = 1행 → 재검증 2 = 0 (C3-F26 closed, A-R3이 캐논 쌍으로)
    stale_input_receipt: 0               # 재검증 1 = 2문서 → 재검증 2 = 0 (C3-F28 closed, 2경로 집계 일치)
    worldview_audit_stale_observed: 0    # 재검증 2 = 5곳 → 재검증 3 = 0 (C3-F34 closed, 감사가 다섯 위치를 실측 위에서 재작성)
    violation_open:
      - "C6-F17 · S2 · 비트 본문 장소 ≠ zoneId 4건 — c2-b4(gate ↔ 당직실·부두사무소) · c6-b4(pump ↔ 당직실)는 스테이지 zoneIds 밖, c1-b2(hub ↔ 제3수문) · c5-b2(dock ↔ 저지대)는 스테이지 안. 검증기 Z-01/Z-02는 zoneId만 보고 본문을 보지 않는다 [OBSERVED R6 · X-8]"
    open_rfc:
      - "C6-F33 · 에필로그 '보존 등급 문장 1줄' 축이 바이블 §6 에필로그 4항목·감사 A25에 없다 (economy → worldview ack 대기)"
    closed_since_previous:
      - "C3-F29 → 지목 범위(c4-b1·c4-b3·c1-b4)는 live 데이터에서 해소 확인. 잔여 실체를 C6-F17로 승계하고 원 id 닫음"
      - "C3-F25 → 디렉터 C3 종료 판정으로 closed (H-1:40 캐논 유지, 폐기는 H-1:20·H+0:10 둘뿐). **이 게이트가 직전 판까지 open-rfc 로 적은 것은 QA 스테일이었다**"
      - "C3-F30 → 디렉터 판정 완료(인용 키 = campaign id). 잔여 파생 불일치는 consistency-audit A42 로 이관"
      - "C3-F36 → model.mjs L58 이 캐논 쌍 '밸브 개폐 각인 ↔ 봉인 완료 접점 각인' 으로 교체됨"
    narrative_citation_defects:
      - "C6-F15 · 초안에 연표·캐논 시각(H-1:40/H-1:24/H-1:04/H+0:12)·순서 앵커 0행 — 3장·6장 정답 판정 기준을 초안으로 알 수 없다"
      - "C6-F16 · T0 서사 공개 상한(RFC-P3-012) 0행"
      - "C6-F20 · '인용, 재작성 금지' 표의 2개 열이 축약 재작성"
      - "C6-F21 · 무대 문장에 '사람이 무엇을 확인했는지'(K-01 대상) 누락"
      - "C6-F22 · 결말 표 열이 바이블 §6 문구가 아닌 파생문인데 [INFERENCE] 미표기"
    worldview_self_audit_claim: "pass 40 / violation 0 / open 3 · 43행 (consistency-audit.md §1, 2026-09-10 4차). open 3건 = A25(에필로그 조합 축약) · A29(EN 표기 상표 조사 n=0) · A42(synopsis 표의 B# 파생 불일치). [R6 재측정] 이전 인용값 pass 37 / open 4 는 3차 시점 값이었다. **감사에 C6-F17(및 그 전신 C3-F29)이 등재돼 있지 않아 이 게이트의 유일 violation 을 증거 소스가 싣지 않는다** — worldview 가 감사 §2 에 항목을 추가해야 게이트와 증거가 같은 것을 가리킨다"
  method: '문서 검토 + 스크립트 실행'
  evidence:
    - _workspace/current/qa/c3-review.md                     # §7 재검증 1 · §8 재검증 2 · §9 재검증 3 (판정·명령 원문)
    - _workspace/current/qa/defect-register.md               # C3-F3 · F25 · F26 · F28 · F29 · F30
    - _workspace/current/worldview/worldview-bible.md        # §3 6법 정본표 (RFC-P3-014) · §3-bis (RFC-P3-009)
    - _workspace/current/worldview/timeline.md               # §2 사건 시각 (RFC-P3-013) · §7 33슬롯 재도출
    - _workspace/current/worldview/glossary.md               # 승격 명사 · §7 자료 카탈로그
    - _workspace/current/worldview/consistency-audit.md      # §1 집계(37/0/4) · §2 41항목 · §3 기계 검사 · §4 사용 금지 문구
    - _workspace/current/synopsis/continuity.md              # §4 공개 상한 (RFC-P3-012) · §5 K1~K10
    - _workspace/current/synopsis/scenes-and-dialogue.md     # S6 L137
    - _workspace/current/planning/campaign.json
    - _workspace/current/systems/system-specs/tide-alignment.md   # §3 A-R2·A-R3 (C3-F26 수정 확인)
    - _workspace/current/systems/data-schemas/plates.md           # §4 캐논 고정값 (순서 앵커 간격)
    - _workspace/current/systems/tech-verification/c3-fixloop2-canon-alignment.md  # systems 수정 영수증
  command: |
    node _workspace/current/planning/validate-campaign.mjs        # K-01~K-05 캐논 회귀 5검사
    grep -rn "서명 확인 전|봉인 호출|밸브 명령 대기 흔적|계통 영구 고장|4회째 결정 붕괴" _workspace/current
    grep -rn "판은 재생할수록 닳는다|소금은 모든 것을 먹는다|당직은 하나, 서명은 둘|시계는 조수에 매인다|물은 한 번에 한 곳으로만 간다" _workspace/current
    grep -rn "H-1:20|H+0:10|H-1:40" _workspace/current
    grep -rc "H-1:20" _workspace/current                                          # 재검증2: 11파일/31행, systems 0
    grep -rlE "<법 문구 5종>" _workspace/current                                   # 재검증2: 4파일/13행, systems 0파일
    grep -rlE "<법 문구 5종>" _workspace/current/systems | wc -l                    # 재검증3 T1: 0파일 / 0행
    sed -n '9p' _workspace/current/systems/system-specs/{6종}.md                    # 재검증3 T4: bible L44~L49와 6/6 문자 일치
    F=_workspace/current/worldview/consistency-audit.md; S=$(grep -n "^| A01 " $F|cut -d: -f1); E=$(grep -n "^| A41 " $F|cut -d: -f1)
    awk -v s=$S -v e=$E 'NR>=s&&NR<=e' $F | awk -F'|' '{print $5}' | sed 's/ //g' | sort | uniq -c   # 재검증3 T3: 37 pass / 4 open / 41행
    python3 -c "clues[].{originId,sourceType} 집계"                                # 73 / 27·22·24 / 31종
    grep -rln "archive/20260909-preproduction-c3/worldview" _workspace/current    # 8
    for t in 가설판 증거함 사건판 ... ; do grep -c "^| $t " _workspace/current/worldview/glossary.md; done
  timestamp: 2026-09-10 (KST)
  runtime: NOT-MEASURED
  note: >
    재검증 2: 기각·폐기 문구 14종 전수 검사에서 **본문 사용 0행**(재검증 1 = 6행). 남은 히트는
    전부 검증기 금지 문자열·폐기 원장·검토 기록이다. 1차의 문서 위반 7건은 전건 닫혔다.
    현재 violation 2건은 성격이 다르다 — C3-F29는 데이터↔단일출처표의 실제 반전이고,
    C3-F34는 세계관 감사가 자기 레인에 **불리한 방향으로** 틀린 스테일 [OBSERVED]다.
    관찰 1건(C3-F36)은 draft 프로토타입의 라벨이라 위반으로 세지 않았다.
    G1은 여전히 문서 수준 판정이며 세계관 몰입·서사 설득력은 측정되지 않았다.
    재검증 3: C3-F34 closed 로 violation 은 **1건(C3-F29)** 만 남았다. 감사 문서가 지목된 다섯 위치를
    실측 위에서 재작성했고 QA 독립 재측정(T1~T5)이 전건 일치했다. 폐기 문구 5행은 삭제하지 않고 §4-1을
    '해소 이력·재유입 방지 원장'으로 문맥 전환해 보존했다 — systems 스펙 6종 L11이 그 절을 인용주로 쓰기 때문이다.
    이번 회차의 유일한 값 변화는 이 한 줄이며, **런타임 축은 0건 그대로**다.
```

### #g1 · R6 (C6/C7) 재측정 [OBSERVED 2026-09-10]

```yaml
measured:
  value: "FAIL (문서 수준) — violation 1건 (C6-F17)"
  command: |
    node _workspace/current/planning/validate-campaign.mjs            # 47/47 PASS · K-01~K-06 전건 PASS
    python3  # beats[].subtasks[0] 의 구역 명사 ↔ beat.zoneId ↔ stage.zoneIds 전수 대조 (X-8)
    sed -n '/^## 1\./,/^## 2\./p' _workspace/current/worldview/consistency-audit.md   # pass 40 / violation 0 / open 3
  what_this_does_not_claim: >
    검증기 K-계열 PASS 는 **데이터가 캐논 문자열을 갖고 있다**는 뜻이지 초안이 그것을 전달한다는 뜻이 아니다.
    C6-F15·F16 은 정확히 그 간극이다 — 데이터는 지키고 문서가 말하지 않는다.
  runtime: NOT-MEASURED
```

## #g2 — 밸런스 밴드

```yaml
measured:
  value: "N/A (전투 항목) + NOT-MEASURED (대체 항목) — PASS 불가"
  na_scope:
    win_rate_band: null
    ttk_target_s: null
    ttk_tolerance: null
    combo_ev_cap_vs_median: null
    paid_free_power_delta: null
  na_reason: >
    계약 `## Premium overrides` — "전투 승률·TTK·유료 무료 격차·일간 인플레이션은 비전투 완결형
    게임에 부적합하다. 전투가 없다는 확인이 있을 때만 N/A로 명시하고 … 대체한다."
    확인 근거: planning/gdd.md §1 "전투 없음 · 유료 재화 없음 · 멀티플레이 없음",
    campaign.json kind enum = puzzle 21 / dialogue 5 / payoff 5 / exploration 2, combat 0 [OBSERVED].
    체력·피해·적 개체·대전 상대가 설계에 존재하지 않으므로 모집단 자체가 없다.
  substitute_checks:
    plate_replay_headroom_min: { target: 1.50, measured: null, status: NOT-MEASURED }
    corrosion_remaining_ratio_min: { target: 0.20, measured: null, status: NOT-MEASURED }
    alignment_pass_residual_min: { target: 4, measured: null, status: NOT-MEASURED }
    hint_reachability_stuck_after_l3: { target: 0, measured: null, status: NOT-MEASURED }
    difficulty_monotonicity: { target: "상승 ≤2/스테이지", doc_value: [5,5,7,7,7,8,10,9,4], status: DOC_ONLY }
    progress_block_count: { target: 0, measured: null, status: NOT-MEASURED }
  blocking_defects: [C3-F31, C3-F27]        # C3-F3은 재검증 2에서 closed
  blocking_defects_1차: [C3-F4, C3-F2, C3-F1, C3-F9]   # 재검증 1(2026-09-10)에서 4건 전부 closed
  difficulty_monotonicity_note: >
    C3-F31 — balance §7 난이도 지수 3열 중 2열(최대 단서·최대 도구)은 QA 재측정과 전건 일치하나,
    판정을 뒤집는 세 번째 열 '동시 가설 수'는 [INFERENCE]이며 이번 회차에 재측정되지 않았다.
    그 열을 빼면 지수 [4,5,5,4,4,5,7,4,3]이고 C5→C6 = +2로 규칙을 통과한다 [OBSERVED].
    따라서 '규칙 위반 1건'은 현재 DISPUTED이며 밴드를 넓히는 처리도 planner 재저작도 착수하지 않는다.
  method: '문서 검토'
  evidence:
    - _workspace/current/balance/balance-sheet.md
    - _workspace/current/balance/patch-deltas.md
    - _workspace/current/balance/sim-results/README.md
    - _workspace/current/economy/currency-map.md
    - _workspace/current/planning/campaign.json
  timestamp: 2026-09-09T16:1xZ
  runtime: NOT-MEASURED
  note: >
    대체 항목의 유도값은 재집계가 필요하다. balance-sheet.md가 인용한 sha256 `2bfe4d52…`는
    저장소에 존재하지 않고(C3-F2), 집계치(염판 22·reader 8·seal 6)는 live 파일에서 21·11·7이다.
    또 부식 예산의 정본이 balance(6계통) / economy(전역 9) 두 벌이라(C3-F4)
    "부식 잔여율"이 무엇의 잔여율인지 확정되지 않았다. 시뮬 S1~S3 실행 0회.
```

### #g2 · R6 (C6/C7) 재측정 [OBSERVED 2026-09-10]

```yaml
measured:
  value: "N/A (전투 항목) + NOT-MEASURED (대체 항목) — PASS 불가. 문서 수준 신규 S2 2건"
  doc_defects_new:
    - "C6-F3 · S2 · hints[0] 33건 중 다수가 1단 정의('주지 않는 것: 어떤 자료인지')를 어기고 퍼즐의 인과·조작 대상·자료를 지정한다. 검증기 H-01 은 hints.length !== 3 과 빈 문자열만 검사한다 [OBSERVED X-2 소스 확인]"
    - "C6-F5 · S2 · subtasks 에 「표」 또는 「칸」을 포함한 비트 21 / 33. 클라이맥스 C7(65분)은 손 조작 1비트. routing 3 · corrosion 3 [OBSERVED X-2 aggregates + python 계수]"
    - "C6-F35 · S4 · 무진전 180초 제안을 끄는 옵션 부재 · offerMaxPerSession = null(미정)"
  difficulty_index_doc_value: [4,5,5,4,4,5,7,4,3]   # C3-F31 판정 후 2열 재계산값. DOC_ONLY — 사람 난이도 측정 아님
  blocked_source: "balance/puzzle-balance.md 는 스스로 '정본이 아니다'(L11)라고 고지 — 차단 유지가 의도된 상태"
  runtime: NOT-MEASURED
```

## #g3 — 경제 건전성

```yaml
measured:
  value: "N/A (통화 항목) + NOT-MEASURED (대체 항목) — PASS 불가"
  na_scope:
    inflation_monthly_max_pct: null
    paid_free_winrate_delta_max_pp: null
    comeback_reversal_probability_max: null
    steady_parity_sessions_band: null
    source_per_day_sink_per_day_ratio_band: null
  na_reason: >
    계약 `## Premium overrides` 및 planning/gdd.md §9 범위 out — "유료 통화, 가챠, 시즌패스,
    행동 에너지, 게임 내 상점" 0종, 전투·PvP·랭킹 0종, 반복 세션·일일 보상 0종.
    팽창할 값·승패·일 단위 재생 자원이 존재하지 않는다 [OBSERVED economy/currency-map.md §1].
    N/A는 "검사하지 않았다"가 아니라 "대상이 존재하지 않는다"이며 대체 검사는 면제되지 않는다.
  substitute_checks:
    blocked_by_resource_count: { target: 0, measured: null, status: NOT-MEASURED }
    practice_and_undo_zero_cost: { doc_status: 충족, enforcement: 미강제(INV10/INV11 미구현) }
    reward_channels_allowlist: { target: "A/L/D/Z 4종 외 0건", doc_status: 충족, status: DOC_ONLY }
    base_entitlement_delta_without_dlc: { target: 0, doc_status: 0, status: DOC_ONLY }
    corrosion_safety_floor: { formula: "limit >= max(정상 선택지 비용)+1", doc_value: "9 >= 9 (여유 0)", status: DISPUTED }
  blocking_defects: [C3-F27]
  blocking_defects_1차: [C3-F4, C3-F15, C3-F16, C3-F23]   # 재검증 1에서 4건 전부 closed
  schema_conflict_note: >
    C3-F27(c) — economy currency-map §4.1의 계통별 표시 누계가 systems가 삭제한 save 필드
    `operationalCorrosion`에 걸려 있다(save.md L94 제거 절 [OBSERVED]). 문장 수정으로 닫히지 않는
    모델 충돌이며, G3 대체 검사의 자원 표시 축이 저장 근거를 잃은 상태다.
  method: '문서 검토'
  evidence:
    - _workspace/current/economy/currency-map.md
    - _workspace/current/economy/sink-source-ledger.md
    - _workspace/current/economy/reward-bands.md
    - _workspace/current/economy/negotiation-record.md
    - _workspace/current/balance/balance-sheet.md
  timestamp: 2026-09-09T16:1xZ
  runtime: NOT-MEASURED
  note: >
    economy 레인은 §6에서 "systems/ops/가 빈 디렉터리라 G3는 PASS할 수 없다"고 적었으나
    systems/ops/telemetry-contract.md(c3, current)는 존재한다(C3-F15). 결론(PASS 불가)은
    유지되지만 근거가 바뀌었으므로 재판단이 필요하다. 실제 PASS 불가 사유는 C3-F4(정본 2개)와
    측정 0건이다.
```

### #g3 · R5 추가 — 상품 경제(실화폐)는 이 게임의 경제 건전성이 아니다 [OBSERVED 2026-09-10]

```yaml
c5_product_economics_boundary:
  ruling: "G3 의 대상은 **게임 내 경제**다. product/economics.json 의 가격·수취·손익분기는 G3 의 증거가 아니며 N/A 근거도 아니다."
  why: >
    G3 의 N/A 근거는 "팽창할 게임 내 통화·승패·일 단위 재생 자원이 존재하지 않는다"이며
    이는 economy/currency-map.md §1 의 실측이다. PM 의 실화폐 모델은 **다른 축**이고,
    둘을 같은 게이트에 올리면 "미측정 게임 경제"를 "계산된 상품 경제"로 갈음하게 된다.
    QA 는 구매 의사를 만들어내지 않는다.
  measured_this_round:                       # QA 독립 재계산, 산술 축만
    discount_cap_keeping_9000_floor: { B1: 39, B2: 48, B3: 54 }   # floor((1-9000/P)*100) — 문서와 3/3 일치
    launch_cap_40pct_violation: [B1]                              # 14,900 x 0.60 = 8,940 < 9,000
    net_per_unit_list_price_k1: { B1: 8223, B2: 9745, B3: 11151 } # 할인 0% 기준, 문서 §5 와 6/6 일치
    net_per_unit_discount10_k1: { B1: 7350.95 }                   # 덱·검증기와 3중 일치
    breakeven_cells_checked: 12
    breakeven_mismatch: 0
    price_promoted_to_recommended: 0          # "권장" 승격 전수 0건
    playtime_to_price_derivation: 0           # 8시간→가격 논리 전수 0건 (7개 문서가 명시적으로 차단)
  not_evidence:
    - "위 숫자는 전부 [가정]·[TARGET] 이다. 지불 의사 n=0, 판매 n=0, 환불률·지역계수·배분율 미측정"
    - "economics.json status = 'scenario-not-forecast' · humanPriceStudyN = 0 · shareVerified = false"
    - "손익분기는 예측이 아니라 역산이다. 그만큼 팔릴 근거는 어느 문서에도 없다"
  blocking_defects_c5: [C5-F8, C5-F9]        # 라벨 부재로 같은 이름의 값이 둘 / 근거 인용 단절
  method: '문서 검토 + 산술 재계산 + 스크립트 실행'
  command: |
    node .claude/skills/game-ops-harness/scripts/validate-preproduction.mjs   # status FIX, checks 512/512, errors 2
    node _workspace/current/presentation/generate-deck.mjs --out <scratchpad>/deck-r5.html   # exit 0, errors []
  timestamp: 2026-09-10 (KST)
  runtime: NOT-MEASURED
  verdict_unchanged: "G3 = N/A(통화) + NOT-MEASURED(대체) — PASS 불가. R5 는 근거만 더했다"
```

### #g3 · R6 (C6/C7) 재측정 [OBSERVED 2026-09-10]

```yaml
measured:
  value: "N/A (통화 항목) + NOT-MEASURED (대체 항목) — PASS 불가. 상품 산식은 QA 독립 재계산과 일치"
  product_arithmetic_independent_recompute:      # X-10
    formula_source: _workspace/current/product/economics.json   # unitFormula 그대로 사용
    scenario_full_price:   { checked: 24, matched: 24 }   # §5 Net 6값 + 손익분기 18값
    scenario_launch_10pct: { checked: 12, matched: 12 }   # §5-1 Net 3값 + 손익분기 9값
    note: "B1 7,350.95 · B2 8,720.91 · B3 9,985.49 · 3,000만원 B1 4,082본 — PM 이 요청한 독립 재현 완료 (C5-F8 closed)"
  in_game_economy:
    corrosion_save_field: 0        # C3-F27(c) closed — save.md §2.1 「부식 저장 필드 없음」 신설
    retired_alias_lines: 1         # C3-F35 잔여 — systems/data-schemas/plates.md L39 한 줄 (S3 으로 하향)
  doc_defects_new:
    - "C6-F2 · S2 · 상품 약속('결론이 바뀌는 추리') ↔ 캠페인 전 분기 「비확장」 — 이 모순은 가격이 아니라 **가치 약속**의 문제다"
    - "C6-F6 · S2 · 포지셔닝·가격 한 문장 부재 · C6-F26 · S3 · 환불 2시간 창 시점 분석 0건 · C6-F29 · S3 · 본편 가격만 재기재 누락"
  still_not_measured: "지불의사 humanPriceStudyN = 0 · shareVerified = false · 위시리스트·전환 n=0"
  runtime: NOT-MEASURED
```

## #g4 — 연출 / 몰입

```yaml
measured:
  value: "NOT-MEASURED (런타임) · 문서 수준 FAIL — open S2 3건 (C4-F7 · C4-F9 · C4-F11) [R4, 2026-09-10]"
  previous_value: "NOT-MEASURED — 소스 문서가 status: current 아님 (2026-09-09, C3 회차)"
  immersion_score: null           # qa/immersion-scores.md 미작성 — 빌드도 표본도 없다
  playtest_sessions: 0
  archetypes_covered: 0           # 0 / 5
  detail:
    save_receipt_gating_lanes_done: 1     # animation 만. vfx·motion 미반영 → C4-F7
    tool_label_sets_in_use: 3             # gdd/style-guide/README · systems 2문서 · glossary (C4-F9)
    style_guide_forbidden_candidate_published: 1   # docs/media/verb-seal.jpg (§10-4 후보, README 게시) → C4-F11
    previz_sequence_slot_conflict: 1      # 55~60초 (video-study §채택 ↔ sheets/README §7) → C4-F18
    palette_hex_match_art_direction_vs_style_guide: "4 / 4"   # #E7E3D8 · #36565C · #E2AF62 · #173238 전건 일치
    generated_2d_assets: 45               # 45/45 성공 · runtimeEligible:false 45/45 · 승격 0건
    generated_3d_objects: 12              # 허브 셸 6 + 도구 6
    source_status:
      current: [concept/art-direction.md, concept/style-guide.md, concept/sheets/README.md, concept/generation-manifest.md]
      draft_promotable: [animation/animation-contract.md]
      draft_blocked: [motion/motion-contract.md, vfx/vfx-budget.md, presentation/video-study.md, presentation/deck-outline.md]
  method: '문서 검토 + 명령 재측정 (c4-review.md §R4.0 Q1~Q14)'
  command:
    - "grep -rn \"T0\" _workspace/current/concept/            # → 0행 (concept 은 T0 를 말하지 않는다)"
    - "find assets/generated/2d -type f -name \"*.png\" | wc -l # → 45"
    - "grep -c \"영수증\\|SavePending\" _workspace/current/vfx/vfx-budget.md  # → 0"
    - "ls docs/media/                                          # verb-seal.jpg 존재"
  evidence:
    - _workspace/current/qa/c4-review.md                     # 「재검증 (2026-09-10, R4)」 §R4.1.2 · §R4.2 C4-F7 · C4-F9 · C4-F11 · C4-F18
    - _workspace/current/concept/art-direction.md            # cycle c4, status current
    - _workspace/current/concept/style-guide.md              # cycle c4, status current — §2 팔레트 · §9 도구 6종 · §10 금지
    - _workspace/current/concept/sheets/README.md            # cycle c4, status current — §6·§7·§9 검수 결과
    - _workspace/current/animation/animation-contract.md     # cycle c5, status draft (승격 가능)
    - _workspace/current/motion/motion-contract.md           # cycle c4, status draft (C4-F7 차단)
    - _workspace/current/vfx/vfx-budget.md                   # cycle c4, status draft (C4-F7 차단)
    - _workspace/current/presentation/video-study.md         # cycle c2, status draft (C4-F18)
  timestamp: 2026-09-09T20:05Z   # = 2026-09-10 05:05 KST
  runtime: NOT-MEASURED
  note: >
    이번 회차에 새로 측정된 런타임 값은 0건이다. 위 detail 은 전부 문서·파일 대조 결과이며
    "연출이 좋다/몰입된다"에 대한 증거가 아니다. qa/immersion-scores.md 와 qa/playtest-report.md 는
    이번에도 작성하지 않았다 — 빌드 0줄·표본 n=0 에서 점수를 만드는 것은 측정 위장이다.
    문서 수준 FAIL 의 사유는 "연출이 나쁘다"가 아니라 세 레인이 같은 확정 계약(§0-10 저장 영수증)을
    말하지 않고, UI 라벨이 세 벌이며, 자기 레인이 금지 위반 후보로 표시한 이미지가 README 에 있다는 것이다.
```

### #g4 · R6 (C6/C7) 재측정 [OBSERVED 2026-09-10]

```yaml
measured:
  value: "NOT-MEASURED (런타임) · 문서 수준 FAIL — open S2 2건 (C4-F7 · C4-F11) + C4-F9 잔여 + 신규 C6-F27"
  reverified:
    C4-F7:  "vfx/vfx-budget.md · motion/motion-contract.md 에서 「영수증」 0회 · SavePending 0회 · 「저장 실패」 0회. 두 파일 모두 status: draft — 상태 불변 [CARRIED]"
    C4-F9:  "표시명은 **6/6 통일 확인** (gdd §4 = style-guide §9 = interaction-rules §2 제목). 잔여 = 용어집 미등재 4건 → S2→S3 하향 · 소유 worldview · RFC-S6 대기"
    C4-F11: "docs/media/verb-seal.jpg 존재 · README L38 게시 유지 — 상태 불변 [CARRIED]"
    C4-F18: "presentation/video-study.md 여전히 cycle c2 · status: draft — 상태 불변 [CARRIED]"
  doc_defects_new:
    - "C6-F27 · S3 · 상점 자산 준비도 0 — 캡슐 2장 로고 미포함(가제 미승인) · 45장 중 29장 해상도 불일치 · 크롭/리사이즈 공정 0건 · 트레일러 0(5초 프리비즈 2클립뿐) · 스크린샷 0(빌드 0줄)"
  ui_string_status: "T0 두 도구(배선 추적 · 판독) 포함 4개 표시명이 용어집 미등재 → [INFERENCE] 이며 최종 UI 문자열이 아니다 (C7-F5 와 같은 뿌리)"
  runtime: NOT-MEASURED
```

## #g5 — 에셋 / 이펙트 예산

```yaml
measured:
  value: "NOT-MEASURED (런타임) · 문서 수준 부분 통과 — 수량 계약은 재검산 일치, VFX 예산 항목 부재 · 매니페스트 스테일 3건 [R4, 2026-09-10]"
  previous_value: "NOT-MEASURED — 산정 소유가 미이관 (2026-09-09, C3 회차)"
  detail:
    asset_contract_rows: 47              # 셸5 + 도구6 + 초상5 + 공용소품30 + UI프레임1 — QA 재검산 일치 [OBSERVED]
    stage_allocation_sum_check: "셸 5 / 도구 6 / 초상 5"   # T0 1·2·1 + C1~C6 = 합계 계약과 일치 → C4-F4 closed
    t0_three_way_match: true             # unity §10 (hub·circuit·reader·25분) ↔ live campaign.json ↔ asset-budget L24
    greybox_tri_measured: 144            # [OBSERVED] Blender 계산 · 오브젝트 12 · Unity 런타임 관측 아님
    drawcall_measured: null              # 측정 불가 — Unity 임포트 0건. Blender 오브젝트 수를 대용하지 않는다
    texture_resident_mib_measured: 0     # 그레이박스는 단색 재질, 텍스처 0장
    vfx_effects_declared: 6              # brine_flow · pressure_pulse · salt_reveal · water_rise · seal_confirm · rain_window
    vfx_line_in_asset_budget: absent     # 예산표에 VFX 카테고리·견적 인일 없음 → C4-F17
    vfx_drawcall_allocation_rule: absent # 드로콜≤150(asset) ↔ emitter≤8·투명입자≤500(vfx) 사이 배분 규칙 0 → C4-F17
    animation_clips_without_prop: 3      # drawer_open · shutter_raise · water_level → C4-F17
    asset_manifest_stale_claims: 3       # style-guide 부재 · sheets 0개 · OPEN-M1 → C4-F10
    asset_new_ratio_planner_assumption: 0.313   # [TARGET][INFERENCE] campaign-time-budget.md §5 — 에셋 계수 아님
    asset_new_ratio_owner_estimate: null
    runtime_eligible_true_count: 0       # 생성 2D 45 + 3D 12 전건 false [OBSERVED]
  method: '문서 검토 + 파일 계수'
  command:
    - "find assets/generated/2d -type f -name \"*.png\" | wc -l   # → 45"
    - "ls _workspace/current/concept/sheets/                       # → README.md (14.8K)"
    - "grep -c \"VFX\\|이펙트\" _workspace/current/modeling/asset-budget.md  # → 0"
  evidence:
    - _workspace/current/qa/c4-review.md                     # §R4.1.4 · §R4.2 C4-F10 · C4-F17
    - _workspace/current/modeling/asset-budget.md            # cycle c4, status current — 라이브러리 표 · §C4 첫 실측 · "G5가 아직 통과할 수 없는 이유"
    - _workspace/current/modeling/asset-manifest.md          # cycle c4, status current — 47행 · §6 합계 · §7 열린 항목
    - _workspace/current/modeling/pipeline.md                # cycle c4, status current — 단위·피벗·명명·수입 게이트
    - _workspace/current/vfx/vfx-budget.md                   # cycle c4, status draft
    - _workspace/current/concept/generation-manifest.md      # 45/45 · provenance 6종
    - _workspace/current/planning/campaign-time-budget.md    # §5 신규 31.3% (기획 가정)
  timestamp: 2026-09-09T20:05Z   # = 2026-09-10 05:05 KST
  runtime: NOT-MEASURED
  note: >
    asset-budget.md 스스로 "드로콜은 측정하지 못했다 … Blender 오브젝트 수(12)는 드로콜이 아니다"
    라고 적고 §"G5가 아직 통과할 수 없는 이유"를 문서 안에 뒀다. QA 는 그 자기 제한이 옳다고 본다 —
    144 tris 는 그레이박스 하한이며 최종 아트의 예산 소진을 예측하지 못한다.
    뷰당 tri · 드로콜 · 텍스처 상주 세 지표 중 Unity 런타임 실측은 여전히 0건이다.
```

### #g5 · R6 (C6/C7) 재측정 [OBSERVED 2026-09-10]

```yaml
measured:
  value: "NOT-MEASURED (런타임) · 문서 수준 부분 — 승격 가능 자산 0종"
  promotable_assets: 0             # asset-runbook.md §3.1 감사 8항목 중 3항목 미충족
  promotion_blockers:
    - "라이선스 UNVERIFIED — assets/generated/2d(45) · video(2) provenance 전건"
    - "텍스트 100% 확대검수 0건"
    - "런타임 예산 실측 0건 — Unity Assets/ 파일 0 (X-13)"
  provenance_hash_match: "86 / 86 (modeling 레인 전수 재현, mismatch 0 · 파일 없음 0)"
  quantity_contract: "newAssets 47 = 셸5 / 도구6 / 초상5 / 공용30 / UI1 · artDays 63 — 불변"
  doc_defects_new_or_carried:
    - "C4-F17 · S3 · asset-budget.md 에 VFX 카테고리·견적 인일 여전히 부재 [CARRIED]"
    - "C7-F3 · S2 · SM_Tool_*.fbx 0건(GLB 6개뿐) — 브리프는 GLB 임포트, 런북은 FBX 단일화 (X-13)"
    - "C7-F13 · S2 · FORCE=1 재생성이 미추적 원본과 provenance 항목을 덮어쓴다 — CLAUDE.md §2 위반"
  runtime: NOT-MEASURED
```

## #g6 — 운영 안정성

```yaml
measured:
  value: "NOT-MEASURED (런타임) · 문서 수준 FAIL — 세이브 v1 필드 정의가 두 문서에서 갈린다 (C4-F6, RFC-S3 미판정) [R4, 2026-09-10]"
  previous_value: "NOT-MEASURED — 빌드 0줄 (2026-09-09, C3 회차)"
  detail:
    save_schema_single_source: false     # unity-implementation.md §7 ↔ data-schemas/save.md(current) → C4-F6
    save_field_divergences: 4            # chapter/stageId · dayIndex 유무 · propertyProtection bool/enum · checkpointRefs 유무
    recovery_files_declared: 4           # save.json · checkpoint.pre-commit.json · save.bak · 수동 3슬롯 [문서]
    recovery_screen_states: 4            # screens[5] 백업 / 사전 체크포인트 / 수동 슬롯 / 복구 불가
    save_failure_states_declared: 2      # screens[14] 「저장 진행 중 영수증 대기」 · 「저장 실패 재시도 또는 뒤로」
    write_failure_tests_declared: 6      # T-15 ~ T-20 — 전건 미실행
    commit_authority_rule_declared: true # interaction-rules §0-10 "확정은 저장이 성공해야 확정이다"
    atomic_write_declared: true          # 임시 파일 → fsync(지원 시) → 원자적 rename
    upper_schema_reject_declared: true   # 상위·미등록 버전 거부, 파일 바이트 불변
    input_lock_contradiction: 0          # animation-contract 가 "1프레임 입력 잠금" 을 명시적으로 폐기 → C4-F2 closed
    stale_verification_command: 1        # tech-verification/README.md §2.5·§2.6 (T0 30분 · captureScenes hub,gate) → C4-F8
    save_hash_mismatch: null
    save_recovery_path_non_primary_count: null
    crash_free_sessions: null
    frame_time_ms_p95: null
    hw_profile_id: null
  method: '문서 검토 + 명령 재측정'
  command:
    - "grep -n \"슬롯\" _workspace/current/systems/interaction-rules.md   # → L83·L84·L145 (\"자동 1슬롯\" 0행)"
    - "grep -n \"T0 30분\\|captureScenes\" _workspace/current/systems/tech-verification/README.md"
    - "cd _workspace/current/systems/prototype && node test-model.mjs   # → 37 통과 / 0 실패"
  evidence:
    - _workspace/current/qa/c4-review.md                     # §R4.1.2 · §R4.2 C4-F6 · C4-F8
    - _workspace/current/systems/unity-implementation.md     # §7 저장 스키마 · §9 축 분리 · §11 T-15~T-20 (cycle c5, status draft — 승격 차단)
    - _workspace/current/systems/interaction-rules.md        # §0-10 · §5 · §6 (cycle c5, status draft — 승격 차단)
    - _workspace/current/systems/data-schemas/save.md        # cycle c3, status current — 필드 정본 후보
    - _workspace/current/systems/system-specs/save-undo.md
    - _workspace/current/systems/architecture-contract.md    # RFC-S3
    - _workspace/current/systems/ops/telemetry-contract.md   # §7 hw_profile_id 규율
    - _workspace/current/animation/animation-contract.md     # 1프레임 잠금 폐기 · 영수증 게이팅
  timestamp: 2026-09-09T20:05Z   # = 2026-09-10 05:05 KST
  runtime: NOT-MEASURED
  note: >
    설계 문서는 C4 대비 크게 좋아졌다 — 복구 파일 4갈래, 저장 실패 UI 상태, 쓰기 실패 주입 테스트 6건,
    "확정은 저장이 성공해야 확정"의 불변식 승격. 그러나 그 전부가 미실행 계약이고, 세이브 v1 필드는
    같은 레인의 두 문서가 다르게 적는다(CLAUDE.md §9 불변식 구역). telemetry-contract §7 대로
    hw_profile_id 가 비어 있는 성능 수치는 판정에 쓰지 않으며 현재 전부 비어 있다.
    문서 존재는 G6 를 PASS 시키지 않는다.
```

### #g6 · R6 (C6/C7) 재측정 [OBSERVED 2026-09-10]

```yaml
measured:
  value: "NOT-MEASURED (런타임) · 문서 수준 FAIL — 세이브 v1 정의 충돌 유지 + 명령 로그 필드 부재 + T0 실행 불가"
  unity_project_state:
    project_version: "6000.5.6f1 (ProjectVersion.txt)"
    assets_file_count: 0
    batchmode_runs: 0              # Logs/*.log 에 -batchmode 0파일
    required_packages_installed: "0 / 4 (manifest 비-모듈 의존 = com.unity.multiplayer.center 뿐) [X-14]"
    runtime_instance_data: "zones/plates/tools/hints/beats.json 0건 [X-12]"
  doc_defects:
    - "C4-F6 · open-rfc · unity-implementation §7(chapter/dayIndex/eventSeq/eventLogHash) ↔ data-schemas/save.md(stageId · dayIndex 없음). **RFC-S3 는 decision-log 에 0건** [X-15] [CARRIED]"
    - "C7-F7 · S2 · CommandEntry 에 payload 없음(해시만) + commitIdempotencyKey 가 save.md 에 0건 → T-13/T-17/T-19 구현 불가"
    - "C6-F31 · S3 · 저장 파일 명명 세 벌 + save.md 경로가 Windows 전용"
    - "C6-F32 · S3 · 확정 1회당 디스크 쓰기 횟수·스레드·예산 미정의"
    - "C7-F1 · **S1** · T0 인스턴스 데이터 부재 — 이 게이트의 런타임 값이 **측정 가능해지기 전에** 닫혀야 한다"
  performance_budget: "기준 하드웨어 미정(PRE-1) — 프레임 16.7 / Validate 8 / Preview 16 / 세이브 200 / 로드 1500 / 되돌림 16 ms 전부 판정 보류"
  runtime: NOT-MEASURED
```

## #g7 — 피처 수용 / 코어루프

```yaml
measured:
  value: "NOT-MEASURED — 플레이 표본 n=0 (수용 조건은 RFC-P3-011로 정의 완료, 판정 불가 사유가 '정의 미완'에서 '표본 0'으로 축소)"
  observed_median_min: null
  observed_p25_min: null
  observed_p75_min: null          # 보고 대상이나 판정 조건 아님 (RFC-P3-011로 p75 조건 삭제)
  observed_n: 0
  observed_dropout_n: 0
  stuck_after_l3: null
  endings_reached_distribution: null
  design_budget_min: 480          # [TARGET] 문서 상수. validator S-03·S-04 PASS
  design_fast_min: 322            # live campaign.json 실측 합 (R5 재측정 92301c0a… — 값 불변)
  design_deliberate_min: 673      # live campaign.json 실측 합 (R5 재측정 92301c0a… — 값 불변)
  design_activity_budget_min:     # validator T-02 PASS (5범주 합 = 480, 비트별 합 = 그 비트 분)
    exploration: 53
    reasoning: 190
    manipulation: 168
    dialogue: 28
    payoff: 41
  acceptance_key: total_minus_afk_min          # RFC-P3-011 (1). AFK는 세션 후 회고로 판별, 자동 제외 없음
  acceptance_target_band_min: [450, 540]       # RFC-P3-011 (2). 세션 P 목표 유지
  withdrawal_triggers:                         # RFC-P3-011 (3). 통과선이 아니라 경보선
    median_lt: 420
    p25_lt: 360
  acceptance_sample_min: "n >= 12 / 5유형, 탈락 포함 보고"
  acceptance_evaluable: false                  # 이유: 표본 0. 정의 미완(C3-F14)은 해소됨
  scenario_bounds_not_compared: true           # 322/673은 시나리오 경계이며 표본 봉투와 대조하지 않는다 (범주 오류)
  blocking_defects: [C3-F29, C3-F31, C3-F35, C5-F1, C5-F6]  # C5-F1 = 덱이 폐기된 T0 범위로 착수 승인 요청 / C5-F6 = README 가 없는 핸드오프 브리프를 지시
  c5_slice_scope_alignment:                    # R5 — T0 가 "무엇을 재는 슬라이스인가"의 3자 일치 [OBSERVED 2026-09-10]
    canon: "systems/unity-implementation.md §10 = hub 1공간 + circuit·reader 2도구 + 25분"
    live_data: "campaign.json stages[0]: zoneIds ['hub'] · minutes 25"        # M4
    asset_budget: "modeling/asset-budget.md:24 T0 = 셸1·도구2·초상1"           # 수량 일치
    production_estimate: "T0 newAssets 4 · artDays 12.6 (= 5x1 + 3x2 + 1.6x1)" # 단가 환산 일치
    deck_slide_11: "허브에서만 · circuit, reader 2종 (데이터 파생) — 일치"
    deck_slide_26: "허브와 제3수문 · reader/alignment/seal · 제외 3구역 (하드코딩) — **불일치, C5-F1**"
    note: >
      C4-review F4 가 요구한 asset-budget 정정은 **불필요해졌다** — systems 가 T0 를 축소하는
      방향으로 해소했고 modeling·production 문서가 이미 그 범위와 맞는다. 남은 불일치는 덱 1곳뿐이다.
      슬라이스 범위가 바뀌면 "T0 측정치가 무엇을 대표하는가"도 바뀌므로 G7 의 전제에 직접 걸린다.
  method: '문서 검토 + 스크립트 실행'
  evidence:
    - _workspace/current/production/premium-preproduction-contract.md   # ## Time acceptance (RFC-P3-011 반영)
    - _workspace/current/production/decision-log.md                     # RFC-P3-011
    - _workspace/current/systems/ops/telemetry-contract.md              # §1 키 3종 · §1.2 판정 키·밴드·철회 트리거 · §5 AF7~AF9
    - _workspace/current/planning/campaign-time-budget.md               # §8 키표 · §9.2 · §9.3
    - _workspace/current/planning/campaign.json
    - _workspace/current/planning/validate-campaign.mjs                 # 44검사 (T-01~T-06 시간 구조)
    - _workspace/current/synopsis/chapter-beats.md
  command: |
    node _workspace/current/planning/validate-campaign.mjs      # 47/47 PASS, exit 0 — sha·bytes 는 이 출력에서 읽는다 (RFC-Q1)
    # R5 재측정 2026-09-10: sha256 92301c0a… · 121457 B · checks 47 / pass 47 / fail 0
    # 직전 판의 fdabf1d4… · 120479 · 44/44 는 C3 종료 시점 값이며 더 이상 live 가 아니다 (QA 자기정정)
    node .claude/skills/game-ops-harness/scripts/validate-preproduction.mjs   # checks 512/512, errors 2 (c5 회차 산출물)
  timestamp: 2026-09-10 (KST)
  runtime: NOT-MEASURED
  note: >
    C3-F14가 닫혀 수용 조건이 하나의 키(total_minus_afk_min)와 하나의 밴드(450~540)로 확정됐고,
    420/360이 통과선이 아니라 철회 트리거임이 계약·telemetry·time-budget 세 문서에서 같은 문구로 통일됐다.
    설계 480분은 이제 재실행 가능한 44검사로 뒷받침된다(합계·스테이지별·비트별·행동예산 4중 대조).
    그러나 그 어느 것도 사람이 8시간을 플레이했다는 증거가 아니다. 표를 더한 것과 플레이한 것은 다르다.
    G7이 PASS로 갈 수 있는 조건은 빌드 + n>=12/5유형 표본 + AFK 회고이며 현재 셋 다 0이다.
```

### #g7 · R6 (C6/C7) 재측정 [OBSERVED 2026-09-10]

```yaml
measured:
  value: "NOT-MEASURED — 플레이 표본 n=0. 수용 조건 정의는 완료, 판정선은 표본 0 위의 제안값"
  acceptance_key: total_minus_afk_min           # RFC-P3-011
  target_band_min: [450, 540]
  withdrawal_triggers: { median_lt: 420, p25_lt: 360 }   # 통과선이 아니다
  design_budget_min: 480                         # 문서 상수 — observedMedianMinutes: null · humanPlaytests: []
  t0_human_verification_thresholds:              # handoff/verification-plan.md §1.3 — **제안값**
    H1_first_valid_action_median_s: 60
    H2_objective_explained: "10 / 12"
    H3_progress_blocked: "0 / 12"
    status: PROPOSED_NOT_MEASURED
    note: "첫 회차 결과를 본 뒤 통과하도록 낮추는 조정을 금지한다고 문서에 적혀 있으나 기계 검사는 없다"
  core_loop_doc_defects:
    - "C6-F1 · S2 · 첫 30분에 밤의 목표가 없다 (「청문」 최초 등장 c3-b4 · 「제출」 c5-b4)"
    - "C6-F2 · S2 · 선택이 결과를 바꾸는가 — 전 분기 「비확장」"
    - "C6-F4 · S2 · 세션 재개 요약 미스펙 (주말 2~4회 분할 플레이 전제와 충돌)"
    - "C6-F10 / C7-F8 · S2 · T0 에 확정 명령이 없는데 확정·저장 롤백 테스트를 T0 인수로 요구"
    - "C6-F36 · open-rfc · routing·corrosion(유일한 세계 상태 변경 동사)이 사람 검증 없이 본 생산 진입"
  runtime: NOT-MEASURED
```

## #g8 — 신선도 · 메모리

```yaml
measured:
  value: "freshness exit 0 / 0 finding across 96 artifacts — 단, memory_sync 영수증 미검증이므로 PASS 불가"
  freshness_exit_code: 0
  freshness_findings: 0
  artifacts_scanned: 96            # 1차 82 → 재검증1 90 → 재검증2 91 → R5 시작 92 → R5 종료 95 → R4/R5 루프 2 재측정 96
  artifacts_scanned_delta_loop2: 1 # systems/tech-verification/c4-fixloop2-input-binding.md 1건 (C4-F19 영수증). 삭제·이동 0건
  artifacts_count_is_not_a_regression_signal: >
    92 → 95 의 증가분은 본 회차 산출물 1건(qa/c5-review.md)과 **병행 R4 회차가 쓴 animation/ 2건**이다
    (qa/c4-review.md 는 신규가 아니라 R4 가 확장). 이 수는 **측정 시각에 종속**되므로 회귀 신호로 쓰지 않는다.
    회귀 신호로 쓸 축은 `freshness_findings`(0)과 `freshness_exit_code`(0)뿐이다. 근거: qa/c5-review.md §1-a
  c5_cycle_artifacts_missing: 1    # production/cycles/c5-development.md — validate-preproduction.mjs 가 error 로 잡는다 (C5-F3)
  c5_stale_receipts_found: 2       # presentation/steam-game-plan.meta.md r3 표 (C5-F4) · 본 문서 §0·#g7 (QA 자기정정)
  c5_readme_claims_verified:       # 루트 README 주장 대 실측 [OBSERVED 2026-09-10]
    2d_assets: "45장 주장 = 45파일 / provenance 45항목 / runtimeEligible:false 45 — 일치"
    docs_media_provenance: "15항목 = 15파일, sha256 재계산 불일치 0, 'NOT gameplay' 누락 0 — 일치"
    3d_greybox: "144 tris / 12메시 — modeling/asset-manifest.md §6 과 일치"
    video: "Higgsfield 프리비즈 2클립 (5초·720p) — assets/generated/video/provenance.json 과 일치"
    unity_skeleton: "unity/Unknown ProjectVersion 6000.5.6f1 · Assets 0파일 · .cs 0개 — 일치"
    handoff_dir: "README 가 브리프·검증계획·런북을 적으나 파일 0개 — **불일치, C5-F6**"
  orphaned_archive_branch: 0       # 1차 2건 → 해소 (C3-F8 closed)
  archive_c3_worldview_referrers: 8
  memory_sync_verified: false
  memory_sync_receipt: "[SKIPPED: 이번 회차 mex 실행 금지] · graphify [UNGRAPHED: 코드 변경 0건] · llm-wiki 미기록 · zg 미실행"
  method: '문서 검토 + 스크립트 실행'
  evidence:
    - .claude/skills/game-ops-harness/scripts/freshness-check.sh
    - _workspace/current/qa/c3-review.md      # §7.5 토폴로지 표
    - _workspace/current/worldview/worldview-bible.md   # supersedes → archive c3
    - _workspace/current/worldview/timeline.md          # supersedes → archive c3
  command: |
    bash .claude/skills/game-ops-harness/scripts/freshness-check.sh
    node .claude/skills/game-ops-harness/scripts/validate-preproduction.mjs | head -12   # errors[] = 회차 산출물 부재
    grep -rln "archive/20260909-preproduction-c3/worldview" _workspace/current   # 8
    sed -n '1,7p' _workspace/current/worldview/{worldview-bible,timeline,glossary,consistency-audit}.md
    ls -la _workspace/current/handoff/ ; ls _workspace/current/production/cycles/
    git log --oneline -3      # HEAD b79577b — 신규 커밋 0건 (Release safety)
  timestamp: 2026-09-10 (KST)
  runtime: NOT-MEASURED
```

### #g8 스크립트 출력 원문 [OBSERVED 2026-09-10 · R5 재실행]

```
freshness: 0 finding(s) across 95 markdown artifact(s) under _workspace/current
freshness: scope = frontmatter contract + supersedes topology only; memory_sync receipts (mex/llm-wiki/graphify/zg) are NOT verified here
freshness: no cycle start supplied; staleness not measured
EXIT_CODE=0
```

### #g8 스크립트가 잡지 못한 것 [OBSERVED — QA 보완 측정, 재검증 1]

| # | 1차(2026-09-09) 사실 | 재검증 1(2026-09-10) 상태 | 왜 스크립트가 놓치는가 |
|---|---|---|---|
| 1 | 아카이브 c3 worldview 2건이 고아 | **해소.** `worldview-bible.md`·`timeline.md`가 아카이브 c3를 `supersedes`하고 8파일이 인용 | 고아 아카이브 검사가 범위 밖 (여전히) |
| 2 | `campaign.json`이 두 벌, current 4종이 아카이브를 인용 | **해소.** 계보 B 단일 정본, 재실행 가능한 44검사가 대체 | JSON은 frontmatter가 없어 91건에 포함되지 않는다 |
| 3 | 9개 문서가 없는 sha `2bfe4d52…`를 인용 | **해소.** 전체 64자리 `fdabf1d4…`를 31파일이 인용 | 영수증 해시 대조가 범위 밖 |
| 4 | current 문서가 draft 문서를 정본 인용 | **미해소.** RFC-P3-015가 draft 문서를 정본으로 지정한 상태 (C3-F33) | 인용 방향 검사가 범위 밖 |
| 5 | memory_sync 영수증 `[SKIPPED: mex 금지]` | **동일.** 코드 변경 0건이라 graphify도 `[UNGRAPHED]` | 스크립트가 스스로 "NOT verified here"라고 선언 |
| 6 | (신규) 문서 간 집계 영수증이 서로 다른 판을 가리킨다 (C3-F27·F28) | **미해소** | 문서 간 수치 대조가 범위 밖 |
| 7 | (신규) 같은 개념에 두 벌의 번호·구역 배정 (C3-F29·F30) | **미해소** | 의미 축 검사가 범위 밖 |
| 8 | (신규 · R5) **회차 산출물 자체의 부재** — `production/cycles/c5-development.md` 없음 (C5-F3) | **미해소** | `freshness-check.sh` 는 *존재하는* 파일의 frontmatter만 본다. **없는 파일은 검사 대상이 아니다.** `validate-preproduction.mjs` 가 이 축을 잡는다(errors[]) — 두 스크립트를 함께 돌려야 G8 이 완결된다 |
| 9 | (신규 · R5) **생성물 대 문서 주장의 대조** — README 45장·144 tris·2클립·provenance sha (C5-F6 이 잡힌 축) | 8건 중 7건 일치, 1건(`handoff/`) 불일치 | 스크립트 범위 밖. QA 가 `python3` 로 sha 재계산·파일 계수해 보완 측정했다 |

**판정 입력**: G8은 `freshness-check.sh exit 0` **그리고** memory_sync 영수증을 함께 요구한다(CLAUDE.md §6). 후자가 `[SKIPPED]`이므로 현재도 **PASS 불가**다. 다만 1차에서 스크립트가 놓쳤던 3건(고아·계보·해시)은 실제로 닫혔고, 남은 4건은 전부 **스크립트 범위를 넓히면 기계로 잡을 수 있는 것들**이다 — 검사 추가 요청은 `qa/c3-review.md` §7.5.

## 9. 게이트 요약 (재검증 3 · 2026-09-10) — R5 주석은 §10

| 게이트 | 값 | 런타임 | PASS 가능? | 차단 사유 |
|---|---|---|---|---|
| G1 세계관 일관성 | FAIL(문서) violation **1** (재검증2 2 · 재검증1 2 · 1차 7) | NOT-MEASURED | 아니오 | **F3·F26·F28·F34 닫힘**. 남은 violation = **F29**(구역 반전) 1건. open-rfc F25·F30, 관찰 F36 |
| G2 밸런스 밴드 | N/A(전투) + NOT-MEASURED(대체) | NOT-MEASURED | 아니오 | 시뮬 0회. F31(난이도 지수의 판정 열이 [INFERENCE]), F27 |
| G3 경제 건전성 | N/A(통화) + NOT-MEASURED(대체) | NOT-MEASURED | 아니오 | 측정 0건. F27(c) 스키마 충돌, **F35**(철회된 필드명 인용). F4·F15·F16은 닫힘 |
| G4 연출·몰입 | NOT-MEASURED | NOT-MEASURED | 아니오 | 소스 문서 draft, 표본 0. C4 범위 |
| G5 에셋·이펙트 예산 | NOT-MEASURED | NOT-MEASURED | 아니오 | 산정 소유 미이관. C4 범위 |
| G6 운영 안정성 | NOT-MEASURED | NOT-MEASURED | 아니오 | 빌드 0줄, hw_profile_id 미정 |
| G7 피처 수용·코어루프 | NOT-MEASURED (design 480 / 322 / 673, 44/44 PASS) | NOT-MEASURED | 아니오 | n=0. 수용 조건 정의는 **해소**(RFC-P3-011) |
| G8 신선도·메모리 | exit 0 / 0 finding / **91** artifacts | NOT-MEASURED | 아니오 | memory_sync `[SKIPPED: mex 금지 · graphify UNGRAPHED]`. 고아 아카이브는 **해소** |

**0 / 8 PASS.** 다만 차단 사유의 성격이 바뀌었다 — **열린 S1은 0이 됐다**(1차 7 → 재검증1 1 → 재검증2·3 **0**). 지금 PASS를 막는 것은 S1이 아니라 ① **open S2 3건**(F27·F29·F35) ② **빌드 0줄 · 사람 표본 n=0 · 시뮬 0회로 인한 `NOT-MEASURED`** ③ G8의 memory_sync 미검증이다. ②는 문서 작업으로 해소되지 않는다.
재검증 3 대비 전진은 **G1 violation 2 → 1** 한 줄뿐이며 그것도 문서 축이다. **런타임 축은 네 회차 내내 전부 0이다** — 새로 측정된 런타임 값 0건, 표본 n=0, 시뮬 0회, 성능 캡처 0건.

---

## 10. R5 (C5 독립 검토) 이후 게이트 상태 [OBSERVED 2026-09-10]

**0 / 8 PASS 유지.** R5 는 상품·생산·회귀 축의 **근거만** 더했고 어떤 게이트도 움직이지 않았다.

| 게이트 | R5에서 바뀐 것 | 여전한 차단 사유 |
|---|---|---|
| G1 세계관 일관성 | 없음(C5 대상 아님) | violation 1 (C3-F29) · open-rfc F25·F30 |
| G2 밸런스 밴드 | 없음 | 시뮬 0회 · F31 · F27 |
| G3 경제 건전성 | **경계 명문화** — 실화폐 상품 경제는 G3 증거가 아님을 `#g3 · R5 추가`에 고정. 산술 재검산 12칸 일치, "권장" 승격 0, 8시간→가격 논리 0 | N/A(통화) + NOT-MEASURED(대체). 지불 의사 n=0 · C3-F27(c) · C3-F35 · C5-F8·F9 |
| G4 연출·몰입 | 없음 | 표본 0 · 소스 draft |
| G5 에셋·이펙트 예산 | 참고 관측 — 자산 47종 수량·아트 63인일이 `asset-budget` 과 일치, 실재 3D 12메시 **144 tris**, 생성 2D 45장 전건 `runtimeEligible:false` | 런타임 실측 0건(드로콜·텍스처 상주 미측정). **greybox 7 / pending 40 / 생산 완료 0** — C5-F10 |
| G6 운영 안정성 | 없음 | 빌드 0줄 |
| G7 피처 수용·코어루프 | **정본 입력 재측정**(47/47 · `92301c0a…` · 121,457 B) · **T0 슬라이스 범위 3자 일치 확인**(덱 1곳만 불일치) | n=0. 새 차단 **C5-F1**(덱이 폐기된 T0 범위로 착수 승인 요청) · **C5-F6**(README 가 없는 브리프를 지시) |
| G8 신선도·메모리 | freshness **0 finding / 95 artifacts / exit 0** 재실행(시작 시점 92). README 주장 8축 중 7축 실측 일치 | memory_sync `[SKIPPED: mex 금지 · graphify UNGRAPHED · llm-wiki 미기록 · zg 미실행]` 변동 없음. 신규: **회차 산출물 1건 부재**(C5-F3) · 스테일 영수증 2건(C5-F4 · QA 자기정정) |

**R5 가 확인한 정직성 축 (위반 0건)** [OBSERVED]: `gameplay` 0건 · 수상/성과 표기 0건 · 가격 "권장" 승격 0건 · 8시간→가격 도출 0건 · 배분율 무라벨 사용 0건 · Release safety 위반 0건(신규 커밋 0 · 등록 행위 0 · 결제 0, Higgsfield 25 크레딧은 계약 허용 + 영수증) · provenance sha 불일치 0건(60항목 재계산).

**R5 가 측정하지 않은 것**: 사람 플레이 · 지불 의사 · 성능 · 외부 사실(Steam 정책 원문 · 비교작 현재가 · 외부 커밋 핀) · 덱의 시각 품질 · C4 레인 문서의 C4 판정(R4 소유). 근거는 `qa/c5-review.md` §7.


---

## 11. 재검증 1 (R4/R5 수정 루프 1 이후) 게이트 상태 [OBSERVED 2026-09-10]

**0 / 8 PASS 유지.** 이번 루프에서 **새로 측정된 런타임 값은 0건**이다 — Unity 실행 0회 · 빌드 0줄 · 패드 실측 0건 · 프레임/저장 캡처 0건 · 사람 표본 n=0 · 시뮬 0회. 아래는 게이트 값이 아니라 **차단 사유의 변화**만 적는다.

| 게이트 | 이번 루프에서 바뀐 것 | 여전한 차단 사유 |
|---|---|---|
| G1 | 없음 | violation 1(C3-F29) · open-rfc F25·F30 |
| G2 | 없음 | 시뮬 0회 · C3-F31 · C3-F27 |
| G3 | 없음 | N/A(통화) + NOT-MEASURED · 지불 의사 n=0 |
| G4 | 없음 | 표본 0 · 소스 draft(승격 차단 유지) |
| G5 | **C4-F10 해소로 modeling 착수 차단선이 부분 해제**(셸 4 + 도구 4). 자산 상태는 불변 — greybox 7 / pending 40 / 생산 완료 **0** · 전건 `runtimeEligible:false` | 드로콜·텍스처 상주 **실측 0건**. C4-F17(VFX 행 부재) · C5-F10 |
| G6 | **C4-F8 해소로 첫 계측 명령의 인자가 정본과 일치**(`T0 25분` · `-captureScenes hub`). **명령은 실행되지 않았다** | 빌드 0줄 · `PRE-1` 기준 PC `hw_profile_id` 미정 `[CARRIED]` |
| G7 | **C5-F1 closed** — 덱 26번이 live `stages[0]` 파생으로 바뀌어 "덱이 폐기된 T0 범위로 착수 승인을 요청"하는 차단이 사라졌다. 정본 입력 재측정 `47/47 PASS` · sha `92301c0a…` · 121,457 B | **n=0**(완주 표본·도달성·힌트 실효성 전부 미측정). 잔여 차단 **C5-F6**(README 가 없는 브리프를 지시) · C5-F5(덱 17번 사실 오류) |
| G8 | `freshness-check.sh` **0 finding / 95 artifacts / exit 0** 재실행(§#g8 원문과 동일 값). 이번 루프의 편집은 전부 같은 `cycle` 제자리 개정이라 **신규 markdown 산출물 0건** | memory_sync `[SKIPPED: mex 금지(CLAUDE.md §10 TeX 동명 바이너리) · graphify [UNGRAPHED] — 코드 변경 0건 · llm-wiki 미기록 · zg 미실행]` 변동 없음. C5-F3(회차 산출물 1건 부재) 미해소 |

**측정 명령 원문**: `qa/c4-review.md` §V1.0 표(W1~W15) · `qa/c5-review.md` §D0 표(D1~D12). 두 표의 값은 이 문서의 §#g8 출력·§10 값과 **문자 일치**하며, 어긋나는 값이 있으면 이 문서가 아니라 그 표의 명령을 다시 돌려 정한다(RFC-Q1: 고정 숫자 재기재 금지).

**이 절이 주장하지 않는 것**: closed 4건(C4-F5·F8·F10·C5-F1)은 전부 **문서·데이터 대조**로 닫혔다. `verification.matrix` 18행은 여전히 **전건 미실행**이고, 덱 게이트 3종은 **문자열·데이터 대조 게이트**이며, 정정된 계측 명령은 **한 번도 실행되지 않았다**. 게이트를 움직이는 것은 빌드와 표본뿐이며 둘 다 이번 루프에도 0이다.


---

## 루프 2 델타 (2026-09-10, R4/R5 수정 루프 2 이후) — game-qa

> 위 값을 **대체하지 않고 덧쓴다**. 이번 루프에서 배정된 결함은 `C4-F19`(S2 · systems) 1건이며 판정 전문은 `qa/c4-review.md` **「재검증 2」**(V2.0~V2.7) · `qa/c5-review.md` **「재검증 2」**(D2-0~D2-4)에 있다.

**0 / 8 PASS 유지.** **새로 측정된 런타임 값 0건** — Unity 실행 0회 · 빌드 0줄 · 패드 실측 0건 · 프레임/저장 캡처 0건 · 사람 표본 n=0 · 시뮬 0회.

| 게이트 | 이번 루프에서 바뀐 것 | 여전한 차단 사유 |
|---|---|---|
| G1~G4 | **없음** | 재검증 1 사유 전건 `[CARRIED]` |
| G5 | 없음 | 자산 상태 불변(greybox 7 / pending 40 / 생산 완료 0 · 전건 `runtimeEligible:false`) · C4-F17 · C5-F10 |
| G6 | **없음** — 입력 계약 문서 1행이 정합해졌을 뿐 계측 명령은 여전히 **0회 실행** | 빌드 0줄 · `PRE-1` `hw_profile_id` 미정 `[CARRIED]` |
| G7 | **없음.** C4-F19 closed 는 **문서 4곳이 같은 문장을 말한다**는 뜻이며 조작 수용성 측정이 아니다. `verification.matrix[12]`(`X`/`Y` 축)·`matrix[17]`(`LB` 축) **둘 다 미실행** | n=0(완주·도달성·힌트 실효성) · C5-F6 · C5-F5(범위 12곳으로 확대) |
| G8 | `freshness-check.sh` **0 finding / 96 artifacts / exit 0** [OBSERVED 2026-09-10, X11]. 증가분 1 = `systems/tech-verification/c4-fixloop2-input-binding.md`(C4-F19 영수증). 나머지 편집은 전부 같은 `cycle` **제자리 개정**(RFC-Q2) | memory_sync `[SKIPPED: mex 금지(CLAUDE.md §10 TeX 동명 바이너리) · graphify [UNGRAPHED] — 코드 변경 0건 · llm-wiki 미기록 · zg 미실행]` 변동 없음. C5-F3(회차 산출물 1건 부재) 미해소 |

**정본 입력 재측정 [OBSERVED 2026-09-10]**: `planning/campaign.json` sha `92301c0a5ecfc7e1…ae23` · 121,457 B · `47/47 PASS` · exit 0 (X12) · `systems/prototype/test-model.mjs` **37/0** (X13) · `presentation/steam-game-plan.html` `0468eab2…` · 100,041 B **바이트 동일** (Y1) · `systems/game-ui-contract.json` `9c89e9ae…` **불변** (X9). **네 입력 전부 이번 루프에서 움직이지 않았다.**

**측정 명령 원문**: `qa/c4-review.md` §V2.0 표(X1~X15) · `qa/c5-review.md` §D2-0 표(Y1~Y6). 값이 이 문서와 어긋나면 **이 문서가 아니라 그 표의 명령을 다시 돌려** 정한다(RFC-Q1).

**이 절이 주장하지 않는 것**: C4-F19 는 41행 중 **1행**을 닫았다. 같은 파일에 C4-F16(L31 표면 우선순위) · C4-F21(L28·L30 키보드 파생) · C5-F5(「매체 경로」 2곳)가 열려 있고, 신규 **C4-F22**(재도출 명령이 결론을 내지 않는다 · 2곳)가 열렸다. **게이트를 움직이는 것은 빌드와 표본뿐이며 둘 다 이번 루프에도 0이다.**

### #g8 · R6 (C6/C7) 재측정 [OBSERVED 2026-09-10]

```yaml
measured:
  value: "PARTIAL — freshness exit 0 / 0 finding across 103 artifacts. memory_sync 미검증이므로 **G8 전체 PASS 로 읽지 않는다**"
  freshness_exit_code: 0
  freshness_findings: 0
  artifacts_scanned: 104           # R5 96 → 회차 산출물 증가 → 103(본 리뷰 작성 전) → **104**(qa/c6-review.md 생성 후 재실행). 삭제·이동 0건
  artifacts_count_is_not_a_regression_signal: true
  scope_limits_verbatim:           # 스크립트가 스스로 출력하는 경계 — 그대로 옮긴다
    - "scope = frontmatter contract + supersedes topology only; memory_sync receipts (mex/llm-wiki/graphify/zg) are NOT verified here"
    - "no cycle start supplied; staleness not measured"
  cycle_artifacts_missing: 3       # validate-preproduction.mjs errors[] 재실행 확인 — production/cycles/c6-development.md · qa/c7-review.md · production/cycles/c7-development.md (qa/c6-review.md 는 본 회차에 해소됨)
  memory_sync_verified: false
  memory_sync_receipt: "mex [SKIPPED: 이번 회차 mex 실행 금지 · CLAUDE.md §10 정체 검증 규칙] · graphify [UNGRAPHED: 코드 변경 0줄] · llm-wiki 미기록 · zg [미실행: handoff/ 신설로 레이아웃 변경 → 사이클 종료 시 디렉터]"
  rfc_ledger_integrity:            # R6 신규 측정 — G8 의 '메모리' 축에 직접 걸린다
    decision_log_missing_rfc_ids: 10   # RFC-C6-001 · C6-002 · Q3 · S2 · S3 · S5 · S6 · B6 · M3 · CX (X-15)
    note: "CLAUDE.md §4 '파일이 바뀌지 않은 RFC 는 일어나지 않은 것이다'. handoff/README.md §2 의 되묻기 절차(decision-log 검색)가 구조적으로 빈 결과를 낸다 → C6-F7"
  command: |
    bash .claude/skills/game-ops-harness/scripts/freshness-check.sh
    node .claude/skills/game-ops-harness/scripts/validate-preproduction.mjs | head -20
    grep -c "RFC-C6-001\|RFC-C6-002\|RFC-Q3\|RFC-S2\|RFC-S3\|RFC-S5\|RFC-S6\|RFC-B6\|RFC-M3\|RFC-CX" _workspace/current/production/decision-log.md
    git status --short ; git log --oneline -3
  timestamp: 2026-09-10 (KST)
  runtime: NOT-MEASURED
```

#### #g8 스크립트 출력 원문 [OBSERVED 2026-09-10 · R6 재실행]

```
freshness: 0 finding(s) across 104 markdown artifact(s) under _workspace/current
freshness: scope = frontmatter contract + supersedes topology only; memory_sync receipts (mex/llm-wiki/graphify/zg) are NOT verified here
freshness: no cycle start supplied; staleness not measured
EXIT_CODE=0
```

---

## R6 승격 판정 요약 (C3-F33 · RFC-Q2) — 전문은 `qa/c6-review.md` §6

| 판정 | 파일 | 사유 |
|---|---|---|
| **승격 가능** | `product/economics.meta.md` · `presentation/deck-outline.md` · `presentation/steam-game-plan.meta.md` | 각 파일의 잔여 결함(C5-F7 / C5-F5 / C5-F4·F5) 전건 closed, 열린 결함 0건. meta 해시 표는 `shasum` 실측과 문자 일치(3/3 · 2/2) |
| **차단** | `product/business-model.md` | C5-F8·F9 closed이나 §1 가치 약속에 **신규 open S2 C6-F2** |
| **차단** | `systems/interaction-rules.md` | C4-F16·F21·F22 closed이나 **신규 open S2 C7-F10**(§1-3.1 ↔ §1-3.2 자기모순) · RFC-S6 미판정 |
| **차단** | `systems/unity-implementation.md` | **C4-F6(RFC-S3) open-rfc 유지** + C7-F4·F7·F9 |
| **차단** | `systems/game-ui-contract.meta.md` | meta 자체 결함 0이나 기술 대상 `game-ui-contract.json` 에 C4-F12 open S2 |
| **차단 (의도)** | `balance/puzzle-balance.md` | 문서 L11 자기 고지 "정본이 아니다" — 승격하면 정본이 둘이 된다. 사유 불변 |


---

# 최종 게이트 표 (R7 종료 · 2026-09-10) — 재검증 3 기준

> **판정: 8게이트 중 PASS 0.** 이 표는 QA 의 **재고**이며 판정은 디렉터가 한다.
> 값의 성격을 두 층으로 나눠 적는다 — **문서 수준**(파일을 열어 대조한 결과)과 **런타임**(게임을 실행해 잰 값). **런타임은 8/8 전부 `NOT-MEASURED`** 다: Unity 실행 **0회** · 빌드 **0줄** · 사람 플레이 표본 **n = 0** · 프레임/저장 캡처 **0건** · 키 입력 **0건**.
> **열린 S1 = 0**(C7-F1 closed)이지만 그것은 S1 차단 사유의 소멸일 뿐 **측정의 대체가 아니다**. 열린 S2 **5건**과 `NOT-MEASURED` 가 남아 PASS 가능한 게이트는 **0개**다.

## 이번 회차 측정 입력 [OBSERVED 2026-09-10 R7]

| 명령 | 출력 |
|---|---|
| `node _workspace/current/planning/validate-campaign.mjs` | `{checks 49, pass 49, fail 0, verdict PASS}` · `sha256 8a43d334…` · `bytes 124007` |
| `node _workspace/current/planning/validate-campaign.mjs --t0` | `{checks 5, pass 5, fail 0, verdict PASS}` · `T0-01`~`T0-05` · `sourceShaMatchesLiveCampaign: true` |
| `bash .claude/skills/game-ops-harness/scripts/freshness-check.sh` | `0 finding` · `118 artifact` · **exit 0** (원문은 `#g8` 절) |
| `node .claude/skills/game-ops-harness/scripts/validate-preproduction.mjs` | `status FIX` · `runtimeStatus NOT-MEASURED` · `passed 514` · `errors 3` |
| `node _workspace/current/systems/prototype/test-model.mjs` | **37 통과 / 0 실패** · exit 0 |
| `python3 ~/.claude/skills/game-ui-ux/scripts/validate-game-ui.py … game-ui-contract.json` | `PASS: valid game UI contract` · exit 0 |
| `node _workspace/current/systems/pipeline/emit-tables.mjs --scope t0 --out <scratch>` + `shasum` 5쌍 | **5/5 MATCH** (QA 독립 실행) |

**고정 sha 를 문서에 재기재하지 않는다**(RFC-Q1). 위 값은 인용이며 다음 편집에서 바뀌면 이 표가 아니라 명령이 정본이다.

## 8게이트 한 줄 표

| 게이트 | 판정 | 문서 수준 값 [OBSERVED] | 런타임 | 근거 경로 |
|---|---|---|---|---|
| **G1 세계관 일관성** | **PASS 불가 (NOT-MEASURED 아님 · 문서는 통과)** | **violation 0** — C6-F17(비트 구역 4건) closed 후 검증기 `Z-01`·`Z-02`·`Z-03` 전건 PASS, `K-01`~`K-06` 전건 PASS. `consistency-audit.md` §1 `pass 37 / violation 0 / open 4`. 용어집 도구 6/6 등재(C4-F9·RFC-S6 closed) | **NOT-MEASURED** — 플레이 중 캐논 노출 순서 관측 0건 | `qa/gate-measurements.md#g1` · `qa/c6-review.md` §12.1 R7-2·R7-11·R7-23 |
| **G2 밸런스 밴드** | **N/A(전투 항목) + NOT-MEASURED(대체 항목)** | 전투·승률·TTK **없음** → 계약 「Premium overrides」에 따른 **이유 있는 N/A**. 대체 항목 = 난이도 지수 `[4,5,5,4,4,5,7,4,3]`(C3-F31 closed, QA 가 `node -e` 3종 재실행해 포락=단일 비트 실부하까지 재현) · 상승 폭 ≤2 통과 · 힌트 1단 어휘 `H-04` **0위반** | **NOT-MEASURED** — 사람 난이도·완주 시간·힌트 실효성 **n = 0** | `#g2` · `balance/balance-sheet.md` §7·§7.1 · `qa/c6-review.md` §12.1 R7-10·R7-16 |
| **G3 경제 건전성** | **N/A(유료 재화 항목) + NOT-MEASURED(대체 항목)** | 유료 재화·인플레이션·가챠 **없음**(1회 구매) → **이유 있는 N/A**. 대체 항목 = 게임 내 유일 소모성 = `routing` 부식예산 전역 상한 9(RFC-P3-009), 기능 인질 금지 목록, DLC 비의존 완결. **상품 경제(가격·손익)는 G3 가 아니다** — PM 가정이며 판매 실측 **n = 0** | **NOT-MEASURED** | `#g3` · `#g3 · R5 추가` · `economy/resources-and-fairness.md` · `product/business-model.md` §2·§9-1 |
| **G4 연출 / 몰입** | **NOT-MEASURED** | 문서 정합만 개선: C4-F7 closed 로 `vfx-budget.md`·`motion-contract.md`·`animation-contract.md` 셋이 **영수증 이후 발행 / 저장 실패 미발행 / 전역 잠금 없음**을 같은 문면으로 말한다. `motion-contract.md` §6 자기 고지 = `[TARGET]` 만 있고 `[OBSERVED]` **0** | **NOT-MEASURED** — 프레임 캡처 0 · 입력 지연 0 · 몰입 점수 표본 **n = 0** | `#g4` · `qa/c6-review.md` §12.2 C4-F7 |
| **G5 에셋 / 이펙트 예산** | **NOT-MEASURED** | 생성물 실측: GLB **7** · FBX **1** · blend **1** · 렌더 13 · provenance 23항목. 전건 `runtimeEligible:false`, 승격 **0건**. 포맷 정본 = GLB(C7-F3 closed). **라이선스 UNVERIFIED**(C6-F8 closed 로 위험 등재) | **NOT-MEASURED** — 트라이/드로콜/텍스처 메모리 측정 0 · Unity 임포트 **0회** | `#g5` · `handoff/asset-runbook.md` §3.3-0 · `assets/generated/*/provenance.json` |
| **G6 운영 안정성** | **NOT-MEASURED** | 문서 정합: 저장 3파일 분리 + 원자적 rename + `commitIdempotencyKey` + `SV-F6` 로그 접힘(`byteCap` 6 MiB / `entryCap` 20,000). 참조 모형 **37/0**. **`C7-F40` open S2** 로 체크포인트 라벨 문면이 3곳에서 브리프 금지 규칙과 충돌 | **NOT-MEASURED** — 크래시·저장 실패 주입 실행 **0회** | `#g6` · `systems/system-specs/save-undo.md` §6·§9 · `qa/c6-review.md` §12.3 C7-F40 |
| **G7 피처 수용 / 코어루프** | **NOT-MEASURED** | 문서 정합: T0 확정 명령 확정(`reader` 인용 고정, RFC-C7-001) · T0 인스턴스 데이터 **존재하고 결정론 재현**(C7-F1 closed) · 세션 재개 요약 스펙 신설(C6-F4) · 첫 30분 목표 문장(C6-F1) · 진행 보장 `C-07` 17/17 | **NOT-MEASURED** — **H-1/H-2/H-3 표본 0 / 12**. `observedMedianMinutes: null`, 설계 480분은 **관측이 아니다** | `#g7` · `handoff/verification-plan.md` §1.2 · `qa/c6-review.md` §12.1 R7-3·R7-8 |
| **G8 신선도 · 메모리** | **PARTIAL — 스크립트 PASS · memory_sync 미검증** | `freshness-check.sh` **exit 0 · 0 finding · 118 artifact**(원문 아래). 그러나 스크립트 스스로 «memory_sync receipts (mex/llm-wiki/graphify/zg) are NOT verified here» 와 «no cycle start supplied; staleness not measured» 를 고지한다 → **G8 전체 PASS 로 읽지 않는다**(CLAUDE.md §11) | 해당 없음(문서 게이트) | `#g8` · `#g8 스크립트 출력 원문` · **memory_sync 영수증은 디렉터 소유** — `production/retrospectives/` 및 `production/receipts/` 의 디렉터 영수증 블록을 인용하며 QA 는 그 값을 만들지 않는다 |

### G8 `freshness-check.sh` 출력 원문 [OBSERVED 2026-09-10 R7 · 그대로 옮김]

```
freshness: 0 finding(s) across 118 markdown artifact(s) under _workspace/current
freshness: scope = frontmatter contract + supersedes topology only; memory_sync receipts (mex/llm-wiki/graphify/zg) are NOT verified here
freshness: no cycle start supplied; staleness not measured
```
exit code **0**. **`mex` 는 이 회차 지시가 금지했으므로 실행 0회** — mex/llm-wiki/graphify/zg 동기화 영수증은 QA 가 생산하지 않으며 **디렉터 영수증 경로만 인용한다**.

## 이 표를 PASS 로 바꾸는 데 필요한 것 (문서로는 절대 닫히지 않는다)

| 게이트 | 없는 것 | 만들어지는 시점 |
|---|---|---|
| G4 · G5 · G6 · G7 | **Unity 빌드 1개** | T0 수직 슬라이스 |
| G7 | **사람 12명 / 5유형 표본**(H-1 첫 조작 ≤60초 중앙값 · H-2 10/12 · H-3 0/12, 탈락 포함 보고) | T0 사람 검증 |
| G2(대체) | 완주 분·힌트 실효성·이탈 분포 | 동상 |
| G5 | 트라이·드로콜·텍스처 메모리 실측(`hw_profile_id` 확정 후) | T0 성능 캡처 · PRE-1 |
| G8 | memory_sync 영수증 4종 | 사이클 종료 체크리스트(디렉터) |

계약 「Base production gate」 4조건 중 **충족은 ④ 하나**다(열린 S1 0 · `freshness-check.sh` exit 0). ①②는 미착수, ③은 평가 대상 이전. **문서 D 게이트 통과는 실제 게임 G 게이트 통과가 아니다.**
