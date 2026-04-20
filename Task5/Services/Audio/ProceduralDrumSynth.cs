namespace Task5.Services.Audio;

public static class ProceduralDrumSynth
{
    private const float TwoPi = 2f * MathF.PI;

    private const int Kick = 36;
    private const int Rimshot = 37;
    private const int Snare = 38;
    private const int ClosedHat = 42;
    private const int OpenHat = 46;
    private const int Ride = 51;

    public static void Render(float[] buffer, NoteEvent[] drumNotes, float amplitude)
    {
        foreach (var note in drumNotes)
            RenderHit(buffer, note, amplitude);
    }

    private static void RenderHit(float[] buffer, NoteEvent note, float amplitude)
    {
        var startSample = (int)(note.StartTime * AudioConfig.SampleRate);
        if (startSample >= buffer.Length) return;

        var velocity = note.Velocity * amplitude;
        switch (note.MidiNote)
        {
            case Kick or 35:
                RenderKick(buffer, startSample, velocity);
                break;
            case Snare or Rimshot:
                RenderSnare(buffer, startSample, velocity);
                break;
            case ClosedHat:
                RenderHiHat(buffer, startSample, velocity, open: false);
                break;
            case OpenHat:
                RenderHiHat(buffer, startSample, velocity, open: true);
                break;
            case Ride:
                RenderRide(buffer, startSample, velocity);
                break;
            default:
                RenderKick(buffer, startSample, velocity);
                break;
        }
    }

    private static void RenderKick(float[] buffer, int startSample, float velocity)
    {
        var sr = AudioConfig.SampleRate;
        var duration = (int)(0.28f * sr);
        for (var i = 0; i < duration && startSample + i < buffer.Length; i++)
        {
            var t = (float)i / sr;
            var pitchEnv = MathF.Exp(-t * 30f);
            var freq = 50f + 110f * pitchEnv;
            var amp = MathF.Exp(-t * 9f) * velocity * 0.95f;
            buffer[startSample + i] += MathF.Sin(TwoPi * freq * t) * amp;
        }
    }

    private static void RenderSnare(float[] buffer, int startSample, float velocity)
    {
        var sr = AudioConfig.SampleRate;
        var duration = (int)(0.16f * sr);
        var rand = new Random(startSample & 0x7fffffff);
        var lowState = 0f;
        for (var i = 0; i < duration && startSample + i < buffer.Length; i++)
        {
            var t = (float)i / sr;
            var noise = (float)(rand.NextDouble() * 2 - 1);
            lowState += 0.25f * (noise - lowState);
            var bandpassed = noise - lowState;
            var tonal = MathF.Sin(TwoPi * 220f * t) * MathF.Exp(-t * 28f);
            var amp = MathF.Exp(-t * 14f) * velocity * 0.75f;
            buffer[startSample + i] += (bandpassed * 0.65f + tonal * 0.35f) * amp;
        }
    }

    private static void RenderHiHat(float[] buffer, int startSample, float velocity, bool open)
    {
        var sr = AudioConfig.SampleRate;
        var decay = open ? 0.22f : 0.05f;
        var duration = (int)(decay * sr);
        var rand = new Random((startSample ^ 0x5A) & 0x7fffffff);
        var lowState = 0f;
        var decayRate = open ? 8f : 45f;
        for (var i = 0; i < duration && startSample + i < buffer.Length; i++)
        {
            var t = (float)i / sr;
            var noise = (float)(rand.NextDouble() * 2 - 1);
            lowState += 0.45f * (noise - lowState);
            var highpassed = noise - lowState;
            var amp = MathF.Exp(-t * decayRate) * velocity * 0.45f;
            buffer[startSample + i] += highpassed * amp;
        }
    }

    private static void RenderRide(float[] buffer, int startSample, float velocity)
    {
        var sr = AudioConfig.SampleRate;
        var duration = (int)(0.35f * sr);
        var rand = new Random((startSample ^ 0x73) & 0x7fffffff);
        var lowState = 0f;
        for (var i = 0; i < duration && startSample + i < buffer.Length; i++)
        {
            var t = (float)i / sr;
            var noise = (float)(rand.NextDouble() * 2 - 1);
            lowState += 0.25f * (noise - lowState);
            var highpassed = noise - lowState;
            var tonal = MathF.Sin(TwoPi * 4400f * t) * 0.3f + MathF.Sin(TwoPi * 3100f * t) * 0.2f;
            var amp = MathF.Exp(-t * 7f) * velocity * 0.40f;
            buffer[startSample + i] += (highpassed * 0.55f + tonal * 0.45f) * amp;
        }
    }
}
