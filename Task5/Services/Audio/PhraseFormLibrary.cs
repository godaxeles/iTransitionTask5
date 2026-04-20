namespace Task5.Services.Audio;

public static class PhraseFormLibrary
{
    private static readonly int[][] Forms =
    [
        [0, 0, 0, 0],
        [0, 4, 0, 0],
        [0, 0, -3, 0],
        [0, 0, 7, 0],
        [0, -4, 0, 0],
        [0, 4, 4, 0],
        [0, 0, -7, 0],
        [0, 7, 0, -3],
        [0, 2, -2, 0],
        [-3, 0, 2, 0]
    ];

    public static int[] Pick(Random random) => Forms[random.Next(Forms.Length)];
}
