namespace Task5.Services.Audio;

public class DrumComposer
{
    private const int Kick = 36;
    private const int Snare = 38;
    private const int ClosedHat = 42;
    private const int OpenHat = 46;
    private const int RideCymbal = 51;
    private const int Rimshot = 37;

    private const float HitDuration = 0.08f;

    public NoteEvent[] Compose(MusicParams musicParams, DrumStyle style)
    {
        if (style == DrumStyle.None)
            return [];

        var beatDuration = 60f / musicParams.Tempo;
        var barDuration = AudioConfig.BeatsPerBar * beatDuration;
        var notes = new List<NoteEvent>();

        for (var bar = 0; bar < AudioConfig.Bars; bar++)
            EmitBar(notes, style, bar * barDuration, beatDuration);

        return [.. notes];
    }

    private static void EmitBar(List<NoteEvent> notes, DrumStyle style, float barStart, float beatDuration)
    {
        switch (style)
        {
            case DrumStyle.RockBeat:
                EmitRockBeat(notes, barStart, beatDuration);
                break;
            case DrumStyle.PopBeat:
                EmitPopBeat(notes, barStart, beatDuration);
                break;
            case DrumStyle.HipHopBeat:
                EmitHipHopBeat(notes, barStart, beatDuration);
                break;
            case DrumStyle.FourOnFloor:
                EmitFourOnFloor(notes, barStart, beatDuration);
                break;
            case DrumStyle.Punk:
                EmitPunk(notes, barStart, beatDuration);
                break;
            case DrumStyle.Reggae:
                EmitReggae(notes, barStart, beatDuration);
                break;
            case DrumStyle.Jazz:
                EmitJazz(notes, barStart, beatDuration);
                break;
            case DrumStyle.Shuffle:
                EmitShuffle(notes, barStart, beatDuration);
                break;
            case DrumStyle.BlastBeat:
                EmitBlastBeat(notes, barStart, beatDuration);
                break;
        }
    }

    private static void EmitBlastBeat(List<NoteEvent> notes, float barStart, float beat)
    {
        for (var i = 0; i < 8; i++)
            AddHit(notes, Kick, barStart + beat * i * 0.5f, 1.0f);
        for (var i = 0; i < 4; i++)
            AddHit(notes, Snare, barStart + beat * (i + 0.5f), 0.95f);
        for (var i = 0; i < 16; i++)
            AddHit(notes, ClosedHat, barStart + beat * i * 0.25f, 0.5f);
    }

    private static void EmitRockBeat(List<NoteEvent> notes, float barStart, float beat)
    {
        AddHit(notes, Kick, barStart + beat * 0, 1.0f);
        AddHit(notes, Kick, barStart + beat * 2, 0.85f);
        AddHit(notes, Snare, barStart + beat * 1, 0.95f);
        AddHit(notes, Snare, barStart + beat * 3, 0.95f);
        for (var i = 0; i < 8; i++)
            AddHit(notes, ClosedHat, barStart + beat * i * 0.5f, 0.55f);
    }

    private static void EmitPopBeat(List<NoteEvent> notes, float barStart, float beat)
    {
        AddHit(notes, Kick, barStart + beat * 0, 0.95f);
        AddHit(notes, Kick, barStart + beat * 2.5f, 0.80f);
        AddHit(notes, Snare, barStart + beat * 1, 0.90f);
        AddHit(notes, Snare, barStart + beat * 3, 0.90f);
        for (var i = 0; i < 8; i++)
            AddHit(notes, ClosedHat, barStart + beat * i * 0.5f, i % 2 == 0 ? 0.55f : 0.40f);
    }

    private static void EmitHipHopBeat(List<NoteEvent> notes, float barStart, float beat)
    {
        AddHit(notes, Kick, barStart, 1.0f);
        AddHit(notes, Kick, barStart + beat * 2, 0.90f);
        AddHit(notes, Kick, barStart + beat * 2.75f, 0.70f);
        AddHit(notes, Snare, barStart + beat * 1, 0.85f);
        AddHit(notes, Snare, barStart + beat * 3, 0.85f);
        for (var i = 0; i < 16; i++)
            AddHit(notes, ClosedHat, barStart + beat * i * 0.25f, i % 2 == 0 ? 0.40f : 0.28f);
    }

    private static void EmitFourOnFloor(List<NoteEvent> notes, float barStart, float beat)
    {
        for (var i = 0; i < 4; i++)
            AddHit(notes, Kick, barStart + beat * i, 1.0f);
        AddHit(notes, ClosedHat, barStart + beat * 0.5f, 0.50f);
        AddHit(notes, ClosedHat, barStart + beat * 1.5f, 0.50f);
        AddHit(notes, OpenHat, barStart + beat * 2.5f, 0.55f);
        AddHit(notes, ClosedHat, barStart + beat * 3.5f, 0.50f);
        AddHit(notes, Snare, barStart + beat * 1, 0.70f);
        AddHit(notes, Snare, barStart + beat * 3, 0.70f);
    }

    private static void EmitPunk(List<NoteEvent> notes, float barStart, float beat)
    {
        for (var i = 0; i < 4; i++)
            AddHit(notes, Kick, barStart + beat * i, 1.0f);
        AddHit(notes, Snare, barStart + beat * 1, 1.0f);
        AddHit(notes, Snare, barStart + beat * 3, 1.0f);
        for (var i = 0; i < 8; i++)
            AddHit(notes, ClosedHat, barStart + beat * i * 0.5f, 0.65f);
    }

    private static void EmitReggae(List<NoteEvent> notes, float barStart, float beat)
    {
        AddHit(notes, Kick, barStart + beat * 2, 0.9f);
        AddHit(notes, Snare, barStart + beat * 2, 0.85f);
        AddHit(notes, ClosedHat, barStart + beat * 0.5f, 0.6f);
        AddHit(notes, ClosedHat, barStart + beat * 1.5f, 0.6f);
        AddHit(notes, ClosedHat, barStart + beat * 2.5f, 0.6f);
        AddHit(notes, ClosedHat, barStart + beat * 3.5f, 0.6f);
    }

    private static void EmitJazz(List<NoteEvent> notes, float barStart, float beat)
    {
        for (var i = 0; i < 4; i++)
        {
            AddHit(notes, RideCymbal, barStart + beat * i, 0.65f);
            if (i % 2 == 1)
                AddHit(notes, RideCymbal, barStart + beat * i + beat * 0.66f, 0.50f);
        }
        AddHit(notes, ClosedHat, barStart + beat * 1, 0.50f);
        AddHit(notes, ClosedHat, barStart + beat * 3, 0.50f);
    }

    private static void EmitShuffle(List<NoteEvent> notes, float barStart, float beat)
    {
        AddHit(notes, Kick, barStart, 0.95f);
        AddHit(notes, Kick, barStart + beat * 2, 0.85f);
        AddHit(notes, Snare, barStart + beat * 1, 0.90f);
        AddHit(notes, Snare, barStart + beat * 3, 0.90f);
        for (var i = 0; i < 4; i++)
        {
            AddHit(notes, ClosedHat, barStart + beat * i, 0.60f);
            AddHit(notes, ClosedHat, barStart + beat * i + beat * 0.66f, 0.45f);
        }
    }

    private static void AddHit(List<NoteEvent> notes, int midiNote, float time, float velocity)
    {
        notes.Add(new NoteEvent(midiNote, time, time + HitDuration, velocity));
    }
}
