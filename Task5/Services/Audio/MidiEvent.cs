namespace Task5.Services.Audio;

public record struct MidiEvent(float Time, int Channel, int Command, int Data1, int Data2);
