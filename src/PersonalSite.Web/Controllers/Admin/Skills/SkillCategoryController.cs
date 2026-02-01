using PersonalSite.Application.Features.Skills.SkillCategories.Commands.CreateSkillCategory;
using PersonalSite.Application.Features.Skills.SkillCategories.Commands.DeleteSkillCategory;
using PersonalSite.Application.Features.Skills.SkillCategories.Commands.UpdateSkillCategory;
using PersonalSite.Application.Features.Skills.SkillCategories.Dtos;
using PersonalSite.Application.Features.Skills.SkillCategories.Queries.GetSkillCategories;
using PersonalSite.Application.Features.Skills.SkillCategories.Queries.GetSkillCategoryById;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Web.Controllers.Admin.Skills;

[Route("api/admin/[controller]")]
[ApiController]
[Authorize(Roles = "Admin", Policy = "PasswordChanged")]
public class SkillCategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public SkillCategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Result<Guid>>> Create(
        [FromBody] CreateSkillCategoryCommand command,
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
        [FromBody] UpdateSkillCategoryCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest("Mismatched SkillCategory ID.");

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
        var command = new DeleteSkillCategoryCommand(id);
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsSuccess)
            return NoContent();

        return BadRequest(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SkillCategoryAdminDto>> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetSkillCategoryByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);
        if (result.IsFailure)
            return NotFound();

        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<ActionResult<List<SkillCategoryAdminDto>>> GetAll(
        [FromQuery] GetSkillCategoriesQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        if (result.IsFailure)
            return NotFound();

        return Ok(result.Value);
    }
}