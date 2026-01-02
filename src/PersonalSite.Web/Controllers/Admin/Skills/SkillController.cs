using PersonalSite.Application.Features.Skills.Skills.Commands.CreateSkill;
using PersonalSite.Application.Features.Skills.Skills.Commands.DeleteSkill;
using PersonalSite.Application.Features.Skills.Skills.Commands.UpdateSkill;
using PersonalSite.Application.Features.Skills.Skills.Dtos;
using PersonalSite.Application.Features.Skills.Skills.Queries.GetSkillById;
using PersonalSite.Application.Features.Skills.Skills.Queries.GetSkills;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Web.Controllers.Admin.Skills;

[Route("api/admin/[controller]")]
[ApiController]
//[Authorize]
public class SkillController : ControllerBase
{
    private readonly IMediator _mediator;

    public SkillController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Result<Guid>>> Create(
        [FromBody] CreateSkillCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsSuccess)
            return CreatedAtAction(nameof(GetById), new { id = result.Value }, result);

        return BadRequest(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Result>> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateSkillCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest("Mismatched Skill ID.");

        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsSuccess)
            return NoContent();

        return BadRequest(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Result>> Delete(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteSkillCommand(id), cancellationToken);
        if (result.IsSuccess)
            return NoContent();

        return BadRequest(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SkillAdminDto>> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetSkillByIdQuery(id), cancellationToken);
        if (result.IsFailure)
            return NotFound();

        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<ActionResult<List<SkillAdminDto>>> GetAll(
        [FromQuery] GetSkillsQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        if (result.IsFailure)
            return NotFound();

        return Ok(result.Value);
    }
}