namespace Task5.Services.Audio;

public class BassComposer
{
    public NoteEvent[] Compose(MusicParams musicParams)
    {
        var beatDuration = 60f / musicParams.Tempo;
        var barDuration = AudioConfig.BeatsPerBar * beatDuration;
        var notes = new NoteEvent[AudioConfig.Bars];

        for (var bar = 0; bar < AudioConfig.Bars; bar++)
            notes[bar] = CreateBassNote(musicParams, bar, barDuration);

        return notes;
    }

    private static NoteEvent CreateBassNote(MusicParams musicParams, int bar, float barDuration)
    {
        var chordDeg = musicParams.ChordDegrees[bar];
        var noteStart = bar * barDuration;
        var noteEnd = noteStart + barDuration * 0.92f;
        var midiNote = NoteHelper.DegreeToMidi(chordDeg, musicParams.RootNote, musicParams.ScaleIntervals, 0);
        return new NoteEvent(midiNote, noteStart, noteEnd);
    }
}
