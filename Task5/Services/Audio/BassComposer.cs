namespace Task5.Services.Audio;

public class BassComposer
{
    public NoteEvent[] Compose(MusicParams musicParams)
    {
        var beatDuration = 60f / musicParams.Tempo;
        var barDuration = AudioConfig.BeatsPerBar * beatDuration;
        var notes = new List<NoteEvent>();

        for (var bar = 0; bar < AudioConfig.Bars; bar++)
            EmitBarBass(notes, musicParams, bar, beatDuration, barDuration);

        return [.. notes];
    }

    private static void EmitBarBass(List<NoteEvent> notes, MusicParams musicParams, int bar, float beatDuration, float barDuration)
    {
        var chordDeg = musicParams.ChordDegrees[bar];
        var barStart = bar * barDuration;
        var octave = musicParams.BassOctave;

        switch (musicParams.BassPattern)
        {
            case BassPattern.WholeBar:
                AddBassNote(notes, chordDeg, octave, musicParams, barStart, barDuration * 0.92f);
                break;
            case BassPattern.Drone:
                AddBassNote(notes, chordDeg, octave - 1, musicParams, barStart, barDuration * 0.98f);
                break;
            case BassPattern.RootFifth:
                AddBassNote(notes, chordDeg, octave, musicParams, barStart, beatDuration * 1.9f);
                AddBassNote(notes, chordDeg + 4, octave, musicParams, barStart + beatDuration * 2, beatDuration * 1.9f);
                break;
            case BassPattern.Octave:
                AddBassNote(notes, chordDeg, octave, musicParams, barStart, beatDuration * 0.9f);
                AddBassNote(notes, chordDeg + 7, octave, musicParams, barStart + beatDuration, beatDuration * 0.9f);
                AddBassNote(notes, chordDeg, octave, musicParams, barStart + beatDuration * 2, beatDuration * 0.9f);
                AddBassNote(notes, chordDeg + 7, octave, musicParams, barStart + beatDuration * 3, beatDuration * 0.9f);
                break;
            case BassPattern.Walking:
                AddBassNote(notes, chordDeg, octave, musicParams, barStart, beatDuration * 0.88f);
                AddBassNote(notes, chordDeg + 1, octave, musicParams, barStart + beatDuration, beatDuration * 0.88f);
                AddBassNote(notes, chordDeg + 2, octave, musicParams, barStart + beatDuration * 2, beatDuration * 0.88f);
                AddBassNote(notes, chordDeg + 4, octave, musicParams, barStart + beatDuration * 3, beatDuration * 0.88f);
                break;
            case BassPattern.Offbeat:
                for (var i = 0; i < AudioConfig.BeatsPerBar; i++)
                    AddBassNote(notes, chordDeg, octave, musicParams, barStart + beatDuration * (i + 0.5f), beatDuration * 0.4f);
                break;
            case BassPattern.FourOnFloor:
                for (var i = 0; i < AudioConfig.BeatsPerBar; i++)
                    AddBassNote(notes, chordDeg, octave, musicParams, barStart + beatDuration * i, beatDuration * 0.88f);
                break;
            case BassPattern.Pedal:
                for (var i = 0; i < AudioConfig.BeatsPerBar * 2; i++)
                    AddBassNote(notes, chordDeg, octave, musicParams, barStart + (beatDuration / 2f) * i, (beatDuration / 2f) * 0.85f);
                break;
            case BassPattern.Arpeggio:
                AddBassNote(notes, chordDeg, octave, musicParams, barStart, beatDuration * 0.9f);
                AddBassNote(notes, chordDeg + 2, octave, musicParams, barStart + beatDuration, beatDuration * 0.9f);
                AddBassNote(notes, chordDeg + 4, octave, musicParams, barStart + beatDuration * 2, beatDuration * 0.9f);
                AddBassNote(notes, chordDeg + 2, octave, musicParams, barStart + beatDuration * 3, beatDuration * 0.9f);
                break;
        }
    }

    private static void AddBassNote(List<NoteEvent> notes, int degree, int octave, MusicParams musicParams, float startTime, float duration)
    {
        var midiNote = NoteHelper.DegreeToMidi(degree, musicParams.RootNote, musicParams.ScaleIntervals, octave);
        notes.Add(new NoteEvent(midiNote, startTime, startTime + duration));
    }
}
