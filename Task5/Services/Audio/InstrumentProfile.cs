namespace Task5.Services.Audio;

public record InstrumentProfile(
    float AttackSec,
    float DecayRate,
    float ReleaseSec,
    float VibratoDepth,
    float VibratoRate,
    float NoiseAmount,
    bool UseExponentialDecay
);
