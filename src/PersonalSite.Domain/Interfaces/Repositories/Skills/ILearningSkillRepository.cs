using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Enums;

namespace PersonalSite.Domain.Interfaces.Repositories.Skills;

public interface ILearningSkillRepository : IRepository<LearningSkill>
{
    Task<List<LearningSkill>> GetAllOrderedAsync(CancellationToken cancellationToken = default);
    Task<LearningSkill?> GetWithFullDataByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> ExistsBySkillIdAsync(Guid skillId, CancellationToken cancellationToken);
    Task<List<LearningSkill>> GetFilteredAsync(
        LearningStatus? status,
        CancellationToken cancellationToken = default);
    Task<LearningSkill?> GetBySkillIdAsync(Guid skillId, CancellationToken cancellationToken);
}