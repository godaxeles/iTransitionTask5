import { escapeHtml, buildCoverUrl } from './utils.js';
import { play as playerPlay, onPlayerEvent, getCurrentIndex, isPlayingIndex } from './player.js';

const ICONS = {
  play: '<svg viewBox="0 0 24 24" fill="currentColor"><path d="M8 5v14l11-7z"/></svg>',
  pause: '<svg viewBox="0 0 24 24" fill="currentColor"><path d="M6 5h4v14H6zm8 0h4v14h-4z"/></svg>',
};

let currentPage = 0;
let isLoading = false;
let _loadNextFn = null;
let cards = new Map();

onPlayerEvent((event) => {
  if (['trackchange', 'play', 'pause', 'ended'].includes(event)) refreshPlayingState();
});

function nextPage() {
  if (isLoading) return null;
  isLoading = true;
  currentPage++;
  return currentPage;
}

function append(songs, seed) {
  const grid = document.getElementById('gallery-grid');
  songs.forEach(song => {
    const card = createCard(song, seed);
    grid.appendChild(card);
    cards.set(song.index, { card, song, seed });
  });
  refreshPlayingState();
  isLoading = false;
  const sentinel = document.getElementById('gallery-sentinel');
  const { top, bottom } = sentinel.getBoundingClientRect();
  if (top < window.innerHeight && bottom > 0 && _loadNextFn) _loadNextFn();
}

function createCard(song, seed) {
  const card = document.createElement('div');
  card.className = 'card';
  card.dataset.index = song.index;
  card.innerHTML = `
    <div class="card-cover">
      <img src="${buildCoverUrl(song, seed)}" alt="" loading="lazy">
      <button class="card-play" type="button" aria-label="Play">
        <span class="card-play-inner">${ICONS.play}</span>
      </button>
    </div>
    <div class="c-title">${escapeHtml(song.title)}</div>
    <div class="c-artist">${escapeHtml(song.artist)}</div>
    <div class="c-meta">${escapeHtml(song.album)}</div>
  `;
  const onClick = (e) => {
    e.stopPropagation();
    playerPlay(song, seed);
  };
  card.querySelector('.card-play').addEventListener('click', onClick);
  card.addEventListener('click', onClick);
  return card;
}

function refreshPlayingState() {
  const curIdx = getCurrentIndex();
  cards.forEach(({ card }, index) => {
    const playing = isPlayingIndex(index);
    const isCurrent = index === curIdx;
    card.classList.toggle('playing', playing);
    card.classList.toggle('is-current', isCurrent);
    const inner = card.querySelector('.card-play-inner');
    if (inner) inner.innerHTML = playing ? ICONS.pause : ICONS.play;
  });
}

function reset() {
  currentPage = 0;
  isLoading = false;
  cards.clear();
  const grid = document.getElementById('gallery-grid');
  if (grid) grid.innerHTML = '';
}

function initInfiniteScroll(loadNextFn) {
  _loadNextFn = loadNextFn;
  const sentinel = document.getElementById('gallery-sentinel');
  new IntersectionObserver(
    entries => { if (entries[0].isIntersecting) loadNextFn(); },
    { threshold: 0 }
  ).observe(sentinel);
}

export { nextPage, append, reset, initInfiniteScroll };
