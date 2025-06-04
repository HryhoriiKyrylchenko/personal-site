namespace PersonalSite.Domain.Repositories.Translations;

public interface ISkillCategoryTranslationRepository : IRepository<SkillCategoryTranslation>
{
    Task<SkillCategoryTranslation?> GetBySkillCategoryIdAndLanguageAsync(Guid skillCategoryId, string languageCode, CancellationToken cancellationToken = default);
}