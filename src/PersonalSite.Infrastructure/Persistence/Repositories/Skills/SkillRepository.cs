namespace PersonalSite.Infrastructure.Persistence.Repositories.Skills;

public class SkillRepository : EfRepository<Skill>, ISkillRepository
{
    public SkillRepository(
        ApplicationDbContext context, 
        ILogger<SkillRepository> logger,
        IServiceProvider serviceProvider) 
        : base(context, logger, serviceProvider) { }

    public async Task<bool> ExistsByKeyAsync(string key, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Key cannot be null or whitespace", nameof(key));
        
        return await DbContext.Skills.AnyAsync(s => s.Key == key, cancellationToken);  
    }

    public async Task<Skill?> GetWithTranslationsById(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(id));
        
        return await DbContext.Skills
            .Include(s => s.Translations)
            .Include(s => s.Category)
                .ThenInclude(c => c.Translations)
                    .ThenInclude(t => t.Language)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);   
    }

    public async Task<List<Skill>> GetAllOrderedAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Skills
            .Include(s => s.Translations)
                .ThenInclude(t => t.Language)
            .Include(s => s.Category)
                .ThenInclude(c => c.Translations)
                    .ThenInclude(t => t.Language)
            .OrderBy(s => s.CategoryId)
            .ToListAsync(cancellationToken);
    }
}
