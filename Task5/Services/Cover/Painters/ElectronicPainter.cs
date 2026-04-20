using SkiaSharp;

namespace Task5.Services.Cover.Painters;

public class ElectronicPainter : IGenreCoverPainter
{
    private static readonly (SKColor Top, SKColor Bottom, SKColor Sun1, SKColor Sun2)[] Palettes =
    [
        (new SKColor(40, 15, 60), new SKColor(240, 80, 140), new SKColor(255, 220, 80), new SKColor(255, 80, 140)),
        (new SKColor(20, 30, 80), new SKColor(80, 200, 220), new SKColor(255, 120, 200), new SKColor(80, 220, 255)),
        (new SKColor(60, 20, 20), new SKColor(255, 150, 50), new SKColor(255, 240, 120), new SKColor(255, 80, 50))
    ];

    public void PaintScene(SKCanvas canvas, int width, int height, Random random)
    {
        var palette = Palettes[random.Next(Palettes.Length)];
        PaintHelpers.VerticalGradient(canvas, width, height, palette.Top, palette.Bottom);

        var variant = random.Next(3);

        if (variant == 0)
        {
            DrawSun(canvas, width, height, palette.Sun1, palette.Sun2, palette.Top);
            DrawPerspectiveGrid(canvas, width, height);
        }
        else if (variant == 1)
        {
            MusicSilhouettes.DrawSynthKeyboard(canvas, width / 2f, height * 0.38f, 280f, new SKColor(20, 20, 30), palette.Sun1);
            DrawGridDots(canvas, width, height);
        }
        else
        {
            DrawSoundSpectrum(canvas, width, height, random, palette.Sun1);
            MusicSilhouettes.DrawHeadphones(canvas, width / 2f, height * 0.3f, 220f, new SKColor(30, 30, 40));
        }
    }

    private static void DrawSun(SKCanvas canvas, int width, int height, SKColor c1, SKColor c2, SKColor bg)
    {
        var cx = width / 2f;
        var cy = height * 0.42f;
        var radius = 90f;

        var shader = SKShader.CreateLinearGradient(
            new SKPoint(cx, cy - radius),
            new SKPoint(cx, cy + radius),
            [c1, c2],
            null,
            SKShaderTileMode.Clamp);
        using var paint = new SKPaint { Shader = shader, IsAntialias = true };
        canvas.DrawCircle(cx, cy, radius, paint);

        using var barPaint = PaintHelpers.FillPaint(bg);
        for (var i = 0; i < 5; i++)
            canvas.DrawRect(cx - radius, cy + 10 + i * 12, radius * 2, 5, barPaint);
    }

    private static void DrawPerspectiveGrid(SKCanvas canvas, int width, int height)
    {
        using var paint = PaintHelpers.StrokePaint(new SKColor(255, 240, 200, 150), 1.5f);
        var horizonY = height * 0.58f;
        var vanishingX = width / 2f;

        for (var y = horizonY; y <= height; y += 20)
            canvas.DrawLine(0, y, width, y, paint);
        for (var i = -10; i <= 10; i++)
        {
            var bottomX = vanishingX + i * width / 6f;
            canvas.DrawLine(vanishingX, horizonY, bottomX, height, paint);
        }
    }

    private static void DrawSoundSpectrum(SKCanvas canvas, int width, int height, Random random, SKColor accent)
    {
        using var paint = PaintHelpers.FillPaint(accent.WithAlpha(140));
        var bars = 24;
        var barWidth = (float)width / bars;
        for (var i = 0; i < bars; i++)
        {
            var barHeight = (float)(random.NextDouble() * height * 0.25 + 15);
            canvas.DrawRect(i * barWidth + 1, height * 0.55f - barHeight, barWidth - 2, barHeight, paint);
        }
    }

    private static void DrawGridDots(SKCanvas canvas, int width, int height)
    {
        using var paint = PaintHelpers.FillPaint(new SKColor(255, 255, 255, 70));
        for (var y = 20f; y < height * 0.55f; y += 22)
        for (var x = 20f; x < width; x += 22)
            canvas.DrawCircle(x, y, 1.5f, paint);
    }
}
