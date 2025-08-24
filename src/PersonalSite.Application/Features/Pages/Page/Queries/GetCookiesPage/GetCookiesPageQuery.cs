using PersonalSite.Application.Features.Pages.Page.Dtos;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Pages.Page.Queries.GetCookiesPage;

public record GetCookiesPageQuery : IRequest<Result<CookiesPageDto>>;