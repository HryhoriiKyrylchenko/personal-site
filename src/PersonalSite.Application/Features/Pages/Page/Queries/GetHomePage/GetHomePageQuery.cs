using PersonalSite.Application.Features.Pages.Page.Dtos;

namespace PersonalSite.Application.Features.Pages.Page.Queries.GetHomePage;

public record GetHomePageQuery : IRequest<Result<HomePageDto>>;