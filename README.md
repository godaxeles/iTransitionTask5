# MusicStore — The Fake Music Catalogue

A single-page web application that simulates a music store by generating **fake but deterministic** song information. Change the seed → the entire catalogue resets. Same seed always produces the same data, cover art, and audio.

## Features

- **3 locales:** English (USA), Deutsch (Deutschland), Українська (Україна)
- **64-bit seed** with random-seed button
- **Likes 0–10** with fractional probabilistic distribution (e.g. `0.5` → 1-in-2 songs get a like)
- **Table view** with pagination + **Gallery view** with infinite scroll
- **Expandable row** with generated cover art (title + artist + pattern), review, synced lyrics, and persistent bottom audio player
- **Real generated music** — music-theory-based melody + bass (chord progressions, scales, ADSR envelope), reproducible per `(seed, index)`
- **ZIP export** of MP3 files (via `NAudio.Lame`)
- **Synced lyric scrolling** during playback

## Tech stack

- **.NET 10** / ASP.NET Core MVC
- **Bogus** — random text generation per locale
- **SkiaSharp** — cover art rendering
- **NAudio.Lame** — WAV → MP3 encoding for export
- Vanilla JS ESM frontend, no framework

## Project layout

```
Task5/
├── Controllers/        # REST API: Songs, Cover, Audio, Export, Locales
├── Services/
│   ├── Audio/          # Music theory: scale, chord progression, melody, bass, synthesizer
│   ├── DataGeneratorService.cs
│   ├── LikesCalculator.cs
│   ├── SeedHelper.cs
│   ├── CoverGeneratorService.cs + CoverPainter.cs
│   ├── SongPackager.cs + Mp3Encoder.cs
│   └── LocaleDataService.cs + LocaleResolver.cs
├── Models/             # SongRecord, GenerationParams, LocaleData, LocaleInfo
├── Locales/            # en.json, de.json, uk.json (wordlists only — no hardcoded data in code)
├── Views/Home/         # Index.cshtml (single page)
└── wwwroot/
    ├── css/app.css
    └── js/             # app.js, player.js, tableView.js, galleryView.js, utils.js
```

## Run locally

Prerequisites: [.NET 10 SDK](https://dotnet.microsoft.com/download)

```bash
cd Task5
dotnet run
```

Open <http://localhost:5099>.

## Deploy with Docker

```bash
docker build -t musicstore .
docker run -p 8080:8080 musicstore
```

## Architecture notes

- **All generation happens server-side.** The browser only fetches pre-generated pages through `/api/songs`, `/api/cover`, `/api/audio`, `/api/export`.
- **Deterministic seeding:** `SeedHelper.ComputePageSeed = userSeed * 31 + page` combines user seed with page number. Likes use a separate derived seed so changing the likes slider does not alter generated titles/artists/albums/genres.
- **No hardcoded locale-specific content** in source — all wordlists live in `Locales/*.json`. Adding a new locale = dropping a new JSON file.

## Credits

Design direction from `design_handoff_music_catalogue/MusicStore Clean.html` (dark streaming-player aesthetic).
