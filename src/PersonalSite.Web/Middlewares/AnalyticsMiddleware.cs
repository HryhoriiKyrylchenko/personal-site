using PersonalSite.Application.Features.Analytics.AnalyticsEvent.Events.FormSubmittedEvent;
using PersonalSite.Application.Features.Analytics.AnalyticsEvent.Events.PageViewedEvent;

namespace PersonalSite.Web.Middlewares;

public class AnalyticsMiddleware
{
    private readonly RequestDelegate _next;

    public AnalyticsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IMediator mediator)
    {
        var path = context.Request.Path.Value ?? string.Empty;

        if (context.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase) &&
            (path.StartsWith("/api/pages/")))
        {
            await mediator.Publish(new PageViewedEvent(
                PageSlug: path + context.Request.QueryString,
                Referrer: context.Request.Headers["Referer"].ToString(),
                UserAgent: context.Request.Headers["User-Agent"].ToString()
            ));
        }

        if (context.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase) &&
            path.StartsWith("/api/contact"))
        {
            await mediator.Publish(new FormSubmittedEvent(
                PageSlug: path,
                Referrer: context.Request.Headers["Referer"].ToString(),
                UserAgent: context.Request.Headers["User-Agent"].ToString(),
                AdditionalDataJson: "{}"
            ));
        }

        await _next(context);
    }
}
