---
updated: 2026-09-13
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-production-director
---

# Aside 코어 루프 조사 실행 영수증

| 파일 | bytes | SHA-256 |
|---|---:|---|
| `aside-run.json` | 5318 | `ab7c2be7ea7e7dd5a211e05d159aaca8ecaaddfa255fcdceabe22f11fc1adbf2` |

- [최종 권고와 Main 원문/코드 대조](../../handoff/aside-core-loop-results-20260913.md)
- [Aside 전체 보고서](../aside-core-loop-research-20260913.md)
- [실험 계약과 검증 경계](game-design-hypothesis.meta.md)

## 실행·출처

Main이 실제 호출한 Aside CLI 인수, 종료 코드 0, 세션 식별자, handoff/보고서/실험/별도 초점 회귀의 바이트·SHA를 보존했다. `artifact://277`은 현재 도구 세션의 실행 출력 참조이고 장기 공개 URL이 아니다. 전체 개인 브라우저/다른 세션 목록·인증값은 저장하지 않았다.

Aside는 Deep Research 스킬의 원문 조사 절차와 실제 브라우저 읽기를 수행했다. 명시적 deep 모델의 402 실패·기본 프로필 재배정과 표준 Deep Research JSON 파일 파이프라인 미실행을 함께 기록한다. 보고서 12개 출처 항목이 12개 독립 실험이라는 뜻은 아니다.

## 증거의 한계

원문·코드 대조와 JSON 구조 검증은 이번 권고의 이해 효과·재미·접근성·전체 게임 완성도를 증명하지 않는다. 시편과 사람 플레이는 실행하지 않았으며 n=0이다. 초점 복귀 PlayMode 1/1은 앞선 M22 계약의 별도 보완이고 고유 157건에 더하지 않는다. 런타임·정본·상업 게이트·commit/push는 변경하지 않았다.

`mex-agent`는 확인된 실행 파일이 없어 사용하지 않았다. PATH의 동명 `mex`는 TeX이므로 호출하지 않았다. `.mex/ROUTER.md`와 생산 원장에 조사 링크·판정 경계를 직접 반영했다. 파일 freshness 검사는 메모리 전체 동기화나 G8 전체 PASS를 대신하지 않는다.

## 지식 갱신 범위

[OBSERVED 2026-09-13] 기존 저장소의 `refresh-or-fallback` / incremental-refresh로 `graphify update .`를 한 번 실행했다. 실제 설치의 정본인 `graphify-out/graph.json`과 `GRAPH_REPORT.md`를 갱신했고 레이아웃 마이그레이션은 하지 않았다. AST 2,199/2,199파일, 38,634노드·42,670간선·4,102커뮤니티, 43.55초, 새 LLM 호출 없음. 문서/논문/이미지의 의미 추출은 이번 명령 범위 밖이며, 5,000노드 시각화 제한으로 `graph.html`도 갱신되지 않았다. 연구 문서는 Main handoff와 `.mex/ROUTER.md` 경로로 읽는다.

## 동시 작업의 Git 상태

[OBSERVED 2026-09-13] 종료 전 읽기 전용 상태 대조에서 main HEAD는 `b2ae1fb8119be1b797f874c38441cbd433aa72d4`였다. 부모 `52ad551`에서 22:13:25+09:00에 생긴 외부 `docs(m13)` 커밋이며 이 세션이 커밋한 것이 아니다. 워크트리는 여전히 main 하나이고 staged 0/unstaged 66/untracked 73의 관측 스냅샷이다. 런타임·리서치 변경이 미커밋으로 남았으므로 ‘전부 커밋/병합 완료’가 아니다. `commitOrPushPerformedByThisSession:false`는 이 세션의 행동만 뜻한다. 외부 actor·원격 서버 상태는 추정하지 않고 사용자 작업은 모두 보존했다.

[OBSERVED] 파일 freshness 검사는 662개 Markdown에서 0 finding, 두 JSON sidecar와 영수증의 중첩 파일 5개 지문·로컬 링크 6개가 일치했다. zg의 갱신 후 관측은 2,011/2,011파일·104,402 entities·182 truncated fragments·대기/실패 0이었다. 코드 그래프·검색 인덱스의 범위/누락과 G8 전체 조건은 별개다.
