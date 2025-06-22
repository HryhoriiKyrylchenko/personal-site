using PersonalSite.Application.Features.Pages.Page.Dtos;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Pages.Page.Queries.GetHomePage;

public record GetHomePageQuery : IRequest<Result<HomePageDto>>;