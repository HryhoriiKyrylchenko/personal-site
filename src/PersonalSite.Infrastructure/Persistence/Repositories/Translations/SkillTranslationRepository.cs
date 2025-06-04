namespace PersonalSite.Infrastructure.Persistence.Repositories.Translations;

public class SkillTranslationRepository : EfRepository<SkillTranslation>, ISkillTranslationRepository
{
    public SkillTranslationRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<SkillTranslation?> GetBySkillIdAndLanguageAsync(Guid skillId, string languageCode, CancellationToken cancellationToken = default)
    {
        return await DbContext.SkillTranslations
            .FirstOrDefaultAsync(st => st.SkillId == skillId && st.LanguageCode == languageCode, cancellationToken);
    }
}