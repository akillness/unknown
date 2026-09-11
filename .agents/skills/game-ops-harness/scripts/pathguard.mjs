#!/usr/bin/env node
// pathguard.mjs — canonicalize + contain a path inside a base directory, rejecting
// traversal, symlinks and escapes. Shell `realpath`/`cd -P` cannot express the
// "no symlink component" rule portably, so this stays in Node stdlib.
//
// Usage: node pathguard.mjs --root <repo-root> --base <subdir> --path <p> [--require file|any]
// stdout (2 lines): <canonical-absolute-path>\n<path-relative-to-base>
// stderr on rejection: <code>\t<detail>
// exit: 0 ok · 2 rejected · 1 usage/internal error
import fs from 'node:fs';
import path from 'node:path';

const argv = process.argv.slice(2);
const opt = {};
for (let i = 0; i < argv.length; i += 1) {
  const a = argv[i];
  if (a.startsWith('--')) {
    const k = a.slice(2);
    const v = argv[i + 1];
    if (v === undefined || v.startsWith('--')) {
      process.stderr.write(`usage\tmissing value for --${k}\n`);
      process.exit(1);
    }
    opt[k] = v;
    i += 1;
  }
}
const need = ['root', 'base', 'path'];
for (const k of need) {
  if (!opt[k]) {
    process.stderr.write(`usage\t--${k} required\n`);
    process.exit(1);
  }
}
const require_ = opt.require || 'file';

const reject = (code, detail) => {
  process.stderr.write(`${code}\t${detail}\n`);
  process.exit(2);
};

let rootGiven;
let rootReal;
let baseReal;
try {
  rootGiven = path.resolve(opt.root);
  rootReal = fs.realpathSync(rootGiven);
} catch {
  process.stderr.write(`no-root\t${opt.root} does not exist\n`);
  process.exit(1);
}
try {
  baseReal = fs.realpathSync(path.join(rootReal, opt.base));
} catch {
  process.stderr.write(`no-base\t${path.join(rootReal, opt.base)} does not exist\n`);
  process.exit(1);
}

const raw = opt.path;
if (raw === '') reject('empty-path', 'empty path argument');
// Reject traversal in the literal argument before any resolution.
if (raw.split(path.sep).includes('..')) reject('traversal', `'..' component in ${raw}`);
if (raw.includes('\0')) reject('bad-path', 'NUL byte in path');

const resolved = path.isAbsolute(raw) ? path.resolve(raw) : path.resolve(baseReal, raw);

// Symlink-component scan over the *literal* path, limited to the subtree below the
// root as it was given, so a symlinked prefix on the root itself (macOS /tmp) is
// not mistaken for an attack.
const scanRoots = [rootGiven, rootReal, baseReal];
for (const sr of scanRoots) {
  if (resolved === sr || !resolved.startsWith(sr + path.sep)) continue;
  const parts = resolved.slice(sr.length + 1).split(path.sep).filter(Boolean);
  let cur = sr;
  for (const part of parts) {
    cur = path.join(cur, part);
    let st;
    try {
      st = fs.lstatSync(cur);
    } catch {
      break; // missing component; reported below as `missing`
    }
    if (st.isSymbolicLink()) reject('symlink', `symlinked path component: ${cur}`);
  }
  break;
}

// Containment: canonicalize the parent, then re-attach the basename so the final
// entry itself is never followed.
const parentDir = path.dirname(resolved);
let parentReal;
try {
  parentReal = fs.realpathSync(parentDir);
} catch {
  reject('missing', `${parentDir} does not exist`);
}
const finalAbs = path.join(parentReal, path.basename(resolved));

if (finalAbs !== baseReal && !finalAbs.startsWith(baseReal + path.sep)) {
  reject('outside-base', `${finalAbs} is outside ${baseReal}`);
}
if (finalAbs === baseReal) reject('outside-base', 'refusing to operate on the base directory itself');

let st;
try {
  st = fs.lstatSync(finalAbs);
} catch {
  reject('missing', `${finalAbs} does not exist`);
}
if (st.isSymbolicLink()) reject('symlink', `${finalAbs} is a symlink`);
if (require_ === 'file' && !st.isFile()) reject('not-file', `${finalAbs} is not a regular file`);

process.stdout.write(`${finalAbs}\n${path.relative(baseReal, finalAbs)}\n`);
