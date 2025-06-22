using PersonalSite.Application.Features.Pages.Page.Dtos;

namespace PersonalSite.Application.Features.Pages.Page.Queries.GetAboutPage;

public record GetAboutPageQuery : IRequest<Result<AboutPageDto>>;