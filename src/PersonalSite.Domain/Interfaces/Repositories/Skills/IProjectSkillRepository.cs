namespace PersonalSite.Domain.Interfaces.Repositories.Skills;

public interface IProjectSkillRepository : IRepository<ProjectSkill>
{
    Task<List<ProjectSkill>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default);
    Task<List<ProjectSkill>> GetBySkillIdAsync(Guid skillId, CancellationToken cancellationToken = default);
    Task<ProjectSkill?> GetWithSkillDataById(Guid id, CancellationToken cancellationToken = default);
    Task<List<ProjectSkill>> GetAllWithSkillData(CancellationToken cancellationToken = default);
}