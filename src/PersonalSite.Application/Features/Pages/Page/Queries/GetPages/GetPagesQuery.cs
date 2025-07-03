using PersonalSite.Application.Features.Pages.Page.Dtos;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Pages.Page.Queries.GetPages;

public record GetPagesQuery() : IRequest<Result<List<PageAdminDto>>>;