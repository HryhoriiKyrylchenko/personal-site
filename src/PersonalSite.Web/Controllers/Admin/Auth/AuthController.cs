using PersonalSite.Application.Features.Auth.Authentication.Commands.ChangePassword;
using PersonalSite.Application.Features.Auth.Authentication.Commands.Login;
using PersonalSite.Application.Features.Auth.Authentication.Commands.Logout;
using PersonalSite.Application.Features.Auth.Authentication.Queries.GetCurrentUser;

namespace PersonalSite.Web.Controllers.Admin.Auth;

[ApiController]
[Route("api/admin/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return Unauthorized(result.Error);

        return Ok(result.Value);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var result = await _mediator.Send(new LogoutCommand());
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok();
    }
    
    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(
        [FromBody] ChangePasswordCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(new { message = result.Error });

        return Ok();
    }
    
    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var result = await _mediator.Send(new GetCurrentUserQuery());

        if (result.IsFailure)
            return Unauthorized(result.Error);

        return Ok(result.Value);
    }
}

