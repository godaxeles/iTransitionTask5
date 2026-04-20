using SkiaSharp;

namespace Task5.Services.Cover.Painters;

public static class PaintHelpers
{
    public static void VerticalGradient(SKCanvas canvas, int width, int height, SKColor top, SKColor bottom)
    {
        var shader = SKShader.CreateLinearGradient(
            new SKPoint(0, 0),
            new SKPoint(0, height),
            [top, bottom],
            null,
            SKShaderTileMode.Clamp);

        using var paint = new SKPaint { Shader = shader };
        canvas.DrawRect(0, 0, width, height, paint);
    }

    public static void DiagonalGradient(SKCanvas canvas, int width, int height, SKColor start, SKColor end)
    {
        var shader = SKShader.CreateLinearGradient(
            new SKPoint(0, 0),
            new SKPoint(width, height),
            [start, end],
            null,
            SKShaderTileMode.Clamp);

        using var paint = new SKPaint { Shader = shader };
        canvas.DrawRect(0, 0, width, height, paint);
    }

    public static void SolidBackground(SKCanvas canvas, int width, int height, SKColor color)
    {
        using var paint = new SKPaint { Color = color };
        canvas.DrawRect(0, 0, width, height, paint);
    }

    public static SKPaint StrokePaint(SKColor color, float strokeWidth)
    {
        return new SKPaint
        {
            Color = color,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = strokeWidth,
            IsAntialias = true,
            StrokeCap = SKStrokeCap.Round
        };
    }

    public static SKPaint FillPaint(SKColor color)
    {
        return new SKPaint
        {
            Color = color,
            Style = SKPaintStyle.Fill,
            IsAntialias = true
        };
    }
}
