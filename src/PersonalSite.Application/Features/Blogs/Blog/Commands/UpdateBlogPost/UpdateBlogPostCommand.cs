using PersonalSite.Application.Features.Blogs.Blog.Dtos;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Blogs.Blog.Commands.UpdateBlogPost;

public record UpdateBlogPostCommand(
    Guid Id,
    string Slug,
    string CoverImage,
    bool IsDeleted,
    List<BlogPostTranslationDto> Translations,
    List<BlogPostTagDto> Tags
) : IRequest<Result>;