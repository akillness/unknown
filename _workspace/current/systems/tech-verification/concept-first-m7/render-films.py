"""M7 concept-only cinema. All visual inputs come from the selected M7 MCP shots."""
import hashlib
import json
import subprocess
from pathlib import Path

HERE = Path(__file__).resolve().parent
ROOT = HERE.parents[4]
SOURCE = ROOT / 'assets/generated/previz/concept-first-m7'
OUT = ROOT / 'docs/media/concept-first-m7'
CACHE = HERE / 'render-cache'
RTK = '/Users/jangyoung/.local/bin/rtk'
FF = '/opt/homebrew/bin/ffmpeg'
FONTDIR = '/Users/jangyoung/Library/Fonts'
FPS = 24


def run(args):
    return subprocess.run([RTK, 'proxy', *args], capture_output=True, text=True, check=True)


def ff(args):
    return run([FF, '-hide_banner', '-loglevel', 'error', '-y', *args])


def probe(path):
    return json.loads(run(['/opt/homebrew/bin/ffprobe', '-v', 'error', '-show_streams',
        '-show_format', '-of', 'json', str(path)]).stdout)


def stamp(t):
    return f'0:{int(t)//60:02d}:{t%60:05.2f}'


def captions(name):
    lines = ['[Script Info]', 'ScriptType: v4.00+', 'PlayResX: 1280', 'PlayResY: 720',
        'WrapStyle: 2', '[V4+ Styles]',
        'Format: Name, Fontname, Fontsize, PrimaryColour, SecondaryColour, OutlineColour, BackColour, Bold, Italic, Underline, StrikeOut, ScaleX, ScaleY, Spacing, Angle, BorderStyle, Outline, Shadow, Alignment, MarginL, MarginR, MarginV, Encoding',
        'Style: Base,NanumBarunGothic,34,&H00DCE9EC,&H00DCE9EC,&H00161007,&H00161007,0,0,0,0,100,100,0,0,1,1.2,0,7,0,0,0,1',
        '[Events]', 'Format: Layer, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text']
    def text(a, b, x, y, size, content, extra=''):
        lines.append(f'Dialogue: 0,{stamp(a)},{stamp(b)},Base,,0,0,0,,{{\\pos({x},{y})\\fs{size}\\fad(200,200){extra}}}{content}')
    duration = 24 if name == 'cinematic' else 32
    text(0, duration, 48, 25, 18, 'UNKNOWN / CONCEPT FILM', '\\fsp2')
    text(0, duration, 1232, 25, 18, '세계관·원본 컨셉 기반 / 플레이 경험 목표', '\\an9')
    if name == 'cinematic':
        for a,b,title,body in [(0,6,'마지막 당직','파도 너머의 흔적을, 손끝으로 읽다.'),
            (6,12,'흔적을 움직이다','관찰하고, 조건을 시험하고, 다시 확인하다.'),
            (12,18,'기록을 잇다','서로 다른 자료가, 하나의 사건을 가리킨다.'),
            (18,24,'다음 기록으로','선택한 근거를 남기고, 다음 구역을 준비하다.')]:
            text(a+.3,b,56,537,54,title)
            text(a+.5,b,60,621,34,body)
    else:
        blocks = [(0,6,'01  당직의 시작','공간의 소리와 흔적을 따라, 작업대로 시선을 옮깁니다.'),
            (6,12,'02  관찰 · 시험','광학 장치를 손으로 움직여, 달라지는 상태를 비교합니다.'),
            (12,18,'03  대조 · 기록','서로 다른 자료를 대조하고, 선택한 근거를 남깁니다.'),
            (18,24,'04  다음 구역','기록과 준비 조건을 확인한 뒤, 이동을 선택합니다.')]
        for a,b,title,body in blocks:
            text(a,b,52,537,46,title)
            text(a+.3,b,56,621,32,body)
        text(24,32,92,163,52,'관찰  →  시험  →  기록')
        text(24,32,96,270,34,'보이는 흔적을 읽고, 조건의 차이를 확인합니다.')
        text(24,32,96,335,34,'되돌림과 비교를 거쳐, 선택한 근거를 남깁니다.')
        text(24,32,96,428,34,'기록 정리와 다음 구역 진입은 구현할 플레이 목표입니다.')
        text(24,32,96,516,32,'이 영상은 현재 게임 화면이나 완료된 해금을 증명하지 않습니다.')
    path = OUT / f'{name}.ass'
    path.write_text('\n'.join(lines)+'\n')
    return path


def main():
    CACHE.mkdir(parents=True, exist_ok=True)
    OUT.mkdir(parents=True, exist_ok=True)
    selection = json.loads((SOURCE / 'selected-shots.json').read_text())
    clips = []
    inputs = []
    for i, entry in enumerate(selection['shots']):
        path = ROOT / entry['file']
        assert path.is_relative_to(SOURCE) and path.name == 'clip.mp4'
        p = probe(path)
        v = next(s for s in p['streams'] if s['codec_type'] == 'video')
        assert v['r_frame_rate'] == '24/1' and v['nb_frames'] == '141'
        # Hailuo returns the original3:2 ratio for two shots. Top-aligned crop retains
        # the watchroom horizon and gate winch, trimming bottom foreground/water only.
        vf = ('fps=24:start_time=0,scale=1280:720:force_original_aspect_ratio=increase,'
              'crop=1280:720:0:0,setsar=1,tpad=stop_mode=clone:stop_duration=1,'
              'trim=end_frame=144,settb=expr=1/24,setpts=N,'
              'drawbox=x=0:y=0:w=iw:h=62:color=0x071216@0.72:t=fill,'
              'drawbox=x=0:y=526:w=iw:h=194:color=0x071216@0.58:t=fill,'
              'fade=t=in:st=0:d=0.20,fade=t=out:st=5.8:d=0.20')
        dest = CACHE / f'{i}.mp4'
        ff(['-i', str(path), '-an', '-vf', vf, '-frames:v', '144', '-c:v', 'libx264',
            '-preset', 'fast', '-crf', '18', '-r', '24', '-fps_mode', 'cfr', '-pix_fmt', 'yuv420p', str(dest)])
        encoded = probe(dest)['streams'][0]
        assert int(encoded['nb_frames']) == 144 and float(encoded['duration']) == 6
        clips.append(dest)
        inputs.append(dict(file=entry['file'], sha256=hashlib.sha256(path.read_bytes()).hexdigest(),
            width=v['width'],height=v['height'],durationSeconds=float(v['duration']),
            frameCount=141,sourceFPS=24,holdFrames=3,holdSeconds=.125,
            transform='uniform scale to cover; top-aligned16:9crop; no nonuniform stretch'))
    card = CACHE / 'card.mp4'
    ff(['-i', str(ROOT / selection['shots'][2]['file']), '-an', '-vf',
        'select=eq(n\\,140),scale=1280:720:force_original_aspect_ratio=increase,crop=1280:720:0:0,'
        'setsar=1,tpad=stop_mode=clone:stop_duration=8,trim=end_frame=192,settb=expr=1/24,setpts=N,'
        'drawbox=x=0:y=0:w=iw:h=ih:color=0x071216@0.86:t=fill',
        '-frames:v','192','-c:v','libx264','-preset','fast','-crf','18','-r','24','-fps_mode','cfr','-pix_fmt','yuv420p',str(card)])
    outputs = {}
    for name,duration in [('cinematic',24),('gameplay-method',32)]:
        listing = CACHE / f'{name}.txt'
        listing.write_text(''.join(f"file '{p}'\n" for p in clips+([card] if duration==32 else [])))
        ass = captions(name)
        dest = OUT / f'{name}.mp4'
        ff(['-f','concat','-safe','0','-i',str(listing),'-f','lavfi','-i',
            'anoisesrc=color=pink:amplitude=0.25:sample_rate=48000:seed=70911',
            '-t',str(duration),'-vf',f'ass={ass}:fontsdir={FONTDIR}',
            '-af',f'highpass=f=70,lowpass=f=1300,volume=0.3,loudnorm=I=-28:TP=-4:LRA=5,afade=t=in:d=1.2,afade=t=out:st={duration-1.2}:d=1.2',
            '-c:v','libx264','-preset','slow','-crf','18','-r','24','-fps_mode','cfr','-pix_fmt','yuv420p',
            '-c:a','aac','-ar','48000','-ac','2','-b:a','128k','-movflags','+faststart',str(dest)])
        final = probe(dest)
        stream = next(s for s in final['streams'] if s['codec_type']=='video')
        assert int(stream['nb_frames']) == duration*24 and float(stream['duration']) == duration
        ff(['-i',str(dest),'-f','null','-'])
        ff(['-ss','3','-i',str(dest),'-frames:v','1',str(OUT/('poster-cinematic.jpg' if name=='cinematic' else 'poster-gameplay.jpg'))])
        rows = (duration + 7) // 8
        ff(['-i',str(dest),'-vf',f'fps=1/2,scale=426:240,tile=4x{rows}','-frames:v','1',str(HERE/f'{name}-contact-sheet.jpg')])
        outputs[name] = dict(file=str(dest.relative_to(ROOT)),sha256=hashlib.sha256(dest.read_bytes()).hexdigest(),
            ffprobe=final,fullDecode='PASS',frames=int(stream['nb_frames']))
        print(name+' complete',flush=True)
    receipt=dict(schemaVersion=1,scope='concept-only future art/play-experience target',inputs=inputs,
        outputs=outputs,runtimeEligible=False,commercialReleaseEligible=False,
        currentRuntimeVisualInputs=[],derivedRuntimeVisualInputs=[],
        soundtrack=dict(source='new locally authored filtered pink-noise temporary ambience',providerAudioUsed=False,humanAudition=False),
        caveats=['no current gameplay capture','record shot is camera-only','gate remains closed',
                 'physical inverse crank and save/unlock semantics require future runtime implementation',
                 'source aspect ratio deviations are preserved in provenance; final composition uses declared top crop'])
    (HERE/'validation.json').write_text(json.dumps(receipt,ensure_ascii=False,indent=2)+'\n')
    (OUT/'edit-timeline.json').write_text(json.dumps(dict(schemaVersion=1,fps=24,cinematicSeconds=24,methodSeconds=32,
        sourceSelection='../../../assets/generated/previz/concept-first-m7/selected-shots.json',
        shotRangesSeconds=[[0,6],[6,12],[12,18],[18,24]],methodCardRangeSeconds=[24,32],
        sourceHoldSeconds=.125,currentRuntimeVisualInputs=[],runtimeEligible=False),ensure_ascii=False,indent=2)+'\n')


if __name__ == '__main__':
    main()
