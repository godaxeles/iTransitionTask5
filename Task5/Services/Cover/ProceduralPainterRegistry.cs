using Task5.Models;
using Task5.Services.Cover.Painters;

namespace Task5.Services.Cover;

public class ProceduralPainterRegistry
{
    private readonly Dictionary<GenreCategory, IGenreCoverPainter> _painters;

    private readonly IGenreCoverPainter _fallback;

    public ProceduralPainterRegistry()
    {
        _fallback = new RockPainter();
        _painters = new Dictionary<GenreCategory, IGenreCoverPainter>
        {
            [GenreCategory.Rock] = new RockPainter(),
            [GenreCategory.Metal] = new MetalPainter(),
            [GenreCategory.Pop] = new PopPainter(),
            [GenreCategory.Jazz] = new JazzPainter(),
            [GenreCategory.Blues] = new BluesPainter(),
            [GenreCategory.HipHop] = new HipHopPainter(),
            [GenreCategory.Electronic] = new ElectronicPainter(),
            [GenreCategory.Classical] = new ClassicalPainter(),
            [GenreCategory.Country] = new CountryPainter(),
            [GenreCategory.Folk] = new FolkPainter(),
            [GenreCategory.Reggae] = new ReggaePainter(),
            [GenreCategory.Punk] = new PunkPainter(),
            [GenreCategory.Ambient] = new AmbientPainter(),
            [GenreCategory.Indie] = new IndiePainter()
        };
    }

    public IGenreCoverPainter Get(GenreCategory category)
        => _painters.GetValueOrDefault(category, _fallback);
}
