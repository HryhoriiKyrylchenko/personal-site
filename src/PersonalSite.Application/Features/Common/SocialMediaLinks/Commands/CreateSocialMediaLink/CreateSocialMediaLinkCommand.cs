using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Common.SocialMediaLinks.Commands.CreateSocialMediaLink;

public record CreateSocialMediaLinkCommand(
    string Platform,
    string Url,
    int DisplayOrder,
    bool IsActive
) : IRequest<Result<Guid>>;