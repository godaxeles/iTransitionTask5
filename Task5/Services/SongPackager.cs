using System.IO.Compression;
using Task5.Models;

namespace Task5.Services;

public class SongPackager(
    DataGeneratorService dataGeneratorService,
    AudioGeneratorService audioGeneratorService,
    Mp3Encoder mp3Encoder)
{
    private static readonly char[] ExtraInvalidFileChars = ['/', '\\'];

    public async Task<byte[]> CreateZipAsync(GenerationParams parameters, CancellationToken cancellationToken = default)
    {
        var songs = dataGeneratorService.GeneratePage(parameters);
        return await BuildArchiveAsync(songs, parameters.Seed, cancellationToken);
    }

    private async Task<byte[]> BuildArchiveAsync(List<SongRecord> songs, long seed, CancellationToken cancellationToken)
    {
        using var zipStream = new MemoryStream();
        await WriteEntriesAsync(zipStream, songs, seed, cancellationToken);
        return zipStream.ToArray();
    }

    private async Task WriteEntriesAsync(MemoryStream zipStream, List<SongRecord> songs, long seed, CancellationToken cancellationToken)
    {
        using var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, leaveOpen: true);
        foreach (var song in songs)
            await WriteSongEntryAsync(archive, song, seed, cancellationToken);
    }

    private async Task WriteSongEntryAsync(ZipArchive archive, SongRecord song, long seed, CancellationToken cancellationToken)
    {
        var wavBytes = audioGeneratorService.Generate(seed, song.Index);
        var mp3Bytes = await mp3Encoder.EncodeAsync(wavBytes, cancellationToken);
        var entryName = BuildEntryName(song);

        var entry = archive.CreateEntry(entryName, CompressionLevel.Fastest);
        await using var entryStream = entry.Open();
        await entryStream.WriteAsync(mp3Bytes, cancellationToken);
    }

    private static string BuildEntryName(SongRecord song)
    {
        var raw = $"{song.Index:D2} - {song.Title} - {song.Album} - {song.Artist}.mp3";
        return Sanitize(raw);
    }

    private static string Sanitize(string name)
    {
        var invalid = Path.GetInvalidFileNameChars().Concat(ExtraInvalidFileChars).ToHashSet();
        var chars = name.Select(c => invalid.Contains(c) ? '_' : c).ToArray();
        return new string(chars);
    }
}
