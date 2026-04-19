using SkiaSharp;

namespace Task5.Services;

internal class CoverPainter(SKCanvas canvas, CoverPalette palette, Random random, int width, int height)
{
    public void Paint(string title, string artist)
    {
        PaintBackground();
        PaintDecorations();
        PaintOverlayGradient();
        PaintTitle(title);
        PaintArtist(artist);
    }

    private void PaintBackground()
    {
        var shader = SKShader.CreateLinearGradient(
            new SKPoint(0, 0),
            new SKPoint(width, height),
            [palette.Primary, palette.Secondary],
            null,
            SKShaderTileMode.Clamp);

        using var paint = new SKPaint { Shader = shader };
        canvas.DrawRect(0, 0, width, height, paint);
    }

    private void PaintDecorations()
    {
        var count = random.Next(4, 8);
        for (var i = 0; i < count; i++)
            PaintDecorativeRing();
    }

    private void PaintDecorativeRing()
    {
        var cx = (float)(random.NextDouble() * width);
        var cy = (float)(random.NextDouble() * height * 0.65);
        var radius = (float)(random.NextDouble() * 120 + 30);
        var alpha = (byte)random.Next(25, 70);

        using var paint = new SKPaint
        {
            Color = palette.Accent.WithAlpha(alpha),
            Style = SKPaintStyle.Stroke,
            StrokeWidth = (float)(random.NextDouble() * 3 + 0.5),
            IsAntialias = true
        };

        canvas.DrawCircle(cx, cy, radius, paint);

        var innerRadius = radius * (float)(random.NextDouble() * 0.4 + 0.5);
        canvas.DrawCircle(cx, cy, innerRadius, paint);
    }

    private void PaintOverlayGradient()
    {
        var overlayStart = height * 0.50f;

        var shader = SKShader.CreateLinearGradient(
            new SKPoint(0, overlayStart),
            new SKPoint(0, height),
            [SKColors.Transparent, new SKColor(0, 0, 0, 210)],
            null,
            SKShaderTileMode.Clamp);

        using var paint = new SKPaint { Shader = shader };
        canvas.DrawRect(0, overlayStart, width, height - overlayStart, paint);
    }

    private void PaintTitle(string title)
    {
        using var paint = CreateTitlePaint();

        var lines = WrapText(title, paint, width - 48).ToList();
        var lineHeight = paint.TextSize * 1.3f;
        var totalTextHeight = lines.Count * lineHeight;
        var startY = height * 0.74f - totalTextHeight / 2 + paint.TextSize;

        foreach (var line in lines)
        {
            canvas.DrawText(line, width / 2f, startY, paint);
            startY += lineHeight;
        }
    }

    private void PaintArtist(string artist)
    {
        using var paint = CreateArtistPaint();
        canvas.DrawText(artist, width / 2f, height * 0.92f, paint);
    }

    private static SKPaint CreateTitlePaint()
    {
        return new SKPaint
        {
            Color = SKColors.White,
            TextSize = 32f,
            IsAntialias = true,
            TextAlign = SKTextAlign.Center,
            FakeBoldText = true,
            Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold)
                       ?? SKTypeface.Default
        };
    }

    private static SKPaint CreateArtistPaint()
    {
        return new SKPaint
        {
            Color = new SKColor(210, 210, 210, 210),
            TextSize = 18f,
            IsAntialias = true,
            TextAlign = SKTextAlign.Center,
            Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Normal)
                       ?? SKTypeface.Default
        };
    }

    private static IEnumerable<string> WrapText(string text, SKPaint paint, float maxWidth)
    {
        var words = text.Split(' ');
        var current = string.Empty;

        foreach (var word in words)
        {
            var candidate = current.Length == 0 ? word : $"{current} {word}";

            if (paint.MeasureText(candidate) > maxWidth && current.Length > 0)
            {
                yield return current;
                current = word;
            }
            else
            {
                current = candidate;
            }
        }

        if (current.Length > 0)
            yield return current;
    }
}
