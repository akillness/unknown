---
updated: 2026-09-10
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-modeler
---

# `handoff/rfc-inbox/` — 실행자 RFC 제출함 (C7-F12 해소)

**실행자(Codex · GPT-6 Astra)와 레인 에이전트는 `production/decision-log.md` 를 쓰지 않는다.**
되물을 것·영수증·승격 감사는 이 폴더에 **파일 하나로 제출**하고, **디렉터가 판정해 `production/decision-log.md` 에 append** 한다.

> 근거 [OBSERVED 2026-09-10]: `production/decision-log.md` 「C6-F11 / PRE-1 / C7-F14 / C7-F12 / C7-F3 · T0 착수 전 결정 4건 + 실행자 규칙」 판정 **⑥** —
> 「실행자(Codex)는 decision-log 를 쓰지 않는다 — RFC 는 `handoff/rfc-inbox/RFC-CX-{n}.md` 로 제출하고 디렉터가 append 한다(런북 3곳 정정, modeling)」.
> `handoff/README.md` §3 「`production/decision-log.md`(디렉터 소유 — 실행자 쓰지 않는다)」와 같은 방향이다.

## 1. 파일 이름

```
_workspace/current/handoff/rfc-inbox/RFC-CX-{n}.md      n = 001 부터 증가. 이미 쓰인 번호를 재사용하지 않는다
```

번호가 겹치면 큰 쪽을 쓴다(`ls _workspace/current/handoff/rfc-inbox/` 로 먼저 확인). **파일을 지우거나 옮기지 않는다** — 판정 후에도 남는다(CLAUDE.md §2 "삭제는 없다").

## 2. 양식 (그대로 복사해 채운다)

```markdown
---
updated: {YYYY-MM-DD}
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: {제출자 — codex-executor | game-modeler | …}
---

### RFC-CX-{n} · {한 줄 제목}
- lanes: {영향 레인들 — modeling / systems / vfx / animation / director …}
- question: {예/아니오 또는 A/B 로 답할 수 있는 한 문장}
- proposal: {지금 채택하고 싶은 안 + 그 이유}
- evidence: {읽은 파일 경로와 행/절. 없으면 "없음"이라고 적는다}
- blocking: {yes = 판정 전까지 코드·에셋이 못 나간다 / no = 임시안으로 진행 가능}
- measured: {[OBSERVED] 명령과 그 출력. 실측이 없으면 "n=0" 이라고 적는다}
- decided_by: (비워 둔다 — 디렉터가 채운다)
```

- `blocking: no` → **임시안을 `// RFC-CX-{n} 임시안`** 주석으로 표시하고 진행. 판정이 오면 주석을 지운다.
- `blocking: yes` → 컴파일되는 스텁으로 남기고 다른 작업으로. **추측한 값을 데이터 테이블·provenance 에 넣지 않는다.**
- 판정은 디렉터가 `production/decision-log.md` 에 append-only 로 쓴다. 그 뒤에는 **판정문을 인용해** 구현한다.

## 3. 이 폴더로 오는 것 / 오지 않는 것

| 이 폴더에 제출 | 여기가 아님 |
|---|---|
| Higgsfield 크레딧 **영수증**(실행 전/후 잔액과 차액) — `asset-runbook.md` §1.3·§7-6 | 진행 보고·질문 스레드 → `_workspace/current/messages/{seq}-codex-*.md` |
| **승격 감사** 8항목 표 — `asset-runbook.md` §3.1 | 기술 검증 영수증 → `_workspace/current/systems/tech-verification/{name}.md` |
| 패키지 추가 요청(예: GLB 임포터 glTFast 계열 — `pipeline.md` §6.2) | 코드·에셋 자체 → `unity/Unknown/**` |
| 저장 필드 이름·타입, 밸런스/경제 숫자, 도구·구역·비트 id, UI 문자열, 캐논 서술, 범위 확대 | 변수·클래스 이름, 내부 자료구조, 테스트 픽스처 이름(되묻지 않는다) |

## 4. 알려진 문서 불일치 (이 폴더가 스스로 닫지 않는다)

`handoff/README.md` §2 는 RFC 제출처를 `_workspace/current/messages/{seq}-codex-{주제}.md` 로 적는다. 판정 ⑥ 은 **이 폴더**를 지정한다.
`handoff/README.md` 는 systems 레인 소유이므로 modeling 이 고쳐 쓰지 않는다 → **디렉터·systems 판정 사항**.
그때까지의 안전한 읽기: **RFC 는 이 폴더(판정 ⑥ 이 최신·정본), 진행 보고·질문 스레드는 `messages/`.**

## 5. English

Executors never write `production/decision-log.md`. File one Markdown document per RFC here as `RFC-CX-{n}.md` using the template in §2 (question / proposal / evidence / blocking / measured), and the **director** rules on it and appends the ruling to the decision log. Credit receipts (Higgsfield balance before/after) and the 8-item promotion audit go here too. Progress reports and open questions go to `_workspace/current/messages/`; technical verification receipts go to `systems/tech-verification/`. Never delete or move a submitted RFC — it stays after the ruling. Note that `handoff/README.md` §2 still points RFCs at `messages/`; that file is systems-owned, so the discrepancy is the director's to settle — until then, ruling ⑥ (this folder) is the newer instruction.
