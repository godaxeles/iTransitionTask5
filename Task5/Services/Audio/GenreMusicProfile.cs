namespace Task5.Services.Audio;

public record GenreMusicProfile(
    int TempoMin,
    int TempoMax,
    ScaleType Scale,
    int[] MelodyPrograms,
    int[] BassPrograms,
    int[] PadPrograms,
    MelodyDensity Density,
    BassPattern BassPattern,
    float MelodyGain,
    float BassGain,
    float PadGain,
    int MelodyOctave,
    int BassOctave
);
