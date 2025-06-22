using PersonalSite.Application.Features.Pages.Page.Dtos;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Pages.Page.Queries.GetPortfolioPage;

public record GetPortfolioPageQuery : IRequest<Result<PortfolioPageDto>>;