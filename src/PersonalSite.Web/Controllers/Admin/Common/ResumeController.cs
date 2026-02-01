using PersonalSite.Application.Features.Common.Resume.Commands.CreateResume;
using PersonalSite.Application.Features.Common.Resume.Commands.DeleteResume;
using PersonalSite.Application.Features.Common.Resume.Commands.UpdateResume;
using PersonalSite.Application.Features.Common.Resume.Dtos;
using PersonalSite.Application.Features.Common.Resume.Queries.GetResumeById;
using PersonalSite.Application.Features.Common.Resume.Queries.GetResumes;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Web.Controllers.Admin.Common;

[Route("api/admin/[controller]")]
[ApiController]
[Authorize(Roles = "Admin", Policy = "PasswordChanged")]
public class ResumeController : ControllerBase
{
    private readonly IMediator _mediator;

    public ResumeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Result<Guid>>> Create([FromBody] CreateResumeCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsSuccess)
            return CreatedAtAction(nameof(GetById), new { id = result.Value }, result);
        return BadRequest(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Result>> Update([FromRoute] Guid id, [FromBody] UpdateResumeCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest("Mismatched Resume ID.");

        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsSuccess)
            return NoContent();
        return BadRequest(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Result>> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteResumeCommand(id);
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsSuccess)
            return NoContent();
        return BadRequest(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ResumeDto>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetResumeByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);
        if (result.IsFailure)
            return NotFound();
        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResult<ResumeDto>>> GetAll([FromQuery] GetResumesQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        if (result.IsFailure)
            return NotFound();
        return Ok(result);
    }
}
