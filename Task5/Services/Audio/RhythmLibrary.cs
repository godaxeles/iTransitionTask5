namespace Task5.Services.Audio;

public static class RhythmLibrary
{
    private static readonly Dictionary<int, bool[][]> ByLength = new()
    {
        [2] =
        [
            [true, true],
            [true, false]
        ],
        [4] =
        [
            [true, true, true, true],
            [true, false, true, true],
            [true, true, false, true],
            [true, true, true, false]
        ],
        [8] =
        [
            [true, true, true, true, true, true, true, true],
            [true, false, true, true, true, false, true, true],
            [true, true, false, true, true, true, false, true],
            [true, false, true, false, true, true, true, true],
            [true, true, true, false, true, true, false, true],
            [true, true, true, true, false, true, true, true]
        ],
        [16] =
        [
            [true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true],
            [true, false, true, true, true, false, true, true, true, false, true, true, true, false, true, true],
            [true, true, true, false, true, true, false, true, true, true, true, false, true, true, false, true],
            [true, true, false, true, true, false, true, true, true, true, false, true, true, false, true, true]
        ]
    };

    public static bool[] Pick(int length, Random random)
    {
        if (!ByLength.TryGetValue(length, out var set))
            return Enumerable.Repeat(true, length).ToArray();
        return set[random.Next(set.Length)];
    }
}
