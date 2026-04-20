using Task5.Models;

namespace Task5.Services.Audio;

public class GenreAudioProcessor
{
    public void Process(StereoBuffer buffer, GenreCategory category)
    {
        switch (category)
        {
            case GenreCategory.Rock:
                ApplyOverdrive(buffer, drive: 3.0f, mix: 0.80f);
                ApplySoftKneeHardClip(buffer, threshold: 0.45f, knee: 0.12f);
                ApplyHighShelf(buffer, intensity: 0.65f);
                ApplyHaasStereo(buffer, delayMs: 12f);
                break;
            case GenreCategory.Metal:
                ApplyOverdrive(buffer, drive: 10.0f, mix: 1.0f);
                ApplySoftKneeHardClip(buffer, threshold: 0.25f, knee: 0.10f);
                ApplyHighShelf(buffer, intensity: 0.65f);
                ApplyHaasStereo(buffer, delayMs: 10f);
                break;
            case GenreCategory.Punk:
                ApplyOverdrive(buffer, drive: 5.0f, mix: 0.95f);
                ApplySoftKneeHardClip(buffer, threshold: 0.35f, knee: 0.10f);
                ApplyHighShelf(buffer, intensity: 0.5f);
                ApplyHaasStereo(buffer, delayMs: 11f);
                break;
            case GenreCategory.Blues:
                ApplyOverdrive(buffer, drive: 1.8f, mix: 0.55f);
                ApplyFrequencyVibrato(buffer, rateHz: 5.5f, depthFraction: 0.02f);
                ApplyWah(buffer, rateHz: 4.5f, depth: 0.45f);
                ApplyHaasStereo(buffer, delayMs: 14f);
                break;
            case GenreCategory.Pop:
                ApplyCompression(buffer, threshold: 0.45f, ratio: 2.8f);
                ApplyHighShelf(buffer, intensity: 0.25f);
                ApplyHaasStereo(buffer, delayMs: 12f);
                break;
            case GenreCategory.Jazz:
                ApplyLowShelf(buffer, intensity: 0.3f);
                ApplyHaasStereo(buffer, delayMs: 16f);
                break;
            case GenreCategory.Classical:
                ApplyStereoWidening(buffer, amount: 0.25f);
                ApplyHaasStereo(buffer, delayMs: 18f);
                break;
            case GenreCategory.Country:
                ApplyLowShelf(buffer, intensity: 0.2f);
                ApplyHaasStereo(buffer, delayMs: 12f);
                break;
            case GenreCategory.Folk:
                ApplyStereoWidening(buffer, amount: 0.15f);
                ApplyHaasStereo(buffer, delayMs: 14f);
                break;
            case GenreCategory.HipHop:
                ApplyCompression(buffer, threshold: 0.40f, ratio: 3.5f);
                ApplyLowShelf(buffer, intensity: 0.5f);
                ApplyHaasStereo(buffer, delayMs: 10f);
                break;
            case GenreCategory.Electronic:
                ApplyBitCrush(buffer, bitDepth: 10);
                ApplyHighShelf(buffer, intensity: 0.3f);
                ApplyHaasStereo(buffer, delayMs: 13f);
                break;
            case GenreCategory.Reggae:
                ApplyLowShelf(buffer, intensity: 0.35f);
                ApplyWah(buffer, rateHz: 2.2f, depth: 0.20f);
                ApplyHaasStereo(buffer, delayMs: 15f);
                break;
            case GenreCategory.Ambient:
                ApplyReverb(buffer, decay: 0.65f);
                ApplyStereoWidening(buffer, amount: 0.45f);
                ApplyHaasStereo(buffer, delayMs: 22f);
                break;
            case GenreCategory.Indie:
                ApplyCompression(buffer, threshold: 0.55f, ratio: 2.2f);
                ApplyHaasStereo(buffer, delayMs: 13f);
                break;
        }
    }

    private static void ApplyOverdrive(StereoBuffer buffer, float drive, float mix)
    {
        for (var i = 0; i < buffer.Left.Length; i++)
        {
            buffer.Left[i] = BlendOverdrive(buffer.Left[i], drive, mix);
            buffer.Right[i] = BlendOverdrive(buffer.Right[i], drive, mix);
        }
    }

    private static float BlendOverdrive(float sample, float drive, float mix)
    {
        var saturated = MathF.Tanh(sample * drive);
        return sample * (1f - mix) + saturated * mix;
    }

    private static void ApplySoftKneeHardClip(StereoBuffer buffer, float threshold, float knee)
    {
        for (var i = 0; i < buffer.Left.Length; i++)
        {
            buffer.Left[i] = SoftKneeClip(buffer.Left[i], threshold, knee);
            buffer.Right[i] = SoftKneeClip(buffer.Right[i], threshold, knee);
        }
    }

    private static float SoftKneeClip(float sample, float threshold, float knee)
    {
        var abs = MathF.Abs(sample);
        if (abs <= threshold) return sample;
        var over = abs - threshold;
        var softened = threshold + knee * MathF.Tanh(over * 10f);
        return MathF.Sign(sample) * softened;
    }

    private static void ApplyFrequencyVibrato(StereoBuffer buffer, float rateHz, float depthFraction)
    {
        var sr = AudioConfig.SampleRate;
        var scratchL = new float[buffer.Left.Length];
        var scratchR = new float[buffer.Right.Length];
        Array.Copy(buffer.Left, scratchL, buffer.Left.Length);
        Array.Copy(buffer.Right, scratchR, buffer.Right.Length);

        for (var i = 0; i < buffer.Left.Length; i++)
        {
            var t = (float)i / sr;
            var modulation = MathF.Sin(2f * MathF.PI * rateHz * t) * depthFraction;
            var sourceIndex = i + modulation * sr * 0.01f;
            buffer.Left[i] = Sample(scratchL, sourceIndex);
            buffer.Right[i] = Sample(scratchR, sourceIndex);
        }
    }

    private static float Sample(float[] buffer, float index)
    {
        var i0 = (int)MathF.Floor(index);
        var frac = index - i0;
        if (i0 < 0) i0 = 0;
        if (i0 >= buffer.Length - 1) return buffer[^1];
        return buffer[i0] * (1f - frac) + buffer[i0 + 1] * frac;
    }

    private static void ApplyHaasStereo(StereoBuffer buffer, float delayMs)
    {
        var delaySamples = (int)(delayMs * 0.001f * AudioConfig.SampleRate);
        if (delaySamples <= 0 || delaySamples >= buffer.Right.Length)
            return;

        for (var i = buffer.Right.Length - 1; i >= delaySamples; i--)
            buffer.Right[i] = buffer.Right[i - delaySamples];

        for (var i = 0; i < delaySamples; i++)
            buffer.Right[i] = 0f;
    }

    private static void ApplyHighShelf(StereoBuffer buffer, float intensity)
    {
        var prevL = 0f;
        var prevR = 0f;
        for (var i = 0; i < buffer.Left.Length; i++)
        {
            var diffL = buffer.Left[i] - prevL;
            var diffR = buffer.Right[i] - prevR;
            prevL = buffer.Left[i];
            prevR = buffer.Right[i];
            buffer.Left[i] += diffL * intensity;
            buffer.Right[i] += diffR * intensity;
        }
    }

    private static void ApplyLowShelf(StereoBuffer buffer, float intensity)
    {
        var smoothL = 0f;
        var smoothR = 0f;
        const float alpha = 0.08f;
        for (var i = 0; i < buffer.Left.Length; i++)
        {
            smoothL += alpha * (buffer.Left[i] - smoothL);
            smoothR += alpha * (buffer.Right[i] - smoothR);
            buffer.Left[i] += smoothL * intensity;
            buffer.Right[i] += smoothR * intensity;
        }
    }

    private static void ApplyWah(StereoBuffer buffer, float rateHz, float depth)
    {
        const float baseAlpha = 0.12f;
        var smoothL = 0f;
        var smoothR = 0f;
        for (var i = 0; i < buffer.Left.Length; i++)
        {
            var t = (float)i / AudioConfig.SampleRate;
            var modulation = 0.5f + 0.5f * MathF.Sin(2f * MathF.PI * rateHz * t);
            var alpha = baseAlpha + depth * modulation;
            smoothL += alpha * (buffer.Left[i] - smoothL);
            smoothR += alpha * (buffer.Right[i] - smoothR);
            buffer.Left[i] = buffer.Left[i] * (1f - depth) + smoothL * (depth * 2f);
            buffer.Right[i] = buffer.Right[i] * (1f - depth) + smoothR * (depth * 2f);
        }
    }

    private static void ApplyCompression(StereoBuffer buffer, float threshold, float ratio)
    {
        for (var i = 0; i < buffer.Left.Length; i++)
        {
            buffer.Left[i] = Compress(buffer.Left[i], threshold, ratio);
            buffer.Right[i] = Compress(buffer.Right[i], threshold, ratio);
        }
    }

    private static float Compress(float sample, float threshold, float ratio)
    {
        var absSample = MathF.Abs(sample);
        if (absSample <= threshold) return sample;
        var over = absSample - threshold;
        var compressed = threshold + over / ratio;
        return MathF.Sign(sample) * compressed;
    }

    private static void ApplyBitCrush(StereoBuffer buffer, int bitDepth)
    {
        var levels = MathF.Pow(2, bitDepth);
        var inv = 1f / levels;
        for (var i = 0; i < buffer.Left.Length; i++)
        {
            buffer.Left[i] = MathF.Round(buffer.Left[i] * levels) * inv;
            buffer.Right[i] = MathF.Round(buffer.Right[i] * levels) * inv;
        }
    }

    private static void ApplyReverb(StereoBuffer buffer, float decay)
    {
        var delaySamples = AudioConfig.SampleRate / 12;
        for (var i = delaySamples; i < buffer.Left.Length; i++)
        {
            buffer.Left[i] += buffer.Left[i - delaySamples] * decay * 0.35f;
            buffer.Right[i] += buffer.Right[i - delaySamples] * decay * 0.35f;
        }
    }

    private static void ApplyStereoWidening(StereoBuffer buffer, float amount)
    {
        for (var i = 0; i < buffer.Left.Length; i++)
        {
            var mid = (buffer.Left[i] + buffer.Right[i]) * 0.5f;
            var side = (buffer.Left[i] - buffer.Right[i]) * 0.5f;
            var widenedSide = side * (1f + amount);
            buffer.Left[i] = mid + widenedSide;
            buffer.Right[i] = mid - widenedSide;
        }
    }
}
