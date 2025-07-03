using PersonalSite.Application.Features.Common.Language.Commands.CreateLanguage;
using PersonalSite.Application.Features.Common.Language.Commands.DeleteLanguage;
using PersonalSite.Application.Features.Common.Language.Commands.UpdateLanguage;
using PersonalSite.Application.Features.Common.Language.Dtos;
using PersonalSite.Application.Features.Common.Language.Queries.GetLanguageById;
using PersonalSite.Application.Features.Common.Language.Queries.GetLanguages;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Web.Controllers.Admin.Common;

[Route("api/admin/[controller]")]
[ApiController]
[Authorize]
public class LanguageController : ControllerBase
{
    private readonly IMediator _mediator;

    public LanguageController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Result<Guid>>> Create([FromBody] CreateLanguageCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsSuccess)
            return CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value);
        return BadRequest(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateLanguageCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest("Mismatched Language ID.");

        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsSuccess)
            return NoContent();
        return BadRequest(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<string>> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteLanguageCommand(id);
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsSuccess)
            return NoContent();
        return BadRequest(result.Error);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<LanguageDto>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new GetLanguageByIdQuery(id);
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailure)
            return NotFound();
        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<ActionResult<List<LanguageDto>>> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetLanguagesQuery();
        var result = await _mediator.Send(query, cancellationToken);
        if (result.IsFailure || result.Value?.Count == 0)
            return NotFound();
        return Ok(result);
    }
}