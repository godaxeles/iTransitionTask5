using Task5.Models;

namespace Task5.Services.Audio;

public record GenreFilterProfile(float LowPassCutoffHz, float HighPassCutoffHz);

public static class GenreFilterRegistry
{
    private const float NoCutoff = 0f;

    private static readonly Dictionary<GenreCategory, GenreFilterProfile> Filters = new()
    {
        [GenreCategory.Rock] = new(3000f, NoCutoff),
        [GenreCategory.Metal] = new(5000f, 80f),
        [GenreCategory.Pop] = new(NoCutoff, NoCutoff),
        [GenreCategory.Jazz] = new(NoCutoff, NoCutoff),
        [GenreCategory.Blues] = new(1500f, NoCutoff),
        [GenreCategory.HipHop] = new(2500f, NoCutoff),
        [GenreCategory.Electronic] = new(NoCutoff, NoCutoff),
        [GenreCategory.Classical] = new(NoCutoff, NoCutoff),
        [GenreCategory.Country] = new(NoCutoff, NoCutoff),
        [GenreCategory.Folk] = new(NoCutoff, NoCutoff),
        [GenreCategory.Reggae] = new(2000f, NoCutoff),
        [GenreCategory.Punk] = new(NoCutoff, 300f),
        [GenreCategory.Ambient] = new(NoCutoff, NoCutoff),
        [GenreCategory.Indie] = new(NoCutoff, NoCutoff)
    };

    public static GenreFilterProfile For(GenreCategory category)
        => Filters.TryGetValue(category, out var profile) ? profile : Filters[GenreCategory.Rock];
}
