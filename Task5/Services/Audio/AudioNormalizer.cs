namespace Task5.Services.Audio;

public static class AudioNormalizer
{
    private const float TargetPeak = 0.88f;

    private const float MinPeakToNormalize = 0.01f;

    public static void NormalizeToTargetPeak(StereoBuffer buffer)
    {
        var peak = FindPeak(buffer);
        if (peak < MinPeakToNormalize)
            return;

        var scale = TargetPeak / peak;
        ApplyScale(buffer, scale);
    }

    private static float FindPeak(StereoBuffer buffer)
    {
        var peak = 0f;
        foreach (var s in buffer.Left)
        {
            var abs = MathF.Abs(s);
            if (abs > peak) peak = abs;
        }
        foreach (var s in buffer.Right)
        {
            var abs = MathF.Abs(s);
            if (abs > peak) peak = abs;
        }
        return peak;
    }

    private static void ApplyScale(StereoBuffer buffer, float scale)
    {
        for (var i = 0; i < buffer.Left.Length; i++)
            buffer.Left[i] *= scale;
        for (var i = 0; i < buffer.Right.Length; i++)
            buffer.Right[i] *= scale;
    }
}
