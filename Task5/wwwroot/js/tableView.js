import { escapeHtml, buildCoverUrl } from './utils.js';
import { play as playerPlay, onPlayerEvent, getCurrentIndex, isPlayingIndex } from './player.js';

const ICONS = {
  play: '<svg viewBox="0 0 24 24" fill="currentColor"><path d="M8 5v14l11-7z"/></svg>',
};

let expandedIndex = null;
let currentSongs = [];
let currentSeed = null;

onPlayerEvent((event, data) => {
  if (event === 'trackchange') {
    autoExpandForTrack(data.song, data.seed);
  }
  if (event === 'trackchange' || event === 'play' || event === 'pause' || event === 'ended') {
    refreshPlayingState();
  }
  if (event === 'timeupdate') {
    syncActiveLyric(data.currentTime, data.duration);
  }
});

function autoExpandForTrack(song, seed) {
  const row = document.querySelector(`.row[data-index="${song.index}"]`);
  if (!row) return;
  if (expandedIndex === song.index) return;
  collapseCurrent();
  expandedIndex = song.index;
  row.classList.add('expanded');
  row.insertAdjacentElement('afterend', createDetail(song, seed));
  row.scrollIntoView({ behavior: 'smooth', block: 'center' });
}

function render(songs, page, seed, goToPage) {
  currentSongs = songs;
  currentSeed = seed;
  const body = document.getElementById('rows-body');
  body.innerHTML = '';
  expandedIndex = null;
  songs.forEach(song => body.appendChild(createRow(song, seed)));
  refreshPlayingState();
  renderPagination(page, goToPage);
}

function reset() {
  const body = document.getElementById('rows-body');
  if (body) body.innerHTML = '';
  const pager = document.getElementById('pagination-container');
  if (pager) pager.innerHTML = '';
  expandedIndex = null;
  currentSongs = [];
}

function createRow(song, seed) {
  const meta = sideDur(seed, song.index);
  const row = document.createElement('div');
  row.className = 'row';
  row.dataset.index = song.index;
  row.innerHTML = `
    <div class="n">
      <span class="num">${String(song.index).padStart(2, '0')}</span>
      <span class="play-ic">${ICONS.play}</span>
      <span class="bars"><b></b><b></b><b></b></span>
    </div>
    <img class="r-cover" src="${buildCoverUrl(song, seed)}" alt="" loading="lazy">
    <div class="r-title-cell">
      <div class="r-title">${escapeHtml(song.title)}</div>
      <div class="r-side">Side ${meta.side} · ${meta.dur}</div>
    </div>
    <div class="r-artist">${escapeHtml(song.artist)}</div>
    <div class="r-album">${escapeHtml(song.album)}</div>
    <div><span class="r-genre">${escapeHtml(song.genre)}</span></div>
    <div class="r-likes">${song.likes}</div>
  `;
  row.addEventListener('click', () => toggleDetail(song, seed));
  return row;
}

function sideDur(seed, index) {
  const s = Number(seed) || 0;
  const rng = (i, m) => (Math.abs(Math.sin((i + s) * 1.71)) * m) | 0;
  return {
    side: index % 2 ? 'A' : 'B',
    dur: `3:${String(10 + rng(index * 13, 49)).padStart(2, '0')}`,
  };
}

function toggleDetail(song, seed) {
  const wasExpanded = expandedIndex === song.index;
  collapseCurrent();
  if (!wasExpanded) expandRow(song, seed);
  playerPlay(song, seed);
}

function collapseCurrent() {
  const expanded = document.querySelector('.row.expanded');
  if (!expanded) return;
  expanded.classList.remove('expanded');
  const detail = expanded.nextElementSibling;
  if (detail && detail.classList.contains('detail')) detail.remove();
  expandedIndex = null;
}

function expandRow(song, seed) {
  const row = document.querySelector(`.row[data-index="${song.index}"]`);
  if (!row) return;
  expandedIndex = song.index;
  row.classList.add('expanded');
  row.insertAdjacentElement('afterend', createDetail(song, seed));
}

function createDetail(song, seed) {
  const detail = document.createElement('div');
  detail.className = 'detail';
  detail.dataset.index = song.index;
  detail.innerHTML = `
    <img class="d-cover" src="${buildCoverUrl(song, seed)}" alt="Cover" loading="lazy">
    <div class="d-info">
      <div class="d-kicker">Review Nº ${String(song.index).padStart(3, '0')}</div>
      <div class="d-title">${escapeHtml(song.title)}</div>
      <div class="d-meta"><b>${escapeHtml(song.artist)}</b> · ${escapeHtml(song.album)} · ${escapeHtml(song.genre)}</div>
      <div class="d-review">${escapeHtml(song.review)}</div>
    </div>
    ${buildLyricsColumn(song.lyrics)}
  `;
  return detail;
}

function buildLyricsColumn(lyrics) {
  if (!Array.isArray(lyrics) || lyrics.length === 0) {
    return '<div class="d-lyrics"><div class="d-lyrics-head">Lyrics</div><div class="lyric-line">No lyrics available.</div></div>';
  }
  const lines = lyrics
    .map((l, i) => `<div class="lyric-line" data-line="${i}">${escapeHtml(l)}</div>`)
    .join('');
  return `<div class="d-lyrics" data-lines="${lyrics.length}"><div class="d-lyrics-head">Lyrics</div>${lines}</div>`;
}

function syncActiveLyric(currentTime, duration) {
  const detail = document.querySelector('.detail');
  if (!detail) return;
  if (Number(detail.dataset.index) !== getCurrentIndex()) return;
  const wrap = detail.querySelector('.d-lyrics');
  if (!wrap) return;
  const total = Number(wrap.dataset.lines);
  if (!total || !duration) return;
  const step = duration / total;
  const active = Math.min(Math.floor(currentTime / step), total - 1);
  const lines = wrap.querySelectorAll('.lyric-line[data-line]');
  lines.forEach((el, i) => el.classList.toggle('active', i === active));
  const activeEl = lines[active];
  if (!activeEl) return;
  const offset = activeEl.offsetTop - wrap.clientHeight / 2 + activeEl.clientHeight / 2;
  wrap.scrollTo({ top: Math.max(0, offset), behavior: 'smooth' });
}

function refreshPlayingState() {
  const idx = getCurrentIndex();
  document.querySelectorAll('.row').forEach(row => {
    const i = Number(row.dataset.index);
    row.classList.toggle('playing', isPlayingIndex(i));
    row.classList.toggle('is-current', i === idx);
  });
}

function renderPagination(page, goToPage) {
  const pager = document.getElementById('pagination-container');
  pager.innerHTML = '';
  pager.appendChild(buildPgBtn('‹', page - 1, page <= 1, goToPage));
  pager.appendChild(buildPgBtn(String(page), page, true, goToPage, true));
  pager.appendChild(buildPgBtn('›', page + 1, false, goToPage));
}

function buildPgBtn(label, target, disabled, goToPage, active = false) {
  const btn = document.createElement('button');
  btn.className = 'pg' + (active ? ' active' : '');
  btn.type = 'button';
  btn.textContent = label;
  if (disabled) btn.disabled = true;
  if (!disabled) btn.addEventListener('click', () => goToPage(target));
  return btn;
}

function findSongByIndex(index) {
  return currentSongs.find(s => s.index === index) ?? null;
}

export { render, reset, findSongByIndex };
