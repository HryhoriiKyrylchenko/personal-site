namespace PersonalSite.Domain.Interfaces.Repositories.Translations;

public interface ISkillCategoryTranslationRepository : IRepository<SkillCategoryTranslation>
{
    Task<SkillCategoryTranslation?> GetBySkillCategoryIdAndLanguageAsync(Guid skillCategoryId, string languageCode, CancellationToken cancellationToken = default);
    Task<SkillCategoryTranslation?> GetWithLanguageByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<SkillCategoryTranslation>> ListWithLanguageAsync(CancellationToken cancellationToken);
}