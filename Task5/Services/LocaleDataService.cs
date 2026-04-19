using System.Collections.Concurrent;
using System.Text.Json;
using Task5.Models;

namespace Task5.Services;

public class LocaleDataService
{
    private static readonly JsonSerializerOptions DeserializationOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly IWebHostEnvironment _environment;

    private readonly ConcurrentDictionary<string, LocaleData> _cache = new();

    private readonly Lazy<List<LocaleInfo>> _availableLocales;

    private readonly Lazy<HashSet<string>> _supportedCodes;

    public LocaleDataService(IWebHostEnvironment environment)
    {
        _environment = environment;
        _availableLocales = new Lazy<List<LocaleInfo>>(
            ScanLocalesDirectory, LazyThreadSafetyMode.ExecutionAndPublication);
        _supportedCodes = new Lazy<HashSet<string>>(
            () => _availableLocales.Value.Select(l => l.Code).ToHashSet(),
            LazyThreadSafetyMode.ExecutionAndPublication);
    }

    public List<LocaleInfo> GetAvailableLocales()
    {
        return _availableLocales.Value;
    }

    public HashSet<string> GetSupportedCodes()
    {
        return _supportedCodes.Value;
    }

    public LocaleData GetLocaleData(string locale)
    {
        var safe = _supportedCodes.Value.Contains(locale) ? locale : GetDefaultCode();
        return _cache.GetOrAdd(safe, LoadLocaleData);
    }

    public string GetDefaultCode()
    {
        var locales = _availableLocales.Value;
        return locales.Count > 0 ? locales[0].Code : "en";
    }

    private List<LocaleInfo> ScanLocalesDirectory()
    {
        var dir = Path.Combine(_environment.ContentRootPath, "Locales");
        if (!Directory.Exists(dir))
            return [new LocaleInfo("en", "English")];

        return Directory.GetFiles(dir, "*.json")
            .Select(BuildLocaleInfo)
            .OrderBy(l => l.Code)
            .ToList();
    }

    private LocaleInfo BuildLocaleInfo(string path)
    {
        var code = Path.GetFileNameWithoutExtension(path);
        var data = LoadLocaleData(code);
        var name = string.IsNullOrEmpty(data.DisplayName) ? code : data.DisplayName;
        return new LocaleInfo(code, name);
    }

    private LocaleData LoadLocaleData(string locale)
    {
        var path = Path.Combine(_environment.ContentRootPath, "Locales", $"{locale}.json");
        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<LocaleData>(json, DeserializationOptions) ?? new LocaleData();
    }
}
