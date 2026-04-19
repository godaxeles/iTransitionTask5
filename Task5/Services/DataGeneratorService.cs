using Bogus;
using Task5.Models;

namespace Task5.Services;

public class DataGeneratorService(LocaleDataService localeDataService, LikesCalculator likesCalculator)
{
    public List<SongRecord> GeneratePage(GenerationParams parameters)
    {
        var localeData = localeDataService.GetLocaleData(parameters.Locale);
        var faker = CreateFaker(parameters);
        var generator = new SongContentGenerator(localeData, faker);
        return BuildSongList(parameters, generator);
    }

    private static Faker CreateFaker(GenerationParams parameters)
    {
        var pageSeed = SeedHelper.ComputePageSeed(parameters.Seed, parameters.Page);
        var intSeed = SeedHelper.ToInt32(pageSeed);
        return new Faker(parameters.Locale) { Random = new Randomizer(intSeed) };
    }

    private List<SongRecord> BuildSongList(GenerationParams parameters, SongContentGenerator generator)
    {
        var pageSize = parameters.SafePageSize;
        var startIndex = (parameters.Page - 1) * pageSize + 1;
        var songs = new List<SongRecord>(pageSize);

        for (var i = 0; i < pageSize; i++)
            songs.Add(BuildSong(parameters, generator, startIndex + i));

        return songs;
    }

    private SongRecord BuildSong(GenerationParams parameters, SongContentGenerator generator, int songIndex)
    {
        return new SongRecord
        {
            Index = songIndex,
            Title = generator.GenerateTitle(),
            Artist = generator.GenerateArtist(),
            Album = generator.GenerateAlbum(),
            Genre = generator.GenerateGenre(),
            Review = generator.GenerateReview(),
            Lyrics = generator.GenerateLyrics(),
            Likes = likesCalculator.Calculate(parameters.Seed, songIndex, parameters.Likes)
        };
    }
}
