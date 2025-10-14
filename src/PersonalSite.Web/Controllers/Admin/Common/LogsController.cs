using PersonalSite.Application.Features.Common.Logs.Commands.DeleteOldLogs;
using PersonalSite.Application.Features.Common.Logs.Dtos;
using PersonalSite.Application.Features.Common.Logs.Queries.GetLogsPaginated;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Web.Controllers.Admin.Common;

[Route("api/admin/logs")]
[ApiController]
//[Authorize]
public class LogsController : ControllerBase
{
    private readonly IMediator _mediator;
    public LogsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<PaginatedResult<LogEntryDto>>> GetAllPaginated(
        [FromQuery] GetLogsPaginatedQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("older-than")]
    public async Task<IActionResult> DeleteOlderThan(
        [FromQuery] DateTime cutoffDate,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteOldLogsCommand(cutoffDate), cancellationToken);
        return NoContent();
    }
}
