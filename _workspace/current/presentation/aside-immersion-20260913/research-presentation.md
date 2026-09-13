---
updated: 2026-09-13
cycle: 20260909-preproduction-c7
status: draft
supersedes: null
owner: game-presentation-director
---

> [OBSERVED] 독립 조사 시점의 보고서. 제안은 승인·구현·현재 런타임 검증을 뜻하지 않는다. 최종 채택/기각은 이 폴더 decision-and-implementation.md를 우선한다.

# unknown 연출·GTI 리소스 개선 — 독립 evidence 조사 보고

작성: 하위 조사 세션 (구현 없음 / 저장소 편집 0건 / 유료 호출 0건 / 하위에이전트 0건)
조사일: 2026-09-13 (KST)
파이프라인: `deep-research` → `references/web-search-pipeline.md`, 모듈 **general-web + academic-papers** 로딩 후 검색 실행.

---

## 0. 이 문서의 지위와 한계

- 전부 **[SOURCED] 외부 1차 출처 인용 + [DERIVED] 그 출처로부터의 계산값**이다.
  **[MEASURED] 항목은 0건** — 이 세션은 Unity·GTI·프로파일러를 한 번도 실행하지 않았다.
- 아래 어떤 숫자도 "이 게임에서 이런 효과가 났다"는 주장이 아니다. XAG의 수치는
  **Microsoft가 공표한 지침값**이고, 대비표는 **WCAG 상대휘도 공식으로 팔레트 hex를 계산한 값**이다.
  실측 대비·성능·플레이테스트는 전부 `NOT-MEASURED`로 남는다.
- 참고 이미지는 **0건 사용**했다. 런타임/Unity/Blender preview/M5/M6 화면은 열지 않았고,
  제안 프롬프트도 `concept/style-guide.md` + `concept/prompts/*.txt` 하우스 포맷만 근거로 썼다.
- OMP 진행분(**힌트 idle · 판독기 crank · M20 work surface 승격**)과 겹치는 제안은 **의도적으로 배제**했다
  (겹침 회피 근거는 §5 각 항목의 "비중복 판정" 줄에 명시).

읽은 내부 좌표: `.claude/agents/game-presentation-director.md`, `.claude/agents/game-concept-artist.md`,
`_workspace/current/concept/style-guide.md`, `_workspace/current/concept/concept-first-m7-sources.json`,
`_workspace/current/concept/references.md`, `_workspace/current/presentation/video-study.md`,
`_workspace/current/presentation/t0-work-surface-m20.md`, `_workspace/current/presentation/t0-action-plate-m19.md`,
`unity/Unknown/Assets/_Project/UI/T0Interface.cs`.

> ⚠️ **좌표 정정 1건**: 작업 지시의 `_workspace/current/presentation/presentation-spec.md` 는 **존재하지 않는다**
> (`ls _workspace/current/presentation/` 확인). M19/M20 문서가 스스로 "이 파일을 건드리지 않는다"고
> 참조만 하고 있는 상태다. 연출 계약의 실질 정본은 현재 `t0-action-plate-m19.md`(전역 버튼·텍스트) +
> `t0-work-surface-m20.md`(작업면 배경) + `video-study.md`(채택·기각) 세 개로 쪼개져 있다.

---

## 1. 1차 출처 5건 (URL + 짧은 인용)

### S1. Valve — *Illustrative Rendering in Team Fortress 2* (NPAR '07, Mitchell·Francke·Eng)
PDF: https://steamcdn-a.akamaihd.net/apps/valve/2007/NPAR07_IllustrativeRenderingInTeamFortress2.pdf
DOI: https://doi.org/10.1145/1274871.1274883

인용(원문 verbatim, PDF 본문에서 직접 추출):
- > "we deliberately avoided modeling the world in an overly complex or geometrically off-kilter manner as this would add an unnecessary level of visual noise"
- > "keeping repetitive structures such as the bridge trusses, telephone poles or railroad ties to a minimum is preferable for our style, as conveying the impression of repetition in the space is more important than representing every detail explicitly."
- > "high frequency geometric and texture detail found in photorealistic games can often overpower the ability of designers to compose game environments and emphasize gameplay features visually using intentional design choices such as changes in color value."
- > "with muted colors dominating and small areas of saturation to give further visual interest."
- > "Even when viewed only in silhouette with no internal shading at all, the characters are readily identifiable to players." (컨셉 단계에서 실루엣만으로 검증했다는 서술)

**함의**: 가독성은 색이 아니라 **명도(value)** 로 만들고, 그 명도 대비가 작동하려면 **주변 고주파 노이즈와 반복을
먼저 줄여야** 한다. 우리 §10-8(글로시·블룸 금지)과 §5(고정 노드)와 정합하며, **M20의 비-타일 규칙 T1~T5에
외부 근거를 준다**("반복의 인상"만 남기고 실제 반복은 최소화).

### S2. Frictional Games — *Gaps of the Imagination* (Thomas Grip, 2017-06-20)
https://frictionalgames.com/2017-06-gaps-of-the-imagination/

인용:
- > "Sometimes it's best to just leave gaps, and let the player's imagination handle the rest."
- > "Normally you build this sort of gap up by presenting a **negative space** where one or more object/events/reactions/etc. all related to something unseen."
- > "you can often get away with very little if the scene is just set up in the right manner."
- (인과 간극의 3요소) > "The three key elements for achieving this are **consistency, negative space and optimization-avoidance**."
- (게으른 뇌 경고) > "If the pattern becomes 'locked door equals the room is irrelevant', it will be noticed and incorporated into the model."

**함의**: **정적 여백은 '아직 안 그린 자리'가 아니라 설계 대상**이다. 다만 여백이 *일관된 패턴으로 반복되면*
플레이어 뇌가 "이 영역은 무의미"로 최적화해버린다 — 여백을 쓰되 **여백의 처리 방식을 변주**해야 한다.

### S3. Frictional Games — *Storytelling through fragments and situations* (2010-03-15)
https://frictionalgames.com/2010-03-storytelling-through-fragments-and-situations/

인용:
- > "Fragmented storytelling allows for much more freedom as it is possible for the player to pick up fragments in different order and even to miss certain fragments without ruining the story."
- > "Fragments does not only need to be text-heavy information such as dialog or notes. It can be graphics in the environment, sounds, character banter, interactions, etc."
- > "we have made sure to make the most important things are really obvious (and hard to miss) and the less important more hidden."
- (사건 vs 상황) > "In a situation, one creates a some sort of outside pressure and then it is up to the player on how the protagonist should act, never stopping the normal mode of gameplay."

**함의**: 비전투 추리에서 **증거 가독성 = 중요도의 시각적 층화**다. "전부 똑같이 잘 보이게"가 아니라
**핵심 조각은 놓칠 수 없게 / 부차 조각은 숨겨서**. 그리고 압박은 컷신 이벤트가 아니라 **상황(수위·시간)**
으로 준다 — 우리 톤 필러 "조용한 압력"과 정확히 같은 축이며, `style-guide.md` §1 3행과 1:1로 붙는다.

### S4. Microsoft — *Xbox Accessibility Guideline 102: Contrast* (XAG v3.2)
https://learn.microsoft.com/en-us/gaming/accessibility/xbox-accessibility-guidelines/102

인용:
- > "Standard-sized text and visual elements (those that aren't considered large-scale) that provide important information or context for gameplay should have a contrast ratio of at least **4.5:1** against their background."
- 표: 표준 텍스트/시각요소 **4.5:1** · 대형 텍스트/시각요소 **3:1** · **비활성(inactive) 요소 3:1** ·
  고대비 모드 **7:1** · placeholder/입력 필드 4.5:1(표준)·3:1(대형).
- > "Avoid relying on color alone to communicate information."
- 버전 이력: 비활성 요소 기준이 **2.5:1 → 3:1로 상향**됨 (https://learn.microsoft.com/en-us/xbox/accessibility/xag-version-history).

**함의**: M19가 정의한 **P3 비활성 판(흐려짐)** 과 **보조 설명 16px 약화색(배경 쪽 0.32 보간)** 은
지금 **3:1 기준선의 직접 사정권**에 있다(`t0-action-plate-m19.md` §1.1, §1 P3). M20 §8-3이 대비를
`NOT-MEASURED`로 남긴 것은 정직하지만, **배경 후보를 하나 더 얹기 전에 이 축을 먼저 계산**해야 한다.

### S5. Microsoft — *XAG 117: Visual distractions and motion settings* / *XAG 103: Additional channels*
- 117: https://learn.microsoft.com/en-us/gaming/accessibility/xbox-accessibility-guidelines/117
- 103: https://learn.microsoft.com/en-us/gaming/accessibility/xbox-accessibility-guidelines/103

인용(117):
- > "ensure that players can pause or completely stop any content that scrolls, blinks, auto-updates, or otherwise moves."
- > "Avoid any repetitive side-to-side or up-and-down on-screen movement, except that which is core to game play."
- > "Avoid the use of camera shake, camera bobbing effects, motion blur, mouse blur, and more or provide an option to turn off these behaviors."

인용(103):
- > "Any content that's critical to understanding gameplay or comprehending the narrative, and is expressed through color, also needs to be expressed by using at least one additional signifier such as **shape, pattern, iconography, or text labels**."
- > "If a change of color (graying) is used to inform the player of the existence and state of a control that isn't available, another method is also used." (Sea of Thieves 자물쇠 기호 예시)
- > "Color alone should never be used to represent information."

**함의**: "은은한 상태 변화"의 **가장 안전한 구현은 애니메이션이 아니라 지속되는 정적 표식**이다.
움직이는 미세 신호(깜빡임·펄스·스크롤)는 117이 명시적으로 끄기 옵션을 요구하는 부류다.
그리고 M19 P3의 "흐려짐"은 색 단독 부호라 **103 위반 소지**가 있다 — 형태 채널이 하나 더 필요하다.

(보조 참고, 1차 아님) XAG 109 objective clarity: https://learn.microsoft.com/en-us/gaming/accessibility/xbox-accessibility-guidelines/109 —
> "Interruptions ... that aren't directly related to the objective at hand can be postponed or suppressed by the player."
힌트 표면은 OMP 소관이므로 이 축은 **참고만 하고 제안하지 않는다**.

---

## 2. [DERIVED] 팔레트 대비 계산표 — 지금 바로 쓸 수 있는 유일한 정량 축

WCAG 상대휘도 공식(XAG 102가 쓰는 그 정의)에 `style-guide.md` §2 hex를 그대로 넣은 **계산값**이다.
화면 실측이 아니다. 감마·톤매핑·알파 합성·텍스처 국소 명도는 반영되지 않는다.

| 텍스트색 | 배경색 | 대비 | XAG 102 판정 |
|---|---|---|---|
| 7 종이 `#E7E3D8` | 1 심해잉크 `#0E1F26` | **13.18:1** | 7:1 고대비 모드까지 통과 |
| 7 종이 | 2 확정잉크 `#173238` | **10.57:1** | 통과 |
| 7 종이 | 3 청회 `#36565C` | **6.20:1** | 4.5:1 통과 |
| 7 종이 | 4 녹청 `#4F7A6B` | **3.78:1** | 표준 텍스트 **미달**, 대형/비활성 3:1만 통과 |
| 7 종이 | 5 콘크리트 `#8A8E88` | **2.60:1** | **전부 미달** |
| 7 종이 | 8 황토 `#E2AF62` | 1.55:1 | 미달 |
| 2 확정잉크 | 7 종이 | 10.57:1 | 통과 |
| 2 확정잉크 | 6 백청 `#C8D6D3` | 9.04:1 | 통과 |
| 2 확정잉크 | 8 황토 | 6.80:1 | 통과 |
| 2 확정잉크 | 5 콘크리트 | **4.07:1** | 표준 **미달**, 3:1만 통과 |

**결론 3줄**
1. **종이색 본문은 배경 명도가 팔레트 3(청회, L*33) 이하일 때만 4.5:1이 선다.** 배경 아트가
   텍스트 대역에서 팔레트 5(콘크리트)까지 밝아지면 그 순간 미달이다.
2. **잉크색 본문은 배경이 팔레트 6(백청) 이상일 때만 안전하다.** 팔레트 5는 잉크로도 미달(4.07).
3. → **팔레트 5 젖은 콘크리트는 어떤 본문 색과도 4.5:1을 못 만든다.** `style-guide.md` §2에서
   화면의 15~35%를 차지하도록 허용된 색이므로, **콘크리트 대역 위에는 본문을 놓지 않는다**가
   실행 가능한 규칙이 된다. (이건 새 캐논이 아니라 §2 값에서 나온 산술 귀결이다.)

---

## 3. 채택 / 보류 / 기각

### 채택 (A)

| # | 채택 항목 | 근거 | 적용 좌표 |
|---|---|---|---|
| A1 | **여백은 산출물이다** — 연출 스펙에 "무엇을 보여주는가"와 함께 **"어디를 비우는가"**를 라인으로 적는다 | S2 negative space | `presentation/` 신규 문서 또는 M19/M20 §1 형식 |
| A2 | **가독성은 명도로, 색은 보조** — 강조는 채도·경고색이 아니라 명도 계단으로 | S1(색값 변화로 강조), S4(색 단독 금지) | `style-guide.md` §2 명도 대역 규칙과 이미 정합 |
| A3 | **비-타일·저반복 규칙에 외부 근거 부여** — M20 T1~T5는 취향이 아니라 업계 관행 | S1("repetition ... to a minimum") | `t0-work-surface-m20.md` §2 표에 근거 열 추가 |
| A4 | **상태 변화는 정적 지속 표식 우선, 애니메이션은 옵션 뒤** | S5-117 | M19 P2·P3 확장 |
| A5 | **비활성/약화 텍스트에 3:1 계산 게이트 도입** (실측 아님, hex 산술) | S4 표 + §2 계산표 | `t0-action-plate-m19.md` §1.1 보조 16px 약화색 |
| A6 | **증거는 균등하지 않게 — 핵심은 놓칠 수 없게, 부차는 숨김** | S3 | `synopsis`/`planning` 레인과 RFC 필요 |

### 보류 (H) — 판단 근거가 아직 부족

| # | 보류 항목 | 왜 보류인가 | 해제 조건 |
|---|---|---|---|
| H1 | 색약 대체 세트 A/B 확정 | `style-guide.md` §2.1이 스스로 "실사용자 검증 0명"이라 명시 | 시뮬레이터가 아닌 **실사용자** n≥1. XAG 103도 시뮬레이터를 사용자 테스트 대체로 쓰지 말라고 명시 |
| H2 | 45° 2px 빗금 형태 부호 | §2.1이 요구하지만 **코드·에셋 어디에도 구현 흔적을 못 찾았다** [UNVERIFIED] | 절차적(Rect/Panel) 구현으로 충분한지 먼저 판정 → 충분하면 GTI 불필요 |
| H3 | 고대비 7:1 모드 | XAG 102 권장이지만 팔레트 8색 중 7:1을 넘는 조합이 사실상 1~2쌍뿐 | 별도 고대비 팔레트를 캐논으로 추가할지 worldview/컨셉 RFC |
| H4 | 몰입 점수(immersion_target) 산정 | M19가 스스로 N/A 처리, QA 실측 0건 | `qa/immersion-scores.md` 실측 생성 이후 |

### 기각 (R)

| # | 기각 항목 | 사유 |
|---|---|---|
| R1 | 미세 펄스·호흡 글로우·깜빡임으로 "은은한 상태 변화" 표현 | XAG 117이 명시적으로 정지·중단 옵션을 요구하는 부류. §10-8(블룸 금지)와도 충돌 |
| R2 | 카메라 흔들림·미세 패럴랙스·모션블러로 긴장 조성 | XAG 117 직접 금지 + `style-guide.md` §5 "카메라 이동·팬·줌 없음" |
| R3 | 회색 처리(graying)만으로 비활성 표시 | XAG 103 "another method is also used" 위반. M19 P3은 눈금 막대 제거라는 **형태 채널이 이미 있으므로 유지**하되, 색만 쓰는 축소는 금지 |
| R4 | 붉은 강조·경고색으로 증거 하이라이트 | `style-guide.md` §10-7 직접 금지. S1의 "muted dominating + small saturation"과도 반대 |
| R5 | 사실적 고주파 텍스처로 "아카이브다움" 강화 | S1: 고주파 디테일이 명도 강조 능력을 압도한다. 증거 텍스트 배경에서는 정확히 역효과 |
| R6 | Blue Prince 등 참고작의 카드/HUD 배치 차용 | `video-study.md` 기각 항목 + §10-2. 관찰은 **동사 순서까지만** |
| R7 | 힌트 idle · 판독기 crank · M20 승격 관련 제안 | **OMP 진행 중 — 중복 금지 지시** |

---

## 4. [DERIVED] 이번 조사가 새로 발견한 결함 후보 2건 (사용자/부모 판단 필요)

**D1 — M19 보조 설명 색이 3:1 게이트를 통과하는지 아무도 계산하지 않았다.**
`t0-action-plate-m19.md` §1.1은 보조 설명을 "배경 쪽으로 **0.32 보간**"이라고 정의한다.
`paper #E7E3D8` 를 `ink #173238` 쪽으로 0.32 보간하면 **`#A4AAA5`** 이고 [DERIVED] 대비는:

| 배경 | 약화색 `#A4AAA5` 대비 | XAG 102 판정 |
|---|---|---|
| `ink #173238` (게이트 OFF, 현재 단색 배경) | **5.72:1** | 4.5:1까지 통과 — **현재는 안전** |
| 3 청회 `#36565C` | **3.36:1** | 3:1만 통과, 표준 4.5:1 **미달** |
| 5 콘크리트 `#8A8E88` | **1.41:1** | **완전 미달** |

즉 **게이트 OFF 상태는 안전하지만, 배경이 밝아질수록 급격히 무너진다.**
이는 M7 스킨의 `ink`/`paper`가 문서에 적힌 hex와 같다는 **가정** 위의 계산이며
`M7UiSkin.asset`의 실제 값은 확인하지 않았다 [UNVERIFIED].
**M20 r02 텍스처가 깔리면 배경이 더 이상 단색 ink가 아니므로 위 5.72:1은 무효가 된다** —
M20 §8-3의 `NOT-MEASURED`가 정확히 이 지점이고, r02의 밝은 영역이 콘크리트 명도까지 오르면
보조 설명은 **1.41:1까지 내려갈 수 있다**. 게이트 ON 상태의 보조 설명 대비는 **별도 계산 필수**다.

**D2 — 팔레트 5(젖은 콘크리트)는 어떤 본문 색과도 4.5:1을 만들지 못한다** (§2 표).
`style-guide.md` §8은 저지대 주거지·제방을 콘크리트 지배 재질로 규정하는데, 그 공간의
화면 위에 종이색 본문을 얹으면 2.60:1이다. 공간 연출 규칙과 텍스트 배치 규칙이
**서로 모르는 상태**다. 캐논 변경이 아니라 **"콘크리트 대역 위 본문 금지 / 잉크판을 깔고 올린다"**
는 배치 규칙 한 줄로 해소 가능하다. 이건 컨셉+연출 공동 RFC 대상이다.

---

## 5. GTI로 새로 만들 최적 자산 — **최대 2개**

두 후보 모두 **텍스트 0 · 숫자 0 · 로고 0 · 증거 내용 0 · 스포일러 0**이고,
`assets/generated/2d/**` 의 기존 45+개 항목 및 OMP 진행분과 겹치지 않는다.
**둘 다 `runtimeEligible:false`로 시작**하며 승격은 decision-log 감사로만 (§11 계약 유지).

> 생성 자체는 이 세션이 실행하지 않았다. 아래는 **프롬프트 초안 + 통합 계약 + 검증 기준**이다.

---

### 자산 1 — `nav-quiet-field-r01` (사건 열 정적 여백 배경판)

**비중복 판정**: M20 r02는 `T0Interface.cs:127`의 **`"Work Surface"` 패널**(x 0.46~1.0) 배경이다.
이 자산은 **`T0Interface.cs:111` 좌측 `"Navigation Viewport"` 열**(사건 카드 리스트)의 배경으로,
**패널·프로파일·게이트가 전부 다르다**. `T0Interface.cs:105`/`:174`의 헤더·툴바 `M7 frame`도 건드리지 않는다.
현재 이 좌측 열에는 **배킹이 하나도 없다** (`grep` 확인: `SkinBacking` 호출은 header·toolbar·workSurface 3곳뿐).

**용도**: 플레이어가 사건 조각을 **훑는(scan)** 표면. S1이 말한 "고주파 디테일이 명도 강조를 압도한다"를
정면으로 피하고, S2의 negative space를 **의도된 정적 여백**으로 만든다. 카드가 뜨는 자리는 비어 있고,
마모·염 자국은 **열의 바깥 여백에만** 산다.

**톤 필러 매핑**: 조용한 압력 (수평 젖음 경계선 1개가 열을 가로지른다).

**프롬프트 초안** (하우스 포맷 `concept/prompts/*.txt` 4절 구조 준수):

```
SUBJECT: an empty vertical strip of a tide records office interior wall, seen flat-on. A tall
narrow field of wet concrete and dark painted steel, with one single horizontal wet-dry boundary
line crossing the strip in its upper third. Salt bloom and corrosion runs occur only along the far
left and far right margins of the strip; the wide central column of the strip is deliberately empty,
smooth and quiet, with no props, no fixtures, no pipes and no openings. Nothing hangs on this wall.

STYLE: painterly semi-realistic game concept art for a quiet maritime-industrial mystery. Palette is
strictly limited to deep ink #0E1F26, seal ink #173238, wet-metal blue-grey #36565C, verdigris bronze
#4F7A6B, wet-concrete grey #8A8E88, salt-crystal pale cyan #C8D6D3. No ochre accent in this image.
The entire central column of the strip must stay within the dark half of the palette, at or below the
value of wet-metal blue-grey #36565C; wet-concrete grey may appear only in the outer margins.
Overcast night lighting from scattered low work lamps, very low contrast, soft diffuse falloff,
minimal visual noise, no high frequency detail. MATERIALS, applied literally: salt crystal grows as
fine hexagonal clusters at edges with matte scatter and pinpoint highlights; corroded bronze is a dark
base with verdigris speckle and run marks that always flow downward; wet concrete reads twenty percent
darker than dry concrete with a hard horizontal tide line marking past water level. Nothing is new,
nothing is glossy, everything has been worn by salt and water.

CAMERA: flat orthographic wall elevation, no perspective convergence, no vanishing point, no floor and
no ceiling in frame, no implied camera movement. Single continuous surface, one composition, not a tile
and not a repeating pattern. Tall vertical aspect.

NEGATIVE: no text, no letters, no words, no numbers, no captions, no signage, no labels, no logos, no
watermark, no signature, no laurel wreath, no award badge, no review score, no user-interface copied
from any existing game, no real-world disaster photography, no weapons, no combat, no blood, no corpses,
no ghosts, no apparitions, no supernatural glow, no anime style, no glossy CGI render, no neon, no lens
flare, no bloom, no red warning lights, no chrome, no brand marks, no documents, no paper, no notes, no
photographs, no maps, no charts, no seals, no stamps, no obvious repeating pattern, no border frame,
no vignette, no icons, no people, no hands.
```

**통합 위치 (정확 좌표)**
- 대상 패널: `unity/Unknown/Assets/_Project/UI/T0Interface.cs:111` `Rect("Navigation Viewport", left, …)`
  의 **부모 `left` 패널**. 배킹은 `left`의 **첫 자식(index 0)** 으로 삽입해 카드·텍스트가 전경에 남게 한다.
- 재사용 헬퍼: `T0Interface.cs:250-257` 의 **`FullBleedBacking()`**(M20이 이미 만든 비-타일 헬퍼).
  **새 헬퍼를 만들지 않는다.** `uvRect=(0,0,1,1)`, `raycastTarget=false`, 임포트 `wrapMode=Clamp`.
- **`backings` 리스트(`T0Interface.cs:81`, `TiledBacking`)에 등록하지 않는다** — 등록하면 `uvRect`가
  타일 값으로 덮여 M20 T3와 같은 위반이 된다.
- 게이트: M20과 **별도 진단 플래그**(예: `--m21-nav-field-diagnostic`) + `runtimeApproved:false` 고정.
  M20 프로파일 에셋을 재사용하지 않는다(승인 범위 오염 방지).

---

### 자산 2 — `state-mark-strip-r01` (무문자 상태 표식 3분할 스트립)

**비중복 판정**: 기존 `assets/generated/2d/ui/ui-status-badge-sheet.png`는 **뱃지(아이콘) 시트**이고,
이 자산은 **표면 처리(surface treatment)** 다 — 아이콘을 추가하는 게 아니라 **판/카드 표면 자체의 결**을
바꾼다. M19는 **평면 단색 + Bevel/Rule**만 쓰므로(`t0-action-plate-m19.md` §2) 겹치지 않고,
M20은 작업면 배경이라 겹치지 않으며, OMP의 crank/idle/M20과도 무관하다.

**용도**: XAG 103이 요구하는 **색 이외의 두 번째 채널**을 애니메이션 없이 제공한다(S5-117 준수).
가로 3분할, 각 슬라이스가 하나의 상태에 대응하는 **무문자 표면 결**:

| 슬라이스 | 상태 | 표면 결 | 색 단독 아님을 보장하는 축 |
|---|---|---|---|
| 좌 1/3 | 미확정 (open) | 평활한 넝마지 결, 눌림 없음 | 질감 없음 |
| 중 1/3 | 대기/보류 (pending) | 45° 미세 빗금 결 (§2.1이 요구한 형태 부호) | 방향성 있는 결 |
| 우 1/3 | 확정 (sealed) | **글리프 없는 빈 압인 눌림** — 종이가 눌린 깊이만 남음 | 요철(음영) |

**톤 필러 매핑**: 절차의 무게. `style-guide.md` §9 `seal`("책임이 형태를 얻는다")의 **결과**를
문자 없이 표면으로만 표현한다.

**스포일러 안전**: 압인 안에 **기관명·서명·날짜·글리프가 전혀 없다**. 어떤 사건의 어떤 기록이
확정됐는지 이미지가 말하지 않는다 — 상태는 **런타임이 어느 슬라이스를 샘플하느냐**로만 결정된다.

**프롬프트 초안**:

```
SUBJECT: a flat archival study sheet of three surface treatments on the same sheet of rag paper,
arranged as three equal vertical thirds of one wide image, separated only by a soft change in the
paper surface itself and never by a drawn line, border or divider. LEFT THIRD: plain undisturbed rag
paper, smooth, untouched. CENTRE THIRD: the same paper carrying fine parallel diagonal hatching
running at forty five degrees, a shallow tooling texture pressed into the fibre, evenly spaced and
regular. RIGHT THIRD: the same paper carrying one large blank pressed impression, a circular deboss
with a raised outer ring and a completely empty smooth centre, the paper fibre crushed and lifted
around the rim, catching a faint pale salt bloom in the crushed fibre. The impression is entirely
empty: no emblem, no device, no mark, no motif inside it.

STYLE: painterly semi-realistic game concept art, archival material study, top-down flat lighting.
Palette is strictly limited to seal ink #173238, wet-metal blue-grey #36565C, wet-concrete grey
#8A8E88, salt-crystal pale cyan #C8D6D3 and records-paper warm off-white #E7E3D8. No ochre accent.
Low contrast, matte, no gloss anywhere. Nothing is new; the paper is aged by salt and damp but is not
torn, burnt or stained with liquid. Differences between the three thirds must be readable from surface
relief and texture direction alone, so that the three thirds remain distinguishable when the image is
converted to greyscale and when it is converted to a one bit silhouette.

CAMERA: flat orthographic top-down copy-stand view, no perspective, no page curl, no drop shadow onto
a table, no surrounding desk, no implied camera movement. Wide aspect, one continuous sheet.

NEGATIVE: no text, no letters, no words, no numbers, no captions, no signage, no labels, no logos, no
watermark, no signature, no monogram, no crest, no emblem, no heraldry, no seal device, no stamp
imagery, no barcode, no QR code, no handwriting, no ink strokes, no drawings, no diagrams, no maps, no
charts, no fingerprints, no laurel wreath, no award badge, no review score, no user-interface copied
from any existing game, no real-world disaster photography, no weapons, no combat, no blood, no
ghosts, no supernatural glow, no anime style, no glossy CGI render, no neon, no lens flare, no bloom,
no red warning lights, no chrome, no brand marks, no vignette, no border frame, no icons, no people,
no hands.
```

**통합 위치 (정확 좌표)**
- 소비 지점 후보 1: 사건 카드 — `T0Interface.cs:120-122` (`card` / `"CaseThread"`, 이미
  `raycastTarget=false` 규칙이 서 있는 자리)의 **비-raycast 자식 배킹**.
- 소비 지점 후보 2: 액션 판 — `T0Interface.cs`의 `Button()`이 만드는 판. **단, M19 §2가
  "버튼에 텍스처를 새로 붙이지 않는다"고 스스로를 묶고 있으므로 여기 적용하려면 M19 개정이 선행**이다.
  → **기본 권고는 후보 1만.**
- 샘플링: 슬라이스 3개를 `uvRect`의 **x 오프셋 0 / 1⁄3 / 2⁄3, width 1⁄3** 로 **1회씩만** 샘플.
  반복 아님. `wrapMode=Clamp` 필수(경계 슬라이스가 반대편으로 새는 것을 엔진 수준에서 차단).
- 게이트: 자산 1과 **또 별도 플래그**로 분리. 두 자산을 한 게이트에 묶지 않는다
  (한쪽이 반려돼도 다른 쪽이 살아남게).

**⚠️ 자산 2의 정직한 반대 논거 (부모가 먼저 판정할 것)**
45° 빗금과 압인 링은 **`Rect`/`Panel` 프리미티브 + 코드로 그릴 수 있는 형상**일 수 있다.
그렇다면 GTI 자산은 **불필요한 에셋 추가**다(M19가 스스로 지킨 "새 에셋 0건" 원칙과 충돌).
GTI가 정당화되는 유일한 근거는 **종이 섬유의 눌림·염 결정 침착 같은 미세 재질**이며,
그 재질이 실제로 필요한지는 부모의 판단 사항이다. **필요 없다고 판단되면 자산 2는 폐기하고
자산 1만 진행할 것을 권고한다.**

---

## 6. 검증 기준 (전부 오프라인·무유료·자동 판정 가능 형태)

### V1. 문자 부재 (두 자산 공통)
- 육안 검수 + 가능하면 OCR 1회. **육안만 했다면 "OCR 미실행"을 provenance에 명시** —
  M20 §1.1이 이미 이 한계를 정직하게 적어둔 선례를 따른다.
- 판정: 글리프·숫자·로고 **0개**. 1개라도 있으면 REDO(대체 편집 금지, 재생성).

### V2. 팔레트 준수 (`style-guide.md` §2)
- 전 픽셀을 팔레트 8색 + 예비 적갈에 최근접 매핑했을 때:
  - 8 황토 `#E2AF62` 비율 **≤ 8%** (자산 1·2 모두 프롬프트에서 아예 배제했으므로 사실상 ≈0%)
  - 예비 적갈 `#8C4A3A` **≤ 2%**
  - 자산 1: 3 청회 25~45%, 5 콘크리트 15~35%
- 명도 3대역(최암부 1~2 / 중간 3~5 / 최명부 6~7)이 **각각 ≥10%** (§2 명도 대역 규칙).
  미달 시 "중간값만의 회색 컷" → REDO.

### V3. 대비 게이트 [DERIVED, 실측 아님]
- 자산 1: **텍스트가 놓일 중앙 컬럼**(가로 중앙 70% × 세로 전체)의 **모든 픽셀**에 대해
  WCAG 상대휘도 공식으로 `paper #E7E3D8` 대비를 계산 → **최솟값 ≥ 4.5:1** (XAG 102 표준 텍스트).
  §2 계산표상 이는 **배경 픽셀 휘도가 팔레트 3(청회) 이하**여야 한다는 뜻이다.
- 자산 1: 같은 영역에서 M19 보조 설명 약화색(**`#A4AAA5`**, §4 D1 계산) 대비 **≥ 3:1** (XAG 102 inactive).
  이 조건은 배경 픽셀이 청회 `#36565C`보다 어두울 때만 선다(청회에서 이미 3.36:1).
- 자산 2: 세 슬라이스 각각에서 `ink #173238` 대비 **≥ 4.5:1** (카드 본문이 잉크색이라면).
- **이 값들은 텍스처 자체의 산술이며 최종 화면 대비가 아니다.** 알파 합성·틴트·톤매핑 이후의
  실측은 여전히 `NOT-MEASURED`이고, 그렇게 보고해야 한다.

### V4. 비-타일 / 저반복 (S1 + M20 T1~T5)
- 자기상관 또는 2D FFT로 **주기적 격자 피크 부재** 확인. 정량 임계값을 지금 발명하지 않고,
  **판정을 "M7 타일 텍스처 대비 상대 비교"로** 한다(`assets/generated/2d/texture/m7-*` 가
  타일 기준선이고 `m7-tiling-qa-report.json`이 이미 존재한다 → 같은 스크립트 재사용).
- 계약 검사: `uvRect.width == 1`(자산 1) / `== 1/3`(자산 2), `wrapMode == Clamp`,
  `backings` 리스트 **미등록**, 형제 index **0**, `raycastTarget == false`.

### V5. 색 제거 생존성 (XAG 103 + `style-guide.md` §6·§9)
- 자산 2를 **그레이스케일 변환** 후, 그리고 **1비트 임계 변환** 후에도
  세 슬라이스가 **상호 구분**되는지 육안 판정. 구분 안 되면 형태 채널이 실패한 것 → REDO.
- 자산 1은 이 검사 대상이 아니다(정보를 전달하지 않는 배경이므로).

### V6. 불변식 회귀 (M19·M20 계약 보존)
- 게이트 OFF: 대상 패널의 **자식 수·형제 index·사각형이 OFF와 바이트 동일**.
- 게이트 ON/OFF 공통: **액션 ID 순서 · 라벨 문자열 · `interactable` · 포커스 가능 수 ·
  M19 Bevel/Rule · 포커스 반전 · 텍스트 3단 위계 전부 동일**.
  (`t0-work-surface-m20.md` §7-5의 수용 기준을 그대로 재사용한다 — 새 기준을 발명하지 않는다.)
- `T0Strings.json` 무변경, `PuzzleCommand` 제출 0건, `assets/generated/**` 기존 항목 편집 0건.

### V7. provenance
- `runtimeEligible:false`, `promoted_by:null`, `license: UNVERIFIED (generated; check backend ToS
  before commercial use)`, **요청 크기와 실제 크기를 둘 다 기록**(백엔드가 요청 크기를 무시하는
  것이 이미 2회 관측됐다: M20 r02 `2048x1152 → 1672x941`, m8-review-card `1024x1024 → 1536x1024`).
- `concept/references.md` §1은 **제3자 참고 0건 상태를 유지**한다(이번 자산도 참고 이미지 0장).

---

## 7. 부모가 바로 쓸 수 있는 좌표 요약

| 항목 | 좌표 |
|---|---|
| 연출 계약 정본(현재) | `_workspace/current/presentation/t0-action-plate-m19.md`, `t0-work-surface-m20.md`, `video-study.md` |
| **없는 파일** | `_workspace/current/presentation/presentation-spec.md` (지시문 좌표 오류) |
| 스타일 계약 | `_workspace/current/concept/style-guide.md` §1 톤필러 / §2 팔레트·명도 / §5 카메라 / §6 매체 실루엣 / §9 도구 / §10 금지 / §11 산출물 |
| 원본 허용목록 | `_workspace/current/concept/concept-first-m7-sources.json` (`mode: exact-path-and-sha256-allowlist`, `default: deny`) |
| 권리 대장 | `_workspace/current/concept/references.md` (제3자 참고 **0건** 유지) |
| 프롬프트 하우스 포맷 | `_workspace/current/concept/prompts/*.txt` (SUBJECT / STYLE / CAMERA / NEGATIVE 4절) |
| 작업면 패널 | `unity/Unknown/Assets/_Project/UI/T0Interface.cs:127` |
| M7 타일 배킹 호출 | `T0Interface.cs:132` (M20이 대체) |
| **좌측 사건 열(자산 1 대상)** | `T0Interface.cs:111-113`, 카드 `:120-122` |
| 헤더·툴바 배킹(불변) | `T0Interface.cs:105`, `:174` |
| 타일 배킹 헬퍼 | `T0Interface.cs:243-248`, 리스트 `:81` |
| **비-타일 헬퍼(재사용)** | `T0Interface.cs:250-257` |
| Signal / Overlay 그래픽 | `T0Interface.cs:147`, `:153` |
| 힌트 표면(**OMP 소관, 손대지 말 것**) | `T0Interface.cs:58-61` `hintOfferPanel` / `hintOfferText` |
| 타일 QA 기준선 스크립트 산출물 | `assets/generated/2d/texture/m7-tiling-qa-report.json` |

---

## 8. 출처 목록

1. Mitchell, Francke, Eng — *Illustrative Rendering in Team Fortress 2*, NPAR '07 (Valve).
   https://steamcdn-a.akamaihd.net/apps/valve/2007/NPAR07_IllustrativeRenderingInTeamFortress2.pdf ·
   https://doi.org/10.1145/1274871.1274883
2. Thomas Grip — *Gaps of the Imagination*, Frictional Games, 2017-06-20.
   https://frictionalgames.com/2017-06-gaps-of-the-imagination/
3. Thomas Grip — *Storytelling through fragments and situations*, Frictional Games, 2010-03-15.
   https://frictionalgames.com/2010-03-storytelling-through-fragments-and-situations/
4. Microsoft — *Xbox Accessibility Guideline 102: Contrast* (XAG v3.2, 2023-06-08).
   https://learn.microsoft.com/en-us/gaming/accessibility/xbox-accessibility-guidelines/102
5. Microsoft — *XAG 117: Visual distractions and motion settings*.
   https://learn.microsoft.com/en-us/gaming/accessibility/xbox-accessibility-guidelines/117
6. Microsoft — *XAG 103: Additional channels for visual and audio cues*.
   https://learn.microsoft.com/en-us/gaming/accessibility/xbox-accessibility-guidelines/103
7. (보조) Microsoft — *XAG 109: Objective clarity*.
   https://learn.microsoft.com/en-us/gaming/accessibility/xbox-accessibility-guidelines/109
8. (보조) Microsoft — *XAG version history* (비활성 요소 2.5:1 → 3:1 상향 근거).
   https://learn.microsoft.com/en-us/xbox/accessibility/xag-version-history
9. (보조) Thomas Grip — *4-Layers, A Narrative Design Approach*, 2014-04-29.
   https://frictionalgames.com/2014-04-4-layers-a-narrative-design-approach
