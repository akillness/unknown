---
updated: 2026-09-11
cycle: 20260909-preproduction-c7
status: current
supersedes: null
owner: game-concept-artist
---

# M7 원본 컨셉 우선 시각 출처 감사

[OBSERVED] 사용자의 M7 수정 지시에 따라 **정식 세계관과 초기 프리프로덕션 컨셉**을 새 영상의 출발점으로 삼는다. 현재 게임플레이·프리팹·런타임 리소스·M5/M6 파생 결과는 새 영상의 외형을 제한하는 참조로 사용하지 않는다. 이 문서는 출처 감사이며 새 런타임 구현·플레이 검증·상업 이용 승인이 아니다.

## 허용 원본 4장

정확한 경로와 SHA256이 아래와 모두 일치할 때만 원본 시각 참조로 허용한다. 폴더 전체를 허용하지 않는다. 기존 provenance의 출력 SHA와 원본 프롬프트 SHA를 각각 4/4 대조했고 모두 일치했다. 각 원본은 직접 시각 검토했다.

| 원본 경로 | 실제 크기 | SHA256 | 출처 기록 |
|---|---|---|---|
| assets/generated/2d/concept/space-hub-watchroom-mood.png | 1536×1024 | a90bc9ca404b93602ea2a799acbe43add6fff8861cffe18e6179cfcd3768a79c | assets/generated/2d/concept/provenance.json → space-hub-watchroom-mood |
| assets/generated/2d/concept/tool-reader-hero.png | 1312×1199 | ec8f6e4174db859ce3f831a93e2d0f68c87a17ad3a4748d15ddfad4f0cf9a5a8 | assets/generated/2d/concept/provenance.json → tool-reader-hero |
| assets/generated/2d/concept/space-gate-three-mood.png | 1536×1024 | 65e28ab329de3776ceb6bc87dd00dc78b8d4b8b23aa0283881fd0c200c6e7013 | assets/generated/2d/concept/provenance.json → space-gate-three-mood |
| assets/generated/2d/keyart/keyart-watchroom-wide.png | 1672×941 | e434eceecaac1987a4e7010c698e92950e527dc75452fa2c9b5e66a610f5f992 | assets/generated/2d/keyart/provenance.json → keyart-watchroom-wide |

[OBSERVED] 네 원본은 기존 기록상 GTI, 요청 오케스트레이션 모델 gpt-6-astra로 생성되었다. 실제 이미지 모델은 별도로 확정하지 않는다. 원본 provenance의 runtimeEligible:false, 생성 이미지/게임플레이 아님 표시, UNVERIFIED 이용약관·권리 상태를 유지한다. **참조 허용은 런타임 승격이나 상업 이용 허가가 아니다.** 전체 원본 provenance 항목, provenance 파일 해시, 원본 프롬프트 해시는 동명 JSON에 보존했다.

## 원본에서 읽은 시각 기준

- **당직실**: 낮고 넓게 이어지는 창, 방파제 사이 바다, 여러 칸 서랍장, 젖은 타공 금속 작업대, 종이 더미와 받침 구조, 원형 벽 계기, 소금이 쌓인 배관 이음과 창틀. 작은 실용등만 따뜻하다.
- **판독기**: 두꺼운 원형 회전 베이스와 얕은 받침, **육각 소금 결정 판**, 오른쪽 수직 기둥과 관절식 광학 암, 큰 유리 확대경, 수직 탐침·미세조절 노브, 옆 크랭크와 황토 손잡이. 원본 이미지에는 **직사각 종이 베드와 평행 이중 가이드 레일이 보이지 않는다.** 원본 프롬프트도 원형 받침·육각 소금 판·확대경/탐침 암·크랭크를 명시한다.
- **제3수문**: 양쪽 콘크리트 기둥 사이 수직 사각 문짝, 상부 보행교·대형 권양 드럼, 양측 체인, 왼쪽 수동 휠과 아날로그 계기, 하단 수면·수위 얼룩. 개방 조건이나 진행 판정은 이 정지 이미지로 확정하지 않는다.
- **와이드 키아트**: 왼쪽 인물의 3/4 후면과 묶은 머리, 앞치마·어두운 작업복, 넓은 바다 창, 낮은 원형 판독기, 오른쪽 작업등과 종이 묶음. 인물 얼굴·대사·입력 동작의 증거로 사용하지 않는다. 판독기 세부는 hero 원본이 우선한다.

[OBSERVED] 이번 감사에서는 현재 3D·게임 화면·M5/M6 자료를 다시 열거나 대조하지 않았다. **현재 리소스 대비 일치/불일치를 새로 판정하지 않고 원본 자체의 특징을 명확히 고정한다.**

## 팔레트와 재료

[TARGET] style-guide.md의 정식 8색: 심해 잉크 **#0E1F26**, 확정 잉크 **#173238**, 젖은 금속 청회 **#36565C**, 부식 청동 녹청 **#4F7A6B**, 젖은 콘크리트 **#8A8E88**, 소금 결정 백청 **#C8D6D3**, 기록 종이 회백 **#E7E3D8**, 유일한 난색 악센트 황토 **#E2AF62**(프레임 ≤8%). 예비 적갈 #8C4A3A는 도관 이음의 산화 흔적에만 ≤2%이며 일반 아홉 번째 색으로 확장하지 않는다.

[TARGET] 소금은 가장자리에서 자라는 미세 육각 군집과 무광 산란, 청동은 어두운 바탕·녹청 반점·아래로 흐르는 부식 흔적, 젖은 콘크리트는 마른 부분 대비 명도 −20%와 수평 수위 흔적을 사용한다. 염선 도관은 일정 굵기·이음마다 백색 염화 고리·원형 아날로그 계기가 기준이다. 광택 크롬, 새 표면, 균일 민트 페인트, 네온·홀로그램·디지털 대시보드·초자연적 광휘는 도입하지 않는다.

[TARGET] 톤 필러는 **젖은 금속과 소금 / 절차의 무게 / 조용한 압력**이다. art-direction.md는 시각 의도, style-guide.md는 생성 이전의 규범 TARGET이다. 이 수치들은 M7 렌더 실측치가 아니다. style-guide의 고정 시점 규칙은 인게임 TARGET이며, 기존 런타임 카메라·프리팹 외형을 M7 영화의 제약으로 역수입하지 않는다. 영화 카메라는 원본 컨셉과 세계관에 맞춰 별도 저작한다.

## 차단 목록과 다음 생성

- unity/**: 화면·씬·프리팹·메시·재료·빌드 전부 시각 참조 제외.
- docs/media/**, _workspace/current/systems/tech-verification/**: 실제 녹화·스크린샷과 이전 편집 영상 제외.
- assets/generated/3d/**: 현재 Blender/3D 모델·미리보기 전부 제외.
- assets/generated/previz/**: M5/M6를 포함한 이전 생성 영상 제외.
- assets/generated/2d/**: 위 네 경로를 제외한 전부 기본 차단. M5 인트로·플레이플로·방향표 표면, M6 판독기·기록 키프레임도 제외.

새 이미지·텍스처 제공자는 **GTI**다. 구체적인 참조 부분집합·프롬프트를 디렉터가 지정한 뒤 dry-run → 생성 → 원본 그대로 보존 → 시각 검토한다. 새 M7 파생물도 자동 참조 허용하지 않으며 후속 영상 입력은 별도 검토한다. 이 감사 종료 시점까지 새 생성은 0건이다. 생성 실행 승인·후속 상태는 동명 JSON의 별도 항목에 기록한다.

## 별도 승인된 후속 생성

[OBSERVED] 원본 감사 종료 후 디렉터가 tool-reader-hero + space-hub-watchroom-mood 두 참조와 구체적인 프롬프트를 지정했다. GTI dry-run 및 실제 생성은 exit 0으로 완료했다. 결과: assets/generated/2d/concept/m7-optical-workbench-r01/image.png, 요청 2048×1152 / 실제 1672×941, SHA256 8bdf991d3328ee9445ce2d6c4bb0dd6fe151db0f5cff6c2725fb0e05ca4c9628. 원본 출력은 픽셀 후편집 없이 보존했다. 소금 결정·광학 암·확대경·탐침·크랭크는 시각 검토 통과했으며 영상 입력 전 디렉터 검토를 기다린다. 기존 원본 4장 allowlist는 변경하지 않았다.
