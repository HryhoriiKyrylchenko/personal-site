using PersonalSite.Application.Features.Common.SocialMediaLinks.Dtos;

namespace PersonalSite.Application.Features.Common.SocialMediaLinks.Queries.GetSocialMediaLinkById;

public record GetSocialMediaLinkByIdQuery(Guid Id) : IRequest<Result<SocialMediaLinkDto>>;