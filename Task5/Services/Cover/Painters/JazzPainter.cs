using SkiaSharp;

namespace Task5.Services.Cover.Painters;

public class JazzPainter : IGenreCoverPainter
{
    private static readonly (SKColor Start, SKColor End, SKColor Silhouette, SKColor Label)[] Palettes =
    [
        (new SKColor(90, 45, 20), new SKColor(200, 130, 60), new SKColor(30, 20, 10), new SKColor(220, 90, 60)),
        (new SKColor(40, 20, 60), new SKColor(180, 80, 120), new SKColor(20, 10, 30), new SKColor(240, 200, 80)),
        (new SKColor(70, 50, 30), new SKColor(220, 170, 80), new SKColor(30, 20, 10), new SKColor(200, 50, 50))
    ];

    public void PaintScene(SKCanvas canvas, int width, int height, Random random)
    {
        var palette = Palettes[random.Next(Palettes.Length)];
        PaintHelpers.DiagonalGradient(canvas, width, height, palette.Start, palette.End);

        var cx = width / 2f;
        var cy = height * 0.38f;
        var variant = random.Next(3);

        if (variant == 0)
            MusicSilhouettes.DrawSaxophone(canvas, cx, cy, 220f, palette.Silhouette);
        else if (variant == 1)
            MusicSilhouettes.DrawVinyl(canvas, cx, cy, 240f, palette.Silhouette, palette.Label);
        else
            MusicSilhouettes.DrawTrumpet(canvas, cx, cy, 220f, palette.Silhouette);

        DrawSmokeCurves(canvas, width, height, random);
    }

    private static void DrawSmokeCurves(SKCanvas canvas, int width, int height, Random random)
    {
        using var paint = PaintHelpers.StrokePaint(new SKColor(255, 220, 180, 60), 2f);
        for (var i = 0; i < 3; i++)
        {
            using var path = new SKPath();
            var startX = width * 0.15f + (float)(random.NextDouble() * width * 0.1);
            path.MoveTo(startX, height * 0.6f);
            for (var y = height * 0.6f; y > height * 0.1f; y -= 15)
            {
                startX += (float)((random.NextDouble() - 0.5) * 25);
                path.LineTo(startX, y);
            }
            canvas.DrawPath(path, paint);
        }
    }
}
