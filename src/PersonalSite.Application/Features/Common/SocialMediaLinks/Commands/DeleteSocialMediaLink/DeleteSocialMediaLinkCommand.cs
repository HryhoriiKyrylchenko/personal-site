namespace PersonalSite.Application.Features.Common.SocialMediaLinks.Commands.DeleteSocialMediaLink;

public record DeleteSocialMediaLinkCommand(Guid Id) : IRequest<Result>;