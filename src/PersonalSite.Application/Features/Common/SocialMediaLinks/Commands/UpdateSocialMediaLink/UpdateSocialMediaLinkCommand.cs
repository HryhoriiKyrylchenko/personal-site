namespace PersonalSite.Application.Features.Common.SocialMediaLinks.Commands.UpdateSocialMediaLink;

public record UpdateSocialMediaLinkCommand(
    Guid Id,
    string Platform,
    string Url,
    int DisplayOrder,
    bool IsActive
) : IRequest<Result>;