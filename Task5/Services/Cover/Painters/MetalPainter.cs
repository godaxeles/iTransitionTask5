using SkiaSharp;

namespace Task5.Services.Cover.Painters;

public class MetalPainter : IGenreCoverPainter
{
    private static readonly (SKColor Bg, SKColor Accent, SKColor Glow)[] Palettes =
    [
        (new SKColor(10, 10, 12), new SKColor(200, 30, 30), new SKColor(140, 0, 0, 180)),
        (new SKColor(6, 10, 20), new SKColor(180, 50, 220), new SKColor(90, 0, 150, 180)),
        (new SKColor(10, 20, 18), new SKColor(60, 220, 140), new SKColor(0, 120, 80, 180))
    ];

    public void PaintScene(SKCanvas canvas, int width, int height, Random random)
    {
        var palette = Palettes[random.Next(Palettes.Length)];
        PaintHelpers.SolidBackground(canvas, width, height, palette.Bg);
        DrawGlow(canvas, width, height, palette.Glow);

        var cx = width / 2f;
        var cy = height * 0.38f;
        var variant = random.Next(3);

        if (variant == 0)
            DrawStarburst(canvas, width, height, random, palette.Accent);
        else if (variant == 1)
        {
            MusicSilhouettes.DrawElectricGuitar(canvas, cx, cy, 240f, palette.Accent);
            DrawFireLicks(canvas, width, height, random, palette.Accent);
        }
        else
            DrawSkullOutline(canvas, cx, cy, 180f, palette.Accent);
    }

    private static void DrawStarburst(SKCanvas canvas, int width, int height, Random random, SKColor accent)
    {
        using var paint = PaintHelpers.StrokePaint(accent, 3f);
        var cx = width / 2f;
        var cy = height * 0.4f;
        var spikes = random.Next(10, 16);
        var outerRadius = width * 0.42f;

        using var path = new SKPath();
        for (var i = 0; i < spikes; i++)
        {
            var angle = (float)(i * 2 * Math.PI / spikes);
            var x = cx + outerRadius * MathF.Cos(angle);
            var y = cy + outerRadius * MathF.Sin(angle);
            path.MoveTo(cx, cy);
            path.LineTo(x, y);
        }
        canvas.DrawPath(path, paint);
    }

    private static void DrawFireLicks(SKCanvas canvas, int width, int height, Random random, SKColor accent)
    {
        using var paint = PaintHelpers.FillPaint(accent.WithAlpha(180));
        for (var i = 0; i < 5; i++)
        {
            var baseX = width * 0.3f + i * width * 0.1f;
            using var flame = new SKPath();
            flame.MoveTo(baseX, height * 0.78f);
            flame.CubicTo(baseX - 15, height * 0.65f, baseX + 20, height * 0.55f, baseX, height * 0.42f);
            flame.CubicTo(baseX + 15, height * 0.55f, baseX + 25, height * 0.68f, baseX + 20, height * 0.78f);
            flame.Close();
            canvas.DrawPath(flame, paint);
        }
    }

    private static void DrawSkullOutline(SKCanvas canvas, float cx, float cy, float size, SKColor color)
    {
        using var fill = PaintHelpers.FillPaint(color);
        using var bgFill = PaintHelpers.FillPaint(new SKColor(10, 10, 12));

        using var skull = new SKPath();
        skull.MoveTo(cx - size * 0.45f, cy);
        skull.CubicTo(cx - size * 0.5f, cy - size * 0.55f, cx + size * 0.5f, cy - size * 0.55f, cx + size * 0.45f, cy);
        skull.LineTo(cx + size * 0.35f, cy + size * 0.3f);
        skull.LineTo(cx - size * 0.35f, cy + size * 0.3f);
        skull.Close();
        canvas.DrawPath(skull, fill);

        canvas.DrawCircle(cx - size * 0.18f, cy - size * 0.05f, size * 0.12f, bgFill);
        canvas.DrawCircle(cx + size * 0.18f, cy - size * 0.05f, size * 0.12f, bgFill);

        using var nose = new SKPath();
        nose.MoveTo(cx, cy + size * 0.05f);
        nose.LineTo(cx - size * 0.06f, cy + size * 0.18f);
        nose.LineTo(cx + size * 0.06f, cy + size * 0.18f);
        nose.Close();
        canvas.DrawPath(nose, bgFill);

        for (var i = -2; i <= 2; i++)
            canvas.DrawRect(cx + i * size * 0.08f - size * 0.02f, cy + size * 0.22f, size * 0.04f, size * 0.1f, bgFill);
    }

    private static void DrawGlow(SKCanvas canvas, int width, int height, SKColor glow)
    {
        var shader = SKShader.CreateRadialGradient(
            new SKPoint(width / 2f, height * 0.42f),
            width * 0.45f,
            [glow, SKColors.Transparent],
            null,
            SKShaderTileMode.Clamp);
        using var paint = new SKPaint { Shader = shader };
        canvas.DrawRect(0, 0, width, height, paint);
    }
}
