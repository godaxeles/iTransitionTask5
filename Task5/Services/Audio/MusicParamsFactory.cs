namespace Task5.Services.Audio;

internal class MusicParamsFactory
{
    private static readonly int[] RootNotes = [48, 50, 52, 53, 55, 57, 59, 60];

    public MusicParams Create(Random random)
    {
        var rootNote = RootNotes[random.Next(RootNotes.Length)];
        var isMajor = random.Next(2) == 0;
        var tempo = random.Next(75, 131);
        var scaleIntervals = ScaleProvider.GetScale(isMajor);
        var chordDegrees = ChordProgressionProvider.GetProgression(isMajor, random);
        return new MusicParams(rootNote, scaleIntervals, tempo, chordDegrees);
    }
}
