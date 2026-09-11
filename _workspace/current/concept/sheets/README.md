---
updated: 2026-09-10
cycle: 20260909-preproduction-c4
status: current
supersedes: null
owner: game-concept-artist
---

# 컨셉 시트 색인 — 2D 생성 결과 (45장 + 프리비즈 GIF 1)

**전량 컨셉/프리비즈다. 게임플레이 화면이 아니다.** 모든 항목 `runtimeEligible:false`.
스타일 규범: `../style-guide.md` · 생성 계약: `../generation-manifest.md` · 프롬프트: `../prompts/<asset-id>.txt`
권리: `../references.md` (제3자 참고 0건, 생성물 라이선스 `UNVERIFIED`)

## 0. 집계 [OBSERVED]

| 항목 | 값 |
|---|---|
| 생성 성공 | **45 / 45** (skipped 0) |
| provenance 항목 수 | **45** (6개 카테고리 각각 파일과 1:1 일치, 중복 id 0) |
| `runtimeEligible:false` 아닌 항목 | **0** |
| 재생성한 항목 | **13** (인물 10 + `previz-f02` + C7 회차 2장: `readme-verb-seal`, `space-gate-three-mood`) |
| 시각검수 완료 | **45 / 45** (썸네일 육안 검수. 확대 픽셀 검수는 `readme-verb-seal` 책자 1건뿐) |
| 1장 생성 실측 시간 | 90.9초 (`tool-reader-hero` 스모크 테스트) |
| 프리비즈 GIF | `assets/generated/previz/previz-cutscene-concept.gif` (9프레임, 1.4fps, 960×540, 3.4MB) |

### 0.1 도구 한계 — 요청 크기가 지켜지지 않는다 [OBSERVED]
`gti --size` 는 **참고값일 뿐 강제되지 않는다**: 45장 중 **29장이 요청과 다른 해상도**로 반환됐다.
예: `ui-status-badge-sheet` 요청 1024×1024 → 실제 2056×765, `ui-workbench-frame` 요청 2048×1152 → 실제 1536×1024.
→ **Steam 캡슐은 이 상태로 쓸 수 없다.** 상점 규격(header 920×430 / library 600×900 등)은 별도 크롭·리사이즈 공정이 필요하며, 그 공정은 아직 0건이다.
→ 반면 previz 9프레임은 전부 1672×941로 **동일**해 GIF 조립에는 문제가 없었다.

## 1. concept/ — 인물 (10장)

| asset-id | 실제 해상도 | 용도 | 시각검수 결과 [OBSERVED] | 재생성 |
|---|---|---|---|---|
| `char-seorin-sheet` | 1024×1536 | 서린 전신 시트 | 앞치마·허리 수첩·걷은 소매·염 탈색 소맷단 전부 확인. 3/4 각 성립 | 완료(1차 인종 불일치 수정) |
| `char-jaehwa-sheet` | 1024×1536 | 재화 전신 시트 | 두꺼운 방수점퍼·허리 열쇠고리·가슴 주머니 장갑·황토 칼라탭 확인 | 완료 |
| `char-eunjeong-sheet` | 1024×1536 | 은정 전신 시트 | 두 팔로 안은 반투명 방수 서류파일·레인케이프·방어 자세 확인 | 완료 |
| `char-seongchan-sheet` | 1024×1536 | 성찬 전신 시트 | 누빔 조끼·가슴 온도계·여는 손 제스처 확인 | 완료 |
| `char-doyeon-sheet` | 1024×1536 | 도연 전신 시트 | 수리한 경첩 안경·긴 카디건·빈 서명칸 클립보드·굽은 어깨 확인 | 완료 |
| `char-seorin-portrait` | 1145×1374 | 대화 UI 초상 | 3/4 각, 평상 표정, 중립 배경. **요청 1:1 아님** | — |
| `char-jaehwa-portrait` | 1145×1374 | 대화 UI 초상 | 어깨 염 결정 표현 양호. **요청 1:1 아님** | — |
| `char-eunjeong-portrait` | 1024×1536 | 대화 UI 초상 | 파일을 안은 실루엣 유지. **요청 1:1 아님** | — |
| `char-seongchan-portrait` | 1145×1374 | 대화 UI 초상 | 온도계 눈금이 미세 문자로 보일 여지 → **확대 검수 필요**. 요청 1:1 아님 | 조건부 |
| `char-doyeon-portrait` | 1024×1536 | 대화 UI 초상 | 클립보드는 빈 괘선만, 문자 없음. 요청 1:1 아님 | — |

> **1차 생성 전량 폐기 사유 [OBSERVED]**: 최초 프롬프트에 국적을 명시하지 않아 5인 전원이 비(非)한국계로 생성됐다. 세계관 캐논(은포항·한국식 인명)과 충돌하므로 프롬프트에 한국인 지정 + 3/4 각 강제 문장을 추가해 10장 전부 `FORCE=1` 재생성했다. provenance 는 같은 id 로 덮어써 45개를 유지한다.
> **초상 5장 공통 결함**: 요청한 1024×1024 정사각이 하나도 나오지 않았다. 대화 UI 슬롯이 정사각이면 크롭이 필요하다.

## 2. concept/ — 공간 (5장)

| asset-id | 실제 해상도 | 시각검수 결과 [OBSERVED] | 재생성 |
|---|---|---|---|
| `space-hub-watchroom-mood` | 1536×1024 | 원형 판독기·금속 문진 눌린 종이 더미·서랍장·서명대·황토 램프 전부 확인. 벽시계는 아날로그 침만, 숫자 없음 | 불필요 |
| `space-gate-three-mood` | 1536×1024 | **재생성 완료(2026-09-10).** 콘크리트 갑문 피어 2개·수직 슬라이드 강판 문짝·상부 권양기 드럼과 수직 리프트 체인·격자 발판·파이프 난간·밸브 휠·무문자 원형 게이지 2개·커플링마다 염화 고리 확인. **성곽/아치교 모티프 소멸 — 피어 위로 솟는 구조물 0** | **완료** (C4-F11 동반 재생성, §9 3순위 소진) |
| `space-lowland-mood` | 1672×941 | 판잣길·침하 주거·벽 염 자국 수위선 모두 성립. 동아시아 어촌으로 읽힘 — 최상 | 불필요 |
| `space-wharf-mood` | 1536×1024 | 셔터 격자·냉장 덕트·**무문자 조위관측판**·테트라포드 확인 | 불필요 |
| `space-pumphouse-one-mood` | 1536×1024 | 멈춘 원통 펌프·크랭크·전면 녹청·지하수로 만수면 확인 | 불필요 |

## 3. concept/ — 도구 히어로 프롭 (6장)

| asset-id | 실제 해상도 | 실루엣 규칙 충족 | 시각검수 결과 [OBSERVED] | 재생성 |
|---|---|---|---|---|
| `tool-circuit-hero` | 1536×1024 | 격자 ○ | 경첩 황동 격자 유리 + 도관 지도 + 탐침선. 지도에 문자 없음 | 불필요 |
| `tool-reader-hero` | 1312×1199 | 팔 달린 원 ○ | 원형 거치·육각 염판·스타일러스 스윙암·확대렌즈·크랭크(황토) | 불필요 |
| `tool-alignment-hero` | 1536×1024 | 평행선+눈금 ○ | 평행 레일 2줄·투명 슬라이드 2·집게 3. **콘크리트 배경과 명도차가 낮아 실루엣 분리 약함(§3의 3px 규칙 미달)** | 조건부 |
| `tool-routing-hero` | 1312×1199 | 분기 Y ○ | Y 매니폴드·페그 경로·황토 레버·계기 2 | 불필요 |
| `tool-corrosion-hero` | 1536×1024 | 육각 결정 △ | 저울대·한도 노치·시료컵. **결정이 육각이 아니라 입방체로 나옴** (§4 소금 결정 규칙 부분 위반) | 조건부 |
| `tool-seal-hero` | 1312×1199 | 겹친 사각 2 ○ | 경첩 2판 프레스·분리된 서명 거치 2개·황토 레버 | 불필요 |

## 4. ui/ (4장)

| asset-id | 실제 해상도 | 시각검수 결과 [OBSERVED] | 재생성 |
|---|---|---|---|
| `ui-workbench-frame` | 1536×1024 | 좌측 6슬롯 · 중앙 빈 가설판(핀홀·끈 가이드) · 우측 증거함 5칸이 명확히 분리되고 **전 영역 무문자**. 다만 요청 2048×1152 미달로 실사용 시 업스케일 필요 | 조건부(해상도) |
| `ui-tool-icon-sheet` | 1536×1024 | 3×2 = 6글리프. 격자/팔 달린 원/평행선+눈금/분기 Y/육각 결정/겹친 사각. **색 제거 1비트에서도 상호 구분 성립** | 불필요 |
| `ui-medium-badge-sheet` | 1774×887 | 육각 디스크 / 실제본 책자 / 세로 장부 등 — 외곽 실루엣만으로 구분 성립. 손글씨는 문자가 아닌 추상 파선. **배경이 검정/투명이라 UI 합성 전 알파 확인 필요** | 조건부 |
| `ui-status-badge-sheet` | 2056×765 | 5배지 = 채운 원 / 점선 링 / 하단 반쯤 채운 원+수평선 / 상향 셰브론 / 대각 바. **색이 아닌 형태로 구분** — §2.1 접근성 요구 충족 | 불필요 |

## 5. keyart/ · capsule/ (4장)

| asset-id | 실제 해상도 | 시각검수 결과 [OBSERVED] | 재생성 |
|---|---|---|---|
| `keyart-watchroom-wide` | 1672×941 | 뒤에서 본 주인공·원형 판독기·낮은 창 너머 검은 바다·황토 램프 단일 난색. 톤 필러 3개 전부 읽힘 | 불필요 |
| `keyart-watchroom-vertical` | 1024×1536 | 상단 1/4이 빈 천장/공기로 남아 로고 자리 확보됨. 의도대로 | 불필요 |
| `capsule-header-candidate` | 1672×941 | 좌측 1/3 무지 그라디언트 = 로고 자리, 우측에 판독기·수문 실루엣. 축소 시 읽힘 양호. **요청 2048×1152 미달** | 조건부(규격) |
| `capsule-library-candidate` | 1024×1536 | 하단 1/3 로고 밴드는 확보됐으나 **아트와 하드컷으로 잘려 한 그림으로 읽히지 않는다**. 인물도 지나치게 작다 | **권장** |

> 두 캡슐 모두 **로고·문자 없음**을 확인했다. 다만 Steam 상점 규격과 해상도가 맞지 않으므로 지금은 "후보"일 뿐이며 상점 업로드 대상이 아니다.

## 6. readme/ (7장) — 동사 → 공간 변화

| asset-id | 실제 해상도 | 변화가 한 프레임에서 읽히는가 [OBSERVED] | 재생성 |
|---|---|---|---|
| `readme-hero` | 1983×793 | 방조제→수문→저지대→부두→양수장이 한 디오라마로 읽힘. 최상 | 불필요 |
| `readme-verb-circuit` | 1536×1024 | ○ 센서 범위 안 도관만 황토로 발광, 밖은 냉청. 경계가 즉시 보임 | 불필요 |
| `readme-verb-reader` | 1536×1024 | ○ 좌: 부스러지는 염판 원본 / 우: 인쇄된 종이 사본. 보존이 읽힘 | 불필요 |
| `readme-verb-alignment` | 1916×821 | ○ 곡선 띠 2줄 + 집게 3 + 하단 오차띠가 우측으로 갈수록 얇아짐. 가장 설명적인 컷 | 불필요 |
| `readme-verb-routing` | 1536×1024 | △ Y 매니폴드와 한쪽 배수는 보이나 "두 판잣길 중 하나만 마른다"가 약함 | 조건부 |
| `readme-verb-corrosion` | 1536×1024 | ○ 구성안 배치 + 한도 노치를 넘어 기운 저울대 + 넘치는 염 결정 | 불필요 |
| `readme-verb-seal` | 1536×1024 | **재생성 완료(2026-09-10).** ○ 2거치(육각 염판/책자) + 기울어 닫히는 2판 프레스 + 압착부에 돋아난 구조 리브(before/after 한 프레임). 책자 펼침면 **100% 확대 검수** 결과 괘선과 짧은 파선뿐, 글자꼴·자소 0 — **§10-4 위반 해소** | **완료** (C4-F11) |

## 7. previz/ (9장 + GIF 1)

| asset-id | 실제 해상도 | 시퀀스 위치 (`video-study.md` 채택 순서) | 시각검수 결과 [OBSERVED] |
|---|---|---|---|
| `previz-f01-circuit-connect` | 1672×941 | 0~6초 · 배선 연결 | 체인 참조 원본. 염화 고리가 갈라지며 이음쇠가 앉음 |
| `previz-f02-water-drop` | 1672×941 | 0~6초 · 수위 하강 | **1차 실패 후 재생성.** 1차는 f01 구도를 그대로 상속해 손이 남고 수위 변화가 미미했다. 2차에서 손 제거 + 방조제 노출폭 확대로 변화가 한눈에 읽힘 |
| `previz-f03-compare-records` | 1672×941 | 6~18초 · 두 기록 대조 | 좌 염판 / 우 손글씨 일지, 사이에 장갑 낀 손. 일지 글씨는 추상 파선(문자 아님) |
| `previz-f04-valve-trial` | 1672×941 | 18~32초 · 밸브 시험 | 휠을 돌리는 손, 압력계 바늘 상단으로 이동 |
| `previz-f05-valve-revert` | 1672×941 | 18~32초 · 되돌림 | **f04와 완전 동일 노드**, 손 제거·바늘 복귀·수면 불변·이음쇠에 새 염 흉터. 되돌림이 정확히 읽히는 최고 성과 컷 |
| `previz-f06-flood-cost` | 1672×941 | 32~45초 · 침수 선택의 대가 | 좌 마른 판잣길(상자 들어올림) / 우 침수 골목. 대가가 즉시 읽힘 |
| `previz-f07-three-places` | 1672×941 | 45~55초 · 세 장소 대비 | 수문·저지대·부두가 한 프레임에, 각자 작업등 하나씩 |
| `previz-f08-signing-desk` | 1672×941 | 55~60초 · 서명대 | 빈 서류 거치대·열린 프레스·물러난 의자·무인 |
| `previz-f09-signing-close` | 1672×941 | 55~60초 · 서명 | 장갑 낀 두 손이 백지 문서를 거치대에 놓고 프레스가 닫힘 |
| `previz-cutscene-concept.gif` | 960×540 | 전체 | 9프레임 1.4fps 루프. **파일명·provenance·이 표 모두에 previz 로 명시** |

> **previz 세트 내부 일관성: 양호 [OBSERVED].** 같은 당직실 노드, 같은 램프, 같은 계기, 같은 결정형 염 표현이 9장 전부에 유지됐다. `--image` 체인 참조가 스타일 유지에는 확실히 효과가 있었다.
> **previz ↔ concept/keyart 간 렌더 불일치: 있음 [OBSERVED].** previz 세트는 윤곽선이 강한 일러스트 화법으로, 회화적·반사실적인 concept·keyart 세트와 붙여 놓으면 다른 게임처럼 보인다. 두 세트를 같은 README·같은 상점 페이지에 나란히 쓰면 안 된다. 어느 쪽으로 통일할지는 presentation-director 판정 사항이다.

## 8. 모델링·VFX·애니메이션용 측정 가능 제약

```yaml
constraints:
  camera:
    mode: fixed_node_2_5d
    lens_mm_equiv: 35
    eye_height_m: 1.55
    pitch_deg: -18
    roll_deg: 0
    waterline_band_pct: [38, 44]
  palette:
    core_hex: ["#0E1F26","#173238","#36565C","#4F7A6B","#8A8E88","#C8D6D3","#E7E3D8","#E2AF62"]
    accent_ochre_max_frame_pct: 8
    reserve_hex: "#8C4A3A"
    reserve_max_frame_pct: 2
    color_only_encoding: forbidden      # 색+형태+위치 3중 부호
  silhouette:
    min_outline_px_at_1080p: 3
    icon_1bit_distinguishable: required  # 6도구·3매체·5상태 전부
  subjects:
    characters: 5        # 각 표정 3종(평상/방어/결심), 두상 공유, 별도 실루엣 금지
    spaces: 5
    hero_tools: 6        # 피벗=도구 원점, 라벨 슬롯 공통, 단위 1m
    media_badges: 3
    status_badges: 5
  forbidden_in_image:
    - text_letters_numbers_logos_signage
    - award_laurel_or_score
    - other_game_hud_copy
    - real_disaster_photography
    - combat_weapons_blood_ghosts
    - unverified_working_title_strings
  provenance:
    runtimeEligible: false             # 45/45 확인
    promotion_path: production/decision-log.md audit only
```

## 9. 재생성 우선순위 (다음 사이클 입력)

**2026-09-10 갱신**: 1순위(`readme-verb-seal`)와 3순위(`space-gate-three-mood`)는 이 회차에 소진됐다(C4-F11 · RFC-M1). 아래는 남은 목록이다.

| 순위 | 대상 | 사유 | 예상 비용 | 상태 |
|---|---|---|---|---|
| ~~1~~ | ~~`readme-verb-seal`~~ | 책자 손글씨가 문자로 읽힐 위험 — §10-4 | 실측 1장 | **완료 2026-09-10** (NEGATIVE 강화 후 `FORCE=1` 재생성, 확대 검수 통과) |
| ~~3~~ | ~~`space-gate-three-mood`~~ | 배경 성곽/아치교 모티프가 캐논 밖 | 실측 1장 | **완료 2026-09-10** (콘크리트 갑문·권양기 명시, 유럽 판타지 모티프 네거티브 추가) |
| 1 | `capsule-library-candidate` | 로고 밴드가 하드컷으로 분리, 인물 과소 | 1장 | open |
| 2 | 캡슐·UI 프레임 규격 공정 | `gti --size` 미준수 → Steam 규격 크롭·리사이즈 파이프라인 신설 | 코드 작업 | open |
| 3 | `tool-alignment-hero`, `tool-corrosion-hero` | 실루엣 분리 약함 / 결정이 육각 아님 | 2장 | open |
| 4 | 초상 5장 | 정사각 슬롯이 확정되면 1:1 재생성 또는 크롭 | 5장 | open |
| 5 | previz ↔ concept 화법 통일 | 어느 쪽으로 맞출지 결정 후 최대 9장 또는 21장 재생성 | 결정 선행 | open — **C7 재생성 2장도 회화적 붓질보다 사실적 3D 렌더 화법으로 반환됐다** [OBSERVED]. 화법 정본은 여전히 미결 |

## 10. 미측정 · 이 문서가 주장하지 않는 것

- **인게임 적합성 0건.** Unity 프레임 안에서 이 아트가 읽히는지 실측하지 않았다. 해상도·대비·가독성은 전부 문서상 판단이다.
- **색약 검증 0명.** `style-guide.md` §2.1 대체 세트 A/B는 [TARGET]이며 실제 색각 이상 사용자 테스트를 거치지 않았다.
- **문자 부재는 대체로 썸네일 육안 검수 결과다.** OCR 검사는 0건이다. 2026-09-10에 `readme-verb-seal` 책자 펼침면만 100% 확대 검수를 했고(글자꼴 0), `char-seongchan-portrait`(온도계 눈금)·`previz-f03`(일지)은 **여전히 확대 검수 미실시**다.
- **재생성 2장의 카메라 규범 이탈 [OBSERVED].** `space-gate-three-mood`의 바다 수평선은 프레임 상단에서 약 27%, `readme-verb-seal`은 약 14% 지점으로 style-guide 카메라 절의 "38~44% 밴드" 밖이다. 두 컷 모두 근경 중심 구도라 밴드 규범이 적용되지 않는다고 판단했으나, 이는 육안 추정치이며 픽셀 측정이 아니다.
- **팔레트 준수율 미계산.** §2의 "황토 ≤8%" 같은 면적 비율을 픽셀 단위로 측정하지 않았다. 육안 판단이다.
- **라이선스 미확인.** 생성 백엔드 ToS를 확인하지 않았다. 상업적 사용 가부는 컨셉 레인의 권한 밖이다.
- **승격 0건.** 45장 전부 `runtimeEligible:false`이며 `unity/Unknown/Assets/` 로 옮긴 것은 없다.
