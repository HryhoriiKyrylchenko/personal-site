using PersonalSite.Application.Features.Common.SocialMediaLinks.Dtos;

namespace PersonalSite.Application.Features.Common.SocialMediaLinks.Queries.GetSocialMediaLinks;

public record GetSocialMediaLinksQuery(
    string? Platform = null,
    bool? IsActive = null
) : IRequest<List<SocialMediaLinkDto>>;