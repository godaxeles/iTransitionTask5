using Task5.Models;

namespace Task5.Services.Audio;

public class AudioSynthesizer
{
    private static readonly Random NoiseRandom = new(17);

    public StereoBuffer SynthesizeInstruments(
        NoteEvent[] melodyNotes,
        NoteEvent[] bassNotes,
        NoteEvent[] padNotes,
        MusicParams musicParams,
        GenreCategory category,
        float totalDuration)
    {
        var totalSamples = (int)(AudioConfig.SampleRate * totalDuration) + 1;
        var mono = new float[totalSamples];

        var bassInst = GmToInstrumentMapper.Map(musicParams.BassProgram);
        var padInst = GmToInstrumentMapper.Map(musicParams.PadProgram);
        var melodyProfile = GetMelodyProfile(category);

        RenderMelody(mono, melodyNotes, category, melodyProfile, musicParams.MelodyGain);
        RenderVoice(mono, bassNotes, bassInst, musicParams.BassGain);
        RenderVoice(mono, padNotes, padInst, musicParams.PadGain);

        return DuplicateToStereo(mono);
    }

    public void AddDrums(StereoBuffer buffer, NoteEvent[] drumNotes)
    {
        var mono = new float[buffer.Length];
        ProceduralDrumSynth.Render(mono, drumNotes, amplitude: 1.8f);
        for (var i = 0; i < buffer.Length; i++)
        {
            buffer.Left[i] += mono[i];
            buffer.Right[i] += mono[i];
        }
    }

    private static StereoBuffer DuplicateToStereo(float[] mono)
    {
        var stereo = StereoBuffer.Allocate(mono.Length);
        Array.Copy(mono, stereo.Left, mono.Length);
        Array.Copy(mono, stereo.Right, mono.Length);
        return stereo;
    }

    private static InstrumentProfile GetMelodyProfile(GenreCategory category) => category switch
    {
        GenreCategory.Classical => InstrumentRegistry.Get(Instrument.Strings),
        GenreCategory.Ambient => InstrumentRegistry.Get(Instrument.Bell),
        GenreCategory.Pop or GenreCategory.Jazz or GenreCategory.HipHop or GenreCategory.Indie
            => InstrumentRegistry.Get(Instrument.Piano),
        GenreCategory.Country or GenreCategory.Folk => InstrumentRegistry.Get(Instrument.Pluck),
        _ => InstrumentRegistry.Get(Instrument.ElectricLead)
    };

    private static void RenderMelody(float[] buffer, NoteEvent[] notes, GenreCategory category, InstrumentProfile profile, float amplitude)
    {
        foreach (var note in notes)
            RenderMelodyNote(buffer, note, category, profile, amplitude);
    }

    private static void RenderMelodyNote(float[] buffer, NoteEvent note, GenreCategory category, InstrumentProfile profile, float amplitude)
    {
        var startSample = (int)(note.StartTime * AudioConfig.SampleRate);
        var endSample = Math.Min((int)(note.EndTime * AudioConfig.SampleRate), buffer.Length);
        if (startSample >= buffer.Length) return;

        var baseFreq = NoteHelper.ToFrequency(note.MidiNote);
        var duration = endSample - startSample;
        var attackSamples = Math.Min((int)(profile.AttackSec * AudioConfig.SampleRate), duration / 2);
        var releaseSamples = Math.Min((int)(profile.ReleaseSec * AudioConfig.SampleRate), duration);
        var noteAmplitude = amplitude * note.Velocity;
        var hasVibrato = profile.VibratoDepth > 0f;

        for (var i = startSample; i < endSample; i++)
        {
            var offset = i - startSample;
            var t = (float)offset / AudioConfig.SampleRate;
            var freq = hasVibrato ? ApplyVibrato(baseFreq, t, profile) : baseFreq;
            var tone = GenreOscillator.Evaluate(category, freq, t);
            var envelope = ComputeEnvelope(offset, duration, attackSamples, releaseSamples, profile);
            buffer[i] += tone * noteAmplitude * envelope;
        }
    }

    private static void RenderVoice(float[] buffer, NoteEvent[] notes, Instrument instrument, float amplitude)
    {
        var profile = InstrumentRegistry.Get(instrument);
        foreach (var note in notes)
            RenderNote(buffer, note, instrument, profile, amplitude);
    }

    private static void RenderNote(float[] buffer, NoteEvent note, Instrument instrument, InstrumentProfile profile, float amplitude)
    {
        var startSample = (int)(note.StartTime * AudioConfig.SampleRate);
        var endSample = Math.Min((int)(note.EndTime * AudioConfig.SampleRate), buffer.Length);
        if (startSample >= buffer.Length) return;

        var baseFreq = NoteHelper.ToFrequency(note.MidiNote);
        var duration = endSample - startSample;
        var attackSamples = Math.Min((int)(profile.AttackSec * AudioConfig.SampleRate), duration / 2);
        var releaseSamples = Math.Min((int)(profile.ReleaseSec * AudioConfig.SampleRate), duration);
        var noteAmplitude = amplitude * note.Velocity;
        var hasVibrato = profile.VibratoDepth > 0f;
        var hasNoise = profile.NoiseAmount > 0f;

        for (var i = startSample; i < endSample; i++)
        {
            var offset = i - startSample;
            var t = (float)offset / AudioConfig.SampleRate;
            var freq = hasVibrato ? ApplyVibrato(baseFreq, t, profile) : baseFreq;

            var tone = InstrumentSynthesizer.Evaluate(instrument, freq, t);
            if (hasNoise)
                tone = BlendNoise(tone, profile.NoiseAmount);

            var envelope = ComputeEnvelope(offset, duration, attackSamples, releaseSamples, profile);
            buffer[i] += tone * noteAmplitude * envelope;
        }
    }

    private static float ApplyVibrato(float freq, float t, InstrumentProfile profile)
    {
        var mod = MathF.Sin(2f * MathF.PI * profile.VibratoRate * t);
        return freq * (1f + profile.VibratoDepth * mod);
    }

    private static float BlendNoise(float tone, float amount)
    {
        var noise = (float)(NoiseRandom.NextDouble() * 2 - 1);
        return tone * (1f - amount) + noise * amount;
    }

    private static float ComputeEnvelope(int offset, int duration, int attackSamples, int releaseSamples, InstrumentProfile profile)
    {
        float envelope;
        if (attackSamples > 0 && offset < attackSamples)
        {
            var progress = (float)offset / attackSamples;
            envelope = progress * progress;
        }
        else if (releaseSamples > 0 && offset > duration - releaseSamples)
        {
            var progress = (float)(duration - offset) / releaseSamples;
            envelope = progress * progress;
        }
        else
        {
            envelope = 1f;
        }

        if (profile.UseExponentialDecay && profile.DecayRate > 0f)
        {
            var elapsed = (float)offset / AudioConfig.SampleRate;
            envelope *= MathF.Exp(-profile.DecayRate * elapsed);
        }

        return envelope;
    }
}
