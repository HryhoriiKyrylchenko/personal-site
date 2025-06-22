using PersonalSite.Application.Features.Contact.ContactMessages.Commands.SendContactMessage;

namespace PersonalSite.Web.Controllers.Public;

[ApiController]
[Route("api/contact")]
public class ContactController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ContactController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
    {
        _mediator = mediator;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] SendContactMessageCommand command, CancellationToken cancellationToken = default)
    {
        command.IpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        command.UserAgent = _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString() ?? "unknown";

        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok(new { Message = "Your message has been sent successfully." })
            : BadRequest(result.Error);
    }
}