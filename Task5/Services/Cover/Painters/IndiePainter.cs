using SkiaSharp;

namespace Task5.Services.Cover.Painters;

public class IndiePainter : IGenreCoverPainter
{
    private static readonly SKColor[][] Palettes =
    [
        [new SKColor(220, 100, 80), new SKColor(80, 140, 180), new SKColor(240, 200, 100), new SKColor(140, 180, 120)],
        [new SKColor(200, 130, 160), new SKColor(130, 180, 170), new SKColor(230, 180, 120), new SKColor(150, 130, 200)],
        [new SKColor(100, 160, 140), new SKColor(210, 140, 90), new SKColor(170, 200, 220), new SKColor(220, 180, 150)]
    ];

    private static readonly SKColor[] Backgrounds =
    [
        new(240, 230, 215),
        new(230, 235, 225),
        new(240, 228, 230),
        new(225, 225, 230)
    ];

    public void PaintScene(SKCanvas canvas, int width, int height, Random random)
    {
        var bg = Backgrounds[random.Next(Backgrounds.Length)];
        var palette = Palettes[random.Next(Palettes.Length)];
        PaintHelpers.SolidBackground(canvas, width, height, bg);

        var cx = width / 2f;
        var cy = height * 0.36f;
        var variant = random.Next(3);

        if (variant == 0)
            DrawGeometricShapes(canvas, width, height, random, palette);
        else if (variant == 1)
        {
            DrawGeometricShapes(canvas, width, height, random, palette);
            MusicSilhouettes.DrawCassette(canvas, cx, cy, 200f, palette[0], palette[1]);
        }
        else
            DrawLineArtWithVinyl(canvas, width, height, cx, cy, random, palette);
    }

    private static void DrawGeometricShapes(SKCanvas canvas, int width, int height, Random random, SKColor[] palette)
    {
        var shapeCount = 4 + random.Next(3);
        for (var i = 0; i < shapeCount; i++)
        {
            var color = palette[random.Next(palette.Length)];
            using var paint = PaintHelpers.FillPaint(color.WithAlpha(210));
            var shapeType = random.Next(3);
            var cx = (float)(random.NextDouble() * width);
            var cy = (float)(random.NextDouble() * height * 0.5);
            var size = 30f + (float)(random.NextDouble() * 50);

            switch (shapeType)
            {
                case 0:
                    canvas.DrawCircle(cx, cy, size, paint);
                    break;
                case 1:
                    canvas.DrawRect(cx - size / 2, cy - size / 2, size, size, paint);
                    break;
                default:
                    using (var path = new SKPath())
                    {
                        path.MoveTo(cx, cy - size);
                        path.LineTo(cx + size, cy + size);
                        path.LineTo(cx - size, cy + size);
                        path.Close();
                        canvas.DrawPath(path, paint);
                    }
                    break;
            }
        }
    }

    private static void DrawLineArtWithVinyl(SKCanvas canvas, int width, int height, float cx, float cy, Random random, SKColor[] palette)
    {
        MusicSilhouettes.DrawVinyl(canvas, cx, cy, 200f, palette[0], palette[1]);

        for (var i = 0; i < 3; i++)
        {
            var color = palette[random.Next(palette.Length)];
            using var paint = PaintHelpers.StrokePaint(color.WithAlpha(200), 2.5f);
            var startX = (float)(random.NextDouble() * width);
            var startY = (float)(random.NextDouble() * height * 0.25);
            var endX = (float)(random.NextDouble() * width);
            var endY = startY;
            canvas.DrawLine(startX, startY, endX, endY, paint);
        }
    }
}
