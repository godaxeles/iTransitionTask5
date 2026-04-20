using SkiaSharp;
using Task5.Models;
using Task5.Services.Cover;

namespace Task5.Services;

public class CoverGeneratorService(ProceduralPainterRegistry painterRegistry)
{
    private const int Width = 400;

    private const int Height = 400;

    public byte[] Generate(long seed, int songIndex, string title, string artist, GenreCategory category)
    {
        using var bitmap = new SKBitmap(Width, Height);
        using var canvas = new SKCanvas(bitmap);

        var random = CreateRandom(seed, songIndex);
        var painter = painterRegistry.Get(category);
        painter.PaintScene(canvas, Width, Height, random);
        CoverTextRenderer.Paint(canvas, Width, Height, title, artist);

        return EncodeToBytes(bitmap);
    }

    private static Random CreateRandom(long seed, int songIndex)
    {
        return new Random(SeedHelper.ToInt32(SeedHelper.ComputeCoverSeed(seed, songIndex)));
    }

    private static byte[] EncodeToBytes(SKBitmap bitmap)
    {
        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        return data.ToArray();
    }
}
