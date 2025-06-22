using PersonalSite.Application.Features.Blogs.Blog.Dtos;

namespace PersonalSite.Application.Features.Blogs.Blog.Commands.CreateBlogPost;

public record CreateBlogPostCommand(
    string Slug,
    string CoverImage,
    bool IsPublished,
    List<BlogPostTranslationDto> Translations,
    List<BlogPostTagDto> Tags
) : IRequest<Result<Guid>>;