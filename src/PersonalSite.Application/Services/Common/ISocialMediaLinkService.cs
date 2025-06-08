namespace PersonalSite.Application.Services.Common;

public interface ISocialMediaLinkService : ICrudService<SocialMediaLinkDto, SocialMediaLinkAddRequest, SocialMediaLinkUpdateRequest>
{
    Task<IReadOnlyList<SocialMediaLinkDto>> GetAllActiveAsync(CancellationToken cancellationToken = default);
    Task<SocialMediaLinkDto?> GetByPlatformAsync(string platform, CancellationToken cancellationToken = default);
}