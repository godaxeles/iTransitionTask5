namespace Task5.Services.Audio;

public record DensityPlan(int SubsPerBar, bool Swing, float SkipProbability);

public static class DensityPlanner
{
    public static DensityPlan For(MelodyDensity density) => density switch
    {
        MelodyDensity.Half => new DensityPlan(2, false, 0f),
        MelodyDensity.Quarter => new DensityPlan(4, false, 0f),
        MelodyDensity.Eighth => new DensityPlan(8, false, 0f),
        MelodyDensity.Sixteenth => new DensityPlan(16, false, 0.10f),
        MelodyDensity.SwingEighth => new DensityPlan(8, true, 0f),
        MelodyDensity.Syncopated => new DensityPlan(8, false, 0.25f),
        _ => new DensityPlan(4, false, 0f)
    };
}
