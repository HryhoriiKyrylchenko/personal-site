namespace PersonalSite.Domain.Interfaces.Repositories.Common;

public interface ISocialMediaLinkRepository : IRepository<SocialMediaLink>
{
    Task<List<SocialMediaLink>> GetAllActiveAsync(CancellationToken cancellationToken = default);
    Task<SocialMediaLink?> GetActiveByPlatformAsync(string platform, CancellationToken cancellationToken = default);
}