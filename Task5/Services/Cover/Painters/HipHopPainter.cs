using SkiaSharp;

namespace Task5.Services.Cover.Painters;

public class HipHopPainter : IGenreCoverPainter
{
    private static readonly (SKColor Bg, SKColor Accent, SKColor Silhouette)[] Palettes =
    [
        (new SKColor(20, 20, 20), new SKColor(240, 200, 30), new SKColor(240, 200, 30)),
        (new SKColor(15, 20, 30), new SKColor(255, 100, 80), new SKColor(255, 100, 80)),
        (new SKColor(20, 10, 30), new SKColor(160, 255, 80), new SKColor(160, 255, 80))
    ];

    public void PaintScene(SKCanvas canvas, int width, int height, Random random)
    {
        var palette = Palettes[random.Next(Palettes.Length)];
        PaintHelpers.SolidBackground(canvas, width, height, palette.Bg);
        DrawDiagonalStripes(canvas, width, height, random, palette.Accent);

        var cx = width / 2f;
        var cy = height * 0.38f;
        var variant = random.Next(3);

        if (variant == 0)
            MusicSilhouettes.DrawTurntable(canvas, cx, cy, 240f, new SKColor(30, 30, 30), palette.Silhouette);
        else if (variant == 1)
            MusicSilhouettes.DrawMicrophone(canvas, cx, cy, 220f, palette.Silhouette);
        else
            MusicSilhouettes.DrawCassette(canvas, cx, cy, 240f, new SKColor(40, 40, 40), palette.Silhouette);
    }

    private static void DrawDiagonalStripes(SKCanvas canvas, int width, int height, Random random, SKColor accent)
    {
        using var paint = PaintHelpers.FillPaint(accent.WithAlpha(80));
        var stripeCount = 2 + random.Next(2);
        for (var i = 0; i < stripeCount; i++)
        {
            var offsetX = (float)(random.NextDouble() * width);
            using var path = new SKPath();
            path.MoveTo(offsetX, 0);
            path.LineTo(offsetX + 30, 0);
            path.LineTo(offsetX - 90, height * 0.6f);
            path.LineTo(offsetX - 120, height * 0.6f);
            path.Close();
            canvas.DrawPath(path, paint);
        }
    }
}
