using PersonalSite.Application.Features.Pages.Page.Dtos;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Pages.Page.Queries.GetBlogPage;

public record GetBlogPageQuery : IRequest<Result<BlogPageDto>>;