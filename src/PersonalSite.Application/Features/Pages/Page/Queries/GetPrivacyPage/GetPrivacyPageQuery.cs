using PersonalSite.Application.Features.Pages.Page.Dtos;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Pages.Page.Queries.GetPrivacyPage;

public record GetPrivacyPageQuery : IRequest<Result<PrivacyPageDto>>;