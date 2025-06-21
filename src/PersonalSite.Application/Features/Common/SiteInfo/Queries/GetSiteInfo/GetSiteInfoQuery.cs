namespace PersonalSite.Application.Features.Common.SiteInfo.Queries.GetSiteInfo;

public record GetSiteInfoQuery() : IRequest<Result<SiteInfoDto>>;