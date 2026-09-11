---
updated: 2026-09-10
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
---

# r7-t0-data — T0 인스턴스 데이터 생성 영수증 (C7-F1 **S1** · RFC-C7-001)

> **명령 + 관측 결과**를 나란히 적는다. 요약이 원본을 대체하지 않는다.
> **이 파일은 어떤 게이트도 올리지 않는다.** Unity 임포트 0회 · 빌드 0회 · 플레이 표본 n=0 · 프레임타임 캡처 0건.
> 표기: `[OBSERVED]` 이 저장소에서 명령으로 확인 · `[TARGET]` 설계값 · `[INFERENCE]` 문서 대조 도출.

---

## 0. 무엇이 결함이었나

`qa/defect-register.md` **C7-F1(S1)**: 「T0 퍼즐의 인스턴스 데이터가 저장소에 없다 — `zones/plates/tools/hints/beats.json` `find` 0건, `campaign.json` 최상위에도 없음. DoD 4 는 캐논·수치 발명을 요구하고 README §2 가 그것을 금지한다」.

디렉터 판정 `production/decision-log.md` 「**RFC-C7-001 · C7-F1 (S1)**」 (2)·(3): 저장소에 `systems/data/t0/{zones,records,tools,hints,beats}.json` + `.meta.md` 를 만들되 **값은 발명이 아니라 파생**이며, `t0-b1`~`b3` **완료 술어**를 `beats.json` 에 명시한다.

해소 방식: **손으로 쓰지 않는다.** `systems/pipeline/emit-tables.mjs` 를 `--scope` 인식 생성기로 확장해 저작 문서에서 파생한다. 파싱이 실패하거나 문서 값과 생성 값이 어긋나면 **exit 2 로 멈추고 파일을 만들지 않는다**(fail-closed).

---

## 1. 생성 명령과 그 출력 원문 [OBSERVED 2026-09-10]

```
$ cd /Users/jangyoung/orca/unknown
$ node _workspace/current/systems/pipeline/emit-tables.mjs --out _workspace/current/systems/data/t0
```
종료 코드 **0**. stdout 은 `_workspace/current/systems/data/t0/tables-receipt.json` 과 **같은 내용**이며 전문은 그 파일이 보존한다. 아래는 그 출력 원문의 판정부다.

```json
{
  "emitter": "systems/pipeline/emit-tables.mjs",
  "emitterOwner": "game-systems-designer",
  "emittedUtc": "2026-09-10T00:43:44.019Z",
  "scope": "t0",
  "scopeSource": "out-dir-basename",
  "dryRun": false,
  "source": {
    "path": "planning/campaign.json",
    "sha256": "8a43d334f8f69d6642a74708e7b3d68353c7982fb5f506dbba291021a538a93a",
    "bytes": 124007
  },
  "validator": {
    "path": "planning/validate-campaign.mjs",
    "owner": "game-planner",
    "exitCode": 0,
    "checks": 49,
    "pass": 49,
    "fail": 0,
    "verdict": "PASS"
  },
  "tables": [
    { "file": "beats.json",   "derivation": "subset+predicate",     "sha256": "9bf5634a0908ef8f6bbcecf2fdd6d19ecf01e76eff1820ce213eec35a4bd5a8f", "bytes": 16532, "rows": 3 },
    { "file": "hints.json",   "derivation": "projection",           "sha256": "8bbfc9318ee6decde387245482901959be04dfe28f74e0831d530ffbd9bb4839", "bytes": 4032,  "rows": 9 },
    { "file": "tools.json",   "derivation": "schema-projection",    "sha256": "bf33d63389f31c058eb75601c4acad92ed7905ad64ffe69c915dac31f745e951", "bytes": 9499,  "rows": 2 },
    { "file": "zones.json",   "derivation": "authoring-derivation", "sha256": "3c3d043f6f4573ebafff1d4a5c6229d3ac632f3dadc3a425ef07e65214bd0f08", "bytes": 15341, "rows": 1 },
    { "file": "records.json", "derivation": "markdown-parse+rule",  "sha256": "c7c19303df1219fa49181dc0c28f9d262ed952b1e3b3cb97d28cf6baa3411dee", "bytes": 68470, "rows": 5 }
  ],
  "crossChecks": {
    "plateAnchors": "33/33 일치",
    "ledgerAnchors": "28/28 일치",
    "clueIdOriginMedium": "문서 ↔ campaign 전건 일치(불일치 시 exit 2)"
  },
  "toolInvariants": { "T-I1": "toolId 중복 0 · 6종 등록(구현 2 + 스텁 4)", "T-I3": "PASS", "T-I4": "PASS", "T-I7": "PASS" }
}
```

> **고정 sha·바이트를 다른 문서로 옮겨 적지 않는다**(RFC-Q1·RFC-B6). 위 값은 *이 실행*의 관측이며, 저작 원본이 바뀌면 달라진다. 인용이 필요하면 명령을 다시 돌린다. 여기 적는 이유는 「명령 + 관측 결과를 나란히」라는 §⑪-1 규약 때문이며, 이 표가 다른 문서의 인용 출처가 되지는 않는다.

생성된 파일 [OBSERVED, `ls -la _workspace/current/systems/data/t0/`]:

```
beats.json 16532  beats.meta.md 1917
hints.json 4032   hints.meta.md 1910
tools.json 9499   tools.meta.md 1917
zones.json 15341  zones.meta.md 1921
records.json 68470 records.meta.md 1926
tables-receipt.json 5305  tables-receipt.meta.md 1934
```

`.meta.md` 6건은 **생성기가 함께 낸다**(CLAUDE.md §10 「비-Markdown 산출물은 같은 basename 의 `.meta.md`」). 손으로 쓴 메타는 0건이며 각 메타는 `generated: true` 를 갖는다.

---

## 2. 값의 출처 — 발명 0건

| 파일 | 무엇에서 파생했나 | 발명하지 않은 방법 |
|---|---|---|
| `beats.json` | `planning/campaign.json` T0 서브셋 3비트 **그대로** + `completionPredicate` | 비트 본문은 필드 단위 복사. 술어는 live `beat.completion` 문장이 지목하는 대상을 기계 형태로 옮긴 것이며 원문을 `_srcCompletion` 에 함께 싣는다 |
| `hints.json` | 같은 3비트의 `hints[3]` 9행 투영 | 문자열은 `sourceTextKo` 로 옮기고 키는 결정론 규칙 `hint.{beatId}.l{level}` |
| `tools.json` | `systems/data-schemas/tools.md` §1 도구 표 · §4 노브 + `system-specs/{wiring-trace,plate-readout}.md` + `interaction-rules.md` §1-3.2/§1-3.3 | `lawId`·도입/재문제 비트·`commitHoldSeconds` 는 **표에서 파싱**. 명령 목록은 스펙 §1·§2 의 행을 옮긴 것이며 각 명령이 `_src` 를 갖는다 |
| `zones.json` | `concept/style-guide.md` §5(카메라 상수) · `modeling/specs/hub-watchroom.md` §2(방·작업대·선반) · `modeling/asset-manifest.md` §2(도구 6종 배치) · `worldview/worldview-bible.md` §2(회선 3개소·분해능) · `campaign.json`(스테이지↔구역) | 상수·좌표를 **정규식으로 파싱**한다. 파싱 실패는 exit 2 — 값을 채워 넣는 경로가 없다 |
| `records.json` | `synopsis/t0-records.md` 의 **형식 고정 표**(§2·§3.1·§4.1·§5.0~§5.5·§6·§7.1~§7.3) + `campaign.json`(단서 id·`originId`·`sourceType`) | 표 파서로 읽고, 곡선은 §5.2 코드 펜스의 **계수를 파싱해 규칙으로 생성**한다. 문서 앵커와 전건 대조해 어긋나면 exit 2 |

`rootOriginId` 는 `copiedFrom` 체인을 끝까지 따라가 계산한다(`interaction-rules.md` §3 · **RFC-S5 예외 없음**). T0 5레코드는 전부 `copiedFrom: null` 이므로 `rootOriginId == originId` 이며, 그 사실도 계산 결과다(하드코딩 아님).

---

## 3. 기계 대조 — 이 회차에 실제로 돌아간 검사

| # | 검사 | 결과 [OBSERVED] |
|---|---|---|
| X-1 | 검증기 `verdict` | `PASS` · `checks 49 / pass 49 / fail 0` · exit 0 |
| X-2 | 문서 단서 ↔ live 단서: `clueId` 존재 · `originId` 일치 · `sourceType` 일치 | T0 6단서 **6/6 일치** |
| X-3 | 염판 곡선 규칙(§5.2) ↔ 앵커 표(§5.3) | **33/33 일치, 불일치 0** |
| X-4 | 조위대장 규칙(§7.2) ↔ 앵커 표(§7.3) | **28/28 일치, 불일치 0** |
| X-5 | 링 샘플 수 = §5.0 `sampleCount` | 생성 **180** = 문서 180 |
| X-6 | 결손 구간 샘플 수 | **60 샘플 = 240분 = 정확히 4시간** (`t0-b3.consequence` 와 일치) |
| X-7 | 평평 구간 샘플 수 | **8 / 8 / 8** (합 24) · 상태 분포 `normal 96 · flat 24 · missing 60` = 180 |
| X-8 | 조위대장 칸 수 = §7.2 `136칸` | 생성 **136**, 공백 0 |
| X-9 | `viewNodes.neighbors` 양방향 · 고아 0 (`zones.md` Z-I3) | 6노드 링 · **위반 0** |
| X-10 | 노드 카메라가 방 경계 안 (`hub-watchroom` §2) | 6/6 통과 · 거리 클램프 발동 **0회** |
| X-11 | 도구 불변식 `T-I1`·`T-I3`·`T-I4`·`T-I7` (`tools.md` §5) | 전건 **PASS** |
| X-12 | `proofRequired` 비트에 확정 명령 존재 | `t0-b3` 1/1 — 없으면 exit 2 |

### 3.1 fail-closed 픽스처 — **실패가 실제로 실패하는지** [OBSERVED]

| 픽스처 | 명령 | 관측 |
|---|---|---|
| (a) 저작 원본 위반 | `t0-b1.zoneId` 를 `lowland` 로 바꾼 사본에 `--source` 로 물림 | 검증기 `fail=2` → 생성기 **exit 1**, 출력 디렉터리 **미생성**(`ls` = No such file) |
| (b) 저작 문서 ↔ 규칙 불일치 | `t0-records.md` 사본의 §5.3 앵커 `t=-340` 을 `57 → 58` 로 조작 | **exit 2** + 「§5.3 앵커 불일치 t=-340: 생성 … ≠ 문서 …」, 출력 디렉터리 **미생성** |

(b) 는 이번 회차에 **새로 생긴 안전장치**다 — 저작 문서와 생성 규칙이 갈라지면 조용히 두 번째 진실이 생기는 대신 빌드가 멈춘다.

---

## 4. 완료 술어 (RFC-C7-001 (3)) [OBSERVED · `beats.json` 실물]

| 비트 | `kind` | 확정 명령 | 요구 |
|---|---|---|---|
| `t0-b1` | `viewing` | **없음** | `rec-handover-brief` 3행(`hb-l1~l3`) 열람 · `rec-transfer-list` 3행(`tl-r1~r3`) 열람 · 상시 슬롯에 `plate-zero` 적재 · `tl-r4` 처리 여부 1회 기록(가역) |
| `t0-b2` | `marking` | **없음** | `hub` 미배선 구획 3개 지정 + 구획당 근거 매체 1건 |
| `t0-b3` | `citationPinned` | **`CiteToBoard`** (`checkpointBefore: true`) | 인용 고정 2건(`t0-b3-c1` plate/`plate-standard-hub` × `t0-b3-c2` ledger/`tide-ledger-bureau`) · 독립쌍 `C-07` · 자동 사본 1점 · 결손 양 끝(H-1:00 / H+3:00) 고정 |

`t0-b3` 의 독립쌍은 **손으로 고르지 않았다** — `node planning/validate-campaign.mjs --pairs` 출력(C-07 의 `independentPair`)을 생성기가 그대로 옮긴다. 그래서 planner 가 `proofRequired` 를 바꾸면 술어도 같이 바뀐다.

**`commitCommandBeats = ["t0-b3"]` · `proofRequiredBeats = ["t0-b3"]`.** 두 집합이 어긋나면 생성기가 exit 2 한다 — DoD 6·8 이 검사할 대상이 사라지는 것을 데이터 층에서 막는다(C7-F8).

---

## 5. 스코프 결정 규칙 (새 동작)

| 입력 | `scope` | `scopeSource` |
|---|---|---|
| `--scope t0` | `t0` | `--scope` |
| `--out …/t0` (마지막 경로 조각이 `^[a-z]\d$`) | `t0` | `out-dir-basename` |
| 그 외 | `all` | `default` |

세 경우 모두 영수증에 `scope`/`scopeSource` 로 찍힌다 — 어떤 규칙이 적용됐는지 출력만 보고 알 수 있다.
**기존 `--out <Unity Data/Tables>` 동작은 바뀌지 않았다** [OBSERVED]: `scope=all` 드라이런에서 `beats.json derivation=copy` 이고 그 sha256 이 저작 원본과 **같다**(`8a43d334…`), `hints.json` 99행. Unity `Data/Tables/` 에는 `.meta.md` 를 만들지 않는다(브리프 §④-1 의 3파일 계약 유지).

---

## 6. 이 영수증이 주장하지 않는 것

- **Unity 가 이 파일들을 읽은 적이 없다.** 임포트 0회 · `ZoneAsset`/`RecordAsset`/`ToolAsset` 로 변환한 적 0회. `systems/data/t0/` 는 **저작 산출물**이며 런타임 자산이 아니다.
- **`viewNodes` 6개는 [TARGET]이다.** 프레이밍이 실제로 성립하는지, 노드 수가 6이 맞는지는 n=0 이다(`data-schemas/zones.md` §6). `loadCostMb` 는 `null` 로 두었고 숫자를 지어내지 않았다.
- **좌표계는 그레이박스 프레임(Blender Z-up, m)이다.** Unity 변환식은 `zones.json` `poseFrame.unityConversion` 에 `[INFERENCE]` 로 적었고 임포트 실측(인수 `T-Z1`)이 확인 대상이다. 이 파일이 변환을 확정하지 않는다.
- **`hub-uncovered-1/2/3` 은 캐논 명사가 아니다.** `synopsis/t0-records.md` §5.6 이 옥외 구획 명명을 거부하고 **RFC-N9** 로 올렸으므로, 이 id 는 기술 식별자이고 `displayNameKey` 는 `null` 이며 `pendingRfc: "RFC-N9"` 를 달았다. 판정 전에는 UI 문자열로 쓰지 않는다.
- **`hub-view-drawer` 의 대상은 모델링되지 않았다.** 그레이박스에 서랍 오브젝트가 0개라 작업대 하부로 배치한 `[TARGET]` 이며 `assetStatus: "not-modeled"` 로 표시했다. modeling 레인 확인 대상이다.
- **곡선이 그럴듯한 것은 물리 검증이 아니다**(`t0-records.md` §11 승계). 규칙은 캐논 문장(대조차 9.2 m · 분해능 4분)을 만족하는 하나의 표현이다.

---

## 7. 열린 항목 (이 편집이 만든 것)

| id | 내용 | 대상 레인 |
|---|---|---|
| **OPEN-S8** | `zones.json` `poseFrame.unityConversion` 이 `[INFERENCE]`. 임포터가 `hub-greybox.glb` 임포트 결과와 대조해 확정 → 인수 `T-Z1` 신설 필요 | systems(실행자) |
| **OPEN-S9** | `systemIds: ["hub"]` 은 구역 토큰 재사용이다. 캐논은 계통에 고유명을 주지 않는다(`worldview-bible` §2 는 채널만 열거). 고유명이 필요해지면 worldview 판정 | worldview |
| **OPEN-S10** | `hub-uncovered-*` 3구획의 표시 이름 = **RFC-N9** 대기. 그 전까지 `displayNameKey: null` | worldview |
| **OPEN-S11** | `hub-view-drawer` 대상 프롭 미모델링(그레이박스 서랍 0개) | modeling |

## 8. 변경 로그 (같은 사이클 제자리 개정 · RFC-Q2)

| 날짜 | 회차 | 결함 | 바뀐 것 |
|---|---|---|---|
| 2026-09-10 | **R7 종료 수정** | **C7-F1**(S1) · C7-F8 | (신규 파일) 생성기 `--scope` 확장 · `systems/data/t0/` 12파일 생성 · 완료 술어 3건 · fail-closed 픽스처 2종 |

이 개정은 `cycle` 값을 바꾸지 않는다. **새로 측정된 게임 값 0건** — 바뀐 것은 저작 데이터의 존재와 그 정합뿐이다.
