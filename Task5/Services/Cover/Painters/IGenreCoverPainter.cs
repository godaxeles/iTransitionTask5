using SkiaSharp;

namespace Task5.Services.Cover.Painters;

public interface IGenreCoverPainter
{
    void PaintScene(SKCanvas canvas, int width, int height, Random random);
}
