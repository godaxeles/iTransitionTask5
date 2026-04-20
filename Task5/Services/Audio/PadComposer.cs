namespace Task5.Services.Audio;

public class PadComposer
{
    public NoteEvent[] Compose(MusicParams musicParams)
    {
        var beatDuration = 60f / musicParams.Tempo;
        var barDuration = AudioConfig.BeatsPerBar * beatDuration;
        var notes = new List<NoteEvent>(AudioConfig.Bars * 2);

        for (var bar = 0; bar < AudioConfig.Bars; bar++)
        {
            var chordDeg = musicParams.ChordDegrees[bar];
            var start = bar * barDuration;
            var end = start + barDuration * 0.98f;

            notes.Add(BuildPadNote(chordDeg, musicParams, start, end));
            notes.Add(BuildPadNote(chordDeg + 2, musicParams, start, end));
            notes.Add(BuildPadNote(chordDeg + 4, musicParams, start, end));
        }

        return [.. notes];
    }

    private static NoteEvent BuildPadNote(int degree, MusicParams musicParams, float start, float end)
    {
        var midiNote = NoteHelper.DegreeToMidi(degree, musicParams.RootNote, musicParams.ScaleIntervals, 1);
        return new NoteEvent(midiNote, start, end, 0.55f);
    }
}
