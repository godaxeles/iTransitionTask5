namespace Task5.Services.Audio;

public record MusicParams(
    int RootNote,
    int[] ScaleIntervals,
    int Tempo,
    int[] ChordDegrees
);
