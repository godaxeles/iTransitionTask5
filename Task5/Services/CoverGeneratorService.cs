using SkiaSharp;

namespace Task5.Services;

public class CoverGeneratorService
{
    private const int Width = 400;

    private const int Height = 400;

    public byte[] Generate(long seed, int songIndex, string title, string artist)
    {
        using var bitmap = new SKBitmap(Width, Height);
        using var canvas = new SKCanvas(bitmap);

        var random = CreateRandom(seed, songIndex);
        var palette = CreatePalette(random);
        var painter = new CoverPainter(canvas, palette, random, Width, Height);

        painter.Paint(title, artist);

        return EncodeToBytes(bitmap);
    }

    private static Random CreateRandom(long seed, int songIndex)
    {
        return new Random(SeedHelper.ToInt32(SeedHelper.ComputeCoverSeed(seed, songIndex)));
    }

    private static CoverPalette CreatePalette(Random random)
    {
        var hue = (float)(random.NextDouble() * 360);
        var primary = ColorFromHsl(hue, 0.65f, 0.28f);
        var secondary = ColorFromHsl((hue + 50) % 360, 0.70f, 0.52f);
        var accent = ColorFromHsl((hue + 200) % 360, 0.60f, 0.75f);
        return new CoverPalette(primary, secondary, accent);
    }

    private static SKColor ColorFromHsl(float h, float s, float l)
    {
        var c = (1 - MathF.Abs(2 * l - 1)) * s;
        var x = c * (1 - MathF.Abs(h / 60 % 2 - 1));
        var m = l - c / 2;

        var (r, g, b) = (int)(h / 60) switch
        {
            0 => (c, x, 0f),
            1 => (x, c, 0f),
            2 => (0f, c, x),
            3 => (0f, x, c),
            4 => (x, 0f, c),
            _ => (c, 0f, x)
        };

        return new SKColor(
            (byte)((r + m) * 255),
            (byte)((g + m) * 255),
            (byte)((b + m) * 255));
    }

    private static byte[] EncodeToBytes(SKBitmap bitmap)
    {
        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        return data.ToArray();
    }
}
