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
        var header = context.Request.Headers["X-Locale"].FirstOrDefault()?.ToLower();

        string? lang = header;
        
        if (lang == null)
        {
            var al = context.Request.Headers["Accept-Language"].ToString();
            if (!string.IsNullOrWhiteSpace(al) && al.Length >= 2)
                lang = al.Substring(0, 2).ToLower();
        }

        lang ??= "en";
        
        languageContext.LanguageCode = await languageService.IsSupportedAsync(lang) ? lang : "en";

        await _next(context);
    }
}
