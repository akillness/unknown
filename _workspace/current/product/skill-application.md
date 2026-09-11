---
updated: 2026-09-10
cycle: 20260909-preproduction-c5
status: current
supersedes: null
owner: game-product-manager
---

# PM and Solo Application

[OBSERVED] 감사 핀: phuryn/pm-skills 18468a95b427e70e258b51389796367c6f684e7d, bam-bam-2/solo-skills d881ced23fae35466f0aa44c623267260fe0937c. 이 작업은 설치가 아니라 검증한 절차의 선택 적용이다. 업스트림 코드를 통째로 복제·실행하지 않았다.

## 채택 → 파일
| 실제 PM 스킬 | 적용 산출물 |
|---|---|
| monetization-strategy | product/business-model.md의 단품/서사DLC/사운드트랙/디럭스 선택 |
| pricing-strategy | economics.json·가격3후보·손익민감도·4문항실험 |
| brainstorm-experiments-new | assumption-tests.md 행동기반 실험 |
| prioritize-assumptions | 영향/증거/비용/정지선 표 |
| opportunity-solution-tree | planning/market-decision.md 기회트리 |
| beachhead-segment | 가설 타깃, 이미조사한사용자라고하지않음 |
| positioning-ideas | 완결형·공간변화·직접조작 3문장제품가치 |
| competitor-analysis | 날짜·가격·할인·출처가붙은비교표 |

로컬 PM 래퍼의 오래된 커맨드 목록은 호출하지 않는다. 정본 경로는 https://github.com/phuryn/pm-skills/tree/18468a95b427e70e258b51389796367c6f684e7d. 공개서브트리 감사에서 로컬호출좌표 드리프트를 발견했으나 전역스킬 수정은 범위밖이라 하지 않았다.

## Solo의 실제 역할
humanize-korean: 대사의 명사나열을 줄이고 인물마다 짧은 동사를 다르게 썼다. meeting-summary: RFC를 '결정사항/담당액션' 두층으로 적었다. measured-ui-callouts: 실제 최종화면 픽셀/DOM경계로 **넘침**을 검수했다(4개 뷰포트 × 36장, 넘침 0장 — `presentation/steam-game-plan.meta.md` 「기하 측정」 절). **포커스 순서·초점 트랩은 측정하지 않았다** — `inert`/`aria-hidden` 적용은 선언일 뿐 실측이 아니다(QA C5-F11, 2026-09-10 정정: 이전 판본이 "넘침과포커스 검수"로 적어 하지 않은 측정을 했다고 읽힐 수 있었다). 다음 회차에 포커스 순회를 실제로 측정하거나, 측정 전까지는 이 문구를 유지한다. web-demo-video: 동사→결과→새질문 스토리보드만 적용, 웹iframe조작을Unity로가장하지 않는다.

가격·BM은Solo의전문산출물이아니다. 저자계정/SSH/Notion공지/예약게시/유료생성은 채택하지 않았다. https://github.com/bam-bam-2/solo-skills/tree/d881ced23fae35466f0aa44c623267260fe0937c.

## 시장가치에 대한 한계
프레임워크의출력은 구매증거가아니다. 출시가격/판매량/재미는 모든파일에서TARGET또는미측정. 8시간당가격/TAM스케일/경쟁작리뷰→판매량으로강행하지 않는다.
