namespace PersonalSite.Application.Features.Blogs.Blog.Mappers;

public class BlogPostMapper : ITranslatableMapper<BlogPost, BlogPostDto>, IAdminMapper<BlogPost, BlogPostAdminDto>
{
    private readonly IS3UrlBuilder _urlBuilder;
    private readonly BlogPostTagMapper _tagMapper;
    private readonly BlogPostTranslationMapper _translationMapper;

    public BlogPostMapper(
        IS3UrlBuilder urlBuilder,
        BlogPostTagMapper tagMapper,
        BlogPostTranslationMapper translationMapper)
    {
        _urlBuilder = urlBuilder;   
        _tagMapper = tagMapper;  
        _translationMapper = translationMapper; 
    }
    
    public BlogPostDto MapToDto(BlogPost entity, string languageCode)
    {
        var translation = entity.Translations
            .FirstOrDefault(t => t.Language.Code.Equals(languageCode, StringComparison.OrdinalIgnoreCase));

        return new BlogPostDto
        {
            Id = entity.Id,
            Slug = entity.Slug,
            CoverImage = _urlBuilder.BuildUrl(entity.CoverImage),
            IsPublished = entity.IsPublished,
            PublishedAt = entity.PublishedAt,
                
            Title = translation?.Title ?? string.Empty,
            Excerpt = translation?.Excerpt ?? string.Empty,
            Content = translation?.Content ?? string.Empty,
                
            MetaTitle = translation?.MetaTitle ?? string.Empty,
            MetaDescription = translation?.MetaDescription ?? string.Empty,
            OgImage = string.IsNullOrWhiteSpace(translation?.OgImage) ? string.Empty 
                : _urlBuilder.BuildUrl(translation.OgImage),
            
            Tags = _tagMapper.MapToDtoList(entity.PostTags.Select(pt => pt.BlogPostTag))
        };
    }

    public List<BlogPostDto> MapToDtoList(IEnumerable<BlogPost> entities, string languageCode)
    {
        return entities.Select(e => MapToDto(e, languageCode)).ToList();
    }

    public BlogPostAdminDto MapToAdminDto(BlogPost entity)
    {
        return new BlogPostAdminDto
        {
            Id = entity.Id,
            Slug = entity.Slug,
            CoverImage = _urlBuilder.BuildUrl(entity.CoverImage),
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            IsDeleted = entity.IsDeleted,
            IsPublished = entity.IsPublished,
            PublishedAt = entity.PublishedAt,
            Translations = _translationMapper.MapToDtoList(entity.Translations),
            Tags = _tagMapper.MapToDtoList(entity.PostTags.Select(pt => pt.BlogPostTag))
        };
    }

    public List<BlogPostAdminDto> MapToAdminDtoList(IEnumerable<BlogPost> entities)
    {
        return entities.Select(MapToAdminDto).ToList();
    }
}