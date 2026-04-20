namespace Task5.Services.Audio;

public static class ScaleProvider
{
    private static readonly Dictionary<ScaleType, int[]> Scales = new()
    {
        [ScaleType.Major] = [0, 2, 4, 5, 7, 9, 11],
        [ScaleType.Minor] = [0, 2, 3, 5, 7, 8, 10],
        [ScaleType.MajorPentatonic] = [0, 2, 4, 7, 9],
        [ScaleType.MinorPentatonic] = [0, 3, 5, 7, 10],
        [ScaleType.Blues] = [0, 3, 5, 6, 7, 10],
        [ScaleType.Dorian] = [0, 2, 3, 5, 7, 9, 10],
        [ScaleType.Phrygian] = [0, 1, 3, 5, 7, 8, 10]
    };

    public static int[] GetScale(ScaleType type)
        => Scales.TryGetValue(type, out var scale) ? scale : Scales[ScaleType.Major];

    public static bool IsMajorLike(ScaleType type)
        => type is ScaleType.Major or ScaleType.MajorPentatonic;
}
