using PersonalSite.Application.Features.Pages.Page.Dtos;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Pages.Page.Queries.GetAboutPage;

public record GetAboutPageQuery : IRequest<Result<AboutPageDto>>;