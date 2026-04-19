namespace Task5.Services.Audio;

internal class MelodyComposer
{
    public NoteEvent[] Compose(MusicParams musicParams, Random random)
    {
        var beatDuration = 60f / musicParams.Tempo;
        var notes = new List<NoteEvent>();
        var currentDegree = 4;

        for (var bar = 0; bar < AudioConfig.Bars; bar++)
        {
            var chordDeg = musicParams.ChordDegrees[bar];

            for (var beat = 0; beat < AudioConfig.BeatsPerBar; beat++)
            {
                currentDegree = ChooseDegree(currentDegree, chordDeg, random, beat);
                notes.Add(CreateNote(currentDegree, musicParams, bar, beat, beatDuration));
            }
        }

        return [.. notes];
    }

    private static NoteEvent CreateNote(int degree, MusicParams musicParams, int bar, int beat, float beatDuration)
    {
        var noteStart = (bar * AudioConfig.BeatsPerBar + beat) * beatDuration;
        var noteEnd = noteStart + beatDuration * 0.88f;
        var midiNote = NoteHelper.DegreeToMidi(degree, musicParams.RootNote, musicParams.ScaleIntervals, 1);
        return new NoteEvent(midiNote, noteStart, noteEnd);
    }

    private static int ChooseDegree(int current, int chordDeg, Random random, int beat)
    {
        return IsStrongBeat(beat)
            ? ChooseChordTone(chordDeg, random)
            : ChooseStepwise(current, random);
    }

    private static bool IsStrongBeat(int beat) => beat is 0 or 2;

    private static int ChooseChordTone(int chordDeg, Random random)
    {
        var options = new[] { chordDeg, chordDeg + 2, chordDeg + 4 };
        return options[random.Next(options.Length)];
    }

    private static int ChooseStepwise(int current, Random random)
    {
        var step = random.Next(4) switch
        {
            0 => -1,
            1 => 0,
            2 => 0,
            _ => 1
        };
        return Math.Clamp(current + step, 2, 9);
    }
}
