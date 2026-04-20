using SkiaSharp;

namespace Task5.Services.Cover.Painters;

public class CountryPainter : IGenreCoverPainter
{
    private static readonly (SKColor Top, SKColor Bottom, SKColor Silhouette, SKColor Sun)[] Palettes =
    [
        (new SKColor(255, 160, 90), new SKColor(180, 70, 100), new SKColor(60, 30, 45), new SKColor(255, 230, 130)),
        (new SKColor(250, 200, 120), new SKColor(220, 110, 70), new SKColor(80, 50, 30), new SKColor(255, 245, 180)),
        (new SKColor(220, 130, 150), new SKColor(100, 60, 110), new SKColor(40, 30, 60), new SKColor(255, 200, 200))
    ];

    public void PaintScene(SKCanvas canvas, int width, int height, Random random)
    {
        var palette = Palettes[random.Next(Palettes.Length)];
        PaintHelpers.VerticalGradient(canvas, width, height, palette.Top, palette.Bottom);
        DrawSun(canvas, width, height, palette.Sun);

        var cx = width / 2f;
        var cy = height * 0.48f;
        var variant = random.Next(2);

        if (variant == 0)
        {
            DrawMountains(canvas, width, height, random, palette.Silhouette);
            MusicSilhouettes.DrawAcousticGuitar(canvas, cx, cy, 200f, palette.Silhouette);
        }
        else
        {
            DrawWheatField(canvas, width, height, random, palette.Silhouette);
            DrawCowboyHat(canvas, cx, height * 0.32f, 180f, palette.Silhouette);
        }
    }

    private static void DrawSun(SKCanvas canvas, int width, int height, SKColor color)
    {
        using var paint = PaintHelpers.FillPaint(color);
        canvas.DrawCircle(width * 0.78f, height * 0.22f, 44f, paint);
    }

    private static void DrawCowboyHat(SKCanvas canvas, float cx, float cy, float size, SKColor color)
    {
        using var fill = PaintHelpers.FillPaint(color);
        using var brim = new SKPath();
        brim.MoveTo(cx - size * 0.5f, cy + size * 0.05f);
        brim.CubicTo(cx - size * 0.5f, cy + size * 0.25f, cx + size * 0.5f, cy + size * 0.25f, cx + size * 0.5f, cy + size * 0.05f);
        brim.CubicTo(cx + size * 0.45f, cy, cx - size * 0.45f, cy, cx - size * 0.5f, cy + size * 0.05f);
        brim.Close();
        canvas.DrawPath(brim, fill);

        using var crown = new SKPath();
        crown.MoveTo(cx - size * 0.25f, cy + size * 0.05f);
        crown.CubicTo(cx - size * 0.3f, cy - size * 0.2f, cx + size * 0.3f, cy - size * 0.2f, cx + size * 0.25f, cy + size * 0.05f);
        crown.Close();
        canvas.DrawPath(crown, fill);
    }

    private static void DrawMountains(SKCanvas canvas, int width, int height, Random random, SKColor color)
    {
        using var paint = PaintHelpers.FillPaint(color.WithAlpha(200));
        using var path = new SKPath();
        path.MoveTo(0, height * 0.7f);
        var peaks = 5;
        for (var i = 0; i <= peaks; i++)
        {
            var x = i * width / (float)peaks;
            var y = height * (0.6f + (float)(random.NextDouble() * 0.12));
            path.LineTo(x, y);
        }
        path.LineTo(width, height);
        path.LineTo(0, height);
        path.Close();
        canvas.DrawPath(path, paint);
    }

    private static void DrawWheatField(SKCanvas canvas, int width, int height, Random random, SKColor color)
    {
        using var groundPaint = PaintHelpers.FillPaint(color);
        canvas.DrawRect(0, height * 0.65f, width, height * 0.35f, groundPaint);

        using var stalkPaint = PaintHelpers.StrokePaint(color.WithAlpha(180), 1.5f);
        for (var i = 0; i < 40; i++)
        {
            var x = (float)(random.NextDouble() * width);
            var baseY = height * 0.65f + (float)(random.NextDouble() * 20);
            canvas.DrawLine(x, baseY, x + (float)((random.NextDouble() - 0.5) * 6), baseY - 30, stalkPaint);
        }
    }
}
