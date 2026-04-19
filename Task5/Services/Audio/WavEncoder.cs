namespace Task5.Services.Audio;

public class WavEncoder
{
    private const short PcmFormat = 1;

    private const short Channels = 1;

    private const short BitsPerSample = 16;

    private const int BytesPerSample = BitsPerSample / 8;

    public byte[] Encode(float[] samples)
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        WriteHeader(writer, samples.Length);
        WriteSamples(writer, samples);
        return stream.ToArray();
    }

    private static void WriteHeader(BinaryWriter writer, int sampleCount)
    {
        var dataSize = sampleCount * BytesPerSample;
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

    private static void WriteSamples(BinaryWriter writer, float[] samples)
    {
        foreach (var sample in samples)
        {
            var clamped = Math.Clamp(sample, -1f, 1f);
            writer.Write((short)(clamped * short.MaxValue));
        }
    }
}
