"""Validate delivered M6 media, exact frame boundaries, and gallery links."""
import hashlib
import json
import re
import subprocess
from html.parser import HTMLParser
from pathlib import Path

HERE = Path(__file__).resolve().parent
ROOT = HERE.parents[4]
OUT = ROOT / 'docs/media/cinematic-gameplay-m6'
RTK = '/Users/jangyoung/.local/bin/rtk'


def command(args):
    return subprocess.run([RTK, 'proxy', *args], capture_output=True, text=True, check=True)


def probe(path):
    return json.loads(command(['/opt/homebrew/bin/ffprobe', '-v', 'error', '-show_streams',
        '-show_format', '-of', 'json', str(path)]).stdout)


class Links(HTMLParser):
    def __init__(self):
        super().__init__()
        self.links = []
        self.videos = []

    def handle_starttag(self, tag, attrs):
        attrs = dict(attrs)
        self.links.extend(v for k, v in attrs.items() if k in ('href', 'src', 'poster') and v)
        if tag == 'video':
            self.videos.append(attrs)


timeline = json.loads((OUT / 'edit-timeline.json').read_text())
report = dict(schemaVersion=1, scope='M6 file/media checks, not gameplay tests or human immersion review',
              films={}, linkChecks=[], failures=[])


def check(condition, name):
    if not condition:
        report['failures'].append(name)


for name, duration in [('cinematic', 36), ('gameplay-method', 54)]:
    path = OUT / f'{name}.mp4'
    info = probe(path)
    video = next(s for s in info['streams'] if s['codec_type'] == 'video')
    sound = next(s for s in info['streams'] if s['codec_type'] == 'audio')
    check(int(video['nb_frames']) == duration * 30, name + ': exact video frame count')
    check((video['width'], video['height'], video['r_frame_rate']) == (1280, 720, '30/1'), name + ': picture format')
    check(abs(float(video['duration']) - duration) < .001, name + ': exact video duration')
    check(abs(float(info['format']['duration']) - duration) < .05, name + ': container duration')
    check(sound['sample_rate'] == '48000' and sound['channels'] == 2, name + ': soundtrack format')
    decoded = command(['/opt/homebrew/bin/ffmpeg', '-hide_banner', '-nostats', '-i', str(path),
        '-af', 'loudnorm=I=-27:TP=-3:LRA=5:print_format=json', '-f', 'null', '-'])
    (HERE / f'{name}-decode.log').write_text('\n'.join(line.rstrip() for line in decoded.stderr.splitlines()) + '\n')
    levels = json.loads(re.findall(r'\{[^{}]+\}', decoded.stderr)[-1])
    check(-30 <= float(levels['input_i']) <= -24, name + ': ambient bed loudness')
    check(float(levels['input_tp']) <= -3, name + ': audio true peak')
    accumulated = 0
    boundaries = []
    for i, segment in enumerate(timeline['films'][name]):
        clip = probe(HERE / 'render-cache' / f'{name}-{i:02d}.mp4')['streams'][0]
        frames = int(clip['nb_frames'])
        check(frames == segment['duration'] * 30, f'{name}: segment {i} frames')
        check(accumulated == round(segment['startSeconds'] * 30), f'{name}: segment {i} caption boundary')
        boundaries.append(dict(segment=i, firstFrame=accumulated, frames=frames,
                               source=segment['source'], sourceStartSeconds=segment['start']))
        accumulated += frames
    report['films'][name] = dict(sha256=hashlib.sha256(path.read_bytes()).hexdigest(),
        bytes=path.stat().st_size, ffprobe=info, loudness=levels, boundaries=boundaries,
        fullDecode='passed', rawGeneratedAudioUsed=False)

links = Links()
links.feed((OUT / 'index.html').read_text())
for link in links.links:
    if link.startswith(('#', 'http:', 'https:', 'data:')):
        continue
    target = (OUT / link.split('#')[0]).resolve()
    exists = target.is_file()
    check(exists, 'missing gallery asset: ' + link)
    report['linkChecks'].append(dict(link=link, exists=exists))
check(len(links.videos) == 2, 'two gallery players')
check(all('controls' in v and 'autoplay' not in v for v in links.videos), 'manual video controls')
report['result'] = 'PASS' if not report['failures'] else 'FAIL'
(HERE / 'validation.json').write_text(json.dumps(report, ensure_ascii=False, indent=2) + '\n')
print(json.dumps(dict(result=report['result'], films={k:dict(frames=v['ffprobe']['streams'][0]['nb_frames'],
    lufs=v['loudness']['input_i'], truePeak=v['loudness']['input_tp']) for k,v in report['films'].items()},
    links=len(report['linkChecks']), failures=report['failures']), ensure_ascii=False))
raise SystemExit(bool(report['failures']))
