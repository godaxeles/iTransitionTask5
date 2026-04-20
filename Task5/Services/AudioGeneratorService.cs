using Task5.Models;
using Task5.Services.Audio;

namespace Task5.Services;

public class AudioGeneratorService(
    MusicParamsFactory musicParamsFactory,
    MelodyComposer melodyComposer,
    BassComposer bassComposer,
    PadComposer padComposer,
    DrumComposer drumComposer,
    AudioSynthesizer audioSynthesizer,
    MidiEventBuilder midiEventBuilder,
    MidiAudioRenderer midiAudioRenderer,
    SoundFontProvider soundFontProvider,
    GenreAudioProcessor genreProcessor,
    WavEncoder wavEncoder)
{
    public byte[] Generate(long seed, int songIndex, GenreCategory category)
    {
        var random = CreateRandom(seed, songIndex);
        var musicParams = musicParamsFactory.Create(random, category);

        var melodyNotes = melodyComposer.Compose(musicParams, random);
        var bassNotes = bassComposer.Compose(musicParams);
        var padNotes = padComposer.Compose(musicParams);
        var drumNotes = drumComposer.Compose(musicParams, DrumStyleRegistry.For(category));

        var totalDuration = ComputeTotalDuration(musicParams.Tempo);
        var buffer = RenderInstruments(musicParams, category, melodyNotes, bassNotes, padNotes, totalDuration);

        GenreFilter.Apply(buffer, GenreFilterRegistry.For(category));
        genreProcessor.Process(buffer, category);
        audioSynthesizer.AddDrums(buffer, drumNotes);
        AudioNormalizer.NormalizeToTargetPeak(buffer);

        return wavEncoder.Encode(buffer.Left, buffer.Right);
    }

    private StereoBuffer RenderInstruments(
        MusicParams musicParams,
        GenreCategory category,
        NoteEvent[] melodyNotes,
        NoteEvent[] bassNotes,
        NoteEvent[] padNotes,
        float totalDuration)
    {
        var soundFont = soundFontProvider.Get();
        if (soundFont is null)
            return audioSynthesizer.SynthesizeInstruments(melodyNotes, bassNotes, padNotes, musicParams, category, totalDuration);

        var events = midiEventBuilder.Build(musicParams, melodyNotes, bassNotes, padNotes, []);
        return midiAudioRenderer.Render(soundFont, events, totalDuration);
    }

    private static Random CreateRandom(long seed, int songIndex)
    {
        return new Random(SeedHelper.ToInt32(SeedHelper.ComputeAudioSeed(seed, songIndex)));
    }

    private static float ComputeTotalDuration(int tempo)
        => AudioConfig.Bars * AudioConfig.BeatsPerBar * (60f / tempo);
}
