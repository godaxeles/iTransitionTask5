using SkiaSharp;

namespace Task5.Services.Cover.Painters;

public class FolkPainter : IGenreCoverPainter
{
    private static readonly (SKColor Top, SKColor Bottom, SKColor Silhouette, SKColor Accent)[] Palettes =
    [
        (new SKColor(80, 60, 40), new SKColor(30, 40, 30), new SKColor(15, 20, 15), new SKColor(235, 225, 190)),
        (new SKColor(40, 30, 60), new SKColor(60, 80, 100), new SKColor(10, 20, 30), new SKColor(230, 220, 240)),
        (new SKColor(100, 70, 50), new SKColor(50, 90, 60), new SKColor(20, 30, 20), new SKColor(250, 230, 180))
    ];

    public void PaintScene(SKCanvas canvas, int width, int height, Random random)
    {
        var palette = Palettes[random.Next(Palettes.Length)];
        PaintHelpers.VerticalGradient(canvas, width, height, palette.Top, palette.Bottom);

        var cx = width / 2f;
        var cy = height * 0.42f;
        var variant = random.Next(2);

        if (variant == 0)
        {
            DrawMoon(canvas, width, palette.Accent);
            DrawTrees(canvas, width, height, random, palette.Silhouette);
            MusicSilhouettes.DrawAcousticGuitar(canvas, cx, cy, 200f, palette.Silhouette);
        }
        else
        {
            DrawSun(canvas, width, height, palette.Accent);
            DrawRollingHills(canvas, width, height, random, palette.Silhouette);
            DrawBanjo(canvas, cx, cy, 200f, palette.Silhouette, palette.Accent);
        }
    }

    private static void DrawMoon(SKCanvas canvas, int width, SKColor color)
    {
        using var paint = PaintHelpers.FillPaint(color);
        canvas.DrawCircle(width * 0.78f, 70f, 32f, paint);
    }

    private static void DrawSun(SKCanvas canvas, int width, int height, SKColor color)
    {
        using var paint = PaintHelpers.FillPaint(color.WithAlpha(200));
        canvas.DrawCircle(width * 0.22f, height * 0.22f, 44f, paint);
    }

    private static void DrawTrees(SKCanvas canvas, int width, int height, Random random, SKColor color)
    {
        using var paint = PaintHelpers.FillPaint(color);
        for (var i = 0; i < 5; i++)
        {
            var x = (float)(random.NextDouble() * width);
            var treeHeight = 80f + (float)(random.NextDouble() * 50);
            var baseY = height * 0.78f;
            using var path = new SKPath();
            path.MoveTo(x, baseY - treeHeight);
            path.LineTo(x + 18, baseY);
            path.LineTo(x - 18, baseY);
            path.Close();
            canvas.DrawPath(path, paint);
        }
    }

    private static void DrawRollingHills(SKCanvas canvas, int width, int height, Random random, SKColor color)
    {
        for (var i = 0; i < 3; i++)
        {
            var alpha = (byte)(100 + i * 40);
            using var paint = PaintHelpers.FillPaint(color.WithAlpha(alpha));
            using var path = new SKPath();
            var baseY = height * 0.65f + i * 30f + (float)(random.NextDouble() * 10);
            path.MoveTo(0, baseY);
            for (var x = 0f; x <= width; x += 20)
                path.LineTo(x, baseY + MathF.Sin(x * 0.02f + i) * 15);
            path.LineTo(width, height);
            path.LineTo(0, height);
            path.Close();
            canvas.DrawPath(path, paint);
        }
    }

    private static void DrawBanjo(SKCanvas canvas, float cx, float cy, float size, SKColor body, SKColor accent)
    {
        using var bodyPaint = PaintHelpers.FillPaint(body);
        canvas.DrawCircle(cx, cy + size * 0.15f, size * 0.3f, bodyPaint);

        using var headPaint = PaintHelpers.FillPaint(accent);
        canvas.DrawCircle(cx, cy + size * 0.15f, size * 0.24f, headPaint);

        using var neckPaint = PaintHelpers.FillPaint(body);
        canvas.DrawRect(cx - size * 0.04f, cy - size * 0.5f, size * 0.08f, size * 0.55f, neckPaint);
        canvas.DrawRect(cx - size * 0.07f, cy - size * 0.55f, size * 0.14f, size * 0.08f, neckPaint);
    }
}
