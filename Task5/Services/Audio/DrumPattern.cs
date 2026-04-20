using Task5.Models;

namespace Task5.Services.Audio;

public enum DrumStyle
{
    None,
    RockBeat,
    PopBeat,
    HipHopBeat,
    FourOnFloor,
    Punk,
    Reggae,
    Jazz,
    Shuffle,
    BlastBeat
}

public static class DrumStyleRegistry
{
    private static readonly Dictionary<GenreCategory, DrumStyle> Styles = new()
    {
        [GenreCategory.Rock] = DrumStyle.RockBeat,
        [GenreCategory.Metal] = DrumStyle.BlastBeat,
        [GenreCategory.Pop] = DrumStyle.PopBeat,
        [GenreCategory.Jazz] = DrumStyle.Jazz,
        [GenreCategory.Blues] = DrumStyle.Shuffle,
        [GenreCategory.HipHop] = DrumStyle.HipHopBeat,
        [GenreCategory.Electronic] = DrumStyle.FourOnFloor,
        [GenreCategory.Classical] = DrumStyle.None,
        [GenreCategory.Country] = DrumStyle.PopBeat,
        [GenreCategory.Folk] = DrumStyle.None,
        [GenreCategory.Reggae] = DrumStyle.Reggae,
        [GenreCategory.Punk] = DrumStyle.Punk,
        [GenreCategory.Ambient] = DrumStyle.None,
        [GenreCategory.Indie] = DrumStyle.PopBeat
    };

    public static DrumStyle For(GenreCategory category)
        => Styles.TryGetValue(category, out var style) ? style : DrumStyle.None;
}
