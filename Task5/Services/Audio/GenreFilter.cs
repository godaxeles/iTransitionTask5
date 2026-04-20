namespace Task5.Services.Audio;

public static class GenreFilter
{
    public static void Apply(StereoBuffer buffer, GenreFilterProfile profile)
    {
        if (profile.LowPassCutoffHz > 0f)
            ApplyLowPass(buffer, profile.LowPassCutoffHz);
        if (profile.HighPassCutoffHz > 0f)
            ApplyHighPass(buffer, profile.HighPassCutoffHz);
    }

    private static void ApplyLowPass(StereoBuffer buffer, float cutoffHz)
    {
        var alpha = ComputeAlpha(cutoffHz);
        var prevL = 0f;
        var prevR = 0f;
        for (var i = 0; i < buffer.Left.Length; i++)
        {
            prevL += alpha * (buffer.Left[i] - prevL);
            prevR += alpha * (buffer.Right[i] - prevR);
            buffer.Left[i] = prevL;
            buffer.Right[i] = prevR;
        }
    }

    private static void ApplyHighPass(StereoBuffer buffer, float cutoffHz)
    {
        var alpha = ComputeAlpha(cutoffHz);
        var smoothedL = 0f;
        var smoothedR = 0f;
        for (var i = 0; i < buffer.Left.Length; i++)
        {
            smoothedL += alpha * (buffer.Left[i] - smoothedL);
            smoothedR += alpha * (buffer.Right[i] - smoothedR);
            buffer.Left[i] -= smoothedL;
            buffer.Right[i] -= smoothedR;
        }
    }

    private static float ComputeAlpha(float cutoffHz)
    {
        var dt = 1f / AudioConfig.SampleRate;
        var rc = 1f / (2f * MathF.PI * cutoffHz);
        return dt / (rc + dt);
    }
}
