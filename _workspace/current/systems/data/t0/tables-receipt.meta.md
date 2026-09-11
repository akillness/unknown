---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-systems-designer
describes: _workspace/current/systems/data/t0/tables-receipt.json
generated: true
---

# `tables-receipt.json` 메타 — T0 인스턴스 데이터 (RFC-C7-001 · C7-F1)

**이 파일도 `tables-receipt.json` 도 손으로 쓰지 않는다.** 둘 다 아래 명령의 출력이며, 값의 출처는 각 필드 옆 `_src` 가 갖는다.

## 1. 생성 명령

```
node _workspace/current/systems/pipeline/emit-tables.mjs --out _workspace/current/systems/data/t0
```

## 2. 이번 출력 [OBSERVED 2026-09-11]

| 항목 | 값 |
|---|---|
| 파생 | `receipt` |
| sha256 | `3c293c5a5b7a9faff243d413faaae34835ce2b228103c53ba48e7bc525a9583b` |
| 바이트 (`wc -c`) | 5461 |
| 행 수 | 5 |
| 검증기 판정 | `verdict PASS · checks 50 · fail 0` (exit 0) |

> 해시·바이트는 저작 원본이 바뀌면 달라진다. 인용할 때 옮겨 적지 말고 생성기를 다시 돌린다(RFC-Q1).

## 3. 저작 출처 (이번 실행이 읽은 파일)

| 파일 | sha256(앞 12) |
|---|---|
| `planning/campaign.json` | `044780ef4349…` |
| `synopsis/t0-records.md` | `d586adf86cd1…` |
| `systems/data-schemas/tools.md` | `f7ee4e1b854f…` |
| `systems/data-schemas/zones.md` | `0c166c80b9b5…` |
| `concept/style-guide.md` | `a70e3dc79292…` |
| `modeling/specs/hub-watchroom.md` | `d2e08e4e119f…` |
| `modeling/asset-manifest.md` | `b1e8654f4fcf…` |
| `worldview/worldview-bible.md` | `87a9617ec9fe…` |
| `systems/system-specs/wiring-trace.md` | `60d6f2b15844…` |
| `systems/system-specs/plate-readout.md` | `55f502a63cbf…` |
| `systems/interaction-rules.md` | `8e28c2bfe584…` |
| `planning/t0-circuit-overlay.json` | `2e3ea0a1aead…` |

## 4. runtime 관측

**NOT-MEASURED.** Unity 임포트 0회 · 빌드 0회 · 플레이 표본 n=0 · 프레임타임 캡처 0건.
이 파일은 데이터 정합만 증명하며 어떤 게이트도 올리지 않는다.
