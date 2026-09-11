---
updated: 2026-09-09
cycle: 20260909-preproduction-c5
status: superseded
supersedes: null
owner: game-product-manager
---

# Steam 등록 절차 체크리스트 (C5 초안 준비물)

## 0. 이 문서의 지위와 안전 경계
[TARGET] **C5 초안 준비물이며 완료된 사이클이 아니다.** 아래는 공식 문서에서 확인한 요건을 순서로 배열한 **점검표**이고, 실행 기록이 아니다. **현재까지 수행된 등록 행위는 0건이다.**

🔴 **다음은 전부 사용자 별도 승인이 있어야 한다**: Steam 계정 생성·연동, NDA·Steam Distribution Agreement 전자서명, 신원·은행·세무 정보 제출, $100 결제, 스토어 페이지 공개, 빌드 업로드, 리뷰 제출, 출시 버튼. 이 문서를 근거로 자동 진행하지 않는다.

🔴 **출시는 자동이 아니다.** Valve 문서는 리뷰 통과 후 "The release controls are now in your hands"라고 적는다 — 즉 마지막에 사람이 직접 누르는 행위가 남는다. 동시에 30일 대기·Coming Soon 2주·리뷰 통과가 **모두** 충족되기 전에는 누를 수 없다.

## 1. 계정과 보안 (S 단계)
- [ ] S1. Steam 소비자 계정 준비. Steamworks 온보딩과 $100 결제는 Steam 계정 기반으로 진행된다.
- [ ] S2. **앱 크레딧 귀속 주의**: 등록비를 결제한 **개인 계정에 app credit이 귀속되고 그 사람만 활성화할 수 있다.** 결제자와 실제 운영자를 다르게 두면 안 된다. 근거: https://partner.steamgames.com/doc/gettingstarted/appfee
- [ ] S3. **결제 수단**: Steam 지갑 잔액으로는 결제 불가("excluding Steam wallet funds"). 지역에서 지원되는 다른 결제수단 필요. 세금계산서는 **결제한 개인 계정 명의**로 발행되고 Valve는 같은 거래에 이중 인보이스를 발행하지 않는다. 근거: 위 appfee 문서.
- [ ] S4. **2단계 인증(Steam Guard 모바일 인증기)**: 🔴 **[미확인]**. 2026-09-09에 https://partner.steamgames.com/steamdirect 와 https://partner.steamgames.com/doc/gettingstarted/onboarding 본문을 전문 조회했으나 authenticator / Steam Guard / two-factor 문구가 **없었다**. "필수다 / 아니다"를 단정하지 말고, 실제 온보딩 진입 화면과 Steam 계정 보안 설정에서 직접 확인할 것. 운영 권고로는 켜 두는 것이 안전하다(권고이지 인용 가능한 요건이 아니다).

## 2. 법인격·은행·세무 일치 (L 단계)
근거: https://partner.steamgames.com/doc/gettingstarted/onboarding

- [ ] L1. **NDA와 Steam Distribution Agreement 전자서명**이 온보딩에 포함된다.
- [ ] L2. **법적 명의 일치**: "The Company Legal Name must be the legal entity that owns or has right to publish the product" 이고 "must match the name as written on official documents with your bank and on United States IRS tax documents or foreign tax documents". **DBA·상호·별칭 사용 금지.**
- [ ] L3. **개인 개발자**: 소유 주체가 개인이면 Company Form에 "Sole Proprietorship", Company Name에 **법적 성명**을 입력한다.
- [ ] L4. **은행 계좌 명의 일치**: "The account holder name on your bank account must match the name you provide when onboarding." 사업용 계좌가 없다면 **먼저 개설**해야 진행된다. 라우팅 번호·계좌번호·은행 주소 필요.
- [ ] L5. **세무 인터뷰**: 미국 조세조약 체결국 거주자는 **W-8BEN**에 준하는 정보가 필요하다. 제출한 세무 정보는 **제3자 검증**을 거치며 "**2-7 business days**"가 걸리고, 그 사이에는 **수정 불가**이며 추가 서류 요청 메일이 올 수 있다.
- [ ] L6. **원천징수**: 세율은 **0~30%**이며 **미국 원천 매출(US Share)에만** 적용된다. 조세조약 적용 세율을 낮추려면 W-8BEN과 TIN이 필요하고, 이미 납부된 원천세는 환급되지 않는다. 비미국 납세자는 연 1회 **1042-S**를 받는다. 근거: https://partner.steamgames.com/doc/finance/taxfaq
- [ ] L7. 🔴 **[미확인]** 한·미 조세조약상 로열티 실제 적용 세율(%)은 Steam 공개 문서에 없고 **세무 인터뷰 결과 화면**에서만 확인된다 → 세무대리인 확인 항목.
- [ ] L8. 🔴 **[미확인]** 개인 개발자의 국내 사업자등록·게임제작업 등록 필요 범위. 국세청 https://www.nts.go.kr/ · 홈택스 https://www.hometax.go.kr/ 에서 직접 확인. 해외 플랫폼 수익도 국내 신고 대상이 될 수 있다는 수준까지만 서술한다.

## 3. 등록비와 회수 (F 단계)
근거: https://partner.steamgames.com/doc/gettingstarted/appfee

- [ ] F1. **$100 USD(또는 상당액) / 앱 1개당**.
- [ ] F2. **환불되지 않는다(not refundable).**
- [ ] F3. 다만 **회수 가능(recoupable)** — 제품이 Steam 상점 또는 인앱 구매에서 **조정총매출(Adjusted Gross Revenue) $1,000 이상**을 달성한 이후의 지급분에서 회수되며, **월간 리포트에 별도 라인 항목**으로 표시된다.
- [ ] F4. 회계 처리 규칙: **선지출 고정비가 아니라 조건부 회수 항목**으로 분리 표기한다(business-model.md §4).
- [ ] F5. 지급 조건: 전월 매출이 **월 $100 임계**를 넘어야 EFT 지급, 미만이면 이월. 지급은 **판매월 +30일, 통상 월말**. 미국 외 은행은 USD SWIFT. 근거: https://partner.steamgames.com/doc/finance/payments_salesreporting

## 4. 시간 게이트 (T 단계)
- [ ] T1. **30일 초기 대기**: "A **30-day waiting period** between when you paid the app fee and when you can release your game." 근거: onboarding 문서.
- [ ] T2. **Coming Soon 최소 2주**: "put up a publicly-visible 'coming soon' page for **at least two weeks**" / "you **must have a Coming Soon page up for at least two weeks** before releasing." 근거: onboarding · https://partner.steamgames.com/doc/store/coming_soon
- [ ] T3. **스토어 페이지 리뷰 3~5영업일**: "Our review of your store presence typically takes **3-5 business days**." 근거: https://partner.steamgames.com/doc/store/review_process
- [ ] T4. **빌드 리뷰도 3~5영업일**, 동일 문서에서 "we ask that you plan for **at least 7 business days**"라고 권고 → **7영업일 버퍼를 일정에 고정한다.**
- [ ] T5. 온보딩 문서는 같은 리뷰를 "**This takes between 1-5 days**"로 표기한다. **두 수치를 섞지 말고 보수적으로 3~5영업일 + 7영업일 버퍼로 계획**한다.
- [ ] T6. **Review once; Update any time** — 최초 승인 이후 업데이트는 재심사 대상이 아니다(review_process).
- [ ] T7. 크리티컬 패스 합산: 결제 → 30일 대기 / 페이지 리뷰 제출 → 7영업일 버퍼 → 승인 → Coming Soon 2주 → 빌드 리뷰 7영업일 버퍼 → 출시. **이 구간들은 병렬 가능하지만 30일 대기와 Coming Soon 2주는 대체 불가다.**

## 5. 콘텐츠 설문 · 생성형 AI 공개 (D 단계)
근거: https://partner.steamgames.com/doc/gettingstarted/contentsurvey

- [ ] D1. 설문은 **일반 콘텐츠 / 성인 콘텐츠 / 생성형 AI** 3개 섹션이며 "**all must be completed prior to submitting the game to the Review Process**".
- [ ] D2. AI 섹션의 대상은 "**creating content that ships with your game, and is consumed by players**" — 아트, 사운드, 서사, 현지화 등. **개발 효율화 도구 사용은 이 섹션의 대상이 아니다.**
- [ ] D3. **Pre-Generated**(개발 중 AI로 만들어 동봉되는 콘텐츠)와 **Live-Generated**(실행 중 생성)를 구분해 신고한다. Live-Generated는 추가로 **가드레일**을 설명해야 한다.
- [ ] D4. 이 제품의 현재 설계에는 **실시간 AI NPC·절차적 대사가 제외**되어 있으므로(gdd.md 범위) 기본 신고는 Live-Generated 아님이 되지만, **실제 제작 시 사용한 도구 이력에 따라 Pre-Generated 신고 여부가 달라진다.** 제작 착수 시점부터 에셋별 생성 이력을 기록해 둔다.
- [ ] D5. 설문 답변이 면책이 되지 않는다: "Products on Steam must adhere to the content rules, **regardless of whether it is disclosed in these surveys**."
- [ ] D6. 출시 승인 후에는 일부 설문 항목을 **Steam 지원 문의 없이 수정할 수 없다** → 제출 전 확정.

## 6. 한국 등급분류 (K 단계 — 미확인 경로 포함)
- [ ] K1. `[확인됨]` 국내 사업자·개인이 국내에 유통·제공하는 PC온라인/비디오콘솔 게임물(사행성 제외)은 **게임콘텐츠등급분류위원회(GCRB)** 등급분류 대상이라고 GCRB가 안내한다. 출처: https://www.gcrb.or.kr/Institution/EtcForm01.aspx
- [ ] K2. `[미확인]` 근거 법령 조문 전문(게임산업법 제21조·제24조의2·제25조)은 자동 수집 시 iframe으로만 반환되어 축자 확보 실패. 인용이 필요하면 https://www.law.go.kr/LSW/lsInfoP.do?lsId=010196 에서 직접 열어 확인할 것.
- [ ] K3. 🔴 `[미확인]` **Steam(Valve)의 한국 자체등급분류사업자 지정 여부**는 공식 지정 현황 원문을 확보하지 못했다. **"스팀은 자체등급분류사업자다 / 아니다"를 어느 쪽으로도 단정하지 않는다.** 확인 창구: 게임물관리위원회 https://www.grac.or.kr/
- [ ] K4. **경로 판정 절차(미실행)**: ① grac.or.kr에서 자체등급분류사업자 지정 현황 원문 확인 → ② 해당되면 Steam 경유 등급 표시로 충분한지, 별도 신청이 필요한지 확인 → ③ 해당되지 않으면 GCRB 신청 절차·수수료·소요기간 확인. **세 단계 모두 미수행이며 사용자 확인 사항이다.**
- [ ] K5. Steam 콘텐츠 설문이 생성하는 지역 등급 표시는 "**several regional rating boards**"용 표시이며, **각국 심의기관의 법정 등급분류를 대체한다고 공식 문서가 말하지 않는다.** 두 축을 분리해 설명한다(contentsurvey 문서).

## 7. 스토어·빌드 준비 (P 단계)
근거: https://partner.steamgames.com/doc/store/review_process · https://partner.steamgames.com/doc/sdk/uploading

- [ ] P1. 스토어 페이지에는 **출시 시점에 실제 제공되는 기능·콘텐츠만** 표시한다. 미구현 기능을 적으면 리뷰에서 걸린다.
- [ ] P2. 캡슐 이미지에 **판독 가능한 타이틀/로고**, **스크린샷은 게임플레이만**, 설명은 구체적·일관되게, **외부 링크 금지**.
- [ ] P3. 빌드는 **표기한 모든 OS에서 정상 실행**되어야 하고, 스토어에 적은 기능이 **현재 빌드에 구현**되어 있어야 한다.
- [ ] P4. 업로드는 **SteamPipe**(청크 분할·차분 전송, 베타 브랜치·롤백 지원).
- [ ] P5. 데모를 낼 경우 **별도 App ID**이며 뎁포와 빌드를 따로 구성해야 하고, 데모 공개 후 **베이스 게임 스토어 페이지를 수동 재게시**해야 "Download Demo" 버튼이 노출된다. 근거: https://partner.steamgames.com/doc/store/application/demos
- [ ] P6. Coming Soon 페이지 공개 즉시 커뮤니티 허브가 활성화되고 **위시리스트 수집이 시작**된다 → 공개 시점은 마케팅 결정이자 되돌리기 어려운 공개 행위다(승인 필요).

## 8. 미확인 항목 요약 (사용자 확인 필요)
| # | 항목 | 상태 | 확인 창구 |
|---|---|---|---|
| U1 | Steamworks 온보딩의 2FA 필수 여부 | **미확인**(공개 문서에 문구 없음) | 온보딩 진입 화면 / Steam 계정 보안 설정 |
| U2 | KRW 최저 기준가 숫자 | **미확인**(로그인 필요) | https://partner.steamgames.com/pricing/minimums |
| U3 | 수익 배분율 수치 | **미확인**(계약서에만 존재) | Steam Distribution Agreement |
| U4 | Steam의 한국 자체등급분류사업자 지정 여부 | **미확인** | https://www.grac.or.kr/ |
| U5 | 한·미 조세조약 실제 적용 세율 | **미확인** | 세무 인터뷰 결과 / 세무대리인 |
| U6 | 국내 사업자등록·게임제작업 등록 범위 | **미확인** | 국세청·홈택스 |

## 9. 출처 (전부 2026-09-09 접속)
1. https://partner.steamgames.com/steamdirect
2. https://partner.steamgames.com/doc/gettingstarted/onboarding
3. https://partner.steamgames.com/doc/gettingstarted/appfee
4. https://partner.steamgames.com/doc/gettingstarted/contentsurvey
5. https://partner.steamgames.com/doc/store/review_process
6. https://partner.steamgames.com/doc/store/coming_soon
7. https://partner.steamgames.com/doc/store/application/demos
8. https://partner.steamgames.com/doc/sdk/uploading
9. https://partner.steamgames.com/doc/finance/payments_salesreporting
10. https://partner.steamgames.com/doc/finance/taxfaq
11. https://partner.steamgames.com/pricing/minimums (로그인 필요, 미확보)
12. https://www.gcrb.or.kr/Institution/EtcForm01.aspx
13. https://www.law.go.kr/LSW/lsInfoP.do?lsId=010196 (조문 전문 미확보)
14. https://www.grac.or.kr/
