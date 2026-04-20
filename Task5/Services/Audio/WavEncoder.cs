namespace Task5.Services.Audio;

public class WavEncoder
{
    private const short PcmFormat = 1;

    private const short Channels = 2;

    private const short BitsPerSample = 16;

    private const int BytesPerSample = BitsPerSample / 8;

    public byte[] Encode(float[] left, float[] right)
    {
        var count = Math.Min(left.Length, right.Length);
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        WriteHeader(writer, count);
        WriteSamples(writer, left, right, count);
        return stream.ToArray();
    }

    private static void WriteHeader(BinaryWriter writer, int frameCount)
    {
        var dataSize = frameCount * Channels * BytesPerSample;
        var byteRate = AudioConfig.SampleRate * Channels * BytesPerSample;
        var blockAlign = (short)(Channels * BytesPerSample);

        writer.Write("RIFF"u8);
        writer.Write(36 + dataSize);
        writer.Write("WAVE"u8);
        writer.Write("fmt "u8);
        writer.Write(16);
        writer.Write(PcmFormat);
        writer.Write(Channels);
        writer.Write(AudioConfig.SampleRate);
        writer.Write(byteRate);
        writer.Write(blockAlign);
        writer.Write(BitsPerSample);
        writer.Write("data"u8);
        writer.Write(dataSize);
    }

    private static void WriteSamples(BinaryWriter writer, float[] left, float[] right, int count)
    {
        for (var i = 0; i < count; i++)
        {
            writer.Write(ToPcm(left[i]));
            writer.Write(ToPcm(right[i]));
        }
    }

    private static short ToPcm(float sample)
    {
        var clamped = Math.Clamp(sample, -1f, 1f);
        return (short)(clamped * short.MaxValue);
    }
}
