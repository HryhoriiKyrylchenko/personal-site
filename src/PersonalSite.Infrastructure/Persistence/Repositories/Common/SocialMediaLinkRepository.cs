using PersonalSite.Domain.Entities.Common;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Infrastructure.Persistence.Repositories.Common;

public class SocialMediaLinkRepository : EfRepository<SocialMediaLink>, ISocialMediaLinkRepository
{
    public SocialMediaLinkRepository(
        ApplicationDbContext context, 
        ILogger<SocialMediaLinkRepository> logger,
        IServiceProvider serviceProvider) 
        : base(context, logger, serviceProvider) { }

    public async Task<List<SocialMediaLink>> GetAllActiveAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.SocialMediaLinks
            .Where(l => l.IsActive)
            .ToListAsync(cancellationToken);
    }
}