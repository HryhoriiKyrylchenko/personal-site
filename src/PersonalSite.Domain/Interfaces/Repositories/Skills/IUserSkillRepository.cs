using PersonalSite.Domain.Entities.Skills;

namespace PersonalSite.Domain.Interfaces.Repositories.Skills;

public interface IUserSkillRepository : IRepository<UserSkill>
{
    Task<List<UserSkill>> GetAllActiveAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsBySkillIdAsync(Guid requestSkillId, CancellationToken cancellationToken);
    Task<List<UserSkill>> GetFilteredAsync(
        Guid? skillId,
        short? minProficiency,
        short? maxProficiency,
        CancellationToken cancellationToken = default);
}