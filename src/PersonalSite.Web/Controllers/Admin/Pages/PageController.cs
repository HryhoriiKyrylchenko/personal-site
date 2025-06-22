using PersonalSite.Application.Features.Pages.Page.Commands.CreatePage;
using PersonalSite.Application.Features.Pages.Page.Commands.DeletePage;
using PersonalSite.Application.Features.Pages.Page.Commands.UpdatePage;
using PersonalSite.Application.Features.Pages.Page.Dtos;
using PersonalSite.Application.Features.Pages.Page.Queries.GetPageById;
using PersonalSite.Application.Features.Pages.Page.Queries.GetPages;

namespace PersonalSite.Web.Controllers.Admin.Pages;

[Route("api/admin/[controller]")]
[ApiController]
[Authorize]
public class PageController : ControllerBase
{
    private readonly IMediator _mediator;

    public PageController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Result<Guid>>> Create([FromBody] CreatePageCommand command, 
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsSuccess)
            return CreatedAtAction(nameof(GetById), new { id = result.Value }, result);
        return BadRequest(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Result>> Update([FromRoute] Guid id, [FromBody] UpdatePageCommand command, 
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest("Mismatched Page ID.");

        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsSuccess)
            return NoContent();
        return BadRequest(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Result>> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeletePageCommand(id);
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsSuccess)
            return NoContent();
        return BadRequest(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PageAdminDto>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetPageByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);
        if (result.IsFailure)
            return NotFound();
        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<ActionResult<List<PageAdminDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetPagesQuery(), cancellationToken);
        if (result.IsFailure)
            return NotFound();
        return Ok(result.Value);
    }
}