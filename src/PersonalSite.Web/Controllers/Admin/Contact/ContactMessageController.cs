using PersonalSite.Application.Features.Contact.ContactMessages.Commands.DeleteContactMessages;
using PersonalSite.Application.Features.Contact.ContactMessages.Commands.SendContactMessage;
using PersonalSite.Application.Features.Contact.ContactMessages.Commands.UpdateContactMessagesReadStatus;
using PersonalSite.Application.Features.Contact.ContactMessages.Dtos;
using PersonalSite.Application.Features.Contact.ContactMessages.Queries.GetContactMessageById;
using PersonalSite.Application.Features.Contact.ContactMessages.Queries.GetContactMessages;

namespace PersonalSite.Web.Controllers.Admin.Contact;

[Route("api/admin/[controller]")]
[ApiController]
[Authorize]
public class ContactMessageController : ControllerBase
{
    private readonly IMediator _mediator;

    public ContactMessageController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Result<Guid>>> Send([FromBody] SendContactMessageCommand command, 
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsSuccess)
            return Ok("Message sent successfully.");
        return BadRequest(result);
    }

    [HttpPut("read-status")]
    public async Task<ActionResult<Result>> UpdateReadStatus([FromBody] UpdateContactMessagesReadStatusCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsSuccess)
            return NoContent();
        return BadRequest(result);
    }

    [HttpDelete]
    public async Task<ActionResult<Result>> Delete([FromBody] DeleteContactMessagesCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsSuccess)
            return NoContent();
        return BadRequest(result);
    }


    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ContactMessageDto>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetContactMessageByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);
        if (result.IsFailure)
            return NotFound();
        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResult<ContactMessageDto>>> GetAll([FromQuery] GetContactMessagesQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        if (result.IsFailure)
            return NotFound();
        return Ok(result);
    }
}