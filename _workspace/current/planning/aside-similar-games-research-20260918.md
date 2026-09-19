---
updated: 2026-09-18
cycle: 20260918-content-update-m26
status: current
supersedes: null
owner: game-planner
---
# 유사 추리 게임 조사: 첫 10–30분과 증거·가설 UI
## 0. 조사 결론
[OBSERVED] 현재 개발 가능 범위는 T0 3비트부터 C1-b2까지이며, 배선 추적과 판독이 본편에 열려 있고 조위정합은 본편과 격리된 연습장으로만 존재한다. [P01][P03]
[OBSERVED] M23에서 R1 두 자료 동시 비교, R2 직접 근거 선택, R3 출처 계보, R5 조위정합 시편, R7 저장 피드백이 구현·검증됐다. [P03]
[OBSERVED] R6 사람 평가와 R8·R9 후반 생산, R10 전체 캠페인 검증은 gate-blocked다. [P03]
[OBSERVED] M25는 F2 안내 오버레이에 목표·세 단계·도구 절차·조작·규칙·인물·구역을 모았고, 회로·판독 화면에는 현재 단계와 남은 조건 수를 표시한다. [P01]
[INFERENCE] 현재 슬라이스의 강점은 안전한 실험, 출처 독립성, 비교, 저장 실패의 가시화다.
[INFERENCE] 비교작 대비 가장 큰 미확인 영역은 ① 규칙을 작은 현장 과제로 가르치는 도입, ② 현재 질문 중심의 지식 연결, ③ 자료 충분성과 가설 정답을 분리한 부분 상태, ④ 기록이 추가됐을 때 다음 조사 방향을 만드는 로그다.
[TARGET] 제작 우선순위는 UI 확장보다 먼저 한서린 플레이어 캐릭터의 Higgsfield 정체성 리소스와 T0–C1 도구·저장·확정 연출, 이펙트, 모션을 강화하는 것이다.
[INFERENCE] Higgsfield 영상·이미지는 캐릭터 일관성 시트와 모션 프리비즈로 사용하고, 입력에 반응해야 하는 실제 런타임 효과는 Unity 애니메이션·VFX로 재저작·검증해야 한다.
[INFERENCE] 아래에서 “없다”는 표현은 지정된 현재 문서·캡처에서 확인되지 않았다는 뜻이며, 검사하지 않은 코드 전체에 절대 부재한다는 뜻이 아니다.
[TARGET] 권고는 전투·유료 재화·멀티플레이·실시간 타이머·자유 카메라·새 도구 동사를 추가하지 않는다.
[TARGET] 모든 확정은 기존 계약대로 프리뷰 → 2단계 확인 → 저장 성공 뒤 적용, 무료 힌트, 무제한 Undo를 유지한다.
[TARGET] 모든 증거 제출은 서로 다른 매체 2종 규칙을 유지하고 플레이어에게 답을 직접 말하지 않는다.
[TARGET] 인물 공개는 시작 시 한서린·한도연, C1 이후 문재화까지만 허용한다.
## 1. 방법과 한계
[OBSERVED] 비교작 10개를 본문 대상으로 선정했다: Obra Dinn, Case/Rise of the Golden Idol, Roottrees, Chants of Sennaar, Lorelei, Strange Horticulture, Her Story, Papers Please, Outer Wilds. [W01][W06][W11][W16][W21][W26][W32][W37][W42][W47]
[OBSERVED] Pentiment와 IMMORTALITY는 각각 불확실한 책임 판단과 비선형 미디어 탐색의 보조 근거로만 사용했다. [W52][W54]
[OBSERVED] 각 본문 게임은 Steam 상점, 영어 전체 기간 Most Helpful 긍정·부정 리뷰 스냅샷, 개발자 인터뷰·글 또는 설계 자료를 읽었다.
[OBSERVED] Steam 리뷰 정렬은 가중 도움순 스냅샷이며 전체 언어의 절대 득표 순위나 전체 이용자의 빈도 통계가 아니다.
[OBSERVED] 첫 10–30분은 공개 가이드·개발자 설명·리뷰로 재구성했으며 직접 플레이 시간을 측정하지 않았다.
[INFERENCE] 리뷰의 찬반은 설계 신호이지 객관적 결함 판정이 아니다.
[INFERENCE] 가격은 2026-09-19 한국 Steam 표시가이며 할인·지역 정책에 따라 바뀔 수 있다.
## 2. 게임별 관찰
### 2.1 Return of the Obra Dinn
[OBSERVED] 가격·포지셔닝: ₩21,500, 보험 조사관이 실종선의 인물과 운명을 관찰·논리로 규명하는 1인칭 미스터리다. [W01]
[OBSERVED] Core loop: 시체의 마지막 순간 진입 → 정지 장면·대사·명부·스케치 교차대조 → 책에 이름·운명 가설 입력 → 다른 기억에서 검증한다. [W04]
[OBSERVED] 초반 teaching: 책과 시계를 받은 뒤 첫 시체에서 장면 탐색과 운명 입력을 행동으로 익히며, 답 대신 다음 시체의 존재와 책의 빈칸을 제공한다. [W04]
[OBSERVED] Evidence UI: 책은 승선 명부, 스케치, 갑판도, 장별 기억, 가설 입력을 한 구조에 결합한다. [W04]
[OBSERVED] 부분 상태: 흐린 얼굴이 선명해지는 것은 필요한 장면을 봤다는 신호이고, 삼각형 1–3개는 식별 난도이며 확신도 게이지가 아니다. [W04]
[OBSERVED] 오답·충분성: 올바른 운명 3개가 모이면 인쇄체로 잠기며, 오답은 즉시 특정되지 않고 손글씨 가설로 남는다. [W04]
[OBSERVED] Provenance: 기억은 장·시체 위치와 연결되고 대사 전사는 페이지에 붙지만, 억양 같은 음성 정보는 텍스트만으로 완전히 대체되지 않는다. [W04][W05]
[OBSERVED] 접근성: 한국어·컨트롤러·대사 텍스트를 지원하며 공식 멀미 문서는 출력 모드·색 조합·30fps 선택을 안내한다. [W01][W05]
[OBSERVED] 리뷰 칭찬 Top 3: “everything comes together”, “actually perform the challenges”, “feeling of accomplishment”로 완성도·직접 추론·진행 성취를 칭찬한다. [W02][W56]
[OBSERVED] 리뷰 불만 Top 3: “pacing … forced”, 재방문이 “super tedious”, 인간 기억에 “memory intensive”하다는 불만이다. [W03]
[INFERENCE] Unknown이 가져올 것은 3개 잠금 숫자가 아니라 가설·관찰·확정의 서체와 상태 분리다.
### 2.2 The Case of the Golden Idol
[OBSERVED] 가격·포지셔닝: ₩20,500, 연결된 죽음 사건의 범인·동기·방법을 정적 장면과 구조화 문장으로 재구성한다. [W11]
[OBSERVED] Core loop: 장면 조사 → 단어 수집 → Thinking 화면에서 인물·장소·인과 문장 빈칸 배치 → 섹션 검증이다. [W14][W57]
[OBSERVED] 초반 teaching: 첫 사건은 살인 행위 자체는 명백하게 보이고, 소지품·계약서·지도·배경을 통해 이름과 위치만 추론하게 해 규칙을 축소한다. [W14]
[OBSERVED] Evidence UI: Exploring과 Thinking을 분리하고, 저자가 정한 문장 틀 안에서 가설을 외부화한다. [W14][W57]
[OBSERVED] 부분 상태: 수집 단어 총수는 자료 수집 완료를 알리지만 장면 이해를 보증하지 않는다. [W14]
[OBSERVED] 오답·충분성: “Two or less slots are incorrect”는 정답 거리를 거칠게 알리되 틀린 칸을 직접 지목하지 않는다. [W14]
[OBSERVED] Provenance: 단어 저장소는 출처를 압축하지만 실제 추론은 주머니·문서·지도·물건·시선·애니메이션의 문맥에 의존한다. [W14]
[OBSERVED] 접근성: Redux는 가독성·힌트·저장·Steam Deck 대응을 개선했고 Classic/Redux 선택을 제공하지만, 탭 증가를 비판하는 리뷰도 있다. [W15]
[OBSERVED] 리뷰 칭찬 Top 3: “space to think”, “everything would come into focus”, “satisfying ‘aha!’ moments”로 자력 추론·도달 가능한 답·깨달음을 칭찬한다. [W12]
[OBSERVED] 리뷰 불만 Top 3: “extremely short”, “detective gameplay felt quite shallow”, 정보가 “just handed to you”라는 불만이다. [W13]
[INFERENCE] Unknown은 정답 근접 수치를 복제하지 말고, 구조적 제출 준비도만 알려야 한다.
### 2.3 The Rise of the Golden Idol
[OBSERVED] 가격·포지셔닝: ₩21,500, 1970년대의 20개 사건과 장 단위의 큰 연결을 푸는 독립형 후속작이다. [W16]
[OBSERVED] Core loop: 장면 위 이동식 퍼즐 창에서 이름·사건 문장을 채우고, 장 끝에서 여러 사건의 관계를 다시 요약한다. [W19]
[OBSERVED] 초반 teaching: 단순 빈칸 조작 뒤 신분증 직접 증거 → 명단·방 번호 대조 → 도구의 출처 순으로 교차 추론을 확장한다. [W19]
[OBSERVED] Evidence UI: 별도 사고 화면 대신 장면 위 창을 띄워 자료와 가설을 동시에 보게 하지만 창 관리 비용이 생긴다. [W19]
[OBSERVED] 오답·충분성: 틀림/틀린 단어 2개 이하 피드백과 부분별 힌트가 있으며, 오답은 편집 가능한 임시 답안이다. [W19]
[OBSERVED] Provenance: 문서·대화·소지품뿐 아니라 TV 재생·되감기·프레임과 동일 공간의 다른 시점이 근거가 된다. [W19]
[OBSERVED] 접근성: 세밀한 이미지와 중첩 창은 비교에 유리하지만 UI 배율·대비에 관한 사용자 불만은 현재 패치에서 재검증이 필요하다. [W19]
[OBSERVED] 리뷰 칭찬 Top 3: “flip back and forth between completed scenarios”, “better overarching story”, “trouble putting it down”으로 재연결·큰 서사·흡입력을 칭찬한다. [W17]
[OBSERVED] 리뷰 불만 Top 3: “minor word mix-up”, “forced … convoluted narrative”, 답을 이해하기보다 “formatting my answers”에 시간을 쓴다는 불만이다. [W18]
[INFERENCE] Unknown은 문장 맞히기보다 실제로 선택한 두 기록과 범위가 주장 카드에 남게 해야 한다.
### 2.4 The Roottrees Are Dead
[OBSERVED] 가격·포지셔닝: ₩21,000, 1998년풍 가상 인터넷의 기사·사진·책을 찾아 가족 관계를 확정하는 조사 퍼즐이다. [W21]
[OBSERVED] Core loop: 검색어 작성 → 문서 읽기 → 텍스트·얼굴을 노트와 가계도에 저장 → 관계 가설 입력 → 3명 단위 검증이다. [W24]
[OBSERVED] 초반 teaching: 세 자매라는 작은 닫힌 집합에서 검색, 기사 추출, 단체사진 대응, 출생순서 입력을 끝낸 뒤 큰 가계도를 연다. [W24]
[OBSERVED] Evidence UI: 노트의 출처를 누르면 원문으로, 원문의 하이라이트를 누르면 노트로 돌아가는 양방향 링크가 있다. [W24]
[OBSERVED] 부분 상태: Evidence Intuition은 미완성 인물 관련 단서가 현재 증거에 남았음을 알리지만 정답 확률은 말하지 않는다. [W24]
[OBSERVED] 오답·충분성: 3명을 정확히 식별하면 확정되고, 검증 전 입력은 수정 가능한 가설이다. [W24]
[OBSERVED] Provenance: 기사·책·정기간행물·사진이 분리되며 추출 텍스트와 원문 연결을 보존한다. [W24]
[OBSERVED] 접근성: CRT·점멸·전환 끄기와 일부 텍스트 크기 조절이 있으나 모든 영역 확대·스크린리더·컨트롤러에는 한계 보고가 있다. [W25]
[OBSERVED] 리뷰 칭찬 Top 3: “curated Wikipedia deep dive”, “breadcrumbs to follow”, “incredibly satisfying gameplay loop”로 정보 사냥·연결·확정 쾌감을 칭찬한다. [W22]
[OBSERVED] 리뷰 불만 Top 3: 후반이 “considerably harder”, “awkward digging through minutiae”, 중도 포기할 정도의 재검색 피로라는 불만이다. [W23]
[INFERENCE] Unknown에 가장 직접적인 차용점은 자유 메모보다 출처로 즉시 돌아가는 양방향 경로다.
### 2.5 Chants of Sennaar
[OBSERVED] 가격·포지셔닝: ₩22,800, 다섯 민족의 문자를 상황에서 해독하고 단절된 소통을 회복하는 퍼즐 어드벤처다. [W26]
[OBSERVED] Core loop: 행동·대화·표지의 문맥 관찰 → 문자에 임시 뜻 입력 → 다음 문맥에서 시험 → 그림 대응 페이지로 확정한다. [W30]
[OBSERVED] 초반 teaching: 문·레버의 열다/닫다를 행동으로 먼저 익힌 뒤 인사·나·너처럼 대화 의미로 범위를 넓힌다. [W30]
[OBSERVED] Evidence UI: 새 문자는 자동 수집되고 임시 뜻이 이후 대화 위에 표시되어 가설이 실제 읽기에 적용된다. [W30]
[OBSERVED] 부분 상태: 검증 페이지 개방은 필요한 노출을 봤다는 신호이지 의미를 이해했다는 보증이 아니다. [W31]
[OBSERVED] 오답·충분성: 임시 뜻은 자유 수정 가능하고 검증 오답에 벌점이 없으며, 정답 대응이 완료되면 공식 뜻으로 바뀐다. [W29]
[OBSERVED] Provenance: 대화·표지·벽화·책·행동이 근거지만 사전은 원래 문장·장소의 전역 원문 로그를 충분히 보존하지 않는다는 불만이 있다. [W29]
[OBSERVED] 접근성: 컨트롤러와 상호작용 대상 표시가 있으나 전체 글자 확대·화면낭독기 지원은 이번 조사에서 확인하지 못했다. [W30]
[OBSERVED] 리뷰 칭찬 Top 3: “love for my profession”, 해독의 연속적 아하, 문화·건축·문자의 결합을 칭찬한다. [W27]
[OBSERVED] 리뷰 불만 Top 3: “formally confirmed detracts”, “brute force”, “not having access to previous sentences”로 조기 확정·대입·원문 재방문 비용을 비판한다. [W29]
[INFERENCE] Unknown은 가설을 다음 기록 읽기에 적용해 보게 하되 공식 정답 문장으로 치환하지 않아야 한다.
### 2.6 Lorelei and the Laser Eyes
[OBSERVED] 가격·포지셔닝: ₩29,500, 오래된 호텔의 문서·숫자·공간 관계를 푸는 비선형 아트하우스 퍼즐 상자다. [W32]
[OBSERVED] Core loop: 문서와 공간에서 숫자·기호를 찾고 Photographic Memory를 재열람해 잠금·장치·메타 퍼즐을 푼다. [W35]
[OBSERVED] 초반 teaching: 자동차의 설명서·편지·지도와 밑줄 정보를 가까운 입구 잠금에 적용한 뒤 호텔의 다수 퍼즐로 확장한다. [W35]
[OBSERVED] Evidence UI: Photographic Memory는 문서, Mental Notes는 미해결 과제, Personal History는 완료 이력을 나눈다. [W35]
[OBSERVED] 부분 상태: 발견한 퍼즐이 지금 풀 수 있는지, 아직 자료가 부족한지 알려주는 일반 신호는 확인되지 않았다. [W35]
[OBSERVED] 오답·충분성: 일반 잠금은 재시도 가능하지만 일부 질문은 Game Over와 불러오기를 요구하며, 취소하려고 일부러 오답을 넣는 불만이 있다. [W34]
[OBSERVED] Provenance: 편지·책·포스터·게임 속 게임의 원형을 기억 메뉴에서 다시 읽을 수 있으나 가설 연결은 외부 메모에 많이 남는다. [W35]
[OBSERVED] 접근성: 한 버튼 접근 의도와 달리 뒤로가기 부재, 흰 문서의 눈부심, 색각 구분 문제에 대한 리뷰 마찰이 있다. [W33][W34]
[OBSERVED] 리뷰 칭찬 Top 3: “nails the intrigue, aesthetic and puzzle design”, 정보 연결의 강한 해결감, 비선형 서사의 여운이다. [W34]
[OBSERVED] 리뷰 불만 Top 3: “extremely user-unfriendly”, “no back button”, 지도·문서 접근의 반복 조작이다. [W34]
[INFERENCE] Unknown은 비선형 탐색을 늘리기보다 현재 비트 안에서 자료 부족과 추론 미완료를 구분하는 편이 우선이다.
### 2.7 Strange Horticulture
[OBSERVED] 가격·포지셔닝: ₩17,500, 식물 표본·도감·지도·고객 요청을 대조하는 책상형 서사 퍼즐이다. [W37]
[OBSERVED] Core loop: 고객 요청 읽기 → 표본의 모양·냄새·촉감과 도감 대조 → 잠정 라벨 → 식물 제공 또는 지도 탐색 → 결과 확정이다. [W40]
[OBSERVED] 초반 teaching: 첫날은 표본-도감 매칭에서 시작해 편지의 장소 단서를 지도 좌표로 바꾸는 문제로 매체를 확장한다. [W40]
[OBSERVED] Evidence UI: 도감, 표본, 확대 관찰, 고객 대사, 편지, 지도를 같은 작업대에 두고 색상 라벨로 잠정 식별한다. [W40]
[OBSERVED] 부분 상태: 성공적으로 사용한 식물은 식별 확정되고 도감·편지에 체크가 붙지만 사전 확신도 점수는 없다. [W40]
[OBSERVED] 오답·충분성: 잘못된 제출은 Rising Dread를 올리고 가득 차면 짧은 회복 퍼즐 뒤 중단 지점으로 돌아간다. [W40]
[OBSERVED] Provenance: 손님에게서 얻은 표본, 편지로 찾은 장소, 새 도감 페이지는 서로 다른 정보 획득 경로다. [W40]
[OBSERVED] 접근성: Simplified Text, 확대, 자동 라벨, 확대 중 키보드 이동이 있으나 좁은 화면에서 자료 누적과 작은 글씨 부담이 남는다. [W41]
[OBSERVED] 리뷰 칭찬 Top 3: “breath of fresh air”, “investigating the clues … satisfying”, 차분한 비·식물·고양이 분위기다. [W38]
[OBSERVED] 리뷰 불만 Top 3: “people come in and tell you what they want”, “almost no freewill”, 식별·정리 반복이 기대한 전문가 추론보다 얕다는 불만이다. [W39]
[INFERENCE] Unknown은 벌점 자원을 가져오지 말고, 잠정 표식과 검증 완료 표식의 분리만 가져오는 편이 캐논에 맞다.
### 2.8 Her Story
[OBSERVED] 가격·포지셔닝: ₩11,000, 1994년 경찰 인터뷰 데이터베이스를 검색해 사건을 재구성하는 비선형 FMV 서사다. [W42]
[OBSERVED] Core loop: 클립에서 단어 발견 → 검색어 입력 → 첫 5개 결과 시청 → 태그·세션 저장 → 새 검색어 도출이다. [W45]
[OBSERVED] 초반 teaching: 빈 검색창이 불투명해 `MURDER`를 미리 넣고 네 클립을 제공해 첫 검색 행동만 시동한다. [W45]
[OBSERVED] Evidence UI: Query History, User Tags, Add to Session, DB Checker가 검색·보관·시청 상태를 맡는다. [W46]
[OBSERVED] 부분 상태: DB Checker는 미시청/시청/최근 시청을 표시할 뿐 가설 정답이나 충분한 이해를 판정하지 않는다. [W46]
[OBSERVED] 오답·충분성: 틀린 검색은 0건 또는 이미 본 영상으로 돌아가며, 최종 답안 제출 없이 플레이어가 종료를 판단한다. [W46]
[OBSERVED] Provenance: 경찰 인터뷰라는 단일 아카이브 안에서 날짜·클립 맥락이 있고, 사용자 태그도 검색되므로 실제 발화와 플레이어 해석을 구분해야 한다. [W46]
[OBSERVED] 접근성: 자막과 화면 반사 설정은 있으나 글자 크기·스크린리더·색상 대체는 이번 조사에서 확인하지 못했다. [W46]
[OBSERVED] 리뷰 칭찬 Top 3: “incredibly well written”, 짧은 영상이 “take on a whole new meaning”, 연기의 “intonation, mannerisms”를 칭찬한다. [W43]
[OBSERVED] 리뷰 불만 Top 3: “no real conclusion”, 후반 “complete guesswork”, 핵심 정보를 “first 5 minutes”에 볼 수 있다는 불만이다. [W44]
[INFERENCE] Unknown은 빈 상태에서 자유 검색을 요구하지 말고 첫 유효 질문과 첫 비교쌍만 시동해야 한다.
### 2.9 Papers, Please
[OBSERVED] 가격·포지셔닝: ₩11,000, 입국 심사관이 문서와 진술을 규정에 대조해 허가·거절·체포를 결정한다. [W06]
[OBSERVED] Core loop: 규칙 확인 → 문서·외모·진술 비교 → 두 항목 연결로 불일치 증명 → 최종 도장 → 경고·세계 반응이다. [W09]
[OBSERVED] 초반 teaching: Day 1 여권, Day 2 유효기간·사진·발급 도시, Day 3 추가 서류처럼 하루마다 한 검증 차원을 더한다. [W09]
[OBSERVED] Evidence UI: 작업대·규칙책·당일 공문·진술 전사·검사 결과가 서로 다른 권위와 수명을 가진다. [W10]
[OBSERVED] 부분 상태: 두 항목의 일치·불일치는 확인하지만 여행객 전체의 적격성을 보증하지 않는다. [W10]
[OBSERVED] 오답·충분성: 최종 오판 뒤 사유가 적힌 경고장이 나오고 첫 두 건 이후에는 금전적 불이익이 생긴다. [W09]
[OBSERVED] Provenance: 공식 규정, 신청자 문서, 신청자 발언, 검사 결과를 분리해 문서 권위와 사실 진실성을 동일시하지 않게 한다. [W10]
[OBSERVED] 접근성: 한국어·대화 전사·Easy mode의 추가 수입이 있으나 독립 글자 확대·스크린리더 지원은 확인하지 못했다. [W06]
[OBSERVED] 리뷰 칭찬 Top 3: “many moral dilemmas”, “world reacts”, 작은 결정이 분위기를 “complete”한다는 평가다. [W07]
[OBSERVED] 리뷰 불만 Top 3: “too stressful and repetitive”, “too realistic”, “feels too much like doing work”라는 불만이다. [W08]
[INFERENCE] Unknown은 시간·경제 벌점을 가져올 수 없지만, 불충분한 제출의 이유를 근거 종류로 명확히 설명하는 방식은 가져올 수 있다.
### 2.10 Outer Wilds — ship-log / knowledge progression만
[OBSERVED] 가격·포지셔닝: ₩29,500, 지식 획득이 다음 행동을 여는 오픈월드 미스터리다. [W47]
[OBSERVED] Core loop: 질문을 품고 장소 탐색 → 현장 기록·대화 번역 → ship log 갱신 → 연결된 다른 장소에서 가설 시험이다. [W50]
[OBSERVED] 초반 teaching: 마을의 선택적 활동이 관찰 도구·이동·수리·번역을 작은 안전한 과제로 가르친다. [W50]
[OBSERVED] Evidence UI: Map Mode는 장소, Rumor Mode는 질문·관계, `?`는 미방문, 주황 `*`는 중요한 추가 정보 가능성을 표시한다. [W50]
[OBSERVED] 부분 상태: “There’s more to explore here”는 수집 미완료 신호이지 현재 가설이 틀렸다는 판정이 아니다. [W50]
[OBSERVED] 오답·충분성: 가설을 답안으로 채점하지 않고 세계에서 다음 관찰·실험으로 시험한다. [W50]
[OBSERVED] Provenance: 현장 원문을 장소별 행동 가능한 요약과 관계선으로 압축하며, 관계선을 선택하면 연결 이유를 읽을 수 있다. [W50]
[OBSERVED] 접근성: Steam 등록에는 글자 크기·카메라 편의·색상 대안·음량 분리가 있으나 판매자 등록과 독립 검증은 다르다. [W47]
[OBSERVED] 리뷰 칭찬 Top 3: “ship log records what you learn”, “that’s your progression”, “making hypotheses”와 다음 장소 선택의 자율성을 칭찬한다. [W48]
[OBSERVED] 리뷰 불만 Top 3: 조작이 “tear my hair out”, 재접근 비용, 초기 정보가 “disconnected and incomplete”하게 느껴진다는 불만이다. [W49]
[INFERENCE] Unknown은 타이머·죽음·반복을 가져오지 않고 질문 중심 관계망과 ‘추가 자료 있음’ 신호만 차용할 수 있다.
## S1. 비교표
| 게임 | teaching method | evidence / hypothesis UI | wrong-answer handling | provenance visibility | “enough evidence” signal |
|---|---|---|---|---|---|
| Obra Dinn | [OBSERVED] 첫 시체와 책으로 관찰→기입 반복 | [OBSERVED] 기억·명부·스케치·가설을 책에 결합 | [OBSERVED] 즉시 특정하지 않고 3개 정답 묶음 잠금 | [OBSERVED] 장·시체·대사 페이지 연결 | [OBSERVED] 얼굴 선명화는 노출, 잠금은 검증 |
| Case | [OBSERVED] 명백한 행위+좁은 이름·장소 추론 | [OBSERVED] Exploring/Thinking 분리, 구조화 문장 | [OBSERVED] 2칸 이하 오답 거리 | [OBSERVED] 장면 문맥은 남지만 단어함은 압축 | [OBSERVED] 단어 총수와 문장 검증을 분리 |
| Rise | [OBSERVED] 직접 증거→명단 대조→도구 출처 | [OBSERVED] 장면 위 이동식 퍼즐 창 | [OBSERVED] 편집 가능한 부분 답안 | [OBSERVED] TV 프레임·시점·문서가 혼합 | [OBSERVED] 근접 오답 신호, 완전한 자료 충분성은 불명 |
| Roottrees | [OBSERVED] 세 자매 미니 사건 뒤 큰 가계도 | [OBSERVED] 검색·노트·가계도와 양방향 원문 링크 | [OBSERVED] 3명 묶음 확정, 입력은 가설 | [OBSERVED] 문서 유형·얼굴·출처를 보존 | [OBSERVED] Evidence Intuition은 남은 단서만 알림 |
| Sennaar | [OBSERVED] 행동에서 뜻 경험 후 문자 정리 | [OBSERVED] 임시 뜻이 다음 문장 위에 적용 | [OBSERVED] 벌점 없는 재대입·공식 뜻 확정 | [OBSERVED] 매체는 다양하나 전역 원문 로그 약함 | [OBSERVED] 페이지 개방=노출 충분, 정답은 별도 |
| Lorelei | [OBSERVED] 가까운 문서 둘로 입구 잠금 해결 | [OBSERVED] Memory/Notes/History 분리 | [OBSERVED] 재시도 가능, 일부 고위험 시험 존재 | [OBSERVED] 문서 원형 재열람 | [OBSERVED] 일반적인 자료 충분성 신호가 약함 |
| Strange Horticulture | [OBSERVED] 표본→도감→지도 순으로 매체 확장 | [OBSERVED] 작업대의 표본·책·잠정 라벨 | [OBSERVED] 짧은 회복 비용, 영구 실패 없음 | [OBSERVED] 획득 경로·도감·표본 분리 | [OBSERVED] 행동 성공 뒤 식별 확정 |
| Her Story | [OBSERVED] `MURDER` 검색어로 첫 행동 시동 | [OBSERVED] 검색·태그·세션·시청 상태 | [OBSERVED] 0건/중복 결과, 추론 채점 없음 | [OBSERVED] 클립·날짜는 있으나 사용자 태그와 발화 혼합 | [OBSERVED] 플레이어가 종료 판단 |
| Papers Please | [OBSERVED] 하루마다 검사 차원 하나씩 추가 | [OBSERVED] 규칙책·문서·전사·두 항목 연결 | [OBSERVED] 사유 경고 뒤 경제 벌점 | [OBSERVED] 권위가 다른 문서 표면을 분리 | [OBSERVED] 국소 일치와 최종 적격성 분리 |
| Outer Wilds | [OBSERVED] 선택적 안전 과제로 도구 학습 | [OBSERVED] 장소 지도+질문 관계망 | [OBSERVED] 답안 채점 없이 다음 관찰로 시험 | [OBSERVED] 장소 요약과 관계 이유 표시 | [OBSERVED] `?`, `*`, 새 기록을 분리 |
[INFERENCE] 공통 우수 패턴 1은 첫 과제를 작게 닫고 다음 규칙을 하나씩 추가하는 것이다.
[INFERENCE] 공통 우수 패턴 2는 관찰, 플레이어 가설, 시스템 확정을 시각적으로 다른 상태로 두는 것이다.
[INFERENCE] 공통 우수 패턴 3은 “자료를 충분히 봄”과 “답이 맞음”을 같은 표시로 쓰지 않는 것이다.
[INFERENCE] 공통 우수 패턴 4는 자동 노트가 결론을 쓰지 않고 출처·재방문·다음 질문만 지원하는 것이다.
[INFERENCE] 반복 불만은 재방문 비용, 창·메뉴 관리, 조기 확정, 정답 문구 맞히기, 작은 글씨와 색 단독 신호다.
## S2. T0–C1 개발 권고
### D0-A. 한서린 플레이어 캐릭터 정체성 팩을 Higgsfield로 강화 — NEW · 제작 1순위
[TARGET] 화면·흐름: 시작 화면, 허브 대기, 판독 진입, 근거 고정, 미리보기, 저장 성공에 같은 한서린 얼굴·머리·앞치마·소매·허리 수첩을 쓰는 정체성 팩을 적용한다.
[TARGET] Higgsfield 산출물은 정면·3/4 상반신, 전신 고정 포즈, 회로·판독 손 클로즈업, 평상·집중·결심 표정, 5초 무음 동작 프리비즈로 구성한다.
[INFERENCE] 현재 M24 3D 한서린과 M25 초상이 별도 제작 계층이므로, 새 팩은 캐릭터를 교체하기보다 승인된 얼굴·실루엣·재질 기준을 하나로 묶는 참조 정본이어야 한다. [P01][P04]
[INFERENCE] Her Story의 연기·몸짓 재해석과 IMMORTALITY의 영화적 동작 설계는 캐릭터 표현이 추론의 분위기와 기억성을 높일 수 있음을 보여준다. [W43][W55]
[TARGET] QA 인수: 8개 정지 이미지와 3개 프리비즈에서 얼굴 비율·헤어 실루엣·앞치마·수첩·소매가 동일하고, 타 인물 얼굴·문자·로고·후반 NPC 노출이 0건이다.
[INFERENCE] 위험은 생성물의 얼굴 drift, 과도한 감정 연기로 정답 암시, 프리비즈를 gameplay로 오인하는 것이며 모든 원본은 `runtimeEligible:false`로 시작한다.
[TARGET] 시각 리소스: 한서린 identity sheet 1장, 표정 3장, 전신 2장, 회로 손 2장, 판독 손 2장, 5초 무음 모션 3종.
### D0-B. 현재 슬라이스의 입력·시험·확정·저장을 구분하는 모션·이펙트 문법 — NEW · 제작 2순위
[TARGET] 화면·흐름: circuit 추적 수락, reader 스타일러스 스윕, 기록 핀 고정, 미리보기 개방, 2단계 확인, 저장 성공, Undo 복귀를 서로 다른 짧은 모션으로 구분한다.
[TARGET] 효과는 정답이 아니라 `입력 수락 / 시험 결과 표시 / 세계 변경 대기 / 저장 완료`만 전달하며 성공 반짝임·정답 색·붉은 경고등을 쓰지 않는다.
[INFERENCE] M23 R7은 상태 구분을 이미 구현했고, Obra Dinn과 Lorelei 리뷰는 긴 전환·취소 비용이 추론보다 큰 마찰이 되는 위험을 보여준다. [P03][W03][W34]
[INFERENCE] Strange Horticulture의 작업대 촉감과 Papers Please의 문서 반응은 작은 물리 피드백이 절차의 무게를 만든다는 비교 근거다. [W07][W40]
[TARGET] QA 인수: 7개 사건이 1:1 모션 id를 갖고, 저장 실패에는 저장 성공 모션 0회, Undo에는 확정 모션 0회, 모션 축소 모드에서는 150ms 이하의 정적 전환으로 대체된다.
[INFERENCE] 위험은 모션이 실시간 타이머처럼 압박하거나 정답을 예고하는 것이며, 카메라 이동 없이 고정 노드 안의 손·도구·UI 레이어만 움직인다.
[TARGET] 시각 리소스: circuit 염선 펄스, reader 암 스윕, 황동 핀 settle, 미리보기 종이 들림, 저장 인장 압착, Undo 잔상 제거의 5초 무음 프리비즈와 시작·중간·끝 키프레임.
### D1. 첫 8분을 F2 요약이 아니라 세 개의 현장 마이크로 과제로 재배치 — NEW
[TARGET] 화면·흐름: T0-b1에서 문서 1개를 열고 `당직 인수 사실 1개`를 기록, T0-b2에서 회로 범위 안/밖을 한 번 비교, T0-b3에서 서로 다른 매체 두 기록을 고정하는 세 과제로 나눈다.
[TARGET] 전체 F2 안내는 언제든 열 수 있는 참조로 남기되, 첫 행동 전에 목표·규칙·조작·인물·구역을 한꺼번에 읽도록 요구하지 않는다.
[INFERENCE] Papers Please의 하루별 규칙 추가, Sennaar의 행동→뜻 정리, Outer Wilds의 선택적 안전 과제가 같은 방향을 지지한다. [W09][W30][W50]
[TARGET] QA 인수: 새 저장에서 F2를 열지 않아도 ① 첫 유효 행동 ② 배선 범위 변화 관찰 ③ 두 매체 고정까지 각각 한 화면의 지시와 한 행동으로 도달한다.
[TARGET] QA 인수: 각 과제는 새 규칙 명사 2개 이하를 도입하고 완료 뒤 다음 과제 한 줄만 연다.
[INFERENCE] 스포일러 위험은 낮지만, 자동으로 올바른 기록을 선택하면 답을 가르치는 위반이므로 후보는 플레이어가 고른다.
[TARGET] 시각 리소스: 작은 당직 인수 카드 받침, 회로 범위용 비문자 투명지 가장자리, 두 매체를 꽂는 낡은 황동 집게 2개.
### D2. 화면 상단에 ‘현재 조사 질문’ 한 장을 고정 — NEW
[TARGET] 화면·흐름: 일반 목표 대신 `어느 기록이 같은 시간대를 말하는가?`처럼 답이 아닌 질문을 한 문장으로 표시하고, 질문 아래에는 `확인한 사실 / 아직 필요한 매체` 두 줄만 둔다.
[INFERENCE] Outer Wilds는 장소보다 질문을 따라 관계를 이동시키고, Her Story는 미리 채운 첫 검색어로 빈 상태의 불투명함을 줄인다. [W45][W50][W51]
[TARGET] T0 질문은 인물 이름 없이 기록과 시간 범위만 사용하고, C1 진입 뒤에만 문재화를 질문 카드에 허용한다.
[TARGET] QA 인수: T0 3비트와 C1-b1/b2 각각에 질문 카드가 하나만 존재하고, 카드 본문에 정답 쌍·가해자·의도 단어가 0개다.
[TARGET] QA 인수: 증거 화면·가설판·판독 화면에서 동일 질문 id와 문구가 유지된다.
[INFERENCE] 위험은 질문 자체가 힌트가 되는 것이며, 질문은 `무엇을 비교할까`까지만 말하고 `어느 것이 맞나`를 좁히지 않는다.
[TARGET] 시각 리소스: 기록 종이 회백 카드, 상단을 고정하는 부식 청동 클립, 질문 종류를 나타내는 비문자 기호 3종.
### D3. 가설판에 ‘판정’ 대신 네 단계 구조 상태를 표시 — NEW
[TARGET] 화면·흐름: 주장 카드 상태를 `미검토 / 한 매체 근거 / 서로 다른 매체 2종 / 반례 고정`으로 표시하고 참·거짓 확률이나 정답 근접도는 표시하지 않는다.
[INFERENCE] Obra Dinn의 손글씨→인쇄체, Roottrees의 남은 단서 신호, Sennaar의 임시 뜻→공식 뜻이 관찰·가설·검증 분리의 장점을 보인다. [W04][W24][W30]
[TARGET] `서로 다른 매체 2종`은 제출 형식 준비도일 뿐 주장이 참이라는 뜻이 아니며, 카드에 이 문구를 항상 병기한다.
[TARGET] QA 인수: 같은 원본의 사본 2개는 한 매체 상태를 넘지 못하고, 다른 매체 2종이 있어도 반례가 고정되면 `제출 준비`로 바뀌지 않는다.
[TARGET] QA 인수: 네 상태는 색을 제거한 1비트 캡처에서도 외곽 형태로 구분된다.
[INFERENCE] 자동 반례 판정은 답을 말할 위험이 있으므로 플레이어가 `반례로 고정`한 기록만 상태에 반영한다.
[TARGET] 시각 리소스: 열린 고리, 1개 홈, 이중 프레임, 사선이 아닌 분리 홈 형태의 1비트 상태 글리프 4종.
### D4. 모든 인용 카드의 출처 왕복과 매체 실루엣을 일관 배치 — ALREADY-COVERED-BY-R1–R3
[OBSERVED] M23은 두 기록 동시 비교, 원본·사본 계보, 같은 원본·같은 매체 거부를 이미 구현했다. [P03]
[TARGET] 새 시스템을 만들지 말고 증거함·가설판·미리보기·확정 화면의 같은 위치에 `염판 / 당직일지 / 조위대장` 실루엣과 원문 돌아가기 버튼을 유지한다.
[INFERENCE] Roottrees의 양방향 링크와 Papers Please의 문서 권위 분리가 이 일관성의 가치를 지지한다. [W10][W24]
[TARGET] QA 인수: 인용 카드에서 원문까지 키보드 3입력 이하, 원문에서 같은 카드 복귀 3입력 이하, 초점 손실 0건이다.
[TARGET] QA 인수: 세 매체는 색·텍스트 없이 실루엣만으로 구분되고 같은 root의 사본에는 동일 계보 표식이 붙는다.
[INFERENCE] 위험은 새 아이콘이 기존 §6 캐논과 충돌하는 것이므로 외곽 형상은 style guide를 그대로 사용한다. [P04]
[TARGET] 시각 리소스: 육각 염판, 둥근 실제본 당직일지, 세로 장부 등 3종; 기존 자산이 있으면 재생성하지 않는다.
### D5. ‘새 기록’ 알림을 결론이 아닌 다음 질문으로 연결 — NEW
[TARGET] 화면·흐름: 판독이나 저장 성공 뒤 `새 기록 1건`을 표시하고, 열면 원문 요약 1줄·출처·연결된 현재 질문만 보여준다.
[INFERENCE] Outer Wilds의 새 로그·관계선과 Rise의 완료 사건 재연결은 과거 자료가 다음 질문을 만드는 흐름을 보여준다. [W17][W50]
[TARGET] 자동 로그는 인물의 의도·범인·인과를 쓰지 않고 플레이어가 실제로 읽은 사실만 기록한다.
[TARGET] QA 인수: T0-b3 저장 성공 전에는 새 기록이 0건이고, 성공 뒤 정확히 1건이며, 저장 실패·취소·Undo 뒤에는 중복 알림 0건이다.
[TARGET] QA 인수: 알림에서 원문과 현재 질문으로 각각 2입력 이하에 이동한다.
[INFERENCE] 위험은 요약이 정답을 굳히는 것이며, 문장은 원문 필드와 시간 범위만 재진술한다.
[TARGET] 시각 리소스: 황토 악센트 8% 이하의 작은 금속 탭, 젖은 종이 가장자리, 텍스트 없는 새 기록 모서리 표식.
### D6. 판독 화면에 수동 ‘대조 레일’을 두고 관계를 플레이어가 붙이게 함 — ALREADY-COVERED-BY-R1–R2, 확장만
[OBSERVED] M23은 현재 기록과 고정 기록을 동시에 표시하고 플레이어가 근거를 직접 선택하게 했다. [P03]
[TARGET] 두 차트 아래에 `일치 / 충돌 / 아직 모름` 세 관계 표식을 두되, 시스템이 자동 선택하지 않고 플레이어가 선택한 상태만 가설판에 반영한다.
[INFERENCE] Sennaar의 임시 뜻 적용과 Golden Idol의 구조화 가설은 사고를 외부화하지만, 정답 문구 대입은 피해야 한다. [W14][W19][W30]
[TARGET] QA 인수: 같은 두 기록으로 세 관계를 자유롭게 바꿀 수 있고, 어느 선택도 세계 상태를 바꾸지 않으며 Undo 없이도 편집 가능하다.
[TARGET] QA 인수: 확정 미리보기는 선택한 관계·두 출처·시간 범위를 보여주지만 정답 여부를 말하지 않는다.
[INFERENCE] 위험은 일치/충돌이 새 동사처럼 보이는 것이므로 `reader` 안의 노트 조작으로만 표현하고 도구 휠에는 추가하지 않는다.
[TARGET] 시각 리소스: 평행 브래킷, 갈라진 브래킷, 열린 브래킷의 비문자 관계 마커 3종과 청회 금속 레일.
### D7. 제출 실패를 ‘형식 부족 / 출처 중복 / 반례 미해결 / 저장 실패’로 분리 — ALREADY-COVERED-BY-R2–R3–R7
[OBSERVED] 현재 구현은 같은 원본·같은 매체·미기록 후보를 거부하고 수락·작업 저장·확정 저장·실패를 구별한다. [P03]
[TARGET] 실패 메시지는 정답을 암시하지 않고 어떤 계약 축이 충족되지 않았는지만 네 범주 중 하나로 표시한다.
[INFERENCE] Papers Please의 불일치 사유와 Golden Idol의 거친 근접 피드백은 막막함을 줄이지만, 특정 정답 칸 공개는 대입을 부른다. [W10][W14]
[TARGET] QA 인수: 네 실패 fixture가 각각 정확히 한 범주만 표시하고, 올바른 기록 id·답 문구·새 인물 이름을 메시지에 포함하지 않는다.
[TARGET] QA 인수: 실패 뒤 후보·스크롤·핀·작업 저장 상태가 보존되고, 무제한 Undo와 재시도가 가능하다.
[INFERENCE] 위험은 ‘반례 미해결’이 자동 의미 판정으로 변하는 것이므로 플레이어가 직접 반례로 고정한 기록만 검사한다.
[TARGET] 시각 리소스: 신규 생성 불필요; 기존 저장 영역과 상태 글리프를 재사용한다.
### D8. 가설판을 장소 목록이 아니라 질문–기록 관계 보기로 전환할 수 있게 함 — NEW
[TARGET] 화면·흐름: 기본 목록은 유지하고 보조 보기에서 현재 질문을 중심에, 읽은 기록을 주변에, 연결 이유를 `언급 / 같은 시간창 / 같은 계보 / 플레이어 고정`으로 표시한다.
[INFERENCE] Outer Wilds Rumor Mode와 Roottrees 원문 링크는 정보량보다 관계와 재방문 이유를 드러낸다. [W24][W50][W51]
[TARGET] 자동으로 생성되는 연결은 메타데이터 관계만 허용하고 인과·범인·의도 연결은 플레이어 고정만 허용한다.
[TARGET] QA 인수: T0–C1-b2의 공개 자료만 노드가 되고 오은정·표성찬·후반 장소·후반 도구 노드는 0개다.
[TARGET] QA 인수: 모든 연결선은 선택 시 출처 2개와 관계 이유를 표시하고 원문으로 왕복할 수 있다.
[INFERENCE] 위험은 그래프가 답 지도처럼 보이는 것이며, 방향 화살표·정답 색·중앙 결론 노드를 금지한다.
[TARGET] 시각 리소스: 젖은 금속 핀보드 표면, 종이 탭 5종, 무방향 얇은 실선과 황동 핀; 텍스트는 Unity UI로만 올린다.
### D9. 150% 읽기 모드에서 문서와 비교를 동시에 유지 — NEW
[TARGET] 화면·흐름: 150%에서 본문 확대 시 비교 대상 한쪽을 숨기지 않고, 두 기록의 제목·단위·시간창·매체 실루엣을 고정 헤더에 유지한다.
[INFERENCE] Obra Dinn·Lorelei·Roottrees 리뷰는 기억 부담, 메뉴 왕복, 작은 텍스트가 추론보다 큰 비용이 되는 위험을 보여준다. [W03][W25][W34]
[TARGET] 문서 흰색은 `#E7E3D8`을 넘지 않고, 배경은 `#0E1F26/#173238`, 본문 대비는 최소 4.5:1, 비텍스트 경계는 최소 3:1을 목표로 한다.
[TARGET] QA 인수: 16:9와 4:3, 100%와 150%의 판독·증거함·가설판·미리보기 4화면에서 텍스트 클리핑·겹침·스크롤 도달 불가가 0건이다.
[TARGET] QA 인수: 색을 제거한 캡처에서도 매체·상태·초점이 형태와 위치로 구분된다.
[INFERENCE] 위험은 화면을 넓히려고 카메라 줌·자유 이동을 넣는 것이며, 고정 2.5D 노드와 UI 재배치만 사용한다.
[TARGET] 시각 리소스: 신규 삽화보다 중립 종이·금속 9-slice 표면 2종만 필요하며, 장식 디테일은 최소화한다.
### D10. T0 종료에 ‘확정한 것 / 보류한 것 / 다음 질문’ 3칸 영수증 제공 — NEW
[TARGET] 화면·흐름: 저장 성공 뒤 한 화면에서 확정된 절차 사실, 플레이어가 보류한 관계, C1-b1에서 이어질 질문을 각각 한 줄로 보여준다.
[INFERENCE] Outer Wilds의 행동 가능한 로그, Golden Idol의 사건 단위 닫힘, Her Story의 완료감 부족 불만이 작은 결론과 열린 질문의 동시 제공을 지지한다. [W14][W44][W50]
[TARGET] `확정한 것`은 저장된 절차·인용만, `보류한 것`은 플레이어가 `아직 모름`으로 둔 관계만, `다음 질문`은 새 이름 없이 기록 범위만 쓴다.
[TARGET] QA 인수: T0-b3 저장 성공에서 한 번만 표시되고 재시작 후 검토 노트에서 동일 내용으로 다시 열 수 있다.
[TARGET] QA 인수: Undo로 T0-b2로 돌아가면 영수증이 확정 상태로 남지 않고, Redo만으로 자동 재확정되지 않는다.
[INFERENCE] 위험은 C1 답을 예고하는 것이며, 새 고유명·의도·후반 장소를 금지한다.
[TARGET] 시각 리소스: 세 칸짜리 낡은 이관 봉투, 겹친 사각 2개를 재사용한 저장 인장 자리, 빈 보류 슬롯.
## S3. Higgsfield 이미지 생성용 리소스 쇼핑 리스트
[OBSERVED] 팔레트는 `#0E1F26 #173238 #36565C #4F7A6B #8A8E88 #C8D6D3 #E7E3D8 #E2AF62`, 예비 산화 적갈 `#8C4A3A` 2% 이하이며 이미지 내 텍스트·숫자·로고·네온은 금지다. [P04]
[OBSERVED] 모든 항목은 한국의 낡은 항구 당직실·젖은 금속·소금·종이·수평 수위선의 시각 언어를 따른다. [P04]
| item id | 목적 | 권고 | aspect ratio | 한 문장 visual brief |
|---|---|---|---|---|
| HIG-M26-00A | 한서린 identity sheet | D0-A | 16:9 | [TARGET] 동일 얼굴·낡은 작업앞치마·걷은 소매·허리 수첩의 정면/3-4/전신 포즈 시트, 한국 항구 복원사, no text/logos/neon |
| HIG-M26-00B | 한서린 도구 손 클로즈업 | D0-A | 3:2 pair | [TARGET] 회로 탐침을 잡는 손과 원형 판독기 스타일러스를 누르는 손, 종이 회백 소매와 염 탈색, no text/numbers/logos |
| HIG-M26-00C | 한서린 5초 무음 모션 팩 | D0-A | 16:9 video | [TARGET] 고정 카메라에서 기록 확인·집중 호흡·결심 후 손을 내리는 세 동작, 과장 표정과 입 모양 대사 없이, no camera move |
| HIG-M26-00D | 도구·저장 이펙트 프리비즈 | D0-B | 16:9 video | [TARGET] 염선 펄스·판독 암 스윕·황동 핀 고정·종이 미리보기·인장 저장·Undo 잔상 제거, no neon/red alert/text/logos |
| HIG-M26-01 | 당직 인수 카드 받침 | D1 | 4:3 | [TARGET] 젖은 금속 책상 위 회백 종이 카드와 부식 청동 클립, 팔레트 8색만, 한국 항구 행정실, no text/logos/neon |
| HIG-M26-02 | 회로 투명지 가장자리 | D1 | 16:9 | [TARGET] 접이식 황동 격자 투명지의 테두리와 염화 고리, 중앙은 UI 투과용 빈 영역, no text/numbers/logos/neon |
| HIG-M26-03 | 현재 조사 질문 카드 | D2 | 3:1 | [TARGET] 낮은 수평 비율의 기록 종이 탭과 한 개의 녹청 클립, 빗물 자국과 소금 가장자리, no text/logos/neon |
| HIG-M26-04 | 가설 상태 글리프 시트 | D3 | 1:1 spritesheet | [TARGET] 열린 고리·한 홈·이중 프레임·분리 홈 4개를 1비트 실루엣으로, 확정 잉크와 종이 회백, no letters/numbers |
| HIG-M26-05 | 기록 매체 실루엣 3종 | D4 | 3:1 spritesheet | [TARGET] 육각 소금판, 둥근 모서리 당직일지, 세로 긴 조위대장 등만 정면 아이콘으로, no text/logos/neon |
| HIG-M26-06 | 새 기록 모서리 탭 | D5 | 1:1 | [TARGET] 황토 악센트 8% 이하의 작은 금속 탭과 젖은 종이 모서리, 붉은 경고등 없이 조용한 주목성, no text |
| HIG-M26-07 | 대조 관계 마커 | D6 | 3:1 spritesheet | [TARGET] 평행 브래킷·갈라진 브래킷·열린 브래킷 3종, 청회 금속과 백청 염화 흔적, no arrows/text/logos |
| HIG-M26-08 | 질문–기록 핀보드 표면 | D8 | 16:9 | [TARGET] 낮은 유리창 아래 젖은 청회 금속 보드와 황동 핀, 수평 수위선 40% 대역, 중앙 장식 최소, no text/logos/neon |
| HIG-M26-09 | 종이·금속 9-slice 표면 | D9 | 2:1 pair | [TARGET] 회백 종이 패널과 심해 잉크 금속 패널 한 쌍, 모서리 마모와 염화 고리, 균일 중간 회색 금지, no text |
| HIG-M26-10 | T0 이관 영수증 봉투 | D10 | 4:3 | [TARGET] 세 칸 구조의 낡은 이관 봉투와 겹친 사각 인장 자리, 한국 항구 관공서 정물, no text/numbers/logos/neon |
[INFERENCE] HIG-M26-04·05·07은 이미지 생성보다 벡터 저작이 더 정확할 수 있으며, 생성본은 컨셉 참조로만 쓰고 최종 글리프는 1비트 실루엣 검증을 거쳐야 한다.
[TARGET] 모든 생성물은 `runtimeEligible:false`로 시작하고, 상업 사용권 확인 전 개발 프로필 밖으로 승격하지 않는다.
[TARGET] 기존 M25 매체·도구 자산이 같은 용도를 충족하면 새로 생성하지 않고 재사용한다.
## S4. 자동화 또는 단일 리뷰어로 가능한 작은 A/B 확인
### A/B-1. F2 선행형 A vs 현장 마이크로 과제형 B
[TARGET] 표본: 동일 빌드의 격리 저장 2개, 단일 리뷰어가 공략 없이 각 조건을 한 번 수행하며 순서는 B→A가 아니라 별도 새 저장으로 교차한다.
[TARGET] 측정: 첫 유효 행동까지 화면 전환 수, 첫 비교까지 입력 수, F2 강제 열람 여부, 잘못된 화면 진입 수.
[TARGET] PASS: B는 첫 유효 행동 ≤3 화면 전환, 첫 비교 ≤12 입력, 잘못된 화면 진입 0, A보다 총 입력 20% 이상 감소다.
[TARGET] FAIL: B가 기록 후보를 자동 선택하거나 질문 카드가 정답 매체명을 공개하면 수치와 무관하게 실패다.
### A/B-2. 목록형 가설판 A vs 질문–기록 관계 보기 B
[TARGET] 자동 fixture: T0-b3의 허용된 기록 5개와 관계 4개를 주고 `같은 시간창`, `같은 root`, `다른 매체`, `플레이어 고정` 경로를 검사한다.
[TARGET] 측정: 현재 질문에서 두 원문을 왕복하는 최소 키 입력, 잘못된 후반 노드 노출 수, 연결 이유 누락 수.
[TARGET] PASS: B의 각 원문 왕복 ≤3입력, 후반·비공개 노드 0, 이유 없는 연결 0, A보다 왕복 입력 중앙값 25% 이상 감소다.
[TARGET] FAIL: 방향 화살표나 색으로 인과·정답을 자동 암시하면 실패다.
### A/B-3. 단일 완료 표시 A vs 구조 상태 4단계 B
[TARGET] 자동 fixture: 같은 원본 사본 2개, 같은 매체 다른 root, 다른 매체 2종, 다른 매체 2종+플레이어 고정 반례의 네 상태를 재생한다.
[TARGET] 측정: 표시 상태 정확도, 제출 버튼 활성 조건, 저장 전후·Undo/Redo 상태 일치, 150% 레이아웃 충돌 수.
[TARGET] PASS: 4/4 상태 정확, 잘못된 제출 활성 0, 저장 실패 후 세계 변경 0, Undo/Redo 불일치 0, 100%·150% 충돌 0이다.
[TARGET] FAIL: `다른 매체 2종`을 참으로 표시하거나 반례를 시스템이 자동 판정하면 실패다.
## 6. 채택 우선순위
[TARGET] 제작 1순위는 D0-A 한서린 정체성 팩이며, M24 3D 캐릭터·M25 초상·손 연출이 같은 얼굴과 실루엣을 공유하게 한다.
[TARGET] 제작 2순위는 D0-B 도구·저장 모션 문법이며, 현재 열린 circuit·reader와 핀·미리보기·확정·저장·Undo만 강화한다.
[TARGET] 제작 3순위는 D1 현장 마이크로 과제와 D2 현재 조사 질문이며, 새 연출을 첫 10분의 이해와 연결한다.
[TARGET] 다음은 D3 구조 상태와 D10 T0 영수증, 그 뒤 D5 새 기록과 D8 질문 관계 보기다.
[TARGET] D4·D6·D7은 새 기능으로 재구현하지 않고 M23 구현의 일관성과 회귀만 강화한다.
[TARGET] D9는 모든 위 변경의 출하 조건이며 생성 리소스의 화려함보다 가독성·모션 축소·누설 0을 먼저 검증한다.
[INFERENCE] 사람 플레이테스트가 없으므로 재미·몰입·실제 이해 개선을 주장하지 않고, 이번 사이클에서는 조작 수·상태 정확도·누설 0·레이아웃 0결함만 검증한다.
[INFERENCE] 후반 캠페인·새 NPC·새 장소·새 도구 생산 없이도 10개 권고 모두 T0–C1-b2에서 구현·검사 가능하다.
## 7. 하지 않을 것
[TARGET] Obra Dinn의 3개 잠금, Golden Idol의 2칸 이하 오답, Strange Horticulture의 벌점 자원, Papers Please의 타이머·벌금은 복제하지 않는다.
[TARGET] Outer Wilds의 시간 루프·죽음·이동 비용, Lorelei의 취소 불가·Game Over, Her Story의 무제한 자유 검색을 도입하지 않는다.
[TARGET] 정답 추론 AI, NPC의 정답 대행, 자동 인과선, 자동 올바른 증거 선택, 자유서술 정답 채점은 도입하지 않는다.
[TARGET] 가설판의 새 관계 조작은 `reader`의 기록 정리이며 일곱 번째 도구·동사로 취급하지 않는다.
[TARGET] 오은정·표성찬·후반 장·후반 장소·후반 사건 이미지는 이번 범위에서 생성하거나 노출하지 않는다.
[TARGET] 이미지 안에 텍스트·숫자·로고·간판·네온·붉은 경고등을 넣지 않는다.
## 8. 프로젝트 근거
[OBSERVED] [P01] `file:///Users/jangyoung/orca/unknown/README.md` — 열람 2026-09-19, M25 플레이 가능 범위·가이드·접근성·도구·저장 계약.
[OBSERVED] [P02] `file:///Users/jangyoung/orca/unknown/_workspace/current/planning/aside-core-loop-research-20260913.md` — 열람 2026-09-19, 이전 R1–R10·외부 근거·Base gate.
[OBSERVED] [P03] `file:///Users/jangyoung/orca/unknown/_workspace/current/handoff/m23-results-and-improvement-plan.md` — 열람 2026-09-19, R1/R2/R3/R5/R7 구현 및 R6/R8/R9/R10 차단.
[OBSERVED] [P04] `file:///Users/jangyoung/orca/unknown/_workspace/current/concept/style-guide.md` — 열람 2026-09-19, 팔레트·매체 실루엣·도구 글리프·금지 목록.
## 9. 웹 출처
[OBSERVED] [W01] Steam, Return of the Obra Dinn store, https://store.steampowered.com/app/653530/Return_of_the_Obra_Dinn/ — 열람 2026-09-19.
[OBSERVED] [W02] Steam review, Obra Dinn positive, https://steamcommunity.com/profiles/76561198025676063/recommended/653530/ — 열람 2026-09-19.
[OBSERVED] [W03] Steam reviews, Obra Dinn negative, https://steamcommunity.com/profiles/76561197997711695/recommended/653530/ and https://steamcommunity.com/profiles/76561197984586984/recommended/653530/ — 열람 2026-09-19.
[OBSERVED] [W04] Game Developer, Road to the IGF: Lucas Pope's Return of the Obra Dinn, https://www.gamedeveloper.com/business/road-to-the-igf-lucas-pope-s-i-return-of-the-obra-dinn-i- — 열람 2026-09-19.
[OBSERVED] [W05] 3909 official support, motion sickness settings, https://3909.zendesk.com/hc/en-us/articles/360021736134-Getting-Motion-Sickness — 열람 2026-09-19.
[OBSERVED] [W06] Steam, Papers Please store, https://store.steampowered.com/app/239030/Papers_Please/ — 열람 2026-09-19.
[OBSERVED] [W07] Steam review, Papers Please positive, https://steamcommunity.com/profiles/76561198035742939/recommended/239030/ — 열람 2026-09-19.
[OBSERVED] [W08] Steam reviews, Papers Please negative, https://steamcommunity.com/profiles/76561198106881785/recommended/239030/ and https://steamcommunity.com/profiles/76561198098053655/recommended/239030/ — 열람 2026-09-19.
[OBSERVED] [W09] Game Developer, Road to the IGF: Papers Please, https://www.gamedeveloper.com/design/road-to-the-igf-lucas-pope-s-i-papers-please-i- — 열람 2026-09-19.
[OBSERVED] [W10] Lucas Pope devlog, Cramming Papers Please Onto Phones, https://dukope.com/devlogs/papers-please/mobile/ — 열람 2026-09-19.
[OBSERVED] [W11] Steam, The Case of the Golden Idol store, https://store.steampowered.com/app/1677770/The_Case_of_the_Golden_Idol/ — 열람 2026-09-19.
[OBSERVED] [W12] Steam review, Case positive, https://steamcommunity.com/profiles/76561198041983779/recommended/1677770/ — 열람 2026-09-19.
[OBSERVED] [W13] Steam review, Case negative, https://steamcommunity.com/profiles/76561198047056354/recommended/1677770/ — 열람 2026-09-19.
[OBSERVED] [W14] Game Developer, frequent testing and Thought Path, https://www.gamedeveloper.com/business/-the-case-of-the-golden-idol-i-used-frequent-testing-to-improve-its-mystery-solving — 열람 2026-09-19.
[OBSERVED] [W15] Official Steam news, Golden Idol Redux, https://store.steampowered.com/news/app/1677770/view/4677641741933164088 — 열람 2026-09-19.
[OBSERVED] [W16] Steam, The Rise of the Golden Idol store, https://store.steampowered.com/app/2716400/The_Rise_of_the_Golden_Idol/ — 열람 2026-09-19.
[OBSERVED] [W17] Steam review, Rise positive, https://steamcommunity.com/id/substrate-level_phosphorylation/recommended/2716400/ — 열람 2026-09-19.
[OBSERVED] [W18] Steam review, Rise negative, https://steamcommunity.com/profiles/76561198155737608/recommended/2716400/ — 열람 2026-09-19.
[OBSERVED] [W19] FOLD, Becoming a Detective interview, https://www.fold.lv/en/becoming-a-detective-interview-with-andrejs-and-ernests-klavins/ — 열람 2026-09-19.
[OBSERVED] [W20] The Verge, Rise review and hint behavior, https://www.theverge.com/24295956/the-rise-of-the-golden-idol-review-pc-playstation-xbox-netflix-mobile — 열람 2026-09-19.
[OBSERVED] [W21] Steam, The Roottrees Are Dead store, https://store.steampowered.com/app/2754380/The_Roottrees_are_Dead/ — 열람 2026-09-19.
[OBSERVED] [W22] Steam review, Roottrees positive, https://steamcommunity.com/profiles/76561198022719848/recommended/2754380/ — 열람 2026-09-19.
[OBSERVED] [W23] Steam review, Roottrees negative, https://steamcommunity.com/profiles/76561198024505662/recommended/2754380/ — 열람 2026-09-19.
[OBSERVED] [W24] Robin Ward developer blog, Building The Roottrees are Dead, https://eviltrout.com/blog/2025-01-27-building-the-roottrees/ — 열람 2026-09-19.
[OBSERVED] [W25] Can I Play That, Roottrees accessibility review, https://caniplaythat.com/2025/12/29/indie-spotlight-the-roottrees-are-dead/ — 열람 2026-09-19.
[OBSERVED] [W26] Steam, Chants of Sennaar store, https://store.steampowered.com/app/1931770/Chants_of_Sennaar/ — 열람 2026-09-19.
[OBSERVED] [W27] Steam review, Chants positive, https://steamcommunity.com/profiles/76561198288222329/recommended/1931770/ — 열람 2026-09-19.
[OBSERVED] [W28] Steam review, Chants most-helpful negative EULA snapshot, https://steamcommunity.com/profiles/76561199123838755/recommended/1931770/ — 열람 2026-09-19; 현재 수집 사실로 일반화하지 않음.
[OBSERVED] [W29] Steam review, Chants design-focused negative, https://steamcommunity.com/profiles/76561197964155376/recommended/1931770/ — 열람 2026-09-19.
[OBSERVED] [W30] Game Developer, Chants of Sennaar design interview, https://www.gamedeveloper.com/design/immersing-players-in-the-culture-of-a-people-with-language-puzzler-chants-of-sennaar — 열람 2026-09-19.
[OBSERVED] [W31] Developer Steam discussion, missed glyph/page guidance, https://steamcommunity.com/app/1931770/discussions/0/4554911223882689093 — 열람 2026-09-19.
[OBSERVED] [W32] Steam, Lorelei and the Laser Eyes store, https://store.steampowered.com/app/2008920/ — 열람 2026-09-19.
[OBSERVED] [W33] Steam review, Lorelei positive/accessibility request, https://steamcommunity.com/profiles/76561198273160352/recommended/2008920/ — 열람 2026-09-19.
[OBSERVED] [W34] Steam review, Lorelei negative/control details, https://steamcommunity.com/profiles/76561198034026303/recommended/2008920/ — 열람 2026-09-19.
[OBSERVED] [W35] Screen Rant, Simon Flesser interview, https://screenrant.com/lorelei-laser-eyes-interview-challenge-puzzle-simon-flesser/ — 열람 2026-09-19.
[OBSERVED] [W36] TheGamer, Lorelei beginner guide, https://www.thegamer.com/lorelei-and-the-laser-eyes-beginner-tips/ — 열람 2026-09-19.
[OBSERVED] [W37] Steam, Strange Horticulture store, https://store.steampowered.com/app/1574580/Strange_Horticulture/ — 열람 2026-09-19.
[OBSERVED] [W38] Steam review, Strange Horticulture positive, https://steamcommunity.com/profiles/76561197970876965/recommended/1574580/ — 열람 2026-09-19.
[OBSERVED] [W39] Steam review, Strange Horticulture negative, https://steamcommunity.com/profiles/76561198007307152/recommended/1574580/ — 열람 2026-09-19.
[OBSERVED] [W40] Game Developer, Finding the right plant for the job, https://www.gamedeveloper.com/design/finding-right-plant-job-strange-horticulture — 열람 2026-09-19.
[OBSERVED] [W41] IGN, Strange Horticulture Day 1 and tips, https://www.ign.com/wikis/strange-horticulture/Day_1 and https://www.ign.com/wikis/strange-horticulture/Tips_and_Tricks — 열람 2026-09-19.
[OBSERVED] [W42] Steam, Her Story store, https://store.steampowered.com/app/368370/Her_Story/ — 열람 2026-09-19.
[OBSERVED] [W43] Steam review, Her Story positive, https://steamcommunity.com/profiles/76561198049641617/recommended/368370/ — 열람 2026-09-19.
[OBSERVED] [W44] Steam review, Her Story negative, https://steamcommunity.com/profiles/76561198115719777/recommended/368370/ — 열람 2026-09-19.
[OBSERVED] [W45] PC Gamer, Sam Barlow on Her Story's fifth birthday, https://www.pcgamer.com/on-her-storys-5th-birthday-sam-barlow-looks-back-at-his-breakout-gameand-talks-about-whats-next — 열람 2026-09-19.
[OBSERVED] [W46] GameFAQs, Her Story UI and Readme transcription, https://gamefaqs.gamespot.com/pc/162562-her-story/faqs/71925 — 열람 2026-09-19.
[OBSERVED] [W47] Steam, Outer Wilds store, https://store.steampowered.com/app/753640/Outer_Wilds/ — 열람 2026-09-19.
[OBSERVED] [W48] Steam review, Outer Wilds positive, https://steamcommunity.com/profiles/76561198021427479/recommended/753640/ — 열람 2026-09-19.
[OBSERVED] [W49] Steam reviews, Outer Wilds negative, https://steamcommunity.com/profiles/76561198141696432/recommended/753640/ and https://steamcommunity.com/profiles/76561198104439692/recommended/753640/ — 열람 2026-09-19.
[OBSERVED] [W50] GDC YouTube, Sparking Curiosity-Driven Exploration Through Narrative in Outer Wilds, https://www.youtube.com/watch?v=QaGu9tGCNbI — 영문 자막 열람 2026-09-19.
[OBSERVED] [W51] Alex Beachum interview, question-led ship log, https://medium.com/@cordialkobold/interview-with-alex-beachum-creative-director-of-outer-wilds-a01bb9631e20 — 열람 2026-09-19.
[OBSERVED] [W52] Steam, Pentiment store, https://store.steampowered.com/app/1205520/Pentiment/ — 열람 2026-09-19.
[OBSERVED] [W53] Josh Sawyer interview, uncertainty and no canonical killer, https://www.thegamer.com/interview-obsidian-josh-sawyer-pentiment/ — 열람 2026-09-19.
[OBSERVED] [W54] Steam, IMMORTALITY store, https://store.steampowered.com/app/1350200/IMMORTALITY/ — 열람 2026-09-19.
[OBSERVED] [W55] GDC YouTube, An IMMORTALITY Project: Creating the Ultimate Interactive Movie, https://www.youtube.com/watch?v=jhFHX3TCw6I — 영문 자막 열람 2026-09-19.
[OBSERVED] [W56] Steam review, Obra Dinn direct-deduction praise, https://steamcommunity.com/profiles/76561198010217378/recommended/653530/ — 열람 2026-09-19.
[OBSERVED] [W57] PC Gamer, The Case of the Golden Idol review and Classic UI, https://www.pcgamer.com/the-case-of-the-golden-idol-review/ — 열람 2026-09-19.
RESEARCH_DONE — 61 sources cited
