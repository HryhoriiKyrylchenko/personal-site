using PersonalSite.Application.Features.Common.SocialMediaLinks.Dtos;
using PersonalSite.Domain.Entities.Common;

namespace PersonalSite.Application.Features.Common.SocialMediaLinks.Mappers;

public class SocialMediaLinkMapper : IMapper<SocialMediaLink, SocialMediaLinkDto>
{
    private readonly IS3UrlBuilder _urlBuilder;

    public SocialMediaLinkMapper(IS3UrlBuilder urlBuilder)
    {
        _urlBuilder = urlBuilder;  
    }
    
    public SocialMediaLinkDto MapToDto(SocialMediaLink entity)
    {
        return new SocialMediaLinkDto
        {
            Id = entity.Id,
            Platform = entity.Platform,
            Url = _urlBuilder.BuildUrl(entity.Url),
            DisplayOrder = entity.DisplayOrder,
            IsActive = entity.IsActive
        };
    }

    public List<SocialMediaLinkDto> MapToDtoList(IEnumerable<SocialMediaLink> entities)
    {
        return entities.Select(MapToDto).ToList();
    }
}