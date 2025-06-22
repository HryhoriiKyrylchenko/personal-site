using PersonalSite.Application.Features.Pages.Page.Dtos;

namespace PersonalSite.Application.Features.Pages.Page.Queries.GetPortfolioPage;

public record GetPortfolioPageQuery : IRequest<Result<PortfolioPageDto>>;