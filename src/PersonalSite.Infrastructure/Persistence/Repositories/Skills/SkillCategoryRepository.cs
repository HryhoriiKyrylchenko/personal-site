namespace PersonalSite.Infrastructure.Persistence.Repositories.Skills;

public class SkillCategoryRepository : EfRepository<SkillCategory>, ISkillCategoryRepository
{
    public SkillCategoryRepository(
        ApplicationDbContext context, 
        ILogger<SkillCategoryRepository> logger,
        IServiceProvider serviceProvider) 
        : base(context, logger, serviceProvider) { }

    public async Task<SkillCategory?> GetByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Key cannot be null or whitespace", nameof(key));
        
        return await DbContext.SkillCategories
            .Include(sc => sc.Translations)
            .FirstOrDefaultAsync(sc => sc.Key == key, cancellationToken);
    }

    public async Task<List<SkillCategory>> GetAllOrderedAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.SkillCategories
            .Include(sc => sc.Translations)
            .OrderBy(sc => sc.DisplayOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task<SkillCategory?> GetWithTranslationsByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(id));
        
        return await DbContext.SkillCategories
            .Include(sc => sc.Translations)
            .FirstOrDefaultAsync(sc => sc.Id == id, cancellationToken);
    }
}