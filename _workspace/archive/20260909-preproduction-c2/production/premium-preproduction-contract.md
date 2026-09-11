---
updated: 2026-09-09
cycle: 20260909-preproduction-c1
status: superseded
supersedes: null
owner: game-production-director
---

# Premium Preproduction Contract

## Scope
[OBSERVED] 사용자 요청: Steam 최신 인기 경향 검토, Unity, 8시간 완결형 본편, 원화 출시 정가 최소 9,000원, 인터랙티브한 플레이, 세계관·서사·연출·UI·튜토리얼·DLC, 최소 5회 실제 기획 개선 사이클, 등록/수익화 HTML 슬라이드.

[OBSERVED] baseline b79577b에는 하네스만 있고 게임·시장·플레이 측정은 없다. 라이브 서비스라는 기존 전제는 이번 신작에 적용되지 않는다.

## New cycle type: preproduction
기존 hotfix/balance-patch/content-update/season을 삭제하지 않고, 출시 전 preproduction을 추가한다. 진입 P0→P1→P2→문서/모형 제작→독립 검토→수정→종료. 5회는 서로 다른 입력과 출력 해시, 문제, 수정, 잔여 위험이 있어야 한다. 동일 문서에 회차 제목 5개만 만드는 것은 미실행이다.

1. C1 시장·범위: 후보 최소 3개, 비교작 최소 6개, 가격/일시/표본편향, JTBD·가설.
2. C2 인과·서사: 세계 규칙, 등장인물의 이해관계, 단서→판단→행동→결과, 유료 결말 금지.
3. C3 캠페인·시간: 튜토리얼부터 결말까지 필수 경로, 각 세션 목표·규칙·힌트·실패 복구·저장, 순수 플레이타임 예산.
4. C4 상호작용·연출·Unity: 입력/포커스/접근성, 다섯 시각 레인, 기능 슬라이스, 측정 가능한 인수 기준.
5. C5 상품·생산·회귀: 가격 민감도, DLC 증분 가치, 제작량/예산, 등록 공식 근거, 전 문서 일관성.

## Honesty gates
문서 검증 D1~D5와 실제 게임 G1~G8은 다르다. 시간 합계가 480분이어도 G7 플레이 검증을 PASS하지 않는다. 실제 빌드가 없으면 G2/G4/G5/G6/G7은 NOT-MEASURED. PM의 수익/판매/위시리스트/전환율은 목표·가정이며 관측치로 쓰지 않는다. 시장성은 매출 보장이 아니다.

## Premium overrides
전투 승률·TTK·유료 무료 격차·일간 인플레이션은 비전투 완결형 게임에 부적합하다. 전투가 없다는 확인이 있을 때만 N/A로 명시하고, 퍼즐 도달 가능성·진행 막힘·선택 상태·무료 힌트·본편 완결성·DLC 의존성 검증으로 대체한다. 누락을 PASS로 위장하지 않는다.

## Time acceptance proposal
[TARGET] 처음 플레이한 일반 이용자(공략 없음)의 본편 완주 중앙값 450~540분; 설계 중심값 480분. 익숙한 이용자의 빠른 완주를 막지 않는다. 선택 콘텐츠·뉴게임플러스·수집 100%·로딩·일시정지·자리비움은 본편 목표에 합산하지 않는다. 실제 표본 최소 12명/5유형, 탈락 포함 보고, 60초 무입력은 자동 제외하지 않고 회고로 생각 시간과 자리비움을 구별한다.

## Release safety
이 작업은 로컬 기획·하네스 개선과 문서 제작 승인이다. Steam 계정 계약/신원/은행/세금 제출, Direct 비용 결제, 유료 에셋/외주/API, 외부 모집·상점 공개, git commit/push는 별도 승인. Unity 전체 콘텐츠 생산은 문서만으로 자동 시작하지 않는다. 검증용 최소 슬라이스를 먼저 만든다.

## Owners
기존 13개 전문 역할을 유지하고 독립 game-product-manager를 추가한다. PM은 제품 가치·가격·GTM·손익, economy는 게임 내 자원과 공정성 담당. PM이 QA 결과를 수정하거나 QA가 구매 의사를 만들어내지 않는다.

## Evidence storage
기존 archive는 수정 금지. 대체 문서는 archive-cycle 경유, 현재 버전은 supersedes 연결. JSON/CSV/HTML은 frontmatter를 넣어 파서를 깨지 않고 같은 basename의 .meta.md에 소유자·해시·출처·관측/목표를 기록한다. 실제 측정이 없으면 evidence=[]/NOT-MEASURED.
