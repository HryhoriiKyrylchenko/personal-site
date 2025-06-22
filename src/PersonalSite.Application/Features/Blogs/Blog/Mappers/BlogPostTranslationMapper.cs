namespace PersonalSite.Application.Features.Blogs.Blog.Mappers;

public class BlogPostTranslationMapper : IMapper<BlogPostTranslation, BlogPostTranslationDto>
{
    private readonly IS3UrlBuilder _urlBuilder;

    public BlogPostTranslationMapper(IS3UrlBuilder urlBuilder)
    {
        _urlBuilder = urlBuilder;   
    }
    
    public BlogPostTranslationDto MapToDto(BlogPostTranslation entity)
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
            OgImage = string.IsNullOrWhiteSpace(entity.OgImage) ? string.Empty : _urlBuilder.BuildUrl(entity.OgImage)
        };
    }

    public List<BlogPostTranslationDto> MapToDtoList(IEnumerable<BlogPostTranslation> entities)
    {
        return entities.Select(MapToDto).ToList();
    }
}