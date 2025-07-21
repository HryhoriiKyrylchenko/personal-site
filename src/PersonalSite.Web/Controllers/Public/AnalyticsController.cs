using PersonalSite.Application.Features.Analytics.AnalyticsEvent.Commands.TrackAnalyticsEvent;

namespace PersonalSite.Web.Controllers.Public;

[ApiController]
[Route("api/analytics")]
public class AnalyticsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AnalyticsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [RequireAnalyticsApiKey]
    [HttpPost("track")]
    public async Task<IActionResult> Track([FromBody] TrackAnalyticsEventRequest request)
    {
        var command = new TrackAnalyticsEventCommand(
            request.EventType,
            request.PageSlug,
            Request.Headers["Referer"].ToString(),
            Request.Headers["User-Agent"].ToString(),
            request.AdditionalDataJson
        );

        var result = await _mediator.Send(command);
        return result.IsSuccess
            ? Ok(new { Message = "Analytics data saved successfully." })
            : BadRequest(new { error = result.Error });
    }
}
