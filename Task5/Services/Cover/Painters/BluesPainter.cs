using SkiaSharp;

namespace Task5.Services.Cover.Painters;

public class BluesPainter : IGenreCoverPainter
{
    private static readonly (SKColor Top, SKColor Bottom, SKColor Silhouette, SKColor Moon)[] Palettes =
    [
        (new SKColor(10, 20, 50), new SKColor(30, 60, 120), new SKColor(20, 30, 50), new SKColor(245, 240, 210)),
        (new SKColor(20, 30, 70), new SKColor(60, 30, 90), new SKColor(15, 10, 30), new SKColor(230, 220, 255)),
        (new SKColor(5, 15, 35), new SKColor(20, 80, 110), new SKColor(10, 20, 30), new SKColor(240, 230, 200))
    ];

    public void PaintScene(SKCanvas canvas, int width, int height, Random random)
    {
        var palette = Palettes[random.Next(Palettes.Length)];
        PaintHelpers.VerticalGradient(canvas, width, height, palette.Top, palette.Bottom);
        DrawMoon(canvas, width, random, palette.Moon, palette.Bottom);

        var cx = width / 2f;
        var cy = height * 0.42f;
        var variant = random.Next(2);

        if (variant == 0)
            MusicSilhouettes.DrawAcousticGuitar(canvas, cx, cy, 240f, palette.Silhouette);
        else
            MusicSilhouettes.DrawMicOnStand(canvas, cx, cy, 220f, palette.Silhouette);

        DrawWaves(canvas, width, height);
    }

    private static void DrawMoon(SKCanvas canvas, int width, Random random, SKColor moonColor, SKColor bgColor)
    {
        var cx = width * 0.78f + (float)(random.NextDouble() * 10);
        var cy = 75f;
        var radius = 42f;

        using var moonPaint = PaintHelpers.FillPaint(moonColor);
        canvas.DrawCircle(cx, cy, radius, moonPaint);

        using var cutPaint = PaintHelpers.FillPaint(bgColor);
        canvas.DrawCircle(cx + 16, cy - 6, radius, cutPaint);
    }

    private static void DrawWaves(SKCanvas canvas, int width, int height)
    {
        using var paint = PaintHelpers.StrokePaint(new SKColor(100, 160, 220, 120), 2f);
        for (var i = 0; i < 3; i++)
        {
            using var path = new SKPath();
            var y = height * 0.70f + i * 16f;
            path.MoveTo(0, y);
            for (var x = 0f; x <= width; x += 10)
                path.LineTo(x, y + MathF.Sin(x * 0.03f + i) * 5);
            canvas.DrawPath(path, paint);
        }
    }
}
