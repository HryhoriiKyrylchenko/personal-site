namespace PersonalSite.Application.Features.Skills.LearningSkills.Queries.GetLearningSkills;

public record GetLearningSkillsQuery(
    Guid? SkillId = null,
    LearningStatus? Status = null
) : IRequest<Result<List<LearningSkillAdminDto>>>;
