using PersonalSite.Application.Features.Pages.Page.Dtos;
using PersonalSite.Domain.Entities.Translations;

namespace PersonalSite.Application.Features.Pages.Page.Mappers;

public class PageTranslationMapper : IMapper<PageTranslation, PageTranslationDto>
{
    private readonly IS3UrlBuilder _urlBuilder;

    public PageTranslationMapper(IS3UrlBuilder urlBuilder)
    {
        _urlBuilder = urlBuilder;  
    }
    
    public PageTranslationDto MapToDto(PageTranslation entity)
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
            OgImage = string.IsNullOrWhiteSpace(entity.OgImage) ? string.Empty : _urlBuilder.BuildUrl(entity.OgImage)
        };
    }
    
    public List<PageTranslationDto> MapToDtoList(IEnumerable<PageTranslation> entities)
    {
        return entities.Select(MapToDto).ToList();
    }
}