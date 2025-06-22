using PersonalSite.Domain.Entities.Translations;

namespace PersonalSite.Domain.Interfaces.Repositories.Translations;

public interface ISkillCategoryTranslationRepository : IRepository<SkillCategoryTranslation>
{
    Task<List<SkillCategoryTranslation>> GetBySkillCategoryIdAsync(Guid skillCategoryId, CancellationToken cancellationToken = default);
}