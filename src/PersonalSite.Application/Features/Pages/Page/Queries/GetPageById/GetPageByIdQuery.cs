using PersonalSite.Application.Features.Pages.Page.Dtos;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Pages.Page.Queries.GetPageById;

public record GetPageByIdQuery(Guid Id) : IRequest<Result<PageAdminDto>>;