using SkiaSharp;

namespace Task5.Services.Cover.Painters;

public class AmbientPainter : IGenreCoverPainter
{
    private static readonly (SKColor Top, SKColor Bottom, SKColor Silhouette)[] Palettes =
    [
        (new SKColor(180, 190, 230), new SKColor(240, 200, 220), new SKColor(80, 90, 120)),
        (new SKColor(210, 240, 230), new SKColor(250, 220, 200), new SKColor(90, 110, 100)),
        (new SKColor(200, 180, 220), new SKColor(220, 240, 250), new SKColor(100, 90, 130)),
        (new SKColor(230, 210, 180), new SKColor(180, 210, 230), new SKColor(120, 100, 80))
    ];

    public void PaintScene(SKCanvas canvas, int width, int height, Random random)
    {
        var palette = Palettes[random.Next(Palettes.Length)];
        PaintHelpers.VerticalGradient(canvas, width, height, palette.Top, palette.Bottom);

        var variant = random.Next(3);

        if (variant == 0)
            DrawCloudWaves(canvas, width, height, random);
        else if (variant == 1)
            DrawConcentricCircles(canvas, width, height, random);
        else
            MusicSilhouettes.DrawHeadphones(canvas, width / 2f, height * 0.36f, 220f, palette.Silhouette);
    }

    private static void DrawCloudWaves(SKCanvas canvas, int width, int height, Random random)
    {
        for (var i = 0; i < 5; i++)
        {
            var baseY = height * 0.25f + i * 22f;
            using var path = new SKPath();
            path.MoveTo(0, baseY);

            for (var x = 0f; x <= width; x += 12)
            {
                var y = baseY + MathF.Sin(x * 0.02f + i * 1.3f) * 15
                    + (float)(random.NextDouble() - 0.5) * 3;
                path.LineTo(x, y);
            }

            path.LineTo(width, baseY + 40);
            path.LineTo(0, baseY + 40);
            path.Close();

            using var paint = PaintHelpers.FillPaint(new SKColor(255, 255, 255, 60));
            canvas.DrawPath(path, paint);
        }
    }

    private static void DrawConcentricCircles(SKCanvas canvas, int width, int height, Random random)
    {
        var cx = width / 2f + (float)((random.NextDouble() - 0.5) * 40);
        var cy = height * 0.35f;
        for (var i = 0; i < 8; i++)
        {
            var alpha = (byte)(60 + i * 12);
            using var paint = PaintHelpers.StrokePaint(new SKColor(255, 255, 255, alpha), 2.5f);
            canvas.DrawCircle(cx, cy, 30f + i * 18, paint);
        }
    }
}
