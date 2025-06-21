namespace PersonalSite.Application.Features.Pages.Page.Mappers;

public static class PageTranslationMapper
{
    public static PageTranslationDto MapToDto(PageTranslation entity)
    {
        return new PageTranslationDto
        {
            Id = entity.Id,
            LanguageCode = entity.Language.Code,
            PageId = entity.PageId,
            Data = entity.Data,
            Title = entity.Title,
            Description = entity.Description,
            MetaTitle = entity.MetaTitle,
            MetaDescription = entity.MetaDescription,
            OgImage = string.IsNullOrWhiteSpace(entity.OgImage) ? string.Empty : S3UrlHelper.BuildImageUrl(entity.OgImage)
        };
    }
    
    public static List<PageTranslationDto> MapToDtoList(IEnumerable<PageTranslation> entities)
    {
        return entities.Select(MapToDto).ToList();
    }
}