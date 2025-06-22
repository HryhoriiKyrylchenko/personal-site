using PersonalSite.Domain.Common.Results;
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

    public async Task<PaginatedResult<SocialMediaLink>> GetFilteredAsync(string? platform, bool? isActive, CancellationToken cancellationToken = default)
    {
        var query = DbContext.SocialMediaLinks.AsQueryable().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(platform))
            query = query.Where(x => x.Platform.Contains(platform));

        if (isActive.HasValue)
            query = query.Where(x => x.IsActive == isActive.Value);

        var entities = await query
            .OrderBy(x => x.DisplayOrder)
            .ToListAsync(cancellationToken);
        
        return PaginatedResult<SocialMediaLink>.Success(entities, 1, 10, entities.Count);  
    }
}