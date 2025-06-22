using PersonalSite.Domain.Entities.Skills;

namespace PersonalSite.Domain.Interfaces.Repositories.Skills;

public interface ISkillCategoryRepository : IRepository<SkillCategory>
{
    Task<SkillCategory?> GetWithTranslationsByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByKeyAsync(string key, CancellationToken cancellationToken = default);
}