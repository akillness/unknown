"""Reproducible M6 previz edit; no Unity/runtime mutations or provider calls."""
import json
import subprocess
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parents[5]
OUT = ROOT / 'docs/media/cinematic-gameplay-m6'
EVIDENCE = Path(__file__).resolve().parent
TMP = EVIDENCE / 'render-cache'
RTK = '/Users/jangyoung/.local/bin/rtk'
FFMPEG = '/opt/homebrew/bin/ffmpeg'
FONTDIR = '/Users/jangyoung/Library/Fonts'
GEN = ROOT / 'assets/generated/previz/cinematic-gameplay-m6'
NATIVE = ROOT / 'docs/media/intro-gameplay-m5/native-gameplay.mp4'
SOURCES = {str(i): GEN / name / 'clip.mp4' for i, name in enumerate([
    'm6-s1-duty-room', 'm6-s2-observe', 'm6-s3-trial-surface', 'm6-s4-record-context'], 1)}
SOURCES['native'] = NATIVE


def run(args):
    subprocess.run([RTK, 'proxy', FFMPEG, '-hide_banner', '-loglevel', 'error', '-y', *args], check=True)


def segment(source, duration, start=0, kind='concept', title='', caption='', eyebrow=''):
    return dict(source=source, duration=duration, start=start, kind=kind,
                title=title, caption=caption, eyebrow=eyebrow)


FILMS = {
    'cinematic': [
        segment('1', 6, title='마지막 당직', caption='바다가 지운 흔적을, 기록으로 잇다.', eyebrow='UNKNOWN  /  CINEMATIC STUDY'),
        segment('1', 6, kind='rule', title='기록이 다음 문을 엽니다', caption='T0 기록 결합  →  C1 순찰', eyebrow='01  /  진행 규칙'),
        segment('native', 6, 14, 'native', '서명지 작업으로', '저장된 순찰 기록에서, 다음 작업을 선택합니다.', '02  /  저장 상태에서 이어하기'),
        segment('2', 6, title='관찰', caption='같은 사건을 가리키는, 서로 다른 흔적.', eyebrow='03  /  LOOK CLOSER'),
        segment('3', 6, title='시험', caption='조건을 바꾸고, 결과를 되돌려 확인합니다.', eyebrow='04  /  TRY · REVERT'),
        segment('4', 3, title='기록', caption='기록은 남고, 미해결은 가려진 채 남습니다.', eyebrow='05  /  LEAVE A TRACE'),
        segment('native', 3, 159, 'native', '기록 · 저장 완료', '현재 구현 구간은 여기까지입니다.', '06  /  실제 저장 결과'),
    ],
    'gameplay-method': [
        segment('1', 6, title='관찰 · 시험 · 기록', caption='흔적을 읽고, 조건을 시험하고, 근거를 남깁니다.', eyebrow='UNKNOWN  /  HOW TO PLAY'),
        segment('1', 9, kind='map', title='작업을 마치면, 다음 단계로', caption='저장 확인 뒤 이동을 직접 선택합니다.', eyebrow='01  /  단계별 진행 규칙'),
        segment('native', 9, 12, 'native', '저장된 순찰 기록 → 서명지 작업', '완료된 기록을 복원한 실제 플레이 장면입니다.', '02  /  이어하기 · 명시적 이동'),
        segment('2', 3, title='관찰', caption='자료를 선택해 살펴보세요.', eyebrow='03  /  OBSERVE'),
        segment('native', 3, 27, 'native', '관찰', '서로 다른 자료를 열어 출처를 확인합니다.', '03  /  실제 조작 발췌'),
        segment('3', 3, title='시험', caption='조건을 선택하고, 결과를 확인합니다.', eyebrow='04  /  TEST'),
        segment('native', 3, 63, 'trial', '시험', '시험 전과 후를 확인합니다. 설정값은 가렸습니다.', '04  /  실제 화면 · 설정값 가림'),
        segment('4', 3, title='기록', caption='사본을 남기고, 가려진 부분도 기록합니다.', eyebrow='05  /  RECORD'),
        segment('native', 3, 89, 'copies', '사본 보존', '원본의 가림을 유지하며 사본을 남깁니다.', '05  /  실제 조작 · 설정값 가림'),
        segment('native', 5, 143, 'ready', '확정 준비', '아직 저장 전입니다. 결과와 근거를 검토합니다.', '06  /  준비 상태 · 저장 전'),
        segment('native', 4, 158, 'native', '기록 · 저장 완료', '저장된 결과를 확인합니다. 가림은 유지됩니다.', '07  /  별도 발췌 · 저장 결과'),
        segment('4', 3, title='미해결도, 기록의 일부', caption='C1 서명지 작업까지 구현 · 이후 단계는 제작 예정', eyebrow='CURRENT PLAYABLE SLICE'),
    ],
}


def ass_time(seconds):
    cs = round(seconds * 100)
    return f'{cs // 360000}:{cs // 6000 % 60:02d}:{cs // 100 % 60:02d}.{cs % 100:02d}'


def make_ass(name, segments):
    header = '''[Script Info]
ScriptType: v4.00+
PlayResX: 1280
PlayResY: 720
WrapStyle: 2
ScaledBorderAndShadow: yes
[V4+ Styles]
Format: Name, Fontname, Fontsize, PrimaryColour, SecondaryColour, OutlineColour, BackColour, Bold, Italic, Underline, StrikeOut, ScaleX, ScaleY, Spacing, Angle, BorderStyle, Outline, Shadow, Alignment, MarginL, MarginR, MarginV, Encoding
Style: Base,NanumBarunGothic,28,&H00ECE9DC,&H00ECE9DC,&H00100D08,&H00100D08,0,0,0,0,100,100,0,0,1,1.2,0,7,0,0,0,1
[Events]
Format: Layer, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text
'''
    lines = []
    captions = []
    def text(start, end, x, y, size, value, color='ECE9DC', extra=''):
        # ASS colours use BBGGRR.
        rr, gg, bb = color[0:2], color[2:4], color[4:6]
        fade = '0,0' if end - start <= 3 else '180,180'
        tags = f'\\pos({x},{y})\\fs{size}\\c&H{bb}{gg}{rr}&\\fad({fade}){extra}'
        lines.append(f'Dialogue: 0,{ass_time(start)},{ass_time(end)},Base,,0,0,0,,{{{tags}}}{value}')
    cursor = 0
    for i, s in enumerate(segments):
        a, b = cursor, cursor + s['duration']
        native = s['source'] == 'native'
        label = '실제 플레이 녹화 · 발췌 편집' if native else '생성 시네마틱 · 연출 프리비즈'
        if s['kind'] in ('rule', 'map'):
            label = '진행 규칙 설명 · 생성 배경'
        text(a, b, 48, 26, 18, s['eyebrow'], 'E2AF62', '\\fsp2')
        text(a, b, 1228, 27, 16, label, 'A8C7C2', '\\an9')
        if native:
            text(a, b, 48, 628, 32, s['title'], 'E2AF62')
            text(a, b, 48, 671, 32, s['caption'])
        elif s['kind'] == 'rule':
            text(a + .3, b, 76, 194, 45, s['title'])
            text(a + .8, b, 78, 300, 36, s['caption'], 'E2AF62')
            text(a + 1.2, b, 78, 371, 32, '기록 결합 저장 완료  →  순찰로 이동 선택', 'C8D6D3')
            text(a + 1.2, b, 78, 425, 32, '순찰 기록 저장 완료  →  서명지 작업 선택', 'C8D6D3')
            text(a, b, 78, 624, 32, '해금 규칙 설명 · 실제 전환 녹화와 구분합니다.', 'A8C7C2')
        elif s['kind'] == 'map':
            text(a, b, 76, 122, 42, s['title'])
            rows = [('T0 · 인계', '인계 자료와 빈칸 판단'), ('T0 · 증거 정리', '세 구역의 자료를 연결'),
                    ('T0 · 기록 결합', '기록 저장 후 순찰 이동'), ('C1 · 순찰', '순찰 기록 저장 후 서명지 작업'),
                    ('C1 · 서명지', '근거 확인과 저장으로 현재 구간 완료')]
            for n, (stage, condition) in enumerate(rows):
                y = 212 + n * 70
                text(a + .25 * n, b, 80, y, 32, f'{n + 1:02d}   {stage}', 'E2AF62')
                text(a + .25 * n, b, 411, y + 2, 32, condition, 'D7E0DD')
            text(a, b, 78, 624, 32, s['caption'], 'A8C7C2')
        else:
            text(a + .15, b, 76, 480, 70 if i == 0 else 58, s['title'])
            text(a, b, 80, 580, 32, s['caption'], 'D3DCD6')
            text(a, b, 80, 660, 16, f'{i + 1:02d} / {len(segments):02d}   ━   OBSERVE · TEST · RECORD', 'A8C7C2', '\\fsp1')
        captions.append(dict(startSeconds=a, endSeconds=b, **s))
        cursor = b
    (OUT / f'{name}.ass').write_text(header + '\n'.join(lines) + '\n')
    return captions


def render(name, segments):
    clips = []
    for i, s in enumerate(segments):
        dest = TMP / f'{name}-{i:02d}.mp4'
        if '--typeset-only' in sys.argv and dest.exists():
            clips.append(dest)
            continue
        native = s['source'] == 'native'
        vf = 'fps=30:start_time=0,scale=1280:720,setsar=1,format=yuv420p'
        if native:
            # Remove a puzzle-condition status row from the public edit, not the source.
            # Only the ready shot contains the solved humidity value; cover its exact row.
            if s['kind'] in ('ready', 'trial', 'copies'):
                vf += ',drawbox=x=600:y=149:w=650:h=27:color=0x172A2C:t=fill'
            if s['kind'] == 'trial':
                vf += ',drawbox=x=612:y=365:w=619:h=166:color=0x172A2C:t=fill'
            vf += ',scale=1000:562,pad=1280:720:140:64:color=0x071216'
        else:
            vf += ',eq=brightness=-0.018:saturation=0.86'
            if s['source'] == '3':
                vf += ',eq=brightness=-0.10:saturation=0.68:contrast=1.08'
            vf += ',drawbox=x=0:y=0:w=iw:h=64:color=0x071216@0.80:t=fill,drawbox=x=0:y=466:w=iw:h=254:color=0x071216@0.58:t=fill'
            if s['kind'] in ('rule', 'map'):
                vf += ',drawbox=x=0:y=64:w=iw:h=656:color=0x071216@0.68:t=fill'
        vf += ',fade=t=in:st=0:d=0.16,fade=t=out:st=' + str(s['duration'] - .16) + ':d=0.16'
        vf += f',trim=end_frame={s["duration"] * 30},setpts=N/(30*TB)'
        run(['-stream_loop', '-1', '-ss', str(s['start']), '-i', str(SOURCES[s['source']]),
             '-frames:v', str(s['duration'] * 30), '-an', '-vf', vf, '-c:v', 'libx264', '-preset', 'fast', '-crf', '18', '-r', '30', str(dest)])
        clips.append(dest)
    listing = TMP / f'{name}-concat.txt'
    listing.write_text(''.join(f"file '{p}'\n" for p in clips))
    duration = sum(s['duration'] for s in segments)
    # A deterministic authored wind/room bed. Raw model audio awaits human audition.
    audio = 'anoisesrc=color=pink:amplitude=0.32:sample_rate=48000:seed=260911'
    af = f'highpass=f=65,lowpass=f=1400,volume=0.28,apulsator=mode=sine:hz=0.13:amount=0.15,loudnorm=I=-27:TP=-3:LRA=5,afade=t=in:d=1.2,afade=t=out:st={duration-1.5}:d=1.5'
    run(['-f', 'concat', '-safe', '0', '-i', str(listing), '-f', 'lavfi', '-i', audio,
         '-t', str(duration), '-vf', f'ass={OUT / (name + ".ass")}:fontsdir={FONTDIR}',
         '-af', af, '-c:v', 'libx264', '-preset', 'slow', '-crf', '18', '-pix_fmt', 'yuv420p',
         '-c:a', 'aac', '-b:a', '160k', '-ar', '48000', '-ac', '2', '-movflags', '+faststart', str(OUT / f'{name}.mp4')])
    run(['-ss', '3', '-i', str(OUT / f'{name}.mp4'), '-frames:v', '1', str(OUT / ('poster-cinematic.jpg' if name == 'cinematic' else 'poster-gameplay.jpg'))])
    rows = (duration + 8) // 9
    run(['-i', str(OUT / f'{name}.mp4'), '-vf', f'fps=1/3,scale=426:240,tile=3x{rows}', '-frames:v', '1', str(EVIDENCE / f'{name}-contact-sheet.jpg')])


if __name__ == '__main__':
    OUT.mkdir(parents=True, exist_ok=True)
    TMP.mkdir(parents=True, exist_ok=True)
    timeline = {name: make_ass(name, segments) for name, segments in FILMS.items()}
    (OUT / 'edit-timeline.json').write_text(json.dumps(dict(schemaVersion=1,
        purpose='Cinematic and gameplay-method previz edit; not continuous gameplay evidence',
        fps=30, width=1280, height=720, films=timeline, runtimeEligible=False,
        commercialReleaseEligible=False, soundtrack=dict(source='locally-authored deterministic filtered pink noise',
        rawGeneratedAudioUsed=False, targetLUFS=-27, reason='Generated audio has not received a listening approval'),
        evidenceLimits=['Earlier unlocks are manual rule explanations.', 'Native source is an existing three-take montage.',
                        'No M6 Unity changes.', 'Generated camera motion is previz only.']), ensure_ascii=False, indent=2) + '\n')
    for name, segments in FILMS.items():
        render(name, segments)
        print(name + ' rendered', flush=True)
