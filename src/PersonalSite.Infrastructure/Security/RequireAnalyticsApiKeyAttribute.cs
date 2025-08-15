namespace PersonalSite.Infrastructure.Security;

public class RequireAnalyticsApiKeyAttribute : Attribute, IAsyncActionFilter
{
    private const string HeaderName = "X-Api-Key";

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var expectedKey = config["Analytics:ApiKey"];
        var expectedOrigin = config["Analytics:AllowedOrigin"];
        
        if (context.HttpContext.Request.Headers.TryGetValue("Origin", out var origin))
        {
            if (origin.ToString().TrimEnd('/') != expectedOrigin)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }

        if (!context.HttpContext.Request.Headers.TryGetValue(HeaderName, out var actualKey) ||
            actualKey != expectedKey)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        await next();
    }
}
