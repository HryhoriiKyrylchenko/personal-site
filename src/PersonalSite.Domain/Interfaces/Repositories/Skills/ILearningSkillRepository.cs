namespace PersonalSite.Domain.Interfaces.Repositories.Skills;

public interface ILearningSkillRepository : IRepository<LearningSkill>
{
    Task<List<LearningSkill>> GetByStatusAsync(LearningStatus status, CancellationToken cancellationToken = default);
    Task<List<LearningSkill>> GetAllOrderedAsync(CancellationToken cancellationToken = default);
    Task<LearningSkill?> GetWithFullDataByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> ExistsBySkillIdAsync(Guid skillId, CancellationToken cancellationToken);
}