using PersonalSite.Domain.Entities.Skills;

namespace PersonalSite.Domain.Interfaces.Repositories.Skills;

public interface IProjectSkillRepository : IRepository<ProjectSkill>
{
    Task<List<ProjectSkill>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default);
}