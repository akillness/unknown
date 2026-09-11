---
updated: 2026-09-10
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-presentation-director
---

# 미디어 화법 판정 — README·상점 정본 화법과 프리비즈 GIF 2종

## 0. 이 문서의 지위
이 문서는 **화법(畵法) 판정과 재생성 범위 제안**이다. 상점 게시·자산 승격·외주 발주의 근거가 아니다.
여기서 다루는 모든 이미지·GIF는 `docs/media/provenance.json` 기준 `runtimeEligible: false` 이며 `claim` 에 `NOT gameplay` 가 붙어 있다 [OBSERVED].
**게임플레이라고 부르지 않는다** — Unity 실캡처는 0건이다.

## 1. 실제로 무엇이 있는가 [OBSERVED 2026-09-10]

측정 명령: `ffprobe -v error -select_streams v:0 -show_entries stream=width,height,nb_frames,avg_frame_rate -of default=nw=1 docs/media/<f>.gif` · `wc -c` · 프레임 직접 시각 검수.

| 파일 | 해상도 | 프레임 | fps | 바이트 | 유래 (`provenance.json`) |
|---|---|---|---|---|---|
| `docs/media/previz-cutscene-video.gif` | 640×360 | 101 | 10 | 9,185,383 | `previz-clip-01-circuit-to-water.mp4` + `previz-clip-02-valve-trial.mp4` |
| `docs/media/previz-cutscene-concept.gif` | 960×540 | 9 | 100/71 (≈1.41) | 3,548,951 | `assets/generated/2d/previz/previz-f01..f09` (GTI 체인) |
| `docs/media/previz-hub-turntable.gif` | 640×360 | 12 | 25/3 (≈8.33) | 1,048,236 | `assets/generated/3d/renders/turntable/frame_*.png` |

재생 길이는 프레임/​fps 로 계산한 값이다 — 각각 약 **10.1초 · 6.4초 · 1.4초** `[INFERENCE]`. **GIF 재생 자체(루프 이음새, 첫 프레임 정지 인상, 저사양 브라우저 디코드)는 사람이 눌러 확인해야 하며 확인 0건이다** `[TARGET 검증]`.

## 2. 화법 판정 — 배정 전제와 실측이 다르다 `[OBSERVED]`

이번 배정은 두 프리비즈 GIF 의 스타일 분리를 "**previz 선화 vs concept 회화**"로 전제했다. **프레임을 직접 열어 본 결과 그 분리는 두 GIF 사이에 없다.**

| 표본 | 검수한 프레임 | 관측된 화법 |
|---|---|---|
| `previz-cutscene-video.gif` | `previz-clip-01` n=20, `previz-clip-02` n=20 (ffmpeg 추출 후 직접 열람) | **회화** — 젖은 콘크리트·부식 배관·염결정·게이지 눈금까지 명암과 질감으로 그려져 있다. 윤곽선만으로 성립하는 선화가 아니다 |
| `previz-cutscene-concept.gif` | `previz-f01-circuit-connect.png` (1672×941, 원본 열람) | **회화** — 위와 **같은 구도·같은 화법**이다. 영상 클립이 이 정지 프레임을 움직인 것이며, 16:9 로 잘려 좌우가 좁아졌을 뿐이다 |
| `previz-hub-turntable.gif` | `docs/media/hub-greybox-render.jpg` (같은 렌더 계열, 원본 열람) | **무텍스처 그레이박스** — 회색 셸에 색 블록만 놓인 볼륨 검증용 렌더. 회화도 선화도 아니다 |

**판정**: 두 컷씬 GIF 는 **같은 화법의 정지본/동영상본**이고, 진짜 화법 경계는 **회화(2D 프리비즈) ↔ 그레이박스(3D 볼륨)** 사이에 있다. 배정 문구의 "선화"에 해당하는 자산은 `docs/media/` 에 **존재하지 않는다** [OBSERVED]. 이 판정은 배정을 반박하는 것이 아니라, **없는 축으로 정본을 고르는 일을 막기 위한 것**이다.

### 2.1 지금 README 가 만드는 인상 `[OBSERVED]`
`README.md` L25 는 표 한 줄에 **회화 컷씬 GIF 와 그레이박스 턴테이블 GIF 를 나란히** 둔다. 두 칸의 화법이 다르다는 사실 자체는 정직하지만(각 캡션에 "실제 게임플레이 아님" · "최종 아트 아님"이 붙어 있다), **처음 보는 사람에게는 "왼쪽이 게임이고 오른쪽이 개발 중"으로 읽힐 여지**가 있다. 회화 GIF 는 게임 화면이 아니라 **컷씬 프리비즈**다.
`previz-cutscene-concept.gif` 는 본문 표에 없고 L27 에 링크로만 있다 [OBSERVED].

## 3. `[TARGET]` 정본 화법 제안 — 상점과 README 를 분리한다

| 면 | 제안하는 정본 화법 | 이유 |
|---|---|---|
| **Steam 상점 (1번 트레일러·스크린샷)** | **Unity 실캡처만**. 확보 전에는 **게시 0건 유지** | Steam 공식 트레일러 가이드는 게임플레이 위주·HUD 노출을 권장한다(`presentation/video-study.md` §공식 유통 근거 [OBSERVED]). 회화 프리비즈로 상점 1번 자리를 채우면 **없는 것을 있다고 보여주는 화면**이 된다 |
| **Steam 상점 (보조 아트: 캡슐·배경)** | **회화** | 캡슐은 원래 일러스트 면이며 게임플레이 주장과 충돌하지 않는다 |
| **README 상단(히어로·컷씬)** | **회화 1종으로 통일** | 저장소 첫 화면에서 화법이 섞이면 무엇이 목표 룩인지 읽히지 않는다 |
| **README 개발 진행 절** | **그레이박스** — 단, 상단이 아니라 별도 절로 내린다 | 그레이박스는 "이만큼 만들었다"의 증거지 룩의 주장이 아니다 |

**즉 정본 화법은 회화(painterly previz)이고, 그레이박스는 정본 화법이 아니라 진행 증거다.** 상점 정본은 아직 **어느 화법도 아니며 Unity 캡처를 기다린다**.

## 4. `[TARGET]` 재생성 범위 — 무엇을 다시 만들고 무엇을 손대지 않는가

**다시 만드는 것 (presentation 요청 · 실행은 concept/motion 레인)**
1. `previz-cutscene-video.gif` **용량 다이어트**: 9,185,383 B 는 README 한 장에 싣는 GIF 로 과하다 `[INFERENCE]`. 프레임 수(101)를 줄이거나 팔레트를 재산출한다. **목표 바이트를 지금 숫자로 못 박지 않는다** — 재인코딩 전후를 측정해 결정한다.
2. `previz-cutscene-concept.gif` **역할 재정의**: 현재 9프레임 ≈1.41fps 는 GIF 라기보다 슬라이드쇼다. 이것을 **정지 스토리보드 시트(정적 이미지 1장)** 로 재출력하고 GIF 자리를 비우는 편이 읽기 쉽다.
3. `README.md` L25 표 재편(**README 는 presentation 소유가 아니다 — RFC 로 요청**): 상단은 회화 1종, 그레이박스는 아래 진행 절로.

**손대지 않는 것**
- `assets/generated/**` 원본 프레임과 `provenance.json` — 생성 원본은 역사다. 재인코딩은 **파생본만** 갱신하고 `output_sha256` · `bytes` 를 다시 적는다.
- 상점 게시·자산 승격(`runtimeEligible: false` → true)은 **decision-log 감사로만** 가능하다(CLAUDE.md §9). 이 문서는 승격을 요청하지 않는다.
- 새 회화 프레임 생성 요청 0건. `reuse over new` — 있는 9프레임으로 먼저 재편한다.

## 5. 이 문서가 주장하지 않는 것
- 어떤 GIF 도 **게임플레이가 아니다**. 사람 플레이 n=0, Unity 빌드 0건 [CARRIED].
- 화법 판정은 **정지 프레임 표본 4장(`f01`, clip-01 n=20, clip-02 n=20, greybox 렌더 1장)** 에 기반한다. 전 프레임 전수 검수가 아니다.
- "회화가 더 낫다"는 미적 우열 주장이 아니라, **혼합을 줄이자는 일관성 주장**이다.
- 몰입 점수(G4)는 이 문서로 움직이지 않는다. 측정은 QA 소유다.

## 6. 열린 것
- README 편집 권한: README 는 presentation 레인 폴더 밖이다. §4 3항은 **RFC 로만** 진행한다.
- 상점 캡슐 회화의 권리·라이선스 [OBSERVED 2026-09-10]: 생성 원본 쪽 `provenance.json` 은 라이선스를 적는다 — 2D 계열 6파일(캡슐·컨셉·키아트·프리비즈·README·UI, 자산 45건)은 전부 `UNVERIFIED (generated; check backend ToS before commercial use)`, `assets/generated/video` 2건은 `UNVERIFIED (Higgsfield ToS)`, `assets/generated/previz` 1건은 `UNVERIFIED (assembled from gti-generated frames; …)`, `assets/generated/3d` 23건만 `original greybox` 다. **상업 사용 판정은 이 문서 밖이며 미해결이다.**
- **파생본이 라이선스를 잃는다 [OBSERVED · 신규]**: `docs/media/provenance.json` 자산 **15건 전부 `license` 키가 없다**(`None`). 원본에는 `UNVERIFIED …` 가 있는데 README 에 실리는 파생본에는 그 문구가 따라오지 않는다 — 즉 **가장 외부에 노출되는 사본이 가장 적은 권리 정보를 갖고 있다**. 재현: `python3 -c "import json;print({a.get('license') for a in json.load(open('docs/media/provenance.json'))['assets']})"` → `{None}`. `docs/media/` 는 presentation 레인 폴더가 아니므로 **고치지 않고 RFC 로 올린다**(소유 판정 필요: concept 또는 modeling).
