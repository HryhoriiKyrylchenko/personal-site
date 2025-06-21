namespace PersonalSite.Application.Features.Common.SocialMediaLinks.Mappers;

public static class SocialMediaLinkMapper
{
    public static SocialMediaLinkDto MapToDto(SocialMediaLink entity)
    {
        return new SocialMediaLinkDto
        {
            Id = entity.Id,
            Platform = entity.Platform,
            Url = S3UrlHelper.BuildImageUrl(entity.Url),
            DisplayOrder = entity.DisplayOrder,
            IsActive = entity.IsActive
        };
    }

    public static List<SocialMediaLinkDto> MapToDtoList(IEnumerable<SocialMediaLink> entities)
    {
        return entities.Select(MapToDto).ToList();
    }
}