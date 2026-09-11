---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-concept-artist
---

# C1 M4 — 서명지와 판독대 아트 브리프

## 정본·사용 범위

[OBSERVED] `planning/campaign.json:c1-b2`의 겹쳐 붙은 서명지, `worldview/glossary.md`의 서명지(`signature-annex`)·판독기(`reader`)·판 #0(`plate-zero`)를 시각화한다. 장소는 당직실이며 새 장소·인물·도구·이름을 추가하지 않는다. 습도판 받침은 기존 판독기 작업면의 부속 형상으로 취급한다. 연출 선행 계약은 `presentation/c1-signature-presentation.md`다.

[OBSERVED] 기존 `concept/prompts/tool-reader-hero.txt`와 `assets/generated/2d/concept/tool-reader-hero.png`가 판독기의 원형 받침·스타일러스 암·확대렌즈 형태를 이미 규정한다. 이번 작업은 그 방향을 재사용한다. M3 네이티브 panel-front와 T0 gameplay JPG를 시각 검토했다. 청회 금속/회백 종이/어두운 외곽의 연속성을 유지한다. 아카이브와 concept/sheets에서 별도 signature/reader 이름의 시트는 찾지 못했다. 기존 컨셉 프롬프트·이미지를 대신 참고하며 임의의 전용 시트 승인을 주장하지 않는다.

[TARGET] 톤 필러는 **절차의 무게**, 보조는 **젖은 금속과 소금**이다. 조형은 낮은 원형 판독기 + 옆의 얕은 종이 받침 + 분명히 구별되는 종이 두 면이다. 분리되어도 하단 소금 그늘은 남는다. 작품의 문장·번호·서명·이름은 생성 자산에 없다.

## 팔레트와 표면

| 용도 | 기준 |
|---|---|
| 암부·윤곽 | `#0E1F26`, `#173238`; 중요한 윤곽 1080p 기준 3 px 이상 |
| 판독대 몸체 | `#36565C`; 새 페인트·크롬·LED 없이 닳은 모서리와 낮은 광택 |
| 부식 부속 | `#4F7A6B`; 어두운 바탕 위 불규칙 녹청, 아래로 흐른 자국 |
| 소금 | `#C8D6D3`; 가장자리 미세 육각 군집, 무광, 눈·설탕 덩어리 금지 |
| 종이 | `#E7E3D8`; 잔섬유·눌린 가장자리, 본문 뒤는 저대비 |
| 선택 악센트 | `#E2AF62`; 전체 프레임 8% 이하, UI의 변경 예정에 한정 |

형태·색·배치를 같이 사용한다. 원본과 사본 구분은 테두리와 한국어 라벨에 있고 종이 색만으로 판정하지 않는다. 서명 두 개의 필압·번짐 차이는 다음 비트이므로 이 자산으로 먼저 결론 내리게 하지 않는다.

## 제작 요청 A — Higgsfield 종이 표면

- 자산 id 제안: `c1-signature-paper-r01`. 하나의 **무문자 재사용 표면**이며 증거 이미지를 생성하지 않는다.
- 요청 크기 [TARGET]: 1024 × 1024 PNG, 정면 정사영, 가장자리까지 차는 종이 표면. 프레임·원근·바닥 그림자·물체 외부 배경 없음. 중앙 84% 영역은 균일하게 읽기 좋은 저대비.
- 원본 출력은 그대로 보존하고 요청/실제 크기를 각각 기록한다. 접착 소금·하단 가림·본문·페이지 번호·출처 관계는 별도 UI 레이어다. 생성 이미지 픽셀에서 사실을 추출하지 않는다.
- 같은 표면을 두 원본과 확보한 두 사본에 재사용한다. 구분은 시스템 상태·레이아웃·문자로 만든다. 소금 그늘 아래 글자를 숨기지 않는다.

```text
Create one original flat orthographic material swatch for a quiet maritime-industrial mystery game: worn records paper, warm off-white #E7E3D8, very subtle compressed fibres, slight cool-grey damp mottling, softly worn fibres near the outermost edge. The material fills the entire square image edge to edge. Keep the central eighty-four percent extremely quiet and low contrast so separately rendered dark UI text remains readable. Painterly semi-realistic surface detail, matte diffuse light, no directional cast shadows, no perspective, no object border, no surrounding tabletop. Restrict colour to records-paper warm off-white #E7E3D8, wet-concrete grey #8A8E88 used sparingly, salt-crystal pale cyan #C8D6D3 as a subtle tint only. This is a blank substrate, not a historical document and not a solved clue.

NEGATIVE: no text, no letters, no words, no numbers, no numerals, no handwriting, no signatures, no ink strokes, no stamps, no symbols, no glyphs, no captions, no labels, no logos, no watermark, no graph, no chart, no borders, no page grid, no stains resembling writing, no torn holes, no salt deposits, no adhesive salt edge, no dark lower obstruction, no snow, no sugar crystals, no photographic document, no modern clean office paper, no glossy CGI, no neon, no bloom, no lens flare, no red warning lights, no weapons, no real-world disaster imagery.
```

Provider runner: 사용자가 지정한 Higgsfield를 우선하며 root가 연결된 실행 수단을 선택한다. MuAPI 미인증 상태에서 생성 성공을 기록하지 않는다. 다른 제공자의 출력을 Higgsfield 생성물로 표기하지 않는다.

## 制作 요청 B — Blender 판독기·습도판 받침

자산 id 제안: `c1-signature-reader-r01`. 기존 reader의 시각적 구현이며 새 도구가 아니다. Blender에서 별도 새 scene으로 제작하고 사용자의 기존 scene을 지우지 않는다.

| 항목 | 저작 목표 [TARGET] |
|---|---|
| 실루엣 | 좌측 낮은 원형 판독기, 후면의 짧은 스윙암·확대렌즈, 우측 얕은 사각 종이 받침 |
| 블록아웃 크기 | 폭 0.72 m × 깊이 0.42 m × 높이 0.28 m 이내; 미터 단위, pivot은 받침 바닥 중앙 |
| 폴리곤 | 전체 6,000 triangles 이하; 네이티브 실측값 별도 기록 |
| 메시·재질 | 정적 메시 8개 이하, 재질 그룹 4개 이하; 투명 유리 대신 어두운 무광 렌즈면 사용 가능 |
| 텍스처 | 공유 1024² metal PBR maps 4장 이하 + 요청 A의 별도 UI 종이 1장; 4K 없음 |
| 부속 | 원형 받침·스윙암·렌즈 링·짧은 크랭크·얕은 습도판·무문자 라벨 홈; 버튼과 단계는 UI가 표현 |
| 출력 | `.blend`, `.fbx`, `.glb`, maps, 1024² preview, triangle/mesh/material/byte measurements |
| 가동부 | 이번 납품은 정적; rig/animation/바늘 상태·습도 표시 없음 |
| 충돌 | 시각적 장식만. 상호작용 collider나 물리 논리 불필요 |

```text
Build an original low-poly static plate-reader and shallow paper-treatment tray for the existing reader tool in a quiet maritime-industrial mystery. Use metres and place the origin at the centre of the base. The overall footprint fits inside 0.72 m width, 0.42 m depth, 0.28 m height. A shallow circular reader cradle occupies the left; a short rear swing arm with a round magnifier ring and simple side hand crank makes the reader silhouette recognisable. A low rectangular tray occupies the right and can support two paper sheets presented by the UI. Use rounded worn edges, sparse bolts, and a single short constant-diameter coupling with a matte white salt ring. Reserve blank inset label recesses only.

Materials: worn wet-metal blue-grey #36565C over deep ink #0E1F26, a few dark-base verdigris bronze #4F7A6B spots with downward run marks, matte salt-crystal pale cyan #C8D6D3 as fine edge clusters. Keep metal rough and old. No glossy chrome or bright emissive surfaces. Put the visual weight on the two readable paper planes and the round mechanical cradle; avoid busy ornament. Fixed presentation camera, no animated gauges. Export static meshes and material groups with measured counts, source file and provenance.

NEGATIVE: no text, no letters, no words, no numbers, no numerals, no scale markings, no labels containing glyphs, no logo, no watermark, no signature, no named document, no historical reproduction, no extra tool, no futuristic display, no LED, no neon, no bloom, no lens flare, no red warning light, no weapons, no supernatural elements, no animated moisture, no visual state that pretends to solve the puzzle.
```

## UI 서명지 구성과 납품 경계

[TARGET] UI는 A의 표면 위에 다음 층을 구성한다. 코드 구현은 systems 소유이며 이 문서는 시각 계약만 정의한다. [OBSERVED · director ACK] RFC-CX-005가 이 정적 리소스 방향과 0 ms 상태 컷을 승인했다. 아래 좌표는 planner가 승인한 정규화 영역 주석이며 물리 측정이 아니다.

1. 모서리가 둥근 사각 종이 두 면. 접착 상태는 너비 약 12%의 수평 어긋남으로 겹치고 분리 후에는 겹침이 없다. 이 배치는 시각 치수이며 증거 좌표가 아니다.
2. 가장자리에만 놓는 접착 소금 레이어. 분리 완료 상태에서만 숨긴다. 종이 표면에 굽지 않는다.
3. 두 번째 면 아래의 불투명한 소금 그늘 레이어. M4 모든 상태에서 유지하며 아래에 이름이나 후속 문자를 렌더하지 않는다. 정규화 종이 좌상단 기준 `(x: 0.12, y: 0.70, width: 0.76, height: 0.22)` 주석을 정의 데이터에서 읽는다. 이는 미해결 영역 표시이며 이름 복원 영역의 물리 실측이 아니다. 증거 좌표는 같은 데이터로 별도 문장 표시한다.
4. 관찰 후 본문·매수·번호 관계·출처 라벨. 한국어 UI 폰트로 렌더하고 생성 이미지에 포함하지 않는다. 두 사본은 같은 원전에서 나왔음을 유지한다.
5. 플레이어의 명시적 대조 뒤에만 육각 `plate-zero` 매체 카드와 확인된 대역 관계를 표시한다. 정본에 없는 수치축이나 실측 곡선을 그리지 않는다.

신규 이미지는 A 한 점, 신규 3D는 B 한 세트다. 기존 자료 프레임·아이콘·T0 폰트를 재사용한다. 판독대의 비어 있는 배경은 기존 암부 `#0E1F26`로 맞추고 기본 Unity 하늘·수평 지평선은 없앤다. 이는 판독기 관찰 영역의 한정 배경 변경이며 새로운 환경 장면 제작이 아니다. 생성 서명·인물 초상·영화 컷·환경 영상·신규 VFX는 이번 범위에 필요하지 않다.

## 출처·검수 계약

모든 생성 자산은 `runtimeEligible:false`로 시작한다. 폴더 `provenance.json`에 실제 provider/tool/version/model, 프롬프트, 요청/실제 크기, output 해시·바이트, 생성 시각, 권리 확인 상태, recipe와 원본 출력 경로를 보존한다. 이 브리프 자체는 생성 영수증이나 런타임 사용 승인이 아니다.

Root/모델러는 출력 시각 검사 → Unity 진단 임포트 → 실제 레이아웃·상태별 캡처 순서로 확인한다. 범위 한정 승격은 `production/decision-log.md`에 감사 근거와 함께 기록한다. 이미지 생성 실패나 텍스트 혼입은 원본/실패 사유를 보존하고 채택하지 않는다.

| 검수 | 채택 기준 [TARGET] |
|---|---|
| 종이 | 원문자·숫자·문자 같은 얼룩이 없고, 중앙 본문 대비를 해치지 않음 |
| 모델 | 원형 판독기와 얕은 종이 받침이 작은 관찰 영역에서도 구분됨; 클리핑·뒤집힌 면·사라진 재질 없음 |
| 일관성 | 정식 8색·무광·마모·수평 작업면·차분한 고정 시점 유지 |
| 서사 | 접착 해제와 하단 미해결 가림이 별개; 서명 이름/필적 차이/후속 봉인 접점 선공개 0 |
| 접근성 | 150%에서 한국어 본문이 이미지와 독립적으로 확대·스크롤되고, 저감모션에서도 모든 상태 동일 |
| 증거 | preview는 preview라고 표시; 네이티브 캡처만 runtime capture라고 표시; G4/G5 실측은 별도 |
