using SkiaSharp;

namespace Task5.Services.Cover.Painters;

public class ClassicalPainter : IGenreCoverPainter
{
    private static readonly (SKColor Top, SKColor Bottom, SKColor Silhouette, SKColor Border)[] Palettes =
    [
        (new SKColor(245, 235, 210), new SKColor(200, 175, 130), new SKColor(90, 60, 30), new SKColor(180, 140, 60)),
        (new SKColor(230, 220, 240), new SKColor(140, 120, 170), new SKColor(60, 40, 90), new SKColor(120, 90, 160)),
        (new SKColor(235, 240, 230), new SKColor(130, 150, 140), new SKColor(40, 70, 50), new SKColor(90, 130, 100))
    ];

    public void PaintScene(SKCanvas canvas, int width, int height, Random random)
    {
        var palette = Palettes[random.Next(Palettes.Length)];
        PaintHelpers.VerticalGradient(canvas, width, height, palette.Top, palette.Bottom);

        var cx = width / 2f;
        var cy = height * 0.4f;
        var variant = random.Next(3);

        if (variant == 0)
            MusicSilhouettes.DrawTrebleClef(canvas, cx, cy, 220f, palette.Silhouette);
        else if (variant == 1)
            DrawGrandPiano(canvas, cx, cy, 240f, palette.Silhouette);
        else
            DrawColumns(canvas, width, height, palette.Silhouette);

        DrawStaffLines(canvas, width, height, palette.Border);
        DrawBorder(canvas, width, height, palette.Border);
    }

    private static void DrawGrandPiano(SKCanvas canvas, float cx, float cy, float size, SKColor color)
    {
        using var fill = PaintHelpers.FillPaint(color);
        using var body = new SKPath();
        body.MoveTo(cx - size * 0.35f, cy);
        body.LineTo(cx + size * 0.45f, cy);
        body.CubicTo(cx + size * 0.55f, cy, cx + size * 0.5f, cy - size * 0.3f, cx + size * 0.3f, cy - size * 0.35f);
        body.LineTo(cx - size * 0.3f, cy - size * 0.35f);
        body.LineTo(cx - size * 0.35f, cy);
        body.Close();
        canvas.DrawPath(body, fill);

        using var whitePaint = PaintHelpers.FillPaint(new SKColor(245, 240, 225));
        canvas.DrawRect(cx - size * 0.35f, cy, size * 0.8f, size * 0.08f, whitePaint);
        using var blackPaint = PaintHelpers.FillPaint(new SKColor(20, 18, 14));
        var keyWidth = size * 0.8f / 14;
        for (var i = 0; i < 14; i++)
            canvas.DrawRect(cx - size * 0.35f + i * keyWidth, cy, 1f, size * 0.08f, blackPaint);

        using var legPaint = PaintHelpers.FillPaint(color);
        canvas.DrawRect(cx - size * 0.1f, cy + size * 0.08f, size * 0.05f, size * 0.15f, legPaint);
        canvas.DrawRect(cx + size * 0.3f, cy + size * 0.08f, size * 0.05f, size * 0.15f, legPaint);
    }

    private static void DrawColumns(SKCanvas canvas, int width, int height, SKColor color)
    {
        using var paint = PaintHelpers.FillPaint(color.WithAlpha(160));
        var columnCount = 5;
        var spacing = width / (columnCount + 1);
        var columnWidth = 18f;

        for (var i = 1; i <= columnCount; i++)
        {
            var x = i * spacing - columnWidth / 2;
            canvas.DrawRect(x, height * 0.15f, columnWidth, height * 0.55f, paint);
            canvas.DrawRect(x - 5, height * 0.15f, columnWidth + 10, 8, paint);
            canvas.DrawRect(x - 5, height * 0.7f - 8, columnWidth + 10, 8, paint);
        }
    }

    private static void DrawStaffLines(SKCanvas canvas, int width, int height, SKColor color)
    {
        using var paint = PaintHelpers.StrokePaint(color.WithAlpha(80), 1.2f);
        for (var i = 0; i < 5; i++)
            canvas.DrawLine(40, height * 0.18f + i * 8, width - 40, height * 0.18f + i * 8, paint);
    }

    private static void DrawBorder(SKCanvas canvas, int width, int height, SKColor color)
    {
        using var paint = PaintHelpers.StrokePaint(color, 3f);
        canvas.DrawRect(12, 12, width - 24, height - 24, paint);
    }
}
