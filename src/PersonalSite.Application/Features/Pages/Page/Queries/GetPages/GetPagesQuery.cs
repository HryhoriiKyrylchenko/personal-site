using PersonalSite.Application.Features.Pages.Page.Dtos;

namespace PersonalSite.Application.Features.Pages.Page.Queries.GetPages;

public record GetPagesQuery() : IRequest<Result<List<PageAdminDto>>>;