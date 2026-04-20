using SkiaSharp;

namespace Task5.Services.Cover.Painters;

public class PopPainter : IGenreCoverPainter
{
    private static readonly (SKColor Start, SKColor End, SKColor Silhouette)[] Palettes =
    [
        (new SKColor(255, 100, 180), new SKColor(120, 160, 255), new SKColor(255, 255, 255)),
        (new SKColor(255, 200, 80), new SKColor(255, 80, 140), new SKColor(40, 20, 80)),
        (new SKColor(120, 220, 240), new SKColor(180, 120, 255), new SKColor(255, 255, 255)),
        (new SKColor(255, 170, 120), new SKColor(255, 80, 200), new SKColor(40, 20, 80))
    ];

    public void PaintScene(SKCanvas canvas, int width, int height, Random random)
    {
        var palette = Palettes[random.Next(Palettes.Length)];
        PaintHelpers.DiagonalGradient(canvas, width, height, palette.Start, palette.End);
        DrawBubbles(canvas, width, height, random);

        var cx = width / 2f;
        var cy = height * 0.36f;
        var variant = random.Next(3);

        if (variant == 0)
            MusicSilhouettes.DrawMicrophone(canvas, cx, cy, 220f, palette.Silhouette);
        else if (variant == 1)
            MusicSilhouettes.DrawCassette(canvas, cx, cy, 220f, palette.Silhouette, palette.End);
        else
            DrawDiscoBall(canvas, cx, cy, 100f, palette.Silhouette);
    }

    private static void DrawBubbles(SKCanvas canvas, int width, int height, Random random)
    {
        for (var i = 0; i < 30; i++)
        {
            var alpha = (byte)random.Next(100, 200);
            using var paint = PaintHelpers.FillPaint(new SKColor(255, 255, 255, alpha));
            var x = (float)(random.NextDouble() * width);
            var y = (float)(random.NextDouble() * height * 0.55);
            var r = (float)(random.NextDouble() * 16 + 3);
            canvas.DrawCircle(x, y, r, paint);
        }
    }

    private static void DrawDiscoBall(SKCanvas canvas, float cx, float cy, float radius, SKColor color)
    {
        using var body = PaintHelpers.FillPaint(color);
        canvas.DrawCircle(cx, cy, radius, body);

        using var facet = PaintHelpers.StrokePaint(new SKColor(0, 0, 0, 80), 1.5f);
        for (var r = radius * 0.25f; r < radius; r += radius * 0.18f)
            canvas.DrawCircle(cx, cy, r, facet);
        for (var a = 0; a < 360; a += 30)
        {
            var rad = a * MathF.PI / 180f;
            canvas.DrawLine(cx, cy, cx + MathF.Cos(rad) * radius, cy + MathF.Sin(rad) * radius, facet);
        }
    }
}
