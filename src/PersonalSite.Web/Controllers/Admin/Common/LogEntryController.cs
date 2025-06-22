using PersonalSite.Application.Features.Common.LogEntries.Commands.DeleteLogs;
using PersonalSite.Application.Features.Common.LogEntries.Dtos;
using PersonalSite.Application.Features.Common.LogEntries.Queries.GetLogEntries;

namespace PersonalSite.Web.Controllers.Admin.Common;

[Route("api/admin/[controller]")]
[ApiController]
[Authorize]
public class LogEntryController : ControllerBase
{
    private readonly IMediator _mediator;

    public LogEntryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpDelete]
    public async Task<ActionResult<Result>> Delete([FromBody] DeleteLogsCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsSuccess)
            return NoContent();
        return BadRequest(result);
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResult<LogEntryDto>>> GetAll([FromQuery] GetLogEntriesQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        if (result.IsFailure)
            return NotFound();
        return Ok(result);
    }
}
