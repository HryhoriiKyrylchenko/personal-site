using PersonalSite.Application.Features.Pages.Page.Dtos;

namespace PersonalSite.Application.Features.Pages.Page.Queries.GetPageById;

public record GetPageByIdQuery(Guid Id) : IRequest<Result<PageDto>>;