namespace Task5.Services.Audio;

internal record MusicParams(
    int RootNote,
    int[] ScaleIntervals,
    int Tempo,
    int[] ChordDegrees
);
