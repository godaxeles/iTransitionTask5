namespace Task5.Services.Audio;

internal static class ScaleProvider
{
    private static readonly int[] MajorIntervals = [0, 2, 4, 5, 7, 9, 11];
    
    private static readonly int[] MinorIntervals = [0, 2, 3, 5, 7, 8, 10];

    public static int[] GetScale(bool isMajor)
        => isMajor ? MajorIntervals : MinorIntervals;
}
