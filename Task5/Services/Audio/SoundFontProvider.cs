using MeltySynth;

namespace Task5.Services.Audio;

public class SoundFontProvider
{
    private readonly string _path;
    private readonly ILogger<SoundFontProvider> _logger;
    private readonly Lazy<SoundFont?> _soundFont;

    public SoundFontProvider(IWebHostEnvironment environment, ILogger<SoundFontProvider> logger)
    {
        _path = Path.Combine(environment.ContentRootPath, "Content", "soundfont.sf2");
        _logger = logger;
        _soundFont = new Lazy<SoundFont?>(Load, LazyThreadSafetyMode.ExecutionAndPublication);
    }

    public SoundFont? Get() => _soundFont.Value;

    public bool IsAvailable => _soundFont.Value is not null;

    private SoundFont? Load()
    {
        if (!File.Exists(_path))
        {
            _logger.LogWarning("SoundFont not found at {Path}. Falling back to procedural synthesis.", _path);
            return null;
        }

        try
        {
            using var stream = File.OpenRead(_path);
            var sf = new SoundFont(stream);
            _logger.LogInformation("SoundFont loaded from {Path}", _path);
            return sf;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load SoundFont from {Path}", _path);
            return null;
        }
    }
}
