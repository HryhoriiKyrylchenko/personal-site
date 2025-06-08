using PersonalSite.Application.Services.Aggregates.DTOs;

namespace PersonalSite.Web.Controllers.Public;

[ApiController]
[Route("api/site-info")]
public class SiteInfoController : ControllerBase
{
    private readonly ISiteInfoService _siteInfoService;
    
    public SiteInfoController(ISiteInfoService siteInfoService)
    {
        _siteInfoService = siteInfoService;
    }

    [HttpGet]
    public async Task<ActionResult<SiteInfoDto>> GetSiteInfo(CancellationToken cancellationToken)
    {
        var siteInfo = await _siteInfoService.GetAsync(cancellationToken);
        
        return Ok(siteInfo);
    }
}