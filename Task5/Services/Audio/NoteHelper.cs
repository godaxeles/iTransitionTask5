namespace Task5.Services.Audio;

public static class NoteHelper
{
    public static float ToFrequency(int midiNote)
        => 440f * MathF.Pow(2f, (midiNote - 69) / 12f);

    public static int DegreeToMidi(int degree, int rootNote, int[] scale, int octave)
    {
        var normalizedDeg = ((degree % 7) + 7) % 7;
        var octaveShift = degree / 7 + octave;
        return rootNote + scale[normalizedDeg] + octaveShift * 12;
    }
}
