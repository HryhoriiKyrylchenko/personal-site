using PersonalSite.Application.Features.Analytics.AnalyticsEvent.Commands.DeleteAnalyticsEvent;
using PersonalSite.Application.Features.Analytics.AnalyticsEvent.Commands.DeleteAnalyticsEventsRange;
using PersonalSite.Application.Features.Analytics.AnalyticsEvent.Commands.TrackAnalyticsEvent;
using PersonalSite.Application.Features.Analytics.AnalyticsEvent.Dtos;
using PersonalSite.Application.Features.Analytics.AnalyticsEvent.Queries.GetAnalyticsEvents;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Web.Controllers.Admin.Analytics;

[Route("api/admin/[controller]")]
[ApiController]
//[Authorize]
public class AnalyticsEventController : ControllerBase
{
    private readonly IMediator _mediator;

    public AnalyticsEventController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResult<AnalyticsEventDto>>> GetAll(
        [FromQuery] GetAnalyticsEventsQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost("track")]
    public async Task<ActionResult<string>> Track([FromBody] TrackAnalyticsEventCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess 
            ? Ok("Analytics data saved successfully.") 
            : BadRequest(result.Error);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id,[FromBody] DeleteAnalyticsEventCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest("Mismatched AnalyticsEvent ID.");
        
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess 
            ? NoContent() 
            : BadRequest(result.Error);
    }

    [HttpPost("range")]
    public async Task<IActionResult> DeleteRange([FromBody] DeleteAnalyticsEventsRangeCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess 
            ? NoContent() 
            : BadRequest(result.Error);
    }
}