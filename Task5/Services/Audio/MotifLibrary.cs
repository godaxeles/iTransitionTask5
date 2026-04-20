namespace Task5.Services.Audio;

public static class MotifLibrary
{
    private static readonly int[][] Contours =
    [
        [0, 2, 4, 2, 0, 2, 4, 7],
        [4, 2, 0, 2, 4, 2, 0, -1],
        [0, 4, 2, 0, 4, 2, 0, 2],
        [0, -1, 0, 2, 4, 2, 0, -1],
        [2, 4, 0, 2, -1, 2, 4, 2],
        [0, 2, 0, 4, 2, 0, 2, 4],
        [4, 0, 2, 4, 0, -1, 0, 2],
        [0, 2, 4, 7, 4, 2, 0, -1],
        [0, 0, 2, 2, 4, 4, 2, 0],
        [2, 0, 4, 2, 0, 2, -1, 0],
        [0, 4, 0, 2, 0, 4, 0, -1],
        [-1, 0, 2, 4, 2, 0, -1, 0],
        [0, 2, 4, 5, 4, 2, 0, 2],
        [4, 5, 4, 2, 0, 2, 4, 2],
        [0, 7, 4, 0, -3, 0, 4, 7],
        [0, 4, -3, 4, 0, 7, 4, 0],
        [0, 2, 4, 7, 9, 7, 4, 2],
        [9, 7, 4, 2, 0, 2, 4, 7],
        [0, -3, 0, 4, 7, 4, 0, -1],
        [7, 4, 2, 0, -3, 0, 2, 4],
        [0, 7, 0, 4, 0, 7, 2, 0],
        [2, 9, 4, 2, 0, 4, 7, 4]
    ];

    public static int[] Pick(Random random) => Contours[random.Next(Contours.Length)];

    public static int SampleAt(int[] motif, int subIndex, int subsPerBar)
    {
        var scaled = subIndex * motif.Length / Math.Max(1, subsPerBar);
        return motif[Math.Clamp(scaled, 0, motif.Length - 1)];
    }
}
