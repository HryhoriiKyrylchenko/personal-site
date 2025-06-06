namespace PersonalSite.Application.Common.Localization;

public static class SupportedLanguages
{
    public static readonly IReadOnlyList<string> All = new[] { "en", "pl", "ru", "ua" };

    public static bool IsSupported(string code) =>
        All.Contains(code, StringComparer.OrdinalIgnoreCase);
}