using PersonalSite.Application.Features.Skills.LearningSkills.Commands.CreateLearningSkill;
using PersonalSite.Application.Features.Skills.LearningSkills.Commands.DeleteLearningSkill;
using PersonalSite.Application.Features.Skills.LearningSkills.Commands.UpdateLearningSkill;
using PersonalSite.Application.Features.Skills.LearningSkills.Dtos;
using PersonalSite.Application.Features.Skills.LearningSkills.Queries.GetLearningSkillById;
using PersonalSite.Application.Features.Skills.LearningSkills.Queries.GetLearningSkills;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Web.Controllers.Admin.Skills;

[Route("api/admin/[controller]")]
[ApiController]
[Authorize]
public class LearningSkillController : ControllerBase
{
    private readonly IMediator _mediator;

    public LearningSkillController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Result<Guid>>> Create(
        [FromBody] CreateLearningSkillCommand command,
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
        [FromBody] UpdateLearningSkillCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest("Mismatched LearningSkill ID.");

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
        var command = new DeleteLearningSkillCommand(id);
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsSuccess)
            return NoContent();

        return BadRequest(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<LearningSkillAdminDto>> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetLearningSkillByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);
        if (result.IsFailure)
            return NotFound();

        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<ActionResult<List<LearningSkillAdminDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetLearningSkillsQuery(), cancellationToken);
        if (result.IsFailure)
            return NotFound();

        return Ok(result.Value);
    }
}