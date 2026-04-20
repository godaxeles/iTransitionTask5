using SkiaSharp;

namespace Task5.Services.Cover.Painters;

public static class MusicSilhouettes
{
    public static void DrawElectricGuitar(SKCanvas canvas, float cx, float cy, float size, SKColor color)
    {
        using var fill = PaintHelpers.FillPaint(color);
        using var dark = PaintHelpers.FillPaint(new SKColor(0, 0, 0, 140));

        canvas.Save();
        canvas.Translate(cx, cy);
        canvas.RotateDegrees(-15);

        canvas.DrawOval(0, size * 0.18f, size * 0.24f, size * 0.20f, fill);
        canvas.DrawOval(0, -size * 0.02f, size * 0.18f, size * 0.16f, fill);
        canvas.DrawRect(-size * 0.18f, -size * 0.02f, size * 0.36f, size * 0.20f, fill);

        canvas.DrawOval(0, size * 0.18f, size * 0.05f, size * 0.05f, dark);
        canvas.DrawRect(-size * 0.16f, size * 0.28f, size * 0.32f, size * 0.02f, dark);
        canvas.DrawRect(-size * 0.16f, size * 0.32f, size * 0.32f, size * 0.015f, dark);

        canvas.DrawRect(-size * 0.035f, -size * 0.62f, size * 0.07f, size * 0.44f, fill);
        canvas.DrawRect(-size * 0.10f, -size * 0.72f, size * 0.20f, size * 0.12f, fill);

        using var frets = PaintHelpers.StrokePaint(dark.Color, size * 0.004f);
        for (var i = 1; i <= 4; i++)
            canvas.DrawLine(-size * 0.035f, -size * 0.20f - i * size * 0.08f, size * 0.035f, -size * 0.20f - i * size * 0.08f, frets);

        canvas.Restore();
    }

    public static void DrawAcousticGuitar(SKCanvas canvas, float cx, float cy, float size, SKColor color)
    {
        using var fill = PaintHelpers.FillPaint(color);
        using var holePaint = PaintHelpers.FillPaint(new SKColor(0, 0, 0, 180));
        using var stroke = PaintHelpers.StrokePaint(color, size * 0.04f);

        canvas.DrawOval(cx, cy + size * 0.15f, size * 0.35f, size * 0.42f, fill);
        canvas.DrawOval(cx, cy + size * 0.05f, size * 0.08f, size * 0.08f, holePaint);
        canvas.DrawRect(cx - size * 0.04f, cy - size * 0.5f, size * 0.08f, size * 0.55f, fill);
        canvas.DrawRect(cx - size * 0.07f, cy - size * 0.55f, size * 0.14f, size * 0.08f, fill);
    }

    public static void DrawSaxophone(SKCanvas canvas, float cx, float cy, float size, SKColor color)
    {
        using var fill = PaintHelpers.FillPaint(color);
        using var stroke = PaintHelpers.StrokePaint(color, size * 0.11f);
        stroke.StrokeCap = SKStrokeCap.Round;
        stroke.StrokeJoin = SKStrokeJoin.Round;

        using var spine = new SKPath();
        spine.MoveTo(cx + size * 0.18f, cy - size * 0.45f);
        spine.CubicTo(
            cx + size * 0.18f, cy - size * 0.2f,
            cx + size * 0.0f, cy - size * 0.1f,
            cx - size * 0.02f, cy + size * 0.15f);
        spine.CubicTo(
            cx - size * 0.05f, cy + size * 0.35f,
            cx - size * 0.18f, cy + size * 0.4f,
            cx - size * 0.2f, cy + size * 0.35f);
        canvas.DrawPath(spine, stroke);

        using var bell = new SKPath();
        bell.MoveTo(cx - size * 0.32f, cy + size * 0.2f);
        bell.LineTo(cx - size * 0.05f, cy + size * 0.32f);
        bell.LineTo(cx - size * 0.15f, cy + size * 0.45f);
        bell.LineTo(cx - size * 0.42f, cy + size * 0.42f);
        bell.Close();
        canvas.DrawPath(bell, fill);

        using var mouthFill = PaintHelpers.FillPaint(color);
        canvas.DrawRect(cx + size * 0.16f, cy - size * 0.52f, size * 0.05f, size * 0.08f, mouthFill);

        using var keyPaint = PaintHelpers.FillPaint(new SKColor(245, 230, 200));
        canvas.DrawCircle(cx + size * 0.13f, cy - size * 0.2f, size * 0.022f, keyPaint);
        canvas.DrawCircle(cx + size * 0.08f, cy - size * 0.08f, size * 0.022f, keyPaint);
        canvas.DrawCircle(cx + size * 0.02f, cy + size * 0.05f, size * 0.022f, keyPaint);
        canvas.DrawCircle(cx - size * 0.04f, cy + size * 0.18f, size * 0.022f, keyPaint);
    }

    public static void DrawMicrophone(SKCanvas canvas, float cx, float cy, float size, SKColor color)
    {
        using var fill = PaintHelpers.FillPaint(color);
        using var stroke = PaintHelpers.StrokePaint(color, size * 0.05f);

        canvas.DrawRoundRect(cx - size * 0.15f, cy - size * 0.4f, size * 0.3f, size * 0.45f, size * 0.15f, size * 0.15f, fill);
        using var grille = PaintHelpers.StrokePaint(new SKColor(0, 0, 0, 180), size * 0.03f);
        for (var i = 0; i < 4; i++)
            canvas.DrawLine(cx - size * 0.12f, cy - size * 0.3f + i * size * 0.1f, cx + size * 0.12f, cy - size * 0.3f + i * size * 0.1f, grille);

        canvas.DrawLine(cx, cy + size * 0.05f, cx, cy + size * 0.3f, stroke);
        canvas.DrawLine(cx - size * 0.2f, cy + size * 0.35f, cx + size * 0.2f, cy + size * 0.35f, stroke);
    }

    public static void DrawVinyl(SKCanvas canvas, float cx, float cy, float size, SKColor disc, SKColor label)
    {
        using var discPaint = PaintHelpers.FillPaint(disc);
        canvas.DrawCircle(cx, cy, size * 0.5f, discPaint);

        using var groove = PaintHelpers.StrokePaint(new SKColor(255, 255, 255, 40), size * 0.004f);
        for (var r = size * 0.18f; r <= size * 0.45f; r += size * 0.04f)
            canvas.DrawCircle(cx, cy, r, groove);

        using var labelPaint = PaintHelpers.FillPaint(label);
        canvas.DrawCircle(cx, cy, size * 0.14f, labelPaint);
        using var holePaint = PaintHelpers.FillPaint(disc);
        canvas.DrawCircle(cx, cy, size * 0.02f, holePaint);
    }

    public static void DrawCassette(SKCanvas canvas, float cx, float cy, float size, SKColor body, SKColor accent)
    {
        using var bodyPaint = PaintHelpers.FillPaint(body);
        canvas.DrawRoundRect(cx - size * 0.45f, cy - size * 0.3f, size * 0.9f, size * 0.6f, size * 0.04f, size * 0.04f, bodyPaint);

        using var labelPaint = PaintHelpers.FillPaint(accent);
        canvas.DrawRect(cx - size * 0.38f, cy - size * 0.22f, size * 0.76f, size * 0.16f, labelPaint);

        using var reelPaint = PaintHelpers.FillPaint(new SKColor(0, 0, 0, 200));
        canvas.DrawCircle(cx - size * 0.18f, cy + size * 0.08f, size * 0.09f, reelPaint);
        canvas.DrawCircle(cx + size * 0.18f, cy + size * 0.08f, size * 0.09f, reelPaint);
        using var reelCenter = PaintHelpers.FillPaint(body);
        canvas.DrawCircle(cx - size * 0.18f, cy + size * 0.08f, size * 0.03f, reelCenter);
        canvas.DrawCircle(cx + size * 0.18f, cy + size * 0.08f, size * 0.03f, reelCenter);
    }

    public static void DrawHeadphones(SKCanvas canvas, float cx, float cy, float size, SKColor color)
    {
        using var fill = PaintHelpers.FillPaint(color);
        using var bandPaint = PaintHelpers.StrokePaint(color, size * 0.08f);

        using var band = new SKPath();
        band.MoveTo(cx - size * 0.38f, cy);
        band.ArcTo(new SKRect(cx - size * 0.4f, cy - size * 0.4f, cx + size * 0.4f, cy + size * 0.1f), 180, 180, false);
        canvas.DrawPath(band, bandPaint);

        canvas.DrawRoundRect(cx - size * 0.48f, cy - size * 0.05f, size * 0.18f, size * 0.25f, size * 0.04f, size * 0.04f, fill);
        canvas.DrawRoundRect(cx + size * 0.30f, cy - size * 0.05f, size * 0.18f, size * 0.25f, size * 0.04f, size * 0.04f, fill);
    }

    public static void DrawTrebleClef(SKCanvas canvas, float cx, float cy, float size, SKColor color)
    {
        using var stroke = PaintHelpers.StrokePaint(color, size * 0.06f);
        using var path = new SKPath();
        path.MoveTo(cx, cy - size * 0.5f);
        path.CubicTo(cx + size * 0.25f, cy - size * 0.3f, cx + size * 0.1f, cy - size * 0.05f, cx - size * 0.1f, cy - size * 0.05f);
        path.CubicTo(cx - size * 0.3f, cy - size * 0.05f, cx - size * 0.3f, cy + size * 0.25f, cx + size * 0.05f, cy + size * 0.2f);
        path.CubicTo(cx + size * 0.3f, cy + size * 0.15f, cx + size * 0.2f, cy - size * 0.2f, cx, cy - size * 0.3f);
        path.LineTo(cx, cy + size * 0.5f);
        canvas.DrawPath(path, stroke);

        using var fill = PaintHelpers.FillPaint(color);
        canvas.DrawCircle(cx - size * 0.05f, cy + size * 0.5f, size * 0.07f, fill);
    }

    public static void DrawSynthKeyboard(SKCanvas canvas, float cx, float cy, float size, SKColor body, SKColor accent)
    {
        using var bodyPaint = PaintHelpers.FillPaint(body);
        canvas.DrawRoundRect(cx - size * 0.5f, cy - size * 0.2f, size, size * 0.4f, size * 0.04f, size * 0.04f, bodyPaint);

        using var whitePaint = PaintHelpers.FillPaint(new SKColor(245, 240, 225));
        canvas.DrawRect(cx - size * 0.45f, cy, size * 0.9f, size * 0.15f, whitePaint);

        using var blackPaint = PaintHelpers.FillPaint(new SKColor(20, 18, 14));
        var keyWidth = size * 0.9f / 14;
        for (var i = 0; i < 14; i++)
            canvas.DrawRect(cx - size * 0.45f + i * keyWidth, cy, 1.5f, size * 0.15f, blackPaint);
        var pattern = new[] { 0, 1, 3, 4, 5 };
        for (var i = 0; i < 14; i++)
            if (pattern.Contains(i % 7))
                canvas.DrawRect(cx - size * 0.45f + i * keyWidth + keyWidth * 0.65f, cy, keyWidth * 0.5f, size * 0.1f, blackPaint);

        using var knobPaint = PaintHelpers.FillPaint(accent);
        for (var i = 0; i < 4; i++)
            canvas.DrawCircle(cx - size * 0.4f + i * size * 0.12f, cy - size * 0.1f, size * 0.04f, knobPaint);
    }

    public static void DrawTrumpet(SKCanvas canvas, float cx, float cy, float size, SKColor color)
    {
        using var fill = PaintHelpers.FillPaint(color);
        using var stroke = PaintHelpers.StrokePaint(color, size * 0.08f);

        canvas.DrawLine(cx - size * 0.4f, cy, cx + size * 0.15f, cy, stroke);

        using var bell = new SKPath();
        bell.MoveTo(cx + size * 0.15f, cy - size * 0.22f);
        bell.LineTo(cx + size * 0.45f, cy - size * 0.3f);
        bell.LineTo(cx + size * 0.45f, cy + size * 0.3f);
        bell.LineTo(cx + size * 0.15f, cy + size * 0.22f);
        bell.Close();
        canvas.DrawPath(bell, fill);

        for (var i = 0; i < 3; i++)
            canvas.DrawRect(cx - size * 0.25f + i * size * 0.12f, cy - size * 0.12f, size * 0.04f, size * 0.1f, fill);
    }

    public static void DrawTurntable(SKCanvas canvas, float cx, float cy, float size, SKColor body, SKColor disc)
    {
        using var bodyPaint = PaintHelpers.FillPaint(body);
        canvas.DrawRoundRect(cx - size * 0.48f, cy - size * 0.42f, size * 0.96f, size * 0.84f, size * 0.03f, size * 0.03f, bodyPaint);

        using var discPaint = PaintHelpers.FillPaint(disc);
        canvas.DrawCircle(cx - size * 0.05f, cy, size * 0.32f, discPaint);
        using var holePaint = PaintHelpers.FillPaint(body);
        canvas.DrawCircle(cx - size * 0.05f, cy, size * 0.03f, holePaint);

        using var armPaint = PaintHelpers.StrokePaint(new SKColor(220, 220, 220), size * 0.02f);
        canvas.DrawLine(cx + size * 0.38f, cy - size * 0.3f, cx + size * 0.1f, cy + size * 0.05f, armPaint);
        using var armBase = PaintHelpers.FillPaint(new SKColor(220, 220, 220));
        canvas.DrawCircle(cx + size * 0.38f, cy - size * 0.3f, size * 0.04f, armBase);
    }

    public static void DrawDrumKit(SKCanvas canvas, float cx, float cy, float size, SKColor color, SKColor accent)
    {
        using var drum = PaintHelpers.FillPaint(color);
        using var rim = PaintHelpers.FillPaint(accent);

        canvas.DrawCircle(cx, cy + size * 0.18f, size * 0.28f, drum);
        canvas.DrawCircle(cx, cy + size * 0.18f, size * 0.26f, rim);

        canvas.DrawOval(cx - size * 0.3f, cy - size * 0.1f, size * 0.12f, size * 0.08f, drum);
        canvas.DrawOval(cx + size * 0.3f, cy - size * 0.1f, size * 0.12f, size * 0.08f, drum);

        using var cymbal = PaintHelpers.FillPaint(new SKColor(255, 220, 100, 220));
        canvas.DrawOval(cx + size * 0.35f, cy - size * 0.3f, size * 0.16f, size * 0.03f, cymbal);

        using var stick = PaintHelpers.StrokePaint(accent, size * 0.02f);
        canvas.DrawLine(cx + size * 0.35f, cy - size * 0.3f, cx + size * 0.35f, cy + size * 0.2f, stick);
    }

    public static void DrawMicOnStand(SKCanvas canvas, float cx, float cy, float size, SKColor color)
    {
        using var fill = PaintHelpers.FillPaint(color);
        canvas.DrawCircle(cx, cy - size * 0.25f, size * 0.15f, fill);
        using var grille = PaintHelpers.StrokePaint(new SKColor(255, 255, 255, 100), size * 0.015f);
        for (var i = 0; i < 3; i++)
            canvas.DrawCircle(cx, cy - size * 0.25f, size * 0.12f - i * size * 0.03f, grille);

        using var stand = PaintHelpers.StrokePaint(color, size * 0.03f);
        canvas.DrawLine(cx, cy - size * 0.08f, cx, cy + size * 0.35f, stand);
        canvas.DrawLine(cx - size * 0.15f, cy + size * 0.38f, cx + size * 0.15f, cy + size * 0.38f, stand);
    }
}
