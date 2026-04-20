namespace Task5.Services.Audio;

public static class NoteHelper
{
    public static float ToFrequency(int midiNote)
        => 440f * MathF.Pow(2f, (midiNote - 69) / 12f);

    public static int DegreeToMidi(int degree, int rootNote, int[] scale, int octave)
    {
        var length = scale.Length;
        var normalizedDeg = ((degree % length) + length) % length;
        var octaveShift = (degree >= 0 ? degree : degree - (length - 1)) / length + octave;
        return rootNote + scale[normalizedDeg] + octaveShift * 12;
    }
}
