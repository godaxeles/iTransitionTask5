using NAudio.Lame;
using NAudio.Wave;

namespace Task5.Services;

public class Mp3Encoder
{
    public byte[] Encode(byte[] wavBytes)
    {
        using var wavStream = new MemoryStream(wavBytes);
        using var reader = new WaveFileReader(wavStream);
        using var mp3Stream = new MemoryStream();
        EncodeToStream(reader, mp3Stream);
        return mp3Stream.ToArray();
    }

    private static void EncodeToStream(WaveFileReader reader, MemoryStream destination)
    {
        using var writer = new LameMP3FileWriter(destination, reader.WaveFormat, LAMEPreset.STANDARD);
        reader.CopyTo(writer);
    }
}
