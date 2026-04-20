namespace Task5.Services.Audio;

public static class GmToInstrumentMapper
{
    public static Instrument Map(int program) => program switch
    {
        >= 0 and <= 7 => Instrument.Piano,
        >= 8 and <= 15 => Instrument.Bell,
        >= 16 and <= 23 => Instrument.Pad,
        >= 24 and <= 31 => Instrument.Pluck,
        32 or 43 or 42 => Instrument.UprightBass,
        33 or 34 or 35 or 36 or 37 => Instrument.BassGuitar,
        38 or 39 => Instrument.SubBass,
        >= 40 and <= 47 => Instrument.Strings,
        >= 48 and <= 55 => Instrument.Strings,
        >= 56 and <= 63 => Instrument.Brass,
        >= 64 and <= 71 => Instrument.Brass,
        >= 72 and <= 79 => Instrument.Flute,
        >= 80 and <= 87 => Instrument.ElectricLead,
        >= 88 and <= 95 => Instrument.Pad,
        >= 96 and <= 103 => Instrument.NoiseSweep,
        _ => Instrument.Piano
    };
}
