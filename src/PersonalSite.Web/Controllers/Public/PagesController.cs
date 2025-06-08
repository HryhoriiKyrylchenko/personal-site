using PersonalSite.Application.Services.Aggregates.DTOs;

namespace PersonalSite.Web.Controllers.Public;

[ApiController]
[Route("api/pages")]
public class PagesController : ControllerBase
{
    private readonly IPagesDataService _pagesDataService;

    public PagesController(
        IPagesDataService pagesesDataService)
    {
        _pagesDataService = pagesesDataService;

    }

    [HttpGet("home")]
    public async Task<ActionResult<HomePageDto>> GetHomePage(CancellationToken cancellationToken = default)
    {
        var result = await _pagesDataService.GetHomePageAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("about")]
    public async Task<ActionResult<AboutPageDto>> GetAboutPage(CancellationToken cancellationToken = default)
    {
        var result = await _pagesDataService.GetAboutPageAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("portfolio")]
    public async Task<ActionResult<PortfolioPageDto>> GetPortfolioPage(CancellationToken cancellationToken = default)
    {
        var result = await _pagesDataService.GetPortfolioPageAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("blog")]
    public async Task<ActionResult<BlogPageDto>> GetBlogPage(CancellationToken cancellationToken = default)
    {
        var result = await _pagesDataService.GetBlogPageAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("contacts")]
    public async Task<ActionResult<ContactPageDto>> GetContactPage(CancellationToken cancellationToken = default)
    {
        var result = await _pagesDataService.GetContactPageAsync(cancellationToken);
        return Ok(result);
    }
}