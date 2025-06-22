using PersonalSite.Domain.Entities.Skills;

namespace PersonalSite.Domain.Interfaces.Repositories.Skills;

public interface ISkillRepository : IRepository<Skill>
{
    Task<bool> ExistsByKeyAsync(string requestKey, CancellationToken cancellationToken);
    Task<Skill?> GetWithTranslationsById(Guid id, CancellationToken cancellationToken = default);
    Task<List<Skill>> GetFilteredAsync(
        Guid? categoryId,
        string? keyFilter,
        CancellationToken cancellationToken = default);
}