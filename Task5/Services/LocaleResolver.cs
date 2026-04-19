namespace Task5.Services;

public static class LocaleResolver
{
    public static string Resolve(string requested, string acceptLanguage, LocaleDataService localeDataService)
    {
        var supported = localeDataService.GetSupportedCodes();
        var defaultCode = localeDataService.GetDefaultCode();

        if (supported.Contains(requested))
            return requested;

        foreach (var part in acceptLanguage.Split(','))
        {
            var tag = part.Split(';')[0].Trim();
            var lang = tag.Length >= 2 ? tag[..2].ToLowerInvariant() : tag.ToLowerInvariant();
            if (supported.Contains(lang))
                return lang;
        }

        return defaultCode;
    }
}
