using PersonalSite.Domain.Entities.Common;

namespace PersonalSite.Domain.Interfaces.Repositories.Common;

public interface ISocialMediaLinkRepository : IRepository<SocialMediaLink>
{
    Task<List<SocialMediaLink>> GetAllActiveAsync(CancellationToken cancellationToken = default);
    Task<PaginatedResult<SocialMediaLink>> GetFilteredAsync(
        string? platform,
        bool? isActive,
        CancellationToken cancellationToken = default);
}