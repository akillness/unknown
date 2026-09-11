---
updated: 2026-09-10
cycle: 20260909-preproduction-c3
status: current
supersedes: null
owner: game-synopsis-writer
---

# 연속성 대장 — 조수기록국: 마지막 당직 (가제)

## 0. 이 문서의 지위

- 용도: `synopsis/chapter-beats.md`의 B01~B33이 **서로 모순되지 않는지**를 상태·인지·단서 세 축으로 검사한 결과다. 게이트 판정 문서가 아니다 — 판정은 `qa/gate-measurements.md#g1`이 인용하고 디렉터가 확정한다.
- 대장은 **늘어나기만 한다.** 출시 콘텐츠가 없으므로 현재는 전부 설계 단계 항목이며, 확정된 항목은 이후 사이클에서 삭제·수정하지 않는다(CLAUDE.md §9).
- 검사 방법 [OBSERVED]: 문서 대조만 했다. 빌드·플레이 표본 0건이므로 도달 가능성은 **문서상 불변식의 존재 여부**만 확인했고 실제 도달률은 측정하지 않았다.
- 상위 캐논: `worldview/worldview-bible.md`(6법·인물·결말) · `worldview/timeline.md`(진실 연표·인지 범위·§7 상한) · `worldview/glossary.md`(명사). 본 문서는 이들을 인용만 한다.
- 같은 레인의 `synopsis/t0-records.md`(cycle `…-c7`, current)는 T0 단서 6건의 **매체 원문**을 저작한다. 본 문서 §2 의 T0 3행·§5 K1 은 이제 그 원문을 인용할 수 있다. 두 문서가 어긋나면 **캠페인 데이터가 정본이고 둘 다 틀린 것**이다.

**R4 개정 (2026-09-10 · 같은 사이클 제자리 갱신, RFC-Q2 → `supersedes: null` 유지)** [OBSERVED]

| 절 | 무엇이 바뀌었나 | 근거 |
|---|---|---|
| §1 · §2 · §3 | 위치 열을 live `campaign.json` 비트 `zoneId` **파생**으로 재작성, 사건 id를 1열로, B#를 C3-F30 정의로 전건 재도출. `pump` P0→P2 행의 폐기된 `pump_motor` 소모 표현 제거 | C3-F22 · C3-F29 · C3-F30 · RFC-P3-009 |
| §4 · §4.1 | **RFC-W4** 반영 — `c4-b3`은 *효력 판정*까지, 도연의 *의도*는 `c6-b4`에서 확정. 상한표에 「도연의 의도」 행 신설 | RFC-W4 · 검증기 `K-06` |
| §5 | K 표를 **`validate-campaign.mjs --pairs` 출력에서 파생**으로 전환(저자가 고른 쌍 0개) → **A37 / C3-F12 closed**. 파생이 드러낸 신규 2건은 §5.1-bis | A37 · C3-F12 |
| §5.2 | P1 "10/10 충족"을 **8/10 기계 검증 · 2 미검증**으로 철회·정정 | §5.1 파생 결과 |
| §7 | RFC-S1(B# 포크)·RFC-P3-004(`zoneId`) 해소, RFC-S4 신설 | 본 개정 |

**R7 개정 (2026-09-10 · 같은 사이클 제자리 갱신, RFC-Q2 → `supersedes: null` 유지)** [OBSERVED]

| 절 | 무엇이 바뀌었나 | 근거 |
|---|---|---|
| §5.0 | `--pairs` 전사를 **15행 → 17행**으로 재도출(`c1-b2`·`c3-b1` 추가). 고정 sha 인용을 명령 인용으로 교체 | RFC-N6 적용 · RFC-Q1 |
| §5.1 | K2·K8 행의 "없음 — 검증기 미출력"을 **기계 출력 쌍**으로 교체 | 같은 파생 |
| §5.1-bis · §5.2 | RFC-S4 → **RFC-N6** 로 재번호하고 **부분 해소**로 갱신. P1 을 **8/10 → 9/10 기계 검증**으로 정정(K8 재확인 경로 여전히 0건) | A37-obs2 존치 |
| §7 | synopsis-로컬 RFC id 가 systems 의 `RFC-S2/S3/S4` 와 **충돌**하던 것을 `RFC-N6/N7/N8` 로 재번호. **RFC-N9·N10 신설** | 디렉터 「RFC-S4(id 충돌 정리)」 판정 |
| §0 | 신설 문서 `synopsis/t0-records.md` 를 상위 인용에 추가 | RFC-C7-001 (2) |

## 1. 구역 상태 전이 — 언제, 무엇 때문에 [TARGET]

상태 코드는 `planning/content-matrix.md` §2 소유. 전이는 **확정(commit)으로만** 일어나고, `*1`(대기) 상태는 연습 층의 시각화라 세이브에 남지 않는다.

| 구역 | 전이 | 발생 비트(`campaign` id · B#) | 원인 | 세이브 |
|---|---|---|---|---|
| `hub` | H0 → H1 | C1 진입 `c1-b1` B04 | 도구 6종 개방 | 남는다 |
| `hub` | H1 → H2 | C7 진입 `c7-b1` B28 | 청문 소집 | 남는다 |
| `hub` | H2 → H3 | E0 진입 `e0-b1` B32 | 제출 종료·이관 | 남는다 |
| `gate` | G0 → G1 | `c5-b2` B21 | 대기 명령 프리뷰 | **남지 않음** |
| `gate` | G0 → G2 | **`c5-b4` B23 확정** | 배수 우선순위 확정으로 갑문 부분 차단 | 남는다 |
| `gate` | G2 → G3 | `c7-b2` B29 (`zoneId`는 `hub`, 시연 이동으로 `gate`를 밟는다) | 그날 조건 재현 시연 후 복구 | 남는다 |
| `lowland` / `dock` | L0/D0 → L1/D1 | `c5-b2` B21 · `c5-b3` B22 | 두 경로 연습 | **남지 않음** |
| `lowland` / `dock` | → L4·D2 **또는** L2·D4 | **`c5-b4` B23 확정** | 법4 제로섬, `propertyProtection` 이진 플래그 | 남는다 |
| `lowland` / `dock` | 침수 측 → L3/D3 | `e0-b1` B32 | 에필로그 복구 | 남는다 |
| `pump` | P0 → P1 | `c2-b2` B09 | 가상 시험 구성 | **남지 않음** |
| `pump` | P0 → P2 | **`c5-b4` B23 확정** | 배수 확정 경로가 양수장 계통을 지난다. **소모가 아니라 경로 통과다** — RFC-P3-009로 계통별 부식 차감(`pump_motor` 등 6계통)은 폐기됐고 걸리는 것은 `routing` 구성안의 전역 상한 9 하나뿐이다 | 남는다 |
| `pump` | P2 → P3 | C6 진입 `c6-b1` B24 | 1호기 균열 노출 | 남는다 |

상호배타 규칙 [CARRIED `content-matrix.md` §2]: `lowland`와 `dock`의 보호 상태는 동시에 성립하지 않는다. 조합 수는 **2**이지 4가 아니다.

## 2. 비트별 진입 시 상태 전제 (33행)

`( )`는 연습 층 시각화이며 세이브에 남지 않는 상태다. `X` = `c5-b4`(B23)의 선택에 따라 갈리는 값.

**위치 열은 live `campaign.json`의 비트 `zoneId`에서 파생한다** [OBSERVED, C3-F22·C3-F29 · R4]. 이전 판이 손으로 적던 `lowland+dock` 두 값 표기는 **2행에서 사라졌다**(`c5-b1` B20 · `c5-b4` B23) — `zoneId`는 단일 값이고 제로섬 상대 구역은 *(교차)* 주석으로만 남는다. `c2-b4`(B11)·`c6-b4`(B27)의 위치는 **R7에서 `hub`로 바뀌었다** — C6-F17 판정대로 데이터가 본문("당직실에서…")을 따라갔고 스테이지 `zoneIds`에 `hub`가 추가됐다. 검증기 `Z-01`·`Z-03` PASS [OBSERVED]. 전후 대조와 그 대가는 `chapter-beats.md` §1.1.

| 사건 id | B# | 위치(`zoneId`) | `hub` | `gate` | `lowland` | `dock` | `pump` |
|---|---|---|---|---|---|---|---|
| `t0-b1` | B01 | `hub` | H0 | G0 | L0 | D0 | P0 |
| `t0-b2` | B02 | `hub` | H0 | G0 | L0 | D0 | P0 |
| `t0-b3` | B03 | `hub` | H0 | G0 | L0 | D0 | P0 |
| `c1-b1` | B04 | `gate` | H1 | G0 | L0 | D0 | P0 |
| `c1-b2` | B05 | `hub` | H1 | G0 | L0 | D0 | P0 |
| `c1-b3` | B06 | `hub` | H1 | G0 | L0 | D0 | P0 |
| `c1-b4` | B07 | `hub` | H1 | G0 | L0 | D0 | P0 |
| `c2-b1` | B08 | `pump` | H1 | G0 | L0 | D0 | P0 |
| `c2-b2` | B09 | `pump` | H1 | G0 | L0 | D0 | (P1) |
| `c2-b3` | B10 | `gate` | H1 | G0 | L0 | D0 | P0 |
| `c2-b4` | B11 | `hub` *(C6-F17 적용: `gate`→`hub`)* | H1 | G0 | L0 | D0 | P0 |
| `c3-b1` | B12 | `dock` | H1 | G0 | L0 | D0 | P0 |
| `c3-b2` | B13 | `dock` | H1 | G0 | L0 | D0 | P0 |
| `c3-b3` | B14 | `dock` | H1 | G0 | L0 | D0 | P0 |
| `c3-b4` | B15 | `dock` | H1 | G0 | L0 | D0 | P0 |
| `c4-b1` | B16 | `lowland` | H1 | G0 | L0 | D0 | P0 |
| `c4-b2` | B17 | `hub` | H1 | G0 | L0 | D0 | P0 |
| `c4-b3` | B18 | `hub` | H1 | G0 | L0 | D0 | P0 |
| `c4-b4` | B19 | `hub` | H1 | G0 | L0 | D0 | P0 |
| `c5-b1` | B20 | `lowland` *(교차 `dock`)* | H1 | G0 | L0 | D0 | P0 |
| `c5-b2` | B21 | `dock` *(교차 `lowland`)* | H1 | (G1) | (L1) | (D1) | P0 |
| `c5-b3` | B22 | `lowland` | H1 | G0 | (L1) | D0 | P0 |
| `c5-b4` | B23 | `lowland` *(교차 `dock`)* | H1 | G0 → **G2** | L0 → **L4\|L2** | D0 → **D2\|D4** | P0 → **P2** |
| `c6-b1` | B24 | `pump` | H1 | G2 | X | X | P2 → **P3** |
| `c6-b2` | B25 | `pump` | H1 | G2 | X | X | P3 |
| `c6-b3` | B26 | `pump` | H1 | G2 | X | X | P3 |
| `c6-b4` | B27 | `hub` *(C6-F17 적용: `pump`→`hub`)* | H1 | G2 | X | X | P3 |
| `c7-b1` | B28 | `hub` | **H2** | G2 | X | X | P3 |
| `c7-b2` | B29 | `hub` *(교차 `gate`)* | H2 | G2 → **G3** | X | X | P3 |
| `c7-b3` | B30 | `hub` | H2 | G3 | X | X | P3 |
| `c7-b4` | B31 | `hub` | H2 | G3 | X | X | P3 |
| `e0-b1` | B32 | `hub` | **H3** | G3 | 침수 측 → **L3** | 침수 측 → **D3** | P3 |
| `e0-b2` | B33 | `hub` | H3 | G3 | L3\|L4 | D3\|D4 | P3 |

검사 결과 [INFERENCE]
- 상태를 **읽기만 하고 바꾸지 않는 비트**: 28/33. 상태를 바꾸는 비트는 B04·B23·B24·B28·B29·B32 6건이며 그중 플레이어 선택에 좌우되는 것은 **B23 하나뿐**이다.
- 후반 비트(B24~B33)는 전부 `X`(보호 선택)에 노출되지만, 노출되는 것은 **배경·후일담·물자**이며 단서 접근은 노출되지 않는다(§5 P2 참조).
- `gate` G2 상태에서 C6의 `pump` 접근이 막히지 않는지 확인 필요 [OPEN-C1] — 배수 경로 그래프(`systems/data-schemas/zones.md` `drainEdges`)가 아직 데이터로 없어 문서 대조만으로는 판정 불가. systems 소유.

## 3. 이야기 시계 정합

| 장 | 시계 | 비트 | 근거 |
|---|---|---|---|
| T0 | 21:00 | B01~B03 (`t0-b1`~`t0-b3`) | `systems/data-schemas/beats.md` §3 `storyClock` |
| C1 | 21:30 | B04~B07 (`c1-b1`~`c1-b4`) | 동 |
| C2 | 22:10 | B08~B11 (`c2-b1`~`c2-b4`) | 동 |
| C3 | 23:00 | B12~B15 (`c3-b1`~`c3-b4`) | 동 |
| C4 | 00:00 | B16~B19 (`c4-b1`~`c4-b4`) | 동 |
| C5 | 01:00 | B20~B23 (`c5-b1`~`c5-b4`) | 동 |
| C6 | 02:00 | B24~B27 (`c6-b1`~`c6-b4`) | 동 |
| C7 | 03:30 | B28~B31 (`c7-b1`~`c7-b4`) | 동 |
| E0 | 05:00 | B32~B33 (`e0-b1`·`e0-b2`) | 동 |

규칙 [CARRIED bible §3-bis.4]: 시계는 **장 완료로만** 전진한다. 실시간 제한·전역 타이머 0개. 장 안에서 시각을 대사로 못 박지 않는다(비트 순서가 바뀌어도 대사가 깨지지 않게).

## 4. 인물 인지 범위 위반 점검 [OBSERVED 문서 대조]

기준: `worldview/timeline.md` §5. 검사 대상은 "그 인물이 그 비트에서 말할 수 있는 것"이다. **인용 키는 `campaign` id이고 B#는 C3-F30 정의(스테이지 순차)의 표시 색인이다** — 본 판에서 B#를 전건 재도출했으므로 이전 판의 번호와 다르다(`chapter-beats.md` §0).

| 인물 | 등장 비트 | 이 인물이 아는 범위 | 검사 | 결과 |
|---|---|---|---|---|
| 문재화 | B07(`c1-b4`) · B29(`c7-b2`) · B32(`e0-b1`) | 자기 지시와 통화. **도연이 왜 딸 이름을 썼는지 모른다** | B07에서 서명란의 글자·판 #0을 설명하지 않는다("서명을 받은 사람이지 읽은 사람이 아니다"). B26의 은폐 동기를 그가 말하지 않는다 | pass |
| 표성찬 | B12(`c3-b1`) · B15(`c3-b4`) · B30(`c7-b3`) · B29 · B32 | 통화의 존재. **1호기 균열을 모른다** | B15에서 서류를 넘기되 그 의미를 해설하지 않는다. 그의 증언 목록에 1호기 항이 **없다**는 사실만 단서가 된다. B25의 결론을 그가 말하지 않는다 | pass |
| 오은정 | B16(`c4-b1`) · B22(`c5-b3`) · B23 결과 · B32 | 침수 시각과 피해. **밸브 명령 계통을 모른다** | B16·B22에서 밸브·계통 판정을 하지 않는다. 소각은 그녀의 자백이 아니라 보관 일지 × 문 개폐 로그 대조로 확정된다 | pass |
| 한도연 | B19(`c4-b4`, 전화) · B26 재해석의 대상 · B32 | 서명의 진짜 의도. **밸브가 봉인 완료 접점보다 먼저 돌아간 사실을 모른다** | B19에서 순서를 말하지 않는다. 진술은 고정 문장 1건만 단서로 승격되고 확정 칸에 들어가지 않는다. B26의 재해석은 그녀의 진술이 아니라 B14·B18·B17의 대조로 성립한다 | pass |
| 한서린 | 전 비트 | **사건 인과 지식 0에서 시작.** 단, 판 #0이 자기 소유물이라는 것과 어머니의 이름은 T0부터 안다 | 플레이어가 확정하지 않은 사실을 내레이션하지 않는다. B17 이전에 **서명란에 적힌 글자**를 언급하지 않는다 | pass |

### 4.1 공개 상한 통제 [정본: RFC-P3-012 · `worldview/timeline.md` §7]

이전 판의 "한도연 최초 등장 = 4장" 규칙은 **폐기**한다. 디렉터 판정(RFC-P3-012)이 T0 인수 각서의 기재를 캐논으로 확정했다. 아래 표는 **RFC-W4**(R2 의도 공개 시점)를 함께 반영한다.

| 고유명·사실 | 게임 내 최초 등장 | 그 전까지의 표현 | 근거 |
|---|---|---|---|
| **한도연**(이름) | **B01 `t0-b1`** — 인수 각서가 12년 전 대조의 밤 당직 주임으로 기재. 서린의 어머니임도 함께 | (없음. 튜토리얼부터 공개) | RFC-P3-012 · timeline §7 B01 슬롯 · live `t0-b1.clues[0]` |
| **판 #0** | **B01 `t0-b1`** — 서린이 구 서고에서 빼내 숨긴 **자기 소유물**로 등장, 상시 슬롯에 적재 | (없음. 튜토리얼부터 공개) | RFC-P3-012 · bible §4 서린 항 · live `t0-b1.objective` |
| **서명란에 적힌 이름 '서린'** | **B17 `c4-b2`** — 소금 그늘 역산으로 복원 | "두 번째 서명자" · "각서상 당직 주임의 자리" · "자격 없는 이름" | RFC-P3-012("두 번째 대필 이름 '서린'은 4장까지 미공개") · timeline §7 B01 금지 열 |
| **무효 서명의 효력**(집행 요건 미비) | **B18 `c4-b3`** — 여기까지만. 자격 없는 이름이 둘째 칸에 들어가면 명령은 집행 요건을 갖추지 못한다 | "필압·잉크가 다른 서명 두 개" | **RFC-W4**(세션 P 연표 §3 채택) · timeline §7 · 검증기 `K-06`(`c4-b3`에 의도 문장 **부재** 강제) |
| **도연의 의도**(방패가 아니라 잠금장치) | **B27 `c6-b4`** — live `c6-b4.inference` 원문 "도연이 고른 무효 서명도 같은 성격이다 — 그것은 방패가 아니라 잠금장치다". `c6-b3`(B26)은 R2 **재해석 착수**까지이며 확정 문구는 `c6-b4`가 소유한다 | "자격 없는 이름"·"딸을 방패로 썼다"(오답 후보) | **RFC-W4** · 검증기 `K-06`(`c6-b4`에 문장 **존재** 강제) · live `c4-b3.consequence`("도연이 왜 무효를 골랐는지는 아직 미확정") |
| **1호기 고장 은폐(동기)** | B26 `c6-b3` | "임시운전 연장 기록의 단절" · "센서 설치일이 사건 이후" | timeline §7 |
| **밸브(H-1:24) → 봉인 완료 접점(H-1:04) 선후** | B26 `c6-b3` | B14에서 총 오차폭 8분만 확정하고 6분 쌍은 unknown 이월 | RFC-P3-013 · timeline §2 |

### 4.2 금지 대상의 범위 [RFC-P3-012 decision]

이 절의 금지는 **저자 확정 서술**(작가가 참으로 단언하는 문장, 확정 항목, 캐논 서술)만을 대상으로 한다. 다음은 금지가 아니다.

| 형태 | 예 | 판정 |
|---|---|---|
| 사건판의 **오답 후보 / 오해 유지** 문구 | live `c1-b4.consequence` "…**누군가 도연을 도왔다**"가 오답 후보로 오른다 | **허용.** 저자가 참이라고 말하는 것이 아니라 플레이어가 세운 가설이며, B17·B26에서 반증된다. 이전 판이 이 문자열을 금지한 것은 과잉이었다(RFC-P3-012) |
| NPC가 자기 인지 범위 안에서 하는 진술 | 재화 "서명란에 있는 게 그 이름인지는 제가 모릅니다" | 허용 |
| 인용 표기가 붙은 **폐기된 캐논** | 폐기 시각 **H-1:20**(밸브)·**H+0:10**(침수)을 "폐기됐다"고 명시해 인용 | 허용(역사 인용). H-1:40은 폐기가 아니라 **현행 캐논**(도연이 두 번째 서명란에 이름을 적은 시각, timeline §2)이다 |
| 확정되지 않은 인과를 저자가 단언 | "도연은 딸을 방패로 썼다" | **금지**(오답 후보로만 등장 가능) |
| 기록되지 않은 인간 행위를 확정 근거로 사용 | "그것을 누가 확인했다" | **금지**(법1, `synopsis.md` §7 금지 7) |

전지적 화자 검사 [OBSERVED]: 33비트 중 **결론을 NPC가 말해주는 비트 0건**. 모든 확정은 매체 2종 대조로만 성립한다(표 B 전 행).

## 5. 필수 단서 K1~K10 — 결말 3갈래 도달 요건과 보존 경로

정의 [CARRIED bible §3-bis.3]: **필수 단서** = 결말 도달에 확정돼야 하는 증거. **독립 쌍** = `systems/interaction-rules.md` §3의 판정을 통과하는 자료 두 점 — **`sourceType`이 서로 다르고 `copiedFrom`을 재귀 해석한 루트 `originId`도 서로 다를 때만** 독립이다(AND).

### 5.0 독립쌍의 출처 — **검증기 파생** [OBSERVED, 2026-09-10 · R4] — A37 / C3-F12 대응

디렉터 C3 종료 판정(**A37 / C3-F12**)은 "§5 K 표는 손으로 짝을 고르지 않고 `validate-campaign.mjs`의 C-07 출력(=`--pairs`)에서 파생한다"였다. 본 판이 그 전환을 실행한다.

재도출 명령 [OBSERVED, 2026-09-10 R7 재실행]
```
$ node _workspace/current/planning/validate-campaign.mjs --pairs ; echo "exit=$?"
exit=0
$ node _workspace/current/planning/validate-campaign.mjs --pairs | node -e '…'   # 아래 17행으로 평탄화
```
**R4 → R7 차분** [OBSERVED]: planner 가 RFC-N6 판정을 적용해 `c1-b2`·`c3-b1` 의 `proofRequired` 를 `true` 로 올렸다 → `proofRequiredBeats` **15 → 17**, 전사 행 **15 → 17**. **단서를 추가하지 않고도** 두 비트가 규칙을 통과했다(§5.1-bis). 같은 편집에서 C6-F17 이 적용돼 `c2-b4` 의 구역 열이 `gate` → `hub` 로 바뀌었다.
판정 규칙(검증기 `rule` 필드 원문): *"C-07과 같은 규칙: 루트 originId 상이 AND sourceType 상이. 사본은 루트를 물려받으므로 원본×사본은 쌍이 되지 못한다"* — 즉 **매체가 다르다는 것만으로는 독립이 아니다**.

#### --pairs 출력 17행 (원문 전사)

| 비트 | B# | 구역 | 단서 수 | 검증기가 찍은 독립쌍 (clue id · sourceType · 루트 originId) |
|---|---|---|---:|---|
| `t0-b3` | B03 | `hub` | 2 | `t0-b3-c1`(plate, 루트 `plate-standard-hub`) × `t0-b3-c2`(ledger, 루트 `tide-ledger-bureau`) |
| **`c1-b2`** | B05 | `hub` | 2 | `c1-b2-c1`(log, 루트 `signature-annex`) × `c1-b2-c2`(plate, 루트 `plate-zero`) — **R7 신규** |
| `c1-b3` | B06 | `hub` | 3 | `c1-b3-c1`(log, 루트 `signature-annex`) × `c1-b3-c2`(plate, 루트 `brine-log-gate3`) |
| `c2-b3` | B10 | `gate` | 3 | `c2-b3-c1`(plate, 루트 `plate-standard-hub`) × `c2-b3-c2`(ledger, 루트 `tide-ledger-bureau`) |
| `c2-b4` | B11 | `hub` | 2 | `c2-b4-c1`(log, 루트 `duty-roster`) × `c2-b4-c2`(plate, 루트 `brine-log-gate3`) |
| **`c3-b1`** | B12 | `dock` | 2 | `c3-b1-c1`(ledger, 루트 `tide-ledger-seongchan`) × `c3-b1-c2`(log, 루트 `brine-log-dock`) — **R7 신규** |
| `c3-b2` | B13 | `dock` | 2 | `c3-b2-c1`(ledger, 루트 `tide-ledger-seongchan`) × `c3-b2-c2`(plate, 루트 `plate-standard-hub`) |
| `c3-b3` | B14 | `dock` | 2 | `c3-b3-c1`(plate, 루트 `brine-log-gate3`) × `c3-b3-c2`(ledger, 루트 `tide-ledger-bureau`) |
| `c4-b1` | B16 | `lowland` | 2 | `c4-b1-c1`(ledger, 루트 `tide-ledger-bureau`) × `c4-b1-c2`(log, 루트 `council-custody-log`) |
| `c4-b2` | B17 | `hub` | 2 | `c4-b2-c1`(log, 루트 `signature-annex`) × `c4-b2-c2`(plate, 루트 `plate-zero`) |
| `c4-b3` | B18 | `hub` | 2 | `c4-b3-c1`(log, 루트 `regulation-dual-seal`) × `c4-b3-c2`(ledger, 루트 `family-allowance-ledger`) |
| `c5-b1` | B20 | `lowland` | 3 | `c5-b1-c1`(plate, 루트 `brine-log-pump1`) × `c5-b1-c2`(ledger, 루트 `forecast-table`) |
| `c5-b3` | B22 | `lowland` | 3 | `c5-b3-c1`(log, 루트 `council-custody-log`) × `c5-b3-c2`(plate, 루트 `brine-log-hub`) |
| `c6-b1` | B24 | `pump` | 2 | `c6-b1-c1`(plate, 루트 `brine-log-pump1`) × `c6-b1-c2`(ledger, 루트 `pump-maintenance-ledger`) |
| `c6-b2` | B25 | `pump` | 2 | `c6-b2-c1`(plate, 루트 `brine-log-pump1`) × `c6-b2-c2`(ledger, 루트 `sensor-install-ledger`) |
| `c6-b3` | B26 | `pump` | 4 | `c6-b3-c1`(plate, 루트 `brine-log-gate3`) × `c6-b3-c3`(ledger, 루트 `pump-maintenance-ledger`) |
| `c7-b2` | B29 | `hub` | 2 | `c7-b2-c1`(ledger, 루트 `tide-ledger-bureau`) × `c7-b2-c2`(plate, 루트 `brine-log-gate3`) |

`beatsWithoutPair`: **[]** (**17/17** 전건 쌍 보유) · `proofRequiredBeats`: **17**. sha·바이트는 **인용하지 않는다** — 위 명령의 출력이 보유하며, 손으로 옮겨 적은 숫자는 R4→R7 한 회차 만에 스테일이 됐다(RFC-Q1 · C7-F38 유형).

**K2·K8의 쌍은 R7에서 새로 생긴 것이 아니라 이제서야 *출력된* 것이다** [OBSERVED]. 두 비트의 단서 구성은 R4 때도 규칙을 통과할 수 있었고, `proofRequired: false` 라 검증기가 **검사조차 하지 않았을** 뿐이다 — 플래그를 켜자 단서를 하나도 더하지 않고 C-07 이 PASS 했다.

**검증기는 비트당 쌍을 하나만 찍는다** [OBSERVED]. 단서가 3~4개인 비트(`c1-b3` `c2-b3` `c5-b1` `c5-b3` `c6-b3`)에도 출력은 **첫 적격 쌍 1개**다. 따라서 아래 K 표의 "검증기 쌍" 열은 **손으로 고른 최선의 쌍이 아니라 기계가 찍은 그 쌍**이며, 이전 판처럼 두 번째 쌍을 저자가 구성하지 않는다. 같은 비트에서 확정되는 K가 둘이면(K6·K10 = `c6-b3`) **같은 쌍을 인용**한다 — 이는 중복이 아니라 파생의 결과다.

사본 계보 [OBSERVED, 검증기 `rootOriginId` 필드]: 카탈로그 31종 중 사본은 **2건**뿐이다 — `council-copybook`(ledger, `copiedFrom: tide-ledger-bureau`)과 `bureau-copy-index`(ledger, `copiedFrom: council-copybook`). 둘 다 루트는 `tide-ledger-bureau`다. `c4-b1`의 쌍이 성립하는 것은 상대가 `council-custody-log`(log, 자기 루트)이기 때문이지 사본이라서가 아니다.

**이전 판의 오류가 무엇이었나** [OBSERVED]: 옛 §5는 K1·K3·K6·K9의 "경로 2"를 "원본 × 접수부 확정 사본" 또는 "원본 × 주민회 사본"으로 세웠다. 루트 상속 규칙상 그 조합은 거부된다. 본 판은 그 열 자체를 없애고 **검증기가 찍은 쌍만** 인용한다 — 저자가 쌍을 만들 자리가 사라졌다는 점이 A37의 요구였다.

**불파괴(P2)의 근거도 사본이 아니다**: `systems/interaction-rules.md` §0.3 "필수 단서는 첫 판독 시 **자동 보존**(`autoKeptClueIds`)되며 어떤 행동으로도 사라지지 않는다" · `systems/data-schemas/save.md` 동일. P2는 접수부·주민회 사본이 아니라 **첫 판독 자동 사본**이 보장한다.

### 5.1 K 표 — **`--pairs` 출력에서 파생** (저자 선택 없음)

`검증기 쌍` 열은 §5.0 표에서 해당 비트 행을 **그대로 옮긴 것**이다. `proofRequired` 열이 `false`면 검증기가 그 비트에 쌍을 찍지 않으므로 **인용할 쌍이 없다**(§5.1-bis).

| id | 필수 단서 | 확정 비트 | `proofRequired` | 검증기 쌍 (§5.0 원문) | 재확인 비트의 검증기 쌍 | 불파괴 근거 | 제출 시 인용 관점 |
|---|---|---|---|---|---|---|---|
| K1 | 결손 4시간의 존재 | `t0-b3` B03 | **true** | `t0-b3-c1`(plate, 루트 `plate-standard-hub`) × `t0-b3-c2`(ledger, 루트 `tide-ledger-bureau`) | `c2-b3`: `c2-b3-c1`(plate, `plate-standard-hub`) × `c2-b3-c2`(ledger, `tide-ledger-bureau`) | 첫 판독 자동 사본(`t0-b3` 결과) | A·B·C |
| K2 | 판 #0 곡선의 **명령 대기 구간** | `c1-b2` B05 | **true** (R7) | `c1-b2-c1`(log, 루트 `signature-annex`) × `c1-b2-c2`(plate, 루트 `plate-zero`) | `c4-b2`: `c4-b2-c1`(log, `signature-annex`) × `c4-b2-c2`(plate, `plate-zero`) | 판 #0은 서린 소유·잠금 보관, 원본 무소모(`c4-b2.recovery`) | A·B |
| K3 | 두 서명의 필압·잉크 차이 | `c1-b3` B06 | **true** | `c1-b3-c1`(log, 루트 `signature-annex`) × `c1-b3-c2`(plate, 루트 `brine-log-gate3`) | `c4-b2`(같은 서명지 계보를 그늘 조건으로 재확인) | 분리 후 사본, 원본 손상 없음(`c1-b2.recovery`) | A |
| K4 | 부식 패턴 ≠ 정전 (R1) | `c2-b3` B10 | **true** | `c2-b3-c1`(plate, 루트 `plate-standard-hub`) × `c2-b3-c2`(ledger, 루트 `tide-ledger-bureau`) | `c2-b4`: `c2-b4-c1`(log, `duty-roster`) × `c2-b4-c2`(plate, `brine-log-gate3`) | 서사 부식은 읽기 전용(P4) | A·B |
| K5 | 정합 잔차 ≤4분 · 총 오차폭 8분 · unknown 규칙 | `c3-b2` B13 → `c3-b3` B14 | **true** / **true** | `c3-b2-c1`(ledger, 루트 `tide-ledger-seongchan`) × `c3-b2-c2`(plate, 루트 `plate-standard-hub`) | `c3-b3`: `c3-b3-c1`(plate, `brine-log-gate3`) × `c3-b3-c2`(ledger, `tide-ledger-bureau`) | `c3-b1`의 첫 판독 자동 사본이 증거함에 남는다 | C |
| K6 | **밸브 개폐(H-1:24)가 봉인 완료 접점(H-1:04)보다 20분 앞섰다** | `c6-b3` B26 | **true** | `c6-b3-c1`(plate, 루트 `brine-log-gate3`) × `c6-b3-c3`(ledger, 루트 `pump-maintenance-ledger`) | `c7-b2`: `c7-b2-c1`(ledger, `tide-ledger-bureau`) × `c7-b2-c2`(plate, `brine-log-gate3`) | 두 각인 모두 기계 각인, 자료 소모 없음(`c6-b3.recovery`) | A |
| K7 | 두 번째 서명란의 이름과 필자 (R2) | `c4-b2` B17 | **true** | `c4-b2-c1`(log, 루트 `signature-annex`) × `c4-b2-c2`(plate, 루트 `plate-zero`) | `c4-b3`(효력 판정): `c4-b3-c1`(log, `regulation-dual-seal`) × `c4-b3-c2`(ledger, `family-allowance-ledger`) | 원본 잠금 보관, 사본 무제한 재생 | A |
| K8 | 통화 개시 시각·계통 (내용은 남지 않는다) | `c3-b1` B12 | **true** (R7) | `c3-b1-c1`(ledger, 루트 `tide-ledger-seongchan`) × `c3-b1-c2`(log, 루트 `brine-log-dock`) | **없음 — `proofRequired` 17건 중 회선 기록 계보(`brine-log-dock`·`watchlog-bureau`)를 쓰는 *재확인* 비트가 여전히 0건** [OBSERVED R7] | 첫 판독 자동 사본이 증거함에 보존 | B |
| K9 | 증설 승인 ↔ 임시운전·센서 설치 서류 불일치 | `c6-b2` B25 | **true** | `c6-b2-c1`(plate, 루트 `brine-log-pump1`) × `c6-b2-c2`(ledger, 루트 `sensor-install-ledger`) | `c6-b1`: `c6-b1-c1`(plate, `brine-log-pump1`) × `c6-b1-c2`(ledger, `pump-maintenance-ledger`) | 서류 원본은 타 기관 보관, 플레이어 행동으로 소실되지 않는다 | B |
| K10 | 고장 은폐가 실제 동기 (R3) | `c6-b3` B26 | **true** | `c6-b3-c1`(plate, 루트 `brine-log-gate3`) × `c6-b3-c3`(ledger, 루트 `pump-maintenance-ledger`) — **K6과 같은 쌍**(검증기는 비트당 1쌍) | `c6-b2`: `c6-b2-c1`(plate, `brine-log-pump1`) × `c6-b2-c2`(ledger, `sensor-install-ledger`) | 위와 같음 | B |

**거부되는 조합(원장으로 남긴다)** [OBSERVED, 검증기 규칙 파생]: `brine-log-gate3` × `plate-zero`(루트는 다르나 **둘 다 plate**) · `tide-ledger-bureau` × `council-copybook`(루트 동일) · `council-copybook` × `bureau-copy-index`(루트 동일). live `c6-b3.completion`이 순서 확정을 "제3수문 염선 로그와 판 #0 **두 계통**"으로 적는 것은 첫째 조합이며 규칙상 성립하지 않는다 → `chapter-beats.md` §4-4로 planner·systems 인계. **검증기가 `c6-b3`에 찍은 쌍은 그 조합이 아니라 `c6-b3-c1` × `c6-b3-c3`이다** — 데이터는 옳고 `completion` 문장이 틀렸다.

### 5.1-bis 파생이 드러낸 것 — 신규 관측 2건 [OBSERVED, R4]

| # | 관측 | 왜 문제인가 | 소유 |
|---|---|---|---|
| ~~RFC-S4~~ → **RFC-N6** | **해소(R7).** 디렉터 판정대로 planner 가 `c1-b2`·`c3-b1` 의 `proofRequired` 를 `true` 로 올렸다 | **단서 추가 없이 C-07 PASS** — `proofRequiredBeats 17` · `beatsWithoutPair []` [OBSERVED `--pairs`]. 예측과 실측이 일치했다(§7 RFC-N6 의 사전 시뮬레이션). id 는 systems 의 `RFC-S4` 와 충돌해 **RFC-N6 으로 재번호**됐다(디렉터 「RFC-S4(id 충돌 정리)」) | closed |
| A37-obs2 | **여전히 열려 있다.** K8의 *재확인* 경로가 0건 — `proofRequired` **17**비트 중 회선 기록 계보(`brine-log-dock` · `watchlog-bureau`)를 쓰는 비트는 `c3-b1` 하나뿐이고 그것이 곧 확정 비트다 [OBSERVED R7 재측정] | K8은 결말 B의 인용 근거인데 **두 번째 기계 검증 지점이 캠페인 전체에 없다**. P1(이중 경로)의 문서상 근거가 K8에 대해서만 약하다 | planner · synopsis · systems → §7 RFC-N6 (b) |

**A37 / C3-F12 판정** [OBSERVED]: 요구는 "K 표를 검증기 출력에서 파생하고 손으로 고르지 않는 것"이었다. **그 전환은 완료됐다 — 본 절의 모든 쌍은 `--pairs` 17행의 전사이며 저자가 구성한 쌍은 0개다. A37 closed.** 다만 파생은 두 개의 새 사실을 드러냈고(위 표) 그것은 **A37의 미해소가 아니라 파생이 산출한 신규 결함**이다 — 손으로 고르던 이전 판에서는 보이지 않던 종류의 결함이며, 이 두 건은 planner 소유로 연다.

### 5.2 불변식 점검 [INFERENCE]

| 불변식 | 검사 | 결과 |
|---|---|---|
| P1 이중 경로 | K1~K10 각각 §3을 통과하는 독립 경로 2개 이상 | **기계 검증 9/10 · 미검증 1** [OBSERVED R7, §5.1에서 파생]. R7 에서 K2 가 8/10 → 9/10 으로 올라갔다 — 확정 비트 `c1-b2` 가 `proofRequired: true` 가 되며 쌍이 출력됐고, 재확인 경로 `c4-b2` 는 이전부터 기계 출력이었다. **남은 1건은 K8** — 확정 경로(`c3-b1`)는 이제 기계 검증이지만 **재확인 경로가 캠페인에 없다**(A37-obs2). 저자가 쌍을 구성해 세웠던 "10/10"은 여전히 **철회 상태**이며, 10/10 은 K8 재확인 비트가 생길 때만 회복된다 |
| P2 불파괴 | 각 필수 단서가 플레이어 행동으로 파괴되지 않는가 | **10/10 충족 [INFERENCE, 문서 대조]**. 근거는 접수부·주민회 사본이 아니라 **첫 판독 자동 사본**(`autoKeptClueIds`, 축소 불가)이며 `interaction-rules.md` §0.3·`save.md`가 소유한다. 이 축은 검증기가 보지 않는다 — **쌍의 독립성(P1)과 달리 불파괴는 기계 검사가 없다** |
| P3 확정 사본 | 확정 순간 접수부 등재 | 충족. **단, B01~B03의 판정은 첫 등재 이전이라 등재가 없다** → §7 RFC-N5 |
| P4 자기오염 금지 | 플레이어 부식예산 ≠ R1 증거 부식 | 충족. RFC-P3-009로 부식예산은 `routing` 구성 상한 하나뿐이므로 계통 공유 자체가 없다 |
| P5 손실의 종류 | B23·B22의 손실이 물자·후일담·신뢰·편의에 한정 | 충족. B22의 소각분은 복원되지 않으나 K1~K10 어디에도 소각분이 단독 경로로 쓰이지 않는다 |

### 5.3 "필요한 결말"에 대한 정정 [OBSERVED] — C3-F17 대응

live `campaign.json`의 `prerequisites`를 전수 집계하면 **t0-b1 → t0-b2 → … → e0-b2 단일 사슬**이다(교차 간선 `c2-b3←t0-b3`, `c4-b2←c1-b2`, `c4-b3←c1-b3`, `c5-b3←c4-b1`, `c6-b2←t0-b2`, `c6-b3←c3-b3·c4-b3·c1-b3`, `c7-b2←c5-b2`, `c7-b3←c6-b3`은 모두 앞선 비트를 가리켜 사슬을 벗어나지 않는다). 따라서:

- **도달 요건**: K1~K10은 **세 결말 전부에 공통**이다. 결말은 `c7-b4`의 관점 선택으로만 갈리며, 특정 K를 확정하지 못해 결말이 닫히는 경로는 데이터에 없다.
- **인용 요건**: 위 표의 마지막 열과 `synopsis.md` §5의 A/B/C 목록은 **제출 문서가 어느 근거를 인용하는가**를 뜻하며 도달 게이트가 아니다.

재측정 [OBSERVED, 2026-09-10 · R4]: `prerequisites` 전 간선을 비트 순서 색인으로 검사했다 — **전진 간선 0건 · 미상 참조 0건**, 교차 간선은 위 10건 그대로이며 전부 앞선 비트를 가리킨다. 명령:
```
node -e "const c=require('./_workspace/current/planning/campaign.json');const o=[];for(const s of c.stages)for(const b of s.beats)o.push(b.id);\
const i=Object.fromEntries(o.map((x,k)=>[x,k]));for(const s of c.stages)for(const b of s.beats)for(const p of (b.prerequisites??[]))\
{if(i[p]===undefined)console.log('UNKNOWN',b.id,p);if(i[p]>=i[b.id])console.log('FORWARD',b.id,p);}"
```

이전 판은 K2의 "필요한 결말: A·B"를 적고 `synopsis.md` §5의 A·B 목록에는 K2를 넣지 않아 어긋났다(C3-F17). 본 판은 **K2를 A·B 인용 목록에 추가**하고(`synopsis.md` §5), 두 표의 의미 차이를 위와 같이 명시한다. K2가 A·B에 인용되는 이유: 판 #0의 명령 대기 곡선은 A의 K7(그늘 조건 상한)과 B의 K6·K10(20분 구간을 덮는 독립 곡선) 양쪽에 입력으로 들어간다 [OBSERVED `c4-b2.clues[1]` · `c6-b3.clues[1]`].

결말 자격 검사 [INFERENCE]: **B23의 선택(`propertyProtection`)은 K1~K10 어디에도 나타나지 않는다** → 5장의 제로섬은 결말 자격을 잠그지 않는다(bible §6 주석 E3 충족, live `c5-b4.consequence`가 같은 문장을 담는다).

## 6. 상한(G1) 자기 점검과 4분 분해능 검사

### 6.1 timeline §7 상한 초과 점검 [2026-09-10 재대조]

| 검사 | 방법 | 결과 |
|---|---|---|
| 각 비트의 확정 사실이 해당 슬롯의 "공개 상한" 안에 있는가 | B01~B33 행별 대조(상호 참조는 `campaign` id로) | 33/33 위반 0 [INFERENCE] |
| "이 비트에서 말할 수 없는 것"을 말하는 비트가 있는가 | 금지 열 대조 | 0건. **B01은 한도연·판 #0을 공개하되(RFC-P3-012) 서명란의 글자와 12년 전 인과는 말하지 않는다.** B23는 결말 잠금 암시를, B33은 DLC 미완 인상을 만들지 않는다 |
| 오해 유지 열이 채워진 비트에서 그 오해를 부정하는가 | B03·B07·B11·B15·B18·B22 | 0건 |
| 반전 씨앗·회수가 33슬롯 안에 있는가 | R1 `t0-b2` B02 → `c2-b3` B10 · R2 씨앗 `c1-b2` B05·`c1-b4` B07 → **사실 회수** `c4-b2` B17 → **효력 판정** `c4-b3` B18 → **재해석 착수** `c6-b3` B26 → **의도 확정** `c6-b4` B27 (RFC-W4) · R3 씨앗 `c2-b4` B11·`c3-b4` B15 → 회수 `c6-b3` B26 · 순서 앵커 씨앗 `c1-b3` B06 → 규칙 `c3-b3` B14 → 확정 `c6-b3` B26 | 일치 [OBSERVED timeline §4·§7 · 검증기 `K-06`] |
| 고유명이 `glossary.md`에 있는가 | 표 A·B의 고유명 전수 대조 | **미수록 0건** [OBSERVED 2026-09-10 재측정]. 이전 판이 미수록으로 보고한 3건은 worldview가 승격했다 — `인수 각서`(glossary §7 `handover-brief`) · `당직 자격 명부`(§7 규정 부속) · `근무표`(§7 `duty-roster`). `봉인 완료 접점`·`자동 사본`·`상시 슬롯`·`가설판`·`증거함`·`사건판`도 §2~§4에 수록됐다 |
| 서류 이름을 새로 만들었는가 | 표 B 단서 열 | 0건. 표 B의 서류 이름은 전부 live `campaign.json`의 `originId` 31종과 glossary §7 표에서 온다 |

### 6.2 4분 분해능 의존 검사 [`planning/feature-specs/verb-02-plate-read.md` D6가 synopsis에 배정한 항목]

법3 규칙: 정합 후 잔차 ±4분 → 두 관측소를 걸친 총 오차폭 8분. **간격 > 8분일 때만 순서를 확정한다.**

| 비트 | 확정이 의존하는 시간 간격 | 오차 조건 | 판정 |
|---|---|---|---|
| B03 `t0-b3` | 결손 구간 4시간(H-1:00 붕괴 시작 ~ H+3:00 대장 연속 끝) | 240분 ≫ 8분 | 확정 가능 |
| B10 `c2-b3` | 부식 무늬 층 vs 정전 구간 | 시간 간격이 아니라 무늬 층 판정 | 해당 없음 |
| B11 `c2-b4` | **결손 4분** | 4분 = 분해능 1칸 → 단일 표본 확정 불가 | **반복률로만 판정.** 단일 표본 확정 금지 · **[BLOCKED:lore OPEN-3]** |
| B13 `c3-b2` | 공통 피크 간격 | 정합 성립 요건 자체 | 확정 가능(잔차 ≤4분) |
| B14 `c3-b3` | 20분 쌍 / 6분 쌍 | 20분 > 8분 → 규칙상 확정 가능, 6분 < 8분 → unknown | **규칙과 반증 시험까지만.** 그날 두 각인의 사건 확정은 B26으로 이월(대체가설 미배제) |
| B16 `c4-b1` | 결번 2장의 앞뒤 페이지 시각 사이 구간 | 구간 좁히기이며 선후 단정이 아니다 | 확정 가능(H-1:40 부근 좌표화) |
| B24 `c6-b1` | 연장 기입 마지막 날짜 ↔ 공식 폐쇄일 (일 단위) | 일 단위 ≫ 8분 | 확정 가능 |
| B26 `c6-b3` | **밸브 H-1:24 ↔ 봉인 완료 접점 H-1:04 = 20분** | 20분 > 8분, 대체가설(정비 시험)은 정비 대장의 기록 부재로 배제 | 확정 가능 [OBSERVED RFC-P3-013 · timeline §2·§8] |

결과 [INFERENCE]: 4분 미만 간격에 의존하는 확정은 **0건**. 유일한 경계 항목 B11은 확정이 아니라 반복률 판정으로 설계했다.

## 7. 미해소 항목과 RFC [2026-09-10 갱신]

해소된 항목 [OBSERVED]

| id | 처리 | 근거 |
|---|---|---|
| OPEN-2 판 #0의 매체 구성·호칭 | **해소.** 정의는 "번호만 적힌 미봉인 염판 1매"이며 저작 보류를 푼다 | `worldview/glossary.md` §2 판 #0 · `worldview-bible.md`("판 #0은 '번호만 적힌 미봉인 염판' — audit A11(OPEN-2)이 이로써 해소된다") |
| RFC-N2 1장 오해 문구 | **해소.** 오답 후보 문구 "누군가 도연을 도왔다"는 저자 서술이 아니므로 허용된다 | RFC-P3-012 · 본 문서 §4.2 |
| RFC-N3 `seal`·`routing` 재집계와 C5 지급 판 2→3 | **철회.** 전제였던 B16 매체 변경이 live 값으로 원복됐고, RFC-P3-009로 도구 확정에는 자원 소모가 없다 | `chapter-beats.md` §3 |
| 법 문구 불일치 | **해소.** 6법 호명 문구의 정본은 아카이브 c3 `worldview-bible.md` §3 표 하나뿐 | RFC-P3-014 |
| 사건 시각 3종 | **해소.** 밸브 H-1:24 → 봉인 완료 접점 H-1:04, 저지대 침수 H+0:12 | RFC-P3-013 |
| 고유명 미수록 3건 | **해소.** worldview가 승격 | 본 문서 §6.1 |

남은 항목

| id | 항목 | 상태 | 소유 |
|---|---|---|---|
| OPEN-3 | 결손 4분의 반복률 정의 | `c2-b4`(B11)의 판정 수치 미정 | systems · planner |
| OPEN-1 | 제1양수장 폐쇄일 | `c6-b1`(B24)·`c6-b2`(B25)는 중립 표현("연장 기입이 T-19 이후 끊긴다", "공식 폐쇄일은 사건 이후")만 사용해 저작이 대기하지 않는다 | director |
| OPEN-C1 | `gate` G2에서 `pump` 접근성 | `drainEdges` 데이터 부재로 문서 판정 불가 | systems |
| RFC-N5 | 자원 차감 시점 분쟁 | **범위 축소.** RFC-P3-009로 도구 확정에 소모가 없어졌으므로 "봉인 시점 vs 확정 시점" 분쟁 자체가 소멸했다. 남은 것은 P3(확정 사본 등재)가 B01~B03에 적용되지 않는 문제뿐 | worldview |
| ~~RFC-S1~~ | ~~B# 부여 체계 포크~~ → **해소(2026-09-10).** C3-F30이 정의를 하나로 못 박았고(스테이지 순차, 소유는 `timeline.md` §7) 본 문서·`chapter-beats.md`·`content-matrix.md`가 전건 재도출됐다. 상호 참조 키는 `사건 id`다 | closed | — |
| **RFC-N7** (구 RFC-S2 — id 충돌 재번호) | `c6-b3.completion`의 "제3수문 염선 로그와 판 #0 두 계통"이 §3 AND 규칙(종류 상이)에서 거부된다 | **범위 축소.** 검증기가 `c6-b3`에 찍은 쌍은 `c6-b3-c1`(plate) × `c6-b3-c3`(ledger)이므로 **데이터의 쌍은 적법**하다. 남은 것은 `completion` **문장**이 그 쌍이 아니라 염판 2점을 말한다는 것뿐 | planner(문장) |
| **RFC-N8** (구 RFC-S3 — id 충돌 재번호) | `balance/balance-sheet.md` §4가 RFC-P3-009로 폐기된 6계통 모델을 유지 | balance 소유 | balance |
| ~~RFC-P3-004~~ | ~~비트별 `zoneId` 신설~~ → **해소(2026-09-10).** C3-F22 판정으로 planner가 33/33 추가, 검증기 `Z-01`·`Z-02` PASS. 비트-구역의 단일 출처는 JSON이다 | closed | — |
| **RFC-N6** (구 RFC-S4 — 디렉터 「RFC-S4(id 충돌 정리)」 판정으로 재번호) | (a) K2·K8 확정 비트의 `proofRequired` → **적용 완료 · C-07 PASS**. (b) **K8 재확인 경로 0건은 미해소** | (a) planner 가 `true` 로 올렸고 **단서를 하나도 더하지 않고** 통과했다(17/17 쌍 보유). (b) 회선 기록 계보를 쓰는 두 번째 `proofRequired` 비트가 필요하다 → **§7.1** | (a) closed · (b) planner(플래그) · synopsis(후보 제안) |
| **A37-obs2** | K8의 **재확인** 기계 검증 지점이 캠페인 전체에 없다(확정 지점은 R7에 생겼다) | 결말 B의 인용 근거인데 P1의 문서상 근거가 K8에서만 약하다 → §7.1 (b) | planner · synopsis |
| **RFC-N9 (신규 · R7)** | `synopsis/t0-records.md` 의 [TARGET] 수치 6묶음이 캐논 등재 없이 저작됐다 — ① 염판 링 창 `H-6:00~H+5:56`(창의 기준점이 캐논에 없다) ② 조위·압력·염도·수위 4식과 계수 ③ 평평 3구간 경계 ④ 이관 목록 3줄의 품목·수량 ⑤ 손글씨 누락 표시 3건 ⑥ 각서 산출물 조항 `hb-l4` 문안. **더불어**: `timeline.md` §2 는 H+0:12 급등이 "주민회 사본에만 온전"이라 적고 `t0-b3-c2` 는 기록국 조위대장이 H+3:00까지 끊김 없다고 적는다 — **끊기지 않는 열과 온전하지 않은 기재가 같은 대장의 어느 열인지** 미정 | 값 자체는 캐논 문장을 표현한 것이고 새 인물·사건·사건시각은 0건이다(`t0-records.md` §9). 그러나 **표현값의 캐논 등재는 worldview 소유**이며 승인 전까지 systems 는 값을 쓰되 캐논으로 인용하지 않는다 | worldview(판정) · synopsis(저작) |
| **RFC-N10 (신규 · R7)** | live `t0-b1.objective`(C6-F1 적용분)가 **T0 시점에 청문 절차와 그 마감이 인수 각서에 적혀 있다**고 전제한다. `timeline.md` §1·§3 에 그 전제를 적은 행이 없고 청문은 이전까지 `c3-b4` 이후에만 나왔다 [OBSERVED 전수] | 디렉터 판정(C6-F1)이 요구한 문장이므로 **적용은 옳다**. 남은 것은 캐논 등재 한 줄이며, 등재되지 않으면 다음 회차 QA가 "상한표에 없는 사실"로 다시 연다 | worldview(등재) · director |

### 7.1 RFC-N6 — 두 번째 매체 단서 후보 (카탈로그 31종 안, 루트 `originId` 상이) [synopsis 제안 · planner 적용]

**(a) 지금은 후보가 필요하지 않다** [OBSERVED, 2026-09-10 · 적용 전 예측 + 적용 후 실측]

디렉터 판정은 *"검증기 C-07 이 FAIL 하면 synopsis/planner 가 두 번째 매체 단서를 카탈로그 안에서 추가한다"* 는 **조건부**였다. synopsis 가 적용 **전에** 시뮬레이션으로 예측하고 적용 **후에** 실측으로 확인한 결과, 두 비트 모두 **단서 추가 없이** 통과한다.

```
# 적용 전 예측 — campaign.json 을 메모리에서만 바꿔 C-07 규칙(종류 상이 AND 루트 상이)을 실행
c1-b2 (당시 proofRequired=false) → 적격쌍 ["c1-b2-c1","c1-b2-c2"]   # log signature-annex × plate plate-zero
c3-b1 (당시 proofRequired=false) → 적격쌍 ["c3-b1-c1","c3-b1-c2"]   # ledger tide-ledger-seongchan × log brine-log-dock

# 적용 후 실측
$ node _workspace/current/planning/validate-campaign.mjs --pairs ; echo "exit=$?"
exit=0     # proofRequiredBeats 17 · beatsWithoutPair [] · C-07 PASS
```
→ **RFC-N6 (a) closed.** 아래 후보표는 **쓰지 않은 예비**이며, 단서 구성이 바뀌거나 P1 을 한 겹 더 두껍게 하려 할 때만 꺼낸다.

**(b) 아직 필요한 것 — K8 의 재확인 경로 1건** [OBSERVED, A37-obs2 존치]

`proofRequired` 17비트 중 회선 기록 계보(`brine-log-dock` · `watchlog-bureau`)를 쓰는 비트는 **`c3-b1` 하나뿐**이고 그것이 곧 K8 의 확정 비트다. 두 번째 기계 검증 지점이 없으므로 P1 은 K8 에서만 1경로다.

**가장 싼 해법 — 단서 추가 0건** [OBSERVED · 규칙 실행으로 확인]: **`c3-b4`(B15)의 `proofRequired` 를 `true` 로 올린다.**
```
c3-b4 clues: ledger `tide-ledger-seongchan` | log `brine-log-dock`
적격쌍: ["c3-b4-c1","c3-b4-c2"]      # 종류 상이 AND 루트 상이 → C-07 통과
```
상한 적합 [INFERENCE]: B15 상한은 "성찬의 조건부 증언을 제안·결락·검증가능성으로 분해한다. 기준 관측소 차이는 12년 내내 같았다" — **두 매체를 실제로 대조하는 비트**이므로 `true` 가 서술과 어긋나지 않는다. 다만 `c3-b4` 는 도구 없는 5비트 중 하나이므로 **"확정 비트인데 도구가 없다"는 새 질문**이 생긴다 — 판단은 planner·systems 소유이며 synopsis 는 손으로 쌍을 만들지 않는다(A37 금지).

**후보표 — 만약 단서를 더해야 한다면** [TARGET 제안 · `glossary.md` §7 카탈로그 31종 안에서만]

| 대상 비트 | 후보 `originId` (KO) | `sourceType` | 루트 | 더해지는 독립쌍 | 상한 적합(`timeline.md` §7) | 순위 |
|---|---|---|---|---|---|---|
| `c1-b2` (K2) | `bureau-copy-index` (기록국 자동 사본 목록) | `ledger` | `tide-ledger-bureau` | × `signature-annex`(log) · × `plate-zero`(plate) → **2쌍** | B05 가 이미 "부속 서명지 두 장의 **등재 수**와 번호 불일치"를 허용한다 — 등재 대장이 그 자리에 필요하다 | **1** |
| `c1-b2` (K2) | `watchlog-archive-t12` (당직일지 12년 전) | `log` | 자기 루트 | × `plate-zero`(plate) → 1쌍 | 상한 안(그 시간대를 "전원 계통 불안"으로만 적은 일지). `log` 중복 | 2 |
| `c1-b2` (K2) | `brine-log-gate3` (제3수문 계통 로그) | `plate` | 자기 루트 | × `signature-annex`(log) → 1쌍 | B05 "판 #0의 번호대가 그날 제3수문 계통 12시간 대역과 같다"를 직접 뒷받침. `plate` 중복이고 `c1-b3` 가 이미 보유 | 3 |
| `c3-b1` (K8) | `plate-standard-hub` (당직실 표준판) | `plate` | 자기 루트 | × `tide-ledger-seongchan`(ledger) · × `brine-log-dock`(log) → **2쌍** | B12 "그의 기준 관측소가 기록국 표준과 **다르다**"가 대조 상대를 요구한다. `c3-b2` 가 같은 조합을 쓴다 | **1** |
| `c3-b1` (K8) | `tide-ledger-bureau` (기록국 조위대장) | `ledger` | 자기 루트 | × `brine-log-dock`(log) → 1쌍 | 상한 안. `ledger` 중복 | 2 |
| `c3-b1` (K8) | `forecast-table` (조위 예보표) | `ledger` | 자기 루트 | × `brine-log-dock`(log) → 1쌍 | **비추천** — 그날 예보와의 대조는 `c5-b1`·`c7-b2` 소유이며 3장에서 열면 상한을 앞당긴다 | 3 |

거부되는 조합(원장으로 남긴다) [OBSERVED, 검증기 규칙 파생]: `c1-b2` 에 `council-copybook` 과 `bureau-copy-index` 를 **둘 다** 넣어도 루트가 같아(`tide-ledger-bureau`) 서로는 쌍이 되지 못한다 · `c3-b1` 에 `tide-ledger-seongchan` 계열을 더하는 것도 같은 루트라 무효.

**이 절이 증명하지 않는 것** [OBSERVED]: 쌍이 기계 검증을 통과한다는 것은 **문서 정합**이며, 플레이어가 실제로 그 두 자료를 찾아 대조하는지는 미측정(n=0)이다 — 검증기 `notMeasured` 필드가 같은 문장을 담는다.

## 8. 측정되지 않은 것 [OBSERVED]

상태 전이의 실제 체감, 인지 범위 위반의 플레이 중 발생 여부, 필수 단서 도달률, 결말 3갈래 실제 도달, 4분 분해능 판정의 이해도 — **전부 n=0**. 본 문서는 문서 간 모순의 부재만 주장하며, 플레이에서 모순이 없다는 주장은 하지 않는다.
