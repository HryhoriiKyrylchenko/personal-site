namespace PersonalSite.Infrastructure.Persistence.Repositories.Translations;

public class SkillTranslationRepository : EfRepository<SkillTranslation>, ISkillTranslationRepository
{
    public SkillTranslationRepository(
        ApplicationDbContext context, 
        ILogger<SkillTranslationRepository> logger,
        IServiceProvider serviceProvider) 
        : base(context, logger, serviceProvider) { }

    public async Task<SkillTranslation?> GetBySkillIdAndLanguageAsync(Guid skillId, string languageCode, CancellationToken cancellationToken = default)
    {
        if (skillId == Guid.Empty)
            throw new ArgumentException("Skill ID cannot be empty", nameof(skillId));
        
        if (string.IsNullOrWhiteSpace(languageCode))
            throw new ArgumentException("Language code cannot be null or whitespace", nameof(languageCode));
        
        return await DbContext.SkillTranslations
            .FirstOrDefaultAsync(st => st.SkillId == skillId && st.Language.Code == languageCode, cancellationToken);
    }

    public async Task<SkillTranslation?> GetWithLanguageByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(id));
        
        return await DbContext.SkillTranslations
            .Include(t => t.Language)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<SkillTranslation>> ListWithLanguageAsync(CancellationToken cancellationToken)
    {
        return await DbContext.SkillTranslations
            .Include(t => t.Language)
            .ToListAsync(cancellationToken); 
    }
}