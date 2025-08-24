using PersonalSite.Application.Features.Pages.Page.Dtos;
using PersonalSite.Domain.Entities.Translations;

namespace PersonalSite.Application.Features.Pages.Page.Mappers;

public class PageMapper 
    : ITranslatableMapper<Domain.Entities.Pages.Page, PageDto>, 
    IAdminMapper<Domain.Entities.Pages.Page, PageAdminDto>
{
    private readonly IS3UrlBuilder _urlBuilder;
    private readonly IMapper<PageTranslation, PageTranslationDto> _translationMapper;
    
    public PageMapper(
        IS3UrlBuilder urlBuilder,
        IMapper<PageTranslation, PageTranslationDto> translationMapper)
    {
        _urlBuilder = urlBuilder;   
        _translationMapper = translationMapper;
    }
    
    public PageDto MapToDto(Domain.Entities.Pages.Page entity, string languageCode)
    {
        var translation = entity.Translations
            .FirstOrDefault(p => p.Language.Code.Equals(languageCode, 
                StringComparison.OrdinalIgnoreCase));

        return new PageDto
        {
            Id = entity.Id,
            Key = entity.Key,
            LanguageCode = translation?.Language.Code ?? string.Empty,
            Data = translation?.Data ?? new Dictionary<string, string>(),
            Title = translation?.Title ?? string.Empty,
            Description = translation?.Description ?? string.Empty,
            MetaTitle = translation?.MetaTitle ?? string.Empty,
            MetaDescription = translation?.MetaDescription ?? string.Empty,
            OgImage = string.IsNullOrWhiteSpace(translation?.OgImage) 
                ? string.Empty 
                : _urlBuilder.BuildUrl(translation.OgImage)
        };
    }

    public List<PageDto> MapToDtoList(IEnumerable<Domain.Entities.Pages.Page> entities, string languageCode)
    {
        return entities.Select(e => MapToDto(e, languageCode)).ToList();
    }
    
    public PageAdminDto MapToAdminDto(Domain.Entities.Pages.Page entity)
    {
        return new PageAdminDto
        {
            Id = entity.Id,
            Key = entity.Key,
            Translations = _translationMapper.MapToDtoList(entity.Translations)
        };
    }

    public List<PageAdminDto> MapToAdminDtoList(IEnumerable<Domain.Entities.Pages.Page> entities)
    {
        return entities.Select(MapToAdminDto).ToList();   
    }
}