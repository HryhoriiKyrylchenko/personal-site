using PersonalSite.Application.Features.Common.Logs.Commands.DeleteOldLogs;
using PersonalSite.Application.Features.Common.Logs.Queries.GetRecentLogs;

namespace PersonalSite.Web.Controllers.Admin.Common;

[Route("api/admin/logs")]
[ApiController]
//[Authorize]
public class LogsController : ControllerBase
{
    private readonly IMediator _mediator;
    public LogsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int count = 50)
    {
        var logs = await _mediator.Send(new GetRecentLogsQuery(count));
        return Ok(logs);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] DateTime cutoff)
    {
        var deleted = await _mediator.Send(new DeleteOldLogsCommand(cutoff));
        return Ok(new { Deleted = deleted });
    }
}
