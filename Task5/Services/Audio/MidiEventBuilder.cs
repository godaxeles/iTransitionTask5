namespace Task5.Services.Audio;

public class MidiEventBuilder
{
    private const int MelodyChannel = 0;
    private const int BassChannel = 1;
    private const int PadChannel = 2;
    private const int DrumChannel = 9;

    public List<MidiEvent> Build(
        MusicParams musicParams,
        NoteEvent[] melodyNotes,
        NoteEvent[] bassNotes,
        NoteEvent[] padNotes,
        NoteEvent[] drumNotes)
    {
        var events = new List<MidiEvent>();
        AddProgramChange(events, MelodyChannel, musicParams.MelodyProgram);
        AddProgramChange(events, BassChannel, musicParams.BassProgram);
        AddProgramChange(events, PadChannel, musicParams.PadProgram);

        AddMelodicNotes(events, melodyNotes, MelodyChannel, strongVel: 115, weakVel: 85);
        AddMelodicNotes(events, bassNotes, BassChannel, strongVel: 100, weakVel: 78);
        AddMelodicNotes(events, padNotes, PadChannel, strongVel: 70, weakVel: 55);
        AddDrumNotes(events, drumNotes, DrumChannel);

        SortStable(events);
        return events;
    }

    private static void AddProgramChange(List<MidiEvent> events, int channel, int program)
    {
        events.Add(new MidiEvent(0f, channel, 0xC0, program, 0));
    }

    private static void AddMelodicNotes(List<MidiEvent> events, NoteEvent[] notes, int channel, int strongVel, int weakVel)
    {
        foreach (var note in notes)
        {
            var velocity = note.Velocity >= 0.95f ? strongVel : weakVel;
            events.Add(new MidiEvent(note.StartTime, channel, 0x90, note.MidiNote, velocity));
            events.Add(new MidiEvent(note.EndTime, channel, 0x80, note.MidiNote, 0));
        }
    }

    private static void AddDrumNotes(List<MidiEvent> events, NoteEvent[] notes, int channel)
    {
        foreach (var note in notes)
        {
            var velocity = (int)Math.Clamp(note.Velocity * 127f, 80f, 127f);
            events.Add(new MidiEvent(note.StartTime, channel, 0x90, note.MidiNote, velocity));
            events.Add(new MidiEvent(note.EndTime, channel, 0x80, note.MidiNote, 0));
        }
    }

    private static void SortStable(List<MidiEvent> events)
    {
        events.Sort((a, b) =>
        {
            var timeCompare = a.Time.CompareTo(b.Time);
            if (timeCompare != 0) return timeCompare;

            var aPriority = Priority(a.Command);
            var bPriority = Priority(b.Command);
            return aPriority.CompareTo(bPriority);
        });
    }

    private static int Priority(int command) => (command & 0xF0) switch
    {
        0xC0 => 0,
        0xB0 => 1,
        0x80 => 2,
        0x90 => 3,
        _ => 4
    };
}
