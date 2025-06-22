using PersonalSite.Application.Features.Pages.Page.Dtos;

namespace PersonalSite.Application.Features.Pages.Page.Queries.GetBlogPage;

public record GetBlogPageQuery : IRequest<Result<BlogPageDto>>;