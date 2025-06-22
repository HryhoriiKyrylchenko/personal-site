using PersonalSite.Application.Features.Common.SiteInfo.Dtos;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Common.SiteInfo.Queries.GetSiteInfo;

public record GetSiteInfoQuery() : IRequest<Result<SiteInfoDto>>;