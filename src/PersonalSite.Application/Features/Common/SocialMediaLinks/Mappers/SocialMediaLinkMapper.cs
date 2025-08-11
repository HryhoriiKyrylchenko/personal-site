using PersonalSite.Application.Features.Common.SocialMediaLinks.Dtos;
using PersonalSite.Domain.Entities.Common;

namespace PersonalSite.Application.Features.Common.SocialMediaLinks.Mappers;

public class SocialMediaLinkMapper : IMapper<SocialMediaLink, SocialMediaLinkDto>
{
    public SocialMediaLinkDto MapToDto(SocialMediaLink entity)
    {
        return new SocialMediaLinkDto
        {
            Id = entity.Id,
            Platform = entity.Platform,
            Url = entity.Url,
            DisplayOrder = entity.DisplayOrder,
            IsActive = entity.IsActive
        };
    }

    public List<SocialMediaLinkDto> MapToDtoList(IEnumerable<SocialMediaLink> entities)
    {
        return entities.Select(MapToDto).ToList();
    }
}