namespace PersonalSite.Infrastructure.Persistence.Repositories.Translations;

public class SkillCategoryTranslationRepository : EfRepository<SkillCategoryTranslation>, ISkillCategoryTranslationRepository
{
    public SkillCategoryTranslationRepository(
        ApplicationDbContext context, 
        ILogger<SkillCategoryTranslationRepository> logger,
        IServiceProvider serviceProvider) 
        : base(context, logger, serviceProvider) { }


    public async Task<List<SkillCategoryTranslation>> GetBySkillCategoryIdAsync(Guid skillCategoryId, CancellationToken cancellationToken = default)
    {
        if (skillCategoryId == Guid.Empty)
            throw new ArgumentException("Skill category ID cannot be empty", nameof(skillCategoryId));
        
        return await DbContext.SkillCategoryTranslations
            .Where(t => t.SkillCategoryId == skillCategoryId)
            .Include(t => t.Language)
            .ToListAsync(cancellationToken);
    }
}