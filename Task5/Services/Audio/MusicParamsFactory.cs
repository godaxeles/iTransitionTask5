using Task5.Models;

namespace Task5.Services.Audio;

public class MusicParamsFactory
{
    private static readonly int[] RootNotes = [40, 43, 45, 48, 50, 52, 53, 55, 57, 60, 62, 64, 67, 69];

    public MusicParams Create(Random random, GenreCategory category)
    {
        var profile = GenreMusicRegistry.Get(category);
        var rootNote = RootNotes[random.Next(RootNotes.Length)];
        var tempo = random.Next(profile.TempoMin, profile.TempoMax + 1);
        var scaleIntervals = ScaleProvider.GetScale(profile.Scale);
        var isMajorLike = ScaleProvider.IsMajorLike(profile.Scale);
        var chordDegrees = ChordProgressionProvider.GetProgression(isMajorLike, random);

        var melodyProgram = Pick(profile.MelodyPrograms, random);
        var bassProgram = Pick(profile.BassPrograms, random);
        var padProgram = Pick(profile.PadPrograms, random);

        return new MusicParams(
            rootNote,
            scaleIntervals,
            tempo,
            chordDegrees,
            melodyProgram,
            bassProgram,
            padProgram,
            profile.Density,
            profile.BassPattern,
            profile.MelodyGain,
            profile.BassGain,
            profile.PadGain,
            profile.MelodyOctave,
            profile.BassOctave);
    }

    private static int Pick(int[] pool, Random random)
        => pool.Length == 0 ? 0 : pool[random.Next(pool.Length)];
}
