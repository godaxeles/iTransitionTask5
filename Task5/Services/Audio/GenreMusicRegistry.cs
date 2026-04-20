using Task5.Models;

namespace Task5.Services.Audio;

public static class GenreMusicRegistry
{
    private static readonly Dictionary<GenreCategory, GenreMusicProfile> Profiles = new()
    {
        [GenreCategory.Rock] = new(118, 140, ScaleType.MinorPentatonic,
            MelodyPrograms: [29, 30, 27, 18],
            BassPrograms: [33, 34],
            PadPrograms: [89, 90, 48],
            MelodyDensity.Eighth, BassPattern.Octave,
            0.28f, 0.34f, 0.14f, 1, 0),

        [GenreCategory.Metal] = new(160, 200, ScaleType.Phrygian,
            MelodyPrograms: [30, 29, 80, 81],
            BassPrograms: [33, 38],
            PadPrograms: [89, 91, 95],
            MelodyDensity.Sixteenth, BassPattern.Pedal,
            0.26f, 0.38f, 0.12f, 0, -1),

        [GenreCategory.Pop] = new(92, 118, ScaleType.Major,
            MelodyPrograms: [0, 4, 80, 11, 1],
            BassPrograms: [33, 34],
            PadPrograms: [89, 90, 48, 88],
            MelodyDensity.Eighth, BassPattern.RootFifth,
            0.34f, 0.32f, 0.16f, 1, 0),

        [GenreCategory.Jazz] = new(130, 180, ScaleType.Dorian,
            MelodyPrograms: [0, 4, 11, 56, 65, 66, 57],
            BassPrograms: [32, 43],
            PadPrograms: [48, 52, 49],
            MelodyDensity.SwingEighth, BassPattern.Walking,
            0.34f, 0.30f, 0.14f, 1, 0),

        [GenreCategory.Blues] = new(80, 110, ScaleType.Blues,
            MelodyPrograms: [29, 22, 27, 4, 65],
            BassPrograms: [33, 32],
            PadPrograms: [48, 89, 17],
            MelodyDensity.SwingEighth, BassPattern.RootFifth,
            0.30f, 0.32f, 0.14f, 1, 0),

        [GenreCategory.HipHop] = new(72, 94, ScaleType.Minor,
            MelodyPrograms: [4, 0, 11, 80],
            BassPrograms: [38, 39, 35],
            PadPrograms: [91, 88, 89],
            MelodyDensity.Sixteenth, BassPattern.RootFifth,
            0.30f, 0.40f, 0.14f, 1, -1),

        [GenreCategory.Electronic] = new(105, 128, ScaleType.Minor,
            MelodyPrograms: [81, 80, 84, 82, 87],
            BassPrograms: [38, 39],
            PadPrograms: [88, 89, 90, 94],
            MelodyDensity.Sixteenth, BassPattern.FourOnFloor,
            0.26f, 0.36f, 0.14f, 2, -1),

        [GenreCategory.Classical] = new(68, 100, ScaleType.Major,
            MelodyPrograms: [40, 0, 73, 6, 68, 71, 41],
            BassPrograms: [42, 43, 32],
            PadPrograms: [48, 49, 50, 52],
            MelodyDensity.Eighth, BassPattern.Arpeggio,
            0.32f, 0.28f, 0.18f, 2, -1),

        [GenreCategory.Country] = new(88, 116, ScaleType.MajorPentatonic,
            MelodyPrograms: [25, 24, 105, 40, 22],
            BassPrograms: [32, 33],
            PadPrograms: [48, 89, 40],
            MelodyDensity.Eighth, BassPattern.RootFifth,
            0.36f, 0.30f, 0.12f, 1, 0),

        [GenreCategory.Folk] = new(72, 100, ScaleType.Major,
            MelodyPrograms: [24, 105, 22, 21, 73],
            BassPrograms: [32, 43],
            PadPrograms: [48, 49, 89],
            MelodyDensity.Eighth, BassPattern.Drone,
            0.36f, 0.26f, 0.14f, 1, 0),

        [GenreCategory.Reggae] = new(85, 95, ScaleType.Minor,
            MelodyPrograms: [27, 29, 16, 17],
            BassPrograms: [33, 34],
            PadPrograms: [89, 48, 19],
            MelodyDensity.Syncopated, BassPattern.Offbeat,
            0.28f, 0.36f, 0.14f, 1, -1),

        [GenreCategory.Punk] = new(155, 185, ScaleType.MinorPentatonic,
            MelodyPrograms: [30, 29],
            BassPrograms: [33, 34],
            PadPrograms: [89, 90],
            MelodyDensity.Eighth, BassPattern.Pedal,
            0.28f, 0.36f, 0.10f, 0, -1),

        [GenreCategory.Ambient] = new(55, 76, ScaleType.Major,
            MelodyPrograms: [14, 98, 10, 8, 9, 88, 92],
            BassPrograms: [89, 88, 95],
            PadPrograms: [88, 91, 94, 99, 100],
            MelodyDensity.Half, BassPattern.Drone,
            0.28f, 0.22f, 0.18f, 2, -1),

        [GenreCategory.Indie] = new(88, 120, ScaleType.Major,
            MelodyPrograms: [0, 4, 27, 9, 11],
            BassPrograms: [33, 32],
            PadPrograms: [89, 48, 88],
            MelodyDensity.Eighth, BassPattern.RootFifth,
            0.32f, 0.30f, 0.14f, 1, 0)
    };

    public static GenreMusicProfile Get(GenreCategory category)
        => Profiles.TryGetValue(category, out var profile) ? profile : Profiles[GenreCategory.Rock];
}
