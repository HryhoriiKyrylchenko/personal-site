using PersonalSite.Application.Features.Common.SocialMediaLinks.Commands.CreateSocialMediaLink;
using PersonalSite.Application.Features.Common.SocialMediaLinks.Commands.DeleteSocialMediaLink;
using PersonalSite.Application.Features.Common.SocialMediaLinks.Commands.UpdateSocialMediaLink;
using PersonalSite.Application.Features.Common.SocialMediaLinks.Dtos;
using PersonalSite.Application.Features.Common.SocialMediaLinks.Queries.GetSocialMediaLinkById;
using PersonalSite.Application.Features.Common.SocialMediaLinks.Queries.GetSocialMediaLinks;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Web.Controllers.Admin.Common;

[Route("api/admin/[controller]")]
[ApiController]
//[Authorize]
public class SocialMediaLinkController : ControllerBase
{
    private readonly IMediator _mediator;

    public SocialMediaLinkController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Result<Guid>>> Create([FromBody] CreateSocialMediaLinkCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsSuccess)
            return CreatedAtAction(nameof(GetById), new { id = result.Value }, result);
        return BadRequest(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Result>> Update([FromRoute] Guid id, [FromBody] UpdateSocialMediaLinkCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest("Mismatched SocialMediaLink ID.");

        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsSuccess)
            return NoContent();
        return BadRequest(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Result>> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteSocialMediaLinkCommand(id);
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsSuccess)
            return NoContent();
        return BadRequest(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SocialMediaLinkDto>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetSocialMediaLinkByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);
        if (result.IsFailure)
            return NotFound();
        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResult<SocialMediaLinkDto>>> GetAll([FromQuery] GetSocialMediaLinksQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        if (result.IsFailure)
            return NotFound();
        return Ok(result);
    }
}