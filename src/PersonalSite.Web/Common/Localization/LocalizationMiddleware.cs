namespace PersonalSite.Web.Common.Localization;

public class LocalizationMiddleware
{
    private readonly RequestDelegate _next;

    public LocalizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, LanguageContext languageContext)
    {
        var lang = context.Request.Headers["Accept-Language"].FirstOrDefault()
                   ?? "en";

        var formattedLang = lang[..2].ToLower();

        languageContext.LanguageCode = SupportedLanguages.IsSupported(formattedLang) ? formattedLang : "en";

        await _next(context);
    }
}
