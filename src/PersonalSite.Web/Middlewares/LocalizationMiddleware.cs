namespace PersonalSite.Web.Middlewares;

public class LocalizationMiddleware
{
    private readonly RequestDelegate _next;

    public LocalizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, LanguageContext languageContext, ILanguageService languageService)
    {
        var langHeader = context.Request.Headers["Accept-Language"].FirstOrDefault() ?? "en";

        var formattedLang = langHeader.Length >= 2
            ? langHeader[..2].ToLower()
            : langHeader.ToLower();

        languageContext.LanguageCode = await languageService.IsSupportedAsync(formattedLang) ? formattedLang : "en";

        await _next(context);
    }
}
