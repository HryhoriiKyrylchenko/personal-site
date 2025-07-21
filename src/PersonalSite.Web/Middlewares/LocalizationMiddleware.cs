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
        var cookie = context.Request.Cookies["translocoLang"];
        
        var lang = header ?? ( !string.IsNullOrWhiteSpace(cookie) ? cookie.Substring(0,2).ToLower() : null );
        if (lang == null) {
            var al = context.Request.Headers["Accept-Language"].ToString();
            lang = al.Substring(0,2).ToLower();
        }
        
        languageContext.LanguageCode = await languageService.IsSupportedAsync(lang)
            ? lang
            : "en";

        await _next(context);
    }
}
