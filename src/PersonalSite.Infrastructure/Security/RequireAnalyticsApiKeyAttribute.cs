namespace PersonalSite.Infrastructure.Security;

public class RequireAnalyticsApiKeyAttribute : Attribute, IAsyncActionFilter
{
    private const string HeaderName = "x-api-key";

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var expectedKey = config["Analytics:ApiKey"];

        if (!context.HttpContext.Request.Headers.TryGetValue(HeaderName, out var actualKey) ||
            actualKey != expectedKey)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        await next();
    }
}
