using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

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
            .Include(s => s.Translations.Where(t => !t.Language.IsDeleted))
                .ThenInclude(t => t.Language)
            .Include(s => s.Category)
                .ThenInclude(c => c.Translations.Where(t => !t.Language.IsDeleted))
                    .ThenInclude(t => t.Language)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);   
    }

    public async Task<List<Skill>> GetFilteredAsync(Guid? categoryId, string? keyFilter, CancellationToken cancellationToken = default)
    {
        var query = DbContext.Skills.AsQueryable()
            .Include(s => s.Category)
                .ThenInclude(c => c.Translations.Where(t => !t.Language.IsDeleted))
                    .ThenInclude(t => t.Language)
            .Include(s => s.Translations.Where(t => !t.Language.IsDeleted))
                .ThenInclude(t => t.Language)
            .AsNoTracking();

        if (categoryId.HasValue)
            query = query.Where(s => s.CategoryId == categoryId.Value);

        if (!string.IsNullOrWhiteSpace(keyFilter))
            query = query.Where(s => s.Key.Contains(keyFilter));

        var entities = await query
            .OrderBy(s => s.Key)
            .ToListAsync(cancellationToken);
        
        return entities; 
    }
}
