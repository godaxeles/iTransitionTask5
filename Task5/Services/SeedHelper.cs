namespace Task5.Services;

public static class SeedHelper
{
    private const long PageMultiplier = 2_147_483_647L;

    private const long LikesMultiplier = 1_000_003L;

    private const long AudioTrackMultiplier = 777_991L;

    private const long AudioIndexMultiplier = 131_071L;

    private const long CoverTrackMultiplier = 999_983L;

    private const long CoverIndexMultiplier = 7_919L;

    public static long ComputePageSeed(long userSeed, int page)
        => userSeed * PageMultiplier + page;

    public static long ComputeLikesSeed(long userSeed, int songIndex)
        => userSeed * LikesMultiplier + songIndex;

    public static long ComputeAudioSeed(long userSeed, int songIndex)
        => userSeed * AudioTrackMultiplier + songIndex * AudioIndexMultiplier;

    public static long ComputeCoverSeed(long userSeed, int songIndex)
        => userSeed * CoverTrackMultiplier + songIndex * CoverIndexMultiplier;

    public static int ToInt32(long seed)
        => (int)(seed ^ (seed >> 32));
}
