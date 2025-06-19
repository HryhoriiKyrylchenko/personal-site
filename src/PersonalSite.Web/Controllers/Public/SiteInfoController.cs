using PersonalSite.Application.Features.Common.Common.Dtos;
using PersonalSite.Application.Features.Common.SiteInfo.Queries.GetSiteInfo;

namespace PersonalSite.Web.Controllers.Public;

[ApiController]
[Route("api/site-info")]
public class SiteInfoController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public SiteInfoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<SiteInfoDto>> GetSiteInfo(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetSiteInfoQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}