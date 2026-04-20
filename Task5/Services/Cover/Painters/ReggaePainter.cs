using SkiaSharp;

namespace Task5.Services.Cover.Painters;

public class ReggaePainter : IGenreCoverPainter
{
    public void PaintScene(SKCanvas canvas, int width, int height, Random random)
    {
        DrawHorizontalBands(canvas, width, height);
        DrawSun(canvas, width * 0.22f, height * 0.18f);

        var cx = width / 2f;
        var cy = height * 0.4f;
        var variant = random.Next(3);

        if (variant == 0)
            DrawPalmSilhouette(canvas, width * 0.78f, height * 0.22f);
        else if (variant == 1)
            MusicSilhouettes.DrawMicrophone(canvas, cx, cy, 200f, new SKColor(25, 25, 25));
        else
            MusicSilhouettes.DrawAcousticGuitar(canvas, cx, cy, 200f, new SKColor(30, 30, 30));
    }

    private static void DrawHorizontalBands(SKCanvas canvas, int width, int height)
    {
        var bandHeight = height / 3f;
        using var red = PaintHelpers.FillPaint(new SKColor(210, 50, 50));
        using var yellow = PaintHelpers.FillPaint(new SKColor(250, 210, 70));
        using var green = PaintHelpers.FillPaint(new SKColor(40, 130, 70));

        canvas.DrawRect(0, 0, width, bandHeight, red);
        canvas.DrawRect(0, bandHeight, width, bandHeight, yellow);
        canvas.DrawRect(0, bandHeight * 2, width, bandHeight, green);
    }

    private static void DrawSun(SKCanvas canvas, float cx, float cy)
    {
        using var paint = PaintHelpers.FillPaint(new SKColor(255, 240, 180, 220));
        canvas.DrawCircle(cx, cy, 36f, paint);
    }

    private static void DrawPalmSilhouette(SKCanvas canvas, float trunkX, float topY)
    {
        using var paint = PaintHelpers.StrokePaint(new SKColor(20, 40, 20), 5f);
        canvas.DrawLine(trunkX, topY, trunkX + 10, topY + 200, paint);

        for (var angle = -60; angle <= 60; angle += 30)
        {
            var rad = angle * MathF.PI / 180f;
            var x = trunkX + MathF.Cos(rad) * 70;
            var y = topY + MathF.Sin(rad) * 40 - 20;
            canvas.DrawLine(trunkX, topY, x, y, paint);
        }
    }
}
