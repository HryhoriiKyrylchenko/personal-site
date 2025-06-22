using PersonalSite.Application.Features.Projects.Project.Commands.CreateProject;
using PersonalSite.Application.Features.Projects.Project.Commands.DeleteProject;
using PersonalSite.Application.Features.Projects.Project.Commands.UpdateProject;
using PersonalSite.Application.Features.Projects.Project.Dtos;
using PersonalSite.Application.Features.Projects.Project.Queries.GetProjectById;
using PersonalSite.Application.Features.Projects.Project.Queries.GetProjects;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Web.Controllers.Admin.Projects;

[Route("api/admin/[controller]")]
[ApiController]
[Authorize]
public class ProjectController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Result<Guid>>> Create(
        [FromBody] CreateProjectCommand command,
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
        [FromBody] UpdateProjectCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest("Mismatched Project ID.");

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
        var command = new DeleteProjectCommand(id);
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsSuccess)
            return NoContent();

        return BadRequest(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProjectAdminDto>> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetProjectByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);
        if (result.IsFailure)
            return NotFound();

        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProjectAdminDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetProjectsQuery(), cancellationToken);
        if (result.IsFailure)
            return NotFound();

        return Ok(result.Value);
    }
}