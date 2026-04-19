using Task5.Services.Audio;

namespace Task5.Services;

public class AudioGeneratorService(
    MusicParamsFactory musicParamsFactory,
    MelodyComposer melodyComposer,
    BassComposer bassComposer,
    AudioSynthesizer audioSynthesizer,
    WavEncoder wavEncoder)
{
    public byte[] Generate(long seed, int songIndex)
    {
        var random = CreateRandom(seed, songIndex);
        var musicParams = musicParamsFactory.Create(random);

        var melodyNotes = melodyComposer.Compose(musicParams, random);
        var bassNotes = bassComposer.Compose(musicParams);

        var totalDuration = ComputeTotalDuration(musicParams.Tempo);
        var samples = audioSynthesizer.Synthesize(melodyNotes, bassNotes, totalDuration);

        return wavEncoder.Encode(samples);
    }

    private static Random CreateRandom(long seed, int songIndex)
    {
        return new Random(SeedHelper.ToInt32(SeedHelper.ComputeAudioSeed(seed, songIndex)));
    }

    private static float ComputeTotalDuration(int tempo)
        => AudioConfig.Bars * AudioConfig.BeatsPerBar * (60f / tempo);
}
