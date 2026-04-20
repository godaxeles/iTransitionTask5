import { formatTime, buildCoverUrl, buildAudioUrl } from './utils.js';

const ICONS = {
  play: '<svg viewBox="0 0 24 24" fill="currentColor"><path d="M8 5v14l11-7z"/></svg>',
  pause: '<svg viewBox="0 0 24 24" fill="currentColor"><path d="M6 5h4v14H6zm8 0h4v14h-4z"/></svg>',
  volUp: '<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M11 5L6 9H2v6h4l5 4V5zM15 9a4 4 0 0 1 0 6M19 5a8 8 0 0 1 0 14"/></svg>',
  volMute: '<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M11 5L6 9H2v6h4l5 4V5zM23 9l-6 6M17 9l6 6"/></svg>',
};

const listeners = new Set();
const audio = () => document.getElementById('global-audio');

let currentIndex = null;
let currentSeed = null;
let lastVolume = 1;
let isSeeking = false;

function notify(event, data) {
  listeners.forEach(fn => fn(event, data));
}

function onPlayerEvent(fn) {
  listeners.add(fn);
  return () => listeners.delete(fn);
}

function getCurrentIndex() {
  return currentIndex;
}

function isPlayingIndex(index) {
  const a = audio();
  return currentIndex === index && a && !a.paused;
}

function play(song, seed) {
  const a = audio();
  if (!a) return;

  if (currentIndex === song.index && currentSeed === seed) {
    togglePlayPause();
    return;
  }

  currentIndex = song.index;
  currentSeed = seed;

  setPlayerInfo(song, seed);
  a.src = buildAudioUrl(song, seed);
  a.load();
  a.play().catch(() => {});
  notify('trackchange', { song, seed });
}

function togglePlayPause() {
  const a = audio();
  if (!a || !a.src) return;
  if (a.paused) a.play().catch(() => {});
  else a.pause();
}

function setPlayerInfo(song, seed) {
  const cover = document.getElementById('pl-cover');
  const title = document.getElementById('pl-title');
  const artist = document.getElementById('pl-artist');
  cover.style.backgroundImage = `url('${buildCoverUrl(song, seed)}')`;
  title.textContent = song.title;
  artist.textContent = song.artist;
}

function updateVolIcon() {
  const a = audio();
  const el = document.getElementById('vol-icon');
  if (!a || !el) return;
  el.innerHTML = (a.muted || a.volume === 0) ? ICONS.volMute : ICONS.volUp;
}

function updatePlayBtn() {
  const a = audio();
  const inner = document.querySelector('#pl-play .pl-play-inner');
  if (!a || !inner) return;
  inner.innerHTML = a.paused ? ICONS.play : ICONS.pause;
}

function wireDragBar(bar, onChange) {
  let activePointerId = null;

  const computePct = (clientX) => {
    const rect = bar.getBoundingClientRect();
    return Math.max(0, Math.min(1, (clientX - rect.left) / rect.width));
  };

  const onMove = e => {
    if (e.pointerId !== activePointerId) return;
    onChange(computePct(e.clientX));
  };

  const onUp = e => {
    if (e.pointerId !== activePointerId) return;
    activePointerId = null;
    bar.releasePointerCapture?.(e.pointerId);
    bar.removeEventListener('pointermove', onMove);
    bar.removeEventListener('pointerup', onUp);
    bar.removeEventListener('pointercancel', onUp);
  };

  bar.addEventListener('pointerdown', e => {
    if (e.button !== 0 && e.pointerType === 'mouse') return;
    activePointerId = e.pointerId;
    bar.setPointerCapture?.(e.pointerId);
    bar.addEventListener('pointermove', onMove);
    bar.addEventListener('pointerup', onUp);
    bar.addEventListener('pointercancel', onUp);
    onChange(computePct(e.clientX));
  });
}

function initPlayer() {
  const a = audio();
  if (!a) return;

  a.volume = 1;

  const playBtn = document.getElementById('pl-play');
  const prevBtn = document.getElementById('pl-prev');
  const nextBtn = document.getElementById('pl-next');
  const bar = document.getElementById('pl-bar');
  const fill = document.getElementById('pl-fill');
  const currentEl = document.getElementById('pl-current');
  const durationEl = document.getElementById('pl-duration');
  const volBtn = document.getElementById('vol-btn');
  const volBar = document.getElementById('vol-bar');
  const volFill = document.getElementById('vol-fill');

  playBtn.addEventListener('click', togglePlayPause);
  prevBtn.addEventListener('click', () => notify('prev'));
  nextBtn.addEventListener('click', () => notify('next'));

  a.addEventListener('loadedmetadata', () => {
    durationEl.textContent = formatTime(a.duration);
  });
  a.addEventListener('timeupdate', () => {
    if (isSeeking) return;
    currentEl.textContent = formatTime(a.currentTime);
    if (a.duration) fill.style.width = (a.currentTime / a.duration) * 100 + '%';
    notify('timeupdate', { currentTime: a.currentTime, duration: a.duration });
  });
  a.addEventListener('play', () => { updatePlayBtn(); notify('play'); });
  a.addEventListener('pause', () => { updatePlayBtn(); notify('pause'); });
  a.addEventListener('ended', () => { updatePlayBtn(); notify('ended'); });
  a.addEventListener('seeking', () => { isSeeking = true; });
  a.addEventListener('seeked', () => {
    isSeeking = false;
    currentEl.textContent = formatTime(a.currentTime);
    if (a.duration) fill.style.width = (a.currentTime / a.duration) * 100 + '%';
  });

  wireDragBar(bar, pct => {
    if (!a.duration || !isFinite(a.duration)) return;
    currentEl.textContent = formatTime(pct * a.duration);
    fill.style.width = pct * 100 + '%';
    a.currentTime = pct * a.duration;
  });

  wireDragBar(volBar, pct => {
    a.muted = pct === 0;
    a.volume = pct;
    if (pct > 0) lastVolume = pct;
    volFill.style.width = pct * 100 + '%';
    updateVolIcon();
  });

  volBtn.addEventListener('click', () => {
    if (a.muted || a.volume === 0) {
      a.muted = false;
      a.volume = lastVolume || 1;
      volFill.style.width = a.volume * 100 + '%';
    } else {
      lastVolume = a.volume;
      a.muted = true;
      volFill.style.width = '0%';
    }
    updateVolIcon();
  });

  updateVolIcon();
  updatePlayBtn();
}

export { initPlayer, play, togglePlayPause, onPlayerEvent, getCurrentIndex, isPlayingIndex };
