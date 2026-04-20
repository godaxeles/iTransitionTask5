namespace Task5.Services.Audio;

public static class InstrumentSynthesizer
{
    private const float TwoPi = 2f * MathF.PI;

    public static float Evaluate(Instrument instrument, float freq, float t) => instrument switch
    {
        Instrument.Piano => PianoTone(freq, t),
        Instrument.Strings => StringsSaw(freq, t),
        Instrument.Pad => PadSineStack(freq, t),
        Instrument.Pluck => PluckTone(freq, t),
        Instrument.Bell => FmBell(freq, t),
        Instrument.Flute => FluteTone(freq, t),
        Instrument.Brass => BrassTone(freq, t),
        Instrument.ElectricLead => RawSaw(freq, t),
        Instrument.BassGuitar => BassGuitarTone(freq, t),
        Instrument.SubBass => PureSine(freq, t),
        Instrument.UprightBass => UprightBassTone(freq, t),
        Instrument.Drone => DroneTone(freq, t),
        Instrument.NoiseSweep => NoiseTonal(freq, t),
        _ => PureSine(freq, t)
    };

    private static float PureSine(float freq, float t)
        => 0.55f * MathF.Sin(TwoPi * freq * t);

    private static float PadSineStack(float freq, float t)
    {
        var phase = TwoPi * freq * t;
        return 0.38f * (
              MathF.Sin(phase)
            + 0.45f * MathF.Sin(phase * 2)
            + 0.20f * MathF.Sin(phase * 3));
    }

    private static float DroneTone(float freq, float t)
    {
        var phase = TwoPi * freq * t;
        return 0.42f * (
              MathF.Sin(phase)
            + 0.55f * MathF.Sin(phase * 0.5f)
            + 0.28f * MathF.Sin(phase * 2));
    }

    private static float FluteTone(float freq, float t)
    {
        var phase = TwoPi * freq * t;
        return 0.52f * (MathF.Sin(phase) + 0.08f * MathF.Sin(phase * 2));
    }

    private static float PianoTone(float freq, float t)
    {
        var phase = TwoPi * freq * t;
        return 0.48f * (
              MathF.Sin(phase)
            + 0.40f * MathF.Sin(phase * 2.001f)
            + 0.22f * MathF.Sin(phase * 3.004f)
            + 0.11f * MathF.Sin(phase * 4.009f)
            + 0.05f * MathF.Sin(phase * 5.016f));
    }

    private static float PluckTone(float freq, float t)
    {
        var phase = TwoPi * freq * t;
        return 0.52f * (
              MathF.Sin(phase)
            + 0.42f * MathF.Sin(phase * 2)
            + 0.28f * MathF.Sin(phase * 3)
            + 0.15f * MathF.Sin(phase * 4)
            + 0.08f * MathF.Sin(phase * 5));
    }

    private static float FmBell(float freq, float t)
    {
        var modulatorPhase = TwoPi * freq * 1.414f * t;
        var modulator = MathF.Sin(modulatorPhase);
        var carrierPhase = TwoPi * freq * t + modulator * 2.8f;
        return 0.42f * MathF.Sin(carrierPhase);
    }

    private static float BrassTone(float freq, float t)
    {
        var phase = TwoPi * freq * t;
        var sum = 0f;
        for (var n = 1; n <= 6; n++)
            sum += MathF.Sin(phase * n) / n;
        return 0.55f * sum;
    }

    private static float StringsSaw(float freq, float t)
    {
        var phase = TwoPi * freq * t;
        var sum = 0f;
        for (var n = 1; n <= 10; n++)
            sum += MathF.Sin(phase * n) / n;
        return 0.42f * sum;
    }

    private static float RawSaw(float freq, float t)
    {
        var phase = t * freq;
        phase -= MathF.Floor(phase);
        return 0.68f * (2f * phase - 1f);
    }

    private static float BassGuitarTone(float freq, float t)
    {
        var phase = TwoPi * freq * t;
        return 0.58f * (
              MathF.Sin(phase)
            + 0.55f * MathF.Sin(phase * 2)
            + 0.30f * MathF.Sin(phase * 3)
            + 0.15f * MathF.Sin(phase * 4));
    }

    private static float UprightBassTone(float freq, float t)
    {
        var phase = TwoPi * freq * t;
        return 0.55f * (
              MathF.Sin(phase)
            + 0.50f * MathF.Sin(phase * 2)
            + 0.28f * MathF.Sin(phase * 3)
            + 0.12f * MathF.Sin(phase * 4)
            + 0.05f * MathF.Sin(phase * 5));
    }

    private static float NoiseTonal(float freq, float t)
    {
        var phase = TwoPi * freq * t;
        return 0.30f * MathF.Sin(phase);
    }
}
