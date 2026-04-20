namespace Task5.Services.Audio;

public record MusicParams(
    int RootNote,
    int[] ScaleIntervals,
    int Tempo,
    int[] ChordDegrees,
    int MelodyProgram,
    int BassProgram,
    int PadProgram,
    MelodyDensity Density,
    BassPattern BassPattern,
    float MelodyGain,
    float BassGain,
    float PadGain,
    int MelodyOctave,
    int BassOctave
);
