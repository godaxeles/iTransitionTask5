namespace Task5.Services.Audio;

public static class InstrumentRegistry
{
    private static readonly Dictionary<Instrument, InstrumentProfile> Profiles = new()
    {
        [Instrument.Piano] = new(0.005f, 1.8f, 0.25f, 0f, 0f, 0f, true),
        [Instrument.Strings] = new(0.18f, 0f, 0.45f, 0.004f, 5.5f, 0f, false),
        [Instrument.Pad] = new(0.35f, 0f, 0.80f, 0.002f, 3.0f, 0f, false),
        [Instrument.Pluck] = new(0.003f, 2.5f, 0.18f, 0f, 0f, 0.04f, true),
        [Instrument.Bell] = new(0.003f, 3.5f, 0.60f, 0f, 0f, 0f, true),
        [Instrument.Flute] = new(0.08f, 0f, 0.20f, 0.005f, 4.5f, 0.08f, false),
        [Instrument.Brass] = new(0.06f, 0f, 0.22f, 0.003f, 5.5f, 0f, false),
        [Instrument.ElectricLead] = new(0.015f, 0f, 0.14f, 0.002f, 5.0f, 0f, false),
        [Instrument.BassGuitar] = new(0.01f, 0f, 0.10f, 0f, 0f, 0f, false),
        [Instrument.SubBass] = new(0.03f, 0f, 0.15f, 0f, 0f, 0f, false),
        [Instrument.UprightBass] = new(0.008f, 1.2f, 0.20f, 0f, 0f, 0.02f, true),
        [Instrument.Drone] = new(0.60f, 0f, 1.20f, 0.001f, 2.5f, 0f, false),
        [Instrument.NoiseSweep] = new(0.40f, 0f, 0.60f, 0f, 0f, 0.85f, false)
    };

    public static InstrumentProfile Get(Instrument instrument)
        => Profiles.TryGetValue(instrument, out var p) ? p : Profiles[Instrument.Piano];
}
