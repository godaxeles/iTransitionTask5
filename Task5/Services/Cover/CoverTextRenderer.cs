using SkiaSharp;

namespace Task5.Services.Cover;

public static class CoverTextRenderer
{
    public static void Paint(SKCanvas canvas, int width, int height, string title, string artist)
    {
        PaintOverlayGradient(canvas, width, height);
        PaintTitle(canvas, width, height, title);
        PaintArtist(canvas, width, height, artist);
    }

    private static void PaintOverlayGradient(SKCanvas canvas, int width, int height)
    {
        var overlayStart = height * 0.45f;

        var shader = SKShader.CreateLinearGradient(
            new SKPoint(0, overlayStart),
            new SKPoint(0, height),
            [SKColors.Transparent, new SKColor(0, 0, 0, 225)],
            null,
            SKShaderTileMode.Clamp);

        using var paint = new SKPaint { Shader = shader };
        canvas.DrawRect(0, overlayStart, width, height - overlayStart, paint);
    }

    private static void PaintTitle(SKCanvas canvas, int width, int height, string title)
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

    private static void PaintArtist(SKCanvas canvas, int width, int height, string artist)
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
            Color = new SKColor(210, 210, 210, 220),
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
