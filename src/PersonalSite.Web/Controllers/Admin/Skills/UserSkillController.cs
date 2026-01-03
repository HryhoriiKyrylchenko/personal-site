using PersonalSite.Application.Features.Skills.UserSkills.Commands.CreateUserSkill;
using PersonalSite.Application.Features.Skills.UserSkills.Commands.DeleteUserSkill;
using PersonalSite.Application.Features.Skills.UserSkills.Commands.UpdateUserSkill;
using PersonalSite.Application.Features.Skills.UserSkills.Dtos;
using PersonalSite.Application.Features.Skills.UserSkills.Queries.GetUserSkillById;
using PersonalSite.Application.Features.Skills.UserSkills.Queries.GetUserSkills;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Web.Controllers.Admin.Skills;

[Route("api/admin/[controller]")]
[ApiController]
//[Authorize]
public class UserSkillController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserSkillController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Result<Guid>>> Create(
        [FromBody] CreateUserSkillCommand command,
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
        [FromBody] UpdateUserSkillCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest("Mismatched UserSkill ID.");

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
        var result = await _mediator.Send(new DeleteUserSkillCommand(id), cancellationToken);
        if (result.IsSuccess)
            return NoContent();

        return BadRequest(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserSkillAdminDto>> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetUserSkillByIdQuery(id), cancellationToken);
        if (result.IsFailure)
            return NotFound();

        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<ActionResult<List<UserSkillAdminDto>>> GetAll(
        [FromQuery] GetUserSkillsQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        if (result.IsFailure)
            return NotFound();

        return Ok(result.Value);
    }
}