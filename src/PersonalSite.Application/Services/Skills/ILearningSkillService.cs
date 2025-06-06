namespace PersonalSite.Application.Services.Skills;

public interface ILearningSkillService : ICrudService<LearningSkillDto, LearningSkillAddRequest, LearningSkillUpdateRequest>
{
    Task<List<LearningSkillDto>> GetAllWithFullDataAsync(CancellationToken cancellationToken = default);
}