using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Common.SocialMediaLinks.Commands.DeleteSocialMediaLink;

public record DeleteSocialMediaLinkCommand(Guid Id) : IRequest<Result>;