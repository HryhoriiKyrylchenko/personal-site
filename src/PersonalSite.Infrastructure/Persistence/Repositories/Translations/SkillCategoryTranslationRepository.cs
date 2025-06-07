namespace PersonalSite.Infrastructure.Persistence.Repositories.Translations;

public class SkillCategoryTranslationRepository : EfRepository<SkillCategoryTranslation>, ISkillCategoryTranslationRepository
{
    public SkillCategoryTranslationRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<SkillCategoryTranslation?> GetBySkillCategoryIdAndLanguageAsync(Guid skillCategoryId, string languageCode, CancellationToken cancellationToken = default)
    {
        return await DbContext.SkillCategoryTranslations
            .FirstOrDefaultAsync(sct => sct.SkillCategoryId == skillCategoryId && sct.Language.Code == languageCode, cancellationToken);
    }
}