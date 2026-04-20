namespace Task5.Services.Audio;

public class MelodyComposer
{
    private const float StrongVelocity = 1.0f;

    private const float WeakVelocity = 0.72f;

    public NoteEvent[] Compose(MusicParams musicParams, Random random)
    {
        var plan = DensityPlanner.For(musicParams.Density);
        var song = BuildSongPlan(plan, random);
        var beatDuration = 60f / musicParams.Tempo;
        var subDuration = beatDuration * AudioConfig.BeatsPerBar / plan.SubsPerBar;

        var notes = new List<NoteEvent>();
        for (var bar = 0; bar < AudioConfig.Bars; bar++)
            EmitBar(notes, musicParams, bar, song, plan, subDuration);

        ApplyCadence(notes, musicParams);
        return [.. notes];
    }

    private static SongPlan BuildSongPlan(DensityPlan plan, Random random)
    {
        var motifA = MotifLibrary.Pick(random);
        var motifB = MotifLibrary.Pick(random);
        var rhythmA = RhythmLibrary.Pick(plan.SubsPerBar, random);
        var rhythmB = RhythmLibrary.Pick(plan.SubsPerBar, random);
        var phraseForm = PhraseFormLibrary.Pick(random);
        return new SongPlan(motifA, motifB, rhythmA, rhythmB, phraseForm);
    }

    private static void EmitBar(List<NoteEvent> notes, MusicParams musicParams, int bar, SongPlan song, DensityPlan plan, float subDuration)
    {
        var isBBar = bar == 2;
        var motif = isBBar ? song.MotifB : song.MotifA;
        var rhythm = isBBar ? song.RhythmB : song.RhythmA;
        var chordDeg = musicParams.ChordDegrees[bar];
        var phraseShift = song.PhraseForm[bar];

        for (var sub = 0; sub < plan.SubsPerBar; sub++)
        {
            if (!rhythm[sub])
                continue;

            var offset = MotifLibrary.SampleAt(motif, sub, plan.SubsPerBar);
            var degree = chordDeg + offset + phraseShift;
            notes.Add(BuildNote(degree, musicParams, bar, sub, plan, subDuration));
        }
    }

    private static NoteEvent BuildNote(int degree, MusicParams musicParams, int bar, int sub, DensityPlan plan, float subDuration)
    {
        var start = (bar * plan.SubsPerBar + sub) * subDuration;
        if (plan.Swing && sub % 2 == 1)
            start += subDuration * 0.33f;

        var duration = subDuration * 0.88f;
        var melodyOctave = 1 + musicParams.MelodyOctave;
        var midiNote = NoteHelper.DegreeToMidi(degree, musicParams.RootNote, musicParams.ScaleIntervals, melodyOctave);
        var velocity = IsStrongSubdivision(sub, plan) ? StrongVelocity : WeakVelocity;
        return new NoteEvent(midiNote, start, start + duration, velocity);
    }

    private static bool IsStrongSubdivision(int sub, DensityPlan plan)
    {
        var stride = plan.SubsPerBar / AudioConfig.BeatsPerBar;
        return stride > 0 && sub % stride == 0;
    }

    private static void ApplyCadence(List<NoteEvent> notes, MusicParams musicParams)
    {
        if (notes.Count == 0) return;
        var last = notes[^1];
        var tonic = NoteHelper.DegreeToMidi(0, musicParams.RootNote, musicParams.ScaleIntervals, 1 + musicParams.MelodyOctave);
        notes[^1] = last with { MidiNote = tonic };
    }

    private record SongPlan(int[] MotifA, int[] MotifB, bool[] RhythmA, bool[] RhythmB, int[] PhraseForm);
}
