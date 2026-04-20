namespace Task5.Services.Audio;

public static class ChordProgressionProvider
{
    private static readonly int[][] MajorProgressions =
    [
        [0, 4, 5, 3],
        [0, 3, 4, 0],
        [0, 5, 3, 4],
        [0, 3, 0, 4],
        [5, 3, 0, 4],
        [0, 2, 3, 4],
        [0, 5, 1, 4],
        [0, 4, 5, 2],
        [3, 4, 5, 0],
        [0, 2, 5, 4],
        [0, 4, 3, 0],
        [0, 5, 4, 3]
    ];

    private static readonly int[][] MinorProgressions =
    [
        [0, 5, 3, 6],
        [0, 3, 4, 0],
        [0, 6, 5, 4],
        [0, 3, 6, 4],
        [0, 5, 6, 4],
        [0, 6, 3, 0],
        [5, 6, 0, 4],
        [0, 2, 5, 4],
        [0, 3, 0, 4],
        [0, 4, 3, 0],
        [3, 0, 6, 4],
        [0, 5, 4, 0]
    ];

    public static int[] GetProgression(bool isMajor, Random random)
    {
        var progressions = isMajor ? MajorProgressions : MinorProgressions;
        return progressions[random.Next(progressions.Length)];
    }
}
