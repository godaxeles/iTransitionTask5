namespace Task5.Services.Audio;

public record StereoBuffer(float[] Left, float[] Right)
{
    public int Length => Math.Min(Left.Length, Right.Length);

    public static StereoBuffer Allocate(int sampleCount)
        => new(new float[sampleCount], new float[sampleCount]);
}
