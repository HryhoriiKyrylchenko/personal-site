namespace PersonalSite.Application.Features.Pages.Page.Mappers;

public static class PageMapper
{
    public static PageDto MapToDto(Domain.Entities.Pages.Page entity, string languageCode)
    {
        var translation = entity.Translations
            .FirstOrDefault(p => p.Language.Code.Equals(languageCode, 
                StringComparison.OrdinalIgnoreCase));

        return new PageDto
        {
            Id = entity.Id,
            Key = entity.Key,
            Data = translation?.Data ?? new Dictionary<string, string>(),
            Title = translation?.Title ?? string.Empty,
            Description = translation?.Description ?? string.Empty,
            MetaTitle = translation?.MetaTitle ?? string.Empty,
            MetaDescription = translation?.MetaDescription ?? string.Empty,
            OgImage = string.IsNullOrWhiteSpace(translation?.OgImage) ? string.Empty : S3UrlHelper.BuildImageUrl(translation.OgImage)
        };
    }

    public static List<PageDto> MapToDtoList(IEnumerable<Domain.Entities.Pages.Page> entities, string languageCode)
    {
        return entities.Select(e => MapToDto(e, languageCode)).ToList();
    }
    
    public static PageAdminDto MapToAdminDto(Domain.Entities.Pages.Page entity)
    {
        return new PageAdminDto
        {
            Id = entity.Id,
            Key = entity.Key,
            Translations = PageTranslationMapper.MapToDtoList(entity.Translations)
        };
    }

    public static List<PageAdminDto> MapToAdminDtoList(IEnumerable<Domain.Entities.Pages.Page> entities)
    {
        return entities.Select(MapToAdminDto).ToList();   
    }
}