namespace Task5.Services;

public class LikesCalculator
{
    public int Calculate(long userSeed, int songIndex, double avgLikes)
    {
        var random = CreateRandom(userSeed, songIndex);
        return ComputeLikes(random, avgLikes);
    }

    private static Random CreateRandom(long userSeed, int songIndex)
    {
        var combined = SeedHelper.ComputeLikesSeed(userSeed, songIndex);
        return new Random(SeedHelper.ToInt32(combined));
    }

    private static int ComputeLikes(Random random, double avgLikes)
    {
        var guaranteed = (int)avgLikes;
        var fraction = avgLikes - guaranteed;
        var extra = random.NextDouble() < fraction ? 1 : 0;
        return guaranteed + extra;
    }
}
