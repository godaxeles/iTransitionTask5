using MeltySynth;

namespace Task5.Services.Audio;

public class MidiAudioRenderer
{
    private const int BlockSize = 128;

    public StereoBuffer Render(SoundFont soundFont, List<MidiEvent> events, float totalDuration)
    {
        var settings = new SynthesizerSettings(AudioConfig.SampleRate);
        var synth = new Synthesizer(soundFont, settings);

        var totalSamples = (int)(AudioConfig.SampleRate * totalDuration) + 1;
        var buffer = StereoBuffer.Allocate(totalSamples);
        var blockLeft = new float[BlockSize];
        var blockRight = new float[BlockSize];

        var eventIndex = 0;
        var samplesRendered = 0;

        while (samplesRendered < totalSamples)
        {
            var currentTime = (float)samplesRendered / AudioConfig.SampleRate;
            eventIndex = DispatchEventsUpTo(synth, events, eventIndex, currentTime);

            var remaining = Math.Min(BlockSize, totalSamples - samplesRendered);
            synth.Render(blockLeft.AsSpan(0, remaining), blockRight.AsSpan(0, remaining));

            Array.Copy(blockLeft, 0, buffer.Left, samplesRendered, remaining);
            Array.Copy(blockRight, 0, buffer.Right, samplesRendered, remaining);

            samplesRendered += remaining;
        }

        return buffer;
    }

    private static int DispatchEventsUpTo(Synthesizer synth, List<MidiEvent> events, int startIndex, float currentTime)
    {
        var index = startIndex;
        while (index < events.Count && events[index].Time <= currentTime)
        {
            var e = events[index];
            synth.ProcessMidiMessage(e.Channel, e.Command, e.Data1, e.Data2);
            index++;
        }
        return index;
    }
}
