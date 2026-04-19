using System.Collections.Concurrent;
using System.Text.Json;
using Task5.Models;

namespace Task5.Services;

public class LocaleDataService(IWebHostEnvironment environment)
{
    private static readonly JsonSerializerOptions DeserializationOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly ConcurrentDictionary<string, LocaleData> _cache = new();

    private List<LocaleInfo>? _availableLocales;

    public List<LocaleInfo> GetAvailableLocales()
    {
        return _availableLocales ??= ScanLocalesDirectory();
    }

    public HashSet<string> GetSupportedCodes()
    {
        return GetAvailableLocales().Select(l => l.Code).ToHashSet();
    }

    public LocaleData GetLocaleData(string locale)
    {
        var safe = GetSupportedCodes().Contains(locale) ? locale : GetDefaultCode();
        return _cache.GetOrAdd(safe, LoadLocaleData);
    }

    public string GetDefaultCode()
    {
        var locales = GetAvailableLocales();
        return locales.Count > 0 ? locales[0].Code : "en";
    }

    private List<LocaleInfo> ScanLocalesDirectory()
    {
        var dir = Path.Combine(environment.ContentRootPath, "Locales");
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
        var path = Path.Combine(environment.ContentRootPath, "Locales", $"{locale}.json");
        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<LocaleData>(json, DeserializationOptions) ?? new LocaleData();
    }
}
