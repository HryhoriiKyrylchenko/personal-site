namespace PersonalSite.Application.Features.Blogs.Blog.Mappers;

public static class BlogPostMapper
{
    public static BlogPostDto MapToDto(BlogPost entity, string languageCode)
    {
        var translation = entity.Translations
            .FirstOrDefault(t => t.Language.Code.Equals(languageCode, StringComparison.OrdinalIgnoreCase));

        return new BlogPostDto
        {
            Id = entity.Id,
            Slug = entity.Slug,
            CoverImage = S3UrlHelper.BuildImageUrl(entity.CoverImage),
            IsPublished = entity.IsPublished,
            PublishedAt = entity.PublishedAt,
                
            Title = translation?.Title ?? string.Empty,
            Excerpt = translation?.Excerpt ?? string.Empty,
            Content = translation?.Content ?? string.Empty,
                
            MetaTitle = translation?.MetaTitle ?? string.Empty,
            MetaDescription = translation?.MetaDescription ?? string.Empty,
            OgImage = string.IsNullOrWhiteSpace(translation?.OgImage) ? string.Empty : S3UrlHelper.BuildImageUrl(translation.OgImage),
            
            Tags = BlogPostTagMapper.MapToDtoList(entity.PostTags.Select(pt => pt.BlogPostTag))
        };
    }

    public static List<BlogPostDto> MapToDtoList(IEnumerable<BlogPost> entities, string languageCode)
    {
        return entities.Select(e => MapToDto(e, languageCode)).ToList();
    }

    public static BlogPostAdminDto MapToAdminDto(BlogPost entity)
    {
        return new BlogPostAdminDto
        {
            Id = entity.Id,
            Slug = entity.Slug,
            CoverImage = entity.CoverImage,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            IsDeleted = entity.IsDeleted,
            IsPublished = entity.IsPublished,
            PublishedAt = entity.PublishedAt,
            Translations = BlogPostTranslationMapper.MapToDtoList(entity.Translations),
            Tags = BlogPostTagMapper.MapToDtoList(entity.PostTags.Select(pt => pt.BlogPostTag))
        };
    }

    public static List<BlogPostAdminDto> MapToAdminDtoList(IEnumerable<BlogPost> entities)
    {
        return entities.Select(MapToAdminDto).ToList();
    }
}