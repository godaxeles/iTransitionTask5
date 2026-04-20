using Bogus;
using Task5.Models;

namespace Task5.Services;

public class SongContentGenerator(LocaleData localeData, Faker faker)
{
    private const float SingleProbability = 0.2f;

    private const float SoloArtistProbability = 0.5f;

    private const int LyricLineCount = 6;

    private static readonly GenreCategory[] AllCategories = Enum.GetValues<GenreCategory>();

    public string GenerateTitle()
    {
        var adjective = faker.PickRandom(localeData.TitleAdjectives);
        var noun = faker.PickRandom(localeData.TitleNouns);
        return $"{adjective} {noun}";
    }

    public string GenerateArtist()
    {
        return faker.Random.Bool(SoloArtistProbability)
            ? GenerateSoloArtist()
            : GenerateBandName();
    }

    public string GenerateAlbum()
    {
        return faker.Random.Bool(SingleProbability)
            ? "Single"
            : GenerateAlbumTitle();
    }

    public GenreCategory GenerateGenreCategory()
        => faker.PickRandom(AllCategories);

    public string LocalizeGenre(GenreCategory category)
    {
        var index = (int)category;
        if (localeData.Genres.Length == 0) return category.ToString();
        return index < localeData.Genres.Length ? localeData.Genres[index] : localeData.Genres[0];
    }

    public string GenerateReview()
    {
        var sentences = faker.PickRandom(localeData.ReviewSentences, 3);
        return string.Join(" ", sentences);
    }

    public List<string> GenerateLyrics()
    {
        return faker.PickRandom(localeData.LyricLines, LyricLineCount).ToList();
    }

    private string GenerateSoloArtist()
        => faker.Name.FullName();

    private string GenerateBandName()
    {
        var prefix = faker.PickRandom(localeData.BandPrefixes);
        var noun = faker.PickRandom(localeData.TitleNouns);
        var suffix = faker.PickRandom(localeData.BandSuffixes);
        return $"{prefix} {noun} {suffix}";
    }

    private string GenerateAlbumTitle()
    {
        var adjective = faker.PickRandom(localeData.TitleAdjectives);
        var word = faker.PickRandom(localeData.AlbumWords);
        return $"{adjective} {word}";
    }
}
