namespace PersonalSite.Domain.Interfaces.Repositories.Skills;

public interface ISkillCategoryRepository : IRepository<SkillCategory>
{
    Task<SkillCategory?> GetByKeyAsync(string key, CancellationToken cancellationToken = default);
    Task<List<SkillCategory>> GetAllOrderedAsync(CancellationToken cancellationToken = default);
}