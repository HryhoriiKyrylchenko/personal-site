namespace PersonalSite.Domain.Interfaces.Repositories.Translations;

public interface ISkillCategoryTranslationRepository : IRepository<SkillCategoryTranslation>
{
    Task<SkillCategoryTranslation?> GetBySkillCategoryIdAndLanguageAsync(Guid skillCategoryId, string languageCode, CancellationToken cancellationToken = default);
}