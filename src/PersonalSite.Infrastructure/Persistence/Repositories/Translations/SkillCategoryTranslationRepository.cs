namespace PersonalSite.Infrastructure.Persistence.Repositories.Translations;

public class SkillCategoryTranslationRepository : EfRepository<SkillCategoryTranslation>, ISkillCategoryTranslationRepository
{
    public SkillCategoryTranslationRepository(
        ApplicationDbContext context, 
        ILogger<SkillCategoryTranslationRepository> logger,
        IServiceProvider serviceProvider) 
        : base(context, logger, serviceProvider) { }

    public async Task<SkillCategoryTranslation?> GetBySkillCategoryIdAndLanguageAsync(Guid skillCategoryId, string languageCode, CancellationToken cancellationToken = default)
    {
        if (skillCategoryId == Guid.Empty)
            throw new ArgumentException("Skill category ID cannot be empty", nameof(skillCategoryId));
        
        return await DbContext.SkillCategoryTranslations
            .FirstOrDefaultAsync(sct => sct.SkillCategoryId == skillCategoryId && sct.Language.Code == languageCode, cancellationToken);
    }

    public async Task<SkillCategoryTranslation?> GetWithLanguageByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(id));
        
        return await DbContext.SkillCategoryTranslations
            .Include(t => t.Language)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);   
    }

    public async Task<IEnumerable<SkillCategoryTranslation>> ListWithLanguageAsync(CancellationToken cancellationToken)
    {
        return await DbContext.SkillCategoryTranslations
            .Include(t => t.Language)
            .ToListAsync(cancellationToken);  
    }
}