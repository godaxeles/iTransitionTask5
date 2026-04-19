using Task5.Services.Audio;

namespace Task5.Services;

public class AudioGeneratorService
{
    private readonly MusicParamsFactory _musicParamsFactory = new();
    
    private readonly MelodyComposer _melodyComposer = new();
    
    private readonly BassComposer _bassComposer = new();
    
    private readonly AudioSynthesizer _audioSynthesizer = new();
    
    private readonly WavEncoder _wavEncoder = new();

    public byte[] Generate(long seed, int songIndex)
    {
        var random = CreateRandom(seed, songIndex);
        var musicParams = _musicParamsFactory.Create(random);

        var melodyNotes = _melodyComposer.Compose(musicParams, random);
        var bassNotes = _bassComposer.Compose(musicParams);

        var totalDuration = ComputeTotalDuration(musicParams.Tempo);
        var samples = _audioSynthesizer.Synthesize(melodyNotes, bassNotes, totalDuration);

        return _wavEncoder.Encode(samples);
    }

    private static Random CreateRandom(long seed, int songIndex)
    {
        var combined = seed * 777_991L + songIndex * 131_071L;
        return new Random(SeedHelper.ToInt32(combined));
    }

    private static float ComputeTotalDuration(int tempo)
        => AudioConfig.Bars * AudioConfig.BeatsPerBar * (60f / tempo);
}
