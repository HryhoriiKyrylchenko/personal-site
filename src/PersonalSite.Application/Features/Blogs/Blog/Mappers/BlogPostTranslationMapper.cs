namespace PersonalSite.Application.Features.Blogs.Blog.Mappers;

public static class BlogPostTranslationMapper
{
    public static BlogPostTranslationDto MapToDto(BlogPostTranslation entity)
    {
        return new BlogPostTranslationDto
        {
            Id = entity.Id,
            LanguageCode = entity.Language.Code,
            BlogPostId = entity.BlogPostId,
            Title = entity.Title,
            Excerpt = entity.Excerpt,
            Content = entity.Content,
            MetaTitle = entity.MetaTitle,
            MetaDescription = entity.MetaDescription,
            OgImage = string.IsNullOrWhiteSpace(entity.OgImage) ? string.Empty : S3UrlHelper.BuildImageUrl(entity.OgImage)
        };
    }

    public static List<BlogPostTranslationDto> MapToDtoList(
        IEnumerable<BlogPostTranslation> entities)
    {
        return entities.Select(MapToDto).ToList();
    }
}