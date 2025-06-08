namespace PersonalSite.Web.Controllers.Public;

[ApiController]
[Route("api/contact")]
public class ContactController : ControllerBase
{
    private readonly IContactMessageService _contactMessageService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ContactController(IContactMessageService contactMessageService, IHttpContextAccessor httpContextAccessor)
    {
        _contactMessageService = contactMessageService;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] ContactMessageAddRequest request, CancellationToken cancellationToken = default)
    {
        var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var userAgent = _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString() ?? "unknown";

        request.IpAddress = ipAddress;
        request.UserAgent = userAgent;
        
        await _contactMessageService.AddAsync(request, cancellationToken);

        return Ok(new { Message = "Your message has been sent successfully." });
    }
}