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

    public async Task<SocialMediaLink?> GetActiveByPlatformAsync(string platform, CancellationToken cancellationToken = default)
    {
        return await DbContext.SocialMediaLinks
            .FirstOrDefaultAsync(l => l.Platform == platform && l.IsActive, cancellationToken);
    }
}