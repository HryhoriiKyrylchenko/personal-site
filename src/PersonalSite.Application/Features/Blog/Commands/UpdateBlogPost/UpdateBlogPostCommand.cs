namespace PersonalSite.Application.Features.Blog.Commands.UpdateBlogPost;

public record UpdateBlogPostCommand(
    Guid Id,
    string Slug,
    string CoverImage,
    bool IsDeleted,
    List<BlogPostTranslationDto> Translations,
    List<BlogPostTagDto> Tags
) : IRequest<Result>;