namespace PersonalSite.Infrastructure.Persistence.Repositories.Skills;

public class SkillRepository : EfRepository<Skill>, ISkillRepository
{
    public SkillRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Skill?> GetByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        return await DbContext.Skills
            .Include(s => s.Translations)
            .Include(s => s.Category)
            .FirstOrDefaultAsync(s => s.Key == key, cancellationToken);
    }

    public async Task<List<Skill>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        return await DbContext.Skills
            .Where(s => s.CategoryId == categoryId)
            .Include(s => s.Translations)
            .ToListAsync(cancellationToken);
    }
}
