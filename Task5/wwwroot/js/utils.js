export function formatTime(s) {
  if (!isFinite(s)) return '0:00';
  const m = Math.floor(s / 60);
  return m + ':' + String(Math.floor(s % 60)).padStart(2, '0');
}

export function escapeHtml(str) {
    return String(str ?? '')
        .replace(/&/g, '&amp;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;')
        .replace(/"/g, '&quot;');
}

export function buildCoverUrl(song, seed) {
    const params = new URLSearchParams({
        seed,
        index: song.index,
        title: song.title,
        artist: song.artist,
    });
    return `/api/cover?${params}`;
}
