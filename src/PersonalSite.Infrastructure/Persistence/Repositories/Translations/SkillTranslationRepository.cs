namespace PersonalSite.Infrastructure.Persistence.Repositories.Translations;

public class SkillTranslationRepository : EfRepository<SkillTranslation>, ISkillTranslationRepository
{
    public SkillTranslationRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<SkillTranslation?> GetBySkillIdAndLanguageAsync(Guid skillId, string languageCode, CancellationToken cancellationToken = default)
    {
        return await DbContext.SkillTranslations
            .FirstOrDefaultAsync(st => st.SkillId == skillId && st.Language.Code == languageCode, cancellationToken);
    }

    public async Task<SkillTranslation?> GetWithLanguageByIdAsync(Guid id, CancellationToken cancellationToken)
    {
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