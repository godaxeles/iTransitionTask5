using SkiaSharp;

namespace Task5.Services.Cover.Painters;

public class PunkPainter : IGenreCoverPainter
{
    private static readonly (SKColor Bg, SKColor Accent)[] Palettes =
    [
        (new SKColor(15, 15, 15), new SKColor(235, 40, 130)),
        (new SKColor(15, 15, 15), new SKColor(255, 220, 40)),
        (new SKColor(15, 15, 15), new SKColor(60, 220, 200))
    ];

    public void PaintScene(SKCanvas canvas, int width, int height, Random random)
    {
        var palette = Palettes[random.Next(Palettes.Length)];
        PaintHelpers.SolidBackground(canvas, width, height, palette.Bg);
        DrawSplatter(canvas, width, height, random);

        var cx = width / 2f;
        var cy = height * 0.38f;
        var variant = random.Next(3);

        if (variant == 0)
            DrawAnarchySymbol(canvas, cx, cy, palette.Accent);
        else if (variant == 1)
            MusicSilhouettes.DrawElectricGuitar(canvas, cx, cy, 220f, palette.Accent);
        else
            DrawSafetyPin(canvas, cx, cy, 200f, palette.Accent);
    }

    private static void DrawAnarchySymbol(SKCanvas canvas, float cx, float cy, SKColor accent)
    {
        using var circlePaint = PaintHelpers.StrokePaint(accent, 10f);
        canvas.DrawCircle(cx, cy, 80f, circlePaint);

        using var aPaint = PaintHelpers.StrokePaint(accent, 9f);
        canvas.DrawLine(cx - 60, cy + 40, cx, cy - 50, aPaint);
        canvas.DrawLine(cx, cy - 50, cx + 60, cy + 40, aPaint);
        canvas.DrawLine(cx - 40, cy + 10, cx + 40, cy + 10, aPaint);
    }

    private static void DrawSafetyPin(SKCanvas canvas, float cx, float cy, float size, SKColor color)
    {
        using var stroke = PaintHelpers.StrokePaint(color, size * 0.03f);
        var oval = new SKRect(cx - size * 0.35f, cy - size * 0.15f, cx + size * 0.35f, cy + size * 0.15f);
        canvas.DrawOval(oval, stroke);
        canvas.DrawLine(cx - size * 0.35f, cy, cx + size * 0.45f, cy + size * 0.05f, stroke);

        using var fill = PaintHelpers.FillPaint(color);
        canvas.DrawCircle(cx - size * 0.35f, cy, size * 0.06f, fill);
    }

    private static void DrawSplatter(SKCanvas canvas, int width, int height, Random random)
    {
        using var paint = PaintHelpers.FillPaint(new SKColor(255, 255, 255, 160));
        for (var i = 0; i < 35; i++)
        {
            var x = (float)(random.NextDouble() * width);
            var y = (float)(random.NextDouble() * height * 0.6);
            var r = (float)(random.NextDouble() * 4 + 1);
            canvas.DrawCircle(x, y, r, paint);
        }
    }
}
