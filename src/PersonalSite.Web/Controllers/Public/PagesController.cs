using PersonalSite.Application.Features.Analytics.AnalyticsEvent.Commands.TrackAnalyticsEvent;
using PersonalSite.Application.Features.Pages.Page.Dtos;
using PersonalSite.Application.Features.Pages.Page.Queries.GetAboutPage;
using PersonalSite.Application.Features.Pages.Page.Queries.GetBlogPage;
using PersonalSite.Application.Features.Pages.Page.Queries.GetContactPage;
using PersonalSite.Application.Features.Pages.Page.Queries.GetCookiesPage;
using PersonalSite.Application.Features.Pages.Page.Queries.GetHomePage;
using PersonalSite.Application.Features.Pages.Page.Queries.GetPortfolioPage;
using PersonalSite.Application.Features.Pages.Page.Queries.GetPrivacyPage;

namespace PersonalSite.Web.Controllers.Public;

[ApiController]
[Route("api/pages")]
public class PagesController : ControllerBase
{
    private readonly IMediator _mediator;

    public PagesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("home")]
    public async Task<ActionResult<HomePageDto>> GetHomePage(CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetHomePageQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { error = result.Error });
    }

    [HttpGet("about")]
    public async Task<ActionResult<AboutPageDto>> GetAboutPage(CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetAboutPageQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { error = result.Error });
    }

    [HttpGet("portfolio")]
    public async Task<ActionResult<PortfolioPageDto>> GetPortfolioPage(CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetPortfolioPageQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { error = result.Error });
    }

    [HttpGet("blog")]
    public async Task<ActionResult<BlogPageDto>> GetBlogPage(CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetBlogPageQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { error = result.Error });
    }

    [HttpGet("contacts")]
    public async Task<ActionResult<ContactPageDto>> GetContactPage(CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetContactPageQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { error = result.Error });
    }
    
    [HttpGet("cookies")]
    public async Task<ActionResult<CookiesPageDto>> GetCookiesPage(CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetCookiesPageQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { error = result.Error });
    }
    
    [HttpGet("privacy")]
    public async Task<ActionResult<PrivacyPageDto>> GetPrivacyPage(CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetPrivacyPageQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { error = result.Error });
    }
}