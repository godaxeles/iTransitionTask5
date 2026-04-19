namespace Task5.Services;

public static class SeedHelper
{
    public static long ComputePageSeed(long userSeed, int page)
        => userSeed * 31L + page;

    public static long ComputeLikesSeed(long userSeed, int songIndex)
        => userSeed * 1_000_003L + songIndex;

    public static int ToInt32(long seed)
        => (int)(seed ^ (seed >> 32));
}
