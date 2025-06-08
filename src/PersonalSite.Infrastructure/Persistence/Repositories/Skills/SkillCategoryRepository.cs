namespace PersonalSite.Infrastructure.Persistence.Repositories.Skills;

public class SkillCategoryRepository : EfRepository<SkillCategory>, ISkillCategoryRepository
{
    public SkillCategoryRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<SkillCategory?> GetByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
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
        return await DbContext.SkillCategories
            .Include(sc => sc.Translations)
            .FirstOrDefaultAsync(sc => sc.Id == id, cancellationToken);
    }
}