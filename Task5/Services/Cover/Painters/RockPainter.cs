using SkiaSharp;

namespace Task5.Services.Cover.Painters;

public class RockPainter : IGenreCoverPainter
{
    private static readonly (SKColor Top, SKColor Bottom, SKColor Silhouette, SKColor Glow)[] Palettes =
    [
        (new SKColor(22, 22, 30), new SKColor(80, 15, 25), new SKColor(255, 220, 80), new SKColor(200, 40, 40, 120)),
        (new SKColor(15, 15, 25), new SKColor(15, 40, 90), new SKColor(255, 240, 120), new SKColor(60, 100, 220, 140)),
        (new SKColor(30, 10, 30), new SKColor(90, 25, 100), new SKColor(255, 200, 60), new SKColor(220, 60, 180, 130))
    ];

    public void PaintScene(SKCanvas canvas, int width, int height, Random random)
    {
        var palette = Palettes[random.Next(Palettes.Length)];
        PaintHelpers.VerticalGradient(canvas, width, height, palette.Top, palette.Bottom);
        DrawGlow(canvas, width, height, palette.Glow);

        var cx = width / 2f;
        var cy = height * 0.36f;
        var variant = random.Next(3);

        if (variant == 0)
            MusicSilhouettes.DrawElectricGuitar(canvas, cx, cy, 220f, palette.Silhouette);
        else if (variant == 1)
            MusicSilhouettes.DrawDrumKit(canvas, cx, cy, 260f, palette.Silhouette, new SKColor(240, 230, 220));
        else
            DrawStageBeams(canvas, width, height, random, palette.Silhouette);
    }

    private static void DrawStageBeams(SKCanvas canvas, int width, int height, Random random, SKColor accent)
    {
        var originX = width / 2f;
        var originY = height * 0.18f;
        var beams = 5 + random.Next(3);

        for (var i = 0; i < beams; i++)
        {
            var angle = (float)(-Math.PI / 2 + (i - beams / 2f) * 0.28);
            var length = height * 0.9f;
            var endX = originX + MathF.Cos(angle) * length;
            var endY = originY + MathF.Sin(angle) * length;

            using var path = new SKPath();
            path.MoveTo(originX, originY);
            path.LineTo(endX - 12, endY);
            path.LineTo(endX + 12, endY);
            path.Close();

            var shader = SKShader.CreateLinearGradient(
                new SKPoint(originX, originY),
                new SKPoint(endX, endY),
                [accent.WithAlpha(200), accent.WithAlpha(0)],
                null,
                SKShaderTileMode.Clamp);
            using var paint = new SKPaint { Shader = shader, IsAntialias = true };
            canvas.DrawPath(path, paint);
        }
    }

    private static void DrawGlow(SKCanvas canvas, int width, int height, SKColor glow)
    {
        var shader = SKShader.CreateRadialGradient(
            new SKPoint(width / 2f, height * 0.5f),
            width * 0.6f,
            [glow, SKColors.Transparent],
            null,
            SKShaderTileMode.Clamp);
        using var paint = new SKPaint { Shader = shader };
        canvas.DrawRect(0, 0, width, height, paint);
    }
}
