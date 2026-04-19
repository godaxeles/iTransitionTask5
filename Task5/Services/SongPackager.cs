using System.IO.Compression;
using Task5.Models;

namespace Task5.Services;

public class SongPackager(
    DataGeneratorService dataGeneratorService,
    AudioGeneratorService audioGeneratorService,
    Mp3Encoder mp3Encoder)
{
    public byte[] CreateZip(GenerationParams parameters)
    {
        var songs = dataGeneratorService.GeneratePage(parameters);
        return BuildArchive(songs, parameters.Seed);
    }

    private byte[] BuildArchive(List<SongRecord> songs, long seed)
    {
        using var zipStream = new MemoryStream();
        WriteEntries(zipStream, songs, seed);
        return zipStream.ToArray();
    }

    private void WriteEntries(MemoryStream zipStream, List<SongRecord> songs, long seed)
    {
        using var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, leaveOpen: true);
        foreach (var song in songs)
            WriteSongEntry(archive, song, seed);
    }

    private void WriteSongEntry(ZipArchive archive, SongRecord song, long seed)
    {
        var wavBytes = audioGeneratorService.Generate(seed, song.Index);
        var mp3Bytes = mp3Encoder.Encode(wavBytes);
        var entryName = BuildEntryName(song);

        var entry = archive.CreateEntry(entryName, CompressionLevel.Fastest);
        using var entryStream = entry.Open();
        entryStream.Write(mp3Bytes, 0, mp3Bytes.Length);
    }

    private static string BuildEntryName(SongRecord song)
    {
        var raw = $"{song.Index:D2} - {song.Title} - {song.Album} - {song.Artist}.mp3";
        return Sanitize(raw);
    }

    private static string Sanitize(string name)
    {
        var invalid = Path.GetInvalidFileNameChars();
        var chars = name.Select(c => invalid.Contains(c) ? '_' : c).ToArray();
        return new string(chars);
    }
}
