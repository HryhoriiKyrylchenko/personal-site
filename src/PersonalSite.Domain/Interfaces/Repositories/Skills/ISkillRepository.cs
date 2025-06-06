namespace PersonalSite.Domain.Interfaces.Repositories.Skills;

public interface ISkillRepository : IRepository<Skill>
{
    Task<Skill?> GetByKeyAsync(string key, CancellationToken cancellationToken = default);
    Task<List<Skill>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken = default);
}