using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Infrastructure.Persistence.Repositories.Skills;

public class SkillCategoryRepository : EfRepository<SkillCategory>, ISkillCategoryRepository
{
    public SkillCategoryRepository(
        ApplicationDbContext context, 
        ILogger<SkillCategoryRepository> logger,
        IServiceProvider serviceProvider) 
        : base(context, logger, serviceProvider) { }

    public async Task<SkillCategory?> GetWithTranslationsByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(id));
        
        return await DbContext.SkillCategories
            .Include(sc => sc.Translations.Where(t => !t.Language.IsDeleted))
                .ThenInclude(t => t.Language)
            .AsSplitQuery()
            .FirstOrDefaultAsync(sc => sc.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("Key cannot be null or whitespace", nameof(key));
        }
        
        return await DbContext.SkillCategories.AnyAsync(sc => sc.Key == key, cancellationToken); 
    }

    public async Task<List<SkillCategory>> GetFilteredAsync(string? keyFilter, short? minDisplayOrder, short? maxDisplayOrder,
        CancellationToken cancellationToken = default)
    {
        var query = DbContext.SkillCategories.AsQueryable()
            .Include(sc => sc.Translations.Where(t => !t.Language.IsDeleted))
                .ThenInclude(t => t.Language)
            .AsSplitQuery()
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(keyFilter))
            query = query.Where(x => x.Key.Contains(keyFilter));

        if (minDisplayOrder.HasValue)
            query = query.Where(x => x.DisplayOrder >= minDisplayOrder.Value);

        if (maxDisplayOrder.HasValue)
            query = query.Where(x => x.DisplayOrder <= maxDisplayOrder.Value);

        var entities = await query.OrderBy(sc => sc.DisplayOrder).ToListAsync(cancellationToken);
        
        return entities;  
    }
}