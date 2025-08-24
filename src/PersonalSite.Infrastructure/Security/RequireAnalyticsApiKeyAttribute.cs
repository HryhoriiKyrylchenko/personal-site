namespace PersonalSite.Infrastructure.Security;

[AttributeUsage(AttributeTargets.Method)]
public class RequireAnalyticsApiKeyAttribute : Attribute, IAsyncActionFilter
{
    private const string HeaderName = "X-Api-Key";

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var expectedKey = config["Analytics:ApiKey"];
        var expectedOrigins = config["AllowedOrigins:Public"]?.Split(",");

        if (expectedOrigins == null || expectedOrigins.Length == 0) 
            return;
        
        if (context.HttpContext.Request.Headers.TryGetValue("Origin", out var origin))
        {
            if (expectedOrigins.All(expected => origin.ToString().TrimEnd('/') != expected.TrimEnd('/')))
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
