namespace Task5.Services.Audio;

internal class AudioSynthesizer
{
    public float[] Synthesize(NoteEvent[] melodyNotes, NoteEvent[] bassNotes, float totalDuration)
    {
        var totalSamples = (int)(AudioConfig.SampleRate * totalDuration) + 1;
        var buffer = new float[totalSamples];

        RenderNotes(buffer, melodyNotes, MelodyWave, 0.35f);
        RenderNotes(buffer, bassNotes, BassWave, 0.45f);

        return buffer;
    }

    private static void RenderNotes(float[] buffer, NoteEvent[] notes, Func<float, float, float> waveFunc, float amplitude)
    {
        foreach (var note in notes)
            RenderNote(buffer, note, waveFunc, amplitude);
    }

    private static void RenderNote(float[] buffer, NoteEvent note, Func<float, float, float> waveFunc, float amplitude)
    {
        var startSample = (int)(note.StartTime * AudioConfig.SampleRate);
        var endSample = Math.Min((int)(note.EndTime * AudioConfig.SampleRate), buffer.Length);

        if (startSample >= buffer.Length)
            return;

        var freq = NoteHelper.ToFrequency(note.MidiNote);
        var duration = endSample - startSample;
        var attackSamples = Math.Min((int)(0.02f * AudioConfig.SampleRate), duration / 4);
        var releaseSamples = Math.Min((int)(0.08f * AudioConfig.SampleRate), duration / 4);

        for (var i = startSample; i < endSample; i++)
        {
            var t = (float)(i - startSample) / AudioConfig.SampleRate;
            var offset = i - startSample;
            var envelope = ComputeEnvelope(offset, duration, attackSamples, releaseSamples);
            buffer[i] += waveFunc(freq, t) * amplitude * envelope;
        }
    }

    private static float ComputeEnvelope(int offset, int duration, int attackSamples, int releaseSamples)
    {
        if (offset < attackSamples)
            return (float)offset / attackSamples;

        if (offset > duration - releaseSamples)
            return (float)(duration - offset) / releaseSamples;

        return 1f;
    }

    private static float MelodyWave(float freq, float t)
        => MathF.Sin(2 * MathF.PI * freq * t);

    private static float BassWave(float freq, float t)
        => 0.6f * MathF.Sin(2 * MathF.PI * freq * t)
         + 0.3f * MathF.Sin(4 * MathF.PI * freq * t)
         + 0.1f * MathF.Sin(6 * MathF.PI * freq * t);
}
