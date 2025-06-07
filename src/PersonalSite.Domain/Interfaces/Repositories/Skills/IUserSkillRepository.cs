namespace PersonalSite.Domain.Interfaces.Repositories.Skills;

public interface IUserSkillRepository : IRepository<UserSkill>
{
    Task<List<UserSkill>> GetBySkillIdAsync(Guid skillId, CancellationToken cancellationToken = default);
    Task<List<UserSkill>> GetAllActiveAsync(CancellationToken cancellationToken = default);
    Task<UserSkill?> GetWithSkillDataById(Guid id, CancellationToken cancellationToken);
}