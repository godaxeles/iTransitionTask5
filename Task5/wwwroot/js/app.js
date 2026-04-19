import { render as renderTable, reset as resetTable, findSongByIndex } from './tableView.js';
import { nextPage as galleryNextPage, append as galleryAppend, reset as resetGallery, initInfiniteScroll } from './galleryView.js';
import { initPlayer, onPlayerEvent, play as playerPlay, getCurrentIndex } from './player.js';

const state = {
    locale: 'en',
    seed: generateRandomSeed(),
    likes: 0,
    view: 'table',
    tablePage: 1,
    lastSongs: [],
};

function generateRandomSeed() {
    const bytes = crypto.getRandomValues(new Uint8Array(8));
    bytes[0] &= 0x7F;
    let result = 0n;
    for (const b of bytes) result = (result << 8n) | BigInt(b);
    return result.toString();
}

function isValidSeed(value) {
    return /^-?\d+$/.test(value);
}

function buildApiUrl(page) {
    const params = new URLSearchParams({
        page,
        seed: state.seed,
        locale: state.locale,
        likes: state.likes,
    });
    return `/api/songs?${params}`;
}

function buildExportUrl() {
    const params = new URLSearchParams({
        seed: state.seed,
        locale: state.locale,
        likes: state.likes,
        pageSize: 10,
    });
    return `/api/export?${params}`;
}

async function fetchSongs(page) {
    const response = await fetch(buildApiUrl(page));
    return response.json();
}

async function onParamsChanged() {
    state.tablePage = 1;
    if (state.view === 'table') {
        await loadTablePage({ fullReset: true });
    } else {
        resetGallery();
        await loadNextGalleryPage();
    }
}

async function loadTablePage({ fullReset = false } = {}) {
    const section = document.getElementById('table-section');
    section.classList.add('is-loading');
    try {
        const songs = await fetchSongs(state.tablePage);
        state.lastSongs = songs;
        if (fullReset) resetTable();
        renderTable(songs, state.tablePage, state.seed, goToTablePage);
        updateCountLine(songs.length);
    } finally {
        requestAnimationFrame(() => section.classList.remove('is-loading'));
    }
}

async function loadNextGalleryPage() {
    const page = galleryNextPage();
    if (page === null) return;
    const section = document.getElementById('gallery-section');
    const isFirstPage = page === 1;
    if (isFirstPage) section.classList.add('is-loading');
    try {
        const songs = await fetchSongs(page);
        galleryAppend(songs, state.seed);
        const grid = document.getElementById('gallery-grid');
        updateCountLine(grid.children.length);
    } finally {
        if (isFirstPage) requestAnimationFrame(() => section.classList.remove('is-loading'));
    }
}

function updateCountLine(count) {
    const countNum = document.getElementById('count-num');
    const pageLine = document.getElementById('page-line');
    countNum.textContent = count;
    if (state.view === 'table') {
        pageLine.innerHTML = `Page <b id="page-num">${state.tablePage}</b>`;
    } else {
        pageLine.innerHTML = `Showing <b>${count}</b> · infinite scroll`;
    }
}

function goToTablePage(page) {
    state.tablePage = page;
    window.scrollTo({ top: 0, behavior: 'smooth' });
    loadTablePage();
}

async function loadLocales() {
    const localeEl = document.getElementById('locale');
    const response = await fetch('/api/locales');
    const locales = await response.json();
    localeEl.innerHTML = '';
    locales.forEach(loc => {
        const opt = document.createElement('option');
        opt.value = loc.code;
        opt.textContent = loc.name;
        localeEl.appendChild(opt);
    });
    if (locales.length > 0) state.locale = locales[0].code;
    localeEl.value = state.locale;
}

function initToolbar() {
    const localeEl = document.getElementById('locale');
    const seedEl = document.getElementById('seed');
    const seedIndicator = document.getElementById('seed-indicator');
    const randomSeedBtn = document.getElementById('random-seed');
    const likesEl = document.getElementById('likes');
    const likesValueEl = document.getElementById('likes-value');

    seedEl.value = state.seed;
    seedIndicator.textContent = state.seed;

    localeEl.addEventListener('change', () => {
        state.locale = localeEl.value;
        onParamsChanged();
    });

    seedEl.addEventListener('input', debounce(() => {
        if (isValidSeed(seedEl.value)) {
            state.seed = seedEl.value;
            seedIndicator.textContent = state.seed;
            onParamsChanged();
        }
    }, 400));

    randomSeedBtn.addEventListener('click', () => {
        state.seed = generateRandomSeed();
        seedEl.value = state.seed;
        seedIndicator.textContent = state.seed;
        onParamsChanged();
    });

    const debouncedLikes = debounce(() => {
        state.likes = parseFloat(likesEl.value);
        onParamsChanged();
    }, 150);

    likesEl.addEventListener('input', () => {
        likesValueEl.textContent = `${parseFloat(likesEl.value).toFixed(1)} / 10`;
        debouncedLikes();
    });
}

function initExportButton() {
    const btn = document.getElementById('export-zip');
    const label = btn.querySelector('.export-label');
    btn.addEventListener('click', async () => {
        if (btn.disabled) return;
        const originalLabel = label.textContent;
        btn.disabled = true;
        label.textContent = 'Packing…';
        try {
            const response = await fetch(buildExportUrl());
            if (!response.ok) throw new Error('Export failed');
            const blob = await response.blob();
            triggerDownload(blob, `musicstore-${state.seed}.zip`);
        } finally {
            btn.disabled = false;
            label.textContent = originalLabel;
        }
    });
}

function triggerDownload(blob, filename) {
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = filename;
    document.body.appendChild(a);
    a.click();
    a.remove();
    URL.revokeObjectURL(url);
}

function initViewToggle() {
    const tableBtn = document.getElementById('view-table');
    const galleryBtn = document.getElementById('view-gallery');
    const tableSection = document.getElementById('table-section');
    const gallerySection = document.getElementById('gallery-section');

    tableBtn.addEventListener('click', () => {
        if (state.view === 'table') return;
        state.view = 'table';
        tableSection.classList.add('active');
        gallerySection.classList.remove('active');
        tableBtn.classList.add('active');
        galleryBtn.classList.remove('active');
        state.tablePage = 1;
        loadTablePage();
    });

    galleryBtn.addEventListener('click', () => {
        if (state.view === 'gallery') return;
        state.view = 'gallery';
        tableSection.classList.remove('active');
        gallerySection.classList.add('active');
        tableBtn.classList.remove('active');
        galleryBtn.classList.add('active');
        resetGallery();
        loadNextGalleryPage();
    });
}

function initPlayerNavigation() {
    onPlayerEvent((event) => {
        if (event !== 'prev' && event !== 'next' && event !== 'ended') return;
        const currentIndex = getCurrentIndex();
        if (currentIndex === null) return;
        const delta = event === 'prev' ? -1 : 1;
        const nextIndex = currentIndex + delta;
        const song = findSongByIndex(nextIndex);
        if (song) playerPlay(song, state.seed);
    });
}

function debounce(fn, delay) {
    let timer;
    return (...args) => {
        clearTimeout(timer);
        timer = setTimeout(() => fn(...args), delay);
    };
}

document.addEventListener('DOMContentLoaded', async () => {
    initPlayer();
    initPlayerNavigation();
    await loadLocales();
    initToolbar();
    initViewToggle();
    initExportButton();
    initInfiniteScroll(loadNextGalleryPage);
    loadTablePage();
});
