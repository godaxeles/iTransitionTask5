using Task5.Models;

namespace Task5.Services.Audio;

public static class GenreOscillator
{
    private const float TwoPi = 2f * MathF.PI;

    public static float Evaluate(GenreCategory category, float freq, float t) => category switch
    {
        GenreCategory.Rock => RawSaw(freq, t),
        GenreCategory.Metal => MetalSawWithSub(freq, t),
        GenreCategory.Pop => PianoClean(freq, t),
        GenreCategory.Jazz => PianoClean(freq, t),
        GenreCategory.Blues => RawSaw(freq, t),
        GenreCategory.HipHop => PianoClean(freq, t),
        GenreCategory.Electronic => RawSaw(freq, t),
        GenreCategory.Classical => StringsSaw(freq, t),
        GenreCategory.Country => TriangleWave(freq, t),
        GenreCategory.Folk => FolkFormant(freq, t),
        GenreCategory.Reggae => SquareWave(freq, t),
        GenreCategory.Punk => SquareWave(freq, t),
        GenreCategory.Ambient => PadBell(freq, t),
        GenreCategory.Indie => TriangleWave(freq, t),
        _ => RawSaw(freq, t)
    };

    private static float RawSaw(float freq, float t)
    {
        var phase = t * freq;
        phase -= MathF.Floor(phase);
        return 0.70f * (2f * phase - 1f);
    }

    private static float SquareWave(float freq, float t)
    {
        var phase = t * freq;
        phase -= MathF.Floor(phase);
        return phase < 0.5f ? 0.60f : -0.60f;
    }

    private static float TriangleWave(float freq, float t)
    {
        var phase = t * freq;
        phase -= MathF.Floor(phase);
        return 0.65f * (phase < 0.5f ? (4f * phase - 1f) : (3f - 4f * phase));
    }

    private static float PianoClean(float freq, float t)
    {
        var phase = TwoPi * freq * t;
        return 0.55f * (
              MathF.Sin(phase)
            + 0.30f * MathF.Sin(phase * 2)
            + 0.10f * MathF.Sin(phase * 3)
            + 0.04f * MathF.Sin(phase * 4));
    }

    private static float StringsSaw(float freq, float t)
    {
        var phase = TwoPi * freq * t;
        var sum = 0f;
        for (var n = 1; n <= 10; n++)
            sum += MathF.Sin(phase * n) / n;
        return 0.42f * sum;
    }

    private static float MetalSawWithSub(float freq, float t)
    {
        var phase = t * freq;
        phase -= MathF.Floor(phase);
        var saw = 2f * phase - 1f;
        var subPhase = TwoPi * freq * 0.5f * t;
        var sub = MathF.Sin(subPhase);
        return 0.65f * saw + 0.30f * sub;
    }

    private static float PadBell(float freq, float t)
    {
        var phase = TwoPi * freq * t;
        return 0.45f * (
              MathF.Sin(phase)
            + 0.19f * MathF.Sin(phase * 2f)
            + 0.52f * MathF.Sin(phase * 3.03f)
            + 0.18f * MathF.Sin(phase * 5.01f));
    }

    private static float FolkFormant(float freq, float t)
    {
        var phase = TwoPi * freq * t;
        return 0.50f * (
              MathF.Sin(phase)
            + 0.45f * MathF.Sin(phase * 2)
            + 0.30f * MathF.Sin(phase * 3)
            + 0.97f * MathF.Sin(phase * 4)
            + 0.20f * MathF.Sin(phase * 5));
    }
}
